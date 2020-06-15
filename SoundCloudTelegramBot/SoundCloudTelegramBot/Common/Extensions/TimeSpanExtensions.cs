using System;

namespace SoundCloudTelegramBot.Common.Extensions
{
    public static class TimeSpanExtensions
    {
        public static double GetFileSizeWith120KbpsInMegabytes(this TimeSpan duration)
            => Math.Round(duration.TotalSeconds) * 15 / 1000;
    }
}