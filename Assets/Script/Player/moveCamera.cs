using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveCamera : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private string whereIsPlayer;
    private Vector3 playerPosition;

    /* 슬램가 거리 1 */
    private Vector3 position_Of_Sector1_Of_Street1_In_Slum = new Vector3(0, 0, -10);
    private Vector3 position_Of_Sector2_Of_Street1_In_Slum = new Vector3(1280.0f, 0, -10);
    private Vector3 position_Of_Sector3_Of_Street1_In_Slum = new Vector3(2560.0f, 0, -10);

    /* 슬램가 거리 2 */
    private Vector3 position_Of_Sector1_Of_Street2_In_Slum = new Vector3(816.9f, -941.0f, -10);
    private Vector3 position_Of_Sector2_Of_Street2_In_Slum = new Vector3(2096.9f, -941.0f, -10);

    /*시장 거리 1*/
    private Vector3 position_Of_Sector1_Of_Street1_In_Market = new Vector3(0, 1200, -10);

    /*주택가 거리 1*/
    private Vector3 position_Of_Sector1_Of_Street1_In_Village = new Vector3(5000f, 5300f, -10);
    private Vector3 position_Of_Sector2_Of_Street1_In_Village = new Vector3(6280f, 5300f, -10);
    private Vector3 position_Of_Sector3_Of_Street1_In_Village = new Vector3(7560f, 5300f, -10);

    /*주택가 거리 2*/
    private Vector3 position_Of_Sector1_Of_Street2_In_Village = new Vector3(5000f, 4200f, -10);
    private Vector3 position_Of_Sector2_Of_Street2_In_Village = new Vector3(6280f, 4200f, -10);
    private Vector3 position_Of_Sector3_Of_Street2_In_Village = new Vector3(7560f, 4200f, -10);

    /*주택가 거리 3*/
    private Vector3 position_Of_Sector1_Of_Street3_In_Village = new Vector3(5000f, 3100f, -10);
    private Vector3 position_Of_Sector2_Of_Street3_In_Village = new Vector3(6280f, 3100f, -10);
    private Vector3 position_Of_Sector3_Of_Street3_In_Village = new Vector3(7560f, 3100f, -10);

    void Start()
    {
        whereIsPlayer = PlayerManager.instance.GetCurrentPosition();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SetCameraPosition();
    }

    public void SetCameraPosition()
    {
        whereIsPlayer = PlayerManager.instance.GetCurrentPosition();
        playerPosition = player.transform.localPosition;

        if (whereIsPlayer == "Slum_Street1")
        {
            if (playerPosition.x >= -640.0f && playerPosition.x < 640.0f)
            {
                transform.localPosition = position_Of_Sector1_Of_Street1_In_Slum;
            }
            else if (playerPosition.x >= 640.0f && playerPosition.x < 1920.0f)
            {
                transform.localPosition = position_Of_Sector2_Of_Street1_In_Slum;

            }
            else if (playerPosition.x >= 1920.0f && playerPosition.x < 3200.0f)
            {
                transform.localPosition = position_Of_Sector3_Of_Street1_In_Slum;
            }
        }

        else if (whereIsPlayer == "Slum_Street2")
        {
            if (playerPosition.x >= 176.0f && playerPosition.x < 1456.0f)
            {
                transform.localPosition = position_Of_Sector1_Of_Street2_In_Slum;
            }
            else if (playerPosition.x >= 1456.0f && playerPosition.x < 2736.0f)
            {
                transform.localPosition = position_Of_Sector2_Of_Street2_In_Slum;

            }
        }

        else if (whereIsPlayer == "Market_Street1") {

            transform.localPosition = position_Of_Sector1_Of_Street1_In_Market;
        }

        else if(whereIsPlayer == "Village_Street1") {

            if (playerPosition.x >= 4360.0f && playerPosition.x < 5640.0f) {
                transform.localPosition = position_Of_Sector1_Of_Street1_In_Village;
            }
            else if (playerPosition.x >= 5640.0f && playerPosition.x < 6920.0f) {
                transform.localPosition = position_Of_Sector2_Of_Street1_In_Village;
            }
            else if (playerPosition.x >= 6920.0f && playerPosition.x < 8200.0f) {
                transform.localPosition = position_Of_Sector3_Of_Street1_In_Village;
            }
        }

        else if (whereIsPlayer == "Village_Street2") {

            if (playerPosition.x >= 4360.0f && playerPosition.x < 5640.0f) {
                transform.localPosition = position_Of_Sector1_Of_Street2_In_Village;
            }
            else if (playerPosition.x >= 5640.0f && playerPosition.x < 6920.0f) {
                transform.localPosition = position_Of_Sector2_Of_Street2_In_Village;
            }
            else if (playerPosition.x >= 6920.0f && playerPosition.x < 8200.0f) {
                transform.localPosition = position_Of_Sector3_Of_Street2_In_Village;
            }
        }

        else if (whereIsPlayer == "Village_Street3") {

            if (playerPosition.x >= 4360.0f && playerPosition.x < 5640.0f) {
                transform.localPosition = position_Of_Sector1_Of_Street3_In_Village;
            }
            else if (playerPosition.x >= 5640.0f && playerPosition.x < 6920.0f) {
                transform.localPosition = position_Of_Sector2_Of_Street3_In_Village;
            }
            else if (playerPosition.x >= 6920.0f && playerPosition.x < 8200.0f) {
                transform.localPosition = position_Of_Sector3_Of_Street3_In_Village;
            }
        }
    }
}
