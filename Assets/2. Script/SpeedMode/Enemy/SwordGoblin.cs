
namespace SpeedMode
{
    public class SwordGoblin : Enemy
    {
        protected override void Awake()
        {
            base.Awake();

            EnemyType = Types.SwordGoblin;
            maxHealth = 1;
            canEscape = false;

            correctInput = new()
            {
                {1, Swordman.State.Attack}
            };
        }

        // 오히려 잔상이 남는 느낌
        // protected override void Die()
        // {
        //     DelayInvoke(0.05f, objectPool.ReturnEnemy, this);
        // }
    }
}
