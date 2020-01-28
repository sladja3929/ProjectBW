using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotButtonColor : MonoBehaviour
{

    //마우스로 선택된 단서 슬롯의 색을 변경하기 위한 함수
    public void ChangeSlotColor(int tempIndex)
    {
        ColorBlock tempColorBlock = UIManager.instance.testButton.colors;
        tempColorBlock.normalColor = Color.white;
        tempColorBlock.highlightedColor = Color.white;
        tempColorBlock.pressedColor = Color.white;
        UIManager.instance.testButton.colors = tempColorBlock;

        tempColorBlock = Inventory.instance.GetSlotObject(tempIndex).colors;
        tempColorBlock.normalColor = Color.gray;
        tempColorBlock.highlightedColor = Color.gray;
        tempColorBlock.pressedColor = Color.gray;
        Inventory.instance.GetSlotObject(tempIndex).colors = tempColorBlock;
        //Debug.Log("변경 완료");
    }

    /* 사건버튼의 색을 회색과 하얀색으로 변경하기 위한 함수 작성해야 할 듯 */
    public void ChangeSlotColor()
    {

    }
}
