using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;


namespace Battle.Units
{

	public abstract class AbstractStateManager
	{
		public StageData _stageData { get; private set; } = null;

		protected AbstractIdleState _idleState = null;
		protected AbstractAttackState _attackState = null;
		protected AbstractDamagedState _damagedState = null;
		protected AbstractThrowState _throwState = null;
		protected AbstractDieState _dieState = null;
		protected AbstractMoveState _moveState = null;
		protected AbstractWaitState _waitState = null;
		protected AbstractUnitState _currrentState = null;
		protected List<AbstractUnitState> _abstractUnitStateList = new List<AbstractUnitState>();

		protected float _waitExtraTime = 0;

		public void Reset_CurrentUnitState(AbstractUnitState unitState)
		{
			_currrentState = unitState;
		}
		public AbstractUnitState Return_CurrentUnitState()
		{
			return _currrentState;
		}

		public abstract void SetState();

		/// <summary>
		/// 리스트에 있는 스테이트들 리셋
		/// </summary>
		public void SetInStateList()
		{
			for (int i = 0; i < _abstractUnitStateList.Count; i++)
			{
				_abstractUnitStateList[i].SetStateManager(this);
			}
		}

		/// <summary>
		/// 리스트에 있는 스테이트들의 애니메이션 정리
		/// </summary>
		public void ResetAnimationInStateList()
		{
			for (int i = 0; i < _abstractUnitStateList.Count; i++)
			{
				_abstractUnitStateList[i].ResetThisStateAnimation();
			}
		}

		public virtual void Reset_State(Transform myTrm, Transform mySprTrm, Unit myUnit)
		{
			for (int i = 0; i < _abstractUnitStateList.Count; i++)
			{
				_abstractUnitStateList[i].ChangeUnit(myTrm, mySprTrm, myUnit);
				_abstractUnitStateList[i].ResetState();
				_abstractUnitStateList[i].SetAnimation();
			}

			Set_WaitExtraTime(0);
			Reset_CurrentUnitState(_idleState);
		}

		public virtual void Set_Attack(Unit targetUnit)
		{
			_currrentState.SetEvent(eEvent.EXIT);
			_attackState.Set_Target(targetUnit);
			_currrentState._nextState = _attackState;
			_attackState.ResetState();
			Reset_CurrentUnitState(_attackState);
		}

		public virtual void Set_Damaged(AtkData atkData)
		{
			_currrentState.SetEvent(eEvent.EXIT);
			_damagedState.Set_AtkData(atkData);
			_currrentState._nextState = _damagedState;
			_damagedState.ResetState();
			Reset_CurrentUnitState(_damagedState);
		}

		public virtual void Set_Die()
		{
			_currrentState.SetEvent(eEvent.EXIT);
			_currrentState._nextState = _dieState;
			_dieState.ResetState();
			Reset_CurrentUnitState(_dieState);
		}

		public virtual void Set_Idle()
		{
			_currrentState.SetEvent(eEvent.EXIT);
			_currrentState._nextState = _idleState;
			_idleState.ResetState();
			Reset_CurrentUnitState(_idleState);
		}

		public virtual void Set_Move()
		{
			_currrentState.SetEvent(eEvent.EXIT);
			_currrentState._nextState = _moveState;
			_moveState.ResetState();
			Reset_CurrentUnitState(_moveState);
		}

		public virtual void Set_Throw()
		{
			_currrentState.SetEvent(eEvent.EXIT);
			_currrentState._nextState = _throwState;
			_throwState.ResetState();
			Reset_CurrentUnitState(_throwState);
		}

		public virtual void Set_Wait(float time)
		{
			_currrentState.SetEvent(eEvent.EXIT);
			_waitState.Set_Time(time);
			_waitState.Set_ExtraTime(_waitExtraTime);
			_currrentState._nextState = _waitState;
			_waitState.ResetState();
			Reset_CurrentUnitState(_waitState);
		}
		public virtual void Set_WaitExtraTime(float extraTime)
		{
			this._waitExtraTime = extraTime;
		}

		public virtual void Set_ThrowPos(Vector2 pos)
		{
			this._throwState.SetThrowPos(pos);
		}

		public void SetStageData(StageData stageData)
		{
			_stageData = stageData;
		}

		public StageData GetStageData()
		{
			return _stageData;
		}

	}

}