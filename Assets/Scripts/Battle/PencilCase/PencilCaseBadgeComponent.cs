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
	public class PencilCaseBadgeComponent : BattleComponent
	{
        //참조 변수
        private PencilCaseComponent _pencilCaseComponent = null;

        private List<AbstractBadge> _playerBadges = new List<AbstractBadge>();
        private List<AbstractBadge> _enemyBadges = new List<AbstractBadge>();
        
        /// <summary>
        /// 초기화
        /// </summary>
        public void SetInitialization(PencilCaseComponent pencilCaseComponent)
        {
            _pencilCaseComponent = pencilCaseComponent;
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


        /// <summary>
        /// 뱃지 능력 사용
        /// </summary>
        /// <param name="badges"></param>
        public void RunBadgeAbility(TeamType teamType)
        {
            if(teamType == TeamType.MyTeam)
            {
                for (int i = 0; i < _playerBadges.Count; i++)
                {
                    _playerBadges[i].RunBadgeAbility();
                }
            }
            else if(teamType == TeamType.EnemyTeam)
            {
                for (int i = 0; i < _enemyBadges.Count; i++)
                {
                    _enemyBadges[i].RunBadgeAbility();
                }
            }
        }


        /// <summary>
        /// 플레이어 뱃지 설정
        /// </summary>
        public void SetPlayerBadgeAbility(List<BadgeData> badgeDatas, PencilCaseUnit playerPencilCase)
        {
            for (int i = 0; i < badgeDatas.Count; i++)
            {
                BadgeData badgeData = badgeDatas[i];
                AbstractBadge badge = ReturnBadge(badgeData._badgeType);
                if (badge != null)
                {
                    badge.SetBadge(_pencilCaseComponent, playerPencilCase, TeamType.MyTeam, badgeData);
                    _playerBadges.Add(badge);
                }
            }
        }


        /// <summary>
        /// 적 뱃지 설정
        /// </summary>
        public void SetEnemyBadgeAbility(List<BadgeData> badgeDatas, PencilCaseUnit enemyPencilCase)
        {
            for (int i = 0; i < badgeDatas.Count; i++)
            {
                BadgeData badgeData = badgeDatas[i];
                AbstractBadge badge = ReturnBadge(badgeData._badgeType);
                if (badge != null)
                {
                    badge.SetBadge(_pencilCaseComponent, enemyPencilCase, TeamType.MyTeam, badgeData);
                    _enemyBadges.Add(badge);
                }
            }
        }

        /// <summary>
        /// 플레이어 필통 뱃지를 찾아 있는지 확인한다
        /// </summary>
        /// <param name="badgeType"></param>
        /// <returns></returns>
        public bool FindBadge(TeamType teamType, BadgeType badgeType)
		{
            if(teamType == TeamType.MyTeam)
            {
                if (_playerBadges.Find(x => x._bageType == badgeType) != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if(teamType == TeamType.EnemyTeam)
            {
                if (_playerBadges.Find(x => x._bageType == badgeType) != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
		}
    }

}