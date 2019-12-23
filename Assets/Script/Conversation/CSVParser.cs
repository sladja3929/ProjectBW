using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CSVParser : MonoBehaviour
{
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

    /* 구조는 dataList와 일맥상통하다 */
    // csv 형식의 단서 테이블의 데이터들을 저장할 수 있는 변수
    private Dictionary<int, Dictionary<string, string>> clueList;

    //게임에 필요한 상호작용들을 가지고 있을 리스트변수 선언
    private List<Interaction> interactionLists;

    private List<ClueStructure> clueStructureLists;


    /* csv 파일 불러오면서 적용시키기 */
    void Awake()
    {
        ParsingConversationCSV();   // 대화 테이블 파싱
        ParsingClueDataCSV();       // 단서 테이블 파싱
    }//Awake()

    /* 대화 테이블을 파싱하는 함수 */
    public void ParsingConversationCSV()
    {
        dataList = new Dictionary<int, Dictionary<string, string>>();
        interactionLists = new List<Interaction>();

        //TextAsset textAsset = Resources.Load<TextAsset>("Data/Interaction");
        string textAsset = File.ReadAllText(Application.streamingAssetsPath + "/Data/Interaction_ver1_5.csv");
        //string textAsset = File.ReadAllText(Application.streamingAssetsPath + "/Data/사건_3_전체_테이블.csv");

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
                dataArr[j] = ReplaceDoubleQuotationMark(dataArr[j]);
                dataArr[j] = RemoveDoubleQuotationMark(dataArr[j]);     // 대화에 줄바꿈이 있을경우, 양끝에 "가 붙은걸 없애기
                dataArr[j] = ReplaceComma(dataArr[j]);
                dataArr[j] = ReplaceEnter(dataArr[j]);
                //Debug.Log("index = " + index);
                //Debug.Log("subjectArr[" + j + "] = " + subjectArr[j]);
                //Debug.Log("dataArr[" + j + "] = " + dataArr[j]);
                dataList[index].Add(subjectArr[j], dataArr[j]);

            }//for j
            index++;
        }//for i

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

                        tempInteraction.SetAct(int.Parse((dataList[i])[subjectArr[j]]));
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

                        tempInteraction.SetSetOfDesc(int.Parse((dataList[i])[subjectArr[j]]));
                        break;

                    case "id":

                        tempInteraction.SetId(int.Parse((dataList[i])[subjectArr[j]]));
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
                        break;

                    case "npcFrom":

                        tempInteraction.SetNpcFrom((dataList[i])[subjectArr[j]]);
                        break;
                    /*
                case "npcTo":

                    tempInteraction.SetNpcTo((dataList[i])[subjectArr[j]]);
                    break;
                    */
                    case "desc":

                        tempInteraction.SetDesc((dataList[i])[subjectArr[j]]);
                        break;

                    case "반복성":

                        tempInteraction.SetRepeatability((dataList[i])[subjectArr[j]]);
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

                        tempInteraction.SetStatus(int.Parse((dataList[i])[subjectArr[j]]));
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

                        tempInteraction.SetParent(int.Parse((dataList[i])[subjectArr[j]]));

                        break;
                    /* 삭제 예정(11/12) */
                    case "단서 루트 해금":

                        string tempRevealList = ReplaceComma((dataList[i])[subjectArr[j]]);
                        string[] revealList = tempRevealList.Split(',');
                        tempInteraction.SetRevealList(revealList);

                        break;
                    // 아래 두 부분은, 나중에 이벤트 처리할 때 수정할 것(1223)
                    case "발생 여부":

                        tempInteraction.SetOccurrence((dataList[i])[subjectArr[j]]);

                        break;

                    case "새로운 이벤트":

                        tempInteraction.SetEventIndexToOccur(((dataList[i])[subjectArr[j]]));
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
    public void ParsingClueDataCSV()
    {
        clueList = new Dictionary<int, Dictionary<string, string>>();
        clueStructureLists = new List<ClueStructure>();
        
        string textAsset = File.ReadAllText(Application.streamingAssetsPath + "/Data/단서.csv");

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

                        tempClueStructure.SetSpecialNum(((clueList[i])[subjectArr[j]]));
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
        }//for i
    }//ParsingClueDataCSV()

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


    public Dictionary<int, Dictionary<string, string>> GetDataList()
    {
        return dataList;
    }

    public List<Interaction> GetInteractionLists()
    {
        return interactionLists;
    }

    public Dictionary<int, Dictionary<string, string>> GetClueList()
    {
        return clueList;
    }

    public List<ClueStructure> GetClueStructureLists()
    {
        return clueStructureLists;
    }

}
