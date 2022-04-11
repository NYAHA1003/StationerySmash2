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
    private Canvas _canvas = null; //������ ĵ����
    [SerializeField]
    private Image _delayBar = null; //�����̹�
    [SerializeField]
    private SpriteMask _spriteMask = null; //���� ����ũ
    [SerializeField]
    private SpriteRenderer _spriteRenderer = null; //���� ��������Ʈ������
    [SerializeField]
    private SpriteRenderer _hpSpriteRenderer = null; //���� �����̹��� ������
    [SerializeField]
    private Sprite[] _hpSprites = null; // ���� �����̹�����

    /// <summary>
    /// �ʱ� ��������Ʈ �� ���� UI ����
    /// </summary>
    /// <param name="eTeam"></param>
    /// <param name="sprite"></param>
    public void SetUIAndSprite(TeamType eTeam, Sprite sprite)
    {
        _canvas.worldCamera = Camera.main;
        _delayBar.rectTransform.anchoredPosition = eTeam.Equals(TeamType.MyTeam) ? new Vector2(-960.15f, -540.15f) : new Vector2(-959.85f, -540.15f);
        _spriteRenderer.sprite = sprite;
        _spriteMask.sprite = sprite;
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
    /// �����̹� ������Ʈ
    /// </summary>
    /// <param name="delay"></param>
    public void UpdateDelayBar(float delay)
    {
        _delayBar.fillAmount = delay;
    }

    /// <summary>
    /// ĵ���� Ű�� ����
    /// </summary>
    /// <param name="isShow">True�� ĵ���� Ű�� �ƴϸ� ����</param>
    public void ShowCanvas(bool isShow)
    {
        _canvas.gameObject.SetActive(isShow);
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
