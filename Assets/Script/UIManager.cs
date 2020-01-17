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
    [SerializeField]
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
    private GameObject conversationUI;  //대화창 전체 UI
    [SerializeField]
    private GameObject characterNameBg; //대화 캐릭터명 창
    [SerializeField]
    private GameObject conversationBg;  //대화창 배경
    [SerializeField]
    private Text conversationText;      //대화 텍스트 창
    [SerializeField]
    private Text npcNameText;           //대화 캐릭터 텍스트 창
    [SerializeField]
    private Image npcImage;             //대화 캐릭터 이미지

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
    [SerializeField]
    private GameObject clueListContent; // 단서슬롯을 담고 있는 오브젝트(w,s키로 단서 슬롯을 선택할때 필요한 스크롤 이동에 쓰임
    public int shownSlotIndex;         // 현재 보고 있는 단서 슬롯의 index를 저장하기 위한 변수
    public bool isMovingSlot;          // 단서 슬롯이 이동해야하는지 여부를 저장하기 위한 변수
    public float tempYPosition;

    /* 단서 정리 UI */
    [SerializeField]
    private GameObject canvasForParchment;  // 양피지에 나오는 단서 리스트의 부모를 캔버스로 바꾸기 위한 변수
    [SerializeField]
    private GameObject parchment; // 단서 정리할 때 나오는 전체 양피지의 오브젝트를 가진 변수
    [SerializeField]
    private GameObject parchmentHelper; // 양피지를 스크롤 할 수 있도록 도와주는 스크롤뷰 오브젝트
    [SerializeField]
    private GameObject parchmentClueScrollList; // 이중 스크롤 하기 위해서 필요한 변수 -> 따로 스크롤 뷰를 빼와서 양피지 helper의 위쪽 순서로 놓으면 이중 스크롤을 할 수 있음. (layer 개념과 비슷함)
    private bool isOpenedParchment;         // 단서 정리창 열렸는지 여부
    [SerializeField]
    private RectTransform rectOfParchment;  // 양피지의 position 값을 가질 변수
    [SerializeField]
    private RectTransform rectOfParchmentHelper;    // 양피지 helper의 position 값을 가질 변수
    [SerializeField]
    private RectTransform rectOfParchmentClueScrollList;    // 양피지에 나타날 단서들의 리스트를 담을 스크롤뷰의 position 값을 가질 변수 ( = 양피지의 y위치 값과 같아야 함)
    private float yMinValue_RectOfParchment = -720.0f;
    private float yMinVallue_RectOfHelper = 0.0f;
    private float tempValue_RectOfParchment;        // 양피지의 Rect y값과 helper의 Rect y값을 매칭시켜 양피지의 Rect y값을 변화시키면 스크롤이 될 것임
    private float tempValue_RectOfHelper;
    [SerializeField]
    private GameObject parchmentUpButton;   // 양피지의 스크롤을 위한 위쪽 화살표
    [SerializeField]
    private GameObject parchmentDownButton;   // 양피지의 스크롤을 위한 아래쪽 화살표
    [SerializeField]
    private RectTransform paperOfDocument;     // 안드렌의 서류를 뜻하는 오브젝트
    [SerializeField]
    private GameObject documentCover;      // 안드렌의 서류봉투 열리는 부분의 게임 오브젝트

    /* W,S로 버튼 이동 Test */
    public Button testButton;
    public int buttonIndex;
    public string buttonNumOfAct;
    public Selectable nextButton;
    public ColorBlock colorBlock;

    private string currentPage;
    private bool isOpened;          //수첩이 열려있는지 확인
    public bool isPaging;           //책이 펼쳐지고 있는 중에는 Act 버튼이 눌리면 안됨.
    public bool isConversationing;  //대화창이 열려있는지 확인
    public bool canSkipConversation;//다른 대화로 넘어갈 수 있는지 확인
    public List<string> npcNameLists;  // 단서 내용1에 필요한 npc이름들 기록
    public List<string> sentenceList;     // 단서 내용1에 필요한 대화들 기록
    public int howManyOpenNote;
    private int tempIndex;              // 눌렀던 단서 슬롯을 또 누르게 되면 페이지 넘김 효과를 주지 않기 위한 변수

    //private List<Interaction> interactionLists;

    public Navigation customNav;
    public Navigation customNav2;

    // Use this for initialization
    void Awake () {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        currentPage = "51";
        isOpened = false;
        isPaging = false;
        isOpenedNote = false;
        isOpenedParchment = false;  /* test 1105 */
        howManyOpenNote = 0;
        tempIndex = -1;     // -1 == null
        shownSlotIndex = 1;
        isMovingSlot = false;

        Background.SetActive(isOpenedNote);
        NoteBook.SetActive(isOpenedNote);
        GetClueUI.SetActive(isOpenedNote);
        clueScroller.SetActive(isOpenedNote);
        /* 단서정리 test 1105 */
        parchment.SetActive(isOpenedParchment);
        parchmentHelper.SetActive(isOpenedParchment);
        parchmentClueScrollList.SetActive(isOpenedParchment);
        parchmentUpButton.SetActive(isOpenedParchment);
        parchmentDownButton.SetActive(isOpenedParchment);

        isConversationing = false;
        canSkipConversation = false;
        /* DialogManager에서 쓰임(test)
        SetAlphaToZero_ConversationUI();    //대화창 UI 투명화
        conversationUI.SetActive(false);
        */
        npcNameLists = new List<string>();
        sentenceList = new List<string>();


        //customNav = new Navigation();
        //customNav2 = new Navigation();
        //customNav.mode = Navigation.Mode.None;
        //customNav2.mode = Navigation.Mode.Vertical;

        /* 양피지 스크롤을 위한 작업 */
        tempValue_RectOfHelper = rectOfParchmentHelper.localPosition.y;

    }

    void Update()
    {
        /* 단서 정리 테스트용 0115 */
        if (Input.GetKeyDown(KeyCode.Y))
        {
            // On & Off
            isOpenedParchment = !isOpenedParchment;

            //양피지를 보이게 하기
            parchment.SetActive(isOpenedParchment);
            parchmentHelper.SetActive(isOpenedParchment);
            parchmentClueScrollList.SetActive(isOpenedParchment);
            // 만약 양피지가 닫힐 때, 화살표도 없애기
            if (!isOpenedParchment)
            {
                parchmentUpButton.SetActive(isOpenedParchment);
                parchmentDownButton.SetActive(isOpenedParchment);
            }

            // 현재 시간대에 발견한 단서가 4개 미만이라면, 양피지를 부모로 취해 단서 리스트의 영역에 마우스 커서가 있을 때에도 양피지가 스크롤이 되게끔 만든다.
            if (PlayerManager.instance.GetCount_ClueList_In_Certain_Timeslot() < 4)
                parchmentClueScrollList.transform.SetParent(parchment.transform);
            else
                parchmentClueScrollList.transform.SetParent(canvasForParchment.transform);

            // 양피지에 단서 리스트 출력(중복처리해야함)
            Inventory.instance.MakeClueSlotInParchment();
        }

        if (documentCover.activeSelf && paperOfDocument.localPosition.y > 600)
        {
            documentCover.SetActive(false);
        }

        // 마우스 휠을 올리거나 내렸을때, 양피지가 열려있을 때, 양피지를 스크롤하는 작업
        if (Input.GetAxis("Mouse ScrollWheel") < 0 || Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (GetIsOpenParchment())
            {
                // 양피지의 위치값을 양피지 helper의 위치값과 양피지의 최소 위치값을 이용하여 적용시킴
                rectOfParchment.localPosition = new Vector2(rectOfParchment.localPosition.x, rectOfParchmentHelper.localPosition.y + yMinValue_RectOfParchment);
                // 바뀐 양피지의 위치값을 이용하여, 양피지의 단서 리스트가 표현되는 스크롤 뷰의 위치값을 같게 만듦 (양피지를 따라가게)
                rectOfParchmentClueScrollList.localPosition = new Vector2(rectOfParchmentClueScrollList.localPosition.x, rectOfParchment.localPosition.y);

                /*
                tempValue_RectOfParchment = rectOfParchmentHelper.localPosition.y - tempValue_RectOfHelper; // 현재 y값과 후에 저장된 y값의 차이를 저장함, 이는 양피지의 위치에 반영될 것임
                rectOfParchmentHelper.localPosition = new Vector2(rectOfParchmentHelper.localPosition.x, tempValue_RectOfParchment + yMinVallue_RectOfHelper);
                rectOfParchment.localPosition = new Vector2(rectOfParchment.localPosition.x, tempValue_RectOfParchment + yMinValue_RectOfParchment);
                tempValue_RectOfHelper = rectOfParchmentHelper.localPosition.y; // 이전에 저장된 y값을 백업 해놓기 -> tempValue_RectOfParchment 를 구하기 위해서 필요함
                */
            }
        }

        if(Input.GetKeyDown(KeyCode.Space) && !isPaging && !isConversationing)
        {
            isOpened = !isOpened;       //열려있으면 닫고, 닫혀있으면 연다.

            // 수첩 열고닫을때마다 초기화
            ResetWrittenClueData();

            Inventory.instance.ResetSlotForTest();

            isOpenedNote = !isOpenedNote;
            //GetClueButton.SetActive(!isOpenedNote);
            Background.SetActive(isOpenedNote);
            NoteBook.SetActive(isOpenedNote);
            GetClueUI.SetActive(isOpenedNote);
            clueScroller.SetActive(isOpenedNote);

            tempIndex = 0;

            buttonIndex = 0;    /* for Button test */
            
            ItemDatabase.instance.LoadHaveDataOfAct("51");     // 수첩을 열면, 항상 사건 1의 첫번째 단서가 보여져야 함


            /* 아래의 코드는 전에 봤던 사건의 단서를 계속 봐야하는 것으로 기획이 변경되면 쓰면 됨 */
            /* 쓸 때는 AutoFlip 스크립트의 FlipPage(int PressAct)의 howManyOpenNote 주석처리 풀어야 함 */
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

        //w,s키로 인해 스크롤이 이동중일때, 마우스 휠로 새로운 이동을 감지하면, 이동중인 스크롤 멈추기
        if (Input.GetAxis("Mouse ScrollWheel") > 0 || Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            isMovingSlot = false;
        }

        //w,s키로 인해 스크롤이 이동될 수 있도록 움직여주는 if
        if (isMovingSlot)
        {
            clueListContent.GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(clueListContent.GetComponent<RectTransform>().localPosition,
                new Vector3(clueListContent.GetComponent<RectTransform>().localPosition.x, tempYPosition, 0),
                Time.deltaTime * 100);

            if (tempYPosition == clueListContent.GetComponent<RectTransform>().localPosition.y)
            {
                isMovingSlot = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.W) && isOpened)
        {
            if (!isPaging && (buttonIndex != 0) && (Inventory.instance.GetSlotCount() != 0))
            {
                // w,s로 이동될 다음 버튼이 있으면, 현재 버튼의 색을 회색에서 하얀색으로 바꿔놓기 위한 if
                if (nextButton != null)
                {
                    SetColorBlockToWhite();
                }

                // w,s키를 이용한 스크롤의 이동을 위한 if-else
                if (shownSlotIndex <= 1)
                {
                    shownSlotIndex = 1;
                }
                else
                {
                    shownSlotIndex -= 1;

                    if (clueListContent.GetComponent<RectTransform>().localPosition.y > 285.0f)
                    {
                        tempYPosition = clueListContent.GetComponent<RectTransform>().localPosition.y - 90.0f;
                        isMovingSlot = true;
                    }
                }

                nextButton = testButton.FindSelectableOnUp();

                SetColorBlockToGray();

                buttonIndex--;
                AutoFlip.instance.FlipPage(buttonIndex, buttonNumOfAct);
                
            }
            else if (isPaging)
            {
                Debug.Log("페이지 넘기는중");
                //Inventory.instance.GetSlotObject(buttonIndex).navigation = customNav;
            }

        }

        if (Input.GetKeyDown(KeyCode.S) && isOpened)
        {
            if (!isPaging && (buttonIndex != Inventory.instance.GetSlotCount() - 1) && (Inventory.instance.GetSlotCount() != 0))
            {
                // w,s로 이동될 다음 버튼이 있으면, 현재 버튼의 색을 회색에서 하얀색으로 바꿔놓기 위한 if
                if (nextButton != null)
                {
                    SetColorBlockToWhite();
                }

                //Inventory.instance.GetSlotObject(buttonIndex).navigation = customNav2;
                nextButton = testButton.FindSelectableOnDown();

                SetColorBlockToGray();  //선택된 버튼 색깔 바꾸기

                buttonIndex++;
                AutoFlip.instance.FlipPage(buttonIndex, buttonNumOfAct);

                // w,s키를 이용한 스크롤의 이동을 위한 if
                if (shownSlotIndex > 6)
                {
                    // 6번째 이후에 있는 단서 슬롯의 단서를 보려고 하는 경우, 단서 리스트를 y축으로 90 만큼 이동시킨다.
                    tempYPosition = clueListContent.GetComponent<RectTransform>().localPosition.y + 90.0f;
                    isMovingSlot = true;
                }
            }
            else if (isPaging)
            {
                Debug.Log("페이지 넘기는중");
                //Inventory.instance.GetSlotObject(buttonIndex).navigation = customNav;
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
    
    public void SetCurrentPage(string pressedAct)
    {
        this.currentPage = pressedAct;
    }

    public string GetCurrentPage()
    {
        return currentPage;
    }

    // 단서를 누를 때, 단서에 대한 스케치, 설명, 정리된 내용을 불러옴
    public void ShowClueData(int clueIndex, string numOfAct)
    {
        List<ClueStructure> tempList = PlayerManager.instance.playerClueLists.FindAll(x => x.GetNumOfAct() == numOfAct);

        if (tempList.Count == 0)
        {
            // 해당 사건의 획득한 단서가 없으면 빈 페이지를 보여줌
            Debug.Log("사건" + numOfAct + "의 단서가 없어요");
            CloseClueUI();
            return;
        }
        else
        {
            // 해당 사건의 획득한 단서가 있으면 ClueUI 활성화
            OpenClueUI();
            // 해당하는 단서의 index를 찾았으면, 그것을 토대로 수첩에서의 사진, 텍스트 등을 변경
            clueSketch.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/AboutClue/ClueImage/" + tempList[clueIndex].GetClueName());
            clueContent.GetComponent<Text>().text = "<size=30>" + tempList[clueIndex].GetObtainPos1() + "</size>" + "\n" + "<size=26>" + tempList[clueIndex].GetObtainPos2() + "</size>";

            /* 이름 : "대화" 형식으로 붙혀야함 */
            /* 이름 = tempNpcNameLists, 대화 = sentenceLists */
            textAboutFirstClue.GetComponent<Text>().text = tempList[clueIndex].GetFirstInfoOfClue();
            textAboutSecondClue.GetComponent<Text>().text = tempList[clueIndex].GetDesc();
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

    public bool GetIsOpenNote()
    {
        return isOpenedNote;
    }

    public bool GetIsOpenParchment()
    {
        return isOpenedParchment;
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

    // 대화창 Fade in
    public void OpenConversationUI()
    {
        conversationUI.SetActive(true);
        isConversationing = true;
    }

    // 대화창 Fade out
    public void CloseConversationUI()
    {
        conversationUI.SetActive(false);
        isConversationing = false;
    }

    // 1. 대화창 & 캐릭터명 창 fade in
    // 2. 캐릭터 이미지 & 캐릭터 이름 fade in
    // 3. 대화 출력(\n 을 csv에서 어떻게 받아올 수 있는지 고민해봐야함)
    public IEnumerator FadeEffect(float fadeTime, string fadeWhat)
    {
        //대화 글자를 나타내게 하고 싶으면, conversationText의 color도 다른방식으로 이용하면 될듯
        Color tempColor1, tempColor2, tempColor3, tempColor4;

        tempColor1 = conversationBg.GetComponent<Image>().color;
        tempColor2 = characterNameBg.GetComponent<Image>().color;
        tempColor3 = npcNameText.color;
        tempColor4 = npcImage.color;

        if (fadeWhat.Equals("In"))
        {
            // 투명 -> 불투명
            while (tempColor1.a < 1f && tempColor2.a < 1f && tempColor3.a < 1f && tempColor4.a < 1f)
            {
                tempColor1.a += Time.deltaTime / fadeTime;
                tempColor2.a = tempColor1.a;
                tempColor3.a = tempColor1.a;
                tempColor4.a = tempColor1.a;

                conversationBg.GetComponent<Image>().color = tempColor1;
                characterNameBg.GetComponent<Image>().color = tempColor2;
                npcNameText.color = tempColor3;
                npcImage.color = tempColor4;

                if (tempColor1.a >= 1f || tempColor2.a >= 1f || tempColor3.a >= 1f || tempColor4.a >= 1f)
                {
                    tempColor1.a = 1f;
                    tempColor2.a = 1f;
                    tempColor3.a = 1f;
                    tempColor4.a = 1f;
                }

                yield return null;
            }

            conversationBg.GetComponent<Image>().color = tempColor1;
            characterNameBg.GetComponent<Image>().color = tempColor2;
            npcNameText.color = tempColor3;
            npcImage.color = tempColor4;

            //StartCoroutine(DialogManager.instance.FadeTextEffect(fadeTime, fadeWhat));
        }
        else if (fadeWhat.Equals("Out"))
        {
            // 불투명 -> 투명
            while (tempColor1.a > 0f && tempColor2.a > 0f && tempColor3.a > 0f && tempColor4.a > 0f)
            {
                tempColor1.a -= Time.deltaTime / fadeTime;
                tempColor2.a = tempColor1.a;
                tempColor3.a = tempColor1.a;
                tempColor4.a = tempColor1.a;

                conversationBg.GetComponent<Image>().color = tempColor1;
                characterNameBg.GetComponent<Image>().color = tempColor2;
                npcNameText.color = tempColor3;
                npcImage.color = tempColor4;

                if (tempColor1.a <= 0f || tempColor2.a <= 0f || tempColor3.a <= 0f || tempColor4.a <= 0f)
                {
                    tempColor1.a = 0f;
                    tempColor2.a = 0f;
                    tempColor3.a = 0f;
                    tempColor4.a = 0f;
                }

                yield return null;
            }

            conversationBg.GetComponent<Image>().color = tempColor1;
            characterNameBg.GetComponent<Image>().color = tempColor2;
            npcNameText.color = tempColor3;
            npcImage.color = tempColor4;

            //StartCoroutine(DialogManager.instance.FadeTextEffect(fadeTime, fadeWhat));
            CloseConversationUI();
        }

        //yield return null;
    }

    public void SetAlphaToZero_ConversationUI()
    {
        Color tempColor;
        tempColor = characterNameBg.GetComponent<Image>().color;
        tempColor.a = 0f;
        characterNameBg.GetComponent<Image>().color = tempColor;

        tempColor = conversationBg.GetComponent<Image>().color;
        tempColor.a = 0f;
        conversationBg.GetComponent<Image>().color = tempColor;

        tempColor = npcNameText.color;
        tempColor.a = 0f;
        npcNameText.color = tempColor;

        tempColor = npcImage.color;
        tempColor.a = 0f;
        npcImage.color = tempColor;

        /* 추후에 글자를 1개씩 "나타나게" 하는 효과가 필요할 경우 사용할 것
        tempColor = conversationText.color;
        tempColor.a = 0f;
        conversationText.color = tempColor;
        */

        conversationUI.SetActive(false);
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

    public void SetColorBlockToWhite()
    {
        colorBlock = testButton.colors;
        colorBlock.normalColor = Color.white;
        colorBlock.highlightedColor = Color.white;
        colorBlock.pressedColor = Color.white;
        testButton.colors = colorBlock;

        colorBlock = nextButton.GetComponent<Button>().colors;
        colorBlock.normalColor = Color.white;
        colorBlock.highlightedColor = Color.white;
        colorBlock.pressedColor = Color.white;
        nextButton.GetComponent<Button>().colors = colorBlock;
    }

    public void SetColorBlockToGray()
    {
        //선택한 단서 슬롯의 색을 변화시키기
        colorBlock = testButton.colors;
        colorBlock.normalColor = Color.white;
        colorBlock.highlightedColor = Color.white;
        colorBlock.pressedColor = Color.white;
        testButton.colors = colorBlock;

        colorBlock = nextButton.GetComponent<Button>().colors;
        colorBlock.normalColor = Color.gray;
        colorBlock.highlightedColor = Color.gray;
        colorBlock.pressedColor = Color.gray;
        nextButton.GetComponent<Button>().colors = colorBlock;
    }
}
