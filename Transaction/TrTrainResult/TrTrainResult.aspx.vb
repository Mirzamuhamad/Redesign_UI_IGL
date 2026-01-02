Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class Transaction_TrTrainResult_TrTrainResult
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "SELECT * FROM V_PETrainResultHd"

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
            End If
            lbStatus.Text = ""
            'BtnGo.Visible = ddlCommand.Visible
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If

            If Not Session("Result") Is Nothing Then

                If ViewState("Sender") = "btnPlan" Then
                    tbPlanningNo.Text = Session("Result")(0).ToString
                    BindToDropList(ddlDepartment, Session("Result")(1).ToString)
                    BindToDropList(ddlFgSchedule, Session("Result")(2).ToString)
                    tbCourseTitle.Text = Session("Result")(3).ToString
                    BindToDropList(ddlTraining, Session("Result")(4).ToString)
                    BindToDropList(ddlTrainingPlace, Session("Result")(5).ToString)
                    tbTrainingLocation.Text = Session("Result")(6).ToString
                    tbSasaran.Text = Session("Result")(7).ToString
                    tbMateri.Text = Session("Result")(8).ToString
                    BindToDropList(ddlInstitution, Session("Result")(9).ToString)
                    tbInstructor.Text = Session("Result")(10).ToString
                    tbTrainingCost.Text = FormatFloat(Session("Result")(11).ToString, ViewState("DigitQty"))
                    BindToDropList(ddlFgSertifikat, Session("Result")(12).ToString)
                    BindToDate(tbStartDate, Session("Result")(13).ToString)
                    BindToDate(tbEndDate, Session("Result")(14).ToString)
                End If
                If ViewState("Sender") = "btnEmp" Then
                    tbEmpNo.Text = Session("Result")(0).ToString
                    tbEmpName.Text = Session("Result")(1).ToString
                    BindToDropList(ddlJobTitle, Session("Result")(2).ToString)
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
            FillCombo(ddlDepartment, "SELECT Dept_Code, Dept_Name FROM VMsDepartment", True, "Dept_Code", "Dept_Name", ViewState("DBConnection"))
            FillCombo(ddlTraining, "SELECT * FROM VMsTraining", True, "TrainingCode", "TrainingName", ViewState("DBConnection"))
            FillCombo(ddlInstitution, "SELECT * FROM VMsInstitution", True, "InstitutionCode", "InstitutionName", ViewState("DBConnection"))
            FillCombo(ddlJobTitle, "SELECT Job_Title_Code, Job_Title_Name FROM VMsJobTitle", True, "Job_Title_Code", "Job_Title_Name", ViewState("DBConnection"))

            ViewState("SortExpression") = Nothing
            ViewState("DigitCurr") = 0
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
            If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
                ddlCommand.Items.Add("Print")
                ddlCommand2.Items.Add("Print")
            End If
            tbTrainingCost.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbTrainingCost.Attributes.Add("OnBlur", "setformat();")
            tbQty.Attributes.Add("ReadOnly", "True")
            tbEmpName.Attributes.Add("ReadOnly", "True")
            tbPlanningNo.Attributes.Add("ReadOnly", "True")
            tbCourseTitle.Attributes.Add("ReadOnly", "True")
            tbTrainingLocation.Attributes.Add("ReadOnly", "True")
            tbSasaran.Attributes.Add("ReadOnly", "True")
            tbMateri.Attributes.Add("ReadOnly", "True")
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
        Return "SELECT * FROM V_PETrainResultDt WHERE TransNmbr = " + QuotedStr(Nmbr)
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
                Session("SelectCommand") = "EXEC S_PEFormTrainResult " + Result
                Session("ReportFile") = ".../../../Rpt/FormTrainResult.frx"
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
                        Result = ExecSPCommandGo(ActionValue, "S_PETrainResult", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            btnPlan.Visible = State
            ddlInstitution.Enabled = State
            tbInstructor.Enabled = State
            tbStartDate.Enabled = State
            tbEndDate.Enabled = State
            tbTrainingCost.Enabled = State
            ddlFgSertifikat.Enabled = State
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
            tbPlanningNo.Text = ""
            ddlDepartment.SelectedIndex = 0
            ddlFgSchedule.SelectedIndex = 0
            tbCourseTitle.Text = ""
            ddlTraining.SelectedIndex = 0
            ddlTrainingPlace.SelectedIndex = 0
            tbTrainingLocation.Text = ""
            tbSasaran.Text = ""
            tbMateri.Text = ""
            ddlInstitution.SelectedIndex = 0
            tbInstructor.Text = ""
            tbStartDate.SelectedDate = ViewState("ServerDate")
            tbEndDate.SelectedDate = ViewState("ServerDate")
            tbTrainingCost.Text = "0"
            ddlFgSertifikat.SelectedIndex = 0
            tbRemark.Text = ""
            tbQty.Text = "0"
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub Cleardt()
        Try
            tbEmpNo.Text = ""
            tbEmpName.Text = ""
            ddlJobTitle.SelectedIndex = 0
            ddlFgTraining.SelectedIndex = 1
            tbSertificate.Text = ""
            tbConclusion.Text = ""
            tbSuggestion.Text = ""
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
            If tbPlanningNo.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Plan No Must Have Value")
                tbPlanningNo.Focus()
                Return False
            End If
            If ddlDepartment.SelectedValue.Trim = "" Then
                lbStatus.Text = MessageDlg("Organization Must Have Value")
                ddlDepartment.Focus()
                Return False
            End If
            If tbStartDate.SelectedDate = Nothing Then
                lbStatus.Text = MessageDlg("Start Date must have value")
                tbStartDate.Focus()
                Return False
            End If
            If tbEndDate.SelectedDate = Nothing Then
                lbStatus.Text = MessageDlg("End Date must have value")
                tbEndDate.Focus()
                Return False
            End If
            If tbEndDate.SelectedDate < tbStartDate.SelectedDate Then
                lbStatus.Text = MessageDlg("End date can not smaller than start date must have value")
                tbStartDate.Focus()
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
                If tbEmpNo.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Employee No Must Have Value")
                    tbEmpNo.Focus()
                    Return False
                End If
                If ddlFgTraining.SelectedValue.Trim = "" Then
                    lbStatus.Text = MessageDlg("Status Must Have Value")
                    ddlFgTraining.Focus()
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
            BindToText(tbPlanningNo, Dt.Rows(0)("PlanNo").ToString)
            BindToDropList(ddlDepartment, Dt.Rows(0)("Department").ToString)
            BindToDropList(ddlFgSchedule, Dt.Rows(0)("FgSchedule").ToString)
            BindToText(tbCourseTitle, Dt.Rows(0)("CourseTitle").ToString)
            BindToDropList(ddlTraining, Dt.Rows(0)("Training").ToString)
            BindToDropList(ddlTrainingPlace, Dt.Rows(0)("TrainingPlace").ToString)
            BindToText(tbTrainingLocation, Dt.Rows(0)("TrainingLocation").ToString)
            BindToText(tbSasaran, Dt.Rows(0)("Sasaran").ToString)
            BindToText(tbMateri, Dt.Rows(0)("Materi").ToString)
            BindToDropList(ddlInstitution, Dt.Rows(0)("Institusi").ToString)
            BindToText(tbInstructor, Dt.Rows(0)("Instructor").ToString)
            BindToDate(tbStartDate, Dt.Rows(0)("StartDate").ToString)
            BindToDate(tbEndDate, Dt.Rows(0)("EndDate").ToString)
            BindToText(tbTrainingCost, Dt.Rows(0)("TrainingCost").ToString)
            BindToDropList(ddlFgSertifikat, Dt.Rows(0)("FgSertifikat").ToString)
            BindToText(tbQty, Dt.Rows(0)("QtyParticipant").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal EmpNumb As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("EmpNumb = " + QuotedStr(EmpNumb))
            If Dr.Length > 0 Then
                BindToText(tbEmpNo, Dr(0)("EmpNumb").ToString)
                BindToText(tbEmpName, Dr(0)("Emp_Name").ToString)
                BindToDropList(ddlJobTitle, Dr(0)("JobTitle").ToString)
                BindToDropList(ddlFgTraining, Dr(0)("FgTraining").ToString)
                BindToText(tbSertificate, Dr(0)("CertificationNo").ToString)
                BindToText(tbConclusion, Dr(0)("Conclusion").ToString)
                BindToText(tbSuggestion, Dr(0)("Suggestion").ToString)
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
                If ViewState("PKDt") <> tbEmpNo.Text Then
                    If CekExistData(ViewState("Dt"), "EmpNumb", tbEmpNo.Text) Then
                        lbStatus.Text = "Employee " + tbEmpNo.Text + " has been already exist"
                        Exit Sub
                    End If
                End If

                Row = ViewState("Dt").Select("EmpNumb = " + QuotedStr(ViewState("PKDt")))(0)
                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                Row("EmpNumb") = tbEmpNo.Text
                Row("Emp_Name") = tbEmpName.Text
                Row("JobTitle") = ddlJobTitle.SelectedValue
                Row("Job_Title_Name") = ddlJobTitle.SelectedItem.Text
                Row("FgTraining") = ddlFgTraining.SelectedValue
                Row("CertificationNo") = tbSertificate.Text
                Row("Conclusion") = tbConclusion.Text
                Row("Suggestion") = tbSuggestion.Text
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
                dr("Emp_Name") = tbEmpName.Text
                dr("JobTitle") = ddlJobTitle.SelectedValue
                dr("Job_Title_Name") = ddlJobTitle.SelectedItem.Text
                dr("FgTraining") = ddlFgTraining.SelectedValue
                dr("CertificationNo") = tbSertificate.Text
                dr("Conclusion") = tbConclusion.Text
                dr("Suggestion") = tbSuggestion.Text
                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, PnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            BindGridDt(ViewState("Dt"), GridDt)
            CountParticipant()
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
                tbCode.Text = GetAutoNmbr("TR", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO PETrainResultHd (TransNmbr, TransDate, Status, Department, PlanNo, FgSchedule, " + _
                " CourseTitle, Training, TrainingPlace, TrainingLocation, " + _
                " Instructor, Institusi, StartDate, EndDate, Sasaran, Materi, " + _
                " TrainingCost, QtyParticipant, " + _
                " FgSertifikat, Remark, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", 'H', " + QuotedStr(ddlDepartment.SelectedValue) + ", " + QuotedStr(tbPlanningNo.Text) + ", " + QuotedStr(ddlFgSchedule.SelectedValue) + ", " + _
                QuotedStr(tbCourseTitle.Text) + ", " + QuotedStr(ddlTraining.SelectedValue) + ", " + QuotedStr(ddlTrainingPlace.SelectedValue) + ", " + QuotedStr(tbTrainingLocation.Text) + ", " + _
                QuotedStr(tbInstructor.Text) + ", " + QuotedStr(ddlInstitution.SelectedValue) + ", " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(Format(tbEndDate.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(tbSasaran.Text) + ", " + QuotedStr(tbMateri.Text) + ", " + _
                tbTrainingCost.Text.Replace(",", "") + ", " + tbQty.Text + ", " + _
                QuotedStr(ddlFgSertifikat.SelectedValue) + ", " + QuotedStr(tbRemark.Text) + ", " + _
                QuotedStr(ViewState("UserId").ToString) + ", GetDate() "

                ViewState("TransNmbr") = tbCode.Text

            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("SELECT Status FROM PETrainResultHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE PETrainResultHd SET Department= " + QuotedStr(ddlDepartment.SelectedValue) + ", PlanNo = " + QuotedStr(tbPlanningNo.Text) + ", FgSchedule = " + QuotedStr(ddlFgSchedule.SelectedValue) + _
                ", CourseTitle = " + QuotedStr(tbCourseTitle.Text) + ", Training = " + QuotedStr(ddlTraining.SelectedValue) + ", TrainingPlace = " + QuotedStr(ddlTrainingPlace.SelectedValue) + ", TrainingLocation = " + QuotedStr(tbTrainingLocation.Text) + _
                ", Instructor = " + QuotedStr(tbInstructor.Text) + ", Institusi = " + QuotedStr(ddlInstitution.SelectedValue) + ", StartDate = " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + ", EndDate = " + QuotedStr(Format(tbEndDate.SelectedValue, "yyyy-MM-dd")) + ", Sasaran = " + QuotedStr(tbSasaran.Text) + _
                ", Materi = " + QuotedStr(tbMateri.Text) + _
                ", TrainingCost = " + tbTrainingCost.Text.Replace(",", "") + ", QtyParticipant = " + tbQty.Text + ", FgSertifikat = " + QuotedStr(ddlFgSertifikat.SelectedValue) + _
                ", Remark = " + QuotedStr(tbRemark.Text) + _
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
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, EmpNumb, JobTitle, FgTraining, CertificationNo, Conclusion, Suggestion FROM PETrainResultDt WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt As New DataTable("PETrainResultDt")

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
            tbCourseTitle.Focus()
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
            FDateName = "Date, Start Date, End Date"
            FDateValue = "TransDate, StartDate, EndDate"
            FilterName = "Result No, Status, Plan No, Organization, Training Yearly , Course Title, Training Type, Training Place, Training Location, Sasaran, Materi, Institution, Instructor, Training Cost, Qty Participant, Sertification, Remark"
            FilterValue = "TransNmbr, Status, PlanNo, Department_Name, FgSchedule, CourseTitle, TrainingName, TrainingPlace, TrainingLocation, Sasaran, Materi, InstitutionName, Instructor, TrainingCost, QtyParticipant, FgSertifikat, Remark"
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
                    ViewState("Status") = GVR.Cells(3).Text
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
                        Session("SelectCommand") = "EXEC S_PEFormTrainResult '''" + GVR.Cells(2).Text + "'''"
                        Session("ReportFile") = ".../../../Rpt/FormTrainResult.frx"
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
            CountParticipant()
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
            ViewState("PKDt") = GVR.Cells(1).Text
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
            Session("filter") = "SELECT EmpNumb, EmpName, JobTitleCode, JobTitleName FROM V_PETrainResultGetPlanDt WHERE Plan_No = " + QuotedStr(tbPlanningNo.Text)
            ResultField = "EmpNumb, EmpName, JobTitleCode"
            ViewState("Sender") = "btnEmp"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnEmp_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbEmpNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbEmpNo.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Dim SQLString As String
        Try
            SQLString = "SELECT EmpNumb, EmpName, JobTitleCode, JobTitleName FROM V_PETrainResultGetPlanDt WHERE Plan_No = " + QuotedStr(tbPlanningNo.Text) + " AND EmpNumb = " + QuotedStr(tbEmpNo.Text)
            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbEmpNo.Text = Dr("EmpNumb")
                tbEmpName.Text = Dr("EmpName")
                ddlJobTitle.SelectedValue = Dr("JobTitleCode")
            Else
                tbEmpNo.Text = ""
                tbEmpName.Text = ""
                ddlJobTitle.SelectedIndex = 0
            End If

        Catch ex As Exception
            Throw New Exception("tbEmpNo_TextChanged error: " + ex.ToString)
        End Try
    End Sub

    Private Sub CountParticipant()
        Dim dr As DataRow
        Dim iQty As Double
        Try
            iQty = 0
            For Each dr In ViewState("Dt").Rows
                If Not dr.RowState = DataRowState.Deleted Then
                    iQty = iQty + 1
                End If
            Next
            tbQty.Text = FormatFloat(iQty, ViewState("DigitQty"))
        Catch ex As Exception
            Throw New Exception("Count Dt Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnPlan_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPlan.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT * FROM V_PETrainResultGetPlanHd"
            ResultField = "Plan_No, Department_Code, Training_Yearly, Training_Name, Training_Type_Code, TrainingPlace, TrainingLocation, Sasaran, Materi, InstitutionCode, Instructor, TrainingCost, Certification, StartDate, EndDate"
            ViewState("Sender") = "btnPlan"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnPlan_Click Error : " + ex.ToString
        End Try
    End Sub
End Class
