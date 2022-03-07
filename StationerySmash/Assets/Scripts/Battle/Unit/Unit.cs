using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill;

public abstract class Unit : MonoBehaviour 
{
    public UnitState unitState { get; protected set; }

    [SerializeField]
    protected SpriteRenderer spr;


    protected bool isSettingEnd;

    public int myDamagedId = 0;
    public int damageCount = 0;
    public int myUnitId;
    public int hp { get; protected set; } = 100;
    public int weight { get; protected set; }
    public int maxhp { get; protected set; }
    public bool isInvincibility { get; protected set; }

    public TeamType eTeam;

    public BattleManager battleManager { get; protected set; }


    #region 기본 로직

    /// <summary>
    /// 유닛 생성
    /// </summary>
    /// <param name="unitData">유닛 데이터</param>
    /// <param name="eTeam">팀 변수</param>
    /// <param name="battleManager">배틀매니저</param>
    /// <param name="id"></param>
    public virtual void Set_UnitData(DataBase unitData, TeamType eTeam, BattleManager battleManager, int id)
    {
        //팀, 이름 설정
        this.eTeam = eTeam;
        transform.name = unitData.unitName + this.eTeam;
        switch (this.eTeam)
        {
            case TeamType.Null:
                throw new System.Exception("팀 에러");
            case TeamType.MyTeam:
                spr.color = Color.red;
                break;
            case TeamType.EnemyTeam:
                spr.color = Color.blue;
                break;
        }
        
        
        spr.sprite = unitData.sprite;
        this.battleManager = battleManager;
        maxhp = unitData.hp;
        hp = unitData.hp;
        weight = unitData.weight;
        this.myUnitId = id;

        isSettingEnd = true;
    }

    /// <summary>
    /// 유닛 스테이트 로직 실행
    /// </summary>
    protected virtual void Update()
    {
        if (!isSettingEnd) return;

        unitState = unitState.Process();

    }

    /// <summary>
    /// 유닛 사망시 삭제
    /// </summary>
    public virtual void Delete_Unit()
    {
        battleManager.Pool_DeleteUnit(this);

        if (eTeam == TeamType.MyTeam)
        {
            battleManager.unit_MyDatasTemp.Remove(this);
            return;
        }
        battleManager.unit_EnemyDatasTemp.Remove(this);
    }
    #endregion

    #region 무조건 재정의 해야함
    /// <summary>
    /// 공격 맞음
    /// </summary>
    /// <param name="atkData">공격 데이터</param>
    public abstract void Run_Damaged(AtkData atkData);

    #endregion

    #region 던지기 시스템

    /// <summary>
    /// 당길 유닛을 선택했을 때
    /// </summary>
    /// <returns></returns>
    public virtual Unit Pull_Unit()
    {
        //당기 유닛 선택
        return null;
    }

    /// <summary>
    /// 유닛을 당기고 있을 때
    /// </summary>
    /// <returns></returns>
    public virtual Unit Pulling_Unit()
    {
        //유닛 당기는 중
        return null;
    }

    /// <summary>
    /// 유닛을 던졌을 때
    /// </summary>
    public virtual void Throw_Unit()
    {

    }

    #endregion

    /// <summary>
    /// 무적 여부 설정
    /// </summary>
    /// <param name="isboolean">True면 무적, False면 비무적</param>
    public void Set_IsInvincibility(bool isboolean)
    {
        isInvincibility = isboolean;
    }

    /// <summary>
    /// 체력 감소
    /// </summary>
    /// <param name="damage">줄어들 체력</param>
    public virtual void Subtract_HP(int damage)
    {
        hp -= damage;
    }
}
