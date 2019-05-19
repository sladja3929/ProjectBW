using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    public void FlipPage(int pressedAct)
    {
        UIManager.instance.ActivateUpDownButton(false);

        //페이지가 넘겨지고 있는 중이 아니라면 실행
        if (!UIManager.instance.isPaging)
        {
            UIManager.instance.isPaging = true; //Act 버튼 비활성화

            /* 현재 단서창으로 보고있는 Act에 따라서 오른쪽, 왼쪽 넘기기 구분 */
            int currentAct = UIManager.instance.GetCurrentPage();

            if (pressedAct > currentAct)
            {
                FlipRightPage(pressedAct);
                UIManager.instance.howManyOpenNote++;
                //UIManager.instance.isPaging = false;        //페이지를 넘겨도 된다는 뜻
            }
            else if (pressedAct < currentAct)
            {
                FlipLeftPage(pressedAct);
                UIManager.instance.howManyOpenNote++;
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
    public void FlipPage(int tempIndex, int numOfAct)
    {
        int num = Random.Range(0, 1 + 1);

        // 보고있는 단서를 다시 보려하면 페이지 넘김 효과 적용 안함
        if (UIManager.instance.GetTempIndex() == tempIndex) return;

        if(num % 2 == 0)
        {
            FlipRightPage(tempIndex, numOfAct);
        } else
        {
            FlipLeftPage(tempIndex, numOfAct);
        }
    }

    public void FlipRightPage(int pressedAct)
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

    public void FlipRightPage(int tempIndex, int numOfAct)
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

    public void FlipLeftPage(int pressedAct)
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

    public void FlipLeftPage(int tempIndex, int numOfAct)
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
    IEnumerator FlipRTL(float xc, float xl, float h, float frameTime, float dx, int pressedAct)
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
        PlayerManager.instance.NumOfAct = pressedAct;
    }

    IEnumerator FlipRTL(float xc, float xl, float h, float frameTime, float dx, int tempIndex, int numOfAct)
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
        //ItemDatabase.instance.LoadHaveDataOfAct(pressedAct);
    }

    IEnumerator FlipLTR(float xc, float xl, float h, float frameTime, float dx, int pressedAct)
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
        PlayerManager.instance.NumOfAct = pressedAct;
    }

    IEnumerator FlipLTR(float xc, float xl, float h, float frameTime, float dx, int tempIndex, int numOfAct)
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
        //ItemDatabase.instance.LoadHaveDataOfAct(pressedAct);
    }
}
