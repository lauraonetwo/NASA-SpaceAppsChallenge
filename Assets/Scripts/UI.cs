using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI : MonoBehaviour
{
    public List<Button> buttons = new List<Button>();
    bool clicked = false;


    private void Awake()
    {
        foreach (Button button in buttons)
        {
            button.gameObject.SetActive(false);
        }
        clicked = false;
    }

    public void ShowOptions()
    {
        clicked = !clicked;
        foreach (Button button in buttons)
        {
            button.gameObject.SetActive(clicked);
        }
    }
}
