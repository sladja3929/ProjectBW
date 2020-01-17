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

    // Use this for initialization
    void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    // 1초 후, 안드렌의 서류 애니메이션 플레이
    public void InvokeDocumentAnim()
    {
        Invoke("PlayDocumentAnim", 1.0f);
    }

    public void PlayDocumentAnim()
    {
        playableDirector.Play();
    }

}
