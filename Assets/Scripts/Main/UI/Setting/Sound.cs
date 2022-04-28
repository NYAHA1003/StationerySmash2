using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Threading.Tasks;
using Utill.Data;
using Utill.Tool;
using TMPro;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;



public class Sound : MonoBehaviour
{
    [SerializeField]
    private GameObject _SoundSourcePrefab;
    [SerializeField]
    private List<AudioSource> _effSources;
    [SerializeField]
    private List<AudioSource> _bgmSources;

    public static Dictionary<EffSoundType, AudioClip> _effectSoundDictionary = new Dictionary<EffSoundType, AudioClip>();
    public static Dictionary<BGMSoundType, AudioClip> _bgmSoundDictionary = new Dictionary<BGMSoundType, AudioClip>();

    public async void Start()
    {
        await AllLoadAssetAsync();
        CreateSoundSorces();
        PlayEffInput();
        PlayBgmInput();
    }

    /// <summary>
    /// 효과음을 어드레서블로 가져와 등록한다
    /// </summary>
    /// <param name="effSoundType"></param>
    /// <returns></returns>
    public async Task EFFAssetAsync(EffSoundType effSoundType)
    {
        //데이터가 있는지 확인하고 딕셔너리에 추가
        if (!_effectSoundDictionary.TryGetValue(effSoundType, out var value))
        {
            //어드레서블로 소리 데이터를 가져온다
            string name = System.Enum.GetName(typeof(EffSoundType), effSoundType);
            AsyncOperationHandle<AudioClip> handle = Addressables.LoadAssetAsync<AudioClip>(name);
            await handle.Task;

            _effectSoundDictionary.Add(effSoundType, handle.Result);
        }
    }

    /// <summary>
    /// 배경음악을 어드레서블로 가져와 등록한다
    /// </summary>
    /// <param name="bgmSoundType"></param>
    /// <returns></returns>
    public async Task BGMAssetAsync(BGMSoundType bgmSoundType)
    {
        //데이터가 있는지 확인하고 딕셔너리에 추가
        if (!_bgmSoundDictionary.TryGetValue(bgmSoundType, out var value))
        {
            //어드레서블로 소리 데이터를 가져온다
            string name = System.Enum.GetName(typeof(BGMSoundType), bgmSoundType);
            AsyncOperationHandle<AudioClip> handle = Addressables.LoadAssetAsync<AudioClip>(name);
            await handle.Task;

            _bgmSoundDictionary.Add(bgmSoundType, handle.Result);
        }
    }

    /// <summary>
    /// 모든 데이터를 불러온다
    /// </summary>
    /// <returns></returns>
    public async Task AllLoadAssetAsync()
    {
        for (int i = 0; i < System.Enum.GetValues(typeof(EffSoundType)).Length; i++)
        {
            await EFFAssetAsync((EffSoundType)i);
        }
        for (int i = 0; i < System.Enum.GetValues(typeof(BGMSoundType)).Length; i++)
        {
            await BGMAssetAsync((BGMSoundType)i);
        }
    }

    /// <summary>
    /// 타입의 갯수만큼 오디오 소스들을 생성한다
    /// </summary>
    private void CreateSoundSorces()
    {
        for (int i = 0; i < System.Enum.GetValues(typeof(EffSoundType)).Length; i++)
        {
            CreateEffSoundSource();
        }
        for (int i = 0; i < System.Enum.GetValues(typeof(BGMSoundType)).Length; i++)
        {
            CreateBgmSoundSource();
        }
    }

    /// <summary>
    /// 이펙트 사운드 오디오 소스를 생성한다
    /// </summary>
    private void CreateEffSoundSource()
    {
        GameObject obj = Instantiate(_SoundSourcePrefab, transform);

        //오디오소스 컴포넌트를 리스트에 넣어준다
        AudioSource audio = obj.GetComponent<AudioSource>();
        _effSources.Add(audio);
    }

    /// <summary>
    /// BGM 사운드 오디오 소스를 생성한다
    /// </summary>
    private void CreateBgmSoundSource()
    {
        GameObject obj = Instantiate(_SoundSourcePrefab, transform);

        //오디오소스 컴포넌트를 리스트에 넣어준다
        AudioSource audio = obj.GetComponent<AudioSource>();
        _bgmSources.Add(audio);
    }


    /// <summary>
    /// 효과음 타입에 따른 효과음 반환
    /// </summary>
    /// <param name="effSoundType"></param>
    /// <returns></returns>
    public AudioClip GetEFFSound(EffSoundType effSoundType)
    {
        return _effectSoundDictionary[effSoundType];
    }

    /// <summary>
    /// 배경음 타입에 따른 효과음 반환
    /// </summary>
    /// <param name="effSoundType"></param>
    /// <returns></returns>
    public AudioClip GetBGMSound(BGMSoundType bgmSoundType)
    {
        return _bgmSoundDictionary[bgmSoundType];
    }

    /// <summary>
    /// 효과음 오디오 소스들에게 클립을 넣어준다
    /// </summary>
    public void PlayEffInput()
    {
        for (int i = 0; i < System.Enum.GetValues(typeof(EffSoundType)).Length; i++)
        {
            _effSources[i].clip = GetEFFSound((EffSoundType)i);
        }
    }

    /// <summary>
    /// 배경음악 오디오 소스들에게 클립을 넣어준다
    /// </summary>
    public void PlayBgmInput()
    {
        for (int i = 0; i < System.Enum.GetValues(typeof(BGMSoundType)).Length; i++)
        {
            _bgmSources[i].clip = GetBGMSound((BGMSoundType)i);
        }
    }
}
