Namespace My

    ' The following events are available for MyApplication:
    ' 
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    Partial Friend Class MyApplication

        Private Sub MyApplication_Shutdown( _
    ByVal sender As Object, _
    ByVal e As System.EventArgs _
) Handles Me.Shutdown
            emp_action_start_time = get_server_time()
            mysql_data_str("UPDATE `emp_active_hours` SET end_time=NOW() WHERE emp_id='" & emp_id & "' AND login_date='" & mysql_date_format(emp_login_date, True) & "' AND end_time IS NULL")
            mysql_data_str("UPDATE emp_login_log SET logout_comment='User LogOff', end_time='" & mysql_date_format(get_server_time, False) & "' where emp_id='" & emp_id & "' and login_date='" & mysql_date_format(emp_login_date, True) & "';")
            mysql_data_str("update tbl_emp_monitor set system_ip_address='', system_name='', current_status='Shift Logout' where emp_id='" & emp_id & "'")
        End Sub
    End Class

End Namespace

