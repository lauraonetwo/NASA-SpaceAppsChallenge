using UnityEngine;

public class DragNDrop : MonoBehaviour
{
    private bool isDragging = false;            // Bandera para saber si est� arrastrando algo
    private Camera mainCamera;                  // Referencia a la c�mara principal
    private GameObject objectToDrag;            // Referencia al objeto que se est� arrastrando
    private Collider2D objectCollider;          // Collider del objeto arrastrado

    void Start()
    {
        // Obtener la referencia a la c�mara principal
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Detectar cuando se hace clic en el mouse
        if (Input.GetMouseButtonDown(0))
        {
            TryPickObject(); // Intentar seleccionar un objeto con Collider2D debajo del mouse
        }

        // Detectar cuando se suelta el bot�n del mouse
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            TryPlaceOnCollider();  // Intentar soltar el objeto sobre otro collider
            isDragging = false;    // Dejar de arrastrar
            objectToDrag = null;   // Limpiar referencia del objeto arrastrado
        }

        // Si se est� arrastrando un objeto, seguir la posici�n del mouse
        if (isDragging && objectToDrag != null)
        {
            Vector3 mousePosition = GetMouseWorldPosition();
            objectToDrag.transform.position = mousePosition;
        }
    }

    // Intentar seleccionar el objeto debajo del mouse
    void TryPickObject()
    {
        // Obtener la posici�n del mouse en el mundo
        Vector3 mousePosition = GetMouseWorldPosition();

        // Detectar si hay un Collider2D en la posici�n del mouse usando OverlapPointAll (para mayor precisi�n)
        Collider2D[] hitColliders = Physics2D.OverlapPointAll(mousePosition);

        // Si se detecta alg�n collider, seleccionar el primero que sea un PolygonCollider2D
        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider != null && hitCollider is PolygonCollider2D && hitCollider.tag.Equals("Object"))
            {
                objectToDrag = hitCollider.gameObject;   // Guardar el objeto que vamos a arrastrar
                objectCollider = objectToDrag.GetComponent<Collider2D>(); // Obtener su collider
                isDragging = true; // Cambiar a estado de arrastrar
                break; // Salimos despu�s de encontrar el primer objeto v�lido
            }
        }
    }

    // M�todo para obtener la posici�n del mouse en el mundo (coordenadas 2D en Unity)
    Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPosition = Input.mousePosition; // Posici�n del mouse en la pantalla
        mouseScreenPosition.z = Mathf.Abs(mainCamera.transform.position.z); // Distancia en la profundidad de la c�mara
        return mainCamera.ScreenToWorldPoint(mouseScreenPosition);
    }

    // M�todo para intentar colocar el objeto sobre otro collider
    void TryPlaceOnCollider()
    {
        Vector3 mousePosition = GetMouseWorldPosition();

        objectCollider.enabled = false;

        Collider2D[] hitColliders = Physics2D.OverlapPointAll(mousePosition);

        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider != null && hitCollider is PolygonCollider2D && hitCollider.tag.Equals("Empty"))
            {
                // Colocar el objeto que estamos arrastrando en la posici�n exacta del collider detectado
                objectToDrag.transform.position = hitCollider.transform.position;
                break; // Salimos despu�s de encontrar un collider v�lido
            }
        }

        // Volver a activar el collider del objeto arrastrado
        objectCollider.enabled = true;
    }
}
