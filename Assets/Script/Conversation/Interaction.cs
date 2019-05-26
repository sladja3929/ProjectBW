using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction
{
    /* 각 대화의 정보의 틀이 되는 클래스 */
    private int id;  // 상호작용의 id
    private string startObject; // 상호작용이 시작되게하는 오브젝트
    private string npcFrom; // 각 상호작용의 주체 npc 혹은 오브젝트 이름
    private string npcTo;   // 대화를 듣고있는 상대방 npc
    private string desc;     // 각 상호작용에서 발생하는 대화
    private int status;      // 해당 상호작용을 한적이 있는지 확인
    private string rewards;   //해당 상호작용으로 얻을 수 있는 단서 목록
    private int parent;      // 엮여있는 상호작용들을 확인할때 쓰기

    public int GetId()
    {
        return id;
    }

    public void SetId(int id)
    {
        this.id = id;
    }

    public string GetStartObject()
    {
        return startObject;
    }

    public void SetStartObject(string startObject)
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

    public string GetNpcTo()
    {
        return npcTo;
    }

    public void SetNpcTo(string npcTo)
    {
        this.npcTo = npcTo;
    }

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
