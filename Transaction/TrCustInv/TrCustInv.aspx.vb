Imports System.Data
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO


Partial Class CustInv
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    'Protected GetStringHd As String = "Select * From V_FNCustInvHd"
    Private Function GetStringHd(ByVal Type As String) As String
        Return "Select distinct TransNmbr, Nmbr, TransDate, Status, FgReport, Customer, Customer_Name, CustTaxAddress, CustTaxNPWP, BillTo, Bill_To_Name, FgPriceIncludeTax, Attn, Term, Term_Name, DueDate, BankReceipt, Bank_Name, PPnNo, PPnDate, PPnRate, Currency, ForexRate, BaseForex, DiscForex, DPForex, PPn, PPnForex, PPnHome, TotalForex, TotalInvoice, Remark, UserPrep, DatePrep, UserAppr, DateAppr From V_FNCustInvHd "
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Try
            If Not IsPostBack Then
                InitProperty()
                lblJudul.Text = " "
                FillCombo(ddlTerm, "EXEC S_GetTerm", False, "Term_Code", "Term_Name", ViewState("DBConnection"))
                FillCombo(ddlCurr, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
                FillCombo(ddlUnitCommision, "EXEC S_GetUnit", False, "Unit_Code", "Unit_Code", ViewState("DBConnection"))
                lbCount.Text = SQLExecuteScalar("SELECT * FROM V_CountSJ", ViewState("DBConnection").ToString)
                SetInit()
                Session("AdvanceFilter") = ""
               
            End If
            GridDt.Columns(7).Visible = False
            GridDt.Columns(8).Visible = False
            lbStatus.Text = ""
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If
            ' hasil dari search dialog
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnCust" Then
                    '"Customer_Code, Customer_Name, Currency, Term, Contact_Person"
                    BindToText(tbCustCode, Session("Result")(0).ToString)
                    BindToText(tbCustName, Session("Result")(1).ToString)
                    BindToDropList(ddlCurr, Session("Result")(2).ToString)
                    BindToText(tbBillToCode, Session("Result")(0).ToString)
                    BindToText(tbBillToName, Session("Result")(1).ToString)
                    BindToText(tbAttn, TrimStr(Session("Result")(4).ToString))
                    'BindToDropList(ddlTerm, Session("Result")(3).ToString)
                    ddlCurr_SelectedIndexChanged(Nothing, Nothing)
                    ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection").ToString)
                    'tbDueDate.SelectedDate = FindDueDate(ddlTerm.SelectedValue, tbDate.SelectedDate, ViewState("DBConnection").ToString)
                    If Session("Result")(7).ToString = "Y" Then
                        tbPPN.Text = "10"
                    Else
                        tbPPN.Text = "0"
                    End If
                    Dim dr As DataRow
                    dr = SQLExecuteQuery("EXEC S_FNCustInvGetCust " + QuotedStr(tbCustCode.Text), ViewState("DBConnection").ToString).Tables(0).Rows(0)
                    If Not dr Is Nothing Then
                        BindToText(tbCustTaxAddress, dr("CustTaxAddress").ToString)
                        BindToText(tbCustTaxNPWP, dr("CustTaxNPWP").ToString)
                    Else
                        tbCustTaxAddress.Text = ""
                        tbCustTaxNPWP.Text = ""
                    End If
                End If
                If ViewState("Sender") = "btnCustTax" Then
                    BindToText(tbCustTaxAddress, Session("Result")(0).ToString)
                    BindToText(tbCustTaxNPWP, Session("Result")(1).ToString)
                End If
                If ViewState("Sender") = "btnBillTo" Then
                    '"Customer_Code, Customer_Name, Currency, Term, Contact_Person"
                    BindToText(tbBillToCode, Session("Result")(0).ToString)
                    BindToText(tbBillToName, Session("Result")(1).ToString)
                    BindToText(tbAttn, TrimStr(Session("Result")(2).ToString))
                End If
                If ViewState("Sender") = "btnSJNo" Then
                    tbSJNo.Text = Session("Result")(0).ToString
                    tbSONo.Text = Session("Result")(1).ToString
                    tbProductCode.Text = Session("Result")(2).ToString
                    tbProductName.Text = Session("Result")(3).ToString + " " + Session("Result")(4).ToString
                    tbQty.Text = Session("Result")(5).ToString
                    tbQtyM2.Text = Session("Result")(6).ToString
                    tbQtyRoll.Text = Session("Result")(7).ToString
                    tbUnit.Text = Session("Result")(8).ToString
                    tbQty.Text = FormatFloat(tbQty.Text, ViewState("DigitQty"))
                    tbQtyM2.Text = FormatFloat(tbQtyM2.Text, ViewState("DigitQty"))
                    tbQtyRoll.Text = FormatFloat(tbQtyRoll.Text, ViewState("DigitQty"))
                End If
                If ViewState("Sender") = "btnDPNo" Then
                    'ResultField = "DP_No, Currency, ForexRate, PPnRate, PPn, BaseForex, PPNForex, TotalForex, BasePaid, PPNPaid, TotalPaid "
                    tbDPNo.Text = Session("Result")(0).ToString
                    tbDPCurrency.Text = Session("Result")(1).ToString
                    ViewState("DigitCurrDt2") = SQLExecuteScalar("Select Digit FROM VMsCurrency WHERE Currency = " + QuotedStr(tbDPCurrency.Text), ViewState("DBConnection"))
                    tbDPRate.Text = FormatFloat(Session("Result")(2).ToString, ViewState("DigitRate"))
                    If tbDPCurrency.Text = ViewState("Currency") Then
                        tbDPRate.Enabled = False
                    End If
                    If tbDPCurrency.Text = ddlCurr.SelectedValue Then
                        tbDPRate.Enabled = False
                    End If
                    If tbDPCurrency.Text <> ddlCurr.SelectedValue Then
                        tbDPRate.Enabled = True
                    End If
                    tbDPPPnRate.Text = FormatFloat(Session("Result")(3).ToString, ViewState("DigitRate"))
                    tbDPPPnPercent.Text = FormatFloat(Session("Result")(4).ToString, ViewState("DigitPercent"))
                    tbDPBase.Text = FormatFloat(Session("Result")(5).ToString, ViewState("DigitCurrDt2"))
                    tbDPPPn.Text = FormatFloat(Session("Result")(6).ToString, ViewState("DigitCurrDt2"))
                    tbDPTotal.Text = FormatFloat(Session("Result")(7).ToString, ViewState("DigitCurrDt2"))
                    tbPaidBase.Text = FormatFloat(Session("Result")(8).ToString, ViewState("DigitCurrDt2"))
                    tbPaidPPN.Text = FormatFloat(Session("Result")(9).ToString, ViewState("DigitCurrDt2"))
                    tbPaidTotal.Text = FormatFloat(Session("Result")(10).ToString, ViewState("DigitCurrDt2"))
                    tbDPBaseForex.Text = FormatFloat(CFloat(tbDPBase.Text) - CFloat(tbPaidBase.Text), ViewState("DigitCurrDt2"))
                    tbDPPPnForex.Text = FormatFloat(CFloat(tbDPPPn.Text) - CFloat(tbPaidPPN.Text), ViewState("DigitCurrDt2"))
                    tbDPTotalForex.Text = FormatFloat(CFloat(tbDPBaseForex.Text) + CFloat(tbDPPPnForex.Text), ViewState("DigitCurrDt2"))
                    If tbDPCurrency.Text <> ddlCurr.SelectedValue Then
                        tbDPDPInvoice.Text = FormatFloat((CFloat(tbDPBaseForex.Text) * CFloat(tbDPRate.Text)) / CFloat(tbRate.Text), ViewState("DigitCurrDt2"))
                    Else
                        tbDPDPInvoice.Text = tbDPBaseForex.Text
                    End If
                End If
                If ViewState("Sender") = "btnGetDt" Then
                    Dim drResult As DataRow
                    Dim FirstTime As Boolean = True
                    For Each drResult In Session("Result").Rows
                        'insert
                        If FirstTime Then
                            If tbCustCode.Text = "" Then
                                BindToText(tbCustCode, drResult("Customer_Code"))
                                BindToText(tbCustName, drResult("Customer_Name"))
                                tbCustCode_TextChanged(Nothing, Nothing)
                            End If                            
                            BindToDropList(ddlReport, drResult("Report"))
                            ChangeReport("Add", "Y", True, tbDate, tbRate, tbPPnNo, tbPPndate, tbPpnRate) 'ddlReport.SelectedValue
                            BindToDropList(ddlTerm, drResult("Term"))
                            BindToDropList(ddlCurr, drResult("Currency"))
                            ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, ViewState("DBConnection").ToString)
                            'lbDigit.Text = ViewState("DigitCurr")
                            BindToDropList(ddlfgInclude, drResult("PriceIncludeTax"))
                            tbDueDate.SelectedDate = FindDueDate(ddlTerm.SelectedValue, tbDate.SelectedDate, ViewState("DBConnection"))
                        End If
                        ViewState("SJ_No") = drResult("SJ_No")

                        If CekExistData(ViewState("Dt"), "SJNo,Product", drResult("SJ_No") + "|" + drResult("Product_Code")) = False Then
                            Dim dr As DataRow
                            dr = ViewState("Dt").NewRow
                            dr("SJNo") = drResult("SJ_No")
                            dr("SONo") = drResult("SO_No")
                            dr("Product") = drResult("Product_Code")
                            dr("Product_Name") = drResult("Product_Name")
                            dr("Unit") = drResult("Unit")
                            dr("Qty") = FormatFloat(drResult("Qty"), ViewState("DigitQty"))
                            dr("QtyM2") = FormatFloat(drResult("QtyM2"), ViewState("DigitQty"))
                            dr("QtyRoll") = FormatFloat(drResult("QtyRoll"), ViewState("DigitQty"))
                            dr("PriceInUnit") = drResult("PriceInUnit")
                            dr("PriceForex") = drResult("PriceForex") 'FormatNumber()
                            dr("AmountForex") = drResult("AmountForex")
                            dr("Disc") = drResult("Disc")
                            dr("DiscForex") = drResult("DiscForex")
                            dr("NettoForex") = drResult("NettoForex")
                            dr("CommissionForex") = FormatFloat(0, ViewState("DigitCurr"))
                            'dr("DiscForex") = FormatFloat(drResult("DiscForex"), ViewState("DigitCurr"))
                            'dr("Disc") = FormatFloat(drResult("Disc"), ViewState("DigitCurr"))
                            'dr("AmountForex") = FormatFloat(drResult("AmountForex"), ViewState("DigitCurr"))
                            'dr("NettoForex") = FormatFloat(drResult("NettoForex"), ViewState("DigitCurr"))
                            ViewState("Dt").Rows.Add(dr)
                        End If
                        FirstTime = False
                    Next
                    ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
                    ChangeReport("Add", "Y", ddlCurr.SelectedValue = ViewState("Currency"), tbDate, tbRate, tbPPnNo, tbPPndate, tbPpnRate) 'ddlReport.SelectedValue
                    If ddlCurr.SelectedValue <> ViewState("Currency") Then 'And (ddlReport.SelectedValue = "Y")
                        tbPpnRate.Text = FormatFloat(FindTaxRate(ddlCurr.SelectedValue, tbPPndate.SelectedValue, ViewState("DBConnection").ToString), ViewState("DigitCurr"))
                    End If




                    BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    AttachScript("setformat();", Page, Me.GetType())
                End If

                If ViewState("Sender") = "btnOut" Then
                    BtnAdd_Click(Nothing, Nothing)
                    Dim drResult As DataRow
                    Dim FirstTime As Boolean = True
                    For Each drResult In Session("Result").Rows
                        'insert
                        If FirstTime Then
                            If tbCustCode.Text = "" Then
                                BindToText(tbCustCode, drResult("Customer_Code"))
                                BindToText(tbCustName, drResult("Customer_Name"))
                                tbCustCode_TextChanged(Nothing, Nothing)
                            End If
                            BindToDropList(ddlReport, drResult("Report"))
                            ChangeReport("Add", "Y", True, tbDate, tbRate, tbPPnNo, tbPPndate, tbPpnRate) 'ddlReport.SelectedValue
                            BindToDropList(ddlTerm, drResult("Term"))
                            BindToDropList(ddlCurr, drResult("Currency"))
                            ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, ViewState("DBConnection").ToString)
                            'lbDigit.Text = ViewState("DigitCurr")
                            BindToDropList(ddlfgInclude, drResult("PriceIncludeTax"))
                            tbDueDate.SelectedDate = FindDueDate(ddlTerm.SelectedValue, ViewState("ServerDate"), ViewState("DBConnection"))
                        End If

                        ViewState("SJ_No") = drResult("SJ_No")

                        If CekExistData(ViewState("Dt"), "SJNo,Product", drResult("SJ_No") + "|" + drResult("Product_Code")) = False Then
                            Dim dr As DataRow
                            dr = ViewState("Dt").NewRow
                            dr("SJNo") = drResult("SJ_No")
                            dr("SONo") = drResult("SO_No")
                            dr("Product") = drResult("Product_Code")
                            dr("Product_Name") = drResult("Product_Name")
                            dr("Unit") = drResult("Unit")
                            dr("Qty") = FormatFloat(drResult("Qty"), ViewState("DigitQty"))
                            dr("QtyM2") = FormatFloat(drResult("QtyM2"), ViewState("DigitQty"))
                            dr("QtyRoll") = FormatFloat(drResult("QtyRoll"), ViewState("DigitQty"))
                            dr("PriceInUnit") = drResult("PriceInUnit")
                            dr("PriceForex") = drResult("PriceForex") 'FormatNumber()
                            dr("AmountForex") = drResult("AmountForex")
                            dr("Disc") = drResult("Disc")
                            dr("DiscForex") = drResult("DiscForex")
                            dr("NettoForex") = drResult("NettoForex")
                            dr("CommissionForex") = FormatFloat(0, ViewState("DigitCurr"))
                            'dr("DiscForex") = FormatFloat(drResult("DiscForex"), ViewState("DigitCurr"))
                            'dr("Disc") = FormatFloat(drResult("Disc"), ViewState("DigitCurr"))
                            'dr("AmountForex") = FormatFloat(drResult("AmountForex"), ViewState("DigitCurr"))
                            'dr("NettoForex") = FormatFloat(drResult("NettoForex"), ViewState("DigitCurr"))
                            ViewState("Dt").Rows.Add(dr)
                        End If
                        FirstTime = False
                    Next
                    ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
                    ChangeReport("Add", "Y", ddlCurr.SelectedValue = ViewState("Currency"), tbDate, tbRate, tbPPnNo, tbPPndate, tbPpnRate) 'ddlReport.SelectedValue
                    If ddlCurr.SelectedValue <> ViewState("Currency") Then 'And (ddlReport.SelectedValue = "Y")
                        tbPpnRate.Text = FormatFloat(FindTaxRate(ddlCurr.SelectedValue, tbPPndate.SelectedValue, ViewState("DBConnection").ToString), ViewState("DigitCurr"))
                    End If




                    BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    AttachScript("setformat();", Page, Me.GetType())
                End If



                If Not ViewState("Sender") Is Nothing Then
                    ViewState("Sender") = Nothing
                End If
                If Not Session("Result") Is Nothing Then
                    Session("Result") = Nothing
                End If
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
        ViewState("DigitCurr") = 0
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection"))
        FillAction(BtnAdd, BtnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
        If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
            ddlCommand.Items.Add("Print")
            'ddlCommand2.Items.Add("Print Tax")
        End If
        tbBaseForex.Attributes.Add("ReadOnly", "True")
        tbPPNForex.Attributes.Add("ReadOnly", "True")
        tbDiscForex.Attributes.Add("ReadOnly", "True")
        tbTotalForex.Attributes.Add("ReadOnly", "True")
        tbTotalInvoice.Attributes.Add("ReadOnly", "True")

        tbAmountForex.Attributes.Add("ReadOnly", "True")
        tbTotalForexDt.Attributes.Add("ReadOnly", "True")

        tbRate.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbPPN.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbPriceForex.Attributes.Add("OnKeyDown", "return PressNumeric();")

        tbBaseForex.Attributes.Add("OnBlur", "setformat();")
        tbDiscForex.Attributes.Add("OnBlur", "setformat();")
        tbPPN.Attributes.Add("OnBlur", "setformat();")
        tbPPNForex.Attributes.Add("OnBlur", "setformat();")
        tbDPForex.Attributes.Add("OnBlur", "setformat();")
        tbTotalForex.Attributes.Add("OnBlur", "setformat();")
        tbTotalInvoice.Attributes.Add("OnBlur", "setformat();")

        tbQty.Attributes.Add("OnBlur", "setformatdt('');")
        tbPriceForex.Attributes.Add("OnBlur", "setformatdt('');")
        tbAmountForex.Attributes.Add("OnBlur", "setformatdt('');")

        tbDisc.Attributes.Add("OnBlur", "setformatdt('Disc');")
        tbDiscForexDt.Attributes.Add("OnBlur", "setformatdt('DiscForex');")
        tbTotalForexDt.Attributes.Add("OnBlur", "setformatdt('');")

        tbDPBaseForex.Attributes.Add("OnBlur", "setformatdt2();")
        tbDPPPnForex.Attributes.Add("OnBlur", "setformatdt2();")
        tbDPTotalForex.Attributes.Add("OnBlur", "setformatdt2();")
        tbDPRate.Attributes.Add("OnBlur", "setformatdt2();")
        tbDPDPInvoice.Attributes.Add("OnBlur", "setformatdt2();")
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
            DT = BindDataTransaction(GetStringHd(lblJudul.Text.Trim), StrFilter, ViewState("DBConnection").ToString)
            If DT.Rows.Count = 0 Then
                lbStatus.Text = "No Data"
                pnlNav.Visible = False
            End If
            ddlCommand.Visible = DT.Rows.Count > 0
            BtnGo.Visible = DT.Rows.Count > 0
            ddlCommand2.Visible = ddlCommand.Visible
            btnGo2.Visible = BtnGo.Visible
            BtnAdd2.Visible = BtnGo.Visible
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
        Return "SELECT * From V_FNCustInvDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function
    Private Function GetStringDP(ByVal Nmbr As String) As String
        Return "SELECT * From V_FNCustInvDP WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            GridView1.PageIndex = 0
            'ddlCommand.Visible = True
            'BtnGo.Visible = True
            Session("AdvanceFilter") = ""
            BindData(Session("AdvanceFilter"))
            pnlNav.Visible = True
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
                        If (GVR.Cells(3).Text = "P") Or (GVR.Cells(3).Text = "G") Or (GVR.Cells(3).Text = "H") Then
                            ListSelectNmbr = GVR.Cells(2).Text
                            If Pertamax Then
                                Result = "'''" + ListSelectNmbr + "''"
                                Pertamax = False
                            Else
                                Result = Result + ",''" + ListSelectNmbr + "''"
                            End If
                        End If
                    End If
                Next
                Result = Result + "'"
                Session("DBConnection") = ViewState("DBConnection")
                Session("SelectCommand") = "EXEC S_FNFormCustInv " + Result
                Session("ReportFile") = ".../../../Rpt/FormCustInv.frx"
                'lbStatus.Text = Session("SelectCommand")
                'Exit Sub

                AttachScript("openprintdlg();", Page, Me.GetType)

            ElseIf ActionValue = "Print Export" Then
                Dim GVR As GridViewRow
                Dim CB As CheckBox
                Dim Pertamax As Boolean

                Pertamax = True
                Result = ""

                For Each GVR In GridView1.Rows
                    CB = GVR.FindControl("cbSelect")
                    If CB.Checked Then
                        If (GVR.Cells(3).Text = "P") Or (GVR.Cells(3).Text = "G") Or (GVR.Cells(3).Text = "H") Then
                            ListSelectNmbr = GVR.Cells(2).Text
                            If Pertamax Then
                                Result = "'''" + ListSelectNmbr + "''"
                                Pertamax = False
                            Else
                                Result = Result + ",''" + ListSelectNmbr + "''"
                            End If
                        End If
                    End If
                Next
                Result = Result + "'"
                If Result.Length > 10 Then
                    If Request.QueryString("ContainerId").ToString = "CustInvAgentID" Then
                        Session("SelectCommand") = "EXEC S_FNFormCustInvExp " + Result + ", 'Customer' "
                        Session("ReportFile") = ".../../../Rpt/FormCustInvExp.frx"
                    End If
                    If Request.QueryString("ContainerId").ToString = "CustInvAffliasiID" Then
                        Session("SelectCommand") = "EXEC S_FNFormCustInvAff " + Result
                        Session("ReportFile") = ".../../../Rpt/FormCustInvAff.frx"
                    End If
                    'lbStatus.Text = Session("SelectCommand")
                    'Exit Sub
                    AttachScript("openprintdlg();", Page, Me.GetType)
                End If
            ElseIf ActionValue = "Print 2" Then
                Dim GVR As GridViewRow
                Dim CB As CheckBox
                Dim Pertamax As Boolean

                Pertamax = True
                Result = ""

                For Each GVR In GridView1.Rows
                    CB = GVR.FindControl("cbSelect")
                    If CB.Checked Then
                        If (GVR.Cells(3).Text = "P") Or (GVR.Cells(3).Text = "G") Or (GVR.Cells(3).Text = "H") Then
                            ListSelectNmbr = GVR.Cells(2).Text
                            If Pertamax Then
                                Result = "'''" + ListSelectNmbr + "''"
                                Pertamax = False
                            Else
                                Result = Result + ",''" + ListSelectNmbr + "''"
                            End If
                        End If
                    End If
                Next
                Result = Result + "'"
                If Result.Length > 10 Then
                    If Request.QueryString("ContainerId").ToString = "CustInvAgentID" Then
                        Session("SelectCommand") = "EXEC S_FNFormCustInv " + Result + ", 'Buyer' "
                        Session("ReportFile") = ".../../../Rpt/FormCustInv.frx"
                    End If
                    If Request.QueryString("ContainerId").ToString = "CustInvAffliasiID" Then
                        Session("SelectCommand") = "EXEC S_FNFormCustInvAff " + Result
                        Session("ReportFile") = ".../../../Rpt/FormCustInvAff.frx"
                    End If
                    AttachScript("openprintdlg();", Page, Me.GetType)
                End If
            ElseIf ActionValue = "Print Tax" Then
                Dim GVR As GridViewRow
                Dim CB As CheckBox
                Dim Pertamax As Boolean

                Pertamax = True
                Result = ""

                For Each GVR In GridView1.Rows
                    CB = GVR.FindControl("cbSelect")
                    If CB.Checked Then
                        If (GVR.Cells(3).Text = "P") Or (GVR.Cells(3).Text = "G") Or (GVR.Cells(3).Text = "H") Then
                            ListSelectNmbr = GVR.Cells(2).Text
                            If Pertamax Then
                                Result = "'''" + ListSelectNmbr + "''"
                                Pertamax = False
                            Else
                                Result = Result + ",''" + ListSelectNmbr + "''"
                            End If
                        End If
                    End If
                Next
                Result = Result + "'"
                If Result.Length > 10 Then
                    Session("SelectCommand") = "EXEC S_FNFormCustInvFPS " + Result
                    Session("ReportFile") = ".../../../Rpt/FormCustInvFaktur.frx"
                    AttachScript("openprintdlg();", Page, Me.GetType)
                End If
            Else
                Status = CekStatus(ActionValue)
                ListSelectNmbr = ""
                GetListCommand(Status, GridView1, "3,2", ListSelectNmbr, Nmbr, lbStatus.Text)
                If ListSelectNmbr = "" Then Exit Sub
                For j = 0 To (Nmbr.Length - 1)
                    If Nmbr(j) = "" Then
                        Exit For
                    Else
                        Result = ExecSPCommandGo(ActionValue, "S_FNCustInv", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            tbCustCode.Enabled = State
            btnCust.Visible = State
            ddlCurr.Enabled = State
            btnBillTo.Visible = True
            BtnGetData.Visible = State
            tbPPN.Enabled = State
            tbRate.Enabled = ddlCurr.SelectedValue <> ViewState("Currency")
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
            If dt.Rows.Count > 0 Then
                dr = dt.Rows(0)
                ViewState("SJ_No") = dr("SJNo").ToString
            End If
            
            'If ViewState("Dt") = Nothing Then
            '    GridDt.Columns(0).Visible = False
            'Else
            '    GridDt.Columns(0).Visible = True
            'End If

        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDP(ByVal Nmbr As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt2") = Nothing
            dt = SQLExecuteQuery(GetStringDP(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt2") = dt
            BindGridDt(dt, GridDt2)
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

    Protected Sub btnCancelDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelDt.Click
        Try
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt.Click
        Dim Row As DataRow
        Try
            If ViewState("StateDt") = "Edit" Then
                If ViewState("DtValue") <> tbSJNo.Text + "|" + tbProductCode.Text Then
                    If CekExistData(ViewState("Dt"), "SJNo,Product", tbSJNo.Text + "|" + tbProductCode.Text) Then
                        lbStatus.Text = "SJ No " + tbSJNo.Text + " Product " + tbProductName.Text + " has been already exist"
                        Exit Sub
                    End If
                End If
                Row = ViewState("Dt").Select("SJNo+'|'+Product = " + QuotedStr(ViewState("DtValue")))(0)
                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                Row("SJNo") = tbSJNo.Text
                Row("SONo") = tbSONo.Text
                Row("Product") = tbProductCode.Text
                Row("Product_Name") = tbProductName.Text
                Row("Qty") = tbQty.Text
                Row("QtyM2") = tbQtyM2.Text
                Row("QtyRoll") = tbQtyRoll.Text
                Row("Unit") = tbUnit.Text
                If Row("Unit") = "" Then
                    Row("Unit") = DBNull.Value
                End If
                Row("Remark") = tbRemarkDt.Text
                Row("PriceForex") = tbPriceForex.Text
                Row("AmountForex") = cekValue(tbAmountForex.Text)
                Row("Disc") = tbDisc.Text
                Row("DiscForex") = tbDiscForexDt.Text
                Row("NettoForex") = tbTotalForexDt.Text
                Row("CommissionForex") = tbcommision.Text
                Row("CommissionUnit") = ddlUnitCommision.SelectedValue
                Row("PriceInUnit") = ddlUnitWrhsDt.SelectedValue
                Row.EndEdit()

            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If
                If CekExistData(ViewState("Dt"), "SJNo,Product", tbSJNo.Text + "|" + tbProductCode.Text) = True Then
                    lbStatus.Text = MessageDlg("SJ No " + tbSJNo.Text + " Product " + tbProductName.Text + " has already been exist")
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("SJNo") = tbSJNo.Text
                dr("SONo") = tbSONo.Text
                dr("Product") = tbProductCode.Text
                dr("Product_Name") = tbProductName.Text
                dr("Qty") = tbQty.Text
                dr("QtyM2") = tbQtyM2.Text
                dr("QtyRoll") = tbQtyRoll.Text
                dr("Unit") = tbUnit.Text
                If dr("Unit") = "" Then
                    dr("Unit") = DBNull.Value
                End If
                dr("Remark") = tbRemarkDt.Text
                dr("PriceForex") = tbPriceForex.Text
                dr("AmountForex") = cekValue(tbAmountForex.Text)

                dr("Disc") = tbDisc.Text
                dr("DiscForex") = cekValue(tbDiscForexDt.Text)

                dr("NettoForex") = tbTotalForexDt.Text
                dr("CommissionForex") = tbcommision.Text
                dr("CommissionUnit") = ddlUnitCommision.SelectedValue
                dr("PriceInUnit") = ddlUnitWrhsDt.SelectedValue

                ViewState("Dt").Rows.Add(dr)
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

    Private Sub SaveAll()
        Dim SQLString As String
        Dim I As Integer
        Try            
            If pnlEditDt.Visible = True Or pnlEditDt2.Visible = True Then
                lbStatus.Text = "Detail Data must be saved first"
                Exit Sub
            End If
            If Not (ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit") Then
                Exit Sub
            Else
                ViewState("StateSave") = ViewState("StateHd")
                ViewState("StateHd") = "View"
            End If
            
            Dim CekSJ As String
            CekSJ = "N"
            CekSJ = SQLExecuteScalar("SELECT Report FROM V_STSJPendingHdForCI WHERE SJ_No = " + QuotedStr(ViewState("SJ_No").ToString), ViewState("DBConnection").ToString)
            If ddlReport.SelectedValue <> CekSJ Then
                ddlReport.SelectedValue = CekSJ
            End If
            
            'Save Hd
            If ViewState("StateSave") = "Insert" Then
                'ddlReport.SelectedValue
                tbCode.Text = GetAutoNmbrParamAll("CIA", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
                'End If
                SQLString = "INSERT INTO FINCustInvHd (TransNmbr, FgReport, Status, TransDate, " + _
                "Customer, Attn, BillTo, CustTaxAddress, CustTaxNPWP, " + _
                "Term, DueDate, FgPriceIncludeTax, PPnNo, PPnDate, PPnRate, Currency, ForexRate, " + _
                "BaseForex, DiscForex, DPForex, PPn, PPnForex, TotalForex, TotalInvoice, " + _
                "Remark, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(ddlReport.SelectedValue) + _
                ", 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', " + _
                QuotedStr(tbCustCode.Text) + ", " + QuotedStr(tbAttn.Text) + ", " + QuotedStr(tbBillToCode.Text) + ", " + _
                QuotedStr(tbCustTaxAddress.Text) + ", " + QuotedStr(tbCustTaxNPWP.Text) + ", " + _
                QuotedStr(ddlTerm.SelectedValue) + ", '" + Format(tbDueDate.SelectedValue, "yyyy-MM-dd") + "', " + QuotedStr(ddlfgInclude.SelectedValue) + ", " + _
                QuotedStr(tbPPnNo.Text) + ", '" + Format(tbPPndate.SelectedValue, "yyyy-MM-dd") + "', " + tbPpnRate.Text.Replace(",", "") + ", " + _
                QuotedStr(ddlCurr.SelectedValue) + ", " + tbRate.Text.Replace(",", "") + ", " + _
                tbBaseForex.Text.Replace(",", "") + ", " + tbDiscForex.Text.Replace(",", "") + ", " + tbDPForex.Text.Replace(",", "") + ", " + _
                tbPPN.Text.Replace(",", "") + ", " + tbPPNForex.Text.Replace(",", "") + ", " + _
                tbTotalForex.Text.Replace(",", "") + ", " + tbTotalInvoice.Text.Replace(",", "") + ", " + _
                QuotedStr(tbRemark.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", getDate()"

            Else
                'InvNo = SQLExecuteScalar("Select TOP 1 TransNmbr from FINCustInvHd WHERE Reference = " + QuotedStr(tbRef.Text) + " AND Status <> 'D' AND TransNmbr <> " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                'If InvNo.ToString.Length >= 5 Then
                '    lbStatus.Text = "Save failed... SO No " + QuotedStr(tbRef.Text) + " has already used by Invoice No " + QuotedStr(InvNo)
                '    Exit Sub
                'End If
                Dim CekStatus As String
                CekStatus = SQLExecuteScalar("Select Status FROM FINCustInvHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                If CekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                'Dim UserBook As String
                'UserBook = SQLExecuteScalar("Select UserEdit FROM FINCustInvHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                'If UserBook <> ViewState("UserId").ToString Then
                '    lbStatus.Text = "Save failed... Data has already modified by another user (" + UserBook + ")"
                '    Exit Sub
                'End If
                Dim Stat As String
                If ViewState("StatusTrans") = "G" Then
                    Stat = "H"
                Else : Stat = ViewState("StatusTrans")
                End If
                SQLString = "UPDATE FINCustInvHd SET Customer = " + QuotedStr(tbCustCode.Text) + ", Attn = " + QuotedStr(tbAttn.Text) + _
                ", FgPriceIncludeTax = " + QuotedStr(ddlfgInclude.SelectedValue) + ", Term = " + QuotedStr(ddlTerm.SelectedValue) + ", BillTo = " + QuotedStr(tbBillToCode.Text) + _
                ", PPnNo = " + QuotedStr(tbPPnNo.Text) + _
                ", CustTaxAddress = " + QuotedStr(tbCustTaxAddress.Text) + ", CustTaxNPWP = " + QuotedStr(tbCustTaxNPWP.Text) + _
                ", PPnDate = '" + Format(tbPPndate.SelectedValue, "yyyy-MM-dd") + "', PPnRate = " + tbPpnRate.Text.Replace(",", "") + _
                ", DueDate = '" + Format(tbDueDate.SelectedValue, "yyyy-MM-dd") + "'" + _
                ", Currency = " + QuotedStr(ddlCurr.SelectedValue) + ", ForexRate = " + tbRate.Text.Replace(",", "") + _
                ", BaseForex = " + tbBaseForex.Text.Replace(",", "") + ", DiscForex = " + tbDiscForex.Text.Replace(",", "") + ", PPn = " + tbPPN.Text.Replace(",", "") + ", PPnForex = " + tbPPNForex.Text.Replace(",", "") + _
                ", DPForex = " + tbDPForex.Text.Replace(",", "") + _
                ", TotalForex = " + tbTotalForex.Text.Replace(",", "") + _
                ", TotalInvoice = " + tbTotalInvoice.Text.Replace(",", "") + _
                ", Remark = " + QuotedStr(tbRemark.Text) + _
                ", TransDate = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', DatePrep = getDate()" + _
                ", Status = " + QuotedStr(Stat) + _
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
            Dim ConnString As String = ViewState("DBConnection").ToString 'System.Configuration.ConfigurationManager.AppSettings.Get("ConString")
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, SJNo, SONo, Product, Qty, QtyM2, QtyRoll, Unit, PriceForex, AmountForex, Disc, DiscForex, " + _
                                        "NettoForex, PriceInUnit, CommissionForex, CommissionUnit, Remark FROM FINCustInvDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
            "UPDATE FINCustInvDt SET SJNo = @SJNo, SONo = @SONo, " + _
            "Product = @Product, Qty = @Qty, Unit = @Unit, " + _
            "QtyM2 = @QtyM2, QtyRoll = @QtyRoll, Remark = @Remark, " + _
            "PriceForex = @PriceForex, AmountForex = @AmountForex, Disc = @Disc, " + _
            "DiscForex = @DiscForex, NettoForex = @NettoForex, PriceInUnit = @PriceInUnit, " + _
            "CommissionForex = @CommissionForex, CommissionUnit = @CommissionUnit " + _
            "WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND SJNo = @OldSJNo AND Product = @OldProduct", con)

            ' Define output parameters.
            Update_Command.Parameters.Add("@SJNo", SqlDbType.VarChar, 20, "SJNo")
            Update_Command.Parameters.Add("@SONo", SqlDbType.VarChar, 20, "SONo")
            Update_Command.Parameters.Add("@Product", SqlDbType.VarChar, 20, "Product")
            Update_Command.Parameters.Add("@Qty", SqlDbType.Float, 18, "Qty")
            Update_Command.Parameters.Add("@Unit", SqlDbType.VarChar, 5, "Unit")
            Update_Command.Parameters.Add("@QtyM2", SqlDbType.Float, 18, "QtyM2")
            Update_Command.Parameters.Add("@QtyRoll", SqlDbType.Float, 18, "QtyRoll")
            Update_Command.Parameters.Add("@PriceForex", SqlDbType.Float, 23, "PriceForex")
            Update_Command.Parameters.Add("@AmountForex", SqlDbType.Float, 23, "AmountForex")
            Update_Command.Parameters.Add("@Disc", SqlDbType.Float, 23, "Disc")
            Update_Command.Parameters.Add("@DiscForex", SqlDbType.Float, 23, "DiscForex")
            Update_Command.Parameters.Add("@NettoForex", SqlDbType.Float, 23, "NettoForex")
            Update_Command.Parameters.Add("@PriceInUnit", SqlDbType.VarChar, 20, "PriceInUnit")
            Update_Command.Parameters.Add("@CommissionForex", SqlDbType.Float, 23, "CommissionForex")
            Update_Command.Parameters.Add("@CommissionUnit", SqlDbType.VarChar, 5, "CommissionUnit")
            Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 255, "Remark")

            '' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@OldSJNo", SqlDbType.VarChar, 20, "SJNo")
            param.SourceVersion = DataRowVersion.Original
            param = Update_Command.Parameters.Add("@OldProduct", SqlDbType.VarChar, 20, "Product")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM FINCustInvDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND SJNo = @SJNo AND Product = @Product ", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@SJNo", SqlDbType.VarChar, 20, "SJNo")
            param.SourceVersion = DataRowVersion.Original
            param = Delete_Command.Parameters.Add("@Product", SqlDbType.VarChar, 20, "Product")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("FINCustInvDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()

            ViewState("Dt") = Dt

            Dim cmdSql2 As New SqlCommand("SELECT  TransNmbr, DPNo, Currency, ForexRate, PPn, PPnRate, DPBase, DPPPn, DPTotal, PaidBase, PaidPPN, PaidTotal, BaseForex, PPnForex, TotalForex, DPInvoice, Remark FROM FINCustInvDP WHERE TransNmbr = '" & ViewState("TransNmbr") & "'", con)
            da = New SqlDataAdapter(cmdSql2)
            Dim dbcommandBuilder2 As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder2.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder2.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder2.GetUpdateCommand

            Dim param2 As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command2 = New SqlCommand( _
            "UPDATE FINCustInvDP SET DPNo = @DPNo, Currency = @Currency, " + _
            "ForexRate = @ForexRate, PPn = @PPn, PPnRate = @PPnRate, " + _
            "DPBase = @DPBase, DPPPn = @DPPPn, DPTotal = @DPTotal, " + _
            "PaidBase = @PaidBase, PaidPPN = @PaidPPN, PaidTotal = @PaidTotal, " + _
            "BaseForex = @BaseForex, PPnForex = @PPnForex, TotalForex = @TotalForex, " + _
            "DPInvoice = @DPInvoice " + _
            "WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND DPNo = @OldDPNo", con)

            ' Define output parameters.
            Update_Command2.Parameters.Add("@DPNo", SqlDbType.VarChar, 20, "DPNo")
            Update_Command2.Parameters.Add("@Currency", SqlDbType.VarChar, 5, "Currency")
            Update_Command2.Parameters.Add("@ForexRate", SqlDbType.Float, 18, "ForexRate")
            Update_Command2.Parameters.Add("@PPn", SqlDbType.Float, 18, "PPn")
            Update_Command2.Parameters.Add("@PPnRate", SqlDbType.Float, 18, "PPnRate")
            Update_Command2.Parameters.Add("@DPBase", SqlDbType.Float, 22, "DPBase")
            Update_Command2.Parameters.Add("@DPPPn", SqlDbType.Float, 22, "DPPPn")
            Update_Command2.Parameters.Add("@DPTotal", SqlDbType.Float, 22, "DPTotal")
            Update_Command2.Parameters.Add("@PaidBase", SqlDbType.Float, 22, "PaidBase")
            Update_Command2.Parameters.Add("@PaidPPN", SqlDbType.Float, 22, "PaidPPN")
            Update_Command2.Parameters.Add("@PaidTotal", SqlDbType.Float, 22, "PaidTotal")
            Update_Command2.Parameters.Add("@BaseForex", SqlDbType.Float, 22, "BaseForex")
            Update_Command2.Parameters.Add("@PPnForex", SqlDbType.Float, 22, "PPnForex")
            Update_Command2.Parameters.Add("@TotalForex", SqlDbType.Float, 22, "TotalForex")
            Update_Command2.Parameters.Add("@DPInvoice", SqlDbType.Float, 22, "DPInvoice")

            '' Define intput (WHERE) parameters.
            param2 = Update_Command2.Parameters.Add("@OldDPNo", SqlDbType.VarChar, 20, "DPNo")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command2

            ' Create the DeleteCommand.
            Dim Delete_Command2 = New SqlCommand( _
                "DELETE FROM FINCustInvDP WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND DPNo = @DPNo ", con)
            ' Add the parameters for the DeleteCommand.
            param2 = Delete_Command2.Parameters.Add("@DPNo", SqlDbType.VarChar, 20, "DPNo")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command2

            Dim Dt2 As New DataTable("FINCustInvDP")
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
            tbFilter.Text = tbCode.Text
            ddlField.SelectedValue = "TransNmbr"
            btnSearch_Click(Nothing, Nothing)
            tbFilter.Text = CurrFilter
            ddlField.SelectedValue = Value
        Catch ex As Exception
            lbStatus.Text = "Save All Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click, BtnAdd2.Click
        Try
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, pnlDt, GridDt)
            newTrans()
            ChangeReport("Add", "Y", True, tbDate, tbRate, tbPPnNo, tbPPndate, tbPpnRate) 'ddlReport.SelectedValue
            MultiView1.ActiveViewIndex = 0
            Menu1.Items.Item(0).Selected = True
            btnHome.Visible = False
            'ddlReport.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ClearHd()
            Cleardt()
            ClearDP()
            ViewState("DigitCurr") = 0
            'ddlCurr.SelectedValue = ViewState("Currency").ToString
            'ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, ViewState("DBConnection").ToString)
            pnlDt.Visible = True
            BindDataDt("")
            BindDataDP("")
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbCode.Text = ""
            tbDueDate.SelectedDate = ViewState("ServerDate") 'Today
            tbCustCode.Text = ""
            tbCustName.Text = ""
            tbBillToCode.Text = ""
            tbBillToName.Text = ""
            tbAttn.Text = ""
            ddlTerm.SelectedIndex = 0
            tbPPnNo.Text = ""
            tbPPndate.SelectedDate = Nothing
            tbPpnRate.Text = ""
            ddlCurr.SelectedValue = ViewState("Currency")
            tbRate.Text = "0"
            tbBaseForex.Text = "0"
            'tbDisc.Text = "0"
            tbDiscForex.Text = "0"
            'tbPPN.Text = "10"
            tbPPN.Text = "0" ' sesuai dengan FgPPn di MsCustomer
            tbPPNForex.Text = "0"
            tbTotalForex.Text = "0"
            tbRemark.Text = ""
            tbDate.SelectedDate = ViewState("ServerDate") 'Today
            ddlfgInclude.SelectedValue = "N"
            ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
            tbDPNo.Text = ""
            tbDPCurrency.Text = ""
            tbDPRate.Text = "0"
            tbDPPPnRate.Text = "0"
            tbDPPPnPercent.Text = "0"
            tbDPBase.Text = "0"
            tbDPPPn.Text = "0"
            tbDPTotal.Text = "0"
            tbPaidBase.Text = "0"
            tbPaidPPN.Text = "0"
            tbPaidTotal.Text = "0"
            tbDPBaseForex.Text = "0"
            tbDPPPnForex.Text = "0"
            tbDPTotalForex.Text = "0"
            tbDPRemarkDt.Text = ""
            tbCustTaxAddress.Text = ""
            tbCustTaxNPWP.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbSJNo.Text = ""
            tbSONo.Text = ""
            tbProductCode.Text = ""
            tbProductName.Text = ""
            tbRemarkDt.Text = ""
            tbQty.Text = "0"
            tbQtyM2.Text = "0"
            tbQtyRoll.Text = "0"
            tbPriceForex.Text = "0"
            tbAmountForex.Text = "0"
            tbDisc.Text = "0"
            tbDiscForexDt.Text = "0"
            tbTotalForexDt.Text = "0"
            tbcommision.Text = "0"
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

    Private Sub totalingDt()
        Dim dr As DataRow
        Dim amount, disc, total, TotalDP, Base As Double
        Try
            total = 0
            disc = 0
            amount = 0
            TotalDP = 0
            For Each dr In ViewState("Dt").Rows
                If Not dr.RowState = DataRowState.Deleted Then
                    amount = amount + CFloat(dr("AmountForex").ToString)
                    disc = disc + CFloat(dr("DiscForex").ToString)
                    total = total + CFloat(dr("NettoForex").ToString)
                End If
            Next
            ViewState("DigitCurr") = SQLExecuteScalar("Select Digit FROM VMsCurrency WHERE Currency = " + QuotedStr(ddlCurr.SelectedValue), ViewState("DBConnection"))
            If ddlfgInclude.SelectedValue = "N" Then
                tbBaseForex.Text = FormatFloat(amount, CInt(ViewState("DigitCurr")))
                Base = amount
            Else
                '(Total Netto Forex * 100 / (100+PPn)) + Disc Forex
                tbBaseForex.Text = FormatFloat((total * 100 / (100 + CFloat(tbPPN.Text))) + disc, CInt(ViewState("DigitCurr")))
                Base = (total * 100 / (100 + CFloat(tbPPN.Text))) + disc
            End If
            tbDiscForex.Text = FormatFloat(disc, CInt(ViewState("DigitCurr")))

            If Not ViewState("Dt2") Is Nothing Then
                For Each dr In ViewState("Dt2").Rows
                    If Not dr.RowState = DataRowState.Deleted Then
                        TotalDP = TotalDP + CFloat(dr("DPInvoice").ToString)
                    End If
                Next
            End If
            
            'TotalDP = GetTotalSum(ViewState("Dt2"), "DPInvoice")
            tbDPForex.Text = FormatFloat(TotalDP, ViewState("DigitCurr"))

            'If ddlCurr.SelectedValue = ViewState("Currency") Then
            '    total = Math.Floor(total)
            'End If
            tbTotalForex.Text = FormatFloat(CFloat(tbBaseForex.Text) - CFloat(tbDiscForex.Text) - CFloat(tbDPForex.Text), ViewState("DigitCurr"))
            'tbPPNForex.Text = FormatFloat((CFloat(tbPPN.Text) / 100) * CFloat(tbTotalForex.Text), ViewState("DigitCurr"))
            tbPPNForex.Text = FormatFloat((CFloat(tbPPN.Text) / 100) * (Base - disc - TotalDP), ViewState("DigitCurr"))
            tbTotalInvoice.Text = FormatFloat(CFloat(tbTotalForex.Text) + CFloat(tbPPNForex.Text), ViewState("DigitCurr"))
            AttachScript("setformat();", Page, Me.GetType())
        Catch ex As Exception
            Throw New Exception("Totaling Dt Error : " + ex.ToString)
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
                If CekDt(dr) = False Then
                    Exit Sub
                End If
            Next
            SaveAll()
            newTrans()
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
        Catch ex As Exception
            lbStatus.Text = "btn save all error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCust_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCust.Click
        Dim ResultField As String
        Try
            Session("filter") = "select * from VMsCustomer WHERE FgActive = 'Y'"
            'Session("filter") = "EXEC S_FNCustInvGetCust ''"
            ResultField = "Customer_Code, Customer_Name, Currency, Term, Contact_Person, Address, NPWP, PPN"
            ViewState("Sender") = "btnCust"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Cust Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbCustCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCustCode.TextChanged
        Dim Dt As DataTable
        Dim dr As DataRow
        Try
            'Dr = FindMaster("Customer", tbCustCode.Text, ViewState("DBConnection").ToString)
            Dt = SQLExecuteQuery("EXEC S_FNCustInvGetCust " + QuotedStr(tbCustCode.Text), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                dr = Dt.Rows(0)
                tbCustCode.Text = dr("Customer_Code").ToString
                tbCustName.Text = dr("Customer_Name").ToString
                BindToDropList(ddlCurr, dr("Currency").ToString)
                'BindToDropList(ddlTerm, Dr("Term").ToString)
                'tbDueDate.SelectedDate = FindDueDate(ddlTerm.SelectedValue, tbDate.SelectedDate, ViewState("DBConnection"))
                BindToText(tbBillToCode, dr("Customer_Code").ToString)
                BindToText(tbBillToName, dr("Customer_Name").ToString)
                BindToText(tbAttn, dr("Contact_Person").ToString)
                If dr("PPn").ToString = "Y" Then
                    tbPPN.Text = "10"
                Else
                    tbPPN.Text = "0"
                End If
                BindToText(tbCustTaxAddress, dr("CustTaxAddress").ToString)
                BindToText(tbCustTaxNPWP, dr("CustTaxNPWP").ToString)
            Else
                tbCustCode.Text = ""
                tbCustName.Text = ""
                ddlCurr.SelectedValue = ViewState("Currency")
                tbBillToCode.Text = ""
                tbBillToName.Text = ""
                tbAttn.Text = ""
                tbPPN.Text = "0"
                tbCustTaxAddress.Text = ""
                tbCustTaxNPWP.Text = ""
            End If
            ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
            'AttachScript("setformat();", Page, Me.GetType())
            tbCustCode.Focus()
        Catch ex As Exception
            Throw New Exception("tb CustCode Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlCurr_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCurr.SelectedIndexChanged
        If ViewState("InputCurrency") = "Y" Then
            RefreshMaster("S_GetCurrency", "Currency", "Currency", ddlCurr, ViewState("DBConnection"))
            ViewState("InputCurrency") = Nothing
        End If
        ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
        ChangeReport("Edit", "Y", ddlCurr.SelectedValue = ViewState("Currency"), tbDate, tbRate, tbPPnNo, tbPPndate, tbPpnRate) 'ddlReport.SelectedValue
        If tbPPndate.Enabled Then
            tbPpnRate.Text = FormatFloat(FindTaxRate(ddlCurr.SelectedValue, tbPPndate.SelectedValue, ViewState("DBConnection").ToString), ViewState("DigitRate"))
        End If
        'AttachScript("setformat();", Page, Me.GetType())
        tbRate.Focus()
    End Sub

    'Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt.Click, btnAddDt2.Click
    '    Cleardt()
    '    If CekHd() = False Then
    '        Exit Sub
    '    End If
    '    ViewState("StateDt") = "Insert"
    '    MovePanel(pnlDt, pnlEditDt)
    '    EnableHd(False)
    '    StatusButtonSave(False)
    '    tbProductCode.Focus()
    'End Sub

    Function CekHd() As Boolean
        Try

            If tbDate.IsNull Then
                lbStatus.Text = MessageDlg("Customer Invoice Date must have value")
                tbDate.Focus()
                Return False
            End If
            'If CInt(Session(Request.QueryString("KeyId"))("Year")) <> Year(tbDate.SelectedValue) Or CInt(Session(Request.QueryString("KeyId"))("Period")) <> Month(tbDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(Session(Request.QueryString("KeyId"))("Period")) + " " + Session(Request.QueryString("KeyId"))("Year").ToString.Trim)
            '    Return False
            'End If
            If tbCustCode.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Customer must have value")
                btnCust.Focus()
                Return False
            End If
            If tbBillToCode.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Bill To must have value")
                tbBillToCode.Focus()
                Return False
            End If
            If ddlCurr.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Currency must have value")
                ddlCurr.Focus()
                Return False
            End If
            If CFloat(tbRate.Text) <= 0 Then
                lbStatus.Text = MessageDlg("Rate must have value")
                tbRate.Focus()
                Return False
            End If
            If ddlCurr.SelectedValue <> ViewState("Currency") And CFloat(tbRate.Text) = 1 Then
                lbStatus.Text = MessageDlg("Rate must have value")
                tbRate.Focus()
                Return False
            End If
            If ddlTerm.SelectedValue.Trim = "" Then
                lbStatus.Text = MessageDlg("Term must have value")
                ddlTerm.Focus()
                Return False
            End If
            If tbDueDate.IsNull Then
                lbStatus.Text = MessageDlg("Due Date must have value")
                tbDueDate.Focus()
                Return False
            End If
            Dim SQLStr, Hasil As String
            SQLStr = "DECLARE @A VARCHAR(255) " + _
                "EXEC S_FNCekBalance " + FormatFloat(CFloat(tbPPN.Text), 4).Replace(",", "") + ", " + FormatFloat(CFloat(tbPPNForex.Text), 4).Replace(",", "") + _
                "," + FormatFloat(CFloat(tbBaseForex.Text), 4).Replace(",", "") + "," + FormatFloat(CFloat(tbDiscForex.Text), 4).Replace(",", "") + _
                "," + FormatFloat(CFloat(tbDPForex.Text), 4).Replace(",", "") + "," + FormatFloat(CFloat(tbTotalInvoice.Text), 4).Replace(",", "") + _
                ", @A OUT SELECT @A"

            Hasil = SQLExecuteScalar(SQLStr, ViewState("DBConnection"))
            Hasil = Replace(Hasil, "0", "")

            If Trim(Hasil) <> "" Then
                lbStatus.Text = MessageDlg(Hasil)
                Return False
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Ceh Hd Error : " + ex.ToString)
        End Try
    End Function

    Function CekDt(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not (Dr Is Nothing) Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("SJNo").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("SJ No Must Have Value")
                    Return False
                End If
                If Dr("SONo").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("SO No Must Have Value")
                    Return False
                End If
                If Dr("Product").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Product Must Have Value")
                    Return False
                End If
                If Dr("Unit").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Unit Order Must Have Value")
                    Return False
                End If
                If CFloat(Dr("Qty").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Must Have Value")
                    Return False
                End If
            Else
                If tbSJNo.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("SJ No Must Have Value")
                    tbSJNo.Focus()
                    Return False
                End If
                If tbSONo.Text.Trim = "" Then
                    lbStatus.Text = "SO No Must Have Value"
                    tbSONo.Focus()
                    Return False
                End If
                If tbProductCode.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Product Must Have Value")
                    tbProductCode.Focus()
                    Return False
                End If
                If tbUnit.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Unit Order Must Have Value")
                    tbUnit.Focus()
                    Return False
                End If
                If CFloat(tbQty.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Must Have Value")
                    tbQty.Focus()
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
            If Not (Dr Is Nothing) Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("DPNo").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("DP No Must Have Value")
                    Return False
                End If
                If CFloat(Dr("TotalForex").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("To be Paid Must Have Value")
                    Return False
                End If
            Else
                If tbDPNo.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("DP No Must Have Value")
                    btnDPNo.Focus()
                    Return False
                End If
                If CFloat(tbDPBaseForex.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("To be Paid Must Have Value")
                    tbDPBaseForex.Focus()
                    Return False
                End If

            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt2 Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        CheckAll(GridView1, sender)
    End Sub

    Protected Sub LbAdvSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LbAdvSearch.Click
        Dim FDateName, FDateValue, FilterName, FilterValue As String
        Try
            FDateName = "Date, Due Date"
            FDateValue = "TransDate, DueDate"
            FilterName = "Invoice No, Date, Customer, Bill To, PPn No, Currency, Term, Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), Customer, BillTo, PPnNo, Currency, Term_Name, Remark"
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
        'Dim Result, ListSelectNmbr As String
        Dim CekMenu As String
        Dim index As Integer
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
                    ChangeReport("View", "Y", ddlCurr.SelectedValue = ViewState("Currency"), tbDate, tbRate, tbPPnNo, tbPPndate, tbPpnRate) 'ddlReport.SelectedValue
                    BindDataDt(ViewState("TransNmbr"))
                    BindDataDP(ViewState("TransNmbr"))
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, pnlDt, GridDt)
                    ModifyInput2(False, pnlInput, pnlDt, GridDt2)
                    MultiView1.ActiveViewIndex = 0
                    Menu1.Items.Item(0).Selected = True
                    btnHome.Visible = True
                    btnAddDP2.Visible = False
                    btnAddDPKe2.Visible = False

                ElseIf DDL.SelectedValue = "Edit" Then
                    If GVR.Cells(3).Text = "H" Or GVR.Cells(3).Text = "G" Then
                        CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        MovePanel(PnlHd, pnlInput)
                        MultiView1.ActiveViewIndex = 0
                        Menu1.Items.Item(0).Selected = True
                        ViewState("TransNmbr") = GVR.Cells(2).Text
                        ViewState("StatusTrans") = GVR.Cells(3).Text
                        GridDt.PageIndex = 0
                        FillTextBoxHd(ViewState("TransNmbr"))
                        BindDataDt(ViewState("TransNmbr"))                       
                        BindDataDP(ViewState("TransNmbr"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        ModifyInput2(True, pnlInput, pnlDt, GridDt2)                        
                        btnAddDP2.Visible = True
                        btnAddDPKe2.Visible = True
                        btnHome.Visible = False
                        If tbCustName.Text.Trim = "" Then
                            GridDt.Columns(0).Visible = False
                        End If
                        'ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, ViewState("DBConnection").ToString)                        
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                        'SQLExecuteNonQuery("Update FINCustInvHd SET UserEdit = " + QuotedStr(ViewState("UserId").ToString) + " WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), ViewState("DBConnection").ToString)
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

                        Dim SQLStr, Hasil As String
                        SQLStr = "DECLARE @A VARCHAR(255) " + _
                            "EXEC S_FNCustInvCekAmount " + QuotedStr(GVR.Cells(2).Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ",'Print', @A OUT " + _
                            "SELECT @A"

                        Hasil = SQLExecuteScalar(SQLStr, ViewState("DBConnection"))
                        Hasil = Replace(Hasil, "0", "")

                        If Trim(Hasil) <> "" Then
                            lbStatus.Text = MessageDlg(Hasil)
                            Exit Sub
                        End If

                        'If (GVR.Cells(3).Text = "P") Or (GVR.Cells(3).Text = "G") Or (GVR.Cells(3).Text = "H") Then
                        If (GVR.Cells(3).Text = "P") Then '2015-06-03-0191
                            Session("DBConnection") = ViewState("DBConnection")
                            Session("SelectCommand") = "EXEC S_FNFormCustInv '''" + GVR.Cells(2).Text + "'''"
                            Session("ReportFile") = ".../../../Rpt/FormCustInv.frx"
                            AttachScript("openprintdlg();", Page, Me.GetType)
                        Else
                            'lbStatus.Text = "Cannot print or preview, status Invoice No " + QuotedStr(GVR.Cells(2).Text) + " must be posted or get approval"
                            lbStatus.Text = MessageDlg("Data must be Posted to Print") '"Cannot print or preview, status Invoice No " + QuotedStr(GVR.Cells(2).Text) + " must be posted"
                            Exit Sub
                        End If

                    Catch ex As Exception
                        lbStatus.Text = "btn print Error = " + ex.ToString
                    End Try
                ElseIf DDL.SelectedValue = "Print Export" Then
                    Try
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        'If (GVR.Cells(3).Text = "P") Or (GVR.Cells(3).Text = "G") Or (GVR.Cells(3).Text = "H") Then
                        If (GVR.Cells(3).Text = "P") Then
                            Session("DBConnection") = ViewState("DBConnection")
                            If Request.QueryString("ContainerId").ToString = "CustInvAgentID" Then
                                Session("SelectCommand") = "EXEC S_FNFormCustInvExp '''" + GVR.Cells(2).Text + "''', 'Customer' "
                                Session("ReportFile") = ".../../../Rpt/FormCustInvExp.frx"
                            End If
                            If Request.QueryString("ContainerId").ToString = "CustInvAffliasiID" Then
                                Session("SelectCommand") = "EXEC S_FNFormCustInvAff '''" + GVR.Cells(2).Text + "'''"
                                Session("ReportFile") = ".../../../Rpt/FormCustInvAff.frx"
                            End If
                            'lbStatus.Text = Session("SelectCommand")
                            'Exit Sub

                            AttachScript("openprintdlg();", Page, Me.GetType)
                        Else
                            lbStatus.Text = MessageDlg("Data must be Posted to Print") '"Cannot print or preview, status Invoice No " + QuotedStr(GVR.Cells(2).Text) + " must be posted or get approval"
                            Exit Sub
                        End If
                    Catch ex As Exception
                        lbStatus.Text = "btn print Error = " + ex.ToString
                    End Try
                ElseIf DDL.SelectedValue = "Print 2" Then
                    Try
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        'If (GVR.Cells(3).Text = "P") Or (GVR.Cells(3).Text = "G") Or (GVR.Cells(3).Text = "H") Then
                        If (GVR.Cells(3).Text = "P") Then
                            Session("DBConnection") = ViewState("DBConnection")
                            If Request.QueryString("ContainerId").ToString = "CustInvAgentID" Then
                                Session("SelectCommand") = "EXEC S_FNFormCustInv '''" + GVR.Cells(2).Text + "''', 'Buyer' "
                                Session("ReportFile") = ".../../../Rpt/FormCustInv.frx"
                            End If
                            If Request.QueryString("ContainerId").ToString = "CustInvAffliasiID" Then
                                Session("SelectCommand") = "EXEC S_FNFormCustInvAff '''" + GVR.Cells(2).Text + "'''"
                                Session("ReportFile") = ".../../../Rpt/FormCustInvAff.frx"
                            End If

                            AttachScript("openprintdlg();", Page, Me.GetType)
                        Else
                            lbStatus.Text = MessageDlg("Data must be Posted to Print") '"Cannot print or preview, status Invoice No " + QuotedStr(GVR.Cells(2).Text) + " must be posted or get approval"
                            Exit Sub
                        End If
                    Catch ex As Exception
                        lbStatus.Text = "btn print Error = " + ex.ToString
                    End Try
                ElseIf DDL.SelectedValue = "Print Tax" Then
                    Try
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        'If (GVR.Cells(3).Text = "P") Or (GVR.Cells(3).Text = "G") Or (GVR.Cells(3).Text = "H") Then
                        If (GVR.Cells(3).Text = "P") Then
                            Session("DBConnection") = ViewState("DBConnection")
                            Session("SelectCommand") = "EXEC S_FNFormCustInvFPS '''" + GVR.Cells(2).Text + "'''"
                            Session("ReportFile") = ".../../../Rpt/FormCustInvFaktur.frx"

                            AttachScript("openprintdlg();", Page, Me.GetType)
                        Else
                            lbStatus.Text = MessageDlg("Data must be Posted to Print") '"Cannot print or preview, status Invoice No " + QuotedStr(GVR.Cells(2).Text) + " must be posted or get approval"
                            Exit Sub
                        End If
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
                ' btnAddDt_Click(Nothing, Nothing)
                'btnGetDt.Enabled = False
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Dim dr As DataRow()
        Dim r As DataRow
        Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
        dr = ViewState("Dt").Select("SJNo = " + QuotedStr(GVR.Cells(1).Text))
        For Each r In dr
            r.Delete()
        Next
        BindGridDt(ViewState("Dt"), GridDt)
        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
        totalingDt()
        If GetCountRecord(ViewState("Dt")) = 0 Then
            'tbPONo.Text = ""
        End If
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            ViewState("DtValue") = GVR.Cells(1).Text + "|" + GVR.Cells(3).Text
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            ViewState("ProductDt") = GVR.Cells(3).Text
            FillTextBoxDt(ViewState("DtValue"))
            btnSaveDt.Focus()
            StatusButtonSave(False)
            AttachScript("setformatdt('');", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbCurr_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCurr.Click
        Try
            ViewState("InputCurrency") = "Y"
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsCurrency')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb Currency Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbCust_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCust.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsCustomer')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb Customer Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub lbTerm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbTerm.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsTerm')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb Term Error : " + ex.ToString
        End Try
    End Sub

    Dim BaseForex As Decimal = 0
    Dim DiscForex As Decimal = 0
    ' untuk tampilkan data total di grid
    Protected Sub GridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt.RowDataBound
        Try
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "Product")) Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                    '' add the UnitPrice and QuantityTotal to the running total variables
                    'BaseForex += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "AmountForex"))
                    'DiscForex += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DiscForex"))
                ElseIf e.Row.RowType = DataControlRowType.Footer Then
                    'If ddlCurr.SelectedValue = Session("Currency") Then
                    '    BaseForex = Math.Floor(BaseForex)
                    '    DiscForex = Math.Floor(DiscForex)
                    'End If
                    'BaseForex = GetTotalSum(ViewState("Dt"), "AmountForex")
                    'DiscForex = GetTotalSum(ViewState("Dt"), "DiscForex")
                    'If ddlfgInclude.SelectedValue = "N" Then
                    '    tbBaseForex.Text = FormatFloat(BaseForex, ViewState("DigitCurr"))
                    '    tbDiscForex.Text = FormatFloat(DiscForex, ViewState("DigitCurr"))
                    '    tbPPNForex.Text = FormatFloat((CFloat(tbBaseForex.Text) - CFloat(tbDiscForex.Text) - CFloat(tbDPForex.Text)) * CFloat(tbPPN.Text) / 100.0, ViewState("DigitCurr"))
                    'Else
                    '    '(Total Netto Forex * 100 / (100+PPn)) + Disc Forex
                    '    tbBaseForex.Text = FormatFloat(((BaseForex - DiscForex) * 100 / (100 + CFloat(tbPPN.Text))) + DiscForex, CInt(ViewState("DigitCurr")))
                    '    tbDiscForex.Text = FormatFloat(DiscForex, ViewState("DigitCurr"))
                    '    tbPPNForex.Text = FormatFloat(((BaseForex - DiscForex - CFloat(tbDPForex.Text)) * CFloat(tbPPN.Text) / (100.0 + CFloat(tbPPN.Text))), CInt(ViewState("DigitCurr")))
                    'End If                    
                    'tbTotalForex.Text = FormatFloat(CFloat(tbBaseForex.Text) - CFloat(tbDiscForex.Text) - CFloat(tbDPForex.Text), ViewState("DigitCurr"))
                    'tbTotalInvoice.Text = FormatFloat(CFloat(tbTotalForex.Text) + CFloat(tbPPNForex.Text), ViewState("DigitCurr"))
                    'AttachScript("setformat();", Page, Me.GetType())
                    totalingDt()
                End If
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            ViewState("Hd") = Nothing
            Dt = BindDataTransaction(GetStringHd(lblJudul.Text.Trim), "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            ViewState("Hd") = Dt
            tbCode.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            'BindToDropList(ddlReport, Dt.Rows(0)("FgReport").ToString)
            BindToDropList(ddlfgInclude, Dt.Rows(0)("FgPriceIncludeTax").ToString)
            BindToText(tbCustCode, Dt.Rows(0)("Customer").ToString)
            BindToText(tbCustName, Dt.Rows(0)("Customer_Name").ToString)
            BindToText(tbBillToCode, Dt.Rows(0)("BillTo").ToString)
            BindToText(tbBillToName, Dt.Rows(0)("Bill_To_Name").ToString)
            BindToText(tbAttn, Dt.Rows(0)("Attn").ToString)
            BindToDropList(ddlTerm, Dt.Rows(0)("Term").ToString)
            BindToDate(tbDueDate, Dt.Rows(0)("Duedate").ToString)
            BindToText(tbPPnNo, Dt.Rows(0)("PPnNo").ToString)
            BindToDate(tbPPndate, Dt.Rows(0)("PPndate").ToString)
            BindToDropList(ddlCurr, Dt.Rows(0)("Currency").ToString)
            ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, ViewState("DBConnection").ToString)
            'BindToText(tbPpnRate, Dt.Rows(0)("PPnRate").ToString, ViewState("DigitRate"))
            BindToText(tbPpnRate, Dt.Rows(0)("PPnRate").ToString, ViewState("DigitCurr"))
            'BindToText(tbRate, Dt.Rows(0)("ForexRate").ToString, ViewState("DigitRate"))
            BindToText(tbRate, Dt.Rows(0)("ForexRate").ToString, ViewState("DigitCurr"))
            BindToText(tbBaseForex, Dt.Rows(0)("BaseForex").ToString, ViewState("DigitCurr"))
            'BindToDropList(ddlBankReceipt, Dt.Rows(0)("BankReceipt").ToString)
            'BindToText(tbDiscForex, Dt.Rows(0)("DiscForex").ToString)
            BindToText(tbPPN, Dt.Rows(0)("PPn").ToString, ViewState("DigitPercent"))
            BindToText(tbPPNForex, Dt.Rows(0)("PPnForex").ToString, ViewState("DigitCurr"))
            BindToText(tbTotalForex, Dt.Rows(0)("TotalForex").ToString, ViewState("DigitCurr"))
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
            BindToText(tbCustTaxAddress, Dt.Rows(0)("CustTaxAddress").ToString)
            BindToText(tbCustTaxNPWP, Dt.Rows(0)("CustTaxNPWP").ToString)

        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal Product As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("SJNo+'|'+Product = " + QuotedStr(Product))
            If Dr.Length > 0 Then
                BindToText(tbSJNo, Dr(0)("SJNo").ToString)
                BindToText(tbSONo, Dr(0)("SONo").ToString)
                BindToText(tbProductCode, Dr(0)("Product").ToString)
                BindToText(tbProductName, Dr(0)("Product_Name").ToString)
                BindToText(tbQty, Dr(0)("Qty").ToString)
                BindToText(tbQtyM2, Dr(0)("QtyM2").ToString)
                BindToText(tbQtyRoll, Dr(0)("QtyRoll").ToString)
                BindToText(tbUnit, Dr(0)("Unit").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
                BindToText(tbPriceForex, CFloat(Dr(0)("PriceForex").ToString), ViewState("DigitCurr"))
                BindToText(tbAmountForex, Dr(0)("AmountForex").ToString)
                BindToText(tbDisc, Dr(0)("Disc").ToString)
                BindToText(tbDiscForexDt, Dr(0)("DiscForex").ToString)
                BindToText(tbTotalForexDt, Dr(0)("NettoForex").ToString)
                BindToText(tbcommision, Dr(0)("CommissionForex").ToString)
                BindToDropList(ddlUnitCommision, Dr(0)("CommissionUnit").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDP(ByVal DPNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt2").select("DPNo = " + QuotedStr(DPNo))
            If Dr.Length > 0 Then
                BindToText(tbDPNo, Dr(0)("DPNo").ToString)
                BindToText(tbDPCurrency, Dr(0)("Currency").ToString)
                ViewState("DigitCurrDt2") = SQLExecuteScalar("Select Digit FROM VMsCurrency WHERE Currency = " + QuotedStr(tbDPCurrency.Text), ViewState("DBConnection"))
                BindToText(tbDPRate, Dr(0)("ForexRate").ToString)
                BindToText(tbDPPPnRate, Dr(0)("PPnRate").ToString)
                BindToText(tbDPPPnPercent, Dr(0)("PPn").ToString)
                BindToText(tbDPBase, Dr(0)("DPBase").ToString)
                BindToText(tbDPPPn, Dr(0)("DPPPn").ToString)
                BindToText(tbDPTotal, Dr(0)("DPTotal").ToString)
                BindToText(tbPaidBase, Dr(0)("PaidBase").ToString)
                BindToText(tbPaidPPN, Dr(0)("PaidPPn").ToString)
                BindToText(tbPaidTotal, Dr(0)("PaidTotal").ToString)
                BindToText(tbDPBaseForex, Dr(0)("BaseForex").ToString)
                BindToText(tbDPPPnForex, Dr(0)("PPnForex").ToString)
                BindToText(tbDPTotalForex, Dr(0)("TotalForex").ToString)
                BindToText(tbDPDPInvoice, Dr(0)("DPInvoice").ToString)
                BindToText(tbDPRemarkDt, Dr(0)("Remark").ToString)
                ViewState("DtDPNo") = tbDPNo.Text
            End If
            'Dt = BindDataTransaction(GetStringDt(tbRef.Text), "ItemNo = " + ItemNo, Session("DBConnection").ToString)
            'newTrans()
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub

    Private Sub StatusButtonSave(ByVal Bool As Boolean)
        btnSaveAll.Visible = Bool
        btnSaveTrans.Visible = Bool
        btnBack.Visible = Bool
        BtnGetData.Visible = Bool
    End Sub

    'Protected Sub ddlReport_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlReport.SelectedIndexChanged
    '    ChangeReport("Edit", ddlReport.SelectedValue, ddlCurr.SelectedValue = ViewState("Currency"), tbDate, tbRate, tbPPnNo, tbPPndate, tbPpnRate)
    '    'FillCombo(ddlBankReceipt, "EXEC S_GetBankReceipt " + QuotedStr(ddlReport.SelectedValue), False, "Bank_Code", "Bank_Name", ViewState("DBConnection"))
    'End Sub

    Protected Sub btnGetData_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnGetData.Click
        Dim ResultField, CriteriaField, DefaultField, ResultSame, Filter As String
        Try
            Filter = ""
            If tbCustCode.Text.Trim <> "" Then
                Filter = Filter + " AND Customer_Code = " + QuotedStr(tbCustCode.Text)
            End If
            Session("Filter") = "EXEC S_FNCustInvGetSJ " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Filter)
            ResultField = "SJ_No, SJ_Date, SO_No, CustPONo, Customer_Code, Customer_Name, DeliveryCode, DeliveryName, Report, Currency, Term, PriceIncludeTax, SJ_Remark, Product_Code, Product_Name, Specification, Qty, QtyM2, QtyRoll, Unit, PriceInUnit, PriceForex, AmountForex, Disc, DiscForex, NettoForex"
            DefaultField = "SJ_No"
            CriteriaField = "SJ_No, SJ_Date, SO_No, CustPONo, Customer_Code, Customer_Name, DeliveryCode, DeliveryName, Report, Currency, Term, PriceIncludeTax, SJ_Remark, Product_Code, Product_Name, Specification, Qty, QtyM2, QtyRoll, Unit, PriceInUnit, PriceForex, AmountForex, Disc, DiscForex, NettoForex"
            Session("ClickSame") = "SJ_No"
            ViewState("Sender") = "btnGetDt"
            Session("ColumnDefault") = DefaultField.Split(",")
            Session("Column") = ResultField.Split(",")
            Session("CriteriaField") = CriteriaField.Split(",")
            ResultSame = "SO_No"
            Session("ResultSame") = ResultSame.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            'AttachScript("OpenSearchMultiDlg2();", Page, Me.GetType())
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
            'btnGetData.Visible = False
        Catch ex As Exception
            lbStatus.Text = "btn get Dt Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub lbCount_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCount.Click
        Dim ResultField, CriteriaField, DefaultField, ResultSame, Filter As String
        Try
            Filter = ""
            If tbCustCode.Text.Trim <> "" Then
                Filter = Filter + " AND Customer_Code = " + QuotedStr(tbCustCode.Text)
            End If
            Session("Filter") = "EXEC S_FNCustInvGetSJ " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Filter)
            ResultField = "SJ_No, SJ_Date, SO_No, CustPONo, Customer_Code, Customer_Name, DeliveryCode, DeliveryName, Report, Currency, Term, PriceIncludeTax, SJ_Remark, Product_Code, Product_Name, Specification, Qty, QtyM2, QtyRoll, Unit, PriceInUnit, PriceForex, AmountForex, Disc, DiscForex, NettoForex"
            DefaultField = "SJ_No"
            CriteriaField = "SJ_No, SJ_Date, SO_No, CustPONo, Customer_Code, Customer_Name, DeliveryCode, DeliveryName, Report, Currency, Term, PriceIncludeTax, SJ_Remark, Product_Code, Product_Name, Specification, Qty, QtyM2, QtyRoll, Unit, PriceInUnit, PriceForex, AmountForex, Disc, DiscForex, NettoForex"
            Session("ClickSame") = "SJ_No"
            ViewState("Sender") = "btnOut"
            Session("ColumnDefault") = DefaultField.Split(",")
            Session("Column") = ResultField.Split(",")
            Session("CriteriaField") = CriteriaField.Split(",")
            ResultSame = "SO_No"
            Session("ResultSame") = ResultSame.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            'AttachScript("OpenSearchMultiDlg2();", Page, Me.GetType())
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
            'btnGetData.Visible = False
        Catch ex As Exception
            lbStatus.Text = "btn get Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlTerm_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTerm.SelectedIndexChanged
        tbDueDate.SelectedDate = FindDueDate(ddlTerm.SelectedValue, tbDate.SelectedDate, ViewState("DBConnection").ToString)
    End Sub

    Protected Sub tbDate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbDate.SelectionChanged
        tbDueDate.SelectedDate = FindDueDate(ddlTerm.SelectedValue, tbDate.SelectedDate, ViewState("DBConnection").ToString)
    End Sub

    Protected Sub btnBillTo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBillTo.Click
        Dim ResultField As String
        Try
            Session("filter") = "select Bill_To, Bill_To_Name, Attn from VMsCustBillTo WHere CustCode = " + QuotedStr(tbCustCode.Text)
            ResultField = "Bill_To, Bill_To_Name, Attn"
            ViewState("Sender") = "btnBillTo"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn BillTo Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbBillToCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbBillToCode.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("CustBillTo", tbCustCode.Text + "|" + tbBillToCode.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbBillToCode.Text = Dr("Bill_To")
                tbBillToName.Text = Dr("Bill_To_Name")
                BindToText(tbAttn, Dr("Attn"))
            Else
                tbBillToCode.Text = ""
                tbBillToName.Text = ""
                tbAttn.Text = ""
            End If
            tbBillToCode.Focus()
        Catch ex As Exception
            Throw New Exception("tb BillToCode Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSJNo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSJNo.Click
        Dim ResultField As String
        Try

            Session("filter") = " EXEC S_FNCustInvGetDt ''"
            ResultField = "SJ_No, SO_No, Product_Code, Product_Name, Specification, Qty, QtyM2, QtyRoll, Unit"
            ViewState("Sender") = "btnSJNo"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn SJNo Error : " + ex.ToString
        End Try
    End Sub
    'Protected Sub ddlDisc_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDisc.SelectedIndexChanged
    '    Dim dr As DataRow
    '    Try
    '        tbDiscUnit1.Enabled = ddlDisc.SelectedValue <> ""
    '        If ddlDisc.SelectedValue = "" Then
    '            tbDiscUnit1.Text = "0"
    '            tbDiscForex1.Text = "0"
    '        Else
    '            dr = SQLExecuteQuery("Select * FROM VMsDiscType WHERE Disc_Type_Code = " + QuotedStr(ddlDisc.SelectedValue)).Tables(0).Rows(0)
    '            If Not dr Is Nothing Then
    '                ddlDiscUnit1.SelectedValue = dr("FgDiscUnit").ToString
    '                ddlFgOperator1.SelectedValue = dr("FgValue").ToString
    '            End If
    '        End If
    '        AttachScript("setformatdt();", Page, Me.GetType)
    '        btnSaveDt.Focus()
    '    Catch ex As Exception
    '        lbStatus.Text = "ddl disc type 1 changed Error : " + ex.ToString
    '    End Try
    'End Sub

    Private Function cekValue(ByVal val As String) As String
        If val.Trim = "" Then
            Return "0"
        Else
            Return val
        End If
    End Function


    'Protected Sub btnhapusdt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhapusdt.Click
    '    Dim dr() As DataRow
    '    Dim count As Integer
    '    dr = ViewState("Dt").Select("Product <> ''")
    '    count = dr.Count - 1
    '    For i As Integer = 0 To count
    '        dr(i).Delete()
    '    Next
    '    BindGridDt(ViewState("Dt"), GridDt)
    '    EnableHd(True)
    '    tbTotalForex.Text = FormatNumber(0, ViewState("DigitCurr"))    
    '    'AttachScript("BaseDiscPPnOtherTotal(" + Me.tbBaseForex.ClientID + "," + tbDisc.ClientID + "," + tbDiscForex.ClientID + "," + Me.tbPPN.ClientID + "," + Me.tbPPNForex.ClientID + "," + tbOtherForex.ClientID + "," + Me.tbTotalForex.ClientID + "); setformat();", Page, Me.GetType())
    '    AttachScript("setformat();", Page, Me.GetType())
    'End Sub

    Protected Sub tbDisc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbDisc.TextChanged
        Try
            tbDiscForexDt.Text = FormatFloat((CFloat(tbDisc.Text) / 100) * CFloat(tbAmountForex.Text), ViewState("DigitCurr"))
            tbTotalForexDt.Text = FormatFloat((CFloat(tbQty.Text) * CFloat(tbPriceForex.Text)) - CFloat(tbDiscForexDt.Text), ViewState("DigitCurr"))

        Catch ex As Exception
            Throw New Exception("tbDisc_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbPriceForex_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPriceForex.TextChanged
        Try
            If ddlUnitWrhsDt.SelectedValue = "M2" Then
                tbAmountForex.Text = FormatFloat(CFloat(tbQtyM2.Text) * CFloat(tbPriceForex.Text), ViewState("DigitCurr"))
            ElseIf ddlUnitWrhsDt.SelectedValue = "Roll" Then
                tbAmountForex.Text = FormatFloat(CFloat(tbQtyRoll.Text) * CFloat(tbPriceForex.Text), ViewState("DigitCurr"))
            Else
                tbAmountForex.Text = FormatFloat(CFloat(tbQty.Text) * CFloat(tbPriceForex.Text), ViewState("DigitCurr"))
            End If
            tbDiscForexDt.Text = FormatFloat(CFloat(tbDisc.Text) * CFloat(tbAmountForex.Text) / 100, ViewState("DigitCurr"))
            tbTotalForexDt.Text = FormatFloat(CFloat(tbAmountForex.Text) - CFloat(tbDiscForexDt.Text), ViewState("DigitCurr"))
            'tbPriceForex.Text = FormatFloat(tbPriceForex.Text)
            'AttachScript("setformatdt();", Me.Page, Me.GetType())
            tbDisc.Focus()
        Catch ex As Exception
            Throw New Exception("tbPriceForexDt_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnDPNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDPNo.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "select DP_No,dbo.FormatDate(DP_Date) AS DP_Date,Customer,Customer_Name,Attn, DPListNo, SONo, Currency, dbo.FormatFloat(ForexRate,dbo.DigitCurrRate()) AS ForexRate, dbo.FormatFloat(PPnRate,dbo.DigitCurrRate()) AS PPnRate, " + _
                "dbo.FormatFloat(BaseForex, dbo.DigitCurrForex(Currency)) AS BaseForex, dbo.FormatFloat(PPN,dbo.DigitPercent()) AS PPn, " + _
                "dbo.FormatFloat(PPnForex, dbo.DigitCurrForex(Currency)) AS PPnForex, dbo.FormatFloat(TotalForex, dbo.DigitCurrForex(Currency)) AS TotalForex," + _
                "dbo.FormatFloat(BasePaid, dbo.DigitCurrForex(Currency)) AS BasePaid, dbo.FormatFloat(PPnPaid, dbo.DigitCurrForex(Currency)) AS PPnPaid, dbo.FormatFloat(BasePaid+PPnPaid, dbo.DigitCurrForex(Currency)) AS TotalPaid " + _
                "from V_FNDPCustForCI WHERE Customer = " + QuotedStr(tbCustCode.Text) + " AND PPn = " + (CInt(tbPPN.Text)).ToString
            ResultField = "DP_No, Currency, ForexRate, PPnRate, PPn, BaseForex, PPNForex, TotalForex, BasePaid, PPNPaid, TotalPaid "
            ViewState("Sender") = "btnDPNo"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search DP Error : " + ex.ToString
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

    Protected Sub btnAddDP2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDP2.Click, btnAddDPKe2.Click
        Try
            ClearDP()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt2") = "Insert"
            MovePanel(pnlDt2, pnlEditDt2)
            EnableHd(False)
            StatusButtonSave(False)
            tbDPNo.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add DP error : " + ex.ToString
        End Try
    End Sub
    Private Sub ClearDP()
        Try
            tbDPNo.Text = ""
            tbDPCurrency.Text = ViewState("Currency")
            tbDPRate.Text = FormatFloat("1", ViewState("DigitRate"))
            tbDPPPnRate.Text = FormatFloat("1", ViewState("DigitRate"))
            tbDPPPnPercent.Text = FormatFloat("0", ViewState("DigitPercent"))
            tbDPBase.Text = FormatFloat("0", ViewState("DigitCurrDt2"))
            tbDPPPn.Text = FormatFloat("0", ViewState("DigitCurrDt2"))
            tbDPTotal.Text = FormatFloat("0", ViewState("DigitCurrDt2"))
            tbPaidBase.Text = FormatFloat("0", ViewState("DigitCurrDt2"))
            tbPaidPPN.Text = FormatFloat("0", ViewState("DigitCurrDt2"))
            tbPaidTotal.Text = FormatFloat("0", ViewState("DigitCurrDt2"))
            tbDPBaseForex.Text = FormatFloat("0", ViewState("DigitCurrDt2"))
            tbDPPPnForex.Text = FormatFloat("0", ViewState("DigitCurrDt2"))
            tbDPTotalForex.Text = FormatFloat("0", ViewState("DigitCurrDt2"))
            tbDPDPInvoice.Text = FormatFloat("0", ViewState("DigitCurrDt2"))
            tbDPRemarkDt.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt 2 Error " + ex.ToString)
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
    Dim TotalDP As Decimal = 0
    Dim TotalAmount As Decimal = 0
    Dim TotalDisc As Decimal = 0
    Protected Sub GridDt2_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt2.RowDataBound
        Try
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "DPNo")) Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                    ' add the UnitPrice and QuantityTotal to the running total variables
                    'TotalDP += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DPInvoice"))
                ElseIf e.Row.RowType = DataControlRowType.Footer Then
                    'TotalDP = GetTotalSum(ViewState("Dt2"), "DPInvoice")
                    'TotalAmount = GetTotalSum(ViewState("Dt"), "AmountForex")
                    'TotalDisc = GetTotalSum(ViewState("Dt"), "DiscForex")
                    'tbDPForex.Text = FormatFloat(TotalDP, ViewState("DigitCurr"))
                    'tbTotalForex.Text = FormatFloat(CFloat(tbBaseForex.Text) - CFloat(tbDiscForex.Text) - CFloat(tbDPForex.Text), CInt(ViewState("DigitCurr")))
                    'If ddlfgInclude.SelectedValue = "N" Then
                    '    tbPPNForex.Text = FormatFloat((CFloat(tbBaseForex.Text) - CFloat(tbDiscForex.Text) - CFloat(tbDPForex.Text)) * (CFloat(tbPPN.Text) / 100.0), CInt(ViewState("DigitCurr")))
                    'Else
                    '    tbPPNForex.Text = FormatFloat((TotalAmount - TotalDisc - (TotalDP * (100.0 + CFloat(tbPPN.Text)) / 100.0)) * CFloat(tbPPN.Text) / (100.0 + CFloat(tbPPN.Text)), CInt(ViewState("DigitCurr")))
                    'End If
                    'tbTotalInvoice.Text = FormatFloat(CFloat(tbTotalForex.Text) + CFloat(tbPPNForex.Text), ViewState("DigitCurr"))
                    totalingDt()
                End If
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbDPBaseForex_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbDPBaseForex.TextChanged
        Try
            tbDPTotalForex.Text = FormatFloat(CFloat(tbDPBaseForex.Text) + CFloat(tbDPPPnForex.Text), ViewState("DigitCurrDt2"))
            If tbDPCurrency.Text <> ddlCurr.SelectedValue Then
                tbDPDPInvoice.Text = FormatFloat((CFloat(tbDPBaseForex.Text) * CFloat(tbDPRate.Text)) / CFloat(tbRate.Text), ViewState("DigitCurrDt2"))
            Else
                tbDPDPInvoice.Text = tbDPBaseForex.Text
            End If
        Catch ex As Exception
            lbStatus.Text = "tbRate_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveDt2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt2.Click
        Try
            'If CekExistData(ViewState("Dt2"), "DPNo", tbDPNo.Text.Trim) Then
            If CekDt2() = False Then
                Exit Sub
            End If
            If ViewState("StateDt2") = "Edit" Then
                If ViewState("DtDPNo") <> tbDPNo.Text Then
                    If CekExistData(ViewState("Dt2"), "DPNo", tbDPNo.Text) Then
                        lbStatus.Text = "DP No " + tbDPNo.Text + " has been already exist"
                        Exit Sub
                    End If
                End If
                Dim Row As DataRow
                Row = ViewState("Dt2").Select("DPNo = " + QuotedStr(tbDPNo.Text))(0)
                Row.BeginEdit()
                Row("Currency") = tbDPCurrency.Text
                Row("ForexRate") = FormatFloat(tbDPRate.Text, ViewState("DigitRate"))
                Row("PPnRate") = FormatFloat(tbDPPPnRate.Text, ViewState("DigitRate"))
                Row("PPn") = FormatFloat(tbDPPPnPercent.Text, ViewState("DigitPercent"))
                Row("DPBase") = FormatFloat(tbDPBase.Text, ViewState("DigitCurrDt2"))
                Row("DPPPN") = FormatFloat(tbDPPPn.Text, ViewState("DigitCurrDt2"))
                Row("DPTotal") = FormatFloat(tbDPTotal.Text, ViewState("DigitCurrDt2"))
                Row("PaidBase") = FormatFloat(tbPaidBase.Text, ViewState("DigitCurrDt2"))
                Row("PaidPPN") = FormatFloat(tbPaidPPN.Text, ViewState("DigitCurrDt2"))
                Row("PaidTotal") = FormatFloat(tbPaidTotal.Text, ViewState("DigitCurrDt2"))
                Row("BaseForex") = FormatFloat(tbDPBaseForex.Text, ViewState("DigitCurrDt2"))
                Row("PPNForex") = FormatFloat(tbDPPPnForex.Text, ViewState("DigitCurrDt2"))
                Row("TotalForex") = FormatFloat(tbDPTotalForex.Text, ViewState("DigitCurrDt2"))
                If tbDPCurrency.Text <> ddlCurr.SelectedValue Then
                    tbDPDPInvoice.Text = FormatFloat((CFloat(tbDPBaseForex.Text) * CFloat(tbDPRate.Text)) / CFloat(tbRate.Text), ViewState("DigitCurrDt2"))
                Else
                    tbDPDPInvoice.Text = tbDPBaseForex.Text
                End If
                Row("DPInvoice") = FormatFloat(tbDPDPInvoice.Text, ViewState("DigitCurrDt2"))
                Row("Remark") = tbDPRemarkDt.Text
                Row.EndEdit()
            Else
                'Insert
                If CekExistData(ViewState("Dt2"), "DPNo", tbDPNo.Text) Then
                    lbStatus.Text = "DP No " + tbDPNo.Text + " has been already exist"
                    Exit Sub
                End If

                Dim dr As DataRow
                dr = ViewState("Dt2").NewRow
                dr("DPNo") = tbDPNo.Text
                dr("Currency") = tbDPCurrency.Text
                dr("ForexRate") = tbDPRate.Text
                dr("PPnRate") = tbDPPPnRate.Text
                dr("PPn") = FormatFloat(tbDPPPnPercent.Text, ViewState("DigitPercent"))
                dr("DPBase") = FormatFloat(tbDPBase.Text, ViewState("DigitCurrDt2"))
                dr("DPPPN") = FormatFloat(tbDPPPn.Text, ViewState("DigitCurrDt2"))
                dr("DPTotal") = FormatFloat(tbDPTotal.Text, ViewState("DigitCurrDt2"))
                dr("PaidBase") = FormatFloat(tbPaidBase.Text, ViewState("DigitCurrDt2"))
                dr("PaidPPN") = FormatFloat(tbPaidPPN.Text, ViewState("DigitCurrDt2"))
                dr("PaidTotal") = FormatFloat(tbPaidTotal.Text, ViewState("DigitCurrDt2"))
                dr("BaseForex") = FormatFloat(tbDPBaseForex.Text, ViewState("DigitCurrDt2"))
                dr("PPNForex") = FormatFloat(tbDPPPnForex.Text, ViewState("DigitCurrDt2"))
                dr("TotalForex") = FormatFloat(tbDPTotalForex.Text, ViewState("DigitCurrDt2"))
                If tbDPCurrency.Text <> ddlCurr.SelectedValue Then
                    tbDPDPInvoice.Text = FormatFloat((CFloat(tbDPBaseForex.Text) * CFloat(tbDPRate.Text)) / CFloat(tbRate.Text), ViewState("DigitCurrDt2"))
                Else
                    tbDPDPInvoice.Text = tbDPBaseForex.Text
                End If
                dr("DPInvoice") = FormatFloat(tbDPDPInvoice.Text, ViewState("DigitCurrDt2"))
                dr("Remark") = tbDPRemarkDt.Text
                ViewState("Dt2").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt2, pnlDt2)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
            totalingDt()
            BindGridDt(ViewState("Dt2"), GridDt2)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save dt2 Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub tbDPCurrency_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbDPCurrency.TextChanged
        Try
            If tbDPCurrency.Text = ViewState("Currency") Then
                tbDPRate.Enabled = False
            End If
            If tbDPCurrency.Text = ddlCurr.SelectedValue Then
                tbDPRate.Enabled = False
            End If
            If tbDPCurrency.Text <> ddlCurr.SelectedValue Then
                tbDPRate.Enabled = True
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub GridDt2_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt2.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow = GridDt2.Rows(e.RowIndex)
            dr = ViewState("Dt2").Select("DPNo = " + QuotedStr(GVR.Cells(1).Text))
            dr(0).Delete()
            BindGridDt(ViewState("Dt2"), GridDt2)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
            totalingDt()
        Catch ex As Exception
            lbStatus.Text = "Grid Dt 2 Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt2_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt2.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt2.Rows(e.NewEditIndex)
            FillTextBoxDP(GVR.Cells(1).Text)            
            EnableHd(False)
            ViewState("StateDt2") = "Edit"
            StatusButtonSave(False)
            MovePanel(pnlDt2, pnlEditDt2)
            btnSaveDt2.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid dt2 Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbRate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbRate.TextChanged
        Try
            If tbDPCurrency.Text <> ddlCurr.SelectedValue Then
                tbDPDPInvoice.Text = FormatFloat((CFloat(tbDPBaseForex.Text) * CFloat(tbDPRate.Text)) / CFloat(tbRate.Text), ViewState("DigitCurrDt2"))
            Else
                tbDPDPInvoice.Text = tbDPBaseForex.Text
            End If
        Catch ex As Exception
            lbStatus.Text = "tbRate_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbQty_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbQty.TextChanged
        Try
            If ddlUnitWrhsDt.SelectedValue = "M2" Then
                tbAmountForex.Text = FormatFloat(CFloat(tbQtyM2.Text) * CFloat(tbPriceForex.Text), ViewState("DigitCurr"))
            ElseIf ddlUnitWrhsDt.SelectedValue = "Roll" Then
                tbAmountForex.Text = FormatFloat(CFloat(tbQtyRoll.Text) * CFloat(tbPriceForex.Text), ViewState("DigitCurr"))
            Else
                tbAmountForex.Text = FormatFloat(CFloat(tbQty.Text) * CFloat(tbPriceForex.Text), ViewState("DigitCurr"))
            End If
            tbDiscForexDt.Text = FormatFloat(CFloat(tbDisc.Text) * CFloat(tbAmountForex.Text) / 100, ViewState("DigitCurr"))
            tbTotalForexDt.Text = FormatFloat(CFloat(tbAmountForex.Text) - CFloat(tbDiscForexDt.Text), ViewState("DigitCurr"))
            'tbPriceForex.Text = FormatFloat(tbPriceForex.Text, ViewState("DigitCurr"))
            'AttachScript("setformatdt();", Me.Page, Me.GetType())
            tbPriceForex.Focus()
        Catch ex As Exception
            lbStatus.Text = "tbQty_TextChanged Error : " + ex.ToString
        End Try
        
    End Sub

    Protected Sub tbDPDPInvoice_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbDPDPInvoice.TextChanged
        Try
            If tbDPCurrency.Text = ddlCurr.SelectedValue Then
                tbDPBaseForex.Text = tbDPDPInvoice.Text
            End If
        Catch ex As Exception
            lbStatus.Text = "tbDPDPInvoice_TextChanged Error : " + ex.ToString
        End Try
        
    End Sub

    Protected Sub tbPPndate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPPndate.SelectionChanged
        Try
            tbPpnRate.Text = FormatFloat(FindTaxRate(ddlCurr.SelectedValue, tbPPndate.SelectedValue, ViewState("DBConnection").ToString), ViewState("DigitRate"))
        Catch ex As Exception
            lbStatus.Text = "tbPPndate_SelectionChanged Error : " + ex.ToString
        End Try
    End Sub

    
    Protected Sub btnCustTax_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCustTax.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT CustCode, CustTaxAddress, CustTaxNPWP FROM V_MsCustTaxAddress WHERE CustCode = " + QuotedStr(tbCustCode.Text)
            ResultField = "CustTaxAddress, CustTaxNPWP"
            ViewState("Sender") = "btnCustTax"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn CustTax Error : " + ex.ToString
        End Try
    End Sub

    
    Protected Sub tbDiscForexDt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbDiscForexDt.TextChanged
        Try
            ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, ViewState("DBConnection").ToString)
            tbDisc.Text = FormatFloat((CFloat(tbDiscForexDt.Text) / CFloat(tbAmountForex.Text)) * 100, 4)
            tbTotalForexDt.Text = FormatFloat((CFloat(tbQty.Text) * CFloat(tbPriceForex.Text)) - CFloat(tbDiscForexDt.Text), ViewState("DigitCurr"))
        Catch ex As Exception
            Throw New Exception("tbDisc_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbEditPrice_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbEditPrice.TextChanged
        Dim Row As DataRow
        Dim GVR As GridViewRow
        Try

            For Each GVR In GridDt.Rows
                GVR.Cells(7).Text = tbEditPrice.Text
                Row = ViewState("Dt").Select("SJNo+'|'+SoNo+'|'+Product = " + QuotedStr(GVR.Cells(1).Text + "|" + GVR.Cells(2).Text + "|" + GVR.Cells(3).Text))(0)
                Row.BeginEdit()

                Row("PriceForex") = tbEditPrice.Text
                'Row("Qty") = GVR.Cells(5).Text
                Row("AmountForex") = FormatNumber((GVR.Cells(5).Text) * (GVR.Cells(7).Text), 0)
                Row("Disc") = GVR.Cells(11).Text
                Row("DiscForex") = FormatNumber((Row("AmountForex").ToString * Row("Disc").ToString) / 100, 0)
                Row("NettoForex") = FormatNumber(Row("AmountForex").ToString - Row("DiscForex").ToString, 0)
                Row.EndEdit()

                'lbStatus.Text = GVR.Cells(11).Text
                'Exit Sub
                'btnSaveDt_Click(Nothing, Nothing)

            Next
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            totalingDt()
            BindGridDt(ViewState("Dt"), GridDt)
            StatusButtonSave(True)
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            tbEditPrice.Text = ""
        Catch ex As Exception
            lbStatus.Text = "tbDPDPInvoice_TextChanged Error : " + ex.ToString
        End Try

    End Sub

End Class