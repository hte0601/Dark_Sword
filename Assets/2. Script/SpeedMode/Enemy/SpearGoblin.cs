
namespace SpeedMode
{
    public class SpearGoblin : Enemy
    {
        protected override void Awake()
        {
            base.Awake();

            EnemyType = Type.SpearGoblin;
            MAX_HEALTH = 2;
            correctInput.Add(2, Swordman.State.Guard);
            correctInput.Add(1, Swordman.State.Attack);
        }
    }
}
