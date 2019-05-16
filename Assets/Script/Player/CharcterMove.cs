using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CharcterMove : MonoBehaviour
{
    public Animator anim;
    float speed = 100f;
    int horizontal_dir;
    int vertical_dir;
    //double size_d;//사이즈 변화량
    //double distance;//기준점과 소실점 사이의 거리
    //Vector2 size;

    //Vector2 destination_1 = new Vector2(-50,-50);//기준점 - 이쪽으로 갈 수록 캐릭터는 커짐
    //Vector2 destination_2 = new Vector2(50,50);//소실점 - 이쪽으로 갈 수록 캐릭터는 작아짐

    //Vector2 prevlocation;//이전위치
    //Vector2 nowlocation;//현재위치

    //void OnCollisionEnter2D(Collision2D coll)
    //{
    //    if (coll.collider.tag == "ground")
    //    {
    //        Debug.Log("충돌");
    //    }
    //}
    //void OnCollisionExit2D(Collision2D coll)
    //{
    //    if (coll.collider.tag == "ground")
    //    {
    //        Debug.Log("충돌해제");
    //    }
    //}

    void Start()
    {
        init();
    }

    void init()
    {
        //size.x = 35;
        //size.y = 35;
        //size_d = 1.0f;
        anim.SetBool("iswalking", false);
        anim.SetBool("isstanding", true);
        horizontal_dir = 1;
        vertical_dir = 1;
    }

    void Update()
    {
        //transform.localScale = new Vector3(size.x, size.y, 0.0f);
        Charactermove();
    }

    void Charactermove()
    {
            //prevlocation = transform.position;
            if (Input.GetKey(KeyCode.W))
            {
                if (vertical_dir == 0)
                {
                    vertical_dir = 1;
                    //size.y *= -1;
                }
                transform.Translate(Vector2.up * Time.deltaTime * speed, Space.Self);
                anim.SetBool("iswalking", true);
                anim.SetBool("isstanding", false);
            }
            if (Input.GetKey(KeyCode.S))
            {
                if (vertical_dir == 1)
                {
                     vertical_dir = 0;
                     //size.y *= -1;
                }
                transform.Translate(Vector2.down * Time.deltaTime * speed, Space.Self);
                anim.SetBool("iswalking", true);
                anim.SetBool("isstanding", false);

            }
            if (Input.GetKey(KeyCode.A))
            {
                if (horizontal_dir == 1)
                {
                    horizontal_dir = 0;
                    //size.x *= -1;
                }
                transform.Translate(Vector2.left * Time.deltaTime * speed, Space.Self);
                anim.SetBool("iswalking", true);
                anim.SetBool("isstanding", false);
            }
            
            if (Input.GetKey(KeyCode.D))
            {
                if (horizontal_dir == 0)
                {
                    horizontal_dir = 1;
                    //size.x *= -1;
                }
                transform.Translate(Vector2.right * Time.deltaTime * speed, Space.Self);
                anim.SetBool("iswalking", true);
                anim.SetBool("isstanding", false);
            }

            /*애니메이션 제어*/
            if (Input.GetKeyUp(KeyCode.A))
            {
                anim.SetBool("iswalking", false);
                anim.SetBool("isstanding", true);
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                anim.SetBool("iswalking", false);
                anim.SetBool("isstanding", true);
            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                anim.SetBool("iswalking", false);
                anim.SetBool("isstanding", true);
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                anim.SetBool("iswalking", false);
                anim.SetBool("isstanding", true);
            }
            //Debug.Log(transform.position);
            //nowlocation = transform.position;

            /*캐릭터 사이즈 조절 함수*/
            //ChangeCharaceterSize();
    }
}

