using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    static public LoadingManager instance;

    public static LoadingManager Instance

    {

        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<LoadingManager>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    instance = Create();
                }
            }
            return instance;
        }

        private set
        {
            instance = value;
        }
    }

    private void Awake()
    {
        if (Instance != this)

        {

            Destroy(gameObject);

            return;

        }



        DontDestroyOnLoad(gameObject);

    }

    public bool isLoading;

    [SerializeField]

    private CanvasGroup LoadingCanvasGroup;

    [SerializeField]
    public string loadSceneName;

    public GameObject LoadingCanvas;

    void Start()
    {
        isLoading = false;
    }

    void Update()
    {
        
    }

    public static LoadingManager Create()

    {
        LoadingManager SceneLoaderPrefab = Resources.Load<LoadingManager>("LoadingManager");

        return Instantiate(SceneLoaderPrefab);

    }


    //private IEnumerator Load(string sceneName)

    //{
    //    yield return StartCoroutine(Fade(true));

    //    AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);

    //    op.allowSceneActivation = false;

    //    float timer = 0.0f;

    //    while (!op.isDone)
    //    {
    //        yield return null;
    //        timer += Time.unscaledDeltaTime;

    //        if (op.progress > 0.9f)
    //        {
    //            if (timer > 3.0f)
    //            {
    //                op.allowSceneActivation = true;
    //                yield break;
    //            }
    //        }
    //    }
    //}

    public void LoadSceneEnd(Scene curscene, LoadSceneMode loadSceneMode)
    {
        Debug.Log("LoadSceneEnd 발동");
        if (curscene.name == loadSceneName)
        {
            StartCoroutine(Fade(false));
            SceneManager.sceneLoaded -= LoadSceneEnd;
        }
    }

    public IEnumerator Fade(bool isLoadingfadeIn)
    {
        float timer = 0f;

        if (isLoadingfadeIn)
        {
            LoadingCanvas.SetActive(true);
        }

        while (timer <= 1f)
        {
            yield return null;
            timer += Time.unscaledDeltaTime * 2f;
            LoadingCanvasGroup.alpha = Mathf.Lerp(isLoadingfadeIn ? 0 : 1, isLoadingfadeIn ? 1 : 0, timer);
        }

        if (!isLoadingfadeIn)
        {
            LoadingCanvas.SetActive(false);
        }

    }
}
