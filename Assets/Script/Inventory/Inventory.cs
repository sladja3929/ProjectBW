using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    public static Inventory instance = null;

    public GameObject inventory;     // canvas
    public GameObject slotButton;    // slot 
    public GameObject inventoryPanel; // 슬롯을 담는 판넬
    /* 단서정리를 위해 0115 추가 */
    [SerializeField]
    private GameObject clueList_Panel_In_Parchment;    // 단서 정리 양피지에 단서들을 담을 판넬
    [SerializeField]
    private GameObject slot_In_Parchment;           //양피지의 판넬에 담길 슬롯의 프리팹을 저장할 변수
    private List<GameObject> slotList_In_Parchment; // 양피지에 보일 단서들을 저장할 리스트 변수

    private int dataCount;                  // clue count -> json 데이터 상의 단서 전체의 개수
    private List<GameObject> slot; // 획득한 단서에 해당하는 슬롯들을 담을 list
    private ItemDatabase itemDatabase;  // ItemDataBase 스크립트에 있는 내용을 사용하기 위한 변수
    
	// Use this for initialization
	void Start () {
        if (instance == null)
            instance = this;
        
        slot = new List<GameObject>();
        slotList_In_Parchment = new List<GameObject>();
        itemDatabase = GameObject.Find("DataManager").GetComponent<ItemDatabase>();

    }

    /* 단서 정리 (해당 시간대에 얻은 단서들을 양피지에 띄우기위한 함수) 0115 추가 */
    public void MakeClueSlotInParchment()
    {
        // 플레이어가 현재 시간대에 얻은 단서들의 리스트를 가져온다.
        List<ClueStructure> certainClueLists = PlayerManager.instance.playerClueLists_In_Certain_Timeslot;

        if (certainClueLists.Count == 0)
        {
            //Debug.Log("현 시간대에 얻은 단서가 없음");
            return;
        }

        GameObject addedSlot = slot_In_Parchment;   // 단서정리에 필요한 슬롯의 프리팹 정보를 담을 변수
        GameObject tempSlot;
        string tempClueName;

        // 리스트에 있는 단서들의 수만큼 for문으로 slot을 만들어 넣는다.
        for (int i = 0; i < certainClueLists.Count; i++)
        {
            tempSlot = Instantiate(addedSlot, clueList_Panel_In_Parchment.transform);
            slotList_In_Parchment.Add(tempSlot);
            tempClueName = certainClueLists[i].GetClueName();
            tempSlot.transform.Find("CluePortrait").GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/AboutClue/ClueImage/" + tempClueName);
            tempSlot.transform.Find("ClueName").GetComponent<Text>().text = "" + tempClueName;
        }

        // 현재 시간대에 가지고 있는 단서 리스트 초기화
        PlayerManager.instance.ResetClueList_In_Certain_Timeslot();
    }
    

    /* 단서를 획득할 때 마다, 수첩에 슬롯을 하나씩 추가시켜야함. */
    /* 이미 있는 단서를 획득처리할 경우 비정상적으로 작동하지만, 게임 내에서 중복되는 단서는 없을것 같다. */
    /* 있으면 수정해야댐 */
    public void MakeClueSlot(string clueName, string numOfAct)
    {
        GameObject addedSlotButton = slotButton;
        //Debug.Log("playerClueLists.Count = " + PlayerManager.instance.playerClueLists.Count);

        int tempIndex = PlayerManager.instance.playerClueLists.Count - 1; /* 0926 변경 */

        // 캐릭터의 ClueList에서 해당하는 단서가 있는지 찾고, 있으면 tempIndex에 해당 단서의 index 대입
        for (int i = 0; i < PlayerManager.instance.playerClueLists.Count; i++)
        {
            if (PlayerManager.instance.playerClueLists[i].GetClueName().Equals(clueName))
            {
                //int tempIndex = i;      // 캐릭터의 clueList에서 찾고자 하는 단서의 index가 저장될 변수  /* 0926 변경 */
                //Debug.Log("tempIndex = " + tempIndex);
                // 슬롯을 수첩에 추가
                slot.Add(Instantiate(addedSlotButton, inventoryPanel.transform));

                // 알맞은 단서의 이미지를 적용 (단서명_슬롯 의 형태로 이미지 저장할것!!)
                slot[tempIndex].transform.Find("SlotImage").GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/AboutClue/SlotImage/Slot_" + clueName);

                slot[tempIndex].transform.Find("SlotText").GetComponent<Text>().text = clueName;

                // 단서의 이미지를 적용시킨 후, 슬롯을 클릭했을때의 이벤트 처리
                slot[tempIndex].transform.GetComponent<Button>().onClick.AddListener(() => addedSlotButton.GetComponent<SlotButtonColor>().ChangeSlotColor(tempIndex));
                slot[tempIndex].transform.GetComponent<Button>().onClick.AddListener(() => AutoFlip.instance.FlipPage(tempIndex, numOfAct));
                //slot[tempIndex].transform.GetComponent<Button>().onClick.AddListener(() => UIManager.instance.ShowClueData(tempIndex, numOfAct));
                //slot[tempIndex].transform.GetComponent<Button>().navigation = UIManager.instance.customNav;

                ColorBlock tempColorBlock = slot[tempIndex].transform.GetComponent<Button>().colors;
                tempColorBlock.normalColor = Color.white;
                tempColorBlock.highlightedColor = Color.white;
                tempColorBlock.pressedColor = Color.white;
                slot[tempIndex].transform.GetComponent<Button>().colors = tempColorBlock;

                //Debug.Log(clueName + " 슬롯 생성 완료");
                break;
            }
            else
            {
                //Debug.Log("해당하는 단서 없음");
                tempIndex--;
            }
        } // for
        ResetSlotForTest();
    }

    public int GetNumSlotList_In_Parchment()
    {
        return slotList_In_Parchment.Count;
    }

    public void DestroySlotInParchment()
    {
        for (int i = 0; i < slotList_In_Parchment.Count; i++)
        {
            Destroy(slotList_In_Parchment[i]);
        }
    }

    public void MakeClueSlot(string numOfAct)
    {
        GameObject addedSlotButton = slotButton;
        List<ClueStructure> tempClueList = PlayerManager.instance.playerClueLists.FindAll(x => x.GetNumOfAct() == numOfAct);

        // 해당 액트의 데이터들을 보여주기
        for (int i = 0; i < tempClueList.Count; i++)
        {
            int tempIndex = i;
            string clueName = tempClueList[i].GetClueName(); 

            // 슬롯을 수첩에 추가
            slot.Add(Instantiate(addedSlotButton, inventoryPanel.transform));

            // 알맞은 단서의 이미지를 적용
            slot[tempIndex].transform.Find("SlotImage").GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/AboutClue/SlotImage/Slot_" + clueName);
            slot[tempIndex].transform.Find("SlotText").GetComponent<Text>().text = clueName;
            
            // 단서의 이미지를 적용시킨 후, 슬롯을 클릭했을때의 이벤트 처리
            slot[tempIndex].transform.GetComponent<Button>().onClick.AddListener(() => addedSlotButton.GetComponent<SlotButtonColor>().ChangeSlotColor(tempIndex));
            slot[tempIndex].transform.GetComponent<Button>().onClick.AddListener(() => AutoFlip.instance.FlipPage(tempIndex, numOfAct));
            //slot[tempIndex].transform.GetComponent<Button>().onClick.AddListener(() => UIManager.instance.ShowClueData(tempIndex, numOfAct));
            //slot[tempIndex].transform.GetComponent<Button>().navigation = UIManager.instance.customNav;

            ColorBlock tempColorBlock = slot[tempIndex].transform.GetComponent<Button>().colors;
            tempColorBlock.normalColor = Color.white;
            tempColorBlock.highlightedColor = Color.white;
            tempColorBlock.pressedColor = Color.white;
            slot[tempIndex].transform.GetComponent<Button>().colors = tempColorBlock;
            //Debug.Log(clueName + " 슬롯 생성 완료");
        }
    }

    /* 저장된 player의 단서파일을 불러올 시 수첩에 slot들을 만들어 주는 함수 */
    /* 플레이어가 가지고있던 index번째 단서에 맞게 이미지, 이벤트를 적용시킴 */
    /*
    public void MakeClueSlotByLoad(int index)
    {
        //기본값은 Act 1의 데이터들이다.
        GameObject addedSlotButton = slotButton;
        
        slot.Add(Instantiate(addedSlotButton, inventoryPanel.transform));
        slot[index].transform.Find("SlotImage").GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/AboutClue/SlotImage/Slot_" + (PlayerManager.instance.ClueLists[0])[index].GetName());
        slot[index].transform.GetComponent<Button>().onClick.AddListener(() => AutoFlip.instance.FlipPage(index, 0));
        //slot[index].transform.GetComponent<Button>().navigation = UIManager.instance.customNav;
        //slot[index].transform.GetComponent<Button>().onClick.AddListener(() => UIManager.instance.ShowClueData(index, 0));
    }
    */

    /* 저장된 player의 단서파일을 불러올 시
     * 기존에 있던 수첩의 slot 데이터 초기화 및 버튼 삭제 */
    public void ResetSlot()
    {
        int SlotCount = slot.Count;
        slot.Clear();

        //노트를 활성화 시켜야만 기존의 slot들을 비활성화 시킬 수 있음.
        //UIManager.instance.NoteOpen();

        //object를 비활성화 시키는것이 메모리에 많은 영향을 준다면 이 방법은 고쳐야할 필요가 있음
        for (int i = 0; i < SlotCount; i++)
        {
            GameObject.FindWithTag("SlotButton").SetActive(false);
        }
        //UIManager.instance.NoteClose();
    }

    public void ResetSlotForTest()
    {
        int SlotCount = slot.Count;
        slot.Clear();

        //노트를 활성화 시켜야만 기존의 slot들을 비활성화 시킬 수 있음.
        UIManager.instance.NoteOpen();

        //object를 비활성화 시키는것이 메모리에 많은 영향을 준다면 이 방법은 고쳐야할 필요가 있음
        for (int i = 0; i < SlotCount; i++)
        {
            GameObject.FindWithTag("SlotButton").SetActive(false);
        }
        UIManager.instance.NoteClose();
    }

    /* test for ws button */
    public int GetSlotCount()
    {
        return slot.Count;
    }

    public Button GetSlotObject(int index)
    {
        return slot[index].GetComponent<Button>();
    }

}
