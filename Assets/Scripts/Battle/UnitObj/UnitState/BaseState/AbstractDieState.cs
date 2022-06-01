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

			//유닛리스트에서 제거
			_myUnit.RemoveUnitList();

			//스티커 사용
			_myUnit.UnitSticker.RunDieStickerAbility(_curState);

			//딜레이바 등의 UI 안 보이게 하고 상태이상 삭제
			_myUnit.UnitStateEff.DeleteEffStetes();
			_myUnit.SetIsDontThrow(true);
			_myUnit.UnitSprite.ShowUI(false);

			//뒤짐
			_myUnit.SetIsInvincibility(true);

			//랜덤으로 죽는 애니메이션 재생
			_stateManager.SetAnimation(eState.DIE);
			RandomDieAnimation();
			_myUnit.StartCoroutine(DeleteUnit());

			base.Enter();
		}


		/// <summary>
		/// 랜덤으로 3가지 죽음 애니메이션중 하나를 사용
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
		/// 화면에 부딪치는 죽음
		/// </summary>
		protected void AnimationScreenKO()
		{
			_myUnit.Animator.SetTrigger("isScreenKO");
		}

		/// <summary>
		/// 날라가서 별이 되는 죽음
		/// </summary>
		protected void AnimationStarKO()
		{
			_myUnit.Animator.SetTrigger("isStarKO");
		}

		/// <summary>
		/// 스테이지 바깥쪽으로 날라가는 죽음
		/// </summary>
		protected void AnimationOutKO()
		{
			_myUnit.Animator.SetTrigger("isOutKO");
		}

		/// <summary>
		/// 애니메이션 끝난 후 유닛 삭제
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