using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerController : MonoBehaviour
{
    [SerializeField] private AudioMixer m_AudioMixer;
    [SerializeField] private Slider m_MusicMasterSlider;
    [SerializeField] private Slider m_MusicBGMSlider;
    [SerializeField] private Slider m_MusicSESlider;

    // 싱글톤 인스턴스
    public static AudioMixerController instance;

    private void Awake()
    {
        // 싱글톤 패턴 적용: 이 오브젝트가 이미 존재하면 새로 생성된 오브젝트는 삭제
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬 변경 시에도 삭제되지 않게 설정
        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 존재하면 새로 생성된 오브젝트 파괴
            return;
        }
    }

    private void Start()
    {
        // 슬라이더의 값 변경 이벤트 리스너 추가
        m_MusicMasterSlider.onValueChanged.AddListener(SetMasterVolume);
        m_MusicBGMSlider.onValueChanged.AddListener(SetMusicVolume);
        m_MusicSESlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMasterVolume(float volume)
    {
        m_AudioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
    }

    public void SetMusicVolume(float volume)
    {
        m_AudioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        m_AudioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }
}
