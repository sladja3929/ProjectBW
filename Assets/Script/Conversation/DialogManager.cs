using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance = null;

    [SerializeField]
    private Text conversationText;
    [SerializeField]
    private Text npcNameText;
    [SerializeField]
    private Image npcImage;
    
    private string[] sentences;
    private int index;
    public float typingSpeed;
    private int numOfText = 0;  //현재 출력된 텍스트 수
    private int textLimit;  //한 대화창에 출력할 최대 텍스트 수
    public bool isTextFull; //한 대화창에 출력할 최대 텍스트에 도달했는지의 여부
    public bool isSentenceDone; //출력할 문장이 다 출력 됐는지 여부

    private Dictionary<int, Dictionary<string, string>> dataLists;
    private List<Interaction> interactionLists;
    private List<string> imagePathLists;    //npcFrom에 해당하는 npc들의 이미지의 경로 리스트
    private List<int> tempIdLists;          //status 변경용
    private List<string> tempNpcNameLists;  //npc 이름 불러오는용
    private int numNpcNameLists;            //대화에 연관된 npc가 몇명인지 저장용
    private int curNumOfNpcNameLists;       //현재 대화중인 npc 이름의 index
    private List<string> rewardsLists;      //대화로 인해 얻어질 단서 목록들
    private List<string> sentenceLists;     //출력해야 할 대화들을 담을 리스트
    private EventConversationManager eventManager;

    private NpcParser npcParser;
    private string tempNpcName;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        textLimit = 125;
        isTextFull = false;
        isSentenceDone = false;
        index = 0;
        numNpcNameLists = 0;
        curNumOfNpcNameLists = 0;
        sentences = new string[] { "" };
        //UIManager 오브젝트에 있는 CSVParser의 스크립트 안에 있는 GetDataList() 함수로 상호작용 dictionary 불러오기
        dataLists = GameObject.Find("DataManager").GetComponent<CSVParser>().GetDataList();
        interactionLists = GameObject.Find("DataManager").GetComponent<CSVParser>().GetInteractionLists();

        imagePathLists = new List<string>(); //캐릭터 이미지 추가되면 적용(테스트) 해야함
        tempIdLists = new List<int>();
        tempNpcNameLists = new List<string>();
        rewardsLists = new List<string>();
        sentenceLists = new List<string>();
        eventManager = new EventConversationManager();

        npcParser = new NpcParser();
    }

    void Update()
    {
        // 대화가 텍스트창에 한계치만큼 가득 찼거나, 한 대화가 모두 출력됐을때, 다음 대화로 넘어갈 수 있게 해줌
        if( (numOfText > textLimit) || (UIManager.instance.isConversationing && isSentenceDone) )
        {
            UIManager.instance.canSkipConversation = true;
        }
    }

    public void InteractionWithObject(string objectName)
    {
        /* 확장성을 위해 objectName을 파라미터로 받아서, 해당 물체를 조사하는 상호작용 구현하기 */
        /* 시연을 위해서는 "ER의 증언"과 "터진 쓰레기 봉지" 를 파라미터로 받아올 것임. */
        /* 대화가 여러개 있을 수도 있다.. */
        /* npcFrom : 대화를 하는 주체 */
        UIManager.instance.isConversationing = true;    // 대화중
        UIManager.instance.OpenConversationUI();        // 대화창 오픈
        //UIManager.instance.CloseGetClueButton();               // 단서 선택창 비활성화(임시)

        //EventConversationManager eventManager = new EventConversationManager(); //CheckEvent 함수를 위한 클래스 변수
        
        string targetObject = objectName;   //npcFrom에 해당하는 값

        //targetObject에 해당하는 npc의 이름을 가진 클래스의 index 알아오기
        //int indexOfInteraction = interactionLists.FindIndex(x => x.GetStartObject() == targetObject);

        /* 해당 오브젝트에 관한 이벤트가 있는지 먼저 확인해야함. */
        /* 이름(startObject)이 같은 것이 여러개일 수 있으니, int형 리스트의 형태로 저장하고, 그 중에서 골라내는 것은 어떨까???? */
        List<Interaction> targetOfInteractionList = new List<Interaction>();
        targetOfInteractionList = interactionLists.FindAll(x => x.GetStartObject() == targetObject);

        //대화목록의 id값
        int tempId;
        int eventCheckValue = eventManager.CheckEvent(targetOfInteractionList, interactionLists);

        // 해당 오브젝트에 대한 이벤트 유무 확인
        if (eventCheckValue != -1)
        {
            // 이벤트가 있다면 진행 시켜야지
            tempId = eventCheckValue;
        }
        else
        {
            // 이벤트가 없다면 원래대로 진행
            //targetObject에 해당하는 npc의 이름을 가진 클래스의 index 알아오기
            int indexOfInteraction = interactionLists.FindIndex(x => x.GetStartObject() == targetObject);

            tempId = int.Parse((dataLists[indexOfInteraction])["id"]);
        }

        //tempIdLists = indexList;    // 진행된 대화들의 status를 올리기위해 tempIdLists에 indexList 저장
        
        //대화가 이어질 수 있도록 parent값을 이용
        int tempParentIndex = interactionLists.FindIndex(x => x.GetId() == tempId);
        int tempParent = interactionLists[tempParentIndex].GetParent();
        Debug.Log("초기의 tempParent : " + tempParent);

        //대화가 진행됐는지 알 수 있도록 status값을 이용 -> 대화 안했으면 0, 했으면 1 -> 1일 때의 예외처리도 추후에 추가해야함.
        //추후에 각 대화에 해당하는, 변경된 status값을 저장하기 위한 코드 필요(통째로 csv형식으로 저장하는 것이 좋을듯) 
        //int tempStatus = int.Parse((dataLists[tempId])["status"]);

        /* 캐릭터의 이름, 사진을 바꿔줘야함. -> 이름은 완료함, 사진을 바꿔줘야 함.*/

        /* Id와 parent값이 같다는 것은 더이상 관련있는 대화가 없다는 것이다. */
        /* 처음에는 tempId와 tempParent가 같던 다르던 일단 읽음 */

        CheckAndAddSentence(tempId);    //해당 tempId에 맞는 대화로 인해 얻을 수 있는 정보를 얻는 함수

        /* 현재의 tempId와 tempParent가 다르다면(연결된 대화가 있다면) 진행 */
        while (tempId != tempParent)
        {
            tempId = tempParent; //다음 대화를 위해 index에 tempParent(다음 연관된 대화와 관련된 id값)값을 넣음
                                 //대화가 이어질 수 있도록 parent값을 이용
            tempParentIndex = interactionLists.FindIndex(x => x.GetId() == tempId);
            tempParent = interactionLists[tempParentIndex].GetParent();

            CheckAndAddSentence(tempId);
        }
        
        sentences = sentenceLists.ToArray();
        numNpcNameLists = tempNpcNameLists.Count;

        /* debug */
        //Debug.Log("npc 수 : " + numNpcNameLists);
        //for (int i = 0; i < tempNpcNameLists.Count; i++)
        //{
        //    Debug.Log("npc " + (i + 1) + " : " + tempNpcNameLists[i]);
        //}
        //for (int i = 0; i < sentences.Length; i++)
        //{
        //    Debug.Log("sentence" + (i + 1) + " : " + sentences[i]);
        //}

        /* while문을 빠져나오면 sentenceLists에 대화목록이 쭉 저장되어있을 것이다. */
        StartCoroutine(Type());

    }

    IEnumerator Type()
    {
        //대화 할 때 마다 대화중인 캐릭터 이름 변경
        /* tempNpcNameLists[curNumOfNpcNameLists]을 이용하여 고유한 character code 마다 이름으로 바꿔줘야함 */
        tempNpcName = npcParser.GetNpcName(tempNpcNameLists[curNumOfNpcNameLists]);

        if (tempNpcName != null)
            npcNameText.text = tempNpcName;
        else
            npcNameText.text = "stranger";

        if (tempNpcNameLists.Count > 1) curNumOfNpcNameLists++;

        conversationText.text = "";
        numOfText = 0;
        isSentenceDone = false;

        foreach (char letter in sentences[index].ToCharArray())
        {
            //출력된 텍스트 수가 최대 텍스트 수보다 작은 경우 -> 정상출력
            if (numOfText <= textLimit)
            {
                if (numOfText == textLimit)
                    isTextFull = true;
                conversationText.text += letter;
                numOfText++;
                UIManager.instance.canSkipConversation = false;

                if (numOfText > textLimit)
                {
                    UIManager.instance.canSkipConversation = true;
                    yield return new WaitUntil(() => !isTextFull);  //isTextFull이 false가 될때까지 기다린다. (마우스 왼쪽 클릭 -> isTextFull = false)

                    conversationText.text = "";
                    numOfText = 0;
                }
                else
                {
                    yield return new WaitForSeconds(typingSpeed);
                }
            } 
        }
        
        UIManager.instance.canSkipConversation = true;

        isSentenceDone = true;
    }

    /* 해당 tempId에 맞는 대화로 인해 얻을 수 있는 정보를 얻는 함수 *
     * 말하고 있는 npc 이름, 획득할 수 있는 단서, 대화 텍스트 */
    public void CheckAndAddSentence(int tempId)
    {
        tempIdLists.Add(tempId);                                //status 변경용

        tempNpcNameLists.Add((dataLists[tempId])["npcFrom"]);    //대화중인 npc이름 변경용

        if (dataLists[tempId]["rewards"] != null)
        {
            //대화를 통해 얻을 수 있는 단서들의 목록 만들기
            if (dataLists[tempId]["rewards"].Contains(","))
            {
                string[] rewardArr = dataLists[tempId]["rewards"].Split(',');
                for (int i = 0; i < rewardArr.Length; i++)
                {
                    rewardsLists.Add(rewardArr[i]);
                    Debug.Log((i + 1) + "번째로 획득할 단서 : " + rewardArr[i]);
                }
            }
            else
            {
                string reward = dataLists[tempId]["rewards"];
                rewardsLists.Add(reward);
                Debug.Log("획득할 단서 : " + rewardsLists[0]);
            }
        }

        sentenceLists.Add((dataLists[tempId])["desc"]);  //해당 id값의 대화 추가
    }


    public void NextSentence()
    {
        UIManager.instance.canSkipConversation = false;

        if (index < sentences.Length - 1)
        {
            index++;
            conversationText.text = "";
            StartCoroutine(Type());
        } else
        {
            conversationText.text = "";
           
            for(int i=0; i<tempIdLists.Count; i++)
            {   //지금 까지 한 모든 대화 읽음 처리
                //(dataLists[tempIdLists[i]])["status"] = "1";
                int tempIndex = interactionLists.FindIndex(x => x.GetId() == tempIdLists[i]);
                interactionLists[tempIndex].SetStatus(interactionLists[tempIndex].GetStatus() + 1); //진행된 대화는 status 1 증가
            }
            UIManager.instance.isConversationing = false;
            UIManager.instance.CloseConversationUI();   //모든 대화가 끝나면 대화창 닫기

            //대화로 인해 얻은 보상이 있으면 단서창에 추가
            if (rewardsLists.Count > 1)
            {
                for (int i = 0; i < rewardsLists.Count; i++)
                {
                    GameManager.instance.GetClue(rewardsLists[i]);
                }
            }
            else if(rewardsLists.Count == 1)
            {
                GameManager.instance.GetClue(rewardsLists[0]);
            }

            /* 단서 내용1을 위해 각 clueName에 연관되어있는 대화들을 player의 clueList안의 firstInfoOfClue 변수에다가 넣음 */
            for (int i = 0; i < rewardsLists.Count; i++)
            {
                string tempText = "<i>";    //기울임 효과를 위한 <i></i> 태그
                for (int j = 0; j < tempNpcNameLists.Count; j++)
                {   //이름 : "대화"
                    /* tempNpcNameLists[j]을 이용하여 고유한 character code 마다 이름으로 바꿔줘야함 */
                    tempNpcName = npcParser.GetNpcName(tempNpcNameLists[j]);

                    if (tempNpcName == null)
                        tempNpcName = "stranger";

                    tempText += (tempNpcName + " : " + sentenceLists[j]);
                }
                tempText += "</i>";
                Debug.Log(tempText);

                /* 플레이어가 획득한 단서중에서 보상으로 받은 단서의 이름이 같은게 있으면, 해당 단서(Clue class)의 firstInfoOfClue 변수에 대화를 저장한다. */
                for (int j = 0; j < PlayerManager.instance.ClueLists.Length; j++)
                {
                    for (int k = 0; k < PlayerManager.instance.ClueLists[j].Count; k++)
                    {
                        if (rewardsLists[i].Equals(PlayerManager.instance.ClueLists[j][k].GetName()))
                        {
                            PlayerManager.instance.ClueLists[j][k].SetFirstInfoOfClue(tempText);
                        }
                    }
                }
            }

            //하나의 대화가 끝났으므로, 리셋
            index = 0;
            numOfText = 0;
            sentences = null;
            sentenceLists.Clear();
            imagePathLists.Clear();
            tempIdLists.Clear();
            tempNpcNameLists.Clear();
            curNumOfNpcNameLists = 0;
            rewardsLists.Clear();
            //UIManager.instance.OpenGetClueButton();               // 단서 선택창 비활성화(임시)
        }
    }

}
