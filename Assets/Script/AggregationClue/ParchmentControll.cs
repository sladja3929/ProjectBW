using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParchmentControll : MonoBehaviour
{
    private double maxScrollPosition;   // 스크롤할 수 있는 최대치 높이
    private double minScrollPosition;   // 스크롤할 수 있는 최소치 높이
    private bool isPlayingDocumentAnim; // 안드렌 서류 타임라인 애니메이션이 실행되고 있는지를 알기 위한 변수

    [SerializeField]
    private RectTransform rect_Parchment;   // 제어할 양피지의 RectTransform 컴포넌트
    [SerializeField]
    private RectTransform rect_AggregationClueScrollList;
    [SerializeField]
    private RectTransform rect_Helper_Content;

    /* 양피지를 옮기기 위한 화살표 */
    [SerializeField]
    private GameObject parchmentUpButton;
    [SerializeField]
    private GameObject parchmentDownButton;

    public static ParchmentControll instance = null;
    
    void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        maxScrollPosition = 720.0f;
        minScrollPosition = -720.0f;
        isPlayingDocumentAnim = false;
    }

    void Update()
    {
        if (UIManager.instance.GetIsOpenParchment() && rect_Parchment.localPosition.y == -720) // -720 일때 위 화살표 없애기
        {
            parchmentUpButton.SetActive(false);
            parchmentDownButton.SetActive(true);
        }
        else if (UIManager.instance.GetIsOpenParchment() && rect_Parchment.localPosition.y == 720) // 720 일때 아래 화살표 없애기
        {
            parchmentUpButton.SetActive(true);
            parchmentDownButton.SetActive(false);
            UIManager.instance.isReadParchment = true;

            // 중복 실행을 방지
            // 2월 발표회 이후 구현할 예정 (1월 28일 메모)
            /* 안드렌의 서류(단서) 등장 이벤트 진행 코드 */
            //if (!isPlayingDocumentAnim)
            //{
            //    DocumentControll.instance.InvokeDocumentAnim();
            //    isPlayingDocumentAnim = !isPlayingDocumentAnim;
            //}

        }
        else if (UIManager.instance.GetIsOpenParchment() && rect_Parchment.localPosition.y > -720 && rect_Parchment.localPosition.y < 720) //-720 초과 ~720 미만 일때 양쪽 화살표 나타내기
        {
            parchmentUpButton.SetActive(true);
            parchmentDownButton.SetActive(true);
        }

    }

    public Vector2 GetHelperContentPosition()
    {
        return rect_Helper_Content.localPosition;
    }

    public void SetHelperContentPosition(Vector2 position)
    {
        rect_Helper_Content.localPosition = position;
    }

    public Vector2 GetAggregationClueListScrollListPosition()
    {
        return rect_AggregationClueScrollList.localPosition;
    }

    public void SetAggregationClueListScrollListPosition(Vector2 position)
    {
        rect_AggregationClueScrollList.localPosition = position;
    }

    public Vector2 GetParchmentPosition()
    {
        return rect_Parchment.localPosition;
    }

    public void SetParchmentPosition(Vector2 position)
    {
        rect_Parchment.localPosition = position;
    }

    // 단서 정리가 완전히 끝난 후 실행되야 할 함수임
    public void SetIsPlayingDocumentAnimToFalse()
    {
        isPlayingDocumentAnim = false;
    }

    public void UpButton()
    {
        if(rect_Parchment.localPosition.y > -720)
            rect_Parchment.localPosition = new Vector2(rect_Parchment.localPosition.x, rect_Parchment.localPosition.y - 80.0f);
    }

    public void DownButton()
    {
        if (rect_Parchment.localPosition.y < 720)
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

}
