﻿Imports MySql.Data.MySqlClient
Imports System.Security.Cryptography
Imports System.IO

Module connection_module
    Private enc As System.Text.UTF8Encoding
    Private encryptor As ICryptoTransform
    Private decryptor As ICryptoTransform

    'Public conn1 As New MySqlConnection("Server=localhost;Database=user_information;Uid=root;Pwd=;  Connection Timeout=240")
    'Public mongo_conn As String = "mongodb://172.29.2.131/"

    Public conn1 As New MySqlConnection("Server=172.29.2.68;Database=user_information; Uid=root;Pwd='veeb0t#2o1n1ne'; Connection Timeout=240")
    Public mongo_conn As String = "mongodb://172.29.2.68/"

    Public conn2 As New MySqlConnection("Server=localhost;Database=wfm_vpms_2019; Uid=root;Pwd=; Connection Timeout=240")

    Function encrypt_str(encrypt_str_text As String) As String
        Dim sPlainText As String = encrypt_str_text
        If Not String.IsNullOrEmpty(sPlainText) Then
            Dim memoryStream As MemoryStream = New MemoryStream()
            Dim cryptoStream As CryptoStream = New CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write)
            cryptoStream.Write(enc.GetBytes(sPlainText), 0, sPlainText.Length)
            cryptoStream.FlushFinalBlock()
            encrypt_str_text = Convert.ToBase64String(memoryStream.ToArray())
            memoryStream.Close()
            cryptoStream.Close()
            Return encrypt_str_text
        End If
        Return 0
    End Function

    Public Function checkConnection(MySqlqry As String) As Boolean
        Try
            Dim ds1 As DataSet = Nothing
            call_connect()
            Dim cmd As MySqlCommand = New MySqlCommand(MySqlqry, conn1)
            Dim da As New MySqlDataAdapter 'DataAdapter can be used to fill DataSet
            Dim ds As New DataSet
            da.SelectCommand = cmd
            da.Fill(ds, "student")
            Return True
        Catch ex As Exception
            Return False
        Finally
            call_disconnect()
        End Try
    End Function
    Public Function mysql_data_str(MySqlqry As String, Optional connId As Integer = 0) As DataSet
        Try
            Dim ds1 As DataSet = Nothing
            call_connect()

            Dim cmd As MySqlCommand = New MySqlCommand(MySqlqry, conn1)
            'Dim version As String
            Dim da As New MySqlDataAdapter 'DataAdapter can be used to fill DataSet
            Dim ds As New DataSet
            da.SelectCommand = cmd
            da.Fill(ds, "student")
            'call_disconnect()
            Return ds
        Catch ex As Exception
            'MsgBox("Database Connection Issue. Contact AppDev Team.", MsgBoxStyle.Critical)
            'call_disconnect()
            form_closing_id = 1
            loc.Close()
            'End
            Dim ds2 As DataSet = Nothing
            Return ds2
        Finally
            call_disconnect()

        End Try
    End Function
    Public Function mysql_data_str_live(MySqlqry As String, ByVal ParamArray variableArray() As String) As DataSet
        Try
            Dim ds1 As DataSet = Nothing
            conn2.Open()
            Dim cmd As MySqlCommand = New MySqlCommand(MySqlqry, conn2)
            'Dim version As String
            Dim da As New MySqlDataAdapter 'DataAdapter can be used to fill DataSet
            Dim ds As New DataSet
            da.SelectCommand = cmd

            da.Fill(ds, "student")
            conn2.Dispose()
            Return ds
        Catch ex As Exception
            conn2.Dispose()
            Dim ds2 As DataSet = Nothing
            Return ds2

        Finally
            conn2.Dispose()
        End Try
    End Function

    Public Sub call_connect()
        If conn1.State = ConnectionState.Open Then
            conn1.Dispose()
        Else
            conn1.Open()
        End If
    End Sub
    Public Sub call_disconnect()
        If conn1.State = ConnectionState.Open Then
            'conn1.Close()
            conn1.Dispose()
            'conn1 = Nothing

        End If
    End Sub
End Module
