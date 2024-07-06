using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private WaypointManager waypointManager;

    void Start()
    {
        waypointManager = GetComponent<WaypointManager>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SetNewTarget();
        }
    }

    void SetNewTarget()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            waypointManager.SetDestination(hit.point);
        }
    }
}
