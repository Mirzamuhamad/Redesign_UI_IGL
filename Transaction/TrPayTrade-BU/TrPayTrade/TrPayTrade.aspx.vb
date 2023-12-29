Imports System.Data
Imports System.Data.SqlClient
Imports BasicFrame.WebControls
Partial Class Transaction_TrPayTrade_TrPayTrade
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select distinct TransNmbr, Nmbr, TransDate, Status, FgReport, SupplierCode, SupplierName, Supplier, Attn, Currency, TotalPayForexStr, TotalPayForex, TotalPayment, TotalOthers, TotalInvoice, TotalPPh, TotalCharge, TotalKurs, TotalSelisih, TotalDP, Remark From V_FNPayTradeHd "

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
                ' lblTitle.Text = Request.QueryString("MenuName").ToString
            End If
            lbStatus.Text = ""
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnSupplier" Then
                    tbSuppCode.Text = Session("Result")(0).ToString
                    tbSuppName.Text = Session("Result")(1).ToString
                    BindToText(tbAttn, Session("Result")(2).ToString)
                ElseIf ViewState("Sender") = "btnDocNo" Then
                    tbDocumentNo.Text = Session("Result")(0).ToString
                    ddlCurr.SelectedValue = Session("Result")(1).ToString
                    tbDPRate.Text = FormatFloat(Session("Result")(2).ToString, ViewState("DigitRate"))
                    ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection").ToString, ViewState("DigitHome"))
                    tbRate.Text = FormatFloat(tbRate.Text.Replace(",", ""), ViewState("DigitRate"))
                    tbPaymentForex.Text = CFloat(Session("Result")(3).ToString) - CFloat(Session("Result")(4).ToString).ToString
                    ' AttachScript("setformatdt();", Page, Me.GetType())
                    'tbRate.Enabled = False
                    If tbSuppCode.Text.Trim = "" Then
                        BindToText(tbSuppCode, Session("Result")(5).ToString)
                        BindToText(tbSuppName, Session("Result")(6).ToString)
                    End If
                    AttachScript("setformatdt();", Page, Me.GetType)
                ElseIf ViewState("Sender") = "btnInvNo" Then
                    'Invoice_No, Currency, Forex_Rate, PPN_Rate, Supplier_Invoice, Due_Date, Base_Forex, PPN_Forex, PPh_Forex, Base_Paid, PPN_Paid, PPh_Paid, PPh_Saldo, FgValue, Supplier, Supplier_Name, Original_Rate"
                    Dim amount, amountppn, balans, balansppn, amountpph, balanspph As Double
                    Dim PayRate, PPnRate, sisainv, sisabayar, sisappn, sisapph, lastInvoice As Double
                    amount = CFloat(Session("Result")(6).ToString)
                    amountppn = CFloat(Session("Result")(7).ToString)
                    amountpph = CFloat(Session("Result")(8).ToString)
                    balans = CFloat(Session("Result")(9).ToString)
                    balansppn = CFloat(Session("Result")(10).ToString)
                    balanspph = CFloat(Session("Result")(11).ToString)
                    sisapph = CFloat(Session("Result")(12).ToString)
                    'rate = CFloat(Session("Result")(2).ToString)

                    BindToText(tbInvNoDt2, Session("Result")(0).ToString)
                    BindToDropList(ddlCurrDt2, Session("Result")(1).ToString)
                    ViewState("DigitCurrInv") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlCurrDt2.SelectedValue), ViewState("DBConnection"))
                    tbRateDt2.Text = FormatFloat(Session("Result")(2).ToString, ViewState("DigitRate"))
                    tbPPNRateDt2.Text = FormatFloat(Session("Result")(3).ToString, ViewState("DigitRate"))
                    BindToText(tbSuppInvNoDt2, Session("Result")(4).ToString)
                    BindToDate(tbDueDateDt2, Session("Result")(5).ToString)
                    BindToText(tbFgValueDt2, Session("Result")(13).ToString)
                    If tbSuppCode.Text.Trim = "" Then
                        BindToText(tbSuppCode, Session("Result")(14).ToString)
                        BindToText(tbSuppName, Session("Result")(15).ToString)
                        tbSuppCode.Enabled = False
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
                    'ViewState("TotalPayDt2") = CFloat(tbPayHomeDt2.Text)
                    'ViewState("TotalInvDt2") = CFloat(tbInvHomeDt2.Text)
                    'ViewState("TotalPPhDt2") = CFloat(tbPPhHomeDt2.Text)
                    'ViewState("TotalChargeDt2") = CFloat(tbChargeHomeDt2.Text)
                    'ViewState("TotalSelisihDt2") = CFloat(tbSelisihHomeDt2.Text)
                    ViewState("TotalToPaid") = 0
                    'ViewState("PayRate") = CFloat(tbPayRateDt2.Text)

                    If (lblCurrPayDt.Text = ViewState("Currency").ToString) Then
                        tbPayRatePPn.Text = tbPPNRateDt2.Text
                    Else
                        tbPayRatePPn.Text = tbPayRateDt2.Text
                    End If
                    PayRate = CFloat(tbPayRateDt2.Text.Replace(",", ""))
                    PPnRate = CFloat(tbPPNRateDt2.Text.Replace(",", ""))

                    tbTotalInvoiceDt2.Text = FormatFloat(amount + amountppn + amountpph, ViewState("DigitCurrInv"))
                    tbBaseInvoiceDt2.Text = FormatFloat(amount, ViewState("DigitCurrInv"))
                    tbPPNInvoiceDt2.Text = FormatFloat(amountppn, ViewState("DigitCurrInv"))
                    tbPPhInvoiceDt2.Text = FormatFloat(amountpph, ViewState("DigitCurrInv"))

                    TbTotalPaidDt2.Text = FormatFloat(balans + balansppn + balanspph, ViewState("DigitCurrInv"))
                    tbBasePaidDt2.Text = FormatFloat(balans, ViewState("DigitCurrInv"))
                    tbPPNPaidDt2.Text = FormatFloat(balansppn, ViewState("DigitCurrInv"))
                    tbPPhPaidDt2.Text = FormatFloat(balanspph, ViewState("DigitCurrInv"))

                    sisainv = ((amount - balans) * PayRate) + ((amountppn - balansppn) * PPnRate)
                    sisappn = (amountppn - balansppn) * PPnRate
                    sisapph = (amountpph - balanspph) * PayRate
                    lastInvoice = 0
                    If ViewState("StateDt2") = "Edit" Then
                        lastInvoice = (CFloat(tbPayRateDt2.Text) * CFloat(tbBaseToPaidDt2.Text)) + (CFloat(tbPPNRateDt2.Text) * CFloat(tbPPNToPaidDt2.Text))
                    End If
                    If CFloat(tbFgValueDt2.Text) >= 0 Then
                        sisabayar = (CFloat(tbPayHomeDt.Text) - CFloat(tbInvHomeDt.Text) + CFloat(tbPPhHomeDt.Text)) + lastInvoice
                    Else
                        sisabayar = sisainv
                    End If
                    'diedit
                    If sisabayar >= sisapph Then
                        tbPPhToPaidDt2.Text = FormatNumber(sisapph / PayRate, 8)
                        'sisabayar = sisabayar - sisapph
                    Else
                        tbPPhToPaidDt2.Text = FormatNumber(sisabayar / PayRate, 8)
                        'sisabayar = 0
                    End If
                    If sisabayar >= sisappn Then
                        tbPPNToPaidDt2.Text = FormatNumber(sisappn / PPnRate, 8)
                        sisabayar = sisabayar - sisappn
                    Else
                        tbPPNToPaidDt2.Text = FormatNumber(sisabayar / PPnRate, 8)
                        sisabayar = 0
                    End If
                    
                    If sisabayar >= sisainv Then
                        tbBaseToPaidDt2.Text = FormatNumber(sisainv / PayRate, 8)
                    Else
                        tbBaseToPaidDt2.Text = FormatNumber(sisabayar / PayRate, 8)
                    End If
                    'lbStatus.Text = FormatNumber(sisabayar, 0)
                    'Exit Sub

                    ''tbTotalToPaidDt2.Text = FormatNumber(CFloat(tbPPNToPaidDt2.Text) + CFloat(tbBaseToPaidDt2.Text), ViewState("DigitCurrInv"))
                    AttachScript("setformatdt2('base');", Page, Me.GetType())

                ElseIf ViewState("Sender") = "btnGetInv" Then
                    Dim drResult As DataRow
                    Dim ExistRow As DataRow()

                    For Each drResult In Session("Result").Rows

                        ExistRow = ViewState("Dt2").Select("ItemNo = " + lbItemNodt2.Text + " AND InvoiceNo = " + QuotedStr(drResult("Invoice_No").ToString))
                        If ExistRow.Count = 0 Then
                            'insert
                            If tbSuppCode.Text.Trim = "" Then
                                BindToText(tbSuppCode, drResult("Supplier").ToString)
                                BindToText(tbSuppName, drResult("Supplier_Name").ToString)
                            End If
                            Dim dr As DataRow
                            dr = ViewState("Dt2").NewRow
                            dr("ItemNo") = CInt(lbItemNodt2.Text)
                            dr("InvoiceNo") = drResult("Invoice_No").ToString
                            dr("SupplierInvNo") = drResult("Supplier_Invoice").ToString
                            'dr("ProductType") = drResult("ProductType").ToString
                            dr("DueDate") = drResult("Due_Date").ToString
                            'dr("Due_Date") = drResult("Due_Date").ToString
                            dr("Currency") = drResult("Currency").ToString
                            ViewState("DigitCurrInv") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(drResult("Currency").ToString), ViewState("DBConnection"))
                            dr("ForexRate") = drResult("Forex_Rate").ToString
                            dr("PPNRate") = drResult("PPn_Rate").ToString
                            If drResult("Currency").ToString = lblCurrPayDt.Text Then
                                dr("PayRate") = lblRatePayDt.Text
                                ViewState("PayRateDtInv") = lblRatePayDt.Text
                            ElseIf drResult("Currency").ToString = ViewState("Currency") Then
                                dr("PayRate") = FormatFloat(1, ViewState("DigitRate"))
                                ViewState("PayRateDtInv") = FormatFloat(1, ViewState("DigitRate"))
                            ElseIf drResult("Currency").ToString <> ViewState("Currency") Then
                                Dim DrRate As DataRow
                                DrRate = FindMaster("Rate", drResult("Currency").ToString + "|" + Format(tbDate.SelectedDate, "yyyy-MM-dd"), ViewState("DBConnection").ToString)
                                If Not DrRate Is Nothing Then
                                    dr("PayRate") = DrRate("Rate").ToString
                                    ViewState("PayRateDtInv") = DrRate("Rate").ToString
                                Else
                                    dr("PayRate") = FormatFloat(1, ViewState("DigitRate"))
                                    ViewState("PayRateDtInv") = FormatFloat(1, ViewState("DigitRate"))
                                End If
                            End If
                            If (lblCurrPayDt.Text = ViewState("Currency").ToString) Then
                                dr("PayRatePPn") = drResult("PPn_Rate").ToString
                            Else
                                dr("PayRatePPn") = ViewState("PayRateDtInv")
                            End If

                            dr("FgValue") = drResult("FgValue").ToString
                            dr("TotalInvoice") = drResult("Total_Forex").ToString
                            dr("TotalPaid") = drResult("Total_Paid").ToString
                            dr("TotalToPaid") = drResult("Amount_Saldo").ToString
                            dr("BaseInvoice") = drResult("Base_Forex").ToString
                            dr("BasePaid") = drResult("Base_Paid").ToString
                            dr("BaseToPaid") = FormatFloat(CFloat(drResult("Base_Forex").ToString) - CFloat(drResult("Base_Paid").ToString), ViewState("DigitCurrInv"))
                            dr("PPNInvoice") = drResult("PPn_Forex").ToString
                            dr("PPNPaid") = drResult("PPN_Paid").ToString
                            dr("PPNToPaid") = FormatFloat(CFloat(drResult("PPn_Forex").ToString) - CFloat(drResult("PPN_Paid").ToString), ViewState("DigitCurrInv"))
                            dr("PPNToPaidHome") = FormatFloat(dr("PPNToPaid") * dr("PPNRate"), ViewState("DigitHome"))
                            dr("PPhInvoice") = drResult("PPh_Forex").ToString
                            dr("PPhPaid") = drResult("PPh_Paid").ToString
                            dr("PPhToPaid") = drResult("PPh_Saldo").ToString
                            dr("Remark") = ""
                            dr("Type") = drResult("Type").ToString
                            ViewState("Dt2").Rows.Add(dr)
                        End If
                    Next
                    EnableHd(GetCountRecord(ViewState("Dt2")) = 0)
                    Dim drow As DataRow()
                    drow = ViewState("Dt2").Select("ItemNo = " + lbItemNodt2.Text)
                    CountTotalInvoiceHome(lbItemNodt2.Text)
                    BindGridDt(drow.CopyToDataTable, GridDt2)
                    StatusButtonSave(True)
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
            lbTotPay.Text = "Payment (" + ViewState("Currency") + ")"
            lbTotDPForex.Text = "DP (" + ViewState("Currency") + ")"
            lbTotOther.Text = "Others (" + ViewState("Currency") + ")"
            lbTotInvoice.Text = "Invoice (" + ViewState("Currency") + ")"
            lbTotPPh.Text = "PPh (" + ViewState("Currency") + ")"
            lbTotCharge.Text = "Expense (" + ViewState("Currency") + ")"
            lbTotKurs.Text = "Selisih Kurs(" + ViewState("Currency") + ")"
            lbTotSelisih.Text = "Difference (" + ViewState("Currency") + ")"

            lbPayHome.Text = "Payment (" + ViewState("Currency") + ")"
            lbInvoiceHome.Text = "Invoice (" + ViewState("Currency") + ")"
            lbPPhHome.Text = "PPh (" + ViewState("Currency") + ")"
            'lbChargeHome.Text = "Charge (" + ViewState("Currency") + ")"
            'lbSelisihHome.Text = "Selisih Kurs(" + ViewState("Currency") + ")"

            FillCombo(ddlCurr, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
            'FillCombo(ddlChargeCurr, "EXEC S_GetCurrency", True, "Currency", "Currency", ViewState("DBConnection"))
            FillCombo(ddlCurrDt2, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
            'FillCombo(ddlProductType, "EXEC S_GetProductType", True, "Type_Code", "Type_Name", Session("DBConnection"))

            'FillCombo(ddlPaymentType, "EXEC S_GetPayTypeUser(" + QuotedStr("PaymentTrade" + ddlReport.SelectedValue) + ", " + QuotedStr(Session("UserId").ToString) + ")", True, "Payment_Code", "Payment_Name", Session("DBConnection"))
            'FillCombo(ddlBankPayment, "EXEC S_GetBankPayment", True, "Bank_Code", "Bank_Name", Session("DBConnection"))

            ViewState("SortExpression") = Nothing
            ViewState("DigitCurr") = 0
            ViewState("DigitCurrInv") = 0
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
            If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
                ddlCommand.Items.Add("Print")
                'ddlCommand.Items.Add("Print Full")
                ddlCommand2.Items.Add("Print")
                'ddlCommand2.Items.Add("Print Full")
            End If
            tbTotalPaymentForex.Attributes.Add("ReadOnly", "True")
            tbTotalDPForex.Attributes.Add("ReadOnly", "True")
            tbTotalPayment.Attributes.Add("ReadOnly", "True")
            tbTotalOther.Attributes.Add("ReadOnly", "True")
            tbTotalInvoice.Attributes.Add("ReadOnly", "True")
            tbTotalPPh.Attributes.Add("ReadOnly", "True")
            tbTotalCharge.Attributes.Add("ReadOnly", "True")
            tbTotalKurs.Attributes.Add("ReadOnly", "True")
            tbTotalSelisih.Attributes.Add("ReadOnly", "True")

            tbPaymentHome.Attributes.Add("ReadOnly", "True")
            tbInvoiceHome.Attributes.Add("ReadOnly", "True")
            tbPPhHome.Attributes.Add("ReadOnly", "True")
            'tbSelisihHome.Attributes.Add("ReadOnly", "True")
            'tbChargeHome.Attributes.Add("ReadOnly", "True")

            tbPPNRateDt2.Attributes.Add("ReadOnly", "True")
            tbTotalInvoiceDt2.Attributes.Add("ReadOnly", "True")
            TbTotalPaidDt2.Attributes.Add("ReadOnly", "True")
            tbBaseInvoiceDt2.Attributes.Add("ReadOnly", "True")
            tbBasePaidDt2.Attributes.Add("ReadOnly", "True")
            tbPPNInvoiceDt2.Attributes.Add("ReadOnly", "True")
            tbPPNPaidDt2.Attributes.Add("ReadOnly", "True")
            'tbPPNToPaidDt2.Attributes.Add("ReadOnly", "True")
            tbPPhInvoiceDt2.Attributes.Add("ReadOnly", "True")
            tbPPhPaidDt2.Attributes.Add("ReadOnly", "True")

            tbTotalPaymentForex.Attributes.Add("OnKeyDown", "return PressNumeric2();")
            'tbTotalInvoiceDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")
            'TbTotalPaidDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbTotalToPaidDt2.Attributes.Add("OnKeyDown", "return PressNumeric2();")
            'tbBaseInvoiceDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")
            'tbBasePaidDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbBaseToPaidDt2.Attributes.Add("OnKeyDown", "return PressNumeric2();")
            'tbPPNInvoiceDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")
            'tbPPNPaidDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbPPNToPaidDt2.Attributes.Add("OnKeyDown", "return PressNumeric2();")
            tbPayRateDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")

            'tbPPhInvoiceDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")
            'tbPPhPaidDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbPPhToPaidDt2.Attributes.Add("OnKeyDown", "return PressNumeric2();")

            tbPaymentForex.Attributes.Add("OnKeyDown", "return PressNumericMinus();")
            'tbChargeForex.Attributes.Add("OnKeyDown", "return PressNumeric();")

            tbTotalPaymentForex.Attributes.Add("OnBlur", "setformat();")
            tbTotalPayment.Attributes.Add("OnBlur", "setformat();")
            tbTotalDPForex.Attributes.Add("OnBlur", "setformat();")
            tbTotalOther.Attributes.Add("OnBlur", "setformat();")
            tbTotalInvoice.Attributes.Add("OnBlur", "setformat();")
            tbTotalPPh.Attributes.Add("OnBlur", "setformat();")
            tbTotalCharge.Attributes.Add("OnBlur", "setformat();")
            tbTotalKurs.Attributes.Add("OnBlur", "setformat();")
            tbTotalSelisih.Attributes.Add("OnBlur", "setformat();")

            tbPaymentForex.Attributes.Add("OnBlur", "setformatdt();")
            tbPaymentHome.Attributes.Add("OnBlur", "setformatdt();")
            tbInvoiceHome.Attributes.Add("OnBlur", "setformatdt();")
            'tbPPhHome.Attributes.Add("OnBlur", "setformatdt();")
            'tbSelisihHome.Attributes.Add("OnBlur", "setformatdt();")
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
            tbPPhInvoiceDt2.Attributes.Add("OnBlur", "setformatdt2('pph');")
            tbPPhPaidDt2.Attributes.Add("OnBlur", "setformatdt2('pph');")
            tbPPhToPaidDt2.Attributes.Add("OnBlur", "setformatdt2('pph');")
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
                'GridView1.HeaderRow.Cells(8).Text = "Payment Forex"
                'GridView1.HeaderRow.Cells(9).Text = "Payment (" + ViewState("Currency") + ")"
                'GridView1.HeaderRow.Cells(10).Text = "Other (" + ViewState("Currency") + ")"
                'GridView1.HeaderRow.Cells(11).Text = "Invoice (" + ViewState("Currency") + ")"
                'GridView1.HeaderRow.Cells(10).Text = "PPh (" + ViewState("Currency") + ")"
                'GridView1.HeaderRow.Cells(11).Text = "Charge (" + ViewState("Currency") + ")"
                'GridView1.HeaderRow.Cells(12).Text = "Selisih Kurs (" + ViewState("Currency") + ")"
            End If
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub
    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT * From V_FNPayTradeDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function
    Private Function GetStringDt2(ByVal Nmbr As String) As String
        Return "SELECT * From V_FNPayTradeInv WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

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
            If (ActionValue = "Print") Or (ActionValue = "Print Full") Then
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

                Session("SelectCommand") = "EXEC S_FNFormVoucher" + Result + ",'PAP'," + QuotedStr(ViewState("UserId"))
                Session("ReportFile") = ".../../../Rpt/FormVoucher.frx"
                'lbStatus.Text = Session("SelectCommand")
                'Exit Sub

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
                        Result = ExecSPCommandGo(ActionValue, "S_FNPayTrade", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
        'Dim Count As Integer
        Try
            'ddlReport.Enabled = State And ViewState("StateHd") = "Insert"
            'Count = GetCountRecord(ViewState("Dt2"))
            tbSuppCode.Enabled = State  'ViewState("StateHd") = "Insert" And Count = 0
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
            GridDt.HeaderRow.Cells(13).Text = "Payment (" + ViewState("Currency") + ")"
            GridDt.HeaderRow.Cells(14).Text = "Invoice (" + ViewState("Currency") + ")"
            GridDt.HeaderRow.Cells(15).Text = "PPh (" + ViewState("Currency") + ")"
            'GridDt.HeaderRow.Cells(16).Text = "Charge (" + ViewState("Currency") + ")"
            'GridDt.HeaderRow.Cells(17).Text = "Selisih Kurs (" + ViewState("Currency") + ")"
            'End If
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub
    Private Sub BindDataDt2(ByVal Nmbr As String)
        Try
            Dim dt As New DataTable
            'Dim dr As DataRow
            ViewState("Dt2") = Nothing
            dt = SQLExecuteQuery(GetStringDt2(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt2") = dt                                    
            BindGridDt(dt, GridDt2)
            GridDt2.HeaderRow.Cells(16).Text = "PPn To Paid (" + ViewState("Currency") + ")"
            'If dt.Rows.Count > 0 Then

            'End If
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
            tbSuppCode.Text = ""
            tbSuppName.Text = ""
            'ViewState("ProductType") = ""
            'tbProductType.Text = ""
            'ddlReport.SelectedValue = "Y"
            tbDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            tbTotalPaymentForex.Text = FormatFloat(0, ViewState("DigitCurr"))
            tbTotalPayment.Text = "0"
            tbTotalDPForex.Text = "0"
            tbTotalOther.Text = "0"
            tbTotalInvoice.Text = "0"
            tbTotalPPh.Text = "0"
            tbTotalCharge.Text = "0"
            tbTotalKurs.Text = "0"
            tbTotalSelisih.Text = "0"
            tbAttn.Text = ""
            tbRemark.Text = ""
            tbDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            FillCombo(ddlPaymentType, "EXEC S_GetPayTypeUser " + QuotedStr("PaymentTrade" + "Y") + ", " + QuotedStr(ViewState("UserId").ToString), True, "Payment_Code", "Payment_Name", ViewState("DBConnection")) 'ddlReport.SelectedValue
            FillCombo(ddlBankPayment, "EXEC S_GetBankPaymentTrade " + QuotedStr(ddlCurr.SelectedValue), True, "Bank_Code", "Bank_Name", ViewState("DBConnection")) 'ddlReport.SelectedValue
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
           
            ddlPaymentType.SelectedIndex = 0
            ddlBankPayment.SelectedIndex = 0
            tbRemarkDt.Text = ""
            tbPaymentDate.SelectedDate = tbDate.SelectedDate
            tbDueDate.SelectedDate = Nothing
            tbGiroDate.SelectedDate = Nothing
            tbDocumentNo.Text = ""
            tbVoucherNo.Enabled = False
            tbVoucherNo.Text = ""            
            'tbPaymentForex.Text = "0"

            'tbSelisihHome.Text = FormatFloat(0, ViewState("DigitCurr"))
            'tbChargeRate.Text = FormatFloat(0, ViewState("DigitCurr"))
            'tbChargeForex.Text = FormatFloat(0, ViewState("DigitCurr"))
            'tbChargeHome.Text = FormatFloat(0, ViewState("DigitCurr"))
            tbRemarkDt.Text = tbRemark.Text
            ddlCurr.SelectedValue = ViewState("Currency")
            'ddlChargeCurr.SelectedValue = ViewState("Currency")
            ChangePaymentType2(ddlPaymentType.SelectedValue, tbFgMode, tbDate, tbDueDate, ddlBankPayment, ddlCurr, tbRate, ViewState("Currency"), ViewState("DBConnection"))
            FillCombo(ddlBankPayment, "EXEC S_GetBankPaymentTrade " + QuotedStr(ddlCurr.SelectedValue), True, "Bank_Code", "Bank_Name", ViewState("DBConnection")) 'ddlReport.SelectedValue
            'ChangeCurrency(ddlCurr, tbPaymentDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection").ToString, ViewState("DigitHome"))
            tbRate.Text = FormatFloat(tbRate.Text.Replace(",", ""), ViewState("DigitRate"))
            tbDPRate.Text = tbRate.Text

            If CFloat(tbTotalInvoice.Text) > (CFloat(tbTotalPayment.Text) + CFloat(tbTotalDPForex.Text) - CFloat(tbTotalOther.Text)) Then
                tbPaymentForex.Text = FormatFloat((CFloat(tbTotalInvoice.Text) - (CFloat(tbTotalPayment.Text) + CFloat(tbTotalDPForex.Text) - CFloat(tbTotalOther.Text))) / CFloat(tbRate.Text), ViewState("DigitCurr"))
            Else
                tbPaymentForex.Text = FormatFloat(0, ViewState("DigitCurr"))
            End If
            tbPaymentHome.Text = FormatFloat(CFloat(tbPaymentForex.Text) * CFloat(tbRate.Text), ViewState("DigitHome"))
            tbInvoiceHome.Text = FormatFloat(0, ViewState("DigitHome"))
            tbPPhHome.Text = FormatFloat(0, ViewState("DigitHome"))

            tbGiroDate.Enabled = tbDueDate.Enabled
            If tbGiroDate.Enabled = False Then
                tbGiroDate.SelectedDate = Nothing
            Else
                tbGiroDate.SelectedDate = tbPaymentDate.SelectedDate
            End If

            If tbFgMode.Text = "D" Then
                lbDPRate.Visible = True
                tbDPRate.Visible = True
            Else
                lbDPRate.Visible = False
                tbDPRate.Visible = False
            End If
            'AttachScript("setformatdt();", Page, Me.GetType)
            'ChangePaymentType(ddlPaymentType.SelectedValue, tbFgMode, tbPaymentDate, tbGiroDate, ddlBankPayment, ddlCurr, ddlChargeCurr, tbRate, tbChargeRate, tbChargeForex, Session("Currency"), ViewState("DigitCurr"), Session("DBConnection"))
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt2()
        Try
            tbSuppInvNoDt2.Text = ""
            tbInvNoDt2.Text = ""
            tbDueDateDt2.SelectedDate = Nothing
            tbPPNRateDt2.Text = "0"
            tbPayRateDt2.Text = "0"
            tbTotalInvoiceDt2.Text = "0"
            TbTotalPaidDt2.Text = "0"
            tbTotalToPaidDt2.Text = "0"
            tbBaseInvoiceDt2.Text = "0"
            tbBasePaidDt2.Text = "0"
            tbBaseToPaidDt2.Text = "0"
            tbPPNInvoiceDt2.Text = "0"
            tbPPNPaidDt2.Text = "0"
            tbPPNToPaidDt2.Text = "0"
            tbPPhInvoiceDt2.Text = "0"
            tbPPhPaidDt2.Text = "0"
            tbPPhToPaidDt2.Text = "0"
            tbFgValueDt2.Text = "1"
            ddlCurrDt2.SelectedValue = ViewState("Currency")
            ChangeCurrency(ddlCurrDt2, tbDate, tbRateDt2, ViewState("Currency"), ViewState("DigitCurrInv"), ViewState("DBConnection"), ViewState("DigitHome"))
            tbRemarkDt2.Text = ""
            tbType.Text = ""
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
                If ddlPaymentType.SelectedValue = "" Then
                    lbStatus.Text = MessageDlg("Payment Type Must Have Value")
                    ddlPaymentType.Focus()
                    Return False
                End If
                If tbPaymentDate.SelectedDate = Nothing Then
                    lbStatus.Text = MessageDlg("PaymentDate Date Must Have Value")
                    tbPaymentDate.Focus()
                    Return False
                End If
                If tbFgMode.Text = "G" Then
                    If tbDocumentNo.Text.Trim = "" Then
                        lbStatus.Text = MessageDlg("Document No Must Have Value")
                        tbDocumentNo.Focus()
                        Return False
                    End If
                    If ddlBankPayment.SelectedValue = "" Then
                        lbStatus.Text = MessageDlg("Bank Payment Must Have Value")
                        ddlBankPayment.Focus()
                        Return False
                    End If
                    If tbDueDate.SelectedDate = Nothing Then
                        lbStatus.Text = MessageDlg("Due Date Must Have Value")
                        tbDueDate.Focus()
                        Return False
                    End If
                    If tbGiroDate.SelectedDate = Nothing Then
                        lbStatus.Text = MessageDlg("Giro Date Must Have Value")
                        tbGiroDate.Focus()
                        Return False
                    End If
                    'If ddlProductType.SelectedValue = "" Then
                    '    lbStatus.Text = MessageDlg("Product Type Must Have Value")
                    '    ddlProductType.Focus()
                    '    Return False
                    'End If
                End If

                'If CFloat(tbPaymentForex.ToString) <= 0 Then
                '    lbStatus.Text = MessageDlg("Amount Forex Must Have Value")
                '    tbPaymentForex.Focus()
                '    Return False
                'End If
            End If
            If tbFgMode.Text = "D" Then
                If tbDocumentNo.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("DP No Must Have Value")
                    tbDocumentNo.Focus()
                    Return False
                End If
            End If
            If tbFgMode.Text = "B" Or tbFgMode.Text = "K" Then
                If tbVoucherNo.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Voucher No Must Have Value")
                    tbVoucherNo.Focus()
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
                If CFloat(tbTotalToPaidDt2.Text) = 0 Then
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

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveDt.Click
        Try
            If CekDt() = False Then
                btnSaveDt.Focus()
                Exit Sub
            End If
            If tbFgMode.Text = "G" Then
                If CekExistGiroOut(tbDocumentNo.Text.Trim, ViewState("DBConnection").ToString) = True Then
                    lbStatus.Text = "Giro Payment '" + tbDocumentNo.Text.Trim + "' has already exists in Giro Listing'"
                    Exit Sub
                End If
            End If
            If ViewState("StateDt") = "Edit" Then
                Dim Row As DataRow

                Row = ViewState("Dt").Select("ItemNo = " + lbItemNo.Text)(0)

                Row.BeginEdit()
                Row("PaymentType") = ddlPaymentType.SelectedValue
                Row("PaymentName") = ddlPaymentType.SelectedItem.Text
                Row("PaymentDate") = Format(tbPaymentDate.SelectedDate, "yyyy/MM/dd")
                Row("DocumentNo") = tbDocumentNo.Text
                Row("Reference") = tbVoucherNo.Text
                If tbGiroDate.Enabled Then
                    Row("GiroDate") = Format(tbGiroDate.SelectedDate, "dd MMM yyyy")
                Else
                    Row("GiroDate") = DBNull.Value
                End If
                If tbDueDate.Enabled Then
                    Row("DueDate") = Format(tbDueDate.SelectedDate, "dd MMM yyyy")
                Else
                    Row("DueDate") = DBNull.Value
                End If

                Row("BankPayment") = ddlBankPayment.SelectedValue
                If ddlBankPayment.SelectedValue = "" Then
                    Row("BankPaymentName") = ""
                Else
                    Row("BankPaymentName") = ddlBankPayment.SelectedItem.Text
                End If
                Row("Currency") = ddlCurr.SelectedValue
                'Row("ForexRate") = FormatNumber(tbRate.Text.Replace(",", ""), ViewState("DigitRate"))
                Row("ForexRate") = tbRate.Text
                Row("PaymentForex") = tbPaymentForex.Text
                Row("PaymentHome") = tbPaymentHome.Text
                Row("InvoiceHome") = tbInvoiceHome.Text
                Row("PPhHome") = tbPPhHome.Text
                'Row("SelisihHome") = tbSelisihHome.Text
                'Row("ChargeCurrency") = ddlChargeCurr.SelectedValue
                'Row("ChargeRate") = tbChargeRate.Text
                'Row("ChargeForex") = tbChargeForex.Text
                'Row("ChargeHome") = tbChargeHome.Text
                Row("Remark") = tbRemarkDt.Text
                Row("FgMode") = tbFgMode.Text
                'Row("ProductType") = ddlProductType.SelectedValue
                'Row("Type_Name") = ddlProductType.SelectedItem
                Row("DPRate") = tbDPRate.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow

                dr = ViewState("Dt").NewRow
                dr("ItemNo") = lbItemNo.Text
                dr("PaymentType") = ddlPaymentType.SelectedValue
                dr("PaymentName") = ddlPaymentType.SelectedItem.Text
                dr("PaymentDate") = Format(tbPaymentDate.SelectedDate, "yyyy/MM/dd")
                dr("DocumentNo") = tbDocumentNo.Text
                dr("Reference") = tbVoucherNo.Text
                If tbGiroDate.Enabled Then
                    dr("GiroDate") = Format(tbGiroDate.SelectedDate, "dd MMM yyyy")
                Else
                    dr("GiroDate") = DBNull.Value
                End If
                If tbDueDate.Enabled Then
                    dr("DueDate") = Format(tbDueDate.SelectedDate, "dd MMM yyyy")
                Else
                    dr("DueDate") = DBNull.Value
                End If
                dr("BankPayment") = ddlBankPayment.SelectedValue
                If ddlBankPayment.SelectedValue = "" Then
                    dr("BankPaymentName") = ""
                Else
                    dr("BankPaymentName") = ddlBankPayment.SelectedItem.Text
                End If
                dr("Currency") = ddlCurr.SelectedValue
                'dr("ForexRate") = FormatNumber(tbRate.Text.Replace(",", ""), ViewState("DigitRate"))
                dr("ForexRate") = tbRate.Text
                dr("PaymentForex") = tbPaymentForex.Text
                dr("PaymentHome") = tbPaymentHome.Text
                dr("InvoiceHome") = tbInvoiceHome.Text
                dr("PPhHome") = tbPPhHome.Text
                'dr("SelisihHome") = tbSelisihHome.Text
                'dr("ChargeCurrency") = ddlChargeCurr.SelectedValue
                'dr("ChargeRate") = tbChargeRate.Text
                'dr("ChargeForex") = tbChargeForex.Text
                'dr("ChargeHome") = tbChargeHome.Text
                dr("Remark") = tbRemarkDt.Text
                dr("FgMode") = tbFgMode.Text
                'dr("ProductType") = ddlProductType.SelectedValue
                'dr("Type_Name") = ddlProductType.SelectedItem
                dr("DPRate") = tbDPRate.Text
                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, PnlDt)
            EnableHd(GetCountRecord(ViewState("Dt2")) = 0)
            UpdateDtInvoice(lbItemNo.Text, ddlCurr.SelectedValue, tbRate.Text.Replace(",", ""))
            CountTotalInvoiceHome(lbItemNo.Text)
            CountTotalDt()
            BindGridDt(ViewState("Dt"), GridDt)
            StatusButtonSave(True)
            GridDt.HeaderRow.Cells(13).Text = "Payment (" + ViewState("Currency") + ")"
            GridDt.HeaderRow.Cells(14).Text = "Invoice (" + ViewState("Currency") + ")"
            GridDt.HeaderRow.Cells(15).Text = "PPh (" + ViewState("Currency") + ")"
            'GridDt.HeaderRow.Cells(16).Text = "Charge (" + ViewState("Currency") + ")"
            'GridDt.HeaderRow.Cells(17).Text = "Selisih Kurs (" + ViewState("Currency") + ")"

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
            ExistRow = ViewState("Dt2").Select("ItemNo = " + lbItemNodt2.Text + " AND InvoiceNo = " + QuotedStr(tbInvNoDt2.Text))
            If ViewState("StateDt2") = "Edit" Then
                Dim Row As DataRow

                If ExistRow.Count > AllowedRecordDt2() Then
                    lbStatus.Text = MessageDlg("Data Exist, Cannot Edit Data")
                    Exit Sub
                End If

                Row = ViewState("Dt2").Select("ItemNo = " + lbItemNodt2.Text + " AND InvoiceNo = " + QuotedStr(ViewState("InvoiceNo")))(0)

                Row.BeginEdit()
                Row("InvoiceNo") = tbInvNoDt2.Text
                Row("SupplierInvNo") = tbSuppInvNoDt2.Text
                Row("DueDate") = Format(tbDueDateDt2.SelectedDate, "dd MMMM yyyy")
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
                Row("PPhInvoice") = tbPPhInvoiceDt2.Text
                Row("PPhPaid") = tbPPhPaidDt2.Text
                Row("PPhToPaid") = tbPPhToPaidDt2.Text
                Row("Remark") = tbRemarkDt2.Text
                Row("Type") = tbType.Text
                'Row("ProductType") = tbProductType.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                If ExistRow.Count > 0 Then
                    lbStatus.Text = MessageDlg("Data Exist, Cannot Insert Data")
                    Exit Sub
                End If
                dr = ViewState("Dt2").NewRow
                dr("ItemNo") = lbItemNodt2.Text
                dr("InvoiceNo") = tbInvNoDt2.Text
                dr("SupplierInvNo") = tbSuppInvNoDt2.Text
                dr("DueDate") = Format(tbDueDateDt2.SelectedDate, "dd MMMM yyyy")
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
                dr("PPhInvoice") = tbPPhInvoiceDt2.Text
                dr("PPhPaid") = tbPPhPaidDt2.Text
                dr("PPhToPaid") = tbPPhToPaidDt2.Text
                dr("Remark") = tbRemarkDt2.Text
                dr("Type") = tbType.Text
                'dr("ProductType") = tbProductType.Text
                ViewState("Dt2").Rows.Add(dr)
            End If
            'ViewState("ProductType") = tbProductType.Text
            MovePanel(pnlEditDt2, pnlDt2)
            EnableHd(GetCountRecord(ViewState("Dt2")) = 0)
            Dim drow As DataRow()
            drow = ViewState("Dt2").Select("ItemNo = " + lbItemNodt2.Text)
            CountTotalInvoiceHome(lbItemNodt2.Text)
            BindGridDt(drow.CopyToDataTable, GridDt2)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save dt2 Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub

    Private Sub CountTotalInvoiceHome(ByVal Item As String)
        Dim HasilKurs, HasilInv, HasilPPh, Base, PPN, PPh, RateInv, RatePPn, RatePay, PayRate, PayRatePPn, FgValue, HasilKursDP, RateOri, RateDP, PayForex As Double
        Dim Dr As DataRow
        Dim drow As DataRow()
        Dim havedetail As Boolean
        Try
            drow = ViewState("Dt2").Select("ItemNo = " + Item)
            HasilInv = 0
            HasilPPh = 0
            If drow.Length > 0 Then
                havedetail = False
                For Each Dr In drow.CopyToDataTable.Rows
                    If Not Dr.RowState = DataRowState.Deleted Then
                        FgValue = CFloat(Dr("FgValue").ToString)
                        Base = CFloat(Dr("BaseToPaid").ToString)
                        PPN = CFloat(Dr("PPNToPaid").ToString)
                        PPh = CFloat(Dr("PPhToPaid").ToString)
                        'RateNew = CFloat(Dr("PayRate").ToString)
                        RateInv = CFloat(Dr("ForexRate").ToString)
                        RatePPn = CFloat(Dr("PPNRate").ToString)
                        RatePay = CFloat(Dr("PayRate").ToString)
                        ' ini yg diubah : HasilInv = HasilInv + ((((Base + PPh) * RateNew) + (PPN * RatePPn)) * FgValue)
                        'HasilInv = HasilInv + ((((Base + PPh) * RatePay) + (PPN * RatePay)) * FgValue)--salah nih, ppn harusnya dikali ppnrate
                        HasilInv = HasilInv + ((((Base + PPh) * RateInv) + (PPN * RatePPn)) * FgValue)
                        HasilPPh = HasilPPh + ((PPh * RateInv) * FgValue)
                        'If RatePay = 0 Then
                        'ini yg diubah HasilSR = HasilSR - ((Base + PPh) * (RateNew - RateOri))
                        'HasilSR = HasilSR - ((Base + PPh + PPN) * (RateNew - RateOri))
                        'Else
                        'ini yg diubah HasilSR = HasilSR + ((Base + PPh) * (RatePay - RateNew))
                        'HasilSR = HasilSR + ((Base + PPh) * (RatePay - RateNew)) + (PPN * (RatePay - RatePPn))                        
                        'End If
                        havedetail = True
                    End If
                Next
                If havedetail = False Then
                    'tbProductType.Text = ""
                    'ViewState("ProductType") = ""
                End If
            End If
            Dr = ViewState("Dt").Select("ItemNo=" + Item)(0)
            Dr.BeginEdit()
            Dr("InvoiceHome") = FormatNumber(HasilInv, ViewState("DigitHome"))
            Dr("PPhHome") = FormatNumber(HasilPPh, ViewState("DigitHome"))
            'Dr("SelisihHome") = FormatNumber(HasilSR, ViewState("DigitHome"))
            Dr.EndEdit()
            BindGridDt(ViewState("Dt"), GridDt)

            HasilInv = 0
            HasilPPh = 0
            HasilKurs = 0
            For Each Dr In ViewState("Dt2").Rows
                If Not Dr.RowState = DataRowState.Deleted Then
                    FgValue = CFloat(Dr("FgValue").ToString)
                    Base = CFloat(Dr("BaseToPaid").ToString)
                    PPN = CFloat(Dr("PPNToPaid").ToString)
                    RateInv = CFloat(Dr("ForexRate").ToString)
                    PayRate = CFloat(Dr("PayRate").ToString)
                    RatePPn = CFloat(Dr("PPNRate").ToString)
                    PayRatePPn = CFloat(Dr("PayRatePPn").ToString)
                    'Hasil = Hasil + (((Base * PayRate) + (PPN * PPNRate)) * FgValue) --kohjim suruh ganti 20151015 krn ada selisih kurs
                    HasilInv = HasilInv + ((((Base + PPh) * RateInv) + (PPN * RatePPn)) * FgValue)
                    HasilPPh = HasilPPh + ((PPh * RateInv) * FgValue)
                    HasilKurs = HasilKurs + (((Base * (PayRate - RateInv)) + (PPN * (PayRatePPn - RatePPn))) * FgValue)
                End If
            Next

            HasilKursDP = 0
            For Each Dr In ViewState("Dt").Rows
                If Not Dr.RowState = DataRowState.Deleted Then
                    RateOri = CFloat(Dr("ForexRate").ToString)
                    RateDP = CFloat(Dr("DPRate").ToString)
                    PayForex = CFloat(Dr("PaymentForex").ToString)
                    HasilKursDP = HasilKursDP + ((RateDP - RateOri) * PayForex)
                End If
            Next

            tbTotalInvoice.Text = FormatNumber(HasilInv, ViewState("DigitHome"))
            tbTotalPPh.Text = FormatNumber(HasilPPh, ViewState("DigitHome"))
            tbTotalKurs.Text = FormatNumber(HasilKurs + HasilKursDP, ViewState("DigitHome"))
            tbTotalSelisih.Text = FormatNumber(CFloat(tbTotalPayment.Text) + CFloat(tbTotalDPForex.Text) + CFloat(tbTotalOther.Text) + CFloat(tbTotalPPh.Text) - CFloat(tbTotalCharge.Text) - CFloat(tbTotalInvoice.Text) - CFloat(tbTotalKurs.Text), ViewState("DigitHome"))
        Catch ex As Exception
            Throw New Exception("Count Total Invoice Home Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub CountTotalDt()
        Dim infois, peimen, PPh, cas, other, payforex, DP As Double
        Dim Dr As DataRow
        Try
            infois = 0
            peimen = 0
            cas = 0
            PPh = 0
            other = 0
            payforex = 0
            DP = 0
            For Each Dr In ViewState("Dt").Rows
                If Not Dr.RowState = DataRowState.Deleted Then
                    infois = infois + CFloat(Dr("InvoiceHome").ToString)                    
                    PPh = PPh + CFloat(Dr("PPhHome").ToString)
                    'If (Dr("FgMode").ToString = "D" Or Dr("FgMode").ToString = "B" Or Dr("FgMode").ToString = "K" Or Dr("FgMode").ToString = "G") Then
                    If Dr("FgMode").ToString = "E" Then  'Expense
                        cas = cas + CFloat(Dr("PaymentHome").ToString)
                    ElseIf Dr("FgMode").ToString = "O" Then  'Expense
                        other = other + CFloat(Dr("PaymentHome").ToString)
                    ElseIf Dr("FgMode").ToString = "D" Then  'Expense
                        DP = DP + CFloat(Dr("PaymentHome").ToString)
                    Else
                        peimen = peimen + CFloat(Dr("PaymentHome").ToString)
                        payforex = payforex + CFloat(Dr("PaymentForex").ToString)
                    End If
                End If
            Next
            tbTotalDPForex.Text = FormatNumber(DP, ViewState("DigitCurr"))
            tbTotalPaymentForex.Text = FormatNumber(payforex, ViewState("DigitCurr"))
            tbTotalPayment.Text = FormatNumber(peimen, ViewState("DigitHome"))
            tbTotalOther.Text = FormatNumber(other, ViewState("DigitHome"))
            tbTotalCharge.Text = FormatNumber(cas, ViewState("DigitHome"))
            'tbTotalInvoice.Text = FormatNumber(infois, ViewState("DigitHome"))
            tbTotalPPh.Text = FormatNumber(PPh, ViewState("DigitHome"))
            tbTotalSelisih.Text = FormatNumber(CFloat(tbTotalPayment.Text) + CFloat(tbTotalDPForex.Text) + CFloat(tbTotalOther.Text) + CFloat(tbTotalPPh.Text) - CFloat(tbTotalCharge.Text) - CFloat(tbTotalInvoice.Text) - CFloat(tbTotalKurs.Text), ViewState("DigitHome"))
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
                tbCode.Text = GetAutoNmbr("PYT", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)

                'FORMAT(A.TotalCharge, DigitCurrHome()) AS TotalCharge, A.Remark
                SQLString = "INSERT INTO FINPayTradeHd (TransNmbr, TransDate, STATUS, FgReport, " + _
                "Supplier, Attn, TotalDP, TotalPayForex, TotalPayment, TotalOthers, TotalInvoice, TotalPPh, TotalCharge, TotalKurs, TotalSelisih, Remark, UserPrep, " + _
                "DatePrep) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", 'H', " + _
                QuotedStr("Y") + ", " + QuotedStr(tbSuppCode.Text) + ", " + QuotedStr(tbAttn.Text) + _
                ", " + tbTotalDPForex.Text.Replace(",", "") + ", " + tbTotalPaymentForex.Text.Replace(",", "") + _
                ", " + tbTotalPayment.Text.Replace(",", "") + ", " + tbTotalOther.Text.Replace(",", "") + _
                ", " + tbTotalInvoice.Text.Replace(",", "") + ", " + tbTotalPPh.Text.Replace(",", "") + ", " + tbTotalCharge.Text.Replace(",", "") + _
                ", " + tbTotalKurs.Text.Replace(",", "") + _
                ", " + tbTotalSelisih.Text.Replace(",", "") + ", " + QuotedStr(tbRemark.Text) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", getDate()"

                ViewState("TransNmbr") = tbCode.Text

            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM FINPayTradeHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE FINPayTradeHd SET FgReport =" + QuotedStr("Y") + ", Supplier=" + QuotedStr(tbSuppCode.Text) + _
                ", TransDate = " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", Attn =" + QuotedStr(tbAttn.Text) + _
                ", TotalDP=" + tbTotalDPForex.Text.Replace(",", "") + _
                ", TotalPayForex=" + tbTotalPaymentForex.Text.Replace(",", "") + _
                ", TotalPayment=" + tbTotalPayment.Text.Replace(",", "") + ", TotalOthers =" + tbTotalOther.Text.Replace(",", "") + _
                ", TotalInvoice= " + tbTotalInvoice.Text.Replace(",", "") + ", TotalPPh= " + tbTotalPPh.Text.Replace(",", "") + _
                ", TotalKurs = " + tbTotalKurs.Text.Replace(",", "") + _
                ", TotalCharge= " + tbTotalCharge.Text.Replace(",", "") + ", TotalSelisih= " + tbTotalSelisih.Text.Replace(",", "") + ", Remark = " + QuotedStr(tbRemark.Text) + _
                ", DatePrep = GetDate()" + _
                " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            End If
            
            SQLString = ChangeQuoteNull(SQLString)
            SQLString = SQLString.Replace("'1900-01-01'", "NULL")
            SQLString = SQLString.Replace("'0001-01-01'", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            'update Primary Key on Dt
            Dim Row As DataRow()

            Row = ViewState("Dt").Select("TransNmbr is NULL")
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

            If Not ViewState("Dt2") Is Nothing Then
                Row = ViewState("Dt2").Select("TransNmbr IS NULL")
                For I = 0 To Row.Length - 1
                    Row(I).BeginEdit()
                    Row(I)("TransNmbr") = tbCode.Text                   
                    Row(I).EndEdit()
                Next
            End If
            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString
            con = New SqlConnection(ConnString)
            con.Open()
            
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, ItemNo, PaymentType, PaymentDate, DocumentNo, Reference, GiroDate, DueDate, BankPayment, Currency, ForexRate, PaymentForex, PaymentHome, InvoiceHome, PPhHome, Remark, FgMode, FgValue, DPRate FROM FinPayTradeDt WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)
            
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
            "UPDATE FinPayTradeDt SET ItemNo = @ItemNo, PaymentType = @PaymentType, " + _
            "PaymentDate = @PaymentDate, DocumentNo = @DocumentNo, Reference = @Reference, " + _
            "GiroDate = @GiroDate, DueDate = @DueDate, BankPayment = @BankPayment, Currency = @Currency, " + _
            "ForexRate = @ForexRate, PaymentForex = @PaymentForex, PaymentHome = @PaymentHome, InvoiceHome = @InvoiceHome, " + _
            "PPhHome = @PPhHome, Remark = @Remark, FgMode = @FgMode, FgValue = @FgValue, DPRate = @DPRate " + _
            "WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND ItemNo = @OldItemNo", con)

            ' Define output parameters.
            Update_Command.Parameters.Add("@ItemNo", SqlDbType.Int, 4, "ItemNo")
            Update_Command.Parameters.Add("@PaymentType", SqlDbType.VarChar, 5, "PaymentType")
            Update_Command.Parameters.Add("@PaymentDate", SqlDbType.DateTime, 8, "PaymentDate")
            Update_Command.Parameters.Add("@DocumentNo", SqlDbType.VarChar, 60, "DocumentNo")
            Update_Command.Parameters.Add("@Reference", SqlDbType.VarChar, 20, "Reference")
            Update_Command.Parameters.Add("@GiroDate", SqlDbType.DateTime, 8, "GiroDate")
            Update_Command.Parameters.Add("@DueDate", SqlDbType.DateTime, 8, "DueDate")
            Update_Command.Parameters.Add("@BankPayment", SqlDbType.VarChar, 5, "BankPayment")
            Update_Command.Parameters.Add("@Currency", SqlDbType.VarChar, 5, "Currency")
            Update_Command.Parameters.Add("@ForexRate", SqlDbType.Decimal, 22, "ForexRate")
            Update_Command.Parameters.Add("@PaymentForex", SqlDbType.Decimal, 22, "PaymentForex")
            Update_Command.Parameters.Add("@PaymentHome", SqlDbType.Decimal, 22, "PaymentHome")
            Update_Command.Parameters.Add("@InvoiceHome", SqlDbType.Decimal, 22, "InvoiceHome")
            Update_Command.Parameters.Add("@PPhHome", SqlDbType.Float, 13, "PPhHome")
            Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 255, "Remark")
            Update_Command.Parameters.Add("@FgMode", SqlDbType.VarChar, 1, "FgMode")
            Update_Command.Parameters.Add("@FgValue", SqlDbType.VarChar, 1, "FgValue")
            Update_Command.Parameters.Add("@DPRate", SqlDbType.Decimal, 22, "DPRate")

            '' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@OldItemNo", SqlDbType.Int, 4, "ItemNo")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM FINPayTradeDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND ItemNo = @ItemNo ", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@ItemNo", SqlDbType.Int, 4, "ItemNo")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command
            
            Dim Dt As New DataTable("FINPayTradeDt")
            
            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt

            If Not ViewState("Dt2") Is Nothing Then
                'save dt2
                cmdSql = New SqlCommand("SELECT TransNmbr, ItemNo, InvoiceNo, SupplierInvNo, DueDate, Currency, ForexRate, PayRate, PayRatePPn, PPnRate, TotalInvoice, TotalPaid, TotalToPaid, BaseInvoice, BasePaid, BaseToPaid, PPNInvoice, PPNPaid, PPNToPaid, PPhInvoice, PPhPaid, PPhToPaid, Remark, FgValue, Type FROM FinPayTradeDtInv WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
                
                da = New SqlDataAdapter(cmdSql)
                dbcommandBuilder = New SqlCommandBuilder(da)
                da.InsertCommand = dbcommandBuilder.GetInsertCommand
                da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
                da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

                Dim Dt2 As New DataTable("FINPayTradeDtInv")

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
            If tbSuppCode.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Supplier must have value")
                tbSuppCode.Focus()
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
                Exit Sub
            End If
            'If CFloat(tbTotalInvoice.Text.Replace(",", "")) <> CFloat(tbTotalPayment.Text.Replace(",", "")) Then
            '    lbStatus.Text = MessageDlg("Can't Add Data, Total Payment value is Equal to Total Invoice Value")
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
            AttachScript("setformat();", Page, Me.GetType())
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
            FDateName = "Date, Giro Date, Due Date"
            FDateValue = "TransDate, GiroDate, DueDate"
            FilterName = "Payment No, Payment Date, Supplier, Attn, Document No, Vooucher No, Invoice No, Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), Supplier, Attn, DocumentNo, Vooucher_No, Invoice_No, Remark"
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
                    ' FillCombo(ddlPaymentType, "EXEC S_GetPayTypeUser(" + QuotedStr("PaymentTrade" + ddlReport.SelectedValue) + ", " + QuotedStr(Session("UserId").ToString) + ")", True, "Payment_Code", "Payment_Name", Session("DBConnection"))
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
                        FillCombo(ddlPaymentType, "EXEC S_GetPayTypeUser " + QuotedStr("PaymentTrade" + "Y") + ", " + QuotedStr(ViewState("UserId").ToString), True, "Payment_Code", "Payment_Name", ViewState("DBConnection")) 'ddlReport.SelectedValue
                        FillCombo(ddlBankPayment, "EXEC S_GetBankPaymentTrade " + QuotedStr(ddlCurr.SelectedValue), True, "Bank_Code", "Bank_Name", ViewState("DBConnection")) 'ddlReport.SelectedValue
                        EnableHd(GetCountRecord(ViewState("Dt2")) = 0)
                    Else
                        lbStatus.Text = MessageDlg("Data must be Hold or Get Approval to edit")
                        Exit Sub
                    End If
                ElseIf (DDL.SelectedValue = "Print") Or DDL.SelectedValue = "Print Full" Then
                    Try
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        Session("DBConnection") = ViewState("DBConnection")
                        Session("SelectCommand") = "EXEC S_FNFormVoucher '''" + GVR.Cells(2).Text + "''','PAP'," + QuotedStr(ViewState("UserId"))
                        Session("ReportFile") = ".../../../Rpt/FormVoucher.frx"
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
        Try
            If e.CommandName = "Detail" Then
                Dim GVR As GridViewRow
                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))
                If GVR.Cells(2).Text = "&nbsp;" Then
                    Exit Sub
                End If

          
                lbItemNodt2.Text = GVR.Cells(2).Text
                lblPaymentTypeDt2.Text = GVR.Cells(3).Text
                lblCurrPayDt.Text = GVR.Cells(10).Text
                lblRatePayDt.Text = GVR.Cells(11).Text
                lblVPayHome.Text = "Payment (" + GVR.Cells(10).Text + ")"
                lblVInvHome.Text = "Invoice (" + GVR.Cells(10).Text + ")"
                lblVPPhHome.Text = "PPh (" + GVR.Cells(10).Text + ")"
                'lblVPayHome.Text = "Payment (" + ViewState("Currency") + ")"
                'lblVInvHome.Text = "Invoice (" + ViewState("Currency") + ")"
                'lblVPPhHome.Text = "PPh (" + ViewState("Currency") + ")"
                ' lblVChargeHome.Text = "Charge (" + ViewState("Currency") + ")"
                '  lblVSelisihHome.Text = "Selisih Kurs(" + ViewState("Currency") + ")"

                Dim lb As Label
                lb = GVR.FindControl("lbFgMode")
                
                lbFgModeDt2.Text = lb.Text

                'lb = GVR.FindControl("lbProductType")
                'lbProductTypeDt2.Text = lb.Text
                tbPayHomeDt.Text = GVR.Cells(12).Text
                tbInvHomeDt.Text = GVR.Cells(14).Text
                tbPPhHomeDt.Text = GVR.Cells(15).Text
                'tbPayHomeDt.Text = GVR.Cells(13).Text
                'tbInvHomeDt.Text = GVR.Cells(14).Text
                'tbPPhHomeDt.Text = GVR.Cells(15).Text
                'tbChargeHomeDt2.Text = GVR.Cells(16).Text
                'tbSelisihHomeDt2.Text = GVR.Cells(17).Text
                MultiView1.ActiveViewIndex = 1

                If ViewState("StateHd") = "View" Then
                    ModifyInput2(False, pnlInput, pnlDt2, GridDt2)
                Else
                    ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
                End If

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

                StatusButtonSave(True)
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Command Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Try
            Dim dr(), ExistRow() As DataRow
            Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
            ExistRow = ViewState("Dt2").Select("ItemNo = " + QuotedStr(GVR.Cells(2).Text))
            If ExistRow.Length > 0 Then
                lbStatus.Text = MessageDlg("Detail Invoice Exist, cannot delete data")
                Exit Sub
            End If
            dr = ViewState("Dt").Select("ItemNo = " + QuotedStr(GVR.Cells(2).Text))
            dr(0).Delete()
            BindGridDt(ViewState("Dt"), GridDt)
            EnableHd(GetCountRecord(ViewState("Dt2")) = 0)

            CountTotalDt()

        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Dim TotalInvoice As Double = 0
    Dim TotalPPh As Double = 0
    'Dim TotalSRDt As Decimal = 0
    Protected Sub GridDt2_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt2.RowDataBound
        Dim FgValue, Base, RateNew, RatePPn, RatePay, PPn, PPh, RatePayPPn As Decimal
        Try
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "ItemNo")) Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                    FgValue = CFloat(DataBinder.Eval(e.Row.DataItem, "FgValue").ToString)
                    Base = CFloat(DataBinder.Eval(e.Row.DataItem, "BaseToPaid").ToString)
                    PPn = CFloat(DataBinder.Eval(e.Row.DataItem, "PPnToPaid").ToString)
                    PPh = CFloat(DataBinder.Eval(e.Row.DataItem, "PPhToPaid").ToString)
                    RateNew = CFloat(DataBinder.Eval(e.Row.DataItem, "ForexRate").ToString)
                    RatePPn = CFloat(DataBinder.Eval(e.Row.DataItem, "PPnRate").ToString)
                    RatePayPPn = CFloat(DataBinder.Eval(e.Row.DataItem, "PayRatePPn").ToString)
                    RatePay = CFloat(DataBinder.Eval(e.Row.DataItem, "PayRate").ToString)
                    'TotalInvoice = TotalInvoice + (((((Base + PPh) * RatePay) + (PPn * RatePPn)) * FgValue) / CFloat(lblRatePayDt.Text))
                    TotalInvoice = TotalInvoice + (((((Base + PPh) * RatePay) + (PPn * RatePayPPn)) * FgValue) / CFloat(lblRatePayDt.Text))
                    TotalPPh = TotalPPh + ((PPh * RatePay * FgValue) / CFloat(lblRatePayDt.Text))
                    'If RatePay = 0 Then
                    '    TotalSRDt = TotalSRDt - ((FgValue * (Base + PPh) * (RateNew - RateOri)) + (FgValue * PPn * (RateNew - RateOri)))
                    'Else
                    'TotalSRDt = TotalSRDt + ((FgValue * (Base + PPh) * (RatePay - RateNew)) + (FgValue * PPn * (RatePay - RatePPn)))
                    'lbStatus.Text = RatePay.ToString + " " + RateNew.ToString + " " + Base.ToString + " " + PPh.ToString
                    'Exit Sub
                    'End If
                ElseIf e.Row.RowType = DataControlRowType.Footer Then
                    If lblCurrPayDt.Text <> "" Then
                        ViewState("DigitInv") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(lblCurrPayDt.Text), ViewState("DBConnection"))
                    Else
                        ViewState("DigitInv") = "0"
                    End If
                    tbInvHomeDt.Text = FormatNumber(TotalInvoice, ViewState("DigitInv"))
                    tbPPhHomeDt.Text = FormatNumber(TotalPPh, ViewState("DigitInv"))
                    'tbSelisihHomeDt2.Text = FormatNumber(TotalSRDt, ViewState("DigitHome"))
                End If
            End If
            'EnableHd(GetCountRecord(ViewState("Dt")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub GridDt2_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt2.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow = GridDt2.Rows(e.RowIndex)
            dr = ViewState("Dt2").Select("ItemNo = " + lbItemNodt2.Text + " AND InvoiceNo = " + QuotedStr(GVR.Cells(1).Text))
            dr(0).Delete()
            Dim drow As DataRow()
            drow = ViewState("Dt2").Select("ItemNo = " + lbItemNodt2.Text)
            If drow.Length > 0 Then
                BindGridDt(drow.CopyToDataTable, GridDt2)
                GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
            Else
                Dim DtTemp As New DataTable
                DtTemp = ViewState("Dt2").Clone
                DtTemp.Rows.Add(DtTemp.NewRow()) ' create a new blank row to the DataTable
                GridDt2.DataSource = DtTemp
                GridDt2.DataBind()
                GridDt2.Columns(0).Visible = False
            End If
            CountTotalInvoiceHome(lbItemNodt2.Text)
            ' BindGridDt(ViewState("Dt2"), GridDt2)
            ' EnableHd(GetCountRecord(ViewState("Dt")) = 0)
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
            BindToText(tbSuppCode, Dt.Rows(0)("SupplierCode").ToString)
            BindToText(tbSuppName, Dt.Rows(0)("SupplierName").ToString)
            BindToText(tbAttn, Dt.Rows(0)("Attn").ToString)
            ViewState("DigitCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(Dt.Rows(0)("Currency").ToString), ViewState("DBConnection"))
            BindToText(tbTotalPaymentForex, Dt.Rows(0)("TotalPayForex").ToString, ViewState("DigitCurr"))
            BindToText(tbTotalDPForex, Dt.Rows(0)("TotalDP").ToString, ViewState("DigitHome"))
            BindToText(tbTotalPayment, Dt.Rows(0)("TotalPayment").ToString, ViewState("DigitHome"))
            BindToText(tbTotalOther, Dt.Rows(0)("TotalOthers").ToString, ViewState("DigitHome"))
            BindToText(tbTotalInvoice, Dt.Rows(0)("TotalInvoice").ToString, ViewState("DigitHome"))
            BindToText(tbTotalPPh, Dt.Rows(0)("TotalPPh").ToString, ViewState("DigitHome"))
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
                BindToDropList(ddlPaymentType, Dr(0)("PaymentType").ToString)
                tbPaymentDate.SelectedDate = tbDate.SelectedDate
                'BindToDate(tbPaymentDate, Dr(0)("PaymentDate").ToString)
                BindToText(tbDocumentNo, Dr(0)("DocumentNo").ToString)
                BindToText(tbVoucherNo, Dr(0)("Reference").ToString)
                BindToText(tbFgMode, Dr(0)("FgMode").ToString)
                BindToDate(tbGiroDate, Dr(0)("GiroDate").ToString)
                BindToDate(tbDueDate, Dr(0)("DueDate").ToString)                
                BindToDropList(ddlCurr, Dr(0)("Currency").ToString)
                'BindToDropList(ddlChargeCurr, Dr(0)("ChargeCurrency").ToString)
                'BindToDropList(ddlProductType, Dr(0)("ProductType").ToString)
                'ddlProductType.Enabled = tbFgMode.Text = "G"
                FillCombo(ddlBankPayment, "EXEC S_GetBankPaymentTrade " + QuotedStr(ddlCurr.SelectedValue), True, "Bank_Code", "Bank_Name", ViewState("DBConnection")) 'ddlReport.SelectedValue
                BindToDropList(ddlBankPayment, Dr(0)("BankPayment").ToString)
                ViewState("DigitCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlCurr.SelectedValue), ViewState("DBConnection"))
                'ViewState("DigitChargeCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlChargeCurr.SelectedValue), ViewState("DBConnection"))                

                BindToText(tbRate, Dr(0)("ForexRate").ToString)
                tbRate.Text = FormatFloat(tbRate.Text.Replace(",", ""), ViewState("DigitRate"))
                'BindToText(tbChargeRate, Dr(0)("ChargeRate").ToString)
                BindToText(tbPaymentForex, Dr(0)("PaymentForex").ToString)
                BindToText(tbPaymentHome, Dr(0)("PaymentHome").ToString)
                BindToText(tbInvoiceHome, Dr(0)("InvoiceHome").ToString)
                BindToText(tbPPhHome, Dr(0)("PPhHome").ToString)
                'BindToText(tbSelisihHome, Dr(0)("SelisihHome").ToString)
                'BindToText(tbChargeForex, Dr(0)("ChargeForex").ToString)
                'BindToText(tbChargeHome, Dr(0)("ChargeHome").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)

                If tbPPhHome.Text = "" Then
                    tbPPhHome.Text = "0"
                End If
                'If tbSelisihHome.Text = "" Then
                '    tbSelisihHome.Text = "0"
                'End If
                'If ddlChargeCurr.SelectedValue = "" Then
                '    tbChargeForex.Text = "0"
                '    tbChargeHome.Text = "0"
                'End If
                tbRate.Enabled = Dr(0)("Currency").ToString <> ViewState("Currency")
                ddlBankPayment.Enabled = tbFgMode.Text = "G"
                tbGiroDate.Enabled = tbFgMode.Text = "G"
                tbDueDate.Enabled = tbFgMode.Text = "G"
                'ddlChargeCurr.Enabled = tbFgMode.Text = "B"
                'tbChargeForex.Enabled = tbFgMode.Text = "B"
                'tbChargeRate.Enabled = ddlChargeCurr.SelectedValue <> Session("Currency") And tbFgMode.Text = "B" And ddlChargeCurr.SelectedValue <> ""
                tbVoucherNo.Enabled = (tbFgMode.Text = "B" Or tbFgMode.Text = "K")

                'ChangePaymentType2(ddlPaymentType.SelectedValue, tbFgMode, tbPaymentDate, tbDueDate, ddlBankPayment, ddlCurr, tbRate, Session("Currency"), ViewState("DigitCurr"), Session("DBConnection"))

                BindToText(tbDPRate, Dr(0)("DPRate").ToString)
                tbDPRate.Text = FormatFloat(tbDPRate.Text.Replace(",", ""), ViewState("DigitRate"))
                lbDPRate.Visible = tbFgMode.Text = "D"
                tbDPRate.Visible = tbFgMode.Text = "D"
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxDt2(ByVal ItemNo As String, ByVal InvoiceNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt2").select("ItemNo = " + ItemNo + " AND InvoiceNo =" + QuotedStr(InvoiceNo.ToString))
            If Dr.Length > 0 Then
                lbItemNodt2.Text = ItemNo
                BindToText(tbInvNoDt2, Dr(0)("InvoiceNo").ToString)
                BindToText(tbSuppInvNoDt2, Dr(0)("SupplierInvNo").ToString)
                BindToDate(tbDueDateDt2, Dr(0)("DueDate").ToString)
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
                BindToText(tbPPhInvoiceDt2, Dr(0)("PPhInvoice").ToString)
                BindToText(tbPPhPaidDt2, Dr(0)("PPhPaid").ToString)
                BindToText(tbPPhToPaidDt2, Dr(0)("PPhToPaid").ToString)
                'BindToText(tbProductType, Dr(0)("ProductType").ToString)
                BindToText(tbRemarkDt2, Dr(0)("Remark").ToString)
                BindToText(tbType, Dr(0)("Type").ToString)
                'If lblCurrPayDt.Text = Session("Currency").ToString Then
                'tbPayRateDt2.Enabled = False
                'Else
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
                'End If
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail 2 error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            FillTextBoxDt(GVR.Cells(2).Text)
            MovePanel(PnlDt, pnlEditDt)
            EnableHd(False)
            'ChangePaymentType(ddlPaymentType.SelectedValue, tbFgMode, tbPaymentDate, tbDueDate, ddlBankPayment, ddlCurr, ddlChargeCurr, tbRate, tbChargeRate, tbChargeForex, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"), "Edit")
            'ChangePaymentType(ddlPaymentType.SelectedValue, tbFgMode, tbPaymentDate, tbGiroDate, ddlBankPayment, ddlCurr, ddlChargeCurr, tbRate, tbChargeRate, tbChargeForex, Session("Currency"), ViewState("DigitCurr"), Session("DBConnection"))
            tbGiroDate.Enabled = tbDueDate.Enabled
            If tbGiroDate.Enabled = False Then
                tbGiroDate.SelectedDate = Nothing
                'Else
                '    tbGiroDate.SelectedDate = tbPaymentDate.SelectedDate
            End If
            'ChangeCurrency(ddlCurr, tbDate, tbRate, Session("Currency"), ViewState("DigitCurr"), Session("DBConnection"))
            ViewState("StateDt") = "Edit"
            btnSaveDt.Focus()
            StatusButtonSave(False)
            btnDocNo.Visible = tbFgMode.Text = "D"
            tbDocumentNo.Enabled = Not tbFgMode.Text = "D"
            lbDPRate.Visible = tbFgMode.Text = "D"
            tbDPRate.Visible = tbFgMode.Text = "D"
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub GridDt2_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt2.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt2.Rows(e.NewEditIndex)
            FillTextBoxDt2(lbItemNodt2.Text, GVR.Cells(1).Text)
            MovePanel(pnlDt2, pnlEditDt2)
            EnableHd(False)
            ViewState("StateDt2") = "Edit"
            ViewState("InvoiceNo") = GVR.Cells(1).Text
            ViewState("DtPayRate") = CFloat(GVR.Cells(5).Text)
            ViewState("DtPPnRate") = CFloat(GVR.Cells(6).Text)
            ViewState("DtBaseToPaid") = CFloat(GVR.Cells(13).Text)
            ViewState("DtPPnToPaid") = CFloat(GVR.Cells(14).Text)
            ViewState("DtPPhToPaid") = CFloat(GVR.Cells(18).Text)

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
            Session("filter") = "SELECT * FROM VMsSupplier"
            ResultField = "Supplier_Code, Supplier_Name, Contact_Person"
            ViewState("Sender") = "btnSupplier"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Supplier Click Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub tbSuppCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbSuppCode.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Supplier", tbSuppCode.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbSuppCode.Text = Dr("Supplier_Code")
                tbSuppName.Text = Dr("Supplier_Name")
                BindToText(tbAttn, Dr("Contact_Person"))
            Else
                tbSuppCode.Text = ""
                tbSuppName.Text = ""
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
            'AttachScript("setformatdt();", Page, Me.GetType)
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt") = "Insert"
            lbItemNo.Text = GetNewItemNo(ViewState("Dt"))
            MovePanel(PnlDt, pnlEditDt)
            EnableHd(False)
            StatusButtonSave(False)
            ddlPaymentType.Focus()
            btnDocNo.Visible = False
            tbDocumentNo.Enabled = True
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
            'If tbFgMode.Text = "D" Then
            '    tbRate.Enabled = False
            'End If
            If tbFgMode.Text <> "D" Then
                tbDPRate.Text = tbRate.Text
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

    Protected Sub ddlPaymentType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPaymentType.SelectedIndexChanged
        Dim dr As DataRow
        Dim VoucherNo As String
        dr = FindMaster("PayType", ddlPaymentType.SelectedValue, ViewState("DBConnection"))
        If Not dr Is Nothing Then
            BindToText(tbFgMode, dr("FgMode"))
            'BindToDropList(ddlReport, dr("FgReport"))
            BindToDropList(ddlCurr, dr("Currency"))
        Else
            tbFgMode.Text = "B"
        End If
        ChangePaymentType2(ddlPaymentType.SelectedValue, tbFgMode, tbDate, tbDueDate, ddlBankPayment, ddlCurr, tbRate, ViewState("Currency"), ViewState("DBConnection"))
        FillCombo(ddlBankPayment, "EXEC S_GetBankPaymentTrade " + QuotedStr(ddlCurr.SelectedValue), True, "Bank_Code", "Bank_Name", ViewState("DBConnection")) 'ddlReport.SelectedValue
        'ChangeCurrency(ddlCurr, tbPaymentDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection").ToString, ViewState("DigitHome"))
        'ChangePaymentType(ddlPaymentType.SelectedValue, tbFgMode, tbPaymentDate, tbGiroDate, ddlBankPayment, ddlCurr, ddlChargeCurr, tbRate, tbChargeRate, tbChargeForex, Session("Currency"), ViewState("DigitCurr"), Session("DBConnection"))
        tbGiroDate.Enabled = tbDueDate.Enabled
        If tbGiroDate.Enabled = False Then
            tbGiroDate.SelectedDate = Nothing
        Else
            tbGiroDate.SelectedDate = tbPaymentDate.SelectedDate
        End If
        'ViewState("DigitChargeCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlChargeCurr.SelectedValue), ViewState("DBConnection"))
        'If ViewState("DigitChargeCurr") Is Nothing Or ViewState("DigitChargeCurr") = "" Then
        '    ViewState("DigitChargeCurr") = "0"
        'End If
        tbPaymentForex.Text = FormatFloat(CFloat(tbPaymentHome.Text) / CFloat(tbRate.Text), ViewState("DigitCurr"))
        tbRate.Text = FormatFloat(tbRate.Text.Replace(",", ""), ViewState("DigitRate"))
        'tbPaymentForex.Text = FormatFloat(tbPaymentForex.Text.Replace(",", ""), ViewState("DigitCurr"))
        tbPaymentHome.Text = FormatFloat(CFloat(tbPaymentForex.Text) * CFloat(tbRate.Text), ViewState("DigitHome"))
        'tbChargeHome.Text = FormatFloat(CFloat(tbChargeForex.Text) * CFloat(tbChargeRate.Text), ViewState("DigitChargeCurr"))
        'AttachScript("kali(" + Me.tbRate.ClientID + "," + Me.tbPaymentForex.ClientID + "," + Me.tbPaymentHome.ClientID + "); kali(" + Me.tbChargeRate.ClientID + "," + Me.tbChargeForex.ClientID + "," + Me.tbChargeHome.ClientID + "); setformatdt();", Page, Me.GetType())
        btnDocNo.Visible = tbFgMode.Text = "D"
        tbDocumentNo.Enabled = Not tbFgMode.Text = "D"
        tbDocumentNo.Text = ""
        'ddlProductType.Enabled = tbFgMode.Text = "G"
        'ddlProductType.SelectedValue = ""
        VoucherNo = ""
        If tbFgMode.Text = "B" Or tbFgMode.Text = "K" Then
            VoucherNo = SQLExecuteScalar("Declare @A VarChar(255) EXEC S_SAAutoVoucherNmbr " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr("Y") + ", " + QuotedStr(ddlPaymentType.SelectedValue) + ", 'OUT', @A OUT SELECT @A", ViewState("DBConnection").ToString) 'ddlReport.SelectedValue
        End If

        tbVoucherNo.Enabled = (tbFgMode.Text = "B" Or tbFgMode.Text = "K")
        tbVoucherNo.Text = VoucherNo
        btnSaveDt.Focus()

        lbDPRate.Visible = tbFgMode.Text = "D"
        tbDPRate.Visible = tbFgMode.Text = "D"        
        tbDPRate.Text = tbRate.Text
    End Sub

    'Protected Sub ddlReport_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlReport.SelectedIndexChanged
    '    FillCombo(ddlPaymentType, "EXEC S_GetPayTypeUser " + QuotedStr("PaymentTrade" + ddlReport.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString), True, "Payment_Code", "Payment_Name", ViewState("DBConnection"))
    '    FillCombo(ddlBankPayment, "EXEC S_GetBankPayment " + QuotedStr(ddlReport.SelectedValue), True, "Bank_Code", "Bank_Name", ViewState("DBConnection"))
    'End Sub

    Protected Sub btnInvNo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnInvNo.Click
        Dim ResultField, CriteriaField, sqlstring As String
        Try
            sqlstring = "EXEC S_FNPayTradeGetInv '" + tbSuppCode.Text.Trim + "','Y'," + QuotedStr(ViewState("TransNmbr")) 'ddlReport.SelectedValue.ToString
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = sqlstring
            ResultField = "Invoice_No, Currency, Forex_Rate, PPN_Rate, Supplier_Invoice, Due_Date, Base_Forex, PPN_Forex, PPh_Forex, Base_Paid, PPN_Paid, PPh_Paid, PPh_Saldo, FgValue, Supplier, Supplier_Name"
            CriteriaField = "Invoice_No, Supplier_Invoice, Supplier, Supplier_Name, Currency"
            Session("CriteriaField") = CriteriaField.Split(",")
            ViewState("Sender") = "btnInvNo"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Supplier Click Error : " + ex.ToString
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
            If tbSuppCode.Text.Trim = "" Then
                Session("filter") = "SELECT DP_No, DP_Date, User_Code, User_Name, PO_No, PPN_Rate, Currency, Rate, Base_Forex, PPN_Forex, Total_Forex, Base_Paid, PPN_Paid, Total_Paid FROM V_FNDPSuppPending " + _
                "WHERE PPN_Forex = 0 and User_Type = 'Supplier' AND Report =" + QuotedStr("Y") 'ddlReport.SelectedValue
            Else
                Session("filter") = "SELECT DP_No, DP_Date, User_Code, User_Name, PO_No, PPN_Rate, Currency, Rate, Base_Forex, PPN_Forex, Total_Forex, Base_Paid, PPN_Paid, Total_Paid FROM V_FNDPSuppPending " + _
            "WHERE PPN_Forex = 0 and User_Type = 'Supplier' AND User_Code =" + QuotedStr(tbSuppCode.Text) + " AND Report =" + QuotedStr("Y") 'ddlReport.SelectedValue
            End If
            Session("DBConnection") = ViewState("DBConnection")
            ResultField = "DP_No, Currency, Rate, Total_Forex, Total_Paid, User_Code, User_Name"
            ViewState("Sender") = "btnDocNo"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Doc No Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnBackDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBackDt2.Click, btnBackDt2Ke2.Click
        MultiView1.ActiveViewIndex = 0
        btnSaveTrans.Visible = True
        btnSaveAll.Visible = True        
    End Sub

    'Protected Sub tbPayRateDt2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPayRateDt2.TextChanged
    'Dim BaseTotal, BasePaid, BaseToPaid, PPnTotal, PPnPaid, PPnToPaid, RateNew, RatePPn, Sisa, TotalToPaid As Double
    'BaseTotal = CFloat(tbBaseInvoiceDt2.Text)
    'PPnTotal = CFloat(tbPPNInvoiceDt2.Text)

    'BasePaid = CFloat(tbBasePaidDt2.Text)
    'PPnPaid = CFloat(tbPPNPaidDt2.Text)
    'RateNew = CFloat(tbPayRateDt2.Text)
    'RatePPn = CFloat(tbPPNRateDt2.Text)

    'Sisa = ViewState("TotalPayDt2") - ViewState("TotalInvDt2") - ViewState("TotalChargeDt2") + (ViewState("PayRate") * ViewState("TotalToPaid"))

    'PPnToPaid = 0
    'If PPnTotal - PPnPaid > 0 Then
    '    PPnToPaid = (PPnTotal - PPnPaid) * RatePPn
    '    If PPnToPaid > Sisa Then
    '        PPnToPaid = Sisa
    '        Sisa = 0
    '    Else
    '        Sisa = Sisa - PPnToPaid
    '    End If
    'End If
    'tbPPNToPaidDt2.Text = FormatNumber(PPnToPaid / RatePPn, ViewState("DigitCurrInv"))

    'BaseToPaid = 0
    'If Sisa > 0 Then
    '    If BaseTotal - BasePaid > 0 Then
    '        BaseToPaid = (BaseTotal - BasePaid) * RateNew
    '        If BaseToPaid > Sisa Then
    '            BaseToPaid = Sisa
    '        End If
    '    End If
    'End If
    'tbBaseToPaidDt2.Text = FormatNumber(BaseToPaid / RateNew, ViewState("DigitCurrInv"))
    'TotalToPaid = (BaseToPaid / RateNew) + (PPnToPaid / RatePPn)
    'tbTotalToPaidDt2.Text = FormatNumber(TotalToPaid, ViewState("DigitCurrInv"))
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
            If tbSuppCode.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Supplier must have value")
                tbSuppCode.Focus()
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
                Exit Sub
            End If
            'If CFloat(tbTotalInvoice.Text.Replace(",", "")) <> CFloat(tbTotalPayment.Text.Replace(",", "")) Then
            '    lbStatus.Text = MessageDlg("Can't Add Data, Total Payment value is Equal to Total Invoice Value")
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
            BtnAdd_Click(Nothing, Nothing)
        Catch ex As Exception
            lbStatus.Text = "btn saveall Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnGetInv_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGetInv.Click
        Dim ResultField, CriteriaField, sqlstring, ResultSame As String
        Try
            sqlstring = "EXEC S_FNPayTradeGetInv '" + tbSuppCode.Text.Trim + "','Y'," + QuotedStr(ViewState("TransNmbr")) '" + ddlReport.SelectedValue.ToString + "
            'If lbFgModeDt2.Text = "G" Then
            '    If tbSuppCode.Text.Trim = "" Then
            '        'ini diremark, karena product type tidak dipakai lagi
            '        'If tbProductType.Text = "" Then
            '        sqlstring = "SELECT * FROM V_FNAPPosting WHERE Report = " + QuotedStr(ddlReport.SelectedValue)
            '        'Else
            '        'sqlstring = "SELECT * FROM V_FNAPPosting WHERE Report = " + QuotedStr(ddlReport.SelectedValue) + " and ProductType =" + QuotedStr(tbProductType.Text)
            '        'End If
            '    Else
            '        'If tbProductType.Text = "" Then
            '        sqlstring = "SELECT * FROM V_FNAPPosting WHERE Supplier = " + QuotedStr(tbSuppCode.Text) + " and Report = " + QuotedStr(ddlReport.SelectedValue)
            '        'Else
            '        'sqlstring = "SELECT * FROM V_FNAPPosting WHERE Supplier = " + QuotedStr(tbSuppCode.Text) + " and Report = " + QuotedStr(ddlReport.SelectedValue) + " and ProductType =" + QuotedStr(tbProductType.Text)
            '        'End If
            '    End If
            'Else
            '    If tbSuppCode.Text.Trim = "" Then
            '        sqlstring = "SELECT * FROM V_FNAPPosting WHERE Report = " + QuotedStr(ddlReport.SelectedValue)
            '    Else
            '        sqlstring = "SELECT * FROM V_FNAPPosting WHERE Supplier = " + QuotedStr(tbSuppCode.Text) + " and Report = " + QuotedStr(ddlReport.SelectedValue)
            '    End If
            'End If
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = sqlstring
            ResultField = "Invoice_No, Currency, Forex_Rate, Supplier_Invoice, Base_Forex, PPN_Forex, Total_Forex, Base_Paid, PPN_Paid, Total_Paid, Amount_Saldo, PPN_Rate, Due_Date, FgValue, PPh_Forex, PPh_Paid, PPh_Saldo, Supplier, Supplier_Name, Type"
            CriteriaField = "Invoice_No, Supplier_Invoice, Supplier, Supplier_Name, Currency, Type"
            'Session("ClickSame") = "Bill_To"
            Session("Column") = ResultField.Split(",")
            Session("CriteriaField") = CriteriaField.Split(",")
            ResultSame = "Supplier"
            Session("ResultSame") = ResultSame.Split(",")
            ViewState("Sender") = "btnGetInv"
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Customer Click Error : " + ex.ToString
        End Try
    End Sub

    Public Sub ChangePaymentType2(ByVal Payment As String, ByRef TxFgMode As TextBox, ByVal txdate As BasicDatePicker, ByRef txduedate As BasicDatePicker, ByRef ddlbank As DropDownList, ByRef ddlCurr As DropDownList, ByRef txRate As TextBox, ByVal HomeCurrency As String, Optional ByVal DBConnection As String = "Nothing", Optional ByVal State As String = "Add")
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
                ChangeCurrency(ddlCurr, txdate, txRate, HomeCurrency, ViewState("DigitCurr"), DBConnection, ViewState("DigitHome"))
            End If
            txduedate.Enabled = TxFgMode.Text = "G"
            ddlbank.Enabled = TxFgMode.Text = "G"
        Catch ex As Exception
            Throw New Exception("Change Payment Error : " + ex.ToString)
        End Try
    End Sub
End Class
