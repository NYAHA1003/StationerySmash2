using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using Utill.Data;
using System.Threading.Tasks;
using System;

namespace Utill.Tool
{
	public static class AddressableTool
	{
		/// <summary>
		/// ��巹���� ������ �����´�
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name"></param>
		/// <returns></returns>
		public static async void GetAddressableAssetAsync<T>(Action<T> actioin, string name)
		{
			var handle = Addressables.LoadAssetAsync<T>(name);
			await handle.Task;
			actioin.Invoke(handle.Result);
		}

		/// <summary>
		/// ��巹���� ������ �����´� ��ųʸ��뵵
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name"></param>
		/// <returns></returns>
		public static async void GetAddressableAssetDicAsync<K, T>(Action<K, T> actioin, K key, string name)
		{
			var handle = Addressables.LoadAssetAsync<T>(name);
			await handle.Task;
			actioin.Invoke(key, handle.Result);
		}
	}
}
