using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextScroll : MonoBehaviour
{
    [SerializeField]
    private GameObject upButton;
    [SerializeField]
    private GameObject downButton;
    [SerializeField]
    private RectTransform clueText;

    public bool isEndOfContentList;

    // Start is called before the first frame update
    void Awake()
    {
        upButton.SetActive(false);
        downButton.SetActive(false);
        isEndOfContentList = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (clueText.rect.height - clueText.localPosition.y >= 124.8f
            && clueText.rect.height - clueText.localPosition.y <= 125.2f)
        {
            isEndOfContentList = true;
            downButton.SetActive(false);
        } else
        {
            isEndOfContentList = false;
        }

        if (clueText.rect.height >= 250)
        {
            if (!isEndOfContentList)
                downButton.SetActive(true);
        }

        if(clueText.localPosition.y <= 126)
        {
            upButton.SetActive(false);
        } else
        {
            upButton.SetActive(true);
        }
    }

    public void UpButton()
    {
        float yPos = clueText.localPosition.y;
        yPos -= 23.0f;
        clueText.localPosition = new Vector2(clueText.localPosition.x, yPos);
    }

    public void DownButton()
    {
        float yPos = clueText.localPosition.y;
        yPos += 23.0f;
        clueText.localPosition = new Vector2(clueText.localPosition.x, yPos);
    }
}
