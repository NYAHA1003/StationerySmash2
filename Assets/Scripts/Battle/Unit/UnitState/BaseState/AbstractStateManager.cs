using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;
using DG.Tweening;

namespace Battle.Units
{

	public abstract class AbstractStateManager
	{
		public StageData _stageData { get; private set; } = null;
		public Tweener AnimationTweener => _animationTweener;
		public Animator Animator => _animator;

		protected AbstractIdleState _idleState = null;
		protected AbstractAttackState _attackState = null;
		protected AbstractDamagedState _damagedState = null;
		protected AbstractThrowState _throwState = null;
		protected AbstractDieState _dieState = null;
		protected AbstractMoveState _moveState = null;
		protected AbstractWaitState _waitState = null;
		protected AbstractUnitState _currrentState = null;
		protected List<AbstractUnitState> _abstractUnitStateList = new List<AbstractUnitState>();
		protected Tweener _animationTweener = default;
		protected Animator _animator = null;


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
		/// ����Ʈ�� �ִ� ������Ʈ�� ����
		/// </summary>
		public void SetInStateList()
		{
			for (int i = 0; i < _abstractUnitStateList.Count; i++)
			{
				_abstractUnitStateList[i].SetStateManager(this);
			}
		}

		public void SetAnimation(eState eState)
		{
			switch (eState)
			{
				case eState.IDLE:
					AllFasleAnimation();
					break;
				case eState.MOVE:
					_animator.SetBool("isMove", true);
					_animator.SetBool("isWait", false);
					break;
				case eState.ATTACK:
					_animator.SetTrigger("isAttack");
					break;
				case eState.WAIT:
					_animator.SetBool("isWait", true);
					_animator.SetBool("isThrow", false);
					_animator.SetBool("isMove", false);
					_animator.SetBool("isDamaged", false);
					break;
				case eState.DAMAGED:
					_animator.SetBool("isDamaged", true);
					_animator.SetBool("isMove", false);
					_animator.SetBool("isWait", false);
					_animator.SetBool("isThrow", false);
					break;
				case eState.DIE:
					_animator.SetBool("isDie", true);
					_animator.SetBool("isWait", false);
					break;
				case eState.THROW:
					_animator.SetBool("isThrow", true);
					_animator.SetBool("isMove", false);
					_animator.SetBool("isWait", false);
					break;
				case eState.NONE:
					break;
			}
		}

		public virtual void Reset_State(Transform myTrm, Transform mySprTrm, Unit myUnit)
		{
			_animator = myUnit.Animator;
			AllFasleAnimation();

			for (int i = 0; i < _abstractUnitStateList.Count; i++)
			{
				_abstractUnitStateList[i].ChangeUnit(myTrm, mySprTrm, myUnit);
				_abstractUnitStateList[i].ResetState();
			}

			Set_WaitExtraTime(0);
			Reset_CurrentUnitState(_idleState);
		}

		/// <summary>
		/// ���¸� �������� �ٲ۴�
		/// </summary>
		/// <param name="targetUnit"></param>
		public virtual void Set_Attack(Unit targetUnit)
		{
			_currrentState.SetEvent(eEvent.EXIT);
			_attackState.Set_Target(targetUnit);
			_currrentState._nextState = _attackState;
			_attackState.ResetState();
			Reset_CurrentUnitState(_attackState);
		}

		/// <summary>
		/// ���¸� ���� �������� �ٲ۴�
		/// </summary>
		/// <param name="atkData"></param>
		public virtual void Set_Damaged(AtkData atkData)
		{
			_currrentState.SetEvent(eEvent.EXIT);
			_damagedState.Set_AtkData(atkData);
			_currrentState._nextState = _damagedState;
			_damagedState.ResetState();
			Reset_CurrentUnitState(_damagedState);
		}

		/// <summary>
		/// ���¸� �������� �ٲ۴�
		/// </summary>
		public virtual void Set_Die()
		{
			_currrentState.SetEvent(eEvent.EXIT);
			_currrentState._nextState = _dieState;
			_dieState.ResetState();
			Reset_CurrentUnitState(_dieState);
		}

		/// <summary>
		/// ���¸� ��ȯ������ �ٲ۴�
		/// </summary>
		public virtual void Set_Idle()
		{
			_currrentState.SetEvent(eEvent.EXIT);
			_currrentState._nextState = _idleState;
			_idleState.ResetState();
			Reset_CurrentUnitState(_idleState);
		}

		/// <summary>
		/// ���¸� �̵����� �ٲ۴�
		/// </summary>
		public virtual void Set_Move()
		{
			_currrentState.SetEvent(eEvent.EXIT);
			_currrentState._nextState = _moveState;
			_moveState.ResetState();
			Reset_CurrentUnitState(_moveState);
		}

		/// <summary>
		/// ���¸� ������� �ٲ۴�
		/// </summary>
		public virtual void Set_Throw()
		{
			_currrentState.SetEvent(eEvent.EXIT);
			_currrentState._nextState = _throwState;
			_throwState.ResetState();
			Reset_CurrentUnitState(_throwState);
		}

		/// <summary>
		/// ���¸� ���� �ٲ۴�
		/// </summary>
		/// <param name="time"></param>
		public virtual void Set_Wait(float time)
		{
			_currrentState.SetEvent(eEvent.EXIT);
			_waitState.Set_Time(time);
			_waitState.Set_ExtraTime(_waitExtraTime);
			_currrentState._nextState = _waitState;
			_waitState.ResetState();
			Reset_CurrentUnitState(_waitState);
		}

		/// <summary>
		/// �߰� ��� �ð� ����
		/// </summary>
		/// <param name="extraTime"></param>
		public virtual void Set_WaitExtraTime(float extraTime)
		{
			this._waitExtraTime = extraTime;
		}

		/// <summary>
		/// �������� ��ġ ����
		/// </summary>
		/// <param name="pos"></param>
		public virtual void Set_ThrowPos(Vector2 pos)
		{
			this._throwState.SetThrowPos(pos);
		}

		/// <summary>
		/// �������� ������ ����
		/// </summary>
		/// <param name="stageData"></param>
		public void SetStageData(StageData stageData)
		{
			_stageData = stageData;
		}

		/// <summary>
		/// ��� �ִϸ��̼� �Ķ���͸� False�� ó��
		/// </summary>
		public void AllFasleAnimation()
		{
			_animator.SetBool("isWait", false);
			_animator.SetBool("isMove", false);
			_animator.SetBool("isDamaged", false);
			_animator.SetBool("isThrow", false);
			_animator.SetBool("isDie", false);
		}

		public StageData GetStageData()
		{
			return _stageData;
		}

	}

}