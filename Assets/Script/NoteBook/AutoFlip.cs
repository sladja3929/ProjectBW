using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(Book))]
public class AutoFlip : MonoBehaviour {

    public static AutoFlip instance = null;

    public FlipMode Mode;
    public float PageFlipTime = 1;
    public float TimeBetweenPages = 1;
    public float DelayBeforeStarting = 0;
    public bool AutoStartFlip=true;
    public Book ControledBook;
    public int AnimationFramesCount = 40;
    bool isFlipping = false;

    [SerializeField]
    private GameObject firstTextScroll;
    [SerializeField]
    private GameObject secondTextScroll;

    // Use this for initialization
    void Start () {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        if (!ControledBook)
            ControledBook = GetComponent<Book>();
        if (AutoStartFlip)
            StartFlipping();
        ControledBook.OnFlip.AddListener(new UnityEngine.Events.UnityAction(PageFlipped));
	}
    void PageFlipped()
    {
        isFlipping = false;
    }
	public void StartFlipping()
    {
        //StartCoroutine(FlipToEnd());
    }

    public void FlipPage(string pressedAct)
    {
        int tempPressedAct = int.Parse(pressedAct);

        UIManager.instance.ActivateUpDownButton(false);
        
        //페이지가 넘겨지고 있는 중이 아니라면 실행
        if (!UIManager.instance.isPaging)
        {
            UIManager.instance.isPaging = true; //Act 버튼 비활성화

            /* 현재 단서창으로 보고있는 Act에 따라서 오른쪽, 왼쪽 넘기기 구분 */
            int currentAct = int.Parse(UIManager.instance.GetCurrentPage());

            if (tempPressedAct > currentAct)
            {
                FlipRightPage(pressedAct);
                UIManager.instance.shownSlotIndex = 1;
                //UIManager.instance.howManyOpenNote++;
                //UIManager.instance.isPaging = false;        //페이지를 넘겨도 된다는 뜻
            }
            else if (tempPressedAct < currentAct)
            {
                FlipLeftPage(pressedAct);
                UIManager.instance.shownSlotIndex = 1;
                //UIManager.instance.howManyOpenNote++;
                //UIManager.instance.isPaging = false;        //페이지를 넘겨도 된다는 뜻
            }
            else
            {
                UIManager.instance.isPaging = false;
                return;
            }
        }
    }

    //페이지 넘김 효과를 준 후, 단서의 내용을 출력하기 위한 함수
    public void FlipPage(int tempIndex, string numOfAct)
    {
        int num = Random.Range(0, 1 + 1);

        if (!UIManager.instance.isPaging)
        {
            UIManager.instance.isPaging = true;
            // 보고있는 단서를 다시 보려하면 페이지 넘김 효과 적용 안함
            if (UIManager.instance.GetTempIndex() == tempIndex)
            {
                //Debug.Log("봤던걸 왜 또 봐");
                UIManager.instance.isPaging = false;
                return;
            }

            if (num % 2 == 0)
            {
                FlipRightPage(tempIndex, numOfAct);
                UIManager.instance.shownSlotIndex = tempIndex + 1;
            }
            else
            {
                FlipLeftPage(tempIndex, numOfAct);
                UIManager.instance.shownSlotIndex = tempIndex + 1;
            }
        }
    }

    public void FlipRightPage(string pressedAct)
    {
        if (isFlipping) return;
        if (ControledBook.currentPage >= ControledBook.TotalPageCount) return;
        isFlipping = true;
        float frameTime = PageFlipTime / AnimationFramesCount;
        float xc = (ControledBook.EndBottomRight.x + ControledBook.EndBottomLeft.x) / 2;
        float xl = ((ControledBook.EndBottomRight.x - ControledBook.EndBottomLeft.x) / 2) * 0.9f;
        //float h =  ControledBook.Height * 0.5f;
        float h = Mathf.Abs(ControledBook.EndBottomRight.y) * 0.9f;
        float dx = (xl)*2 / AnimationFramesCount;
        StartCoroutine(FlipRTL(xc, xl, h, frameTime, dx, pressedAct));
    }

    public void FlipRightPage(int tempIndex, string numOfAct)
    {
        if (isFlipping) return;
        if (ControledBook.currentPage >= ControledBook.TotalPageCount) return;
        isFlipping = true;
        float frameTime = PageFlipTime / AnimationFramesCount;
        float xc = (ControledBook.EndBottomRight.x + ControledBook.EndBottomLeft.x) / 2;
        float xl = ((ControledBook.EndBottomRight.x - ControledBook.EndBottomLeft.x) / 2) * 0.9f;
        //float h =  ControledBook.Height * 0.5f;
        float h = Mathf.Abs(ControledBook.EndBottomRight.y) * 0.9f;
        float dx = (xl) * 2 / AnimationFramesCount;
        StartCoroutine(FlipRTL(xc, xl, h, frameTime, dx, tempIndex, numOfAct));
    }

