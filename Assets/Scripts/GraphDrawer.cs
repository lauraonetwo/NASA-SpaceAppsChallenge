using System.Collections.Generic;
using UnityEngine;

public class GraphDrawer : MonoBehaviour
{
    public List<List<float>> linesData = new List<List<float>>();
    public List<Color> lineColors = new List<Color>();
    public List<string> categories = new List<string> { "Public Transport", "Housing", "Green Spaces", "Waste Management", "Energy Production" };
    public List<string> xAxisLabels = new List<string> { "Year 1", "Year 2", "Year 3", "Year 4", "Year 5" };

    public float horizontalMargin = 50f;
    public float verticalMargin = 50f;
    public float lineThickness = 2f;
    public float pointSize = 6f;
    public float axesThickness = 2f;

    public Vector2 legendPosition = new Vector2(10f, 50f);
    public int legendFontSize = 14;

    private float animationProgress = 0f;
    public float animationSpeed = 1f;

    private void Start()
    {
        linesData.Add(new List<float> { 10, 20, 30, 40, 50 });
        linesData.Add(new List<float> { 5, 35, 20, 45, 10 });
        linesData.Add(new List<float> { 30, 25, 35, 15, 40 });
        linesData.Add(new List<float> { 20, 15, 25, 35, 30 });
        linesData.Add(new List<float> { 25, 10, 20, 30, 45 });

        lineColors.Add(Color.blue);
        lineColors.Add(Color.yellow);
        lineColors.Add(Color.green);
        lineColors.Add(Color.red);
        lineColors.Add(Color.cyan);
    }

    private void Update()
    {
        animationProgress += Time.deltaTime * animationSpeed;
        animationProgress = Mathf.Clamp(animationProgress, 0f, xAxisLabels.Count - 1);
    }

    private void OnGUI()
    {
        legendPosition.x = Mathf.Clamp(legendPosition.x, 0, Screen.width / 2 - 50);
        DrawGraph();
        DrawLegend();
    }

    private void DrawGraph()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        float graphWidth = screenWidth - 2 * horizontalMargin - 150;
        float graphHeight = screenHeight - 2 * verticalMargin;
        float startX = horizontalMargin + 150;
        float startY = verticalMargin;

        DrawAxesAndGrid(startX, startY, graphWidth, graphHeight);
        float maxValue = GetMaxValue();

        for (int lineIndex = 0; lineIndex < linesData.Count; lineIndex++)
        {
            List<float> lineData = linesData[lineIndex];

            int numPointsToDraw = Mathf.FloorToInt(animationProgress);

            for (int i = 0; i < numPointsToDraw; i++)
            {
                if (i < lineData.Count - 1)
                {
                    float normalizedValue1 = lineData[i] / maxValue;
                    float normalizedValue2 = lineData[i + 1] / maxValue;
                    float x1 = startX + (i * (graphWidth / (lineData.Count - 1)));
                    float y1 = startY + graphHeight - (normalizedValue1 * graphHeight);
                    float x2 = startX + ((i + 1) * (graphWidth / (lineData.Count - 1)));
                    float y2 = startY + graphHeight - (normalizedValue2 * graphHeight);

                    Drawing.DrawLine(new Vector2(x1, y1), new Vector2(x2, y2), lineColors[lineIndex], lineThickness);

                    Drawing.DrawCircle(new Vector2(x1, y1), pointSize, lineColors[lineIndex]);
                }
            }

            if (numPointsToDraw < lineData.Count)
            {
                float x = startX + (numPointsToDraw * (graphWidth / (lineData.Count - 1)));
                float y = startY + graphHeight - (lineData[numPointsToDraw] / maxValue * graphHeight);
                Drawing.DrawCircle(new Vector2(x, y), pointSize, lineColors[lineIndex]);
            }
        }

        for (int i = 0; i < xAxisLabels.Count; i++)
        {
            float x = startX + (i * (graphWidth / (xAxisLabels.Count - 1)));
            GUI.Label(new Rect(x - 20, startY + graphHeight + 10, 100, 20), xAxisLabels[i]);
        }

        for (int i = 0; i <= 5; i++)
        {
            float percentage = (i * 20);
            float y = startY + graphHeight - (i * (graphHeight / 5));
            GUI.Label(new Rect(startX - 50, y - 10, 40, 20), percentage.ToString("0") + "%");
        }
    }

    private void DrawLegend()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = legendFontSize;
        style.normal.textColor = Color.black;

        for (int i = 0; i < categories.Count; i++)
        {
            style.normal.textColor = lineColors[i];
            GUI.Label(new Rect(legendPosition.x, legendPosition.y + i * 50, 200, 20), categories[i], style);
        }

        GUI.color = Color.white;
    }

    private void DrawAxesAndGrid(float startX, float startY, float graphWidth, float graphHeight)
    {
        Drawing.DrawLine(new Vector2(startX, startY), new Vector2(startX, startY + graphHeight), Color.black, axesThickness);
        Drawing.DrawLine(new Vector2(startX, startY + graphHeight), new Vector2(startX + graphWidth, startY + graphHeight), Color.black, axesThickness);

        for (int i = 0; i < xAxisLabels.Count; i++)
        {
            float x = startX + (i * (graphWidth / (xAxisLabels.Count - 1)));
            Drawing.DrawLine(new Vector2(x, startY), new Vector2(x, startY + graphHeight), Color.gray, 1f);
        }

        for (int i = 0; i <= 5; i++)
        {
            float y = startY + graphHeight - (i * (graphHeight / 5));
            Drawing.DrawLine(new Vector2(startX, y), new Vector2(startX + graphWidth, y), Color.gray, 1f);
        }
    }

    private float GetMaxValue()
    {
        float max = float.MinValue;
        foreach (var line in linesData)
        {
            foreach (var value in line)
            {
                if (value > max)
                    max = value;
            }
        }
        return max;
    }
}

public static class Drawing
{
    private static Texture2D lineTex = null;

    public static void DrawLine(Vector2 pointA, Vector2 pointB, Color color, float width)
    {
        if (!lineTex)
        {
            lineTex = new Texture2D(1, 1);
        }

        Color previousColor = GUI.color;
        GUI.color = color;

        Matrix4x4 matrix = GUI.matrix;
        float angle = Mathf.Atan2(pointB.y - pointA.y, pointB.x - pointA.x) * 180f / Mathf.PI;
        float length = (pointB - pointA).magnitude;

        GUIUtility.RotateAroundPivot(angle, pointA);
        GUI.DrawTexture(new Rect(pointA.x, pointA.y, length, width), lineTex);
        GUI.matrix = matrix;

        GUI.color = previousColor;
    }

    public static void DrawCircle(Vector2 center, float radius, Color color)
    {
        if (!lineTex)
        {
            lineTex = new Texture2D(1, 1);
        }

        Color previousColor = GUI.color;
        GUI.color = color;

        GUI.DrawTexture(new Rect(center.x - radius / 2, center.y - radius / 2, radius, radius), lineTex);

        GUI.color = previousColor;
    }
}
