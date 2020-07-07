using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Threading.Tasks;
using System.Threading;

public class CSVParser : MonoBehaviour
{
    public static CSVParser instance = null;

    /* UTF-8로 인코딩 된 csv 파일로부터 대화 데이터들을 모두 가져와서 저장해줄 클래스
    * csv 파일을 수정하고 나서는 꼭! 다른이름으로 저장을 한다(UTF-8 csv형식)
    * 규칙! csv 파일 안에 쉼표(,)를 사용할경우에는 , 대신 $ 로써 표현을 한다
    * <b> text </b> : bold 효과 표현
    * <i> text </i> : 기울임 효과 표현 
    * <size=?> text </size> : ?만큼의 size 효과 표현 
    * 큰따옴표(")는 csv상에서 """로 표현되니 Replace 함수 써서 \"로 바꾸기 */

    /* 메인 Dictionary의 key값은 각 interaction의 id값들을 넣고, value는 내부 dictionary를 넣는다 */
    /* 내부 Dictionary의 key값은 각 interaction이 가지고 있는 npcName 등의 값이고, value는 해당 key값의 실질적인 값을 가지게 한다. */
    private Dictionary<int, Dictionary<string, string>> dataList;
    private Dictionary<int, Dictionary<string, string>> status_Repeatability_DataList;
    private Dictionary<int, Dictionary<string, string>> endingDataList;

    // 단서 테이블을 파싱하면서, S.no 값에 따라 단서들을 각 리스트에 넣을 것임.
    // 엔딩 분기 화면을 띄우기 전에, 리스트 안에 있는 단서들을 모두 가지고 있는지에 따라 분기 화면을 나눌 것임. (0702)
    //[SerializeField] private List<string> valueEndingClueLists; // 발루아엔딩은 항상 볼 수 있는 엔딩
    [SerializeField] private List<string> andrenEndingClueLists;
    [SerializeField] private List<string> arnoldEndingClueLists;
    [SerializeField] private List<string> trueEndingClueLists;

    /* 구조는 dataList와 일맥상통하다 */
    // csv 형식의 단서 테이블의 데이터들을 저장할 수 있는 변수
    private Dictionary<int, Dictionary<string, string>> clueList;

    //게임에 필요한 상호작용들을 가지고 있을 리스트변수 선언
    private List<Interaction> interactionLists;
    private List<Interaction> endingInteractionLists;
    private List<ClueStructure> clueStructureLists;
    private List<string> startObjectListsForEnding;

    [SerializeField] public List<string> diary_Contents;

    private string initConversationDataPath;
    private string initClueDataPath;
    private string playerConversationDataPath;
    private string trueEndingDataPath;
    private string valuaEndingDataPath;
    private string arnoldEndingDataPath;
    private string andrenEndingDataPath;

    /* csv 파일 불러오면서 적용시키기 */
    void Awake()
    {
        DontDestroyOnLoad(this);

        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        SettingDataPath(); // 데이터 파일 경로 세팅

        // 파일 컨트롤은 GameManager에서 수행
        //InitDataFromCSV();
    }//Awake()

