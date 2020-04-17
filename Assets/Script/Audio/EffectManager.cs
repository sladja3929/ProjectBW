using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name; //이펙트 사운드의 이름.

    public AudioClip clip; //이펙트 사운드 파일
    private AudioSource source; //이펙트 사운드 플레이어

    
    private bool loop;//루프 여부

    /*이펙트 사운드 소스 지정*/
    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.loop = loop;
        source.volume = EffectManager.instance.EffectVolume;
    }

    /*받아온 이펙트 사운드 볼륨을 실제로 적용*/
    public void SetVolume(float _volume)
    {
        source.volume = _volume;
    }

    /*이펙트 사운드 재생*/
    public void Play()
    {
        source.Play();
    }
    /*이펙트 사운드 중단*/
    public void Stop()
    {
        source.Stop();
    }
    /*이펙트 사운드 루프*/
    public void SetLoop()
    {
        source.loop = true;
    }
    /*이펙트 사운드 루프 중단*/
    public void SetLoopCancel()
    {
        source.loop = false;
    }
}

public class EffectManager : MonoBehaviour
{
    static public EffectManager instance;

    [SerializeField]
    public Sound[] sounds;//이펙트 사운드 리스트

    public float EffectVolume;//이펙트 사운드 볼륨

    //public bool EMStartEnd;


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

        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject soundObject = new GameObject(sounds[i].name);
            sounds[i].SetSource(soundObject.AddComponent<AudioSource>());
            soundObject.transform.SetParent(this.transform);
        }

        //EMStartEnd = false;
    }
   
    void Start()
    {
        EffectVolume = 1f; //초기 볼륨

        SetLoopInit();//루프 초기값

        //EMStartEnd = true;
    }
    
    /*사운드 이름을 참조해 개별 이펙트 사운드 재생 or 중단 */
    public void Play(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].Play();
                return;
            }
        }
    }
    public void Stop(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].Stop();
                return;
            }
        }
    }

    /*사운드 이름을 참조해 개별 이펙트 사운드 루프 재생 or 중단*/
    public void SetLoop(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].SetLoop();
                return;
            }
        }
    }
    public void SetLoopCancel(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].SetLoopCancel();
                return;
            }
        }
    }

    /*이펙트 사운드별 루프 재생 초기값 설정*/
    public void SetLoopInit()
    {
       //루프가 필요한 이펙트가 있다면 
    }

    /*모든 이펙트 사운드 볼륨 설정*/
    public void SetEffectVolume(float _volume)
    {
        EffectVolume = _volume;

        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].SetVolume(EffectVolume);
        }
    }
}
