Public Class AppProductivity

    Private Sub AppProductivity_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        load_combos("Project Group", ComboBox1)
        load_reporting()
    End Sub
    Sub load_reporting()
        Dim ds1 As DataSet
        ds1 = mysql_data_str("SELECT a.emp_id, a.emp_first_name FROM tbl_emp_monitor b LEFT JOIN mas_emp_detail a ON a.emp_id=b.emp_id WHERE a.emp_access_level<>6 AND a.emp_status=0 AND (b.reporting1='1041' OR b.reporting2='1041') ORDER BY a.emp_first_name")
        With ComboBox5
            .DataSource = ds1.Tables(0)
            .DisplayMember = "emp_first_name"
            .ValueMember = "emp_id"
            .Text = "emp_first_name"
        End With
    End Sub
    Sub load_sub_reporting()
        Dim ds1 As DataSet
        ds1 = mysql_data_str("SELECT a.emp_id, a.emp_first_name FROM tbl_emp_monitor b LEFT JOIN mas_emp_detail a ON a.emp_id=b.emp_id WHERE a.emp_access_level<>6 AND a.emp_status=0 AND (b.reporting1='" & ComboBox5.SelectedValue & "' OR b.reporting2='" & ComboBox5.SelectedValue & "') ORDER BY a.emp_first_name")
        With ComboBox4
            .DataSource = ds1.Tables(0)
            .DisplayMember = "emp_first_name"
            .ValueMember = "emp_id"
            .Text = "emp_first_name"
        End With
    End Sub
    Public Function load_combos(combo_type As String, combo_Name As ComboBox) As ComboBox
        Dim cb1 As New ComboBox
        Dim ds1 As DataSet
        If combo_type = "Project Group" Then
            ds1 = mysql_data_str("select * from mas_project_group_list where active=0")
            With combo_Name
                .DataSource = ds1.Tables(0)
                .DisplayMember = "project_group"
                .ValueMember = "project_group_id"
                .Text = Nothing
            End With
        ElseIf combo_type = "Project" Then
            ds1 = mysql_data_str("select * from mas_project_list where active=0")
            With combo_Name
                .DataSource = ds1.Tables(0)
                .DisplayMember = "Project_Name"
                .ValueMember = "Client_ID"
                .Text = Nothing
            End With
        ElseIf combo_type = "Process" Then
            ds1 = mysql_data_str("select * mas_project_group_list where active=0")
            With cb1
                .DataSource = ds1.Tables(0)
                .DisplayMember = "project_group"
                .ValueMember = "project_group_id"
                .Text = Nothing
            End With
        End If

        Return cb1

    End Function

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        load_grid()
    End Sub
    Sub load_grid()
        If ComboBox4.Text = Nothing Then
            MsgBox("Select reporting head to proceed further")
            Exit Sub
        End If
        DataGridView1.Rows.Clear()
        Dim ds1 As DataSet
        ds1 = mysql_data_str("SELECT AppName, TIMEDIFF(end_time, start_time) 'Duration' FROM application_tracking_log WHERE AppName NOT LIKE '<%' AND TIMEDIFF(end_time, start_time) > '00:01:00' AND emp_id IN (SELECT a.emp_id FROM tbl_emp_monitor a WHERE a.reporting1='" & ComboBox4.SelectedValue & "' OR a.reporting2='" & ComboBox4.SelectedValue & "') GROUP BY AppName ORDER BY TIMEDIFF(end_time, start_time) DESC, appName DESC")
        Dim i As Integer
        Do Until i = ds1.Tables(0).Rows.Count
            DataGridView1.Rows.Add(False, ds1.Tables(0).Rows(i).Item("AppName").ToString)
            i = i + 1
        Loop

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        DataGridView2.Rows.Clear()
        Dim res_box As MsgBoxResult = MsgBox("Are you sure to move this for Productivty Calculation...", MsgBoxStyle.YesNo)
        If res_box = MsgBoxResult.Yes Then
            Dim i As Integer
            Do Until i = DataGridView1.Rows.Count
                If DataGridView1.Rows(i).Cells("col_select").Value = True Then
                    DataGridView2.Rows.Add(False, DataGridView1.Rows(i).Cells("col_AppName").Value)
                    mysql_data_str("INSERT INTO `user_information`.`application_tracking_master` (`AppName`, `Reporting`, `InsertedBy`, `InsertedOn`, `start_date`, `end_date`, `project_id`) VALUES ('" & DataGridView1.Rows(i).Cells("col_AppName").ToString & "', '" & ComboBox4.SelectedValue & "', '" & ComboBox4.SelectedValue & "', '" & mysql_date_format(get_server_time(), True) & "', '" & mysql_date_format(emp_login_date, True) & "', '" & mysql_date_format(vbot_end_date, True) & "', ''); ")
                End If
                i = i + 1
            Loop
        End If

    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click

    End Sub

    Private Sub ComboBox5_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ComboBox5.Validating
        If ComboBox5.Text = Nothing Then
            Exit Sub
        End If
        load_sub_reporting()
    End Sub
End Class