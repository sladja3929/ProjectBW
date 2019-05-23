using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroll : MonoBehaviour
{
    [SerializeField]
    private RectTransform list; //움직일 오브젝트
    [SerializeField]    
    private int count;          //나눠야할 값
    [SerializeField]
    private GameObject upButton;
    [SerializeField]
    private GameObject downButton;

    private float pos;          //list의 localposition
    private float movepos;      //움직일 값
    public bool isScroll = false;  //움직여야 하는지 구별
    public bool isEndOfClueList = false;   //스크롤이 제일 아래로 내려갔는지 구별

    public static Scroll instance = null;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        pos = list.localPosition.y;
        movepos = list.rect.yMin - list.rect.yMin / count;
        
    }

    // Update is called once per frame
    void Update()
    {
        /* 단서 리스트 끝에 다다르면 downButton 비활성화 */
        //if (list.rect.height - list.localPosition.y >= 274.8f && list.rect.height - list.localPosition.y <= 275.2f)
        //{
        //    isEndOfClueList = true;
        //    downButton.SetActive(false);
        //    isScroll = false;
        //} else
        //{
        //    isEndOfClueList = false;
        //}

        /* 단서창을 열었을때, 단서가 일정갯수 이상이면 downButton 활성화 */
        //if (list.rect.height >= 600)
        //{
        //    if (!isEndOfClueList)
        //        downButton.SetActive(true);
        //}

        
        //if (list.localPosition.y <= 280)
        //{
        //    isScroll = false;
        //    //upButton.SetActive(false);
        //} else
        //{
        //    //upButton.SetActive(true);
        //}

        //도중에 마우스 휠 할경우 isScroll = false;
        //if(Input.GetAxis("Mouse ScrollWheel") < 0 || Input.GetAxis("Mouse ScrollWheel") > 0)
        //{
        //    isScroll = false;
        //}
    }

    public void Up()
    {
        if (list.rect.yMax + list.rect.yMin / count == movepos)
        {

        }
        else
        {
            isEndOfClueList = false;
            isScroll = true;
            pos = list.localPosition.y;
            movepos = pos - 80;//list.rect.height / count;
            pos = movepos;
            StartCoroutine(scroll());
        }
    }

    public void Down()
    {
        if (list.rect.yMin - list.rect.yMin / count == movepos)
        {

        }
        else
        {
            upButton.SetActive(true);
            isScroll = true;
            pos = list.localPosition.y;
            movepos = pos + 80; //list.rect.height / count;
            pos = movepos;
            StartCoroutine(scroll());
        }
    }
    

    IEnumerator scroll()
    {
        while (isScroll)
        {
            list.localPosition = Vector2.Lerp(list.localPosition, new Vector2(0, movepos), Time.deltaTime * 5);
            if (Vector2.Distance(list.localPosition, new Vector2(0, movepos)) < 1.0f)
            {
                isScroll = false;
            }
            yield return null;
        }
    }

    public void ActivateUpButton(bool setBool)
    {
        upButton.SetActive(setBool);
    }

    public void ActivateDownButton(bool setBool)
    {
        downButton.SetActive(setBool);
    }
}
