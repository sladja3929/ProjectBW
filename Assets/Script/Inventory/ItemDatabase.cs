using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using UnityEngine.UI;

public class ItemDatabase : MonoBehaviour {

    public static ItemDatabase instance = null;

    public List<Clue> itemList; // 저장되어 있는 단서 목록 저장변수

    public string jsonString;
    public JsonData jsonData;

    private Dictionary<int, Dictionary<string, string>> clueList;
    private List<ClueStructure> clueStructureLists;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        if (instance == null)
            instance = this;

    }

    // 단서 정보 최신화
    public void SetLists()
    {
        clueList = GameObject.Find("DataManager").GetComponent<CSVParser>().GetClueList();
        clueStructureLists = GameObject.Find("DataManager").GetComponent<CSVParser>().GetClueStructureLists();
    }

    /* 게임에서 필요한 모든 단서데이터를 불러옴 */
    public void LoadAllClueData()
    {
        /* build시에도 데이터파일들이 함께 포함되게 하기 위해서, streamingAsset 폴더 이용 */
        //jsonString = File.ReadAllText(Application.dataPath + "/Resources/Data/ClueData.json");
        jsonString = File.ReadAllText(Application.streamingAssetsPath + "/Data/ClueData.json");
        jsonData = JsonMapper.ToObject(jsonString);
    }

    /* 게임에 필요한 모든 단서 데이터의 개수파악(임시) */
    public int GetDataCount()
    {
        //JsonData data = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/Resources/Data/ClueData.json"));
        JsonData data = JsonMapper.ToObject(File.ReadAllText(Application.streamingAssetsPath + "/Data/ClueData.json"));
        return data.Count;
    }

    /* player가 가지고 있던 단서 저장 */
    public void SavePlayerData(List<Clue>[] playerClueList)
    {        
        JsonData tempJsonData = JsonMapper.ToJson(playerClueList);
        string tempStringData = tempJsonData.ToString();

        //File.WriteAllText(Application.dataPath + "/Resources/Data/Player.json", tempStringData);
        File.WriteAllText(Application.streamingAssetsPath + "/Data/Player.json", tempStringData);
        Debug.Log("현재 플레이어가 가지고 있는 단서들의 데이터를 저장했습니다.");
    }

    /* 여태 획득했던 단서 데이터 불러오기 */
    /* 잘 불러와지나, json 파일 상에서 한글로 표현이 안됨 */
    /* JsonMapper.ToJson() 함수에서 반환해줄 때, 한글 데이터가 유니코드로 찍히는것 확인 */
    /* 수첩에 유동적인 단서슬롯의 구현시에 문제가 된다면 JSON pluggin을 교체하는것을 고려할 것 */
    /*
    public void LoadPlayerData()
    {
        PlayerManager tempPlayerManager = PlayerManager.instance;
        //기존에 있던 player의 데이터 리셋
        tempPlayerManager.ResetClueList();
        //기존에 있던 수첩의 slot 데이터 및 오브젝트 리셋
        UIManager.instance.NoteOpen();
        Inventory.instance.ResetSlot(); //수첩이 열려있어야 슬롯 리셋 가능
        UIManager.instance.NoteClose();

        Clue tempClue;
        //JsonData tempData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/Resources/Data/Player.json"));
        JsonData tempData = JsonMapper.ToObject(File.ReadAllText(Application.streamingAssetsPath + "/Data/Player.json"));

        for (int i = 0; i < tempData.Count; i++)
        {
            for (int j = 0; j < tempData[i].Count; j++)
            {
                tempClue = new Clue((tempData[i])[j]["numOfAct"].ToString(), (tempData[i])[j]["name"].ToString(), (tempData[i])[j]["description"].ToString(), (tempData[i])[j]["firstInfoOfClue"].ToString(), (tempData[i])[j]["arranged_content"].ToString());
                tempPlayerManager.AddClueToList(tempClue, i);

                if (i == 0)
                {
                    //불러온 데이터들에 맞춰, Inventory의 slot을 생성시켜야함 
                    // 첫 act의 단서가 기본으로 나타나게 하자.
                    Inventory.instance.MakeClueSlotByLoad(i);
                }
            }
        }

        Debug.Log("플레이어의 데이터를 불러왔습니다");
    }
    */

    /* player가 특정 단서를 처음 얻었을 때, 그 단서를 player의 cluelist에 추가하는 함수 */
    public string FindClue(string name)
    {
        int clueIndex = clueStructureLists.FindIndex(x => x.GetClueName() == name);
        string numOfAct = clueStructureLists[clueIndex].GetNumOfAct();
        PlayerManager.instance.AddClueToList(clueStructureLists[clueIndex]);
        return numOfAct;
    }

    public void AddClueForTest(string name)
    {
        int clueIndex = clueStructureLists.FindIndex(x => x.GetClueName() == name);
        PlayerManager.instance.AddClueToList(clueStructureLists[clueIndex]);
    }

    /* 추후에 단서의 content를 변경시킬 함수 필요 */
    public void ClueUpdate()
    {

    }

    /* 이미 저장되어있는 데이터들을 이용하여 해당 액트에 따른 데이터 불러오기 */
    public void LoadHaveDataOfAct(string numOfAct)
    {
        //Inventory.instance.ResetSlotForTest();
        UIManager.instance.SetCurrentPage(numOfAct);
        Inventory.instance.MakeClueSlot(numOfAct);
        UIManager.instance.ShowClueData(0, numOfAct);   //해당 사건의 첫번째로 획득한 단서에 대한 내용 보여주기
    }

}
