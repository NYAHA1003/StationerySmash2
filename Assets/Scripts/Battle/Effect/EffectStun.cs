using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using Battle;
using Battle.Effect;

public class EffectStun : IEffect
{
    private float angle = 0, starX = 0, starY = 0, speed = 5f, width = 0.2f, height = 0.05f;

    protected EffectObject effObj;

    [SerializeField]
    private float delete_time = 0.5f;
    public void Set_Effect(EffectObject effObj, EffData effData)
    {
        if (effData.trm == null)
        {
            throw new System.Exception("EffData가 이상함");
        }

        this.effObj ??= effObj;

        this.effObj.CancelInvoke();

        delete_time = effData.lifeTime;

        this.effObj.gameObject.SetActive(true);

        this.effObj.transform.position = effData.pos;

        this.effObj.Invoke("Delete_Effect", delete_time);
    }

    public virtual void Update_Effect(EffectObject effObj, EffData effData)
    {
        angle += Time.deltaTime * speed;
        starX = Mathf.Cos(angle) * width + effData.trm.position.x;
        starY = Mathf.Sin(angle) * height + effData.trm.position.y;
        this.effObj.transform.position = new Vector3(starX, starY, 0);
    }
    public void Delete_Effect()
    {
        this.effObj.Delete_Effect();
    }
}
