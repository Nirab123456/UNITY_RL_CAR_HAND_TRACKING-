from peaceful_pie.unity_comms import UnityComms
import argparse
from gym import Env
from gym.spaces import Box, Discrete
import numpy as np

class UnityEnv(Env):
    def __init__(self, unity_comms):
        self.unity_comms = unity_comms
        self.action_space = Discrete(15)  # Scale the action range to -1.0 to 1.0
        self.observation_space = Box(low=0, high=255, shape=(2, 36, 41), dtype=np.uint8)

    def step(self, action):
        if action < 0 or action >= self.action_space.n:
            raise ValueError("Invalid action index")

        # Perform the action based on the provided action index
        # Modify the code to handle multiple cars based on their respective UnityComms instances
        for unity_comm in self.unity_comms:
            if action == 0:
                unity_comm.GoForward_8000()
            elif action == 1:
                unity_comm.GoReverse_8000()
            elif action == 2:
                unity_comm.TurnLeft_8000()
            elif action == 3:
                unity_comm.TurnRight_8000()
            elif action == 4:
                unity_comm.Handbrake_8000()
            elif action == 5:
                unity_comm.GoForward_8000()
                unity_comm.TurnLeft_8000()
            elif action == 6:
                unity_comm.GoForward_8000()
                unity_comm.TurnRight_8000()
            elif action == 7:
                unity_comm.GoForward_8000()
                unity_comm.Handbrake_8000()
            elif action == 8:
                unity_comm.GoReverse_8000()
                unity_comm.TurnLeft_8000()
            elif action == 9:
                unity_comm.GoReverse_8000()
                unity_comm.TurnRight_8000()
            elif action == 10:
                unity_comm.GoReverse_8000()
                unity_comm.Handbrake_8000()
            elif action == 11:
                unity_comm.GoForward_8000()
                unity_comm.TurnLeft_8000()
                unity_comm.Handbrake_8000()
            elif action == 12:
                unity_comm.GoForward_8000()
                unity_comm.TurnRight_8000()
                unity_comm.Handbrake_8000()
            elif action == 13:
                unity_comm.GoReverse_8000()
                unity_comm.TurnLeft_8000()
                unity_comm.Handbrake_8000()
            elif action == 14:
                unity_comm.GoReverse_8000()
                unity_comm.TurnRight_8000()
                unity_comm.Handbrake_8000()

        # TODO: Implement the observation retrieval logic
        observation = self.get_observation()

        # TODO: Implement the reward calculation logic
        reward = self.calculate_reward()

        # TODO: Implement the termination condition logic
        done = self.check_termination()

        # TODO: Implement the additional information dictionary
        info = {}

        return observation, reward, done, info

    def get_observation(self):
        # TODO: Implement the code to retrieve the observation from Unity
        observation = np.zeros((2, 36, 41), dtype=np.uint8)
        return observation

    def calculate_reward(self):
        # TODO: Implement the code to calculate the reward
        reward = 0.0
        return reward

    def check_termination(self):
        # TODO: Implement the code to check the termination condition
        done = False
        return done


# Parse command-line arguments
parser = argparse.ArgumentParser()
parser.add_argument('--ports', type=int, nargs='+', default=[8000])
args = parser.parse_args()

unity_comms = [UnityComms(port=port) for port in args.ports]
env = UnityEnv(unity_comms)

for _ in range(1000):
    action = np.random.randint(0, 15)
    env.step(action)
