using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public static UIManager instance = null;

    /* 테스트용 단서 획득 UI */
    [SerializeField]
    private GameObject GetClueUI;
    [SerializeField]
    private GameObject GetClueButton;

    /* Clue UI */
    [SerializeField]
    private GameObject Background;     // 배경
    [SerializeField]
    private GameObject NoteBook;        // 수첩
    private bool isOpenedNote;              // 수첩이 열려있는지의 여부
    [SerializeField]
    private GameObject clueSketch;      //단서의 스케치 이미지
    [SerializeField]
    private GameObject clueContent;     //단서의 기초 설명 텍스트
    [SerializeField]
    private GameObject textAboutFirstClue; // 정리된 단서에 대한 설명 텍스트
    [SerializeField]
    private GameObject textAboutSecondClue; // 정리된 단서에 대한 설명 텍스트
    [SerializeField]
    private GameObject conversationUI;  //대화창 UI
    [SerializeField]
    private GameObject clueScroller;    //수첩 내의 단서 리스트 스크롤바
    [SerializeField]
    private GameObject firstClueUpButton;
    [SerializeField]
    private GameObject firstClueDownButton;
    [SerializeField]
    private GameObject secondClueUpButton;
    [SerializeField]
    private GameObject secondClueDownButton;


    private int currentPage;
    public bool isPaging;           //책이 펼쳐지고 있는 중에는 Act 버튼이 눌리면 안됨.
    public bool isConversationing;  //대화창이 열려있는지 확인
    public bool canSkipConversation;//다른 대화로 넘어갈 수 있는지 확인
    public List<string> npcNameLists;  // 단서 내용1에 필요한 npc이름들 기록
    public List<string> sentenceList;     // 단서 내용1에 필요한 대화들 기록
    public int howManyOpenNote;
    private int tempIndex;              // 눌렀던 단서 슬롯을 또 누르게 되면 페이지 넘김 효과를 주지 않기 위한 변수

    //private List<Interaction> interactionLists;

    // Use this for initialization
    void Awake () {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        currentPage = 0;
        isPaging = false;
        isOpenedNote = false;
        howManyOpenNote = 0;
        tempIndex = -1;     // -1 == null

        //Background.SetActive(isOpenedNote);
        NoteBook.SetActive(isOpenedNote);
        GetClueUI.SetActive(isOpenedNote);
        clueScroller.SetActive(isOpenedNote);

        isConversationing = false;
        canSkipConversation = false;
        conversationUI.SetActive(false);
        
        npcNameLists = new List<string>();
        sentenceList = new List<string>();
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !isPaging && !isConversationing)
        {
            // 수첩 열고닫을때마다 초기화
            ResetWrittenClueData();

            Inventory.instance.ResetSlotForTest();

            isOpenedNote = !isOpenedNote;
            GetClueButton.SetActive(!isOpenedNote);
            //Background.SetActive(isOpenedNote);
            NoteBook.SetActive(isOpenedNote);
            GetClueUI.SetActive(isOpenedNote);
            clueScroller.SetActive(isOpenedNote);

            tempIndex = 0;
            
            ItemDatabase.instance.LoadHaveDataOfAct(0);     // 수첩을 열면, 항상 사건 1의 첫번째 단서가 보여져야 함

            /* 아래의 코드는 전에 봤던 사건의 단서를 계속 봐야하는 것으로 기획이 변경되면 쓰면 됨 */
            //if (howManyOpenNote == 0)   // 사건 버튼을 누를때 howManyOpenNote 변수 값 증가
            //{
            //    ShowClueData(0, 0);     // 수첩을 처음 열면, 사건 1의 첫번째 단서가 보여져야 함
            //}
            //else
            //{
            //    ShowClueData(0, PlayerManager.instance.NumOfAct); // 수첩을 열때마다 전에 봤었던 사건의 첫번째 단서가 보여져야함
            //}

            ActivateUpDownButton(!isOpenedNote);
        }

        if (Input.GetMouseButtonDown(0) && isConversationing && canSkipConversation)
        {
            //텍스트가 가득 찼으면 textfull만 false로 바꾸고, 가득찬게 아니면 다음 대화 출력
            if (DialogManager.instance.isTextFull)
            {
                DialogManager.instance.isTextFull = false;
            }
            else
            {
                DialogManager.instance.NextSentence();
                Debug.Log("NextSentence() 실행중");
            }
        }
    }

    public void ResetWrittenClueData()
    {
        clueSketch.GetComponent<Image>().sprite = null;
        clueContent.GetComponent<Text>().text = "";
        textAboutFirstClue.GetComponent<Text>().text = "";
        textAboutSecondClue.GetComponent<Text>().text = "";
    }
    
    public void SetCurrentPage(int pressedAct)
    {
        this.currentPage = pressedAct;
    }

    public int GetCurrentPage()
    {
        return currentPage;
    }

    // 단서를 누를 때, 단서에 대한 스케치, 설명, 정리된 내용을 불러옴
    public void ShowClueData(int clueIndex, int numOfAct)
    {
        if (PlayerManager.instance.ClueLists[numOfAct].Count == 0)
        {
            // 해당 사건의 획득한 단서가 없으면 빈 페이지를 보여줌
            CloseClueUI();
            return;
        }
        else
        {
            // 해당 사건의 획득한 단서가 있으면 ClueUI 활성화
            OpenClueUI();
            // 해당하는 단서의 index를 찾았으면, 그것을 토대로 수첩에서의 사진, 텍스트 등을 변경
            clueSketch.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/AboutClue/ClueImage/" + PlayerManager.instance.ClueLists[numOfAct][clueIndex].GetName());
            clueContent.GetComponent<Text>().text = PlayerManager.instance.ClueLists[numOfAct][clueIndex].GetDesc();

            /* 이름 : "대화" 형식으로 붙혀야함 */
            /* 이름 = tempNpcNameLists, 대화 = sentenceLists */
            textAboutFirstClue.GetComponent<Text>().text = PlayerManager.instance.ClueLists[numOfAct][clueIndex].GetFirstInfoOfClue();
            textAboutSecondClue.GetComponent<Text>().text = PlayerManager.instance.ClueLists[numOfAct][clueIndex].GetArrangedContent();
        }
    }

    public void SetTempNpcNameLists(List<string> npcNameLists)
    {
        this.npcNameLists = npcNameLists;
    }

    public void SetTempSentenceLists(List<string> sentenceLists)
    {
        this.sentenceList = sentenceLists;
    }
    
    public void NoteOpen()
    {
        NoteBook.SetActive(true);
        GetClueUI.SetActive(true);
        clueScroller.SetActive(true);
    }

    public void NoteClose()
    {
        NoteBook.SetActive(false);
        GetClueUI.SetActive(false);
        clueScroller.SetActive(false);
    }

    public void OpenClueUI()
    {
        GetClueUI.SetActive(true);
    }

    public void CloseClueUI()
    {
        GetClueUI.SetActive(false);
    }

    public void OpenConversationUI()
    {
        conversationUI.SetActive(true);
        isConversationing = true;
    }

    public void CloseConversationUI()
    {
        conversationUI.SetActive(false);
        isConversationing = false;
    }

    public void OpenGetClueButton()
    {
        GetClueButton.SetActive(true);
    }

    public void CloseGetClueButton()
    {
        GetClueButton.SetActive(false);
    }

    public void ActivateUpDownButton(bool setBool)
    {
        firstClueUpButton.SetActive(setBool);
        firstClueDownButton.SetActive(setBool);
        secondClueUpButton.SetActive(setBool);
        secondClueDownButton.SetActive(setBool);
    }

    public int GetTempIndex()
    {
        return this.tempIndex;
    }

    public void SetTempIndex(int tempIndex)
    {
        this.tempIndex = tempIndex;
    }
}
