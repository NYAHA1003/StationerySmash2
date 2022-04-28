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
    /// ȿ������ ��巹����� ������ ����Ѵ�
    /// </summary>
    /// <param name="effSoundType"></param>
    /// <returns></returns>
    public async Task EFFAssetAsync(EffSoundType effSoundType)
    {
        //�����Ͱ� �ִ��� Ȯ���ϰ� ��ųʸ��� �߰�
        if (!_effectSoundDictionary.TryGetValue(effSoundType, out var value))
        {
            //��巹����� �Ҹ� �����͸� �����´�
            string name = System.Enum.GetName(typeof(EffSoundType), effSoundType);
            AsyncOperationHandle<AudioClip> handle = Addressables.LoadAssetAsync<AudioClip>(name);
            await handle.Task;

            _effectSoundDictionary.Add(effSoundType, handle.Result);
        }
    }

    /// <summary>
    /// ��������� ��巹����� ������ ����Ѵ�
    /// </summary>
    /// <param name="bgmSoundType"></param>
    /// <returns></returns>
    public async Task BGMAssetAsync(BGMSoundType bgmSoundType)
    {
        //�����Ͱ� �ִ��� Ȯ���ϰ� ��ųʸ��� �߰�
        if (!_bgmSoundDictionary.TryGetValue(bgmSoundType, out var value))
        {
            //��巹����� �Ҹ� �����͸� �����´�
            string name = System.Enum.GetName(typeof(BGMSoundType), bgmSoundType);
            AsyncOperationHandle<AudioClip> handle = Addressables.LoadAssetAsync<AudioClip>(name);
            await handle.Task;

            _bgmSoundDictionary.Add(bgmSoundType, handle.Result);
        }
    }

    /// <summary>
    /// ��� �����͸� �ҷ��´�
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
    /// Ÿ���� ������ŭ ����� �ҽ����� �����Ѵ�
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
    /// ����Ʈ ���� ����� �ҽ��� �����Ѵ�
    /// </summary>
    private void CreateEffSoundSource()
    {
        GameObject obj = Instantiate(_SoundSourcePrefab, transform);

        //������ҽ� ������Ʈ�� ����Ʈ�� �־��ش�
        AudioSource audio = obj.GetComponent<AudioSource>();
        _effSources.Add(audio);
    }

    /// <summary>
    /// BGM ���� ����� �ҽ��� �����Ѵ�
    /// </summary>
    private void CreateBgmSoundSource()
    {
        GameObject obj = Instantiate(_SoundSourcePrefab, transform);

        //������ҽ� ������Ʈ�� ����Ʈ�� �־��ش�
        AudioSource audio = obj.GetComponent<AudioSource>();
        _bgmSources.Add(audio);
    }


    /// <summary>
    /// ȿ���� Ÿ�Կ� ���� ȿ���� ��ȯ
    /// </summary>
    /// <param name="effSoundType"></param>
    /// <returns></returns>
    public AudioClip GetEFFSound(EffSoundType effSoundType)
    {
        return _effectSoundDictionary[effSoundType];
    }

    /// <summary>
    /// ����� Ÿ�Կ� ���� ȿ���� ��ȯ
    /// </summary>
    /// <param name="effSoundType"></param>
    /// <returns></returns>
    public AudioClip GetBGMSound(BGMSoundType bgmSoundType)
    {
        return _bgmSoundDictionary[bgmSoundType];
    }

    /// <summary>
    /// ȿ���� ����� �ҽ��鿡�� Ŭ���� �־��ش�
    /// </summary>
    public void PlayEffInput()
    {
        for (int i = 0; i < System.Enum.GetValues(typeof(EffSoundType)).Length; i++)
        {
            _effSources[i].clip = GetEFFSound((EffSoundType)i);
        }
    }

    /// <summary>
    /// ������� ����� �ҽ��鿡�� Ŭ���� �־��ش�
    /// </summary>
    public void PlayBgmInput()
    {
        for (int i = 0; i < System.Enum.GetValues(typeof(BGMSoundType)).Length; i++)
        {
            _bgmSources[i].clip = GetBGMSound((BGMSoundType)i);
        }
    }
}
