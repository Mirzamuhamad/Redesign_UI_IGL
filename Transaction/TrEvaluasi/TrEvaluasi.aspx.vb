
Imports System.Data
Imports System.Data.SqlClient
Imports System.Runtime.Serialization
Partial Class Transaction_TrEvaluasi_TrEvaluasi
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select * From V_PEEvaluasiHd"

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
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnEmp" Then
                    BindToText(tbEmp, Session("Result")(0).ToString)
                    BindToText(tbEmpName, Session("Result")(1).ToString)
                    BindToDropList(ddlJobTitle, Session("Result")(2).ToString)
                    BindToDropList(ddlJobLevel, Session("Result")(3).ToString)
                End If
                If ViewState("Sender") = "btnEmpAppr1" Then
                    BindToText(tbEmpAppr1, Session("Result")(0).ToString)
                    BindToText(tbEmpNameAppr1, Session("Result")(1).ToString)
                    BindToDropList(ddlJobTitleAppr1, Session("Result")(2).ToString)
                    BindToDropList(ddlJobLevelAppr1, Session("Result")(3).ToString)
                End If
                If ViewState("Sender") = "btnEmpAppr2" Then
                    BindToText(tbEmpAppr2, Session("Result")(0).ToString)
                    BindToText(tbEmpNameAppr2, Session("Result")(1).ToString)
                    BindToDropList(ddlJobTitleAppr2, Session("Result")(2).ToString)
                    BindToDropList(ddlJobLevelAppr2, Session("Result")(3).ToString)
                End If
                If ViewState("Sender") = "btnDept" Then
                    BindToText(tbDeptCode, Session("Result")(0).ToString)
                    BindToText(tbDeptName, Session("Result")(1).ToString)
                End If

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

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        Dim index As Integer
        Try
            index = Int32.Parse(e.Item.Value)
            MultiView1.ActiveViewIndex = index
            'AttachScript("hitungtotal();", Page, Me.GetType())
            btnSaveTrans.Focus()
        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub
    Private Sub SetInit()
        Try
            FillCombo(ddlJobTitle, "EXEC S_GetJobTitle", True, "JobTtlCode", "JobTtlName", ViewState("DBConnection"))
            FillCombo(ddlJobLevel, "EXEC S_GetJobLevel", True, "JobLvlCode", "JobLvlName", ViewState("DBConnection"))
            FillCombo(ddlCompType, "EXEC S_GetMsBobotPA", True, "CompetenceType", "CompetenceType", ViewState("DBConnection"))
            FillCombo(ddlJobTitleAppr1, "EXEC S_GetJobTitle", True, "JobTtlCode", "JobTtlName", ViewState("DBConnection"))
            FillCombo(ddlJobLevelAppr1, "EXEC S_GetJobLevel", True, "JobLvlCode", "JobLvlName", ViewState("DBConnection"))
            FillCombo(ddlJobTitleAppr2, "EXEC S_GetJobTitle", True, "JobTtlCode", "JobTtlName", ViewState("DBConnection"))
            FillCombo(ddlJobLevelAppr2, "EXEC S_GetJobLevel", True, "JobLvlCode", "JobLvlName", ViewState("DBConnection"))
            FillCombo(ddlReviewReason, "EXEC S_GetMsReviewReason", False, "ReviewReasonCode", "ReviewReasonName", ViewState("DBConnection"))
            
            ViewState("SortExpression") = Nothing
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))

            tbNilai1.Attributes.Add("ReadOnly", "True")
            tbNilai2.Attributes.Add("ReadOnly", "True")
            tbNilai3.Attributes.Add("ReadOnly", "True")
            tbNilai4.Attributes.Add("ReadOnly", "True")
            tbNilai5.Attributes.Add("ReadOnly", "True")

            tbBobot1.Attributes.Add("ReadOnly", "True")
            tbBobot2.Attributes.Add("ReadOnly", "True")
            tbBobot3.Attributes.Add("ReadOnly", "True")
            tbBobot5.Attributes.Add("ReadOnly", "True")

            tbNilaiAkhir1.Attributes.Add("ReadOnly", "True")
            tbNilaiAkhir2.Attributes.Add("ReadOnly", "True")
            tbNilaiAkhir3.Attributes.Add("ReadOnly", "True")
            tbNilaiAkhir4.Attributes.Add("ReadOnly", "True")
            tbNilaiAkhir5.Attributes.Add("ReadOnly", "True")

            tbBobot4.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbTarget.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbBobotDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbRealisasi.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbSkor.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbNilaiDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbNilaiDt3.Attributes.Add("OnKeyDown", "return PressNumeric();")

            tbNilai1.Attributes.Add("OnBlur", "hitungtotal();")
            tbNilai2.Attributes.Add("OnBlur", "hitungtotal();")
            tbNilai3.Attributes.Add("OnBlur", "hitungtotal();")
            tbNilai4.Attributes.Add("OnBlur", "hitungtotal();")
            tbNilai5.Attributes.Add("OnBlur", "hitungtotal();")

            tbBobot1.Attributes.Add("OnBlur", "hitungtotal();")
            tbBobot2.Attributes.Add("OnBlur", "hitungtotal();")
            tbBobot3.Attributes.Add("OnBlur", "hitungtotal();")
            tbBobot4.Attributes.Add("OnBlur", "hitungtotal();")
            tbBobot5.Attributes.Add("OnBlur", "hitungtotal();")

            tbNilaiAkhir1.Attributes.Add("OnBlur", "hitungtotal();")
            tbNilaiAkhir2.Attributes.Add("OnBlur", "hitungtotal();")
            tbNilaiAkhir3.Attributes.Add("OnBlur", "hitungtotal();")
            tbNilaiAkhir4.Attributes.Add("OnBlur", "hitungtotal();")
            tbNilaiAkhir5.Attributes.Add("OnBlur", "hitungtotal();")
        Catch ex As Exception
            Throw New Exception("Set Init Error : " + ex.ToString)
        End Try
    End Sub
    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            'If AdvanceFilter.Length > 1 And StrFilter.Length > 1 Then
            '    StrFilter = StrFilter + " And " + AdvanceFilter
            'ElseIf AdvanceFilter.Length > 1 And StrFilter.Length <= 1 Then
            '    StrFilter = AdvanceFilter
            'End If
            If AdvanceFilter.Length > 1 Then
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
            DV.Sort = ViewState("SortExpression")
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "TransDate DESC"
            End If
            GridView1.DataSource = DV
            GridView1.DataBind()

        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub
    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT * From V_PEEvaluasiDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function
    Private Function GetStringDt2(ByVal Nmbr As String) As String
        Return "SELECT * From V_PEEvaluasiDt2 WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function
    Private Function GetStringDt3(ByVal Nmbr As String) As String
        Return "SELECT * From V_PEEvaluasiDt3 WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function
    
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
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

            Status = CekStatus(ActionValue)

            ListSelectNmbr = ""
            GetListCommand(Status, GridView1, "3,2", ListSelectNmbr, Nmbr, lbStatus.Text)
            If ListSelectNmbr = "" Then Exit Sub
            For j = 0 To (Nmbr.Length - 1)
                If Nmbr(j) = "" Then
                    Exit For
                Else
                    Result = ExecSPCommandGo(ActionValue, "S_PEEvaluasi", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            tbDeptCode.Enabled = State
            btnDept.Visible = State
            ddlCompType.Enabled = State
            tbEmp.Enabled = State
            btnEmp.Visible = State
            tbReviewPeriod.Enabled = State
            ddlReviewReason.Enabled = State
            tbEmpAppr1.Enabled = State
            btnEmpAppr1.Visible = State
            tbEmpAppr2.Enabled = State
            btnEmpAppr2.Visible = State
            ddlFgSetuju.Enabled = State
            tbKomentarEmp.Enabled = State
            tbKomentarEmpAppr1.Enabled = State
            tbKomentarEmpAppr2.Enabled = State
            tbImprovement.Enabled = State
            tbPencapaian.Enabled = State
            tbKekuatan.Enabled = State
            tbBobot4.Enabled = State
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
    Private Sub BindDataDt2(ByVal Nmbr As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt2") = Nothing
            dt = SQLExecuteQuery(GetStringDt2(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt2") = dt
            BindGridDt(dt, GridDt2)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt2 Error : " + ex.ToString)
        End Try
    End Sub
    Private Sub BindDataDt3(ByVal Nmbr As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt3") = Nothing
            dt = SQLExecuteQuery(GetStringDt3(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt3") = dt
            BindGridDt(dt, GridDt3)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt3 Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBack.Click, btnHome.Click
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

    Protected Sub btnCancelDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelDt.Click
        Try
            MovePanel(pnlEditDt, PnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnCancelDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelDt2.Click
        Try
            MovePanel(pnlEditDt2, pnlDt2)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt2  Error : " + ex.ToString
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbCode.Text = ""
            tbDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            ddlCompType.SelectedIndex = 0
            tbDeptCode.Text = ""
            tbDeptName.Text = ""
            tbEmp.Text = ""
            tbEmpName.Text = ""
            ddlJobTitle.SelectedValue = ""
            ddlJobLevel.SelectedValue = ""
            tbEmpAppr1.Text = ""
            tbEmpNameAppr1.Text = ""
            ddlJobTitleAppr1.SelectedValue = ""
            ddlJobLevelAppr1.SelectedValue = ""
            tbEmpAppr2.Text = ""
            tbEmpNameAppr2.Text = ""
            ddlJobTitleAppr2.SelectedValue = ""
            ddlJobLevelAppr2.SelectedValue = ""
            tbReviewPeriod.Text = ""
            ddlReviewReason.SelectedIndex = 0
            tbKekuatan.Text = ""
            tbImprovement.Text = ""
            tbPencapaian.Text = ""
            tbNilai1.Text = "0"
            tbNilai2.Text = "0"
            tbNilai3.Text = "0"
            tbNilai4.Text = "0"
            tbNilai5.Text = "0"
            tbBobot1.Text = "0"
            tbBobot2.Text = "0"
            tbBobot3.Text = "0"
            tbBobot4.Text = "0"
            tbBobot5.Text = "0"
            tbNilaiAkhir1.Text = "0"
            tbNilaiAkhir2.Text = "0"
            tbNilaiAkhir3.Text = "0"
            tbNilaiAkhir4.Text = "0"
            tbNilaiAkhir5.Text = "0"
            tbKomentarEmpAppr1.Text = ""
            tbKomentarEmp.Text = ""
            tbKomentarEmpAppr2.Text = ""
            MultiView1.ActiveViewIndex = 1            
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbIndikator.Text = ""
            tbBobotDt.Text = "0"
            tbTarget.Text = "0"
            ddlTypeDt.SelectedIndex = 0
            tbRealisasi.Text = "0"
            tbSkor.Text = "0"
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub

    Private Sub Cleardt2()
        Try
            tbTypeDt2.Text = ""
            tbCompCode.Text = ""
            tbCompName.Text = ""
            tbCompNo.Text = "0"
            tbDescription1.Text = ""
            tbDescription2.Text = ""
            tbNilaiDt2.Text = "0"
        Catch ex As Exception
            Throw New Exception("Clear Dt 2 Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt3()
        Try
            tbProject.Text = ""
            tbPencapaianDt3.Text = ""
            tbNilaiDt3.Text = "0"
        Catch ex As Exception
            Throw New Exception("Clear Dt 3 Error " + ex.ToString)
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
            If tbDeptName.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Department must have value")
                tbDeptCode.Focus()
                Return False
            End If
            If ddlCompType.SelectedValue.Trim = "" Then
                lbStatus.Text = MessageDlg("Type must have value")
                ddlCompType.Focus()
                Return False
            End If
            If tbEmpName.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Employee must have value")
                tbEmp.Focus()
                Return False
            End If
            If tbReviewPeriod.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Periode Review must have value")
                tbReviewPeriod.Focus()
                Return False
            End If
            If ddlReviewReason.SelectedValue.Trim = "" Then
                lbStatus.Text = MessageDlg("Reason must have value")
                ddlReviewReason.Focus()
                Return False
            End If
            If tbEmpNameAppr1.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Penilai must have value")
                tbEmpAppr1.Focus()
                Return False
            End If
            If tbEmpNameAppr2.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Atasan Penilai must have value")
                tbEmpAppr2.Focus()
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
                If Dr("Indikator").ToString = "" Then
                    lbStatus.Text = MessageDlg("Indikator Must Have Value")
                    Return False
                End If
                If CFloat(Dr("Bobot").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Bobot Must Have Value")
                    Return False
                End If
                If CFloat(Dr("Realisasi").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Realisasi Must Have Value")
                    Return False
                End If
                If CFloat(Dr("Target").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Target Must Have Value")
                    Return False
                End If
                If Dr("Type").ToString = "" Then
                    lbStatus.Text = MessageDlg("Type Must Have Value")
                    Return False
                End If
                If CFloat(Dr("Skor").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Skor Must Have Value")
                    Return False
                End If
            Else
                If tbIndikator.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Indikator Must Have Value")
                    tbIndikator.Focus()
                    Return False
                End If
                If CFloat(tbBobotDt.Text.Trim) <= 0 Then
                    lbStatus.Text = MessageDlg("Bobot Must Have Value")
                    tbBobotDt.Focus()
                    Return False
                End If
                If CFloat(tbRealisasi.Text.Trim) <= 0 Then
                    lbStatus.Text = MessageDlg("Realisasi Must Have Value")
                    tbRealisasi.Focus()
                    Return False
                End If
                If CFloat(tbTarget.Text.Trim) <= 0 Then
                    lbStatus.Text = MessageDlg("Target Must Have Value")
                    tbTarget.Focus()
                    Return False
                End If
                If ddlTypeDt.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Type Must Have Value")
                    ddlTypeDt.Focus()
                    Return False
                End If
                If CFloat(tbSkor.Text.Trim) <= 0 Then
                    lbStatus.Text = MessageDlg("Skor Must Have Value")
                    tbSkor.Focus()
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
            BindToText(tbDeptCode, Dt.Rows(0)("Department_Code").ToString)
            BindToText(tbDeptName, Dt.Rows(0)("Dept_Name").ToString)
            BindToDropList(ddlCompType, Dt.Rows(0)("CompetenceType").ToString)
            BindToText(tbEmp, Dt.Rows(0)("EmpNumb").ToString)
            BindToText(tbEmpName, Dt.Rows(0)("Emp_Name").ToString)
            BindToDropList(ddlJobTitle, Dt.Rows(0)("Job_Title_Code").ToString)
            BindToDropList(ddlJobLevel, Dt.Rows(0)("Job_Level_Code").ToString)
            BindToText(tbReviewPeriod, Dt.Rows(0)("ReviewPeriod").ToString)
            BindToDropList(ddlReviewReason, Dt.Rows(0)("ReviewReason").ToString)
            BindToText(tbEmpAppr1, Dt.Rows(0)("EmpAppr1").ToString)
            BindToText(tbEmpNameAppr1, Dt.Rows(0)("Emp_NameAppr1").ToString)
            BindToDropList(ddlJobTitleAppr1, Dt.Rows(0)("JobTitleCode_Appr1").ToString)
            BindToDropList(ddlJobLevelAppr1, Dt.Rows(0)("JobLevelCode_Appr1").ToString)
            BindToText(tbEmpAppr2, Dt.Rows(0)("EmpAppr2").ToString)
            BindToText(tbEmpNameAppr2, Dt.Rows(0)("Emp_NameAppr2").ToString)
            BindToDropList(ddlJobTitleAppr2, Dt.Rows(0)("JobTitleCode_Appr2").ToString)
            BindToDropList(ddlJobLevelAppr2, Dt.Rows(0)("JobLevelCode_Appr2").ToString)
            BindToText(tbKekuatan, Dt.Rows(0)("Kekuatan").ToString)
            BindToText(tbImprovement, Dt.Rows(0)("Improvement").ToString)
            BindToText(tbPencapaian, Dt.Rows(0)("Pencapaian").ToString)
            BindToText(tbNilai1, Dt.Rows(0)("KPINilai").ToString)
            BindToText(tbBobot1, Dt.Rows(0)("KPIBobot").ToString)
            BindToText(tbNilaiAkhir1, Dt.Rows(0)("KPINilaiAkhir").ToString)
            BindToText(tbNilai2, Dt.Rows(0)("UmumNilai").ToString)
            BindToText(tbBobot2, Dt.Rows(0)("UmumBobot").ToString)
            BindToText(tbNilaiAkhir2, Dt.Rows(0)("UmumNilaiAkhir").ToString)
            BindToText(tbNilai3, Dt.Rows(0)("FungsionalNilai").ToString)
            BindToText(tbBobot3, Dt.Rows(0)("FungsionalBobot").ToString)
            BindToText(tbNilaiAkhir3, Dt.Rows(0)("FungsionalNilaiAkhir").ToString)
            BindToText(tbNilai4, Dt.Rows(0)("ProjectNilai").ToString)
            BindToText(tbBobot4, Dt.Rows(0)("ProjectBobot").ToString)
            BindToText(tbNilaiAkhir4, Dt.Rows(0)("ProjectNilaiAkhir").ToString)
            BindToText(tbNilai5, Dt.Rows(0)("TotalNilai").ToString)
            BindToText(tbBobot5, Dt.Rows(0)("TotalBobot").ToString)
            BindToText(tbNilaiAkhir5, Dt.Rows(0)("TotalNilaiAkhir").ToString)
            BindToDropList(ddlFgSetuju, Dt.Rows(0)("FgSetuju").ToString)
            BindToText(tbKomentarEmp, Dt.Rows(0)("KomentarEmp").ToString)
            BindToText(tbKomentarEmpAppr1, Dt.Rows(0)("KomentarEmpAppr1").ToString)
            BindToText(tbKomentarEmpAppr2, Dt.Rows(0)("KomentarEmpAppr2").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("ItemNo = " + ItemNo)
            If Dr.Length > 0 Then
                lbItemNoDt.Text = ItemNo.ToString
                BindToText(tbIndikator, Dr(0)("Indikator").ToString)
                BindToDropList(ddlTypeDt, Dr(0)("Type").ToString)
                BindToText(tbBobotDt, Dr(0)("Bobot").ToString)
                BindToText(tbTarget, Dr(0)("Target").ToString)
                BindToText(tbRealisasi, Dr(0)("Realisasi").ToString)
                BindToText(tbSkor, Dr(0)("Skor").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxDt2(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt2").select("ItemNo = " + QuotedStr(ItemNo))
            If Dr.Length > 0 Then
                lbItemNoDt2.Text = ItemNo.ToString
                BindToText(tbTypeDt2, Dr(0)("Type").ToString)
                BindToText(tbCompCode, Dr(0)("CompetenceCode").ToString)
                BindToText(tbCompName, Dr(0)("CompetenceName").ToString)
                BindToText(tbCompNo, Dr(0)("CompetenceItem").ToString)
                BindToText(tbDescription1, Dr(0)("Description1").ToString)
                BindToText(tbDescription2, Dr(0)("Description2").ToString)
                BindToText(tbNilaiDt2, Dr(0)("Nilai").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail 2 error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt3(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt3").select("ItemNo = " + ItemNo)
            If Dr.Length > 0 Then
                lbItemNoDt3.Text = ItemNo.ToString
                BindToText(tbProject, Dr(0)("ProjectName").ToString)
                BindToText(tbPencapaianDt3, Dr(0)("Pencapaian").ToString)
                BindToText(tbNilaiDt3, Dr(0)("Nilai").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail 3 error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveDt.Click
        Try
            If CekDt() = False Then
                btnSaveDt.Focus()
                Exit Sub
            End If

            If ViewState("StateDt") = "Edit" Then
                Dim Row As DataRow
                Row = ViewState("Dt").Select("ItemNo = " + lbItemNoDt.Text)(0)
                Row.BeginEdit()
                Row("ItemNo") = lbItemNoDt.Text
                Row("Indikator") = tbIndikator.Text
                Row("Bobot") = tbBobotDt.Text
                Row("Realisasi") = tbRealisasi.Text
                Row("Target") = tbTarget.Text
                Row("Type") = ddlTypeDt.SelectedValue
                Row("Skor") = tbSkor.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("ItemNo") = lbItemNoDt.Text
                dr("Indikator") = tbIndikator.Text
                dr("Bobot") = tbBobotDt.Text
                dr("Realisasi") = tbRealisasi.Text
                dr("Target") = tbTarget.Text
                dr("Type") = ddlTypeDt.SelectedValue
                dr("Skor") = tbSkor.Text
                ViewState("Dt").Rows.Add(dr)
            End If
            CountKPINilai()
            MovePanel(pnlEditDt, PnlDt)
            MultiView1.ActiveViewIndex = 1
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
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
                tbCode.Text = GetAutoNmbr("EP", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO PEEvaluasiHd (TransNmbr, TransDate, STATUS, " + _
                "Department, CompetenceType, EmpNumb, EmpAppr1, EmpAppr2, ReviewPeriod, ReviewReason, " + _
                "Kekuatan, Improvement, Pencapaian, KPINilai, UmumNilai, FungsionalNilai, ProjectNilai, TotalNilai,  " + _
                "KPIBobot, UmumBobot, FungsionalBobot, ProjectBobot, TotalBobot,  " + _
                "KPINilaiAkhir, UmumNilaiAkhir, FungsionalNilaiAkhir, ProjectNilaiAkhir, TotalNilaiAkhir, " + _
                "FgSetuju, KomentarEmpAppr1, KomentarEmp, KomentarEmpAppr2, " + _
                "UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", 'H', " + _
                QuotedStr(tbDeptCode.Text) + "," + QuotedStr(ddlCompType.SelectedValue) + "," + QuotedStr(tbEmp.Text) + "," + _
                QuotedStr(tbEmpAppr1.Text) + "," + QuotedStr(tbEmpAppr2.Text) + "," + QuotedStr(tbReviewPeriod.Text) + "," + _
                QuotedStr(ddlReviewReason.SelectedValue) + "," + QuotedStr(tbKekuatan.Text) + "," + QuotedStr(tbImprovement.Text) + "," + _
                QuotedStr(tbPencapaian.Text) + "," + tbNilai1.Text.ToString + "," + tbNilai2.Text.ToString + "," + _
                tbNilai3.Text.ToString + "," + tbNilai4.Text.ToString + "," + tbNilai5.Text.ToString + "," + _
                tbBobot1.Text.ToString + "," + tbBobot2.Text.ToString + "," + tbBobot3.Text.ToString + "," + _
                tbBobot4.Text.ToString + "," + tbBobot5.Text.ToString + "," + _
                tbNilaiAkhir1.Text.ToString + "," + tbNilaiAkhir2.Text.ToString + "," + _
                tbNilaiAkhir3.Text.ToString + "," + tbNilaiAkhir4.Text.ToString + "," + tbNilaiAkhir5.Text.ToString + "," + _
                QuotedStr(ddlFgSetuju.SelectedValue) + "," + QuotedStr(tbKomentarEmp.Text) + "," + _
                QuotedStr(tbKomentarEmpAppr1.Text) + "," + QuotedStr(tbKomentarEmpAppr2.Text) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", GetDate()"
                ViewState("TransNmbr") = tbCode.Text

            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM PEEvaluasiHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed. Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE PEEvaluasiHd SET TransDate = " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + _
                ", Department = " + QuotedStr(tbDeptCode.Text) + ", CompetenceType = " + QuotedStr(ddlCompType.SelectedValue) + _
                ", EmpNumb = " + QuotedStr(tbEmp.Text) + ", EmpAppr1 = " + QuotedStr(tbEmpAppr1.Text) + _
                ", EmpAppr2 = " + QuotedStr(tbEmpAppr2.Text) + ", ReviewPeriod = " + QuotedStr(tbReviewPeriod.Text) + _
                ", ReviewReason = " + QuotedStr(ddlReviewReason.Text) + ", Kekuatan = " + QuotedStr(tbKekuatan.Text) + _
                ", Improvement = " + QuotedStr(tbImprovement.Text) + ", Pencapaian = " + QuotedStr(tbPencapaian.Text) + _
                ", KPINilai = " + tbNilai1.Text.ToString + ", UmumNilai = " + tbNilai2.Text.ToString + _
                ", FungsionalNilai = " + tbNilai3.Text.ToString + ", ProjectNilai = " + tbNilai4.Text.ToString + _
                ", TotalNilai = " + tbNilai5.Text.ToString + ", KPIBobot = " + tbBobot1.Text.ToString + _
                ", UmumBobot = " + tbBobot2.Text.ToString + ", FungsionalBobot = " + tbBobot3.Text.ToString + _
                ", ProjectBobot = " + tbBobot4.Text.ToString + ", TotalBobot = " + tbBobot5.Text.ToString + _
                ", KPINilaiAkhir = " + tbNilaiAkhir1.Text.ToString + _
                ", UmumNilaiAkhir = " + tbNilaiAkhir2.Text.ToString + _
                ", FungsionalNilaiAkhir = " + tbNilaiAkhir3.Text.ToString + _
                ", ProjectNilaiAkhir = " + tbNilaiAkhir4.Text.ToString + _
                ", TotalNilaiAkhir = " + tbNilaiAkhir5.Text.ToString + _
                ", FgSetuju = " + QuotedStr(ddlFgSetuju.SelectedValue) + _
                ", KomentarEmpAppr1 = " + QuotedStr(tbKomentarEmpAppr1.Text) + _
                ", KomentarEmp = " + QuotedStr(tbKomentarEmp.Text) + _
                ", KomentarEmpAppr2 = " + QuotedStr(tbKomentarEmpAppr2.Text) + _
                ", DatePrep = GetDate()" + _
                " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            End If
            
            SQLString = Replace(SQLString, "''", "NULL")
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

            Row = ViewState("Dt2").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbCode.Text
                Row(I).EndEdit()
            Next

            Row = ViewState("Dt3").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbCode.Text
                Row(I).EndEdit()
            Next

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, ItemNo, Indikator, Bobot, Realisasi, Target, Type, Skor FROM PEEvaluasiDt WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt As New DataTable("PEEvaluasiDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt

            'save dt2
            cmdSql = New SqlCommand("SELECT TransNmbr, ItemNo, CompetenceCode, CompetenceItem, Description1, Description2, Nilai, Type FROM PEEvaluasiDt2 WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            dbcommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt2 As New DataTable("PEEvaluasiDt2")

            Dt2 = ViewState("Dt2")
            da.Update(Dt2)
            Dt2.AcceptChanges()
            ViewState("Dt2") = Dt2

            'save dt3
            cmdSql = New SqlCommand("SELECT TransNmbr, ItemNo, ProjectName, Pencapaian, Nilai FROM PEEvaluasiDt3 WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            dbcommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt3 As New DataTable("PEEvaluasiDt3")

            Dt3 = ViewState("Dt3")
            da.Update(Dt3)
            Dt3.AcceptChanges()
            ViewState("Dt3") = Dt3
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
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail Goal / KPI must have at least 1 record")
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt2")) = 0 Then
                lbStatus.Text = MessageDlg("Detail Kompetensi must have at least 1 record")
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt3")) = 0 Then
                lbStatus.Text = MessageDlg("Detail Project must have at least 1 record")
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

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, PnlDt, GridDt)
            ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
            ModifyInput2(True, pnlInput, pnlDt3, GridDt3)
            newTrans()
            btnHome.Visible = False
            MultiView1.ActiveViewIndex = 0
            Menu1.Items.Item(0).Selected = True
            MultiView2.ActiveViewIndex = 0
            Menu2.Items.Item(0).Selected = True
            tbCode.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdddt.Click, btnAddDtke2.Click
        Try
            Cleardt()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt") = "Insert"
            MovePanel(PnlDt, pnlEditDt)
            EnableHd(False)
            StatusButtonSave(False)
            lbItemNoDt.Text = GetNewItemNo(ViewState("Dt"))
            tbIndikator.Focus()
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
            Cleardt2()
            PnlDt.Visible = True
            BindDataDt("")
            BindDataDt2("")
            BindDataDt3("")
            EnableHd(True)
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
            FDateName = "Evaluasi Date"
            FDateValue = "TransDate"
            FilterName = "Evaluasi No, Evaluasi Date"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate)"
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
                    BindDataDt2(ViewState("TransNmbr"))
                    BindDataDt3(ViewState("TransNmbr"))
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, PnlDt, GridDt)
                    ModifyInput2(False, pnlInput, pnlDt2, GridDt2)
                    ModifyInput2(False, pnlInput, pnlDt3, GridDt3)
                    MultiView1.ActiveViewIndex = 0
                    Menu1.Items.Item(0).Selected = True
                    MultiView2.ActiveViewIndex = 0
                    Menu2.Items.Item(0).Selected = True
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
                        BindDataDt2(ViewState("TransNmbr"))
                        BindDataDt3(ViewState("TransNmbr"))
                        FillTextBoxHd(ViewState("TransNmbr"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, PnlDt, GridDt)
                        ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
                        ModifyInput2(True, pnlInput, pnlDt3, GridDt3)
                        btnHome.Visible = False
                        MultiView1.ActiveViewIndex = 0
                        Menu1.Items.Item(0).Selected = True
                        MultiView2.ActiveViewIndex = 0
                        Menu2.Items.Item(0).Selected = True
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
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
                        Session("SelectCommand") = "EXEC S_PEFormEvaluasi " + QuotedStr(GVR.Cells(2).Text) + ",0," + QuotedStr(ViewState("UserId"))
                        Session("SelectCommand2") = "EXEC S_PEFormEvaluasi " + QuotedStr(GVR.Cells(2).Text) + ",1," + QuotedStr(ViewState("UserId"))
                        Session("SelectCommand3") = "EXEC S_PEFormEvaluasi " + QuotedStr(GVR.Cells(2).Text) + ",2," + QuotedStr(ViewState("UserId"))
                        Session("SelectCommand4") = "EXEC S_PEFormEvaluasi " + QuotedStr(GVR.Cells(2).Text) + ",3," + QuotedStr(ViewState("UserId"))
                        Session("ReportFile") = ".../../../Rpt/FormPEEvaluasi.frx"
                        Session("DBConnection") = ViewState("DBConnection")
                        AttachScript("openprintdlg4ds();", Page, Me.GetType)
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
            Dim dr As DataRow()
            Dim r As DataRow
            Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
            dr = ViewState("Dt").Select("ItemNo = " + GVR.Cells(1).Text)
            For Each r In dr
                r.Delete()
            Next
            CountKPINilai()
            BindGridDt(ViewState("Dt"), GridDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt2_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt2.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow
            GVR = GridDt2.Rows(e.RowIndex)
            dr = ViewState("Dt2").Select("ItemNo = " + GVR.Cells(1).Text)
            dr(0).Delete()
            BindGridDt(ViewState("Dt2"), GridDt2)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt 2 Row Deleting Error : " + ex.ToString
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
            btnSaveDt.Focus()
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub GridDt2_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt2.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt2.Rows(e.NewEditIndex)
            FillTextBoxDt2(GVR.Cells(1).Text)
            MovePanel(pnlDt2, pnlEditDt2)
            EnableHd(False)
            ViewState("StateDt2") = "Edit"
            StatusButtonSave(False)
            btnSaveDt2.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid dt2 Editing Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnSaveAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveAll.Click
        Dim CurrFilter, Value As String
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail Goal / KPI must have at least 1 record")
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt2")) = 0 Then
                lbStatus.Text = MessageDlg("Detail Kompetensi must have at least 1 record")
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt3")) = 0 Then
                lbStatus.Text = MessageDlg("Detail Project must have at least 1 record")
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
            BtnAdd_Click(Nothing, Nothing)
        Catch ex As Exception
            lbStatus.Text = "btnSaveTrans Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnEmp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEmp.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT * FROM V_MsEmployee WHERE COALESCE(Fg_Active,'N') = 'Y' AND Department LIKE '" + tbDeptCode.Text + "%'"
            ResultField = "Emp_No, Emp_Name, Job_Title, Job_Level"
            ViewState("Sender") = "btnEmp"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "Btn Emp Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbEmp_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbEmp.TextChanged
        Dim Dr As DataRow
        Dim DT As DataTable
        Try
            
            DT = SQLExecuteQuery("SELECT * FROM V_MsEmployee WHERE COALESCE(Fg_Active,'N') = 'Y' AND Department LIKE '%" + tbDeptCode.Text + "%' AND Emp_No = " + QuotedStr(tbEmp.Text), ViewState("DBConnection")).Tables(0)
            If DT.Rows.Count > 0 Then
                Dr = DT.Rows(0)
            Else
                Dr = Nothing
            End If
            'FindMaster("Employee", tbEmp.Text, ViewState("DBConnection"))
            If Not Dr Is Nothing Then
                tbEmp.Text = Dr("Emp_No")
                tbEmpName.Text = Dr("Emp_Name")
                BindToDropList(ddlJobTitle, Dr("Job_Title"))
                BindToDropList(ddlJobLevel, Dr("Job_Level"))
            Else
                tbEmp.Text = ""
                tbEmpName.Text = ""
                ddlJobTitle.SelectedValue = ""
                ddlJobLevel.SelectedValue = ""
            End If
        Catch ex As Exception
            lbStatus.Text = "tb Emp Text Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnEmpAppr1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEmpAppr1.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT * FROM V_MsEmployee WHERE COALESCE(Fg_Active,'N') = 'Y'  "
            ResultField = "Emp_No, Emp_Name, Job_Title, Job_Level"
            ViewState("Sender") = "btnEmpAppr1"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "Btn EmpAppr1 Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnEmpAppr2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEmpAppr2.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT * FROM V_MsEmployee WHERE COALESCE(Fg_Active,'N') = 'Y'  "
            ResultField = "Emp_No, Emp_Name, Job_Title, Job_Level"
            ViewState("Sender") = "btnEmpAppr2"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "Btn EmpAppr2 Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbEmpAppr1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbEmpAppr1.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Employee", tbEmpAppr1.Text, ViewState("DBConnection"))
            If Not Dr Is Nothing Then
                tbEmpAppr1.Text = Dr("Emp_No")
                tbEmpNameAppr1.Text = Dr("Emp_Name")
                BindToDropList(ddlJobTitleAppr1, Dr("Job_Title_Code"))
                BindToDropList(ddlJobLevelAppr1, Dr("Job_Level_Code"))
            Else
                tbEmpAppr1.Text = ""
                tbEmpNameAppr1.Text = ""
                ddlJobTitleAppr1.SelectedValue = ""
                ddlJobTitleAppr2.SelectedValue = ""
            End If
        Catch ex As Exception
            lbStatus.Text = "tbEmpAppr1 TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbEmpAppr2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbEmpAppr2.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Employee", tbEmpAppr2.Text, ViewState("DBConnection"))
            If Not Dr Is Nothing Then
                tbEmpAppr2.Text = Dr("Emp_No")
                tbEmpNameAppr2.Text = Dr("Emp_Name")
                BindToDropList(ddlJobTitleAppr2, Dr("Job_Title_Code"))
                BindToDropList(ddlJobLevelAppr2, Dr("Job_Level_Code"))
            Else
                tbEmpAppr2.Text = ""
                tbEmpNameAppr2.Text = ""
                ddlJobTitleAppr2.SelectedValue = ""
                ddlJobLevelAppr2.SelectedValue = ""
            End If
        Catch ex As Exception
            lbStatus.Text = "tbEmpAppr2 TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnDept_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDept.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT Dept_Code, Dept_Name FROM VMsDepartment "
            ResultField = "Dept_Code, Dept_Name"
            ViewState("Sender") = "btnDept"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnDept Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbDeptCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbDeptCode.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Department", tbDeptCode.Text, ViewState("DBConnection"))
            If Not Dr Is Nothing Then
                tbDeptCode.Text = Dr("Dept_Code")
                tbDeptName.Text = Dr("Dept_Name")
            Else
                tbDeptCode.Text = ""
                tbDeptName.Text = ""
            End If
        Catch ex As Exception
            lbStatus.Text = "tbDeptCode TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub Menu2_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu2.MenuItemClick
        Dim index As Integer
        Try
            index = Int32.Parse(e.Item.Value)
            MultiView2.ActiveViewIndex = index
            MultiView1.ActiveViewIndex = 1
            Menu1.Items.Item(1).Selected = True
            AttachScript("hitungtotal();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddDt3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt3.Click
        Try
            Cleardt3()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt3") = "Insert"
            MovePanel(pnlDt3, pnlEditDt3)
            EnableHd(False)
            StatusButtonSave(False)
            lbItemNoDt3.Text = GetNewItemNo(ViewState("Dt3"))
            tbProject.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt3 error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveDt3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt3.Click
        Try
            If CekDt3() = False Then
                btnSaveDt3.Focus()
                Exit Sub
            End If

            If ViewState("StateDt3") = "Edit" Then
                Dim Row As DataRow
                Row = ViewState("Dt3").Select("ItemNo = " + lbItemNoDt.Text)(0)
                Row.BeginEdit()
                Row("ItemNo") = lbItemNoDt3.Text
                Row("ProjectName") = tbProject.Text
                Row("Pencapaian") = tbPencapaianDt3.Text
                Row("Nilai") = tbNilaiDt3.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt3").NewRow
                dr("ItemNo") = lbItemNoDt3.Text
                dr("ProjectName") = tbProject.Text
                dr("Pencapaian") = tbPencapaianDt3.Text
                dr("Nilai") = tbNilaiDt3.Text
                ViewState("Dt3").Rows.Add(dr)
            End If
            CountProjectNilai()
            MovePanel(pnlEditDt3, pnlDt3)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
            BindGridDt(ViewState("Dt3"), GridDt3)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save Dt3 Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub

    Protected Sub GridDt3_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt3.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow
            GVR = GridDt3.Rows(e.RowIndex)
            dr = ViewState("Dt3").Select("ItemNo = " + GVR.Cells(1).Text)
            dr(0).Delete()
            CountProjectNilai()
            BindGridDt(ViewState("Dt3"), GridDt3)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt 3 Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt3_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt3.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt3.Rows(e.NewEditIndex)
            FillTextBoxDt3(GVR.Cells(1).Text)
            MovePanel(pnlDt3, pnlEditDt3)
            EnableHd(False)
            ViewState("StateDt3") = "Edit"
            StatusButtonSave(False)
            btnSaveDt3.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid dt3 Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlCompType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCompType.SelectedIndexChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("MsBobotPA", ddlCompType.SelectedValue, ViewState("DBConnection"))
            If Not Dr Is Nothing Then
                tbBobot1.Text = Dr("Bobot1").ToString
                tbBobot2.Text = Dr("Bobot2").ToString
                tbBobot3.Text = Dr("Bobot3").ToString
            Else
                tbBobot1.Text = "0"
                tbBobot2.Text = "0"
                tbBobot3.Text = "0"
            End If
            MultiView1.ActiveViewIndex = 1
            Menu1.Items.Item(1).Selected = True
            AttachScript("hitungtotal();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "ddlCompType_SelectedIndexChanged Error : " + ex.ToString
        End Try
    End Sub
    Private Sub HitungKPIDetail()
        If CFloat(tbBobotDt.Text) < 0 Then tbBobotDt.Text = "0"
        If (CFloat(tbBobotDt.Text) > 0) And (CFloat(tbTarget.Text) > 0) And (ddlCompType.SelectedValue <> "") And (CFloat(tbRealisasi.Text) > 0) Then
            If ddlTypeDt.SelectedItem.Text = "MIN" Then
                If CFloat(tbTarget.Text) <= 0 Then tbTarget.Text = "1"
                tbSkor.Text = FormatFloat(CFloat(tbBobotDt.Text) * (CFloat(tbRealisasi.Text) / CFloat(tbTarget.Text)), ViewState("DigitQty"))
            Else
                If CFloat(tbRealisasi.Text) <= 0 Then tbRealisasi.Text = "1"
                tbSkor.Text = FormatFloat(CFloat(tbBobotDt.Text) * (CFloat(tbTarget.Text) / CFloat(tbRealisasi.Text)), ViewState("DigitQty"))
            End If
        Else
            tbSkor.Text = "0"
        End If

    End Sub

    Protected Sub tbBobotDt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbBobotDt.TextChanged
        HitungKPIDetail()
    End Sub

    Protected Sub tbTarget_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbTarget.TextChanged
        HitungKPIDetail()
    End Sub

    Protected Sub tbRealisasi_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbRealisasi.TextChanged
        HitungKPIDetail()
    End Sub

    Private Sub CountKPINilai()
        Dim dr As DataRow
        Dim iQty As Double
        Try
            iQty = 0
            For Each dr In ViewState("Dt").Rows
                If Not dr.RowState = DataRowState.Deleted Then
                    iQty = iQty + CFloat(dr("Skor"))
                End If
            Next
            tbNilai1.Text = FormatFloat(iQty, ViewState("DigitQty"))
            'MultiView1.ActiveViewIndex = 1
            'Menu1.Items.Item(1).Selected = True
            AttachScript("hitungtotal();", Page, Me.GetType())
        Catch ex As Exception
            Throw New Exception("CountKPINilai Error : " + ex.ToString)
        End Try
    End Sub
    Private Sub CountProjectNilai()
        Dim dr As DataRow
        Dim iQty As Double
        Try
            iQty = 0
            For Each dr In ViewState("Dt3").Rows
                If Not dr.RowState = DataRowState.Deleted Then
                    iQty = iQty + CFloat(dr("Nilai"))
                End If
            Next
            tbNilai4.Text = FormatFloat(iQty, ViewState("DigitQty"))
            'MultiView1.ActiveViewIndex = 1
            'Menu1.Items.Item(1).Selected = True
            AttachScript("hitungtotal();", Page, Me.GetType())
        Catch ex As Exception
            Throw New Exception("CountProjectNilai Error : " + ex.ToString)
        End Try
    End Sub
    Private Sub CountKompetensiNilai()
        Dim dr As DataRow
        Dim QtyU, QtyK As Double
        'Dim N5 As Double
        Try
            QtyU = 0
            QtyK = 0
            For Each dr In ViewState("Dt2").Select("Type = 'UMUM'")
                If Not dr.RowState = DataRowState.Deleted Then
                    QtyU = QtyU + 1
                End If
            Next
            For Each dr In ViewState("Dt2").Select("Type = 'KHUSUS'")
                If Not dr.RowState = DataRowState.Deleted Then
                    QtyK = QtyK + 1
                End If
            Next
            tbNilai2.Text = FormatFloat(QtyU, ViewState("DigitQty"))
            tbNilai3.Text = FormatFloat(QtyK, ViewState("DigitQty"))
            MultiView1.ActiveViewIndex = 1
            Menu1.Items.Item(1).Selected = True
            AttachScript("hitungtotal();", Page, Me.GetType())
        Catch ex As Exception
            Throw New Exception("CountKompetensiNilai Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnGetKompetensi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetKompetensi.Click
        Try
            Dim dt As New DataTable
            Dim dr, drw As DataRow
            Dim r As DataRow()
            Dim Row As DataRow
            If CekHd() = False Then
                Exit Sub
            End If
            dt = SQLExecuteQuery("EXEC S_PEEvaluasiGetDt " + QuotedStr(tbDeptCode.Text) + "," + QuotedStr(ddlCompType.SelectedValue) + "," + QuotedStr(ddlJobLevel.SelectedValue), ViewState("DBConnection").ToString).Tables(0)
            If dt.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("No data")
            End If
            For Each dr In dt.Rows
                r = ViewState("Dt2").Select(" Type = " + QuotedStr(dr("Type")) + " AND CompetenceCode = " + QuotedStr(dr("Competence")) + " AND CompetenceItem = " + dr("ItemNo").ToString)
                If r.Count = 0 Then
                    drw = ViewState("Dt2").NewRow
                    drw("ItemNo") = dr("Nmbr")
                    drw("CompetenceCode") = dr("Competence")
                    drw("CompetenceName") = dr("CompetenceName")
                    drw("CompetenceItem") = dr("ItemNo")
                    drw("Description1") = dr("Description1")
                    drw("Description2") = dr("Description2")
                    drw("Nilai") = 0
                    drw("Type") = dr("Type")
                    ViewState("Dt2").Rows.Add(drw)
                Else
                    Row = ViewState("Dt2").Select(" Type = " + QuotedStr(dr("Type")) + " AND CompetenceCode = " + QuotedStr(dr("Competence")) + " AND CompetenceItem = " + dr("ItemNo").ToString)(0)
                    Row.BeginEdit()
                    Row("Description1") = dr("Description1")
                    Row("Description2") = dr("Description2")
                    Row("Nilai") = 0
                    Row("Type") = dr("Type")
                    Row.EndEdit()
                End If
            Next
            BindGridDt(ViewState("Dt2"), GridDt2)
            CountKompetensiNilai()
        Catch ex As Exception
            lbStatus.Text = "btnGetKompetensi_Click Error : " + ex.ToString
        End Try
        
    End Sub

    Protected Sub btnSaveDt2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt2.Click
        Try
            If CekDt2() = False Then
                btnSaveDt2.Focus()
                Exit Sub
            End If

            If ViewState("StateDt2") = "Edit" Then
                Dim Row As DataRow
                Row = ViewState("Dt2").Select("ItemNo = " + lbItemNoDt2.Text)(0)
                Row.BeginEdit()                
                Row("Nilai") = tbNilaiDt2.Text
                Row.EndEdit()
            Else
                ''Insert
                'Dim dr As DataRow
                'dr = ViewState("Dt3").NewRow
                'dr("ItemNo") = lbItemNoDt3.Text
                'dr("ProjectName") = tbProject.Text
                'dr("Pencapaian") = tbPencapaianDt3.Text
                'dr("Nilai") = tbNilaiDt3.Text
                'ViewState("Dt3").Rows.Add(dr)
            End If
            CountKompetensiNilai()
            MovePanel(pnlEditDt2, pnlDt2)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
            BindGridDt(ViewState("Dt2"), GridDt2)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save Dt2 Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub
    Function CekDt2(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not Dr Is Nothing Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If CFloat(Dr("Nilai")) <= 0 Then
                    lbStatus.Text = MessageDlg("Nilai Must Have Value")
                    Return False
                End If

            Else
                If CFloat(tbNilaiDt2.Text.Trim) < 0 Then
                    lbStatus.Text = MessageDlg("Nilai Must Have Value")
                    tbNilaiDt2.Focus()
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt2 Error : " + ex.ToString)
        End Try
    End Function
    Protected Sub ddlTypeDt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTypeDt.TextChanged
        HitungKPIDetail()
    End Sub
    Function CekDt3(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not Dr Is Nothing Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("ProjectName").ToString = "" Then
                    lbStatus.Text = MessageDlg("Project Name Must Have Value")
                    Return False
                End If
                If Dr("Pencapaian").ToString = "" Then
                    lbStatus.Text = MessageDlg("Pencapaian Must Have Value")
                    Return False
                End If
                If CFloat(Dr("Nilai")) <= 0 Then
                    lbStatus.Text = MessageDlg("Nilai Must Have Value")
                    Return False
                End If

            Else
                If tbProject.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Project Name Must Have Value")
                    tbProject.Focus()
                    Return False
                End If
                If tbPencapaianDt3.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Pencapaian Must Have Value")
                    tbPencapaianDt3.Focus()
                    Return False
                End If
                If CFloat(tbNilaiDt3.Text.Trim) < 0 Then
                    lbStatus.Text = MessageDlg("Nilai Must Have Value")
                    tbNilaiDt3.Focus()
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt3 Error : " + ex.ToString)
        End Try
    End Function
End Class
