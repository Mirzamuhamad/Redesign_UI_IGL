Imports System.Data
Imports System.Data.SqlClient
Partial Class Transaction_TrProSchedule_TrProSchedule
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select * From V_PDScheduleHd"

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
                If ViewState("Sender") = "btnStart" Then
                    tbStartPeriod.Text = Session("Result")(0).ToString
                    lbStartPeriod.Text = Session("Result")(1).ToString
                    
                End If
                If ViewState("Sender") = "btnEnd" Then
                    tbEndPeriod.Text = Session("Result")(0).ToString
                    lbEndPeriod.Text = Session("Result")(1).ToString
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
            lbChargeHome.Text = "Charge (" + ViewState("Currency") + ")"
            'FillCombo(ddlCurrDt, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
            'FillCombo(ddlCurrDt2, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
            'FillCombo(ddlChargeCurrDt2, "EXEC S_GetCurrency", True, "Currency", "Currency", ViewState("DBConnection"))
            'FillCombo(ddlCostCenterDt, "EXEC S_GetCostCtr", True, "Cost_Ctr_Code", "Cost_Ctr_Name", ViewState("DBConnection"))
            
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
            tbAmountHomeDt.Attributes.Add("ReadOnly", "True")
            tbChargeHomeDt2.Attributes.Add("ReadOnly", "True")
            tbPaymentHomeDt2.Attributes.Add("ReadOnly", "True")

            tbRateDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbRateDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbChargeRateDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")

            tbAmountForexDt.Attributes.Add("OnKeyDown", "return PressNumericMinus();")
            tbPaymentForexDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbChargeForexDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")

            tbRateDt.Attributes.Add("OnBlur", "kali(" + Me.tbRateDt.ClientID + "," + Me.tbAmountForexDt.ClientID + "," + Me.tbAmountHomeDt.ClientID + "); setformatdt();")
            tbAmountForexDt.Attributes.Add("OnBlur", "kali(" + Me.tbRateDt.ClientID + "," + Me.tbAmountForexDt.ClientID + "," + Me.tbAmountHomeDt.ClientID + "); setformatdt();")

            tbRateDt2.Attributes.Add("OnBlur", "kali(" + Me.tbRateDt2.ClientID + "," + Me.tbPaymentForexDt2.ClientID + "," + Me.tbPaymentHomeDt2.ClientID + "); setformatdt2();")
            tbPaymentForexDt2.Attributes.Add("OnBlur", "kali(" + Me.tbRateDt2.ClientID + "," + Me.tbPaymentForexDt2.ClientID + "," + Me.tbPaymentHomeDt2.ClientID + "); setformatdt2();")

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
        Return "SELECT * From V_PDScheduleDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Private Function GetStringDt2(ByVal Nmbr As String) As String
        Return "SELECT * From V_FNProScheduleCr WHERE TransNmbr = " + QuotedStr(Nmbr)
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
                Session("SelectCommand") = "EXEC S_FNFormBuktiBankDt " + Result + ",'PAYMENTNONTRADE'," + QuotedStr(ViewState("UserId"))
                Session("ReportFile") = ".../../../Rpt/FormBuktiBankDt.frx"
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
                        Result = ExecSPCommandGo(ActionValue, "S_FNProSchedule", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            btnStartPeriod.Enabled = State
            tbStartPeriod.Enabled = State
            btnEndPeriod.Enabled = State
            tbEndPeriod.Enabled = State
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
            'GridDt.HeaderRow.Cells(9).Text = "Amount (" + ViewState("Currency") + ")"
            'End If
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub

    'Private Sub BindDataDt2(ByVal Nmbr As String)
    '    Try
    '        Dim dt As New DataTable
    '        Dim dr As DataRow
    '        ViewState("Dt2") = Nothing
    '        dt = SQLExecuteQuery(GetStringDt2(Nmbr), ViewState("DBConnection").ToString).Tables(0)
    '        ViewState("Dt2") = dt
    '        BindGridDt(dt, GridDt2)
    '        For Each dr In dt.Rows
    '            If dr("FgMode").ToString = "B" Or dr("FgMode").ToString = "K" Or dr("FgMode").ToString = "G" Or dr("FgMode").ToString = "D" Then
    '                ViewState("PayType") = dt.Rows(0)("PaymentType").ToString
    '                Exit For
    '            End If
    '        Next
    '    Catch ex As Exception
    '        Throw New Exception("Bind Data Dt Error : " + ex.ToString)
    '    End Try
    'End Sub

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
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnCancelDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelDt2.Click
        Try
            MovePanel(pnlEditDt2, pnlDt2)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt2  Error : " + ex.ToString
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbCode.Text = ""
            tbStartPeriod.Text = ""
            lbStartPeriod.Text = ""
            tbEndPeriod.Text = ""
            lbEndPeriod.Text = ""
            tbDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            tbRemark.Text = ""
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
            tbRateDt.Text = FormatFloat(1, ViewState("DigitRate"))
            tbRateDt.Enabled = False
            tbAmountForexDt.Text = "0"
            tbAmountHomeDt.Text = "0"
            tbRemarkDt.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnStartPeriod_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStartPeriod.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DbConnection")
            If tbEndPeriod.Text = "" Then
                Session("filter") = "Select PeriodCode, PeriodName, Year, dbo.FormatDate(StartDate) AS Start_Date, dbo.FormatDate(EndDate) AS End_Date from VMsPeriod "
            Else
                Session("filter") = "Select PeriodCode, PeriodName, Year, dbo.FormatDate(StartDate) AS Start_Date, dbo.FormatDate(EndDate) AS End_Date from VMsPeriod WHERE PeriodCode <= " + QuotedStr(tbEndPeriod.Text)
            End If
            ResultField = "PeriodCode, PeriodName"
            ViewState("Sender") = "btnStart"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnStartPeriod_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnEndPeriod_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEndPeriod.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DbConnection")
            If tbStartPeriod.Text = "" Then
                Session("filter") = "Select PeriodCode, PeriodName, Year, dbo.FormatDate(StartDate) AS Start_Date, dbo.FormatDate(EndDate) AS End_Date from VMsPeriod "
            Else
                Session("filter") = "Select PeriodCode, PeriodName, Year, dbo.FormatDate(StartDate) AS Start_Date, dbo.FormatDate(EndDate) AS End_Date from VMsPeriod WHERE PeriodCode >= " + QuotedStr(tbStartPeriod.Text)
            End If
            ResultField = "PeriodCode, PeriodName"
            ViewState("Sender") = "btnEnd"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnEndPeriod_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbStartPeriod_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbStartPeriod.TextChanged
        Dim dt As DataTable
        Try
            If tbEndPeriod.Text = "" Then
                dt = SQLExecuteQuery("Select PeriodCode, PeriodName, Year, dbo.FormatDate(StartDate) AS Start_Date, dbo.FormatDate(EndDate) AS End_Date from VMsPeriod WHERE PeriodCode = " + QuotedStr(tbStartPeriod.Text), ViewState("DBConnection")).Tables(0)
            Else
                dt = SQLExecuteQuery("Select PeriodCode, PeriodName, Year, dbo.FormatDate(StartDate) AS Start_Date, dbo.FormatDate(EndDate) AS End_Date from VMsPeriod WHERE PeriodCode = " + QuotedStr(tbStartPeriod.Text) + " AND PeriodCode <= " + QuotedStr(tbEndPeriod.Text), ViewState("DBConnection")).Tables(0)
            End If
            If dt.Rows.Count > 0 Then
                tbStartPeriod.Text = dt.Rows(0)("PeriodCode").ToString
                lbStartPeriod.Text = dt.Rows(0)("PeriodName").ToString
                If tbEndPeriod.Text < tbStartPeriod.Text Then
                    tbEndPeriod.Text = ""
                    lbEndPeriod.Text = ""
                End If
            Else
                tbStartPeriod.Text = ""
                lbStartPeriod.Text = ""
            End If
        Catch ex As Exception
            lbStatus.Text = "tbStartPeriod_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbEndPeriod_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbEndPeriod.TextChanged
        Dim dt As DataTable
        Try
            If tbStartPeriod.Text = "" Then
                dt = SQLExecuteQuery("Select PeriodCode, PeriodName, Year, dbo.FormatDate(StartDate) AS Start_Date, dbo.FormatDate(EndDate) AS End_Date from VMsPeriod WHERE PeriodCode = " + QuotedStr(tbEndPeriod.Text), ViewState("DBConnection")).Tables(0)
            Else
                dt = SQLExecuteQuery("Select PeriodCode, PeriodName, Year, dbo.FormatDate(StartDate) AS Start_Date, dbo.FormatDate(EndDate) AS End_Date from VMsPeriod WHERE PeriodCode = " + QuotedStr(tbEndPeriod.Text) + " AND PeriodCode >= " + QuotedStr(tbStartPeriod.Text), ViewState("DBConnection")).Tables(0)
            End If
            If dt.Rows.Count > 0 Then
                tbEndPeriod.Text = dt.Rows(0)("PeriodCode").ToString
                lbEndPeriod.Text = dt.Rows(0)("PeriodName").ToString
                If tbEndPeriod.Text < tbStartPeriod.Text Then
                    tbEndPeriod.Text = ""
                    lbEndPeriod.Text = ""
                End If
            Else
                tbEndPeriod.Text = ""
                lbEndPeriod.Text = ""
            End If
        Catch ex As Exception
            lbStatus.Text = "tbEndPeriod_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    'Private Sub Cleardt2()
    '    Try
    '        ddlPayTypeDt2.SelectedIndex = 0
    '        tbPaymentDateDt2.SelectedDate = ViewState("ServerDate") 'Now.Date
    '        tbDocumentNoDt2.Text = ""
    '        tbVoucherNo.Enabled = False
    '        tbVoucherNo.Text = ""
    '        tbDueDateDt2.SelectedDate = Nothing
    '        tbGiroDateDt2.SelectedDate = Nothing
    '        ddlCurrDt2.SelectedValue = ViewState("Currency")
    '        tbPaymentForexDt2.Text = "0"
    '        tbPaymentHomeDt2.Text = "0"
    '        tbChargeRateDt2.Text = "0"
    '        tbChargeForexDt2.Text = "0"
    '        tbChargeHomeDt2.Text = "0"
    '        tbRemarkDt2.Text = ""
    '        'ChangeCurrency(ddlCurrDt2, tbDate, tbRateDt2, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
    '        'ChangePaymentType(ddlPayTypeDt2.SelectedValue, tbFgModeDt2, tbPaymentDateDt2, tbDueDateDt2, ddlBankPaymentDt2, ddlCurrDt2, ddlChargeCurrDt2, tbRateDt2, tbChargeRateDt2, tbChargeForexDt2, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
    '        'ChangePaymentType(ddlPayTypeDt2.SelectedValue, tbFgModeDt2, tbPaymentDateDt2, tbGiroDateDt2, ddlBankPaymentDt2, ddlCurrDt2, ddlChargeCurrDt2, tbRateDt2, tbChargeRateDt2, tbChargeForexDt2, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
    '    Catch ex As Exception
    '        Throw New Exception("Clear Dt 2 Error " + ex.ToString)
    '    End Try
    'End Sub

    Function CekHd() As Boolean
        Try
            If tbDate.SelectedDate = Nothing Then
                lbStatus.Text = MessageDlg("Date must have value")
                tbDate.Focus()
                Return False
            End If
            If tbStartPeriod.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Start Period must have value")
                tbStartPeriod.Focus()
                Return False
            End If
            If tbEndPeriod.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("End Period must have value")
                tbEndPeriod.Focus()
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
            BindToText(tbStartPeriod, Dt.Rows(0)("StartPeriod").ToString)
            BindToText(lbStartPeriod, Dt.Rows(0)("StartPeriodName").ToString)
            BindToText(tbEndPeriod, Dt.Rows(0)("EndPeriod").ToString)
            BindToText(lbEndPeriod, Dt.Rows(0)("EndPeriodName").ToString)
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
                tbPaymentDateDt2.SelectedDate = Dr(0)("PaymentDate").ToString
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
                BindToDropList(ddlBankPaymentDt2, Dr(0)("BankPayment").ToString)
                BindToDropList(ddlChargeCurrDt2, Dr(0)("ChargeCurrency").ToString)
                ViewState("DigitCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlCurrDt2.SelectedValue), ViewState("DBConnection"))
                ViewState("DigitExpenseCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlChargeCurrDt2.SelectedValue), ViewState("DBConnection"))
                BindToText(tbChargeRateDt2, Dr(0)("ChargeRate").ToString)
                BindToText(tbChargeForexDt2, Dr(0)("ChargeForex").ToString)
                BindToText(tbChargeHomeDt2, Dr(0)("ChargeHome").ToString)

                If ddlChargeCurrDt2.SelectedValue = "" Then
                    tbChargeForexDt2.Text = "0"
                    tbChargeHomeDt2.Text = "0"
                End If
                tbChargeForexDt2.Enabled = ddlChargeCurrDt2.SelectedValue <> ""
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
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
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
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
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
            If Not (ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit") Then
                Exit Sub
            Else
                ViewState("StateSave") = ViewState("StateHd")
                ViewState("StateHd") = "View"
            End If
            'Save Hd
            If ViewState("StateSave") = "Insert" Then
                tbCode.Text = GetAutoNmbr("PDS", "N", CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("PayType").ToString, ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO ProScheduleHd (TransNmbr, TransDate, STATUS, StartPeriod, EndPeriod, Remark, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", 'H', " + _
                QuotedStr(tbStartPeriod.Text) + ", " + QuotedStr(tbEndPeriod.Text) + ", " + QuotedStr(tbRemark.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", GetDate()"
                ViewState("TransNmbr") = tbCode.Text

            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM ProScheduleHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE ProScheduleHd SET TransDate = " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", StartPeriod=" + QuotedStr(tbStartPeriod.Text) + ", EndPeriod=" + QuotedStr(tbEndPeriod.Text) + _
                ", Remark = " + QuotedStr(tbRemark.Text) + ", DatePrep = GetDate()" + _
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

            'Row = ViewState("Dt2").Select("TransNmbr IS NULL")
            'For I = 0 To Row.Length - 1
            '    Row(I).BeginEdit()
            '    Row(I)("TransNmbr") = tbCode.Text
            '    Row(I).EndEdit()
            'Next

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("Select TransNmbr, Period, Product, Machine, ScheduleDate, Shift, StartDate, StartTime, Qty, CycleTime, SettingTime, CostTime, EndDate, EndTime, OffsetStr1, OffsetStr2, OffsetFloat1, OffsetFloat2, MinAllowOffset, MinAllowStart, PolaCode, FgJoin, JoinWith, FgOverDue, UrutProcess FROM PROScheduleDt WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt As New DataTable("PROScheduleDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt

            'save dt2
            'cmdSql = New SqlCommand("SELECT TransNmbr, ItemNo, PaymentType, PaymentDate, DocumentNo, Reference, GiroDate, DueDate, BankPayment, FgMode, Currency, ForexRate, PaymentForex, PaymentHome, Remark, ChargeCurrency, ChargeRate, ChargeForex, ChargeHome FROM FINProScheduleCr WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            'da = New SqlDataAdapter(cmdSql)
            'dbcommandBuilder = New SqlCommandBuilder(da)
            'da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            'Dim Dt2 As New DataTable("FINProScheduleCr")

            'Dt2 = ViewState("Dt2")
            'da.Update(Dt2)
            'Dt2.AcceptChanges()
            'ViewState("Dt2") = Dt2
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
            'If GetCountRecord(ViewState("Dt")) = 0 Then
            '    lbStatus.Text = MessageDlg("Detail Account must have at least 1 record")
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
            tbAccountDt.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnAddDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddDt2.Click, btnAddDt2Ke2.Click
        Try
            'Cleardt2()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt2") = "Insert"
            MovePanel(pnlDt2, pnlEditDt2)
            EnableHd(False)
            StatusButtonSave(False)
            lbItemNoDt2.Text = GetNewItemNo(ViewState("Dt2"))
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
            'Cleardt2()
            PnlDt.Visible = True
            BindDataDt("")
            'BindDataDt2("")
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
            FDateName = "Schedule Date"
            FDateValue = "TransDate"
            FilterName = "Schedule No, Schedule Date, Start Period, End Period, Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), StartPeriodName, EndPeriodName, Remark"
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
                    'BindDataDt2(ViewState("TransNmbr"))
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
                        'BindDataDt2(ViewState("TransNmbr"))
                        FillTextBoxHd(ViewState("TransNmbr"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, PnlDt, GridDt)
                        ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
                        btnHome.Visible = False
                        MultiView1.ActiveViewIndex = 0
                        Menu1.Items.Item(0).Selected = True
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
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
                        'Session("SelectCommand") = "EXEC S_FNFormBuktiBank '" + GVR.Cells(2).Text + "','PAYMENTNONTRADE'"
                        'Session("ReportFile") = ".../../../Rpt/FormBuktiBank.frx"
                        Session("SelectCommand") = "EXEC S_FNFormBuktiBankDt '''" + GVR.Cells(2).Text + "''','PAYMENTNONTRADE'," + QuotedStr(ViewState("UserId"))
                        Session("ReportFile") = ".../../../Rpt/FormBuktiBankDt.frx"
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

    'Dim TotalExpense As Decimal = 0
    ' untuk tampilkan data total di grid
    Protected Sub GridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt.RowDataBound
        Try
            'If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "Product")) Then
            '    If e.Row.RowType = DataControlRowType.DataRow Then
            '        ' add the UnitPrice and QuantityTotal to the running total variables
            '        TotalExpense += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "AmountHome"))
            '    ElseIf e.Row.RowType = DataControlRowType.Footer Then
            '        tbTotalExpense.Text = FormatNumber(TotalExpense, ViewState("DigitHome"))
            '    End If
            'End If
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
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    
    Protected Sub GridDt2_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt2.RowDataBound
        Try
            'If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "ItemNo")) Then
            '    If e.Row.RowType = DataControlRowType.DataRow Then
            '        ' add the UnitPrice and QuantityTotal to the running total variables
            '        TotalPayment += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "PaymentHome"))
            '        TotalCharge += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "ChargeHome"))
            '    ElseIf e.Row.RowType = DataControlRowType.Footer Then
            '        tbTotalPayment.Text = FormatNumber(TotalPayment, ViewState("DigitHome"))
            '        tbTotalCharge.Text = FormatNumber(TotalCharge, ViewState("DigitHome"))
            '    End If
            'End If
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
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
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
            ViewState("StateDt2") = "Edit"
            StatusButtonSave(False)
            btnDocNo.Visible = tbFgModeDt2.Text = "D"
            tbDocumentNoDt2.Enabled = Not tbFgModeDt2.Text = "D"
            btnSaveDt2.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid dt2 Editing Error : " + ex.ToString
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

    Protected Sub ddlChargeCurrDt2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlChargeCurrDt2.SelectedIndexChanged
        Try
            ChangeCurrency(ddlChargeCurrDt2, tbPaymentDateDt2, tbChargeRateDt2, ViewState("Currency"), ViewState("DigitExpenseCurr"), ViewState("DBConnection"))
            If ddlChargeCurrDt2.SelectedValue = "" Then
                tbChargeForexDt2.Text = "0"
                tbChargeHomeDt2.Text = "0"
            End If
            ViewState("DigitExpenseCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlChargeCurrDt2.SelectedValue), ViewState("DBConnection"))
            tbChargeForexDt2.Enabled = ddlChargeCurrDt2.SelectedValue <> ""
            tbChargeRateDt2.Focus()
        Catch ex As Exception
            lbStatus.Text = "ddl curr expense selected index changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveAll.Click
        Dim CurrFilter, Value As String
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail Account must have at least 1 record")
                Exit Sub
            End If
            'If GetCountRecord(ViewState("Dt2")) = 0 Then
            If ViewState("PayType").ToString = "" Then
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

End Class
