/*
© 2025 Van Phan. All Rights Reserved.
Fish object to demonstrate optimizations for large masses of objects
and integration with ML-Agents.

Architecture: https://youtube.com/watch?v=32wtJZ3yRfw&list=PLX2vGYjWbI0R08eWQkO7nQkGiicHAX7IX&index=1&pp=iAQB
Documentation: https://docs.unity3d.com/Packages/com.unity.ml-agents@4.0/manual/Installation.html
*/

using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

namespace Monobehaviors.Fish
{
    /// <summary>
    /// Fish agent that moves a game object around the scene similar to a fish.
    /// Uses ML-Agents to learn optimized swarm behaviors.
    /// </summary>
    public class FishAgent : Agent
    {
        #region FIELDS
        [SerializeField] private FishSwarmParams data;

        private RayPerceptionSensor raySensor;

        private Vector3 targetPoint;

        private Vector3 velocity;
        #endregion

        #region MONOBEHAVIOR
        public override void Initialize()
        {
            if (FishManager.Instance == null) new FishManager(1);

            if (data == null)
            {
                Debug.LogError("FishAgent: 'data' is not assigned! Please assign a FishSwarmParams ScriptableObject.");
                data = ScriptableObject.CreateInstance<FishSwarmParams>(); // optional default
            }

            velocity = Vector3.forward * data.minSpeed; // safe default
            targetPoint = transform.position;           // safe default

            var rayComponent = GetComponent<RayPerceptionSensorComponent3D>();
            if (rayComponent != null)
            {
                raySensor = rayComponent.RaySensor;
            }
        }
        private void Start()
        {
            velocity = Random.insideUnitSphere.normalized * data.minSpeed;
        }
        #endregion

        #region AGENT
        /// <summary>
        /// Called at the beginning of a training episode.
        /// Resets position and velocity.
        /// </summary>
        public override void OnEpisodeBegin()
        {
            transform.position = Random.insideUnitSphere * 5f;
            velocity = Random.insideUnitSphere.normalized * data.minSpeed;

            // Assign a random goal within radius
            targetPoint = Random.insideUnitSphere * 5f;
        }

        /// <summary>
        /// Collects observations about the fish’s state for training.
        /// </summary>
        /// <param name="sensor">The vector sensor provided by ML-Agents.</param>
        public override void CollectObservations(VectorSensor sensor)
        {
            if (sensor == null)
            {
                Debug.LogError("VectorSensor is null in CollectObservations!");
                return;
            }

            if (velocity == null) velocity = Vector3.forward * 1f;
            if (targetPoint == null) targetPoint = transform.position;

            // Velocity (normalized, 3 floats)
            sensor.AddObservation(velocity.normalized);

            // Speed magnitude (1 float)
            sensor.AddObservation(velocity.magnitude);

            // Direction to target (3 floats)
            Vector3 dirToTarget = (targetPoint - transform.position).normalized;
            sensor.AddObservation(dirToTarget);

            // **Note Space size in behaviour parameters must stay 0 so ray perception space
            // is automatically allocated
        }

        /// <summary>
        /// Applies actions from the ML-Agents brain to update movement.
        /// </summary>
        /// <param name="actions">Action buffer from the model or heuristic.</param>
        public override void OnActionReceived(ActionBuffers actions)
        {
            if (!FishBounds.IsInside(transform.position))
            {
                AddReward(-1.0f);   // harsh penalty
                EndEpisode();       // reset agent
            }

            Vector3 steer = new Vector3(
            actions.ContinuousActions[0],
            actions.ContinuousActions[1],
            actions.ContinuousActions[2]);

            // Add manual obstacle avoidance influence
            steer += ComputeAvoidance() * data.maxSteerForce;

            velocity += steer * data.maxSteerForce * Time.deltaTime;
            velocity = Vector3.ClampMagnitude(velocity, data.maxSpeed);

            if (velocity.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(velocity.normalized, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, data.rotationSpeed * Time.deltaTime);
            }

            transform.position += transform.forward * velocity.magnitude * Time.deltaTime;

            // Reward for getting closer
            float distanceToTarget = Vector3.Distance(transform.position, targetPoint);
            if (distanceToTarget < 0.5f)
            {
                SetReward(1.0f); // reward for reaching
                EndEpisode();    // reset
            }
            else
            {
                // small step reward for reducing distance
                float stepReward = -distanceToTarget * 0.001f;
                AddReward(stepReward);
            }
        }

        /// <summary>
        /// Provides manual controls when no ML model is assigned.
        /// Useful for debugging.
        /// </summary>
        public override void Heuristic(in ActionBuffers actionsOut)
        {
            var continuousActions = actionsOut.ContinuousActions;
            continuousActions[0] = Input.GetAxis("Horizontal");
            continuousActions[1] = 0f;
            continuousActions[2] = Input.GetAxis("Vertical");
        }
        #endregion

        #region FISH
        private Vector3 ComputeAvoidance()
        {
            Vector3 avoidance = Vector3.zero;
            float rayDistance = 5f;
            Vector3[] rayDirections = {
                transform.forward,
                (transform.forward + transform.right).normalized,
                (transform.forward - transform.right).normalized,
                (transform.forward + transform.up).normalized,
                (transform.forward - transform.up).normalized
            };

            foreach (var dir in rayDirections)
            {
                if (Physics.Raycast(transform.position, dir, out RaycastHit hit, rayDistance))
                {
                    if (hit.collider.CompareTag("Obstacle"))
                    {
                        avoidance += (transform.position - hit.point).normalized * (1f - hit.distance / rayDistance);
                    }
                }
            }

            return avoidance.normalized;
        }

        #endregion
    }
}