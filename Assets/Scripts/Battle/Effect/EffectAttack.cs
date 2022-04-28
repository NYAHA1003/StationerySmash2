using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using Battle.Effect;

namespace Battle.Effect
{

	public class EffectAttack : IEffect
	{
		[SerializeField]
		private ParticleSystem particleSys;
		[SerializeField]
		private float delete_time = 0.5f;
		public virtual void Set_Effect(EffectObject effObj, EffData effData)
		{
			particleSys ??= effObj.GetComponent<ParticleSystem>();

			effObj.CancelInvoke();

			effObj.gameObject.SetActive(true);

			//재생 시간 설정
			var main = particleSys.main;
			main.startLifetime = effData.lifeTime;

			particleSys.Stop();
			particleSys.Play();

			effObj.transform.position = effData.pos;

			effObj.Invoke("Delete_Effect", delete_time);
		}

		public void Update_Effect(EffectObject effObj, EffData effData)
		{
			//업데이트 없음
		}

		public void Delete_Effect()
		{
			//자동 삭제
		}
	}
}