﻿using Newtonsoft.Json;
using Wordki.Core.Enums;

namespace Wordki.Infrastructure.DTO
{
    public class GroupDTO
    {
        [JsonConverter(typeof(LongToStringConverter))]
        public long Id { get; set; }
        public string Name { get; set; }
        public LanguageType Language1 { get; set; }
        public LanguageType Language2 { get; set; }
        public int WordsCount { get; set; }
        public int ResultsCount { get; set; }
    }
}