    public void FlipLeftPage(string pressedAct)
    {
        if (isFlipping) return;
        if (ControledBook.currentPage <= -10) return;
        isFlipping = true;
        float frameTime = PageFlipTime / AnimationFramesCount;
        float xc = (ControledBook.EndBottomRight.x + ControledBook.EndBottomLeft.x) / 2;
        float xl = ((ControledBook.EndBottomRight.x - ControledBook.EndBottomLeft.x) / 2) * 0.9f;
        //float h =  ControledBook.Height * 0.5f;
        float h = Mathf.Abs(ControledBook.EndBottomRight.y) * 0.9f;
        float dx = (xl) * 2 / AnimationFramesCount;
        StartCoroutine(FlipLTR(xc, xl, h, frameTime, dx, pressedAct));
    }

    public void FlipLeftPage(int tempIndex, string numOfAct)
    {
        if (isFlipping) return;
        if (ControledBook.currentPage <= -10) return;
        isFlipping = true;
        float frameTime = PageFlipTime / AnimationFramesCount;
        float xc = (ControledBook.EndBottomRight.x + ControledBook.EndBottomLeft.x) / 2;
        float xl = ((ControledBook.EndBottomRight.x - ControledBook.EndBottomLeft.x) / 2) * 0.9f;
        //float h =  ControledBook.Height * 0.5f;
        float h = Mathf.Abs(ControledBook.EndBottomRight.y) * 0.9f;
        float dx = (xl) * 2 / AnimationFramesCount;
        StartCoroutine(FlipLTR(xc, xl, h, frameTime, dx, tempIndex, numOfAct));
    }

    IEnumerator FlipToEnd()
    {
        yield return new WaitForSeconds(DelayBeforeStarting);
        float frameTime = PageFlipTime / AnimationFramesCount;
        float xc = (ControledBook.EndBottomRight.x + ControledBook.EndBottomLeft.x) / 2;
        float xl = ((ControledBook.EndBottomRight.x - ControledBook.EndBottomLeft.x) / 2)*0.9f;
        //float h =  ControledBook.Height * 0.5f;
        float h = Mathf.Abs(ControledBook.EndBottomRight.y)*0.9f;
        //y=-(h/(xl)^2)*(x-xc)^2          
        //               y         
        //               |          
        //               |          
        //               |          
        //_______________|_________________x         
        //              o|o             |
        //           o   |   o          |
        //         o     |     o        | h
        //        o      |      o       |
        //       o------xc-------o      -
        //               |<--xl-->
        //               |
        //               |
        float dx = (xl)*2 / AnimationFramesCount;
        switch (Mode)
        {
            case FlipMode.RightToLeft:
                while (ControledBook.currentPage < ControledBook.TotalPageCount)
                {
                    //StartCoroutine(FlipRTL(xc, xl, h, frameTime, dx));
                    yield return new WaitForSeconds(TimeBetweenPages);
                }
                break;
            case FlipMode.LeftToRight:
                while (ControledBook.currentPage > 0)
                {
                    //StartCoroutine(FlipLTR(xc, xl, h, frameTime, dx));
                    yield return new WaitForSeconds(TimeBetweenPages);
                }
                break;
        }
    }
    IEnumerator FlipRTL(float xc, float xl, float h, float frameTime, float dx, string pressedAct)
    {
        // slot reset
        Inventory.instance.ResetSlot();

        UIManager.instance.CloseClueUI();

        float x = xc + xl;
        float y = (-h / (xl * xl)) * (x - xc) * (x - xc);

        ControledBook.DragRightPageToPoint(new Vector3(x, y, 0));
        for (int i = 0; i < AnimationFramesCount; i++)
        {
            y = (-h / (xl * xl)) * (x - xc) * (x - xc);
            ControledBook.UpdateBookRTLToPoint(new Vector3(x, y, 0));
            yield return new WaitForSeconds(frameTime);
            x -= dx;
        }
        ControledBook.ReleasePage();

        yield return new WaitForSeconds(0.2f);

        UIManager.instance.isPaging = false;        //페이지를 넘겨도 된다는 뜻
        UIManager.instance.OpenClueUI();
        UIManager.instance.ResetWrittenClueData();
        ItemDatabase.instance.LoadHaveDataOfAct(pressedAct);
        PlayerManager.instance.NumOfAct = pressedAct.ToString();

        UIManager.instance.SetTempIndex(0); // 사건단위로 수첩을 넘기면, 처음 단서를 보여줘야 하므로 tempIndex를 0으로 설정
        UIManager.instance.buttonIndex = 0;
        UIManager.instance.buttonNumOfAct = pressedAct;


        //단서 슬롯이 하나 이상 있을 때, 첫번째 단서를 보고있는 효과를 주기 위한 if
        if (Inventory.instance.GetSlotCount() > 0)
        {
            UIManager.instance.testButton = Inventory.instance.GetSlotObject(0);
            UIManager.instance.nextButton = UIManager.instance.testButton;
            UIManager.instance.SetColorBlockToGray();
        }
    }

