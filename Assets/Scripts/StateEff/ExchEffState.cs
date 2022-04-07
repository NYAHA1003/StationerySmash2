using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using DG.Tweening;

public class ExchEffState : EffState
{
    private bool isAddEff = false;
    private float _exchOneTime = 0.0f; // ��ȯ ù��° �ð�
    private float _exchTwoTime = 0.0f; // ��ȯ �ι��� �ð�
    private float attackSubtractPercent = 0; //���ݷ� �ۼ�Ʈ�� �󸶳� Ŀ���� �پ����ΰ�
    private float moveSpeedSubtractPercent = 0;//�̵��ӵ� �ۼ�Ʈ�� �󸶳� Ŀ���� �پ����ΰ�

    public override void Enter()
    {
        SprTrm.GetComponent<SpriteRenderer>().color = Color.green;
        _myUnit.UnitStat.IncreaseAttackPercent((int)attackSubtractPercent);
        _myUnit.UnitStat.IncreaseMoveSpeedPercent((int)moveSpeedSubtractPercent);

        //����Ʈ ������Ʈ ��������
        //_effectObj = _battleManager.CommandEffect.SetEffect(EffectType.Stun, new EffData(new Vector2(Trm.position.x, Trm.position.y + 0.1f), stunTime, Trm));

        base.Enter();
    }

    public override void Update()
    {
        if(!ExchOneTimer())
        {
            return;
        }
        if(!isAddEff)
        {
            _myUnit.UnitStat.IncreaseAttackPercent(-(int)attackSubtractPercent * 2);
            _myUnit.UnitStat.IncreaseMoveSpeedPercent(-(int)moveSpeedSubtractPercent * 2);
            isAddEff = true;
        }
        if (!ExchTwoTimer())
        {
            return;
        }
        _curEvent = eEvent.EXIT;
    }

    public override void Exit()
    {
        _myUnit.UnitStat.IncreaseAttackPercent((int)attackSubtractPercent);
        _myUnit.UnitStat.IncreaseMoveSpeedPercent((int)moveSpeedSubtractPercent);
        DeleteEffectObject();
        base.Exit();
    }

    public override void SetEffValue(params float[] value)
    {
        _exchOneTime = value[0];
        _exchTwoTime = value[0];
        attackSubtractPercent = value[1];
        moveSpeedSubtractPercent = value[2];
    }


    /// <summary>
    /// ��ȯ 1�ܰ� ���� �ð�
    /// </summary>
    private bool ExchOneTimer()
    {
        if (_exchOneTime > 0)
        {
            _exchOneTime -= Time.deltaTime;
            return false;
        }
        return true;
    }

    /// <summary>
    /// ��ȯ 2�ܰ� ���� �ð�
    /// </summary>
    private bool ExchTwoTimer()
    {
        if (_exchTwoTime > 0)
        {
            _exchTwoTime -= Time.deltaTime;
            return false;
        }
        return true;
    }

}
