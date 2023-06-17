from peaceful_pie.unity_comms import UnityComms
import argparse

def run(args: argparse.Namespace) -> None:
    unity_comms = UnityComms(port=args.port)
    collision_count = 0

    for i in range(2000):
        collision_count_regular = unity_comms.GetPlaneCollision_5000()
        collision_count += collision_count_regular
        print(collision_count_regular)
        if collision_count >= 1:
            print("5 times collided End of session")
            break

if __name__ == '__main__':
    parser = argparse.ArgumentParser()
    parser.add_argument('--port', type=int, default=5000)
    args = parser.parse_args()
    run(args)
