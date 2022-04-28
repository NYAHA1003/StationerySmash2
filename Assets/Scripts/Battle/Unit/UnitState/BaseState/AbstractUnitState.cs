using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Utill;
using Battle;

namespace Battle.Units
{

	public abstract class AbstractUnitState
	{
		//프로퍼티
		public eState CurState => _curState; //현재 스테이트 상태
		public eEvent CurEvent => _curEvent; //현재 이벤트 상태
		public AbstractUnitState NextState => _nextState; // 다음 상태
		public Transform MyTrm => _myTrm; //유닛 트랜스폼
		public Transform MySprTrm => _mySprTrm; // 유닛 스프라이트 트랜스폼
		public Unit MyUnit => _myUnit; //유닛
		public AbstractStateManager StateManager => _stateManager; //스테이트 매니저
		public UnitData MyUnitData => _myUnitData; //유닛 데이터


		//참조형 변수
		protected Transform _myTrm = null;
		protected Transform _mySprTrm = null;
		public AbstractUnitState _nextState = null; // 다음 상태
		protected AbstractStateManager _stateManager = null;
		protected UnitData _myUnitData = null;
		protected Unit _myUnit = null;

		//변수
		protected eState _curState = eState.IDLE;
		protected eEvent _curEvent = eEvent.ENTER;
		protected float[] originValue = default;
		protected Tweener _animationTweener = default;

		/// <summary>
		/// 풀링된 유닛에 맞춰 유닛변수들을 바꿔줌
		/// </summary>
		/// <param name="myTrm"></param>
		/// <param name="mySprTrm"></param>
		/// <param name="myUnit"></param>
		public void ChangeUnit(Transform myTrm, Transform mySprTrm, Unit myUnit)
		{
			_myTrm = myTrm;
			_mySprTrm = mySprTrm;
			_myUnit = myUnit;
			_myUnitData = _myUnit.UnitData;
			originValue = _myUnitData.unitablityData;
		}
		/// <summary>
		/// 스테이트 초기
		/// </summary>
		public virtual void Enter() { _curEvent = eEvent.UPDATE; }
		/// <summary>
		/// 스테이트 반복
		/// </summary>
		public virtual void Update() { _curEvent = eEvent.UPDATE; }
		/// <summary>
		/// 스테이트 말기
		/// </summary>
		public virtual void Exit() { _curEvent = eEvent.EXIT; }

		/// <summary>
		/// 로직 실행
		/// </summary>
		/// <returns></returns>
		public virtual AbstractUnitState Process()
		{
			if (_curEvent.Equals(eEvent.ENTER))
			{
				Enter();
			}
			if (_curEvent.Equals(eEvent.UPDATE))
			{
				Update();
			}
			if (_curEvent.Equals(eEvent.EXIT))
			{
				Exit();
				return _nextState;
			}

			return this;
		}

		/// <summary>
		/// 이벤트 상태 설정
		/// </summary>
		/// <param name="eEvent"></param>
		public void SetEvent(eEvent eEvent)
		{
			_curEvent = eEvent;
		}

		/// <summary>
		/// 스테이트 매니저를 설정해준다
		/// </summary>
		/// <param name="stateManager"></param>
		public void SetStateManager(AbstractStateManager stateManager)
		{
			_stateManager = stateManager;
		}


		/// <summary>
		/// 스테이트들을 초기화
		/// </summary>
		public void ResetState()
		{
			_nextState = null;
			_curEvent = eEvent.ENTER;
		}

		/// <summary>
		/// 데미지를 받을 때 사용하는 함수, 유닛에서 호출함
		/// </summary>
		/// <param name="atkData"></param>
		public virtual void RunDamaged(AtkData atkData)
		{
			if (atkData.damageId == -1)
			{
				//무조건 무시해야할 공격
				return;
			}
			if (atkData.damageId == _myUnit.MyDamagedId)
			{
				//똑같은 공격 아이디를 지닌 공격은 무시함
				return;
			}
			_myUnit.SetDamagedId(atkData.damageId);
			this._stateManager.Set_Damaged(atkData);
		}

