using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventConversationManager// : MonoBehaviour
{
    public int numOfCorrectCondition;

    public EventConversationManager()
    {
        numOfCorrectCondition = 0;
    }
    
    /* < 이벤트 관련 ID >
     * 발생X : 0, 발생O : 1, 이벤트 : 2, 반복 : 3
     * 이벤트&반복 : 4, 일반단서 : 5, 특정단서 : 6,
     * 단서 X : 7, 다른 단서를 획득할 수 있는 루트 해금 : 8
       */

    public int CheckEvent(List<Interaction> targetOfInteractionList, List<Interaction> interactionLists)
    {
        int resultIndex = -1;

        for (int i = 0; i < targetOfInteractionList.Count; i++)
        {
            // 반복성 = 이벤트 인 경우
            // 이벤트는 항상 대사 조건이 있어야 하지만, 이벤트&반복 속성은 다르다.

            if (targetOfInteractionList[i].GetRepeatability().Equals("4") && targetOfInteractionList[i].GetConditionOfDesc() != null
                            && targetOfInteractionList[i].GetStatus() == 0)
            {

                resultIndex = targetOfInteractionList[i].GetSetOfDesc(); // 해당 event 대화묶음의 index를 리턴
                //Debug.Log("4444444444444444444");
                return resultIndex;
            }
            else if (targetOfInteractionList[i].GetRepeatability().Equals("4") && targetOfInteractionList[i].GetConditionOfDesc() == null
              && targetOfInteractionList[i].GetStatus() == 0)
            {
                int index = targetOfInteractionList[i].GetSetOfDesc();

                // 이벤트 226번
                if (index == 4025)
                {
                    //사체묘사 대사와 함께 해당 사체의 일러스트가 화면에 띄워진다. -> 대사는 자동진행이니, 사체의 일러스트만 띄우기 위한 임의의 UIManager의 제어 변수를 만들어서 조작하면 됨.
                }

                return index;
            }
            else if (targetOfInteractionList[i].GetRepeatability().Equals("2") && targetOfInteractionList[i].GetConditionOfDesc() == null
              && targetOfInteractionList[i].GetStatus() == 0)
            {
                // 반복성이 2인데, 대사조건이 없고, 한번도 진행되지 않은 대화일 때 처리
                // 반복성이 2인 대화를 1개 1개씩 진행되게끔 함
                int index = targetOfInteractionList[i].GetSetOfDesc();

                //2002, 2045, 2023, 2028, 2065, 2070 대화 묶음에 해당하는 부분만 50%확률로 진행되도록 처리
                if (index == 2002 || index == 2045 || index == 2023 || index == 2028 || index == 2065 || index == 2070)
                {
                    int randNum = Random.Range(0, 2);
                    if (randNum == 0)
                        return index;
                }

                return targetOfInteractionList[i].GetSetOfDesc();
            }
            else if (targetOfInteractionList[i].GetRepeatability().Equals("2") && targetOfInteractionList[i].GetConditionOfDesc() != null
                     && targetOfInteractionList[i].GetStatus() == 0)
            { // 이벤트중에서 발생하지 않은 것들(status == 0)만 취급한다.
              // 반복성이 2이고, 대사조건이 있고, 한번도 진행되지 않은 대화일때 처리
              //대사조건이 있는 이벤트라는 것을 확인했으면, 해당 대사조건이 가리키는 id의 status가 대사조건에 부합하는지 확인해야 한다.
              //현재 이벤트는 암묵적으로 1회만 발생하는 것으로 생각한다.(07.12)

                //Debug.Log("222222222222222222222222222");
                if (targetOfInteractionList[i].GetConditionOfDesc().Length > 1)
                {
                    //int numOfCorrectCondition = 0;  // 대사 조건에 부합하는 수를 알기 위한 변수 선언
                    int tempIndex;
                    //대사 조건이 2개 이상인 경우
                    for (int j = 0; j < targetOfInteractionList[i].GetConditionOfDesc().Length; j++)
                    {

                        try
                        {
                            //j번째 대사 조건에 해당하는 대화묶음의 index를 원래의 대사 목록중에서 찾은 다음 tempIndex에 저장
                            tempIndex = interactionLists.FindIndex(x => x.GetSetOfDesc() == int.Parse(targetOfInteractionList[i].GetConditionOfDesc()[j]));
                        }
                        catch
                        {
                            //int.Parse 포맷에 맞지 않으면 -1을 리턴하게 함 (1225 수정)
                            // 임시적으로 만든 예외처리이고, 대화 조건에 숫자가 아닌 문자가 들어갔기 때문에 만들었음. ex) 멜리사와의 대화
                            //      -> 추후에 문자그대로 처리해서 따로 변수를 만들어서 작업해야 할듯?
                            return -1;
                        }

                        if (interactionLists[tempIndex].GetStatus() >= 1)
                        {
                            //tempIndex에 해당하는 status가 1 이상일 경우 -> 즉 tempIndex에 해당하는 대사가 1번 이상 나타났을 경우
                            numOfCorrectCondition++;    //조건 만족 수 1 증가
                        }
                    }

                    if (numOfCorrectCondition == targetOfInteractionList[i].GetConditionOfDesc().Length)
                    {
                        resultIndex = targetOfInteractionList[i].GetSetOfDesc(); // 해당 event 대화묶음의 index를 리턴
                        numOfCorrectCondition = 0;
                        return resultIndex;
                    }

                }
                else
                {

                    //대사 조건이 1개인 경우
                    if (targetOfInteractionList[i].GetConditionOfDesc()[0] != "x" || targetOfInteractionList[i].GetConditionOfDesc()[0] != "")
                    {
                        // 대사 조건이 대화 묶음일경우
                        if (targetOfInteractionList[i].GetConditionOfDesc()[0].Length == 4)
                        {
                            int tempIndex = interactionLists.FindIndex(x => x.GetSetOfDesc() == int.Parse(targetOfInteractionList[i].GetConditionOfDesc()[0]));
                            
                            if (interactionLists[tempIndex].GetStatus() >= 1)
                            {
                                resultIndex = targetOfInteractionList[i].GetSetOfDesc(); // 해당 event 대사의 id를 저장
                                numOfCorrectCondition = 0;
                                return resultIndex;
                            }//if
                            
                        }
                        else if (targetOfInteractionList[i].GetConditionOfDesc()[0].Length == 3)
                        {
                            // 대사 조건이 이벤트일 경우
                            resultIndex = targetOfInteractionList[i].GetSetOfDesc();
                            numOfCorrectCondition = 0;
                            return resultIndex;
                        }
                    }//if
                    
                    
                }//if-else
            }//if
        }//for-i

        //resultIndex가 -1이면, 이벤트 발생은 없는 것임
        numOfCorrectCondition = 0;
        return resultIndex;
    }


}
