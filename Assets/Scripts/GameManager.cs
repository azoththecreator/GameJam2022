using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    Canvas canvas;
    [SerializeField] RectTransform cursor;
    [SerializeField] Transform tooltip;

    [SerializeField] ButtonManager rollButton, submitButton;

    public GameObject selectedArea;
    public int remainDice = 3;
    [SerializeField] TextMeshProUGUI remainDiceText;
    string remainDiceDefault = " more left!";
    public bool reroll = false;
    string rollString = "Roll";
    public bool rolling = false;

    [SerializeField] DiceManager diceManager1, diceManager2, diceManager3, diceManager4, diceManager5;
    public List<DiceManager> dices;

    [SerializeField] List<GameObject> tooltips;
    GameObject currentTooltip;
    [SerializeField] GameObject alert1, alert2;
    float timer;

    Camera mainCam;
    Vector3 zoomPos = new Vector3(-3.85f, 4, -10);
    float zoomSize = 2;
    Vector3 defaultPos = new Vector3(0, 0, -10);
    float defaultSize = 5;

    public List<int> score = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    int submitted = 0;

    [SerializeField] Image fade;
    [SerializeField] GameObject result;
    [SerializeField] RectTransform resultPaper;
    [SerializeField] List<TextMeshProUGUI> scoreTexts;
    [SerializeField] TextMeshProUGUI bonusText;
    bool bonus = false;
    int resultScore;
    [SerializeField] TextMeshProUGUI resultScoreText;
    int scoreCount = 0;

    [SerializeField] AudioSource impact;

    WaitForSeconds waitHalf, waitOne;
    [SerializeField] GameObject block;

    [SerializeField] TextMeshProUGUI resultText;

    [SerializeField] AudioSource audio;
    [SerializeField] AudioClip gameClear, gameOver;
    [SerializeField] ButtonManager restartButton;
    [SerializeField] Color fadeColor;
    [SerializeField] Image fadeOut;

    [SerializeField] GameObject confetti;
    [SerializeField] AudioSource confettiSound;

    private void Awake()
    {
        instance = this;

        canvas = GetComponent<Canvas>();
    }

    private void Start()
    {
        Cursor.visible = false;

        mainCam = Camera.main;
        mainCam.transform.DOMoveY(0, 1).SetDelay(.5f);

        waitHalf = new WaitForSeconds(.5f);
        waitOne = new WaitForSeconds(1);
    }

    private void Update()
    {
        Vector2 movePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out movePos);
        tooltip.position = transform.TransformPoint(movePos);
        
        Vector2 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        cursor.position = mousePos;
        
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = 0;
            alert1.SetActive(false);
            alert2.SetActive(false);
        }
    }

    public void RollButtonUp()
    {
        rollButton.ButtonUp();
    }

    public void RollButtonDown()
    {
        rollButton.ButtonDown();
    }

    public void SubmitButtonUp()
    {
        submitButton.ButtonUp();

        if (remainDice.Equals(3))
                remainDiceText.text = "";
        else
                remainDiceText.text = remainDice.ToString() + remainDiceDefault;
    }

    public void SubmitButtonDown()
    {
        submitButton.ButtonDown();
    }

    public void UpdateText(string str)
    {
        rollButton.UpdateText(str);
        remainDiceText.text = "";
    }

    public void ResetArea(GameObject selected)
    {
        GameObject prevSelected = selectedArea;
        selectedArea = selected;
        if (prevSelected != null)
            prevSelected.GetComponent<AreaManager>().PointerExit();
    }

    public void Roll()
    {
        mainCam.DOOrthoSize(zoomSize, .5f).SetDelay(1);
        mainCam.transform.DOMove(zoomPos, .5f).SetDelay(1);

        if (!reroll)
        {
            StartCoroutine(diceManager1.Roll());
            StartCoroutine(diceManager2.Roll());
            StartCoroutine(diceManager3.Roll());
            StartCoroutine(diceManager4.Roll());
            StartCoroutine(diceManager5.Roll());
        }
        else
        {
            for (int i = 0; i < dices.Count; i++)
            {
                dices = dices.OrderBy(o => o.name).ToList();
                dices[i].UpdateIndex(i);
                StartCoroutine(dices[i].Roll());
            }
        }
    }

    public void RollResult(int diceNum, int eye)
    {
        selectedArea.GetComponent<AreaManager>().RollResult(diceNum, eye);
    }

    public IEnumerator CameraReset()
    {
        yield return waitOne;
        mainCam.DOOrthoSize(defaultSize, 1);
        mainCam.transform.DOMove(defaultPos, 1);
    }

    public void Submit()
    {
        mainCam.DOShakePosition(.5f, .3f);
        impact.Play();

        remainDice = 3;
        reroll = false;

        diceManager1.ResetIndex();
        diceManager2.ResetIndex();
        diceManager3.ResetIndex();
        diceManager4.ResetIndex();
        diceManager5.ResetIndex();

        selectedArea.GetComponent<AreaManager>().Submitted(diceManager1.eye, diceManager2.eye, diceManager3.eye, diceManager4.eye, diceManager5.eye);

        SubmitButtonDown();
        UpdateText(rollString);

        submitted++;
        if (submitted.Equals(13))///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        {
            Invoke("Result", 2);
            GameObject.Find("Music").GetComponent<AudioSource>().DOFade(0, 3);
        }
    }

    public void TooltipOn(string name)
    {
        currentTooltip = tooltips.Find(x => x.name == name);
        currentTooltip.SetActive(true);
        timer = 0;
    }

    public void TooltipOff()
    {
        currentTooltip.SetActive(false);
    }

    public void Result()
    {
        fade.gameObject.SetActive(true);
        block.gameObject.SetActive(true);
        fade.DOColor(new Color(8 / 255f, 35 / 255f, 55 / 255f, 200 / 255f), .5f);
        result.SetActive(true);
        resultPaper.localPosition = new Vector3(0, -900, 0);

        for (int i = 0; i < scoreTexts.Count; i++)
        {
            scoreTexts[i].text = score[i].ToString();
            scoreTexts[i].gameObject.SetActive(false);
        }
        if (score[0] + score[1] + score[2] + score[3] + score[4] + score[5] >= 63)
        {
            bonus = true;
            bonusText.text = "35";
        }
        else
            bonusText.text = "0";
        bonusText.gameObject.SetActive(false);

        for (int i = 0; i < score.Count; i++)
            resultScore += score[i];
        if (bonus)
            resultScore += 35;
        resultScoreText.text = resultScore.ToString();
        resultScoreText.gameObject.SetActive(false);

        StartCoroutine(ResultScore());
    }

    IEnumerator ResultScore()
    {
        yield return waitHalf;
        if (scoreCount.Equals(7))
        {
            resultPaper.DOLocalMoveY(0, .5f).SetEase(Ease.Linear);
            yield return waitOne;
        }

        GameObject active = null;

        if (scoreCount < 6)
        {
            active = scoreTexts[scoreCount].gameObject;
        }
        else if (scoreCount > 6)
        {
            active = scoreTexts[scoreCount - 1].gameObject;
        }
        else if (scoreCount.Equals(6))
        {
            active = bonusText.gameObject;
        }
        impact.Play();

        if (scoreCount.Equals(0) || scoreCount.Equals(7))
        {
            resultPaper.DOLocalMoveY(587f, 3).SetEase(Ease.Linear).SetRelative();
        }
        float randomAngle = Random.Range(-10, 10);
        active.transform.DORotate(Vector3.forward * randomAngle, 0);
        active.SetActive(true);

        scoreCount++;

        if (scoreCount < 14)
            StartCoroutine(ResultScore());
        else
        {
            yield return waitHalf;
            StartCoroutine(FinalResult());
        }
    }

    IEnumerator FinalResult()
    {
        yield return waitHalf;
        resultPaper.DOLocalMoveY(0, .5f).SetEase(Ease.Linear);
        yield return waitOne;

        resultScoreText.gameObject.SetActive(true);
        impact.Play();

        block.SetActive(false);
        restartButton.ButtonUp();

        if (resultScore >= 150)
        {
            confetti.SetActive(true);
            StartCoroutine(Confetti());

            audio.clip = gameClear;
            audio.Play();
            audio.DOFade(.8f, 1);
        }
        else
        {
            int randomText = Random.Range(0, 4);
            switch (randomText)
            {
                case 0:
                    resultText.text = "Try Again!";
                    break;
                case 1:
                    resultText.text = "That was close!";
                    resultText.transform.DOScaleX(.8f, 0);
                    break;
                case 2:
                    resultText.text = "You can do it!";
                    resultText.transform.DOScaleX(.9f, 0);
                    break;
                case 3:
                    resultText.text = "Maybe Next Time";
                    resultText.transform.DOScaleX(.7f, 0);
                    break;
            }
            yield return waitOne;
            resultText.transform.parent.gameObject.SetActive(true);
            impact.Play();

            audio.clip = gameOver;
            audio.Play();
            audio.DOFade(.8f, 2);
        }
    }

    IEnumerator Confetti()
    {
        confettiSound.Play();
        yield return waitHalf;
        StartCoroutine(Confetti());
    }
    
    public void Alert(int i)
    {
        timer = 1.5f;
        if (i.Equals(1))
        {
            alert1.SetActive(true);
        }
        if (i.Equals(2))
        {
            alert2.SetActive(true);
        }
    }

    public void Restart()
    {
        StartCoroutine(RestartLevel());
    }

    IEnumerator RestartLevel()
    {
        rollButton.button.Play();
        audio.DOFade(0, 1);
        confettiSound.DOFade(0, 1);
        fadeOut.gameObject.SetActive(true);
        fadeOut.DOColor(fadeColor, 1);
        yield return waitOne;
        SceneManager.LoadScene(1);
    }
}
