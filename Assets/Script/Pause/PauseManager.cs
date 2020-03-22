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
        if (Input.GetKeyDown(KeyCode.Escape) && !MiniMapManager.instance.isMiniMapOpen())
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
        // 세이브
        GameManager.instance.thread = new Thread(GameManager.instance.SaveGameData);
        GameManager.instance.thread.IsBackground = true;
        GameManager.instance.thread.Start();

        SceneManager.LoadScene("Title_Tmp");
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
}
