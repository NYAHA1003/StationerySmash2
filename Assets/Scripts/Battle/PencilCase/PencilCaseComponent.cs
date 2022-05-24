using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Utill.Data;
using Utill.Tool;
using Battle.Badge;
using Battle.PCAbility;
using Main.Event;
using DG.Tweening;

namespace Battle
{
    [System.Serializable]
    public class PencilCaseComponent : BattleComponent
    {
        //프로퍼티
        public PencilCaseDataSO PencilCaseDataMy => pencilCaseDataMy;
        public PencilCaseDataSO PencilCaseDataEnemy => pencilCaseDataEnemy;
        public PencilCaseUnit PlayerPencilCase => _playerPencilCase;
        public PencilCaseUnit EnemyPencilCase => _enemyPencilCase;
        public AbstractPencilCaseAbility PlayerAbilityState => _playerAbilityState;
        public AbstractPencilCaseAbility EnemyAbilityState => _enemyAbilityState;

        //참조 변수
        private UnitComponent _unitCommand = null;
        private StageData _stageData = null;
        private PencilCaseDataSO pencilCaseDataMy = null;
        private PencilCaseDataSO pencilCaseDataEnemy = null;
        private AbstractPencilCaseAbility _playerAbilityState = null;
        private AbstractPencilCaseAbility _enemyAbilityState = null;
        private PencilCaseBadgeComponent _pencilCaseBadgeComponent = null;

        //인스펙터 참조 변수
        [SerializeField]
        private PencilCaseUnit _playerPencilCase = null;
        [SerializeField]
        private PencilCaseUnit _enemyPencilCase = null;
        [SerializeField]
        private Button _pencilCaseAbilityButton = null;
        [SerializeField]
        private RectTransform _bloodEffectImage = null;

        //변수
        private Sequence _bloodEffect = null;

        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="battleManager"></param>
        /// <param name="pencilCase_My"></param>
        /// <param name="pencilCase_Enemy"></param>
        /// <param name="pencilCaseDataMy"></param>
        /// <param name="pencilCaseDataEnemy"></param>
        public void SetInitialization(UnitComponent unitCommand, StageData stageData)
        {
            _pencilCaseBadgeComponent = new PencilCaseBadgeComponent();

            this._unitCommand = unitCommand;
            this._stageData = stageData;

            pencilCaseDataMy = _playerPencilCase.PencilCaseData;
            pencilCaseDataEnemy = _enemyPencilCase.PencilCaseData;

            _pencilCaseBadgeComponent.SetInitialization(this);

            //플레이어 필통
            SetPencilCaseUnit(_playerPencilCase, pencilCaseDataMy, TeamType.MyTeam, -1);

            //적 필통
            SetPencilCaseUnit(_enemyPencilCase, pencilCaseDataEnemy, TeamType.EnemyTeam, -2);


            EventManager.StartListening(EventsType.PencilCaseAbility, OnPencilCaseAbility);
        }

        /// <summary>
        /// 플레이어 필통능력 사용
        /// </summary>
        public void RunPlayerPencilCaseAbility()
        {
            _playerAbilityState.RunPencilCaseAbility();
        }

        /// <summary>
        /// 적 필통능력 사용
        /// </summary>
        public void RunEnemyPencilCaseAbility()
        {
            _enemyAbilityState.RunPencilCaseAbility();
        }

        /// <summary>
        /// 필통 피격 이펙트 재생
        /// </summary>
        public void PlayBloodEffect(TeamType teamType)
        {

            if (teamType == TeamType.MyTeam)
            {
                SetBloodEffect();
                _bloodEffect.Restart();
            }
		}

        /// <summary>
        /// 뱃지가 있는지 확인한다
        /// </summary>
        /// <returns></returns>
        public bool FindBadge(TeamType teamType, BadgeType badgeType)
		{
            return _pencilCaseBadgeComponent.FindBadge(teamType, badgeType);
		}

