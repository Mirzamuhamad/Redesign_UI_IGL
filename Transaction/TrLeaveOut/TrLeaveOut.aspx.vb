Imports System.Data
Imports System.Data.SqlClient

Partial Class Transaction_TrLeaveOut_TrLeaveOut
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "SELECT DISTINCT Nmbr, TransNmbr,TransDate, Status, LeaveCategory, Remark, Employee_No, Employee_Name, Done_Complete, UserPrep, DatePrep, UserAppr, DateAppr FROM V_PELeaveOutHd"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Try
            If Not IsPostBack Then
                InitProperty()
                ViewState("SetLeaveType") = True
                SetInit()
                Session("AdvanceFilter") = ""
            End If
            lbStatus.Text = ""
            'BtnGo.Visible = ddlCommand.Visible
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If

            If Not Session("Result") Is Nothing Then
                'EmpNumb, EmpName, Department, HireDate, SubSectionCode, JobTitle, LeaveType, FgLess1Day, StartDate, EndDate, DayTotal, DayHoliday, DayDispensasi, DayMass, DayTaken, ContactAddr, ContactPhone, ReasonLeave, HireDate, Department, StrDayTotal, StrDayHoliday, StrDayDispensasi, StrDayMass, StrDayTaken"
                If ViewState("Sender") = "btnEmp" Then
                    tbEmpNumb.Text = Session("Result")(0).ToString
                    tbEmpName.Text = Session("Result")(1).ToString
                    BindToDropList(ddlDepartment, Session("Result")(2).ToString)
                    BindToDate(tbHireDate, Session("Result")(3).ToString)
                    tbEmpNumb_TextChanged(Nothing, Nothing)
                ElseIf ViewState("Sender") = "btnGetDt" Then
                    Dim drResult, dr As DataRow
                    Dim Row As DataRow()

                    For Each drResult In Session("Result").Rows
                        Row = ViewState("Dt").Select("EmpNumb = " + QuotedStr(drResult("EmpNumb")))
                        If Row.Count = 0 Then
                            'EmpNumb, EmpName, HireDate, SubSectionCode, SubSectionName, JobTitle, JobTitleName, LeaveType, LeaveTypeName, FgLess1Day, StartDate, StartTime, EndDate, EndTime, DayTotal, DayHoliday, DayDispensasi, DayMass, DayTaken, ContactAddr, , ContactPhone, ReasonLeave, HireDate, Department
                            dr = ViewState("Dt").NewRow
                            dr("EmpNumb") = drResult("EmpNumb")
                            dr("EmpName") = drResult("EmpName")
                            dr("HireDate") = drResult("HireDate")
                            dr("StartDate") = drResult("StartDate")
                            dr("EndDate") = drResult("EndDate")
                            dr("StartTime") = drResult("StartTime")
                            dr("EndTime") = drResult("EndTime")
                            dr("Department") = drResult("Department")
                            dr("DepartmentName") = drResult("DepartmentName")
                            dr("JobTitle") = drResult("JobTitleCode")
                            dr("JobTitleName") = drResult("JobTitleName")
                            dr("LeaveType") = drResult("LeaveType")
                            dr("LeaveTypeName") = drResult("LeaveTypeName")
                            dr("FgLess1Day") = "N"
                            dr("QtyTotal") = FormatFloat(drResult("DayTotal"), ViewState("DigitQty"))
                            dr("QtyDispensasi") = FormatFloat(drResult("DayDispensasi"), ViewState("DigitQty"))
                            dr("QtyHoliday") = FormatFloat(drResult("DayHoliday"), ViewState("DigitQty"))
                            dr("QtyTaken") = FormatFloat(drResult("DayTaken"), ViewState("DigitQty"))
                            dr("ReasonLeave") = drResult("ReasonLeave")
                            dr("DoneComplete") = "N"
                            ViewState("Dt").Rows.Add(dr)
                        End If
                    Next
                    BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
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
        Try
            FillRange(ddlRange)
            'FillCombo(ddlSubSection, "EXEC S_GetSubSection", True, "Sub_Section_Code", "Sub_Section_Name", ViewState("DBConnection"))
            FillCombo(ddlJobTitle, "EXEC S_GetJobTitle", True, "JobTtlCode", "JobTtlName", ViewState("DBConnection"))
            FillCombo(ddlDepartment, "EXEC S_GetDepartment", True, "Department_Code", "Department_Name", ViewState("DBConnection"))
            'FillCombo(ddlLeaveType, "EXEC S_GetLeaves", True, "Leave_Code", "Leave_Name", ViewState("DBConnection"))

            ViewState("SortExpression") = Nothing
            ViewState("DigitCurr") = 0
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
            If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
                ddlCommand.Items.Add("Print")
                ddlCommand2.Items.Add("Print")
            End If
            If Request.QueryString("ContainerId").ToString = "LeaveOutId" Then
                ViewState("StrKode") = "LO"
                lbTitle.Text = "Employee Leave"
                ViewState("Type") = "Leave"
            Else
                ViewState("StrKode") = "LP"
                lbTitle.Text = "Employee Permission"
                ViewState("Type") = "Permission"
            End If
            tbTotal.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbTotal.Attributes.Add("OnBlur", "setformat();")

        Catch ex As Exception
            Throw New Exception("Set Init Error : " + ex.ToString)
        End Try
    End Sub
    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim StrFilter, GetStringHd1 As String
        Try
            StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue, ddlRange.SelectedValue)
            If AdvanceFilter.Length > 1 Then
                StrFilter = AdvanceFilter
            End If
            GetStringHd1 = "Select * From V_PELeaveOutHd WHERE LeaveCategory = " + QuotedStr(ViewState("Type").ToString)
            
            DT = BindDataTransaction(GetStringHd1, StrFilter, ViewState("DBConnection").ToString)
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
        Return "SELECT * FROM V_PELeaveOutDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        Try
            GridView1.PageIndex = 0
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
                Session("SelectCommand") = "EXEC S_PEFormLeaveOut " + Result
                Session("ReportFile") = ".../../../Rpt/FormLeaveOut.frx"
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
                        Result = ExecSPCommandGo(ActionValue, "S_PELeaveOut", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            ddlLeaveCategory.Enabled = State

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
            GridDt.Columns(1).Visible = ViewState("Status") = "P"

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
            tbDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            ddlLeaveCategory.SelectedValue = ViewState("Type").ToString
            tbRemark.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbEmpNumb.Text = ""
            tbEmpName.Text = ""
            'ddlSubSection.SelectedIndex = 0
            ddlJobTitle.SelectedIndex = 0
            ddlDepartment.SelectedIndex = 0
            ddlLeaveType.SelectedIndex = -1
            tbTotal.Text = "0"
            tbTaken.Text = "0"
            tbHoliday.Text = "0"
            tbDispensasi.Text = "0"
            tbStartDate.SelectedDate = ViewState("ServerDate")
            tbEndDate.SelectedDate = ViewState("ServerDate")
            tbStartTime.Text = "00:00"
            tbEndTime.Text = "00:00"
            tbaddr.Text = ""
            tbPhone.Text = ""
            tbReason.Text = ""
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
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Hd Error : " + ex.ToString)
        End Try
    End Function
    Function CekDt(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not Dr Is Nothing Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("EmpNumb").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Employee Must Have Value")
                    Return False
                End If

                If Dr("LeaveType").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Leave Type Must Have Value")
                    Return False
                End If
                If Dr("QtyTotal").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Total Must Have Value")
                    Return False
                End If
            Else
                If tbEmpNumb.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Employee No Must Have Value")
                    tbEmpNumb.Focus()
                    Return False
                End If
                If ddlLeaveType.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Leave Type Must Have Value")
                    ddlLeaveType.Focus()
                    Return False
                End If
                If tbTotal.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Total Must Have Value")
                    tbTotal.Focus()
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
            BindToDropList(ddlLeaveCategory, Dt.Rows(0)("LeaveCategory").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal Course As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("EmpNumb = " + QuotedStr(Course))
            If Dr.Length > 0 Then
                BindToText(tbEmpNumb, Dr(0)("EmpNumb").ToString)
                BindToText(tbEmpName, Dr(0)("EmpName").ToString)
                'BindToDropList(ddlSubSection, Dr(0)("SubSection").ToString)
                BindToDropList(ddlDepartment, Dr(0)("Department").ToString)
                BindToDropList(ddlJobTitle, Dr(0)("JobTitle").ToString)
                BindToDropList(ddlLeaveType, Dr(0)("LeaveType").ToString)
                BindToDate(tbHireDate, Dr(0)("HireDate").ToString)
                BindToDate(tbStartDate, Dr(0)("StartDate").ToString)
                BindToDate(tbEndDate, Dr(0)("EndDate").ToString)
                BindToText(tbTotal, Dr(0)("QtyTotal").ToString)
                BindToText(tbTaken, Dr(0)("QtyTaken").ToString)
                BindToText(tbHoliday, Dr(0)("QtyHoliday").ToString)
                BindToText(tbStartTime, Dr(0)("StartTime").ToString)
                BindToText(tbEndTime, Dr(0)("EndTime").ToString)
                BindToDropList(ddlFgLess1Day, Dr(0)("FgLess1Day").ToString)
                BindToText(tbaddr, Dr(0)("Contactaddr").ToString)
                BindToText(tbPhone, Dr(0)("ContactPhone").ToString)
                BindToText(tbReason, Dr(0)("ReasonLeave").ToString)
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
                If ViewState("PKDt") <> tbEmpNumb.Text Then
                    If CekExistData(ViewState("Dt"), "EmpNumb", tbEmpNumb.Text) Then
                        lbStatus.Text = MessageDlg("Employee " + tbEmpName.Text + " has been already exist")
                        Exit Sub
                    End If
                End If

                Row = ViewState("Dt").Select("EmpNumb = " + QuotedStr(ViewState("PKDt")))(0)
                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                Row("EmpNumb") = tbEmpNumb.Text
                Row("EmpName") = tbEmpName.Text
                'Row("SubSection") = ddlSubSection.SelectedValue
                'Row("SubSectionName") = ddlSubSection.SelectedItem.Text
                Row("JobTitle") = ddlJobTitle.SelectedValue
                Row("JobTitleName") = ddlJobTitle.SelectedItem.Text
                Row("Department") = ddlDepartment.SelectedValue
                Row("DepartmentName") = ddlDepartment.SelectedItem.Text
                Row("LeaveType") = ddlLeaveType.SelectedValue
                Row("LeaveTypeName") = ddlLeaveType.SelectedItem.Text
                Row("HireDate") = tbHireDate.SelectedDate
                Row("StartDate") = tbStartDate.SelectedDate
                Row("EndDate") = tbEndDate.SelectedDate
                Row("StartTime") = tbStartTime.Text
                Row("EndTime") = tbEndTime.Text
                Row("FgLess1Day") = ddlFgLess1Day.SelectedValue
                Row("QtyTotal") = FormatFloat(tbTotal.Text, ViewState("DigitQty"))
                Row("QtyTaken") = FormatFloat(tbTaken.Text, ViewState("DigitQty"))
                Row("QtyDispensasi") = FormatFloat(tbDispensasi.Text, ViewState("DigitQty"))
                Row("QtyHoliday") = FormatFloat(tbHoliday.Text, ViewState("DigitQty"))
                Row("ContactAddr") = tbaddr.Text
                Row("ContactPhone") = tbPhone.Text
                Row("ReasonLeave") = tbReason.Text
                Row("DoneComplete") = "N"
                Row.EndEdit()
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If
                If CekExistData(ViewState("Dt"), "EmpNumb", tbEmpNumb.Text) = True Then
                    lbStatus.Text = MessageDlg("Employee " + tbEmpName.Text + " has already been exist")
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("EmpNumb") = tbEmpNumb.Text
                dr("EmpName") = tbEmpName.Text
                dr("JobTitle") = ddlJobTitle.SelectedValue
                dr("JobTitleName") = ddlJobTitle.SelectedItem.Text
                dr("Department") = ddlDepartment.SelectedValue
                dr("DepartmentName") = ddlDepartment.SelectedItem.Text
                'dr("SubSection") = ddlSubSection.SelectedValue
                'dr("SubSectionName") = ddlSubSection.SelectedItem.Text
                dr("LeaveType") = ddlLeaveType.SelectedValue
                dr("LeaveTypeName") = ddlLeaveType.SelectedItem.Text
                dr("HireDate") = tbHireDate.SelectedDate
                dr("StartDate") = tbStartDate.SelectedDate
                dr("EndDate") = tbEndDate.SelectedDate
                dr("StartTime") = tbStartTime.Text
                dr("EndTime") = tbEndTime.Text
                dr("FgLess1Day") = ddlFgLess1Day.SelectedValue
                dr("QtyTotal") = FormatFloat(tbTotal.Text, ViewState("DigitQty"))
                dr("QtyTaken") = FormatFloat(tbTaken.Text, ViewState("DigitQty"))
                dr("QtyDispensasi") = FormatFloat(tbDispensasi.Text, ViewState("DigitQty"))
                dr("QtyHoliday") = FormatFloat(tbHoliday.Text, ViewState("DigitQty"))
                dr("ContactAddr") = tbaddr.Text
                dr("ContactPhone") = tbPhone.Text
                dr("ReasonLeave") = tbReason.Text
                dr("DoneComplete") = "N"
                ViewState("Dt").Rows.Add(dr)
            End If
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
            'Save Hd

            If ViewState("StateHd") = "Insert" Then
                tbCode.Text = GetAutoNmbr(ViewState("StrKode").ToString, "Y", CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), "", ViewState("DBConnection").ToString)
                'tbCode.Text = GetAutoNmbr(ViewState("StrKode").ToString, "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), ddlWrhsSrc.SelectedValue, ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO PELeaveOutHd (TransNmbr, TransDate, Status, LeaveCategory, " + _
                "Remark, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", 'H', " + _
                QuotedStr(ddlLeaveCategory.SelectedValue) + ", " + QuotedStr(tbRemark.Text) + _
                ", " + QuotedStr(ViewState("UserId").ToString) + ", GetDate() "

                ViewState("TransNmbr") = tbCode.Text

            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("SELECT Status FROM PELeaveOutHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE PELeaveOutHd SET LeaveCategory = " + QuotedStr(ddlLeaveCategory.SelectedValue) + _
                ", DatePrep = GetDate(), Remark = " + QuotedStr(tbRemark.Text) + _
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
            Dim ConnString As String = ViewState("DBConnection").ToString
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, EmpNumb, JobTitle, LeaveType, FgLess1Day, StartDate, StartTime, EndDate, EndTime, QtyTotal, QtyHoliday, QtyDispensasi, QtyTaken, ContactAddr, " + _
            " ContactPhone, ReasonLeave, DoneComplete " + _
            " FROM PELeaveOutDt WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            ', ActStartDate, ActStartTime, ActEndDate, ActEndTime, ActQtyTotal, ActQtyHoliday, ActQtyDispensasi, ActQtyTaken, ActRemark, ActAbsLeave, ActAbsHoliday, FgHoliday,  QtyMass, ActQtyMass, QtyLeave 
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt As New DataTable("PELeaveOutDt")

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
                lbStatus.Text = MessageDlg("Detail Account must have at least 1 record")
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
            lbStatus.Text = "btnSaveTrans Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            ViewState("StateHd") = "Insert"
            ViewState("DigitCurr") = 0
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, PnlDt, GridDt)
            ViewState("TransNmbr") = ""
            newTrans()
            ddlFgLess1Day_SelectedIndexChanged(Nothing, Nothing)
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
            
            If ViewState("SetLeaveType") = True Then
                FillCombo(ddlLeaveType, "EXEC S_GetLeavesCategory " + QuotedStr(ddlLeaveCategory.SelectedValue), True, "Leave_Code", "Leave_Name", ViewState("DBConnection"))
                ViewState("SetLeaveType") = False
            End If

            ViewState("StateDt") = "Insert"
            MovePanel(PnlDt, pnlEditDt)
            EnableHd(False)
            StatusButtonSave(False)
            tbEmpNumb.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt error : " + ex.ToString
        End Try
    End Sub
    Private Sub newTrans()
        Try
            ClearHd()
            Cleardt()
            tbDoneComplete.Text = "N"
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
        Dim FDateName, FDateValue, FilterName, FilterValue As String
        Try
            FDateName = "Date"
            FDateValue = "TransDate"
            FilterName = "Train No, Status, LeaveType, Remark"
            FilterValue = "TransNmbr, Status, LeaveType, Remark"
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
                        ViewState("SetLeaveType") = True
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
                        Session("SelectCommand") = "EXEC S_PEFormLeaveOut '''" + GVR.Cells(2).Text + "'''"
                        Session("ReportFile") = ".../../../Rpt/FormLeaveOut.frx"
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

    Protected Sub GridDt_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridDt.RowCommand
        Try
            If e.CommandName = "Closing" Then
                Dim GVR As GridViewRow
                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))

                If ViewState("Status") <> "P" Then
                    lbStatus.Text = MessageDlg("Status Leave Taken is not Post, cannot complete employee")
                    Exit Sub
                End If
                ViewState("EmpClose") = GVR.Cells(3).Text
                'AttachScript("closing();", Page, Me.GetType)

                Try
                    Dim sqlstring, result As String
                    'sqlstring = "Declare @A VarChar(255) EXEC S_PELeaveOutCompleteCek '" + tbCode.Text + "', '" + GVR.Cells(2).Text + "', @A OUT SELECT @A"
                    'result = SQLExecuteScalar(sqlstring, ViewState("DBConnection").ToString)

                    ''If result.Length > 2 Then
                    'lbStatus.Text = result
                    'Else

                    Session("FgComplete") = "Taken"
                    Session("LbTitle") = lbTitle.Text
                    Session("FgType") = ddlLeaveCategory.SelectedValue
                    Session("Nmbr") = tbCode.Text
                    Session("EmpNo") = GVR.Cells(3).Text
                    Session("EmpName") = GVR.Cells(4).Text
                    Session("StartDate") = GVR.Cells(10).Text
                    Session("StartTime") = GVR.Cells(11).Text
                    Session("EndDate") = GVR.Cells(12).Text
                    Session("EndTime") = GVR.Cells(13).Text
                    Session("ActQtyMass") = 0
                    Session("ActQtyTotal") = GVR.Cells(14).Text
                    Session("ActQtyHoliday") = GVR.Cells(15).Text
                    Session("ActQtyDispensasi") = GVR.Cells(16).Text
                    Session("ActQtyTaken") = GVR.Cells(17).Text
                    Session("LeaveType") = GVR.Cells(8).Text
                    Session("TransDate") = tbDate.SelectedDate
                    'Session("ReasonLeave") = GVR.Cells(21).Text
                    Session("FgLess1Day") = GVR.Cells(10).Text
                    Session("KeyId") = Request.QueryString("KeyId")
                    Response.Redirect("..\..\Transaction\TrLeaveOut\FormComplete.aspx")
                    'AttachScript("openClosingdlg('" + Request.QueryString("KeyId") + "','TrEmpOverTime');", Page, Me.GetType())

                    'End If
                Catch ex As Exception
                    lbStatus.Text = "openClosingdlg Error = " + ex.ToString
                End Try
            ElseIf e.CommandName = "UnClosing" Then
                Dim GVR As GridViewRow
                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))
                
                If (ViewState("Status") <> "F" And GVR.Cells(20).Text = "N") Then
                    lbStatus.Text = MessageDlg("Status Leave Taken is not Post, cannot Un employee")
                    Exit Sub
                End If
                
                ViewState("EmpClose") = GVR.Cells(3).Text
                'AttachScript("closing();", Page, Me.GetType)

                Try
                    Dim sqlstring, result As String

                    sqlstring = "Declare @A VarChar(255) EXEC S_PELeaveOutUnComplete '" + ViewState("TransNmbr") + "', '" + GVR.Cells(3).Text + "'," + CInt(Session(Request.QueryString("KeyId"))("Year")).ToString + "," + CInt(Session(Request.QueryString("KeyId"))("Period")).ToString + ", " + QuotedStr(ViewState("UserId")) + ", @A OUT SELECT @A"
                    result = SQLExecuteScalar(sqlstring, ViewState("DBConnection").ToString)
                    If result.Length > 2 Then
                        lbStatus.Text = result
                    Else
                        BindDataDt(ViewState("TransNmbr"))
                        GridDt.Columns(2).Visible = GVR.Cells(20).Text = "N"
                        GridDt.Columns(1).Visible = GVR.Cells(20).Text = "Y"
                        ViewState("Status") = "P"
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
        Dim LbType As Label
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            LbType = GVR.FindControl("lbLeaveType")

            If ViewState("SetLeaveType") Then
                FillCombo(ddlLeaveType, "EXEC S_GetLeavesCategory " + QuotedStr(ddlLeaveCategory.SelectedValue), True, "Leave_Code", "Leave_Name", ViewState("DBConnection"))
                ViewState("SetLeaveType") = False
            End If

            FillTextBoxDt(GVR.Cells(3).Text)
            MovePanel(PnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            ViewState("PKDt") = GVR.Cells(3).Text
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


    Protected Sub btnEmp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEmp.Click
        Dim ResultField As String
        Try
            'Session("filter") = "SELECT EmpNumb, EmpName, JobTitleCode, HireDate, StartLeave, EndDate FROM V_MsEmpLeaves WHERE " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + " - HireDate >= 366 "
            Session("filter") = "EXEC S_PELeaveOutReff '', ''," + QuotedStr(ddlLeaveCategory.SelectedValue) + ", " + QuotedStr(tbDate.Text)
            ', SubSectionCode, JobTitle, LeaveType, FgLess1Day, StartDate, EndDate, DayTotal, DayHoliday, DayDispensasi, DayMass, DayTaken, ContactAddr, ContactPhone, ReasonLeave, HireDate, Department, FgHoliday, StrDayTotal, StrDayHoliday, StrDayDispensasi, StrDayMass, StrDayTaken
            ResultField = "EmpNumb, EmpName, Department, HireDate"
            ViewState("Sender") = "btnEmp"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnEmp_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbEmpNumb_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbEmpNumb.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Dim SQLString As String
        Try
            SQLString = "EXEC S_PELeaveOutReff ' AND A.EmpNumb = '" + QuotedStr(tbEmpNumb.Text) + "'', ''," + QuotedStr(ddlLeaveCategory.SelectedValue) + ", " + QuotedStr(tbDate.Text)
            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbEmpNumb.Text = Dr("EmpNumb")
                tbEmpName.Text = Dr("EmpName")
                BindToDropList(ddlJobTitle, Dr("JobTitleCode").ToString)
                'BindToDropList(ddlSubSection, Dr("SubSectionCode").ToString)
                BindToDropList(ddlDepartment, Dr("Department").ToString)
                BindToDropList(ddlLeaveType, Dr("LeaveType").ToString)
                BindToDropList(ddlFgLess1Day, Dr("FgLess1Day").ToString)
                BindToDate(tbHireDate, Dr("HireDate").ToString)
                BindToDate(tbStartDate, Dr("StartDate").ToString)
                tbStartTime.Text = Dr("StartTime")
                BindToDate(tbEndDate, Dr("EndDate").ToString)
                tbEndTime.Text = Dr("EndTime")
                tbTotal.Text = FormatFloat(Dr("DayTotal"), ViewState("DigitQty"))
                tbHoliday.Text = FormatFloat(Dr("DayHoliday"), ViewState("DigitQty"))
                tbDispensasi.Text = FormatFloat(Dr("DayDispensasi"), ViewState("DigitQty"))
                tbTaken.Text = FormatFloat(Dr("DayTaken"), ViewState("DigitQty"))
                BindToText(tbaddr, Dr("ContactAddr").ToString)
                BindToText(tbPhone, Dr("ContactPhone").ToString)
                BindToText(tbReason, Dr("ReasonLeave").ToString)
            Else
                tbEmpNumb.Text = ""
                tbEmpName.Text = ""
                ddlJobTitle.SelectedValue = ""
                'ddlSubSection.SelectedValue = ""
                ddlDepartment.SelectedValue = ""
                tbTotal.Text = FormatFloat(0, ViewState("DigitQty"))
                tbHoliday.Text = FormatFloat(0, ViewState("DigitQty"))
                tbDispensasi.Text = FormatFloat(0, ViewState("DigitQty"))
                tbTaken.Text = FormatFloat(0, ViewState("DigitQty"))
                tbaddr.Text = ""
                tbPhone.Text = ""
                tbReason.Text = ""
            End If
        Catch ex As Exception
            Throw New Exception("tbEmp_TextChanged error: " + ex.ToString)
        End Try
    End Sub
    Protected Sub btnGetData_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGetData.Click
        Dim ResultField, CriteriaField As String 'ResultSame 
        Try
            'If ddlJobTitle.SelectedValue = "" Then
            '    lbStatus.Text = MessageDlg("Shipment Type must have value")
            '    Exit Sub
            'End If
            If CekHd() = False Then
                Exit Sub
            End If
            Session("Result") = Nothing
            Session("filter") = "EXEC S_PELeaveOutReff '',''," + QuotedStr(ddlLeaveCategory.SelectedValue) + ", " + QuotedStr(tbDate.Text)
            'SubSectionCode, SubSectionName, LeaveType, LeaveTypeName, FgLess1Day, StartDate, StartTime, EndDate, 
            'EndTime, DayTotal, DayHoliday, DayDispensasi, DayMass, DayTaken, ContactAddr, ContactPhone, ReasonLeave, HireDate, Department, DepartmentName, FgHoliday, StrDayTotal, StrDayHoliday, StrDayDispensasi, StrDayMass, StrDayTaken
            ResultField = "EmpNumb, EmpName, HireDate, JobTitleCode, JobTitleName, LeaveType, LeaveTypeName, FgLess1Day, StartDate, StartTime, EndDate, EndTime, DayTotal, DayHoliday, DayDispensasi, DayMass, DayTaken, ContactAddr, ContactPhone, ReasonLeave, Department, DepartmentName"
            CriteriaField = "EmpNumb, EmpName, HireDate, JobTitleCode, JobTitleName, LeaveType, LeaveTypeName, FgLess1Day, StartDate, StartTime, EndDate, EndTime, DayTotal, DayHoliday, DayDispensasi, DayMass, DayTaken, ContactAddr, ContactPhone, ReasonLeave, Department, DepartmentName"
            ViewState("CriteriaField") = CriteriaField.Split(",")
            Session("Column") = ResultField.Split(",")
            ViewState("Sender") = "btnGetDt"
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Get Data Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlFgLess1Day_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFgLess1Day.SelectedIndexChanged
        Try
            If ddlFgLess1Day.SelectedValue = "Y" Then
                tbStartTime.Enabled = True
            Else
                tbStartTime.Enabled = False
            End If
            tbEndTime.Enabled = tbStartTime.Enabled
            tbStartDate.Enabled = Not tbStartTime.Enabled
            tbEndDate.Enabled = Not tbStartTime.Enabled
            tbStartTime.Text = "00:00"
            tbEndTime.Text = "00:00"
        Catch ex As Exception
            lbStatus.Text = "ddlFgLess1Day_SelectedIndexChanged Data Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlLeaveType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlLeaveType.SelectedIndexChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Dim SQLString As String
        Try
            SQLString = "EXEC S_FindLeaveType " + QuotedStr(ddlLeaveType.SelectedValue)
            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbDispensasi.Text = FormatFloat(Dr("Dispensasi").ToString, ViewState("DigitQty"))
                tbEndDate_SelectionChanged(Nothing, Nothing)
                lbFgHoliday.Text = Dr("FgHoliday").ToString
            Else
                tbDispensasi.Text = FormatFloat(0, ViewState("DigitQty"))
            End If
        Catch ex As Exception
            lbStatus.Text = "ddlLeaveType_SelectedIndexChanged Data Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbEndDate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbEndDate.SelectionChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Dim SQLString As String
        Try
            ' ddlLeaveType_SelectedIndexChanged(Nothing, Nothing)
            SQLString = "EXEC S_PELeaveOutGetTaken " + QuotedStr(tbEmpNumb.Text) + ", " + QuotedStr(ddlLeaveType.SelectedValue) + ", " + QuotedStr(ddlFgLess1Day.SelectedValue) + ", " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(tbStartTime.Text) + ", " + QuotedStr(Format(tbEndDate.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(tbEndTime.Text)
            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbTaken.Text = FormatFloat(Dr("Taken").ToString, ViewState("DigitQty"))
                tbHoliday.Text = FormatFloat(Dr("Holiday").ToString, ViewState("DigitQty"))
                tbDispensasi.Text = FormatFloat(Dr("Dispensasi").ToString, ViewState("DigitQty"))
                tbTotal.Text = FormatFloat(Dr("Total").ToString, ViewState("DigitQty"))
            End If
        Catch ex As Exception
            lbStatus.Text = "ddlLeaveType_SelectedIndexChanged Data Error : " + ex.ToString
        End Try
    End Sub
End Class
