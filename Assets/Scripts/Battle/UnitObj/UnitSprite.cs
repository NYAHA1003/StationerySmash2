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
    private SortingGroup _delayBarSortingGroup;
    //�ν����� ���� ����
    [SerializeField]
    private SortingGroup _delayHalfBarSortingGroup;
    [SerializeField]
    private GameObject _delayBar;
    [SerializeField]
    private GameObject _delayHalfBar;
    [SerializeField]
    private SpriteRenderer _delayHalfBarImage;
    [SerializeField]
    private SpriteRenderer _delayBarImage;
    [SerializeField]
    private SpriteMask _delayMaskPart;
    [SerializeField]
    private SpriteMask _delayMask;
    [SerializeField]
    private SpriteMask _spriteMask = null; //���� ����ũ
    [SerializeField]
    private SpriteRenderer _spriteRenderer = null; //���� ��������Ʈ������
    [SerializeField]
    private SpriteRenderer _hpSpriteRenderer = null; //���� �����̹��� ������
    [SerializeField]
    private Sprite[] _hpSprites = null; // ���� �����̹�����
    [SerializeField]
    private Sprite[] _delaySprites = null; // ���� �����̹��̹�����

    private TeamType _eTeam = TeamType.Null;

    public void ResetSprite(TeamType teamType, CardData cardData, UnitStat unitStat, int orderIndex, int grade)
    {
        SetUIAndSprite(teamType, SkinData.GetSkin(cardData._skinData._skinType), grade);
        UpdateDelayBar(unitStat.AttackDelay);
        ShowUI(true);
        SetTeamColor(teamType);
        SetHPSprite(unitStat.Hp, unitStat.MaxHp);
        OrderDraw(orderIndex);
    }

    /// <summary>
    /// �ʱ� ��������Ʈ �� ���� UI ����
    /// </summary>
    /// <param name="eTeam"></param>
    /// <param name="sprite"></param>
    public void SetUIAndSprite(TeamType eTeam, Sprite sprite, int grade)
    {
        _eTeam = eTeam;
        SetDelayBar();
        
        _spriteRenderer.sprite = sprite;
        _spriteMask.sprite = sprite;

        switch(grade)
		{
            case 1:
                _delayBarImage.sprite = _delaySprites[0];
                _delayHalfBarImage.sprite = _delaySprites[0];
                break;
            case 2:
                _delayBarImage.sprite = _delaySprites[1];
                _delayHalfBarImage.sprite = _delaySprites[1];
                break;
            case 3:
                _delayBarImage.sprite = _delaySprites[2];
                _delayHalfBarImage.sprite = _delaySprites[2];
                break;
        }
    }

    /// <summary>
    /// ���� �ε����� ���� ���̾� ���� ���ϱ�
    /// </summary>
    public void OrderDraw(int orderIndex)
    {
        _spriteRenderer.sortingOrder = -orderIndex;
        _delayBarSortingGroup.sortingOrder = -orderIndex;
        _delayHalfBarSortingGroup.sortingOrder = -orderIndex;
    }

    /// <summary>
    /// ü�� ������ ���� ���� �̹���
    /// </summary>
    public void SetHPSprite(int hp, int maxhp)
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
        _delayMaskPart.gameObject.SetActive(true);
        _delayMaskPart.transform.rotation = Quaternion.Euler(0, 0, 180);
        _delayMask.transform.rotation = Quaternion.identity;
        _delayBarImage.transform.rotation = Quaternion.identity;
        _delayHalfBar.SetActive(false);
    }

    /// <summary>
    /// �����̹� ������Ʈ
    /// </summary>
    /// <param name="delay"></param>
    public void UpdateDelayBar(float delay)
    {
        _delayMask.transform.rotation = Quaternion.Euler(0, 0, delay * 360);

        if (delay >= 0.5f)
        {
            _delayMaskPart.gameObject.SetActive(false);
            _delayMask.gameObject.SetActive(true);
            _delayHalfBar.SetActive(true);
        }
        else
        {
            _delayMaskPart.gameObject.SetActive(true);
            _delayMask.gameObject.SetActive(true);
            _delayHalfBar.SetActive(false);
        }
    }

    /// <summary>
    /// �����̹� Ű�� ����
    /// </summary>
    /// <param name="isShow">True�� ĵ���� Ű�� �ƴϸ� ����</param>
    public void ShowUI(bool isShow)
    {
        _delayBar.SetActive(isShow);
        _delayHalfBar.SetActive(false);
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
                _spriteRenderer.material = ShaderData.GetShader(ShaderType.DefaultShader);
                break;
            case TeamType.MyTeam:
                _spriteRenderer.material = ShaderData.GetShader(ShaderType.MyTeamNormalShader);
                break;
            case TeamType.EnemyTeam:
                _spriteRenderer.material = ShaderData.GetShader(ShaderType.EnemyTeamNormalShader);
                break;
        }
    }

    /// <summary>
    /// ���׸����� �����Ѵ�
    /// </summary>
    public void ChangeMaterial(Material material)
	{
        _spriteRenderer.material = material;
	}
}