    IEnumerator FlipRTL(float xc, float xl, float h, float frameTime, float dx, int tempIndex, string numOfAct)
    {
        // slot reset
        //Inventory.instance.ResetSlot();
        
        UIManager.instance.CloseClueUI();
        float x = xc + xl;
        float y = (-h / (xl * xl)) * (x - xc) * (x - xc);

        ControledBook.DragRightPageToPoint(new Vector3(x, y, 0));
        for (int i = 0; i < AnimationFramesCount; i++)
        {
            y = (-h / (xl * xl)) * (x - xc) * (x - xc);
            ControledBook.UpdateBookRTLToPoint(new Vector3(x, y, 0));
            yield return new WaitForSeconds(frameTime);
            x -= dx;
        }
        ControledBook.ReleasePage();

        yield return new WaitForSeconds(0.2f);

        UIManager.instance.isPaging = false;        //페이지를 넘겨도 된다는 뜻
        UIManager.instance.OpenClueUI();
        UIManager.instance.ResetWrittenClueData();
        UIManager.instance.ShowClueData(tempIndex, numOfAct);
        UIManager.instance.SetTempIndex(tempIndex);

        if (Inventory.instance.GetSlotCount() > 0)
        {
            UIManager.instance.testButton = Inventory.instance.GetSlotObject(0);
        }

        /* test for ws button */
        UIManager.instance.buttonIndex = tempIndex;
        //UIManager.instance.buttonNumOfAct = numOfAct;
        UIManager.instance.testButton = Inventory.instance.GetSlotObject(UIManager.instance.buttonIndex);
        //ItemDatabase.instance.LoadHaveDataOfAct(pressedAct);
        
    }

    IEnumerator FlipLTR(float xc, float xl, float h, float frameTime, float dx, string pressedAct)
    {
        // slot reset
        Inventory.instance.ResetSlot();

        UIManager.instance.CloseClueUI();

        float x = xc - xl;
        float y = (-h / (xl * xl)) * (x - xc) * (x - xc);
        ControledBook.DragLeftPageToPoint(new Vector3(x, y, 0));
        for (int i = 0; i < AnimationFramesCount; i++)
        {
            y = (-h / (xl * xl)) * (x - xc) * (x - xc);
            ControledBook.UpdateBookLTRToPoint(new Vector3(x, y, 0));
            yield return new WaitForSeconds(frameTime);
            x += dx;
        }
        ControledBook.ReleasePage();

        yield return new WaitForSeconds(0.2f);

        UIManager.instance.isPaging = false;        //페이지를 넘겨도 된다는 뜻
        UIManager.instance.OpenClueUI();
        UIManager.instance.ResetWrittenClueData();
        ItemDatabase.instance.LoadHaveDataOfAct(pressedAct);
        PlayerManager.instance.NumOfAct = pressedAct.ToString();

        UIManager.instance.SetTempIndex(0); // 사건단위로 수첩을 넘기면, 처음 단서를 보여줘야 하므로 tempIndex를 0으로 설정
        UIManager.instance.buttonIndex = 0;
        UIManager.instance.buttonNumOfAct = pressedAct;

        if (Inventory.instance.GetSlotCount() > 0)
        {
            UIManager.instance.testButton = Inventory.instance.GetSlotObject(0);
            UIManager.instance.nextButton = UIManager.instance.testButton;
            UIManager.instance.SetColorBlockToGray();
        }
    }

    IEnumerator FlipLTR(float xc, float xl, float h, float frameTime, float dx, int tempIndex, string numOfAct)
    {
        // slot reset
        //Inventory.instance.ResetSlot();
        
        UIManager.instance.CloseClueUI();
        float x = xc - xl;
        float y = (-h / (xl * xl)) * (x - xc) * (x - xc);
        ControledBook.DragLeftPageToPoint(new Vector3(x, y, 0));
        for (int i = 0; i < AnimationFramesCount; i++)
        {
            y = (-h / (xl * xl)) * (x - xc) * (x - xc);
            ControledBook.UpdateBookLTRToPoint(new Vector3(x, y, 0));
            yield return new WaitForSeconds(frameTime);
            x += dx;
        }
        ControledBook.ReleasePage();

        yield return new WaitForSeconds(0.2f);

        UIManager.instance.isPaging = false;        //페이지를 넘겨도 된다는 뜻
        UIManager.instance.OpenClueUI();
        UIManager.instance.ResetWrittenClueData();
        UIManager.instance.ShowClueData(tempIndex, numOfAct);
        UIManager.instance.SetTempIndex(tempIndex);

        if (Inventory.instance.GetSlotCount() > 0)
        {
            UIManager.instance.testButton = Inventory.instance.GetSlotObject(0);
        }

        /* test for ws button */
        UIManager.instance.buttonIndex = tempIndex;
        //UIManager.instance.buttonNumOfAct = numOfAct;
        UIManager.instance.testButton = Inventory.instance.GetSlotObject(UIManager.instance.buttonIndex);
        //ItemDatabase.instance.LoadHaveDataOfAct(pressedAct);

    }
}
