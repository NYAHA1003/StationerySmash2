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
/// 유닛 스프라이트 컴포넌트
/// </summary>
public class UnitSprite
{

    //프로퍼티
    public SpriteRenderer SpriteRenderer => _spriteRenderer; // 스프라이트렌더러 프로퍼티

    //인스펙터 참조 변수
    [SerializeField]
    private SortingGroup _delayBarSortingGroup;
    [SerializeField]
    private Animator _animator;
    //인스펙터 참조 변수
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
    private SpriteRenderer _spriteRenderer = null; //유닛 스프라이트렌더러
    [SerializeField]
    private SpriteRenderer _shadowSpriteRenderer = null; //유닛 그림자 스프라이트렌더러
    [SerializeField]
    private SpriteRenderer _hpSpriteRenderer = null; //유닛 체력 스프라이트
    [SerializeField]
    private Transform _starTransform = null; //별 트랜스폼
    [SerializeField]
    private Sprite[] _delaySprites = null; // 유닛 딜레이바이미지들

    private TeamType _eTeam = TeamType.Null;

    public void ResetSprite(TeamType teamType, CardData cardData, UnitStat unitStat, int orderIndex, int grade)
    {
        SetUIAndSprite(teamType, SkinData.GetSkin(cardData._skinData._skinType), grade);
        UpdateDelayBar(unitStat.AttackDelay);
        ShowUI(true);
        SetTeamColor(teamType);
        SetHPSprite(unitStat.Hp, unitStat.MaxHp);
        OrderDraw(orderIndex);
        _animator.runtimeAnimatorController = AnimationData.GetAnimator(cardData._skinData._skinType);
    }

    /// <summary>
    /// 초기 스프라이트 및 유닛 UI 설정
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
        _starTransform.DOScale(Vector3.one * 0.12f, 1f).SetEase(Ease.OutQuad).SetLoops(-1, LoopType.Yoyo);

        switch (grade)
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
    /// 오더 인덱스에 따른 레이어 순서 정하기
    /// </summary>
    public void OrderDraw(int orderIndex)
    {
        _spriteRenderer.sortingOrder = -orderIndex;
        _delayBarSortingGroup.sortingOrder = -orderIndex;
        _delayHalfBarSortingGroup.sortingOrder = -orderIndex;
    }

    /// <summary>
    /// 체력 비율에 따른 깨짐 이미지
    /// </summary>
    public void SetHPSprite(int hp, int maxhp)
    {
        float percent = (float)(maxhp - hp) / maxhp;

        Color color = new Color(1,0,0, percent);

        _hpSpriteRenderer.color = color;
    }

    /// <summary>
    /// 딜레이바 설정
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
    /// 딜레이바 업데이트
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
    /// 딜레이바 키기 끄기
    /// </summary>
    /// <param name="isShow">True면 캔버스 키기 아니면 끄기</param>
    public void ShowUI(bool isShow)
    {
        _delayBar.SetActive(isShow);
        _delayHalfBar.SetActive(false);
    }

    /// <summary>
    /// 팀 설정에 따른 색깔 설정
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
    /// 머테리얼을 변경한다
    /// </summary>
    public void ChangeMaterial(Material material)
	{
        _spriteRenderer.material = material;
	}

    /// <summary>
    /// 던지기가 가능 여부에 따른 이미지를 껐다 킨다
    /// </summary>
    public void SetThrowImage(bool isActive)
	{
        _starTransform.gameObject.SetActive(isActive);
	}
}
