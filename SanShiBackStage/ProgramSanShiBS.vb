Imports System.Windows.Forms


Module ProgramSanShiBS

    ''' <summary>
    ''' 定义一个定时控件
    ''' </summary>
    Private WithEvents timerToRun As System.Windows.Forms.Timer


    ''' <summary>
    ''' 定义临时定时布尔变量
    ''' </summary>
    Private bolIsTime As Boolean
    ''' <summary>
    ''' 定义是周几运行的布尔变量
    ''' </summary>
    Private bolIsWeedDay As Boolean


    ''' <summary>
    ''' 定义整体配置对象
    ''' </summary>
    Private mcMainConfig As MainConfig
    ''' <summary>
    ''' 定义整体配置窗体
    ''' </summary>
    Private WithEvents formMainConfig As MainConfigForm = Nothing



    ''' <summary>
    ''' 定义一个配置入数过程窗体
    ''' </summary>
    Private WithEvents formInsertDataBaseConfig As BaseSationDetailsInsertConfigForm = Nothing


    ''' <summary>
    ''' 定义临时的目录对象,用来加入托管控件中的
    ''' </summary>
    Private WithEvents mitemOpen As MenuItem

    ''' <summary>
    '''定义一个服务栏弹出托管控件 
    ''' </summary>
    Private WithEvents icnPop As System.Windows.Forms.NotifyIcon



    ''' <summary>
    ''' 定义运行对象入数过程运行窗体
    ''' </summary>
    Private WithEvents formRun As RunConsole = Nothing


    Sub Main()
        '创建新的托管图标
        Dim menMenu As New ContextMenu()


        Try

            '初始化定时布尔变量
            bolIsTime = False
            bolIsWeedDay = False
            '如果已有程序运行即退出程序
            If Process.GetProcessesByName(Process.GetCurrentProcess.ProcessName).Length > 1 Then
                Console.WriteLine("Not the First!")
                End
            Else
                Console.WriteLine("First!")
            End If


            '创建任务栏图标
            icnPop = New System.Windows.Forms.NotifyIcon()


            '定义托管图标的目录以及触发事件,以下为配置输入过程
            mitemOpen = New MenuItem("MainConfig", New EventHandler(AddressOf SetMainConfig))
            menMenu.MenuItems.Add(mitemOpen)

            menMenu.MenuItems.Add("-")



            '定义托管图标的目录以及触发事件,以下为配置输入过程
            mitemOpen = New MenuItem("BaseSationDetailsInsertConfig", New EventHandler(AddressOf SetBSDetailsInsertConfig))
            menMenu.MenuItems.Add(mitemOpen)


            mitemOpen = New MenuItem("Run", New EventHandler(AddressOf SetRunConsoleGo))
            menMenu.MenuItems.Add(mitemOpen)


            menMenu.MenuItems.Add("-")




            '定义退出事件
            mitemOpen = New MenuItem("Exit", New EventHandler(AddressOf ExitMenu))
            menMenu.MenuItems.Add(mitemOpen)


            icnPop.ContextMenu = menMenu
            icnPop.Icon = My.Resources.ResourceALL.Clone_CD
            icnPop.Text = "小白"
            icnPop.Visible = True

            '创建Timer控件
            timerToRun = New System.Windows.Forms.Timer()
            timerToRun.Interval = 60000
            timerToRun.Enabled = True


            mcMainConfig = MainConfig.Load

            Application.EnableVisualStyles()
            Application.Run()
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try


    End Sub





    Private Sub SetMainConfig()
        Try

            Console.WriteLine("崔秉龙好靓仔")
            'If formRun Is Nothing Then

            If formMainConfig Is Nothing Then
                formMainConfig = New MainConfigForm(mcMainConfig)
                formMainConfig.Show()


                AddHandler formMainConfig.FormClosed, AddressOf CloseMainConfigForm


            Else
                formMainConfig.Activate()
            End If
            'Else
            '    MessageBox.Show("正在运行啊!")
            'End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try


    End Sub

    Private Sub SetBSDetailsInsertConfig()
        Console.WriteLine("崔秉龙好靓仔")
        'If formRun Is Nothing Then

        Try


            If formInsertDataBaseConfig Is Nothing Then
                formInsertDataBaseConfig = New BaseSationDetailsInsertConfigForm()
                formInsertDataBaseConfig.Show()


                AddHandler formInsertDataBaseConfig.FormClosed, AddressOf CloseInsertDataBaseConfigForm


            Else
                formInsertDataBaseConfig.Activate()
            End If
            'Else
            '    MessageBox.Show("正在运行啊!")
            'End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub



    Private Sub SetRunConsoleGo()
        TimeToRun(False, True)
    End Sub



    Private Sub TimeToRun(ByRef bolIsBackStage As Boolean, ByRef bolIsWeekDay As Boolean)
        Console.WriteLine("崔秉龙好靓仔")
        'If formRun Is Nothing Then
        Try

            If formRun Is Nothing Then
                formRun = New RunConsole(bolIsBackStage, bolIsWeekDay)

                If Not bolIsBackStage Then
                    formRun.Show()
                Else
                    formRun.btnGo_Click(Nothing, Nothing)
                End If

                icnPop.ContextMenu.MenuItems(0).Enabled = False
                icnPop.ContextMenu.MenuItems(2).Enabled = False
                icnPop.ContextMenu.MenuItems(5).Enabled = False


                AddHandler formRun.FormClosed, AddressOf CloseRunForm
            Else
                formRun.Visible = True
                formRun.Activate()
            End If
            'Else
            '    MessageBox.Show("正在运行啊!")
            'End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub

    ''' <summary>
    ''' 当配置文件关闭时必须清空
    ''' </summary>
    Private Sub CloseRunForm()
        formRun = Nothing
        icnPop.ContextMenu.MenuItems(0).Enabled = True
        icnPop.ContextMenu.MenuItems(2).Enabled = True
        icnPop.ContextMenu.MenuItems(5).Enabled = True

    End Sub



    ''' <summary>
    ''' 当配置文件关闭时必须清空
    ''' </summary>
    Private Sub CloseInsertDataBaseConfigForm()
        Try

            formInsertDataBaseConfig = Nothing
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub



    Private Sub CloseMainConfigForm()
        formMainConfig = Nothing
        'If mcMainConfig.bolIsAutoRun Then
        '    icnPop.Icon = My.Resources.ResourceAll.Ready
        'Else
        '    icnPop.Icon = My.Resources.ResourceAll._Stop
        'End If

    End Sub




    ''' <summary>
    ''' 退出程序
    ''' </summary>
    Private Sub ExitMenu()
        icnPop.Visible = False
        icnPop.Dispose()
        icnPop = Nothing
        timerToRun.Dispose()
        timerToRun = Nothing
        mcMainConfig.Dispose()
        mcMainConfig = Nothing
        Application.Exit()
    End Sub

    Private Sub timerToRun_Tick(sender As Object, e As EventArgs) Handles timerToRun.Tick
        Dim bolIsWeekDayNow As Boolean
        Console.WriteLine("崔秉龙好靓仔")
        '手动定义在1点的时候运行
        Try

            bolIsWeekDayNow = False
            If Now.Hour = mcMainConfig.intWhatTimeToRun Then
                '定义一个小时内只运行一次
                If bolIsTime = False And mcMainConfig.bolIsAutoRun Then

                    bolIsTime = True


                    If Now.DayOfWeek = mcMainConfig.intWhichWeedDayToRun Then
                        If bolIsWeedDay = False Then
                            bolIsWeedDay = True

                            bolIsWeekDayNow = True



                        End If
                    Else
                        bolIsWeedDay = False

                    End If
                    TimeToRun(True, bolIsWeekDayNow)

                    ''触发入数过程
                    'If (formInsertDataBaseConfig Is Nothing) Or (formMainConfig Is Nothing) Then
                    '    If formRun Is Nothing Then
                    '        formRun = New RunConsole(cdInsertDataBaseConfigData, True, mcMainConfig.bolIsAutoRun, mcMainConfig.intHowManyDaysAgo)
                    '        formRun.Show()
                    '        icnPop.Icon = My.Resources.ResourceALL.Running

                    '        formRun.btnGo_Click(Nothing, Nothing)
                    '        AddHandler formRun.FormClosed, AddressOf CloseRunForm
                    '    Else
                    '        formRun.Activate()

                    '    End If
                    'Else
                    '    MessageBox.Show("正在配置啊!!")
                    'End If

                End If


            Else
                bolIsTime = False

            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub
End Module
