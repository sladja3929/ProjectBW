using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class TitleManager : MonoBehaviour
{

    public GameObject SettingPanel;

    private bool issetting; // 환경설정중인가?

    // Scene을 Load하는 함수를 담을 Delegate
    public delegate void CorrectLoadSceneFunc();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (issetting == true)
            {
                SettingManager.instance.SaveCurSetting();
                CloseSettingPanel();
            }
        }
    }

    // LoadDataForScene 에는 CSV 데이터 등을 로드하는 함수를 넣으면 됨
    IEnumerator LoadAsyncNewGameScene(CorrectLoadSceneFunc LoadDataForScene)
    {
        //GameManager.instance.PlayNewGame();
        LoadDataForScene();

        while (!CSVParser.instance.CompleteLoadFile())
        {
            yield return null;
        }

        AsyncOperation asyncLoad;
        
        if (GameManager.instance.GetGameState() == GameManager.GameState.NewGame_Loaded)
        {
            asyncLoad = SceneManager.LoadSceneAsync("Prologue");

            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
        else if (GameManager.instance.GetGameState() == GameManager.GameState.PastGame_Loaded)
        {
            asyncLoad = SceneManager.LoadSceneAsync("BW_K");

            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
    }

    // 처음부터
    public void NewGame()
    {
        //SceneManager.LoadScene("BW_H");
        GameManager.instance.SetGameState(GameManager.GameState.NewGame_Loaded);
        
        // 데이터 파일 체크
        if (CSVParser.instance.CheckSaveData())
        {
            Debug.Log("처음하기 로딩 성공");
            StartCoroutine(LoadAsyncNewGameScene(GameManager.instance.PlayNewGame));
        }
        else
        {
            Debug.Log("처음하기 로딩 실패");
            GameManager.instance.SetGameState(GameManager.GameState.Idle);
        }
    }

    // 이어하기
    public void PlayPastGame()
    {
        GameManager.instance.SetGameState(GameManager.GameState.PastGame_Loaded);

        // 데이터 파일 체크
        if (CSVParser.instance.CheckSaveData())
        {
            Debug.Log("이어하기 로딩 성공, " + GameManager.instance.GetGameState().ToString());
            StartCoroutine(LoadAsyncNewGameScene(GameManager.instance.PlaySaveGame));
        }
        else
        {
            Debug.Log("이어하기 로딩 실패");
            GameManager.instance.SetGameState(GameManager.GameState.Idle);
        }
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


    public void GameOver()
    {
        #if UNITY_EDITOR

                UnityEditor.EditorApplication.isPlaying = false;
        #else
                    Application.Quit();
        
        #endif

    }
}
