
namespace SpeedMode
{
    public class C_FasterModeData : ModeData
    {
        public C_FasterModeData() : base()
        {
            waveDataDict = new()
            {
                // 총 2000마리, 난이도는 하드 모드에서 타이머 속도만 더 빠름
                {01, new Wave(01, 3.8f, 250, 0.52f, 0.42f, 0.06f)},
                {02, new Wave(02, 4.0f, 250, 0.51f, 0.42f, 0.07f)},
                {03, new Wave(03, 4.2f, 250, 0.51f, 0.41f, 0.08f)},
                {04, new Wave(04, 4.4f, 250, 0.50f, 0.41f, 0.09f)},
                {05, new Wave(05, 4.6f, 250, 0.50f, 0.40f, 0.10f)},
                {06, new Wave(06, 4.8f, 250, 0.49f, 0.40f, 0.11f)},
                {07, new Wave(07, 5.0f, 500, 0.49f, 0.39f, 0.12f)}
            };
        }
    }
}
