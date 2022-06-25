using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill.Load;
using Utill.Tool;
using Main.Deck;

public class SceneLoadButtonManager : MonoBehaviour
{
    [SerializeField]
    private Button[] buttons;
    [SerializeField]
    private Sprite[] _stageSprites;
    private InitLoadButton initLoadButton;
    private WarrningComponent _warrningComponent = null; //��� ������Ʈ


    // Start is called before the first frame update
    private void Start()
    {
        initLoadButton = GetComponent<InitLoadButton>();
        _warrningComponent = FindObjectOfType<WarrningComponent>();
         buttons[0].onClick.AddListener(() => initLoadButton.LoadBattleDataStageMake(BattleStageType.ST_MAKE));
        SetBattleLoadButtons();
    }
   
    private void SetBattleLoadButtons()
    {
        for (int i = 1; i < System.Enum.GetValues(typeof(BattleStageType)).Length; i++)
        {
            //�� ��ư�� enum�� �����ϱ�
            int temp = i;
            if(UserSaveManagerSO.UserSaveData._lastPlayStage >= (BattleStageType)i) //�������� Ŭ����
            {
                buttons[temp].GetComponent<Image>().sprite = _stageSprites[2];
            }
            else if ((int)UserSaveManagerSO.UserSaveData._lastPlayStage - i == -1) //�������� ���� ����
            {
                buttons[temp].GetComponent<Image>().sprite = _stageSprites[1];
            }
            else if (UserSaveManagerSO.UserSaveData._lastPlayStage < (BattleStageType)i) //�������� ��Ŭ����
            {
                buttons[temp].GetComponent<Image>().sprite = _stageSprites[0];
            }
            buttons[temp].onClick.AddListener(() => LoadBattleData((BattleStageType)temp));
        }
    }
    private void LoadBattleData(BattleStageType battleStageType)
    {
        Debug.Log($"{battleStageType} is loding...");
        if ((int)UserSaveManagerSO.UserSaveData._lastPlayStage - (int)battleStageType < -1)
        {
            _warrningComponent.SetWarrning("���� ���������� Ŭ�����ؾ��մϴ�.");
            return;
        }
        initLoadButton.InitBattleData(battleStageType);
    }
    
}
