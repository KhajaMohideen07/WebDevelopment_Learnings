Imports System.Threading.Thread

Public Class Count1
    Public Current_Time As Date
    Sub Count()
        Dim i As Integer = 0
        Do Until i = 1
            Sleep(1000)
            Current_Time = Current_Time.AddSeconds(1)
        Loop
    End Sub

End Class