Imports System.Data
Imports System.Data.SqlClient
Partial Class Transaction_TrEmpReprimand_TrEmpReprimand
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "SELECT * FROM V_PEEmpReprimandHd"

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
            BtnGo.Visible = ddlCommand.Visible
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnEmpAppr1" Then
                    tbEmpAppr1.Text = Session("Result")(0).ToString
                    tbEmpNameAppr1.Text = Session("Result")(1).ToString
                    ddlJbtAppr1.SelectedValue = Session("Result")(2).ToString
                ElseIf ViewState("Sender") = "btnEmp" Then
                    tbEmpNo.Text = Session("Result")(0).ToString
                    tbEmpName.Text = Session("Result")(1).ToString
                    ddlDepartment.SelectedValue = Session("Result")(2).ToString
                    ddlJobTitle.SelectedValue = Session("Result")(3).ToString
                ElseIf ViewState("Sender") = "btnEmpAppr2" Then
                    tbEmpAppr2.Text = Session("Result")(0).ToString
                    tbEmpNameAppr2.Text = Session("Result")(1).ToString
                    ddlJbtAppr2.SelectedValue = Session("Result")(2).ToString
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
            FillCombo(ddlJbtAppr1, "SELECT * FROM VMsJobTitle", True, "Job_Title_Code", "Job_Title_Name", ViewState("DBConnection"))
            FillCombo(ddlDepartment, "SELECT Dept_Code, Dept_Name FROM VMsDepartment", True, "Dept_Code", "Dept_Name", ViewState("DBConnection"))
            FillCombo(ddlJobTitle, "SELECT * FROM VMsJobTitle", True, "Job_Title_Code", "Job_Title_Name", ViewState("DBConnection"))
            FillCombo(ddlJobTitle, "SELECT * FROM VMsJobTitle ", True, "Job_Title_Code", "Job_Title_Name", ViewState("DBConnection"))
            FillCombo(ddlJbtAppr2, "SELECT * FROM VMsJobTitle", True, "Job_Title_Code", "Job_Title_Name", ViewState("DBConnection"))
            
            ViewState("SortExpression") = Nothing
            ViewState("DigitCurr") = 0
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
            If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
                'ddlCommand.Items.Add("Print")
                'ddlCommand2.Items.Add("Print")
            End If
            tbMasaBerlaku.Attributes.Add("OnKeyDown", "return PressNumeric();")
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
        Return "SELECT * FROM V_PEEmpReprimandDt WHERE TransNmbr = " + QuotedStr(Nmbr)
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
                        Result = ExecSPCommandGo(ActionValue, "S_PEEmpReprimand", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            tbEffectiveDate.Enabled = State
            ddlIndisStatus.Enabled = State
            ddlIndisType.Enabled = State
            tbMasaBerlaku.Enabled = State
            tbEmpAppr1.Enabled = State
            btnEmpAppr1.Visible = State
            ddlJbtAppr1.Enabled = False
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
            tbEffectiveDate.SelectedDate = ViewState("ServerDate")
            ddlIndisType.SelectedValue = "Teguran"
            ddlIndisStatus.SelectedValue = "Lisan"
            tbMasaBerlaku.Text = "0"
            tbEmpAppr1.Text = ""
            tbEmpNameAppr1.Text = ""
            ddlJbtAppr1.SelectedIndex = 0
            ddlJbtAppr1.Enabled = False
            tbKerjaBersama.Text = ""
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
            tbIndisInfo.Text = ""
            tbStatementEmp.Text = ""
            tbStatementAppr.Text = ""
            tbEmpAppr2.Text = ""
            tbEmpNameAppr2.Text = ""
            ddlJbtAppr2.SelectedIndex = 0
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
            If tbEffectiveDate.SelectedDate = Nothing Then
                lbStatus.Text = MessageDlg("Effective Date must have value")
                tbDate.Focus()
                Return False
            End If
            If ddlIndisType.SelectedValue.Trim = "" Then
                lbStatus.Text = MessageDlg("Indiscipliner Type Must Have Value")
                ddlIndisType.Focus()
                Return False
            End If
            If ddlIndisStatus.SelectedValue.Trim = "" Then
                lbStatus.Text = MessageDlg("Indiscipliner Status Must Have Value")
                ddlIndisStatus.Focus()
                Return False
            End If
            If CInt(tbMasaBerlaku.Text) <= "0" Then
                lbStatus.Text = MessageDlg("Masa Berlaku Must Have Value")
                tbMasaBerlaku.Focus()
                Return False
            End If
            If tbEmpAppr1.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Emp Approval 1 Must Have Value")
                tbEmpAppr1.Focus()
                Return False
            End If
            If tbKerjaBersama.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Perjanjian Kerja Bersama Must Have Value")
                tbKerjaBersama.Focus()
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
                    lbStatus.Text = MessageDlg("Employee Must Have Value")
                    tbEmpNo.Focus()
                    Return False
                End If
                If tbIndisInfo.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Indiscipliner Info Must Have Value")
                    tbIndisInfo.Focus()
                    Return False
                End If
                If tbEmpAppr2.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Emp Approval 2 must have value")
                    tbEmpAppr2.Focus()
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
            BindToDate(tbEffectiveDate, Dt.Rows(0)("EffectiveDate").ToString)
            BindToDropList(ddlIndisType, Dt.Rows(0)("IndisciplinerType").ToString)
            BindToDropList(ddlIndisStatus, Dt.Rows(0)("IndisciplinerStatus").ToString)
            BindToText(tbMasaBerlaku, Dt.Rows(0)("MasaBerlaku").ToString)
            BindToText(tbEmpAppr1, Dt.Rows(0)("EmpAppr1").ToString)
            BindToText(tbEmpNameAppr1, Dt.Rows(0)("EmpNameAppr1").ToString)
            BindToDropList(ddlJbtAppr1, Dt.Rows(0)("JbtAppr1").ToString)
            BindToText(tbKerjaBersama, Dt.Rows(0)("Perjanjian").ToString)
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
                BindToText(tbEmpName, Dr(0)("Emp_Name").ToString)
                BindToDropList(ddlDepartment, Dr(0)("Department").ToString)
                BindToDropList(ddlJobTitle, Dr(0)("JobTitle").ToString)
                BindToText(tbIndisInfo, Dr(0)("IndisciplinerInfo").ToString)
                BindToText(tbStatementEmp, Dr(0)("StatementEmp").ToString)
                BindToText(tbStatementAppr, Dr(0)("StatementAppr").ToString)
                BindToText(tbEmpAppr2, Dr(0)("EmpAppr2").ToString)
                BindToDropList(ddlJbtAppr2, Dr(0)("JbtAppr2").ToString)
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
                Row("Emp_Name") = tbEmpName.Text
                Row("Department") = ddlDepartment.SelectedValue
                Row("DeptName") = ddlDepartment.SelectedItem.Text
                Row("JobTitle") = ddlJobTitle.SelectedValue
                Row("JobTitleName") = ddlJobTitle.SelectedItem.Text
                Row("IndisciplinerInfo") = tbIndisInfo.Text
                Row("StatementEmp") = tbStatementEmp.Text
                Row("StatementAppr") = tbStatementAppr.Text
                Row("EmpAppr2") = tbEmpAppr2.Text
                Row("EmpNameAppr2") = tbEmpNameAppr2.Text
                Row("JbtAppr2") = ddlJbtAppr2.SelectedValue
                Row("JbtNameAppr2") = ddlJbtAppr2.SelectedItem.Text
                Row("Remark") = tbRemarkDt.Text
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
                dr("Department") = ddlDepartment.SelectedValue
                dr("DeptName") = ddlDepartment.SelectedItem.Text
                dr("JobTitle") = ddlJobTitle.SelectedValue
                dr("JobTitleName") = ddlJobTitle.SelectedItem.Text
                dr("IndisciplinerInfo") = tbIndisInfo.Text
                dr("StatementEmp") = tbStatementEmp.Text
                dr("StatementAppr") = tbStatementAppr.Text
                dr("EmpAppr2") = tbEmpAppr2.Text
                dr("EmpNameAppr2") = tbEmpNameAppr2.Text
                dr("JbtAppr2") = ddlJbtAppr2.SelectedValue
                dr("JbtNameAppr2") = ddlJbtAppr2.SelectedItem.Text
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
                If ddlIndisType.SelectedValue = "Teguran" Then
                    tbCode.Text = GetAutoNmbr("ST", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
                Else
                    tbCode.Text = GetAutoNmbr("SP", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
                End If

                SQLString = "INSERT INTO PEEmpReprimandHd (TransNmbr, TransDate, Status, EffectiveDate, " + _
                "IndisciplinerType, IndisciplinerStatus, MasaBerlaku, EmpAppr1, JbtAppr1, Perjanjian, Remark, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", 'H', " + QuotedStr(Format(tbEffectiveDate.SelectedValue, "yyyy-MM-dd")) + ", " + _
                QuotedStr(ddlIndisType.SelectedValue) + ", " + QuotedStr(ddlIndisStatus.SelectedValue) + ", " + _
                tbMasaBerlaku.Text.Replace(",", "") + ", " + QuotedStr(tbEmpAppr1.Text) + ", " + QuotedStr(ddlJbtAppr1.SelectedValue) + ", " + QuotedStr(tbKerjaBersama.Text) + ", " + QuotedStr(tbRemark.Text) + _
                ", " + QuotedStr(ViewState("UserId").ToString) + ", GetDate() "

                ViewState("TransNmbr") = tbCode.Text

            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("SELECT Status FROM PEEmpReprimandHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE PEEmpReprimandHd SET EffectiveDate = " + QuotedStr(Format(tbEffectiveDate.SelectedValue, "yyyy-MM-dd")) + _
                ", IndisciplinerStatus =" + QuotedStr(ddlIndisStatus.Text) + ", MasaBerlaku = " + tbMasaBerlaku.Text.Replace(",", "") + ", EmpAppr1 = " + QuotedStr(tbEmpAppr1.Text) + _
                ", JbtAppr1 = " + QuotedStr(ddlJbtAppr1.SelectedValue) + ", Perjanjian = " + QuotedStr(tbKerjaBersama.Text) + _
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
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, EmpNumb, Department, JobTitle, IndisciplinerInfo, StatementEmp, StatementAppr, EmpAppr2, JbtAppr2, Remark FROM PEEmpReprimandDt WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
                da = New SqlDataAdapter(cmdSql)
                Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

                da.InsertCommand = dbcommandBuilder.GetInsertCommand
                da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
                da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt As New DataTable("PEEmpReprimandDt")

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
            FDateName = "Date, Effective Date"
            FDateValue = "TransDate, EffectiveDate"
            FilterName = "Indiscipliner No, Status, Indiscipliner Type, Indiscipliner Status, Masa Berlaku, Emp Approval 1, Emp Name Approval 1, Job Title Approval 1, Perjanjian Kerja Bersama, Remark"
            FilterValue = "TransNmbr, Status, Indisciplinerype, IndisciplinerStatus, MasaBerlaku, EmpAppr1, EmpNameAppr1, JbtNameAppr1,  Perjanjian, Remark"
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
                        ddlIndisType.Enabled = False
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
                        Session("SelectCommand") = "EXEC S_PEFormEmpReprimand '" + GVR.Cells(2).Text + "'"
                        If GVR.Cells(6).Text = "Teguran" Then
                            Session("ReportFile") = ".../../../Rpt/FormEmpReprimandTeguran.frx"
                        Else
                            Session("ReportFile") = ".../../../Rpt/FormEmpReprimandSP.frx"
                        End If
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

    Protected Sub tbEmpAppr1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbEmpAppr1.TextChanged
        Dim Dr As DataRow
        Dim Ds As DataSet
        Try
            Ds = SQLExecuteQuery("SELECT Emp_No, Emp_Name, Job_Title, Job_Title_Name FROM V_MsEmployee WHERE Fg_Active = 'Y' AND Emp_No = " + QuotedStr(tbEmpAppr1.Text) + " AND Emp_No <> " + QuotedStr(tbEmpAppr2.Text) + " AND Emp_No <> " + QuotedStr(tbEmpNo.Text), ViewState("DBConnection").ToString)
            If Ds.Tables(0).Rows.Count = 1 Then
                Dr = Ds.Tables(0).Rows(0)
                tbEmpAppr1.Text = TrimStr(Dr("Emp_No").ToString)
                tbEmpNameAppr1.Text = TrimStr(Dr("Emp_Name").ToString)
                ddlJbtAppr1.Text = TrimStr(Dr("Job_Title").ToString)
            Else
                tbEmpAppr1.Text = ""
                tbEmpNameAppr1.Text = ""
                ddlJbtAppr1.SelectedIndex = 0
            End If

        Catch ex As Exception
            Throw New Exception("tbEmpAppr1_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnEmpAppr1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEmpAppr1.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DbConnection")
            Session("filter") = "SELECT Emp_No, Emp_Name, Job_Title, Job_Title_Name FROM V_MsEmployee WHERE Fg_Active = 'Y' AND Emp_No <> " + QuotedStr(tbEmpAppr2.Text) + " AND Emp_No <> " + QuotedStr(tbEmpNo.Text)
            ResultField = "Emp_No, Emp_Name, Job_Title"
            ViewState("Sender") = "btnEmpAppr1"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnEmpAppr1_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnEmpAppr2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEmpAppr2.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DbConnection")
            Session("filter") = "SELECT Emp_No, Emp_Name, Job_Title, Job_Title_Name FROM V_MsEmployee WHERE Fg_Active = 'Y' AND Emp_No <> " + QuotedStr(tbEmpAppr1.Text) + " AND Emp_No <> " + QuotedStr(tbEmpNo.Text)
            ResultField = "Emp_No, Emp_Name, Job_Title"
            ViewState("Sender") = "btnEmpAppr2"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnEmpAppr1_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbEmpAppr2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbEmpAppr2.TextChanged
        Dim Dr As DataRow
        Dim Ds As DataSet
        Try
            Ds = SQLExecuteQuery("SELECT Emp_No, Emp_Name, Job_Title, Job_Title_Name FROM V_MsEmployee WHERE Fg_Active = 'Y' AND Emp_No = " + QuotedStr(tbEmpAppr2.Text) + " AND Emp_No <> " + QuotedStr(tbEmpAppr1.Text) + " AND Emp_No <> " + QuotedStr(tbEmpNo.Text), ViewState("DBConnection").ToString)
            If Ds.Tables(0).Rows.Count = 1 Then
                Dr = Ds.Tables(0).Rows(0)
                tbEmpAppr2.Text = TrimStr(Dr("Emp_No").ToString)
                tbEmpNameAppr2.Text = TrimStr(Dr("Emp_Name").ToString)
                ddlJbtAppr2.Text = TrimStr(Dr("Job_Title").ToString)
            Else
                tbEmpAppr2.Text = ""
                tbEmpNameAppr2.Text = ""
                ddlJbtAppr2.SelectedIndex = 0
            End If

        Catch ex As Exception
            Throw New Exception("tbEmpAppr1_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnEmp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEmp.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DbConnection")
            Session("filter") = "SELECT Emp_No, Emp_Name, Department, Department_Name, Job_Title, Job_Title_Name FROM V_MsEmployee WHERE Fg_Active = 'Y' AND Emp_No <> " + QuotedStr(tbEmpAppr1.Text) + " AND Emp_No <> " + QuotedStr(tbEmpAppr2.Text)
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
            Ds = SQLExecuteQuery("SELECT Emp_No, Emp_Name, Department, Department_Name, Job_Title, Job_Title_Name FROM V_MsEmployee WHERE Fg_Active = 'Y' AND Emp_No = " + QuotedStr(tbEmpNo.Text) + " AND Emp_No <> " + QuotedStr(tbEmpAppr1.Text) + " AND Emp_No <> " + QuotedStr(tbEmpAppr2.Text), ViewState("DBConnection").ToString)
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

        Catch ex As Exception
            Throw New Exception("tbEmpNo_TextChanged Error : " + ex.ToString)
        End Try
    End Sub
End Class
