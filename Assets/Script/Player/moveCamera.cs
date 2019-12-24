using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private string whereIsPlayer;
    private Vector3 playerPosition;

    /* Main Camera 오브젝트의 위치를 직접 옮기며 측정 */
    /* 슬램가 거리 1 */
    private Vector3 position_Of_Sector1_Of_Street1_In_Slum = new Vector3(0, 0, -10);
    private Vector3 position_Of_Sector2_Of_Street1_In_Slum = new Vector3(1280.0f, 0, -10);
    private Vector3 position_Of_Sector3_Of_Street1_In_Slum = new Vector3(2560.0f, 0, -10);

    /* 슬램가 거리 2 */
    private Vector3 position_Of_Sector1_Of_Street2_In_Slum = new Vector3(816.9f, -941.0f, -10);
    private Vector3 position_Of_Sector2_Of_Street2_In_Slum = new Vector3(2096.9f, -941.0f, -10);

    /*시장 거리 1*/
    private Vector3 position_Of_Sector1_Of_Street1_In_Market = new Vector3(-3942.0f, 1200.0f, -10);
    private Vector3 position_Of_Sector2_Of_Street1_In_Market = new Vector3(-2662.0f, 1200.0f, -10);
    private Vector3 position_Of_Sector3_Of_Street1_In_Market = new Vector3(-1382.0f, 1200.0f, -10);

    /* 시장 거리 2 */
    private Vector3 position_Of_Sector1_Of_Street2_In_Market = new Vector3(0, 1200.0f, -10);
    private Vector3 position_Of_Sector2_Of_Street2_In_Market = new Vector3(1280.0f, 1200.0f, -10);
    private Vector3 position_Of_Sector3_Of_Street2_In_Market = new Vector3(2560.0f, 1200.0f, -10);

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

    /* 도심 거리 1 */
    private Vector3 position_Of_Sector1_Of_Street1_In_Downtown = new Vector3(5000f, 100f, -10);
    private Vector3 position_Of_Sector2_Of_Street1_In_Downtown = new Vector3(6280f, 100f, -10);
    private Vector3 position_Of_Sector3_Of_Street1_In_Downtown = new Vector3(7560f, 100f, -10);

    /* 저택가 거리 1 */
    private Vector3 position_Of_Sector1_Of_Street1_In_Mansion = new Vector3(10000.0f, 1600.0f, -10);
    private Vector3 position_Of_Sector2_Of_Street1_In_Mansion = new Vector3(11280.0f, 1600.0f, -10);
    private Vector3 position_Of_Sector3_Of_Street1_In_Mansion = new Vector3(12560.0f, 1600.0f, -10);

    /* 저택가 거리 2 */
    private Vector3 position_Of_Sector1_Of_Street2_In_Mansion = new Vector3(13200.0f, 500.0f, -10);

    /* 저택가 거리 3 */
    private Vector3 position_Of_Sector1_Of_Street3_In_Mansion = new Vector3(13940.0f, 1600.0f, -10);
    private Vector3 position_Of_Sector2_Of_Street3_In_Mansion = new Vector3(15200.0f, 1600.0f, -10);
    private Vector3 position_Of_Sector3_Of_Street3_In_Mansion = new Vector3(16500.0f, 1600.0f, -10);

    /* 항구 거리 1 */
    private Vector3 position_Of_Sector1_Of_Street1_In_Harbor = new Vector3(0, 3800.0f, -10);
    private Vector3 position_Of_Sector2_Of_Street1_In_Harbor = new Vector3(1280.0f, 3800.0f, -10);

    /* 지부 거리 1 */
    private Vector3 position_Of_Sector1_Of_Street1_In_Chapter = new Vector3(6277.0f, 1600.0f, -10);

    /* 숲 거리 1 */
    private Vector3 position_Of_Sector1_Of_Street1_In_Forest = new Vector3(6277.0f, -1400.0f, -10);

    /* 숲 거리 2 */
    private Vector3 position_Of_Sector1_Of_Street2_In_Forest = new Vector3(5000.0f, -2500.0f, -10);
    private Vector3 position_Of_Sector2_Of_Street2_In_Forest = new Vector3(6280.0f, -2500.0f, -10);
    private Vector3 position_Of_Sector3_Of_Street2_In_Forest = new Vector3(7560.0f, -2500.0f, -10);

    /* 숲 거리 3 */
    private Vector3 position_Of_Sector1_Of_Street3_In_Forest = new Vector3(8940.0f, -2500.0f, -10);
    private Vector3 position_Of_Sector2_Of_Street3_In_Forest = new Vector3(10220.0f, -2500.0f, -10);
    private Vector3 position_Of_Sector3_Of_Street3_In_Forest = new Vector3(11500.0f, -2500.0f, -10);

    /* 레이나의 집 */
    private Vector3 position_Of_Sector1_Of_Raina_House = new Vector3(5000.0f, 7500.0f, -10);
    private Vector3 position_Of_Sector2_Of_Raina_House = new Vector3(6280.0f, 7500.0f, -10);
    private Vector3 position_Of_Sector3_Of_Raina_House = new Vector3(7560.0f, 7500.0f, -10);

    /* 발루아의 집 */
    private Vector3 position_Of_Sector1_Of_Balrua_House = new Vector3(5000.0f, 6400.0f, -10);
    private Vector3 position_Of_Sector2_Of_Balrua_House = new Vector3(6280.0f, 6400.0f, -10);

    /* 지부 */
    private Vector3 position_Of_Sector1_Of_Chapter_First_Floor = new Vector3(10300.0f, 3200.0f, -10);
    private Vector3 position_Of_Sector2_Of_Chapter_First_Floor = new Vector3(11580.0f, 3200.0f, -10);
    private Vector3 position_Of_Sector1_Of_Chapter_Second_Floor = new Vector3(10300.0f, 4300.0f, -10);
    private Vector3 position_Of_Sector2_Of_Chapter_Second_Floor = new Vector3(11580.0f, 4300.0f, -10);

    /* 자작의 저택 */
    private Vector3 position_Of_Sector1_Of_Viscount_Mansion_First_Floor = new Vector3(13400.0f, 3200.0f, -10);
    private Vector3 position_Of_Sector2_Of_Viscount_Mansion_First_Floor = new Vector3(14680.0f, 3200.0f, -10);
    private Vector3 position_Of_Sector3_Of_Viscount_Mansion_First_Floor = new Vector3(15960.0f, 3200.0f, -10);
    private Vector3 position_Of_Sector1_Of_Viscount_Mansion_Second_Floor = new Vector3(13400.0f, 4300.0f, -10);
    private Vector3 position_Of_Sector2_Of_Viscount_Mansion_Second_Floor = new Vector3(14680.0f, 4300.0f, -10);
    private Vector3 position_Of_Sector3_Of_Viscount_Mansion_Second_Floor = new Vector3(15960.0f, 4300.0f, -10);

    /* 총장의 저택 */
    private Vector3 position_Of_Sector1_Of_Presidents_Mansion_First_Floor = new Vector3(17800.0f, 3700.0f, -10);
    private Vector3 position_Of_Sector2_Of_Presidents_Mansion_First_Floor = new Vector3(19080.0f, 3700.0f, -10);
    private Vector3 position_Of_Sector3_Of_Presidents_Mansion_First_Floor = new Vector3(20360.0f, 3700.0f, -10);
    private Vector3 position_Of_Sector1_Of_Presidents_Mansion_Second_Floor = new Vector3(17800.0f, 4800.0f, -10);
    private Vector3 position_Of_Sector2_Of_Presidents_Mansion_Second_Floor = new Vector3(19080.0f, 4800.0f, -10);
    private Vector3 position_Of_Sector3_Of_Presidents_Mansion_Second_Floor = new Vector3(20360.0f, 4800.0f, -10);
    private Vector3 position_Of_Sector1_Of_Presidents_Mansion_Outhouse = new Vector3(18430.0f, 2600.0f, -10);
    private Vector3 position_Of_Sector2_Of_Presidents_Mansion_Outhouse = new Vector3(19710.0f, 2600.0f, -10);

    /* 살롱 */
    private Vector3 position_Of_Sector1_Of_Salon = new Vector3(9200.0f, 100.0f, -10);
    private Vector3 position_Of_Sector2_Of_Salon = new Vector3(10480.0f, 100.0f, -10);
    private Vector3 position_Of_Sector3_Of_Salon = new Vector3(11760.0f, 100.0f, -10);

    void Start()
    {
        whereIsPlayer = PlayerManager.instance.GetCurrentPosition();
    }

    void FixedUpdate()
    {
        SetCameraPosition();
    }

    public void SetCameraPosition()
    {
        whereIsPlayer = PlayerManager.instance.GetCurrentPosition();
        playerPosition = player.transform.localPosition;

        /* 나중에 최적화할 때, 겹칠 수 있는건 겹처서 처리하도록 해야함. -> 코드 수를 줄이자 (1224) */
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

        else if (whereIsPlayer == "Market_Street1")
        {
            if (playerPosition.x >= -4582.0f && playerPosition.x < -3302.0f)
            {
                transform.localPosition = position_Of_Sector1_Of_Street1_In_Market;
            }
            else if (playerPosition.x >= -3302.0f && playerPosition.x < -2022.0f)
            {
                transform.localPosition = position_Of_Sector2_Of_Street1_In_Market;
            }
            else if (playerPosition.x >= -2022.0f && playerPosition.x < -742.0f)
            {
                transform.localPosition = position_Of_Sector3_Of_Street1_In_Market;
            }
        }

        else if (whereIsPlayer == "Market_Street2")
        {
            if (playerPosition.x >= -640.0f && playerPosition.x < 640.0f)
            {
                transform.localPosition = position_Of_Sector1_Of_Street2_In_Market;
            }
            else if (playerPosition.x >= 640.0f && playerPosition.x < 1920.0f)
            {
                transform.localPosition = position_Of_Sector2_Of_Street2_In_Market;
            }
            else if (playerPosition.x >= 1920.0f && playerPosition.x < 3200.0f)
            {
                transform.localPosition = position_Of_Sector3_Of_Street2_In_Market;
            }
        }

        else if (whereIsPlayer == "Village_Street1")
        {

            if (playerPosition.x >= 4360.0f && playerPosition.x < 5640.0f)
            {
                transform.localPosition = position_Of_Sector1_Of_Street1_In_Village;
            }
            else if (playerPosition.x >= 5640.0f && playerPosition.x < 6920.0f)
            {
                transform.localPosition = position_Of_Sector2_Of_Street1_In_Village;
            }
            else if (playerPosition.x >= 6920.0f && playerPosition.x < 8200.0f)
            {
                transform.localPosition = position_Of_Sector3_Of_Street1_In_Village;
            }
        }

        else if (whereIsPlayer == "Village_Street2")
        {

            if (playerPosition.x >= 4360.0f && playerPosition.x < 5640.0f)
            {
                transform.localPosition = position_Of_Sector1_Of_Street2_In_Village;
            }
            else if (playerPosition.x >= 5640.0f && playerPosition.x < 6920.0f)
            {
                transform.localPosition = position_Of_Sector2_Of_Street2_In_Village;
            }
            else if (playerPosition.x >= 6920.0f && playerPosition.x < 8200.0f)
            {
                transform.localPosition = position_Of_Sector3_Of_Street2_In_Village;
            }
        }

        else if (whereIsPlayer == "Village_Street3")
        {

            if (playerPosition.x >= 4360.0f && playerPosition.x < 5640.0f)
            {
                transform.localPosition = position_Of_Sector1_Of_Street3_In_Village;
            }
            else if (playerPosition.x >= 5640.0f && playerPosition.x < 6920.0f)
            {
                transform.localPosition = position_Of_Sector2_Of_Street3_In_Village;
            }
            else if (playerPosition.x >= 6920.0f && playerPosition.x < 8200.0f)
            {
                transform.localPosition = position_Of_Sector3_Of_Street3_In_Village;
            }
        }

        else if (whereIsPlayer == "Downtown_Street1")
        {
            if (playerPosition.x >= 4360.0f && playerPosition.x < 5640.0f)
            {
                transform.localPosition = position_Of_Sector1_Of_Street1_In_Downtown;
            }
            else if (playerPosition.x >= 5640.0f && playerPosition.x < 6920.0f)
            {
                transform.localPosition = position_Of_Sector2_Of_Street1_In_Downtown;
            }
            else if (playerPosition.x >= 6920.0f && playerPosition.x < 8200.0f)
            {
                transform.localPosition = position_Of_Sector3_Of_Street1_In_Downtown;
            }
        }

        else if (whereIsPlayer == "Mansion_Street1")
        {
            if (playerPosition.x >= 9360.0f && playerPosition.x < 10640.0f)
            {
                transform.localPosition = position_Of_Sector1_Of_Street1_In_Mansion;
            }
            else if (playerPosition.x >= 10640.0f && playerPosition.x < 11920.0f)
            {
                transform.localPosition = position_Of_Sector2_Of_Street1_In_Mansion;
            }
            else if (playerPosition.x >= 11920.0f && playerPosition.x < 13200.0f)
            {
                transform.localPosition = position_Of_Sector3_Of_Street1_In_Mansion;
            }
        }

        else if (whereIsPlayer == "Mansion_Street2")
        {
            if (playerPosition.x >= 12560.0f && playerPosition.x < 13840.0f)
            {
                transform.localPosition = position_Of_Sector1_Of_Street2_In_Mansion;
            }
        }

        else if (whereIsPlayer == "Mansion_Street3")
        {
            if (playerPosition.x >= 13300.0f && playerPosition.x < 14580.0f)
            {
                transform.localPosition = position_Of_Sector1_Of_Street3_In_Mansion;
            }
            else if (playerPosition.x >= 14580.0f && playerPosition.x < 15860.0f)
            {
                transform.localPosition = position_Of_Sector2_Of_Street3_In_Mansion;
            }
            else if (playerPosition.x >= 15860.0f && playerPosition.x < 17140.0f)
            {
                transform.localPosition = position_Of_Sector3_Of_Street3_In_Mansion;
            }
        }

        else if (whereIsPlayer == "Harbor_Street1")
        {
            if (playerPosition.x >= -640.0f && playerPosition.x < 640.0f)
            {
                transform.localPosition = position_Of_Sector1_Of_Street1_In_Harbor;
            }
            else if (playerPosition.x >= 640.0f && playerPosition.x < 1920.0f)
            {
                transform.localPosition = position_Of_Sector2_Of_Street1_In_Harbor;
            }
        }

        else if (whereIsPlayer == "Chapter_Street1")
        {
            if (playerPosition.x >= 5637.0f && playerPosition.x < 6917.0f)
            {
                transform.localPosition = position_Of_Sector1_Of_Street1_In_Chapter;
            }
        }

        else if (whereIsPlayer == "Forest_Street1")
        {
            if (playerPosition.x >= 5637.0f && playerPosition.x < 6917.0f)
            {
                transform.localPosition = position_Of_Sector1_Of_Street1_In_Forest;
            }
        }

        else if (whereIsPlayer == "Forest_Street2")
        {
            if (playerPosition.x >= 4360.0f && playerPosition.x < 5640.0f)
            {
                transform.localPosition = position_Of_Sector1_Of_Street2_In_Forest;
            }
            else if (playerPosition.x >= 5640.0f && playerPosition.x < 6920.0f)
            {
                transform.localPosition = position_Of_Sector2_Of_Street2_In_Forest;
            }
            else if (playerPosition.x >= 6920.0f && playerPosition.x < 8200.0f)
            {
                transform.localPosition = position_Of_Sector3_Of_Street2_In_Forest;
            }
        }

        else if (whereIsPlayer == "Forest_Street3")
        {
            if (playerPosition.x >= 8300.0f && playerPosition.x < 9580.0f)
            {
                transform.localPosition = position_Of_Sector1_Of_Street3_In_Forest;
            }
            else if (playerPosition.x >= 9580.0f && playerPosition.x < 10860.0f)
            {
                transform.localPosition = position_Of_Sector2_Of_Street3_In_Forest;
            }
            else if (playerPosition.x >= 10860.0f && playerPosition.x < 12140.0f)
            {
                transform.localPosition = position_Of_Sector3_Of_Street3_In_Forest;
            }
        }
        else if (whereIsPlayer == "Village_Raina_House")
        {
            if (playerPosition.x >= 4360.0f && playerPosition.x < 5640.0f)
            {
                transform.localPosition = position_Of_Sector1_Of_Raina_House;
            }
            else if (playerPosition.x >= 5640.0f && playerPosition.x < 6920.0f)
            {
                transform.localPosition = position_Of_Sector2_Of_Raina_House;
            }
            else if (playerPosition.x >= 6920.0f && playerPosition.x < 8200.0f)
            {
                transform.localPosition = position_Of_Sector3_Of_Raina_House;
            }
        }
        else if (whereIsPlayer == "Village_Balrua_House")
        {
            if (playerPosition.x >= 4360.0f && playerPosition.x < 5640.0f)
            {
                transform.localPosition = position_Of_Sector1_Of_Balrua_House;
            }
            else if (playerPosition.x >= 5640.0f && playerPosition.x < 6920.0f)
            {
                transform.localPosition = position_Of_Sector2_Of_Balrua_House;
            }
        }
        else if (whereIsPlayer == "Chapter_Chapter_First_Floor")
        {
            if (playerPosition.x >= 9660.0f && playerPosition.x < 10940.0f)
            {
                transform.localPosition = position_Of_Sector1_Of_Chapter_First_Floor;
            }
            else if (playerPosition.x >= 10940.0f && playerPosition.x < 12220.0f)
            {
                transform.localPosition = position_Of_Sector2_Of_Chapter_First_Floor;
            }
        }
        else if (whereIsPlayer == "Chapter_Chapter_Second_Floor")
        {
            if (playerPosition.x >= 9660.0f && playerPosition.x < 10940.0f)
            {
                transform.localPosition = position_Of_Sector1_Of_Chapter_Second_Floor;
            }
            else if (playerPosition.x >= 10940.0f && playerPosition.x < 12220.0f)
            {
                transform.localPosition = position_Of_Sector2_Of_Chapter_Second_Floor;
            }
        }
        else if (whereIsPlayer == "Mansion_Viscount_Mansion_First_Floor")
        {
            if (playerPosition.x >= 12760.0f && playerPosition.x < 14040.0f)
            {
                transform.localPosition = position_Of_Sector1_Of_Viscount_Mansion_First_Floor;
            }
            else if (playerPosition.x >= 14040.0f && playerPosition.x < 15320.0f)
            {
                transform.localPosition = position_Of_Sector2_Of_Viscount_Mansion_First_Floor;
            }
            else if (playerPosition.x >= 15320.0f && playerPosition.x < 16600.0f)
            {
                transform.localPosition = position_Of_Sector3_Of_Viscount_Mansion_First_Floor;
            }
        }
        else if (whereIsPlayer == "Mansion_Viscount_Mansion_Second_Floor")
        {
            if (playerPosition.x >= 12760.0f && playerPosition.x < 14040.0f)
            {
                transform.localPosition = position_Of_Sector1_Of_Viscount_Mansion_Second_Floor;
            }
            else if (playerPosition.x >= 14040.0f && playerPosition.x < 15320.0f)
            {
                transform.localPosition = position_Of_Sector2_Of_Viscount_Mansion_Second_Floor;
            }
            else if (playerPosition.x >= 15320.0f && playerPosition.x < 16600.0f)
            {
                transform.localPosition = position_Of_Sector3_Of_Viscount_Mansion_Second_Floor;
            }
        }
        else if (whereIsPlayer == "Mansion_President_Mansion_First_Floor")
        {
            if (playerPosition.x >= 17160.0f && playerPosition.x < 18440.0f)
            {
                transform.localPosition = position_Of_Sector1_Of_Presidents_Mansion_First_Floor;
            }
            else if (playerPosition.x >= 18440.0f && playerPosition.x < 19720.0f)
            {
                transform.localPosition = position_Of_Sector2_Of_Presidents_Mansion_First_Floor;
            }
            else if (playerPosition.x >= 19720.0f && playerPosition.x < 21000.0f)
            {
                transform.localPosition = position_Of_Sector3_Of_Presidents_Mansion_First_Floor;
            }
        }
        else if (whereIsPlayer == "Mansion_President_Mansion_Second_Floor")
        {
            if (playerPosition.x >= 17160.0f && playerPosition.x < 18440.0f)
            {
                transform.localPosition = position_Of_Sector1_Of_Presidents_Mansion_Second_Floor;
            }
            else if (playerPosition.x >= 18440.0f && playerPosition.x < 19720.0f)
            {
                transform.localPosition = position_Of_Sector2_Of_Presidents_Mansion_Second_Floor;
            }
            else if (playerPosition.x >= 19720.0f && playerPosition.x < 21000.0f)
            {
                transform.localPosition = position_Of_Sector3_Of_Presidents_Mansion_Second_Floor;
            }
        }
        else if (whereIsPlayer == "Mansion_President_Mansion_Outhouse")
        {
            if (playerPosition.x >= 17790.0f && playerPosition.x < 19070.0f)
            {
                transform.localPosition = position_Of_Sector1_Of_Presidents_Mansion_Outhouse;
            }
            else if (playerPosition.x >= 19070.0f && playerPosition.x < 20350.0f)
            {
                transform.localPosition = position_Of_Sector2_Of_Presidents_Mansion_Outhouse;
            }
        }
        else if (whereIsPlayer == "Downtown_Salon")
        {
            if (playerPosition.x >= 8560.0f && playerPosition.x < 9840.0f)
            {
                transform.localPosition = position_Of_Sector1_Of_Salon;
            }
            else if (playerPosition.x >= 9840.0f && playerPosition.x < 11120.0f)
            {
                transform.localPosition = position_Of_Sector2_Of_Salon;
            }
            else if (playerPosition.x >= 11120.0f && playerPosition.x < 12400.0f)
            {
                transform.localPosition = position_Of_Sector3_Of_Salon;
            }
        }
    }
}
