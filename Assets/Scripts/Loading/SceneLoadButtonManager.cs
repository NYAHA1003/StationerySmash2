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
    private AIDataSO aIDataSO;


    // Start is called before the first frame update
    private void Awake()
    {
        SetBattleLoadButtons();
    }
   
    private void SetBattleLoadButtons()
    {
        for (int i = 0; i < System.Enum.GetValues(typeof(BattleStageType)).Length; i++)
        {
            //�� ��ư�� enum�� �����ϱ�
            int temp = i;
            buttons[temp].onClick.AddListener(() => LoadBattleData((BattleStageType)temp));
        }
    }
    private void LoadBattleData(BattleStageType battleStageType)
    {
        //so�����͸� aidataSO�� PencilCaseDataSO�� �־���
        loadingBattleDataSO.SetCurrentIndex(loadingBattleDataSO.loadDatas.FindIndex(x => x.battleStageType == battleStageType));
        var currentData = loadingBattleDataSO.CurrentStageData;
        PencilCaseDataManagerSO.SetEnemyPencilCaseData(currentData);

        aIDataSO.summonGrade = currentData.summonGrade;
        aIDataSO.throwSpeed = currentData.throwSpeed;
        aIDataSO.isAIOn = currentData.isAIOn;
        aIDataSO.cardDataList = currentData.cardDataList;
        aIDataSO.pos = currentData.pos;
        aIDataSO.max_Delay = currentData.max_Delay;

        //pencilCaseDataSO.battleStageType = currentData.battleStageType;
    }
}
