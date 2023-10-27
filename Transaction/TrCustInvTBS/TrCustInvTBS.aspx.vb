Imports System.Data
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO


Partial Class CustInvTBS
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    'Protected GetStringHd As String = "Select * From V_FNCustInvTBSHd"
    Private Function GetStringHd(ByVal Type As String) As String
        Return "Select distinct TransNmbr, TransDate, Status, FgReport, CustCode, CustName, Attn, Term, TermName, DueDate, SPBNO, SPBManual, BankReceipt, BankReceiptName, PPnNo, PPnDate, PPnRate, CurrCode, ForexRate, BaseForex, PPn, PPnForex, TotalForex, Remark, UserPrep, DatePrep, UserAppr, DateAppr From V_FNCustInvTBSHd "
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()

                FillCombo(ddlTerm, "EXEC S_GetTerm", False, "Term_Code", "Term_Name", ViewState("DBConnection"))
                FillCombo(ddlCurr, "EXEC S_GetCurrency", True, "Currency", "Currency", ViewState("DBConnection"))
                FillCombo(ddlReceipt, "EXEC S_GetPayType", True, "PayCode", "PayName", ViewState("DBConnection"))

                SetInit()
                Session("AdvanceFilter") = ""
            End If
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
                    BindToText(tbAttn, TrimStr(Session("Result")(4).ToString))
                    ddlCurr_SelectedIndexChanged(Nothing, Nothing)
                    ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection").ToString)
                    If Session("Result")(7).ToString = "Y" Then
                        tbPPN.Text = "10"
                    Else
                        tbPPN.Text = "0"
                    End If
                    'Dim dr As DataRow
                    'dr = SQLExecuteQuery("EXEC S_FNCustInvGetCust " + QuotedStr(tbCustCode.Text), ViewState("DBConnection").ToString).Tables(0).Rows(0)
                    'If Not dr Is Nothing Then
                    '    BindToText(tbCustTaxAddress, dr("CustTaxAddress").ToString)
                    '    BindToText(tbCustTaxNPWP, dr("CustTaxNPWP").ToString)
                    'Else
                    '    tbCustTaxAddress.Text = ""
                    '    tbCustTaxNPWP.Text = ""
                    'End If
                End If

                'If ViewState("Sender") = "btnSPBNo" Then
                '    BindToText(tbSPB, Session("Result")(0).ToString)
                '    BindToText(tbCustCode, Session("Result")(1).ToString)
                '    BindToText(tbCustName, Session("Result")(2).ToString)
                '    BindToDropList(ddlTerm, Session("Result")(3).ToString)
                '    BindToDate(tbDueDate, Session("Result")(4).ToString)
                '    BindToDropList(ddlCurr, Session("Result")(5).ToString)
                '    BindToText(tbRate, Session("Result")(6).ToString)
                '    BindToText(tbSPBManual, Session("Result")(7).ToString)
                '    BindToText(tbCurr, Session("Result")(5).ToString)
                '    tbTotalForex.Text = FormatFloat(tbTotalForex.Text, ViewState("DigitQty"))
                '    tbPPN.Text = FormatFloat(tbPPN.Text, ViewState("DigitQty"))
                '    tbTotalInvoice.Text = FormatFloat(tbTotalInvoice.Text, ViewState("DigitQty"))
                'End If
                'If ViewState("Sender") = "btnDPNo" Then
                '    'ResultField = "DP_No, Currency, ForexRate, PPnRate, PPn, BaseForex, PPNForex, TotalForex, BasePaid, PPNPaid, TotalPaid "
                '    tbDPNo.Text = Session("Result")(0).ToString
                '    tbDPCurrency.Text = Session("Result")(1).ToString
                '    ViewState("DigitCurrDt2") = SQLExecuteScalar("Select Digit FROM VMsCurrency WHERE Currency = " + QuotedStr(tbDPCurrency.Text), ViewState("DBConnection"))
                '    tbDPRate.Text = FormatFloat(Session("Result")(2).ToString, ViewState("DigitRate"))
                '    If tbDPCurrency.Text = ViewState("Currency") Then
                '        tbDPRate.Enabled = False
                '    End If
                '    If tbDPCurrency.Text = ddlCurr.SelectedValue Then
                '        tbDPRate.Enabled = False
                '    End If
                '    If tbDPCurrency.Text <> ddlCurr.SelectedValue Then
                '        tbDPRate.Enabled = True
                '    End If
                '    tbDPPPnRate.Text = FormatFloat(Session("Result")(3).ToString, ViewState("DigitRate"))
                '    tbDPPPnPercent.Text = FormatFloat(Session("Result")(4).ToString, ViewState("DigitPercent"))
                '    tbDPBase.Text = FormatFloat(Session("Result")(5).ToString, ViewState("DigitCurrDt2"))
                '    tbDPPPn.Text = FormatFloat(Session("Result")(6).ToString, ViewState("DigitCurrDt2"))
                '    tbDPTotal.Text = FormatFloat(Session("Result")(7).ToString, ViewState("DigitCurrDt2"))
                '    tbPaidBase.Text = FormatFloat(Session("Result")(8).ToString, ViewState("DigitCurrDt2"))
                '    tbPaidPPN.Text = FormatFloat(Session("Result")(9).ToString, ViewState("DigitCurrDt2"))
                '    tbPaidTotal.Text = FormatFloat(Session("Result")(10).ToString, ViewState("DigitCurrDt2"))
                '    tbDPBaseForex.Text = FormatFloat(CFloat(tbDPBase.Text) - CFloat(tbPaidBase.Text), ViewState("DigitCurrDt2"))
                '    tbDPPPnForex.Text = FormatFloat(CFloat(tbDPPPn.Text) - CFloat(tbPaidPPN.Text), ViewState("DigitCurrDt2"))
                '    tbDPTotalForex.Text = FormatFloat(CFloat(tbDPBaseForex.Text) + CFloat(tbDPPPnForex.Text), ViewState("DigitCurrDt2"))
                '    If tbDPCurrency.Text <> ddlCurr.SelectedValue Then
                '        tbDPDPInvoice.Text = FormatFloat((CFloat(tbDPBaseForex.Text) * CFloat(tbDPRate.Text)) / CFloat(tbRate.Text), ViewState("DigitCurrDt2"))
                '    Else
                '        tbDPDPInvoice.Text = tbDPBaseForex.Text
                '    End If
                'End If
                If ViewState("Sender") = "btnGetDt" Then
                    Dim drResult As DataRow
                    Dim FirstTime As Boolean = True
                    For Each drResult In Session("Result").Rows
                        'insert
                        If FirstTime Then
                            If tbCustCode.Text = "" Then
                                BindToText(tbSPB, drResult("Reference"))
                                BindToText(tbSPBManual, drResult("SPBManualNo"))
                                BindToText(tbCustCode, drResult("Customer"))
                                BindToText(tbCustName, drResult("Customer_Name"))
                                tbCustCode_TextChanged(Nothing, Nothing)
                            End If
                            'BindToDropList(ddlReport, drResult("Report"))
                            ChangeReport("Add", "Y", True, tbDate, tbRate, tbPPnNo, tbPPndate, tbPpnRate) 'ddlReport.SelectedValue
                            'BindToDropList(ddlTerm, drResult("Term"))

                            ' BindToDropList(ddlCurr, drResult("Currency"))

                            ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, ViewState("DBConnection").ToString)
                            'lbDigit.Text = ViewState("DigitCurr")
                            tbDueDate.SelectedDate = FindDueDate(ddlTerm.SelectedValue, tbDate.SelectedDate, ViewState("DBConnection"))
                        End If
                        Dim MaxItem As String
                        ' ViewState("No") = drResult("SJ_No")
                        'If CekExistData(ViewState("Dt"), "SJNo,Product", drResult("SJ_No") + "|" + drResult("Product_Code")) = False Then
                        Dim dr As DataRow
                        MaxItem = GetNewItemNo(ViewState("Dt"))
                        'Dim dr As DataRow
                        dr = ViewState("Dt").NewRow
                        dr("ItemNo") = MaxItem
                        dr("TahunTanam") = drResult("StatusTanam")
                        dr("TahunTanamName") = drResult("StatusTanamName")
                        dr("PriceForex") = FormatFloat(drResult("Price"), ViewState("DigitQty"))
                        dr("NettoWeight") = FormatFloat(drResult("Netto_Kgs"), ViewState("DigitQty"))
                        dr("AmountForex") = FormatFloat(drResult("Price") * drResult("Netto_Kgs"), ViewState("DigitQty"))
                        ViewState("Dt").Rows.Add(dr)
                        'End If
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
        tbPPNForex.Attributes.Add("ReadOnly", "True")
        tbTotalForex.Attributes.Add("ReadOnly", "True")
        tbTotalInvoice.Attributes.Add("ReadOnly", "True")

        tbAmountForex.Attributes.Add("ReadOnly", "True")
        tbNetto.Attributes.Add("ReadOnly", "True")

        tbRate.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbPPN.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbPriceForex.Attributes.Add("OnKeyDown", "return PressNumeric();")

        tbPPN.Attributes.Add("OnBlur", "setformat();")
        tbPPNForex.Attributes.Add("OnBlur", "setformat();")
        tbTotalForex.Attributes.Add("OnBlur", "setformat();")
        tbTotalInvoice.Attributes.Add("OnBlur", "setformat();")

        tbAmountForex.Attributes.Add("OnBlur", "setformatdt('');")

        tbNetto.Attributes.Add("OnBlur", "setformatdt('');")

        
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
            DT = BindDataTransaction(GetStringHd(""), StrFilter, ViewState("DBConnection").ToString)
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
        Return "SELECT * From V_FNCustInvTBSDt WHERE TransNmbr = " + QuotedStr(Nmbr)
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
                Session("SelectCommand") = "EXEC S_FNFormCustInvTBS " + Result
                Session("ReportFile") = ".../../../Rpt/FormCustInvTBS.frx"
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
                        Session("SelectCommand") = "EXEC S_FNFormCustInvTBS " + Result + ", 'Buyer' "
                        Session("ReportFile") = ".../../../Rpt/FormCustInvTBS.frx"
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
                        Result = ExecSPCommandGo(ActionValue, "S_FNCustInvTBS", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            btnSPB.Visible = True
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
            'If dt.Rows.Count > 0 Then
            '    dr = dt.Rows(0)
            '    ViewState("SJ_No") = dr("SJNo").ToString
            'End If

            'If ViewState("Dt") = Nothing Then
            '    GridDt.Columns(0).Visible = False
            'Else
            '    GridDt.Columns(0).Visible = True
            'End If

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
                If ViewState("DtValue") <> tbNo.Text Then
                    If CekExistData(ViewState("Dt"), "ItemNo", tbNo.Text) Then
                        lbStatus.Text = "Item No " + tbNo.Text + " has been already exist"
                        Exit Sub
                    End If
                End If
                Row = ViewState("Dt").Select("ItemNo = " + QuotedStr(ViewState("DtValue")))(0)
                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                Row("ItemNo") = tbNo.Text
                Row("JenisBarang") = tbDescription.Text
                Row("PriceForex") = tbPriceForex.Text
                Row("AmountForex") = cekValue(tbAmountForex.Text)
                Row("NettoWeight") = tbNetto.Text
                Row.EndEdit()

            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If
                If CekExistData(ViewState("Dt"), "ItemNo", tbNo.Text) = True Then
                    lbStatus.Text = MessageDlg("Item No " + tbNo.Text + " has already been exist")
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("ItemNo") = tbNo.Text
                dr("JenisBarang") = tbDescription.Text
                dr("PriceForex") = tbPriceForex.Text
                dr("AmountForex") = cekValue(tbAmountForex.Text)
                dr("NettoWeight") = tbNetto.Text
                
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
            If pnlEditDt.Visible = True Then
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
            'CekSJ = SQLExecuteScalar("SELECT Report FROM V_STSJPendingHdForCI WHERE SJ_No = " + QuotedStr(ViewState("SJ_No").ToString), ViewState("DBConnection").ToString)
            If ddlReport.SelectedValue <> CekSJ Then
                ddlReport.SelectedValue = CekSJ
            End If

            'Save Hd
            If ViewState("StateSave") = "Insert" Then
                'ddlReport.SelectedValue
                tbCode.Text = GetAutoNmbrParamAll("CTB", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
                'End If
                SQLString = "INSERT INTO FINCustInvTBSHd (TransNmbr, FgReport, Status, TransDate, " + _
                "CustCode, Attn, SPBNo, SPBManual, BankReceipt, " + _
                "Term, DueDate, PPnNo, PPnDate, PPnRate, CurrCode, ForexRate, " + _
                "PPn, PPnForex, BaseForex, TotalForex, " + _
                "Remark, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(ddlReport.SelectedValue) + _
                ", 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', " + _
                QuotedStr(tbCustCode.Text) + ", " + QuotedStr(tbAttn.Text) + ", " + QuotedStr(tbSPB.Text) + ", " + _
                QuotedStr(tbSPBManual.Text) + ", " + QuotedStr(ddlReceipt.SelectedValue) + ", " + _
                QuotedStr(ddlTerm.SelectedValue) + ", '" + Format(tbDueDate.SelectedValue, "yyyy-MM-dd") + "', " + _
                QuotedStr(tbPPnNo.Text) + ", '" + Format(tbPPndate.SelectedValue, "yyyy-MM-dd") + "', " + tbPpnRate.Text.Replace(",", "") + ", " + _
                QuotedStr(ddlCurr.SelectedValue) + ", " + tbRate.Text.Replace(",", "") + ", " + _
                tbPPN.Text.Replace(",", "") + ", " + tbPPNForex.Text.Replace(",", "") + ", " + _
                tbTotalForex.Text.Replace(",", "") + ", " + tbTotalInvoice.Text.Replace(",", "") + ", " + _
                QuotedStr(tbRemark.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", getDate()"

            Else
                'InvNo = SQLExecuteScalar("Select TOP 1 TransNmbr from FINCustInvTBSHd WHERE Reference = " + QuotedStr(tbRef.Text) + " AND Status <> 'D' AND TransNmbr <> " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                'If InvNo.ToString.Length >= 5 Then
                '    lbStatus.Text = "Save failed... SO No " + QuotedStr(tbRef.Text) + " has already used by Invoice No " + QuotedStr(InvNo)
                '    Exit Sub
                'End If
                Dim CekStatus As String
                CekStatus = SQLExecuteScalar("Select Status FROM FINCustInvTBSHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                If CekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                'Dim UserBook As String
                'UserBook = SQLExecuteScalar("Select UserEdit FROM FINCustInvTBSHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                'If UserBook <> ViewState("UserId").ToString Then
                '    lbStatus.Text = "Save failed... Data has already modified by another user (" + UserBook + ")"
                '    Exit Sub
                'End If
                Dim Stat As String
                If ViewState("StatusTrans") = "G" Then
                    Stat = "H"
                Else : Stat = ViewState("StatusTrans")
                End If
                SQLString = "UPDATE FINCustInvTBSHd SET CustCode = " + QuotedStr(tbCustCode.Text) + ", Attn = " + QuotedStr(tbAttn.Text) + _
                ", Term = " + QuotedStr(ddlTerm.SelectedValue) + ", BankReceipt = " + QuotedStr(ddlReceipt.SelectedValue) + _
                ", PPnNo = " + QuotedStr(tbPPnNo.Text) + _
                ", SPBNo = " + QuotedStr(tbSPB.Text) + ", SPBManual = " + QuotedStr(tbSPBManual.Text) + _
                ", PPnDate = '" + Format(tbPPndate.SelectedValue, "yyyy-MM-dd") + "', PPnRate = " + tbPpnRate.Text.Replace(",", "") + _
                ", DueDate = '" + Format(tbDueDate.SelectedValue, "yyyy-MM-dd") + "'" + _
                ", CurrCode = " + QuotedStr(ddlCurr.SelectedValue) + ", ForexRate = " + tbRate.Text.Replace(",", "") + _
                ", BaseForex = " + tbTotalForex.Text.Replace(",", "") + ", PPn = " + tbPPN.Text.Replace(",", "") + ", PPnForex = " + tbPPNForex.Text.Replace(",", "") + _
                ", TotalForex = " + tbTotalInvoice.Text.Replace(",", "") + _
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


            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString 'System.Configuration.ConfigurationManager.AppSettings.Get("ConString")
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, ItemNo, JenisBarang, TahunTanam, PriceForex, AmountForex,  " + _
                                        "NettoWeight FROM FINCustInvTBSDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
            "UPDATE FINCustInvTBSDt SET ItemNo = @ItemNo, Jenisbarang = @Jenisbarang, " + _
            "TahunTanam = @TahunTanam, " + _
            "PriceForex = @PriceForex, AmountForex = @AmountForex, Nettoweight = @Nettoweight " + _
            "WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND ItemNo = @OldItemNo ", con)

            ' Define output parameters.
            Update_Command.Parameters.Add("@ItemNo", SqlDbType.Int, 4, "ItemNo")
            Update_Command.Parameters.Add("@Jenisbarang", SqlDbType.VarChar, 100, "Jenisbarang")
            Update_Command.Parameters.Add("@TahunTanam", SqlDbType.VarChar, 60, "TahunTanam")
            Update_Command.Parameters.Add("@PriceForex", SqlDbType.Float, 23, "PriceForex")
            Update_Command.Parameters.Add("@AmountForex", SqlDbType.Float, 23, "AmountForex")
            Update_Command.Parameters.Add("@Nettoweight", SqlDbType.Float, 23, "Nettoweight")
            
            '' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@OldItemNo", SqlDbType.Int, 4, "ItemNo")
            param.SourceVersion = DataRowVersion.Original
            
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM FINCustInvTBSDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND ItemNo = @ItemNo ", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@ItemNo", SqlDbType.Int, 4, "ItemNo")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("FINCustInvTBSDt")

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
            ViewState("DigitCurr") = 0
            'ddlCurr.SelectedValue = ViewState("Currency").ToString
            'ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, ViewState("DBConnection").ToString)
            pnlDt.Visible = True
            BindDataDt("")

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
            tbSPB.Text = ""
            tbSPBManual.Text = ""
            tbAttn.Text = ""
            ddlTerm.SelectedIndex = 0
            ddlReceipt.SelectedValue = ""
            tbPPnNo.Text = ""
            tbPPndate.SelectedDate = Nothing
            tbPpnRate.Text = ""
            ddlCurr.SelectedValue = ViewState("Currency")
            tbRate.Text = "0"
            tbPPN.Text = "0" ' sesuai dengan FgPPn di MsCustomer
            tbPPNForex.Text = "0"
            tbTotalForex.Text = "0"
            tbRemark.Text = ""
            tbDate.SelectedDate = ViewState("ServerDate") 'Today
            ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
            
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbNo.Text = "1"
            tbPriceForex.Text = "0"
            tbAmountForex.Text = "0"
            tbNetto.Text = "0"
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
        Dim amount, total, Base As Double
        Try
            total = 0
            amount = 0
            For Each dr In ViewState("Dt").Rows
                If Not dr.RowState = DataRowState.Deleted Then
                    amount = amount + CFloat(dr("AmountForex").ToString)
                    total = total + CFloat(dr("NettoWeight").ToString)
                End If
            Next
            ViewState("DigitCurr") = SQLExecuteScalar("Select Digit FROM VMsCurrency WHERE Currency = " + QuotedStr(ddlCurr.SelectedValue), ViewState("DBConnection"))
            '(Total Netto Forex * 100 / (100+PPn)) + Disc Forex
            tbTotalForex.Text = FormatFloat((amount * 100 / (100 + CFloat(tbPPN.Text))), CInt(ViewState("DigitCurr")))
            Base = (amount * 100 / (100 + CFloat(tbPPN.Text)))
            
            tbPPNForex.Text = FormatFloat((CFloat(tbPPN.Text) / 100) * (Base), ViewState("DigitCurr"))
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
                BindToText(tbCurr, dr("Currency"))
                'BindToDropList(ddlTerm, Dr("Term").ToString)
                'tbDueDate.SelectedDate = FindDueDate(ddlTerm.SelectedValue, tbDate.SelectedDate, ViewState("DBConnection"))
                BindToText(tbAttn, dr("Contact_Person").ToString)
                If dr("PPn").ToString = "Y" Then
                    tbPPN.Text = "10"
                Else
                    tbPPN.Text = "0"
                End If
                
            Else
                tbCustCode.Text = ""
                tbCustName.Text = ""
                ddlCurr.SelectedValue = ViewState("Currency")
                tbAttn.Text = ""
                tbPPN.Text = "0"
                
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
            If tbSPB.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("SPB No must have value")
                tbSPB.Focus()
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
            'Dim SQLStr, Hasil As String
            'SQLStr = "DECLARE @A VARCHAR(255) " + _
            '    "EXEC S_FNCekBalance " + FormatFloat(CFloat(tbPPN.Text), 4).Replace(",", "") + ", " + FormatFloat(CFloat(tbPPNForex.Text), 4).Replace(",", "") + _
            '    "," + FormatFloat(CFloat(tbBaseForex.Text), 4).Replace(",", "") + "," + FormatFloat(CFloat(tbDiscForex.Text), 4).Replace(",", "") + _
            '    "," + FormatFloat(CFloat(tbDPForex.Text), 4).Replace(",", "") + "," + FormatFloat(CFloat(tbTotalInvoice.Text), 4).Replace(",", "") + _
            '    ", @A OUT SELECT @A"

            'Hasil = SQLExecuteScalar(SQLStr, ViewState("DBConnection"))
            'Hasil = Replace(Hasil, "0", "")

            'If Trim(Hasil) <> "" Then
            '    lbStatus.Text = MessageDlg(Hasil)
            '    Return False
            'End If
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
                If Dr("ItemNo").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Item No Must Have Value")
                    Return False
                End If
                If Dr("JenisBarang").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Description Must Have Value")
                    Return False
                End If
                If CFloat(Dr("PriceForex").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Price Must Have Value")
                    Return False
                End If
            Else
                If tbNo.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Item No Must Have Value")
                    tbNo.Focus()
                    Return False
                End If
                If tbDescription.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Description Must Have Value")
                    tbNo.Focus()
                    Return False
                End If
                If CFloat(tbTotalForex.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Netto Must Have Value")
                    tbTotalForex.Focus()
                    Return False
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
            FDateName = "Date, Due Date"
            FDateValue = "TransDate, DueDate"
            FilterName = "Invoice No, Date, Customer, SPB No, SPB Manual, PPn No, Currency, Term, Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), Customer, SPBNo, SPBManual, PPnNo, Currency, Term_Name, Remark"
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
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, pnlDt, GridDt)
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
                        MultiView1.ActiveViewIndex = 0
                        Menu1.Items.Item(0).Selected = True
                        ViewState("TransNmbr") = GVR.Cells(2).Text
                        ViewState("StatusTrans") = GVR.Cells(3).Text
                        GridDt.PageIndex = 0
                        FillTextBoxHd(ViewState("TransNmbr"))
                        BindDataDt(ViewState("TransNmbr"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        btnHome.Visible = False
                        If tbCustName.Text.Trim = "" Then
                            GridDt.Columns(0).Visible = False
                        End If
                        'ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, ViewState("DBConnection").ToString)                        
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                        'SQLExecuteNonQuery("Update FINCustInvTBSHd SET UserEdit = " + QuotedStr(ViewState("UserId").ToString) + " WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), ViewState("DBConnection").ToString)
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
                        ''SQLStr = "DECLARE @A VARCHAR(255) " + _
                        ''    "EXEC S_FNCustInvCekAmount " + QuotedStr(GVR.Cells(2).Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ",'Print', @A OUT " + _
                        ''    "SELECT @A"

                        ''Hasil = SQLExecuteScalar(SQLStr, ViewState("DBConnection"))
                        ''Hasil = Replace(Hasil, "0", "")

                        ''If Trim(Hasil) <> "" Then
                        ''    lbStatus.Text = MessageDlg(Hasil)
                        ''    Exit Sub
                        ''End If

                        'If (GVR.Cells(3).Text = "P") Or (GVR.Cells(3).Text = "G") Or (GVR.Cells(3).Text = "H") Then
                        If (GVR.Cells(3).Text <> "D") Then '2015-06-03-0191
                            Session("DBConnection") = ViewState("DBConnection")
                            Session("SelectCommand") = "EXEC S_FNFormCustInvTBS '''" + GVR.Cells(2).Text + "'''"
                            
                            Session("ReportFile") = ".../../../Rpt/FormCustInvTBS.frx"
                            AttachScript("openprintdlg();", Page, Me.GetType)
                        Else
                            'lbStatus.Text = "Cannot print or preview, status Invoice No " + QuotedStr(GVR.Cells(2).Text) + " must be posted or get approval"
                            lbStatus.Text = MessageDlg("Data must be delete to Print") '"Cannot print or preview, status Invoice No " + QuotedStr(GVR.Cells(2).Text) + " must be posted"
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
        dr = ViewState("Dt").Select("ITemNo = " + QuotedStr(GVR.Cells(1).Text))
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
            ViewState("DtValue") = GVR.Cells(1).Text
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            'ViewState("ProductDt") = GVR.Cells(3).Text
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
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "ItemNo")) Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                    '' add the UnitPrice and QuantityTotal to the running total variables
                    'BaseForex += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "AmountForex"))
                    'DiscForex += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DiscForex"))
                ElseIf e.Row.RowType = DataControlRowType.Footer Then
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
            Dt = BindDataTransaction(GetStringHd(""), "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            ViewState("Hd") = Dt
            tbCode.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            'BindToDropList(ddlReport, Dt.Rows(0)("FgReport").ToString)
            BindToText(tbSPB, Dt.Rows(0)("SPBNo").ToString)
            BindToText(tbSPBManual, Dt.Rows(0)("SPBManual").ToString)
            BindToText(tbCustCode, Dt.Rows(0)("CustCode").ToString)
            BindToText(tbCustName, Dt.Rows(0)("CustName").ToString)
            BindToText(tbAttn, Dt.Rows(0)("Attn").ToString)
            BindToDropList(ddlTerm, Dt.Rows(0)("Term").ToString)
            BindToDate(tbDueDate, Dt.Rows(0)("Duedate").ToString)
            BindToText(tbPPnNo, Dt.Rows(0)("PPnNo").ToString)
            BindToDate(tbPPndate, Dt.Rows(0)("PPndate").ToString)
            BindToDropList(ddlCurr, Dt.Rows(0)("CurrCode").ToString)
            BindToText(tbCurr, Dt.Rows(0)("CurrCode").ToString)
            ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, ViewState("DBConnection").ToString)
            'BindToText(tbPpnRate, Dt.Rows(0)("PPnRate").ToString, ViewState("DigitRate"))
            BindToText(tbPpnRate, Dt.Rows(0)("PPnRate").ToString, ViewState("DigitCurr"))
            'BindToText(tbRate, Dt.Rows(0)("ForexRate").ToString, ViewState("DigitRate"))
            BindToText(tbRate, Dt.Rows(0)("ForexRate").ToString, ViewState("DigitCurr"))
            BindToDropList(ddlReceipt, Dt.Rows(0)("BankReceipt").ToString)
            'BindToText(tbDiscForex, Dt.Rows(0)("DiscForex").ToString)
            BindToText(tbPPN, Dt.Rows(0)("PPn").ToString, ViewState("DigitPercent"))
            BindToText(tbPPNForex, Dt.Rows(0)("PPnForex").ToString, ViewState("DigitCurr"))
            BindToText(tbTotalForex, Dt.Rows(0)("TotalForex").ToString, ViewState("DigitCurr"))
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)

        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal Product As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("ItemNo = " + QuotedStr(Product))
            If Dr.Length > 0 Then
                BindToText(tbNo, Dr(0)("ItemNo").ToString)
                BindToText(tbDescription, Dr(0)("JeniSBarang").ToString)
                BindToText(tbStatusTanam, Dr(0)("TahunTanam").ToString)
                BindToText(tbNetto, Dr(0)("NettoWeight").ToString)
                BindToText(tbPriceForex, Dr(0)("PriceForex").ToString)
                BindToText(tbAmountForex, Dr(0)("AmountForex").ToString)
                
            End If
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

    Protected Sub btnSPB_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSPB.Click
        Dim ResultField, CriteriaField, DefaultField, ResultSame, Filter As String
        Try
            Filter = ""
            If tbCustCode.Text.Trim <> "" Then
                Filter = Filter + " AND Reference = " + QuotedStr(tbSPB.Text)
            End If
            Session("Filter") = "EXEC S_FNCustInvTBSReff " + "'', " + QuotedStr(Filter)
            ResultField = "Reference, SPBManualNo, Customer, Customer_Name, StatusTanam, StatusTanamName, Netto_Kgs, Price, Term, Currency"
            DefaultField = "Reference"
            CriteriaField = "Reference, SPBManualNo, Customer, Customer_Name, StatusTanam, StatusTanamName, Netto_Kgs, Price, Term, Currency"
            Session("ClickSame") = "Reference"
            ViewState("Sender") = "btnGetDt"
            Session("ColumnDefault") = DefaultField.Split(",")
            Session("Column") = ResultField.Split(",")
            Session("CriteriaField") = CriteriaField.Split(",")
            ResultSame = "Reference"
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

    'Protected Sub btnSJNo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSJNo.Click
    '    Dim ResultField As String
    '    Try

    '        Session("filter") = " EXEC S_FNCustInvGetDt ''"
    '        ResultField = "SJ_No, SO_No, Product_Code, Product_Name, Specification, Qty, QtyM2, QtyRoll, Unit"
    '        ViewState("Sender") = "btnSJNo"
    '        Session("Column") = ResultField.Split(",")
    '        Session("DBConnection") = ViewState("DBConnection")
    '        AttachScript("OpenSearchDlg();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lbStatus.Text = "btn SJNo Error : " + ex.ToString
    '    End Try
    'End Sub

    Private Function cekValue(ByVal val As String) As String
        If val.Trim = "" Then
            Return "0"
        Else
            Return val
        End If
    End Function

    Protected Sub tbPriceForex_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPriceForex.TextChanged
        Try
            
            tbAmountForex.Text = FormatFloat(CFloat(tbNetto.Text) * CFloat(tbPriceForex.Text), ViewState("DigitCurr"))
            'tbPriceForex.Text = FormatFloat(tbPriceForex.Text)
            'AttachScript("setformatdt();", Me.Page, Me.GetType())
            ' tbDescription.Focus()
        Catch ex As Exception
            Throw New Exception("tbPriceForexDt_TextChanged Error : " + ex.ToString)
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
    'Protected Sub tbRate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbRate.TextChanged
    '    Try
    '        If tbDPCurrency.Text <> ddlCurr.SelectedValue Then
    '            tbDPDPInvoice.Text = FormatFloat((CFloat(tbDPBaseForex.Text) * CFloat(tbDPRate.Text)) / CFloat(tbRate.Text), ViewState("DigitCurrDt2"))
    '        Else
    '            tbDPDPInvoice.Text = tbDPBaseForex.Text
    '        End If
    '    Catch ex As Exception
    '        lbStatus.Text = "tbRate_TextChanged Error : " + ex.ToString
    '    End Try
    'End Sub
End Class