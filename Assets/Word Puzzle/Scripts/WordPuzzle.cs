using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using System.Text.RegularExpressions;

public class WordPuzzle : MonoBehaviour
{
    private TMP_Text[] allLetterTexts;
    private List<TMP_Text> usedLetterTexts = new List<TMP_Text>();
    private List<string> words = new List<string>();

    private int currentCount;
    private int currentSubmit = 0;

    public GameObject congratulations;
    public GameObject retry;
    public TMP_Text retryText;

    public Color inWordColor = Color.yellow;
    public Color perfectMatchColor = Color.green;
    public Color wrongColor = Color.red;

    public string currentWord = string.Empty;

    private void Awake()
    {
        allLetterTexts = GetComponentsInChildren<TMP_Text>();

        foreach (var text in allLetterTexts)
        {
            text.text = string.Empty;
        }

        var textAsset = Resources.Load<TextAsset>("words_alpha");
        words = Regex.Split(textAsset.text, Environment.NewLine).ToList();

        currentWord = words[UnityEngine.Random.Range(0, words.Count)];
    }

    private void Update()
    {
        foreach (char c in Input.inputString)
        {
            if (c == '\b')
            {
                RemoveChar();
            }
            else if ((c == '\n') || (c == '\r'))
            {
                SubmitButtonClick();
            }
            else
            {
                DoKeyPress(char.ToLower(c));
            }
        }
    }

    private void RemoveChar()
    {
        print("remove");
        if (!usedLetterTexts.Any() || currentCount <= 0)
            return;

        usedLetterTexts.Last().text = string.Empty;
        usedLetterTexts.RemoveAt(usedLetterTexts.Count - 1);
        currentCount--;
    }

    private void DoKeyPress(char character)
    {
        print(character);
        if (usedLetterTexts.Count >= allLetterTexts.Length || currentCount >= 5)
            return;

        var currentText = allLetterTexts[usedLetterTexts.Count];
        currentText.text = character.ToString();
        usedLetterTexts.Add(currentText);

        currentCount++;
    }

    private void SubmitButtonClick()
    {
        print("submit");
        if (currentCount != 5)
            return;

        var submittedWord = string.Empty;

        int ii = 0;
        for (int i = 0 + (currentSubmit * 5); i < usedLetterTexts.Count; i++)
        {
            TMP_Text texts = usedLetterTexts[i];
            var textsChar = char.Parse(texts.text);
            submittedWord += textsChar;

            texts.color = wrongColor;

            if (currentWord.Contains(textsChar))
            {
                texts.color = inWordColor;

                if (textsChar == currentWord[ii])
                    texts.color = perfectMatchColor;
            }

            ii++;
        }

        print(submittedWord);

        if (words.Contains(submittedWord))
        {
            if(submittedWord == currentWord)
            {
                congratulations.SetActive(true);
                print("success!");
                return;
            }

            print("reset count");

            currentSubmit++;
            currentCount = 0;
        }
        else
        {
            print("fail!");

            for (int i = 0 + (currentSubmit * 5); i < usedLetterTexts.Count; i++)
            {
                TMP_Text texts = usedLetterTexts[i];

                texts.color = Color.white;
            }
        }

        if(currentSubmit == 6)
        {
            retry.SetActive(true);
            retryText.text = currentWord;
        }
    }

    public void ResetPuzzle()
    {
        retry.SetActive(false);
        congratulations.SetActive(false);

        usedLetterTexts.Clear();

        foreach (var texts in allLetterTexts)
        {
            texts.text = string.Empty;
            texts.color = Color.white;
        }

        currentSubmit = 0;
        currentCount = 0;

        currentWord = words[UnityEngine.Random.Range(0, words.Count)];
    }
}
