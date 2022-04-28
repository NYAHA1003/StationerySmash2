using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

namespace Battle.Effect
{

	public class EffectObject : MonoBehaviour
	{
		public EffectType _effectType;
		public IEffect _effectState;
		private EffData _effData;
		private bool _isSettingEnd;

		/// <summary>
		/// 이펙트 리셋 및 설정
		/// </summary>
		/// <param name="pos">이펙트 위치</param>
		/// <param name="startLifeTime">이펙트 유지시간</param>
		/// <param name="isSetLifeTime">이펙트를 유지시간을 바꿀 것인지</param>
		public void SetEffect(EffData effData)
		{
			switch (_effectType)
			{
				case EffectType.Attack:
					_effectState ??= new Effect_Attack();
					break;
				case EffectType.Stun:
					_effectState ??= new Effect_Stun();
					break;
				case EffectType.Ink:
					//effectState ??= new Effect_Slow();
					break;
				case EffectType.Slow:
					_effectState ??= new Effect_Slow();
					break;
			}

			this._effData = effData;
			_effectState.Set_Effect(this, effData);

			_isSettingEnd = true;
		}

		private void Update()
		{
			if (!_isSettingEnd)
				return;
			_effData.lifeTime -= Time.deltaTime;
			_effectState.Update_Effect(this, _effData);
		}

		public void Delete_Effect()
		{
			gameObject.SetActive(false);
			_isSettingEnd = false;
		}
	}

}