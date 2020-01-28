using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    private Rigidbody2D myRigidBody;
    private Animator myAnimator;

    [SerializeField]
    private float speed;
    private bool facingLeft;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
        facingLeft = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!UIManager.instance.GetIsOpenNote() && !UIManager.instance.isConversationing && !UIManager.instance.GetIsOpenedParchment() && !UIManager.instance.isFading)
        {
            float xInput = Input.GetAxisRaw("Horizontal");
            float yInput = Input.GetAxisRaw("Vertical");

            //모션이 끊기면서 이동되는 이유는 무엇일까..?
            //한번에 이동하는 간격이 길어서 그런것 같은데..
            if (!UIManager.instance.isPortaling)
            {
                myRigidBody.velocity = new Vector2((xInput / 1.0f) * speed, (yInput / 1.0f) * speed);

                if (xInput != 0)
                {
                    if (xInput != 0)
                        myAnimator.SetFloat("y", 0);

                    myAnimator.SetFloat("x", xInput);

                    myAnimator.SetBool("Walking", true);
                }

                if (yInput != 0)
                {
                    if (xInput != 0)
                        myAnimator.SetFloat("y", 0);
                    else
                        myAnimator.SetFloat("y", yInput);

                    myAnimator.SetBool("Walking", true);
                }
            }
            else if (UIManager.instance.isPortaling)
            {
                myRigidBody.velocity = new Vector2((xInput / 1.0f) * 0, (yInput / 1.0f) * 0);

                if (yInput != 0)
                {
                    if (xInput != 0)
                        myAnimator.SetFloat("y", 0);
                    else
                        myAnimator.SetFloat("y", yInput);

                    myAnimator.SetBool("Walking", false);
                }
            }
        }
        else if (UIManager.instance.GetIsOpenNote() || UIManager.instance.isConversationing || UIManager.instance.GetIsOpenedParchment() || UIManager.instance.isFading || UIManager.instance.isPortaling)
        {
            //캐릭터 도리도리 현상 발생. 
            myRigidBody.velocity = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        if (!UIManager.instance.GetIsOpenNote() && !UIManager.instance.isConversationing && !UIManager.instance.GetIsOpenedParchment() && !UIManager.instance.isFading && !UIManager.instance.isPortaling)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            if (!UIManager.instance.isConversationing)
                Flip(horizontal);

            if ((horizontal == 0 && vertical == 0) || UIManager.instance.isPortaling)
            {
                myAnimator.SetBool("Walking", false);
            }
        }

        if (UIManager.instance.GetIsOpenedParchment())
            myAnimator.SetBool("Walking", false);
    }

    private void Flip(float horizontal)
    {
        if (horizontal > 0 && facingLeft || horizontal < 0 && !facingLeft)
        {
            facingLeft = !facingLeft;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }


    public void Temp()
    {
        /* 사용은 안하지만, 캐릭터 애니메이션 프레임 수 늘리기 위해 그냥 만듦 */
    }
}
