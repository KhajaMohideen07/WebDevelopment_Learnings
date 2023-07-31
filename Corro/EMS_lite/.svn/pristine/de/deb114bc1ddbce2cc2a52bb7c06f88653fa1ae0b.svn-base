
Public Class loc
    Private Declare Function BlockInput Lib "user32" Alias "BlockInput" (ByVal fBlock As Integer) As Integer
    Private Declare Function LockWorkStation Lib "User32.dll" () As Long

    Dim child_clock As analog_clock
    Dim show_id As Integer = 0
    Sub update_break_details()
        Dim i As Integer
        Dim subqry As String = Nothing

        Do Until i = appDgv.Rows.Count - 2
            If appDgv.Rows(i).Cells("Comments").Value = "" Then
                'mysql_data_str("Insert into application_tracking_log (Emp_ID, Login_Date, Start_Time, End_Time, AppName, AppNamegroup) values ('" & emp_id & "', '" & mysql_date_format(emp_login_date, True) & "', '" & mysql_date_format(appDgv.Rows(i).Cells("start_time").Value, False) & "', '" & mysql_date_format(appDgv.Rows(i + 1).Cells("start_time").Value, False) & "', '" & appDgv.Rows(i).Cells("AppName").Value & "','" & appDgv.Rows(i).Cells("AppNameGroup").Value & "');")
                If subqry = Nothing Then
                    subqry = "('" & emp_id & "', '" & mysql_date_format(emp_login_date, True) & "', '" & mysql_date_format(appDgv.Rows(i).Cells("start_time").Value, False) & "', '" & mysql_date_format(appDgv.Rows(i + 1).Cells("start_time").Value, False) & "', '" & appDgv.Rows(i).Cells("AppName").Value & "','" & appDgv.Rows(i).Cells("AppNameGroup").Value & "')"
                Else
                    If appDgv.Rows(i + 1).Cells("start_time").Value = Nothing Then
                        subqry = subqry & ", ('" & emp_id & "', '" & mysql_date_format(emp_login_date, True) & "', '" & mysql_date_format(appDgv.Rows(i).Cells("start_time").Value, False) & "', now(), '" & appDgv.Rows(i).Cells("AppName").Value & "','" & appDgv.Rows(i).Cells("AppNameGroup").Value & "')"
                    Else
                        subqry = subqry & ", ('" & emp_id & "', '" & mysql_date_format(emp_login_date, True) & "', '" & mysql_date_format(appDgv.Rows(i).Cells("start_time").Value, False) & "', '" & mysql_date_format(appDgv.Rows(i + 1).Cells("start_time").Value, False) & "', '" & appDgv.Rows(i).Cells("AppName").Value & "','" & appDgv.Rows(i).Cells("AppNameGroup").Value & "')"
                    End If

                End If
                appDgv.Rows(i).Cells("Comments").Value = "Updated"
            End If
            i = i + 1
        Loop
        mysql_data_str("Insert into application_tracking_log (Emp_ID, Login_Date, Start_Time, End_Time, AppName, AppNamegroup) values " & subqry)

        i = 0
        Do Until i = BreakDgv.Rows.Count
            mysql_data_str("insert into break_event_log (Emp_id, Login_Date, Start_Time, End_Time, reason, break_type) values ('" & emp_id & "', '" & mysql_date_format(emp_login_date, True) & "', '" & mysql_date_format(CDate(BreakDgv.Rows(i).Cells("Start_Time").Value), False) & "', '" & mysql_date_format(CDate(BreakDgv.Rows(i).Cells("End_time").Value), False) & "', '" & BreakDgv.Rows(i).Cells("reason").Value & "', '" & BreakDgv.Rows(i).Cells("breaktype").Value & "')")
            i = i + 1
        Loop
        'appDgv.Rows.Clear()
        BreakDgv.Rows.Clear()
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            DataGridView1 = appDgv

            If ComboBox1.Text = Nothing Then
                Exit Sub
            End If
            If (ComboBox1.SelectedValue = 3 And ComboBox1.Text = "Idle Time") Then
                If ComboBox2.Text = Nothing Then
                    Exit Sub
                End If

                If ComboBox2.Text.Contains("   ") = True Then
                    Exit Sub
                End If

                If Len(Trim_Replace(ComboBox2.Text)) < 5 Then
                    Exit Sub
                End If
            End If

            form_closing_id = 1

            If checkConnection("select now()") = True Then
                '    update_break_details()
                emp_action_stop_time = get_server_time()
                mysql_data_str("insert into break_event_log (Emp_id, Login_Date, Start_Time, End_Time, reason, break_type) values ('" & emp_id & "', '" & mysql_date_format(emp_login_date, True) & "', '" & mysql_date_format(emp_action_start_time, False) & "', '" & mysql_date_format(emp_action_stop_time, False) & "', '" & ComboBox2.Text & "', '" & ComboBox1.SelectedValue & "')")
                mysql_data_str("update tbl_emp_monitor set updated_on='" & mysql_date_format(emp_action_stop_time, False) & "', current_status='Active' where emp_id='" & emp_id & "'")
                'emp_login_time = get_server_time()

            Else
                BreakDgv.Rows.Add(emp_id, emp_login_date, emp_action_start_time, emp_action_stop_time, ComboBox2.Text, ComboBox1.SelectedValue)
            End If
            'Exit Sub
            veeBOT.auto_lock_or_manual_lock = 0
            messenger_activity_id = 1
            Me.Close()
        Catch ex As Exception
            Err.Clear()
            Me.Close()
        End Try
        'appDgv.Rows.Clear()
    End Sub

    Private Sub loc_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If form_closing_id = 0 Then
            form_closing_id = 1
            'emp_login_time = get_server_time()
            mysql_data_str("insert into break_event_log (Emp_id, Login_Date, Start_Time, End_Time, reason, break_type) values ('" & emp_id & "', '" & mysql_date_format(emp_login_date, True) & "', '" & mysql_date_format(emp_action_start_time, False) & "', '" & mysql_date_format(emp_action_stop_time, False) & "', '" & ComboBox2.Text & "', '11')")
            mysql_data_str("update tbl_emp_monitor set updated_on='" & mysql_date_format(emp_action_stop_time, False) & "', current_status='Active' where emp_id='" & emp_id & "'")
            veeBOT.auto_lock_or_manual_lock = 0
            messenger_activity_id = 1
            Me.Close()

        Else
            e.Cancel = False
        End If

    End Sub

    Private Sub loc_Load(sender As Object, e As EventArgs) Handles Me.Load
        form_closing_id = 0
        load_combo()
        ComboBox2.Focus()
        ComboBox2.Select()
        '' Me.TableLayoutPanel1.Controls.Add(child_clock, 1, 0)
        show_id = 30
        Label4.Text = show_id & " sec to mark as Personal Break"
        Timer1.Enabled = True
        If veeBOT.auto_lock_or_manual_lock = 1 Then
            RadioButton1.Visible = True
            RadioButton2.Visible = False
            RadioButton3.Visible = False
            RadioButton4.Visible = False
            RadioButton5.Visible = False
            RadioButton6.Visible = False
            RadioButton7.Visible = False
            RadioButton8.Visible = False
            RadioButton9.Visible = False
        Else
            RadioButton1.Visible = False
        End If
        Me.TopMost = True
    End Sub
    Sub load_combo()
        Try
            Dim ds1 As DataSet
            If veeBOT.auto_lock_or_manual_lock = 1 Then
                ds1 = idle_break_data_set
                With ComboBox1
                    .DataSource = ds1.Tables(0)
                    .DisplayMember = "Break_Type"
                    .ValueMember = "id"
                    .Text = "Break_Type"
                End With

                Dim ds2 As DataSet
                ds2 = sub_break_data_set
                With ComboBox2
                    .DataSource = ds2.Tables(0)
                    .DisplayMember = "Break_Type"
                    .Text = ""
                End With
            Else
                ds1 = normal_break_data_set
                With ComboBox1
                    .DataSource = ds1.Tables(0)
                    .DisplayMember = "Break_Type"
                    .ValueMember = "id"
                    .Text = ""
                End With

                Dim ds2 As DataSet = normal_break_data_set
                With breakCB1
                    .DataSource = ds2.Tables(0)
                    .DisplayMember = "Break_Type"
                    .ValueMember = "id"
                    .Text = ""
                End With

                Dim ds3 As DataSet = normal_break_data_set
                With breakCB2
                    .DataSource = ds3.Tables(0)
                    .DisplayMember = "Break_Type"
                    .ValueMember = "id"
                    .Text = ""
                End With

                Dim ds4 As DataSet = normal_break_data_set
                With breakCB3
                    .DataSource = ds4.Tables(0)
                    .DisplayMember = "Break_Type"
                    .ValueMember = "id"
                    .Text = ""
                End With

            End If

        Catch ex As Exception
            Err.Clear()
            Exit Sub
        End Try

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged

    End Sub

    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click

    End Sub

    Private Sub PictureBox2_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs)
    End Sub

    Private Sub Combobox2_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub Combobox2_Validated(sender As Object, e As EventArgs)
        ComboBox1.Focus()
    End Sub

    Private Sub ComboBox1_Validated(sender As Object, e As EventArgs) Handles ComboBox1.Validated
        Button1.Select()
    End Sub

    Private Sub PictureBox2_Click_1(sender As Object, e As EventArgs) Handles PictureBox2.Click

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'emp_login_time = get_server_time()
        mysql_data_str("insert into break_event_log (Emp_id, Login_Date, Start_Time, End_Time, reason, break_type) values ('" & emp_id & "', '" & mysql_date_format(emp_login_date, True) & "', '" & mysql_date_format(emp_action_start_time, False) & "', '" & mysql_date_format(emp_action_stop_time, False) & "', '" & ComboBox2.Text & "', '" & ComboBox1.SelectedValue & "')")
        mysql_data_str("update tbl_emp_monitor set updated_on='" & mysql_date_format(emp_action_stop_time, False) & "', current_status='Active' where emp_id='" & emp_id & "'")
        veeBOT.auto_lock_or_manual_lock = 0
        form_closing_id = 1
        messenger_activity_id = 1
        Me.Close()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        show_id = show_id - 1
        Label4.Text = show_id & " sec to mark as Personal Break"

        If show_id = 0 Then
            'If ComboBox1.Text = "Idle Time" Then
            '    LockWorkStation()
            'End If
            'emp_login_time = get_server_time()
            mysql_data_str("insert into break_event_log (Emp_id, Login_Date, Start_Time, End_Time, reason, break_type) values ('" & emp_id & "', '" & mysql_date_format(emp_login_date, True) & "', '" & mysql_date_format(emp_action_start_time, False) & "', '" & mysql_date_format(emp_action_stop_time, False) & "', 'Comments Auto Updated', '11')")
            mysql_data_str("update tbl_emp_monitor set updated_on='" & mysql_date_format(emp_action_stop_time, False) & "', current_status='Active' where emp_id='" & emp_id & "'")
            veeBOT.auto_lock_or_manual_lock = 0
            form_closing_id = 1
            messenger_activity_id = 1
            Timer1.Enabled = False
            Me.Close()
        End If
    End Sub

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork

    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs)
    End Sub
    Sub record_break(brk_id As Integer)
        form_closing_id = 1
        'emp_login_time = get_server_time()
        mysql_data_str("insert into break_event_log (Emp_id, Login_Date, Start_Time, End_Time, reason, break_type) values ('" & emp_id & "', '" & mysql_date_format(emp_login_date, True) & "', '" & mysql_date_format(emp_action_start_time, False) & "', '" & mysql_date_format(emp_action_stop_time, False) & "', '" & ComboBox2.Text & "', '" & brk_id & "')")
        mysql_data_str("update tbl_emp_monitor set updated_on='" & mysql_date_format(emp_action_stop_time, False) & "', current_status='Active' where emp_id='" & emp_id & "'")
        veeBOT.auto_lock_or_manual_lock = 0
        messenger_activity_id = 1
        Me.Close()
    End Sub
    Private Sub RadioButton7_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton7.CheckedChanged
        If RadioButton7.Checked = True Then
            Me.Close()
            record_break(4)
        End If
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton1.Checked = True Then
            If ComboBox2.Text = Nothing Then
                MsgBox("Please enter the break reason")
                Exit Sub
            End If
            Me.Close()
            record_break(3)
        End If
    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton2.CheckedChanged
        If RadioButton2.Checked = True Then
            Me.Close()
            record_break(2)
        End If
    End Sub

    Private Sub RadioButton3_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton3.CheckedChanged
        If RadioButton3.Checked = True Then
            Me.Close()
            record_break(1)
        End If
    End Sub

    Private Sub RadioButton4_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton4.CheckedChanged
        If RadioButton4.Checked = True Then
            Me.Close()
            record_break(11)
        End If
    End Sub

    Private Sub RadioButton5_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton5.CheckedChanged
        If RadioButton5.Checked = True Then
            Me.Close()
            record_break(6)
        End If
    End Sub

    Private Sub RadioButton6_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton6.CheckedChanged
        If RadioButton6.Checked = True Then
            Me.Close()
            record_break(5)
        End If
    End Sub

    Private Sub RadioButton8_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton8.CheckedChanged
        If RadioButton8.Checked = True Then
            Me.Close()
            record_break(15)
        End If
    End Sub

    Private Sub RadioButton9_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton9.CheckedChanged
        If RadioButton8.Checked = True Then
            Me.Close()
            record_break(8)
        End If
    End Sub

    Private Sub Button3_Click_1(sender As Object, e As EventArgs) Handles Button3.Click
        Try
            If MaskedTextBox1.Text = Label5.Text Then
                Exit Sub
            End If
            If MaskedTextBox1.Text = "  :  :" Then
                Exit Sub
            End If
            Dim date1 As Date = CDate("01/01/01 " & Label5.Text)
            Dim date2 As Date = CDate("01/01/01 " & MaskedTextBox1.Text)
            If date2 < date1 Then
                Panel4.Visible = True
                MaskedTextBox2.Text = Format(DateAdd(DateInterval.Second, DateDiff(DateInterval.Second, date2, date1), CDate("01/01/01 00:00:00")), "HH:mm:ss")
            Else
                MaskedTextBox1.Text = Label5.Text
            End If
        Catch
            Exit Sub
        End Try
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Try
            If MaskedTextBox2.Text = "  :  :" Then
                Exit Sub
            End If
            Panel5.Visible = True
        Catch
            Exit Sub
        End Try

    End Sub

    Private Sub RadioButton11_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton11.CheckedChanged
        Timer1.Enabled = False
        Panel2.Visible = True
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Try

            If breakCB1.Text = Nothing Or MaskedTextBox1.Text = "  :  :" Then
                Exit Sub
            End If

            If breakCB2.Visible = True And (breakCB2.Text = Nothing Or MaskedTextBox2.Text = "  :  :") Then
                Exit Sub
            End If

            If breakCB3.Visible = True And (breakCB3.Text = Nothing Or MaskedTextBox3.Text = "  :  :") Then
                Exit Sub
            End If

            form_closing_id = 1
            'emp_login_time = get_server_time()
            Dim break_end_time As Date
            Dim break_end_time1 As Date
            Dim break_end_time2 As Date

            Dim date1 As Date = CDate("01/01/01 " & MaskedTextBox1.Text)
            Dim date2 As Date = CDate("01/01/01 " & MaskedTextBox2.Text)
            Dim date3 As Date = CDate("01/01/01 " & MaskedTextBox3.Text)

            If breakCB1.Visible = True Then
                MsgBox(DateDiff(DateInterval.Second, CDate("01/01/01 00:00:00"), date1))
                MsgBox(date1.Second)
                break_end_time = CDate(DateAdd(DateInterval.Second, date1.Second, emp_action_start_time))
                mysql_data_str("insert into break_event_log (Emp_id, Login_Date, Start_Time, End_Time, reason, break_type) values ('" & emp_id & "', '" & mysql_date_format(emp_login_date, True) & "', '" & mysql_date_format(emp_action_start_time, False) & "', '" & mysql_date_format(break_end_time, False) & "', '" & ComboBox2.Text & "', '" & ComboBox1.SelectedValue & "')")
            End If

            If breakCB2.Visible = True Then
                break_end_time1 = CDate(DateAdd(DateInterval.Second, date2.Second, break_end_time))
                mysql_data_str("insert into break_event_log (Emp_id, Login_Date, Start_Time, End_Time, reason, break_type) values ('" & emp_id & "', '" & mysql_date_format(emp_login_date, True) & "', '" & mysql_date_format(break_end_time, False) & "', '" & mysql_date_format(break_end_time1, False) & "', '" & ComboBox2.Text & "', '" & ComboBox1.SelectedValue & "')")
            End If

            If breakCB3.Visible = True Then
                break_end_time2 = CDate(DateAdd(DateInterval.Second, date3.Second, break_end_time1))
                mysql_data_str("insert into break_event_log (Emp_id, Login_Date, Start_Time, End_Time, reason, break_type) values ('" & emp_id & "', '" & mysql_date_format(emp_login_date, True) & "', '" & mysql_date_format(break_end_time1, False) & "', '" & mysql_date_format(break_end_time2, False) & "', '" & ComboBox2.Text & "', '" & ComboBox1.SelectedValue & "')")
            End If

            mysql_data_str("update tbl_emp_monitor set updated_on='" & mysql_date_format(emp_action_stop_time, False) & "', current_status='Active' where emp_id='" & emp_id & "'")
            veeBOT.auto_lock_or_manual_lock = 0
            messenger_activity_id = 1
            Me.Close()
        Catch ex As Exception
            Err.Clear()
            Me.Close()
        End Try
    End Sub
End Class