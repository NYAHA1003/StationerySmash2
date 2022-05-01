using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;
using Battle.Badge;
using Battle.PCAbility;

namespace Battle
{
    [System.Serializable]
    public class PencilCaseComponent : BattleComponent
    {
        public PencilCaseDataSO PencilCaseDataMy => pencilCaseDataMy;
        public PencilCaseDataSO PencilCaseDataEnemy => pencilCaseDataEnemy;
        public PencilCaseUnit PlayerPencilCase => _playerPencilCase;
        public PencilCaseUnit EnemyPencilCase => _enemyPencilCase;
        public AbstractPencilCaseAbility PlayerAbilityState => _playerAbilityState;
        public AbstractPencilCaseAbility EnemyAbilityState => _enemyAbilityState;
        public List<AbstractBadge> PlayerBadges => _playerBadges;


        //참조 변수
        private UnitComponent _unitCommand = null;
        private StageData _stageData = null;
        private PencilCaseDataSO pencilCaseDataMy = null;
        private PencilCaseDataSO pencilCaseDataEnemy = null;
        private AbstractPencilCaseAbility _playerAbilityState = null;
        private AbstractPencilCaseAbility _enemyAbilityState = null;

        //인스펙터 참조 변수
        [SerializeField]
        private PencilCaseUnit _playerPencilCase = null;
        [SerializeField]
        private PencilCaseUnit _enemyPencilCase = null;

        //변수
        private List<AbstractBadge> _playerBadges = new List<AbstractBadge>();
        private List<AbstractBadge> _enemyBadges = new List<AbstractBadge>();

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
            this._unitCommand = unitCommand;
            this._stageData = stageData;

            pencilCaseDataMy = _playerPencilCase.PencilCaseData;
            pencilCaseDataEnemy = _enemyPencilCase.PencilCaseData;

            //플레이어 필통
            _playerPencilCase.SetUnitData(pencilCaseDataMy.PencilCasedataBase.pencilCaseData, TeamType.MyTeam, _stageData, -1, 1, 0);
            _unitCommand._playerUnitList.Add(_playerPencilCase);
            _playerPencilCase.transform.position = new Vector2(-_stageData.max_Range, 0);
            _playerAbilityState = _playerPencilCase.AbilityState;
            _playerAbilityState.SetTeam(TeamType.MyTeam);
            SetPlayerBadgeAbility();
            RunBadgeAbility(_playerBadges);

            //적 필통
            _enemyPencilCase.SetUnitData(pencilCaseDataEnemy.PencilCasedataBase.pencilCaseData, TeamType.EnemyTeam, _stageData, -2, 1, 0);
            _unitCommand._enemyUnitList.Add(_enemyPencilCase);
            _enemyPencilCase.transform.position = new Vector2(_stageData.max_Range, 0);
            _enemyAbilityState = _enemyPencilCase.AbilityState;
            _enemyAbilityState.SetTeam(TeamType.EnemyTeam);
            SetEnemyBadgeAbility();
            RunBadgeAbility(_enemyBadges);
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
        /// 플레이어 뱃지 설정
        /// </summary>
        public void SetPlayerBadgeAbility()
        {
            for(int i = 0; i < pencilCaseDataMy.PencilCasedataBase._badgeDatas.Count; i++)
            {
                BadgeData badgeData = pencilCaseDataMy.PencilCasedataBase._badgeDatas[i];
                AbstractBadge badge = ReturnBadge(badgeData._badgeType);
                if(badge != null)
                {
                    badge.SetBadge(this, PlayerPencilCase, TeamType.MyTeam, badgeData);
                    _playerBadges.Add(badge);
                }
            }
        }


        /// <summary>
        /// 적 뱃지 설정
        /// </summary>
        public void SetEnemyBadgeAbility()
        {
            for (int i = 0; i < pencilCaseDataEnemy.PencilCasedataBase._badgeDatas.Count; i++)
            {
                BadgeData badgeData = pencilCaseDataEnemy.PencilCasedataBase._badgeDatas[i];
                AbstractBadge badge = ReturnBadge(badgeData._badgeType);
                if (badge != null)
                {
                    badge.SetBadge(this, EnemyPencilCase, TeamType.MyTeam, badgeData);
                    _enemyBadges.Add(badge);
                }
            }
        }

        /// <summary>
        /// 뱃지 능력 사용
        /// </summary>
        /// <param name="badges"></param>
        public void RunBadgeAbility(List<AbstractBadge> badges)
        {
            for(int i = 0; i < badges.Count; i++)
            {
                badges[i].RunBadgeAbility();
            }
        }

        /// <summary>
        /// 뱃지 가져오기
        /// </summary>
        /// <param name="badgeType"></param>
        /// <returns></returns>
        public AbstractBadge ReturnBadge(BadgeType badgeType)
        {
            AbstractBadge abstractBadge = null;
            switch (badgeType)
            {
                case BadgeType.None:
                    break;
                case BadgeType.Health:
                    abstractBadge = PoolManager.GetBadge<HealthBadge>();
                    break;
                case BadgeType.Discount:
                    abstractBadge = PoolManager.GetBadge<DiscountBadge>();
                    break;
                case BadgeType.Increase:
                    abstractBadge = PoolManager.GetBadge<IncreaseBadge>();
                    break;
                case BadgeType.TimeUp:
                    abstractBadge = PoolManager.GetBadge<TimeUpBadge>();
                    break;
                case BadgeType.TimeDown:
                    abstractBadge = PoolManager.GetBadge<TimeDownBadge>();
                    break;
                case BadgeType.Blanket:
                    abstractBadge = PoolManager.GetBadge<BlanketBadge>();
                    break;
                case BadgeType.Thorn:
                    abstractBadge = PoolManager.GetBadge<ThornBadge>();
                    break;
                case BadgeType.GrowingSeed:
                    abstractBadge = PoolManager.GetBadge<GrowingSeedBadge>();
                    break;
                case BadgeType.Invincibel:
                    abstractBadge = PoolManager.GetBadge<InvincibleBadge>();
                    break;
                case BadgeType.Snack:
                    abstractBadge = PoolManager.GetBadge<SnackBadge>();
                    break;
            }
            return abstractBadge;
        }

    }

}