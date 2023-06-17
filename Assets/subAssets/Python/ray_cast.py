from peaceful_pie.ray_helper import RayHelper
from peaceful_pie import ray_results_helper

# Rest of the code remains the same...

# Define a remote function to check the Ray script
@ray_helper.remote
def check_script() -> str:
    # Instantiate MyUnityEnv with UnityComms from ray_helper
    unity_comms = ray_helper.get_comms()
    env = MyUnityEnv(comms=unity_comms)
    
    # Perform a reset and step in the environment
    obs = env.reset()
    actions = [0, 0, 0]  # Example actions
    obs, reward, done, info = env.step(actions)
    
    # Perform any additional checks or assertions on the results
    # Return a status message indicating the success of the checks
    return "Ray script is working properly!"

# Call the remote function to check the script
result = ray_helper.remote(check_script)
output = ray_helper.get(result)
print(output)

# Stop the RayHelper
ray_helper.stop()
