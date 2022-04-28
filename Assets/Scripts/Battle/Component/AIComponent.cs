using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;

namespace Battle
{
    [System.Serializable]
    public class AIComponent
    {
        //�ν����� ���� ����
        [SerializeField]
        private AIDataSO _aiEnemyDataSO;
        [SerializeField]
        private AIDataSO _aiPlayerDataSO;
        [SerializeField]
        private StageLog _aiLog;
        [SerializeField]
        private bool isEnemyAIOn = false;
        [SerializeField]
        private bool isPlayerAIOn = false;

        //���� ����
        private UnitComponent _unitCommand = null;
        private PencilCaseComponent _pencilCaseCommand = null;

        //����
        private List<CardData> enemyCardDataList = new List<CardData>();
        private List<Vector2> enemyPos = new List<Vector2>();
        private List<float> enemyMaxDelay = new List<float>();
        private List<CardData> playerCardDataList;
        private List<Vector2> playerPos;
        private List<float> playerMaxDelay;

        //Enemy��
        private int _enemyCurrentIndex = 0;
        private int enemySummonGrade = 1;
        private float enemyCurDelay = 0.0f;
        private float enemyThrowSpeed = 0.0f;
        private float enemyThrowCurDelay = 0.0f;
        private float enemyThrowMaxDelay = 100.0f;

        //Player��
        private int playerCurrent;
        private int playerSummonGrade = 1;
        private float playerCurDelay = 0.0f;
        private float playerThrowSpeed = 0.0f;
        private float playerThrowCurDelay = 0.0f;
        private float playerThrowMaxDelay = 100;

        /// <summary>
        /// �ʱ�ȭ
        /// </summary>
        /// <param name="battleManager"></param>
        /// <param name="aIenemyDataSO"></param>
        /// <param name="aIplayerDataSO"></param>
        /// <param name="isEnemyAIOn"></param>
        /// <param name="isPlayerAIOn"></param>
        public void SetInitialization(PencilCaseComponent pencilCaseCommand, UnitComponent unitCommand, ref System.Action updateAction)
        {
            this._unitCommand = unitCommand;
            this._pencilCaseCommand = pencilCaseCommand;
            
            //�� AI
            this.enemySummonGrade = _aiEnemyDataSO.summonGrade;
            this.enemyCardDataList = _aiEnemyDataSO.cardDataList;
            this.enemyPos = _aiEnemyDataSO.pos;
            this.enemyMaxDelay = _aiEnemyDataSO.max_Delay;
            this.enemyThrowSpeed = _aiEnemyDataSO.throwSpeed;

            //�׽�Ʈ�� Player AI
            this.playerSummonGrade = _aiPlayerDataSO.summonGrade;
            this.playerCardDataList = _aiPlayerDataSO.cardDataList;
            this.playerPos = _aiPlayerDataSO.pos;
            this.playerMaxDelay = _aiPlayerDataSO.max_Delay;
            this.playerThrowSpeed = _aiPlayerDataSO.throwSpeed;

            //��Ʋ�Ŵ����� ������Ʈ�� �Լ��� �ִ´�
            updateAction += UpdateEnemyAICard;
            updateAction += UpdateEnemyAIThrow;
            updateAction += UpdatePlayerAICard;
            updateAction += UpdatePlayerAIThrow;
            updateAction += UpdateRunEnemyPencilcaseAbility;
        }

        /// <summary>
        /// �� ���� �ɷ� ���
        /// </summary>
        public void UpdateRunEnemyPencilcaseAbility()
        {
            if(_pencilCaseCommand.EnemyAbilityState.AIAbilityCondition())
            {
                _pencilCaseCommand.RunEnemyPencilCaseAbility();
            }
        }


        /// <summary>
        /// �� AI�� ������
        /// </summary>
        public void UpdateEnemyAIThrow()
        {
            if (!isEnemyAIOn)
            {
                return;
            }
            if (_unitCommand._enemyUnitList.Count < 3)
            {
                return;
            }
            if (enemyThrowCurDelay < enemyThrowMaxDelay)
            {
                enemyThrowCurDelay += enemyThrowSpeed * Time.deltaTime;
                return;
            }
            int selectUnit = Random.Range(2, _unitCommand._enemyUnitList.Count - 1);
            Vector2 pos = _unitCommand._enemyUnitList[selectUnit].transform.position;
            pos.x += Random.Range(2.0f, 4.0f);
            pos.y -= Random.Range(2.0f, 4.0f);
            _unitCommand._enemyUnitList[selectUnit].Throw_Unit(pos);
            enemyThrowCurDelay = 0;
        }

        /// <summary>
        /// �� AI�� ���� ��ȯ
        /// </summary>
        public void UpdateEnemyAICard()
        {
            if (!isEnemyAIOn)
                return;
            if (enemyCurDelay < enemyMaxDelay[_enemyCurrentIndex])
            {
                enemyCurDelay += Time.deltaTime;
                return;
            }
            _unitCommand.SummonUnit(enemyCardDataList[_enemyCurrentIndex], new Vector3(enemyPos[_enemyCurrentIndex].x, 0, 0), enemySummonGrade, TeamType.EnemyTeam);
            _enemyCurrentIndex++;
            if (_enemyCurrentIndex == enemyMaxDelay.Count)
            {
                _enemyCurrentIndex = 0;
            }
            enemyCurDelay = 0;
        }

        /// <summary>
        /// �÷��̾� AI�� ���� ������
        /// </summary>
        public void UpdatePlayerAIThrow()
        {
            if (!isPlayerAIOn)
                return;
            if (_unitCommand._playerUnitList.Count < 3)
                return;
            if (playerThrowCurDelay < playerThrowMaxDelay)
            {
                playerThrowCurDelay += playerThrowSpeed * Time.deltaTime;
                return;
            }
            int selectUnit = Random.Range(2, _unitCommand._playerUnitList.Count - 1);
            Vector2 pos = _unitCommand._playerUnitList[selectUnit].transform.position;
            pos.x -= Random.Range(2.0f, 4.0f);
            pos.y -= Random.Range(2.0f, 4.0f);
            _unitCommand._playerUnitList[selectUnit].Throw_Unit(pos);
            playerThrowCurDelay = 0;
        }

        /// <summary>
        /// �÷��̾� AI�� ���� ��ȯ
        /// </summary>
        public void UpdatePlayerAICard()
        {
            if (!isPlayerAIOn)
                return;
            if (playerCurDelay < playerMaxDelay[playerCurrent])
            {
                playerCurDelay += Time.deltaTime;
                return;
            }
            _unitCommand.SummonUnit(playerCardDataList[playerCurrent], new Vector3(playerPos[playerCurrent].x, 0, 0), playerSummonGrade, TeamType.MyTeam);
            playerCurrent++;
            if (playerCurrent == playerMaxDelay.Count)
            {
                playerCurrent = 0;
            }
            playerCurDelay = 0;
        }
    }

}