Imports System.IO
Imports System
Imports System.Data
Imports System.Net.Mail.SmtpClient
Imports System.Data.Odbc
Imports System.Data.OleDb


Public Class HeadCountUpload
    Dim file_name As String
    Dim row_count As Integer = 0
    Dim sr As StreamReader
    Sub Datagridview_Clear_All()
        On Error Resume Next
        DataGridView1.Rows.Clear()
        On Error Resume Next
        datagridview1.Columns.Clear()
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Call Datagridview_Clear_All()

            If Clipboard.GetText.Length <= 0 Then
                MsgBox("No Data", MsgBoxStyle.Critical)
                Exit Sub
            End If

            Dim item() As String
            Dim looper As Integer = 0

            Dim j As Integer

            For Each line As String In Clipboard.GetText.Split(vbNewLine)

                line = line.Replace(Chr(10), "")
                Dim item1() As String = line.Split(vbTab)

                If looper = 0 Then
                    item = Split(line, vbTab)
                    j = 0

                    'If UBound(item) <> 6 Then
                    '    MsgBox("Invalid Data. Data contained more than one column.", MsgBoxStyle.Critical)

                    '    Exit Sub
                    'End If

                    ''MsgBox(item(0).ToString.ToLower & "Facility" & vbCrLf & item(1).ToString.ToLower & "PatientId" & vbCrLf & item(2).ToString.ToLower & "PatientName" & vbCrLf & item(3).ToString.ToLower & "Birthdate" & vbCrLf & item(4).ToString.ToLower & "Date" & vbCrLf & item(5).ToString.ToLower & "DoctorName" & vbCrLf & item(6).ToString.ToLower & "Insurance in CPS")
                    'If item(0).ToString.ToLower <> "facility" And item(1).ToString.ToLower <> "patientid" And item(2).ToString.ToLower <> "patientname" And item(3).ToString.ToLower <> "birthdate" And item(4).ToString.ToLower <> "date" And item(5).ToString.ToLower <> "doctorname" And item(6).ToString.ToLower <> "financial" Then

                    '    MsgBox("Invalid column Names", MsgBoxStyle.Critical)
                    '    Exit Sub
                    'End If


                    Do Until j = UBound(item) + 1
                        DataGridView1.Columns.Add(item(j), item(j).ToUpper)
                        j = j + 1
                    Loop
                    DataGridView1.Columns.Add("Auto_Comment", "Comment")
                Else
                    DataGridView1.Rows.Add(item1).ToString.Trim()
                End If
                looper = looper + 1
            Next
            datagridview1.Rows.RemoveAt(datagridview1.Rows.Count - 1)

            Button1.Enabled = False
            Button2.Enabled = True
            MsgBox("Import Successfully", MsgBoxStyle.Information)

        Catch ex As Exception
            MsgBox("Error No: " & Err.Number & vbCrLf & "Error Description: " & Err.Description, MsgBoxStyle.Critical)
            Err.Clear()
            Exit Sub
        End Try


    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim temp_emp_id As Integer
        Dim proj_grop_id As Integer
        Dim i As Integer
        Dim ds2 As DataSet
        Dim col_counter_validation As Integer = 0
        Do Until i = DataGridView1.Columns.Count
            If DataGridView1.Columns(i).HeaderText.Contains("ASSOCIATE CODE") = True Then
                col_counter_validation = col_counter_validation + 1
            ElseIf DataGridView1.Columns(i).HeaderText.Contains("LOCATION") = True Then
                col_counter_validation = col_counter_validation + 1
            ElseIf DataGridView1.Columns(i).HeaderText.Contains("DEPARTMENT") = True Then
                col_counter_validation = col_counter_validation + 1
            ElseIf DataGridView1.Columns(i).HeaderText.Contains("COST CENTER") = True Then
                col_counter_validation = col_counter_validation + 1
            ElseIf DataGridView1.Columns(i).HeaderText.Contains("ASSOCIATE DESIGNATION") = True Then
                col_counter_validation = col_counter_validation + 1
            ElseIf DataGridView1.Columns(i).HeaderText.Contains("PROJECT NAME") = True Then
                col_counter_validation = col_counter_validation + 1
            ElseIf DataGridView1.Columns(i).HeaderText.Contains("ASSOCIATE NAME") = True Then
                col_counter_validation = col_counter_validation + 1
            ElseIf DataGridView1.Columns(i).HeaderText.Contains("DOJ") = True Then
                col_counter_validation = col_counter_validation + 1
            ElseIf DataGridView1.Columns(i).HeaderText.Contains("REPORTING MANAGER EMP CODE") = True Then
                col_counter_validation = col_counter_validation + 1
            End If
            i = i + 1
        Loop
        If col_counter_validation <> 9 Then
            MsgBox("One of the Column missing. Required Columns:" & vbCrLf & vbCrLf & "ASSOCIATE CODE" & vbCrLf & "ASSOCIATE NAME" & vbCrLf & "ASSOCIATE DESIGNATION" & vbCrLf & "DOJ" & vbCrLf & "DEPARTMENT" & vbCrLf & "LOCATION" & vbCrLf & "PROJECT NAME" & vbCrLf & "COST CENTER" & vbCrLf & "REPORTING MANAGER EMP CODE")
            Exit Sub
        End If
        i = 0
        Do Until i = DataGridView1.Rows.Count
            Dim emp_level_str As Integer = 0
            Dim ds1 As DataSet
            ds1 = mysql_data_str("select emp_id from mas_emp_detail where emp_system_name='" & DataGridView1.Rows(i).Cells("ASSOCIATE CODE").Value.ToString.Trim & "'")
            'insert


            If ds1.Tables(0).Rows.Count = 0 Then

                If DataGridView1.Rows(i).Cells("Associate Designation").Value.ToString.ToLower.Trim.Contains("executive") Then
                    emp_level_str = 6
                Else
                    emp_level_str = 4
                End If
                Dim loc_str As Integer = -1

