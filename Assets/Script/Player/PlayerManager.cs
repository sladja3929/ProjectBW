using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public static PlayerManager instance = null;

    /* player가 있다는 가정 */
    //public List<Clue> ClueList;        // player가 얻은 단서들의 리스트
    public List<Clue>[] ClueLists;        // player가 얻은 단서들의 리스트
    public int NumOfAct { get; set; }    // player가 현재 진행하고 있는 Act

    // Use this for initialization
    void Start () {
        if (instance == null)
            instance = this;

        //ClueList = new List<Clue>();
        ClueLists = new List<Clue>[5];  //Act5까지의 단서들 리스트

        //ClueLists 초기화
        for (int i = 0; i < ClueLists.Length; i++)
            ClueLists[i] = new List<Clue>();

        NumOfAct = 1;   //Act1 시작
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    

    /* player의 단서파일을 불러올때, 초기화시키기 위함 */
    public void ResetClueList()
    {
        for(int i=0; i<ClueLists.Length; i++)
            ClueLists[i].Clear();
    }

    /* player가 얻은 데이터를 단서리스트에 추가 */
    //public void AddClueToList(Clue clueData)
    //{
    //    ClueList.Add(clueData);
    //    Debug.Log("여태 획득한 단서 수 : " + ClueList.Count);
    //}

    /* player가 얻은 해당 Act의 단서를 단서리스트에 추가 */
    public void AddClueToList(Clue clueData, int numOfAct)
    {
        ClueLists[numOfAct].Add(clueData);
    }

    /* 단서 중복 방지 */
    public bool CheckClue(string clueName)
    {
        for(int i=0; i<ClueLists.Length; i++)
        {
            for(int j=0; j<ClueLists[i].Count; j++)
            {
                if (ClueLists[i][j].GetName().Equals(clueName))
                    return false;   //단서 스크롤바 테스트할떄 -> false로 바꾸기
            }
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

}
