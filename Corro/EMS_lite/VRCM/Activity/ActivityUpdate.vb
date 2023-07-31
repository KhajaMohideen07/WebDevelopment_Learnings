Public Class ActivityUpdate

    Private Sub ActivityUpdate_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        load_main_combo()
        load_my_production()
    End Sub
    Sub load_my_production()
        Dim ds1 As DataSet
        ds1 = mysql_data_str("SELECT round(SUM(`count`/target*100),2) 'Prod%', round((SUM(`count`/(TIME_TO_SEC(duration) * target /28800) * 100))/count(emp_id),2) 'Perf%' FROM `emp_activities_log` WHERE emp_id='" & emp_id & "' AND login_date='" & mysql_date_format(emp_login_date, True) & "'")
        If ds1.Tables(0).Rows(0)(0).ToString = Nothing Then
            Label6.Text = 0
        Else
            Label6.Text = ds1.Tables(0).Rows(0)(0).ToString & "%"
        End If

        If ds1.Tables(0).Rows(0)(1).ToString = Nothing Then
            Label7.Text = 0
        Else
            Label7.Text = ds1.Tables(0).Rows(0)(1).ToString & "%"
        End If

    End Sub
    Sub load_main_combo()
        Dim ds1 As DataSet
        ds1 = mysql_data_str("SELECT a.* FROM mas_project_group_list a LEFT JOIN `mas_project_process_list` b ON b.id IN (SELECT process_id FROM emp_activities_assign WHERE emp_id='" & emp_id & "' AND start_date <= CURRENT_DATE AND end_date>CURRENT_DATE) LEFT JOIN `mas_project_list` c ON c.Client_id=b.Project_list_id WHERE a.project_group_id = c.project_group_Id")
        With ComboBox1
            .DataSource = ds1.Tables(0)
            .DisplayMember = "project_group"
            .ValueMember = "project_group_id"
            .Text = Nothing
        End With
    End Sub

    Sub load_project_combo()
        Dim ds1 As DataSet
        ds1 = mysql_data_str("SELECT c.* FROM mas_project_group_list a LEFT JOIN `mas_project_process_list` b ON b.id IN (SELECT process_id FROM emp_activities_assign WHERE emp_id='" & emp_id & "' AND start_date <= CURRENT_DATE AND end_date>CURRENT_DATE) LEFT JOIN `mas_project_list` c ON c.Client_id=b.Project_list_id WHERE a.project_group_id = c.project_group_Id and a.project_group_id='" & ComboBox1.SelectedValue & "' and a.active=0")
        With ComboBox2
            .DataSource = ds1.Tables(0)
            .DisplayMember = "Project_Name"
            .ValueMember = "Client_ID"
            .Text = Nothing
        End With

    End Sub
    Sub load_process_combo()
        Dim ds1 As DataSet
        ds1 = mysql_data_str("SELECT b.* FROM mas_project_group_list a LEFT JOIN `mas_project_process_list` b ON b.id IN (SELECT process_id FROM emp_activities_assign WHERE emp_id='" & emp_id & "' AND start_date <= CURRENT_DATE AND end_date>CURRENT_DATE) LEFT JOIN `mas_project_list` c ON c.Client_id=b.Project_list_id WHERE a.project_group_id = c.project_group_Id and b.project_list_id='" & ComboBox2.SelectedValue & "' and b.active=0")
        With ComboBox3
            .DataSource = ds1.Tables(0)
            .DisplayMember = "Process_Name"
            .ValueMember = "id"
            .Text = Nothing
        End With
    End Sub
    Private Sub ComboBox1_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ComboBox1.Validating
        If ComboBox1.Text = Nothing Then
            Exit Sub
        End If
        load_project_combo()
    End Sub

    Private Sub ComboBox2_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ComboBox2.Validating
        If ComboBox2.Text = Nothing Then
            Exit Sub
        End If
        load_process_combo()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If DateTimePicker1.Value > emp_login_date Then
            MsgBox("Future Date not allowed for Production Update.")
            Exit Sub
        End If

        If DataGridView1.Rows.Count = 0 Then
            MsgBox("No records to update")
            Exit Sub
        End If

        Dim ds2 As DataSet
        ds2 = mysql_data_str("SELECT SEC_TO_TIME(SUM(TIME_TO_SEC(TIMEDIFF(NOW(), start_time)))) 'Logged' FROM emp_login_log WHERE emp_id=" & emp_id & " AND login_date='" & mysql_date_format(emp_login_date, True) & "' LIMIT 0, 1000; Select SEC_TO_TIME(SUM(TIME_TO_SEC(TIMEDIFF(end_time, start_time)))) 'Break' FROM break_event_log WHERE emp_id=" & emp_id & " AND login_date='" & mysql_date_format(emp_login_date, True) & "' LIMIT 0, 1000; Select SEC_TO_TIME(SUM(TIME_TO_SEC(duration))) 'updated' FROM emp_activities_log WHERE emp_id=" & emp_id & " AND login_date='" & mysql_date_format(emp_login_date, True) & "' LIMIT 0, 1000;")
        Dim acutal_time As Integer = 0
        acutal_time = DateDiff(DateInterval.Second, CDate(ds2.Tables(1).Rows(0)(0).ToString), CDate(ds2.Tables(0).Rows(0).Item(0).ToString))
        MsgBox(CDate(ds2.Tables(1).Rows(0)(0).ToString) & vbCrLf & CDate(ds2.Tables(0).Rows(0).Item(0).ToString))
        MsgBox(DateAdd(DateInterval.Second, acutal_time, CDate("01/01/01 00:00:00")))



        Dim i As Integer = 0
        Dim prod_entry_status As Integer = 0
        Dim validation_id As Integer = 1
        Do Until i = DataGridView1.Rows.Count
            If DataGridView1.Rows(i).Cells(0).Value = True Then
                If DataGridView1.Rows(i).Cells("col_duration").Value = Nothing Then
                    validation_id = 0
                Else
                    If DataGridView1.Rows(i).Cells("col_duration").Value.Contains(":") = False Then
                        validation_id = 0
                    End If
                End If

                If IsNumeric(DataGridView1.Rows(i).Cells("col_count").Value) = False Then
                    validation_id = 0
                End If

                If validation_id = 0 Then
                    DataGridView1.Rows(i).DefaultCellStyle.BackColor = Color.Orange
                Else
                    If DataGridView1.Rows(i).DefaultCellStyle.BackColor <> Color.Orange Then
                        If DataGridView1.Rows(i).Cells("old_count").Value.ToString <> Nothing And Val(DataGridView1.Rows(i).Cells("old_count").Value) > 0 Then
                            mysql_data_str("UPDATE `emp_activities_log` SET duration=ADDTIME(duration,'" & DataGridView1.Rows(i).Cells("col_duration").Value & "'), `count`='" & Val(DataGridView1.Rows(i).Cells("old_count").Value) + Val(DataGridView1.Rows(i).Cells("col_count").Value) & "', updated_on='" & mysql_date_format(get_server_time(), False) & "' WHERE emp_id='" & emp_id & "' AND login_date='" & mysql_date_format(DateTimePicker1.Value, True) & "' AND activity_id='" & DataGridView1.Rows(i).Cells("col_id").Value & "'")
                        Else
                            mysql_data_str("INSERT INTO `user_information`.`emp_activities_log` (`emp_id`, `login_date`, `activity_id`, `target`, `count`, `updated_on`, `updated_by`, `comments`, `duration`) VALUES ('" & emp_id & "', '" & mysql_date_format(DateTimePicker1.Value, True) & "', '" & DataGridView1.Rows(i).Cells("col_id").Value & "', '" & DataGridView1.Rows(i).Cells("col_target").Value & "', '" & DataGridView1.Rows(i).Cells("old_count").Value + DataGridView1.Rows(i).Cells("col_count").Value & "', '" & mysql_date_format(get_server_time(), False) & "', '" & emp_id & "', '" & DataGridView1.Rows(i).Cells("comments").Value & "','" & DataGridView1.Rows(i).Cells("col_duration").Value & "'); ")
                        End If
                    End If
                    prod_entry_status = 1
                End If
            End If
            i = i + 1
        Loop
        If prod_entry_status = 1 Then
            MsgBox("Production Updated Successfully", MsgBoxStyle.Information)
            load_my_production()
            ComboBox1.Text = Nothing
            ComboBox2.Text = Nothing
            ComboBox3.Text = Nothing
            DataGridView1.Rows.Clear()
        Else
            MsgBox("No Information to update", MsgBoxStyle.Information)
        End If

    End Sub

    Private Sub ComboBox3_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ComboBox3.Validating
        load_grid()
    End Sub
    Sub load_grid()
        DataGridView1.Rows.Clear()
        Dim ds1 As DataSet
        ds1 = mysql_data_str("SELECT a.id, a.activity, a.target, b.count FROM emp_activities a LEFT JOIN `emp_activities_log` b ON b.activity_id=a.id AND login_date='" & mysql_date_format(emp_login_date, True) & "' and b.emp_id='" & emp_id & "' LEFT JOIN `mas_project_process_list` c ON c.id=a.process_id WHERE process_id='" & ComboBox3.SelectedValue & "'")
        Dim i As Integer
        Do Until i = ds1.Tables(0).Rows.Count
            DataGridView1.Rows.Add(False, ds1.Tables(0).Rows(i).Item("id").ToString, ds1.Tables(0).Rows(i).Item("activity").ToString, ds1.Tables(0).Rows(i).Item("target").ToString, ds1.Tables(0).Rows(i).Item("count").ToString)
            i = i + 1
        Loop
    End Sub

    Private Sub Label6_Click(sender As Object, e As EventArgs) Handles Label6.Click
        If Label6.Text = "0" Then
            Exit Sub
        End If
        Dim ds1 As DataSet
        ds1 = mysql_data_str("SELECT d.project_Name 'Cost Center', c.Process_Name, b.Activity, 'Productive' as 'Work Type', a.Target, a.Count, a.Duration, round((a.`count`/a.target*100),2) 'Prod%', round((a.`count`/(TIME_TO_SEC(a.duration) * a.target /28800) * 100),2) 'Perf%' FROM `emp_activities_log` a LEFT JOIN emp_activities b ON b.id=a.activity_id LEFT JOIN mas_project_process_list c ON c.id=b.process_id LEFT JOIN mas_project_list d ON d.Client_Id=c.project_list_id WHERE a.emp_id='" & emp_id & "' AND a.login_date='" & mysql_date_format(emp_login_date, True) & "' union all SELECT '','',(CASE WHEN break_type=5 THEN 'Meeting' ELSE 'Training' END),'Non Productive' AS 'Work Type','480',ROUND((CASE WHEN break_type=5 THEN (SUM(TIME_TO_SEC(TIMEDIFF(end_time, start_time)))) ELSE (SUM(TIME_TO_SEC(TIMEDIFF(end_time, start_time)))) END)/60,0),(CASE WHEN break_type=5 THEN SEC_TO_TIME(SUM(TIME_TO_SEC(TIMEDIFF(end_time, start_time)))) ELSE SEC_TO_TIME(SUM(TIME_TO_SEC(TIMEDIFF(end_time, start_time)))) END),ROUND(((CASE WHEN break_type=5 THEN (SUM(TIME_TO_SEC(TIMEDIFF(end_time, start_time)))) ELSE (SUM(TIME_TO_SEC(TIMEDIFF(end_time, start_time)))) END)/60)/480*100,2),'100.00' FROM `break_event_log` WHERE login_date='" & mysql_date_format(emp_login_date, True) & "' AND break_type IN (5,6) AND emp_id='" & emp_id & "' GROUP BY break_type")
        ActivityDetails.DataGridView1.DataSource = ds1.Tables(0)
        ActivityDetails.Show()
        ActivityDetails.Activate()
    End Sub
End Class