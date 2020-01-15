using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParchmentControll : MonoBehaviour
{
    /* 오브젝트와의 상호작용을 위한 변수 */
    private Vector2 pos;            //마우스로 클릭한 곳의 위치
    private Ray2D ray;              //마우스로 클릭한 곳에 보이지않는 광선을 쏨
    private RaycastHit2D hit;       //쏜 광선에 닿은것이 뭔지 확인하기위한 변수
    private double maxScrollPosition;   // 스크롤할 수 있는 최대치 높이
    private double minScrollPosition;   // 스크롤할 수 있는 최소치 높이

    [SerializeField]
    private GameObject Parchment;       // 제어할 양피지 오브젝트
    private RectTransform Rect_Parchment;   // 양피지의 RectTransform 컴포넌트 저장용

    void Start()
    {
        maxScrollPosition = 720.0f;
        minScrollPosition = -720.0f;
    }

    // Update is called once per frame
    void Update()
    {
        /* 오브젝트와의 상호작용을 위한 if */
        /* 양피지 중간에 단서 스크롤과 겹쳐지면 고쳐야함.. */

        /* 양피지 위에 마우스 커서를 두고, 마우스 휠을 위로 올릴 때 */
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            /*
            pos = Camera.main.WorldToScreenPoint(Input.mousePosition);
            ray = new Ray2D(pos, Vector2.zero);
            hit = Physics2D.Raycast(ray.origin, ray.direction);
            */
            //if (hit.collider.tag == "Parchment")
            if (UIManager.instance.GetIsOpenParchment())
            {
                Rect_Parchment = Parchment.GetComponent<RectTransform>();

                if (Rect_Parchment.localPosition.y >= minScrollPosition && Rect_Parchment.localPosition.y < maxScrollPosition)
                {
                    Parchment.GetComponent<RectTransform>().localPosition = new Vector2(Rect_Parchment.localPosition.x, Rect_Parchment.localPosition.y + 40.0f);
                }
            }
        }

        /* 양피지 위에 마우스 커서를 두고, 마우스 휠을 아래로 내릴 때 */
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            /*
            pos = Camera.main.WorldToScreenPoint(Input.mousePosition);
            ray = new Ray2D(pos, Vector2.zero);
            hit = Physics2D.Raycast(ray.origin, ray.direction);
            */
            //if (hit.collider.tag == "Parchment")
            if (UIManager.instance.GetIsOpenParchment())
            {
                Rect_Parchment = Parchment.GetComponent<RectTransform>();

                if (Rect_Parchment.localPosition.y <= maxScrollPosition && Rect_Parchment.localPosition.y > minScrollPosition)
                {
                    Parchment.GetComponent<RectTransform>().localPosition = new Vector2(Rect_Parchment.localPosition.x, Rect_Parchment.localPosition.y - 40.0f);
                }
            }
        }
    }
}
