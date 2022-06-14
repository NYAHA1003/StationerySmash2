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
        public PencilCaseUnit PlayerPencilCase => _playerPencilCase;
        public PencilCaseUnit EnemyPencilCase => _enemyPencilCase;
        public AbstractPencilCaseAbility PlayerAbilityState => _playerAbilityState;
        public AbstractPencilCaseAbility EnemyAbilityState => _enemyAbilityState;

        //참조 변수
        private UnitComponent _unitCommand = null;
        private CardComponent _cardComponent = null;
        private StageData _stageData = null;
        private AbstractPencilCaseAbility _playerAbilityState = null;
        private AbstractPencilCaseAbility _enemyAbilityState = null;
        private PencilCaseBadgeComponent _pencilCaseBadgeComponent = null;
        private CameraComponent _cameraComponent = null;

        //인스펙터 참조 변수
        [SerializeField]
        private PencilCaseUnit _playerPencilCase = null;
        [SerializeField]
        private PencilCaseUnit _enemyPencilCase = null;
        [SerializeField]
        private Button _pencilCaseAbilityButton = null;
        [SerializeField]
        private RectTransform _pencilCaseAbilityButtonRect = null;
        [SerializeField]
        private RectTransform _bloodEffectImage = null;
        [SerializeField]
        private Image _disableImage = null;

        //변수
        private Sequence _bloodEffect = null;
        private int _needGauge = 0;
        private int _currentGauge = 0;
        private bool _isActiveButton; //버튼이 켜졌을 때

        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="battleManager"></param>
        /// <param name="pencilCase_My"></param>
        /// <param name="pencilCase_Enemy"></param>
        /// <param name="pencilCaseDataMy"></param>
        /// <param name="pencilCaseDataEnemy"></param>
        public void SetInitialization(CameraComponent cameraComponent, CardComponent cardComponent , UnitComponent unitCommand, StageData stageData)
        {
            _pencilCaseBadgeComponent = new PencilCaseBadgeComponent();

            this._unitCommand = unitCommand;
            this._stageData = stageData;
            this._cardComponent = cardComponent;
            this._cameraComponent = cameraComponent;
            this._needGauge = PencilCaseDataManagerSO.InGamePencilCaseData._needGauge;

            _pencilCaseBadgeComponent.SetInitialization(this);

            //플레이어 필통
            SetPencilCaseUnit(_playerPencilCase, PencilCaseDataManagerSO.InGamePencilCaseData, TeamType.MyTeam, -1);

            //적 필통
            SetPencilCaseUnit(_enemyPencilCase, PencilCaseDataManagerSO.EnemyGamePencilCaseData, TeamType.EnemyTeam, -2);

            _cardComponent.AddDictionary<CardObj>(_cardComponent.SetUseCard, AddGaugeAsCost);

            EventManager.Instance.StartListening(EventsType.PencilCaseAbility, OnPencilCaseAbility);
        }

        /// <summary>
        /// 플레이어 필통능력 사용
        /// </summary>
        public void RunPlayerPencilCaseAbility()
        {
            if(_currentGauge >= _needGauge)
			{
                _currentGauge = 0;
                _playerAbilityState.RunPencilCaseAbility();
                DrawPencilCaseButtonDisable();
            }
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
        /// 코스트 만큼 게이지를 증가시킨다
        /// </summary>
        /// <param name="cardObj"></param>
        private void AddGaugeAsCost(CardObj cardObj)
		{
            if(_currentGauge < _needGauge)
			{
               _currentGauge += cardObj.CardCost;
                DrawPencilCaseButtonDisable();
                if(_currentGauge >= _needGauge)
				{
                    if (!_isActiveButton)
                    {
                        _isActiveButton = true;
                        _pencilCaseAbilityButtonRect.DOShakeAnchorPos(0.3f);
                    }
                }
            }
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
            _cameraComponent.ShakeCamera(0.1f, 0.3f);
        }

        /// <summary>
        /// 클릭하면 필통 능력 사용
        /// </summary>
        private void OnPencilCaseAbility()
        {
            _isActiveButton = false;
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
            _pencilCaseBadgeComponent.SetEnemyBadgeAbility(PencilCaseDataManagerSO.EnemyGamePencilCaseData._badgeDatas, EnemyPencilCase);
        }

        /// <summary>
        /// 플레이어 뱃지 설정
        /// </summary>
        private void SetPlayerBadgeAbility()
        {
            _pencilCaseBadgeComponent.SetPlayerBadgeAbility(PencilCaseDataManagerSO.InGamePencilCaseData._badgeDatas, PlayerPencilCase);
        }

        /// <summary>
        /// 필통 설정
        /// </summary>
        /// <param name="pencilCaseUnit"></param>
        /// <param name="pencilCaseDataSO"></param>
        /// <param name="teamType"></param>
        /// <param name="index"></param>
        private void SetPencilCaseUnit(PencilCaseUnit pencilCaseUnit, PencilCaseData pencilCaseData, TeamType teamType, int index)
        {
            //필통 유닛 설정
            pencilCaseUnit.SetUnitData(pencilCaseData.ReturnCardData(), teamType, _stageData, index, 1, 0);

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
                pencilCaseUnit.transform.position = new Vector2(-_stageData.max_Range + 0.05f, 0);
            }
            else if (teamType == TeamType.EnemyTeam)
            {
                pencilCaseUnit.transform.position = new Vector2(_stageData.max_Range - 0.05f, 0);
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

        /// <summary>
        /// 필통 능력 사용 여부를 그린다
        /// </summary>
        private void DrawPencilCaseButtonDisable()
        {
            _disableImage.fillAmount = (float)(_needGauge - _currentGauge) / _needGauge;
        }

    }
}