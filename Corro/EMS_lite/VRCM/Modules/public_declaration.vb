Imports System.Net.IPAddress
Imports System.Runtime.InteropServices
Imports MySql.Data.MySqlClient
Imports MongoDB.Bson
Imports MongoDB.Driver

Module public_declaration
    Public appDgv As DataGridView
    Public BreakDgv As DataGridView
    Public emp_active_start As Date
    Public emp_active_end As Date
    Public vbot_end_date As Date = "2021-12-31"
    Public current_version As String = "EMS_Lite_Ver5"
    Public mongo_client As MongoClient
    Public mango_db As IMongoDatabase
    Public machine_name As String
    Public temp_end_time As Date
    Public app_start_time As Date
    Public emp_activity_busy As Integer = 0
    Public emp_activity As Integer = 1
    Public form_closing_id As Integer = 0
    Public Application_Exit_code As Integer = 0
    Public Home_Form_Closing_int As Integer = 1
    Public Error_description As String
    Public function_return_str As String
    Public function_return_int As Integer
    Public Permission_Late_check As Boolean = False
    Public communicator_late_check As Integer
    Public messengers(1000000) As Messenger
    Public notify_last_process_action As Integer = 0
    Public notify_last_break_action As Integer = 0
    Public notify_last_group_reporting1_action As Integer = 0

    Public monitor_table_status As Integer

    Public emp_current_action As String
    Public emp_action_start_time As Date
    Public emp_action_stop_time As Date
    Public sys_lock_status As String
    Public set_calender_date As Date
    Public emp_shift_time As DateTime
    Public system_lock_secs As Integer = 180 'seconds

    Public Break_seconds_indicator_Int As Integer = 32400

    Public messenger_activity_id As Integer '1 = active, 0 - Inactive
    Public messenger_activity_message As String

    Public idle_break_data_set As DataSet
    Public normal_break_data_set As DataSet
    Public sub_break_data_set As DataSet

    Public emp_project_group As String
    Public emp_sys_name As String
    Public emp_Track_App As Integer
    Public emp_id As Integer
    Public emp_name As String
    Public emp_status As Integer
    Public emp_access_level As Integer
    Public emp_system_name As String
    Public emp_login_date As Date
    Public emp_login_time As Date
    Public server_time As Date = get_server_time()
    Public emp_ip_address As String
    Public emp_location As Integer
    Public Function mongo_connect() As IMongoDatabase
        mongo_client = New MongoClient(mongo_conn)
        mango_db = mongo_client.GetDatabase("vrcm_ems")
        Return mango_db
    End Function
    Public Function Trim_Replace(ByVal Str_Input As String) As String
        If Str_Input <> Nothing Then
            Str_Input = Trim(Str_Input.Replace("'", "''"))
            Str_Input = Trim(Str_Input.Replace("  ", " "))
            Str_Input = Trim(Str_Input.Replace(vbCrLf, " "))
            Str_Input = Trim(Str_Input.Replace("\", "\\"))
        End If
        Return Str_Input
    End Function
    Function get_server_time() As Date
        Dim ds1 As DataSet = mysql_data_str("select now()")
        Return ds1.Tables(0).Rows(0)(0).ToString
    End Function
    Function get_last_action_time(tmp_emp_id As Integer) As Integer
        Dim ds1 As DataSet = mysql_data_str("SELECT ROUND(TIME_TO_SEC(TIMEDIFF(NOW(),updated_on))/3600,0) FROM tbl_emp_monitor WHERE emp_id='" & tmp_emp_id & "'")
        Return Val(ds1.Tables(0).Rows(0)(0).ToString)
    End Function

    Public Function mysql_date_format(date_str As Date, type_str As Boolean) As String
        If type_str = True Then
            mysql_date_format = Format(date_str, "yyyy-MM-dd")
        Else
            mysql_date_format = Format(date_str, "yyyy-MM-dd HH:mm:ss")
        End If
        Return mysql_date_format
    End Function


    Public Function update_emp_activity(ByVal act As Integer)
        Try

            conn1.Open()
            Dim sda11 As MySqlDataAdapter
            Dim sds11 As DataSet

            If act = 1 And emp_activity_busy = 1 Then
                act = 3
            End If

            sds11 = mysql_data_str("update emp_monitor set sys_status = '" & act & "', last_active_time = now() where emp_id = '" & emp_id & "'")

            sda11 = Nothing
            sds11 = Nothing
            Call conn1.Close()

            'Home.Sbar_status_icon.Text = ""

            If act = 1 Then
                '   Home.Sbar_status_icon.Image = ProTrak.My.Resources.Available
                messenger_activity_id = 1
            ElseIf act = 2 Then
                '  Home.Sbar_status_icon.Image = ProTrak.My.Resources.Away
                messenger_activity_id = 0
            ElseIf act = 3 Then
                ' Home.Sbar_status_icon.Image = ProTrak.My.Resources.Busy
                messenger_activity_id = 1
            Else
                'Home.Sbar_status_icon.Image = ProTrak.My.Resources.Unavailable
                messenger_activity_id = 0
            End If

            Return 0

        Catch ex As Exception
            'MsgBox("test: " & Err.Description)
            Err.Clear()
            Return 0
        End Try
    End Function
    Function ip_address() As String
        Dim hostName As String = System.Net.Dns.GetHostName
        'Console.WriteLine("Host Name : " & hostName & vbNewLine)
        Dim Address = System.Net.Dns.GetHostEntry(hostName).AddressList()
        Return Address(2).ToString
    End Function

    Public Sub action_track(ByVal process As String, ByVal work_type As String)
        mysql_data_str("update tbl_emp_monitor set current_status='Idle Time' where Emp_id = '" & emp_id & "'")
    End Sub

    Public Function Session_Check() As Boolean

        Call call_connect()
        Return True
        'Dim sda6 As MySqlDataAdapter
        'Dim sds6 As DataSet
        'sda6 = New MySqlDataAdapter("select * from emp_login_session where emp_id = '" & emp_id & "' and login_date = '" & mysql_date_format(emp_login_date, True) & "' order by id desc limit 1", conn1)
        'sds6 = New DataSet
        'sda6.Fill(sds6, "login_info")

        'temp_end_time = sds6.Tables(0).Rows(0).Item("stop_time").ToString

        'If DateDiff(DateInterval.Hour, temp_end_time, get_server_time()) >= 9 Then
        '    Return False
        'Else
        '    Return True
        'End If

        'sda6 = Nothing
        'sds6 = Nothing
        conn1.Close()
    End Function

    Function get_emp_id_from_full_name(tmp_emp_full_name) As Integer
        Dim ds1 As DataSet
        ds1 = mysql_data_str("select emp_id from mas_emp_detail where concat(emp_first_name,' ',emp_last_name)='" & tmp_emp_full_name & "'")
        If ds1.Tables(0).Rows.Count = 0 Then
            Return 0
        Else
            Return ds1.Tables(0).Rows(0)(0).ToString
        End If
    End Function
    Function get_full_name_from_emp_id(tmp_emp_id) As String
        Dim ds1 As DataSet
        ds1 = mysql_data_str("select concat(emp_first_name,' ',emp_last_name) from mas_emp_detail where emp_id='" & tmp_emp_id & "'")
        If ds1.Tables(0).Rows.Count = 0 Then
            Return 0
        Else
            Return ds1.Tables(0).Rows(0)(0).ToString
        End If
    End Function
    Function load_conection(t_emp_id As String, t_login_date As Date, t_start_time As Date, t_app_name As String, t_app_name_group As String)
        Dim client As MongoClient
        Dim db As IMongoDatabase

        client = New MongoClient(mongo_conn)
        db = client.GetDatabase("vrcm_ems")

        Dim collection As IMongoCollection(Of BsonDocument) = db.GetCollection(Of BsonDocument)("application_tracking")

        Dim emp As BsonDocument = New BsonDocument
        With emp
            .Add("emp_id", t_emp_id.ToString) 'Guid.NewGuid().ToString)
            .Add("login_date", t_login_date)
            .Add("start_time", t_start_time)
            '.Add("login_date", mysql_date_format(t_login_date, True).ToString)
            '.Add("start_time", mysql_date_format(t_start_time, False).ToString)
            .Add("end_time", "")
            .Add("app_name", t_app_name.ToString)
            .Add("appnamegroup", t_app_name_group.ToString)

            '.Add("emp_id", t_emp_id) 'Guid.NewGuid().ToString)
            '.Add("login_date", t_login_date)
            '.Add("start_time", t_start_time)
            '.Add("end_time", t_end_time)
            '.Add("app_name", t_app_name)
            '.Add("appnamegroup", t_app_name_group)

        End With
        app_start_time = t_start_time
        collection.InsertOne(emp)
        Return 0

    End Function

    Function update_document(t_emp_id As String, t_end_time As String, t_start_time As String)
        'Dim client As MongoClient
        'Dim db As IMongoDatabase

        'client = New MongoClient(mongo_conn)
        'db = client.GetDatabase("vrcm_ems")
        'Dim collection As IMongoCollection(Of BsonDocument) = db.GetCollection(Of BsonDocument)("application_tracking")

        'Dim fltr = Builders(Of BsonDocument).Filter.Eq(Of String)("emp_id", t_emp_id)
        'Dim fltr1 = Builders(Of BsonDocument).Filter.Eq(Of String)("end_time", "")
        'Dim fltr2 = Builders(Of BsonDocument).Filter.Eq(Of String)("login_date", mysql_date_format(emp_login_date, True).ToString)
        ''Dim fltr1 = Builders(Of BsonDocument).Filter.Eq(Of String)("start_time", t_start_time)
        'Dim comb_filt = fltr And fltr1 And fltr2
        ''collection.UpdateMany(comb_filt, New BsonDocument("$set", New BsonDocument("end_time", t_end_time)))
        'collection.UpdateMany(comb_filt, New BsonDocument("$set", New BsonDocument("end_time", mysql_date_format(t_end_time, False))))

        Return 0
    End Function

    Public Function get_project_id_from_string(project_str As String) As Integer
        Dim ds1 As DataSet
        ds1 = mysql_data_str("select client_id from mas_project_list where project_name='" & project_str & "'")
        If ds1.Tables(0).Rows.Count <> 0 Then
            Return Val(ds1.Tables(0).Rows(0)(0).ToString)
        End If
        Return 0
    End Function

    Public Function create_messengers(ByVal form_emp_id As Integer, ByVal form_emp_name As String)
        Try
            messengers(form_emp_id).Show()
            'messengers(form_emp_id).Activate()
            messengers(form_emp_id).Timer1.Enabled = True


            If messengers(form_emp_id).WindowState = FormWindowState.Minimized Then
                messengers(form_emp_id).WindowState = FormWindowState.Normal
            End If

            messengers(form_emp_id).BringToFront()
            messengers(form_emp_id).TopMost = True
            Return 0
        Catch ex As Exception
            If Err.Number = 5 Or Err.Number = 91 Then
                messengers(form_emp_id) = New Messenger
                messengers(form_emp_id).Text = form_emp_name
                messengers(form_emp_id).Name = Str(form_emp_id)
                messengers(form_emp_id).Show()
                messengers(form_emp_id).TextBox1.Focus()
                '   messengers(form_emp_id).Activate()
                messengers(form_emp_id).Timer1.Enabled = True
                messengers(form_emp_id).BringToFront()
                messengers(form_emp_id).TopMost = True

                If messengers(form_emp_id).WindowState = FormWindowState.Minimized Then
                    messengers(form_emp_id).WindowState = FormWindowState.Normal
                End If
            End If
            Err.Clear()
            Return 0
            Exit Function
        End Try
    End Function


End Module
