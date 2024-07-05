using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaypointManager : MonoBehaviour
{
    public Transform[] waypoints;
    private NavMeshAgent agent;
    private int currentWaypointIndex = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        MoveToNextWaypoint();
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            MoveToNextWaypoint();
        }
    }

    void MoveToNextWaypoint()
    {
        if (waypoints.Length == 0)
            return;

        agent.destination = waypoints[currentWaypointIndex].position;
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
    }

    public void SetDestination(Vector3 targetPosition)
    {
        waypoints[waypoints.Length - 1].position = targetPosition;
        currentWaypointIndex = 0;
        MoveToNextWaypoint();
    }
}
