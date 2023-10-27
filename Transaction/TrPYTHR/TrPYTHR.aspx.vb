Imports System.Data
Imports System.Data.SqlClient

Partial Class Transaction_TrPYTHR_TrPYTHR
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select Tahun, ProcessDate, MethodTHR, Method, Status, Variable, UserPrep, DatePrep, UserAppr, DateAppr, Payroll, DumpValue, MethodTHRName, MethodName, EmpNumb, EmpName, UserId From V_PYTHRHd "


    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Load
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
                    tbEmp.Text = Session("Result")(0).ToString
                    tbempName.Text = Session("Result")(1).ToString
                    ddlJobTitle.SelectedValue = Session("Result")(2).ToString
                    tbHireDate.SelectedValue = Session("Result")(3).ToString
                    'GetPrevAmount(ddlYear.SelectedValue, ddlPeriod.SelectedValue, tbAccount.Text)
                End If
                Session("Result") = Nothing
                ViewState("Sender") = Nothing
            End If
        Catch ex As Exception
            lbStatus.Text = "Form Load Error : " + ex.ToString
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
            FillCombo(ddlYear, "EXEC S_GetYear", False, "Year", "Year", ViewState("DBConnection"))
            FillCombo(ddlMethod, "EXEC S_GetMethodUser " + QuotedStr(Viewstate("UserId").ToString), False, "MethodCode", "MethodName", ViewState("DBConnection"))
            FillCombo(ddlMethodTHR, "EXEC S_GetMethodTHR", False, "MethodTHR_Code", "MethodTHR_Code", ViewState("DBConnection"))
            FillCombo(ddlJobTitle, "EXEC S_GetJobTitle", True, "JobTtlCode", "JobTtlName", ViewState("DBConnection"))
            FillCombo(ddlCurr, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
            FillCombo(ddlM, "EXEC S_GetMethodUser" + QuotedStr(ViewState("UserId").ToString), False, "MethodCode", "MethodName", ViewState("DBConnection"))
            ViewState("SortExpression") = Nothing
            ViewState("DigitCurr") = 0
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
            If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then

            End If

            tbPaid.Attributes.Add("OnKeyDown", "return PressNumeric();")

            tbTotalGP.Attributes.Add("OnBlur", "setformatdt();")
            'tbTotalTT.Attributes.Add("OnBlur", "setformatdt();")
            'tbMasa.Attributes.Add("OnBlur", "setformatdt();")

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
            DT = BindDataTransaction(GetStringHd, StrFilter + "UserId =" + QuotedStr(ViewState("UserId")), ViewState("DBConnection").ToString)
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
                ViewState("SortExpression") = "Tahun DESC"
            End If
            DV.Sort = ViewState("SortExpression")
            GridView1.DataSource = DV
            GridView1.DataBind()
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

    Private Function GetStringDt(ByVal Year As String, ByVal ProcessDate As String, ByVal Method As String, ByVal MethodTHR As String) As String
        Return "SELECT * From V_PYTHRDt WHERE Tahun = " + Year + " AND ProcessDate =" + QuotedStr(ProcessDate) + " AND Method =" + QuotedStr(Method) + " AND MethodTHR =" + QuotedStr(MethodTHR)
    End Function

    Private Sub EnableHd(ByVal State As Boolean)
        Try
            ddlYear.Enabled = State
            tbProcessDate.Enabled = State
            ddlMethod.Enabled = State
            ddlMethodTHR.Enabled = State
            btnGetData.Enabled = State
        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub BindDataDt(ByVal Year As String, ByVal ProcessDate As String, ByVal Method As String, ByVal MethodTHR As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt") = Nothing
            dt = SQLExecuteQuery(GetStringDt(Year, ProcessDate, Method, MethodTHR), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt") = dt
            BindGridDtExtended()
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
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

    Private Function AllowedRecord() As Integer
        Try
            If ViewState("Employee") = tbEmp.Text Then
                Return 1
            Else
                Return 0
            End If
        Catch ex As Exception
            Throw New Exception("Allowed Record Error : " + ex.ToString)
        End Try
    End Function


    Private Sub SaveAll()
        Dim SQLString, CekTrans As String
        Dim I As Integer
        Try
            If pnlEditDt.Visible = True Then
                lbStatus.Text = MessageDlg("Detail Data must be saved first")
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
                'tbPFI.Text = GetAutoNmbr("SF", "N", CInt(Session("GLYear")), CInt(Session("GLPeriod")), "", Session("DBConnection").ToString)

                CekTrans = SQLExecuteScalar("SELECT COUNT(Tahun) FROM PYTHRHd WHERE Tahun = " + ddlYear.SelectedValue + " AND ProcessDate = " + QuotedStr(Format(tbProcessDate.SelectedValue, "yyyy-MM-dd")) + " And Method = " + QuotedStr(ddlMethod.SelectedValue) + " And MethodTHR = " + QuotedStr(ddlMethodTHR.SelectedValue), ViewState("DBConnection").ToString)
                If CekTrans <> "0" Then
                    lbStatus.Text = MessageDlg("THR for " + ddlYear.SelectedValue + " and ProcessDate " + Format(tbProcessDate.SelectedValue, "yyyy-MM-dd") + " and Method " + ddlMethod.SelectedValue + " and Method THR " + ddlMethodTHR.SelectedValue + " exist, cannot save data")
                    Exit Sub
                End If

                SQLString = "INSERT INTO PYTHRHd (Tahun, ProcessDate, Method, MethodTHR, Status, Variable, UserPrep, DatePrep) " + _
                "SELECT " + ddlYear.SelectedValue + "," + QuotedStr(Format(tbProcessDate.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(ddlMethod.SelectedValue) + ", " + QuotedStr(ddlMethodTHR.SelectedValue) + ", 'H'," + _
                QuotedStr(tbVariable.Text) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", getDate()"

                ViewState("Year") = ddlYear.SelectedValue
                ViewState("ProcessDate") = Format(tbProcessDate.SelectedValue, "yyyy-MM-dd")
                ViewState("Method") = ddlMethod.SelectedValue
                ViewState("MethodTHR") = ddlMethodTHR.SelectedValue
            Else
                SQLString = "UPDATE PYTHRHd SET Variable = " + QuotedStr(tbVariable.Text) + _
                ", ProcessDate = " + QuotedStr(Format(tbProcessDate.SelectedValue, "yyyy-MM-dd")) + _
                ", Method = " + QuotedStr(ddlMethod.SelectedValue) + _
                ", MethodTHR = " + QuotedStr(ddlMethodTHR.SelectedValue) + _
                ", DatePrep = getDate()" + _
                " WHERE Tahun = " + ddlYear.SelectedValue + " AND ProcessDate = " + QuotedStr(Format(tbProcessDate.SelectedValue, "yyyy-MM-dd")) + " And Method = " + QuotedStr(ddlMethod.SelectedValue) + " And MethodTHR = " + QuotedStr(ddlMethodTHR.SelectedValue)
            End If
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            'update Primary Key on Dt
            Dim Row As DataRow()

            Row = ViewState("Dt").Select("Tahun IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("Tahun") = ddlYear.SelectedValue
                Row(I)("ProcessDate") = ViewState("ProcessDate")
                Row(I)("Method") = ddlMethod.SelectedValue
                Row(I)("MethodTHR") = ddlMethodTHR.SelectedValue
                Row(I).EndEdit()
            Next

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT Tahun, ProcessDate, Method, MethodTHR, EmpNumb, Currency, TotalGP, TotalTT, MasaKerja, TotalTHR, TotalPaid, Formula, xPeriod, ProcessCode" + _
                                         " FROM PYTHRDt WHERE Tahun = " & ViewState("Year") & " AND ProcessDate = '" & ViewState("ProcessDate") & "' AND Method = '" & ViewState("Method") & "' AND MethodTHR = " & QuotedStr(ViewState("MethodTHR")), con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt As New DataTable("PYTHRDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt

        Catch ex As Exception
            Throw New Exception("Save All Data Error : " + ex.ToString)
        End Try
    End Sub


    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ViewState("DigitCurr") = 0
            ViewState("Year") = Now.Year.ToString
            ViewState("ProcessDate") = Format(tbProcessDate.SelectedValue, "yyyy-MM-dd")
            ViewState("Method") = ""
            ViewState("MethodTHR") = ""
            ClearHd()
            Cleardt()
            pnlDt.Visible = True
            BindDataDt("1", Date.Today, "", "")

        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            BindToDropList(ddlYear, Now.Year.ToString)
            tbVariable.Text = "0"
	    tbProcessDate.SelectedDate =ViewState("ServerDate")
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbEmp.Text = ""
            tbempName.Text = ""
            ddlCurr.SelectedValue = ViewState("Currency")
            tbTotalGP.Text = "0"
            tbTotalTT.Text = "0"
            tbMasa.Text = "0"
            tbadjust.Text = "0"
            tbFormula.Text = "0"
            tbPaid.Text = "0"
            tbLastSalary.Text = "0"
            tbTHR.Text = "0"
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub


    Function CekHd() As Boolean
        Try
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Hd Error : " + ex.ToString)
        End Try
    End Function
    Function CekDt(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not (Dr Is Nothing) Then
                'If tbEmp.Text.Trim = "" Then
                '    lbStatus.Text = MessageDlg("Employee No Must Have Value")
                '    tbEmp.Focus()
                '    Return False
                'End If

            Else
                If tbEmp.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Employee No Must Have Value")
                    tbEmp.Focus()
                    Return False
                End If

            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function



    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            CheckAll(GridView1, sender)
        Catch ex As Exception
            lbStatus.Text = "cb Select Checked Changed Error : " + ex.ToString
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
            If e.CommandName = "Go" Then
                index = Convert.ToInt32(e.CommandArgument)
                GVR = GridView1.Rows(index)
                DDL = GridView1.Rows(index).FindControl("ddl")
                If DDL.SelectedValue = "View" Then
                    MovePanel(PnlHd, pnlInput)
                    pnlDt.Visible = True
                    ViewState("Year") = GVR.Cells(2).Text
                    ViewState("ProcessDate") = GVR.Cells(3).Text
                    ViewState("MethodTHR") = GVR.Cells(4).Text
                    ViewState("Method") = GVR.Cells(6).Text
                    GridDt.PageIndex = 0
                    FillTextBoxHd(ViewState("Year"), ViewState("ProcessDate"), ViewState("Method"), ViewState("MethodTHR"))
                    BindDataDt(ViewState("Year"), ViewState("ProcessDate"), ViewState("Method"), ViewState("MethodTHR"))
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, pnlDt, GridDt)
                    btnHome.Visible = True
                    btnGetData.Visible = False
                ElseIf DDL.SelectedValue = "Print" Then
                    Try
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        ViewState("Year") = GVR.Cells(2).Text
                        ViewState("ProcessDate") = GVR.Cells(3).Text
                        ViewState("MethodTHR") = GVR.Cells(4).Text
                        ViewState("Method") = GVR.Cells(6).Text
                        Session("SelectCommand") = "EXEC S_PYFormTHR " + QuotedStr(ViewState("Year") + "," + QuotedStr(ViewState("MethodTHR")) + "," + QuotedStr(ViewState("Method")) + "," + QuotedStr(ViewState("ProcessDate")))
                        Session("ReportFile") = ".../../../Rpt/FormPYTHR.frx"
                        Session("DBConnection") = ViewState("DBConnection")
                        AttachScript("openprintdlg();", Page, Me.GetType)
                    Catch ex As Exception
                        lbStatus.Text = "btn print Error = " + ex.ToString
                    End Try
                Elseif DDL.SelectedValue = "Edit" Then
                    If GVR.Cells(8).Text = "H" Or GVR.Cells(8).Text = "G" Then
                        CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        MovePanel(PnlHd, pnlInput)
                        pnlDt.Visible = True
                        ViewState("Year") = GVR.Cells(2).Text
                        ViewState("ProcessDate") = GVR.Cells(3).Text
                        ViewState("MethodTHR") = GVR.Cells(4).Text
                        ViewState("Method") = GVR.Cells(6).Text
                        GridDt.PageIndex = 0
                        BindDataDt(ViewState("Year"), ViewState("ProcessDate"), ViewState("Method"), ViewState("MethodTHR"))
                        FillTextBoxHd(ViewState("Year"), ViewState("ProcessDate"), ViewState("Method"), ViewState("MethodTHR"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        btnHome.Visible = False
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                        btnGetData.Visible = True
                    Else
                        lbStatus.Text = MessageDlg("Data must be Hold or Get Approval to edit")
                        Exit Sub
                    End If

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
            ViewState("StateDt") = "Edit"
            ViewState("Employee") = GVR.Cells(1).Text
            FillTextBoxDt(GVR.Cells(1).Text)
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            tbEmp.Focus()
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub FillTextBoxHd(ByVal Tahun As String, ByVal ProcessDate As String, ByVal Method As String, ByVal MethodTHR As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "Tahun = " + Tahun + " AND ProcessDate = " + QuotedStr(ProcessDate) + " AND Method = " + QuotedStr(Method) + " AND MethodTHR = " + QuotedStr(MethodTHR), ViewState("DBConnection").ToString)
            ddlYear.SelectedValue = Tahun
            BindToDate(tbProcessDate, Dt.Rows(0)("ProcessDate").ToString)
            BindToDropList(ddlMethod, Dt.Rows(0)("MethodName").ToString)
            BindToDropList(ddlMethodTHR, Dt.Rows(0)("MethodTHRName").ToString)
            BindToText(tbVariable, Dt.Rows(0)("Variable").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxDt(ByVal PK As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("EmpNumb = " + QuotedStr(PK))

            If Dr.Length > 0 Then
                BindToText(tbEmp, Dr(0)("EmpNumb").ToString)
                BindToText(tbempName, Dr(0)("EmpName").ToString)
                BindToDropList(ddlCurr, Dr(0)("Currency").ToString)
                BindToDropList(ddlJobTitle, Dr(0)("JobTitle").ToString)
                BindToDate(tbHireDate, Dr(0)("HireDate").ToString)
                BindToDropList(ddlM, Dr(0)("Method").ToString)
                BindToText(tbTotalGP, Dr(0)("TotalGP").ToString)
                BindToText(tbTotalTT, Dr(0)("TotalTT").ToString)
                BindToText(tbMasa, Dr(0)("MasaKerja").ToString)
                BindToText(tbFormula, Dr(0)("Formula").ToString)
                BindToText(tbTHR, Dr(0)("TotalTHR").ToString)
                BindToText(tbPaid, Dr(0)("TotalPaid").ToString)
                BindToText(tbXperiod, Dr(0)("Xperiod").ToString)
                BindToText(tbLastSalary, Dr(0)("ProcessCode").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbEmp_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbEmp.TextChanged
        Dim Dr As DataRow
        Dim DS As DataSet
	Dim SqlString As String
        Try
		SqlString ="Select A.EmpNumb, A.EmpName, A.JobTitle, A.HireDate from MsEmployee A  " + _
			" INNER JOIN V_MsMethodAccess B ON A.EmpNumb = B.EmpNumb WHERE A.EmpNumb = " + QuotedStr(tbEmp.Text) + " AND B.UserId = " + QuotedStr(ViewState("UserId").ToString)
            DS = SQLExecuteQuery(SqlString , ViewState("DBConnection"))
            Dr = DS.Tables(0).Rows(0)
            If Not Dr Is Nothing Then
                tbEmp.Text = Dr("EmpNumb")
                tbempName.Text = Dr("EmpName")
                ddlJobTitle.SelectedValue = Dr("JobTitle")
                tbHireDate.SelectedDate = Dr("HireDate")
            Else
                tbEmp.Text = ""
                tbempName.Text = ""
                'ddlJobTitle.SelectedIndex = 0
		'tbHireDate.Clear

            End If
        Catch ex As Exception
            Throw New Exception("tb Employee change Error : " + ex.ToString)
        End Try
    End Sub


    Private Sub BindGridDtExtended()
        Try
            BindGridDt(ViewState("Dt"), GridDt)
            'If GetCountRecord(ViewState("Dt")) > 0 Then
            '    GridDt.Columns(1).Visible = True
            'Else
            '    GridDt.Columns(1).Visible = False
            'End If
        Catch ex As Exception
            Throw New Exception("BindGridDtExtended Error : " + ex.ToString)
        End Try
    End Sub

    Private Function CheckCurrentYear() As Boolean
        Dim result As String
        Try
            result = SQLExecuteScalar("Select Count(Tahun) from PYTHRHd where Tahun = " + ddlYear.SelectedValue + " and ProcessDate = " + Format(tbProcessDate.SelectedValue, "yyyy-MM-dd"), ViewState("DBConnection"))
            lbStatus.Text = result
            If result <> "0" Then
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            Throw New Exception("Cek Current Year Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            MovePanel(PnlHd, pnlInput)
            pnlDt.Visible = True
            ModifyInput2(True, pnlInput, pnlDt, GridDt)
            newTrans()
            btnHome.Visible = False
            btnGetData.Visible = True
            ddlYear.Focus()
            EnableHd(True)
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
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
            '3 = status, 2 & 3 = key, 
            GetListCommand(Status, GridView1, "8,2,4,6,3", ListSelectNmbr, Nmbr, lbStatus.Text)

            If ListSelectNmbr = "" Then Exit Sub
            For j = 0 To (Nmbr.Length - 1)
                If Nmbr(j) = "" Then
                    Exit For
                Else

                    Result = ExecSPCommandGo(ActionValue, "S_PYTHR", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)

                    If Trim(Result) <> "" Then
                        lbStatus.Text = lbStatus.Text + Result + " <br/>"
                    End If
                End If
            Next
            BindData("LTRIM(STR(Tahun))+'|'+LTRIM(STR(MethodTHR))+'|'+Method+'|'+ProcessDate in (" + ListSelectNmbr + ")")
        Catch ex As Exception
            lbStatus.Text = "BtnGo_Click Error : " + ex.ToString
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

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            GridView1.PageIndex = 0
            Session("AdvanceFilter") = ""
            BindData(Session("AdvanceFilter"))
            pnlNav.Visible = True
        Catch ex As Exception
            lbStatus.Text = "Btn Search Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnGetData_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetData.Click
        Dim DR, CurDr As DataRow
        Dim ds As DataSet
        Dim dt As DataTable
        Try
            If GetCountRecord(ViewState("Dt")) > 0 Then
                lbStatus.Text = MessageDlg("Data not empty")
                Exit Sub
            End If
            If CheckCurrentYear() = False Then
                lbStatus.Text = MessageDlg("Data for year " + ddlYear.SelectedValue + " and ProcessDate = " + Format(tbProcessDate.SelectedValue, "yyyy-MM-dd") + " exist")
                Exit Sub
            End If

            ds = SQLExecuteQuery("EXEC S_PYTHRGetDt " + ddlYear.SelectedValue + "," + QuotedStr(Format(tbProcessDate.SelectedValue, "yyyy-MM-dd")) + ", " + tbVariable.Text + ", " + QuotedStr(ddlMethod.SelectedValue) + ", " + QuotedStr(ddlMethodTHR.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString), ViewState("DBConnection").ToString)

            dt = ds.Tables(0)

            If dt.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("No Data for Last Period")
                Exit Sub
            End If

            For Each CurDr In dt.Rows
                DR = ViewState("Dt").NewRow
                DR("EmpNumb") = CurDr("EmpNumb")
                DR("EmpName") = CurDr("EmpName")
                DR("HireDate") = CurDr("HireDate")
                DR("JobTitle") = CurDr("JobTitle")
                DR("Currency") = CurDr("Currency")
                DR("TotalGP") = FormatFloat(CurDr("TotalGP"), ViewState("DigitCurr"))
                DR("TotalTT") = FormatFloat(CurDr("TotalTT"), ViewState("DigitCurr"))
                DR("TotalTHR") = FormatFloat(CurDr("TotalTHR"), ViewState("DigitCurr"))
                DR("TotalPaid") = FormatFloat(CurDr("TotalPaid"), ViewState("DigitCurr"))
                DR("MasaKerja") = FormatFloat(CurDr("MasaKerja"), ViewState("DigitCurr"))
                DR("Formula") = CurDr("Formula")
                DR("ProcessCode") = CurDr("ProcessCode")
                DR("xPeriod") = CurDr("xPeriod")
                DR("Method") = CurDr("Method")
                ViewState("Dt").Rows.Add(DR)
            Next
            BindGridDtExtended()

        Catch ex As Exception
            lbStatus.Text = "Btn Get Data Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAdddt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdddt.Click, btnAddDt2.Click
        Try
            Cleardt()

            If CekHd() = False Then
                Exit Sub
            End If
            If CheckCurrentYear() = False And ViewState("StateHd") = "Insert" Then
                lbStatus.Text = MessageDlg("Data for year " + ddlYear.SelectedValue + " and ProcessDate " + Format(tbProcessDate.SelectedValue, "yyyy-MM-dd") + " exist")
                Exit Sub
            End If
            ViewState("StateDt") = "Insert"
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            StatusButtonSave(False)
            tbEmp.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt error : " + ex.ToString
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

            SaveAll()
            newTrans()
        Catch ex As Exception
            lbStatus.Text = "btn save all error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt.Click
        Try
            Dim ExistRow As DataRow()
            ExistRow = ViewState("Dt").Select("EmpNumb = " + QuotedStr(tbEmp.Text))

            If ViewState("StateDt") = "Edit" Then
                Dim Row As DataRow

                'If ExistRow.Count > AllowedRecord() Then
                '    lbStatus.Text = MessageDlg("Data Exist, Cannot Edit Data")
                '    Exit Sub
                'End If

                Row = ViewState("Dt").Select("EmpNumb = " + QuotedStr(ViewState("Employee")))(0)

                If CekDt() = False Then
                    btnSaveDt.Focus()
                    Exit Sub
                End If

                Row.BeginEdit()
                Row("EmpNumb") = tbEmp.Text
                Row("EmpName") = tbempName.Text
                Row("JobTitle") = ddlJobTitle.SelectedValue
                Row("HireDate") = Format(tbProcessDate.SelectedValue, "yyyy-MM-dd")
                Row("TotalGP") = tbTotalGP.Text
                Row("TotalTT") = tbTotalTT.Text
                Row("MasaKerja") = tbMasa.Text
                Row("TotalPaid") = FormatFloat(tbPaid.Text, ViewState("DigitQty"))
                Row("TotalAdjust") = FormatFloat(tbadjust.Text, ViewState("DigitQty"))
                Row("Formula") = FormatFloat(tbFormula.Text, ViewState("DigitQty"))
                Row.EndEdit()
                ViewState("Employee") = Nothing
            Else
                'Insert
                If CekDt() = False Then
                    btnSaveDt.Focus()
                    Exit Sub
                End If

                Dim dr As DataRow
                If ExistRow.Count > 0 Then
                    lbStatus.Text = MessageDlg("Data Exist, Cannot Insert Data")
                    Exit Sub
                End If

                dr = ViewState("Dt").NewRow
                dr("EmpNumb") = tbEmp.Text
                dr("EmpName") = tbempName.Text
                dr("JobTitle") = ddlJobTitle.SelectedValue
                dr("HireDate") = Format(tbProcessDate.SelectedValue, "yyyy-MM-dd")
                dr("TotalGP") = tbTotalGP.Text
                dr("TotalTT") = tbTotalTT.Text
                dr("MasaKerja") = tbMasa.Text
                dr("TotalPaid") = FormatFloat(tbPaid.Text, ViewState("DigitQty"))
                dr("TotalAdjust") = FormatFloat(tbadjust.Text, ViewState("DigitQty"))
                dr("Formula") = FormatFloat(tbFormula.Text, ViewState("DigitQty"))

                ViewState("Dt").Rows.Add(dr)

            End If
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            BindGridDtExtended()
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save Dt Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
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
            If lbStatus.Text.Length > 0 Then Exit Sub
            MovePanel(pnlInput, PnlHd)

            CurrFilter = tbFilter.Text
            Value = ddlField.SelectedValue
            tbFilter.Text = ddlYear.SelectedValue
            ddlField.SelectedValue = "Tahun"
            btnSearch_Click(Nothing, Nothing)
            tbFilter.Text = CurrFilter
            ddlField.SelectedValue = Value
        Catch ex As Exception
            lbStatus.Text = "btnSaveTrans Error : " + ex.ToString
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

    Protected Sub btnHome_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHome.Click
        Try
            MovePanel(pnlInput, PnlHd)
        Catch ex As Exception
            lbStatus.Text = "btn back Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Try
            MovePanel(pnlInput, PnlHd)
        Catch ex As Exception
            lbStatus.Text = "btn back Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnEmp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEmp.Click
        Dim ResultField As String
        Try
            Session("filter") = "Select A.EmpNumb, A.EmpName, A.HireDate, A.JobTitle from MsEmployee A "  + _
				" INNER JOIN V_MsMethodAccess B ON A.EmpNumb = B.EmpNumb Where B.UserId = " + QuotedStr(ViewState("UserId").ToString)
            Session("DBConnection") = ViewState("DBConnection")
            ResultField = "EmpNumb, EmpName, JobTitle, HireDate"
            ViewState("Sender") = "btnEmp"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Factory Click Error : " + ex.ToString
        End Try
    End Sub
    'Protected Sub GetPrevAmount(ByVal Year As String, ByVal Period As String, ByVal Account As String)
    '    Dim DR, CurDr As DataRow
    '    Dim ds As DataSet
    '    Dim dt As DataTable
    '    Try

    '        ds = SQLExecuteQuery("EXEC S_PYTHRGetPrevAmount " + Year + "," + Period + "," + QuotedStr(Account), ViewState("DBConnection").ToString)

    '        dt = ds.Tables(0)

    '        If dt.Rows.Count = 0 Then
    '            lbStatus.Text = MessageDlg("No Data for Last Period")
    '            Exit Sub
    '        End If

    '        For Each CurDr In dt.Rows
    '            tbTotalGP.Text = CurDr("AmountPrev")
    '            tbTotalTT.Text = CurDr("ActualPrev")
    '        Next

    '    Catch ex As Exception
    '        lbStatus.Text = "GetPrevAmount Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub tbPaid_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPaid.TextChanged
        Try
            tbadjust.Text = FormatFloat(tbTHR.Text - tbPaid.Text, ViewState("DigitQty"))
            btnSaveDt.Focus()
        Catch ex As Exception
            lbStatus.Text = "tbPaid_TextChanged Error : " + ex.ToString
        End Try
    End Sub
End Class
