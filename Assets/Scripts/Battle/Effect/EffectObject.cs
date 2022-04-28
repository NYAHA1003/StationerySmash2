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
		/// ����Ʈ ���� �� ����
		/// </summary>
		/// <param name="pos">����Ʈ ��ġ</param>
		/// <param name="startLifeTime">����Ʈ �����ð�</param>
		/// <param name="isSetLifeTime">����Ʈ�� �����ð��� �ٲ� ������</param>
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