re_loop:

                loc_str = cbLocation.FindString(DataGridView1.Rows(i).Cells("LOCATION").Value.ToString.Trim)
                cbLocation.SelectedIndex = loc_str
                loc_str = -1
                loc_str = cbDesignation.FindString(DataGridView1.Rows(i).Cells("Associate Designation").Value.ToString.Trim)
                cbDesignation.SelectedIndex = loc_str
                loc_str = -1
                loc_str = cbDepartment.FindString(DataGridView1.Rows(i).Cells("Department").Value.ToString.Trim)
                cbDepartment.SelectedIndex = loc_str
                loc_str = -1
                loc_str = cbCostCenter.FindString(DataGridView1.Rows(i).Cells("Cost Center").Value.ToString.Trim)
                cbCostCenter.SelectedIndex = loc_str
                loc_str = -1
                loc_str = cbProject.FindString(DataGridView1.Rows(i).Cells("Project Name").Value.ToString.Trim)
                If loc_str = -1 Then
                    Dim ds6 As DataSet = mysql_data_str("INSERT INTO `mas_project_list` (`Project_Name`, `Project_Group_ID`) VALUES ('" & DataGridView1.Rows(i).Cells("Project Name").Value.ToString & "','6'); SELECT LAST_INSERT_ID();")
                    load_combo()
                    GoTo re_loop
                End If
                cbProject.SelectedIndex = loc_str

                Dim ds4 As DataSet
                ds4 = mysql_data_str("SELECT a.`Project_Group_ID`, b.project_group FROM `mas_project_list` a LEFT JOIN `mas_project_group_list` b ON b.project_group_id=a.`Project_Group_ID` WHERE `Client_ID`='" & cbProject.SelectedValue & "'")
                proj_grop_id = ds4.Tables(0).Rows(0)(0).ToString

                If DataGridView1.Rows(i).Cells("REPORTING MANAGER EMP CODE ").Value.ToString.Trim = "VRCM0001" Then
                    ds2 = mysql_data_str("SELECT '999999' as 'rep1', '999999' as 'rep2'")
                Else
                    ds2 = mysql_data_str("SELECT a.emp_id 'rep1', b.reporting_id1 'rep2' FROM mas_emp_detail a LEFT JOIN `log_emp_reporting1` b ON b.emp_id=a.emp_id WHERE a.emp_system_name='" & DataGridView1.Rows(i).Cells("REPORTING MANAGER EMP CODE ").Value.ToString.Trim & "'")
                End If

                Dim ds3 As DataSet
                ds3 = mysql_data_str("INSERT INTO `mas_emp_detail`(`emp_id_prefix`,`emp_system_name`, `emp_first_name`, `emp_last_name`, `emp_status`, `DOB`, `DOJ`, `emp_type`, `emp_access_level`, `Mobile1`, `official_email`, `personal_email`, `work_location_id`, `emp_Track_App`, `system_lock_secs`, `monitor_tbl_status`) VALUES ('VRCM', '" & DataGridView1.Rows(i).Cells("ASSOCIATE CODE").Value.ToString.Trim & "', '" & DataGridView1.Rows(i).Cells("ASSOCIATE NAME").Value.ToString.Trim & "', '', 0, '" & mysql_date_format(CDate(DataGridView1.Rows(i).Cells("DOJ").Value), True) & "', '" & mysql_date_format(CDate(DataGridView1.Rows(i).Cells("DOJ").Value), True) & "', 0, '" & emp_level_str & "', '', '" & DataGridView1.Rows(i).Cells("EMAIL ADDRESS").Value.ToString.Trim & "', '', '" & cbLocation.SelectedValue & "', 1, '300', '1'); SELECT LAST_INSERT_ID();")
                temp_emp_id = ds3.Tables(0).Rows(0)(0).ToString
                mysql_data_str("INSERT INTO `tbl_emp_monitor` (`emp_id`, `login_date`, `current_status`, `reporting1`, `reporting2`, `assigned_process`, `department`, `designation`, `cost_center`, `project_group`) VALUES ('" & temp_emp_id & "','" & mysql_date_format(emp_login_date, True) & "','Shift Logout','" & ds2.Tables(0).Rows(0)(0).ToString & "','" & ds2.Tables(0).Rows(0)(1).ToString & "','" & cbProject.SelectedValue & "','" & cbDepartment.SelectedValue & "','" & cbDesignation.SelectedValue & "','" & cbCostCenter.SelectedValue & "','" & proj_grop_id & "');")
            Else
                temp_emp_id = ds1.Tables(0).Rows(0)(0).ToString
            End If
            i = i + 1
        Loop

        i = 0
        Do Until i = DataGridView1.Rows.Count
            Dim emp_level_str As Integer = 0
            Dim ds1 As DataSet
            ds1 = mysql_data_str("select emp_id from mas_emp_detail where emp_system_name='" & DataGridView1.Rows(i).Cells("ASSOCIATE CODE").Value.ToString.Trim & "'")
            'insert
            temp_emp_id = ds1.Tables(0).Rows(0)(0).ToString

            If DataGridView1.Rows(i).Cells("Associate Designation").Value.ToString.ToLower.Trim = "VRCMHYD1084" Then
                MsgBox("wait")
            End If

            If DataGridView1.Rows(i).Cells("Associate Designation").Value.ToString.ToLower.Trim.Contains("executive") Then
                emp_level_str = 6
            Else
                emp_level_str = 4
            End If
            mysql_data_str("update mas_emp_detail set emp_access_level='" & emp_level_str & "' where emp_id='" & temp_emp_id & "'")


            Dim loc_str As Integer = -1
