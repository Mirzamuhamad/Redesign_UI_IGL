Imports System.Data
Imports System.Data.SqlClient
'Imports System.Data.Odbc
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc


Partial Class JEntry
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select * From V_GLJournalHD WHERE TransClass = 'JE'"

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

            ' hasil dari search dialog
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnAcc" Then
                    tbAccCode.Text = Session("Result")(0).ToString
                    tbAccName.Text = Session("Result")(1).ToString
                    tbfgSubled.Text = Session("Result")(2).ToString
                    BindToDropList(ddlCurr, Session("Result")(3))
                    tbFgCostCtr.Text = Session("Result")(4).ToString
                    ddlCostCtr.Enabled = tbFgCostCtr.Text <> "N"
                    ViewState("FgType") = Session("Result")(5).ToString
                    If tbFgCostCtr.Text = "N" Then
                        ddlCostCtr.SelectedValue = ""
                    End If
                    If ViewState("FgType") = "BS" Then
                        ddlCurr.Enabled = False
                    Else : ddlCurr.Enabled = True
                    End If
                    tbSubled.Enabled = tbfgSubled.Text <> "N"
                    btnSubled.Visible = tbSubled.Enabled
                    ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"), ViewState("DigitHome"), ViewState("DigitRate"))
                    If tbfgSubled.Text = "N" Then
                        tbSubled.Text = ""
                        tbSubledName.Text = ""
                    End If
                    tbDebitHome.Text = FormatNumber(CFloat(tbDebitForex.Text) * CFloat(tbRate.Text), ViewState("DigitHome"))
                    tbCreditHome.Text = FormatNumber(CFloat(tbCreditForex.Text) * CFloat(tbRate.Text), ViewState("DigitHome"))
                End If
                If ViewState("Sender") = "btnSubled" Then
                    tbSubled.Text = Session("Result")(0).ToString
                    tbSubledName.Text = Session("Result")(1).ToString
                End If
                If ViewState("Sender") = "btnGetDt" Then
                    Dim drResult As DataRow
                    Dim MaxItem As String
                    If IsNothing(Session("Result")) Then
                        lbStatus.Text = "Session is empty"
                        Exit Sub
                    End If
                    For Each drResult In Session("Result").Rows
                        MaxItem = GetNewItemNo(ViewState("Dt"))
                        'insert
                        Dim dr As DataRow
                        dr = ViewState("Dt").NewRow
                        dr("ItemNo") = MaxItem
                        dr("Account") = drResult("Account")
                        dr("AccountName") = drResult("Description")
                        dr("Subled") = ""
                        dr("Costctr") = ""
                        dr("FgSubled") = drResult("FgSubled")
                        dr("FgCostCtr") = drResult("FgCostCtr")
                        dr("Remark") = ""
                        dr("Currency") = drResult("Currency")
                        dr("ForexRate") = GetCurrRate(drResult("Currency").ToString, tbDate.SelectedDate, ViewState("DBConnection"))
                        dr("DebitForex") = 0
                        dr("DebitHome") = 0
                        dr("CreditForex") = 0
                        dr("CreditHome") = 0
                        dr("TransClass") = "JE"
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
        FillCombo(ddlJE, "EXEC S_GetFormatJE", True, "Format_Code", "Format_Name", ViewState("DBConnection"))
        FillCombo(ddlCostCtr, "EXEC S_GetCostCtr", True, "Cost_Ctr_Code", "Cost_Ctr_Name", ViewState("DBConnection"))
        FillCombo(ddlCurr, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
        GridView1.PageSize = CInt(ViewState("PageSizeGrid"))
        GridDt.PageSize = CInt(ViewState("PageSizeGrid"))
        ViewState("SortExpression") = Nothing
        tbFgCostCtr.Text = ""
        ViewState("DigitCurr") = 0
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
        If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
            ddlCommand.Items.Add("Print")
            ddlCommand2.Items.Add("Print")
        End If
        tbDebitHome.Attributes.Add("ReadOnly", "True")
        tbCreditHome.Attributes.Add("ReadOnly", "True")
        Me.tbNominal.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbRate.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbDebitForex.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbCreditForex.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbRate.Attributes.Add("OnBlur", "RatexDCForex(" + Me.tbRate.ClientID + "," + Me.tbDebitForex.ClientID + "," + Me.tbCreditForex.ClientID + "," + Me.tbDebitHome.ClientID + "," + Me.tbCreditHome.ClientID + "); setformat();")
        Me.tbDebitForex.Attributes.Add("OnBlur", "RatexDCForex(" + Me.tbRate.ClientID + "," + Me.tbDebitForex.ClientID + "," + Me.tbCreditForex.ClientID + "," + Me.tbDebitHome.ClientID + "," + Me.tbCreditHome.ClientID + "); setformat();")
        Me.tbCreditForex.Attributes.Add("OnBlur", "RatexDCForex(" + Me.tbRate.ClientID + "," + Me.tbDebitForex.ClientID + "," + Me.tbCreditForex.ClientID + "," + Me.tbDebitHome.ClientID + "," + Me.tbCreditHome.ClientID + "); setformat();")
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
            'lbStatus.Text = ddlRange.SelectedValue + " " + StrFilter
            'Exit Sub
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
            If DT.Rows.Count > 0 Then
                GridView1.HeaderRow.Cells(8).Text = "Debit (" + ViewState("Currency") + ")"
                GridView1.HeaderRow.Cells(9).Text = "Credit (" + ViewState("Currency") + ")"
            End If
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT * From V_GLJournalDt WHERE Reference = " + QuotedStr(Nmbr) + " AND TransClass = 'JE' "
    End Function

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
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
                        ListSelectNmbr = GVR.Cells(2).Text + "|JE"
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
                Session("SelectCommand") = "EXEC S_GLFormJournal " + Result + " , 'JE'," + QuotedStr(ViewState("UserId").ToString)
                'lbStatus.Text = Session("SelectCommand")
                'Exit Sub
                Session("ReportFile") = ".../../../Rpt/FormJournal.frx"
                AttachScript("openprintdlg();", Page, Me.GetType)
            Else
                Status = CekStatus(ActionValue)
                ListSelectNmbr = ""
                GetListCommand(Status, GridView1, "3,2,6", ListSelectNmbr, Nmbr, lbStatus.Text)
                If ListSelectNmbr = "" Then Exit Sub
                For j = 0 To (Nmbr.Length - 1)
                    If Nmbr(j) = "" Then
                        Exit For
                    Else
                        Result = ExecSPCommandGo(ActionValue, "S_GLJournal", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                        If Trim(Result) <> "" Then
                            lbStatus.Text = lbStatus.Text + Result + " <br/>"
                        End If
                    End If
                Next
                BindData("Reference+'|'+TransClass in (" + ListSelectNmbr + ")")
            End If
        Catch ex As Exception
            lbStatus.Text = "Go Command Error : " + ex.ToString
        End Try
    End Sub

    Private Sub EnableHd(ByVal State As Boolean)
        Try
            BtnGetDt.Visible = State
            tbRef.Enabled = State
            ddlJE.Enabled = State
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

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBack.Click, btnHome.Click
        Try
            MovePanel(pnlInput, PnlHd)
        Catch ex As Exception
            lbStatus.Text = "btn back Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancelDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelDt.Click
        Try
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            'EnableHd(GridDt.Rows.Count = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveDt.Click
        Dim Row As DataRow
        Try
            If ViewState("StateDt") = "Edit" Then
                Row = ViewState("Dt").Select("ItemNo = " + lbItemNo.Text)(0)
                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                Row("Account") = tbAccCode.Text
                Row("AccountName") = tbAccName.Text
                Row("Subled") = tbSubled.Text
                Row("Subled_Name") = tbSubledName.Text()
                Row("CostCtr") = ddlCostCtr.SelectedValue
                Row("FgSubled") = tbfgSubled.Text
                Row("Remark") = tbRemarkDt.Text
                If Row("CostCtr") = "" Then
                    Row("Costctr") = DBNull.Value
                End If
                Row("Currency") = ddlCurr.SelectedValue
                If Row("Currency") = "" Then
                    Row("Currency") = DBNull.Value
                End If
                Row("ForexRate") = tbRate.Text
                Row("DebitForex") = tbDebitForex.Text
                Row("DebitHome") = tbDebitHome.Text
                Row("CreditForex") = tbCreditForex.Text
                Row("CreditHome") = tbCreditHome.Text
                Row.EndEdit()
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("ItemNo") = lbItemNo.Text
                dr("TransClass") = "JE"
                dr("Account") = tbAccCode.Text
                dr("AccountName") = tbAccName.Text
                dr("Subled") = tbSubled.Text
                dr("Subled_Name") = tbSubledName.Text()
                dr("CostCtr") = ddlCostCtr.SelectedValue
                If dr("CostCtr") = "" Then
                    dr("Costctr") = DBNull.Value
                End If
                dr("FgSubled") = tbfgSubled.Text
                dr("Remark") = tbRemarkDt.Text
                dr("Currency") = ddlCurr.SelectedValue
                If dr("Currency") = "" Then
                    dr("Currency") = DBNull.Value
                End If
                dr("ForexRate") = tbRate.Text
                dr("DebitForex") = tbDebitForex.Text
                dr("DebitHome") = tbDebitHome.Text
                dr("CreditForex") = tbCreditForex.Text
                dr("CreditHome") = tbCreditHome.Text
                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
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
                tbRef.Text = GetAutoNmbr("JE", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO GLJournalHd (Reference, TransClass, Status, TransDate, FormatJE, Remark, UserPrep, DatePrep, FgReport) " + _
                "SELECT '" + tbRef.Text + "', 'JE', 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', '" + ddlJE.SelectedValue + "', " + QuotedStr(tbRemark.Text) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", getDate(), " + QuotedStr(ddlReport.SelectedValue)
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM GLJOURNALHD WHERE Reference = '" + tbRef.Text + "' AND TransClass = 'JE'", ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE GLJOURNALHD SET FormatJE = '" + ddlJE.SelectedValue + "', Remark = " + QuotedStr(tbRemark.Text) + "," + _
                " TransDate = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', DateAppr = getDate()" + ", FgReport = " + QuotedStr(ddlReport.SelectedValue) + _
                " WHERE Reference = '" + tbRef.Text + "' AND TransClass = 'JE'"
            End If
            SQLString = SQLString.Replace("'1900-01-01'", "NULL")
            SQLString = SQLString.Replace("'0001-01-01'", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            'update Primary Key on Dt
            Dim Row As DataRow()

            Row = ViewState("Dt").Select("Reference IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("Reference") = tbRef.Text
                Row(I)("TransClass") = "JE"
                Row(I).EndEdit()
            Next

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString 'System.Configuration.ConfigurationManager.AppSettings.Get("ConString")
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT Reference, TransClass, ItemNo, Account, FgSubled, Subled, CostCtr, Currency, ForexRate, DebitForex, DebitHome, CreditForex, CreditHome, Remark FROM GLJournalDt WHERE Reference = '" & ViewState("Reference") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt As New DataTable("GLJournalDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt
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
            'If IsNothing(ViewState("Dt")) Then
            '    lbStatus.Text = MessageDlg("Detail must have at least 1 record")
            '    Exit Sub
            'End If
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
            tbFilter.Text = tbRef.Text
            ddlField.SelectedValue = "Reference"
            btnSearch_Click(Nothing, Nothing)
            tbFilter.Text = CurrFilter
            ddlField.SelectedValue = Value
        Catch ex As Exception
            lbStatus.Text = "Save All Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, pnlDt, GridDt)
            newTrans()
            btnHome.Visible = False
            tbNominal.Visible = True
            BtnFormatJE.Visible = True
            ddlReport.Enabled = True
            tbDate.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ViewState("DigitCurr") = 0
            tbDate.SelectedDate = ViewState("ServerDate") 'Today
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
            tbRef.Text = ""
            tbRemark.Text = ""
            ddlJE.SelectedValue = ""
            tbNominal.Enabled = False
            tbNominal.Text = "0"
            ddlReport.SelectedValue = "Y"
            'tbDate.Clear()
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            lbItemNo.Text = ""
            tbAccCode.Text = ""
            tbAccName.Text = ""
            tbSubled.Text = ""
            tbSubledName.Text = ""
            tbRemarkDt.Text = ""
            ddlCostCtr.SelectedIndex = 0
            tbCreditForex.Text = "0"
            tbCreditHome.Text = "0"
            tbDebitForex.Text = "0"
            tbDebitHome.Text = "0"
            ddlCurr.SelectedValue = ViewState("Currency")
            tbRate.Text = FormatNumber(1, ViewState("DigitRate"))
            ViewState("DigitCurr") = ViewState("DigitHome")
            'ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"), ViewState("DigitHome"), ViewState("DigitRate"))
            btnSubled.Visible = False
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
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

    Protected Sub btnSaveAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveAll.Click
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

    Protected Sub btnAcc_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAcc.Click
        Dim ResultField As String
        Try
            Session("filter") = "select Account, Description, Currency, Class_Account, Sub_Group_Account, Group_Account, FgSubLed, FgType, FgCostCtr from V_MsAccountJE WHERE (Type = '*JE*' OR Type = " + QuotedStr(ddlJE.SelectedValue) + ") "
            ResultField = "Account, Description, FgSubled, Currency, FgCostCtr, FgType"
            ViewState("Sender") = "btnAcc"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Acc Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccCode.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("AccountJE", tbAccCode.Text.Trim + "|" + ddlJE.SelectedValue, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbAccCode.Text = TrimStr(Dr("Account").ToString)
                tbAccName.Text = TrimStr(Dr("AccountName").ToString)
                BindToDropList(ddlCurr, Dr("Currency"))
                ddlCurr_SelectedIndexChanged(Nothing, Nothing)
                tbfgSubled.Text = TrimStr(Dr("FgSubled").ToString)
                tbFgCostCtr.Text = TrimStr(Dr("FgCostCtr").ToString)
                ViewState("FgType") = TrimStr(Dr("FgType").ToString)
                If tbFgCostCtr.Text = "N" Then
                    ddlCostCtr.SelectedValue = ""
                    ddlCostCtr.Enabled = False
                Else : ddlCostCtr.Enabled = True
                End If
                If ViewState("FgType") = "BS" Then
                    ddlCurr.Enabled = False
                Else : ddlCurr.Enabled = True
                End If
            Else
                tbAccCode.Text = ""
                tbAccName.Text = ""
                tbfgSubled.Text = "N"
                tbFgCostCtr.Text = "N"
                If tbFgCostCtr.Text = "N" Then
                    ddlCostCtr.SelectedValue = ""
                    ddlCostCtr.Enabled = False
                Else : ddlCostCtr.Enabled = True
                End If
                ddlCurr.Enabled = False
            End If
            ChangeFgSubLed(tbfgSubled, tbSubled, btnSubled)
            tbDebitForex.Text = FormatNumber(CFloat(tbDebitForex.Text) * CFloat(tbRate.Text), ViewState("DigitCurr"))
            tbCreditHome.Text = FormatNumber(CFloat(tbCreditForex.Text) * CFloat(tbRate.Text), ViewState("DigitCurr"))
            tbAccCode.Focus()
        Catch ex As Exception
            Throw New Exception("tb acc Code Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlCurr_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCurr.SelectedIndexChanged
        If ViewState("InputCurrency") = "Y" Then
            RefreshMaster("S_GetCurrency", "Currency", "Currency", ddlCurr, ViewState("DBConnection"))
            ViewState("InputCurrency") = Nothing
        End If
        ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"), ViewState("DigitHome"), ViewState("DigitRate"))
        tbRate.Focus()
        AttachScript("RatexDCForex(" + Me.tbRate.ClientID + "," + Me.tbDebitForex.ClientID + "," + Me.tbCreditForex.ClientID + "," + Me.tbDebitHome.ClientID + "," + Me.tbCreditHome.ClientID + "); setformat();", Page, Me.GetType())
    End Sub

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddDt.Click, btnAddDt2.Click
        Cleardt()
        If CekHd() = False Then
            Exit Sub
        End If
        ViewState("StateDt") = "Insert"
        lbItemNo.Text = GetNewItemNo(ViewState("Dt"))
        MovePanel(pnlDt, pnlEditDt)
        EnableHd(False)
        StatusButtonSave(False)
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
                If Dr("Account").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Account Must Have Value")
                    Return False
                End If
                If Dr("FgSubled").ToString <> "N" And Dr("Subled").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Subled Must Be Filled for FgSubled other than N")
                    Return False
                End If
                If (Dr("CostCtr").ToString = "") And (Dr("FgCostCtr").ToString = "Y") Then
                    lbStatus.Text = MessageDlg("Account " + Dr("Account").ToString + " must have value Cost Ctr")
                    Return False
                End If

                If Dr("Currency").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Currency must have value")
                    Return False
                End If
                If CFloat(Dr("ForexRate").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Rate must have value")
                    Return False
                End If
                If CFloat(Dr("ForexRate").ToString) = 1 And Dr("Currency").ToString <> ViewState("Currency") Then
                    lbStatus.Text = MessageDlg("Rate must have value")
                    Return False
                End If
                If CFloat(Dr("DebitForex").ToString) = 0 And CFloat(Dr("CreditForex").ToString) = 0 Then
                    lbStatus.Text = MessageDlg("Debit or Credit Forex must have value")
                    Return False
                End If
            Else
                If tbAccCode.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Account Must Have Value")
                    tbAccCode.Focus()
                    Return False
                End If
                If tbfgSubled.Text.Trim <> "N" And tbSubled.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Subled Must Be Filled for FgSubled other than N")
                    tbSubled.Focus()
                    Return False
                End If
                If (ddlCostCtr.SelectedValue.Trim = "") And (tbFgCostCtr.Text = "Y") Then
                    lbStatus.Text = MessageDlg("Cost Center must have value")
                    Return False
                End If
                If ddlCurr.SelectedValue.Trim = "" Then
                    lbStatus.Text = MessageDlg("Currency must have value")
                    ddlCurr.Focus()
                    Return False
                End If
                If CFloat(tbRate.Text) = 0 Then
                    lbStatus.Text = MessageDlg("Rate must have value")
                    tbRate.Focus()
                    Return False
                End If
                If CFloat(tbDebitForex.Text) = 0 And CFloat(tbCreditForex.Text) = 0 Then
                    lbStatus.Text = MessageDlg("Debit or Credit Forex must have value")
                    tbDebitForex.Focus()
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub ddlJE_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlJE.SelectedIndexChanged
        Try
            If ViewState("InputFormatJE") = "Y" Then
                RefreshMaster("S_GetFormatJE", "Format_Code", "Format_Name", ddlJE, ViewState("DBConnection"))
                ViewState("InputFormatJE") = Nothing
            End If
            Dim Type As String
            Type = SQLExecuteScalar("Select Type FROM VMsFormatJE WHERE Format_Code = " + QuotedStr(ddlJE.SelectedValue), ViewState("DBConnection").ToString)
            If Type = "Percentage" Then
                tbNominal.Text = "0"
                tbNominal.Enabled = True
            Else
                tbNominal.Text = "0"
                tbNominal.Enabled = False
            End If
        Catch ex As Exception
            lbStatus.Text = "ddl JE Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        CheckAll(GridView1, sender)
        'Dim cb, cbselek As CheckBox
        'Dim GRW As GridViewRow
        'Try
        '    cb = sender
        '    For Each GRW In GridView1.Rows
        '        cbselek = GRW.FindControl("cbSelect")
        '        cbselek.Checked = cb.Checked
        '    Next
        'Catch ex As Exception
        '    lbStatus.Text = "cb Hd Checked Changed Error : " + ex.ToString
        'End Try
    End Sub

    Protected Sub LbAdvSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LbAdvSearch.Click
        Dim FDateName, FDateValue, FilterName, FilterValue As String
        Try
            FDateName = "Date"
            FDateValue = "TransDate"
            FilterName = "Reference, Status, Format Journal, Remark"
            FilterValue = "Reference, Status, Format_Name, Remark"
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
                    tbNominal.Visible = False
                    BtnFormatJE.Visible = False
                    ddlReport.Enabled = False
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
                        ViewState("Reference") = GVR.Cells(2).Text
                        GridDt.PageIndex = 0
                        BindDataDt(ViewState("Reference"))
                        FillTextBoxHd(ViewState("Reference"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        tbNominal.Visible = True
                        BtnFormatJE.Visible = True
                        btnHome.Visible = False
                        ddlReport.Enabled = False
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
                        Session("SelectCommand") = "EXEC S_GLFormJournal ''" + QuotedStr(GVR.Cells(2).Text + "|JE") + "'', 'JE'," + QuotedStr(ViewState("UserId").ToString)
                        Session("ReportFile") = ".../../../Rpt/FormJournal.frx"
                        AttachScript("openprintdlg();", Page, Me.GetType)
                    Catch ex As Exception
                        lbStatus.Text = "Print Error : " + ex.ToString
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
        'Dim ds As DataSet
        'Dim i As Integer
        Try
            If e.CommandName = "Insert" Then
                btnAddDt_Click(Nothing, Nothing)
                BtnGetDt.Enabled = False
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Dim dr() As DataRow
        Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
        dr = ViewState("Dt").Select("ItemNo = " + GVR.Cells(1).Text)
        dr(0).Delete()
        BindGridDt(ViewState("Dt"), GridDt)
        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            FillTextBoxDt(GVR.Cells(1).Text)
            'ViewState("DigitCurr") = CInt(SQLExecuteScalar("Select Digit FROM VMsCurrency WHERE Currency = " + QuotedStr(ddlCurr.SelectedValue)))
            'pnlEditDt.Visible = True
            'pnlDt.Visible = False
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            tbAccCode.Focus()
            BtnGetDt.Enabled = False
            tbSubled.Enabled = tbfgSubled.Text <> "N"
            btnSubled.Enabled = tbSubled.Enabled
            ddlCostCtr.Enabled = tbFgCostCtr.Text <> "N"
            'tbAccCode_TextChanged(Nothing, Nothing)
            StatusButtonSave(False)
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

    Protected Sub lbCostCtr_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCostCtr.Click
        Try
            ViewState("InputCostCtr") = "Y"
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsCostCtr')();", Page, Me.GetType())
            'If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
            '    Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenMaster('MsCostCtr')();", True)
            'End If
        Catch ex As Exception
            lbStatus.Text = "lb Cost Ctr Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlCostCtr_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCostCtr.SelectedIndexChanged
        If ViewState("InputCostCtr") = "Y" Then
            RefreshMaster("S_GetCostCtr", "Cost_Ctr_Code", "Cost_Ctr_Name", ddlCostCtr, ViewState("DBConnection"))
            ViewState("InputCostCtr") = Nothing
        End If
    End Sub

    Protected Sub lbAccount_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAccount.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsAccount')();", Page, Me.GetType())
            'If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
            '    Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenMsAccount();", True)
            'End If
        Catch ex As Exception
            lbStatus.Text = "lb Account Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbFormatJE_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbFormatJE.Click
        Try
            ViewState("InputFormatJE") = "Y"
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsFormatJE')();", Page, Me.GetType())
            'If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
            '    Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenMsFormatJE();", True)
            'End If
        Catch ex As Exception
            lbStatus.Text = "lb Format JE Error : " + ex.ToString
        End Try
    End Sub



    Dim CrHome As Decimal = 0
    Dim DbHome As Decimal = 0
    ' untuk tampilkan data total di grid
    Protected Sub GridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt.RowDataBound
        Try
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "ItemNo")) Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                    '' add the UnitPrice and QuantityTotal to the running total variables
                    'CrHome += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CreditHome"))
                    ''CrForex += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CreditForex"))
                    'DbHome += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DebitHome"))
                    ''DbForex += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DebitForex"))
                ElseIf e.Row.RowType = DataControlRowType.Footer Then
                    DbHome = GetTotalSum(ViewState("Dt"), "DebitHome")
                    CrHome = GetTotalSum(ViewState("Dt"), "CreditHome")
                    'e.Row.Cells(7).Text = "Total:"
                    ' for the Footer, display the running totals
                    'e.Row.Cells(8).Text = FormatNumber(DbHome, ViewState("DigitHome"))
                    'e.Row.Cells(10).Text = FormatNumber(CrHome, ViewState("DigitHome"))
                End If
                tbDebit.Text = FormatNumber(DbHome, ViewState("DigitHome"))
                tbCredit.Text = FormatNumber(CrHome, ViewState("DigitHome"))
                tbSelisih.Text = FormatNumber(DbHome - CrHome, ViewState("DigitHome"))
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnGetDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnGetDt.Click
        Dim ResultField As String 'ResultSame 
        Try
            Session("Result") = Nothing
            Session("Filter") = "select * from V_MsAccountDt WHERE TransType = 'JE' "
            ResultField = "Account, Description, Currency, FgSubled, FgCostCtr"
            Session("Column") = ResultField.Split(",")
            'ResultSame = "Currency"
            'Session("ResultSame") = ResultSame.Split(",")
            ViewState("Sender") = "btnGetDt"
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn get Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSubled_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSubled.Click
        Dim ResultField As String
        Try
            Session("filter") = "select Subled_No, Subled_Name from VMsSubled WHERE FgSubled = '" + tbfgSubled.Text + "'"
            ResultField = "SubLed_No, SubLed_Name"
            ViewState("Sender") = "btnSubled"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Subled Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Dim Type As String
        Try
            Dt = BindDataTransaction(GetStringHd, "Reference = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            'newTrans()
            tbRef.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToDropList(ddlJE, Dt.Rows(0)("FormatJE").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
            BindToDropList(ddlReport, Dt.Rows(0)("FgReport").ToString)
            Type = SQLExecuteScalar("Select Type FROM VMsFormatJE WHERE Format_Code = " + QuotedStr(ddlJE.SelectedValue), ViewState("DBConnection").ToString)
            If Type = "Percentage" Then
                tbNominal.Text = "0"
                tbNominal.Enabled = True
            Else
                tbNominal.Text = "0"
                tbNominal.Enabled = False
            End If
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("ItemNo = " + ItemNo)
            If Dr.Length > 0 Then
                lbItemNo.Text = Dr(0)("ItemNo").ToString
                BindToText(tbAccCode, Dr(0)("Account").ToString)
                BindToText(tbAccName, Dr(0)("Accountname").ToString)
                BindToText(tbfgSubled, Dr(0)("FgSubLed").ToString)
                BindToText(tbfgcostCtr, Dr(0)("FgCostCtr").ToString)
                BindToText(tbSubled, Dr(0)("SubLed").ToString)
                BindToText(tbSubledName, Dr(0)("Subled_Name").ToString)
                BindToDropList(ddlCostCtr, Dr(0)("CostCtr").ToString)
                BindToDropList(ddlCurr, Dr(0)("Currency").ToString)
                BindToText(tbRate, Dr(0)("ForexRate").ToString)
                BindToText(tbDebitForex, Dr(0)("DebitForex").ToString)
                BindToText(tbCreditForex, Dr(0)("CreditForex").ToString)
                BindToText(tbDebitHome, Dr(0)("DebitHome").ToString)
                BindToText(tbCreditHome, Dr(0)("CreditHome").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
                ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"), ViewState("DigitHome"), ViewState("DigitRate"))
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

    Protected Sub BtnFormatJE_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnFormatJE.Click
        Dim JEDS As DataSet
        Dim NewRow As DataRow
        Dim ExistRow As DataRow()
        Dim CountRow, I As Integer
        Try
            'If tbRef.Text.Trim = "" Then
            '    Exit Sub
            'End If

            JEDS = SQLExecuteQuery("EXEC S_GLJournalGetFormatJE '" + ddlJE.SelectedValue + "', " + tbNominal.Text.ToString, ViewState("DBConnection").ToString)
            If JEDS.Tables(0).Rows.Count > 0 Then
                ExistRow = ViewState("Dt").Select("ItemNo > 0")
                CountRow = GetCountRecord(ViewState("Dt"))
                CountRow = CountRow - 1
                If CountRow >= 0 Then
                    I = 0
                    For I = 0 To CountRow
                        ExistRow(I).Delete()
                    Next
                End If
            End If
            For Each dr In JEDS.Tables(0).Rows
                NewRow = ViewState("Dt").NewRow
                NewRow("ItemNo") = dr("ItemNo") 'GetNewItemNo(ViewState("Dt"))
                NewRow("Account") = dr("Account")
                NewRow("AccountName") = dr("Description")
                NewRow("FgSubled") = dr("FgSubled")
                NewRow("Subled") = TrimStr(dr("Subled").ToString)
                NewRow("Costctr") = ""
                NewRow("Remark") = ""
                NewRow("Currency") = dr("Currency")
                ViewState("DigitCurr") = CInt(SQLExecuteScalar("Select Digit FROM VMsCurrency WHERE Currency = " + QuotedStr(dr("Currency").ToString), ViewState("DBConnection").ToString))
                NewRow("ForexRate") = GetCurrRate(dr("Currency").ToString, tbDate.SelectedDate, ViewState("DBConnection"))
                NewRow("DebitForex") = FormatNumber(dr("Debit"), ViewState("DigitCurr").ToString)
                NewRow("DebitHome") = FormatNumber(dr("Debit") * GetCurrRate(dr("Currency").ToString, tbDate.SelectedDate, ViewState("DBConnection")), ViewState("DigitHome").ToString)
                NewRow("CreditForex") = FormatNumber(dr("Credit"), ViewState("DigitCurr").ToString)
                NewRow("CreditHome") = FormatNumber(dr("Credit") * GetCurrRate(dr("Currency").ToString, tbDate.SelectedDate, ViewState("DBConnection")), ViewState("DigitHome").ToString)

                ViewState("Dt").Rows.Add(NewRow)
            Next
            BindGridDt(ViewState("Dt"), GridDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
        Catch ex As Exception
            lbStatus.Text = "btn JE Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbSubled_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbSubled.TextChanged
        Dim Dr As DataRow
        Try
            If tbSubled.Text.Trim = "" Or tbfgSubled.Text = "N" Then
                tbSubled.Text = ""
                tbSubledName.Text = ""
                Exit Sub
            End If
            Dr = FindMaster("Subled", tbfgSubled.Text + "|" + tbSubled.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbSubled.Text = Dr("SubLed_No")
                tbSubledName.Text = Dr("SubLed_Name")
            Else
                tbSubled.Text = ""
                tbSubledName.Text = ""
            End If
        Catch ex As Exception
            Throw New Exception("tb Subled change Error : " + ex.ToString)
        End Try
    End Sub
End Class
