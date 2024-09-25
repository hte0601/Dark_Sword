using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleMode
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager sm;

        //BGM source
        public AudioSource BGM;

        [Header("Player")]
        //Player sound
        public AudioSource playerMusicPlayer;
        public AudioClip slashSound;
        public AudioClip defenseSound;
        public AudioClip perfectDefenseSound;
        public AudioClip pierceSound;
        public AudioClip sidecutSound;
        public AudioClip avoidSound;

        [Header("Player Walk")]
        public AudioSource playerWalkPlayer;
        public AudioClip playerFootSound;

        [Header("Enemy")]
        //Enemy sound
        public AudioSource enemyMusicPlayer;
        public AudioClip enemyLandingSound;
        public AudioClip enemyDownAttackSound;
        public AudioClip enemyPierceAttackSound;
        public AudioClip enemyThrowSpearSound;

        [Header("Enemy Weapon")]
        //enemy weapon sound
        public AudioSource enemyWeaponPlayer;
        public AudioClip enemyWeaponSound;

        [Header("Enemy Walk")]
        public AudioSource enemyWalkPlayer;
        public AudioClip enemyFootSound;

        [Header("Effect")]
        //effect sound
        public AudioSource effectSoundPlayer;
        public AudioClip enemyHitSound;
        public AudioClip playerHitSound;
        public AudioClip GameOverUISound;
        public AudioClip VictoryUISound;


        [Header("GameSFX")]
        //game sound
        public AudioSource gameSfxSoundPlayer;
        public AudioClip phaseChangeSound;
        public AudioClip gameOverSound;
        public AudioClip gameWinSound;


        void Start()
        {
            sm = this;
        }

        //Player sound
        public static void PlayerSlashSound()
        {
            sm.playerMusicPlayer.Stop();
            sm.playerMusicPlayer.clip = sm.slashSound;
            sm.playerMusicPlayer.loop = false;
            sm.playerMusicPlayer.time = 0;
            sm.playerMusicPlayer.Play();
        }

        public static void PlayerDefenseSound()
        {
            sm.playerMusicPlayer.Stop();
            sm.playerMusicPlayer.clip = sm.defenseSound;
            sm.playerMusicPlayer.loop = false;
            sm.playerMusicPlayer.time = 0;
            sm.playerMusicPlayer.Play();
        }

        public static void PlayerPerfectDefenseSound()
        {
            sm.playerMusicPlayer.Stop();
            sm.playerMusicPlayer.clip = sm.perfectDefenseSound;
            sm.playerMusicPlayer.loop = false;
            sm.playerMusicPlayer.time = 0;
            sm.playerMusicPlayer.Play();
        }

        public static void PlayerPierceSound()
        {
            sm.playerMusicPlayer.Stop();
            sm.playerMusicPlayer.clip = sm.pierceSound;
            sm.playerMusicPlayer.loop = false;
            sm.playerMusicPlayer.time = 0;
            sm.playerMusicPlayer.Play();
        }

        public static void PlayerSidecutSound()
        {
            sm.playerMusicPlayer.Stop();
            sm.playerMusicPlayer.clip = sm.sidecutSound;
            sm.playerMusicPlayer.loop = false;
            sm.playerMusicPlayer.time = 0;
            sm.playerMusicPlayer.Play();
        }

        public static void PlayerAvoidSound()
        {
            sm.playerMusicPlayer.Stop();
            sm.playerMusicPlayer.clip = sm.avoidSound;
            sm.playerMusicPlayer.loop = false;
            sm.playerMusicPlayer.time = 0;
            sm.playerMusicPlayer.Play();
        }

        //walk sound
        public static void PlayerFootSound()
        {
            if (sm.playerMusicPlayer.isPlaying)
            {
                sm.playerWalkPlayer.Stop();
                return;
            }
            else if (sm.playerWalkPlayer.isPlaying)
                return;

            sm.playerWalkPlayer.Stop();
            sm.playerWalkPlayer.loop = true;
            sm.playerWalkPlayer.time = 0;
            sm.playerWalkPlayer.Play();
        }

        public static void playerSoundStop()
        {
            sm.playerWalkPlayer.Stop();
        }

        public static void EnemyFootSound()
        {
            if (sm.enemyMusicPlayer.isPlaying)
            {
                sm.enemyWalkPlayer.Stop();
                return;
            }
            else if (sm.enemyWalkPlayer.isPlaying)
                return;

            sm.enemyWalkPlayer.Stop();
            sm.enemyWalkPlayer.clip = sm.enemyFootSound;
            sm.enemyWalkPlayer.loop = true;
            sm.enemyWalkPlayer.time = 0;
            sm.enemyWalkPlayer.Play();
        }

        public static void EnemyWalkingSoundStop()
        {
            sm.enemyWalkPlayer.Stop();
        }

        //Enemy Sound
        public static void EnemyLandingSound()
        {
            sm.enemyMusicPlayer.Stop();
            sm.enemyMusicPlayer.clip = sm.enemyLandingSound;
            sm.enemyMusicPlayer.loop = false;
            sm.enemyMusicPlayer.time = 0;
            sm.enemyMusicPlayer.Play();
        }

        public static void EnemyDownAttackSound()
        {
            sm.enemyMusicPlayer.Stop();
            sm.enemyMusicPlayer.clip = sm.enemyDownAttackSound;
            sm.enemyMusicPlayer.loop = false;
            sm.enemyMusicPlayer.time = 0;
            sm.enemyMusicPlayer.Play();
        }

        public static void EnemyPierceAttackSound()
        {
            sm.enemyMusicPlayer.Stop();
            sm.enemyMusicPlayer.clip = sm.enemyPierceAttackSound;
            sm.enemyMusicPlayer.loop = false;
            sm.enemyMusicPlayer.time = 0;
            sm.enemyMusicPlayer.Play();
        }

        public static void EnemyThrowSpearSound()
        {
            sm.enemyMusicPlayer.Stop();
            sm.enemyMusicPlayer.clip = sm.enemyThrowSpearSound;
            sm.enemyMusicPlayer.loop = false;
            sm.enemyMusicPlayer.time = 0;
            sm.enemyMusicPlayer.Play();
        }

        //Enemy Weapon Sound
        public static void EnemyWeaponSound()
        {
            sm.enemyWeaponPlayer.Stop();
            sm.enemyWeaponPlayer.clip = sm.enemyWeaponSound;
            sm.enemyWeaponPlayer.loop = false;
            sm.enemyWeaponPlayer.time = 0;
            sm.enemyWeaponPlayer.Play();
        }

        //effect sound
        public static void PlayerHitSound()
        {
            sm.effectSoundPlayer.Stop();
            sm.effectSoundPlayer.clip = sm.playerHitSound;
            sm.effectSoundPlayer.loop = false;
            sm.effectSoundPlayer.time = 0;
            sm.effectSoundPlayer.Play();
        }

        public static void EnemyHitSound()
        {
            sm.effectSoundPlayer.Stop();
            sm.effectSoundPlayer.clip = sm.enemyHitSound;
            sm.effectSoundPlayer.loop = false;
            sm.effectSoundPlayer.time = 0;
            sm.effectSoundPlayer.Play();
        }

        public static void GameOverUIPlay()
        {
            sm.effectSoundPlayer.Stop();
            sm.effectSoundPlayer.clip = sm.GameOverUISound;
            sm.effectSoundPlayer.loop = false;
            sm.effectSoundPlayer.time = 0;
            sm.effectSoundPlayer.Play();
        }

        public static void VictoryUIPlay()
        {
            sm.effectSoundPlayer.Stop();
            sm.effectSoundPlayer.clip = sm.VictoryUISound;
            sm.effectSoundPlayer.loop = false;
            sm.effectSoundPlayer.time = 0;
            sm.effectSoundPlayer.Play();
        }

        //Dead sound
        public static void PlayGameOverSound()
        {
            sm.gameSfxSoundPlayer.Stop();
            sm.gameSfxSoundPlayer.clip = sm.gameOverSound;
            sm.gameSfxSoundPlayer.loop = false;
            sm.gameSfxSoundPlayer.time = 0;
            sm.gameSfxSoundPlayer.Play();
        }

        public static void PlayGameWinSound()
        {
            sm.gameSfxSoundPlayer.Stop();
            sm.gameSfxSoundPlayer.clip = sm.gameWinSound;
            sm.gameSfxSoundPlayer.loop = false;
            sm.gameSfxSoundPlayer.time = 0;
            sm.gameSfxSoundPlayer.Play();
        }

        public static void EnemyPhaseChangeSound()
        {
            sm.gameSfxSoundPlayer.Stop();
            sm.gameSfxSoundPlayer.clip = sm.phaseChangeSound;
            sm.gameSfxSoundPlayer.loop = false;
            sm.gameSfxSoundPlayer.time = 0;
            sm.gameSfxSoundPlayer.Play();
        }

        //BGM sound
        public static void BGMStop()
        {
            sm.BGM.Stop();
        }

        public static void BGMStart()
        {
            sm.BGM.Play();
        }
    }
}
