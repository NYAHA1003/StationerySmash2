using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;
using Battle;
using Battle.PCAbility;
using Battle.StateEff;
using Battle.Units;
using Battle.Badge;
using Battle.Sticker;

namespace Utill.Tool
{

	public class PoolManager : MonoBehaviour
	{
		private static BattleManager _battleManager;

		//상태 풀링
		public static Dictionary<string, object> stateDictionary = new Dictionary<string, object>();

		private void Awake()
		{
			_battleManager = FindObjectOfType<BattleManager>();
		}


		/// <summary>
		/// 오브젝트 생성
		/// </summary>
		/// <param name="prefeb">생성할 프리펩</param>
		/// <param name="position">생성할 위치</param>
		/// <param name="quaternion">생성할 때의 각도</param>
		/// <returns></returns>
		public static GameObject CreateObject(GameObject prefeb, Vector3 position, Quaternion quaternion)
		{
			return Instantiate(prefeb, position, quaternion);
		}
		public static Unit CreateUnit(GameObject prefeb, Vector3 position, Quaternion quaternion)
		{
			Unit unit = Instantiate(prefeb, position, quaternion).GetComponent<Unit>();
			unit.SetBattleManager(_battleManager);
			return unit;
		}

		/// <summary>
		/// 풀링매니저 생성
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="count"></param>
		public static void CreatePoolState<T>(Transform myTrm, Transform mySprTrm, Unit myUnit) where T : AbstractStateManager, new()
		{
			Queue<T> q = new Queue<T>();

			T g = new T();
			g.SetState();
			g.Reset_State(myTrm, mySprTrm, myUnit);

			q.Enqueue(g);

			try
			{
				stateDictionary.Add(typeof(T).Name, q);
			}
			catch
			{
				stateDictionary.Clear();
				stateDictionary.Add(typeof(T).Name, q);
			}
		}

		/// <summary>
		/// 상태 풀링 매니저 생성
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="myTrm"></param>
		/// <param name="mySprTrm"></param>
		/// <param name="myUnit"></param>
		/// <param name="statusEffect"></param>
		/// <param name="valueList"></param>
		public static void CreatePoolEff<T>(Transform myTrm, Transform mySprTrm, Unit myUnit, EffAttackType statusEffect, params float[] valueList) where T : EffState, new()
		{
			Queue<T> q = new Queue<T>();

			T g = new T();
			g.SetStateEff(myTrm, mySprTrm, myUnit, statusEffect, valueList);

			q.Enqueue(g);

			try
			{
				stateDictionary.Add(typeof(T).Name, q);
			}
			catch
			{
				stateDictionary.Clear();
				stateDictionary.Add(typeof(T).Name, q);
			}
		}

		/// <summary>
		/// 상태 풀링 매니저 생성
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="myTrm"></param>
		/// <param name="mySprTrm"></param>
		/// <param name="myUnit"></param>
		/// <param name="statusEffect"></param>
		/// <param name="valueList"></param>
		public static void CreatePoolPencilCase<T>() where T : AbstractPencilCaseAbility, new()
		{
			Queue<T> q = new Queue<T>();

			T g = new T();
			g.SetState(_battleManager);

			q.Enqueue(g);

			try
			{
				stateDictionary.Add(typeof(T).Name, q);
			}
			catch
			{
				stateDictionary.Clear();
				stateDictionary.Add(typeof(T).Name, q);
			}
		}
		/// <summary>
		/// 스티커 풀링 매니저 생성
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="myTrm"></param>
		/// <param name="mySprTrm"></param>
		/// <param name="myUnit"></param>
		/// <param name="statusEffect"></param>
		/// <param name="valueList"></param>
		public static void CreatePoolSticker<T>() where T : AbstractSticker, new()
		{
			Queue<T> q = new Queue<T>();

			T g = new T();

			q.Enqueue(g);

			try
			{
				stateDictionary.Add(typeof(T).Name, q);
			}
			catch
			{
				stateDictionary.Clear();
				stateDictionary.Add(typeof(T).Name, q);
			}
		}


