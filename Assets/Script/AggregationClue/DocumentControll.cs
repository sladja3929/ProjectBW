using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class DocumentControll : MonoBehaviour
{
    [SerializeField]
    private PlayableDirector playableDirector;  // 안드렌의 서류 애니메이션을 담당할 변수
    //[SerializeField]
    //private TimelineAsset timeLine; // 별도의 타임라인 애니메이션을 사용할 때 사용하는 변수 ex) ~.Play(timeLine);

    public static DocumentControll instance = null;

    [SerializeField]
    private RectTransform rect_DocumentControl;
    [SerializeField]
    private RectTransform rect_DocumentOfAndren;
    [SerializeField]
    private RectTransform rect_DocumentOpener;
    [SerializeField]
    private RectTransform rect_PaperOfDocument;
    [SerializeField]
    private RectTransform rect_DocumentCover;

    [SerializeField]
    private GameObject first_AndrenClue;
    [SerializeField]
    private GameObject second_AndrenClue;
    [SerializeField]
    private GameObject third_AndrenClue;

    private Vector2 initPositionDocumentOfControl;
    private Vector2 initPositionDocumentOfAndren;
    private Quaternion initRotationDocumentOpener;
    private Vector2 initSizePaperOfDocument;
    private Vector2 initPositionDocumentCover;

    // Use this for initialization
    void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        Init_Document();
    }

    public void Init_Document()
    {
        initPositionDocumentOfControl = rect_DocumentControl.localPosition;
        initPositionDocumentOfAndren = rect_DocumentOfAndren.localPosition;
        initRotationDocumentOpener = rect_DocumentOpener.localRotation;
        initSizePaperOfDocument = rect_PaperOfDocument.sizeDelta;
        initPositionDocumentCover = rect_DocumentCover.localPosition;

        //Debug.Log("init = " + initPositionDocumentOfControl + ", " + initPositionDocumentOfAndren + ", " + initRotationDocumentOpener + ", " + initSizePaperOfDocument + ", " + initPositionDocumentCover);
    }

    public void ResetDocumentOfAndren()
    {
        //Debug.Log("reset = " + initPositionDocumentOfControl + ", " + initPositionDocumentOfAndren + ", " + initRotationDocumentOpener + ", " + initSizePaperOfDocument + ", " + initPositionDocumentCover);
        StopDocumentAnim();
        rect_DocumentControl.localPosition = initPositionDocumentOfControl;
        rect_DocumentOfAndren.localPosition = initPositionDocumentOfAndren;
        rect_DocumentOpener.localRotation = initRotationDocumentOpener;
        rect_PaperOfDocument.sizeDelta = initSizePaperOfDocument;
        rect_DocumentCover.localPosition = initPositionDocumentCover;
        UIManager.instance.SetDocumentCover(true);
        //ParchmentControll.instance.SetIsPlayingDocumentAnimToFalse();
        ParchmentControll.instance.SetIsPlayingDocumentAnim(false);
        ParchmentControll.instance.atOnce = false;
        UIManager.instance.isReadParchment_In_74_79 = false;
        //UIManager.instance.SetDocumentControll(false);
    }

    // 1초 후, 안드렌의 서류 애니메이션 플레이
    public void InvokeDocumentAnim()
    {
        StopDocumentAnim();
        SettingAndrenClue(PlayerManager.instance.NumOfAct);
        Invoke("PlayDocumentAnim", 1.0f);
    }

    public void PlayDocumentAnim()
    {
        playableDirector.Play();
    }

    public void StopDocumentAnim()
    {
        playableDirector.Stop();
    }

    public void SettingAndrenClue(string numOfAct)
    {
        List<ClueStructure> tempClueLists = GameObject.Find("DataManager").GetComponent<CSVParser>().GetClueStructureLists();

        if (numOfAct.Equals("53"))
        {
            first_AndrenClue.transform.Find("ClueName").GetComponent<Text>().text = "과거 후원의 아이";
            first_AndrenClue.transform.Find("ClueText").GetComponent<Text>().text = tempClueLists.Find(x => x.GetClueName().Equals("과거 후원의 아이")).GetDesc();

            second_AndrenClue.transform.Find("ClueName").GetComponent<Text>().text = "용의자 추정";
            second_AndrenClue.transform.Find("ClueText").GetComponent<Text>().text = tempClueLists.Find(x => x.GetClueName().Equals("용의자 추정")).GetDesc();

            third_AndrenClue.transform.Find("ClueName").GetComponent<Text>().text = "슬램가의 한 남매";
            third_AndrenClue.transform.Find("ClueText").GetComponent<Text>().text = tempClueLists.Find(x => x.GetClueName().Equals("슬램가의 한 남매")).GetDesc();
        }
        else if (numOfAct.Equals("54"))
        {
            first_AndrenClue.transform.Find("ClueName").GetComponent<Text>().text = "륑 집안의 과거";
            first_AndrenClue.transform.Find("ClueText").GetComponent<Text>().text = tempClueLists.Find(x => x.GetClueName().Equals("륑 집안의 과거")).GetDesc();

            second_AndrenClue.transform.Find("ClueName").GetComponent<Text>().text = "범인의 행적";
            second_AndrenClue.transform.Find("ClueText").GetComponent<Text>().text = tempClueLists.Find(x => x.GetClueName().Equals("범인의 행적")).GetDesc();

            third_AndrenClue.transform.Find("ClueName").GetComponent<Text>().text = "...";
            third_AndrenClue.transform.Find("ClueText").GetComponent<Text>().text = tempClueLists.Find(x => x.GetClueName().Equals("...")).GetDesc();
        }
    }

    public void GetAndrenClue(string numOfAct)
    {
        if (numOfAct.Equals("53"))
        {
            GameManager.instance.GetClue("과거 후원의 아이");
            GameManager.instance.GetClue("용의자 추정");
            GameManager.instance.GetClue("슬램가의 한 남매");
        }
        else if (numOfAct.Equals("54"))
        {
            GameManager.instance.GetClue("륑 집안의 과거");
            GameManager.instance.GetClue("범인의 행적");
            GameManager.instance.GetClue("...");
        }
    }
}
