using UnityEngine;

public class StoneController : MonoBehaviour
{
    private Vector3 offset;
    private bool isDragging = false;
    private PipeController currentPipe;

    private void OnMouseEnter()
    {
        // Tambahkan efek hover di sini
    }

    private void OnMouseExit()
    {
        // Hapus efek hover di sini
    }

    private void OnMouseDown()
    {
        offset = transform.position - GetMouseWorldPosition();
        isDragging = true;
        currentPipe = GetComponentInParent<PipeController>();
    }

    private void OnMouseUp()
    {
        isDragging = false;
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
                // Batasi gerakan sepanjang pipa berdasarkan pusat collider
                float clampedX = Mathf.Clamp(mousePosition.x, pipeCenter.x - pipeLength, pipeCenter.x + pipeLength);
                transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
            }
            else
            {
                // Batasi gerakan sepanjang pipa berdasarkan pusat collider
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
