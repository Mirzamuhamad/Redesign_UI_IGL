Imports System.Data
Imports System.Data.SqlClient

Partial Class Transaction_TrPYProcess_TrPYProcess
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "SELECT distinct ProcessCode, Status, Method, MethodName, Year, Month, Remark FROM V_PYProcessHd "

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

            End If
            lbStatus.Text = ""
            'BtnGo.Visible = ddlCommand.Visible
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If
            If Not Session("Result") Is Nothing Then
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
        Try
            FillRange(ddlRange)
            FillCombo(ddlMethod, "EXEC S_GetMethodUser " + QuotedStr(ViewState("UserId").ToString), True, "MethodCode", "MethodName", ViewState("DBConnection"))
            FillCombo(ddlYear, "S_GetYear", True, "Year", "Year", ViewState("DBConnection"))
            FillCombo(ddlMonth, "Select Period, Description from GLPeriod WHERE Period BETWEEN 1 and 12", True, "Period", "Description", ViewState("DBConnection"))
            FillCombo(ddlPayroll, "Select PayrollGrpCode, PayrollGrpName from VMsPayrollGroup", True, "PayrollGrpCode", "PayrollGrpName", ViewState("DBConnection"))
            ViewState("SortExpression") = Nothing
            ViewState("DigitCurr") = 0
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
            If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
                'ddlCommand.Items.Add("Print")
                'ddlCommand2.Items.Add("Print")
            End If
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
                ViewState("SortExpression") = "ProcessCode DESC"
            End If
            DV.Sort = ViewState("SortExpression")
            GridView1.DataSource = DV
            GridView1.DataBind()
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub
    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT * FROM V_PYProcessDt WHERE ProcessCode = " + QuotedStr(Nmbr)
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
                Session("SelectCommand") = "EXEC S_PYProcess " + Result
                Session("ReportFile") = ".../../../Rpt/FormNonAbsence.frx"
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
                        Result = ExecSPCommandGo(ActionValue, "S_PYProcess", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                        If Trim(Result) <> "" Then
                            lbStatus.Text = lbStatus.Text + Result + " <br/>"
                        End If
                    End If
                Next
                BindData("ProcessCode in (" + ListSelectNmbr + ")")
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
            'tbDate.Enabled = State
            ddlMethod.Enabled = State
            ddlYear.Enabled = State
            ddlMonth.Enabled = State
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
    Private Sub StatusButtonSave(ByVal Bool As Boolean)
        Try
            btnSaveAll.Visible = Bool
            btnSaveTrans.Visible = Bool
            btnBack.Visible = Bool
        Catch ex As Exception
            Throw New Exception("Status Button Save Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnCancelDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelDt.Click
        Try
            MovePanel(pnlEditDt, PnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbCode.Text = ""
            'tbDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            ddlMethod.SelectedIndex = 0
            ddlYear.SelectedIndex = 0
            ddlMonth.SelectedIndex = 0
            ViewState("StartDate") = Nothing
            ViewState("EndDate") = Nothing
            tbRemark.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            ddlPayroll.SelectedIndex = 0
            If Not ViewState("StartDate") = Nothing Then
                tbStartDate.SelectedDate = ViewState("StartDate")
                tbEndDate.SelectedDate = ViewState("EndDate")
            End If
            tbRemarkDt.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub

    Function CekHd() As Boolean
        Try
            'If tbDate.SelectedDate = Nothing Then
            '    lbStatus.Text = MessageDlg("Date must have value")
            '    tbDate.Focus()
            '    Return False
            'End If
            'If CInt(Session(Request.QueryString("KeyId"))("Year")) <> Year(tbDate.SelectedValue) Or CInt(Session(Request.QueryString("KeyId"))("Period")) <> Month(tbDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(Session(Request.QueryString("KeyId"))("Period")) + " " + Session(Request.QueryString("KeyId"))("Year").ToString.Trim)
            '    Return False
            'End If
            If ddlMethod.SelectedValue.Trim = "" Then
                lbStatus.Text = MessageDlg("Method Salary Must Have Value")
                ddlMethod.Focus()
                Return False
            End If
            If ddlYear.SelectedValue.Trim = "" Then
                lbStatus.Text = MessageDlg("Year Salary Must Have Value")
                ddlYear.Focus()
                Return False
            End If
            If ddlMonth.SelectedValue.Trim = "" Then
                lbStatus.Text = MessageDlg("Month Salary Must Have Value")
                ddlMonth.Focus()
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

            Else
                If ddlPayroll.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Payroll Group Must Have Value")
                    ddlPayroll.Focus()
                    Return False
                End If
                If tbStartDate.SelectedDate = Nothing Then
                    lbStatus.Text = MessageDlg("Start Date Must have values")
                    tbStartDate.Focus()
                    Return False
                End If
                If tbEndDate.SelectedDate = Nothing Then
                    lbStatus.Text = MessageDlg("End Date Must have values")
                    tbEndDate.Focus()
                    Return False
                End If
                If tbStartDate.SelectedDate > tbEndDate.SelectedDate Then
                    lbStatus.Text = MessageDlg("End Date Must be greater than Start Date")
                    tbStartDate.Focus()
                    Return False
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
            Dt = BindDataTransaction(GetStringHd, "ProcessCode = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            tbCode.Text = Nmbr
            BindToDropList(ddlMethod, Dt.Rows(0)("Method").ToString)
            BindToDropList(ddlYear, Dt.Rows(0)("Year").ToString)
            BindToDropList(ddlMonth, Dt.Rows(0)("Month").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxDt(ByVal PayrollGrp As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("PayrollGroup = " + QuotedStr(PayrollGrp))
            If Dr.Length > 0 Then
                BindToDropList(ddlPayroll, Dr(0)("PayrollGroup").ToString)
                BindToDate(tbStartDate, Dr(0)("StartDate").ToString)
                BindToDate(tbEndDate, Dr(0)("EndDate").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
                ViewState("StartDate") = tbStartDate.SelectedDate
                ViewState("EndDate") = tbEndDate.SelectedDate
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt.Click
        Try
            If CekDt() = False Then
                btnSaveDt.Focus()
                Exit Sub
            End If
            If ViewState("StateDt") = "Edit" Then
                If ddlPayroll.SelectedValue <> ViewState("PayrollGroup") Then
                    If CekExistData(ViewState("Dt"), "PayrollGroup", ddlPayroll.SelectedValue) = True Then
                        lbStatus.Text = "Payroll Group " + ddlPayroll.Text + " has already been exist"
                        Exit Sub
                    End If
                End If
                Dim Row As DataRow
                Row = ViewState("Dt").Select("PayrollGroup = " + QuotedStr(ddlPayroll.SelectedValue))(0)
                Row.BeginEdit()
                Row("PayrollGroup") = ddlPayroll.SelectedValue
                Row("PayrollGrpName") = ddlPayroll.SelectedItem.Text
                Row("StartDate") = tbStartDate.SelectedDate
                Row("EndDate") = tbEndDate.SelectedDate
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()
            Else
                'Insert
                If CekExistData(ViewState("Dt"), "PayrollGroup", ddlPayroll.SelectedValue) = True Then
                    lbStatus.Text = "Payroll Group " + ddlPayroll.Text + " has already been exist"
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("PayrollGroup") = ddlPayroll.SelectedValue
                dr("PayrollGrpName") = ddlPayroll.SelectedItem.Text
                dr("StartDate") = tbStartDate.SelectedDate
                dr("EndDate") = tbEndDate.SelectedDate
                dr("Remark") = tbRemarkDt.Text
                ViewState("Dt").Rows.Add(dr)
            End If
            ViewState("StartDate") = tbStartDate.SelectedDate
            ViewState("EndDate") = tbEndDate.SelectedDate
            MovePanel(pnlEditDt, PnlDt)
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
            If Not (ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit") Then
                Exit Sub
            Else
                ViewState("StateSave") = ViewState("StateHd")
                ViewState("StateHd") = "View"
            End If
            'Save Hd
            If ViewState("StateSave") = "Insert" Then
                tbCode.Text = GetAutoNmbr("PSA", "Y", CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), "", ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO PYProcessHd (ProcessCode, Status, Method, Year, Month, Remark, FgPPhYear, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", 'H', " + QuotedStr(ddlMethod.SelectedValue) + ", " + QuotedStr(ddlYear.SelectedValue) + ", " + QuotedStr(ddlMonth.SelectedValue) + ", " + QuotedStr(tbRemark.Text) + _
                ", 'N', " + QuotedStr(ViewState("UserId").ToString) + ", GetDate() "
                ViewState("TransNmbr") = tbCode.Text
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("SELECT Status FROM PYProcessHd WHERE ProcessCode = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE PYProcessHd SET Method =" + QuotedStr(ddlMethod.SelectedValue) + ", Year = " + ddlYear.SelectedValue + ", Month = " + ddlMonth.SelectedValue + _
                ", Remark =" + QuotedStr(tbRemark.Text) + ", UserPrep = " + QuotedStr(ViewState("UserId").ToString) + _
                ", DatePrep = GetDate()" + _
                " WHERE ProcessCode = " + QuotedStr(tbCode.Text)
            End If
            SQLString = ChangeQuoteNull(SQLString)
            SQLString = SQLString.Replace("'1900-01-01'", "NULL")
            SQLString = SQLString.Replace("'0001-01-01'", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            'update Primary Key on Dt
            Dim Row As DataRow()

            Row = ViewState("Dt").Select("ProcessCode IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("ProcessCode") = tbCode.Text
                Row(I).EndEdit()
            Next


            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT ProcessCode, PayrollGroup, StartDate, EndDate, Remark FROM PYProcessDt WHERE ProcessCode = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand
            Dim Dt As New DataTable("PYProcessDt")
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
            SaveAll()
            MovePanel(pnlInput, PnlHd)
            CurrFilter = tbFilter.Text
            Value = ddlField.SelectedValue
            tbFilter.Text = tbCode.Text
            ddlField.SelectedValue = "ProcessCode"
            btnSearch_Click(Nothing, Nothing)
            tbFilter.Text = CurrFilter
            ddlField.SelectedValue = Value
        Catch ex As Exception
            lbStatus.Text = "btnSaveTrans Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, PnlDt, GridDt)
            newTrans()
            btnHome.Visible = False
            tbCode.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdddt.Click, btnAddDtke2.Click
        Try
            Cleardt()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt") = "Insert"
            MovePanel(PnlDt, pnlEditDt)
            EnableHd(False)
            StatusButtonSave(False)
            ddlPayroll.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt error : " + ex.ToString
        End Try
    End Sub
    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ViewState("TransNmbr") = ""
            ClearHd()
            Cleardt()
            EnableHd(True)
            PnlDt.Visible = True
            BindDataDt("")
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
        Dim FilterName, FilterValue As String
        Try
            FilterName = "Reference, Status, Method, Year, Month, Remark"
            FilterValue = "ProcessCode, Status, Method, Year, Month, Remark"
            'Session("DateFieldName") = FDateName.Split(",")
            'Session("DateFieldValue") = FDateValue.Split(",")
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
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, PnlDt, GridDt)
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
                        ModifyInput2(True, pnlInput, PnlDt, GridDt)
                        btnHome.Visible = False
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
                        Session("SelectCommand") = "EXEC S_PEFormNonAbsence '" + GVR.Cells(2).Text + "'"
                        Session("ReportFile") = ".../../../Rpt/FormNonAbsence.frx"
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

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)

            dr = ViewState("Dt").Select("PayrollGroup = " + QuotedStr(GVR.Cells(1).Text))
            dr(0).Delete()
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
            FillTextBoxDt(GVR.Cells(1).Text)
            MovePanel(PnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            ViewState("PayrollGroup") = GVR.Cells(1).Text
            btnSaveDt.Focus()
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
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
            EnableHd(True)
        Catch ex As Exception
            lbStatus.Text = "btn save all error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlMethod_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMethod.SelectedIndexChanged
        Dim dt As DataTable
        Try
            dt = SQLExecuteQuery("EXEC S_PYProcessNextPeriod " + QuotedStr(ddlMethod.SelectedValue), ViewState("DBConnection").ToString).Tables(0)
            ddlYear.SelectedValue = dt.Rows(0)("Year")
            ddlMonth.SelectedValue = dt.Rows(0)("Month")
        Catch ex As Exception
            lbStatus.Text = "ddlMethod_SelectedIndexChanged error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
        Dim dt As DataTable
        Dim dr As DataRow
        Try
            If ddlMethod.SelectedValue.Trim = "" Then
                lbStatus.Text = MessageDlg("Methode Salary must have value")
                ddlMethod.Focus()
                Exit Sub
            End If
            If ddlYear.SelectedValue.Trim = "" Then
                lbStatus.Text = MessageDlg("Year must have value")
                ddlYear.Focus()
                Exit Sub
            End If
            If ddlMonth.SelectedValue.Trim = "" Then
                lbStatus.Text = MessageDlg("Month must have value")
                ddlMonth.Focus()
                Exit Sub
            End If

            dt = SQLExecuteQuery("EXEC S_PYProcessGetDetail " + QuotedStr(ddlMethod.SelectedValue) + ", " + ddlYear.SelectedValue + ", " + ddlMonth.SelectedValue, ViewState("DBConnection").ToString).Tables(0)
            If dt.Rows.Count > 0 Then
                For Each dr In dt.Rows()
                    If CekExistData(ViewState("Dt"), "PayrollGroup", dr("PayrollGroup")) = False Then
                        Dim drnew As DataRow
                        drnew = ViewState("Dt").NewRow
                        drnew("PayrollGroup") = dr("PayrollGroup")
                        drnew("PayrollGrpName") = dr("PayrollGrpName")
                        drnew("StartDate") = dr("StartDate")
                        drnew("EndDate") = dr("EndDate")
                        drnew("Remark") = ""
                        ViewState("Dt").Rows.Add(drnew)
                    End If
                Next
                BindGridDt(ViewState("Dt"), GridDt)
            End If
        Catch ex As Exception
            lbStatus.Text = "btnGenerate_Click error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlPayroll_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPayroll.SelectedIndexChanged
        Dim dt As DataTable
        Try
            dt = SQLExecuteQuery("EXEC S_PYProcessGetDate " + QuotedStr(ddlMethod.SelectedValue) + ", " + QuotedStr(ddlPayroll.SelectedValue), ViewState("DBConnection").ToString).Tables(0)
            If dt.Rows.Count > 0 Then
                tbStartDate.SelectedDate = dt.Rows(0)("StartDate")
                tbEndDate.SelectedDate = dt.Rows(0)("EndDate")
            End If
        Catch ex As Exception
            lbStatus.Text = "ddlPayroll_SelectedIndexChanged error : " + ex.ToString
        End Try
    End Sub
End Class
