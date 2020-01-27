using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProhibitionEntry : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "character" && PlayerManager.instance.TimeSlot.Equals("71") && PlayerManager.instance.CheckEventCodeFromPlayedEventList("241"))
        {
            DialogManager.instance.InteractionWithObject("조건미충족시 자동");
        }
    }

}
