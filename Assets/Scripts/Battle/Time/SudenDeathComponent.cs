using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using DG.Tweening;
using Main.Setting;

namespace Battle
{
	public class SudenDeathComponent : BattleComponent
    {
        //참조 변수
        private TimeComponent _timeComponent = null;
        private UnitComponent _unitCommand = null;
        private CardComponent _cardCommand = null;
        private CostComponent _costCommand = null;
        private GameObject _suddenDeathTextObj = null;
        private PencilCaseComponent _pencilCaseComponent = null;
        private Unit _playerPencilCase = null;
        private Unit _enemyPencilCase = null;
        private GameObject _suddenDeathBackground;
        private GameObject _suddenDeathGlow;
        private AudioMixerGroup _bgmMixerGruop = null;

        //변수
        private bool _isSuddenDeath;


        public void SetInitialization(GameObject suddenDeathTextObj, TimeComponent timeComponent, UnitComponent unitComponent, CardComponent cardComponent, CostComponent costComponent, PencilCaseComponent pencilCaseComponent, GameObject suddenBack, GameObject suddenGlow, AudioMixerGroup bgmMixerGruop)
		{
            _suddenDeathTextObj = suddenDeathTextObj;
            _timeComponent = timeComponent;
            _unitCommand = unitComponent;
            _cardCommand = cardComponent;
            _costCommand = costComponent;
            _pencilCaseComponent = pencilCaseComponent;
            _suddenDeathBackground = suddenBack;
            _suddenDeathGlow = suddenGlow;
            _bgmMixerGruop = bgmMixerGruop;
        }

        /// <summary>
        /// 서든데스 시작
        /// </summary>
        public void SetSuddenDeath()
        {
            _cardCommand.ClearCards();
            _unitCommand.ClearUnit();

            if (!_isSuddenDeath)
            {
                _suddenDeathTextObj.gameObject.SetActive(true);
                _suddenDeathTextObj.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack)
                    .SetLoops(2, LoopType.Yoyo)
                    .OnComplete(() => _suddenDeathTextObj.gameObject.SetActive(false));
                _cardCommand.SetMaxCard(8);
                _costCommand.SetCostSpeed(500);
                _isSuddenDeath = true;
                _timeComponent.SetTime(20);
                _suddenDeathBackground.SetActive(true);
                _suddenDeathGlow.SetActive(true);
                _bgmMixerGruop.audioMixer.SetFloat("BGMPitch", 1.2f);
                Sound.PlayEff(1);
                return;
            }

            //체력 비교
            if (_pencilCaseComponent.PlayerPencilCase.UnitStat.Hp > _pencilCaseComponent.EnemyPencilCase.UnitStat.Hp)
            {
                _timeComponent.Lose();
                _timeComponent.SetFinallyEnd(true);
                return;
            }
            else if (_pencilCaseComponent.PlayerPencilCase.UnitStat.Hp < _pencilCaseComponent.EnemyPencilCase.UnitStat.Hp)
            {
                _timeComponent.Lose();
                _timeComponent.SetFinallyEnd(true);
                return;
            }
            else
            {
                _timeComponent.Lose();
                _timeComponent.SetFinallyEnd(true);
            }
        }
    }
}
