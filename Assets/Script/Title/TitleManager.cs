using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class TitleManager : MonoBehaviour
{

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
            if (SettingManager.instance.GetIsSetting() == true)
            {
                SettingManager.instance.CloseSettingPanel();
            }
        }
    }

    // LoadDataForScene 에는 CSV 데이터 등을 로드하는 함수를 넣으면 됨
    IEnumerator LoadAsyncNewGameScene(CorrectLoadSceneFunc LoadDataForScene)
    {
        SceneManager.sceneLoaded += LoadingManager.instance.LoadSceneEnd;
        yield return StartCoroutine(LoadingManager.instance.Fade(true));

        

        AsyncOperation asyncLoad;

        float timer = 0.0f;

        if (GameManager.instance.GetGameState() == GameManager.GameState.NewGame_Loaded)
        {
            LoadingManager.instance.loadSceneName = "Prologue";
            asyncLoad = SceneManager.LoadSceneAsync("Prologue");

            asyncLoad.allowSceneActivation = false;

            LoadDataForScene();

            while (!CSVParser.instance.CompleteLoadFile())
            {
                yield return null;
            }

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
        else if (GameManager.instance.GetGameState() == GameManager.GameState.PastGame_Loaded)
        {
            LoadingManager.instance.loadSceneName = "BW_H";
            asyncLoad = SceneManager.LoadSceneAsync("BW_H");

            asyncLoad.allowSceneActivation = false;

            LoadDataForScene();

            while (!CSVParser.instance.CompleteLoadFile())
            {
                //Debug.Log(CSVParser.instance.GetInteractionLists().Count);
                //yield return new WaitForSeconds(1.0f);
                yield return null;
            }
            

            //Debug.Log("일단 이어하기로 loadSceneName : " + LoadingManager.instance.loadSceneName);
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

    // 처음부터
    public void NewGame()
    {
        //SceneManager.LoadScene("BW_H");
        GameManager.instance.SetGameState(GameManager.GameState.NewGame_Loaded);
        
        // 데이터 파일 체크
        if (CSVParser.instance.CheckSaveData())
        {
            //Debug.Log("처음하기 로딩 성공");
            StartCoroutine(LoadAsyncNewGameScene(GameManager.instance.PlayNewGame));
        }
        else
        {
            //Debug.Log("처음하기 로딩 실패");
            GameManager.instance.SetGameState(GameManager.GameState.Idle);
        }
    }

    // 이어하기
    public void PlayPastGame()
    {
        GameManager.instance.SetGameState(GameManager.GameState.PastGame_Loaded);
        GameManager.instance.SetPlayState(GameManager.PlayState.Act);

        // 데이터 파일 체크
        if (CSVParser.instance.CheckSaveData())
        {
            //Debug.Log("이어하기 로딩 성공, " + GameManager.instance.GetGameState().ToString());
            StartCoroutine(LoadAsyncNewGameScene(GameManager.instance.PlaySaveGame));
        }
        else
        {
            //Debug.Log("이어하기 로딩 실패");
            GameManager.instance.SetGameState(GameManager.GameState.Idle);
        }
    }

    //설정
    public void Setting()
    {
        SettingManager.instance.OpenSettingPanel();
    }

    //종료하기
    public void GameOver()
    {
        #if UNITY_EDITOR

                UnityEditor.EditorApplication.isPlaying = false;
        #else
                    Application.Quit();
        
        #endif

    }
}
