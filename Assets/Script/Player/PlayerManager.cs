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

    // Use this for initialization
    void Awake () {
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
    
        currentPosition = "Downtown_Street1";

        isInPortalZone = false;
    }

    // Update is called once per frame
    void Update () {
        /* 오브젝트와의 상호작용을 위한 if */
        if (Input.GetMouseButtonDown(0) && isNearObject && !UIManager.instance.isConversationing)
        {
            pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            ray = new Ray2D(pos, Vector2.zero);
            hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider == null)
            {
                Debug.Log("아무것도 안맞죠?");
            }
            else if (hit.collider.tag == "InteractionObject")
            {
                Debug.Log("hit.collider.name : " + npcParser.GetNpcCodeFromName(hit.collider.name));
                DialogManager.instance.InteractionWithObject(npcParser.GetNpcCodeFromName(hit.collider.name));
                //if(hit.collider.name.Equals("ER"))
                //    DialogManager.instance.InteractionWithObject(er);

                //if (hit.collider.name.Equals("GarbageBag"))
                //    DialogManager.instance.InteractionWithObject(garbageBag);
            }
        }

        /* for test 1226 */
        if (Input.GetKey(KeyCode.Alpha1))
        {
            TimeSlot = "71";
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            TimeSlot = "83";
        }
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
