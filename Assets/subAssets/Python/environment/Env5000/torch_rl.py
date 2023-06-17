from peaceful_pie.unity_comms import UnityComms
import argparse
import gym
from gym import Env
from gym.spaces import Box, MultiBinary,Discrete
from torchrl.envs.utils import check_env_specs, ExplorationType, set_exploration_type
import numpy as np
import os
from torch.utils.tensorboard import SummaryWriter
import gym
from gym import spaces
from gym.wrappers import FrameStack
from collections import deque
# Import UnityComms from peaceful_pie.unity_comms
from peaceful_pie.unity_comms import UnityComms
from peaceful_pie.unity_comms import UnityComms
from peaceful_pie import ray_results_helper
import pandas as pd
import matplotlib.pyplot as plt
import os
import uuid

import torch
from torch import nn
from torchrl.collectors import MultiaSyncDataCollector
from torchrl.data import LazyMemmapStorage, MultiStep, TensorDictReplayBuffer
from torchrl.envs import (
    EnvCreator,
    ExplorationType,
    ParallelEnv,
    RewardScaling,
    StepCounter,
)
from torchrl.envs.libs.gym import GymEnv
from torchrl.envs.transforms import (
    CatFrames,
    Compose,
    GrayScale,
    ObservationNorm,
    Resize,
    ToTensorImage,
    TransformedEnv,
)
from torchrl.modules import DuelingCnnDQNet, EGreedyWrapper, QValueActor

from torchrl.objectives import DQNLoss, SoftUpdate
from torchrl.record.loggers.csv import CSVLogger
from torchrl.trainers import (
    LogReward,
    Recorder,
    ReplayBufferTrainer,
    Trainer,
    UpdateWeights,
)


def unity_comms(port: int):
    unity_comms = UnityComms(port)
    return unity_comms



port = 5000  # Replace with your desired port number
unity_comms_instance = unity_comms(port)


unity_comms_instance 
unity_comms = unity_comms_instance


class MyVector3:
    def __init__(self, x, y, z):
        self.x = x
        self.y = y
        self.z = z


