Imports System.Data
Imports System.Data.SqlClient
Partial Class Transaction_TrTrainSchedule_TrTrainSchedule
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "SELECT * FROM V_PETrainScheduleHd"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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

                If ViewState("Sender") = "btnNoIden" Then
                    tbNoIden.Text = Session("Result")(0).ToString
                    BindToDropList(ddlDepartment, Session("Result")(1).ToString)
                    tbCourseTitle.Text = ""
                    ddlTrainingType.SelectedIndex = 0
                    ddlTrainingPlace.SelectedIndex = 0
                    ddlInstitution.SelectedIndex = 0
                    tbInstructor.Text = ""
                    tbEstCost.Text = "0"
                    tbParticipant.Text = ""
                    tbMateri.Text = ""
                    tbSasaran.Text = ""
                End If
                If ViewState("Sender") = "btnCourse" Then
                    tbCourseTitle.Text = Session("Result")(0).ToString
                    BindToDropList(ddlTrainingType, Session("Result")(1).ToString)
                    BindToDropList(ddlTrainingPlace, Session("Result")(2).ToString)
                    BindToDropList(ddlInstitution, Session("Result")(3).ToString)
                    tbInstructor.Text = Session("Result")(4).ToString
                    BindToDropList(ddlCurrency, Session("Result")(5).ToString)
                    tbEstCost.Text = FormatNumber(Session("Result")(6).ToString, ViewState("DigitQty"))
                    tbParticipant.Text = Session("Result")(7).ToString
                    BindToDropList(ddlEstMonth, Session("Result")(8).ToString)
                    tbMateri.Text = Session("Result")(9).ToString
                    tbSasaran.Text = Session("Result")(10).ToString
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
            FillCombo(ddlYear, "EXEC S_GetYeaR", False, "Year", "Year", ViewState("DBConnection"))
            FillCombo(ddlInstitution, "SELECT * FROM VMsInstitution", True, "InstitutionCode", "InstitutionName", ViewState("DBConnection"))
            FillCombo(ddlWorkPlace, "SELECT WorkPlaceCode, WorkPlaceName FROM MsWorkPlace", True, "WorkPlaceCode", "WorkPlaceName", ViewState("DBConnection"))
            FillCombo(ddlTrainingType, "SELECT * FROM VMsTraining", True, "TrainingCode", "TrainingName", ViewState("DBConnection"))
            FillCombo(ddlDepartment, "SELECT Dept_Code, Dept_Name FROM VMsDepartment", True, "Dept_Code", "Dept_Name", ViewState("DBConnection"))
            FillCombo(ddlEstMonth, "EXEC S_GetPeriod", False, "Period", "Period", ViewState("DBConnection"))
            FillCombo(ddlCurrency, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))

            ddlYear.SelectedValue = ViewState("GLYear")
            ddlEstMonth.SelectedValue = ViewState("GLPeriod")

            ViewState("SortExpression") = Nothing
            ViewState("DigitCurr") = 0
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
            If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
                ddlCommand.Items.Add("Print")
                ddlCommand2.Items.Add("Print")
            End If
            tbEstCost.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbEstCost.Attributes.Add("OnBlur", "setformat();")
            tbCourseTitle.Attributes.Add("ReadOnly", "True")
            tbInstructor.Attributes.Add("ReadOnly", "True")
            tbParticipant.Attributes.Add("ReadOnly", "True")
            tbSasaran.Attributes.Add("ReadOnly", "True")
            tbMateri.Attributes.Add("ReadOnly", "True")
            tbEstCost.Attributes.Add("ReadOnly", "True")
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
        Return "SELECT * FROM V_PETrainScheduleDt WHERE TransNmbr = " + QuotedStr(Nmbr)
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
                Session("SelectCommand") = "EXEC S_PEFormTrainSchedule " + Result
                Session("ReportFile") = ".../../../Rpt/FormTrainSchedule.frx"
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
                        Result = ExecSPCommandGo(ActionValue, "S_PETrainSchedule", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            ddlYear.Enabled = State
            ddlWorkPlace.Enabled = State
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
            ddlYear.SelectedValue = ViewState("GLYear")
            ddlWorkPlace.SelectedIndex = 0
            tbRemark.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbNoIden.Text = ""
            ddlDepartment.SelectedIndex = 0
            tbCourseTitle.Text = ""
            ddlTrainingType.SelectedIndex = 0
            ddlTrainingPlace.Text = "In House"
            ddlInstitution.SelectedIndex = 0
            tbInstructor.Text = ""
            ddlCurrency.SelectedValue = ViewState("Currency")
            ddlCurrency_SelectedIndexChanged(Nothing, Nothing)
            tbEstCost.Text = "0"
            tbParticipant.Text = ""
            tbSasaran.Text = ""
            tbMateri.Text = ""
            ddlPrioriry.SelectedIndex = 0
            tbRemarkDt.Text = ""
            ddlEstMonth.SelectedValue = ViewState("GLPeriod")
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
            If CInt(Session(Request.QueryString("KeyId"))("Year")) <> ddlYear.SelectedValue Then
                lbStatus.Text = MessageDlg("Year must be inputed in accounting " + Session(Request.QueryString("KeyId"))("Year").ToString.Trim)
                ddlYear.Focus()
                Return False
            End If
            If ddlWorkPlace.SelectedValue.Trim = "" Then
                lbStatus.Text = MessageDlg("Work Place Must Have Value")
                ddlWorkPlace.Focus()
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
                If tbNoIden.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Identification No Must Have Value")
                    tbNoIden.Focus()
                    Return False
                End If
                If tbCourseTitle.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Training Name Must Have Value")
                    tbCourseTitle.Focus()
                    Return False
                End If
                If ddlDepartment.SelectedValue.Trim = "" Then
                    lbStatus.Text = MessageDlg("Organization Must Have Value")
                    ddlDepartment.Focus()
                    Return False
                End If
                If ddlPrioriry.SelectedValue.Trim = "" Then
                    lbStatus.Text = MessageDlg("Priority must have value")
                    ddlPrioriry.Focus()
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
            BindToDropList(ddlYear, Dt.Rows(0)("Year").ToString)
            BindToDropList(ddlWorkPlace, Dt.Rows(0)("WorkPlace").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal Course As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("IdentificateNo+'|'+CourseTitle = " + QuotedStr(Course))
            If Dr.Length > 0 Then
                BindToText(tbNoIden, Dr(0)("IdentificateNo").ToString)
                BindToDropList(ddlDepartment, Dr(0)("Department").ToString)
                BindToText(tbCourseTitle, Dr(0)("CourseTitle").ToString)
                BindToDropList(ddlTrainingType, Dr(0)("Training").ToString)
                BindToDropList(ddlTrainingPlace, Dr(0)("TrainingPlace").ToString)
                BindToDropList(ddlInstitution, Dr(0)("Institution").ToString)
                BindToText(tbInstructor, Dr(0)("Instructor").ToString)
                BindToDropList(ddlCurrency, Dr(0)("Currency").ToString)
                BindToText(tbEstCost, Dr(0)("CostPerson").ToString)
                BindToText(tbParticipant, Dr(0)("Participant").ToString)
                BindToDropList(ddlEstMonth, Dr(0)("EstMonth").ToString)
                BindToText(tbSasaran, Dr(0)("Sasaran").ToString)
                BindToText(tbMateri, Dr(0)("Materi").ToString)
                BindToDropList(ddlPrioriry, Dr(0)("Prioritas").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
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
                If ViewState("PKDt") <> tbNoIden.Text + "|" + tbCourseTitle.Text Then
                    If CekExistData(ViewState("Dt"), "IdentificateNo,CourseTitle", tbNoIden.Text + "|" + tbCourseTitle.Text) Then
                        lbStatus.Text = "Identification No " + tbNoIden.Text + " And Training Name " + tbCourseTitle.Text + " has been already exist"
                        Exit Sub
                    End If
                End If

                Row = ViewState("Dt").Select("IdentificateNo+'|'+CourseTitle = " + QuotedStr(ViewState("PKDt")))(0)
                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                Row("IdentificateNo") = tbNoIden.Text
                Row("Department_Name") = ddlDepartment.SelectedItem.Text
                Row("Department") = ddlDepartment.SelectedValue
                Row("CourseTitle") = tbCourseTitle.Text
                Row("Training") = ddlTrainingType.SelectedValue
                Row("TrainingName") = ddlTrainingType.SelectedItem.Text
                Row("TrainingPlace") = ddlTrainingPlace.SelectedValue
                Row("Institution") = ddlInstitution.SelectedValue
                Row("InstitutionName") = ddlInstitution.SelectedItem.Text
                Row("Instructor") = tbInstructor.Text
                Row("Currency") = ddlCurrency.SelectedValue
                Row("CostPerson") = tbEstCost.Text.Replace(",", "")
                Row("Participant") = tbParticipant.Text
                Row("EstMonth") = ddlEstMonth.SelectedValue
                Row("Sasaran") = tbSasaran.Text
                Row("Materi") = tbMateri.Text
                Row("Prioritas") = ddlPrioriry.SelectedValue
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If
                If CekExistData(ViewState("Dt"), "IdentificateNo,CourseTitle", tbNoIden.Text + "|" + tbCourseTitle.Text) = True Then
                    lbStatus.Text = "Identification No " + tbNoIden.Text + " And Training Name " + tbCourseTitle.Text + " has already been exist"
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("IdentificateNo") = tbNoIden.Text
                dr("Department_Name") = ddlDepartment.SelectedItem.Text
                dr("Department") = ddlDepartment.SelectedValue
                dr("CourseTitle") = tbCourseTitle.Text
                dr("Training") = ddlTrainingType.SelectedValue
                dr("TrainingName") = ddlTrainingType.SelectedItem.Text
                dr("TrainingPlace") = ddlTrainingPlace.SelectedValue
                dr("Institution") = ddlInstitution.SelectedValue
                dr("InstitutionName") = ddlInstitution.SelectedItem.Text
                dr("Instructor") = tbInstructor.Text
                dr("Currency") = ddlCurrency.SelectedValue
                dr("CostPerson") = tbEstCost.Text.Replace(",", "")
                dr("Participant") = tbParticipant.Text
                dr("EstMonth") = ddlEstMonth.SelectedValue
                dr("Sasaran") = tbSasaran.Text
                dr("Materi") = tbMateri.Text
                dr("Prioritas") = ddlPrioriry.SelectedValue
                dr("Remark") = tbRemarkDt.Text
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
                tbCode.Text = GetAutoNmbr("TSY", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO PETrainScheduleHd (TransNmbr, TransDate, Status, Year, " + _
                "WorkPlace, Remark, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", 'H', " + _
                ddlYear.SelectedValue + ", " + _
                QuotedStr(ddlWorkPlace.SelectedValue) + ", " + QuotedStr(tbRemark.Text) + _
                ", " + QuotedStr(ViewState("UserId").ToString) + ", GetDate() "

                ViewState("TransNmbr") = tbCode.Text

            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("SELECT Status FROM PETrainScheduleHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE PETrainScheduleHd SET Year = " + ddlYear.SelectedValue + _
                ", WorkPlace = " + QuotedStr(ddlWorkPlace.SelectedValue) + ", Remark = " + QuotedStr(tbRemark.Text) + _
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
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, IdentificateNo, CourseTitle, Department, Prioritas, Remark  FROM PETrainScheduleDt WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt As New DataTable("PETrainScheduleDt")

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
            FDateName = "Date"
            FDateValue = "TransDate"
            FilterName = "Train No, Status, Work Place, Year, Remark"
            FilterValue = "TransNmbr, Status, WorkPalceName, Year, Remark"
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
                        Session("SelectCommand") = "EXEC S_PEFormTrainSchedule '''" + GVR.Cells(2).Text + "'''"
                        Session("ReportFile") = ".../../../Rpt/FormTrainSchedule.frx"
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

            dr = ViewState("Dt").Select("IdentificateNo+'|'+CourseTitle = " + QuotedStr(GVR.Cells(1).Text + "|" + GVR.Cells(3).Text))
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
            FillTextBoxDt(GVR.Cells(1).Text + "|" + GVR.Cells(3).Text)
            MovePanel(PnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            ViewState("PKDt") = GVR.Cells(1).Text + "|" + GVR.Cells(3).Text
            'ChangeCurrency(ddlCurrency, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
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

    Protected Sub ddlCurrency_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCurrency.SelectedIndexChanged
        Try
            'ChangeCurrency(ddlCurrency, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
            'tbRate.Focus()
        Catch ex As Exception
            lbStatus.Text = "ddl Curr selected index Changed : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnNoIden_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNoIden.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT DISTINCT Identification_No, Department_Code, Department_Name FROM V_PETrainIdentForSchedule WHERE Year = " + ddlYear.SelectedValue
            ResultField = "Identification_No, Department_Code, Department_Name"
            ViewState("Sender") = "btnNoIden"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnNoIden_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCourse_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCourse.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT Course_Title, Participant, Training_Type, Training_Type_Name, TrainingPlace, Institution, InstitutionName, Instructor, Currency, Cost_Person, Est_Month, Sasaran, Materi  FROM V_PETrainIdentForSchedule WHERE Identification_No = " + QuotedStr(tbNoIden.Text)
            ResultField = "Course_Title, Training_Type, TrainingPlace, Institution, Instructor, Currency, Cost_Person, Participant, Est_Month, Sasaran, Materi"
            ViewState("Sender") = "btnCourse"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnCourse_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbNoIden_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbNoIden.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Dim SQLString As String
        Try
            SQLString = "SELECT DISTINCT Identification_No, Department_Code, Department_Name FROM V_PETrainIdentForSchedule WHERE Identification_No = " + QuotedStr(tbNoIden.Text) + " AND Year = " + ddlYear.SelectedValue
            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbNoIden.Text = Dr("Identification_No")
                ddlDepartment.SelectedValue = Dr("Department_Code")
            Else
                tbNoIden.Text = ""
                ddlDepartment.SelectedIndex = 0
            End If

            tbCourseTitle.Text = ""
            ddlTrainingType.SelectedIndex = 0
            ddlTrainingPlace.SelectedIndex = 0
            ddlInstitution.SelectedIndex = 0
            tbInstructor.Text = ""
            tbEstCost.Text = "0"
            tbParticipant.Text = ""
            tbMateri.Text = ""
            tbSasaran.Text = ""
        Catch ex As Exception
            Throw New Exception("tbNoIden_TextChanged error: " + ex.ToString)
        End Try
    End Sub
End Class
