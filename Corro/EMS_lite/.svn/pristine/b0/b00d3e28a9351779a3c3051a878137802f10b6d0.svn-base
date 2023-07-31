Imports Microsoft.Win32
Imports System.Runtime.InteropServices
Imports System
Imports System.IO
Imports System.Data.Odbc
Imports System.Management


Module System_info_module
    Public Family_str As String = Nothing

    Public Sub System_info_update(ByVal my_system_ip As String)
        Try

            Dim os As OperatingSystem = Environment.OSVersion
            Dim ossverr As String = My.Computer.Info.OSFullName
            ossverr = ossverr.Replace("Microsoft Windows", "").Trim
            ossverr = Trim_Replace(Microsoft.VisualBasic.Left(ossverr, 200))

            Dim office_version As String = Nothing
            Dim SP_Str As String = Nothing
            SP_Str = os.ServicePack.ToString()
            SP_Str = SP_Str.Replace("Service Pack", "").Trim
            SP_Str = Trim_Replace(Microsoft.VisualBasic.Left(SP_Str, 200))

            Family_str = Nothing
            Family_str = System.Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE")
            Family_str = Trim_Replace(Microsoft.VisualBasic.Left(Family_str, 200))


            Dim monitor_int As Integer = Nothing
            monitor_int = Val(System.Windows.Forms.SystemInformation.MonitorCount)


            Dim Processor_Str As String = Nothing
            Processor_Str = Trim(My.Computer.Registry.LocalMachine.OpenSubKey("HARDWARE\DESCRIPTION\System\CentralProcessor\0").GetValue("ProcessorNameString").ToString.Trim()).Replace("  ", " ")
            Processor_Str = Trim_Replace(Microsoft.VisualBasic.Left(Processor_Str, 250))

            Dim Memory_str As String = Nothing
            Memory_str = FormatBigBytes(My.Computer.Info.TotalPhysicalMemory, 2)
            Memory_str = Trim_Replace(Microsoft.VisualBasic.Left(Memory_str, 25))

            Dim Adobe_AS As String = Nothing
            Adobe_AS = get_adobe_Version()
            Adobe_AS = Trim_Replace(Microsoft.VisualBasic.Left(Adobe_AS, 1))

            Dim excel_version As String = Nothing
            Dim word_version As String = Nothing
            excel_version = get_Excel_Version()



            If Val(excel_version) > 0 Then
                word_version = get_Word_Version()

                If Val(word_version) > 0 Then
                    office_version = "Office - " & excel_version
                Else
                    office_version = "Excel - " & excel_version
                End If
            Else
                office_version = "N/A"
            End If
            office_version = Trim_Replace(Microsoft.VisualBasic.Left(office_version, 25))

            Dim thetotalSpace As Decimal = Nothing
            Dim HD_Size As String = Nothing

            For Each curDrive As DriveInfo In My.Computer.FileSystem.Drives
                If curDrive.DriveType = DriveType.Fixed Then
                    thetotalSpace = thetotalSpace + (curDrive.TotalSize / 1024 / 1024 / 1024)
                End If
            Next

            thetotalSpace = Math.Round(thetotalSpace, 1)
            HD_Size = Trim_Replace(Microsoft.VisualBasic.Left(thetotalSpace.ToString, 15))

            'ListBox1.Items.Clear()
            'Dim moReturn As Management.ManagementObjectCollection
            'Dim moSearch As Management.ManagementObjectSearcher
            'Dim mo As Management.ManagementObject

            'moSearch = New Management.ManagementObjectSearcher("Select * from Win32_Product")

            'moReturn = moSearch.Get

            'mysql_data_str("Update mas_ip_address set last_updated_time = now(), System_Name = '" & machine_name & "', OS_Type = '" & ossverr & "', Service_Pack = '" & SP_Str & "', Family = '" & Family_str & "', Monitor = '" & monitor_int & "', Processor = '" & Processor_Str & "', RAM_Size = '" & Memory_str & "',  `Adobe Reader` = '" & Adobe_AS & "', `Ms-Office` = '" & office_version & "', HD_Size_GB = '" & HD_Size & "' where IP_Address = '" & my_system_ip & "'")
            mysql_data_str("Update mas_ip_address set last_updated_time = now(), IP_Address = '" & my_system_ip & "', OS_Type = '" & ossverr & "', Service_Pack = '" & SP_Str & "', Family = '" & Family_str & "', Monitor = '" & monitor_int & "', Processor = '" & Processor_Str & "', RAM_Size = '" & Memory_str & "',  `Adobe Reader` = '" & Adobe_AS & "', `Ms-Office` = '" & office_version & "', HD_Size_GB = '" & HD_Size & "' where system_name = '" & machine_name & "'")
        Catch ex As Exception
            MsgBox(Err.Description)
            Err.Clear()
            Exit Sub
        End Try
    End Sub


    Private Function FormatBigBytes(ByVal num As Double, ByVal digits As Integer) As String
        Dim postfixes() As String = {"bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"}
        For i As Integer = 0 To postfixes.Length - 1
            If num < 1024 Then
                Return FormatNumber(num, digits) & " " & postfixes(i)
            End If
            num /= 1024
        Next i

        Return FormatNumber(num * 1024, digits) & " " & postfixes(postfixes.Length - 1)
    End Function

    Function get_Excel_Version() As String
        Try
            Dim regKey As RegistryKey

            regKey = My.Computer.Registry.ClassesRoot.OpenSubKey("Excel.Application", False).OpenSubKey("CurVer", False)
            Dim excel_version() As String = regKey.GetValue("").ToString().Split(".")
            Dim excel_sVersion As String = Nothing
            excel_sVersion = excel_version(2)

            Select Case excel_sVersion
                Case "7"
                    excel_sVersion = "95"
                Case "8"
                    excel_sVersion = "97"
                Case "9"
                    excel_sVersion = "2000"
                Case "10"
                    excel_sVersion = "2002"
                Case "11"
                    excel_sVersion = "2003"
                Case "12"
                    excel_sVersion = "2007"
                Case "14"
                    excel_sVersion = "2010"
                Case "15"
                    excel_sVersion = "2012"
                Case Else
                    excel_sVersion = "0"
            End Select


            Return excel_sVersion
            Exit Function
        Catch ex As Exception
            Return "0"
            Err.Clear()
            Exit Function
        End Try
    End Function

    Function get_Word_Version() As String
        Try
            Dim regKey As RegistryKey

            regKey = My.Computer.Registry.ClassesRoot.OpenSubKey("Word.Application", False).OpenSubKey("CurVer", False)
            Dim word_version() As String = regKey.GetValue("").ToString().Split(".")
            Dim word_sVersion As String = Nothing
            word_sVersion = word_version(2)

            Select Case word_sVersion
                Case "7"
                    word_sVersion = "95"
                Case "8"
                    word_sVersion = "97"
                Case "9"
                    word_sVersion = "2000"
                Case "10"
                    word_sVersion = "2002"
                Case "11"
                    word_sVersion = "2003"
                Case "12"
                    word_sVersion = "2007"
                Case "14"
                    word_sVersion = "2010"
                Case "15"
                    word_sVersion = "2012"
                Case Else
                    word_sVersion = "0"
            End Select


            Return word_sVersion
            Exit Function
        Catch ex As Exception
            Return "0"
            Err.Clear()
            Exit Function
        End Try
    End Function


    Function get_adobe_Version() As String
        Try
            Dim regkey, subkey As Microsoft.Win32.RegistryKey
            Dim value As String
            Dim regpath As String = "Software\Microsoft\Windows\CurrentVersion\Uninstall"
            regkey = My.Computer.Registry.LocalMachine.OpenSubKey(regpath)
            Dim subkeys() As String = regkey.GetSubKeyNames
            Dim includes As Boolean
            For Each subk As String In subkeys
                subkey = regkey.OpenSubKey(subk)
                value = subkey.GetValue("DisplayName", "")
                If value <> "" Then
                    includes = True
                    If value.IndexOf("Hotfix") <> -1 Then includes = False
                    If value.IndexOf("Security Update") <> -1 Then includes = False
                    If value.IndexOf("Update for") <> -1 Then includes = False
                    If includes = True Then
                        If value.Trim = "Adobe Acrobat 8 Standard" Or value.Trim = "Adobe Acrobat  8 Standard" Then
                            Return "Y"
                            Exit Function
                        End If
                    End If

                End If
            Next
            Return "N"
            Exit Function
        Catch ex As Exception
            Return "N"
            Err.Clear()
            Exit Function
        End Try
    End Function

    Function get_adobe_flash_Version() As String
        Try
            Dim regkey, subkey As Microsoft.Win32.RegistryKey
            Dim value As String
            Dim regpath As String = "Software\Microsoft\Windows\CurrentVersion\Uninstall"
            regkey = My.Computer.Registry.LocalMachine.OpenSubKey(regpath)
            Dim subkeys() As String = regkey.GetSubKeyNames
            Dim includes As Boolean
            For Each subk As String In subkeys
                subkey = regkey.OpenSubKey(subk)
                value = subkey.GetValue("DisplayName", "")
                If value <> "" Then
                    includes = True
                    If value.IndexOf("Hotfix") <> -1 Then includes = False
                    If value.IndexOf("Security Update") <> -1 Then includes = False
                    If value.IndexOf("Update for") <> -1 Then includes = False
                    If includes = True Then
                        If value.Trim.Contains("Adobe Flash") Then
                            Return "Y"
                            Exit Function
                        End If
                    End If

                End If
            Next
            Return "N"
            Exit Function
        Catch ex As Exception
            Return "N"
            Err.Clear()
            Exit Function
        End Try
    End Function


    Function get_adobe_reader_Version() As String
        Try
            Dim regkey, subkey As Microsoft.Win32.RegistryKey
            Dim value As String
            Dim regpath As String = "Software\Microsoft\Windows\CurrentVersion\Uninstall"
            regkey = My.Computer.Registry.LocalMachine.OpenSubKey(regpath)
            Dim subkeys() As String = regkey.GetSubKeyNames
            Dim includes As Boolean
            For Each subk As String In subkeys
                subkey = regkey.OpenSubKey(subk)
                value = subkey.GetValue("DisplayName", "")
                If value <> "" Then
                    includes = True
                    If value.IndexOf("Hotfix") <> -1 Then includes = False
                    If value.IndexOf("Security Update") <> -1 Then includes = False
                    If value.IndexOf("Update for") <> -1 Then includes = False
                    If includes = True Then
                        If value.Trim.Contains("Adobe Reader") Then
                            Return "Y"
                            Exit Function
                        End If
                    End If

                End If
            Next
            Return "N"
            Exit Function
        Catch ex As Exception
            Return "N"
            Err.Clear()
            Exit Function
        End Try
    End Function

End Module
