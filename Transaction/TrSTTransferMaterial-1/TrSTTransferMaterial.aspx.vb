Imports System.Data
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO

Partial Class Transaction_TrSTTransferRM_TrTransferRM
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter

    Private Function GetStringHd() As String
        Return "Select * From v_sttransferHd "
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim CurrFilter, Value As String
        Try
            If Not IsPostBack Then
                InitProperty()
                ViewState("SetLocation") = False
                FillCombo(ddlWrhsArea, "EXEC S_GetWrhsArea", True, "Wrhs_Area_Code", "Wrhs_Area_Name", ViewState("DBConnection"))
                FillCombo(ddlWrhsSrc, "EXEC S_GetWrhsUserArea " + QuotedStr(ViewState("UserId").ToString) + "," + QuotedStr(ddlWrhsArea.SelectedValue), True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection"))
                FillCombo(ddlWrhsDest, "EXEC S_GetWrhsUserArea " + QuotedStr(ViewState("UserId").ToString) + "," + QuotedStr(ddlWrhsArea.SelectedValue), True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection"))
                FillCombo(ddlCostCtrSrc, "EXEC S_GetCostCtr ", True, "Cost_Ctr_Code", "Cost_Ctr_Name", ViewState("DBConnection"))
                FillCombo(ddlCostCtrDest, "EXEC S_GetCostCtr ", True, "Cost_Ctr_Code", "Cost_Ctr_Name", ViewState("DBConnection"))
                FillCombo(ddlShift, "Select Shift, ShiftName From VMsShiftProduction", True, "Shift", "ShiftName", ViewState("DBConnection"))

                SetInit()

                Session("AdvanceFilter") = ""
                If ddlReffType.SelectedValue = "Material" Then
                    lbred1.Visible = True
                Else : lbred1.Visible = False
                End If

                If Not Request.QueryString("transid") Is Nothing Then
                    If Request.QueryString("transid").ToString.Length > 1 Then
                        'lbStatus.Text = Request.QueryString("transid").ToString
                        'Exit Sub
                        ddlRange.SelectedValue = "0"
                        CurrFilter = tbFilter.Text
                        Value = ddlField.SelectedValue
                        tbFilter.Text = Request.QueryString("transid").ToString
                        ddlField.SelectedValue = "TransNmbr"
                        btnSearch_Click(Nothing, Nothing)
                        tbFilter.Text = CurrFilter
                        ddlField.SelectedValue = Value
                    End If
                End If
            End If
           
            lbStatus.Text = ""
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If
            ' hasil dari search dialog
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnProdSrc" Then
                    '"Product_Code, Product_Name, Unit, Specification, Location, Location_Name, Qn_Hand"
                    tbProdSrcCode.Text = Session("Result")(0).ToString
                    tbProdSrcName.Text = Session("Result")(1).ToString + " " + TrimStr(Session("Result")(3).ToString)
                    tbUnitSrc.Text = Session("Result")(2).ToString
                    tbProdDestCode.Text = tbProdSrcCode.Text
                    tbProdDestName.Text = tbProdSrcName.Text
                    ddlLocationDest.SelectedValue = ddlLocationSrc.SelectedValue
                    ddlLocationDest.SelectedItem.Text = ddlLocationSrc.SelectedItem.Text
                    tbUnitDest.Text = tbUnitSrc.Text
                    lbQtyPackSrc.Text = Session("Result")(4).ToString
                    lbQtyPackDest.Text = Session("Result")(4).ToString
                    lbQtyM2Src.Text = Session("Result")(5).ToString
                    lbQtyM2Dest.Text = Session("Result")(5).ToString
                    BindToDropList(ddlCostCtrSrc, Session("Result")(6).ToString)
                    BindToDropList(ddlCostCtrDest, Session("Result")(6).ToString)
                    tbProdSrcCode.Focus()
                    GetInfo(tbProdSrcCode.Text)
                End If
                If ViewState("Sender") = "btnReffNo" Then
                    tbReffNo.Text = Session("Result")(0).ToString
                    ddlWrhsArea.SelectedValue = Session("Result")(1).ToString
                    If Request.QueryString("ContainerId").ToString = "TransMaterialID" Then
                        FillCombo(ddlWrhsSrc, "EXEC S_GetWrhsUserAreaF " + QuotedStr(ViewState("UserId").ToString) + "," + QuotedStr(ddlWrhsArea.SelectedValue) + ", ' And Wrhs_Type in (''Owner'', ''Deposit Out'') '", True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection"))
                        FillCombo(ddlWrhsDest, "EXEC S_GetWrhsUserAreaF " + QuotedStr(ViewState("UserId").ToString) + "," + QuotedStr(ddlWrhsArea.SelectedValue) + ", ' And Wrhs_Type = ''Production''' ", True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection"))
                        ddlWrhsArea.Enabled = True
                        ddlWrhsArea.Focus()
                    Else
                        FillCombo(ddlWrhsSrc, "EXEC S_GetWrhsUserAreaF " + QuotedStr(ViewState("UserId").ToString) + "," + QuotedStr(ddlWrhsArea.SelectedValue) + ", ' And Wrhs_Type = ''Production'''", True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection"))
                        FillCombo(ddlWrhsDest, "EXEC S_GetWrhsUserAreaF " + QuotedStr(ViewState("UserId").ToString) + "," + QuotedStr(ddlWrhsArea.SelectedValue) + ", ' And Wrhs_Type in (''Owner'', ''Deposit Out'')' ", True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection"))
                        ddlWrhsArea.Enabled = False
                        ddlWrhsDest.Focus()
                    End If
                    ddlWrhsSrc.Enabled = ddlWrhsArea.Enabled
                    tbSubledSrc.Enabled = ddlWrhsArea.Enabled
                    'tbSubledSrcName.Enabled = ddlWrhsArea.Enabled
                    ddlWrhsSrc.SelectedValue = Session("Result")(2).ToString
                    tbRemark.Text = Session("Result")(3).ToString
                    tbCodeFG.Text = Session("Result")(4).ToString
                    tbNameFG.Text = Session("Result")(5).ToString

                End If
                If ViewState("Sender") = "btnProdDest" Then
                    tbProdDestCode.Text = Session("Result")(0).ToString
                    tbProdDestName.Text = Session("Result")(1).ToString + " " + Trim(Session("Result")(3).ToString)
                    tbUnitDest.Text = Session("Result")(2).ToString
                    tbProdDestCode.Focus()
                End If
                If ViewState("Sender") = "btnSubLedSrc" Then
                    tbSubledSrc.Text = Session("Result")(0).ToString
                    tbSubledSrcName.Text = Session("Result")(1).ToString
                    tbSubledSrc.Focus()
                End If
                If ViewState("Sender") = "btnSubLedDest" Then
                    tbSubledDest.Text = Session("Result")(0).ToString
                    tbSubledDestName.Text = Session("Result")(1).ToString
                    tbSubledDest.Focus()
                End If
                If ViewState("Sender") = "btnGetDt" Then
                    Dim drResult As DataRow
                    For Each drResult In Session("Result").Rows
                        'insert
                        Dim dr As DataRow
                        dr = ViewState("Dt").NewRow
                        If tbReffNo.Text <> "" Then
                            dr("ProductDest") = drResult("Product_Code")
                            dr("ProductSrc") = drResult("Product_Code")
                            dr("QtyDest") = FormatFloat(drResult("Qty"), ViewState("DigitQty"))
                            dr("QtySrc") = FormatFloat(drResult("Qty"), ViewState("DigitQty"))
                            dr("LocationDest") = drResult("LocationDest").ToString
                            dr("LocationSrc") = drResult("LocationSrc").ToString
                            dr("Location_Dest_Name") = drResult("LocationDestName").ToString
                            dr("Location_Src_Name") = drResult("LocationSrcName").ToString
                            dr("QtyRollSrc") = FormatFloat(drResult("QtyRoll"), ViewState("DigitQty"))
                            dr("QtyRollDest") = FormatFloat(drResult("QtyRoll"), ViewState("DigitQty"))
                            dr("QtyM2Src") = FormatFloat(drResult("QtyM2"), ViewState("DigitQty"))
                            dr("QtyM2Dest") = FormatFloat(drResult("QtyM2"), ViewState("DigitQty"))
                            dr("CostCtrSrc") = drResult("CostCtr")
                            dr("CostCtrSrcName") = drResult("CostCtr_Name")
                            dr("CostCtrDest") = drResult("CostCtr")
                            dr("CostCtrDestName") = drResult("CostCtr_Name")
                        Else
                            dr("ProductDest") = drResult("Product_Code")
                            dr("ProductSrc") = drResult("Product_Code")
                            dr("QtyDest") = FormatNumber(0, ViewState("DigitQty")) 'FormatNumber(drResult("On_Hand"), ViewState("DigitQty"))
                            dr("QtySrc") = FormatNumber(0, ViewState("DigitQty")) 'FormatNumber(drResult("On_Hand"), ViewState("DigitQty"))
                            dr("LocationDest") = ""
                            dr("LocationSrc") = ""
                            dr("Location_Dest_Name") = ""
                            dr("Location_Src_Name") = ""
                            dr("QtyRollSrc") = FormatNumber(0, ViewState("DigitQty"))
                            dr("QtyRollDest") = FormatNumber(0, ViewState("DigitQty"))
                            dr("QtyM2Src") = FormatNumber(0, ViewState("DigitQty"))
                            dr("QtyM2Dest") = FormatNumber(0, ViewState("DigitQty"))
                            dr("CostCtrSrc") = drResult("CostCtr")
                            dr("CostCtrSrcName") = drResult("CostCtrName")
                            dr("CostCtrDest") = drResult("CostCtr")
                            dr("CostCtrDestName") = drResult("CostCtrName")
                        End If
                        dr("Product_Dest_Name") = drResult("Product_Name")
                        dr("Product_Src_Name") = drResult("Product_Name")

                        dr("Unitdest") = drResult("Unit")
                        dr("UnitSrc") = drResult("Unit")
                        dr("UnitPackSrc") = drResult("UnitPack")
                        dr("UnitPackDest") = drResult("UnitPack")
                        dr("UnitM2Src") = drResult("UnitM2")
                        dr("UnitM2Dest") = drResult("UnitM2")
                        'If tbReffNo.Text <> "" Then
                        '    dr("Remark") = drResult("Remark")
                        'End If
                        ViewState("Dt").Rows.Add(dr)
                    Next
                    BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                End If
                Session("Result") = Nothing
                ViewState("Sender") = Nothing
                Session("filter") = Nothing
                Session("Column") = Nothing
            End If

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

    Private Sub SetInit()
        FillRange(ddlRange)
        ViewState("SortExpression") = Nothing
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
        If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
            ddlCommand.Items.Add("Print")
            ddlCommand2.Items.Add("Print")
        End If
        'Kalo ada java script
        'tbQty.Attributes.Add("ReadOnly", "True")
        If Request.QueryString("ContainerId").ToString = "TransMaterialID" Then
            ViewState("FgType") = "FG"
            ViewState("StrKode") = "TRGM"
            lbTrans.Text = "TRANSFER MATERIAL"
            ViewState("TransferType") = "Material"
            ddlReffType.SelectedValue="Material"
            tbType.Text = "Material"
            btnGetDt.Visible = True
            'btnReffNo.Visible = True
            ddlReffType.Enabled = True
            tbCodeFG.Visible = True
            tbNameFG.Visible = True
            lbTitik.Visible = True
            lbRM.Visible = True
            tbProdDestCode.Visible = False
            tbProdDestName.Visible = tbProdDestCode.Visible
            btnProdDest.Visible = tbProdDestCode.Visible
            ddlCostCtrDest.Visible = tbProdDestCode.Visible
            tbQtyDest.Visible = tbProdDestCode.Visible
            tbUnitDest.Visible = tbProdDestCode.Visible
            tbQtyM2Dest.Visible = tbProdDestCode.Visible
            tbQtyRollDest.Visible = tbProdDestCode.Visible
            lbQtyPackDest.Visible = tbProdDestCode.Visible
            lbQtyM2Dest.Visible = tbProdDestCode.Visible
            FillCombo(ddlWrhsSrc, "EXEC S_GetWrhsUserAreaF " + QuotedStr(ViewState("UserId").ToString) + "," + QuotedStr(ddlWrhsArea.SelectedValue) + ", ' And Wrhs_Type = ''Production'''", True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection"))
            FillCombo(ddlWrhsDest, "EXEC S_GetWrhsUserAreaF " + QuotedStr(ViewState("UserId").ToString) + "," + QuotedStr(ddlWrhsArea.SelectedValue) + ", ' And Wrhs_Type in (''Owner'', ''Deposit Out'')' ", True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection"))

        ElseIf Request.QueryString("ContainerId").ToString = "TransReturnID" Then
            ViewState("FgType") = "FG"
            ViewState("StrKode") = "TRR"
            tbProdDestCode.Enabled = False
            tbProdDestName.Enabled = False
            tbQtyDest.Enabled = False
            lbTrans.Text = "TRANSFER RETURN MATERIAL"
            ViewState("TransferType") = "Return"
            tbType.Text = "Return"
            ddlReffType.SelectedValue = "Return"
            ddlReffType.Enabled = True
            btnGetDt.Visible = True
            'btnReffNo.Visible = True
            tbCodeFG.Visible = True
            tbNameFG.Visible = True
            lbTitik.Visible = True
            lbRM.Visible = True
            FillCombo(ddlWrhsSrc, "EXEC S_GetWrhsUserAreaF " + QuotedStr(ViewState("UserId").ToString) + "," + QuotedStr(ddlWrhsArea.SelectedValue) + ", ' And Wrhs_Type = ''Production'''", True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection"))
            FillCombo(ddlWrhsDest, "EXEC S_GetWrhsUserAreaF " + QuotedStr(ViewState("UserId").ToString) + "," + QuotedStr(ddlWrhsArea.SelectedValue) + ", ' And Wrhs_Type in (''Owner'', ''Deposit Out'')' ", True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection"))

        ElseIf Request.QueryString("ContainerId").ToString = "TransProjectID" Then
            ViewState("FgType") = "FG"
            ViewState("StrKode") = "TRP"
            tbProdDestCode.Enabled = False
            tbProdDestName.Enabled = False
            tbQtyDest.Enabled = False
            lbTrans.Text = "TRANSFER PROJECT"
            ViewState("TransferType") = "Project"
            tbType.Text = "Project"
            ddlReffType.SelectedValue = "Project"
            ddlReffType.Enabled = True
            btnGetDt.Visible = True
            'btnReffNo.Visible = True
            tbCodeFG.Visible = False
            tbNameFG.Visible = False
            lbTitik.Visible = False
            lbRM.Visible = False
        End If
    End Sub

    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim StrFilter, GetStringHd1 As String
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
            GetStringHd1 = "Select * From v_sttransferHd WHERE TransferType = " + QuotedStr(ViewState("TransferType").ToString)
            DT = BindDataTransaction(GetStringHd1, StrFilter, ViewState("DBConnection").ToString)
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
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "TransDate DESC"
            End If
            DV.Sort = ViewState("SortExpression")
            GridView1.DataSource = DV
            GridView1.DataBind()
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT * From v_sttransferdt WHERE TransNmbr = " + QuotedStr(Nmbr) + " "
    End Function

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
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

    Protected Sub BtnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGo.Click, btnGo2.Click
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
            If ActionValue = "Print" Then
                Dim GVR As GridViewRow
                Dim CB As CheckBox
                Dim Pertamax As Boolean

                Pertamax = True
                Result = ""

                For Each GVR In GridView1.Rows
                    CB = GVR.FindControl("cbSelect")
                    If CB.Checked Then
                        ListSelectNmbr = GVR.Cells(2).Text
                        If Pertamax Then
                            Result = "'''" + ListSelectNmbr + "''"
                            Pertamax = False
                        Else
                            Result = Result + ",''" + ListSelectNmbr + "''"
                        End If
                    End If
                Next
                Result = Result + "'"
                Session("DBConnection") = ViewState("DBConnection")
                Session("SelectCommand") = "EXEC S_STFormTransfer " + Result + "," + QuotedStr(lbTrans.Text)
                If ViewState("TransferType").ToString = "Location" Or ViewState("TransferType").ToString = "Blokir" Then
                    Session("ReportFile") = ".../../../Rpt/FormSTTransferLoc.frx"
                Else : Session("ReportFile") = ".../../../Rpt/FormSTTransferRM.frx"
                End If
                AttachScript("openprintdlg();", Page, Me.GetType)
            Else
                Status = CekStatus(ActionValue)

            ListSelectNmbr = ""
            GetListCommand(Status, GridView1, "3,2", ListSelectNmbr, Nmbr, lbStatus.Text)
            If ListSelectNmbr = "" Then Exit Sub
            For j = 0 To (Nmbr.Length - 1)
                If Nmbr(j) = "" Then
                    Exit For
                Else
                    Result = ExecSPCommandGo(ActionValue, "S_STTransfer", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                    If Trim(Result) <> "" Then
                        lbStatus.Text = lbStatus.Text + Result + " <br/>"

                        End If
                    End If
                Next
                BindData("TransNmbr in (" + ListSelectNmbr + ")")
            End If
        Catch ex As Exception
            lbStatus.Text = "Go Command Error : " + ex.ToString
        End Try
    End Sub

    Private Sub EnableHd(ByVal State As Boolean)
        Try
            'btnGetDt.Visible = State
            'ddlWrhsArea.Enabled = State
            'ddlWrhsSrc.Enabled = State
            'tbSubledSrc.Enabled = State And tbFgSubledSrc.Text.Trim <> "N"
            'btnSubledSrc.Visible = tbSubledSrc.Enabled
            ddlShift.Enabled = State
            ddlReffType.Enabled = State
            btnReffNo.Visible = State
            ddlWrhsDest.Enabled = State
            tbSubledDest.Enabled = State And tbFgSubledDest.Text.Trim <> "N"
            BtnSubledDest.Visible = tbSubledDest.Enabled
        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDt(ByVal Referens As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt") = Nothing
            dt = SQLExecuteQuery(GetStringDt(Referens), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt") = dt
            BindGridDt(dt, GridDt)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnHome.Click
        Try
            MovePanel(pnlInput, PnlHd)
        Catch ex As Exception
            lbStatus.Text = "btn back Error : " + ex.ToString
        End Try
    End Sub

    Private Sub SaveAll()
        Dim SQLString As String
        Dim I As Integer
        Try
            If pnlDt.Visible = False Then
                lbStatus.Text = "Detail Data must be saved first"
                Exit Sub
            End If
            If Not (ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit") Then
                Exit Sub
            Else
                ViewState("StateSave") = ViewState("StateHd")
                ViewState("StateHd") = "View"
            End If
            'Save Hd
            If ViewState("StateSave") = "Insert" Then
                tbTransNo.Text = GetAutoNmbr(ViewState("StrKode").ToString, "N", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), ddlWrhsSrc.SelectedValue, ViewState("DBConnection").ToString)
                SQLString = "INSERT INTO stctransferhd(TransNmbr,Status,TransDate,WrhsArea,WrhsSrc,FgSrcSubLed,SrcSubLed,WrhsDest,FgDestSubLed,DestSubLed,Operator,MemoMBD,Remark,UserPrep,DatePrep,TransferType, ReffType, ReffNmbr, CarNo, Driver, " + _
                " Shift ) " + _
                "SELECT " + QuotedStr(tbTransNo.Text) + ", 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', " + _
                QuotedStr(ddlWrhsArea.SelectedValue) + ", " + QuotedStr(ddlWrhsSrc.SelectedValue) + ", " + QuotedStr(tbFgSubledSrc.Text) + ", " + QuotedStr(tbSubledSrc.Text) + ", " + QuotedStr(ddlWrhsDest.SelectedValue) + ", " + _
                QuotedStr(tbFgSubledDest.Text) + ", " + QuotedStr(tbSubledDest.Text) + "," + QuotedStr(tbOperator.Text) + ", NULL," + QuotedStr(tbRemark.Text) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", getDate(), " + QuotedStr(ViewState("TransferType").ToString) + "," + QuotedStr(ddlReffType.SelectedValue) + "," + QuotedStr(tbReffNo.Text) + "," + QuotedStr(tbCarNo.Text) + "," + QuotedStr(tbDriver.Text) + ", " + _
                QuotedStr(ddlShift.SelectedValue)
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM stctransferhd WHERE TransNmbr = " + QuotedStr(tbTransNo.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE stctransferhd SET TransDate = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + _
                "', WrhsArea = " + QuotedStr(ddlWrhsArea.Text) + _
                ", WrhsSrc = " + QuotedStr(ddlWrhsSrc.SelectedValue) + ", SrcSubled = " + QuotedStr(tbSubledSrc.Text) + _
                ", FgSrcSubLed = " + QuotedStr(tbFgSubledSrc.Text) + ", WrhsDest = " + QuotedStr(ddlWrhsDest.Text) + _
                ", DestSubLed = " + QuotedStr(tbSubledDest.Text) + ", FgDestSubLed = " + QuotedStr(tbFgSubledDest.Text) + _
                ", Operator = " + QuotedStr(tbOperator.Text) + ", Remark = " + QuotedStr(tbRemark.Text) + _
                ", ReffType = " + QuotedStr(ddlReffType.SelectedValue) + ", ReffNmbr = " + QuotedStr(tbReffNo.Text) + _
                ", CarNo = " + QuotedStr(tbCarNo.Text) + ", Driver = " + QuotedStr(tbDriver.Text) + ", " + _
                " DateAppr = getDate()" + _
                ", Shift = " + QuotedStr(ddlShift.Text) + _
                " WHERE TransNmbr = " + QuotedStr(tbTransNo.Text)
            End If
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            'update Primary Key on Dt
            Dim Row As DataRow()

            Row = ViewState("Dt").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("Transnmbr") = tbTransNo.Text
                Row(I).EndEdit()
            Next

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString 'System.Configuration.ConfigurationManager.AppSettings.Get("ConString")
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, ProductSrc, Locationsrc, ProductDest, LocationDest, QtySrc, UnitSrc, QtyDest, UnitDest, QtyRollSrc, QtyM2Src, QtyRollDest, QtyM2Dest, CostCtrSrc, CostCtrDest, Remark FROM STCTransferDt WHERE TransNmbr = '" & ViewState("Reference") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
                    "UPDATE stctransferdt SET ProductSrc = @ProductSrc, LocationSrc = @LocationSrc, ProductDest = @ProductDest, LocationDest = @LocationDest, QtySrc = @QtySrc, UnitSrc = @UnitSrc, QtyDest = @QtyDest, UnitDest = @UnitDest, Remark = @Remark,  " + _
                    " QtyRollSrc = @QtyRollSrc, QtyM2Src = @QtyM2Src, QtyRollDest = @QtyRollDest, QtyM2Dest = @QtyM2Dest, CostCtrSrc =@CostCtrSrc, CostCtrDest = @CostCtrDest WHERE TransNmbr = '" & ViewState("Reference") & "' AND ProductSrc = @OldProductSrc AND LocationSrc = @OldLocationSrc AND ProductDest = @OldProductDest AND LocationDest = @OldLocationDest ", con)
            ' Define output parameters.
            Update_Command.Parameters.Add("@ProductSrc", SqlDbType.VarChar, 20, "ProductSrc")
            Update_Command.Parameters.Add("@LocationSrc", SqlDbType.VarChar, 10, "LocationSrc")
            Update_Command.Parameters.Add("@ProductDest", SqlDbType.VarChar, 20, "ProductDest")
            Update_Command.Parameters.Add("@LocationDest", SqlDbType.VarChar, 10, "LocationDest")
            Update_Command.Parameters.Add("@QtySrc", SqlDbType.Float, 18, "QtySrc")
            Update_Command.Parameters.Add("@UnitSrc", SqlDbType.VarChar, 5, "UnitSrc")
            Update_Command.Parameters.Add("@QtyDest", SqlDbType.Float, 18, "QtyDest")
            Update_Command.Parameters.Add("@UnitDest", SqlDbType.VarChar, 5, "UnitDest")
            Update_Command.Parameters.Add("@QtyRollSrc", SqlDbType.Float, 18, "QtyRollSrc")
            Update_Command.Parameters.Add("@QtyM2Src", SqlDbType.Float, 18, "QtyM2Src")
            Update_Command.Parameters.Add("@QtyRollDest", SqlDbType.Float, 18, "QtyRollDest")
            Update_Command.Parameters.Add("@QtyM2Dest", SqlDbType.Float, 18, "QtyM2Dest")
            Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 255, "Remark")
            Update_Command.Parameters.Add("@CostCtrSrc", SqlDbType.VarChar, 20, "CostCtrSrc")
            Update_Command.Parameters.Add("@CostCtrDest", SqlDbType.VarChar, 20, "CostCtrDest")
            ' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@OldProductSrc", SqlDbType.VarChar, 20, "ProductSrc")
            param.SourceVersion = DataRowVersion.Original
            param = Update_Command.Parameters.Add("@OldLocationSrc", SqlDbType.VarChar, 10, "LocationSrc")
            param.SourceVersion = DataRowVersion.Original
            param = Update_Command.Parameters.Add("@OldProductDest", SqlDbType.VarChar, 20, "ProductDest")
            param.SourceVersion = DataRowVersion.Original
            param = Update_Command.Parameters.Add("@OldLocationDest", SqlDbType.VarChar, 10, "LocationDest")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM stctransferdt WHERE TransNmbr = '" & ViewState("Reference") & "' AND ProductSrc = @ProductSrc AND LocationSrc = @LocationSrc AND ProductDest = @ProductDest AND LocationDest = @LocationDest", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@ProductSrc", SqlDbType.VarChar, 20, "ProductSrc")
            param.SourceVersion = DataRowVersion.Original
            param = Delete_Command.Parameters.Add("@LocationSrc", SqlDbType.VarChar, 10, "LocationSrc")
            param.SourceVersion = DataRowVersion.Original
            param = Delete_Command.Parameters.Add("@ProductDest", SqlDbType.VarChar, 20, "ProductDest")
            param.SourceVersion = DataRowVersion.Original
            param = Delete_Command.Parameters.Add("@LocationDest", SqlDbType.VarChar, 10, "LocationDest")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("stctransferdt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt
        Catch ex As Exception
            Throw New Exception("Save All Data Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSaveTrans_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveTrans.Click
        Dim CurrFilter, Value As String
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            'If IsNothing(ViewState("Dt")) Then
            '    lbStatus.Text = MessageDlg("Detail must have at least 1 record")
            '    Exit Sub
            'End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
                Exit Sub
            End If
            For Each dr In ViewState("Dt").Rows
                If CekDt(dr, "ProductSrc,LocationSrc,ProductDest,LocationDest") = False Then
                    Exit Sub
                End If
            Next
            SaveAll()
            MovePanel(pnlInput, PnlHd)
            CurrFilter = tbFilter.Text
            Value = ddlField.SelectedValue
            tbFilter.Text = tbTransNo.Text
            ddlField.SelectedValue = "TransNmbr"
            btnSearch_Click(Nothing, Nothing)
            tbFilter.Text = CurrFilter
            ddlField.SelectedValue = Value
        Catch ex As Exception
            lbStatus.Text = "Save All Dt Error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ClearHd()
            Cleardt()
            tbDate.SelectedDate = ViewState("ServerDate") 'Today
            pnlDt.Visible = True
            BindDataDt("")
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            btnReffNo.Visible = True
            tbTransNo.Text = ""
            tbReffNo.Text = ""
            tbCodeFG.Text = ""
            tbNameFG.Text = ""
            ddlReffType.SelectedIndex = 0
            ddlWrhsArea.SelectedIndex = 0
            ddlWrhsSrc.SelectedIndex = 0
            ddlWrhsDest.SelectedIndex = 0
            tbFgSubledSrc.Text = "N"
            tbSubledSrc.Text = ""
            tbSubledSrc.Enabled = tbFgSubledSrc.Text <> "N"
            btnSubledSrc.Visible = tbSubledSrc.Enabled
            tbSubledSrc.Text = ""
            tbFgSubledDest.Text = "N"
            tbSubledDest.Text = ""
            tbSubledDest.Enabled = tbFgSubledDest.Text <> "N"
            BtnSubledDest.Visible = tbSubledDest.Enabled
            tbSubledDest.Text = ""
            tbCarNo.Text = ""
            tbDriver.Text = ""
            tbRemark.Text = ""
            tbOperator.Text = ViewState("UserId").ToString
            ddlShift.SelectedIndex = 0
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbProdSrcCode.Text = ""
            tbProdSrcName.Text = ""
            ddlLocationSrc.SelectedIndex = -1
            tbUnitSrc.Text = ""
            tbQtySrc.Text = FormatNumber(0, ViewState("DigitQty"))
            tbProdDestCode.Text = ""
            tbProdDestName.Text = ""
            ddlLocationDest.SelectedIndex = -1
            tbUnitDest.Text = ""
            tbRemarkDt.Text = ""
            tbQtyDest.Text = FormatNumber(0, ViewState("DigitQty"))
            ddlCostCtrSrc.SelectedValue = ""
            ddlCostCtrDest.SelectedValue = ""
            lbQtyPackSrc.Text = ""
            lbQtyM2Src.Text = ""
            tbQtyRollSrc.Text = FormatFloat(0, ViewState("DigitQty"))
            tbQtyM2Src.Text = FormatFloat(0, ViewState("DigitQty"))
            lbQtyPackDest.Text = ""
            lbQtyM2Dest.Text = ""
            tbQtyRollDest.Text = FormatFloat(0, ViewState("DigitQty"))
            tbQtyM2Dest.Text = FormatFloat(0, ViewState("DigitQty"))
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnExpand_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExpand.Click
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

    Protected Sub btnSaveAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveAll.Click
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            If IsNothing(ViewState("Dt")) Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
                Exit Sub
            End If
            For Each dr In ViewState("Dt").Rows
                If CekDt(dr, "ProductSrc,LocationSrc,ProductDest,LocationDest") = False Then
                    Exit Sub
                End If
            Next
            SaveAll()
            newTrans()
        Catch ex As Exception
            lbStatus.Text = "btn save all error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt.Click, btnAddDt2.Click
        Cleardt()
        If CekHd() = False Then
            Exit Sub
        End If
        If ViewState("SetLocation") Then
            FillCombo(ddlLocationSrc, "EXEC S_GetWrhsLocation " + QuotedStr(ddlWrhsSrc.SelectedValue), True, "Location_Code", "Location_Name", ViewState("DBConnection"))
            FillCombo(ddlLocationDest, "EXEC S_GetWrhsLocation " + QuotedStr(ddlWrhsDest.SelectedValue), True, "Location_Code", "Location_Name", ViewState("DBConnection"))
            ViewState("SetLocation") = False
        End If
        ViewState("StateDt") = "Insert"
        MovePanel(pnlDt, pnlEditDt)
        EnableHd(False)
        StatusButtonSave(False)
    End Sub

    Function CekHd() As Boolean
        Try
            If ddlShift.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Shift must have value")
                ddlShift.Focus()
                Return False
            End If
            If ddlReffType.SelectedValue = "Material" Then
                If tbReffNo.Text = "" Then
                    lbStatus.Text = MessageDlg("Reff No must have value")
                    tbReffNo.Focus()
                    Return False
                End If
            End If
            If ddlWrhsArea.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Warehouse Area must have value")
                ddlWrhsArea.Focus()
                Return False
            End If
            If ddlWrhsSrc.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Warehouse Src must have value")
                ddlWrhsSrc.Focus()
                Return False
            End If
            If tbSubledSrc.Text.Trim = "" And tbFgSubledSrc.Text.Trim <> "N" Then
                lbStatus.Text = MessageDlg("SubLed Src must have value")
                tbSubledSrc.Focus()
                Return False
            End If
            If ddlWrhsDest.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Warehouse Dest must have value")
                ddlWrhsDest.Focus()
                Return False
            End If
            If tbSubledDest.Text.Trim = "" And tbFgSubledDest.Text.Trim <> "N" Then
                lbStatus.Text = MessageDlg("SubLed Dest must have value")
                tbSubledDest.Focus()
                Return False
            End If
            'If CInt(Session(Request.QueryString("KeyId"))("Year")) <> Year(tbDate.SelectedValue) Or CInt(Session(Request.QueryString("KeyId"))("Period")) <> Month(tbDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(Session(Request.QueryString("KeyId"))("Period")) + " " + Session(Request.QueryString("KeyId"))("Year").ToString.Trim)
            '    Exit Function
            'End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Hd Error : " + ex.ToString)
        End Try
    End Function

    Function CekDt(Optional ByVal Dr As DataRow = Nothing, Optional ByVal FieldKey As String = "") As Boolean
        Try
            If Not (Dr Is Nothing) Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("ProductSrc").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Product Src Must Have Value")
                    Return False
                End If
                If Dr("LocationSrc").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Location Src Must Have Value")
                    Return False
                End If
                If Dr("ProductDest").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Product Dest Must Have Value")
                    Return False
                End If
                If Dr("LocationDest").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Location Dest Must Have Value")
                    Return False
                End If
                If Dr("UnitSrc").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Unit Src Must Have Value")
                    Return False
                End If
                If Dr("CostCtrSrc").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Cost Center Src Must Have Value")
                    Return False
                End If
                If CFloat(Dr("QtySrc").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Src Must Have Value")
                    Return False
                End If
                If Dr("UnitDest").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Unit Dest Must Have Value")
                    Return False
                End If
                If CFloat(Dr("QtyDest")) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Dest Must Have Value")
                    Return False
                End If
                If Dr("CostCtrDest").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Cost Center Destination Must Have Value")
                    Return False
                End If
                'If (ViewState("StrKode") = "TRGF") Or (ViewState("StrKode") = "TRGM") Then
                If (ViewState("StrKode") = "TRG") Then
                    If tbProdSrcCode.Text.Trim = tbProdDestCode.Text.Trim Then
                        lbStatus.Text = MessageDlg("Destination product cannot be same with source product")
                        tbProdDestCode.Focus()
                    End If
                End If
                'If (ViewState("StrKode") = "TRLF") Or (ViewState("StrKode") = "TRLM") Or (ViewState("StrKode") = "TRBF") Or (ViewState("StrKode") = "TRBM") Then
                If (ViewState("StrKode") = "TRL") Then
                    If (ddlLocationSrc.SelectedValue = ddlLocationDest.SelectedValue) And (ddlWrhsSrc.SelectedValue = ddlWrhsDest.SelectedValue) Then
                        lbStatus.Text = MessageDlg("Destination location cannot be same with source")
                        ddlLocationDest.Focus()
                    End If
                End If
            Else
                If tbProdSrcCode.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Product Src Must Have Value")
                    tbProdSrcCode.Focus()
                    Return False
                End If
                If ddlLocationSrc.SelectedValue.Trim = "" Then
                    lbStatus.Text = MessageDlg("Location Src Must Have Value")
                    ddlLocationSrc.Focus()
                    Return False
                End If
		If ddlCostCtrSrc.SelectedValue.Trim = "" Then
                    lbStatus.Text = MessageDlg("Cost Center Src Must Have Value")
                    ddlCostCtrSrc.Focus()
                    Return False
                End If
                If tbProdDestCode.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Product Dest Must Have Value")
                    tbProdDestCode.Focus()
                    Return False
                End If
                If ddlLocationDest.SelectedValue.Trim = "" Then
                    lbStatus.Text = MessageDlg("Location Dest Must Have Value")
                    ddlLocationDest.Focus()
                    Return False
                End If
                If CFloat(tbQtySrc.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Src Must Have Value")
                    tbQtySrc.Focus()
                    Return False
                End If
                If tbUnitSrc.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Unit Src Must Have Value")
                    tbUnitSrc.Focus()
                    Return False
                End If
		If ddlCostCtrDest.SelectedValue.Trim = "" Then
                    lbStatus.Text = MessageDlg("Cost Center Destination Must Have Value")
                    ddlCostCtrDest.Focus()
                    Return False
                End If
                If CFloat(tbQtyDest.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Dest Must Have Value")
                    tbQtyDest.Focus()
                    Return False
                End If
                If tbUnitDest.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Unit Dest Must Have Value")
                    tbUnitDest.Focus()
                    Return False
                End If
                'If ViewState("StrKode") = "TRR" Then
                '    If tbProdSrcCode.Text.Trim = tbProdDestCode.Text.Trim Then
                '        lbStatus.Text = MessageDlg("Destination product cannot be same with source product")
                '        tbProdDestCode.Focus()
                '        Return False
                '    End If
                'End If
                If (ViewState("StrKode") = "TRM") Then
                    If (ddlLocationSrc.SelectedValue = ddlLocationDest.SelectedValue) And (ddlWrhsSrc.SelectedValue = ddlWrhsDest.SelectedValue) Then
                        lbStatus.Text = MessageDlg("Destination location cannot be same with source")
                        ddlLocationDest.Focus()
                        Return False
                    End If
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        CheckAll(GridView1, sender)
    End Sub

    Protected Sub LbAdvSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LbAdvSearch.Click
        Dim FDateName, FDateValue, FilterName, FilterValue As String
        Try
            FDateName = "Date"
            FDateValue = "TransDate"
            FilterName = "Reference, Date, Status, Warehouse Area, Warehouse Src, Subled Src, Warehouse Dest, Subled Dest, Operator, FgReport, Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), Status, wrhs_area_Name, Wrhs_Src_Name, Subled_Src_Name, Wrhs_Dest_Name, Subled_Dest_Name, Operator, FgReport, Remark"
            Session("DateFieldName") = FDateName.Split(",")
            Session("DateFieldValue") = FDateValue.Split(",")
            Session("FieldName") = FilterName.Split(",")
            Session("FieldValue") = FilterValue.Split(",")
            AttachScript("OpenFilterCriteria();", Page, Me.GetType())
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
        Dim cekMenu As String
        Try
            If Not (e.CommandName = "Sort" Or e.CommandName = "Page") Then
                index = Convert.ToInt32(e.CommandArgument)
                GVR = GridView1.Rows(index)
            End If

            If e.CommandName = "Go" Then
                DDL = GridView1.Rows(index).FindControl("ddl")
                If DDL.SelectedValue = "View" Then
                    MovePanel(PnlHd, pnlInput)
                    ViewState("Reference") = GVR.Cells(2).Text
                    GridDt.PageIndex = 0
                    GridDt.Columns(1).Visible = False
                    FillTextBoxHd(ViewState("Reference"))
                    BindDataDt(ViewState("Reference"))
                    ViewState("StateHd") = "View"

                    ModifyInput2(False, pnlInput, pnlDt, GridDt)
                    btnHome.Visible = True
                ElseIf DDL.SelectedValue = "Edit" Then
                    If GVR.Cells(3).Text = "H" Or GVR.Cells(3).Text = "G" Then
                        cekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If cekMenu <> "" Then
                            lbStatus.Text = cekMenu
                            Exit Sub
                        End If
                        ViewState("SetLocation") = True
                        MovePanel(PnlHd, pnlInput)
                        ViewState("Reference") = GVR.Cells(2).Text
                        GridDt.PageIndex = 0
                        BindDataDt(ViewState("Reference"))
                        GridDt.Columns(1).Visible = True
                        FillTextBoxHd(ViewState("Reference"))
                        ViewState("StateHd") = "Edit"
                        FillCombo(ddlLocationAllSrc, "EXEC S_GetWrhsLocation " + QuotedStr(ddlWrhsSrc.SelectedValue), True, "Location_Code", "Location_Name", ViewState("DBConnection"))

                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        btnHome.Visible = False
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    Else
                        lbStatus.Text = MessageDlg("Data must Hold or Get Approval to edit")
                        Exit Sub
                    End If
                ElseIf DDL.SelectedValue = "Print" Then
                    Try
                        cekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If cekMenu <> "" Then
                            lbStatus.Text = cekMenu
                            Exit Sub
                        End If
                        Session("DBConnection") = ViewState("DBConnection")
                        Session("SelectCommand") = "EXEC S_STFormTransfer ''" + QuotedStr(GVR.Cells(2).Text) + "''," + QuotedStr(lbTrans.Text)
                        'lbStatus.Text = Session("SelectCommand")
                        'Exit Sub
                        If ViewState("TransferType").ToString = "Location" Or ViewState("TransferType").ToString = "Blokir" Then
                            Session("ReportFile") = ".../../../Rpt/FormSTTransferLoc.frx"
                        Else : Session("ReportFile") = ".../../../Rpt/FormSTTransferRM.frx"
                        End If
                        
                        AttachScript("openprintdlg();", Page, Me.GetType)
                    Catch ex As Exception
                        lbStatus.Text = "btn print Error = " + ex.ToString
                    End Try
                ElseIf DDL.SelectedValue = "Print Sample" Then
                    Try
                        cekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If cekMenu <> "" Then
                            lbStatus.Text = cekMenu
                            Exit Sub
                        End If
                        Session("DBConnection") = ViewState("DBConnection")
                        Session("SelectCommand") = "EXEC S_STFormTransferSample ''" + QuotedStr(GVR.Cells(2).Text) + "''," + QuotedStr(lbTrans.Text)
                        'lbStatus.Text = Session("SelectCommand")
                        'Exit Sub
                        If ViewState("TransferType").ToString = "Location" Or ViewState("TransferType").ToString = "Blokir" Then
                            Session("ReportFile") = ".../../../Rpt/FormSTTransferSample.frx"
                        Else : Session("ReportFile") = ".../../../Rpt/FormSTTransferSample.frx"
                        End If

                        AttachScript("openprintdlg();", Page, Me.GetType)
                    Catch ex As Exception
                        lbStatus.Text = "btn print Error = " + ex.ToString
                    End Try
                ElseIf DDL.SelectedValue = "Input Lot No" Then
                    'Dim i As Integer
                    Dim paramgo As String
                    Try
                        paramgo = "TI|" + GVR.Cells(2).Text
                        Dim SQLString As String
                        SQLString = "Select Product from V_STStockLotReff WHERE TransNmbr = " + QuotedStr(GVR.Cells(2).Text) + " and Type = 'TI' "
                        
                        Dim dt As DataTable
                        dt = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)
                        If dt.Rows.Count > 0 Then
                            Dim i As Integer
                            Dim pageurl As String
                            pageurl = HttpContext.Current.Request.Url.AbsoluteUri
                            i = pageurl.IndexOf("&transid")
                            If i > 0 Then
                                pageurl = pageurl.Substring(0, i)
                            End If
                            'lbStatus.Text = "***" + pageurl + "****"
                            'Exit Sub
                            Session("PrevPageStock") = pageurl  'HttpContext.Current.Request.Url.AbsoluteUri
                            AttachScript("OpenTransactionSelf('TrStockLot', '" + Request.QueryString("KeyId") + "', '" + paramgo + "' );", Page, Me.GetType())
                        Else
                            lbStatus.Text = "Transaction does not need input Lot No"
                        End If

                    Catch ex As Exception
                        lbStatus.Text = "Print Error : " + ex.ToString
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

    Protected Sub GridDt_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridDt.PageIndexChanging
        Try
            GridDt.PageIndex = e.NewPageIndex
            GridDt.DataSource = ViewState("Dt")
            GridDt.DataBind()
        Catch ex As Exception
            lbStatus.Text = "Grid dT Page Index Change Error : " + ex.ToString + " page index : " + e.NewPageIndex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridDt.RowCommand
        Try
            If e.CommandName = "Insert" Then
                btnAddDt_Click(Nothing, Nothing)
                'btnGetDt.Enabled = False
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub

    Dim QtySrc As Decimal = 0
    Dim QtySrcR As Decimal = 0
    Dim QtySrcM As Decimal = 0

    Dim QtyDest As Decimal = 0
    Dim QtyDestR As Decimal = 0
    Dim QtyDestM As Decimal = 0
    Protected Sub GridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt.RowDataBound
        Try
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "ProductSrc")) Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                    '' add the UnitPrice and QuantityTotal to the running total variables
                    'CrHome += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CreditHome"))
                    ''CrForex += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CreditForex"))
                ElseIf e.Row.RowType = DataControlRowType.Footer Then
                    QtySrc = GetTotalSum(ViewState("Dt"), "QtySrc")
                    QtySrcR = GetTotalSum(ViewState("Dt"), "QtyRollSrc")
                    QtySrcM = GetTotalSum(ViewState("Dt"), "QtyM2Src")
                    QtyDest = GetTotalSum(ViewState("Dt"), "QtyDest")
                    QtyDestR = GetTotalSum(ViewState("Dt"), "QtyRollDest")
                    QtyDestM = GetTotalSum(ViewState("Dt"), "QtyM2Dest")
                    e.Row.Cells(9).Text = "Total :"
                    ' for the Footer, display the running totals
                    e.Row.Cells(10).Text = FormatFloat(QtySrc, ViewState("DigitQty"))
                    e.Row.Cells(12).Text = FormatFloat(QtySrcR, ViewState("DigitQty"))
                    e.Row.Cells(14).Text = FormatFloat(QtySrcM, ViewState("DigitQty"))
                    'e.Row.Cells(17).Text = FormatFloat(QtyDest, ViewState("DigitQty"))
                    'e.Row.Cells(19).Text = FormatFloat(QtyDestR, ViewState("DigitQty"))
                    'e.Row.Cells(21).Text = FormatFloat(QtyDestM, ViewState("DigitQty"))
                End If
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Dim dr() As DataRow
        Dim lbSrc, lbDest, lbProdDest As Label
        Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
        lbSrc = GVR.FindControl("lbLocationSrc")
        lbDest = GVR.FindControl("lbLocationDest")
        lbProdDest = GVR.FindControl("lbProductDest")
        dr = ViewState("Dt").Select("ProductSrc+'|'+LocationSrc+'|'+ProductDest+'|'+LocationDest = " + QuotedStr(GVR.Cells(2).Text + "|" + TrimStr(lbSrc.Text) + "|" + lbProdDest.Text + "|" + TrimStr(lbDest.Text)))
        dr(0).Delete()
        BindGridDt(ViewState("Dt"), GridDt)
        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Dim lbSrc, lbDest, lbProdDest As Label
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            lbSrc = GVR.FindControl("lbLocationSrc")
            lbDest = GVR.FindControl("lbLocationDest")
            lbProdDest = GVR.FindControl("lbProductDest")
            ViewState("DtValue") = GVR.Cells(2).Text + "|" + TrimStr(lbSrc.Text) + "|" + lbProdDest.Text + "|" + TrimStr(lbDest.Text)
            If ViewState("SetLocation") Then
                FillCombo(ddlLocationSrc, "EXEC S_GetWrhsLocation " + QuotedStr(ddlWrhsSrc.SelectedValue), True, "Location_Code", "Location_Name", ViewState("DBConnection"))
                FillCombo(ddlLocationDest, "EXEC S_GetWrhsLocation " + QuotedStr(ddlWrhsDest.SelectedValue), True, "Location_Code", "Location_Name", ViewState("DBConnection"))
                ViewState("SetLocation") = False
            End If
            FillTextBoxDt(ViewState("DtValue"))
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"

            tbProdSrcCode.Focus()
            'btnGetDt.Enabled = False
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            'newTrans()
            tbTransNo.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToText(tbReffNo, Dt.Rows(0)("ReffNmbr").ToString)
            BindToText(tbCodeFG, Dt.Rows(0)("ProductFG").ToString)
            BindToText(tbNameFG, Dt.Rows(0)("ProductFG_Name").ToString)
            BindToDropList(ddlReffType, Dt.Rows(0)("ReffType").ToString)
            BindToDropList(ddlWrhsArea, Dt.Rows(0)("WrhsArea").ToString)
            BindToDropList(ddlShift, Dt.Rows(0)("Shift").ToString)
            If Request.QueryString("ContainerId").ToString = "TransMaterialID" Then
                FillCombo(ddlWrhsSrc, "EXEC S_GetWrhsUserAreaF " + QuotedStr(ViewState("UserId").ToString) + "," + QuotedStr(ddlWrhsArea.SelectedValue) + ", ' And Wrhs_Type in (''Owner'', ''Deposit Out'') '", True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection"))
                FillCombo(ddlWrhsDest, "EXEC S_GetWrhsUserAreaF " + QuotedStr(ViewState("UserId").ToString) + "," + QuotedStr(ddlWrhsArea.SelectedValue) + ", ' And Wrhs_Type = ''Production''' ", True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection"))
            Else
                FillCombo(ddlWrhsSrc, "EXEC S_GetWrhsUserAreaF " + QuotedStr(ViewState("UserId").ToString) + "," + QuotedStr(ddlWrhsArea.SelectedValue) + ", ' And Wrhs_Type = ''Production'''", True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection"))
                FillCombo(ddlWrhsDest, "EXEC S_GetWrhsUserAreaF " + QuotedStr(ViewState("UserId").ToString) + "," + QuotedStr(ddlWrhsArea.SelectedValue) + ", ' And Wrhs_Type in (''Owner'', ''Deposit Out'')' ", True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection"))
            End If
            BindToDropList(ddlWrhsSrc, Dt.Rows(0)("WrhsSrc").ToString)
            BindToText(tbFgSubledSrc, Dt.Rows(0)("FgSrcSubled").ToString)
            BindToText(tbSubledSrc, Dt.Rows(0)("SrcSubled").ToString)
            BindToText(tbSubledSrcName, Dt.Rows(0)("Subled_Src_Name").ToString)
            BindToDropList(ddlWrhsDest, Dt.Rows(0)("WrhsDest").ToString)
            BindToText(tbSubledDest, Dt.Rows(0)("DestSubled").ToString)
            BindToText(tbFgSubledDest, Dt.Rows(0)("FgDestSubled").ToString)
            BindToText(tbSubledDestName, Dt.Rows(0)("Subled_Dest_Name").ToString)
            BindToText(tbType, Dt.Rows(0)("TransferType").ToString)
            BindToText(tbOperator, Dt.Rows(0)("Operator").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("ProductSrc+'|'+LocationSrc+'|'+Productdest+'|'+LocationDest = " + QuotedStr(ItemNo))
            If Dr.Length > 0 Then
                BindToText(tbProdSrcCode, Dr(0)("ProductSrc").ToString)
                GetInfo(tbProdSrcCode.Text)
                BindToText(tbProdSrcName, Dr(0)("Product_Src_Name").ToString)
                BindToText(tbUnitSrc, Dr(0)("UnitSrc").ToString)
                BindToDropList(ddlLocationSrc, Dr(0)("Locationsrc").ToString)
                BindToText(tbQtySrc, Dr(0)("QtySrc").ToString)
                BindToText(tbProdDestCode, Dr(0)("ProductDest").ToString)
                BindToText(tbProdDestName, Dr(0)("Product_Dest_Name").ToString)
                BindToText(tbUnitDest, Dr(0)("UnitDest").ToString)
                BindToDropList(ddlLocationDest, Dr(0)("LocationDest").ToString)
                BindToText(tbQtyDest, Dr(0)("QtyDest").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
                BindToDropList(ddlCostCtrSrc, TrimStr(Dr(0)("CostCtrSrc").ToString))
                BindToDropList(ddlCostCtrDest, TrimStr(Dr(0)("CostCtrDest").ToString))
                BindToText(tbQtyRollSrc, Dr(0)("QtyRollSrc").ToString)
                BindToText(tbQtyM2Src, Dr(0)("QtyM2Src").ToString)
                lbQtyPackSrc.Text = Dr(0)("UnitPackSrc").ToString
                lbQtyM2Src.Text = Dr(0)("UnitM2Src").ToString
                BindToText(tbQtyRollDest, Dr(0)("QtyRollDest").ToString)
                BindToText(tbQtyM2Dest, Dr(0)("QtyM2Dest").ToString)
                lbQtyPackDest.Text = Dr(0)("UnitPackDest").ToString
                lbQtyM2Dest.Text = Dr(0)("UnitM2Dest").ToString
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub

    Private Sub StatusButtonSave(ByVal Bool As Boolean)
        btnSaveAll.Visible = Bool
        btnSaveTrans.Visible = Bool
        btnBack.Visible = Bool
    End Sub

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveDt.Click
        Dim Row As DataRow
        Try
            If ViewState("StateDt") = "Edit" Then
                If ViewState("DtValue") <> tbProdSrcCode.Text + "|" + ddlLocationSrc.SelectedValue + "|" + tbProdDestCode.Text + "|" + ddlLocationDest.SelectedValue Then
                    If CekExistData(ViewState("Dt"), "ProductSrc,LocationSrc,ProductDest,LocationDest", tbProdSrcCode.Text + "|" + ddlLocationSrc.SelectedValue + "|" + tbProdDestCode.Text + "|" + ddlLocationDest.SelectedValue) Then
                        lbStatus.Text = "Product Src '" + tbProdSrcName.Text + "' Location Src '" + ddlLocationSrc.SelectedItem.Text + "' Product Dest '" + tbProdSrcName.Text + "' Location Src '" + ddlLocationSrc.SelectedItem.Text + "' has already exists"
                        Exit Sub
                    End If
                End If
                Row = ViewState("Dt").Select("ProductSrc+'|'+LocationSrc+'|'+ProductDest+'|'+LocationDest = " + QuotedStr(ViewState("DtValue")))(0)
                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                Row("ProductSrc") = tbProdSrcCode.Text
                Row("Product_Src_Name") = tbProdSrcName.Text
                Row("LocationSrc") = ddlLocationSrc.SelectedValue
                Row("Location_Src_Name") = ddlLocationSrc.SelectedItem.Text
                Row("ProductDest") = tbProdDestCode.Text
                Row("Product_Dest_Name") = tbProdDestName.Text
                Row("LocationDest") = ddlLocationDest.SelectedValue
                Row("Location_Dest_Name") = ddlLocationDest.SelectedItem.Text
                Row("UnitSrc") = tbUnitSrc.Text
                Row("QtyDest") = tbQtyDest.Text
                Row("UnitDest") = tbUnitDest.Text
                Row("QtySrc") = tbQtySrc.Text
                Row("Remark") = tbRemarkDt.Text
                Row("UnitPackSrc") = lbQtyPackSrc.Text
                Row("UnitM2Src") = lbQtyM2Src.Text
                Row("UnitPackDest") = lbQtyPackDest.Text
                Row("UnitM2Dest") = lbQtyM2Dest.Text
                Row("QtyRollSrc") = FormatFloat(tbQtyRollSrc.Text, ViewState("DigitQty"))
                Row("QtyM2Src") = FormatFloat(tbQtyM2Src.Text, ViewState("DigitQty"))
                Row("QtyRollDest") = FormatFloat(tbQtyRollDest.Text, ViewState("DigitQty"))
                Row("QtyM2Dest") = FormatFloat(tbQtyM2Dest.Text, ViewState("DigitQty"))
                Row("CostCtrSrc") = ddlCostCtrSrc.SelectedValue
                Row("CostCtrSrcName") = ddlCostCtrSrc.SelectedItem.Text
                Row("CostCtrDest") = ddlCostCtrDest.SelectedValue
                Row("CostCtrDestName") = ddlCostCtrDest.SelectedItem.Text
                Row.EndEdit()
                'ViewState("Dt").AcceptChanges()
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If
                If CekExistData(ViewState("Dt"), "ProductSrc,LocationSrc,ProductDest,LocationDest", tbProdSrcCode.Text + "|" + ddlLocationSrc.SelectedValue + "|" + tbProdDestCode.Text + "|" + ddlLocationDest.SelectedValue) Then
                    lbStatus.Text = "Product Src '" + tbProdSrcName.Text + "' Location Src '" + ddlLocationSrc.SelectedItem.Text + "' Product Dest '" + tbProdDestName.Text + "' Location Dest '" + ddlLocationDest.SelectedItem.Text + "' has already exists"
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("ProductSrc") = tbProdSrcCode.Text
                dr("Product_Src_Name") = tbProdSrcName.Text
                dr("LocationSrc") = ddlLocationSrc.SelectedValue
                dr("Location_Src_Name") = ddlLocationSrc.SelectedItem.Text
                dr("UnitSrc") = tbUnitSrc.Text
                dr("QtySrc") = tbQtySrc.Text
                dr("ProductDest") = tbProdDestCode.Text
                dr("Product_Dest_Name") = tbProdDestName.Text
                dr("LocationDest") = ddlLocationDest.SelectedValue
                dr("Location_Dest_Name") = ddlLocationDest.SelectedItem.Text
                dr("UnitDest") = tbUnitDest.Text
                dr("QtyDest") = tbQtyDest.Text
                dr("Remark") = tbRemarkDt.Text
                dr("UnitPackSrc") = lbQtyPackSrc.Text
                dr("UnitM2Src") = lbQtyM2Src.Text
                dr("UnitPackDest") = lbQtyPackDest.Text
                dr("UnitM2Dest") = lbQtyM2Dest.Text
                dr("QtyRollSrc") = FormatFloat(tbQtyRollSrc.Text, ViewState("DigitQty"))
                dr("QtyM2Src") = FormatFloat(tbQtyM2Src.Text, ViewState("DigitQty"))
                dr("QtyRollDest") = FormatFloat(tbQtyRollDest.Text, ViewState("DigitQty"))
                dr("QtyM2Dest") = FormatFloat(tbQtyM2Dest.Text, ViewState("DigitQty"))
                dr("CostCtrSrc") = ddlCostCtrSrc.SelectedValue
                dr("CostCtrSrcName") = ddlCostCtrSrc.SelectedItem.Text
                dr("CostCtrDest") = ddlCostCtrDest.SelectedValue
                dr("CostCtrDestName") = ddlCostCtrDest.SelectedItem.Text
                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            BindGridDt(ViewState("Dt"), GridDt)
            StatusButtonSave(True)
            PnlInfo.Visible = False
        Catch ex As Exception
            lbStatus.Text = "btn save Dt Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try

    End Sub

    Protected Sub btnCancelDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBack.Click, btnCancelDt.Click
        Try
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            'EnableHd(GridDt.Rows.Count = 0)
            StatusButtonSave(True)
            PnlInfo.Visible = False
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbProdSrcCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbProdSrcCode.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try
            Dt = SQLExecuteQuery("SELECT Product_Code, Product_Name, Specification, Unit, UnitPack, UnitM2, CostCtr, CostCtrName FROM VMsProduct WHERE Product_Code = " + QuotedStr(tbProdSrcCode.Text), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbProdSrcCode.Text = Dr("Product_Code")
                tbProdSrcName.Text = Dr("Product_Name") + " " + TrimStr(Dr("Specification").ToString)
                tbUnitSrc.Text = Dr("Unit")
                tbUnitDest.Text = Dr("Unit")
                lbQtyPackSrc.Text = Dr("UnitPack").ToString
                lbQtyM2Src.Text = Dr("UnitM2").ToString
                BindToDropList(ddlCostCtrSrc, Dr("CostCtr").ToString)
                BindToDropList(ddlCostCtrDest, Dr("CostCtr").ToString)
                tbProdDestCode.Text = Dr("Product_Code")
                tbProdDestName.Text = Dr("Product_Name") + " " + TrimStr(Dr("Specification").ToString)
                GetInfo(tbProdSrcCode.Text)
            Else
                tbProdSrcCode.Text = ""
                tbProdSrcName.Text = ""
                tbUnitSrc.Text = ""
                lbQtyPackSrc.Text = ""
                lbQtyM2Src.Text = ""
                ddlCostCtrSrc.SelectedValue = ""
                PnlInfo.Visible = False
            End If
            If (ViewState("StrKode") = "TRR") Then
                tbProdDestCode.Text = tbProdSrcCode.Text
                tbProdDestName.Text = tbProdSrcName.Text
                tbUnitDest.Text = tbUnitSrc.Text
                lbQtyPackDest.Text = lbQtyPackSrc.Text
                lbQtyM2Dest.Text = lbQtyM2Src.Text
                ddlCostCtrDest.SelectedValue = ddlCostCtrSrc.SelectedValue
            End If
            tbQtySrc_TextChanged(Nothing, Nothing)
            AttachScript("setformatdt();", Page, Me.GetType())
            tbProdSrcCode.Focus()
        Catch ex As Exception
            Throw New Exception("tb ProductCode Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlLocationSrc_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlLocationSrc.SelectedIndexChanged
        If ViewState("InputLocation") = "Y" Then
            RefreshMaster("S_GetWrhsLocation", "Location_Code", "Location_Name", ddlLocationSrc, ViewState("DBConnection"))
            ViewState("InputLocation") = Nothing
        End If
        ddlLocationSrc.Focus()
    End Sub

    Protected Sub tbProdDestCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbProdDestCode.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try
            Dt = SQLExecuteQuery("SELECT Product_Code, Product_Name, Specification, Unit FROM VMsProduct WHERE Product_Code = '" + tbProdDestCode.Text, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbProdDestCode.Text = Dr("Product_Code")
                tbProdDestName.Text = Dr("Product_Name") + " " + TrimStr(Dr("Specification").ToString)
                tbUnitDest.Text = Dr("Unit")
                lbQtyPackDest.Text = Dr("UnitPack").ToString
                lbQtyM2Dest.Text = Dr("UnitM2").ToString
                BindToDropList(ddlCostCtrDest, TrimStr(Dr("CostCtr").ToString))
            Else
                tbProdDestCode.Text = ""
                tbProdDestName.Text = ""
                tbUnitDest.Text = ""
                lbQtyPackDest.Text = ""
                lbQtyM2Dest.Text = ""
                ddlCostCtrDest.SelectedValue = ""
            End If
            tbQtyDest_TextChanged(Nothing, Nothing)
            AttachScript("setformatdt();", Page, Me.GetType())
            tbProdDestCode.Focus()
        Catch ex As Exception
            Throw New Exception("tb ProductCode Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlLocationDest_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlLocationDest.SelectedIndexChanged
        If ViewState("InputLocation") = "Y" Then
            RefreshMaster("S_GetWrhsLocation", "Location_Code", "Location_Name", ddlLocationDest, ViewState("DBConnection"))
            ViewState("InputLocation") = Nothing
        End If
        ddlLocationDest.Focus()
    End Sub

    Protected Sub ddlWrhsSrc_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlWrhsSrc.SelectedIndexChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Warehouse", ddlWrhsSrc.SelectedValue, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbFgSubledSrc.Text = Dr("FgSubLed")
                tbSubledSrc.Text = ""
                tbSubledSrcName.Text = ""
            Else
                tbFgSubledSrc.Text = "N"
                tbSubledSrc.Text = ""
                tbSubledSrcName.Text = ""
            End If
            ViewState("SetLocation") = True
            ViewState("SetLocation") = True
            FillCombo(ddlLocationAllSrc, "EXEC S_GetWrhsLocation " + QuotedStr(ddlWrhsSrc.SelectedValue), True, "Location_Code", "Location_Name", ViewState("DBConnection"))

            tbSubledSrc.Enabled = tbFgSubledSrc.Text <> "N"
            btnSubledSrc.Visible = tbSubledSrc.Enabled
            ddlWrhsSrc.Focus()
        Catch ex As Exception
            Throw New Exception("tb WrhsCode Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlWrhsDest_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlWrhsDest.SelectedIndexChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Warehouse", ddlWrhsDest.SelectedValue, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbFgSubledDest.Text = Dr("FgSubLed")
                tbSubledDest.Text = ""
                tbSubledDestName.Text = ""
            Else
                tbFgSubledDest.Text = "N"
                tbSubledDest.Text = ""
                tbSubledDestName.Text = ""
            End If
            'CekHd()
            ViewState("SetLocation") = True
            tbSubledDest.Enabled = tbFgSubledDest.Text <> "N"
            BtnSubledDest.Visible = tbSubledDest.Enabled
            ddlWrhsDest.Focus()
        Catch ex As Exception
            Throw New Exception("tb WrhsCode Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSubledSrc_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSubledSrc.Click
        Dim ResultField As String
        Try
            Session("filter") = "select SubLed_No, SubLed_Name from VMsSubLed WHERE FgSubLed = " + QuotedStr(tbFgSubledSrc.Text)
            ResultField = "SubLed_No, SubLed_Name"
            ViewState("Sender") = "btnSubLedSrc"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Cust Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSubledDest_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnSubledDest.Click
        Dim ResultField As String
        Try
            Session("filter") = "select SubLed_No, SubLed_Name from VMsSubLed WHERE FgSubLed = " + QuotedStr(tbFgSubledDest.Text)
            ResultField = "SubLed_No, SubLed_Name"
            ViewState("Sender") = "btnSubLedDest"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Cust Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbSubledSrc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbSubledSrc.TextChanged
        Dim Dr As DataRow
        Dim DT As DataTable
        Dim SPName As String
        Try
            Try
                SPName = "EXEC S_FindSubled " + QuotedStr(tbFgSubledSrc.Text) + ", " + QuotedStr(tbSubledSrc.Text)
                DT = SQLExecuteQuery(SPName, ViewState("DBConnection").ToString).Tables(0)
                If DT.Rows.Count > 0 Then
                    Dr = DT.Rows(0)
                Else
                    Dr = Nothing
                End If
            Catch ex As Exception
                Throw New Exception("EXEC S_FindSubled Error : " + ex.ToString)
            End Try
            If Not Dr Is Nothing Then
                tbSubledSrc.Text = Dr("Subled_No")
                tbSubledSrcName.Text = Dr("Subled_Name")
            Else
                tbSubledSrc.Text = ""
                tbSubledSrcName.Text = ""
            End If
            tbSubledSrc.Focus()
        Catch ex As Exception
            Throw New Exception("tbSubledSrc_TextChanged : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbSubledDest_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbSubledDest.TextChanged
        Dim Dr As DataRow
        Dim DT As DataTable
        Dim SPName As String
        Try
            Try
                SPName = "EXEC S_FindSubled " + QuotedStr(tbFgSubledDest.Text) + ", " + QuotedStr(tbSubledDest.Text)
                DT = SQLExecuteQuery(SPName, ViewState("DBConnection").ToString).Tables(0)
                If DT.Rows.Count > 0 Then
                    Dr = DT.Rows(0)
                Else
                    Dr = Nothing
                End If
            Catch ex As Exception
                Throw New Exception("EXEC S_FindSubled Error : " + ex.ToString)
            End Try
            If Not Dr Is Nothing Then
                tbSubledDest.Text = Dr("Subled_No")
                tbSubledDestName.Text = Dr("Subled_Name")
            Else
                tbSubledDest.Text = ""
                tbSubledDestName.Text = ""
            End If
            tbSubledDest.Focus()
        Catch ex As Exception
            Throw New Exception("tbSubledDest_TextChanged : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnProdSrc_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnProdSrc.Click
        Dim ResultField, CriteriaField As String
        Try
            CriteriaField = "Product_Code, Product_Name, Specification, Unit, Qty, QtyRoll, QtyM2, UnitPack, UnitM2, CostCtr, CostCtr_Name , LocationSrc, LocationSrcName, LocationDest, LocationDestName "
            Session("Filter") = "EXEC S_STTransferRMReff " + QuotedStr(ddlReffType.SelectedValue) + ", " + QuotedStr(tbReffNo.Text) + ", " + QuotedStr(ddlWrhsSrc.SelectedValue) + ", " + QuotedStr(tbSubledSrc.Text) + ", " + QuotedStr(ddlWrhsDest.SelectedValue) + ", " + QuotedStr(tbSubledDest.Text)
            'ResultField = "Product_Code, Product_Name, specification, Unit, Qty, QtyRoll, QtyM2, UnitPack, UnitM2, CostCtr, CostCtr_Name, LocationSrc, LocationSrcName, LocationDest, LocationDestName "
            Session("CriteriaField") = CriteriaField.Split(",")
            'ResultField = "Product_Code, Product_Name, Unit, Specification, Qty, QtyRoll, QtyM2, UnitPack, UnitM2, CostCtr, CostCtr_Name, LocationSrc, LocationSrcName, LocationDest, LocationDestName "


            'CriteriaField = "Product_Code, Product_Name, Unit, Specification, UnitPack, UnitM2, CostCtr, CostCtrName"
            'Session("Filter") = "EXEC S_STTransferReffNR '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "' , " + QuotedStr(ddlWrhsSrc.SelectedValue) + ", " + QuotedStr(tbSubledSrc.Text) + ", " + QuotedStr(ViewState("UserId").ToString)
            ResultField = "Product_Code, Product_Name, Unit, Specification, UnitPack, UnitM2, CostCtr, CostCtr_Name"
            'Session("CriteriaField") = CriteriaField.Split(",")

            ViewState("Sender") = "btnProdSrc"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Product Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnProdDest_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnProdDest.Click
        Dim ResultField, CriteriaField As String
        Try
            CriteriaField = "Product_Code, Product_Name, Specification, Unit, Qty, QtyRoll, QtyM2, UnitPack, UnitM2, CostCtr, CostCtr_Name , LocationSrc, LocationSrcName, LocationDest, LocationDestName "
            Session("Filter") = "EXEC S_STTransferRMReff " + QuotedStr(ddlReffType.SelectedValue) + ", " + QuotedStr(tbReffNo.Text) + ", " + QuotedStr(ddlWrhsSrc.SelectedValue) + ", " + QuotedStr(tbSubledSrc.Text) + ", " + QuotedStr(ddlWrhsDest.SelectedValue) + ", " + QuotedStr(tbSubledDest.Text)
            'ResultField = "Product_Code, Product_Name, specification, Unit, Qty, QtyRoll, QtyM2, UnitPack, UnitM2, CostCtr, CostCtr_Name, LocationSrc, LocationSrcName, LocationDest, LocationDestName "
            Session("CriteriaField") = CriteriaField.Split(",")
            'ResultField = "Product_Code, Product_Name, Unit, Specification, Qty, QtyRoll, QtyM2, UnitPack, UnitM2, CostCtr, CostCtr_Name, LocationSrc, LocationSrcName, LocationDest, LocationDestName "


            'CriteriaField = "Product, Product_Name, Specification, Unit, Qty"
            'Session("filter") = "select Product, Product_Name, Specification, Qty, Unit, UnitPack, UnitM2, CostCtr, CostCtrName  from V_STTransferRMDt WHERE ReffNmbr = " + QuotedStr(tbReffNo.Text)
            'End If
            ResultField = "Product_Code, Product_Name, Unit, Specification, Qty, UnitPack, UnitM2, CostCtr, CostCtr_Name "
            ViewState("Sender") = "btnProdDest"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Product Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdd2.Click, BtnAdd.Click
        Try
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, pnlDt, GridDt)
            newTrans()
            btnHome.Visible = False
            tbDate.Focus()
            PnlInfo.Visible = False
            ddlReffType.Enabled = False
            'btnGetDt.Visible = False
            If Request.QueryString("ContainerId").ToString = "TransMaterialID" Then
                ddlReffType.SelectedValue = "Material"
            ElseIf Request.QueryString("ContainerId").ToString = "TransProjectID" Then
                ddlReffType.SelectedValue = "Project"
            Else
                ddlReffType.SelectedValue = "Return"
            End If
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlWrhsArea_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlWrhsArea.SelectedIndexChanged
        'FillCombo(ddlWrhsSrc, "EXEC S_GetWrhsUserArea " + QuotedStr(ViewState("UserId").ToString) + "," + QuotedStr(ddlWrhsArea.SelectedValue), True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection"))
        'FillCombo(ddlWrhsDest, "EXEC S_GetWrhsUserArea " + QuotedStr(ViewState("UserId").ToString) + "," + QuotedStr(ddlWrhsArea.SelectedValue), True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection"))
        Try
            
            If Request.QueryString("ContainerId").ToString = "TransMaterialID" Then
                FillCombo(ddlWrhsSrc, "EXEC S_GetWrhsUserAreaF " + QuotedStr(ViewState("UserId").ToString) + "," + QuotedStr(ddlWrhsArea.SelectedValue) + ", ' And Wrhs_Type in (''Owner'', ''Deposit Out'') '", True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection"))
                FillCombo(ddlWrhsDest, "EXEC S_GetWrhsUserAreaF " + QuotedStr(ViewState("UserId").ToString) + "," + QuotedStr(ddlWrhsArea.SelectedValue) + ", ' And Wrhs_Type = ''Production''' ", True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection"))
            Else
                FillCombo(ddlWrhsSrc, "EXEC S_GetWrhsUserAreaF " + QuotedStr(ViewState("UserId").ToString) + "," + QuotedStr(ddlWrhsArea.SelectedValue) + ", ' And Wrhs_Type = ''Production'''", True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection"))
                FillCombo(ddlWrhsDest, "EXEC S_GetWrhsUserAreaF " + QuotedStr(ViewState("UserId").ToString) + "," + QuotedStr(ddlWrhsArea.SelectedValue) + ", ' And Wrhs_Type in (''Owner'', ''Deposit Out'')' ", True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection"))
            End If
        Catch ex As Exception
            lbStatus.Text = "ddlWrhsArea_SelectedIndexChanged Error : " + ex.ToString
        End Try
        
    End Sub

    Protected Sub tbQtySrc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbQtySrc.TextChanged
        Try
            Dim QtyRoll, QtyM2 As Double

            If UCase(tbUnitSrc.Text) = UCase(lbQtyPackSrc.Text) Then
                QtyRoll = SQLExecuteScalar("Select dbo.ConvertQtyUnit( " + QuotedStr(tbProdSrcCode.Text) + ", " + QuotedStr(lbQtyPackSrc.Text) + ", " + tbQtySrc.Text.Replace(",", "") + ", " + QuotedStr(tbUnitSrc.Text) + " )", ViewState("DBConnection"))
                tbQtyRollSrc.Text = FormatFloat(QtyRoll, ViewState("DigitQty"))
                QtyM2 = SQLExecuteScalar("Select dbo.ConvertQtyUnit( " + QuotedStr(tbProdSrcCode.Text) + ", " + QuotedStr(lbQtyM2Src.Text) + ", " + tbQtySrc.Text.Replace(",", "") + ", " + QuotedStr(tbUnitSrc.Text) + " )", ViewState("DBConnection"))
                tbQtyM2Src.Text = FormatFloat(QtyM2, ViewState("DigitQty"))
            ElseIf UCase(tbUnitSrc.Text) = UCase(lbQtyM2Src.Text) Then
                QtyRoll = SQLExecuteScalar("Select dbo.ConvertQtyUnit( " + QuotedStr(tbProdSrcCode.Text) + ", " + QuotedStr(lbQtyPackSrc.Text) + ", " + tbQtySrc.Text.Replace(",", "") + ", " + QuotedStr(tbUnitSrc.Text) + " )", ViewState("DBConnection"))
                tbQtyRollSrc.Text = FormatFloat(QtyRoll, ViewState("DigitQty"))
                QtyM2 = SQLExecuteScalar("Select dbo.ConvertQtyUnit( " + QuotedStr(tbProdSrcCode.Text) + ", " + QuotedStr(lbQtyM2Src.Text) + ", " + tbQtySrc.Text.Replace(",", "") + ", " + QuotedStr(tbUnitSrc.Text) + " )", ViewState("DBConnection"))
                tbQtyM2Src.Text = FormatFloat(QtyM2, ViewState("DigitQty"))
            Else
                QtyRoll = SQLExecuteScalar("Select dbo.ConvertQtyUnit( " + QuotedStr(tbProdSrcCode.Text) + ", " + QuotedStr(lbQtyPackSrc.Text) + ", " + tbQtySrc.Text.Replace(",", "") + ", " + QuotedStr(tbUnitSrc.Text) + " )", ViewState("DBConnection"))
                tbQtyRollSrc.Text = FormatFloat(QtyRoll, ViewState("DigitQty"))
                QtyM2 = SQLExecuteScalar("Select dbo.ConvertQtyUnit( " + QuotedStr(tbProdSrcCode.Text) + ", " + QuotedStr(lbQtyM2Src.Text) + ", " + tbQtySrc.Text.Replace(",", "") + ", " + QuotedStr(tbUnitSrc.Text) + " )", ViewState("DBConnection"))
                tbQtyM2Src.Text = FormatFloat(QtyM2, ViewState("DigitQty"))
            End If
            tbQtySrc.Text = FormatFloat(tbQtySrc.Text, ViewState("DigitQty"))
            tbQtyDest.Text = FormatFloat(tbQtySrc.Text, ViewState("DigitQty"))
            tbQtyRollSrc.Text = FormatFloat(tbQtyRollSrc.Text, ViewState("DigitQty"))
            tbQtyRollDest.Text = FormatFloat(tbQtyRollSrc.Text, ViewState("DigitQty"))
            tbQtyM2Src.Text = FormatFloat(tbQtyM2Src.Text, ViewState("DigitQty"))
            tbQtyM2Dest.Text = FormatFloat(tbQtyM2Src.Text, ViewState("DigitQty"))
            ddlLocationDest.Focus()
        Catch ex As Exception
            lbStatus.Text = "tbQty_TextChanged Error : " + ex.ToString
        End Try
        
    End Sub

    Private Sub GetInfo(ByVal Product As String)
        Dim SqlString As String
        Dim DS As DataSet
        Try
            SqlString = "EXEC S_GetInfoStock " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + _
            ", " + QuotedStr(ddlWrhsSrc.SelectedValue) + ", " + QuotedStr(tbSubledSrc.Text) + ", " + QuotedStr(Product)

            DS = SQLExecuteQuery(SqlString, ViewState("DBConnection"))

            GridInfo.DataSource = DS.Tables(0)
            GridInfo.DataBind()
            PnlInfo.Visible = True
            lbInfo.Visible = DS.Tables(0).Rows.Count > 0
        Catch ex As Exception
            Throw New Exception("get info error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnReffNo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnReffNo.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT DISTINCT ReffNmbr, TransDate, ProductFG, ProductFGName, Wrhs_Area, Warehouse, WarehouseName, Remark from V_STTransferRMHd WHERE ReffType = " + QuotedStr(ddlReffType.SelectedValue)
            ResultField = "ReffNmbr, Wrhs_Area, Warehouse, Remark, ProductFG, ProductFGName"
            ViewState("Sender") = "btnReffNo"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Reff No Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnGetDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGetDt.Click
        Dim ResultField, CriteriaField As String 'ResultSame 
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            Session("Result") = Nothing
            If tbReffNo.Text = "" Then
                CriteriaField = "Product_Code, Product_Name, Specification, Unit, UnitPack, UnitM2, CostCtr, CostCtrName"
                Session("Filter") = "EXEC S_STTransferReffNR '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "' , " + QuotedStr(ddlWrhsSrc.SelectedValue) + ", " + QuotedStr(tbSubledSrc.Text) + ", " + QuotedStr(ViewState("UserId").ToString)
                ResultField = "Product_Code, Product_Name, Specification, Unit, UnitPack, UnitM2, CostCtr, CostCtrName"
                Session("CriteriaField") = CriteriaField.Split(",")
            Else
                CriteriaField = "Product_Code, Product_Name, Specification, Unit, Qty, QtyRoll, QtyM2, UnitPack, UnitM2, CostCtr, CostCtr_Name , LocationSrc, LocationSrcName, LocationDest, LocationDestName "
                Session("Filter") = "EXEC S_STTransferRMReff " + QuotedStr(ddlReffType.SelectedValue) + ", " + QuotedStr(tbReffNo.Text) + ", " + QuotedStr(ddlWrhsSrc.SelectedValue) + ", " + QuotedStr(tbSubledSrc.Text) + ", " + QuotedStr(ddlWrhsDest.SelectedValue) + ", " + QuotedStr(tbSubledDest.Text)
                ResultField = "Product_Code, Product_Name, specification, Unit, Qty, QtyRoll, QtyM2, UnitPack, UnitM2, CostCtr, CostCtr_Name, LocationSrc, LocationSrcName, LocationDest, LocationDestName "
                Session("CriteriaField") = CriteriaField.Split(",")
                ResultField = "Product_Code, Product_Name, Unit, Specification, Qty, QtyRoll, QtyM2, UnitPack, UnitM2, CostCtr, CostCtr_Name, LocationSrc, LocationSrcName, LocationDest, LocationDestName "
                Session("CriteriaField") = CriteriaField.Split(",")
            End If

            Session("Column") = ResultField.Split(",")
            ViewState("Sender") = "btnGetDt"
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn get Dt Error : " + ex.ToString
        End Try
    End Sub

    
    Dim QtyOnHand As Decimal = 0
    Protected Sub GridInfo_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridInfo.RowDataBound
        Try
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "Qty")) Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                    ' add the UnitPrice and QuantityTotal to the running total variables
                    QtyOnHand += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Qty"))
                ElseIf e.Row.RowType = DataControlRowType.Footer Then
                    e.Row.Cells(0).Text = "Total On Hand :"
                    ' for the Footer, display the running totals
                    e.Row.Cells(1).Text = FormatFloat(QtyOnHand, ViewState("DigitQty"))
                End If
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Info Row Data Bound Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbQtyDest_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbQtyDest.TextChanged
        Try
           
            Dim QtyRoll, QtyM2 As Double
            If UCase(tbUnitDest.Text) = UCase(lbQtyPackDest.Text) Then
                tbQtyRollDest.Text = FormatFloat(QtyRoll, ViewState("DigitQty"))
                QtyM2 = SQLExecuteScalar("Select dbo.ConvertQtyUnit( " + QuotedStr(tbProdDestCode.Text) + ", " + QuotedStr(lbQtyM2Dest.Text) + ", " + tbQtyDest.Text.Replace(",", "") + ", " + QuotedStr(tbUnitDest.Text) + " )", ViewState("DBConnection"))
                tbQtyM2Dest.Text = FormatFloat(QtyM2, ViewState("DigitQty"))
            ElseIf UCase(tbUnitDest.Text) = UCase(lbQtyM2Dest.Text) Then
                QtyRoll = SQLExecuteScalar("Select dbo.ConvertQtyUnit( " + QuotedStr(tbProdDestCode.Text) + ", " + QuotedStr(lbQtyPackDest.Text) + ", " + tbQtyDest.Text.Replace(",", "") + ", " + QuotedStr(tbUnitDest.Text) + " )", ViewState("DBConnection"))
                tbQtyRollDest.Text = FormatFloat(QtyRoll, ViewState("DigitQty"))
                QtyM2 = SQLExecuteScalar("Select dbo.ConvertQtyUnit( " + QuotedStr(tbProdDestCode.Text) + ", " + QuotedStr(lbQtyM2Dest.Text) + ", " + tbQtyDest.Text.Replace(",", "") + ", " + QuotedStr(tbUnitDest.Text) + " )", ViewState("DBConnection"))
                tbQtyM2Dest.Text = FormatFloat(QtyM2, ViewState("DigitQty"))
            Else
                QtyRoll = SQLExecuteScalar("Select dbo.ConvertQtyUnit( " + QuotedStr(tbProdDestCode.Text) + ", " + QuotedStr(lbQtyPackDest.Text) + ", " + tbQtyDest.Text.Replace(",", "") + ", " + QuotedStr(tbUnitDest.Text) + " )", ViewState("DBConnection"))
                tbQtyRollDest.Text = FormatFloat(QtyRoll, ViewState("DigitQty"))
                QtyM2 = SQLExecuteScalar("Select dbo.ConvertQtyUnit( " + QuotedStr(tbProdDestCode.Text) + ", " + QuotedStr(lbQtyM2Dest.Text) + ", " + tbQtyDest.Text.Replace(",", "") + ", " + QuotedStr(tbUnitDest.Text) + " )", ViewState("DBConnection"))
                tbQtyM2Dest.Text = FormatFloat(QtyM2, ViewState("DigitQty"))
            End If
            'tbQty.Text = FormatNumber(tbQty.Text, ViewState("DigitQty"))
            tbQtyDest.Text = FormatFloat(tbQtyDest.Text, ViewState("DigitQty"))
        Catch ex As Exception
            lbStatus.Text = "tbQty_TextChanged Error : " + ex.ToString
        End Try
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
                    'btnGetSetZero.Visible = True
                End If
            Next
        Catch ex As Exception
            Throw New Exception("Check All Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub cbSelectHd_CheckedChanged1(ByVal sender As Object, ByVal e As System.EventArgs)
        CheckAllDt(GridDt, sender)
    End Sub
    Private Sub bindDataSetLocation()
        Try
            Dim GVR As GridViewRow
            Dim CB As CheckBox
            Dim lb As Label
            Dim GetQSystem As Double
            'Dim Dt As DataTable
            Dim HaveSelect As Boolean
            Dim CekKey, PrimaryKey, Product As String
            Dim Row As DataRow
            HaveSelect = False
            For Each GVR In GridDt.Rows
                CB = GVR.FindControl("cbSelect")
                lb = GVR.FindControl("lbLocation")
                GetQSystem = 0
                lbStatus.Text = GVR.Cells(2).Text + "|" + lb.Text
                Exit For
                If CB.Checked Then
                    CekKey = GVR.Cells(2).Text + "|" + ddlLocationAllSrc.SelectedValue
                    Product = GVR.Cells(2).Text
                    
                    PrimaryKey = GVR.Cells(2).Text + "|" + lb.Text
                    HaveSelect = True
                    If CekKey <> PrimaryKey Then
                        If Not CekExistData(ViewState("Dt"), "ProductSrc,LocationSrc", CekKey) Then
                            Row = ViewState("Dt").Select("ProductSrc+'|'+LocationSrc = " + QuotedStr(PrimaryKey))(0)
                            Row.BeginEdit()
                            Row("LocationSrc") = ddlLocationAllSrc.SelectedValue
                            Row("LocationSrc_Name") = ddlLocationAllSrc.SelectedItem.Text
                            Row.EndEdit()
                        End If
                    End If
                End If
            Next
            If HaveSelect = False Then
                lbStatus.Text = MessageDlg("Please Check Product for Process")
                Exit Sub
            Else
                lbStatus.Text = MessageDlg("Set All Location Success for Selected Product")
            End If
            'BindGridDt(ViewState("Dt"), GridDt)
            BindGridDtView()
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
        Catch ex As Exception
            Throw New Exception("bindDataGridlocation Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnProcess_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnProcess.Click
        Try
            bindDataSetLocation()
        Catch ex As Exception
            Throw New Exception("btnProcess_Click Error : " + ex.ToString)
        End Try
    End Sub
    Sub BindGridDtView()
        Dim IsEmpty As Boolean
        Dim DtTemp, source As DataTable
        Dim dr As DataRow()
        Dim DV As DataView
        Try
            DV = ViewState("Dt").DefaultView
            If ViewState("SortExpressionDt") = Nothing Then
                ViewState("SortExpressionDt") = "Product ASC"
            End If
            DV.Sort = ViewState("SortExpressionDt")

            IsEmpty = False
            source = DV.Table
            dr = source.Select("", "", DataViewRowState.CurrentRows)
            If dr.Count = 0 Then
                'If source.Rows.Count = 0 Then
                DtTemp = source.Clone
                DtTemp.Rows.Add(DtTemp.NewRow()) ' create a new blank row to the DataTable
                IsEmpty = True
                GridDt.DataSource = DtTemp
            Else
                GridDt.DataSource = source
            End If
            GridDt.DataBind()
            GridDt.Columns(0).Visible = Not IsEmpty
            GridDt.Columns(1).Visible = Not IsEmpty
            'Panel2.Visible = GridDt.Columns(1).Visible And ViewState("StateHd") <> "View"
            'PnlInfo0.Visible = GridDt.Columns(1).Visible And ViewState("StateHd") <> "View"
            'Panel1.Visible = GridDt.Columns(1).Visible And ViewState("StateHd") <> "View"

        Catch ex As Exception
            Throw New Exception("ShowGridDtIfEmpty Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlCostCtrSrc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCostCtrSrc.TextChanged
        Try
            ddlCostCtrDest.SelectedValue = ddlCostCtrSrc.SelectedValue
        Catch ex As Exception
            Throw New Exception("ddlCostCtrSrc_TextChanged Error : " + ex.ToString)
        End Try
    End Sub
End Class
