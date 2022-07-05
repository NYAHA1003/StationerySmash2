using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Tool;
using Utill.Data;
using Main.Event;
using Main.Setting;
using System.Linq;

namespace Utill.Load
{
    public class InitLoadButton : MonoBehaviour
    {
		private SceneLoadComponenet _sceneLoadComponenet;
        [SerializeField]
        private StageDataListSO _stageDataListSO = null;
        [SerializeField]
        private StageDetailPopupPanel _popupPanel = null;
        [SerializeField]
        private LoadingBattleDataSO loadingBattleDataSO;

		public void Start()
		{
            EventManager.Instance.StartListening(EventsType.SetNextStageData, NextBattle);
		}

		/// <summary>
		/// 다음 배틀로 이동
		/// </summary>
		public void NextBattle()
        {
            if(UserSaveManagerSO.UserSaveData._lastPlayStage == BattleStageType.S1_1 && AIAndStageData.Instance._currentStageDatas._stageType == BattleStageType.S1_2)
            {
                return;
            }
            var last = System.Enum.GetValues(typeof(BattleStageType)).Cast<BattleStageType>().Last();
            if (AIAndStageData.Instance._currentStageDatas._stageType == last)
			{
                return;
			}
            var next = AIAndStageData.Instance._currentStageDatas._stageType + 1;
            loadingBattleDataSO.SetCurrentIndex(next);
            var currentData = loadingBattleDataSO.CurrentStageData;
            PencilCaseDataManagerSO.SetEnemyPencilCaseData(currentData);
            AIAndStageData.Instance.SetAIData(currentData);
            AIAndStageData.Instance._currentStageDatas = _stageDataListSO.stageDatas.Find(x => x._stageType == next);
        }

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
