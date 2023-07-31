Public Class reporting_changes

    Private Sub reporting_changes_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        load_combo()
    End Sub
    Sub load_combo()
        Dim ds1 As DataSet
        ds1 = mysql_data_str("SELECT emp_id, concat(emp_system_name,'-',emp_first_name) emp_system_name FROM mas_emp_detail WHERE emp_access_level=4 AND emp_status=0 order by emp_first_name")
        With ComboBox1
            .DataSource = ds1.Tables(0)
            .DisplayMember = "emp_system_name"
            .ValueMember = "emp_id"
            .Text = Nothing
        End With

        ds1 = Nothing
        ds1 = mysql_data_str("SELECT emp_id, concat(emp_system_name,'-',emp_first_name) emp_system_name FROM mas_emp_detail WHERE emp_access_level=4 AND emp_status=0 order by emp_first_name")
        With ComboBox2
            .DataSource = ds1.Tables(0)
            .DisplayMember = "emp_system_name"
            .ValueMember = "emp_id"
            .Text = Nothing
        End With
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If TextBox1.Text = Nothing Then
            Exit Sub
        End If

        If ComboBox1.Text = Nothing Then
            Exit Sub
        End If

        If ComboBox2.Text = Nothing Then
            Exit Sub
        End If

        Dim emp_id_str As String = "'"
        'emp_id_str = emp_id_str & Replace(TextBox1.Text, ",", "', '") & "'"
        emp_id_str = emp_id_str & Replace(TextBox1.Text, vbCrLf, "', '") & "'"
        mysql_data_str("UPDATE `user_information`.`tbl_emp_monitor` SET `reporting1` = '" & ComboBox1.SelectedValue & "' , `reporting2` = '" & ComboBox2.SelectedValue & "' WHERE `emp_id` in (SELECT emp_id FROM mas_emp_detail WHERE emp_system_name IN (" & emp_id_str & ")); UPDATE log_emp_reporting1 SET reporting_id1='" & ComboBox1.SelectedValue & "' WHERE end_date='2021-12-31' AND `emp_id` in (SELECT emp_id FROM mas_emp_detail WHERE emp_system_name IN (" & emp_id_str & ")); UPDATE log_emp_reporting2 SET reporting_id2='" & ComboBox2.SelectedValue & "' WHERE end_date='2021-12-31' and `emp_id` in (SELECT emp_id FROM mas_emp_detail WHERE emp_system_name IN (" & emp_id_str & ")); ")
        MsgBox("Successfully Updated")
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If TextBox1.Text = Nothing Then
            Exit Sub
        End If

        Dim res_box As MsgBoxResult
        res_box = MsgBox("Are you sure to update as Resigned", MsgBoxStyle.YesNo)
        If res_box = MsgBoxResult.Yes Then
            Dim emp_id_str As String = "'"
            'emp_id_str = emp_id_str & Replace(TextBox1.Text, ",", "', '") & "'"
            emp_id_str = emp_id_str & Replace(TextBox1.Text, vbCrLf, "', '") & "'"
            mysql_data_str("UPDATE `user_information`.mas_emp_detail set DOR=CURRENT_DATE, emp_status=1 WHERE emp_system_name IN (" & emp_id_str & ");")
            MsgBox("Successfully Updated")
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If TextBox1.Text = Nothing Then
            Exit Sub
        End If

        Dim res_box As MsgBoxResult
        res_box = MsgBox("Are you sure to update as Active", MsgBoxStyle.YesNo)
        If res_box = MsgBoxResult.Yes Then
            Dim emp_id_str As String = "'"
            'emp_id_str = emp_id_str & Replace(TextBox1.Text, ",", "', '") & "'"
            emp_id_str = emp_id_str & Replace(TextBox1.Text, vbCrLf, "', '") & "'"
            mysql_data_str("UPDATE `user_information`.mas_emp_detail set DOR=null, emp_status=0 WHERE emp_system_name IN (" & emp_id_str & ");")
            MsgBox("Successfully Updated")
        End If
    End Sub
End Class