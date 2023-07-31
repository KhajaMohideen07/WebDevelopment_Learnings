
Imports System.Data.Odbc
Imports System.Threading
Imports System.Threading.Thread
Imports System.Threading.Monitor
Imports System.IO
Imports System.Net.NetworkInformation
Imports System
Imports Microsoft.Win32
Imports System.Windows.Forms
Imports MongoDB.Bson
Imports MongoDB.Driver
Imports System.DirectoryServices
Imports System.DirectoryServices.AccountManagement


Public Class veeBOT

    Private Delegate Sub myDelegate()
    'Dim strFile As String = "\\172.29.2.68\vfs\vBOT-App\AppLog.txt"
    'Dim sw As StreamWriter = File.AppendText(strFile)
    Dim last_active_id As Integer
    Dim hlm_messenger_T As Thread
    Dim shift_type As Integer
    Dim keyboard_check As Integer = 0
    Dim mra_mail_id As Integer = 0
    Dim counter1 As New Count1()
    Dim Thread1 As New System.Threading.Thread(AddressOf counter1.Count)
    Dim current_break As String = Nothing
    'Dim live_time As Date = get_server_time()
    Public auto_lock_or_manual_lock As Integer = 0 '1-auto lock, 0-manual lock
    Private it As New IdleTime
    Dim last_title As String = Nothing

    Public Shared Event SessionSwitch_Event As Microsoft.Win32.SessionSwitchEventHandler
    Public Delegate Sub SessionSwitchEventHandler(ByVal sender As Object, ByVal e As SessionSwitchEventArgs)

    Dim h As System.Net.IPHostEntry = System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName)
    Private Class NativeMethodsHome
        Private Declare Function LockWorkStation Lib "User32.dll" () As Long

        Declare Function GetForegroundWindow Lib "user32" Alias "GetForegroundWindow" () As Integer
        Declare Function GetWindowTextLength Lib "user32.dll" Alias "GetWindowTextLengthA" (ByVal hwnd As Integer) As Integer
        Declare Function GetWindowText Lib "user32" Alias "GetWindowTextA" (ByVal hwnd As Integer, ByVal lpString As String, ByVal cch As Integer) As Integer

    End Class

    Private Sub SystemEvents_Sessionswitch(ByVal sender As Object, ByVal e As Microsoft.Win32.SessionSwitchEventArgs)
        Try
            If e.Reason = Microsoft.Win32.SessionSwitchReason.SessionLock Or e.Reason = Microsoft.Win32.SessionSwitchReason.ConsoleDisconnect Then
                'sw.WriteLine(Environment.UserName & ":" & emp_action_start_time & ": System Locked")
                'MsgBox("locked at: " & DateTime.Now)
                'App_Timer.Enabled = False
                messenger_activity_id = 0
                mysql_data_str("update tbl_emp_monitor set updated_on=now(), current_status='System Locked' where emp_id='" & emp_id & "'")
                mysql_data_str("UPDATE `emp_active_hours` SET end_time=NOW() WHERE id='" & last_active_id & "'")
                emp_action_start_time = get_server_time()
            End If

            If e.Reason = Microsoft.Win32.SessionSwitchReason.SessionLogon Then
                'sw.WriteLine(Environment.UserName & ":" & emp_action_start_time & ": System Logon")
            End If

            If e.Reason = Microsoft.Win32.SessionSwitchReason.SessionUnlock Or e.Reason = Microsoft.Win32.SessionSwitchReason.ConsoleConnect Then
                'If shift_type = 0 Then
                '    emp_login_date = CDate(emp_login_time)
                'Else
                '    If Format(emp_login_time, "HH:mm:ss") < "11:59:00" Then
                '        emp_login_date = DateAdd(DateInterval.Day, -1, CDate(emp_login_time))
                '    Else
                '        emp_login_date = CDate(emp_login_time)
                '    End If
                'End If

                'MsgBox(DateDiff(DateInterval.Minute, emp_action_start_time, Now()))
                If DateDiff(DateInterval.Minute, emp_action_start_time, Now()) > 180 Then
                    If System.IO.File.Exists("\\vrcm-ops\vfs\vBOT-App\setup.exe") = True Then
                        Process.Start("\\vrcm-ops\vfs\vBOT-App\setup.exe")
                    ElseIf System.IO.File.Exists("\\172.29.2.68\vfs\vBOT-App\setup.exe") = True Then
                        Process.Start("\\172.29.2.68\vfs\vBOT-App\setup.exe")
                    Else
                        If System.IO.File.Exists("\\vrcm-ops\vfs\vBOT-App\EMS_Shortcut.lnk") = True Then
                            Process.Start("\\vrcm-ops\vfs\vBOT-App\EMS_Shortcut.lnk")
                        ElseIf System.IO.File.Exists("\\172.29.2.68\vfs\vBOT-App\EMS_Shortcut.lnk") = True Then
                            Process.Start("\\172.29.2.68\vfs\vBOT-App\EMS_Shortcut.lnk")
                        Else
                            If System.IO.File.Exists("C:\Users\" & Environment.UserName & "\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Visionary RCM\EMS\vBOT.appref-ms") = True Then
                                Process.Start("C:\Users\" & Environment.UserName & "\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Visionary RCM\EMS\vBOT.appref-ms")
                            Else
                                MsgBox("Contact AppDev Team / IT Support. Ext: 3214")
                                End
                            End If
                        End If
                    End If
                End If
                'hlm_messenger_T.Abort()
                ''MsgBox("unlocked at: " & DateTime.Now                
                loc.ComboBox2.Select()
                loc.ComboBox2.Focus()
                loc.Show()
                loc.Activate()
                loc.Label5.Text = Format(DateAdd(DateInterval.Second, DateDiff(DateInterval.Second, emp_action_start_time, emp_action_stop_time), CDate("01/01/01 00:00:00")), "HH:mm:ss")
                Dim ds9 As DataSet = mysql_data_str("INSERT INTO `user_information`.`emp_active_hours` (`emp_id`, `login_date`, `start_time`) VALUES ('" & emp_id & "', '" & mysql_date_format(emp_login_date, True) & "', now());  select last_insert_id() ")
                last_active_id = ds9.Tables(0).Rows(0)(0).ToString
                'App_Timer.Enabled = True
            End If


            If e.Reason = Microsoft.Win32.SessionSwitchReason.SessionLogoff Then
                emp_action_start_time = get_server_time()
                mysql_data_str("UPDATE `emp_active_hours` SET end_time=NOW() WHERE id='" & last_active_id & "'")
                mysql_data_str("UPDATE emp_login_log SET logout_comment='User LogOff', end_time='" & mysql_date_format(get_server_time, False) & "' where emp_id='" & emp_id & "' and login_date='" & mysql_date_format(emp_login_date, True) & "';")
                mysql_data_str("update tbl_emp_monitor set updated_on=now(), system_ip_address='', system_name='', current_status='Shift Logout' where emp_id='" & emp_id & "'")
                End
            End If
        Catch ex As Exception
            Err.Clear()
            Exit Sub
        End Try
    End Sub

    Private Function GetWindowTitle(ByVal window_hwnd As Integer) As String
        Try
            ' See how long the window's title is.
            Dim length As Integer
            length = NativeMethodsHome.GetWindowTextLength(window_hwnd) + 1
            If length <= 1 Then
                ' There's no title. Use the hWnd.
                Return "<" & window_hwnd & ">"
            Else
                ' Get the title.
                Dim buf As String = Space$(length)
                length = NativeMethodsHome.GetWindowText(window_hwnd, buf, length)
                Return buf.Substring(0, length)
            End If
        Catch ex As Exception
            Err.Clear()
            Return 0
        End Try
    End Function
    Function check_application_running_from_server() As Boolean
        'MsgBox(Application.StartupPath.ToString)
        If Application.StartupPath.ToString.Contains("172.16.0.5") = True Then
            Return True
        End If
        Return False
    End Function
    Public Sub Handler_SessionEnding(ByVal sender As Object, _
               ByVal e As Microsoft.Win32.SessionEndingEventArgs)

    
        If e.Reason = Microsoft.Win32.SessionEndReasons.Logoff Then
            'sw.WriteLine(Environment.UserName & ":" & emp_action_start_time & ": Logoff the System")
            mysql_data_str("UPDATE `emp_active_hours` SET end_time=NOW() WHERE id='" & last_active_id & "'")
            mysql_data_str("Update emp_login_log set logout_comment='User Logout', end_time='" & mysql_date_format(get_server_time, False) & "' where emp_id='" & emp_id & "' and login_date='" & mysql_date_format(emp_login_date, True) & "';")
            mysql_data_str("update tbl_emp_monitor set updated_on=now(), system_ip_address='', system_name='', current_status='Shift Logout' where emp_id='" & emp_id & "'")
            'mysql_data_str("update application_tracking_log set End_Time = now() where Emp_ID = '" & emp_id & "' and End_Time is null order by id desc limit 1")
            'update_document(emp_id, get_server_time(), app_start_time)
            End
        ElseIf e.Reason = Microsoft.Win32.SessionEndReasons.SystemShutdown Then
            'sw.WriteLine(Environment.UserName & ":" & emp_action_start_time & ": System Shutdown")
        End If
    End Sub
    Function machine_available(temp_machine As String) As Boolean
        Dim ds1 As DataSet
        ds1 = mysql_data_str("select system_name from tbl_emp_monitor where system_name='" & Trim(temp_machine) & "' and current_status='Active'")
        If ds1.Tables(0).Rows.Count = 0 Then
            Return True
        End If
        Return False
    End Function
    Private Sub veeBOT_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Dim myNA() As NetworkInterface = NetworkInterface.GetAllNetworkInterfaces
            'MsgBox(myNA(0).Speed)

            'VisionIM.Show()

            AddHandler Microsoft.Win32.SystemEvents.SessionEnding, _
               AddressOf Handler_SessionEnding

            If check_application_running_from_server() = True Then
                MsgBox("Kindly run the application from Local Desktop.")
                End
            End If
            AddHandler SystemEvents.SessionSwitch, AddressOf SystemEvents_Sessionswitch
            If checkConnection("select now()") = False Then
                MsgBox("Unable to reach the Server (172.29.2.68). " & vbCrLf & vbCrLf & " Contact IT Support / IT Helpdesk to fix the Issue", MsgBoxStyle.Exclamation)
                End
            End If
            Dim ds1 As DataSet = mysql_data_str("select now()")
            Label1.Text = CDate(ds1.Tables(0).Rows(0)(0).ToString)
            DBTime.Enabled = True

            emp_system_name = Environment.UserName
            'emp_system_name = "Selvam"
            'emp_system_name = "VRCM2309"


            Me.Icon = My.Resources.favicon
            emp_login_time = get_server_time()
            machine_name = Environment.MachineName

            load_my_detail(emp_system_name)
            emp_ip_address = h.AddressList.GetValue(0).ToString()
            If emp_ip_address = "172.16.3.3" Or emp_ip_address = "172.16.3.4" Or emp_ip_address = "172.16.3.5" Or emp_ip_address = "172.16.3.6" Or emp_ip_address = "172.29.3.8" Then
machine_no:     Dim tmp_machine As String = InputBox("Kindly Enter the BAY Number ", "Location Tracker")
                If tmp_machine.Length < 5 Then
                    MsgBox("Please enter Valid Bay Number")
                    GoTo machine_no
                End If
                If tmp_machine.Contains("LB-") = False Then
                    MsgBox("Please enter Valid Bay Number." & vbCrLf & "Format should be: Ex. LB-F7-154")
                    GoTo machine_no
                End If

                If machine_available(tmp_machine) = False Then
                    MsgBox("BAY number entered by other user." & vbCrLf & "Format should be: Ex. LB-F7-154")
                    GoTo machine_no
                End If

                'If machine_available(tmp_machine) = False Then
                '    MsgBox("BAY number entered by other user." & vbCrLf & "Format should be: Ex. LB-F7-154")
                '    GoTo machine_no
                'End If


                machine_name = tmp_machine
            End If
            Call System_info_update(emp_ip_address)
            emp_sys_name = Microsoft.VisualBasic.LCase(emp_sys_name)
            emp_login_log()
            If emp_access_level <> 6 Or emp_id = 2355 Then
                VMessageToolStripMenuItem.Visible = True
            Else
                VMessageToolStripMenuItem.Visible = False
            End If
            sbar_ist.Text = get_server_time()
            IST_Time_Update()
            messenger_activity_id = 1
            NotifyIcon1.Visible = True
            NotifyIcon1.Icon = My.Resources.favicon
            NotifyIcon1.BalloonTipIcon = ToolTipIcon.Info
            NotifyIcon1.BalloonTipTitle = current_version
            Me.NotifyIcon1.Text = current_version
            NotifyIcon1.BalloonTipText = "powered by VRCM-AppDev"
            NotifyIcon1.ShowBalloonTip(50000)
            ShowInTaskbar = False
            Me.FormBorderStyle = FormBorderStyle.SizableToolWindow
            ' Call SetWindowLong(Me.Handle, GWL_EXSTYLE, WS_EX_TOOLWINDOW)
            Me.Opacity = 0
            Timer1.Enabled = True
            'Remainder.Enabled = True
            App_Timer.Enabled = True
            'sec_refresh.Enabled = True
            'NullAppUpdater.Enabled = True
            update_my_data()

            ' ''Lite Version
            'If version_checking() = False Then
            '    MsgBox("Invalid Version. Application will be closed and Reopen.")
            '    If System.IO.File.Exists("\\vrcm-ops\vfs\vBOT-App\setup.exe") = True Then
            '        Process.Start("\\vrcm-ops\vfs\vBOT-App\setup.exe")
            '    ElseIf System.IO.File.Exists("\\172.29.2.68\vfs\vBOT-App\setup.exe") = True Then
            '        Process.Start("\\172.29.2.68\vfs\vBOT-App\setup.exe")
            '    Else
            '        'If System.IO.File.Exists("\\vrcm-ops\vfs\vBOT-App\EMS_Shortcut.lnk") = True Then
            '        '    Process.Start("\\vrcm-ops\vfs\vBOT-App\EMS_Shortcut.lnk")
            '        'ElseIf System.IO.File.Exists("\\172.29.2.68\vfs\vBOT-App\EMS_Shortcut.lnk") = True Then
            '        '    Process.Start("\\172.29.2.68\vfs\vBOT-App\EMS_Shortcut.lnk")
            '        'Else
            '        '    If System.IO.File.Exists("C:\Users\" & Environment.UserName & "\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Visionary RCM\EMS\vBOT.appref-ms") = True Then
            '        '        Process.Start("C:\Users\" & Environment.UserName & "\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Visionary RCM\EMS\vBOT.appref-ms")
            '        '    Else
            '        MsgBox("Contact AppDev Team / IT Support. Ext: 3214")
            '        End
            '    End If
            '    'End If
            '    '    End If
            'End If
            If emp_id = 2355 Then
                'NULLUpdaterToolStripMenuItem.Visible = True
                'LogoutValidationToolStripMenuItem.Visible = True
                'ToolStripMenuItem1.Visible = True
                'ViewActivityToolStripMenuItem.Visible = True
                'ActivityPlannerToolStripMenuItem.Visible = True
                HeadcountUploadToolStripMenuItem.Visible = True
                'ActivityPlannerToolStripMenuItem.Visible = True
                'ViewActivityToolStripMenuItem.Visible = True
                VMessageToolStripMenuItem.Visible = True
                LogOffToolStripMenuItem.Visible = True
                ReportingToolStripMenuItem.Visible = True
                TLAccessToolStripMenuItem.Visible = True
                ToolStripMenuItem2.Visible = True
            End If

            hlm_messenger_T = New Thread(AddressOf Me.hlk_Messenger)
            hlm_messenger_T.Start()
            idle_break_data_set = mysql_data_str("select * from break_types where id=3 order by id desc")
            sub_break_data_set = mysql_data_str("select * from break_sub_types where active=0 order by break_type asc")
            normal_break_data_set = mysql_data_str("select * from break_types where id<>3 and active=0 order by break_type asc")
            emp_active_start = get_server_time()
            Dim ds9 As DataSet = mysql_data_str("INSERT INTO `user_information`.`emp_active_hours` (`emp_id`, `login_date`, `start_time`) VALUES ('" & emp_id & "', '" & mysql_date_format(emp_login_date, True) & "', now()); select last_insert_id()")
            last_active_id = ds9.Tables(0).Rows(0)(0).ToString
            appDgv = New DataGridView
            appDgv.Columns.Clear()
            appDgv.Rows.Clear()
            appDgv.Columns.Add("Emp_ID", "Emp_ID")
            appDgv.Columns.Add("Login_Date", "Login_Date")
            appDgv.Columns.Add("Start_Time", "Start_Time")
            appDgv.Columns.Add("End_Time", "End_Time")
            appDgv.Columns.Add("AppName", "AppName")
            appDgv.Columns.Add("AppNamegroup", "AppNamegroup")
            appDgv.Columns.Add("Comments", "Comments")


            BreakDgv = New DataGridView
            BreakDgv.Columns.Clear()
            BreakDgv.Rows.Clear()
            BreakDgv.Columns.Add("Emp_ID", "Emp_ID")
            BreakDgv.Columns.Add("Login_Date", "Login_Date")
            BreakDgv.Columns.Add("Start_Time", "Start_Time")
            BreakDgv.Columns.Add("End_Time", "End_Time")
            BreakDgv.Columns.Add("reason", "reason")
            BreakDgv.Columns.Add("breaktype", "breaktype")


            loc.DataGridView1.Columns.Clear()
            loc.DataGridView1.Rows.Clear()
            loc.DataGridView1.Columns.Add("Emp_ID", "Emp_ID")
            loc.DataGridView1.Columns.Add("Login_Date", "Login_Date")
            loc.DataGridView1.Columns.Add("Start_Time", "Start_Time")
            loc.DataGridView1.Columns.Add("End_Time", "End_Time")
            loc.DataGridView1.Columns.Add("AppName", "AppName")
            loc.DataGridView1.Columns.Add("AppNamegroup", "AppNamegroup")


            'Process.Start("\\172.29.2.68\vfs\vBOT-App\EMS_Finder.exe")
        Catch ex As Exception
            Err.Clear()
            End
            'Exit Sub
        End Try

    End Sub
    Function version_checking() As String
        Try
            Dim ds1 As DataSet
            ds1 = mysql_data_str("select version from tool_version_log order by id desc limit 1")
            If ds1.Tables(0).Rows(0)(0).ToString = current_version Then
                Return True
            End If
            Return False
        Catch ex As Exception
            Err.Clear()
            Return False
        End Try
    End Function
    Sub update_my_data()
        Try
            Dim ds1 As DataSet
            ds1 = mysql_data_str("SELECT `project_group_id` FROM `log_emp_project` WHERE emp_id='" & emp_id & "' AND start_date <= '" & mysql_date_format(emp_login_date, True) & "' AND end_date > '" & mysql_date_format(emp_login_date, True) & "'")
            If ds1.Tables(0).Rows.Count <> 0 Then
                emp_project_group = ds1.Tables(0).Rows(0)(0)
                mysql_data_str("UPDATE tbl_emp_monitor SET cost_center =(SELECT `cost_center_id` FROM `log_emp_cost_center` WHERE emp_id='" & emp_id & "' AND start_date <= '" & mysql_date_format(emp_login_date, True) & "' AND end_date > '" & mysql_date_format(emp_login_date, True) & "') WHERE emp_id='" & emp_id & "'; UPDATE `tbl_emp_monitor` SET department =(SELECT `department_id` FROM `log_emp_department` WHERE emp_id='" & emp_id & "' AND start_date <= '" & mysql_date_format(emp_login_date, True) & "' AND end_date > '" & mysql_date_format(emp_login_date, True) & "') WHERE emp_id='" & emp_id & "'; UPDATE `tbl_emp_monitor` SET designation =(SELECT `designation_id` FROM `log_emp_designation` WHERE emp_id='" & emp_id & "' AND start_date <= '" & mysql_date_format(emp_login_date, True) & "' AND end_date > '" & mysql_date_format(emp_login_date, True) & "') WHERE emp_id='" & emp_id & "'; UPDATE `tbl_emp_monitor` SET assigned_process =(SELECT `project_id` FROM `log_emp_process` WHERE emp_id='" & emp_id & "' AND start_date <= '" & mysql_date_format(emp_login_date, True) & "' AND end_date > '" & mysql_date_format(emp_login_date, True) & "') WHERE emp_id='" & emp_id & "'; UPDATE `tbl_emp_monitor` SET project_group =(SELECT `project_group_id` FROM `log_emp_project` WHERE emp_id='" & emp_id & "' AND start_date <= '" & mysql_date_format(emp_login_date, True) & "' AND end_date > '" & mysql_date_format(emp_login_date, True) & "') WHERE emp_id='" & emp_id & "'; UPDATE `tbl_emp_monitor` SET `reporting1` =(SELECT `reporting_id1` FROM `log_emp_reporting1` WHERE emp_id='" & emp_id & "' AND start_date <= '" & mysql_date_format(emp_login_date, True) & "' AND end_date > '" & mysql_date_format(emp_login_date, True) & "') WHERE emp_id='" & emp_id & "'; UPDATE `tbl_emp_monitor` SET reporting2 =(SELECT `reporting_id2` FROM `log_emp_reporting2` WHERE emp_id='" & emp_id & "' AND start_date <= '" & mysql_date_format(emp_login_date, True) & "' AND end_date > '" & mysql_date_format(emp_login_date, True) & "') WHERE emp_id='" & emp_id & "'; UPDATE `tbl_emp_monitor` a, (SELECT `Shift_In`, `Shift_out` FROM `log_shift_roaster` WHERE emp_id='" & emp_id & "' AND start_from <= '" & mysql_date_format(emp_login_date, True) & "' AND end_on > '" & mysql_date_format(emp_login_date, True) & "') b SET a.`Shift_In`=b.Shift_In, a.`Shift_out`=b.Shift_out  WHERE emp_id='" & emp_id & "';")
            End If
        Catch ex As Exception
            Err.Clear()
            Exit Sub
        End Try
    End Sub
    Sub emp_login_log()
        Try
            Dim ds1 As DataSet
            ds1 = mysql_data_str("SELECT emp_id FROM emp_login_log WHERE emp_id='" & emp_id & "' AND login_date='" & mysql_date_format(emp_login_date, True) & "'")
            If ds1.Tables(0).Rows.Count = 0 Then
                mysql_data_str("INSERT INTO emp_login_log (emp_id, login_date, start_time, end_time, system_ip_address, bay_name) VALUES ('" & emp_id & "','" & mysql_date_format(emp_login_date, True) & "','" & mysql_date_format(emp_login_time, False) & "','" & mysql_date_format(emp_login_time, False) & "','" & emp_ip_address & "','" & machine_name & "');")
                mysql_data_str("INSERT into `application_tracking_log` (emp_id, `login_date`, `start_time`, `end_time`, `appname`, `AppNameGroup`) VALUE ('" & emp_id & "','" & mysql_date_format(emp_login_date, True) & "',NOW(),NOW(),'Login','Login')")
            End If

            mysql_data_str("UPDATE `emp_restart_activity_log` SET relogin=now() WHERE emp_id='" & emp_id & "' AND login_date='" & mysql_date_format(emp_login_date, True) & "';")

            If monitor_table_status = 0 Then
                mysql_data_str("INSERT INTO `user_information`.`tbl_emp_monitor` (`emp_id`, `login_date`, `system_ip_address`, `current_status`, `reporting1`, `reporting2`, `assigned_process`, `department`, `designation`, system_name) VALUES ('" & emp_id & "', '" & mysql_date_format(emp_login_date, True) & "', '" & emp_ip_address & "', 'Active', '', '', '', '', '','" & machine_name & "'); ")
                mysql_data_str("update mas_emp_detail set monitor_tbl_status=1 where emp_id='" & emp_id & "'")
            Else
                mysql_data_str("update tbl_emp_monitor set updated_on=now(), system_name='" & machine_name & "', login_date='" & mysql_date_format(emp_login_date, True) & "', system_ip_address='" & emp_ip_address & "', current_status='Active', current_version='" & current_version & "' where emp_id='" & emp_id & "'")
            End If
        Catch ex As Exception
            Err.Clear()
            Exit Sub
        End Try
    End Sub
    Private Function ValidateActiveDirectoryLogin(ByVal Domain As String, ByVal Username As String, ByVal Password As String) As Boolean
        Dim Success As Boolean = False
        Dim Entry As New System.DirectoryServices.DirectoryEntry("LDAP://" & Domain, Username, Password)
        Dim Searcher As New System.DirectoryServices.DirectorySearcher(Entry)
        Searcher.SearchScope = DirectoryServices.SearchScope.OneLevel
        Try
            Dim Results As System.DirectoryServices.SearchResult = Searcher.FindOne
            Success = Not (Results Is Nothing)
        Catch
            Success = False
        End Try
        Return Success
    End Function
    Sub load_my_detail(emp_system_name As String)
        Try
            Dim ds1 As DataSet
            ds1 = mysql_data_str("select * from mas_emp_detail where emp_system_name='" & emp_system_name & "'")
            If ds1.Tables(0).Rows.Count = 0 Then
                MsgBox("Your Login Account not found. So feed your VRCM Details to continue")
                Dim tempId As String = InputBox("Enter your VRCM Emp ID", "VRCM Login ID")
                Dim temppwd As String = InputBox("Enter your VRCM System Password", "VRCM Login Password")
                If ValidateActiveDirectoryLogin("172.16.0.11:3268", tempId, temppwd) = True Then
                    emp_system_name = tempId
                    GoTo EmpOk
                Else
                    If ValidateActiveDirectoryLogin("172.16.0.15:3268", tempId, temppwd) = True Then
                        emp_system_name = tempId
                        GoTo EmpOk
                    Else
                        MsgBox("Your Credentials are mismatch. Please try again." & vbCrLf & vbCrLf & "Contact: AppDev Team. Ext 3214.", MsgBoxStyle.Critical)
                        End
                    End If
                End If

            End If

EmpOk:          ds1 = mysql_data_str("select * from mas_emp_detail where emp_system_name='" & emp_system_name & "' and emp_status=0")
            If ds1.Tables(0).Rows.Count = 0 Then
                MsgBox("You are not Authorized." & vbCrLf & vbCrLf & "Contact: AppDev Team. Ext 3214.", MsgBoxStyle.Information)
                End
            End If

                emp_id = ds1.Tables(0).Rows(0).Item("emp_id").ToString
                emp_name = ds1.Tables(0).Rows(0).Item("emp_first_name").ToString & " " & ds1.Tables(0).Rows(0).Item("emp_last_name").ToString
                emp_access_level = ds1.Tables(0).Rows(0).Item("emp_access_level").ToString
                emp_Track_App = ds1.Tables(0).Rows(0).Item("emp_Track_App").ToString

                system_lock_secs = ds1.Tables(0).Rows(0).Item("system_lock_secs").ToString
                monitor_table_status = ds1.Tables(0).Rows(0).Item("monitor_tbl_status").ToString
                emp_location = ds1.Tables(0).Rows(0).Item("work_location_id").ToString
                shift_type = Val(ds1.Tables(0).Rows(0).Item("shift_type").ToString)

                If shift_type = 0 Then
                    emp_login_date = CDate(emp_login_time)
                Else
                    If Format(emp_login_time, "HH:mm:ss") < "11:00:00" Then
                        If get_last_action_time(emp_id) > 4 Then

                            If Format(emp_login_time, "HH:mm:ss") < "05:00:00" Then
                                emp_login_date = DateAdd(DateInterval.Day, -1, CDate(emp_login_time))
                            Else
                                emp_login_date = CDate(emp_login_time)
                            End If
                        Else
                            emp_login_date = DateAdd(DateInterval.Day, -1, CDate(emp_login_time))
                        End If
                    Else
                        emp_login_date = CDate(emp_login_time)
                    End If
                End If

        Catch ex As Exception
            Err.Clear()
            If shift_type = 0 Then
                emp_login_date = CDate(emp_login_time)
            Else
                If Format(emp_login_time, "HH:mm:ss") < "11:00:00" Then
                    If get_last_action_time(emp_id) > 4 Then
                        emp_login_date = CDate(emp_login_time)
                    Else
                        emp_login_date = DateAdd(DateInterval.Day, -1, CDate(emp_login_time))
                    End If
                Else
                    emp_login_date = CDate(emp_login_time)
                End If
            End If
            Exit Sub
        End Try
    End Sub

    Private Sub veeBOT_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        If Me.WindowState = FormWindowState.Minimized Then
            NotifyIcon1.Visible = True
            NotifyIcon1.Icon = My.Resources.favicon
            NotifyIcon1.BalloonTipIcon = ToolTipIcon.Info
            NotifyIcon1.BalloonTipTitle = "Communicator"
            NotifyIcon1.BalloonTipText = "powered by VRCM-AppDev"
            NotifyIcon1.ShowBalloonTip(50000)
            'Me.Hide()
            ShowInTaskbar = False
        End If
    End Sub

    Sub Check_System_Lock_Time()
        Try
            If Val(it.IdleTime) > system_lock_secs Then
                If messenger_activity_id = 1 Then
                    emp_action_start_time = get_server_time()
                    mysql_data_str("update tbl_emp_monitor set updated_on=now(), current_status='Idle Time' where emp_id='" & emp_id & "'")
                    auto_lock_or_manual_lock = 1
                    'LockWorkStation()
                    loc.Activate()
                    loc.Show()
                End If
            End If
        Catch ex As Exception
            Err.Clear()
            Exit Sub
        End Try
    End Sub

    Private Sub sec_refresh_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles sec_refresh.Tick
        'auto_sign_out()
        sbar_ist.Text = Format(DateAdd(DateInterval.Second, 1, CDate(sbar_ist.Text)), "HH:mm:ss")
        On Error Resume Next
        lbl_lock.Text = (Val(system_lock_secs) - Val(it.IdleTime))

        If lbl_lock.Text = "20" Then
            NativeMethods.Show()
            NativeMethods.Activate()
        End If

        'Call Check_System_Lock_Time()


        Dim sec_rep As String = Format(TimeValue(sbar_ist.Text), "ss")
        If shift_type = 0 And Format(Now(), "HH:mm:ss") = "03:00:00" Then
            If get_last_action_time(emp_id) > 4 Then
                mysql_data_str("update tbl_emp_monitor set updated_on=now(), system_ip_address='', system_name='', current_status='Shift Logout' where emp_id='" & emp_id & "'")
                End
            End If
        ElseIf shift_type = 1 And Format(Now(), "HH:mm:ss") = "11:00:00" Then
            If get_last_action_time(emp_id) > 4 Then
                mysql_data_str("update tbl_emp_monitor set updated_on=now(), system_ip_address='', system_name='', current_status='Shift Logout' where emp_id='" & emp_id & "'")
                End
            End If
        End If       
    End Sub

    Sub Manage_System_Logoff_Event()

        If emp_current_action = "System Locked" Then
            Call system_unlock()
        End If
    End Sub



    Sub application_locked()
        Call update_emp_activity(2)
        If emp_current_action = "" Then
            emp_current_action = "Idle Time"
            Call system_lock()
        End If
    End Sub

    Sub application_unlocked()
        Call update_emp_activity(1)
        'Call update_emp_activity(1)
        Call IST_Time_Update()
        If emp_current_action = "Break" Then
            'Call child_break.Break_Stop()
        ElseIf emp_current_action = "System Locked" Then
            Call system_unlock()
        End If
    End Sub

    Sub IST_Time_Update()
        Try
            counter1.Current_Time = get_server_time()
            sbar_ist.Text = Format(TimeValue(counter1.Current_Time), "hh:mm:ss %t'M'")
        Catch ex As Exception
            Err.Clear()
            Exit Sub
        End Try
    End Sub

    Sub system_lock()
        Try
            If Session_Check() = True Then
                emp_action_start_time = get_server_time()
                Call action_track("NA", "Idle Time")
            Else
                'Call session_expired()
                Exit Sub
            End If
        Catch ex As Exception
            emp_current_action = ""
            MsgBox("System Lock action error. Error code: " & Err.Number & vbCrLf & "Error Description: " & Err.Description)
            Err.Clear()
            Exit Sub
        End Try

    End Sub

    Sub system_unlock()
        Try

            Dim temp_Break_id As Integer = 11
            mysql_data_str("insert into break_event_log (Emp_id, Login_Date, Start_Time, End_Time, reason, break_type) values ('" & emp_id & "', '" & mysql_date_format(DateValue(emp_login_date), True) & "', '" & mysql_date_format(emp_action_start_time, False) & "', now(), '', '" & temp_Break_id & "')")
            Call action_track("NA", "Active")
        Catch ex As Exception
            MsgBox("System unLock action error. Error code: " & Err.Number & vbCrLf & "Error Description: " & Err.Description)
            Err.Clear()
            Exit Sub
        End Try
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Try
            If system_lock_secs = lbl_lock.Text Then
                keyboard_check = keyboard_check + 1
            Else
                keyboard_check = 0
            End If
            'MsgBox(keyboard_check)
            If keyboard_check = 600 Then
                mysql_data_str("INSERT INTO `user_information`.`tbl_internal_messenger` (`login_date`, `sent_by`, `received_by`, `message_content`, `sent_on`) VALUES ('" & mysql_date_format(emp_login_date, True) & "', '" & emp_id & "', (SELECT reporting1 FROM `tbl_emp_monitor` WHERE emp_id='" & emp_id & "'), 'Auto Generated Malfunction Message - Check the System!', '" & mysql_date_format(get_server_time(), False) & "'); ")
            End If

            'load_alert()
            'check_new_message()
        Catch ex As Exception
            Err.Clear()
            Exit Sub
        End Try

    End Sub
    Sub load_alert()
        Try
            Dim ds1 As DataSet
            If emp_id = 2355 Then
                ds1 = mysql_data_str("SELECT b.Emp_first_name, c.break_type FROM break_event_log a LEFT JOIN mas_emp_detail b ON b.emp_id=a.emp_id LEFT JOIN `break_types` c ON c.id=a.break_type LEFT JOIN tbl_emp_monitor d ON d.emp_id=a.emp_id WHERE end_time between ADDTIME(NOW(),'-00:00:01') and NOW() ")
            Else
                ds1 = mysql_data_str("SELECT b.Emp_first_name, c.break_type FROM break_event_log a LEFT JOIN mas_emp_detail b ON b.emp_id=a.emp_id LEFT JOIN `break_types` c ON c.id=a.break_type LEFT JOIN tbl_emp_monitor d ON d.emp_id=a.emp_id WHERE d.reporting1='" & emp_id & "' AND end_time between ADDTIME(NOW(),'-00:00:01') and NOW() ")
            End If
            If ds1.Tables(0).Rows.Count <> 0 Then
                NotifyIcon1.Visible = True
                NotifyIcon1.Icon = My.Resources.favicon
                NotifyIcon1.BalloonTipIcon = ToolTipIcon.Info
                NotifyIcon1.BalloonTipTitle = ds1.Tables(0).Rows(0)(0).ToString
                Me.NotifyIcon1.Text = current_version
                NotifyIcon1.BalloonTipText = ds1.Tables(0).Rows(0)(1).ToString
                NotifyIcon1.ShowBalloonTip(30000)
            End If
        Catch ex As Exception
            Err.Clear()
            Exit Sub
        End Try
    End Sub

    Sub check_new_message()
        Try
            Dim ds1 As DataSet
            ds1 = mysql_data_str("SELECT a.sent_by, a.message_content, a.sent_on, concat(b.emp_first_name,' ',b.emp_last_name,'(',emp_system_name,')') 'Name' FROM tbl_internal_messenger a LEFT JOIN mas_emp_detail b ON b.emp_id=a.sent_by where status=0 and received_by='" & emp_id & "'")
            If ds1.Tables(0).Rows.Count = 0 Then
                Exit Sub
            End If
            If Messenger.Visible = True Then
                Messenger.DataGridView1.Rows.Add("", ds1.Tables(0).Rows(0)(1).ToString)
                Messenger.DataGridView1.Rows(Messenger.DataGridView1.Rows.Count - 1).Cells(1).Style.ForeColor = Color.Blue
                Messenger.DataGridView1.Rows.Add("", ds1.Tables(0).Rows(0)(2).ToString)
                Messenger.DataGridView1.Rows(Messenger.DataGridView1.Rows.Count - 2).Cells(1).Style.ForeColor = Color.Blue
                mysql_data_str("update tbl_internal_messenger set status=1 where received_by='" & emp_id & "'")
            Else
                Messenger.DataGridView1.Rows.Add("", ds1.Tables(0).Rows(0)(1).ToString)
                Messenger.DataGridView1.Rows(Messenger.DataGridView1.Rows.Count - 1).Cells(1).Style.ForeColor = Color.Blue
                Messenger.DataGridView1.Rows.Add("", ds1.Tables(0).Rows(0)(2).ToString)
                Messenger.DataGridView1.Rows(Messenger.DataGridView1.Rows.Count - 2).Cells(1).Style.ForeColor = Color.Blue
                Messenger.Text = ds1.Tables(0).Rows(0)(3).ToString
                Messenger.Show()
                Messenger.Activate()
                Messenger.TopMost = True
                mysql_data_str("update tbl_internal_messenger set status=1 where received_by='" & emp_id & "'")
            End If
            If ds1.Tables(0).Rows(0)(1).ToString = "Kindly lock your system while unattended" Then
                NativeMethods.Show()
                NativeMethods.activate_lock_screen()
            End If
            If ds1.Tables(0).Rows(0)(1).ToString = "Release" Then
                loc.Button2.Visible = True
            End If
        Catch ex As Exception
            Err.Clear()
            Exit Sub
        End Try
    End Sub
    Private Sub VMessageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VMessageToolStripMenuItem.Click
        Communicator.Show()
        Communicator.Activate()
    End Sub

    Private Sub LogOffToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LogOffToolStripMenuItem.Click
        Try
            If checkConnection("select now()") = True Then
                'end_time_updater()
                mysql_data_str("Update emp_login_log set logout_comment='App Logout', end_time='" & mysql_date_format(get_server_time, False) & "' where emp_id='" & emp_id & "' and login_date='" & mysql_date_format(emp_login_date, True) & "';")
                mysql_data_str("update tbl_emp_monitor set updated_on=now(), system_ip_address='', system_name='', current_status='Shift Logout' where emp_id='" & emp_id & "'")
                loc.update_break_details()
                mysql_data_str("INSERT into `application_tracking_log` (emp_id, `login_date`, `start_time`, `end_time`, `appname`, `AppNameGroup`) VALUE ('" & emp_id & "','" & mysql_date_format(emp_login_date, True) & "',NOW(),NOW(),'Logout','Logout')")
                MsgBox("Have a Nice Day!", MsgBoxStyle.Information, "VRCM - AppDev Team")
                'mysql_data_str("update application_tracking_log set End_Time = now() where Emp_ID = '" & emp_id & "' and End_Time is null order by id desc limit 1")
                'update_document(emp_id, get_server_time(), app_start_time)
                End
            Else
                MsgBox("Server Connection missing. Please connect NetExtender/vDESK to save your Data.")
                Exit Sub
            End If
        Catch ex As Exception
            Err.Clear()
            End
        End Try

    End Sub

    Private Sub hlk_thread()
        Try

            Dim j As Integer = 1
            Do Until j = 10
                Sleep(10)
                'Dim aDelegate As New myDelegate(AddressOf Hyper_link_marque)
                ' Me.Invoke(aDelegate)

            Loop
        Catch ex As Exception
            MsgBox(Err.Description)
            Err.Clear()
            Exit Sub
        End Try
    End Sub



    Sub IST_Time_Refresh()
        Try
            sbar_ist.Text = Format(TimeValue(counter1.Current_Time), "hh:mm:ss %t'M'")
        Catch ex As Exception
            Err.Clear()
            sbar_ist.Text = Format(TimeValue(Now()), "hh:mm:ss %t'M'")
        End Try
    End Sub


    Private Sub hlk_IST_Time()
        Try

            Dim j As Integer = 1
            Do Until j = 10
                Sleep(1000)
                Dim aDelegate As New myDelegate(AddressOf IST_Time_Refresh)
                Me.Invoke(aDelegate)

            Loop
        Catch ex As Exception
            MsgBox(Err.Description)
            Err.Clear()
            Exit Sub
        End Try
    End Sub

    'Sub Messenger_Refresh()
    '    Try
    '        If messenger_activity_id = 1 Then
    '            ' Call get_messenger_message()
    '            Call check_new_message()
    '        End If
    '    Catch ex As Exception
    '        Err.Clear()
    '        sbar_ist.Text = Format(TimeValue(Now()), "hh:mm:ss %t'M'")
    '    End Try
    'End Sub

    Private Sub hlk_Messenger()
        Try

            Dim j As Integer = 1
            Do Until j = 10
                Sleep(120000) '10000-10 seconds 60000 - 1 min
                Dim aDelegate As New myDelegate(AddressOf Messenger_Refresh)
                Me.Invoke(aDelegate)
            Loop
        Catch ex As Exception
            MsgBox(Err.Description)
            Err.Clear()
            Exit Sub
        End Try
    End Sub

    Private Sub App_Timer_Tick(sender As Object, e As EventArgs) Handles App_Timer.Tick
        Try
            If emp_Track_App = 1 Then
                Dim fg_hwnd As Long = NativeMethodsHome.GetForegroundWindow()
                If last_title = GetWindowTitle(fg_hwnd) Then Exit Sub
                NativeMethodsHome.GetForegroundWindow.ToString()
                If last_title <> "" Then
                    'Update_Tracking_Data(last_title)
                End If
                Insert_Tracking_Data(GetWindowTitle(fg_hwnd), NativeMethodsHome.GetForegroundWindow.ToString())
                last_title = GetWindowTitle(fg_hwnd)
            End If
        Catch ex As Exception
            Err.Clear()
            Exit Sub
        End Try
    End Sub


    Sub Insert_Tracking_Data(ByVal app_Str As String, app_group_name As String)
        Try
            If app_Str = "" Then
                Exit Sub
            End If
            Dim app_str1 As String = Trim_Replace(app_Str)
            app_str1 = Microsoft.VisualBasic.Left(app_str1, 250)
            Dim split_str As String() = app_str1.Split("-")
            Dim app_process_name As String = Nothing
            'MsgBox(split_str.Length)
            If split_str.Length = 1 Then
                app_process_name = split_str(0).ToString
            ElseIf split_str.Length = 2 Then
                app_process_name = split_str(1).ToString
            ElseIf split_str.Length = 3 Then
                app_process_name = split_str(2).ToString
            Else
                app_process_name = split_str(split_str.Length - 1).ToString
            End If
            ''---- for Low Internet---
            'app_start_time = get_server_time()
            'Dim ds1 As DataSet
            'ds1 = mysql_data_str("UPDATE application_tracking_log SET end_time='" & mysql_date_format(app_start_time, False) & "' WHERE id='" & last_app_id & "'; Insert into application_tracking_log (Emp_ID, Login_Date, Start_Time, End_Time, AppName, AppNamegroup) values ('" & emp_id & "', '" & mysql_date_format(emp_login_date, True) & "', '" & mysql_date_format(app_start_time, False) & "', null, '" & app_str1 & "','" & app_process_name & "'); SELECT LAST_INSERT_ID()")
            'last_app_id = ds1.Tables(0).Rows(0).Item(0).ToString

            '---- for Low Internet---
            app_start_time = CDate(Label1.Text)
            If appDgv.Rows.Count = 1 Then
                appDgv.Rows.Add(emp_id, emp_login_date, app_start_time, "", app_str1, app_process_name)
                loc.DataGridView1.Rows.Add(emp_id, emp_login_date, app_start_time, "", app_str1, app_process_name)
            Else
                'MsgBox(loc.DataGridView1.Rows.Count)
                appDgv.Rows(appDgv.Rows.Count - 1).Cells("end_time").Value = app_start_time
                appDgv.Rows.Add(emp_id, emp_login_date, app_start_time, "", app_str1, app_process_name)
                loc.DataGridView1.Rows(loc.DataGridView1.Rows.Count - 1).Cells("end_time").Value = app_start_time
                loc.DataGridView1.Rows.Add(emp_id, emp_login_date, app_start_time, "", app_str1, app_process_name)
            End If

            'load_conection(emp_id, emp_login_date, app_start_time, app_str1.ToString, app_process_name)
        Catch ex As Exception
            Err.Clear()
            Exit Sub
        End Try
    End Sub
    Public last_app_id As Integer = 0
    Sub Update_Tracking_Data(ByVal app_Str As String)
        Try
            If app_Str = "" Then
                Exit Sub
            End If

            Dim app_str1 As String = Trim_Replace(app_Str)
            app_str1 = Microsoft.VisualBasic.Left(app_str1, 250)

            mysql_data_str("update application_tracking_log set End_Time = now() where Emp_ID = '" & emp_id & "' and AppName = '" & app_str1 & "' and End_Time is null order by id desc limit 1")
            'update_document(emp_id, get_server_time(), app_start_time)
        Catch ex As Exception
            Err.Clear()
            Exit Sub
        End Try
    End Sub

    Private Sub lbl_lock_Click(sender As Object, e As EventArgs) Handles lbl_lock.Click

    End Sub

    Private Sub DOWNTIMEToolStripMenuItem_Click(sender As Object, e As EventArgs)

    End Sub
    Sub check_for_status_change()
        NotifyIcon1.BalloonTipTitle = "Communicator"
        NotifyIcon1.BalloonTipText = "powered by VRCM-AppDev"
    End Sub
    Private Sub ContextMenuStrip1_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip1.Opening

    End Sub


    Private Sub ViewActivityToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ViewActivityToolStripMenuItem.Click
        CurrentActivty.Show()
        '    loc.Show()

    End Sub

    Private Sub NullAppUpdater_Tick(sender As Object, e As EventArgs) Handles NullAppUpdater.Tick
        'Dim ds1 As DataSet
        'ds1 = mysql_data_str("SELECT * FROM application_tracking_log WHERE emp_id=" & emp_id & " AND login_date='" & mysql_date_format(emp_login_date, True) & "' AND end_time IS NULL")
        'If ds1.Tables(0).Rows.Count = 1 Then
        '    Exit Sub
        'End If

        'Dim i As Integer
        'Do Until i = ds1.Tables(0).Rows.Count - 1
        '    mysql_data_str("UPDATE application_tracking_log a, (SELECT MIN(start_time) last_time FROM application_tracking_log WHERE emp_id='" & emp_id & "' AND login_date='" & mysql_date_format(emp_login_date, True) & "' AND id > '" & ds1.Tables(0).Rows(i).Item(0).ToString & "') b SET a.end_time=b.last_time WHERE id='" & ds1.Tables(0).Rows(i).Item(0).ToString & "'")
        '    i = i + 1
        'Loop


    End Sub

    Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem1.Click
        User_Details.Activate()
        User_Details.Show()
    End Sub

    Private Sub LogoutValidationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LogoutValidationToolStripMenuItem.Click
        Try
            Dim logoutdate As String = InputBox("Enter the logout in mm/dd/yyyy")
            If IsDate(logoutdate) = False Then
                Exit Sub
            End If
            Dim i As Integer
            Do Until i = 3500
                Dim ds1 As DataSet
                ds1 = mysql_data_str("SELECT emp_id, MAX(end_time) FROM application_tracking_log WHERE login_date='" & mysql_date_format(CDate(logoutdate), True) & "' and emp_id='" & i & "'")
                If ds1.Tables(0).Rows(0)(0).ToString <> Nothing Then
                    mysql_data_str("update emp_login_log set end_time='" & mysql_date_format(CDate(ds1.Tables(0).Rows(0).Item(1).ToString), False) & "' where login_date='" & mysql_date_format(CDate(logoutdate), True) & "' and emp_id='" & ds1.Tables(0).Rows(0).Item(0).ToString & "'")
                    mysql_data_str("UPDATE tbl_emp_monitor SET current_status='Shift Logout' WHERE login_date<>'" & mysql_date_format(Today, True) & "'")
                End If
                i = i + 1
            Loop

            MsgBox("Logout Updated")
        Catch ex As Exception
            Err.Clear()
            Exit Sub
        End Try

    End Sub

    Private Sub NULLUpdaterToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NULLUpdaterToolStripMenuItem.Click
        Try
            Dim tmp_date As String = InputBox("Enter Date")
            If IsDate(tmp_date) = False Then
                MsgBox("Date Invalid")
                Exit Sub
            End If
            Dim ds1 As DataSet
            ds1 = mysql_data_str("SELECT emp_id FROM emp_login_log WHERE login_date='" & mysql_date_format(CDate(tmp_date), True) & "'")
            'ds1 = mysql_data_str("SELECT * FROM application_tracking_log WHERE end_time IS NULL")
            If ds1.Tables(0).Rows.Count = 1 Then
                Exit Sub
            End If

            Dim i As Integer
            Do Until i = ds1.Tables(0).Rows.Count
                Dim ds5 As DataSet
                ds5 = mysql_data_str("SELECT * FROM application_tracking_log WHERE end_time IS NULL AND emp_id='" & ds1.Tables(0).Rows(i).Item(0).ToString & "' AND login_date='" & mysql_date_format(CDate(tmp_date), True) & "'")
                If ds5.Tables(0).Rows.Count > 1 Then
                    Dim j As Integer = 0
                    Do Until j = ds5.Tables(0).Rows.Count
                        Dim ds6 As DataSet = mysql_data_str("SELECT * FROM application_tracking_log WHERE emp_id='" & ds1.Tables(0).Rows(i).Item(0).ToString & "' AND login_date='" & mysql_date_format(CDate(tmp_date), True) & "' AND id > '" & ds5.Tables(0).Rows(j).Item("id").ToString & "' LIMIT 1")
                        If ds6.Tables(0).Rows.Count > 0 Then
                            mysql_data_str("UPDATE application_tracking_log, (SELECT start_time FROM application_tracking_log WHERE id='" & ds6.Tables(0).Rows(0).Item("id").ToString & "') z SET end_time=z.start_time WHERE id='" & ds5.Tables(0).Rows(j).Item("id").ToString & "'")
                        End If

                        j = j + 1
                    Loop
                End If


                'Dim ds2 As DataSet
                'ds2 = mysql_data_str("SELECT MIN(start_time) last_time FROM application_tracking_log WHERE emp_id='" & ds1.Tables(0).Rows(i).Item("emp_id").ToString & "' AND login_date='" & mysql_date_format(CDate(ds1.Tables(0).Rows(i).Item("login_date").ToString), True) & "' AND id > '" & ds1.Tables(0).Rows(i).Item("id").ToString & "'")
                'If ds2.Tables(0).Rows.Count > 0 Then
                '    'MsgBox(ds2.Tables(0).Rows(0).Item(0).ToString)
                '    If ds2.Tables(0).Rows(0).Item(0).ToString = Nothing Then
                '        mysql_data_str("UPDATE application_tracking_log SET end_time=start_time WHERE id='" & ds1.Tables(0).Rows(i).Item("id").ToString & "'")
                '    Else
                '        mysql_data_str("UPDATE application_tracking_log SET end_time='" & mysql_date_format(CDate(ds2.Tables(0).Rows(0).Item(0).ToString), False) & "' WHERE id='" & ds1.Tables(0).Rows(i).Item("id").ToString & "'")
                '    End If
                'End If
                i = i + 1
            Loop

            MsgBox("Completed")
        Catch ex As Exception
            Err.Clear()
            Exit Sub
        End Try
    End Sub

    Private Sub LaunchDashboardToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LaunchDashboardToolStripMenuItem.Click
        On Error Resume Next
        Process.Start("chrome.exe", "http://172.29.2.68/IntraVision/mydashboard")
        Exit Sub
        On Error Resume Next
        Process.Start("iexplore.exe", "http://172.29.2.68/IntraVision/mydashboard")
    End Sub
    Sub end_time_updater()
        Try
            Dim ds1 As DataSet
            ds1 = mysql_data_str("SELECT * FROM application_tracking_log WHERE emp_id='" & emp_id & "' AND end_time IS NULL and start_time >'2019-03-10 13:07:30' ORDER BY id DESC")
            Dim i As Integer = 1
            Dim prev_time As Date = Nothing
            Do Until i = ds1.Tables(0).Rows.Count
                mysql_data_str("update application_tracking_log set end_time='" & mysql_date_format(CDate(ds1.Tables(0).Rows(i - 1).Item("start_time").ToString), False) & "' where id='" & ds1.Tables(0).Rows(i).Item("id").ToString & "'")
                i = i + 1
            Loop
        Catch ex As Exception
            Err.Clear()
            Exit Sub
        End Try
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        Try
            'end_time_updater()
        Catch ex As Exception
            Err.Clear()
            Exit Sub
        End Try

    End Sub
    Sub auto_sign_out()
        Try
            If emp_id = 2355 Then
                mysql_data_str("UPDATE `break_event_log` SET start_time=end_time WHERE start_time='0001-01-01 00:00:00'; UPDATE `break_event_log` SET end_time=start_time WHERE end_time='0001-01-01 00:00:00'")
            End If

            Dim ds1 As DataSet
            ds1 = mysql_data_str("SELECT updated_on FROM `tbl_emp_monitor` WHERE emp_id='" & emp_id & "'")

            If DateDiff(DateInterval.Minute, CDate(ds1.Tables(0).Rows(0)(0).ToString), get_server_time()) > 240 Then
                'Dim ds2 As New DataSet
                'ds2 = mysql_data_str("(SELECT start_time FROM `application_tracking_log` WHERE emp_id ='" & emp_id & "' AND login_date='" & mysql_date_format(emp_login_date, True) & "' ORDER BY id DESC LIMIT 1)")
                'Dim logouttime As Date = CDate(ds2.Tables(0).Rows(0)(0).ToString)
                'mysql_data_str("Update emp_login_log set logout_comment='Auto Sign out Method. System Updated', end_time='" & mysql_date_format(logouttime, False) & "' where emp_id='" & emp_id & "' and login_date='" & mysql_date_format(emp_login_date, True) & "';")
                'mysql_data_str("update tbl_emp_monitor set updated_on=now(), system_ip_address='', system_name='', current_status='Shift Logout' where emp_id='" & emp_id & "'")
                loc.update_break_details()
                End
            End If
        Catch ex As Exception
            Err.Clear()
            Exit Sub
        End Try
    End Sub

    Private Sub auto_signout_Tick(sender As Object, e As EventArgs) Handles auto_signout.Tick
        Try
            'If shift_type = 0 Then
            '    If Format(get_server_time, "HH:mm:ss") >= "02:00:00" And Format(get_server_time, "HH:mm:ss") <= "02:59:00" Then
            '        auto_sign_out()
            '    End If
            'Else
            '    If Format(get_server_time, "HH:mm:ss") >= "11:00:00" And Format(get_server_time, "HH:mm:ss") <= "11:59:00" Then
            '        auto_sign_out()
            '    End If
            'End If
            auto_sign_out()
        Catch ex As Exception
            Err.Clear()
            Exit Sub
        End Try

    End Sub

    Private Sub ActivityPlannerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ActivityPlannerToolStripMenuItem.Click
        AppProductivity.Show()
        AppProductivity.Activate()
    End Sub

    Private Sub UpdateToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UpdateToolStripMenuItem.Click
        ActivityUpdate.DateTimePicker1.Value = emp_login_date
        ActivityUpdate.DateTimePicker1.Enabled = False
        ActivityUpdate.Show()
        ActivityUpdate.Activate()
    End Sub

    Private Sub ManageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ManageToolStripMenuItem.Click
        ActivityCreate.Show()
        ActivityCreate.Activate()
    End Sub

    Private Sub AssignToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AssignToolStripMenuItem.Click
        ActivityAssign.Show()
        ActivityAssign.Activate()
    End Sub

    Private Sub HeadcountUploadToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HeadcountUploadToolStripMenuItem.Click
        HeadCountUpload.Show()
        HeadCountUpload.Activate()

    End Sub

    Private Sub ReportingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReportingToolStripMenuItem.Click
        reporting_changes.Show()
        reporting_changes.Activate()
    End Sub

    Private Sub ToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem2.Click
        Try
            Dim temp_emp_str As String = InputBox("Enter Employee ID to proceed", "Resignation Update")
            If temp_emp_str.Length > 3 Then
                mysql_data_str("update mas_emp_detail set emp_status=0 where emp_system_name ='" & temp_emp_str & "'")
                MsgBox("Employee de-activated successfully")
            End If
        Catch ex As Exception
            Err.Clear()
            Exit Sub
        End Try
    End Sub

    Private Sub ReleaseKeyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReleaseKeyToolStripMenuItem.Click
        Try
            Dim key_value As String = InputBox("Enter Key to signoff")
            If key_value = "v6cm`p0ook" Then
                End
            End If
        Catch ex As Exception
            Err.Clear()
            Exit Sub
        End Try
    End Sub
    Sub Messenger_Refresh()
        Try
            If messenger_activity_id = 1 Then
                Call get_messenger_message()
            End If
        Catch ex As Exception
            Err.Clear()
            sbar_ist.Text = Format(TimeValue(Now()), "hh:mm:ss %t'M'")
        End Try
    End Sub
    Sub get_messenger_message()
        Try
            Dim da1 As OdbcDataAdapter
            Dim ds1 As DataSet

            ds1 = mysql_data_str("SELECT a.sent_by 'Sent by', a.message_content 'Message', a.sent_on 'Sent Time', concat(b.emp_first_name,' ',b.emp_last_name,'(',emp_system_name,')') 'Sent Name', a.Status FROM tbl_internal_messenger a LEFT JOIN mas_emp_detail b ON b.emp_id=a.sent_by where status=0 and received_by='" & emp_id & "'")

            Dim i As Integer = 0
            Dim j As Integer = 0

            If ds1.Tables(0).Rows.Count = 0 Then
                Exit Sub
            End If

            Do Until i = ds1.Tables(0).Rows.Count
                Dim com_mes_to As Integer = 0
                Dim com_mes_message As String = Nothing
                Dim com_mes_time As String = Nothing
                Dim com_mes_name As String = Nothing
                Dim com_mes_status As Integer = 0

                com_mes_to = Val(ds1.Tables(0).Rows(i).Item("Sent by").ToString())
                com_mes_name = ds1.Tables(0).Rows(i).Item("Sent Name").ToString()
                com_mes_message = ds1.Tables(0).Rows(i).Item("Message").ToString()
                com_mes_time = ds1.Tables(0).Rows(i).Item("Sent Time").ToString()
                com_mes_status = Val(ds1.Tables(0).Rows(i).Item("Status").ToString())

                create_messengers(com_mes_to, com_mes_name)

                Dim color1 As New Color
                If com_mes_status = 3 Then
                    messengers(com_mes_to).DataGridView1.Rows.Add(New String() {"Group Message", com_mes_message})
                    color1 = Color.Red
                Else
                    messengers(com_mes_to).DataGridView1.Rows.Add(New String() {"Message", com_mes_message})
                    color1 = Color.Blue
                End If


                messengers(com_mes_to).DataGridView1.Rows.Add(New String() {"", com_mes_time})


                Dim message_row1 As Integer = messengers(com_mes_to).DataGridView1.Rows.Count - 2
                Dim message_row2 As Integer = messengers(com_mes_to).DataGridView1.Rows.Count - 1
                messengers(com_mes_to).DataGridView1.Rows(message_row1).Cells(0).Style.ForeColor = color1
                messengers(com_mes_to).DataGridView1.Rows(message_row1).Cells(1).Style.ForeColor = color1
                messengers(com_mes_to).DataGridView1.Rows(message_row2).Cells(0).Style.ForeColor = color1
                messengers(com_mes_to).DataGridView1.Rows(message_row2).Cells(1).Style.ForeColor = color1

                Dim row_last As Integer = 0
                row_last = messengers(com_mes_to).DataGridView1.Rows.Count
                row_last = row_last - 1
                messengers(com_mes_to).DataGridView1.CurrentCell = messengers(com_mes_to).DataGridView1(1, row_last)

                If messengers(com_mes_to).DataGridView1.Rows.Count <> 0 Then
                    messengers(com_mes_to).DataGridView1.Columns(0).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                End If

                i = i + 1
            Loop

            mysql_data_str("update tbl_internal_messenger set status=1 where received_by='" & emp_id & "'")

            If ds1.Tables(0).Rows(0)(1).ToString = "Kindly lock your system while unattended" Then
                NativeMethods.Show()
                NativeMethods.activate_lock_screen()
            End If
            If ds1.Tables(0).Rows(0)(1).ToString = "Release" Then
                loc.Button2.Visible = True
            End If

            da1 = Nothing
            ds1 = Nothing

        Catch ex As Exception
            Err.Clear()
            Exit Sub
        End Try
    End Sub

    Private Sub RestartToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RestartToolStripMenuItem.Click
        Try
            mysql_data_str("INSERT INTO `user_information`.`emp_restart_activity_log` (`emp_id`, `login_date`, `updated_on`, `mail_alert`) VALUES ('" & emp_id & "', '" & mysql_date_format(emp_login_date, True) & "', now(), '0'); ")
            If System.IO.File.Exists("\\vrcm-ops\vfs\EMS_lite\setup.exe") = True Then
                Process.Start("\\vrcm-ops\vfs\EMS_lite\setup.exe")
            ElseIf System.IO.File.Exists("\\172.29.2.68\vfs\EMS_lite\setup.exe") = True Then
                Process.Start("\\172.29.2.68\vfs\EMS_lite\setup.exe")
            Else
                MsgBox("Unable to access VFS folder. Contact AppDev Team. Ext: 3214")
            End If
            End
        Catch ex As Exception
            Err.Clear()
            Exit Sub
        End Try
    End Sub

    Private Sub Remainder_Tick(sender As Object, e As EventArgs) Handles Remainder.Tick
        Try
            Dim ds1 As DataSet
            ds1 = mysql_data_str("select * from emp_self_remainder where emp_id='" & emp_id & "' and follow_date='" & mysql_date_format(emp_login_date, True) & "' and follow_time between '" & Format(Now(), "HH:mm") & ":00' and '" & Format(Now(), "HH:mm") & ":59' and active=0")
            If ds1.Tables(0).Rows.Count <> 0 Then
                mysql_data_str("INSERT INTO `user_information`.`tbl_internal_messenger` (`login_date`, `sent_by`, `received_by`, `message_content`, `sent_on`) VALUES ('" & mysql_date_format(emp_login_date, True) & "', '" & emp_id & "','" & emp_id & "' , 'REMINDER: " & ds1.Tables(0).Rows(0)("Task").ToString & "', '" & mysql_date_format(get_server_time(), False) & "'); ")
                mysql_data_str("update emp_self_remainder set active=1 where id='" & ds1.Tables(0).Rows(0)("id").ToString & "'")
            End If
        Catch ex As Exception
            Err.Clear()
            Exit Sub
        End Try
    End Sub

    Private Sub TLAccessToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TLAccessToolStripMenuItem.Click
        Try
            Dim temp_emp_str As String = InputBox("Enter Employee ID to proceed", "Access Update")
            If temp_emp_str.Length > 3 Then
                mysql_data_str("update mas_emp_detail set emp_access_level=4 where emp_system_name ='" & temp_emp_str & "'")
                MsgBox("Employee Access updated successfully")
            End If
        Catch ex As Exception
            Err.Clear()
            Exit Sub
        End Try

    End Sub

    Private Sub ReportingToolStripMenuItem_DisplayStyleChanged(sender As Object, e As EventArgs) Handles ReportingToolStripMenuItem.DisplayStyleChanged

    End Sub
    Private Sub MainInterface_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If e.CloseReason = CloseReason.TaskManagerClosing Then
            MsgBox(e.CloseReason)
        End If
    End Sub

    Private Sub Timer4_Tick(sender As Object, e As EventArgs) Handles Timer4.Tick
        If checkConnection("select now()") = True Then
            loc.update_break_details()
        Else
            MsgBox("Server Connection missing. Please connect NetExtender/vDESK to save your Data.")
        End If
    End Sub

    Private Sub DBTime_Tick(sender As Object, e As EventArgs) Handles DBTime.Tick
        Label1.Text = DateAdd(DateInterval.Second, 1, CDate(Label1.Text))
    End Sub
End Class