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
            PlayerManager.instance.SetIsNearObject(false);
        }
    }
}
