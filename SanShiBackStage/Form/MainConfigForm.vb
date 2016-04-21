Imports System.Windows.Forms
Imports Microsoft.Win32



Public Class MainConfigForm
    Private mcMainConfig As MainConfig

    Public Sub New(mcTmpConfig As MainConfig)

        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。

        mcMainConfig = mcTmpConfig
        txtHowManyDay.Text = mcMainConfig.intHowManyDaysAgo.ToString
        txtWhatTimeToRun.Text = mcMainConfig.intWhatTimeToRun.ToString
        txtWhichWeedDayRun.Text = mcMainConfig.intWhichWeedDayToRun.ToString
        cbAutoRun.Checked = mcMainConfig.bolIsAutoRun
        cbRunWhenStart.Checked = mcMainConfig.bolRunWhenStart



    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        Try


            If txtHowManyDay.Text <> "" And txtWhatTimeToRun.Text <> "" And txtWhichWeedDayRun.Text <> "" Then
                Dim RegistyKeyItem As RegistryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run", True)



                mcMainConfig.intHowManyDaysAgo = CType(txtHowManyDay.Text, Integer)
                mcMainConfig.intWhatTimeToRun = CType(txtWhatTimeToRun.Text, Integer)
                mcMainConfig.intWhichWeedDayToRun = CType(txtWhichWeedDayRun.Text, Integer)
                mcMainConfig.bolIsAutoRun = cbAutoRun.Checked
                mcMainConfig.bolRunWhenStart = cbRunWhenStart.Checked

                If mcMainConfig.bolRunWhenStart Then
                    RegistyKeyItem.SetValue("AutoInsertToDataBase", Application.ExecutablePath.ToString)
                    RegistyKeyItem.Close()
                Else
                    If RegistyKeyItem.GetValue("AutoInsertToDataBase") <> Nothing Then
                        RegistyKeyItem.DeleteValue("AutoInsertToDataBase")
                        RegistyKeyItem.Close()
                    End If

                End If

                MainConfig.Save(mcMainConfig)
                Me.Close()
            Else
                MessageBox.Show("不能为空!!")
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub


    Private Sub txtWhatTimeToRun_TextChanged(sender As Object, e As EventArgs) Handles txtWhatTimeToRun.TextChanged
        Try

            If txtWhatTimeToRun.Text <> "" Then
                If Not IsNumeric(txtWhatTimeToRun.Text) Then
                    MessageBox.Show("只能是数字啊！！")
                    txtWhatTimeToRun.Text = ""
                Else
                    If CType(txtWhatTimeToRun.Text, Integer) > 23 Or CType(txtWhatTimeToRun.Text, Integer) < 0 Then
                        MessageBox.Show("不能大于一天而且必须大于0")
                        txtWhatTimeToRun.Text = ""
                    End If
                End If
            End If
        Catch ex As Exception
        MessageBox.Show(ex.ToString)
        End Try

    End Sub

    Private Sub txtWhichWeedDayRun_TextChanged(sender As Object, e As EventArgs) Handles txtWhichWeedDayRun.TextChanged
        Try

            If txtWhichWeedDayRun.Text <> "" Then
                If Not IsNumeric(txtWhichWeedDayRun.Text) Then
                    MessageBox.Show("只能是数字啊！！")
                    txtWhichWeedDayRun.Text = ""
                Else
                    If CType(txtWhichWeedDayRun.Text, Integer) > 7 Or CType(txtWhichWeedDayRun.Text, Integer) < 1 Then
                        MessageBox.Show("取数只能为1-7")
                        txtWhichWeedDayRun.Text = ""
                    End If
                End If
            End If
        Catch ex As Exception
        MessageBox.Show(ex.ToString)
        End Try

    End Sub

    Private Sub txtHowManyDay_TextChanged(sender As Object, e As EventArgs) Handles txtHowManyDay.TextChanged
        Try
            If txtHowManyDay.Text <> "" Then
                If Not IsNumeric(txtHowManyDay.Text) Then
                    MessageBox.Show("只能是数字啊！！")
                    txtHowManyDay.Text = ""
                Else
                    If CType(txtHowManyDay.Text, Integer) > 365 Or CType(txtHowManyDay.Text, Integer) <= 0 Then
                        MessageBox.Show("不能大于一年而且必须大于0")
                        txtHowManyDay.Text = ""
                    End If
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub
End Class