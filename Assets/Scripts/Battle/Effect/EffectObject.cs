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
			//��Ȱ�� ������ �ڵ�. ���� ��� �� ��

			//for (int i = 0; i < System.Enum.GetValues(typeof(EffectType)).Length; i++)
			//{
			//    string effType = System.Enum.GetName(typeof(EffectType), i);
			//    //enum�����ͼ� playSound
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