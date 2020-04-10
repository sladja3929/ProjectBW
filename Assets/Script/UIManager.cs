using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public static UIManager instance = null;

    /*일시정지*/
    [SerializeField]
    private bool ispaused; // 일시정지되었는가?

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

    [SerializeField]
    public GameObject act3Button;
    [SerializeField]
    public GameObject act4Button;
    [SerializeField]
    public GameObject act5Button;

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
    private GameObject DocumentOfAndren;    // 안드렌의 서류 전체를 담당하는 오브젝트
    [SerializeField]
    private RectTransform paperOfDocument;     // 안드렌의 서류를 뜻하는 오브젝트
    [SerializeField]
    private GameObject documentCover;      // 안드렌의 서류봉투 열리는 부분의 게임 오브젝트
    public bool isReadParchment;            // 양피지를 끝까지 읽었는지 확인하는 변수
    [SerializeField]
    private GameObject fadeInOutPanel;      // 시간대가 지났다는 것을 알리기 위한 FadeInOut 패널
    [SerializeField]
    private Animator fadeInOutAnimator;     // fadeinout 애니메이터
    [SerializeField]
    private GameObject timeSlotText;        // 시간대 변경 텍스트
    [SerializeField]
    private GameObject wordOfMerte;         // 메르테의 말
    [SerializeField]
    private GameObject nameOfCase;          // 양피지 맨 윗쪽에 있는 텍스트

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
    public bool isTypingText;      // 대화가 출력되고 있는가?
    public bool isFading;           //대화창이 Fade되고 있는가?
    public bool canSkipConversation;//다른 대화로 넘어갈 수 있는지 확인
    public bool playerWantToSkip;   // 플레이어가 스킵을 할 경우
    public List<string> npcNameLists;  // 단서 내용1에 필요한 npc이름들 기록
    public List<string> sentenceList;     // 단서 내용1에 필요한 대화들 기록
    public int howManyOpenNote;
    private int tempIndex;              // 눌렀던 단서 슬롯을 또 누르게 되면 페이지 넘김 효과를 주지 않기 위한 변수

    /* 포탈을 타고있을때 */
    public bool isPortaling;    // 포탈을 통해 이동을 하고 있는지 확인

    [SerializeField]
    private GameObject deadBodyImage;   // 사체 묘사 이미지

    // 대화창 초상화
    [SerializeField]
    private GameObject characterFrame;
    [SerializeField]
    private GameObject characterBackfroundImage;
    [SerializeField]
    private GameObject characterImage;

    // Use this for initialization
    void Awake () {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        /*일시정지*/
        SetIsPausedFalse();

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
        isReadParchment = false;
        isPortaling = false;

        isConversationing = false;
        isTypingText = false;
        isFading = false;
        canSkipConversation = false;
        playerWantToSkip = false;
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
        //일시정지 상태가 아닐 때 && Act를 플레이하고 있을 때
        if (!UIManager.instance.GetIsPaused())
        {
            /* 단서 정리 테스트용 0115 */
            /*
            if ((Input.GetKeyDown(KeyCode.E) && isReadParchment))
            {
                isReadParchment = false;
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
            }*/

            if (documentCover.activeSelf && paperOfDocument.localPosition.y > 600)
            {
                SetDocumentCover(false);
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

            // esc로 수첩 닫기
            if (Input.GetKeyDown(KeyCode.Escape) && isOpenedNote && !isPaging && !isFading && !isOpenedParchment)
            {
                if (GameManager.instance.GetPlayState() == GameManager.PlayState.Act && !isConversationing)
                {
                    Background.SetActive(!isOpenedNote);
                    NoteBook.SetActive(!isOpenedNote);
                    GetClueUI.SetActive(!isOpenedNote);
                    clueScroller.SetActive(!isOpenedNote);

                    Invoke("SetNegativeIsOpenedNote", 0.05f); // 해당 작업 안해주면, 수첩 닫히면서 pause 화면 나옴
                }
                else if (GameManager.instance.GetPlayState() == GameManager.PlayState.Tutorial)
                {
                    TutorialManager.instance.isNoteTutorial = false;

                    Background.SetActive(!isOpenedNote);
                    NoteBook.SetActive(!isOpenedNote);
                    GetClueUI.SetActive(!isOpenedNote);
                    clueScroller.SetActive(!isOpenedNote);

                    Invoke("SetNegativeIsOpenedNote", 0.05f); // 해당 작업 안해주면, 수첩 닫히면서 pause 화면 나옴
                }
            }

            if (Input.GetKeyDown(KeyCode.Space) && !MiniMapManager.instance.IsMiniMapOpen() && !isPaging && !isFading && !isOpenedParchment)
            {
                if (GameManager.instance.GetPlayState() == GameManager.PlayState.Act && !isConversationing)
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

                    if(isOpened == true)
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
                else if (GameManager.instance.GetPlayState() == GameManager.PlayState.Tutorial && TutorialManager.instance.isCompletedTutorial[19])
                {   // 수첩 튜토리얼을 진행하고 있거나, 진행한 적이 있는 경우에만 수첩 활성화
                    isOpened = !isOpened;       //열려있으면 닫고, 닫혀있으면 연다.

                    TutorialManager.instance.isNoteTutorial = false;

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

                    if (isOpened == true)
                        ItemDatabase.instance.LoadHaveDataOfAct("51");     // 수첩을 열면, 항상 사건 1의 첫번째 단서가 보여져야 함
                    
                    ActivateUpDownButton(!isOpenedNote);
                }

                // testButton 오브젝트가 null이기 때문에, 수첩을 처음 열었을 때 사건 1,2 단서 버튼을 누르면 발생하는 NullReferenceException 에러를 해결하기 위한 if
                if (Inventory.instance.GetSlotCount() > 0)
                {
                    testButton = Inventory.instance.GetSlotObject(0); // 첫번째 슬롯을 testButton 으로써 사용한다
                }

            }

            if ((Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)) && isConversationing && !isFading)
            {
                if (canSkipConversation)
                {
                    // 미니맵 튜토리얼 중이 아닐 경우
                    if (!TutorialManager.instance.isMinimapTutorial && !TutorialManager.instance.isNoteTutorial && !TutorialManager.instance.isParchmentTutorial)
                    {
                        //텍스트가 가득 찼으면 textfull만 false로 바꾸고, 가득찬게 아니면 다음 대화 출력
                        if (DialogManager.instance.isTextFull)
                        {
                            DialogManager.instance.isTextFull = false;
                            //Debug.Log("isTextFull => false");
                        }
                        else
                        {
                            DialogManager.instance.NextSentence();
                            //Debug.Log("NextSentence() 실행중");
                        }
                    }
                }
                else
                {
                    playerWantToSkip = true;
                    //Debug.Log("스킵 눌림");
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
                    //Debug.Log("페이지 넘기는중");
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
                    //Debug.Log("페이지 넘기는중");
                    //Inventory.instance.GetSlotObject(buttonIndex).navigation = customNav;
                }
            }
        }
    }

    public bool GetIsOpenedParchment()
    {
        return isOpenedParchment;
    }

    public void ArrangeClue()
    {
        //if ((Input.GetKeyDown(KeyCode.E) && isReadParchment))
        //{
        isReadParchment = false;
        // On & Off
        isOpenedParchment = !isOpenedParchment;

        //양피지를 보이게 하기
        parchment.SetActive(isOpenedParchment);
        parchmentHelper.SetActive(isOpenedParchment);
        parchmentClueScrollList.SetActive(isOpenedParchment);

        // 메르테의 말 ( 사건 3은 44개의 단서, 전체 115개의 단서가 존재)
        float clearRate = (PlayerManager.instance.playerClueLists.Count / 115.0f) * 100;
        if (clearRate < 5.0f)
            wordOfMerte.GetComponent<Text>().text = "내가 현재 잘 하고 있는건지 잘 모르겠다. 조금만 더 열심히 해보자.";
        else if(clearRate >= 5.0f && clearRate < 21.0f)
            wordOfMerte.GetComponent<Text>().text = "좋아, 아직 초반이니까. 이정도면 많이 해낸거야. 꼭 범인을 밝혀내야만 해... 알았지? 우린 잘하고 있어.";
        else if(clearRate >= 21.0f && clearRate < 41.0f)
            wordOfMerte.GetComponent<Text>().text = "머리 속이 혼란스럽다. 나는 지금 이 일을 맡고 있는 걸 잘한걸까? 다시... 다시 해야만 해.";
        else if(clearRate >= 41.0f && clearRate < 61.0f)
            wordOfMerte.GetComponent<Text>().text = "어디서부터 잘못된건지 모르겠어... 누가 좀 알려줘... 제발... 고통스러워... 아니... 아닌가? 모르겠어 더 이상.";
        else if(clearRate >= 61.0f && clearRate < 81.0f)
            wordOfMerte.GetComponent<Text>().text = "더 이상의 미련은 없어. 이제 고지가 코 앞일테니까... 더 유능한 누군가가 대신 밝혀내주길... 이 지긋지긋한 사슬을. 그리고 끊어내줘. 그만하고 싶어.";
        else if(clearRate >= 81.0f && clearRate < 96.0f)
            wordOfMerte.GetComponent<Text>().text = "...";
        else if(clearRate >= 96.0f && clearRate <= 100.0f)
            wordOfMerte.GetComponent<Text>().text = "축하해.";

        // 만약 양피지가 닫힐 때, 화살표도 없애기
        if (!isOpenedParchment)
        {
            parchmentUpButton.SetActive(isOpenedParchment);
            parchmentDownButton.SetActive(isOpenedParchment);

            Inventory.instance.DestroySlotInParchment();
        }

        // 현재 시간대에 발견한 단서가 4개 미만이라면, 양피지를 부모로 취해 단서 리스트의 영역에 마우스 커서가 있을 때에도 양피지가 스크롤이 되게끔 만든다.
        // 튜토리얼 진행중일때도 스크롤이 되게끔 만든다.
        if (PlayerManager.instance.GetCount_ClueList_In_Certain_Timeslot() < 4 || GameManager.instance.GetPlayState() == GameManager.PlayState.Tutorial)
            parchmentClueScrollList.transform.SetParent(parchment.transform);
        else
            parchmentClueScrollList.transform.SetParent(canvasForParchment.transform);

        // 양피지에 단서 리스트 출력(중복처리해야함)
        if (GameManager.instance.GetPlayState() == GameManager.PlayState.Act)
        {
            Inventory.instance.MakeClueSlotInParchment();
        }
        //}
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
            //Debug.Log("사건" + numOfAct + "의 단서가 없어요");
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
        EffectManager.instance.Play("수첩 나오는 소리");//수첩나오는 소리
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
        EffectManager.instance.Play("오브젝트 클릭음");//오브젝트 클릭음
        conversationUI.SetActive(true);
        isConversationing = true;
    }

    // 대화창 Fade out
    public void CloseConversationUI()
    {
        conversationUI.SetActive(false);
        isConversationing = false;
    }


    public IEnumerator FadeEffectForTutorial(string position, Vector3 mainPosition, Vector3 subPosition)
    {
        isFading = true;
        TutorialManager.instance.isPlayingTutorial = true;

        /*페이드 아웃*/
        fadeInOutPanel.SetActive(true);
        fadeInOutAnimator.SetBool("isfadeout", true);
        //yield return new WaitForSeconds(1.7f);
        yield return new WaitForSeconds(2.0f);

        /*이동*/
        PlayerManager.instance.SetCurrentPosition(position);
        PlayerManager.instance.SetPlayerPosition(mainPosition);

        if (TutorialManager.instance.tutorial_Index == 901)
            TutorialManager.instance.SetAssistantPosition(subPosition);
        else if (TutorialManager.instance.tutorial_Index == 918)
        {
            TutorialManager.instance.SetZaralPosition(subPosition);
            TutorialManager.instance.SetSpriteCharacterFor918();
        }
        else if (TutorialManager.instance.tutorial_Index == 919)
        {
            TutorialManager.instance.SetZaralPosition(subPosition);
            TutorialManager.instance.SetSpriteCharacterFor919();
        }
        else if (TutorialManager.instance.tutorial_Index == 920)
        {
            TutorialManager.instance.SetActiveFalse_Tutorial_Objects();
        }

        /*플레이어의 위치에 따른 BGM변경*/
        BGMManager.instance.AutoSelectBGM();

        /* 패널 페이드 인*/
        fadeInOutAnimator.SetBool("isfadeout", false);
        yield return new WaitForSeconds(2.5f);
        isFading = false;

        TutorialManager.instance.isFading = false;
    }

    // 시간대 변경을 위한 Fade In & Out
    public IEnumerator FadeEffectForChangeTimeSlot()
    {
        isFading = true;

        // 시간대가 변경되는 동안 캐릭터가 행동 못하게 해야함
        // 1. FadeIn 된다.
        /*페이드 아웃*/
        fadeInOutPanel.SetActive(true);
        fadeInOutAnimator.SetBool("isfadeout", true);
        yield return new WaitForSeconds(1.7f);

        /* 시간대가 지났다는 텍스트 띄우기 */
        // PlayState가 Tutorial 일 경우 "세 번째 연쇄 살인" 문구를 출력한다.
        // PlayState가 Act일 경우 "~시간대가 지났다" 문구 출력한다
        timeSlotText.SetActive(true);

        if (GameManager.instance.GetPlayState() == GameManager.PlayState.Tutorial)
        {
            timeSlotText.GetComponent<Text>().text = "세 번째 연쇄 살인";

            yield return new WaitForSeconds(0.7f);

        }
        else if (GameManager.instance.GetPlayState() == GameManager.PlayState.Act)
        {
            string timeslot = PlayerManager.instance.TimeSlot;

            if (PlayerManager.instance.NumOfAct.Equals("53"))
                timeSlotText.GetComponent<Text>().text = "사건 발생으로부터 " + timeslot[1] + "주가 지나갔다";
            else if (PlayerManager.instance.NumOfAct.Equals("54"))
            {
                timeSlotText.GetComponent<Text>().text = "사건 발생으로부터 " + (int.Parse(timeslot) - 74) + "일이 지나갔다";
            }// if-else

            // 이벤트를 적용시킬 것이 있는지 확인 후, 적용
            EventManager.instance.PlayEvent();

            //Debug.Log("시간대 넘기는중");

            yield return new WaitForSeconds(0.7f);

            // 시간대 바꾸기
            if (PlayerManager.instance.NumOfAct.Equals("53"))
            {
                if (PlayerManager.instance.TimeSlot.Equals("71"))
                    PlayerManager.instance.TimeSlot = "72";
                else if (PlayerManager.instance.TimeSlot.Equals("72"))
                    PlayerManager.instance.TimeSlot = "73";
                else if (PlayerManager.instance.TimeSlot.Equals("73"))
                    PlayerManager.instance.TimeSlot = "74";
                else if (PlayerManager.instance.TimeSlot.Equals("74"))
                {
                    PlayerManager.instance.NumOfAct = "54";
                    SetNameOfCase("사건4 연쇄살인 4번째 피해자_륑 에고이스모");
                    EventManager.instance.AbleNpcForEvent(36);
                }
            }// if

            if (PlayerManager.instance.NumOfAct.Equals("54"))
            {
                if (!act4Button.activeSelf)
                    act4Button.SetActive(true);

                if (PlayerManager.instance.TimeSlot.Equals("74"))
                    PlayerManager.instance.TimeSlot = "75";
                else if (PlayerManager.instance.TimeSlot.Equals("75"))
                    PlayerManager.instance.TimeSlot = "76";
                else if (PlayerManager.instance.TimeSlot.Equals("76"))
                    PlayerManager.instance.TimeSlot = "77";
                else if (PlayerManager.instance.TimeSlot.Equals("77"))
                    PlayerManager.instance.TimeSlot = "78";
                else if (PlayerManager.instance.TimeSlot.Equals("78"))
                    PlayerManager.instance.TimeSlot = "79";
                else if (PlayerManager.instance.TimeSlot.Equals("79"))
                {
                    PlayerManager.instance.NumOfAct = "55";
                    PlayerManager.instance.TimeSlot = "79";

                    if (!act5Button.activeSelf)
                        act5Button.SetActive(true);
                }// if-else
            }// if

            // 세이브
            GameManager.instance.thread = new Thread(GameManager.instance.SaveGameData);
            GameManager.instance.thread.IsBackground = true;
            GameManager.instance.thread.Start();

        }// if-else


        // 디버깅용
        PlayerManager.instance.checkNumOfAct = PlayerManager.instance.NumOfAct;
        PlayerManager.instance.checkTimeSlot = PlayerManager.instance.TimeSlot;

        // 3. 문구와 같이 Fade Out 된다.
        /* 문구 페이드 인 */
        Color tempColor1;
        tempColor1 = timeSlotText.GetComponent<Text>().color;

        while (tempColor1.a > 0f)
        {
            tempColor1.a -= Time.deltaTime / 2.1f;
            timeSlotText.GetComponent<Text>().color = tempColor1;

            if (tempColor1.a <= 0f)
            {
                tempColor1.a = 0f;
            }
            yield return null;
        }

        timeSlotText.GetComponent<Text>().color = tempColor1;
        isFading = false;
        timeSlotText.SetActive(false);

        /* 패널 페이드 인*/
        fadeInOutAnimator.SetBool("isfadeout", false);
        yield return new WaitForSeconds(2.5f);

        tempColor1 = timeSlotText.GetComponent<Text>().color;
        tempColor1.a = 1f;
        timeSlotText.GetComponent<Text>().color = tempColor1;

        if (GameManager.instance.GetPlayState() == GameManager.PlayState.Tutorial)
        {
            act3Button.SetActive(true);
            TutorialManager.instance.EndTutorial();
        }
    }

    // 301번 이벤트를 위한 Fade In & Out
    public IEnumerator FadeInForPlaying915()
    {
        // 1. FadeIn 된다.
        fadeInOutPanel.SetActive(true);
        fadeInOutAnimator.SetBool("isfadeout3", true);

        isFading = true;
        isConversationing = true;
        TutorialManager.instance.isPlayingTutorial = true;

        yield return new WaitForSeconds(1.7f);

        yield return new WaitForSeconds(0.7f);

        StartCoroutine(TutorialManager.instance.TypingFor915());
    }

    public IEnumerator FadeOutForPlaying915()
    {

        // 2. Fade Out 된다.
        fadeInOutAnimator.SetBool("isfadeout3", false);

        yield return new WaitForSeconds(2.0f);

        isConversationing = false;
        isFading = false;

        MiniMapManager.instance.SetIsInsideNow(true);       // 메르테의 사무실에 있으므로, isInsideNow 값을 true로 변경
        TutorialManager.instance.IncreaseTutorial_Index();

        TutorialManager.instance.InvokeTutorial();
    }

    // 301번 이벤트를 위한 Fade In & Out
    public IEnumerator FadeEffectForEvent301()
    {
        // 1. FadeIn 된다.
        fadeInOutPanel.SetActive(true);
        fadeInOutAnimator.SetBool("isfadeout", true);

        isFading = true;
        isConversationing = true;
        yield return new WaitForSeconds(1.7f);

        yield return new WaitForSeconds(0.7f);

        // 2. Fade Out 된다.
        fadeInOutAnimator.SetBool("isfadeout", false);

        PlayerManager.instance.ChangeStateFor301Event_Fisrt();
        PlayerManager.instance.SetPlayerPosition(new Vector3(15850.0f, 4100.0f, 0));
        PlayerManager.instance.SetCurrentPosition("Mansion_Viscount_Mansion_Second_Floor");
        PlayerManager.instance.AddEventCodeToList("301");

        yield return new WaitForSeconds(2.5f);

        isFading = false;

        // 302번 이벤트에 해당하는 대사묶음이 작성될 경우 수정 (0313)
        DialogManager.instance.InteractionWithObject("302");
    }

    // 302번 이벤트를 위한 Fade In 코루틴
    public IEnumerator FadeInForEvent302()
    {
        isFading = true;

        fadeInOutPanel.SetActive(true);
        fadeInOutAnimator.SetBool("isfadeout2", true);
        yield return new WaitForSeconds(2.4f);

        PlayerManager.instance.ChangeStateFor301Event_Second();
        PlayerManager.instance.SetPlayerPosition(new Vector3(11419.0f, 5220.0f, 0));
        PlayerManager.instance.SetCurrentPosition("Chapter_Merte_Office");
        PlayerManager.instance.AddEventCodeToList("302");

        isFading = false;

        yield return null;
    }

    // 302번 이벤트를 위한 Fade Out 코루틴
    public IEnumerator FadeOutForEvent302()
    {
        fadeInOutAnimator.SetBool("isfadeout2", false);

        yield return new WaitForSeconds(2.5f);

        //CloseConversationUI();
        isConversationing = false;
        isFading = false;
        EventManager.instance.isPlaying302Event = false;
        yield return null;
    }

    // 1. 대화창 & 캐릭터명 창 fade in
    // 2. 캐릭터 이미지 & 캐릭터 이름 fade in
    // 3. 대화 출력(\n 을 csv에서 어떻게 받아올 수 있는지 고민해봐야함)
    public IEnumerator FadeEffect(float fadeTime, string fadeWhat)
    {
        isFading = true;

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
            isFading = false;
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
            isFading = false;
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

    public void ActivateDeadBodyImage()
    {
        if (!deadBodyImage.activeSelf)
            deadBodyImage.SetActive(true);
    }

    public void RemoveDeadBodyImage()
    {
        if (deadBodyImage.activeSelf)
            deadBodyImage.SetActive(false);
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
    
    /*일시정지 관련*/
    public bool GetIsPaused()
    {
        return ispaused;
    }

    public void SetIsPausedTrue()
    {
        ispaused = true;
    }

    public void SetIsPausedFalse()
    {
        ispaused = false;
    }

    public void SetNameOfCase(string textOfAct4)
    {
        nameOfCase.GetComponent<Text>().text = textOfAct4;
    }

    public bool IsBookOpened()
    {
        if (isOpenedNote == true)
            return true;
        else
            return false;
    }

    public void SetNegativeIsOpenedNote()
    {
        isOpened = !isOpened;
        isOpenedNote = !isOpenedNote;
    }

    public void SetDocumentControll(bool isOpen)
    {
        DocumentOfAndren.SetActive(isOpen);
    }

    public void SetDocumentCover(bool isOpen)
    {
        documentCover.SetActive(isOpen);
    }

    public void SetActivePortrait(bool boolValue)
    {
        characterFrame.SetActive(boolValue);
        characterBackfroundImage.SetActive(boolValue);
        characterImage.SetActive(boolValue);
    }
}
