from peaceful_pie.unity_comms import UnityComms
import argparse
import gym
from gym import Env
from gym.spaces import Box, MultiBinary,Discrete
import numpy as np
import os
from torch.utils.tensorboard import SummaryWriter
from stable_baselines3 import PPO
from stable_baselines3.common.callbacks import BaseCallback
import gym
from gym import spaces
from gym.wrappers import FrameStack
from collections import deque
# Import UnityComms from peaceful_pie.unity_comms
from peaceful_pie.unity_comms import UnityComms

parser = argparse.ArgumentParser()
parser.add_argument('--port', type=int, default=9000)
args = parser.parse_args()

unity_comms = UnityComms(port=args.port)



# Define the frame skip frequency
frame_skip_frequency = 5

# Define the number of training steps
total_timesteps = 100000

# Define the directory paths
LOG_DIR = './logs/'
OPT_DIR = './opt_modeldata/'
CHECKPOINT_DIR = './train_modeldata/'

# Create the directories if they don't exist
os.makedirs(LOG_DIR, exist_ok=True)
os.makedirs(OPT_DIR, exist_ok=True)
os.makedirs(CHECKPOINT_DIR, exist_ok=True)

class MyVector3:
    def __init__(self, x, y, z):
        self.x = x
        self.y = y
        self.z = z

class UnityEnv(Env):
    def __init__(self, unity_comms):
        self.unity_comms = unity_comms
        self.action_space = Discrete(15)  # Scale the action range to -1.0 to 1.0
        self.observation_space = spaces.Box(low=-np.inf, high=np.inf, shape=(3,), dtype=np.float32)
        self.initial_position = None
        self.prev_position = None
        self.prev_velocity = None

    def step(self, action):
        # Perform the action based on the provided action index
        if action == 0:
            self.unity_comms.GoForward()
        elif action == 1:
            self.unity_comms.GoReverse()
        elif action == 2:
            self.unity_comms.TurnLeft()
        elif action == 3:
            self.unity_comms.TurnRight()
        elif action == 4:
            self.unity_comms.Handbrake()
        elif action == 5:
            self.unity_comms.GoForward()
            self.unity_comms.TurnLeft()
        elif action == 6:
            self.unity_comms.GoForward()
            self.unity_comms.TurnRight()
        elif action == 7:
            self.unity_comms.GoForward()
            self.unity_comms.Handbrake()
        elif action == 8:
            self.unity_comms.GoReverse()
            self.unity_comms.TurnLeft()
        elif action == 9:
            self.unity_comms.GoReverse()
            self.unity_comms.TurnRight()
        elif action == 10:
            self.unity_comms.GoReverse()
            self.unity_comms.Handbrake()
        elif action == 11:
            self.unity_comms.GoForward()
            self.unity_comms.TurnLeft()
            self.unity_comms.Handbrake()
        elif action == 12:
            self.unity_comms.GoForward()
            self.unity_comms.TurnRight()
            self.unity_comms.Handbrake()
        elif action == 13:
            self.unity_comms.GoReverse()
            self.unity_comms.TurnLeft()
            self.unity_comms.Handbrake()
        elif action == 14:
            self.unity_comms.GoReverse()
            self.unity_comms.TurnRight()
            self.unity_comms.Handbrake()
        else:
            raise ValueError("Invalid action index")
        


        self.prev_position = self.get_position()
        self.prev_velocity = self.get_velocity()

        position = self.get_position()
        velocity = self.get_velocity()

        # Get the done flag
        done = self.get_done()
        reward = self.get_reward(position, velocity)

        # Concatenate the position and velocity to form the observation
        observation = np.array([position[0], position[1], velocity], dtype=np.float32)

        # Return the observation, reward, done flag, and additional info
        return observation, reward, done, {}

    def reset(self):
        # Reset the environment to the initial state
        self.unity_comms.ResetPosition()
        self.initial_position = self.get_position()
        return self.initial_position

    def render(self):
        pass

    def get_reward(self, position, velocity):
        # Calculate the distance between the initial position and final position
        distance = np.linalg.norm(np.array(position) - np.array(self.initial_position))

        # Calculate the reward based on distance and speed
        reward = 0.1 * distance + 0.01 * velocity

        # Check for collisions and apply punishment
        if self.unity_comms.CheckCollision() > 0:
            reward -= 1.0

        return reward

    def get_position(self):
        position = self.unity_comms.GetPosition()
        # Extract x, y, and z components from position
        x = position['x']
        y = position['y']
        z = position['z']
        return x, y, z

    def get_velocity(self):
        velocity = self.unity_comms.CarSpeedUI()  # Get the velocity from the Unity environment
        return velocity

    def get_done(self):
        collision_count_regular = self.unity_comms.CheckCollision()

        if not hasattr(self, 'collision_count'):
            self.collision_count = 0

        self.collision_count += collision_count_regular

        if self.collision_count >= 2 or self.check_stuck():
            self.collision_count = 0
            return True
        else:
            return False


    def check_stuck(self):
        position_threshold = .01  # Adjust this value based on your environment
        consecutive_steps = 20  # Number of consecutive steps to consider for being stuck

        position_counter = 0

        for _ in range(consecutive_steps):
            x, y, z = self.get_position()

            position_diff = np.linalg.norm(np.array([x, y, z]) - np.array(self.prev_position))
            if position_diff < position_threshold:
                position_counter += 1
            else:
                position_counter = 0

            self.prev_position = [x, y, z]

        if position_counter >= consecutive_steps:
            return True
        else:
            return False


