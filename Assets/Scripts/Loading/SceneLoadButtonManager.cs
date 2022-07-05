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
    private Button[] stageButtons;
    [SerializeField]
    private Button[] eventButtons;
    [SerializeField]
    private Sprite[] _stageSprites;
    private InitLoadButton initLoadButton;
    private WarrningComponent _warrningComponent = null; //��� ������Ʈ


    // Start is called before the first frame update
    private void Start()
    {
        initLoadButton = GetComponent<InitLoadButton>();
        _warrningComponent = FindObjectOfType<WarrningComponent>();
        stageButtons[0].onClick.AddListener(() => initLoadButton.LoadBattleDataStageMake(BattleStageType.ST_MAKE));
        LoadEventLoadButtons();
        SetBattleLoadButtons();
    }

    private void SetBattleLoadButtons()
    {
        int i = 1;
        while ((int)BattleStageType.SNormalMAx != (int)(BattleStageType)i)
        //for (int i = 1; i < System.Enum.GetValues(typeof(BattleStageType)).Length; i++)
        {
            //�� ��ư�� enum�� �����ϱ�
            //int temp = i;
            int temp = i;
            if (UserSaveManagerSO.UserSaveData._lastPlayStage >= (BattleStageType)i) //�������� Ŭ����
            {
                stageButtons[temp].GetComponent<Image>().sprite = _stageSprites[2];
            }
            else if ((int)UserSaveManagerSO.UserSaveData._lastPlayStage - i == -1) //�������� ���� ����
            {
                stageButtons[temp].GetComponent<Image>().sprite = _stageSprites[1];
            }
            else if (UserSaveManagerSO.UserSaveData._lastPlayStage < (BattleStageType)i) //�������� ��Ŭ����
            {
                stageButtons[temp].GetComponent<Image>().sprite = _stageSprites[0];
            }
            stageButtons[temp].onClick.AddListener(() => LoadBattleData((BattleStageType)temp));
            i++;
        }
    }
    private void LoadEventLoadButtons()
    {
        int j = 0;
        int k = 0;
        for (int i = (int)BattleStageType.E_Money; i < (int)BattleStageType.EMax; i++)
        {
            k = i;
            eventButtons[j++].onClick.AddListener(() => LoadEventData((BattleStageType)k));
        }
    }
    private void LoadEventData(BattleStageType battleStageType)
    {
        initLoadButton.InitBattleData(battleStageType);
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
