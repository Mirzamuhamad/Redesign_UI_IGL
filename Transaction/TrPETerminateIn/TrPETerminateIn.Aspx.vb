Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Odbc
'Imports CrystalDecisions.CrystalReports.Engine
'Imports CrystalDecisions.Shared
'Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc


Partial Class TrPETerminateIn
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    'Protected GetStringHd As String = "Select * From V_PRCRequestHd WHERE UserId = " + QuotedStr(ViewState("UserId").ToString)

    Private Function GetStringHd() As String
        Return "Select * From V_PETerminateInHd"
    End Function

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
                If Not Session("PostNmbr") = Nothing Then
                    tbFilter.Text = Session("PostNmbr")
                    btnSearch_Click(Nothing, Nothing)
                    Session("PostNmbr") = Nothing
                    tbFilter.Text = ""
                End If
            End If
            lbStatus.Text = ""
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If

            ' hasil dari search dialog
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnEmployee" Then
                    tbEmpCode.Text = Session("Result")(0).ToString
                    tbEmpName.Text = Session("Result")(1).ToString
                    tbJobTitledt.Text = Session("Result")(2).ToString
                    tbJobTitleNameDt.Text = Session("Result")(3).ToString
                    tbJobLevel.Text = Session("Result")(4).ToString
                    tbJobLevelName.Text = Session("Result")(5).ToString
                    tbEmpStatus.Text = Session("Result")(6).ToString
                    tbEmpStatusName.Text = Session("Result")(7).ToString
                    tbDept.Text = Session("Result")(8).ToString
                    tbDeptName.Text = Session("Result")(9).ToString
                    tbWorkPlace.Text = Session("Result")(10).ToString
                    tbWorkPlaceName.Text = Session("Result")(11).ToString
                    tbContractEndDate.Text = Session("Result")(12).ToString
                End If
                If ViewState("Sender") = "btnApproved" Then
                    tbApproved.Text = Session("Result")(0).ToString
                    tbApprovedName.Text = Session("Result")(1).ToString
                    tbJobTitle.Text = Session("Result")(2).ToString
                    tbJobTitleName.Text = Session("Result")(3).ToString
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
        FillRange(ddlRange)
        'FillCombo(ddlReason, "EXEC S_GetPHKReason", True, "PHKCode", "PHKName", ViewState("DBConnection"))
        'FillCombo(ddlEmpReason, "EXEC S_GetPHKReason", True, "PHKCode", "PHKName", ViewState("DBConnection"))
        'FillCombo(ddlEmpStatus, "EXEC S_GetEmpStatus", True, "EmpStatusCode", "EmpStatusName", ViewState("DBConnection"))

        ViewState("SortExpression") = Nothing
        ViewState("DigitCurr") = 0
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))

        'If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
        '    ddlCommand.Items.Add("Print")
        '    ddlCommand2.Items.Add("Print")
        'End If


    End Sub

    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue, ddlRange.SelectedValue)
            If AdvanceFilter.Length > 1 Then
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
        Return "SELECT * From V_PETerminateIndt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        Try
            GridView1.PageIndex = 0
            Session("AdvanceFilter") = ""
            BindData(Session("AdvanceFilter"))
            pnlNav.Visible = True
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
                Session("SelectCommand") = "EXEC S_PETErminateOut " + Result + "," + QuotedStr(ViewState("UserId"))
                Session("ReportFile") = ".../../../Rpt/FormMedicalIn.frx"
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
                    ElseIf ActionValue = "Post" Then

                        Result = ExecSPCommandGo(ActionValue, "S_PEEmpRenewal", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                        If Trim(Result) <> "" Then
                            lbStatus.Text = lbStatus.Text + Result + " <br/>"
                        End If

                    Else
                        Result = ExecSPCommandGo(ActionValue, "S_PEEmpRenewal", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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

    Private Sub EnableHd(ByVal State As Boolean)
        Try
            'ddlDept.Enabled = False
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


    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBack.Click, btnHome.Click
        Try
            MovePanel(pnlInput, PnlHd)
        Catch ex As Exception
            lbStatus.Text = "btn back Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancelDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelDt.Click
        Try
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveDt.Click
        Dim Row As DataRow
        'Dim Joblvl As String
        Try
            If ViewState("StateDt") = "Edit" Then
                If ViewState("DtValue") <> tbEmpCode.Text Then
                    If CekExistData(ViewState("Dt"), "EmpNumb", tbEmpCode.Text) Then
                        lbStatus.Text = "Employee '" + tbEmpName.Text + " has already exists"
                        Exit Sub
                    End If
                End If
                Row = ViewState("Dt").Select("EmpNumb = " + QuotedStr(ViewState("DtValue")))(0)
                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                Row("EmpNumb") = tbEmpCode.Text
                Row("NewEmpNumb") = tbNewEmpNo.Text
                Row("Emp_Name") = tbEmpName.Text
                Row("JobTitle") = tbJobTitledt.Text
                Row("JobTitle_Name") = tbJobTitleNameDt.Text
                Row("JobLevel") = tbJobLevel.Text
                Row("JobLevel_Name") = tbJobLevelName.Text
                Row("EmpStatus") = tbEmpStatus.Text
                Row("EmpStatus_Name") = tbEmpStatusName.Text
                Row("Department") = tbDept.Text
                Row("Dept_Name") = tbDeptName.Text
                Row("WorkPlace") = tbWorkPlace.Text
                Row("WorkPlace_Name") = tbWorkPlaceName.Text
                Row("End_Date_Contract") = tbContractEndDate.Text
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()

            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If
                If CekExistData(ViewState("Dt"), "EmpNumb", tbEmpCode.Text) Then
                    lbStatus.Text = "Employee '" + tbEmpName.Text + " has already exists"
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("EmpNumb") = tbEmpCode.Text
                dr("NewEmpNumb") = tbNewEmpNo.Text
                dr("Emp_Name") = tbEmpName.Text
                dr("JobTitle") = tbJobTitledt.Text
                dr("JobTitle_Name") = tbJobTitleNameDt.Text
                dr("JobLevel") = tbJobLevel.Text
                dr("JobLevel_Name") = tbJobLevelName.Text
                dr("EmpStatus") = tbEmpStatus.Text
                dr("EmpStatus_Name") = tbEmpStatusName.Text
                dr("Department") = tbDept.Text
                dr("Dept_Name") = tbDeptName.Text
                dr("Emp_Name") = tbEmpName.Text
                dr("JobTitle") = tbJobTitledt.Text
                dr("JobTitle_Name") = tbJobTitleNameDt.Text
                dr("WorkPlace") = tbWorkPlace.Text
                dr("WorkPlace_Name") = tbWorkPlaceName.Text
                dr("End_Date_Contract") = tbContractEndDate.Text
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
                tbTransNo.Text = GetAutoNmbr("EI", "N", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
                SQLString = "INSERT INTO PEEmpRenewalHd(TransNmbr, STATUS, Transdate, EffectiveDate, EmpAppr,JobTitleAppr, Remark, UserPrep, DatePrep) " + _
                "SELECT '" + tbTransNo.Text + "', 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', '" + Format(tbEffectiveDate.SelectedValue, "yyyy-MM-dd") + "'" + _
                "," + QuotedStr(tbApproved.Text) + "," + QuotedStr(tbJobTitle.Text) + _
                "," + QuotedStr(tbRemark.Text) + "," + QuotedStr(ViewState("UserId").ToString) + ", getDate()"

            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM PEEmpTerminateHd WHERE TransNmbr = " + QuotedStr(tbTransNo.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE PEEmpRenewalHd SET Transdate = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + _
                "',EffectiveDate = '" + Format(tbEffectiveDate.SelectedValue, "yyyy-MM-dd") + _
                "',EmpAppr = " + QuotedStr(tbApproved.Text) + _
                ",JobTitleAppr = " + QuotedStr(tbJobTitle.Text) + _
                ",UserPrep = " + QuotedStr(ViewState("UserId").ToString) + _
                ",Remark = " + QuotedStr(tbRemark.Text) + ", DateAppr = getDate()" + _
                "WHERE TransNmbr = " + QuotedStr(tbTransNo.Text)
            End If
            SQLString = ChangeQuoteNull(SQLString)
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            'update Primary Key on Dt
            Dim Row As DataRow()

            Row = ViewState("Dt").Select("TransNmbr IS NULL")

            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbTransNo.Text
                Row(I).EndEdit()
            Next

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand(" SELECT TransNmbr, EmpNumb, NewEmpNumb, JobTitle, JobLevel, EmpStatus, " + _
                                         " Department,WorkPlace,Remark FROM PEEmpRenewalDt WHERE TransNmbr = '" & ViewState("Reference") & "'", con)

            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand


            Dim Dt As New DataTable("{PEEmpRenewalDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt
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
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
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
            tbFilter.Text = tbTransNo.Text
            ddlField.SelectedValue = "TransNmbr"
            btnSearch_Click(Nothing, Nothing)
            tbFilter.Text = CurrFilter
            ddlField.SelectedValue = Value
        Catch ex As Exception
            lbStatus.Text = "Save All Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, pnlDt, GridDt)
            newTrans()
            EnableHd(False)
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
            pnlDt.Visible = True
            GridDt.Columns(1).Visible = False
            BindDataDt("")
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        'Dim Dr As DataRow
        'Dim DT As DataTable
        'Dim SQLString As String
        Try

            'SQLString = "EXEC S_PEMedicalInCekTransdate " + QuotedStr(ViewState("ServerDate"))

            'DT = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            'If DT.Rows.Count > 0 Then
            '    Dr = DT.Rows(0)
            'Else
            '    Dr = Nothing
            'End If

            'If Not Dr Is Nothing Then
            '    tbDate.SelectedDate = Dr("Transdate").ToString
            'Else
            '    tbDate.SelectedDate = ViewState("ServerDate")
            'End If

            'Today

            tbTransNo.Text = ""
            tbDate.SelectedDate = ViewState("ServerDate")
            tbApproved.Text = ""
            tbApprovedName.Text = ""
            tbJobTitle.Text = ""
            tbJobTitleName.Text = ""
            tbContractEndDate.Text = ""
            tbRemark.Text = ""
            tbEffectiveDate.SelectedDate = ViewState("ServerDate")
            MultiView1.ActiveViewIndex = 0

        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbEmpCode.Text = ""
            tbEmpName.Text = ""
            tbNewEmpNo.Text = ""
            tbJobTitledt.Text = ""
            tbJobTitleNameDt.Text = ""
            tbJobLevel.Text = ""
            tbJobLevelName.Text = ""
            tbDept.Text = ""
            tbDeptName.Text = ""
            tbWorkPlace.Text = ""
            tbWorkPlaceName.Text = ""
            tbContractEndDate.Text = ""
            tbRemarkDt.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
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

    Protected Sub btnSaveAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveAll.Click
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            If IsNothing(ViewState("Dt")) Then
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
        Catch ex As Exception
            lbStatus.Text = "btn save all error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddDt.Click, btnAddDt2.Click
        Cleardt()
        If CekHd() = False Then
            Exit Sub
        End If
        ViewState("StateDt") = "Insert"
        MovePanel(pnlDt, pnlEditDt)
        EnableHd(False)
        StatusButtonSave(False)
    End Sub

    Function CekHd() As Boolean
        Try
            'If CInt(ViewState("GLYear")) <> Year(tbDate.SelectedValue) Or CInt(ViewState("GLPeriod")) <> Month(tbDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(ViewState("GLPeriod")) + " " + ViewState("GLYear").ToString.Trim)
            '    Return False
            'End If
            If tbEffectiveDate.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Effective Date Must have value")
                tbEffectiveDate.Focus()
                Return False
            End If

            If tbApproved.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Approved By Must have value")
                tbApproved.Focus()
                Return False
            End If



            Return True
        Catch ex As Exception
            Throw New Exception("Cek Hd Error : " + ex.ToString)
        End Try
    End Function

    Function CekDt(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not (Dr Is Nothing) Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If

                If Dr("EmpNumb").ToString.Trim = "" Then
                    lbStatus.Text = "Employee Must Have Value"
                    Return False
                End If

                If Dr("NewEmpNumb").ToString.Trim = "" Then
                    lbStatus.Text = "New Employee No. Must Have Value"
                    Return False
                End If

            Else
                If tbEmpCode.Text.Trim = "" Then
                    lbStatus.Text = "Employee Must Have Value"
                    tbEmpCode.Focus()
                    Return False
                End If

                If tbNewEmpNo.Text.Trim = "" Then
                    lbStatus.Text = "New Employee No. Must Have Value"
                    tbNewEmpNo.Focus()
                    Return False
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
            FDateName = "Date"
            FDateValue = "TransDate"
            FilterName = "Reference, Date, Effective Date, Emp. Approval Name,Job Title, Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), EffectiveDate,EmpAppr_Name, JobTtlAppr_Name, Remark"
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
                    ViewState("Status") = GVR.Cells(3).Text
                    If ViewState("Status") = "P" Then
                        GridDt.Columns(1).Visible = True
                    Else
                        GridDt.Columns(1).Visible = False
                    End If
                    GridDt.PageIndex = 0
                    MultiView1.ActiveViewIndex = 0
                    BindDataDt(ViewState("Reference"))
                    FillTextBoxHd(ViewState("Reference"))
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
                        GridDt.Columns(1).Visible = False
                        GridDt.PageIndex = 0
                        MultiView1.ActiveViewIndex = 0
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
                    Try
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        Session("SelectCommand") = "EXEC S_PEFormMedicalin ''" + QuotedStr(GVR.Cells(2).Text) + "''" + "," + QuotedStr(ViewState("UserId"))
                        Session("ReportFile") = ".../../../Rpt/FormMedicalin.frx"
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
        'Dim ds As DataSet
        'Dim i As Integer
        Try
            If e.CommandName = "Insert" Then
                btnAddDt_Click(Nothing, Nothing)
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub
    Dim AmountPaid As Decimal
    

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Dim dr() As DataRow
        Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
        dr = ViewState("Dt").Select("EmpNumb = " + QuotedStr(GVR.Cells(2).Text))
        dr(0).Delete()
        BindGridDt(ViewState("Dt"), GridDt)
        EnableHd(GetCountRecord(ViewState("Dt")) = 0)

    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            FillTextBoxDt(GVR.Cells(2).Text)
            ViewState("DtValue") = tbEmpCode.Text
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            tbEmpCode.Focus()
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub




    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            tbTransNo.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToDate(tbEffectiveDate, Dt.Rows(0)("EffectiveDate").ToString)
            BindToText(tbApproved, Dt.Rows(0)("EmpAppr").ToString)
            BindToText(tbApprovedName, Dt.Rows(0)("EmpAppr_Name").ToString)
            BindToText(tbJobTitle, Dt.Rows(0)("JobTitleAppr").ToString)
            BindToText(tbJobTitleName, Dt.Rows(0)("JobTtlAppr_Name").ToString)
            BindToText(tbRemark, Dt.Rows(0)("RemarkHd").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal KeyDt As String)
        Dim Dr As DataRow()

        Try
            Dr = ViewState("Dt").select("EmpNumb = " + QuotedStr(KeyDt))
            If Dr.Length > 0 Then
 
                BindToText(tbEmpCode, Dr(0)("EmpNumb").ToString)
                BindToText(tbEmpName, Dr(0)("Emp_Name").ToString)
                BindToText(tbJobLevel, Dr(0)("JobLevel").ToString)
                BindToText(tbJobLevelName, Dr(0)("JobLevel_Name").ToString)
                BindToText(tbJobTitledt, Dr(0)("JobTitle").ToString)
                BindToText(tbJobTitleNameDt, Dr(0)("JobTitle_Name").ToString)
                BindToText(tbEmpStatus, Dr(0)("EmpStatus").ToString)
                BindToText(tbEmpStatusName, Dr(0)("EmpStatus_Name").ToString)
                BindToText(tbDept, Dr(0)("Department").ToString)
                BindToText(tbDeptName, Dr(0)("Dept_Name").ToString)
                BindToText(tbWorkPlace, Dr(0)("WorkPlace").ToString)
                BindToText(tbWorkPlaceName, Dr(0)("WorkPlace_Name").ToString)
                BindToText(tbContractEndDate, Dr(0)("End_Date_Contract").ToString)
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

    Protected Sub btnApproved_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnApproved.Click
        Dim ResultField As String
        Try
            Session("Filter") = " SELECT Emp_No, Emp_Name, Job_Level, Job_Level_Name, Job_Title, Job_Title_Name, Emp_Status, Emp_Status_Name, Gender " + _
                                " FROM V_MsEmployee Where Fg_Active = 'Y' "
            ResultField = "Emp_No,Emp_Name,Job_Title,Job_Title_Name,Emp_Status,Gender"
            ViewState("Sender") = "btnApproved"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Family Click Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub tbApproved_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbApproved.TextChanged
        Dim Dr As DataRow
        Dim DT As DataTable
        Dim SQLString As String
        Try


            SQLString = " SELECT Emp_No, Emp_Name, Job_Level, Job_Title, Job_Title_Name, Emp_Status, Gender " + _
                        " FROM V_MsEmployee Where Fg_Active = 'Y' AND Emp_No = " + QuotedStr(tbApproved.Text)


            DT = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If DT.Rows.Count > 0 Then
                Dr = DT.Rows(0)
            Else
                Dr = Nothing
            End If

            If Not Dr Is Nothing Then
                BindToText(tbApproved, Dr("Emp_No").ToString)
                BindToText(tbApprovedName, Dr("Emp_Name").ToString)
                BindToText(tbJobTitle, Dr("Job_Title").ToString)
                BindToText(tbJobTitleName, Dr("Job_Title_Name").ToString)
            Else
                tbApproved.Text = ""
                tbApprovedName.Text = ""
                tbJobTitle.Text = ""
                tbJobTitleName.Text = ""
            End If
            tbApproved.Focus()

        Catch ex As Exception
            Throw New Exception("tb Employee Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub btnEmp_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEmp.Click
        Dim ResultField As String
        Try
            Session("Filter") = " SELECT Emp_No, Emp_Name, Job_Level, Job_Level_Name, Job_Title, Job_Title_Name, Emp_Status, Emp_Status_Name, " + _
                                " Department, Department_Name, Work_Place, Work_Place_Name, End_Date_Contract FROM V_MsEmployee Where Fg_Active = 'N' "
            ResultField = "Emp_No,Emp_Name,Job_Title,Job_Title_Name, Job_Level,Job_Level_Name," + _
                          "Emp_Status,Emp_Status_Name,Department,Department_Name,Work_Place,Work_Place_Name,End_Date_Contract"
            ViewState("Sender") = "btnEmployee"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Family Click Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub tbEmpCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbEmpCode.TextChanged
        Dim Dr As DataRow
        Dim DT As DataTable
        Dim SQLString As String
        Try
            SQLString = " SELECT Emp_No, Emp_Name, Job_Level, Job_Level_Name, Job_Title, Job_Title_Name, Emp_Status, Emp_Status_Name, " + _
                        " Department, Department_Name, Work_Place, Work_Place_Name, End_Date_Contract FROM V_MsEmployee Where Fg_Active = 'N' And Emp_No = " + QuotedStr(tbEmpCode.Text)

            DT = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If DT.Rows.Count > 0 Then
                Dr = DT.Rows(0)
            Else
                Dr = Nothing
            End If

            If Not Dr Is Nothing Then
                BindToText(tbEmpCode, Dr("Emp_No").ToString)
                BindToText(tbEmpName, Dr("Emp_Name").ToString)
                BindToText(tbJobTitledt, Dr("Job_Title").ToString)
                BindToText(tbJobTitleNameDt, Dr("Job_Title_Name").ToString)
                BindToText(tbJobLevel, Dr("Job_Level").ToString)
                BindToText(tbJobLevelName, Dr("Job_Level_Name").ToString)
                BindToText(tbEmpStatus, Dr("Emp_Status").ToString)
                BindToText(tbEmpStatusName, Dr("Emp_Status_Name").ToString)
                BindToText(tbDept, Dr("Department").ToString)
                BindToText(tbDeptName, Dr("Department_Name").ToString)
                BindToText(tbWorkPlace, Dr("Work_Place").ToString)
                BindToText(tbWorkPlaceName, Dr("Work_Place_Name").ToString)
                BindToText(tbContractEndDate, Dr("End_Date_Contract").ToString)
            Else
                tbEmpCode.Text = ""
                tbEmpName.Text = ""
                tbJobTitledt.Text = ""
                tbJobTitleNameDt.Text = ""
                tbJobLevel.Text = ""
                tbJobLevelName.Text = ""
                tbEmpStatus.Text = ""
                tbEmpStatusName.Text = ""
                tbDept.Text = ""
                tbDeptName.Text = ""
                tbJobTitledt.Text = ""
                tbWorkPlace.Text = ""
                tbWorkPlaceName.Text = ""
                tbContractEndDate.Text = ""
            End If
            tbEmpCode.Focus()

        Catch ex As Exception
            Throw New Exception("tb Employee Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub lbEmployee_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbEmployee.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsEmployee')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb Employee Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbApprovedBy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbApprovedBy.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsEmployee')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb Employee Error : " + ex.ToString
        End Try
    End Sub

   


End Class
