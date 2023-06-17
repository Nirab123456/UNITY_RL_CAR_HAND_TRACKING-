from peaceful_pie.unity_comms import UnityComms
import argparse

def run(args: argparse.Namespace) -> int:
    unity_comms = UnityComms(port=args.port)
    collision_count = 0

    for i in range(2000):
        collision_count_regular = unity_comms.GetPlaneCollision()
        
        if collision_count_regular is not None:
            collision_count += collision_count_regular
            print(collision_count_regular)
        
        if collision_count_regular == 1:  # Check if collision is detected
            print("Collision detected")
            return 1  # Return 1 when collision is detected

        if collision_count >= 5:
            print("End of session")
            break

    return 0  # Return 0 when no collision is detected

if __name__ == '__main__':
    parser = argparse.ArgumentParser()
    parser.add_argument('--port', type=int, default=6000)
    args = parser.parse_args()
    result = run(args)
    print("Collision result:", result)