# Import optuna for HPO
import optuna
# Import PPO for algos
from stable_baselines3 import PPO
# Evaluate Policy
from stable_baselines3.common.evaluation import evaluate_policy
# Import wrappers
from stable_baselines3.common.monitor import Monitor
from stable_baselines3.common.vec_env import DummyVecEnv, VecFrameStack
import os
LOG_DIR = './logs/'
if not os.path.exists(LOG_DIR):
    os.makedirs(LOG_DIR)

OPT_DIR = './opt_modeldata/'
if not os.path.exists(OPT_DIR):
    os.makedirs(OPT_DIR)


# #https://github.com/araffin/rl-baselines-zoo/issues/29
def optimize_ppo(trial):
    """ Learning hyperparamters we want to optimise"""
    return {
        'n_steps': trial.suggest_int('n_steps', 2048, 8192),
        'gamma': trial.suggest_loguniform('gamma', 0.8, 0.9999),
        'learning_rate': trial.suggest_loguniform('learning_rate', 1e-5, 1e-4),
        'clip_range': trial.suggest_uniform('clip_range', 0.1, 0.4),
        'gae_lambda': trial.suggest_uniform('gae_lambda', 0.8, .99)
    }


def optimize_agent(trial):
    try:
        model_params = optimize_ppo(trial)
        env = UnityEnv(unity_comms)
        env = Monitor(env, LOG_DIR)
        env = DummyVecEnv([lambda: env])
        env = VecFrameStack(env, 4, channels_order='last')
        model = PPO('MlpPolicy', env,batch_size=4096, tensorboard_log=LOG_DIR, verbose=0, **model_params)
        model.learn(total_timesteps=10000)
        mean_reward, _ = evaluate_policy(model, env, n_eval_episodes=5)
        env.close()
        SAVE_PATH = os.path.join(OPT_DIR, 'trial_{}_best_model'.format(trial.number))
        model.save(SAVE_PATH)
        return mean_reward
    except Exception as e: 
        return -1000

study = optuna.create_study(direction='maximize')
study.optimize(optimize_agent, n_trials=5, n_jobs=1)
model_params = study.best_params

# Import os for file path management
import os 
# Import Base Callback for saving models
from stable_baselines3.common.callbacks import BaseCallback


class TrainAndLoggingCallback(BaseCallback):

    def __init__(self, check_freq, save_path, verbose=1):
        super(TrainAndLoggingCallback, self).__init__(verbose)
        self.check_freq = check_freq
        self.save_path = save_path

    def _init_callback(self):
        if self.save_path is not None:
            os.makedirs(self.save_path, exist_ok=True)

    def _on_step(self):
        if self.n_calls % self.check_freq == 0:
            model_path = os.path.join(self.save_path, 'best_model_{}'.format(self.n_calls))
            self.model.save(model_path)

        return True
    
CHECKPOINT_DIR = './train_modeldata/'
if not os.path.exists(CHECKPOINT_DIR):
    os.makedirs(CHECKPOINT_DIR)

callback = TrainAndLoggingCallback(check_freq=10000, save_path=CHECKPOINT_DIR)

env = UnityEnv(unity_comms)
env = Monitor(env, LOG_DIR)
env = DummyVecEnv([lambda: env])
env = VecFrameStack(env, 4, channels_order='last')





model = PPO('MlpPolicy', env,batch_size=4096, tensorboard_log=LOG_DIR, verbose=1, **model_params)

model.learn(total_timesteps=total_timesteps, callback=callback)






# class SaveModelCallback(BaseCallback):
#     def __init__(self, save_path, save_freq):
#         super(SaveModelCallback, self).__init__()
#         self.save_path = save_path
#         self.save_freq = save_freq

#     def _init_callback(self):
#         os.makedirs(self.save_path, exist_ok=True)

#     def _on_step(self) -> bool:
#         if self.n_calls % self.save_freq == 0:
#             self.model.save(os.path.join(self.save_path, f"model_step_{self.num_timesteps}.zip"))
#         return True



# def run(args: argparse.Namespace) -> None:
#     unity_comms = UnityComms(port=args.port)
#     env = UnityEnv(unity_comms)

#     # Define the model and hyperparameters
#     model = PPO("MlpPolicy", env, verbose=1)
#     callback = SaveModelCallback(save_path="./models", save_freq=1000)

#     # Train the model
#     model.learn(total_timesteps=10000, callback=callback)

#     # Save the final model
#     model.save(os.path.join("./models", "final_model.zip"))

#     # Test the trained model
#     obs = env.reset()
#     for _ in range(100):
#         action, _ = model.predict(obs, deterministic=True)
#         obs, reward, done, _ = env.step(action)
#         if done:
#             obs = env.reset()

#     unity_comms.Close()

# if __name__ == '__main__':
#     parser = argparse.ArgumentParser()
#     parser.add_argument('--port', type=int, default=9000)
#     args = parser.parse_args()
#     run(args)
