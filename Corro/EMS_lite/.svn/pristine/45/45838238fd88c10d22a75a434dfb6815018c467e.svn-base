Public Class Messenger
    Dim tmp_emp_id As Integer

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
                Case Keys.ControlKey + Keys.V
                    Exit Select
                Case Else
                    ' others we don't handle 
                    Exit Select
            End Select
        End If
        Return (handled OrElse MyBase.ProcessCmdKey(msg, keyData))
        ' if we haven't handled this key, pass it to the base 
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            If TextBox1.Text = Nothing Then
                Exit Sub
            End If

            If TextBox1.Text.ToLower.Contains("mrn") = True Then
                MsgBox("Unable to send PHI Identifier contents")
                Exit Sub
            End If

            tmp_emp_id = get_emp_id_from_full_name(Me.Text)
            If TextBox1.Text.ToLower = "lock" Then
                TextBox1.Text = "Kindly lock your system while unattended"
            End If
            mysql_data_str("INSERT INTO `user_information`.`tbl_internal_messenger` (`login_date`, `sent_by`, `received_by`, `message_content`, `sent_on`) VALUES ('" & mysql_date_format(emp_login_date, True) & "', '" & emp_id & "', '" & tmp_emp_id & "', '" & TextBox1.Text & "', '" & mysql_date_format(get_server_time(), False) & "'); ")
            DataGridView1.Rows.Add("", TextBox1.Text)
            DataGridView1.Rows.Add("", get_server_time())
            DataGridView1.Rows(DataGridView1.Rows.Count - 1).Cells(1).Style.Alignment = DataGridViewContentAlignment.MiddleRight
            DataGridView1.Rows(DataGridView1.Rows.Count - 2).Cells(1).Style.Alignment = DataGridViewContentAlignment.MiddleRight
            TextBox1.Text = Nothing
        Catch ex As Exception
            Err.Clear()
            Exit Sub
        End Try

    End Sub

    Private Sub Messenger_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Icon = My.Resources.favicon
        DataGridView1.ContextMenuStrip = ContextMenuStrip1
        tmp_emp_id = get_emp_id_from_full_name(Me.Text)      
        Me.TopMost = True
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub ShowHistoryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ShowHistoryToolStripMenuItem.Click
        Try
            Dim ds1 As DataSet
            ds1 = mysql_data_str("SELECT a.sent_by, a.message_content, a.sent_on, CONCAT(b.emp_first_name, ' ',b.emp_last_name) 'Name' FROM tbl_internal_messenger a LEFT JOIN mas_emp_detail b ON b.emp_id=a.sent_by WHERE STATUS=1 AND (received_by='" & emp_id & "' OR a.sent_by='" & tmp_emp_id & "') AND (a.sent_by='" & emp_id & "' OR a.received_by='" & tmp_emp_id & "') ORDER BY sent_on ASC")
            Dim i As Integer
            DataGridView1.Rows.Clear()
            Do Until i = ds1.Tables(0).Rows.Count
                If ds1.Tables(0).Rows(i)(0).ToString = emp_id.ToString Then
                    DataGridView1.Rows.Add("", ds1.Tables(0).Rows(i)(1).ToString)
                    DataGridView1.Rows(DataGridView1.Rows.Count - 1).Cells(1).Style.ForeColor = Color.Blue
                    DataGridView1.Rows.Add("", ds1.Tables(0).Rows(i)(2).ToString)
                    DataGridView1.Rows(DataGridView1.Rows.Count - 2).Cells(1).Style.ForeColor = Color.Blue
                    DataGridView1.Rows(DataGridView1.Rows.Count - 1).Cells(1).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                    DataGridView1.Rows(DataGridView1.Rows.Count - 2).Cells(1).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                Else
                    DataGridView1.Rows.Add("", ds1.Tables(0).Rows(i)(1).ToString)
                    DataGridView1.Rows(DataGridView1.Rows.Count - 1).Cells(1).Style.ForeColor = Color.Blue
                    DataGridView1.Rows.Add("", ds1.Tables(0).Rows(i)(2).ToString)
                    DataGridView1.Rows(DataGridView1.Rows.Count - 2).Cells(1).Style.ForeColor = Color.Blue
                End If
                i = i + 1
            Loop
            DataGridView1.Rows(DataGridView1.Rows.Count - 2).Selected = True
            DataGridView1.Rows(DataGridView1.Rows.Count - 1).Cells(0).Selected = True
            DataGridView1.BeginEdit(True)
        Catch ex As Exception
            Err.Clear()
            Exit Sub
        End Try

    End Sub
End Class