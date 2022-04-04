using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill;
using Battle;

[System.Serializable]
/// <summary>
/// ������ ���� ������Ʈ
/// </summary>
public class UnitSprite
{
    //�ν����� ���� ����
    [SerializeField]
    private Canvas _canvas;
    [SerializeField]
    private Image _delayBar;
    [SerializeField]
    private SpriteMask _spriteMask;
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private SpriteRenderer _hpSpriteRenderer;
    [SerializeField]
    private Sprite[] _hpSprites;


    public SpriteRenderer SpriteRenderer => _spriteRenderer;

    /// <summary>
    /// �ʱ� ��������Ʈ �� ���� UI ����
    /// </summary>
    /// <param name="eTeam"></param>
    /// <param name="sprite"></param>
    public void SetUIAndSprite(TeamType eTeam, Sprite sprite)
    {
        _canvas.worldCamera = Camera.main;
        _delayBar.rectTransform.anchoredPosition = eTeam.Equals(TeamType.MyTeam) ? new Vector2(-960.15f, -540.15f) : new Vector2(-959.85f, -540.15f);
        this._spriteRenderer.sprite = sprite;
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
    public void Update_DelayBar(float delay)
    {
        _delayBar.fillAmount = delay;
    }


    /// <summary>
    /// ĵ���� Ű�� ����
    /// </summary>
    /// <param name="isShow">True�� ĵ���� Ű�� �ƴϸ� ����</param>
    public void Show_Canvas(bool isShow)
    {
        _canvas.gameObject.SetActive(isShow);
    }


    /// <summary>
    /// �� ����
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
