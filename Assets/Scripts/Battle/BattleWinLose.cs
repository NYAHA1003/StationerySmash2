using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BattleWinLose : BattleCommand
{
    private Canvas _winLoseCanvas;
    private RectTransform _winPanel;
    private RectTransform _losePanel;
    private RectTransform _winText;
    private RectTransform _loseText;

    public BattleWinLose(BattleManager battleManager, Canvas winLoseCanvas, RectTransform winPanel, RectTransform losePanel) : base(battleManager)
    {
        this._winLoseCanvas = winLoseCanvas;
        this._winPanel = winPanel;
        this._losePanel = losePanel;

        _winText = winPanel.GetChild(0).GetComponent<RectTransform>();
        _loseText = losePanel.GetChild(0).GetComponent<RectTransform>();
    }

    public void SetWinLosePanel(bool isWin)
    {
        _winLoseCanvas.gameObject.SetActive(true);

        _winPanel.gameObject.SetActive(isWin);
        _losePanel.gameObject.SetActive(!isWin);
        if (isWin)
        {
            _winPanel.sizeDelta = new Vector2(3, 3);
            _winPanel.DOScale(1, 0.3f).SetEase(Ease.OutExpo).
                OnComplete(() =>
                {
                    _winText.DOScale(1.5f, 0.3f).SetLoops(-1, LoopType.Yoyo);
                });
            return;
        }
        _losePanel.sizeDelta = new Vector2(3, 3);
        _losePanel.DOScale(1, 0.3f).SetEase(Ease.OutExpo).
                OnComplete(() =>
                {
                    _loseText.DOScale(1.5f, 0.3f).SetLoops(-1, LoopType.Yoyo);
                });

    }
}
