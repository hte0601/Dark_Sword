using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeedMode
{
    public static class SFX
    {
        public enum Game
        {
            BestScoreUpdate,
            HealthLoss
        }

        public enum Swordman
        {
            Slash,
            Guard,
            AttackHit
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
        [SerializeField] private AudioClip healthLossSound;

        [Header("SwordmanSFX")]
        [SerializeField] private AudioSource swordmanSFXPlayer;
        [SerializeField] private AudioClip swordmanSlashSound;
        [SerializeField] private AudioClip swordmanGuardSound;
        [SerializeField] private AudioClip swordmanAttackHitSound;

        [Header("EnemySFX")]
        [SerializeField] private AudioSource enemySFXPlayer;

        private readonly Dictionary<SFX.Game, AudioClip> gameSFXDict = new();
        private readonly Dictionary<SFX.Swordman, AudioClip> swordmanSFXDict = new();


        private void Awake()
        {
            sm = this;

            gameSFXDict.Add(SFX.Game.BestScoreUpdate, bestScoreUpdateSound);
            gameSFXDict.Add(SFX.Game.HealthLoss, healthLossSound);

            swordmanSFXDict.Add(SFX.Swordman.Slash, swordmanSlashSound);
            swordmanSFXDict.Add(SFX.Swordman.Guard, swordmanGuardSound);
            swordmanSFXDict.Add(SFX.Swordman.AttackHit, swordmanAttackHitSound);
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

        public static void PlaySFX(SFX.Swordman sound)
        {
            sm.swordmanSFXPlayer.PlayOneShot(sm.swordmanSFXDict[sound]);
        }
    }
}
