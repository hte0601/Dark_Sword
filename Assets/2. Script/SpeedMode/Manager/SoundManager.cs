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
        private static SoundManager sm;

        [Header("BGM")]
        [SerializeField] private AudioSource BGMPlayer;

        [Header("GameSFX")]
        [SerializeField] private AudioSource gameSFXPlayer;
        [SerializeField] private AudioClip bestScoreUpdateSound;

        private readonly Dictionary<SFX.Game, AudioClip> gameSFXDict = new();


        private void Awake()
        {
            sm = this;

            gameSFXDict.Add(SFX.Game.BestScoreUpdate, bestScoreUpdateSound);
        }


        public static void PlayBGM()
        {
            sm.BGMPlayer.Play();
        }

        public static void StopBGM()
        {
            sm.BGMPlayer.Stop();
        }

        public static void PlaySFX(SFX.Game sound)
        {
            sm.gameSFXPlayer.PlayOneShot(sm.gameSFXDict[sound]);
        }
    }
}
