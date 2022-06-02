using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Tool;

namespace Utill.Data
{
	public enum ShaderType
	{
		DefaultShader = 0,
		ThrowShader,
		MyTeamNormalShader,
		EnemyTeamNormalShader,
	}

	public static class ShaderData
	{
		public static Dictionary<ShaderType, Material> _shaderDictionary = new Dictionary<ShaderType, Material>();


		/// <summary>
		/// ���׸��� ��ȯ�Ѵ�.
		/// </summary>
		/// <param name="skinType"></param>
		/// <returns></returns>
		public static Material GetShader(ShaderType shaderType)
		{
			Material material = null;
			if (_shaderDictionary.TryGetValue(shaderType, out material))
			{
				return material;

			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// �������� ��Ų�� ��������Ʈ�� ����Ѵ�.
		/// </summary>
		/// <param name="skinType"></param>
		public static void SetSkin(ShaderType shaderType)
		{
			if (_shaderDictionary.TryGetValue(shaderType, out var data))
			{
				return;
			}
			else
			{
				string name = System.Enum.GetName(typeof(ShaderType), shaderType);
				AddressableTool.GetAddressableAssetDicAsync<ShaderType, Material>(AddShaderDictionary, shaderType, name);
			}
		}

		/// <summary>
		/// ��ųʸ��� Ű�� ��������Ʈ�� �߰��Ѵ�
		/// </summary>
		/// <param name="skinType"></param>
		/// <param name="sprite"></param>
		private static void AddShaderDictionary(ShaderType shaderType, Material material)
		{
			if (_shaderDictionary.TryGetValue(shaderType, out var data))
			{
				return;
			}
			else
			{
				_shaderDictionary.Add(shaderType, material);
			}
		}
	}
}
