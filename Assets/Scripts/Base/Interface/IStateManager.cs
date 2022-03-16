using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public interface IStateManager
{
    /// <summary>
    /// 상태를 설정함
    /// </summary>
    public void Set_State(Transform myTrm, Transform mySprTrm, Unit myUnit);

    /// <summary>
    /// 현재 상태를 지정함
    /// </summary>
    public void Reset_CurrentUnitState(UnitState unitState);

    /// <summary>
    /// 현재 상태를 반환함
    /// </summary>
    /// <returns></returns>
    public UnitState Return_CurrentUnitState();

    /// <summary>
    /// 상태를 다시 설정함
    /// </summary>
    /// <param name="unit"></param>
    public void Reset_State(Transform myTrm, Transform mySprTrm, Unit myUnit);
    /// <summary>
    /// 다음 상태를 Idle로 설정함
    /// </summary>
    public void Set_Idle();
    /// <summary>
    /// 다음 상태를 Wiat로 설정함
    /// </summary>
    public void Set_Wait(float time);
    /// <summary>
    /// Wiat의 추가 정지 시간을 설정함
    /// </summary>
    public void Set_WaitExtraTime(float extraTime);
    /// <summary>
    /// 다음 상태를 Move로 설정함
    /// </summary>
    public void Set_Move();
    /// <summary>
    /// 다음 상태를 Damaged로 설정함
    /// </summary>
    public void Set_Damaged(AtkData atkData);
    /// <summary>
    /// 다음 상태를 Attack로 설정함
    /// </summary>
    public void Set_Attack(Unit targetUnit);
    /// <summary>
    /// 다음 상태를 Die로 설정함
    /// </summary>
    public void Set_Die();
    /// <summary>
    /// 다음 상태를 Throw로 설정함
    /// </summary>
    public void Set_Throw();
    /// <summary>
    /// Throw의 던질 위치를 결정함
    /// </summary>
    /// <param name="pos"></param>
    public void Set_ThrowPos(Vector2 pos);


}
