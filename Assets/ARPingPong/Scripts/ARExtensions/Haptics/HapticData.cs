namespace Ricoh.TeamHaptics.AR.Extensions.Haptics
{
    using UnityEngine;

    /// <summary>
    /// A data container of Haptics.
    /// </summary>
    public class HapticData
    {
        public long[] pattern {get; private set;}
        public int[] amplitudes {get; private set;}
        public int repeat {get; private set;}

        /// <summary>
        /// Same as Unity's Handheld.Vibrate()
        /// </summary>
        public HapticData() 
        {
            pattern = new long[] {1000};
            amplitudes = new int[] {255};
            repeat = -1;
        }

        static public HapticData CreateFrom()
        {
            return new HapticData();
        }

        static public HapticData CreateFrom(AudioClip sound)
        {
            var hapticData = new HapticData();
            var samples = sound.samples;
            var msec = (int) (sound.length * 1000);
            var sampleData = new float[samples * sound.channels];
            sound.GetData(sampleData, 0);

            var length = samples / msec;
            var amplitudes = new int[msec];
            var pattern = new long[msec];

            for (int i = 0; i < msec; i ++)
            {
                var sum = 0.0f;
                for (int j = 0; j < length; j++)
                {
                    sum += sampleData[i * length + 2 * j] + 1.0f + sampleData[i * length + (2 * j) + 1] + 1.0f;
                }
                pattern[i] = 1;
                amplitudes[i] = (int) ((sum * 255.0 / length) / 4.0f);
            }
            hapticData.pattern = pattern;
            hapticData.amplitudes = amplitudes;
            hapticData.repeat = -1;
            return hapticData;
        }
    }
}