﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
using PowerPointLabs.DataSources;
using PowerPointLabs.Models;
using PowerPointLabs.Utils;
using PPExtraEventHelper;
using Shape = System.Windows.Shapes.Shape;

namespace PowerPointLabs.DrawingsLab
{
    /// <summary>
    /// Interaction logic for DrawingsPaneWPF.xaml
    /// </summary>
    public partial class DrawingsPaneWPF
    {
        private static bool hotkeysInitialised = false;

        public static DrawingsLabDataSource dataSource { get; private set; }

        public DrawingsPaneWPF()
        {
            InitializeComponent();

            InitialiseHotkeys();

            BindDataToPanels();

            InitToolTipControl();
        }

        #region ToolTip
        private void InitToolTipControl()
        {
            //toolTip1.SetToolTip(panel1, TextCollection.ColorsLabText.MainColorBoxTooltips);
        }
        #endregion

        #region DataBindings
        private void BindDataToPanels()
        {
            dataSource = FindResource("DrawingsLabData") as DrawingsLabDataSource;
        }
        #endregion


        #region HotkeyInitialisation
        private bool IsPanelOpen()
        {
            var drawingsPane = Globals.ThisAddIn.GetActivePane(typeof(DrawingsPane));
            return drawingsPane.Visible;
        }

        private bool IsReadingHotkeys()
        {
            // Is reading hotkeys when panel is open and user is not selecting text.
            return IsPanelOpen() &&
                   dataSource.HotkeysEnabled &&
                   PowerPointCurrentPresentationInfo.CurrentSelection.Type != PpSelectionType.ppSelectionText;
        }

        private Action RunOnlyWhenOpen(Action action)
        {
            return () => { if (IsReadingHotkeys()) action(); };
        }

        private void InitialiseHotkeys()
        {
            if (hotkeysInitialised) return;
            hotkeysInitialised = true;

            PPKeyboard.AddConditionToBlockTextInput(IsReadingHotkeys);

            var numericKeys = new[]
            {
                Native.VirtualKey.VK_0,
                Native.VirtualKey.VK_1,
                Native.VirtualKey.VK_2,
                Native.VirtualKey.VK_3,
                Native.VirtualKey.VK_4,
                Native.VirtualKey.VK_5,
                Native.VirtualKey.VK_6,
                Native.VirtualKey.VK_7,
                Native.VirtualKey.VK_8,
                Native.VirtualKey.VK_9,
            };

            // I use a regular for loop due to inconsistent compiler behaviour when foreach is used.
            for (int i = 0; i < numericKeys.Length; ++i)
            {
                var key = numericKeys[i];
                // Assign number and ctrl+number to control group commands.
                PPKeyboard.AddKeyupAction(key, RunOnlyWhenOpen(() => DrawingsLabMain.SelectControlGroup(key)));
                PPKeyboard.AddKeyupAction(key, RunOnlyWhenOpen(() => DrawingsLabMain.SetControlGroup(key)), ctrl: true);

                // Block shift+number and ctrl+shift+number
                PPKeyboard.AddConditionToBlockTextInput(IsReadingHotkeys, key, shift: true);
                PPKeyboard.AddConditionToBlockTextInput(IsReadingHotkeys, key, ctrl: true, shift: true);

                // Assign shift+number and ctrl+shift+number to control group commands.
                PPKeyboard.AddKeyupAction(key, RunOnlyWhenOpen(() => DrawingsLabMain.SelectControlGroup(key, appendToSelection: true)), shift: true);
                PPKeyboard.AddKeyupAction(key, RunOnlyWhenOpen(() => DrawingsLabMain.SetControlGroup(key, appendToGroup: true)), ctrl: true, shift: true);
            }

            PPKeyboard.AddKeyupAction(Native.VirtualKey.VK_Q, RunOnlyWhenOpen(DrawingsLabMain.SwitchToLineTool));
            PPKeyboard.AddKeyupAction(Native.VirtualKey.VK_W, RunOnlyWhenOpen(DrawingsLabMain.SwitchToRectangleTool));
            PPKeyboard.AddKeyupAction(Native.VirtualKey.VK_E, RunOnlyWhenOpen(DrawingsLabMain.SwitchToCircleTool));
            PPKeyboard.AddKeyupAction(Native.VirtualKey.VK_A, RunOnlyWhenOpen(DrawingsLabMain.SelectAllOfType));
            PPKeyboard.AddKeyupAction(Native.VirtualKey.VK_H, RunOnlyWhenOpen(DrawingsLabMain.HideTool));
            PPKeyboard.AddKeyupAction(Native.VirtualKey.VK_D, RunOnlyWhenOpen(DrawingsLabMain.CloneTool));
            PPKeyboard.AddKeyupAction(Native.VirtualKey.VK_F, RunOnlyWhenOpen(DrawingsLabMain.MultiCloneExtendTool));
            PPKeyboard.AddKeyupAction(Native.VirtualKey.VK_G, RunOnlyWhenOpen(DrawingsLabMain.MultiCloneBetweenTool));
        }
        #endregion