		/// <summary>
		/// 던지기 유닛으로 선택 당했을 때 사용하는 함수, 유닛에서 호출함
		/// </summary>
		/// <returns></returns>
		public virtual Unit PullUnit()
		{
			if (_myUnit._isNeverDontThrow)
			{
				return null;
			}
			if (_myUnit._isDontThrow)
			{
				return null;
			}
			if (_curState.Equals(eState.DAMAGED))
			{
				return null;
			}

			_stateManager.Set_Wait(2);
			return _myUnit;
		}
		/// <summary>
		/// 당기고 있을 때 사용하는 함수, 유닛에서 함수
		/// </summary>
		/// <returns></returns>
		public virtual Unit PullingUnit()
		{
			if (_myUnit._isNeverDontThrow)
			{
				return null;
			}
			if (_myUnit._isDontThrow)
			{
				return null;
			}
			if (_curState.Equals(eState.DAMAGED))
			{
				return null;
			}

			return _myUnit;
		}
		/// <summary>
		/// 던져졌을 때 사용하는 함수, 던지기 상태로 변경함, 유닛에서 호출
		/// </summary>
		/// <param name="pos"></param>
		public virtual void ThrowUnit(Vector2 pos)
		{
			if (_myUnit._isNeverDontThrow)
			{
				return;
			}
			_stateManager.Set_ThrowPos(pos);
			_stateManager.Set_Throw();
		}
		/// <summary>
		/// 애니메이션 설정
		/// </summary>
		public virtual void SetAnimation()
		{
			//애니메이션 시퀀스 설정
		}

		/// <summary>
		/// 애니메이션 함수, 각각의 스테이트에서 오버라이드해서 호출함
		/// </summary>
		/// <param name="value"></param>
		public virtual void Animation()
		{
			//오버라이드해서 사용
		}

		/// <summary>
		/// 모든 스테이트들의 애니메이션 정지
		/// </summary>
		public void ResetAllStateAnimation()
		{
			_stateManager.ResetAnimationInStateList();
			ResetSprTrm();
		}

		/// <summary>
		/// 애니메이션을 제거하고 스프라이트 위치와 각도를 초기화
		/// </summary>
		public void ResetThisStateAnimation()
		{
			_animationTweener.Pause();
		}

		/// <summary>
		/// 스프라이트 트랜스폼 초기화
		/// </summary>
		public void ResetSprTrm()
		{
			_mySprTrm.localScale = Vector3.one;
			_mySprTrm.localPosition = Vector3.zero;
			_mySprTrm.localRotation = Quaternion.identity;
		}

		/// <summary>
		/// 넉백 설정 및 재생
		/// </summary>
		/// <param name="sequence"></param>
		public void SetKnockBack(Sequence sequence)
		{
			_myUnit.KnockbackTweener.Pause();
			_myUnit.SetKnockBack(sequence);
			_myUnit.KnockbackTweener.Play();
		}

		/// <summary>
		/// 스테이지 끝에 닿았는지 체크해서 닿으면 튕겨져 나오게 한다.
		/// </summary>
		public bool CheckWall()
		{
			if (_stateManager.GetStageData().max_Range <= _myTrm.position.x)
			{
				//왼쪽으로 튕겨져 나옴
				ResetAllStateAnimation();
				SetKnockBack(_myTrm.DOJump(new Vector3(_myTrm.position.x - 0.2f, 0, _myTrm.position.z), 0.3f, 1, 1).OnComplete(() =>
				{
					_stateManager.Set_Wait(0.5f);
				}).SetEase(Parabola.Return_ParabolaCurve()));
				return true;
			}
			if (-_stateManager.GetStageData().max_Range >= _myTrm.position.x)
			{
				//오른쪽으로 튕겨져 나옴
				ResetAllStateAnimation();
				SetKnockBack(_myTrm.DOJump(new Vector3(_myTrm.position.x + 0.2f, 0, _myTrm.position.z), 0.3f, 1, 1).OnComplete(() =>
				{
					_stateManager.Set_Wait(0.5f);
				}).SetEase(Parabola.Return_ParabolaCurve()));
				return true;
			}
			return false;
		}
	}


}