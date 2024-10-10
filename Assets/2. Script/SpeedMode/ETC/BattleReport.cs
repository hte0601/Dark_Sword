
namespace SpeedMode
{
    public struct BattleReport
    {
        public enum Result
        {
            InputCorrect,
            SwordmanGroggy,
            GameOver,
            SkillCast,
            SkillHit
        }

        public enum EnemyState
        {
            Alive,
            Killed,
            Escaped
        }

        public Result result;
        public Swordman.State? playerInput;
        public Enemy.Types enemyType;
        public EnemyState enemyState;
        public int dealtDamage;

        public bool IsEnemyRemoved()
        {
            return enemyState == EnemyState.Killed || enemyState == EnemyState.Escaped;
        }
    }
}
