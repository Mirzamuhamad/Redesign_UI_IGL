Imports System.Data
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO

Partial Class Transaction_TrPEEmpTerm_TrPEEmpTerm
    Inherits System.Web.UI.Page
    Protected con, con2 As New SqlConnection
    Protected da, da2 As New SqlDataAdapter
    Protected GetStringHd As String = "Select * From V_PEEmpTermHd"

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
            ' hasil dari search dialog
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnEmpAppr" Then
                    BindToText(tbEmpAppr, Session("Result")(0).ToString)
                    BindToText(tbEmpApprName, Session("Result")(1).ToString)
                    BindToDropList(ddlEmpApprJobTitle, Session("Result")(2).ToString)
                End If
                If ViewState("Sender") = "btnEmp" Then
                    BindToText(tbEmpCode, Session("Result")(0).ToString)
                    BindToText(tbEmpName, Session("Result")(1).ToString)
                    BindToDropList(ddlCurrJobTtl, Session("Result")(2).ToString)
                    BindToDropList(ddlCurrJobLevel, Session("Result")(4).ToString)
                    BindToDropList(ddlCurrDept, Session("Result")(6).ToString)
                    BindToDropList(ddlCurrEmpStatus, Session("Result")(8).ToString)
                    BindToDropList(ddlCurrWorkPlace, Session("Result")(10).ToString)
                    'If ddlTermType.SelectedValue <> "Pengangkatan" Then
                    BindToDropList(ddlNewJobTtl, Session("Result")(2).ToString)
                    BindToDropList(ddlNewDept, Session("Result")(6).ToString)
                    'End If
                    'If ddlTermType.SelectedValue <> "Mutasi" Then
                    BindToDropList(ddlNewEmpStatus, Session("Result")(8).ToString)
                    'End If
                    'If (ddlTermType.SelectedValue <> "Mutasi") And (ddlTermType.SelectedValue <> "Pengangkatan") Then
                    BindToDropList(ddlNewJobLevel, Session("Result")(4).ToString)
                    BindToDropList(ddlNewWorkPlace, Session("Result")(10).ToString)

                    'If Session("Result")(13).ToString.Trim = "" Then
                    tbCurrEndDate.Clear()
                    'If CDate(Session("Result")(12).ToString).Year <= 1900 Then
                    'tbCurrEndDate.SelectedDate = Nothing
                    'Else
                    BindToDate(tbCurrEndDate, Session("Result")(12).ToString)

                    'End If
                    
                    CekFgPermanent()
                End If
                Session("Result") = Nothing
                ViewState("Sender") = Nothing
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
        ViewState("SortExpression") = Nothing
        ViewState("DigitCurr") = ViewState("DigitHome")
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
        If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then

        End If
        FillCombo(ddlEmpApprJobTitle, "EXEC S_GetJobTitle", True, "JobTtlCode", "JobTtlName", ViewState("DBConnection"))
        FillCombo(ddlCurrJobTtl, "EXEC S_GetJobTitle", True, "JobTtlCode", "JobTtlName", ViewState("DBConnection"))
        FillCombo(ddlCurrJobLevel, "EXEC S_GetJobLevel", True, "JobLvlCode", "JobLvlName", ViewState("DBConnection"))
        FillCombo(ddlCurrDept, "SELECT Dept_Code, Dept_Name FROM VMsDepartment", True, "Dept_Code", "Dept_Name", ViewState("DBConnection"))
        FillCombo(ddlCurrEmpStatus, "EXEC S_GetEmpStatus", True, "EmpStatusCode", "EmpStatusName", ViewState("DBConnection"))
        FillCombo(ddlCurrWorkPlace, "EXEC S_GetMsWorkPlace", True, "WorkPlaceCode", "WorkPlaceName", ViewState("DBConnection"))
        FillCombo(ddlNewJobTtl, "EXEC S_GetJobTitle", True, "JobTtlCode", "JobTtlName", ViewState("DBConnection"))
        FillCombo(ddlNewJobLevel, "EXEC S_GetJobLevel", True, "JobLvlCode", "JobLvlName", ViewState("DBConnection"))
        FillCombo(ddlNewDept, "SELECT Dept_Code, Dept_Name FROM VMsDepartment", True, "Dept_Code", "Dept_Name", ViewState("DBConnection"))
        FillCombo(ddlNewEmpStatus, "EXEC S_GetEmpStatus", True, "EmpStatusCode", "EmpStatusName", ViewState("DBConnection"))
        FillCombo(ddlNewWorkPlace, "EXEC S_GetMsWorkPlace", True, "WorkPlaceCode", "WorkPlaceName", ViewState("DBConnection"))

    End Sub

    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)

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
        Return "SELECT * From V_PEEmpTermDt WHERE TransNmbr = " + QuotedStr(Nmbr) + " "
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
                    Result = ExecSPCommandGo(ActionValue, "S_PEEmpTerm", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            tbEffectiveDate.Enabled = State
            ddlTermType.Enabled = State
            tbEmpAppr.Enabled = State
            btnEmpAppr.Visible = State
        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub EnableDt(ByVal EmpType As String)
        Try
            If EmpType = "Mutasi" Then
                ddlNewJobTtl.Enabled = True
                ddlNewJobLevel.Enabled = False
                ddlNewDept.Enabled = True
                ddlNewEmpStatus.Enabled = False
                ddlNewWorkPlace.Enabled = False
            ElseIf EmpType = "Pengangkatan" Then
                ddlNewJobTtl.Enabled = False
                ddlNewJobLevel.Enabled = False
                ddlNewDept.Enabled = False
                ddlNewEmpStatus.Enabled = True
                ddlNewWorkPlace.Enabled = False
            Else
                ddlNewJobTtl.Enabled = True
                ddlNewJobLevel.Enabled = True
                ddlNewDept.Enabled = True
                ddlNewEmpStatus.Enabled = True
                ddlNewWorkPlace.Enabled = True
            End If
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
                tbRef.Text = GetAutoNmbr("SK", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
                SQLString = "INSERT INTO PETermHd (TransNmbr, Status, TransDate, EffectiveDate, TermType, EmpAppr, JobTtlAppr, Remark, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbRef.Text) + ", 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', '" + tbEffectiveDate.SelectedValue + "', " + _
                QuotedStr(ddlTermType.SelectedValue) + "," + QuotedStr(tbEmpAppr.Text) + "," + QuotedStr(ddlEmpApprJobTitle.SelectedValue) + "," + _
                QuotedStr(tbRemark.Text) + "," + QuotedStr(ViewState("UserId").ToString) + ", getDate()"
                ViewState("Reference") = tbRef.Text
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM PETermHd WHERE TransNmbr = " + QuotedStr(tbRef.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE PETermHd SET EffectiveDate = " + QuotedStr(tbEffectiveDate.SelectedValue) + _
                ", TermType = " + QuotedStr(ddlTermType.SelectedValue) + ", EmpAppr = " + QuotedStr(tbEmpAppr.Text) + _
                ", JobTtlAppr = " + QuotedStr(ddlEmpApprJobTitle.SelectedValue) + _
                ", Remark = " + QuotedStr(tbRemark.Text) + "," + _
                " TransDate = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', DateAppr = getDate()" + _
                " WHERE TransNmbr = '" + tbRef.Text + "'"
            End If
            SQLString = SQLString.Replace("'1900-01-01'", "NULL")
            SQLString = SQLString.Replace("'0001-01-01'", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            'update Primary Key on Dt
            Dim Row As DataRow()

            Row = ViewState("Dt").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbRef.Text
                Row(I).EndEdit()
            Next

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, EmpNumb, SKCompanyNo, CurrJobTtl, CurrJobLvl, CurrDept, CurrEmpType, CurrWorkPlace, CurrContractEndDate, NewJobTtl, NewJobLvl, NewDept, NewEmpType, NewWorkPlace, NewContractEndDate, Appraisal, ChangeReason, Remark, EndEffectiveDate FROM PETermDt WHERE TransNmbr = '" & ViewState("Reference") & "'", con)

            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt As New DataTable("PETermDt")

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
            For Each dr In ViewState("Dt").Rows
                If CekDt(dr, "EmpNumb") = False Then
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
            lbStatus.Text = "Save All Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, pnlDt, GridDt)
            newTrans()
            btnHome.Visible = False
            tbDate.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ViewState("DigitCurr") = 0
            ClearHd()
            Cleardt()
            tbDate.SelectedDate = ViewState("ServerDate") 'Today
            pnlDt.Visible = True
            BindDataDt("")
            EnableHd(True)
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbRef.Text = ""
            tbDate.SelectedDate = Now.Date
            tbEffectiveDate.SelectedValue = Now.Date
            ddlTermType.SelectedIndex = 0
            tbEmpAppr.Text = ""
            tbEmpApprName.Text = ""
            ddlEmpApprJobTitle.SelectedValue = ""
            ddlEmpApprJobTitle.Enabled = False
            tbRemark.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbEmpCode.Text = ""
            tbEmpName.Text = ""
            tbSKNo.Text = ""
            ddlCurrJobTtl.SelectedValue = ""
            ddlCurrJobLevel.SelectedValue = ""
            ddlCurrDept.SelectedValue = ""
            ddlCurrEmpStatus.SelectedValue = ""
            ddlCurrWorkPlace.selectedvalue = ""
            ddlNewJobTtl.SelectedValue = ""
            ddlNewJobLevel.SelectedValue = ""
            ddlNewDept.SelectedValue = ""
            ddlNewEmpStatus.SelectedValue = ""
            ddlNewWorkPlace.SelectedValue = ""
            tbCurrEndDate.Clear()
            tbNewEndDate.Clear()
            tbAppraisal.Text = ""
            tbChangeReason.Text = ""
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
            If IsNothing(ViewState("Dt")) Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
                Exit Sub
            End If
            For Each dr In ViewState("Dt").Rows
                If CekDt(dr, "EmpNumb") = False Then
                    Exit Sub
                End If
            Next
            SaveAll()
            newTrans()
        Catch ex As Exception
            lbStatus.Text = "btn save all error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdddt.Click, btnAddDt2.Click
        If CekHd() = False Then
            Exit Sub
        End If
        Cleardt()
        ViewState("StateDt") = "Insert"
        ViewState("FgPermanent") = Nothing
        EnableDt(ddlTermType.SelectedValue)
        MovePanel(pnlDt, pnlEditDt)
        EnableHd(False)
        StatusButtonSave(False)
    End Sub

    Function CekHd() As Boolean
        Try
            If tbEffectiveDate.IsNull Then
                lbStatus.Text = MessageDlg("Effective Date must have value. ")
                tbEffectiveDate.Focus()
                Return False
            End If
            If tbEmpAppr.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Approved By must have value. ")
                tbEmpAppr.Focus()
                Return False
            End If

            Return True
        Catch ex As Exception
            Throw New Exception("Cek Hd Error : " + ex.ToString)
        End Try
    End Function

    Function CekDt(Optional ByVal Dr As DataRow = Nothing, Optional ByVal FieldKey As String = "") As Boolean
        Try
            If Not (Dr Is Nothing) Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("Emp_Name").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Employee Must Have Value")
                    Return False
                End If
                If Dr("SKCompanyNo").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("SK Company No must have value")
                    Return False
                End If
                If ddlTermType.SelectedValue <> "Pengangkatan" Then
                    If Dr("NewJobTtl").ToString.Trim = "" Then
                        lbStatus.Text = MessageDlg("New Job Title Must Have Value")
                        Return False
                    End If
                    If Dr("NewDept").ToString.Trim = "" Then
                        lbStatus.Text = MessageDlg("New Department must have value")
                        Return False
                    End If
                End If
                If (ddlTermType.SelectedValue <> "Mutasi") And (ddlTermType.SelectedValue <> "Pengangkatan") Then
                    If Dr("NewJobLvl").ToString.Trim = "" Then
                        lbStatus.Text = MessageDlg("New Job Level Must Have Value")
                        Return False
                    End If
                    If Dr("NewWorkPlace").ToString.Trim = "" Then
                        lbStatus.Text = MessageDlg("New Work Place Must Have Value")
                        Return False
                    End If
                End If
                If (ddlTermType.SelectedValue <> "Mutasi") Then
                    If Dr("NewEmpType").ToString.Trim = "" Then
                        lbStatus.Text = MessageDlg("New Emp Status Must Have Value")
                        Return False
                    End If
                End If

            Else
                If tbEmpName.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Employee must have value")
                    tbEmpCode.Focus()
                    Return False
                End If
                If tbSKNo.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("SK Company No must have value")
                    tbSKNo.Focus()
                    Return False
                End If
                If ddlTermType.SelectedValue <> "Pengangkatan" Then
                    If ddlNewJobTtl.SelectedValue.Trim = "" Then
                        lbStatus.Text = MessageDlg("New Job Title must have value")
                        ddlNewJobTtl.Focus()
                        Return False
                    End If
                    If ddlNewDept.SelectedValue.Trim = "" Then
                        lbStatus.Text = MessageDlg("New Department must have value")
                        ddlNewDept.Focus()
                        Return False
                    End If
                End If
                If (ddlTermType.SelectedValue <> "Mutasi") And (ddlTermType.SelectedValue <> "Pengangkatan") Then
                    If ddlNewJobLevel.SelectedValue.Trim = "" Then
                        lbStatus.Text = MessageDlg("New Job Level must have value")
                        ddlNewJobLevel.Focus()
                        Return False
                    End If
                    If ddlNewWorkPlace.SelectedValue.Trim = "" Then
                        lbStatus.Text = MessageDlg("New Work Place must have value")
                        ddlNewWorkPlace.Focus()
                        Return False
                    End If
                End If
                If ddlTermType.SelectedValue <> "Mutasi" Then
                    If ddlNewEmpStatus.SelectedValue.Trim = "" Then
                        lbStatus.Text = MessageDlg("New Emp Status must have value")
                        ddlNewEmpStatus.Focus()
                        Return False
                    End If
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
            FDateName = "Date, Effective Date"
            FDateValue = "TransDate, EffectiveDate"
            FilterName = "TransNmbr, Status, SK Type, Emp Appr, JobTitleAppr, Remark"
            FilterValue = "TransNmbr, Status, TermType, EmpApprName, EmpApprJobTtl, Remark"
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
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, pnlDt, GridDt)
                    btnHome.Visible = True
                ElseIf DDL.SelectedValue = "Edit" Then
                    If GVR.Cells(3).Text = "H" Or GVR.Cells(3).Text = "G" Then
                        CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        MovePanel(PnlHd, pnlInput)
                        ViewState("Reference") = GVR.Cells(2).Text
                        GridDt.PageIndex = 0
                        BindDataDt(ViewState("Reference"))
                        FillTextBoxHd(ViewState("Reference"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        btnHome.Visible = False
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    Else
                        lbStatus.Text = MessageDlg("Data must Hold or Get Approval to edit")
                        Exit Sub
                    End If
                ElseIf DDL.SelectedValue = "Print" Then
                    CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                    If CekMenu <> "" Then
                        lbStatus.Text = CekMenu
                        Exit Sub
                    End If
                    Session("SelectCommand") = "EXEC S_PEFormEmpTerm " + QuotedStr(GVR.Cells(2).Text) + "," + QuotedStr(ViewState("UserId"))
                    If (GVR.Cells(6).Text = "New Employee") Or (GVR.Cells(6).Text = "Pengangkatan") Then
                        Session("ReportFile") = ".../../../Rpt/FormPEEmpTermNew.frx"
                    Else
                        Session("ReportFile") = ".../../../Rpt/FormPEEmpTermChange.frx"
                    End If
                    Session("DBConnection") = ViewState("DBConnection")
                    AttachScript("openprintdlg();", Page, Me.GetType)
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
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Dim dr() As DataRow
        Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
        dr = ViewState("Dt").Select("EmpNumb = " + QuotedStr(GVR.Cells(1).Text))
        dr(0).Delete()
        BindGridDt(ViewState("Dt"), GridDt)
        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            FillTextBoxDt(GVR.Cells(1).Text)
            ViewState("DtValue") = GVR.Cells(1).Text
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
            BindToDropList(ddlTermType, Dt.Rows(0)("TermType").ToString)
            BindToText(tbEmpAppr, Dt.Rows(0)("EmpAppr").ToString)
            BindToText(tbEmpApprName, Dt.Rows(0)("EmpApprName").ToString)
            BindToDropList(ddlEmpApprJobTitle, Dt.Rows(0)("JobTtlAppr").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("EmpNumb = " + QuotedStr(ItemNo))
            If Dr.Length > 0 Then
                BindToText(tbEmpCode, Dr(0)("EmpNumb").ToString)
                BindToText(tbEmpCode, Dr(0)("Emp_Name").ToString)
                BindToText(tbSKNo, Dr(0)("SKCompanyNo").ToString)
                BindToDropList(ddlCurrJobTtl, Dr(0)("CurrJobTtl").ToString)
                BindToDropList(ddlCurrJobLevel, Dr(0)("CurrJobLvl").ToString)
                BindToDropList(ddlCurrDept, Dr(0)("CurrDept").ToString)
                BindToDropList(ddlCurrWorkPlace, Dr(0)("CurrWorkPlace").ToString)
                BindToDate(tbCurrEndDate, Dr(0)("CurrContractEndDate").ToString)
                BindToDropList(ddlNewJobTtl, Dr(0)("NewJobTtl").ToString)
                BindToDropList(ddlNewJobLevel, Dr(0)("NewJobLvl").ToString)
                BindToDropList(ddlNewDept, Dr(0)("NewDept").ToString)
                BindToDropList(ddlNewEmpStatus, Dr(0)("NewEmpType").ToString)
                BindToDropList(ddlNewWorkPlace, Dr(0)("NewWorkPlace").ToString)
                BindToDate(tbNewEndDate, Dr(0)("NewContractEndDate").ToString)
                BindToText(tbAppraisal, Dr(0)("Appraisal").ToString)
                BindToText(tbChangeReason, Dr(0)("ChangeReason").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
            End If

        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub

    Private Sub StatusButtonSave(ByVal Bool As Boolean)
        btnSaveAll.Visible = Bool
        btnSaveTrans.Visible = Bool
        btnBack.Visible = Bool
    End Sub


    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt.Click
        Dim Row As DataRow
        Try
            If ViewState("StateDt") = "Edit" Then
                If ViewState("DtValue") <> tbEmpCode.Text Then
                    If CekExistData(ViewState("Dt"), "EmpNumb", tbEmpCode.Text) Then
                        lbStatus.Text = "Employee " + QuotedStr(tbEmpName.Text) + " has already exists"
                        Exit Sub
                    End If
                End If
                Row = ViewState("Dt").Select("EmpNumb = " + QuotedStr(ViewState("DtValue")))(0)
                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                Row("EmpNumb") = tbEmpCode.Text
                Row("Emp_Name") = tbEmpName.Text
                Row("SKCompanyNo") = tbSKNo.Text
                Row("CurrJobTtl") = ddlCurrJobTtl.SelectedValue
                Row("CurrJobLvl") = ddlCurrJobLevel.SelectedValue
                Row("CurrDept") = ddlCurrDept.SelectedValue
                Row("CurrEmpType") = ddlCurrEmpStatus.SelectedValue
                Row("CurrWorkPlace") = ddlCurrWorkPlace.selectedvalue
                If ddlCurrJobTtl.SelectedValue <> "" Then
                    Row("CurrJobTtlName") = ddlCurrJobTtl.SelectedItem.Text
                Else
                    Row("CurrJobTtlName") = ""
                End If
                If ddlCurrJobLevel.SelectedValue <> "" Then
                    Row("CurrJobLvlName") = ddlCurrJobLevel.SelectedItem.Text
                Else
                    Row("CurrJobLvlName") = ""
                End If
                If ddlCurrDept.SelectedValue <> "" Then
                    Row("CurrDeptName") = ddlCurrDept.SelectedItem.Text
                Else
                    Row("CurrDeptName") = ""
                End If
                If ddlCurrEmpStatus.SelectedValue <> "" Then
                    Row("CurrEmpStatusName") = ddlCurrEmpStatus.SelectedItem.Text
                Else
                    Row("CurrEmpStatusName") = ""
                End If
                If ddlCurrWorkPlace.SelectedValue <> "" Then
                    Row("CurrWorkPlaceName") = ddlCurrWorkPlace.SelectedItem.Text
                Else
                    Row("CurrWorkPlaceName") = ""
                End If

                Row("CurrContractEndDate") = tbCurrEndDate.SelectedDate
                If (Row("CurrContractEndDate") = "1/1/0001") Or (Row("CurrContractEndDate") = "1/1/1900") Then
                    Row("CurrContractEndDate") = DBNull.Value
                End If
                Row("NewJobTtl") = ddlNewJobTtl.SelectedValue
                Row("NewJobLvl") = ddlNewJobLevel.SelectedValue
                Row("NewDept") = ddlNewDept.SelectedValue
                Row("NewEmpType") = ddlNewEmpStatus.SelectedValue
                Row("NewWorkPlace") = ddlNewWorkPlace.SelectedValue
                If ddlNewJobTtl.SelectedValue <> "" Then
                    Row("NewJobTtlName") = ddlNewJobTtl.SelectedItem.Text
                Else
                    Row("NewJobTtlName") = ""
                End If
                If ddlNewJobLevel.SelectedValue <> "" Then
                    Row("NewJobLvlName") = ddlNewJobLevel.SelectedItem.Text
                Else
                    Row("NewJobLvlName") = ""
                End If
                If ddlNewDept.SelectedValue <> "" Then
                    Row("NewDeptName") = ddlNewDept.SelectedItem.Text
                Else
                    Row("NewDeptName") = ""
                End If
                If ddlNewEmpStatus.SelectedValue <> "" Then
                    Row("NewEmpStatusName") = ddlNewEmpStatus.SelectedItem.Text
                Else
                    Row("NewEmpStatusName") = ""
                End If
                If ddlNewWorkPlace.SelectedValue <> "" Then
                    Row("NewWorkPlaceName") = ddlNewWorkPlace.SelectedItem.Text
                Else
                    Row("NewWorkPlaceName") = ""
                End If
                Row("NewContractEndDate") = tbNewEndDate.SelectedDate
                If (Row("NewContractEndDate") = "1/1/0001") Or (Row("NewContractEndDate") = "1/1/1900") Then
                    Row("NewContractEndDate") = DBNull.Value
                End If
                Row("Appraisal") = tbAppraisal.Text
                Row("ChangeReason") = tbChangeReason.Text
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If
                If CekExistData(ViewState("Dt"), "EmpNumb", tbEmpCode.Text) Then
                    lbStatus.Text = "Employee " + QuotedStr(tbEmpName.Text) + " has already exists"
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("EmpNumb") = tbEmpCode.Text
                dr("Emp_Name") = tbEmpName.Text
                dr("SKCompanyNo") = tbSKNo.Text
                dr("CurrJobTtl") = ddlCurrJobTtl.SelectedValue
                dr("CurrJobLvl") = ddlCurrJobLevel.SelectedValue
                dr("CurrDept") = ddlCurrDept.SelectedValue
                dr("CurrEmpType") = ddlCurrEmpStatus.SelectedValue
                dr("CurrWorkPlace") = ddlCurrWorkPlace.SelectedValue
                If ddlCurrJobTtl.SelectedValue <> "" Then
                    dr("CurrJobTtlName") = ddlCurrJobTtl.SelectedItem.Text
                Else
                    dr("CurrJobTtlName") = ""
                End If
                If ddlCurrJobLevel.SelectedValue <> "" Then
                    dr("CurrJobLvlName") = ddlCurrJobLevel.SelectedItem.Text
                Else
                    dr("CurrJobLvlName") = ""
                End If
                If ddlCurrDept.SelectedValue <> "" Then
                    dr("CurrDeptName") = ddlCurrDept.SelectedItem.Text
                Else
                    dr("CurrDeptName") = ""
                End If
                If ddlCurrEmpStatus.SelectedValue <> "" Then
                    dr("CurrEmpStatusName") = ddlCurrEmpStatus.SelectedItem.Text
                Else
                    dr("CurrEmpStatusName") = ""
                End If
                If ddlCurrWorkPlace.SelectedValue <> "" Then
                    dr("CurrWorkPlaceName") = ddlCurrWorkPlace.SelectedItem.Text
                Else
                    dr("CurrWorkPlaceName") = ""
                End If
                dr("CurrContractEndDate") = tbCurrEndDate.SelectedDate
                If (dr("CurrContractEndDate") = "1/1/0001") Or (dr("CurrContractEndDate") = "1/1/1900") Then
                    dr("CurrContractEndDate") = DBNull.Value
                End If
                dr("NewJobTtl") = ddlNewJobTtl.SelectedValue
                dr("NewJobLvl") = ddlNewJobLevel.SelectedValue
                dr("NewDept") = ddlNewDept.SelectedValue
                dr("NewEmpType") = ddlNewEmpStatus.SelectedValue
                dr("NewWorkPlace") = ddlNewWorkPlace.SelectedValue
                If ddlNewJobTtl.SelectedValue <> "" Then
                    dr("NewJobTtlName") = ddlNewJobTtl.SelectedItem.Text
                Else
                    dr("NewJobTtlName") = ""
                End If
                If ddlNewJobLevel.SelectedValue <> "" Then
                    dr("NewJobLvlName") = ddlNewJobLevel.SelectedItem.Text
                Else
                    dr("NewJobLvlName") = ""
                End If
                If ddlNewDept.SelectedValue <> "" Then
                    dr("NewDeptName") = ddlNewDept.SelectedItem.Text
                Else
                    dr("NewDeptName") = ""
                End If
                If ddlNewEmpStatus.SelectedValue <> "" Then
                    dr("NewEmpStatusName") = ddlNewEmpStatus.SelectedItem.Text
                Else
                    dr("NewEmpStatusName") = ""
                End If
                If ddlNewWorkPlace.SelectedValue <> "" Then
                    dr("NewWorkPlaceName") = ddlNewWorkPlace.SelectedItem.Text
                Else
                    dr("NewWorkPlaceName") = ""
                End If
                dr("NewContractEndDate") = tbNewEndDate.SelectedDate
                If (dr("NewContractEndDate") = "1/1/0001") Or (dr("NewContractEndDate") = "1/1/1900") Then
                    dr("NewContractEndDate") = DBNull.Value
                End If
                dr("Appraisal") = tbAppraisal.Text
                dr("ChangeReason") = tbChangeReason.Text
                dr("Remark") = tbRemarkDt.Text
                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, pnlDt)
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

    Protected Sub btnCancelDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelDt.Click
        Try
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnEmpAppr_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEmpAppr.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT * FROM V_MsEmployee WHERE COALESCE(Fg_Active, 'N') = 'Y' "
            Session("DBConnection") = ViewState("DBConnection")
            ResultField = "Emp_No, Emp_Name, Job_Title"
            ViewState("Sender") = "btnEmpAppr"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn EmpAppr Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbEmpAppr_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbEmpAppr.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Employee ", tbEmpAppr.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbEmpAppr.Text = Dr("Emp_No")
                tbEmpApprName.Text = Dr("Emp_Name")
                ddlEmpApprJobTitle.Text = Dr("Job_Title_Code")
            Else
                tbEmpAppr.Text = ""
                tbEmpApprName.Text = ""
                ddlEmpApprJobTitle.Text = ""
            End If
            tbEmpAppr.Focus()
        Catch ex As Exception
            Throw New Exception("tb EmpAppr change Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbEmpCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbEmpCode.TextChanged
        Dim Dr As DataRow
        Dim DT As DataTable
        Dim StrTermType As String
        Try
            If ddlTermType.SelectedValue = "Pengangkatan" Then
                StrTermType = " AND COALESCE(FgPermanent, 'N') = 'N' "
            Else
                StrTermType = ""
            End If
            DT = SQLExecuteQuery("EXEC S_FindEmpForPETerm " + QuotedStr(tbEmpCode.Text) + ", " + QuotedStr(tbEmpAppr.Text) + "," + QuotedStr(StrTermType), ViewState("DBConnection").ToString).Tables(0)
            If DT.Rows.Count > 0 Then
                Dr = DT.Rows(0)
            Else
                Dr = Nothing
            End If
            If Not Dr Is Nothing Then
                tbEmpCode.Text = Dr("Emp_No")
                tbEmpName.Text = Dr("Emp_Name")
                'tbSKNo.Text = Dr("SKNo")
                ddlCurrJobTtl.SelectedValue = Dr("Job_Title")
                ddlCurrJobLevel.SelectedValue = Dr("Job_Level")
                ddlCurrDept.SelectedValue = Dr("Department")
                ddlCurrEmpStatus.SelectedValue = Dr("Emp_Status")
                'If CDate(Dr("End_Date_Contract").ToString).Year <= 1900 Then
                'tbCurrEndDate.SelectedDate = Nothing
                'Else
                tbCurrEndDate.SelectedDate = Dr("End_Date_Contract")
                'End If
                ddlCurrWorkPlace.SelectedValue = Dr("Work_Place")
                ddlNewJobTtl.SelectedValue = Dr("Job_Title")
                ddlNewJobLevel.SelectedValue = Dr("Job_Level")
                ddlNewDept.SelectedValue = Dr("Department")
                ddlNewEmpStatus.SelectedValue = Dr("Emp_Status")
                ddlNewWorkPlace.SelectedValue = Dr("Work_Place")
                'tbNewEndDate.SelectedDate = Dr("End_Date_Contract")
            Else
                tbEmpCode.Text = ""
                tbEmpName.Text = ""
                tbSKNo.Text = ""
                ddlCurrJobTtl.SelectedValue = ""
                ddlCurrJobLevel.SelectedValue = ""
                ddlCurrDept.SelectedValue = ""
                ddlCurrEmpStatus.SelectedValue = ""
                tbCurrEndDate.Clear()
                ddlCurrWorkPlace.SelectedValue = ""
                ddlNewJobTtl.SelectedValue = ""
                ddlNewJobLevel.SelectedValue = ""
                ddlNewDept.SelectedValue = ""
                ddlNewEmpStatus.SelectedValue = ""
                ddlNewWorkPlace.SelectedValue = ""
                tbNewEndDate.Clear()
            End If
            CekFgPermanent()
            tbEmpCode.Focus()
        Catch ex As Exception
            Throw New Exception("tb EmpCode change Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlNewEmpStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlNewEmpStatus.SelectedIndexChanged
        Try
            CekFgPermanent()
        Catch ex As Exception
            Throw New Exception("ddlNewEmpStatus_SelectedIndexChanged Error : " + ex.ToString)
        End Try
    End Sub
    Private Sub CekFgPermanent()
        Dim Dr As DataRow
        Try
            Dr = FindMaster("EmpStatus ", ddlNewEmpStatus.SelectedValue, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                ViewState("FgPermanent") = Dr("Fg_Permanent")
            Else
                ViewState("FgPermanent") = Nothing
            End If
            tbNewEndDate.Clear()
            If ViewState("FgPermanent") = "N" Then
                tbNewEndDate.Enabled = True
            Else
                tbNewEndDate.Enabled = False
            End If
        Catch ex As Exception
            Throw New Exception("CekFgPermanent Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnEmp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEmp.Click
        Dim ResultField, StrTermType As String
        Try
            If ddlTermType.SelectedValue = "Pengangkatan" Then
                StrTermType = " AND COALESCE(FgPermanent, 'N') = 'N' "
            Else
                StrTermType = ""
            End If
            Session("filter") = "EXEC S_FindEmpForPETerm '', " + QuotedStr(tbEmpAppr.Text) + "," + QuotedStr(StrTermType)
            Session("DBConnection") = ViewState("DBConnection")
            ResultField = "Emp_No, Emp_Name, Job_Title, Job_Title_Name, Job_Level, Job_Level_Name, Department, Department_Name, Emp_Status, Emp_Status_Name, Work_Place, Work_Place_Name, End_Date_Contract"
            ViewState("Sender") = "btnEmp"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Emp Click Error : " + ex.ToString
        End Try
    End Sub
End Class
