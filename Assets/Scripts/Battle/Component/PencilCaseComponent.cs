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
        [SerializeField]
        private Button _pencilCaseAbilityButton = null;
        [SerializeField]
        private RectTransform _bloodEffectImage = null;
        private RectTransform _bloodEffectImageDotween = null;


        //변수
        private List<AbstractBadge> _playerBadges = new List<AbstractBadge>();
        private List<AbstractBadge> _enemyBadges = new List<AbstractBadge>();
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
            this._unitCommand = unitCommand;
            this._stageData = stageData;

            pencilCaseDataMy = _playerPencilCase.PencilCaseData;
            pencilCaseDataEnemy = _enemyPencilCase.PencilCaseData;

            //플레이어 필통
            _playerPencilCase.SetUnitData(pencilCaseDataMy._pencilCaseData._pencilCaseData, TeamType.MyTeam, _stageData, -1, 1, 0);
            _unitCommand._playerUnitList.Add(_playerPencilCase);
            _playerPencilCase.transform.position = new Vector2(-_stageData.max_Range, 0);
            _playerAbilityState = _playerPencilCase.AbilityState;
            _playerAbilityState.SetTeam(TeamType.MyTeam);
            SetPlayerBadgeAbility();
            RunBadgeAbility(_playerBadges);

            //적 필통
            _enemyPencilCase.SetUnitData(pencilCaseDataEnemy._pencilCaseData._pencilCaseData, TeamType.EnemyTeam, _stageData, -2, 1, 0);
            _unitCommand._enemyUnitList.Add(_enemyPencilCase);
            _enemyPencilCase.transform.position = new Vector2(_stageData.max_Range, 0);
            _enemyAbilityState = _enemyPencilCase.AbilityState;
            _enemyAbilityState.SetTeam(TeamType.EnemyTeam);
            SetEnemyBadgeAbility();
            RunBadgeAbility(_enemyBadges);

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
                Debug.Log("필통1");
                SetBloodEffect();
                _bloodEffect.Restart();
            }
		}

        /// <summary>
        /// 피격 이펙트 닷트윈 설정
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
        /// 뱃지 가져오기
        /// </summary>
        /// <param name="badgeType"></param>
        /// <returns></returns>
        private AbstractBadge ReturnBadge(BadgeType badgeType)
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

        /// <summary>
        /// 뱃지 능력 사용
        /// </summary>
        /// <param name="badges"></param>
        private void RunBadgeAbility(List<AbstractBadge> badges)
        {
            for (int i = 0; i < badges.Count; i++)
            {
                badges[i].RunBadgeAbility();
            }
        }

        /// <summary>
        /// 적 뱃지 설정
        /// </summary>
        private void SetEnemyBadgeAbility()
        {
            for (int i = 0; i < pencilCaseDataEnemy._pencilCaseData._badgeDatas.Count; i++)
            {
                BadgeData badgeData = pencilCaseDataEnemy._pencilCaseData._badgeDatas[i];
                AbstractBadge badge = ReturnBadge(badgeData._badgeType);
                if (badge != null)
                {
                    badge.SetBadge(this, EnemyPencilCase, TeamType.MyTeam, badgeData);
                    _enemyBadges.Add(badge);
                }
            }
        }

        /// <summary>
        /// 플레이어 뱃지 설정
        /// </summary>
        private void SetPlayerBadgeAbility()
        {
            for (int i = 0; i < pencilCaseDataMy._pencilCaseData._badgeDatas.Count; i++)
            {
                BadgeData badgeData = pencilCaseDataMy._pencilCaseData._badgeDatas[i];
                AbstractBadge badge = ReturnBadge(badgeData._badgeType);
                if (badge != null)
                {
                    badge.SetBadge(this, PlayerPencilCase, TeamType.MyTeam, badgeData);
                    _playerBadges.Add(badge);
                }
            }
        }
    }

}