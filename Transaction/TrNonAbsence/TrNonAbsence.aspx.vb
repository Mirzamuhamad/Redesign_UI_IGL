Imports System.Data
Imports System.Data.SqlClient

Partial Class Transaction_TrNonAbsence_TrNonAbsence
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "SELECT DISTINCT TransNmbr, Nmbr, TransDate, Status, KeepingDate, EmpAppr, DeptCode, DeptName, EmpApprName, Remark FROM V_PENonAbsenceHd"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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
                If ViewState("Sender") = "btnEmpAppr" Then
                    tbEmpAppr.Text = Session("Result")(0).ToString
                    tbEmpNameAppr.Text = Session("Result")(1).ToString
                ElseIf ViewState("Sender") = "btnEmp" Then
                    tbEmpNo.Text = Session("Result")(0).ToString
                    tbEmpName.Text = Session("Result")(1).ToString
                    ddlDepartment.SelectedValue = Session("Result")(2).ToString
                    ddlJobTitle.SelectedValue = Session("Result")(3).ToString
                    ddlShift.SelectedValue = Session("Result")(4).ToString
                    tbTimeIn.Text = Session("Result")(5).ToString
                    tbTimeOut.Text = Session("Result")(6).ToString
                ElseIf ViewState("Sender") = "btnGetData" Then
                    Dim drResult, dr As DataRow
                    Dim Row As DataRow()
                    For Each drResult In Session("Result").Rows
                        Row = ViewState("Dt").Select("EmpNumb = " + QuotedStr(drResult("Emp_No")))
                        If Row.Count = 0 Then
                            dr = ViewState("Dt").NewRow
                            dr("EmpNumb") = drResult("Emp_No")
                            dr("EmpName") = drResult("Emp_Name")
                            dr("Department") = drResult("Department")
                            dr("DepartmentName") = drResult("Department_Name")
                            dr("JobTitle") = drResult("Job_Title")
                            dr("JobTitleName") = drResult("Job_Title_Name")
                            dr("Shift") = drResult("Shift")
                            dr("ShiftName") = drResult("ShiftName")
                            dr("TimeIn") = drResult("TimeIn")
                            dr("TimeOut") = drResult("TimeOut")
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
            FillCombo(ddlJobTitle, "SELECT * FROM VMsJobTitle", True, "Job_Title_Code", "Job_Title_Name", ViewState("DBConnection"))
            FillCombo(ddlShift, "SELECT ShiftCode, ShiftName FROM MsShift", True, "ShiftCode", "ShiftName", ViewState("DBConnection"))
            FillCombo(ddlJobTitle, "SELECT * FROM VMsJobTitle ", True, "Job_Title_Code", "Job_Title_Name", ViewState("DBConnection"))
            FillCombo(ddlAbsStatus, "SELECT Absence, AbsenceName from V_MsAbsGroupDt WHERE AbsGroup = 'Unscanned'", True, "Absence", "AbsenceName", ViewState("DBConnection"))

            ViewState("SortExpression") = Nothing
            ViewState("DigitCurr") = 0
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
            If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
                ddlCommand.Items.Add("Print")
                ddlCommand2.Items.Add("Print")
            End If
            BtnGo.Visible = False
            'tbMasaBerlaku.Attributes.Add("OnKeyDown", "return PressNumeric();")
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
        Return "SELECT * FROM V_PENonAbsenceDt WHERE TransNmbr = " + QuotedStr(Nmbr)
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
                Session("SelectCommand") = "EXEC S_PEFormNonAbsence " + Result
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
                        Result = ExecSPCommandGo(ActionValue, "S_PENonAbsence", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            tbKeepingDate.Enabled = State
            tbEmpAppr.Enabled = State
            btnEmpAppr.Visible = State
            'ddlJbtAppr1.Enabled = False
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
            tbDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            tbKeepingDate.SelectedDate = ViewState("ServerDate")
            tbEmpAppr.Text = ""
            tbEmpNameAppr.Text = ""
            tbRemark.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbEmpNo.Text = ""
            tbEmpName.Text = ""
            ddlDepartment.SelectedIndex = 0
            ddlJobTitle.SelectedIndex = 0
            ddlShift.SelectedIndex = 0
            ddlAbsStatus.SelectedIndex = 0
            tbTimeIn.Text = ""
            tbTimeOut.Text = ""
            tbReason.Text = ""
            tbHaveAbsence.Text = "N"
            tbTimeIn.Enabled = tbHaveAbsence.Text = "Y"
            tbTimeOut.Enabled = tbHaveAbsence.Text = "Y"
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
            If tbKeepingDate.SelectedDate = Nothing Then
                lbStatus.Text = MessageDlg("Keeping Date must have value")
                tbKeepingDate.Focus()
                Return False
            End If
            If tbEmpAppr.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Approved By Must Have Value")
                tbEmpAppr.Focus()
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
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("EmpNumb").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Employee Must Have Value")
                    Return False
                End If
                If Dr("AbsStatus").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Sts Absensi Must Have Value")
                    Return False
                End If
                If Dr("FgAbsence").ToString.Trim = "Y" Then
                    If Dr("TimeIn").ToString.Trim = "" Or Dr("TimeIn").ToString.Trim = ":" Then
                        lbStatus.Text = MessageDlg("Time In Must Have Value")
                        Return False
                    End If
                    If Dr("TimeOut").ToString.Trim = "" Or Dr("TimeOut").ToString.Trim = ":" Then
                        lbStatus.Text = MessageDlg("Time Out Must Have Value")
                        Return False
                    End If
                    If IsTime(Dr("TimeIn").ToString) = False Then
                        lbStatus.Text = MessageDlg("Time In is invalid")
                        Return False
                    End If
                    If IsTime(Dr("TimeOut").ToString) = False Then
                        lbStatus.Text = MessageDlg("Time Out is invalid")
                        Return False
                    End If
                End If
            Else
                If tbEmpNo.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Employee Must Have Value")
                    tbEmpNo.Focus()
                    Return False
                End If
                If ddlAbsStatus.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Sts Absensi Must Have Value")
                    ddlAbsStatus.Focus()
                    Return False
                End If
                If tbHaveAbsence.Text = "Y" Then
                    If tbTimeIn.Text.Trim = "" Or tbTimeIn.Text.Trim = ":" Then
                        lbStatus.Text = MessageDlg("Time In Must Have Value")
                        tbTimeIn.Focus()
                        Return False
                    End If
                    If tbTimeOut.Text.Trim = "" Or tbTimeOut.Text.Trim = ":" Then
                        lbStatus.Text = MessageDlg("Time Out Must Have Value")
                        tbTimeOut.Focus()
                        Return False
                    End If
                    If IsTime(tbTimeIn.Text) = False Then
                        lbStatus.Text = MessageDlg("Time In is invalid")
                        tbTimeIn.Focus()
                        Return False
                    End If
                    If IsTime(tbTimeOut.Text) = False Then
                        lbStatus.Text = MessageDlg("Time Out is invalid")
                        tbTimeOut.Focus()
                        Return False
                    End If
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
            BindToDate(tbKeepingDate, Dt.Rows(0)("KeepingDate").ToString)
            BindToText(tbEmpAppr, Dt.Rows(0)("EmpAppr").ToString)
            BindToText(tbEmpNameAppr, Dt.Rows(0)("EmpApprName").ToString)
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
                BindToDropList(ddlDepartment, Dr(0)("Department").ToString)
                BindToDropList(ddlJobTitle, Dr(0)("JobTitle").ToString)
                BindToDropList(ddlShift, Dr(0)("Shift").ToString)
                BindToDropList(ddlAbsStatus, Dr(0)("AbsStatus").ToString)
                BindToText(tbHaveAbsence, Dr(0)("FgAbsence").ToString)
                BindToText(tbTimeIn, Dr(0)("TimeIn").ToString)
                BindToText(tbTimeOut, Dr(0)("TimeOut").ToString)
                BindToText(tbReason, Dr(0)("Reason").ToString)
                tbTimeIn.Enabled = tbHaveAbsence.Text = "Y"
                tbTimeOut.Enabled = tbHaveAbsence.Text = "Y"
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
                Row("EmpName") = tbEmpName.Text
                Row("Department") = ddlDepartment.SelectedValue
                Row("DepartmentName") = ddlDepartment.SelectedItem.Text
                Row("JobTitle") = ddlJobTitle.SelectedValue
                Row("JobTitleName") = ddlJobTitle.SelectedItem.Text
                Row("Shift") = ddlShift.SelectedValue
                Row("ShiftName") = ddlShift.SelectedItem.Text
                Row("TimeIn") = tbTimeIn.Text
                Row("TimeOut") = tbTimeOut.Text
                Row("AbsStatus") = ddlAbsStatus.SelectedValue
                Row("AbsStatusName") = ddlAbsStatus.SelectedItem.Text
                Row("Reason") = tbReason.Text
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
                dr("Department") = ddlDepartment.SelectedValue
                dr("DepartmentName") = ddlDepartment.SelectedItem.Text
                dr("JobTitle") = ddlJobTitle.SelectedValue
                dr("JobTitleName") = ddlJobTitle.SelectedItem.Text
                dr("Shift") = ddlShift.SelectedValue
                dr("ShiftName") = ddlShift.SelectedItem.Text
                dr("TimeIn") = tbTimeIn.Text
                dr("TimeOut") = tbTimeOut.Text
                dr("AbsStatus") = ddlAbsStatus.SelectedValue
                dr("AbsStatusName") = ddlAbsStatus.SelectedItem.Text
                dr("Reason") = tbReason.Text
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
            If Not (ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit") Then
                Exit Sub
            Else
                ViewState("StateSave") = ViewState("StateHd")
                ViewState("StateHd") = "View"
            End If
            'Save Hd
            If ViewState("StateSave") = "Insert" Then
                tbCode.Text = GetAutoNmbr("NA", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO PENonAbsenceHd (TransNmbr, TransDate, Status, KeepingDate, " + _
                "EmpAppr, Remark, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", 'H', " + QuotedStr(Format(tbKeepingDate.SelectedValue, "yyyy-MM-dd")) + ", '" + _
                tbEmpAppr.Text.Replace(",", "") + "', " + QuotedStr(tbRemark.Text) + _
                ", " + QuotedStr(ViewState("UserId").ToString) + ", GetDate() "

                ViewState("TransNmbr") = tbCode.Text

            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("SELECT Status FROM PENonAbsenceHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE PENonAbsenceHd SET KeepingDate = " + QuotedStr(Format(tbKeepingDate.SelectedValue, "yyyy-MM-dd")) + _
                ", EmpAppr =" + QuotedStr(tbEmpAppr.Text) + _
                ", Remark =" + QuotedStr(tbRemark.Text) + _
                ", DatePrep = GetDate()" + _
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
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, EmpNumb, JobTitle, AbsStatus, Shift, TimeIn, TimeOut, Reason, Department FROM PENonAbsenceDt WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)

            da = New SqlDataAdapter(cmdSql)

            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand


            Dim Dt As New DataTable("PENonAbsenceDt")

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
        Catch ex As Exception
            lbStatus.Text = "btn add dt error : " + ex.ToString
        End Try
    End Sub
    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ViewState("DigitCurr") = 0
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
        Dim FDateName, FDateValue, FilterName, FilterValue As String
        Try
            FDateName = "Date, Keeping Date"
            FDateValue = "TransDate, KeepingDate"
            FilterName = "Trans No, Status, Approved By, Approved By Name, Remark"
            FilterValue = "TransNmbr, Status, EmpAppr, EmpApprName, Remark"
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
                        Session("SelectCommand") = "EXEC S_PEFormNonAbsence '''" + GVR.Cells(2).Text + "''' "
                        'lbStatus.Text = Session("SelectCommand")
                        'Exit Sub
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

            dr = ViewState("Dt").Select("EmpNumb = " + QuotedStr(GVR.Cells(1).Text))
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
            ViewState("EmpNo") = GVR.Cells(1).Text
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

    Protected Sub tbEmpAppr_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbEmpAppr.TextChanged
        Dim Dr As DataRow
        Dim Ds As DataSet
        Try
            Ds = SQLExecuteQuery("SELECT Emp_No, Emp_Name, Job_Title, Job_Title_Name FROM V_MsEmployee WHERE Fg_Active = 'Y' AND Emp_No = " + QuotedStr(tbEmpAppr.Text) + " AND Emp_No <> " + QuotedStr(tbEmpNo.Text), ViewState("DBConnection").ToString)
            If Ds.Tables(0).Rows.Count = 1 Then
                Dr = Ds.Tables(0).Rows(0)
                tbEmpAppr.Text = TrimStr(Dr("Emp_No").ToString)
                tbEmpNameAppr.Text = TrimStr(Dr("Emp_Name").ToString)
            Else
                tbEmpAppr.Text = ""
                tbEmpNameAppr.Text = ""
            End If

        Catch ex As Exception
            Throw New Exception("tbEmpAppr_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnEmpAppr_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEmpAppr.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DbConnection")
            Session("filter") = "SELECT Emp_No, Emp_Name, Job_Title, Job_Title_Name FROM V_MsEmployee WHERE Fg_Active = 'Y' AND Emp_No <> " + QuotedStr(tbEmpNo.Text)
            ResultField = "Emp_No, Emp_Name, Job_Title"
            ViewState("Sender") = "btnEmpAppr"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnEmpAppr_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnEmp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEmp.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DbConnection")
            Session("filter") = "Select Emp_No, Emp_Name, Department, Department_Name, Job_Title, Job_Title_Name, Shift, dbo.GetShiftTimeIn(ScheduleDate, Shift) As TimeIn, dbo.GetShiftTimeOut(ScheduleDate, Shift) AS TimeOut, Method FROM V_MsEmployeeForNonAbsDt Where ScheduleDate = " + QuotedStr(Format(tbKeepingDate.SelectedValue, "yyyy-MM-dd")) + " AND HaveAttendance = 'N' AND Emp_No <> " + QuotedStr(tbEmpAppr.Text)
            ResultField = "Emp_No, Emp_Name, Department, Job_Title, Shift, TimeIn, TimeOut"
            ViewState("Sender") = "btnEmp"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnEmp_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbEmpNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbEmpNo.TextChanged
        Dim Dr As DataRow
        Dim Ds As DataSet
        Try
            Ds = SQLExecuteQuery("EXEC S_PENonAbsenceFindEmp " + QuotedStr(Format(tbKeepingDate.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(tbEmpNo.Text), ViewState("DBConnection").ToString)
            'lbStatus.Text = "EXEC S_PENonAbsenceFindEmp " + QuotedStr(Format(tbKeepingDate.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(tbEmpNo.Text)
            'Exit Sub
            If Ds.Tables(0).Rows.Count = 1 Then
                Dr = Ds.Tables(0).Rows(0)
                tbEmpNo.Text = TrimStr(Dr("Emp_No").ToString)
                tbEmpName.Text = TrimStr(Dr("Emp_Name").ToString)
                ddlDepartment.Text = TrimStr(Dr("Department").ToString)
                ddlJobTitle.Text = TrimStr(Dr("Job_Title").ToString)
                ddlShift.Text = TrimStr(Dr("Shift").ToString)
                tbTimeIn.Text = TrimStr(Dr("TimeIn").ToString)
                tbTimeOut.Text = TrimStr(Dr("TimeOut").ToString)
            Else
                tbEmpNo.Text = ""
                tbEmpName.Text = ""
                ddlDepartment.SelectedIndex = 0
                ddlJobTitle.SelectedIndex = 0
                ddlShift.SelectedIndex = 0
                tbTimeIn.Text = ""
                tbTimeOut.Text = ""
            End If

        Catch ex As Exception
            Throw New Exception("tbEmpNo_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlAbsStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlAbsStatus.SelectedIndexChanged
        Try
            If ddlAbsStatus.SelectedValue = "" Then
                tbHaveAbsence.Text = "N"
            Else
                tbHaveAbsence.Text = SQLExecuteScalar("Select FgAbsence from MsAbsStatus WHERE AbsStatusCode = " + QuotedStr(ddlAbsStatus.SelectedValue), ViewState("DBConnection"))
            End If
            tbTimeIn.Enabled = tbHaveAbsence.Text = "Y"
            tbTimeOut.Enabled = tbHaveAbsence.Text = "Y"
            If tbHaveAbsence.Text = "N" Then
                tbTimeIn.Text = ""
                tbTimeOut.Text = ""
            End If
        Catch ex As Exception
            Throw New Exception("ddlAbsStatus_SelectedIndexChanged Error : " + ex.ToString)
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
            Session("filter") = "SELECT Emp_No, Emp_Name, Department, Department_Name, Job_Title, Job_Title_Name, Shift, ShiftName, dbo.GetShiftTimeIn(ScheduleDate, Shift) As TimeIn, dbo.GetShiftTimeOut(ScheduleDate, Shift) AS TimeOut, Method FROM V_MsEmployeeForNonAbsDt Where ScheduleDate = " + QuotedStr(Format(tbKeepingDate.SelectedValue, "yyyy-MM-dd")) + " AND HaveAttendance = 'N' AND Emp_No <> " + QuotedStr(tbEmpAppr.Text)
            ResultField = "Emp_No, Emp_Name, Department, Department_Name, Job_Title, Job_Title_Name, Shift, ShiftName, TimeIn, TimeOut"
            Session("DBConnection") = ViewState("DBConnection")
            CriteriaField = "Emp_No, Emp_Name, Department, Department_Name, Job_Title, Job_Title_Name, Shift, ShiftName, TimeIn, TimeOut"
            ViewState("CriteriaField") = CriteriaField.Split(",")
            Session("Column") = ResultField.Split(",")
            ViewState("Sender") = "btnGetData"
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnGetData_Click Data Error : " + ex.ToString
        End Try
    End Sub
End Class
