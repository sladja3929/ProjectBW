using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction
{
    /* 각 대화의 정보의 틀이 되는 클래스 */
    private int act;            // 사건
    //private int time;           // 시간대
    //private string time;        //임시(0822)
    private string[] time;        // 여러 시간대를 갖고 있을 수 있음 (1210)
    //private int position;       // 위치
    //private string position;    //임시(0822)
    private string[] position; // 수정(1223)
    private int setOfDesc;      // 대화 묶음
    private int id;             // 상호작용의 id
    //private string startObject; // 상호작용이 시작되게하는 오브젝트
    private string[] startObject; // 수정(1223)
    private string npcFrom;     // 각 상호작용의 주체 npc 혹은 오브젝트 이름
    //private string npcTo;       // 대화를 듣고있는 상대방 npc
    private string desc;        // 각 상호작용에서 발생하는 대화
    private string repeatability;   // 반복성
    private string[] conditionOfDesc;    //대사 조건
    private int status;         // 해당 상호작용을 한적이 있는지 확인
    private int parent;         // 엮여있는 상호작용들을 확인할때 쓰기
    private string rewards;     // 해당 상호작용으로 얻을 수 있는 단서 목록
    private string[] revealList;  // 단서 루트 해금 리스트
    private string occurrence;     //발생 여부
    private string eventIndexToOccur;   //새로운 이벤트

    public string GetEventIndexToOccur()
    {
        return eventIndexToOccur;
    }

    public void SetEventIndexToOccur(string eventIndexToOccur)
    {
        this.eventIndexToOccur = eventIndexToOccur;
    }

    public string GetOccurrence()
    {
        return occurrence;
    }

    public void SetOccurrence(string occurrence)
    {
        this.occurrence = occurrence;
    }

    public string[] GetRevealList()
    {
        return revealList;
    }

    public void SetRevealList(string[] revealList)
    {
        this.revealList = revealList;
    }

    public string[] GetConditionOfDesc()
    {
        return conditionOfDesc;
    }

    public void SetConditionOfDesc(string[] conditionOfDesc)
    {
        this.conditionOfDesc = conditionOfDesc;
    }

    public string GetRepeatability()
    {
        return repeatability;
    }

    public void SetRepeatability(string repeatability)
    {
        this.repeatability = repeatability;
    }

    public int GetSetOfDesc()
    {
        return setOfDesc;
    }

    public void SetSetOfDesc(int setOfDesc)
    {
        this.setOfDesc = setOfDesc;
    }
    /*
    public int GetPosition()
    {
        return position;
    }

    public void SetPosition(int position)
    {
        // 코드에 따라서 분류하는 if문 만들어야함
        this.position = position;
    }
    */
    //임시(0822)
    // 메르테의 현재 위치값과 비교해야할 상황이 올 수 있음. (11/12)
    public string[] GetPosition()
    {
        return position;
    }

    public void SetPosition(string[] position)
    {
        // 코드에 따라서 분류하는 if문 만들어야함
        this.position = position;
    }

    /*
    public int GetTime()
    {
        return time;
    }

    public void SetTime(int time)
    {
        this.time = time;
    }
    */

    //임시(0822)
    // 해당 time값에 특정 시간대의 코드가 들어있는지 확인하는 것으로 해당 시간대의 대화가 맞는지 확인할 수 있을 듯(테스트 필요) (11/12)
    //public string GetTime()
    //{
    //    return time;
    //}

    //public void SetTime(string time)
    //{
    //    this.time = time;
    //}

    // 여러 시간대 체크 가능한지 테스트(1210)
    public string[] GetTime()
    {
        return time;
    }

    // 대화 테이블에서 해당 대화가 캐릭터의 시간대에 맞는 대화인지 체크하는 함수 (1210) -> DialogManager.cs 에서 사용
    public bool CheckTime(string timeSlot)
    {
        string[] tempTime = this.GetTime();
        
        for (int i = 0; i < tempTime.Length; i++)
        {
            if (tempTime[i] == timeSlot)
            {
                //Debug.Log("tempTime[" + i + " ] = " + tempTime[i] + ", timeSlot = " + timeSlot + ", That's true");
                return true;
            }
        }
        //Debug.Log("Time : " + timeSlot + "CheckTime = false");
        return false;
    }

    public void SetTime(string[] time)
    {
        this.time = time;
    }

    public int GetAct()
    {
        return act;
    }

    public void SetAct(int act)
    {
        this.act = act;
    }

    public int GetId()
    {
        return id;
    }

    public void SetId(int id)
    {
        this.id = id;
    }

    public string[] GetStartObject()
    {
        return startObject;
    }

    // 대화 테이블에서 해당 StartObject가 알맞게 존재하는지 체크하는 함수(1223) -> DialogManager.cs 에서 사용
    public bool CheckStartObject(string startObject)
    {
        string[] tempStartObject = this.GetStartObject();

        for (int i = 0; i < tempStartObject.Length; i++)
        {
            //Debug.Log("tempStartObject[" + i + "] = " + tempStartObject[i]);
            if (tempStartObject[i] == startObject)
            {
                return true;
            }
        }

        //Debug.Log("startObject = " + startObject);
        return false;
    }

    public void SetStartObject(string[] startObject)
    {
        this.startObject = startObject;
    }

    public string GetNpcFrom()
    {
        return npcFrom;
    }

    public void SetNpcFrom(string npcFrom)
    {
        this.npcFrom = npcFrom;
    }
    /*
    public string GetNpcTo()
    {
        return npcTo;
    }

    public void SetNpcTo(string npcTo)
    {
        this.npcTo = npcTo;
    }
    */
    public string GetDesc()
    {
        return desc;
    }

    public void SetDesc(string desc)
    {
        this.desc = desc;
    }

    public int GetStatus()
    {
        return status;
    }

    public void SetStatus(int status)
    {
        this.status = status;
    }

    public string GetRewards()
    {
        return rewards;
    }

    public void SetRewards(string rewards)
    {
        this.rewards = rewards;
    }

    public int GetParent()
    {
        return parent;
    }

    public void SetParent(int parent)
    {
        this.parent = parent;
    }
}
