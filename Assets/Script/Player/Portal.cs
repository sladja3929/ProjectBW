using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private GameObject arrow;
    public GameObject destination;      //포탈 출구

    private Animator Fadeanimator;      //Fadeinout용 애니메이터
    [SerializeField] private GameObject FadeImage;        //Fade용 이미지

    private void Awake() {
        arrow = transform.GetChild(0).gameObject;
        arrow.SetActive(false);

        Fadeanimator = GameObject.Find("Fade_Image").transform.GetComponent<Animator>();
        FadeImage = GameObject.Find("Fade_Image");
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        if(other.tag == "character") {
            arrow.SetActive(true);

            if (Input.GetKeyDown(KeyCode.W) && arrow.transform.name == "UpToTake") {
                StartCoroutine(FadeWithTakePortal());
            }

            if (Input.GetKeyDown(KeyCode.S) && arrow.transform.name == "DownToTake") {
                StartCoroutine(FadeWithTakePortal());
            }

            if (Input.GetKeyDown(KeyCode.A) && arrow.transform.name == "LeftToTake")
            {
                StartCoroutine(FadeWithTakePortal());
            }

            if (Input.GetKeyDown(KeyCode.D) && arrow.transform.name == "RightToTake")
            {
                StartCoroutine(FadeWithTakePortal());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "character") {
            arrow.SetActive(false);
        }
    }

    public void TakePortal() {

        Vector3 tempPosition = PlayerManager.instance.GetPlayerPosition();
        tempPosition.x = destination.transform.position.x;
        tempPosition.y = destination.transform.position.y;
        PlayerManager.instance.SetPlayerPosition(tempPosition);
        string position = destination.transform.parent.parent.parent.name
                           + "_" + destination.transform.parent.parent.name;
        PlayerManager.instance.SetCurrentPosition(position);
        MiniMapManager.instance.MoveArrowPosition();
        Debug.Log(PlayerManager.instance.GetCurrentPosition() + "으로 이동");
    }

    private IEnumerator FadeWithTakePortal()
    {
        /*페이드 아웃*/
        FadeImage.SetActive(true);
        Fadeanimator.SetBool("isfadeout", true);
        yield return new WaitForSeconds(0.5f);

        /*이동*/
        TakePortal();
        
        /*페이드 인*/
        yield return new WaitForSeconds(1f);
        Fadeanimator.SetBool("isfadeout", false);
    }

}
