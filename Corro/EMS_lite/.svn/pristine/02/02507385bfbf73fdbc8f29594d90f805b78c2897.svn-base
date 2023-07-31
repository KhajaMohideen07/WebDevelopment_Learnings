Public Class FloorView

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged, ComboBox2.SelectedIndexChanged

    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click

    End Sub

    Private Sub FloorView_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        load_location_combo()
        DataGridView1.Width = Me.Width - 100
        DataGridView1.Height = Me.Height - 150
    End Sub

    Private Sub ComboBox1_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ComboBox1.Validating
        If ComboBox1.Text <> Nothing Then
            load_floor_combo(ComboBox1.SelectedValue)
        End If
    End Sub
    Sub load_location_combo()
        Try
            Dim ds1 As DataSet
            ds1 = mysql_data_str("select * from mas_work_location")
            With ComboBox1
                .DataSource = ds1.Tables(0)
                .DisplayMember = "Work_Location_Name"
                .ValueMember = "work_location_id"
                .Text = "Work_Location_Name"
            End With
        Catch ex As Exception
            Err.Clear()
            Exit Sub
        End Try

    End Sub
    Sub load_floor_combo(location_id As Integer)
        Try
            Dim ds1 As DataSet
            ds1 = mysql_data_str("SELECT `floor` FROM mas_ip_address WHERE location_id=" & location_id & " GROUP BY `floor` ")
            With ComboBox2
                .DataSource = ds1.Tables(0)
                .DisplayMember = "floor"
                .Text = "floor"
            End With
        Catch ex As Exception
            Err.Clear()
            Exit Sub
        End Try

    End Sub

    Private Sub ComboBox2_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ComboBox2.Validating
        If ComboBox2.Text <> Nothing Then
            load_grid()
        End If
    End Sub
    Sub load_grid()
        Try
            Dim ds1 As DataSet
            ds1 = mysql_data_str("SELECT MAX(row_id), MAX(col_id) FROM mas_ip_address WHERE location_id=" & ComboBox1.SelectedValue & " AND FLOOR=" & ComboBox2.Text & ";")
            Dim i As Integer = 0
            DataGridView1.Rows.Clear()
            DataGridView1.Columns.Clear()
            Do Until i = ds1.Tables(0).Rows(0)(1).ToString
                DataGridView1.Columns.Add("col" & i, i)
                i = i + 1
            Loop
            i = 0
            Do Until i = ds1.Tables(0).Rows(0)(0).ToString
                DataGridView1.Rows.Add()
                i = i + 1
            Loop
            load_detail()
        Catch ex As Exception
            Err.Clear()
            Exit Sub
        End Try

    End Sub
    Sub load_detail()
        Try
            Dim ds1 As DataSet
            ds1 = mysql_data_str("SELECT a.System_Name, b.current_status, a.Row_id, a.Col_id FROM mas_ip_address a LEFT JOIN tbl_emp_monitor b ON b.system_name=a.system_name WHERE location_id='" & ComboBox1.SelectedValue & "' AND FLOOR='" & ComboBox2.Text & "'")
            Dim i As Integer
            Do Until i = ds1.Tables(0).Rows.Count
                Dim row_id As Integer = Val(ds1.Tables(0).Rows(i).Item(2).ToString) - 1
                Dim col_id As Integer = Val(ds1.Tables(0).Rows(i).Item(3).ToString) - 1
                DataGridView1.Rows(row_id).Cells(col_id).Value = ds1.Tables(0).Rows(i).Item(0).ToString
                If ds1.Tables(0).Rows(i).Item(1).ToString = "Active" Then
                    DataGridView1.Rows(row_id).Cells(col_id).Style.BackColor = Color.LightGreen
                ElseIf ds1.Tables(0).Rows(i).Item(1).ToString = "Shift Logout" Or ds1.Tables(0).Rows(i).Item(1).ToString = Nothing Then
                    DataGridView1.Rows(row_id).Cells(col_id).Style.BackColor = Color.LightGray
                Else
                    DataGridView1.Rows(row_id).Cells(col_id).Style.BackColor = Color.Orange
                End If
                i = i + 1
            Loop
        Catch ex As Exception
            Err.Clear()
            Exit Sub
        End Try

    End Sub
End Class