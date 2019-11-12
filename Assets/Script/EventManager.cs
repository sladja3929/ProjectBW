using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance = null;

    private List<string> eventIndexList;

    [SerializeField]
    private List<GameObject> npcListForEvent;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        eventIndexList = new List<string>();

        /* for test */
        AddToEventIndexList("202");    //발루아 등장 이벤트 index 추가

        DisableNpcForEvent();
    }

    // 추후에 이벤트 테이블 파일에서 이벤트들 불러올때 사용할 것임.
    // 혹은 대사를 불러오는 동안에 이 함수를 이용하여 발생할 수 있는 이벤트의 인덱스를 추가할 수 있음.
    public void AddToEventIndexList(string eventIndex)
    {
        eventIndexList.Add(eventIndex);
    }

    // npcList에 있는 모든 npc들 비활성화
    public void DisableNpcForEvent()
    {
        for (int i = 0; i < npcListForEvent.Count; i++)
        {
            npcListForEvent[i].SetActive(false);
        }
    }

    //NPC의 발생 뿐만 아니라, 특정한 이벤트들도 다룰 수 있도록 함수 명 변경 필요(11/12)
    public void ActivateNpcForEvent(string eventIndex)
    {
        if (CheckEventIndexList(eventIndex))
        {
            switch (eventIndex)
            {
                // 발루아 등장 이벤트
                case "202":
                    int tempIndex = npcListForEvent.FindIndex(x => x.gameObject.name == "발루아");
                    npcListForEvent[tempIndex].SetActive(true);
                    break;

                default:

                    break;
            }
        }
    }

    // 해당 eventIndex가 eventIndexList에 존재 하는지 체크
    public bool CheckEventIndexList(string eventIndex)
    {
        if (eventIndexList.Contains(eventIndex))
            return true;
        else
            return false;
    }

}
