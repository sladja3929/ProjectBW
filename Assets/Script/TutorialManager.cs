using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance = null;

    [SerializeField]
    private Sprite[] nonHighlight_Sprite;
    [SerializeField]
    private Sprite[] highlight_Sprite;
    [SerializeField]
    private GameObject[] highlightObject;
    [SerializeField]
    private Sprite highlightMiniMap;
    [SerializeField]
    private Sprite nonHighLightMiniMap;
    [SerializeField]
    private GameObject zaral;               // 제렐
    [SerializeField]
    private GameObject assistant;           // 조수
    [SerializeField]
    private GameObject inGameCharacter;     // 메르테

    [SerializeField]
    private GameObject tutorial_Arrow;      // 레이나 집 진입 화살표
    [SerializeField]
    public GameObject entrance_RainaHouse;
    [SerializeField]
    private GameObject tutorial_Exit_Arrow; // 레이나 집 나가는 화살표
    [SerializeField]
    private GameObject[] GuideArrowToDowntown;  // 도심까지 가이드해주는 화살표

    [SerializeField] private GameObject tutorial_903_Trigger;
    [SerializeField] private GameObject tutorial_904_Trigger;
    [SerializeField] private GameObject tutorial_914_Trigger;

    public bool isPlayingTutorial;          // 튜토리얼 진행중에 이동 제어
    public int tutorial_Index;             // 튜토리얼 대화묶음 인덱스

    private bool[] checkCondition_901;      // wasd를 눌럿는지 체크
    public bool isFading;                   // 페이드아웃이 됐는지 여부
    public bool[] isCompletedTutorial;      // 대화묶음 진행 여부 ( 0 ~ 21 )

    public int index;                       // 915 대화 진행

    [SerializeField]
    private Text textFor915;
    private List<Interaction> interactionLists;
    private List<Interaction> targetInteractionLists;
    private bool isConversationing;
    private bool isTextFull;
    public bool isPlaying915;
    public bool isMinimapTutorial;
    public bool isPushedTab;
    public bool isNoteTutorial;         // 920 수첩 튜토리얼 제어
    public bool isParchmentTutorial;    // 921 양피지 튜토리얼 제어

    [SerializeField] private Sprite merte_Idle;
    [SerializeField] private Sprite merte_Right;
    [SerializeField] private Sprite merte_Left;
    [SerializeField] private Sprite zaral_Idle;
    [SerializeField] private Sprite zaral_Left;

    [SerializeField]
    private GameObject wall_For_903;        // 레이나 집으로 들어가는 과정에서 필요한 벽
    [SerializeField]
    private GameObject[] blocking_Object;   // 튜토리얼중 맵 이동 제어
    [SerializeField]
    private GameObject[] deActive_Object;   // 튜토리얼중 오브젝트 활성화 제어

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        if (GameManager.instance.GetPlayState() == GameManager.PlayState.Tutorial)
            InitSetting();
        else if (GameManager.instance.GetPlayState() == GameManager.PlayState.Act)
            SetActiveFalse_Tutorial_Character();
    }

    // Update is called once per frame
    void Update()
    {
        // 901번 대화묶음 체크
        if (GameManager.instance.GetPlayState() == GameManager.PlayState.Tutorial && !isCompletedTutorial[0])
        {
            if (Input.GetKeyDown(KeyCode.W))
                checkCondition_901[0] = true;
            if (Input.GetKeyDown(KeyCode.A))
                checkCondition_901[1] = true;
            if (Input.GetKeyDown(KeyCode.S))
                checkCondition_901[2] = true;
            if (Input.GetKeyDown(KeyCode.D))
                checkCondition_901[3] = true;

            if (CheckComplete901())
            {
                isCompletedTutorial[0] = true;

                InvokeTutorial();
                StartCoroutine(FlashTutorialEntranceArrow());
                entrance_RainaHouse.SetActive(true);
            }
        }

        // 915
        if (GameManager.instance.GetPlayState() == GameManager.PlayState.Tutorial && isPlaying915 && (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)) && !isConversationing)
        {
            if (isTextFull)
            {
                isTextFull = false;
            }
            else
            {
                TutorialNextSentence();
            }
        }
    }

    //currentPosition = "Chapter_Merte_Office"; // 메르테 위치 : 11419, 5169
    //currentPosition = "Chapter_Zaral_Office"; // 제렐 위치 : 11474, 7399
    public void InitSetting()
    {
        //MoveCamera.instance.SetPlayer(zaral);
        //inGameCharacter.SetActive(false);
        PlayerManager.instance.SetPlayer(zaral);
        PlayerManager.instance.SetCurrentPosition("Chapter_Zaral_Office");
        PlayerManager.instance.SetPlayerPosition(new Vector3(11474.0f, 7399.0f, 0f));
        assistant.GetComponent<Transform>().localPosition = new Vector3(12052.0f, 7410.0f, 0);
        //PlayerManager.instance.SetPlayer(inGameCharacter, "Chapter_Merte_Office");
        //tutorialCharcacter.SetActive(false);

        All_Tag_Change_To_Untagged();

        // 900 ~ 921
        tutorial_Index = 900;
        isFading = true;
        isPlayingTutorial = false;
        checkCondition_901 = new bool[4];
        tutorial_Arrow.SetActive(false);
        tutorial_Exit_Arrow.SetActive(false);
        isCompletedTutorial = new bool[22];
        isCompletedTutorial[0] = true;
        index = 0;
        isConversationing = false;
        isTextFull = false;
        isPlaying915 = false;
        isMinimapTutorial = false;
        isPushedTab = false;
        isNoteTutorial = false;
        isParchmentTutorial = false;

        for (int i = 0; i < GuideArrowToDowntown.Length; i++)
        {
            GuideArrowToDowntown[i].SetActive(false);
        }

        wall_For_903.SetActive(true);

        for (int i = 0; i < blocking_Object.Length; i++)
        {
            blocking_Object[i].SetActive(true);
        }

        for (int i = 0; i < deActive_Object.Length; i++)
        {
            deActive_Object[i].SetActive(false);
        }

        interactionLists = DialogManager.instance.GetInteractionList();

        InvokeTutorial();
    }

    public void InvokeTutorial()
    {
        if (tutorial_Index == 901)
        {
            StartCoroutine(Play901());
        }
        else if (tutorial_Index == 903)
        {
            StartCoroutine(Play903());
        }
        else if (tutorial_Index == 904)
        {
            Invoke("PlayTutorial", 0.1f);
            StartCoroutine(Highlight_Object(0, isCompletedTutorial[3]));
            tutorial_904_Trigger.SetActive(false);
        }
        else if (tutorial_Index == 908)
        {
            Invoke("PlayTutorial", 0.3f);
            StartCoroutine(Highlight_Object(2, isCompletedTutorial[7]));
        }
        else if (tutorial_Index == 911)
        {
            Invoke("PlayTutorial", 0.6f);
        }
        else if (tutorial_Index == 914)
        {
            Invoke("PlayTutorial", 0.3f);
            tutorial_914_Trigger.SetActive(false);
        }
        else if (tutorial_Index == 918 || tutorial_Index == 919 || tutorial_Index == 920)
        {
            Invoke("PlayTutorial", 4.5f);
        }
        else
        {
            Invoke("PlayTutorial", 0.3f);
        }
    }

    public void PlayTutorial()
    {
        DialogManager.instance.InteractionWithObject(tutorial_Index.ToString());  // 900 ~ 921
    }

    public void IncreaseTutorial_Index()
    {
        tutorial_Index++;
        //Debug.Log(tutorial_Index + "로 증가");
    }

    // 튜토리얼을 진행하는 메인 캐릭터 설정 함수
    public void TutorialCharacterSetPosition(Vector2 position)
    {
        zaral.GetComponent<Transform>().localPosition = position;
    }

    public GameObject GetTutorialCharacter()
    {
        return zaral;
    }

    public void SetActiveFalse_Tutorial_Character()
    {
        zaral.SetActive(false);
        assistant.SetActive(false);
    }

    public void SetAssistantPosition(Vector3 position)
    {
        assistant.GetComponent<Transform>().localPosition = position;
    }

    public void SetZaralPosition(Vector3 position)
    {
        zaral.GetComponent<Transform>().localPosition = position;
    }

    // wasd가 다 눌렸는지 확인
    public bool CheckComplete901()
    {
        for (int i = 0; i < 4; i++)
        {
            if (checkCondition_901[i] == false)
                return false;
        }

        return true;
    }

    // 레이나 집으로 가기 위한 가이드 화살표 점멸
    public IEnumerator FlashTutorialEntranceArrow()
    {
        yield return new WaitForSeconds(0.3f);

        while (!isCompletedTutorial[1])
        {
            tutorial_Arrow.SetActive(true);

            yield return new WaitForSeconds(0.5f);

            tutorial_Arrow.SetActive(false);

            yield return new WaitForSeconds(0.5f);
        }

        if (isCompletedTutorial[1] && tutorial_Arrow.activeSelf)
            tutorial_Arrow.SetActive(false);
    }

    // 레이나 집에서 나가기 위한 화살표 점멸
    public IEnumerator FlashTutorialExitArrow()
    {
        yield return new WaitForSeconds(0.3f);

        while (!isCompletedTutorial[9])
        {
            tutorial_Exit_Arrow.SetActive(true);

            yield return new WaitForSeconds(0.5f);

            tutorial_Exit_Arrow.SetActive(false);

            yield return new WaitForSeconds(0.5f);
        }

        if (isCompletedTutorial[9] && tutorial_Exit_Arrow.activeSelf)
            tutorial_Exit_Arrow.SetActive(false);
    }

    // 도심으로 유도하기 위한 가이드 화살표 점멸
    public IEnumerator FlashGuideArrow()
    {
        while (!isCompletedTutorial[14])
        {
            for(int i=0; i< GuideArrowToDowntown.Length; i++) GuideArrowToDowntown[i].SetActive(true);

            yield return new WaitForSeconds(0.5f);

            for (int i = 0; i < GuideArrowToDowntown.Length; i++) GuideArrowToDowntown[i].SetActive(false);

            yield return new WaitForSeconds(0.5f);
        }

        if (isCompletedTutorial[14])
        {
            for (int i = 0; i < GuideArrowToDowntown.Length; i++) GuideArrowToDowntown[i].SetActive(false);
        }
    }

    // 각 튜토리얼에서 상호작용해야할 오브젝트들을 강조
    public IEnumerator Highlight_Object(int indexOfObject, bool isCompleted)
    {
        yield return new WaitForSeconds(0.3f);
        
        while (!isCompleted)
        {
            highlightObject[indexOfObject].GetComponent<SpriteRenderer>().sprite = highlight_Sprite[indexOfObject];

            yield return new WaitForSeconds(0.5f);

            highlightObject[indexOfObject].GetComponent<SpriteRenderer>().sprite = nonHighlight_Sprite[indexOfObject];

            if ((tutorial_Index == 905 && isCompletedTutorial[3]) || (tutorial_Index == 907 && isCompletedTutorial[5]) || (tutorial_Index == 909 && isCompletedTutorial[7]) || (tutorial_Index == 912 && isCompletedTutorial[11])
                || (tutorial_Index == 914 && isCompletedTutorial[13]) || (tutorial_Index == 921 && isCompletedTutorial[21]))
            {
                isCompleted = true;
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    // 그때그때 튜토리얼 오브젝트의 태그명 변경
    public void TagChange(int indexOfObject, string tag)
    {
        // ob = 튜토리얼 오브젝트 , tag = InteractionObject or Untagged
        highlightObject[indexOfObject].tag = tag;
    }

    public void All_Tag_Change_To_Untagged()
    {
        for (int i = 0; i < highlightObject.Length; i++)
        {
            TagChange(i, "Untagged");
        }
    }

    public IEnumerator Play901()
    {
        yield return new WaitUntil(() => !isFading);

        yield return new WaitForSeconds(0.6f);

        DialogManager.instance.InteractionWithObject("901");
        tutorial_Index++;
        isFading = true;
        isPlayingTutorial = false;
    }

    public IEnumerator Play903()
    {
        SetAssistantPosition(new Vector3(4630.0f, 7300.0f, 0));

        yield return new WaitForSeconds(0.8f);

        DialogManager.instance.InteractionWithObject("903");
        tutorial_Index++;

        tutorial_903_Trigger.SetActive(false);
        wall_For_903.SetActive(false);
    }

    // 알맞은 대화를 출력해주는 코루틴
    public IEnumerator TypingFor915()
    {
        UIManager.instance.isFading = true;

        isPlaying915 = true;
        isConversationing = true;
        targetInteractionLists = interactionLists.FindAll(x => (x.CheckStartObject("915")) == true);
        
        string text = targetInteractionLists[index].GetDesc();

        float tempAlpha = textFor915.color.a;
        Color tempColor = textFor915.color;
        tempColor.a = 0f;
        textFor915.color = tempColor;

        textFor915.text = text;

        while (textFor915.color.a < 1f)
        {
            tempColor.a += 0.02f;

            textFor915.color = tempColor;

            yield return new WaitForSeconds(0.02f);
        }

        isConversationing = false;
        isTextFull = false;

        yield return new WaitUntil(() => !isTextFull);
    }

    public void TutorialNextSentence()
    {
        try
        {
            if (index < targetInteractionLists.Count - 1)
            {
                index++;

                textFor915.text = "";

                StartCoroutine(TypingFor915());
            }
            else
            {
                textFor915.text = "";

                //하나의 대화가 끝났으므로, 리셋
                index = 0;
                targetInteractionLists.Clear();
                //interactionLists.Clear();

                // 915 대화묶음이 끝난경우
                //SetAssistantPosition(new Vector3(11330.0f, 5220.0f, 0)); // 3번 인덱스의 대화와 함께 조수 등장
                inGameCharacter.SetActive(true);
                MoveCamera.instance.SetPlayer(inGameCharacter);
                PlayerManager.instance.SetPlayer(inGameCharacter);
                PlayerManager.instance.SetCurrentPosition("Chapter_Merte_Office");
                PlayerManager.instance.SetPlayerPosition(new Vector3(12110.0f, 5220.0f, 0));
                isPlaying915 = false;

                StartCoroutine(UIManager.instance.FadeOutForPlaying915());
            }
        }
        catch
        {
            Debug.Log("대화중 오류 발생");

            //하나의 대화가 끝났으므로, 리셋
            index = 0;
            targetInteractionLists.Clear();
        }
    }

    public void SetActive_HighlightObject(int index, bool boolValue)
    {
        highlightObject[index].SetActive(boolValue);
    }

    public void SetSpriteCharacterFor918()
    {
        inGameCharacter.GetComponent<Animator>().SetFloat("x", 1);
        inGameCharacter.GetComponent<SpriteRenderer>().sprite = merte_Right;

        if (zaral.GetComponent<Transform>().localScale.x < 0)
        {
            Transform temp = zaral.GetComponent<Transform>();
            temp.localScale = new Vector3(temp.localScale.x * -1, temp.localScale.y, temp.localScale.z);
            zaral.GetComponent<Transform>().localScale = temp.localScale;
        }

        if (inGameCharacter.GetComponent<Transform>().localScale.x > 0)
        {
            Transform temp = inGameCharacter.GetComponent<Transform>();
            temp.localScale = new Vector3(temp.localScale.x * -1, temp.localScale.y, temp.localScale.z);
            inGameCharacter.GetComponent<Transform>().localScale = temp.localScale;
        }

        zaral.GetComponent<SpriteRenderer>().sprite = zaral_Left;
    }

    public void SetSpriteCharacterFor919()
    {
        //inGameCharacter.GetComponent<Animator>().SetFloat("x", -1);
        inGameCharacter.GetComponent<SpriteRenderer>().sprite = merte_Left;

        if (zaral.GetComponent<Transform>().localScale.x > 0)
        {
            Transform temp = zaral.GetComponent<Transform>();
            temp.localScale = new Vector3(temp.localScale.x * -1, temp.localScale.y, temp.localScale.z);
            zaral.GetComponent<Transform>().localScale = temp.localScale;
        }

        if (inGameCharacter.GetComponent<Transform>().localScale.x < 0)
        {
            Transform temp = inGameCharacter.GetComponent<Transform>();
            temp.localScale = new Vector3(temp.localScale.x * -1, temp.localScale.y, temp.localScale.z);
            inGameCharacter.GetComponent<Transform>().localScale = temp.localScale;
        }

        zaral.GetComponent<SpriteRenderer>().sprite = zaral_Left;
    }

    public void SetSpriteCharacterFor920()
    {
        inGameCharacter.GetComponent<Animator>().SetFloat("y", -1);

        if (inGameCharacter.GetComponent<Transform>().localScale.x < 0)
        {
            Transform temp = inGameCharacter.GetComponent<Transform>();
            temp.localScale = new Vector3(temp.localScale.x * -1, temp.localScale.y, temp.localScale.z);
            inGameCharacter.GetComponent<Transform>().localScale = temp.localScale;
        }
    }

    public void Invoke_SetSpriteCharacterFor920()
    {
        Invoke("SetSpriteCharacterFor920", 2.0f);
    }

    public void EndTutorial()
    {
        for (int i = 0; i < blocking_Object.Length; i++)
        {
            blocking_Object[i].SetActive(false);
        }

        for (int i = 0; i < deActive_Object.Length; i++)
        {
            deActive_Object[i].SetActive(true);
        }

        GameManager.instance.SetPlayState(GameManager.PlayState.Act);
        isPlayingTutorial = false;
    }
}
