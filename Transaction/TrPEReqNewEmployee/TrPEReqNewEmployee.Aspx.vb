Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Odbc
'Imports CrystalDecisions.CrystalReports.Engine
'Imports CrystalDecisions.Shared
'Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc


Partial Class TrPEReqNewEmployee
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    'Protected GetStringHd As String = "Select * From V_PRCRequestHd WHERE UserId = " + QuotedStr(ViewState("UserId").ToString)

    Private Function GetStringHd() As String
        Return "Select * From V_PERequestEmpHd"
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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

                Session("Result") = Nothing
                ViewState("Sender") = Nothing
                Session("filter") = Nothing
                Session("Column") = Nothing
            End If

            If Not ViewState("JobTitleClose") Is Nothing Then
                Dim Row As DataRow()
                Row = ViewState("Dt").Select("JobTitle = " + QuotedStr(ViewState("JobTitleClose")) + " AND EmpStatus = " + QuotedStr(ViewState("EmpStatusClose")))


                If HiddenRemarkClose.Value <> "False Value" Then
                    Dim sqlstring, result As String
                    sqlstring = "Declare @A VarChar(255) EXEC S_PERequestEmpClosing " + QuotedStr(tbTransNo.Text) + "," + QuotedStr(ViewState("JobTitleClose")) + "," + QuotedStr(ViewState("EmpStatusClose")) + "," + QuotedStr(HiddenRemarkClose.Value) + "," + QuotedStr(ViewState("ServerDate")) + ", " + QuotedStr(ViewState("UserId")) + ", @A OUT SELECT @A"
                    result = SQLExecuteScalar(sqlstring, ViewState("DBConnection"))
                    If result.Length > 2 Then
                        lbStatus.Text = MessageDlg(result)
                    Else
                        BindDataDt(ViewState("Reference"))
                    End If
                End If
                ViewState("JobTitleClose") = Nothing
                ViewState("EmpStatusClose") = Nothing
                GridDt.Columns(0).Visible = False
                HiddenRemarkClose.Value = ""
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
        FillCombo(ddlDepartment, "EXEC S_GetDepartment", True, "Department_Code", "Department_Name", ViewState("DBConnection"))
        FillCombo(ddlJobTitle, "EXEC S_GetJobTitle", True, "JobTtlCode", "JobTtlName", ViewState("DBConnection"))
        FillCombo(ddlEmpStatus, "EXEC S_GetEmpStatus", True, "EmpStatusCode", "EmpStatusName", ViewState("DBConnection"))
        
        ViewState("SortExpression") = Nothing
        ViewState("DigitCurr") = 0
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))

        If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
            ddlCommand.Items.Add("Print")
            ddlCommand2.Items.Add("Print")
        End If

        tbTotalMale.Attributes.Add("ReadOnly", "True")
        tbTotalFemale.Attributes.Add("ReadOnly", "True")
        tbTotal.Attributes.Add("ReadOnly", "True")

        'tbTotalMale.Attributes.Add("OnBlur", "setformat('-');")

        tbMale.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbFemale.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbTotaldt.Attributes.Add("OnKeyDown", "return PressNumeric();")
        

        tbTotalMale.Attributes.Add("OnBlur", "setformat();")
        tbTotalFemale.Attributes.Add("OnBlur", "setformat();")
        tbTotal.Attributes.Add("OnBlur", "setformat();")
        
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
        Return "SELECT * From V_PERequestEmpdt WHERE TransNmbr = " + QuotedStr(Nmbr)
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
                Session("SelectCommand") = "EXEC S_PEFormRequestEmp " + Result + "," + QuotedStr(ViewState("UserId"))
                Session("ReportFile") = ".../../../Rpt/FormRequestEmp.frx"
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

                        Result = ExecSPCommandGo(ActionValue, "S_PERequestEmp", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                        If Trim(Result) <> "" Then
                            lbStatus.Text = lbStatus.Text + Result + " <br/>"
                        End If

                    Else
                        Result = ExecSPCommandGo(ActionValue, "S_PERequestEmp", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
                Row("JobTitle") = ddlJobTitle.SelectedValue
                Row("EmpStatus") = ddlEmpStatus.Text
                Row("RangeTime") = tbJangkaWaktu.Text
                Row("StartDate") = dpStartDate.SelectedDate
                'tbMale.Text = tbMale.Text.Replace(",", "")
                'tbFemale.Text = tbFemale.Text.Replace(",", "")
                'tbTotaldt.Text = tbTotaldt.Text.Replace(",", "")
                Row("QtyMale") = CInt(tbMale.Text)
                Row("QtyFemale") = CInt(tbFemale.Text)
                Row("QtyTotal") = CInt(tbTotaldt.Text)
                Row("SyaratUmum") = tbSyaratUmum.Text
                Row("SyaratKhusus") = tbSyaratKhusus.Text
                Row("Responsibility") = tbResponsibility.Text
                Row("Remark") = tbRemarkDt.Text()
                Row.EndEdit()
                SumTotalMale()
                SumTotalFemale()
                SumTotal()

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
                dr("JobTitle") = ddlJobTitle.SelectedValue
                dr("EmpStatus") = ddlEmpStatus.SelectedValue
                dr("RangeTime") = tbJangkaWaktu.Text
                dr("StartDate") = dpStartDate.SelectedDate
                'tbMale.Text = tbMale.Text.Replace(",", "")
                'tbFemale.Text = tbFemale.Text.Replace(",", "")
                'tbTotaldt.Text = tbTotaldt.Text.Replace(",", "")
                dr("QtyMale") = CInt(tbMale.Text)
                dr("QtyFemale") = CInt(tbFemale.Text)
                dr("QtyTotal") = CInt(tbTotaldt.Text)
                dr("SyaratUmum") = tbSyaratUmum.Text
                dr("SyaratKhusus") = tbSyaratKhusus.Text
                dr("Responsibility") = tbResponsibility.Text
                dr("Remark") = tbRemarkDt.Text
                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            BindGridDt(ViewState("Dt"), GridDt)
            SumTotalMale()
            SumTotalFemale()
            SumTotal()
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
                tbTransNo.Text = GetAutoNmbr("RE", "N", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
                SQLString = "INSERT INTO PERequestEmphd (TransNmbr, STATUS, Transdate, Department , RequestType, TotalMale, TotalFemale, Total," + _
                            "Reason, Remark, UserPrep,DatePrep) " + _
                "SELECT '" + tbTransNo.Text + "', 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', " + QuotedStr(ddlDepartment.SelectedValue) + _
                "," + QuotedStr(ddlReqType.SelectedValue) + "," + QuotedStr(tbTotalMale.Text.Replace(",", "")) + "," + QuotedStr(tbTotalFemale.Text.Replace(",", "")) + _
                "," + QuotedStr(tbTotal.Text.Replace(",", "")) + ", " + QuotedStr(tbReason.Text) + _
                "," + QuotedStr(tbRemark.Text) + _
                "," + QuotedStr(ViewState("UserId").ToString) + ", getDate()"

            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM PERequestEmphd WHERE TransNmbr = " + QuotedStr(tbTransNo.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE PERequestEmphd SET Transdate = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + _
                "',Department = " + QuotedStr(ddlDepartment.SelectedValue) + " ,Reason = " + QuotedStr(tbReason.Text) + _
                ",RequestType = " + QuotedStr(ddlReqType.SelectedValue) + ", TotalMale =" + QuotedStr(tbTotalMale.Text.Replace(",", "")) + _
                ",TotalFemale = " + QuotedStr(tbTotalFemale.Text.Replace(",", "")) + ", Total = " + QuotedStr(tbTotal.Text.Replace(",", "")) + _
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

            Dim cmdSql As New SqlCommand(" SELECT TransNmbr, JobTitle, EmpStatus, RangeTime, StartDate, QtyMale," + _
                                         " QtyFemale, QtyTotal, SyaratUmum, SyaratKhusus, Responsibility," + _
                                         " Remark FROM PERequestEmpdt WHERE TransNmbr = '" & ViewState("Reference") & "'", con)

            'Dim cmdSql As New SqlCommand(" SELECT TransNmbr, Medical, ClaimType, ReceiptNo, ReceiptDate, AmountClaim, PercentPaid, AmountPaid, AmountTobePaid, " + _
            '                            " Remark FROM PEMedicalClaimdt WHERE TransNmbr = '" & ViewState("Reference") & "'", con)
            
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand


            Dim Dt As New DataTable("{PERequestEmpdt")

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
        Try
            tbTransNo.Text = ""
            tbDate.SelectedDate = ViewState("ServerDate") 'Today
            tbRemark.Text = ""
            ddlDepartment.SelectedValue = ""
            ddlJobTitle.SelectedValue = ""
            ddlEmpStatus.SelectedValue = ""

            ddlReqType.SelectedIndex = 0
            tbReason.Text = ""
            tbTotalMale.Text = 0
            tbTotalFemale.Text = 0
            tbTotal.Text = 0
            tbRemark.Text = ""
            MultiView1.ActiveViewIndex = 0

        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            ddlJobTitle.SelectedIndex = 0
            ddlEmpStatus.SelectedIndex = 0
            ddlJobTitle.Enabled = True
            ddlEmpStatus.Enabled = True
            tbJangkaWaktu.Text = ""
            dpStartDate.SelectedDate = ViewState("ServerDate")
            tbRemarkDt.Text = ""
            tbMale.Text = "0"
            tbFemale.Text = "0"
            tbTotaldt.Text = "0"
            tbSyaratKhusus.Text = ""
            tbSyaratUmum.Text = ""
            tbResponsibility.Text = ""
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
            If ddlReqType.SelectedValue.Trim = "" Then
                lbStatus.Text = MessageDlg("Request Type Must have value")
                ddlReqType.Focus()
                Return False
            End If
            
            If ddlDepartment.SelectedValue.Trim = "" Then
                lbStatus.Text = MessageDlg("Department Must have value")
                ddlDepartment.Focus()
                Return False
            End If

            If tbReason.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Reason must have value")
                tbReason.Focus()
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
                If Dr("JobTitle").ToString.Trim = "" Then
                    lbStatus.Text = "Job Title Must Have Value"
                    Return False
                End If
                If Dr("EmpStatus").ToString.Trim = "" Then
                    lbStatus.Text = "Employee Status Must Have Value"
                    Return False
                End If
                'If CFloat(Dr("RangeTime").ToString) <= 0 Then
                '    lbStatus.Text = "Work Period Must Have Value"
                '    Return False
                'End If
                If CFloat(Dr("QtyMale").ToString) < 0 Then
                    lbStatus.Text = "Male Must Have Value"
                    Return False
                End If
                If CFloat(Dr("QtyFemale").ToString) < 0 Then
                    lbStatus.Text = "Female Must Have Value"
                    Return False
                End If
                If CFloat(Dr("QtyTotal").ToString) < 0 Then
                    lbStatus.Text = "Male / Female Must Have Value"
                    Return False
                End If
                If CFloat(Dr("QtyMale").ToString) <= 0 And CFloat(Dr("QtyFemale").ToString) <= 0 And CFloat(Dr("QtyTotal").ToString) <= 0 Then
                    lbStatus.Text = "Person Requirement Must Have Value"
                    Return False
                End If
                If Dr("SyaratUmum").ToString.Trim = "" Then
                    lbStatus.Text = "General Requirements Must Have Value"
                    Return False
                End If
                If Dr("SyaratKhusus").ToString.Trim = "" Then
                    lbStatus.Text = "Special Requirements Must Have Value"
                    Return False
                End If
                If Dr("Responsibility").ToString.Trim = "" Then
                    lbStatus.Text = "Responsibility Must Have Value"
                    Return False
                End If
                If Dr("Remark").ToString.Trim = "" Then
                    lbStatus.Text = "Remark Must Have Value"
                    Return False
                End If
                

            Else
                If ddlJobTitle.SelectedValue.Trim = "" Then
                    lbStatus.Text = "Job Title Must Have Value"
                    ddlJobTitle.Focus()
                    Return False
                End If
                If ddlEmpStatus.SelectedValue.Trim = "" Then
                    lbStatus.Text = "Employee Status Must Have Value"
                    ddlEmpStatus.Focus()
                    Return False
                End If
                'If CFloat(tbJangkaWaktu.Text) <= 0 Then
                '    lbStatus.Text = "Work Period Must Have Value"
                '    tbJangkaWaktu.Focus()
                '    Return False
                'End If
                If CFloat(tbMale.Text) < 0 Then
                    lbStatus.Text = "Male Must Have Value"
                    tbMale.Focus()
                    Return False
                End If
                If CFloat(tbFemale.Text) < 0 Then
                    lbStatus.Text = "Female Must Have Value"
                    tbFemale.Focus()
                    Return False
                End If
                If CFloat(tbTotaldt.Text) < 0 Then
                    lbStatus.Text = "Male / Female Must Have Value"
                    tbTotaldt.Focus()
                    Return False
                End If
                If CFloat(tbMale.Text) <= 0 And CFloat(tbFemale.Text) <= 0 And CFloat(tbTotaldt.Text) <= 0 Then
                    lbStatus.Text = "Person Requirement Must Have Value"
                    tbTotaldt.Focus()
                    Return False
                End If
                If tbSyaratUmum.Text.Trim = "" Then
                    lbStatus.Text = "General Requirements Must Have Value"
                    tbSyaratUmum.Focus()
                    Return False
                End If
                If tbSyaratKhusus.Text.Trim = "" Then
                    lbStatus.Text = "Special Requirements Must Have Value"
                    tbSyaratKhusus.Focus()
                    Return False
                End If

            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function

    'Protected Sub ddlJE_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlJE.SelectedIndexChanged
    '    Try
    '        If ViewState("InputFormatJE") = "Y" Then
    '            RefreshMaster("S_GetFormatJE", "Format_Code", "Format_Name", ddlJE, ViewState("DBConnection"))
    '            ViewState("InputFormatJE") = Nothing
    '        End If
    '    Catch ex As Exception
    '        lbStatus.Text = "ddl JE Error : " + ex.ToString
    '    End Try
    'End Sub

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
                        Session("SelectCommand") = "EXEC S_PEFormRequestEmp ''" + QuotedStr(GVR.Cells(2).Text) + "''" + "," + QuotedStr(ViewState("UserId"))
                        Session("ReportFile") = ".../../../Rpt/FormRequestEmp.frx"
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
            If e.CommandName = "Insert" Then
                btnAddDt_Click(Nothing, Nothing)
            ElseIf e.CommandName = "Closing" Then
                Dim GVR As GridViewRow
                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))
                'If ViewState("Status") <> "P" Then
                'lbStatus.Text = MessageDlg("Status New Request Employee is not Post, cannot close Job Title")
                'Exit Sub
                ViewState("JobTitleClose") = GVR.Cells(2).Text
                ViewState("EmpStatusClose") = GVR.Cells(3).Text
                AttachScript("closing();", Page, Me.GetType)
                'End If
                End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub
    Dim AmountPaid As Decimal
    

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Dim dr() As DataRow
        Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
        dr = ViewState("Dt").Select("JobTitle = " + QuotedStr(GVR.Cells(2).Text))
        dr(0).Delete()
        BindGridDt(ViewState("Dt"), GridDt)
        EnableHd(GetCountRecord(ViewState("Dt")) = 0)

    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            FillTextBoxDt(GVR.Cells(2).Text)
            ViewState("DtValue") = ddlJobTitle.SelectedValue()
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
            BindToDropList(ddlDepartment, Dt.Rows(0)("Department").ToString)
            BindToText(tbReason, Dt.Rows(0)("Reason").ToString)
            BindToDropList(ddlReqType, Dt.Rows(0)("RequestType").ToString)
            BindToText(tbTotalMale, Dt.Rows(0)("TotalMale").ToString)
            BindToText(tbTotalFemale, Dt.Rows(0)("TotalFemale").ToString)
            BindToText(tbTotal, Dt.Rows(0)("Total").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal KeyDt As String)
        Dim Dr As DataRow()
        'Dim infois, peimen, cas As Double
        Try
            Dr = ViewState("Dt").select("JobTitle = " + QuotedStr(KeyDt))
            If Dr.Length > 0 Then
                'infois = 0
                'peimen = 0
                'cas = 0
                'For Each Dr In ViewState("Dt").Rows
                '    If Not Dr.RowState = DataRowState.Deleted Then
                '        infois = infois + CFloat(Dr("InvoiceHome").ToString)
                '        peimen = peimen + CFloat(Dr("ReceiptHome").ToString)
                '        cas = cas + CFloat(Dr("ChargeHome").ToString)
                '    End If
                'Next
                BindToDropList(ddlJobTitle, Dr(0)("JobTitle").ToString)
                BindToDropList(ddlEmpStatus, Dr(0)("EmpStatus").ToString)
                BindToText(tbJangkaWaktu, Dr(0)("RangeTime").ToString)
                BindToDate(dpStartDate, Dr(0)("StartDate").ToString)
                BindToText(tbMale, Dr(0)("QtyMale").ToString, ViewState("DigitHome"))
                BindToText(tbFemale, Dr(0)("QtyFemale").ToString, ViewState("DigitHome"))
                BindToText(tbTotaldt, Dr(0)("QtyTotal").ToString, ViewState("DigitHome"))
                BindToText(tbSyaratUmum, Dr(0)("SyaratUmum").ToString)
                BindToText(tbSyaratKhusus, Dr(0)("SyaratKhusus").ToString)
                BindToText(tbResponsibility, Dr(0)("Responsibility").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)

                If tbMale.Text = "" Then
                    tbMale.Text = "0"
                End If
                If tbFemale.Text = "" Then
                    tbFemale.Text = "0"
                End If
                If tbTotaldt.Text = "" Then
                    tbTotaldt.Text = "0"
                End If
             

            End If
            'Dt = BindDataTransaction(GetStringDt(tbRef.Text), "ItemNo = " + ItemNo, ViewState("DBConnection").ToString)
            'newTrans()
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
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
    Protected Sub lbDept_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbDepartment.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsDepartment')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb Job Title Error : " + ex.ToString
        End Try
    End Sub
    Private Sub SumTotalMale()
        Dim dr As DataRow
        Dim TotalMale As Decimal = 0
        For Each dr In ViewState("Dt").Rows
            If Not dr.RowState = DataRowState.Deleted Then
                TotalMale += CFloat(dr("QtyMale").ToString)
            End If
        Next
        tbTotalMale.Text = FormatNumber(TotalMale, ViewState("DigitCurr"))
        AttachScript("setformat('-');", Page, Me.GetType())
    End Sub
    Private Sub SumTotalFemale()
        Dim dr As DataRow
        Dim TotalFemale As Decimal = 0
        For Each dr In ViewState("Dt").Rows
            If Not dr.RowState = DataRowState.Deleted Then
                TotalFemale += CFloat(dr("QtyFemale").ToString)
            End If
        Next
        tbTotalFemale.Text = FormatNumber(TotalFemale, ViewState("DigitCurr"))
        AttachScript("setformat('-');", Page, Me.GetType())
    End Sub
    Private Sub SumTotal()
        Dim dr As DataRow
        Dim Total As Decimal = 0
        For Each dr In ViewState("Dt").Rows
            If Not dr.RowState = DataRowState.Deleted Then
                Total += CFloat(dr("QtyTotal").ToString)
            End If
        Next
        tbTotal.Text = FormatNumber(Total, ViewState("DigitCurr"))
        AttachScript("setformat('-');", Page, Me.GetType())
    End Sub

   
   
End Class
