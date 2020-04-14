using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading;

public class PauseManager : MonoBehaviour
{
    private SettingManager theSM;

    public GameObject PausePanel;
    public GameObject SettingPanel;
   
    private bool issetting; // 환경설정중인가?

    void Start()
    {
        //theSM = SettingManager.instance;
        theSM = FindObjectOfType<SettingManager>();
        UIManager.instance.SetIsPausedFalse();
        issetting = false;
    }

    void Update()
    {
        /*키입력 받아 일시정지*/
        if (Input.GetKeyDown(KeyCode.Escape) && !MiniMapManager.instance.IsMiniMapOpen() && !UIManager.instance.IsBookOpened())
        {
            if (UIManager.instance.GetIsPaused() == false)
            {
                UIManager.instance.SetIsPausedTrue();
                OpenPausePanel();
            }
            else if (UIManager.instance.GetIsPaused() == true && GetIsSetting() == false)
            {
                BacktoGame();
            }
            else if (UIManager.instance.GetIsPaused() == true && GetIsSetting() == true)
            {
                Debug.Log("환경설정 저장됨");
                theSM.SaveCurSetting();//환경설정 저장
                CloseSettingPanel();
            }
        }
    }

    public void BacktoGame()
    {
        EffectManager.instance.Play("버튼 클릭음");
        UIManager.instance.SetIsPausedFalse();
        ClosePausePanel();       
    }

    public void BacktoMain()
    {
        EffectManager.instance.Play("버튼 클릭음");
        ClosePausePanel();
        UIManager.instance.SetIsPausedFalse();

        StartCoroutine(LoadAsyncTitleScene());
    }

    public void OpenSettingPanel()
    {
        EffectManager.instance.Play("버튼 클릭음");
        issetting = true;        
        SettingPanel.SetActive(true);
        SettingManager.instance.GetPrevSetting();//패널 열면서 이전설정 불러오기
    }

    void CloseSettingPanel()
    {
        issetting = false;
        SettingPanel.SetActive(false);
    }

    void OpenPausePanel()
    {
        PausePanel.SetActive(true);
    }

    void ClosePausePanel()
    {
        PausePanel.SetActive(false);
    }

    bool GetIsSetting()
    {
        return issetting;
    }

    IEnumerator LoadAsyncTitleScene()
    {
        GameManager.instance.SetPlayState(GameManager.PlayState.Title);

        SceneManager.sceneLoaded += LoadingManager.instance.LoadSceneEnd;
        yield return StartCoroutine(LoadingManager.instance.Fade(true));

        float timer = 0.0f;

        LoadingManager.instance.loadSceneName = "Title_Tmp";
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Title_Tmp");

        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            yield return null;

            timer += Time.unscaledDeltaTime;

            if (asyncLoad.progress >= 0.9f)
            {
                if (timer > 2.0f)//페이크 로딩
                {
                    asyncLoad.allowSceneActivation = true;

                    timer = 0.0f;
                    yield break;
                }
            }
        }

    }
}
