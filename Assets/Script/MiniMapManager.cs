using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapManager : MonoBehaviour
{

    public static MiniMapManager instance = null;

    public GameObject StreetUI;
    public GameObject miniMapUI;
    public RenderTexture StreetRenderTexture;
    public RenderTexture TempRenderTexture;

    private Camera StreetCamera;
    private GameObject arrow;
    private bool isOpen;
    private bool isZoomIn;

    /*마커 좌표*/
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
    private Vector3 miniMap_Position_Chapter_Street1 = new Vector3(0.2f, 3.5f, -1f);

    void Start()
    {
        if (instance == null)
            instance = this;

        isOpen = false;
        isZoomIn = false;

        arrow = transform.GetChild(0).gameObject;

        arrow.transform.localPosition = miniMap_Position_Chapter_Street1;
        //MoveArrowPosition();
        miniMapUI.SetActive(false);
        StreetUI.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M) && isOpen == false) {
            miniMapUI.SetActive(true);
            isOpen = true;
        }

        else if(Input.GetKeyDown(KeyCode.M) && isOpen == true) {

            if(isZoomIn == true) {
                StreetCamera.targetTexture = TempRenderTexture;
                StreetUI.SetActive(false);
                miniMapUI.transform.Find("MiniMapRenderer").gameObject.SetActive(true);
                isZoomIn = false;
            }

            miniMapUI.SetActive(false);
            isOpen = false;
        }

        if(Input.GetMouseButtonDown(0) && isZoomIn == true) {
            StreetCamera.targetTexture = TempRenderTexture;
            StreetUI.SetActive(false);
            miniMapUI.transform.Find("MiniMapRenderer").gameObject.SetActive(true);
            isZoomIn = false;
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
            default:
                break;
        }
    }

    private void PopUpStreetUI() {
        isZoomIn = true;
        StreetUI.SetActive(true);
        miniMapUI.transform.Find("MiniMapRenderer").gameObject.SetActive(false);
        StreetCamera.targetTexture = StreetRenderTexture;
    }

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


}
