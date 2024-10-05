using UnityEngine;

public class DragNDrop : MonoBehaviour
{
    private bool isDragging = false;            // Bandera para saber si está arrastrando algo
    private Camera mainCamera;                  // Referencia a la cámara principal
    private GameObject objectToDrag;            // Referencia al objeto que se está arrastrando
    private Collider2D objectCollider;          // Collider del objeto arrastrado

    void Start()
    {
        // Obtener la referencia a la cámara principal
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Detectar cuando se hace clic en el mouse
        if (Input.GetMouseButtonDown(0))
        {
            TryPickObject(); // Intentar seleccionar un objeto con Collider2D debajo del mouse
        }

        // Detectar cuando se suelta el botón del mouse
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            TryPlaceOnCollider();  // Intentar soltar el objeto sobre otro collider
            isDragging = false;    // Dejar de arrastrar
            objectToDrag = null;   // Limpiar referencia del objeto arrastrado
        }

        // Si se está arrastrando un objeto, seguir la posición del mouse
        if (isDragging && objectToDrag != null)
        {
            Vector3 mousePosition = GetMouseWorldPosition();
            objectToDrag.transform.position = mousePosition;
        }
    }

    // Intentar seleccionar el objeto debajo del mouse
    void TryPickObject()
    {
        // Obtener la posición del mouse en el mundo
        Vector3 mousePosition = GetMouseWorldPosition();

        // Detectar si hay un Collider2D en la posición del mouse usando OverlapPointAll (para mayor precisión)
        Collider2D[] hitColliders = Physics2D.OverlapPointAll(mousePosition);

        // Si se detecta algún collider, seleccionar el primero que sea un PolygonCollider2D
        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider != null && hitCollider is PolygonCollider2D && hitCollider.tag.Equals("Object"))
            {
                objectToDrag = hitCollider.gameObject;   // Guardar el objeto que vamos a arrastrar
                objectCollider = objectToDrag.GetComponent<Collider2D>(); // Obtener su collider
                isDragging = true; // Cambiar a estado de arrastrar
                break; // Salimos después de encontrar el primer objeto válido
            }
        }
    }

    // Método para obtener la posición del mouse en el mundo (coordenadas 2D en Unity)
    Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPosition = Input.mousePosition; // Posición del mouse en la pantalla
        mouseScreenPosition.z = Mathf.Abs(mainCamera.transform.position.z); // Distancia en la profundidad de la cámara
        return mainCamera.ScreenToWorldPoint(mouseScreenPosition);
    }

    // Método para intentar colocar el objeto sobre otro collider
    void TryPlaceOnCollider()
    {
        Vector3 mousePosition = GetMouseWorldPosition();

        objectCollider.enabled = false;

        Collider2D[] hitColliders = Physics2D.OverlapPointAll(mousePosition);

        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider != null && hitCollider is PolygonCollider2D && hitCollider.tag.Equals("Empty"))
            {
                // Colocar el objeto que estamos arrastrando en la posición exacta del collider detectado
                objectToDrag.transform.position = hitCollider.transform.position;
                break; // Salimos después de encontrar un collider válido
            }
        }

        // Volver a activar el collider del objeto arrastrado
        objectCollider.enabled = true;
    }
}
