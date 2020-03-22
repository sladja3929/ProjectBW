using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DocumentControll : MonoBehaviour
{
    [SerializeField]
    private PlayableDirector playableDirector;  // 안드렌의 서류 애니메이션을 담당할 변수
    //[SerializeField]
    //private TimelineAsset timeLine; // 별도의 타임라인 애니메이션을 사용할 때 사용하는 변수 ex) ~.Play(timeLine);

    public static DocumentControll instance = null;

    [SerializeField]
    private RectTransform rect_DocumentOfAndren;
    [SerializeField]
    private RectTransform rect_DocumentOpener;
    [SerializeField]
    private RectTransform rect_PaperOfDocument;
    [SerializeField]
    private RectTransform rect_DocumentCover;

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
        initPositionDocumentOfAndren = rect_DocumentOfAndren.localPosition;
        initRotationDocumentOpener = rect_DocumentOpener.localRotation;
        initSizePaperOfDocument = rect_PaperOfDocument.sizeDelta;
        initPositionDocumentCover = rect_DocumentCover.localPosition;
    }

    public void ResetDocumentOfAndren()
    {
        rect_DocumentOfAndren.localPosition = initPositionDocumentOfAndren;
        rect_DocumentOpener.localRotation = initRotationDocumentOpener;
        rect_PaperOfDocument.sizeDelta = initSizePaperOfDocument;
        rect_DocumentCover.localPosition = initPositionDocumentCover;
        ParchmentControll.instance.SetIsPlayingDocumentAnimToFalse();
        //UIManager.instance.SetDocumentControll(false);
    }

    // 1초 후, 안드렌의 서류 애니메이션 플레이
    public void InvokeDocumentAnim()
    {
        playableDirector.Stop();
        Invoke("PlayDocumentAnim", 1.0f);
        //UIManager.instance.SetDocumentCover(true);
    }

    public void PlayDocumentAnim()
    {
        playableDirector.Play();
    }

}
