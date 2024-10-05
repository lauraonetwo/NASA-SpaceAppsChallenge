using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class UI : MonoBehaviour
{
    private Camera mainCamera;
    public List<Button> buttons = new List<Button>();
    public List<GameObject> toSpawn = new List<GameObject>();
    bool clicked = false;
    float counter = 0;
    [SerializeField] private float timeToWait = 2.0f;
    bool startTimer = false;
    bool show = false;
    int value = 0;
    GameObject instantiatedObject = null;

    public List<GameObject> details = new List<GameObject>();

    private void Awake()
    {
        foreach (Button button in buttons)
        {
            button.gameObject.SetActive(false);
        }
        clicked = false;
        mainCamera = Camera.main;
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
            details[value].SetActive(true);
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
        value = v;
    }

    public void ResetTimer(int v)
    {
        startTimer = false;
        counter = 0;
        show = false;
        details[value].SetActive(false);
    }
}
