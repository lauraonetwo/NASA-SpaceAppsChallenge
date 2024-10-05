using UnityEngine;

public class DragNDrop : MonoBehaviour
{
    public bool isDragging = false;           
    private Camera mainCamera;                 
    private GameObject objectToDrag;           
    private Collider2D objectCollider;         

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryPickObject();
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            TryPlaceOnCollider();  
            isDragging = false;    
            objectToDrag = null;   
        }

        if (isDragging && objectToDrag != null)
        {
            Vector3 mousePosition = GetMouseWorldPosition();
            objectToDrag.transform.position = mousePosition;
        }
    }

    public void TryPickObject()
    {
        Vector3 mousePosition = GetMouseWorldPosition();

        Collider2D[] hitColliders = Physics2D.OverlapPointAll(mousePosition);

        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider != null && hitCollider is PolygonCollider2D && hitCollider.tag.Equals("Object"))
            {
                objectToDrag = hitCollider.gameObject;
                objectCollider = objectToDrag.GetComponent<Collider2D>();
                isDragging = true;
                break;
            }
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = Mathf.Abs(Camera.main.transform.position.z);
        return Camera.main.ScreenToWorldPoint(mouseScreenPosition);
    }

    void TryPlaceOnCollider()
    {
        Vector3 mousePosition = GetMouseWorldPosition();

        objectCollider.enabled = false;

        Collider2D[] hitColliders = Physics2D.OverlapPointAll(mousePosition);

        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider != null && hitCollider is PolygonCollider2D && hitCollider.tag.Equals("Empty"))
            {
                objectToDrag.transform.position = hitCollider.transform.position;
                break;
            }
        }

        objectCollider.enabled = true;
    }
}
