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
		/// 투사체 유닛 데이터를 가져와 반환한다
		/// </summary>
		/// <param name="unitType"></param>
		public static async Task<CardData> ReturnProjectileUnitAsync(UnitType unitType)
		{
			AsyncOperationHandle<UnitDataSO> handle = Addressables.LoadAssetAsync<UnitDataSO>("ProjectileUnitSO");
			handle.WaitForCompletion();
			return handle.Result.unitDatas.Find(x => x.unitType == unitType);
			//반환값 주의사항 함수 뒤에 .Result를 붙여야함
		}
	}
}
