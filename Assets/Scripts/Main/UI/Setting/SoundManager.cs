using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using Util;
using TMPro;

namespace Util
{
    enum SoundType
    {
        Bgm,
        Effect
    }
    enum EffSoundType
    {
        Exp,
        Attack,
        Button,
        Button1,
        Spawn,
        Money,
        Die,
        paperButton,
        LevelUp
    }
    enum BGMSoundType
    { 
        Main,
        Intro,
        Stage1,
        Stage2,
        Stage3,
        Stage4,
        GongJackSo
    }
}


public class SoundManager : MonoBehaviour
{
    

    [SerializeField]
    private AudioMixer _audioMixer;
    [SerializeField]
    private List<AudioSource> effSources;
    [SerializeField]
    private AudioSource bgmAudioSource;
    [SerializeField]
    private List<AudioSource> bgmSources;
    [SerializeField]
    private Slider _bgmSlider;
    [SerializeField]
    private Slider _effectSoundSlider;
    [SerializeField]
    private TextMeshProUGUI bgmText;
    [SerializeField]
    private TextMeshProUGUI effText;
    [SerializeField]
    private Button EffBtn;
    [SerializeField]
    private Button BgmBtn;

    
    private bool iseffSound = false;
    private bool isbgmSound = false;
    private float effValue = 0;
    private float bgmValue = 0;


    [SerializeField]
    int minValue = -40;
    [SerializeField]
    int maxValue = 0;

    private void Awake()
    {
        SetFuntionSound_UI();
        BGMSliderText(_bgmSlider.value);
        EFFSliderText(_effectSoundSlider.value);
    }

    private void OnEnable()
    {
        _audioMixer.GetFloat("BGM", out float bgmValue);
        _audioMixer.GetFloat("EFF", out float effectValue);
        _bgmSlider.value = bgmValue;
        _effectSoundSlider.value = effectValue; 
    }   

    /*public void OnSetAudio(int soundType)
    {
        sound[0] = _bgmSlider.value;
        sound[1] = _effectSoundSlider.value;
        switch (soundType)
        {
            case (int)SoundType.Bgm:
                _audioMixer.SetFloat("BGM", Mathf.Log10(sound[soundType]) * 20);
                break;
            case (int)SoundType.Effect:
                _audioMixer.SetFloat("Effect", Mathf.Log10(sound[soundType]) * 20);
                break; 
        }
       
        //PlayerPrefs.SetFloat("Bgm",);
        //PlayerPrefs.SetFloat("Effect",);
    }*/
    public void PlayEffValum(int sound)
    {
        effSources[sound].Play();
    }

    private void SetFuntionSound_UI()
    {
        ResetFunctionSound_UI();
        _effectSoundSlider.onValueChanged.AddListener(EFFSliderText); 
        _bgmSlider.onValueChanged.AddListener(BGMSliderText);
        BgmBtn.onClick.AddListener(BGMBtnClick);
        EffBtn.onClick.AddListener(EFFBtnClick);
    }

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

        BgmBtn.onClick.RemoveAllListeners();
        EffBtn.onClick.RemoveAllListeners();
    }

    private void BGMSliderText(float _value)
    {
        _audioMixer.SetFloat("BGM", _bgmSlider.value); 
        _value = (_bgmSlider.value - minValue) / (maxValue - minValue) * 100;
        bgmText.text = _value.ToString();
    }

    private void EFFSliderText(float _value)
    {
        _audioMixer.SetFloat("EFF", _effectSoundSlider.value);
        _value = (_effectSoundSlider.value - minValue) / (maxValue - minValue) * 100;
        effText.text = _value.ToString();
        
    }

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

    private void BGMBtnClick()
    {
        
        if(!isbgmSound)
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
