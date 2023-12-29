Imports System.Data
Imports System.Data.SqlClient
'Imports System.Data.Odbc
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc

Partial Class Transaction_TrMTNOrder_TrMTNOrder
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "SELECT * FROM V_MTNOrderHd "

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

            If Not ViewState("MachineClose") Is Nothing Then
                If HiddenRemarkClose.Value <> "False Value" Then
                    Dim sqlstring, result As String
                    'lbStatus.Text = "Product '" + ViewState("ProductClose").ToString + "' Remark Close '" + HiddenRemarkClose.Value + "'"
                    'Exit Sub
                    sqlstring = "Declare @A VarChar(255) EXEC S_MTNOrderClosing '" + ViewState("Reference") + "', '" + ViewState("MachineClose").ToString + "','" + ViewState("MaintenanceItemClose").ToString + "','" + ViewState("JobClose").ToString + "','" + HiddenRemarkClose.Value + "'," + QuotedStr(ViewState("UserId")) + ", @A OUT SELECT @A"
                    result = SQLExecuteScalar(sqlstring, ViewState("DBConnection").ToString)
                    If result.Length > 2 Then
                        lbStatus.Text = result
                    Else
                        BindDataDt(ViewState("Reference"))
                        GridDt.Columns(0).Visible = False
                        'GridDt.Columns(1).Visible = False
                    End If
                End If
                ViewState("MachineClose") = Nothing
                ViewState("MaintenanceItemClose") = Nothing
                ViewState("JobClose") = Nothing
                HiddenRemarkClose.Value = ""
                'GridDt.Columns(0).Visible = False
            End If
            ' hasil dari search dialog
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnGetData" Then
                    Dim drResult As DataRow
                    If IsNothing(Session("Result")) Then
                        lbStatus.Text = "Session is empty"
                        Exit Sub
                    End If
                    For Each drResult In Session("Result").Rows
                        If Not CekExistData(ViewState("Dt"), "MaintenanceItem,Job", drResult("MaintenanceItem") + "|" + drResult("Job")) Then
                            'insert
                            Dim dr As DataRow
                            dr = ViewState("Dt").NewRow
                            dr("MaintenanceItem") = drResult("MaintenanceItem")
                            dr("MaintenanceItemName") = drResult("MaintenanceItemName")
                            dr("Job") = drResult("Job")
                            dr("JobName") = drResult("JobName")
                            dr("ItemSpec") = drResult("Specification")
                            dr("JobDescription") = ""
                            dr("Remark") = ""
                            ViewState("Dt").Rows.Add(dr)
                        End If
                    Next
                    BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    Session("ResultSame") = Nothing
                End If
                If ViewState("Sender") = "btnReffNmbr" Then
                    tbReffNmbr.Text = Session("Result")(0).ToString
                    ddlMachine.SelectedValue = Session("Result")(1).ToString
                End If
                If ViewState("Sender") = "btnMaintenanceItem" Then
                    ddlMaintenanceItem.SelectedValue = Session("Result")(0).ToString
                    ddlJob.SelectedValue = Session("Result")(1).ToString
                    tbitemSpec.Text = Session("Result")(2).ToString
                End If
                If ViewState("Sender") = "btnProjectManager" Then
                    tbProjectManager.Text = Session("Result")(0).ToString
                    tbProjectManagerName.Text = Session("Result")(1).ToString
                End If
                If ViewState("Sender") = "btnPrepare" Then
                    tbPrepare.Text = Session("Result")(0).ToString
                    tbPrepareName.Text = Session("Result")(1).ToString
                End If
                If ViewState("Sender") = "btnApproval" Then
                    tbApprovalBy.Text = Session("Result")(0).ToString
                    tbApprovalByName.Text = Session("Result")(1).ToString
                End If
                If ViewState("Sender") = "btnAcknowled" Then
                    tbAcknowledBy.Text = Session("Result")(0).ToString
                    tbAcknowledByName.Text = Session("Result")(1).ToString
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
        ViewState("SortExpression") = Nothing
        ViewState("DigitCurr") = 0
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
        If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
            ddlCommand.Items.Add("Print")
            ddlCommand2.Items.Add("Print")
        End If
        If Request.QueryString("ContainerId").ToString = "TrMTNOrderCorrID" Then
            lbjudul.Text = "Maintenance Order - Corrective"
            lbReffType.Text = "Request"
            ViewState("MOType") = "Corrective"
        Else
            lbjudul.Text = "Maintenance Order - Preventive"
            lbReffType.Text = "Schedule"
            ViewState("MOType") = "Preventive"
        End If
        FillCombo(ddlMachine, "Select MachineCode, MachineName FROM MsMachine ", True, "MachineCode", "MachineName", ViewState("DBConnection"))
        FillCombo(ddlMaintenanceItem, "Select ItemNo, ItemName FROM MsMaintenanceItem ", True, "ItemNo", "ItemName", ViewState("DBConnection"))
        FillCombo(ddlJob, "Select JobCode, JobName FROM MsJob ", True, "JobCode", "JobName", ViewState("DBConnection"))
        'tbRef.Attributes.Add("ReadOnly", "True")
        'tbMachineName.Attributes.Add("ReadOnly", "True")
        'tbAdjPercent.Attributes.Add("OnKeyDown", "return PressNumeric();")
    End Sub

    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim StrFilter, GetStringHd1 As String
        Try
            StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue, ddlRange.SelectedValue)
            If AdvanceFilter.Length > 1 Then
                StrFilter = AdvanceFilter
            End If
            GetStringHd1 = "Select * From V_MTNOrderHd WHERE MOType = " + QuotedStr(ViewState("MOType").ToString)
            DT = BindDataTransaction(GetStringHd1, StrFilter, ViewState("DBConnection").ToString)
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
        Return "SELECT * FROM V_MTNOrderDt WHERE TransNmbr = " + QuotedStr(Nmbr) + " "
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
                Session("SelectCommand") = "EXEC S_MTNFormOrder " + Result
                Session("ReportFile") = ".../../../Rpt/FormMTNOrder.frx"
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
                        Result = ExecSPCommandGo(ActionValue, "S_MTNOrder", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            'tbRef.Enabled = State
            btnReffNmbr.Enabled = State
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
                If ViewState("MOType") = "Corrective" Then
                    tbRef.Text = GetAutoNmbr("MTWOC", "N", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
                Else
                    tbRef.Text = GetAutoNmbr("MTWOP", "N", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
                End If
                SQLString = "INSERT INTO MTNOrderHd (TransNmbr, Status, TransDate, ReffNmbr, Machine, FgSubkon, EstCompleteDate, ProjectManager, PrepareBy, ApprovedBy, AcknowledBy, Remark, UserPrep, DatePrep, MOType ) " + _
                "SELECT '" + tbRef.Text + "', 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', " + QuotedStr(tbReffNmbr.Text) + ", " + QuotedStr(ddlMachine.SelectedValue) + ", " + QuotedStr(ddlFgSubkon.SelectedValue) + ", " + _
                " '" + Format(tbEstComplete.SelectedValue, "yyyy-MM-dd") + "', " + QuotedStr(tbProjectManager.Text) + "," + QuotedStr(tbPrepare.Text) + "," + QuotedStr(tbApprovalBy.Text) + "," + QuotedStr(tbAcknowledBy.Text) + "," + QuotedStr(tbRemark.Text) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", GetDate(), " + QuotedStr(ViewState("MOType"))
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("SELECT Status FROM MTNOrderHd WHERE TransNmbr = " + QuotedStr(tbRef.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE MTNOrderHd SET ReffNmbr = " + QuotedStr(tbReffNmbr.Text) + ", TransDate = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', " + _
                " EstCompleteDate = '" + Format(tbEstComplete.SelectedValue, "yyyy-MM-dd") + "', Machine = " + QuotedStr(ddlMachine.SelectedValue) + ", FgSubkon = " + QuotedStr(ddlFgSubkon.SelectedValue) + ", ProjectManager = " + QuotedStr(tbProjectManager.Text) + ", PrepareBy = " + QuotedStr(tbPrepare.Text) + ", ApprovedBy = " + QuotedStr(tbApprovalBy.Text) + ", AcknowledBy = " + QuotedStr(tbAcknowledBy.Text) + ", Remark = '" + tbRemark.Text + "'," + _
                " UserPrep = " + QuotedStr(ViewState("UserId").ToString) + ", DatePrep = getDate() " + _
                " WHERE TransNmbr = '" + tbRef.Text + "' AND MOType = " + QuotedStr(ViewState("MOType"))
            End If
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            'update Primary Key on Dt
            Dim Row As DataRow()

            Row = ViewState("Dt").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbRef.Text
                'Row(I)("TransClass") = "JE"
                Row(I).EndEdit()
            Next

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString 'System.Configuration.ConfigurationManager.AppSettings.Get("ConString")
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, MaintenanceItem, Job, ItemSpec, JobDescription, Remark FROM MTNOrderDt WHERE TransNmbr = '" & ViewState("Reference") & "' ", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt As New DataTable("MTNOrderDt")

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
            MovePanel(pnlInput, PnlHd)
            CurrFilter = tbFilter.Text
            Value = ddlField.SelectedValue
            tbFilter.Text = tbRef.Text
            ddlField.SelectedValue = "TransNmbr"
            btnSearch_Click(Nothing, Nothing)
            tbFilter.Text = CurrFilter
            ddlField.SelectedValue = Value
        Catch ex As Exception
            lbStatus.Text = "Save All Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            'tbAdjPercent.Text = "0"
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, pnlDt, GridDt)
            newTrans()
            EnableHd(True)
            btnHome.Visible = False
            tbDate.Focus()
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
            BindDataDt("")
            GridDt.Columns(1).Visible = False
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbRef.Text = ""
            tbDate.SelectedDate = ViewState("ServerDate") 'Today
            tbReffNmbr.Text = ""
            ddlMachine.SelectedValue = ""
            tbPrepare.Text = ""
            tbPrepareName.Text = ""
            tbProjectManager.Text = ""
            tbProjectManagerName.Text = ""
            tbApprovalBy.Text = ""
            tbApprovalByName.Text = ""
            tbAcknowledBy.Text = ""
            tbAcknowledByName.Text = ""
            tbEstComplete.SelectedDate = ViewState("ServerDate") 'Today
            tbRemark.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            FillMaintenanceItem(False)
            ddlMaintenanceItem.SelectedValue = ""
            FillJob(False)
            ddlJob.SelectedValue = ""
            tbitemSpec.Text = ""
            tbJobDesc.Text = ""
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
        Catch ex As Exception
            lbStatus.Text = "btn save all error : " + ex.ToString
        End Try
    End Sub

    Protected Sub FillMaintenanceItem(ByVal FgAll As Boolean)
        Try
            If FgAll = True Then
                FillCombo(ddlMaintenanceItem, "Select distinct MaintenanceItem, MaintenanceItemName FROM V_MTNOrderReff WHERE Reference = " + QuotedStr(tbReffNmbr.Text) + " And Machine = " + QuotedStr(ddlMachine.SelectedValue), True, "MaintenanceItem", "MaintenanceItemName", ViewState("DBConnection"))
            Else
                FillCombo(ddlMaintenanceItem, "Select distinct MaintenanceItem, MaintenanceItemName FROM V_MTNOrderReff WHERE DoneMO = 'N' AND Reference = " + QuotedStr(tbReffNmbr.Text) + " And Machine = " + QuotedStr(ddlMachine.SelectedValue), True, "MaintenanceItem", "MaintenanceItemName", ViewState("DBConnection"))
            End If

        Catch ex As Exception
            lbStatus.Text = "FillMaintenanceItem error : " + ex.ToString
        End Try
    End Sub

    Protected Sub FillJob(ByVal FgAll As Boolean)
        Try
            If FgAll = True Then
                FillCombo(ddlJob, "Select Job, JobName FROM V_MTNOrderReff WHERE Reference = " + QuotedStr(tbReffNmbr.Text) + " And Machine = " + QuotedStr(ddlMachine.SelectedValue) + " and MaintenanceItem = " + QuotedStr(ddlMaintenanceItem.SelectedValue), True, "Job", "JobName", ViewState("DBConnection"))
            Else
                FillCombo(ddlJob, "Select Job, JobName FROM V_MTNOrderReff WHERE DoneMO = 'N' AND Reference = " + QuotedStr(tbReffNmbr.Text) + " And Machine = " + QuotedStr(ddlMachine.SelectedValue) + " and MaintenanceItem = " + QuotedStr(ddlMaintenanceItem.SelectedValue), True, "Job", "JobName", ViewState("DBConnection"))
            End If
        Catch ex As Exception
            lbStatus.Text = "FillJob error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt.Click, btnAddDt2.Click
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
            If tbDate.IsNull Then
                lbStatus.Text = MessageDlg("Date must have value")
                tbDate.Focus()
                Return False
            End If
            If tbReffNmbr.Text = "" Then
                If ViewState("MOType") = "Corrective" Then
                    lbStatus.Text = MessageDlg("Request No must have value")
                Else
                    lbStatus.Text = MessageDlg("Schedule No must have value")
                End If
                btnreffNmbr.Focus()
                Return False
            End If
            If ddlMachine.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Machine must have value")
                ddlMachine.Focus()
                Return False
            End If
            If tbProjectManager.Text = "" Then
                lbStatus.Text = MessageDlg("Project Manager must have value")
                tbProjectManager.Focus()
                Return False
            End If
            If tbPrepare.Text = "" Then
                lbStatus.Text = MessageDlg("Prepare by must have value")
                tbPrepare.Focus()
                Return False
            End If
            If tbApprovalBy.Text = "" Then
                lbStatus.Text = MessageDlg("Approval By must have value")
                tbApprovalBy.Focus()
                Return False
            End If
            If tbAcknowledBy.Text = "" Then
                lbStatus.Text = MessageDlg("Acknowled By must have value")
                tbAcknowledBy.Focus()
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
                If Dr("MaintenanceItem").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Maintenance Item Must Have Value")
                    Return False
                End If
                If Dr("Job").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Job Must Have Value")
                    Return False
                End If
            Else
                If ddlMaintenanceItem.SelectedValue = "" Then
                    lbStatus.Text = MessageDlg("Maintenance Item Must Have Value")
                    ddlMaintenanceItem.Focus()
                    Return False
                End If
                If ddlJob.SelectedValue.Trim = "" Then
                    lbStatus.Text = MessageDlg("Job Must Have Value")
                    ddlJob.Focus()
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
            FilterName = "Reference, Status, Request No, Machine, Subkon, Est Complete Date, Project Manager, Prepare By, Approved By, Acknowled By, Remark"
            FilterValue = "TransNmbr, Status, ReffNmbr, MachineName, FgSubkon, dbo.FormatDate(EstCompleteDate), ProjectManagerName, PrepareByName, ApprovedByName, AcknowledByName, Remark"
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
                    FillTextBoxHd(ViewState("Reference"))
                    BindDataDt(ViewState("Reference"))
                    GridDt.Columns(0).Visible = False
                    GridDt.Columns(1).Visible = GVR.Cells(3).Text = "P"
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, pnlDt, GridDt)
                    btnHome.Visible = True
                    'Dim Dr As DataRow
                    'Dr = FindMaster("Rate", ddlCurrency.SelectedValue + "|" + Format(tbDate.SelectedDate, "yyyy-MM-dd"), Session("DBConnection").ToString)
                    'If Not Dr Is Nothing Then
                    '    ViewState("DigitCurr") = Dr("digit")
                    '    AttachScript("setformat();", Page, Me.GetType())
                    'End If
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
                        GridDt.Columns(0).Visible = True
                        GridDt.Columns(1).Visible = False
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        btnHome.Visible = False
                        'tbAdjPercent.Text = "0"
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
                        Session("SelectCommand") = "EXEC S_MTNFormOrder ''" + QuotedStr(GVR.Cells(2).Text) + " ''"
                        Session("ReportFile") = ".../../../Rpt/FormMTNOrder.frx"
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

    Protected Sub GridDt_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridDt.RowCommand
        Try
            If e.CommandName = "Insert" Then
                btnAddDt_Click(Nothing, Nothing)
            ElseIf e.CommandName = "Closing" Then
                Dim GVR As GridViewRow
                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))
                If ViewState("Status") <> "P" Then
                    lbStatus.Text = MessageDlg("Status Maintenance Schedule is not Post, cannot close Detail Schedule")
                    Exit Sub
                End If
                ViewState("MachineClose") = GVR.Cells(4).Text
                ViewState("MaintenanceItemClose") = GVR.Cells(6).Text
                ViewState("JobClose") = GVR.Cells(8).Text
                If GVR.Cells(14).Text <> "Y" Then
                    AttachScript("closing();", Page, Me.GetType)
                Else
                    ViewState("MachineClose") = Nothing
                    ViewState("MaintenanceItemClose") = Nothing
                    ViewState("JobClose") = Nothing
                End If
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)

            dr = ViewState("Dt").Select("MaintenanceItem = " + QuotedStr(GVR.Cells(2).Text) + " and Job = " + QuotedStr(GVR.Cells(4).Text))
            dr(0).Delete()
            'ViewState("Dt").AcceptChanges()
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
            ViewState("DtValue") = GVR.Cells(2).Text + "|" + GVR.Cells(4).Text
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
            BindToText(tbReffNmbr, Dt.Rows(0)("ReffNmbr").ToString)
            BindToDropList(ddlMachine, Dt.Rows(0)("Machine").ToString)
            BindToDropList(ddlFgSubkon, Dt.Rows(0)("FgSubkon").ToString)
            BindToText(tbProjectManager, Dt.Rows(0)("ProjectManager").ToString)
            BindToText(tbProjectManagerName, Dt.Rows(0)("ProjectManagerName").ToString)
            BindToText(tbPrepare, Dt.Rows(0)("PrepareBy").ToString)
            BindToText(tbPrepareName, Dt.Rows(0)("PrepareByName").ToString)
            BindToText(tbApprovalBy, Dt.Rows(0)("ApprovedBy").ToString)
            BindToText(tbApprovalByName, Dt.Rows(0)("ApprovedByName").ToString)
            BindToText(tbAcknowledBy, Dt.Rows(0)("AcknowledBy").ToString)
            BindToText(tbAcknowledByName, Dt.Rows(0)("AcknowledByName").ToString)
            BindToDate(tbEstComplete, Dt.Rows(0)("EstCompleteDate").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal Product As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("MaintenanceItem+'|'+Job = " + QuotedStr(Product))
            If Dr.Length > 0 Then
                FillMaintenanceItem(True)
                BindToDropList(ddlMaintenanceItem, Dr(0)("MaintenanceItem").ToString)
                FillJob(True)
                BindToDropList(ddlJob, Dr(0)("Job").ToString)
                BindToText(tbitemSpec, Dr(0)("ItemSpec").ToString)
                BindToText(tbJobDesc, Dr(0)("JobDescription").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
            End If
            'Dt = BindDataTransaction(GetStringDt(tbRef.Text), "ItemNo = " + ItemNo, Session("DBConnection").ToString)
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
            gv.Columns(1).Visible = False
            'dituup sementara, karena ga da detail
            'gv.Columns(2).Visible = Not IsEmpty
        Catch ex As Exception
            Throw New Exception("ShowGridDtIfEmpty Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt.Click
        Dim Row As DataRow
        Try
            If ViewState("StateDt") = "Edit" Then
                If ViewState("DtValue") <> TrimStr(ddlMaintenanceItem.SelectedValue + "|" + ddlJob.SelectedValue) Then
                    If CekExistData(ViewState("Dt"), "MaintenanceItem,Job", ddlMaintenanceItem.SelectedValue + "|" + ddlJob.SelectedValue) Then
                        lbStatus.Text = "Maintenance Item " + ddlMaintenanceItem.SelectedItem.Text + " Job " + ddlJob.SelectedItem.Text + " has already exists"
                        Exit Sub
                    End If
                End If
                Row = ViewState("Dt").Select("MaintenanceItem+'|'+Job = " + QuotedStr(ViewState("DtValue")))(0)
                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                Row("MaintenanceItem") = ddlMaintenanceItem.SelectedValue
                Row("MaintenanceItemName") = ddlMaintenanceItem.SelectedItem.Text
                Row("Job") = ddlJob.SelectedValue
                Row("JobName") = ddlJob.SelectedItem.Text
                Row("ItemSpec") = tbitemSpec.Text
                Row("JobDescription") = tbJobDesc.Text
                'Row("MaintenanceDuration") = ddlMaintenanceDuration.SelectedValue
                'If ddlMaintenanceDuration.SelectedValue = "" Then
                'Row("MaintenanceDurationName") = ""
                'Else
                'Row("MaintenanceDurationName") = ddlMaintenanceDuration.SelectedItem.Text
                'End If
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()
                'ViewState("Dt").AcceptChanges()
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If
                If CekExistData(ViewState("Dt"), "MaintenanceItem,Job", ddlMaintenanceItem.SelectedValue + "|" + ddlJob.SelectedValue) Then
                    lbStatus.Text = "Maintenance Item " + ddlMaintenanceItem.SelectedItem.Text + " Job " + ddlJob.SelectedItem.Text + " has already exists"
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("MaintenanceItem") = ddlMaintenanceItem.SelectedValue
                dr("MaintenanceItemName") = ddlMaintenanceItem.SelectedItem.Text
                dr("Job") = ddlJob.SelectedValue
                dr("JobName") = ddlJob.SelectedItem.Text
                dr("ItemSpec") = tbitemSpec.Text
                dr("JobDescription") = tbJobDesc.Text
                'dr("MaintenanceDuration") = ddlMaintenanceDuration.SelectedValue
                'If ddlMaintenanceDuration.SelectedValue = "" Then
                'dr("MaintenanceDurationName") = ""
                'Else
                'dr("MaintenanceDurationName") = ddlMaintenanceDuration.SelectedItem.Text
                'End If
                dr("Remark") = tbRemarkDt.Text
                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            BindGridDt(ViewState("Dt"), GridDt)
            StatusButtonSave(True)
            ViewState("Add") = "NotClear"
        Catch ex As Exception
            lbStatus.Text = "btn save Dt Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try

    End Sub

    Protected Sub btnCancelDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnCancelDt.Click
        Try
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            'EnableHd(GridDt.Rows.Count = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub

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

    Protected Sub btnGetdata_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetData.Click
        Dim ResultField As String
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            Session("Result") = Nothing
            Session("filter") = "Select MaintenanceItem, MaintenanceItemName, Job, JobName, Specification, Explanation, RemarkDt from V_MTNOrderReff WHERE DoneMO = 'N' AND Reference = " + QuotedStr(tbReffNmbr.Text) + " and Machine = " + QuotedStr(ddlMachine.SelectedValue)
            ResultField = "MaintenanceItem, MaintenanceItemName, Job, JobName, Specification"
            ViewState("Sender") = "btnGetData"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            Throw New Exception("btnGetdata_Click error: " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnReffNmbr_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReffNmbr.Click
        Dim ResultField As String
        Try
            Session("Result") = Nothing
            If ViewState("MOType") = "Corrective" Then
                Session("filter") = "Select distinct Reference, dbo.FormatDate(TransDate) AS Date, Machine, MachineName, Remark from V_MTNOrderReff WHERE DoneMO = 'N' and Type = 'Request'"
            Else
                Session("filter") = "Select distinct Reference, dbo.FormatDate(TransDate) AS Date, Machine, MachineName, Remark from V_MTNOrderReff WHERE DoneMO = 'N' and Type = 'Schedule'"
            End If
            ResultField = "Reference, Machine, MachineName"
            ViewState("Sender") = "btnReffNmbr"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            Throw New Exception("btnReffNmbr_Click error: " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnProjectManager_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProjectManager.Click
        Dim ResultField As String
        Try
            Session("Result") = Nothing
            Session("filter") = "Select * from V_MsEmployee"
            ResultField = "Emp_No, Emp_Name"
            ViewState("Sender") = "btnProjectManager"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            Throw New Exception("btnProjectManager_Click error: " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnPrepare_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrepare.Click
        Dim ResultField As String
        Try
            Session("Result") = Nothing
            Session("filter") = "Select * from V_MsEmployee"
            ResultField = "Emp_No, Emp_Name"
            ViewState("Sender") = "btnPrepare"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            Throw New Exception("btnPrepare_Click error: " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnApproval_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApproval.Click
        Dim ResultField As String
        Try
            Session("Result") = Nothing
            Session("filter") = "Select * from V_MsEmployee"
            ResultField = "Emp_No, Emp_Name"
            ViewState("Sender") = "btnApproval"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            Throw New Exception("btnApproval_Click error: " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnAcknowled_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAcknowled.Click
        Dim ResultField As String
        Try
            Session("Result") = Nothing
            Session("filter") = "Select * from V_MsEmployee"
            ResultField = "Emp_No, Emp_Name"
            ViewState("Sender") = "btnAcknowled"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            Throw New Exception("btnAcknowled_Click error: " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbAcknowledBy_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAcknowledBy.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Dim SQLString As String
        Try
            SQLString = "Select EmpNumb, EmpName FROM MsEmployee WHERE EmpNumb = " + QuotedStr(tbAcknowledBy.Text)
            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbAcknowledBy.Text = Dr("EmpNumb")
                tbAcknowledByName.Text = Dr("EmpName")
            Else
                tbAcknowledBy.Text = ""
                tbAcknowledByName.Text = ""
            End If
        Catch ex As Exception
            Throw New Exception("tbAcknowledBy_TextChanged error: " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbApprovalBy_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbApprovalBy.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Dim SQLString As String
        Try
            SQLString = "Select EmpNumb, EmpName FROM MsEmployee WHERE EmpNumb = " + QuotedStr(tbApprovalBy.Text)
            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbApprovalBy.Text = Dr("EmpNumb")
                tbApprovalByName.Text = Dr("EmpName")
            Else
                tbApprovalBy.Text = ""
                tbApprovalByName.Text = ""
            End If
        Catch ex As Exception
            Throw New Exception("tbApprovalBy_TextChanged error: " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbPrepare_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPrepare.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Dim SQLString As String
        Try
            SQLString = "Select EmpNumb, EmpName FROM MsEmployee WHERE EmpNumb = " + QuotedStr(tbPrepare.Text)
            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbPrepare.Text = Dr("EmpNumb")
                tbPrepareName.Text = Dr("EmpName")
            Else
                tbPrepare.Text = ""
                tbPrepareName.Text = ""
            End If
        Catch ex As Exception
            Throw New Exception("tbPrepare_TextChanged error: " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbProjectManager_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbProjectManager.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Dim SQLString As String
        Try
            SQLString = "Select EmpNumb, EmpName FROM MsEmployee WHERE EmpNumb = " + QuotedStr(tbProjectManager.Text)
            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbProjectManager.Text = Dr("EmpNumb")
                tbProjectManagerName.Text = Dr("EmpName")
            Else
                tbProjectManager.Text = ""
                tbProjectManagerName.Text = ""
            End If
        Catch ex As Exception
            Throw New Exception("tbProjectManager_TextChanged error: " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnMaintenanceItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMaintenanceItem.Click
        Dim ResultField As String
        Try
            Session("Result") = Nothing
            Session("filter") = "Select MaintenanceItem, MaintenanceItemName, Job, JobName, Specification, Explanation, RemarkDt from V_MTNOrderReff WHERE DoneMO = 'N' AND Reference = " + QuotedStr(tbReffNmbr.Text) + " and Machine = " + QuotedStr(ddlMachine.SelectedValue)
            ResultField = "MaintenanceItem, Job, Specification"
            ViewState("Sender") = "btnMaintenanceItem"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            Throw New Exception("btnMaintenanceItem_Click error: " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlMaintenanceItem_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMaintenanceItem.SelectedIndexChanged
        Try
            FillJob(False)
            ddlJob.SelectedValue = ""
        Catch ex As Exception
            Throw New Exception("ddlMaintenanceItem_SelectedIndexChanged error: " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlJob_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlJob.SelectedIndexChanged
        Dim dt As DataTable
        Try
            dt = SQLExecuteQuery("Select Specification FROM V_MTNOrderReff WHERE Reference = " + QuotedStr(tbReffNmbr.Text) + " And Machine = " + QuotedStr(ddlMachine.SelectedValue) + " and MaintenanceItem = " + QuotedStr(ddlMaintenanceItem.SelectedValue) + " AND Job = " + QuotedStr(ddlJob.SelectedValue), ViewState("DBConnection")).Tables(0)
            If dt.Rows.Count > 0 Then
                tbitemSpec.Text = dt.Rows(0)("Specification")
            Else
                tbitemSpec.Text = ""
            End If
        Catch ex As Exception
            Throw New Exception("ddlJob_SelectedIndexChanged error: " + ex.ToString)
        End Try
    End Sub
End Class
