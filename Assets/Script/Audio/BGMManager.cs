using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    static public BGMManager instance;

    public AudioClip[] clips; // BGM 리스트
    private AudioSource source;

    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    private float BGMVolume; //BGM 볼륨

    private int BGMnowplaying; //현재 재생중인 BGM

    //public bool BMStartEnd;

    /*씬 이름들*/
    public const string titlescene = "Title_Tmp";
    public const string gamescene = "BW_H";
    public const string prologuescene = "Prologue";

    /*튜토용 트리거 1*/
    public bool tutorialBGMtrigger_1;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        source = this.GetComponent<AudioSource>();

        //BMStartEnd = false;
    }

    void Start()
    {
        tutorialBGMtrigger_1 = false; ;

        AutoSelectBGM(SceneManager.GetActiveScene(),LoadSceneMode.Single);//BGM 셀렉
        PlayBGM(BGMnowplaying);//BGM 재생

        SceneManager.sceneLoaded += AutoSelectBGM;

        //BMStartEnd = true;
    }

    /*씬 변경 시마다 BGM 적용*/
    /*BGM 자동선택 - 씬과 플레이어 위치 판별*/
    public void AutoSelectBGM(Scene scene, LoadSceneMode loadSceneMode)
    {
        int preBGMnowPlaying = BGMnowplaying;
        int nextBGMnowPlaying = preBGMnowPlaying;

        string curscene = scene.name;

        if (curscene == titlescene)//타이틀 씬 (메인)
        {
            nextBGMnowPlaying = 0;
        }
        else if (curscene == gamescene)// 인게임 씬
        {
                string curpos = PlayerManager.instance.GetCurrentPosition();
                string curhighpos = PlayerManager.instance.GetHigherCurrentPosition();
                nextBGMnowPlaying = GetAreaBGM(curpos, curhighpos);
        }
        else if (curscene == prologuescene)
        {
            nextBGMnowPlaying = 1;
        }
        //이후 엔딩 추가


        /*BGM 변경 시*/
        if (preBGMnowPlaying != nextBGMnowPlaying)
        {
            BGMnowplaying = nextBGMnowPlaying;
            FadeOutBGM();
            //StopBGM();
            PlayBGM(BGMnowplaying);
            FadeInBGM();
        }           
    }

    /*플레이어의 위치에 따른 BGM변경*/
    private int GetAreaBGM(string curpos_, string curhighpos_)
    {
        int area_num;
        switch (curhighpos_)
        {           
            case "Forest":
            case "Mansion":
            case "Chapter":
                area_num = 2;
                break;
            case "Downtown":
            case "Market":
                area_num = 3;
                break;
            case "Village":
            case "Harbor":
                area_num = 4;
                break;
            case "Slum":
                area_num = 5;
                break;
            default:
                area_num = 0;
                break;
        }

        switch (curpos_)
        {
            case "Harbor_Cruise":
                area_num = 6;
                break;
            case "Harbor_Prison":
                area_num = 7;
                break;
            case "Forest_Bro_sis_home":
                area_num = 8;
                break;
            case "Village_Raina_House":
                area_num = 9;
                break;
            case "Village_Balrua_House":
                area_num = 10;
                break;
            case "Mansion_President_Mansion_Outhouse":
            case "Mansion_Girls_Room":
            case "Mansion_Boys_Room":
            case "Mansion_Study_Room":
            case "Mansion_Dining_Room":
                area_num = 11;
                break;
            case "Downtown_Real_estate":
                area_num = 12;
                break;
            case "Downtown_Salon":
                area_num = 13;
                break;
            case "Mansion_Guest_Room1":
            case "Mansion_Guest_Room2":
            case "Mansion_Viscount_Mansion_First_Floor":
            case "Mansion_Viscount_Mansion_Second_Floor":
                area_num = 14;
                break;
            case "Slum_Information_agency":
                area_num = 15;
                break;
            case "Chapter_Chapter_First_Floor":
            case "Chapter_Chapter_Second_Floor":
            case "Chapter_Merte_Office":
            case "Chapter_Zaral_Office":
            case "Chapter_President_Office":
                area_num = 16;
                break;
            case "Chapter_Secret_Space":
                area_num = 17;
                break;
            case "Mansion_President_Room":
            case "Mansion_President_Mansion_First_Floor":
            case "Mansion_President_Mansion_Second_Floor":
                area_num = 18;
                break;
            case "Downtown_Cafe":
                area_num = 19;
                break;
            default:
                break;
        }

        return area_num;
    }

    /*BGM 재생*/
    public void PlayBGM(int _playMusicTrack)
    {
        source.volume = BGMVolume;
        source.clip = clips[_playMusicTrack];
        source.Play();
    }

    /*볼륨 조정*/
    public void SetBGMVolume(float _volume)
    {
        BGMVolume = _volume;
        source.volume = BGMVolume;
    }

    /*음악 일시 정지*/
    public void PauseBGM()
    {
        source.Pause();
    }

    /*음악 일시 정지 해제*/
    public void UnpauseBGM()
    {
        source.UnPause();
    }

    /*음악 재생 중단*/
    public void StopBGM()
    {
        source.Stop();
    }

    /*음악 페이드 아웃*/
    public void FadeOutBGM()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutBGMCoroutine());
    }

    IEnumerator FadeOutBGMCoroutine()
    {
        for (float i = BGMVolume; i >= 0f; i -= 0.01f)
        {
            source.volume = i;
            yield return waitTime;
        }
    }

    /*음악 페이드 인*/
    public void FadeInBGM()
    {
        StopAllCoroutines();
        StartCoroutine(FadeInBGMCoroutine());
    }
    IEnumerator FadeInBGMCoroutine()
    {
        for (float i = 0f; i <= BGMVolume; i += 0.01f)
        {
            source.volume = i;
            yield return waitTime;
        }
    }
}
