using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill.Load;
using Main.Deck;

public class SceneLoadButtonManager : MonoBehaviour
{
    [SerializeField]
    private Button[] buttons;
    [SerializeField]
    private LoadingBattleDataSO loadingBattleDataSO;
    [SerializeField]
    private AIDataSO aIDataSO;
    [SerializeField]
    private PencilCaseDataSO pencilCaseDataSO;


    // Start is called before the first frame update
    private void Awake()
    {
        SetBattleLoadButtons();
    }
   
    private void SetBattleLoadButtons()
    {
        for (int i = 0; i < System.Enum.GetValues(typeof(BattleStageType)).Length; i++)
        {
            //각 버튼에 enum값 대입하기
            int temp = i;
            buttons[temp].onClick.AddListener(() => LoadBattleData((BattleStageType)temp));
        }
    }
    private void LoadBattleData(BattleStageType battleStageType)
    {
        //so데이터를 aidataSO와 PencilCaseDataSO에 넣어줌
        loadingBattleDataSO.SetCurrentIndex(loadingBattleDataSO.loadDatas.FindIndex(x => x.battleStageType == battleStageType));
        var currentData = loadingBattleDataSO.CurrentStageData;
        pencilCaseDataSO._pencilCaseData._maxCard = currentData._pencilCaseData._maxCard;
        pencilCaseDataSO._pencilCaseData._costSpeed = currentData._pencilCaseData._costSpeed;
        pencilCaseDataSO._pencilCaseData._throwGaugeSpeed = currentData._pencilCaseData._throwGaugeSpeed;
        pencilCaseDataSO._pencilCaseData._pencilCaseType = currentData._pencilCaseData._pencilCaseType;
        pencilCaseDataSO._pencilCaseData._pencilState = currentData._pencilCaseData._pencilState;
        pencilCaseDataSO._pencilCaseData._badgeDatas = currentData._pencilCaseData._badgeDatas;

        aIDataSO.summonGrade = currentData.summonGrade;
        aIDataSO.throwSpeed = currentData.throwSpeed;
        aIDataSO.isAIOn = currentData.isAIOn;
        aIDataSO.cardDataList = currentData.cardDataList;
        aIDataSO.pos = currentData.pos;
        aIDataSO.max_Delay = currentData.max_Delay;

        pencilCaseDataSO.battleStageType = currentData.battleStageType;
    }
}
