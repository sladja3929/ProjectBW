using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapManager : MonoBehaviour
{

    public static MiniMapManager instance = null;

    public GameObject miniMapUI;
    private GameObject arrow;
    private bool isOpen;

    private Vector3 miniMap_Position_Slum_Street1 = new Vector3(-10.4f, -4.5f, -1f);
    private Vector3 miniMap_Position_Slum_Street2 = new Vector3(-10f, -6.5f, -1f);
    private Vector3 miniMap_Position_Market_Street1 = new Vector3(-10.5f, 2f, -1f);

    void Start()
    {
        if (instance == null)
            instance = this;

        isOpen = false;
        arrow = transform.GetChild(0).gameObject;
        MoveArrowPosition();
        miniMapUI.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M) && isOpen == false) {
            miniMapUI.SetActive(true);
            isOpen = true;
        }
        else if(Input.GetKeyDown(KeyCode.M) && isOpen == true) {
            miniMapUI.SetActive(false);
            isOpen = false;
        }
    }

    public void MoveArrowPosition() {
        if(PlayerManager.instance.GetCurrentPosition() == "Slum_Street1") {
            arrow.transform.localPosition = miniMap_Position_Slum_Street1;
        }

        else if (PlayerManager.instance.GetCurrentPosition() == "Slum_Street2") {
            arrow.transform.localPosition = miniMap_Position_Slum_Street2;
        }

        else if (PlayerManager.instance.GetCurrentPosition() == "Market_Street1") {
            arrow.transform.localPosition = miniMap_Position_Market_Street1;
        }
    }
}
