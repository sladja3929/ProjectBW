﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventConversationManager// : MonoBehaviour
{
    public int numOfCorrectCondition;

    public EventConversationManager()
    {
        numOfCorrectCondition = 0;
    }

    public int CheckEvent(List<Interaction> targetOfInteractionList, List<Interaction> interactionLists)
    {
        int resultIndex = -1;

        for (int i = 0; i < targetOfInteractionList.Count; i++)
        {
            // 반복성 = 이벤트 인 경우
            // 이벤트는 항상 대사 조건이 있어야만 한다.
            // 안그러면 이벤트가 아니겠지..?
            if (targetOfInteractionList[i].GetRepeatability().Equals("이벤트") && targetOfInteractionList[i].GetConditionOfDesc() != null 
                && targetOfInteractionList[i].GetStatus() == 0) { // 이벤트중에서 발생하지 않은 것들(status == 0)만 취급한다.
                //대사조건이 있는 이벤트라는 것을 확인했으면, 해당 대사조건이 가리키는 id의 status가 대사조건에 부합하는지 확인해야 한다.
                //현재 이벤트는 암묵적으로 1회만 발생하는 것으로 생각한다.(07.12)

                if (targetOfInteractionList[i].GetConditionOfDesc().Length > 1)
                {
                    //int numOfCorrectCondition = 0;  // 대사 조건에 부합하는 수를 알기 위한 변수 선언

                    //대사 조건이 2개 이상인 경우
                    for (int j = 0; j < targetOfInteractionList[i].GetConditionOfDesc().Length; j++)
                    {
                        //j번째 대사 조건에 해당하는 id를 가지는 index를 원래의 대사 목록중에서 찾은 다음 tempIndex에 저장
                        int tempIndex = interactionLists.FindIndex(x => x.GetId() == int.Parse(targetOfInteractionList[i].GetConditionOfDesc()[j]));
                        
                        if (interactionLists[tempIndex].GetStatus() >= 1)
                        {
                            //tempIndex에 해당하는 status가 1 이상일 경우 -> 즉 tempIndex에 해당하는 대사가 1번 이상 나타났을 경우
                            numOfCorrectCondition++;    //조건 만족 수 1 증가
                        }
                    }
                    
                    if (numOfCorrectCondition == targetOfInteractionList[i].GetConditionOfDesc().Length)
                    {
                        resultIndex = targetOfInteractionList[i].GetId(); // 해당 event 대사의 id를 리턴
                        numOfCorrectCondition = 0;
                        return resultIndex;
                    }
                    
                }
                else
                {
                    //대사 조건이 1개인 경우
                    if (targetOfInteractionList[i].GetConditionOfDesc()[0] != "x")
                    {
                        int tempIndex = targetOfInteractionList.FindIndex(x => x.GetId() == int.Parse(targetOfInteractionList[i].GetConditionOfDesc()[0]));

                        if (targetOfInteractionList[tempIndex].GetStatus() >= 1)
                        {
                            resultIndex = targetOfInteractionList[i].GetId(); // 해당 event 대사의 id를 저장
                            numOfCorrectCondition = 0;
                            return resultIndex;
                        }//if
                    }//if
                }//if-else
            }//if
        }//for-i

        //resultIndex가 -1이면, 이벤트 발생은 없는 것임
        numOfCorrectCondition = 0;
        return resultIndex;
    }


}