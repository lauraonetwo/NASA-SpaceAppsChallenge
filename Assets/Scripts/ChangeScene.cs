using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    public void ChangeToScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
