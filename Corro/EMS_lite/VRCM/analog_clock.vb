Imports System.Drawing.Drawing2D
Public Class analog_clock
    Const Convert As Double = Math.PI / 180

    Const SecRadius As Double = 185
    Const MinRadius As Double = 180
    Const HrRadius As Double = 155

    Dim SecAngle As Double
    Dim MinAngle As Double
    Dim HrAngle As Double

    Dim SecX As Single = 220
    Dim SecY As Single = 20
    Dim MinX As Single = 220
    Dim MinY As Single = 20
    Dim HrX As Single = 220
    Dim HrY As Single = 20

    Dim hrs, min, value As Integer
    Dim TimeString As String

    Dim WithEvents tmrClock As New Timer

    Dim WithEvents lblPanel As New Label
    Dim lblTB As New Label

    Dim StartPoint(60) As PointF
    Dim EndPoint(60) As PointF
    Dim NumberPoint() As PointF = {New PointF(285, 50),
       New PointF(350, 115), New PointF(376, 203),
       New PointF(350, 290), New PointF(285, 350),
       New PointF(205, 366), New PointF(125, 350),
       New PointF(60, 290), New PointF(38, 203),
       New PointF(55, 120), New PointF(112, 59),
       New PointF(196, 36)}
    'Create the Pens
    Dim GreenPen As Pen = New Pen(Color.Green, 4)
    Dim BluePen As Pen = New Pen(Color.Blue, 4)
    Dim OrangePen As Pen = New Pen(Color.DarkOrange, 5)
    Dim BlackPen As Pen = New Pen(Color.Black, 6)
    Dim myPen As New Pen(Color.DarkBlue, 8)
    'Create the Fonts
    Dim NumberFont As New Font("Arial", 25, FontStyle.Bold)
    Dim ClockFont As New Font("Arial", 18, FontStyle.Bold)

    'Create the Bitmap to draw the clock face
    Dim ClockFace As New Bitmap(445, 445)
    Dim gr As Graphics = Graphics.FromImage(ClockFace)


    Private Sub Form1_Load(ByVal sender As System.Object, _
       ByVal e As System.EventArgs) Handles MyBase.Load

        BluePen.SetLineCap(LineCap.Round, LineCap.ArrowAnchor, _
           DashCap.Flat)
        OrangePen.SetLineCap(LineCap.Round, LineCap.ArrowAnchor, _
           DashCap.Flat)
        BlackPen.SetLineCap(LineCap.Round, LineCap.ArrowAnchor, _
           DashCap.Flat)
        DoubleBuffered = True
        Me.Size = New Size(570, 470)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.TransparencyKey = SystemColors.Control
        Me.CenterToScreen()
        CalculatePerimeter()
        DrawFace()
        tmrClock.Interval = 990
        tmrClock.Start()
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As  _
          System.Windows.Forms.PaintEventArgs)
        e.Graphics.SmoothingMode = SmoothingMode.HighQuality
        'Draw Clock Background
        e.Graphics.DrawImage(ClockFace, Point.Empty)
        'Draw Digital Time
        e.Graphics.DrawString(TimeString, ClockFont, _
           Brushes.White, 170, 260)
        'Draw Hands
        e.Graphics.DrawLine(BlackPen, 220, 220, HrX, HrY)
        e.Graphics.FillEllipse(Brushes.Black, 210, 210, 20, 20)
        e.Graphics.DrawLine(OrangePen, 220, 220, MinX, MinY)
        e.Graphics.FillEllipse(Brushes.DarkOrange, 212, 212, 16, 16)
        e.Graphics.DrawLine(BluePen, 220, 220, SecX, SecY)
        e.Graphics.FillEllipse(Brushes.Blue, 215, 215, 10, 10)
    End Sub

    Sub DrawFace()
        gr.SmoothingMode = SmoothingMode.HighQuality
        'Draw Clock Background
        gr.FillEllipse(Brushes.Beige, 20, 20, 400, 400)
        gr.DrawEllipse(GreenPen, 20, 20, 400, 400)
        gr.DrawEllipse(Pens.Red, 120, 120, 200, 200)
        'Draw Increments around cicumferance
        For I As Integer = 1 To 60
            gr.DrawLine(GreenPen, StartPoint(I), _
               EndPoint(I))
        Next
        'Draw Numbers
        For I As Integer = 1 To 12
            gr.DrawString(I.ToString, NumberFont, _
               Brushes.Black, NumberPoint(I - 1))
        Next

        'Draw Digital Clock Background
        gr.FillRectangle(Brushes.DarkBlue, _
           170, 260, 100, 30)
        myPen.LineJoin = LineJoin.Round
        gr.DrawRectangle(myPen, 170, 260, 100, 30)
    End Sub

    Sub CalculatePerimeter()
        Dim X, Y As Integer
        Dim radius As Integer
        For I As Integer = 1 To 60
            If I Mod 5 = 0 Then
                radius = 182
            Else
                radius = 190
            End If
            'Calculate Start Point
            X = CInt(radius * Math.Cos((90 - I * 6) * _
               Convert)) + 220
            Y = 220 - CInt(radius * Math.Sin((90 - I * 6) * _
               Convert))
            StartPoint(I) = New PointF(X, Y)
            'Calculate End Point
            X = CInt(200 * Math.Cos((90 - I * 6) * _
               Convert)) + 220
            Y = 220 - CInt(200 * Math.Sin((90 - I * 6) * _
               Convert))
            EndPoint(I) = New PointF(X, Y)
        Next
    End Sub

    Private Sub tmrClock_Tick(ByVal sender As System.Object, _
          ByVal e As System.EventArgs) Handles tmrClock.Tick
        TimeString = Now.ToString("HH:mm:ss")
        'Set The Angle of the Second, Minute and Hour hand
        'according to the time
        SecAngle = (Now.Second * 6)
        MinAngle = (Now.Minute + Now.Second / 60) * 6
        HrAngle = (Now.Hour + Now.Minute / 60) * 30
        'Get the X,Y co-ordinates of the end point of each hand
        SecX = CInt(SecRadius * Math.Cos((90 - SecAngle) * _
           Convert)) + 220
        SecY = 220 - CInt(SecRadius * Math.Sin((90 - SecAngle) * _
           Convert))
        MinX = CInt(MinRadius * Math.Cos((90 - MinAngle) * _
           Convert)) + 220
        MinY = 220 - CInt(MinRadius * Math.Sin((90 - MinAngle) * _
           Convert))
        HrX = CInt(HrRadius * Math.Cos((90 - HrAngle) * _
           Convert)) + 220
        HrY = 220 - CInt(HrRadius * Math.Sin((90 - HrAngle) * _
           Convert))
        Refresh()
    End Sub
End Class