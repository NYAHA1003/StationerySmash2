using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using Utill.Data;
using System.Threading.Tasks;

namespace Utill.Tool
{
	public class AddressableTool
	{
		/// <summary>
		/// ����ü ���� �����͸� ������ ��ȯ�Ѵ�
		/// </summary>
		/// <param name="unitType"></param>
		public static async Task<CardData> ReturnProjectileUnitAsync(UnitType unitType)
		{
			AsyncOperationHandle<UnitDataSO> handle = Addressables.LoadAssetAsync<UnitDataSO>("ProjectileUnitSO");
			handle.WaitForCompletion();
			return handle.Result.unitDatas.Find(x => x.unitType == unitType);
			//��ȯ�� ���ǻ��� �Լ� �ڿ� .Result�� �ٿ�����
		}
	}
}
