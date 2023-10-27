Imports System.Data
Imports System.Data.SqlClient
'Imports System.Data.Odbc
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc


Partial Class CustRetur
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    'Protected GetStringHd As String = "Select * From V_FNCustReturHd"
    
    Protected GetStringHd As String = "Select distinct TransNmbr, Nmbr, TransDate, Trans_Date, Status, FgReport, FgPriceTax, Customer, Customer_Name, BillTo, Bill_To_Name, Attn, Term, Term_Name, DueDate, Due_Date, PPnNo, PPnDate, PPn_Date, PPnRate, Currency, ForexRate, BaseForex, DiscForex, PPn, PPnForex, TotalForex, Remark, UserPrep, DatePrep, UserAppr, DateAppr From V_FNCustReturHd "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                FillCombo(ddlTerm, "EXEC S_GetTerm", False, "Term_Code", "Term_Name", ViewState("DBConnection"))
                FillCombo(ddlCurr, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
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
                    BindToText(tbCustCode, Session("Result")(0).ToString)
                    BindToText(tbCustName, Session("Result")(1).ToString)
                    BindToDropList(ddlCurr, Session("Result")(2).ToString)
                    BindToText(tbBillToCode, Session("Result")(0).ToString)
                    BindToText(tbBillToName, Session("Result")(1).ToString)
                    BindToText(tbAttn, TrimStr(Session("Result")(4).ToString))
                    BindToDropList(ddlTerm, Session("Result")(3).ToString)
                    ddlCurr_SelectedIndexChanged(Nothing, Nothing)
                    tbDueDate.SelectedDate = FindDueDate(ddlTerm.SelectedValue, tbDate.SelectedDate, ViewState("DBConnection"))
                End If
                If ViewState("Sender") = "btnBillTo" Then
                    BindToText(tbBillToCode, Session("Result")(0).ToString)
                    BindToText(tbBillToName, Session("Result")(1).ToString)
                    BindToText(tbAttn, TrimStr(Session("Result")(2).ToString))
                End If
                'If ViewState("Sender") = "btnBPBNo" Then
                '    tbProductCode.Text = Session("Result")(0).ToString
                '    tbProductName.Text = Session("Result")(1).ToString + " " + Session("Result")(2).ToString
                '    tbQty.Text = Session("Result")(3).ToString
                '    tbUnit.Text = Session("Result")(4).ToString
                '    tbQty.Text = FormatFloat(tbQty.Text, Session("DigitQty"))
                '    tbPriceForex.Text = Session("Result")(5).ToString
                'End If
                If ViewState("Sender") = "btnGetDt" Then
                    Dim drResult As DataRow
                    Dim FirstTime As Boolean = True
                    '"SJNo, Product, Product_Name, Qty,Unit, QtyM2,QtyRoll, PriceForex, AmountForex, PPnDate, PPNRate, Disc, DiscForex "
                    For Each drResult In Session("Result").Rows
                        If FirstTime Then
                            lbCustCode.Text = drResult("Customer_Code")
                            BindToText(tbCustCode, drResult("Customer_Code"))
                            tbCustCode_TextChanged(Nothing, Nothing)
                            'BindToDropList(ddlReport, drResult("Report"))
                            ChangeReport("Add", "Y", True, tbDate, tbRate, tbPPnNo, tbPPndate, tbPpnRate) 'ddlReport.SelectedValue
                            BindToText(tbPPnNo, drResult("PPNNo"))
                            If IsDBNull(drResult("PPnDate")) Then
                                tbPPndate.SelectedDate = tbDate.SelectedDate
                            Else
                                BindToDate(tbPPndate, drResult("PPnDate"))
                            End If
                            'BindToDate(tbPPndate, drResult("PPnDate"))
                            ' BindToDate(tbPPndate, drResult("SJCustDate"))
                            If TrimStr(drResult("Rate").ToString) = "" Or IsDBNull(drResult("Rate")) Then
                                ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
                                tbPpnRate.Text = FormatFloat(FindTaxRate(ddlCurr.SelectedValue, tbPPndate.SelectedValue, ViewState("DBConnection").ToString), ViewState("DigitCurr"))

                            Else
                                BindToText(tbRate, FormatNumber(drResult("Rate"), CInt(ViewState("DigitRate"))))
                                BindToText(tbPpnRate, FormatNumber(drResult("PPNRate"), CInt(ViewState("DigitRate"))))
                            End If
                            BindToText(tbCustName, drResult("Customer_Name"))
                            BindToDropList(ddlCurr, drResult("Currency"))
                            ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, ViewState("DBConnection").ToString)
                            BindToText(tbPriceTax, drResult("PriceIncludeTax"))
                        End If
                        If CekExistData(ViewState("Dt"), "RRNo,SJNo,Product", drResult("RR_No") + "|" + drResult("SJNo") + "|" + drResult("Product")) = False Then
                            Dim dr As DataRow
                            dr = ViewState("Dt").NewRow
                            dr("RRNo") = drResult("RR_No")
                            dr("SJNo") = drResult("SJNo")
                            dr("Product") = drResult("Product")
                            dr("Product_Name") = drResult("Product_Name")
                            dr("Unit") = drResult("Unit")
                            dr("Qty") = drResult("Qty")
                            dr("QtyM2") = drResult("QtyM2")
                            dr("QtyRoll") = drResult("QtyRoll")
                            'dr("PriceInUnit") = drResult("PriceInUnit")
                            dr("PriceForex") = drResult("PriceForex")
                            dr("AmountForex") = drResult("AmountForex")
                            dr("Disc") = drResult("Disc")
                            dr("DiscForex") = drResult("DiscForex")
                            dr("NettoForex") = drResult("AmountForex") - drResult("DiscForex")
                            ViewState("Dt").Rows.Add(dr)
                        End If
                        FirstTime = False
                    Next
                    'ChangeReport("Add", "Y", True, tbDate, tbRate, tbPPnNo, tbPPndate, tbPpnRate) 'ddlReport.SelectedValue                    
                    'If ddlReport.SelectedValue = "N" Then
                    '    tbPPN.Text = "0"
                    'End If
                    BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    SumNettoForex()
                    'AttachScript("setformat('-');", Page, Me.GetType())
                    Session("ResultSame") = Nothing
                    Session("ClickSame") = Nothing
                End If

                Session("Result") = Nothing
                ViewState("Sender") = Nothing
                Session("filter") = Nothing
                Session("Column") = Nothing
            End If

        Catch ex As Exception
            lbStatus.Text = "Page Load Error : " + ex.ToString
        End Try
    End Sub

    Private Sub SetInit()
        FillRange(ddlRange)
        ViewState("SortExpression") = Nothing
        ViewState("DigitCurr") = 0
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
        If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
            ddlCommand.Items.Add("Print")
            ddlCommand2.Items.Add("Print")
        End If
        tbBaseForex.Attributes.Add("ReadOnly", "True")
        tbDiscForex.Attributes.Add("ReadOnly", "True")
        tbPPNForex.Attributes.Add("ReadOnly", "True")
        tbTotalForex.Attributes.Add("ReadOnly", "True")
        tbAmountForex.Attributes.Add("ReadOnly", "True")
        tbNettoForex.Attributes.Add("ReadOnly", "True")
        tbPriceTax.Attributes.Add("ReadOnly", "True")

        tbRate.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbPPN.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbPriceForex.Attributes.Add("OnKeyDown", "return PressNumeric();")

        'tbBaseForex.Attributes.Add("OnBlur", "setformat('-');")
        'tbDiscForex.Attributes.Add("OnBlur", "setformat('-');")
        'tbPPN.Attributes.Add("OnBlur", "setformat('-');")
        tbQty.Attributes.Add("OnBlur", "setformatdt();")
        tbQtyM2.Attributes.Add("OnBlur", "setformatdt();")
        tbQtyRoll.Attributes.Add("OnBlur", "setformatdt();")
        'ddlPriceUnit.Attributes.Add("OnBlur", "setformatdt();")
        tbPriceForex.Attributes.Add("OnBlur", "setformatdt();")
        tbAmountForex.Attributes.Add("OnBlur", "setformatdt();")
        tbDiscDtForex.Attributes.Add("OnBlur", "setformatdt();")
        tbNettoForex.Attributes.Add("OnBlur", "setformatdt();")
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


    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue, ddlRange.SelectedValue)

            If AdvanceFilter.Length > 1 Then
                StrFilter = AdvanceFilter
            End If
            DT = BindDataTransaction(GetStringHd, StrFilter, ViewState("DBConnection").ToString)
            If DT.Rows.Count = 0 Then
                lbStatus.Text = "No Data"
                pnlNav.Visible = False
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
        Return "SELECT * From V_FNCustReturDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            GridView1.PageIndex = 0
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
                        'If GVR.Cells(3).Text = "P" Then
                        ListSelectNmbr = GVR.Cells(2).Text
                        If Pertamax Then
                            Result = "'''" + ListSelectNmbr + "''"
                            Pertamax = False
                        Else
                            Result = Result + ",''" + ListSelectNmbr + "''"
                        End If
                        'End If
                    End If
                Next
                Result = Result + "'"

                Session("DBConnection") = ViewState("DBConnection")
                Session("SelectCommand") = "EXEC S_FNFormCustRetur " + Result + ""
                Session("ReportFile") = ".../../../Rpt/FormCustRetur.frx"

                AttachScript("openprintdlg();", Page, Me.GetType)
            ElseIf ActionValue = "Print 2" Then
                Dim GVR As GridViewRow
                Dim CB As CheckBox
                Dim Pertamax As Boolean

                Pertamax = True
                Result = ""

                For Each GVR In GridView1.Rows
                    CB = GVR.FindControl("cbSelect")
                    If CB.Checked Then
                        'If GVR.Cells(3).Text = "P" Then
                        ListSelectNmbr = GVR.Cells(2).Text
                        If Pertamax Then
                            Result = "'''" + ListSelectNmbr + "''"
                            Pertamax = False
                        Else
                            Result = Result + ",''" + ListSelectNmbr + "''"
                        End If
                        'End If
                    End If
                Next
                Result = Result + "'"

                Session("DBConnection") = ViewState("DBConnection")
                Session("SelectCommand") = "EXEC S_FNFormCustRetur " + Result + ""
                Session("ReportFile") = ".../../../Rpt/FormCustRetur.frx"
                AttachScript("openprintdlg();", Page, Me.GetType)
            ElseIf ActionValue = "Print Tax" Then
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
                        'End If
                    End If
                Next
                Result = Result + "'"

                Session("DBConnection") = ViewState("DBConnection")
                Session("SelectCommand") = "EXEC S_FNFormCustReturFPS " + Result
                Session("ReportFile") = ".../../../Rpt/FormCustReturFaktur.frx"
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
                        Result = ExecSPCommandGo(ActionValue, "S_FNCustRetur", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            'ddlReport.Enabled = State
            tbCustCode.Enabled = State
            'tbCustCode2.Enabled = State
            btnCust.Visible = State
            ddlCurr.Enabled = State
            btnBillTo.Visible = True
            btnGetData.Visible = State
        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDt(ByVal Referens As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt") = Nothing
            dt = SQLExecuteQuery(GetStringDt(Referens), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt") = dt
            BindGridDt(dt, GridDt)
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
                If ViewState("DtValue") <> tbBPBNo.Text + "|" + tbSJNo.Text + "|" + tbProductCode.Text Then
                    If CekExistData(ViewState("Dt"), "RRNo,SJNo,Product", tbBPBNo.Text + "|" + tbSJNo.Text + "|" + tbProductCode.Text) Then
                        lbStatus.Text = "RR No " + tbBPBNo.Text + " SJ No " + tbSJNo.Text + " Product " + tbProductName.Text + " has been already exist"
                        Exit Sub
                    End If
                End If
                Row = ViewState("Dt").Select("RRNo+'|'+SJNo+'|'+Product = " + QuotedStr(ViewState("DtValue")))(0)
                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                Row("RRNo") = tbBPBNo.Text
                Row("SJNo") = tbSJNo.Text
                Row("Product") = tbProductCode.Text
                Row("Product_Name") = tbProductName.Text
                Row("Qty") = tbQty.Text
                Row("QtyM2") = tbQtyM2.Text
                Row("QtyRoll") = tbQtyRoll.Text
                Row("Unit") = tbUnit.Text
                If Row("Unit") = "" Then
                    Row("Unit") = DBNull.Value
                End If
                'Row("PriceInUnit") = ddlPriceUnit.SelectedValue
                Row("Remark") = tbRemarkDt.Text
                Row("PriceForex") = tbPriceForex.Text
                Row("AmountForex") = cekValue(tbAmountForex.Text)
                Row("DiscForex") = cekValue(tbDiscDtForex.Text)
                Row("Disc") = cekValue(tbDiscDt.Text)
                Row("NettoForex") = tbNettoForex.Text
                Row.EndEdit()
                SumNettoForex()
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If
                If CekExistData(ViewState("Dt"), "RRNo,SJNo,Product", tbBPBNo.Text + "|" + tbSJNo.Text + "|" + tbProductCode.Text) = True Then
                    lbStatus.Text = "RR No " + tbBPBNo.Text + " SJ No " + tbSJNo.Text + " Product " + tbProductName.Text + " has already been exist"
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("RRNo") = tbBPBNo.Text
                dr("SJNo") = tbSJNo.Text
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
                dr("Disc") = cekValue(tbDiscDt.Text)
                dr("DiscForex") = cekValue(tbDiscDtForex.Text)
                dr("NettoForex") = tbNettoForex.Text
                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            BindGridDt(ViewState("Dt"), GridDt)
            SumNettoForex()
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save Dt Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub

    Private Sub SaveAll()
        Dim SQLString, AddParam As String
        Dim I As Integer
        Try
            If pnlDt.Visible = False Then
                lbStatus.Text = "Detail Data must be saved first"
                Exit Sub
            End If
            If Not (ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit") Then
                Exit Sub
            Else
                ViewState("StateSave") = ViewState("StateHd")
                ViewState("StateHd") = "View"
            End If
            'Save Hd
            If ViewState("StateSave") = "Insert" Then
                AddParam = ""
                'ddlReport.SelectedValue
                tbCode.Text = GetAutoNmbr("CRE", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), AddParam, ViewState("DBConnection").ToString)
                SQLString = "INSERT INTO FINCustReturHd (TransNmbr, FgReport, Status, TransDate, " + _
                "Customer, Attn, BillTo, Term, DueDate, PPnNo, PPnDate, PPnRate, Currency, ForexRate, " + _
                "BaseForex, DiscForex, PPn, PPnForex, TotalForex, " + _
                "Remark, UserPrep, DatePrep,FgPriceIncludeTax) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr("Y") + _
                ", 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', " + _
                QuotedStr(tbCustCode.Text) + ", " + QuotedStr(tbAttn.Text) + ", " + QuotedStr(tbBillToCode.Text) + ", " + _
                QuotedStr(ddlTerm.SelectedValue) + ", '" + Format(tbDueDate.SelectedValue, "yyyy-MM-dd") + "', " + _
                QuotedStr(tbPPnNo.Text) + ", '" + Format(tbPPndate.SelectedValue, "yyyy-MM-dd") + "', " + tbPpnRate.Text.Replace(",", "") + ", " + _
                QuotedStr(ddlCurr.SelectedValue) + ", " + tbRate.Text.Replace(",", "") + ", " + _
                tbBaseForex.Text.Replace(",", "") + ", " + tbDiscForex.Text.Replace(",", "") + ",  " + _
                tbPPN.Text.Replace(",", "") + ", " + tbPPNForex.Text.Replace(",", "") + ", " + _
                tbTotalForex.Text.Replace(",", "") + ", " + _
                QuotedStr(tbRemark.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", getDate(), " + QuotedStr(tbPriceTax.Text)
            Else
                Dim CekStatus As String
                CekStatus = SQLExecuteScalar("Select Status FROM FINCustReturHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                If CekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE FINCustReturHd SET Customer = " + QuotedStr(tbCustCode.Text) + ", Attn = " + QuotedStr(tbAttn.Text) + _
                ", Term = " + QuotedStr(ddlTerm.SelectedValue) + ", BillTo = " + QuotedStr(tbBillToCode.Text) + _
                ", PPnNo = " + QuotedStr(tbPPnNo.Text) + ", PPnDate = '" + Format(tbPPndate.SelectedValue, "yyyy-MM-dd") + "', PPnRate = " + tbPpnRate.Text.Replace(",", "") + _
                ", DueDate = '" + Format(tbDueDate.SelectedValue, "yyyy-MM-dd") + "'" + _
                ", Currency = " + QuotedStr(ddlCurr.SelectedValue) + ", ForexRate = " + tbRate.Text.Replace(",", "") + _
                ", BaseForex = " + tbBaseForex.Text.Replace(",", "") + ", DiscForex = " + tbDiscForex.Text.Replace(",", "") + ", PPn = " + tbPPN.Text.Replace(",", "") + ", PPnForex = " + tbPPNForex.Text.Replace(",", "") + _
                ", TotalForex = " + tbTotalForex.Text.Replace(",", "") + _
                ", Remark = " + QuotedStr(tbRemark.Text) + ", FgPriceIncludeTax = " + QuotedStr(tbPriceTax.Text) + _
                ", TransDate = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', DatePrep = getDate() " + _
                " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            End If
            SQLString = ChangeQuoteNull(SQLString)
            SQLString = SQLString.Replace("'1900-01-01'", "NULL")
            SQLString = SQLString.Replace("'0001-01-01'", "NULL")
            'lbStatus.Text = SQLString
            'Exit Sub
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
            Dim ConnString As String = ViewState("DBConnection").ToString
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, RRNo, SJNo, Product, Qty, Unit, QtyM2, QtyRoll, PriceForex, AmountForex, Disc, DiscForex, NettoForex, Remark FROM FINCustReturDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
            "UPDATE FINCustReturDt SET SJNo = @SJNo, RRNo = @RRNo, " + _
            "Product = @Product, Qty = @Qty, Unit = @Unit, " + _
            "QtyM2 = @QtyM2, QtyRoll = @QtyRoll, Remark = @Remark, " + _
            "PriceForex = @PriceForex, AmountForex = @AmountForex, Disc = @Disc, " + _
            "DiscForex = @DiscForex, NettoForex = @NettoForex " + _
            "WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND SJNo = @OldSJNo AND RRNo = @OldRRNo AND Product = @OldProduct", con)

            ' Define output parameters.
            Update_Command.Parameters.Add("@SJNo", SqlDbType.VarChar, 20, "SJNo")
            Update_Command.Parameters.Add("@RRNo", SqlDbType.VarChar, 20, "RRNo")
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
            Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 255, "Remark")

            '' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@OldSJNo", SqlDbType.VarChar, 20, "SJNo")
            param.SourceVersion = DataRowVersion.Original
            param = Update_Command.Parameters.Add("@OldRRNo", SqlDbType.VarChar, 20, "RRNo")
            param.SourceVersion = DataRowVersion.Original
            param = Update_Command.Parameters.Add("@OldProduct", SqlDbType.VarChar, 20, "Product")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM FINCustReturDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND SJNo = @SJNo AND RRNo = @RRNo AND Product = @Product ", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@SJNo", SqlDbType.VarChar, 20, "SJNo")
            param.SourceVersion = DataRowVersion.Original
            param = Delete_Command.Parameters.Add("@RRNo", SqlDbType.VarChar, 20, "RRNo")
            param.SourceVersion = DataRowVersion.Original
            param = Delete_Command.Parameters.Add("@Product", SqlDbType.VarChar, 20, "Product")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command


            Dim Dt As New DataTable("FINCustReturDt")

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

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, pnlDt, GridDt)
            newTrans()
            ChangeReport("Add", "Y", True, tbDate, tbRate, tbPPnNo, tbPPndate, tbPpnRate) 'ddlReport.SelectedValue
            btnHome.Visible = False
            'ddlReport.Enabled = False
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
            'ddlCurr.SelectedValue = Session("Currency").ToString
            'ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, Session("DBConnection").ToString)
            pnlDt.Visible = True
            BindDataDt("")
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbCode.Text = ""
            tbDate.SelectedDate = ViewState("ServerDate") 'Today
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
            tbPPN.Text = "10"
            tbPPNForex.Text = "0"
            tbTotalForex.Text = "0"
            tbRemark.Text = ""
            ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbBPBNo.Text = ""
            tbSJNo.Text = ""
            tbProductCode.Text = ""
            tbProductName.Text = ""
            tbRemarkDt.Text = ""
            tbQty.Text = "0"
            tbPriceForex.Text = "0"
            tbAmountForex.Text = "0"
            tbNettoForex.Text = "0"
            tbPriceForex.Text = "0"
            tbDiscDtForex.Text = "0"
            tbDiscDt.Text = "0"
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
        Catch ex As Exception
            lbStatus.Text = "btn save all error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCust_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCust.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "select * from VMsCustomer"
            ResultField = "Customer_Code, Customer_Name, Currency, Term, Contact_Person, Address, NPWP"
            ViewState("Sender") = "btnCust"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Cust Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbCustCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCustCode.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Customer", tbCustCode.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbCustCode.Text = Dr("Customer_Code").ToString
                tbCustName.Text = Dr("Customer_Name").ToString

                'tbCustCode2.Text = String.Format("{0}" + Environment.NewLine + "{1}", Dr("Customer_Name").ToString + " (" + Dr("Customer_Code").ToString + ")", Dr("Address").ToString)
                'tbCustTaxNPWP.Text = Dr("NPWP").ToString
                BindToDropList(ddlCurr, Dr("Currency").ToString)
                BindToDropList(ddlTerm, Dr("Term").ToString)
                tbDueDate.SelectedDate = FindDueDate(ddlTerm.SelectedValue, tbDate.SelectedDate, ViewState("DBConnection").ToString)
                BindToText(tbBillToCode, Dr("Customer_Code").ToString)
                BindToText(tbBillToName, Dr("Customer_Name").ToString)
                BindToText(tbAttn, Dr("Contact_Person").ToString)
            Else
                tbCustCode.Text = ""
                tbCustName.Text = ""
                'tbCustCode2.Text = ""
                'tbCustTaxNPWP.Text = ""
                ddlCurr.SelectedValue = ViewState("Currency")
                tbBillToCode.Text = ""
                tbBillToName.Text = ""
                tbAttn.Text = ""
            End If
            ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
            'AttachScript("setformat('-');", Page, Me.GetType())
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
        'ChangeReport("Edit", "Y", ddlCurr.SelectedValue = ViewState("Currency"), tbDate, tbRate, tbPPnNo, tbPPndate, tbPpnRate) 'ddlReport.SelectedValue
        'AttachScript("setformat('-');", Page, Me.GetType())        
        tbRate.Focus()
    End Sub

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt.Click, btnAddDt2.Click
        Cleardt()
        If CekHd() = False Then
            Exit Sub
        End If
        ViewState("StateDt") = "Insert"
        MovePanel(pnlDt, pnlEditDt)
        EnableHd(False)
        StatusButtonSave(False)
        tbProductCode.Focus()
    End Sub

    Function CekHd() As Boolean
        Try

            If tbDate.IsNull Then
                lbStatus.Text = MessageDlg("Customer Invoice Date must have value")
                tbDate.Focus()
                Return False
            End If
            'If CInt(ViewState("GLYear")) <> Year(tbDate.SelectedValue) Or CInt(ViewState("GLPeriod")) <> Month(tbDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(ViewState("GLPeriod")) + " " + ViewState("GLYear").ToString.Trim)
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
                If Dr("RRNo").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("BPB No Must Have Value")
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
                If CFloat(Dr("QtyM2").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty M2 Must Have Value")
                    Return False
                End If
                If CFloat(Dr("QtyRoll").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Roll Must Have Value")
                    Return False
                End If
            Else
                If tbBPBNo.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("BPB No Must Have Value")
                    tbBPBNo.Focus()
                    Return False
                End If
                If tbProductCode.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Product Must Have Value")
                    tbProductCode.Focus()
                    Return False
                End If
                If tbUnit.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Unit Must Have Value")
                    tbUnit.Focus()
                    Return False
                End If
                If CFloat(tbQty.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Must Have Value")
                    tbQty.Focus()
                    Return False
                End If
                If CFloat(tbQtyM2.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty M2 Must Have Value")
                    tbQty.Focus()
                    Return False
                End If
                If CFloat(tbQtyRoll.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Roll Must Have Value")
                    tbQty.Focus()
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
            FilterName = "Reference, Status, Date, RRNo, CustomerName, Bill To, Term, Due Date, Currency, Remark"
            FilterValue = "TransNmbr, Status, Trans_Date, RR_No, Customer_Name, Bill_To_Name, Term_Name, Due_Date, Currency, Remark "
            Session("DBConnection") = ViewState("DBConnection")
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
                        FillTextBoxHd(ViewState("TransNmbr"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        btnHome.Visible = False
                        ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, ViewState("DBConnection").ToString)
                        'ChangeReport("Edit", "Y", ddlCurr.SelectedValue = ViewState("Currency"), tbDate, tbRate, tbPPnNo, tbPPndate, tbPpnRate) 'ddlReport.SelectedValue
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
                        Session("DBConnection") = ViewState("DBConnection")
                        Session("SelectCommand") = "EXEC S_FNFormCustRetur '''" + GVR.Cells(2).Text + "'''"
                        Session("ReportFile") = ".../../../Rpt/FormCustRetur.frx"
                        AttachScript("openprintdlg();", Page, Me.GetType)
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
                        Session("DBConnection") = ViewState("DBConnection")
                        Session("SelectCommand") = "EXEC S_FNFormCustReturFPS '''" + GVR.Cells(2).Text + "'''"
                        Session("ReportFile") = ".../../../Rpt/FormCustReturFaktur.frx"
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
                btnAddDt_Click(Nothing, Nothing)
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
        dr = ViewState("Dt").Select("RRNo+'|'+SJNo+'|'+Product = " + QuotedStr(GVR.Cells(1).Text + "|" + GVR.Cells(2).Text + "|" + GVR.Cells(3).Text))
        For Each r In dr
            r.Delete()
        Next
        BindGridDt(ViewState("Dt"), GridDt)
        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
        If GetCountRecord(ViewState("Dt")) = 0 Then
            'tbPONo.Text = ""
        End If
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            ViewState("DtValue") = GVR.Cells(1).Text + "|" + GVR.Cells(2).Text + "|" + GVR.Cells(3).Text
            FillTextBoxDt(ViewState("DtValue"))
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            btnSaveDt.Focus()
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbCurr_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCurr.Click
        Try
            ViewState("InputCurrency") = "Y"
            AttachScript("OpenMaster('MsCurrency')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb Currency Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbCust_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCust.Click
        Try
            AttachScript("OpenMaster('MsCustomer')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb Customer Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub lbTerm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbTerm.Click
        Try
            AttachScript("OpenMaster('MsTerm')();", Page, Me.GetType())
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
                    BaseForex += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "AmountForex"))
                    DiscForex += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DiscForex"))
                ElseIf e.Row.RowType = DataControlRowType.Footer Then
                    If tbPriceTax.Text = "N" Then
                        tbBaseForex.Text = FormatNumber(BaseForex, CInt(ViewState("DigitCurr")))
                    Else
                        tbBaseForex.Text = FormatNumber(((BaseForex - DiscForex) * 100 / (100 + CFloat(tbPPN.Text))) + DiscForex, CInt(ViewState("DigitCurr")))
                    End If
                    tbDiscForex.Text = FormatNumber(DiscForex, ViewState("DigitCurr"))
                    tbPPNForex.Text = FormatFloat((CFloat(tbBaseForex.Text) - CFloat(tbDiscForex.Text)) * CFloat(tbPPN.Text) / 100, ViewState("DigitCurr"))
                    tbTotalForex.Text = FormatFloat(CFloat(tbBaseForex.Text) - CFloat(tbDiscForex.Text) + CFloat(tbPPNForex.Text), ViewState("DigitCurr"))
                End If
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            tbCode.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            'BindToDropList(ddlReport, Dt.Rows(0)("FgReport").ToString)
            BindToText(tbCustCode, Dt.Rows(0)("Customer").ToString)
            BindToText(tbCustName, Dt.Rows(0)("Customer_Name").ToString)
            BindToText(tbBillToCode, Dt.Rows(0)("BillTo").ToString)
            BindToText(tbBillToName, Dt.Rows(0)("Bill_To_Name").ToString)
            BindToText(tbAttn, Dt.Rows(0)("Attn").ToString)
            BindToDropList(ddlTerm, Dt.Rows(0)("Term").ToString)
            BindToDate(tbDueDate, Dt.Rows(0)("Duedate").ToString)
            BindToText(tbPPnNo, Dt.Rows(0)("PPnNo").ToString)
            BindToDate(tbPPndate, Dt.Rows(0)("PPndate").ToString)
            BindToText(tbPpnRate, Dt.Rows(0)("PPnRate").ToString)
            BindToText(tbPriceTax, Dt.Rows(0)("FgPriceTax").ToString)
            BindToDropList(ddlCurr, Dt.Rows(0)("Currency").ToString)
            BindToText(tbRate, Dt.Rows(0)("ForexRate").ToString)
            BindToText(tbBaseForex, Dt.Rows(0)("BaseForex").ToString)
            BindToText(tbPPN, Dt.Rows(0)("PPn").ToString)
            BindToText(tbPPNForex, Dt.Rows(0)("PPnForex").ToString)
            BindToText(tbTotalForex, Dt.Rows(0)("TotalForex").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
            ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, ViewState("DBConnection").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal Product As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("RRNo+'|'+SJNo+'|'+Product = " + QuotedStr(Product))
            If Dr.Length > 0 Then
                

                BindToText(tbBPBNo, Dr(0)("RRNo").ToString)
                BindToText(tbSJNo, Dr(0)("SJNo").ToString)
                BindToText(tbProductCode, Dr(0)("Product").ToString)
                BindToText(tbProductName, Dr(0)("Product_Name").ToString)
                BindToText(tbQty, Dr(0)("Qty").ToString)
                BindToText(tbUnit, Dr(0)("Unit").ToString)
                BindToText(tbQtyM2, Dr(0)("QtyM2").ToString)
                BindToText(tbQtyRoll, Dr(0)("QtyRoll").ToString)
                'BindToDropList(ddlPriceUnit, Dr(0)("PriceInUnit").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
                BindToText(tbPriceForex, FormatFloat(Dr(0)("PriceForex").ToString, ViewState("DigitCurr")))
                BindToText(tbAmountForex, FormatFloat(Dr(0)("AmountForex").ToString, ViewState("DigitCurr")))
                BindToText(tbDiscDt, Dr(0)("Disc").ToString)
                BindToText(tbDiscDtForex, FormatFloat(Dr(0)("DiscForex").ToString, ViewState("DigitCurr")))
                BindToText(tbNettoForex, FormatFloat(Dr(0)("NettoForex").ToString, ViewState("DigitCurr")))
                'ddlCurr_SelectedIndexChanged(Nothing, Nothing)
                AttachScript("setformatdt();", Page, Me.GetType())
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub

    Private Sub StatusButtonSave(ByVal Bool As Boolean)
        btnSaveAll.Visible = Bool
        btnSaveTrans.Visible = Bool
        btnBack.Visible = Bool
        btnGetData.Visible = Bool
    End Sub

    'Protected Sub ddlReport_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlReport.SelectedIndexChanged
    '    ChangeReport("Edit", ddlReport.SelectedValue, ddlCurr.SelectedValue = Session("Currency"), tbDate, tbRate, tbPPnNo, tbPPndate, tbPpnRate)
    '    'FillCombo(ddlBankReceipt, "EXEC S_GetBankReceipt " + QuotedStr(ddlReport.SelectedValue), False, "Bank_Code", "Bank_Name", Session("DBConnection"))
    'End Sub

    Protected Sub btnGetData_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetData.Click

        Dim ResultField, ResultSame, Filter As String
        Dim CriteriaField As String
        Try
            Session("Result") = Nothing
            Filter = ""
            
            If ViewState("StateHd") = "Edit" Then
                Session("Filter") = "EXEC S_FNCustReturGetRRRetur 'Y'," + QuotedStr(tbCode.Text) + " , " + QuotedStr(tbCustCode.Text) 'ddlReport.SelectedValue
            Else
                Session("Filter") = "EXEC S_FNCustReturGetRRRetur '', '', " + QuotedStr(tbCustCode.Text)
            End If
            ResultField = "RR_No, Customer_Code, Customer_Name, Report, Currency, Term, Remark,PriceIncludeTax," + _
                          "SJNo, Product, Product_Name, Qty,Unit, QtyM2,QtyRoll, PriceForex, AmountForex, PPnNo, PPnDate, PPNRate, Rate, Disc, DiscForex, SJCustDate "
            CriteriaField = "RR_No, RR_Date, Customer_Code, Customer_Name, Report, Currency, Term, PPnNo, Remark"
            Session("ClickSame") = "RR_No"
            Session("Column") = ResultField.Split(",")
            Session("CriteriaField") = CriteriaField.Split(",")
            ResultSame = "RR_No"
            Session("ResultSame") = ResultSame.Split(",")
            ViewState("Sender") = "btnGetDt"
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn get Dt Error : " + ex.ToString
        End Try


        'Dim ResultField, CriteriaField As String
        'Dim ResultSame As String
        'Try
        '    Session("Result") = Nothing
        '    Session("filter") = "EXEC S_FNCustReturGetRRRetur '','' "
        '    ResultField = "RR_No, Customer_Code, Customer_Name, Report, Currency, Term, Remark,PriceIncludeTax "
        '    CriteriaField = "RR_No, RR_Date, Customer_Code, Customer_Name, Report, Currency, Term, Remark"
        '    Session("ClickSame") = "RR_No"
        '    Session("Column") = ResultField.Split(",")
        '    Session("CriteriaField") = CriteriaField.Split(",")
        '    ViewState("Sender") = "btnGetDt"
        '    ResultSame = "RR_No"
        '    Session("ResultSame") = ResultSame.Split(",")
        '    ViewState("Sender") = "btnGetDt"
        '    Session("DBConnection") = ViewState("DBConnection")
        '    AttachScript("OpenSearchMultiDlg2();", Page, Me.GetType())

        'Catch ex As Exception
        '    lbStatus.Text = "btn get Dt Error : " + ex.ToString
        'End Try
    End Sub

    Protected Sub ddlTerm_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTerm.SelectedIndexChanged
        tbDueDate.SelectedDate = FindDueDate(ddlTerm.SelectedValue, tbDate.SelectedDate, ViewState("DBConnection"))
    End Sub

    Protected Sub tbDate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbDate.SelectionChanged
        tbDueDate.SelectedDate = FindDueDate(ddlTerm.SelectedValue, tbDate.SelectedDate, ViewState("DBConnection"))
    End Sub

    Protected Sub btnBillTo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBillTo.Click
        Dim ResultField As String
        Try
            Session("filter") = "select Bill_To, Bill_To_Name, Attn from VMsCustBillTo WHere CustCode = " + QuotedStr(tbCustCode.Text)
            Session("DBConnection") = ViewState("DBConnection")
            ResultField = "Bill_To, Bill_To_Name, Attn"
            ViewState("Sender") = "btnBillTo"
            Session("Column") = ResultField.Split(",")
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

    'Protected Sub btnBPBNo_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnBPBNo.Click
    '    Dim ResultField As String
    '    Try

    '        'session("filter") = " EXEC S_FNCustReturGetRRRetur " + "," + QuotedStr(tbFactory.Text) + "," + QuotedStr(tbBPBNo.Text)
    '        ResultField = "Product_Code, Product_Name, Specification, Qty, Unit, Price, Total "
    '        ViewState("Sender") = "btnBPBNo"
    '        Session("Column") = ResultField.Split(",")
    '        AttachScript("OpenSearchDlg();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lbStatus.Text = "btn BPBNo Error : " + ex.ToString
    '    End Try
    'End Sub

    Private Function cekValue(ByVal val As String) As String
        If val.Trim = "" Then
            Return "0"
        Else
            Return val
        End If
    End Function

    Private Sub SumNettoForex()
        Dim dr As DataRow
        Dim BaseForex As Double = 0
        Dim DiscForex As Double = 0
        For Each dr In ViewState("Dt").Rows
            If Not dr.RowState = DataRowState.Deleted Then
                BaseForex += CFloat(dr("AmountForex").ToString)
                DiscForex += CFloat(dr("DiscForex").ToString)
            End If
        Next
        If tbPriceTax.Text = "N" Then
            tbBaseForex.Text = FormatNumber(BaseForex, CInt(ViewState("DigitCurr")))
        Else
            tbBaseForex.Text = FormatNumber(((BaseForex - DiscForex) * 100 / (100 + CFloat(tbPPN.Text))) + DiscForex, CInt(ViewState("DigitCurr")))
        End If
        tbDiscForex.Text = FormatNumber(DiscForex, ViewState("DigitCurr"))
        tbPPNForex.Text = FormatFloat((CFloat(tbBaseForex.Text) - CFloat(tbDiscForex.Text)) * CFloat(tbPPN.Text) / 100, ViewState("DigitCurr"))
        tbTotalForex.Text = FormatFloat(CFloat(tbBaseForex.Text) - CFloat(tbDiscForex.Text) + CFloat(tbPPNForex.Text), ViewState("DigitCurr"))
    End Sub
    'Protected Sub ddlUnit_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPriceUnit.SelectedIndexChanged
    '    tbPrice_TextChanged(Nothing, Nothing)
    'End Sub

    Protected Sub tbPrice_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPriceForex.TextChanged
        Try
            'If ddlPriceUnit.SelectedValue = "M2" Then
            '    tbAmountForex.Text = FormatFloat(CFloat(tbQtyM2.Text) * CFloat(tbPriceForex.Text), ViewState("DigitCurr"))
            'ElseIf ddlPriceUnit.SelectedValue = "Roll" Then
            '    tbAmountForex.Text = FormatFloat(CFloat(tbQtyRoll.Text) * CFloat(tbPriceForex.Text), ViewState("DigitCurr"))
            'Else
            tbAmountForex.Text = FormatFloat(CFloat(tbQty.Text) * CFloat(tbPriceForex.Text), ViewState("DigitCurr"))
            'End If
            tbPriceForex.Text = FormatFloat(tbPriceForex.Text, ViewState("DigitCurr"))
            tbNettoForex.Text = FormatFloat(CFloat(tbAmountForex.Text) - CFloat(tbDiscDtForex.Text), ViewState("DigitCurr"))
            tbRemarkDt.Focus()
        Catch ex As Exception
            Throw New Exception("tbPriceForexDt_TextChanged Error : " + ex.ToString)
        End Try

    End Sub
    
    Protected Sub tbDiscDt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbDiscDt.TextChanged
        Try
            tbDiscDtForex.Text = FormatFloat((CFloat(tbDiscDt.Text) / 100) * CFloat(tbAmountForex.Text), ViewState("DigitCurr"))
            tbNettoForex.Text = FormatFloat((CFloat(tbQty.Text) * CFloat(tbPriceForex.Text)) - CFloat(tbDiscDtForex.Text), ViewState("DigitCurr"))

        Catch ex As Exception
            Throw New Exception("tbDisc_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbDiscDtForex_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbDiscDtForex.TextChanged
        Try
            ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, ViewState("DBConnection").ToString)
            tbDiscDt.Text = FormatFloat((CFloat(tbDiscDtForex.Text) / CFloat(tbAmountForex.Text)) * 100, 4)
            tbNettoForex.Text = FormatFloat((CFloat(tbQty.Text) * CFloat(tbPriceForex.Text)) - CFloat(tbDiscDtForex.Text), ViewState("DigitCurr"))

        Catch ex As Exception
            Throw New Exception("tbDisc_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbPPN_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPPN.TextChanged
        Try
            tbPPNForex.Text = FormatFloat((CFloat(tbBaseForex.Text) - CFloat(tbDiscForex.Text)) * CFloat(tbPPN.Text) / 100, ViewState("DigitCurr"))
            tbTotalForex.Text = FormatFloat(CFloat(tbBaseForex.Text) - CFloat(tbDiscForex.Text) + CFloat(tbPPNForex.Text), ViewState("DigitCurr"))
        Catch ex As Exception
            Throw New Exception("tbPPN_TextChanged Error : " + ex.ToString)
        End Try
    End Sub
End Class