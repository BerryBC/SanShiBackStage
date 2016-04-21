Imports SimpleJson.SimpleJson
Imports System.IO



Public Class MainConfig
    Implements IDisposable
    Private Shared strConfigFile As String = "MainConfig.json"


    ''' <summary>
    ''' 运行多少天前的数据
    ''' </summary>
    Public intHowManyDaysAgo As Integer
    ''' <summary>
    ''' 几点开始运行
    ''' </summary>
    Public intWhatTimeToRun As Integer

    ''' <summary>
    ''' 周几去入基础数据表
    ''' </summary>
    Public intWhichWeedDayToRun As Integer
    ''' <summary>
    ''' 是否自动运行?
    ''' </summary>
    Public bolIsAutoRun As Boolean
    ''' <summary>
    ''' 是否自动启动入数
    ''' </summary>
    Public bolRunWhenStart As Boolean


    Public Shared Sub Save(sfSaveConfigFile As MainConfig)
        Dim strPathConfig As String
        Dim swSaveStream As StreamWriter
        Dim strSaveJson As String

        Try
            If Not System.IO.Directory.Exists(AppDomain.CurrentDomain.BaseDirectory & "config\") Then
                My.Computer.FileSystem.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory & "config\")
            End If

            strPathConfig = AppDomain.CurrentDomain.BaseDirectory & "config\" & strConfigFile
            swSaveStream = New StreamWriter(File.Open(strPathConfig, FileMode.Create))
            strSaveJson = SimpleJson.SimpleJson.SerializeObject(sfSaveConfigFile)
            swSaveStream.Write(strSaveJson)
            swSaveStream.Flush()
            swSaveStream.Close()
            swSaveStream = Nothing
        Catch ex As Exception
            Console.WriteLine(ex)
        End Try
    End Sub



    Public Shared Function Load() As MainConfig
        Dim cdLoadMainConfigFile As MainConfig
        Dim strJsonLoad As String

        Try
            strJsonLoad = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory & "config\" & strConfigFile)
            cdLoadMainConfigFile = SimpleJson.SimpleJson.DeserializeObject(Of MainConfig)(strJsonLoad)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
            Return New MainConfig
        End Try
        Return cdLoadMainConfigFile
    End Function

    Public Sub New()
        intHowManyDaysAgo = 1
        intWhatTimeToRun = 1
        intWhichWeedDayToRun = 5
        bolIsAutoRun = False
        bolRunWhenStart = False
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