re_loop1:

            loc_str = cbLocation.FindString(DataGridView1.Rows(i).Cells("LOCATION").Value.ToString.Trim)
            cbLocation.SelectedIndex = loc_str
            If loc_str > -1 Then
                loc_str = cbDesignation.FindString(DataGridView1.Rows(i).Cells("Associate Designation").Value.ToString.Trim)
                cbDesignation.SelectedIndex = loc_str
                If loc_str > -1 Then
                    loc_str = cbDepartment.FindString(DataGridView1.Rows(i).Cells("Department").Value.ToString.Trim)
                    cbDepartment.SelectedIndex = loc_str
                    If loc_str > -1 Then
                        loc_str = cbCostCenter.FindString(DataGridView1.Rows(i).Cells("Cost Center").Value.ToString.Trim)
                        cbCostCenter.SelectedIndex = loc_str
                        If loc_str > -1 Then
                            loc_str = cbProject.FindString(DataGridView1.Rows(i).Cells("Project Name").Value.ToString.Trim)
                            cbProject.SelectedIndex = loc_str
                            If loc_str = -1 Then
                                Dim ds6 As DataSet = mysql_data_str("INSERT INTO `mas_project_list` (`Project_Name`, `Project_Group_ID`) VALUES ('" & DataGridView1.Rows(i).Cells("Project Name").Value.ToString & "','6'); SELECT LAST_INSERT_ID();")
                                load_combo()
                                GoTo re_loop1
                            End If
                            cbProject.SelectedIndex = loc_str
                        End If
                    End If
                End If
            End If
            

            Dim ds4 As DataSet
            ds4 = mysql_data_str("SELECT a.`Project_Group_ID`, b.project_group FROM `mas_project_list` a LEFT JOIN `mas_project_group_list` b ON b.project_group_id=a.`Project_Group_ID` WHERE `Client_ID`='" & cbProject.SelectedValue & "'")
            proj_grop_id = ds4.Tables(0).Rows(0)(0).ToString

            If DataGridView1.Rows(i).Cells("REPORTING MANAGER EMP CODE ").Value.ToString.Trim = "VRCM0001" Then
                ds2 = mysql_data_str("SELECT '999999' as 'rep1', '999999' as 'rep2'")
            Else
                ds2 = mysql_data_str("SELECT a.emp_id 'rep1', b.reporting_id1 'rep2' FROM mas_emp_detail a LEFT JOIN `log_emp_reporting1` b ON b.emp_id=a.emp_id WHERE a.emp_system_name='" & DataGridView1.Rows(i).Cells("REPORTING MANAGER EMP CODE ").Value.ToString.Trim & "'")
            End If

            mysql_data_str("UPDATE `log_emp_cost_center` SET end_date='" & mysql_date_format(DateAdd(DateInterval.Day, -1, emp_login_date), True) & "', updated_on=now() WHERE emp_id='" & temp_emp_id & "' AND end_date='2021-12-31' AND cost_center_id<>'" & cbCostCenter.SelectedValue & "'; INSERT IGNORE INTO `log_emp_cost_center` VALUES ('" & temp_emp_id & "','" & cbCostCenter.SelectedValue & "','" & mysql_date_format(emp_login_date, True) & "','2021-12-31','999999',now());")
            mysql_data_str("UPDATE `log_emp_department` SET  end_date='" & mysql_date_format(DateAdd(DateInterval.Day, -1, emp_login_date), True) & "', updated_on=now() WHERE emp_id='" & temp_emp_id & "' AND end_date='2021-12-31' AND `department_id`<>'" & cbDepartment.SelectedValue & "'; INSERT IGNORE INTO `log_emp_department` VALUES ('" & temp_emp_id & "','" & cbDepartment.SelectedValue & "','" & mysql_date_format(emp_login_date, True) & "','2021-12-31','999999',now());")
            mysql_data_str("UPDATE `log_emp_designation` SET  end_date='" & mysql_date_format(DateAdd(DateInterval.Day, -1, emp_login_date), True) & "', updated_on=now() WHERE emp_id='" & temp_emp_id & "' AND end_date='2021-12-31' AND `designation_id`<>'" & cbDesignation.SelectedValue & "'; INSERT IGNORE INTO `log_emp_designation` VALUES ('" & temp_emp_id & "','" & cbDesignation.SelectedValue & "','" & mysql_date_format(emp_login_date, True) & "','2021-12-31','999999',now());")
            mysql_data_str("UPDATE `log_emp_project` SET  end_date='" & mysql_date_format(DateAdd(DateInterval.Day, -1, emp_login_date), True) & "', updated_on=now() WHERE emp_id='" & temp_emp_id & "' AND end_date='2021-12-31' AND `project_group_id`<>'" & proj_grop_id & "'; INSERT IGNORE INTO `log_emp_project` VALUES ('" & temp_emp_id & "','" & proj_grop_id & "','" & mysql_date_format(emp_login_date, True) & "','2021-12-31','999999',now());")
            mysql_data_str("UPDATE `log_emp_process` SET  end_date='" & mysql_date_format(DateAdd(DateInterval.Day, -1, emp_login_date), True) & "', updated_on=now() WHERE emp_id='" & temp_emp_id & "' AND end_date='2021-12-31' AND `project_id`<>'" & cbProject.SelectedValue & "'; INSERT IGNORE INTO `log_emp_process` VALUES ('" & temp_emp_id & "','" & cbProject.SelectedValue & "','" & mysql_date_format(emp_login_date, True) & "','2021-12-31','999999',now());")
            mysql_data_str("UPDATE `log_emp_reporting1` SET  end_date='" & mysql_date_format(DateAdd(DateInterval.Day, -1, emp_login_date), True) & "', updated_on=now() WHERE emp_id='" & temp_emp_id & "' AND end_date='2021-12-31' AND `reporting_id1`<>'" & ds2.Tables(0).Rows(0)(0).ToString & "'; INSERT IGNORE INTO `log_emp_reporting1` VALUES ('" & temp_emp_id & "','" & ds2.Tables(0).Rows(0)(0).ToString & "','" & mysql_date_format(emp_login_date, True) & "','2021-12-31','999999',now());")
            mysql_data_str("UPDATE `log_emp_reporting2` SET  end_date='" & mysql_date_format(DateAdd(DateInterval.Day, -1, emp_login_date), True) & "', updated_on=now() WHERE emp_id='" & temp_emp_id & "' AND end_date='2021-12-31' AND `reporting_id2`<>'" & ds2.Tables(0).Rows(0)(1).ToString & "'; INSERT IGNORE INTO `log_emp_reporting2` VALUES ('" & temp_emp_id & "','" & ds2.Tables(0).Rows(0)(1).ToString & "','" & mysql_date_format(emp_login_date, True) & "','2021-12-31','999999',now());")
            mysql_data_str("DELETE FROM tbl_emp_communicator WHERE emp_id='" & temp_emp_id & "'; INSERT IGNORE INTO tbl_emp_communicator (`emp_id`, `Type`, `Level1`, `Level2`, `Level3`) VALUES ('" & temp_emp_id & "','" & ds4.Tables(0).Rows(0)(1).ToString & "','" & cbDepartment.Text & "','" & cbProject.Text & "','" & cbProject.SelectedText & "');")
            i = i + 1
        Loop
        MsgBox("Upload Completed")
    End Sub

    Private Sub HeadCountUpload_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        load_combo()
    End Sub
    Sub load_combo()
        Dim ds1 As DataSet
        ds1 = mysql_data_str("SELECT id, cost_center title FROM mas_cost_center WHERE active=0;  SELECT department_id id, Department title FROM mas_department WHERE active=0; SELECT designation_id id , Designation title FROM mas_designation WHERE active=0; SELECT work_location_id id, work_location_name title FROM mas_work_location WHERE active=0; SELECT client_id id, Project_Name title FROM mas_project_list WHERE active=0")

        With cbCostCenter
            .DataSource = ds1.Tables(0)
            .ValueMember = "id"
            .DisplayMember = "title"
            .Text = Nothing
        End With

        With cbDepartment
            .DataSource = ds1.Tables(1)
            .ValueMember = "id"
            .DisplayMember = "title"
            .Text = Nothing
        End With

        With cbDesignation
            .DataSource = ds1.Tables(2)
            .ValueMember = "id"
            .DisplayMember = "title"
            .Text = Nothing
        End With

        With cbLocation
            .DataSource = ds1.Tables(3)
            .ValueMember = "id"
            .DisplayMember = "title"
            .Text = Nothing
        End With

        With cbProject
            .DataSource = ds1.Tables(4)
            .ValueMember = "id"
            .DisplayMember = "title"
            .Text = Nothing
        End With
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim i As Integer
        Dim shift_type As Integer = 0
        Do Until i = DataGridView1.Rows.Count
            If Val(Microsoft.VisualBasic.Left(Trim(DataGridView1.Rows(i).Cells("Emp Shift").Value.ToString), 2)) <= 15 Then
                shift_type = 0
            Else
                shift_type = 1
            End If
            If DataGridView1.Rows(i).Cells("Associate Code").Value.ToString = "VRCMHYD2165" Then
                MsgBox("hi")
            End If
            mysql_data_str("UPDATE `user_information`.`mas_emp_detail` SET reporting_empcode='" & DataGridView1.Rows(i).Cells("Reporting Manager Emp Code ").Value.ToString.Trim & "', reporting_name='" & DataGridView1.Rows(i).Cells("Reporting Manager Name").Value.ToString.Trim & "', emp_shift='" & DataGridView1.Rows(i).Cells("EMP SHIFT").Value.ToString.Trim & "', shift_type='" & shift_type & "', `DOJ`='" & mysql_date_format(CDate(DataGridView1.Rows(i).Cells("DOJ").Value.ToString.Trim), True) & "', official_email='" & DataGridView1.Rows(i).Cells("Email Address").Value.ToString.Trim & "',`Location` = '" & DataGridView1.Rows(i).Cells("Location").Value.ToString.Trim & "' , `Vertical` = '" & DataGridView1.Rows(i).Cells("Project Classification").Value.ToString.Trim & "' ,`process` = '" & DataGridView1.Rows(i).Cells("Department").Value.ToString.Trim & "' ,  `project` = '" & DataGridView1.Rows(i).Cells("Project Name").Value.ToString.Trim & "' , `SubProject` = '' , `Function` = '" & DataGridView1.Rows(i).Cells("Cost Center").Value.ToString.Trim & "' WHERE `emp_system_name` = '" & DataGridView1.Rows(i).Cells("Associate Code").Value.ToString.Trim & "';")
            i = i + 1
        Loop
        MsgBox("Successfully Updated")
        Me.Close()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Try
            Dim new_role_combo As New ComboBox
            Dim ds2 As DataSet
            ds2 = mysql_data_str("SELECT `Designation`, `VPMS_Role` FROM mas_designation WHERE active=0")
            With new_role_combo
                .DataSource = ds2.Tables(0)
                .DisplayMember = "Designation"
                .ValueMember = "VPMS_Role"
                .Text = Nothing
            End With
            Dim ds1 As DataSet = Nothing
            Dim i As Integer
            Do Until i = DataGridView1.Rows.Count
                Dim get_role As String = 0
                Dim ds3 As DataSet = mysql_data_str("SELECT `Designation`, `VPMS_Role` FROM mas_designation WHERE Designation='" & DataGridView1.Rows(i).Cells("Associate Designation").Value & "'")
                get_role = ds3.Tables(0).Rows(0)(1).ToString
                Dim rep_role As String
                ds3 = mysql_data_str("SELECT `Designation`, `VPMS_Role` FROM mas_designation WHERE Designation='" & DataGridView1.Rows(i).Cells("Reporting Manager Designation").Value & "'")
                rep_role = ds3.Tables(0).Rows(0)(1).ToString
                mysql_data_str_live("INSERT Ignore INTO `wfm_vpms_2019`.`tbl_users` (`empCode`, `associateName`, `emailAddress`, `fileNo`, `designation`, `role`, `doj`, `department`, `location`, `empShift`, `tenurePosition`, `projectName`, `functionalHeads`, `projectClassification`, `costCentre`, `cl`, `sl`, `ml`, `lop`, `totalLeave`, `warningLetter`,`NoOfwarningLetter`, `warningLetterDates`, `warningLetterReason`, `reportingManagerName`, `reportingManagerEmpCode`, `reportingManagerDesignation`, `reportingManagerRole`, `password`, `q1`, `q2`, `q3`, `q4`, `nextApproverId1`, `nextApproverId2`, `nextApproverId3`, `nextApproverId4`, `status1`, `status2`, `status3`, `status4`) VALUES ('" & DataGridView1.Rows(i).Cells("Associate Code").Value.ToString.Trim & "', '" & DataGridView1.Rows(i).Cells("Associate Name").Value.ToString.Trim & "', '" & DataGridView1.Rows(i).Cells("Email Address").Value.ToString.Trim & "', '" & DataGridView1.Rows(i).Cells("File No").Value.ToString.Trim & "', '" & DataGridView1.Rows(i).Cells("Associate Designation").Value.ToString.Trim & "', '" & get_role & "', '" & Format(CDate(DataGridView1.Rows(i).Cells("DOJ").Value), "yyyy-MM-dd") & "', '" & DataGridView1.Rows(i).Cells("Department").Value & "', '" & DataGridView1.Rows(i).Cells("Location").Value & "', '" & DataGridView1.Rows(i).Cells("Emp Shift").Value & "', '" & DataGridView1.Rows(i).Cells("Tenure Position").Value & "', '" & DataGridView1.Rows(i).Cells("Project Name").Value & "', '" & DataGridView1.Rows(i).Cells("Functional Heads").Value & "', '" & DataGridView1.Rows(i).Cells("Project Classification").Value & "', '" & DataGridView1.Rows(i).Cells("Cost Center").Value & "', '" & DataGridView1.Rows(i).Cells("Casual Leave").Value & "', '" & DataGridView1.Rows(i).Cells("Sick Leave").Value & "', '" & DataGridView1.Rows(i).Cells("Maternity Leave").Value & "', '" & DataGridView1.Rows(i).Cells("Loss of Pay").Value & "', '" & DataGridView1.Rows(i).Cells("Total Leave").Value & "', '', '', '', '', '" & DataGridView1.Rows(i).Cells("Reporting Manager Name").Value & "', '" & DataGridView1.Rows(i).Cells("Reporting Manager Emp Code ").Value & "', '" & DataGridView1.Rows(i).Cells("Reporting Manager Designation").Value & "', '" & rep_role & "', '', '0', '0', '-', '-', '', '', '', '', '0', '0', '0', '0'); ")
                i = i + 1
            Loop
            i = 0
            Do Until i = DataGridView1.Rows.Count
                If DataGridView1.Rows(i).Cells("Reporting Manager Emp Code ").Value <> "VRCM0001" Then


                    Dim ds5 As New DataSet
                    ds5 = mysql_data_str_live("SELECT empCode, `associateName`, `role`, `designation` FROM tbl_users WHERE empCode='{0}'", DataGridView1.Rows(i).Cells("Reporting Manager Emp Code ").Value)
                    'MsgBox(ds5.Tables(0).Rows.Count)
