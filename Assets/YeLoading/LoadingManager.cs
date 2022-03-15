using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoadingManager : MonoBehaviour
{
    private static string nextScene;
    private int previousRandomNum = 0;
    [SerializeField]
    private Image progressBar_Image;
    [SerializeField]
    private string[] tip_StrList;
    [SerializeField]
    private Text tip_Text;
    [Range (0, 5)]
    public float repeatTerm;
    void Start()
    {
        StartCoroutine(LoadSceneProcess());
        InvokeRepeating("Random_Tips", repeatTerm, 0f);
    }
    /// <summary>
    /// �� �ε��ϴ� �Լ�
    /// </summary>
    /// <param name="sceneName">�ε��� ���� �̸�</param>
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
        while(operation.isDone)
        {
            yield return null;
            if(operation.progress < 0.9)    //90�۱����� ���������� �ε��� ��Ÿ��
            {
                progressBar_Image.fillAmount = operation.progress;
            }
            else                           //�� ���Ŀ� ����ũ�ε� ����
            {
                timer += Time.unscaledDeltaTime;
                progressBar_Image.fillAmount = Mathf.Lerp(0.9f, 1f, timer);
                if(operation.progress >=1f)
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
