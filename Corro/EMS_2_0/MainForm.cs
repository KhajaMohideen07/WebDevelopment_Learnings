using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EMS_2_0.Helpers;
using Microsoft.Win32;

namespace EMS_2_0
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        #region Session Handles
        private void SystemEvents_Sessionswitch(object sender, Microsoft.Win32.SessionSwitchEventArgs e)
        {
            if (e.Reason == Microsoft.Win32.SessionSwitchReason.SessionLock | e.Reason == Microsoft.Win32.SessionSwitchReason.ConsoleDisconnect)
            {
                FormHelpers.WriteLogs("Session Locked");
            }

            if (e.Reason == Microsoft.Win32.SessionSwitchReason.SessionUnlock | e.Reason == Microsoft.Win32.SessionSwitchReason.ConsoleConnect)
            {
                FormHelpers.WriteLogs("Session Unlocked");
            }

            if(e.Reason==Microsoft.Win32.SessionSwitchReason.SessionLogon)
            {
                FormHelpers.WriteLogs("Session Logged On");
            }

            if (e.Reason == Microsoft.Win32.SessionSwitchReason.SessionLogoff)
            {
                FormHelpers.WriteLogs("Session Logged Off");
            }

        }
        #endregion

        #region LoadEvents
       
        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Icon = Properties.Resources.AppIcon;

            SystemEvents.SessionSwitch += SystemEvents_Sessionswitch;

        }
        #endregion
    }
}
