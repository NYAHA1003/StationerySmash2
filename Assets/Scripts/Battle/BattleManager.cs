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
		//프로퍼티들
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

		//변수
		private bool _isEndSetting = false;
		private Action _updateAction = () => { };

		//인스펙터 변수
		[SerializeField]
		private StageDataSO _stageDataSO = null;

		//컴포넌트들
		[SerializeField, Header("카드시스템 BattleCard"), Space(30)]
		private CardComponent _cardComponent = null;
		[SerializeField, Header("유닛시스템 BattleUnit"), Space(30)]
		private UnitComponent _unitComponent = null;
		[SerializeField, Header("카메라시스템 BattleCamera"), Space(30)]
		private CameraComponent _cameraComponent = null;
		[SerializeField, Header("이펙트 시스템 BattleEffect"), Space(30)]
		private EffectComponent _effectComponent = null;
		[SerializeField, Header("던지기 시스템 BattleThrow"), Space(30)]
		private ThrowComponent _throwComponent = null;
		[SerializeField, Header("던지기바 유닛 표시시스템 BattleUnitSign"), Space(30)]
		private UnitSignComponent _unitSignComponent = null;
		[SerializeField, Header("시간시스템 BattleTime"), Space(30)]
		private TimeComponent _timeComponent = null;
		[SerializeField, Header("AI 시스템 BattleAi"), Space(30)]
		private AIComponent _aiComponent = null;
		[SerializeField, Header("코스트 시스템 BattleCost"), Space(30)]
		private CostComponent _costComponent = null;
		[SerializeField, Header("필통시스템 BattlePencilCase"), Space(30)]
		private PencilCaseComponent _pencilCaseComponent = null;
		[SerializeField, Header("일시정지시스템 Battle_Pause"), Space(30)]
		private PauseComponent _pauseComponent = null;
		[SerializeField, Header("승리패배시스템 BattleWinLose"), Space(30)]
		private WinLoseComponent _winLoseComponent = null;
		[SerializeField, Header("인트로시스템 BattleIntro"), Space(30)]
		private IntroComponent _introComponent = null;

		private IEnumerator Start()
		{
			//프레임 60 고정
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
			//던지기 시스템
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

			//유닛 시스템
			if (Input.GetKeyDown(KeyCode.Q))
			{
				_unitComponent.ClearUnit();
			}


			//컴포넌트들의 업데이트가 필요한 함수 재생
			_updateAction.Invoke();
		}

		/// <summary>
		/// 업데이트 액션에 사용할 함수 추가
		/// </summary>
		/// <param name="method"></param>
		public void AddUpdateAction(Action method)
		{
			_updateAction += method;
		}


		/// <summary>
		/// 버튼함수. 유닛을 소환할 때의 팀
		/// </summary>
		public void OnChangeTeam()
		{
			//내 팀인지 적팀인지 체크
			if (UnitComponent.eTeam.Equals(TeamType.MyTeam))
			{
				UnitComponent.eTeam = TeamType.EnemyTeam;
				_unitTeamText.text = "적의 팀";
				return;
			}
			if (UnitComponent.eTeam.Equals(TeamType.EnemyTeam))
			{
				UnitComponent.eTeam = TeamType.MyTeam;
				_unitTeamText.text = "나의 팀";
				return;
			}
		}
	}

}