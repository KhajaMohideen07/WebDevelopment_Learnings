Imports MongoDB.Bson
Imports MongoDB.Driver

Public Class CurrentActivty
    Sub load_grid()
        Try
            Dim sub_str As String = Nothing

            If CheckBox1.Checked = True Then
                sub_str = Nothing
            Else
                Dim i As Integer
                Do Until i = CheckedListBox1.Items.Count
                    If sub_str = Nothing Then
                        'MsgBox(CheckedListBox1.GetItemCheckState(i).ToString)
                        If CheckedListBox1.GetItemCheckState(i) = CheckState.Checked Then
                            sub_str = "'" & CheckedListBox1.Items(i).ToString & "'"
                        End If
                    Else
                        If CheckedListBox1.GetItemCheckState(i) = CheckState.Checked Then
                            sub_str = sub_str & ",'" & CheckedListBox1.Items(i).ToString & "'"
                        End If
                    End If
                    i = i + 1
                Loop
                If sub_str <> Nothing Then
                    sub_str = " and a.AppName in (" & sub_str & ")"
                End If
            End If
            Dim ds5 As DataSet = Nothing

            If emp_id = 2355 Then
                If RadioButton1.Checked = True Then
                    ds5 = mysql_data_str("SELECT a.id, c.emp_system_name 'Emp ID', c.emp_first_name 'Name', b.current_status, a.AppName 'Current Application' , a.AppNameGroup 'App Group', a.Start_Time 'Start on' FROM application_tracking_log a LEFT JOIN tbl_emp_monitor b ON b.emp_id=a.emp_id LEFT JOIN mas_emp_detail c ON c.emp_id=a.emp_id WHERE a.End_time IS NULL AND a.login_date='" & mysql_date_format(emp_login_date, True) & "'")
                ElseIf RadioButton2.Checked = True Then
                    ds5 = mysql_data_str("SELECT c.emp_id 'VBOT ID', c.emp_system_name 'Emp ID', c.emp_first_name 'Name', e.emp_first_name 'Reporting to', d.Start_Time 'Login Time', TIMEDIFF(d.end_time, d.Start_Time) 'Logged Hours', SEC_TO_TIME(SUM(TIME_TO_SEC(TIMEDIFF(a.end_time, a.Start_Time)))) 'Duration',f.Break_Hours  FROM application_tracking_log a LEFT JOIN tbl_emp_monitor b ON b.emp_id=a.emp_id LEFT JOIN mas_emp_detail c ON c.emp_id=a.emp_id LEFT JOIN emp_login_log d ON d.emp_id=a.emp_id AND d.login_date=a.login_date LEFT JOIN mas_emp_detail e ON e.emp_id=b.reporting1 LEFT JOIN break_view f ON f.emp_id=a.emp_id AND f.login_date=a.login_date WHERE a.login_date='" & mysql_date_format(DateTimePicker1.Value, True) & "' " & sub_str & "  GROUP BY a.emp_id")
                End If
            Else
                If RadioButton1.Checked = True Then
                    ds5 = mysql_data_str("SELECT a.id, c.emp_system_name 'Emp ID', c.emp_first_name 'Name', b.current_status, a.AppName 'Current Application' , a.AppNameGroup 'App Group', a.Start_Time 'Start on' FROM application_tracking_log a LEFT JOIN tbl_emp_monitor b ON b.emp_id=a.emp_id LEFT JOIN mas_emp_detail c ON c.emp_id=a.emp_id WHERE a.End_time IS NULL AND a.login_date='" & mysql_date_format(emp_login_date, True) & "' AND (b.reporting1='" & emp_id & "' OR b.reporting2='" & emp_id & "')")
                ElseIf RadioButton2.Checked = True Then
                    ds5 = mysql_data_str("SELECT c.emp_id 'VBOT ID', c.emp_system_name 'Emp ID', c.emp_first_name 'Name', e.emp_first_name 'Reporting to', d.Start_Time 'Login Time', TIMEDIFF(d.end_time, d.Start_Time) 'Logged Hours', SEC_TO_TIME(SUM(TIME_TO_SEC(TIMEDIFF(a.end_time, a.Start_Time)))) 'Duration',f.Break_Hours  FROM application_tracking_log a LEFT JOIN tbl_emp_monitor b ON b.emp_id=a.emp_id LEFT JOIN mas_emp_detail c ON c.emp_id=a.emp_id LEFT JOIN emp_login_log d ON d.emp_id=a.emp_id AND d.login_date=a.login_date LEFT JOIN mas_emp_detail e ON e.emp_id=b.reporting1 LEFT JOIN break_view f ON f.emp_id=a.emp_id AND f.login_date=a.login_date WHERE (b.reporting1='" & emp_id & "' OR b.reporting2='" & emp_id & "') and a.login_date='" & mysql_date_format(DateTimePicker1.Value, True) & "' " & sub_str & "  GROUP BY a.emp_id")
                End If
            End If
            DataGridView1.DataSource = ds5.Tables(0)

        Catch ex As Exception
            Err.Clear()
            Exit Sub
        End Try

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        load_grid()
    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton2.CheckedChanged
        load_grid()
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        load_grid()
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub CurrentActivty_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        load_activity()
    End Sub
    Sub load_activity()
        Try
            Dim ds1 As DataSet
            If emp_id = 2355 Then
                ds1 = mysql_data_str("SELECT a.AppName FROM application_tracking_log a LEFT JOIN tbl_emp_monitor b ON b.emp_id=a.emp_id WHERE a.login_date='" & mysql_date_format(DateTimePicker1.Value, True) & "' AND a.AppName NOT LIKE '<%' GROUP BY a.AppName ORDER BY SEC_TO_TIME(SUM(TIME_TO_SEC(TIMEDIFF(a.end_time, a.Start_Time)))) DESC")
            Else
                ds1 = mysql_data_str("SELECT a.AppName FROM application_tracking_log a LEFT JOIN tbl_emp_monitor b ON b.emp_id=a.emp_id WHERE (b.reporting1='" & emp_id & "' OR b.reporting2='" & emp_id & "') and a.login_date='" & mysql_date_format(DateTimePicker1.Value, True) & "' AND a.AppName NOT LIKE '<%' GROUP BY a.AppName ORDER BY SEC_TO_TIME(SUM(TIME_TO_SEC(TIMEDIFF(a.end_time, a.Start_Time)))) DESC")
            End If

            CheckedListBox1.Items.Clear()
            Dim I As Integer
            Do Until I = ds1.Tables(0).Rows.Count
                CheckedListBox1.Items.Add(ds1.Tables(0).Rows(I).Item(0).ToString, True)
                I = I + 1
            Loop
        Catch ex As Exception
            Err.Clear()
            Exit Sub
        End Try

    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        Dim I As Integer
        If CheckBox1.Checked = True Then
            load_activity()
        Else
            Do Until I = CheckedListBox1.Items.Count
                CheckedListBox1.SetItemCheckState(I, False)
                I = I + 1
            Loop
        End If
    End Sub
End Class