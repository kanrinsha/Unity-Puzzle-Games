using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonSaysGameManager : MonoBehaviour
{
    public int currentPuzzle = 1;
    public SimonSaysPuzzle[] shapePuzzles = null;

    private void Start()
    {
        for (int i = 0; i < shapePuzzles.Length; i++)
        {
            SimonSaysPuzzle puzzle = shapePuzzles[i];

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
