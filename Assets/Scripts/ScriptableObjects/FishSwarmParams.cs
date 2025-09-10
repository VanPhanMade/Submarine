using UnityEngine;

[CreateAssetMenu(fileName = "FishSwarmParams", menuName = "Scriptable Objects/FishSwarmParams")]
public class FishSwarmParams : ScriptableObject
{
    public Mesh fishMesh;
    public Material fishMaterial;

    [Header("Movement")]
    [Tooltip("Minimum swimming speed of the fish.")]
    [Range(0f, 10f)]
    public float minSpeed = 2f;

    [Tooltip("Maximum swimming speed of the fish.")]
    [Range(0f, 20f)]
    public float maxSpeed = 5f;

    [Tooltip("Preferred distance to keep away from other fish.")]
    [Range(0.5f, 5f)]
    public float preferredSeparation = 1.5f;

    [Tooltip("How far the fish can see neighbors.")]
    [Range(1f, 10f)]
    public float neighborRadius = 3f;

    [Tooltip("How strongly the fish can steer when adjusting course.")]
    [Range(0.1f, 10f)]
    public float maxSteerForce = 3f;

    [Tooltip("How quickly the fish rotates toward its velocity direction.")]
    [Range(0.1f, 20f)]
    public float rotationSpeed = 5f;
}
