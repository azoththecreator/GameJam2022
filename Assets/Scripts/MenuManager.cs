using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance = null;

    [SerializeField] Transform diceGod;
    [SerializeField] Transform halo;
    [SerializeField] GameObject fade;
    [SerializeField] Color fadeColor;

    [SerializeField] RectTransform cursor;

    [SerializeField] GameObject music;

    [SerializeField] Transform logo;

    [SerializeField] AudioSource button;

    [SerializeField] Image block;
    [SerializeField] GameObject instruction;
    [SerializeField] Transform instructionPaper;
    [SerializeField] Canvas canvas;
    public Transform tooltip;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        DontDestroyOnLoad(music);

        Cursor.visible = false;

        logo.DOLocalMoveY(100, 2).SetDelay(.5f).SetEase(Ease.OutBounce);
        diceGod.DOLocalMoveY(-400, 2).SetDelay(2);
        halo.GetComponent<Image>().DOColor(Color.white, 3).SetDelay(4);
        halo.DORotate(new Vector3(0, 0, -360), 10, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
    }

    private void Update()
    {
        Vector2 movePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out movePos);
        tooltip.position = transform.TransformPoint(movePos);

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursor.position = mousePos;
    }

    public void StartGame()
    {
        fade.SetActive(true);
        fade.GetComponent<Image>().DOColor(fadeColor, 2);
        button.Play();
        Invoke("Load", 2.5f);
    }

    void Load()
    {
        SceneManager.LoadScene(1);
    }

    public void HowTo()
    {
        button.Play();
        block.gameObject.SetActive(true); 
        block.DOColor(new Color(8 / 255f, 35 / 255f, 55 / 255f, 200 / 255f), .5f);
        instruction.SetActive(true);
        instructionPaper.localPosition = new Vector3(0, -900, 0);

    }

    public void Close()
    {
        StartCoroutine(CloseInstruction());
    }

    IEnumerator CloseInstruction()
    {
        button.Play();
        instructionPaper.DOLocalMoveY(-900, .2f);
        block.DOColor(new Color(8 / 255f, 35 / 255f, 55 / 255f, 0 / 255f), .5f);
        yield return new WaitForSeconds(.15f);
        instruction.SetActive(false);
        yield return new WaitForSeconds(.4f);
        block.gameObject.SetActive(false);
    }
}
