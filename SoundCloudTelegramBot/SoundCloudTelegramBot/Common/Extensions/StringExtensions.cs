using System;
using System.Collections.Generic;
using System.Linq;

namespace SoundCloudTelegramBot.Common.Extensions
{
    public static class StringExtensions
    {
        private const string soundCloudUrl = "https://soundcloud.com";
        public static bool TryExtractSoundCloudUrl(this string text, out string url)
        {
            url = text.SplitAndFilter(" ")
                .SelectMany(x => x.SplitAndFilter("\n"))
                .FirstOrDefault(x => x.StartsWith(soundCloudUrl));
            return !string.IsNullOrEmpty(url);
        }

        private static IEnumerable<string> SplitAndFilter(this string value, string splitBy)
            => value
                .Split(splitBy, StringSplitOptions.RemoveEmptyEntries)
                .Where(x => x.Length >= soundCloudUrl.Length);
    }
}