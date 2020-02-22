using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapManager : MonoBehaviour
{
    public static MiniMapManager instance = null;

    /*맵이름 UI*/
    private Text MapName;

    /*미니맵 UI*/
    public GameObject InsideUI;
    public GameObject StreetUI;
    public GameObject miniMapUI;

    /*미니맵 카메라 용 렌더러 텍스쳐*/
    public RenderTexture StreetRenderTexture;
    public RenderTexture InsideRenderTexture;
    public RenderTexture TempRenderTexture;

    /*미니맵 카메라*/
    private GameObject StreetCameraWhole; //카메라를 관리하는 empty object용 - Findchild 용도
    private GameObject InsideCameraWhole;
    private Camera StreetCamera;
    private Camera InsideCamera;

    /*미니맵 화살표 마커*/
    private GameObject arrow;

    /*검사용 bool*/
    private bool isOpen;//미니맵 활성화용
    private bool isZoomOpen;//클릭을 통한 작은 구역 미니맵 활성화용
    private bool isInsideOpen;//건물 안 구역 미니맵 활성화용
    private bool isInsideNow;//현재 건물 안인가?

    /*마커용 좌표*/
    private Vector3 miniMap_Position_Slum_Street1 = new Vector3(-10.4f, -4.5f, -1f);
    private Vector3 miniMap_Position_Slum_Street2 = new Vector3(-10f, -6.5f, -1f);
    private Vector3 miniMap_Position_Market_Street1 = new Vector3(-10.5f, 2f, -1f);
    private Vector3 miniMap_Position_Market_Street2 = new Vector3(-10.5f, 2f, -1f);
    private Vector3 miniMap_Position_Market_Street3 = new Vector3(-10.5f, 2f, -1f);
    private Vector3 miniMap_Position_Village_Street1 = new Vector3(2.5f, 13.6f, -1f);
    private Vector3 miniMap_Position_Village_Street2 = new Vector3(2.5f, 11.4f, -1f);
    private Vector3 miniMap_Position_Village_Street3 = new Vector3(2.5f, 8.8f, -1f);
    private Vector3 miniMap_Position_Mansion_Street1 = new Vector3(10f, 3.6f, -1f);
    private Vector3 miniMap_Position_Mansion_Street2 = new Vector3(10.5f, 1f, -1f);
    private Vector3 miniMap_Position_Mansion_Street3 = new Vector3(10f, -1f, -1f);
    private Vector3 miniMap_Position_Downtown_Street1 = new Vector3(-0.2f, -0.5f, -1f);
    private Vector3 miniMap_Position_Forest_Street1 = new Vector3(0.75f, -7.5f, -1f);
    private Vector3 miniMap_Position_Forest_Street2 = new Vector3(0.75f, -9.3f, -1f);
    private Vector3 miniMap_Position_Forest_Street3 = new Vector3(0.75f, -11f, -1f);
    private Vector3 miniMap_Position_Harbor_Street1 = new Vector3(-10f, 11f, -1f);
    private Vector3 miniMap_Position_Chapter_Street1 = new Vector3(0.2f, 3.5f, -1f);
    

    void Start()
    {
        if (instance == null)
            instance = this;

        isOpen = false;
        isZoomOpen = false;
        isInsideOpen = false;

        arrow = transform.GetChild(0).gameObject;
        MapName = GameObject.Find("MapName").GetComponent<Text>();

        StreetCameraWhole = GameObject.Find("Street Camera");
        InsideCameraWhole = GameObject.Find("Inside Camera");

        /*초기위치 지부 설정*/
        arrow.transform.localPosition = miniMap_Position_Chapter_Street1;
        MapName.text = "안녕";
        //MoveArrowPosition();

        miniMapUI.SetActive(false);
        StreetUI.SetActive(false);

        /*건물 내부인지 감식 후 적용*/
        CheckMerteInside();
    }

    void Update()
    {
        /*맵 이름 적용*/
        ShowMapName();

            if (Input.GetKeyDown(KeyCode.Tab) && isOpen == false)
            {
                miniMapUI.SetActive(true);
                isOpen = true;

                if (isOpen == true && isInsideNow == true && isInsideOpen == false)
                {
                    PopUpInsideUI();
                }
            }
            //미니맵 끄기
            else if (Input.GetKeyDown(KeyCode.Tab) && isOpen == true)
            {

                if (isZoomOpen == true)
                {
                    StreetCamera.gameObject.SetActive(false);
                    StreetCamera.targetTexture = TempRenderTexture;
                    StreetUI.SetActive(false);
                    miniMapUI.transform.Find("MiniMapRenderer").gameObject.SetActive(true);
                    isZoomOpen = false;
                }

                if (isInsideOpen == true)
                {
                    InsideCamera.gameObject.SetActive(false);
                    InsideCamera.targetTexture = TempRenderTexture;
                    InsideUI.SetActive(false);
                    miniMapUI.transform.Find("MiniMapRenderer").gameObject.SetActive(true);
                    isInsideOpen = false;
                }

                miniMapUI.SetActive(false);
                isOpen = false;
            }

            //스트리트 카메라 켜져있을 때

            if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(0)) && isZoomOpen == true)
            {
                StreetCamera.targetTexture = TempRenderTexture;
                StreetUI.SetActive(false);
                miniMapUI.transform.Find("MiniMapRenderer").gameObject.SetActive(true);
                isZoomOpen = false;
            }
   
    }

    /*미니맵이 켜진채로 작용 - 포탈을 넘을 때 */
    public void MoveMap()
    {
        /*현재 위치 로그*/
        Debug.Log(PlayerManager.instance.GetCurrentPosition());

        //외부 - > 내부
        if (isOpen == true && isInsideNow == true && isInsideOpen == false)
        {
            //Debug.Log("외부에서 내부로");
            if (StreetCamera != null)
            { 
                StreetCamera.gameObject.SetActive(false);
            }
            PopUpInsideUI();
        }
        //내부 -> 외부
        else if (isOpen == true && isInsideNow == false && isInsideOpen == true)
        {
            //Debug.Log("내부에서 외부로");
            InsideCamera.gameObject.SetActive(false);
            InsideCamera.targetTexture = TempRenderTexture;
            InsideUI.SetActive(false);
            miniMapUI.transform.Find("MiniMapRenderer").gameObject.SetActive(true);
            isInsideOpen = false;
        }
        //내부 -> 내부
        if (isOpen == true && isInsideOpen == true && isInsideNow == true)
        {
            //Debug.Log("내부에서 내부로");
            InsideCamera.gameObject.SetActive(false);
            InsideCamera.targetTexture = TempRenderTexture;
            PopUpInsideUI();
        }
    }

    /*맵 이름 표시 함수*/
    private void ShowMapName()
    {
        string tmp = "맵이름";
        string MerteCurPos = PlayerManager.instance.GetCurrentPosition();
        //MapName.text = MerteCurPos;

        switch (MerteCurPos)
        {
            case "Slum_Street1":
                tmp = "슬럼가 거리 1";
                break;
            case "Slum_Street2":
                tmp = "슬럼가 거리 2";
                break;
            case "Market_Street1":
                tmp = "시장 1";
                break;
            case "Market_Street2":
                tmp = "시장 2";
                break;
            case "Market_Street3":
                tmp = "시장 3";
                break;
            case "Village_Street1":
                tmp = "주택가 거리 1";
                break;
            case "Village_Street2":
                tmp = "주택가 거리 2";
                break;
            case "Village_Street3":
                tmp = "주택가 거리 3";
                break;
            case "Mansion_Street1":
                tmp = "저택가 거리 1";
                break;
            case "Mansion_Street2":
                tmp = "저택가 거리 2";
                break;
            case "Mansion_Street3":
                tmp = "저택가 거리 3";
                break;
            case "Downtown_Street1":
                tmp = "도심 거리";
                break;
            case "Chapter_Street1":
                tmp = "지부 앞";
                break;
            case "Forest_Street1":
                tmp = "숲 1";
                break;
            case "Forest_Street2":
                tmp = "숲 2";
                break;
            case "Forest_Street3":
                tmp = "숲 3";
                break;
            case "Harbor_Street1":
                tmp = "선착장";
                break;
            case "Village_Raina_House":
                tmp = "레이나의 집";
                break;
            case "Village_Balrua_House":
                tmp = "발루아의 집";
                break;
            case "Chapter_Chapter_First_Floor":
                tmp = "지부 1층";
                break;
            case "Chapter_Chapter_Second_Floor":
                tmp = "지부 2층";
                break;
            case "Mansion_Viscount_Mansion_First_Floor":
                tmp = "자작의 저택 1층";
                break;
            case "Mansion_Viscount_Mansion_Second_Floor":
                tmp = "자작의 저택 2층";
                break;
            case "Mansion_President_Mansion_First_Floor":
                tmp = "총장의 저택 1층";
                break;
            case "Mansion_President_Mansion_Second_Floor":
                tmp = "총장의 저택 2층";
                break;
            case "Mansion_President_Mansion_Outhouse":
                tmp = "총장의 저택 별채";
                break;
            case "Downtown_Salon":
                tmp = "살롱";
                break;
            case "Downtown_Cafe":
                tmp = "카페";
                break;
            case "Downtown_Real_estate":
                tmp = "부동산";
                break;
            case "Harbor_Cruise":
                tmp = "유람선";
                break;
            case "Harbor_Prison":
                tmp = "감옥";
                break;
            case "Forest_Bro_sis_home":
                tmp = "남매의 집";
                break;
            case "Slum_Information_agency":
                tmp = "정보상";
                break;
            case "Chapter_President_Office":
                tmp = "총장의 사무실";
                break;
            case "Chapter_Secret_Space":
                tmp = "비밀의 방";
                break;
            case "Chapter_Merte_Office":
                tmp = "메르테의 사무실";
                break;
            case "Mansion_Guest_Room1":
                tmp = "손님방 1";
                break;
            case "Mansion_Guest_Room2":
                tmp = "손님방 2";
                break;
            case "Mansion_President_Room":
                tmp = "총장의 방";
                break;
            case "Mansion_Girls_Room":
                tmp = "소녀의 방";
                break;
            case "Mansion_Boys_Room":
                tmp = "소년의 방";
                break;
            case "Mansion_Study_Room":
                tmp = "공부방";
                break;
            case "Mansion_Dining_Room":
                tmp = "식당";
                break;
            default:
                break;
        }

        MapName.text = tmp;
    }

    /*건물 내부인지 감식 함수*/
    public void CheckMerteInside()
    {
        string MerteCurPos = PlayerManager.instance.GetCurrentPosition();

        switch (MerteCurPos)
        {
            //내부
            case "Village_Raina_House":
            case "Village_Balrua_House":
            case "Chapter_Chapter_First_Floor":
            case "Chapter_Chapter_Second_Floor":
            case "Mansion_Viscount_Mansion_First_Floor":
            case "Mansion_Viscount_Mansion_Second_Floor":
            case "Mansion_President_Mansion_First_Floor":
            case "Mansion_President_Mansion_Second_Floor":
            case "Mansion_President_Mansion_Outhouse":
            case "Downtown_Salon":
            case "Downtown_Cafe":
            case "Downtown_Real_estate":
            case "Harbor_Cruise":
            case "Harbor_Prison":
            case "Forest_Bro_sis_home":
            case "Slum_Information_agency":
            case "Chapter_President_Office":
            case "Chapter_Secret_Space":
            case "Chapter_Merte_Office":
            case "Mansion_Guest_Room1":
            case "Mansion_Guest_Room2":
            case "Mansion_President_Room":
            case "Mansion_Girls_Room":
            case "Mansion_Boys_Room":
            case "Mansion_Study_Room":
            case "Mansion_Dining_Room":
                isInsideNow = true;
                break;
            //외부
            default:
                isInsideNow = false;
                break;
           
        }
    }

    public void MoveArrowPosition() {

        string MerteCurPos = PlayerManager.instance.GetCurrentPosition();

        switch (MerteCurPos)
        {
            case "Slum_Street1":
                arrow.transform.localPosition = miniMap_Position_Slum_Street1;
                break;
            case "Slum_Street2":
                arrow.transform.localPosition = miniMap_Position_Slum_Street2;
                break;
            case "Market_Street1":
                arrow.transform.localPosition = miniMap_Position_Market_Street1;
                break;
            case "Market_Street2":
                arrow.transform.localPosition = miniMap_Position_Market_Street2;
                break;
            case "Market_Street3":
                arrow.transform.localPosition = miniMap_Position_Market_Street3;
                break;
            case "Village_Street1":
                arrow.transform.localPosition = miniMap_Position_Village_Street1;
                break;
            case "Village_Street2":
                arrow.transform.localPosition = miniMap_Position_Village_Street2;
                break;
            case "Village_Street3":
                arrow.transform.localPosition = miniMap_Position_Village_Street3;
                break;
            case "Mansion_Street1":
                arrow.transform.localPosition = miniMap_Position_Mansion_Street1;
                break;
            case "Mansion_Street2":
                arrow.transform.localPosition = miniMap_Position_Mansion_Street2;
                break;
            case "Mansion_Street3":
                arrow.transform.localPosition = miniMap_Position_Mansion_Street3;
                break;
            case "Downtown_Street1":
                arrow.transform.localPosition = miniMap_Position_Downtown_Street1;
                break;
            case "Chapter_Street1":
                arrow.transform.localPosition = miniMap_Position_Chapter_Street1;
                break;
            case "Forest_Street1":
                arrow.transform.localPosition = miniMap_Position_Forest_Street1;
                break;
            case "Forest_Street2":
                arrow.transform.localPosition = miniMap_Position_Forest_Street2;
                break;
            case "Forest_Street3":
                arrow.transform.localPosition = miniMap_Position_Forest_Street3;
                break;
            case "Harbor_Street1":
                arrow.transform.localPosition = miniMap_Position_Harbor_Street1;
                break;
            default:
                break;
        }
    }

    private void PopUpStreetUI() {

        isZoomOpen = true;
        StreetUI.SetActive(true);
        miniMapUI.transform.Find("MiniMapRenderer").gameObject.SetActive(false);
        StreetCamera.gameObject.SetActive(true);
        StreetCamera.targetTexture = StreetRenderTexture;
    }

    private void PopUpInsideUI()
    {
        isInsideOpen = true;
        InsideUI.SetActive(true);
        miniMapUI.transform.Find("MiniMapRenderer").gameObject.SetActive(false);


        string MerteCurPos = PlayerManager.instance.GetCurrentPosition();

        switch (MerteCurPos)
        {
            case "Village_Raina_House":
                Village_Raina_House_Inside();
                break;
            case "Village_Balrua_House":
                Village_Balrua_House_Inside();
                break;
            case "Chapter_Chapter_First_Floor":
                Chapter_Chapter_First_Floor_Inside();
                break;
            case "Chapter_Chapter_Second_Floor":
                Chapter_Chapter_Second_Floor_Inside();
                break;
            case "Mansion_Viscount_Mansion_First_Floor":
                Mansion_Viscount_Mansion_First_Floor_Inside();
                break;
            case "Mansion_Viscount_Mansion_Second_Floor":
                Mansion_Viscount_Mansion_Second_Floor_Inside();
                break;
            case "Mansion_President_Mansion_First_Floor":
                Mansion_President_Mansion_First_Floor_Inside();
                break;
            case "Mansion_President_Mansion_Second_Floor":
                Mansion_President_Mansion_Second_Floor_Inside();
                    break;
            case "Mansion_President_Mansion_Outhouse":
                Mansion_President_Mansion_Outhouse_Inside();
                break;
            case "Downtown_Salon":
                Downtown_Salon_Inside();
                break;
            case "Downtown_Cafe":
                Downtown_Cafe_Inside();
                break;
            case "Downtown_Real_estate":
                Downtown_Real_estate_Inside();
                break;
            case "Harbor_Cruise":
                Harbor_Cruise_Inside();
                break;
            case "Harbor_Prison":
                Harbor_Prison_Inside();
                break;
            case "Forest_Bro_sis_home":
                Forest_Bro_sis_home_Inside();
                break;
            case "Slum_Information_agency":
                Slum_Information_agency_Inside();
                break;
            case "Chapter_President_Office":
                Chapter_President_Office_Inside();
                break;
            case "Chapter_Secret_Space":
                Chapter_Secret_Space_Inside();
                break;
            case "Chapter_Merte_Office":
                Chapter_Merte_Office_Inside();
                break;
            case "Mansion_Guest_Room1":
                Mansion_Guest_Room1_Inside();
                break;
            case "Mansion_Guest_Room2":
                Mansion_Guest_Room2_Inside();
                break;
            case "Mansion_President_Room":
                Mansion_President_Room_Inside();
                break;
            case "Mansion_Girls_Room":
                Mansion_Girls_Room_Inside();
                break;
            case "Mansion_Boys_Room":
                Mansion_Boys_Room_Inside();
                break;
            case "Mansion_Study_Room":
                Mansion_Study_Room_Inside();
                break;
            case "Mansion_Dining_Room":
                Mansion_Dining_Room_Inside();
                break;
            default:
                break;
        }
        //Debug.Log(MerteCurPos+"로 내부 갱신 카메라는 현재 "+ InsideCamera.name);

        InsideCamera.gameObject.SetActive(true);
        InsideCamera.targetTexture = InsideRenderTexture;
    }

    /*건물 내부 미니맵*/
    public void Village_Raina_House_Inside()
    {
        InsideCamera = InsideCameraWhole.transform.Find("Village_Raina_House Camera").GetComponent<Camera>();    
    }
    public void Village_Balrua_House_Inside()
    {
        InsideCamera = InsideCameraWhole.transform.Find("Village_Balrua_House Camera").GetComponent<Camera>();
    }
    public void Chapter_Chapter_First_Floor_Inside()
    {
        InsideCamera = InsideCameraWhole.transform.Find("Chapter_Chapter_First_Floor Camera").GetComponent<Camera>();
    }
    public void Chapter_Chapter_Second_Floor_Inside()
    {
        InsideCamera = InsideCameraWhole.transform.Find("Chapter_Chapter_Second_Floor Camera").GetComponent<Camera>();
    }
    public void Mansion_Viscount_Mansion_First_Floor_Inside()
    {
        InsideCamera = InsideCameraWhole.transform.Find("Mansion_Viscount_Mansion_First_Floor Camera").GetComponent<Camera>();
    }
    public void Mansion_Viscount_Mansion_Second_Floor_Inside()
    {
        InsideCamera = InsideCameraWhole.transform.Find("Mansion_Viscount_Mansion_Second_Floor Camera").GetComponent<Camera>();
    }
    public void Mansion_President_Mansion_First_Floor_Inside()
    {
        InsideCamera = InsideCameraWhole.transform.Find("Mansion_President_Mansion_First_Floor Camera").GetComponent<Camera>();
    }
    public void Mansion_President_Mansion_Second_Floor_Inside()
    {
        InsideCamera = InsideCameraWhole.transform.Find("Mansion_President_Mansion_Second_Floor Camera").GetComponent<Camera>();
    }
    public void Mansion_President_Mansion_Outhouse_Inside()
    {
        InsideCamera = InsideCameraWhole.transform.Find("Mansion_President_Mansion_Outhouse Camera").GetComponent<Camera>();
    }
    public void Downtown_Salon_Inside()
    {
        InsideCamera = InsideCameraWhole.transform.Find("Downtown_Salon Camera").GetComponent<Camera>();
    }
    public void Downtown_Cafe_Inside()
    {
        InsideCamera = InsideCameraWhole.transform.Find("Downtown_Cafe Camera").GetComponent<Camera>();
    }
    public void Downtown_Real_estate_Inside()
    {
        InsideCamera = InsideCameraWhole.transform.Find("Downtown_Real_estate Camera").GetComponent<Camera>();
    }
    public void Harbor_Cruise_Inside()
    {
        InsideCamera = InsideCameraWhole.transform.Find("Harbor_Cruise Camera").GetComponent<Camera>();
    }
    public void Harbor_Prison_Inside()
    {
        InsideCamera = InsideCameraWhole.transform.Find("Harbor_Prison Camera").GetComponent<Camera>();
    }
    public void Forest_Bro_sis_home_Inside()
    {
        InsideCamera = InsideCameraWhole.transform.Find("Forest_Bro_sis_home Camera").GetComponent<Camera>();
    }
    public void Slum_Information_agency_Inside()
    {
        InsideCamera = InsideCameraWhole.transform.Find("Slum_Information_agency Camera").GetComponent<Camera>();
    }
    public void Chapter_President_Office_Inside()
    {
        InsideCamera = InsideCameraWhole.transform.Find("Chapter_President_Office Camera").GetComponent<Camera>();
    }
    public void Chapter_Secret_Space_Inside()
    {
        InsideCamera = InsideCameraWhole.transform.Find("Chapter_Secret_Space Camera").GetComponent<Camera>();
    }
    public void Chapter_Merte_Office_Inside()
    {
        InsideCamera = InsideCameraWhole.transform.Find("Chapter_Merte_Office Camera").GetComponent<Camera>();
    }
    public void Mansion_Guest_Room1_Inside()
    {
        InsideCamera = InsideCameraWhole.transform.Find("Mansion_Guest_Room1 Camera").GetComponent<Camera>();
    }
    public void Mansion_Guest_Room2_Inside()
    {
        InsideCamera = InsideCameraWhole.transform.Find("Mansion_Guest_Room2 Camera").GetComponent<Camera>();
    }
    public void Mansion_President_Room_Inside()
    {
        InsideCamera = InsideCameraWhole.transform.Find("Mansion_President_Room Camera").GetComponent<Camera>();
    }
    public void Mansion_Girls_Room_Inside()
    {
        InsideCamera = InsideCameraWhole.transform.Find("Mansion_Girls_Room Camera").GetComponent<Camera>();
    }
    public void Mansion_Boys_Room_Inside()
    {
        InsideCamera = InsideCameraWhole.transform.Find("Mansion_Boys_Room Camera").GetComponent<Camera>();
    }
    public void Mansion_Study_Room_Inside()
    {
        InsideCamera = InsideCameraWhole.transform.Find("Mansion_Study_Room Camera").GetComponent<Camera>();
    }
    public void Mansion_Dining_Room_Inside()
    {
        InsideCamera = InsideCameraWhole.transform.Find("Mansion_Dining_Room Camera").GetComponent<Camera>();
    }


    /*클릭으로 확대하는 스트리트별 미니맵*/
    public void Village_Street1_Button() {
       
        StreetCamera = StreetCameraWhole.transform.Find("Village_Street1 Camera").GetComponent<Camera>();
        PopUpStreetUI();
    }

    public void Village_Street2_Button() {
        StreetCamera = StreetCameraWhole.transform.Find("Village_Street2 Camera").GetComponent<Camera>();
        PopUpStreetUI();
    }

    public void Village_Street3_Button() {
        StreetCamera = StreetCameraWhole.transform.Find("Village_Street3 Camera").GetComponent<Camera>();
        PopUpStreetUI();
    }

    public void Mansion_Street1_Button() {
        StreetCamera = StreetCameraWhole.transform.Find("Mansion_Street1 Camera").GetComponent<Camera>();
        PopUpStreetUI();
    }

    public void Mansion_Street2_Button() {
        StreetCamera = StreetCameraWhole.transform.Find("Mansion_Street2 Camera").GetComponent<Camera>();
        PopUpStreetUI();
    }

    public void Mansion_Street3_Button() {
        StreetCamera = StreetCameraWhole.transform.Find("Mansion_Street3 Camera").GetComponent<Camera>();
        PopUpStreetUI();
    }

    public void Downtown_Street1_Button() {
        StreetCamera = StreetCameraWhole.transform.Find("Downtown_Street1 Camera").GetComponent<Camera>();
        PopUpStreetUI();
    }

    public void Chapter_Street1_Button() {
        StreetCamera = StreetCameraWhole.transform.Find("Chapter_Street1 Camera").GetComponent<Camera>();
        PopUpStreetUI();
    }

    public void Forest_Street1_Button() {
        StreetCamera = StreetCameraWhole.transform.Find("Forest_Street1 Camera").GetComponent<Camera>();
        PopUpStreetUI();
    }

    public void Forest_Street2_Button() {
        StreetCamera = StreetCameraWhole.transform.Find("Forest_Street2 Camera").GetComponent<Camera>();
        PopUpStreetUI();
    }

    public void Forest_Street3_Button() {
        StreetCamera = StreetCameraWhole.transform.Find("Forest_Street3 Camera").GetComponent<Camera>();
        PopUpStreetUI();
    }

    public void Slum_Street1_Button() {
        StreetCamera = StreetCameraWhole.transform.Find("Slum_Street1 Camera").GetComponent<Camera>();
        PopUpStreetUI();
    }

    public void Slum_Street2_Button() {
        StreetCamera = StreetCameraWhole.transform.Find("Slum_Street2 Camera").GetComponent<Camera>();
        PopUpStreetUI();
    }

    public void Market_Street1_Button() {
        StreetCamera = StreetCameraWhole.transform.Find("Market_Street1 Camera").GetComponent<Camera>();
        PopUpStreetUI();
    }

    public void Market_Street2_Button() {
        StreetCamera = StreetCameraWhole.transform.Find("Market_Street2 Camera").GetComponent<Camera>();
        PopUpStreetUI();
    }

    public void Harbor_Street1_Button() {
        StreetCamera = StreetCameraWhole.transform.Find("Harbor_Street1 Camera").GetComponent<Camera>();
        PopUpStreetUI();
    }
}
