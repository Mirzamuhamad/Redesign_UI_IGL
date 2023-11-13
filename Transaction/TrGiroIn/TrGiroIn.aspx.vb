Imports System.Data
Partial Class Transaction_TrGiroIn_TrGiroIn
    Inherits System.Web.UI.Page

    Protected GetStringHd As String = "Select * From V_FNGiroIn"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                SetInit()
                Session("AdvanceFilter") = ""
                Dim dt As DataTable
                dt = SQLExecuteQuery("Select RptTitle, MenuName, COALESCE(MenuParam,'') AS MenuParam, COALESCE(FastRpt,'') AS FastRpt, COALESCE(ReportType,'') AS ReportType, COALESCE(ReportType2,'') AS ReportType2, COALESCE(FgPrintValue,'N') AS FgPrintValue, COALESCE(FgForceNewPage,'N') AS FgForceNewPage from MsMenu WHERE MenuId = " + QuotedStr(Request.QueryString("ContainerId").ToString), ViewState("DBConnection").ToString).Tables(0)
                lblTitle.Text = dt.Rows(0)("MenuName").ToString
            End If
            lbStatus.Text = ""
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnSupplier" Then
                End If
                Session("Result") = Nothing
                ViewState("Sender") = Nothing
                'Session("filter") = Nothing
                'Session("Column") = Nothing
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
        Try
            FillRange(ddlRange)
            FillCombo(ddlBankSetorSetor, "EXEC S_GetBank", True, "Bank_Code", "Bank_Name", ViewState("DBConnection"))
            FillCombo(ddlBankSetorDrawn, "EXEC S_GetBank", True, "Bank_Code", "Bank_Name", ViewState("DBConnection"))

            ViewState("SortExpression") = Nothing
            ViewState("DigitCurr") = 0
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            'FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))

            ddlCommand.Items.Add("Setor")
            ddlCommand.Items.Add("Drawn")
            ddlCommand.Items.Add("Cancel")
            ddlCommand.Items.Add("Un-Post")

            ddlCommand2.Items.Add("Setor")
            ddlCommand2.Items.Add("Drawn")
            ddlCommand2.Items.Add("Cancel")
            ddlCommand2.Items.Add("Un-Post")

            tbBankChargeDrawn.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbBankChargeSetor.Attributes.Add("OnKeyDown", "return PressNumeric();")

            tbBankChargeDrawn.Attributes.Add("OnBlur", "setformatdrawn();")
            tbBankChargeSetor.Attributes.Add("OnBlur", "setformatsetor();")

        Catch ex As Exception
            Throw New Exception("Set Init Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim StrFilter As String
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
            DT = BindDataTransaction(GetStringHd, StrFilter, ViewState("DBConnection").ToString)
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
            DV = DT.DefaultView
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "ReceiptDate DESC"
            End If
            DV.Sort = ViewState("SortExpression")
            GridView1.DataSource = DV
            GridView1.DataBind()
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataGiro(Optional ByVal StrFilter As String = "")
        Dim DT As DataTable
        'Dim DV As DataView
        Try
            DT = BindDataTransaction(GetStringHd, StrFilter, ViewState("DBConnection").ToString)
            'DV = DT.DefaultView
            'DV.Sort = ViewState("SortExpression")
            GridGiro.DataSource = DT
            GridGiro.DataBind()
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

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

    Private Sub ClearSetor()
        Try
            tbDateToBank.SelectedDate = ViewState("ServerDate") 'Today
            tbBuktiBankNoSetor.Text = ""
            tbBankChargeSetor.Text = "0"
            ddlBankReceiptSetor.SelectedValue = ""
            ddlBankSetorSetor.SelectedValue = ""
        Catch ex As Exception
            Throw New Exception("Clear Setor Error " + ex.ToString)
        End Try
    End Sub
    Private Sub ClearDrawn()
        Try
            tbDateToDrawn.SelectedDate = ViewState("ServerDate") 'Today
            tbBuktiBankNoDrawn.Text = ""
            tbBankChargeDrawn.Text = "0"
            tbRemarkDrawn.Text = ""
            ddlBankReceiptDrawn.SelectedValue = ""
        Catch ex As Exception
            Throw New Exception("Clear Setor Error " + ex.ToString)
        End Try
    End Sub
    Private Sub ClearCancel()
        Try
            tbDateCancel.SelectedDate = ViewState("ServerDate") 'Today
            tbCancelReason.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Setor Error " + ex.ToString)
        End Try
    End Sub

    Function cekSetor() As Boolean
        Try
            If tbDateToBank.SelectedDate = Nothing Then
                lbStatus.Text = MessageDlg("Date to Bank must have value")
                tbDateToBank.Focus()
                Return False
            End If
            If ddlBankReceiptSetor.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Bank Receipt must have value")
                ddlBankReceiptSetor.Focus()
                Return False
            End If
            'If ddlBankSetorSetor.SelectedValue = "" Then
            '    lbStatus.Text = MessageDlg("Bank Setor must have value")
            '    ddlBankSetorSetor.Focus()
            '    Return False
            'End If
            'If tbBuktiBankNoSetor.Text.Trim = "" Then
            '    lbStatus.Text = MessageDlg("Bukti Terima Bank must have value")
            '    tbBuktiBankNoSetor.Focus()
            '    Return False
            'End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Hd Error : " + ex.ToString)
        End Try
    End Function
    Function cekDrawn() As Boolean
        Try
            If tbDateToDrawn.SelectedDate = Nothing Then
                lbStatus.Text = MessageDlg("Date to Drawn must have value")
                tbDateToDrawn.Focus()
                Return False
            End If
            If ddlBankReceiptDrawn.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Bank Receipt must have value")
                ddlBankReceiptDrawn.Focus()
                Return False
            End If
            'If ddlBankSetorDrawn.SelectedValue = "" Then
            '    lbStatus.Text = MessageDlg("Bank Setor must have value")
            '    ddlBankSetorDrawn.Focus()
            '    Return False
            'End If
            'If tbBuktiBankNoDrawn.Text.Trim = "" Then
            '    lbStatus.Text = MessageDlg("Bukti Terima Bank must have value")
            '    tbBuktiBankNoDrawn.Focus()
            '    Return False
            'End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Hd Error : " + ex.ToString)
        End Try
    End Function
    Function cekCancel() As Boolean
        Try
            If tbDateCancel.SelectedDate = Nothing Then
                lbStatus.Text = MessageDlg("Date Cancel must have value")
                tbDateCancel.Focus()
                Return False
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Hd Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub FillTextBoxDrawn(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "GiroNo = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            If Dt.Rows(0) Is Nothing Then
                Exit Sub
            End If
            tbVoucherNmbr.Text = ""
            BindToDate(tbDateToDrawn, Dt.Rows(0)("DateToDrawn").ToString)
            BindToDropList(ddlBankReceiptDrawn, Dt.Rows(0)("BankReceipt").ToString)
            BindToDropList(ddlBankSetorDrawn, Dt.Rows(0)("BankSetor").ToString)
            BindToText(tbBuktiBankNoDrawn, Dt.Rows(0)("BuktiBankNo").ToString)
            BindToText(tbBankChargeDrawn, Dt.Rows(0)("BankCharge").ToString)
            BindToText(tbRemarkDrawn, Dt.Rows(0)("DrawnRemark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxAll(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "GiroNo = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            BindToDate(tbDateCancel, Dt.Rows(0)("TolakDate").ToString)
            BindToText(tbCancelReason, Dt.Rows(0)("TolakReason").ToString)
            BindToDate(tbDateToDrawn, Dt.Rows(0)("DateToDrawn").ToString)
            BindToText(tbVoucherNmbr, Dt.Rows(0)("Reference").ToString)
            BindToDropList(ddlBankReceiptDrawn, Dt.Rows(0)("BankReceipt").ToString)
            BindToDropList(ddlBankSetorDrawn, Dt.Rows(0)("BankSetor").ToString)
            BindToText(tbBuktiBankNoDrawn, Dt.Rows(0)("BuktiBankNo").ToString)
            BindToText(tbBankChargeDrawn, FormatFloat(Dt.Rows(0)("BankCharge").ToString, 2))
            BindToText(tbRemarkDrawn, Dt.Rows(0)("DrawnRemark").ToString)
            'BindToDate(tbDateToBank, Dt.Rows(0)("DateSetor").ToString)
            BindToDate(tbDateToBank, Dt.Rows(0)("DateToBank").ToString)
            BindToDropList(ddlBankReceiptSetor, Dt.Rows(0)("BankReceipt").ToString)
            BindToDropList(ddlBankSetorSetor, Dt.Rows(0)("BankSetor").ToString)
            BindToText(tbBuktiBankNoSetor, Dt.Rows(0)("BuktiBankNo").ToString)
            BindToText(tbBankChargeSetor, FormatFloat(Dt.Rows(0)("BankCharge").ToString, 2))
            lbGiroNo.Text = Dt.Rows(0)("GiroNo").ToString
            lbInfoStatus.Text = Dt.Rows(0)("Status").ToString
            lbReport.Text = Dt.Rows(0)("FgReport").ToString
            lbUserype.Text = Dt.Rows(0)("UserType").ToString
            lbUserCode.Text = Dt.Rows(0)("UserCode").ToString
            lbUserName.Text = Dt.Rows(0)("UserName").ToString
            lbReceiptNo.Text = Dt.Rows(0)("ReceiptNo").ToString
            lbReceiptDate.Text = Format(Dt.Rows(0)("ReceiptDate"), "dd MMM yyyy")
            lbBankGiroName.Text = Dt.Rows(0)("BankGiroName").ToString
            lbCurr.Text = Dt.Rows(0)("Currency").ToString
            lbRate.Text = FormatFloat(Dt.Rows(0)("ForexRate").ToString, ViewState("DigitRate"))
            lbDueDate.Text = Format(Dt.Rows(0)("DueDate"), "dd MMM yyyy")
            lbRemark.Text = Dt.Rows(0)("Remark").ToString
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            CheckAll(GridView1, sender)
        Catch ex As Exception
            lbStatus.Text = "cb Select Checked Changed Error : " + ex.ToString
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
        Try
            If Not (e.CommandName = "Sort" Or e.CommandName = "Page") Then
                index = Convert.ToInt32(e.CommandArgument)
                GVR = GridView1.Rows(index)
            End If
            If e.CommandName = "Go" Then
                DDL = GridView1.Rows(index).FindControl("ddl")
                If DDL.SelectedValue = "View" Then
                    FillCombo(ddlBankReceiptSetor, "EXEC S_GetBankReceipt " + QuotedStr(GVR.Cells(4).Text) + " ", True, "Bank_Code", "Bank_Name", ViewState("DBConnection"))
                    FillCombo(ddlBankReceiptDrawn, "EXEC S_GetBankReceipt " + QuotedStr(GVR.Cells(4).Text) + " ", True, "Bank_Code", "Bank_Name", ViewState("DBConnection"))
                    FillTextBoxAll(GVR.Cells(2).Text)
                    ModifyInput(False, pnlInfo)
                    ModifyInput(False, pnlSetor)
                    ModifyInput(False, pnlCancel)
                    ModifyInput(False, pnlDrawn)
                    btnHome.Visible = True
                    btnSaveSetor.Visible = False
                    btnCancelSetor.Visible = False
                    btnSaveDrawn.Visible = False
                    btnCancelDrawn.Visible = False
                    btnSaveCancel.Visible = False
                    btnCancelCancel.Visible = False
                    PnlDt.Visible = True
                    PnlHd.Visible = True
                    pnlSetor.Visible = True
                    pnlInfo.Visible = True
                    PnlGiroSelect.Visible = False
                    pnlDrawn.Visible = True
                    pnlCancel.Visible = True
                    PnlHd.Visible = False
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

    Protected Sub BtnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGo.Click, btnGo2.Click
        Dim Status As String
        Dim ListSelectNmbr, TBankReceipt, TBankSetor, TBuktiNo, ListGiro, ActionValue, FgReport, Result As String
        Dim HaveDataProcess As Boolean
        Dim GVR As GridViewRow
        Dim cbSelek As CheckBox
        Dim lbBankReceipt, lbBankSetor, lbBuktiNo As Label
        Dim j As Integer
        Dim Nmbr(100) As String
        Dim FirstTime As Boolean
        Dim CekMenu As String
        Try

            CekMenu = CheckMenuLevel("Insert", ViewState("MenuLevel").Rows(0))
            If CekMenu <> "" Then
                lbStatus.Text = CekMenu
                Exit Sub
            End If

            If sender.ID.ToString = "BtnGo" Then
                ActionValue = ddlCommand.SelectedValue
            Else
                ActionValue = ddlCommand2.SelectedValue
            End If

            HaveDataProcess = False
            FirstTime = True
            FgReport = ""
            TBankReceipt = ""
            TBankSetor = ""
            TBuktiNo = ""
            If ActionValue = "Setor" Or ActionValue = "Drawn" Then
                For Each GVR In GridView1.Rows
                    cbSelek = GVR.FindControl("cbSelect")
                    lbBankReceipt = GVR.FindControl("lbBankReceipt")
                    lbBankSetor = GVR.FindControl("lbBankSetor")
                    lbBuktiNo = GVR.FindControl("lbBuktiNo")
                    If cbSelek.Checked Then
                        If FirstTime Then
                            FgReport = GVR.Cells(4).Text
                            FirstTime = False
                        Else
                            If FgReport <> GVR.Cells(4).Text.ToString Then
                                lbStatus.Text = MessageDlg("Selected Giro must have 1 kind of report")
                                Exit Sub
                            End If
                        End If
                        If TBankReceipt = "" And lbBankReceipt.Text <> "" Then
                            TBankReceipt = lbBankReceipt.Text
                            TBankSetor = lbBankSetor.Text
                            TBuktiNo = lbBuktiNo.Text
                        ElseIf TBankReceipt <> "" And lbBankReceipt.Text <> "" Then
                            If TBankReceipt <> lbBankReceipt.Text Then
                                lbStatus.Text = MessageDlg("Selected Giro must have 1 kind of Bank Receipt (" + TBankReceipt + ")")
                                Exit Sub
                            End If
                        End If

                    End If
                Next
            End If

            FirstTime = True
            If ActionValue = "Setor" Then
                Status = "H,C"
                ListSelectNmbr = ""
                ListGiro = ""
                GetListCommand(Status, GridView1, "3,2", ListSelectNmbr, Nmbr, lbStatus.Text)
                For j = 0 To Nmbr.Length - 1
                    If Nmbr(j) <> "" Then
                        HaveDataProcess = True
                        If FirstTime Then
                            FirstTime = False
                            ListGiro = QuotedStr(Nmbr(j))
                        Else
                            ListGiro = ListGiro + "," + QuotedStr(Nmbr(j))
                        End If
                    End If
                Next
                If HaveDataProcess = False Then Exit Sub
                ViewState("ListSelectNmbr") = ListSelectNmbr
                ViewState("Nmbr") = Nmbr
                PnlHd.Visible = False
                PnlDt.Visible = True
                pnlInfo.Visible = False
                BindDataGiro("GiroNo In (" + ListGiro + ")")
                PnlGiroSelect.Visible = True
                btnHome.Visible = False
                pnlSetor.Visible = True
                pnlDrawn.Visible = False
                pnlCancel.Visible = False
                FillCombo(ddlBankReceiptSetor, "EXEC S_GetBankReceipt " + QuotedStr(FgReport) + " ", True, "Bank_Code", "Bank_Name", ViewState("DBConnection"))
                ClearSetor()

                ModifyInput(True, pnlSetor)
            ElseIf ActionValue = "Drawn" Then
                Status = "H,S,C"
                ListSelectNmbr = ""
                ListGiro = ""
                GetListCommand(Status, GridView1, "3,2", ListSelectNmbr, Nmbr, lbStatus.Text)
                For j = 0 To Nmbr.Length - 1
                    If Nmbr(j) <> "" Then
                        HaveDataProcess = True
                        If FirstTime Then
                            FirstTime = False
                            ListGiro = QuotedStr(Nmbr(j))
                        Else
                            ListGiro = ListGiro + "," + QuotedStr(Nmbr(j))
                        End If
                    End If
                Next
                If HaveDataProcess = False Then Exit Sub
                ViewState("ListSelectNmbr") = ListSelectNmbr
                ViewState("Nmbr") = Nmbr
                PnlHd.Visible = False
                PnlDt.Visible = True
                pnlInfo.Visible = False
                BindDataGiro("GiroNo In (" + ListGiro + ")")
                PnlGiroSelect.Visible = True
                btnHome.Visible = False
                pnlSetor.Visible = False
                pnlDrawn.Visible = True
                pnlCancel.Visible = False
                FillCombo(ddlBankReceiptDrawn, "EXEC S_GetBankReceipt " + QuotedStr(FgReport) + " ", True, "Bank_Code", "Bank_Name", ViewState("DBConnection"))
                ViewState("FgReport") = FgReport
                ClearDrawn()
                ModifyInput(True, pnlDrawn)
                'ddlBankSetorDrawn.Enabled = TBankReceipt = ""
                'ddlBankReceiptDrawn.Enabled = TBankReceipt = ""
                'tbBuktiBankNoDrawn.Enabled = TBankReceipt = ""
                'If TBankReceipt <> "" Then
                '    BindToDropList(ddlBankReceiptDrawn, TBankReceipt)
                '    BindToDropList(ddlBankSetorDrawn, TBankSetor)
                '    BindToText(tbBuktiBankNoDrawn, TBuktiNo)
                'End If
                Dim VoucherBank As String
                VoucherBank = SQLExecuteScalar("Declare @A VarChar(30) EXEC S_SAAutoVoucherNmbr '" + Format(tbDateToDrawn.SelectedValue, "yyyy-MM-dd") + "', '" + FgReport + "', '" + ddlBankReceiptDrawn.SelectedValue + "', 'IN', @A OUT Select @A ", ViewState("DBConnection").ToString)
                tbVoucherNmbr.Text = VoucherBank
            ElseIf ActionValue = "Cancel" Then
                Status = "H,S"
                ListSelectNmbr = ""
                ListGiro = ""
                GetListCommand(Status, GridView1, "3,2", ListSelectNmbr, Nmbr, lbStatus.Text)
                For j = 0 To Nmbr.Length - 1
                    If Nmbr(j) <> "" Then
                        HaveDataProcess = True
                        If FirstTime Then
                            FirstTime = False
                            ListGiro = QuotedStr(Nmbr(j))
                        Else
                            ListGiro = ListGiro + "," + QuotedStr(Nmbr(j))
                        End If
                    End If
                Next
                If HaveDataProcess = False Then Exit Sub
                ViewState("ListSelectNmbr") = ListSelectNmbr
                ViewState("Nmbr") = Nmbr
                PnlHd.Visible = False
                PnlDt.Visible = True
                pnlInfo.Visible = False
                BindDataGiro("GiroNo In (" + ListGiro + ")")
                PnlGiroSelect.Visible = True
                btnHome.Visible = False
                pnlSetor.Visible = False
                pnlDrawn.Visible = False
                pnlCancel.Visible = True
                ClearCancel()
                ModifyInput(True, pnlCancel)
            ElseIf ActionValue = "Un-Post" Then
                Status = "S,D,C"
                ListSelectNmbr = ""
                GetListCommand(Status, GridView1, "3,2", ListSelectNmbr, Nmbr, lbStatus.Text)
                If ListSelectNmbr = "" Then Exit Sub
                For j = 0 To (Nmbr.Length - 1)
                    If Nmbr(j) = "" Then
                        Exit For
                    Else
                        Result = SQLExecuteScalar("Declare @A VarChar(255) EXEC S_FNGiroInUnPost " + QuotedStr(Nmbr(j)) + ", " + CStr(Session(Request.QueryString("KeyId"))("Year")) + ", " + CStr(Session(Request.QueryString("KeyId"))("Period")) + ", " + QuotedStr(ViewState("UserId").ToString) + ", @A Out  Select @A ", ViewState("DBConnection").ToString)
                        If Trim(Result).Length > 5 Then
                            lbStatus.Text = lbStatus.Text + Result + " <br/>"
                        End If
                    End If
                Next
                BindData("GiroNo In (" + ListSelectNmbr + ")")
            End If
        Catch ex As Exception
            lbStatus.Text = "Go Command Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnCancelSetor_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelSetor.Click, btnCancelCancel.Click, btnCancelDrawn.Click, btnHome.Click
        Try
            MovePanel(PnlDt, PnlHd)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Setor Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveSetor_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveSetor.Click
        Dim Nmbr As String()
        Dim result As String
        Try
            'If tbBuktiBankNoSetor.Text.Trim = "" Then
            '    lbStatus.Text = "Bukti Terima Bank must be input"
            '    tbBuktiBankNoSetor.Focus()
            '    Exit Sub
            'End If
            If ddlBankReceiptSetor.SelectedValue.Trim = "" Then
                lbStatus.Text = "Bank Receipt must be input"
                ddlBankReceiptSetor.Focus()
                Exit Sub
            End If
            'If ddlBankSetorSetor.SelectedValue.Trim = "" Then
            '    lbStatus.Text = "Bank Setor must be input"
            '    ddlBankSetorSetor.Focus()
            '    Exit Sub
            'End If
            Nmbr = ViewState("Nmbr")
            For j = 0 To Nmbr.Length - 1
                If Nmbr(j) = "" Then
                    Exit For
                Else
                    result = SQLExecuteScalar("Declare @A VARCHAR(255) EXEC S_FNGiroInSetor " + QuotedStr(Nmbr(j)) + ", " + QuotedStr(Format(tbDateToBank.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(ddlBankReceiptSetor.SelectedValue) + "," + QuotedStr(ddlBankSetorSetor.SelectedValue) + "," + QuotedStr(tbBuktiBankNoSetor.Text) + ", " + tbBankChargeSetor.Text.Replace(",", "") + "," + QuotedStr(ViewState("UserId").ToString) + ", @A Out Select @A ", ViewState("DBConnection"))
                    If Trim(result).Length > 5 Then
                        lbStatus.Text = lbStatus.Text + result + " <br/>"
                    End If
                End If
            Next
            BindData("GiroNo In (" + ViewState("ListSelectNmbr") + ")")
            PnlDt.Visible = False
            PnlHd.Visible = True
        Catch ex As Exception
            lbStatus.Text = "btn SaveDrawn Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnSaveDrawn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDrawn.Click
        Dim Nmbr As String()
        Dim result As String
        Try
            'If tbBuktiBankNoDrawn.Text.Trim = "" Then
            '    lbStatus.Text = "Bukti Keluar Bank must be input"
            '    tbBuktiBankNoDrawn.Focus()
            '    Exit Sub
            'End If
            If ddlBankReceiptDrawn.SelectedValue.Trim = "" Then
                lbStatus.Text = "Bank Receipt must be input"
                ddlBankReceiptDrawn.Focus()
                Exit Sub
            End If
            'If ddlBankSetorDrawn.SelectedValue.Trim = "" Then
            '    lbStatus.Text = "Bank Setor must be input"
            '    ddlBankSetorDrawn.Focus()
            '    Exit Sub
            'End If
            Nmbr = ViewState("Nmbr")
            For j = 0 To Nmbr.Length - 1
                If Nmbr(j) = "" Then
                    Exit For
                Else
                    result = SQLExecuteScalar("Declare @A VARCHAR(255) EXEC S_FNGiroInDrawn " + QuotedStr(Nmbr(j)) + ", " + QuotedStr(tbVoucherNmbr.Text) + "," + QuotedStr(Format(tbDateToDrawn.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(tbBuktiBankNoDrawn.Text) + ", " + QuotedStr(ddlBankReceiptDrawn.SelectedValue) + ", " + QuotedStr(ddlBankSetorDrawn.SelectedValue) + ", " + tbBankChargeDrawn.Text.Replace(",", "") + ", " + QuotedStr(tbRemarkDrawn.Text) + ",  " + CStr(Session(Request.QueryString("KeyId"))("Year")) + ", " + CStr(Session(Request.QueryString("KeyId"))("Period")) + ", " + QuotedStr(ViewState("UserId").ToString) + ", @A Out Select @A ", ViewState("DBConnection"))
                    If Trim(result).Length > 5 Then
                        lbStatus.Text = lbStatus.Text + result + " <br/>"
                    End If
                End If
            Next
            BindData("GiroNo In (" + ViewState("ListSelectNmbr") + ")")
            PnlDt.Visible = False
            PnlHd.Visible = True
        Catch ex As Exception
            lbStatus.Text = "btn SaveDrawn Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnSaveCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveCancel.Click
        Dim Nmbr As String()
        Dim result As String
        Try
            Nmbr = ViewState("Nmbr")
            For j = 0 To Nmbr.Length - 1
                If Nmbr(j) = "" Then
                    Exit For
                Else
                    result = SQLExecuteScalar("Declare @A VARCHAR(255) EXEC S_FNGiroInCancel '" + Nmbr(j) + "', '" + Format(tbDateCancel.SelectedValue, "yyyy-MM-dd") + "', '" + tbCancelReason.Text + "', " + QuotedStr(ViewState("UserId").ToString) + ", @A Out Select @A ", ViewState("DBConnection"))
                    If Trim(result).Length > 5 Then
                        lbStatus.Text = lbStatus.Text + result + " <br/>"
                    End If
                End If
            Next
            BindData("GiroNo In (" + ViewState("ListSelectNmbr") + ")")
            PnlDt.Visible = False
            PnlHd.Visible = True
        Catch ex As Exception
            lbStatus.Text = "btn SaveCancel Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlBankReceiptSetor_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlBankReceiptSetor.SelectedIndexChanged
        Dim Hasil As String
        Try
            Hasil = SQLExecuteScalar("SELECT Bank FROM VMsBankReceipt WHERE Bank_Code = " + QuotedStr(ddlBankReceiptSetor.SelectedValue), ViewState("DBConnection"))
            BindToDropList(ddlBankSetorSetor, Hasil)
        Catch ex As Exception
            lbStatus.Text = "btn ddlBankReceiptSetor Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlBankReceiptDrawn_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlBankReceiptDrawn.SelectedIndexChanged
        Dim Hasil, VoucherBank As String
        Try
            Hasil = SQLExecuteScalar("SELECT Bank FROM VMsBankReceipt WHERE Bank_Code = " + QuotedStr(ddlBankReceiptDrawn.SelectedValue), ViewState("DBConnection"))
            BindToDropList(ddlBankSetorDrawn, Hasil)
            VoucherBank = SQLExecuteScalar("Declare @A VarChar(255) EXEC S_SAAutoVoucherNmbr " + QuotedStr(Format(tbDateToDrawn.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(ViewState("FgReport").ToString) + ", " + QuotedStr(ddlBankReceiptDrawn.SelectedValue) + ", 'IN', @A OUT SELECT @A", ViewState("DBConnection").ToString)
            tbVoucherNmbr.Text = VoucherBank
        Catch ex As Exception
            lbStatus.Text = "btn ddlBankReceiptSetor Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbDateToDrawn_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbDateToDrawn.SelectionChanged
        Dim VoucherBank As String
        VoucherBank = SQLExecuteScalar("Declare @A VarChar(30) EXEC S_SAAutoVoucherNmbr '" + Format(tbDateToDrawn.SelectedValue, "yyyy-MM-dd") + "', '" + ViewState("FgReport").ToString + "', '" + ddlBankReceiptDrawn.SelectedValue + "', 'IN', @A OUT Select @A ", ViewState("DBConnection").ToString)
        tbVoucherNmbr.Text = VoucherBank
    End Sub
End Class
