Imports System.Data
Imports System.Data.SqlClient
'Imports System.Data.Odbc
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc

Partial Class Transaction_TrPYPPHYear_TrPYPPHYear
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "SELECT * FROM V_PYPPHYearHd"
    Protected GetStringDt1 As String = "SELECT * FROM V_PYPPHYearDt"

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

                If ViewState("Sender") = "btnGetData" Then
                    'Dim drResult As DataRow
                    'If IsNothing(Session("Result")) Then
                    '    lbStatus.Text = "Session is empty"
                    '    Exit Sub
                    'End If
                    'For Each drResult In Session("Result").Rows
                    '    If Not CekExistData(ViewState("Dt"), "Machine,MaintenanceItem,Job", drResult("Machine") + "|" + drResult("MaintenanceItem") + "|" + drResult("Job")) Then
                    '        'insert
                    '        Dim dr As DataRow
                    '        dr = ViewState("Dt").NewRow
                    '        dr("Machine") = drResult("Machine")
                    '        dr("MachineName") = drResult("MachineName")
                    '        dr("MaintenanceItem") = drResult("MaintenanceItem")
                    '        dr("MaintenanceItemName") = drResult("MaintenanceItemName")
                    '        dr("Job") = drResult("Job")
                    '        dr("JobName") = drResult("JobName")
                    '        dr("LastMaintenance") = drResult("LastSchedule")
                    '        dr("ScheduleDate") = drResult("NewSchedule")
                    '        dr("Remark") = ""
                    '        ViewState("Dt").Rows.Add(dr)
                    '    End If
                    'Next
                    'BindGridDt(ViewState("Dt"), GridDt)
                    'EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    'Session("ResultSame") = Nothing
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
        FillCombo(ddlYear, "EXEC S_PYPPhYearGetYear ''", False, "Year", "Year", ViewState("DBConnection"))
        ViewState("SortExpression") = Nothing
        ViewState("DigitCurr") = 0
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
        If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
            'ddlCommand.Items.Add("Print")
            'ddlCommand2.Items.Add("Print")
        End If
        'tbMachineName.Attributes.Add("ReadOnly", "True")
        'tbAdjPercent.Attributes.Add("OnKeyDown", "return PressNumeric();")
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
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "Year DESC"
            End If
            DV.Sort = ViewState("SortExpression")
            GridView1.DataSource = DV
            GridView1.DataBind()
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT * FROM V_PYPPHYearDt WHERE Year = " + Nmbr
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
                Session("SelectCommand") = "EXEC S_MTNFormSchedule " + Result
                Session("ReportFile") = ".../../../Rpt/FormMTNSchedule.frx"
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
                        Result = ExecSPCommandGo(ActionValue, "S_PYPPhYear", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                        If Trim(Result) <> "" Then
                            lbStatus.Text = lbStatus.Text + Result + " <br/>"

                        End If
                    End If
                Next
                BindData("LTRIM(STR(Year)) in (" + ListSelectNmbr + ")")
            End If
        Catch ex As Exception
            lbStatus.Text = "Go Command Error : " + ex.ToString
        End Try
    End Sub

    Private Sub EnableHd(ByVal State As Boolean)
        Try
            ddlYear.Enabled = State
            btnGetData.Enabled = State
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
            'BindGridDt(dt, GridDt)
            BindGridDtExtended()
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
                'tbRef.Text = GetAutoNmbr("MTSC", "N", CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), "", ViewState("DBConnection").ToString)
                SQLString = "INSERT INTO PYPPHYearHd(Year, Status, UserPrep, DatePrep) " + _
                "SELECT " + ddlYear.SelectedValue + ", 'H', " + QuotedStr(ViewState("UserId").ToString) + ", GetDate()"
                ViewState("Reference") = ddlYear.SelectedValue
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("SELECT Status FROM PYPPHYearHd WHERE Year = " + ddlYear.SelectedValue, ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE PYPPHYearHd SET UserPrep = " + QuotedStr(ViewState("UserId").ToString) + ", DatePrep = GetDate() " + _
                " WHERE Year = " + ddlYear.SelectedValue
            End If
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            'update Primary Key on Dt
            Dim Row As DataRow()

            Row = ViewState("Dt").Select("Year IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("Year") = ddlYear.SelectedValue
                Row(I).EndEdit()
            Next

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString 'System.Configuration.ConfigurationManager.AppSettings.Get("ConString")
            con = New SqlConnection(ConnString)
            con.Open()

            Dim SQLString2 As String
            SQLString2 = "SELECT Year, EmpNumb, StartDate, EndDate, CurrCode, YearBruto, YearPremi, YearIuranJbt, YearNetto, YearPTKP, YearPKP, YearPPH, TotalBruto, TotalPremi, TotalTHR, TotalNetto, TotalInsentif, TotalPPH, PPHHasPaid, PPHAdjust, TotalPesangon, MaritalTax, PPHPesangon, TotalIuranJbt FROM PYPPHYearDt WHERE Year = " & ViewState("Reference")

            Dim cmdSql As New SqlCommand(SQLString2, con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
                    "UPDATE PYPPhYearDt SET TotalTHR = @TotalTHR, TotalInsentif = @TotalInsentif, PPHPesangon = @PPHPesangon, TotalIuranJbt = @TotalIuranJbt WHERE Year = " & ddlYear.SelectedValue & " AND EmpNumb = @OldEmpNumb", con)
            ' Define output parameters.
            Update_Command.Parameters.Add("@TotalTHR", SqlDbType.Float, 20, "TotalTHR")
            Update_Command.Parameters.Add("@TotalInsentif", SqlDbType.Float, 20, "TotalInsentif")
            Update_Command.Parameters.Add("@PPHPesangon", SqlDbType.Float, 1, "PPHPesangon")
            Update_Command.Parameters.Add("@TotalIuranJbt", SqlDbType.Float, 18, "TotalIuranJbt")
            ' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@OldEmpNumb", SqlDbType.VarChar, 20, "EmpNumb")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            '' Create the DeleteCommand.
            'Dim Delete_Command = New SqlCommand( _
            '    "DELETE FROM STCAdjustDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND OpnameNo = '' AND Product = @Product AND Location = @Location", ViewState("DBConnection"))
            '' Add the parameters for the DeleteCommand.
            'param = Delete_Command.Parameters.Add("@Product", SqlDbType.VarChar, 20, "Product")
            'param.SourceVersion = DataRowVersion.Original
            'param = Delete_Command.Parameters.Add("@Location", SqlDbType.VarChar, 20, "Location")
            'param.SourceVersion = DataRowVersion.Original
            'da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("PYPPhYearDt")

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
            If lbStatus.Text.Length > 0 Then Exit Sub
            MovePanel(pnlInput, PnlHd)
            CurrFilter = tbFilter.Text
            Value = ddlField.SelectedValue
            tbFilter.Text = ddlYear.SelectedValue
            ddlField.SelectedValue = "Year"
            btnSearch_Click(Nothing, Nothing)
            tbFilter.Text = CurrFilter
            ddlField.SelectedValue = Value
        Catch ex As Exception
            lbStatus.Text = "Save All Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, pnlDt, GridDt)
            newTrans()
            EnableHd(True)
            btnHome.Visible = False
            ddlYear.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ViewState("Add") = "Clear"
            ViewState("DigitCurr") = 0
            ClearHd()
            Cleardt()
            pnlDt.Visible = True
            BindDataDt("1")
            FillCombo(ddlYear, "EXEC S_PYPPhYearGetYear ' AND FgUsed = ''N'''", False, "Year", "Year", ViewState("DBConnection"))
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            'ddlYear.SelectedValue = ViewState("GLYear")            
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            lbPPHAdjust.Text = "0"
            LbPPHHAsPaid.Text = "0"
            LbTotalPPH.Text = "0"
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
            ViewState("StateHd") = "Insert"
        Catch ex As Exception
            lbStatus.Text = "btn save all error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt.Click, btnAddDt2.Click
        'Cleardt()
        'If CekHd() = False Then
        '    Exit Sub
        'End If
        'ViewState("StateDt") = "Insert"
        'MovePanel(pnlDt, pnlEditDt)
        'EnableHd(False)
        'StatusButtonSave(False)
        'tbMachineCode.Focus()
    End Sub

    Function CekHd() As Boolean
        Try
            If ddlYear.SelectedValue.Trim = "" Then
                lbStatus.Text = MessageDlg("Year must have value")
                ddlYear.Focus()
                Return False
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Hd Error : " + ex.ToString)
        End Try
    End Function

    Function CekDt(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            'If Not (Dr Is Nothing) Then
            '    If Dr.RowState = DataRowState.Deleted Then
            '        Return True
            '    End If
            '    If Dr("Machine").ToString.Trim = "" Then
            '        lbStatus.Text = MessageDlg("Machine Must Have Value")
            '        Return False
            '    End If
            '    If Dr("MaintenanceItem").ToString.Trim = "" Then
            '        lbStatus.Text = MessageDlg("Maintenance Item Must Have Value")
            '        Return False
            '    End If
            '    If Dr("Job").ToString.Trim = "" Then
            '        lbStatus.Text = MessageDlg("Job Must Have Value")
            '        Return False
            '    End If
            '    If Dr("ScheduleDate").ToString = "" Then
            '        lbStatus.Text = MessageDlg("Schedule Date Must Have Value")
            '        Return False
            '    End If
            'Else
            '    If tbMachineCode.ToString.Trim = "" Then
            '        lbStatus.Text = MessageDlg("Machine Must Have Value")
            '        tbMachineCode.Focus()
            '        Return False
            '    End If
            '    If ddlMaintenanceItem.SelectedValue = "" Then
            '        lbStatus.Text = MessageDlg("Maintenance Item Must Have Value")
            '        ddlMaintenanceItem.Focus()
            '        Return False
            '    End If
            '    If ddlJob.SelectedValue.Trim = "" Then
            '        lbStatus.Text = MessageDlg("Job Must Have Value")
            '        ddlJob.Focus()
            '        Return False
            '    End If
            '    If Not tbLastMaintenance.SelectedDate = Nothing Then
            '        If tbScheduleDate.SelectedDate <= tbLastMaintenance.SelectedDate Then
            '            lbStatus.Text = MessageDlg("Schedule Date Must be greater than Last Maintenance")
            '            tbScheduleDate.Focus()
            '            Return False
            '        End If
            '    End If
            '    If tbScheduleDate.SelectedDate = Nothing Then
            '        lbStatus.Text = MessageDlg("Schedule Date Must Have Value")
            '        tbScheduleDate.Focus()
            '        Return False
            '    End If
            'End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        CheckAll(GridView1, sender)
    End Sub

    Protected Sub LbAdvSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LbAdvSearch.Click
        'Dim FDateName, FDateValue, FilterName, FilterValue As String
        'Try
        '    FDateName = "Date"
        '    FDateValue = "TransDate"
        '    FilterName = "Reference, Quotation No, Start Effective Date, Customer Code, Customer Name, Currency, Price Include Tax, Remark"
        '    FilterValue = "Reference, QuotationNo, dbo.FormatDate(StartEffective), Customer, Customer_Name, Currency, PriceIncludeTax, Remark"
        '    Session("DateFieldName") = FDateName.Split(",")
        '    Session("DateFieldValue") = FDateValue.Split(",")
        '    Session("FieldName") = FilterName.Split(",")
        '    Session("FieldValue") = FilterValue.Split(",")
        '    AttachScript("OpenFilterCriteria();", Page, Me.GetType())
        'Catch ex As Exception
        '    lbStatus.Text = "Advanced Search Click Error : " + ex.ToString
        'End Try
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
                    FillCombo(ddlYear, "EXEC S_PYPPhYearGetYear ''", False, "Year", "Year", ViewState("DBConnection"))
                    FillTextBoxHd(ViewState("Reference"))
                    BindDataDt(ViewState("Reference"))
                    GridDt.Columns(0).Visible = False
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, pnlDt, GridDt)
                    'Panel1.Visible = False
                    btnHome.Visible = True
                ElseIf DDL.SelectedValue = "Edit" Then
                    If GVR.Cells(3).Text = "H" Or GVR.Cells(3).Text = "G" Then
                        CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        MovePanel(PnlHd, pnlInput)
                        'Panel1.Visible = True
                        ViewState("Reference") = GVR.Cells(2).Text
                        GridDt.PageIndex = 0
                        BindDataDt(ViewState("Reference"))
                        FillCombo(ddlYear, "EXEC S_PYPPhYearGetYear ''", False, "Year", "Year", ViewState("DBConnection"))
                        FillTextBoxHd(ViewState("Reference"))
                        GridDt.Columns(0).Visible = True
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
                        Session("DBCOnnection") = ViewState("DBConnection")
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        Session("SelectCommand") = "EXEC S_PYFormPPHYear " + QuotedStr(GVR.Cells(2).Text)
                        Session("ReportFile") = ".../../../Rpt/FormPPHYear.frx"
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

    Protected Sub GridView1_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowCreated

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

    Protected Sub GridDt_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles GridDt.RowCancelingEdit
        Try
            GridDt.EditIndex = -1
            BindGridDt(ViewState("Dt"), GridDt)
        Catch ex As Exception
            lbStatus.Text = "datagrid row canceling edit Error : " + ex.ToString
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
        Dim txtID As Label
        Try
            Dim dr() As DataRow
            'Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)

            txtID = GridDt.Rows(e.RowIndex).FindControl("EmpNumb")

            'dr = ViewState("Dt").Select("EmpNumb = " + QuotedStr(GVR.Cells(2).Text))
            dr = ViewState("Dt").Select("EmpNumb = " + QuotedStr(txtID.Text))
            dr(0).Delete()
            'ViewState("Dt").AcceptChanges()
            BindGridDt(ViewState("Dt"), GridDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            countDt()
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        'Dim GVR As GridViewRow
        'Dim obj As GridViewRow
        'Dim txt As Label
        Try
            'GVR = GridDt.Rows(e.NewEditIndex)
            'ViewState("DtValue") = GVR.Cells(2).Text
            'FillTextBoxDt(ViewState("DtValue"))
            'MovePanel(pnlDt, pnlEditDt)
            'EnableHd(False)
            'StatusButtonSave(False)
            'GridDt.EditIndex = e.NewEditIndex

            'BindGridDt(ViewState("Dt"), GridDt)
            'obj = GridDt.Rows(e.NewEditIndex)
            'txt = obj.FindControl("EmpNumbEdit")
            'ViewState("DtValue") = txt
            'ViewState("StateDt") = "Edit"

            GridDt.EditIndex = e.NewEditIndex
            BindGridDt(ViewState("Dt"), GridDt)
            ViewState("StateDt") = "Edit"
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt, Dt2 As DataTable
        'Dim Dr As DataRow()
        Try
            Dt = BindDataTransaction(GetStringHd, "Year = " + Nmbr, ViewState("DBConnection").ToString)
            'newTrans()
            ddlYear.SelectedValue = Nmbr

            Dt2 = BindDataTransaction(GetStringDt1, "Year = " + Nmbr, ViewState("DBConnection").ToString)
            LbTotalPPH.Text = Dt2.Rows(0)("TTotalPPH")
            LbPPHHAsPaid.Text = Dt2.Rows(0)("TotalPPHHasPaid")
            lbPPHAdjust.Text = Dt2.Rows(0)("TotalPPHAdjust")

        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal EmpNumb As String)
        'Dim Dr As DataRow()
        'Try

        'Catch ex As Exception
        '    Throw New Exception("fill text box detail error : " + ex.ToString)
        'End Try
    End Sub

    Private Sub StatusButtonSave(ByVal Bool As Boolean)
        btnSaveAll.Visible = Bool
        btnSaveTrans.Visible = Bool
        btnBack.Visible = Bool
    End Sub

    Sub BindGridDt(ByVal source As DataTable, ByVal gv As GridView)
        Dim IsEmpty As Boolean
        Dim DtTemp As DataTable
        Dim dr As DataRow()
        Try
            IsEmpty = False
            dr = source.Select("", "", DataViewRowState.CurrentRows)
            If dr.Count = 0 Then
                'If source.Rows.Count = 0 Then
                DtTemp = source.Clone
                DtTemp.Rows.Add(DtTemp.NewRow()) ' create a new blank row to the DataTable
                IsEmpty = True
                gv.DataSource = DtTemp
            Else
                gv.DataSource = source
            End If
            gv.DataBind()
            gv.Columns(0).Visible = Not IsEmpty
            'dituup sementara, karena ga da detail
            'gv.Columns(2).Visible = Not IsEmpty
        Catch ex As Exception
            Throw New Exception("ShowGridDtIfEmpty Error : " + ex.ToString)
        End Try
    End Sub

    'Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt.Click
    'Dim Row As DataRow
    'Try
    '    If ViewState("StateDt") = "Edit" Then
    '        If ViewState("DtValue") <> TrimStr(tbMachineCode.Text + "|" + ddlMaintenanceItem.SelectedValue + "|" + ddlJob.SelectedValue) Then
    '            If CekExistData(ViewState("Dt"), "Machine,MaintenanceItem,Job", tbMachineCode.Text + "|" + ddlMaintenanceItem.SelectedValue + "|" + ddlJob.SelectedValue) Then
    '                lbStatus.Text = "Machine " + tbMachineName.Text + " Maintenance Item " + ddlMaintenanceItem.SelectedItem.Text + " Job " + ddlJob.SelectedItem.Text + " has already exists"
    '                Exit Sub
    '            End If
    '        End If
    '        Row = ViewState("Dt").Select("Machine+'|'+MaintenanceItem+'|'+Job = " + QuotedStr(ViewState("DtValue")))(0)
    '        If CekDt() = False Then
    '            Exit Sub
    '        End If
    '        Row.BeginEdit()
    '        Row("Machine") = tbMachineCode.Text
    '        Row("MachineName") = tbMachineName.Text
    '        Row("MaintenanceItem") = ddlMaintenanceItem.SelectedValue
    '        Row("MaintenanceItemName") = ddlMaintenanceItem.SelectedItem.Text
    '        Row("Job") = ddlJob.SelectedValue
    '        Row("JobName") = ddlJob.SelectedItem.Text
    '        'Row("MaintenanceDuration") = ddlMaintenanceDuration.SelectedValue
    '        'If ddlMaintenanceDuration.SelectedValue = "" Then
    '        'Row("MaintenanceDurationName") = ""
    '        'Else
    '        'Row("MaintenanceDurationName") = ddlMaintenanceDuration.SelectedItem.Text
    '        'End If
    '        If tbLastMaintenance.SelectedDate = Nothing Then
    '            Row("LastMaintenance") = DBNull.Value
    '        Else
    '            Row("LastMaintenance") = tbLastMaintenance.SelectedDate
    '        End If
    '        Row("ScheduleDate") = tbScheduleDate.SelectedDate
    '        Row("Remark") = tbRemarkDt.Text
    '        Row.EndEdit()
    '        'ViewState("Dt").AcceptChanges()
    '    Else
    '        'Insert
    '        If CekDt() = False Then
    '            Exit Sub
    '        End If
    '        If CekExistData(ViewState("Dt"), "Machine,MaintenanceItem,Job", tbMachineCode.Text + "|" + ddlMaintenanceItem.SelectedValue + "|" + ddlJob.SelectedValue) Then
    '            lbStatus.Text = "Machine " + tbMachineName.Text + " Maintenance Item " + ddlMaintenanceItem.SelectedItem.Text + " Job " + ddlJob.SelectedItem.Text + " has already exists"
    '            Exit Sub
    '        End If
    '        Dim dr As DataRow
    '        dr = ViewState("Dt").NewRow
    '        dr("Machine") = tbMachineCode.Text
    '        dr("MachineName") = tbMachineName.Text
    '        dr("MaintenanceItem") = ddlMaintenanceItem.SelectedValue
    '        dr("MaintenanceItemName") = ddlMaintenanceItem.SelectedItem.Text
    '        dr("Job") = ddlJob.SelectedValue
    '        dr("JobName") = ddlJob.SelectedItem.Text
    '        'dr("MaintenanceDuration") = ddlMaintenanceDuration.SelectedValue
    '        'If ddlMaintenanceDuration.SelectedValue = "" Then
    '        'dr("MaintenanceDurationName") = ""
    '        'Else
    '        'dr("MaintenanceDurationName") = ddlMaintenanceDuration.SelectedItem.Text
    '        'End If
    '        If tbLastMaintenance.SelectedDate = Nothing Then
    '            dr("LastMaintenance") = DBNull.Value
    '        Else
    '            dr("LastMaintenance") = tbLastMaintenance.SelectedDate
    '        End If
    '        dr("ScheduleDate") = tbScheduleDate.SelectedDate
    '        dr("Remark") = tbRemarkDt.Text
    '        ViewState("Dt").Rows.Add(dr)
    '    End If
    '    MovePanel(pnlEditDt, pnlDt)
    '    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
    '    BindGridDt(ViewState("Dt"), GridDt)
    '    StatusButtonSave(True)
    '    ViewState("Add") = "NotClear"
    'Catch ex As Exception
    '    lbStatus.Text = "btn save Dt Error : " + ex.ToString
    'Finally
    '    If Not con Is Nothing Then con.Dispose()
    '    If Not da Is Nothing Then da.Dispose()
    'End Try

    'End Sub

    'Protected Sub btnCancelDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnCancelDt.Click
    'Try
    '    MovePanel(pnlEditDt, pnlDt)
    '    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
    '    'EnableHd(GridDt.Rows.Count = 0)
    '    StatusButtonSave(True)
    'Catch ex As Exception
    '    lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
    'End Try
    'End Sub

    Public Sub CheckAllDt(ByRef gd As GridView, ByVal sender As CheckBox)
        Dim cb, cbselek As CheckBox
        Dim GRW As GridViewRow
        Try
            cb = sender
            For Each GRW In gd.Rows
                cbselek = GRW.FindControl("cbSelect")
                cbselek.Checked = cb.Checked
                'If cb.Checked = False Then
                'btnGetSetZero.Visible = True
                'End If
            Next
        Catch ex As Exception
            Throw New Exception("Check All Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub cbSelectHd_CheckedChanged1(ByVal sender As Object, ByVal e As System.EventArgs)
        CheckAllDt(GridDt, sender)
    End Sub

    Protected Sub btnGetDate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetData.Click
        Dim Dt As DataTable
        Dim ds As DataSet
        Dim CurDr, Dr As DataRow
        Try
            If CekHd() = False Then
                Exit Sub
            End If

            Dim SQLString, result As String
            SQLString = "   DECLARE @A VARCHAR(255) EXEC S_PYPPhYearGetDtCek " + ddlYear.SelectedValue + ", @A OUT SELECT @A"
            result = SQLExecuteScalar(SQLString, ViewState("DBConnection").ToString)

            If result.Length > 2 Then
                lbStatus.Text = result
                Exit Sub
            End If

            If GetCountRecord(ViewState("Dt")) > 0 Then
                lbStatus.Text = MessageDlg("Data not empty")
                Exit Sub
            End If

            ds = SQLExecuteQuery("EXEC S_PYPPhYearGetDt " + ddlYear.SelectedValue, ViewState("DBConnection").ToString)
            Dt = ds.Tables(0)

            If Dt.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("No Data")
                Exit Sub
            End If

            For Each CurDr In Dt.Rows
                Dr = ViewState("Dt").NewRow
                Dr("EmpNumb") = CurDr("EmpNumb")
                Dr("EmpName") = CurDr("EmpName")
                Dr("StartDate") = CurDr("StartDate")
                Dr("EndDate") = CurDr("EndDate")
                Dr("MaritalTax") = CurDr("MaritalTax")
                Dr("CurrCode") = CurDr("CurrCode")
                Dr("YearBruto") = FormatNumber(CurDr("YearBruto"), ViewState("DigitCurr"))
                Dr("YearPremi") = FormatNumber(CurDr("YearPremi"), ViewState("DigitCurr"))
                Dr("YearIuranJbt") = FormatNumber(CurDr("YearIuranJbt"), ViewState("DigitCurr"))
                Dr("YearNetto") = FormatNumber(CurDr("YearNetto"), ViewState("DigitCurr"))
                Dr("YearPTKP") = FormatNumber(CurDr("YearPTKP"), ViewState("DigitCurr"))
                Dr("YearPKP") = FormatNumber(CurDr("YearPKP"), ViewState("DigitCurr"))
                Dr("YearPPH") = FormatNumber(CurDr("YearPPH"), ViewState("DigitCurr"))
                Dr("TotalBruto") = FormatNumber(CurDr("TotalBruto"), ViewState("DigitCurr"))
                Dr("TotalPremi") = FormatNumber(CurDr("TotalPremi"), ViewState("DigitCurr"))
                Dr("TotalTHR") = FormatNumber(CurDr("TotalTHR"), ViewState("DigitCurr"))
                Dr("TotalNetto") = FormatNumber(CurDr("TotalNetto"), ViewState("DigitCurr"))
                Dr("TotalInsentif") = FormatNumber(CurDr("TotalInsentif"), ViewState("DigitCurr"))
                Dr("TotalIuranJbt") = FormatNumber(CurDr("TotalIuranJbt"), ViewState("DigitCurr"))
                Dr("TotalPesangon") = FormatNumber(CurDr("TotalPesangon"), ViewState("DigitCurr"))
                Dr("TotalPPH") = FormatNumber(CurDr("TotalPPH"), ViewState("DigitCurr"))
                Dr("PPHPesangon") = FormatNumber(CurDr("PPHPesangon"), ViewState("DigitCurr"))
                Dr("PPHHasPaid") = FormatNumber(CurDr("PPHHasPaid"), ViewState("DigitCurr"))
                Dr("PPHAdjust") = FormatNumber(CurDr("PPHAdjust"), ViewState("DigitCurr"))
                ViewState("Dt").Rows.Add(Dr)
            Next
            BindGridDtExtended()
            countDt()
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
        Catch ex As Exception
            Throw New Exception("btnGetDate_Click error: " + ex.ToString)
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

    Protected Sub GridDt_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles GridDt.RowUpdating
        'Dim SQLString As String
        Dim lbEmpNumb, lbEmpName, lbStartDate, lbEndDate, lbMaritalTax, lbCurrCode, lbTotalPesangon, lbTotalBruto, lbTotalPremi, lbTotalInsentif As Label
        Dim lbTotalTHR, lbTotalNetto, lbYearBruto, lbYearIuranJbt, lbYearPremi, lbYearNetto, lbYearPTKP, lbYearPKP, lbYearPPH, lbTotalPPH, lbPPHHasPaid, lbPPHAdjust As Label
        Dim tbTotalIuranJbt, tbTotalInsentif, tbTotalTHR, tbPPHPesangon As TextBox
        Dim GVR As GridViewRow
        Dim Row As DataRow

        Try
            GVR = GridDt.Rows(e.RowIndex)
            lbEmpNumb = GVR.FindControl("EmpNumbEdit")
            lbEmpName = GVR.FindControl("EmpNameEdit")
            lbStartDate = GVR.FindControl("StartDateEdit")
            lbEndDate = GVR.FindControl("EndDateEdit")
            lbMaritalTax = GVR.FindControl("MaritalTaxEdit")
            lbCurrCode = GVR.FindControl("CurrCodeEdit")
            lbTotalPesangon = GVR.FindControl("TotalPesangonEdit")
            lbTotalBruto = GVR.FindControl("TotalBrutoEdit")
            lbTotalPremi = GVR.FindControl("TotalPremiEdit")
            lbTotalInsentif = GVR.FindControl("TotalInsentifEdit")
            lbTotalTHR = GVR.FindControl("TotalTHREdit")
            lbTotalNetto = GVR.FindControl("TotalNettoEdit")
            lbYearBruto = GVR.FindControl("YearBrutoEdit")
            lbYearIuranJbt = GVR.FindControl("YearIuranJbtEdit")
            lbYearPremi = GVR.FindControl("YearPremiEdit")
            lbYearNetto = GVR.FindControl("YearNettoEdit")
            lbYearPTKP = GVR.FindControl("YearPTKPEdit")
            lbYearPKP = GVR.FindControl("YearPKPEdit")
            lbYearPPH = GVR.FindControl("YearPPHEdit")
            lbTotalPPH = GVR.FindControl("TotalPPHEdit")
            lbPPHHasPaid = GVR.FindControl("PPHHasPaidEdit")
            lbPPHAdjust = GVR.FindControl("PPHAdjustEdit")
            tbTotalIuranJbt = GVR.FindControl("TotalIuranJbtEdit")
            tbTotalInsentif = GVR.FindControl("TotalYearInsentifEdit")
            tbTotalTHR = GVR.FindControl("TotalYearTHREdit")
            tbPPHPesangon = GVR.FindControl("PPHPesangonEdit")


            If IsNumeric(tbTotalIuranJbt.Text.Replace(",", "")) = 0 Then
                lbStatus.Text = MessageDlg("Total Iuran Jbt must be in numeric.")
                tbTotalIuranJbt.Focus()
                Exit Sub
            End If

            If IsNumeric(tbTotalInsentif.Text.Replace(",", "")) = 0 Then
                lbStatus.Text = MessageDlg("Total Insentif must be in numeric.")
                tbTotalInsentif.Focus()
                Exit Sub
            End If

            If IsNumeric(tbTotalTHR.Text.Replace(",", "")) = 0 Then
                lbStatus.Text = MessageDlg("Total THR must be in numeric.")
                tbTotalTHR.Focus()
                Exit Sub
            End If

            If IsNumeric(tbPPHPesangon.Text.Replace(",", "")) = 0 Then
                lbStatus.Text = MessageDlg("Total PPH Pesangon must be in numeric.")
                tbPPHPesangon.Focus()
                Exit Sub
            End If


            If ViewState("StateDt") = "Edit" Then

                Row = ViewState("Dt").Select("EmpNumb = " + QuotedStr(lbEmpNumb.Text))(0)

                Row.BeginEdit()
                Row("EmpNumb") = lbEmpNumb.Text
                Row("EmpName") = lbEmpName.Text
                Row("StartDate") = lbStartDate.Text
                Row("EndDate") = lbEndDate.Text
                Row("MaritalTax") = lbMaritalTax.Text
                Row("CurrCode") = lbCurrCode.Text
                Row("TotalPesangon") = lbTotalPesangon.Text
                Row("TotalBruto") = lbTotalBruto.Text
                Row("TotalIuranJbt") = tbTotalIuranJbt.Text
                Row("TotalPremi") = lbTotalPremi.Text
                Row("TotalInsentif") = lbTotalInsentif.Text
                Row("TotalTHR") = lbTotalTHR.Text
                Row("TotalNetto") = lbTotalNetto.Text
                Row("YearBruto") = lbYearBruto.Text
                Row("YearIuranJbt") = lbYearIuranJbt.Text
                Row("YearPremi") = lbYearPremi.Text
                Row("TotalInsentif") = tbTotalInsentif.Text
                Row("TotalTHR") = tbTotalTHR.Text
                Row("YearNetto") = lbYearNetto.Text
                Row("YearPTKP") = lbYearPTKP.Text
                Row("YearPKP") = lbYearPKP.Text
                Row("YearPPH") = lbYearPPH.Text
                Row("TotalPPH") = lbTotalPPH.Text
                Row("PPHPesangon") = tbPPHPesangon.Text
                Row("PPHHasPaid") = lbPPHHasPaid.Text
                Row("PPHAdjust") = lbPPHAdjust.Text
                Row.EndEdit()
                'ViewState("Dt").AcceptChanges()
            End If
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            GridDt.EditIndex = -1
            BindGridDt(ViewState("Dt"), GridDt)
            countDt()
        Catch ex As Exception
            lbStatus.Text = "data grid row updating error : " + ex.ToString
        End Try
    End Sub

    Private Sub countDt()
        Dim dr As DataRow
        Dim hasil, hasil2, hasil3 As Double
        Try
            hasil = 0
            hasil2 = 0
            hasil3 = 0
            For Each dr In ViewState("Dt").Rows
                If Not dr.RowState = DataRowState.Deleted Then
                    hasil = hasil + CFloat(dr("PPHHasPaid").ToString)
                    hasil2 = hasil2 + CFloat(dr("PPHAdjust").ToString)
                    hasil3 = hasil3 + CFloat(dr("TotalPPH").ToString)
                End If
            Next
            LbPPHHAsPaid.Text = FormatNumber(hasil, ViewState("DigitCurr"))
            lbPPHAdjust.Text = FormatNumber(hasil2, ViewState("DigitCurr"))
            LbTotalPPH.Text = FormatNumber(hasil3, ViewState("DigitCurr"))
        Catch ex As Exception
            Throw New Exception("Count Dt Error : " + ex.ToString)
        End Try
    End Sub

End Class