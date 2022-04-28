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
		Tweener _animationSKTweenerRotate = default;
		Tweener _animationSKTweenerJump = default;
		Tweener _animationSKTweenerScale = default;
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
			ResetAllStateAnimation();

			//랜덤으로 죽는 애니메이션 재생
			RandomDieAnimation();

			base.Enter();
		}

		public override void SetAnimation()
		{
			base.SetAnimation();


			//_animationSKTweenerRotate = _mySprTrm.DOLocalRotate(new Vector3(0, 0, -360), 0.3f, RotateMode.FastBeyond360).SetLoops(3, LoopType.Incremental);
			////_animationSKTweenerJump =
			//_myTrm.DOJump(Vector3.zero, 2f, 1, 1f);
			//_mySprTrm.DOScale(10, 0.6f).SetDelay(0.3f).SetEase(Utill.Parabola.Return_ScreenKoCurve()).OnComplete(() =>
			//{
			//    _mySprTrm.eulerAngles = new Vector3(0, 0, Random.Range(_mySprTrm.eulerAngles.z - 10, _mySprTrm.eulerAngles.z + 10));
			//    _mySprTrm.DOShakePosition(0.6f, 0.1f, 30).OnComplete(() =>
			//    {
			//        _mySprTrm.DOMoveY(-3, 1).OnComplete(() =>
			//        {
			//            ResetSprTrm();
			//            _curEvent = eEvent.EXIT;
			//            _myUnit.Delete_Unit();
			//        });
			//    });
			//
			//});
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
			//날라가는 위치 설정
			Vector3 diePos = new Vector3(_myTrm.position.x, _myTrm.position.y + 0.4f, 0);
			if (_myUnit.ETeam == TeamType.MyTeam)
			{
				diePos.x -= Random.Range(0.1f, 0.2f);
			}
			if (_myUnit.ETeam == TeamType.EnemyTeam)
			{
				diePos.x += Random.Range(0.1f, 0.2f);
			}

			_mySprTrm.DOLocalRotate(new Vector3(0, 0, -360), 0.3f, RotateMode.FastBeyond360).SetLoops(3, LoopType.Incremental);
			_myTrm.DOJump(diePos, 2f, 1, 1f);
			_mySprTrm.DOScale(10, 0.6f).SetDelay(0.3f).SetEase(Parabola.Return_ScreenKoCurve()).OnComplete(() =>
			{
				_mySprTrm.eulerAngles = new Vector3(0, 0, Random.Range(_mySprTrm.eulerAngles.z - 10, _mySprTrm.eulerAngles.z + 10));
				_mySprTrm.DOShakePosition(0.6f, 0.1f, 30).OnComplete(() =>
				{
					_mySprTrm.DOMoveY(-3, 1).OnComplete(() =>
					{
						ResetSprTrm();
						_curEvent = eEvent.EXIT;
						_myUnit.Delete_Unit();
					});
				});

			});
		}

		/// <summary>
		/// 날라가서 별이 되는 죽음
		/// </summary>
		protected void AnimationStarKO()
		{
			//날라가는 위치 설정
			Vector3 diePos = new Vector3(_myTrm.position.x, 1, 0);
			if (_myUnit.ETeam == TeamType.MyTeam)
			{
				diePos.x += Random.Range(-2f, -1f);
			}
			if (_myUnit.ETeam == TeamType.EnemyTeam)
			{
				diePos.x += Random.Range(1f, 2f);
			}

			_mySprTrm.DOLocalRotate(new Vector3(0, 0, -360), 0.5f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental);
			_mySprTrm.DOScale(0.1f, 1f).SetDelay(1);
			_myTrm.DOJump(diePos, 3, 1, 2f).OnComplete(() =>
			{
				ResetSprTrm();
				_curEvent = eEvent.EXIT;
				_myUnit.Delete_Unit();
			});
		}

		/// <summary>
		/// 스테이지 바깥쪽으로 날라가는 죽음
		/// </summary>
		protected void AnimationOutKO()
		{
			//날라가는 위치 설정
			Vector3 diePos = new Vector3(_myTrm.position.x, _myTrm.position.y - 2, 0);
			if (_myUnit.ETeam == TeamType.MyTeam)
			{
				diePos.x -= _stateManager.GetStageData().max_Range + 1;
			}
			if (_myUnit.ETeam == TeamType.EnemyTeam)
			{
				diePos.x += _stateManager.GetStageData().max_Range + 1;
			}

			float time = Mathf.Abs(_myTrm.position.x - diePos.x) / 2;
			_mySprTrm.DOScale(3f, time);
			_mySprTrm.DOLocalRotate(new Vector3(0, 0, -360), time, RotateMode.FastBeyond360);
			_myTrm.DOJump(diePos, Random.Range(3, 5), 1, time).OnComplete(() =>
			{
				ResetSprTrm();
				_curEvent = eEvent.EXIT;
				_myUnit.Delete_Unit();
			});
		}

	}

}