using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class LoadingManager : MonoBehaviour
{
    private static string nextScene;
    private int previousRandomNum = 0;
    [SerializeField]
    private Image progressBar_Image;
    [SerializeField]
    private string[] tip_StrList;
    [SerializeField]
    private TextMeshProUGUI tip_Text;
    [Range (0, 5)]
    public float repeatTerm;
    void Start()
    {
        StartCoroutine(LoadSceneProcess());
        InvokeRepeating("Random_Tips", repeatTerm, 0f);
    }
    /// <summary>
    /// 씬 로드하는 함수
    /// </summary>
    /// <param name="sceneName">로드할 씬의 이름</param>
    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    public IEnumerator LoadSceneProcess()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(nextScene);
        operation.allowSceneActivation = false;

        float timer = 0f;
        while(!operation.isDone)
        {
            yield return null;
                timer += Time.deltaTime;
            if(operation.progress < 0.9)    //90퍼까지는 정상적으로 로딩을 나타냄
            {
                progressBar_Image.fillAmount = Mathf.Lerp(progressBar_Image.fillAmount, operation.progress, timer);
                //progressBar_Image.fillAmount = operation.progress;
                if (progressBar_Image.fillAmount >= operation.progress)
                {
                    timer = 0f;
                }
            }
            else                           //그 이후엔 페이크로딩 진행
            {
                progressBar_Image.fillAmount = Mathf.Lerp(progressBar_Image.fillAmount, 1f, timer);
                if(progressBar_Image.fillAmount >=1f)
                {
                    operation.allowSceneActivation = true;
                    yield break;
                }
            }
        }


    }

    private void Random_Tips()
    {
        int random = Random.Range(0, tip_StrList.Length);
        if(previousRandomNum == random)
            random = Random.Range(0, tip_StrList.Length);
        tip_Text.text = tip_StrList[random];
        previousRandomNum = random;
    }
}
