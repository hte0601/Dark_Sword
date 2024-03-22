using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeedMode
{
    public class BalanceData
    {
        public const float MAX_TIME = 10f; //시간의 최대치
        public const float BONUS_TIME_VALUE = 1f; //입력에 성공했을 때 증가하는 시간
        public const float MIN_TIME_SPEED = 2f; //게임속도 - 시작값 (단위는 1초동안 감소하는 시간의 양)
        public const float MAX_TIME_SPEED = 5f; //게임속도 - 최댓값
        public const float INCREASE_TIME_SPEED = 1f; //term마다 증가하는 게임속도

        public const float SWORDMAN_ENEMY_GAP = 1.85f; // Swordman(플레이어)과 적 사이의 간격 (이렇게 못 쓸듯)
        public readonly Vector3 ENEMY_MOVE_TARGET_POSITION = new Vector3(0, 0, 0); // 입력 필요
        public readonly Vector3 ENEMY_ENEMY_GAP = new Vector3(2f, 0, 0); // 적과 적 사이의 간격
        public readonly float ENEMY_MOVE_SPEED = 12f; // 적의 이동 속도

        // 적 간격 1.65, 이동 속도 10 기준 이론상 1초에 최대 6.06마리의 적을 처치할 수 있음

        public const float RED_GOBLINE_RATE = 0.35f; //전체 enemy 중 빨간 고블린의 비율
        public const float MIN_ATTACK_GOBLINE_RATE = 0.05f; //초록 고블린 중 공격 고블린의 비율 - 시작값
        public const float MAX_ATTACK_GOBLINE_RATE = 0.2f; //초록 고블린 중 공격 고블린의 비율 - 최댓값
        public const float INCREASE_ATTACK_GOBLINE_RATE = 0.025f; //term마다 증가하는 공격 고블린의 비율
        public const float TERM_ATTACK_GOBLINE_RATE = 10f; //공격 고블린의 비율이 증가하는 주기 (단위 초)
                                                           //공격 고블린의 비율이 최대까지 증가하는데 1분이 걸림
    }
}
