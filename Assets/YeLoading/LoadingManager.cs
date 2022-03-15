using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoadingManager : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(LoadSceneProcess());
    }
    private static string nextScene;
    [SerializeField]
    private Image progressBar_Image;
    /// <summary>
    /// �� �ε��ϴ� �Լ�
    /// </summary>
    /// <param name="sceneName">�ε��� ���� �̸�</param>
    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    private IEnumerator LoadSceneProcess()
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
}
