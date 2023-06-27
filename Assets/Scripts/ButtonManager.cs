using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    GameManager gameManager;

    [SerializeField] RectTransform buttonTransform;
    [SerializeField] TextMeshProUGUI buttonText;

    public bool clickable;

    public AudioSource button;

    private void Start()
    {
        gameManager = GameManager.instance;
    }

    public void ButtonUp()
    {
        buttonTransform.DOAnchorPosY(0, .1f);
        clickable = true;
    }

    public void Hold()
    {
        if (clickable)
            buttonTransform.DOAnchorPosY(-35, .1f);
    }

    public void Released()
    {
        if (clickable)
            buttonTransform.DOAnchorPosY(0, .1f);
    }

    public void Roll()
    {
        if (clickable)
        {
            button.Play();
            gameManager.Roll();
            ButtonDown();
            gameManager.reroll = true;
        }
        else
        {
            if (gameManager.remainDice < 3)
                gameManager.Alert(2);
            else
                gameManager.Alert(1);
        }
    }

    public void Submit()
    {
        if (clickable)
        gameManager.Submit();
    }

    public void ButtonDown()
    {
        buttonTransform.DOAnchorPosY(-35, .1f);
        clickable = false;
    }

    public void UpdateText(string str)
    {
            buttonText.text = str;
    }
}
