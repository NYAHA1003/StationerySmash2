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
        //������ ����
        _myUnit = myUnit;
        _unitData = _myUnit.UnitData;
        _stickerData = _unitData.stickerData;

        //��ƼĿ �ɷ� ��������
        SetStickerAbility();

        //��ġ ����
        //SetPosition();
    }

    /// <summary>
    /// �����ε����� ���� ���̾� ����
    /// </summary>
    public void OrderDraw(int orderIndex)
    {
        if(_stickerSprite != null)
        { 
            _stickerSprite.sortingOrder = -orderIndex ;
        }
    }

    /// <summary>
    /// ��ƼĿ �ɷ� ���
    /// </summary>
    /// <param name="eState"></param>
    public void RunStickerAbility(eState eState)
    {
        _stickerablity?.RunStickerAblity(eState);
    }

    /// <summary>
    /// ��ƼĿ �ɷ� �ݳ�
    /// </summary>
    public void DeleteSticekr()
    {
        if(_stickerablity != null)
        {
            PoolManager.AddSticker(_stickerablity);
        }
    }

    /// <summary>
    /// ��ƼĿ �ɷ� ��������
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
    /// ��ƼĿ ��ġ ����
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
