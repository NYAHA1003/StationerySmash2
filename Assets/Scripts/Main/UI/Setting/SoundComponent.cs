using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Threading.Tasks;
using Utill;
using TMPro;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

namespace Main.Setting
{
    public class SoundComponent : MonoBehaviour
    {
        [SerializeField]
        private AudioMixer _audioMixer;
        [SerializeField]
        private AudioSource _bgmAudioSource;
        [SerializeField]
        private Slider _bgmSlider;
        [SerializeField]
        private Slider _effectSoundSlider;
        [SerializeField]
        private TextMeshProUGUI _bgmText;
        [SerializeField]
        private TextMeshProUGUI _effText;
        [SerializeField]
        private Button _EffBtn;
        [SerializeField]
        private Button _BgmBtn;


        private bool iseffSound = false;
        private bool isbgmSound = false;
        private float effValue = 0;
        private float bgmValue = 0;


        [SerializeField]
        int minValue = -40;
        [SerializeField]
        int maxValue = 0;

        private void Start()
        {
            SetFuntionSound_UI();
            BGMSliderText(_bgmSlider.value);
            EFFSliderText(_effectSoundSlider.value);
        }

        /// <summary>
        /// 리스너들을 생성해준다.
        /// </summary>
        private void SetFuntionSound_UI()
        {
            ResetFunctionSound_UI();
            _effectSoundSlider.onValueChanged.AddListener(EFFSliderText);
            _bgmSlider.onValueChanged.AddListener(BGMSliderText);
            _BgmBtn.onClick.AddListener(BGMBtnClick);
            _EffBtn.onClick.AddListener(EFFBtnClick);
        }

        /// <summary>
        /// 리스너들을 지우고, 슬라이더의 값을 넣어준다
        /// </summary>
        private void ResetFunctionSound_UI()
        {
            _bgmSlider.minValue = minValue;
            _bgmSlider.maxValue = maxValue;
            _bgmSlider.wholeNumbers = true;
            _bgmSlider.onValueChanged.RemoveAllListeners();

            _effectSoundSlider.minValue = minValue;
            _effectSoundSlider.maxValue = maxValue;
            _effectSoundSlider.wholeNumbers = true;
            _effectSoundSlider.onValueChanged.RemoveAllListeners();

            _BgmBtn.onClick.RemoveAllListeners();
            _EffBtn.onClick.RemoveAllListeners();
        }

        /// <summary>
        /// BGM 슬라이더의 텍스트를 출력한다
        /// </summary>
        /// <param name="_value"></param>
        private void BGMSliderText(float _value)
        {
            _audioMixer.SetFloat("BGM", _bgmSlider.value);
            _value = (_bgmSlider.value - minValue) / (maxValue - minValue) * 100;
            _bgmText.text = _value.ToString();
        }

        /// <summary>
        /// 이펙트 슬라이더의 텍스트를 출력한다
        /// </summary>
        /// <param name="_value"></param>
        private void EFFSliderText(float _value)
        {
            _audioMixer.SetFloat("EFF", _effectSoundSlider.value);
            _value = (_effectSoundSlider.value - minValue) / (maxValue - minValue) * 100;
            _effText.text = _value.ToString();

        }

        /// <summary>
        /// 이펙트 사우드 버튼을 눌렀을때 음소거, 음소거 해제
        /// </summary>
        private void EFFBtnClick()
        {
            if (!iseffSound)
            {
                effValue = _effectSoundSlider.value;
                _effectSoundSlider.value = minValue;
                _audioMixer.SetFloat("EFF", minValue);
                _effectSoundSlider.interactable = false;
                iseffSound = true;
                return;
            }
            _effectSoundSlider.value = effValue;
            _audioMixer.SetFloat("EFF", effValue);
            _effectSoundSlider.interactable = true;
            iseffSound = false;
            return;
        }

        /// <summary>
        /// BGM 사운드 버튼을 눌렀을때 음소거, 음소거 해제
        /// </summary>
        private void BGMBtnClick()
        {

            if (!isbgmSound)
            {
                bgmValue = _bgmSlider.value;
                _bgmSlider.value = minValue;
                _audioMixer.SetFloat("BGM", minValue);
                _bgmSlider.interactable = false;
                isbgmSound = true;
                return;
            }
            _bgmSlider.value = bgmValue;
            _audioMixer.SetFloat("BGM", bgmValue);
            _bgmSlider.interactable = true;
            isbgmSound = false;
            return;
        }
    }
}