    // 초기 데이터 파일 암호화 용도
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.F12))
    //    {
    //        string temp = File.ReadAllText(initConversationDataPath);
    //        File.WriteAllText(initConversationDataPath, GameManager.instance.EncryptData(temp), System.Text.Encoding.UTF8);
    //        //File.WriteAllText(initConversationDataPath, temp, System.Text.Encoding.UTF8);

    //        //string temp = File.ReadAllText(initClueDataPath);
    //        //File.WriteAllText(initClueDataPath, GameManager.instance.EncryptData(temp), System.Text.Encoding.UTF8);
    //        //File.WriteAllText(initClueDataPath, temp, System.Text.Encoding.UTF8);

    //        temp = File.ReadAllText(trueEndingDataPath);
    //        File.WriteAllText(trueEndingDataPath, GameManager.instance.EncryptData(temp), System.Text.Encoding.UTF8);

    //        //temp = File.ReadAllText(valuaEndingDataPath);
    //        //File.WriteAllText(valuaEndingDataPath, GameManager.instance.EncryptData(temp), System.Text.Encoding.UTF8);

    //        //temp = File.ReadAllText(arnoldEndingDataPath);
    //        //File.WriteAllText(arnoldEndingDataPath, GameManager.instance.EncryptData(temp), System.Text.Encoding.UTF8);

    //        //temp = File.ReadAllText(andrenEndingDataPath);
    //        //File.WriteAllText(andrenEndingDataPath, GameManager.instance.EncryptData(temp), System.Text.Encoding.UTF8);

    //        //GameManager.instance.thread = new Thread(GameManager.instance.SaveGameData);
    //        //GameManager.instance.thread.IsBackground = true;
    //        //GameManager.instance.thread.Start();
    //        Debug.Log("파일 암호화 완료");
    //    }
    //}

    // 데이터 파일 경로 지정 함수
    public void SettingDataPath()
    {
        initConversationDataPath = Application.streamingAssetsPath + "/Data/전체_테이블.csv";
        initClueDataPath = Application.streamingAssetsPath + "/Data/단서.csv";
        playerConversationDataPath = Application.streamingAssetsPath + "/Data/PlayerConversation.csv";
        trueEndingDataPath = Application.streamingAssetsPath + "/Data/True_Ending.csv";
        valuaEndingDataPath = Application.streamingAssetsPath + "/Data/Valua_Ending.csv";
        arnoldEndingDataPath = Application.streamingAssetsPath + "/Data/Arnold_Ending.csv";
        andrenEndingDataPath = Application.streamingAssetsPath + "/Data/Andren_Ending.csv";
    }

    /* 타이틀에서 새로하기를 눌렀을 때, 실행 */
    public void InitDataFromCSV()
    {
        ParsingConversationCSV(initConversationDataPath);   // 대화 테이블 파싱
        ParsingClueDataCSV(initClueDataPath);       // 단서 테이블 파싱
    }

    // 달성한 엔딩에 따라 알맞은 엔딩 파일 불러오기
    public void LoadingEndingDataFromCSV()
    {
        GameManager.EndingState gameEndingState = GameManager.instance.GetEndingState();

        switch (gameEndingState)
        {
            case GameManager.EndingState.Valua:
                ParsingEndingCSV(valuaEndingDataPath);
                break;
            case GameManager.EndingState.Arnold:
                ParsingEndingCSV(arnoldEndingDataPath);
                break;
            case GameManager.EndingState.Andren:
                ParsingEndingCSV(andrenEndingDataPath);
                break;
            case GameManager.EndingState.True:
                ParsingEndingCSV(trueEndingDataPath);
                break;
            default:
                Debug.Log("엔딩 파일 불러오기 오류, 엔딩 코드 : " + gameEndingState);
                break;
        }
    }
    
    public void GetInfoParsingConversationCSV(object dataPath, object status_Repeat_DataPath)
    {
        dataList = new Dictionary<int, Dictionary<string, string>>();
        status_Repeatability_DataList = new Dictionary<int, Dictionary<string, string>>();

        interactionLists = new List<Interaction>();

        string textAsset = File.ReadAllText(dataPath.ToString(), System.Text.Encoding.UTF8); // old
        string newTextAsset = File.ReadAllText(status_Repeat_DataPath.ToString(), System.Text.Encoding.UTF8);
        string[] tempStringArr = new string[2];

        if (GameManager.instance.isEncrypted)
        {
            tempStringArr = GameManager.instance.DecryptData(textAsset, newTextAsset);
            //Debug.Log("textAsset 1 = " + tempStringArr[0]);
            //Debug.Log("textAsset 2 = " + tempStringArr[1]);
        }

        string[] stringArrOld = tempStringArr[1].Split(new string[] { "줄바꿈\r\n" }, System.StringSplitOptions.None);
        string[] subjectArrOld = stringArrOld[0].Split(',');      //속성에 해당하는 첫째줄 분리

        int index = 0;

        for (int i = 1; i < stringArrOld.Length - 1; i++)
        {
            //두번째 줄부터 ,를 기준으로 쪼갬
            string[] dataArrOld = stringArrOld[i].Split(',');

            //해당 index가 dictionary에 없으면 추가
            if (!status_Repeatability_DataList.ContainsKey(index))
                status_Repeatability_DataList.Add(index, new Dictionary<string, string>());

            //메인 dictionary에는 index의 키값을 가지는 내부 dictionary가 있을것이다.
            //내부 dictionary에 각 속성들의 값을 대입하기위해 for문을 돌린다.
            for (int j = 0; j < dataArrOld.Length; j++)
            {
                /* """ -> " , $ -> , 변환해서 데이터 넣기 */
                //dataArr[j] = ReplaceDoubleQuotationMark(dataArr[j]);
                //dataArr[j] = RemoveDoubleQuotationMark(dataArr[j]);     // 대화에 줄바꿈이 있을경우, 양끝에 "가 붙은걸 없애기
                dataArrOld[j] = ReplaceComma(dataArrOld[j]);
                dataArrOld[j] = ReplaceEnter(dataArrOld[j]);
                //if (index >= 1340)
                //{
                //    Debug.Log("index = " + index);
                //    Debug.Log("subjectArr[" + j + "] = " + subjectArr[j]);
                //    Debug.Log("dataArr[" + j + "] = " + dataArr[j]);
                //}
                status_Repeatability_DataList[index].Add(subjectArrOld[j], dataArrOld[j]);

            }//for j
            index++;
        }//for i

        //전체 데이터 줄바꿈단위로 분리 (csv파일의 한 문장 끝에는 \r\n이 붙어있음)
        //string[] stringArr = textAsset.text.Split(new string[] { "\r\n" }, System.StringSplitOptions.None);
        string[] stringArr = tempStringArr[0].Split(new string[] { "줄바꿈\r\n" }, System.StringSplitOptions.None);
        string[] subjectArr = stringArr[0].Split(',');      //속성에 해당하는 첫째줄 분리

        index = 0;

        //맨 마지막 줄은 한 줄 띄워져있으니 생략하기위해 길이 - 1 해줌
        //첫번째줄 속성줄을 무시하기위해 i = 1 부터 시작
        for (int i = 1; i < stringArr.Length - 1; i++)
        {
            //두번째 줄부터 ,를 기준으로 쪼갬
            string[] dataArr = stringArr[i].Split(',');

            //int index = int.Parse(dataArr[0]);  //첫번째 속성인 id값을 int형으로 집어넣기

            /* FindIndex가 0부터 반환하면 0, 1부터 반환하면 1로 고쳐야함 */
            //int index = 0; // 그냥 0부터 차례대로 박아넣기 // int.Parse(dataArr[4]);  //5번째 속성인 id값을 int형으로 집어넣기

            //해당 index가 dictionary에 없으면 추가
            if (!dataList.ContainsKey(index))
                dataList.Add(index, new Dictionary<string, string>());

            //메인 dictionary에는 index의 키값을 가지는 내부 dictionary가 있을것이다.
            //내부 dictionary에 각 속성들의 값을 대입하기위해 for문을 돌린다.
            for (int j = 0; j < dataArr.Length; j++)
            {
                /* """ -> " , $ -> , 변환해서 데이터 넣기 */
                //dataArr[j] = ReplaceDoubleQuotationMark(dataArr[j]);
                //dataArr[j] = RemoveDoubleQuotationMark(dataArr[j]);     // 대화에 줄바꿈이 있을경우, 양끝에 "가 붙은걸 없애기
                dataArr[j] = ReplaceComma(dataArr[j]);
                dataArr[j] = ReplaceEnter(dataArr[j]);
                //if (index >= 1340)
                //{
                //    Debug.Log("index = " + index);
                //    Debug.Log("subjectArr[" + j + "] = " + subjectArr[j]);
                //    Debug.Log("dataArr[" + j + "] = " + dataArr[j]);
                //}
                dataList[index].Add(subjectArr[j], dataArr[j]);

            }//for j
            index++;
        }//for i

        //Debug.Log("대화 갯수 = " + index);

        //interation list에 추가하기 -> id를 알기 위한 클래스 리스트
        for (int i = 0; i < dataList.Count; i++)
        {
            //dataList.Count 만큼 interaction 클래스 객체가 만들어짐.
            Interaction tempInteraction = new Interaction();

            for (int j = 0; j < dataList[i].Count; j++)
            {
                //(dataList[i])[subjectArr[j]]
                switch (subjectArr[j])
                {
                    case "사건":
                        try
                        {
                            tempInteraction.SetAct(int.Parse((dataList[i])[subjectArr[j]]));
                        }
                        catch
                        {
                            //Debug.Log("(dataList[" + i + "])[subjectArr[" + j + "]])" + (dataList[i])[subjectArr[j]] + "사건에서 오류발생");
                        }
                        break;

                    case "시간대":

                        //tempInteraction.SetTime(int.Parse((dataList[i])[subjectArr[j]]));
                        //tempInteraction.SetTime(((dataList[i])[subjectArr[j]]));

                        // 각 대화가 여러 시간대에서 나타날 수 있는 대화일 때를 포함한 작업
                        string tempTime = ((dataList[i])[subjectArr[j]]);
                        string[] tempTimeList;
                        if (tempTime.Contains(","))
                        {   // 여러개일 경우
                            tempTimeList = tempTime.Split(',');
                            tempInteraction.SetTime(tempTimeList);
                        }
                        else
                        {   // 1개일 경우
                            tempTimeList = new string[1];
                            tempTimeList[0] = tempTime;
                            tempInteraction.SetTime(tempTimeList);
                        }

                        break;

                    case "위치":

                        //tempInteraction.SetPosition(int.Parse((dataList[i])[subjectArr[j]]));
                        //tempInteraction.SetPosition(((dataList[i])[subjectArr[j]]));
                        string tempPosition = ((dataList[i])[subjectArr[j]]);
                        string[] tempPositionList;
                        if (tempPosition.Contains(","))
                        {   // 여러개일 경우
                            tempPositionList = tempPosition.Split(',');
                            tempInteraction.SetPosition(tempPositionList);
                        }
                        else
                        {   // 1개일 경우
                            tempPositionList = new string[1];
                            tempPositionList[0] = tempPosition;
                            tempInteraction.SetPosition(tempPositionList);
                        }
                        break;

                    case "대화 묶음":
                        try
                        {
                            tempInteraction.SetSetOfDesc(int.Parse((dataList[i])[subjectArr[j]]));
                        }
                        catch
                        {
                            //Debug.Log("(dataList[" + i + "])[subjectArr[" + j + "]])" + (dataList[i])[subjectArr[j]] + "대화 묶음에서 오류발생");
                        }
                        break;

                    case "id":
                        try
                        {
                            tempInteraction.SetId(int.Parse((dataList[i])[subjectArr[j]]));
                        }
                        catch
                        {
                            //Debug.Log("(dataList["+i+"])[subjectArr["+j+"]])" + (dataList[i])[subjectArr[j]] + "id에서 오류발생");
                        }
                        break;

                    case "startObject":
                        //tempInteraction.SetStartObject((dataList[i])[subjectArr[j]]);
                        string tempStartObject = (dataList[i])[subjectArr[j]];
                        string[] tempStartObjectList;
                        if (tempStartObject.Contains(","))
                        {   // 여러개일 경우
                            tempStartObjectList = tempStartObject.Split(',');
                            tempInteraction.SetStartObject(tempStartObjectList);
                        }
                        else
                        {   // 1개일 경우
                            tempStartObjectList = new string[1];
                            tempStartObjectList[0] = tempStartObject;
                            tempInteraction.SetStartObject(tempStartObjectList);
                        }
                        //if (tempStartObject.Equals("800"))
                        //{
                        //    Debug.Log(tempStartObject + "의 대화 적용 완료");
                        //}
                        break;

                    case "npcFrom":
                        try
                        {
                            tempInteraction.SetNpcFrom((dataList[i])[subjectArr[j]]);
                        }
                        catch
                        {
                            //Debug.Log("(dataList[" + i + "])[subjectArr[" + j + "]])" + (dataList[i])[subjectArr[j]] + "npcFrom에서 오류발생");
                        }
                        break;
                    /*
                case "npcTo":

                    tempInteraction.SetNpcTo((dataList[i])[subjectArr[j]]);
                    break;
                    */
                    case "desc":
                        try
                        {
                            tempInteraction.SetDesc((dataList[i])[subjectArr[j]]);
                        }
                        catch
                        {
                            //Debug.Log("(dataList[" + i + "])[subjectArr[" + j + "]])" + (dataList[i])[subjectArr[j]] + "desc에서 오류발생");
                        }
                        break;

                    case "반복성":
                        try
                        {
                            if (i == 3604)
                            {
                                int a = 1;
                            }

                            if ((status_Repeatability_DataList[i])[subjectArr[j]] != null)
                            {
                                int b = 1;
                            }
                            else if ((status_Repeatability_DataList[i])[subjectArr[j]].Equals(""))
                            {
                                int c = 1;
                            }
                            else if ((status_Repeatability_DataList[i])[subjectArr[j]] == null)
                            {
                                int d = 1;
                            }
                            else
                            {
                                int e = 1;
                            }

                            //tempInteraction.SetRepeatability((dataList[i])[subjectArr[j]]);
                            if ((status_Repeatability_DataList[i])[subjectArr[j]] != null)
                            {
                                // 저장된 파일에 해당 Interaction에 대한 정보가 있다면, 저장된 데이터를 설정
                                tempInteraction.SetRepeatability((status_Repeatability_DataList[i])[subjectArr[j]]);
                            }
                            else
                            {
                                // 저장된 파일에 해당 Interaction에 대한 정보가 없다면, 대화 데이터가 추가되었기 때문에, 새로 읽은 데이터를 설정
                                tempInteraction.SetRepeatability((dataList[i])[subjectArr[j]]);
                            }
                            //Debug.Log("(dataList[" + i + "])[subjectArr[" + j + "]])" + (status_Repeatability_DataList[i])[subjectArr[j]] + "반복성 추가");
                        }
                        catch
                        {
                            // 조건문 실행중에 null 비교 하면서 오류발생할 수 있음.
                            // 저장된 파일에 해당 Interaction에 대한 정보가 없다면, 대화 데이터가 추가되었기 때문에, 새로 읽은 데이터를 설정
                            tempInteraction.SetRepeatability((dataList[i])[subjectArr[j]]);
                            
                            //Debug.Log("(dataList[" + i + "])[subjectArr[" + j + "]]) 반복성에서 오류발생하여 init 데이터인 " + (dataList[i])[subjectArr[j]] + " 값이 설정됨");
                        }
                        break;

                    case "대사 조건":

                        string tempCondition = (dataList[i])[subjectArr[j]];

                        if (tempCondition.Contains(","))   // 여러개일 경우
                        {
                            string[] tempConditionList;
                            tempConditionList = tempCondition.Split(',');
                            tempInteraction.SetConditionOfDesc(tempConditionList);
                        }
                        else
                        {   // 1개이거나 없는 경우
                            string[] tempConditionList;
                            tempConditionList = new string[1];
                            tempConditionList[0] = tempCondition;
                            tempInteraction.SetConditionOfDesc(tempConditionList);
                        }

                        break;

                    case "status":
                        try
                        {
                            //tempInteraction.SetStatus(int.Parse((dataList[i])[subjectArr[j]]));
                            if ((status_Repeatability_DataList[i])[subjectArr[j]] != null)
                            {
                                // 저장된 파일에 해당 Interaction에 대한 정보가 있다면, 저장된 데이터를 설정
                                tempInteraction.SetStatus(int.Parse((status_Repeatability_DataList[i])[subjectArr[j]]));
                            }
                            else
                            {
                                // 저장된 파일에 해당 Interaction에 대한 정보가 없다면, 대화 데이터가 추가되었기 때문에, 새로 읽은 데이터를 설정
                                tempInteraction.SetStatus(int.Parse((dataList[i])[subjectArr[j]]));
                            }
                            //Debug.Log("(dataList[" + i + "])[subjectArr[" + j + "]])" + (status_Repeatability_DataList[i])[subjectArr[j]] + "status 추가");
                        }
                        catch
                        {
                            // 조건문 실행중에 null 비교 하면서 오류발생할 수 있음.
                            // 저장된 파일에 해당 Interaction에 대한 정보가 없다면, 대화 데이터가 추가되었기 때문에, 새로 읽은 데이터를 설정
                            tempInteraction.SetStatus(int.Parse((dataList[i])[subjectArr[j]]));

                            //Debug.Log("(dataList[" + i + "])[subjectArr[" + j + "]]) status에서 오류발생하여 init 데이터인 " + (dataList[i])[subjectArr[j]] + " 값이 설정됨");
                        }
                        break;

                    case "rewards":

                        //rewards는 여러개 일 수 있음. 그것은 보상을 얻을때 , 를 기점으로 나눌것
                        //Debug.Log("tempRewards = " + (dataList[i])[subjectArr[j]]);
                        string tempRewards = ReplaceComma((dataList[i])[subjectArr[j]]);
                        //string[] rewardsList = tempRewards.Split(',');
                        tempInteraction.SetRewards(tempRewards);
                        //tempInteraction.SetRewards((dataList[i])[subjectArr[j]]);
                        break;

                    case "parent":
                        try
                        {
                            tempInteraction.SetParent(int.Parse((dataList[i])[subjectArr[j]]));
                        }
                        catch
                        {
                            //Debug.Log("(dataList[" + i + "])[subjectArr[" + j + "]])" + (dataList[i])[subjectArr[j]] + "parent에서 오류발생");
                        }

                        break;
                    /* 삭제 예정(11/12) */
                    case "단서 루트 해금":

                        string tempRevealList = ReplaceComma((dataList[i])[subjectArr[j]]);
                        string[] revealList = tempRevealList.Split(',');
                        tempInteraction.SetRevealList(revealList);

                        break;
                    // 아래 두 부분은, 나중에 이벤트 처리할 때 수정할 것(1223)
                    case "발생 여부":
                        try
                        {
                            tempInteraction.SetOccurrence((dataList[i])[subjectArr[j]]);
                        }
                        catch
                        {
                            //Debug.Log("(dataList[" + i + "])[subjectArr[" + j + "]])" + (dataList[i])[subjectArr[j]] + "발생 여부에서 오류발생");
                        }

                        break;

                    case "새로운 이벤트 발생":
                        string tempEvent = (dataList[i])[subjectArr[j]];

                        try
                        {
                            //tempInteraction.SetEventIndexToOccur(((dataList[i])[subjectArr[j]]));

                            if (tempEvent.Contains(","))   // 여러개일 경우
                            {
                                string[] tempEventList;
                                tempEventList = tempEvent.Split(',');
                                tempInteraction.SetEventIndexToOccur(tempEventList);
                            }
                            else
                            {   // 1개이거나 없는 경우
                                string[] tempEventList;
                                tempEventList = new string[1];
                                tempEventList[0] = tempEvent;
                                tempInteraction.SetEventIndexToOccur(tempEventList);
                            }

                            /* 굳이 이 로직을 실행할 필요 없음 (1월 27일 메모)
                            // 대화로 인해 발생할 이벤트가 존재한다면
                            if (!((dataList[i])[subjectArr[j]]).Equals(""))
                            {
                                // npc와 관련된(생성 및 삭제) 새로운 이벤트가 있다면 추가
                                EventManager.instance.AddToEventIndexList(tempInteraction.GetEventIndexToOccur());
                            }
                            */
                        }
                        catch
                        {
                            //Debug.Log("(dataList[" + i + "])[subjectArr[" + j + "]])" + (dataList[i])[subjectArr[j]] + "새로운 이벤트에서 오류발생");
                        }
                        break;

                    default:
                        continue;
                }//switch
            }//for j

            //추출해서 적용시킨 interaction 클래스를 리스트에 추가
            interactionLists.Add(tempInteraction);
        }//for i

        //Debug.Log("index = " + index);
    } //ParsingConversationCSV()

    /* 엔딩 테이블을 파싱하는 함수 */
    public void ParsingEndingCSV(object dataPath)
    {
        
        endingDataList = new Dictionary<int, Dictionary<string, string>>();
        endingInteractionLists = new List<Interaction>();
        startObjectListsForEnding = new List<string>();

        string textAsset = File.ReadAllText(dataPath.ToString(), System.Text.Encoding.UTF8);

        if (GameManager.instance.isEncrypted)
            textAsset = GameManager.instance.DecryptData(textAsset);

        //전체 데이터 줄바꿈단위로 분리 (csv파일의 한 문장 끝에는 \r\n이 붙어있음)
        string[] stringArr = textAsset.Split(new string[] { "줄바꿈\r\n" }, System.StringSplitOptions.None);
        string[] subjectArr = stringArr[0].Split(',');      //속성에 해당하는 첫째줄 분리

        int index = 0;

        //맨 마지막 줄은 한 줄 띄워져있으니 생략하기위해 길이 - 1 해줌
        //첫번째줄 속성줄을 무시하기위해 i = 1 부터 시작
        for (int i = 1; i < stringArr.Length - 1; i++)
        {
            //두번째 줄부터 ,를 기준으로 쪼갬
            string[] dataArr = stringArr[i].Split(',');

            //해당 index가 dictionary에 없으면 추가
            if (!endingDataList.ContainsKey(index))
                endingDataList.Add(index, new Dictionary<string, string>());

            //메인 dictionary에는 index의 키값을 가지는 내부 dictionary가 있을것이다.
            //내부 dictionary에 각 속성들의 값을 대입하기위해 for문을 돌린다.
            for (int j = 0; j < dataArr.Length; j++)
            {
                /* """ -> " , $ -> , 변환해서 데이터 넣기 */
                dataArr[j] = ReplaceComma(dataArr[j]);
                dataArr[j] = ReplaceEnter(dataArr[j]);
                endingDataList[index].Add(subjectArr[j], dataArr[j]);
            }//for j
            index++;
        }//for i

        //Debug.Log("대화 갯수 = " + index);

        //interation list에 추가하기 -> id를 알기 위한 클래스 리스트
        for (int i = 0; i < endingDataList.Count; i++)
        {
            //dataList.Count 만큼 interaction 클래스 객체가 만들어짐.
            Interaction tempInteraction = new Interaction();

            for (int j = 0; j < endingDataList[i].Count; j++)
            {
                //(dataList[i])[subjectArr[j]]
                switch (subjectArr[j])
                {
                    case "사건":
                        try
                        {
                            tempInteraction.SetAct(int.Parse((endingDataList[i])[subjectArr[j]]));
                        }
                        catch
                        {
                            Debug.Log("(endingDataList[" + i + "])[subjectArr[" + j + "]])" + (endingDataList[i])[subjectArr[j]] + "사건에서 오류발생");
                        }
                        break;
                        
                    case "대화 묶음":
                        try
                        {
                            tempInteraction.SetSetOfDesc(int.Parse((endingDataList[i])[subjectArr[j]]));
                        }
                        catch
                        {
                            Debug.Log("(endingDataList[" + i + "])[subjectArr[" + j + "]])" + (endingDataList[i])[subjectArr[j]] + "대화 묶음에서 오류발생");
                        }
                        break;

                    case "id":
                        try
                        {
                            tempInteraction.SetId(int.Parse((endingDataList[i])[subjectArr[j]]));
                        }
                        catch
                        {
                            Debug.Log("(endingDataList[" + i+"])[subjectArr["+j+"]])" + (endingDataList[i])[subjectArr[j]] + "id에서 오류발생");
                        }
                        break;

                    case "startObject":
                        try
                        {
                            //tempInteraction.SetStartObject((dataList[i])[subjectArr[j]]);
                            string tempStartObject = (endingDataList[i])[subjectArr[j]];
                            string[] tempStartObjectList;

                            // 1개일 경우
                            tempStartObjectList = new string[1];
                            tempStartObjectList[0] = tempStartObject;
                            tempInteraction.SetStartObject(tempStartObjectList);

                            if (!startObjectListsForEnding.Contains(tempStartObject))
                                startObjectListsForEnding.Add(tempStartObject);
                        }
                        catch
                        {
                            Debug.Log("(endingDataList[" + i + "])[subjectArr[" + j + "]])" + (endingDataList[i])[subjectArr[j]] + "startObject에서 오류발생");
                        }
                        break;

                    case "npcFrom":
                        try
                        {
                            tempInteraction.SetNpcFrom((endingDataList[i])[subjectArr[j]]);
                        }
                        catch
                        {
                            Debug.Log("(endingDataList[" + i + "])[subjectArr[" + j + "]])" + (endingDataList[i])[subjectArr[j]] + "npcFrom에서 오류발생");
                        }
                        break;

                    case "desc":
                        try
                        {
                            tempInteraction.SetDesc((endingDataList[i])[subjectArr[j]]);
                        }
                        catch
                        {
                            Debug.Log("(endingDataList[" + i + "])[subjectArr[" + j + "]])" + (endingDataList[i])[subjectArr[j]] + "desc에서 오류발생");
                        }
                        break;
                        
                    case "parent":
                        try
                        {
                            tempInteraction.SetParent(int.Parse((endingDataList[i])[subjectArr[j]]));
                        }
                        catch
                        {
                            Debug.Log("(endingDataList[" + i + "])[subjectArr[" + j + "]])" + (endingDataList[i])[subjectArr[j]] + "parent에서 오류발생");
                        }

                        break;

                    default:
                        continue;
                }//switch
            }//for j

            //추출해서 적용시킨 interaction 클래스를 리스트에 추가
            endingInteractionLists.Add(tempInteraction);
        }//for i

        /* interactionList에 있는 내용들 출력(debug) */
        //for (int i = 0; i < endingInteractionLists.Count; i++)
        //{
        //    Debug.Log((i + 1) + "번째 데이터" +
        //        "\nact : " + endingInteractionLists[i].GetAct() +
        //        "\nsetOfDesc : " + endingInteractionLists[i].GetSetOfDesc() +
        //        "\nid : " + endingInteractionLists[i].GetId() +
        //        "\nstartObject : " + endingInteractionLists[i].GetStartObject() +
        //        "\nnpcFrom : " + endingInteractionLists[i].GetNpcFrom() +
        //        "\ndesc : " + endingInteractionLists[i].GetDesc() +
        //        "\nparent : " + endingInteractionLists[i].GetParent());
        //}

    } //ParsingEndingCSV()

    /* 대화 테이블을 파싱하는 함수 */
    public void ParsingConversationCSV(object dataPath)
    {
        dataList = new Dictionary<int, Dictionary<string, string>>();
        interactionLists = new List<Interaction>();

        //TextAsset textAsset = Resources.Load<TextAsset>("Data/Interaction");
        //string textAsset = File.ReadAllText(Application.streamingAssetsPath + "/Data/Interaction_ver1_5.csv");
        
        string textAsset = File.ReadAllText(dataPath.ToString(), System.Text.Encoding.UTF8);
        
        if(GameManager.instance.isEncrypted)
            textAsset = GameManager.instance.DecryptData(textAsset);

        //전체 데이터 줄바꿈단위로 분리 (csv파일의 한 문장 끝에는 \r\n이 붙어있음)
        //string[] stringArr = textAsset.text.Split(new string[] { "\r\n" }, System.StringSplitOptions.None);
        string[] stringArr = textAsset.Split(new string[] { "줄바꿈\r\n" }, System.StringSplitOptions.None);
        string[] subjectArr = stringArr[0].Split(',');      //속성에 해당하는 첫째줄 분리

        int index = 0;

        //맨 마지막 줄은 한 줄 띄워져있으니 생략하기위해 길이 - 1 해줌
        //첫번째줄 속성줄을 무시하기위해 i = 1 부터 시작
        for (int i = 1; i < stringArr.Length - 1; i++)
        {
            //두번째 줄부터 ,를 기준으로 쪼갬
            string[] dataArr = stringArr[i].Split(',');

            //int index = int.Parse(dataArr[0]);  //첫번째 속성인 id값을 int형으로 집어넣기

            /* FindIndex가 0부터 반환하면 0, 1부터 반환하면 1로 고쳐야함 */
            //int index = 0; // 그냥 0부터 차례대로 박아넣기 // int.Parse(dataArr[4]);  //5번째 속성인 id값을 int형으로 집어넣기

            //해당 index가 dictionary에 없으면 추가
            if (!dataList.ContainsKey(index))
                dataList.Add(index, new Dictionary<string, string>());

            //메인 dictionary에는 index의 키값을 가지는 내부 dictionary가 있을것이다.
            //내부 dictionary에 각 속성들의 값을 대입하기위해 for문을 돌린다.
            for (int j = 0; j < dataArr.Length; j++)
            {
                /* """ -> " , $ -> , 변환해서 데이터 넣기 */
                //dataArr[j] = ReplaceDoubleQuotationMark(dataArr[j]);
                //dataArr[j] = RemoveDoubleQuotationMark(dataArr[j]);     // 대화에 줄바꿈이 있을경우, 양끝에 "가 붙은걸 없애기
                dataArr[j] = ReplaceComma(dataArr[j]);
                dataArr[j] = ReplaceEnter(dataArr[j]);
                //if (index >= 1340)
                //{
                //    Debug.Log("index = " + index);
                //    Debug.Log("subjectArr[" + j + "] = " + subjectArr[j]);
                //    Debug.Log("dataArr[" + j + "] = " + dataArr[j]);
                //}
                dataList[index].Add(subjectArr[j], dataArr[j]);

            }//for j
            index++;
        }//for i

        //Debug.Log("대화 갯수 = " + index);

        //interation list에 추가하기 -> id를 알기 위한 클래스 리스트
        for (int i = 0; i < dataList.Count; i++)
        {
            //dataList.Count 만큼 interaction 클래스 객체가 만들어짐.
            Interaction tempInteraction = new Interaction();

            for (int j = 0; j < dataList[i].Count; j++)
            {
                //(dataList[i])[subjectArr[j]]
                switch (subjectArr[j])
                {
                    case "사건":
                        try
                        {
                            tempInteraction.SetAct(int.Parse((dataList[i])[subjectArr[j]]));
                        }
                        catch
                        {
                            //Debug.Log("(dataList[" + i + "])[subjectArr[" + j + "]])" + (dataList[i])[subjectArr[j]] + "사건에서 오류발생");
                        }
                        break;

                    case "시간대":

                        //tempInteraction.SetTime(int.Parse((dataList[i])[subjectArr[j]]));
                        //tempInteraction.SetTime(((dataList[i])[subjectArr[j]]));

                        // 각 대화가 여러 시간대에서 나타날 수 있는 대화일 때를 포함한 작업
                        string tempTime = ((dataList[i])[subjectArr[j]]);
                        string[] tempTimeList;
                        if (tempTime.Contains(",")) 
                        {   // 여러개일 경우
                            tempTimeList = tempTime.Split(',');
                            tempInteraction.SetTime(tempTimeList);
                        }
                        else
                        {   // 1개일 경우
                            tempTimeList = new string[1];
                            tempTimeList[0] = tempTime;
                            tempInteraction.SetTime(tempTimeList);
                        }

                        break;

                    case "위치":

                        //tempInteraction.SetPosition(int.Parse((dataList[i])[subjectArr[j]]));
                        //tempInteraction.SetPosition(((dataList[i])[subjectArr[j]]));
                        string tempPosition = ((dataList[i])[subjectArr[j]]);
                        string[] tempPositionList;
                        if (tempPosition.Contains(","))
                        {   // 여러개일 경우
                            tempPositionList = tempPosition.Split(',');
                            tempInteraction.SetPosition(tempPositionList);
                        }
                        else
                        {   // 1개일 경우
                            tempPositionList = new string[1];
                            tempPositionList[0] = tempPosition;
                            tempInteraction.SetPosition(tempPositionList);
                        }
                        break;

                    case "대화 묶음":
                        try
                        {
                            tempInteraction.SetSetOfDesc(int.Parse((dataList[i])[subjectArr[j]]));
                        }
                        catch
                        {
                            //Debug.Log("(dataList[" + i + "])[subjectArr[" + j + "]])" + (dataList[i])[subjectArr[j]] + "대화 묶음에서 오류발생");
                        }
                        break;

                    case "id":
                        try
                        {
                            tempInteraction.SetId(int.Parse((dataList[i])[subjectArr[j]]));
                        }
                        catch
                        {
                            //Debug.Log("(dataList["+i+"])[subjectArr["+j+"]])" + (dataList[i])[subjectArr[j]] + "id에서 오류발생");
                        }
                        break;

                    case "startObject":
                        //tempInteraction.SetStartObject((dataList[i])[subjectArr[j]]);
                        string tempStartObject = (dataList[i])[subjectArr[j]];
                        string[] tempStartObjectList;
                        if (tempStartObject.Contains(","))
                        {   // 여러개일 경우
                            tempStartObjectList = tempStartObject.Split(',');
                            tempInteraction.SetStartObject(tempStartObjectList);
                        }
                        else
                        {   // 1개일 경우
                            tempStartObjectList = new string[1];
                            tempStartObjectList[0] = tempStartObject;
                            tempInteraction.SetStartObject(tempStartObjectList);
                        }
                        //if (tempStartObject.Equals("800"))
                        //{
                        //    Debug.Log(tempStartObject + "의 대화 적용 완료");
                        //}
                        break;

                    case "npcFrom":
                        try
                        {
                            tempInteraction.SetNpcFrom((dataList[i])[subjectArr[j]]);
                        }
                        catch
                        {
                            //Debug.Log("(dataList[" + i + "])[subjectArr[" + j + "]])" + (dataList[i])[subjectArr[j]] + "npcFrom에서 오류발생");
                        }
                        break;
                    /*
                case "npcTo":

                    tempInteraction.SetNpcTo((dataList[i])[subjectArr[j]]);
                    break;
                    */
                    case "desc":
                        try
                        {
                            tempInteraction.SetDesc((dataList[i])[subjectArr[j]]);
                        }
                        catch
                        {
                            //Debug.Log("(dataList[" + i + "])[subjectArr[" + j + "]])" + (dataList[i])[subjectArr[j]] + "desc에서 오류발생");
                        }
                        break;

                    case "반복성":
                        try
                        {
                            tempInteraction.SetRepeatability((dataList[i])[subjectArr[j]]);
                        }
                        catch
                        {
                            //Debug.Log("(dataList[" + i + "])[subjectArr[" + j + "]])" + (dataList[i])[subjectArr[j]] + "반복성에서 오류발생");
                        }
                        break;

                    case "대사 조건":

                        string tempCondition = (dataList[i])[subjectArr[j]];

                        if (tempCondition.Contains(","))   // 여러개일 경우
                        {
                            string[] tempConditionList;
                            tempConditionList = tempCondition.Split(',');
                            tempInteraction.SetConditionOfDesc(tempConditionList);
                        }
                        else
                        {   // 1개이거나 없는 경우
                            string[] tempConditionList;
                            tempConditionList = new string[1];
                            tempConditionList[0] = tempCondition;
                            tempInteraction.SetConditionOfDesc(tempConditionList);
                        }

                        break;

                    case "status":
                        try
                        {
                            tempInteraction.SetStatus(int.Parse((dataList[i])[subjectArr[j]]));
                        }
                        catch
                        {
                            //Debug.Log("(dataList[" + i + "])[subjectArr[" + j + "]])" + (dataList[i])[subjectArr[j]] + "status에서 오류발생");
                        }
                        break;

                    case "rewards":

                        //rewards는 여러개 일 수 있음. 그것은 보상을 얻을때 , 를 기점으로 나눌것
                        //Debug.Log("tempRewards = " + (dataList[i])[subjectArr[j]]);
                        string tempRewards = ReplaceComma((dataList[i])[subjectArr[j]]);
                        //string[] rewardsList = tempRewards.Split(',');
                        tempInteraction.SetRewards(tempRewards);
                        //tempInteraction.SetRewards((dataList[i])[subjectArr[j]]);
                        break;

                    case "parent":
                        try
                        {
                            tempInteraction.SetParent(int.Parse((dataList[i])[subjectArr[j]]));
                        }
                        catch
                        {
                            //Debug.Log("(dataList[" + i + "])[subjectArr[" + j + "]])" + (dataList[i])[subjectArr[j]] + "parent에서 오류발생");
                        }

                        break;
                    /* 삭제 예정(11/12) */
                    case "단서 루트 해금":

                        string tempRevealList = ReplaceComma((dataList[i])[subjectArr[j]]);
                        string[] revealList = tempRevealList.Split(',');
                        tempInteraction.SetRevealList(revealList);

                        break;
                    // 아래 두 부분은, 나중에 이벤트 처리할 때 수정할 것(1223)
                    case "발생 여부":
                        try
                        {
                            tempInteraction.SetOccurrence((dataList[i])[subjectArr[j]]);
                        }
                        catch
                        {
                            //Debug.Log("(dataList[" + i + "])[subjectArr[" + j + "]])" + (dataList[i])[subjectArr[j]] + "발생 여부에서 오류발생");
                        }

                        break;

                    case "새로운 이벤트 발생":
                        string tempEvent = (dataList[i])[subjectArr[j]];

                        try
                        {
                            //tempInteraction.SetEventIndexToOccur(((dataList[i])[subjectArr[j]]));

                            if (tempEvent.Contains(","))   // 여러개일 경우
                            {
                                string[] tempEventList;
                                tempEventList = tempEvent.Split(',');
                                tempInteraction.SetEventIndexToOccur(tempEventList);
                            }
                            else
                            {   // 1개이거나 없는 경우
                                string[] tempEventList;
                                tempEventList = new string[1];
                                tempEventList[0] = tempEvent;
                                tempInteraction.SetEventIndexToOccur(tempEventList);
                            }

                            /* 굳이 이 로직을 실행할 필요 없음 (1월 27일 메모)
                            // 대화로 인해 발생할 이벤트가 존재한다면
                            if (!((dataList[i])[subjectArr[j]]).Equals(""))
                            {
                                // npc와 관련된(생성 및 삭제) 새로운 이벤트가 있다면 추가
                                EventManager.instance.AddToEventIndexList(tempInteraction.GetEventIndexToOccur());
                            }
                            */
                        }
                        catch
                        {
                            //Debug.Log("(dataList[" + i + "])[subjectArr[" + j + "]])" + (dataList[i])[subjectArr[j]] + "새로운 이벤트에서 오류발생");
                        }
                        break;

                    default:
                        continue;
                }//switch
            }//for j

            //추출해서 적용시킨 interaction 클래스를 리스트에 추가
            interactionLists.Add(tempInteraction);
        }//for i

        /* interactionList에 있는 내용들 출력(debug) */
        //for (int i = 0; i < interactionLists.Count; i++)
        //{

        //    Debug.Log((i + 1) + "번째 데이터" +
        //        "\nact : " + interactionLists[i].GetAct() +
        //        "\ntime : " + interactionLists[i].GetTime() +
        //        "\nposition : " + interactionLists[i].GetPosition() +
        //        "\nsetOfDesc : " + interactionLists[i].GetSetOfDesc() +
        //        "\nid : " + interactionLists[i].GetId() +
        //        "\nstartObject : " + interactionLists[i].GetStartObject() +
        //        "\nnpcFrom : " + interactionLists[i].GetNpcFrom() +
        //        "\nnpcTo : " + interactionLists[i].GetNpcTo() +
        //        "\ndesc : " + interactionLists[i].GetDesc() +
        //        "\nrepeatability : " + interactionLists[i].GetRepeatability() +
        //        "\nstatus : " + interactionLists[i].GetStatus() +
        //        "\nparent : " + interactionLists[i].GetParent());

        //    string rewardsList = interactionLists[i].GetRewards();

        //    if (rewardsList.Contains(","))
        //    {
        //        string[] rewardArr = rewardsList.Split(',');
        //        for (int j = 0; j < rewardArr.Length; j++)
        //        {
        //            Debug.Log((j + 1) + "번째로 획득할 단서 : " + rewardArr[j]);
        //        }
        //    }
        //    else
        //    {
        //            Debug.Log("획득할 단서 : " + rewardsList);
        //    }

        //    for (int j = 0; j < interactionLists[i].GetConditionOfDesc().Length; j++)
        //        Debug.Log((j + 1) + "번째 conditionOfDesc : " + interactionLists[i].GetConditionOfDesc()[j]);

        //    for (int j = 0; j < interactionLists[i].GetRevealList().Length; j++)
        //        Debug.Log((j + 1) + "번째 revealList : " + interactionLists[i].GetRevealList()[j]);

        //    //if (rewardsList.Length >= 2)
        //    //{
        //    //    //rewards가 여러개 있는 데이터일 경우
        //    //    for (int j = 0; j < rewardsList.Length; j++)
        //    //    {
        //    //        Debug.Log((j + 1) + "번째 rewards : " + rewardsList[j]);
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    Debug.Log("rewards : " + rewardsList[0]);
        //    //}

        //    //Debug.Log("parent : " + interactionLists[i].GetParent());
        //}

        //Debug.Log((dataList[0])["npc"]);
    } //ParsingConversationCSV()

    /* 단서 테이블을 파싱하는 함수 */
    public void ParsingClueDataCSV(string dataPath)
    {
        clueList = new Dictionary<int, Dictionary<string, string>>();
        clueStructureLists = new List<ClueStructure>();
        arnoldEndingClueLists = new List<string>();
        andrenEndingClueLists = new List<string>();
        trueEndingClueLists = new List<string>();
        
        string textAsset = File.ReadAllText(dataPath, System.Text.Encoding.UTF8);

        if (GameManager.instance.isEncrypted)
            textAsset = GameManager.instance.DecryptData(textAsset); // 복호화가 왜 안돼?

        //전체 데이터 줄바꿈단위로 분리 (csv파일의 한 문장 끝에는 \r\n이 붙어있음)
        string[] stringArr = textAsset.Split(new string[] { "\r\n" }, System.StringSplitOptions.None);
        string[] subjectArr = stringArr[0].Split(',');      //속성에 해당하는 첫째줄 분리

        int index = 0;
        //맨 마지막 줄은 한 줄 띄워져있으니 생략하기위해 길이 - 1 해줌
        //첫번째줄 속성줄을 무시하기위해 i = 1 부터 시작
        for (int i = 1; i < stringArr.Length - 1; i++)
        {
            //두번째 줄부터 ,를 기준으로 쪼갬
            string[] dataArr = stringArr[i].Split(',');

            /* FindIndex가 0부터 반환하면 0, 1부터 반환하면 1로 고쳐야함 */

            //해당 index가 dictionary에 없으면 추가
            if (!clueList.ContainsKey(index))
                clueList.Add(index, new Dictionary<string, string>());
            
            //메인 dictionary에는 index의 키값을 가지는 내부 dictionary가 있을것이다.
            //내부 dictionary에 각 속성들의 값을 대입하기위해 for문을 돌린다.
            for (int j = 0; j < dataArr.Length; j++)
            {
                /* """ -> " && s -> , 변환해서 데이터 넣기 */
                dataArr[j] = ReplaceDoubleQuotationMark(dataArr[j]);
                dataArr[j] = ReplaceComma(dataArr[j]);
                dataArr[j] = ReplaceEnter(dataArr[j]);
                //Debug.Log("index = " + index);
                //Debug.Log("subjectArr[" + j + "] = " + subjectArr[j]);
                //Debug.Log("dataArr[" + j + "] = " + dataArr[j]);
                clueList[index].Add(subjectArr[j], dataArr[j]);

            }//for j
            index++;
        }//for i

        //interation list에 추가하기 -> id를 알기 위한 클래스 리스트
        for (int i = 0; i < clueList.Count; i++)
        {
            //dataList.Count 만큼 interaction 클래스 객체가 만들어짐.
            ClueStructure tempClueStructure = new ClueStructure();

            for (int j = 0; j < clueList[i].Count; j++)
            {
                //(clueList[i])[subjectArr[j]]
                switch (subjectArr[j])
                {
                    case "S.no":

                        //tempClueStructure.SetSpecialNum(((clueList[i])[subjectArr[j]]));
                        string tempSpecialNum = (clueList[i])[subjectArr[j]];
                        string[] tempSpecialNumList;

                        if (tempSpecialNum.Contains(","))
                        {   // 여러개일때
                            tempSpecialNumList = tempSpecialNum.Split(',');
                            tempClueStructure.SetSpecialNum(tempSpecialNumList);
                        }
                        else
                        {   // 1개일때
                            tempSpecialNumList = new string[1];
                            tempSpecialNumList[0] = tempSpecialNum;
                            tempClueStructure.SetSpecialNum(tempSpecialNumList);
                        }
                        break;

                    case "E.no":
                        
                        tempClueStructure.SetEventNum(((clueList[i])[subjectArr[j]]));
                        break;

                    case "id":

                        tempClueStructure.SetId(int.Parse((clueList[i])[subjectArr[j]]));
                        break;

                    case "rewards": // 단서 이름

                        tempClueStructure.SetClueName((clueList[i])[subjectArr[j]]);
                        break;

                    case "사건":

                        tempClueStructure.SetNumOfAct((clueList[i])[subjectArr[j]]);
                        break;

                    case "시간대":

                        tempClueStructure.SetTimeSlot((clueList[i])[subjectArr[j]]);
                        break;

                    case "획득 위치 1":

                        //tempClueStructure.SetObtainPos1((clueList[i])[subjectArr[j]]);
                        string tempObtainPos1 = (clueList[i])[subjectArr[j]];
                        string[] tempObtainPos1List;

                        if (tempObtainPos1.Contains(","))
                        {   // 여러개일때
                            tempObtainPos1List = tempObtainPos1.Split(',');
                            tempClueStructure.SetObtainPos1(tempObtainPos1List);
                        }
                        else
                        {   // 1개일때
                            tempObtainPos1List = new string[1];
                            tempObtainPos1List[0] = tempObtainPos1;
                            tempClueStructure.SetObtainPos1(tempObtainPos1List);
                        }

                        break;

                    case "획득 위치 2":
                        
                        tempClueStructure.SetObtainPos2((clueList[i])[subjectArr[j]]);
                        break;

                    case "desc2":

                        tempClueStructure.SetDesc((clueList[i])[subjectArr[j]]);
                        break;

                    default:
                        continue;
                }//switch
            }//for j

            //추출해서 적용시킨 ClueStructure 클래스를 리스트에 추가
            clueStructureLists.Add(tempClueStructure);

            if (tempClueStructure.GetNumSpecialNum() > 0)
            {
                for (int k = 0; k < tempClueStructure.GetSpecialNum().Length; k++)
                {
                    if (tempClueStructure.GetSpecialNum()[k].Equals("아놀드"))
                    {
                        if (!arnoldEndingClueLists.Contains(tempClueStructure.GetClueName()))
                        {
                            arnoldEndingClueLists.Add(tempClueStructure.GetClueName());
                        }
                    }

                    if (tempClueStructure.GetSpecialNum()[k].Equals("안드렌"))
                    {
                        if (!andrenEndingClueLists.Contains(tempClueStructure.GetClueName()))
                        {
                            andrenEndingClueLists.Add(tempClueStructure.GetClueName());
                        }
                    }

                    if (tempClueStructure.GetSpecialNum()[k].Equals("메르테"))
                    {
                        if (!trueEndingClueLists.Contains(tempClueStructure.GetClueName()))
                        {
                            trueEndingClueLists.Add(tempClueStructure.GetClueName());
                        }
                    }
                } // for k
            } // if

        }//for i
    }//ParsingClueDataCSV()

    /* 단서 관련 테이블은 저장할 필요가 없다. 변하는게 없기 때문 */
    /* 현재까지 진행된 csv 파일 저장하기 */
    public void SaveCSVData()
    {
        string attributeString = "사건,시간대,위치,대화 묶음,id,startObject,npcFrom,desc,반복성,대사 조건,status,parent,rewards,단서 루트 해금,발생 여부,새로운 이벤트 발생,줄바꿈\r\n";
        string conversationData = "";

        for (int i = 0; i < interactionLists.Count; i++)
        {
            conversationData += (
                (interactionLists[i].GetAct() + ",") +
                (RecoverComma(interactionLists[i].GetTime()) + ",") +
                (RecoverComma(interactionLists[i].GetPosition()) + ",") +
                (interactionLists[i].GetSetOfDesc() + ",") +
                (interactionLists[i].GetId() + ",") +
                (RecoverComma(interactionLists[i].GetStartObject()) + ",") +
                (interactionLists[i].GetNpcFrom() + ",") +
                (RecoverEnter(RecoverComma(interactionLists[i].GetDesc())) + ",") +
                (interactionLists[i].GetRepeatability() + ",") +
                (RecoverComma(interactionLists[i].GetConditionOfDesc()) + ",") +
                (interactionLists[i].GetStatus() + ",") +
                (interactionLists[i].GetParent() + ",") +
                (interactionLists[i].GetRewards() + ",") +
                (RecoverComma(interactionLists[i].GetRevealList()) + ",") +
                (interactionLists[i].GetOccurrence() + ",") +
                (RecoverComma(interactionLists[i].GetEventIndexToOccur()) + ",") +
                "줄바꿈\r\n");
        }

        if (GameManager.instance.isEncrypted)
        {
            // 생성된 세이브 파일 암호화
            string resultString = GameManager.instance.EncryptData(attributeString + conversationData);
            File.WriteAllText(playerConversationDataPath, resultString, System.Text.Encoding.UTF8);
        }
        else
        {
            // 세이브 파일 생성
            File.WriteAllText(playerConversationDataPath, attributeString + conversationData, System.Text.Encoding.UTF8);
        }
    }

    /* 저장된 csv 파일 불러오기 */
    public void LoadCSVData()
    {
        try
        {
            string conversationDataPath = Application.streamingAssetsPath + "/Data/PlayerConversation.csv";
            string clueDataPath = Application.streamingAssetsPath + "/Data/단서.csv";
            //ParsingConversationCSV(conversationDataPath);
            GetInfoParsingConversationCSV(initConversationDataPath, conversationDataPath);
            ParsingClueDataCSV(clueDataPath);
        }
        catch
        {
            Debug.Log("PlayerConversation.csv, 단서.csv 로드 에러");
        }
    }

    // 세이브 파일이 있는지 체크
    public bool CheckSaveData()
    {
        string conversationDataPath = Application.streamingAssetsPath + "/Data/PlayerConversation.csv";
        string eventVariableDataPath = Application.streamingAssetsPath + "/Data/PlayerEventVariable.json";
        string playerInfoDataPath = Application.streamingAssetsPath + "/Data/PlayerInfo.json";
        string initConversationDataPath = Application.streamingAssetsPath + "/Data/전체_테이블.csv";
        string clueDataPath = Application.streamingAssetsPath + "/Data/단서.csv";
        
        if (GameManager.instance.GetGameState() == GameManager.GameState.PastGame_Loaded)
        {
            if (File.Exists(initConversationDataPath) == false || File.Exists(clueDataPath) == false || File.Exists(conversationDataPath) == false
                || File.Exists(eventVariableDataPath) == false || File.Exists(playerInfoDataPath) == false)
                return false;
            else
                return true;
        }
        else if (GameManager.instance.GetGameState() == GameManager.GameState.NewGame_Loaded)
        {
            if (File.Exists(initConversationDataPath) == false || File.Exists(clueDataPath) == false)
                return false;
            else
                return true;
        }
        else
        {
            return false;
        }
    }

    /* """ -> " */
    public string ReplaceDoubleQuotationMark(string text)
    {
        if (text.Contains("\"\"\""))
        {
            //csv 파일에 있는 문장 데이터를 엑셀 상에서 "를 넣고 저장하면 
            //"가 3개가 붙기 때문에, 텍스트 출력할때 보기좋게 1개로 줄인다.
            text = text.Replace("\"\"\"", "\"");
            
            return text;
        }
        else
            return text;
    }

    public string RecoverDoubleQuotationMark(string text)
    {
        if (text.Contains("\""))
        {
            text = text.Replace("\"", "\"\"\"");

            return text;
        }
        else
            return text;
    }

    public string RemoveDoubleQuotationMark(string text)
    {
        if (text.Contains("\""))
        {
            text = text.Replace("\"", "");

            return text;
        }
        else
            return text;
    }
    

    /* $ -> , */
    public string ReplaceComma(string text)
    {
        if (text.Contains("$"))
        {
            //현재 csv 파일에서 쉼표를 표현하기위해서는 대체제로 $를 사용했으니, 바꿔줘야한다.
            text = text.Replace("$", ",");
            
            return text;
        }
        else
            return text;
    }

    public string RecoverComma(string text)
    {
        if (text.Contains(","))
        {
            //현재 csv 파일에서 쉼표를 표현하기위해서는 대체제로 $를 사용했으니, 바꿔줘야한다.
            text = text.Replace(",", "$");

            return text;
        }
        else
            return text;
    }

    public string RecoverComma(string[] text)
    {
        string tempText = "";

        for (int i = 0; i < text.Length; i++)
        {
            tempText += text[i];

            // 끝이 아닐 경우 $를 붙여서 쉼표가 있음을 나타냄.
            if (i != (text.Length - 1))
            {
                tempText += "$";
            }
        }

        return tempText;
    }

    /* # -> '\n' */
    public string ReplaceEnter(string text)
    {
        if (text.Contains("#"))
        {
            text = text.Replace("#", "\n");

            return text;
        }
        else
            return text;
    }

    public string RecoverEnter(string text)
    {
        if (text.Contains("\n"))
        {
            text = text.Replace("\n", "#");

            return text;
        }
        else
            return text;
    }


    public ref Dictionary<int, Dictionary<string, string>> GetDataList()
    {
        return ref dataList;
    }

    public ref Dictionary<int, Dictionary<string, string>> GetEndingDataList()
    {
        return ref endingDataList;
    }

    public ref List<Interaction> GetInteractionLists()
    {
        return ref interactionLists;
    }

    public ref List<Interaction> GetEndingInteractionLists()
    {
        return ref endingInteractionLists;
    }

    public ref List<string> GetEndingStartObjectLists()
    {
        return ref startObjectListsForEnding;
    }

    public Dictionary<int, Dictionary<string, string>> GetClueList()
    {
        return clueList;
    }

    public List<ClueStructure> GetClueStructureLists()
    {
        return clueStructureLists;
    }

    public bool CompleteLoadFile()
    {
        if (interactionLists == null)
            return false;

        if (interactionLists.Count >= 3604)
            return true;
        else
            return false;
    }

    public List<string> GetArnoldEndingClueLists()
    {
        return arnoldEndingClueLists;
    }

    public List<string> GetAndrenEndingClueLists()
    {
        return andrenEndingClueLists;
    }

    public List<string> GetTrueEndingClueLists()
    {
        return trueEndingClueLists;
    }

    public void SetDiaryContents()
    {
        int startSetOfDescNum = 1400;

        while (true)
        {
            // 1400 부터 찾는다.
            List<Interaction> tempInteractionLists = interactionLists.FindAll(x => x.GetSetOfDesc() == startSetOfDescNum);

            if (tempInteractionLists.Count == 0)
                break;

            string tempDiaryContent = "";

            for (int i = 0; i < tempInteractionLists.Count; i++)
            {
                tempDiaryContent += tempInteractionLists[i].GetDesc();

                if (i != tempInteractionLists.Count - 1)
                {
                    tempDiaryContent += "\n";
                }
            }

            diary_Contents.Add(tempDiaryContent);
            startSetOfDescNum++;
        }
    }
}
