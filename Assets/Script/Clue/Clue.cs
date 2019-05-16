using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clue {

    public string numOfAct;
    public string name;
    public string description;
    /* LitJson 이용시, Sprite, float 등의 자료형이 저장되지 않아서 
     * 다른 방식으로 이미지 로드 구현했음 (UIManager 코드 참고) */
    //public Sprite itemSprite;
    public string firstInfoOfClue;  //단서 내용1에 해당하는 내용
    public string arranged_content; //단서 내용2에 해당하는 내용

    /*
    public string Arranged_Text
    {
        get
        {
            return arranged_Text;
        }

        set
        {
            arranged_Text = value;
        }
    }
    */
    public Clue(string numOfAct, string name, string desc, string arranged_content)
    {
        this.numOfAct = numOfAct;
        this.name = name;
        description = desc;
        //itemSprite = Resources.Load<Sprite>("Image/" + this.name);
        this.firstInfoOfClue = "";
        this.arranged_content = arranged_content;
    }

    public Clue(string numOfAct, string name, string desc, string firstInfoOfClue, string arranged_content)
    {
        this.numOfAct = numOfAct;
        this.name = name;
        description = desc;
        //itemSprite = Resources.Load<Sprite>("Image/" + this.name);
        this.firstInfoOfClue = firstInfoOfClue;
        this.arranged_content = arranged_content;
    }

    public string GetNumOfAct()
    {
        return numOfAct;
    }

    public string GetName()
    {
        return name;
    }

    public string GetDesc()
    {
        return description;
    }
    /*
    public Sprite GetSprite()
    {
        return itemSprite;
    }
    */

    public string GetFirstInfoOfClue()
    {
        return firstInfoOfClue;
    }

    public void SetFirstInfoOfClue(string firstInfoOfClue)
    {
        this.firstInfoOfClue = firstInfoOfClue;
    }
    
    public string GetArrangedContent()
    {
        return arranged_content;
    }

    public void SetArrangedText(string arranged_content)
    {
        this.arranged_content = arranged_content;
    }
}
