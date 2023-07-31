Imports System.Net
Imports System.Runtime.InteropServices
Public Class VisionIM

    Private tmp = IO.Path.Combine(My.Computer.FileSystem.SpecialDirectories.Temp, "snafu.fubar")
    Private Downloading As Boolean = False

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        If Downloading Then Exit Sub
        Downloading = True

        Dim wc As New WebClient
        AddHandler wc.DownloadProgressChanged, AddressOf wc_ProgressChanged
        AddHandler wc.DownloadFileCompleted, AddressOf wc_DownloadDone

        wc.DownloadFileAsync(New Uri("http://cachefly.cachefly.net/100mb.test"), tmp, Stopwatch.StartNew)

    End Sub

    Private Sub wc_DownloadDone(sender As Object, e As System.ComponentModel.AsyncCompletedEventArgs)
        Downloading = False
    End Sub

    Private Sub wc_ProgressChanged(sender As Object, e As DownloadProgressChangedEventArgs)
        lblTime.Text = (e.BytesReceived / (DirectCast(e.UserState, Stopwatch).ElapsedMilliseconds / 1000.0#)).ToString("#")
    End Sub


    'Private Declare Function WTSRegisterSessionNotification Lib "Wtsapi32" (ByVal hWnd As IntPtr, ByVal THISSESS As Integer) As Integer
    'Private Declare Function WTSUnRegisterSessionNotification Lib "Wtsapi32" (ByVal hWnd As IntPtr) As Integer

    'Private Const NOTIFY_FOR_ALL_SESSIONS As Integer = 1
    'Private Const NOTIFY_FOR_THIS_SESSION As Integer = 0
    'Private Const WM_WTSSESSION_CHANGE As Integer = &H2B1

    'Public Class IdleTime
    '    Private Class NativeMethods
    '        <DllImport("user32.dll")> _
    '        Public Shared Function GetLastInputInfo(ByRef plii As LASTINPUTINFO) As Boolean
    '        End Function
    '    End Class

    '    Public Structure LASTINPUTINFO
    '        Public cbSize As UInteger
    '        Public dwTime As UInteger
    '    End Structure

    '    'API call that let us detect any keyboard and/or mouse activity

    '    'Returns the time since last user activity
    '    Public Function GetInactiveTime() As Nullable(Of TimeSpan)
    '        Dim info As LASTINPUTINFO = New LASTINPUTINFO
    '        info.cbSize = CUInt(Marshal.SizeOf(info))
    '        If (NativeMethods.GetLastInputInfo(info)) Then
    '            Return TimeSpan.FromMilliseconds(Environment.TickCount - info.dwTime)
    '        Else
    '            Return Nothing
    '        End If
    '    End Function

    'End Class

    'Private Enum WTS
    '    CONSOLE_CONNECT = 1
    '    CONSOLE_DISCONNECT = 2
    '    REMOTE_CONNECT = 3
    '    REMOTE_DISCONNECT = 4
    '    SESSION_LOGON = 5
    '    SESSION_LOGOFF = 6
    '    SESSION_LOCK = 7
    '    SESSION_UNLOCK = 8
    '    SESSION_REMOTE_CONTROL = 9
    'End Enum
    'Private IdleTimer As IdleTime

    'Shared FileName As String
    'Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    Me.Hide()
    '    WTSRegisterSessionNotification(Me.Handle, NOTIFY_FOR_ALL_SESSIONS)
    '    Timer1.Start()
    '    Me.Hide()
    '    FileName = Application.StartupPath + "\TimeSheet_" + DateTime.Today.ToString("dd-MMM-yyyy") + ".txt"
    '    System.IO.File.Create(FileName)

    '    'check autolock time / 10 minutes default
    '    Dim AutoLockTime As Integer = 600

    '    If AutoLockTime <= 0 Then
    '        InactivityTimer.Enabled = False     'Disabled
    '    Else
    '        AutoLockTime = AutoLockTime + 10    'Plus 10 Seconds Allowance
    '        InactivityTimer.Enabled = True      'Activate Checking of Idle
    '    End If

    'End Sub

    'Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
    '    Select Case m.Msg
    '        Case WM_WTSSESSION_CHANGE
    '            Select Case m.WParam.ToInt32
    '                Case WTS.CONSOLE_CONNECT
    '                    'MessageBox.Show("A session was connected to the console session.")
    '                    Timer1.Start()
    '                    System.IO.File.AppendAllText(FileName, "IN - " + lblTime.Text)
    '                Case WTS.CONSOLE_DISCONNECT
    '                    'MessageBox.Show("A session was disconnected from the console session.")
    '                    System.IO.File.AppendAllText(FileName, "OUT - " + lblTime.Text + vbCrLf)
    '                    Timer1.Stop()
    '                Case WTS.REMOTE_CONNECT
    '                    'MessageBox.Show("A session was connected to the remote session.")
    '                Case WTS.REMOTE_DISCONNECT
    '                    'MessageBox.Show("A session was disconnected from the remote session.")
    '                Case WTS.SESSION_LOGON
    '                    Timer1.Start()
    '                    System.IO.File.AppendAllText(FileName, "IN - " + lblTime.Text)
    '                    'MessageBox.Show("A user has logged on to the session.")
    '                Case WTS.SESSION_LOGOFF
    '                    'MessageBox.Show("A user has logged off the session.")
    '                    System.IO.File.AppendAllText(FileName, "OUT - " + lblTime.Text + vbCrLf)
    '                    Timer1.Stop()
    '                Case WTS.SESSION_LOCK
    '                    'MessageBox.Show("A session has been locked.")
    '                    mysql_data_str("INSERT INTO `user_information`.`tbl_internal_messenger` (`login_date`, `sent_by`, `received_by`, `message_content`, `sent_on`) VALUES ('2019-01-31', '347', '2355', 'New Success', '" & Format(CDate(lblTime.Text), "yyyy-MM-dd hh:mm:ss") & "'); ")
    '                    'Timer1.Stop()
    '                Case WTS.SESSION_UNLOCK
    '                    'MessageBox.Show("A session has been unlocked.")
    '                    'Timer1.Start()
    '                    mysql_data_str("INSERT INTO `user_information`.`tbl_internal_messenger` (`login_date`, `sent_by`, `received_by`, `message_content`, `sent_on`) VALUES ('2019-01-31', '347', '2355', 'New Success', '" & Format(CDate(lblTime.Text), "yyyy-MM-dd hh:mm:ss") & "'); ")
    '                    'If m_clsBalloonEvents Is Nothing Then
    '                    '    'allow our ballon class to create the event class for us, hooking into the notify icon that will 
    '                    '    'be associated with the balloons...
    '                    '    m_clsBalloonEvents = Balloon.HookBalloonEvents(niMain)
    '                    '    'add handlers for the events that we are concerned with in this application...
    '                    '    AddHandler m_clsBalloonEvents.BallonClicked, AddressOf BalloonClicked
    '                    '    AddHandler m_clsBalloonEvents.BallonHidden, AddressOf BalloonHidden
    '                    'End If
    '                    'Balloon.DisplayBalloon(niMain, "Today's Time", lblTime.Text, CType(IIf(False, Balloon.BalloonMessageTypes.Error, Balloon.BalloonMessageTypes.Info), Balloon.BalloonMessageTypes))
    '                Case WTS.SESSION_REMOTE_CONTROL
    '                    MessageBox.Show("A session has changed its remote controlled status. To determine the status, call GetSystemMetrics and check the SM_REMOTECONTROL metric.")
    '            End Select

    '    End Select

    '    MyBase.WndProc(m)
    'End Sub

    'Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
    '    lblTime.Text = Now()
    'End Sub
End Class