using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private EventVariable eventVariable;

    public bool skipText;   // 대화 출력을 스킵할 때 사용
    

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

        //NumOfAct = "54";   //사건4 시작
        //TimeSlot = "75";   //첫날 시작

        checkNumOfAct = NumOfAct;
        checkTimeSlot = TimeSlot;

        //currentPosition = "Downtown_Street1";
        currentPosition = "Chapter_Merte_Office"; // 메르테 위치 : 11419, 5169
        //currentPosition = "Chapter_Zaral_Office";
        //currentPosition = "Slum_Information_agency"; // -3212 -224

        isInPortalZone = false;

        playedEventList = new List<string>();
        eventVariable = new EventVariable();

        // 추후에, 상호작용 될 수 있는 오브젝트의 근처에 있을 때만 상호작용 되도록 할 것(1월 27일 메모)
        SetIsNearObject(true);

        skipText = false;
    }

    void Start()
    {
        if (GameManager.instance.GetGameState().Equals(GameManager.GameState.NewGame_Loaded))
        {
            NumOfAct = "53";   //사건3 시작
            TimeSlot = "71";   //첫째주 시작

            if (GameManager.instance.GetPlayState() == GameManager.PlayState.Tutorial)
                UIManager.instance.act3Button.SetActive(false);
            UIManager.instance.act4Button.SetActive(false);
            UIManager.instance.act5Button.SetActive(false);

            checkNumOfAct = NumOfAct;
            checkTimeSlot = TimeSlot;

            //DialogManager.instance.SetLists();
            ItemDatabase.instance.SetLists();
        }
        else if (GameManager.instance.GetGameState().Equals(GameManager.GameState.PastGame_Loaded))
        {
            
            NumOfAct = "";
            TimeSlot = "";

            //DialogManager.instance.SetLists();
            ItemDatabase.instance.SetLists();
            GameManager.instance.LoadPlayerData();
            eventVariable = GameManager.instance.GetEventVariable();
            //GameManager.instance.SetEventVariable(ref GetEventVariableClass());

            if (NumOfAct.Equals("53"))
            {
                UIManager.instance.act4Button.SetActive(false);
                UIManager.instance.act5Button.SetActive(false);
            }
            else if (NumOfAct.Equals("54"))
            {
                UIManager.instance.SetNameOfCase("사건4 연쇄살인 4번째 피해자_륑 에고이스모");
                UIManager.instance.act4Button.SetActive(true);
                UIManager.instance.act5Button.SetActive(false);
            }
            else if (NumOfAct.Equals("55"))
            {
                UIManager.instance.SetNameOfCase("사건4 연쇄살인 4번째 피해자_륑 에고이스모");
                UIManager.instance.act4Button.SetActive(true);
                UIManager.instance.act5Button.SetActive(true);
            }

            checkNumOfAct = NumOfAct;
            checkTimeSlot = TimeSlot;

            ResetClueList_In_Certain_Timeslot(); // for PlaySaveGame 

            EventManager.instance.PlayEvent();
        }
    }

    // Update is called once per frame
    void Update() {

        if (!UIManager.instance.GetIsPaused())//일시정지 상태가 아닐때
        {
            if (UIManager.instance.isReadParchment && Input.GetKeyDown(KeyCode.E) && !ParchmentControll.instance.GetIsPlayingDocumentAnim())
            { 
                if (GameManager.instance.GetPlayState() == GameManager.PlayState.Act)
                {
                    UIManager.instance.isFading = true;
                    DocumentControll.instance.ResetDocumentOfAndren();

                    if ((NumOfAct.Equals("53") && TimeSlot.Equals("74")) || (NumOfAct.Equals("54") && TimeSlot.Equals("79")))
                    {
                        DocumentControll.instance.GetAndrenClue(NumOfAct);  // 안드렌이 모은 단서 획득
                        UIManager.instance.SetIsReadParchment_In_74_79(false);
                    }

                    //Debug.Log("단서 정리 시스템 종료");
                    UIManager.instance.ArrangeClue();

                    if (NumOfAct.Equals("54") && TimeSlot.Equals("79"))
                    {
                        // 사건4 마지막날에 단서정리 양피지를 닫은 경우, 엔딩 선택 분기 UI가 나타나야한다.


                        UIManager.instance.isReadParchment = false;
                        return;
                    }

                    //단서 정리 시스템을 종료 한 후, 화면이 Fade in 되고 "~시간대가 지났다" 라는 텍스트 출력 후, 같이 Fade out되고 시간대 변경
                    StartCoroutine(UIManager.instance.FadeEffectForChangeTimeSlot());
                }
                else if (GameManager.instance.GetPlayState() == GameManager.PlayState.Tutorial)
                {
                    TutorialManager.instance.isParchmentTutorial = false;

                    UIManager.instance.ArrangeClue();
                }

                UIManager.instance.isReadParchment = false;
            }

            //if (Input.GetMouseButtonDown(0))
            //{
            //    Debug.Log(UIManager.instance.GetIsOpenedParchment() + ", " + UIManager.instance.isFading + ", " + UIManager.instance.GetIsOpenNote() + ", " + UIManager.instance.isPortaling + ", " + MiniMapManager.instance.IsMiniMapOpen() + ", " + isNearObject);
            //}

            if (Input.GetKeyDown(KeyCode.Escape) && !UIManager.instance.GetIsReadParchment_In_74_79())
            {
                // 양피지 비활성화
                UIManager.instance.SetActiveFalseToParchment();
                DocumentControll.instance.ResetDocumentOfAndren();
            }

            /* 오브젝트와의 상호작용을 위한 if */
            if ( Input.GetMouseButtonDown(0) && CheckCondition_InteractionObject() )
            {
                pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                ray = new Ray2D(pos, Vector2.zero);
                hit = Physics2D.Raycast(ray.origin, ray.direction);

                if (hit.collider == null)
                {
                    //Debug.Log("아무것도 안맞죠?");
                }
                else if ((hit.collider.tag == "MerteDesk" || hit.collider.tag == "InteractionObject") && GameManager.instance.GetPlayState() != GameManager.PlayState.Ending)
                {
                    if (hit.collider.name.Equals("책상_메르테 사무실"))
                    {
                        if (!UIManager.instance.isReadParchment && !TutorialManager.instance.isPlayingEndTutorial)
                        {
                            // 메르테 책상 강조 종료 및 다음 대화 출력 트리거
                            if (GameManager.instance.GetPlayState() == GameManager.PlayState.Tutorial && !TutorialManager.instance.isCompletedTutorial[21])
                            {
                                TutorialManager.instance.isCompletedTutorial[21] = true;
                                TutorialManager.instance.isParchmentTutorial = false;
                            }

                            //Debug.Log("단서 정리 시스템 활성화");
                            UIManager.instance.SetDocumentControll(true);
                            // 양피지 갱신
                            ParchmentControll.instance.UpdateParchmentPosition(playerClueLists_In_Certain_Timeslot.Count);

                            if (ParchmentControll.instance.GetParchmentPosition().y != ParchmentControll.instance.min_Scroll_Position_Parchment)
                                ParchmentControll.instance.SetParchmentPosition(new Vector2(0, ParchmentControll.instance.min_Scroll_Position_Parchment));

                            //if (ParchmentControll.instance.GetAggregationClueListScrollListPosition().y != -720)
                            //    ParchmentControll.instance.SetAggregationClueListScrollListPosition(new Vector2(0, -720));

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
                        if (GameManager.instance.GetPlayState() == GameManager.PlayState.Act)
                        {
                            if (DialogManager.instance.CheckInteraction(hit.collider.name))
                            {
                                //Debug.Log("hit.collider.name : " + npcParser.GetNpcCodeFromName(hit.collider.name));
                                //try
                                //{
                                //    Debug.Log("사건" + NumOfAct + "의 " + TimeSlot + "시간대에서 " + hit.collider.name + "과 대화시도");
                                //    DialogManager.instance.InteractionWithObject(npcParser.GetNpcCodeFromName(hit.collider.name));
                                //}
                                //catch
                                //{
                                //    Debug.Log("사건" + NumOfAct + "의 " + TimeSlot + "시간대에서 " + hit.collider.name + "과 대화 중 오류 발생");
                                //}

                                Debug.Log("사건" + NumOfAct + "의 " + TimeSlot + "시간대에서 " + hit.collider.name + "( + " + npcParser.GetNpcCodeFromName(hit.collider.name) + ")과 대화시도");
                                DialogManager.instance.InteractionWithObject(npcParser.GetNpcCodeFromName(hit.collider.name));
                            }
                        }
                        else if (GameManager.instance.GetPlayState() == GameManager.PlayState.Tutorial)
                        {
                            if (hit.collider.name.Equals("시체_튜토리얼") && !TutorialManager.instance.isCompletedTutorial[4])
                            {
                                TutorialManager.instance.isCompletedTutorial[3] = true;
                                TutorialManager.instance.isCompletedTutorial[4] = true;
                                //DialogManager.instance.InteractionWithObject("905");

                                TutorialManager.instance.InvokeTutorial();
                                //TutorialManager.instance.IncreaseTutorial_Index();

                                TutorialManager.instance.TagChange(0, "Untagged");
                            }

                            if (hit.collider.name.Equals("식탁_레이나 집") && !TutorialManager.instance.isCompletedTutorial[6])
                            {
                                TutorialManager.instance.isCompletedTutorial[5] = true;
                                TutorialManager.instance.isCompletedTutorial[6] = true;
                                //DialogManager.instance.InteractionWithObject("907");

                                TutorialManager.instance.InvokeTutorial();
                                //TutorialManager.instance.IncreaseTutorial_Index();

                                TutorialManager.instance.TagChange(1, "Untagged");
                            }

                            if (hit.collider.name.Equals("싱크대_레이나 집") && !TutorialManager.instance.isCompletedTutorial[8])
                            {
                                TutorialManager.instance.isCompletedTutorial[7] = true;
                                TutorialManager.instance.isCompletedTutorial[8] = true;
                                //DialogManager.instance.InteractionWithObject("909");

                                TutorialManager.instance.InvokeTutorial();
                                //TutorialManager.instance.IncreaseTutorial_Index();

                                TutorialManager.instance.TagChange(2, "Untagged");
                            }

                            if (hit.collider.name.Equals("바륀") && !TutorialManager.instance.isCompletedTutorial[11])
                            {
                                TutorialManager.instance.isCompletedTutorial[9] = true;
                                TutorialManager.instance.isCompletedTutorial[11] = true;
                                //TutorialManager.instance.IncreaseTutorial_Index();
                                //TutorialManager.instance.isCompletedTutorial[11] = true;
                                //DialogManager.instance.InteractionWithObject("912");

                                TutorialManager.instance.InvokeTutorial();
                                //TutorialManager.instance.IncreaseTutorial_Index();

                                TutorialManager.instance.TagChange(3, "Untagged");
                            }
                        }// if-else PlayState
                    }// if-else hit 책상
                }// if-else hit null
                else if (GameManager.instance.GetPlayState() == GameManager.PlayState.Ending)
                {
                    if (DialogManager.instance.CheckInteraction(hit.collider.name))
                    {
                        Debug.Log("사건" + NumOfAct + "의 " + TimeSlot + "시간대에서 " + hit.collider.name + "( + " + npcParser.GetNpcCodeFromName(hit.collider.name) + ")과 대화시도");

                        if (hit.collider.name.Equals("금고_주인공사무실"))
                        {
                            // 일기장을 얻었다는 대화가 끝난 후, 일기장 활성화 시키기
                            DialogManager.instance.InteractionWithObject("getDiary");
                        }
                        else
                        {
                            // 금고가 아닌 다른 오브젝트와 상호작용 시
                            DialogManager.instance.InteractionWithObject("findUsingKey");
                        }
                    }
                    
                }
            }// if-else GetMouseButtonDown
        }// if GetIsPaused
    }
    
    // for tutorial
    public void SetPlayer(GameObject character)
    {
        player = character;
    }

    // 플레이어(메르테)의 x포지션 값 반환
    public float GetPositionOfMerte()
    {
        return player.GetComponent<Transform>().localPosition.x;
    }

    public List<string> GetPlayedEventList()
    {
        return playedEventList;
    }

    // 플레이어가 수행한 이벤트를 추가
    public void AddEventCodeToList(string eventCode)
    {
        // 발생하지 않은 이벤트만 추가
        if (!CheckEventCodeFromPlayedEventList(eventCode))
        {
            playedEventList.Add(eventCode);
        }
    }

    // 플레이어가 수행한 이벤트중 하나를 삭제
    public void DeleteEventCodeFromList(string eventCode)
    {
        // 발생한 이벤트만 삭제
        if (CheckEventCodeFromPlayedEventList(eventCode))
        {
            playedEventList.Remove(eventCode);
        }
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
        if(GameManager.instance.GetPlayState() == GameManager.PlayState.Act)
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

    public string GetHigherCurrentPosition()
    {
        string str = currentPosition;
        string[] result = str.Split(new char[] { '_' });

        return result[0];
    }

    public void SetCurrentPosition(string currentPosition)
    {
        //Debug.Log(currentPosition + "으로 워프!");
        this.currentPosition = currentPosition;
        // 플레이어의 현재 위치에 따른 BGM 변경
        BGMManager.instance.AutoSelectBGM(SceneManager.GetActiveScene(), LoadSceneMode.Single);
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

    // 이벤트 변수 초기화 (처음하기 시에 사용)
    public void InitEventVariable()
    {
        eventVariable.InitEventVariables();
    }

    // 주소를 넘김
    public ref EventVariable GetEventVariableClass()
    {
        return ref eventVariable;
    }

    public bool CheckCondition_InteractionObject()
    {
        if (!UIManager.instance.GetIsOpenedParchment() && !UIManager.instance.isFading && !UIManager.instance.GetIsOpenNote()
            && !UIManager.instance.isPortaling && !MiniMapManager.instance.IsMiniMapOpen() && isNearObject
            && !EventManager.instance.isPlaying302Event)
        {
            if (GameManager.instance.GetPlayState() == GameManager.PlayState.Act)
            {
                if (!UIManager.instance.isConversationing)
                    return true;
            }
            else if (GameManager.instance.GetPlayState() == GameManager.PlayState.Tutorial)
                return true;
            else if (GameManager.instance.GetPlayState() == GameManager.PlayState.Ending)
                return true;

            return false;
        }
        else
            return false;
    }

    // 사건 4 첫 이벤트가 진행될 때, 메르테가 위를 보고있는 것 처럼 만들기
    public void ChangeStateFor301Event_Fisrt()
    {
        player.GetComponent<Animator>().SetFloat("y", 1);
    }

    // 사건 4 첫 이벤트가 진행될 때, 메르테가 정면을 보고있는 것 처럼 만들기
    public void ChangeStateFor301Event_Second()
    {
        player.GetComponent<Animator>().SetFloat("y", -1);
    }
}