class UnityEnv(GymEnv):
    def __init__(self, unity_comms, i):
        self.unity_comms = unity_comms
        self.action_space = Discrete(15)  # Scale the action range to -1.0 to 1.0
        self.observation_space = spaces.Box(low=-np.inf, high=np.inf, shape=(3,), dtype=np.float32)
        self.initial_position = None
        self.prev_position = None
        self.prev_velocity = 0
        self.frame_count=0
        self.i = i

    def RayCast(self):
        ray_results = getattr(self.unity_comms, f"GetRayCastsResults_{self.i}")()
        distance = np.array(ray_results['rayDistances'], dtype=np.float32)
        types = np.array(ray_results['rayHitObjectTypes'], dtype=np.int32)
        num_types = ray_results['NumObjectTypes']
        self.actual_result = ray_results_helper.ray_results_to_feature_np(
            ray_results_helper.RayResults(
                NumObjectTypes=num_types,
                rayDistances=distance,
                rayHitObjectTypes=types,
            )
        )  

        return self.actual_result
    

    def Is_obstacle_visible(self):
        actual_result=self.RayCast()
        obstacle_channel = actual_result[0]  # Extract the obstacle channel from the actual result
        if np.max(obstacle_channel) > 0:
            return True
        return False

    def Is_reward_visible(self):
        actual_result=self.RayCast()
        reward_channel = actual_result[1]  # Extract the reward channel from the actual result
        if np.max(reward_channel) > 0:
            return True
        return False

    def Goal(self):
        if self.Is_reward_visible() == True:
            goal_channel = self.actual_result[1]
            if np.max(goal_channel) > 1:
                return True
        return False


    def Get_reward(self):
        obstacle_penalty = -1.0  # Penalty for encountering an obstacle
        reward_bonus = .7 # Reward for finding a reward object
        goal_reward = 1  # Reward for reaching the goal
        car_collision_penalty = -1.0  # Penalty for colliding with the car
        valocity_reward = 0.1 #reward for moving forward

        reward = 0.0

        if self.Is_obstacle_visible()== True:
            reward += obstacle_penalty

        if self.Is_reward_visible()== True:
            reward += reward_bonus

        reward += self.Step_panalty()

        if self.Goal() == True:
            reward += goal_reward

        if self.Get_carCollisionDetected() == True:
            reward += car_collision_penalty

        if self.Check_valocity_increment()== True:
            reward += valocity_reward

        return reward

    def reset(self):
        self.RayCast()
        Reset_Car = getattr(self.unity_comms, f"ResetPosition_{self.i}")()  
        Reset_Agent = getattr(self.unity_comms, f"ResetPosition_Plane_{self.i}")()
        position =self.Get_position()
        position = np.array(position, dtype=np.float32)
        self.frame_count=0
        return position

    def done(self):
        if self.Get_carCollisionDetected() == True:
            self.frame_count=0
            return True
        elif self.Get_movingPlaneCollision() == True:
            self.frame_count=0
            return True
        elif self.Get_planeCollision() == True:
            self.frame_count=0
            return True
        elif self.Check_stuck() == True:
            self.frame_count=0
            return True
        else:
            self.frame_count += 1
            return False

    def Get_carCollisionDetected(self):
        collision_count = 0
        collision = collision = getattr(self.unity_comms, f"CarCollisionDetected_{self.i}")()
        if collision == 1:
            collision_count += 1
            if collision_count >=2:
                collision_count = 0
                return True

        return False
    
    def Get_movingPlaneCollision(self):
        collision_count = 0
        collision = getattr(self.unity_comms, f"GetMovingPlaneCollision_{self.i}")()
        if collision == 1:
            collision_count += 1
            if collision_count >= 1:
                collision_count = 0
                return True

        return False
    
    def Get_planeCollision(self):
        collision_count = 0
        collision = getattr(self.unity_comms, f"GetPlaneCollision_{self.i}")()
        if collision == 1:
            collision_count += 1
            if collision_count > 0:
                collision_count = 0
                return True

        return False
    
    
    def Get_rewardCollision(self):
        collision = getattr(self.unity_comms, f"GetRewardCollision_{self.i}")()
        self.rewardcollision = collision

    def Step_panalty(self):
        if self.done() == True:
            return -1
        else:
            return 0

    def step(self, action):
        # Perform the action based on the provided action index
        if action == 0:
            getattr(self.unity_comms, f"GoForward_{self.i}")()
        elif action == 1:
            getattr(self.unity_comms, f"GoReverse_{self.i}")()
        elif action == 2:
            getattr(self.unity_comms, f"TurnLeft_{self.i}")()
        elif action == 3:
            getattr(self.unity_comms, f"TurnRight_{self.i}")()
        elif action == 4:
            getattr(self.unity_comms, f"Handbrake_{self.i}")()
        elif action == 5:
            getattr(self.unity_comms, f"GoForward_{self.i}")()
            getattr(self.unity_comms, f"TurnLeft_{self.i}")()
        elif action == 6:
            getattr(self.unity_comms, f"GoForward_{self.i}")()
            getattr(self.unity_comms, f"TurnRight_{self.i}")()
        elif action == 7:
            getattr(self.unity_comms, f"GoForward_{self.i}")()
            getattr(self.unity_comms, f"Handbrake_{self.i}")()
        elif action == 8:
            getattr(self.unity_comms, f"GoReverse_{self.i}")()
            getattr(self.unity_comms, f"TurnLeft_{self.i}")()
        elif action == 9:
            getattr(self.unity_comms, f"GoReverse_{self.i}")()
            getattr(self.unity_comms, f"TurnRight_{self.i}")()
        elif action == 10:
            getattr(self.unity_comms, f"GoReverse_{self.i}")()
            getattr(self.unity_comms, f"Handbrake_{self.i}")()
        elif action == 11:
            getattr(self.unity_comms, f"GoForward_{self.i}")()
            getattr(self.unity_comms, f"TurnLeft_{self.i}")()
            getattr(self.unity_comms, f"Handbrake_{self.i}")()
        elif action == 12:
            getattr(self.unity_comms, f"GoForward_{self.i}")()
            getattr(self.unity_comms, f"TurnRight_{self.i}")()
            getattr(self.unity_comms, f"Handbrake_{self.i}")()
        elif action == 13:
            getattr(self.unity_comms, f"GoReverse_{self.i}")()
            getattr(self.unity_comms, f"TurnLeft_{self.i}")()
            getattr(self.unity_comms, f"Handbrake_{self.i}")()
        elif action == 14:
            getattr(self.unity_comms, f"GoReverse_{self.i}")()
            getattr(self.unity_comms, f"TurnRight_{self.i}")()
            getattr(self.unity_comms, f"Handbrake_{self.i}")()
        else:
            raise ValueError("Invalid action index")

        self.prev_position = self.Get_position()
        self.prev_velocity = self.Get_velocity()

        position = self.Get_position()
        velocity = self.Get_velocity()
        # Concatenate the position and velocity to form the observation
        observation = np.array([position[0], position[1], velocity], dtype=np.float32)

        reward = self.Get_reward()
        done = self.done()
    # Update the info dictionary with relevant information
        info = {
            'episode_reward': reward,  # Replace with the actual episode reward
            'episode_length': self.frame_count,  # Replace with the actual episode length
            'current_observation': observation,
            'action_taken': action,
            'obstacle_visible': self.Is_obstacle_visible(),
            'reward_visible': self.Is_reward_visible(),
            'goal_reached': self.Goal(),
            'car_collision_detected': self.Get_carCollisionDetected(),
            'velocity_incremented': self.Check_valocity_increment(),
        }
        return observation, reward, done, info


    def Get_velocity(self):
        valocity =  getattr(self.unity_comms, f"CarSpeedUI_{self.i}")()
        return valocity

    def Check_valocity_increment(self):
        valocity = self.Get_velocity()
        if valocity > self.prev_velocity:
            self.prev_velocity = valocity
            return True
        else:
            return False

    def Check_stuck(self):
        position_threshold = 0.01
        consecutive_steps = 20
        position_counter = 0
        
        if self.prev_position is None:
            self.prev_position = self.get_position()

        for _ in range(consecutive_steps):
            x, y, z = self.Get_position()

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


    def Get_position(self):
        position = getattr(self.unity_comms, f"GetPosition_{self.i}")()
        # Extract x, y, and z components from position
        x = position['x']
        y = position['y']
        z = position['z']
        return x, y, z


env = UnityEnv(unity_comms,5000)
check = check_env_specs(env)
print(check)
