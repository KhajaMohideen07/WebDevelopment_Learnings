Public Class group_message

    Private Sub group_message_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'load_grid()
    End Sub
    Sub load_grid(Optional rep_id As Integer = 0)
        Try
            rep_id = Label6.Text
            Dim sb_qry As String = Nothing
            DataGridView1.Rows.Clear()
            If RadioButton1.Checked = True Then
                sb_qry = " and b.current_status='Active' "
            ElseIf RadioButton2.Checked = True Then
                sb_qry = " and (b.current_status='System Locked' or b.current_status='Idle Time') "
            ElseIf RadioButton3.Checked = True Then
                sb_qry = " and b.current_status='Shift Logout' "
            Else
                sb_qry = Nothing
            End If
            Dim ds1 As DataSet
            If rep_id = 0 Then
                If emp_id = 2355 Or emp_id = 347 Or emp_id = 348 Then
                    ds1 = mysql_data_str("SELECT a.*, b.* FROM mas_emp_detail a LEFT JOIN tbl_emp_monitor b ON b.emp_id=a.emp_id where a.emp_status=0 " & sb_qry & " order by CONCAT(a.emp_first_name, ' ',a.emp_last_name)")
                Else
                    ds1 = mysql_data_str("SELECT a.*, b.* FROM mas_emp_detail a LEFT JOIN tbl_emp_monitor b ON b.emp_id=a.emp_id where a.emp_status=0 and (reporting1='" & emp_id & "'         OR reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1='" & emp_id & "')         OR reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1='" & emp_id & "'))        OR reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1='" & emp_id & "')))        OR reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1='" & emp_id & "'))))        OR reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1='" & emp_id & "')))))) " & sb_qry & " order by CONCAT(a.emp_first_name, ' ',a.emp_last_name)")
                End If

            Else
                ds1 = mysql_data_str("SELECT a.*, b.* FROM mas_emp_detail a LEFT JOIN tbl_emp_monitor b ON b.emp_id=a.emp_id where a.emp_status=0 and (reporting1='" & rep_id & "'         OR reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1='" & rep_id & "')         OR reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1='" & rep_id & "'))        OR reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1='" & rep_id & "')))        OR reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1='" & rep_id & "'))))        OR reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1='" & rep_id & "')))))) " & sb_qry & " order by CONCAT(a.emp_first_name, ' ',a.emp_last_name)")
            End If


            Dim i As Integer
            Do Until i = ds1.Tables(0).Rows.Count

                If ds1.Tables(0).Rows(i).Item("Current_Status").ToString = "Active" Then
                    DataGridView1.Rows.Add(0, ds1.Tables(0).Rows(i).Item("emp_id").ToString, My.Resources.active, ds1.Tables(0).Rows(i).Item("emp_system_name").ToString, (ds1.Tables(0).Rows(i).Item("emp_first_name").ToString & " " & ds1.Tables(0).Rows(i).Item("emp_last_name").ToString))
                ElseIf ds1.Tables(0).Rows(i).Item("Current_Status").ToString = "Shift Logout" Then
                    DataGridView1.Rows.Add(0, ds1.Tables(0).Rows(i).Item("emp_id").ToString, My.Resources.out, ds1.Tables(0).Rows(i).Item("emp_system_name").ToString, (ds1.Tables(0).Rows(i).Item("emp_first_name").ToString & " " & ds1.Tables(0).Rows(i).Item("emp_last_name").ToString))
                Else
                    DataGridView1.Rows.Add(0, ds1.Tables(0).Rows(i).Item("emp_id").ToString, My.Resources.away, ds1.Tables(0).Rows(i).Item("emp_system_name").ToString, (ds1.Tables(0).Rows(i).Item("emp_first_name").ToString & " " & ds1.Tables(0).Rows(i).Item("emp_last_name").ToString))
                End If
                i = i + 1
            Loop
        Catch ex As Exception
            Err.Clear()
            Exit Sub
        End Try

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim i As Integer
        Do Until i = DataGridView1.Rows.Count
            If DataGridView1.Rows(i).Cells(0).Value = True Or DataGridView1.Rows(i).Cells(0).Value = 1 Then
                mysql_data_str("INSERT INTO `user_information`.`tbl_internal_messenger` (`login_date`, `sent_by`, `received_by`, `message_content`, `sent_on`) VALUES ('" & mysql_date_format(emp_login_date, True) & "', '" & emp_id & "', '" & DataGridView1.Rows(i).Cells(1).Value & "', '" & TextBox1.Text & "', '" & mysql_date_format(get_server_time(), False) & "'); ")
                DataGridView1.Rows(i).Cells(0).Value = 0
                DataGridView1.Rows(i).Cells(0).Value = False
            End If
            i = i + 1
        Loop
        MsgBox("Group Message Successfully Sent")
        TextBox1.Text = Nothing
        Me.Close()
    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        Dim i As Integer
        If CheckBox2.Checked = True Then
            Do Until i = DataGridView1.Rows.Count
                DataGridView1.Rows(i).Cells(0).Value = True
                i = i + 1
            Loop
        Else
            Do Until i = DataGridView1.Rows.Count
                DataGridView1.Rows(i).Cells(0).Value = False
                i = i + 1
            Loop
        End If

    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        load_grid()
    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton2.CheckedChanged
        load_grid()
    End Sub

    Private Sub RadioButton3_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton3.CheckedChanged
        load_grid()
    End Sub
End Class