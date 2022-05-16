using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Utill.Data;
using Utill.Tool;
using Battle;

[System.Serializable]
/// <summary>
/// ���� ��������Ʈ ������Ʈ
/// </summary>
public class UnitSprite
{

    //������Ƽ
    public SpriteRenderer SpriteRenderer => _spriteRenderer; // ��������Ʈ������ ������Ƽ

    //�ν����� ���� ����
    [SerializeField]
    private SortingGroup _sortingGroup;
    [SerializeField]
    private GameObject _delayBar;
    [SerializeField]
    private SpriteRenderer _delayRotate;
    [SerializeField]
    private SpriteRenderer _delayPart;
    [SerializeField]
    private SpriteMask _delayMask;
    [SerializeField]
    private SpriteMask _spriteMask = null; //���� ����ũ
    [SerializeField]
    private SpriteRenderer _spriteRenderer = null; //���� ��������Ʈ������
    [SerializeField]
    private SpriteRenderer _hpSpriteRenderer = null; //���� �����̹��� ������
    [SerializeField]
    private SpriteRenderer _throwSpriteRenderer = null; //���� ������ ���� ������
    [SerializeField]
    private Sprite[] _hpSprites = null; // ���� �����̹�����

    private TeamType _eTeam = TeamType.Null;

    /// <summary>
    /// �ʱ� ��������Ʈ �� ���� UI ����
    /// </summary>
    /// <param name="eTeam"></param>
    /// <param name="sprite"></param>
    public void SetUIAndSprite(TeamType eTeam, Sprite sprite)
    {
        _eTeam = eTeam;
        SetDelayBar();
        
        _spriteRenderer.sprite = sprite;
        _spriteMask.sprite = sprite;
    }

    /// <summary>
    /// ���� �ε����� ���� ���̾� ���� ���ϱ�
    /// </summary>
    public void OrderDraw(int orderIndex)
    {
        _spriteRenderer.sortingOrder = -orderIndex;
        _sortingGroup.sortingOrder = -orderIndex;
    }

    /// <summary>
    /// ������ ������ Ű�� ����
    /// </summary>
    /// <param name="isActive"></param>
    public void SetThrowRenderer(bool isActive)
    {
        _throwSpriteRenderer.gameObject.SetActive(isActive);
    }

    /// <summary>
    /// ü�� ������ ���� ���� �̹���
    /// </summary>
    public void Set_HPSprite(int hp, int maxhp)
    {
        float percent = (float)hp / maxhp;

        if (percent > 0.5f)
        {
            _hpSpriteRenderer.sprite = null;
        }
        else if (percent > 0.2f)
        {
            _hpSpriteRenderer.sprite = _hpSprites[0];
        }
        else
        {
            _hpSpriteRenderer.sprite = _hpSprites[1];
        }
    }

    /// <summary>
    /// �����̹� ����
    /// </summary>
    public void SetDelayBar()
    {
        _delayPart.gameObject.SetActive(false);
        _delayPart.transform.rotation = Quaternion.Euler(0, 0, 180);
        _delayMask.transform.rotation = Quaternion.identity;
        _delayRotate.transform.rotation = Quaternion.identity;
    }

    /// <summary>
    /// �����̹� ������Ʈ
    /// </summary>
    /// <param name="delay"></param>
    public void UpdateDelayBar(float delay)
    {
        _delayRotate.transform.rotation = Quaternion.Euler(0, 0, delay * 360);

        if (delay >= 0.5f)
        {
            _delayPart.gameObject.SetActive(true);
            _delayMask.gameObject.SetActive(false);
        }
        else
        {
            _delayPart.gameObject.SetActive(false);
            _delayMask.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// �����̹� Ű�� ����
    /// </summary>
    /// <param name="isShow">True�� ĵ���� Ű�� �ƴϸ� ����</param>
    public void ShowUI(bool isShow)
    {
        _delayBar.SetActive(isShow);
    }

    /// <summary>
    /// �� ������ ���� ���� ����
    /// </summary>
    /// <param name="eTeam"></param>
    public void SetTeamColor(TeamType eTeam)
    {
        switch (eTeam)
        {
            case TeamType.Null:
                _spriteRenderer.color = Color.white;
                break;
            case TeamType.MyTeam:
                _spriteRenderer.color = Color.red;
                break;
            case TeamType.EnemyTeam:
                _spriteRenderer.color = Color.blue;
                break;
        }
    }
}
