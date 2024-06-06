
public static class Tool
{
    public static class Random
    {
        public static float Value(bool isMaxInclusive = false)
        {
            if (isMaxInclusive)
            {
                return UnityEngine.Random.value;
            }
            else
            {
                float randomValue;

                while (true)
                {
                    randomValue = UnityEngine.Random.value;

                    if (randomValue != 1.0f)
                        return randomValue;
                }
            }
        }

        public static float Range(float minInclusive, float max, bool isMaxInclusive = false)
        {
            if (isMaxInclusive)
            {
                return UnityEngine.Random.Range(minInclusive, max);
            }
            else
            {
                float randomValue;

                while (true)
                {
                    randomValue = UnityEngine.Random.Range(minInclusive, max);

                    if (randomValue != max)
                        return randomValue;
                }
            }
        }

        public static int Range(int minInclusive, int maxExclusive)
        {
            return UnityEngine.Random.Range(minInclusive, maxExclusive);
        }
    }
}
