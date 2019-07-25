using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (PlayerManager.instance.GetCurrentPosition() == "Slam_Street2"
                && PlayerManager.instance.GetIsInPortalZone())
            {
                PlayerManager.instance.Move_From_Street2_To_Street1_In_Slam();
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (PlayerManager.instance.GetCurrentPosition() == "Slam_Street1"
                && PlayerManager.instance.GetIsInPortalZone())
            {
                PlayerManager.instance.Move_From_Street1_To_Street2_In_Slam();
            }
        }
    }


}
