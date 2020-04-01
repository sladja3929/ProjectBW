using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProhibitionEntry : MonoBehaviour
{
    private bool[] atonce = new bool[5];

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

        if (GameManager.instance.GetPlayState() == GameManager.PlayState.Tutorial && other.tag == "character")
        {
            if (!TutorialManager.instance.isCompletedTutorial[2] && !atonce[0])
            {
                Debug.Log(TutorialManager.instance.tutorial_Index + "진입");
                TutorialManager.instance.isCompletedTutorial[2] = true;
                atonce[0] = true;
                TutorialManager.instance.InvokeTutorial();
            }
            else if (!TutorialManager.instance.isCompletedTutorial[3] && !atonce[1])
            {
                Debug.Log(TutorialManager.instance.tutorial_Index + "진입");
                atonce[1] = true;
                TutorialManager.instance.InvokeTutorial();
            }
            else if (!TutorialManager.instance.isCompletedTutorial[14] && !atonce[2])
            {
                Debug.Log(TutorialManager.instance.tutorial_Index + "진입");
                atonce[2] = true;
                TutorialManager.instance.IncreaseTutorial_Index();
                TutorialManager.instance.InvokeTutorial();
            }
        }
    }

}
