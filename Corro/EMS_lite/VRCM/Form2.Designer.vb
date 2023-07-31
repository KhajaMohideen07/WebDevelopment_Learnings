<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form2
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.typingArea = New System.Windows.Forms.TextBox()
        Me.logArea = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'typingArea
        '
        Me.typingArea.Location = New System.Drawing.Point(12, 12)
        Me.typingArea.Multiline = True
        Me.typingArea.Name = "typingArea"
        Me.typingArea.Size = New System.Drawing.Size(815, 295)
        Me.typingArea.TabIndex = 0
        '
        'logArea
        '
        Me.logArea.Location = New System.Drawing.Point(12, 313)
        Me.logArea.Multiline = True
        Me.logArea.Name = "logArea"
        Me.logArea.Size = New System.Drawing.Size(815, 53)
        Me.logArea.TabIndex = 1
        '
        'Form2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(866, 378)
        Me.Controls.Add(Me.logArea)
        Me.Controls.Add(Me.typingArea)
        Me.Name = "Form2"
        Me.Text = "Form2"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents typingArea As System.Windows.Forms.TextBox
    Friend WithEvents logArea As System.Windows.Forms.TextBox
End Class