        /// <summary>
        /// 피격 이펙트 설정
        /// </summary>
        private void SetBloodEffect()
        {
            _bloodEffect ??= DOTween.Sequence()
                .SetAutoKill(false)
                .OnStart(() =>
                {
                    _bloodEffectImage.sizeDelta = new Vector2(2700, 0);
                })
                .Append(_bloodEffectImage.DOSizeDelta(new Vector2(2700, 1000), 0.2f).SetAutoKill(false))
                .Append(_bloodEffectImage.DOSizeDelta(new Vector2(2700, 0), 0.1f).SetAutoKill(false));
        }

        /// <summary>
        /// 클릭하면 필통 능력 사용
        /// </summary>
        private void OnPencilCaseAbility()
        {
            RunPlayerPencilCaseAbility();
        }

        /// <summary>
        /// 뱃지 능력 사용
        /// </summary>
        /// <param name="badges"></param>
        private void RunBadgeAbility(TeamType teamType)
        {
            _pencilCaseBadgeComponent.RunBadgeAbility(teamType);
        }

        /// <summary>
        /// 적 뱃지 설정
        /// </summary>
        private void SetEnemyBadgeAbility()
        {
            _pencilCaseBadgeComponent.SetEnemyBadgeAbility(pencilCaseDataEnemy._pencilCaseData._badgeDatas, EnemyPencilCase);
        }

        /// <summary>
        /// 플레이어 뱃지 설정
        /// </summary>
        private void SetPlayerBadgeAbility()
        {
            _pencilCaseBadgeComponent.SetPlayerBadgeAbility(pencilCaseDataMy._pencilCaseData._badgeDatas, PlayerPencilCase);
        }

        /// <summary>
        /// 필통 설정
        /// </summary>
        /// <param name="pencilCaseUnit"></param>
        /// <param name="pencilCaseDataSO"></param>
        /// <param name="teamType"></param>
        /// <param name="index"></param>
        private void SetPencilCaseUnit(PencilCaseUnit pencilCaseUnit, PencilCaseDataSO pencilCaseDataSO, TeamType teamType, int index)
        {
            //필통 유닛 설정
            pencilCaseUnit.SetUnitData(pencilCaseDataSO._pencilCaseData._pencilCaseData, teamType, _stageData, index, 1, 0);

            //유닛 리스트에 필통 유닛을 넣는다
            if (teamType == TeamType.MyTeam)
            {
                _unitCommand._playerUnitList.Add(pencilCaseUnit);
            }
            else if (teamType == TeamType.EnemyTeam)
            {
                _unitCommand._enemyUnitList.Add(pencilCaseUnit);
            }

            //필통 위치 설정
            if (teamType == TeamType.MyTeam)
            {
                pencilCaseUnit.transform.position = new Vector2(-_stageData.max_Range, 0);
            }
            else if (teamType == TeamType.EnemyTeam)
            {
                pencilCaseUnit.transform.position = new Vector2(_stageData.max_Range, 0);
            }

            //필통 특수 능력 설정
            if (teamType == TeamType.MyTeam)
            {
                _playerAbilityState = pencilCaseUnit.AbilityState;
            }
            else if (teamType == TeamType.EnemyTeam)
            {
                _enemyAbilityState = pencilCaseUnit.AbilityState;
            }

            //필통 특수 능력의 팀 설정
            if (teamType == TeamType.MyTeam)
            {
                _playerAbilityState.SetTeam(teamType);
            }
            else if (teamType == TeamType.EnemyTeam)
            {
                _enemyAbilityState.SetTeam(teamType);
            }

            //뱃지 능력 설정
            if (teamType == TeamType.MyTeam)
            {
                SetPlayerBadgeAbility();
            }
            else if (teamType == TeamType.EnemyTeam)
            {
                SetEnemyBadgeAbility();
            }

            //시작 뱃지 능력 발동
            RunBadgeAbility(teamType);
        }

    }
}