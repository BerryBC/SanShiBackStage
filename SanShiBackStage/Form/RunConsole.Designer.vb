﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class RunConsole
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。  
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.lstConsole = New System.Windows.Forms.ListBox()
        Me.btnGo = New System.Windows.Forms.Button()
        Me.timerShowRuning = New System.Windows.Forms.Timer(Me.components)
        Me.timerGoSave = New System.Windows.Forms.Timer(Me.components)
        Me.wbGetNAMS = New System.Windows.Forms.WebBrowser()
        Me.btnTest = New System.Windows.Forms.Button()
        Me.timerRefreshNAMS = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'lstConsole
        '
        Me.lstConsole.FormattingEnabled = True
        Me.lstConsole.HorizontalScrollbar = True
        Me.lstConsole.ItemHeight = 12
        Me.lstConsole.Location = New System.Drawing.Point(12, 12)
        Me.lstConsole.Name = "lstConsole"
        Me.lstConsole.Size = New System.Drawing.Size(516, 256)
        Me.lstConsole.TabIndex = 0
        '
        'btnGo
        '
        Me.btnGo.Location = New System.Drawing.Point(12, 274)
        Me.btnGo.Name = "btnGo"
        Me.btnGo.Size = New System.Drawing.Size(516, 26)
        Me.btnGo.TabIndex = 1
        Me.btnGo.Text = "Go！！"
        Me.btnGo.UseVisualStyleBackColor = True
        '
        'timerShowRuning
        '
        Me.timerShowRuning.Interval = 1000
        '
        'timerGoSave
        '
        Me.timerGoSave.Interval = 1000
        '
        'wbGetNAMS
        '
        Me.wbGetNAMS.Location = New System.Drawing.Point(622, 295)
        Me.wbGetNAMS.MinimumSize = New System.Drawing.Size(20, 20)
        Me.wbGetNAMS.Name = "wbGetNAMS"
        Me.wbGetNAMS.Size = New System.Drawing.Size(250, 250)
        Me.wbGetNAMS.TabIndex = 2
        Me.wbGetNAMS.Visible = False
        '
        'btnTest
        '
        Me.btnTest.Location = New System.Drawing.Point(12, 358)
        Me.btnTest.Name = "btnTest"
        Me.btnTest.Size = New System.Drawing.Size(516, 26)
        Me.btnTest.TabIndex = 3
        Me.btnTest.Text = "Test！！"
        Me.btnTest.UseVisualStyleBackColor = True
        '
        'timerRefreshNAMS
        '
        Me.timerRefreshNAMS.Interval = 1000
        '
        'RunConsole
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(540, 306)
        Me.Controls.Add(Me.btnTest)
        Me.Controls.Add(Me.wbGetNAMS)
        Me.Controls.Add(Me.btnGo)
        Me.Controls.Add(Me.lstConsole)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "RunConsole"
        Me.Text = "RunConsole"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lstConsole As System.Windows.Forms.ListBox
    Friend WithEvents btnGo As System.Windows.Forms.Button
    Friend WithEvents timerShowRuning As System.Windows.Forms.Timer
    Friend WithEvents timerGoSave As System.Windows.Forms.Timer
    Friend WithEvents wbGetNAMS As System.Windows.Forms.WebBrowser
    Friend WithEvents btnTest As System.Windows.Forms.Button
    Friend WithEvents timerRefreshNAMS As System.Windows.Forms.Timer
End Class
