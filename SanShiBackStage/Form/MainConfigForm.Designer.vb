<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainConfigForm
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
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

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。  
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.txtHowManyDay = New System.Windows.Forms.TextBox()
        Me.txtWhatTimeToRun = New System.Windows.Forms.TextBox()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.lblHowManyDay = New System.Windows.Forms.Label()
        Me.lblWhatTimeToRun = New System.Windows.Forms.Label()
        Me.cbAutoRun = New System.Windows.Forms.CheckBox()
        Me.cbRunWhenStart = New System.Windows.Forms.CheckBox()
        Me.lblWhichWeekDayRun = New System.Windows.Forms.Label()
        Me.txtWhichWeedDayRun = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'txtHowManyDay
        '
        Me.txtHowManyDay.Location = New System.Drawing.Point(155, 12)
        Me.txtHowManyDay.Name = "txtHowManyDay"
        Me.txtHowManyDay.Size = New System.Drawing.Size(75, 21)
        Me.txtHowManyDay.TabIndex = 0
        '
        'txtWhatTimeToRun
        '
        Me.txtWhatTimeToRun.Location = New System.Drawing.Point(155, 66)
        Me.txtWhatTimeToRun.Name = "txtWhatTimeToRun"
        Me.txtWhatTimeToRun.Size = New System.Drawing.Size(75, 21)
        Me.txtWhatTimeToRun.TabIndex = 1
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(236, 12)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(118, 93)
        Me.btnSave.TabIndex = 2
        Me.btnSave.Text = "保存"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'lblHowManyDay
        '
        Me.lblHowManyDay.AutoSize = True
        Me.lblHowManyDay.Location = New System.Drawing.Point(12, 15)
        Me.lblHowManyDay.Name = "lblHowManyDay"
        Me.lblHowManyDay.Size = New System.Drawing.Size(137, 12)
        Me.lblHowManyDay.TabIndex = 3
        Me.lblHowManyDay.Text = "处理最近多少天的数据 :"
        '
        'lblWhatTimeToRun
        '
        Me.lblWhatTimeToRun.AutoSize = True
        Me.lblWhatTimeToRun.Location = New System.Drawing.Point(12, 69)
        Me.lblWhatTimeToRun.Name = "lblWhatTimeToRun"
        Me.lblWhatTimeToRun.Size = New System.Drawing.Size(89, 12)
        Me.lblWhatTimeToRun.TabIndex = 4
        Me.lblWhatTimeToRun.Text = "几点自动运行 :"
        '
        'cbAutoRun
        '
        Me.cbAutoRun.AutoSize = True
        Me.cbAutoRun.Location = New System.Drawing.Point(14, 95)
        Me.cbAutoRun.Name = "cbAutoRun"
        Me.cbAutoRun.Size = New System.Drawing.Size(96, 16)
        Me.cbAutoRun.TabIndex = 5
        Me.cbAutoRun.Text = "是否自动运行"
        Me.cbAutoRun.UseVisualStyleBackColor = True
        '
        'cbRunWhenStart
        '
        Me.cbRunWhenStart.AutoSize = True
        Me.cbRunWhenStart.Location = New System.Drawing.Point(116, 95)
        Me.cbRunWhenStart.Name = "cbRunWhenStart"
        Me.cbRunWhenStart.Size = New System.Drawing.Size(96, 16)
        Me.cbRunWhenStart.TabIndex = 6
        Me.cbRunWhenStart.Text = "是否开机启动"
        Me.cbRunWhenStart.UseVisualStyleBackColor = True
        '
        'lblWhichWeekDayRun
        '
        Me.lblWhichWeekDayRun.AutoSize = True
        Me.lblWhichWeekDayRun.Location = New System.Drawing.Point(12, 41)
        Me.lblWhichWeekDayRun.Name = "lblWhichWeekDayRun"
        Me.lblWhichWeekDayRun.Size = New System.Drawing.Size(137, 12)
        Me.lblWhichWeekDayRun.TabIndex = 8
        Me.lblWhichWeekDayRun.Text = "周几运行基础数据更新 :"
        '
        'txtWhichWeedDayRun
        '
        Me.txtWhichWeedDayRun.Location = New System.Drawing.Point(155, 38)
        Me.txtWhichWeedDayRun.Name = "txtWhichWeedDayRun"
        Me.txtWhichWeedDayRun.Size = New System.Drawing.Size(75, 21)
        Me.txtWhichWeedDayRun.TabIndex = 7
        '
        'MainConfigForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(366, 117)
        Me.Controls.Add(Me.lblWhichWeekDayRun)
        Me.Controls.Add(Me.txtWhichWeedDayRun)
        Me.Controls.Add(Me.cbRunWhenStart)
        Me.Controls.Add(Me.cbAutoRun)
        Me.Controls.Add(Me.lblWhatTimeToRun)
        Me.Controls.Add(Me.lblHowManyDay)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.txtWhatTimeToRun)
        Me.Controls.Add(Me.txtHowManyDay)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "MainConfigForm"
        Me.Text = "Main Config Form"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtHowManyDay As System.Windows.Forms.TextBox
    Friend WithEvents txtWhatTimeToRun As System.Windows.Forms.TextBox
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents lblHowManyDay As System.Windows.Forms.Label
    Friend WithEvents lblWhatTimeToRun As System.Windows.Forms.Label
    Friend WithEvents cbAutoRun As System.Windows.Forms.CheckBox
    Friend WithEvents cbRunWhenStart As System.Windows.Forms.CheckBox
    Friend WithEvents lblWhichWeekDayRun As System.Windows.Forms.Label
    Friend WithEvents txtWhichWeedDayRun As System.Windows.Forms.TextBox
End Class
