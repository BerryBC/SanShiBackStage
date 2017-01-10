﻿Imports Microsoft.VisualBasic
Imports SQLServerLibrary
Imports System.Data.SqlClient
Imports System.Data
Imports CSVLibrary.LoadCSV
Imports CSVLibrary
Imports ExcelLibrary.LoadExcel
Imports ExcelLibrary
Imports System.Data.OleDb
Imports AccessLibrary
Imports SQLServerLibrary.LoadSQLServer
Imports System.Web


Public Class BaseSationDetailsLibrary
    Implements IDisposable

    Dim sqllSSLibrary As LoadSQLServer


    Public Sub New()
        sqllSSLibrary = New LoadSQLServer()
    End Sub

    Public Function ReturnBaseSationDetailsMan() As DataTable
        Dim scmdCMD As SqlCommand
        Dim dtBaseSationDetailsMana As DataTable
        Try
            scmdCMD = sqllSSLibrary.GetCommandStr("select * from dt_ManagementTable", CommonLibrary.GetSQLServerConnect("ConnectionBaseStationDetailsDB"))
            dtBaseSationDetailsMana = sqllSSLibrary.GetSQLServerDataTable(scmdCMD)
            Return dtBaseSationDetailsMana
        Catch ex As Exception
            Throw New Exception(ex.Message, ex)
            dtBaseSationDetailsMana.Dispose()
            dtBaseSationDetailsMana = Nothing
            scmdCMD.Dispose()
            scmdCMD = Nothing
        End Try
    End Function

    Public Function ReturnOneBaseSationDetailsMan(intConfigID As Integer) As DataTable
        Dim scmdCMD As SqlCommand
        Dim dtBaseSationDetailsMana As DataTable
        Dim spID As SqlParameter
        Try
            scmdCMD = sqllSSLibrary.GetCommandProc("proc_GetSomeDataTableConfig", CommonLibrary.GetSQLServerConnect("ConnectionBaseStationDetailsDB"))
            spID = New SqlParameter("@ID", SqlDbType.Int)
            spID.Value = intConfigID
            scmdCMD.Parameters.Add(spID)
            dtBaseSationDetailsMana = sqllSSLibrary.GetSQLServerDataTable(scmdCMD)
            Return dtBaseSationDetailsMana
        Catch ex As Exception
            Throw New Exception(ex.Message, ex)
            dtBaseSationDetailsMana.Dispose()
            dtBaseSationDetailsMana = Nothing
            scmdCMD.Dispose()
            scmdCMD = Nothing
        End Try
    End Function



    Public Function ModifyConfig(intDataTableID As Integer, strConfigName As String, strDataTableName As String, strUpDatePath As String, strFileSuffix As String, strIFExcelThenSheetName As String, intMultiFile As Integer, strUpdateSource As String) As Integer
        Dim scmdCMD As SqlCommand
        Dim spDataTableID As SqlParameter
        Dim spConfigName As SqlParameter
        Dim spDataTableName As SqlParameter
        Dim spUpDatePath As SqlParameter
        Dim spFileSuffix As SqlParameter
        Dim spIFExcelThenSheetName As SqlParameter
        Dim spMultiFile As SqlParameter
        Dim spUpdateSource As SqlParameter
        Dim spReturnValue As SqlParameter
        Try
            scmdCMD = sqllSSLibrary.GetCommandProc("proc_ModifyDataTableConfig", CommonLibrary.GetSQLServerConnect("ConnectionBaseStationDetailsDB"))
            spDataTableID = New SqlParameter("@DataTableID", SqlDbType.Int)
            spConfigName = New SqlParameter("@ConfigName", SqlDbType.VarChar, 50)
            spDataTableName = New SqlParameter("@DataTableName", SqlDbType.VarChar, 50)
            spUpDatePath = New SqlParameter("@UpDatePath", SqlDbType.VarChar, 100)
            spFileSuffix = New SqlParameter("@FileSuffix", SqlDbType.VarChar, 10)
            spIFExcelThenSheetName = New SqlParameter("@IFExcelThenSheetName", SqlDbType.VarChar, 50)
            spMultiFile = New SqlParameter("@MultiFile", SqlDbType.Int)
            spUpdateSource = New SqlParameter("@UpDateSource", SqlDbType.VarChar, 50)
            spReturnValue = New SqlParameter("ReturnValue", SqlDbType.Int, 4)
            spDataTableID.Value = intDataTableID
            spConfigName.Value = strConfigName
            spDataTableName.Value = strDataTableName
            spUpDatePath.Value = strUpDatePath
            spFileSuffix.Value = strFileSuffix
            spIFExcelThenSheetName.Value = strIFExcelThenSheetName
            spUpdateSource.Value = strUpdateSource
            spMultiFile.Value = intMultiFile
            scmdCMD.Parameters.Add(spDataTableID)
            scmdCMD.Parameters.Add(spConfigName)
            scmdCMD.Parameters.Add(spDataTableName)
            scmdCMD.Parameters.Add(spUpDatePath)
            scmdCMD.Parameters.Add(spFileSuffix)
            scmdCMD.Parameters.Add(spIFExcelThenSheetName)
            scmdCMD.Parameters.Add(spMultiFile)
            scmdCMD.Parameters.Add(spReturnValue)
            scmdCMD.Parameters.Add(spUpdateSource)
            spReturnValue.Direction = ParameterDirection.ReturnValue
            sqllSSLibrary.ExecNonQuery(scmdCMD)
            Return Convert.ToInt32(spReturnValue.Value.ToString())
        Catch ex As Exception
            Throw New Exception(ex.Message, ex)
            scmdCMD.Dispose()
            scmdCMD = Nothing
        End Try
    End Function


    Public Function BulkCopyToSQLServer(strDataTableName As String, strUpDatePath As String, strFileSuffix As String, strIFExcelThenSheetName As String, intMultiFile As Integer, strUpdateSource As String, intNumberOfConfig As Integer) As Integer
        Dim strtmpListDir As New List(Of String)
        Dim strtmpFileName As String
        Dim intI As Integer
        Dim intJ As Integer
        Dim intK As Integer
        Dim strHeadOfSource As String
        Dim strDir As New List(Of String)
        Dim intWhereYear As Integer
        Dim intWhereMonth As Integer
        Dim intWhereDay As Integer
        Dim dtFormat As DataTable
        Dim exlExl As LoadExcel
        Dim csvCSV As LoadCSV
        Dim dtExl As DataTable
        Dim dtCsv As DataTable
        Dim dtData As DataTable
        Dim scmdCommand As SqlCommand
        Try
            scmdCommand = sqllSSLibrary.GetCommandStr("delete from " & strDataTableName & ";", CommonLibrary.GetSQLServerConnect("ConnectionBaseStationDetailsDB"))
            scmdCommand.CommandTimeout = 180
            sqllSSLibrary.ExecNonQuery(scmdCommand)
            intJ = strUpdateSource.IndexOf("*")
            intK = strUpdateSource.IndexOf("%")
            If intJ <> -1 And intK <> -1 Then
                intI = CommonLibrary.GetMinNumber(intJ, intK)
                strHeadOfSource = strUpdateSource.Substring(0, intI)
            ElseIf intJ = -1 And intK = -1 Then
                strHeadOfSource = strUpdateSource
            ElseIf intJ = -1 Then
                strHeadOfSource = strUpdateSource.Substring(0, intK)
            Else
                strHeadOfSource = strUpdateSource.Substring(0, intJ)
            End If

            If System.IO.Directory.Exists(strUpDatePath) Then
                strtmpListDir = (From T In IO.Directory.GetFiles(strUpDatePath, strHeadOfSource & "*." & strFileSuffix, IO.SearchOption.AllDirectories)).ToList
            End If
            intWhereYear = strUpdateSource.IndexOf("%yyyy")
            intWhereMonth = strUpdateSource.IndexOf("%mm") - 1
            intWhereDay = strUpdateSource.IndexOf("%dd") - 2
            If strHeadOfSource = "载调&传输提单汇总表" Then
                strDir = strtmpListDir.ToList

            Else
                If intWhereDay >= 0 And intWhereMonth >= 0 And intWhereYear >= 0 Then

                    strDir = CommonLibrary.GetMaxDateFile(strtmpListDir, intWhereYear, intWhereMonth, intWhereDay)
                Else

                    strDir.Add(strtmpListDir(0))

                End If

            End If
                dtFormat = sqllSSLibrary.ReturnFormat(strDataTableName, CommonLibrary.GetSQLServerConnect("ConnectionBaseStationDetailsDB"))
            If intMultiFile = 0 Then
                If strDir.Count > 0 Then

                    strtmpListDir.Clear()
                    strtmpListDir.Add(strDir.Item(0))
                    strDir.Clear()
                    strDir = strtmpListDir
                End If
            End If

            If strDir.Count = 0 Then
                strtmpListDir.Clear()
                strtmpListDir = Nothing
                strDir.Clear()
                strDir = Nothing
                scmdCommand.Dispose()
                scmdCommand = Nothing
                Return 0
            End If
            If strUpdateSource.IndexOf("轩驰表") > 0 Then
                strFileSuffix = "csv"
            End If
            If strFileSuffix = "xls" Or strFileSuffix = "xlsx" Then
                '如果是Excel格式的
                For Each strtmpFileName In strDir
                    exlExl = New LoadExcel(strtmpFileName)
                    If strHeadOfSource <> "载调&传输提单汇总表" Then

                        exlExl.GetInformation()
                    End If
                    If strIFExcelThenSheetName = "" Then
                        strIFExcelThenSheetName = exlExl.strSheets(0)
                    End If

                    dtExl = exlExl.GetData(strIFExcelThenSheetName)
                    dtData = CommonLibrary.ReturnNewNormalDT(dtExl, dtFormat)
                    sqllSSLibrary.BlukInsert(strDataTableName, dtData, CommonLibrary.GetSQLServerConnect("ConnectionBaseStationDetailsDB"))

                    If exlExl.strSheets.Count > 0 Then
                        dtData.Dispose()
                        dtData = Nothing
                    End If
                    exlExl.Dispose()
                    exlExl = Nothing
                Next
            ElseIf strFileSuffix = "csv" Or strFileSuffix = "txt" Then
                For Each strtmpFileName In strDir
                    '其他所有的直接入
                    If strUpdateSource.IndexOf("轩驰表") > 0 Then
                        csvCSV = New LoadCSV(strtmpFileName, Chr(9))
                    Else
                        csvCSV = New LoadCSV(strtmpFileName)
                    End If
                    dtCsv = csvCSV.GetDataViaTxtReader(dtFormat)
                    sqllSSLibrary.BlukInsert(strDataTableName, dtCsv, CommonLibrary.GetSQLServerConnect("ConnectionBaseStationDetailsDB"))
                    dtCsv.Dispose()
                    dtCsv = Nothing
                    csvCSV.Dispose()
                    csvCSV = Nothing
                Next
            Else
                Return -44
            End If
            UpdateConfigDate(intNumberOfConfig, IO.Path.GetFileName(strDir(0)))
        Catch ex As Exception
            Throw New Exception(ex.Message, ex)
            scmdCommand.Dispose()
            scmdCommand = Nothing
            Return -44

        End Try
        Return 88
    End Function

    Public Sub DeleteConfig(ByRef intWhatNumber As Integer)
        Dim scmdCMD As SqlCommand
        Try
            scmdCMD = sqllSSLibrary.GetCommandStr("delete from dt_ManagementTable where [DataTableID]=" & intWhatNumber.ToString & ";", CommonLibrary.GetSQLServerConnect("ConnectionBaseStationDetailsDB"))
            sqllSSLibrary.ExecNonQuery(scmdCMD)

        Catch ex As Exception
            Throw New Exception(ex.Message, ex)
            scmdCMD.Dispose()
            scmdCMD = Nothing
        End Try

    End Sub
    Public Sub AddConfig()
        Dim scmdCMD As SqlCommand
        Try
            scmdCMD = sqllSSLibrary.GetCommandProc("proc_AddConfig", CommonLibrary.GetSQLServerConnect("ConnectionBaseStationDetailsDB"))
            sqllSSLibrary.GetSQLServerDataTable(scmdCMD)
        Catch ex As Exception
            Throw New Exception(ex.Message, ex)
            scmdCMD.Dispose()
            scmdCMD = Nothing
        End Try

    End Sub

    Public Sub UpdateConfigDate(intDataTableID As Integer, strFileName As String)
        Dim scmdCMD As SqlCommand
        Dim spDataTableID As SqlParameter
        Dim spFileName As SqlParameter
        Try
            scmdCMD = sqllSSLibrary.GetCommandProc("proc_UpdateDate", CommonLibrary.GetSQLServerConnect("ConnectionBaseStationDetailsDB"))
            spDataTableID = New SqlParameter("@DataTableID", SqlDbType.Int)
            spFileName = New SqlParameter("@FileName", SqlDbType.VarChar, 100)
            spFileName.Value = strFileName
            spDataTableID.Value = intDataTableID
            scmdCMD.Parameters.Add(spDataTableID)
            scmdCMD.Parameters.Add(spFileName)
            sqllSSLibrary.ExecNonQuery(scmdCMD)
        Catch ex As Exception
            Throw New Exception(ex.Message, ex)
            scmdCMD.Dispose()
            scmdCMD = Nothing
        End Try
    End Sub



    Public Function HowManyRowsOfDataTable(strDataTable As String) As Integer
        Dim scmdCMD As SqlCommand
        Dim spDataTableName As SqlParameter
        Dim spReturnValue As SqlParameter
        Try
            scmdCMD = sqllSSLibrary.GetCommandProc("proc_HowManyDateRow", CommonLibrary.GetSQLServerConnect("ConnectionBaseStationDetailsDB"))
            spDataTableName = New SqlParameter("@TableName", SqlDbType.VarChar, 200)
            spReturnValue = New SqlParameter("ReturnValue", SqlDbType.Int, 4)
            spDataTableName.Value = strDataTable
            scmdCMD.Parameters.Add(spDataTableName)
            scmdCMD.Parameters.Add(spReturnValue)
            spReturnValue.Direction = ParameterDirection.ReturnValue
            sqllSSLibrary.ExecNonQuery(scmdCMD)
            Return Convert.ToInt32(spReturnValue.Value.ToString())
        Catch ex As Exception
            Throw New Exception(ex.Message, ex)
            scmdCMD.Dispose()
            scmdCMD = Nothing
        End Try
    End Function


    Public Sub LogOnTextBoxAndDataBaseForBaseSation(strLogStr As String, strWhatFormat As String, ByRef txtLogListBox As System.Windows.Forms.ListBox)
        Dim strWhatLog As String
        Dim scmdCMD As SqlCommand
        Dim spLogString As SqlParameter
        Try
            If strWhatFormat = "," Then
                strWhatLog = "- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - "
                txtLogListBox.Items.Add(strWhatLog)
            ElseIf strWhatFormat = "." Then
                strWhatLog = "------------------------------------------------------------"
                txtLogListBox.Items.Add(strWhatLog)
            Else
                strWhatLog = " " & strLogStr
                txtLogListBox.Items.Add(Now.ToString & strWhatLog)
            End If
            scmdCMD = sqllSSLibrary.GetCommandProc("[proc_LogBaseSationConfig]", CommonLibrary.GetSQLServerConnect("ConnectionLogDB"))
            spLogString = New SqlParameter("@LogString", SqlDbType.Text)
            spLogString.Value = strWhatLog
            scmdCMD.Parameters.Add(spLogString)
            sqllSSLibrary.ExecNonQuery(scmdCMD)
        Catch ex As Exception
            Throw New Exception(ex.Message, ex)
            scmdCMD.Dispose()
            scmdCMD = Nothing
        End Try
    End Sub

    Public Function GetParameterConfig(strWhichConfig As String) As DataTable
        Dim scmdCMD As SqlCommand
        Dim dtBaseSationDetailsMana As DataTable
        Try
            scmdCMD = sqllSSLibrary.GetCommandStr("select * from dt_ManagementParameter", CommonLibrary.GetSQLServerConnect("ConnectionBaseStationDetailsDB"))
            dtBaseSationDetailsMana = sqllSSLibrary.GetSQLServerDataTable(scmdCMD)
            dtBaseSationDetailsMana = (From someDTR As DataRow In dtBaseSationDetailsMana.AsEnumerable Where someDTR.Field(Of String)("ConfigName") = strWhichConfig Select someDTR).CopyToDataTable
            Return dtBaseSationDetailsMana
        Catch ex As Exception
            Throw New Exception(ex.Message, ex)
            dtBaseSationDetailsMana.Dispose()
            dtBaseSationDetailsMana = Nothing
            scmdCMD.Dispose()
            scmdCMD = Nothing
        End Try
    End Function


    Public Function GetParameterConfig() As DataTable
        Dim scmdCMD As SqlCommand
        Dim dtBaseSationDetailsMana As DataTable

        Try
            scmdCMD = sqllSSLibrary.GetCommandStr("select * from dt_ManagementParameter", CommonLibrary.GetSQLServerConnect("ConnectionBaseStationDetailsDB"))
            dtBaseSationDetailsMana = sqllSSLibrary.GetSQLServerDataTable(scmdCMD)
            Return dtBaseSationDetailsMana
        Catch ex As Exception
            Throw New Exception(ex.Message, ex)
            dtBaseSationDetailsMana.Dispose()
            dtBaseSationDetailsMana = Nothing
            scmdCMD.Dispose()
            scmdCMD = Nothing
        End Try
    End Function

    Public Function ModifyParaConfig(strConfigName As String, strUpDatePath As String, strUpdateSource As String) As Integer
        Dim scmdCMD As SqlCommand
        Dim spConfigName As SqlParameter
        Dim spUpDatePath As SqlParameter
        Dim spUpdateSource As SqlParameter
        Dim spReturnValue As SqlParameter
        Try
            scmdCMD = sqllSSLibrary.GetCommandProc("proc_ModifyParaConfig", CommonLibrary.GetSQLServerConnect("ConnectionBaseStationDetailsDB"))
            spConfigName = New SqlParameter("@ConfigName", SqlDbType.VarChar, 50)
            spUpDatePath = New SqlParameter("@UpDatePath", SqlDbType.VarChar, 100)
            spUpdateSource = New SqlParameter("@UpDateSource", SqlDbType.VarChar, 100)
            spReturnValue = New SqlParameter("ReturnValue", SqlDbType.Int, 4)
            spConfigName.Value = strConfigName
            spUpDatePath.Value = strUpDatePath
            spUpdateSource.Value = strUpdateSource
            scmdCMD.Parameters.Add(spConfigName)
            scmdCMD.Parameters.Add(spUpDatePath)
            scmdCMD.Parameters.Add(spReturnValue)
            scmdCMD.Parameters.Add(spUpdateSource)
            spReturnValue.Direction = ParameterDirection.ReturnValue
            sqllSSLibrary.ExecNonQuery(scmdCMD)
            Return Convert.ToInt32(spReturnValue.Value.ToString())
        Catch ex As Exception
            Throw New Exception(ex.Message, ex)
            scmdCMD.Dispose()
            scmdCMD = Nothing
        End Try
    End Function

    Public Sub UpdateParaDate(strConfigName As String, dateUpdateDate As Date)

        Dim scmdCMD As SqlCommand
        Dim spConfigName As SqlParameter
        Dim spUpdateDate As SqlParameter
        Try
            scmdCMD = sqllSSLibrary.GetCommandProc("proc_UpdateParaDate", CommonLibrary.GetSQLServerConnect("ConnectionBaseStationDetailsDB"))
            spConfigName = New SqlParameter("@ConfigName", SqlDbType.VarChar, 50)
            spUpdateDate = New SqlParameter("@UpDateDate", SqlDbType.DateTime)
            spConfigName.Value = strConfigName
            spUpdateDate.Value = dateUpdateDate
            scmdCMD.Parameters.Add(spConfigName)
            scmdCMD.Parameters.Add(spUpdateDate)
            sqllSSLibrary.ExecNonQuery(scmdCMD)
        Catch ex As Exception
            Throw New Exception(ex.Message, ex)
            scmdCMD.Dispose()
            scmdCMD = Nothing
        End Try


    End Sub

#Region "IDisposable Support"
    Private disposedValue As Boolean ' 要检测冗余调用

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: 释放托管状态(托管对象)。
            End If

            ' TODO: 释放未托管资源(未托管对象)并在以下内容中替代 Finalize()。
            ' TODO: 将大型字段设置为 null。
        End If
        disposedValue = True
    End Sub

    ' TODO: 仅当以上 Dispose(disposing As Boolean)拥有用于释放未托管资源的代码时才替代 Finalize()。
    'Protected Overrides Sub Finalize()
    '    ' 请勿更改此代码。将清理代码放入以上 Dispose(disposing As Boolean)中。
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' Visual Basic 添加此代码以正确实现可释放模式。
    Public Sub Dispose() Implements IDisposable.Dispose
        ' 请勿更改此代码。将清理代码放入以上 Dispose(disposing As Boolean)中。
        Dispose(True)
        ' TODO: 如果在以上内容中替代了 Finalize()，则取消注释以下行。
        ' GC.SuppressFinalize(Me)
    End Sub
#End Region



End Class
