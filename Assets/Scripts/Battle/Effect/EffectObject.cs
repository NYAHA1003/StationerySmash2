using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;
using Main.Setting;
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
					_effectState ??= new EffectAttack();
					break;
				case EffectType.Stun:
					_effectState ??= new EffectStun();
					break;
				case EffectType.Ink:
					_effectState ??= new EffectInk();
					break;
				case EffectType.Slow:
					_effectState ??= new EffectSlow();
					break;
				case EffectType.Throw:
					_effectState ??= new EffectThrowAttack();
					break;
			}

			this._effData = effData;
			_effectState.Set_Effect(this, effData);

			_isSettingEnd = true;
		}
		public void PlaySound(EffData effData)
		{
			//재활용 가능한 코드. 아직 사용 안 함

			//for (int i = 0; i < System.Enum.GetValues(typeof(EffectType)).Length; i++)
			//{
			//    string effType = System.Enum.GetName(typeof(EffectType), i);
			//    //enum가져와서 playSound
			//    foreach (EffSoundType eff in Sound._effectSoundDictionary.Keys)
			//    {
			//        string soundType = System.Enum.GetName(typeof(EffSoundType), eff);
			//        if (effType == soundType)
			//        {
			//            Sound.PlayEff(i);
			//        }
			//    }
			//}
			Sound.PlayEff(EffSoundType.Attack);
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