using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeedMode
{
    public static class SFX
    {
        public enum Game
        {
            BestScoreUpdate
        }
    }

    public class SoundManager : MonoBehaviour
    {
        public static SoundManager instance;

        [Header("BGM")]
        [SerializeField] private AudioSource BGMPlayer;

        [Header("GameSFX")]
        [SerializeField] private AudioSource gameSFXPlayer;
        [SerializeField] private AudioClip bestScoreUpdateSound;

        private readonly Dictionary<SFX.Game, AudioClip> gameSFXDict = new();


        private void Awake()
        {
            instance = this;

            gameSFXDict.Add(SFX.Game.BestScoreUpdate, bestScoreUpdateSound);
        }


        public void PlayBGM()
        {
            BGMPlayer.Play();
        }

        public void StopBGM()
        {
            BGMPlayer.Stop();
        }

        public void PlaySFX(SFX.Game sound)
        {
            gameSFXPlayer.PlayOneShot(gameSFXDict[sound]);
        }
    }
}
