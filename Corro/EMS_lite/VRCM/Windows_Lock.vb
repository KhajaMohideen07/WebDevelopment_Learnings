Public Class NativeMethods

    Private Declare Function LockWorkStation Lib "User32.dll" () As Long

    Private Declare Function WTSRegisterSessionNotification Lib "Wtsapi32" (ByVal hWnd As IntPtr, ByVal THISSESS As Integer) As Integer
    Private Declare Function WTSUnRegisterSessionNotification Lib "Wtsapi32" (ByVal hWnd As IntPtr) As Integer
    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, ByVal keyData As Keys) As Boolean
        Const WM_KEYDOWN As Integer = &H100 ' regular key press 
        Const WM_SYSKEYDOWN As Integer = &H104 ' system key press (?) 
        Dim handled As Boolean = False ' we haven't handled this key 
        If (msg.Msg = WM_KEYDOWN) OrElse (msg.Msg = WM_SYSKEYDOWN) Then ' if they are pressing a key 
            Select Case keyData
                ' look at which key they're pressing 
                Case Keys.Escape
                    ' handle the Esc key as if they pressed "Exit" 
                    Me.Close()
                    handled = True
                    ' we handled the key press 
                    Exit Select
                Case Else
                    ' others we don't handle 
                    Exit Select
            End Select
        End If
        Return (handled OrElse MyBase.ProcessCmdKey(msg, keyData))
        ' if we haven't handled this key, pass it to the base 
    End Function


    Sub activate_lock_screen()
        Label1.Text = 10
        Timer1.Enabled = True
    End Sub
    Private Sub cmd_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_close.Click
        Me.Close()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Label1.Text = Val(Label1.Text) - 1
        If Val(Label1.Text) = 0 Then
            LockWorkStation()
            Me.Close()
        End If
    End Sub

    Private Sub Windows_Lock_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.TopMost = True
        activate_lock_screen()
    End Sub
End Class