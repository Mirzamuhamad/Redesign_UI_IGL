Imports System.Data
Imports System.Data.SqlClient
Partial Class TrChangeGiroIn
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select * From V_FNChangeGiroInHd"

    'TransNmbr, TransDate, STATUS, FgReport, UserType, UserCode, UserName, Attn, Remark
    'DB : ItemNo, Account, AccountName, FgSubled, Subled, SubledName, CostCtr, Currency, ForexRate, AmountForex, Remark
    'CR : ItemNo, PayType, PayName, PayDate, DocumentNo, CurrCode, ForexRate, AmountForex, Remark, DueDate, BankGiro, CurrExpense, BankExpense, RateExpense


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                SetInit()
                Session("AdvanceFilter") = ""
            End If
            lbStatus.Text = ""
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If

            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnUser" Then
                    tbUserCode.Text = Session("Result")(0).ToString
                    tbUserName.Text = Session("Result")(1).ToString
                    BindToText(tbAttn, Session("Result")(2).ToString)
                ElseIf ViewState("Sender") = "btnGiroNo" Then
                    ' "Giro_No, Receipt_Date, Due_Date, Bank_Giro, Currency, Rate, Amount_Forex "
                    tbGiroNoDt.Text = Session("Result")(0).ToString
                    BindToDate(tbReceiptDateDt, Session("Result")(1).ToString)
                    BindToDate(tbDueDateDt, Session("Result")(2).ToString)
                    BindToDropList(ddlBankGiroDt, Session("Result")(3).ToString)
                    BindToDropList(ddlCurrDt, Session("Result")(4).ToString)
                    BindToText(tbRateDt, Session("Result")(5).ToString)
                    BindToText(tbAmountForexDt, Session("Result")(6).ToString)
                    BindToText(tbAmountHomeDt, FormatNumber(CFloat(Session("Result")(6).ToString) * CFloat(Session("Result")(5).ToString), ViewState("DigitHome")))
                    ViewState("DigitCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlCurrDt.SelectedValue), ViewState("DBConnection"))
                    ' ChangeCurrency(ddlCurrDt, tbDate, tbRateDt, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
                    AttachScript("kali(" + Me.tbRateDt.ClientID + "," + Me.tbAmountForexDt.ClientID + "," + Me.tbAmountHomeDt.ClientID + "); setformatdt();", Page, Me.GetType())
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

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        Dim index As Integer
        Try
            index = Int32.Parse(e.Item.Value)
            MultiView1.ActiveViewIndex = index
            btnSaveTrans.Focus()
        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub
    Private Sub SetInit()
        Try
            FillRange(ddlRange)
            lbPayHome.Text = "Receipt (" + ViewState("Currency") + ")"
            lbChargeHome.Text = "Charge (" + ViewState("Currency") + ")"
            lbTotPay.Text = "Receipt (" + ViewState("Currency") + ")"
            lbTotExpense.Text = "Giro (" + ViewState("Currency") + ")"
            lbTotCharge.Text = "Charge (" + ViewState("Currency") + ")"

            FillCombo(ddlCurrDt, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
            FillCombo(ddlCurrDt2, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
            FillCombo(ddlChargeCurrDt2, "EXEC S_GetCurrency", True, "Currency", "Currency", ViewState("DBConnection"))
            'FillCombo(ddlPayTypeDt2, "EXEC S_GetPayTypeUser(" + QuotedStr("ReceiptGiro" + ddlReport.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString) + ")", True, "Payment_Code", "Payment_Name", ViewState("DBConnection"))
            FillCombo(ddlBankGiroDt, "EXEC S_GetBank", True, "Bank_Code", "Bank_Name", ViewState("DBConnection"))
            FillCombo(ddlBankGiroDt2, "EXEC S_GetBank", True, "Bank_Code", "Bank_Name", ViewState("DBConnection"))

            ViewState("PayType") = ""
            ViewState("SortExpression") = Nothing
            ViewState("DigitCurr") = 0
            ViewState("DigitCurrDt") = 0
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
            If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
                ddlCommand.Items.Add("Print")
                ddlCommand2.Items.Add("Print")
            End If
            tbTotalReceipt.Attributes.Add("ReadOnly", "True")
            tbTotalGiro.Attributes.Add("ReadOnly", "True")
            tbTotalCharge.Attributes.Add("ReadOnly", "True")

            tbAmountHomeDt.Attributes.Add("ReadOnly", "True")
            tbChargeHomeDt2.Attributes.Add("ReadOnly", "True")
            tbReceiptHomeDt2.Attributes.Add("ReadOnly", "True")

            tbRateDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbRateDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbChargeRateDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")

            tbAmountForexDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbReceiptForexDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbChargeForexDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")

            tbRateDt.Attributes.Add("OnBlur", "kali(" + Me.tbRateDt.ClientID + "," + Me.tbAmountForexDt.ClientID + "," + Me.tbAmountHomeDt.ClientID + "); setformatdt();")
            tbAmountForexDt.Attributes.Add("OnBlur", "kali(" + Me.tbRateDt.ClientID + "," + Me.tbAmountForexDt.ClientID + "," + Me.tbAmountHomeDt.ClientID + "); setformatdt();")

            tbRateDt2.Attributes.Add("OnBlur", "kali(" + Me.tbRateDt2.ClientID + "," + Me.tbReceiptForexDt2.ClientID + "," + Me.tbReceiptHomeDt2.ClientID + "); setformatdt2();")
            tbReceiptForexDt2.Attributes.Add("OnBlur", "kali(" + Me.tbRateDt2.ClientID + "," + Me.tbReceiptForexDt2.ClientID + "," + Me.tbReceiptHomeDt2.ClientID + "); setformatdt2();")

            tbChargeRateDt2.Attributes.Add("OnBlur", "kali(" + Me.tbChargeRateDt2.ClientID + "," + Me.tbChargeForexDt2.ClientID + "," + Me.tbChargeHomeDt2.ClientID + "); setformatdt2();")
            tbChargeForexDt2.Attributes.Add("OnBlur", "kali(" + Me.tbChargeRateDt2.ClientID + "," + Me.tbChargeForexDt2.ClientID + "," + Me.tbChargeHomeDt2.ClientID + "); setformatdt2();")
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
            btnAdd2.Visible = BtnGo.Visible
            DV = DT.DefaultView
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "TransDate DESC"
            End If
            DV.Sort = ViewState("SortExpression")
            GridView1.DataSource = DV
            GridView1.DataBind()
            If DT.Rows.Count > 0 Then
                GridView1.HeaderRow.Cells(9).Text = "Receipt (" + ViewState("Currency") + ")"
                GridView1.HeaderRow.Cells(10).Text = "Giro (" + ViewState("Currency") + ")"
                GridView1.HeaderRow.Cells(11).Text = "Charge (" + ViewState("Currency") + ")"
            End If
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub
    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT * From V_FNChangeGiroInDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function
    Private Function GetStringDt2(ByVal Nmbr As String) As String
        Return "SELECT * From V_FNChangeGiroInPay WHERE TransNmbr = " + QuotedStr(Nmbr)
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
                Session("SelectCommand") = "EXEC S_FNFormBuktiBank " + Result + ",'CHANGEGIROIN'"
                Session("ReportFile") = ".../../../Rpt/FormBuktiBank.frx"
                Session("DBConnection") = ViewState("DBConnection")
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
                        Result = ExecSPCommandGo(ActionValue, "S_FNChangeGiroIn", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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

    Private Sub EnableHd(ByVal State As Boolean)
        Try
            'ddlReport.Enabled = State And ViewState("StateHd") = "Insert"
            tbUserCode.Enabled = State
            btnUser.Visible = State
            ddlUserType.Enabled = State
        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub BindDataDt(ByVal Nmbr As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt") = Nothing
            dt = SQLExecuteQuery(GetStringDt(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt") = dt
            BindGridDt(dt, GridDt)
            If dt.Rows.Count > 0 Then
                GridDt.HeaderRow.Cells(8).Text = "Amount (" + ViewState("Currency") + ")"
            End If
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub
    Private Sub BindDataDt2(ByVal Nmbr As String)
        Try
            Dim dt As New DataTable
            Dim dr As DataRow
            ViewState("Dt2") = Nothing
            dt = SQLExecuteQuery(GetStringDt2(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt2") = dt
            BindGridDt(dt, GridDt2)
            If dt.Rows.Count > 0 Then
                GridDt2.HeaderRow.Cells(9).Text = "Receipt (" + ViewState("Currency") + ")"
                GridDt2.HeaderRow.Cells(10).Text = "Charge (" + ViewState("Currency") + ")"
            End If
            For Each dr In dt.Rows
                If dr("FgMode").ToString = "B" Or dr("FgMode").ToString = "K" Or dr("FgMode").ToString = "G" Or dr("FgMode").ToString = "D" Then
                    ViewState("PayType") = dt.Rows(0)("ReceiptType").ToString
                    Exit For
                End If
            Next
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
    Private Sub StatusButtonSave(ByVal Bool As Boolean)
        Try
            btnSaveAll.Visible = Bool
            btnSaveTrans.Visible = Bool
            btnBack.Visible = Bool
        Catch ex As Exception
            Throw New Exception("Status Button Save Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnCancelDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelDt.Click
        Try
            MovePanel(pnlEditDt, PnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnCancelDt2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelDt2.Click
        Try
            MovePanel(pnlEditDt2, pnlDt2)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt2  Error : " + ex.ToString
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbCode.Text = ""
            tbUserCode.Text = ""
            tbUserName.Text = ""
            'ddlReport.SelectedValue = "Y"
            ddlUserType.SelectedValue = "Customer"
            tbDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            tbTotalReceipt.Text = "0"
            tbTotalGiro.Text = "0"
            tbTotalCharge.Text = "0"
            tbAttn.Text = ""
            tbRemark.Text = ""
            FillCombo(ddlPayTypeDt2, "EXEC S_GetPayTypeUser " + QuotedStr("ReceiptGiro" + "Y") + ", " + QuotedStr(ViewState("UserId").ToString), True, "Payment_Code", "Payment_Name", ViewState("DBConnection")) 'ddlReport.SelectedValue
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbGiroNoDt.Text = ""
            tbReceiptDateDt.SelectedDate = Nothing
            tbDueDateDt.SelectedDate = Nothing
            ddlBankGiroDt.SelectedIndex = 0
            ddlCurrDt.SelectedValue = ViewState("Currency")
            tbAmountForexDt.Text = "0"
            tbAmountHomeDt.Text = "0"
            tbRemarkDt.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt2()
        Try
            ddlPayTypeDt2.SelectedIndex = 0
            tbReceiptDateDt2.SelectedDate = ViewState("ServerDate") 'Now.Date
            tbDocumentNoDt2.Text = ""
            tbVoucherNo.Enabled = False
            tbVoucherNo.Text = ""
            tbDueDateDt2.SelectedDate = Nothing
            ddlCurrDt2.SelectedValue = ViewState("Currency")
            tbReceiptForexDt2.Text = "0"
            tbReceiptHomeDt2.Text = "0"
            tbChargeRateDt2.Text = "0"
            tbChargeForexDt2.Text = "0"
            tbChargeHomeDt2.Text = "0"
            tbRemarkDt2.Text = ""
            ChangeCurrency(ddlCurrDt2, tbDate, tbRateDt2, ViewState("Currency"), ViewState("DigitCurrDt"), ViewState("DBConnection"))
            ChangePaymentType(ddlPayTypeDt2.SelectedValue, tbFgModeDt2, tbReceiptDateDt2, tbDueDateDt2, ddlBankGiroDt2, ddlCurrDt2, ddlChargeCurrDt2, tbRateDt2, tbChargeRateDt2, tbChargeForexDt2, ViewState("Currency"), ViewState("DigitCurrDt"), ViewState("DBConnection"))
        Catch ex As Exception
            Throw New Exception("Clear Dt 2 Error " + ex.ToString)
        End Try
    End Sub

    Function CekHd() As Boolean
        Try
            If tbDate.SelectedDate = Nothing Then
                lbStatus.Text = MessageDlg("Date must have value")
                tbDate.Focus()
                Return False
            End If
            'If CInt(Session(Request.QueryString("KeyId"))("Year")) <> Year(tbDate.SelectedValue) Or CInt(Session(Request.QueryString("KeyId"))("Period")) <> Month(tbDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(Session(Request.QueryString("KeyId"))("Period")) + " " + Session(Request.QueryString("KeyId"))("Year").ToString.Trim)
            '    Return False
            'End If
            If tbUserCode.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("User must have value")
                tbUserCode.Focus()
                Return False
            End If
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
                If Dr("OldGiro").ToString = "" Then
                    lbStatus.Text = MessageDlg("Giro No Must Have Value")
                    Return False
                End If
                If Dr("Currency").ToString = "" Then
                    lbStatus.Text = MessageDlg("Currency Must Have Value")
                    Return False
                End If
                If CFloat(Dr("ForexRate").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Currency Rate Must Have Value")
                    Return False
                End If
                If Dr("Currency").ToString <> ViewState("Currency") And CFloat(Dr("ForexRate").ToString) = 1 Then
                    lbStatus.Text = MessageDlg("Currency Rate Must Have Value")
                    Return False
                End If
                If CFloat(Dr("AmountForex").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Amount Forex Must Have Value")
                    Return False
                End If
            Else
                If tbGiroNoDt.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Giro No Must Have Value")
                    tbGiroNoDt.Focus()
                    Return False
                End If
                If ddlCurrDt.SelectedValue = "" Then
                    lbStatus.Text = MessageDlg("Currency Must Have Value")
                    ddlCurrDt.Focus()
                    Return False
                End If
                If CFloat(tbRateDt.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Currency Rate Must Have Value")
                    tbRateDt.Focus()
                    Return False
                End If
                If CFloat(tbRateDt.Text) = 1 And ddlCurrDt.SelectedValue <> ViewState("Currency") Then
                    lbStatus.Text = MessageDlg("Currency Rate Must Have Value")
                    tbRateDt.Focus()
                    Return False
                End If
                If CFloat(tbAmountForexDt.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Amount Forex Must Have Value")
                    tbAmountForexDt.Focus()
                    Return False
                End If
            End If
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
                If Dr("ReceiptType").ToString = "" Then
                    lbStatus.Text = MessageDlg("Receipt Type Must Have Value")
                    Return False
                End If
                If Dr("FgMode").ToString = "G" Then
                    If Dr("DocumentNo").ToString = "" Then
                        lbStatus.Text = MessageDlg("Document No Must Have Value")
                        Return False
                    End If
                    If Dr("BankGiro").ToString = "" Then
                        lbStatus.Text = MessageDlg("Bank Giro Must Have Value")
                        Return False
                    End If
                    If Dr("Due Date").ToString = "" Then
                        lbStatus.Text = MessageDlg("Due Date Must Have Value")
                        Return False
                    End If
                End If
                If Dr("FgMode").ToString = "B" Or Dr("FgMode").ToString = "K" Then
                    If Dr("Reference").ToString = "" Then
                        lbStatus.Text = MessageDlg("Voucher No Must Have Value")
                        Return False
                    End If
                End If
            Else
                If ddlPayTypeDt2.SelectedValue = "" Then
                    lbStatus.Text = MessageDlg("Receipt Type Must Have Value")
                    ddlPayTypeDt2.Focus()
                    Return False
                End If
                If tbFgModeDt2.Text = "G" Then
                    If tbDocumentNoDt2.Text.Trim = "" Then
                        lbStatus.Text = MessageDlg("Document No Must Have Value")
                        tbDocumentNoDt2.Focus()
                        Return False
                    End If
                    If ddlBankGiroDt2.SelectedValue = "" Then
                        lbStatus.Text = MessageDlg("Bank Giro Must Have Value")
                        ddlBankGiroDt2.Focus()
                        Return False
                    End If
                    If tbDueDateDt2.SelectedDate = Nothing Then
                        lbStatus.Text = MessageDlg("Due Date Must Have Value")
                        tbDueDateDt2.Focus()
                        Return False
                    End If
                End If
                If tbFgModeDt2.Text = "B" Or tbFgModeDt2.Text = "K" Then
                    If tbVoucherNo.Text.Trim = "" Then
                        lbStatus.Text = MessageDlg("Voucher No Must Have Value")
                        tbVoucherNo.Focus()
                        Return False
                    End If
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            tbCode.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            'BindToDropList(ddlReport, Dt.Rows(0)("FgReport").ToString)
            BindToDropList(ddlUserType, Dt.Rows(0)("UserType").ToString)
            BindToText(tbUserCode, Dt.Rows(0)("UserCode").ToString)
            BindToText(tbUserName, Dt.Rows(0)("UserName").ToString)
            BindToText(tbAttn, Dt.Rows(0)("Attn").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxDt(ByVal OldGiroNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("OldGiro = " + QuotedStr(OldGiroNo))
            If Dr.Length > 0 Then
                ' FgSubled, Subled, SubledName, CostCtr, Currency, ForexRate, AmountForex, Remark
                BindToText(tbGiroNoDt, Dr(0)("OldGiro").ToString)
                BindToDate(tbReceiptDateDt, Dr(0)("ReceiptDate").ToString)
                BindToDate(tbDueDateDt, Dr(0)("DueDate").ToString)
                BindToDropList(ddlBankGiroDt, Dr(0)("BankGiro").ToString)
                BindToDropList(ddlCurrDt, Dr(0)("Currency").ToString)
                BindToText(tbRateDt, Dr(0)("ForexRate").ToString)
                BindToText(tbAmountForexDt, Dr(0)("AmountForex").ToString)
                BindToText(tbAmountHomeDt, Dr(0)("AmountHome").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
                ViewState("DigitCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlCurrDt.SelectedValue), ViewState("DBConnection"))
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxDt2(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt2").select("ItemNo = " + ItemNo)
            If Dr.Length > 0 Then
                lbItemNoDt2.Text = ItemNo
                BindToDropList(ddlPayTypeDt2, Dr(0)("ReceiptType").ToString)
                BindToDate(tbReceiptDateDt2, Dr(0)("ReceiptDate").ToString)
                BindToText(tbDocumentNoDt2, Dr(0)("DocumentNo").ToString)
                BindToText(tbVoucherNo, Dr(0)("Reference").ToString)
                BindToDate(tbDueDateDt2, Dr(0)("DueDate").ToString)
                BindToDropList(ddlCurrDt2, Dr(0)("Currency").ToString)
                BindToText(tbRateDt2, Dr(0)("ForexRate").ToString)
                BindToText(tbReceiptForexDt2, Dr(0)("ReceiptForex").ToString)
                BindToText(tbReceiptHomeDt2, Dr(0)("ReceiptHome").ToString)
                BindToText(tbRemarkDt2, Dr(0)("Remark").ToString)
                BindToText(tbFgModeDt2, Dr(0)("FgMode").ToString)
                BindToDropList(ddlBankGiroDt2, Dr(0)("BankGiro").ToString)
                BindToDropList(ddlChargeCurrDt2, Dr(0)("ChargeCurrency").ToString)
                ViewState("DigitCurrDt") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlCurrDt2.SelectedValue), ViewState("DBConnection"))
                ViewState("DigitExpenseCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlChargeCurrDt2.SelectedValue), ViewState("DBConnection"))
                BindToText(tbChargeRateDt2, Dr(0)("ChargeRate").ToString)
                BindToText(tbChargeForexDt2, Dr(0)("ChargeForex").ToString)
                BindToText(tbChargeHomeDt2, Dr(0)("ChargeHome").ToString)

                If ddlChargeCurrDt2.SelectedValue = "" Then
                    tbChargeForexDt2.Text = "0"
                    tbChargeHomeDt2.Text = "0"
                End If
                tbChargeForexDt2.Enabled = ddlChargeCurrDt2.SelectedValue <> ""
                tbRateDt2.Enabled = ddlCurrDt2.SelectedValue <> Session("Currency")
                ddlBankGiroDt2.Enabled = tbFgModeDt2.Text = "G"
                tbDueDateDt2.Enabled = tbFgModeDt2.Text = "G"
                ddlChargeCurrDt2.Enabled = tbFgModeDt2.Text = "B"
                tbChargeForexDt2.Enabled = tbFgModeDt2.Text = "B"
                tbChargeRateDt2.Enabled = ddlChargeCurrDt2.SelectedValue <> Session("Currency") And tbFgModeDt2.Text = "B"

                tbVoucherNo.Enabled = (tbFgModeDt2.Text = "B" Or tbFgModeDt2.Text = "K")
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail 2 error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt.Click
        Try
            If CekDt() = False Then
                btnSaveDt.Focus()
                Exit Sub
            End If

            If ViewState("StateDt") = "Edit" Then
                Dim Row As DataRow
                Row = ViewState("Dt").Select("OldGiro = " + QuotedStr(tbGiroNoDt.Text))(0)
                Row.BeginEdit()
                Row("OldGiro") = tbGiroNoDt.Text
                Row("ReceiptDate") = tbReceiptDateDt.SelectedDate
                Row("DueDate") = tbDueDateDt.SelectedDate
                Row("BankGiro") = ddlBankGiroDt.SelectedValue
                Row("BankGiroName") = ddlBankGiroDt.SelectedItem.Text
                Row("Currency") = ddlCurrDt.SelectedValue
                Row("ForexRate") = tbRateDt.Text
                Row("AmountForex") = tbAmountForexDt.Text
                Row("AmountHome") = tbAmountHomeDt.Text
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("OldGiro") = tbGiroNoDt.Text
                dr("DueDate") = tbDueDateDt.SelectedDate
                dr("ReceiptDate") = tbReceiptDateDt.SelectedDate
                dr("BankGiro") = ddlBankGiroDt.SelectedValue
                dr("BankGiroName") = ddlBankGiroDt.SelectedItem.Text
                dr("Currency") = ddlCurrDt.SelectedValue
                dr("ForexRate") = tbRateDt.Text
                dr("AmountForex") = tbAmountForexDt.Text
                dr("AmountHome") = tbAmountHomeDt.Text
                dr("Remark") = tbRemarkDt.Text
                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, PnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
            BindGridDt(ViewState("Dt"), GridDt)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save Dt Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub

    Protected Sub btnSaveDt2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt2.Click
        Try
            If Session("PeriodInfo")("1Payment").ToString = "Y" Then
                If GetCountRecord(ViewState("Dt2")) >= 1 Then
                    If tbFgModeDt2.Text = "K" Or tbFgModeDt2.Text = "B" Or tbFgModeDt2.Text = "G" Or tbFgModeDt2.Text = "D" Then
                        If ViewState("StateDt2") <> "Edit" Then
                            If ddlPayTypeDt2.SelectedValue <> ViewState("PayType").ToString Then
                                lbStatus.Text = "Cannot input more than one receipt type"
                                Exit Sub
                            End If
                        End If
                    End If
                End If
            End If
            If CekDt2() = False Then
                btnSaveDt2.Focus()
                Exit Sub
            End If
            If tbFgModeDt2.Text = "G" Then
                If CekExistGiroIn(tbDocumentNoDt2.Text.Trim, ViewState("DBConnection").ToString) = True Then
                    lbStatus.Text = "Giro Payment '" + tbDocumentNoDt2.Text.Trim + "' has already exists in Giro Listing'"
                    Exit Sub
                End If
            End If
            If ViewState("StateDt2") = "Edit" Then
                Dim Row As DataRow
                Row = ViewState("Dt2").Select("ItemNo = " + lbItemNoDt2.Text)(0)
                Row.BeginEdit()
                Row("ReceiptType") = ddlPayTypeDt2.SelectedValue
                Row("ReceiptName") = ddlPayTypeDt2.SelectedItem.Text
                Row("ReceiptDate") = Format(tbReceiptDateDt2.SelectedDate, "dd MMMM yyyy")
                Row("DocumentNo") = tbDocumentNoDt2.Text
                Row("Reference") = tbVoucherNo.Text
                If tbDueDateDt2.Enabled Then
                    Row("DueDate") = Format(tbDueDateDt2.SelectedDate, "dd MMMM yyyy")
                Else
                    Row("DueDate") = DBNull.Value
                End If
                Row("BankGiro") = ddlBankGiroDt2.SelectedValue
                Row("BankGiroName") = ddlBankGiroDt2.SelectedItem.Text
                Row("FgMode") = tbFgModeDt2.Text
                Row("Currency") = ddlCurrDt2.SelectedValue
                Row("ForexRate") = tbRateDt2.Text
                Row("ReceiptForex") = tbReceiptForexDt2.Text
                Row("ReceiptHome") = tbReceiptHomeDt2.Text
                Row("Remark") = tbRemarkDt2.Text
                Row("ChargeCurrency") = ddlChargeCurrDt2.SelectedValue
                Row("ChargeRate") = tbChargeRateDt2.Text
                Row("ChargeForex") = tbChargeForexDt2.Text
                Row("ChargeHome") = tbChargeHomeDt2.Text
                Row("Remark") = tbRemarkDt2.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow

                dr = ViewState("Dt2").NewRow
                dr("ItemNo") = CInt(lbItemNoDt2.Text)
                dr("ReceiptType") = ddlPayTypeDt2.SelectedValue
                dr("ReceiptName") = ddlPayTypeDt2.SelectedItem.Text
                dr("ReceiptDate") = Format(tbReceiptDateDt2.SelectedDate, "dd MMMM yyyy")
                dr("DocumentNo") = tbDocumentNoDt2.Text
                dr("Reference") = tbVoucherNo.Text
                If tbDueDateDt2.Enabled Then
                    dr("DueDate") = Format(tbDueDateDt2.SelectedDate, "dd MMMM yyyy")
                Else
                    dr("DueDate") = DBNull.Value
                End If
                dr("BankGiro") = ddlBankGiroDt2.SelectedValue
                dr("BankGiroName") = ddlBankGiroDt2.SelectedItem.Text
                dr("FgMode") = tbFgModeDt2.Text
                dr("Currency") = ddlCurrDt2.SelectedValue
                dr("ForexRate") = tbRateDt2.Text
                dr("ReceiptForex") = tbReceiptForexDt2.Text
                dr("ReceiptHome") = tbReceiptHomeDt2.Text
                dr("Remark") = tbRemarkDt2.Text
                dr("ChargeCurrency") = ddlChargeCurrDt2.Text
                dr("ChargeRate") = tbChargeRateDt2.Text
                dr("ChargeForex") = tbChargeForexDt2.Text
                dr("ChargeHome") = tbChargeHomeDt2.Text
                dr("Remark") = tbRemarkDt2.Text
                ViewState("Dt2").Rows.Add(dr)
            End If
            If tbFgModeDt2.Text = "K" Or tbFgModeDt2.Text = "B" Or tbFgModeDt2.Text = "G" Or tbFgModeDt2.Text = "D" Then
                ViewState("PayType") = ddlPayTypeDt2.SelectedValue
            End If
            MovePanel(pnlEditDt2, pnlDt2)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
            BindGridDt(ViewState("Dt2"), GridDt2)
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
            'Save Hd

            'TransNmbr, TransDate, STATUS, FgReport, UserType, UserCode, UserName, Attn, Remark
            'DB : ItemNo, Account, AccountName, FgSubled, Subled, SubledName, CostCtr, Currency, ForexRate, AmountForex, Remark
            'CR : ItemNo, ReceiptType, ReceiptName, ReceiptDate, DocumentNo, Currency, ForexRate, ReceiptForex, ReceiptHome, Remark, DueDate, BankGiro, ChargeCurrency, ChargeRate, ChargeForex, ChargeHome
            If Not (ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit") Then
                Exit Sub
            Else
                ViewState("StateSave") = ViewState("StateHd")
                ViewState("StateHd") = "View"
            End If
            'Save Hd
            If ViewState("StateSave") = "Insert" Then
                'ddlReport.SelectedValue
                tbCode.Text = GetAutoNmbr("CGR", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), ViewState("PayType").ToString, ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO FINChangeGiroInHd (TransNmbr, TransDate, STATUS, FgReport, " + _
                "UserType, UserCode, Attn, TotalReceipt, TotalGiro, TotalCharge, Remark, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", 'H', " + _
                QuotedStr("Y") + ", " + QuotedStr(ddlUserType.SelectedValue) + ", " + _
                QuotedStr(tbUserCode.Text) + ", " + QuotedStr(tbAttn.Text) + ", " + tbTotalReceipt.Text.Replace(",", "") + ", " + tbTotalGiro.Text.Replace(",", "") + ", " + tbTotalCharge.Text.Replace(",", "") + _
                ", " + QuotedStr(tbRemark.Text) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", getDate()"

                ViewState("TransNmbr") = tbCode.Text

            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM FINChangeGiroInHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE FINChangeGiroInHd SET UserType=" + QuotedStr(ddlUserType.SelectedValue) + ", UserCode=" + QuotedStr(tbUserCode.Text) + _
                ", Attn =" + QuotedStr(tbAttn.Text) + ", TotalReceipt = " + tbTotalReceipt.Text.Replace(",", "") + ", TotalGiro = " + tbTotalGiro.Text.Replace(",", "") + ", TotalCharge = " + tbTotalCharge.Text.Replace(",", "") + ", Remark = " + QuotedStr(tbRemark.Text) + _
                ", DatePrep = getDate()" + _
                " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            End If
            SQLString = ChangeQuoteNull(SQLString)
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

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, OldGiro, ReceiptDate, DueDate, BankGiro, Currency, ForexRate, AmountForex, AmountHome, Remark FROM FINChangeGiroInDt WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt As New DataTable("FINChangeGiroInDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt

            'save dt2
            cmdSql = New SqlCommand("SELECT TransNmbr, ItemNo, ReceiptType, ReceiptDate, DocumentNo, Reference, DueDate, BankGiro, FgMode, Currency, ForexRate, ReceiptForex, ReceiptHome, Remark, ChargeCurrency, ChargeRate, ChargeForex, ChargeHome FROM FINChangeGiroInPay WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            dbcommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt2 As New DataTable("FINChangeGiroInPay")

            Dt2 = ViewState("Dt2")
            da.Update(Dt2)
            Dt2.AcceptChanges()
            ViewState("Dt2") = Dt2
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
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail Giro must have at least 1 record")
                Exit Sub
            End If
            'If GetCountRecord(ViewState("Dt2")) = 0 Then
            If ViewState("PayType").ToString = "" Then
                lbStatus.Text = MessageDlg("Detail Receipt must have at least 1 record")
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
        Catch ex As Exception
            lbStatus.Text = "btnSaveTrans Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, PnlDt, GridDt)
            ModifyInput2(True, pnlInput, pnlDt2, GridDt2)

            newTrans()
            btnHome.Visible = False
            MultiView1.ActiveViewIndex = 0
            Menu1.Items.Item(0).Selected = True
            tbCode.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdddt.Click, btnAddDtke2.Click
        Try
            Cleardt()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt") = "Insert"
            MovePanel(PnlDt, pnlEditDt)
            EnableHd(False)
            StatusButtonSave(False)
            tbGiroNoDt.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnAddDt2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt2.Click, btnAddDt2Ke2.Click
        Try
            Cleardt2()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt2") = "Insert"
            MovePanel(pnlDt2, pnlEditDt2)
            EnableHd(False)
            StatusButtonSave(False)
            lbItemNoDt2.Text = GetNewItemNo(ViewState("Dt2"))
            If Session("PeriodInfo")("1Payment").ToString = "Y" Then
                BindToDropList(ddlPayTypeDt2, ViewState("PayType").ToString.Trim)
            End If
            ddlPayTypeDt2.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ViewState("PayType") = ""
            ViewState("DigitCurr") = 0
            ViewState("DigitCurrDt") = 0
            ViewState("TransNmbr") = ""
            ClearHd()
            Cleardt()
            Cleardt2()
            PnlDt.Visible = True
            BindDataDt("")
            BindDataDt2("")
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
            FDateName = "Receipt Date"
            FDateValue = "TransDate"
            FilterName = "Receipt No, Receipt Date, User Type, User Receipt, Attn, Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), UserType, UserReceipt, Attn, Remark"
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
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, PnlDt, GridDt)
                    ModifyInput2(False, pnlInput, pnlDt2, GridDt2)
                    MultiView1.ActiveViewIndex = 0
                    Menu1.Items.Item(0).Selected = True
                    btnHome.Visible = True
                ElseIf DDL.SelectedValue = "Edit" Then
                    If GVR.Cells(3).Text = "H" Or GVR.Cells(3).Text = "G" Then
                        CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        MovePanel(PnlHd, pnlInput)
                        ViewState("TransNmbr") = GVR.Cells(2).Text
                        GridDt.PageIndex = 0
                        BindDataDt(ViewState("TransNmbr"))
                        BindDataDt2(ViewState("TransNmbr"))
                        FillTextBoxHd(ViewState("TransNmbr"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, PnlDt, GridDt)
                        ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
                        btnHome.Visible = False
                        MultiView1.ActiveViewIndex = 0
                        Menu1.Items.Item(0).Selected = True
                        FillCombo(ddlPayTypeDt2, "EXEC S_GetPayTypeUser " + QuotedStr("ReceiptGiro" + "Y") + ", " + QuotedStr(ViewState("UserId").ToString), True, "Payment_Code", "Payment_Name", ViewState("DBConnection")) 'ddlReport.SelectedValue
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
                    Else
                        lbStatus.Text = MessageDlg("Data must be Hold or Get Approval to edit")
                        Exit Sub
                    End If
                ElseIf DDL.SelectedValue = "Print" Then
                    Try
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        Session("SelectCommand") = "EXEC S_FNFormBuktiBank '''" + GVR.Cells(2).Text + "''','CHANGEGIROIN'"
                        'lbStatus.Text = Session("SelectCommand")
                        'Exit Sub
                        Session("ReportFile") = ".../../../Rpt/FormBuktiBank.frx"
                        Session("DBConnection") = ViewState("DBConnection")
                        AttachScript("openprintdlg();", Page, Me.GetType)
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

    Dim TotalGiro As Decimal = 0
    ' untuk tampilkan data total di grid
    Protected Sub GridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt.RowDataBound
        Try
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "OldGiro")) Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                    ' add the UnitPrice and QuantityTotal to the running total variables
                    TotalGiro += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "AmountHome"))
                ElseIf e.Row.RowType = DataControlRowType.Footer Then
                    tbTotalGiro.Text = FormatNumber(TotalGiro, ViewState("DigitHome"))
                End If
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow
            GVR = GridDt.Rows(e.RowIndex)
            dr = ViewState("Dt").Select("OldGiro = " + QuotedStr(GVR.Cells(1).Text))
            dr(0).Delete()
            BindGridDt(ViewState("Dt"), GridDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Dim TotalReceipt As Decimal = 0
    Dim TotalCharge As Decimal = 0
    Protected Sub GridDt2_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt2.RowDataBound
        Try
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "ItemNo")) Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                    ' add the UnitPrice and QuantityTotal to the running total variables
                    TotalReceipt += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "ReceiptHome"))
                    TotalCharge += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "ChargeHome"))
                ElseIf e.Row.RowType = DataControlRowType.Footer Then
                    tbTotalReceipt.Text = FormatNumber(TotalReceipt, ViewState("DigitHome"))
                    tbTotalCharge.Text = FormatNumber(TotalCharge, ViewState("DigitHome"))
                End If
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub GridDt2_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt2.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow
            GVR = GridDt2.Rows(e.RowIndex)
            dr = ViewState("Dt2").Select("ItemNo = " + GVR.Cells(1).Text)
            dr(0).Delete()
            BindGridDt(ViewState("Dt2"), GridDt2)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt 2 Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            FillTextBoxDt(GVR.Cells(1).Text)
            MovePanel(PnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            btnSaveDt.Focus()
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub GridDt2_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt2.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt2.Rows(e.NewEditIndex)
            FillTextBoxDt2(GVR.Cells(1).Text)
            MovePanel(pnlDt2, pnlEditDt2)
            EnableHd(False)
            'ChangePaymentType(ddlPayTypeDt2.SelectedValue, tbFgModeDt2, tbReceiptDateDt2, tbDueDateDt2, ddlBankGiroDt2, ddlCurrDt2, ddlChargeCurrDt2, tbRateDt2, tbChargeRateDt2, tbChargeForexDt2, ViewState("Currency"), ViewState("DigitCurrDt"), ViewState("DBConnection"), "Edit")
            ViewState("StateDt2") = "Edit"
            StatusButtonSave(False)
            btnSaveDt2.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid dt2 Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlPayTypeDt2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPayTypeDt2.SelectedIndexChanged
        Try
            Dim dr As DataRow
            Dim VoucherNo As String
            dr = FindMaster("PayType", ddlPayTypeDt2.SelectedValue, ViewState("DBConnection"))
            If Not dr Is Nothing Then
                BindToText(tbFgModeDt2, dr("FgMode"))
                'BindToDropList(ddlReport, dr("FgReport"))
                BindToDropList(ddlCurrDt2, dr("Currency"))
            Else
                tbFgModeDt2.Text = "B"
            End If
            ChangePaymentType(ddlPayTypeDt2.SelectedValue, tbFgModeDt2, tbReceiptDateDt2, tbDueDateDt2, ddlBankGiroDt2, ddlCurrDt2, ddlChargeCurrDt2, tbRateDt2, tbChargeRateDt2, tbChargeForexDt2, ViewState("Currency"), ViewState("DigitCurrDt"), ViewState("DBConnection"))
            ViewState("DigitExpenseCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlChargeCurrDt2.SelectedValue), ViewState("DBConnection"))
            AttachScript("kali(" + Me.tbRateDt2.ClientID + "," + Me.tbReceiptForexDt2.ClientID + "," + Me.tbReceiptHomeDt2.ClientID + "); kali(" + Me.tbChargeRateDt2.ClientID + "," + Me.tbChargeForexDt2.ClientID + "," + Me.tbChargeHomeDt2.ClientID + "); setformatdt2();", Page, Me.GetType())

            VoucherNo = ""
            If tbFgModeDt2.Text = "B" Or tbFgModeDt2.Text = "K" Then
                VoucherNo = SQLExecuteScalar("Declare @A VarChar(255) EXEC S_SAAutoVoucherNmbr " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr("Y") + ", " + QuotedStr(ddlPayTypeDt2.SelectedValue) + ", 'IN', @A OUT SELECT @A", ViewState("DBConnection").ToString) 'ddlReport.SelectedValue
            End If
            tbVoucherNo.Enabled = (tbFgModeDt2.Text = "B" Or tbFgModeDt2.Text = "K")
            tbVoucherNo.Text = VoucherNo
            btnSaveDt.Focus()
        Catch ex As Exception
            lbStatus.Text = "ddl Pay Type Select Index Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnGiroNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGiroNo.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT * FROM V_FNGiroInPending WHERE Report = " + QuotedStr("Y") + " and User_Type = " + QuotedStr(ddlUserType.SelectedValue) + " and User_Code = " + QuotedStr(tbUserCode.Text) 'ddlReport.SelectedValue
            ResultField = "Giro_No, Receipt_Date, Due_Date, Bank_Giro, Currency, Rate, Amount_Forex "
            ViewState("Sender") = "btnGiroNo"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "Btn Giro Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnUser_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUser.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT User_Code, User_Name, Contact_Person FROM VMsUserType WHERE User_Type = " + QuotedStr(ddlUserType.SelectedValue)
            ResultField = "User_Code, User_Name, Contact_Person"
            ViewState("Sender") = "btnUser"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "Btn User Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbUserCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbUserCode.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("UserType", tbUserCode.Text + "|" + ddlUserType.SelectedValue, ViewState("DBConnection"))
            If Not Dr Is Nothing Then
                tbUserCode.Text = Dr("User_Code")
                tbUserName.Text = Dr("User_Name")
                tbAttn.Text = Dr("Contact_Person")
            Else
                tbUserCode.Text = ""
                tbUserName.Text = ""
                tbAttn.Text = ""
            End If
        Catch ex As Exception
            lbStatus.Text = "tb User Code Text Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlChargeCurrDt2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlChargeCurrDt2.SelectedIndexChanged
        Try
            ChangeCurrency(ddlChargeCurrDt2, tbReceiptDateDt2, tbChargeRateDt2, ViewState("Currency"), ViewState("DigitExpenseCurr"), ViewState("DBConnection"))
            If ddlChargeCurrDt2.SelectedValue = "" Then
                tbChargeForexDt2.Text = "0"
                tbChargeHomeDt2.Text = "0"
            End If
            tbChargeForexDt2.Enabled = ddlChargeCurrDt2.SelectedValue <> ""
            tbChargeRateDt2.Focus()
        Catch ex As Exception
            lbStatus.Text = "ddl curr expense selected index changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlUserType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlUserType.SelectedIndexChanged
        tbUserCode.Text = ""
        tbUserName.Text = ""
        tbAttn.Text = ""
    End Sub

    'Protected Sub ddlReport_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlReport.SelectedIndexChanged
    '    FillCombo(ddlPayTypeDt2, "EXEC S_GetPayTypeUser " + QuotedStr("ReceiptGiro" + ddlReport.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString), True, "Payment_Code", "Payment_Name", ViewState("DBConnection"))
    'End Sub

    Protected Sub btnSaveAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveAll.Click
        Dim CurrFilter, Value As String
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail Giro must have at least 1 record")
                Exit Sub
            End If
            'If GetCountRecord(ViewState("Dt2")) = 0 Then
            If ViewState("PayType").ToString = "" Then
                lbStatus.Text = MessageDlg("Detail Receipt must have at least 1 record")
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

End Class
