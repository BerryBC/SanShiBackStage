Imports System.ComponentModel
Imports ExcelLibrary
Imports SQLServerLibrary
Imports System.Windows.Forms
Imports System.Runtime.InteropServices
Imports System.IO


Public Class RunConsole
    ''' <summary>
    ''' 后台工作机器人
    ''' </summary>
    Public WithEvents bwGetEnterWorker As BackgroundWorker


    Private Const BM_CLICK = &HF5
    Private Const WM_SETTEXT = &HC
    Private Const WM_LBUTTONDOWN = &H201
    Private Const WM_LBUTTONUP = &H202
    Private Const WM_GETTEXT = &HD
    Private Const WM_KEYDOWN = &H100
    Private Const WM_KEYUP = &H101
    Private Declare Function SendMessage Lib "user32" Alias "SendMessageA" (ByVal hwnd As Integer, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As String) As Integer
    Private Declare Function FindWindow Lib "user32" Alias "FindWindowA" (ByVal lpClassName As String, ByVal lpWindowName As String) As Integer
    Private Declare Function FindWindowEx Lib "user32" Alias "FindWindowExA" (ByVal hWnd1 As Integer, ByVal hWnd2 As Integer, ByVal lpsz1 As String, ByVal lpsz2 As String) As Integer
    Private Declare Function GetClassName Lib "user32" Alias "GetClassNameA" (ByVal hwnd As Integer, ByVal lpClassName As String, ByVal nMaxCount As Integer) As Integer
    Private Delegate Function EnumWindowProcess(ByVal Handle As IntPtr, ByVal Parameter As IntPtr) As Boolean
    Private Declare Function EnumChildWindows Lib "user32" (ByVal WindowHandle As IntPtr, ByVal Callback As EnumWindowProcess, ByVal lParam As IntPtr) As Boolean


    Dim boolGo1 As Boolean = False

    Private intCount As Integer = 0
    Dim strFileAdd As String


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

    Dim strSanShiPath As String = "G:\SanShi\"

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
        bsdlCommonLibraryInRun.LogOnTextBoxAndDataBaseForBaseSation("", ".", lstConsole)
        Dim srGetAdd As System.IO.StreamReader
        If File.Exists(AppDomain.CurrentDomain.BaseDirectory & "config\Address.txt") Then
            srGetAdd = New StreamReader(AppDomain.CurrentDomain.BaseDirectory & "config\Address.txt")
            strFileAdd = srGetAdd.ReadLine
            strFileAdd = strFileAdd.Split("=")(1)
            srGetAdd.Close()
        End If

        'GoHandleBtn()
        intCount = 1
        wbGetNAMS.Navigate("http://10.244.78.93:8081//default.aspx")


    End Sub


    Private Sub GoHandleBtn()
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

            'If bolIsBSConfig Then

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
            'Else
            '    listbsdipBSInsertPara = Nothing
            '    bsdlCommonLibraryInRun.LogOnTextBoxAndDataBaseForBaseSation("列队插入数据库--不入基站信息表", "", lstConsole)
            'End If


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
                arrobjParaOfBSCPara = {strDir(0), dateWhatNow, strSanShiPath & "EarthlyBranch\BSDetails\Config\BSCParaConfig.json", strSanShiPath & "EarthlyBranch\BSDetails\Config\GSMCellParaConfig.json", True, True}
                bsdlCommonLibraryInRun.LogOnTextBoxAndDataBaseForBaseSation("列队插入数据库--找到例行P文件了，最新的文件日期是: " & dateWhatNow.ToString, "", lstConsole)
                bsdlCommonLibraryInRun.LogOnTextBoxAndDataBaseForBaseSation("", ",", lstConsole)
            Else
                bsdlCommonLibraryInRun.LogOnTextBoxAndDataBaseForBaseSation("列队插入数据库--找不到例行P数文件哟:× ", "", lstConsole)
                bsdlCommonLibraryInRun.LogOnTextBoxAndDataBaseForBaseSation("", ",", lstConsole)
            End If


            If (System.IO.File.Exists(strSanShiPath & "EarthlyBranch\TrafficStatistics\Config\GSMIndexOfCell.json")) Then
                strJsonLoad = System.IO.File.ReadAllText((strSanShiPath & "EarthlyBranch\TrafficStatistics\Config\GSMIndexOfCell.json"))
                gsmiccInsertToDataBase = SimpleJson.SimpleJson.DeserializeObject(Of GSMIndexOfCellConfig)(strJsonLoad)
                intJ = gsmiccInsertToDataBase.strFileName.IndexOf("*")
                intK = gsmiccInsertToDataBase.strFileName.IndexOf("%")
                If intJ <> -1 And intK <> -1 Then
                    intI = CommonLibrary.GetMinNumber(intJ, intK)
                    strHeadOfSource = gsmiccInsertToDataBase.strFileName.Substring(0, intI)
                ElseIf intJ = -1 And intK = -1 Then
                    strHeadOfSource = gsmiccInsertToDataBase.strFileName.ToString
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

                If intWhereDay >= 0 And intWhereMonth >= 0 And intWhereYear >= 0 Then
                    strDir = CommonLibrary.GetAfterOneDateFile(strtmpListDir, intWhereYear, intWhereMonth, intWhereDay, gsmioclLibrary.GetGSMIndexMaxDate)
                Else
                    strDir.Add(strtmpListDir(0))
                End If


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





            arrobjParaOfBGWorker = {listbsdipBSInsertPara, arrobjParaOfBSCPara, arrobjParaOfBGWorkerForIndexOfTraffic}
            'arrobjParaOfBGWorker = {Nothing, Nothing, arrobjParaOfBGWorkerForIndexOfTraffic}

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
        Dim intNumberOfFiles As Integer

        Try
            intNumberOfFiles = 0

            listbsdipBSInsertPara = e.Argument(0)
            If listbsdipBSInsertPara IsNot Nothing Then

                For Each bsdipOneOfPara In listbsdipBSInsertPara
                    Try

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
                    Catch ex As Exception
                        strOutput += bsdipOneOfPara.strConfigName & "  ×-入数有问题！！问题是: " & ex.Message & vbCrLf
                    End Try

                Next
            Else
                strOutput += "  ×-不入基站信息数据"

            End If

            arrobjParaOfBSCPara = e.Argument(1)
            If arrobjParaOfBSCPara IsNot Nothing Then


                If arrobjParaOfBSCPara.Count > 0 Then

                    If arrobjParaOfBSCPara(4) Then
                        Try

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
                        Catch ex As Exception

                        End Try

                    End If
                    bwGetEnterWorker.ReportProgress(0)

                    If arrobjParaOfBSCPara(5) Then
                        Try

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
                        Catch ex As Exception

                        End Try
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
                        Try

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
                        Catch ex As Exception

                        End Try

                    Next

                End If
            Else
                strOutput += "×-不入GSM网格每日小区" & vbCrLf
            End If

            strDir = (From T In IO.Directory.GetFiles(strSanShiPath & "TmpFiles", "*.*", IO.SearchOption.AllDirectories)).ToList
            For Each strtmpDir In strDir
                Try
                    If IO.File.Exists(strtmpDir) Then

                        If IO.File.GetCreationTime(strtmpDir) < Now.AddDays(-1.5) Then
                            My.Computer.FileSystem.DeleteFile(strtmpDir, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin, FileIO.UICancelOption.DoNothing)
                            intNumberOfFiles += 1
                        End If
                    End If
                Catch ex As Exception

                End Try

            Next
            strOutput += "√-已删除过期临时文件 " & intNumberOfFiles & " 个" & vbCrLf
            bwGetEnterWorker.ReportProgress(0)



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



    Public Shared Function GetChildWindows(ByVal ParentHandle As IntPtr) As IntPtr()
        Dim ChildrenList As New List(Of IntPtr)
        Dim ListHandle As GCHandle = GCHandle.Alloc(ChildrenList)
        Try
            EnumChildWindows(ParentHandle, AddressOf EnumWindow, GCHandle.ToIntPtr(ListHandle))
        Finally
            If ListHandle.IsAllocated Then ListHandle.Free()
        End Try
        Return ChildrenList.ToArray
    End Function

    Private Shared Function EnumWindow(ByVal Handle As IntPtr, ByVal Parameter As IntPtr) As Boolean
        Dim ChildrenList As List(Of IntPtr) = GCHandle.FromIntPtr(Parameter).Target
        If ChildrenList Is Nothing Then Throw New Exception("GCHandle Target could not be cast as List(Of IntPtr)")
        ChildrenList.Add(Handle)
        Return True
    End Function

    Private Sub RunConsole_Load(sender As Object, e As EventArgs) Handles Me.Load

    End Sub

    Private Sub timerGoSave_Tick(sender As Object, e As EventArgs) Handles timerGoSave.Tick
        Dim Hwnd_SaveFile As Integer
        Dim ExHwnd_SaveFile As Integer
        Dim intptrAryHwnd_SaveFile As IntPtr()
        Dim intptrExHwnd_SaveFile As IntPtr
        Dim strTmpClass As String

        strTmpClass = Space(255)


        Hwnd_SaveFile = FindWindow(Nothing, "文件下载")
        If Hwnd_SaveFile <> 0 Then
            ExHwnd_SaveFile = FindWindowEx(Hwnd_SaveFile, 0, "Button", "保存(S)")
            SendMessage(ExHwnd_SaveFile, BM_CLICK, 0, "")
            ExHwnd_SaveFile = FindWindowEx(Hwnd_SaveFile, 0, "Button", "保存(&S)")
            SendMessage(ExHwnd_SaveFile, BM_CLICK, 0, "")
        End If
        Hwnd_SaveFile = FindWindow(Nothing, "另存为")
        If Hwnd_SaveFile <> 0 Then
            intptrExHwnd_SaveFile = New IntPtr(Hwnd_SaveFile)
            intptrAryHwnd_SaveFile = GetChildWindows(intptrExHwnd_SaveFile)
            For Each intptrExHwnd_SaveFile In intptrAryHwnd_SaveFile

                GetClassName(intptrExHwnd_SaveFile.ToInt32, strTmpClass, 255)
                strTmpClass = strTmpClass.Replace(" ", "")
                If boolGo1 Then
                    ExHwnd_SaveFile = FindWindowEx(Hwnd_SaveFile, 0, "Button", "保存(S)")
                    SendMessage(ExHwnd_SaveFile, BM_CLICK, 0, "")
                    ExHwnd_SaveFile = FindWindowEx(Hwnd_SaveFile, 0, "Button", "保存(&S)")
                    SendMessage(ExHwnd_SaveFile, BM_CLICK, 0, "")
                    boolGo1 = False
                    'timerGoSave.Enabled = False
                End If

                If strTmpClass.Substring(0, Math.Min(8, strTmpClass.Length)) = "ComboBox" Then
                    Dim dteNow As Date
                    Dim strSaveAdd As String
                    dteNow = Now
                    strSaveAdd = strFileAdd.Substring(0, strFileAdd.Length - 4) & "   " & Format(dteNow, "yyyyMMddHHmm")
                    SendKeys.Send(" ")
                    SendMessage(intptrExHwnd_SaveFile.ToInt32, WM_SETTEXT, 255, strSaveAdd)
                    boolGo1 = True
                End If
                If strTmpClass.Substring(0, 4) = "Edit" Then
                    Dim dteNow As Date
                    Dim strSaveAdd As String
                    dteNow = Now
                    strSaveAdd = strFileAdd & "  " & Format(dteNow, "yyyyMMddHHmm")

                    SendKeys.Send(" ")
                    SendMessage(intptrExHwnd_SaveFile.ToInt32, WM_SETTEXT, 255, strSaveAdd)
                    boolGo1 = True
                End If
                If strTmpClass.Substring(0, Math.Min(14, strTmpClass.Length)) = "FloatNotifySin" Then
                    Dim dteNow As Date
                    Dim strSaveAdd As String
                    dteNow = Now
                    strSaveAdd = strFileAdd & "  " & Format(dteNow, "yyyyMMddHHmm")

                    SendKeys.Send(" ")
                    SendMessage(intptrExHwnd_SaveFile.ToInt32, WM_SETTEXT, 255, strSaveAdd)
                End If
            Next
            GoHandleBtn()

        End If

        Hwnd_SaveFile = FindWindow(Nothing, "下载完毕")
        If Hwnd_SaveFile <> 0 Then
            ExHwnd_SaveFile = FindWindowEx(Hwnd_SaveFile, 0, "Button", "关闭")
            SendMessage(ExHwnd_SaveFile, BM_CLICK, 0, "")
            timerGoSave.Enabled = False
            bsdlCommonLibraryInRun.LogOnTextBoxAndDataBaseForBaseSation("  NAMS保存完毕", "", lstConsole)

        End If

    End Sub

    Private Sub wbGetNAMS_DocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs) Handles wbGetNAMS.DocumentCompleted
        Dim eleHTMLEle As HtmlElement
        Dim eleHTMLEle2 As HtmlElement
        Dim docHTMLDoc As HtmlDocument
        Dim cookieStr As String
        Dim cookstr() As String
        Dim str As String
        Dim cookieCode() As String
        If intCount = 1 Then
            Try
                For Each eleHTMLEle In wbGetNAMS.Document.All
                    wbGetNAMS.Document.GetElementById("TextBox1").SetAttribute("value", "wangxi2")
                    wbGetNAMS.Document.GetElementById("TextBox2").SetAttribute("value", "013579")



                    '读取Cookie
                    cookieStr = wbGetNAMS.Document.Cookie
                    If cookieStr Is Nothing Then
                        timerRefreshNAMS.Enabled = True
                    End If
                    cookstr = cookieStr.Split(";")

                    For Each str In cookstr
                        cookieCode = str.Split("=")
                        If cookieCode(0) = "yzmcode" Then
                            wbGetNAMS.Document.GetElementById("TextBox3").SetAttribute("value", cookieCode(1))
                            wbGetNAMS.Document.GetElementById("Anthem_loginButton__").FirstChild.InvokeMember("Click")
                            bsdlCommonLibraryInRun.LogOnTextBoxAndDataBaseForBaseSation("  登陆NAMS", "", lstConsole)
                            intCount = 2
                            Exit Sub

                        Else
                            bsdlCommonLibraryInRun.LogOnTextBoxAndDataBaseForBaseSation("  NAMS傻了", "", lstConsole)
                            GoHandleBtn()
                            Exit Sub
                        End If
                    Next

                Next
            Catch
                For Each eleHTMLEle In wbGetNAMS.Document.All
                    If eleHTMLEle.OuterText <> Nothing And eleHTMLEle.TagName = "A" Then
                        If eleHTMLEle.OuterText.ToString = "进度管控查询" Then
                            eleHTMLEle.InvokeMember("Click")
                            intCount = 3
                            bsdlCommonLibraryInRun.LogOnTextBoxAndDataBaseForBaseSation("  打开NAMS查询界面", "", lstConsole)
                        End If
                    ElseIf eleHTMLEle.OuterText <> Nothing Then
                        If eleHTMLEle.OuterText.IndexOf("错误") > 0 Then
                            MessageBox.Show("错误了!!!!")
                            intCount = 0
                        End If
                    End If
                Next

            End Try
        ElseIf intCount = 2 Then
            For Each eleHTMLEle In wbGetNAMS.Document.All
                If eleHTMLEle.OuterText <> Nothing And eleHTMLEle.TagName = "A" Then
                    If eleHTMLEle.OuterText.ToString = "进度管控查询" Then
                        eleHTMLEle.InvokeMember("Click")
                        intCount = 3
                        bsdlCommonLibraryInRun.LogOnTextBoxAndDataBaseForBaseSation("  打开NAMS查询界面", "", lstConsole)
                    End If
                ElseIf eleHTMLEle.OuterText <> Nothing Then
                    If eleHTMLEle.OuterText.IndexOf("错误") > 0 Then
                        MessageBox.Show("错误了!!!!")
                        intCount = 0
                    End If
                End If
            Next

        ElseIf intCount = 3 Then


            For Each eleHTMLEle In wbGetNAMS.Document.All
                If eleHTMLEle.Name.ToString = "mainFrame" Then
                    docHTMLDoc = eleHTMLEle.Document.Window.Frames(eleHTMLEle.Name.ToString).Document

                    For Each eleHTMLEle2 In docHTMLDoc.All
                        If eleHTMLEle2.Id <> Nothing Then
                            If eleHTMLEle2.Id.ToString = "btnQuery" Then
                                eleHTMLEle2.InvokeMember("Click")
                                intCount = 4
                                bsdlCommonLibraryInRun.LogOnTextBoxAndDataBaseForBaseSation("  开始查询", "", lstConsole)
                            End If
                        ElseIf eleHTMLEle2.OuterText <> Nothing Then
                            If eleHTMLEle2.OuterText.IndexOf("错误") > 0 Then
                                MessageBox.Show("错误了!!!!")
                                intCount = 0
                            End If
                        End If
                    Next
                End If
            Next
        ElseIf intCount = 4 Then
            For Each eleHTMLEle In wbGetNAMS.Document.All
                If eleHTMLEle.Name.ToString = "mainFrame" Then
                    docHTMLDoc = eleHTMLEle.Document.Window.Frames(eleHTMLEle.Name.ToString).Document
                    For Each eleHTMLEle2 In docHTMLDoc.All
                        If eleHTMLEle2.Id <> Nothing Then
                            If eleHTMLEle2.Id.ToString = "btnExport" Then
                                eleHTMLEle2.InvokeMember("Click")
                                intCount = 5
                                bsdlCommonLibraryInRun.LogOnTextBoxAndDataBaseForBaseSation("  开始保存", "", lstConsole)
                                timerGoSave.Enabled = True
                            End If
                        ElseIf eleHTMLEle2.OuterText <> Nothing Then
                            If eleHTMLEle2.OuterText.IndexOf("错误") > 0 Then
                                MessageBox.Show("错误了!!!!")
                                intCount = 0
                            End If
                        End If
                    Next
                End If
            Next

        ElseIf intCount = 5 Then
            intCount = 0

        End If

    End Sub

    Private Sub btnTest_Click(sender As Object, e As EventArgs) Handles btnTest.Click
        Dim objResult As Object
        Dim aa As String

        objResult = bsdlCommonLibraryInRun.BulkCopyToSQLServer("dt_GSM_ID", "C:\Users\BerryCui\Desktop\临时数据\20161021", "xlsx", "", 0, "ID %yyyy%mm%dd", 3)

        aa = objResult.ToString

    End Sub

    Private Sub timerRefreshNAMS_Tick(sender As Object, e As EventArgs) Handles timerRefreshNAMS.Tick
        wbGetNAMS.Navigate("http://10.244.78.93:8081//default.aspx")
        timerRefreshNAMS.Enabled = False

    End Sub
End Class