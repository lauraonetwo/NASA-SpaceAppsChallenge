using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;

public class DragNDrop : MonoBehaviour
{
    public bool isDragging = false;           
    private GameObject objectToDrag;           
    private Collider2D objectCollider;
    GameObject budgetObject;
    TextMeshProUGUI text;
    bool paid = false;

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
                Info info = objectToDrag.GetComponent<Info>();
                GameObject empty = info.assignedEmpty;
                if (info.assignedEmpty != null)
                {
                    empty.GetComponent<Available>().isAvailable = true;
                }
                info.assignedEmpty = null;
                if (!paid)
                {
                    budgetObject = GameObject.Find("Budget");
                    text = budgetObject.GetComponent<TextMeshProUGUI>();
                    string numericString = new string(text.text.Where(char.IsDigit).ToArray());
                    ulong currentBudget = ulong.Parse(numericString, NumberStyles.AllowThousands);
                    currentBudget -= GetComponent<Info>().price;
                    text.text = "Budget: $" + currentBudget.ToString("N0");
                    paid = true;
                }
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
            if (hitCollider != null && hitCollider is PolygonCollider2D && hitCollider.tag.Equals("Empty") && hitCollider.GetComponent<Available>().isAvailable)
            {
                objectToDrag.transform.position = hitCollider.transform.position;
                objectToDrag.GetComponent<Info>().assignedEmpty = hitCollider.gameObject;
                hitCollider.GetComponent<Available>().isAvailable = false;
                break;
            }
        }

        objectCollider.enabled = true;
    }
}
