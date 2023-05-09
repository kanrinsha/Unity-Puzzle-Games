using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ShapePuzzle : MonoBehaviour
{
    public float distanceMargin;

    public Shape[] shapes;
    public Dictionary<Shape, Vector2> startPositions = new Dictionary<Shape, Vector2>();

    [HideInInspector] public ShapePuzzleManager manager;

    private List<Shape> correctShapes = new List<Shape>();

    private void Awake()
    {
        shapes = GetComponentsInChildren<Shape>();

        foreach (var item in shapes)
        {
            startPositions.Add(item, item.m_transform.anchoredPosition);
        }

        RandomizeShapes();

        while (CheckPuzzleComplete())
            RandomizeShapes();
    }

    private void RandomizeShapes()
    {
        var usedIndexes = new List<int>();

        for (int i = 0; i < shapes.Length;)
        {
            Shape shape = shapes[i];
            var index = Random.Range(0, startPositions.Count);

            if (!usedIndexes.Contains(index))
            {
                shape.m_transform.anchoredPosition = startPositions.ElementAt(index).Value;
                usedIndexes.Add(index);
                i++;
            }
        }
    }

    private void FixedUpdate()
    {
        correctShapes.Clear();

        foreach (var pair in startPositions)
        {
            var startPos = pair.Value;

            if (!pair.Key.isShapeSpecific)
            {
                foreach (var shape in shapes)
                {
                    if (Vector2.Distance(shape.m_transform.anchoredPosition, startPos) < distanceMargin && shape.shapeType == pair.Key.shapeType)
                    {
                        correctShapes.Add(shape);
                        break;
                    }
                }
            }
            else if (Vector2.Distance(pair.Key.m_transform.anchoredPosition, startPos) < distanceMargin)
            {
                correctShapes.Add(pair.Key);
            }
        }

        if (CheckPuzzleComplete())
        {
            manager.PuzzleCompleted();
        }
    }

    private bool CheckPuzzleComplete()
    {
        if (correctShapes.Count == shapes.Length)
            return true;

        return false;
    }
}
