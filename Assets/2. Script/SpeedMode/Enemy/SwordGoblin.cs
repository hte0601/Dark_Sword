
namespace SpeedMode
{
    public class SwordGoblin : Enemy
    {
        protected override void Awake()
        {
            base.Awake();

            EnemyType = Type.SwordGoblin;
            MAX_HEALTH = 1;
            correctInput.Add(1, Swordman.State.Attack);
        }
    }
}