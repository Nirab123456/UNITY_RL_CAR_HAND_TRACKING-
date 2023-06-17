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
import time 
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
        self.stuck_count=None 

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
        self.unity_comms.ResetPosition_Plane()
        self.initial_position = self.get_position()
        return self.initial_position
    
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
        
    def get_reward(self, position, velocity):
        # Calculate the distance between the initial position and final position
        distance = np.linalg.norm(np.array(position) - np.array(self.initial_position))

        # Calculate the reward based on distance and speed
        reward = 0.1 * distance + 0.01 * velocity
        reward = reward - self.stuck_count
        reward = reward + self.GetRewardCollision

        # Check for collisions and apply punishment
        if self.unity_comms.MovingPlaneCollisionDetected() > 0:
            reward -= 1.0

        return reward

    def get_done(self):
        if  self.MovingPlaneCollisionDetected() or self.check_stuck() or self.GetPlaneCollision():
            return True
        else:
            False

    def GetRewardCollision(self):
        self.Get_Reward_Collision_regular = self.unity_comms.GetRewardCollision()

        self.Get_Reward_Collision = self.Get_Reward_Collision_regular
        return self.Get_Reward_Collision

            
    def MovingPlaneCollisionDetected(self):
        self.Moving_Plane_Collision_Detected = self.unity_comms.MovingPlaneCollisionDetected()

        if not hasattr (self,'MovingPlaneCollisionDetected'):
            self.Moving_Plane_Collision_Detected = self.Moving_Plane_Collision_Detected

            if self.Moving_Plane_Collision_Detected >= 2:
                self.Moving_Plane_Collision_Detected=0
                return True
            else:
                return False
            

            
    def GetPlaneCollision(self):
        self.Get_Plane_Collision_regular = self.unity_comms.GetPlaneCollision()

        if not hasattr (self,'GetPlaneCollision'):
            self.Get_Plane_Collision = self.Get_Plane_Collision_regular

            if self.Get_Plane_Collision >= 1:
                self.Get_Plane_Collision=0
                return True
            else:
                return False

    def check_stuck(self):
        position_threshold = 0.01  # Adjust this value based on your environment
        consecutive_steps = 20  # Number of consecutive steps to consider for being stuck

        position_counter = 0

        for _ in range(consecutive_steps):
            x, y, z = self.get_position()

            position_diff = np.linalg.norm(np.array([x, y, z]) - np.array(self.prev_position))
            if position_diff < position_threshold:
                position_counter += 1
                self.stuck_count=position_counter
                if position_counter >= 3:  # If position_counter reaches 3, consider the car stuck
                    return True
            else:
                position_counter = 0

            self.prev_position = [x, y, z]

        return False
    
    def render(self):
        pass

parser = argparse.ArgumentParser()
parser.add_argument('--port', type=int, default=9000)
args = parser.parse_args()

unity_comms = UnityComms(port=args.port)


env = UnityEnv(unity_comms)
obs=env.reset()
done = False
while not done:
    action = np.random.randint(0, 15)
    obs, reward, done, info = env.step(action)
    print(obs, reward, done, info)
    env.render()
    time.sleep(0.1)