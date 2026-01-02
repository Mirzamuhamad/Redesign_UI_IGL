Imports System.Data
Imports System.Data.SqlClient
'Imports System.Data.Odbc
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc


Partial Class TrAPRate
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select * From V_FNAPRateHd "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Try
            If Not IsPostBack Then
                InitProperty()
                FillCombo(ddlCostCtr, "EXEC S_GetCostCtr", True, "Cost_Ctr_Code", "Cost_Ctr_Name", ViewState("DBConnection"))
                FillCombo(ddlCurr, "EXEC S_GetCurrencyForRate", False, "Currency", "Currency", ViewState("DBConnection"))
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
                If ViewState("Sender") = "btnInvNo" Then
                    'ResultField = "Invoice_No, Invoice_Date, Supplier, Supplier_Name, Currency, Forex_Rate, Supplier_Invoice, Invoice_Balance, FgValue"
                    BindToText(tbInvType, Session("Result")(0).ToString)
                    BindToText(tbInvNo, Session("Result")(1).ToString)
                    BindToDate(tbInvDate, Session("Result")(2).ToString)
                    BindToText(tbSuppCode, Session("Result")(3).ToString)
                    BindToText(tbSuppName, Session("Result")(4).ToString)
                    BindToText(tbForexRate, Session("Result")(6).ToString)
                    BindToText(tbSuppInv, Session("Result")(7).ToString)
                    BindToText(tbAmountForex, Session("Result")(8).ToString)
                    BindToText(tbFgValue, Session("Result")(9).ToString)
                    tbAmountHome.Text = FormatFloat(CFloat(tbForexRate.Text) * CFloat(tbAmountForex.Text), ViewState("DigitHome"))
                    tbNewAmountHome.Text = FormatFloat(CFloat(tbAmountForex.Text) * CFloat(tbRate.Text), ViewState("DigitHome"))
                    tbAdjust.Text = FormatFloat((CFloat(tbNewAmountHome.Text) - CFloat(tbAmountHome.Text)) * CFloat(tbFgValue.Text), ViewState("DigitHome"))
                End If
                If ViewState("Sender") = "btnGetDt" Then
                    Dim drResult As DataRow
                    If IsNothing(Session("Result")) Then
                        lbStatus.Text = MessageDlg("Session is empty")
                        Exit Sub
                    End If
                    For Each drResult In Session("Result").Rows
                        'insert
                        If CekExistData(ViewState("Dt"), "Type,InvoiceNo", drResult("Type").ToString + "|" + drResult("Invoice_No").ToString) Then
                            lbStatus.Text = MessageDlg(drResult("Type").ToString + " No " + QuotedStr(drResult("Invoice_No").ToString) + " has been already exist")
                            Exit Sub
                        End If
                        Dim dr As DataRow
                        dr = ViewState("Dt").NewRow
                        'ResultField = "Type, Invoice_No, Invoice_Date, Supplier, Supplier_Name, 
                        'Currency, Forex_Rate, Supplier_Invoice, Invoice_Balance, FgValue"
                        dr("Type") = drResult("Type")
                        dr("InvoiceNo") = drResult("Invoice_No")
                        dr("ForexRate") = drResult("Forex_Rate")
                        dr("AmountForex") = drResult("Invoice_Balance")
                        dr("AmountHome") = FormatFloat(CFloat(drResult("Forex_Rate").ToString) * CFloat(drResult("Invoice_Balance").ToString), ViewState("DigitHome"))
                        dr("NewAmountHome") = FormatFloat(CFloat(drResult("Invoice_Balance").ToString) * CFloat(tbRate.Text), ViewState("DigitHome"))
                        dr("AdjustHome") = FormatFloat((CFloat(dr("NewAmountHome").ToString) - CFloat(dr("AmountHome").ToString)) * drResult("FgValue"), ViewState("DigitHome"))
                        dr("FgValue") = drResult("FgValue")
                        dr("InvoiceDate") = drResult("Invoice_Date")
                        dr("Supplier") = drResult("Supplier")
                        dr("Supplier_Name") = drResult("Supplier_Name")
                        dr("SuppInvoice") = drResult("Supplier_Invoice")
                        ViewState("Dt").Rows.Add(dr)
                    Next
                    BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    'Session("ResultSame") = Nothing
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
        lbAmountHome.Text = "Amount (" + ViewState("Currency") + ")"
        lbNewAmountHome.Text = "New Amount (" + ViewState("Currency") + ")"
        lbAdjust.Text = "Adjust (" + ViewState("Currency") + ")"
        ViewState("SortExpression") = Nothing
        ViewState("DigitCurr") = 0
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
        If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then

        End If
        GridView1.PageSize = CInt(ViewState("PageSizeGrid"))
        'GridDt.PageSize = CInt(ViewState("PageSizeGrid"))
        tbNewAmountHome.Attributes.Add("Readonly", "true")
        tbAmountHome.Attributes.Add("Readonly", "true")
        tbAdjust.Attributes.Add("Readonly", "true")

        tbRate.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbRate.Attributes.Add("OnBlur", "setformat();")

    End Sub

    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue, ddlRange.SelectedValue)
            If AdvanceFilter.Length > 1 And StrFilter.Length > 1 Then
                StrFilter = StrFilter + " And " + AdvanceFilter
            ElseIf AdvanceFilter.Length > 1 And StrFilter.Length <= 1 Then
                StrFilter = AdvanceFilter
            End If
            DT = BindDataTransaction(GetStringHd, StrFilter, ViewState("DBConnection").ToString)
            If DT.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("No Data")
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
        Return "SELECT * From V_FNAPRateDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

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
            Status = CekStatus(ActionValue)

            ListSelectNmbr = ""
            GetListCommand(Status, GridView1, "3,2", ListSelectNmbr, Nmbr, lbStatus.Text)
            If ListSelectNmbr = "" Then Exit Sub
            For j = 0 To (Nmbr.Length - 1)
                If Nmbr(j) = "" Then
                    Exit For
                Else
                    Result = ExecSPCommandGo(ActionValue, "S_FNAPRate", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                    If Trim(Result) <> "" Then
                        lbStatus.Text = lbStatus.Text + Result + " <br/>"
                    End If
                End If
            Next
            If lbStatus.Text <> "" Then
                lbStatus.Text = MessageDlg(lbStatus.Text)
            End If
            BindData("TransNmbr in (" + ListSelectNmbr + ")")
        Catch ex As Exception
            lbStatus.Text = "Go Command Error : " + ex.ToString
        End Try
    End Sub

    Private Sub EnableHd(ByVal State As Boolean)
        Try
            ddlCostCtr.Enabled = State
            ddlReport.Enabled = State
            ddlCurr.Enabled = State
            tbDate.Enabled = State
            tbRate.Enabled = State
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
            GridDt.HeaderRow.Cells(6).Text = "Amount (" + ViewState("Currency") + ")"
            GridDt.HeaderRow.Cells(7).Text = "New Amount (" + ViewState("Currency") + ")"
            GridDt.HeaderRow.Cells(8).Text = "Adjust (" + ViewState("Currency") + ")"
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
            'EnableHd(GridDt.Rows.Count = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt.Click
        Dim Row As DataRow
        Try
            If ViewState("StateDt") = "Edit" Then
                If ViewState("DtValue") <> tbInvType.Text + "|" + tbInvNo.Text Then
                    If CekExistData(ViewState("Dt"), "Type, InvoiceNo", tbInvType.Text + "|" + tbInvNo.Text) Then
                        lbStatus.Text = MessageDlg(tbInvType.Text + " No " + QuotedStr(tbInvNo.Text) + " has been already exist")
                        Exit Sub
                    End If
                End If
                Row = ViewState("Dt").Select("Type+'|'+InvoiceNo= " + QuotedStr(ViewState("DtValue")))(0)
                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                Row("Type") = tbInvType.Text
                Row("InvoiceNo") = tbInvNo.Text
                Row("ForexRate") = tbForexRate.Text
                Row("AmountForex") = tbAmountForex.Text
                Row("AmountHome") = tbAmountHome.Text
                Row("NewAmountHome") = tbNewAmountHome.Text
                Row("AdjustHome") = tbAdjust.Text
                Row("FgValue") = tbFgValue.Text
                Row("InvoiceDate") = tbInvDate.SelectedValue
                Row("Supplier") = tbSuppCode.Text
                Row("Supplier_Name") = tbSuppName.Text
                Row("SuppInvoice") = tbSuppInv.Text
                Row.EndEdit()
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If

                If CekExistData(ViewState("Dt"), "Type,InvoiceNo", tbInvType.Text + "|" + tbInvNo.Text) = True Then
                    lbStatus.Text = MessageDlg(tbInvType.Text + " No " + QuotedStr(tbInvNo.Text) + " has been already exist")
                    Exit Sub
                End If

                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("Type") = tbInvType.Text
                dr("InvoiceNo") = tbInvNo.Text
                dr("ForexRate") = tbForexRate.Text
                dr("AmountForex") = tbAmountForex.Text
                dr("AmountHome") = tbAmountHome.Text
                dr("NewAmountHome") = tbNewAmountHome.Text
                dr("AdjustHome") = tbAdjust.Text
                dr("FgValue") = tbFgValue.Text
                dr("InvoiceDate") = tbInvDate.SelectedValue
                dr("Supplier") = tbSuppCode.Text
                dr("Supplier_Name") = tbSuppName.Text
                dr("SuppInvoice") = tbSuppInv.Text
                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            BindGridDt(ViewState("Dt"), GridDt)
            GridDt.HeaderRow.Cells(6).Text = "Amount (" + ViewState("Currency") + ")"
            GridDt.HeaderRow.Cells(7).Text = "New Amount (" + ViewState("Currency") + ")"
            GridDt.HeaderRow.Cells(8).Text = "Adjust (" + ViewState("Currency") + ")"
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
                tbReference.Text = GetAutoNmbr("APR", ddlReport.SelectedValue, Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
                SQLString = "INSERT INTO FINAPRateHd (TransNmbr, Status, TransDate, FgReport, CostCtr, " + _
                "Currency, NewRate, Remark, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbReference.Text) + ", 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', " + _
                QuotedStr(ddlReport.SelectedValue) + ", " + QuotedStr(ddlCostCtr.SelectedValue) + ", " + _
                QuotedStr(ddlCurr.SelectedValue) + "," + tbRate.Text.Replace(",", "") + "," + _
                QuotedStr(tbRemark.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", getDate()"
            Else
                Dim CekStatus As String
                CekStatus = SQLExecuteScalar("Select Status FROM FINAPRateHd WHERE TransNmbr = " + QuotedStr(tbReference.Text), ViewState("DBConnection").ToString)
                If CekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE FINAPRateHd SET TransDate = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "' " + _
                ", CostCtr = " + QuotedStr(ddlCostCtr.SelectedValue) + _
                ", Currency = " + QuotedStr(ddlCurr.SelectedValue) + ", NewRate = " + tbRate.Text.Replace(",", "") + _
                ", Remark = " + QuotedStr(tbRemark.Text) + _
                ", UserPrep = " + QuotedStr(ViewState("UserId").ToString) + ", DatePrep = getDate()" + _
                " WHERE TransNmbr = " + QuotedStr(tbReference.Text) + ""
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
                Row(I)("TransNmbr") = tbReference.Text
                Row(I).EndEdit()
            Next

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString 'System.Configuration.ConfigurationManager.AppSettings.Get("ConString")
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, Type, InvoiceNo, ForexRate, AmountForex, AmountHome, NewAmountHome, FgValue FROM FINAPRateDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt As New DataTable("FINAPRateDt")

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
            tbFilter.Text = tbReference.Text
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
            btnHome.Visible = False
            tbDate.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ClearHd()
            Cleardt()
            pnlDt.Visible = True
            BindDataDt("")
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbReference.Text = ""
            tbDate.SelectedDate = ViewState("ServerDate") 'Today
            ddlCostCtr.SelectedIndex = 0
            ddlCurr.SelectedIndex = 0
            ViewState("DigitCurr") = 0
            ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
            tbRemark.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbInvType.Text = ""
            tbInvNo.Text = ""
            tbInvDate.Clear()
            tbSuppCode.Text = ""
            tbSuppName.Text = ""
            tbSuppInv.Text = ""
            tbForexRate.Text = "0"
            tbAmountForex.Text = "0"
            tbAmountHome.Text = "0"
            tbNewAmountHome.Text = "0"
            tbAdjust.Text = "0"
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
            ModifyInput2(True, pnlInput, pnlDt, GridDt)
            newTrans()
        Catch ex As Exception
            lbStatus.Text = "btn save all error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdddt.Click, btnAddDt2.Click
        Cleardt()
        If CekHd() = False Then
            Exit Sub
        End If
        ViewState("StateDt") = "Insert"
        MovePanel(pnlDt, pnlEditDt)
        EnableHd(False)
        StatusButtonSave(False)
        btnInvNo.Focus()
    End Sub

    Function CekHd() As Boolean
        Try
            If tbDate.IsNull Then
                lbStatus.Text = MessageDlg("Date must have value")
                tbDate.Focus()
                Return False
            End If
            'If CInt(Session(Request.QueryString("KeyId"))("Year")) <> Year(tbDate.SelectedValue) Or CInt(Session(Request.QueryString("KeyId"))("Period")) <> Month(tbDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(Session(Request.QueryString("KeyId"))("Period")) + " " + Session(Request.QueryString("KeyId"))("Year").ToString.Trim)
            '    Return False
            'End If
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
            If ddlCostCtr.SelectedValue.Trim = "" Then
                lbStatus.Text = MessageDlg("CostCtr must have value")
                ddlCostCtr.Focus()
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
                If Dr("InvoiceNo").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Invoice No Must Have Value")
                    Return False
                End If
            Else
                If tbInvNo.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Invoice No Must Have Value")
                    tbInvNo.Focus()
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
            FDateName = "Date"
            FDateValue = "TransDate"
            FilterName = "Reference, Date, Status, Cost Center, Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), Status, CostCtr, Remark"
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
                    ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, ViewState("DBConnection").ToString)
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
                        FillTextBoxHd(ViewState("TransNmbr"))
                        ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, ViewState("DBConnection").ToString)
                        BindDataDt(ViewState("TransNmbr"))
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
                        Session("SelectCommand") = "EXEC S_FNFormAPRate " + QuotedStr(GVR.Cells(2).Text)
                        Session("ReportFile") = ".../../../Rpt/FormFNAPRate.frx"
                        Session("DBConnection") = ViewState("DBConnection")
                        AttachScript("openprintdlg();", Page, Me.GetType)
                    Catch ex As Exception
                        lbStatus.Text = "btn print Error = " + ex.ToString
                    End Try
                ElseIf DDL.SelectedValue = "Delete" Then
                    CekMenu = CheckMenuLevel("Delete", ViewState("MenuLevel").Rows(0))
                    If CekMenu <> "" Then
                        lbStatus.Text = CekMenu
                        Exit Sub
                    End If
                    'Code For Deleting
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
    Dim AmountF As Decimal = 0
    Dim AmountH As Decimal = 0
    Dim NewAmountH As Decimal = 0
    Dim Adjust As Decimal = 0
    Protected Sub GridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt.RowDataBound
        Try
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "InvoiceNo")) Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                    AmountF += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "AmountForex"))
                    AmountH += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "AmountHome"))
                    NewAmountH += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "NewAmountHome"))
                    Adjust += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "AdjustHome"))
                ElseIf e.Row.RowType = DataControlRowType.Footer Then
                    e.Row.Cells(5).Text = "Total : "
                    e.Row.Cells(6).Text = FormatNumber(AmountF, ViewState("DigitCurr"))
                    e.Row.Cells(7).Text = FormatNumber(AmountH, ViewState("DigitHome"))
                    e.Row.Cells(8).Text = FormatNumber(NewAmountH, ViewState("DigitHome"))
                    e.Row.Cells(9).Text = FormatNumber(Adjust, ViewState("DigitHome"))
                End If
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Dim dr() As DataRow
        Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
        dr = ViewState("Dt").Select("InvoiceNo=" + QuotedStr(GVR.Cells(2).Text))
        dr(0).Delete()
        BindGridDt(ViewState("Dt"), GridDt)
        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            ViewState("DtValue") = GVR.Cells(1).Text + "|" + GVR.Cells(2).Text
            FillTextBoxDt(ViewState("DtValue"))
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            btnInvNo.Focus()
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            tbReference.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToDropList(ddlReport, Dt.Rows(0)("FgReport").ToString)
            BindToDropList(ddlCostCtr, Dt.Rows(0)("CostCtr").ToString)
            BindToDropList(ddlCurr, Dt.Rows(0)("Currency").ToString)
            BindToText(tbRate, Dt.Rows(0)("NewRate").ToString, ViewState("DigitRate"))
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal InvoiceNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("Type+'|'+InvoiceNo= " + QuotedStr(InvoiceNo))
            If Dr.Length > 0 Then
                BindToText(tbInvType, Dr(0)("Type").ToString)
                BindToText(tbInvNo, Dr(0)("InvoiceNo").ToString)
                BindToDate(tbInvDate, Dr(0)("InvoiceDate").ToString)
                BindToText(tbSuppCode, Dr(0)("Supplier").ToString)
                BindToText(tbSuppName, Dr(0)("Supplier_Name").ToString)
                BindToText(tbForexRate, Dr(0)("ForexRate").ToString)
                BindToText(tbSuppInv, Dr(0)("SuppInvoice").ToString)
                BindToText(tbAmountForex, Dr(0)("AmountForex").ToString)
                BindToText(tbAmountHome, Dr(0)("AmountHome").ToString)
                BindToText(tbNewAmountHome, Dr(0)("NewAmountHome").ToString)
                BindToText(tbAdjust, Dr(0)("AdjustHome").ToString)
                BindToText(tbFgValue, Dr(0)("FgValue").ToString)
            End If
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
        If ViewState("InputCurrency") = "Y" Then
            RefreshMaster("S_GetCurrencyForRate", "Currency", "Currency", ddlCurr, ViewState("DBConnection"))
            ViewState("InputCurrency") = Nothing
        End If
        ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
        AttachScript("setformat();", Page, Me.GetType())
        tbRate.Focus()
    End Sub

    

    

    Protected Sub btnInvNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInvNo.Click
        Dim ResultField As String
        Try
            Session("Filter") = "SELECT * FROM V_FNAPForRate WHERE Currency = " + QuotedStr(ddlCurr.SelectedValue) + _
            " AND Report = " + QuotedStr(ddlReport.SelectedValue) + _
            " AND CONVERT(FLOAT,REPLACE(Forex_Rate,',','')) <> " + (Format(tbRate.Text)).Replace(",", "")
            ResultField = "Type, Invoice_No, Invoice_Date, Supplier, Supplier_Name, Currency, Forex_Rate, Supplier_Invoice, Invoice_Balance, FgValue"
            ViewState("Sender") = "btnInvNo"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search InvNo Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnGetDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetDt.Click
        Dim ResultField As String 'ResultSame 
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            Session("Result") = Nothing
            Session("Filter") = "SELECT Type, Invoice_No, Invoice_Date, Supplier, Supplier_Name, Supplier_Invoice, Supplier_PO_No, " + _
                "PONo, Term, Due_Date, Currency, Forex_Rate, Invoice_Forex, Invoice_Paid, Invoice_Balance, FgValue FROM V_FNAPForRate WHERE Currency = " + QuotedStr(ddlCurr.SelectedValue) + _
            " AND InvoiceDate <= " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + " AND Report = " + QuotedStr(ddlReport.SelectedValue) + _
            " AND CONVERT(FLOAT,REPLACE(Forex_Rate,',','')) <> " + (Format(tbRate.Text)).Replace(",", "")
            ResultField = "Type, Invoice_No, Invoice_Date, Supplier, Supplier_Name, Currency, Forex_Rate, Supplier_Invoice, Invoice_Balance, FgValue"
            Session("Column") = ResultField.Split(",")
            ViewState("Sender") = "btnGetDt"
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn get Dt Error : " + ex.ToString
        End Try
    End Sub
End Class
