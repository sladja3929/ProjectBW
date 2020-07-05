using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProhibitionEntry : MonoBehaviour
{
    private bool[] atonce = new bool[3];

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (PlayerManager.instance.GetCurrentPosition().Equals("Slum_Information_agency") && other.tag == "character" && PlayerManager.instance.TimeSlot.Equals("71") && !PlayerManager.instance.CheckEventCodeFromPlayedEventList("241"))
        {
            PlayerManager.instance.AddEventCodeToList("241");
            DialogManager.instance.InteractionWithObject("조건미충족시 자동");
        }

        // 사이드 1 트리거
        if (PlayerManager.instance.GetPositionOfMerte() <= 11000.0f && PlayerManager.instance.GetCurrentPosition().Equals("Chapter_Chapter_First_Floor") && other.tag == "character" && PlayerManager.instance.TimeSlot.Equals("75") && !PlayerManager.instance.CheckEventCodeFromPlayedEventList("300"))
        {
            PlayerManager.instance.AddEventCodeToList("300");
            DialogManager.instance.InteractionWithObject("사건4 시작");
        }

        // 260 이벤트 (Event_260_262_Trigger)
        if (PlayerManager.instance.GetCurrentPosition().Equals("Chapter_Chapter_First_Floor") && other.tag == "character" && PlayerManager.instance.TimeSlot.Equals("72") && !PlayerManager.instance.CheckEventCodeFromPlayedEventList("260"))
        {
            DialogManager.instance.InteractionWithObject("Starting72_AfterOutOffice");
        }

        // 261 이벤트 ( 사이드 1 트리거 )
        if (PlayerManager.instance.GetPositionOfMerte() <= 11000.0f && PlayerManager.instance.GetCurrentPosition().Equals("Chapter_Chapter_First_Floor") && other.tag == "character" && PlayerManager.instance.TimeSlot.Equals("73") && !PlayerManager.instance.CheckEventCodeFromPlayedEventList("261"))
        {
            DialogManager.instance.InteractionWithObject("Starting73_Info_SideCut");
        }

        // 262 이벤트 (Event_260_262_Trigger)
        if (PlayerManager.instance.GetCurrentPosition().Equals("Chapter_Chapter_First_Floor") && other.tag == "character" && PlayerManager.instance.TimeSlot.Equals("74") && !PlayerManager.instance.CheckEventCodeFromPlayedEventList("262"))
        {
            DialogManager.instance.InteractionWithObject("Starting74_AfterOutOffice");
        }

        // 263 이벤트
        if (PlayerManager.instance.GetCurrentPosition().Equals("Chapter_Street1") && other.tag == "character" && PlayerManager.instance.TimeSlot.Equals("71") && !PlayerManager.instance.CheckEventCodeFromPlayedEventList("263"))
        {
            DialogManager.instance.InteractionWithObject("Starting71_AfterOutChapter");
        }

        // 316 이벤트
        if (PlayerManager.instance.GetCurrentPosition().Equals("Chapter_Street1") && other.tag == "character" && PlayerManager.instance.TimeSlot.Equals("76") && !PlayerManager.instance.CheckEventCodeFromPlayedEventList("316"))
        {
            DialogManager.instance.InteractionWithObject("Starting76_AfterOutChapter");
        }

        if (GameManager.instance.GetPlayState() == GameManager.PlayState.Tutorial && other.tag == "character")
        {
            if (!TutorialManager.instance.isCompletedTutorial[2] && !atonce[0])
            {
                TutorialManager.instance.isCompletedTutorial[2] = true;
                atonce[0] = true;
                TutorialManager.instance.InvokeTutorial();
            }
            else if (!TutorialManager.instance.isCompletedTutorial[3] && !atonce[1])
            {
                atonce[1] = true;
                TutorialManager.instance.InvokeTutorial();
            }
            else if (!TutorialManager.instance.isCompletedTutorial[14] && !atonce[2])
            {
                atonce[2] = true;
                TutorialManager.instance.IncreaseTutorial_Index();
                TutorialManager.instance.InvokeTutorial();
            }
        }
    }

}
