using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public static PlayerManager instance = null;

    /* player가 있다는 가정 */
    //public List<Clue> ClueList;        // player가 얻은 단서들의 리스트
    public List<Clue>[] ClueLists;        // player가 얻은 단서들의 리스트

    public List<ClueStructure> playerClueLists; // player가 얻은 단서들의 리스트
    public List<ClueStructure> playerClueLists_In_Certain_Timeslot; // 단서 정리에 사용될, 현재 시간대에 얻은 단서들의 목록

    // 나중에 private set 으로 바꿀 수 있는지 체크해보기(1210)
    public string NumOfAct { get; set; }    // player가 현재 진행하고 있는 Act
    public string TimeSlot { get; set; }    // player가 현재 진행하고 있는 시간대

    public string checkNumOfAct;
    public string checkTimeSlot;

    /* 맵 이동 관련 변수 */
    [SerializeField] private GameObject player; // 플레이어의 위치값을 받을 변수
    private string currentPosition;             //플레이어의 맵에서의 현재 위치
    private bool isInPortalZone;                //플레이어가 포탈존에 있는지 유무 확인

    /* 오브젝트와의 상호작용을 위한 변수 */
    [SerializeField] private bool isNearObject;      //상호작용할 수 있는 오브젝트와 가까이 있는가?
    private Vector2 pos;            //마우스로 클릭한 곳의 위치
    private Ray2D ray;              //마우스로 클릭한 곳에 보이지않는 광선을 쏨
    private RaycastHit2D hit;       //쏜 광선에 닿은것이 뭔지 확인하기위한 변수

    /* character code test */
    private string er;
    private string garbageBag;
    private NpcParser npcParser;

    /* 플레이어가 수행한 이벤트 리스트 */
    private List<string> playedEventList;

    /* EventManager의 PlayEvent 함수에서 이벤트를 다루기 위한 행동 변수들을 정의 */
    // 이벤트의 조건이 "랜덤" 인 것이 정확히 무슨의미인지 확인해야할 필요가 있음.
    //public int num_Talk_With_1100_1101 = 0;                 // 1100, 1101과 대화한 횟수 (관련 이벤트 203, 204)
    public int num_Talk_With_1105 = 0;                      // 1105와 대화한 횟수 (관련 이벤트 209)
    //public int num_Talk_With_1107_1108 = 0;                 // 1107, 1108과 대화한 횟수 (관련 이벤트 213, 214)
    public bool isInvestigated_StrangeDoor = false;         // 이상한 문 오브젝트가 조사됐는지의 여부 (관련 이벤트 221)
    //public bool isInvestigated_FirstJail = false;           // 첫번째 철장이 조사됐는지의 여부 (관련 이벤트 222)
    //public bool isInvestigated_SecondJail = false;          // 두번째 철장이 조사됐는지의 여부 (관련 이벤트 222)
    //public bool isInvestigated_ThridJail = false;           // 세번째 철장이 조사됐는지의 여부 (관련 이벤트 222)
    public int num_Try_to_Enter_in_Mansion = 0;             // 자작의 저택 방문 시도 횟수 (관련 이벤트 228,233)
    public int num_Talk_With_1003 = 0;                      // 1003(멜리사)와 대화를 진행한 횟수 (관련 이벤트 230)
    public int num_investigation_Raina_house_object = 0;    // 레이나의 집의 오브젝트와 상호작용한 횟수 (관련 이벤트 230)
    public int num_Play_5027_or_5030 = 0;                   // 5027 or 5030 대화가 진행된 횟수 (관련 이벤트 230)
    public int num_Talk_With_1601_in_73 = 0;                // 73 시간대에 1601과 대화한 횟수 (관련 이벤트 231)
    public int num_Talk_With_1803_1804_in_71 = 0;           // 1803,1804와 대화한 횟수 (관련 이벤트 234)
    public int num_Talk_With_1003_in_73 = 0;                // 73 시간대에 1003과 대화한 횟수 (관련 이벤트 235, 230)
    public int num_Interrogate_about_case = 0;              // 사건에 관해서 심문한 횟수 (관련 이벤트 236)
    //public bool isEnter_InformationAgency = false;          // 정보상에 들어간 적 있는지의 여부 (관련 이벤트 237)
    public int num_Enter_or_Investigate_BroSisHouse = 0;    // 남매의 집에 방문 or 조사한 횟수 (관련 이벤트 238)
    public bool isCheckedSecretCode = false;                // 암호확인 여부 (관련 이벤트 239)
    public bool isTakenSecretCodeEvent = false;             // 암호이벤트 이수 여부 (관련 이벤트 240)
    public bool isInvestigated_Raina_house = false;         // 레이나 집 수사(오브젝트 조사) 여부 (관련 이벤트 242)
    public int num_Talk_With_1603_in_72 = 0;                // 72 시간대에 1603과 대화한 횟수 (관련 이벤트 244)
    public int num_Enter_in_Mansion = 0;                    // 자작의 저택에 들어간 횟수(방문 횟수, 관련 이벤트 245)
    public int num_Talk_With_1202 = 0;                      // 1113과 대화한 횟수 (관련 이벤트 246)
    public int num_Talk_With_1205_in_71 = 0;                // 71 시간대에 1205와 대화한 횟수 (관련 이벤트 247)
    //public bool isEnter_In_512 = false;                     // 73 시간대에 512맵(항구 1사이드 2컷)에 처음 들어갈 때 발생 (관련 이벤트 248)
    public bool isPossessed_3A01_3A08_Clues = false;        // 3A08까지 단서 획득 여부 (관련 이벤트 249, 250)
    public bool isEnter_In_Cruise = false;                  // 유람선에 들어간 적이 있는지 여부 (관련 이벤트 251)


    // Use this for initialization
    void Awake() {
        if (instance == null)
            instance = this;

        /* for the character code test */
        er = "1000";
        garbageBag = "1001";
        npcParser = new NpcParser();

        //ClueList = new List<Clue>();
        ClueLists = new List<Clue>[5];  //Act5까지의 단서들 리스트

        playerClueLists = new List<ClueStructure>();
        playerClueLists_In_Certain_Timeslot = new List<ClueStructure>();

        //ClueLists 초기화
        for (int i = 0; i < ClueLists.Length; i++)
            ClueLists[i] = new List<Clue>();

        NumOfAct = "53";   //사건3 시작
        TimeSlot = "71";   //첫째주 시작

        checkNumOfAct = NumOfAct;
        checkTimeSlot = TimeSlot;

        //currentPosition = "Downtown_Street1";
        currentPosition = "Chapter_Merte_Office"; // 메르테 위치 : 11419, 5169
        //currentPosition = "Slum_Information_agency"; // -3212 -224

        isInPortalZone = false;

        playedEventList = new List<string>();

        // 추후에, 상호작용 될 수 있는 오브젝트의 근처에 있을 때만 상호작용 되도록 할 것(1월 27일 메모)
        SetIsNearObject(true);
    }

    // Update is called once per frame
    void Update() {

        if (UIManager.instance.isReadParchment && Input.GetKeyDown(KeyCode.E))
        {
            UIManager.instance.isFading = true;
            //Debug.Log("단서 정리 시스템 종료");
            UIManager.instance.ArrangeClue();
            //단서 정리 시스템을 종료 한 후, 화면이 Fade in 되고 "~시간대가 지났다" 라는 텍스트 출력 후, 같이 Fade out되고 시간대 변경
            StartCoroutine(UIManager.instance.FadeEffectForChangeTimeSlot());
            UIManager.instance.isReadParchment = false;
        }

        /* 오브젝트와의 상호작용을 위한 if */
        if (!UIManager.instance.isConversationing)
        {
            if (((Input.GetMouseButtonDown(0) && !UIManager.instance.GetIsOpenedParchment() && !UIManager.instance.isFading && !UIManager.instance.GetIsOpenNote() && !UIManager.instance.isPortaling && !UIManager.instance.isFading)
                    || (Input.GetKeyDown(KeyCode.E) && !UIManager.instance.GetIsOpenedParchment() && !UIManager.instance.isFading && !UIManager.instance.GetIsOpenNote() && !UIManager.instance.isPortaling && !UIManager.instance.isFading))
                && isNearObject)
            {
                pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                ray = new Ray2D(pos, Vector2.zero);
                hit = Physics2D.Raycast(ray.origin, ray.direction);

                if (hit.collider == null)
                {
                    //Debug.Log("아무것도 안맞죠?");
                }
                else if (hit.collider.tag == "InteractionObject")
                {
                    if (hit.collider.name.Equals("책상_메르테 사무실"))
                    {
                        if (!UIManager.instance.isReadParchment)
                        {
                            //Debug.Log("단서 정리 시스템 활성화");

                            if (ParchmentControll.instance.GetParchmentPosition().y != -720)
                                ParchmentControll.instance.SetParchmentPosition(new Vector2(0, -720));

                            if (ParchmentControll.instance.GetAggregationClueListScrollListPosition().y != -720)
                                ParchmentControll.instance.SetAggregationClueListScrollListPosition(new Vector2(0, -720));

                            if (ParchmentControll.instance.GetHelperContentPosition().y != 0)
                                ParchmentControll.instance.SetHelperContentPosition(new Vector2(0, 0));

                            UIManager.instance.ArrangeClue();
                        }
                        else
                        {
                            //Debug.Log("단서 정리 시스템 활성화 실패");
                        }
                    }
                    else
                    {
                        //Debug.Log("hit.collider.name : " + npcParser.GetNpcCodeFromName(hit.collider.name));
                        try
                        {
                            if (!UIManager.instance.isConversationing && !UIManager.instance.isFading)
                            {
                                DialogManager.instance.InteractionWithObject(npcParser.GetNpcCodeFromName(hit.collider.name));
                            }
                            //if(hit.collider.name.Equals("ER"))
                            //    DialogManager.instance.InteractionWithObject(er);

                            //if (hit.collider.name.Equals("GarbageBag"))
                            //    DialogManager.instance.InteractionWithObject(garbageBag);
                        }
                        catch
                        {

                        }
                    }
                }
            }
        }

        /* for test 1226 */
        /*
        if (Input.GetKey(KeyCode.Alpha1))
        {
            TimeSlot = "71";
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            TimeSlot = "83";
        }
        */

    }

    // 플레이어(메르테)의 x포지션 값 반환
    public float GetPositionOfMerte()
    {
        return player.GetComponent<Transform>().localPosition.x;
    }

    // 플레이어가 수행한 이벤트를 추가
    public void AddEventCodeToList(string eventCode)
    {
        playedEventList.Add(eventCode);
    }

    // 플레이어가 수행한 이벤트중 하나를 삭제
    public void DeleteEventCodeFromList(string eventCode)
    {
        playedEventList.Remove(eventCode);
    }

    // 특정 이벤트가 플레이어에 의해 진행된 적이 있는지를 판단하는 함수
    public bool CheckEventCodeFromPlayedEventList(string eventCode)
    {
        if (playedEventList.Contains(eventCode))
            return true;
        else if (playedEventList.Count == 0)
            return false;
        else
            return false;
    }

    /* player의 단서파일을 불러올때, 초기화시키기 위함 */
    public void ResetClueList()
    {
        for(int i=0; i<ClueLists.Length; i++)
            ClueLists[i].Clear();

        playerClueLists.Clear();
    }

    // 단서 정리를 마친 후에 쓰여질 함수
    public void ResetClueList_In_Certain_Timeslot()
    {
        playerClueLists_In_Certain_Timeslot.Clear();
    }

    // 현재 시간대에서 얻은 단서들의 갯수를 리턴하는 함수
    public int GetCount_ClueList_In_Certain_Timeslot()
    {
        return playerClueLists_In_Certain_Timeslot.Count;
    }

    /* player가 얻은 데이터를 단서리스트에 추가 */
    //public void AddClueToList(Clue clueData)
    //{
    //    ClueList.Add(clueData);
    //    Debug.Log("여태 획득한 단서 수 : " + ClueList.Count);
    //}

    /* player가 얻은 해당 Act의 단서를 단서리스트에 추가 */
    public void AddClueToList(ClueStructure clueData)
    {
        playerClueLists.Add(clueData);
        playerClueLists_In_Certain_Timeslot.Add(clueData);  // 단서 정리를 위한 단서 저장
    }

    /* 단서 중복 방지 */
    public bool CheckClue(string clueName)
    {
        for (int i = 0; i < playerClueLists.Count; i++)
        {
            if (playerClueLists[i].GetClueName().Equals(clueName))
                return true;
        }

        return false;
    }

    /* player가 여태 얻은 단서들의 리스트 보기(임시) */
    public void PrintClueList()
    {
        for (int i = 0; i < ClueLists.Length; i++)
        {
            for (int j = 0; j < ClueLists[i].Count; j++)
            {
                Debug.Log((j + 1) + " : name(" + (ClueLists[i])[j].GetName() + "), desc(" + (ClueLists[i])[j].GetDesc() + "), arranged(" + (ClueLists[i])[j].GetArrangedContent() + ")");
            }
        }
    }

    /* player의 데이터를 저장 */
    public void SavePlayer()
    {
        ItemDatabase.instance.SavePlayerData(ClueLists);
    }

    public string GetCurrentPosition()
    {
        return currentPosition;
    }

    public void SetCurrentPosition(string currentPosition)
    {
        this.currentPosition = currentPosition;
    }

    public bool GetIsInPortalZone()
    {
        return isInPortalZone;
    }

    public void SetIsInPortalZone(bool isInPortalZone)
    {
        this.isInPortalZone = isInPortalZone;
    }

    public bool GetIsNearObject()
    {
        return isNearObject;
    }

    public void SetIsNearObject(bool isNearObject)
    {
        this.isNearObject = isNearObject;
    }

    public Vector3 GetPlayerPosition() {
        return player.transform.localPosition;
    }

    public void SetPlayerPosition(Vector3 tempPosition) {
        player.transform.localPosition = tempPosition;
    }
}
