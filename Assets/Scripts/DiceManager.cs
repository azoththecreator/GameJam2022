using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class DiceManager : MonoBehaviour
{
    GameManager gameManager;

    [SerializeField] List<Sprite> sprites;
    int spriteIndex = 0;
    Image image;
    public int index, rollIndex;
    int rollNumber = 0;

    public List<int> eyeList;
    public int eye;

    [SerializeField] TextMeshProUGUI tooltip;

    [SerializeField] Image selectedImage;
    [SerializeField] Color highlightedColor;
    [SerializeField] Color selectedColor;
    [SerializeField] Color transparentColor;

    public bool selected = false;

    [SerializeField] AudioSource sfx;

    private void Start()
    {
        gameManager = GameManager.instance;

        image = GetComponent<Image>();
        index = transform.GetSiblingIndex();
        rollIndex = index;

        eyeList = new List<int> { 1, 2, 3, 4, 5, 6 };
    }

    public void ResetIndex()
    {
        index = transform.GetSiblingIndex();
        rollIndex = index;
    }
    
    public IEnumerator Roll()
    {
        gameManager.rolling = true;
        image.sprite = sprites[spriteIndex];
        yield return new WaitForSeconds(.1f);
        spriteIndex++;
        if (spriteIndex.Equals(6))
        {
            spriteIndex = 0;
            rollNumber++;
        }

        if (rollNumber < 3 + rollIndex)
            StartCoroutine(Roll());
        else
        {
            sfx.pitch = Random.Range(.7f, 1);
            sfx.Play();
            rollNumber = 0;
            eye = eyeList[Random.Range(0, eyeList.Count)];
            image.sprite = sprites[eye - 1];

            switch(eye)
            {
                case 1:
                    eyeList.Add(2);
                    eyeList.Add(3);
                    eyeList.Add(4);
                    eyeList.Add(5);
                    eyeList.Add(6);
                    eyeList.Add(2);
                    eyeList.Add(3);
                    eyeList.Add(4);
                    eyeList.Add(5);
                    eyeList.Add(6);
                    break;
                case 2:
                    eyeList.Add(1);
                    eyeList.Add(3);
                    eyeList.Add(4);
                    eyeList.Add(5);
                    eyeList.Add(6);
                    eyeList.Add(1);
                    eyeList.Add(3);
                    eyeList.Add(4);
                    eyeList.Add(5);
                    eyeList.Add(6);
                    break;
                case 3:
                    eyeList.Add(1);
                    eyeList.Add(2);
                    eyeList.Add(4);
                    eyeList.Add(5);
                    eyeList.Add(6);
                    eyeList.Add(1);
                    eyeList.Add(2);
                    eyeList.Add(4);
                    eyeList.Add(5);
                    eyeList.Add(6);
                    break;
                case 4:
                    eyeList.Add(1);
                    eyeList.Add(2);
                    eyeList.Add(3);
                    eyeList.Add(5);
                    eyeList.Add(6);
                    eyeList.Add(1);
                    eyeList.Add(2);
                    eyeList.Add(3);
                    eyeList.Add(5);
                    eyeList.Add(6);
                    break;
                case 5:
                    eyeList.Add(1);
                    eyeList.Add(2);
                    eyeList.Add(3);
                    eyeList.Add(4);
                    eyeList.Add(6);
                    eyeList.Add(1);
                    eyeList.Add(2);
                    eyeList.Add(3);
                    eyeList.Add(4);
                    eyeList.Add(6);
                    break;
                case 6:
                    eyeList.Add(1);
                    eyeList.Add(2);
                    eyeList.Add(3);
                    eyeList.Add(4);
                    eyeList.Add(5);
                    eyeList.Add(1);
                    eyeList.Add(2);
                    eyeList.Add(3);
                    eyeList.Add(4);
                    eyeList.Add(5);
                    break;
            }

            string chance1String, chance2String, chance3String, chance4String, chance5String, chance6String;

            float chance1 = 100f / eyeList.Count * eyeList.FindAll(x => x == 1).Count;
            float chance2 = 100f / eyeList.Count * eyeList.FindAll(x => x == 2).Count;
            float chance3 = 100f / eyeList.Count * eyeList.FindAll(x => x == 3).Count;
            float chance4 = 100f / eyeList.Count * eyeList.FindAll(x => x == 4).Count;
            float chance5 = 100f / eyeList.Count * eyeList.FindAll(x => x == 5).Count;
            float chance6 = 100f / eyeList.Count * eyeList.FindAll(x => x == 6).Count;

            if (chance1 * 100 % 1 != 0)
            {
                chance1 = Mathf.Floor(chance1 * 100) / 100f;
                chance1String = chance1.ToString() + "...%";
            }
            else
                chance1String = chance1.ToString() + "%";

            if (chance2 * 100 % 1 != 0)
            {
                chance2 = Mathf.Floor(chance2 * 100) / 100f;
                chance2String = chance2.ToString() + "...%";
            }
            else
                chance2String = chance2.ToString() + "%";

            if (chance3 * 100 % 1 != 0)
            {
                chance3 = Mathf.Floor(chance3 * 100) / 100f;
                chance3String = chance3.ToString() + "...%";
            }
            else
                chance3String = chance3.ToString() + "%";

            if (chance4 * 100 % 1 != 0)
            {
                chance4 = Mathf.Floor(chance4 * 100) / 100f;
                chance4String = chance4.ToString() + "...%";
            }
            else
                chance4String = chance4.ToString() + "%";

            if (chance5 * 100 % 1 != 0)
            {
                chance5 = Mathf.Floor(chance5 * 100) / 100f;
                chance5String = chance5.ToString() + "...%";
            }
            else
                chance5String = chance5.ToString() + "%";

            if (chance6 * 100 % 1 != 0)
            {
                chance6 = Mathf.Floor(chance6 * 100) / 100f;
                chance6String = chance6.ToString() + "...%";
            }
            else
                chance6String = chance6.ToString() + "%";

            float chanceMin = Mathf.Min(chance1, chance2, chance3, chance4, chance5, chance6);
            float chanceMax = Mathf.Max(chance1, chance2, chance3, chance4, chance5, chance6);
            if (chanceMax != chanceMin)
            {
                if (chance1.Equals(chanceMin))
                    chance1String = "<#C2482F>" + chance1String + "</color>";
                else if (chance1.Equals(chanceMax))
                    chance1String = "<#3499AE>" + chance1String + "</color>";
                if (chance2.Equals(chanceMin))
                    chance2String = "<#C2482F>" + chance2String + "</color>";
                else if (chance2.Equals(chanceMax))
                    chance2String = "<#3499AE>" + chance2String + "</color>";
                if (chance3.Equals(chanceMin))
                    chance3String = "<#C2482F>" + chance3String + "</color>";
                else if (chance3.Equals(chanceMax))
                    chance3String = "<#3499AE>" + chance3String + "</color>";
                if (chance4.Equals(chanceMin))
                    chance4String = "<#C2482F>" + chance4String + "</color>";
                else if (chance4.Equals(chanceMax))
                    chance4String = "<#3499AE>" + chance4String + "</color>";
                if (chance5.Equals(chanceMin))
                    chance5String = "<#C2482F>" + chance5String + "</color>";
                else if (chance5.Equals(chanceMax))
                    chance5String = "<#3499AE>" + chance5String + "</color>";
                if (chance6.Equals(chanceMin))
                    chance6String = "<#C2482F>" + chance6String + "</color>";
                else if (chance6.Equals(chanceMax))
                    chance6String = "<#3499AE>" + chance6String + "</color>";
            }

            tooltip.text = chance1String + "\n" + chance2String + "\n" + chance3String + "\n" + chance4String + "\n" + chance5String + "\n" + chance6String;

            if (selected)
                Select();

            gameManager.RollResult(index, eye);
        }
    }

    public void PointerEnter()
    {
        gameManager.TooltipOn(this.name);

        if (gameManager.reroll && !selected && gameManager.remainDice > 0 && !gameManager.rolling)
            selectedImage.DOColor(highlightedColor, .1f);
    }

    public void PointerExit()
    {
        gameManager.TooltipOff();

        if (gameManager.reroll && !selected && gameManager.remainDice > 0 && !gameManager.rolling)
            selectedImage.DOColor(transparentColor, .1f);
    }

    public void Select()
    {
        if (gameManager.reroll && gameManager.remainDice > 0)
        {
            if (!selected)
            {
                if (!gameManager.rolling)
                {
                    selected = true;
                    selectedImage.DOColor(selectedColor, .1f);
                    gameManager.dices.Add(this);
                    gameManager.RollButtonUp();
                    gameManager.SubmitButtonDown();
                }
            }
            else
            {
                selected = false;
                selectedImage.DOColor(transparentColor, .1f);
                gameManager.dices.Remove(this);
                if (gameManager.dices.Count == 0)
                {
                    gameManager.RollButtonDown();
                    gameManager.SubmitButtonUp();
                }
            }
        }
    }

    public void UpdateIndex(int i)
    {
        rollIndex = i;
    }
}
