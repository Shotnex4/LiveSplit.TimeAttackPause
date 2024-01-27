#nullable enable

using System;

namespace LiveSplit.TimeAttackPause.DTO
{
    public record Split
    {
        public TimeSpan? Time { get; set; }
        public string? Name { get; set; }
    }
}