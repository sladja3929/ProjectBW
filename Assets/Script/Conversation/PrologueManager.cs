using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PrologueManager : MonoBehaviour
{
    [SerializeField]
    private Sprite[] prologueCuts;
    [SerializeField]
    private GameObject prologueBlackBackground;
    [SerializeField]
    private GameObject prologueCut;
    [SerializeField]
    private Text prologueText;

    private Dictionary<int, Dictionary<string, string>> dataList;
    private List<Interaction> interactionLists;
    private List<Interaction> targetOfInteractionList;
    private int index;
    private int numOfText;
    [SerializeField]
    private bool isTextFull;
    private int cutIndex;           // 대화 index
    private int setOfConversation;  // 대화 묶음 번호
    private bool isSentenceDone;    // 해댕 대화 묶음이 모두 끝난경우
    private bool isConversationing; // 프롤로그 대화가 진행중인가?
    
    void Start()
    {
        GameManager.instance.SetPlayState(GameManager.PlayState.Prologue);

        dataList = GameObject.Find("DataManager").GetComponent<CSVParser>().GetDataList();
        interactionLists = GameObject.Find("DataManager").GetComponent<CSVParser>().GetInteractionLists();

        targetOfInteractionList = new List<Interaction>();

        InitPrologueSetting();

        BGMManager.instance.AutoSelectBGM();
    }
    
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)) && !isConversationing)
        {
            if (isTextFull)
                isTextFull = false;
            else
                PrologueNextSentence();
        }
    }

    public void InitPrologueSetting()
    {
        index = 0;
        numOfText = 0;
        isTextFull = false;
        prologueText.text = "";
        cutIndex = -1;
        setOfConversation = 800;
        isSentenceDone = false;
        isConversationing = false;

        PlayPrologue(setOfConversation.ToString());
    }

    // 800 ~ 819
    public void PlayPrologue(string conversationNum)
    {
        if (setOfConversation != 800)
        {
            prologueCut.SetActive(true);
            prologueBlackBackground.SetActive(false);
            prologueCut.GetComponent<Image>().sprite = prologueCuts[cutIndex];
        }
        else
        {
            prologueBlackBackground.SetActive(true);
            prologueCut.SetActive(false);
        }

        isSentenceDone = false;

        //해당 대화 묶음의 모든 대화를 찾아 저장
        targetOfInteractionList = interactionLists.FindAll(x => (x.CheckStartObject(conversationNum) == true));

        StartCoroutine(PrologueType()); // 첫 대화 출력
    }
    
    // 알맞은 대화를 출력해주는 코루틴
    IEnumerator PrologueType()
    {
        isConversationing = true;
        string text = targetOfInteractionList[index].GetDesc();

        // 한 문장의 한 단어씩 출력하는 반복문
        foreach (char letter in text.ToCharArray())
        {
            //출력된 텍스트 수가 최대 텍스트 수보다 작은 경우 -> 정상출력
            if (numOfText <= text.Length - 1)
            {
                // 대사 한줄이 모두 출력됐을때
                if (numOfText == text.Length - 1)
                {
                    isTextFull = true;
                }

                prologueText.text += letter;
                numOfText++;

                //글자 타이핑 소리?

                // 대사 한줄이 모두 출력됐을 때
                if (numOfText > text.Length - 1)
                {
                    isTextFull = false;
                    yield return new WaitUntil(() => !isTextFull);
                    
                    //prologueText.text = "";
                    numOfText = 0;
                    isConversationing = false;
                }
                else
                {
                    yield return new WaitForSeconds(0.02f);
                }
            }
        }
    }

    public void PrologueNextSentence()
    {
        try
        {
            if (index < targetOfInteractionList.Count - 1)
            {
                index++;

                prologueText.text = "";

                StartCoroutine(PrologueType());
            }
            else
            {
                prologueText.text = "";

                //하나의 대화가 끝났으므로, 리셋
                index = 0;
                numOfText = 0;
                targetOfInteractionList.Clear();
                isSentenceDone = true;

                cutIndex++;
                setOfConversation++;

                if (setOfConversation == 820)
                    StartCoroutine(LoadAsyncAct3Scene());
                else
                {
                    // 프롤로그의 대화묶음이 끝난경우
                    PlayPrologue(setOfConversation.ToString());
                }
            }
        }
        catch
        {
            Debug.Log("대화중 오류 발생");

            //하나의 대화가 끝났으므로, 리셋
            index = 0;
            numOfText = 0;
            targetOfInteractionList.Clear();
        }
    }

    IEnumerator LoadAsyncAct3Scene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("BW_K");

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

    }
}
