using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "character")
        {
            PlayerManager.instance.SetIsNearObject(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "character")
        {
            //PlayerManager.instance.SetIsNearObject(false);
            /* 추후, 상호작용 할 수 있는 오브젝트에 근접해 있을 때만 상호작용 되도록 할것(1월 27일 메모) */
            PlayerManager.instance.SetIsNearObject(true);
        }
    }
}
