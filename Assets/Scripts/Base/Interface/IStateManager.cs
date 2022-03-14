using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public interface IStateManager
{
    /// <summary>
    /// ���¸� ������
    /// </summary>
    public void Set_State(Transform myTrm, Transform mySprTrm, Unit myUnit);

    /// <summary>
    /// ���� ���¸� ������
    /// </summary>
    public void Reset_CurrentUnitState(UnitState unitState);

    /// <summary>
    /// ���� ���¸� ��ȯ��
    /// </summary>
    /// <returns></returns>
    public UnitState Return_CurrentUnitState();

    /// <summary>
    /// ���¸� �ٽ� ������
    /// </summary>
    /// <param name="unit"></param>
    public void Reset_State(Transform myTrm, Transform mySprTrm, Unit myUnit);
    /// <summary>
    /// ���� ���¸� Idle�� ������
    /// </summary>
    public void Set_Idle();
    /// <summary>
    /// ���� ���¸� Wiat�� ������
    /// </summary>
    public void Set_Wait(float time);
    /// <summary>
    /// Wiat�� �߰� ���� �ð��� ������
    /// </summary>
    public void Set_WaitExtraTime(float extraTime);
    /// <summary>
    /// ���� ���¸� Move�� ������
    /// </summary>
    public void Set_Move();
    /// <summary>
    /// ���� ���¸� Damaged�� ������
    /// </summary>
    public void Set_Damaged(AtkData atkData);
    /// <summary>
    /// ���� ���¸� Attack�� ������
    /// </summary>
    public void Set_Attack(Unit targetUnit);
    /// <summary>
    /// ���� ���¸� Die�� ������
    /// </summary>
    public void Set_Die();
    /// <summary>
    /// ���� ���¸� Throw�� ������
    /// </summary>
    public void Set_Throw();
    /// <summary>
    /// Throw�� ���� ��ġ�� ������
    /// </summary>
    /// <param name="pos"></param>
    public void Set_ThrowPos(Vector2 pos);


}
