using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class TitleManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void NewGame()
    {
        SceneManager.LoadScene("BW_H");
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
