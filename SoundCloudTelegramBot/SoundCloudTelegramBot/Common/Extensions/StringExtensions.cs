using System;
using System.Linq;

namespace SoundCloudTelegramBot.Common.Extensions
{
    public static class StringExtensions
    {
        public static bool TryExtractSoundCloudUrl(this string text, out string url)
        {
            url = text.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault(x => x.StartsWith("https://soundcloud.com"));
            return string.IsNullOrEmpty(url);
        }
    }
}