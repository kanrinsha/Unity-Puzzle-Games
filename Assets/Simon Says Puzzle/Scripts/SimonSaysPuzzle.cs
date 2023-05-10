using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SimonSaysPuzzle : MonoBehaviour
{
    private Dictionary<Button, Image> buttonPairs = new Dictionary<Button, Image>();
    private Dictionary<Button, Color> buttonColors = new Dictionary<Button, Color>();
    private List<Button> buttonSequence = new List<Button>();
    private List<Button> clickedButtons = new List<Button>();

    public int currentButtonSequence = 0;
    public int buttonSequenceCount = 10;

    public GameObject startButton;
    public GameObject completeScreen;
    public GameObject failedScreen;

    private bool isDoingPuzzle;
    private Button clickedButton = null;

    private void Awake()
    {
        foreach(var button in GetComponentsInChildren<Button>())
        {
            if (button.name == "Start")
                continue;

            var image = button.GetComponent<Image>();
            buttonPairs.Add(button, image);
            buttonColors.Add(button, new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f),
                UnityEngine.Random.Range(0f, 1f)));

            button.onClick.AddListener(() => StartCoroutine(ButtonClicked(button)));
        }
    }

    private IEnumerator ButtonClicked(Button button)
    {
        if (!isDoingPuzzle)
            yield break;

        if(buttonColors.TryGetValue(button, out var color))
        {
            clickedButton = button;

            buttonPairs[button].color = color;

            yield return new WaitForSeconds(0.25f);

            buttonPairs[button].color = Color.black;
        }
    }

    public void StartPuzzle()
    {
        startButton.SetActive(false);

        ResetColors();

        isDoingPuzzle = true;

        for (int i = 0; i < buttonSequenceCount; i++)
        {
            var randomButton = buttonPairs.ElementAt(UnityEngine.Random.Range(0, buttonPairs.Count));
            buttonSequence.Add(randomButton.Key);         
        }

        StartCoroutine(DoPuzzleLoop());
    }

    private IEnumerator DoPuzzleLoop()
    {
        while(isDoingPuzzle)
        {
            if(currentButtonSequence > buttonSequence.Count)
            {
                //game complete
                print("Game Complete!");

                completeScreen.SetActive(true);

                //EndPuzzle();
                yield break;
            }

            for (int i = 0; i < currentButtonSequence; i++)
            {
                buttonPairs.TryGetValue(buttonSequence[i], out var image);

                image.color = buttonColors[buttonSequence[i]];

                yield return new WaitForSeconds(0.25f);

                image.color = Color.black;

                yield return new WaitForSeconds(0.25f);
            }

            currentButtonSequence++;

            clickedButtons.Clear();

            while(clickedButtons.Count != currentButtonSequence - 1)
            {
                yield return new WaitUntil(() => clickedButton != null);

                if (clickedButton != buttonSequence[clickedButtons.Count])
                {
                    print("Game Failed!");
                    failedScreen.SetActive(true);


                    EndPuzzle();
                }
                clickedButtons.Add(clickedButton);

                clickedButton = null;
            }

            yield return new WaitForSeconds(1f);
        }
    }

    private void ResetColors()
    {
        foreach (var pair in buttonPairs) {
            var image = pair.Value;

            image.color = Color.black;
        }
    }

    public void EndPuzzle()
    {
        isDoingPuzzle = false;
        buttonSequence.Clear();
        clickedButtons.Clear();
        currentButtonSequence = 0;
        clickedButton = null;

        foreach (var pair in buttonPairs)
        {
            pair.Value.color = Color.black;

            buttonColors[pair.Key] = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f),
                UnityEngine.Random.Range(0f, 1f));
        }

        startButton.SetActive(true);
    }
}
