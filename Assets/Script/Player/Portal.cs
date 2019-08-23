using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private GameObject arrow;
    public GameObject destination;      //포탈 출구

    private void Awake() {
        arrow = transform.GetChild(0).gameObject;
        arrow.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        if(other.tag == "character") {
            arrow.SetActive(true);

            if (Input.GetKeyDown(KeyCode.W) && arrow.transform.name == "UpToTake") {
                TakePortal();
            }

            if (Input.GetKeyDown(KeyCode.S) && arrow.transform.name == "DownToTake") {
                TakePortal();
            }

            if (Input.GetKeyDown(KeyCode.A) && arrow.transform.name == "LeftToTake")
            {
                TakePortal();
            }

            if (Input.GetKeyDown(KeyCode.D) && arrow.transform.name == "RightToTake")
            {
                TakePortal();
            }
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
        PlayerManager.instance.SetCurrentPosition(position);
        MiniMapManager.instance.MoveArrowPosition();
        Debug.Log(PlayerManager.instance.GetCurrentPosition() + "으로 이동");
    }

}
