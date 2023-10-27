Imports System.Data
Imports System.Data.SqlClient
'Imports System.Data.Odbc
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc

Partial Class Transaction_TrPYRemuneration_TrPYRemuneration
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "SELECT distinct TransNmbr, Nmbr, Transdate, Status, EffectiveDate, Remark, UserId FROM V_PYRemunerationHd "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                ViewState("SetGrade") = False
                FillCombo(ddlCurrency, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
                FillCombo(ddlFormula, "EXEC S_GetFormula", True, "FormulaCode", "FormulaName", ViewState("DBConnection"))
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

                If ViewState("Sender") = "btnGetDt" Then
                    Dim drResult As DataRow
                    If IsNothing(Session("Result")) Then
                        lbStatus.Text = "Session is empty"
                        Exit Sub
                    End If
                    For Each drResult In Session("Result").Rows
                        'insert
                        If Not CekExistData(ViewState("Dt"), "Payroll,ItemCode", drResult("Payroll").ToString + "|" + drResult("ItemCode").ToString) Then
                            Dim dr As DataRow
                            dr = ViewState("Dt").NewRow
                            dr("Payroll") = drResult("Payroll")
                            dr("PayrollName") = drResult("PayrollName")
                            dr("Type") = drResult("Type")
                            dr("ItemCode") = drResult("ItemCode")
                            dr("ItemName") = drResult("ItemName")
                            dr("LastEffectiveDate") = drResult("StartDate")
                            dr("Currency") = drResult("Currency")
                            dr("Formula") = drResult("Formula")
                            dr("FormulaName") = drResult("FormulaName")
                            dr("CurrentAmount") = drResult("Amount")
                            dr("NewAmount") = drResult("Amount")
                            dr("PercAmount") = "0"
                            dr("AdjustAmount") = "0"
                            dr("Remark") = ""
                            ViewState("Dt").Rows.Add(dr)
                        End If
                    Next
                    BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    GridDt.Columns(1).Visible = True
                    'Session("ResultSame") = Nothing
                End If
                If ViewState("Sender") = "btnPayroll" Then
                    '"PayrollCode, PayrollName, GroupBy, Formula, Amount"
                    tbPayroll.Text = Session("Result")(0).ToString
                    tbPayrollName.Text = Session("Result")(1).ToString
                    tbType.Text = Session("Result")(2).ToString
                    ddlFormula.SelectedValue = Session("Result")(3).ToString
                    tbCurrentAmount.Text = FormatFloat(Session("Result")(4).ToString, -1)
                    tbAdjustPercent.Text = FormatFloat(0.ToString, -1)
                    tbAdjustAmount.Text = FormatFloat(0, -1)
                    tbNewAmount.Text = FormatFloat(Session("Result")(4).ToString, -1)
                End If
                If ViewState("Sender") = "btnItem" Then
                    Dim dtSlip As DataTable
                    tbItemCode.Text = Session("Result")(0).ToString
                    tbItemName.Text = Session("Result")(1).ToString

                    dtSlip = SQLExecuteQuery("Select Currency, StartDate, Amount, Formula from VMsPayrollSlip WHERE EndDate = '20500101' AND Payroll = " + QuotedStr(tbPayroll.Text) + " AND ItemCode = " + QuotedStr(tbItemCode.Text), ViewState("DBConnection").ToString).Tables(0)
                    If dtSlip.Rows.Count > 0 Then
                        ddlCurrency.SelectedValue = dtSlip.Rows(0)("Currency")
                        tbLastEffective.SelectedValue = dtSlip.Rows(0)("StartDate")
                        ddlFormula.SelectedValue = dtSlip.Rows(0)("Formula")
                        tbCurrentAmount.Text = FormatFloat(dtSlip.Rows(0)("Amount"), -1)
                        tbNewAmount.Text = FormatFloat(dtSlip.Rows(0)("Amount"), -1)
                    Else
                        tbLastEffective.SelectedValue = Nothing
                        tbCurrentAmount.Text = "0"
                        tbNewAmount.Text = "0"
                    End If
                    tbAdjustAmount.Text = "0"
                    tbAdjustPercent.Text = "0"
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
        'ViewState("DigitCurr") = 0
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
        If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then

        End If
        'tbRef.Attributes.Add("ReadOnly", "True")
        tbCurrentAmount.Attributes.Add("ReadOnly", "True")
        tbPayrollName.Attributes.Add("ReadOnly", "True")
        Me.tbCurrentAmount.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbNewAmount.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbAdjustPercent.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbAdjPercent.Attributes.Add("OnKeyDown", "return PressNumericMinus();")
        Me.tbAdjustAmount.Attributes.Add("OnKeyDown", "return PressNumericMinus();")
        Me.tbNewAmount.Attributes.Add("OnBlur", "setformat('new');")
        Me.tbAdjustPercent.Attributes.Add("OnBlur", "setformat('percent');")
        Me.tbAdjustAmount.Attributes.Add("OnBlur", "setformat('adjust');")
    End Sub

    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim StrFilter As String
        Try
            If AdvanceFilter.Length > 1 Then
                StrFilter = " AND UserId =" + QuotedStr(ViewState("UserId")) + AdvanceFilter
            Else
                StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue, ddlRange.SelectedValue) 
            End If
		
            DT = BindDataTransaction(GetStringHd, StrFilter , ViewState("DBConnection").ToString)
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
        Return "SELECT * FROM V_PYRemunerationDt WHERE TransNmbr = " + QuotedStr(Nmbr) + " "
    End Function

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
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
                    Result = ExecSPCommandGo(ActionValue, "S_PYRemuneration", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                    If Trim(Result) <> "" Then
                        lbStatus.Text = lbStatus.Text + Result + " <br/>"

                    End If
                End If
            Next
            BindData("TransNmbr in (" + ListSelectNmbr + ")")
        Catch ex As Exception
            lbStatus.Text = "Go Command Error : " + ex.ToString
        End Try
    End Sub

    Private Sub EnableHd(ByVal State As Boolean)
        Try
            'btnGetDt.Visible = State
            'tbRef.Enabled = State
            tbEffectiveDate.Enabled = State
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
                tbRef.Text = GetAutoNmbr("PRM", "N", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
                SQLString = "INSERT INTO PYRemunerationHd (TransNmbr, Status, TransDate, EffectiveDate, Remark, UserPrep, DatePrep) " + _
                "SELECT '" + tbRef.Text + "', 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', '" + Format(tbEffectiveDate.SelectedValue, "yyyy-MM-dd") + "', '" + tbRemark.Text + "'," + _
                QuotedStr(ViewState("UserId").ToString) + ", GetDate()"
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("SELECT Status FROM PYRemunerationHd WHERE TransNmbr = " + QuotedStr(tbRef.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE PYRemunerationHd SET EffectiveDate = '" + Format(tbEffectiveDate.SelectedValue, "yyyy-MM-dd") + "'," + _
                " Remark = '" + tbRemark.Text + "', TransDate = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', UserPrep = " + QuotedStr(ViewState("UserId").ToString) + ", DatePrep = getDate()" + _
                " WHERE TransNmbr = '" + tbRef.Text + "'"
            End If
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            'update Primary Key on Dt
            Dim Row As DataRow()

            Row = ViewState("Dt").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("Transnmbr") = tbRef.Text
                'Row(I)("TransClass") = "JE"
                Row(I).EndEdit()
            Next

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString 'System.Configuration.ConfigurationManager.AppSettings.Get("ConString")
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, Payroll, ItemCode, Type, LastEffectiveDate, Currency, CurrentAmount, PercAmount, AdjustAmount, NewAmount, Formula, Remark FROM PYRemunerationDt WHERE TransNmbr = '" & ViewState("Reference") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt As New DataTable("MKTPriceDt")

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
            EnableHd(True)
            btnHome.Visible = False
            Panel1.Visible = True
            tbDate.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ViewState("Add") = "Clear"
            'ViewState("DigitCurr") = 0
            tbAdjPercent.Text = "0"
            ClearHd()
            Cleardt()
            btnGetDt.Visible = True
            tbDate.SelectedDate = ViewState("ServerDate") 'Today
            tbEffectiveDate.SelectedDate = ViewState("ServerDate") 'Today
            'ViewState("DigitCurr") = GetCurrDigit(ddlCurrency.SelectedValue, ViewState("DBConnection").ToString)
            pnlDt.Visible = True
            BindDataDt("")
            GridDt.Columns(1).Visible = False
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbRef.Text = ""
            tbRemark.Text = ""
            tbEffectiveDate.Clear()
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbPayroll.Text = ""
            tbPayrollName.Text = ""
            tbType.Text = ""
            tbItemCode.Text = ""
            tbItemName.Text = ""
            ddlCurrency.SelectedValue = ViewState("Currency")
            ddlFormula.SelectedValue = ""
            tbCurrentAmount.Text = "0"
            tbAdjustPercent.Text = "0"
            tbAdjustAmount.Text = "0"
            tbNewAmount.Text = "0"
            tbRemarkDt.Text = ""
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
            newTrans()
        Catch ex As Exception
            lbStatus.Text = "btn save all error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt.Click, btnAddDt2.Click
        If CekHd() = False Then
            Exit Sub
        End If
        Cleardt()
        ViewState("StateDt") = "Insert"
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
            If tbEffectiveDate.IsNull Then
                lbStatus.Text = MessageDlg("Effective Date must have value")
                tbEffectiveDate.Focus()
                Return False
            End If
            If tbEffectiveDate.SelectedDate < tbDate.SelectedDate Then
                lbStatus.Text = MessageDlg("Effective Date must greater than Transaction Date")
                tbEffectiveDate.Focus()
                Return False
            End If
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
                If Dr("Payroll").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Payroll Must Have Value")
                    Return False
                End If
                If Dr("ItemCode").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Item Must Have Value")
                    Return False
                End If
                If Dr("Currency").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Currency Must Have Value")
                    Return False
                End If
                If Dr("Formula").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Formula Must Have Value")
                    Return False
                End If
                If CFloat(Dr("NewAmount").ToString.Trim) < "0" Then
                    lbStatus.Text = MessageDlg("New Amount Must Have Value")
                    Return False
                End If
            Else
                If tbPayroll.ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Payroll Must Have Value")
                    tbPayroll.Focus()
                    Return False
                End If
                If tbItemCode.ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Item Must Have Value")
                    tbItemCode.Focus()
                    Return False
                End If
                If ddlFormula.SelectedValue.Trim = "" Then
                    lbStatus.Text = MessageDlg("Formula Must Have Value")
                    ddlFormula.Focus()
                    Return False
                End If
                If ddlCurrency.SelectedValue.Trim = "" Then
                    lbStatus.Text = MessageDlg("Currency Must Have Value")
                    ddlCurrency.Focus()
                    Return False
                End If
                If CFloat(tbNewAmount.Text.Trim) < "0" Then
                    lbStatus.Text = MessageDlg("New Amount Must Have Value")
                    tbNewAmount.Focus()
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
            FilterName = "Reference, Effective Date, Payroll, Item Name, Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(EffectiveDate), PayrollName, ItemName, Remark"
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
                    GridDt.Columns(1).Visible = False
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, pnlDt, GridDt)
                    Panel1.Visible = False
                    btnHome.Visible = True
                    btnGetDt.Visible = False
                    'Dim Dr As DataRow
                    'Dr = FindMaster("Rate", ddlCurrency.SelectedValue + "|" + Format(tbDate.SelectedDate, "yyyy-MM-dd"), Session("DBConnection").ToString)
                    'If Not Dr Is Nothing Then
                    '    ViewState("DigitCurr") = Dr("digit")
                    '    AttachScript("setformat();", Page, Me.GetType())
                    'End If
                ElseIf DDL.SelectedValue = "Edit" Then
                    If GVR.Cells(3).Text = "H" Or GVR.Cells(3).Text = "G" Then
                        CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        MovePanel(PnlHd, pnlInput)
                        Panel1.Visible = True
                        ViewState("Reference") = GVR.Cells(2).Text
                        GridDt.PageIndex = 0
                        BindDataDt(ViewState("Reference"))
                        FillTextBoxHd(ViewState("Reference"))
                        GridDt.Columns(1).Visible = True
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        btnHome.Visible = False
                        btnGetDt.Visible = True
                        tbAdjPercent.Text = "0"
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                        'ViewState("DigitCurr") = GetCurrDigit(ddlCurrency.SelectedValue, ViewState("DBConnection").ToString)
                    Else
                        lbStatus.Text = MessageDlg("Data must Hold or Get Approval to edit")
                        Exit Sub
                    End If
                ElseIf DDL.SelectedValue = "Print" Then
                    Try
                        Session("DBCOnnection") = ViewState("DBConnection")
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        Session("SelectCommand") = "EXEC S_MKFormPrice " + QuotedStr(GVR.Cells(2).Text)
                        Session("ReportFile") = ".../../../Rpt/FormMKPrice.frx"
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
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)

            dr = ViewState("Dt").Select("Payroll+'|'+ItemCode = " + QuotedStr(GVR.Cells(2).Text + "|" + GVR.Cells(4).Text))
            dr(0).Delete()
            'ViewState("Dt").AcceptChanges()
            BindGridDt(ViewState("Dt"), GridDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            ViewState("DtValue") = GVR.Cells(2).Text + "|" + GVR.Cells(4).Text
            FillTextBoxDt(ViewState("DtValue"))
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            'newTrans()
            tbRef.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToDate(tbEffectiveDate, Dt.Rows(0)("EffectiveDate").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal Payroll As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("Payroll+'|'+ItemCode = " + QuotedStr(Payroll))
            If Dr.Length > 0 Then
                BindToText(tbPayroll, Dr(0)("Payroll").ToString)
                BindToText(tbPayrollName, Dr(0)("PayrollName").ToString)
                BindToText(tbType, Dr(0)("Type").ToString)
                BindToText(tbItemCode, Dr(0)("ItemCode").ToString)
                BindToText(tbItemName, Dr(0)("ItemName").ToString)
                BindToDate(tbLastEffective, Dr(0)("LastEffectiveDate").ToString)
                BindToDropList(ddlCurrency, Dr(0)("Currency").ToString)
                BindToDropList(ddlFormula, Dr(0)("Formula").ToString)
                'ViewState("DigitCurr") = GetCurrDigit(ddlCurrency.SelectedValue, ViewState("DBConnection").ToString)
                tbCurrentAmount.Text = FormatFloat(Dr(0)("CurrentAmount").ToString, -1)
                tbAdjustPercent.Text = FormatFloat(Dr(0)("PercAmount").ToString, -1)
                tbAdjustAmount.Text = FormatFloat(Dr(0)("AdjustAmount").ToString, -1)
                tbNewAmount.Text = FormatFloat(Dr(0)("NewAmount").ToString, -1)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)

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
    End Sub

    Protected Sub BtnGetDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetDt.Click
        Dim ResultField As String 'ResultSame 
        Try
            Session("DBConnection") = ViewState("DBConnection")
            If CekHd() = False Then
                Exit Sub
            End If
            Session("Result") = Nothing
            Session("Filter") = "Select A.Payroll, P.PayrollName, A.Type, A.ItemCode, A.ItemName, A.Currency, A.StartDate, dbo.FormatFloat(A.Amount,-1) AS Amount, A.Formula, F.FormulaName from VMsPayrollSlip A INNER JOIN MsPayroll P ON A.Payroll = P.PayrollCode INNER JOIN MsFormula F ON A.Formula = F.FormulaCode " + _
                " INNER JOIN V_MsMethodAccess M ON A.ItemCode = M.EmpNumb WHERE M.UserId = " + QuotedStr(ViewState("UserId").ToString) + _
		" AND A.EndDate = '20500101' and A.StartDate < " + QuotedStr(Format(tbEffectiveDate.SelectedValue, "yyyy-MM-dd"))
            ResultField = "Payroll, PayrollName, Type, ItemCode, ItemName, Currency, StartDate, Amount, Formula, FormulaName"
            Session("Column") = ResultField.Split(",")
            ViewState("Sender") = "btnGetDt"
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn get Dt Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub tbPrice_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPrice.SelectedIndexChanged
    '    'If ViewState("InputPayroll") = "Y" Then
    '    '    RefreshMaster("S_GetPayrollPrice", "Payroll_Price_Code", "Payroll_Price_Name", tbPrice, Session("DBConnection"))
    '    '    ViewState("InputPayroll") = Nothing
    '    'End If
    '    Dim dr As SqlDataReader
    '    dr = SQLExecuteReader("EXEC S_GLSalesPriceGetPrice " + QuotedStr(ddlClass.SelectedValue) + ", " + QuotedStr(ddlSize.SelectedValue) + ", " + QuotedStr(tbPrice.Text) + ", " + QuotedStr(ddlCustType.SelectedValue) + ", " + QuotedStr(ddlCurrency.SelectedValue) + ", " + QuotedStr(Format(tbEffectiveDate.SelectedDate, "yyyy-MM-dd")), Session("DBConnection"))
    '    dr.Read()
    '    tbOldPrice.Text = FormatNumber(dr("Price"), ViewState("DigitCurr"))
    '    tbNewPrice.Text = FormatNumber(dr("Price"), ViewState("DigitCurr"))
    '    dr.Close()
    '    'AttachScript("setformat();", Page, Me.GetType())
    '    tbNewPrice.Focus()
    'End Sub

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt.Click
        Dim Row As DataRow
        Try
            If ViewState("StateDt") = "Edit" Then
                If ViewState("DtValue") <> TrimStr(tbPayroll.Text + "|" + tbItemCode.Text) Then
                    If CekExistData(ViewState("Dt"), "Payroll,ItemCode", tbPayroll.Text + "|" + tbItemCode.Text) Then
                        lbStatus.Text = "Payroll '" + tbPayrollName.Text + "' Item '" + tbItemName.Text + "' has already exists"
                        Exit Sub
                    End If
                End If
                Row = ViewState("Dt").Select("Payroll+'|'+ItemCode = " + QuotedStr(ViewState("DtValue")))(0)
                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                'Row("PayrollPrice") = tbCode.Text
                Row("Payroll") = tbPayroll.Text
                Row("PayrollName") = tbPayrollName.Text
                Row("ItemCode") = tbItemCode.Text
                Row("ItemName") = tbItemName.Text
                Row("Type") = tbType.Text
                Row("Currency") = ddlCurrency.SelectedValue
                Row("Formula") = ddlFormula.SelectedValue
                Row("FormulaName") = ddlFormula.SelectedItem.Text
                Row("LastEffectiveDate") = tbLastEffective.SelectedValue
                Row("CurrentAmount") = tbCurrentAmount.Text
                Row("AdjustAmount") = tbAdjustAmount.Text
                Row("PercAmount") = tbAdjustPercent.Text
                Row("NewAmount") = tbNewAmount.Text
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()
                'ViewState("Dt").AcceptChanges()
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If
                If CekExistData(ViewState("Dt"), "Payroll,ItemCode", tbPayroll.Text + "|" + tbItemCode.Text) Then
                    lbStatus.Text = "Payroll '" + tbPayrollName.Text + "' Item '" + tbItemName.Text + "' has already exists"
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("Payroll") = tbPayroll.Text
                dr("PayrollName") = tbPayrollName.Text
                dr("ItemCode") = tbItemCode.Text
                dr("ItemName") = tbItemName.Text
                dr("Type") = tbType.Text
                dr("Currency") = ddlCurrency.SelectedValue
                dr("Formula") = ddlFormula.SelectedValue
                dr("FormulaName") = ddlFormula.SelectedItem.Text
                dr("LastEffectiveDate") = tbLastEffective.SelectedValue
                dr("CurrentAmount") = tbCurrentAmount.Text
                dr("AdjustAmount") = tbAdjustAmount.Text
                dr("PercAmount") = tbAdjustPercent.Text
                dr("NewAmount") = tbNewAmount.Text
                dr("Remark") = tbRemarkDt.Text
                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            BindGridDt(ViewState("Dt"), GridDt)
            StatusButtonSave(True)
            ViewState("Add") = "NotClear"
        Catch ex As Exception
            lbStatus.Text = "btn save Dt Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try

    End Sub

    Protected Sub btnCancelDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnCancelDt.Click
        Try
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            'EnableHd(GridDt.Rows.Count = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GetOldPrice()
        Dim dr1 As SqlDataReader
        Try
            dr1 = SQLExecuteReader("EXEC S_MKPriceGetDt " + QuotedStr(ddlCurrency.SelectedValue) + ", " + QuotedStr(Format(tbEffectiveDate.SelectedValue, "yyyy-MM-dd")), ViewState("DBConnection"))
            dr1.Read()
            tbCurrentAmount.Text = FormatNumber(dr1("OldPrice"), ViewState("DigitCurr"))
            tbNewAmount.Text = FormatNumber(dr1("NewPrice"), ViewState("DigitCurr"))
            dr1.Close()
            tbNewAmount.Focus()
            AttachScript("setformatdt();", Page, Me.GetType())
        Catch ex As Exception
            Throw New Exception("Get Price Error : " + ex.ToString)
        End Try
    End Sub

    Public Sub CheckAllDt(ByRef gd As GridView, ByVal sender As CheckBox)
        Dim cb, cbselek As CheckBox
        Dim GRW As GridViewRow
        Try
            cb = sender
            For Each GRW In gd.Rows
                cbselek = GRW.FindControl("cbSelect")
                cbselek.Checked = cb.Checked
                'If cb.Checked = False Then
                'btnGetSetZero.Visible = True
                'End If
            Next
        Catch ex As Exception
            Throw New Exception("Check All Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub cbSelectHd_CheckedChanged1(ByVal sender As Object, ByVal e As System.EventArgs)
        CheckAllDt(GridDt, sender)
    End Sub

    Private Sub bindDataSetCustType()
        Try
            Dim GVR As GridViewRow
            Dim CB As CheckBox

            Dim HaveSelect As Boolean
            Dim CekKey As String
            Dim dr As DataRow
            HaveSelect = False
            For Each GVR In GridDt.Rows
                CB = GVR.FindControl("cbSelect")
                'lbStatus.Text = "3 : " + GVR.Cells(3).Text + " 4 : " + GVR.Cells(4).Text + " 5 : " + GVR.Cells(5).Text + " 6 : " + GVR.Cells(6).Text + " 7 : " + GVR.Cells(7).Text + " 8 : " + GVR.Cells(8).Text + " 9 : " + GVR.Cells(9).Text + " 10 : " + GVR.Cells(10).Text + " 11 : " + GVR.Cells(11).Text + " 14 : " + GVR.Cells(14).Text + " 15 : " + GVR.Cells(15).Text + " 16 : " + GVR.Cells(16).Text
                'Exit Sub
                '3 : Life Style Centro 4 : 5 : Motif Seri A 6 : 7 : 40x40 8 : 9 : KW A 10 : 11 : All Motif 12 : 13 : PT
                If CB.Checked Then
                    CekKey = TrimStr(GVR.Cells(2).Text) + "|" + TrimStr(GVR.Cells(4).Text)
                    HaveSelect = True
                    If CekExistData(ViewState("Dt"), "Payroll,ItemCode", CekKey) Then
                        dr = ViewState("Dt").Select("Payroll+'|'+ItemCode = " + QuotedStr(CekKey))(0)
                        dr.BeginEdit()
                        dr("Payroll") = TrimStr(GVR.Cells(2).Text)
                        dr("PayrollName") = TrimStr(GVR.Cells(3).Text)
                        dr("ItemCode") = TrimStr(GVR.Cells(4).Text)
                        dr("ItemName") = TrimStr(GVR.Cells(5).Text)
                        If ddlAdjustType.SelectedValue = "Percentage" Then
                            dr("PercAmount") = FormatFloat(tbAdjPercent.Text, -1)
                            dr("AdjustAmount") = FormatFloat(CFloat(dr("CurrentAmount")) * CFloat(tbAdjPercent.Text) / 100, -1)
                        Else
                            dr("AdjustAmount") = FormatFloat(tbAdjPercent.Text, -1)
                            dr("PercAmount") = FormatFloat(CFloat(tbAdjPercent.Text) * 100 / CFloat(dr("CurrentAmount")), -1)
                        End If
                        dr("NewAmount") = FormatNumber(CFloat(dr("CurrentAmount")) + CFloat(dr("AdjustAmount")), -1)
                        dr.EndEdit()
                    End If
                End If
            Next
            If HaveSelect = False Then
                lbStatus.Text = "Please Check Payroll for Process"
                Exit Sub
            Else
                lbStatus.Text = "Set Adjust Success for Selected Payroll"
            End If
            BindGridDt(ViewState("Dt"), GridDt)
            'BindGridDtView()
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
        Catch ex As Exception
            Throw New Exception("bindDataGridCustType Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnProcess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcess.Click
        Try
            bindDataSetCustType()
        Catch ex As Exception
            Throw New Exception("btnProcess_Click Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub lbPayroll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbPayroll.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsPayroll')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb Payroll Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GetTypeChange()
        Try
            'If ddlAdjType.SelectedValue = "+" Then
            tbNewAmount.Text = CFloat(tbCurrentAmount.Text) + (CFloat(tbAdjustPercent.Text) * CFloat(tbCurrentAmount.Text)) / 100
            'Else
            'tbNewPrice.Text = CFloat(tbOldPrice.Text) - (CFloat(tbAdjustPercent.Text) * CFloat(tbOldPrice.Text)) / 100
            'End If
            AttachScript("setformatdt();", Page, Me.GetType())
        Catch ex As Exception
            Throw New Exception("GetTypeChange Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbAdjustPercent_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAdjustPercent.TextChanged
        If CFloat(tbCurrentAmount.Text) = 0 Then
            tbAdjustPercent.Text = "0"
        Else
            GetTypeChange()
        End If
    End Sub

    Protected Sub BtnPayroll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnPayroll.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT PayrollCode, PayrollName, GroupBy, Formula, FormulaName, Amount from VMsPayroll WHERE PayrollPref = 'P' "
            ResultField = "PayrollCode, PayrollName, GroupBy, Formula, Amount"
            ViewState("Sender") = "btnPayroll"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Payroll Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbPayroll_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPayroll.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Dim SQLString As String
        Try
            SQLString = "SELECT PayrollCode, PayrollName, GroupBy, Formula, FormulaName, Amount from VMsPayroll WHERE PayrollPref = 'P' AND PayrollCode = " + QuotedStr(tbPayroll.Text)
            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbPayroll.Text = Dr("PayrollCode")
                tbPayrollName.Text = Dr("PayrollName")
                tbType.Text = Dr("GroupBy")
                ddlFormula.SelectedValue = Dr("Formula")
                tbItemCode.Text = ""
                tbItemName.Text = ""
                tbCurrentAmount.Text = FormatFloat(Dr("Amount"), -1)
                tbNewAmount.Text = FormatFloat(Dr("Amount"), -1)
                tbAdjustAmount.Text = "0"
                tbAdjustPercent.Text = "0"
            Else
                tbPayroll.Text = ""
                tbPayrollName.Text = ""
                ddlFormula.SelectedValue = ""
                tbType.Text = ""
                tbCurrentAmount.Text = "0"
                tbNewAmount.Text = "0"
                tbAdjustAmount.Text = "0"
                tbAdjustPercent.Text = "0"
            End If
        Catch ex As Exception
            Throw New Exception("tbPayroll_TextChanged error: " + ex.ToString)
        End Try
    End Sub

    Protected Sub GetAdjPercent()
        Dim HargaLama, HargaBaru, Hasil As Double
        Try
            HargaLama = tbCurrentAmount.Text
            HargaBaru = tbNewAmount.Text
            Hasil = 0
            
            If CFloat(HargaBaru) > CFloat(HargaLama) Then
                Hasil = ((CFloat(HargaBaru) - CFloat(HargaLama)) / CFloat(HargaLama) * 100)
            Else
                Hasil = ((CFloat(HargaLama) - CFloat(HargaBaru)) / CFloat(HargaLama) * 100)
            End If

            tbAdjustPercent.Text = FormatFloat(Hasil, ViewState("DigitCurr"))

            AttachScript("setformatdt();", Page, Me.GetType())
        Catch ex As Exception
            Throw New Exception("GetAdjPercent Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbNewPrice_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbNewAmount.TextChanged
        GetAdjPercent()
    End Sub

    Protected Sub btnItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnItem.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT A.ItemCode, A.ItemName from VMsItemSlip A INNER JOIN V_MsMethodAccess B ON A.ItemCode = B.EmpNumb " + _
			" WHERE B.UserId = " + QuotedStr(ViewState("UserId").ToString)+ " AND A.Type = " + QuotedStr(tbType.Text) + _
                        " AND ItemCode NOT IN ( SELECT A.ItemCode FROM VMsPayrollSlip A WHERE A.Payroll = " + QuotedStr(tbPayroll.Text) + " AND A.StartDate >= '" + Format(tbEffectiveDate.SelectedValue, "yyyy-MM-dd") + "')"
            ResultField = "ItemCode, ItemName"
            ViewState("Sender") = "btnItem"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnItem_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbItemCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbItemCode.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Dim dtSlip As DataTable
        Dim SQLString As String
        Try
            SQLString = "SELECT A.ItemCode, A.ItemName from VMsItemSlip A INNER JOIN V_MsMethodAccess B ON A.ItemCode = B.EmpNumb " + _
			" WHERE B.UserId = " + QuotedStr(ViewState("UserId").ToString)+ " AND A.Type = " + QuotedStr(tbType.Text) + " AND ItemCode = " + QuotedStr(tbItemCode.Text) + _
                        " AND ItemCode NOT IN ( SELECT A.ItemCode FROM VMsPayrollSlip A WHERE A.Payroll = " + QuotedStr(tbPayroll.Text) + " AND A.StartDate >= '" + Format(tbEffectiveDate.SelectedValue, "yyyy-MM-dd") + "')"
            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbItemCode.Text = Dr("ItemCode")
                tbItemName.Text = Dr("ItemName")
                dtSlip = SQLExecuteQuery("Select A.Currency, A.StartDate, A.Amount, A.Formula from VMsPayrollSlip A " + _
			" WHERE A.EndDate = '20500101' AND A.Payroll = " + QuotedStr(tbPayroll.Text) + " AND A.ItemCode = " + QuotedStr(tbItemCode.Text), ViewState("DBConnection").ToString).Tables(0)

                If dtSlip.Rows.Count > 0 Then
                    ddlCurrency.SelectedValue = dtSlip.Rows(0)("Currency")
                    tbLastEffective.SelectedValue = dtSlip.Rows(0)("StartDate")
                    ddlFormula.SelectedValue = dtSlip.Rows(0)("Formula")
                    tbCurrentAmount.Text = FormatFloat(dtSlip.Rows(0)("Amount"), -1)
                    tbNewAmount.Text = FormatFloat(dtSlip.Rows(0)("Amount"), -1)
                Else
                    tbLastEffective.SelectedValue = Nothing
                    tbCurrentAmount.Text = "0"
                    tbNewAmount.Text = "0"
                End If
                tbAdjustAmount.Text = "0"
                tbAdjustPercent.Text = "0"
            Else
                tbItemCode.Text = ""
                tbItemName.Text = ""
                tbCurrentAmount.Text = "0"
                tbNewAmount.Text = "0"
                tbAdjustAmount.Text = "0"
                tbAdjustPercent.Text = "0"
            End If
        Catch ex As Exception
            Throw New Exception("tbItemCode_TextChanged error: " + ex.ToString)
        End Try
    End Sub
End Class
