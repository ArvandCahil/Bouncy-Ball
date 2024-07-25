using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class UnitController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 1f;
    [SerializeField] int maxMoves; 

    [SerializeField] Vector2Int movementBoundsMin = new Vector2Int(-10, -10);
    [SerializeField] Vector2Int movementBoundsMax = new Vector2Int(10, 10);

    [SerializeField] GameObject cameraObject; 
    private CinemachineFreeLook cinemachineFreeLook; 
    private CameraControl cameraControl; 

    Transform selectedUnit;
    bool unitSelected = false;

    List<Node> path = new List<Node>();

    GridManager gridManager;
    Pathfinding pathFinder;
    Tp tp;

    int moveCount = 0;
    [SerializeField] float doubleTapTime = 0.3f;
    [SerializeField] float lastTapTime = 0f;
    private bool isDoubleTap = false;

    private bool cameraLock = false; 
    private float unitCooldown = 0f; 

    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        pathFinder = FindObjectOfType<Pathfinding>();

        
        if (cameraObject != null)
        {
            cinemachineFreeLook = cameraObject.GetComponent<CinemachineFreeLook>();
            if (cinemachineFreeLook == null)
            {
                Debug.LogError("CinemachineFreeLook component not found on the camera GameObject.");
            }
        }
        else
        {
            Debug.LogError("Camera GameObject reference is missing.");
        }

        
        cameraControl = FindObjectOfType<CameraControl>();
        if (cameraControl == null)
        {
            Debug.LogError("CameraControl component not found.");
        }

        Debug.Log($"Max Moves: {maxMoves}");
        Debug.Log(gameObject.name);
    }

    void Update()
    {
        RaycastFunc();
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
                if (hit.transform.CompareTag("tile") || hit.transform.CompareTag("tp"))
                {                    
                    if (unitSelected)
                    {
                        Vector2Int targetCords = hit.transform.GetComponent<Tile>().cords;
                        Vector2Int startCords = new Vector2Int((int)selectedUnit.transform.position.x, (int)selectedUnit.transform.position.z) / gridManager.UnityGridSize;

                        if (IsWithinBounds(targetCords))
                        {
                            if (moveCount < maxMoves)
                            {
                                StartCoroutine(WaitAndMove(startCords, targetCords)); 
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

    IEnumerator WaitAndMove(Vector2Int startCords, Vector2Int targetCords)
    {
        
        yield return new WaitForSeconds(1f);

        
        if (cameraLock && unitCooldown > 0)
        {
            Debug.Log("Camera lock is active and unit cooldown is not 0. Skipping movement.");
            yield break; 
        }

        
        if (cameraControl != null)
        {
            if (cameraControl.isCameraModeActive)
            {
                Debug.Log("Camera is in camera mode. Waiting until camera is not in mode.");
                while (cameraControl.isCameraModeActive)
                {
                    yield return null; 
                }

                
                if (unitCooldown <= 0)
                {
                    Debug.Log("Camera is now not in mode. Proceeding with movement.");
                    pathFinder.SetNewDestination(startCords, targetCords);
                    RecalculatePath(true);
                }
                else
                {
                    Debug.Log("Camera is not in mode but cooldown is not 0. Skipping movement.");
                }
            }
        }
        else
        {
            Debug.Log("CameraControl component is not assigned or missing.");
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
            Vector3 endPosition = gridManager.GetPositionFromCoordinates(path[i].cords);
            selectedUnit.LookAt(new Vector3(endPosition.x, selectedUnit.position.y, endPosition.z));

            float travelTime = Vector3.Distance(selectedUnit.position, endPosition) / movementSpeed;

            Tween moveXTween = selectedUnit.DOMoveX(endPosition.x, travelTime).SetEase(Ease.Linear);
            Tween moveZTween = selectedUnit.DOMoveZ(endPosition.z, travelTime).SetEase(Ease.Linear);

            yield return DOTween.Sequence().Join(moveXTween).Join(moveZTween).WaitForCompletion();

            Debug.Log(endPosition);
        }

        tp.Teleport(() =>
        {
            Debug.Log("Teleportation complete. Callback executed in a different script.");
        });
    }

    bool IsWithinBounds(Vector2Int position)
    {
        return position.x >= movementBoundsMin.x && position.x <= movementBoundsMax.x &&
               position.y >= movementBoundsMin.y && position.y <= movementBoundsMax.y;
    }
}
