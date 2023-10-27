Imports System.Data
Imports System.Data.SqlClient
Partial Class Transaction_TrProbation_TrProbation
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "SELECT * FROM V_PEProbationHd"

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
                If ViewState("Sender") = "btnEmp" Then
                    tbEmpNo.Text = Session("Result")(0).ToString
                    tbEmpName.Text = Session("Result")(1).ToString
                    ddlDepartment.SelectedValue = Session("Result")(2).ToString
                    ddlJobTitle.SelectedValue = Session("Result")(3).ToString
                ElseIf ViewState("Sender") = "btnAppr1" Then
                    tbAppr1.Text = Session("Result")(0).ToString
                    tbApprName1.Text = Session("Result")(1).ToString                    
                ElseIf ViewState("Sender") = "btnAppr2" Then
                    tbAppr2.Text = Session("Result")(0).ToString
                    tbApprName2.Text = Session("Result")(1).ToString
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
            
            ViewState("SortExpression") = Nothing
            ViewState("DigitCurr") = 0
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
            If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
                'ddlCommand.Items.Add("Print")
                'ddlCommand2.Items.Add("Print")
            End If
            tbKehadiran1.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbKehadiran2.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbKehadiran3.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbKehadiran4.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbKehadiran5.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbKehadiran6.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbLate1.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbLate2.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbLate3.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbLate4.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbLate5.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbLate6.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbKPI.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbUmum.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbFungsional.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbTotal.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbKPI.Attributes.Add("ReadOnly", "True")
            tbUmum.Attributes.Add("ReadOnly", "True")
            tbFungsional.Attributes.Add("ReadOnly", "True")
            tbTotal.Attributes.Add("ReadOnly", "True")
            tbApprName1.Attributes.Add("ReadOnly", "True")
            tbApprName2.Attributes.Add("ReadOnly", "True")
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
        Return "SELECT * FROM V_PEProbationDt WHERE TransNmbr = " + QuotedStr(Nmbr)
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
                Session("SelectCommand") = "EXEC S_FNFormBuktiBank " + Result + ",'CASHOUT'"
                Session("ReportFile") = ".../../../Rpt/FormBuktiBank.frx"
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
                        Result = ExecSPCommandGo(ActionValue, "S_PEProbation", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            tbEmpNo.Enabled = State
            btnEmp.Visible = State
            tbPeriod.Enabled = State
            tbKehadiran1.Enabled = State
            tbKehadiran2.Enabled = State
            tbKehadiran3.Enabled = State
            tbKehadiran4.Enabled = State
            tbKehadiran5.Enabled = State
            tbKehadiran6.Enabled = State
            tbLate1.Enabled = State
            tbLate2.Enabled = State
            tbLate3.Enabled = State
            tbLate4.Enabled = State
            tbLate5.Enabled = State
            tbLate6.Enabled = State
            tbAppr1.Enabled = State
            btnAppr1.Visible = State
            tbAppr2.Enabled = State
            btnAppr2.Visible = State
            ddlLulus.Enabled = State
            ddlDepartment.Enabled = False
            ddlJobTitle.Enabled = False
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
            'BtnGo.Visible = True
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
            tbEmpNo.Text = ""
            tbEmpName.Text = ""
            tbPeriod.Text = ""
            tbKehadiran1.Text = "0"
            tbKehadiran2.Text = "0"
            tbKehadiran3.Text = "0"
            tbKehadiran4.Text = "0"
            tbKehadiran5.Text = "0"
            tbKehadiran6.Text = "0"
            tbLate1.Text = "0"
            tbLate2.Text = "0"
            tbLate3.Text = "0"
            tbLate4.Text = "0"
            tbLate5.Text = "0"
            tbLate6.Text = "0"
            tbKPI.Text = "0"
            tbUmum.Text = "0"
            tbFungsional.Text = "0"
            tbTotal.Text = "0"
            tbAppr1.Text = ""
            tbApprName1.Text = ""
            tbApprName2.Text = ""
            tbAppr2.Text = ""
            tbRemark.Text = ""
            ddlDepartment.SelectedIndex = 0
            ddlJobTitle.SelectedIndex = 0
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbKegiatan.Text = ""
            tbInstruktur.Text = ""
            tbRemarkDt.Text = ""           
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
            If tbEmpNo.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Employee Must Have Value")
                tbEmpNo.Focus()
                Return False
            End If
            If tbPeriod.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Periode Penilaian Must Have Value")
                tbPeriod.Focus()
                Return False
            End If
            If tbAppr1.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Approval 1 Must Have Value")
                tbAppr1.Focus()
                Return False
            End If
            If tbAppr2.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Approval 2 Must Have Value")
                tbAppr2.Focus()
                Return False
            End If
            If (CFloat(tbKehadiran1.Text) > 100) Or (CFloat(tbKehadiran1.Text) < 0) Then
                lbStatus.Text = MessageDlg("Failed...  value cannot greater than 100%")
                tbKehadiran1.Focus()
                Return False
            End If
            If (CFloat(tbKehadiran2.Text) > 100) Or (CFloat(tbKehadiran2.Text) < 0) Then
                lbStatus.Text = MessageDlg("Failed...  value cannot greater than 100%")
                tbKehadiran2.Focus()
                Return False
            End If
            If (CFloat(tbKehadiran3.Text) > 100) Or (CFloat(tbKehadiran3.Text) < 0) Then
                lbStatus.Text = MessageDlg("Failed...  value cannot greater than 100%")
                tbKehadiran3.Focus()
                Return False
            End If
            If (CFloat(tbKehadiran4.Text) > 100) Or (CFloat(tbKehadiran4.Text) < 0) Then
                lbStatus.Text = MessageDlg("Failed...  value cannot greater than 100%")
                tbKehadiran4.Focus()
                Return False
            End If
            If (CFloat(tbKehadiran5.Text) > 100) Or (CFloat(tbKehadiran5.Text) < 0) Then
                lbStatus.Text = MessageDlg("Failed...  value cannot greater than 100%")
                tbKehadiran5.Focus()
                Return False
            End If
            If (CFloat(tbKehadiran6.Text) > 100) Or (CFloat(tbKehadiran6.Text) < 0) Then
                lbStatus.Text = MessageDlg("Failed...  value cannot greater than 100%")
                tbKehadiran6.Focus()
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
                'If tbEmpNo.Text.Trim = "" Then
                '    lbStatus.Text = MessageDlg("Employee Must Have Value")
                '    tbEmpNo.Focus()
                '    Return False
                'End If
                'If tbIndisInfo.Text.Trim = "" Then
                '    lbStatus.Text = MessageDlg("Indiscipliner Info Must Have Value")
                '    tbIndisInfo.Focus()
                '    Return False
                'End If
                'If tbEmpAppr2.Text.Trim = "" Then
                '    lbStatus.Text = MessageDlg("Emp Approval 2 must have value")
                '    tbEmpAppr2.Focus()
                '    Return False
                'End If
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
            BindToText(tbEmpNo, Dt.Rows(0)("EmpNumb").ToString)
            BindToText(tbEmpName, Dt.Rows(0)("EmpName").ToString)
            BindToDropList(ddlDepartment, Dt.Rows(0)("Dept_Code").ToString)
            BindToDropList(ddlJobTitle, Dt.Rows(0)("Job_Title_Code").ToString)
            BindToText(tbPeriod, Dt.Rows(0)("AssessmentPeriod").ToString)
            BindToText(tbKehadiran1, Dt.Rows(0)("AttendPerc1").ToString)
            BindToText(tbLate1, Dt.Rows(0)("LateMinute1").ToString)
            BindToText(tbKehadiran2, Dt.Rows(0)("AttendPerc2").ToString)
            BindToText(tbLate2, Dt.Rows(0)("LateMinute2").ToString)
            BindToText(tbKehadiran3, Dt.Rows(0)("AttendPerc3").ToString)
            BindToText(tbLate3, Dt.Rows(0)("LateMinute3").ToString)
            BindToText(tbKehadiran4, Dt.Rows(0)("AttendPerc4").ToString)
            BindToText(tbLate4, Dt.Rows(0)("LateMinute4").ToString)
            BindToText(tbKehadiran5, Dt.Rows(0)("AttendPerc5").ToString)
            BindToText(tbLate5, Dt.Rows(0)("LateMinute5").ToString)
            BindToText(tbKehadiran6, Dt.Rows(0)("AttendPerc6").ToString)
            BindToText(tbLate6, Dt.Rows(0)("LateMinute6").ToString)
            BindToDropList(ddlLulus, Dt.Rows(0)("FgLulus").ToString)
            BindToText(tbKPI, Dt.Rows(0)("KPI").ToString)
            BindToText(tbUmum, Dt.Rows(0)("KompetensiUmum").ToString)
            BindToText(tbFungsional, Dt.Rows(0)("KompetensiFungsional").ToString)
            BindToText(tbTotal, Dt.Rows(0)("Total").ToString)
            BindToText(tbAppr1, Dt.Rows(0)("EmpAppr1").ToString)
            BindToText(tbApprName1, Dt.Rows(0)("EmpNameAppr1").ToString)
            BindToText(tbAppr2, Dt.Rows(0)("EmpAppr2").ToString)
            BindToText(tbApprName2, Dt.Rows(0)("EmpNameAppr2").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("ItemNo = " + ItemNo)
            If Dr.Length > 0 Then
                lbNo.Text = ItemNo
                BindToText(tbKegiatan, Dr(0)("Kegiatan").ToString)
                BindToText(tbInstruktur, Dr(0)("Instruktur").ToString)
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

                Row = ViewState("Dt").Select("ItemNo = " + lbNo.Text)(0)
                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                Row("Kegiatan") = tbKegiatan.Text
                Row("Instruktur") = tbInstruktur.Text
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("ItemNo") = CInt(lbNo.Text)
                dr("Kegiatan") = tbKegiatan.Text
                dr("Instruktur") = tbInstruktur.Text
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
                tbCode.Text = GetAutoNmbr("PRB", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO PEProbationHd (TransNmbr, TransDate, Status, EmpNumb, " + _
                "AssessmentPeriod, EmpAppr1, EmpAppr2, FgLulus, AttendPerc1, LateMinute1, AttendPerc2, LateMinute2, AttendPerc3, LateMinute3, AttendPerc4, LateMinute4, AttendPerc5, LateMinute5, AttendPerc6, LateMinute6, " + _
                "KPI, KompetensiUmum, KompetensiFungsional, Total, Remark, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", 'H', " + QuotedStr(tbEmpNo.Text) + ", " + _
                QuotedStr(tbPeriod.Text) + ", " + QuotedStr(tbAppr1.Text) + ", " + QuotedStr(tbAppr2.Text) + ", " + QuotedStr(ddlLulus.SelectedValue) + ", " + _
                tbKehadiran1.Text + ", " + tbLate1.Text + ", " + tbKehadiran2.Text + ", " + tbLate2.Text + ", " + tbKehadiran3.Text + ", " + tbLate3.Text + ", " + tbKehadiran4.Text + ", " + tbLate4.Text + ", " + tbKehadiran5.Text + ", " + tbLate5.Text + ", " + tbKehadiran6.Text + ", " + tbLate6.Text + ", " + _
                tbKPI.Text + ", " + tbUmum.Text + ", " + tbFungsional.Text + ", " + tbTotal.Text + ", " + QuotedStr(tbRemark.Text) + _
                ", " + QuotedStr(ViewState("UserId").ToString) + ", GetDate() "

                ViewState("TransNmbr") = tbCode.Text

            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("SELECT Status FROM PEProbationHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE PEProbationHd SET EmpNumb = " + QuotedStr(tbEmpNo.Text) + _
                ", AssessmentPeriod =" + QuotedStr(tbPeriod.Text) + ", EmpAppr1 = " + QuotedStr(tbAppr1.Text) + ", EmpAppr2 = " + QuotedStr(tbAppr2.Text) + _
                ", FgLulus= " + QuotedStr(ddlLulus.SelectedValue) + ", AttendPerc1 = " + tbKehadiran1.Text + ", LateMinute1 = " + tbLate1.Text + _
                ", AttendPerc2 = " + tbKehadiran2.Text + ", LateMinute2 = " + tbLate2.Text + ", AttendPerc3 = " + tbKehadiran3.Text + ", LateMinute3 = " + tbLate3.Text + _
                ", AttendPerc4 = " + tbKehadiran4.Text + ", LateMinute4 = " + tbLate4.Text + ", AttendPerc5 = " + tbKehadiran5.Text + ", LateMinute5 = " + tbLate5.Text + _
                ", AttendPerc6 = " + tbKehadiran6.Text + ", LateMinute6 = " + tbLate6.Text + ", KPI = " + tbKPI.Text + ", KompetensiUmum = " + tbUmum.Text + ", KompetensiFungsional = " + tbFungsional.Text + _
                ", Total = " + tbTotal.Text + ", Remark =" + QuotedStr(tbRemark.Text) + _
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
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, ItemNo, Kegiatan, Instruktur, Remark FROM PEProbationDt WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand

            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
                    "UPDATE PEProbationDt SET Kegiatan = @Kegiatan, Instruktur = @Instruktur, Remark = @Remark WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND ItemNo = @OldItemNo", con)
            ' Define output parameters.
            Update_Command.Parameters.Add("@Kegiatan", SqlDbType.VarChar, 255, "Kegiatan")
            Update_Command.Parameters.Add("@Instruktur", SqlDbType.VarChar, 50, "Instruktur")
            Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 255, "Remark")
            ' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@OldItemNo", SqlDbType.Int, 10, "ItemNo")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM PEProbationDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND ItemNo = @ItemNo", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@ItemNo", SqlDbType.Int, 10, "ItemNo")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("PEProbationDt")

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
            'If GetCountRecord(ViewState("Dt")) = 0 Then
            '    lbStatus.Text = MessageDlg("Detail must have at least 1 record")
            '    Exit Sub
            'End If

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
            lbNo.Text = GetNewItemNo(ViewState("Dt"))
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
            FDateName = "Date"
            FDateValue = "TransDate"
            FilterName = "Probation No, Status, Employee No, Employee Name, Organization, Job Title, Periode Penilaian, Lulus, Approval1, Approval2, Remark"
            FilterValue = "TransNmbr, Status, EmpNumb, EmpName, Dept_Name, Job_Title_Name, AssessmentPeriod,  FgLulus, EmpNameAppr1, EmpNameAppr2, Remark"
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
                        Session("SelectCommand") = "EXEC S_PEFormProbation '" + GVR.Cells(2).Text + "'"
                        Session("ReportFile") = ".../../../Rpt/FormTrProbation.frx"
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

            dr = ViewState("Dt").Select("ItemNo = " + QuotedStr(GVR.Cells(1).Text))
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
            ViewState("Item") = GVR.Cells(1).Text
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
            'If GetCountRecord(ViewState("Dt")) = 0 Then
            '    lbStatus.Text = MessageDlg("Detail must have at least 1 record")
            '    Exit Sub
            'End If
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
            Session("DBConnection") = ViewState("DbConnection")
            Session("filter") = "SELECT Emp_No, Emp_Name, Department, Department_Name, Job_Title, Job_Title_Name  FROM V_MsEmployee WHERE Fg_Active = 'Y' AND Emp_No <> " + QuotedStr(tbAppr1.Text) + " AND Emp_No <> " + QuotedStr(tbAppr2.Text)
            ResultField = "Emp_No, Emp_Name, Department, Job_Title"
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
        Try
            Ds = SQLExecuteQuery("SELECT Emp_No, Emp_Name, Department, Department_Name, Job_Title, Job_Title_Name FROM V_MsEmployee WHERE Fg_Active = 'Y' AND Emp_No = " + QuotedStr(tbEmpNo.Text) + " AND Emp_No <> " + QuotedStr(tbAppr1.Text) + " AND Emp_No <> " + QuotedStr(tbAppr2.Text), ViewState("DBConnection").ToString)
            If Ds.Tables(0).Rows.Count = 1 Then
                Dr = Ds.Tables(0).Rows(0)
                tbEmpNo.Text = TrimStr(Dr("Emp_No").ToString)
                tbEmpName.Text = TrimStr(Dr("Emp_Name").ToString)
                ddlDepartment.Text = TrimStr(Dr("Department").ToString)
                ddlJobTitle.Text = TrimStr(Dr("Job_Title").ToString)
            Else
                tbEmpNo.Text = ""
                tbEmpName.Text = ""
                ddlDepartment.SelectedIndex = 0
                ddlJobTitle.SelectedIndex = 0
            End If

            GetNilai(tbEmpNo.Text, Format(tbDate.SelectedValue, "yyyy-MM-dd"))

        Catch ex As Exception
            Throw New Exception("tbEmpNo_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnAppr1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAppr1.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DbConnection")
            Session("filter") = "SELECT Emp_No, Emp_Name, Job_Title, Job_Title_Name FROM V_MsEmployee WHERE Fg_Active = 'Y' AND Emp_No <> " + QuotedStr(tbAppr2.Text) + " AND Emp_No <> " + QuotedStr(tbEmpNo.Text)
            ResultField = "Emp_No, Emp_Name"
            ViewState("Sender") = "btnAppr1"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnAppr1_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAppr1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAppr1.TextChanged
        Dim Dr As DataRow
        Dim Ds As DataSet
        Try
            Ds = SQLExecuteQuery("SELECT Emp_No, Emp_Name, Job_Title, Job_Title_Name FROM V_MsEmployee WHERE Fg_Active = 'Y' AND Emp_No = " + QuotedStr(tbAppr1.Text) + " AND Emp_No <> " + QuotedStr(tbAppr2.Text) + " AND Emp_No <> " + QuotedStr(tbEmpNo.Text), ViewState("DBConnection").ToString)
            If Ds.Tables(0).Rows.Count = 1 Then
                Dr = Ds.Tables(0).Rows(0)
                tbAppr1.Text = TrimStr(Dr("Emp_No").ToString)
                tbApprName1.Text = TrimStr(Dr("Emp_Name").ToString)
            Else
                tbAppr1.Text = ""
                tbApprName1.Text = ""
            End If

        Catch ex As Exception
            Throw New Exception("tbEmpAppr1_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnAppr2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAppr2.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DbConnection")
            Session("filter") = "SELECT Emp_No, Emp_Name, Job_Title, Job_Title_Name FROM V_MsEmployee WHERE Fg_Active = 'Y' AND Emp_No <> " + QuotedStr(tbAppr1.Text) + " AND Emp_No <> " + QuotedStr(tbEmpNo.Text)
            ResultField = "Emp_No, Emp_Name"
            ViewState("Sender") = "btnAppr2"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnAppr2_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAppr2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAppr2.TextChanged
        Dim Dr As DataRow
        Dim Ds As DataSet
        Try
            Ds = SQLExecuteQuery("SELECT Emp_No, Emp_Name, Job_Title, Job_Title_Name FROM V_MsEmployee WHERE Fg_Active = 'Y' AND Emp_No = " + QuotedStr(tbAppr2.Text) + " AND Emp_No <> " + QuotedStr(tbAppr1.Text) + " AND Emp_No <> " + QuotedStr(tbEmpNo.Text), ViewState("DBConnection").ToString)
            If Ds.Tables(0).Rows.Count = 1 Then
                Dr = Ds.Tables(0).Rows(0)
                tbAppr2.Text = TrimStr(Dr("Emp_No").ToString)
                tbApprName2.Text = TrimStr(Dr("Emp_Name").ToString)
            Else
                tbAppr2.Text = ""
                tbApprName2.Text = ""
            End If

        Catch ex As Exception
            Throw New Exception("tbAppr2_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub GetNilai(ByVal Emp As String, ByVal Tanggal As DateTime)
        Dim Dr As DataRow
        Dim Ds As DataSet
        Try
            Ds = SQLExecuteQuery("EXEC S_PEProbationGetNilai " + QuotedStr(Emp) + ", " + QuotedStr(Tanggal), ViewState("DBConnection").ToString)
            If Ds.Tables(0).Rows.Count = 1 Then
                Dr = Ds.Tables(0).Rows(0)
                tbKPI.Text = TrimStr(Dr("NilaiKPI").ToString)
                tbUmum.Text = TrimStr(Dr("NilaiKompetensiUmum").ToString)
                tbFungsional.Text = TrimStr(Dr("NilaiKompetensiFungsional").ToString)
            Else
                tbKPI.Text = "0"
                tbUmum.Text = "0"
                tbFungsional.Text = "0"
            End If

        Catch ex As Exception
            Throw New Exception("GetNilai Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbKehadiran1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbKehadiran1.TextChanged
        Try
            If (CFloat(tbKehadiran1.Text) > 100) Or (CFloat(tbKehadiran1.Text) < 0) Then
                lbStatus.Text = MessageDlg("Failed...  value cannot greater than 100%")
                tbKehadiran1.Focus()
                Exit Sub
            End If
        Catch ex As Exception
            Throw New Exception("tbKehadiran1_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbKehadiran2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbKehadiran2.TextChanged
        Try
            If (CFloat(tbKehadiran2.Text) > 100) Or (CFloat(tbKehadiran2.Text) < 0) Then
                lbStatus.Text = MessageDlg("Failed...  value cannot greater than 100%")
                tbKehadiran2.Focus()
                Exit Sub
            End If
        Catch ex As Exception
            Throw New Exception("tbKehadiran2_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbKehadiran3_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbKehadiran3.TextChanged
        Try
            If (CFloat(tbKehadiran3.Text) > 100) Or (CFloat(tbKehadiran3.Text) < 0) Then
                lbStatus.Text = MessageDlg("Failed...  value cannot greater than 100%")
                tbKehadiran3.Focus()
                Exit Sub
            End If
        Catch ex As Exception
            Throw New Exception("tbKehadiran3_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbKehadiran4_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbKehadiran4.TextChanged
        Try
            If (CFloat(tbKehadiran4.Text) > 100) Or (CFloat(tbKehadiran4.Text) < 0) Then
                lbStatus.Text = MessageDlg("Failed...  value cannot greater than 100%")
                tbKehadiran4.Focus()
                Exit Sub
            End If
        Catch ex As Exception
            Throw New Exception("tbKehadiran4_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbKehadiran5_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbKehadiran5.TextChanged
        Try
            If (CFloat(tbKehadiran5.Text) > 100) Or (CFloat(tbKehadiran5.Text) < 0) Then
                lbStatus.Text = MessageDlg("Failed...  value cannot greater than 100%")
                tbKehadiran5.Focus()
                Exit Sub
            End If
        Catch ex As Exception
            Throw New Exception("tbKehadiran5_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbKehadiran6_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbKehadiran6.TextChanged
        Try
            If (CFloat(tbKehadiran6.Text) > 100) Or (CFloat(tbKehadiran6.Text) < 0) Then
                lbStatus.Text = MessageDlg("Failed...  value cannot greater than 100%")
                tbKehadiran6.Focus()
                Exit Sub
            End If
        Catch ex As Exception
            Throw New Exception("tbKehadiran6_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbKPI_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbKPI.TextChanged, tbUmum.TextChanged, tbFungsional.TextChanged
        Try
            tbTotal.Text = CFloat(tbKPI.Text) + CFloat(tbUmum.Text) + CFloat(tbFungsional.Text)
        Catch ex As Exception
            Throw New Exception("tbKPI_TextChanged Error : " + ex.ToString)
        End Try
    End Sub
End Class
