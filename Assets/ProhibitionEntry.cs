using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProhibitionEntry : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (PlayerManager.instance.GetCurrentPosition().Equals("Slum_Information_agency") && other.tag == "character" && PlayerManager.instance.TimeSlot.Equals("71") && PlayerManager.instance.CheckEventCodeFromPlayedEventList("241"))
        {
            DialogManager.instance.InteractionWithObject("조건미충족시 자동");
        }

        if (PlayerManager.instance.GetCurrentPosition().Equals("Chapter_Chapter_First_Floor") && other.tag == "character" && PlayerManager.instance.TimeSlot.Equals("75") && PlayerManager.instance.CheckEventCodeFromPlayedEventList("300"))
        {
            DialogManager.instance.InteractionWithObject("사건4 시작");
        }
    }

}
