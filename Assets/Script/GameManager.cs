using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    // Use this for initialization
    void Start () {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    public void GetClue(string clueName)
    {
        if (!PlayerManager.instance.CheckClue(clueName))
        {
            int numOfAct = ItemDatabase.instance.FindClue(clueName);
            Debug.Log("Act " + (numOfAct + 1) + "의 단서인 " + clueName + "를 얻었습니다.");
            Inventory.instance.MakeClueSlot(clueName, numOfAct); // 수첩에 Clue slot 추가
            
        } else
        {
            Debug.Log("이미 획득한 단서입니다.");
        }
    }

    //동생의 상처 단서 획득
    public void GetInjuryOfSister()
    {
        GetClue("동생의 상처");
    }

    //찢겨진 그림 단서 획득
    public void GetTornPainting()
    {
        GetClue("찢겨진 그림");
    }
    
}
