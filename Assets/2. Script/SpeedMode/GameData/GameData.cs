using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeedMode
{
    public class GameData
    {
        public static class TimerData
        {
            public const float MAX_TIME = 12f; //시간의 최대치
            public const float ADDITIONAL_TIME = 1f; //입력에 성공했을 때 증가하는 시간
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
            public const int MAX_ENEMY_NUMBER = 12;  // 씬에 동시에 존재할 수 있는 적의 최대 숫자
        }
    }
}
