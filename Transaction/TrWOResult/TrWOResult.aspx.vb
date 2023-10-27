Imports System.Data
Imports System.Data.SqlClient
Imports System.Runtime.Serialization

Partial Class Transaction_TrWOResult_TrWOResult
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    'Protected GetStringHd As String = "SELECT DISTINCT TransNmbr, TransDate, Status, CheckBy, AcknowledBy, WorkBy, WrhsCode, WrhsFgSubLed, FgBorongan, Remark, UserPrep, DatePrep, UserAppr, " + _
    '                "DateAppr, Division, DivisionName, Supplier, FgProcess, WrhsName, WorkByName, AcknowledByName, CheckByName, SupplierName FROM V_PLWOResultHd"

    Private Function GetStringHd() As String
        Return "SELECT DISTINCT A.TransNmbr, A.TransDate, A.Status, A.CheckBy, A.AcknowledBy, A.WorkBy, A.WrhsCode, A.WrhsFgSubLed, A.FgBorongan, A.Remark, A.UserPrep, A.DatePrep, A.UserAppr, A.DateAppr, A.Division, A.DivisionName, A.Supplier, A.FgProcess, A.WrhsName, A.WorkByName, A.AcknowledByName, A.CheckByName, A.SupplierName FROM V_PLWOResultHd A INNER JOIN V_SAUserDivision B ON A.Division = B.Division WHERE B.UserId  = " + QuotedStr(ViewState("UserId").ToString) '+ " AND POType <> 'Service' "
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                SetInit()

                Session("AdvanceFilter") = ""
                ' lbCount.Text = SQLExecuteScalar("SELECT COUNT(Reference) FROM V_PLWOResultGetReffDt ", ViewState("DBConnection").ToString)
                lbCount.Text = SQLExecuteScalar("EXEC S_PLWoCount '" + ViewState("UserId") + "','','' ", ViewState("DBConnection").ToString)
            End If

            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnMandor" Then
                    BindToText(tbMandor, Session("Result")(0).ToString)
                    BindToText(tbMandorName, Session("Result")(1).ToString)
                End If
                If ViewState("Sender") = "btnAsisten" Then
                    BindToText(tbAsisten, Session("Result")(0).ToString)
                    BindToText(tbAsistenName, Session("Result")(1).ToString)
                End If
                If ViewState("Sender") = "btnMaterial" Then
                    BindToText(tbMaterial, Session("Result")(0).ToString)
                    BindToText(tbMaterialName, Session("Result")(1).ToString)
                    BindToText(tbQtyMaterial, Session("Result")(2).ToString)
                    BindToText(tbUnit, Session("Result")(3).ToString)
                End If
                If ViewState("Sender") = "btnKontraktor" Then
                    BindToText(tbKontraktor, Session("Result")(0).ToString)
                    BindToText(tbKontraktorName, Session("Result")(1).ToString)
                End If
                If ViewState("Sender") = "btnMachine" Then
                    BindToText(tbMachine, Session("Result")(0).ToString)
                    BindToText(tbMachineName, Session("Result")(1).ToString)
                End If


                If ViewState("Sender") = "btnOperator" Then
                    BindToText(tbOperator, Session("Result")(0).ToString)
                    BindToText(tbOperatorName, Session("Result")(1).ToString)
                    BindToText(tbOpBorongan, Session("Result")(0).ToString)
                End If


                If ViewState("Sender") = "btnOp" Then
                    BindToText(tbOp, Session("Result")(0).ToString)
                    BindToText(tbOpName, Session("Result")(1).ToString)
                End If

                If ViewState("Sender") = "btnGetDt" Then
                    Dim drResult As DataRow
                    Dim ExistRow As DataRow()
                    Dim Divisi, Type As String
                    For Each drResult In Session("Result").Rows
                        Type = drResult("Type").ToString.Trim
                        Divisi = drResult("Divisi_Block").ToString
                        ExistRow = ViewState("Dt").Select("DivisiBlok = " + QuotedStr(Divisi) + " AND Type = " + QuotedStr(Type))
                        If ExistRow.Count = 0 Then
                            Dim dr As DataRow
                            dr = ViewState("Dt").NewRow
                            dr("WONo") = drResult("Reference").ToString
                            dr("Type") = drResult("Type").ToString
                            dr("JobPlant") = drResult("Job").ToString
                            dr("Job_Name") = drResult("Job_Name").ToString
                            dr("HK") = 0
                            'dr("WorkBy") = drResult("WorkBy").ToString
                            'dr("WorkByName") = drResult("WorkBy_Name").ToString
                            'dr("Borongan") = drResult("FgBorongan").ToString
                            dr("DivisiBlok") = drResult("Divisi_Block").ToString
                            dr("DivisiBlokName") = drResult("Divisi_Block_Name").ToString
                            dr("QtyWO") = drResult("QtyWO").ToString
                            dr("QtyDone") = drResult("QtyDone").ToString
                            dr("Qty") = drResult("Qty").ToString
                            dr("QtySDHI") = drResult("QtySDHI").ToString
                            dr("Unit") = drResult("Unit").ToString
                            dr("Capacity") = drResult("NormaHK").ToString
                            dr("HKNorma") = 0 'drResult("NormaHK").ToString
                            dr("TKTotal") = drResult("TKTotal").ToString ', ViewState("DigitQty"))
                            dr("TKDone") = FormatFloat(drResult("TKDone").ToString, ViewState("DigitQty"))
                            dr("Person") = FormatFloat(drResult("Person").ToString, ViewState("DigitQty"))
                            dr("TKSDHI") = FormatFloat(drResult("TKSDHI").ToString, ViewState("DigitQty"))
                            If dr("TKSDHI") = 0 Then dr("TKSDHI") = 1
                            dr("HariTotal") = FormatFloat(drResult("HariTotal").ToString, ViewState("DigitQty"))
                            dr("HariDone") = FormatFloat(drResult("HariDone").ToString, ViewState("DigitQty"))
                            'dr("Mandor") = drResult("Mandor").ToString
                            'dr("MandorName") = drResult("Mandor_Name").ToString
                            'dr("Asisten") = drResult("Asisten").ToString
                            'dr("AsistenName") = drResult("Asisten_Name").ToString
                            If dr("Qty") >= (dr("QtyWO") - dr("QtyDone") - 0.01) Then
                                dr("FgFinish") = "Y"
                            Else
                                dr("FgFinish") = "N"
                            End If
                            'GetMachine(tbJob.Text, tbWONO.Text, dr("DivisiBlok").ToString, drResult("Type").ToString)
                            'GetMaterial(ddlReffType.SelectedValue, tbWONO.Text, tbJob.Text, dr("DivisiBlok").ToString, tbRotasi.Text, drResult("Type").ToString, drResult("PlantPeriodCode").ToString, dr("Qty"), dr("Pokok"))
                            ViewState("Dt").Rows.Add(dr)
                        End If

                    Next
                    BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    GridDt.Columns(1).Visible = GetCountRecord(ViewState("Dt")) > 0
                    'Session("ResultSame") = Nothing
                End If



                If ViewState("Sender") = "btnOut" Then
                    BtnAdd_Click(Nothing, Nothing)
                    Dim drResult As DataRow
                    Dim ExistRow As DataRow()
                    Dim FirstTime As Boolean = True
                    Dim Divisi, Type As String
                    For Each drResult In Session("Result").Rows
                        'insert
                        If FirstTime Then

                            Divisi = drResult("Divisi").ToString
                            'BindToText(tbJob, drResult("JobPlant").ToString)
                            BindToDropList(ddlDivisi, drResult("Divisi").ToString)
                            BindToDropList(ddlFgBorongan, drResult("FgBorongan").ToString)
                            ddlFgBorongan.Enabled = False
                            BindToDropList(ddlWorkBy, drResult("WorkBy").ToString)
                            BindToText(tbKontraktor, drResult("Supplier").ToString)
                            BindToText(tbKontraktorName, drResult("Supplier_Name").ToString)
                        End If
                        Type = drResult("Type").ToString.Trim
                        Divisi = drResult("Divisi_Block").ToString
                        ExistRow = ViewState("Dt").Select("DivisiBlok = " + QuotedStr(Divisi) + " AND Type = " + QuotedStr(Type))
                        If ExistRow.Count = 0 Then
                            Dim dr As DataRow
                            dr = ViewState("Dt").NewRow
                            dr("WONo") = drResult("Reference").ToString
                            dr("Type") = drResult("Type").ToString
                            dr("JobPlant") = drResult("Job").ToString
                            dr("Job_Name") = drResult("Job_Name").ToString
                            dr("HK") = 0
                            'dr("WorkByName") = drResult("WorkBy_Name").ToString
                            'dr("Borongan") = drResult("FgBorongan").ToString
                            dr("DivisiBlok") = drResult("Divisi_Block").ToString
                            dr("DivisiBlokName") = drResult("Divisi_Block_Name").ToString
                            dr("QtyWO") = drResult("QtyWO").ToString
                            dr("QtyDone") = drResult("QtyDone").ToString
                            dr("Qty") = drResult("Qty").ToString
                            dr("QtySDHI") = drResult("QtySDHI").ToString
                            dr("Unit") = drResult("Unit").ToString
                            dr("Capacity") = drResult("NormaHK").ToString
                            dr("HKNorma") = 0 'drResult("NormaHK").ToString
                            dr("TKTotal") = drResult("TKTotal").ToString ', ViewState("DigitQty"))
                            dr("TKDone") = FormatFloat(drResult("TKDone").ToString, ViewState("DigitQty"))
                            dr("Person") = FormatFloat(drResult("Person").ToString, ViewState("DigitQty"))
                            dr("TKSDHI") = FormatFloat(drResult("TKSDHI").ToString, ViewState("DigitQty"))
                            If dr("TKSDHI") = 0 Then dr("TKSDHI") = 1
                            dr("HariTotal") = FormatFloat(drResult("HariTotal").ToString, ViewState("DigitQty"))
                            dr("HariDone") = FormatFloat(drResult("HariDone").ToString, ViewState("DigitQty"))
                            'dr("Mandor") = drResult("Mandor").ToString
                            'dr("MandorName") = drResult("Mandor_Name").ToString 
                            'dr("Asisten") = drResult("Asisten").ToString
                            'dr("AsistenName") = drResult("Asisten_Name").ToString
                            dr("StartGawang") = "0"
                            dr("EndGawang") = "0"
                            If dr("Qty") >= (dr("QtyWO") - dr("QtyDone") - 0.01) Then
                                dr("FgFinish") = "Y"
                            Else
                                dr("FgFinish") = "N"
                            End If
                            'GetMachine(tbJob.Text, tbWONO.Text, dr("DivisiBlok").ToString, drResult("Type").ToString)
                            'GetMaterial(ddlReffType.SelectedValue, tbWONO.Text, tbJob.Text, dr("DivisiBlok").ToString, tbRotasi.Text, drResult("Type").ToString, drResult("PlantPeriodCode").ToString, dr("Qty"), dr("Pokok"))
                            ViewState("Dt").Rows.Add(dr)
                        End If

                    Next
                    BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    GridDt.Columns(1).Visible = GetCountRecord(ViewState("Dt")) > 0
                    '    'Session("ResultSame") = Nothing
                End If

               

                Session("filter") = Nothing
                Session("Column") = Nothing
                Session("Result") = Nothing
            End If
            'dsUseRollNo.ConnectionString = ViewState("DBConnection")
            lbStatus.Text = ""
        Catch ex As Exception
            lbStatus.Text = "Page Load Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub InitProperty()
        ViewState("DBConnection") = Session(Request.QueryString("KeyId"))("DBConnection")
        ViewState("UserId") = Session(Request.QueryString("KeyId"))("UserId")
        ViewState("UserName") = Session(Request.QueryString("KeyId"))("UserName")
        ViewState("FgAdmin") = Session(Request.QueryString("KeyId"))("FgAdmin")
        ViewState("Currency") = Session(Request.QueryString("KeyId"))("Currency")
        ViewState("GLYear") = Session(Request.QueryString("KeyId"))("Year")
        ViewState("GLPeriod") = Session(Request.QueryString("KeyId"))("Period")
        ViewState("GLPeriodName") = Session(Request.QueryString("KeyId"))("PeriodName")
        ViewState("CompanyName") = Session(Request.QueryString("KeyId"))("CompanyName")
        ViewState("Address1") = Session(Request.QueryString("KeyId"))("Address1")
        ViewState("Address2") = Session(Request.QueryString("KeyId"))("Address2")
        ViewState("PageSizeGrid") = Session(Request.QueryString("KeyId"))("PageSizeGrid")
        ViewState("1Payment") = Session(Request.QueryString("KeyId"))("1Payment")
        ViewState("DigitRate") = Session(Request.QueryString("KeyId"))("DigitRate")
        ViewState("DigitQty") = Session(Request.QueryString("KeyId"))("DigitQty")
        ViewState("DigitHome") = Session(Request.QueryString("KeyId"))("DigitHome")
        ViewState("DigitPercent") = Session(Request.QueryString("KeyId"))("DigitPercent")
        ViewState("ServerDate") = Session(Request.QueryString("KeyId"))("ServerDate")
    End Sub

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        Dim index As Integer
        Try
            index = Int32.Parse(e.Item.Value)
            MultiView1.ActiveViewIndex = index
            If MultiView1.ActiveViewIndex = 1 Then
                pnlDt2.Visible = True
                pnlEditDt2.Visible = False
                GridDt2.Columns(0).Visible = GetCountRecord(ViewState("Dt2")) > 0

            ElseIf MultiView1.ActiveViewIndex = 2 Then
                pnlDt3.Visible = True
                pnlEditDt3.Visible = False
                GridDt3.Columns(0).Visible = GetCountRecord(ViewState("Dt3")) > 0

                'BindDataDt3(ViewState("TransNmbr"))
            ElseIf MultiView1.ActiveViewIndex = 3 Then
                PnlDt4.Visible = True
                pnlEditDt4.Visible = False
                GridDt4.Columns(0).Visible = GetCountRecord(ViewState("Dt4")) > 0
                'BindDataDt4(ViewState("TransNmbr"))
            Else
                pnlDt.Visible = True
                pnlEditDt.Visible = False
            End If
            btnSaveTrans.Focus()
        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub

    Private Sub SetInit()
        Try
            FillRange(ddlRange)
            FillCombo(ddlDivisi, "EXEC S_GetPlantDivisionAll " + QuotedStr(ViewState("UserId").ToString), True, "DivisionCode", "DivisionName", ViewState("DBConnection"))
            FillCombo(ddlWrhs, " SELECT WrhsCode, WrhsName FROM MsWarehouse  Where WrhsType in ('OWNER', 'TITIP OUT') ", True, "WrhsCode", "WrhsName", ViewState("DBConnection"))
            FillCombo(ddlWorkBy, "SELECT Team_Code, Team_Name FROM V_MsTeam WHERE FgPanen='N'", True, "Team_Code", "Team_Name", ViewState("DBConnection"))
            'FillCombo(ddlFgBorongan, " SELECT WrhsCode, WrhsName FROM MsWarehouse  Where WrhsType in ('OWNER', 'TITIP OUT') ", True, "WrhsCode", "WrhsName", ViewState("DBConnection"))

            ViewState("SortExpression") = Nothing
            ViewState("SortExpressionOut") = Nothing
            ViewState("SortExpressionUse") = Nothing
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))

            tbQty.Attributes.Add("OnKeyDown", "return PressNumeric()")
            tbQtyCapacity.Attributes.Add("OnKeyDown", "return PressNumeric()")
            tbHariWO.Attributes.Add("OnKeyDown", "return PressNumeric()")
            tbHariDone.Attributes.Add("OnKeyDown", "return PressNumeric()")
            tbHK.Attributes.Add("OnKeyDown", "return PressNumeric()")
            'tbQtyBlok.Attributes.Add("OnBlur", "setformatdt()")
            'tbQtyTotal.Attributes.Add("ReadOnly", "True")
            'tbQtyTotal.Attributes.Add("OnBlur", "setformatdt()")
            'tbQtyE.Attributes.Add("OnKeyDown", "return PressNumeric()")
            'tbQtyWeek.Attributes.Add("OnKeyDown", "return PressNumeric()")
            'tbQtyWeek.Attributes.Add("OnBlur", "setformatdt()")



        Catch ex As Exception
            Throw New Exception("Set Init Error : " + ex.ToString)
        End Try
    End Sub
    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim SQLString, StrFilter As String
        Try
            StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue, ddlRange.SelectedValue)
            'If AdvanceFilter.Length > 1 And StrFilter.Length > 1 Then
            '    StrFilter = StrFilter + " And " + AdvanceFilter
            'ElseIf AdvanceFilter.Length > 1 And StrFilter.Length <= 1 Then
            '    StrFilter = AdvanceFilter
            'End If
            If AdvanceFilter.Length > 1 Then
                StrFilter = AdvanceFilter
            End If
            SQLString = GetStringHd()
            DT = BindDataTransaction(SQLString, StrFilter, ViewState("DBConnection").ToString)
            If DT.Rows.Count = 0 Then
                lbStatus.Text = "No Data"
                pnlNav.Visible = False
                'ddlCommand.Visible = False
                'BtnGo.Visible = False
            End If
            ddlCommand.Visible = DT.Rows.Count > 0
            BtnGo.Visible = DT.Rows.Count > 0
            ddlCommand2.Visible = ddlCommand.Visible
            btnGo2.Visible = BtnGo.Visible
            btnAdd2.Visible = BtnGo.Visible
            DV = DT.DefaultView
            DV.Sort = ViewState("SortExpression")
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "TransDate DESC"
            End If
            GridView1.DataSource = DV
            GridView1.DataBind()

        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub
    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT * From V_PLWOResultDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function
    Private Function GetStringDt2(ByVal Nmbr As String) As String
        Return "SELECT * From V_PLWOResultMachine WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Private Function GetStringDt3(ByVal Nmbr As String) As String
        Return "SELECT * From V_PLWOResultEmp WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Private Function GetStringDt4(ByVal Nmbr As String) As String
        Return "SELECT * From V_PLWOResultRM WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        Try
            GridView1.PageIndex = 0
            Session("AdvanceFilter") = ""
            BindData(Session("AdvanceFilter"))
            pnlNav.Visible = True
            'ddlCommand.Visible = True
            'BtnGo.Visible = True
        Catch ex As Exception
            lbStatus.Text = "Btn Search Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub BtnGo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnGo.Click, btnGo2.Click
        Dim Status As String
        Dim Result, ListSelectNmbr, ActionValue As String
        Dim Nmbr(100) As String
        Dim j As Integer
        Try
            If sender.ID.ToString = "BtnGo" Then
                ActionValue = ddlCommand.SelectedValue
            Else
                ActionValue = ddlCommand2.SelectedValue
            End If

            Status = CekStatus(ActionValue)

            ListSelectNmbr = ""
            GetListCommand(Status, GridView1, "3,2", ListSelectNmbr, Nmbr, lbStatus.Text)
            If ListSelectNmbr = "" Then Exit Sub
            For j = 0 To (Nmbr.Length - 1)
                If Nmbr(j) = "" Then
                    Exit For
                Else
                    Result = ExecSPCommandGo(ActionValue, "S_PLWOResult", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                    If Trim(Result) <> "" Then
                        lbStatus.Text = lbStatus.Text + Result + " <br/>"
                    End If
                End If
            Next
            BindData("TransNmbr in (" + ListSelectNmbr + ")")

        Catch ex As Exception
            lbStatus.Text = "Go Command Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnExpand_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExpand.Click
        Try
            tbfilter2.Text = ""
            If pnlSearch.Visible Then
                pnlSearch.Visible = False
            Else
                pnlSearch.Visible = True
            End If
        Catch ex As Exception
            lbStatus.Text = "btn Expand Error : " + ex.ToString
        End Try
    End Sub

    Private Sub EnableHd(ByVal State As Boolean)
        Try
            ddlDivisi.Enabled = State
            'btnGetDt.Visible = State

        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub EnableDt3(ByVal State As Boolean)
        Try
            'tbOp.Enabled = State
        Catch ex As Exception
            Throw New Exception("Enable Dt3 Error " + ex.ToString)
        End Try
    End Sub
    Private Sub EnableDt4(ByVal State As Boolean)
        Try
            tbMachine.Enabled = State
            'diremark tbStartTime.Enabled = State
        Catch ex As Exception
            Throw New Exception("Enable Dt4 Error " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDt(ByVal Nmbr As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt") = Nothing
            dt = SQLExecuteQuery(GetStringDt(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt") = dt
            BindGridDt(dt, GridDt)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub
    Private Sub BindDataDt2(ByVal Nmbr As String)
        Dim Drow As DataRow()
        Try
            Dim dt As New DataTable
            ViewState("Dt2") = Nothing
            dt = SQLExecuteQuery(GetStringDt2(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt2") = dt
            Drow = ViewState("Dt2").Select("WONo+'|'+Type+'|'+DivisiBlok=" + QuotedStr(tbWONO.Text + "|" + LblTypeM.Text + "|" + LblBatchM.Text))
            If Drow.Length > 0 Then
                BindGridDt(Drow.CopyToDataTable, GridDt2)
                GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "ViewA"
            Else
                Dim DtTemp As DataTable
                DtTemp = ViewState("Dt2").Clone
                DtTemp.Rows.Add(DtTemp.NewRow())
                GridDt2.DataSource = DtTemp
                GridDt2.DataBind()
                GridDt2.Columns(0).Visible = False
            End If
        Catch ex As Exception
            Throw New Exception("Bind Data Dt2 Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDt3(ByVal Nmbr As String)
        Dim Drow As DataRow()
        Try
            Dim dt As New DataTable
            ViewState("Dt3") = Nothing
            dt = SQLExecuteQuery(GetStringDt3(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt3") = dt
            BindGridDt(dt, GridDt3)
            Drow = ViewState("Dt3").Select("WONo+'|'+Type+'|'+DivisiBlok=" + QuotedStr(LblTypeE.Text + "|" + LblBatchE.Text + "|" + LblBatchNameE.Text))
            If Drow.Length > 0 Then
                BindGridDt(Drow.CopyToDataTable, GridDt3)
                GridDt3.Columns(0).Visible = Not ViewState("StateHd") = "ViewE"
            Else
                If ddlFgBorongan.SelectedValue = "Y" Then
                    Dim DtTemp As DataTable
                    DtTemp = ViewState("Dt3").Clone
                    DtTemp.Rows.Add(DtTemp.NewRow())
                    GridDt3.DataSource = DtTemp
                    GridDt3.DataBind()
                    GridDt3.Columns(0).Visible = False
                    GridDt3.Columns(1).Visible = False
                Else
                    Dim DtTemp As DataTable
                    DtTemp = ViewState("Dt3").Clone
                    DtTemp.Rows.Add(DtTemp.NewRow())
                    GridDt3.DataSource = DtTemp
                    GridDt3.DataBind()
                    GridDt3.Columns(0).Visible = False
                End If
                
            End If
        Catch ex As Exception
            Throw New Exception("Bind Data Dt3 Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDt4(ByVal Nmbr As String)
        Dim Drow As DataRow()
        Try
            Dim dt As New DataTable
            ViewState("Dt4") = Nothing
            dt = SQLExecuteQuery(GetStringDt4(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt4") = dt
            'BindGridDt(dt, GridDt4)
            Drow = ViewState("Dt4").Select("WONo+'|'+Type+'|'+DivisiBlok =" + QuotedStr(lblWONoMT.Text + "|" + LblTypeMT.Text + "|" + LblBatchMT.Text))
            If Drow.Length > 0 Then
                BindGridDt(Drow.CopyToDataTable, GridDt4)
                GridDt4.Columns(0).Visible = Not ViewState("StateHd") = "ViewM"
            Else
                Dim DtTemp As DataTable
                DtTemp = ViewState("Dt4").Clone
                DtTemp.Rows.Add(DtTemp.NewRow())
                GridDt4.DataSource = DtTemp
                GridDt4.DataBind()
                GridDt4.Columns(0).Visible = False
            End If
        Catch ex As Exception
            Throw New Exception("Bind Data Dt4 Error : " + ex.ToString)
        End Try
    End Sub


    Sub BindGridDt2(ByVal source As DataTable, ByVal gv As GridView)
        Dim IsEmpty As Boolean
        Dim DtTemp As DataTable
        Dim dr As DataRow()
        Try
            IsEmpty = False
            dr = source.Select("", "", DataViewRowState.CurrentRows)
            If dr.Count = 0 Then
                'If source.Rows.Count = 0 Then
                DtTemp = source.Clone
                DtTemp.Rows.Add(DtTemp.NewRow()) ' create a new blank row to the DataTable
                IsEmpty = True
                gv.DataSource = DtTemp
            Else
                gv.DataSource = source
            End If
            gv.DataBind()
            If IsEmpty = True Then
                gv.Columns(0).Visible = False
            Else
                gv.Columns(0).Visible = True
            End If

        Catch ex As Exception
            Throw New Exception("ShowGridDtIfEmpty Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBack.Click, btnHome.Click
        Try
            MovePanel(pnlInput, PnlHd)
        Catch ex As Exception
            lbStatus.Text = "btn back Error : " + ex.ToString
        End Try
    End Sub
    Private Sub StatusButtonSave(ByVal Bool As Boolean)
        Try
            btnSaveAll.Visible = Bool
            btnSaveTrans.Visible = Bool
            btnBack.Visible = Bool
        Catch ex As Exception
            Throw New Exception("Status Button Save Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnCancelDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelDt.Click
        Try
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt3")) = 0 And GetCountRecord(ViewState("Dt4")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnCancelDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelDt2.Click
        Try
            MovePanel(pnlEditDt2, pnlDt2)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt3")) = 0 And GetCountRecord(ViewState("Dt4")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt2  Error : " + ex.ToString
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbCode.Text = ""
            tbDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            ddlFgBorongan.SelectedValue = "N"
            ddlWrhs.SelectedValue = ""
            ddlWorkBy.SelectedValue = ""
            ddlDivisi.SelectedValue = ""
            tbKontraktor.Text = ""
            tbKontraktorName.Text = ""
            tbMandor.Text = ""
            tbMandorName.Text = ""
            tbAsisten.Text = ""
            tbAsistenName.Text = ""
            tbRemark.Text = ""
            'Dim Division As String
            'Division = SQLExecuteScalar("EXEC S_GetPlantDivisionAll " + QuotedStr(ViewState("UserId")), ViewState("DBConnection"))
            'ddlDivisi.SelectedValue = Division
            'ddlDivisi_TextChanged(Nothing, Nothing)
            'ddlReffType_TextChanged(Nothing, Nothing)
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbWONO.Text = ""
            'ddlType.SelectedValue = 0
            tbBlock.Text = ""
            tbBlockName.Text = ""
            tbQtyWO.Text = "0"
            tbQtyDone.Text = "0"
            tbQty.Text = "0"
            tbQtySDHI.Text = "0"
            tbQtyCapacity.Text = "0"
            tbTKWO.Text = "0"
            tbTKDone.Text = "0"
            tbTKLKM.Text = "0"
            tbTKSDHI.Text = "0"
            tbHariWO.Text = "0"
            tbHariDone.Text = "0"
            tbHKNorma.Text = "0"
            tbHK.Text = "0"
            tbUnit.Text = "0"
            tbfgFinish.Text = "N"
            tbJob.Text = ""
            TbJobName.Text = ""
            tbStartGawang.Text = "0"
            tbEndGawang.Text = "0"
            tbRemarkDt.Text = ""
            tbCompleteAsisten.Text = "N"
            tbCompleteAudit.Text = "N"
            tbCompleteDenda.Text = "N"
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub

    Private Sub Cleardt2()
        Try
            tbMachine.Text = ""
            tbMachineName.Text = ""
            tbStartHour.Text = "0"
            tbEndHour.Text = "0"
            tbQtyDuration.Text = "0"
            tbOp.Text = ""
            tbOpName.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt 2 Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt3()
        Try
            tbOperator.Text = ""
            tbOperatorName.Text = ""
            tbOpBorongan.Text = ""
            tbWorkDay.Text = "1"
            tbWorkAdd.Text = "0"
            tbRemarkGawangan.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt 3 Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt4()
        Try
            tbMaterial.Text = ""
            tbMaterialName.Text = ""
            tbQtyTarget.Text = "0"
            tbQtyMaterial.Text = "0"
            tbQtyDoneMaterial.Text = "0"
            tbQtySDHIMaterial.Text = "0"
        Catch ex As Exception
            Throw New Exception("Clear Dt 4 Error " + ex.ToString)
        End Try
    End Sub

    Function CekHd() As Boolean
        Try
            If tbDate.SelectedDate = Nothing Then
                lbStatus.Text = MessageDlg("Date must have value")
                tbDate.Focus()
                Return False
            End If
            'If ddlWrhs.SelectedValue = "" Then
            '    lbStatus.Text = MessageDlg("Warehouse must have value")
            '    ddlWrhs.Focus()
            '    Return False
            'End If
            If ddlWorkBy.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Team must have value")
                ddlWorkBy.Focus()
                Return False
            End If
            If tbMandor.Text = "" Then
                lbStatus.Text = MessageDlg("Mandor must have value")
                tbMandor.Focus()
                Return False
            End If
            If tbAsisten.Text = "" Then
                lbStatus.Text = MessageDlg("Asisten must have value")
                tbAsisten.Focus()
                Return False
            End If


            'Dim Row As DataRow
            Dim GVR As GridViewRow
            Dim cekStatus As String

            For Each GVR In GridDt.Rows
                LblCode.Text = GVR.Cells(2).Text
                cekStatus = SQLExecuteScalar("SELECT COUNT(B.Material) As Material FROM PLWOResultDt A Inner join PLWOPlanRM B On A.WONo = B.TransNmbr  where A.WONo = " + QuotedStr(GVR.Cells(2).Text), ViewState("DBConnection").ToString)
                'lbStatus.Text = cekStatus
                If cekStatus > 0 Then
                    If ddlWrhs.SelectedValue = "" Then
                        lbStatus.Text = MessageDlg("Warehouse must have value")
                        ddlWrhs.Focus()
                        Return False
                    End If
                End If
            Next


            'If CInt(Session(Request.QueryString("KeyId"))("Year")) <> Year(tbDate.SelectedValue) Or CInt(Session(Request.QueryString("KeyId"))("Period")) <> Month(tbDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(Session(Request.QueryString("KeyId"))("Period")) + " " + Session(Request.QueryString("KeyId"))("Year").ToString.Trim)
            '    Return False
            'End If
            'If CFloat(tbWorkHour.Text) <= 0 Then
            '    tbWorkHour.Text = MessageDlg("Work Hour must have value")
            '    tbWorkHour.Focus()
            '    Return False
            'End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Hd Error : " + ex.ToString)
        End Try
    End Function

    Function CekDt(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not Dr Is Nothing Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("Type").ToString = "" Then
                    lbStatus.Text = MessageDlg("Type Must Have Value")
                    Return False
                End If
                
                If TrimStr(Dr("DivisiBlok").ToString) = "" Then
                    lbStatus.Text = MessageDlg("Divisi / Batch Must Have Value")
                    Return False
                End If

                If Dr("HK").ToString = "" Then
                    lbStatus.Text = MessageDlg("HK Days Must Have Value")
                    Return False
                End If

                
            End If

            Dim GVR As GridViewRow
            Dim cekStatus As String

            For Each GVR In GridDt.Rows
                LblCode.Text = GVR.Cells(2).Text
                cekStatus = SQLExecuteScalar("SELECT COUNT(B.Material) As Material FROM PLWOResultDt A Inner join PLWOPlanRM B On A.WONo = B.TransNmbr  where A.WONo = " + QuotedStr(GVR.Cells(2).Text), ViewState("DBConnection").ToString)
                'lbStatus.Text = cekStatus
                If cekStatus > 0 Then
                    lbStatus.Text = MessageDlg("WO No : " + GVR.Cells(2).Text + " Must have material")
                    ddlWrhs.Focus()
                    Return False
                End If
            Next
            'If tbHK.Text = "0" Or tbHK.Text = "" Then
            '    lbStatus.Text = MessageDlg("HK Days Must Have Value")
            '    Return False
            'End If

            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function


    Function CekDt2(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not Dr Is Nothing Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("MachineName").ToString = "" Then
                    lbStatus.Text = MessageDlg("Machine Must Have Value")
                    Return False
                End If
                If LTrim(Dr("StartHour").ToString) < 0 Then
                    lbStatus.Text = MessageDlg("Start Hour Must Have Value")
                    Return False
                End If
                If LTrim(Dr("EndHour").ToString) < 0 Then
                    lbStatus.Text = MessageDlg("End Hour Must Have Value")
                    Return False
                End If

            Else



                'If ddlStatusTanam.SelectedValue.Trim = "" Then
                '    lbStatus.Text = MessageDlg("Warehouse must have value")
                '    ddlStatusTanam.Focus()
                '    Return False
                'End If
                'If tbSubLed.Text.Trim = "" And tbFgSubLed.Text.Trim <> "N" Then
                '    lbStatus.Text = MessageDlg("SubLed must have value")
                '    tbSubLed.Focus()
                '    Return False
                'End If

            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt2 Error : " + ex.ToString)
        End Try
    End Function

    Function CekDt4(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not Dr Is Nothing Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                'If Dr("Machine").ToString = "" Then
                '    lbStatus.Text = MessageDlg("Machine Must Have Value")
                '    Return False
                'End If
                ''If Dr("StartTime").ToString = "00:00" Then
                ''    lbStatus.Text = MessageDlg("Start Time Must Have Value")
                ''    Return False
                ''End If
                'If CFloat(Dr("Duration").ToString) <= 0 Then
                '    lbStatus.Text = MessageDlg("Duration Must Have Value")
                '    Return False
                'End If


            Else
                'If tbMachineNameDt4.Text = "" Then
                '    lbStatus.Text = MessageDlg("Machine must have value")
                '    tbMachineCodeDt4.Focus()
                '    Return False
                'End If
                ''If tbStartTime.Text.Trim = "00:00" Then
                ''    lbStatus.Text = MessageDlg("Start Time must have value")
                ''    tbStartTime.Focus()
                ''    Return False
                ''End If
                'If CFloat(tbDuration.Text.Trim) <= 0 Then
                '    lbStatus.Text = MessageDlg("Duration Must Have Value")
                '    tbDuration.Focus()
                '    Return False
                'End If

            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt4 Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            tbCode.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToDropList(ddlWrhs, Dt.Rows(0)("WrhsCode").ToString)
            BindToDropList(ddlDivisi, Dt.Rows(0)("Division").ToString)
            BindToDropList(ddlWorkBy, Dt.Rows(0)("WorkBy").ToString)
            BindToDropList(ddlFgBorongan, Dt.Rows(0)("FgBorongan").ToString)
            BindToText(tbKontraktor, Dt.Rows(0)("Supplier").ToString)
            BindToText(tbKontraktorName, Dt.Rows(0)("SupplierName").ToString)
            BindToText(tbMandor, Dt.Rows(0)("CheckBy").ToString)
            BindToText(tbMandorName, Dt.Rows(0)("CheckByName").ToString)
            BindToText(tbAsisten, Dt.Rows(0)("AcknowledBy").ToString)
            BindToText(tbAsistenName, Dt.Rows(0)("AcknowledByName").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)

        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal WONo As String, ByVal Type As String, ByVal DivBlok As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("WONo = " + QuotedStr(WONo) + " AND Type = " + QuotedStr(Type) + " AND DivisiBlok = " + QuotedStr(DivBlok))
            If Dr.Length > 0 Then
                BindToText(tbWONO, Dr(0)("WONo").ToString)
                BindToDropList(ddlType, Dr(0)("Type").ToString)
                BindToText(tbBlock, Dr(0)("DivisiBlok").ToString)
                BindToText(tbBlockName, Dr(0)("DivisiBlokName").ToString)
                BindToText(tbQtyWO, FormatNumber(Dr(0)("QtyWO").ToString), 2)
                BindToText(tbQtyDone, FormatNumber(Dr(0)("QtyDone").ToString), 2)
                BindToText(tbQty, FormatNumber(Dr(0)("Qty").ToString), 2)
                BindToText(tbQtySDHI, FormatNumber(Dr(0)("QtySDHI").ToString), 2)
                BindToText(tbQtyCapacity, FormatNumber(Dr(0)("Capacity").ToString), 2)
                BindToText(tbTKWO, FormatNumber(Dr(0)("TKTotal").ToString), 2)
                BindToText(tbTKDone, FormatNumber(Dr(0)("TKDone").ToString), 2)
                BindToText(tbTKLKM, FormatNumber(Dr(0)("Person").ToString), 2)
                BindToText(tbTKSDHI, FormatNumber(Dr(0)("TKSDHI").ToString), 2)
                'BindToText(tbHariWO, Dr(0)("HariWO").ToString)
                BindToText(tbHariDone, Dr(0)("HariDone").ToString)
                BindToText(tbHKNorma, Dr(0)("HKNorma").ToString)
                BindToText(tbHK, Dr(0)("HK").ToString)
                BindToText(tbUnit, Dr(0)("Unit").ToString)
                BindToText(tbfgFinish, Dr(0)("Fgfinish").ToString)
                BindToText(tbJob, Dr(0)("JobPlant").ToString)
                BindToText(tbJobName, Dr(0)("Job_Name").ToString)
                BindToText(tbStartGawang, Dr(0)("StartGawang").ToString)
                BindToText(tbEndGawang, Dr(0)("EndGawang").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
                BindToText(tbCompleteAsisten, Dr(0)("CompleteAsisten").ToString)
                BindToText(tbCompleteAudit, Dr(0)("CompleteAudit").ToString)
                BindToText(tbCompleteDenda, Dr(0)("CompleteDenda").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub
    'Protected Sub FillTextBoxDt2(ByVal Machine As String, ByVal Tp As String, ByVal ItemNo As String, ByVal WONO As String)
    Protected Sub FillTextBoxDt2(ByVal Machine As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt2").select("Machine = " + QuotedStr(Machine))
            If Dr.Length > 0 Then
                BindToText(tbMachine, Dr(0)("Machine").ToString)
                BindToText(tbMachineName, Dr(0)("Machine_Name").ToString)
                BindToText(tbStartHour, Dr(0)("StartHour").ToString)
                BindToText(tbEndHour, Dr(0)("EndHour").ToString)
                BindToText(tbQtyDuration, Dr(0)("Duration").ToString)
                BindToText(tbOp, Dr(0)("OperatorMachine").ToString)
                BindToText(tbOpName, Dr(0)("OperatorMachineName").ToString)

            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail 2 error : " + ex.ToString)
        End Try
    End Sub
    'Protected Sub FillTextBoxDt3(ByVal Op As String, ByVal OpB As String)
    Protected Sub FillTextBoxDt3(ByVal Op As String)
        Dim Dr As DataRow()
        Try
            'Dr = ViewState("Dt3").select("Operator = " + QuotedStr(Op) + " AND OperatorBorongan = " + QuotedStr(OpB))
            Dr = ViewState("Dt3").select("OperatorBorongan = " + QuotedStr(Op))
            If Dr.Length > 0 Then

                BindToText(tbOperator, Dr(0)("Operator").ToString)
                BindToText(tbOperatorName, Dr(0)("OperatorBorongan").ToString)
                BindToText(tbOpBorongan, Dr(0)("OperatorBorongan").ToString)
                BindToText(tbWorkAdd, Dr(0)("WorkAdd").ToString)
                BindToText(tbWorkDay, Dr(0)("WorkDay").ToString)
                BindToText(tbRemarkGawangan, Dr(0)("RemarkGawangan").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail 3 error : " + ex.ToString)
        End Try
    End Sub
    'Protected Sub FillTextBoxDt4(ByVal Material As String, ByVal Tp As String, ByVal ItemNo As String, ByVal WONO As String)
    Protected Sub FillTextBoxDt4(ByVal Material As String)
        Dim Dr As DataRow()
        Try
            'Dr = ViewState("Dt4").select(" Material = " + QuotedStr(Material) + " AND WONo = " + QuotedStr(WONO) + " AND Type = " + QuotedStr(Tp) + " AND DivisiBlok = " + QuotedStr(ItemNo))
            Dr = ViewState("Dt4").select("Material = " + QuotedStr(Material))
            If Dr.Length > 0 Then

                BindToText(tbMaterial, Dr(0)("Material").ToString)
                BindToText(tbMaterialName, Dr(0)("Material_Name").ToString)
                BindToText(tbQtyTarget, CFloat(Dr(0)("QtyTarget").ToString))
                BindToText(tbQtyDoneMaterial, Dr(0)("QtyDone").ToString)
                BindToText(tbQtyMaterial, CFloat(Dr(0)("Qty").ToString))
                BindToText(tbQtySDHIMaterial, Dr(0)("QtySDHI").ToString)
                'BindToText(tbUnitM, Dr(0)("Unit").ToString)

            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail 4 error : " + ex.ToString)
        End Try
    End Sub


    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveDt.Click
        Try
            If CekDt() = False Then
                btnSaveDt.Focus()
                Exit Sub
            End If

            If ViewState("StateDt") = "Edit" Then
                Dim Row As DataRow
                Row = ViewState("Dt").Select("WONo+'|'+Type+'|'+DivisiBlok = " + QuotedStr(ViewState("DtValue")))(0)
                Row.BeginEdit()
                Row("WONo") = tbWONO.Text
                Row("Type") = ddlType.SelectedValue
                Row("DivisiBlok") = tbBlock.Text
                Row("DivisiBlokName") = tbBlockName.Text
                Row("QtyWO") = tbQtyWO.Text
                Row("QtyDone") = tbQtyDone.Text
                Row("Qty") = tbQty.Text
                Row("QtySDHI") = tbQtySDHI.Text
                Row("Capacity") = tbQtyCapacity.Text
                'Row("TKWO") = tbTKWO.Text
                Row("TKDone") = tbTKDone.Text
                'Row("TKLKM") = tbTKLKM.Text
                Row("TKSDHI") = tbTKSDHI.Text
                'Row("HariWO") = tbHariWO.Text
                Row("HariDone") = tbHariDone.Text
                Row("HKNorma") = tbHKNorma.Text
                Row("HK") = tbHK.Text
                Row("Unit") = tbUnit.Text
                Row("Fgfinish") = tbfgFinish.Text
                Row("JobPlant") = tbJob.Text
                Row("Job_Name") = tbJobName.Text
                Row("StartGawang") = tbStartGawang.Text
                Row("EndGawang") = tbEndGawang.Text
                Row("Remark") = tbRemarkDt.Text
                Row("CompleteAsisten") = tbCompleteAsisten.Text
                Row("CompleteAudit") = tbCompleteAudit.Text
                Row("CompleteDenda") = tbCompleteDenda.Text

                Row.EndEdit()
            Else
                If CekExistData(ViewState("Dt"), "Type,DivisiBlok,WONo", ddlType.Text + "|" + TrimStr(tbBlock.Text) + "|" + TrimStr(tbWONO.Text)) = True Then
                    lbStatus.Text = "Type " + ddlType.SelectedValue + " Block " + tbBlock.Text + " WO No " + tbWONO.Text + " has been already exist"
                    Exit Sub
                End If
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("WONo") = tbWONO.Text
                dr("Type") = ddlType.SelectedValue
                dr("DivisiBlok") = tbBlock.Text
                dr("DivisiBlokName") = tbBlockName.Text
                dr("QtyWO") = tbQtyWO.Text
                dr("QtyDone") = tbQtyDone.Text
                dr("Qty") = tbQty.Text
                dr("QtySDHI") = tbQtySDHI.Text
                dr("QtyCapacity") = tbQtyCapacity.Text
                dr("TKWO") = tbTKWO.Text
                dr("TKDone") = tbTKDone.Text
                dr("TKLKM") = tbTKLKM.Text
                dr("TKSDHI") = tbTKSDHI.Text
                dr("HariWO") = tbHariWO.Text
                dr("HariDone") = tbHariDone.Text
                dr("HKNorma") = tbHKNorma.Text
                dr("HK") = tbHK.Text
                dr("Unit") = tbUnit.Text
                dr("Fgfinish") = tbfgFinish.Text
                dr("Job") = tbJob.Text
                dr("JobName") = tbJobName.Text
                dr("StartGawang") = tbStartGawang.Text
                dr("EndGawang") = tbEndGawang.Text
                dr("Remark") = tbRemarkDt.Text
                dr("CompleteAsisten") = tbCompleteAsisten.Text
                dr("CompleteAudit") = tbCompleteAudit.Text
                dr("CompleteDenda") = tbCompleteDenda.Text
                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, pnlDt)
            'GridDt.Columns(1).Visible = True

            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt3")) = 0 And GetCountRecord(ViewState("Dt4")) = 0)
            BindGridDt(ViewState("Dt"), GridDt)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save Dt Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub

    Protected Sub btnSaveDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveDt2.Click
        Try

            If CekDt2() = False Then
                btnSaveDt2.Focus()
                Exit Sub
            End If

            If ViewState("StateDt2") = "Edit" Then
                Dim Row As DataRow
                'Row = ViewState("Dt2").Select("Machine+'|'+WONo+'|'+Type+'|'+DivisiBlok = " + QuotedStr(ViewState("Dt2Value")))(0)
                Row = ViewState("Dt2").Select("Machine = " + QuotedStr(ViewState("Dt2Value")))(0)
                Row.BeginEdit()
                Row("Machine") = tbMachine.Text
                Row("Machine_Name") = tbMachineName.Text
                Row("StartHour") = tbStartHour.Text
                Row("EndHour") = tbEndHour.Text
                Row("Duration") = tbQtyDuration.Text
                Row("OperatorMachine") = tbOp.Text
                Row("OperatorMachineName") = tbOpName.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt2").NewRow
                dr("WoNo") = LblTypeM.Text
                dr("Type") = LblBatchM.Text
                dr("DivisiBlok") = LblBatchNameM.Text
                dr("Machine") = tbMachine.Text
                dr("Machine_Name") = tbMachineName.Text
                dr("StartHour") = tbStartHour.Text
                dr("EndHour") = tbEndHour.Text
                dr("Duration") = tbQtyDuration.Text
                dr("OperatorMachine") = tbOp.Text
                dr("OperatorMachineName") = tbOpName.Text

                ViewState("Dt2").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt2, pnlDt2)
            'EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0 = 0)
            BindGridDt(ViewState("Dt2"), GridDt2)
            'Dim drow As DataRow()
            'drow = ViewState("Dt2").Select("Type+'|'+DivisiBlok=" + QuotedStr(lbWODt2.Text))
            'If drow.Length > 0 Then
            '    BindGridDt(drow.CopyToDataTable, GridDt2)
            '    GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
            'Else
            '    Dim DtTemp As DataTable
            '    DtTemp = ViewState("Dt2").Clone
            '    DtTemp.Rows.Add(DtTemp.NewRow())
            '    GridDt2.DataSource = DtTemp
            '    GridDt2.DataBind()
            '    GridDt2.Columns(0).Visible = False
            'End If
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save dt2 Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub

    Private Sub SaveAll()
        Dim SQLString As String
        Dim I As Integer
        Try


            If Not (ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit") Then
                Exit Sub
            Else
                ViewState("StateSave") = ViewState("StateHd")
                ViewState("StateHd") = "View"
            End If
            'Save Hd
            If ViewState("StateSave") = "Insert" Then
                tbCode.Text = GetAutoNmbr("WOR", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO PLWOResultHd (TransNmbr, TransDate, Status, Division, CheckBy, AcknowledBy, " + _
                "WorkBy, WrhsCode, WrhsFgSubLed, FgBorongan, Supplier, FgProcess, Remark, " + _
                "UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", 'H', " + QuotedStr(ddlDivisi.SelectedValue) + "," + QuotedStr(tbAsisten.Text) + "," + QuotedStr(tbMandor.Text) + "," + _
                QuotedStr(ddlWorkBy.SelectedValue) + "," + QuotedStr(ddlWrhs.SelectedValue) + "," + QuotedStr(lblFgWrhsFgSubled.Text) + ", " + QuotedStr(ddlFgBorongan.SelectedValue) + ", " + _
                QuotedStr(tbKontraktor.Text) + ", 'N', " + _
                QuotedStr(tbRemark.Text) + "," + QuotedStr(ViewState("UserId").ToString) + ", GetDate()"
                ViewState("TransNmbr") = tbCode.Text

            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM PLWOResultHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed. Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE PLWOResultHd SET TransDate = " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + _
                ", WorkBy = " + QuotedStr(ddlWorkBy.SelectedValue) + ", WrhsCode = " + QuotedStr(ddlWrhs.SelectedValue) + _
                ", Division = " + QuotedStr(ddlDivisi.SelectedValue) + ", CheckBy = " + QuotedStr(tbMandor.Text) + ", AcknowledBy = " + QuotedStr(tbAsisten.Text) + _
                ", WrhsFgSubLed =" + QuotedStr(lblFgWrhsFgSubled.Text) + _
                ", FgBorongan =" + QuotedStr(ddlFgBorongan.SelectedValue) + _
                ", Supplier = " + QuotedStr(tbKontraktor.Text) + _
                ", Remark = " + QuotedStr(tbRemark.Text) + ", DatePrep = GetDate()" + _
                " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            End If
            SQLString = Replace(SQLString, "''", "NULL")
            SQLString = SQLString.Replace("'1900-01-01'", "NULL")
            SQLString = SQLString.Replace("'0001-01-01'", "NULL")

            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            'update Primary Key on Dt
            Dim Row As DataRow()

            Row = ViewState("Dt").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbCode.Text
                Row(I).EndEdit()
            Next

            Row = ViewState("Dt2").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbCode.Text
                Row(I).EndEdit()
            Next

            Row = ViewState("Dt3").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbCode.Text
                Row(I).EndEdit()
            Next

            Row = ViewState("Dt4").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbCode.Text
                Row(I).EndEdit()
            Next


            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, WONo, Type, DivisiBlok, JobPlant, QtyWO, QtyDone, Qty, Capacity, HK, Unit, FgFinish, StartGawang, EndGawang, Remark, Person, HKNorma, CompleteAsisten, CompleteAudit, CompleteDenda FROM PLWOResultDt WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
                    "UPDATE PLWOResultDt SET WONo = @WONo, Type = @Type, DivisiBlok = @DivisiBlok, JobPlant = @JobPlant, QtyWO = @QtyWO, QtyDone = @QtyDone, Qty = @Qty, Capacity = @Capacity, HK = @HK, Unit = @Unit,  " + _
                    " FgFinish = @FgFinish, StartGawang = @StartGawang, EndGawang = @EndGawang, Remark = @Remark, Person = @Person, HKNorma = @HKNorma, CompleteAsisten = @CompleteAsisten, CompleteAudit = @CompleteAudit, CompleteDenda = @CompleteDenda WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND Type = @OldType AND DivisiBlok = @OldDivisiBlok ", con)
            ' Define output parameters.
            Update_Command.Parameters.Add("@WONo", SqlDbType.VarChar, 20, "WONo")
            Update_Command.Parameters.Add("@Type", SqlDbType.VarChar, 8, "Type")
            Update_Command.Parameters.Add("@DivisiBlok", SqlDbType.VarChar, 20, "DivisiBlok")
            Update_Command.Parameters.Add("@JobPlant", SqlDbType.VarChar, 12, "JobPlant")
            Update_Command.Parameters.Add("@QtyWO", SqlDbType.Float, 18, "QtyWO")
            Update_Command.Parameters.Add("@QtyDone", SqlDbType.Float, 18, "QtyDone")
            Update_Command.Parameters.Add("@Qty", SqlDbType.Float, 18, "Qty")
            Update_Command.Parameters.Add("@Capacity", SqlDbType.Float, 18, "Capacity")
            Update_Command.Parameters.Add("@HK", SqlDbType.Float, 18, "HK")
            Update_Command.Parameters.Add("@Unit", SqlDbType.VarChar, 5, "Unit")
            Update_Command.Parameters.Add("@FgFinish", SqlDbType.VarChar, 1, "FgFinish")
            Update_Command.Parameters.Add("@StartGawang", SqlDbType.VarChar, 10, "StartGawang")
            Update_Command.Parameters.Add("@EndGawang", SqlDbType.VarChar, 10, "EndGawang")
            Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 60, "Remark")
            Update_Command.Parameters.Add("@Person", SqlDbType.Float, 18, "Person")
            Update_Command.Parameters.Add("@HKNorma", SqlDbType.Float, 18, "HKNorma")
            Update_Command.Parameters.Add("@CompleteAsisten", SqlDbType.VarChar, 1, "CompleteAsisten")
            Update_Command.Parameters.Add("@CompleteAudit", SqlDbType.VarChar, 1, "CompleteAudit")
            Update_Command.Parameters.Add("@CompleteDenda", SqlDbType.VarChar, 1, "CompleteDenda")

            ' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@OldWONo", SqlDbType.VarChar, 20, "WONo")
            param.SourceVersion = DataRowVersion.Original
            param = Update_Command.Parameters.Add("@OldType", SqlDbType.VarChar, 8, "Type")
            param.SourceVersion = DataRowVersion.Original
            param = Update_Command.Parameters.Add("@OldDivisiBlok", SqlDbType.VarChar, 20, "DivisiBlok")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM PLWOResultDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND WONO = @WONO AND Type = @Type AND DivisiBlok = @DivisiBlok ", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@WONo", SqlDbType.VarChar, 20, "WONo")
            param.SourceVersion = DataRowVersion.Original
            param = Delete_Command.Parameters.Add("@Type", SqlDbType.VarChar, 8, "Type")
            param.SourceVersion = DataRowVersion.Original
            param = Delete_Command.Parameters.Add("@DivisiBlok", SqlDbType.VarChar, 20, "DivisiBlok")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("PLWOResultDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt

            'save dt2
            cmdSql = New SqlCommand("SELECT TransNmbr, WONo,  DivisiBlok, Type, Machine, StartHour, EndHour, Duration, OperatorMachine FROM PLWOResultMachine WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            dbcommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt2 As New DataTable("PLWOResultMachine")

            Dt2 = ViewState("Dt2")
            da.Update(Dt2)
            Dt2.AcceptChanges()
            ViewState("Dt2") = Dt2

            'save dt3
            cmdSql = New SqlCommand("SELECT TransNmbr, WONo,  DivisiBlok, Type, Operator, OperatorBorongan, WorkDay, WorkAdd, RemarkGawangan FROM PLWOResultEmp WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            dbcommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt3 As New DataTable("PLWOResultEmp")

            Dt3 = ViewState("Dt3")
            da.Update(Dt3)
            Dt3.AcceptChanges()
            ViewState("Dt3") = Dt3

            'save dt4
            cmdSql = New SqlCommand("SELECT TransNmbr, WONo, Type, DivisiBlok, Material, Qty, Unit FROM PLWOResultRM WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            dbcommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt4 As New DataTable("PLWOResultRM")

            Dt4 = ViewState("Dt4")
            da.Update(Dt4)
            Dt4.AcceptChanges()
            ViewState("Dt4") = Dt4



        Catch ex As Exception
            Throw New Exception("Save All Data Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSaveTrans_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveTrans.Click
        Dim CurrFilter, Value As String
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail 'Item' must have at least 1 record")
                Exit Sub
            End If
            For Each Dr In ViewState("Dt").Rows
                If CekDt(Dr) = False Then
                    Exit Sub
                End If
            Next
            'If GetCountRecord(ViewState("Dt3")) = 0 Then
            '    lbStatus.Text = MessageDlg("Detail 'Material Waste' must have at least 1 record")
            '    Exit Sub
            'End If
            'If GetCountRecord(ViewState("Dt4")) = 0 Then
            '    lbStatus.Text = MessageDlg("Detail 'Machine Down Time' must have at least 1 record")
            '    Exit Sub
            'End If
            SaveAll()
            MovePanel(pnlInput, PnlHd)
            CurrFilter = tbFilter.Text
            Value = ddlField.SelectedValue
            tbFilter.Text = tbCode.Text
            ddlField.SelectedValue = "TransNmbr"
            btnSearch_Click(Nothing, Nothing)
            tbFilter.Text = CurrFilter
            ddlField.SelectedValue = Value
        Catch ex As Exception
            lbStatus.Text = "btnSaveTrans Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            ModifyInput2(True, pnlInput, pnlDt, GridDt)
            'ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
            ModifyInput2(True, pnlInput, pnlDt3, GridDt3)
            ModifyInput2(True, pnlInput, PnlDt4, GridDt4)

            newTrans()
            btnHome.Visible = False
            MultiView1.ActiveViewIndex = 0
            Menu1.Items.Item(0).Selected = True
            GridDt.Columns(1).Visible = GetCountRecord(ViewState("Dt2")) > 0
            MovePanel(PnlHd, pnlInput)
            EnableHd(True)
            btnAddDt.Visible = False
            btnAddDt2.Visible = GetCountRecord(ViewState("Dt2")) > 0
            btnAddDt3.Visible = GetCountRecord(ViewState("Dt3")) > 0
            btnAdddt4.Visible = GetCountRecord(ViewState("Dt4")) > 0
            btnAddDtKe2.Visible = False
            btnAddDt2ke2.Visible = GetCountRecord(ViewState("Dt2")) > 0
            btnAddDt3ke2.Visible = GetCountRecord(ViewState("Dt3")) > 0
            btnAddDt4ke2.Visible = GetCountRecord(ViewState("Dt4")) > 0
            tbCode.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddDt.Click, btnAddDtKe2.Click
        Try
            Cleardt()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt") = "Insert"
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            StatusButtonSave(False)
            tbMaterial.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnAddDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddDt2.Click, btnAddDt2ke2.Click
        Try
            Cleardt2()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt2") = "Insert"
            MovePanel(pnlDt2, pnlEditDt2)
            EnableHd(False)
            StatusButtonSave(False)
            ddlWrhs.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt2 error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ViewState("TransNmbr") = ""
            ClearHd()
            Cleardt()
            Cleardt2()
            pnlDt.Visible = True
            BindDataDt("")
            BindDataDt2("")
            BindDataDt3("")
            BindDataDt4("")

            EnableHd(True)
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            CheckAll(GridView1, sender)
        Catch ex As Exception
            lbStatus.Text = "cb Select Checked Changed Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub LbAdvSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LbAdvSearch.Click
        Dim FDateName, FDateValue, FilterName, FilterValue As String
        Try
            FDateName = "Report Date"
            FDateValue = "TransDate"
            FilterName = "Trans No, Trans Date, Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), Remark"
            Session("DateFieldName") = FDateName.Split(",")
            Session("DateFieldValue") = FDateValue.Split(",")
            Session("FieldName") = FilterName.Split(",")
            Session("FieldValue") = FilterValue.Split(",")
            AttachScript("OpenFilterCriteria()", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "Advanced Search Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        GridView1.PageIndex = e.NewPageIndex
        BindData(Session("AdvanceFilter"))
    End Sub

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        Dim DDL As DropDownList
        Dim GVR As GridViewRow = Nothing
        Dim index As Integer
        Dim CekMenu As String
        Try
            If Not (e.CommandName = "Sort" Or e.CommandName = "Page") Then
                index = Convert.ToInt32(e.CommandArgument)
                GVR = GridView1.Rows(index)
            End If
            If e.CommandName = "Go" Then
                DDL = GridView1.Rows(index).FindControl("ddl")
                If DDL.SelectedValue = "View" Then
                    MovePanel(PnlHd, pnlInput)
                    ViewState("TransNmbr") = GVR.Cells(2).Text
                    GridDt.PageIndex = 0
                    FillTextBoxHd(ViewState("TransNmbr"))
                    BindDataDt(ViewState("TransNmbr"))
                    BindDataDt2(ViewState("TransNmbr"))
                    BindDataDt3(ViewState("TransNmbr"))
                    BindDataDt4(ViewState("TransNmbr"))

                    ViewState("StateHd") = "View"
                    btnGetDt.Visible = False
                    ModifyInput2(False, pnlInput, pnlDt, GridDt)
                    ModifyInput2(False, pnlInput, pnlDt2, GridDt2)
                    ModifyInput2(False, pnlInput, pnlDt3, GridDt3)
                    ModifyInput2(False, pnlInput, PnlDt4, GridDt4)

                    MultiView1.ActiveViewIndex = 0
                    Menu1.Items.Item(0).Selected = True

                    btnHome.Visible = True
                    'GridDt.Columns(1).Visible = True
                ElseIf DDL.SelectedValue = "Edit" Then
                    If GVR.Cells(3).Text = "H" Or GVR.Cells(3).Text = "G" Then
                        CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        MovePanel(PnlHd, pnlInput)
                        Dim Division As String
                        Division = SQLExecuteScalar("EXEC S_GetPlantDivisionAll " + QuotedStr(ViewState("UserId")), ViewState("DBConnection"))

                        ViewState("TransNmbr") = GVR.Cells(2).Text
                        GridDt.PageIndex = 0
                        BindDataDt(ViewState("TransNmbr"))
                        BindDataDt2(ViewState("TransNmbr"))
                        BindDataDt3(ViewState("TransNmbr"))
                        BindDataDt4(ViewState("TransNmbr"))

                        FillTextBoxHd(ViewState("TransNmbr"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
                        ModifyInput2(True, pnlInput, pnlDt3, GridDt3)
                        ModifyInput2(True, pnlInput, PnlDt4, GridDt4)

                        btnHome.Visible = False
                        MultiView1.ActiveViewIndex = 0
                        Menu1.Items.Item(0).Selected = True
                        'GridDt.Columns(1).Visible = True
                        btnGetDt.Visible = True
                        btnAddDt.Visible = False
                        btnAddDtKe2.Visible = False
                        btnKontraktor.Enabled = False
                        tbKontraktor.Enabled = False
                        ddlFgBorongan.Enabled = False
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt3")) = 0 And GetCountRecord(ViewState("Dt4")) = 0)
                    Else
                        lbStatus.Text = MessageDlg("Data must be Hold or Get Approval to edit")
                        Exit Sub
                    End If
                ElseIf DDL.SelectedValue = "Print" Then
                    Try
                        Dim Form1, Form2, Form3 As String

                        'CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        'If CekMenu <> "" Then
                        '    lbStatus.Text = CekMenu
                        '    Exit Sub
                        'End If
                        Form1 = "1"
                        Form2 = "2"
                        Form3 = "3"

                        Session("SelectCommand") = "EXEC S_PLWOResultForm " + QuotedStr(GVR.Cells(2).Text) + "," + QuotedStr(ViewState("UserId")) + "," + QuotedStr(Form1)
                        Session("SelectCommand2") = "EXEC S_PLWOResultForm " + QuotedStr(GVR.Cells(2).Text) + "," + QuotedStr(ViewState("UserId")) + "," + QuotedStr(Form2)
                        Session("SelectCommand3") = "EXEC S_PLWOResultForm " + QuotedStr(GVR.Cells(2).Text) + "," + QuotedStr(ViewState("UserId")) + "," + QuotedStr(Form3)
                        Session("ReportFile") = ".../../../Rpt/FormPLWOResult.frx"
                        Session("DBConnection") = ViewState("DBConnection")
                        AttachScript("openprintdlg3ds();", Page, Me.GetType)
                        'Note (Edit Mirza 18062020)
                        ' JIka untuk print menggunakan satu sub report atau lebih ganti function openprintdlg() Menjadi openprintdlg2ds() di aspx juga dan PrintForm di ganti menjadi PrintForm2 tergantung banyaknya subreport, jika menggunakan lebih dari satu sub report tinggal ganti angkanya menjadi 3 dan seterusnya, bgitu juga dengan commandnya, beri nomor urutan.

                    Catch ex As Exception
                        lbStatus.Text = "btn print Error = " + ex.ToString
                    End Try


                ElseIf DDL.SelectedValue = "Print CPWO" Then
                    Try
                        'CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        'If CekMenu <> "" Then
                        '    lbStatus.Text = CekMenu
                        '    Exit Sub
                        'End If
                        Session("SelectCommand") = "EXEC S_PLFormCPWO " + QuotedStr(GVR.Cells(2).Text) + "," + QuotedStr(ViewState("UserId"))
                        Session("ReportFile") = ".../../../Rpt/FormCPWO.frx"
                        Session("DBConnection") = ViewState("DBConnection")
                        AttachScript("openprintdlg()", Page, Me.GetType)
                    Catch ex As Exception
                        lbStatus.Text = "btn print Error = " + ex.ToString
                    End Try

                ElseIf DDL.SelectedValue = "Print CPWO Blank" Then
                    Try
                        'CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        'If CekMenu <> "" Then
                        '    lbStatus.Text = CekMenu
                        '    Exit Sub
                        'End If
                        Session("SelectCommand") = "EXEC S_PLFormCPWO " + QuotedStr(GVR.Cells(2).Text) + "," + QuotedStr(ViewState("UserId"))
                        Session("ReportFile") = ".../../../Rpt/FormCPWOBlank.frx"
                        Session("DBConnection") = ViewState("DBConnection")
                        AttachScript("openprintdlg()", Page, Me.GetType)
                    Catch ex As Exception
                        lbStatus.Text = "btn print Error = " + ex.ToString
                    End Try
                End If
            End If


        Catch ex As Exception
            lbStatus.Text = "Item Command Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub GridView1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        Try
            If ViewState("SortOrder") = Nothing Or ViewState("SortOrder") = "DESC" Then
                ViewState("SortOrder") = "ASC"
            Else
                ViewState("SortOrder") = "DESC"
            End If
            ViewState("SortExpression") = e.SortExpression + " " + ViewState("SortOrder")
            BindData(Session("AdvanceFilter"))
        Catch ex As Exception
            lbStatus.Text = "Grid View 1 Sorting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridDt.RowCommand
        Try
            Dim GVR As GridViewRow
            If e.CommandName = "View" Then
                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))
                '  lbWODt2.Text = GVR.Cells(2).Text
                MultiView1.ActiveViewIndex = 1
                Dim drow As DataRow()
                If ViewState("Dt2") Is Nothing Then
                    BindDataDt2(ViewState("TransNmbr"))
                Else
                    drow = ViewState("Dt2").Select("Type+'|'+DivisiBlok+'|'+WONo = " + QuotedStr(GVR.Cells(2).Text + "|" + GVR.Cells(3).Text) + "|" + GVR.Cells(4).Text)
                    If drow.Length > 0 Then
                        BindGridDt(drow.CopyToDataTable, GridDt2)
                        GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
                    Else
                        Dim DtTemp As DataTable
                        DtTemp = ViewState("Dt2").Clone
                        DtTemp.Rows.Add(DtTemp.NewRow())
                        GridDt2.DataSource = DtTemp
                        GridDt2.DataBind()
                        GridDt2.Columns(0).Visible = False
                    End If
                End If
            ElseIf e.CommandName = "ViewA" Then
                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))
                MultiView1.ActiveViewIndex = 1
                LblTypeM.Text = GVR.Cells(2).Text
                LblBatchM.Text = GVR.Cells(3).Text
                LblBatchNameM.Text = GVR.Cells(4).Text
                If ddlWrhs.Enabled = True Then
                    btnAddDt2.Visible = True
                    btnAddDt2ke2.Visible = True
                Else
                    btnAddDt2.Visible = False
                    btnAddDt2ke2.Visible = False
                End If

                Dim drow As DataRow()
                If ViewState("Dt2") Is Nothing Then
                    BindDataDt2(ViewState("TransNmbr"))
                Else
                    drow = ViewState("Dt2").Select("Type+'|'+DivisiBlok+'|'+WONo = " + QuotedStr(GVR.Cells(3).Text + "|" + TrimStr(LblBatchNameM.Text) + "|" + TrimStr(LblTypeM.Text)))

                    If drow.Length > 0 Then
                        BindGridDt(drow.CopyToDataTable, GridDt2)
                        GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
                    Else
                        Dim DtTemp As DataTable
                        DtTemp = ViewState("Dt2").Clone
                        DtTemp.Rows.Add(DtTemp.NewRow())
                        GridDt2.DataSource = DtTemp
                        GridDt2.DataBind()
                        GridDt2.Columns(0).Visible = False
                    End If
                End If
            ElseIf e.CommandName = "ViewE" Then
                MovePanel(pnlEditDt3, pnlDt3)
                EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
                BindGridDt(ViewState("Dt3"), GridDt3)
                StatusButtonSave(True)

                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))
                MultiView1.ActiveViewIndex = 2
                LblTypeE.Text = GVR.Cells(2).Text
                LblBatchE.Text = GVR.Cells(3).Text
                LblBatchNameE.Text = GVR.Cells(4).Text
                If ddlWrhs.Enabled = True Then
                    btnAddDt3.Visible = True
                    btnAddDt3ke2.Visible = True
                Else
                    btnAddDt3.Visible = False
                    btnAddDt3ke2.Visible = False
                End If

                Dim drow As DataRow()
                If ViewState("Dt3") Is Nothing Then
                    BindDataDt3(ViewState("TransNmbr"))
                Else
                    drow = ViewState("Dt3").Select("WONo+'|'+Type+'|'+DivisiBlok=" + QuotedStr(LblTypeE.Text + "|" + LblBatchE.Text + "|" + LblBatchNameE.Text))
                    If drow.Length > 0 Then
                        BindGridDt(drow.CopyToDataTable, GridDt3)
                        GridDt3.Columns(0).Visible = Not ViewState("StateHd") = "View"
                    Else
                        'Nmbah kondisi jika ddlfgborongan= Y            
                        If ddlFgBorongan.SelectedValue = "Y" Then
                            Dim DtTemp As DataTable
                            DtTemp = ViewState("Dt3").Clone
                            DtTemp.Rows.Add(DtTemp.NewRow())
                            GridDt3.DataSource = DtTemp
                            GridDt3.DataBind()
                            GridDt3.Columns(0).Visible = False
                            GridDt3.Columns(1).Visible = False
                            BtnBackDt3.Visible = True
                        Else
                            Dim DtTemp As DataTable
                            DtTemp = ViewState("Dt3").Clone
                            DtTemp.Rows.Add(DtTemp.NewRow())
                            GridDt3.DataSource = DtTemp
                            GridDt3.DataBind()
                            GridDt3.Columns(0).Visible = False
                            BtnBackDt3.Visible = True
                        End If
                    End If

                End If

            ElseIf e.CommandName = "ViewM" Then
                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))
                MultiView1.ActiveViewIndex = 3
                lblWONoMT.Text = GVR.Cells(2).Text
                LblTypeMT.Text = GVR.Cells(3).Text
                LblBatchMT.Text = GVR.Cells(4).Text
                LblBatchNameMT.Text = GVR.Cells(5).Text
                If ddlWrhs.Enabled = True Then
                    btnAdddt4.Visible = True
                    btnAddDt4ke2.Visible = True
                Else
                    btnAdddt4.Visible = False
                    btnAddDt4ke2.Visible = False
                End If

                Dim drow As DataRow()
                If ViewState("Dt4") Is Nothing Then
                    BindDataDt4(ViewState("TransNmbr"))
                Else
                    drow = ViewState("Dt4").Select("Type+'|'+DivisiBlok+'|'+WONo = " + QuotedStr(GVR.Cells(3).Text + "|" + TrimStr(LblBatchMT.Text) + "|" + TrimStr(lblWONoMT.Text)))
                    If drow.Length > 0 Then
                        BindGridDt(drow.CopyToDataTable, GridDt4)
                        GridDt4.Columns(0).Visible = Not ViewState("StateHd") = "View"
                    Else
                        Dim DtTemp As DataTable
                        DtTemp = ViewState("Dt4").Clone
                        DtTemp.Rows.Add(DtTemp.NewRow())
                        GridDt4.DataSource = DtTemp
                        GridDt4.DataBind()
                        GridDt4.Columns(0).Visible = False

                    End If
                End If
            End If
        Catch ex As Exception
            lbStatus.Text = lbStatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub
    Dim TQtyWO As Double = 0

    Dim MaxItem As Integer = 0
    Protected Sub GridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt.RowDataBound
        Try
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "DivisiBlok")) Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                    MaxItem = MaxItem + 1
                    TQtyWO = GetTotalSum(ViewState("Dt"), "Qty")
                ElseIf e.Row.RowType = DataControlRowType.Footer Then
                End If
            End If

            tbQty.Text = TQtyWO
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Try
            Dim dr As DataRow()
            Dim r, drt As DataRow
            Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
            'dr = ViewState("Dt").Select("Type = " + QuotedStr(GVR.Cells(2).Text) + " AND DivisiBlok = " + QuotedStr(GVR.Cells(3).Text) + " AND WOno = " + QuotedStr(GVR.Cells(4).Text))
            dr = ViewState("Dt").Select("WOno = " + QuotedStr(GVR.Cells(2).Text)) ' + " AND Type = " + QuotedStr(GVR.Cells(3).Text) + " AND DivisiBlok = " + QuotedStr(GVR.Cells(4).Text))
            For Each r In dr
                r.Delete()
            Next

            For i = 0 To GetCountRecord(ViewState("Dt2")) - 1
                drt = ViewState("Dt2").Rows(i)
                If Not drt.RowState = DataRowState.Deleted Then
                    drt.Delete()
                End If
            Next

            BindGridDt(ViewState("Dt"), GridDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt2_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt2.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow
            GVR = GridDt2.Rows(e.RowIndex)
            dr = ViewState("Dt2").Select("Machine = " + QuotedStr(GVR.Cells(1).Text))
            dr(0).Delete()
            'BindGridDt(ViewState("Dt2"), GridDt2)
            dr = ViewState("Dt2").Select("Machine = " + QuotedStr(GVR.Cells(1).Text))
            If dr.Length > 0 Then
                BindGridDt(dr.CopyToDataTable, GridDt2)
                GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
            Else
                Dim DtTemp As DataTable
                DtTemp = ViewState("Dt2").Clone
                DtTemp.Rows.Add(DtTemp.NewRow())
                GridDt2.DataSource = DtTemp
                GridDt2.DataBind()
                GridDt2.Columns(0).Visible = False
            End If

            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt 2 Row Deleting Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub GridDt2_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt2.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt2.Rows(e.NewEditIndex)
            'FillTextBoxDt2(GVR.Cells(1).Text, LblTypeM.Text, LblBatchM.Text, LblBatchNameM.Text)
            'ViewState("Dt2Value") = GVR.Cells(1).Text + "|" + LblTypeM.Text + "|" + LblBatchM.Text + "|" + LblBatchNameM.Text
            FillTextBoxDt2(GVR.Cells(1).Text)
            ViewState("Dt2Value") = GVR.Cells(1).Text
            MovePanel(pnlDt2, pnlEditDt2)
            EnableHd(False)
            ViewState("StateDt2") = "Edit"
            StatusButtonSave(False)
            btnSaveDt2.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid dt2 Editing Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnSaveAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveAll.Click
        Dim CurrFilter, Value As String
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail 'WO' must have at least 1 record")
                Exit Sub
            End If

            SaveAll()
            MovePanel(pnlInput, PnlHd)
            CurrFilter = tbFilter.Text
            Value = ddlField.SelectedValue
            tbFilter.Text = tbCode.Text
            ddlField.SelectedValue = "TransNmbr"
            btnSearch_Click(Nothing, Nothing)
            tbFilter.Text = CurrFilter
            ddlField.SelectedValue = Value
            BtnAdd_Click(Nothing, Nothing)
        Catch ex As Exception
            lbStatus.Text = "btnSaveTrans Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnAddDt3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt3.Click
        Try
            Cleardt3()
            If CekHd() = False Then
                Exit Sub
            End If

            If ddlFgBorongan.SelectedValue = "Y" Then
                tbOperatorName.Enabled = True
                tbOperator.Enabled = False
                btnOperator.Visible = False
            Else
                tbOperator.Enabled = True
                btnOperator.Visible = True
                tbOperatorName.Enabled = False
            End If

            ViewState("StateDt3") = "Insert"

            MovePanel(pnlDt3, pnlEditDt3)
            EnableHd(False)
            StatusButtonSave(False)
            btnOp.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt3 error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancelDt3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelDt3.Click
        Try
            MovePanel(pnlEditDt3, pnlDt3)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt3 Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveDt3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt3.Click
        Try
            If CekDt3() = False Then
                btnSaveDt3.Focus()
                Exit Sub
            End If

            If ViewState("StateDt3") = "Edit" Then
                Dim Row As DataRow
                'Row = ViewState("Dt3").Select("Operator+'|'+OperatorBorongan = " + QuotedStr(ViewState("Dt3Value")))(0)
                If ddlFgBorongan.SelectedValue = "Y" Then
                    Row = ViewState("Dt3").Select("OperatorBorongan = " + QuotedStr(ViewState("Dt3Value")))(0)
                Else
                    Row = ViewState("Dt3").Select("OperatorBorongan = " + QuotedStr(ViewState("Dt3Value")))(0)
                End If

                Row.BeginEdit()
                Row("Operator") = tbOperator.Text
                Row("OperatorBorongan") = tbOperatorName.Text
                Row("OperatorBorongan") = tbOpBorongan.Text
                Row("WorkDay") = tbWorkDay.Text
                Row("WorkAdd") = tbWorkAdd.Text
                Row("RemarkGawangan") = tbRemarkGawangan.Text

                Row.EndEdit()
            Else
                'Insert

                'If CekExistData(ViewState("Dt3"), "Operator", tbOperator.Text) = True Then
                '    lbStatus.Text = MessageDlg("Operator " + tbOperator.Text + " has already been exist")
                '    Exit Sub
                'End If

                If CekExistData(ViewState("Dt3"), "OperatorBorongan", tbOperatorName.Text) = True Then
                    lbStatus.Text = MessageDlg("Operator Name " + tbOperatorName.Text + " has already been exist")
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt3").NewRow
                dr("WoNo") = LblTypeE.Text
                dr("Type") = LblBatchE.Text
                dr("DivisiBlok") = LblBatchNameE.Text
                dr("Operator") = tbOperator.Text
                dr("OperatorName") = tbOperatorName.Text
                dr("OperatorBorongan") = tbOperatorName.Text
                dr("WorkDay") = tbWorkDay.Text
                dr("WorkAdd") = tbWorkAdd.Text
                dr("RemarkGawangan") = tbRemarkGawangan.Text
                ViewState("Dt3").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt3, pnlDt3)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
            'BindGridDt(ViewState("Dt3"), GridDt3)
            Dim drow As DataRow()
            If ViewState("Dt3") Is Nothing Then
                BindDataDt3(ViewState("TransNmbr"))
            Else
                drow = ViewState("Dt3").Select("WONo+'|'+Type+'|'+DivisiBlok=" + QuotedStr(LblTypeE.Text + "|" + LblBatchE.Text + "|" + LblBatchNameE.Text))
                If drow.Length > 0 Then
                    If ddlFgBorongan.SelectedValue = "Y" Then
                        BindGridDt(drow.CopyToDataTable, GridDt3)
                        GridDt3.Columns(1).Visible = False
                        'GridDt3.Columns(1).Visible = Not ViewState("StateHd") = "view"
                    Else
                        BindGridDt(drow.CopyToDataTable, GridDt3)
                        GridDt3.Columns(1).Visible = True
                        GridDt3.Columns(0).Visible = Not ViewState("StateHd") = "view"
                    End If

                Else
                    'Nmbah kondisi jika ddlfgborongan= Y
                    If ddlFgBorongan.SelectedValue = "Y" Then
                        Dim DtTemp As DataTable
                        DtTemp = ViewState("Dt3").Clone
                        DtTemp.Rows.Add(DtTemp.NewRow())
                        GridDt3.DataSource = DtTemp
                        GridDt3.DataBind()
                        GridDt3.Columns(1).Visible = False
                    Else
                        Dim DtTemp As DataTable
                        DtTemp = ViewState("Dt3").Clone
                        DtTemp.Rows.Add(DtTemp.NewRow())
                        GridDt3.DataSource = DtTemp
                        GridDt3.DataBind()
                        BtnBackDt3.Visible = True
                    End If
                End If

            End If
            BindGridDt(ViewState("Dt3"), GridDt3)

            StatusButtonSave(True)
            'BindDataDt3(LblTypeE.Text)
            GetOperator()

        Catch ex As Exception
            lbStatus.Text = "btn save Dt3 Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub
    Function CekDt3(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not Dr Is Nothing Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If LTrim(Dr("WorkDay").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Work Day Must Have Value")
                    Return False
                End If
                If LTrim(Dr("WorkDay").ToString) > 1 Then
                    lbStatus.Text = MessageDlg("Work Day cannot greater than 1 days")
                    Return False
                End If
                If LTrim(Dr("WorkAdd").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Add Day Must Have Value")
                    Return False
                End If
                If LTrim(Dr("WorkAdd").ToString) > 1 Then
                    lbStatus.Text = MessageDlg("Add Day cannot greater than 1 days")
                    Return False
                End If

                If ddlFgBorongan.SelectedValue = "N" Then
                    If LTrim(Dr("Operator").ToString) <= 0 Then
                        lbStatus.Text = MessageDlg("Operator Must Have Value")
                        Return False
                    End If
                Else
                    If LTrim(Dr("OperatorBorongan").ToString) <= 0 Then
                        lbStatus.Text = MessageDlg("Operator Borongan Must Have Value")
                        Return False
                    End If
                End If
            Else
            End If

            'If tbOperator.Text = "" Then
            '    lbStatus.Text = MessageDlg("Operator mush have value")
            '    Return False
            'End If

            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub GetOperator()
        Try
            '' => Menghitung jumlah record Operator
            Dim JmlhOperator As Integer
            Dim TKLKM As String

            JmlhOperator = GetCountRecord(ViewState("Dt3"))
            TKLKM = JmlhOperator
            'lbStatus.Text = JmlhOperator
            'Exit Sub

            Dim Row As DataRow
            Row = ViewState("Dt").Select("WONo+'|'+Type+'|'+DivisiBlok = " + QuotedStr(LblTypeE.Text + "|" + LblBatchE.Text + "|" + LblBatchNameE.Text))(0)
            Row.BeginEdit()
            Row("Person") = TKLKM
            Row.EndEdit()
            BindGridDt(ViewState("Dt"), GridDt)
            ''=================
        Catch ex As Exception
            lbStatus.Text = "Error Get Operator : " + ex.ToString
        End Try
    End Sub



    Protected Sub GridDt3_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt3.RowDeleting
        Try
            If ddlFgBorongan.SelectedValue = "Y" Then

                Dim dr() As DataRow
                Dim GVR As GridViewRow
                GVR = GridDt3.Rows(e.RowIndex)
                'dr = ViewState("Dt3").Select("Operator+'|'+OperatorBorongan = " + QuotedStr(GVR.Cells(1).Text) + "|" + GVR.Cells(3).Text)
                If ddlFgBorongan.SelectedValue = "Y" Then
                    dr = ViewState("Dt3").Select("OperatorBorongan = " + QuotedStr(GVR.Cells(2).Text))
                Else
                    dr = ViewState("Dt3").Select("Operator = " + QuotedStr(GVR.Cells(1).Text))
                End If
                dr(0).Delete()
                BindGridDt(ViewState("Dt3"), GridDt3)
                GetOperator()
                EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
            Else

                Dim dr() As DataRow
                Dim GVR As GridViewRow
                GVR = GridDt3.Rows(e.RowIndex)
                'dr = ViewState("Dt3").Select("Operator+'|'+OperatorBorongan = " + QuotedStr(GVR.Cells(1).Text) + "|" + GVR.Cells(3).Text)
                If ddlFgBorongan.SelectedValue = "Y" Then
                    dr = ViewState("Dt3").Select("Operator = " + QuotedStr(GVR.Cells(1).Text))
                Else
                    dr = ViewState("Dt3").Select("OperatorBorongan = " + QuotedStr(GVR.Cells(2).Text))
                End If
                dr(0).Delete()
                BindGridDt(ViewState("Dt3"), GridDt3)
                GetOperator()
                EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
            End If


        Catch ex As Exception
            lbStatus.Text = "Grid Dt 3 Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt3_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt3.RowEditing
        Dim GVR As GridViewRow
        Try
            If ddlFgBorongan.SelectedValue = "Y" Then
                GVR = GridDt3.Rows(e.NewEditIndex)
                'FillTextBoxDt3(GVR.Cells(1).Text, GVR.Cells(2).Text)
                'ViewState("Dt3Value") = GVR.Cells(1).Text + "|" + GVR.Cells(2).Text
                FillTextBoxDt3(GVR.Cells(2).Text)
                ViewState("Dt3Value") = GVR.Cells(2).Text
                MovePanel(pnlDt3, pnlEditDt3)
                EnableHd(False)
                ViewState("StateDt3") = "Edit"
                StatusButtonSave(False)
                btnSaveDt3.Focus()

            Else
                GVR = GridDt3.Rows(e.NewEditIndex)
                'FillTextBoxDt3(GVR.Cells(1).Text, GVR.Cells(2).Text)
                'ViewState("Dt3Value") = GVR.Cells(1).Text + "|" + GVR.Cells(2).Text
                FillTextBoxDt3(GVR.Cells(2).Text)
                ViewState("Dt3Value") = GVR.Cells(2).Text
                MovePanel(pnlDt3, pnlEditDt3)
                EnableHd(False)
                ViewState("StateDt3") = "Edit"
                StatusButtonSave(False)
                btnSaveDt3.Focus()
            End If

        Catch ex As Exception
            lbStatus.Text = "Grid dt3 Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnKontraktor_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnKontraktor.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT Kontraktor_Code, Kontraktor_Name from V_MsKontraktor"
            ResultField = "Kontraktor_Code, Kontraktor_Name"
            ViewState("Sender") = "btnKontraktor"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnKontraktor_Click Error : " + ex.ToString
        End Try

    End Sub


    Protected Sub btnMandor_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMandor.Click
        Dim ResultField As String
        Try
            Session("filter") = "Select * from V_MsEmployee Where Fg_Active='Y'"
            ResultField = "Emp_No, Emp_Name"
            ViewState("Sender") = "btnMandor"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Cust Error : " + ex.ToString
        End Try

    End Sub

    Protected Sub btnAsisten_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAsisten.Click
        Dim ResultField As String
        Try
            Session("filter") = "Select * from V_MsEmployee Where Fg_Active='Y'"
            ResultField = "Emp_No, Emp_Name"
            ViewState("Sender") = "btnAsisten"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Cust Error : " + ex.ToString
        End Try

    End Sub

    Protected Sub btnMaterial_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMaterial.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT Material, Material_Name, Qty, Unit FROM V_PLWOResultGetReffRM WHERE Reference = " + QuotedStr(lblWONoMT.Text) + " AND Warehouse = " + QuotedStr(ddlWrhs.SelectedValue)
            ResultField = "Material, Material_Name, Qty, Unit"
            ViewState("Sender") = "btnMaterial"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnStart_Click Error : " + ex.ToString
        End Try

    End Sub

    Protected Sub tbMaterial_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbMaterial.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try

            Dt = SQLExecuteQuery("SELECT Material, Material_Name, Qty, Unit FROM V_PLWOResultGetReffRM WHERE Reference = " + QuotedStr(lblWONoMT.Text) + " AND Warehouse = " + ddlWrhs.SelectedValue + " AND Material_Code = " + QuotedStr(tbMaterial.Text), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbMaterial.Text = Dr("Material")
                tbMaterialName.Text = Dr("Material_Name")
                tbQtyMaterial.Text = Dr("Qty")
                tbUnit.Text = Dr("Unit")
                btnMaterial.Focus()
            Else
                tbMaterial.Text = ""
                tbMaterialName.Text = ""
                tbQtyMaterial.Text = "0"
                tbUnit.Text = ""
                tbMaterial.Focus()
            End If
        Catch ex As Exception
            Throw New Exception("tbMaterial_TextChanged Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub btnMachine_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMachine.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT  Machine_Code, Machine_Name From V_MsMachine "
            ResultField = "Machine_Code, Machine_Name"
            ViewState("Sender") = "btnMachine"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnMachine_Click Error : " + ex.ToString
        End Try

    End Sub
    Protected Sub tbMachine_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbMachine.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try

            Dt = SQLExecuteQuery("select  Machine_Code, Machine_Name From V_MsMachine WHERE Machine_Code = " + QuotedStr(tbMachine.Text), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbMachine.Text = Dr("Machine_Code")
                tbMachineName.Text = Dr("Machine_Name")
                btnMachine.Focus()
            Else
                tbMachine.Text = ""
                tbMachineName.Text = ""
                tbMachine.Focus()
            End If
        Catch ex As Exception
            Throw New Exception("tbMachine_TextChanged Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub btnOp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOp.Click
        Dim ResultField As String
        Try
            Session("filter") = "Select Emp_No, Emp_Name From V_MsEmployee Where Fg_Active='Y' "
            ResultField = "Emp_No, Emp_Name"
            ViewState("Sender") = "btnOp"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnMachine_Click Error : " + ex.ToString
        End Try

    End Sub

    Protected Sub btnOperator_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOperator.Click
        Dim ResultField As String
        Try
            Session("filter") = "Select Emp_No, Emp_Name From V_MsEmployee Where Fg_Active='Y' "
            ResultField = "Emp_No, Emp_Name"
            ViewState("Sender") = "btnOperator"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnMachine_Click Error : " + ex.ToString
        End Try

    End Sub


    Protected Sub btnSaveDt4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt4.Click
        Try
            If CekDt4() = False Then
                btnSaveDt4.Focus()
                Exit Sub
            End If
            If ViewState("StateDt4") = "Edit" Then
                Dim Row As DataRow
                'Row = ViewState("Dt4").Select("Material+'|'+WONo+'|'+Type+'|'+DivisiBlok = " + QuotedStr(ViewState("Dt4Value")))(0)
                Row = ViewState("Dt4").Select("Material = " + QuotedStr(ViewState("Dt4Value")))(0)
                Row.BeginEdit()
                Row("Material") = tbMaterial.Text
                Row("Material_Name") = tbMaterialName.Text
                Row("Qty") = tbQtyMaterial.Text
                Row("Unit") = tbUnit.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt4").NewRow
                dr("WONo") = lblWONoMT.Text
                dr("Type") = LblTypeMT.Text
                dr("DivisiBlok") = LblBatchMT.Text
                dr("Material") = tbMaterial.Text
                dr("Material_Name") = tbMaterialName.Text
                dr("Qty") = tbQtyMaterial.Text
                dr("Unit") = tbUnit.Text
                ViewState("Dt4").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt4, PnlDt4)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
            BindGridDt(ViewState("Dt4"), GridDt4)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save Dt4 Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub

    Protected Sub GridDt4_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt4.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow
            GVR = GridDt4.Rows(e.RowIndex)
            dr = ViewState("Dt4").Select("Material = " + GVR.Cells(1).Text)
            dr(0).Delete()
            BindGridDt(ViewState("Dt4"), GridDt4)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt 4 Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt4_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt4.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt4.Rows(e.NewEditIndex)
            'FillTextBoxDt4(GVR.Cells(1).Text, GVR.Cells(2).Text, GVR.Cells(3).Text, GVR.Cells(4).Text)
            'ViewState("Dt4Value") = GVR.Cells(1).Text + "|" + GVR.Cells(2).Text + "|" + GVR.Cells(2).Text + "|" + GVR.Cells(2).Text
            FillTextBoxDt4(GVR.Cells(1).Text)
            ViewState("Dt4Value") = GVR.Cells(1).Text
            MovePanel(PnlDt4, pnlEditDt4)
            EnableHd(False)
            ViewState("StateDt4") = "Edit"
            StatusButtonSave(False)
            EnableDt4(False)
        Catch ex As Exception
            lbStatus.Text = "Grid dt4 Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddDt4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdddt4.Click, btnAddDt4ke2.Click
        Try
            Cleardt4()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt4") = "Insert"
            MovePanel(PnlDt4, pnlEditDt4)
            EnableHd(False)
            StatusButtonSave(False)
            EnableDt4(True)
            tbMaterial.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt4 error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnCancelDt4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelDt4.Click
        Try
            MovePanel(pnlEditDt4, PnlDt4)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt4 Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub Menu2_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu2.MenuItemClick
    '    Dim index As Integer
    '    Try
    '        index = Int32.Parse(e.Item.Value)
    '        MultiView.ActiveViewIndex = index
    '        If MultiView.ActiveViewIndex = 1 Then
    '            FillCombo(ddlUseRollNo, "SELECT RollNo FROM V_PLWOResultRollUse WHERE TransNmbr = " + QuotedStr(tbLotLHPNo.Text) + " AND WOItemNo = " + QuotedStr(ddlLotWOItemNo.SelectedValue), True, "RollNo", "RollNo", ViewState("DBConnection"))
    '        End If
    '    Catch ex As Exception
    '        lbStatus.Text = "Menu Item Click Error : " + ex.ToString
    '    End Try
    'End Sub


    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            FillTextBoxDt(GVR.Cells(2).Text, GVR.Cells(3).Text, GVR.Cells(4).Text)
            ViewState("DtValue") = GVR.Cells(2).Text + "|" + GVR.Cells(3).Text + "|" + GVR.Cells(4).Text
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            StatusButtonSave(False)
            btnSaveDt.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid dt Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlShowRecord_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlShowRecord.SelectedIndexChanged
        GridView1.PageIndex = 0
        GridView1.EditIndex = -1
        GridView1.PageSize = ddlShowRecord.SelectedValue
        BindData()
    End Sub

    Public Sub CheckAllDt(ByRef gd As GridView, ByVal sender As CheckBox)
        Dim cb, cbselek As CheckBox
        Dim GRW As GridViewRow
        Try
            cb = sender
            For Each GRW In gd.Rows
                cbselek = GRW.FindControl("cbSelect")
                cbselek.Checked = cb.Checked
                If cb.Checked = False Then
                    '   btnProcessDel.Visible = True
                End If
            Next
        Catch ex As Exception
            Throw New Exception("Check All Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub cbSelectHd_CheckedChanged1(ByVal sender As Object, ByVal e As System.EventArgs)
        '  CheckAllDt(GridRollOutput, sender)
    End Sub


    Protected Sub ddlDivisi_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDivisi.TextChanged
        Try
            FillCombo(ddlWorkBy, "EXEC S_GetTeamWO " + QuotedStr(ddlDivisi.SelectedValue) + ",''", True, "TeamCode", "TeamName", ViewState("DBConnection"))

        Catch ex As Exception
            Throw New Exception("ddlDivisi_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    'Protected Sub btnReff_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReff.Click
    '    Dim ResultField As String
    '    Try
    '        Session("filter") = "SELECT  TransNmbr, Team, Person, JobPlant, JobPlant_Name, Rotasi, StartDate, EndDate, Qty, Section, RK_Year, RK_Month, Supplier, SuppName From V_PLWOResultGetDivBLock where Division =" + QuotedStr(ddlDivisi.SelectedValue)
    '        ResultField = "TransNmbr, JobPlant, JobPlant_Name, Rotasi, Team, Person"
    '        ViewState("Sender") = "btnReff"
    '        Session("Column") = ResultField.Split(",")
    '        Session("DBConnection") = ViewState("DBConnection")
    '        AttachScript("OpenSearchDlg();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lbStatus.Text = "btnReff_Click Error : " + ex.ToString
    '    End Try

    'End Sub

    Protected Sub btnGetDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetDt.Click
        Dim ResultField As String
        Try
            Session("filter") = "EXEC S_PLWOResultReff '" + ViewState("UserId") + "',' And Divisi =  '" + QuotedStr(ddlDivisi.SelectedValue) + "' AND WorkBy = '" + QuotedStr(ddlWorkBy.SelectedValue) + "'',''"
            ResultField = "Reference, Divisi, Divisi_Name, Divisi_Block, Divisi_Block_Name, WorkBy, WorkBy_Name, FgBorongan, Type, Supplier, Supplier_Name " + _
                        ",StatusTanam, Job, Job_Name, NormaHK, QtyWO, Qty, Unit, QtyDone, QtySDHI,Person, TKTotal, TKDone, TKSDHI, HariTotal, HariDone, FgInput, WrhsCode, WrhsName, WrhsGroup"
            'lbStatus.Text = Session("filter")
            '            Exit Sub
            ViewState("Sender") = "btnGetDt"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnGetDt Error : " + ex.ToString
        End Try


    End Sub


    Protected Sub lbCount_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCount.Click
        Dim ResultField, sqlstring, ResultSame, CriteriaField As String
        Try
            'Session("filter") = "EXEC S_PLWOResultReff '',''"
            'ResultField = "Reference, Divisi, Divisi_Name, Divisi_Block, Divisi_Block_Name, WorkBy, WorkBy_Name, FgBorongan, Type, Supplier, Supplier_Name " + _
            '            ",StatusTanam, Job, Job_Name, NormaHK, QtyWO, Qty, Unit, QtyDone, QtySDHI,Person, TKTotal, TKDone, TKSDHI, HariTotal, HariDone, FgInput, WrhsCode, WrhsName, WrhsGroup"
            ''lbStatus.Text = Session("filter")
            ''            Exit Sub
            'ViewState("Sender") = "BtnOut"
            'Session("Column") = ResultField.Split(",")
            'Session("DBConnection") = ViewState("DBConnection")
            'AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())

            sqlstring = "EXEC S_PLWOResultReff '" + ViewState("UserId") + "', '',''"
            Session("filter") = sqlstring
            ResultField = "Reference, Divisi, Divisi_Name, Divisi_Block, Divisi_Block_Name, WorkBy, WorkBy_Name, FgBorongan, Type, Supplier, Supplier_Name " + _
                        ",StatusTanam, Job, Job_Name, NormaHK, QtyWO, Qty, Unit, QtyDone, QtySDHI,Person, TKTotal, TKDone, TKSDHI, HariTotal, HariDone, FgInput, WrhsCode, WrhsName, WrhsGroup"
            CriteriaField = "Reference, Divisi, Divisi_Name, Divisi_Block, Divisi_Block_Name, WorkBy, WorkBy_Name, FgBorongan, Type, Supplier, Supplier_Name " + _
                        ",StatusTanam, Job, Job_Name, NormaHK, QtyWO, Qty, Unit, QtyDone, QtySDHI,Person, TKTotal, TKDone, TKSDHI, HariTotal, HariDone, FgInput, WrhsCode, WrhsName, WrhsGroup"
            Session("DBConnection") = ViewState("DBConnection")
            'Session("ClickSame") = "Bill_To"
            Session("Column") = ResultField.Split(",")
            Session("CriteriaField") = CriteriaField.Split(",")
            ResultSame = "Reference, Divisi, Divisi_Block "
            Session("ResultSame") = ResultSame.Split(",")
            ViewState("Sender") = "btnOut"
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())

        Catch ex As Exception
            lbStatus.Text = "btnGetDt Error : " + ex.ToString
        End Try


    End Sub

    'Protected Sub ddlType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlType.TextChanged
    '    Try


    '    Catch ex As Exception
    '        Throw New Exception("ddlType_TextChanged Error : " + ex.ToString)
    '    End Try
    'End Sub

    Protected Sub ddlWorkBy_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlWorkBy.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try

            Dt = SQLExecuteQuery("EXEC S_FindTeam" + QuotedStr(ddlWorkBy.SelectedValue), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                If Dr("Team_Type") = "Borongan" Then
                    tbKontraktor.Enabled = True
                    tbKontraktorName.Enabled = True
                    btnKontraktor.Enabled = True
                    ddlFgBorongan.SelectedValue = "Y"
                Else

                    tbKontraktor.Enabled = False
                    tbKontraktorName.Enabled = False
                    btnKontraktor.Enabled = False
                    ddlFgBorongan.SelectedValue = "N"
                End If
                'If Dr("Total_Member") > 0 Then
                '    tbPerson.Text = Dr("Total_Member")
                'Else
                '    tbPerson.Text = "1"
                'End If
            End If

            If ddlFgBorongan.SelectedValue = "N" Then
                tbOperator.Enabled = True
                tbOperatorName.Enabled = False
                btnOperator.Visible = True
            Else
                tbOperator.Enabled = False
                tbOperatorName.Enabled = True
                btnOperator.Visible = False
            End If

            Dim drow As DataRow()
            If ViewState("Dt3") Is Nothing Then
                BindDataDt3(ViewState("TransNmbr"))
            Else
                drow = ViewState("Dt3").Select("WONo+'|'+Type+'|'+DivisiBlok=" + QuotedStr(LblTypeE.Text + "|" + LblBatchE.Text + "|" + LblBatchNameE.Text))
                If drow.Length > 0 Then
                    If ddlFgBorongan.SelectedValue = "Y" Then
                        BindGridDt(drow.CopyToDataTable, GridDt3)
                        GridDt3.Columns(1).Visible = False
                        'GridDt3.Columns(1).Visible = Not ViewState("StateHd") = "view"
                    Else
                        BindGridDt(drow.CopyToDataTable, GridDt3)
                        GridDt3.Columns(1).Visible = True
                        GridDt3.Columns(0).Visible = Not ViewState("StateHd") = "view"
                    End If

                Else
                    'Nmbah kondisi jika ddlfgborongan= Y
                    If ddlFgBorongan.SelectedValue = "Y" Then
                        Dim DtTemp As DataTable
                        DtTemp = ViewState("Dt3").Clone
                        DtTemp.Rows.Add(DtTemp.NewRow())
                        GridDt3.DataSource = DtTemp
                        GridDt3.DataBind()
                        GridDt3.Columns(1).Visible = False
                    Else
                        Dim DtTemp As DataTable
                        DtTemp = ViewState("Dt3").Clone
                        DtTemp.Rows.Add(DtTemp.NewRow())
                        GridDt3.DataSource = DtTemp
                        GridDt3.DataBind()
                        BtnBackDt3.Visible = True
                        GridDt3.Columns(1).Visible = True
                    End If
                End If

            End If
            BindGridDt(ViewState("Dt3"), GridDt3)




        Catch ex As Exception
            Throw New Exception("ddlWorkBy_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbKontraktor_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbKontraktor.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try
            Dt = SQLExecuteQuery("Select Kontraktor_Code, Kontraktor_Name from V_MsKontraktor  WHERE Kontraktor_Code =" + QuotedStr(tbKontraktor.Text), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbKontraktor.Text = Dr("Kontraktor_Code")
                tbKontraktorName.Text = Dr("Kontraktor_Name")
                btnKontraktor.Focus()
            Else
                tbKontraktor.Text = ""
                tbKontraktorName.Text = ""
                tbKontraktor.Focus()
            End If

        Catch ex As Exception
            Throw New Exception("tbKontraktor_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbQty_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbQty.TextChanged
        Try
            tbQty.Text = FormatNumber((tbQty.Text), 2)
            'If FormatNumber((tbQty.Text >= tbQtyWO.Text - tbQtyDone - 0.01), 2) Then
            If FormatNumber((tbQty.Text >= tbQtyWO.Text - 0), 2) Then
                tbfgFinish.Text = "Y"
                tbfgFinish.Enabled = False
            Else
                tbfgFinish.Text = "N"
                tbfgFinish.Enabled = True
            End If

            If tbQty.Text = "" Then
                tbQty.Text = 0
            End If

        Catch ex As Exception
            Throw New Exception("tbqty_textChange Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnBackDt2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBackDt2.Click, btnBackDt2ke2.Click, BtnBackDt4.Click, btnbackDt4ke2.Click
        Try
            MultiView1.ActiveViewIndex = 0
            pnlDt.Visible = True
            pnlEditDt.Visible = False
            GridDt.Columns(0).Visible = GetCountRecord(ViewState("Dt")) > 0
        Catch ex As Exception
            Throw New Exception("btnBackDt2_Click Error : " + ex.ToString)
        End Try
        '   btnBackDt2ke2
    End Sub

    Protected Sub BtnBackDt3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnBackDt3.Click, BtnBackDt3ke2.Click
        Try

            MultiView1.ActiveViewIndex = 0
            pnlDt.Visible = True
            pnlEditDt.Visible = False

            GetOperator()
            GridDt.Columns(0).Visible = GetCountRecord(ViewState("Dt")) > 0
        Catch ex As Exception
            Throw New Exception("btnBackDt2_Click Error : " + ex.ToString)
        End Try
        '   btnBackDt2ke2
    End Sub

    'Private Sub btnGetOperator(ByVal job As String, ByVal reff As String, ByVal Divisi As String, ByVal Type As String)
    Protected Sub btnGetOperator_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetOperator.Click

        Dim dtUnit As DataTable
        Dim drUnit As DataRow
        Dim SQLString As String
        Dim cekStatus As String

        Try
            ' Cekdata dari MsTeamdt ada atau engga
            cekStatus = SQLExecuteScalar("SELECT COUNT(TeamCode) FROM MsTeamDt Where TeamCode = " + QuotedStr(ddlWorkBy.SelectedValue), ViewState("DBConnection").ToString)
            If cekStatus = 0 Then
                lbStatus.Text = MessageDlg("No Data to input")
                Exit Sub
            End If


            'ambil data dari sp S_PLWOResultGetOperator untuk di masukan ke dalam grid operator
            'SQLString = "SELECT * FROM PLWOResultEmp"
            SQLString = "  Exec S_PLWOResultGetOperator " + QuotedStr(ddlWorkBy.SelectedValue)
            dtUnit = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)
            drUnit = dtUnit.Rows(0)


            For Each drUnit In dtUnit.Rows
                If dtUnit.Rows.Count > 0 Then
                    If Not CekExistData(ViewState("Dt3"), "OperatorBorongan", drUnit("Employee_Name").ToString) = True Then
                        Dim dr As DataRow
                        dr = ViewState("Dt3").NewRow
                        dr("WoNo") = LblTypeE.Text
                        dr("Type") = LblBatchE.Text
                        dr("DivisiBlok") = LblBatchNameE.Text
                        dr("Operator") = drUnit("Employee").ToString
                        dr("OperatorBorongan") = drUnit("Employee_Name").ToString
                        dr("WorkDay") = 1
                        dr("WorkAdd") = 0 'drUnit("WorkAdd").ToString
                        dr("RemarkGawangan") = "" 'drUnit("RemarkGawangan").ToString
                        ViewState("Dt3").Rows.Add(dr)
                    End If
                End If
            Next
            BindGridDt(ViewState("Dt3"), GridDt3)
            EnableHd(GetCountRecord(ViewState("Dt3")) = 0)
            'GridDt3.Columns(1).Visible = GetCountRecord(ViewState("Dt3")) > 0
        Catch ex As Exception
            Throw New Exception("GetMachine Error : " + ex.ToString)
        End Try
    End Sub


    Private Sub GetMachine(ByVal job As String, ByVal reff As String, ByVal DivisiBlok As String, ByVal Type As String)
        Dim drResult As DataRow
        Dim ExistRow As DataRow()
        Dim dtUnit As DataTable
        Dim drUnit As DataRow
        Dim SQLString As String
        Try
            For Each drResult In Session("Result").Rows
                ExistRow = ViewState("Dt2").Select("DivisiBlok = " + QuotedStr(DivisiBlok) + " AND Type = " + QuotedStr(Type))
                If ExistRow.Count = 0 Then
                    SQLString = "EXEC S_PLWOResultGetMachineAll " + QuotedStr(reff) + " , " + QuotedStr(job)
                    dtUnit = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)
                    If dtUnit.Rows.Count > 0 Then
                        drUnit = dtUnit.Rows(0)
                        Dim dr As DataRow
                        dr = ViewState("Dt2").NewRow
                        dr("Type") = Type
                        dr("DivisiBlok") = DivisiBlok
                        dr("Machine") = drUnit("Machine_Code").ToString
                        dr("MachineName") = drUnit("Machine_Name").ToString
                        dr("EstHour") = FormatFloat(drUnit("Est_Hour").ToString, ViewState("DigitQty"))
                        ViewState("Dt2").Rows.Add(dr)
                    End If

                End If
            Next
            BindGridDt(ViewState("Dt2"), GridDt2)
            EnableHd(GetCountRecord(ViewState("Dt2")) = 0)
            GridDt2.Columns(1).Visible = GetCountRecord(ViewState("Dt2")) > 0
        Catch ex As Exception
            Throw New Exception("GetMachine Error : " + ex.ToString)
        End Try
    End Sub


    Private Sub GetMaterial(ByVal type As String, ByVal reffNo As String, ByVal job As String, ByVal block As String, ByVal rotasi As Integer, ByVal divBlockType As String, ByVal statusTanam As String, ByVal qty As Double, ByVal pkk As Double)
        Dim drResult As DataRow
        Dim ExistRow As DataRow()
        Dim dtUnit As DataTable
        Dim drUnit As DataRow
        Dim SQLString As String
        Try

            For Each drResult In Session("Result").Rows
                ExistRow = ViewState("Dt4").Select("DivisiBlok = " + QuotedStr(block) + " AND Type = " + QuotedStr(divBlockType))
                If ExistRow.Count = 0 Then
                    SQLString = "EXEC S_PLWOResultFindRMReff " + QuotedStr(type) + " , " + QuotedStr(reffNo) + " , " + QuotedStr(job) + " , " + QuotedStr(block) + " , " + rotasi + " , " + QuotedStr(divBlockType) + " , " + QuotedStr(statusTanam) + " , " + qty + " , " + pkk
                    'lbStatus.Text = SQLString
                    'Exit For
                    dtUnit = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)
                    If dtUnit.Rows.Count > 0 Then
                        drUnit = dtUnit.Rows(0)
                        Dim dr As DataRow
                        dr = ViewState("Dt4").NewRow
                        dr("Type") = type.ToString
                        dr("DivisiBlok") = block.ToString
                        dr("Material") = drUnit("Material_Code").ToString
                        dr("MaterialName") = drUnit("Material_Name").ToString
                        dr("Qty") = FormatFloat(drUnit("Qty").ToString, ViewState("DigitQty"))
                        dr("Unit") = drUnit("Unit").ToString
                        If qty > 0 Then dr("QtyDosis") = dr("Qty") / qty Else dr("QtyDosis") = 0
                        If pkk > 0 Then dr("QtyPokok") = dr("Qty") / pkk Else dr("QtyPokok") = 0
                        dr("AltQty") = dr("Qty")
                        ViewState("Dt4").Rows.Add(dr)
                    End If
                End If
            Next

            BindGridDt(ViewState("Dt4"), GridDt4)
            EnableHd(GetCountRecord(ViewState("Dt4")) = 0)
            GridDt4.Columns(1).Visible = GetCountRecord(ViewState("Dt4")) > 0
        Catch ex As Exception
            Throw New Exception("GetMaterial Error : " + ex.ToString)
        End Try
    End Sub
End Class
