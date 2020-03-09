using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    private EventVariable eventVariable;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // Use this for initialization
    void Start () {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        eventVariable = PlayerManager.instance.GetEventVariableClass();
    }

    void Update()
    {
        // 처음 하기
        if (Input.GetKeyDown(KeyCode.F1))
        {
            // 초기 데이터를 불러오고
            CSVParser.instance.InitDataFromCSV();
            // 불러온 데이터를 적용시킴
            DialogManager.instance.SetLists();
            ItemDatabase.instance.SetLists();
        }

        // 저장
        if (Input.GetKeyDown(KeyCode.F2))
        {
            CSVParser.instance.SaveCSVData();
            SavePlayerData();
            eventVariable.SaveEventVariables();
        }

        // 불러오기
        if (Input.GetKeyDown(KeyCode.F3))
        {
            // 저장된 데이터를 불러오고
            CSVParser.instance.LoadCSVData();
            // 불러온 데이터를 적용시킴
            DialogManager.instance.SetLists();
            ItemDatabase.instance.SetLists();
            LoadPlayerData();
            eventVariable.LoadEventVariables();
            //PlayerManager.instance.ResetClueList_In_Certain_Timeslot();
        }
    }

    public void GetClue(string clueName)
    {
        if (!PlayerManager.instance.CheckClue(clueName))
        {
            //int numOfAct = ItemDatabase.instance.FindClue(clueName);
            //Debug.Log("Act " + (numOfAct) + "의 단서인 " + clueName + "를 얻었습니다.");
            string numOfAct = ItemDatabase.instance.FindClue(clueName);
            //Debug.Log("clueName = " + clueName + " , numOfAct = " + numOfAct);
            //Debug.Log("사건 " + numOfAct + "의 단서인 " + clueName + "를 얻었습니다.");
            Inventory.instance.MakeClueSlot(clueName, numOfAct); // 수첩에 Clue slot 추가
            
        } else
        {
            //Debug.Log("이미 획득한 단서입니다.");
        }
    }

    // 처음 부터
    public void PlayNewGame()
    {
        // 초기 데이터를 불러오고
        CSVParser.instance.InitDataFromCSV();
        // 불러온 데이터를 적용시킴
        DialogManager.instance.SetLists();
        ItemDatabase.instance.SetLists();
    }

    // 이어 하기
    public void PlaySaveGame()
    {
        // 저장된 데이터를 불러오고
        CSVParser.instance.LoadCSVData();
        // 불러온 데이터를 적용시킴
        DialogManager.instance.SetLists();
        ItemDatabase.instance.SetLists();
        LoadPlayerData();
        eventVariable.LoadEventVariables();
    }

    /* Load된 Player의 정보를 적용 */
    /* 클래스 변수에 넣어진 값들을, 실제 Player 데이터로 적용함 */
    public void LoadPlayerData()
    {
        PlayerInfo tempPlayerInfo = new PlayerInfo();

        tempPlayerInfo.LoadPlayerInfo();

        string tempNumOfAct = tempPlayerInfo.GetNumOfAct();
        string tempTimeSlot = tempPlayerInfo.GetTimeSlot();
        List<string> tempEventIndexLists = tempPlayerInfo.GetEventIndexList();
        List<string> tempPlayerClueNameLists = tempPlayerInfo.GetPlayerClueNameLists();
        List<string> tempFirstInfoOfClueLists = tempPlayerInfo.GetFirstInfoOfClueLists(); // 로드 끝

        /* 적용 */
        PlayerManager.instance.NumOfAct = tempNumOfAct;
        PlayerManager.instance.TimeSlot = tempTimeSlot;

        for (int i = 0; i < tempEventIndexLists.Count; i++)
        {
            PlayerManager.instance.AddEventCodeToList(tempEventIndexLists[i]);
        }

        for (int i = 0; i < tempPlayerClueNameLists.Count; i++)
        {
            GetClue(tempPlayerClueNameLists[i]);
            PlayerManager.instance.playerClueLists[i].SetFirstInfoOfClue(tempFirstInfoOfClueLists[i]);
        }
    }

    /* Player의 정보를 Save */
    public void SavePlayerData()
    {
        PlayerInfo tempPlayerInfo = new PlayerInfo(PlayerManager.instance.NumOfAct, PlayerManager.instance.TimeSlot, PlayerManager.instance.GetPlayedEventList(), PlayerManager.instance.playerClueLists);

        tempPlayerInfo.SavePlayerInfo();
    }
}
