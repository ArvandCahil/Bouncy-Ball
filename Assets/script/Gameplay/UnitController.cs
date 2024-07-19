using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 1f;

    Transform selectedUnit;
    bool unitSelected = false;

    List<Node> path = new List<Node>();

    GridManager gridManager;
    Pathfinding pathFinder;

    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        pathFinder = FindObjectOfType<Pathfinding>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Ray ray;

            if (Input.GetMouseButtonDown(0))
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            }
            else
            {
                ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            }

            RaycastHit hit;

            bool hasHit = Physics.Raycast(ray, out hit);



            if (hasHit)
            {
                if (hit.transform.tag == "tile")
                {
                    if (unitSelected)
                    {
                        Vector2Int targetCords = hit.transform.GetComponent<Tile>().cords;
                        Vector2Int startCords = new Vector2Int((int)selectedUnit.transform.position.x, (int)selectedUnit.transform.position.z) / gridManager.UnityGridSize;
                        pathFinder.SetNewDestination(startCords, targetCords);
                        RecalculatePath(true);
                    }
                }
                if (hit.transform.tag == "unit")
                {
                    selectedUnit = hit.transform;
                    unitSelected = true;
                    Debug.Log(" unit was clicked ");
                }
            }
        }
    }

    void RecalculatePath(bool resetPath)
    {
        Vector2Int coordinates = new Vector2Int();
        if (resetPath)
        {
            coordinates = pathFinder.StartCords;
        }
        else
        {
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
        }
        StopAllCoroutines();
        path.Clear();
        path = pathFinder.GetNewPath(coordinates);
        StartCoroutine(FollowPath());
    }

    IEnumerator FollowPath()
    {
        for (int i = 1; i < path.Count; i++)
        {
            // Get the target position from the path
            Vector3 endPosition = gridManager.GetPositionFromCoordinates(path[i].cords);

            // Rotate the unit to face the target position
            selectedUnit.LookAt(new Vector3(endPosition.x, selectedUnit.position.y, endPosition.z));

            // Calculate travel time based on movement speed
            float travelTime = Vector3.Distance(selectedUnit.position, endPosition) / movementSpeed;

            // Create tweens to move the unit's x and z coordinates while keeping y position
            Tween moveXTween = selectedUnit.DOMoveX(endPosition.x, travelTime).SetEase(Ease.Linear);
            Tween moveZTween = selectedUnit.DOMoveZ(endPosition.z, travelTime).SetEase(Ease.Linear);

            // Wait for both tweens to complete
            yield return DOTween.Sequence().Join(moveXTween).Join(moveZTween).WaitForCompletion();

            Debug.Log(endPosition);
        }
    }

}