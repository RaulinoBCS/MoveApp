using Microsoft.Win32;
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

        RegistryKey reg = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        public Form1()
        {
            InitializeComponent();

            BtnStop.Enabled = false;

            SetTimer.SelectedIndex = Properties.Settings.Default.Timer;

            if (Properties.Settings.Default.StartWhenOpen == true)
            {
                BtnStop.Enabled = true;
                BtnStart.Enabled = false;
                timer1.Interval = Properties.Settings.Default.Timer * 60000;
                timer1.Start();
                stopToolStripMenuItem.Enabled = true;
                startToolStripMenuItem.Enabled = false;
                SystemTrayIcon.ShowBalloonTip(3000, "MoveApp status:", "Running", ToolTipIcon.Info);
            }
            else
            {

                stopToolStripMenuItem.Enabled = false;
                startToolStripMenuItem.Enabled = true;
            }

            if (Properties.Settings.Default.StartWithWindows == true)
            {
                yesToolStripMenuItem.Enabled = false;
                noToolStripMenuItem.Enabled = true;
            }
            else
            {
                yesToolStripMenuItem.Enabled = true;
                noToolStripMenuItem.Enabled = false;
            }

        }
        protected override CreateParams CreateParams // 
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x80;  // Turn on WS_EX_TOOLWINDOW
                return cp;
            }
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            BtnStop.Enabled = true;
            BtnStart.Enabled = false;
            timer1.Interval = Properties.Settings.Default.Timer * 60000;
            timer1.Start();
            SystemTrayIcon.ShowBalloonTip(3000, "MoveApp status:", "Running", ToolTipIcon.Info);
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            BtnStop.Enabled = false;
            BtnStart.Enabled = true;
            timer1.Stop();
            SystemTrayIcon.ShowBalloonTip(3000, "MoveApp status:", "Stopped", ToolTipIcon.Info);
        }

        private void SetTimer_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Timer = (int)SetTimer.SelectedIndex;
            Properties.Settings.Default.Save();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            SystemTrayIcon.Dispose();
            Application.Exit();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Point originalPosition = Cursor.Position;
            Point newPosition = new Point(originalPosition.X + 5, originalPosition.Y);
            Cursor.Position = newPosition;
            Cursor.Position = originalPosition;
        }

        private void yesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            reg.SetValue("Move", Application.ExecutablePath.ToString());
            yesToolStripMenuItem.Enabled = false;
            noToolStripMenuItem.Enabled = true;
            Properties.Settings.Default.StartWithWindows = true;
            Properties.Settings.Default.Save();
        }

        private void noToolStripMenuItem_Click(object sender, EventArgs e)
        {
            reg.DeleteValue("Move", false);
            yesToolStripMenuItem.Enabled = true;
            noToolStripMenuItem.Enabled = false;
            Properties.Settings.Default.StartWithWindows = false;
            Properties.Settings.Default.Save();
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.StartWhenOpen = true;
            Properties.Settings.Default.Save();
            stopToolStripMenuItem.Enabled = true;
            startToolStripMenuItem.Enabled = false;
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.StartWhenOpen = false;
            Properties.Settings.Default.Save();
            stopToolStripMenuItem.Enabled = false;
            startToolStripMenuItem.Enabled = true;
        }
    }
}