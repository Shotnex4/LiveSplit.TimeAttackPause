using System;
using System.Collections.Generic;
using LiveSplit.Model;

namespace LiveSplit.TimeAttackPause.DTO
{
    public record Run
    {
        public TimingMethod TimingMethod { get; set; }
        public List<Split> Splits { get; set; }
        public int CurrentSplitIndex { get; set; }
        public TimeSpan? CurrentSplitTime { get; set; }
        public TimeSpan? CurrentTime { get; set; }
    }
}