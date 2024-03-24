
namespace SpeedMode
{
    public class FireGoblin : Enemy
    {
        protected override void Awake()
        {
            base.Awake();

            MAX_HEALTH = 1;
            correctInput.Add(1, Swordman.State.Guard);
        }
    }
}
