using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.IO;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    private EventVariable eventVariable;
    public enum GameState {Idle, NewGame_Loaded, PastGame_Loaded };
    private GameState gameState;

    public Thread thread;

    // 대칭키 비밀번호
    private const string passwordForAES = "gmrrhkqor";
    private DataEncryption dataAES;

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

        gameState = GameState.Idle;
        eventVariable = new EventVariable();
        dataAES = new DataEncryption();
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

    public string EncryptData(string text)
    {
        return dataAES.EncryptString(text, passwordForAES);
    }

    public string DecryptData(string text)
    {
        return dataAES.DecryptString(text, passwordForAES);
    }

    public GameState GetGameState()
    {
        return gameState;
    }

    public void SetGameState(GameState gameState)
    {
        this.gameState = gameState;
    }

    // 처음 부터
    public void PlayNewGame()
    {
        // 비동기적으로 초기 데이터를 불러옴
        thread = new Thread(CSVParser.instance.InitDataFromCSV);
        thread.IsBackground = true;
        thread.Start();
        //CSVParser.instance.InitDataFromCSV();
        // 불러온 데이터를 적용시킴
        //DialogManager.instance.SetLists();
        //ItemDatabase.instance.SetLists();
    }

    // 이어 하기
    public void PlaySaveGame()
    {
        try
        {
            // 비동기적으로 저장된 데이터를 불러옴
            thread = new Thread(CSVParser.instance.LoadCSVData);
            thread.IsBackground = true;
            thread.Start();
            //CSVParser.instance.LoadCSVData();
            // 불러온 데이터를 적용시킴
            //DialogManager.instance.SetLists();
            //ItemDatabase.instance.SetLists();
            //LoadPlayerData();
            eventVariable.LoadEventVariables();
        }
        catch
        {
            // 로드 실패시 -> 저장된 게임 데이터가 없을 시
            gameState = GameState.Idle;
            return;
        }
    }

    // PlayerManager.cs 에서 비동기적으로 사용되는 함수
    public void SaveGameData(object th)
    {
        CSVParser.instance.SaveCSVData();
        SavePlayerData();
        eventVariable.SaveEventVariables();

        //Thread tempTh = (Thread)th;
        //tempTh.Abort();
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

    public void SetEventVariable(ref EventVariable eventVariable)
    {
        this.eventVariable = eventVariable;
    }
}
