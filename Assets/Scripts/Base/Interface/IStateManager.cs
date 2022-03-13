using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public interface IStateManager
{

    public void Reset_CurrentUnitState();
    public void Set_State();
    public void Reset_State(UnitState unit);
    public void Return_Idle();
    public void Return_Wait(float time);
    public void Set_WaitExtraTime(float extraTime);
    public void Return_Move();
    public void Return_Damaged(AtkData atkData);
    public void Return_Attack(Unit targetUnit);
    public void Return_Die();
    public void Return_Throw();
    
}
