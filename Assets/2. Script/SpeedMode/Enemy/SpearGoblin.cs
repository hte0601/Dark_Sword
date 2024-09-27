
namespace SpeedMode
{
    public class SpearGoblin : Enemy
    {
        protected override void Awake()
        {
            base.Awake();

            EnemyType = Types.SpearGoblin;
            maxHealth = 2;
            canEscape = false;

            correctInput = new()
            {
                {2, Swordman.State.Guard},
                {1, Swordman.State.Attack}
            };
        }
    }
}
