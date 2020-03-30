using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cheat : MonoBehaviour
{
    public static Cheat instance;

    #region Singleton
    private void Awake()
    {
        if (instance == null)
        {
            //DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion Singleton

    public GameObject cheatpanel;
    public InputField inputfield;
    public Text InputText;

    private bool ischeatopen;

    //private string cheatword = "sugar";

    void Start()
    {
        InputText.text = "";
        cheatpanel.SetActive(false);
        ischeatopen = false;
    }

    void Update()
    {
        if (GameManager.instance.GetPlayState() == GameManager.PlayState.Act && Input.GetKeyDown(KeyCode.F12))
        {
            if (ischeatopen == true)
                HideCheatPanel();
            else
                OpenCheatPanel();
        }
    }

   
    public void InputCheatKey()
    {
        string _tmp;
        _tmp = InputText.text;

        if (_tmp == "")//아무것도 입력하지 않았을때
            return;
        else
        {
            Debug.Log("입력 성공, 입력된 값 : "+_tmp);
            ActivateCheat(_tmp);
            HideCheatPanel();
        }

        /*일정 치트키 입력해야할 때*/
            //if (_tmp == cheatword)//맞음
            //{
            //    HideCheatPanel();

            //    Debug.Log("치트키 입력 성공 ... ");
            //}
            //else//틀림
            //{
            //    HideCheatPanel();

            //    Debug.Log("치트키 틀림");
            //}
        
    }

    public void OpenCheatPanel()
    {
        this.cheatpanel.SetActive(true);
        inputfield.text = "";
        ischeatopen = true;
    }

    public void HideCheatPanel()
    {
        //InputText.text = "";
        inputfield.Select();
        inputfield.text = "";
        ischeatopen = false;

        cheatpanel.SetActive(false);
    }

    private void ActivateCheat(string _tmp)
    {
        DialogManager.instance.CheatConversation(_tmp);
    }
}
