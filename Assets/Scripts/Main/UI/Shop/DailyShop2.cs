using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyShop2 : MonoBehaviour
{
    private List<int> FreeStore = new List<int>();
    private List<string> FreeStoreName = new List<string>();

    [SerializeField]
    private int unitMin;
    [SerializeField]
    private int unitMax;
    [SerializeField]
    private int stickerMin;
    [SerializeField]
    private int stickerMax;
    [SerializeField]
    private int badgeMin;
    [SerializeField]
    private int badgeMax;
    [SerializeField]
    private int goldMin;
    [SerializeField]
    private int goldMax;
    [SerializeField]
    private int dalgonaMin;
    [SerializeField]
    private int dalgonaMax;

    [SerializeField]
    private Button freeButton;
    [SerializeField]
    private Button Button1;
    [SerializeField]
    private Button Button2;
    [SerializeField]
    private Button Button3;
    [SerializeField]
    private Button Button4;
    [SerializeField]
    private Button Button5;

    private int random;
    private int amount;

    void Start()
    {
        ResetFunctionPakage_UI();
        SetFunctionPakage_UI();
    }

    private void ResetFunctionPakage_UI()
    {
        freeButton.onClick.RemoveAllListeners();
        //Button1.onClick.RemoveAllListeners();
        //Button2.onClick.RemoveAllListeners();
        //Button3.onClick.RemoveAllListeners();
        //Button4.onClick.RemoveAllListeners();
        //Button5.onClick.RemoveAllListeners();
    }

    /// <summary>
    /// 이벤트 세팅
    /// </summary>
    private void SetFunctionPakage_UI()
    {
        freeButton.onClick.AddListener(freeButtonSet);
        //Button1.onClick.AddListener(ButtonClick);

        SetfreeList();
    }

    private void SetfreeList()
    {
        FreeStore.Add(0);
        FreeStore.Add(1);

        FreeStoreName.Add("골드");
        FreeStoreName.Add("달고나");
    }

    private void freeButtonSet()
    {
        int min = 0, max = 0;
        random = Random.Range(0, FreeStoreName.Count);
        //이 버튼의 그림 = 에셋 리스트[random];
        switch (random)
        {
            case 0:
                min = goldMin;
                max = goldMax;
                break;
            case 1:
                min = dalgonaMin;
                max = dalgonaMax;
                break;
        }
        amount = Random.Range(min, max + 1);
        Debug.Log($"{FreeStore[random]}, {amount}개");
    }

    private void ButtonClick()
    {

    }
}