		/// <summary>
		/// 뱃지 생성
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="myTrm"></param>
		/// <param name="mySprTrm"></param>
		/// <param name="myUnit"></param>
		/// <param name="statusEffect"></param>
		/// <param name="valueList"></param>
		public static void CreatePoolBadge<T>() where T : AbstractBadge, new()
		{
			Queue<T> q = new Queue<T>();

			T g = new T();
			g.SetBattleManager(_battleManager);

			q.Enqueue(g);

			try
			{
				stateDictionary.Add(typeof(T).Name, q);
			}
			catch
			{
				stateDictionary.Clear();
				stateDictionary.Add(typeof(T).Name, q);
			}
		}

		/// <summary>
		/// 안 쓰는 상태 가져오기
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T GetUnit<T>(Transform myTrm, Transform mySprTrm, Unit myUnit) where T : AbstractStateManager, new()
		{
			T item = default(T);

			if (stateDictionary.ContainsKey(typeof(T).Name))
			{
				Queue<T> q = (Queue<T>)stateDictionary[typeof(T).Name];

				if (q.Count == 0)
				{  //안 사용하는 상태가 없으면 새로운 상태를 만든다
					item = new T();
					item.SetState();
					item.Reset_State(myTrm, mySprTrm, myUnit);
				}
				else
				{
					item = q.Dequeue();
					item.Reset_State(myTrm, mySprTrm, myUnit);
				}
			}
			else
			{
				//한번도 해당 유닛의 관한 상태가 만들어진 적이 없다면 풀매니저를 만든다
				CreatePoolState<T>(myTrm, mySprTrm, myUnit);
				Queue<T> q = (Queue<T>)stateDictionary[typeof(T).Name];
				item = q.Dequeue();
				item.Reset_State(myTrm, mySprTrm, myUnit);
			}

			//할당
			return item;
		}

		/// <summary>
		/// 안 쓰는 상태이상 가져오기
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="myTrm"></param>
		/// <param name="mySprTrm"></param>
		/// <param name="myUnit"></param>
		/// <returns></returns>
		public static T GetEff<T>(Transform myTrm, Transform mySprTrm, Unit myUnit, EffAttackType statusEffect, params float[] valueList) where T : EffState, new()
		{
			T item = default(T);

			if (stateDictionary.ContainsKey(typeof(T).Name))
			{
				Queue<T> q = (Queue<T>)stateDictionary[typeof(T).Name];

				if (q.Count == 0)
				{  //안 사용하는 상태가 없으면 새로운 상태를 만든다
					item = new T();
					item.SetStateEff(myTrm, mySprTrm, myUnit, statusEffect, valueList);
				}
				else
				{
					item = q.Dequeue();
					item.SetStateEff(myTrm, mySprTrm, myUnit, statusEffect, valueList);
				}
			}
			else
			{
				CreatePoolEff<T>(myTrm, mySprTrm, myUnit, statusEffect, valueList);
				Queue<T> q = (Queue<T>)stateDictionary[typeof(T).Name];
				item = q.Dequeue();
				item.SetStateEff(myTrm, mySprTrm, myUnit, statusEffect, valueList);
			}

			//할당
			return item;
		}

