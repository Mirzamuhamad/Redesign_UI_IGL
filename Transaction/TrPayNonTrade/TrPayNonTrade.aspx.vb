Imports System.Data
Imports System.Data.SqlClient
Imports BasicFrame.WebControls
Partial Class Transaction_TrPayNonTrade_TrPayNonTrade
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select distinct TransNmbr, Nmbr, TransDate, Status, FgReport, UserType, Usercode, UserName, UserPayment, Attn, Currency, TotalPayForexStr, TotalPayForex, TotalPayment, TotalOthers, TotalExpense, TotalCharge, TotalSelisih, Remark From V_FNPayNonTradeHd"

    'TransNmbr, TransDate, STATUS, FgReport, UserType, UserCode, UserName, Attn, Remark
    'DB : ItemNo, Account, AccountName, FgSubled, Subled, SubledName, CostCtr, Currency, ForexRate, AmountForex, Remark
    'CR : ItemNo, PayType, PayName, PayDate, DocumentNo, CurrCode, ForexRate, AmountForex, Remark, DueDate, BankPayment, CurrExpense, BankExpense, RateExpense


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
                ElseIf ViewState("Sender") = "btnDocNo" Then
                    tbDocumentNoDt2.Text = Session("Result")(0).ToString
                    ddlCurrDt2.SelectedValue = Session("Result")(1).ToString
                    tbRateDt2.Text = Session("Result")(2).ToString
                    tbPaymentForexDt2.Text = (CFloat(Session("Result")(3).ToString) - CFloat(Session("Result")(4).ToString)).ToString
                    tbRateDt2.Enabled = False
                    If tbUserCode.Text.Trim = "" Then
                        BindToDropList(ddlUserType, Session("Result")(5).ToString)
                        BindToText(tbUserCode, Session("Result")(6).ToString)
                        BindToText(tbUserName, Session("Result")(7).ToString)
                    End If
                    AttachScript("setformatdt()", Page, Me.GetType)
                ElseIf ViewState("Sender") = "btnAccount" Then
                    tbAccountDt.Text = Session("Result")(0).ToString
                    tbAccountNameDt.Text = Session("Result")(1).ToString
                    tbFgSubledDt.Text = Session("Result")(2).ToString
                    BindToDropList(ddlCurrDt, Session("Result")(3).ToString)
                    tbFgType.Text = Session("Result")(4).ToString.ToUpper
                    tbFgCostCtr.Text = Session("Result")(5).ToString
                    ddlCurrDt.Enabled = (tbFgType.Text = "PL")
                    tbSubledDt.Enabled = tbFgSubledDt.Text <> "N"
                    btnSubled.Visible = tbSubledDt.Enabled
                    ViewState("FgType") = tbFgType.Text

                    tbPPnNo.Enabled = (tbAccountDt.Text = ViewState("AccPPn") Or tbAccountDt.Text = ViewState("AccPPn2"))
                    tbPPndate.Enabled = tbPPnNo.Enabled

                    If ViewState("FgType") = "BS" Then
                        ddlCurrDt.Enabled = False
                    Else : ddlCurrDt.Enabled = True
                    End If

                    ddlCostCenterDt.Enabled = tbFgCostCtr.Text <> "N"
                    If tbFgSubledDt.Text = "N" Then
                        tbSubledDt.Text = ""
                        tbSubledNameDt.Text = ""
                    End If
                    If tbFgCostCtr.Text = "N" Then
                        ddlCostCenterDt.SelectedIndex = 0
                    End If
                    btnSubled.Enabled = tbSubledDt.Enabled()

                    ViewState("DigitCurrAcc") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlCurrDt.SelectedValue), ViewState("DBConnection"))
                    ChangeCurrency(ddlCurrDt, tbDate, tbRateDt, ViewState("Currency"), ViewState("DigitCurrAcc"), ViewState("DBConnection"))
                    ChangeFgSubLed(tbFgSubledDt, tbSubledDt, btnSubled)
                    AttachScript("kali(" + Me.tbRateDt.ClientID + "," + Me.tbAmountForexDt.ClientID + "," + Me.tbAmountHomeDt.ClientID + "); setformatdt();", Page, Me.GetType())
                ElseIf ViewState("Sender") = "btnSubled" Then
                    tbSubledDt.Text = Session("Result")(0).ToString
                    tbSubledNameDt.Text = Session("Result")(1).ToString
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
            lbPayHome.Text = "Payment (" + ViewState("Currency") + ")"
            'lbChargeHome.Text = "Charge (" + ViewState("Currency") + ")"
            lbTotPay.Text = "Payment (" + ViewState("Currency") + ")"
            lbTotOthers.Text = "Others (" + ViewState("Currency") + ")"
            lbTotExpense.Text = "Expense (" + ViewState("Currency") + ")"
            lbTotCharge.Text = "Charge (" + ViewState("Currency") + ")"
            FillCombo(ddlCurrDt, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
            FillCombo(ddlCurrDt2, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
            'FillCombo(ddlChargeCurrDt2, "EXEC S_GetCurrency", True, "Currency", "Currency", ViewState("DBConnection"))
            FillCombo(ddlCostCenterDt, "EXEC S_GetCostCtr", True, "Cost_Ctr_Code", "Cost_Ctr_Name", ViewState("DBConnection"))
            'FillCombo(ddlPayTypeDt2, "EXEC S_GetPayTypeUser(" + QuotedStr("PaymentNT" + ddlReport.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString) + ")", True, "Payment_Code", "Payment_Name", ViewState("DBConnection"))
            'FillCombo(ddlBankPaymentDt2, "EXEC S_GetBankPayment ('*')", True, "Bank_Code", "Bank_Name", ViewState("DBConnection"))
            ViewState("AccPPn") = SQLExecuteScalar("EXEC S_FNPayNonTradeGetSetAcc ", ViewState("DBConnection"))
            ViewState("AccPPn2") = SQLExecuteScalar("EXEC S_FNPayNonTradeGetSetAcc2 ", ViewState("DBConnection"))

            ViewState("PayType") = ""
            ViewState("SortExpression") = Nothing
            ViewState("DigitCurr") = 0
            ViewState("DigitCurrAcc") = 0
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
            If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
                ddlCommand.Items.Add("Print")
                ddlCommand2.Items.Add("Print")
                'ddlCommand.Items.Add("Print Full")
                'ddlCommand2.Items.Add("Print Full")
            End If
            lbTitle.Text = "Payment Non Trade" 'Request.QueryString("MenuName").ToString
            tbTotalPaymentForex.Attributes.Add("ReadOnly", "True")
            tbTotalPayment.Attributes.Add("ReadOnly", "True")
            tbTotalOthers.Attributes.Add("ReadOnly", "True")
            tbTotalExpense.Attributes.Add("ReadOnly", "True")
            tbTotalCharge.Attributes.Add("ReadOnly", "True")
            tbTotalSelisih.Attributes.Add("ReadOnly", "True")

            tbTotalPaymentForex.Attributes.Add("OnBlur", "setformat();")
            tbTotalPayment.Attributes.Add("OnBlur", "setformat();")
            tbTotalOthers.Attributes.Add("OnBlur", "setformat();")
            tbTotalExpense.Attributes.Add("OnBlur", "setformat();")
            tbTotalCharge.Attributes.Add("OnBlur", "setformat();")
            tbTotalSelisih.Attributes.Add("OnBlur", "setformat();")

            tbAmountHomeDt.Attributes.Add("ReadOnly", "True")
            'tbChargeHomeDt2.Attributes.Add("ReadOnly", "True")
            tbPaymentHomeDt2.Attributes.Add("ReadOnly", "True")

            tbRateDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbRateDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")
            'tbChargeRateDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")

            tbAmountForexDt.Attributes.Add("OnKeyDown", "return PressNumericMinus();")
            tbPaymentForexDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")
            'tbChargeForexDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")

            tbRateDt.Attributes.Add("OnBlur", "kali(" + Me.tbRateDt.ClientID + "," + Me.tbAmountForexDt.ClientID + "," + Me.tbAmountHomeDt.ClientID + "); setformatdt();")
            tbAmountForexDt.Attributes.Add("OnBlur", "kali(" + Me.tbRateDt.ClientID + "," + Me.tbAmountForexDt.ClientID + "," + Me.tbAmountHomeDt.ClientID + "); setformatdt();")

            tbRateDt2.Attributes.Add("OnBlur", "kali(" + Me.tbRateDt2.ClientID + "," + Me.tbPaymentForexDt2.ClientID + "," + Me.tbPaymentHomeDt2.ClientID + "); setformatdt2();")
            tbPaymentForexDt2.Attributes.Add("OnBlur", "kali(" + Me.tbRateDt2.ClientID + "," + Me.tbPaymentForexDt2.ClientID + "," + Me.tbPaymentHomeDt2.ClientID + "); setformatdt2();")

            ' tbChargeRateDt2.Attributes.Add("OnBlur", "kali(" + Me.tbChargeRateDt2.ClientID + "," + Me.tbChargeForexDt2.ClientID + "," + Me.tbChargeHomeDt2.ClientID + "); setformatdt2();")
            'tbChargeForexDt2.Attributes.Add("OnBlur", "kali(" + Me.tbChargeRateDt2.ClientID + "," + Me.tbChargeForexDt2.ClientID + "," + Me.tbChargeHomeDt2.ClientID + "); setformatdt2();")
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
            DV.Sort = ViewState("SortExpression")
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "TransDate DESC"
            End If
            GridView1.DataSource = DV
            GridView1.DataBind()
            If DT.Rows.Count > 0 Then
                'GridView1.HeaderRow.Cells(10).Text = "Payment (" + ViewState("Currency") + ")"
                'GridView1.HeaderRow.Cells(11).Text = "Other (" + ViewState("Currency") + ")"
                'GridView1.HeaderRow.Cells(12).Text = "Expense (" + ViewState("Currency") + ")"
                'GridView1.HeaderRow.Cells(11).Text = "Charge (" + ViewState("Currency") + ")"
            End If
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT * From V_FNPayNonTradeDb WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Private Function GetStringDt2(ByVal Nmbr As String) As String
        Return "SELECT * From V_FNPayNonTradeCr WHERE TransNmbr = " + QuotedStr(Nmbr)
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
                'Session("SelectCommand") = "EXEC S_FNFormBuktiBank " + Result + ",'PAYMENTNONTRADE'"
                'Session("ReportFile") = ".../../../Rpt/FormBuktiBank.frx"
                Session("SelectCommand") = "EXEC S_FNFormVoucher " + Result + ",'PYN'," + QuotedStr(ViewState("UserId"))
                Session("ReportFile") = ".../../../Rpt/FormVoucher.frx"
                Session("DBConnection") = ViewState("DBConnection")
                AttachScript("openprintdlg();", Page, Me.GetType)
                'ElseIf ActionValue = "Print Full" Then
                '    Dim GVR As GridViewRow
                '    Dim CB As CheckBox
                '    Dim Pertamax As Boolean

                '    Pertamax = True
                '    Result = ""

                '    For Each GVR In GridView1.Rows
                '        CB = GVR.FindControl("cbSelect")
                '        If CB.Checked Then
                '            ListSelectNmbr = GVR.Cells(2).Text
                '            If Pertamax Then
                '                Result = "'''" + ListSelectNmbr + "''"
                '                Pertamax = False
                '            Else
                '                Result = Result + ",''" + ListSelectNmbr + "''"
                '            End If
                '        End If
                '    Next
                '    Result = Result + "'"
                '    Session("SelectCommand") = "EXEC S_FNFormBuktiBankDt " + Result + ",'PAYMENTNONTRADE'," + QuotedStr(ViewState("UserId"))
                '    Session("ReportFile") = ".../../../Rpt/FormBuktiBankDt.frx"
                '    Session("DBConnection") = ViewState("DBConnection")
                '    AttachScript("openprintdlg();", Page, Me.GetType)
            Else
                Status = CekStatus(ActionValue)

                ListSelectNmbr = ""
                GetListCommand(Status, GridView1, "3,2", ListSelectNmbr, Nmbr, lbStatus.Text)
                If ListSelectNmbr = "" Then Exit Sub
                For j = 0 To (Nmbr.Length - 1)
                    If Nmbr(j) = "" Then
                        Exit For
                    Else
                        Result = ExecSPCommandGo(ActionValue, "S_FNPayNonTrade", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            'If dt.Rows.Count > 0 Then
            GridDt.HeaderRow.Cells(9).Text = "Amount (" + ViewState("Currency") + ")"
            'End If
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
            GridDt2.HeaderRow.Cells(10).Text = "Payment (" + ViewState("Currency") + ")"
            'GridDt2.HeaderRow.Cells(11).Text = "Charge (" + ViewState("Currency") + ")"
            If dt.Rows.Count > 0 Then
                For Each dr In dt.Rows
                    If dr("FgMode").ToString = "B" Or dr("FgMode").ToString = "K" Or dr("FgMode").ToString = "G" Or dr("FgMode").ToString = "D" Or dr("FgMode").ToString = "O" Then
                        ViewState("PayType") = dt.Rows(0)("PaymentType").ToString
                        Exit For
                    End If
                Next
                'GridDt2.Columns(0).Visible = True
                'lbStatus.Text = "a"
                'Exit Sub
            Else
                'GridDt2.Columns(0).Visible = False
                'lbStatus.Text = "b"
                'Exit Sub
            End If
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
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
            MovePanel(pnlEditDt, PnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnCancelDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelDt2.Click
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
            ddlUserType.SelectedValue = "Supplier"
            tbDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            tbTotalPaymentForex.Text = FormatFloat(0, ViewState("DigitCurr"))
            tbTotalPayment.Text = "0"
            tbTotalOthers.Text = "0"
            tbTotalExpense.Text = "0"
            tbTotalCharge.Text = "0"
            tbTotalSelisih.Text = "0"
            tbAttn.Text = ""
            tbRemark.Text = ""
            FillCombo(ddlPayTypeDt2, "EXEC S_GetPayTypeUser " + QuotedStr("PaymentNT" + "Y") + ", " + QuotedStr(ViewState("UserId").ToString), True, "Payment_Code", "Payment_Name", ViewState("DBConnection")) 'ddlReport.SelectedValue            
            FillCombo(ddlBankPaymentDt2, "EXEC S_GetBankPaymentTrade " + QuotedStr(ddlCurrDt2.SelectedValue), True, "Bank_Code", "Bank_Name", ViewState("DBConnection"))
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try

            tbAccountDt.Text = ""
            tbAccountNameDt.Text = ""
            tbFgSubledDt.Text = "N"
            tbSubledDt.Text = ""
            tbSubledNameDt.Text = ""
            tbSubledDt.Enabled = False
            btnSubled.Visible = False
            ddlCostCenterDt.SelectedIndex = 0
            ddlCurrDt.SelectedValue = ViewState("Currency")
            tbRateDt.Text = FormatFloat(1, ViewState("DigitRate"))
            tbRateDt.Enabled = False
            ddlCurrDt.Enabled = False
            tbAmountForexDt.Text = "0"
            tbAmountHomeDt.Text = "0"
            tbRemarkDt.Text = ""
            tbPPnNo.Text = ""
            tbPPndate.SelectedDate = Nothing
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub

    Private Sub Cleardt2()
        Try
            ddlPayTypeDt2.SelectedIndex = 0
            tbPaymentDateDt2.SelectedDate = tbDate.SelectedDate
            tbDocumentNoDt2.Text = ""
            tbVoucherNo.Enabled = False
            tbVoucherNo.Text = ""
            tbDueDateDt2.SelectedDate = Nothing
            tbGiroDateDt2.SelectedDate = Nothing
            ddlCurrDt2.SelectedValue = ViewState("Currency")
            tbPaymentForexDt2.Text = "0"
            tbPaymentHomeDt2.Text = "0"
            tbRemarkDt2.Text = tbRemark.Text
            FillCombo(ddlBankPaymentDt2, "EXEC S_GetBankPaymentTrade " + QuotedStr(ddlCurrDt2.SelectedValue), True, "Bank_Code", "Bank_Name", ViewState("DBConnection"))
            FillCombo(ddlPayTypeDt2, "EXEC S_GetPayTypeUser " + QuotedStr("PaymentNT" + "Y") + ", " + QuotedStr(ViewState("UserId").ToString), True, "Payment_Code", "Payment_Name", ViewState("DBConnection"))
            ChangeCurrency(ddlCurrDt2, tbDate, tbRateDt2, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
            ChangePaymentType4(ddlPayTypeDt2.SelectedValue, tbFgModeDt2, tbDate, tbDueDateDt2, ddlBankPaymentDt2, ddlCurrDt2, tbRateDt2, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
            ChangePaymentType4(ddlPayTypeDt2.SelectedValue, tbFgModeDt2, tbDate, tbGiroDateDt2, ddlBankPaymentDt2, ddlCurrDt2, tbRateDt2, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
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
            'If CFloat(tbTotalPayment.Text) <= 0 Then
            '    lbStatus.Text = MessageDlg("Payment must have value")
            '    tbTotalPaymentForex.Focus()
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
                If Dr("Account").ToString = "" Then
                    lbStatus.Text = MessageDlg("Account Must Have Value")
                    Return False
                End If
                If ddlCostCenterDt.Enabled = True Then
                    If ddlCostCenterDt.SelectedIndex = 0 Then
                        lbStatus.Text = MessageDlg("Cost Center Must Have Value")
                        Return False
                    End If
                End If
                If Dr("FgSubLed").ToString <> "N" And Dr("SubLed").ToString = "" Then
                    lbStatus.Text = MessageDlg("SubLed Must Have Value")
                    Return False
                End If
                If (Dr("Account").ToString = ViewState("AccPPn") Or Dr("Account").ToString = ViewState("AccPPn2")) And Dr("PPnNo").ToString = "" Then
                    lbStatus.Text = MessageDlg("PPn No Must Have Value")
                    Return False
                End If
                If (Dr("Account").ToString = ViewState("AccPPn") Or Dr("Account").ToString = ViewState("AccPPn2")) And Dr("PPnDate").ToString Is Nothing Then
                    lbStatus.Text = MessageDlg("PPn Date Must Have Value")
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
                If CFloat(Dr("ForexRate").ToString) = 1 And Dr("Currency").ToString <> ViewState("Currency") Then
                    lbStatus.Text = MessageDlg("Currency Rate Must Have Value")
                    Return False
                End If
                If CFloat(Dr("AmountForex").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Amount Expense Must Have Value")
                    Return False
                End If
            Else
                If tbAccountDt.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Account Must Have Value")
                    tbAccountDt.Focus()
                    Return False
                End If
                If ddlCostCenterDt.Enabled = True Then
                    If ddlCostCenterDt.SelectedIndex = 0 Then
                        lbStatus.Text = MessageDlg("Cost Center Must Have Value")
                        Return False
                    End If
                End If
                If tbFgSubledDt.Text <> "N" And tbSubledDt.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("SubLed Must Have Value")
                    btnSubled.Focus()
                    Return False
                End If
                If (tbAccountDt.Text = ViewState("AccPPn") Or tbAccountDt.Text = ViewState("AccPPn2")) And (tbPPnNo.Text = "") Then
                    lbStatus.Text = MessageDlg("PPn No Must Have Value")
                    Return False
                End If
                If (tbAccountDt.Text = ViewState("AccPPn") Or tbAccountDt.Text = ViewState("AccPPn2")) And (tbPPndate.SelectedDate = Nothing) Then
                    lbStatus.Text = MessageDlg("PPn Date Must Have Value")
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
                    lbStatus.Text = MessageDlg("Amount Expense Must Have Value")
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
                If Dr("PaymentType").ToString = "" Then
                    lbStatus.Text = MessageDlg("Payment Type Must Have Value")
                    Return False
                End If
                If Dr("FgMode").ToString = "G" Then
                    If Dr("DocumentNo").ToString = "" Then
                        lbStatus.Text = MessageDlg("Document No Must Have Value")
                        Return False
                    End If
                    If Dr("BankPayment").ToString = "" Then
                        lbStatus.Text = MessageDlg("Bank Payment Must Have Value")
                        Return False
                    End If
                    If Dr("Due Date").ToString = "" Then
                        lbStatus.Text = MessageDlg("Due Date Must Have Value")
                        Return False
                    End If
                End If
                If Dr("FgMode").ToString = "D" Then
                    If Dr("DocumentNo").ToString = "" Then
                        lbStatus.Text = MessageDlg("DP No Must Have Value")
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
                    lbStatus.Text = MessageDlg("Payment Type Must Have Value")
                    ddlPayTypeDt2.Focus()
                    Return False
                End If
                If tbFgModeDt2.Text = "G" Then
                    If tbDocumentNoDt2.Text.Trim = "" Then
                        lbStatus.Text = MessageDlg("Document No Must Have Value")
                        tbDocumentNoDt2.Focus()
                        Return False
                    End If
                    If ddlBankPaymentDt2.SelectedValue = "" Then
                        lbStatus.Text = MessageDlg("Bank Payment Must Have Value")
                        ddlBankPaymentDt2.Focus()
                        Return False
                    End If
                    If tbGiroDateDt2.SelectedDate = Nothing Then
                        lbStatus.Text = MessageDlg("Giro Date Must Have Value")
                        tbGiroDateDt2.Focus()
                        Return False
                    End If
                    If tbDueDateDt2.SelectedDate = Nothing Then
                        lbStatus.Text = MessageDlg("Due Date Must Have Value")
                        tbDueDateDt2.Focus()
                        Return False
                    End If
                End If
                If tbFgModeDt2.Text = "D" Then
                    If tbDocumentNoDt2.Text.Trim = "" Then
                        lbStatus.Text = MessageDlg("DP No Must Have Value")
                        tbDocumentNoDt2.Focus()
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
            ViewState("DigitCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(Dt.Rows(0)("Currency").ToString), ViewState("DBConnection"))
            If ViewState("DigitCurr") = Nothing Then
                ViewState("DigitCurr") = 0
            End If
            BindToText(tbTotalPaymentForex, Dt.Rows(0)("TotalPayForex").ToString, CInt(ViewState("DigitCurr")))
            BindToText(tbTotalPayment, Dt.Rows(0)("TotalPayment").ToString, ViewState("DigitHome"))
            BindToText(tbTotalOthers, Dt.Rows(0)("TotalOthers").ToString, ViewState("DigitHome"))
            BindToText(tbTotalExpense, Dt.Rows(0)("TotalExpense").ToString, ViewState("DigitHome"))
            BindToText(tbTotalCharge, Dt.Rows(0)("TotalCharge").ToString, ViewState("DigitHome"))
            BindToText(tbTotalSelisih, Dt.Rows(0)("TotalSelisih").ToString, ViewState("DigitHome"))
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("ItemNo = " + ItemNo)
            If Dr.Length > 0 Then
                lbItemNo.Text = ItemNo.ToString
                ' FgSubled, Subled, SubledName, CostCtr, Currency, ForexRate, AmountForex, Remark
                BindToText(tbAccountDt, Dr(0)("Account").ToString)
                BindToText(tbAccountNameDt, Dr(0)("AccountName").ToString)
                BindToText(tbFgSubledDt, Dr(0)("FgSubled").ToString)
                BindToText(tbSubledDt, Dr(0)("Subled").ToString)
                BindToText(tbSubledNameDt, Dr(0)("SubledName").ToString)
                BindToText(tbPPnNo, Dr(0)("PPnNo").ToString)
                BindToDate(tbPPndate, Dr(0)("PPNDate").ToString)
                BindToDropList(ddlCostCenterDt, Dr(0)("CostCtr").ToString)
                BindToDropList(ddlCurrDt, Dr(0)("Currency").ToString)
                BindToText(tbRateDt, Dr(0)("ForexRate").ToString)
                BindToText(tbAmountForexDt, Dr(0)("AmountForex").ToString)
                BindToText(tbAmountHomeDt, Dr(0)("AmountHome").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
                BindToText(tbFgType, Dr(0)("FgType").ToString)
                ddlCurrDt.Enabled = (tbFgType.Text = "PL")
                tbRateDt.Enabled = ddlCurrDt.SelectedValue <> ViewState("Currency").ToString
                ViewState("DigitCurrAcc") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlCurrDt.SelectedValue), ViewState("DBConnection"))
                'ViewState("DigitChargeCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlChargeCurr.SelectedValue), ViewState("DBConnection"))
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
                BindToDropList(ddlPayTypeDt2, Dr(0)("PaymentType").ToString)
                tbPaymentDateDt2.SelectedDate = tbDate.SelectedDate
                'tbPaymentDateDt2.SelectedDate = Dr(0)("PaymentDate").ToString
                BindToText(tbDocumentNoDt2, Dr(0)("DocumentNo").ToString)
                BindToText(tbVoucherNo, Dr(0)("Reference").ToString)
                BindToDate(tbGiroDateDt2, Dr(0)("GiroDate").ToString)
                BindToDate(tbDueDateDt2, Dr(0)("DueDate").ToString)
                BindToDropList(ddlCurrDt2, Dr(0)("Currency").ToString)
                BindToText(tbRateDt2, Dr(0)("ForexRate").ToString)
                BindToText(tbPaymentForexDt2, Dr(0)("PaymentForex").ToString)
                BindToText(tbPaymentHomeDt2, Dr(0)("PaymentHome").ToString)
                BindToText(tbRemarkDt2, Dr(0)("Remark").ToString)
                BindToText(tbFgModeDt2, Dr(0)("FgMode").ToString)
                FillCombo(ddlBankPaymentDt2, "EXEC S_GetBankPaymentTrade " + QuotedStr(ddlCurrDt2.SelectedValue), True, "Bank_Code", "Bank_Name", ViewState("DBConnection"))
                BindToDropList(ddlBankPaymentDt2, Dr(0)("BankPayment").ToString)
                'BindToDropList(ddlChargeCurrDt2, Dr(0)("ChargeCurrency").ToString)
                ViewState("DigitCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlCurrDt2.SelectedValue), ViewState("DBConnection"))
                'ViewState("DigitExpenseCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlChargeCurrDt2.SelectedValue), ViewState("DBConnection"))
                'BindToText(tbChargeRateDt2, Dr(0)("ChargeRate").ToString)
                'BindToText(tbChargeForexDt2, Dr(0)("ChargeForex").ToString)
                'BindToText(tbChargeHomeDt2, Dr(0)("ChargeHome").ToString)

                'If ddlChargeCurrDt2.SelectedValue = "" Then
                '    tbChargeForexDt2.Text = "0"
                '    tbChargeHomeDt2.Text = "0"
                'End If
                tbRateDt2.Enabled = ddlCurrDt2.SelectedValue <> Session("Currency")
                ddlBankPaymentDt2.Enabled = tbFgModeDt2.Text = "G"
                tbGiroDateDt2.Enabled = tbFgModeDt2.Text = "G"
                tbDueDateDt2.Enabled = tbFgModeDt2.Text = "G"
                'ddlChargeCurrDt2.Enabled = tbFgModeDt2.Text = "B"
                'tbChargeForexDt2.Enabled = tbFgModeDt2.Text = "B"
                'tbChargeRateDt2.Enabled = ddlChargeCurrDt2.SelectedValue <> Session("Currency") And tbFgModeDt2.Text = "B" And ddlChargeCurrDt2.SelectedValue <> ""
                tbVoucherNo.Enabled = (tbFgModeDt2.Text = "B" Or tbFgModeDt2.Text = "K")
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail 2 error : " + ex.ToString)
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
                Row = ViewState("Dt").Select("ItemNo = " + lbItemNo.Text)(0)
                Row.BeginEdit()
                Row("Account") = tbAccountDt.Text
                Row("AccountName") = tbAccountNameDt.Text
                Row("FgSubled") = tbFgSubledDt.Text
                Row("Subled") = tbSubledDt.Text
                Row("SubledName") = tbSubledNameDt.Text
                Row("FgType") = tbFgType.Text
                Row("PPnNo") = tbPPnNo.Text
                If tbPPndate.Enabled Then
                    Row("PPnDate") = tbPPndate.SelectedDate.ToString
                Else
                    Row("PPnDate") = DBNull.Value
                End If
                Row("Currency") = ddlCurrDt.SelectedValue
                Row("ForexRate") = tbRateDt.Text
                Row("CostCtr") = ddlCostCenterDt.SelectedValue
                If Row("CostCtr") = "" Then
                    Row("Costctr") = DBNull.Value
                End If
                Row("AmountForex") = tbAmountForexDt.Text
                Row("AmountHome") = tbAmountHomeDt.Text
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("ItemNo") = CInt(lbItemNo.Text)
                dr("Account") = tbAccountDt.Text
                dr("AccountName") = tbAccountNameDt.Text
                dr("FgSubled") = tbFgSubledDt.Text
                dr("Subled") = tbSubledDt.Text
                dr("SubledName") = tbSubledNameDt.Text
                dr("FgType") = tbFgType.Text
                dr("PPnNo") = tbPPnNo.Text
                dr("PPnDate") = tbPPndate.SelectedDate
                If tbPPndate.Enabled Then
                    dr("PPnDate") = tbPPndate.SelectedDate.ToString
                Else
                    dr("PPnDate") = DBNull.Value
                End If
                dr("Currency") = ddlCurrDt.SelectedValue
                dr("ForexRate") = tbRateDt.Text
                dr("CostCtr") = ddlCostCenterDt.SelectedValue
                If dr("CostCtr") = "" Then
                    dr("Costctr") = DBNull.Value
                End If
                dr("AmountForex") = tbAmountForexDt.Text
                dr("AmountHome") = tbAmountHomeDt.Text
                dr("Remark") = tbRemarkDt.Text
                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, PnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
            BindGridDt(ViewState("Dt"), GridDt)
            StatusButtonSave(True)
            ' GridDt.HeaderRow.Cells(9).Text = "Amount (" + ViewState("Currency") + ")"
        Catch ex As Exception
            lbStatus.Text = "btn save Dt Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub

    Protected Sub btnSaveDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveDt2.Click
        Try

            If ViewState("1Payment").ToString = "Y" Then
                If GetCountRecord(ViewState("Dt2")) >= 1 Then
                    If tbFgModeDt2.Text = "K" Or tbFgModeDt2.Text = "B" Or tbFgModeDt2.Text = "G" Or tbFgModeDt2.Text = "D" Then
                        If ViewState("StateDt2") <> "Edit" Then
                            If ddlPayTypeDt2.SelectedValue <> ViewState("PayType").ToString Then
                                lbStatus.Text = "Cannot input more than one payment type"
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
                If CekExistGiroOut(tbDocumentNoDt2.Text.Trim, ViewState("DBConnection").ToString) = True Then
                    lbStatus.Text = "Giro Payment '" + tbDocumentNoDt2.Text.Trim + "' has already exists in Giro Listing'"
                    Exit Sub
                End If
            End If
            If ViewState("StateDt2") = "Edit" Then
                Dim Row As DataRow
                Row = ViewState("Dt2").Select("ItemNo = " + lbItemNoDt2.Text)(0)
                Row.BeginEdit()
                Row("PaymentType") = ddlPayTypeDt2.SelectedValue
                Row("PaymentName") = ddlPayTypeDt2.SelectedItem.Text
                Row("PaymentDate") = Format(tbPaymentDateDt2.SelectedDate, "dd MMMM yyyy")
                Row("DocumentNo") = tbDocumentNoDt2.Text
                Row("Reference") = tbVoucherNo.Text
                If tbGiroDateDt2.Enabled Then
                    Row("GiroDate") = Format(tbGiroDateDt2.SelectedDate, "dd MMMM yyyy")
                Else
                    Row("GiroDate") = DBNull.Value
                End If

                If tbDueDateDt2.Enabled Then
                    Row("DueDate") = Format(tbDueDateDt2.SelectedDate, "dd MMMM yyyy")
                Else
                    Row("DueDate") = DBNull.Value
                End If
                Row("BankPayment") = ddlBankPaymentDt2.SelectedValue
                Row("BankPaymentName") = ddlBankPaymentDt2.SelectedItem.Text
                Row("FgMode") = tbFgModeDt2.Text
                Row("Currency") = ddlCurrDt2.SelectedValue
                Row("ForexRate") = tbRateDt2.Text
                Row("PaymentForex") = tbPaymentForexDt2.Text
                Row("PaymentHome") = tbPaymentHomeDt2.Text
                Row("Remark") = tbRemarkDt2.Text
                'Row("ChargeCurrency") = ddlChargeCurrDt2.SelectedValue
                'Row("ChargeRate") = tbChargeRateDt2.Text
                'Row("ChargeForex") = tbChargeForexDt2.Text
                'Row("ChargeHome") = tbChargeHomeDt2.Text
                Row("Remark") = tbRemarkDt2.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow

                dr = ViewState("Dt2").NewRow
                dr("ItemNo") = CInt(lbItemNoDt2.Text)
                dr("PaymentType") = ddlPayTypeDt2.SelectedValue
                dr("PaymentName") = ddlPayTypeDt2.SelectedItem.Text
                dr("PaymentDate") = Format(tbPaymentDateDt2.SelectedDate, "dd MMMM yyyy")
                dr("DocumentNo") = tbDocumentNoDt2.Text
                dr("Reference") = tbVoucherNo.Text
                If tbGiroDateDt2.Enabled Then
                    dr("GiroDate") = Format(tbGiroDateDt2.SelectedDate, "dd MMMM yyyy")
                Else
                    dr("GiroDate") = DBNull.Value
                End If
                If tbDueDateDt2.Enabled Then
                    dr("DueDate") = Format(tbDueDateDt2.SelectedDate, "dd MMMM yyyy")
                Else
                    dr("DueDate") = DBNull.Value
                End If
                dr("BankPayment") = ddlBankPaymentDt2.SelectedValue
                dr("BankPaymentName") = ddlBankPaymentDt2.SelectedItem.Text
                dr("FgMode") = tbFgModeDt2.Text
                dr("Currency") = ddlCurrDt2.SelectedValue
                dr("ForexRate") = tbRateDt2.Text
                dr("PaymentForex") = tbPaymentForexDt2.Text
                dr("PaymentHome") = tbPaymentHomeDt2.Text
                dr("Remark") = tbRemarkDt2.Text
                'dr("ChargeCurrency") = ddlChargeCurrDt2.Text
                'dr("ChargeRate") = tbChargeRateDt2.Text
                'dr("ChargeForex") = tbChargeForexDt2.Text
                'dr("ChargeHome") = tbChargeHomeDt2.Text
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
            If CFloat(tbTotalPayment.Text) <= 0 Then
                lbStatus.Text = MessageDlg("Payment must have value")
                tbTotalPaymentForex.Focus()
                Exit Sub
            End If

            'TransNmbr, TransDate, STATUS, FgReport, UserType, UserCode, UserName, Attn, Remark
            'DB : ItemNo, Account, AccountName, FgSubled, Subled, SubledName, CostCtr, Currency, ForexRate, AmountForex, Remark
            'CR : ItemNo, PaymentType, PaymentName, PaymentDate, DocumentNo, Currency, ForexRate, PaymentForex, PaymentHome, Remark, DueDate, BankPayment, ChargeCurrency, ChargeRate, ChargeForex, ChargeHome
            If Not (ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit") Then
                Exit Sub
            Else
                ViewState("StateSave") = ViewState("StateHd")
                ViewState("StateHd") = "View"
            End If
            'Save Hd
            If ViewState("StateSave") = "Insert" Then
                'ddlReport.SelectedValue
                tbCode.Text = GetAutoNmbr("PYN", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), ViewState("PayType").ToString, ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO FINPayNonTradeHd (TransNmbr, TransDate, STATUS, FgReport, " + _
                "UserType, UserCode, Attn, TotalPayForex, TotalPayment, TotalOthers, TotalExpense, TotalCharge, TotalSelisih, Remark, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", 'H', " + _
                QuotedStr("Y") + ", " + QuotedStr(ddlUserType.SelectedValue) + ", " + _
                QuotedStr(tbUserCode.Text) + ", " + QuotedStr(tbAttn.Text) + ", " + _
                tbTotalPaymentForex.Text.Replace(",", "") + ", " + tbTotalPayment.Text.Replace(",", "") + ", " + tbTotalOthers.Text.Replace(",", "") + ", " + _
                tbTotalExpense.Text.Replace(",", "") + ", " + tbTotalCharge.Text.Replace(",", "") + _
                ", " + tbTotalSelisih.Text.Replace(",", "") + ", " + QuotedStr(tbRemark.Text) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", GetDate()"

                ViewState("TransNmbr") = tbCode.Text

            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM FINPayNonTradeHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE FINPayNonTradeHd SET TransDate = " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", UserType=" + QuotedStr(ddlUserType.SelectedValue) + ", UserCode=" + QuotedStr(tbUserCode.Text) + _
                ", Attn =" + QuotedStr(tbAttn.Text) + ", TotalPayment = " + tbTotalPayment.Text.Replace(",", "") + _
                ", TotalPayForex = " + tbTotalPaymentForex.Text.Replace(",", "") + _
                ", TotalOthers = " + tbTotalOthers.Text.Replace(",", "") + _
                ", TotalExpense = " + tbTotalExpense.Text.Replace(",", "") + ", TotalCharge = " + tbTotalCharge.Text.Replace(",", "") + ", Remark = " + QuotedStr(tbRemark.Text) + _
                ", TotalSelisih= " + tbTotalSelisih.Text.Replace(",", "") + ", DatePrep = GetDate()" + _
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
                If Row(I)("FgMode") = "E" Then
                    Row(I)("FgValue") = -1
                Else
                    Row(I)("FgValue") = 1
                End If
                Row(I).EndEdit()
            Next

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, ItemNo, Account, FgSubled, Subled, CostCtr, PPnNo, PPnDate, Currency, ForexRate, AmountForex, AmountHome, Remark FROM FINPayNonTradeDB WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand

            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
                    "UPDATE FINPayNonTradeDb SET Account = @Account, FgSubled = @FgSubled, Subled = @Subled, CostCtr = @CostCtr, PPnNo = @PPnNo, PPnDate = @PPnDate, Currency = @Currency, ForexRate = @ForexRate, AmountForex = @AmountForex, AmountHome = @AmountHome, Remark = @Remark WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND ItemNo = @OldItemNo", con)
            ' Define output parameters.
            Update_Command.Parameters.Add("@Account", SqlDbType.VarChar, 20, "Account")
            Update_Command.Parameters.Add("@FgSubled", SqlDbType.VarChar, 1, "FgSubled")
            Update_Command.Parameters.Add("@Subled", SqlDbType.VarChar, 20, "Subled")
            Update_Command.Parameters.Add("@CostCtr", SqlDbType.VarChar, 5, "CostCtr")
            Update_Command.Parameters.Add("@PPnNo", SqlDbType.VarChar, 30, "PPnNo")
            Update_Command.Parameters.Add("@PPnDate", SqlDbType.DateTime, 8, "PPnDate")
            Update_Command.Parameters.Add("@Currency", SqlDbType.VarChar, 5, "Currency")
            Update_Command.Parameters.Add("@ForexRate", SqlDbType.Decimal, 18, "ForexRate")
            Update_Command.Parameters.Add("@AmountForex", SqlDbType.Decimal, 18, "AmountForex")
            Update_Command.Parameters.Add("@AmountHome", SqlDbType.Decimal, 18, "AmountHome")
            Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 255, "Remark")
            ' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@OldItemNo", SqlDbType.Int, 10, "ItemNo")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM FINPayNonTradeDb WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND ItemNo = @ItemNo", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@ItemNo", SqlDbType.Int, 10, "ItemNo")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("FINPayNonTradeDb")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt

            'save dt2
            cmdSql = New SqlCommand("SELECT TransNmbr, ItemNo, PaymentType, PaymentDate, DocumentNo, Reference, GiroDate, DueDate, BankPayment, FgMode, Currency, ForexRate, PaymentForex, PaymentHome, Remark, FgValue FROM FINPayNonTradeCr WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            dbcommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand

            Dim param2 As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command2 = New SqlCommand( _
                    "UPDATE FINPayNonTradeCr SET PaymentType = @PaymentType, PaymentDate = @PaymentDate, DocumentNo = @DocumentNo, " + _
                    " Reference = @Reference, GiroDate = @GiroDate, DueDate = @DueDate, BankPayment = @BankPayment, " + _
                    " FgMode = @FgMode, FgValue = @FgValue, Currency = @Currency, ForexRate = @ForexRate, PaymentForex = @PaymentForex, " + _
                    " PaymentHome = @PaymentHome, Remark = @Remark WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND ItemNo = @OldItemNo", con)
            ' Define output parameters.
            Update_Command2.Parameters.Add("@PaymentType", SqlDbType.VarChar, 5, "PaymentType")
            Update_Command2.Parameters.Add("@PaymentDate", SqlDbType.DateTime, 12, "PaymentDate")
            Update_Command2.Parameters.Add("@DocumentNo", SqlDbType.VarChar, 60, "DocumentNo")
            Update_Command2.Parameters.Add("@Reference", SqlDbType.VarChar, 20, "Reference")
            Update_Command2.Parameters.Add("@GiroDate", SqlDbType.DateTime, 12, "GiroDate")
            Update_Command2.Parameters.Add("@DueDate", SqlDbType.DateTime, 12, "DueDate")
            Update_Command2.Parameters.Add("@BankPayment", SqlDbType.VarChar, 5, "BankPayment")
            Update_Command2.Parameters.Add("@FgMode", SqlDbType.VarChar, 1, "FgMode")
            Update_Command2.Parameters.Add("@FgValue", SqlDbType.Int, 4, "FgValue")
            Update_Command2.Parameters.Add("@Currency", SqlDbType.VarChar, 5, "Currency")
            Update_Command2.Parameters.Add("@ForexRate", SqlDbType.Decimal, 18, "ForexRate")
            Update_Command2.Parameters.Add("@PaymentForex", SqlDbType.Decimal, 18, "PaymentForex")
            Update_Command2.Parameters.Add("@PaymentHome", SqlDbType.Decimal, 18, "PaymentHome")
            Update_Command2.Parameters.Add("@Remark", SqlDbType.VarChar, 255, "Remark")
            'Update_Command2.Parameters.Add("@ChargeCurrency", SqlDbType.VarChar, 255, "ChargeCurrency")
            'Update_Command2.Parameters.Add("@ChargeRate", SqlDbType.Decimal, 18, "ChargeRate")
            'Update_Command2.Parameters.Add("@ChargeForex", SqlDbType.Decimal, 18, "ChargeForex")
            'Update_Command2.Parameters.Add("@ChargeHome", SqlDbType.Decimal, 18, "ChargeHome")
            ' Define intput (WHERE) parameters.
            param2 = Update_Command2.Parameters.Add("@OldItemNo", SqlDbType.Int, 10, "ItemNo")
            param2.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command2

            ' Create the DeleteCommand.
            Dim Delete_Command2 = New SqlCommand( _
                "DELETE FROM FINPayNonTradeCr WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND ItemNo = @ItemNo", con)
            ' Add the parameters for the DeleteCommand.
            param2 = Delete_Command2.Parameters.Add("@ItemNo", SqlDbType.Int, 10, "ItemNo")
            param2.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command2

            Dim Dt2 As New DataTable("FINPayNonTradeCr")

            Dt2 = ViewState("Dt2")
            da.Update(Dt2)
            Dt2.AcceptChanges()
            ViewState("Dt2") = Dt2
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
            If CFloat(tbTotalSelisih.Text) <> 0 Then
                lbStatus.Text = MessageDlg("Debet - Credit must be balance")
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail Account must have at least 1 record")
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt2")) = 0 Then
                'If ViewState("PayType").ToString = "" Then
                lbStatus.Text = MessageDlg("Detail Payment must have at least 1 record")
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

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnAdd.Click, btnAdd2.Click
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

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdddt.Click, btnAddDtke2.Click
        Try
            Cleardt()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt") = "Insert"
            MovePanel(PnlDt, pnlEditDt)
            EnableHd(False)
            StatusButtonSave(False)
            lbItemNo.Text = GetNewItemNo(ViewState("Dt"))
            tbAccountDt.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnAddDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddDt2.Click, btnAddDt2Ke2.Click
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
            'If Session("PeriodInfo")("1Payment").ToString = "Y" Then
            '    BindToDropList(ddlPayTypeDt2, ViewState("PayType").ToString.Trim)
            'End If
            btnDocNo.Visible = False
            tbDocumentNoDt2.Enabled = True
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
            ViewState("DigitCurrAcc") = 0
            ViewState("DigitExpenseCurr") = 0
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
            FDateName = "Payment Date"
            FDateValue = "TransDate"
            FilterName = "Payment No, Payment Date, User Type, User Payment, Attn, DP No, Voucher No, Remark, Account, Account Name"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), UserType, UserPayment, Attn, DPNo, Voucher_No, Remark, Account, Accountname"
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
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
                        FillCombo(ddlPayTypeDt2, "EXEC S_GetPayTypeUser " + QuotedStr("PaymentNT" + "Y") + ", " + QuotedStr(ViewState("UserId").ToString), True, "Payment_Code", "Payment_Name", ViewState("DBConnection")) 'ddlReport.SelectedValue

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
                        Session("SelectCommand") = "EXEC S_FNFormVoucher '''" + GVR.Cells(2).Text + "''','PYN'," + QuotedStr(ViewState("UserId"))
                        Session("ReportFile") = ".../../../Rpt/FormVoucher.frx"
                        Session("DBConnection") = ViewState("DBConnection")
                        AttachScript("openprintdlg();", Page, Me.GetType)
                    Catch ex As Exception
                        lbStatus.Text = "btn print Error = " + ex.ToString
                    End Try
                    'ElseIf DDL.SelectedValue = "Print Full" Then
                    '    Try
                    '        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                    '        If CekMenu <> "" Then
                    '            lbStatus.Text = CekMenu
                    '            Exit Sub
                    '        End If
                    '        Session("SelectCommand") = "EXEC S_FNFormBuktiBankDt '''" + GVR.Cells(2).Text + "''','PAYMENTNONTRADE'," + QuotedStr(ViewState("UserId"))
                    '        Session("ReportFile") = ".../../../Rpt/FormBuktiBankDt.frx"
                    '        Session("DBConnection") = ViewState("DBConnection")
                    '        AttachScript("openprintdlg();", Page, Me.GetType)
                    '    Catch ex As Exception
                    '        lbStatus.Text = "btn print Error = " + ex.ToString
                    '    End Try
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

    Dim TotalExpense As Decimal = 0
    ' untuk tampilkan data total di grid
    Protected Sub GridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt.RowDataBound
        Try
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "ItemNo")) Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                    ' add the UnitPrice and QuantityTotal to the running total variables
                    TotalExpense += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "AmountHome"))
                ElseIf e.Row.RowType = DataControlRowType.Footer Then
                    tbTotalExpense.Text = FormatNumber(TotalExpense, ViewState("DigitHome"))
                End If
            End If

            tbTotalSelisih.Text = FormatNumber(CFloat(tbTotalPayment.Text) + CFloat(tbTotalOthers.Text) - CFloat(tbTotalExpense.Text) - CFloat(tbTotalCharge.Text), ViewState("DigitHome"))

        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
            'dr = ViewState("Dt2").Select("ItemNo = " + GVR.Cells(1).Text)
            'dr(0).Delete()
            'BindGridDt(ViewState("Dt"), GridDt)
            'EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
            dr = ViewState("Dt").Select("ItemNo = " + QuotedStr(GVR.Cells(1).Text))
            dr(0).Delete()
            BindGridDt(ViewState("Dt"), GridDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        End Try
    End Sub


    Dim TotalPaymentForex As Decimal = 0
    Dim TotalPayment As Decimal = 0
    Dim TotalCharge As Decimal = 0
    Dim TotalOther As Decimal = 0
    Protected Sub GridDt2_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt2.RowDataBound
        Try
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "ItemNo")) Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                    ' add the UnitPrice and QuantityTotal to the running total variables
                    If DataBinder.Eval(e.Row.DataItem, "FgMode") = "E" Then
                        TotalCharge += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "PaymentHome"))
                    ElseIf DataBinder.Eval(e.Row.DataItem, "FgMode") = "O" Then
                        TotalOther += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "PaymentHome"))
                    Else
                        TotalPayment += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "PaymentHome"))
                        TotalPaymentForex += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "PaymentForex"))
                    End If
                ElseIf e.Row.RowType = DataControlRowType.Footer Then
                    If GetCountRecord(ViewState("Dt2")) > 0 Then
                        tbTotalPayment.Text = FormatNumber(TotalPayment, ViewState("DigitHome"))
                        tbTotalPaymentForex.Text = FormatNumber(TotalPaymentForex, ViewState("DigitCurr"))
                        tbTotalOthers.Text = FormatNumber(TotalOther, ViewState("DigitHome"))
                        tbTotalCharge.Text = FormatNumber(TotalCharge, ViewState("DigitHome"))
                    Else
                        tbTotalPayment.Text = FormatNumber(0, ViewState("DigitHome"))
                        tbTotalPaymentForex.Text = FormatNumber(0, ViewState("DigitCurr"))
                        tbTotalOthers.Text = FormatNumber(0, ViewState("DigitHome"))
                        tbTotalCharge.Text = FormatNumber(0, ViewState("DigitHome"))
                    End If
                End If
                'AttachScript("setformat();", Page, Me.GetType())
                tbTotalSelisih.Text = FormatNumber(CFloat(tbTotalPayment.Text) + CFloat(tbTotalOthers.Text) - CFloat(tbTotalExpense.Text) - CFloat(tbTotalCharge.Text), ViewState("DigitHome"))
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt2 Row Data Bound Error : " + ex.ToString
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
            'row = ViewState("Dt").Rows(e.NewEditIndex)
            FillTextBoxDt(GVR.Cells(1).Text)
            MovePanel(PnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            btnSaveDt.Focus()
            StatusButtonSave(False)
            tbSubledDt.Enabled = tbFgSubledDt.Text <> "N"
            btnSubled.Enabled = tbSubledDt.Enabled
            ddlCostCenterDt.Enabled = GVR.Cells(6).Text <> "N"
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
            'ChangePaymentType(ddlPayTypeDt2.SelectedValue, tbFgModeDt2, tbPaymentDateDt2, tbDueDateDt2, ddlBankPaymentDt2, ddlCurrDt2, ddlChargeCurrDt2, tbRateDt2, tbChargeRateDt2, tbChargeForexDt2, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"), "Edit")
            'ChangePaymentType(ddlPayTypeDt2.SelectedValue, tbFgModeDt2, tbPaymentDateDt2, tbGiroDateDt2, ddlBankPaymentDt2, ddlCurrDt2, ddlChargeCurrDt2, tbRateDt2, tbChargeRateDt2, tbChargeForexDt2, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"), "Edit")
            ViewState("StateDt2") = "Edit"
            StatusButtonSave(False)
            btnDocNo.Visible = tbFgModeDt2.Text = "D"
            tbDocumentNoDt2.Enabled = Not tbFgModeDt2.Text = "D"
            btnSaveDt2.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid dt2 Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlPayTypeDt2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPayTypeDt2.SelectedIndexChanged
        Try            
            Dim VoucherNo As String

            ViewState("DigitCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlCurrDt2.SelectedValue), ViewState("DBConnection"))
            'Public Sub ChangePaymentType4(ByVal Payment As String, ByRef TxFgMode As TextBox, ByVal txdate As BasicDatePicker, ByRef txduedate As BasicDatePicker, ByRef ddlbank As DropDownList, ByRef ddlCurr As DropDownList, ByRef txRate As TextBox, ByVal HomeCurrency As String, ByVal DigitCurr As Integer, Optional ByVal DBConnection As String = "Nothing", Optional ByVal State As String = "Add")
            ChangePaymentType4(ddlPayTypeDt2.SelectedValue, tbFgModeDt2, tbDate, tbDueDateDt2, ddlBankPaymentDt2, ddlCurrDt2, tbRateDt2, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
            ChangePaymentType4(ddlPayTypeDt2.SelectedValue, tbFgModeDt2, tbDate, tbGiroDateDt2, ddlBankPaymentDt2, ddlCurrDt2, tbRateDt2, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
            FillCombo(ddlBankPaymentDt2, "EXEC S_GetBankPaymentTrade " + QuotedStr(ddlCurrDt2.SelectedValue), True, "Bank_Code", "Bank_Name", ViewState("DBConnection")) 'ddlReport.SelectedValue
            tbPaymentHomeDt2.Text = FormatFloat(CFloat(tbPaymentForexDt2.Text) * CFloat(tbRateDt2.Text), ViewState("DigitCurr"))
            'tbChargeHomeDt2.Text = FormatFloat(CFloat(tbChargeRateDt2.Text) * CFloat(tbChargeForexDt2.Text), ViewState("DigitExpenseCurr"))
            'AttachScript("kali(" + Me.tbRateDt2.ClientID + "," + Me.tbPaymentForexDt2.ClientID + "," + Me.tbPaymentHomeDt2.ClientID + "); kali(" + Me.tbChargeRateDt2.ClientID + "," + Me.tbChargeForexDt2.ClientID + "," + Me.tbChargeHomeDt2.ClientID + "); setformatdt2();", Page, Me.GetType())
            btnDocNo.Visible = tbFgModeDt2.Text = "D"
            tbDocumentNoDt2.Enabled = Not tbFgModeDt2.Text = "D"
            tbDocumentNoDt2.Text = ""

            VoucherNo = ""
            If tbFgModeDt2.Text = "B" Or tbFgModeDt2.Text = "K" Then
                VoucherNo = SQLExecuteScalar("Declare @A VarChar(255) EXEC S_SAAutoVoucherNmbr " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", 'Y', " + QuotedStr(ddlPayTypeDt2.SelectedValue) + ", 'OUT', @A OUT SELECT @A", ViewState("DBConnection").ToString) 'ddlReport.SelectedValue
            End If
            tbVoucherNo.Enabled = (tbFgModeDt2.Text = "B" Or tbFgModeDt2.Text = "K")
            tbVoucherNo.Text = VoucherNo
            btnSaveDt.Focus()
        Catch ex As Exception
            lbStatus.Text = "ddl Pay Type Select Index Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccount_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAccount.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT * FROM V_MsAccountDt WHERE TransType = 'PYN' "
            ResultField = "Account, Description, FgSubled, Currency, FgType,FgCostCtr"
            ViewState("Sender") = "btnAccount"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "Btn Account Click Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub tbAccountDt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccountDt.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Account", tbAccountDt.Text + "|PYN", ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbAccountDt.Text = Dr("Account")
                tbAccountNameDt.Text = Dr("AccountName")
                BindToDropList(ddlCurrDt, Dr("CurrCode"))
                tbFgType.Text = Dr("FgType").ToString.ToUpper
                ddlCurrDt.Enabled = tbFgType.Text = "PL"
                tbFgSubledDt.Text = Dr("FgSubled")

                ViewState("FgType") = tbFgType.Text
                If ViewState("FgType") = "BS" Then
                    ddlCurrDt.Enabled = False
                Else : ddlCurrDt.Enabled = True
                End If

                tbSubledDt_TextChanged(Nothing, Nothing)
                tbFgCostCtr.Text = TrimStr(Dr("FgCostCtr").ToString)
                tbSubledDt.Enabled = tbFgSubledDt.Text <> "N"

                ViewState("DigitCurrAcc") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlCurrDt.SelectedValue), ViewState("DBConnection"))
                ChangeCurrency(ddlCurrDt, tbDate, tbRateDt, ViewState("Currency"), ViewState("DigitCurrAcc"), ViewState("DBConnection"))
                AttachScript("kali(" + Me.tbRateDt.ClientID + "," + Me.tbAmountForexDt.ClientID + "," + Me.tbAmountHomeDt.ClientID + "); setformatdt();", Page, Me.GetType())
            Else
                tbAccountDt.Text = ""
                tbAccountNameDt.Text = ""
                tbFgSubledDt.Text = ""
                tbSubledDt.Text = ""
                tbSubledNameDt.Text = ""
                tbSubledDt.Enabled = False
            End If

            btnSubled.Visible = tbSubledDt.Enabled()
            ddlCostCenterDt.Enabled = tbFgCostCtr.Text <> "N"
            If tbFgCostCtr.Text = "N" Then
                ddlCostCenterDt.SelectedIndex = 0
            End If
            tbPPnNo.Enabled = (tbAccountDt.Text = ViewState("AccPPn") Or tbAccountDt.Text = ViewState("AccPPn2"))
            tbPPndate.Enabled = tbPPnNo.Enabled
            'ChangeFgSubLed(tbFgSubledDt, tbSubledDt, btnSubled)
            'tbAccountDt.Focus()
        Catch ex As Exception
            Throw New Exception("tb Product change Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSubled_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSubled.Click
        Dim ResultField As String
        Try
            If tbFgSubledDt.Text = "N" Then
                Exit Sub
            End If
            Session("filter") = "SELECT Subled_No, Subled_Name FROM VMsSubled WHERE FgSubled = " + QuotedStr(tbFgSubledDt.Text)
            ResultField = "Subled_No, Subled_Name"
            ViewState("Sender") = "btnSubled"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Subled click Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub tbSubledDt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbSubledDt.TextChanged
        Dim Dr As DataRow
        Try
            If tbFgSubledDt.Text = "N" Then
                tbSubledDt.Text = ""
                tbSubledNameDt.Text = ""
                Exit Sub
            End If

            Dr = FindMaster("Subled", tbFgSubledDt.Text + "|" + tbSubledDt.Text, ViewState("DBConnection"))
            If Not Dr Is Nothing Then
                tbSubledDt.Text = Dr("Subled_No")
                tbSubledNameDt.Text = Dr("Subled_Name")
            Else
                tbSubledDt.Text = ""
                tbSubledNameDt.Text = ""
            End If
        Catch ex As Exception
            lbStatus.Text = "tb Subled Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnUser_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUser.Click
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

    'Protected Sub ddlChargeCurrDt2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlChargeCurrDt2.SelectedIndexChanged
    '    Try
    '        ChangeCurrency(ddlChargeCurrDt2, tbPaymentDateDt2, tbChargeRateDt2, ViewState("Currency"), ViewState("DigitExpenseCurr"), ViewState("DBConnection"))
    '        If ddlChargeCurrDt2.SelectedValue = "" Then
    '            tbChargeForexDt2.Text = "0"
    '            tbChargeHomeDt2.Text = "0"
    '        End If
    '        ViewState("DigitExpenseCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlChargeCurrDt2.SelectedValue), ViewState("DBConnection"))
    '        tbChargeForexDt2.Enabled = ddlChargeCurrDt2.SelectedValue <> ""
    '        tbChargeRateDt2.Focus()
    '    Catch ex As Exception
    '        lbStatus.Text = "ddl curr expense selected index changed Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub ddlUserType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlUserType.SelectedIndexChanged
        tbUserCode.Text = ""
        tbUserName.Text = ""
    End Sub

    'Protected Sub ddlReport_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlReport.SelectedIndexChanged
    '    FillCombo(ddlPayTypeDt2, "EXEC S_GetPayTypeUser " + QuotedStr("PaymentNT" + ddlReport.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString), True, "Payment_Code", "Payment_Name", ViewState("DBConnection"))
    '    FillCombo(ddlBankPaymentDt2, "EXEC S_GetBankPayment " + QuotedStr(ddlReport.SelectedValue), True, "Bank_Code", "Bank_Name", ViewState("DBConnection"))
    'End Sub

    Protected Sub btnSaveAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveAll.Click
        Dim CurrFilter, Value As String
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            If CFloat(tbTotalSelisih.Text) <> 0 Then
                lbStatus.Text = MessageDlg("Debet - Credit must be balance")
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail Account must have at least 1 record")
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt2")) = 0 Then
                'If ViewState("PayType").ToString = "" Then
                lbStatus.Text = MessageDlg("Detail Payment must have at least 1 record")
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

    Protected Sub btnDocNo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDocNo.Click
        Dim ResultField As String
        Try
            If tbUserCode.Text.Trim = "" Then
                Session("filter") = "SELECT DP_No, DP_Date, User_Type, User_Code, User_Name, PO_No, PPN_Rate, Currency, Rate, Base_Forex, PPN_Forex, Total_Forex, Base_Paid, PPN_Paid, Total_Paid FROM V_FNDPSuppPending " + _
                "WHERE PPn = 0 and Report =" + QuotedStr("Y") 'ddlReport.SelectedValue
            Else
                Session("filter") = "SELECT DP_No, DP_Date, User_Type, User_Code, User_Name, PO_No, PPN_Rate, Currency, Rate, Base_Forex, PPN_Forex, Total_Forex, Base_Paid, PPN_Paid, Total_Paid FROM V_FNDPSuppPending " + _
                "WHERE PPn = 0 and User_Type = " + QuotedStr(ddlUserType.SelectedValue) + " AND User_Code =" + QuotedStr(tbUserCode.Text) + " AND Report =" + QuotedStr("Y") 'ddlReport.SelectedValue
            End If
            ResultField = "DP_No, Currency, Rate, Total_Forex, Total_Paid, User_Type, User_Code, User_Name"
            ViewState("Sender") = "btnDocNo"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Doc No Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlCurrDt_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCurrDt.SelectedIndexChanged
        Try
            ViewState("DigitCurrAcc") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlCurrDt.SelectedValue), ViewState("DBConnection"))
            ChangeCurrency(ddlCurrDt, tbDate, tbRateDt, ViewState("Currency"), ViewState("DigitCurrAcc"), ViewState("DBConnection"))
            AttachScript("kali(" + Me.tbRateDt.ClientID + "," + Me.tbAmountForexDt.ClientID + "," + Me.tbAmountHomeDt.ClientID + "); setformatdt();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "ddl Currency Error : " + ex.ToString
        End Try
    End Sub
    Public Sub ChangePaymentType4(ByVal Payment As String, ByRef TxFgMode As TextBox, ByVal txdate As BasicDatePicker, ByRef txduedate As BasicDatePicker, ByRef ddlbank As DropDownList, ByRef ddlCurr As DropDownList, ByRef txRate As TextBox, ByVal HomeCurrency As String, ByVal DigitCurr As Integer, Optional ByVal DBConnection As String = "Nothing", Optional ByVal State As String = "Add")
        Try
            If State = "Add" Then
                Dim dr As DataRow
                TxFgMode.Text = "O"
                If Not Payment.Trim = "" Then
                    dr = FindMaster("PayType", Payment, DBConnection)
                    If Not dr Is Nothing Then
                        BindToText(TxFgMode, dr("FgMode").ToString)
                        BindToDropList(ddlCurr, dr("Currency").ToString)
                    End If
                End If
                If Not TxFgMode.Text = "G" Then
                    txduedate.SelectedDate = Nothing
                    txduedate.DisplayType = DatePickerDisplayType.TextBox
                    ddlbank.SelectedIndex = 0
                Else
                    txduedate.SelectedDate = txdate.SelectedDate
                    txduedate.DisplayType = DatePickerDisplayType.TextBoxAndImage
                End If

                ChangeCurrency(ddlCurr, txdate, txRate, ViewState("Currency"), ViewState("DigitCurr"), DBConnection)
            End If
            txduedate.Enabled = TxFgMode.Text = "G"
            ddlbank.Enabled = TxFgMode.Text = "G"
        Catch ex As Exception
            Throw New Exception("Change Payment Error : " + ex.ToString)
        End Try
    End Sub

End Class
