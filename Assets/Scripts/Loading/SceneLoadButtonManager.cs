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
    private AIDataSO aIDataSO;
    [SerializeField]
    private Sprite[] _stageSprites;


    // Start is called before the first frame update
    private void Awake()
    {
        buttons[0].onClick.AddListener(() => LoadBattleDataStageMake(BattleStageType.ST_MAKE));
        SetBattleLoadButtons();
    }
   
    private void SetBattleLoadButtons()
    {
        for (int i = 1; i < System.Enum.GetValues(typeof(BattleStageType)).Length; i++)
        {
            //각 버튼에 enum값 대입하기
            int temp = i;
            buttons[temp].onClick.AddListener(() => LoadBattleData((BattleStageType)temp));
        }
    }
    private void LoadBattleData(BattleStageType battleStageType)
    {
        Debug.Log($"{battleStageType} is loding...");
        loadingBattleDataSO.SetCurrentIndex(battleStageType);
        var currentData = loadingBattleDataSO.CurrentStageData;
        PencilCaseDataManagerSO.SetEnemyPencilCaseData(currentData);
        aIDataSO.SetAIData(currentData);
        _sceneLoadComponenet.SceneLoadBattle();
    }
    private void LoadBattleDataStageMake(BattleStageType battleStageType)
    {
        Debug.Log($"{battleStageType} is loding...");
        loadingBattleDataSO.SetCurrentIndex(battleStageType);
        var currentData = loadingBattleDataSO.CurrentStageData;
        PencilCaseDataManagerSO.SetEnemyPencilCaseData(currentData);
        aIDataSO.SetAIData(currentData);
        _sceneLoadComponenet.SceneLoadStageMake();
    }
}
