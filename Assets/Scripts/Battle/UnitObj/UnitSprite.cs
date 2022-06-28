using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Utill.Data;
using Utill.Tool;
using DG.Tweening;
using Battle;
using UnityEngine.ResourceManagement.AsyncOperations;

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
    [SerializeField]
    private Animator _animator;
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
    private SpriteRenderer _spriteRenderer = null; //���� ��������Ʈ������
    [SerializeField]
    private SpriteRenderer _shadowSpriteRenderer = null; //���� �׸��� ��������Ʈ������
    [SerializeField]
    private SpriteRenderer _hpSpriteRenderer = null; //���� ü�� ��������Ʈ
    [SerializeField]
    private Sprite[] _delaySprites = null; // ���� �����̹��̹�����
    [SerializeField]
    private Sprite[] _gradeSprite = null; //���� �ܰ� �̹���
    [SerializeField]
    private SpriteRenderer _gradeSpriteRender = null; //���� �ܰ� �̹���������

    private TeamType _eTeam = TeamType.Null;

    public void ResetSprite(Unit unit, CardType cardType, TeamType teamType, CardData cardData, UnitStat unitStat, int orderIndex, int grade)
    {
        Vector2 pos = _gradeSpriteRender.transform.position;
        pos.y = unit.CollideData.originpoints[0].y + 0.2f;
        _gradeSpriteRender.transform.position = pos;
        SetUIAndSprite(teamType, SkinData.GetSkin(cardData._skinData._skinType), grade);
        if(cardType == CardType.AttackProjectile)
        {
            ShowGradeUI(false);
        }
        else
        {
            ShowGradeUI(true);
        }
        UpdateDelayBar(unitStat.AttackDelay);
        SetTeamColor(teamType);
        SetHPSprite(unitStat.Hp, unitStat.MaxHp);
        OrderDraw(orderIndex);
        _animator.runtimeAnimatorController = AnimationData.GetAnimator(cardData._skinData._skinType);
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
        _shadowSpriteRenderer.sprite = sprite;
        _hpSpriteRenderer.sprite = sprite;

        switch (grade)
		{
            case 1:
                _gradeSpriteRender.sprite = _gradeSprite[0];
                _delayBarImage.sprite = _delaySprites[0];
                _delayHalfBarImage.sprite = _delaySprites[0];
                break;
            case 2:
                _gradeSpriteRender.sprite = _gradeSprite[1];
                _delayBarImage.sprite = _delaySprites[1];
                _delayHalfBarImage.sprite = _delaySprites[1];
                break;
            case 3:
                _gradeSpriteRender.sprite = _gradeSprite[2];
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
        float percent = (float)(maxhp - hp) / maxhp;

        Color color = new Color(1,0,0, percent);

        _hpSpriteRenderer.color = color;
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
        if(!isShow)
		{
            ShowGradeUI(false);
		}
    }

    /// <summary>
    /// �ܰ� UI ǥ��
    /// </summary>
    /// <param name="isShow"></param>
    public void ShowGradeUI(bool isShow)
	{
        _gradeSpriteRender.gameObject.SetActive(isShow);
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
