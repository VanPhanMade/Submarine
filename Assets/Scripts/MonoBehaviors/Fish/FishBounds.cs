/*
© 2025 Van Phan. All Rights Reserved.
Bounds object that punishes ML-Agents from going outside this bound aggressively
*/

using UnityEngine;

namespace Monobehaviors.Fish
{
    /// <summary>
    /// Defines a global 3D bounding volume that all fish agents must stay inside.
    /// Throws an exception if more than one instance is placed in the scene.
    /// </summary>
    public class FishBounds : MonoBehaviour
    {
        #region FIELDS
        [SerializeField] private Vector3 size = new Vector3(10f, 10f, 10f);

        private static FishBounds instance;
        #endregion

        #region MONOBEHAVIOR
        private void Awake()
        {
            if (instance != null && instance != this)
            {
                throw new System.Exception(
                    $"Class ({gameObject.name}): Multiple FishBounds detected in the scene. Only one is allowed.");

            }
            instance = this;
        }

        private void OnDestroy()
        {
            if (instance == this)
                instance = null;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(transform.position, size);
        }
        #endregion

        #region FISHBOUNDS
        /// <summary>
        /// Returns true if the given world position is inside the fish bounds volume.
        /// </summary>
        /// <param name="position">World position to test.</param>
        public static bool IsInside(Vector3 position)
        {
            if (instance == null)
            {
                throw new System.Exception("Class (FishBounds): No FishBounds instance found in the scene. Please add one.");
            }

            Vector3 localPos = instance.transform.InverseTransformPoint(position);
            return Mathf.Abs(localPos.x) <= instance.size.x * 0.5f &&
                   Mathf.Abs(localPos.y) <= instance.size.y * 0.5f &&
                   Mathf.Abs(localPos.z) <= instance.size.z * 0.5f;
        }
        #endregion
    }
}

