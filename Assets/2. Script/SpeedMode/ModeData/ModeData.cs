using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeedMode
{
    public class ModeData
    {
        public static class TimerData
        {
            public const float MAX_TIME = 12f; //시간의 최대치
            public const float ADDITIONAL_TIME = 1f; //입력에 성공했을 때 증가하는 시간

            /* 고블린 간의 거리 간격이 2이고 속도가 10이므로 이론상 1초에 최대 5마리의 적을 처치할 수 있음
            * 게임속도가 60f까지 올라가고 입력에 성공했을 때 증가하는 시간이 15f이므로
            * 초당 4번 이상 입력에 성공할 경우 게임을 무한히 진행할 수 있음
            * 게임속도가 최대까지 증가하는데 1분 20초가 걸림 */
        }

        public static class SwordmanData
        {
            public const float MAX_BATTLE_RANGE = 3f;  // 전투 할 수 있는 최대 거리
        }

        public static class EnemyData
        {
            public static readonly Vector3 ENEMY_CREATE_POSITION = new Vector3(12f, -3.6f, 0);
            public static readonly Vector3 ENEMY_MOVE_TARGET_POSITION = new Vector3(-7f, -3.6f, 0);
            public static readonly Vector3 ENEMY_ENEMY_GAP = new Vector3(2f, 0, 0);  // 적과 적 사이의 간격
            public const float ENEMY_MOVE_SPEED = 12f;  // 적의 이동 속도
            public const int MAX_ENEMY_NUMBER = 12;  // 맵애 생성되는 적의 최대 숫자

            // 적 간격 1.65, 이동 속도 10 기준 이론상 1초에 최대 6.06마리의 적을 처치할 수 있음
        }
    }
}
