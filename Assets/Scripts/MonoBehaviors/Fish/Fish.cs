/*
© 2025 Van Phan. All Rights Reserved.
Fish object to demonstrate optimizations for large masses of objects
and integration with ML-Agents.
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

        private Vector3 velocity;
        #endregion

        #region MONOBEHAVIOR
        public override void Initialize()
        {
            if (FishManager.Instance == null) new FishManager(1);
        }
        private void Start()
        {
            velocity = Random.insideUnitSphere.normalized * data.minSpeed;
        }
        #endregion

        #region AGENT_METHODS
        /// <summary>
        /// Called at the beginning of a training episode.
        /// Resets position and velocity.
        /// </summary>
        public override void OnEpisodeBegin()
        {
            transform.position = Random.insideUnitSphere * 5f;
            velocity = Random.insideUnitSphere.normalized * data.minSpeed;
        }

        /// <summary>
        /// Collects observations about the fish’s state for training.
        /// </summary>
        /// <param name="sensor">The vector sensor provided by ML-Agents.</param>
        public override void CollectObservations(VectorSensor sensor)
        {
            sensor.AddObservation(velocity.normalized);
            sensor.AddObservation(velocity.magnitude);
        }

        /// <summary>
        /// Applies actions from the ML-Agents brain to update movement.
        /// </summary>
        /// <param name="actions">Action buffer from the model or heuristic.</param>
        public override void OnActionReceived(ActionBuffers actions)
        {
            Vector3 steer = new Vector3(
                actions.ContinuousActions[0],
                actions.ContinuousActions[1],
                actions.ContinuousActions[2]);

            velocity += steer * data.maxSteerForce * Time.deltaTime;
            velocity = Vector3.ClampMagnitude(velocity, data.maxSpeed);

            transform.position += velocity * Time.deltaTime;
            transform.forward = velocity.normalized;
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
    }
}