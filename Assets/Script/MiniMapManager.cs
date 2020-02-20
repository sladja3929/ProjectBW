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

        /*초기위치 지부 설정*/
        arrow.transform.localPosition = miniMap_Position_Chapter_Street1;
        MapName.text = "안녕";
        //MoveArrowPosition();

        miniMapUI.SetActive(false);
        StreetUI.SetActive(false);
    }

    void Update()
    {
        /*맵 이름 적용*/
        ShowMapName();

        /*건물 내부인지 감식 후 실시간 적용*/
        CheckMerteInside();
            if (Input.GetKeyDown(KeyCode.M) && isOpen == false)
            {
                miniMapUI.SetActive(true);
                isOpen = true;
            }
            else if (Input.GetKeyDown(KeyCode.M) && isOpen == true)
            {

                if (isZoomOpen == true)
                {
                    StreetCamera.targetTexture = TempRenderTexture;
                    StreetUI.SetActive(false);
                    miniMapUI.transform.Find("MiniMapRenderer").gameObject.SetActive(true);
                    isZoomOpen = false;
                }

                if (isInsideOpen == true)
                {
                    InsideCamera.targetTexture = TempRenderTexture;
                    InsideUI.SetActive(false);
                    miniMapUI.transform.Find("MiniMapRenderer").gameObject.SetActive(true);
                    isInsideOpen = false;
                }

                miniMapUI.SetActive(false);
                isOpen = false;
            }

        if (Input.GetMouseButtonDown(0) && isZoomOpen == true)
        {
            StreetCamera.targetTexture = TempRenderTexture;
            StreetUI.SetActive(false);
            miniMapUI.transform.Find("MiniMapRenderer").gameObject.SetActive(true);
            isZoomOpen = false;
        }

        /*맵이 켜진채로 작용*/
        //외부 - > 내부
        if (isOpen == true && isInsideNow == true && isInsideOpen == false)
        {
            PopUpInsideUI();
        }
        //내부 -> 외부
        else if (isOpen == true && isInsideNow == false && isInsideOpen == true)
        {
            InsideCamera.targetTexture = TempRenderTexture;
            InsideUI.SetActive(false);
            miniMapUI.transform.Find("MiniMapRenderer").gameObject.SetActive(true);
            isInsideOpen = false;
        }
        //내부 -> 내부
        if (isOpen == true && isInsideOpen == true && isInsideNow == true)
        {
            InsideCamera.targetTexture = TempRenderTexture;
            PopUpInsideUI();
        }
    }

    /*맵 이름 표시 함수*/
    private void ShowMapName()
    {
        string MerteCurPos = PlayerManager.instance.GetCurrentPosition();
        MapName.text = MerteCurPos;

        //한글이름으로 변환용
        //switch (MerteCurPos)
        //{
        //    case "Slum_Street1":
        //        break;
        //    case "Slum_Street2":
        //        break;
        //    case "Market_Street1":
        //        break;
        //    case "Market_Street2":
        //        break;
        //    case "Market_Street3":
        //        break;
        //    case "Village_Street1":
        //        break;
        //    case "Village_Street2":
        //        break;
        //    case "Village_Street3":
        //        break;
        //    case "Mansion_Street1":
        //        break;
        //    case "Mansion_Street2":
        //        break;
        //    case "Mansion_Street3":
        //        break;
        //    case "Downtown_Street1":
        //        break;
        //    case "Chapter_Street1":
        //        break;
        //    case "Forest_Street1":
        //        break;
        //    case "Forest_Street2":
        //        break;
        //    case "Forest_Street3":
        //        break;
        //    case "Harbor_Street1":
        //        break;
        //    case "Village_Raina_House":
        //        break;
        //    case "Village_Balrua_House":
        //        break;
        //    case "Chapter_Chapter_First_Floor":
        //        break;
        //    case "Chapter_Chapter_Second_Floor":
        //        break;
        //    case "Mansion_Viscount_Mansion_First_Floor":
        //        break;
        //    case "Mansion_Viscount_Mansion_Second_Floor":
        //        break;
        //    case "Mansion_President_Mansion_First_Floor":
        //        break;
        //    case "Mansion_President_Mansion_Second_Floor":
        //        break;
        //    case "Mansion_President_Mansion_Outhouse":
        //        break;
        //    case "Downtown_Salon":
        //        break;
        //    case "Downtown_Cafe":
        //        break;
        //    case "Downtown_Real_estate":
        //        break;
        //    case "Harbor_Cruise":
        //        break;
        //    case "Harbor_Prison":
        //        break;
        //    case "Forest_Bro_sis_home":
        //        break;
        //    case "Slum_Information_agency":
        //        break;
        //    case "Chapter_President_Office":
        //        break;
        //    case "Chapter_Secret_Space":
        //        break;
        //    case "Chapter_Merte_Office":
        //        break;
        //    case "Mansion_Guest_Room1":
        //        break;
        //    case "Mansion_Guest_Room2":
        //        break;
        //    case "Mansion_President_Room":
        //        break;
        //    case "Mansion_Girls_Room":
        //        break;
        //    case "Mansion_Boys_Room":
        //        break;
        //    case "Mansion_Study_Room":
        //        break;
        //    case "Mansion_Dining_Room":
        //        break;
        //    default:
        //        break;
        //}

         
    }

    /*건물 내부인지 감식 함수*/
    private void CheckMerteInside()
    {
        string MerteCurPos = PlayerManager.instance.GetCurrentPosition();

        switch (MerteCurPos)
        {
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
            default:
                isInsideNow = false;
                break;
           
        }
    }

    public void MoveArrowPosition() {

        Debug.Log(PlayerManager.instance.GetCurrentPosition());

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


        InsideCamera.targetTexture = InsideRenderTexture;
    }

    /*건물 내부 미니맵*/
    public void Village_Raina_House_Inside()
    {
        InsideCamera = GameObject.Find("Village_Raina_House Camera").GetComponent<Camera>();
    }
    public void Village_Balrua_House_Inside()
    {
        InsideCamera = GameObject.Find("Village_Balrua_House Camera").GetComponent<Camera>();
    }
    public void Chapter_Chapter_First_Floor_Inside()
    {
        InsideCamera = GameObject.Find("Chapter_Chapter_First_Floor Camera").GetComponent<Camera>();
    }
    public void Chapter_Chapter_Second_Floor_Inside()
    {
        InsideCamera = GameObject.Find("Chapter_Chapter_Second_Floor Camera").GetComponent<Camera>();
    }
    public void Mansion_Viscount_Mansion_First_Floor_Inside()
    {
        InsideCamera = GameObject.Find("Mansion_Viscount_Mansion_First_Floor Camera").GetComponent<Camera>();
    }
    public void Mansion_Viscount_Mansion_Second_Floor_Inside()
    {
        InsideCamera = GameObject.Find("Mansion_Viscount_Mansion_Second_Floor Camera").GetComponent<Camera>();
    }
    public void Mansion_President_Mansion_First_Floor_Inside()
    {
        InsideCamera = GameObject.Find("Mansion_President_Mansion_First_Floor Camera").GetComponent<Camera>();
    }
    public void Mansion_President_Mansion_Second_Floor_Inside()
    {
        InsideCamera = GameObject.Find("Mansion_President_Mansion_Second_Floor Camera").GetComponent<Camera>();
    }
    public void Mansion_President_Mansion_Outhouse_Inside()
    {
        InsideCamera = GameObject.Find("Mansion_President_Mansion_Outhouse Camera").GetComponent<Camera>();
    }
    public void Downtown_Salon_Inside()
    {
        InsideCamera = GameObject.Find("Downtown_Salon Camera").GetComponent<Camera>();
    }
    public void Downtown_Cafe_Inside()
    {
        InsideCamera = GameObject.Find("Downtown_Cafe Camera").GetComponent<Camera>();
    }
    public void Downtown_Real_estate_Inside()
    {
        InsideCamera = GameObject.Find("Downtown_Real_estate Camera").GetComponent<Camera>();
    }
    public void Harbor_Cruise_Inside()
    {
        InsideCamera = GameObject.Find("Harbor_Cruise Camera").GetComponent<Camera>();
    }
    public void Harbor_Prison_Inside()
    {
        InsideCamera = GameObject.Find("Harbor_Prison Camera").GetComponent<Camera>();
    }
    public void Forest_Bro_sis_home_Inside()
    {
        InsideCamera = GameObject.Find("Forest_Bro_sis_home Camera").GetComponent<Camera>();
    }
    public void Slum_Information_agency_Inside()
    {
        InsideCamera = GameObject.Find("Slum_Information_agency Camera").GetComponent<Camera>();
    }
    public void Chapter_President_Office_Inside()
    {
        InsideCamera = GameObject.Find("Chapter_President_Office Camera").GetComponent<Camera>();
    }
    public void Chapter_Secret_Space_Inside()
    {
        InsideCamera = GameObject.Find("Chapter_Secret_Space Camera").GetComponent<Camera>();
    }
    public void Chapter_Merte_Office_Inside()
    {
        InsideCamera = GameObject.Find("Chapter_Merte_Office Camera").GetComponent<Camera>();
    }
    public void Mansion_Guest_Room1_Inside()
    {
        InsideCamera = GameObject.Find("Mansion_Guest_Room1 Camera").GetComponent<Camera>();
    }
    public void Mansion_Guest_Room2_Inside()
    {
        InsideCamera = GameObject.Find("Mansion_Guest_Room2 Camera").GetComponent<Camera>();
    }
    public void Mansion_President_Room_Inside()
    {
        InsideCamera = GameObject.Find("Mansion_President_Room Camera").GetComponent<Camera>();
    }
    public void Mansion_Girls_Room_Inside()
    {
        InsideCamera = GameObject.Find("Mansion_Girls_Room Camera").GetComponent<Camera>();
    }
    public void Mansion_Boys_Room_Inside()
    {
        InsideCamera = GameObject.Find("Mansion_Boys_Room Camera").GetComponent<Camera>();
    }
    public void Mansion_Study_Room_Inside()
    {
        InsideCamera = GameObject.Find("Mansion_Study_Room Camera").GetComponent<Camera>();
    }
    public void Mansion_Dining_Room_Inside()
    {
        InsideCamera = GameObject.Find("Mansion_Dining_Room Camera").GetComponent<Camera>();
    }


    /*클릭으로 확대하는 스트리트별 미니맵*/
    public void Village_Street1_Button() {
        StreetCamera = GameObject.Find("Village_Street1 Camera").GetComponent<Camera>();
        PopUpStreetUI();
    }

    public void Village_Street2_Button() {
        StreetCamera = GameObject.Find("Village_Street2 Camera").GetComponent<Camera>();
        PopUpStreetUI();
    }

    public void Village_Street3_Button() {
        StreetCamera = GameObject.Find("Village_Street3 Camera").GetComponent<Camera>();
        PopUpStreetUI();
    }

    public void Mansion_Street1_Button() {
        StreetCamera = GameObject.Find("Mansion_Street1 Camera").GetComponent<Camera>();
        PopUpStreetUI();
    }

    public void Mansion_Street2_Button() {
        StreetCamera = GameObject.Find("Mansion_Street2 Camera").GetComponent<Camera>();
        PopUpStreetUI();
    }

    public void Mansion_Street3_Button() {
        StreetCamera = GameObject.Find("Mansion_Street3 Camera").GetComponent<Camera>();
        PopUpStreetUI();
    }

    public void Downtown_Street1_Button() {
        StreetCamera = GameObject.Find("Downtown_Street1 Camera").GetComponent<Camera>();
        PopUpStreetUI();
    }

    public void Chapter_Street1_Button() {
        StreetCamera = GameObject.Find("Chapter_Street1 Camera").GetComponent<Camera>();
        PopUpStreetUI();
    }

    public void Forest_Street1_Button() {
        StreetCamera = GameObject.Find("Forest_Street1 Camera").GetComponent<Camera>();
        PopUpStreetUI();
    }

    public void Forest_Street2_Button() {
        StreetCamera = GameObject.Find("Forest_Street2 Camera").GetComponent<Camera>();
        PopUpStreetUI();
    }

    public void Forest_Street3_Button() {
        StreetCamera = GameObject.Find("Forest_Street3 Camera").GetComponent<Camera>();
        PopUpStreetUI();
    }

    public void Slum_Street1_Button() {
        StreetCamera = GameObject.Find("Slum_Street1 Camera").GetComponent<Camera>();
        PopUpStreetUI();
    }

    public void Slum_Street2_Button() {
        StreetCamera = GameObject.Find("Slum_Street2 Camera").GetComponent<Camera>();
        PopUpStreetUI();
    }

    public void Market_Street1_Button() {
        StreetCamera = GameObject.Find("Market_Street1 Camera").GetComponent<Camera>();
        PopUpStreetUI();
    }

    public void Market_Street2_Button() {
        StreetCamera = GameObject.Find("Market_Street2 Camera").GetComponent<Camera>();
        PopUpStreetUI();
    }

    public void Harbor_Street1_Button() {
        StreetCamera = GameObject.Find("Harbor_Street1 Camera").GetComponent<Camera>();
        PopUpStreetUI();
    }

    /*맵 상단에 맵 이름 표시*/

}
