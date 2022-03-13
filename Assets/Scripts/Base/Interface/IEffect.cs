using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public interface IEffect
{
    public void Update_Effect(EffectObject effObj, EffData effData);
    public void Set_Effect(EffectObject effObj, EffData effData);
}

public class Effect_Attack : IEffect
{
    [SerializeField]
    private ParticleSystem particleSys;
    [SerializeField]
    private float delete_time = 0.5f;
    public void Set_Effect(EffectObject effObj, EffData effData)
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
}
public class Effect_Stun : IEffect
{
    private float angle = 0, starX = 0, starY = 0, speed = 5f, width = 0.2f, height = 0.05f;
    
    [SerializeField]
    private float delete_time = 0.5f;
    public void Set_Effect(EffectObject effObj, EffData effData)
    {
        if(effData.trm == null)
        {
            throw new System.Exception("EffData가 이상함");
        }
        
        effObj.CancelInvoke();

        delete_time = effData.lifeTime;

        effObj.gameObject.SetActive(true);

        effObj.transform.position = effData.pos;

        effObj.Invoke("Delete_Effect", delete_time);
    }

    public void Update_Effect(EffectObject effObj, EffData effData)
    {
        angle += Time.deltaTime * speed;
        starX = Mathf.Cos(angle) * width + effData.trm.position.x;
        starY = Mathf.Sin(angle) * height + effData.trm.position.y;
        effObj.transform.position = new Vector3(starX, starY, 0);

     
    }
}