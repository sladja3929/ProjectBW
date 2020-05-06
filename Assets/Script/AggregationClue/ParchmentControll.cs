using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParchmentControll : MonoBehaviour
{
    private double maxScrollPosition;   // 스크롤할 수 있는 최대치 높이
    private double minScrollPosition;   // 스크롤할 수 있는 최소치 높이
    private bool isPlayingDocumentAnim; // 안드렌 서류 타임라인 애니메이션이 실행되고 있는지를 알기 위한 변수
    public bool atOnce;

    [SerializeField]
    private RectTransform rect_Parchment;   // 제어할 양피지의 RectTransform 컴포넌트
    [SerializeField]
    private RectTransform rect_Top_Parchment;
    [SerializeField]
    private RectTransform rect_Middle_Parchment;
    [SerializeField]
    private RectTransform rect_Bottom_Parchment;
    [SerializeField]
    private RectTransform rect_Helper_Content;

    public float min_Scroll_Position_Parchment { get; private set; } = -400.0f;  // 초기 Parchment 오브젝트의 y position 값 = -400 * (-100 * n)
    public float max_Scroll_Position_Parchment { get; private set; } = 400.0f;    // 초기값 400 -> 400 + (100 * n)     n은 현 시간대에 얻은 단서의 갯수
    public float height_Parchment { get; private set; } = 800.0f;         // 양피지의 높이, 초기값 800 -> 800 + (200 * n)
    public float top_Parchment_Pos_Y { get; private set; } = 200.0f;      // 양피지 윗부분의 y position, 초기값 200 -> 200 + (100 * n)
    public float middle_Parchment_Pos_y { get; private set; } = -100.0f;   // 양피지 가운데부분의 y position, 초기값 -100 -> -100 + (100 * n)
    public float bottom_Parchment_Pos_y { get; private set; } = -200.0f;   // 양피지 아랫부분의 y position, 초기값 -200 -> -200 - (100 * n),  top_Parchment_Pos_y의 반대값을 띄는 규칙이 있음.
    public float additional_Clue_Pos_y { get; private set; } = 0.0f;    // 추가될 단서의 y position, 초기값 0, -> -200 * (n - 1) (단, n != 0)

    //[SerializeField]
    //private RectTransform rect_AggregationClueScrollList;

    /* 양피지를 옮기기 위한 화살표 */
    [SerializeField]
    private GameObject parchmentUpButton;
    [SerializeField]
    private GameObject parchmentDownButton;

    private bool isActivated_Help_Of_Andren;

    public static ParchmentControll instance = null;
    
    void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        maxScrollPosition = 400.0f;
        minScrollPosition = -400.0f;
        isPlayingDocumentAnim = false;
        atOnce = false;
        isActivated_Help_Of_Andren = false;
    }

    void Update()
    {
        if (GameManager.instance.GetPlayState() == GameManager.PlayState.Tutorial || GameManager.instance.GetPlayState() == GameManager.PlayState.Act)
        {
            if (UIManager.instance.GetIsOpenParchment() && rect_Parchment.localPosition.y == min_Scroll_Position_Parchment) // 760 + (200 * n) 일때 위 화살표 없애기
            {
                parchmentUpButton.SetActive(false);
                parchmentDownButton.SetActive(true);
            }
            else if (UIManager.instance.GetIsOpenParchment() && rect_Parchment.localPosition.y == max_Scroll_Position_Parchment) // 40 일때 아래 화살표 없애기
            {
                parchmentUpButton.SetActive(true);
                parchmentDownButton.SetActive(false);
                UIManager.instance.isReadParchment = true;

                // 중복 실행을 방지
                // 2월 발표회 이후 구현할 예정 (1월 28일 메모)
                /* 안드렌의 서류(단서) 등장 이벤트 진행 코드 */
                if (isActivated_Help_Of_Andren && !atOnce && !isPlayingDocumentAnim 
                    && ( (PlayerManager.instance.NumOfAct.Equals("53") && PlayerManager.instance.TimeSlot.Equals("74") ) || (PlayerManager.instance.NumOfAct.Equals("54") && PlayerManager.instance.TimeSlot.Equals("79")) ))
                {
                    UIManager.instance.isReadParchment_In_74_79 = true;
                    DocumentControll.instance.InvokeDocumentAnim();
                    isPlayingDocumentAnim = true;
                    atOnce = !atOnce;
                    DocumentAnim_End();
                }
            }
            else if (UIManager.instance.GetIsOpenParchment() && rect_Parchment.localPosition.y > min_Scroll_Position_Parchment && rect_Parchment.localPosition.y < max_Scroll_Position_Parchment)
            {
                // 40 초과 ~ 760 + (200 * n) 미만 일때 양쪽 화살표 나타내기
                parchmentUpButton.SetActive(true);
                parchmentDownButton.SetActive(true);
            }
        }
    }

    public bool GetIsActivated_Help_Of_Andren()
    {
        return isActivated_Help_Of_Andren;
    }

    // 현재 시간대에 얻은 단서의 수에 따라서, 양피지 위치 재조정
    public void UpdateParchmentPosition(int clueCount_In_This_TimeSlot)
    {
        //Debug.Log("현 시간대 얻은 단서 수 = " + clueCount_In_This_TimeSlot);

        min_Scroll_Position_Parchment = -400.0f + (-100.0f * clueCount_In_This_TimeSlot);
        UIManager.instance.yMinValue_RectOfParchment = -400.0f + (-100.0f * clueCount_In_This_TimeSlot);
        max_Scroll_Position_Parchment = 400.0f + (100.0f * clueCount_In_This_TimeSlot);
        height_Parchment = 800.0f + (200.0f * clueCount_In_This_TimeSlot);
        top_Parchment_Pos_Y = 200.0f + (100.0f * clueCount_In_This_TimeSlot);
        middle_Parchment_Pos_y = -100.0f + (100.0f * clueCount_In_This_TimeSlot);
        bottom_Parchment_Pos_y = -200.0f - (100.0f * clueCount_In_This_TimeSlot);

        rect_Parchment.sizeDelta = new Vector2(rect_Parchment.rect.width, height_Parchment);
        rect_Helper_Content.sizeDelta = new Vector2(rect_Helper_Content.rect.width, height_Parchment);
        rect_Top_Parchment.localPosition = new Vector3(rect_Top_Parchment.localPosition.x, top_Parchment_Pos_Y, rect_Top_Parchment.localPosition.z);
        rect_Middle_Parchment.localPosition = new Vector3(rect_Middle_Parchment.localPosition.x, middle_Parchment_Pos_y, rect_Middle_Parchment.localPosition.z);
        rect_Bottom_Parchment.localPosition = new Vector3(rect_Bottom_Parchment.localPosition.x, bottom_Parchment_Pos_y, rect_Bottom_Parchment.localPosition.z);
    }

    // 단서의 수를 늘려감에 따라, 추가되는 단서의 위치를 재조정, Inventory.cs에서 사용
    public void UpdateAdditionalCluePosition(int index)
    {
        if (index > 0)
            additional_Clue_Pos_y = -200.0f * (index - 1);
        else
            additional_Clue_Pos_y = 0.0f;
    }

    public Vector2 GetHelperContentPosition()
    {
        return rect_Helper_Content.localPosition;
    }

    public void SetHelperContentPosition(Vector2 position)
    {
        rect_Helper_Content.localPosition = position;
    }

    //public Vector2 GetAggregationClueListScrollListPosition()
    //{
    //    return rect_AggregationClueScrollList.localPosition;
    //}

    //public void SetAggregationClueListScrollListPosition(Vector2 position)
    //{
    //    rect_AggregationClueScrollList.localPosition = position;
    //}

    public Vector2 GetParchmentPosition()
    {
        return rect_Parchment.localPosition;
    }

    public void SetParchmentPosition(Vector2 position)
    {
        rect_Parchment.localPosition = position;
    }

    // 단서 정리가 완전히 끝난 후 실행되야 할 함수임
    //public void SetIsPlayingDocumentAnimToFalse()
    //{
    //    isPlayingDocumentAnim = false;
    //}

    public void DocumentAnim_End()
    {
        //Invoke("SetIsPlayingDocumentAnimToFalse", 6.0f);
        StartCoroutine(SetIsPlayingDocumentAnimToFalse());
    }

    IEnumerator SetIsPlayingDocumentAnimToFalse()
    {
        yield return new WaitForSecondsRealtime(6.0f);
        isPlayingDocumentAnim = false;
    }

    public void SetIsPlayingDocumentAnim(bool boolValue)
    {
        isPlayingDocumentAnim = boolValue;
    }

    public bool GetIsPlayingDocumentAnim()
    {
        return isPlayingDocumentAnim;
    }

    public void UpButton()
    {
        if(rect_Parchment.localPosition.y > minScrollPosition)
            rect_Parchment.localPosition = new Vector2(rect_Parchment.localPosition.x, rect_Parchment.localPosition.y - 80.0f);
    }

    public void DownButton()
    {
        if (rect_Parchment.localPosition.y < maxScrollPosition)
            rect_Parchment.localPosition = new Vector2(rect_Parchment.localPosition.x, rect_Parchment.localPosition.y + 80.0f);
    }

    public GameObject GetUpArrowOfParchment()
    {
        return parchmentUpButton;
    }

    public GameObject GetDownArrowOfParchment()
    {
        return parchmentDownButton;
    }

    public void SetActive_Help_Of_Andren(bool boolValue)
    {
        isActivated_Help_Of_Andren = boolValue;
    }
}
