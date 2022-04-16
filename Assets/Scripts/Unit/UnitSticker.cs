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
        _stickerSprite.sortingOrder = -orderIndex;
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
        switch (_unitData.stickerData.stickerType)
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
                _stickerablity = PoolManager.GetSticker<ScissorsSticker>();
                break;
            case StickerType.Eraser:
                _stickerablity = PoolManager.GetSticker<EraserSticker>();
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
