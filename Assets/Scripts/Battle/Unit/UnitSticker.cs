using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

[System.Serializable]
public class UnitSticker
{
    [SerializeField]
    private SpriteRenderer _stickerSprite = null;

    private UnitData _unitData = null;
    private StickerData _stickerData = null;
    private AbstractSticker _stickerablity = null;
    private Unit _myUnit = null;

    public void SetSticker(Unit myUnit)
    {
        //데이터 설정
        _myUnit = myUnit;
        _unitData = _myUnit.UnitData;
        _stickerData = _unitData.stickerData;

        //스티커 능력 가져오기
        SetStickerAbility();

        //위치 설정
        //SetPosition();
    }

    /// <summary>
    /// 오더인덱스에 따른 레이어 설정
    /// </summary>
    public void OrderDraw(int orderIndex)
    {
        if(_stickerSprite != null)
        { 
            _stickerSprite.sortingOrder = -orderIndex ;
        }
    }

    /// <summary>
    /// 생성 스티커 능력 사용
    /// </summary>
    /// <param name="eState"></param>
    public void RunIdleStickerAbility(eState eState)
    {
        if(_stickerablity == null)
		{
            return;
		}
        if(eState != eState.IDLE)
		{
            return;
		}
        _stickerablity?.RunStickerAblity();
    }
    /// <summary>
    /// 이동 스티커 능력 사용
    /// </summary>
    /// <param name="eState"></param>
    public void RunMoveStickerAbility(eState eState)
    {
        if (_stickerablity == null)
        {
            return;
        }
        if (eState != eState.MOVE)
        {
            return;
        }
        _stickerablity?.RunStickerAblity();
    }
    /// <summary>
    /// 공격 스티커 능력 사용
    /// </summary>
    /// <param name="eState"></param>
    public void RunAttackStickerAbility(eState eState, ref AtkData atkData)
    {
        if (_stickerablity == null)
        {
            return;
        }
        if (eState != eState.ATTACK)
        {
            return;
        }
        (_stickerablity as AbstractAttackSticker).RunAttackStickerAblity(ref atkData);
    }
    /// <summary>
    /// 데미지입음 스티커 능력 사용
    /// </summary>
    /// <param name="eState"></param>
    public void RunDamagedStickerAbility(eState eState, ref AtkData atkData)
    {
        if (_stickerablity == null)
        {
            return;
        }
        if (eState != eState.DAMAGED)
        {
            return;
        }
        (_stickerablity as AbstractDamagedSticker).RunDamagedStickerAblity(ref atkData);
    }
    /// <summary>
    /// 죽음 스티커 능력 사용
    /// </summary>
    /// <param name="eState"></param>
    public void RunDieStickerAbility(eState eState)
    {
        if (_stickerablity == null)
        {
            return;
        }
        if (eState != eState.DIE)
        {
            return;
        }
        _stickerablity?.RunStickerAblity();
    }
    /// <summary>
    /// 대기 스티커 능력 사용
    /// </summary>
    /// <param name="eState"></param>
    public void RunWaitStickerAbility(eState eState)
    {
        if (_stickerablity == null)
        {
            return;
        }
        if (eState != eState.WAIT)
        {
            return;
        }
        _stickerablity?.RunStickerAblity();
    }

    /// <summary>
    /// 던지기 스티커 능력 사용
    /// </summary>
    /// <param name="eState"></param>
    public void RunThrowStickerAbility(eState eState)
    {
        if (_stickerablity == null)
        {
            return;
        }
        if (eState != eState.THROW)
        {
            return;
        }
        _stickerablity?.RunStickerAblity();
    }


    /// <summary>
    /// 스티커 능력 반납
    /// </summary>
    public void DeleteSticekr()
    {
        if(_stickerablity != null)
        {
            PoolManager.AddSticker(_stickerablity);
        }
    }

    /// <summary>
    /// 스티커 능력 가져오기
    /// </summary>
    private void SetStickerAbility()
    {
        switch (_unitData.stickerData._stickerType)
        {
            case StickerType.None:
                _stickerablity = null;
                return;
            case StickerType.Rock:
                _stickerablity = PoolManager.GetSticker<RockSticker>();
                break;
            case StickerType.Paper:
                _stickerablity = PoolManager.GetSticker<PaperSticker>();
                break;
            case StickerType.Scissors:
                _stickerablity = PoolManager.GetSticker<ScissorsPinkingSticker>();
                break;
            case StickerType.Eraser:
                _stickerablity = PoolManager.GetSticker<EraserPieceSticker>();
                break;
            case StickerType.Armor:
                _stickerablity = PoolManager.GetSticker<ArmorSticker>();
                break;
            case StickerType.Run:
                _stickerablity = PoolManager.GetSticker<RunSticker>();
                break;
            case StickerType.Heal:
                _stickerablity = PoolManager.GetSticker<HealSticker>();
                break;
            case StickerType.LongSee:
                _stickerablity = PoolManager.GetSticker<LongSeeSticker>();
                break;
            case StickerType.Heavy:
                _stickerablity = PoolManager.GetSticker<HeavySticker>();
                break;
            case StickerType.Invincible:
                _stickerablity = PoolManager.GetSticker<InvincibleSticker>();
                break;
            case StickerType.PencilNew:
                _stickerablity = PoolManager.GetSticker<PencilNewSticker>();
                break;
            case StickerType.ScissorPinking:
                _stickerablity = PoolManager.GetSticker<ScissorsPinkingSticker>();
                break;
            case StickerType.RustyRuller:
                _stickerablity = PoolManager.GetSticker<RullerRustySticker>();
                break;
            case StickerType.PlasticPen:
                _stickerablity = PoolManager.GetSticker<PenPlasticSticker>();
                break;
        }
        _stickerablity.SetSticker(_myUnit);
    }

    /// <summary>
    /// 스티커 위치 지정
    /// </summary>
    private void SetPosition()
    {
        switch (_unitData.unitType)
        {
            case UnitType.Pencil:
                _stickerSprite.transform.position = Vector2.zero;
                break;
            case UnitType.Sharp:
                break;
            case UnitType.Eraser:
                break;
            case UnitType.BallPen:
                break;
        }
    }
}
