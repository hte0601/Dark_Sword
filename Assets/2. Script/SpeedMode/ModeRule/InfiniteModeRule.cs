
namespace SpeedMode
{
    public class InfiniteModeRule : ModeRule
    {
        Wave infiniteWaveData;

        public InfiniteModeRule() : base()
        {
            infiniteWaveData = new(0, 4.0f, 250, 0.49f, 0.39f, 0.12f);
        }

        public override bool LoadWaveData(int wave, out Wave waveData)
        {
            infiniteWaveData.wave = wave;
            waveData = infiniteWaveData;

            currentWaveData = waveData;
            previousEnemy = Enemy.Type.None;

            return true;
        }
    }
}
