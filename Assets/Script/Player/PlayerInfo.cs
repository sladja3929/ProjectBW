using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerInfo
{
    public string numOfAct;
    public string timeSlot;
    public List<string> eventIndexList;
    public List<string> playerClueNameLists;
    public List<string> firstInfoOfClueLists;

    // Load용 생성자
    public PlayerInfo()
    {
        InitPlayerInfo();
    }

    // Save용 생성자
    public PlayerInfo(string numOfAct, string timeSlot, List<string> eventIndexList, List<ClueStructure> playerClueLists)
    {
        InitPlayerInfo();
        this.numOfAct = numOfAct;
        this.timeSlot = timeSlot;
        this.eventIndexList = eventIndexList;

        for (int i = 0; i < playerClueLists.Count; i++)
        {
            this.playerClueNameLists.Add(playerClueLists[i].GetClueName());
            this.firstInfoOfClueLists.Add(playerClueLists[i].GetFirstInfoOfClue());
        }
    }

    public string GetNumOfAct()
    {
        return numOfAct;
    }

    public string GetTimeSlot()
    {
        return timeSlot;
    }

    public List<string> GetEventIndexList()
    {
        return eventIndexList;
    }

    public List<string> GetPlayerClueNameLists()
    {
        return playerClueNameLists;
    }

    public List<string> GetFirstInfoOfClueLists()
    {
        return firstInfoOfClueLists;
    }

    // PlayerInfo의 변수들 초기화
    public void InitPlayerInfo()
    {
        this.numOfAct = "";
        this.timeSlot = "";
        this.eventIndexList = new List<string>();
        this.playerClueNameLists = new List<string>();
        this.firstInfoOfClueLists = new List<string>();
    }

    /* Save 파일을 불러온 후, 클래스 변수에 넣음 */
    public void SetPlayerInfo(JsonData jsonPlayerInfoData)
    {
        this.numOfAct = jsonPlayerInfoData["numOfAct"].ToString();
        this.timeSlot = jsonPlayerInfoData["timeSlot"].ToString();

        for (int i = 0; i < jsonPlayerInfoData["eventIndexList"].Count; i++)
        {
            eventIndexList.Add(jsonPlayerInfoData["eventIndexList"][i].ToString());
        }

        for (int i = 0; i < jsonPlayerInfoData["playerClueNameLists"].Count; i++)
        {
            playerClueNameLists.Add(jsonPlayerInfoData["playerClueNameLists"][i].ToString());
            firstInfoOfClueLists.Add(jsonPlayerInfoData["firstInfoOfClueLists"][i].ToString());
        }
    }

    /* 이벤트 관련 변수 Save 파일 로드하고, 값 설정하기 */
    public void LoadPlayerInfo()
    {
        string loadDataPath = Application.streamingAssetsPath + "/Data/PlayerInfo.json";
        string tempJsonString = File.ReadAllText(loadDataPath);
        JsonData jsonData = JsonMapper.ToObject(tempJsonString);
        SetPlayerInfo(jsonData);
    }

    /* 이벤트 관련 변수 Save 파일 만들기 */
    public void SavePlayerInfo()
    {
        string saveDataPath = Application.streamingAssetsPath + "/Data/PlayerInfo.json";
        // PlayerInfo 클래스 통째로 json화
        JsonData tempJsonData = JsonMapper.ToJson(this);
        string tempStringData = tempJsonData.ToString();
        File.WriteAllText(saveDataPath, tempStringData, System.Text.Encoding.UTF8);
    }
}
