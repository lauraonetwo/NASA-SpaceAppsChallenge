using System.Collections.Generic;
using TMPro;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public List<GameObject> objects = new List<GameObject>();

    private GameObject budgetObj;
    private ulong demolishPrice = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        budgetObj = GameObject.Find("Budget");
    }

    public void Demolish()
    {
        foreach (GameObject obj in objects)
        {
            Info info = obj.GetComponent<Info>();
            if (info.assignedEmpty != null)
            {
                info.assignedEmpty.GetComponent<Available>().isAvailable = true;
            }
            demolishPrice += info.price;
            Destroy(obj);
        }
        objects.Clear();

        TextMeshProUGUI text = budgetObj.GetComponent<TextMeshProUGUI>();
        string numericString = new string(text.text.Where(char.IsDigit).ToArray());
        ulong.TryParse(numericString, out ulong budget);
        budget -= demolishPrice;
        text.text = "Budget: $" + budget.ToString("N0");
    }
}
