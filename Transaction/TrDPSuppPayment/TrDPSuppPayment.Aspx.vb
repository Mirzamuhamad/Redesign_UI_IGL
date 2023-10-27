Imports System.Data
'Imports System.Data.SqlClient
'Imports System.Data.Odbc
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc

Partial Class TrDPSuppPayment
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select TransNmbr, Nmbr, Status, TransDate, FgReport, SuppInvNo, Supplier, " + _
                                        "Attn, PONo, POReport, PPNNo, PPNDate, PPNRate, Currency, Forexrate, BaseForex, " + _
                                        "PPn, PPNForex, TotalForex, Remark, UserPrep, DatePrep, UserAppr, " + _
                                        "DateAppr, BalanceBase, BalancePPn, User_Type, User_Code, User_Name, CostCtr From V_FNDPSuppHd"

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

            ' hasil dari search dialog
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnSupp" Then
                    tbSuppCode.Text = Session("Result")(0).ToString
                    BindToText(tbSuppName, Session("Result")(1).ToString)
                    BindToDropList(ddlCurr, Session("Result")(2).ToString)
                    BindToText(tbAttn, Session("Result")(3).ToString)
                    tbPONo.Text = ""
                    tbPOReport.Text = "N"
                    ddlCurr.Enabled = tbPONo.Text = ""
                    'ddlReport.Enabled = tbPOReport.Text = "Y"
                    ddlCurr_SelectedIndexChanged(Nothing, Nothing)
                    FillCombo(ddlPayTypeDt2, "EXEC S_GetPayTypeUser 'PAYMENTDP|" + "Y" + "', " + QuotedStr(ViewState("UserId").ToString), True, "Payment_Code", "Payment_Name", ViewState("DBConnection")) '+ ddlReport.SelectedValue +
                End If
                If ViewState("Sender") = "btnPO" Then
                    'ResultField = "Reff_No, Currency, Base_Forex, PPN, Total_Forex, DP_Forex, Supplier_Code, Supplier_Name, FgReport, Term_Payment, CostCtr"
                    tbPONo.Text = Session("Result")(0).ToString
                    BindToDropList(ddlCurr, Session("Result")(1).ToString)
                    tbPPN.Text = "0"
                    ddlCurr.Enabled = tbPONo.Text = ""
                    'BindToText(tbPPN, Session("Result")(3).ToString)
                    ViewState("PONo") = Session("Result")(0).ToString
                    ViewState("DPForex") = CFloat(Session("Result")(5).ToString)
                    BindToText(tbSuppCode, Session("Result")(6).ToString)
                    BindToText(tbSuppName, Session("Result")(7).ToString)
                    BindToText(tbPOReport, Session("Result")(8).ToString)
                    tbRemark.Text = TrimStr(Session("Result")(9).ToString)
                    BindToDropList(ddlCostCtr, Session("Result")(10).ToString)
                    ddlCostCtr.Enabled = (ddlCostCtr.SelectedValue = "") And (tbPONo.Text = "")
                    'ddlReport.Enabled = tbPOReport.Text = "Y"
                    FillCombo(ddlPayTypeDt2, "EXEC S_GetPayTypeUser 'PAYMENT|" + "Y" + "', " + QuotedStr(ViewState("UserId").ToString), True, "Payment_Code", "Payment_Name", ViewState("DBConnection")) '+ ddlReport.SelectedValue +
                    tbBaseForex.Text = FormatFloat(0, ViewState("DigitCurr"))
                    tbTotalForex.Text = FormatFloat(0, ViewState("DigitCurr"))
                    AttachScript("BasePPnTotal(" + Me.tbBaseForex.ClientID + "," + Me.tbPPN.ClientID + "," + Me.tbPPNForex.ClientID + "," + Me.tbTotalForex.ClientID + ", " + Me.tbPPN.Text + "); setformat();", Me.Page, Me.GetType())
                    ddlCurr_SelectedIndexChanged(Nothing, Nothing)
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
        FillRange(ddlRange)
        ViewState("PayType") = ""
        ViewState("SortExpression") = Nothing
        GridView1.PageSize = CInt(ViewState("PageSizeGrid"))
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
        FillCombo(ddlCostCtr, "EXEC S_GetCostCtr", True, "Cost_Ctr_Code", "Cost_Ctr_Name", ViewState("DBConnection"))
        FillCombo(ddlCurr, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))

        If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
            ddlCommand.Items.Add("Print")
            ddlCommand2.Items.Add("Print")
            'ddlCommand.Items.Add("Print Full")
            'ddlCommand2.Items.Add("Print Full")
        End If
        lbPayHome.Text = "Payment (" + ViewState("Currency") + ")"
        'lbChargeHome.Text = "Charge (" + ViewState("Currency") + ")"
        lbDPHome.Text = "DP (" + ViewState("Currency") + ")"
        GridDt.Columns(13).HeaderText = "Payment (" + ViewState("Currency") + ")"
        'GridDt.Columns(14).HeaderText = "Charge (" + ViewState("Currency") + ")"
        GridDt.Columns(14).HeaderText = "DP (" + ViewState("Currency") + ")"
        FillCombo(ddlCurrDt2, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
        'FillCombo(ddlChargeCurrDt2, "EXEC S_GetCurrency", True, "Currency", "Currency", ViewState("DBConnection"))
        'tbChargeHomeDt2.Attributes.Add("ReadOnly", "True")
        tbPaymentHomeDt2.Attributes.Add("ReadOnly", "True")
        tbDPForexDt2.Attributes.Add("ReadOnly", "True")
        tbDPHomeDt2.Attributes.Add("ReadOnly", "True")

        tbRateDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")
        'tbChargeRateDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")

        tbPaymentForexDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")
        'tbChargeForexDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")
        'tbDPForexDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbDPHomeDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")

        tbRateDt2.Attributes.Add("OnBlur", "kali(" + Me.tbRateDt2.ClientID + "," + Me.tbPaymentForexDt2.ClientID + "," + Me.tbPaymentHomeDt2.ClientID + "); setformatdt();")
        tbPaymentForexDt2.Attributes.Add("OnBlur", "kali(" + Me.tbRateDt2.ClientID + "," + Me.tbPaymentForexDt2.ClientID + "," + Me.tbPaymentHomeDt2.ClientID + "); setformatdt();")

        'tbChargeRateDt2.Attributes.Add("OnBlur", "kali(" + Me.tbChargeRateDt2.ClientID + "," + Me.tbChargeForexDt2.ClientID + "," + Me.tbChargeHomeDt2.ClientID + "); setformatdt();")
        'tbChargeForexDt2.Attributes.Add("OnBlur", "kali(" + Me.tbChargeRateDt2.ClientID + "," + Me.tbChargeForexDt2.ClientID + "," + Me.tbChargeHomeDt2.ClientID + "); setformatdt();")

        Me.tbBaseForex.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbPPN.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbPPNRate.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbBaseForex.Attributes.Add("ReadOnly", "True")
        tbPPNForex.Attributes.Add("ReadOnly", "True")
        tbTotalForex.Attributes.Add("ReadOnly", "True")

        Me.tbBaseForex.Attributes.Add("OnBlur", "BasePPnTotal(" + Me.tbBaseForex.ClientID + "," + Me.tbPPN.ClientID + "," + Me.tbPPNForex.ClientID + "," + Me.tbTotalForex.ClientID + "); setformat();")
        Me.tbPPN.Attributes.Add("OnBlur", "BasePPnTotal(" + Me.tbBaseForex.ClientID + "," + Me.tbPPN.ClientID + "," + Me.tbPPNForex.ClientID + "," + Me.tbTotalForex.ClientID + "); setformat();")

        Me.tbDPForexDt2.Attributes.Add("OnBlur", "setformat();")
        'Me.tbDPHomeDt2.Attributes.Add("OnBlur", "setformatDt();")

        Me.tbRate.Attributes.Add("OnKeyDown", "return PressNumeric();")
        'Me.tbAmountDP.Attributes.Add("OnBlur", "setformat();")
    End Sub

    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbFilter2.Text, ddlNotasi.SelectedValue, ddlRange.SelectedValue)
            If AdvanceFilter.Length > 1 And StrFilter.Length > 1 Then
                StrFilter = StrFilter + " And " + AdvanceFilter
            ElseIf AdvanceFilter.Length > 1 And StrFilter.Length <= 1 Then
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
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT TransNmbr,ItemNo,PaymentType,PaymentDate,DocumentNo,Reference,Currency,Forexrate,PaymentForex,PaymentHome,DPForex,DPHome,Remark,FgMode,BankPayment, GiroDate, DueDate,ChargeCurrency,ChargeRate,ChargeForex,ChargeHome,PaymentTypeName,BankPaymentName, FgValue From V_FNDPSuppDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            CheckAll(GridView1, sender)
        Catch ex As Exception
            lbStatus.Text = "cb Select Checked Changed Error : " + ex.ToString
        End Try
    End Sub
    Private Sub EnableHd(ByVal State As Boolean)
        Try
            ddlUserType.Enabled = State
            tbSuppCode.Enabled = State
            btnSupp.Enabled = State
            tbPONo.Enabled = State
            btnPO.Enabled = State
            tbTransDate.Enabled = State
            ddlCurr.Enabled = tbPONo.Text = "" And State
            tbRate.Enabled = State And ddlCurr.SelectedValue <> ViewState("Currency")
            tbPPNRate.Enabled = State 'And ddlReport.SelectedValue = "Y"
            tbPPN.Enabled = State And ddlUserType.SelectedValue = "Supplier"

            'ddlReport.Enabled = State And tbPOReport.Text = "Y"
            'btnGetDt.Visible = State
        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDt(ByVal Referens As String)
        Try
            Dim dt As New DataTable
            Dim dr As DataRow
            ViewState("Dt") = Nothing
            dt = SQLExecuteQuery(GetStringDt(Referens), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt") = dt
            BindGridDt(dt, GridDt)
            For Each dr In dt.Rows
                If dr("FgMode").ToString = "B" Or dr("FgMode").ToString = "K" Or dr("FgMode").ToString = "G" Or dr("FgMode").ToString = "D" Then
                    ViewState("PayType") = dt.Rows(0)("PaymentType").ToString
                    Exit For
                End If
            Next
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub


    Private Sub SaveAll()
        Dim SQLString As String
        Dim tgl As String
        Dim I As Integer
        Try
            If pnlDt.Visible = False Then
                lbStatus.Text = "Detail Data must be saved first"
                Exit Sub
            End If
            'Save Hd
            If tbPPNDate.IsNull Then
                tgl = "NULL"
            Else
                tgl = QuotedStr(Format(tbPPNDate.SelectedDate, "yyyy-MM-dd"))
            End If
            If Not (ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit") Then
                Exit Sub
            Else
                ViewState("StateSave") = ViewState("StateHd")
                ViewState("StateHd") = "View"
            End If
            'Save Hd
            If ViewState("StateSave") = "Insert" Then
                'insert
                'ddlReport.SelectedValue
                tbTransNo.Text = GetAutoNmbr("DS", "Y", Year(tbTransDate.SelectedValue), Month(tbTransDate.SelectedValue), ViewState("PayType").ToString, ViewState("DBConnection").ToString)

                SQLString = "Insert INTO FINDPSuppHd (TransNmbr, Status, TransDate, FgReport, POReport, " + _
                "PONo, UserType, UserCode, SuppInvNo, Attn, Currency, ForexRate, PPNNo, PPNDate, " + _
                "CostCtr, " + _
                "PPNRate, BaseForex, PPN, PPNForex, TotalForex, Remark, UserPrep, DatePrep, BalanceBase, BalancePPN) " + _
                "SELECT " + QuotedStr(tbTransNo.Text) + ",'H'," + QuotedStr(Format(tbTransDate.SelectedDate, "yyyy-MM-dd")) + _
                "," + QuotedStr("Y") + ", " + QuotedStr(tbPOReport.Text) + "," + QuotedStr(tbPONo.Text) + "," + _
                QuotedStr(ddlUserType.SelectedValue) + "," + QuotedStr(tbSuppCode.Text) + "," + QuotedStr(tbSuppInvNo.Text) + "," + QuotedStr(tbAttn.Text) + "," + _
                QuotedStr(ddlCurr.SelectedValue) + "," + tbRate.Text.Replace(",", "") + "," + QuotedStr(tbPPNNo.Text) + "," + _
                tgl + "," + QuotedStr(ddlCostCtr.SelectedValue) + "," + tbPPNRate.Text.Replace(",", "") + "," + _
                tbBaseForex.Text.Replace(",", "") + "," + tbPPN.Text.Replace(",", "") + "," + _
                tbPPNForex.Text.Replace(",", "") + "," + tbTotalForex.Text.Replace(",", "") + "," + _
                QuotedStr(tbRemark.Text) + "," + QuotedStr(ViewState("UserId").ToString) + ", getdate(),0,0"
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM FINDPSuppHd WHERE TransNmbr = " + QuotedStr(tbTransNo.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                'edit
                SQLString = "UPDATE FINDPSuppHd SET TransDate = " + QuotedStr(Format(tbTransDate.SelectedDate, "yyyy-MM-dd")) + _
                ", FgReport=" + QuotedStr("Y") + ", PONo=" + QuotedStr(tbPONo.Text) + ", UserType=" + QuotedStr(ddlUserType.SelectedValue) + ", UserCode=" + QuotedStr(tbSuppCode.Text) + _
                ", Attn=" + QuotedStr(tbAttn.Text) + ", SuppInvNo=" + QuotedStr(tbSuppInvNo.Text) + ", Currency=" + QuotedStr(ddlCurr.SelectedValue) + ", ForexRate=" + tbRate.Text.Replace(",", "") + ", PPNNo=" + QuotedStr(tbPPNNo.Text) + _
                ", PPNDate=" + tgl + ", PPNRate=" + tbPPNRate.Text.Replace(",", "") + _
                ", CostCtr=" + QuotedStr(ddlCostCtr.SelectedValue) + _
                ", BaseForex= " + tbBaseForex.Text.Replace(",", "") + ", PPN=" + tbPPN.Text.Replace(",", "") + _
                ", PPNForex= " + tbPPNForex.Text.Replace(",", "") + ", TotalForex=" + tbTotalForex.Text.Replace(",", "") + _
                ", Remark=" + QuotedStr(tbRemark.Text) + _
                " WHERE TransNmbr = " + QuotedStr(tbTransNo.Text)
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
                Row(I)("TransNmbr") = tbTransNo.Text
                'Row(I)("TransClass") = "JE"
                Row(I).EndEdit()
            Next

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString 'System.Configuration.ConfigurationManager.AppSettings.Get("ConString")
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr,ItemNo,PaymentType,PaymentDate,DocumentNo,Reference,Currency,ForexRate,PaymentForex,PaymentHome,DPForex,DPHome,Remark,FgMode,BankPayment, GiroDate, DueDate, FgValue FROM FINDPSuppDt WHERE TransNmbr = '" & ViewState("Reference") & "'", con)
            ',ChargeCurrency,ChargeRate,ChargeForex,ChargeHome
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt As New DataTable("{FINDPSuppDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt
        Catch ex As Exception
            Throw New Exception("Save All Data Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ViewState("PayType") = ""
            ViewState("DigitCurr") = 0
            ViewState("DigitCurrDt2") = 0
            ViewState("DigitExpenseCurr") = 0
            ClearHd()
            Cleardt()
            pnlDt.Visible = True
            BindDataDt("")
            EnableHd(True)
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbTransNo.Text = ""
            tbTransDate.SelectedDate = ViewState("ServerDate") 'Today
            tbPPNDate.SelectedDate = ViewState("ServerDate") 'Today
            tbSuppCode.Text = ""
            tbSuppName.Text = ""
            tbSuppInvNo.Text = ""
            tbPONo.Text = ""
            tbAttn.Text = ""
            tbPOReport.Text = "N"
            tbPPNNo.Text = ""
            tbPPN.Text = "0"
            ddlCurr.SelectedValue = ViewState("Currency")
            ViewState("DigitCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlCurr.SelectedValue), ViewState("DBConnection"))
            tbPPNForex.Text = FormatFloat(0, ViewState("DigitCurr"))
            tbPPNRate.Text = "0"
            tbBaseForex.Text = FormatFloat(0, ViewState("DigitCurr"))
            tbTotalForex.Text = FormatFloat(0, ViewState("DigitCurr"))
            tbRate.Text = FormatFloat("1", ViewState("DigitRate"))
            'ddlReport.SelectedValue = "N"
            'ddlReport.Enabled = tbPOReport.Text = "Y"
            ddlUserType.SelectedValue = "Supplier"
            tbRemark.Text = ""
            'FillCombo(ddlPayTypeDt2, "EXEC S_GetPayTypeUser " + QuotedStr(ddlReport.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString), True, "Payment_Code", "Payment_Name", ViewState("DBConnection"))
            FillCombo(ddlPayTypeDt2, "EXEC S_GetPayTypeUser 'PAYMENT|" + "Y" + "', " + QuotedStr(ViewState("UserId").ToString), True, "Payment_Code", "Payment_Name", ViewState("DBConnection")) '+ ddlReport.SelectedValue +
            FillCombo(ddlBankPaymentDt2, "EXEC S_GetBankPaymentTrade " + QuotedStr(ddlCurrDt2.SelectedValue), True, "Bank_Code", "Bank_Name", ViewState("DBConnection"))
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            ddlPayTypeDt2.SelectedIndex = 0
            tbPaymentDateDt2.SelectedDate = tbTransDate.SelectedDate
            tbDocumentNoDt2.Text = ""
            tbVoucherNo.Enabled = False
            tbVoucherNo.Text = ""
            tbDueDateDt2.SelectedDate = Nothing
            tbGiroDateDt2.SelectedDate = Nothing
            'ViewState("Currency2") = ddlCurr.SelectedValue
            ddlCurrDt2.SelectedValue = ViewState("Currency")
            tbPaymentForexDt2.Text = "0"
            tbPaymentHomeDt2.Text = "0"
            'tbChargeRateDt2.Text = "0"
            'tbChargeForexDt2.Text = "0"
            'tbChargeHomeDt2.Text = "0"
            tbDPForexDt2.Text = "0"
            tbDPHomeDt2.Text = "0"
            tbRemarkDt2.Text = tbRemark.Text
            tbValue.Text = "1"
            ViewState("DigitCurrDt2") = ViewState("DigitCurr")
            ChangeCurrency(ddlCurrDt2, tbTransDate, tbRateDt2, ViewState("Currency"), ViewState("DigitCurrDt2"), ViewState("DBConnection"), ViewState("DigitHome"), ViewState("DigitRate"))
            ChangePaymentTypeV2(ddlPayTypeDt2.SelectedValue, tbFgModeDt2, tbTransDate, tbDueDateDt2, ddlBankPaymentDt2, ddlCurrDt2, tbRateDt2, ViewState("Currency"), ViewState("DigitCurrDt2"), ViewState("DBConnection"))
            FillCombo(ddlBankPaymentDt2, "EXEC S_GetBankPaymentTrade " + QuotedStr(ddlCurrDt2.SelectedValue), True, "Bank_Code", "Bank_Name", ViewState("DBConnection"))
            'ChangePaymentType(ddlPayTypeDt2.SelectedValue, tbFgModeDt2, tbPaymentDateDt2, tbGiroDateDt2, ddlBankPaymentDt2, ddlCurrDt2, ddlChargeCurrDt2, tbRateDt2, tbChargeRateDt2, tbChargeForexDt2, ViewState("Currency2"), ViewState("DigitCurrDt2"), ViewState("DBConnection"))
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub

    Function CekHd() As Boolean
        Try
            'If tbDate.IsNull Then
            '    lbStatus.Text = MessageDlg("Date must have value")
            '    tbDate.Focus()
            '    Return False
            'End If
            'If ddlReqType.SelectedValue.Trim = "" Then
            '    lbStatus.Text = "Request Type must have value"
            '    ddlReqType.Focus()
            '    Return False
            'End If
            'If CInt(Session(Request.QueryString("KeyId"))("Year")) <> Year(tbTransDate.SelectedValue) Or CInt(Session(Request.QueryString("KeyId"))("Period")) <> Month(tbTransDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(Session(Request.QueryString("KeyId"))("Period")) + " " + Session(Request.QueryString("KeyId"))("Year").ToString.Trim)
            '    Return False
            'End If

            If tbSuppCode.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("" + ddlUserType.SelectedValue + " must have value")
                tbSuppCode.Focus()
                Return False
            End If
            If tbSuppName.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("" + ddlUserType.SelectedValue + " must have value")
                tbSuppName.Focus()
                Return False
            End If
            'If tbPONo.Text.Trim = "" Then
            '    lbStatus.Text = "PO No. must have value"
            '    tbPONo.Focus()
            '    Return False
            'End If
            If tbSuppInvNo.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Supp Inv No. must have value")
                tbSuppInvNo.Focus()
                Return False
            End If
            If ddlCostCtr.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Cost Ctr must have value")
                ddlCostCtr.Focus()
                Return False
            End If
            If ddlCurr.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Currency must have value")
                ddlCurr.Focus()
                Return False
            End If
            If CFloat(tbRate.Text) <= 0 Then
                lbStatus.Text = MessageDlg("Forex Rate must have value")
                tbRate.Focus()
                Return False
            End If
            If ddlCurr.SelectedValue <> ViewState("Currency") And CFloat(tbRate.Text) = 1 Then
                lbStatus.Text = MessageDlg("Rate must have value")
                tbRate.Focus()
                Return False
            End If
            If CFloat(tbPPN.Text) < 0  Then 'And ddlReport.SelectedValue = "Y"
                lbStatus.Text = MessageDlg("PPN must have value")
                tbPPN.Focus()
                Return False
            End If
            If CFloat(tbPPNRate.Text) < 0 Then 'And ddlReport.SelectedValue = "Y"
                lbStatus.Text = MessageDlg("PPN Rate must have value")
                tbPPNRate.Focus()
                Return False
            End If
            'If CFloat(tbBaseForex.Text) <= 0 Then
            '    lbStatus.Text = "Base Forex must have value"
            '    tbBaseForex.Focus()
            '    Return False
            'End If

            Return True
        Catch ex As Exception
            Throw New Exception("Cek Hd Error : " + ex.ToString)
        End Try
    End Function

    Function CekDt(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not (Dr Is Nothing) Then
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
                    If Dr("GiroDate").ToString = "" Then
                        lbStatus.Text = MessageDlg("Giro Date Must Have Value")
                        Return False
                    End If
                    If Dr("DueDate").ToString = "" Then
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
                    lbStatus.Text = MessageDlg("Payment Type Must Have Value")
                    ddlPayTypeDt2.Focus()
                    Return False
                End If
                If CFloat(tbRateDt2.Text) = 0 Then
                    lbStatus.Text = MessageDlg("Forex Rate Must Have Value")
                    tbRateDt2.Focus()
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
                If tbFgModeDt2.Text = "B" Or tbFgModeDt2.Text = "K" Then
                    If tbVoucherNo.Text.Trim = "" Then
                        lbStatus.Text = MessageDlg("Voucher No Must Have Value")
                        tbVoucherNo.Focus()
                        Return False
                    End If
                End If
                If CFloat(tbPaymentForexDt2.Text) = 0 Then
                    lbStatus.Text = MessageDlg("Payment Forex Must Have Value")
                    tbPaymentForexDt2.Focus()
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub LbAdvSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LbAdvSearch.Click
        Dim FDateName, FDateValue, FilterName, FilterValue As String
        Try
            FDateName = "Date"
            FDateValue = "TransDate"
            FilterName = "Reference, Date, Report, User, Supp Inv No., Attn, PO No, PPN No, PPN Date, PPN Rate, Currency, Rate, Base Forex, PPN, PPN Forex, Total Forex, Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), FgReport, User_Name, SuppInvNo, Attn, PONo, PPNNo, dbo.FormatDate(PPnDate), PPNRate, Currency, ForexRate, BaseForex, PPN, PPNForex, TotalForex, Remark"
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
                    ViewState("Reference") = GVR.Cells(2).Text
                    GridDt.PageIndex = 0
                    FillTextBoxHd(ViewState("Reference"))
                    BindDataDt(ViewState("Reference"))
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, pnlDt, GridDt)
                    btnHome.Visible = True
                ElseIf DDL.SelectedValue = "Edit" Then
                    CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                    If CekMenu <> "" Then
                        lbStatus.Text = CekMenu
                        Exit Sub
                    End If
                    If GVR.Cells(3).Text = "H" Or GVR.Cells(3).Text = "G" Then
                        MovePanel(PnlHd, pnlInput)
                        ViewState("Reference") = GVR.Cells(2).Text
                        GridDt.PageIndex = 0
                        BindDataDt(ViewState("Reference"))
                        FillTextBoxHd(ViewState("Reference"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        btnHome.Visible = False
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    Else
                        lbStatus.Text = MessageDlg("Data must Hold or Get Approval to edit")
                        Exit Sub
                    End If
                ElseIf DDL.SelectedValue = "Print" Then
                    Try
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If

                        Session("SelectCommand") = "EXEC S_FNFormVoucher '''" + GVR.Cells(2).Text + "''','DPSPY'," + QuotedStr(ViewState("UserId").ToString)
                        Session("ReportFile") = ".../../../Rpt/FormVoucher.frx"
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
                btnAdddt_Click(Nothing, Nothing)
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub

    Dim BaseForex As Decimal = 0

    ' untuk tampilkan data total di grid
    'Protected Sub GridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt.RowDataBound
    '    Try
    '        If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "ItemNo")) Then
    '            If e.Row.RowType = DataControlRowType.DataRow Then
    '                '' add the UnitPrice and QuantityTotal to the running total variables
    '                BaseForex += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "ChangeForex"))
    '            ElseIf e.Row.RowType = DataControlRowType.Footer Then
    '                tbBaseForex.Text = FormatNumber(BaseForex, ViewState("DigitCurr"))
    '                'AttachScript("BaseDiscPPnTotal(" + Me.tbBaseForex.ClientID + "," + tbDisc.ClientID + "," + tbDiscForex.ClientID + "," + Me.tbPPN.ClientID + "," + Me.tbPPNForex.ClientID + "," + Me.tbTotalForex.ClientID + "); setformat();", Page, Me.GetType())
    '            End If
    '        End If
    '    Catch ex As Exception
    '        lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Dim dr() As DataRow
        Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
        dr = ViewState("Dt").Select("ItemNo = " + GVR.Cells(1).Text)
        dr(0).Delete()
        totalingDt()
        BindGridDt(ViewState("Dt"), GridDt)
        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            FillTextBoxDt(GVR.Cells(1).Text)
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            'tambah 1 baris dari jimmy
            ChangePaymentTypeV2(ddlPayTypeDt2.SelectedValue, tbFgModeDt2, tbTransDate, tbDueDateDt2, ddlBankPaymentDt2, ddlCurrDt2, tbRateDt2, ViewState("Currency"), ViewState("DigitCurrDt2"), ViewState("DBConnection"))
            FillCombo(ddlBankPaymentDt2, "EXEC S_GetBankPaymentTrade " + QuotedStr(ddlCurrDt2.SelectedValue), True, "Bank_Code", "Bank_Name", ViewState("DBConnection"))
            'ChangePaymentType(ddlPayTypeDt2.SelectedValue, tbFgModeDt2, tbPaymentDateDt2, tbGiroDateDt2, ddlBankPaymentDt2, ddlCurrDt2, ddlChargeCurrDt2, tbRateDt2, tbChargeRateDt2, tbChargeForexDt2, ViewState("Currency2"), ViewState("DigitCurrDt2"), ViewState("DBConnection"), "Edit")
            ViewState("StateDt") = "Edit"
            ddlPayTypeDt2.Focus()
            StatusButtonSave(False)
            'If ddlPayTypeDt2.Items.Count = 0 Then            
            'End If
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
            BindToDate(tbTransDate, Dt.Rows(0)("TransDate").ToString)
            BindToDropList(ddlUserType, Dt.Rows(0)("User_Type").ToString)
            BindToText(tbSuppCode, Dt.Rows(0)("User_Code").ToString)
            BindToText(tbSuppName, Dt.Rows(0)("User_Name").ToString)
            BindToText(tbAttn, Dt.Rows(0)("Attn").ToString)
            BindToText(tbSuppInvNo, Dt.Rows(0)("SuppInvNo").ToString)
            BindToText(tbPONo, Dt.Rows(0)("PONo").ToString)
            BindToText(tbPOReport, Dt.Rows(0)("POReport").ToString)
            'BindToDropList(ddlReport, Dt.Rows(0)("FgReport").ToString)
            BindToDropList(ddlCurr, Dt.Rows(0)("Currency").ToString)
            BindToDropList(ddlCostCtr, Dt.Rows(0)("CostCtr").ToString)
            ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, ViewState("DBConnection").ToString)
            BindToText(tbRate, Dt.Rows(0)("ForexRate").ToString, ViewState("DigitRate"))
            BindToText(tbPPNNo, Dt.Rows(0)("PPnNo").ToString)
            BindToDate(tbPPNDate, Dt.Rows(0)("PPnDate").ToString)
            BindToText(tbPPNRate, Dt.Rows(0)("PPnRate").ToString, ViewState("DigitRate"))
            BindToText(tbBaseForex, Dt.Rows(0)("BaseForex").ToString, ViewState("DigitCurr"))
            BindToText(tbPPN, Dt.Rows(0)("PPn").ToString, ViewState("DigitPercent"))
            BindToText(tbPPNForex, Dt.Rows(0)("PPnForex").ToString, ViewState("DigitCurr"))
            BindToText(tbTotalForex, Dt.Rows(0)("TotalForex").ToString, ViewState("DigitCurr"))
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
            'FillCombo(ddlPayTypeDt2, "EXEC S_GetPayTypeUser " + QuotedStr(ddlReport.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString), True, "Payment_Code", "Payment_Name", ViewState("DBConnection"))
            FillCombo(ddlPayTypeDt2, "EXEC S_GetPayTypeUser 'PAYMENT|" + "Y" + "', " + QuotedStr(ViewState("UserId").ToString), True, "Payment_Code", "Payment_Name", ViewState("DBConnection")) '+ ddlReport.SelectedValue +
            FillCombo(ddlBankPaymentDt2, "EXEC S_GetBankPaymentTrade " + QuotedStr(ddlCurrDt2.SelectedValue), True, "Bank_Code", "Bank_Name", ViewState("DBConnection"))

            ddlCurr.Enabled = tbPONo.Text = ""
            'ddlReport.Enabled = tbPOReport.Text = "Y"
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("ItemNo = " + ItemNo)
            If Dr.Length > 0 Then
                lbItemNoDt2.Text = ItemNo
                BindToDropList(ddlPayTypeDt2, Dr(0)("PaymentType").ToString)
                tbPaymentDateDt2.SelectedDate = tbTransDate.SelectedDate
                'tbPaymentDateDt2.SelectedDate = Dr(0)("PaymentDate").ToString
                BindToText(tbDocumentNoDt2, Dr(0)("DocumentNo").ToString)
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
                BindToText(tbValue, Dr(0)("FgValue").ToString)
                'BindToDropList(ddlChargeCurrDt2, Dr(0)("ChargeCurrency").ToString)
                ViewState("DigitCurrDt2") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlCurrDt2.SelectedValue), ViewState("DBConnection"))
                'ViewState("DigitExpenseCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlChargeCurrDt2.SelectedValue), ViewState("DBConnection"))
                'BindToText(tbChargeRateDt2, Dr(0)("ChargeRate").ToString)
                'BindToText(tbChargeForexDt2, Dr(0)("ChargeForex").ToString)
                'BindToText(tbChargeHomeDt2, Dr(0)("ChargeHome").ToString)
                BindToText(tbDPForexDt2, Dr(0)("DPForex").ToString)
                BindToText(tbDPHomeDt2, Dr(0)("DPHome").ToString)

                'If ddlChargeCurrDt2.SelectedValue = "" Then
                '    tbChargeForexDt2.Text = "0"
                '    tbChargeHomeDt2.Text = "0"
                'End If
                'tbChargeForexDt2.Enabled = ddlChargeCurrDt2.SelectedValue <> ""
                tbRateDt2.Enabled = ddlCurrDt2.SelectedValue <> Session("Currency")
                ddlBankPaymentDt2.Enabled = tbFgModeDt2.Text = "G"
                tbDueDateDt2.Enabled = tbFgModeDt2.Text = "G"
                tbGiroDateDt2.Enabled = tbFgModeDt2.Text = "G"
                'ddlChargeCurrDt2.Enabled = tbFgModeDt2.Text = "B"
                'tbChargeForexDt2.Enabled = tbFgModeDt2.Text = "B"
                'tbChargeRateDt2.Enabled = ddlChargeCurrDt2.SelectedValue <> Session("Currency") And tbFgModeDt2.Text = "B"

                tbVoucherNo.Enabled = (tbFgModeDt2.Text = "B" Or tbFgModeDt2.Text = "K")
            End If
            'Dt = BindDataTransaction(GetStringDt(tbRef.Text), "ItemNo = " + ItemNo, ViewState("DBConnection").ToString)
            'newTrans()
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub

    Private Sub StatusButtonSave(ByVal Bool As Boolean)
        btnSaveAll.Visible = Bool
        btnSaveTrans.Visible = Bool
        btnBack.Visible = Bool
    End Sub

    Protected Sub ddlCurr_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCurr.SelectedIndexChanged
        Try
            If ViewState("InputCurrency") = "Y" Then
                RefreshMaster("S_GetCurrency", "Currency", "Currency", ddlCurr, ViewState("DBConnection"))
                ViewState("InputCurrency") = Nothing
            End If
            ChangeCurrency(ddlCurr, tbTransDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"), ViewState("DigitHome"), ViewState("DigitRate"))
            ChangeReport("Edit", "Y", ddlCurr.SelectedValue = ViewState("Currency"), tbTransDate, tbRate, tbPPNNo, tbPPNDate, tbPPNRate) 'ddlReport.SelectedValue
            tbRate.Focus()
            AttachScript("setformat();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "ddl Curr ERror : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbSuppCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbSuppCode.TextChanged
        'Dim Dt As DataTable
        Dim Dr As DataRow
        Try
            'Dt = SQLExecuteQuery("SELECT User_Code, User_Name, Currency, Term FROM VMsUserType WHERE User_Type = '" + ddlUserType.SelectedValue + "' AND User_Code = '" + tbSuppCode.Text + "'", ViewState("DBConnection").ToString).Tables(0)
            Dr = FindMaster("UserType", tbSuppCode.Text + "|" + ddlUserType.SelectedValue, ViewState("DBConnection").ToString)
            'If Dt.Rows.Count > 0 Then
            If Not Dr Is Nothing Then
                'Dr = Dt.Rows(0)
                tbSuppCode.Text = Dr("User_Code")
                tbSuppName.Text = Dr("User_Name")
                tbAttn.Text = Dr("Contact_Person")
                ddlCurr.SelectedValue = Dr("Currency")
            Else
                tbSuppCode.Text = ""
                tbSuppName.Text = ""
            End If
            AttachScript("setformatdt();", Page, Me.GetType())
            tbAttn.Focus()
        Catch ex As Exception
            Throw New Exception("tb ProductCode Error : " + ex.ToString)
        End Try
    End Sub


    Protected Sub lbCurr_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCurr.Click
        Try
            ViewState("InputCurrency") = "Y"
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsCurrency');", Page, Me.GetType)
        Catch ex As Exception
            lbStatus.Text = "lb Currency Error : " + ex.ToString
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
            
            If tbFgModeDt2.Text = "E" Then
                tbValue.Text = -1
            Else : tbValue.Text = 1
            End If
            
            ChangePaymentTypeV2(ddlPayTypeDt2.SelectedValue, tbFgModeDt2, tbTransDate, tbDueDateDt2, ddlBankPaymentDt2, ddlCurrDt2, tbRateDt2, ViewState("Currency"), ViewState("DigitCurrDt2"), ViewState("DBConnection"))
            FillCombo(ddlBankPaymentDt2, "EXEC S_GetBankPaymentTrade " + QuotedStr(ddlCurrDt2.SelectedValue), True, "Bank_Code", "Bank_Name", ViewState("DBConnection"))
            tbGiroDateDt2.SelectedValue = tbDueDateDt2.SelectedValue
            tbGiroDateDt2.Enabled = tbDueDateDt2.Enabled
            'ChangePaymentType(ddlPayTypeDt2.SelectedValue, tbFgModeDt2, tbPaymentDateDt2, tbGiroDateDt2, ddlBankPaymentDt2, ddlCurrDt2, ddlChargeCurrDt2, tbRateDt2, tbChargeRateDt2, tbChargeForexDt2, ViewState("Currency2"), ViewState("DigitCurrDt2"), ViewState("DBConnection"))
            ChangeCurrency(ddlCurrDt2, tbTransDate, tbRateDt2, ViewState("Currency"), ViewState("DigitCurrDt2"), ViewState("DBConnection"), ViewState("DigitHome"), ViewState("DigitRate"))
            'ViewState("DigitCurrDt2") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlCurrDt2.SelectedValue), ViewState("DBConnection"))
            'ViewState("DigitExpenseCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlChargeCurrDt2.SelectedValue), ViewState("DBConnection"))

            tbPaymentForexDt2.Text = FormatFloat((CFloat(tbDPHomeDt2.Text) / CFloat(tbRateDt2.Text)), ViewState("DigitCurrDt2"))
            tbPaymentHomeDt2.Text = tbDPHomeDt2.Text

            tbDPForexDt2.Text = FormatFloat((CFloat(tbDPHomeDt2.Text) / CFloat(tbRateDt2.Text) * tbValue.Text), ViewState("DigitCurrDt2"))
            tbDPHomeDt2.Text = tbDPForexDt2.Text

            'AttachScript("kali(" + Me.tbRateDt2.ClientID + "," + Me.tbPaymentForexDt2.ClientID + "," + Me.tbPaymentHomeDt2.ClientID + "); kali(" + Me.tbChargeRateDt2.ClientID + "," + Me.tbChargeForexDt2.ClientID + "," + Me.tbChargeHomeDt2.ClientID + "); setformatdt2();", Page, Me.GetType())
            VoucherNo = ""
            If tbFgModeDt2.Text = "B" Or tbFgModeDt2.Text = "K" Then
                VoucherNo = SQLExecuteScalar("Declare @A VarChar(255) EXEC S_SAAutoVoucherNmbr " + QuotedStr(Format(tbTransDate.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr("Y") + ", " + QuotedStr(ddlPayTypeDt2.SelectedValue) + ", 'OUT', @A OUT SELECT @A", ViewState("DBConnection").ToString) 'ddlReport.SelectedValue
            End If
            tbVoucherNo.Enabled = (tbFgModeDt2.Text = "B" Or tbFgModeDt2.Text = "K")
            tbVoucherNo.Text = VoucherNo
            btnSaveDt.Focus()
        Catch ex As Exception
            lbStatus.Text = "ddl Pay Type Select Index Changed Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub ddlChargeCurrDt2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlChargeCurrDt2.SelectedIndexChanged
    '    Try
    '        ChangeCurrency(ddlChargeCurrDt2, tbPaymentDateDt2, tbChargeRateDt2, ViewState("Currency"), ViewState("DigitExpenseCurr"), ViewState("DBConnection"), ViewState("DigitHome"), ViewState("DigitRate"))
    '        If ddlChargeCurrDt2.SelectedValue = "" Then
    '            tbChargeForexDt2.Text = "0"
    '            tbChargeHomeDt2.Text = "0"
    '        End If
    '        tbChargeForexDt2.Enabled = ddlChargeCurrDt2.SelectedValue <> ""
    '        tbChargeRateDt2.Focus()
    '        '            AttachScript("setformatDt();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lbStatus.Text = "ddl curr expense selected index changed Error : " + ex.ToString
    '    End Try
    'End Sub

    Dim TotalPayment As Decimal = 0
    Dim TotalCharge As Decimal = 0
    Dim TotalForex As Decimal = 0
    Dim TotalBase As Decimal = 0
    Private Sub totalingDt()
        Dim TotalPayment As Decimal = 0
        Dim TotalDP As Decimal = 0
        Dim TotalCharge As Decimal = 0
        Dim TotalForex As Decimal = 0
        Dim BaseForex As Decimal = 0

        Dim dr As DataRow
        Try
            For Each dr In ViewState("Dt").Rows
                If Not dr.RowState = DataRowState.Deleted Then
                    TotalPayment = TotalPayment + CFloat(dr("PaymentHome").ToString)
                    TotalCharge = TotalCharge + CFloat(dr("ChargeHome").ToString)
                    TotalDP = TotalDP + CFloat(dr("DPHome").ToString)
                End If
            Next
            TotalForex = TotalDP / CFloat(tbRate.Text)
            BaseForex = (TotalForex / (1 + CFloat(tbPPN.Text) / 100))
            tbBaseForex.Text = CStr(BaseForex)
            AttachScript("setformat();", Page, Me.GetType())
        Catch ex As Exception
            Throw New Exception("Totaling Dt Error : " + ex.ToString)
        End Try
    End Sub

    'Protected Sub ddlReport_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlReport.SelectedIndexChanged
    '    ChangeReport("Edit", ddlReport.SelectedValue, ddlCurr.SelectedValue = ViewState("Currency"), tbTransDate, tbRate, tbPPNNo, tbPPNDate, tbPPNRate)
    '    'FillCombo(ddlPayTypeDt2, "EXEC S_GetPayTypeUser " + QuotedStr(ddlReport.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString), True, "Payment_Code", "Payment_Name", ViewState("DBConnection"))
    '    FillCombo(ddlPayTypeDt2, "EXEC S_GetPayTypeUser 'PAYMENT|" + ddlReport.SelectedValue + "', " + QuotedStr(ViewState("UserId").ToString), True, "Payment_Code", "Payment_Name", ViewState("DBConnection"))
    '    FillCombo(ddlBankPaymentDt2, "EXEC S_GetBankPayment " + QuotedStr(ddlReport.SelectedValue), True, "Bank_Code", "Bank_Name", ViewState("DBConnection"))
    '    'tbPONo.Text = ""
    '    If ddlReport.SelectedValue = "N" Then
    '        tbPPN.Enabled = False
    '        tbPPN.Text = FormatFloat("0", ViewState("DigitPercent"))
    '    Else
    '        tbPPN.Enabled = True
    '    End If
    'End Sub

    Protected Sub tbRate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbRate.TextChanged
        'If ddlReport.SelectedValue = "Y" Then
        tbPPNRate.Text = tbRate.Text
        'End If
    End Sub

    'Protected Sub tbChargeForexDt2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbChargeForexDt2.TextChanged
    '    'tbChargeForexDt2.Attributes.Add("OnBlur", "kali(" + Me.tbChargeRateDt2.ClientID + "," + Me.tbChargeForexDt2.ClientID + "," + Me.tbChargeHomeDt2.ClientID + "); setformatdt2();")
    '    'tbDPForexDt2.Text = FormatNumber((CFloat(tbPaymentHomeDt2.Text) - CFloat(tbChargeHomeDt2.Text)) / CFloat(tbRateDt2.Text), ViewState("DigitQty"))
    '    'tbDPHomeDt2.Text = FormatNumber((CFloat(tbPaymentHomeDt2.Text) - CFloat(tbChargeHomeDt2.Text)), ViewState("DigitQty"))

    '    'ubahtbDPForexDt2.Text = FormatNumber((CFloat(tbPaymentHomeDt2.Text) - CFloat(tbChargeHomeDt2.Text)) / CFloat(tbRate.Text), ViewState("DigitHome"))
    '    'ubahtbDPHomeDt2.Text = FormatNumber((CFloat(tbPaymentHomeDt2.Text) - CFloat(tbChargeHomeDt2.Text)), ViewState("DigitHome"))
    '    tbDPForexDt2.Text = FormatNumber((CFloat(tbPaymentHomeDt2.Text)) / CFloat(tbRate.Text), ViewState("DigitHome"))
    '    tbDPHomeDt2.Text = FormatNumber((CFloat(tbPaymentHomeDt2.Text)), ViewState("DigitHome"))

    '    'ChangeCurrency(ddlCurrDt2, tbTransDate, tbChargeRateDt2, ViewState("Currency"), ViewState("DigitCurrDt2"), ViewState("DBConnection"))
    '    totalingDt()
    '    'AttachScript("setformatDt();", Page, Me.GetType())
    'End Sub

    Protected Sub tbPaymentForexDt2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPaymentForexDt2.TextChanged
        'tbDPForexDt2.Text = FormatNumber((CFloat(tbPaymentHomeDt2.Text) - CFloat(tbChargeHomeDt2.Text)) / CFloat(tbRate.Text), ViewState("DigitHome"))
        'tbDPHomeDt2.Text = FormatNumber((CFloat(tbPaymentHomeDt2.Text) - CFloat(tbChargeHomeDt2.Text)), ViewState("DigitHome"))

        tbDPForexDt2.Text = FormatNumber((CFloat(tbPaymentHomeDt2.Text) / CFloat(tbRate.Text) * tbValue.Text), ViewState("DigitHome"))
        tbDPHomeDt2.Text = FormatNumber((CFloat(tbPaymentHomeDt2.Text) * tbValue.Text), ViewState("DigitHome"))
        totalingDt()
        tbRemarkDt2.Focus()
        'AttachScript("setformatDt();", Page, Me.GetType())
    End Sub

    Protected Sub ddlUserType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlUserType.SelectedIndexChanged
        btnPO.Enabled = ddlUserType.SelectedValue = "Supplier"
        tbSuppCode.Text = ""
        tbSuppName.Text = ""
        tbPONo.Text = ""
        tbPOReport.Text = "N"
        tbPPN.Enabled = ddlUserType.SelectedValue = "Supplier"
        tbPPN.Text = 0
        'ddlReport.Enabled = tbPOReport.Text = "Y"
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            GridView1.PageIndex = 0
            BindData(Session("AdvanceFilter"))
            pnlNav.Visible = True
            'ddlCommand.Visible = True
            'BtnGo.Visible = True
        Catch ex As Exception
            lbStatus.Text = "Btn Search Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, pnlDt, GridDt)
            newTrans()
            btnHome.Visible = False
            tbTransDate.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnExpand_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExpand.Click
        Try
            tbFilter2.Text = ""
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
        'Dim CurrFilter, Value As String
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            'If IsNothing(ViewState("Dt")) Then
            '    lbStatus.Text = MessageDlg("Detail must have at least 1 record")
            '    Exit Sub
            'End If

            'If GetCountRecord(ViewState("Dt")) = 0 Then
            If ViewState("PayType").ToString = "" Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
                Exit Sub
            End If
            For Each dr In ViewState("Dt").Rows
                If CekDt(dr) = False Then
                    Exit Sub
                End If
            Next
            SaveAll()
            'MovePanel(pnlInput, PnlHd)
            'CurrFilter = tbFilter.Text
            'Value = ddlField.SelectedValue
            'tbFilter.Text = tbTransNo.Text
            'ddlField.SelectedValue = "TransNmbr"
            'btnSearch_Click(Nothing, Nothing)
            'tbFilter.Text = CurrFilter
            'ddlField.SelectedValue = Value
            BtnAdd_Click(Nothing, Nothing)
        Catch ex As Exception
            lbStatus.Text = "Save All Error : " + ex.ToString
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

            'If GetCountRecord(ViewState("Dt")) = 0 Then
            If ViewState("PayType").ToString = "" Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
                Exit Sub
            End If
            For Each dr In ViewState("Dt").Rows
                If CekDt(dr) = False Then
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

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnHome.Click
        Try
            MovePanel(pnlInput, PnlHd)
        Catch ex As Exception
            lbStatus.Text = "btn back Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAdddt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdddt.Click, btnAddDt2.Click
        Cleardt()
        If CekHd() = False Then
            Exit Sub
        End If
        ViewState("StateDt") = "Insert"
        lbItemNoDt2.Text = GetNewItemNo(ViewState("Dt"))
        MovePanel(pnlDt, pnlEditDt)
        EnableHd(False)
        StatusButtonSave(False)
        'If Session("PeriodInfo")("1Payment").ToString = "Y" Then
        BindToDropList(ddlPayTypeDt2, ViewState("PayType").ToString.Trim)
        'End If
        If Not ViewState("DPForex") Is Nothing And Not ViewState("PONo") Is Nothing Then
            If ViewState("PONo") = tbPONo.Text Then
                ddlCurrDt2.SelectedValue = ddlCurr.SelectedValue
                tbRateDt2.Text = tbRate.Text
                tbPaymentForexDt2.Text = FormatFloat(CFloat(ViewState("DPForex").ToString) * (100 + CFloat(tbPPN.Text)) / 100, ViewState("DigitCurrDt2"))
                tbPaymentHomeDt2.Text = FormatFloat(CFloat(ViewState("DPForex").ToString) * ((100 + CFloat(tbPPN.Text)) / 100) * CFloat(tbRate.Text), ViewState("DigitHome"))
                tbDPForexDt2.Text = FormatFloat(CFloat(ViewState("DPForex").ToString) * (100 + CFloat(tbPPN.Text)) / 100, ViewState("DigitCurrDt2"))
                tbDPHomeDt2.Text = FormatFloat(CFloat(ViewState("DPForex").ToString) * CFloat(tbRate.Text) * (100 + CFloat(tbPPN.Text)) / 100, ViewState("DigitHome"))
            End If
        End If
        ddlPayTypeDt2.Focus()
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
                
                Session("SelectCommand") = "EXEC S_FNFormVoucher " + Result + ",'DPSPY'," + QuotedStr(ViewState("UserId").ToString)
                Session("ReportFile") = ".../../../Rpt/FormVoucher.frx"
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
                        Result = ExecSPCommandGo(ActionValue, "S_FNDPSuppPay", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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

    Protected Sub btnCancelDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelDt.Click
        Try
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            'EnableHd(GridDt.Rows.Count = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt.Click
        Try
            'If Session("PeriodInfo")("1Payment").ToString = "Y" Then
            'If GetCountRecord(ViewState("Dt")) >= 1 Then
            '    If tbFgModeDt2.Text = "K" Or tbFgModeDt2.Text = "B" Or tbFgModeDt2.Text = "G" Or tbFgModeDt2.Text = "D" Then
            '        If ViewState("StateDt") <> "Edit" Then
            '            If ddlPayTypeDt2.SelectedValue <> ViewState("PayType").ToString Then
            '                lbStatus.Text = "Cannot input more than one payment type"
            '                ddlPayTypeDt2.Focus()
            '                Exit Sub
            '            End If
            '        End If
            '    End If
            'End If
            'End If
            If tbFgModeDt2.Text = "G" Then
                If CekExistGiroOut(tbDocumentNoDt2.Text.Trim, ViewState("DBConnection").ToString) = True Then
                    lbStatus.Text = "Giro Payment '" + tbDocumentNoDt2.Text.Trim + "' has already exists in Giro Listing'"
                    Exit Sub
                End If
            End If
            If ViewState("StateDt") = "Edit" Then
                If CekDt() = False Then
                    Exit Sub
                End If
                Dim Row As DataRow
                Row = ViewState("Dt").Select("ItemNo = " + lbItemNoDt2.Text)(0)
                Row.BeginEdit()
                Row("PaymentType") = ddlPayTypeDt2.SelectedValue
                Row("PaymentTypeName") = ddlPayTypeDt2.SelectedItem.Text
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
                If ddlBankPaymentDt2.SelectedIndex = 0 Then
                    Row("BankPayment") = ""
                    Row("BankPaymentName") = ""
                Else
                    Row("BankPayment") = ddlBankPaymentDt2.SelectedValue
                    Row("BankPaymentName") = ddlBankPaymentDt2.SelectedItem.Text
                End If

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
                Row("DPHome") = tbDPHomeDt2.Text 'CFloat(tbPaymentHomeDt2.Text) - CFloat(tbChargeHomeDt2.Text)
                Row("DPForex") = tbDPForexDt2.Text '(CFloat(tbPaymentHomeDt2.Text) - CFloat(tbChargeHomeDt2.Text)) / CFloat(tbRate.Text)
                Row("FgValue") = tbValue.Text
                Row.EndEdit()
                'ViewState("Dt").AcceptChanges()
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If
                If CekExistData(ViewState("Dt"), "ItemNo", lbItemNoDt2.Text) Then
                    lbStatus.Text = "Item No. '" + lbItemNoDt2.Text + "' has already exists"
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("ItemNo") = CInt(lbItemNoDt2.Text)
                dr("PaymentType") = ddlPayTypeDt2.SelectedValue
                dr("PaymentTypeName") = ddlPayTypeDt2.SelectedItem.Text
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
                If ddlBankPaymentDt2.SelectedIndex = 0 Then
                    dr("BankPayment") = ""
                    dr("BankPaymentName") = ""
                Else
                    dr("BankPayment") = ddlBankPaymentDt2.SelectedValue
                    dr("BankPaymentName") = ddlBankPaymentDt2.SelectedItem.Text
                End If
                'dr("BankPayment") = ddlBankPaymentDt2.SelectedValue
                'dr("BankPaymentName") = ddlBankPaymentDt2.SelectedItem.Text
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
                dr("DPForex") = tbDPForexDt2.Text
                dr("DPHome") = tbDPHomeDt2.Text
                dr("Remark") = tbRemarkDt2.Text
                dr("DPHome") = tbDPHomeDt2.Text 'CFloat(tbPaymentHomeDt2.Text) - CFloat(tbChargeHomeDt2.Text)
                dr("DPForex") = tbDPForexDt2.Text '(CFloat(tbPaymentHomeDt2.Text) - CFloat(tbChargeHomeDt2.Text)) / CFloat(tbRate.Text)
                dr("FgValue") = tbValue.Text
                ViewState("Dt").Rows.Add(dr)
            End If
            If tbFgModeDt2.Text = "K" Or tbFgModeDt2.Text = "B" Or tbFgModeDt2.Text = "G" Or tbFgModeDt2.Text = "D" Then
                ViewState("PayType") = ddlPayTypeDt2.SelectedValue
            End If
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            totalingDt()
            BindGridDt(ViewState("Dt"), GridDt)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save Dt Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub

    Protected Sub btnSupp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSupp.Click
        Dim ResultField As String
        Try
            Session("filter") = "select User_Code, User_Name, Contact_Person, Currency, Term FROM VMsUserType where User_Type = " + QuotedStr(ddlUserType.SelectedValue)
            ResultField = "User_Code, User_Name, Currency, Contact_Person"
            ViewState("Sender") = "btnSupp"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType)
        Catch ex As Exception
            lbStatus.Text = "btn Search supp Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnPO_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPO.Click
        Dim ResultField As String
        Try
            If tbSuppCode.Text = "" Then
                Session("filter") = "SELECT Reff_No, Reff_Date, Currency, Base_Forex, PPN, Total_Forex, DP_Forex, Supplier_Code, Supplier_Name, FgReport, Term_Payment, CostCtr, Remark FROM v_fndpSuppgetPO "
            Else
                Session("filter") = "SELECT Reff_No, Reff_Date, Currency, Base_Forex, PPN, Total_Forex, DP_Forex, Supplier_Code, Supplier_Name, FgReport, Term_Payment, CostCtr, Remark FROM v_fndpSuppgetPO WHERE Supplier_Code = " + QuotedStr(tbSuppCode.Text)
            End If
            ResultField = "Reff_No, Currency, Base_Forex, PPN, Total_Forex, DP_Forex, Supplier_Code, Supplier_Name, FgReport, Term_Payment, CostCtr"
            ViewState("Sender") = "btnPO"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType)
        Catch ex As Exception
            lbStatus.Text = "btn Search PO Error : " + ex.ToString
        End Try
    End Sub
End Class
