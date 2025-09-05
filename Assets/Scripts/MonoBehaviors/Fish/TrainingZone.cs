/*
© 2025 Van Phan. All Rights Reserved.
Training zone that provides positive or negative feedback to fish agents.
*/

using UnityEngine;

namespace Monobehaviors.Fish
{
    /// <summary>
    /// Zone that provides feedback (reward or punishment) to fish agents when they enter.
    /// Includes slider controls for tuning value and limiting trigger count.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class TrainingZone : MonoBehaviour
    {
        #region FIELDS
        [Tooltip("Positive = reward, Negative = punishment")]
        [Range(-1f, 1f)]
        [SerializeField] private float feedbackValue = -0.1f;

        [Tooltip("Maximum number of times this zone can trigger before becoming inactive. Set to 0 for infinite.")]
        [SerializeField] private int maxTriggers = 0;

        private int currentTriggers = 0;
        #endregion

        #region MONOBEHAVIOR
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<FishAgent>(out var agent)) return;

            // Check trigger limit
            if (maxTriggers > 0 && currentTriggers >= maxTriggers) return;

            // Apply feedback
            agent.AddReward(feedbackValue);
            currentTriggers++;
        }
        #endregion
    }
}