using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;


namespace Battle
{
    [System.Serializable]
    public class UnitComponent : BattleComponent
    {
        public Transform UnitParent => _unitParent;

        //�ν����� ���� ����
        [SerializeField]
        private GameObject _unitPrefeb;
        [SerializeField]
        private Transform _unitPoolManager;
        [SerializeField]
        private Transform _unitParent;
        [SerializeField]
        private PoolManager _poolManager;

        //���� ����
        public List<Unit> _playerUnitList { get; private set; } = new List<Unit>();
        public List<Unit> _enemyUnitList { get; private set; } = new List<Unit>();
        private StageData _stageData;

        //�׽�Ʈ�� �� ����
        public TeamType eTeam = TeamType.MyTeam;
        private int unitIdCount = 0;

        /// <summary>
        /// �ʱ�ȭ
        /// </summary>
        /// <param name="battleManager"></param>
        /// <param name="_unitPrefeb"></param>
        /// <param name="_unitPoolManager"></param>
        /// <param name="_unitParent"></param>
        public void SetInitialization(ref System.Action updateAction, StageData stageData)
        {
            _stageData = stageData;
            //updateAction += SortAllUnitList;
        }

        /// <summary>
        /// ���� ��ȯ
        /// </summary>
        /// <param name="dataBase"></param>
        /// <param name="Pos"></param>
        /// <param name="count"></param>
        public void SummonUnit(CardData dataBase, Vector3 Pos, int grade, TeamType eTeam = TeamType.Null)
        {
            Unit unit = null;
            int orderIndex = 0;
            unit = ReturnPoolUnit(Pos);
            if (eTeam == TeamType.Null)
            {
                eTeam = this.eTeam;
            }

            if(eTeam == TeamType.MyTeam)
            {
                orderIndex = _playerUnitList.Count - 1;
            }

            if (eTeam == TeamType.EnemyTeam)
            {
                orderIndex = _enemyUnitList.Count - 1;
            }

            unit.SetUnitData(dataBase, eTeam, _stageData, unitIdCount++, grade, orderIndex);


            //���� ����Ʈ�� �߰�
            switch (eTeam)
            {
                case TeamType.Null:
                    break;
                case TeamType.MyTeam:
                    _playerUnitList.Add(unit);
                    SortPlayerUnitList();
                    break;
                case TeamType.EnemyTeam:
                    _enemyUnitList.Add(unit);
                    SortEnemyUnitList();
                    break;
            }
        }

        /// <summary>
        /// �÷��̾� ���� ����
        /// </summary>
        public void SortPlayerUnitList()
        {
            //����Ʈ ����
            _playerUnitList = _playerUnitList.OrderBy(x => x.transform.position.x).ToList();
            
            //���� ����
            int count = _playerUnitList.Count;
            for (int i = 0; i < count; i++)
            {
                _playerUnitList[i].SetOrderIndex(i);
            }
        }

        /// <summary>
        /// �� ���� ����
        /// </summary>
        public void SortEnemyUnitList()
        {
            //����Ʈ ����
            _enemyUnitList = _enemyUnitList.OrderBy(x => -x.transform.position.x).ToList();

            //���� ����
            int count = _enemyUnitList.Count;
            for (int i = 0; i < count; i++)
            {
                _enemyUnitList[i].SetOrderIndex(i);
            }
        }


        /// <summary>
        /// ��� ���� ����
        /// </summary>
        public void ClearUnit()
        {
            for (int i = 1; _playerUnitList.Count > 1;)
            {
                _playerUnitList[i].Delete_Unit();
            }
            for (int i = 1; _enemyUnitList.Count > 1;)
            {
                _enemyUnitList[i].Delete_Unit();
            }
        }

        /// <summary>
        /// ���� ����
        /// </summary>
        /// <param name="unit"></param>
        public void DeletePoolUnit(Unit unit)
        {
            unit.gameObject.SetActive(false);
            unit.transform.SetParent(_unitPoolManager);
        }

        /// <summary>
        /// ���� ���� Ǯ��
        /// </summary>
        /// <param name="Pos"></param>
        /// <returns></returns>
        private Unit ReturnPoolUnit(Vector3 Pos)
        {
            Unit unit_obj = null;
            if (_unitPoolManager.childCount > 0)
            {
                unit_obj = _unitPoolManager.GetChild(0).gameObject.GetComponent<Unit>();
                unit_obj.transform.position = Pos;
                unit_obj.gameObject.SetActive(true);
            }
            unit_obj ??= PoolManager.CreateUnit(_unitPrefeb, Pos, Quaternion.identity);
            unit_obj.transform.SetParent(_unitParent);
            return unit_obj;
        }
    }
}