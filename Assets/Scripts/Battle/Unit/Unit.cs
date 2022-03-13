using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill;

public abstract class Unit : MonoBehaviour
{
    public UnitData unitData;
    public UnitState unitState { get; protected set; }

    [SerializeField]
    protected SpriteRenderer spr;
    
    public TeamType eTeam;


    public int myDamagedId = 0;
    public int damageCount = 0;
    public int myUnitId;
    public int hp { get; protected set; }
    public int maxhp { get; protected set; }
    public int weight { get; protected set; }

    public int damagePercent = 100;
    public int moveSpeedPercent = 100;
    public int attackSpeedPercent = 100;
    public int rangePercent = 100;
    public int accuracyPercent = 100;
    public int weightPercent = 100;
    public int knockbackPercent = 100;
    public bool isInvincibility { get; protected set; }
    public bool isDontThrow { get; protected set; }

    protected bool isSettingEnd;

    public BattleManager battleManager { get; protected set; }
    
    protected StageData stageData;

    #region 기본 로직

    /// <summary>
    /// 유닛 생성
    /// </summary>
    /// <param name="dataBase">유닛 데이터</param>
    /// <param name="eTeam">팀 변수</param>
    /// <param name="battleManager">배틀매니저</param>
    /// <param name="id"></param>
    public virtual void Set_UnitData(DataBase dataBase, TeamType eTeam, BattleManager battleManager, int id)
    {
        this.isSettingEnd = false;

        //팀, 이름 설정
        this.eTeam = eTeam;
        transform.name = dataBase.card_Name + this.eTeam;

        switch (this.eTeam)
        {
            case TeamType.Null:
                spr.color = Color.white;
                break;
            case TeamType.MyTeam:
                spr.color = Color.red;
                break;
            case TeamType.EnemyTeam:
                spr.color = Color.blue;
                break;
        }
        
        
        this.spr.sprite = dataBase.card_Sprite;
        this.battleManager = battleManager;
        this.stageData = battleManager.currentStageData;
        this.maxhp = dataBase.unitData.unit_Hp;
        this.hp = dataBase.unitData.unit_Hp;
        this.weight = dataBase.unitData.unit_Weight;
        this.myUnitId = id;

        this.isSettingEnd = true;
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

        switch (eTeam)
        {
            case TeamType.Null:
                break;
            case TeamType.MyTeam:
                battleManager.unit_MyDatasTemp.Remove(this);
                break;
            case TeamType.EnemyTeam:
                battleManager.unit_EnemyDatasTemp.Remove(this);
                break;
        }
    }

    #endregion

    #region 무조건 재정의 해야함

    /// <summary>
    /// 공격 맞음
    /// </summary>
    /// <param name="atkData">공격 데이터</param>
    public abstract void Run_Damaged(AtkData atkData);

    /// <summary>
    /// 속성 효과 적용
    /// </summary>
    /// <param name="atkType"></param>
    /// <param name="value"></param>
    public abstract void Add_StatusEffect(AtkType atkType, params float[] value);

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
    public virtual void Set_IsInvincibility(bool isboolean)
    {
        isInvincibility = isboolean;
    }

    /// <summary>
    /// 무적 여부 설정
    /// </summary>
    /// <param name="isboolean">True면 던지기 불가능, False면 던지기 가능</param>
    public virtual void Set_IsDontThrow(bool isboolean)
    {
        isDontThrow = isboolean;
    }

    /// <summary>
    /// 체력 감소
    /// </summary>
    /// <param name="damage">줄어들 체력</param>
    public virtual void Subtract_HP(int damage)
    {
        hp -= damage;
    }

    #region 스탯 반환
    public int Return_Damage()
    {
        return Mathf.RoundToInt(unitData.damage * (float)damagePercent / 100);
    }
    public float Return_MoveSpeed()
    {
        return unitData.moveSpeed * (float)moveSpeedPercent / 100;
    }
    public float Return_AttackSpeed()
    {
        return unitData.attackSpeed * (float)attackSpeedPercent / 100;
    }
    public float Return_Range()
    {
        return unitData.range * (float)rangePercent / 100;
    }
    public int Return_Weight()
    {
        return Mathf.RoundToInt(unitData.unit_Weight * (float)weightPercent / 100);
    }
    public float Return_Accuracy()
    {
        return unitData.accuracy * (float)accuracyPercent / 100;
    }
    public int Return_Knockback()
    {
        return Mathf.RoundToInt(unitData.knockback * (float)knockbackPercent / 100);
    }

    #endregion
}
