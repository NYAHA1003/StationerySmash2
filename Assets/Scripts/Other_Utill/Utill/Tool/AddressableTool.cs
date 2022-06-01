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
		/// 어드레서블 에셋을 가져온다
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
	}
}
