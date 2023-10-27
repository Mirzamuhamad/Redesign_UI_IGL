Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Partial Class Transaction_TrEmpOverTime_TrEmpOverTime
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "SELECT * FROM V_PEOvertimeHd"
    Protected GetStringComplete As String = "SELECT * FROM V_PEOvertimeDt"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                SetInit()
                Session("AdvanceFilter") = ""
            End If
            lbStatus.Text = ""
            BtnGo.Visible = ddlCommand.Visible
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If
            If Not Session("Result") Is Nothing Then
                Dim sqlstring As String

                If ViewState("Sender") = "btnEmp" Then
                    tbEmpNo.Text = Session("Result")(0).ToString
                    tbEmpName.Text = Session("Result")(1).ToString
                    ddlDayTypeDt.SelectedValue = ddlDayType.SelectedValue
                    tbStartDateDt.SelectedDate = tbStartDate.SelectedDate
                    tbEndDateDt.SelectedDate = tbEndDate.SelectedDate
                    tbStartTimeDt.Text = tbStartTime.Text
                    tbEndTimeDt.Text = tbEndTime.Text
                    ddlFgAllowanceDt.SelectedValue = ddlFgMealAllowance.SelectedValue
                    tbMinuteBreakDt.Text = tbMinuteBreak.Text.Replace(".", "")
                    sqlstring = "EXEC S_PEOvertimeGetHourNetto " + tbMinuteNetto.Text.Replace(",", "")
                    ViewState("HourNetto") = 0
                    ViewState("HourNetto") = SQLExecuteScalar(sqlstring, ViewState("DBConnection"))
                    tbHourNetto.Text = FormatNumber(ViewState("HourNetto"), CInt(ViewState("DigitCurr")))
                    tbStartTimeDt_TextChanged(Nothing, Nothing)
                    tbEndTimeDt_TextChanged(Nothing, Nothing)
                    tbMinuteBreakDt_TextChanged(Nothing, Nothing)
                End If
                If ViewState("Sender") = "btnAcknow" Then
                    tbAcknowBy.Text = Session("Result")(0).ToString
                    tbAcknowByName.Text = Session("Result")(1).ToString
                    ddlAcknowledgeJbt.SelectedValue = Session("Result")(2).ToString
                End If
                If ViewState("Sender") = "btnAppr" Then
                    tbApprBy.Text = Session("Result")(0).ToString
                    tbApprByName.Text = Session("Result")(1).ToString
                    ddlApprJbt.SelectedValue = Session("Result")(2).ToString
                End If
                If ViewState("Sender") = "btnGetData" Then
                    Dim drResult, dr As DataRow
                    Dim Row As DataRow()
                    For Each drResult In Session("Result").Rows
                        Row = ViewState("Dt").Select("EmpNumb = " + QuotedStr(drResult("Emp_No")))
                        If Row.Count = 0 Then
                            dr = ViewState("Dt").NewRow
                            dr("EmpNumb") = drResult("Emp_No")
                            dr("EmpName") = drResult("Emp_Name")
                            dr("DayType") = ddlDayType.SelectedValue
                            dr("StartDate") = tbStartDate.SelectedDate
                            dr("StartTime") = tbStartTime.Text
                            dr("EndDate") = tbEndDate.SelectedDate
                            dr("EndTime") = tbEndTime.Text
                            dr("MinuteBruto") = tbMinuteBruto.Text.Replace(",", "")
                            dr("MinuteBreak") = tbMinuteBreak.Text.Replace(".", "")
                            dr("MinuteNetto") = tbMinuteNetto.Text.Replace(",", "")
                            sqlstring = "EXEC S_PEOvertimeGetHourNetto " + tbMinuteNetto.Text.Replace(",", "")
                            ViewState("HourNetto") = 0
                            ViewState("HourNetto") = SQLExecuteScalar(sqlstring, ViewState("DBConnection"))
                            dr("HourNetto") = FormatNumber(ViewState("HourNetto"), CInt(ViewState("DigitCurr")))
                            dr("FgMealAllowance") = ddlFgMealAllowance.SelectedValue
                            dr("DoneComplete") = "N"
                            ViewState("Dt").Rows.Add(dr)
                        End If
                    Next
                    BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                End If
                Session("Result") = Nothing
                ViewState("Sender") = Nothing
                'Session("filter") = Nothing
                'Session("Column") = Nothing
            End If
            If Not ViewState("EmpClose") Is Nothing Then
                If HiddenRemarkClose.Value <> "False Value" Then
                    Dim sqlstring, result As String
                    sqlstring = "Declare @A VarChar(255) EXEC S_PEIzinComplete '" + tbCode.Text + "', '" + ViewState("EmpClose").ToString + "'," + QuotedStr(HiddenStartHour.Value) + ", " + QuotedStr(HiddenEndHour.Value) + ", '" + HiddenRemarkClose.Value + "'," + QuotedStr(ViewState("UserId")) + ", @A OUT SELECT @A"
                    result = SQLExecuteScalar(sqlstring, ViewState("DBConnection").ToString)
                    If result.Length > 2 Then
                        lbStatus.Text = result
                    Else
                        BindDataDt(ViewState("TransNmbr"))
                    End If
                End If
                ViewState("EmpClose") = Nothing
                HiddenRemarkClose.Value = ""
                'GridDt.Columns(0).Visible = False
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
            FillCombo(ddlDepartment, "SELECT Dept_Code, Dept_Name FROM VMsDepartment", True, "Dept_Code", "Dept_Name", ViewState("DBConnection"))
            FillCombo(ddlAcknowledgeJbt, "SELECT * FROM VMsJobTitle", True, "Job_Title_Code", "Job_Title_Name", ViewState("DBConnection"))
            FillCombo(ddlApprJbt, "SELECT * FROM VMsJobTitle", True, "Job_Title_Code", "Job_Title_Name", ViewState("DBConnection"))

            ViewState("SortExpression") = Nothing
            ViewState("DigitCurr") = ViewState("DigitHome")
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
            If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
                ddlCommand.Items.Add("Print")
                ddlCommand2.Items.Add("Print")
            End If
            tbMinuteBreak.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbMinuteBreakDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbEmpName.Attributes.Add("ReadOnly", "True")
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
        Return "SELECT * FROM V_PEOvertimeDt WHERE TransNmbr = " + QuotedStr(Nmbr)
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
                Session("SelectCommand") = "EXEC S_PEFormOvertime " + Result
                Session("ReportFile") = ".../../../Rpt/FormEmpOverTime.frx"
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
                        Result = ExecSPCommandGo(ActionValue, "S_PEOvertime", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
        Try
            tbDate.Enabled = State
            ddlDepartment.Enabled = State
            ddlDayType.Enabled = State
            ddlSusulan.Enabled = State
            tbStartDate.Enabled = State
            tbStartTime.Enabled = State
            tbEndDate.Enabled = State
            tbEndTime.Enabled = State
            tbMinuteBreak.Enabled = State
            ddlFgMealAllowance.Enabled = State
            tbAcknowBy.Enabled = State
            btnAcknow.Visible = State
            tbApprBy.Enabled = State
            btnAppr.Visible = State
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
            If ViewState("UnClosing") = "UnComplete" Then
                BindData(Session("AdvanceFilter"))

            End If
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
            btnGetData.Visible = True
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbCode.Text = ""
            tbDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            ddlDepartment.SelectedIndex = 0
            ddlDayType.SelectedValue = "Work"
            ddlSusulan.SelectedValue = "N"
            tbStartDate.SelectedDate = ViewState("ServerDate")
            tbStartTime.Text = "00:00"
            tbEndDate.SelectedDate = ViewState("ServerDate")
            tbEndTime.Text = "00:00"
            tbMinuteBreak.Text = "0"
            tbMinuteBruto.Text = "0"
            tbMinuteNetto.Text = "0"
            ddlFgMealAllowance.SelectedValue = "N"
            tbAcknowBy.Text = ""
            tbAcknowByName.Text = ""
            ddlAcknowledgeJbt.SelectedIndex = 0
            tbApprBy.Text = ""
            tbApprByName.Text = ""
            ddlApprJbt.SelectedIndex = 0
            tbRemark.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Dim sqlString As String
        Try
            tbEmpNo.Text = ""
            tbEmpName.Text = ""
            ddlDayTypeDt.SelectedValue = ddlDayType.SelectedValue
            tbStartDateDt.SelectedDate = tbStartDate.SelectedDate
            tbEndDateDt.SelectedDate = tbEndDate.SelectedDate
            tbStartTimeDt.Text = tbStartTime.Text
            tbEndTimeDt.Text = tbEndTime.Text
            tbMinuteBreakDt.Text = tbMinuteBreak.Text.Replace(".", "")
            tbMinuteBrutoDt.Text = tbMinuteBruto.Text.Replace(",", "")
            tbMinuteNettoDt.Text = tbMinuteNetto.Text.Replace(",", "")
            sqlString = "EXEC S_PEOvertimeGetHourNetto " + tbMinuteNettoDt.Text.Replace(",", "")
            ViewState("HourNetto") = 0
            ViewState("HourNetto") = SQLExecuteScalar(sqlstring, ViewState("DBConnection"))
            tbHourNetto.Text = FormatNumber(ViewState("HourNetto"), CInt(ViewState("DigitCurr")))
            ddlFgAllowanceDt.SelectedValue = ddlFgMealAllowance.SelectedValue
            tbRemarkDt.Text = tbRemark.Text
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub

    Function CekHd() As Boolean
        Try
            If tbDate.SelectedDate = Nothing Then
                lbStatus.Text = MessageDlg("Date must have value")
                tbDate.Focus()
                Return False
            End If
            'If CInt(Session(Request.QueryString("KeyId"))("Year")) <> Year(tbDate.SelectedValue) Or CInt(Session(Request.QueryString("KeyId"))("Period")) <> Month(tbDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(Session(Request.QueryString("KeyId"))("Period")) + " " + Session(Request.QueryString("KeyId"))("Year").ToString.Trim)
            '    Return False
            'End If
            If ddlDepartment.SelectedValue.Trim = "" Then
                lbStatus.Text = MessageDlg("Organization must have value")
                ddlDepartment.Focus()
                Return False
            End If
            If ddlDayType.SelectedValue.Trim = "" Then
                lbStatus.Text = MessageDlg("Day Type must have value")
                ddlDayType.Focus()
                Return False
            End If
            If ddlSusulan.SelectedValue.Trim = "" Then
                lbStatus.Text = MessageDlg("Susulan must have value")
                ddlSusulan.Focus()
                Return False
            End If
            If tbStartDate.SelectedDate = Nothing Then
                lbStatus.Text = MessageDlg("Start Date must have value")
                tbStartDate.Focus()
                Return False
            End If
            If tbStartDate.SelectedDate < tbDate.SelectedDate Then
                lbStatus.Text = MessageDlg("Start Date must greater than Transaction Date")
                tbStartDate.Focus()
                Return False
            End If
            If tbEndDate.SelectedDate = Nothing Then
                lbStatus.Text = MessageDlg("End Date must have value")
                tbEndDate.Focus()
                Return False
            End If
            If tbEndDate.SelectedDate < tbStartDate.SelectedDate Then
                lbStatus.Text = MessageDlg("End Date must greater than Start Date")
                tbEndDate.Focus()
                Return False
            End If
            If tbStartTime.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Start Time must have value")
                tbEndDate.Focus()
                Return False
            End If
            If tbEndTime.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("End Time must have value")
                tbEndTime.Focus()
                Return False
            End If
            If (tbStartTimeDt.Text.Trim <> "00:00") And (tbEndTimeDt.Text.Trim <> "00:00") And (tbStartTimeDt.Text.Trim <> "") And (tbEndTimeDt.Text.Trim <> "") Then
                If (tbEndTime.Text.Trim < tbStartTime.Text.Trim) And (tbStartTime.Text.Trim = tbEndTime.Text.Trim) Then
                    lbStatus.Text = MessageDlg("End Time must be greater than Start Time")
                    tbEndTime.Focus()
                    Return False
                End If
            End If
            If ddlFgMealAllowance.SelectedValue.Trim = "" Then
                lbStatus.Text = MessageDlg("Meal Allowance must have value")
                ddlFgMealAllowance.Focus()
                Return False
            End If
            If tbAcknowBy.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Acknowledge Must Have Value")
                tbAcknowBy.Focus()
                Return False
            End If
            If tbApprBy.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Approval Must Have Value")
                tbApprBy.Focus()
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
                
                If tbDate.SelectedDate > tbStartDateDt.SelectedDate Then
                    lbStatus.Text = MessageDlg("Your Start Date Is Earlier Than Your Transaction Date")
                    tbStartDateDt.SelectedDate = tbDate.SelectedDate
                End If
                If tbEmpNo.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Employee Must Have Value")
                    tbEmpNo.Focus()
                    Return False
                End If
                If ddlDayTypeDt.SelectedValue.Trim = "" Then
                    lbStatus.Text = MessageDlg("Day Type Must Have Value")
                    ddlDayTypeDt.Focus()
                    Return False
                End If
                If tbStartDateDt.SelectedDate = Nothing Then
                    lbStatus.Text = MessageDlg("Start Date Must Have Value")
                    tbStartDateDt.Focus()
                    Return False
                End If
                If tbStartDateDt.SelectedDate < tbDate.SelectedDate Then
                    lbStatus.Text = MessageDlg("Start Date must greater than Transaction Date")
                    tbStartDateDt.Focus()
                    Return False
                End If
                If tbEndDateDt.SelectedDate = Nothing Then
                    lbStatus.Text = MessageDlg("End Date Must Have Value")
                    tbEndDateDt.Focus()
                    Return False
                End If
                If tbEndDateDt.SelectedDate < tbStartDateDt.SelectedDate Then
                    lbStatus.Text = MessageDlg("End Date must greater than Start Date")
                    tbEndDateDt.Focus()
                    Return False
                End If
                If tbStartTimeDt.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Start Time Must Have Value")
                    tbStartTimeDt.Focus()
                    Return False
                End If
                If tbEndTimeDt.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("End Time Must Have Value")
                    tbEndTimeDt.Focus()
                    Return False
                End If
                If (tbStartTimeDt.Text.Trim <> "00:00") And (tbEndTimeDt.Text.Trim <> "00:00") And (tbStartTimeDt.Text.Trim <> "") And (tbEndTimeDt.Text.Trim <> "") Then
                    If (tbEndTimeDt.Text.Trim < tbStartTimeDt.Text.Trim) And (tbStartTimeDt.Text.Trim = tbEndTimeDt.Text.Trim) Then
                        lbStatus.Text = MessageDlg("End Time must be greater than Start Time")
                        tbEndTimeDt.Focus()
                        Return False
                    End If
                End If
                If CFloat(tbMinuteBreakDt.Text.Trim) < "0" Then
                    lbStatus.Text = MessageDlg("Minute Break Must Have Value")
                    tbMinuteBreakDt.Focus()
                    Return False
                End If
                If ddlFgAllowanceDt.SelectedValue.Trim = "" Then
                    lbStatus.Text = MessageDlg("Meal Allowance Must Have Value")
                    ddlFgAllowanceDt.Focus()
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
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            tbCode.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToDropList(ddlDepartment, Dt.Rows(0)("Department").ToString)
            BindToDropList(ddlDayType, Dt.Rows(0)("DayType").ToString)
            BindToDropList(ddlSusulan, Dt.Rows(0)("FgSusulan").ToString)
            BindToDate(tbStartDate, Dt.Rows(0)("StartDate").ToString)
            BindToDate(tbEndDate, Dt.Rows(0)("EndDate").ToString)
            BindToText(tbStartTime, Dt.Rows(0)("StartTime").ToString)
            BindToText(tbEndTime, Dt.Rows(0)("EndTime").ToString)
            BindToText(tbMinuteBruto, Dt.Rows(0)("MinuteBruto").ToString)
            BindToText(tbMinuteBreak, Dt.Rows(0)("MinuteBreak").ToString)
            BindToText(tbMinuteNetto, Dt.Rows(0)("MinuteNetto").ToString)
            BindToDropList(ddlFgMealAllowance, Dt.Rows(0)("FgMealAllowance").ToString)
            BindToText(tbAcknowBy, Dt.Rows(0)("AcknowBy").ToString)
            BindToText(tbAcknowByName, Dt.Rows(0)("AcknowByName").ToString)
            BindToDropList(ddlAcknowledgeJbt, Dt.Rows(0)("AcknowJbt").ToString)
            BindToText(tbApprBy, Dt.Rows(0)("ApprBy").ToString)
            BindToText(tbApprByName, Dt.Rows(0)("ApprByName").ToString)
            BindToDropList(ddlApprJbt, Dt.Rows(0)("ApprJbtName").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal Emp As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("EmpNumb = " + QuotedStr(Emp))
            If Dr.Length > 0 Then
                BindToText(tbEmpNo, Dr(0)("EmpNumb").ToString)
                BindToText(tbEmpName, Dr(0)("EmpName").ToString)
                BindToDropList(ddlDayTypeDt, Dr(0)("DayType").ToString)
                BindToDate(tbStartDateDt, Dr(0)("StartDate").ToString)
                BindToDate(tbEndDateDt, Dr(0)("EndDate").ToString)
                BindToText(tbStartTimeDt, Dr(0)("StartTime").ToString)
                BindToText(tbEndTimeDt, Dr(0)("EndTime").ToString)
                BindToText(tbMinuteBrutoDt, Dr(0)("MinuteBruto").ToString)
                BindToText(tbMinuteBreakDt, Dr(0)("MinuteBreak").ToString)
                BindToText(tbMinuteNettoDt, Dr(0)("MinuteNetto").ToString)
                BindToText(tbHourNetto, Dr(0)("HourNetto").ToString)
                BindToDropList(ddlFgAllowanceDt, Dr(0)("FgMealAllowance").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
                BindToText(tbDoneComplete, Dr(0)("DoneComplete").ToString)
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
                Dim Row As DataRow

                If ViewState("EmpNo") <> tbEmpNo.Text Then
                    If CekExistData(ViewState("Dt"), "EmpNumb", tbEmpNo.Text) Then
                        lbStatus.Text = "Employee " + tbEmpNo.Text + " has been already exist"
                        Exit Sub
                    End If
                End If

                Row = ViewState("Dt").Select("EmpNumb = " + QuotedStr(ViewState("EmpNo")))(0)
                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                Row("EmpNumb") = tbEmpNo.Text
                Row("EmpName") = tbEmpName.Text
                Row("DayType") = ddlDayTypeDt.SelectedValue
                Row("StartDate") = tbStartDateDt.SelectedDate
                Row("EndDate") = tbEndDateDt.SelectedDate
                Row("StartTime") = tbStartTimeDt.Text
                Row("EndTime") = tbEndTimeDt.Text
                Row("MinuteBruto") = tbMinuteBrutoDt.Text.Replace(",", "")
                Row("MinuteBreak") = tbMinuteBreakDt.Text.Replace(".", "")
                Row("MinuteNetto") = tbMinuteNettoDt.Text.Replace(",", "")
                Row("HourNetto") = tbHourNetto.Text.Replace(",", "")
                Row("FgMealAllowance") = ddlFgAllowanceDt.SelectedValue
                Row("Remark") = tbRemarkDt.Text
                Row("DoneComplete") = tbDoneComplete.Text
                Row.EndEdit()
            Else
                'Insert

                If CekDt() = False Then
                    Exit Sub
                End If
                If CekExistData(ViewState("Dt"), "EmpNumb", tbEmpNo.Text) = True Then
                    lbStatus.Text = "Employee " + tbEmpNo.Text + " has already been exist"
                    Exit Sub
                End If

                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("EmpNumb") = tbEmpNo.Text
                dr("EmpName") = tbEmpName.Text
                dr("DayType") = ddlDayTypeDt.SelectedValue
                dr("StartDate") = tbStartDateDt.SelectedDate
                dr("EndDate") = tbEndDateDt.SelectedDate
                dr("StartTime") = tbStartTimeDt.Text
                dr("EndTime") = tbEndTimeDt.Text
                dr("MinuteBruto") = tbMinuteBrutoDt.Text.Replace(",", "")
                dr("MinuteBreak") = tbMinuteBreakDt.Text.Replace(".", "")
                dr("MinuteNetto") = tbMinuteNettoDt.Text.Replace(",", "")
                dr("HourNetto") = tbHourNetto.Text.Replace(",", "")
                dr("FgMealAllowance") = ddlFgAllowanceDt.SelectedValue
                dr("Remark") = tbRemarkDt.Text
                dr("DoneComplete") = tbDoneComplete.Text
                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, PnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            BindGridDt(ViewState("Dt"), GridDt)
            StatusButtonSave(True)
            btnGetData.Visible = True
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
                tbCode.Text = GetAutoNmbr("EOT", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO PEOvertimeHd (TransNmbr, TransDate, Status, Department, DayType, " + _
                "FgSusulan, StartDate, StartTime, EndDate, EndTime, " + _
                "MinuteBruto, MinuteBreak, MinuteNetto, FgMealAllowance, AcknowBy, " + _
                " AcknowJbt, ApprBy, ApprJbt, Remark, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", 'H', " + QuotedStr(ddlDepartment.SelectedValue) + ", " + QuotedStr(ddlDayType.SelectedValue) + _
                ", " + QuotedStr(ddlSusulan.SelectedValue) + ", " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(tbStartTime.Text) + ", " + QuotedStr(Format(tbEndDate.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(tbEndTime.Text) + _
                ", " + tbMinuteBruto.Text.Replace(",", "") + ", " + tbMinuteBreak.Text.Replace(".", "") + ", " + tbMinuteNetto.Text.Replace(",", "") + ", " + QuotedStr(ddlFgMealAllowance.SelectedValue) + ", " + QuotedStr(tbAcknowBy.Text) + _
                ", " + QuotedStr(ddlAcknowledgeJbt.SelectedValue) + ", " + QuotedStr(tbApprBy.Text) + ", " + QuotedStr(ddlApprJbt.SelectedValue) + ", " + QuotedStr(tbRemark.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", GetDate() "

                ViewState("TransNmbr") = tbCode.Text

            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("SELECT Status FROM PEOvertimeHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE PEOvertimeHd SET " + _
                "  Department   = " + QuotedStr(ddlDepartment.SelectedValue) + _
                ", DayType      = " + QuotedStr(ddlDayType.SelectedValue) + _
                ", FgSusulan    = " + QuotedStr(ddlSusulan.SelectedValue) + _
                ", StartDate    = " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + _
                ", StartTime    = " + QuotedStr(tbStartTime.Text) + _
                ", EndDate      = " + QuotedStr(Format(tbEndDate.SelectedValue, "yyyy-MM-dd")) + _
                ", EndTime      = " + QuotedStr(tbEndTime.Text) + _
                ", MinuteBruto  = " + tbMinuteBruto.Text.Replace(",", "") + _
                ", MinuteBreak  = " + tbMinuteBreak.Text.Replace(".", "") + _
                ", MinuteNetto  = " + tbMinuteNetto.Text.Replace(",", "") + _
                ", FgMealAllowance = " + QuotedStr(ddlFgMealAllowance.SelectedValue) + _
                ", AcknowBy     = " + QuotedStr(tbAcknowBy.Text) + _
                ", AcknowJbt    = " + QuotedStr(ddlAcknowledgeJbt.SelectedValue) + _
                ", Apprby       = " + QuotedStr(tbApprBy.Text) + _
                ", ApprJbt      = " + QuotedStr(ddlApprJbt.SelectedValue) + _
                ", Remark       = " + QuotedStr(tbRemark.Text) + _
                ", DatePrep     = GetDate()" + _
                "  WHERE TransNmbr = " + QuotedStr(tbCode.Text)
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
            Dim ConnString As String = ViewState("DBConnection").ToString
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, EmpNumb, DayType, StartDate, StartTime, EndDate, EndTime, MinuteBruto, MinuteBreak, MinuteNetto, HourNetto, FgMealAllowance, Remark, DoneComplete FROM PEOvertimeDt WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt As New DataTable("PEOvertimeDt")

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
            ddlField.SelectedValue = "TransNmbr"
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
            tbEmpNo.Focus()
            btnGetData.Visible = False
        Catch ex As Exception
            lbStatus.Text = "btn add dt error : " + ex.ToString
        End Try
    End Sub
    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ViewState("DigitCurr") = ViewState("DigitHome")
            ViewState("TransNmbr") = ""
            ClearHd()
            Cleardt()
            tbDoneComplete.Text = "N"
            EnableHd(True)
            PnlDt.Visible = True
            GridDt.Columns(1).Visible = False
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
        Dim FDateName, FDateValue, FilterName, FilterValue As String
        Try
            FDateName = "Date, Start Date, End Date"
            FDateValue = "TransDate, StartDate, EndDate"
            FilterName = "Overtime No, Status, Organization, Day Type, Susulan, Start Time, End Time, Minute Bruto, Minute Break, Minute Netto, Meal Allowance, Acknowledge By Name, Acknowledge Job Title, Appr By Name, Appr Job Title, Remark"
            FilterValue = "TransNmbr, Status, DeptName, DayType, FgSusulan, Start Time, EndTime, MinuteBruto, MinuteBreak, MinuteNetto, FgMealAllowance, AcknowByName, AcknowJbt, ApprByName, ApprJbt, Remark"
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
                    ViewState("Status") = GVR.Cells(3).Text
                    If ViewState("Status") = "P" Then
                        GridDt.Columns(1).Visible = True
                        GridDt.Columns(2).Visible = True
                    ElseIf ViewState("Status") = "F" Then
                        GridDt.Columns(1).Visible = False
                        GridDt.Columns(2).Visible = True
                    Else
                        GridDt.Columns(1).Visible = False
                        GridDt.Columns(2).Visible = False
                    End If
                    GridDt.PageIndex = 0
                    FillTextBoxHd(ViewState("TransNmbr"))
                    BindDataDt(ViewState("TransNmbr"))
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, PnlDt, GridDt)
                    'PnlComplete.Visible = False
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
                        GridDt.Columns(1).Visible = False
                        GridDt.Columns(2).Visible = False
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
                        Session("SelectCommand") = "EXEC S_PEFormOvertime '''" + GVR.Cells(2).Text + "'''"
                        Session("ReportFile") = ".../../../Rpt/FormEmpOverTime.frx"
                        Session("DBConnection") = ViewState("DBConnection")
                        AttachScript("openprintdlg();", Page, Me.GetType)
                    Catch ex As Exception
                        lbStatus.Text = "btn print Error = " + ex.ToString
                    End Try
                ElseIf DDL.SelectedValue = "Complete" Then
                    CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                    If CekMenu <> "" Then
                        lbStatus.Text = CekMenu
                        Exit Sub
                    End If
                    index = Convert.ToInt32(e.CommandArgument)
                    GVR = GridView1.Rows(index)

                    If Not GVR.Cells(3).Text = "P" Then
                        lbStatus.Text = MessageDlg("Data Must Post Before Complete")
                        Exit Sub
                    End If

                    Dim Result, SqlString, CurrFilter, Value As String

                    SqlString = "Declare @A VarChar(255) EXEC S_PEOvertimeCompleteAll " + QuotedStr(GVR.Cells(3).Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", @A OUT SELECT @A"
                    Result = SQLExecuteScalar(SqlString, ViewState("DBConnection"))
                    Result = Result.Replace("0", "")
                    If Trim(Result) <> "" Then
                        lbStatus.Text = MessageDlg(Result)
                    End If
                    CurrFilter = tbFilter.Text

                    Value = ddlField.SelectedValue
                    tbFilter.Text = tbCode.Text
                    ddlField.SelectedValue = "TransNmbr"
                    btnSearch_Click(Nothing, Nothing)
                    tbFilter.Text = CurrFilter
                    ddlField.SelectedValue = Value

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
        Dim GVR As GridViewRow = Nothing
        Dim index As Integer
        Try
            'If Not (e.CommandName = "Sort" Or e.CommandName = "Page") Then
            '    index = Convert.ToInt32(e.CommandArgument)
            '    GVR = GridDt.Rows(index)

            'End If

            If e.CommandName = "Closing" Then
                'Dim GVR As GridViewRow
                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))
                'If ViewState("Status") <> "P" Then
                '    lbStatus.Text = MessageDlg("Status Overtime is not Post, cannot close employee")
                '    Exit Sub
                'End If

                If GVR.Cells(16).Text = "Y" Then
                    lbStatus.Text = MessageDlg("Status Overtime has been Completed")
                    Exit Sub
                End If

                
                MultiView1.ActiveViewIndex = 1
                Menu1.Items.Item(1).Selected = True
                
                FillTextBoxComplete(ViewState("TransNmbr"), GVR.Cells(3).Text)
                StatusButtonSave(False)
               
                'PnlComplete.Visible = True

                'ViewState("EmpClose") = GVR.Cells(3).Text



                'Try
                '    Dim sqlstring, result As String
                '    sqlstring = "Declare @A VarChar(255) EXEC S_PEOvertimeCompleteCek '" + tbCode.Text + "', '" + GVR.Cells(3).Text + "', @A OUT SELECT @A"
                '    result = SQLExecuteScalar(sqlstring, ViewState("DBConnection").ToString)

                '    If result.Length > 2 Then
                '        lbStatus.Text = result
                '    Else

                '        Session("FgComplete") = "Overtime"
                '        Session("Nmbr") = tbCode.Text
                '        Session("EmpNo") = GVR.Cells(3).Text
                '        Session("TransDate") = tbDate.SelectedDate
                '        Session("KeyId") = Request.QueryString("KeyId")
                '        Response.Redirect("..\..\Transaction\TrEmpOverTime\FormComplete.aspx")
                '        'AttachScript("openClosingdlg('" + Request.QueryString("KeyId") + "','TrEmpOverTime');", Page, Me.GetType())

                '    End If
                'Catch ex As Exception
                '    lbStatus.Text = "openClosingdlg Error = " + ex.ToString
                'End Try
            ElseIf e.CommandName = "UnClosing" Then
                Dim dr() As DataRow
                'Dim GVR As GridViewRow

                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))
                dr = ViewState("Dt").Select("EmpNumb = " + QuotedStr(GVR.Cells(3).Text))
                ' lbStatus.Text = GVR.Cells(16).Text
                ' Exit Sub
                If ViewState("Status") = "F" Or ViewState("Status") = "P" Then
                    If (GVR.Cells(16).Text = "N") Then
                        lbStatus.Text = MessageDlg("Over Time is not completed")
                        Exit Sub
                    End If
                End If

                ' ViewState("EmpClose") = GVR.Cells(3).Text
                'AttachScript("closing();", Page, Me.GetType)

                Try
                    Dim sqlstring, result As String

                    'sqlstring = "Declare @A VarChar(255) EXEC S_PEOvertimeUnComplete '" + ViewState("TransNmbr") + "', '" + GVR.Cells(3).Text + "'," + CInt(Session(Request.QueryString("KeyId"))("Year")).ToString + "," + CInt(Session(Request.QueryString("KeyId"))("Period")).ToString + ", " + QuotedStr(ViewState("UserId")) + ", @A OUT SELECT @A"
                    sqlstring = "Declare @A VarChar(255) EXEC S_PEOvertimeUnComplete '" + ViewState("TransNmbr") + "', '" + GVR.Cells(3).Text + "', " + QuotedStr(ViewState("UserId")) + ", @A OUT SELECT @A"
                    result = SQLExecuteScalar(sqlstring, ViewState("DBConnection").ToString)
                    If result.Length > 2 Then
                        lbStatus.Text = result
                    Else
                        BindDataDt(ViewState("TransNmbr"))
                        'GridDt.Columns(2).Visible = GVR.Cells(16).Text = "N"
                        GridDt.Columns(0).Visible = False
                        GridDt.Columns(1).Visible = True
                        ViewState("Status") = "P"
                        ViewState("UnClosing") = "UnComplete"
                    End If
                Catch ex As Exception
                    lbStatus.Text = "UnClosing Error = " + ex.ToString
                End Try
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)

            dr = ViewState("Dt").Select("EmpNumb = " + QuotedStr(GVR.Cells(3).Text))
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
            FillTextBoxDt(GVR.Cells(3).Text)
            MovePanel(PnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            ViewState("EmpNo") = GVR.Cells(3).Text
            btnSaveDt.Focus()
            StatusButtonSave(False)
            btnGetData.Visible = False
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
            ViewState("StateHd") = "Insert"
            EnableHd(True)
        Catch ex As Exception
            lbStatus.Text = "btn save all error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnEmp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEmp.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DbConnection")
            Session("filter") = "SELECT * FROM V_MsEmployee WHERE Fg_Active = 'Y' AND Department LIKE '" + ddlDepartment.SelectedValue + "%' AND Emp_No NOT IN (" + QuotedStr(tbAcknowBy.Text) + ", " + QuotedStr(tbApprBy.Text) + ") "
            ResultField = "Emp_No, Emp_Name"
            ViewState("Sender") = "btnEmp"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnEmpAppr1_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbEmpNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbEmpNo.TextChanged
        Dim Dr As DataRow
        Dim Ds As DataSet
        Dim SqlString As String
        Try
            Ds = SQLExecuteQuery("SELECT * FROM V_MsEmployee WHERE Fg_Active = 'Y' AND Department LIKE '" + ddlDepartment.SelectedValue + "%' AND Emp_No NOT IN (" + QuotedStr(tbAcknowBy.Text) + ", " + QuotedStr(tbApprBy.Text) + ") " + " AND Emp_No = " + QuotedStr(tbEmpNo.Text), ViewState("DBConnection").ToString)
            If Ds.Tables(0).Rows.Count = 1 Then
                Dr = Ds.Tables(0).Rows(0)
                tbEmpNo.Text = TrimStr(Dr("Emp_No").ToString)
                tbEmpName.Text = TrimStr(Dr("Emp_Name").ToString)
                ddlDayTypeDt.SelectedValue = ddlDayType.SelectedValue
                tbStartDateDt.SelectedDate = tbStartDate.SelectedDate
                tbEndDateDt.SelectedDate = tbEndDate.SelectedDate
                tbStartTimeDt.Text = tbStartTime.Text
                tbEndTimeDt.Text = tbEndTime.Text
                ddlFgAllowanceDt.SelectedValue = ddlFgMealAllowance.SelectedValue
                tbMinuteBreakDt.Text = tbMinuteBreak.Text.Replace(".", "")

                SqlString = "EXEC S_PEOvertimeGetHourNetto " + tbMinuteNetto.Text.Replace(",", "")
                ViewState("HourNetto") = 0
                ViewState("HourNetto") = SQLExecuteScalar(sqlstring, ViewState("DBConnection"))
                tbHourNetto.Text = FormatNumber(ViewState("HourNetto"), CInt(ViewState("DigitCurr")))

            Else
                tbEmpNo.Text = ""
                tbEmpName.Text = ""
                ddlDayTypeDt.SelectedValue = "Work"
                tbStartDateDt.SelectedDate = ViewState("ServerDate")
                tbEndDateDt.SelectedDate = ViewState("ServerDate")
                tbStartTimeDt.Text = "00:00"
                tbEndTimeDt.Text = "00:00"
                ddlFgAllowanceDt.SelectedValue = "N"
                tbMinuteBreakDt.Text = "0"
                tbHourNetto.Text = "0"
            End If

            tbStartTimeDt_TextChanged(Nothing, Nothing)
            tbEndTimeDt_TextChanged(Nothing, Nothing)
            tbMinuteBreakDt_TextChanged(Nothing, Nothing)
        Catch ex As Exception
            Throw New Exception("tbEmpNo_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnAcknow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAcknow.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DbConnection")
            Session("filter") = "SELECT * FROM V_MsEmployee WHERE Fg_Active = 'Y' AND Fg_Acknowledged = 'Y' AND Emp_No <> " + QuotedStr(tbApprBy.Text)
            ResultField = "Emp_No, Emp_Name, Job_Title"
            ViewState("Sender") = "btnAcknow"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnAcknow_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAcknowBy_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAcknowBy.TextChanged
        Dim Dr As DataRow
        Dim Ds As DataSet
        Try
            Ds = SQLExecuteQuery("SELECT * FROM V_MsEmployee WHERE Fg_Active = 'Y' AND Fg_Acknowledged = 'Y' AND Emp_No <> " + QuotedStr(tbApprBy.Text) + " AND Emp_No = " + QuotedStr(tbAcknowBy.Text), ViewState("DBConnection").ToString)
            If Ds.Tables(0).Rows.Count = 1 Then
                Dr = Ds.Tables(0).Rows(0)
                tbAcknowBy.Text = TrimStr(Dr("Emp_No").ToString)
                tbAcknowByName.Text = TrimStr(Dr("Emp_Name").ToString)
                ddlAcknowledgeJbt.SelectedValue = TrimStr(Dr("Job_Title").ToString)                
            Else
                tbAcknowBy.Text = ""
                tbAcknowByName.Text = ""
                ddlAcknowledgeJbt.SelectedIndex = 0
            End If

        Catch ex As Exception
            Throw New Exception("tbAcknowBy_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnAppr_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAppr.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DbConnection")
            Session("filter") = "SELECT * FROM V_MsEmployee WHERE Fg_Active = 'Y' AND Fg_Approval = 'Y' AND Emp_No <> " + QuotedStr(tbAcknowBy.Text)
            ResultField = "Emp_No, Emp_Name, Job_Title"
            ViewState("Sender") = "btnAppr"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnAppr_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbApprBy_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbApprBy.TextChanged
        Dim Dr As DataRow
        Dim Ds As DataSet
        Try
            Ds = SQLExecuteQuery("SELECT * FROM V_MsEmployee WHERE Fg_Active = 'Y' AND Fg_Approval = 'Y' AND Emp_No <> " + QuotedStr(tbAcknowBy.Text) + " AND Emp_No = " + QuotedStr(tbApprBy.Text), ViewState("DBConnection").ToString)
            If Ds.Tables(0).Rows.Count = 1 Then
                Dr = Ds.Tables(0).Rows(0)
                tbApprBy.Text = TrimStr(Dr("Emp_No").ToString)
                tbApprByName.Text = TrimStr(Dr("Emp_Name").ToString)
                ddlApprJbt.SelectedValue = TrimStr(Dr("Job_Title").ToString)
            Else
                tbApprBy.Text = ""
                tbApprByName.Text = ""
                ddlApprJbt.SelectedIndex = 0
            End If

        Catch ex As Exception
            Throw New Exception("tbApprBy_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbDate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbDate.SelectionChanged
        Try
            tbStartDate.SelectedDate = tbDate.SelectedDate
            tbEndDate.SelectedDate = tbDate.SelectedDate
        Catch ex As Exception
            Throw New Exception("tbDate_SelectionChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbStartDate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbStartDate.SelectionChanged
        Try
            If tbDate.SelectedDate > tbStartDate.SelectedDate Then
                lbStatus.Text = MessageDlg("Your Start Date is earlier than your Transaction Date")
                tbStartDate.SelectedDate = tbDate.SelectedDate
            End If
            If (Not tbEndDate.IsNull) Then
                If tbEndDate.SelectedDate < tbStartDate.SelectedDate Then
                    tbEndDate.SelectedDate = tbStartDate.SelectedDate
                End If
                CekTime()
            End If
            tbMinuteNetto.Text = FormatNumber((CFloat(tbMinuteBruto.Text) - CFloat(tbMinuteBreak.Text)), 0)
        Catch ex As Exception
            Throw New Exception("tbStartDate_SelectionChanged Error : " + ex.ToString)
        End Try        
    End Sub

    Protected Sub tbStartTime_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbStartTime.TextChanged
        Dim StartH, StartM As Double
        Try
            If tbStartTime.Text.Trim <> "" Then
                StartH = CFloat(Strings.Mid(tbStartTime.Text, 1, 2))
                StartM = CFloat(Strings.Mid(tbStartTime.Text, 4, 2))
                If (StartH > 23) Or (StartM > 59) Then
                    lbStatus.Text = MessageDlg("Your Start Time is invalid")
                    tbStartTime.Text = "00:00"
                    Exit Sub
                End If
                CekTime()
            End If
            tbMinuteNetto.Text = FormatNumber((CFloat(tbMinuteBruto.Text) - CFloat(tbMinuteBreak.Text)), 0)
        Catch ex As Exception
            Throw New Exception("tbStartTime_TextChanged Error : " + ex.ToString)
        End Try

    End Sub

    Protected Sub tbEndDate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbEndDate.SelectionChanged
        Try
            If tbEndDate.SelectedDate < tbStartDate.SelectedDate Then
                lbStatus.Text = MessageDlg("Your End Date is earlier than your Start Date")
                tbEndDate.SelectedDate = tbStartDate.SelectedDate                
            End If
            CekTime()
            tbMinuteNetto.Text = FormatNumber(((tbMinuteBruto.Text) - CFloat(tbMinuteBreak.Text)), 0)
        Catch ex As Exception
            Throw New Exception("tbEndDate_SelectionChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbEndTime_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbEndTime.TextChanged
        Dim EndH, EndM As Double
        Try
            If tbEndTime.Text.Trim <> "" Then
                EndH = CFloat(Strings.Mid(tbEndTime.Text, 1, 2))
                EndM = CFloat(Strings.Mid(tbEndTime.Text, 4, 2))
                If (EndH > 23) Or (EndM > 59) Then
                    lbStatus.Text = MessageDlg("Your End Time is invalid")
                    tbEndTime.Text = "00:00"
                    Exit Sub
                End If
                CekTime()
            End If
            tbMinuteNetto.Text = FormatNumber((CFloat(tbMinuteBruto.Text) - CFloat(tbMinuteBreak.Text)), 0)
        Catch ex As Exception
            Throw New Exception("tbEndTime_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbMinuteBruto_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbMinuteBruto.TextChanged
        Try
            tbMinuteNetto.Text = FormatNumber((CFloat(tbMinuteBruto.Text) - CFloat(tbMinuteBreak.Text)), 0)
        Catch ex As Exception
            Throw New Exception("tbMinuteBruto_TextChanged Error : " + ex.ToString)
        End Try        
    End Sub

    Protected Sub tbMinuteBreak_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbMinuteBreak.TextChanged
        Try
            If tbMinuteBreak.Text.Trim = "" Then
                tbMinuteBreak.Text = 0
            End If
            tbMinuteNetto.Text = FormatNumber(((tbMinuteBruto.Text) - CFloat(tbMinuteBreak.Text)), 0)
        Catch ex As Exception
            Throw New Exception("tbMinuteBreak_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbStartDateDt_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbStartDateDt.SelectionChanged
        Try
            If tbDate.SelectedDate > tbStartDateDt.SelectedDate Then
                lbStatus.Text = MessageDlg("Your Start Date is earlier than your Transaction Date")
                tbStartDateDt.SelectedDate = tbDate.SelectedDate
            End If
            If Not tbEndDateDt.IsNull Then
                If tbEndDateDt.SelectedDate < tbStartDateDt.SelectedDate Then
                    tbEndDateDt.SelectedDate = tbStartDateDt.SelectedDate
                End If
                CekTimeDt()
            End If
            tbMinuteNettoDt.Text = FormatNumber((CFloat(tbMinuteBrutoDt.Text) - CFloat(tbMinuteBreakDt.Text)), 0)

            GetHourNetto()
        Catch ex As Exception
            Throw New Exception("tbStartDateDt_SelectionChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbStartTimeDt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbStartTimeDt.TextChanged
        Dim StartH, StartM As Double
        Try
            If tbStartTimeDt.Text.Trim <> "" Then
                StartH = CFloat(Strings.Mid(tbStartTimeDt.Text, 1, 2))
                StartM = CFloat(Strings.Mid(tbStartTimeDt.Text, 4, 2))
                If (StartH > 23) Or (StartM > 59) Then
                    lbStatus.Text = MessageDlg("Your Start Time is invalid")
                    tbStartTimeDt.Text = "00:00"
                    Exit Sub
                End If
                CekTimeDt()
            End If
            tbMinuteNettoDt.Text = FormatNumber((CFloat(tbMinuteBrutoDt.Text) - CFloat(tbMinuteBreakDt.Text)), 0)

            GetHourNetto()
        Catch ex As Exception
            Throw New Exception("tbStartTimeDt_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbEndDateDt_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbEndDateDt.SelectionChanged
        Try
            If tbEndDateDt.SelectedDate < tbStartDateDt.SelectedDate Then
                lbStatus.Text = MessageDlg("Your End Date is earlier than your Start Date")
                tbEndDateDt.SelectedDate = tbStartDateDt.SelectedDate
            End If
            CekTimeDt()
            tbMinuteNettoDt.Text = FormatNumber((CFloat(tbMinuteBrutoDt.Text) - CFloat(tbMinuteBreakDt.Text)), 0)

            GetHourNetto()
        Catch ex As Exception
            Throw New Exception("tbEndDateDt_SelectionChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbEndTimeDt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbEndTimeDt.TextChanged
        Dim EndH, EndM As Double
        Try
            If tbEndTimeDt.Text.Trim <> "" Then
                EndH = CFloat(Strings.Mid(tbEndTimeDt.Text, 1, 2))
                EndM = CFloat(Strings.Mid(tbEndTimeDt.Text, 4, 2))
                If (EndH > 23) Or (EndM > 59) Then
                    lbStatus.Text = MessageDlg("Your End Time is invalid")
                    tbEndTimeDt.Text = "00:00"
                    Exit Sub
                End If
                CekTimeDt()
            End If
            tbMinuteNettoDt.Text = FormatNumber((CFloat(tbMinuteBrutoDt.Text) - CFloat(tbMinuteBreakDt.Text)), 0)

            GetHourNetto()
        Catch ex As Exception
            Throw New Exception("tbEndTimeDt_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbMinuteBrutoDt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbMinuteBrutoDt.TextChanged
        Try
            tbMinuteNettoDt.Text = FormatNumber((CFloat(tbMinuteBrutoDt.Text) - CFloat(tbMinuteBreakDt.Text)), 0)
        Catch ex As Exception
            Throw New Exception("tbMinuteBrutoDt_TextChanged Error : " + ex.ToString)
        End Try        
    End Sub

    Protected Sub tbMinuteBreakDt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbMinuteBreakDt.TextChanged
        Try
            If tbMinuteBreakDt.Text.Trim = "" Then
                tbMinuteBreakDt.Text = 0
            End If
            tbMinuteNettoDt.Text = FormatNumber((CFloat(tbMinuteBrutoDt.Text) - CFloat(tbMinuteBreakDt.Text)), 0)

            Dim Dr As DataRow
            Dim Ds As DataSet

            Ds = SQLExecuteQuery("EXEC S_PEOvertimeGetHourNetto " + tbMinuteNettoDt.Text.Replace(",", ""), ViewState("DBConnection").ToString)
            If Ds.Tables(0).Rows.Count = 1 Then
                Dr = Ds.Tables(0).Rows(0)
                tbHourNetto.Text = TrimStr(FormatNumber(Dr("HourNetto").ToString, ViewState("DigitCurr")))
            Else
                tbHourNetto.Text = "0"
            End If

        Catch ex As Exception
            Throw New Exception("tbMinuteBreakDt_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbMinuteNettoDt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbMinuteNettoDt.TextChanged
        'Dim Dr As DataRow
        'Dim Ds As DataSet
        Try
            'Ds = SQLExecuteQuery("EXEC S_PEOvertimeGetHourNetto " + tbMinuteNettoDt.Text.Replace(",", ""), ViewState("DBConnection").ToString)
            'If Ds.Tables(0).Rows.Count <> 0 Then
            '    Dr = Ds.Tables(0).Rows(0)
            '    tbHourNetto.Text = FormatNumber(TrimStr(Dr("HourNetto").ToString), CInt(ViewState("DigitCurr")))
            'Else
            '    tbHourNetto.Text = "0"
            'End If

            GetHourNetto()
        Catch ex As Exception
            Throw New Exception("tbMinuteNettoDt_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub CekTime()
        'Dim StartH, StartM, EndH, EndM, iStart, iEnd, iTot, iDay As Double
        'Dim Sdd, Smm, Syy, Edd, Emm, Eyy As Integer
        Dim Dr As DataRow
        Dim Ds As DataSet
        Try
            'If (tbEndTime.Text.Trim <> "") Or (tbEndTime.Text.Trim <> "00:00") Then
            '    StartH = CFloat(Strings.Mid(tbStartTime.Text, 1, 2))
            '    StartM = CFloat(Strings.Mid(tbStartTime.Text, 4, 2))
            '    EndH = CFloat(Strings.Mid(tbEndTime.Text, 1, 2))
            '    EndM = CFloat(Strings.Mid(tbEndTime.Text, 4, 2))
            '    Syy = tbStartDate.SelectedDate.Year
            '    Smm = tbStartDate.SelectedDate.Month
            '    Sdd = tbStartDate.SelectedDate.Day
            '    Eyy = tbEndDate.SelectedDate.Year
            '    Emm = tbEndDate.SelectedDate.Month
            '    Edd = tbEndDate.SelectedDate.Day
            '    If (tbStartDate.SelectedDate = tbEndDate.SelectedDate) And ((EndH < StartH) Or ((StartH = EndH) And (EndM < StartM))) Then
            '        lbStatus.Text = MessageDlg("Your End Time is earlier than your Start Time")
            '        tbEndTime.Text = "00:00"
            '        tbMinuteBruto.Text = 0
            '        Exit Sub
            '    End If
            '    iDay = (CFloat(tbEndDate.Text) - CFloat(tbStartDate.Text)) * 24 * 60
            '    iStart = (StartH * 60) + StartM
            '    lbStatus.Text = StartH
            '    Exit Sub
            '    iEnd = (EndH * 60) + EndM
            '    If iEnd > iStart Then
            '        iTot = iEnd - iStart + iDay
            '    Else
            '        iTot = iStart - iEnd + iDay
            '    End If
            '    tbMinuteBruto.Text = iTot
            'End If

            Ds = SQLExecuteQuery("EXEC S_PEOvertimeCekTime " + QuotedStr(tbStartDate.SelectedDate) + ", " + QuotedStr(tbEndDate.SelectedDate) + ", " + QuotedStr(tbStartTime.Text) + ", " + QuotedStr(tbEndTime.Text), ViewState("DBConnection").ToString)
            If Ds.Tables(0).Rows.Count <> 0 Then
                Dr = Ds.Tables(0).Rows(0)
                If TrimStr(Dr("Pesan").ToString) <> "" Then
                    lbStatus.Text = MessageDlg(TrimStr(Dr("Pesan").ToString))
                    tbEndTime.Text = "00:00"
                    tbMinuteBruto.Text = 0
                    Exit Sub
                Else
                    tbMinuteBruto.Text = TrimStr(FormatNumber(Dr("ITot"), 0))
                End If
            Else
                tbMinuteBruto.Text = 0
            End If
        Catch ex As Exception
            Throw New Exception("CekTime Error " + ex.ToString)
        End Try
    End Sub

    Private Sub CekTimeDt()
        'Dim StartH, StartM, EndH, EndM, iStart, iEnd, iTot, iDay As Double
        'Dim Smm, Syy, Sdd, Edd, Emm, Eyy As Integer
        Dim Dr As DataRow
        Dim Ds As DataSet
        Try
            'If tbEndTimeDt.Text.Trim <> "" Then
            '    StartH = CFloat(Strings.Mid(tbStartTimeDt.Text, 1, 2))
            '    StartM = CFloat(Strings.Mid(tbStartTimeDt.Text, 4, 2))
            '    EndH = CFloat(Strings.Mid(tbEndTimeDt.Text, 1, 2))
            '    EndM = CFloat(Strings.Mid(tbEndTimeDt.Text, 4, 2))
            '    Syy = tbStartDateDt.SelectedDate.Year
            '    Smm = tbStartDateDt.SelectedDate.Month
            '    Sdd = tbStartDateDt.SelectedDate.Day
            '    Eyy = tbEndDateDt.SelectedDate.Year
            '    Emm = tbEndDateDt.SelectedDate.Month
            '    Edd = tbEndDateDt.SelectedDate.Day
            '    If (tbStartDateDt.SelectedDate = tbEndDateDt.SelectedDate) And ((EndH < StartH) Or ((StartH = EndH) And (EndM < StartM))) Then
            '        lbStatus.Text = MessageDlg("Your End Time is earlier than your Start Time")
            '        tbEndTimeDt.Text = "00:00"
            '        tbMinuteBrutoDt.Text = 0
            '        tbMinuteBrutoDt.Focus()
            '        Exit Sub
            '    End If
            '    iDay = (CFloat(tbEndDateDt.Text) - CFloat(tbStartDateDt.Text)) * 24 * 60
            '    iStart = (StartH * 60) + StartM
            '    iEnd = (EndH * 60) + EndM
            '    If iEnd > iStart Then
            '        iTot = iEnd - iStart + iDay
            '    Else
            '        iTot = iStart - iEnd + iDay
            '    End If
            '    tbMinuteBrutoDt.Text = iTot
            'End If

            Ds = SQLExecuteQuery("EXEC S_PEOvertimeCekTime " + QuotedStr(tbStartDateDt.SelectedDate) + ", " + QuotedStr(tbEndDateDt.SelectedDate) + ", " + QuotedStr(tbStartTimeDt.Text) + ", " + QuotedStr(tbEndTimeDt.Text), ViewState("DBConnection").ToString)
            If Ds.Tables(0).Rows.Count <> 0 Then
                Dr = Ds.Tables(0).Rows(0)
                If TrimStr(Dr("Pesan").ToString) <> "" Then
                    lbStatus.Text = MessageDlg(TrimStr(Dr("Pesan").ToString))
                    tbEndTimeDt.Text = "00:00"
                    tbMinuteBrutoDt.Text = 0
                    Exit Sub
                Else
                    tbMinuteBrutoDt.Text = TrimStr(FormatNumber(Dr("ITot"), 0))
                End If
            Else
                tbMinuteBrutoDt.Text = 0
            End If
        Catch ex As Exception
            Throw New Exception("CekTime Error " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnGetData_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetData.Click
        Dim ResultField, CriteriaField As String
        Try
            If CekHd() = False Then
                btnGetData.Focus()
                Exit Sub
            End If

            Session("Result") = Nothing
            Session("filter") = "SELECT * FROM V_MsEmployee WHERE Fg_Active = 'Y' AND Department LIKE  '" + ddlDepartment.SelectedValue + "%' AND Emp_No NOT IN (" + QuotedStr(tbAcknowBy.Text) + ", " + QuotedStr(tbApprBy.Text) + ") "
            ResultField = "Emp_No, Emp_Name"
            Session("DBConnection") = ViewState("DBConnection")
            CriteriaField = "Emp_No, Emp_Name"
            ViewState("CriteriaField") = CriteriaField.Split(",")
            Session("Column") = ResultField.Split(",")
            ViewState("Sender") = "btnGetData"
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnGetData_Click Data Error : " + ex.ToString
        End Try
    End Sub

    Private Sub GetHourNetto()
        Dim Dr As DataRow
        Dim Ds As DataSet
        Try
            Ds = SQLExecuteQuery("EXEC S_PEOvertimeGetHourNetto " + tbMinuteNettoDt.Text.Replace(",", ""), ViewState("DBConnection").ToString)
            If Ds.Tables(0).Rows.Count <> 0 Then
                Dr = Ds.Tables(0).Rows(0)
                tbHourNetto.Text = FormatNumber(TrimStr(Dr("HourNetto").ToString), CInt(ViewState("DigitCurr")))
            Else
                tbHourNetto.Text = "0"
            End If
        Catch ex As Exception
            Throw New Exception("tbMinuteNettoDt_TextChanged Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        Dim index As Integer
        Try
            index = Int32.Parse(e.Item.Value)
            MultiView1.ActiveViewIndex = index
        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancel2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel2.Click
        Try
            MultiView1.ActiveViewIndex = 0
            Menu1.Items.Item(0).Selected = True
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "btnCancelWO_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnOK2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK2.Click
        Dim SQLString, result As String
        Dim PrimaryKey() As String
        PrimaryKey = ViewState("TransNmbr").ToString.Trim.Split("|")

        ' If Session("FgComplete") = "Overtime" Then
        Dim GLYear, GLPeriod As Integer
        GLYear = ViewState("GLYear")
        GLPeriod = ViewState("GLPeriod")
        SQLString = "DECLARE @A VarChar(255) EXEC S_PEOvertimeComplete " + QuotedStr(lblOvertime.Text) + ", " + QuotedStr(tbEmpNoC.Text) + ", " + QuotedStr(Format(tbActStartDateC.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(tbActStartTimeC.Text) + ", " + QuotedStr(Format(tbActEndDateC.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(tbActEndTimeC.Text) + ", " + tbActMinuteBrutoC.Text.Replace(",", "") + ", " + tbActMinuteBreakC.Text + ", " + tbActMinuteNettoC.Text.Replace(",", "") + ", " + tbActHournettoC.Text + ", " + QuotedStr(ddlActFgMealAllowanceC.SelectedValue) + ", " + tbOTHour.Text + ", " + GLYear.ToString + ", " + GLPeriod.ToString + ", " + QuotedStr(ViewState("UserId")) + ", @A OUT SELECT @A "
        result = SQLExecuteScalar(SQLString, ViewState("DBConnection").ToString)

        'Dim Dr As DataRow
        Dim Dt As DataTable
        Dim SQL As String

        SQL = "SELECT A.TransNmbr FROM PEOvertimeHd A INNER JOIN PEOvertimeDt B ON A.TransNmbr = B.TransNmbr WHERE A.Status = 'P' AND B.DoneComplete = 'N' AND A.TransNmbr = " + QuotedStr(tbCode.Text)
        Dt = SQLExecuteQuery(SQL, ViewState("DBConnection").ToString).Tables(0)
        
        If Dt.Rows.Count <> 0 Then
        Else
            Dim SQL2 As String
            SQL2 = " UPDATE PEOvertimeHd SET Status = 'F' WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            SQLExecuteNonQuery(SQL2, ViewState("DBConnection").ToString)
        End If

        If result.Length > 2 Then
            lbStatus.Text = result
        Else
            MultiView1.ActiveViewIndex = 0
            Menu1.Items.Item(0).Selected = True
            StatusButtonSave(False)
            'BindGridDt(Dt, GridDt)
            BindDataDt(tbCode.Text)
            GridDt.Columns(0).Visible = False
        End If
        '  End If
    End Sub

    Protected Sub FillTextBoxComplete(ByVal Nmbr As String, ByVal EmpNo As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringComplete, "TransNmbr = " + QuotedStr(Nmbr) + " AND EmpNumb = " + QuotedStr(EmpNo), ViewState("DBConnection").ToString)
            lblOvertime.Text = tbCode.Text
            tbDateC.SelectedDate = tbDate.SelectedDate
            BindToDate(tbStartDateC, Dt.Rows(0)("StartDate").ToString)
            BindToDate(tbEndDateC, Dt.Rows(0)("EndDate").ToString)
            BindToText(tbStartTimeC, Dt.Rows(0)("StartTime").ToString)
            BindToText(tbEndTimeC, Dt.Rows(0)("EndTime").ToString)
            BindToText(tbMinuteBrutoC, Dt.Rows(0)("MinuteBruto").ToString)
            BindToText(tbMinuteBreakC, Dt.Rows(0)("MinuteBreak").ToString)
            BindToText(tbMinuteNettoC, Dt.Rows(0)("MinuteNetto").ToString)
            BindToText(tbEmpNoC, Dt.Rows(0)("EmpNumb").ToString)
            BindToText(tbEmpNameC, Dt.Rows(0)("EmpName").ToString)
            BindToDropList(ddlDayTypeC, Dt.Rows(0)("DayType").ToString)
            BindToDate(tbActStartDateC, Dt.Rows(0)("ActStartDate").ToString)
            BindToDate(tbActEndDateC, Dt.Rows(0)("ActEndDate").ToString)
            BindToText(tbActStartTimeC, Dt.Rows(0)("ActStartHour").ToString)
            BindToText(tbActEndTimeC, Dt.Rows(0)("ActEndHour").ToString)
            BindToText(tbActMinuteBrutoC, Dt.Rows(0)("ActMinuteBruto").ToString)
            BindToText(tbActMinuteBreakC, Dt.Rows(0)("ActMinuteBreak").ToString)
            BindToText(tbActMinuteNettoC, Dt.Rows(0)("ActMinuteNetto").ToString)
            BindToText(tbActHournettoC, FormatNumber(Dt.Rows(0)("ActHourNetto").ToString, 2))
            BindToDropList(ddlActFgMealAllowanceC, Dt.Rows(0)("ActFgMealAllowance").ToString)
            BindToText(tbOTHour, FormatNumber(Dt.Rows(0)("OTHour").ToString, 2))

            Dim Dr As DataRow
            Dim Ds As DataSet

            Ds = SQLExecuteQuery("EXEC S_PEOverTimeInfoAbs " + QuotedStr(EmpNo) + ", '" + Format(tbActStartDateC.SelectedValue, "yyyy-MM-dd") + "' ", ViewState("DBConnection").ToString)
            If Ds.Tables(0).Rows.Count = 1 Then
                Dr = Ds.Tables(0).Rows(0)
                If tbActStartTimeC.Text < TrimStr(Dr("AbsIn").ToString) Then
                    tbActStartTimeC.Text = TrimStr(Dr("AbsIn").ToString)
                End If
                If tbActEndDateC.SelectedDate > Dr("AbsDateOut").ToString Then
                    tbActEndDateC.SelectedDate = Dr("AbsDateOut").ToString
                    tbActEndTimeC.Text = TrimStr(Dr("AbsOut").ToString)
                ElseIf tbActEndDateC.SelectedDate = Dr("AbsDateOut").ToString Then
                    If tbActEndTimeC.Text > TrimStr(Dr("AbsOut").ToString) Then
                        tbActEndTimeC.Text = TrimStr(Dr("AbsOut").ToString)
                    End If
                End If
            End If

            'CekActTime()
            LoadAbsInfo()
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Private Sub LoadAbsInfo()
        Dim Dr As DataRow
        Dim Ds As DataSet
        Try
            Ds = SQLExecuteQuery("EXEC S_PEOverTimeInfoAbs " + QuotedStr(tbEmpNo.Text) + ", '" + Format(tbActStartDateC.SelectedValue, "yyyy-MM-dd") + "' ", ViewState("DBConnection").ToString)
            If Ds.Tables(0).Rows.Count = 1 Then
                Dr = Ds.Tables(0).Rows(0)
                tbTimeIn.Text = TrimStr(Dr("AbsIn").ToString)
                TbTimeOut.Text = TrimStr(Dr("AbsOut").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("LoadAbsInfo Error " + ex.ToString)
        End Try
    End Sub

    Private Sub CekActTime()
        Dim StartH, StartM, EndH, EndM, iStart, iEnd, iTot, iDay As Double
        Dim Sdd, Smm, Syy, Edd, Emm, Eyy As Integer
        Dim Dr As DataRow
        Dim Ds As DataSet
        Try
            If tbActEndTimeC.Text.Trim <> "" Then
                'StartH = CFloat(Strings.Mid(tbActStartTime.Text, 1, 2))
                'StartM = CFloat(Strings.Mid(tbActStartTime.Text, 4, 2))
                'EndH = CFloat(Strings.Mid(tbActEndTime.Text, 1, 2))
                'EndM = CFloat(Strings.Mid(tbActEndTime.Text, 4, 2))
                'Syy = tbActStartDate.SelectedDate.Year
                'Smm = tbActStartDate.SelectedDate.Month
                'Sdd = tbActStartDate.SelectedDate.Day
                'Eyy = tbActEndDate.SelectedDate.Year
                'Emm = tbActEndDate.SelectedDate.Month
                'Edd = tbActEndDate.SelectedDate.Day
                If (tbActStartDateC.SelectedDate = tbStartDate.SelectedDate) And (tbStartTime.Text > tbActStartTimeC.Text) Then
                    lbStatus.Text = MessageDlg("Your Act. Start Hour is earlier than your Start Time")
                    tbActStartTimeC.Text = tbStartTime.Text
                    StartH = CFloat(Strings.Mid(tbActStartTimeC.Text, 1, 2))
                    StartM = CFloat(Strings.Mid(tbActStartTimeC.Text, 4, 2))
                End If
                If (tbActStartDateC.SelectedDate = tbActEndDateC.SelectedDate) And ((EndH < StartH) Or ((StartH = EndH) And (EndM < StartM))) Then
                    lbStatus.Text = MessageDlg("Your Act. End Hour is earlier than your Start Time")
                    tbActEndTimeC.Text = "00:00"
                    tbActMinuteBrutoC.Text = "0"
                    tbActEndTimeC.Focus()
                    Exit Sub
                End If
                'iDay = (CFloat(tbActEndDate.Text) - CFloat(tbActStartDate.Text)) * 24 * 60
                'iStart = (StartH * 60) + StartM
                'iEnd = (EndH * 60) + EndM
                'iTot = iEnd - iStart + iDay
                'tbActMinuteBruto.Text = iTot

                Ds = SQLExecuteQuery("EXEC S_PEOvertimeCekTime " + QuotedStr(tbActStartDateC.SelectedDate) + ", " + QuotedStr(tbActEndDateC.SelectedDate) + ", " + QuotedStr(tbActStartTimeC.Text) + ", " + QuotedStr(tbActEndTimeC.Text), ViewState("DBConnection").ToString)
                If Ds.Tables(0).Rows.Count <> 0 Then
                    Dr = Ds.Tables(0).Rows(0)
                    If TrimStr(Dr("Pesan").ToString) <> "" Then
                        lbStatus.Text = MessageDlg(TrimStr(Dr("Pesan").ToString))
                        tbActEndTimeC.Text = "00:00"
                        tbActMinuteBrutoC.Text = 0
                        Exit Sub
                    Else
                        tbActMinuteBrutoC.Text = TrimStr(FormatNumber(Dr("ITot"), 0))
                    End If
                Else
                    tbActMinuteBrutoC.Text = 0
                End If
            End If
        Catch ex As Exception
            Throw New Exception("CekTime Error " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbActStartDateC_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbActStartDateC.SelectionChanged
        Try
            If tbStartDate.SelectedDate > tbActStartDateC.SelectedDate Then
                lbStatus.Text = MessageDlg("Your Act. Start Date is earlier than your Start Date")
                tbActStartDateC.SelectedDate = tbStartDate.SelectedDate
            End If
            If (Not tbActEndDateC.IsNull) Then
                If tbActEndDateC.SelectedDate < tbActStartDateC.SelectedDate Then
                    tbActEndDateC.SelectedDate = tbActStartDateC.SelectedDate
                End If
                CekActTime()
            End If
            LoadAbsInfo()
            tbActMinuteNettoC.Text = FormatNumber(CFloat(tbActMinuteBrutoC.Text) - CFloat(tbActMinuteBreakC.Text), 0)

            LoadHour()
        Catch ex As Exception
            Throw New Exception("tbActStartDate_SelectionChanged Error " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbActEndDateC_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbActEndDateC.SelectionChanged
        Try
            If tbActEndDateC.SelectedDate < tbActStartDateC.SelectedDate Then
                lbStatus.Text = MessageDlg("Your Act. End Date is earlier than your Act. Start Date")
                tbActEndDateC.SelectedDate = tbActStartDateC.SelectedDate
            End If
            CekActTime()
            tbActMinuteNettoC.Text = FormatNumber(CFloat(tbActMinuteBrutoC.Text) - CFloat(tbActMinuteBreakC.Text), 0)

            LoadHour()
        Catch ex As Exception
            Throw New Exception("tbActEndDate_SelectionChanged Error " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbActStartTimeC_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbActStartTimeC.TextChanged
        Dim StartH, StartM As Double
        Try
            If tbActStartTimeC.Text.Trim <> "" Then
                StartH = CFloat(Strings.Mid(tbActStartTimeC.Text, 1, 2))
                StartM = CFloat(Strings.Mid(tbActStartTimeC.Text, 4, 2))
                If (StartH > 23) Or (StartM > 59) Then
                    lbStatus.Text = MessageDlg("Your Act. Start Hour is invalid")
                    tbActStartTimeC.Text = "00:00"
                    tbActStartTimeC.Focus()
                    Exit Sub
                End If
                CekActTime()
            End If
            tbActMinuteNettoC.Text = FormatNumber(CFloat(tbActMinuteBrutoC.Text) - CFloat(tbActMinuteBreakC.Text), 0)
            LoadHour()
            tbActEndTimeC.Focus()
        Catch ex As Exception
            Throw New Exception("tbActStartTime_TextChanged Error " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbActEndTimeC_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbActEndTimeC.TextChanged
        Dim EndH, EndM As Double
        Try
            If tbActEndTimeC.Text.Trim <> "" Then
                EndH = CFloat(Strings.Mid(tbActEndTimeC.Text, 1, 2))
                EndM = CFloat(Strings.Mid(tbActEndTimeC.Text, 4, 2))
                If (EndH > 23) Or (EndM > 59) Then
                    lbStatus.Text = MessageDlg("Your Act. End Time is invalid")
                    tbActEndTimeC.Text = "00:00"
                    tbActEndTimeC.Focus()
                    Exit Sub
                End If
                CekActTime()
            End If
            tbActMinuteNettoC.Text = FormatNumber(CFloat(tbActMinuteBrutoC.Text) - CFloat(tbActMinuteBreakC.Text), 0)
            LoadHour()
        Catch ex As Exception
            Throw New Exception("tbActEndTime_TextChanged Error " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbActMinuteBreakC_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbActMinuteBreakC.TextChanged
        Try
            If tbActMinuteBreakC.Text.Trim = "" Then
                tbActMinuteBreakC.Text = "0"
            End If
            tbActMinuteNettoC.Text = FormatNumber(CFloat(tbActMinuteBrutoC.Text) - CFloat(tbActMinuteBreakC.Text), 0)

            LoadHour()
            tbActMinuteBreakC.Focus()
        Catch ex As Exception
            Throw New Exception("tbActMinuteBreak_TextChanged Error " + ex.ToString)
        End Try
    End Sub

    Private Sub LoadHour()
        Dim Dr, Dw As DataRow
        Dim Ds, Dt As DataSet
        Try
            Ds = SQLExecuteQuery("EXEC S_PEOvertimeGetHourNetto " + tbActMinuteNettoC.Text.Replace(",", ""), ViewState("DBConnection").ToString)
            If Ds.Tables(0).Rows.Count = 1 Then
                Dr = Ds.Tables(0).Rows(0)
                tbActHournettoC.Text = TrimStr(FormatNumber(Dr("HourNetto").ToString, 2))
            End If

            'netto change
            Dt = SQLExecuteQuery("EXEC S_PEOvertimeGetHourOT " + QuotedStr(ddlDayType.SelectedValue) + ", " + tbActHournettoC.Text, ViewState("DBConnection").ToString)
            If Dt.Tables(0).Rows.Count = 1 Then
                Dw = Dt.Tables(0).Rows(0)
                tbOTHour.Text = TrimStr(FormatNumber(Dw("HourOT").ToString, 2))
            End If

        Catch ex As Exception
            Throw New Exception("LoadAbsInfo Error " + ex.ToString)
        End Try
    End Sub
End Class

