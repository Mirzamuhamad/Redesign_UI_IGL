Imports System.Data
Imports System.Data.SqlClient
Imports BasicFrame.WebControls
Partial Class TrReceiptTrade
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select distinct TransNmbr, Nmbr, TransDate, Status, FgReport, CustomerCode, CustomerName, Customer, Attn, Currency, TotalReceiptForex, TotalReceiptForexStr, TotalReceipt, TotalInvoice, TotalCharge, TotalOthers, TotalDP, TotalKurs, TotalSelisih, Remark From V_FNReceiptTradeHd "

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
                lbCount.Text = SQLExecuteScalar("SELECT COUNT(Invoice_No) FROM V_FNReceiptTradeGetAR", ViewState("DBConnection").ToString)

            End If
            lbStatus.Text = ""
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnCustomer" Then
                    tbCustCode.Text = Session("Result")(0).ToString
                    tbCustName.Text = Session("Result")(1).ToString
                    BindToText(tbAttn, Session("Result")(2).ToString)
                ElseIf ViewState("Sender") = "btnDocNo" Then
                    tbDocumentNo.Text = Session("Result")(0).ToString
                    ddlCurr.SelectedValue = Session("Result")(1).ToString
                    tbRate.Text = Session("Result")(2).ToString
                    tbReceiptForex.Text = (CFloat(Session("Result")(3).ToString) - CFloat(Session("Result")(4).ToString)).ToString
                    tbRate.Enabled = False
                    If tbCustCode.Text.Trim = "" Then
                        BindToText(tbCustCode, Session("Result")(5).ToString)
                        BindToText(tbCustName, Session("Result")(6).ToString)
                    End If
                    AttachScript("setformatdt()", Page, Me.GetType)
                ElseIf ViewState("Sender") = "btnInvNo" Then
                    'ResultField = "Invoice_No, Currency, Forex_Rate, Amount, Amount_PPN, Balance, Balance_PPN, PPN_Rate, Due_Date"
                    'ResultField = "Invoice_No, Currency, Forex_Rate, Base_Forex, PPN_Forex, Base_Paid, PPN_Paid, PPN_Rate, Due_Date, FgValue, Buyer_Code, Buyer_Name, Bill_To, Bill_To_Name"
                    Dim amount, amountppn, balans, balansppn As Double
                    Dim PayRate, rate, sisainv, sisabayar, sisappn As Double
                    amount = CFloat(Session("Result")(3).ToString)
                    amountppn = CFloat(Session("Result")(4).ToString)
                    balans = CFloat(Session("Result")(5).ToString)
                    balansppn = CFloat(Session("Result")(6).ToString)
                    rate = CFloat(Session("Result")(2).ToString)

                    BindToText(tbInvNoDt2, Session("Result")(0).ToString)
                    BindToDropList(ddlCurrDt2, Session("Result")(1).ToString)
                    ViewState("DigitCurrInv") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlCurrDt2.SelectedValue), ViewState("DBConnection"))
                    BindToText(tbRateDt2, Session("Result")(2).ToString)
                    If CFloat(Session("Result")(7).ToString) = 0 Then 'ppn rate
                        BindToText(tbPPNRateDt2, Session("Result")(2).ToString)
                    Else
                        BindToText(tbPPNRateDt2, Session("Result")(7).ToString)
                    End If
                    BindToDate(tbDueDateDt2, Session("Result")(8).ToString)
                    BindToText(tbFgValueDt2, Session("Result")(9).ToString)
                    BindToText(tbBuyerCode, Session("Result")(10).ToString)
                    BindToText(tbBuyer, Session("Result")(11).ToString)
                    If tbCustCode.Text.Trim = "" Then
                        BindToText(tbCustCode, Session("Result")(12).ToString)
                        BindToText(tbCustName, Session("Result")(13).ToString)
                        tbCustCode.Enabled = False
                        btnSupp.Visible = False
                    End If
                    
                    If (lblCurrPayDt.Text = ViewState("Currency").ToString) Then
                        If (ddlCurrDt2.SelectedValue = ViewState("Currency").ToString) Then
                            tbPayRateDt2.Text = FormatFloat("1", ViewState("DigitRate"))
                            tbPayRateDt2.Enabled = False
                        Else
                            tbPayRateDt2.Enabled = True
                            Dim Dr As DataRow
                            Dr = FindMaster("Rate", ddlCurrDt2.SelectedValue + "|" + Format(tbDate.SelectedDate, "yyyy-MM-dd"), ViewState("DBConnection").ToString)
                            If Not Dr Is Nothing Then
                                tbPayRateDt2.Text = FormatFloat(Dr("Rate").ToString, ViewState("DigitRate"))
                            Else
                                tbPayRateDt2.Text = FormatFloat(tbRateDt2.Text, ViewState("DigitRate"))
                            End If
                        End If
                    Else
                        If (ddlCurrDt2.SelectedValue = ViewState("Currency").ToString) Then
                            tbPayRateDt2.Text = FormatFloat("1", ViewState("DigitRate"))
                            tbPayRateDt2.Enabled = False
                        Else
                            If (ddlCurrDt2.SelectedValue = lblCurrPayDt.Text) Then
                                tbPayRateDt2.Text = FormatFloat(lblRatePayDt.Text.Replace(",", ""), ViewState("DigitRate"))
                                tbPayRateDt2.Enabled = False
                            Else
                                'cari rate payment
                                tbPayRateDt2.Enabled = True
                                Dim Dr As DataRow
                                Dr = FindMaster("Rate", ddlCurrDt2.SelectedValue + "|" + Format(tbDate.SelectedDate, "yyyy-MM-dd"), ViewState("DBConnection").ToString)
                                If Not Dr Is Nothing Then
                                    tbPayRateDt2.Text = FormatFloat(Dr("Rate").ToString, ViewState("DigitRate"))
                                Else
                                    tbPayRateDt2.Text = FormatFloat(tbRateDt2.Text.Replace(",", ""), ViewState("DigitRate"))
                                End If
                            End If
                        End If
                    End If
                    If (lblCurrPayDt.Text = ViewState("Currency").ToString) Then
                        tbPayRatePPn.Text = tbPPNRateDt2.Text
                    Else
                        tbPayRatePPn.Text = tbPayRateDt2.Text
                    End If

                    PayRate = CFloat(tbPayRateDt2.Text.Replace(",", ""))

                    tbTotalInvoiceDt2.Text = (amount + amountppn).ToString
                    TbTotalPaidDt2.Text = (balans + balansppn).ToString
                    'tbTotalToPaidDt2.Text = ((amount - balans) + (amountppn - balansppn)).ToString
                    tbBaseInvoiceDt2.Text = amount.ToString
                    tbBasePaidDt2.Text = balans.ToString
                    tbBaseToPaidDt2.Text = (amount - balans).ToString
                    tbPPNInvoiceDt2.Text = amountppn.ToString
                    tbPPNPaidDt2.Text = balansppn.ToString
                    tbPPNToPaidDt2.Text = (amountppn - balansppn).ToString

                    sisainv = ((amount - balans) + (amountppn - balansppn)) * rate
                    'sisabayar = (CFloat(tbPayHomeDt.Text) + CFloat(tbChargeHomeDt.Text) - CFloat(tbInvHomeDt.Text))
                    sisabayar = (CFloat(tbPayHomeDt.Text) - CFloat(tbInvHomeDt.Text))
                    sisappn = (amountppn - balansppn) * PayRate '* rate

                    'If sisainv > sisabayar Then
                    '    tbTotalToPaidDt2.Text = (sisabayar / rate).ToString
                    '    tbBaseToPaidDt2.Text = "0"
                    '    If amountppn - balansppn > 0 Then
                    '        tbPPNToPaidDt2.Text = (amountppn - balansppn).ToString                        
                    '    End If                        
                    'Else : tbTotalToPaidDt2.Text = (sisainv / rate).ToString
                    'End If
                    If sisabayar < sisainv Then
                        tbTotalToPaidDt2.Text = (sisabayar / rate).ToString
                    Else
                        tbTotalToPaidDt2.Text = (sisainv / rate).ToString
                    End If

                    sisabayar = CFloat(tbTotalToPaidDt2.Text) * rate
                    If sisabayar < sisappn Then
                        tbPPNToPaidDt2.Text = (sisabayar / rate).ToString
                        tbBaseToPaidDt2.Text = "0"
                    Else
                        tbPPNToPaidDt2.Text = (sisappn / rate).ToString
                        tbBaseToPaidDt2.Text = ((sisabayar - sisappn) / rate).ToString
                    End If

                    AttachScript("setformatdt2('');", Page, Me.GetType())
                ElseIf ViewState("Sender") = "btnGetInv" Then
                    Dim drResult As DataRow
                    Dim ExistRow As DataRow()

                    For Each drResult In Session("Result").Rows

                        ExistRow = ViewState("Dt2").Select(" InvoiceNo = " + QuotedStr(drResult("Invoice_No").ToString))
                        If ExistRow.Count = 0 Then
                            'insert
                            If tbCustCode.Text.Trim = "" Then
                                BindToText(tbCustCode, drResult("Bill_To").ToString)
                                BindToText(tbCustName, drResult("Bill_To_Name").ToString)
                            End If
                            Dim dr As DataRow
                            dr = ViewState("Dt2").NewRow
                            dr("ItemNo") = GetNewItemNo(ViewState("Dt2"))
                            lbItemNo.Text = GetNewItemNo(ViewState("Dt2"))
                            dr("InvoiceNo") = drResult("Invoice_No").ToString
                            dr("Buyer") = drResult("Buyer_Code").ToString
                            dr("Buyer_Name") = drResult("Buyer_Name").ToString
                            dr("DueDate") = drResult("Due_Date").ToString
                            dr("Due_Date") = drResult("Due_Date").ToString
                            dr("Currency") = drResult("Currency").ToString
                            ViewState("DigitCurrInv") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(drResult("Currency").ToString), ViewState("DBConnection"))
                            dr("ForexRate") = drResult("Forex_Rate").ToString
                            If CFloat(drResult("PPn_Rate").ToString) = 0 Then
                                dr("PPNRate") = drResult("Forex_Rate").ToString
                                ViewState("PPNRateDtInv") = drResult("Forex_Rate").ToString
                            Else
                                dr("PPNRate") = drResult("PPn_Rate").ToString
                                ViewState("PPNRateDtInv") = drResult("PPn_Rate").ToString
                            End If
                            If drResult("Currency").ToString = lblCurrPayDt.Text Then
                                dr("PayRate") = lblRatePayDt.Text
                                ViewState("PayRateDtInv") = lblRatePayDt.Text
                            ElseIf drResult("Currency").ToString = ViewState("Currency") Then
                                dr("PayRate") = "1"
                                ViewState("PayRateDtInv") = "1"
                            ElseIf drResult("Currency").ToString <> ViewState("Currency") Then
                                Dim DrRate As DataRow
                                DrRate = FindMaster("Rate", drResult("Currency").ToString + "|" + Format(tbDate.SelectedDate, "yyyy-MM-dd"), ViewState("DBConnection").ToString)
                                If Not DrRate Is Nothing Then
                                    dr("PayRate") = DrRate("Rate").ToString
                                    ViewState("PayRateDtInv") = DrRate("Rate").ToString
                                Else
                                    dr("PayRate") = "0"
                                    ViewState("PayRateDtInv") = "0"
                                End If                                
                            End If
                            If (lblCurrPayDt.Text = ViewState("Currency").ToString) Then
                                dr("PayRatePPn") = ViewState("PPNRateDtInv")
                            Else
                                dr("PayRatePPn") = ViewState("PayRateDtInv")
                            End If
                            dr("FgValue") = drResult("FgValue").ToString
                            dr("TotalInvoice") = drResult("Total_Forex").ToString
                            dr("TotalPaid") = drResult("Total_Paid").ToString
                            dr("TotalToPaid") = drResult("Amount_Saldo").ToString
                            dr("BaseInvoice") = drResult("Base_Forex").ToString
                            dr("BasePaid") = drResult("Base_Paid").ToString
                            dr("BaseToPaid") = FormatFloat(drResult("Base_Forex") - drResult("Base_Paid"), drResult("DigitCurrForex")) '(drResult("Base_Forex") - drResult("Base_Paid")) 
                            dr("PPNInvoice") = drResult("PPn_Forex").ToString
                            dr("PPNPaid") = drResult("PPN_Paid").ToString
                            dr("PPNToPaid") = FormatFloat(drResult("PPn_Forex") - drResult("PPN_Paid"), drResult("DigitCurrForex"))
                            dr("Remark") = ""
                            ViewState("Dt2").Rows.Add(dr)
                        End If
                    Next
                    CountTotalInvoiceHome()
                    BindGridDt(ViewState("Dt2"), GridDt2)
                    EnableHd(GetCountRecord(ViewState("Dt2")) = 0)
                End If

                If ViewState("Sender") = "btnOut" Then
                    BtnAdd_Click(Nothing, Nothing)
                    Dim drResult As DataRow
                    Dim ExistRow As DataRow()
                    Dim FirstTime As Boolean = True
                    For Each drResult In Session("Result").Rows
                        ExistRow = ViewState("Dt2").Select(" InvoiceNo = " + QuotedStr(drResult("Invoice_No").ToString))
                        'ExistRow = ViewState("Dt2").Select("InvoiceNo = " + QuotedStr(drResult("Invoice_No").ToString))
                        If ExistRow.Count = 0 Then
                            'insert
                            If tbCustCode.Text.Trim = "" Then
                                BindToText(tbCustCode, drResult("Bill_To").ToString)
                                BindToText(tbCustName, drResult("Bill_To_Name").ToString)
                            End If
                            Dim dr As DataRow
                            dr = ViewState("Dt2").NewRow
                            dr("ItemNo") = GetNewItemNo(ViewState("Dt2"))
                            lbItemNo.Text = GetNewItemNo(ViewState("Dt2"))
                            dr("InvoiceNo") = drResult("Invoice_No").ToString
                            dr("Buyer") = drResult("Buyer_Code").ToString
                            dr("Buyer_Name") = drResult("Buyer_Name").ToString
                            dr("DueDate") = drResult("Due_Date").ToString
                            dr("Due_Date") = drResult("Due_Date").ToString
                            dr("Currency") = drResult("Currency").ToString
                            ViewState("DigitCurrInv") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(drResult("Currency").ToString), ViewState("DBConnection"))
                            dr("ForexRate") = drResult("Forex_Rate").ToString
                            If CFloat(drResult("PPn_Rate").ToString) = 0 Then
                                dr("PPNRate") = drResult("Forex_Rate").ToString
                                ViewState("PPNRateDtInv") = drResult("Forex_Rate").ToString
                            Else
                                dr("PPNRate") = drResult("PPn_Rate").ToString
                                ViewState("PPNRateDtInv") = drResult("PPn_Rate").ToString
                            End If
                            If drResult("Currency").ToString = lblCurrPayDt.Text Then
                                dr("PayRate") = lblRatePayDt.Text
                                ViewState("PayRateDtInv") = lblRatePayDt.Text
                            ElseIf drResult("Currency").ToString = ViewState("Currency") Then
                                dr("PayRate") = "1"
                                ViewState("PayRateDtInv") = "1"
                            ElseIf drResult("Currency").ToString <> ViewState("Currency") Then
                                Dim DrRate As DataRow
                                DrRate = FindMaster("Rate", drResult("Currency").ToString + "|" + Format(tbDate.SelectedDate, "yyyy-MM-dd"), ViewState("DBConnection").ToString)
                                If Not DrRate Is Nothing Then
                                    dr("PayRate") = DrRate("Rate").ToString
                                    ViewState("PayRateDtInv") = DrRate("Rate").ToString
                                Else
                                    dr("PayRate") = "0"
                                    ViewState("PayRateDtInv") = "0"
                                End If
                            End If
                            If (lblCurrPayDt.Text = ViewState("Currency").ToString) Then
                                dr("PayRatePPn") = ViewState("PPNRateDtInv")
                            Else
                                dr("PayRatePPn") = ViewState("PayRateDtInv")
                            End If
                            dr("FgValue") = drResult("FgValue").ToString
                            dr("TotalInvoice") = drResult("Total_Forex").ToString
                            dr("TotalPaid") = drResult("Total_Paid").ToString
                            dr("TotalToPaid") = drResult("Amount_Saldo").ToString
                            dr("BaseInvoice") = drResult("Base_Forex").ToString
                            dr("BasePaid") = drResult("Base_Paid").ToString
                            dr("BaseToPaid") = FormatFloat(drResult("Base_Forex") - drResult("Base_Paid"), drResult("DigitCurrForex")) '(drResult("Base_Forex") - drResult("Base_Paid")) 
                            dr("PPNInvoice") = drResult("PPn_Forex").ToString
                            dr("PPNPaid") = drResult("PPN_Paid").ToString
                            dr("PPNToPaid") = FormatFloat(drResult("PPn_Forex") - drResult("PPN_Paid"), drResult("DigitCurrForex"))
                            dr("Remark") = ""
                            ViewState("Dt2").Rows.Add(dr)
                        End If

                        CountTotalInvoiceHome()
                        BindGridDt(ViewState("Dt2"), GridDt2)
                        EnableHd(GetCountRecord(ViewState("Dt2")) = 0)
                    Next


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
            lbTitle.Text = "Receipt Trade" 'Request.QueryString("MenuName").ToString
            lbTotPay.Text = "Receipt (" + ViewState("Currency") + ")"
            lbTotOther.Text = "Others (" + ViewState("Currency") + ")"
            lbTotInvoice.Text = "Invoice (" + ViewState("Currency") + ")"
            lbTotCharge.Text = "Income (" + ViewState("Currency") + ")"
            lbTotKurs.Text = "Selisih Kurs(" + ViewState("Currency") + ")"
            lbTotSelisih.Text = "Difference(" + ViewState("Currency") + ")"

            lbPayHome.Text = "Receipt (" + ViewState("Currency") + ")"
            lbInvoiceHome.Text = "Invoice (" + ViewState("Currency") + ")"
            'lbChargeHome.Text = "Charge (" + ViewState("Currency") + ")"

            FillCombo(ddlCurr, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
            'FillCombo(ddlChargeCurr, "EXEC S_GetCurrency", True, "Currency", "Currency", ViewState("DBConnection"))
            FillCombo(ddlCurrDt2, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
            FillCombo(ddlBankGiro, "SELECT Bank_Code, Bank_Name FROM VMsBankReceipt WHERE Currency = " + QuotedStr(ddlCurr.SelectedValue), True, "Bank_Code", "Bank_Name", ViewState("DBConnection"))

            ViewState("SortExpression") = Nothing
            'ViewState("DigitCurr") = 0
            'ViewState("DigitCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlCurr.SelectedValue), ViewState("DBConnection"))
            ViewState("DigitCurrInv") = 0
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString,ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
            If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
                ddlCommand.Items.Add("Print")
                ddlCommand2.Items.Add("Print")
                'ddlCommand.Items.Add("Print Full")
                'ddlCommand2.Items.Add("Print Full")
            End If
            tbTotalReceiptForex.Attributes.Add("ReadOnly", "True")
            tbTotalReceipt.Attributes.Add("ReadOnly", "True")
            tbTotalDPForex.Attributes.Add("ReadOnly", "True")
            tbTotalOthers.Attributes.Add("ReadOnly", "True")
            tbTotalInvoice.Attributes.Add("ReadOnly", "True")
            tbTotalCharge.Attributes.Add("ReadOnly", "True")
            tbTotalKurs.Attributes.Add("ReadOnly", "True")
            tbTotalSelisih.Attributes.Add("ReadOnly", "True")

            tbReceiptHome.Attributes.Add("ReadOnly", "True")
            tbInvoiceHome.Attributes.Add("ReadOnly", "True")
            'tbChargeHome.Attributes.Add("ReadOnly", "True")

            tbPPNRateDt2.Attributes.Add("ReadOnly", "True")
            tbTotalInvoiceDt2.Attributes.Add("ReadOnly", "True")
            TbTotalPaidDt2.Attributes.Add("ReadOnly", "True")
            tbBaseInvoiceDt2.Attributes.Add("ReadOnly", "True")
            tbBasePaidDt2.Attributes.Add("ReadOnly", "True")
            tbPPNInvoiceDt2.Attributes.Add("ReadOnly", "True")
            tbPPNPaidDt2.Attributes.Add("ReadOnly", "True")

            tbTotalToPaidDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbTotalInvoiceDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")
            TbTotalPaidDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbBaseInvoiceDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbBasePaidDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbBaseToPaidDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbPPNInvoiceDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbPPNPaidDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbPPNToPaidDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")

            tbReceiptForex.Attributes.Add("OnKeyDown", "return PressNumeric();")
            'tbChargeForex.Attributes.Add("OnKeyDown", "return PressNumeric();")

            tbTotalReceiptForex.Attributes.Add("OnBlur", "setformat();")
            tbTotalReceipt.Attributes.Add("OnBlur", "setformat();")
            tbTotalDPForex.Attributes.Add("OnBlur", "setformat();")
            tbTotalOthers.Attributes.Add("OnBlur", "setformat();")
            tbTotalInvoice.Attributes.Add("OnBlur", "setformat();")
            tbTotalCharge.Attributes.Add("OnBlur", "setformat();")
            tbTotalKurs.Attributes.Add("OnBlur", "setformat();")
            tbTotalSelisih.Attributes.Add("OnBlur", "setformat();")

            'tbChargeRate.Attributes.Add("OnBlur", "setformatdt();")
            tbReceiptForex.Attributes.Add("OnBlur", "setformatdt();")
            tbReceiptHome.Attributes.Add("OnBlur", "setformatdt();")
            tbInvoiceHome.Attributes.Add("OnBlur", "setformatdt();")
            'tbChargeForex.Attributes.Add("OnBlur", "setformatdt();")
            'tbChargeHome.Attributes.Add("OnBlur", "setformatdt();")

            tbTotalToPaidDt2.Attributes.Add("OnBlur", "setformatdt2('total');")
            tbTotalInvoiceDt2.Attributes.Add("OnBlur", "setformatdt2('total');")
            TbTotalPaidDt2.Attributes.Add("OnBlur", "setformatdt2('total');")
            tbBaseInvoiceDt2.Attributes.Add("OnBlur", "setformatdt2('base');")
            tbBasePaidDt2.Attributes.Add("OnBlur", "setformatdt2('base');")
            tbBaseToPaidDt2.Attributes.Add("OnBlur", "setformatdt2('base');")
            tbPPNInvoiceDt2.Attributes.Add("OnBlur", "setformatdt2('ppn');")
            tbPPNPaidDt2.Attributes.Add("OnBlur", "setformatdt2('ppn');")
            tbPPNToPaidDt2.Attributes.Add("OnBlur", "setformatdt2('ppn');")
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
                'GridView1.HeaderRow.Cells(8).Text = "Receipt Forex"
                'GridView1.HeaderRow.Cells(9).Text = "Receipt (" + ViewState("Currency") + ")"
                'GridView1.HeaderRow.Cells(10).Text = "Others (" + ViewState("Currency") + ")"
                'GridView1.HeaderRow.Cells(11).Text = "Invoice (" + ViewState("Currency") + ")"
                'GridView1.HeaderRow.Cells(10).Text = "Income (" + ViewState("Currency") + ")"
            End If
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub
    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT * From V_FNReceiptTradeDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function
    Private Function GetStringDt2(ByVal Nmbr As String) As String
        Return "SELECT * From V_FNReceiptTradeInv WHERE TransNmbr = " + QuotedStr(Nmbr)
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
                Session("DBConnection") = ViewState("DBConnection")
                Session("SelectCommand") = "EXEC S_FNFormVoucher " + Result + ",'ORT'," + QuotedStr(ViewState("UserId").ToString)
                Session("ReportFile") = ".../../../Rpt/FormVoucher.frx"
                AttachScript("openprintdlg();", Page, Me.GetType)
            ElseIf ActionValue = "Print Full" Then
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

                Session("SelectCommand") = "EXEC S_FNFormBuktiBankDt " + Result + ", 'RECEIPTTRADE', " + QuotedStr(ViewState("UserId").ToString)

                Session("ReportFile") = ".../../../Rpt/FormBuktiBankDt.frx"
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
                        Result = ExecSPCommandGo(ActionValue, "S_FNReceiptTrade", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
        'Dim count As Integer
        Try
            'ddlReport.Enabled = State And ViewState("StateHd") = "Insert"
            'Count = GetCountRecord(ViewState("Dt2"))
            tbCustCode.Enabled = State  'ViewState("StateHd") = "Insert" And count = 0
            btnSupp.Visible = State 'ViewState("StateHd") = "Insert" And Count = 0
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
            GridDt.HeaderRow.Cells(12).Text = "Receipt (" + ViewState("Currency") + ")"
            GridDt.HeaderRow.Cells(13).Text = "Invoice (" + ViewState("Currency") + ")"
            'GridDt.HeaderRow.Cells(13).Text = "Charge (" + ViewState("Currency") + ")"
            'End If
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDt2(ByVal Nmbr As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt2") = Nothing
            dt = SQLExecuteQuery(GetStringDt2(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt2") = dt
            BindGridDt(dt, GridDt2)
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
            EnableHd(GetCountRecord(ViewState("Dt2")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnCancelDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelDt2.Click
        Try
            MovePanel(pnlEditDt2, pnlDt2)
            EnableHd(GetCountRecord(ViewState("Dt2")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt2  Error : " + ex.ToString
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbCode.Text = ""
            tbCustCode.Text = ""
            tbCustName.Text = ""
            'ddlReport.SelectedValue = "Y"
            tbDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            tbTotalReceiptForex.Text = FormatFloat("0", ViewState("DigitCurr"))
            tbTotalReceipt.Text = FormatFloat("0", ViewState("DigitHome"))
            tbTotalDPForex.Text = FormatFloat("0", ViewState("DigitHome"))
            tbTotalOthers.Text = FormatFloat("0", ViewState("DigitHome"))
            tbTotalInvoice.Text = FormatFloat("0", ViewState("DigitHome"))
            tbTotalCharge.Text = FormatFloat("0", ViewState("DigitHome"))
            tbTotalKurs.Text = FormatFloat("0", ViewState("DigitHome"))
            tbTotalSelisih.Text = FormatFloat("0", ViewState("DigitHome"))
            tbAttn.Text = ""
            tbRemark.Text = ""
            'tbDate.SelectedDate = Session("ServerDate") 'Now.Date
            FillCombo(ddlReceiptType, "EXEC S_GetPayTypeUser " + QuotedStr("ReceiptTrade" + "Y") + ", " + QuotedStr(ViewState("UserId").ToString), True, "Payment_Code", "Payment_Name", ViewState("DBConnection")) 'ddlReport.SelectedValue
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            ddlReceiptType.SelectedIndex = 0
            ddlBankGiro.SelectedIndex = 0
            tbRemarkDt.Text = ""
            tbReceiptDate.SelectedDate = tbDate.SelectedDate
            tbDueDate.SelectedDate = Nothing
            tbDocumentNo.Text = ""
            tbVoucherNo.Enabled = False
            tbVoucherNo.Text = ""            
            tbRemarkDt.Text = tbRemark.Text
            ddlCurr.SelectedValue = ViewState("Currency")
            ViewState("DigitCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlCurr.SelectedValue), ViewState("DBConnection"))
            tbReceiptForex.Text = FormatFloat("0", ViewState("DigitCurr"))
            tbReceiptHome.Text = FormatFloat("0", ViewState("DigitHome"))
            tbInvoiceHome.Text = FormatFloat("0", ViewState("DigitHome"))
            'tbChargeRate.Text = "0"
            'tbChargeForex.Text = "0"
            'tbChargeHome.Text = "0"
            'ddlChargeCurr.SelectedValue = ViewState("Currency")
            ChangePaymentTypeV2(ddlReceiptType.SelectedValue, tbFgMode, tbDate, tbDueDateDt2, ddlBankGiro, ddlCurr, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
            tbRate.Text = FormatFloat(tbRate.Text.Replace(",", ""), ViewState("DigitRate"))
            
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt2()
        Try
            tbInvNoDt2.Text = ""
            tbBuyerCode.Text = ""
            tbBuyer.Text = ""
            tbDueDateDt2.SelectedDate = Nothing
            tbPPNRateDt2.Text = "0"
            tbTotalInvoiceDt2.Text = "0"
            TbTotalPaidDt2.Text = "0"
            tbTotalToPaidDt2.Text = "0"
            tbBaseInvoiceDt2.Text = "0"
            tbBasePaidDt2.Text = "0"
            tbBaseToPaidDt2.Text = "0"
            tbPPNInvoiceDt2.Text = "0"
            tbPPNPaidDt2.Text = "0"
            tbPPNToPaidDt2.Text = "0"
            tbFgValueDt2.Text = "1"
            ddlCurrDt2.SelectedValue = ViewState("Currency")
            ChangeCurrency(ddlCurrDt2, tbDate, tbRateDt2, ViewState("Currency"), ViewState("DigitCurrInv"), ViewState("DBConnection"), ViewState("DigitHome"))
            tbRemarkDt2.Text = ""            
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
            'If CInt(ViewState("GLYear")) <> Year(tbDate.SelectedValue) Or CInt(ViewState("GLPeriod")) <> Month(tbDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(ViewState("GLPeriod")) + " " + ViewState("GLYear").ToString.Trim)
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
                'If Dr.RowState = DataRowState.Deleted Then
                '    Return True
                'End If
                'If Dr("PriceForex").ToString = "0" Then
                '    lbStatus.Text = MessageDlg("Price Must Have Value")
                '    Return False
                'End If
            Else
                If ddlReceiptType.SelectedValue = "" Then
                    lbStatus.Text = MessageDlg("Receipt Type Must Have Value")
                    ddlReceiptType.Focus()
                    Return False
                End If
                If tbReceiptDate.SelectedDate = Nothing Then
                    lbStatus.Text = MessageDlg("Receipt Date Must Have Value")
                    tbReceiptDate.Focus()
                    Return False
                End If
                If tbFgMode.Text = "G" Then
                    If tbDocumentNo.Text.Trim = "" Then
                        lbStatus.Text = MessageDlg("Document No Must Have Value")
                        tbDocumentNo.Focus()
                        Return False
                    End If
                    If ddlBankGiro.SelectedValue = "" Then
                        lbStatus.Text = MessageDlg("Bank Receipt Must Have Value")
                        ddlBankGiro.Focus()
                        Return False
                    End If
                    If tbDueDate.SelectedDate = Nothing Then
                        lbStatus.Text = MessageDlg("Due Date Must Have Value")
                        tbDueDate.Focus()
                        Return False
                    End If

                    If tbFgMode.Text = "B" Or tbFgMode.Text = "K" Then
                        If tbVoucherNo.Text.Trim = "" Then
                            lbStatus.Text = MessageDlg("Voucher No Must Have Value")
                            tbVoucherNo.Focus()
                            Return False
                        End If
                    End If
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
                'If Dr.RowState = DataRowState.Deleted Then
                '    Return True
                'End If
                'If Dr("QtyOrder").ToString = "0" Then
                '    lbStatus.Text = MessageDlg("Qty Must Have Value")
                '    Return False
                'End If
            Else
                If tbInvNoDt2.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Invoice No Must Have Value")
                    tbInvNoDt2.Focus()
                    Return False
                End If
                If tbTotalToPaidDt2.Text.Trim = "" Or tbTotalToPaidDt2.Text.Trim = "0" Then
                    lbStatus.Text = MessageDlg("Total Paid Must Have Value")
                    tbBaseToPaidDt2.Focus()
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function

    Private Function AllowedRecordDt() As Integer
        Try
            If ViewState("FgMode") = tbFgMode.Text.Trim Then
                Return 1
            Else
                Return 0
            End If
        Catch ex As Exception
            Throw New Exception("Allowed Record Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveDt.Click
        Try
            If CekDt() = False Then
                btnSaveDt.Focus()
                Exit Sub
            End If
            If tbFgMode.Text = "G" Then
                If CekExistGiroIn(tbDocumentNo.Text.Trim, ViewState("DBConnection").ToString) = True Then
                    lbStatus.Text = "Giro Payment '" + tbDocumentNo.Text.Trim + "' has already exists in Giro Listing'"
                    Exit Sub
                End If
            End If
            If ViewState("StateDt") = "Edit" Then
                Dim Row As DataRow

                Row = ViewState("Dt").Select("ItemNo = " + lbItemNo.Text)(0)

                Row.BeginEdit()
                Row("ReceiptType") = ddlReceiptType.SelectedValue
                Row("ReceiptName") = ddlReceiptType.SelectedItem.Text
                Row("ReceiptDate") = Format(tbReceiptDate.SelectedDate, "dd MMM yyyy")
                Row("DocumentNo") = tbDocumentNo.Text
                Row("Reference") = tbVoucherNo.Text
                If Format(tbDueDate.SelectedDate, "dd MMM yyyy") <> "01 Jan 0001" Then
                    Row("DueDate") = tbDueDate.SelectedDate.ToString
                    Row("Due_Date") = Format(tbDueDate.SelectedDate, "dd MMM yyyy")
                Else
                    Row("DueDate") = DBNull.Value
                    Row("Due_Date") = ""
                End If

                'If tbDueDate.Enabled Then
                '    Row("DueDate") = Format(tbDueDate.SelectedDate, "dd MMM yyyy")
                'Else
                '    Row("DueDate") = DBNull.Value
                'End If
                Row("BankGiro") = ddlBankGiro.SelectedValue
                If ddlBankGiro.SelectedValue <> "" Then
                    Row("BankGiroName") = ddlBankGiro.SelectedItem.Text
                Else
                    Row("BankGiroName") = ""
                End If

                Row("Currency") = ddlCurr.SelectedValue

                Row("ForexRate") = tbRate.Text.Replace(",", "")
                Row("ReceiptForex") = tbReceiptForex.Text.Replace(",", "")
                Row("ReceiptHome") = tbReceiptHome.Text.Replace(",", "")
                Row("InvoiceHome") = tbInvoiceHome.Text.Replace(",", "")

                Row("ForexRate2") = tbRate.Text
                Row("ReceiptForex2") = tbReceiptForex.Text
                Row("ReceiptHome2") = tbReceiptHome.Text
                Row("InvoiceHome2") = tbInvoiceHome.Text

                'Row("ChargeCurrency") = ddlChargeCurr.SelectedValue
                'Row("ChargeRate") = tbChargeRate.Text
                'Row("ChargeForex") = tbChargeForex.Text
                'Row("ChargeHome") = tbChargeHome.Text
                Row("Remark") = tbRemarkDt.Text
                Row("FgMode") = tbFgMode.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                Dim ExistRow As DataRow()
                ExistRow = ViewState("Dt").Select("FgMode = " + QuotedStr(tbFgMode.Text))
                If ExistRow.Count > AllowedRecordDt() Then
                    lbStatus.Text = MessageDlg("Cannot insert more than one bank")
                    Exit Sub
                End If

                dr = ViewState("Dt").NewRow
                dr("ItemNo") = lbItemNo.Text
                dr("ReceiptType") = ddlReceiptType.SelectedValue
                dr("ReceiptName") = ddlReceiptType.SelectedItem.Text
                dr("ReceiptDate") = Format(tbReceiptDate.SelectedDate, "dd MMM yyyy")
                dr("DocumentNo") = tbDocumentNo.Text
                dr("Reference") = tbVoucherNo.Text
                If Format(tbDueDate.SelectedDate, "dd MMM yyyy") <> "01 Jan 0001" Then
                    dr("DueDate") = tbDueDate.SelectedDate.ToString
                    dr("Due_Date") = Format(tbDueDate.SelectedDate, "dd MMM yyyy")
                Else
                    dr("DueDate") = DBNull.Value
                    dr("Due_Date") = ""
                End If


                'If tbDueDate.Enabled Then
                '    dr("DueDate") = Format(tbDueDate.SelectedDate, "dd MMM yyyy")
                'Else
                '    dr("DueDate") = DBNull.Value
                'End If
                dr("BankGiro") = ddlBankGiro.SelectedValue
                If ddlBankGiro.SelectedValue <> "" Then
                    dr("BankGiroName") = ddlBankGiro.SelectedItem.Text
                Else
                    dr("BankGiroName") = ""
                End If

                dr("Currency") = ddlCurr.SelectedValue

                dr("ForexRate") = tbRate.Text.Replace(",", "")
                dr("ReceiptForex") = tbReceiptForex.Text.Replace(",", "")
                dr("ReceiptHome") = tbReceiptHome.Text.Replace(",", "")
                dr("InvoiceHome") = tbInvoiceHome.Text.Replace(",", "")

                dr("ForexRate2") = tbRate.Text
                dr("ReceiptForex2") = tbReceiptForex.Text
                dr("ReceiptHome2") = tbReceiptHome.Text
                dr("InvoiceHome2") = tbInvoiceHome.Text

                'dr("ChargeCurrency") = ddlChargeCurr.SelectedValue
                'dr("ChargeRate") = tbChargeRate.Text
                'dr("ChargeForex") = tbChargeForex.Text
                'dr("ChargeHome") = tbChargeHome.Text
                dr("Remark") = tbRemarkDt.Text
                dr("FgMode") = tbFgMode.Text
                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, PnlDt)
            EnableHd(GetCountRecord(ViewState("Dt2")) = 0)
            UpdateDtInvoice(lbItemNo.Text, ddlCurr.SelectedValue, tbRate.Text.Replace(",", ""))
            CountTotalInvoiceHome()
            CountTotalDt()
            BindGridDt(ViewState("Dt"), GridDt)
            StatusButtonSave(True)
            GridDt.HeaderRow.Cells(12).Text = "Receipt (" + ViewState("Currency") + ")"
            GridDt.HeaderRow.Cells(13).Text = "Invoice (" + ViewState("Currency") + ")"
            'GridDt.HeaderRow.Cells(13).Text = "Charge (" + ViewState("Currency") + ")"
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

            Dim ExistRow As DataRow()
            ExistRow = ViewState("Dt2").Select("InvoiceNo = " + QuotedStr(tbInvNoDt2.Text))
            If ViewState("StateDt2") = "Edit" Then
                Dim Row As DataRow

                If ExistRow.Count > AllowedRecordDt2() Then
                    lbStatus.Text = MessageDlg("Data Exist, Cannot Edit Data")
                    Exit Sub
                End If

                Row = ViewState("Dt2").Select("InvoiceNo = " + QuotedStr(ViewState("InvoiceNo")))(0)

                'lbStatus.Text = tbDueDateDt2.SelectedDate  'Format(tbDueDateDt2.SelectedDate, "yyyy-dd-mm")
                'Exit Sub

                Row.BeginEdit()
                Row("InvoiceNo") = tbInvNoDt2.Text
                Row("Buyer") = tbBuyerCode.Text
                Row("Buyer_Name") = tbBuyer.Text
                If tbDueDateDt2.Enabled Then
                    Row("DueDate") = tbDueDateDt2.SelectedDate.ToString
                Else
                    Row("DueDate") = DBNull.Value
                End If
                'Row("DueDate") = Format(tbDueDateDt2.SelectedDate, "dd MMM yyyy")
                Row("Due_Date") = Format(tbDueDateDt2.SelectedDate, "dd MMM yyyy")
                Row("Currency") = ddlCurrDt2.SelectedValue
                Row("ForexRate") = tbRateDt2.Text
                If tbPPNRateDt2.Text.Trim = "" Then
                    Row("PPNRate") = "0"
                Else
                    Row("PPNRate") = tbPPNRateDt2.Text
                End If
                Row("PayRate") = tbPayRateDt2.Text
                Row("PayRatePPn") = tbPayRatePPn.Text
                Row("FgValue") = tbFgValueDt2.Text
                Row("TotalInvoice") = tbTotalInvoiceDt2.Text
                Row("TotalPaid") = TbTotalPaidDt2.Text
                Row("TotalToPaid") = tbTotalToPaidDt2.Text
                Row("BaseInvoice") = tbBaseInvoiceDt2.Text
                Row("BasePaid") = tbBasePaidDt2.Text
                Row("BaseToPaid") = tbBaseToPaidDt2.Text
                Row("PPNInvoice") = tbPPNInvoiceDt2.Text
                Row("PPNPaid") = tbPPNPaidDt2.Text
                Row("PPNToPaid") = tbPPNToPaidDt2.Text
                Row("Remark") = tbRemarkDt2.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                If ExistRow.Count > 0 Then
                    lbStatus.Text = MessageDlg("Data Exist, Cannot Insert Data")
                    Exit Sub
                End If
                dr = ViewState("Dt2").NewRow
                dr("ItemNo") = GetNewItemNo(ViewState("Dt2"))
                dr("InvoiceNo") = tbInvNoDt2.Text
                dr("Buyer") = tbBuyerCode.Text
                dr("Buyer_Name") = tbBuyer.Text
                If tbDueDateDt2.Enabled Then
                    dr("DueDate") = tbDueDateDt2.SelectedDate.ToString
                    dr("Due_Date") = Format(tbDueDateDt2.SelectedDate, "dd MMM yyyy")
                Else
                    dr("DueDate") = DBNull.Value
                    dr("Due_Date") = ""
                End If
                'dr("DueDate") = Format(tbDueDateDt2.SelectedDate, "dd MMM yyyy")

                dr("Currency") = ddlCurrDt2.SelectedValue
                dr("ForexRate") = tbRateDt2.Text
                If tbPPNRateDt2.Text.Trim = "" Then
                    dr("PPNRate") = "0"
                Else
                    dr("PPNRate") = tbPPNRateDt2.Text
                End If
                dr("PayRate") = tbPayRateDt2.Text
                dr("PayRatePPn") = tbPayRatePPn.Text
                dr("FgValue") = tbFgValueDt2.Text
                dr("TotalInvoice") = tbTotalInvoiceDt2.Text
                dr("TotalPaid") = TbTotalPaidDt2.Text
                dr("TotalToPaid") = tbTotalToPaidDt2.Text
                dr("BaseInvoice") = tbBaseInvoiceDt2.Text
                dr("BasePaid") = tbBasePaidDt2.Text
                dr("BaseToPaid") = tbBaseToPaidDt2.Text
                dr("PPNInvoice") = tbPPNInvoiceDt2.Text
                dr("PPNPaid") = tbPPNPaidDt2.Text
                dr("PPNToPaid") = tbPPNToPaidDt2.Text
                dr("Remark") = tbRemarkDt2.Text
                ViewState("Dt2").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt2, pnlDt2)
            EnableHd(GetCountRecord(ViewState("Dt2")) = 0)
            CountTotalInvoiceHome()
            BindGridDt(ViewState("Dt2"), GridDt2)
            StatusButtonSave(True)
            'MovePanel(pnlEditDt2, pnlDt2)
            'EnableHd(GetCountRecord(ViewState("Dt2")) = 0)
            'Dim drow As DataRow()
            'drow = ViewState("Dt2").Select("ItemNo = " + lbItemNodt2.Text)
            'CountTotalInvoiceHome(lbItemNodt2.Text)
            'BindGridDt(drow.CopyToDataTable, GridDt2)
            'StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save dt2 Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub

    Private Sub UpdateDtInvoice(ByVal ItemNo As String, ByVal Currency As String, ByVal PayRate As String)
        Dim Dr As DataRow
        Dim drow As DataRow()
        Dim drEdit As DataRow
        Dim CurrencyInv, PPnRate, PayRateInv As String
        Try
            drow = ViewState("Dt2").Select("ItemNo = " + ItemNo)
            If drow.Count > 0 Then
                For Each Dr In drow.CopyToDataTable.Rows
                    If Not Dr.RowState = DataRowState.Deleted Then
                        drEdit = ViewState("Dt2").Select("ItemNo = " + ItemNo + " and InvoiceNo = " + QuotedStr(Dr("InvoiceNo")))(0)
                        drEdit.BeginEdit()
                        CurrencyInv = Dr("Currency")
                        PPnRate = Dr("PPNRate")
                        PayRateInv = Dr("PayRate")
                        If Currency = ViewState("Currency") Then
                            drEdit("PayRatePPn") = PPnRate
                        Else
                            If Currency = CurrencyInv Then
                                drEdit("PayRatePPn") = PayRate
                                drEdit("PayRate") = PayRate
                            Else
                                drEdit("PayRatePPn") = PayRateInv
                            End If
                        End If
                        drEdit.EndEdit()
                    End If
                Next
                BindGridDt(ViewState("Dt2"), GridDt2)
            End If
        Catch ex As Exception
            Throw New Exception("Update Dt Invoice Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub CountTotalInvoiceHome()
        Dim Hasil, Base, Rate, PPNRate, PPN, FgValue, PayRate, Kurs, PayRatePPn As Double
        Dim Dr As DataRow
        Dim drow As DataRow()
        Try
            'drow = ViewState("Dt2").Select("ItemNo = " + ItemNo)
            'Hasil = 0
            'If drow.Length > 0 Then
            '    For Each Dr In drow.CopyToDataTable.Rows
            '        If Not Dr.RowState = DataRowState.Deleted Then
            '            FgValue = CFloat(Dr("FgValue").ToString)
            '            Base = CFloat(Dr("BaseToPaid").ToString)
            '            Rate = CFloat(Dr("ForexRate").ToString)
            '            PayRate = CFloat(Dr("PayRate").ToString)
            '            PPNRate = CFloat(Dr("PPNRate").ToString)
            '            PPN = CFloat(Dr("PPNToPaid").ToString)
            '            PayRatePPn = CFloat(Dr("PayRatePPn").ToString)
            '            'Hasil = Hasil + (((Base * PayRate) + (PPN * PPNRate)) * FgValue) --kohjim suruh ganti 20151015 krn ada selisih kurs
            '            'Hasil = Hasil + (((Base * Rate) + (PPN * PPNRate)) * FgValue)
            '            Hasil = Hasil + ((((Base * Rate) + (PPN * PPNRate)) * FgValue) / CFloat(lblRatePayDt.Text))
            '            'Kurs = Kurs + (((Base * (PayRate - Rate)) + (PPN * (PayRatePPn - PPNRate))) * FgValue)
            '        End If
            '    Next
            'End If
            'Dr = ViewState("Dt").Select("ItemNo=" + ItemNo)(0)
            'Dr.BeginEdit()
            'Dr("InvoiceHome") = FormatNumber(Hasil, ViewState("DigitHome"))
            'Dr.EndEdit()
            'BindGridDt(ViewState("Dt"), GridDt)

            Hasil = 0
            Kurs = 0
            For Each Dr In ViewState("Dt2").Rows
                If Not Dr.RowState = DataRowState.Deleted Then
                    FgValue = CFloat(Dr("FgValue").ToString)
                    Base = CFloat(Dr("BaseToPaid").ToString)
                    PPN = CFloat(Dr("PPNToPaid").ToString)
                    Rate = CFloat(Dr("ForexRate").ToString)
                    PayRate = CFloat(Dr("PayRate").ToString)
                    PPNRate = CFloat(Dr("PPNRate").ToString)
                    PayRatePPn = CFloat(Dr("PayRatePPn").ToString)
                    'Hasil = Hasil + (((Base * PayRate) + (PPN * PPNRate)) * FgValue) --kohjim suruh ganti 20151015 krn ada selisih kurs
                    Hasil = Hasil + (((Base * Rate) + (PPN * PPNRate)) * FgValue)
                    Kurs = Kurs + (((Base * (PayRate - Rate)) + (PPN * (PayRatePPn - PPNRate))) * FgValue)
                End If
            Next
            'For Each Dr In ViewState("Dt").Rows
            '    If Not Dr.RowState = DataRowState.Deleted Then
            '        Hasil = Hasil + CFloat(Dr("InvoiceHome").ToString)
            '    End If
            'Next
            tbTotalInvoice.Text = FormatNumber(Hasil, ViewState("DigitHome"))
            tbTotalKurs.Text = FormatNumber(Kurs, ViewState("DigitHome"))
            tbTotalSelisih.Text = FormatNumber(CFloat(tbTotalReceipt.Text) + CFloat(tbTotalDPForex.Text) + CFloat(tbTotalOthers.Text) - CFloat(tbTotalCharge.Text) - CFloat(tbTotalInvoice.Text) - CFloat(tbTotalKurs.Text), ViewState("DigitHome"))
        Catch ex As Exception
            Throw New Exception("Count Total Invoice Home Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub CountTotalDt()
        Dim infois, peimen, peimenf, cas, other, DP As Double
        Dim Dr As DataRow
        Try
            infois = 0
            peimen = 0
            peimenf = 0
            cas = 0
            other = 0
            DP = 0
            For Each Dr In ViewState("Dt").Rows
                If Not Dr.RowState = DataRowState.Deleted Then
                    infois = infois + CFloat(Dr("InvoiceHome").ToString)
                    If Dr("FgMode").ToString = "I" Then  'Expense
                        cas = cas + CFloat(Dr("ReceiptHome").ToString)
                    ElseIf Dr("FgMode").ToString = "O" Then  'Expense
                        other = other + CFloat(Dr("ReceiptHome").ToString)
                    ElseIf Dr("FgMode").ToString = "D" Then  'Expense
                        DP = DP + CFloat(Dr("ReceiptHome").ToString)
                    Else
                        peimen = peimen + CFloat(Dr("ReceiptHome").ToString)
                        peimenf = peimenf + CFloat(Dr("ReceiptForex").ToString)
                    End If
                    'peimen = peimen + CFloat(Dr("ReceiptHome").ToString)
                    'cas = cas + CFloat(Dr("ChargeHome").ToString)
                End If
            Next
            tbTotalDPForex.Text = FormatNumber(DP, ViewState("DigitCurr"))
            tbTotalReceiptForex.Text = FormatNumber(peimenf, ViewState("DigitCurr"))
            tbTotalReceipt.Text = FormatNumber(peimen, ViewState("DigitHome"))
            tbTotalOthers.Text = FormatNumber(other, ViewState("DigitHome"))
            tbTotalCharge.Text = FormatNumber(cas, ViewState("DigitHome"))
            'tbTotalInvoice.Text = FormatNumber(infois, ViewState("DigitHome"))
            tbTotalSelisih.Text = FormatNumber(CFloat(tbTotalReceipt.Text) + CFloat(tbTotalDPForex.Text) + CFloat(tbTotalOthers.Text) - CFloat(tbTotalCharge.Text) - CFloat(tbTotalInvoice.Text) - CFloat(tbTotalKurs.Text), ViewState("DigitHome"))
        Catch ex As Exception
            Throw New Exception("Count Total Dt Error : " + ex.ToString)
        End Try
    End Sub

    Private Function AllowedRecordDt2() As Integer
        Try
            If ViewState("InvoiceNo") = tbInvNoDt2.Text.Trim Then
                Return 1
            Else
                Return 0
            End If
        Catch ex As Exception
            Throw New Exception("Allowed Record Error : " + ex.ToString)
        End Try
    End Function

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
                'ddlReport.SelectedValue
                tbCode.Text = GetAutoNmbr("ORT", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO FINReceiptTradeHd (TransNmbr, TransDate, STATUS, FgReport, " + _
                "Customer, Attn, TotalDP, TotalReceiptForex, TotalReceipt, TotalOthers, TotalInvoice, TotalCharge, TotalKurs, TotalSelisih, Remark, UserPrep, " + _
                "DatePrep) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", 'H', " + _
                QuotedStr("Y") + ", " + QuotedStr(tbCustCode.Text) + ", " + QuotedStr(tbAttn.Text) + _
                ", " + tbTotalDPForex.Text.Replace(",", "") + _
                ", " + tbTotalReceiptForex.Text.Replace(",", "") + _
                ", " + tbTotalReceipt.Text.Replace(",", "") + ", " + tbTotalOthers.Text.Replace(",", "") + _
                ", " + tbTotalInvoice.Text.Replace(",", "") + ", " + tbTotalCharge.Text.Replace(",", "") + _
                ", " + tbTotalKurs.Text.Replace(",", "") + _
                ", " + tbTotalSelisih.Text.Replace(",", "") + ", " + QuotedStr(tbRemark.Text) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", getDate()"

                ViewState("TransNmbr") = tbCode.Text

            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM FINReceiptTradeHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE FINReceiptTradeHd SET TransDate = " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", FgReport ='Y', Customer=" + QuotedStr(tbCustCode.Text) + _
                ", Attn =" + QuotedStr(tbAttn.Text) + ", TotalReceipt=" + tbTotalReceipt.Text.Replace(",", "") + _
                ", TotalDP=" + tbTotalDPForex.Text.Replace(",", "") + _
                ", TotalReceiptForex=" + tbTotalReceiptForex.Text.Replace(",", "") + ", TotalOthers=" + tbTotalOthers.Text.Replace(",", "") + _
                ", TotalInvoice= " + tbTotalInvoice.Text.Replace(",", "") + ", TotalSelisih = " + tbTotalSelisih.Text.Replace(",", "") + _
                ", TotalKurs = " + tbTotalKurs.Text.Replace(",", "") + _
                ", TotalCharge= " + tbTotalCharge.Text.Replace(",", "") + ", Remark = " + QuotedStr(tbRemark.Text) + _
                ", DatePrep = getDate()" + _
                " WHERE TransNmbr = " + QuotedStr(tbCode.Text)

                ViewState("TransNmbr") = tbCode.Text
            End If
            SQLString = ChangeQuoteNull(SQLString)            
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            'update Primary Key on Dt
            Dim Row As DataRow()

            Row = ViewState("Dt").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbCode.Text
                If Row(I)("FgMode") = "I" Then
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
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, ItemNo, ReceiptType, ReceiptDate, DocumentNo, Reference, DueDate," + _
            " BankGiro, Currency, ForexRate, ReceiptForex, ReceiptHome, InvoiceHome, Remark, " + _
            " FgMode, FgValue FROM FinReceiptTradeDt WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            'Dim cmdSql As New SqlCommand("SELECT TransNmbr, ItemNo, ReceiptType, ReceiptDate, DocumentNo, Reference, DueDate, BankGiro, Currency, ForexRate, ReceiptForex, ReceiptHome, InvoiceHome, Remark, ChargeCurrency, ChargeRate, ChargeForex, ChargeHome, FgMode, ProductClass FROM FinReceiptTradeDt WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)

            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            ' lbStatus.Text = "UPDATE FINReceiptTradeDt SET ItemNo = @ItemNo, ReceiptType = @ReceiptType, ReceiptDate = @ReceiptDate, DocumentNo = @DocumentNo, Reference = @Reference, DueDate = @DueDate, BankGiro = @BankGiro, Currency = @Currency, ForexRate = @ForexRate, ReceiptForex = @ReceiptForex, ReceiptHome = @ReceiptHome, InvoiceHome = @InvoiceHome, Remark = @Remark, ChargeCurrency = @ChargeCurrency, ChargeRate = @ChargeRate, ChargeForex = @ChargeForex, ChargeHome = @ChargeHome, FgMode = @FgMode WHERE TransNmbr = '" & ViewState("TransNmbr") & "' "
            'Exit Sub

            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
            "UPDATE FINReceiptTradeDt SET ItemNo = @ItemNo, ReceiptType = @ReceiptType, " + _
            "ReceiptDate = @ReceiptDate, DocumentNo = @DocumentNo, Reference = @Reference, " + _
            "DueDate = @DueDate, BankGiro = @BankGiro, Currency = @Currency, ForexRate = @ForexRate, " + _
            "ReceiptForex = @ReceiptForex, ReceiptHome = @ReceiptHome, InvoiceHome = @InvoiceHome, " + _
            "Remark = @Remark, FgMode = @FgMode, FgValue = @FgValue " + _
            "WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND ItemNo = @OldItemNo", con)
            
            ' Define output parameters.
            Update_Command.Parameters.Add("@ItemNo", SqlDbType.Int, 4, "ItemNo")
            Update_Command.Parameters.Add("@ReceiptType", SqlDbType.VarChar, 5, "ReceiptType")
            Update_Command.Parameters.Add("@ReceiptDate", SqlDbType.DateTime, 8, "ReceiptDate")
            Update_Command.Parameters.Add("@DocumentNo", SqlDbType.VarChar, 60, "DocumentNo")
            Update_Command.Parameters.Add("@Reference", SqlDbType.VarChar, 20, "Reference")
            Update_Command.Parameters.Add("@DueDate", SqlDbType.DateTime, 8, "DueDate")
            Update_Command.Parameters.Add("@BankGiro", SqlDbType.VarChar, 5, "BankGiro")
            Update_Command.Parameters.Add("@Currency", SqlDbType.VarChar, 5, "Currency")
            Update_Command.Parameters.Add("@ForexRate", SqlDbType.Float, 9, "ForexRate")
            Update_Command.Parameters.Add("@ReceiptForex", SqlDbType.Float, 9, "ReceiptForex")
            Update_Command.Parameters.Add("@ReceiptHome", SqlDbType.Float, 9, "ReceiptHome")
            Update_Command.Parameters.Add("@InvoiceHome", SqlDbType.Float, 9, "InvoiceHome")
            Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 255, "Remark")
            Update_Command.Parameters.Add("@FgMode", SqlDbType.VarChar, 1, "FgMode")
            Update_Command.Parameters.Add("@FgValue", SqlDbType.Int, 4, "FgValue")
            '' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@OldItemNo", SqlDbType.Int, 4, "ItemNo")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM FINReceiptTradeDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND ItemNo = @ItemNo ", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@ItemNo", SqlDbType.Int, 4, "ItemNo")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("FINReceiptTradeDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt

            '===========================================================================================
            If Not ViewState("Dt2") Is Nothing Then
                If Not ViewState("Dt2") Is Nothing Then
                    Row = ViewState("Dt2").Select("TransNmbr IS NULL")
                    For I = 0 To Row.Length - 1
                        Row(I).BeginEdit()
                        Row(I)("TransNmbr") = tbCode.Text
                        Row(I).EndEdit()
                    Next
                End If

                'save dt2
                cmdSql = New SqlCommand("SELECT TransNmbr, ItemNo, InvoiceNo, Buyer, DueDate, Currency, ForexRate, " + _
                                        "PPnRate, PayRate, PayRatePPn, TotalInvoice, TotalPaid, TotalToPaid, BaseInvoice, BasePaid, " + _
                                        "BaseToPaid, PPNInvoice, PPNPaid, PPNToPaid, Remark, FgValue FROM FINReceiptTradeDtInv WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
                da = New SqlDataAdapter(cmdSql)
                dbcommandBuilder = New SqlCommandBuilder(da)
                da.InsertCommand = dbcommandBuilder.GetInsertCommand
                'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
                'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

                Dim param2 As SqlParameter
                ' Create the UpdateCommand.
                Dim Update_Command2 = New SqlCommand( _
                        "UPDATE FINReceiptTradeDtInv SET ItemNo = @ItemNo, InvoiceNo = @InvoiceNo, Buyer = @Buyer, " + _
                        "DueDate = @DueDate, Currency = @Currency, " + _
                        "ForexRate = @ForexRate, PPnRate = @PPnRate, PayRate = @PayRate, PayRatePPn = @PayRatePPn," + _
                        "TotalInvoice = @TotalInvoice, TotalPaid = @TotalPaid, TotalToPaid = @TotalToPaid, " + _
                        "BasePaid = @BasePaid, BaseToPaid = @BaseToPaid, PPNInvoice = @PPNInvoice, PPNPaid = @PPNPaid, " + _
                        "PPNToPaid = @PPNToPaid, Remark = @Remark, FgValue = @FgValue " + _
                        "WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND InvoiceNo = @OldInvoiceNo AND ItemNo = @OldItemNo", con)
                ' Define output parameters.
                Update_Command2.Parameters.Add("@ItemNo", SqlDbType.Int, 4, "ItemNo")
                Update_Command2.Parameters.Add("@InvoiceNo", SqlDbType.VarChar, 30, "InvoiceNo")
                Update_Command2.Parameters.Add("@Buyer", SqlDbType.VarChar, 12, "Buyer")
                Update_Command2.Parameters.Add("@DueDate", SqlDbType.DateTime, 8, "DueDate")
                Update_Command2.Parameters.Add("@Currency", SqlDbType.VarChar, 5, "Currency")
                Update_Command2.Parameters.Add("@ForexRate", SqlDbType.Float, 18, "ForexRate")
                Update_Command2.Parameters.Add("@PPnRate", SqlDbType.Float, 18, "PPnRate")
                Update_Command2.Parameters.Add("@PayRate", SqlDbType.Float, 18, "PayRate")
                Update_Command2.Parameters.Add("@PayRatePPn", SqlDbType.Float, 18, "PayRatePPn")
                Update_Command2.Parameters.Add("@TotalInvoice", SqlDbType.Float, 18, "TotalInvoice")
                Update_Command2.Parameters.Add("@TotalPaid", SqlDbType.Float, 18, "TotalPaid")
                Update_Command2.Parameters.Add("@TotalToPaid", SqlDbType.Float, 18, "TotalToPaid")
                Update_Command2.Parameters.Add("@BasePaid", SqlDbType.Float, 18, "BasePaid")
                Update_Command2.Parameters.Add("@BaseToPaid", SqlDbType.Float, 18, "BaseToPaid")
                Update_Command2.Parameters.Add("@PPNInvoice", SqlDbType.Float, 18, "PPNInvoice")
                Update_Command2.Parameters.Add("@PPNPaid", SqlDbType.Float, 18, "PPNPaid")
                Update_Command2.Parameters.Add("@PPNToPaid", SqlDbType.Float, 18, "PPNToPaid")
                Update_Command2.Parameters.Add("@Remark", SqlDbType.VarChar, 255, "Remark")
                Update_Command2.Parameters.Add("@FgValue", SqlDbType.Int, 4, "FgValue")
                ' Define intput (WHERE) parameters.
                param2 = Update_Command2.Parameters.Add("@OldInvoiceNo", SqlDbType.VarChar, 30, "InvoiceNo")
                param2.SourceVersion = DataRowVersion.Original
                param2 = Update_Command2.Parameters.Add("@OldItemNo", SqlDbType.Int, 4, "ItemNo")
                param2.SourceVersion = DataRowVersion.Original
                ' Attach the update command to the DataAdapter.
                da.UpdateCommand = Update_Command2

                ' Create the DeleteCommand.
                Dim Delete_Command2 = New SqlCommand( _
                    "DELETE FROM FINReceiptTradeDtInv WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND InvoiceNo = @InvoiceNo AND ItemNo = @ItemNo", con)
                ' Add the parameters for the DeleteCommand.
                param2 = Delete_Command2.Parameters.Add("@InvoiceNo", SqlDbType.VarChar, 30, "InvoiceNo")
                param2.SourceVersion = DataRowVersion.Original
                param2 = Delete_Command2.Parameters.Add("@ItemNo", SqlDbType.Int, 4, "ItemNo")
                param2.SourceVersion = DataRowVersion.Original
                da.DeleteCommand = Delete_Command2

                Dim Dt2 As New DataTable("FINReceiptTradeDtInv")

                Dt2 = ViewState("Dt2")
                da.Update(Dt2)
                Dt2.AcceptChanges()
                ViewState("Dt2") = Dt2
            End If
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
            If tbCustCode.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Customer must have value")
                tbCustCode.Focus()
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
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

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ViewState("DigitCurr") = 0
            ViewState("DigitCurrInv") = 0
            ViewState("TransNmbr") = ""
            ClearHd()
            Cleardt()
            Cleardt2()
            PnlDt.Visible = True
            BindDataDt("")
            BindDataDt2("")
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
            FDateName = "Date, Due Date"
            FDateValue = "TransDate, DueDate"
            FilterName = "Receipt No, Receipt Date, Customer, Attn, Document No, Voucher No, Invoice No, Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), Customer, Attn, DocumentNo, Voucher_No, InvoiceNo, Remark"
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
                    ' FillCombo(ddlReceiptType, "EXEC S_GetPayTypeUser(" + QuotedStr("ReceiptTrade" + ddlReport.SelectedValue) + ", " + QuotedStr(Session("UserId").ToString) + ")", True, "Receipt_Code", "Receipt_Name", Session("DBConnection"))
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
                        FillCombo(ddlReceiptType, "EXEC S_GetPayTypeUser " + QuotedStr("ReceiptTrade" + "Y") + ", " + QuotedStr(ViewState("UserId").ToString), True, "Payment_Code", "Payment_Name", ViewState("DBConnection")) 'ddlReport.SelectedValue
                        EnableHd(GetCountRecord(ViewState("Dt2")) = 0)
                    Else
                        lbStatus.Text = MessageDlg("Data must be Hold or Get Approval to edit")
                        Exit Sub
                    End If
                ElseIf DDL.SelectedValue = "Print Full" Then
                    Try
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        Session("DBConnection") = ViewState("DBConnection")
                        Session("SelectCommand") = "EXEC S_FNFormBuktiBank '''" + GVR.Cells(2).Text + "''','RECEIPTTRADE'"
                        Session("ReportFile") = ".../../../Rpt/FormBuktiBank.frx"
                        AttachScript("openprintdlg();", Page, Me.GetType)
                    Catch ex As Exception
                        lbStatus.Text = "btn print Error = " + ex.ToString
                    End Try
                ElseIf DDL.SelectedValue = "Print" Then
                    Try
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        Session("DBConnection") = ViewState("DBConnection")
                        Session("SelectCommand") = "EXEC S_FNFormVoucher '''" + GVR.Cells(2).Text + "''','ORT'," + QuotedStr(ViewState("UserId").ToString)
                        Session("ReportFile") = ".../../../Rpt/FormVoucher.frx"
                        AttachScript("openprintdlg();", Page, Me.GetType)
                    Catch ex As Exception
                        lbStatus.Text = "btn print Error = " + ex.ToString
                    End Try
                ElseIf DDL.SelectedValue = "PrintTT" Then
                    Try
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        Session("SelectCommand") = "EXEC S_FNFormTT '" + GVR.Cells(2).Text + "'"
                        Session("ReportFile") = ".../../../Rpt/FormTT.frx"
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

    Protected Sub GridDt_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridDt.RowCommand
        Dim GVR As GridViewRow

        Try
            If e.CommandName = "Detail" Then
                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))
                If GVR.Cells(2).Text = "&nbsp;" Then
                    Exit Sub
                End If
                lbItemNodt2.Text = GVR.Cells(2).Text
                lblReceiptTypeDt2.Text = GVR.Cells(3).Text
                lblPayCurr.Text = "Receipt (" + GVR.Cells(9).Text + ")"
                lblPayHome.Text = "Invoice (" + GVR.Cells(9).Text + ")"
                'lblPayCurr.Text = "Receipt (" + ViewState("Currency") + ")"
                'lblPayHome.Text = "Invoice (" + ViewState("Currency") + ")"
                'lblInvHome.Text = "Charge (" + ViewState("Currency") + ")"
                lblCurrPayDt.Text = GVR.Cells(9).Text
                lblRatePayDt.Text = GVR.Cells(14).Text
                'tbPayHomeDt.Text = GVR.Cells(12).Text 'payment home
                tbPayHomeDt.Text = GVR.Cells(11).Text 'payment forex
                tbInvHomeDt.Text = GVR.Cells(13).Text
                'tbChargeHomeDt.Text = GVR.Cells(14).Text

                Dim lb As Label
                lb = GVR.FindControl("lbFgMode")
                lbFgModeDt2.Text = lb.Text

                MultiView1.ActiveViewIndex = 1
                Dim drow As DataRow()
                If ViewState("Dt2") Is Nothing Then
                    BindDataDt2(ViewState("TransNmbr"))
                    GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
                Else
                    drow = ViewState("Dt2").Select("ItemNo=" + GVR.Cells(2).Text)
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
            ElseIf e.CommandName = "Edit" Then
                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))

                lblRatePayDt.Text = GVR.Cells(14).Text
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Try
            Dim dr(), ExistRow() As DataRow
            Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
            ExistRow = ViewState("Dt").Select("ItemNo = " + QuotedStr(GVR.Cells(1).Text))
            'If ExistRow.Length > 0 Then
            '    lbStatus.Text = MessageDlg("Detail Invoice Exist, cannot delete data")
            '    Exit Sub
            'End If
            dr = ViewState("Dt").Select("ItemNo = " + QuotedStr(GVR.Cells(1).Text))
            dr(0).Delete()
            BindGridDt(ViewState("Dt"), GridDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            CountTotalDt()
            CountTotalInvoiceHome()
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Dim TotalInvoice As Double = 0
    Protected Sub GridDt2_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt2.RowDataBound
        Dim FgValue, Base, Rate, PPn, PPnRate, PayRate, PayRatePPn As Decimal
        Try
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "ItemNo")) Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                    FgValue = CFloat(DataBinder.Eval(e.Row.DataItem, "FgValue").ToString)
                    Base = CFloat(DataBinder.Eval(e.Row.DataItem, "BaseToPaid").ToString)
                    Rate = CFloat(DataBinder.Eval(e.Row.DataItem, "ForexRate").ToString)
                    PayRate = CFloat(DataBinder.Eval(e.Row.DataItem, "PayRate").ToString)
                    PayRatePPn = CFloat(DataBinder.Eval(e.Row.DataItem, "PayRatePPn").ToString)
                    PPnRate = CFloat(DataBinder.Eval(e.Row.DataItem, "PPnRate").ToString)
                    PPn = CFloat(DataBinder.Eval(e.Row.DataItem, "PPnToPaid").ToString)
                    'TotalInvoice = TotalInvoice + (((Base * PayRate) + (PPn * PPnRate)) * FgValue)
                    'TotalInvoice = TotalInvoice + (((Base * Rate) + (PPn * PPnRate)) * FgValue)
                    TotalInvoice = TotalInvoice + ((((Base * PayRate) + (PPn * PayRatePPn)) * FgValue) / CFloat(lblRatePayDt.Text)) '
                ElseIf e.Row.RowType = DataControlRowType.Footer Then
                    If lblCurrPayDt.Text <> "" Then
                        ViewState("DigitInv") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(lblCurrPayDt.Text), ViewState("DBConnection"))
                    Else
                        ViewState("DigitInv") = "0"
                    End If
                    tbInvHomeDt.Text = FormatNumber(TotalInvoice, ViewState("DigitInv"))
                End If
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub GridDt2_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt2.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow = GridDt2.Rows(e.RowIndex)
            dr = ViewState("Dt2").Select("InvoiceNo = " + QuotedStr(GVR.Cells(1).Text))
            'dr = ViewState("Dt2").Select("ItemNo = " + lbItemNodt2.Text + " AND InvoiceNo = " + QuotedStr(GVR.Cells(2).Text))
            dr(0).Delete()
            'Dim drow As DataRow()
            'drow = ViewState("Dt2").Select("ItemNo = " + lbItemNodt2.Text)
            'If drow.Length > 0 Then
            '    BindGridDt(drow.CopyToDataTable, GridDt2)
            '    GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
            'Else
            '    Dim DtTemp As New DataTable
            '    DtTemp = ViewState("Dt2").Clone
            '    DtTemp.Rows.Add(DtTemp.NewRow()) ' create a new blank row to the DataTable
            '    GridDt2.DataSource = DtTemp
            '    GridDt2.DataBind()
            '    GridDt2.Columns(0).Visible = False
            'End If
            CountTotalInvoiceHome()
            BindGridDt(ViewState("Dt2"), GridDt2)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt 2 Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            tbCode.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            'BindToDropList(ddlReport, Dt.Rows(0)("FgReport").ToString)
            BindToText(tbCustCode, Dt.Rows(0)("CustomerCode").ToString)
            BindToText(tbCustName, Dt.Rows(0)("CustomerName").ToString)
            BindToText(tbAttn, Dt.Rows(0)("Attn").ToString)
            ViewState("DigitCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(Dt.Rows(0)("Currency").ToString), ViewState("DBConnection"))
            BindToText(tbTotalReceiptForex, Dt.Rows(0)("TotalReceiptForex").ToString, ViewState("DigitCurr"))
            BindToText(tbTotalReceipt, Dt.Rows(0)("TotalReceipt").ToString, ViewState("DigitHome"))
            BindToText(tbTotalDPForex, Dt.Rows(0)("TotalDP").ToString, ViewState("DigitHome"))
            BindToText(tbTotalOthers, Dt.Rows(0)("TotalOthers").ToString, ViewState("DigitHome"))
            BindToText(tbTotalInvoice, Dt.Rows(0)("TotalInvoice").ToString, ViewState("DigitHome"))
            BindToText(tbTotalCharge, Dt.Rows(0)("TotalCharge").ToString, ViewState("DigitHome"))
            BindToText(tbTotalKurs, Dt.Rows(0)("TotalKurs").ToString, ViewState("DigitHome"))
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
                BindToDropList(ddlReceiptType, Dr(0)("ReceiptType").ToString)
                tbReceiptDate.SelectedDate = tbDate.SelectedDate
                'BindToDate(tbReceiptDate, Dr(0)("ReceiptDate").ToString)
                BindToText(tbDocumentNo, Dr(0)("DocumentNo").ToString)
                BindToText(tbVoucherNo, Dr(0)("Reference").ToString)
                BindToText(tbFgMode, Dr(0)("FgMode").ToString)
                BindToDate(tbDueDate, Dr(0)("DueDate").ToString)                
                BindToDropList(ddlCurr, Dr(0)("Currency").ToString)
                FillCombo(ddlBankGiro, "SELECT Bank_Code, Bank_Name FROM VMsBankReceipt WHERE Currency = " + QuotedStr(ddlCurr.SelectedValue), True, "Bank_Code", "Bank_Name", ViewState("DBConnection"))
                BindToDropList(ddlBankGiro, Dr(0)("BankGiro").ToString)
                'BindToDropList(ddlChargeCurr, Dr(0)("ChargeCurrency").ToString)
                ViewState("DigitCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlCurr.SelectedValue), ViewState("DBConnection"))
                'ViewState("DigitChargeCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlChargeCurr.SelectedValue), ViewState("DBConnection"))                

                BindToText(tbRate, Dr(0)("ForexRate2").ToString)
                'BindToText(tbChargeRate, Dr(0)("ChargeRate").ToString)
                BindToText(tbReceiptForex, Dr(0)("ReceiptForex2").ToString)
                BindToText(tbReceiptHome, Dr(0)("ReceiptHome2").ToString)
                BindToText(tbInvoiceHome, Dr(0)("InvoiceHome2").ToString)
                'BindToText(tbChargeForex, Dr(0)("ChargeForex").ToString)
                'BindToText(tbChargeHome, Dr(0)("ChargeHome").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)

                'If ddlChargeCurr.SelectedValue = "" Then
                '    tbChargeForex.Text = "0"
                '    tbChargeHome.Text = "0"
                'End If                
                ddlBankGiro.Enabled = tbFgMode.Text = "G"
                tbDueDate.Enabled = tbFgMode.Text = "G"
                'ddlChargeCurr.Enabled = tbFgMode.Text = "B"
                'tbChargeForex.Enabled = tbFgMode.Text = "B"
                'tbChargeRate.Enabled = ddlChargeCurr.SelectedValue <> Session("Currency") And tbFgMode.Text = "B" And ddlChargeCurr.SelectedValue <> ""
                tbVoucherNo.Enabled = (tbFgMode.Text = "B" Or tbFgMode.Text = "K")
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt2(ByVal InvoiceNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt2").select(" InvoiceNo =" + QuotedStr(InvoiceNo.ToString))
            If Dr.Length > 0 Then
                'lbItemNodt2.Text = ItemNo
                BindToText(tbInvNoDt2, Dr(0)("InvoiceNo").ToString)
                BindToText(tbBuyerCode, Dr(0)("Buyer").ToString)
                BindToText(tbBuyer, Dr(0)("Buyer_Name").ToString)
                BindToDate(tbDueDateDt2, Dr(0)("Due_Date").ToString)
                BindToDropList(ddlCurrDt2, Dr(0)("Currency").ToString)
                ViewState("DigitCurrInv") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlCurrDt2.SelectedValue), ViewState("DBConnection"))
                BindToText(tbRateDt2, Dr(0)("ForexRate").ToString)
                BindToText(tbPPNRateDt2, Dr(0)("PPnRate").ToString)
                BindToText(tbPayRateDt2, Dr(0)("PayRate").ToString)
                BindToText(tbPayRatePPn, Dr(0)("PayRatePPn").ToString)
                BindToText(tbFgValueDt2, Dr(0)("FgValue").ToString)
                BindToText(tbTotalInvoiceDt2, Dr(0)("TotalInvoice").ToString)
                BindToText(TbTotalPaidDt2, Dr(0)("TotalPaid").ToString)
                BindToText(tbTotalToPaidDt2, Dr(0)("TotalToPaid").ToString)
                BindToText(tbBaseInvoiceDt2, Dr(0)("BaseInvoice").ToString)
                BindToText(tbBasePaidDt2, Dr(0)("BasePaid").ToString)
                BindToText(tbBaseToPaidDt2, Dr(0)("BaseToPaid").ToString)
                BindToText(tbPPNInvoiceDt2, Dr(0)("PPNInvoice").ToString)
                BindToText(tbPPNPaidDt2, Dr(0)("PPNPaid").ToString)
                BindToText(tbPPNToPaidDt2, Dr(0)("PPNToPaid").ToString)
                BindToText(tbRemarkDt2, Dr(0)("Remark").ToString)
                If (lblCurrPayDt.Text = ViewState("Currency").ToString) Then
                    If (ddlCurrDt2.SelectedValue = ViewState("Currency").ToString) Then
                        tbPayRateDt2.Enabled = False
                    Else
                        tbPayRateDt2.Enabled = True
                    End If
                Else
                    If (ddlCurrDt2.SelectedValue = ViewState("Currency").ToString) Then
                        tbPayRateDt2.Enabled = False
                    Else
                        If (ddlCurrDt2.SelectedValue = lblCurrPayDt.Text) Then
                            tbPayRateDt2.Enabled = False
                        Else
                            tbPayRateDt2.Enabled = True
                        End If
                    End If

                End If

            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail 2 error : " + ex.ToString)
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
            ViewState("StateDt2") = "Edit"
            ViewState("InvoiceNo") = GVR.Cells(1).Text
            StatusButtonSave(False)
            btnSaveDt2.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid dt2 Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSupp_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSupp.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT * FROM VMsCustomer WHERE FgActive = 'Y'"
            ResultField = "Customer_Code, Customer_Name, Contact_Person"
            ViewState("Sender") = "btnCustomer"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Customer Click Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub tbCustCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCustCode.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Customer", tbCustCode.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbCustCode.Text = Dr("Customer_Code")
                tbCustName.Text = Dr("Customer_Name")
                BindToText(tbAttn, Dr("Contact_Person"))
            Else
                tbCustCode.Text = ""
                tbCustName.Text = ""
                tbAttn.Text = ""
            End If
        Catch ex As Exception
            Throw New Exception("tb Product change Error : " + ex.ToString)
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
            btnSaveDt2.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Dt 2 Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddDtke2.Click, btnAdddt.Click
        Try
            Cleardt()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt") = "Insert"
            lbItemNo.Text = GetNewItemNo(ViewState("Dt"))
            MovePanel(PnlDt, pnlEditDt)
            EnableHd(False)
            StatusButtonSave(False)
            btnDocNo.Visible = tbFgMode.Text = "D"
            tbDocumentNo.Enabled = Not tbFgMode.Text = "D"
            ddlReceiptType.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlCurr_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCurr.SelectedIndexChanged
        Try
            If ViewState("InputCurrency") = "Y" Then
                'RefreshMaster("S_GetCurrency", "Currency", "Currency", ddlChargeCurr, ViewState("DBConnection"))
                RefreshMaster("S_GetCurrency", "Currency", "Currency", ddlCurr, ViewState("DBConnection"))
                ViewState("InputCurrency") = Nothing
            End If
            ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"), ViewState("DigitHome"))
            If tbFgMode.Text = "D" Then
                tbRate.Enabled = False
            End If
            tbRate.Focus()
        Catch ex As Exception
            lbStatus.Text = "ddl Curr selected index Changed : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlCurrDt2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCurrDt2.SelectedIndexChanged
        Try
            If ViewState("InputCurrency") = "Y" Then
                RefreshMaster("S_GetCurrency", "Currency", "Currency", ddlCurrDt2, ViewState("DBConnection"))
                ViewState("InputCurrency") = Nothing
            End If
            ChangeCurrency(ddlCurrDt2, tbDate, tbRateDt2, ViewState("Currency"), ViewState("DigitCurrInv"), ViewState("DBConnection"), ViewState("DigitHome"))
            tbRateDt2.Focus()
        Catch ex As Exception
            lbStatus.Text = "ddl Curr dt2 selected index Changed : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlReceiptType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlReceiptType.SelectedIndexChanged
        Try
            Dim VoucherNo As String

            ChangePaymentTypeV2(ddlReceiptType.SelectedValue, tbFgMode, tbDate, tbDueDateDt2, ddlBankGiro, ddlCurr, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
            FillCombo(ddlBankGiro, "SELECT Bank_Code, Bank_Name FROM VMsBankReceipt WHERE Currency = " + QuotedStr(ddlCurr.SelectedValue), True, "Bank_Code", "Bank_Name", ViewState("DBConnection"))
            'ViewState("DigitChargeCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlChargeCurr.SelectedValue), ViewState("DBConnection"))
            ViewState("DigitCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlCurr.SelectedValue), ViewState("DBConnection"))
            'AttachScript("kali(" + Me.tbRate.ClientID + "," + Me.tbReceiptForex.ClientID + "," + Me.tbReceiptHome.ClientID + "); setformatdt();", Page, Me.GetType())
            tbReceiptHome.Text = FormatFloat((CFloat(tbRate.Text) * CFloat(tbReceiptForex.Text)), ViewState("DigitHome"))
            AttachScript("setformatdt();", Page, Me.GetType())
            tbRate.Text = FormatFloat(tbRate.Text.Replace(",", ""), ViewState("DigitRate"))
            btnDocNo.Visible = tbFgMode.Text = "D"
            tbDocumentNo.Enabled = Not tbFgMode.Text = "D"
            VoucherNo = ""
            If tbFgMode.Text = "B" Or tbFgMode.Text = "K" Then
                VoucherNo = SQLExecuteScalar("Declare @A VarChar(255) EXEC S_SAAutoVoucherNmbr " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr("Y") + ", " + QuotedStr(ddlReceiptType.SelectedValue) + ", 'IN', @A OUT SELECT @A", ViewState("DBConnection").ToString) 'ddlReport.SelectedValue
            End If
            tbVoucherNo.Enabled = (tbFgMode.Text = "B" Or tbFgMode.Text = "K")
            tbVoucherNo.Text = VoucherNo
            btnSaveDt.Focus()
        Catch ex As Exception
            lbStatus.Text = "ddl receipt type Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub ddlReport_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlReport.SelectedIndexChanged
    '    FillCombo(ddlReceiptType, "EXEC S_GetPayTypeUser " + QuotedStr("ReceiptTrade" + ddlReport.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString), True, "Payment_Code", "Payment_Name", ViewState("DBConnection"))
    'End Sub

    Protected Sub btnInvNo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnInvNo.Click
        Dim ResultField, CriteriaField, sqlstring As String
        Try            
            'If lbFgModeDt2.Text = "G" Then
            '    If tbCustCode.Text.Trim = "" Then                    
            '        sqlstring = "SELECT * FROM V_FNARPostingForReceipt WHERE Invoice_No IS NOT NULL " 'ddlReport.SelectedValue
            '    Else                    
            '        sqlstring = "SELECT * FROM V_FNARPostingForReceiptM WHERE Bill_To = " + QuotedStr(tbCustCode.Text) 'ddlReport.SelectedValue
            '    End If
            'ElseIf tbCustCode.Text.Trim = "" Then                    
            '    sqlstring = "SELECT * FROM V_FNARPostingForReceipt WHERE Invoice_No IS NOT NULL " 'ddlReport.SelectedValue
            'Else
            '    sqlstring = "SELECT * FROM V_FNARPostingForReceiptM WHERE Bill_To = " + QuotedStr(tbCustCode.Text)
            'End If

            sqlstring = "EXEC S_FNReceiptTradeGetInv '" + tbCustCode.Text.Trim + "', " + QuotedStr(ViewState("TransNmbr"))
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = sqlstring
            ResultField = "Invoice_No, Currency, Forex_Rate, Base_Forex, PPN_Forex, Base_Paid, PPN_Paid, PPN_Rate, Due_Date, FgValue, Buyer_Code, Buyer_Name, Bill_To, Bill_To_Name"
            CriteriaField = "Invoice_No, Bill_To, Bill_To_Name, Currency"
            Session("CriteriaField") = CriteriaField.Split(",")
            ViewState("Sender") = "btnInvNo"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Customer Click Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub ddlChargeCurr_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlChargeCurr.SelectedIndexChanged
    '    Try
    '        If ViewState("InputCurrency") = "Y" Then
    '            RefreshMaster("S_GetCurrency", "Currency", "Currency", ddlCurr, ViewState("DBConnection"))
    '            RefreshMaster("S_GetCurrency", "Currency", "Currency", ddlChargeCurr, ViewState("DBConnection"))
    '            ViewState("InputCurrency") = Nothing
    '        End If
    '        ChangeCurrency(ddlChargeCurr, tbDate, tbChargeRate, ViewState("Currency"), ViewState("DigitChargeCurr"), ViewState("DBConnection"))
    '        If ddlChargeCurr.SelectedValue = "" Then
    '            tbChargeForex.Text = "0"
    '            tbChargeHome.Text = "0"
    '        End If
    '        tbChargeForex.Enabled = ddlChargeCurr.SelectedValue <> ""

    '        tbChargeRate.Focus()
    '        AttachScript("setformatdt();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lbStatus.Text = "ddl Curr selected index Changed : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub btnDocNo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDocNo.Click
        Dim ResultField As String
        Try
            If tbCustCode.Text.Trim = "" Then
                'If ddlReport.SelectedValue = "*" Then
                'Session("filter") = "SELECT DP_No, DP_Date, Customer_Code, Customer_Name, Currency, Rate, Total_Forex, Total_Paid, Balance_Forex FROM V_FNDPCustPending WHERE Report is NOT NULL "
                'Else
                Session("filter") = "SELECT DP_No, DP_Date, Customer_Code, Customer_Name, Currency, Rate, Total_Forex, Total_Paid, Balance_Forex FROM V_FNDPCustPending WHERE DP_No IS NOT NULL "
                'End If
            Else
                'If ddlReport.SelectedValue = "*" Then
                'Session("filter") = "SELECT DP_No, DP_Date, Customer_Code, Customer_Name, Currency, Rate, Total_Forex, Total_Paid, Balance_Forex FROM V_FNDPCustPending " + _
                '"WHERE Customer_Code =" + QuotedStr(tbCustCode.Text)
                'Else
                Session("filter") = "SELECT DP_No, DP_Date, Customer_Code, Customer_Name, Currency, Rate, Total_Forex, Total_Paid, Balance_Forex FROM V_FNDPCustPending " + _
                "WHERE Customer_Code =" + QuotedStr(tbCustCode.Text)
                'End If
            End If
            Session("DBConnection") = ViewState("DBConnection")
            ResultField = "DP_No, Currency, Rate, Total_Forex, Total_Paid, Customer_Code, Customer_Name"
            ViewState("Sender") = "btnDocNo"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Doc No Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnBackDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBackDt2.Click, btnBackDt2Ke2.Click
        MultiView1.ActiveViewIndex = 0
    End Sub

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
            If tbCustCode.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Customer must have value")
                tbCustCode.Focus()
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
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
            lbStatus.Text = "btn saveall Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbInvNoDt2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbInvNoDt2.TextChanged
        Dim Dr As DataRow
        Dim Dt As DataTable
        Dim SQLString As String
        Try
            If lbFgModeDt2.Text = "G" Then
                If tbCustCode.Text = "" Then
                    SQLString = "SELECT * FROM V_FNARPostingForReceipt WHERE Invoice_No = " + QuotedStr(tbInvNoDt2.Text)
                Else
                    SQLString = "SELECT * FROM V_FNARPostingForReceipt WHERE Bill_To = " + QuotedStr(tbCustCode.Text) + " and Invoice_No = " + QuotedStr(tbInvNoDt2.Text)
                End If
            Else
                If tbCustCode.Text = "" Then
                    SQLString = "SELECT * FROM V_FNARPostingForReceipt WHERE Invoice_No = " + QuotedStr(tbInvNoDt2.Text) 'ddlReport.SelectedValue
                Else
                    SQLString = "SELECT * FROM V_FNARPostingForReceipt WHERE Bill_To = " + QuotedStr(tbCustCode.Text) + " and Invoice_No = " + QuotedStr(tbInvNoDt2.Text) 'ddlReport.SelectedValue
                End If
            End If
            'Invoice_No, Currency, Forex_Rate, Base_Forex, PPN_Forex, Base_Paid, PPN_Paid, PPN_Rate, Due_Date, FgValue, Buyer_Code, Buyer_Name, Bill_To, Bill_To_Name
            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
            Else
                Dr = Nothing
            End If
            If Not Dr Is Nothing Then
                Dim amount, amountppn, balans, balansppn As Double
                Dim rate, sisainv, sisabayar, sisappn As Double
                amount = CFloat(Dr("Base_Forex").ToString)
                amountppn = CFloat(Dr("PPn_Forex").ToString)
                balans = CFloat(Dr("Base_Paid").ToString)
                balansppn = CFloat(Dr("PPn_Paid").ToString)
                rate = CFloat(Dr("Forex_Rate").ToString)

                BindToText(tbInvNoDt2, Dr("Invoice_No").ToString)
                BindToDropList(ddlCurrDt2, Dr("Currency").ToString)
                ViewState("DigitCurrInv") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlCurrDt2.SelectedValue), ViewState("DBConnection"))
                BindToText(tbRateDt2, Dr("Forex_Rate").ToString)
                BindToText(tbPPNRateDt2, Dr("PPn_Rate").ToString)
                BindToDate(tbDueDateDt2, Dr("Due_Date").ToString)
                BindToText(tbFgValueDt2, Dr("FgValue").ToString)
                BindToText(tbBuyerCode, Dr("Buyer_Code").ToString)
                BindToText(tbBuyer, Dr("Buyer_Name").ToString)
                If tbCustCode.Text.Trim = "" Then
                    BindToText(tbCustCode, Dr("Bill_To").ToString)
                    BindToText(tbCustName, Dr("Bill_To_Name").ToString)
                    tbCustCode.Enabled = False
                    btnSupp.Visible = False
                End If
                tbTotalInvoiceDt2.Text = (amount + amountppn).ToString
                TbTotalPaidDt2.Text = (balans + balansppn).ToString
                'tbTotalToPaidDt2.Text = ((amount - balans) + (amountppn - balansppn)).ToString
                tbBaseInvoiceDt2.Text = amount.ToString
                tbBasePaidDt2.Text = balans.ToString
                tbBaseToPaidDt2.Text = (amount - balans).ToString
                tbPPNInvoiceDt2.Text = amountppn.ToString
                tbPPNPaidDt2.Text = balansppn.ToString
                tbPPNToPaidDt2.Text = (amountppn - balansppn).ToString

                sisainv = ((amount - balans) + (amountppn - balansppn)) * rate
                'sisabayar = (CFloat(tbPayHomeDt.Text) + CFloat(tbChargeHomeDt.Text) - CFloat(tbInvHomeDt.Text))
                sisabayar = (CFloat(tbPayHomeDt.Text) - CFloat(tbInvHomeDt.Text))
                sisappn = (amountppn - balansppn) * rate

                If sisabayar < sisainv Then
                    tbTotalToPaidDt2.Text = (sisabayar / rate).ToString
                Else
                    tbTotalToPaidDt2.Text = (sisainv / rate).ToString
                End If

                sisabayar = CFloat(tbTotalToPaidDt2.Text) * rate
                If sisabayar < sisappn Then
                    tbPPNToPaidDt2.Text = (sisabayar / rate).ToString
                    tbBaseToPaidDt2.Text = "0"
                Else
                    tbPPNToPaidDt2.Text = (sisappn / rate).ToString
                    tbBaseToPaidDt2.Text = ((sisabayar - sisappn) / rate).ToString
                End If

                AttachScript("setformatdt2('');", Page, Me.GetType())
            Else
                tbInvNoDt2.Text = ""
                tbBuyerCode.Text = ""
                tbBuyer.Text = ""
            End If
            'tbInvoiceNo.Focus()
        Catch ex As Exception
            Throw New Exception("tb InvoiceNo Textchange Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnGetInv_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGetInv.Click
        Dim ResultField, sqlstring, ResultSame, CriteriaField As String
        Try
            'If lbFgModeDt2.Text = "G" Then
            '    If tbCustCode.Text.Trim = "" Then
            '        'If ddlReport.SelectedValue = "*" Then
            '        'sqlstring = "SELECT * FROM V_FNARPostingForReceipt " 'ini yg digunakan                    
            '        sqlstring = "SELECT * FROM V_FNARPostingForReceipt WHERE Invoice_No IS NOT NULL "

            '    Else
            '        'If ddlReport.SelectedValue = "*" Then
            '        'sqlstring = "SELECT * FROM V_FNARPostingForReceiptM WHERE Bill_To = " + QuotedStr(tbCustCode.Text) 'ini yg digunakan                    
            '        sqlstring = "SELECT * FROM V_FNARPostingForReceiptM WHERE Bill_To = " + QuotedStr(tbCustCode.Text)
            '    End If
            'Else
            '    If tbCustCode.Text.Trim = "" Then
            '        'If ddlReport.SelectedValue = "*" Then
            '        'sqlstring = "SELECT * FROM V_FNARPostingForReceipt WHERE Report IS NOT NULL "
            '        'Else
            '        sqlstring = "SELECT * FROM V_FNARPostingForReceipt WHERE Invoice_No IS NOT NULL "
            '        'End If
            '    Else
            '        'If ddlReport.SelectedValue = "*" Then
            '        'sqlstring = "SELECT * FROM V_FNARPostingForReceiptM WHERE Bill_To = " + QuotedStr(tbCustCode.Text)
            '        'Else
            '        sqlstring = "SELECT * FROM V_FNARPostingForReceiptM WHERE Bill_To = " + QuotedStr(tbCustCode.Text)
            '        'End If
            '    End If
            'End If
            sqlstring = "EXEC S_FNReceiptTradeGetInv '" + tbCustCode.Text.Trim + "' ," + QuotedStr(ViewState("TransNmbr"))
            Session("filter") = sqlstring
            ResultField = "Invoice_No, Currency, Forex_Rate, Base_Forex, PPN_Forex, Total_Forex, Base_Paid, PPN_Paid, Total_Paid, Amount_Saldo, PPN_Rate, Due_Date, FgValue, Buyer_Code, Buyer_Name, Bill_To, Bill_To_Name, DigitCurrForex"
            CriteriaField = "Invoice_No, Bill_To, Bill_To_Name, Currency"
            Session("DBConnection") = ViewState("DBConnection")
            'Session("ClickSame") = "Bill_To"
            Session("Column") = ResultField.Split(",")
            Session("CriteriaField") = CriteriaField.Split(",")
            ResultSame = "Bill_To"
            Session("ResultSame") = ResultSame.Split(",")
            ViewState("Sender") = "btnGetInv"
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Multi Invoice Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbCount_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCount.Click
        Dim ResultField, sqlstring, ResultSame, CriteriaField As String
        Try
            sqlstring = "EXEC S_FNReceiptTradeGetInv '',''" '" + QuotedStr(tbCustCode.Text.Trim) + " ," + QuotedStr(ViewState("TransNmbr"))
            Session("filter") = sqlstring
            ResultField = "Invoice_No, Currency, Forex_Rate, Base_Forex, PPN_Forex, Total_Forex, Base_Paid, PPN_Paid, Total_Paid, Amount_Saldo, PPN_Rate, Due_Date, FgValue, Buyer_Code, Buyer_Name, Bill_To, Bill_To_Name, DigitCurrForex"
            CriteriaField = "Invoice_No, Bill_To, Bill_To_Name, Currency"
            Session("DBConnection") = ViewState("DBConnection")
            'Session("ClickSame") = "Bill_To"
            Session("Column") = ResultField.Split(",")
            Session("CriteriaField") = CriteriaField.Split(",")
            ResultSame = "Bill_To"
            Session("ResultSame") = ResultSame.Split(",")
            ViewState("Sender") = "btnOut"
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Multi Invoice Click Error : " + ex.ToString
        End Try
    End Sub
    
End Class
