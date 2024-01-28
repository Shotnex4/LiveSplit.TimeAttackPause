using System;
using LiveSplit.Model;
using LiveSplit.UI.Components;

namespace LiveSplit.TimeAttackPause.UI.Components
{
    internal class TimeAttackPauseFactory : IComponentFactory
    {
        // The displayed name of the component in the Layout Editor.
        public string ComponentName => "Time Attack Pause";

        public string Description =>
            "Adds the possibility to save your progress mid run to a file and then continue from this point on. Only use this for TA runs or unofficial runs that you do not submit to the leaderboards.";

        // The sub-menu this component will appear under when adding the component to the layout.
        public ComponentCategory Category => ComponentCategory.Timer;

        public IComponent Create(LiveSplitState state) => new TimeAttackPauseComponent(state);

        public string UpdateName => ComponentName;

        // Fill in this empty string with the URL of the repository where your component is hosted.
        // This should be the raw content version of the repository. If you're not uploading this
        // to GitHub or somewhere, you can ignore this.
        public string UpdateURL => "";

        // Fill in this empty string with the path of the XML file containing update information.
        // Check other LiveSplit components for examples of this. If you're not uploading this to
        // GitHub or somewhere, you can ignore this.
        public string XMLURL => UpdateURL + "";

        public Version Version => Version.Parse("1.0.0");
    }
}