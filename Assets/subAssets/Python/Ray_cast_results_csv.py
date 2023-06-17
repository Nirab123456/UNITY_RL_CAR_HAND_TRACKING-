from peaceful_pie.unity_comms import UnityComms
from peaceful_pie import ray_results_helper
import numpy as np
import os 
def save_ray_results(port: int, i: int):
    unity_comms = UnityComms(port=port)
    method_name = f"GetRayCastsResults_{i}"
    ray_results = getattr(unity_comms, method_name)()

    print("Ray Results:")
    print(ray_results)

    distance = np.array(ray_results['rayDistances'], dtype=np.float32)
    types = np.array(ray_results['rayHitObjectTypes'], dtype=np.int32)
    num_types = ray_results['NumObjectTypes']

    expected_results = np.full((70, 20), 0.0, dtype=np.float32)

    actual_results = ray_results_helper.ray_results_to_feature_np(
        ray_results_helper.RayResults(
            NumObjectTypes=num_types,
            rayDistances=distance,
            rayHitObjectTypes=types,
        )
    )

    print("Actual Results:")
    print(actual_results)

    # Set element 0 as "Wall" and element 1 as "reward" in actual_results
    actual_results[actual_results == 0] = 0.0
    actual_results[actual_results == 1] = 1.0

    # Reshape actual_results to be 2D
    actual_results_2d = actual_results.reshape((actual_results.shape[0], -1))

    # Write the expected results to a file





    dir = f'D:/results/results{i}'
    if not os.path.exists(dir):
        os.makedirs(dir)



    with open(dir +"expected_results.txt", "w") as file:
        np.savetxt(file, expected_results)

    # Write the actual results to a file
    with open(dir +"actual_results.csv", "w") as file:
        np.savetxt(file, actual_results_2d, delimiter=",")

    # Write the ray distances to a file
    with open(dir +"ray_distances.txt", "w") as file:
        np.savetxt(file, distance)

    # Write the ray hit object types to a file
    with open(dir +"ray_hit_object_types.txt", "w") as file:
        np.savetxt(file, types)

    print("Results written to files: expected_results.txt, actual_results.csv, ray_distances.txt, ray_hit_object_types.txt")
    return distance, types, num_types, expected_results, actual_results
