using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using LiveSplit.Model;
using LiveSplit.TimeAttackPause.DTO;
using Run = LiveSplit.TimeAttackPause.DTO.Run;

namespace LiveSplit.TimeAttackPause.IO
{
    public static class SplitsStateWriter
    {
        public static void SaveSplitsState(LiveSplitState state, string saveFilePath)
        {
            var timingMethod = state.CurrentTimingMethod;

            var splits = state.Run.ToList();

            var splitsDTOs = new List<Split>();
            foreach (var split in splits)
            {
                var time = split.SplitTime[timingMethod];
                var splitDto = new Split()
                {
                    Time = time,
                    Name = split.Name
                };
                splitsDTOs.Add(splitDto);
            }

            var run = new Run()
            {
                TimingMethod = timingMethod,
                Splits = splitsDTOs,
                CurrentSplitIndex = state.CurrentSplitIndex,
                CurrentSplitTime = state.CurrentSplit.SplitTime[timingMethod],
                CurrentTime = state.CurrentTime[timingMethod],
            };
            
            var serializerOptions = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(run, serializerOptions);
            System.IO.File.WriteAllText(saveFilePath, json);
        }
    }
}