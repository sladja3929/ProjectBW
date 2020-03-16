using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{

    public static SettingManager instance;

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

    /*환경설정.. csv로 혹은 playerprefs로 저장해야할 것 같습니다. 일단은 로컬로 구현*/

    /*재생속도 상수*/
    public const float pv_1 = 0.7f;
    public const float pv_2 = 1.0f;
    public const float pv_3 = 1.3f;

    /*환경설정 임시변수 */

    private int brightness; //(0 - 100)
    private int displaymode;
    private int bgmvolume; //(0 - 100)
    private int effectvolume; //(0 - 100)
    private int playvelocity;//(0-2)

    /*playerpref으로 유지되는 환경설정 변수 부가적으로 관리하도록 하기*/

    /*각종 조절 게임 오브젝트 변수*/
    public Slider brightness_S;
    public Slider bgmvolume_S;
    public Slider effectvolume_S;
    public Dropdown displaymode_D;
    public Toggle play_slow;
    public Toggle play_mid;
    public Toggle play_fast;

    void Start()
    {
        GetPrevSetting();
    }

  
    /*설정 변경 시 적용 - EventSystem On value changed 사용*/
    public void UpdateBrightness()
    {
        brightness = Mathf.RoundToInt(brightness_S.value);
        Debug.Log(brightness);
    }
    public void UpdateBGMVolume()
    {
        bgmvolume = Mathf.RoundToInt(bgmvolume_S.value);
        Debug.Log(bgmvolume);
    }
    public void UpdateEffectVolume()
    {
        effectvolume = Mathf.RoundToInt(effectvolume_S.value);
        Debug.Log(effectvolume);
    }
    public void UpdateDisplayMode()
    {
        displaymode = displaymode_D.value;
        Debug.Log(displaymode);
    }
    public void UpdatePlaySlow()
    {
        play_mid.isOn = false;
        play_fast.isOn = false;
        playvelocity = 0;
    }
    public void UpdatePlayMid()
    {
        play_slow.isOn = false;
        play_fast.isOn = false;
        playvelocity = 1;
    }
    public void UpdatePlayFast()
    {
        play_slow.isOn = false;
        play_mid.isOn = false;
        playvelocity = 2;
    }

    /*이전 설정값 불러오기 PlayerPref - 기본 설정도 다 되어있음 ^^*/
    void GetPrevSetting()
    {

    }
    /*현재 설정 PlayerPref에 저장*/
    public void SaveCurSetting()
    {
        
    }
}
