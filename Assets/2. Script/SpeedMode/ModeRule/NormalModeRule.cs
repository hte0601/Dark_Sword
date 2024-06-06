
namespace SpeedMode
{
    public class NormalModeRule : ModeRule
    {
        public NormalModeRule()
        {
            waveDict = new()
            {
                // 총 1200마리
                {01, new Wave(01, 1.2f, 150, 0.55f, 0.45f, 0.00f, 0.1f)},
                {02, new Wave(02, 1.4f, 150, 0.53f, 0.43f, 0.04f, 0.1f)},
                {03, new Wave(03, 1.6f, 150, 0.52f, 0.43f, 0.05f, 0.1f)},
                {04, new Wave(04, 1.8f, 150, 0.52f, 0.42f, 0.06f, 0.1f)},
                {05, new Wave(05, 2.0f, 150, 0.51f, 0.42f, 0.07f, 0.1f)},
                {06, new Wave(06, 2.2f, 150, 0.51f, 0.41f, 0.08f, 0.1f)},
                {07, new Wave(07, 2.5f, 300, 0.50f, 0.41f, 0.09f, 0.1f)}
            };
        }
    }
}
