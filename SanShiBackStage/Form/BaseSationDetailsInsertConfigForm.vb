Imports System.Text.RegularExpressions
Imports System.Windows.Forms
Public Class BaseSationDetailsInsertConfigForm

    Dim bsdlCommonLibraryInForm As BaseSationDetailsLibrary = New BaseSationDetailsLibrary

    Dim dtBaseSationDetailsManaInForm As DataTable
    ''' <summary>
    ''' List控件选项
    ''' </summary>
    Dim intOldSelect As Integer = -1

    Public Sub New()

        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        RefreshTheListBox()

    End Sub

    Private Sub btnADD_Click(sender As Object, e As EventArgs) Handles btnADD.Click
        '创建新入数过程类,并在列表配置类以及窗体右边详细配置中给出默认入数过程配置并保存

        Try


            bsdlCommonLibraryInForm.AddConfig()
            intOldSelect = lstConfig.SelectedIndex

            RefreshTheListBox()
            lstConfig.SelectedIndex = dtBaseSationDetailsManaInForm.Rows.Count - 1
            GetSourceConfig()
            btnSave.Enabled = True
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub


    ''' <summary>
    ''' 刷新入数过程列表
    ''' </summary>
    Private Sub RefreshTheListBox()
        Dim drTmp As DataRow

        Try



            dtBaseSationDetailsManaInForm = bsdlCommonLibraryInForm.ReturnBaseSationDetailsMan()
            lstConfig.Items.Clear()
            If dtBaseSationDetailsManaInForm.Rows.Count > 0 Then
                For Each drTmp In dtBaseSationDetailsManaInForm.Rows
                    Dim strEnable As String
                    strEnable = drTmp.Item("DataTableID") & " - " & drTmp.Item("ConfigName")
                    lstConfig.Items.Add(strEnable)
                Next
                lstConfig.SelectedIndex = intOldSelect
            Else
                btnSave.Enabled = False
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message, ex)
        End Try

    End Sub

    Private Sub btnDetele_Click(sender As Object, e As EventArgs) Handles btnDetele.Click


        Try


            intOldSelect = lstConfig.SelectedIndex
            If (intOldSelect >= 0) And (intOldSelect < (dtBaseSationDetailsManaInForm.Rows.Count)) Then
                bsdlCommonLibraryInForm.DeleteConfig(dtBaseSationDetailsManaInForm.Rows(intOldSelect).Item("DataTableID").ToString)

            End If

            If (intOldSelect >= (dtBaseSationDetailsManaInForm.Rows.Count - 1)) Then
                intOldSelect = (dtBaseSationDetailsManaInForm.Rows.Count) - 2
            End If
            RefreshTheListBox()
            '刷新List列表
            '读取详细配置至右边
            GetSourceConfig()
            ''把当前配置类存至硬盘JSON文件
            'InsertDataBaseConfigData.Save(cdLoadConfig)
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub



    ''' <summary>
    ''' 读取配置类中的入数过程
    ''' </summary>
    Private Sub GetSourceConfig()

        Try


            If (intOldSelect >= 0) And (intOldSelect < dtBaseSationDetailsManaInForm.Rows.Count) Then
                txtName.Text = dtBaseSationDetailsManaInForm.Rows(lstConfig.SelectedIndex).Item("ConfigName").ToString

                txtTableName.Text = dtBaseSationDetailsManaInForm.Rows(intOldSelect).Item("DataTableName").ToString
                txtUpdatePath.Text = dtBaseSationDetailsManaInForm.Rows(intOldSelect).Item("UpDatePath").ToString
                txtSourceFileName.Text = dtBaseSationDetailsManaInForm.Rows(intOldSelect).Item("UpDateSource").ToString
                txtSuffix.Text = dtBaseSationDetailsManaInForm.Rows(intOldSelect).Item("FileSuffix").ToString
                txtIFExcelThenSheetName.Text = dtBaseSationDetailsManaInForm.Rows(intOldSelect).Item("IFExcelThenSheetName").ToString
                txtManyFile.Text = dtBaseSationDetailsManaInForm.Rows(intOldSelect).Item("MultiFile").ToString
            Else
                txtTableName.Text = ""
                txtUpdatePath.Text = ""
                txtSourceFileName.Text = ""
                txtSuffix.Text = ""
                txtIFExcelThenSheetName.Text = ""
                txtManyFile.Text = ""
                btnSave.Enabled = False
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message, ex)
        End Try


    End Sub

    Private Sub lstConfig_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstConfig.SelectedIndexChanged
        Try

            intOldSelect = lstConfig.SelectedIndex
            GetSourceConfig()
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        Dim intMulti As Integer
        Dim intResult As Integer

        Try


            If Regex.IsMatch(txtManyFile.Text, "^[1-9]\d*$") Then
                intMulti = Convert.ToInt32(txtManyFile.Text)
            Else
                intMulti = 0
            End If
            txtManyFile.Text = intMulti.ToString
            intResult = bsdlCommonLibraryInForm.ModifyConfig(Convert.ToInt32(dtBaseSationDetailsManaInForm.Rows(intOldSelect).Item("DataTableID")), txtName.Text, txtTableName.Text, txtUpdatePath.Text, txtSuffix.Text, txtIFExcelThenSheetName.Text, intMulti, txtSourceFileName.Text)
            RefreshTheListBox()


        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub
End Class