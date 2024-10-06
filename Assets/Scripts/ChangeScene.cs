using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public string loadedScene;
    public static void ChangeToScene(string sceneName)
    {
        Debug.Log("Changing scene");
        SceneManager.LoadScene(sceneName);

        if (sceneName == "Menu")
        {
            GameManager.Demolish();
        }
    }

    public static void QuitGame()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }

    public static void LoadAdditionalScene(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }
        else
        {
            Debug.LogWarning("El nombre de la escena a cargar no está asignado.");
        }
    }

    public static void UnloadLoadedScene(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.UnloadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("No hay escena cargada para descargar.");
        }
    }
}
