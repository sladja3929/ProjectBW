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

    /* 카페 */
    private Vector3 position_Of_Sector1_Of_Cafe = new Vector3(9200.0f, -1150.0f, -10);
    private Vector3 position_Of_Sector2_Of_Cafe = new Vector3(10480.0f, -1150.0f, -10);

    /* 부동산 */
    private Vector3 position_Of_Sector1_Of_Realesatate = new Vector3(12200.0f, -1150.0f, -10);
    private Vector3 position_Of_Sector2_Of_Realesatate = new Vector3(13480.0f, -1150.0f, -10);

    /* 유람선 로비층 */
    private Vector3 position_Of_Sector1_Of_Cruise = new Vector3(-1100.0f, 5200.0f, -10);
    private Vector3 position_Of_Sector2_Of_Cruise = new Vector3(180.0f, 5200.0f, -10);
    private Vector3 position_Of_Sector3_Of_Cruise = new Vector3(1460.0f, 5200.0f, -10);

    /* 유람선 감금층 */
    private Vector3 position_Of_Sector1_Of_Prison = new Vector3(-1100.0f, 6200.0f, -10);
    private Vector3 position_Of_Sector2_Of_Prison = new Vector3(180.0f, 6200.0f, -10);
    private Vector3 position_Of_Sector3_Of_Prison = new Vector3(1460.0f, 6200.0f, -10);

    /* 남매의 집 */
    private Vector3 position_Of_Sector1_Of_BroSisHouse = new Vector3(9200.0f, -3500.0f, -10);
    private Vector3 position_Of_Sector2_Of_BroSisHouse = new Vector3(10480.0f, -3500.0f, -10);

    /* 정보상 */
    private Vector3 position_Of_Sector1_Of_InformationAgency = new Vector3(-3000.0f, 0.0f, -10);
    private Vector3 position_Of_Sector2_Of_InformationAgency = new Vector3(-1720.0f, 0.0f, -10);

    /* 총장의 사무실 */
    private Vector3 position_Of_Sector1_Of_PresidentOffice = new Vector3(10300.0f, 6500.0f, -10);
    private Vector3 position_Of_Sector2_Of_PresidentOffice = new Vector3(11580.0f, 6500.0f, -10);

    /* 비밀공간 */
    private Vector3 position_Of_Sector1_Of_SecretSpace = new Vector3(10300.0f, 5400.0f, -10);

    /* 주인공 사무실 */
    private Vector3 position_Of_Sector1_Of_MerteOffice = new Vector3(11800.0f, 5400.0f, -10);

    /* 자작의 손님방1,2 */
    private Vector3 position_Of_Sector1_Of_GuestRoom1 = new Vector3(13400.0f, 5400.0f, -10);
    private Vector3 position_Of_Sector1_Of_GuestRoom2 = new Vector3(13400.0f, 6500.0f, -10);

    /* 총장의 방 */
    private Vector3 position_Of_Sector1_Of_PresidentRoom = new Vector3(17800.0f, 5900.0f, -10);
    private Vector3 position_Of_Sector2_Of_PresidentRoom = new Vector3(19080.0f, 5900.0f, -10);

    /* 여자아이 방, 남자아이 방, 공부 방, 식사 방 */
    private Vector3 position_Of_Sector1_Of_GirlsRoom = new Vector3(18430.0f, 1500.0f, -10);
    private Vector3 position_Of_Sector1_Of_BoysRoom = new Vector3(20000.0f, 1500.0f, -10);
    private Vector3 position_Of_Sector1_Of_StudyRoom = new Vector3(21500.0f, 1500.0f, -10);
    private Vector3 position_Of_Sector1_Of_DiningRoom = new Vector3(23000.0f, 1500.0f, -10);

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
            if (playerPosition.x >= (position_Of_Sector1_Of_Street1_In_Slum.x - 640.0f) && playerPosition.x < (position_Of_Sector1_Of_Street1_In_Slum.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector1_Of_Street1_In_Slum;
            }
            else if (playerPosition.x >= (position_Of_Sector2_Of_Street1_In_Slum.x - 640.0f) && playerPosition.x < (position_Of_Sector2_Of_Street1_In_Slum.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector2_Of_Street1_In_Slum;

            }
            else if (playerPosition.x >= (position_Of_Sector3_Of_Street1_In_Slum.x - 640.0f) && playerPosition.x < (position_Of_Sector3_Of_Street1_In_Slum.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector3_Of_Street1_In_Slum;
            }
        }

        else if (whereIsPlayer == "Slum_Street2")
        {
            if (playerPosition.x >= (position_Of_Sector1_Of_Street2_In_Slum.x - 640.0f) && playerPosition.x < (position_Of_Sector1_Of_Street2_In_Slum.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector1_Of_Street2_In_Slum;
            }
            else if (playerPosition.x >= (position_Of_Sector2_Of_Street2_In_Slum.x - 640.0f) && playerPosition.x < (position_Of_Sector2_Of_Street2_In_Slum.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector2_Of_Street2_In_Slum;

            }
        }

        else if (whereIsPlayer == "Market_Street1")
        {
            if (playerPosition.x >= (position_Of_Sector1_Of_Street1_In_Market.x - 640.0f) && playerPosition.x < (position_Of_Sector1_Of_Street1_In_Market.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector1_Of_Street1_In_Market;
            }
            else if (playerPosition.x >= (position_Of_Sector2_Of_Street1_In_Market.x - 640.0f) && playerPosition.x < (position_Of_Sector2_Of_Street1_In_Market.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector2_Of_Street1_In_Market;
            }
            else if (playerPosition.x >= (position_Of_Sector3_Of_Street1_In_Market.x - 640.0f) && playerPosition.x < (position_Of_Sector3_Of_Street1_In_Market.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector3_Of_Street1_In_Market;
            }
        }

        else if (whereIsPlayer == "Market_Street2")
        {
            if (playerPosition.x >= (position_Of_Sector1_Of_Street2_In_Market.x - 640.0f) && playerPosition.x < (position_Of_Sector1_Of_Street2_In_Market.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector1_Of_Street2_In_Market;
            }
            else if (playerPosition.x >= (position_Of_Sector2_Of_Street2_In_Market.x - 640.0f) && playerPosition.x < (position_Of_Sector2_Of_Street2_In_Market.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector2_Of_Street2_In_Market;
            }
            else if (playerPosition.x >= (position_Of_Sector3_Of_Street2_In_Market.x - 640.0f) && playerPosition.x < (position_Of_Sector3_Of_Street2_In_Market.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector3_Of_Street2_In_Market;
            }
        }

        else if (whereIsPlayer == "Village_Street1")
        {

            if (playerPosition.x >= (position_Of_Sector1_Of_Street1_In_Village.x - 640.0f) && playerPosition.x < (position_Of_Sector1_Of_Street1_In_Village.x + 640.0f))
            {
                // 252번 이벤트에 필요한 이벤트 대화 발생용
                if (!EventManager.instance.isPlaying2032Conversation && PlayerManager.instance.TimeSlot.Equals("74"))
                    EventManager.instance.PlayEvent();
                transform.localPosition = position_Of_Sector1_Of_Street1_In_Village;
            }
            else if (playerPosition.x >= (position_Of_Sector2_Of_Street1_In_Village.x - 640.0f) && playerPosition.x < (position_Of_Sector2_Of_Street1_In_Village.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector2_Of_Street1_In_Village;
            }
            else if (playerPosition.x >= (position_Of_Sector3_Of_Street1_In_Village.x - 640.0f) && playerPosition.x < (position_Of_Sector3_Of_Street1_In_Village.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector3_Of_Street1_In_Village;
            }
        }

        else if (whereIsPlayer == "Village_Street2")
        {

            if (playerPosition.x >= (position_Of_Sector1_Of_Street2_In_Village.x - 640.0f) && playerPosition.x < (position_Of_Sector1_Of_Street2_In_Village.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector1_Of_Street2_In_Village;
            }
            else if (playerPosition.x >= (position_Of_Sector2_Of_Street2_In_Village.x - 640.0f) && playerPosition.x < (position_Of_Sector2_Of_Street2_In_Village.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector2_Of_Street2_In_Village;
            }
            else if (playerPosition.x >= (position_Of_Sector3_Of_Street2_In_Village.x - 640.0f) && playerPosition.x < (position_Of_Sector3_Of_Street2_In_Village.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector3_Of_Street2_In_Village;
            }
        }

        else if (whereIsPlayer == "Village_Street3")
        {

            if (playerPosition.x >= (position_Of_Sector1_Of_Street3_In_Village.x - 640.0f) && playerPosition.x < (position_Of_Sector1_Of_Street3_In_Village.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector1_Of_Street3_In_Village;
            }
            else if (playerPosition.x >= (position_Of_Sector2_Of_Street3_In_Village.x - 640.0f) && playerPosition.x < (position_Of_Sector2_Of_Street3_In_Village.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector2_Of_Street3_In_Village;
            }
            else if (playerPosition.x >= (position_Of_Sector3_Of_Street3_In_Village.x - 640.0f) && playerPosition.x < (position_Of_Sector3_Of_Street3_In_Village.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector3_Of_Street3_In_Village;
            }
        }

        else if (whereIsPlayer == "Downtown_Street1")
        {
            if (playerPosition.x >= (position_Of_Sector1_Of_Street1_In_Downtown.x - 640.0f) && playerPosition.x < (position_Of_Sector1_Of_Street1_In_Downtown.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector1_Of_Street1_In_Downtown;
            }
            else if (playerPosition.x >= (position_Of_Sector2_Of_Street1_In_Downtown.x - 640.0f) && playerPosition.x < (position_Of_Sector2_Of_Street1_In_Downtown.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector2_Of_Street1_In_Downtown;
            }
            else if (playerPosition.x >= (position_Of_Sector3_Of_Street1_In_Downtown.x - 640.0f) && playerPosition.x < (position_Of_Sector3_Of_Street1_In_Downtown.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector3_Of_Street1_In_Downtown;
            }
        }

        else if (whereIsPlayer == "Mansion_Street1")
        {
            if (playerPosition.x >= (position_Of_Sector1_Of_Street1_In_Mansion.x - 640.0f) && playerPosition.x < (position_Of_Sector1_Of_Street1_In_Mansion.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector1_Of_Street1_In_Mansion;
            }
            else if (playerPosition.x >= (position_Of_Sector2_Of_Street1_In_Mansion.x - 640.0f) && playerPosition.x < (position_Of_Sector2_Of_Street1_In_Mansion.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector2_Of_Street1_In_Mansion;
            }
            else if (playerPosition.x >= (position_Of_Sector3_Of_Street1_In_Mansion.x - 640.0f) && playerPosition.x < (position_Of_Sector3_Of_Street1_In_Mansion.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector3_Of_Street1_In_Mansion;
            }
        }

        else if (whereIsPlayer == "Mansion_Street2")
        {
            if (playerPosition.x >= (position_Of_Sector1_Of_Street2_In_Mansion.x - 640.0f) && playerPosition.x < (position_Of_Sector1_Of_Street2_In_Mansion.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector1_Of_Street2_In_Mansion;
            }
        }

        else if (whereIsPlayer == "Mansion_Street3")
        {
            if (playerPosition.x >= (position_Of_Sector1_Of_Street3_In_Mansion.x - 640.0f) && playerPosition.x < (position_Of_Sector1_Of_Street3_In_Mansion.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector1_Of_Street3_In_Mansion;
            }
            else if (playerPosition.x >= (position_Of_Sector2_Of_Street3_In_Mansion.x - 640.0f) && playerPosition.x < (position_Of_Sector2_Of_Street3_In_Mansion.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector2_Of_Street3_In_Mansion;
            }
            else if (playerPosition.x >= (position_Of_Sector3_Of_Street3_In_Mansion.x - 640.0f) && playerPosition.x < (position_Of_Sector3_Of_Street3_In_Mansion.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector3_Of_Street3_In_Mansion;
            }
        }

        else if (whereIsPlayer == "Harbor_Street1")
        {
            if (playerPosition.x >= (position_Of_Sector1_Of_Street1_In_Harbor.x - 640.0f) && playerPosition.x < (position_Of_Sector1_Of_Street1_In_Harbor.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector1_Of_Street1_In_Harbor;
            }
            else if (playerPosition.x >= (position_Of_Sector2_Of_Street1_In_Harbor.x - 640.0f) && playerPosition.x < (position_Of_Sector2_Of_Street1_In_Harbor.x + 640.0f))
            {
                // 208, 209번 이벤트에 필요한 이벤트 대화 발생용
                if(!EventManager.instance.isPlaying8014Conversation && PlayerManager.instance.TimeSlot.Equals("72"))
                    EventManager.instance.PlayEvent();
                transform.localPosition = position_Of_Sector2_Of_Street1_In_Harbor;
            }
        }

        else if (whereIsPlayer == "Chapter_Street1")
        {
            if (playerPosition.x >= (position_Of_Sector1_Of_Street1_In_Chapter.x - 640.0f) && playerPosition.x < (position_Of_Sector1_Of_Street1_In_Chapter.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector1_Of_Street1_In_Chapter;
            }
        }

        else if (whereIsPlayer == "Forest_Street1")
        {
            if (playerPosition.x >= (position_Of_Sector1_Of_Street1_In_Forest.x - 640.0f) && playerPosition.x < (position_Of_Sector1_Of_Street1_In_Forest.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector1_Of_Street1_In_Forest;
            }
        }

        else if (whereIsPlayer == "Forest_Street2")
        {
            if (playerPosition.x >= (position_Of_Sector1_Of_Street2_In_Forest.x - 640.0f) && playerPosition.x < (position_Of_Sector1_Of_Street2_In_Forest.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector1_Of_Street2_In_Forest;
            }
            else if (playerPosition.x >= (position_Of_Sector2_Of_Street2_In_Forest.x - 640.0f) && playerPosition.x < (position_Of_Sector2_Of_Street2_In_Forest.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector2_Of_Street2_In_Forest;
            }
            else if (playerPosition.x >= (position_Of_Sector3_Of_Street2_In_Forest.x - 640.0f) && playerPosition.x < (position_Of_Sector3_Of_Street2_In_Forest.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector3_Of_Street2_In_Forest;
            }
        }

        else if (whereIsPlayer == "Forest_Street3")
        {
            if (playerPosition.x >= (position_Of_Sector1_Of_Street3_In_Forest.x - 640.0f) && playerPosition.x < (position_Of_Sector1_Of_Street3_In_Forest.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector1_Of_Street3_In_Forest;
            }
            else if (playerPosition.x >= (position_Of_Sector2_Of_Street3_In_Forest.x - 640.0f) && playerPosition.x < (position_Of_Sector2_Of_Street3_In_Forest.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector2_Of_Street3_In_Forest;
            }
            else if (playerPosition.x >= (position_Of_Sector3_Of_Street3_In_Forest.x - 640.0f) && playerPosition.x < (position_Of_Sector3_Of_Street3_In_Forest.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector3_Of_Street3_In_Forest;
            }
        }
        else if (whereIsPlayer == "Village_Raina_House")
        {
            if (playerPosition.x >= (position_Of_Sector1_Of_Raina_House.x - 640.0f) && playerPosition.x < (position_Of_Sector1_Of_Raina_House.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector1_Of_Raina_House;
            }
            else if (playerPosition.x >= (position_Of_Sector2_Of_Raina_House.x - 640.0f) && playerPosition.x < (position_Of_Sector2_Of_Raina_House.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector2_Of_Raina_House;
            }
            else if (playerPosition.x >= (position_Of_Sector3_Of_Raina_House.x - 640.0f) && playerPosition.x < (position_Of_Sector3_Of_Raina_House.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector3_Of_Raina_House;
            }
        }
        else if (whereIsPlayer == "Village_Balrua_House")
        {
            if (playerPosition.x >= (position_Of_Sector1_Of_Balrua_House.x - 640.0f) && playerPosition.x < (position_Of_Sector1_Of_Balrua_House.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector1_Of_Balrua_House;
            }
            else if (playerPosition.x >= (position_Of_Sector2_Of_Balrua_House.x - 640.0f) && playerPosition.x < (position_Of_Sector2_Of_Balrua_House.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector2_Of_Balrua_House;
            }
        }
        else if (whereIsPlayer == "Chapter_Chapter_First_Floor")
        {
            if (playerPosition.x >= (position_Of_Sector1_Of_Chapter_First_Floor.x - 640.0f) && playerPosition.x < (position_Of_Sector1_Of_Chapter_First_Floor.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector1_Of_Chapter_First_Floor;
            }
            else if (playerPosition.x >= (position_Of_Sector2_Of_Chapter_First_Floor.x - 640.0f) && playerPosition.x < (position_Of_Sector2_Of_Chapter_First_Floor.x + 640.0f))
            { 
                transform.localPosition = position_Of_Sector2_Of_Chapter_First_Floor;
            }
        }
        else if (whereIsPlayer == "Chapter_Chapter_Second_Floor")
        {
            if (playerPosition.x >= (position_Of_Sector1_Of_Chapter_Second_Floor.x - 640.0f) && playerPosition.x < (position_Of_Sector1_Of_Chapter_Second_Floor.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector1_Of_Chapter_Second_Floor;
            }
            else if (playerPosition.x >= (position_Of_Sector2_Of_Chapter_Second_Floor.x - 640.0f) && playerPosition.x < (position_Of_Sector2_Of_Chapter_Second_Floor.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector2_Of_Chapter_Second_Floor;
            }
        }
        else if (whereIsPlayer == "Mansion_Viscount_Mansion_First_Floor")
        {
            if (playerPosition.x >= (position_Of_Sector1_Of_Viscount_Mansion_First_Floor.x - 640.0f) && playerPosition.x < (position_Of_Sector1_Of_Viscount_Mansion_First_Floor.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector1_Of_Viscount_Mansion_First_Floor;
            }
            else if (playerPosition.x >= (position_Of_Sector2_Of_Viscount_Mansion_First_Floor.x - 640.0f) && playerPosition.x < (position_Of_Sector2_Of_Viscount_Mansion_First_Floor.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector2_Of_Viscount_Mansion_First_Floor;
            }
            else if (playerPosition.x >= (position_Of_Sector3_Of_Viscount_Mansion_First_Floor.x - 640.0f) && playerPosition.x < (position_Of_Sector3_Of_Viscount_Mansion_First_Floor.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector3_Of_Viscount_Mansion_First_Floor;
            }
        }
        else if (whereIsPlayer == "Mansion_Viscount_Mansion_Second_Floor")
        {
            if (playerPosition.x >= (position_Of_Sector1_Of_Viscount_Mansion_Second_Floor.x - 640.0f) && playerPosition.x < (position_Of_Sector1_Of_Viscount_Mansion_Second_Floor.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector1_Of_Viscount_Mansion_Second_Floor;
            }
            else if (playerPosition.x >= (position_Of_Sector2_Of_Viscount_Mansion_Second_Floor.x - 640.0f) && playerPosition.x < (position_Of_Sector2_Of_Viscount_Mansion_Second_Floor.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector2_Of_Viscount_Mansion_Second_Floor;
            }
            else if (playerPosition.x >= (position_Of_Sector3_Of_Viscount_Mansion_Second_Floor.x - 640.0f) && playerPosition.x < (position_Of_Sector3_Of_Viscount_Mansion_Second_Floor.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector3_Of_Viscount_Mansion_Second_Floor;
            }
        }
        else if (whereIsPlayer == "Mansion_President_Mansion_First_Floor")
        {
            if (playerPosition.x >= (position_Of_Sector1_Of_Presidents_Mansion_First_Floor.x - 640.0f) && playerPosition.x < (position_Of_Sector1_Of_Presidents_Mansion_First_Floor.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector1_Of_Presidents_Mansion_First_Floor;
            }
            else if (playerPosition.x >= (position_Of_Sector2_Of_Presidents_Mansion_First_Floor.x - 640.0f) && playerPosition.x < (position_Of_Sector2_Of_Presidents_Mansion_First_Floor.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector2_Of_Presidents_Mansion_First_Floor;
            }
            else if (playerPosition.x >= (position_Of_Sector3_Of_Presidents_Mansion_First_Floor.x - 640.0f) && playerPosition.x < (position_Of_Sector3_Of_Presidents_Mansion_First_Floor.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector3_Of_Presidents_Mansion_First_Floor;
            }
        }
        else if (whereIsPlayer == "Mansion_President_Mansion_Second_Floor")
        {
            if (playerPosition.x >= (position_Of_Sector1_Of_Presidents_Mansion_Second_Floor.x - 640.0f) && playerPosition.x < (position_Of_Sector1_Of_Presidents_Mansion_Second_Floor.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector1_Of_Presidents_Mansion_Second_Floor;
            }
            else if (playerPosition.x >= (position_Of_Sector2_Of_Presidents_Mansion_Second_Floor.x - 640.0f) && playerPosition.x < (position_Of_Sector2_Of_Presidents_Mansion_Second_Floor.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector2_Of_Presidents_Mansion_Second_Floor;
            }
            else if (playerPosition.x >= (position_Of_Sector3_Of_Presidents_Mansion_Second_Floor.x - 640.0f) && playerPosition.x < (position_Of_Sector3_Of_Presidents_Mansion_Second_Floor.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector3_Of_Presidents_Mansion_Second_Floor;
            }
        }
        else if (whereIsPlayer == "Mansion_President_Mansion_Outhouse")
        {
            if (playerPosition.x >= (position_Of_Sector1_Of_Presidents_Mansion_Outhouse.x - 640.0f) && playerPosition.x < (position_Of_Sector1_Of_Presidents_Mansion_Outhouse.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector1_Of_Presidents_Mansion_Outhouse;
            }
            else if (playerPosition.x >= (position_Of_Sector2_Of_Presidents_Mansion_Outhouse.x - 640.0f) && playerPosition.x < (position_Of_Sector2_Of_Presidents_Mansion_Outhouse.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector2_Of_Presidents_Mansion_Outhouse;
            }
        }
        else if (whereIsPlayer == "Downtown_Salon")
        {
            if (playerPosition.x >= (position_Of_Sector1_Of_Salon.x - 640.0f) && playerPosition.x < (position_Of_Sector1_Of_Salon.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector1_Of_Salon;
            }
            else if (playerPosition.x >= (position_Of_Sector2_Of_Salon.x - 640.0f) && playerPosition.x < (position_Of_Sector2_Of_Salon.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector2_Of_Salon;
            }
            else if (playerPosition.x >= (position_Of_Sector3_Of_Salon.x - 640.0f) && playerPosition.x < (position_Of_Sector3_Of_Salon.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector3_Of_Salon;
            }
        }
        else if (whereIsPlayer == "Downtown_Cafe")
        {
            if (playerPosition.x >= (position_Of_Sector1_Of_Cafe.x - 640.0f) && playerPosition.x < (position_Of_Sector1_Of_Cafe.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector1_Of_Cafe;
            }
            else if (playerPosition.x >= (position_Of_Sector2_Of_Cafe.x - 640.0f) && playerPosition.x < (position_Of_Sector2_Of_Cafe.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector2_Of_Cafe;
            }
        }
        else if (whereIsPlayer == "Downtown_Real_estate")
        {
            if (playerPosition.x >= (position_Of_Sector1_Of_Realesatate.x - 640.0f) && playerPosition.x < (position_Of_Sector1_Of_Realesatate.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector1_Of_Realesatate;
            }
            else if (playerPosition.x >= (position_Of_Sector2_Of_Realesatate.x - 640.0f) && playerPosition.x < (position_Of_Sector2_Of_Realesatate.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector2_Of_Realesatate;
            }
        }
        else if (whereIsPlayer == "Harbor_Cruise")
        {
            if (playerPosition.x >= (position_Of_Sector1_Of_Cruise.x - 640.0f) && playerPosition.x < (position_Of_Sector1_Of_Cruise.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector1_Of_Cruise;
            }
            else if (playerPosition.x >= (position_Of_Sector2_Of_Cruise.x - 640.0f) && playerPosition.x < (position_Of_Sector2_Of_Cruise.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector2_Of_Cruise;
            }
            else if (playerPosition.x >= (position_Of_Sector3_Of_Cruise.x - 640.0f) && playerPosition.x < (position_Of_Sector3_Of_Cruise.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector3_Of_Cruise;
            }
        }
        else if (whereIsPlayer == "Harbor_Prison")
        {
            if (playerPosition.x >= (position_Of_Sector1_Of_Prison.x - 640.0f) && playerPosition.x < (position_Of_Sector1_Of_Prison.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector1_Of_Prison;
            }
            else if (playerPosition.x >= (position_Of_Sector2_Of_Prison.x - 640.0f) && playerPosition.x < (position_Of_Sector2_Of_Prison.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector2_Of_Prison;
            }
            else if (playerPosition.x >= (position_Of_Sector3_Of_Prison.x - 640.0f) && playerPosition.x < (position_Of_Sector3_Of_Prison.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector3_Of_Prison;
            }
        }
        else if (whereIsPlayer == "Forest_Bro_sis_home")
        {
            if (playerPosition.x >= (position_Of_Sector1_Of_BroSisHouse.x - 640.0f) && playerPosition.x < (position_Of_Sector1_Of_BroSisHouse.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector1_Of_BroSisHouse;
            }
            else if (playerPosition.x >= (position_Of_Sector2_Of_BroSisHouse.x - 640.0f) && playerPosition.x < (position_Of_Sector2_Of_BroSisHouse.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector2_Of_BroSisHouse;
            }
        }
        else if (whereIsPlayer == "Slum_Information_agency")
        {
            if (playerPosition.x >= (position_Of_Sector1_Of_InformationAgency.x - 640.0f) && playerPosition.x < (position_Of_Sector1_Of_InformationAgency.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector1_Of_InformationAgency;
            }
            else if (playerPosition.x >= (position_Of_Sector2_Of_InformationAgency.x - 640.0f) && playerPosition.x < (position_Of_Sector2_Of_InformationAgency.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector2_Of_InformationAgency;
            }
        }
        else if (whereIsPlayer == "Chapter_President_Office")
        {
            if (playerPosition.x >= (position_Of_Sector1_Of_PresidentOffice.x - 640.0f) && playerPosition.x < (position_Of_Sector1_Of_PresidentOffice.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector1_Of_PresidentOffice;
            }
            else if (playerPosition.x >= (position_Of_Sector2_Of_PresidentOffice.x - 640.0f) && playerPosition.x < (position_Of_Sector2_Of_PresidentOffice.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector2_Of_PresidentOffice;
            }
        }
        else if (whereIsPlayer == "Chapter_Secret_Space")
        {
            if (playerPosition.x >= (position_Of_Sector1_Of_SecretSpace.x - 640.0f) && playerPosition.x < (position_Of_Sector1_Of_SecretSpace.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector1_Of_SecretSpace;
            }
        }
        else if (whereIsPlayer == "Chapter_Merte_Office")
        {
            if (playerPosition.x >= (position_Of_Sector1_Of_MerteOffice.x - 640.0f) && playerPosition.x < (position_Of_Sector1_Of_MerteOffice.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector1_Of_MerteOffice;
            }
        }
        else if (whereIsPlayer == "Mansion_Guest_Room1")
        {
            if (playerPosition.x >= (position_Of_Sector1_Of_GuestRoom1.x - 640.0f) && playerPosition.x < (position_Of_Sector1_Of_GuestRoom1.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector1_Of_GuestRoom1;
            }
        }
        else if (whereIsPlayer == "Mansion_Guest_Room2")
        {
            if (playerPosition.x >= (position_Of_Sector1_Of_GuestRoom2.x - 640.0f) && playerPosition.x < (position_Of_Sector1_Of_GuestRoom2.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector1_Of_GuestRoom2;
            }
        }
        else if (whereIsPlayer == "Mansion_President_Room")
        {
            if (playerPosition.x >= (position_Of_Sector1_Of_PresidentRoom.x - 640.0f) && playerPosition.x < (position_Of_Sector1_Of_PresidentRoom.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector1_Of_PresidentRoom;
            }
            else if (playerPosition.x >= (position_Of_Sector2_Of_PresidentRoom.x - 640.0f) && playerPosition.x < (position_Of_Sector2_Of_PresidentRoom.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector2_Of_PresidentRoom;
            }
        }
        else if (whereIsPlayer == "Mansion_Girls_Room")
        {
            if (playerPosition.x >= (position_Of_Sector1_Of_GirlsRoom.x - 640.0f) && playerPosition.x < (position_Of_Sector1_Of_GirlsRoom.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector1_Of_GirlsRoom;
            }
        }
        else if (whereIsPlayer == "Mansion_Boys_Room")
        {
            if (playerPosition.x >= (position_Of_Sector1_Of_BoysRoom.x - 640.0f) && playerPosition.x < (position_Of_Sector1_Of_BoysRoom.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector1_Of_BoysRoom;
            }
        }
        else if (whereIsPlayer == "Mansion_Study_Room")
        {
            if (playerPosition.x >= (position_Of_Sector1_Of_StudyRoom.x - 640.0f) && playerPosition.x < (position_Of_Sector1_Of_StudyRoom.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector1_Of_StudyRoom;
            }
        }
        else if (whereIsPlayer == "Mansion_Dining_Room")
        {
            if (playerPosition.x >= (position_Of_Sector1_Of_DiningRoom.x - 640.0f) && playerPosition.x < (position_Of_Sector1_Of_DiningRoom.x + 640.0f))
            {
                transform.localPosition = position_Of_Sector1_Of_DiningRoom;
            }
        }
    }
}
