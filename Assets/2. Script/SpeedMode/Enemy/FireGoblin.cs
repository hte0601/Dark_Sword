
namespace SpeedMode
{
    public class FireGoblin : Enemy
    {
        protected override void Awake()
        {
            base.Awake();

            maxHealth = 1;
        }
    }
}
