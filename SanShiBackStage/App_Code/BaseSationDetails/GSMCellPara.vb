﻿Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports AccessLibrary
Imports SQLServerLibrary
Imports System.IO
Imports SQLServerLibrary.LoadSQLServer

Public Class GSMCellPara
    Dim sqllSSLibrary As LoadSQLServer = New LoadSQLServer



    Public Function HandelDailyAccessGSMCellPara(strSourceAccessDataBase As String, strSQLServerTableName As String, dateWhatTimeIsPara As Date, strConfigFile As String, strGetIDSQLS As String) As Integer
        Dim pasTmpParaAndSQL As ParameterAndSQL
        Dim tblTableBscList As TableBSCList
        Dim dtFormat As DataTable
        Dim dtData As DataTable
        Dim dttmpData As DataTable
        Dim odbcOleDBCommand As OleDbCommand
        Dim aceAccess As LoadAccess
        Dim drtmpBscListRow As DataRow
        Dim listBscList As List(Of String)
        Dim strtmpBSC As String
        Dim listBSCPara As List(Of List(Of List(Of Object)))
        Dim listtmpBSCPara As List(Of List(Of Object))
        Dim listtmpBSCParaEveryBSC As New List(Of Object)
        Dim scmdCommand As SqlCommand
        Dim dtOrgData As DataTable
        Dim listOriPara As List(Of BscOriginalPara)
        Dim boptmpBSCPara As BscOriginalPara
        Dim drtmpBSCCompareOf As DataRow
        Dim intI As Integer
        Dim sabConfigSQLSandBSCList As SQLSandBSCList
        Dim strJsonLoad As String
        Dim dtCellID As DataTable
        Dim listdCellID As Dictionary(Of String, String)
        Dim intNumberOfFN As Integer
        Dim tmpType As Type
        Dim dtDataForLog As DataTable
        Dim dtFormatForLog As DataTable
        Dim drtmpBscListRowForLog As DataRow



        Try


            listOriPara = New List(Of BscOriginalPara)
            listBscList = New List(Of String)
            listBSCPara = New List(Of List(Of List(Of Object)))
            listdCellID = New Dictionary(Of String, String)





            strJsonLoad = File.ReadAllText(strConfigFile)
            sabConfigSQLSandBSCList = SimpleJson.SimpleJson.DeserializeObject(Of SQLSandBSCList)(strJsonLoad)


            scmdCommand = sqllSSLibrary.GetCommandStr(strGetIDSQLS, CommonLibrary.GetSQLServerConnect("ConnectionBaseStationDetailsDB"))
            dtCellID = sqllSSLibrary.GetSQLServerDataTable(scmdCommand)
            For Each drtmpBscListRow In dtCellID.Rows
                If ((drtmpBscListRow(0) IsNot DBNull.Value) And (drtmpBscListRow(1) IsNot DBNull.Value)) Then
                    listdCellID.Add(drtmpBscListRow(0), drtmpBscListRow(1))
                End If
            Next


            scmdCommand = sqllSSLibrary.GetCommandStr("select * from dt_GSMP_Cell_Daily", CommonLibrary.GetSQLServerConnect("ConnectionBaseStationDetailsDB"))
            dtOrgData = sqllSSLibrary.GetSQLServerDataTable(scmdCommand)

            scmdCommand = sqllSSLibrary.GetCommandStr("delete from " & strSQLServerTableName & ";", CommonLibrary.GetSQLServerConnect("ConnectionBaseStationDetailsDB"))
            sqllSSLibrary.ExecNonQuery(scmdCommand)



            dtFormat = sqllSSLibrary.ReturnFormat(strSQLServerTableName, CommonLibrary.GetSQLServerConnect("ConnectionBaseStationDetailsDB"))
            dtFormat.Rows.Clear()
            dtData = dtFormat


            For Each drtmpBscListRow In dtOrgData.Rows
                boptmpBSCPara = New BscOriginalPara
                boptmpBSCPara.strBSCName = drtmpBscListRow(0)
                boptmpBSCPara.drParaRow = drtmpBscListRow
                listOriPara.Add(boptmpBSCPara)
            Next



            aceAccess = New LoadAccess(strSourceAccessDataBase, True)




            For Each tblTableBscList In sabConfigSQLSandBSCList.listtblBscList
                Try

                    odbcOleDBCommand = New OleDbCommand("SELECT " & tblTableBscList.strBSCName & " FROM " & tblTableBscList.strTableName & " GROUP BY " & tblTableBscList.strBSCName & ";")
                    dttmpData = aceAccess.GetAccessDataTable(odbcOleDBCommand)
                    For Each drtmpBscListRow In dttmpData.Rows
                        listBscList.Add（drtmpBscListRow(0))
                    Next
                Catch ex As Exception

                End Try
            Next

            For Each pasTmpParaAndSQL In sabConfigSQLSandBSCList.listpasParaAndSQLS
                Try

                    odbcOleDBCommand = New OleDbCommand(pasTmpParaAndSQL.strSQLStatements)
                    dttmpData = aceAccess.GetAccessDataTable(odbcOleDBCommand)
                    listtmpBSCPara = New List(Of List(Of Object))
                    For Each drtmpBscListRow In dttmpData.Rows
                        listtmpBSCParaEveryBSC = New List(Of Object)
                        For intI = 0 To drtmpBscListRow.ItemArray.Count - 1
                            listtmpBSCParaEveryBSC.Add(drtmpBscListRow(intI))
                        Next
                        listtmpBSCPara.Add（listtmpBSCParaEveryBSC）
                    Next
                    listBSCPara.Add(listtmpBSCPara)
                Catch ex As Exception
                    listtmpBSCPara = New List(Of List(Of Object))
                    listBSCPara.Add(listtmpBSCPara)
                End Try
            Next

            aceAccess.Close()
            Dim tmplistBscList = (From tmpStr In listBscList Select tmpStr).Distinct


            For Each strtmpBSC In tmplistBscList
                drtmpBscListRow = dtFormat.NewRow
                drtmpBscListRow(1) = strtmpBSC
                drtmpBscListRow(0) = ReturnCellOnlyID(listdCellID, strtmpBSC)
                For Each listtmpBSCPara In listBSCPara
                    Dim strWhichPara As String
                    strWhichPara = sabConfigSQLSandBSCList.listpasParaAndSQLS(listBSCPara.IndexOf(listtmpBSCPara)).strColName
                    If strWhichPara = "NumberOfFN" Then
                        For Each listtmpBSCParaEveryBSC In listtmpBSCPara
                            If ((listtmpBSCParaEveryBSC(0).ToString = drtmpBscListRow(1)) And (listtmpBSCParaEveryBSC(1) IsNot Nothing)) Then
                                intNumberOfFN = 0
                                For intI = 1 To listtmpBSCParaEveryBSC.Count - 1
                                    intNumberOfFN = intNumberOfFN + listtmpBSCParaEveryBSC(intI).ToString.Count(Function(x) x = " ")
                                Next
                                drtmpBscListRow(strWhichPara) = intNumberOfFN
                                listtmpBSCPara.Remove(listtmpBSCParaEveryBSC)
                                Exit For
                            End If
                        Next
                    Else
                        For Each listtmpBSCParaEveryBSC In listtmpBSCPara
                            If ((listtmpBSCParaEveryBSC(0).ToString = drtmpBscListRow(1)) And (listtmpBSCParaEveryBSC(1) IsNot Nothing) And (listtmpBSCParaEveryBSC(1) IsNot DBNull.Value)) Then
                                Try
                                    drtmpBscListRow(strWhichPara) = CTypeDynamic(listtmpBSCParaEveryBSC(1), dtFormat.Columns(strWhichPara).DataType)
                                    listtmpBSCPara.Remove(listtmpBSCParaEveryBSC)
                                    Exit For
                                Catch ex As Exception
                                    drtmpBscListRow(strWhichPara) = DBNull.Value

                                    Exit For
                                End Try
                            End If
                        Next
                    End If
                Next
                dtData.Rows.Add(drtmpBscListRow)
            Next

            sqllSSLibrary.BlukInsert(strSQLServerTableName, dtData, CommonLibrary.GetSQLServerConnect("ConnectionBaseStationDetailsDB"))
            '----------------------
            dtFormatForLog = sqllSSLibrary.ReturnFormat("dt_GSMP_BSC_Daily_ChangeLog", CommonLibrary.GetSQLServerConnect("ConnectionLogDB"))
            dtFormatForLog.Rows.Clear()
            dtDataForLog = dtFormatForLog
            '----------------------



            For Each drtmpBscListRow In dtData.Rows


                For Each boptmpBSCPara In listOriPara
                    If boptmpBSCPara.strBSCName = drtmpBscListRow(0).ToString Then
                        drtmpBSCCompareOf = boptmpBSCPara.drParaRow
                        Exit For
                    End If
                Next
                If drtmpBSCCompareOf IsNot Nothing Then
                    For intI = 1 To dtData.Columns.Count - 1

                        If ((drtmpBSCCompareOf(intI) IsNot Nothing) And (drtmpBscListRow(intI) IsNot Nothing) And (drtmpBSCCompareOf(intI) IsNot DBNull.Value) And (drtmpBscListRow(intI) IsNot DBNull.Value)) Then

                            If drtmpBSCCompareOf(intI) <> drtmpBscListRow(intI) Then
                                '-----------记录修改问题
                                'ChangeCellParaLog(strSQLServerTableName, drtmpBscListRow(0).ToString, dtFormat.Columns(intI).ColumnName, drtmpBSCCompareOf(intI) & "  -->  " & drtmpBscListRow(intI), dateWhatTimeIsPara)

                                drtmpBscListRowForLog = dtFormatForLog.NewRow
                                drtmpBscListRowForLog(0) = strSQLServerTableName
                                drtmpBscListRowForLog(1) = drtmpBscListRow(0).ToString
                                drtmpBscListRowForLog(2) = dtFormat.Columns(intI).ColumnName
                                drtmpBscListRowForLog(3) = drtmpBSCCompareOf(intI) & "  -->  " & drtmpBscListRow(intI)
                                drtmpBscListRowForLog(4) = dateWhatTimeIsPara
                                drtmpBscListRowForLog(5) = drtmpBSCCompareOf(intI)
                                drtmpBscListRowForLog(6) = drtmpBscListRow(intI)
                                dtDataForLog.Rows.Add(drtmpBscListRowForLog)
                            End If
                        ElseIf ((drtmpBscListRow(intI) IsNot Nothing) And (drtmpBscListRow(intI) IsNot DBNull.Value) And ((drtmpBSCCompareOf(intI) Is Nothing) Or (drtmpBSCCompareOf(intI) Is DBNull.Value))) Then
                            'ChangeCellParaLog(strSQLServerTableName, drtmpBscListRow(0).ToString, dtFormat.Columns(intI).ColumnName, "前期缺数，现网值为 -->  " & drtmpBscListRow(intI), dateWhatTimeIsPara)


                            drtmpBscListRowForLog = dtFormatForLog.NewRow
                            drtmpBscListRowForLog(0) = strSQLServerTableName
                            drtmpBscListRowForLog(1) = drtmpBscListRow(0).ToString
                            drtmpBscListRowForLog(2) = dtFormat.Columns(intI).ColumnName
                            drtmpBscListRowForLog(3) = "前期缺数，现网值为 -->  " & drtmpBscListRow(intI)
                            drtmpBscListRowForLog(4) = dateWhatTimeIsPara
                            drtmpBscListRowForLog(5) = ""
                            drtmpBscListRowForLog(6) = drtmpBscListRow(intI)

                            dtDataForLog.Rows.Add(drtmpBscListRowForLog)


                            '-------------现网缺数不记录-------------
                            'ElseIf ((drtmpBSCCompareOf(intI) IsNot Nothing) And (drtmpBSCCompareOf(intI) IsNot DBNull.Value) And ((drtmpBscListRow(intI) Is Nothing) Or (drtmpBscListRow(intI) Is DBNull.Value))) Then
                            '    'ChangeCellParaLog(strSQLServerTableName, drtmpBscListRow(0).ToString, dtFormat.Columns(intI).ColumnName, "现网缺数", dateWhatTimeIsPara)


                            '    drtmpBscListRowForLog = dtFormatForLog.NewRow
                            '    drtmpBscListRowForLog(0) = strSQLServerTableName
                            '    drtmpBscListRowForLog(1) = drtmpBscListRow(0).ToString
                            '    drtmpBscListRowForLog(2) = dtFormat.Columns(intI).ColumnName
                            '    drtmpBscListRowForLog(3) = "现网缺数"
                            '    drtmpBscListRowForLog(4) = dateWhatTimeIsPara
                            '    dtDataForLog.Rows.Add(drtmpBscListRowForLog)



                        End If
                    Next

                    drtmpBSCCompareOf = Nothing
                    '-------------前期缺数不记录-------------
                    'Else
                    '    'ChangeCellParaLog(strSQLServerTableName, drtmpBscListRow(0).ToString, "", "该网元前期缺数", dateWhatTimeIsPara)

                    '    drtmpBscListRowForLog = dtFormatForLog.NewRow
                    '    drtmpBscListRowForLog(0) = strSQLServerTableName
                    '    drtmpBscListRowForLog(1) = drtmpBscListRow(0).ToString
                    '    drtmpBscListRowForLog(2) = ""
                    '    drtmpBscListRowForLog(3) = "该网元前期缺数"
                    '    drtmpBscListRowForLog(4) = dateWhatTimeIsPara
                    '    dtDataForLog.Rows.Add(drtmpBscListRowForLog)


                End If
            Next


            '----------------------
            sqllSSLibrary.BlukInsert("dt_GSMP_BSC_Daily_ChangeLog", dtDataForLog, CommonLibrary.GetSQLServerConnect("ConnectionLogDB"))
            '----------------------


            pasTmpParaAndSQL = Nothing
            tblTableBscList = Nothing
            dtFormat.Dispose()
            dtFormat = Nothing
            dtData.Dispose()
            dtData = Nothing
            If dttmpData IsNot Nothing Then dttmpData.Dispose()
            dttmpData = Nothing
            If odbcOleDBCommand IsNot Nothing Then odbcOleDBCommand.Dispose()
            odbcOleDBCommand = Nothing
            aceAccess.Dispose()
            aceAccess = Nothing
            drtmpBscListRow = Nothing
            If listBscList.Count > 0 Then listBscList.Clear()
            listBscList = Nothing
            If listBSCPara.Count > 0 Then listBSCPara.Clear()
            listBSCPara = Nothing
            If listtmpBSCPara.Count > 0 Then listtmpBSCPara.Clear()
            listtmpBSCPara = Nothing
            If listtmpBSCParaEveryBSC.Count > 0 Then listtmpBSCParaEveryBSC.Clear()
            listtmpBSCParaEveryBSC = Nothing
            scmdCommand.Dispose()
            scmdCommand = Nothing
            dtOrgData.Dispose()
            dtOrgData = Nothing
            If listOriPara.Count > 0 Then listOriPara.Clear()
            listOriPara = Nothing
            boptmpBSCPara = Nothing
            drtmpBSCCompareOf = Nothing
            sabConfigSQLSandBSCList.Dispose()
            sabConfigSQLSandBSCList = Nothing
            dtCellID.Dispose()
            dtCellID = Nothing
            If listdCellID.Count > 0 Then listdCellID.Clear()



        Catch ex As Exception
            Throw New Exception(ex.Message, ex)
        End Try
        Return 88
    End Function

    Public Function ReturnCellOnlyID(ByRef listdCellID As Dictionary(Of String, String), ByRef strCellName As String) As String
        If listdCellID.ContainsKey(strCellName) Then
            Return listdCellID(strCellName)
        Else
            Return strCellName
        End If

    End Function



    Public Sub ChangeCellParaLog(strTableName As String, strNetElement As String, strChangePara As String, strChangeValue As String, dateWhatTimeChange As Date)
        Dim scmdCMD As SqlCommand
        Dim spTableName As SqlParameter
        Dim spNetElement As SqlParameter
        Dim spChangePara As SqlParameter
        Dim spChangeValue As SqlParameter
        Dim spWhatTimeChange As SqlParameter
        Try
            scmdCMD = sqllSSLibrary.GetCommandProc("proc_ChangeBSCPara", CommonLibrary.GetSQLServerConnect("ConnectionLogDB"))
            spTableName = New SqlParameter("@ChangeTable", SqlDbType.VarChar, 100)
            spNetElement = New SqlParameter("@ChangeNE", SqlDbType.VarChar, 100)
            spChangePara = New SqlParameter("@ChangePara", SqlDbType.VarChar, 100)
            spChangeValue = New SqlParameter("@ChangeValue", SqlDbType.Text)
            spWhatTimeChange = New SqlParameter("@ChangeDate", SqlDbType.DateTime)
            spTableName.Value = strTableName
            spNetElement.Value = strNetElement
            spChangePara.Value = strChangePara
            spChangeValue.Value = strChangeValue
            spWhatTimeChange.Value = dateWhatTimeChange
            scmdCMD.Parameters.Add(spTableName)
            scmdCMD.Parameters.Add(spNetElement)
            scmdCMD.Parameters.Add(spChangePara)
            scmdCMD.Parameters.Add(spChangeValue)
            scmdCMD.Parameters.Add(spWhatTimeChange)
            sqllSSLibrary.ExecNonQuery(scmdCMD)
        Catch ex As Exception
            Throw New Exception(ex.Message, ex)
            scmdCMD.Dispose()
            scmdCMD = Nothing
        End Try
    End Sub

End Class
