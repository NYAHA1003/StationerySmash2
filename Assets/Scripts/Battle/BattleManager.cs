using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Utill.Data;
using Utill.Tool;
using TMPro;
using Battle;
using System;

namespace Battle
{

	public class BattleManager : MonoBehaviour
	{
		//������Ƽ��
		public StageData CurrentStageData
		{
			get
			{
				return _stageDataSO.stageDatas[0];
			}

			private set
			{
			}
		}

		public CardComponent CardComponent => _cardComponent;
		public UnitComponent UnitComponent => _unitComponent;
		public CameraComponent CameraComponent => _cameraComponent;
		public EffectComponent EffectComponent => _effectComponent;
		public ThrowComponent ThrowComponent => _throwComponent;
		public UnitSignComponent UnitSignComponent => _unitSignComponent;
		public TimeComponent TimeComponent => _timeComponent;
		public AIComponent AIComponent => _aiComponent;
		public CostComponent CostComponent => _costComponent;
		public PencilCaseComponent PencilCaseComponent => _pencilCaseComponent;
		public PauseComponent PauseComponent => _pauseComponent;
		public WinLoseComponent WinLoseComponent => _winLoseComponent;

		public TextMeshProUGUI _unitTeamText = null;

		//����
		private bool _isEndSetting = false;
		private Action _updateAction = () => { };

		//�ν����� ����
		[SerializeField]
		private StageDataSO _stageDataSO = null;

		//������Ʈ��
		[SerializeField, Header("ī��ý��� BattleCard"), Space(30)]
		private CardComponent _cardComponent = null;
		[SerializeField, Header("���ֽý��� BattleUnit"), Space(30)]
		private UnitComponent _unitComponent = null;
		[SerializeField, Header("ī�޶�ý��� BattleCamera"), Space(30)]
		private CameraComponent _cameraComponent = null;
		[SerializeField, Header("����Ʈ �ý��� BattleEffect"), Space(30)]
		private EffectComponent _effectComponent = null;
		[SerializeField, Header("������ �ý��� BattleThrow"), Space(30)]
		private ThrowComponent _throwComponent = null;
		[SerializeField, Header("������� ���� ǥ�ýý��� BattleUnitSign"), Space(30)]
		private UnitSignComponent _unitSignComponent = null;
		[SerializeField, Header("�ð��ý��� BattleTime"), Space(30)]
		private TimeComponent _timeComponent = null;
		[SerializeField, Header("AI �ý��� BattleAi"), Space(30)]
		private AIComponent _aiComponent = null;
		[SerializeField, Header("�ڽ�Ʈ �ý��� BattleCost"), Space(30)]
		private CostComponent _costComponent = null;
		[SerializeField, Header("����ý��� BattlePencilCase"), Space(30)]
		private PencilCaseComponent _pencilCaseComponent = null;
		[SerializeField, Header("�Ͻ������ý��� Battle_Pause"), Space(30)]
		private PauseComponent _pauseComponent = null;
		[SerializeField, Header("�¸��й�ý��� BattleWinLose"), Space(30)]
		private WinLoseComponent _winLoseComponent = null;
		[SerializeField, Header("��Ʈ�νý��� BattleIntro"), Space(30)]
		private IntroComponent _introComponent = null;

		private IEnumerator Start()
		{
			//������ 60 ����
			Application.targetFrameRate = 60;

			_pencilCaseComponent.SetInitialization(UnitComponent, CurrentStageData);
			_cardComponent.SetInitialization(this, WinLoseComponent, CameraComponent, UnitComponent, CostComponent, ref _updateAction, CurrentStageData, _pencilCaseComponent.PencilCaseDataMy._pencilCaseData._maxCard);
			_cameraComponent.SetInitialization(CardComponent, WinLoseComponent, ref _updateAction, CurrentStageData);
			_unitComponent.SetInitialization(ref _updateAction, CurrentStageData);
			_effectComponent.SetInitialization();
			_throwComponent.SetInitialization(ref _updateAction, _unitComponent, _cameraComponent, CurrentStageData);
			_unitSignComponent.SetInitialization();
			_aiComponent.SetInitialization(PencilCaseComponent, UnitComponent, ref _updateAction);
			_timeComponent.SetInitialization(ref _updateAction, CurrentStageData, _unitComponent, _cardComponent, _costComponent);
			_costComponent.SetInitialization(ref _updateAction, _pencilCaseComponent.PencilCaseDataMy._pencilCaseData);
			_pauseComponent.SetInitialization();
			_winLoseComponent.SetInitialization();
			_introComponent.SetInitialization(_cameraComponent, this);

			_isEndSetting = true;

			_introComponent.StartIntro();

			while (!_introComponent.isEndIntro)
			{
				yield return null;
			}
		}

		private void Update()
		{
			if (!_isEndSetting ||  !_introComponent.isEndIntro)
			{
				return;
			}
			if(BattleTurtorialComponent._isTutorial == false)
            {
				_cardComponent.CheckTutorialCard(); 
			}
			//������ �ý���
			if (Input.GetMouseButtonDown(0))
			{
				_throwComponent.ClickThrowUnit(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			}
			else if (Input.GetMouseButton(0))
			{
				_throwComponent.PullingThrowUnit(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			}
			else if (Input.GetMouseButtonUp(0))
			{
				_throwComponent.ThrowUnit();
			}

			//���� �ý���
			if (Input.GetKeyDown(KeyCode.Q))
			{
				_unitComponent.ClearUnit();
			}


			//������Ʈ���� ������Ʈ�� �ʿ��� �Լ� ���
			_updateAction.Invoke();
		}

		/// <summary>
		/// ������Ʈ �׼ǿ� ����� �Լ� �߰�
		/// </summary>
		/// <param name="method"></param>
		public void AddUpdateAction(Action method)
		{
			_updateAction += method;
		}


		/// <summary>
		/// ��ư�Լ�. ������ ��ȯ�� ���� ��
		/// </summary>
		public void OnChangeTeam()
		{
			//�� ������ �������� üũ
			if (UnitComponent.eTeam.Equals(TeamType.MyTeam))
			{
				UnitComponent.eTeam = TeamType.EnemyTeam;
				_unitTeamText.text = "���� ��";
				return;
			}
			if (UnitComponent.eTeam.Equals(TeamType.EnemyTeam))
			{
				UnitComponent.eTeam = TeamType.MyTeam;
				_unitTeamText.text = "���� ��";
				return;
			}
		}
	}

}