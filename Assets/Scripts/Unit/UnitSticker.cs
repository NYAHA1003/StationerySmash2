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

        //능력 사용
        _stickerablity?.RunStickerAblity();
    }

    /// <summary>
    /// 스티커 능력 반납
    /// </summary>
    public void DeleteSticekr()
    {
        PoolManager.AddSticker(_stickerablity);
    }

    /// <summary>
    /// 스티커 능력 가져오기
    /// </summary>
    private void SetStickerAbility()
    {
        switch (_unitData.stickerData.stickerType)
        {
            case StickerType.None:
                _stickerablity = null;
                return;
            case StickerType.Rock:
                _stickerablity = PoolManager.GetSticker<RockSticker>();
                break;
            case StickerType.Paper:
                break;
            case StickerType.Scissors:
                break;
        }
        _stickerablity.SetUnit(_myUnit);
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
