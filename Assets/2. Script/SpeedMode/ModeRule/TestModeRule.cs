using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeedMode
{
    public class TestModeRule : ModeRule
    {
        public TestModeRule()
        {
            waveDict = new()
            {
                {01, new Wave(01, 2.8f, 250, 0.52f, 0.42f, 0.06f)},
                {02, new Wave(02, 3.0f, 250, 0.51f, 0.42f, 0.07f)},
                {03, new Wave(03, 3.2f, 250, 0.51f, 0.41f, 0.08f)},
                {04, new Wave(04, 3.4f, 250, 0.50f, 0.41f, 0.09f)},
                {05, new Wave(05, 3.6f, 250, 0.50f, 0.40f, 0.10f)},
                {06, new Wave(06, 3.8f, 250, 0.49f, 0.40f, 0.11f)},
                {07, new Wave(07, 4.0f, 500, 0.49f, 0.39f, 0.12f)}
            };
        }
    }
}