		/// <summary>
		/// 안 쓰는 상태이상 가져오기
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="myTrm"></param>
		/// <param name="mySprTrm"></param>
		/// <param name="myUnit"></param>
		/// <returns></returns>
		public static T GetSticker<T>() where T : AbstractSticker, new()
		{
			T item = default(T);

			if (stateDictionary.ContainsKey(typeof(T).Name))
			{
				Queue<T> q = (Queue<T>)stateDictionary[typeof(T).Name];

				if (q.Count == 0)
				{  //안 사용하는 상태가 없으면 새로운 상태를 만든다
					item = new T();
				}
				else
				{
					item = q.Dequeue();
				}
			}
			else
			{
				CreatePoolSticker<T>();
				Queue<T> q = (Queue<T>)stateDictionary[typeof(T).Name];
				item = q.Dequeue();
			}

			//할당
			return item;
		}
		/// <summary>
		/// 안 쓰는 뱃지 가져오기
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="myTrm"></param>
		/// <param name="mySprTrm"></param>
		/// <param name="myUnit"></param>
		/// <returns></returns>
		public static T GetBadge<T>() where T : AbstractBadge, new()
		{
			T item = default(T);

			if (stateDictionary.ContainsKey(typeof(T).Name))
			{
				Queue<T> q = (Queue<T>)stateDictionary[typeof(T).Name];

				if (q.Count == 0)
				{  //안 사용하는 상태가 없으면 새로운 상태를 만든다
					item = new T();
				}
				else
				{
					item = q.Dequeue();
				}
			}
			else
			{
				CreatePoolBadge<T>();
				Queue<T> q = (Queue<T>)stateDictionary[typeof(T).Name];
				item = q.Dequeue();
			}

			//할당
			return item;
		}

		/// <summary>
		/// 안 쓰는 필통능력 가져오기
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="myTrm"></param>
		/// <param name="mySprTrm"></param>
		/// <param name="myUnit"></param>
		/// <returns></returns>
		public static T GetPencilCase<T>() where T : AbstractPencilCaseAbility, new()
		{
			T item = default(T);

			if (stateDictionary.ContainsKey(typeof(T).Name))
			{
				Queue<T> q = (Queue<T>)stateDictionary[typeof(T).Name];

				if (q.Count == 0)
				{  //안 사용하는 상태가 없으면 새로운 상태를 만든다
					item = new T();
					item.SetState(_battleManager);
				}
				else
				{
					item = q.Dequeue();
				}
			}
			else
			{
				CreatePoolPencilCase<T>();
				Queue<T> q = (Queue<T>)stateDictionary[typeof(T).Name];
				item = q.Dequeue();
			}

			//할당
			return item;
		}

		/// <summary>
		/// 다 쓴 유닛, 상태이상, 클래스 반납
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="state"></param>
		public static void AddItem<T>(T state) where T : class
		{
			Queue<T> q = (Queue<T>)stateDictionary[typeof(T).Name];
			q.Enqueue(state);
		}

		/// <summary>
		/// 유닛 스테이트매니저 반납
		/// </summary>
		/// <param name="state"></param>
		public static void AddUnitState(AbstractStateManager state)
		{
			var type = typeof(PoolManager);
			var method = type.GetMethod("AddItem");
			var gMethod = method.MakeGenericMethod(state.GetType());
			gMethod.Invoke(null, new object[] { state });
		}
		/// <summary>
		/// 상태이상 반납
		/// </summary>
		/// <param name="state"></param>
		public static void AddEffState(EffState state)
		{
			var type = typeof(PoolManager);
			var method = type.GetMethod("AddItem");
			var gMethod = method.MakeGenericMethod(state.GetType());
			gMethod.Invoke(null, new object[] { state });
		}

		/// <summary>
		/// 스티커 반납
		/// </summary>
		/// <param name="state"></param>
		public static void AddSticker(AbstractSticker state)
		{
			var type = typeof(PoolManager);
			var method = type.GetMethod("AddItem");
			var gMethod = method.MakeGenericMethod(state.GetType());
			gMethod.Invoke(null, new object[] { state });
		}
		/// <summary>
		/// 뱃지 반납
		/// </summary>
		/// <param name="state"></param>
		public static void AddBadge(AbstractBadge state)
		{
			var type = typeof(PoolManager);
			var method = type.GetMethod("AddItem");
			var gMethod = method.MakeGenericMethod(state.GetType());
			gMethod.Invoke(null, new object[] { state });
		}
	}

}