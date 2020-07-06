using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingManager : MonoBehaviour
{
    public static EndingManager instance = null;

    // 각 엔딩에서 사용할 오브젝트들만 저장한 후, 각 장면에 인덱스를 부여하여 알맞은 오브젝트 활성화
    [SerializeField]    private GameObject[] valuaEndingObjects;
    [SerializeField]    private GameObject[] arnoldEndingObjects;
    [SerializeField]    private GameObject[] andrenEndingObjects;
    [SerializeField]    private GameObject[] trueEndingObjects;
    
    // 대화창 관련 UI
    [SerializeField]    private GameObject  conversationUI;  //대화창 전체 UI
    [SerializeField]    private GameObject  characterNameBg; //대화 캐릭터명 창
    [SerializeField]    private GameObject  conversationBg;  //대화창 배경
    [SerializeField]    private Text        conversationText;
    [SerializeField]    private Text        npcNameText;
    [SerializeField]    private Image       npcImage;

    // 대화창 초상화
    [SerializeField]    private GameObject  characterFrame;
    [SerializeField]    private GameObject  characterBackgroundImage;
    [SerializeField]    private GameObject  characterImage;

    [SerializeField]    private GameObject  FadeInOutPanel;
    [SerializeField]    private GameObject  FadeInOutPanelForClue; // 쓸모 있는 패널인가? 확인 필요

    private string[] sentences;
    private int     index;
    public  float   typingSpeed;        // 타이핑스피드 - 자막재생속도
    private int     numOfText = 0;      // 현재 출력된 텍스트 수
    private int     numOfTextLimit = 0; // 대화창의 한 줄에 쓰여진 텍스트 수
    private int     textLimit;          // 한 대화창에 출력할 최대 텍스트 수
    private int     tempLimitInOneLine; // 대화 한 줄에 쓰여질 수 있는 텍스트 수
    public  bool    isTextFull;         // 한 대화창에 출력할 최대 텍스트에 도달했는지의 여부
    public  bool    isSentenceDone;     // 출력할 문장이 다 출력 됐는지 여부
    public  bool    atOnce;             // 한번에 한 단어만 출력
    public  bool    isConversationing;
    public  bool    canSkipConversation;
    public  bool    playerWantToSkip;
    public  bool    isFading;
    public  bool    isTypingText;
    public  int     setOfDesc_Index;    // 대화묶음 리스트의 인덱스

    private Dictionary<int, Dictionary<string, string>> dataList;
    private List<Interaction>   interactionLists;
    private List<string>        imagePathLists;         //npcFrom에 해당하는 npc들의 이미지의 경로 리스트
    private List<string>        tempNpcNameLists;       //npc 이름 불러오는용
    private int                 numNpcNameLists;        //대화에 연관된 npc가 몇명인지 저장용
    private int                 curNumOfNpcNameLists;   //현재 대화중인 npc 이름의 index
    private List<string>        sentenceLists;          //출력해야 할 대화들을 담을 리스트
    private List<string>        startObjectLists;       // 대화묶음의 갯수 파악 및 엔딩 대화 자동 시작을 위한 리스트
    private List<int>           setActiveTrueLists;     // 각 대화마다 사건의 값을 담고있는 리스트 (오브젝트 활성화 비활성화에 사용)

    private NpcParser npcParser;
    private string tempNpcName;
    private string tempSentenceOfCondition;

    [SerializeField]
    private bool isFirstConversation;    //대화 묶음의 첫 대사인지 확인하는 변수(Fade in 관련)
    private int enterLimitCount;         //줄바꿈 수를 제한하기 위한 변수
    private string tempObjectPortrait;

    private bool ispaused;

    void Awake()
    {
        if (GameManager.instance.GetEndingState() == GameManager.EndingState.True)
        {
            CSVParser.instance.InitDataFromCSV();   // 대화 & 단서 테이블 파싱 -> 진엔딩 후의 작업에 사용
        }

        // 엔딩 테이블 파일 로딩
        CSVParser.instance.LoadingEndingDataFromCSV(); // CSVParser의 endingDataList, endingInteractionLists 에 테이블 내용들이 저장됨
    }

    void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        textLimit = 125;
        enterLimitCount = 3;    //줄바꿈이 3번 일어나면 대화출력 종료 -> 대화창엔 3줄까지 출력될 것임
        tempLimitInOneLine = 30;    // 띄어쓰기 + 글자가 30자 이상일 경우, 강제로 줄바꿈
        isTextFull = false;
        isSentenceDone = false;
        atOnce = false;
        isConversationing = false;
        canSkipConversation = false;
        playerWantToSkip = false;
        isFading = false;
        isTypingText = false;
        index = 0;
        setOfDesc_Index = 0;
        numNpcNameLists = 0;
        curNumOfNpcNameLists = 0;
        sentences = new string[] { "" };
        ispaused = false;

        dataList = GameObject.Find("DataManager").GetComponent<CSVParser>().GetEndingDataList();
        interactionLists = GameObject.Find("DataManager").GetComponent<CSVParser>().GetEndingInteractionLists();
        startObjectLists = GameObject.Find("DataManager").GetComponent<CSVParser>().GetEndingStartObjectLists();

        imagePathLists = new List<string>(); //캐릭터 이미지 추가되면 적용(테스트) 해야함
        tempNpcNameLists = new List<string>();
        sentenceLists = new List<string>();
        setActiveTrueLists = new List<int>();

        npcParser = new NpcParser();

        SetAlphaToZero_ConversationUI();    //대화창 UI 투명화

        isFirstConversation = false;

        FadeInOutPanel.SetActive(false);
        FadeInOutPanelForClue.SetActive(false);

        /*혹시 몰라서 설정 한번 더 (다이얼로그 속도) 적용...*/
        SettingManager.instance.SetCurSetting();

        // (추후 이렇게 진행할 예정)
        // GameManager 에서 선택된 용의자를 용의자 변수에 저장하고, 해당 용의자에 따라서 endingState를 변경한다.
        // endingState 값에 따라서 엔딩을 시작한다.
        // EndingStart 함수를  호출한다.
        EndingStart(); // endingState 값에 따른 엔딩 시작
    }
    
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)) && isConversationing && !isFading)
        {
            if (canSkipConversation)
            {
                //텍스트가 가득 찼으면 textfull만 false로 바꾸고, 가득찬게 아니면 다음 대화 출력
                if (isTextFull)
                {
                    isTextFull = false;
                    //Debug.Log("isTextFull => false");
                }
                else
                {
                    NextSentence();
                    //Debug.Log("NextSentence() 실행중");
                }
            }
            else
            {
                playerWantToSkip = true;
                //Debug.Log("스킵 눌림");
            }
        }

        if (GameManager.instance.GetPlayState() == GameManager.PlayState.Ending)
        {
            // 대화가 텍스트창에 한계치만큼 가득 찼거나, 한 대화가 모두 출력됐을때, 다음 대화로 넘어갈 수 있게 해줌
            if ((numOfText > textLimit) || (isConversationing && isSentenceDone))
            {
                canSkipConversation = true;
                isSentenceDone = false;
            }
        }
    }

    public void EndingStart()
    {
        switch (GameManager.instance.GetEndingState())
        {
            case GameManager.EndingState.Valua:     // 발루아 엔딩
            case GameManager.EndingState.Arnold:    // 아놀드 엔딩
            case GameManager.EndingState.Andren:    // 안드렌 엔딩
            case GameManager.EndingState.True:      // 진엔딩
                StartEnding();
                break;

            default:
                Debug.Log("(endingState = " + GameManager.instance.GetEndingState() + "), 잘못된 엔딩 코드입니다.");
                return;
        }
    }

    // 같은 대화묶음을 진행할지라도, 다른 사건 값이 있을 때가 있다. 그럴 때, 이미지의 전환을 어떻게 할 것인지 생각해보자.
    // 한 대화묶음의 대화를 하나씩 진행할 때마다, 그 전에 ChangeObject 함수의 인자로 사건 값을 넘겨서 이미지를 바꿔준다??? -> 괜찮은듯?

    // 1. 하나의 대화묶음을 진행할 때 마다, 각 테이블의 사건에 쓰여진 int 값에 맞는 object 배열의 element들이 활성화 되고, 나머지는 비활성화 되어야 한다.
    // 2. 대화의 시작은 startObject 값을 인자로, InteractionWithObject 함수를 이용한다.
    // 3. 대화창은 스킵이 가능하도록 구현해보자.

    // 엔딩 시작
    public void StartEnding()
    {
        
        InteractionWithObject(startObjectLists[setOfDesc_Index++]);
    }

    // 엔딩 대화 테이블의 '사건'에 해당하는 항목 값에 따라서, 오브젝트를 활성화 or 비활성화 시키는 함수를 제작
    public void ChangeObject(int objectIndex)
    {
        switch (GameManager.instance.GetEndingState())
        {
            case GameManager.EndingState.True:
                ChangeTrueEndingObject(objectIndex);
                break;
            case GameManager.EndingState.Andren:
                ChangeAndrenEndingObject(objectIndex);
                break;
            case GameManager.EndingState.Arnold:
                ChangeArnoldEndingObject(objectIndex);
                break;
            case GameManager.EndingState.Valua:
                ChangeValuaEndingObject(objectIndex);
                break;
            default:
                Debug.Log("GetEndingState() 오류 : " + GameManager.instance.GetEndingState());
                break;
        }
    }

    // 일단 모두 비활성화하고, 파라미터로 넘어온 index에 해당하는 오브젝트만 활성화시킨다.
    public void ChangeTrueEndingObject(int index)
    {
        // 진엔딩일 경우, 6 이상의 값을 가지면, 5번 오브젝트(5_background)는 항상 활성화한다.
        // 단, 11의 값을 가질때를 제외한다. (검은화면)

        // 일단 모두 비활성화
        for (int i = 0; i < trueEndingObjects.Length; i++)
        {
            trueEndingObjects[i].SetActive(false);
        }

        // 해당 objectIndex의 object만 활성화
        trueEndingObjects[index].SetActive(true);

        // 진엔딩일 경우, 6 이상의 값을 가지면, 5번 오브젝트(5_background)는 항상 활성화한다.
        if (index >= 6 && index != 11)
        {
            trueEndingObjects[5].SetActive(true);
        }
    }

    public void ChangeAndrenEndingObject(int index)
    {
        // 안드렌 엔딩일 경우, 9 이상의 값을 가지면, 8번 오브젝트(background)를 항상 활성화 시킨다.
        // 2 이하의 값을 가지면, 0번 오브젝트를 항상 활성화 시킨다.
        // 단, 13의 값을 가질때를 제외한다. (검은화면)

        for (int i = 0; i < andrenEndingObjects.Length; i++)
        {
            andrenEndingObjects[i].SetActive(false);
        }

        andrenEndingObjects[index].SetActive(true);

        if (index <= 2)
        {
            andrenEndingObjects[0].SetActive(true);
        }

        if (9 <= index && index < 11 && index != 13)
        {
            andrenEndingObjects[8].SetActive(true);
        }
    }

    public void ChangeArnoldEndingObject(int index)
    {
        // 사건 값이 1 이하이면, 항상 0번 오브젝트를 활성화

        for (int i = 0; i < arnoldEndingObjects.Length; i++)
        {
            arnoldEndingObjects[i].SetActive(false);
        }

        arnoldEndingObjects[index].SetActive(true);

        if (index <= 1)
        {
            arnoldEndingObjects[0].SetActive(true);
        }
    }

    public void ChangeValuaEndingObject(int index)
    {
        // 사건 값이 1보다 작거나 같으면, 항상 0번 오브젝트를 활성화

        for (int i = 0; i < valuaEndingObjects.Length; i++)
        {
            valuaEndingObjects[i].SetActive(false);
        }

        valuaEndingObjects[index].SetActive(true);

        if (index <= 1)
        {
            valuaEndingObjects[0].SetActive(true);
        }
    }

    // 대화 테이블 관련 정보 최신화
    public void SetLists()
    {
        dataList = GameObject.Find("DataManager").GetComponent<CSVParser>().GetEndingDataList();
        interactionLists = GameObject.Find("DataManager").GetComponent<CSVParser>().GetEndingInteractionLists();
    }
    

    public void InteractionWithObject(string objectName)
    {
        /* 확장성을 위해 objectName을 파라미터로 받아서, 해당 물체를 조사하는 상호작용 구현하기 */
        string targetObject = objectName;   //StartObject에 해당하는 값
        tempSentenceOfCondition = targetObject;
        tempObjectPortrait = targetObject;
        
        //targetObject에 해당하는 npc의 이름을 가진 클래스의 index 알아오기
        //int indexOfInteraction = interactionLists.FindIndex(x => x.GetStartObject() == targetObject);
        
        List<Interaction> targetOfInteractionList = new List<Interaction>();

        // StartObject인 대화만 고르기
        targetOfInteractionList = interactionLists.FindAll(x => (x.CheckStartObject(targetObject) == true));

        //Debug.Log("시작오브젝트 = " + targetOfInteractionList[0].GetStartObject() + ", npcFrom = " + targetOfInteractionList[0].GetNpcFrom() + ", desc = " + targetOfInteractionList[0].GetDesc());

        // 해당 NPC와의 대화가 없을 경우, 함수 종료
        if (targetOfInteractionList.Count == 0)
        {
            return;
        }
        

        //대화묶음의 번호 값
        int tempSetOfDesc_Index;
        //대화 묶음 안의 첫 대사 id
        int tempId;
        
        List<Interaction> setOfDescList = new List<Interaction>();
        
        setOfDescList = targetOfInteractionList;

        // 해당 NPC와의 대화가 없을 경우, 함수 종료 (1210에 update 함) -> 이부분은 앞에서 실행하는 부분이라 필요없다고 판단함 (1223)
        if (setOfDescList.Count == 0)
        {
            return;
        }

        // 대화 묶음 번호를 저정하는 리스트
        List<int> setOfDescIndexList = new List<int>();

        // 모든 대사를 토대로, 어떤 대화 묶음이 존재하는지 확인하는 for문
        for (int i = 0; i < setOfDescList.Count; i++)
        {
            int tempsetOfDescIndex = setOfDescList[i].GetSetOfDesc();
            //setOfDescIndexList 안에 해당 대화묶음 번호가 없으면 추가.
            if (!setOfDescIndexList.Contains(tempsetOfDescIndex))
                setOfDescIndexList.Add(tempsetOfDescIndex);
        }

        // 여러개의 대화묶음 번호중에 하나를 채택하기
        int randomValue = Random.Range(0, setOfDescIndexList.Count);
        //Debug.Log("Random value = " + randomValue);
        tempSetOfDesc_Index = setOfDescIndexList[randomValue];
        

        isConversationing = true;    // 대화중
        OpenConversationUI();        // 대화창 오픈
        isFading = true;
        StartCoroutine(FadeEffect(0.5f, "In"));  //0.5초 동안 fade in


        /* 구해진 tempSetOfDesc_Index 에 해당하는 대화묶음에 해당하는 대사들을 id를 통해서 호출하기를 구현 */
        int indexOfInteraction = interactionLists.FindIndex(x => x.GetSetOfDesc() == tempSetOfDesc_Index);
        tempId = int.Parse((dataList[indexOfInteraction])["id"]);

        //대화가 이어질 수 있도록 parent값을 이용
        int tempParentIndex = interactionLists.FindIndex(x => x.GetSetOfDesc() == tempSetOfDesc_Index && x.GetId() == tempId);
        int tempParent = interactionLists[tempParentIndex].GetParent();
        //Debug.Log("대화묶음 " + tempSetOfDesc_Index + "의 초기의 tempParent : " + tempParent);
        
        /* Id와 parent값이 같다는 것은 더이상 관련있는 대화가 없다는 것이다. */
        /* 처음에는 tempId와 tempParent가 같던 다르던 일단 읽음 */

        //tempParentIndex는 특정 대화묶음과 그 묶음 안의 대사의 id를 갖는 특정 대사의 위치Index를 가지고 있는 변수임
        //따라서 tempParentIndex를 이용하면 다시 FindIndex를 쓸 필요 없음.
        //Debug.Log("CheckAndAddSentence 함수 이전의 tempParentindex = " + tempParentIndex);
        //해당 tempId에 맞는 대화로 인해 얻을 수 있는 정보를 얻는 함수
        tempNpcNameLists.Add((dataList[tempParentIndex])["npcFrom"]);    //대화중인 npc이름 변경용
        sentenceLists.Add((dataList[tempParentIndex])["desc"]);  //해당 id값의 대화 추가
        setActiveTrueLists.Add(int.Parse((dataList[tempParentIndex])["사건"]));   // 사건 값 추가 (오브젝트 활성 & 비활성화에 사용)

        //Debug.Log("tempId = " + tempId + ", tempParent = " + tempParent);
        /* 현재의 tempId와 tempParent가 다르다면(연결된 대화가 있다면) 진행 */
        while (tempId != tempParent)
        {
            tempId = tempParent; //다음 대화를 위해 index에 tempParent(다음 연관된 대화와 관련된 id값)값을 넣음
                                 //대화가 이어질 수 있도록 parent값을 이용
            tempParentIndex = interactionLists.FindIndex(x => x.GetSetOfDesc() == tempSetOfDesc_Index && x.GetId() == tempId);

            //Debug.Log("tempParentIndex = " + tempParentIndex);
            tempParent = interactionLists[tempParentIndex].GetParent();

            tempNpcNameLists.Add((dataList[tempParentIndex])["npcFrom"]);    //대화중인 npc이름 변경용
            sentenceLists.Add((dataList[tempParentIndex])["desc"]);  //해당 id값의 대화 추가
            setActiveTrueLists.Add(int.Parse((dataList[tempParentIndex])["사건"]));   // 각 대화마다 활성화 해야할 오브젝트 인덱스(사건 값)를 저장해둠
        }

        sentences = sentenceLists.ToArray();
        numNpcNameLists = tempNpcNameLists.Count;

        /*
        Debug.Log("npc 수 : " + numNpcNameLists);
        for (int i = 0; i < tempNpcNameLists.Count; i++)
        {
            Debug.Log("npc " + (i + 1) + " : " + tempNpcNameLists[i]);
        }
        for (int i = 0; i < sentences.Length; i++)
        {
            Debug.Log("sentence" + (i + 1) + " : " + sentences[i]);
        }
        */

        // while문을 빠져나오면 sentenceLists에 대화목록이 쭉 저장되어있을 것이다.

        if (!isTypingText)
        {
            StartCoroutine(Type()); // 첫 대화 출력
        }

    }

    // 알맞은 대화를 출력해주는 코루틴
    IEnumerator Type()
    {
        isTypingText = true;
        //대화 할 때 마다 대화중인 캐릭터 이름 변경
        /* tempNpcNameLists[curNumOfNpcNameLists]을 이용하여 고유한 character code 마다 이름으로 바꿔줘야함 */
        //Debug.Log("curNumOfNpcNameLists = " + curNumOfNpcNameLists);

        // 엔딩 테이블의 사건 값에 따라서, 이미지 오브젝트들을 활성화 or 비활성화 해주자
        ChangeObject(setActiveTrueLists[index]);

        if (tempSentenceOfCondition.Equals("arnold_4"))
        {
            if (index == 0)
            {
                // "아놀드 엔딩_감옥" bgm 시작
                BGMManager.instance.PlayBGM(22);
            }
        }

        if (tempSentenceOfCondition.Equals("andren_0"))
        {
            if (index == 0)
            {
                // 모든 배경음 stop
                BGMManager.instance.StopBGM();
            }
        }

        if (tempSentenceOfCondition.Equals("andren_1") || tempSentenceOfCondition.Equals("arnold_0") || tempSentenceOfCondition.Equals("valua_0"))
        {
            if (index == 1)
            {
                // "일반 엔딩" bgm 시작
                BGMManager.instance.PlayBGM(20);
            }
        }

        if (tempSentenceOfCondition.Equals("andren_10"))
        {
            if (index == 0)
            {
                // "진엔딩_신문실" bgm 시작
                BGMManager.instance.PlayBGM(23);
            }
        }

        if (tempSentenceOfCondition.Equals("andren_12") || tempSentenceOfCondition.Equals("arnold_5") || tempSentenceOfCondition.Equals("valua_8"))
        {
            if (index == 0)
            {
                // 모든 배경음 stop
                BGMManager.instance.StopBGM();
            }
        }

        if (tempSentenceOfCondition.Equals("true_0"))
        {
            if (index == 0 || index == 30)
            {
                // 모든 배경음 stop
                BGMManager.instance.StopBGM();
            }

            if (index == 21)
            {
                // "신문실_심장소리" 시작
                EffectManager.instance.Play("심문실 심장소리");
            }
        }

        if (tempSentenceOfCondition.Equals("true_1"))
        {
            if (index == 0)
            {
                // "진엔딩" bgm 시작
                BGMManager.instance.PlayBGM(21);
            }
        }

        if (tempSentenceOfCondition.Equals("true_6"))
        {
            if (index == 3)
            {
                // 진엔딩_총소리 시작
                EffectManager.instance.Play("진엔딩 총소리");
            }
        }

        if (tempSentenceOfCondition.Equals("true_7"))
        {
            if (index == 0)
            {
                // 모든 배경음 stop
                BGMManager.instance.StopBGM();
            }
        }

        string tempObjectCode = tempNpcNameLists[curNumOfNpcNameLists];

        tempNpcName = npcParser.GetNpcNameFromCode(tempObjectCode);

        isConversationing = true;

        if (tempNpcName != null)
        {
            npcNameText.text = tempNpcName;
            string objectName = npcParser.GetObjectNameFromCode(tempObjectCode);
            bool checkObject = false;
            try
            {
                if (objectName != null)
                    checkObject = true;
            }
            catch { };

            if (checkObject || tempNpcName.Equals("서술자") || tempNpcName.Equals("시스템") || tempNpcName.Equals("감시자"))
            {
                // 상호작용하는 오브젝트가 사물이라면, 초상화 비활성화
                SetActivePortrait(false);

                if (tempNpcName.Equals("서술자") || tempNpcName.Equals("시스템") || tempNpcName.Equals("감시자"))
                    npcNameText.text = tempNpcName;
                else
                    npcNameText.text = objectName;
            }
            else
            {
                // 상호작용하는 오브젝트가 사물이 아니라면, 초상화 활성화
                SetActivePortrait(true);

                npcImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/PortraitOfCharacter/" + tempNpcName + "_초상화");
            }

            // 사물로부터 시작한 대화에 사람이 껴있는 경우를 위해
            tempObjectPortrait = tempNpcName;
        }
        else
            npcNameText.text = "???";

        if (tempNpcNameLists.Count > 1) curNumOfNpcNameLists++;

        conversationText.text = "";
        numOfText = 0;
        numOfTextLimit = 0;
        isSentenceDone = false;

        if (!isFirstConversation)
        {
            yield return new WaitForSeconds(1.0f);  // 대화창 Fade in이 다 될때 까지 대기
            isFirstConversation = true;
        }

        int tempEnterCount = 0;     // \n의 수를 체크할 변수 선언

        // 한 문장의 한 단어씩 출력하는 반복문
        foreach (char letter in sentences[index].ToCharArray())
        {
            //출력된 텍스트 수가 최대 텍스트 수보다 작은 경우 -> 정상출력
            if (numOfText <= textLimit && !atOnce)
            {
                if (letter.Equals('\n'))
                {
                    tempEnterCount++;
                    numOfTextLimit = 0;
                }

                // 125자가 출력되었거나, 개행문자 \n가 3번 출력되었을 경우 대화 출력 제어
                if (numOfText == textLimit || tempEnterCount == enterLimitCount)
                {
                    isTextFull = true;
                }

                // 플레이어가 대화를 스킵하고자 할 경우, for문 탈출
                if (playerWantToSkip)
                {
                    break;
                }

                conversationText.text += letter;
                atOnce = true;
                numOfText++;
                numOfTextLimit++;
                canSkipConversation = false;

                //글자 타이핑 소리?

                // 자동으로 한줄 띄우기
                if (numOfTextLimit >= tempLimitInOneLine)
                {
                    conversationText.text += '\n';

                    tempEnterCount++;
                    numOfTextLimit = 0;

                    // 125자가 출력되었거나, 개행문자 \n가 3번 출력되었을 경우 대화 출력 제어
                    if (numOfText == textLimit || tempEnterCount == enterLimitCount)
                    {
                        isTextFull = true;
                    }
                }

                // 125자 이상 출력되었거나, 개행문자 \n가 3번 출력되었을 경우 대화 출력 제어
                if (numOfText > textLimit || tempEnterCount == enterLimitCount)
                {
                    canSkipConversation = true;

                    yield return new WaitUntil(() => !isTextFull);  //isTextFull이 false가 될때까지 기다린다. (마우스 왼쪽 클릭 -> isTextFull = false)

                    conversationText.text = "";
                    numOfText = 0;
                    tempEnterCount = 0;
                    numOfTextLimit = 0;

                    atOnce = false;
                    playerWantToSkip = false;
                }
                else
                {
                    yield return new WaitForSeconds(typingSpeed);
                    atOnce = false;
                }
            }
        }

        // 대화가 스킵됐을 때의 로직
        if (playerWantToSkip)
        {
            string tempString = "";
            tempEnterCount = 0;
            numOfText = 0;
            numOfTextLimit = 0;

            foreach (char letter in sentences[index].ToCharArray())
            {
                if (letter.Equals('\n'))
                {
                    tempEnterCount++;
                    numOfTextLimit = 0;
                }

                tempString += letter;

                numOfText++;
                numOfTextLimit++;

                // 자동으로 한줄 띄우기
                if (numOfTextLimit >= tempLimitInOneLine)
                {
                    tempString += '\n';
                    tempEnterCount++;
                    numOfTextLimit = 0;

                    // 125자가 출력되었거나, 개행문자 \n가 3번 출력되었을 경우 대화 출력 제어
                    if (numOfText == textLimit || tempEnterCount == enterLimitCount)
                    {
                        isTextFull = true;
                    }
                }

                // 125자 이상 출력되었거나, 개행문자 \n가 3번 출력되었을 경우 대화 출력 제어
                if (numOfText > textLimit || tempEnterCount == enterLimitCount)
                {
                    conversationText.text = tempString;

                    canSkipConversation = true;
                    yield return new WaitUntil(() => !isTextFull);  //isTextFull이 false가 될때까지 기다린다. (마우스 왼쪽 클릭 -> isTextFull = false)

                    conversationText.text = "";
                    tempString = "";
                    numOfText = 0;
                    tempEnterCount = 0;
                    numOfTextLimit = 0;
                    playerWantToSkip = false;
                }

                conversationText.text = tempString;

                playerWantToSkip = false;
            }
        }

        canSkipConversation = true;

        isSentenceDone = true;

        isTypingText = false;
    }

    // 대화 스킵 함수
    public void SkipConversation()
    {
        canSkipConversation = true;
        isConversationing = true;
        //isTextFull = true;
        isTypingText = false;

        conversationText.text = sentences[index];
    }

    public void NextSentence()
    {
        canSkipConversation = false;
        isConversationing = true;

        try
        {
            conversationText.text = "";

            if (index < sentences.Length - 1)
            {
                index++;
                StartCoroutine(Type());
            }
            else
            {
                isFading = true;
                StartCoroutine(FadeEffect(0.2f, "Out"));  //?초 동안 fade out 후 대화창 닫기

                ResetVariables();

                // 0.5초 후 다음 엔딩 대화 실행
                if (startObjectLists.Count >= setOfDesc_Index + 1)
                {
                    StartCoroutine(AutoStartConversation());
                }
                else
                {
                    // 진엔딩이면 BW_H 사무실 씬으로, 아니면 타이틀 씬으로 비동기 로딩
                    StartCoroutine(LoadAsyncSceneForEnding());
                }
                
                //else if (GameManager.instance.GetEndingState() != GameManager.EndingState.True)
                //{
                //    // 엔딩 대화가 모두 끝나면, 타이틀 화면으로 이동
                //    SceneManager.LoadScene("Title_Tmp");
                //}
                //else if (GameManager.instance.GetEndingState() == GameManager.EndingState.True)
                //{
                //    // 게임 화면으로 돌아가서, 안드렌 시점으로 플레이
                //    //SceneManager.LoadScene("Title_Tmp");    // 임시로 타이틀 화면으로 이동하도록 적용
                //    //SceneManager.LoadScene("BW_H");
                //    // 사무실로 비동기 로딩
                //    StartCoroutine(LoadAsyncSceneForEnding());
                //}
            }
        }
        catch
        {
            ResetVariables();
            return;
        }
    }

    IEnumerator AutoStartConversation()
    {
        yield return new WaitForSeconds(0.5f);

        //setOfDesc_Index++;

        InteractionWithObject(startObjectLists[setOfDesc_Index++]);
    }

    public void ResetVariables()
    {
        //하나의 대화가 끝났으므로, 리셋
        index = 0;
        numOfText = 0;
        sentences = null;
        sentenceLists.Clear();
        imagePathLists.Clear();
        tempNpcNameLists.Clear();
        setActiveTrueLists.Clear();
        curNumOfNpcNameLists = 0;
        isFirstConversation = false;
        isTypingText = false;
    }

    public void SetActivePortrait(bool boolValue)
    {
        characterFrame.SetActive(boolValue);
        characterBackgroundImage.SetActive(boolValue);
        characterImage.SetActive(boolValue);
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

    IEnumerator LoadAsyncSceneForEnding()
    {
        Debug.Log("Starting Load");
        SceneManager.sceneLoaded += LoadingManager.instance.LoadSceneEnd;
        yield return StartCoroutine(LoadingManager.instance.Fade(true));

        AsyncOperation asyncLoad;

        float timer = 0.0f;

        if (GameManager.instance.GetPlayState() == GameManager.PlayState.Ending)
        {
            if (GameManager.instance.GetEndingState() == GameManager.EndingState.True)
            {
                LoadingManager.instance.loadSceneName = "BW_H";
                asyncLoad = SceneManager.LoadSceneAsync("BW_H");
            }
            else
            {
                LoadingManager.instance.loadSceneName = "Title_Tmp";
                asyncLoad = SceneManager.LoadSceneAsync("Title_Tmp");
            }
            Debug.Log("Setting LoadScene is done");
            asyncLoad.allowSceneActivation = false;

            if (GameManager.instance.GetEndingState() == GameManager.EndingState.True)
            {
                while (!CSVParser.instance.CompleteLoadFile())
                {
                    yield return null;
                }
            }
            Debug.Log("CompleteLoadFile");

            while (!asyncLoad.isDone)
            {
                yield return null;
                timer += Time.unscaledDeltaTime;

                if (asyncLoad.progress >= 0.9f)
                {
                    if (timer > 2.0f)//페이크 로딩
                    {
                        asyncLoad.allowSceneActivation = true;

                        timer = 0.0f;
                        yield break;
                    }
                }
            }
            Debug.Log("asyncLoad is Done");
        }
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
}
