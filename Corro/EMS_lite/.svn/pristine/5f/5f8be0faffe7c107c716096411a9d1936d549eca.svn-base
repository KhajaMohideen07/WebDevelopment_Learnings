Public Class User_info

    Private Sub User_info_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
    Sub load_grid(tmp_emp_id As Integer)
        Try
            Dim ds1 As DataSet
            ds1 = mysql_data_str("SELECT a.emp_id 'VBOT_ID', a.emp_system_name 'EMP ID/USER ID', CONCAT(a.emp_first_name, ' ', a.emp_last_name) 'Name', f.Designation, e.Department, g.Project_Name 'Process', h.Start_time 'Login Time', h.end_time 'Logout Time', CONCAT(c.emp_first_name, ' ', c.emp_last_name) 'Reporting to', CONCAT(d.emp_first_name, ' ', d.emp_last_name) 'Manager' FROM mas_emp_detail a LEFT JOIN tbl_emp_monitor b ON b.emp_id=a.emp_id LEFT JOIN mas_emp_detail c ON c.emp_id=b.reporting1 LEFT JOIN mas_emp_detail d ON d.emp_id=b.reporting2 LEFT JOIN mas_department e ON e.department_id=b.department LEFT JOIN `mas_designation` f ON f.designation_id=b.designation LEFT JOIN  mas_project_list g ON g.Client_ID=b.assigned_process LEFT JOIN emp_login_log h ON h.emp_id=a.emp_id AND h.login_date='" & mysql_date_format(emp_login_date, True) & "' WHERE a.emp_id='" & tmp_emp_id & "'")
            Dim i As Integer
            Do Until i = ds1.Tables(0).Columns.Count
                DataGridView1.Rows.Add(ds1.Tables(0).Columns(i).ToString, ds1.Tables(0).Rows(0).Item(i).ToString)
                i = i + 1
            Loop
        Catch ex As Exception
            Err.Clear()
            Exit Sub
        End Try

    End Sub
End Class