looper:             If ds5.Tables(0).Rows.Count <> 0 Then
                        If ds5.Tables(0).Rows(0)(2).ToString = "Team Lead" Or ds5.Tables(0).Rows(0)(2).ToString = "QC & Sr QC" Or ds5.Tables(0).Rows(0)(2).ToString = "Prdn Agents" Then
                            ds5 = mysql_data_str_live("SELECT `reportingManagerEmpCode`, `reportingManagerName`, `reportingManagerRole`,`reportingManagerDesignation` FROM tbl_users WHERE empCode='" & ds5.Tables(0).Rows(0)(0).ToString & "'")
                            GoTo looper
                        Else
                            If ds5.Tables(0).Rows.Count <> 0 Then
                                mysql_data_str_live("UPDATE `tbl_users` SET `reportingManagerName`='" & ds5.Tables(0).Rows(0).Item(1).ToString & "', `reportingManagerEmpCode`='" & ds5.Tables(0).Rows(0).Item(0).ToString & "', `reportingManagerDesignation`='" & ds5.Tables(0).Rows(0).Item(3).ToString & "', `reportingManagerRole`='" & ds5.Tables(0).Rows(0).Item(2).ToString & "' WHERE empCode='" & DataGridView1.Rows(i).Cells("Associate Code").Value & "'")
                            End If
                        End If
                    End If
                End If
                i = i + 1
            Loop
            MsgBox("VPMS Data Upload completed")

        Catch ex As Exception
            MsgBox("Database Connection Issue. Contact AppDev Team.", MsgBoxStyle.Critical)
        Finally
            call_disconnect()
        End Try

    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click

    End Sub
End Class