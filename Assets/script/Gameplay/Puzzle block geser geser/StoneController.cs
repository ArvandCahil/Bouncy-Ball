using UnityEngine;

public class StoneController : MonoBehaviour
{
    [SerializeField] private Material hoverMaterial;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private CameraControl cameraControl;

    private Vector3 offset;
    private bool isDragging = false;
    private PipeController currentPipe;
    private Renderer stoneRenderer;

    private void Awake()
    {
        stoneRenderer = GetComponent<Renderer>();
        if (stoneRenderer != null)
        {
            stoneRenderer.material = defaultMaterial;
        }
    }

    private void OnMouseEnter()
    {
        if (cameraControl != null)
        {
            cameraControl.SetCameraActive(false);
        }

        if (stoneRenderer != null && hoverMaterial != null)
        {
            stoneRenderer.material = hoverMaterial;
        }
    }

    private void OnMouseExit()
    {
        if (!isDragging && cameraControl != null)
        {
            cameraControl.SetCameraActive(true);
        }

        if (stoneRenderer != null && defaultMaterial != null)
        {
            stoneRenderer.material = defaultMaterial;
        }
    }

    private void OnMouseDown()
    {
        offset = transform.position - GetMouseWorldPosition();
        isDragging = true;
        currentPipe = GetComponentInParent<PipeController>();

        if (stoneRenderer != null && hoverMaterial != null)
        {
            stoneRenderer.material = hoverMaterial;
        }
    }

    private void OnMouseUp()
    {
        isDragging = false;

        if (cameraControl != null)
        {
            cameraControl.SetCameraActive(true);
        }

        if (stoneRenderer != null && defaultMaterial != null)
        {
            stoneRenderer.material = defaultMaterial;
        }
    }

    private void Update()
    {
        if (isDragging && currentPipe != null)
        {
            Vector3 mousePosition = GetMouseWorldPosition() + offset;
            Vector3 pipeCenter = currentPipe.GetPipeCenter();
            float pipeLength = currentPipe.GetPipeLength() / 2;

            if (currentPipe.IsHorizontal)
            {
                float clampedX = Mathf.Clamp(mousePosition.x, pipeCenter.x - pipeLength, pipeCenter.x + pipeLength);
                transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
            }
            else
            {
                float clampedY = Mathf.Clamp(mousePosition.y, pipeCenter.y - pipeLength, pipeCenter.y + pipeLength);
                transform.position = new Vector3(transform.position.x, clampedY, transform.position.z);
            }
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}
