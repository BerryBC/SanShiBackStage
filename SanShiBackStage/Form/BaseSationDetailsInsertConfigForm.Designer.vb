<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BaseSationDetailsInsertConfigForm
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
        Me.lstConfig = New System.Windows.Forms.ListBox()
        Me.plConfig = New System.Windows.Forms.Panel()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.lblName = New System.Windows.Forms.Label()
        Me.gbSource = New System.Windows.Forms.GroupBox()
        Me.txtManyFile = New System.Windows.Forms.TextBox()
        Me.lblManyFile = New System.Windows.Forms.Label()
        Me.txtIFExcelThenSheetName = New System.Windows.Forms.TextBox()
        Me.lblIFExcelThenSheetName = New System.Windows.Forms.Label()
        Me.txtSuffix = New System.Windows.Forms.TextBox()
        Me.lblFileSuffix = New System.Windows.Forms.Label()
        Me.txtSourceFileName = New System.Windows.Forms.TextBox()
        Me.lblSourceFileName = New System.Windows.Forms.Label()
        Me.txtUpdatePath = New System.Windows.Forms.TextBox()
        Me.lblUpdatePath = New System.Windows.Forms.Label()
        Me.txtTableName = New System.Windows.Forms.TextBox()
        Me.lblTableName = New System.Windows.Forms.Label()
        Me.btnDetele = New System.Windows.Forms.Button()
        Me.btnADD = New System.Windows.Forms.Button()
        Me.plConfig.SuspendLayout()
        Me.gbSource.SuspendLayout()
        Me.SuspendLayout()
        '
        'lstConfig
        '
        Me.lstConfig.FormattingEnabled = True
        Me.lstConfig.ItemHeight = 12
        Me.lstConfig.Location = New System.Drawing.Point(12, 12)
        Me.lstConfig.Name = "lstConfig"
        Me.lstConfig.Size = New System.Drawing.Size(200, 220)
        Me.lstConfig.TabIndex = 0
        '
        'plConfig
        '
        Me.plConfig.Controls.Add(Me.btnSave)
        Me.plConfig.Controls.Add(Me.txtName)
        Me.plConfig.Controls.Add(Me.lblName)
        Me.plConfig.Controls.Add(Me.gbSource)
        Me.plConfig.Location = New System.Drawing.Point(225, 12)
        Me.plConfig.Name = "plConfig"
        Me.plConfig.Size = New System.Drawing.Size(401, 253)
        Me.plConfig.TabIndex = 1
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(215, 5)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(184, 23)
        Me.btnSave.TabIndex = 4
        Me.btnSave.Text = "保存该配置"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'txtName
        '
        Me.txtName.Location = New System.Drawing.Point(46, 7)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(163, 21)
        Me.txtName.TabIndex = 2
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.Location = New System.Drawing.Point(5, 10)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(35, 12)
        Me.lblName.TabIndex = 1
        Me.lblName.Text = "Name:"
        '
        'gbSource
        '
        Me.gbSource.Controls.Add(Me.txtManyFile)
        Me.gbSource.Controls.Add(Me.lblManyFile)
        Me.gbSource.Controls.Add(Me.txtIFExcelThenSheetName)
        Me.gbSource.Controls.Add(Me.lblIFExcelThenSheetName)
        Me.gbSource.Controls.Add(Me.txtSuffix)
        Me.gbSource.Controls.Add(Me.lblFileSuffix)
        Me.gbSource.Controls.Add(Me.txtSourceFileName)
        Me.gbSource.Controls.Add(Me.lblSourceFileName)
        Me.gbSource.Controls.Add(Me.txtUpdatePath)
        Me.gbSource.Controls.Add(Me.lblUpdatePath)
        Me.gbSource.Controls.Add(Me.txtTableName)
        Me.gbSource.Controls.Add(Me.lblTableName)
        Me.gbSource.Location = New System.Drawing.Point(0, 34)
        Me.gbSource.Name = "gbSource"
        Me.gbSource.Size = New System.Drawing.Size(399, 216)
        Me.gbSource.TabIndex = 0
        Me.gbSource.TabStop = False
        Me.gbSource.Text = "编辑"
        '
        'txtManyFile
        '
        Me.txtManyFile.Location = New System.Drawing.Point(105, 170)
        Me.txtManyFile.Name = "txtManyFile"
        Me.txtManyFile.Size = New System.Drawing.Size(288, 21)
        Me.txtManyFile.TabIndex = 15
        '
        'lblManyFile
        '
        Me.lblManyFile.AutoSize = True
        Me.lblManyFile.Location = New System.Drawing.Point(9, 174)
        Me.lblManyFile.Name = "lblManyFile"
        Me.lblManyFile.Size = New System.Drawing.Size(83, 12)
        Me.lblManyFile.TabIndex = 14
        Me.lblManyFile.Text = "是否多个文件:"
        '
        'txtIFExcelThenSheetName
        '
        Me.txtIFExcelThenSheetName.Location = New System.Drawing.Point(105, 143)
        Me.txtIFExcelThenSheetName.Name = "txtIFExcelThenSheetName"
        Me.txtIFExcelThenSheetName.Size = New System.Drawing.Size(288, 21)
        Me.txtIFExcelThenSheetName.TabIndex = 13
        '
        'lblIFExcelThenSheetName
        '
        Me.lblIFExcelThenSheetName.AutoSize = True
        Me.lblIFExcelThenSheetName.Location = New System.Drawing.Point(9, 147)
        Me.lblIFExcelThenSheetName.Name = "lblIFExcelThenSheetName"
        Me.lblIFExcelThenSheetName.Size = New System.Drawing.Size(83, 12)
        Me.lblIFExcelThenSheetName.TabIndex = 12
        Me.lblIFExcelThenSheetName.Text = "Excel中Sheet:"
        '
        'txtSuffix
        '
        Me.txtSuffix.Location = New System.Drawing.Point(105, 116)
        Me.txtSuffix.Name = "txtSuffix"
        Me.txtSuffix.Size = New System.Drawing.Size(288, 21)
        Me.txtSuffix.TabIndex = 11
        '
        'lblFileSuffix
        '
        Me.lblFileSuffix.AutoSize = True
        Me.lblFileSuffix.Location = New System.Drawing.Point(9, 120)
        Me.lblFileSuffix.Name = "lblFileSuffix"
        Me.lblFileSuffix.Size = New System.Drawing.Size(71, 12)
        Me.lblFileSuffix.TabIndex = 10
        Me.lblFileSuffix.Text = "文件后缀名:"
        '
        'txtSourceFileName
        '
        Me.txtSourceFileName.Location = New System.Drawing.Point(105, 89)
        Me.txtSourceFileName.Name = "txtSourceFileName"
        Me.txtSourceFileName.Size = New System.Drawing.Size(288, 21)
        Me.txtSourceFileName.TabIndex = 9
        '
        'lblSourceFileName
        '
        Me.lblSourceFileName.AutoSize = True
        Me.lblSourceFileName.Location = New System.Drawing.Point(9, 93)
        Me.lblSourceFileName.Name = "lblSourceFileName"
        Me.lblSourceFileName.Size = New System.Drawing.Size(83, 12)
        Me.lblSourceFileName.TabIndex = 8
        Me.lblSourceFileName.Text = "更新源文件名:"
        '
        'txtUpdatePath
        '
        Me.txtUpdatePath.Location = New System.Drawing.Point(105, 62)
        Me.txtUpdatePath.Name = "txtUpdatePath"
        Me.txtUpdatePath.Size = New System.Drawing.Size(288, 21)
        Me.txtUpdatePath.TabIndex = 7
        '
        'lblUpdatePath
        '
        Me.lblUpdatePath.AutoSize = True
        Me.lblUpdatePath.Location = New System.Drawing.Point(9, 66)
        Me.lblUpdatePath.Name = "lblUpdatePath"
        Me.lblUpdatePath.Size = New System.Drawing.Size(59, 12)
        Me.lblUpdatePath.TabIndex = 6
        Me.lblUpdatePath.Text = "更新路径:"
        '
        'txtTableName
        '
        Me.txtTableName.Location = New System.Drawing.Point(105, 35)
        Me.txtTableName.Name = "txtTableName"
        Me.txtTableName.Size = New System.Drawing.Size(288, 21)
        Me.txtTableName.TabIndex = 5
        '
        'lblTableName
        '
        Me.lblTableName.AutoSize = True
        Me.lblTableName.Location = New System.Drawing.Point(9, 39)
        Me.lblTableName.Name = "lblTableName"
        Me.lblTableName.Size = New System.Drawing.Size(35, 12)
        Me.lblTableName.TabIndex = 4
        Me.lblTableName.Text = "表名:"
        '
        'btnDetele
        '
        Me.btnDetele.Location = New System.Drawing.Point(12, 237)
        Me.btnDetele.Name = "btnDetele"
        Me.btnDetele.Size = New System.Drawing.Size(98, 28)
        Me.btnDetele.TabIndex = 2
        Me.btnDetele.Text = "Detele爱人"
        Me.btnDetele.UseVisualStyleBackColor = True
        '
        'btnADD
        '
        Me.btnADD.Location = New System.Drawing.Point(114, 237)
        Me.btnADD.Name = "btnADD"
        Me.btnADD.Size = New System.Drawing.Size(98, 28)
        Me.btnADD.TabIndex = 3
        Me.btnADD.Text = "ADD"
        Me.btnADD.UseVisualStyleBackColor = True
        '
        'BaseSationDetailsInsertConfigForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(635, 277)
        Me.Controls.Add(Me.btnADD)
        Me.Controls.Add(Me.btnDetele)
        Me.Controls.Add(Me.plConfig)
        Me.Controls.Add(Me.lstConfig)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "BaseSationDetailsInsertConfigForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Insert DataBase Config"
        Me.plConfig.ResumeLayout(False)
        Me.plConfig.PerformLayout()
        Me.gbSource.ResumeLayout(False)
        Me.gbSource.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lstConfig As System.Windows.Forms.ListBox
    Friend WithEvents plConfig As System.Windows.Forms.Panel
    Friend WithEvents gbSource As System.Windows.Forms.GroupBox
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents lblName As System.Windows.Forms.Label
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents txtTableName As System.Windows.Forms.TextBox
    Friend WithEvents lblTableName As System.Windows.Forms.Label
    Friend WithEvents txtUpdatePath As System.Windows.Forms.TextBox
    Friend WithEvents lblUpdatePath As System.Windows.Forms.Label
    Friend WithEvents txtSourceFileName As System.Windows.Forms.TextBox
    Friend WithEvents lblSourceFileName As System.Windows.Forms.Label
    Friend WithEvents btnDetele As System.Windows.Forms.Button
    Friend WithEvents btnADD As System.Windows.Forms.Button
    Friend WithEvents txtSuffix As System.Windows.Forms.TextBox
    Friend WithEvents lblFileSuffix As System.Windows.Forms.Label
    Friend WithEvents txtManyFile As System.Windows.Forms.TextBox
    Friend WithEvents lblManyFile As System.Windows.Forms.Label
    Friend WithEvents txtIFExcelThenSheetName As System.Windows.Forms.TextBox
    Friend WithEvents lblIFExcelThenSheetName As System.Windows.Forms.Label
End Class
