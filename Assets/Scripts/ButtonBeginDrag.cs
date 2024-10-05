using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonBeginDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject prefab;
    private GameObject instantiatedObject;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector3 screenPosition = Input.mousePosition;
        screenPosition.z = Mathf.Abs(mainCamera.transform.position.z);
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenPosition);
        instantiatedObject = Instantiate(prefab, worldPosition, Quaternion.identity);
        instantiatedObject.GetComponent<DragNDrop>().TryPickObject();

        if (EventSystem.current != null && instantiatedObject != null)
        {
            EventSystem.current.SetSelectedGameObject(instantiatedObject);

            ExecuteEvents.Execute<IBeginDragHandler>(instantiatedObject, eventData, ExecuteEvents.beginDragHandler);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (instantiatedObject != null)
        {
            Vector3 screenPosition = Input.mousePosition;
            screenPosition.z = Mathf.Abs(mainCamera.transform.position.z);
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenPosition);

            instantiatedObject.transform.position = worldPosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
    }
}
