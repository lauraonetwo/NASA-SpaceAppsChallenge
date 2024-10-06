using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    public void ChangeToScene(string sceneName)
    {
        Debug.Log("Changing scene");
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
