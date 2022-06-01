using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;
using DG.Tweening;


namespace Battle.Units
{

	public abstract class AbstractDieState : AbstractUnitState
	{
		public override void Enter()
		{
			_curState = eState.DIE;
			_curEvent = eEvent.ENTER;

			//���ָ���Ʈ���� ����
			_myUnit.RemoveUnitList();

			//��ƼĿ ���
			_myUnit.UnitSticker.RunDieStickerAbility(_curState);

			//�����̹� ���� UI �� ���̰� �ϰ� �����̻� ����
			_myUnit.UnitStateEff.DeleteEffStetes();
			_myUnit.SetIsDontThrow(true);
			_myUnit.UnitSprite.ShowUI(false);

			//����
			_myUnit.SetIsInvincibility(true);

			//�������� �״� �ִϸ��̼� ���
			_stateManager.SetAnimation(eState.DIE);
			RandomDieAnimation();
			_myUnit.StartCoroutine(DeleteUnit());

			base.Enter();
		}


		/// <summary>
		/// �������� 3���� ���� �ִϸ��̼��� �ϳ��� ���
		/// </summary>
		protected void RandomDieAnimation()
		{
			DieType dietype = Die.Return_RandomDieType();
			switch (dietype)
			{
				case DieType.StarKo:
					AnimationStarKO();
					break;
				case DieType.ScreenKo:
					AnimationScreenKO();
					break;
				case DieType.OutKo:
					AnimationOutKO();
					break;
			}
		}

		/// <summary>
		/// ȭ�鿡 �ε�ġ�� ����
		/// </summary>
		protected void AnimationScreenKO()
		{
			_myUnit.Animator.SetTrigger("isScreenKO");
		}

		/// <summary>
		/// ���󰡼� ���� �Ǵ� ����
		/// </summary>
		protected void AnimationStarKO()
		{
			_myUnit.Animator.SetTrigger("isStarKO");
		}

		/// <summary>
		/// �������� �ٱ������� ���󰡴� ����
		/// </summary>
		protected void AnimationOutKO()
		{
			_myUnit.Animator.SetTrigger("isOutKO");
		}

		/// <summary>
		/// �ִϸ��̼� ���� �� ���� ����
		/// </summary>
		/// <returns></returns>
		private IEnumerator DeleteUnit()
		{
			yield return new WaitForSeconds(1.5f);
			_myUnit.Delete_Unit();
			ResetSprTrm();
			_curEvent = eEvent.EXIT;
		}
	}

}