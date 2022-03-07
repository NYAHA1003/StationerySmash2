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
    /// ����Ʈ ���� �� ����
    /// </summary>
    /// <param name="pos">����Ʈ ��ġ</param>
    /// <param name="startLifeTime">����Ʈ �����ð�</param>
    /// <param name="isSetLifeTime">����Ʈ�� �����ð��� �ٲ� ������</param>
    public void Set_Effect(Vector2 pos, float startLifeTime = 0f, bool isSetLifeTime = false)
    {
        CancelInvoke();


        gameObject.SetActive(true);
        
        //��� �ð� ����
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
