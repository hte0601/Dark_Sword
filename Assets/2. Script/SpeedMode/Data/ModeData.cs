using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeedMode
{
    public class ModeData
    {
        public static class BalanceData
        {
            public const float MAX_TIME = 10f; //시간의 최대치
            public const float BONUS_TIME_VALUE = 1f; //입력에 성공했을 때 증가하는 시간
            public const float MIN_TIME_SPEED = 2f; //게임속도 - 시작값 (단위는 1초동안 감소하는 시간의 양)
            public const float MAX_TIME_SPEED = 5f; //게임속도 - 최댓값
            public const float INCREASE_TIME_SPEED = 1f; //term마다 증가하는 게임속도

            public const float SWORDMAN_ENEMY_GAP = 1.85f; // Swordman(플레이어)과 적 사이의 간격 (이렇게 못 쓸듯)
            public static readonly Vector3 ENEMY_MOVE_TARGET_POSITION = new Vector3(0, 0, 0); // 입력 필요
            public static readonly Vector3 ENEMY_ENEMY_GAP = new Vector3(2f, 0, 0); // 적과 적 사이의 간격
            public const float ENEMY_MOVE_SPEED = 12f; // 적의 이동 속도

            // 적 간격 1.65, 이동 속도 10 기준 이론상 1초에 최대 6.06마리의 적을 처치할 수 있음

            public const float RED_GOBLINE_RATE = 0.35f; //전체 enemy 중 빨간 고블린의 비율
            public const float MIN_ATTACK_GOBLINE_RATE = 0.05f; //초록 고블린 중 공격 고블린의 비율 - 시작값
            public const float MAX_ATTACK_GOBLINE_RATE = 0.2f; //초록 고블린 중 공격 고블린의 비율 - 최댓값
            public const float INCREASE_ATTACK_GOBLINE_RATE = 0.025f; //term마다 증가하는 공격 고블린의 비율
            public const float TERM_ATTACK_GOBLINE_RATE = 10f; //공격 고블린의 비율이 증가하는 주기 (단위 초)
                                                               //공격 고블린의 비율이 최대까지 증가하는데 1분이 걸림
        }

        public static class WaveData
        {
            public static readonly Dictionary<int, Wave> waves = new();

            static WaveData()
            {
                waves.Add(01, new Wave(250, 0.52f, 0.43f, 0.05f));
                waves.Add(02, new Wave(300, 0.52f, 0.42f, 0.06f));
                waves.Add(03, new Wave(350, 0.51f, 0.42f, 0.07f));
                waves.Add(04, new Wave(500, 0.51f, 0.41f, 0.08f));
                waves.Add(05, new Wave(450, 0.50f, 0.41f, 0.09f));
                waves.Add(06, new Wave(500, 0.50f, 0.40f, 0.10f));
                waves.Add(07, new Wave(550, 0.49f, 0.40f, 0.11f));
                waves.Add(08, new Wave(600, 0.49f, 0.39f, 0.12f));
                waves.Add(09, new Wave(650, 0.48f, 0.39f, 0.13f));
                waves.Add(10, new Wave(700, 0.48f, 0.38f, 0.14f));
                waves.Add(11, new Wave(750, 0.47f, 0.38f, 0.15f));
                waves.Add(12, new Wave(800, 0.47f, 0.37f, 0.16f));
                waves.Add(13, new Wave(850, 0.46f, 0.37f, 0.17f));
                waves.Add(14, new Wave(900, 0.46f, 0.36f, 0.18f));
                waves.Add(15, new Wave(950, 0.45f, 0.36f, 0.19f));
                waves.Add(16, new Wave(1000, 0.45f, 0.35f, 0.20f));
            }
        }
    }
}
