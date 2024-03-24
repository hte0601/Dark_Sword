
namespace SpeedMode
{
    public class SwordGoblin : Enemy
    {
        protected override void Awake()
        {
            base.Awake();

            MAX_HEALTH = 1;
            correctInput.Add(1, Swordman.State.Attack);
        }
    }
}