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
		//������Ƽ
		public eState CurState => _curState; //���� ������Ʈ ����
		public eEvent CurEvent => _curEvent; //���� �̺�Ʈ ����
		public AbstractUnitState NextState => _nextState; // ���� ����
		public Transform MyTrm => _myTrm; //���� Ʈ������
		public Transform MySprTrm => _mySprTrm; // ���� ��������Ʈ Ʈ������
		public Unit MyUnit => _myUnit; //����
		public AbstractStateManager StateManager => _stateManager; //������Ʈ �Ŵ���
		public UnitData MyUnitData => _myUnitData; //���� ������


		//������ ����
		protected Transform _myTrm = null;
		protected Transform _mySprTrm = null;
		public AbstractUnitState _nextState = null; // ���� ����
		protected AbstractStateManager _stateManager = null;
		protected UnitData _myUnitData = null;
		protected Unit _myUnit = null;

		//����
		protected eState _curState = eState.IDLE;
		protected eEvent _curEvent = eEvent.ENTER;
		protected float[] originValue = default;
		protected Tweener _animationTweener = default;

		/// <summary>
		/// Ǯ���� ���ֿ� ���� ���ֺ������� �ٲ���
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
		/// ������Ʈ �ʱ�
		/// </summary>
		public virtual void Enter() { _curEvent = eEvent.UPDATE; }
		/// <summary>
		/// ������Ʈ �ݺ�
		/// </summary>
		public virtual void Update() { _curEvent = eEvent.UPDATE; }
		/// <summary>
		/// ������Ʈ ����
		/// </summary>
		public virtual void Exit() { _curEvent = eEvent.EXIT; }

		/// <summary>
		/// ���� ����
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
		/// �̺�Ʈ ���� ����
		/// </summary>
		/// <param name="eEvent"></param>
		public void SetEvent(eEvent eEvent)
		{
			_curEvent = eEvent;
		}

		/// <summary>
		/// ������Ʈ �Ŵ����� �������ش�
		/// </summary>
		/// <param name="stateManager"></param>
		public void SetStateManager(AbstractStateManager stateManager)
		{
			_stateManager = stateManager;
		}


		/// <summary>
		/// ������Ʈ���� �ʱ�ȭ
		/// </summary>
		public void ResetState()
		{
			_nextState = null;
			_curEvent = eEvent.ENTER;
		}

		/// <summary>
		/// �������� ���� �� ����ϴ� �Լ�, ���ֿ��� ȣ����
		/// </summary>
		/// <param name="atkData"></param>
		public virtual void RunDamaged(AtkData atkData)
		{
			if (atkData.damageId == -1)
			{
				//������ �����ؾ��� ����
				return;
			}
			if (atkData.damageId == _myUnit.MyDamagedId)
			{
				//�Ȱ��� ���� ���̵� ���� ������ ������
				return;
			}
			_myUnit.SetDamagedId(atkData.damageId);
			this._stateManager.Set_Damaged(atkData);
		}

		/// <summary>
		/// ������ �������� ���� ������ �� ����ϴ� �Լ�, ���ֿ��� ȣ����
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
		/// ���� ���� �� ����ϴ� �Լ�, ���ֿ��� �Լ�
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
		/// �������� �� ����ϴ� �Լ�, ������ ���·� ������, ���ֿ��� ȣ��
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
		/// �ִϸ��̼� ����
		/// </summary>
		public virtual void SetAnimation()
		{
			//�ִϸ��̼� ������ ����
		}

		/// <summary>
		/// �ִϸ��̼� �Լ�, ������ ������Ʈ���� �������̵��ؼ� ȣ����
		/// </summary>
		/// <param name="value"></param>
		public virtual void Animation()
		{
			//�������̵��ؼ� ���
		}

		/// <summary>
		/// ��� ������Ʈ���� �ִϸ��̼� ����
		/// </summary>
		public void ResetAllStateAnimation()
		{
			_stateManager.ResetAnimationInStateList();
			ResetSprTrm();
		}

		/// <summary>
		/// �ִϸ��̼��� �����ϰ� ��������Ʈ ��ġ�� ������ �ʱ�ȭ
		/// </summary>
		public void ResetThisStateAnimation()
		{
			_animationTweener.Pause();
		}

		/// <summary>
		/// ��������Ʈ Ʈ������ �ʱ�ȭ
		/// </summary>
		public void ResetSprTrm()
		{
			_mySprTrm.localScale = Vector3.one;
			_mySprTrm.localPosition = Vector3.zero;
			_mySprTrm.localRotation = Quaternion.identity;
		}

		/// <summary>
		/// �˹� ���� �� ���
		/// </summary>
		/// <param name="sequence"></param>
		public void SetKnockBack(Sequence sequence)
		{
			_myUnit.KnockbackTweener.Pause();
			_myUnit.SetKnockBack(sequence);
			_myUnit.KnockbackTweener.Play();
		}

		/// <summary>
		/// �������� ���� ��Ҵ��� üũ�ؼ� ������ ƨ���� ������ �Ѵ�.
		/// </summary>
		public bool CheckWall()
		{
			if (_stateManager.GetStageData().max_Range <= _myTrm.position.x)
			{
				//�������� ƨ���� ����
				ResetAllStateAnimation();
				SetKnockBack(_myTrm.DOJump(new Vector3(_myTrm.position.x - 0.2f, 0, _myTrm.position.z), 0.3f, 1, 1).OnComplete(() =>
				{
					_stateManager.Set_Wait(0.5f);
				}).SetEase(Parabola.Return_ParabolaCurve()));
				return true;
			}
			if (-_stateManager.GetStageData().max_Range >= _myTrm.position.x)
			{
				//���������� ƨ���� ����
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