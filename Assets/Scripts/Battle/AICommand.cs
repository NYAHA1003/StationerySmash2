using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class AICommand : BattleCommand
    {
        public List<DataBase> enemyCardDataList = new List<DataBase>();
        public List<Vector2> enemyPos = new List<Vector2>();
        public List<float> enemyMaxDelay = new List<float>();
        public List<DataBase> playerCardDataList;
        public List<Vector2> playerPos;
        public List<float> playerMaxDelay;

        //Enemy��
        private int _enemyCurrentIndex = 0;
        private int enemySummonGrade = 1;
        private float enemyCurDelay = 0.0f;
        private float enemyThrowSpeed = 0.0f;
        private float enemyThrowCurDelay = 0.0f;
        private float enemyThrowMaxDelay = 100.0f;
        private bool isEnemyAIOn = false;

        //Player��
        private int playerCurrent;
        private int playerSummonGrade = 1;
        private float playerCurDelay = 0.0f;
        private float playerThrowSpeed = 0.0f;
        private float playerThrowCurDelay = 0.0f;
        private float playerThrowMaxDelay = 100;
        private bool isPlayerAIOn = false;

        /// <summary>
        /// �ʱ�ȭ
        /// </summary>
        /// <param name="battleManager"></param>
        /// <param name="aIenemyDataSO"></param>
        /// <param name="aIplayerDataSO"></param>
        /// <param name="isEnemyAIOn"></param>
        /// <param name="isPlayerAIOn"></param>
        public void SetInitialization(BattleManager battleManager, AIDataSO aIenemyDataSO, AIDataSO aIplayerDataSO, bool isEnemyAIOn, bool isPlayerAIOn)
        {
            SetBattleManager(battleManager);

            //�� AI
            this.enemySummonGrade = aIenemyDataSO.summonGrade;
            this.enemyCardDataList = aIenemyDataSO.cardDataList;
            this.enemyPos = aIenemyDataSO.pos;
            this.enemyMaxDelay = aIenemyDataSO.max_Delay;
            this.enemyThrowSpeed = aIenemyDataSO.throwSpeed;
            this.isEnemyAIOn = isEnemyAIOn;

            //�׽�Ʈ�� Player AI
            this.playerSummonGrade = aIplayerDataSO.summonGrade;
            this.playerCardDataList = aIplayerDataSO.cardDataList;
            this.playerPos = aIplayerDataSO.pos;
            this.playerMaxDelay = aIplayerDataSO.max_Delay;
            this.playerThrowSpeed = aIplayerDataSO.throwSpeed;
            this.isPlayerAIOn = isPlayerAIOn;

            //��Ʋ�Ŵ����� ������Ʈ�� �Լ��� �ִ´�
            battleManager.AddUpdateAction(UpdateEnemyAICard);
            battleManager.AddUpdateAction(UpdateEnemyAIThrow);
            battleManager.AddUpdateAction(UpdatePlayerAICard);
            battleManager.AddUpdateAction(UpdatePlayerAIThrow);
        }

        /// <summary>
        /// �� AI�� ������
        /// </summary>
        public void UpdateEnemyAIThrow()
        {
            if (!isEnemyAIOn)
                return;
            if (battleManager._enemyUnitDatasTemp.Count < 3)
                return;
            if (enemyThrowCurDelay < enemyThrowMaxDelay)
            {
                enemyThrowCurDelay += enemyThrowSpeed * Time.deltaTime;
                return;
            }
            int selectUnit = Random.Range(2, battleManager._enemyUnitDatasTemp.Count - 1);
            Vector2 pos = battleManager._enemyUnitDatasTemp[selectUnit].transform.position;
            pos.x += Random.Range(2.0f, 4.0f);
            pos.y -= Random.Range(2.0f, 4.0f);
            battleManager._enemyUnitDatasTemp[selectUnit].Throw_Unit(pos);
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
            battleManager.CommandUnit.SummonUnit(enemyCardDataList[_enemyCurrentIndex], new Vector3(enemyPos[_enemyCurrentIndex].x, 0, 0), enemySummonGrade, Utill.TeamType.EnemyTeam);
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
            if (battleManager._myUnitDatasTemp.Count < 3)
                return;
            if (playerThrowCurDelay < playerThrowMaxDelay)
            {
                playerThrowCurDelay += playerThrowSpeed * Time.deltaTime;
                return;
            }
            int selectUnit = Random.Range(2, battleManager._myUnitDatasTemp.Count - 1);
            Vector2 pos = battleManager._myUnitDatasTemp[selectUnit].transform.position;
            pos.x -= Random.Range(2.0f, 4.0f);
            pos.y -= Random.Range(2.0f, 4.0f);
            battleManager._myUnitDatasTemp[selectUnit].Throw_Unit(pos);
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
            battleManager.CommandUnit.SummonUnit(playerCardDataList[playerCurrent], new Vector3(playerPos[playerCurrent].x, 0, 0), playerSummonGrade, Utill.TeamType.MyTeam);
            playerCurrent++;
            if (playerCurrent == playerMaxDelay.Count)
            {
                playerCurrent = 0;
            }
            playerCurDelay = 0;
        }
    }

}