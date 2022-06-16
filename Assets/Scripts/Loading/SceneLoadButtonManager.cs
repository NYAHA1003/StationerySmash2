using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill.Load;
using Utill.Tool;
using Main.Deck;
using Main.Setting;
public class SceneLoadButtonManager : MonoBehaviour
{
    [SerializeField]
    private Button[] buttons;
    [SerializeField]
    private LoadingBattleDataSO loadingBattleDataSO;
    [SerializeField]
    private SceneLoadComponenet _sceneLoadComponenet;
    [SerializeField]
    private AIDatasSO aIDataSO;
    [SerializeField]
    private Sprite[] _stageSprites;
    [SerializeField]
    private StageDetailPopupPanel _popupPanel = null;
    [SerializeField]
    private StageDataListSO _stageDataListSO = null;
    [SerializeField]
    private CurrentStageData _currentDataSO = null;

    private WarrningComponent _warrningComponent = null; //��� ������Ʈ


    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log("��ư �ε���");
        _warrningComponent = FindObjectOfType<WarrningComponent>();
           buttons[0].onClick.AddListener(() => LoadBattleDataStageMake(BattleStageType.ST_MAKE));
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
        Sound.PlayEff(2);
        Debug.Log($"{battleStageType} is loding...");
        if ((int)UserSaveManagerSO.UserSaveData._lastPlayStage - (int)battleStageType < -1)
        {
            _warrningComponent.SetWarrning("���� ���������� Ŭ�����ؾ��մϴ�.");
            return;
        }
        loadingBattleDataSO.SetCurrentIndex(battleStageType);
        var currentData = loadingBattleDataSO.CurrentStageData;
        PencilCaseDataManagerSO.SetEnemyPencilCaseData(currentData);
        aIDataSO.SetAIData(currentData);
        _currentDataSO._currentStageDatas = _stageDataListSO.stageDatas.Find(x => x._stageType == battleStageType);
        _popupPanel.Setting();
    }
    private void LoadBattleDataStageMake(BattleStageType battleStageType)
    {
        Sound.PlayEff(2);
        Debug.Log($"{battleStageType} is loding...");
        loadingBattleDataSO.SetCurrentIndex(battleStageType);
        var currentData = loadingBattleDataSO.CurrentStageData;
        PencilCaseDataManagerSO.SetEnemyPencilCaseData(currentData);
        aIDataSO.SetAIData(currentData);
        _sceneLoadComponenet.SceneLoadStageMake();
    }
}
