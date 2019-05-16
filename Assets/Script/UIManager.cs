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
    private UITextList testText;

    private int currentPage;
    public bool isPaging;           //책이 펼쳐지고 있는 중에는 Act 버튼이 눌리면 안됨.
    public bool isConversationing;  //대화창이 열려있는지 확인
    public bool canSkipConversation;//다른 대화로 넘어갈 수 있는지 확인
    public List<string> npcNameLists;  // 단서 내용1에 필요한 npc이름들 기록
    public List<string> sentenceList;     // 단서 내용1에 필요한 대화들 기록

    //private List<Interaction> interactionLists;

    // Use this for initialization
    void Start () {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        currentPage = 0;
        isPaging = false;
        isOpenedNote = false;

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
        if(Input.GetKeyDown(KeyCode.Tab) && !isPaging && !isConversationing)
        {
            // 수첩 열고닫을때마다 초기화
            ResetWrittenClueData();

            isOpenedNote = !isOpenedNote;
            GetClueButton.SetActive(!isOpenedNote);
            //Background.SetActive(isOpenedNote);
            NoteBook.SetActive(isOpenedNote);
            GetClueUI.SetActive(isOpenedNote);
            clueScroller.SetActive(isOpenedNote);

            Scroll.instance.ActivateUpButton(!isOpenedNote);
            Scroll.instance.ActivateDownButton(!isOpenedNote);
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
        clueContent.GetComponent<Text>().text = "(단서의 획득 장소\n& 단서의 획득 경로)";
        textAboutFirstClue.GetComponent<Text>().text = "(단서 내용 1)";
        textAboutSecondClue.GetComponent<Text>().text = "(단서 내용 2)";
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
        // 해당하는 단서의 index를 찾았으면, 그것을 토대로 수첩에서의 사진, 텍스트 등을 변경
        clueSketch.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/AboutClue/ClueImage/" + PlayerManager.instance.ClueLists[numOfAct][clueIndex].GetName());
        clueContent.GetComponent<Text>().text = PlayerManager.instance.ClueLists[numOfAct][clueIndex].GetDesc();

        /* 이름 : "대화" 형식으로 붙혀야함 */
        /* 이름 = tempNpcNameLists, 대화 = sentenceLists */
        textAboutFirstClue.GetComponent<Text>().text = PlayerManager.instance.ClueLists[numOfAct][clueIndex].GetFirstInfoOfClue();
        textAboutSecondClue.GetComponent<Text>().text = PlayerManager.instance.ClueLists[numOfAct][clueIndex].GetArrangedContent();

        testText.Add(PlayerManager.instance.ClueLists[numOfAct][clueIndex].GetFirstInfoOfClue());
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

}
