Public Class ActivityCreate
    Dim tmp_process_id As Integer = 0

    Private Sub ActivityCreate_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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

    Private Sub ComboBox1_Validated(sender As Object, e As EventArgs) Handles ComboBox1.Validated
        If ComboBox1.Text = Nothing Then
            Exit Sub
        End If
        load_project_combo()

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

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        
    End Sub

    Private Sub ComboBox3_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ComboBox3.Validating
        If ComboBox3.Text = Nothing Then
            Exit Sub
        End If
        If ComboBox3.SelectedValue > 0 Then
            tmp_process_id = ComboBox3.SelectedValue
            load_grid()
        Else
            Dim ds1 As New DataSet
            ds1 = mysql_data_str("INSERT INTO mas_project_process_list (`project_list_id`, `Process_Name`, updated_by, updated_on) VALUES ('" & ComboBox2.SelectedValue & "','" & Trim(ComboBox3.Text) & "','" & emp_id & "',now());SELECT LAST_INSERT_ID();")
            tmp_process_id = ds1.Tables(0).Rows(0)(0).ToString
        End If
    End Sub
    Sub load_grid()
        '        DataGridView1.Rows.Clear()
        Dim ds1 As DataSet
        ds1 = mysql_data_str("SELECT a.id, a.activity,Start_on,`Fresh`, `Week1`, `Week2`, `Week3`, `Week4`, `RampUp`, `Slab1`, `Slab2`, `Slab3`, `Slab4`, `Slab5` FROM emp_activities a LEFT JOIN `mas_project_process_list` c ON c.id=a.process_id WHERE start_on <='" & mysql_date_format(emp_login_date, True) & "' and end_on >= '" & mysql_date_format(emp_login_date, True) & "' and process_id='" & tmp_process_id & "'")
        DataGridView1.DataSource = ds1.Tables(0)
    End Sub

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
        Dim res As String = Nothing

        'If e.ColumnIndex = 1 Then
        '    res = InputBox("Enter revised activity name", "Activity Name Update", DataGridView1.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString)
        '    If res = DataGridView1.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString Or res.Length < 2 Then
        '        Exit Sub
        '    Else
        '        mysql_data_str("update emp_activities set activity='" & res & "' where id='" & DataGridView1.Rows(e.RowIndex).Cells(0).Value.ToString & "'")
        '    End If
        'End If

        'If e.ColumnIndex = 2 Then
        '    res = InputBox("Enter revised Target", "Target Update", DataGridView1.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString)
        '    MsgBox(res & vbCrLf & Val(DataGridView1.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString))
        '    If Val(res) = Val(DataGridView1.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString) Or Val(res) = 0 Then
        '        Exit Sub
        '    Else
        '        mysql_data_str("update emp_activities set Target='" & Val(res) & "' where id='" & DataGridView1.Rows(e.RowIndex).Cells(0).Value.ToString & "'")
        '    End If
        'End If
        activity_id = DataGridView1.Rows(e.RowIndex).Cells(0).Value
        TextBox1.Text = DataGridView1.Rows(e.RowIndex).Cells("activity").Value
        TextBox1.Focus()
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        If TextBox1.Text.Length < 2 Then
            MsgBox("Minimum length required for Activity")
            Exit Sub
        End If
        If ComboBox3.SelectedValue <> Nothing Then
            If activity_id = 0 Then
                mysql_data_str("INSERT INTO `user_information`.`emp_activities` (`process_id`, `activity`, `target`, `start_on`, `end_on`, `updatedby`, `updatedOn`, `Fresh`, `Week1`, `Week2`, `Week3`, `Week4`, `RampUp`, `Slab1`, `Slab2`, `Slab3`, `Slab4`, `Slab5`) VALUES ('" & tmp_process_id & "', '" & TextBox1.Text & "', '0', '" & mysql_date_format(emp_login_date, True) & "', '2021-12-31', '" & emp_id & "', now(), '" & DataGridView2.Rows(0).Cells(1).Value & "', '" & DataGridView2.Rows(1).Cells(1).Value & "', '" & DataGridView2.Rows(2).Cells(1).Value & "', '" & DataGridView2.Rows(3).Cells(1).Value & "', '" & DataGridView2.Rows(4).Cells(1).Value & "', '" & DataGridView2.Rows(5).Cells(1).Value & "', '" & DataGridView2.Rows(6).Cells(1).Value & "', '" & DataGridView2.Rows(7).Cells(1).Value & "', '" & DataGridView2.Rows(8).Cells(1).Value & "', '" & DataGridView2.Rows(9).Cells(1).Value & "', '" & DataGridView2.Rows(10).Cells(1).Value & "');")
            Else
                If CDate(DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells("start_on").Value) = Today Then
                    mysql_data_str("delete from `user_information`.`emp_activities` where id='" & activity_id & "'")
                Else
                    mysql_data_str("update `user_information`.`emp_activities` set end_on='" & mysql_date_format(DateAdd(DateInterval.Day, -1, emp_login_date), True) & "' where id='" & activity_id & "'")
                End If

                mysql_data_str("INSERT INTO `user_information`.`emp_activities` (`process_id`, `activity`, `target`, `start_on`, `end_on`, `updatedby`, `updatedOn`, `Fresh`, `Week1`, `Week2`, `Week3`, `Week4`, `RampUp`, `Slab1`, `Slab2`, `Slab3`, `Slab4`, `Slab5`) VALUES ('" & tmp_process_id & "', '" & TextBox1.Text & "', '0', '" & mysql_date_format(emp_login_date, True) & "', '2021-12-31', '" & emp_id & "', now(), '" & DataGridView2.Rows(0).Cells(1).Value & "', '" & DataGridView2.Rows(1).Cells(1).Value & "', '" & DataGridView2.Rows(2).Cells(1).Value & "', '" & DataGridView2.Rows(3).Cells(1).Value & "', '" & DataGridView2.Rows(4).Cells(1).Value & "', '" & DataGridView2.Rows(5).Cells(1).Value & "', '" & DataGridView2.Rows(6).Cells(1).Value & "', '" & DataGridView2.Rows(7).Cells(1).Value & "', '" & DataGridView2.Rows(8).Cells(1).Value & "', '" & DataGridView2.Rows(9).Cells(1).Value & "', '" & DataGridView2.Rows(10).Cells(1).Value & "');")
            End If

        End If
        TextBox1.Text = Nothing
        DataGridView2.Rows.Clear()
        load_grid()
    End Sub
    Dim activity_id As Integer = 0
    Private Sub TextBox1_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles TextBox1.Validating
        DataGridView2.Rows.Clear()
        If TextBox1.Text <> Nothing Then
            Dim ds1 As DataSet
            ds1 = mysql_data_str("SELECT `Fresh`, `Week1`, `Week2`, `Week3`, `Week4`, `RampUp`, `Slab1`, `Slab2`, `Slab3`, `Slab4`, `Slab5` FROM emp_activities WHERE id='" & activity_id & "'")
            Dim i As Integer
            Do Until i = ds1.Tables(0).Columns.Count
                If ds1.Tables(0).Rows.Count = 0 Then
                    DataGridView2.Rows.Add(ds1.Tables(0).Columns(i).ColumnName.ToString, 0)
                Else
                    DataGridView2.Rows.Add(ds1.Tables(0).Columns(i).ColumnName.ToString, ds1.Tables(0).Rows(0).Item(i).ToString)
                End If

                i = i + 1
            Loop
        End If
    End Sub
End Class