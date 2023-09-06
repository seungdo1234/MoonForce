using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Bgm { Main, Stage, MaintenanceRoom , Victory = 4 }
public enum Sfx { Dead, Hurt, FootStep = 4, EnemyHit = 6, GameOver, ChestOpen, Select }

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("#BGM")]
    public AudioClip[] bgmClip;
    public float bgmVolume;
    private AudioSource bgmPlayer;
    private AudioHighPassFilter bgmEffecter;

    [Header("#SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels; // ���� ȿ������ ���� ���� ä�� �ý���
    private AudioSource[] sfxPlayers; 
    private int channelIndex; // ä�� ���� ��ŭ ��ȸ�ϵ��� �� �������� �÷��� �ߴ� SFX�� �ε�����ȣ�� �����ϴ� ����


    private void Awake()
    {
        instance = this;
        Init();
    }
    private void Start()
    {
        PlayBgm(0);
    }
    private void Init()
    {
        // ����� �÷��̾� �ʱ�ȭ
        // ������Ʈ ����
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;

        bgmPlayer = bgmObject.AddComponent<AudioSource>();
      //  bgmPlayer.playOnAwake = t; // �÷��� ���ڸ��� ���� false
        bgmPlayer.loop = true; // �ݺ� true
        bgmPlayer.volume = bgmVolume; // ����
        bgmEffecter = Camera.main.GetComponent<AudioHighPassFilter>();

        // ȿ���� �÷��̾� �ʱ�ȭ
        GameObject sfxObject = new GameObject("SFXPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for (int i = 0; i < sfxPlayers.Length; i++)
        {
            sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].volume = sfxVolume;
            sfxPlayers[i].bypassListenerEffects = true; // �����н��� �� �ɸ��� ��
        }

    }
    public void PlayBgm(int bgmNumber)
    {
        bgmPlayer.clip = bgmClip[bgmNumber];
        bgmPlayer.Play();
    }
    public void EffectBgm(bool isPlay)
    {
        bgmEffecter.enabled = isPlay;
    }
    public void PlayerSfx(Sfx sfx)
    {
        for (int i = 0; i < sfxPlayers.Length; i++)
        {
            // ������� 5�� �ε����� ���������� ��������� 6 7 8 9 10 1 2 3 4 5 �̷������� ��ȸ�ϰ� �ϱ����� �����
            int loopIndex = (i + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying) // �ش� ä���� Play ���̶��
            {
                continue;
            }

            int randomIndex = 0;
            if (sfx == Sfx.EnemyHit)
            {
                randomIndex = Random.Range(0, 2);
            }
            else if (sfx == Sfx.FootStep)
            {
                randomIndex = Random.Range(0, 3);
            }

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx + randomIndex];
            sfxPlayers[loopIndex].Play();
            break; // ȿ������ �� ä�ο��� ��� �Ʊ� ������ �ݵ�� break�� �ݺ����� ������������
        }

    }
}