using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Threading.Tasks;
using Util;
using TMPro;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

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
    private List<AudioClip> effClips;
    [SerializeField]
    private AudioSource bgmAudioSource;
    [SerializeField]
    private List<AudioClip> bgmClips;
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
    [SerializeField]
    private GameObject BGMSOURCEPrefab;
    [SerializeField]
    private GameObject[] GetChild;
    [SerializeField]
    private List<AudioSource> effSources;


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
        CreateSoundSorces();
        PlayEffValum();
    }

    private void OnEnable()
    {
        _audioMixer.GetFloat("BGM", out float bgmValue);
        _audioMixer.GetFloat("EFF", out float effectValue);
        _bgmSlider.value = bgmValue;
        _effectSoundSlider.value = effectValue;
        
    }

    public async Task EFFAssetAsync(string s)
    {
        AsyncOperationHandle<AudioClip> handle = Addressables.LoadAssetAsync<AudioClip>(s);
        await handle.Task;
        effClips.Add(handle.Result);
    }

    public async Task BGMAssetAsync(string s)
    {
        AsyncOperationHandle<AudioClip> handle = Addressables.LoadAssetAsync<AudioClip>(s);
        await handle.Task;
        bgmClips.Add(handle.Result);
    }

    public async void AllLoadAssetAsync()
    {
        for (int i = 0; i < System.Enum.GetValues(typeof(EffSoundType)).Length; i++)
        {
            string s = System.Enum.GetName(typeof(EffSoundType), i);
            await EFFAssetAsync(s);
        }
        for (int i = 0; i < System.Enum.GetValues(typeof(BGMSoundType)).Length; i++)
        {
            string s = System.Enum.GetName(typeof(BGMSoundType), i);
            await BGMAssetAsync(s);
        }
    }

    /// <summary>
    /// 타입의 갯수만큼 오디오 소스를 생성한다
    /// </summary>
    private void CreateSoundSorces()
    {
        for (int i = 0; i < System.Enum.GetValues(typeof(EffSoundType)).Length; i++)
        {
            CreateSoundSource();
        }
    }

    /// <summary>
    /// 오디오 소스를 생성한다
    /// </summary>
    private void CreateSoundSource()
    {
        GameObject obj = Instantiate(BGMSOURCEPrefab, transform);

        //오디오소스 컴포넌트를 리스트에 넣어준다
        AudioSource audio = obj.GetComponent<AudioSource>();
        effSources.Add(audio);
    }

    public GameObject[] GetChildren()
    {
        GameObject[] children = new GameObject[gameObject.transform.childCount];

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            children[i] = gameObject.transform.GetChild(i).gameObject;
        }

        return children;
    }

    /// <summary>
    /// 오디오 소스들에게 클립을 넣어준다
    /// </summary>
    public void PlayEffValum()
    {
        int count = effClips.Count;
        for (int i = 0; i < count; i++)
        {
            effSources[i].clip = effClips[i];
        }
    }

    private void SetFuntionSound_UI()
    {
        ResetFunctionSound_UI();
        _effectSoundSlider.onValueChanged.AddListener(EFFSliderText); 
        _bgmSlider.onValueChanged.AddListener(BGMSliderText);
        BgmBtn.onClick.AddListener(BGMBtnClick);
        EffBtn.onClick.AddListener(EFFBtnClick);
        AllLoadAssetAsync();
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
