using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using LiveSplit.Model;
using LiveSplit.TimeAttackPause.IO;
using LiveSplit.TimeAttackPause.UI.Components;

namespace LiveSplit.UI.Components
{
    public class TimeAttackPauseComponent : ControlComponent
    {
        private readonly Button _exportButton;
        private readonly Button _importButton;


        private TimeAttackPauseSettings Settings { get; set; }

        private ITimerModel Model { get; set; }

        // This object contains all of the current information about the splits, the timer, etc.
        private LiveSplitState CurrentState { get; set; }

        public override string ComponentName => "TimeAttackPause";

        public override float HorizontalWidth => 50;
        public override float MinimumWidth => 160;
        public override float VerticalHeight => 40;
        public override float MinimumHeight => 40;

        // This function is called when LiveSplit creates your component. This happens when the
        // component is added to the layout, or when LiveSplit opens a layout with this component
        // already added.
        public TimeAttackPauseComponent(LiveSplitState state) : this(state, CreateFormControl(state))
        {
        }

        private static Control CreateFormControl(LiveSplitState state)
        {
            // create a basic button to export the current run state
            var exportButton = new Button
            {
                Name = "ExportButton",
                Text = "Export",
                Location = new Point(0, 0),
                BackColor = Color.LightBlue,
                Dock = DockStyle.Fill,
            };

            // create a new button that will be used to import a run state
            var importButton = new Button
            {
                Name = "ImportButton",
                Text = "Import",
                Location = new Point(0, 0),
                BackColor = Color.LightBlue,
                Dock = DockStyle.Fill,
            };

            var tableLayoutContainer = new TableLayoutPanel
            {
                Name = "Container",
                Size = new Size(100, 32),
                ColumnCount = 2,
                RowCount = 1,
                AutoSize = false,
                Location = new Point(0, 0),
                ColumnStyles =
                {
                    new ColumnStyle(SizeType.Percent, 50F),
                    new ColumnStyle(SizeType.Percent, 50F),
                }
            };

            // add the buttons to the container
            tableLayoutContainer.Controls.Add(exportButton);
            tableLayoutContainer.Controls.Add(importButton);

            return tableLayoutContainer;
        }

        private TimeAttackPauseComponent(LiveSplitState state, Control formControl) : base(state, formControl,
            ex => ErrorCallback(state.Form, ex))
        {
            Settings = new TimeAttackPauseSettings();
            Model = new TimerModel() { CurrentState = state };

            _exportButton = formControl.Controls["ExportButton"] as Button;
            _importButton = formControl.Controls["importButton"] as Button;

            if (_exportButton == null || _importButton == null)
            {
                throw new Exception("Could not find the buttons in the form control.");
            }

            _exportButton.Click += OnExportButtonClick;
            _importButton.Click += OnImportButtonClick;

            CurrentState = state;
        }

        private void OnExportButtonClick(object sender, EventArgs e)
        {
            if (CurrentState.CurrentPhase == TimerPhase.Running)
            {
                Model.Pause();
            }

            // Displays a SaveFileDialog so the user can save the Run as json file
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
            saveFileDialog.Title = "Save Your Run";
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName == "") return;

            SplitsStateWriter.SaveSplitsState(CurrentState, saveFileDialog.FileName);
        }

        private void OnImportButtonClick(object sender, EventArgs e)
        {
            // Displays a OpenFileDialog so the user can save the Run as json file
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
            openFileDialog.Title = "Open Your Run";
            openFileDialog.ShowDialog();

            if (openFileDialog.FileName == "") return;

            if (CurrentState.CurrentPhase != TimerPhase.NotRunning)
            {
                Model.Reset();
            }

            SplitStateImporter.ImportState(openFileDialog.FileName, CurrentState, Model);
        }

        static void ErrorCallback(Form form, Exception ex)
        {
            string requiredBits = Environment.Is64BitProcess ? "64" : "32";
            MessageBox.Show(form, "Error appeared: " + ex.Message, "TimeAttackPause Component Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void DisposeIfError()
        {
            if (ErrorWithControl)
            {
                Dispose();
            }
        }

        public override void DrawHorizontal(Graphics g, LiveSplitState state, float height, Region clipRegion)
        {
            base.DrawHorizontal(g, state, height, clipRegion);
            DisposeIfError();
        }

        // We will be adding the ability to display the component across two rows in our settings menu.
        public override void DrawVertical(Graphics g, LiveSplitState state, float width, Region clipRegion)
        {
            base.DrawVertical(g, state, width, clipRegion);
            DisposeIfError();
        }

        public override Control GetSettingsControl(LayoutMode mode)
        {
            Settings.Mode = mode;
            return Settings;
        }

        public override XmlNode GetSettings(XmlDocument document)
        {
            return Settings.GetSettings(document);
        }

        public override void SetSettings(XmlNode settings)
        {
            Settings.SetSettings(settings);
        }

        // This is the function where we decide what needs to be displayed at this moment in time,
        // and tell the internal component to display it. This function is called hundreds to
        // thousands of times per second.
        public override void Update(IInvalidator invalidator, LiveSplitState state, float width, float height,
            LayoutMode mode)
        {
            CurrentState = state;
        }

        // This function is called when the component is removed from the layout, or when LiveSplit
        // closes a layout with this component in it.
        public override void Dispose()
        {
            base.Dispose();

            _exportButton.Click -= OnExportButtonClick;
            _importButton.Click -= OnImportButtonClick;
        }

        // I do not know what this is for.
        public int GetSettingsHashCode() => Settings.GetSettingsHashCode();
    }
}