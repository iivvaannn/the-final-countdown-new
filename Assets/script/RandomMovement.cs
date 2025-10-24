using UnityEngine;
using UnityEngine.AI;

public class RandomMovement : MonoBehaviour
{
    public NavMeshAgent agent; // Reference to the NavMeshAgent component
    public float range = 10.0f; // Radius of the area to search for random points
    public Transform centrePoint; // Center point for random movement

    void Start()
    {
        // Get the NavMeshAgent component attached to this GameObject
        agent = GetComponent<NavMeshAgent>();
        SetNewDestination(); // Set the initial destination
    }

    void Update()
    {
        // Check if the agent has reached its destination
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            SetNewDestination(); // Set a new destination when the agent stops
        }
    }

    void SetNewDestination()
    {
        Vector3 point;
        // Use the RandomPoint method to find a new point on the NavMesh
        if (RandomPoint(centrePoint.position, range, out point))
        {
            Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); // Visualize the point
            agent.SetDestination(point); // Move the agent to the new point
            Debug.Log("New destination set: " + point); // Log the new destination
        }
        else
        {
            Debug.Log("No valid point found."); // Log if no valid point was found
        }
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++) // Try multiple times to find a valid point
        {
            // Generate a random point within a sphere around the center point
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            // Check if the random point is on the NavMesh
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position; // Return the valid point
                return true; // Successfully found a valid point
            }
        }
        result = Vector3.zero; // Return zero if no valid point is found
        return false; // No valid point found
    }
}