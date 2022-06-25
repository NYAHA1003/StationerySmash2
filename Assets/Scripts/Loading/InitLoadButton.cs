using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Tool;
namespace Utill.Load
{
    public class InitLoadButton : MonoBehaviour
    {
        [SerializeField]
        private SceneLoadComponenet _sceneLoadComponenet;
        [SerializeField]
        private StageDataListSO _stageDataListSO = null;
        [SerializeField]
        private StageDetailPopupPanel _popupPanel = null;
        [SerializeField]
        private LoadingBattleDataSO loadingBattleDataSO;
        public void InitBattleData(BattleStageType battleStageType)
        {
            loadingBattleDataSO.SetCurrentIndex(battleStageType);
            var currentData = loadingBattleDataSO.CurrentStageData;
            PencilCaseDataManagerSO.SetEnemyPencilCaseData(currentData);
            AIAndStageData.Instance.SetAIData(currentData);
            AIAndStageData.Instance._currentStageDatas = _stageDataListSO.stageDatas.Find(x => x._stageType == battleStageType);
            _popupPanel.Setting();

        }
        public void LoadBattleDataStageMake(BattleStageType battleStageType)
        {
            loadingBattleDataSO.SetCurrentIndex(battleStageType);
            var currentData = loadingBattleDataSO.CurrentStageData;
            PencilCaseDataManagerSO.SetEnemyPencilCaseData(currentData);
            AIAndStageData.Instance.SetAIData(currentData);
            _sceneLoadComponenet.SceneLoadStageMake();
        }
    }
}
