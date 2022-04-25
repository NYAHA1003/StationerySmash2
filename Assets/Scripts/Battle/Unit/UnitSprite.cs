using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill;
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
    private GameObject _delayBar;
    [SerializeField]
    private GameObject _delayRotate;
    [SerializeField]
    private GameObject _delayPart;
    [SerializeField]
    private GameObject _delayMask;
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
        _throwSpriteRenderer.sprite = sprite;
        _spriteMask.sprite = sprite;
    }

    /// <summary>
    /// ���� �ε����� ���� ���̾� ���� ���ϱ�
    /// </summary>
    public void OrderDraw(int orderIndex)
    {
        _spriteRenderer.sortingOrder = -orderIndex;
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
        _delayPart.SetActive(false);
        if(_eTeam == TeamType.MyTeam)
        {
            _delayPart.transform.rotation = Quaternion.Euler(0, 0, 180);
            _delayMask.transform.rotation = Quaternion.identity;
            _delayRotate.transform.rotation = Quaternion.identity;
            _delayBar.transform.localPosition = new Vector2(-0.1f, 0);
        }
        else if(_eTeam == TeamType.EnemyTeam)
        {
            _delayPart.transform.rotation = Quaternion.identity;
            _delayMask.transform.rotation = Quaternion.Euler(0, 0, 180);
            _delayRotate.transform.rotation = Quaternion.Euler(0, 0, 180);
            _delayBar.transform.localPosition = new Vector2(0.1f, 0);
        }
    }

    /// <summary>
    /// �����̹� ������Ʈ
    /// </summary>
    /// <param name="delay"></param>
    public void UpdateDelayBar(float delay)
    {
        if(_eTeam == TeamType.MyTeam)
        {
           _delayRotate.transform.rotation = Quaternion.Euler(0, 0, delay * 360);
        }
        else if(_eTeam == TeamType.EnemyTeam)
        {
            _delayRotate.transform.rotation = Quaternion.Euler(0, 0, (-delay * 360) + 180);
        }

        if(delay >= 0.5f)
        {
            _delayPart.SetActive(true);
            _delayMask.SetActive(false);
        }
        else
        {
            _delayPart.SetActive(false);
            _delayMask.SetActive(true);
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
