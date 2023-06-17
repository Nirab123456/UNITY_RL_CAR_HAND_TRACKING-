from peaceful_pie.unity_comms import UnityComms
from peaceful_pie import ray_results_helper
import argparse


def unity_comms(port:int):
    parser = argparse.ArgumentParser()
    parser.add_argument("--port", type=int, default=port)
    args = parser.parse_args()
    unity_comms = UnityComms(args.port)
    return unity_comms

