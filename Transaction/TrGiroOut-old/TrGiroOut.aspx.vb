Imports System.Data

Partial Class Transaction_TrGiroOut_TrGiroOut
    Inherits System.Web.UI.Page

    Protected GetStringHd As String = "Select * From V_FNGiroOut"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
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
            'FillCombo(ddlBankSetorDrawn, "EXEC S_GetBank", False, "Bank_Code", "Bank_Name", ViewState("DBConnection"))
            FillRange(ddlRange)
            ViewState("SortExpression") = Nothing
            ViewState("DigitCurr") = 0
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)

            ddlCommand.Items.Add("Drawn")

            Dim fgAdmin As String
            fgAdmin = SQLExecuteScalar("SELECT FgAdmin FROM Sausers WHERE UserID = " + QuotedStr(ViewState("UserId").ToString), ViewState("DBConnection").ToString)
            If fgAdmin = "Y" Then
                ddlCommand.Items.Add("Cancel")
                ddlCommand.Items.Add("Un-Drawn")

                ddlCommand2.Items.Add("Drawn")
                ddlCommand2.Items.Add("Cancel")

            End If
            

            'ddlCommand2.Items.Add("Un-Post")

            'tbBankChargeDrawn.Attributes.Add("OnKeyDown", "return PressNumeric();")

            'tbBankChargeDrawn.Attributes.Add("OnBlur", "setformatdrawn();")

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
                ViewState("SortExpression") = "PaymentDate DESC"
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
            ' DV.Sort = ViewState("SortExpression")
            ViewState("FgReport") = DT.Rows(0)("FgReport").ToString
            ViewState("BankPayment") = DT.Rows(0)("BankPayment").ToString
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

    Private Sub ClearDrawn()
        Try
            tbDateToDrawn.SelectedDate = ViewState("ServerDate") 'Today
            tbBuktiBankNoDrawn.Text = ""
            'tbBankChargeDrawn.Text = "0"
            tbRemarkDrawn.Text = ""
            'ddlBankPaymentDrawn.SelectedValue = ""
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

    Function cekDrawn() As Boolean
        Try
            If tbDateToDrawn.SelectedDate = Nothing Then
                lbStatus.Text = MessageDlg("Date to Drawn must have value")
                tbDateToDrawn.Focus()
                Return False
            End If
            'If ddlBankPaymentDrawn.SelectedValue = "" Then
            '    lbStatus.Text = MessageDlg("Bank Payment must have value")
            '    ddlBankPaymentDrawn.Focus()
            '    Return False
            'End If
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
            'BindToDropList(ddlBankPaymentDrawn, Dt.Rows(0)("BankPayment").ToString)
            'BindToDropList(ddlBankSetorDrawn, Dt.Rows(0)("BankSetor").ToString)
            BindToText(tbBuktiBankNoDrawn, Dt.Rows(0)("BuktiBankNo").ToString)
            'BindToText(tbBankChargeDrawn, Dt.Rows(0)("BankCharge").ToString)
            BindToText(tbRemarkDrawn, Dt.Rows(0)("DrawnRemark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxAll(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "GiroNo+'|'+PaymentNo = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            BindToDate(tbDateCancel, Dt.Rows(0)("TolakDate").ToString)
            BindToText(tbCancelReason, Dt.Rows(0)("TolakReason").ToString)
            BindToDate(tbDateToDrawn, Dt.Rows(0)("DateToDrawn").ToString)

            BindToText(tbVoucherNmbr, Dt.Rows(0)("Reference").ToString)
            BindToText(tbBuktiBankNoDrawn, Dt.Rows(0)("BuktiBankNo").ToString)
            'BindToText(tbBankChargeDrawn, Dt.Rows(0)("BankCharge").ToString)
            BindToText(tbRemarkDrawn, Dt.Rows(0)("DrawnRemark").ToString)
            lbGiroNo.Text = Dt.Rows(0)("GiroNo").ToString
            lbInfoStatus.Text = Dt.Rows(0)("Status").ToString
            lbReport.Text = Dt.Rows(0)("FgReport").ToString
            lbUserype.Text = Dt.Rows(0)("UserType").ToString
            lbUserCode.Text = Dt.Rows(0)("UserCode").ToString
            lbUserName.Text = Dt.Rows(0)("UserName").ToString
            lbPaymentNo.Text = Dt.Rows(0)("PaymentNo").ToString
            lbPaymentDate.Text = Format(Dt.Rows(0)("PaymentDate"), "dd MMM yyyy")
            lbBankGiroName.Text = Dt.Rows(0)("BankPaymentName").ToString
            lbCurr.Text = Dt.Rows(0)("Currency").ToString
            lbRate.Text = FormatFloat(Dt.Rows(0)("ForexRate").ToString, ViewState("DigitRate"))
            lbDueDate.Text = Format(Dt.Rows(0)("DueDate"), "dd MMM yyyy")
            lbRemark.Text = Dt.Rows(0)("Remark").ToString
            lbAmountForex.Text = FormatFloat(Dt.Rows(0)("AmountForex").ToString, ViewState("DigitHome"))
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
                    'FillCombo(ddlBankPaymentDrawn, "EXEC S_GetBankPayment " + QuotedStr(GVR.Cells(4).Text) + " ", True, "Bank_Code", "Bank_Name", ViewState("DBConnection"))
                    FillTextBoxAll(GVR.Cells(2).Text + "|" + GVR.Cells(8).Text)
                    ModifyInput(False, pnlInfo)
                    ModifyInput(False, pnlCancel)
                    ModifyInput(False, pnlDrawn)
                    btnHome.Visible = True
                    btnSaveDrawn.Visible = False
                    btnCancelDrawn.Visible = False
                    btnSaveCancel.Visible = False
                    btnCancelCancel.Visible = False
                    tbDateToDrawn.DisplayType = BasicFrame.WebControls.DatePickerDisplayType.TextBox
                    tbDateCancel.DisplayType = BasicFrame.WebControls.DatePickerDisplayType.TextBox
                    PnlDt.Visible = True
                    PnlHd.Visible = True
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
        Dim ListSelectNmbr, ListGiro, ActionValue, Result, ListSelectPayment As String
        Dim HaveDataProcess As String
        Dim j As Integer
        Dim Nmbr(100) As String
        Dim FirstTime As Boolean
        Try
            If sender.ID.ToString = "BtnGo" Then
                ActionValue = ddlCommand.SelectedValue
            Else
                ActionValue = ddlCommand2.SelectedValue
            End If
            HaveDataProcess = False
            FirstTime = True
            If ActionValue = "Drawn" Then
                'Status = "H,C"
                Status = "H"
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

                'lbStatus.Text = ListSelectNmbr
                'Exit sub
                If HaveDataProcess = False Then Exit Sub
                ViewState("ListSelectNmbr") = ListSelectNmbr
                
                ViewState("Nmbr") = Nmbr
                PnlHd.Visible = False
                PnlDt.Visible = True
                pnlInfo.Visible = False
                BindDataGiro("GiroNo In (" + ListGiro + ") and Status <> 'C'")
                PnlGiroSelect.Visible = True
                btnHome.Visible = False
                pnlDrawn.Visible = True
                pnlCancel.Visible = False
                ClearDrawn()
                ModifyInput(True, pnlDrawn)
                Dim VoucherBank As String
                VoucherBank = SQLExecuteScalar("Declare @A VarChar(30) EXEC S_SAAutoVoucherNmbr '" + Format(tbDateToDrawn.SelectedValue, "yyyy-MM-dd") + "', '" + ViewState("FgReport").ToString + "', '" + ViewState("BankPayment") + "', 'OUT', @A OUT Select @A ", ViewState("DBConnection").ToString)
                'VoucherBank = SQLExecuteScalar("Declare @A VarChar(30) EXEC S_SAAutoNmbr " + CStr(Session(Request.QueryString("KeyId"))("Year")) + ", " + CStr(Session(Request.QueryString("KeyId"))("Period")) + ", '" + ViewState("FgReport").ToString + "', 'GPD', '" + ViewState("BankPayment") + "', @A OUT Select @A ", ViewState("DBConnection").ToString)
                tbVoucherNmbr.Text = VoucherBank
            ElseIf ActionValue = "Cancel" Then
                Status = "H"
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
                'GetListCommand(Status, GridView1, "3,8", ListSelectPayment, Nmbr, lbStatus.Text)
                'ViewState("ListSelectPayment") = ListSelectPayment
                ViewState("Nmbr") = Nmbr
                PnlHd.Visible = False
                PnlDt.Visible = True
                pnlInfo.Visible = False
                BindDataGiro("GiroNo In (" + ListGiro + ") and Status <> 'C'")
                PnlGiroSelect.Visible = True
                btnHome.Visible = False
                pnlDrawn.Visible = False
                pnlCancel.Visible = True
                ClearCancel()
                ModifyInput(True, pnlCancel)
            ElseIf ActionValue = "Un-Drawn" Then
                Status = "D"
                ListSelectNmbr = ""
                GetListCommand(Status, GridView1, "3,8", ListSelectPayment, Nmbr, lbStatus.Text)
                GetListCommand(Status, GridView1, "3,2", ListSelectNmbr, Nmbr, lbStatus.Text)

                If ListSelectNmbr = "" Then Exit Sub
                For j = 0 To (Nmbr.Length - 1)
                    If Nmbr(j) = "" Then
                        Exit For
                    Else
                        Result = SQLExecuteScalar("Declare @A VarChar(255) EXEC S_FNGiroOutUnPost " + QuotedStr(Nmbr(j)) + ", " + CStr(Session(Request.QueryString("KeyId"))("Year")) + ", " + CStr(Session(Request.QueryString("KeyId"))("Period")) + ", " + QuotedStr(ViewState("UserId").ToString) + ", @A Out  Select @A ", ViewState("DBConnection").ToString)
                        If Trim(Result).Length > 5 Then
                            lbStatus.Text = lbStatus.Text + Result + " <br/>"
                        End If
                    End If
                Next

                BindData("GiroNo In (" + ListSelectNmbr + ") AND PaymentNo in (" + ListSelectPayment + ") ")
            End If
        Catch ex As Exception
            lbStatus.Text = "Go Command Error : " + ex.ToString
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
            Nmbr = ViewState("Nmbr")
            For j = 0 To Nmbr.Length - 1
                If Nmbr(j) = "" Then
                    Exit For
                Else
                    result = SQLExecuteScalar("Declare @A VARCHAR(255) EXEC S_FNGiroOutDrawn '" + Nmbr(j) + "', " + QuotedStr(tbVoucherNmbr.Text) + ", '" + Format(tbDateToDrawn.SelectedValue, "yyyy-MM-dd") + "', '" + tbBuktiBankNoDrawn.Text + "', '" + tbRemarkDrawn.Text + "',  " + CStr(Session(Request.QueryString("KeyId"))("Year")) + ", " + CStr(Session(Request.QueryString("KeyId"))("Period")) + ", " + QuotedStr(ViewState("UserId").ToString) + ", @A Out Select @A ", ViewState("DBConnection"))
                    ' lbStatus.Text = lbStatus.Text + "Declare @A VARCHAR(255) EXEC S_FNGiroOutDrawn '" + Nmbr(j) + "', '" + Format(tbDateToDrawn.SelectedValue, "yyyy-MM-dd") + "', '" + tbBuktiBankNoDrawn.Text + "', '" + tbRemarkDrawn.Text + "',  " + CStr(Session(Request.QueryString("KeyId"))("Year")) + ", " + CStr(Session(Request.QueryString("KeyId"))("Period")) + ", " + QuotedStr(ViewState("UserId").ToString) + ", @A Out Select @A " + " <br/>"
                    If Trim(result).Length > 5 Then
                        lbStatus.Text = lbStatus.Text + result + " <br/>"
                    End If
                End If
            Next
            BindData("GiroNo In (" + ViewState("ListSelectNmbr") + ") AND Status <> 'C'")
            PnlDt.Visible = False
            PnlHd.Visible = True
        Catch ex As Exception
            lbStatus.Text = "btn SaveDrawn Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveCancel.Click
        Dim Nmbr As String()
        Dim result, ListSelectPayment, Status As String
        Try
            Nmbr = ViewState("Nmbr")
            For j = 0 To Nmbr.Length - 1
                If Nmbr(j) = "" Then
                    Exit For
                Else
                    result = SQLExecuteScalar("Declare @A VARCHAR(255) EXEC S_FNGiroOutCancel '" + Nmbr(j) + "', '" + Format(tbDateCancel.SelectedValue, "yyyy-MM-dd") + "', '" + tbCancelReason.Text + "', " + QuotedStr(ViewState("UserId").ToString) + ", @A Out Select @A ", ViewState("DBConnection"))
                    If Trim(result).Length > 5 Then
                        lbStatus.Text = lbStatus.Text + result + " <br/>"
                    End If
                End If
            Next
            GetListCommand("H", GridView1, "3,8", ListSelectPayment, Nmbr, lbStatus.Text)
            BindData("GiroNo In (" + ViewState("ListSelectNmbr") + ") AND PaymentNo IN (" + ListSelectPayment + ") ")
            PnlDt.Visible = False
            PnlHd.Visible = True
        Catch ex As Exception
            lbStatus.Text = "btn SaveCancel Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnHome_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHome.Click, btnCancel.Click, btnCancelDrawn.Click, btnCancelCancel.Click
        Try
            MovePanel(PnlDt, PnlHd)
        Catch ex As Exception
            lbStatus.Text = "btn Home Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbDateToDrawn_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbDateToDrawn.SelectionChanged
        Dim VoucherBank As String
        VoucherBank = SQLExecuteScalar("Declare @A VarChar(30) EXEC S_SAAutoVoucherNmbr '" + Format(tbDateToDrawn.SelectedValue, "yyyy-MM-dd") + "', '" + ViewState("FgReport").ToString + "', '" + ViewState("BankPayment") + "', 'OUT', @A OUT Select @A ", ViewState("DBConnection").ToString)
        tbVoucherNmbr.Text = VoucherBank
    End Sub


End Class
