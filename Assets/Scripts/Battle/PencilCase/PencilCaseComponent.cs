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
        //������Ƽ
        public PencilCaseUnit PlayerPencilCase => _playerPencilCase;
        public PencilCaseUnit EnemyPencilCase => _enemyPencilCase;
        public AbstractPencilCaseAbility PlayerAbilityState => _playerAbilityState;
        public AbstractPencilCaseAbility EnemyAbilityState => _enemyAbilityState;

        //���� ����
        private UnitComponent _unitCommand = null;
        private CardComponent _cardComponent = null;
        private StageData _stageData = null;
        private AbstractPencilCaseAbility _playerAbilityState = null;
        private AbstractPencilCaseAbility _enemyAbilityState = null;
        private PencilCaseBadgeComponent _pencilCaseBadgeComponent = null;
        private CameraComponent _cameraComponent = null;

        //�ν����� ���� ����
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

        //����
        private Sequence _bloodEffect = null;
        private int _needGauge = 0;
        private int _currentGauge = 0;
        private bool _isActiveButton; //��ư�� ������ ��

        /// <summary>
        /// �ʱ�ȭ
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

            //�÷��̾� ����
            SetPencilCaseUnit(_playerPencilCase, PencilCaseDataManagerSO.InGamePencilCaseData, TeamType.MyTeam, -1);

            //�� ����
            SetPencilCaseUnit(_enemyPencilCase, PencilCaseDataManagerSO.EnemyGamePencilCaseData, TeamType.EnemyTeam, -2);

            _cardComponent.AddDictionary<CardObj>(_cardComponent.SetUseCard, AddGaugeAsCost);

            EventManager.Instance.StartListening(EventsType.PencilCaseAbility, OnPencilCaseAbility);
        }

        /// <summary>
        /// �÷��̾� ����ɷ� ���
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
        /// �� ����ɷ� ���
        /// </summary>
        public void RunEnemyPencilCaseAbility()
        {
            _enemyAbilityState.RunPencilCaseAbility();
        }

        /// <summary>
        /// ���� �ǰ� ����Ʈ ���
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
        /// ������ �ִ��� Ȯ���Ѵ�
        /// </summary>
        /// <returns></returns>
        public bool FindBadge(TeamType teamType, BadgeType badgeType)
		{
            return _pencilCaseBadgeComponent.FindBadge(teamType, badgeType);
		}

        /// <summary>
        /// �ڽ�Ʈ ��ŭ �������� ������Ų��
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
        /// �ǰ� ����Ʈ ����
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
        /// Ŭ���ϸ� ���� �ɷ� ���
        /// </summary>
        private void OnPencilCaseAbility()
        {
            _isActiveButton = false;
            RunPlayerPencilCaseAbility();
        }

        /// <summary>
        /// ���� �ɷ� ���
        /// </summary>
        /// <param name="badges"></param>
        private void RunBadgeAbility(TeamType teamType)
        {
            _pencilCaseBadgeComponent.RunBadgeAbility(teamType);
        }

        /// <summary>
        /// �� ���� ����
        /// </summary>
        private void SetEnemyBadgeAbility()
        {
            _pencilCaseBadgeComponent.SetEnemyBadgeAbility(PencilCaseDataManagerSO.EnemyGamePencilCaseData._badgeDatas, EnemyPencilCase);
        }

        /// <summary>
        /// �÷��̾� ���� ����
        /// </summary>
        private void SetPlayerBadgeAbility()
        {
            _pencilCaseBadgeComponent.SetPlayerBadgeAbility(PencilCaseDataManagerSO.InGamePencilCaseData._badgeDatas, PlayerPencilCase);
        }

        /// <summary>
        /// ���� ����
        /// </summary>
        /// <param name="pencilCaseUnit"></param>
        /// <param name="pencilCaseDataSO"></param>
        /// <param name="teamType"></param>
        /// <param name="index"></param>
        private void SetPencilCaseUnit(PencilCaseUnit pencilCaseUnit, PencilCaseData pencilCaseData, TeamType teamType, int index)
        {
            //���� ���� ����
            pencilCaseUnit.SetUnitData(pencilCaseData.ReturnCardData(), teamType, _stageData, index, 1, 0);

            //���� ����Ʈ�� ���� ������ �ִ´�
            if (teamType == TeamType.MyTeam)
            {
                _unitCommand._playerUnitList.Add(pencilCaseUnit);
            }
            else if (teamType == TeamType.EnemyTeam)
            {
                _unitCommand._enemyUnitList.Add(pencilCaseUnit);
            }

            //���� ��ġ ����
            if (teamType == TeamType.MyTeam)
            {
                pencilCaseUnit.transform.position = new Vector2(-_stageData.max_Range + 0.05f, 0);
            }
            else if (teamType == TeamType.EnemyTeam)
            {
                pencilCaseUnit.transform.position = new Vector2(_stageData.max_Range - 0.05f, 0);
            }

            //���� Ư�� �ɷ� ����
            if (teamType == TeamType.MyTeam)
            {
                _playerAbilityState = pencilCaseUnit.AbilityState;
            }
            else if (teamType == TeamType.EnemyTeam)
            {
                _enemyAbilityState = pencilCaseUnit.AbilityState;
            }

            //���� Ư�� �ɷ��� �� ����
            if (teamType == TeamType.MyTeam)
            {
                _playerAbilityState.SetTeam(teamType);
            }
            else if (teamType == TeamType.EnemyTeam)
            {
                _enemyAbilityState.SetTeam(teamType);
            }

            //���� �ɷ� ����
            if (teamType == TeamType.MyTeam)
            {
                SetPlayerBadgeAbility();
            }
            else if (teamType == TeamType.EnemyTeam)
            {
                SetEnemyBadgeAbility();
            }

            //���� ���� �ɷ� �ߵ�
            RunBadgeAbility(teamType);
        }

        /// <summary>
        /// ���� �ɷ� ��� ���θ� �׸���
        /// </summary>
        private void DrawPencilCaseButtonDisable()
        {
            _disableImage.fillAmount = (float)(_needGauge - _currentGauge) / _needGauge;
        }

    }
}