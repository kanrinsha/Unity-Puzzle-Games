using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImagePuzzle : MonoBehaviour
{
    public int currentPuzzle = 1;

    public List<Texture2D> puzzleSprites;

    public List<RawImage> images = new List<RawImage>();
    public List<RectTransform> imagesTransforms = new List<RectTransform>();
    public TMP_Text completeText;

    public Image backgroundImage;

    public Sprite completedAllPuzzlesBackground;

    private RawImage disabledImage;
    private bool hasStarted = false;

    private List<string> correctImageOrder = new List<string>();

    private void Awake()
    {
        foreach (var image in images)
        {
            image.GetComponent<Button>().onClick.AddListener(() => ButtonClicked(image));
        }
    }

    private void ButtonClicked(RawImage image)
    {
        if (!hasStarted)
            return;

        MoveLogic(image);
    }

    private void MoveLogic(RawImage clickedImage)
    {
        int clickedIndex = clickedImage.transform.GetSiblingIndex();

        print(clickedIndex + " | " + CanMove(clickedIndex));

        if (CanMove(clickedIndex))
        {
            var clickedImageName = clickedImage.name;
            var disabledImageName = disabledImage.name;

            clickedImage.enabled = false;
            clickedImage.GetComponent<Button>().enabled = false;

            disabledImage.texture = clickedImage.texture;
            disabledImage.enabled = true;
            disabledImage.GetComponent<Button>().enabled = true;

            disabledImage.name = clickedImageName;
            clickedImage.name = disabledImageName;

            disabledImage = clickedImage;

            CheckPuzzleComplete();
        }
    }

    private void CheckPuzzleComplete()
    {
        for (int i = 0; i < images.Count; i++)
        {
            var image = images[i];
            if (image.name != correctImageOrder[i])
                return;
        }

        print("correct");

        SplitSpriteToImages();

        disabledImage.enabled = true;
        completeText.gameObject.SetActive(true);
        hasStarted = false;
    }

    private bool CanMove(int clickedIndex)
    {
        switch (disabledImage.transform.GetSiblingIndex())
        {
            case 0:
                if (clickedIndex == 1 || clickedIndex == 3)
                    return true;
                break;
            case 1:
                if (clickedIndex == 0 || clickedIndex == 2 || clickedIndex == 4)
                    return true;
                break;
            case 2:
                if (clickedIndex == 1 || clickedIndex == 5)
                    return true;
                break;
            case 3:
                if (clickedIndex == 0 || clickedIndex == 4 || clickedIndex == 6)
                    return true;
                break;
            case 4:
                if (clickedIndex == 1 || clickedIndex == 3 || clickedIndex == 5 || clickedIndex == 7)
                    return true;
                break;
            case 5:
                if (clickedIndex == 2 || clickedIndex == 4 || clickedIndex == 8)
                    return true;
                break;
            case 6:
                if (clickedIndex == 3 || clickedIndex == 7)
                    return true;
                break;
            case 7:
                if (clickedIndex == 4 || clickedIndex == 6 || clickedIndex == 8)
                    return true;
                break;
            case 8:
                if (clickedIndex == 7 || clickedIndex == 5)
                    return true;
                break;
        }

        return false;
    }

    public void StartPuzzle()
    {
        currentPuzzle++;

        if (currentPuzzle > puzzleSprites.Count)
        {
            print("No more puzzles.");
            for (int i = 0; i < images.Count; i++)
            {
                RawImage image = images[i];
                image.enabled = false;
            }

            backgroundImage.sprite = completedAllPuzzlesBackground;

            completeText.text = "All Puzzles Complete!";
            return;
        }

        completeText.gameObject.SetActive(false);

        SplitSpriteToImages();
        ResetImages();

        correctImageOrder.Clear();

        for (int i = 0; i < images.Count; i++)
        {
            RawImage image = images[i];
            correctImageOrder.Add(image.name);
        }

        RandomizeImages();
        DisableRandomImage();

        hasStarted = true;
    }

    [ContextMenu("Split Sprite")]
    public void SplitSpriteToImages()
    {
        var puzzleSprite = puzzleSprites[currentPuzzle - 1];

        for (int i = 0; i < images.Count; i++)
        {
            RawImage image = images[i];
            Texture2D tex = new Texture2D(100, 100, TextureFormat.RGBA32, false);

            if (i < 3)
            {
                var pixels = puzzleSprite.GetPixels(i * 100, puzzleSprite.height - 100, 100, 100);
                tex.SetPixels(pixels, 0);
            }
            else if(i < 6)
            {
                var pixels = puzzleSprite.GetPixels((i - 3) * 100, puzzleSprite.height - 200, 100, 100);
                tex.SetPixels(pixels, 0);
            }
            else if (i < 9)
            {
                var pixels = puzzleSprite.GetPixels((i - 6) * 100, puzzleSprite.height - 300, 100, 100);
                tex.SetPixels(pixels, 0);
            }

            tex.Apply();
            image.texture = tex;
        }
    }

    [ContextMenu("Reset Images")]
    public void ResetImages()
    {
        for (int i = 0; i < images.Count; i++)
        {
            var image = images[i];
            image.transform.SetSiblingIndex(transform.childCount - 1);
            image.enabled = true;
            image.GetComponent<Button>().enabled = true;
        }
    }

    [ContextMenu("Randomize Images")]
    public void RandomizeImages()
    {
        for (int i = 0; i < images.Count; i++)
        {
            int selectedIndex = Random.Range(0, images.Count - 1);
            var image = images[selectedIndex];
            image.transform.SetSiblingIndex(transform.childCount - 1);
        }
    }

    [ContextMenu("Disable Random Image")]
    public void DisableRandomImage()
    {
        int selectedIndex = Random.Range(0, images.Count - 1);
        var image = images[selectedIndex];
        image.enabled = false;
        image.GetComponent<Button>().enabled = false;

        disabledImage = image;
    }
}
