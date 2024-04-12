
namespace SpeedMode
{
    public class FireGoblin : Enemy
    {
        protected override void Awake()
        {
            base.Awake();

            EnemyType = Type.FireGoblin;
            MAX_HEALTH = 1;
            correctInput.Add(1, Swordman.State.Guard);
        }
    }
}
