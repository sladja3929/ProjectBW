using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    // 포탈의 화살표
    private GameObject arrow;
    // 건물 안으로 들어가는 문
    private GameObject door;
    // 각 포탈과 문을 이용해 갈 수 있는 지점
    public GameObject destination;      //포탈 출구

    private Animator Fadeanimator;      //Fadeinout용 애니메이터
    [SerializeField] private GameObject FadeImage;        //Fade용 이미지

    private Vector2 pos;            //마우스로 클릭한 곳의 위치
    private Ray2D ray;              //마우스로 클릭한 곳에 보이지않는 광선을 쏨
    private RaycastHit2D hit;       //쏜 광선에 닿은것이 뭔지 확인하기위한 변수

    private void Awake() {
        // 포탈 스크립트를 가진 오브젝트는 오직 1개의 오브젝트만 가진다. (화살표 or 문)
        arrow = transform.GetChild(0).gameObject;
        door = arrow;
        arrow.SetActive(false);

        Fadeanimator = GameObject.Find("Fade_Image").transform.GetComponent<Animator>();
        FadeImage = GameObject.Find("Fade_Image");
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        if(other.tag == "character") {
            
            if(arrow != null)
                arrow.SetActive(true);
            /*
            // ~ door 들 태그 door로 바꾸고 door.transform.tag == "PortalPoint" 로 바꿔놓기
            if (//조건 시작
                (Input.GetKeyDown(KeyCode.W) && arrow.transform.name == "UpToTake") || (Input.GetKeyDown(KeyCode.S) && arrow.transform.name == "DownToTake")
                || (Input.GetKeyDown(KeyCode.A) && arrow.transform.name == "LeftToTake") || (Input.GetKeyDown(KeyCode.D) && arrow.transform.name == "RightToTake")
                || (Input.GetMouseButtonDown(0) && ((door.transform.name == "RainaHouseDoor") || (door.transform.name == "BalruaHouseDoor") || (door.transform.name == "ChapterDoor") || (door.transform.name == "ViscountMansionDoor")
                                                       || (door.transform.name == "PresidentMansionDoor") || (door.transform.name == "SalonDoor") || (door.transform.name == "GirlsRoomDoor")
                                                       || (door.transform.name == "BoysRoomDoor") || (door.transform.name == "StudyRoomDoor") || (door.transform.name == "DiningRoomDoor")
                                                       || (door.transform.name == "PresidentRoomDoor") || (door.transform.name == "GuestRoom1Door") || (door.transform.name == "GuestRoom2Door")
                                                       || (door.transform.name == "MerteOfficeDoor") || (door.transform.name == "MerteOfficeExitDoor") || (door.transform.name == "PresidentOfficeDoor")
                                                       || (door.transform.name == "BroSisHouseDoor") || (door.transform.name == "BroSisHouseExitDoor") || (door.transform.name == "CafeDoor") || (door.transform.name == "InformationAgencyDoor")
                                                       || (door.transform.name == "RealestateDoor")))
                //조건 끝
                )
            {
                StartCoroutine(FadeWithTakePortal());
            }
            */
            if (//조건 시작
                (!UIManager.instance.isPortaling && (Input.GetKeyDown(KeyCode.W) && arrow.transform.name == "UpToTake") || (Input.GetKeyDown(KeyCode.S) && arrow.transform.name == "DownToTake")
                || (Input.GetKeyDown(KeyCode.A) && arrow.transform.name == "LeftToTake") || (Input.GetKeyDown(KeyCode.D) && arrow.transform.name == "RightToTake"))
                //조건 끝
                )
            {
                StartCoroutine(FadeWithTakePortal());
            }

            if (Input.GetMouseButtonDown(0) && !UIManager.instance.GetIsOpenNote() && !UIManager.instance.isConversationing 
                && !UIManager.instance.GetIsOpenedParchment() && !UIManager.instance.isFading && !UIManager.instance.isPortaling)
            {
                pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                ray = new Ray2D(pos, Vector2.zero);
                hit = Physics2D.Raycast(ray.origin, ray.direction);

                if (hit.collider == null)
                {
                    //Debug.Log("아무것도 안맞죠?2");
                }
                else if (hit.collider.tag == "PortalPoint" && (door.transform.name != "UpToTake") && (door.transform.name != "RightToTake")
                    && (door.transform.name != "DownToTake") && (door.transform.name != "LeftToTake"))
                {
                    if (door.transform.name == "BroSisHouseDoor")
                        PlayerManager.instance.num_Enter_or_Investigate_BroSisHouse++;
                    if (door.transform.name == "RainaHouseDoor")
                        PlayerManager.instance.isInvestigated_Raina_house = true;
                    if (door.transform.name == "ViscountMansionDoor")
                        PlayerManager.instance.num_Enter_in_Mansion++;

                    StartCoroutine(FadeWithTakePortal());
                }
            }

            //if (Input.GetKeyDown(KeyCode.S) && arrow.transform.name == "DownToTake") {
            //    StartCoroutine(FadeWithTakePortal());
            //}

            //if (Input.GetKeyDown(KeyCode.A) && arrow.transform.name == "LeftToTake")
            //{
            //    StartCoroutine(FadeWithTakePortal());
            //}

            //if (Input.GetKeyDown(KeyCode.D) && arrow.transform.name == "RightToTake")
            //{
            //    StartCoroutine(FadeWithTakePortal());
            //}

        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "character") {
            arrow.SetActive(false);
        }
    }

    public void TakePortal() {

        Vector3 tempPosition = PlayerManager.instance.GetPlayerPosition();
        tempPosition.x = destination.transform.position.x;
        tempPosition.y = destination.transform.position.y;
        PlayerManager.instance.SetPlayerPosition(tempPosition);
        string position = destination.transform.parent.parent.parent.name
                           + "_" + destination.transform.parent.parent.name;
        
        //MoveCamera에서 GetCurrentPosition사용
        PlayerManager.instance.SetCurrentPosition(position);
        /* 플레이어가 이동한 곳에 따라 PlayerManager의 이벤트 변수들의 값을 바꿔주는 조건문이 필요 */

        MiniMapManager.instance.MoveArrowPosition();
        //Debug.Log(PlayerManager.instance.GetCurrentPosition() + "으로 이동");
    }

    private IEnumerator FadeWithTakePortal()
    {
        UIManager.instance.isPortaling = true;
        /*페이드 아웃*/
        FadeImage.SetActive(true);
        Fadeanimator.SetBool("isfadeout", true);
        yield return new WaitForSeconds(0.5f);

        /*이동*/
        TakePortal();
        //Debug.Log("포탈 타는중");

        // 이벤트를 적용시킬 것이 있는지 확인 후, 적용
        EventManager.instance.PlayEvent();

        /*페이드 인*/
        yield return new WaitForSeconds(1f);
        Fadeanimator.SetBool("isfadeout", false);
        UIManager.instance.isPortaling = false;
    }
}
