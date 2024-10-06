using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public string loadedScene;
    public void ChangeToScene(string sceneName)
    {
        Debug.Log("Changing scene");
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }

    public void LoadAdditionalScene(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            loadedScene = sceneName;
        }
        else
        {
            Debug.LogWarning("El nombre de la escena a cargar no está asignado.");
        }
    }

    public void UnloadLoadedScene()
    {
        if (!string.IsNullOrEmpty(loadedScene))
        {
            SceneManager.UnloadScene(loadedScene);
        }
        else
        {
            Debug.LogWarning("No hay escena cargada para descargar.");
        }
    }
}
