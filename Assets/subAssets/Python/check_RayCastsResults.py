from peaceful_pie.unity_comms import UnityComms
from peaceful_pie import ray_results_helper
import numpy as np

def save_ray_results(port: int):
    unity_comms = UnityComms(port=port)
    ray_results = unity_comms.GetRayCastsResults()

    distance = np.array(ray_results['rayDistances'], dtype=np.float32)
    types = np.array(ray_results['rayHitObjectTypes'], dtype=np.int32)
    num_types = ray_results['NumObjectTypes']

    expected_results = np.full((41, 36), 0.0, dtype=np.float32)

    actual_results = ray_results_helper.ray_results_to_feature_np(
        ray_results_helper.RayResults(
            NumObjectTypes=num_types,
            rayDistances=distance,
            rayHitObjectTypes=types,
        )
    )

    # Set element 0 as "Wall" and element 1 as "reward" in actual_results
    actual_results[actual_results == 0] = 0.0
    actual_results[actual_results == 1] = 1.0

    return distance, types, num_types, expected_results, actual_results  

