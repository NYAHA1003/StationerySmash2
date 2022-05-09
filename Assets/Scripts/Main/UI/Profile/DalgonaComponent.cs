using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Main.Deck;
public class DalgonaComponent : MonoBehaviour, IUserData
{
    [SerializeField]
    private TextMeshProUGUI _dalgonaText = null;

    private int _currentDalgona = 0;
    private int _previousDalgona = 0;


    public void Awake()
    {
        SaveManager._instance.SaveData.AddObserver(this);
        _previousDalgona = SaveManager._instance.SaveData.userSaveData._dalgona;
        SetDalgonaText();
    }

    public void Notify(ref UserSaveData userSaveData)
    {
        _currentDalgona = userSaveData._dalgona;

        SetDalgonaText();
        StartCoroutine(UpCountingDalgona());
    }

    /// <summary>
    /// 돈 텍스트 값 수정
    /// </summary>
    public void SetDalgonaText()
    {
        _dalgonaText.text = _previousDalgona.ToString();
    }

    /// <summary>
    /// 돈 업데이트
    /// </summary>
    /// <returns></returns>
    private IEnumerator UpCountingDalgona()
    {
        float interval = 0.05f;
        while (_previousDalgona < _currentDalgona)
        {
            _previousDalgona++;
            SetDalgonaText();

            yield return new WaitForSeconds(interval);
        }
    }

}
