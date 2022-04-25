using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill;
using Battle;

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
    private GameObject _delayBar;
    [SerializeField]
    private GameObject _delayRotate;
    [SerializeField]
    private GameObject _delayPart;
    [SerializeField]
    private GameObject _delayMask;
    [SerializeField]
    private SpriteMask _spriteMask = null; //유닛 마스크
    [SerializeField]
    private SpriteRenderer _spriteRenderer = null; //유닛 스프라이트렌더러
    [SerializeField]
    private SpriteRenderer _hpSpriteRenderer = null; //유닛 깨짐이미지 렌더러
    [SerializeField]
    private SpriteRenderer _throwSpriteRenderer = null; //유닛 던지기 가능 렌더러
    [SerializeField]
    private Sprite[] _hpSprites = null; // 유닛 깨짐이미지들

    private TeamType _eTeam = TeamType.Null;

    /// <summary>
    /// 초기 스프라이트 및 유닛 UI 설정
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
    /// 오더 인덱스에 따른 레이어 순서 정하기
    /// </summary>
    public void OrderDraw(int orderIndex)
    {
        _spriteRenderer.sortingOrder = -orderIndex;
    }

    /// <summary>
    /// 던지기 렌더러 키기 끄기
    /// </summary>
    /// <param name="isActive"></param>
    public void SetThrowRenderer(bool isActive)
    {
        _throwSpriteRenderer.gameObject.SetActive(isActive);
    }

    /// <summary>
    /// 체력 비율에 따른 깨짐 이미지
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
    /// 딜레이바 설정
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
    /// 딜레이바 업데이트
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
    /// 딜레이바 키기 끄기
    /// </summary>
    /// <param name="isShow">True면 캔버스 키기 아니면 끄기</param>
    public void ShowUI(bool isShow)
    {
        _delayBar.SetActive(isShow);
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
