Imports System.Runtime.InteropServices
Public Class IdleTime
    Private Declare Function GetLastInputInfo Lib "User32.dll" (ByRef lastInput As LASTINPUTINFO) As Boolean

    Public Structure LASTINPUTINFO
        Public cbsize As Int32
        Public dwtime As Int32
    End Structure

    Public ReadOnly Property IdleTime() As Integer
        Get
            Dim lastInput As New LASTINPUTINFO

            lastInput.cbsize = Marshal.Sizeof(lastInput)
            If GetLastInputInfo(lastInput) Then
                Return (Environment.TickCount - lastInput.dwtime) / 1000
            End If
            Return 0
        End Get
    End Property

End Class