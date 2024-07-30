using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 1f;
    [SerializeField] public int maxMoves;

    [SerializeField] Vector2Int movementBoundsMin = new Vector2Int(-10, -10);
    [SerializeField] Vector2Int movementBoundsMax = new Vector2Int(10, 10);

    Transform selectedUnit;
    bool unitSelected = false;
    private GameObject currentTile;

    List<Node> path = new List<Node>();

    public GameObject grass;

    GridManager gridManager;
    Pathfinding pathFinder;
    Teleport tp;

    public int moveCount = 0;
    public TextMeshProUGUI textRemainingMoves;
    int remainingMoves;

    public GameObject panelGameOver;

    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        pathFinder = FindObjectOfType<Pathfinding>();
        Debug.Log($"Max Moves: {maxMoves}");
        Debug.Log(gameObject.name);
        updateText();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastFunc();
        if (moveCount > maxMoves)
        {
            moveCount = maxMoves;        }
        else
        { 
        }
    }

    public void RaycastFunc()
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
                if (hit.transform.CompareTag("tile") || hit.transform.CompareTag("tp") || hit.transform.CompareTag("geser") )
                {
                    if (unitSelected)
                    {
                        Vector2Int targetCords = hit.transform.GetComponent<Tile>().cords;
                        currentTile = hit.transform.gameObject;
                        Vector2Int startCords = new Vector2Int((int)selectedUnit.transform.position.x, (int)selectedUnit.transform.position.z) / gridManager.UnityGridSize;
                        

                        if (IsWithinBounds(targetCords))
                        {
                            if (moveCount < maxMoves)
                            {
                                pathFinder.SetNewDestination(startCords, targetCords);
                                RecalculatePath(true);
                                moveCount++;
                            }
                            else
                            {
                                Debug.Log("Move limit reached. No further movements allowed.");
                            }
                        }
                        else
                        {
                            Debug.Log("Target is out of bounds.");
                        }
                    }
                }
                else if (hit.transform.CompareTag("unit"))
                {
                    selectedUnit = hit.transform;
                    unitSelected = true;
                    Debug.Log("Unit was clicked.");
                }
            }
        }
    }

    void RecalculatePath(bool resetPath)
    {
        Vector2Int coordinates;
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
        try
        {
            Teleport tp = currentTile.GetComponent<Teleport>();
            tp.Teleports(() =>
            {
                Debug.Log("Teleportation complete. Callback executed in a different script.");
            });
        } catch { Debug.Log("Tp habis"); }

    }

    bool IsWithinBounds(Vector2Int position)
    {
        return position.x >= movementBoundsMin.x && position.x <= movementBoundsMax.x &&
               position.y >= movementBoundsMin.y && position.y <= movementBoundsMax.y;
    }

    public void updateText()
    {
        remainingMoves = maxMoves - moveCount;
        textRemainingMoves.text = remainingMoves.ToString();
        Debug.Log("Max Moves HUD text should be updated. Remaining moves : " + remainingMoves);
    }
}
