Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Odbc
'Imports CrystalDecisions.CrystalReports.Engine
'Imports CrystalDecisions.Shared
'Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc


Partial Class TrPERecruitment
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    'Protected GetStringHd As String = "Select * From V_PRCRequestHd WHERE UserId = " + QuotedStr(ViewState("UserId").ToString)

    Private Function GetStringHd() As String
        Return "Select * From V_PERecruitmenthd"
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

            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnCandidate" Then
                    tbCandidate.Text = Session("Result")(0).ToString
                    tbCandidateName.Text = Session("Result")(1).ToString
                End If
                If ViewState("Sender") = "btnRecruitment" Then
                    tbRecruitment.Text = Session("Result")(0).ToString
                    tbRecruitmentName.Text = Session("Result")(1).ToString
                    FillCombo(ddlGrade, "EXEC S_GetRecruitmentGrade " + QuotedStr(tbRecruitment.Text), True, "Grade", "Grade", ViewState("DBConnection"))
                    ddlGrade_SelectedIndexChanged(Nothing, Nothing)
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
        FillCombo(ddlJobTitle, "EXEC S_GetJobTitle", True, "JobTtlCode", "JobTtlName", ViewState("DBConnection"))
        FillCombo(ddlEmpStatus, "EXEC S_GetEmpStatus", True, "EmpStatusCode", "EmpStatusName", ViewState("DBConnection"))

        ViewState("Recruit") = ""
        ViewState("SortExpression") = Nothing
        ViewState("DigitCurr") = 0
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))

        If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
            ddlCommand.Items.Add("Print")
            ddlCommand2.Items.Add("Print")
        End If


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
        Return "SELECT * From V_PERecruitmentdt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function
    Private Function GetStringDt2(ByVal Nmbr As String) As String
        Return "SELECT * From V_PERecruitmentdtTest WHERE TransNmbr = " + QuotedStr(Nmbr)
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
    Public Function ExecSPCekPosting(ByVal ProcName As String, ByVal Nmbr As String, ByVal UserId As String, Optional ByVal Connection As String = "Nothing") As DataTable
        Dim Mycon As New SqlConnection
        Dim DT As DataTable

        Dim PrimaryKey() As String
        PrimaryKey = Nmbr.Split("|")
        Mycon = New SqlConnection(Connection)

        Dim sqlstring As String
        sqlstring = ""
        If PrimaryKey.Length = 1 Then
            sqlstring = "EXEC " + ProcName + " " + QuotedStr(PrimaryKey(0).ToString) + ", " + QuotedStr(UserId)
        ElseIf PrimaryKey.Length = 2 Then
            sqlstring = "EXEC " + ProcName + " " + QuotedStr(PrimaryKey(0).ToString) + ", " + QuotedStr(PrimaryKey(1).ToString) + ", " + QuotedStr(UserId)
        ElseIf PrimaryKey.Length = 3 Then
            sqlstring = "EXEC " + ProcName + " " + QuotedStr(PrimaryKey(0).ToString) + ", " + QuotedStr(PrimaryKey(1).ToString) + ", " + QuotedStr(PrimaryKey(2).ToString) + "," + QuotedStr(UserId)
        End If
        Try
            DT = SQLExecuteQuery(sqlstring, Connection).Tables(0)
            Return DT
        Catch ex As Exception
            Throw New Exception("Exec SP Posting Error : " + ex.ToString)
        Finally
            Mycon.Close()
        End Try
    End Function



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
                Session("SelectCommand") = "EXEC S_PEFormRecruitment  " + Result + "," + QuotedStr(ViewState("UserId"))
                'lbStatus.Text = Session("SelectCommand")
                'Exit Sub

                Session("ReportFile") = ".../../../Rpt/RptRecruitmentProcess.frx"
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

                        Result = ExecSPCommandGo(ActionValue, "S_PERecruitment", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                        If Trim(Result) <> "" Then
                            lbStatus.Text = lbStatus.Text + Result + " <br/>"
                        End If

                    Else
                        Result = ExecSPCommandGo(ActionValue, "S_PERecruitment", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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

    Private Sub BindDataDt2(ByVal Referens As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt2") = Nothing
            dt = SQLExecuteQuery(GetStringDt2(Referens), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt2") = dt
            BindGridDt(dt, GridDt2)

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
        Try
            If ViewState("StateDt") = "Edit" Then
                If ViewState("DtValue") <> ddlJobTitle.Text Then
                    If CekExistData(ViewState("Dt"), "JobTitle", ddlJobTitle.SelectedValue) Then
                        lbStatus.Text = "Job Title '" + ddlJobTitle.SelectedItem.Text(+"' has already exists")
                        Exit Sub
                    End If
                End If
                Row = ViewState("Dt").Select("JobTitle = " + QuotedStr(ViewState("DtValue")))(0)
                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                Row("CandidateNo") = tbCandidate.Text
                Row("CandidateName") = tbCandidateName.Text
                Row("Fglulus") = ddlLulus.SelectedValue
                Row("ReferenceBy") = tbReffBy.Text
                Row("Remark1") = tbRemark1.Text
                Row("Remark2") = tbRemark2.Text
                Row.EndEdit()
                
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If
                If CekExistData(ViewState("Dt"), "JobTitle", ddlJobTitle.SelectedValue) Then
                    lbStatus.Text = "Job Title '" + ddlJobTitle.SelectedItem.Text + "' has already exists"
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("CandidateNo") = tbCandidate.Text
                dr("CandidateName") = tbCandidateName.Text
                dr("Fglulus") = ddlLulus.SelectedValue
                dr("ReferenceBy") = tbReffBy.Text
                dr("Remark1") = tbRemark1.Text
                dr("Remark2") = tbRemark2.Text
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
            'If pnlDt.Visible = False Then
            '    lbStatus.Text = "Detail Data must be saved first"
            '    Exit Sub
            'End If
            If Not (ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit") Then
                Exit Sub
            Else
                ViewState("StateSave") = ViewState("StateHd")
                ViewState("StateHd") = "View"
            End If
            'Save Hd
            If ViewState("StateSave") = "Insert" Then
                tbTransNo.Text = GetAutoNmbr("CR", "N", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
                SQLString = "INSERT INTO PERecruitmentHd (TransNmbr, STATUS, Transdate, JobTitle , EmpStatus, " + _
                            "Remark,UserPrep,DatePrep) " + _
                "SELECT '" + tbTransNo.Text + "', 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', " + QuotedStr(ddlJobTitle.SelectedValue) + _
                "," + QuotedStr(ddlEmpStatus.SelectedValue) + "," + QuotedStr(tbRemark.Text) + _
                "," + QuotedStr(ViewState("UserId").ToString) + ", getDate()"

                ViewState("Reference") = QuotedStr(tbTransNo.Text)

            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM PERecruitmentHd WHERE TransNmbr = " + QuotedStr(tbTransNo.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE PERecruitmentHd SET Transdate = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + _
                "',JobTitle = " + QuotedStr(ddlJobTitle.SelectedValue) + _
                ",EmpStatus = " + QuotedStr(ddlEmpStatus.SelectedValue) + _
                ",Remark = " + QuotedStr(tbRemark.Text) + ", DateAppr = getDate()" + _
                " WHERE TransNmbr = " + QuotedStr(tbTransNo.Text)
                ViewState("Reference") = QuotedStr(tbTransNo.Text)

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

            If Not ViewState("Dt2") Is Nothing Then
                Row = ViewState("Dt2").Select("TransNmbr IS NULL")
                For I = 0 To Row.Length - 1
                    Row(I).BeginEdit()
                    Row(I)("TransNmbr") = tbTransNo.Text
                    Row(I)("CandidateNo") = lbCandidateNo.Text
                    Row(I).EndEdit()
                Next
            End If


            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString
            con = New SqlConnection(ConnString)
            con.Open()

            Dim cmdSql As New SqlCommand(" SELECT TransNmbr, CandidateNo, FgLulus, ReferenceBy, Remark1, Remark2" + _
                                         " FROM PERecruitmentdt WHERE TransNmbr = " + ViewState("Reference"), con)


            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand


            Dim Dt As New DataTable("PERecruitmentdt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt

            If Not ViewState("Dt2") Is Nothing Then

                cmdSql = New SqlCommand(" SELECT TransNmbr,CandidateNo,RecruitmentTest, Grade" + _
                                        " FROM PERecruitmentdtTest WHERE TransNmbr = " + ViewState("Reference"), con)


                da = New SqlDataAdapter(cmdSql)
                dbcommandBuilder = New SqlCommandBuilder(da)

                da.InsertCommand = dbcommandBuilder.GetInsertCommand
                da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
                da.UpdateCommand = dbcommandBuilder.GetUpdateCommand


                Dim Dt2 As New DataTable("PERecruitmentdtTest")

                Dt2 = ViewState("Dt2")
                da.Update(Dt2)
                Dt2.AcceptChanges()
                ViewState("Dt2") = Dt2

            End If


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
            
            'If ViewState Then

            'End If
            'For Each dr In ViewState("Dt").Rows
            '    If CekDt(dr) = False Then
            '        Exit Sub
            '    End If
            'Next

            If GetCountRecord(ViewState("Dt2")) = 0 Then
                lbStatus.Text = MessageDlg("Detail 2 must have at least 1 record")
                Exit Sub
            End If


            'For Each dr In ViewState("Dt2").Rows
            '    If CekDt(dr) = False Then
            '        Exit Sub
            '    End If
            'Next


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
            'ViewState("DigitCurr") = 0
            EnableHd(False)
            'btnHome.Visible = False           

            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, pnlDt, GridDt)
            ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
            newTrans()
            btnHome.Visible = False
            MultiView1.ActiveViewIndex = 0
            Menu1.Items.Item(0).Selected = True
            tbDate.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ViewState("Reference") = ""
            ClearHd()
            Cleardt()
            pnlDt.Visible = True
            'GridDt.Columns(1).Visible = False
            BindDataDt("")
            BindDataDt2("")
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbTransNo.Text = ""
            tbDate.SelectedDate = ViewState("ServerDate")
            tbRemark.Text = ""
            ddlJobTitle.SelectedValue = ""
            ddlEmpStatus.SelectedValue = ""
            tbRemark.Text = ""
            MultiView1.ActiveViewIndex = 0

        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbCandidate.Text = ""
            tbCandidateName.Text = ""
            ddlLulus.SelectedValue = "N"
            tbReffBy.Text = ""
            tbRemark1.Text = ""
            tbRemark2.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt2()
        Try
            tbRecruitment.Text = ""
            tbRecruitmentName.Text = ""
            ddlGrade.Text = ""
            tbRangeValue.Text = ""
            tbLulus.Text = ""
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

            If IsNothing(ViewState("Dt2")) Then
                lbStatus.Text = MessageDlg("Detail2 (Test) must have at least 1 record")
                Exit Sub
            End If
            For Each dr In ViewState("Dt2").Rows
                If CekDt2(dr) = False Then
                    Exit Sub
                End If
            Next
            SaveAll()
            newTrans()
        Catch ex As Exception
            lbStatus.Text = "btn save all error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddDt.Click, btnAddDtKe2.Click
        Cleardt()
        If CekHd() = False Then
            Exit Sub
        End If
        ViewState("StateDt") = "Insert"
        MovePanel(pnlDt, pnlEditDt)
        EnableHd(False)
        StatusButtonSave(False)
        tbCandidate.Focus()
    End Sub
    Protected Sub btnAddDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddDt2.Click, btnAddDt2Ke2.Click
        Try
            Cleardt2()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt2") = "Insert"
            MovePanel(pnlDt2, pnlEditDt2)
            EnableHd(False)
            StatusButtonSave(False)
            tbRecruitment.Focus()

        Catch ex As Exception
            lbStatus.Text = "btn add dt error : " + ex.ToString
        End Try
    End Sub

    Function CekHd() As Boolean
        Try
            'If CInt(ViewState("GLYear")) <> Year(tbDate.SelectedValue) Or CInt(ViewState("GLPeriod")) <> Month(tbDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(ViewState("GLPeriod")) + " " + ViewState("GLYear").ToString.Trim)
            '    Return False
            'End If
            
            If ddlJobTitle.SelectedValue.Trim = "" Then
                lbStatus.Text = MessageDlg("Job Title Must have value")
                ddlJobTitle.Focus()
                Return False
            End If

            If ddlEmpStatus.SelectedValue.Trim = "" Then
                lbStatus.Text = MessageDlg("Employee Status Must have value")
                ddlJobTitle.Focus()
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
                If Dr("CandidateNo").ToString.Trim = "" Then
                    lbStatus.Text = "Candidate Must Have Value"
                    Return False
                End If

            Else
                If tbCandidate.Text.Trim = "" Then
                    lbStatus.Text = "Candidate Must Have Value"
                    ddlJobTitle.Focus()
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function

    Function CekDt2(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not (Dr Is Nothing) Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("Recruitment").ToString.Trim = "" Then
                    lbStatus.Text = "Recruitment Must Have Value"
                    Return False
                End If

            Else
                If tbRecruitment.Text.Trim = "" Then
                    lbStatus.Text = "Recruitment Must Have Value"
                    tbRecruitment.Focus()
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
        'Dim cb, cbselek As CheckBox
        'Dim GRW As GridViewRow
        'Try
        '    cb = sender
        '    For Each GRW In GridView1.Rows
        '        cbselek = GRW.FindControl("cbSelect")
        '        cbselek.Checked = cb.Checked
        '    Next
        'Catch ex As Exception
        '    lbStatus.Text = "cb Hd Checked Changed Error : " + ex.ToString
        'End Try
    End Sub

    Protected Sub LbAdvSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LbAdvSearch.Click
        Dim FDateName, FDateValue, FilterName, FilterValue As String
        Try
            FDateName = "Date"
            FDateValue = "TransDate"
            FilterName = "Reference, Date, Request Type, Department, Request By, Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), RequestType, Department, RequestBy, Remark"
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
                    GridDt.PageIndex = 0
                    MultiView1.ActiveViewIndex = 0
                    BindDataDt(ViewState("Reference"))
                    BindDataDt2(ViewState("Reference"))
                    FillTextBoxHd(ViewState("Reference"))
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, pnlDt, GridDt)
                    ModifyInput2(False, pnlInput, pnlDt2, GridDt2)
                    MultiView1.ActiveViewIndex = 0
                    Menu1.Items.Item(0).Selected = True
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
                        'GridDt.Columns(1).Visible = False
                        GridDt.PageIndex = 0
                        MultiView1.ActiveViewIndex = 0
                        BindDataDt(ViewState("Reference"))
                        BindDataDt2(ViewState("Reference"))
                        FillTextBoxHd(ViewState("Reference"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
                        btnHome.Visible = False
                        MultiView1.ActiveViewIndex = 0
                        Menu1.Items.Item(0).Selected = True
                        EnableHd(GetCountRecord(ViewState("Dt2")) = 0)
                        'EnableHd(GetCountRecord(ViewState("Dt")) = 0)
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
                        Session("SelectCommand") = "EXEC S_PEFormRecruitment  ''" + QuotedStr(GVR.Cells(2).Text) + "''" + "," + QuotedStr(ViewState("UserId"))
                        Session("ReportFile") = ".../../../Rpt/RptRecruitmentProcess.frx"
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
        Try
            If e.CommandName = "Detail" Then
                Dim GVR As GridViewRow
                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))
                If GVR.Cells(2).Text = "&nbsp;" Then
                    Exit Sub
                End If
                lbCandidateNo.Text = GVR.Cells(2).Text
                MultiView1.ActiveViewIndex = 1

                BindDataDt2(ViewState("Reference"))

                Dim drow As DataRow()
                If tbTransNo.Text.Trim = "" Then
                    BindDataDt2(ViewState("Reference"))
                    'GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
                    GridDt2.Columns(0).Visible = False
                Else
                    BindDataDt2(ViewState("Reference"))
                    drow = ViewState("Dt2").Select("CandidateNo=" + QuotedStr(lbCandidateNo.Text))
                    If drow.Length > 0 Then
                        BindGridDt(drow.CopyToDataTable, GridDt2)
                        GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
                    Else
                        Dim DtTemp As DataTable
                        DtTemp = ViewState("Dt2").Clone
                        DtTemp.Rows.Add(DtTemp.NewRow())
                        GridDt2.DataSource = DtTemp
                        GridDt2.DataBind()
                        GridDt2.Columns(0).Visible = False
                    End If
                End If

            ElseIf e.CommandName = "Insert" Then
                btnAddDt_Click(Nothing, Nothing)
            ElseIf e.CommandName = "Closing" Then
                Dim GVR As GridViewRow
                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))
                ViewState("JobTitleClose") = GVR.Cells(2).Text
                ViewState("EmpStatusClose") = GVR.Cells(3).Text
                AttachScript("closing();", Page, Me.GetType)
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub
    Dim AmountPaid As Decimal
    

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Dim dr() As DataRow
        Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
        dr = ViewState("Dt").Select("CandidateNo = " + QuotedStr(GVR.Cells(2).Text))
        dr(0).Delete()
        BindGridDt(ViewState("Dt"), GridDt)
        EnableHd(GetCountRecord(ViewState("Dt")) = 0)

    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            FillTextBoxDt(GVR.Cells(2).Text)
            ViewState("DtValue") = tbCandidate.Text
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            ddlJobTitle.Focus()
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
            BindToDropList(ddlJobTitle, Dt.Rows(0)("JobTitle").ToString)
            BindToDropList(ddlEmpStatus, Dt.Rows(0)("EmpStatus").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal KeyDt As String)
        Dim Dr As DataRow()

        Try
            Dr = ViewState("Dt").select("JobTitle = " + QuotedStr(KeyDt))
            If Dr.Length > 0 Then
                BindToText(tbCandidate, Dr(0)("CandidateNo").ToString)
                BindToText(tbCandidateName, Dr(0)("CandidateName").ToString)
                BindToText(tbReffBy, Dr(0)("ReferenceBy").ToString)
                BindToText(tbRemark1, Dr(0)("Remark1").ToString)
                BindToText(tbRemark2, Dr(0)("Remark2").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt2(ByVal KeyDt As String, ByVal Candidate As String)
        Dim Dr As DataRow()

        Try
            'Dr = ViewState("Dt").select("CandidateNo = " + QuotedStr(KeyDt))
            Dr = ViewState("Dt2").select("CandidateNo = " + QuotedStr(KeyDt) + " AND RecruitmentTest =" + QuotedStr(Candidate.ToString))
            If Dr.Length > 0 Then
                BindToText(tbRecruitment, Dr(0)("RecruitmentTest").ToString)
                BindToText(tbRecruitmentName, Dr(0)("RecruitmentName").ToString)
                FillCombo(ddlGrade, "EXEC S_GetRecruitmentGrade " + QuotedStr(tbRecruitment.Text), True, "Grade", "Grade", ViewState("DBConnection"))
                BindToDropList(ddlGrade, Dr(0)("Grade").ToString)
                BindToText(tbRangeValue, Dr(0)("RangeValue").ToString)
                BindToText(tbLulus, Dr(0)("FgLulus").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub GridDt2_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt2.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt2.Rows(e.NewEditIndex)
            FillTextBoxDt2(lbCandidateNo.Text, GVR.Cells(1).Text)
            MovePanel(pnlDt2, pnlEditDt2)
            EnableHd(False)
            ViewState("StateDt2") = "Edit"
            'ViewState("DtValue") = tbCandidate.Text
            ViewState("DtValue2") = GVR.Cells(1).Text
            StatusButtonSave(False)
            btnSaveDt2.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid dt2 Editing Error : " + ex.ToString
        End Try
    End Sub
    Private Sub StatusButtonSave(ByVal Bool As Boolean)
        btnSaveAll.Visible = Bool
        btnSaveTrans.Visible = Bool
        btnBack.Visible = Bool
    End Sub
    Protected Sub lbEmpStatus_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbEmpStatus.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsEmpStatus')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb Employee Status Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub lbJobTitle_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbJobTitle.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsJobTitle')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb Job Title Error : " + ex.ToString
        End Try
    End Sub

   
   
    Protected Sub btnCandidate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCandidate.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT * FROM V_PECandidateForRecruitmentHd WHERE " + _
                                " Job_Title_Code = " + QuotedStr(ddlJobTitle.SelectedValue) + _
                                " AND Employee_Status_Code = " + QuotedStr(ddlEmpStatus.SelectedValue)
            ResultField = "Candidate_No, Candidate_Name"
            ViewState("Sender") = "btnCandidate"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Candidate Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbCandidate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCandidate.TextChanged
        Dim Dr As DataRow
        Dim Dt As DataTable
        Dim SQLString As String
        Try
            SQLString = "SELECT * FROM V_PECandidateForRecruitmentHd WHERE Candidate_No = " + QuotedStr(tbCandidate.Text) + _
                        " AND Job_Title_Code = " + QuotedStr(ddlJobTitle.SelectedValue) + _
                        " AND Employee_Status_Code = " + QuotedStr(ddlEmpStatus.SelectedValue)

            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
            Else
                Dr = Nothing
            End If
            If Not Dr Is Nothing Then
                BindToText(tbCandidate, Dr("Candidate_No").ToString)
                BindToText(tbCandidateName, Dr("Candidate_Name").ToString)
            Else
                tbCandidate.Text = ""
                tbCandidateName.Text = ""
            End If
        Catch ex As Exception
            Throw New Exception("tb Candidate change Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        Dim index As Integer
        Try
            index = Int32.Parse(e.Item.Value)
            MultiView1.ActiveViewIndex = index
            btnSaveTrans.Focus()
        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnRecruitment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRecruitment.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT * FROM V_MsRecruitment"
            ResultField = "RecruitmentCode, RecruitmentName"
            ViewState("Sender") = "btnRecruitment"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Recruitment Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbRecruitment_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbRecruitment.TextChanged
        Dim Dr As DataRow
        Dim Dt As DataTable
        Dim SQLString As String
        Try
            SQLString = "SELECT * FROM V_MsRecruitment WHERE RecruitmentCode = " + QuotedStr(tbRecruitment.Text)
            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
            Else
                Dr = Nothing
            End If
            If Not Dr Is Nothing Then
                BindToText(tbRecruitment, Dr("RecruitmentCode").ToString)
                BindToText(tbRecruitmentName, Dr("RecruitmentName").ToString)
                ddlGrade_SelectedIndexChanged(Nothing, Nothing)
                tbRangeValue.Text = ""
                tbLulus.Text = ""
            Else
                tbRecruitment.Text = ""
                tbRecruitmentName.Text = ""
                ddlGrade.Text = ""
                tbRangeValue.Text = ""
                tbLulus.Text = ""
            End If
        Catch ex As Exception
            Throw New Exception("tb Recruitment change Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlGrade_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlGrade.SelectedIndexChanged
        Dim Dr As DataRow
        Dim Dt As DataTable
        Dim SQLString As String
        Try
            SQLString = "SELECT * FROM VMsRecruitmentGrade WHERE RecruitmentCode = " + QuotedStr(tbRecruitment.Text) + " AND Grade = " + QuotedStr(ddlGrade.SelectedValue)
            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
            Else
                Dr = Nothing
            End If
            If Not Dr Is Nothing Then
                BindToText(tbRangeValue, Dr("RangeValue").ToString)
                BindToText(tbLulus, Dr("FgLulus").ToString)
            Else
                tbRangeValue.Text = ""
                tbLulus.Text = ""
            End If
        Catch ex As Exception
            Throw New Exception("ddl Grade change Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSaveDt2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt2.Click
        Dim Row As DataRow
        Try
            If ViewState("StateDt2") = "Edit" Then
                If ViewState("DtValue2") <> tbRecruitment.Text Then
                    If CekExistData(ViewState("Dt2"), "RecruitmentTest", tbRecruitment.Text) Then
                        lbStatus.Text = "Recruitment Test '" + tbRecruitment.Text(+"' has already exists")
                        Exit Sub
                    End If
                End If
                Row = ViewState("Dt2").Select("RecruitmentTest = " + QuotedStr(ViewState("DtValue2")))(0)
                If CekDt2() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                Row("RecruitmentTest") = tbRecruitment.Text
                Row("RecruitmentName") = tbRecruitmentName.Text
                Row("RangeValue") = tbRangeValue.Text
                Row("FgLulus") = tbLulus.Text
                Row("Grade") = ddlGrade.SelectedValue
                Row.EndEdit()
            Else
                'Insert
                If CekDt2() = False Then
                    Exit Sub
                End If
                If CekExistData(ViewState("Dt2"), "RecruitmentTest", tbRecruitment.Text) Then
                    lbStatus.Text = "Recruitment Test '" + tbRecruitment.Text + "' has already exists"
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt2").NewRow
                dr("RecruitmentTest") = tbRecruitment.Text
                dr("RecruitmentName") = tbRecruitmentName.Text
                dr("RangeValue") = tbRangeValue.Text
                dr("FgLulus") = tbLulus.Text
                dr("Grade") = ddlGrade.SelectedValue
                ViewState("Dt2").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt2, pnlDt2)
            EnableHd(GetCountRecord(ViewState("Dt2")) = 0)
            BindGridDt(ViewState("Dt2"), GridDt2)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save Dt2 Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub

    Protected Sub btnBackDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBackDt2.Click, btnBackDt2Ke2.Click
        MultiView1.ActiveViewIndex = 0
    End Sub

    Protected Sub btnCancelDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelDt2.Click
        Try
            MovePanel(pnlEditDt2, pnlDt2)
            EnableHd(GetCountRecord(ViewState("Dt2")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt2  Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub GridDt2_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt2.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow = GridDt2.Rows(e.RowIndex)
            dr = ViewState("Dt2").Select("CandidateNo = " + QuotedStr(lbCandidateNo.Text) + " AND RecruitmentTest = " + QuotedStr(GVR.Cells(1).Text))
            dr(0).Delete()

            Dim drow As DataRow()
            drow = ViewState("Dt2").Select("CandidateNo = " + QuotedStr(lbCandidateNo.Text))
            If drow.Length > 0 Then
                BindGridDt(drow.CopyToDataTable, GridDt2)
                GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
            Else
                Dim DtTemp As New DataTable
                DtTemp = ViewState("Dt2").Clone
                DtTemp.Rows.Add(DtTemp.NewRow()) ' create a new blank row to the DataTable
                GridDt2.DataSource = DtTemp
                GridDt2.DataBind()
                GridDt2.Columns(0).Visible = False
            End If
            
        Catch ex As Exception
            lbStatus.Text = "Grid Dt 2 Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbRecruitment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbRecruitment.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsRecruitment')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb Recruitment Status Error : " + ex.ToString
        End Try

    End Sub
End Class
