using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance = null;

    private List<string> eventIndexList;    // 모든 이벤트 추가 시, 삭제해도 될 변수

    // npc를 나타나게 하거나, 없애야 할때 사용할 npc들의 리스트
    [SerializeField]
    private List<GameObject> npcListForEvent;

    private int tempIndex;

    private bool hasBeenInHarbor;
    [SerializeField]
    private Transform positionOfPrisonInCruise;
    [SerializeField]
    private Transform positionOfMerte;
    [SerializeField]
    private Transform positionOfCruiseOutside;
    private bool isActivatedEvent222;
    public bool triggerKickMerte; // 222번 이벤트에 사용
    [SerializeField]
    private Transform positionOfMainCamera; // 252번 이벤트에 사용
    private Vector3 position_Of_Sector1_Of_Street1_In_Village = new Vector3(5000f, 5300f, -10);
    public bool isPlaying8014Conversation = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        eventIndexList = new List<string>();
        hasBeenInHarbor = false;
        isActivatedEvent222 = false;

        /* for test */
        // 이벤트 시스템 구현으로 인한 주석처리(테스트용, 1월 23일)
        //AddToEventIndexList("202");    //발루아 등장 이벤트 index 추가

        // 항구의 쉐렌, 악당 1, 악당 2를 비활성화 시키고, 주택가의 쉐렌을 활성화 시키기 위한 이벤트 저장
        AddToEventIndexList("0209");

        // id 200번 이벤트부터 253번 이벤트까지 저장
        for (int i = 200; i <= 254; i++)
            AddToEventIndexList(i.ToString());

        DisableNpcForEvent();
    }

    // 추후에 이벤트 테이블 파일에서 이벤트들 불러올때 사용할 것임.
    // 혹은 대사를 불러오는 동안에 이 함수를 이용하여 발생할 수 있는 이벤트의 인덱스를 추가할 수 있음.
    public void AddToEventIndexList(string eventIndex)
    {
        eventIndexList.Add(eventIndex);
    }

    // npcList에 있는 모든 npc들 비활성화
    public void DisableNpcForEvent()
    {
        for (int i = 0; i < npcListForEvent.Count; i++)
        {
            if (i == 4 || i == 14 || i == 30 || i == 20 || i == 23 || i == 26) { }
            else
                npcListForEvent[i].SetActive(false);
        }
    }

    //NPC의 발생 뿐만 아니라, 특정한 이벤트들도 다룰 수 있도록 함수 명 변경 필요(11/12)
    // npc의 나타남과 사라짐을 다루는 이벤트 함수(주로 대화로 인해 이루어지는 이벤트 -> 1월 23일에 추가
    // 그리고 대화가 발생시키는 이벤트들을 모아 놓은 함수
    public void ActivateNpcForEvent(string eventIndex)
    {
        if (CheckEventIndexList(eventIndex))
        {
            switch (eventIndex)
            {
                // 체스미터 등장 이벤트
                // 72시간대가 되면 도심 분수대 앞에 체스미터가 나와야함.(추후 추가할 것)
                case "200":
                    if(PlayerManager.instance.TimeSlot.Equals("71") && PlayerManager.instance.TimeSlot.Equals("53"))
                        PlayerManager.instance.AddEventCodeToList(eventIndex);
                    break;
                case "201":
                    if (PlayerManager.instance.TimeSlot.Equals("71") && (PlayerManager.instance.NumOfAct.Equals("53") || PlayerManager.instance.NumOfAct.Equals("54")))
                        PlayerManager.instance.AddEventCodeToList(eventIndex);
                    break;
                // 발루아 등장 이벤트(71에서 72로 시간대가 넘어갈경우 발루아는 218번이 발생하기 전까지 비활성화 되어야함(나중에 추가할 것)
                case "202":
                case "203":
                    PlayerManager.instance.AddEventCodeToList(eventIndex);
                    break;
                case "204":
                    if (PlayerManager.instance.TimeSlot.Equals("71") && PlayerManager.instance.TimeSlot.Equals("53"))
                        PlayerManager.instance.AddEventCodeToList(eventIndex);
                    break;
                case "211":
                    if ( (PlayerManager.instance.TimeSlot.Equals("71") || PlayerManager.instance.TimeSlot.Equals("72")) && PlayerManager.instance.TimeSlot.Equals("53"))
                        PlayerManager.instance.AddEventCodeToList(eventIndex);
                    break;
                case "212":
                case "213":
                case "214":
                    if (PlayerManager.instance.TimeSlot.Equals("71") && PlayerManager.instance.TimeSlot.Equals("53"))
                        PlayerManager.instance.AddEventCodeToList(eventIndex);
                    break;
                case "205": // 해당 이벤트관련 작업 완료
                    if (PlayerManager.instance.NumOfAct.Equals("53"))
                        PlayerManager.instance.AddEventCodeToList(eventIndex);
                    break;
                case "221":
                    PlayerManager.instance.isInvestigated_StrangeDoor = true;
                    PlayerManager.instance.AddEventCodeToList(eventIndex);
                    break;
                case "209":
                    PlayerManager.instance.AddEventCodeToList(eventIndex);
                    break;
                case "210":
                case "215":
                case "216":
                case "217":
                case "220":
                case "223":
                case "224":
                case "225":
                case "226": // 대화테이블에 4025묶음의 새로운 이벤트 속성값을 226으로 넣어야함
                case "227":
                    PlayerManager.instance.AddEventCodeToList(eventIndex);
                    break;
                case "232":
                case "240":
                case "243":
                case "252":
                case "253":
                case "254":
                    PlayerManager.instance.AddEventCodeToList(eventIndex);
                    break;

                // 정보상 건물 안의 엑스트라들 등장(인물 배치후, 적용할것)
                case "206":
                    if (PlayerManager.instance.TimeSlot.Equals("71") && PlayerManager.instance.NumOfAct.Equals("53"))
                    {
                        PlayerManager.instance.AddEventCodeToList(eventIndex);
                    }
                    break;

                case "218":
                    PlayerManager.instance.DeleteEventCodeFromList("202");
                    PlayerManager.instance.AddEventCodeToList(eventIndex);
                    break;

                default:

                    break;
            }
        }
    }

    // 해당 eventIndex가 eventIndexList에 존재 하는지 체크
    public bool CheckEventIndexList(string eventIndex)
    {
        if (eventIndexList.Contains(eventIndex))
            return true;
        else
            return false;
    }

    // 플레이어의 행동으로 인해 발생되는 이벤트들을 모은 곳 (맵 이동, 오브젝트와의 상호작용 등을 할 때 실행)
    // ActivateNpcForEvent 함수에 있는 이벤트들도 몇몇 포함되어 있음.
    public void PlayEvent()
    {
        // 특정 인물 등장 이벤트 처리 시작
        if (PlayerManager.instance.CheckEventCodeFromPlayedEventList("200") && PlayerManager.instance.TimeSlot.Equals("72"))
        {
            //tempIndex = npcListForEvent.FindIndex(x => x.gameObject.name == "체스미터");
            if (!npcListForEvent[1].activeSelf)
                npcListForEvent[1].SetActive(true);
        }

        if (PlayerManager.instance.CheckEventCodeFromPlayedEventList("202"))
        {
            if (PlayerManager.instance.TimeSlot.Equals("71"))
            {
                //tempIndex = npcListForEvent.FindIndex(x => x.gameObject.name == "발루아");
                if (!npcListForEvent[0].activeSelf)
                    npcListForEvent[0].SetActive(true);
            }

            if (!PlayerManager.instance.TimeSlot.Equals("71"))
            {
                npcListForEvent[0].SetActive(false);
            }
        }

        if (!PlayerManager.instance.CheckEventCodeFromPlayedEventList("207") && PlayerManager.instance.TimeSlot.Equals("72"))
        {
            //index 5 ~ 12 : 정보상 엑스트라들
            for (int i = 5; i <= 12; i++)
            {
                if (!npcListForEvent[i].activeSelf)
                    npcListForEvent[i].SetActive(true);
            }
            PlayerManager.instance.AddEventCodeToList("207");
        }

        if (PlayerManager.instance.CheckEventCodeFromPlayedEventList("218"))
        {
            if (PlayerManager.instance.TimeSlot.Equals("72"))
            {
                if (!npcListForEvent[0].activeSelf)
                    npcListForEvent[0].SetActive(true);
            }
            else
            {
                if (!npcListForEvent[0].activeSelf)
                    npcListForEvent[0].SetActive(false);
            }
        }

        // 유람선 등장 이벤트 (유람선 13, 나룻배 14)
        if (PlayerManager.instance.CheckEventCodeFromPlayedEventList("219"))
        {
            // 나룻배 비활성화
            if (npcListForEvent[14].activeSelf)
                npcListForEvent[14].SetActive(false);

            // 유람선 활성화
            if (!npcListForEvent[13].activeSelf)
                npcListForEvent[13].SetActive(true);
        }

        // 유람선 포탈 등장 이벤트 (15)
        if (PlayerManager.instance.CheckEventCodeFromPlayedEventList("220"))
        {
            if (!npcListForEvent[15].activeSelf)
                npcListForEvent[15].SetActive(true);
        }

        // 유람선이 없어지는 이벤트
        if (PlayerManager.instance.CheckEventCodeFromPlayedEventList("223"))
        {
            if (npcListForEvent[13].activeSelf)
                npcListForEvent[13].SetActive(false);
        }

        // 총장 사무실의 책장에 손잡이를 발생시키는 이벤트 254
        if (PlayerManager.instance.CheckEventCodeFromPlayedEventList("254"))
        {
            // 일반 책장(30) 비활성화
            if (npcListForEvent[30].activeSelf)
                npcListForEvent[30].SetActive(false);
            // 손잡이 달린 책장(16) 활성화
            if (!npcListForEvent[16].activeSelf)
                npcListForEvent[16].SetActive(true);
        }

        // 비밀 공간 포탈 등장 이벤트 (손잡이책장 16, 열린 책장 17, 비밀공간으로 가는 포탈 18)
        if (PlayerManager.instance.CheckEventCodeFromPlayedEventList("225"))
        {
            // 손잡이 달린 책장 비활성화
            if (npcListForEvent[16].activeSelf)
                npcListForEvent[16].SetActive(false);
            // 열린 책장 활성화
            if (!npcListForEvent[17].activeSelf)
                npcListForEvent[17].SetActive(true);
            // 비밀공간으로 가는 포탈 활성화
            if (!npcListForEvent[18].activeSelf)
                npcListForEvent[18].SetActive(true);
        }

        // 정보상 안으로 가는 포탈 등장 이벤트 227번 (19)
        if (PlayerManager.instance.CheckEventCodeFromPlayedEventList("227"))
        {
            if (!npcListForEvent[19].activeSelf)
                npcListForEvent[19].SetActive(true);
            if (npcListForEvent[26].activeSelf)
                npcListForEvent[26].SetActive(false);
        }

        // 사제와 누워있는 아이를 활성화하는 이벤트
        if (PlayerManager.instance.CheckEventCodeFromPlayedEventList("252"))
        {
            if (PlayerManager.instance.TimeSlot.Equals("74"))
            {
                //tempIndex = npcListForEvent.FindIndex(x => x.gameObject.name == "사제");
                if (!npcListForEvent[2].activeSelf)
                    npcListForEvent[2].SetActive(true);

                //tempIndex = npcListForEvent.FindIndex(x => x.gameObject.name == "새로 누워있는 아이");
                if (!npcListForEvent[3].activeSelf)
                    npcListForEvent[3].SetActive(true);
            }
            else
            {
                //tempIndex = npcListForEvent.FindIndex(x => x.gameObject.name == "사제");
                if (!npcListForEvent[2].activeSelf)
                    npcListForEvent[2].SetActive(false);

                //tempIndex = npcListForEvent.FindIndex(x => x.gameObject.name == "새로 누워있는 아이");
                if (!npcListForEvent[3].activeSelf)
                    npcListForEvent[3].SetActive(false);
            }
        }

        // 252번 이벤트가 발생 했고, 카메라가 보여지게하는 위치가 주택가 1사이드 1컷이라면, 2032대화묶음 실행
        if (PlayerManager.instance.CheckEventCodeFromPlayedEventList("252") && PlayerManager.instance.TimeSlot.Equals("74")
            && positionOfMainCamera.localPosition == position_Of_Sector1_Of_Street1_In_Village)
        {
            DialogManager.instance.InteractionWithObject("252번이벤트");
        }

        // 닫혀있는 금고(20)를, 열려있는 금고(21)로 바꾸고, 금고속 종이(22)를 나타나게 하기 -> 253번 이벤트
        if (PlayerManager.instance.CheckEventCodeFromPlayedEventList("253"))
        {
            // 닫혀있는 금고 비활성화
            if (npcListForEvent[20].activeSelf)
                npcListForEvent[20].SetActive(false);
            // 열려있는 금고 활성화
            if (!npcListForEvent[21].activeSelf)
                npcListForEvent[21].SetActive(true);
            // 금고속 종이 활성화
            if (!npcListForEvent[22].activeSelf)
                npcListForEvent[22].SetActive(true);
        }

        // 상호작용이 가능한 자작의 저택(23)을 비활성화하고, 상호작용이 불가능한 자작의 저택(24)과, 자작의 저택으로 가는 포탈(25) 활성화하기
        if (PlayerManager.instance.num_Try_to_Enter_in_Mansion >= 3)
        {
            if (!PlayerManager.instance.CheckEventCodeFromPlayedEventList("233"))
            {
                PlayerManager.instance.AddEventCodeToList("233");

                if (npcListForEvent[23].activeSelf)
                    npcListForEvent[23].SetActive(false);
                if (!npcListForEvent[24].activeSelf)
                    npcListForEvent[24].SetActive(true);
                if (!npcListForEvent[25].activeSelf)
                    npcListForEvent[25].SetActive(true);
            }
        }
        
        // 72 시간대에 진행되는 항구 이벤트가 발생하지 않았다면, 항구의 쉐렌(27), 악당 1(28), 악당 2(29) 활성화, 주택가의 쉐렌(30) 비활성화
        if (!PlayerManager.instance.CheckEventCodeFromPlayedEventList("0209") && PlayerManager.instance.TimeSlot.Equals("72"))
        {
            if (!npcListForEvent[27].activeSelf)
                npcListForEvent[27].SetActive(true);
            if (!npcListForEvent[28].activeSelf)
                npcListForEvent[28].SetActive(true);
            if (!npcListForEvent[29].activeSelf)
                npcListForEvent[29].SetActive(true);
        }

        // 72 시간대에 진행되는 항구 이벤트가 발생하면 항구의 쉐렌(27), 악당 1(28), 악당 2(29) 비활성화 & 주택가의 쉐렌(30) 활성화
        if (PlayerManager.instance.CheckEventCodeFromPlayedEventList("0209"))
        {
            if (npcListForEvent[27].activeSelf)
                npcListForEvent[27].SetActive(false);
            if (npcListForEvent[28].activeSelf)
                npcListForEvent[28].SetActive(false);
            if (npcListForEvent[29].activeSelf)
                npcListForEvent[29].SetActive(false);
        }
        // 특정 인물 등장 이벤트 처리 끝


        // 특정 변수가 조건에 만족할 경우, 특정 이벤트 추가하는 식으로 일단 하기(1월 23일 작업)

        // 이벤트 206
        /*
        if (PlayerManager.instance.num_Talk_With_1105 == 0)
        {
            if (!PlayerManager.instance.CheckEventCodeFromPlayedEventList("206") && PlayerManager.instance.TimeSlot.Equals("71") && PlayerManager.instance.NumOfAct.Equals("53"))
            {
                PlayerManager.instance.AddEventCodeToList("206");
                PlayerManager.instance.AddEventCodeToList("207");
            }
        }
        */

        // 이벤트 208 209에 필요한 이벤트 -> 72시간대에 플레이어가 항구를 처음 가게되면, 8014 대화 발생시키기
        if (PlayerManager.instance.TimeSlot.Equals("72") && PlayerManager.instance.GetCurrentPosition().Equals("Harbor_Street1") && PlayerManager.instance.GetPositionOfMerte() >= 642.0f)
        {
            if (!hasBeenInHarbor)
            {
                isPlaying8014Conversation = true;
                DialogManager.instance.InteractionWithObject("이벤트 자동발생");   // 8014 대화묶음 실행

                hasBeenInHarbor = true;
            }
        }

        /*
        // 이벤트 209
        if (PlayerManager.instance.num_Talk_With_1105 >= 1)
        {
            if (!PlayerManager.instance.CheckEventCodeFromPlayedEventList("209") 
                && (PlayerManager.instance.TimeSlot.Equals("71") || PlayerManager.instance.TimeSlot.Equals("72") || PlayerManager.instance.TimeSlot.Equals("73")))
            {
                PlayerManager.instance.AddEventCodeToList("209");
            }
        }
        */
        // 이벤트 208
        if (PlayerManager.instance.CheckEventCodeFromPlayedEventList("209"))
        {
            if (!PlayerManager.instance.CheckEventCodeFromPlayedEventList("208") && PlayerManager.instance.NumOfAct.Equals("53")
                && (PlayerManager.instance.TimeSlot.Equals("72") || PlayerManager.instance.TimeSlot.Equals("73") || PlayerManager.instance.TimeSlot.Equals("74")))
            {
                PlayerManager.instance.AddEventCodeToList("208");
                PlayerManager.instance.isCheckedSecretCode = true;
            }
        }


        // 이벤트 219 -> 유람선 탄생
        if (PlayerManager.instance.TimeSlot.Equals("73") && PlayerManager.instance.NumOfAct.Equals("53"))
        {
            if (!PlayerManager.instance.CheckEventCodeFromPlayedEventList("219"))
            {
                PlayerManager.instance.AddEventCodeToList("219");
            }
        }


        // 이벤트 221
        if (PlayerManager.instance.isInvestigated_StrangeDoor)
        {
            if (PlayerManager.instance.CheckEventCodeFromPlayedEventList("221"))
            {
                PlayerManager.instance.isInvestigated_StrangeDoor = false;  // 한번만 이동이 이루어지도록 처리
                //유람선 지하로 맵 이동 시키기 -> 나중에 Invoke 같은 함수 써서, 특정 대화가 끝나거나 시작하면 이동되게끔 해보기 (1월 27일 메모)
                PlayerManager.instance.SetCurrentPosition("Harbor_Prison");
                positionOfMerte.localPosition = positionOfPrisonInCruise.localPosition;
            }
        }


        // 이벤트 222
        if (!isActivatedEvent222)
        {
            List<Interaction>[] targetOfInteractionList = new List<Interaction>[3];
            List<Interaction> tempInteractionList = DialogManager.instance.GetInteractionList();
            targetOfInteractionList[0] = tempInteractionList.FindAll(x => (x.GetSetOfDesc() == 8033 && x.GetParent() == 1));
            targetOfInteractionList[1] = tempInteractionList.FindAll(x => (x.GetSetOfDesc() == 8034 && x.GetParent() == 3));
            targetOfInteractionList[2] = tempInteractionList.FindAll(x => (x.GetSetOfDesc() == 8035 && x.GetParent() == 3));

            if (targetOfInteractionList[0][0].GetStatus() == 1 && targetOfInteractionList[1][0].GetStatus() == 1 && targetOfInteractionList[2][0].GetStatus() == 1)
            {
                if (!PlayerManager.instance.CheckEventCodeFromPlayedEventList("222"))
                {
                    // 철장 1 2 3을 모두 조사했으면, 이벤트 대사와 함께, 플레이어를 유람선 밖으로 내보내도록 조작해야함
                    PlayerManager.instance.AddEventCodeToList("222");
                    isActivatedEvent222 = true;
                    DialogManager.instance.InteractionWithObject("대화3개 다하면 자동");
                }
            }
        }
        if (triggerKickMerte)
        {
            // 메르테를 유람선 밖으로 내보내기
            PlayerManager.instance.SetCurrentPosition("Harbor_Street1");
            positionOfMerte.localPosition = positionOfCruiseOutside.localPosition;
            triggerKickMerte = false;
        }

        // 안드렌이 2주마다 단서를 정리하여 사무실에 두고간다. -> 단서 정리 시스템에서 안드렌의 서류를 2주에 한번씩 나타나게 해야함
        if (PlayerManager.instance.CheckEventCodeFromPlayedEventList("224"))
        {

        }

        // 이벤트 228
        // 자작의 대문이 비활성화 되어있어야 함. npcEventList 20번
        if (PlayerManager.instance.num_Try_to_Enter_in_Mansion >= 1)
        {
            if (!PlayerManager.instance.CheckEventCodeFromPlayedEventList("228"))
            {
                PlayerManager.instance.AddEventCodeToList("228");
            }
        }

        // 이벤트 229
        // 레이나의 애인, 덩치 큰 수사관 단서가 수집되어있을 때 이벤트 발생
        if (!PlayerManager.instance.CheckEventCodeFromPlayedEventList("229"))
        {
            int clueNum = 0;
            for (int i = 0; i < PlayerManager.instance.playerClueLists.Count; i++)
            {
                if (PlayerManager.instance.playerClueLists[i].GetClueName().Equals("레이나의 애인"))
                    clueNum++;
                if (PlayerManager.instance.playerClueLists[i].GetClueName().Equals("덩치 큰 수사관"))
                    clueNum++;
            }

            if (clueNum == 2)
                PlayerManager.instance.AddEventCodeToList("229");
        }

        // 이벤트 230
        // 멜리사 엔딩 조건 70% 이상 달성 시 이벤트 발생
        // 레이나 집에서 오브젝트 조사 3회 이상 , 카페에서 멜리사와 대화 2회 이상 , 5027 or 5030 대화 1회 진행
        if (PlayerManager.instance.num_investigation_Raina_house_object >= 3 && PlayerManager.instance.num_Talk_With_1003 >= 2
            && PlayerManager.instance.num_Play_5027_or_5030 >= 1)
        {
            if (!PlayerManager.instance.CheckEventCodeFromPlayedEventList("230"))
            {
                PlayerManager.instance.AddEventCodeToList("230");
            }
        }

        // 이벤트 231
        if (PlayerManager.instance.num_Talk_With_1601_in_73 >= 1)
        {
            if (!PlayerManager.instance.CheckEventCodeFromPlayedEventList("231"))
            {
                PlayerManager.instance.AddEventCodeToList("231");
            }
        }

        // 이벤트 234
        if (PlayerManager.instance.num_Talk_With_1803_1804_in_71 == 0)
        {
            if (!PlayerManager.instance.CheckEventCodeFromPlayedEventList("234"))
            {
                PlayerManager.instance.AddEventCodeToList("234");
            }
        }

        // 이벤트 235
        if (PlayerManager.instance.num_Talk_With_1003_in_73 == 1)
        {
            if (!PlayerManager.instance.CheckEventCodeFromPlayedEventList("235"))
            {
                PlayerManager.instance.AddEventCodeToList("235");
            }
        }

        // 이벤트 236 (멜리사에게 5회 이상 대화 시도)
        if (PlayerManager.instance.num_Interrogate_about_case >= 5)
        {
            if (!PlayerManager.instance.CheckEventCodeFromPlayedEventList("236"))
            {
                PlayerManager.instance.AddEventCodeToList("236");
            }
        }

        // 이벤트 237
        if (PlayerManager.instance.GetCurrentPosition().Equals("Slum_Information_agency"))
        {
            if (!PlayerManager.instance.CheckEventCodeFromPlayedEventList("237"))
            {
                PlayerManager.instance.AddEventCodeToList("237");
            }
        }

        // 이벤트 238
        if (PlayerManager.instance.num_Enter_or_Investigate_BroSisHouse >= 2)
        {
            if (!PlayerManager.instance.CheckEventCodeFromPlayedEventList("238"))
            {
                PlayerManager.instance.AddEventCodeToList("238");
            }
        }
        /*
        // 이벤트 239
        if (!PlayerManager.instance.isCheckedSecretCode)
        {
            if (!PlayerManager.instance.CheckEventCodeFromPlayedEventList("239"))
            {
                PlayerManager.instance.AddEventCodeToList("239");
            }
        }

        // 이벤트 240
        if (PlayerManager.instance.isCheckedSecretCode)
        {
            if (!PlayerManager.instance.CheckEventCodeFromPlayedEventList("240"))
            {
                if (PlayerManager.instance.CheckEventCodeFromPlayedEventList("239"))
                    PlayerManager.instance.DeleteEventCodeFromList("239");

                PlayerManager.instance.AddEventCodeToList("240");
            }
        }*/

        if (!PlayerManager.instance.CheckEventCodeFromPlayedEventList("240"))
        {
            //Debug.Log("240번 이벤트가 없어");
            if (!PlayerManager.instance.CheckEventCodeFromPlayedEventList("239"))
            {
                PlayerManager.instance.AddEventCodeToList("239");
            }
        }

        if (PlayerManager.instance.CheckEventCodeFromPlayedEventList("240"))
        {
            //Debug.Log("240번 이벤트가 이미 있어");
            if (PlayerManager.instance.CheckEventCodeFromPlayedEventList("239"))
            {
                PlayerManager.instance.DeleteEventCodeFromList("239");
            }
        }

        // 이벤트 241
        //71시간대에서 사이드 2를 조사하려 했을 때
        // -> 즉, 71시간대에서 사이드 2에 포함된 오브젝트와 상호작용하려 했을때, 이벤트 발생
        // ProhibitionEntry.cs 참고
        if (PlayerManager.instance.TimeSlot.Equals("71"))
        {
            PlayerManager.instance.AddEventCodeToList("241");
        }

        // 이벤트 242
        if (PlayerManager.instance.isInvestigated_Raina_house)
        {
            if (!PlayerManager.instance.CheckEventCodeFromPlayedEventList("242"))
            {
                PlayerManager.instance.AddEventCodeToList("242");
            }
        }

        // 이벤트 244
        if (PlayerManager.instance.num_Talk_With_1603_in_72 == 1)
        {
            if (!PlayerManager.instance.CheckEventCodeFromPlayedEventList("244"))
            {
                PlayerManager.instance.AddEventCodeToList("244");
            }
        }

        // 이벤트 245
        if (PlayerManager.instance.num_Enter_in_Mansion >= 1)
        {
            if (!PlayerManager.instance.CheckEventCodeFromPlayedEventList("245"))
            {
                PlayerManager.instance.AddEventCodeToList("245");
            }
        }

        // 이벤트 246
        if (PlayerManager.instance.num_Talk_With_1202 >= 2)
        {
            if (!PlayerManager.instance.CheckEventCodeFromPlayedEventList("246"))
            {
                PlayerManager.instance.AddEventCodeToList("246");
            }
        }

        // 이벤트 247
        if (PlayerManager.instance.num_Talk_With_1205_in_71 == 0)
        {
            if (!PlayerManager.instance.CheckEventCodeFromPlayedEventList("247"))
            {
                PlayerManager.instance.AddEventCodeToList("247");
            }
        }

        // 이벤트 248
        if (PlayerManager.instance.TimeSlot.Equals("73") && PlayerManager.instance.GetCurrentPosition().Equals("Harbor_Street1"))
        {
            if (!PlayerManager.instance.CheckEventCodeFromPlayedEventList("248"))
            {
                PlayerManager.instance.AddEventCodeToList("248");
            }
        }

        // 이벤트 249
        int clueNum2 = 0;

        if (!PlayerManager.instance.isPossessed_3A01_3A08_Clues)
        {
            for (int i = 0; i < PlayerManager.instance.playerClueLists.Count; i++)
            {
                if (PlayerManager.instance.playerClueLists[i].GetClueName().Equals("풍선과 배"))
                    clueNum2++;
                if (PlayerManager.instance.playerClueLists[i].GetClueName().Equals("수상한 공간"))
                    clueNum2++;
                if (PlayerManager.instance.playerClueLists[i].GetClueName().Equals("꽃 거래 내역서"))
                    clueNum2++;
                if (PlayerManager.instance.playerClueLists[i].GetClueName().Equals("달력의 날짜"))
                    clueNum2++;
                if (PlayerManager.instance.playerClueLists[i].GetClueName().Equals("큰 돈과 큰 배"))
                    clueNum2++;
                if (PlayerManager.instance.playerClueLists[i].GetClueName().Equals("유람선 티켓"))
                    clueNum2++;
                if (PlayerManager.instance.playerClueLists[i].GetClueName().Equals("입양 서류"))
                    clueNum2++;
                if (PlayerManager.instance.playerClueLists[i].GetClueName().Equals("입양과 후원"))
                    clueNum2++;
            }
        }

        if (clueNum2 == 8)
            PlayerManager.instance.isPossessed_3A01_3A08_Clues = true;

        if (!PlayerManager.instance.isPossessed_3A01_3A08_Clues)
        {
            if (!PlayerManager.instance.CheckEventCodeFromPlayedEventList("249"))
            {
                PlayerManager.instance.AddEventCodeToList("249");
            }
        }

        // 이벤트 250
        if (PlayerManager.instance.isPossessed_3A01_3A08_Clues)
        {
            if (!PlayerManager.instance.CheckEventCodeFromPlayedEventList("250"))
            {
                if (PlayerManager.instance.CheckEventCodeFromPlayedEventList("249"))
                    PlayerManager.instance.DeleteEventCodeFromList("249");

                PlayerManager.instance.AddEventCodeToList("250");
            }
        }

        // 이벤트 251
        if (!PlayerManager.instance.isEnter_In_Cruise && PlayerManager.instance.GetCurrentPosition().Equals("Harbor_Cruise"))
        {
            if (!PlayerManager.instance.CheckEventCodeFromPlayedEventList("251"))
            {
                //유람선에 들어간 적이 없는 경우, "아무도 없는~" 대화가 실행되도록 해당 이벤트 조작해야함
                PlayerManager.instance.AddEventCodeToList("251");
                Invoke("PlayScriptForEvent251", 1.0f);
            }
        }

        // 251번 이벤트를 위한 처리
        if (PlayerManager.instance.GetCurrentPosition().Equals("Harbor_Cruise"))
            PlayerManager.instance.isEnter_In_Cruise = true;
    }

    public void PlayScriptForEvent251()
    {
        DialogManager.instance.InteractionWithObject("유람선첫진입");
    }

}
