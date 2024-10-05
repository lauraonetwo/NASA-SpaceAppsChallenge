using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI : MonoBehaviour
{
    public List<Button> buttons = new List<Button>();
    bool clicked = false;
    float counter = 0;
    [SerializeField] private float timeToWait = 2.0f;
    bool startTimer = false;
    bool show = false;

    public List<GameObject> details = new List<GameObject>();

    private void Awake()
    {
        foreach (Button button in buttons)
        {
            button.gameObject.SetActive(false);
        }
        clicked = false;
    }

    private void Update()
    {
        if (startTimer)
        {
            counter += Time.deltaTime;
            if (counter > timeToWait)
            {
                show = true;
            }
        }

        if (show)
        {
            Debug.Log("Details");
        }
    }

    public void ShowOptions()
    {
        clicked = !clicked;
        foreach (Button button in buttons)
        {
            button.gameObject.SetActive(clicked);
        }
    }

    public void ShowDetails(int v)
    {
        startTimer = true;
        details[v].SetActive(true);
    }

    public void ResetTimer(int v)
    {
        startTimer = false;
        counter = 0;
        show = false;
        details[v].SetActive(false);
    }

    public void Dragging(int v)
    {
        Debug.Log("Dragging");
    }
}
