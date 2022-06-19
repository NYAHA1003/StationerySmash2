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
    private LoadingBattleDataSO loadingBattleDataSO;
    [SerializeField]
    private SceneLoadComponenet _sceneLoadComponenet;
    [SerializeField]
    private Sprite[] _stageSprites;
    [SerializeField]
    private StageDetailPopupPanel _popupPanel = null;
    [SerializeField]
    private StageDataListSO _stageDataListSO = null;

    private WarrningComponent _warrningComponent = null; //경고 컴포넌트


    // Start is called before the first frame update
    private void Start()
    {
        _warrningComponent = FindObjectOfType<WarrningComponent>();
           buttons[0].onClick.AddListener(() => LoadBattleDataStageMake(BattleStageType.ST_MAKE));
        SetBattleLoadButtons();
    }
   
    private void SetBattleLoadButtons()
    {
        for (int i = 1; i < System.Enum.GetValues(typeof(BattleStageType)).Length; i++)
        {
            //각 버튼에 enum값 대입하기
            int temp = i;
            if(UserSaveManagerSO.UserSaveData._lastPlayStage >= (BattleStageType)i) //스테이지 클리어
            {
                buttons[temp].GetComponent<Image>().sprite = _stageSprites[2];
            }
            else if ((int)UserSaveManagerSO.UserSaveData._lastPlayStage - i == -1) //스테이지 도전 가능
            {
                buttons[temp].GetComponent<Image>().sprite = _stageSprites[1];
            }
            else if (UserSaveManagerSO.UserSaveData._lastPlayStage < (BattleStageType)i) //스테이지 미클리어
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
            _warrningComponent.SetWarrning("이전 스테이지를 클리어해야합니다.");
            return;
        }
        loadingBattleDataSO.SetCurrentIndex(battleStageType);
        var currentData = loadingBattleDataSO.CurrentStageData;
        PencilCaseDataManagerSO.SetEnemyPencilCaseData(currentData);
        AIAndStageData.Instance.SetAIData(currentData);
        AIAndStageData.Instance._currentStageDatas = _stageDataListSO.stageDatas.Find(x => x._stageType == battleStageType);
        _popupPanel.Setting();
    }
    private void LoadBattleDataStageMake(BattleStageType battleStageType)
    {
        Debug.Log($"{battleStageType} is loding...");
        loadingBattleDataSO.SetCurrentIndex(battleStageType);
        var currentData = loadingBattleDataSO.CurrentStageData;
        PencilCaseDataManagerSO.SetEnemyPencilCaseData(currentData);
        AIAndStageData.Instance.SetAIData(currentData);
        _sceneLoadComponenet.SceneLoadStageMake();
    }
}
