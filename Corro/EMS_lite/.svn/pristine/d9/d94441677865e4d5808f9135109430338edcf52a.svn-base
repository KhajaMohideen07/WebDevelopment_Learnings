Public Class ActivityAssign

    Private Sub ActivityAssign_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        load_main_combo()

    End Sub
    Sub load_main_combo()
        Dim ds1 As DataSet
        ds1 = mysql_data_str("select * from mas_project_group_list where active=0")
        With ComboBox1
            .DataSource = ds1.Tables(0)
            .DisplayMember = "project_group"
            .ValueMember = "project_group_id"
            .Text = Nothing
        End With
    End Sub

    Sub load_project_combo()
        Dim ds1 As DataSet
        ds1 = mysql_data_str("select * from mas_project_list where project_group_id='" & ComboBox1.SelectedValue & "' and active=0")
        With ComboBox2
            .DataSource = ds1.Tables(0)
            .DisplayMember = "Project_Name"
            .ValueMember = "Client_ID"
            .Text = Nothing
        End With

    End Sub
    Sub load_process_combo()
        Dim ds1 As DataSet
        ds1 = mysql_data_str("select * from mas_project_process_list where project_list_id='" & ComboBox2.SelectedValue & "' and active=0")
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
        load_emp_list()

    End Sub
    Sub load_emp_list()
        CheckedListBox1.Items.Clear()
        Dim ds1 As DataSet
        ds1 = mysql_data_str("SELECT b.emp_id, b.emp_system_name, b.emp_first_name  FROM tbl_emp_communicator a LEFT JOIN mas_emp_detail b ON b.emp_id=a.emp_id WHERE a.level2='" & ComboBox2.Text & "' ORDER BY b.emp_first_name")

        Dim i As Integer
        Do Until i = ds1.Tables(0).Rows.Count
            CheckedListBox1.Items.Add(ds1.Tables(0).Rows(i).Item("emp_first_name").ToString, False)
            i = i + 1
        Loop
    End Sub
    Sub load_grid()
        '        DataGridView1.Rows.Clear()
        Dim ds1 As DataSet
        ds1 = mysql_data_str("SELECT a.id, a.activity,Start_on,`Fresh`, `Week1`, `Week2`, `Week3`, `Week4`, `RampUp`, `Slab1`, `Slab2`, `Slab3`, `Slab4`, `Slab5` FROM emp_activities a LEFT JOIN `mas_project_process_list` c ON c.id=a.process_id WHERE start_on <='" & mysql_date_format(emp_login_date, True) & "' and end_on >= '" & mysql_date_format(emp_login_date, True) & "' and process_id='" & tmp_process_id & "'")
        DataGridView1.DataSource = ds1.Tables(0)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim i As Integer
        Do Until i = DataGridView1.Rows.Count
            If DataGridView1.Rows(i).Cells(0).Value = True Or DataGridView1.Rows(i).Cells(0).Value = 1 Then
                mysql_data_str("INSERT INTO `user_information`.`emp_activities_assign` (`emp_id`, `process_id`, `start_date`, `end_date`, `updated_by`, `updated_on`) VALUES ('2355', '" & ComboBox3.SelectedValue & "', '" & mysql_date_format(emp_login_date, True) & "', '2021-12-31', '" & emp_id & "', now()); ")
            End If
            i = i + 1
        Loop
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)
        Dim i As Integer
        Do Until i = DataGridView1.Rows.Count
            If DataGridView1.Rows(i).Cells(0).Value = True Or DataGridView1.Rows(i).Cells(0).Value = 1 Then
                mysql_data_str("INSERT INTO `user_information`.`emp_activities_assign` (`emp_id`, `process_id`, `start_date`, `end_date`, `updated_by`, `updated_on`) VALUES ('" & DataGridView1.Rows(i).Cells("col_id").Value & "', '" & ComboBox3.SelectedValue & "', '" & mysql_date_format(emp_login_date, True) & "', '2021-12-31', '" & emp_id & "', now()); ")
            End If
            i = i + 1
        Loop
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        Dim i As Integer
        Do Until i = CheckedListBox1.Items.Count
            If CheckBox1.Checked = True Then
                CheckedListBox1.SetItemChecked(i, True)
            Else
                CheckedListBox1.SetItemChecked(i, False)
            End If
            i = i + 1
        Loop
    End Sub
    Dim tmp_process_id As Integer
    Private Sub ComboBox3_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ComboBox3.Validating
        If ComboBox3.Text = Nothing Then
            Exit Sub
        End If
        If ComboBox3.SelectedValue > 0 Then
            tmp_process_id = ComboBox3.SelectedValue
            load_grid()
        End If

    End Sub

    Private Sub CheckedListBox1_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles CheckedListBox1.Validating
        If CheckedListBox1.SelectedItems.Count = 1 Then
            load_grid2()
        End If
    End Sub
    Sub load_grid2()
    End Sub
End Class