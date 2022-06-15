using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill.Tool;

namespace Utill.Data
{
    public class AnimationData : MonoBehaviour
	{
		public static Dictionary<SkinType, RuntimeAnimatorController> _animatorDictionary = new Dictionary<SkinType, RuntimeAnimatorController>();

		/// <summary>
		/// �������� ��Ų�� ��������Ʈ�� ����Ѵ�.
		/// </summary>
		/// <param name="skinType"></param>
		public static void SetAnimator(SkinType skintype)
		{
			if (_animatorDictionary.TryGetValue(skintype, out var data))
			{
				return;
			}
			else
			{
				string address = System.Enum.GetName(typeof(SkinType), skintype) + "_Anim";
				AddressableTool.GetAddressableAssetDicAsync<SkinType, RuntimeAnimatorController>(AddAnimatorDictionary, skintype, address);
			}
		}


		/// <summary>
		/// ��ųʸ��� Ű�� �ִϸ����͸� �߰��Ѵ�
		/// </summary>
		/// <param name="skinType"></param>
		/// <param name="sprite"></param>
		private static void AddAnimatorDictionary(SkinType skintype, RuntimeAnimatorController runtimeAnimatorController)
		{
			if (_animatorDictionary.TryGetValue(skintype, out var data))
			{
				return;
			}
			else
			{
				_animatorDictionary.Add(skintype, runtimeAnimatorController);
			}
		}

		/// <summary>
		/// �ش� Ÿ�� ī���� �ִϸ��̼� ������ ��������
		/// </summary>
		/// <param name="cardNamingType"></param>
		/// <returns></returns>
		public static RuntimeAnimatorController GetAnimator(SkinType skinType)
		{
			_animatorDictionary.TryGetValue(skinType, out var animator);
			return animator;
		}
	}

}