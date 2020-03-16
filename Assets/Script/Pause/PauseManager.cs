using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private SettingManager theSM;

    public GameObject PausePanel;
    public GameObject SettingPanel;

    bool ispaused; // 일시정지창 오픈?
    bool issetting; // 환경설정창 오픈?


    void Start()
    {
        //theSM = SettingManager.instance;
        theSM = FindObjectOfType<SettingManager>();
        ispaused = false;
        issetting = false;
    }

    void Update()
    {
        /*키입력 받아 일시정지*/
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (ispaused == false)
            {
                //게임일시정지
                OpenPausePanel();
            }
            else if (ispaused == true && issetting == false)
            {
                //게임일시정지 해제
                ClosePausePanel();
            }
            else if (ispaused == true && issetting == true)
            {
                theSM.SaveCurSetting();//환경설정 저장
                CloseSettingPanel();
            }
        }
    }

    public void BacktoGame()
    {
        ClosePausePanel();
        //게임일시정지 해제
    }

    public void BacktoMain()
    {
        ClosePausePanel();
        //게임일시정지 해제
        //데이터저장 - 유성님께 받아서 하기 
        //타이틀화면으로 이동
    }

    public void OpenSettingPanel()
    {
        issetting = true;
        SettingPanel.SetActive(true);
    }

    void CloseSettingPanel()
    {
        issetting = false;
        SettingPanel.SetActive(false);
    }

    void OpenPausePanel()
    {
        ispaused = true;
        PausePanel.SetActive(true);
    }

    void ClosePausePanel()
    {
        ispaused = false;
        PausePanel.SetActive(false);
    }

    bool GetIsPaused()
    {
        return ispaused;
    }

    bool GetIsSetting()
    {
        return issetting;
    }
}
