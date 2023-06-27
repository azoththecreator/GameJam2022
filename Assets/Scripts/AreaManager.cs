using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AreaManager : MonoBehaviour
{
    GameManager gameManager;

    Image selected;
    [SerializeField] Color highlightedColor;
    [SerializeField] Color selectedColor;
    [SerializeField] Color transparentColor;
    [SerializeField] Color submittedColor;

    [SerializeField] List<Sprite> diceSprites;
    [SerializeField] List<Image> dice;
    public bool isFixed = false;

    [SerializeField] List<Image> fixSprites;

    string rerollString = "Reroll";

    public List<int> eyes;
    int score = 0;

    [SerializeField] Transform submittedArea;

    private void Start()
    {
        gameManager = GameManager.instance;
        selected = transform.GetChild(0).GetComponent<Image>();
    }

    public void PointerEnter()
    {
        if (gameManager != null)
        {
            if (gameManager.selectedArea != gameObject && !isFixed && !gameManager.reroll)
                selected.DOColor(highlightedColor, .1f);

            gameManager.TooltipOn(this.name);
        }
        else
        {
            selected.DOColor(highlightedColor, .1f);
            MenuManager.instance.tooltip.gameObject.SetActive(true);
        }
    }

    public void PointerExit()
    {
        if (gameManager != null)
        {
            if (gameManager.selectedArea != gameObject && !isFixed && !gameManager.reroll)
                selected.DOColor(transparentColor, .1f);

            gameManager.TooltipOff();
        }
        else
        {
            selected.DOColor(transparentColor, .1f);
            MenuManager.instance.tooltip.gameObject.SetActive(false);
        }
    }

    public void Selected()
    {
        if (!isFixed && !gameManager.reroll)
        {
            selected.DOColor(selectedColor, .1f);
            gameManager.RollButtonUp();
            gameManager.ResetArea(gameObject);
        }
    }

    public void RollResult(int diceNum, int eye)
    {
        dice[diceNum].sprite = diceSprites[eye - 1];
        dice[diceNum].DOColor(Color.white, 0);

        if ((gameManager.dices.Count.Equals(0) && gameManager.remainDice < 3) || diceNum.Equals(4))
        {
            gameManager.reroll = true;
            gameManager.UpdateText(rerollString);
            gameManager.remainDice--;
            gameManager.SubmitButtonUp();
            StartCoroutine(gameManager.CameraReset());
            gameManager.rolling = false;
        }
    }

    public void Submitted(int eye1, int eye2, int eye3, int eye4, int eye5)
    {
        isFixed = true;
        for (int i = 0; i < fixSprites.Count; i++)
        {
            fixSprites[i].DOColor(submittedColor, .1f);
        }

        selected.gameObject.SetActive(false);
        gameManager.selectedArea = null;

        eyes.Add(eye1);
        eyes.Add(eye2);
        eyes.Add(eye3);
        eyes.Add(eye4);
        eyes.Add(eye5);

        ScoreCalculate();
    }

    void ScoreCalculate()
    {
        switch(this.name)
        {
            case "AreaAce":
                for (int i = 0; i < eyes.Count; i++)
                {
                    if (eyes[i].Equals(1))
                        score += 1;
                }
                break;
            case "AreaTwo":
                for (int i = 0; i < eyes.Count; i++)
                {
                    if (eyes[i].Equals(2))
                        score += 2;
                }
                break;
            case "AreaThree":
                for (int i = 0; i < eyes.Count; i++)
                {
                    if (eyes[i].Equals(3))
                        score += 3;
                }
                break;
            case "AreaFour":
                for (int i = 0; i < eyes.Count; i++)
                {
                    if (eyes[i].Equals(4))
                        score += 4;
                }
                break;
            case "AreaFive":
                for (int i = 0; i < eyes.Count; i++)
                {
                    if (eyes[i].Equals(5))
                        score += 5;
                }
                break;
            case "AreaSix":
                for (int i = 0; i < eyes.Count; i++)
                {
                    if (eyes[i].Equals(6))
                        score += 6;
                }
                break;


            case "Area3Kind":
                eyes.Sort();
                if ((eyes[0] == eyes[1] && eyes[0] == eyes[2]) || (eyes[1] == eyes[2] && eyes[1] == eyes[3]) || (eyes[2] == eyes[3] && eyes[2] == eyes[4]))
                    for (int i = 0; i < eyes.Count; i++)
                        score += eyes[i];
                break;
            case "Area4Kind":
                eyes.Sort();
                if ((eyes[0] == eyes[1] && eyes[0] == eyes[2] && eyes[0] == eyes[3]) || (eyes[1] == eyes[2] && eyes[1] == eyes[3] && eyes[1] == eyes[4]))
                    for (int i = 0; i < eyes.Count; i++)
                        score += eyes[i];
                break;
            case "AreaFullHouse":
                eyes.Sort();
                if ((eyes[0] == eyes[1] && eyes[2] == eyes[3] && eyes[2] == eyes[4]) || (eyes[0] == eyes[1] && eyes[0] == eyes[2] && eyes[3] == eyes[4]))
                    score += 25;
                break;
            case "AreaSmall":
                if ((eyes.Contains(1) && eyes.Contains(2) && eyes.Contains(3) && eyes.Contains(4)) || (eyes.Contains(5) && eyes.Contains(2) && eyes.Contains(3) && eyes.Contains(4)) || (eyes.Contains(6) && eyes.Contains(5) && eyes.Contains(3) && eyes.Contains(4)))
                    score += 30;
                break;
            case "AreaLarge":
                if (eyes.Contains(1) && eyes.Contains(2) && eyes.Contains(3) && eyes.Contains(4) && eyes.Contains(5) || eyes.Contains(6) && eyes.Contains(2) && eyes.Contains(3) && eyes.Contains(4) && eyes.Contains(5))
                    score += 40;
                break;
            case "AreaYahtzee":
                if (eyes[0] == eyes[1] && eyes[2] == eyes[3] && eyes[0] == eyes[2] && eyes[0] == eyes[4])
                    score += 50;
                break;
            case "AreaChance":
                for (int i = 0; i < eyes.Count; i++)
                    score += eyes[i];
                break;
        }

        gameManager.score[transform.GetSiblingIndex()] = score;
    }
}
