Imports System.ComponentModel
Imports ExcelLibrary
Imports SQLServerLibrary
Imports System.Windows.Forms


Public Class RunConsole
    ''' <summary>
    ''' 后台工作机器人
    ''' </summary>
    Public WithEvents bwGetEnterWorker As BackgroundWorker


    ''' <summary>
    ''' 定义基站基础信息库
    ''' </summary>
    Dim bsdlCommonLibraryInRun As BaseSationDetailsLibrary = New BaseSationDetailsLibrary


    Dim bscpCommonLibraryInRun As BSCPara = New BSCPara
    Dim gsmcCommonLibraryInRun As GSMCellPara = New GSMCellPara
    Dim gsmiccInsertToDataBase As GSMIndexOfCellConfig = New GSMIndexOfCellConfig
    Dim gsmioclLibrary As GSMIndexOfCellLibrary = New GSMIndexOfCellLibrary
    Dim sqllSSLibrary As LoadSQLServer = New LoadSQLServer



    Dim dtInsertDataBaseConfigData As DataTable
    Dim bolBackState As Boolean
    Dim strOutput As String
    Dim bolIsBSConfig As Boolean
    '配置后必须引用由主程序给出的配置文件
    Public Sub New(bolTmpBackState As Boolean, boltmpIsBSConfig As Boolean)
        ' 此调用是设计器所必需的。
        InitializeComponent()

        '引用主程序给出的调度并初始化
        bolBackState = bolTmpBackState
        bolIsBSConfig = boltmpIsBSConfig
        dtInsertDataBaseConfigData = bsdlCommonLibraryInRun.ReturnBaseSationDetailsMan()
        '初始化后台工作机器人信息
        bwGetEnterWorker = New BackgroundWorker
        bwGetEnterWorker.WorkerReportsProgress = True
        bwGetEnterWorker.WorkerSupportsCancellation = True
        strOutput = ""
    End Sub


    Private Sub timerShowRuning_Tick(sender As Object, e As EventArgs) Handles timerShowRuning.Tick
        Try
            btnGo.Text += "."
            If btnGo.Text.Length > 40 Then
                btnGo.Text = btnGo.Text.Substring(0, 12)
            End If

            If strOutput <> "" Then
                bsdlCommonLibraryInRun.LogOnTextBoxAndDataBaseForBaseSation("  " & strOutput, "", lstConsole)
                strOutput = ""
            End If

        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try


    End Sub

    Public Sub btnGo_Click(sender As Object, e As EventArgs) Handles btnGo.Click
        Dim bsdipOneOfPara As BaseSationDetailsInsertParaClass
        Dim drEveryBaseSationDetailsConfig As DataRow
        Dim listbsdipBSInsertPara As List(Of BaseSationDetailsInsertParaClass)
        Dim dtCellParaDetailsMana As DataTable
        Dim strBSCParaUpDatePath As String
        Dim strBSCParaUpdateSource As String
        Dim intI As Integer
        Dim intJ As Integer
        Dim intK As Integer
        Dim strHeadOfSource As String
        Dim strtmpListDir As New List(Of String)
        Dim intWhereYear As Integer
        Dim intWhereMonth As Integer
        Dim intWhereDay As Integer
        Dim intWhereHour As Integer
        Dim intWhereMin As Integer
        Dim intWhereSec As Integer
        Dim strDir As New List(Of String)
        Dim strtmpFileName As String
        Dim dateWhatNow As Date
        Dim arrobjParaOfBSCPara As Object()
        Dim arrobjParaOfBGWorker As Object()
        Dim arrobjParaOfBGWorkerForIndexOfTraffic As Object()
        Dim strJsonLoad As String

        Try



            arrobjParaOfBSCPara = {}
            arrobjParaOfBGWorkerForIndexOfTraffic = {}
            listbsdipBSInsertPara = New List(Of BaseSationDetailsInsertParaClass)

            bsdlCommonLibraryInRun.LogOnTextBoxAndDataBaseForBaseSation("", ".", lstConsole)

            If bolIsBSConfig Then

                bsdipOneOfPara = New BaseSationDetailsInsertParaClass
                For Each drEveryBaseSationDetailsConfig In dtInsertDataBaseConfigData.Rows
                    bsdipOneOfPara = New BaseSationDetailsInsertParaClass
                    bsdipOneOfPara.strConfigName = drEveryBaseSationDetailsConfig.Item("ConfigName")
                    bsdipOneOfPara.strDataTableName = drEveryBaseSationDetailsConfig.Item("DataTableName")
                    bsdipOneOfPara.strUpDatePath = drEveryBaseSationDetailsConfig.Item("UpDatePath")
                    bsdipOneOfPara.strUpDateSource = drEveryBaseSationDetailsConfig.Item("UpDateSource")
                    bsdipOneOfPara.strFileSuffix = drEveryBaseSationDetailsConfig.Item("FileSuffix")
                    bsdipOneOfPara.strIFExcelThenSheetName = drEveryBaseSationDetailsConfig.Item("IFExcelThenSheetName")
                    bsdipOneOfPara.intMultiFile = drEveryBaseSationDetailsConfig.Item("MultiFile")
                    bsdipOneOfPara.strDataTableID = drEveryBaseSationDetailsConfig.Item("DataTableID")
                    listbsdipBSInsertPara.Add(bsdipOneOfPara)
                Next


                bsdlCommonLibraryInRun.LogOnTextBoxAndDataBaseForBaseSation("列队插入数据库--基站信息表的入数过程初始化完毕", "", lstConsole)
            Else
                listbsdipBSInsertPara = Nothing
                bsdlCommonLibraryInRun.LogOnTextBoxAndDataBaseForBaseSation("列队插入数据库--不入基站信息表", "", lstConsole)
            End If


            bsdlCommonLibraryInRun.LogOnTextBoxAndDataBaseForBaseSation("", ",", lstConsole)
            bsdlCommonLibraryInRun.LogOnTextBoxAndDataBaseForBaseSation("列队插入数据库--例行P文件入到数据库的入数过程初始化完毕", "", lstConsole)

            dtCellParaDetailsMana = bsdlCommonLibraryInRun.GetParameterConfig("GSM Daily Para")
            strBSCParaUpDatePath = dtCellParaDetailsMana.Rows(0).Item("UpDatePath").ToString
            strBSCParaUpdateSource = dtCellParaDetailsMana.Rows(0).Item("UpdateSourceName").ToString


            intJ = strBSCParaUpdateSource.IndexOf("*")
            intK = strBSCParaUpdateSource.IndexOf("%")
            If intJ <> -1 And intK <> -1 Then
                intI = CommonLibrary.GetMinNumber(intJ, intK)
                strHeadOfSource = strBSCParaUpdateSource.Substring(0, intI)
            ElseIf intJ = -1 And intK = -1 Then
                strHeadOfSource = ""
            ElseIf intJ = -1 Then
                strHeadOfSource = strBSCParaUpdateSource.Substring(0, intK)
            Else
                strHeadOfSource = strBSCParaUpdateSource.Substring(0, intJ)
            End If
            If System.IO.Directory.Exists(strBSCParaUpDatePath) Then
                strtmpListDir = (From T In IO.Directory.GetFiles(strBSCParaUpDatePath, strHeadOfSource & "*.mdb", IO.SearchOption.AllDirectories)).ToList
            End If


            bsdlCommonLibraryInRun.LogOnTextBoxAndDataBaseForBaseSation("列队插入数据库--正在找寻例行P文件", "", lstConsole)
            intWhereYear = strBSCParaUpdateSource.IndexOf("%yyyy")
            intWhereMonth = strBSCParaUpdateSource.IndexOf("%mm") - 1
            intWhereDay = strBSCParaUpdateSource.IndexOf("%dd") - 2
            intWhereHour = strBSCParaUpdateSource.IndexOf("%hh") - 3
            intWhereMin = strBSCParaUpdateSource.IndexOf("%MM") - 4
            intWhereSec = strBSCParaUpdateSource.IndexOf("%ss") - 5
            strDir = CommonLibrary.GetMaxDateFile(strtmpListDir, intWhereYear, intWhereMonth, intWhereDay, intWhereHour, intWhereMin, intWhereSec)



            If strDir.Count > 0 Then
                strtmpFileName = IO.Path.GetFileName(strDir(0))
                dateWhatNow = New Date(strtmpFileName.Substring(intWhereYear, 4), strtmpFileName.Substring(intWhereMonth, 2), strtmpFileName.Substring(intWhereDay, 2), strtmpFileName.Substring(intWhereHour, 2), strtmpFileName.Substring(intWhereMin, 2), strtmpFileName.Substring(intWhereSec, 2))
                arrobjParaOfBSCPara = {strDir(0), dateWhatNow, "F:\SanShi\EarthlyBranch\BSDetails\Config\BSCParaConfig.json", "F:\SanShi\EarthlyBranch\BSDetails\Config\GSMCellParaConfig.json", True, True}
                bsdlCommonLibraryInRun.LogOnTextBoxAndDataBaseForBaseSation("列队插入数据库--找到例行P文件了，最新的文件日期是: " & dateWhatNow.ToString, "", lstConsole)
                bsdlCommonLibraryInRun.LogOnTextBoxAndDataBaseForBaseSation("", ",", lstConsole)
            Else
                bsdlCommonLibraryInRun.LogOnTextBoxAndDataBaseForBaseSation("列队插入数据库--找不到例行P数文件哟:× ", "", lstConsole)
                bsdlCommonLibraryInRun.LogOnTextBoxAndDataBaseForBaseSation("", ",", lstConsole)
            End If


            If (System.IO.File.Exists("F:\SanShi\EarthlyBranch\TrafficStatistics\Config\GSMIndexOfCell.json")) Then
                strJsonLoad = System.IO.File.ReadAllText(("F:\SanShi\EarthlyBranch\TrafficStatistics\Config\GSMIndexOfCell.json"))
                gsmiccInsertToDataBase = SimpleJson.SimpleJson.DeserializeObject(Of GSMIndexOfCellConfig)(strJsonLoad)
                intJ = gsmiccInsertToDataBase.strFileName.IndexOf("*")
                intK = gsmiccInsertToDataBase.strFileName.IndexOf("%")
                If intJ <> -1 And intK <> -1 Then
                    intI = CommonLibrary.GetMinNumber(intJ, intK)
                    strHeadOfSource = gsmiccInsertToDataBase.strFileName.Substring(0, intI)
                ElseIf intJ = -1 And intK = -1 Then
                    strHeadOfSource = ""
                ElseIf intJ = -1 Then
                    strHeadOfSource = gsmiccInsertToDataBase.strFileName.Substring(0, intK)
                Else
                    strHeadOfSource = gsmiccInsertToDataBase.strFileName.Substring(0, intJ)
                End If
                If System.IO.Directory.Exists(gsmiccInsertToDataBase.strUpdatePath) Then
                    strtmpListDir = (From T In IO.Directory.GetFiles(gsmiccInsertToDataBase.strUpdatePath, strHeadOfSource & "*.xls", IO.SearchOption.AllDirectories)).ToList
                End If

                bsdlCommonLibraryInRun.LogOnTextBoxAndDataBaseForBaseSation("GSM网格指标每日小区--正在找寻可以入数的文件", "", lstConsole)

                intWhereYear = gsmiccInsertToDataBase.strFileName.IndexOf("%yyyy")
                intWhereMonth = gsmiccInsertToDataBase.strFileName.IndexOf("%mm") - 1
                intWhereDay = gsmiccInsertToDataBase.strFileName.IndexOf("%dd") - 2

                strDir = CommonLibrary.GetAfterOneDateFile(strtmpListDir, intWhereYear, intWhereMonth, intWhereDay, gsmioclLibrary.GetGSMIndexMaxDate)


                If strDir.Count > 0 Then
                    arrobjParaOfBGWorkerForIndexOfTraffic = {strDir}

                    bsdlCommonLibraryInRun.LogOnTextBoxAndDataBaseForBaseSation("GSM网格指标每日小区--开始入数了哦~", "", lstConsole)
                    bsdlCommonLibraryInRun.LogOnTextBoxAndDataBaseForBaseSation("", ",", lstConsole)

                Else
                    bsdlCommonLibraryInRun.LogOnTextBoxAndDataBaseForBaseSation("GSM网格指标每日小区--找不到网格小区指标的文件哟:×", "", lstConsole)
                    bsdlCommonLibraryInRun.LogOnTextBoxAndDataBaseForBaseSation("", ",", lstConsole)


                End If




            Else
                bsdlCommonLibraryInRun.LogOnTextBoxAndDataBaseForBaseSation("GSM网格指标每日小区--找不到配置文件哟:×", "", lstConsole)
                bsdlCommonLibraryInRun.LogOnTextBoxAndDataBaseForBaseSation("", ",", lstConsole)

            End If





            'arrobjParaOfBGWorker = {listbsdipBSInsertPara, arrobjParaOfBSCPara, arrobjParaOfBGWorkerForIndexOfTraffic}
            arrobjParaOfBGWorker = {Nothing, arrobjParaOfBSCPara, Nothing}

            bwGetEnterWorker.RunWorkerAsync(arrobjParaOfBGWorker)

            timerShowRuning.Enabled = True

            btnGo.Enabled = False

            btnGo.Text = "Insterting.."
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub







    ''' <summary>
    ''' 开始运行入数过程
    ''' </summary>
    Private Sub GoEnterTheData(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles bwGetEnterWorker.DoWork
        Dim listbsdipBSInsertPara As List(Of BaseSationDetailsInsertParaClass)
        Dim bsdipOneOfPara As BaseSationDetailsInsertParaClass
        Dim arrobjParaOfBSCPara As Object()
        Dim arrobjParaOfBSCParaForIndexOfTraffic As Object()
        Dim objResult As Object
        Dim strDir As New List(Of String)
        Dim strtmpDir As String
        Dim exlExl As LoadExcel
        Dim strIFExcelThenSheetName As String
        Dim dtExl As DataTable
        Dim dtData As DataTable
        Dim dtFormat As DataTable

        Try


            listbsdipBSInsertPara = e.Argument(0)
            If listbsdipBSInsertPara IsNot Nothing Then

                For Each bsdipOneOfPara In listbsdipBSInsertPara
                    objResult = bsdlCommonLibraryInRun.BulkCopyToSQLServer(bsdipOneOfPara.strDataTableName, bsdipOneOfPara.strUpDatePath, bsdipOneOfPara.strFileSuffix, bsdipOneOfPara.strIFExcelThenSheetName, Convert.ToInt32(bsdipOneOfPara.intMultiFile), bsdipOneOfPara.strUpDateSource, Convert.ToInt32(bsdipOneOfPara.strDataTableID))
                    If IsNumeric(objResult) Then

                        If objResult = 0 Then
                            strOutput += bsdipOneOfPara.strConfigName & " ×-找不到数据" & vbCrLf
                        ElseIf objResult = 88 Then
                            strOutput += bsdipOneOfPara.strConfigName & " √-已经完成入数了" & vbCrLf
                        ElseIf objResult = -44 Then
                            strOutput += bsdipOneOfPara.strConfigName & " ×-入数有问题！！" & vbCrLf
                        End If
                    Else
                        strOutput += "  ×-入数有问题！！问题是: " & objResult & vbCrLf
                    End If
                    bwGetEnterWorker.ReportProgress(0)

                Next
            Else
                strOutput += "  ×-不入基站信息数据"

            End If

            arrobjParaOfBSCPara = e.Argument(1)
            If arrobjParaOfBSCPara IsNot Nothing Then


                If arrobjParaOfBSCPara.Count > 0 Then

                    If arrobjParaOfBSCPara(4) Then
                        objResult = bscpCommonLibraryInRun.HandelDailyAccessBSCPara(arrobjParaOfBSCPara(0), "dt_GSMP_BSC_Daily", arrobjParaOfBSCPara(1), arrobjParaOfBSCPara(2))
                        If IsNumeric(objResult) Then

                            If objResult = 88 Then
                                strOutput += "√-例行P BSC级数据 已经完成入数了，文件是:" & IO.Path.GetFileName(arrobjParaOfBSCPara(0)) & "" & vbCrLf
                            ElseIf objResult = -44 Then
                                strOutput += "×-例行P BSC级数据 入数有问题！！文件是:" & IO.Path.GetFileName(arrobjParaOfBSCPara(0)) & "" & vbCrLf
                            End If
                        Else
                            strOutput += "×-" & IO.Path.GetFileName(arrobjParaOfBSCPara(0)) & " 入数有问题！！问题是: " & objResult.ToString & vbCrLf
                        End If
                        bsdlCommonLibraryInRun.UpdateParaDate("GSM Daily Para", arrobjParaOfBSCPara(1))

                    End If
                    bwGetEnterWorker.ReportProgress(0)

                    If arrobjParaOfBSCPara(5) Then
                        objResult = gsmcCommonLibraryInRun.HandelDailyAccessGSMCellPara(arrobjParaOfBSCPara(0), "dt_GSMP_Cell_Daily", arrobjParaOfBSCPara(1), arrobjParaOfBSCPara(3), "SELECT [CELL],[ID] FROM [SanShi_BaseSationDetails].[dbo].[dt_GSM_ID]")
                        If IsNumeric(objResult) Then

                            If objResult = 88 Then
                                strOutput += "√-例行P Cell级数据 已经完成入数了，文件是:" & IO.Path.GetFileName(arrobjParaOfBSCPara(0)) & "" & vbCrLf
                            ElseIf objResult = -44 Then
                                strOutput += "×-例行P Cell级数据 入数有问题！！文件是:" & IO.Path.GetFileName(arrobjParaOfBSCPara(0)) & "" & vbCrLf
                            End If
                        Else
                            strOutput += "×-" & IO.Path.GetFileName(arrobjParaOfBSCPara(0)) & " 入数有问题！！问题是: " & objResult.ToString & vbCrLf
                        End If
                    End If
                    bwGetEnterWorker.ReportProgress(0)
                End If
            Else
                strOutput += "×-不入例行P"
            End If

            arrobjParaOfBSCParaForIndexOfTraffic = e.Argument(2)
            If arrobjParaOfBSCParaForIndexOfTraffic IsNot Nothing Then



                If arrobjParaOfBSCParaForIndexOfTraffic.Count > 0 Then
                    strDir = arrobjParaOfBSCParaForIndexOfTraffic(0)
                    dtFormat = sqllSSLibrary.ReturnFormat("dt_GSM_Daily_Grib_Traffic", CommonLibrary.GetSQLServerConnect("ConnectionTrafficDB"))

                    For Each strtmpDir In strDir
                        exlExl = New LoadExcel(strtmpDir)
                        exlExl.GetInformation()
                        strIFExcelThenSheetName = exlExl.strSheets(0)

                        dtExl = exlExl.GetData(strIFExcelThenSheetName)
                        dtData = CommonLibrary.ReturnNewNormalDT(dtExl, dtFormat)
                        sqllSSLibrary.BlukInsert("dt_GSM_Daily_Grib_Traffic", dtData, CommonLibrary.GetSQLServerConnect("ConnectionTrafficDB"))

                        If exlExl.strSheets.Count > 0 Then
                            dtData.Dispose()
                            dtData = Nothing
                        End If
                        exlExl.Dispose()
                        exlExl = Nothing

                        strOutput += "√-已完成文件 : " & IO.Path.GetFileName(strtmpDir) & "的入数" & vbCrLf
                        bwGetEnterWorker.ReportProgress(0)

                    Next

                End If
            Else
                strOutput += "×-不入GSM网格每日小区" & vbCrLf
            End If

        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub



    ''' <summary>
    ''' 当运行结束时输出结果
    ''' </summary>
    Private Sub bwGetEnterWorker_RunWorkerCompleted(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs) Handles bwGetEnterWorker.RunWorkerCompleted
        Try


            bsdlCommonLibraryInRun.LogOnTextBoxAndDataBaseForBaseSation("", ",", lstConsole)

            bsdlCommonLibraryInRun.LogOnTextBoxAndDataBaseForBaseSation("列队插入数据库--完成了", "", lstConsole)
            bsdlCommonLibraryInRun.LogOnTextBoxAndDataBaseForBaseSation("", ".", lstConsole)

            timerShowRuning.Enabled = False
            btnGo.Text = "Done , Please Close The Window"
            If bolBackState Then

                Me.OnFormClosed(Nothing)
                Close()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub

    ''' <summary>
    ''' 后台进程的报告段
    ''' </summary>
    ''' <param name="e">进程数变量</param>
    Private Sub bwGetEnterWorker_ProgressChanged(ByVal sender As Object, ByVal e As ProgressChangedEventArgs) Handles bwGetEnterWorker.ProgressChanged
        Try

            If strOutput <> "" Then
                bsdlCommonLibraryInRun.LogOnTextBoxAndDataBaseForBaseSation("  " & strOutput, "", lstConsole)
                strOutput = ""
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub


End Class