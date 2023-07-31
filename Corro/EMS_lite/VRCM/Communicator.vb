Imports MySql.Data.MySqlClient
Imports System.Data.Odbc
Public Class Communicator
    Dim group_selected_Temp As String = Nothing
    Dim process_selected_Temp As String = Nothing
    Private Sub Communicator_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Me.Icon = My.Resources.icon
            Dim ds1 As DataSet = mysql_data_str("select now()")
            'Label1.Text = ds1.Tables(0).Rows(0)(0).ToString
            Me.Visible = True
            Dim x As Integer
            Dim y As Integer
            x = Screen.PrimaryScreen.WorkingArea.Width
            y = Screen.PrimaryScreen.WorkingArea.Height - Me.Height

            Do Until x = Screen.PrimaryScreen.WorkingArea.Width - Me.Width
                x = x - 1
                Me.Location = New Point(x, y)
            Loop
            load_location_combo()
            load_emp_detail()
            treeview_list()
        Catch ex As Exception
            Err.Clear()
            Exit Sub
        End Try

    End Sub

    Sub load_emp_detail(Optional tmp_emp_name As String = Nothing, Optional sub_qry As String = Nothing)
        Try
            Label7.Visible = True
            Label6.Visible = True
            DataGridView1.Rows.Clear()
            Dim ds1 As DataSet
            If emp_id = 2355 Or emp_id = 347 Or emp_id = 348 Then
                If tmp_emp_name = Nothing Then
                    ds1 = mysql_data_str("SELECT a.*, b.* FROM mas_emp_detail a LEFT JOIN tbl_emp_monitor b ON b.emp_id=a.emp_id where a.emp_status=0 " & sub_qry & " order by CONCAT(a.emp_first_name, ' ',a.emp_last_name)")
                    Dim ds2 As DataSet
                    ds2 = mysql_data_str("SELECT SUM(CASE WHEN b.Current_Status='Active' THEN 1 ELSE 0 END) Active, SUM(CASE WHEN b.Current_Status NOT IN ('Active','Shift Logout') THEN 1 ELSE 0 END) Away, COUNT(a.emp_id) - (SUM(CASE WHEN b.Current_Status='Active' THEN 1 ELSE 0 END)+SUM(CASE WHEN b.Current_Status NOT IN ('Active','Shift Logout') THEN 1 ELSE 0 END)) Unavailable FROM mas_emp_detail a LEFT JOIN tbl_emp_monitor b ON b.emp_id=a.emp_id WHERE a.emp_status=0 " & sub_qry & " ORDER BY CONCAT(a.emp_first_name, ' ',a.emp_last_name)")
                    lnkLblActive.Text = ds2.Tables(0).Rows(0)(0).ToString
                    lnkLblAway.Text = ds2.Tables(0).Rows(0)(1).ToString
                    lnkLblOut.Text = ds2.Tables(0).Rows(0)(2).ToString
                Else
                    ds1 = mysql_data_str("SELECT a.*, b.* FROM mas_emp_detail a LEFT JOIN tbl_emp_monitor b ON b.emp_id=a.emp_id where a.emp_status=0 and (a.emp_system_name like '%" & tmp_emp_name & "%' or CONCAT(a.emp_first_name, ' ',a.emp_last_name) like '%" & tmp_emp_name & "%') order by CONCAT(a.emp_first_name, ' ',a.emp_last_name)")
                End If
            Else
                If tmp_emp_name = Nothing Then
                    ds1 = mysql_data_str("SELECT a.*, b.* FROM mas_emp_detail a LEFT JOIN tbl_emp_monitor b ON b.emp_id=a.emp_id where (b.reporting1='" & emp_id & "' or b.reporting2='" & emp_id & "') and a.emp_status=0 " & sub_qry & " order by CONCAT(a.emp_first_name, ' ',a.emp_last_name)")
                    Dim ds2 As DataSet
                    ds2 = mysql_data_str("SELECT SUM(CASE WHEN b.Current_Status='Active' THEN 1 ELSE 0 END) Active, SUM(CASE WHEN b.Current_Status NOT IN ('Active','Shift Logout') THEN 1 ELSE 0 END) Away, COUNT(a.emp_id) - (SUM(CASE WHEN b.Current_Status='Active' THEN 1 ELSE 0 END)+SUM(CASE WHEN b.Current_Status NOT IN ('Active','Shift Logout') THEN 1 ELSE 0 END)) Unavailable FROM mas_emp_detail a LEFT JOIN tbl_emp_monitor b ON b.emp_id=a.emp_id WHERE (b.reporting1='" & emp_id & "' or b.reporting2='" & emp_id & "') and a.emp_status=0 ORDER BY CONCAT(a.emp_first_name, ' ',a.emp_last_name)")
                    lnkLblActive.Text = ds2.Tables(0).Rows(0)(0).ToString
                    lnkLblAway.Text = ds2.Tables(0).Rows(0)(1).ToString
                    lnkLblOut.Text = ds2.Tables(0).Rows(0)(2).ToString
                Else
                    ds1 = mysql_data_str("SELECT a.*, b.* FROM mas_emp_detail a LEFT JOIN tbl_emp_monitor b ON b.emp_id=a.emp_id where (b.reporting1='" & emp_id & "' or b.reporting2='" & emp_id & "') and a.emp_status=0 and (a.emp_system_name like '%" & tmp_emp_name & "%' or CONCAT(a.emp_first_name, ' ',a.emp_last_name) like '%" & tmp_emp_name & "%') order by CONCAT(a.emp_first_name, ' ',a.emp_last_name)")
                End If

            End If

            Dim i As Integer
            Do Until i = ds1.Tables(0).Rows.Count

                If ds1.Tables(0).Rows(i).Item("Current_Status").ToString = "Active" Then
                    DataGridView1.Rows.Add(My.Resources.active, ds1.Tables(0).Rows(i).Item("emp_id").ToString, ds1.Tables(0).Rows(i).Item("emp_system_name").ToString, (ds1.Tables(0).Rows(i).Item("emp_first_name").ToString & " " & ds1.Tables(0).Rows(i).Item("emp_last_name").ToString), My.Resources.mid_icon, My.Resources.up_icon)
                ElseIf ds1.Tables(0).Rows(i).Item("Current_Status").ToString = "Shift Logout" Then
                    DataGridView1.Rows.Add(My.Resources.out, ds1.Tables(0).Rows(i).Item("emp_id").ToString, ds1.Tables(0).Rows(i).Item("emp_system_name").ToString, (ds1.Tables(0).Rows(i).Item("emp_first_name").ToString & " " & ds1.Tables(0).Rows(i).Item("emp_last_name").ToString), My.Resources.night_icon, My.Resources.up_icon)
                Else
                    DataGridView1.Rows.Add(My.Resources.away, ds1.Tables(0).Rows(i).Item("emp_id").ToString, ds1.Tables(0).Rows(i).Item("emp_system_name").ToString, (ds1.Tables(0).Rows(i).Item("emp_first_name").ToString & " " & ds1.Tables(0).Rows(i).Item("emp_last_name").ToString), My.Resources.mid_icon, My.Resources.down_icon)
                End If
                i = i + 1
            Loop
            Label7.Text = ds1.Tables(0).Rows.Count
        Catch ex As Exception
            Err.Clear()
            Exit Sub
        End Try

    End Sub
    Private Sub Form1_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        If Me.WindowState = FormWindowState.Minimized Then
            NotifyIcon1.Visible = True
            NotifyIcon1.Icon = My.Resources.icon
            NotifyIcon1.BalloonTipIcon = ToolTipIcon.Info
            NotifyIcon1.BalloonTipTitle = "Communicator"
            NotifyIcon1.BalloonTipText = "powered by VRCM-AppDev"
            NotifyIcon1.ShowBalloonTip(50000)
            'Me.Hide()
            ShowInTaskbar = False
        End If
    End Sub

    Private Sub NotifyIcon1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles NotifyIcon1.DoubleClick
        'Me.Show()
        ShowInTaskbar = True
        Me.WindowState = FormWindowState.Normal
        NotifyIcon1.Visible = False
    End Sub

    Private Sub TableLayoutPanel1_Paint(sender As Object, e As PaintEventArgs) Handles TableLayoutPanel1.Paint

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        loc.Activate()
        loc.Show()

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)
        Messenger.Show()
        Messenger.Show()
    End Sub

    Private Sub TableLayoutPanel2_Paint(sender As Object, e As PaintEventArgs)

    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub InstantMessageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles InstantMessageToolStripMenuItem.Click
        Messenger.Text = DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells("col_name").Value & "(" & DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells("crm_id").Value & ")"
        Messenger.Show()
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub TextBox1_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles TextBox1.Validating
        load_emp_detail(TextBox1.Text)
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        load_emp_detail(TextBox1.Text)
    End Sub
    Sub load_location_combo()
        Try
            Dim ds1 As DataSet = Nothing
            ds1 = mysql_data_str("select * from mas_work_location")
            With ComboBox1
                .DataSource = ds1.Tables(0)
                .DisplayMember = "Work_Location_Name"
                .ValueMember = "work_location_id"
                .SelectedValue = emp_location
            End With
        Catch ex As Exception
            Err.Clear()
            Exit Sub
        End Try

    End Sub

    Private Sub ComboBox1_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ComboBox1.Validating
        load_emp_detail()
    End Sub

    Private Sub lnkLblActive_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnkLblActive.LinkClicked
        load_emp_detail("", " and b.Current_Status ='Active'")
    End Sub

    Private Sub lnkLblAway_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnkLblAway.LinkClicked
        load_emp_detail("", " and b.Current_Status not in ('Active','Shift Logout') ")
    End Sub

    Private Sub lnkLblOut_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnkLblOut.LinkClicked
        load_emp_detail("", " and b.Current_Status not in ('Active','System Locked') ")
    End Sub

    Private Sub LocateToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LocateToolStripMenuItem.Click
        FloorView.Show()
        FloorView.Activate()
    End Sub

    Private Sub UserInfoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UserInfoToolStripMenuItem.Click
        User_info.load_grid(DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells("col_emp_id").Value)
        User_info.Activate()
        User_info.Show()
    End Sub

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
        Messenger.Text = DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells("col_name").Value & "(" & DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells("crm_id").Value & ")"
        Messenger.Show()

    End Sub

    Private Sub Button2_Click_1(sender As Object, e As EventArgs) Handles Button2.Click
        load_emp_detail()
    End Sub
    Sub treeview_list()
        Try

            Dim ds As DataSet
            Dim gp_msg As String = Nothing
            Dim i As Integer = 0
            If emp_id = 2355 Then
                ds = mysql_data_str("select CONCAT(emp_first_name, ' ', emp_last_name) emp_name FROM mas_emp_detail WHERE emp_status=0 AND emp_access_level=4 ORDER BY emp_first_name")
            Else
                ds = mysql_data_str("SELECT CONCAT(med.emp_first_name, ' ', med.emp_last_name) emp_name FROM mas_emp_detail med            LEFT JOIN  `tbl_emp_monitor` tem ON tem.emp_id=med.emp_id            WHERE med.emp_status=0 AND med.emp_access_level <> 6 AND             (reporting1='" & emp_id & "'             OR reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1='" & emp_id & "' )             OR reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1='" & emp_id & "' ))            OR reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1='" & emp_id & "' )))            OR reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1='" & emp_id & "' ))))            OR reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1='" & emp_id & "' ))))))")
            End If

            If ds.Tables(0).Rows.Count <> 0 Then
                GroupMessage.Visible = True
                Do Until i = ds.Tables(0).Rows.Count
                    TreeView1.Nodes.Add(ds.Tables(0).Rows(i)(0).ToString())
                    i = i + 1
                Loop
            End If


            'i = 0
            'Dim ds2 As DataSet
            'ds2 = mysql_data_str("SELECT fpg_com_gro.Type, fpg_com_gro.Level2 FROM tbl_emp_communicator fpg_com_gro GROUP BY fpg_com_gro.Type, fpg_com_gro.Level2 ORDER BY TYPE, Level2")

            'If ds2.Tables(0).Rows.Count <> 0 Then
            '    Do Until i = ds2.Tables(0).Rows.Count
            '        For Each nd As TreeNode In TreeView1.Nodes
            '            Dim _nd As TreeNode = _Find_Node(nd, ds2.Tables(0).Rows(i)(0).ToString())
            '            If Not _nd Is Nothing Then
            '                _nd.Nodes.Add(ds2.Tables(0).Rows(i)(1).ToString())
            '                Exit For
            '            End If
            '        Next
            '        i = i + 1
            '    Loop
            'End If

        Catch ex As Exception
            Err.Clear()
            Exit Sub
        End Try

    End Sub
    Private Function _Find_Node(ByVal Node As TreeNode, ByVal Text As String, Optional ByRef Found As Boolean = False) As TreeNode
        'This is really not part of your requirement. But it is to add items to the treeview.
        If Node.Text.ToLower() <> Text.ToLower() Then
            For Each SubNode As TreeNode In Node.Nodes
                Dim _FoundNode As TreeNode = _Find_Node(SubNode, Text, Found)
                If Not _FoundNode Is Nothing Then
                    Found = True
                    Return _FoundNode
                End If
            Next
        Else
            Found = True
            Return Node
        End If
        Return Nothing
    End Function


    Private Sub TreeView1_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles TreeView1.AfterSelect
        group_selected_Temp = "Process"
        On Error Resume Next
        'Dim sup_name As String() = TreeView1.SelectedNode.Text.Split("_")

        If TreeView1.SelectedNode.Text <> Nothing Then
            process_selected_Temp = TreeView1.SelectedNode.Text
            'load_emp_detail("", " and b.assigned_process='" & get_project_id_from_string(group_selected_Temp) & "'")
            Dim rep_id As String = get_emp_id_from_full_name(TreeView1.SelectedNode.Text)
            load_emp_detail("", " and (reporting1='" & rep_id & "' OR reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1='" & rep_id & "')         OR reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1='" & rep_id & "'))        OR reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1='" & rep_id & "')))        OR reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1='" & rep_id & "'))))        OR reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1 IN (SELECT emp_id FROM `tbl_emp_monitor` WHERE reporting1='" & rep_id & "'))))))")
            Dim lbl_process_text As String = process_selected_Temp
        End If
    End Sub

    Private Sub GroupMessage_Click(sender As Object, e As EventArgs) Handles GroupMessage.Click
        'Dim sup_name As String() = TreeView1.SelectedNode.Text.Split("_")
        group_message.Label6.text = get_emp_id_from_full_name(TreeView1.SelectedNode.Text)
        group_message.Show()
        group_message.Activate()
        group_message.load_grid()
    End Sub

    Private Sub GroupMessageAllToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GroupMessageAllToolStripMenuItem.Click
        group_message.Label6.Text = 0
        group_message.Show()
        group_message.Activate()
        group_message.load_grid()
    End Sub
End Class
