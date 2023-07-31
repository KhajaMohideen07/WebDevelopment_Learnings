Imports MongoDB.Bson
Imports MongoDB.Driver

Public Class User_Details

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Button1.Text = "Edit" Then
            Enable_fields()
            Button1.Text = "Save"
        Else

        End If
    End Sub
    Sub Enable_fields()

    End Sub
    Sub di_fields()

    End Sub

    Sub load_combo_details()
        Dim ds1 As DataSet
        ds1 = mysql_data_str("select * from mas_department where active=0")
        With cbDepartment
            .DataSource = ds1.Tables(0)
            .DisplayMember = "Department"
            .ValueMember = "department_id"
            .Text = Nothing
        End With

        Dim ds2 As DataSet
        ds2 = mysql_data_str("select * from mas_designation where active=0")
        With cbDesignation
            .DataSource = ds2.Tables(0)
            .DisplayMember = "Designation"
            .ValueMember = "designation_id"
            .Text = Nothing
        End With

        Dim ds3 As DataSet
        ds3 = mysql_data_str("select * from mas_project_group_list where active=0")
        With cbProject
            .DataSource = ds3.Tables(0)
            .DisplayMember = "project_group"
            .ValueMember = "project_group_id"
            .Text = Nothing
        End With

        Dim ds4 As DataSet
        ds4 = mysql_data_str("select * from mas_access_level where active=0")
        With cbAccessRole
            .DataSource = ds4.Tables(0)
            .DisplayMember = "Access_level"
            .ValueMember = "id"
            .Text = Nothing
        End With

        Dim ds5 As DataSet
        ds5 = mysql_data_str("select * from mas_cost_center where active=0")
        With cbCostCenter
            .DataSource = ds5.Tables(0)
            .DisplayMember = "cost_center"
            .ValueMember = "id"
            .Text = Nothing
        End With

        Dim ds6 As DataSet
        ds6 = mysql_data_str("select * from mas_work_location where active=0")
        With cbLocation
            .DataSource = ds6.Tables(0)
            .DisplayMember = "work_location_name"
            .ValueMember = "work_location_id"
            .Text = Nothing
        End With

        Dim ds7 As DataSet
        ds7 = mysql_data_str("select * from mas_emp_detail where emp_status=0 and emp_access_level<>6")
        With cbReporting1
            .DataSource = ds7.Tables(0)
            .DisplayMember = "emp_first_name"
            .ValueMember = "emp_id"
            .Text = Nothing
        End With

        Dim ds8 As DataSet
        ds8 = mysql_data_str("select * from mas_emp_detail where emp_status=0 and emp_access_level<>6")
        With cbReporting2
            .DataSource = ds8.Tables(0)
            .DisplayMember = "emp_first_name"
            .ValueMember = "emp_id"
            .Text = Nothing
        End With

    End Sub
    Private Sub User_Details_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        load_combo_details()
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub TextBox1_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles TextBox1.Validating
        If TextBox1.Text = Nothing Then
            Exit Sub
        End If
        load_my_detils(TextBox1.Text)
    End Sub
    Sub load_my_detils(tmp_emp_id As String)
        Dim ds1 As DataSet
        ds1 = mysql_data_str("select * from mas_emp_detail where emp_system_name='" & tmp_emp_id & "'")

        If ds1.Tables(0).Rows.Count = 0 Then
            Exit Sub
        End If
        Dim ds2 As DataSet
        ds2 = mysql_data_str("select * from tbl_emp_monitor where emp_id='" & ds1.Tables(0).Rows(0).Item("emp_id").ToString & "'")

        tbFirstName.Text = ds1.Tables(0).Rows(0).Item("emp_first_name").ToString
        tbLastName.Text = ds1.Tables(0).Rows(0).Item("emp_last_name").ToString
        dtpDOB.Value = CDate(ds1.Tables(0).Rows(0).Item("dob").ToString)
        dtpDOJ.Value = CDate(ds1.Tables(0).Rows(0).Item("doj").ToString)
        MsgBox(ds1.Tables(0).Rows(0).Item("emp_access_level").ToString)
        cbAccessRole.SelectedValue = ds1.Tables(0).Rows(0).Item("emp_access_level").ToString
        cbLocation.SelectedValue = ds1.Tables(0).Rows(0).Item("work_location_id").ToString

        cbReporting1.SelectedValue = ds2.Tables(0).Rows(0).Item("reporting1").ToString
        cbDepartment.SelectedValue = ds2.Tables(0).Rows(0).Item("department").ToString
        cbDesignation.SelectedValue = ds2.Tables(0).Rows(0).Item("designation").ToString
        cbReporting2.SelectedValue = ds2.Tables(0).Rows(0).Item("reporting2").ToString
        cbProject.SelectedValue = ds2.Tables(0).Rows(0).Item("project_group").ToString
        load_sub_project(ds2.Tables(0).Rows(0).Item("project_group").ToString)
        cbProjectClass.SelectedValue = ds2.Tables(0).Rows(0).Item("assigned_process").ToString
        dtpShiftIn.Value = CDate("01/01/1901 " & ds2.Tables(0).Rows(0).Item("Shift_In").ToString)
        dtpShiftOut.Value = CDate("01/01/1901 " & ds2.Tables(0).Rows(0).Item("Shift_Out").ToString)
    End Sub
    Sub load_sub_project(tmp_proj_id As Integer)

        Dim ds1 As DataSet
        ds1 = mysql_data_str("select * from mas_project_list where active=0 and Project_Group_ID='" & tmp_proj_id & "'")
        With cbProjectClass
            .DataSource = ds1.Tables(0)
            .DisplayMember = "project_name"
            .ValueMember = "Client_ID"
            .Text = Nothing
        End With
    End Sub

    Private Sub cbProject_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles cbProject.Validating
        load_sub_project(cbProject.SelectedValue)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim date_str As String = InputBox("Enter Date")
        Dim new_mongo_db As IMongoDatabase = mongo_connect()
        Dim collection As IMongoCollection(Of BsonDocument) = new_mongo_db.GetCollection(Of BsonDocument)("application_tracking")

        DataGridView1.Rows.Clear()
        Dim filter = Builders(Of BsonDocument).Filter.Eq(Of String)("login_date", "" & date_str & "")
        For Each item As BsonDocument In collection.Find(filter).ToList
            Dim emp_id As BsonElement = item.GetElement("emp_id")
            Dim login_date As BsonElement = item.GetElement("login_date")
            Dim start_time As BsonElement = item.GetElement("start_time")
            Dim end_time As BsonElement = item.GetElement("end_time")
            Dim app_name As BsonElement = item.GetElement("app_name")
            DataGridView1.Rows.Add(emp_id.Value, login_date.Value, start_time.Value, end_time.Value, app_name.Value)
            'Exit Sub
        Next
    End Sub
End Class