        private void LineButton_Click(object sender, EventArgs e)
        {
            DrawingsLabMain.SwitchToLineTool();
        }

        private void RectangleButton_Click(object sender, EventArgs e)
        {
            DrawingsLabMain.SwitchToRectangleTool();
        }

        private void CircleButton_Click(object sender, EventArgs e)
        {
            DrawingsLabMain.SwitchToCircleTool();
        }

        private void SelectAllOfTypeButton_Click(object sender, EventArgs e)
        {
            DrawingsLabMain.SelectAllOfType();
        }

        private void HideButton_Click(object sender, EventArgs e)
        {
            DrawingsLabMain.HideTool();
        }

        private void ShowAllButton_Click(object sender, EventArgs e)
        {
            DrawingsLabMain.ShowAllTool();
        }

        private void CloneButton_Click(object sender, EventArgs e)
        {
            DrawingsLabMain.CloneTool();
        }

        private void MultiCloneExtendButton_Click(object sender, EventArgs e)
        {
            DrawingsLabMain.MultiCloneExtendTool();
        }

        private void MultiCloneBetweenButton_Click(object sender, EventArgs e)
        {
            DrawingsLabMain.MultiCloneBetweenTool();
        }

        private void ApplyPositionButton_Click(object sender, EventArgs e)
        {
            DrawingsLabMain.ApplyPosition();
        }

        private void RecordPositionButton_Click(object sender, EventArgs e)
        {
            DrawingsLabMain.RecordPosition();
        }

        private void ApplyDisplacementButton_Click(object sender, EventArgs e)
        {
            DrawingsLabMain.ApplyDisplacement();
        }

        private void RecordDisplacementButton_Click(object sender, EventArgs e)
        {
            DrawingsLabMain.RecordDisplacement();
        }

        private void ApplyFormatButton_Click(object sender, EventArgs e)
        {
            DrawingsLabMain.ApplyFormat();
        }

        private void RecordFormatButton_Click(object sender, EventArgs e)
        {
            DrawingsLabMain.RecordFormat();
        }

        private void AlignHorizontalButton_Click(object sender, EventArgs e)
        {
            DrawingsLabMain.AlignHorizontal();
        }

        private void AlignVerticalButton_Click(object sender, EventArgs e)
        {
            DrawingsLabMain.AlignVertical();
        }

        private void BringForwardButton_Click(object sender, EventArgs e)
        {
            DrawingsLabMain.BringForward();
        }

        private void BringInFrontOfShapeButton_Click(object sender, EventArgs e)
        {
            DrawingsLabMain.BringInFrontOfShape();
        }

        private void BringToFrontButton_Click(object sender, EventArgs e)
        {
            DrawingsLabMain.BringToFront();
        }

        private void SendBackwardButton_Click(object sender, EventArgs e)
        {
            DrawingsLabMain.SendBackward();
        }

        private void SendBehindShapeButton_Click(object sender, EventArgs e)
        {
            DrawingsLabMain.SendBehindShape();
        }

        private void SendToBackButton_Click(object sender, EventArgs e)
        {
            DrawingsLabMain.SendToBack();
        }

        private void FillColor_Click(object sender, EventArgs e)
        {
            var colorDialog = new ColorDialog
            {
                Color = Graphics.ConvertRgbToColor(dataSource.FormatFillColor),
                FullOpen = true
            };
            if (colorDialog.ShowDialog() == DialogResult.Cancel) return;
            dataSource.FormatFillColor = Graphics.ConvertColorToRgb(colorDialog.Color);
        }

        private void LineColor_Click(object sender, EventArgs e)
        {
            var colorDialog = new ColorDialog
            {
                Color = Graphics.ConvertRgbToColor(dataSource.FormatLineColor),
                FullOpen = true
            };
            if (colorDialog.ShowDialog() == DialogResult.Cancel) return;
            dataSource.FormatLineColor = Graphics.ConvertColorToRgb(colorDialog.Color);
        }
    }
}
