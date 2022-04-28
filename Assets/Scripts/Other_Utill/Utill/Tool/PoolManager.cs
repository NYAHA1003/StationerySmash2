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

		//���� Ǯ��
		public static Dictionary<string, object> stateDictionary = new Dictionary<string, object>();

		private void Awake()
		{
			_battleManager = FindObjectOfType<BattleManager>();
		}


		/// <summary>
		/// ������Ʈ ����
		/// </summary>
		/// <param name="prefeb">������ ������</param>
		/// <param name="position">������ ��ġ</param>
		/// <param name="quaternion">������ ���� ����</param>
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
		/// Ǯ���Ŵ��� ����
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
		/// ���� Ǯ�� �Ŵ��� ����
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
		/// ���� Ǯ�� �Ŵ��� ����
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
		/// ��ƼĿ Ǯ�� �Ŵ��� ����
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
		/// ���� ����
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
		/// �� ���� ���� ��������
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
				{  //�� ����ϴ� ���°� ������ ���ο� ���¸� �����
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
				//�ѹ��� �ش� ������ ���� ���°� ������� ���� ���ٸ� Ǯ�Ŵ����� �����
				CreatePoolState<T>(myTrm, mySprTrm, myUnit);
				Queue<T> q = (Queue<T>)stateDictionary[typeof(T).Name];
				item = q.Dequeue();
				item.Reset_State(myTrm, mySprTrm, myUnit);
			}

			//�Ҵ�
			return item;
		}

		/// <summary>
		/// �� ���� �����̻� ��������
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
				{  //�� ����ϴ� ���°� ������ ���ο� ���¸� �����
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

			//�Ҵ�
			return item;
		}

		/// <summary>
		/// �� ���� �����̻� ��������
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
				{  //�� ����ϴ� ���°� ������ ���ο� ���¸� �����
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

			//�Ҵ�
			return item;
		}
		/// <summary>
		/// �� ���� ���� ��������
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
				{  //�� ����ϴ� ���°� ������ ���ο� ���¸� �����
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

			//�Ҵ�
			return item;
		}

		/// <summary>
		/// �� ���� ����ɷ� ��������
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
				{  //�� ����ϴ� ���°� ������ ���ο� ���¸� �����
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

			//�Ҵ�
			return item;
		}

		/// <summary>
		/// �� �� ����, �����̻�, Ŭ���� �ݳ�
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="state"></param>
		public static void AddItem<T>(T state) where T : class
		{
			Queue<T> q = (Queue<T>)stateDictionary[typeof(T).Name];
			q.Enqueue(state);
		}

		/// <summary>
		/// ���� ������Ʈ�Ŵ��� �ݳ�
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
		/// �����̻� �ݳ�
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
		/// ��ƼĿ �ݳ�
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
		/// ���� �ݳ�
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