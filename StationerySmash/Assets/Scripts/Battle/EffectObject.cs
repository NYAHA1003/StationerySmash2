using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectObject : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem particleSys;
    [SerializeField]
    private float delete_time = 0.5f;
    [SerializeField]
    private float default_LifeTime;

    /// <summary>
    /// 이펙트 리셋 및 설정
    /// </summary>
    /// <param name="pos">이펙트 위치</param>
    /// <param name="startLifeTime">이펙트 유지시간</param>
    /// <param name="isSetLifeTime">이펙트를 유지시간을 바꿀 것인지</param>
    public void Set_Effect(Vector2 pos, float startLifeTime = 0f, bool isSetLifeTime = false)
    {
        CancelInvoke();


        gameObject.SetActive(true);
        
        //재생 시간 설정
        var main = particleSys.main;
        main.startLifetime = isSetLifeTime ? startLifeTime : default_LifeTime;
        
        particleSys.Stop();
        particleSys.Play();

        transform.position = pos;
        
        Invoke("Delete_Effect", delete_time);
    }

    private void Delete_Effect()
    {
        gameObject.SetActive(false);
    }
}
