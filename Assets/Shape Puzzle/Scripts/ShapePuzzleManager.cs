using UnityEngine;

public class ShapePuzzleManager : MonoBehaviour
{
    public int currentPuzzle = 1;
    public ShapePuzzle[] shapePuzzles = null;

    private void Start()
    {
        for (int i = 0; i < shapePuzzles.Length; i++)
        {
            ShapePuzzle puzzle = shapePuzzles[i];

            puzzle.manager = this;

            if (i == currentPuzzle)
            {
                puzzle.gameObject.SetActive(true);
                continue;
            }

            puzzle.gameObject.SetActive(false);
        }
    }

    public void PuzzleCompleted()
    {
        shapePuzzles[currentPuzzle].gameObject.SetActive(false);
        currentPuzzle++;

        if (currentPuzzle < shapePuzzles.Length)
        {
            shapePuzzles[currentPuzzle].gameObject.SetActive(true);

            print("Puzzle advanced!");
        }
        else
        {
            print("No more puzzles!");
        }
    }
}
