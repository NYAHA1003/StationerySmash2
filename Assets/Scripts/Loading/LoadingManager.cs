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
    private Slider progressBar;
    [SerializeField]
    private string[] tip_StrList;
    [SerializeField]
    private TextMeshProUGUI tip_Text;
    [Range (0, 5)]
    public float repeatTerm;
    [SerializeField, Header("스프라이트로딩시스템 BattleSetSkin"), Space(30)]
    private SetSkinComponent _loadingComponent = null;
    private void Awake()
    {
        StartCoroutine(Random_Tips());
    }
    void Start()
    {
        StartCoroutine(LoadSceneProcess());
        _loadingComponent.LoadSkin();
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
        yield return new WaitForSeconds(0.5f); 
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false; 
        
            float timer = 0.0f;
            while (!op.isDone)
            {
                yield return null;
                if (op.progress < 0.9f)
                {
                    progressBar.value = op.progress;


                }
                else
                {
                    timer += Time.deltaTime;
                    progressBar.value = Mathf.Lerp(0.9f,1f, timer * 0.5f);
                    if (progressBar.value >= 1.0f)
                    {
                        op.allowSceneActivation = true;
                        yield break;
                    }
                }

            }


    }

    private IEnumerator Random_Tips()
    {
        int random = Random.Range(0, tip_StrList.Length);
        if(previousRandomNum == random)
            random = Random.Range(0, tip_StrList.Length);
        tip_Text.text = tip_StrList[random];
        previousRandomNum = random;
        Debug.Log("radom");
        yield return new WaitForSeconds(1f);
    }
}
