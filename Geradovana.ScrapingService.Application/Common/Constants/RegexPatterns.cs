using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geradovana.ScrapingService.Application.Common.Constants
{
    public static class RegexPatterns
    {
        public static readonly string EmojiPattern = @"(\u00a9|\u00ae|[\u2000-\u3300]|\ud83c[\ud000-\udfff]|\ud83d[\ud000-\udfff]|\ud83e[\ud000-\udfff])";
        public static readonly string DigitsPattern = @"\d+";
    }
}
