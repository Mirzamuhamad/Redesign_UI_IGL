Imports System.Data
Imports System.Data.SqlClient
'Imports System.Data.Odbc
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc

Partial Class Transaction_TrEmpDinas_TrEmpDinas
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "SELECT * FROM V_PESuratDinasHD"
    Protected GetStringComplete As String = "SELECT * FROM V_PESuratDinasGetComplete"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Try
            If Not IsPostBack Then
                InitProperty()
                ViewState("SetLocation") = False
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
                If ViewState("Sender") = "BtnEmpNo" Then
                    tbEmployee.Text = Session("Result")(0).ToString
                    tbEmpName.Text = Session("Result")(1).ToString
                    tbJobTitle.Text = Session("Result")(2).ToString
                    tbDepartment.Text = Session("Result")(3).ToString
                End If

                If ViewState("Sender") = "BtnBudget" Then
                    tbBudget.Text = Session("Result")(0).ToString
                    tbBudgetName.Text = Session("Result")(1).ToString
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
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
        If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
            ddlCommand.Items.Add("Print")
            ddlCommand2.Items.Add("Print")
        End If
        ddlCommand.Items.Add("Complete")
        ddlCommand2.Items.Add("Complete")


    End Sub

    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim StrFilter, GetString As String
        Try
            GetString = GetStringHd
            StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue, ddlRange.SelectedValue)
            If AdvanceFilter.Length > 1 Then
                StrFilter = AdvanceFilter
            End If
            DT = BindDataTransaction(GetString, StrFilter, ViewState("DBConnection").ToString)
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
        Return "SELECT * From V_PESuratDinasDT WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function
    Private Function GetStringDtJobList(ByVal Nmbr As String) As String
        Return "SELECT * From V_PESuratDinasJob WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function
    Private Function GetStringDtBudget(ByVal Nmbr As String) As String
        Return "SELECT * From V_PESuratDinasBudget WHERE TransNmbr = " + QuotedStr(Nmbr)
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
                Dim QCNo As String

                Pertamax = True
                Result = ""


                For Each GVR In GridView1.Rows
                    CB = GVR.FindControl("cbSelect")
                    If CB.Checked Then
                        If GVR.Cells(3).Text = "P" Then
                            ListSelectNmbr = GVR.Cells(2).Text
                            If Pertamax Then
                                Result = "'''" + ListSelectNmbr + "''"
                                QCNo = GVR.Cells(7).Text.Replace("&nbsp;", "")
                                Pertamax = False
                            Else
                                Result = Result + ",''" + ListSelectNmbr + "''"
                            End If
                        End If
                    End If
                Next
                Result = Result + "'"
                Session("DBConnection") = ViewState("DBConnection")
                Session("SelectCommand") = "EXEC S_PEFormSuratDinas" + Result
                'Session("ReportFile") = ".../../../Rpt/FormRRPO.frx"
                Session("DBConnection") = ViewState("DBConnection")
                AttachScript("openprintdlg();", Page, Me.GetType)

            ElseIf ActionValue = "Complete" Then
                Dim GVR As GridViewRow
                Dim CB As CheckBox

                Dim CekMenu As String = CheckMenuLevel("Complete", ViewState("MenuLevel").Rows(0))
                If CekMenu <> "" Then
                    lbStatus.Text = CekMenu
                    Exit Sub
                End If

                For Each GVR In GridView1.Rows
                    CB = GVR.FindControl("cbSelect")
                    If CB.Checked Then
                        If GVR.Cells(3).Text <> "P" Then
                            lbStatus.Text = MessageDlg("Data must Posted to Complete")
                            Exit Sub
                        Else
                            ViewState("TransNmbr") = GVR.Cells(2).Text
                            ViewState("SetLocation") = True
                            MovePanel(PnlHd, PnlCompleteProcess)
                            FillTextBoxComplete(ViewState("TransNmbr"))
                        End If
                    End If
                Next
            Else
                Status = CekStatus(ActionValue)
                ListSelectNmbr = ""
                GetListCommand(Status, GridView1, "3,2", ListSelectNmbr, Nmbr, lbStatus.Text)
                If ListSelectNmbr = "" Then Exit Sub
                For j = 0 To (Nmbr.Length - 1)
                    If Nmbr(j) = "" Then
                        Exit For
                    Else
                        Result = ExecSPCommandGo(ActionValue, "S_PESuratDinas", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
    Private Sub BindDataDtJob(ByVal Referens As String)
        Try
            Dim dt As New DataTable
            ViewState("DtJob") = Nothing
            dt = SQLExecuteQuery(GetStringDtJobList(Referens), ViewState("DBConnection").ToString).Tables(0)
            ViewState("DtJob") = dt
            BindGridDt(dt, GridDtJob)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub
    Private Sub BindDataDtBudget(ByVal Referens As String)
        Try
            Dim dt As New DataTable
            ViewState("DtBudget") = Nothing
            dt = SQLExecuteQuery(GetStringDtBudget(Referens), ViewState("DBConnection").ToString).Tables(0)
            ViewState("DtBudget") = dt
            BindGridDt(dt, GridDtBudget)
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

    Protected Sub btnCancelDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelDt.Click, btnCanceljob.Click, btnCancelBudget.Click, btnCancelProcess.Click
        Try
            If pnlEditDt.Visible = True Then
                MovePanel(pnlEditDt, pnlDt)
            ElseIf PnlEditJobList.Visible = True Then
                MovePanel(PnlEditJobList, PnlJobList)
            ElseIf PnlEditBudget.Visible = True Then
                MovePanel(PnlEditBudget, PnlBudget)
            End If

            'MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            'EnableHd(GridDt.Rows.Count = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt.Click
        Dim Row As DataRow
        'Dim Result, SQLString As String
        Try
            
            If ViewState("StateDt") = "Edit" Then
                If ViewState("DtValue") <> tbEmployee.Text Then
                    If CekExistData(ViewState("Dt"), "EmpNumb", tbEmployee.Text) Then
                        lbStatus.Text = "Employee " + tbEmpName.Text + "' has been already exist"
                        Exit Sub
                    End If
                End If
                Row = ViewState("Dt").Select("EmpNumb = " + QuotedStr(ViewState("DtValue")))(0)
                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                Row("EmpNumb") = tbEmployee.Text
                Row("Emp_Name") = tbEmpName.Text
                Row("Job_Title") = tbJobTitle.Text
                Row("Department_Name") = tbDepartment.Text
                Row("Destination") = tbDestination.Text
                Row.EndEdit()
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If
                If CekExistData(ViewState("Dt"), "EmpNumb", tbEmployee.Text) Then
                    lbStatus.Text = "Employee " + tbEmpName.Text + "' has been already exist"
                    Exit Sub
                End If

                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("EmpNumb") = tbEmployee.Text
                dr("Emp_Name") = tbEmpName.Text
                dr("Job_Title") = tbJobTitle.Text
                dr("Department_Name") = tbDepartment.Text
                dr("Destination") = tbDestination.Text
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
    Protected Sub btnSaveJob_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveJob.Click
        Dim Row As DataRow
        'Dim Result, SQLString As String
        Try

            If ViewState("StateDtJob") = "Edit" Then
                If ViewState("DtValueJob") <> tbItemNo.Text Then
                    If CekExistData(ViewState("DtJob"), "ItemNo", tbItemNo.Text) Then
                        lbStatus.Text = "Job List " + tbJobList.Text + "' has been already exist"
                        Exit Sub
                    End If
                End If
                Row = ViewState("DtJob").Select("ItemNo = " + QuotedStr(ViewState("DtValueJob")))(0)
                If CekDtJob() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                Row("ItemNo") = tbItemNo.Text
                Row("JobList") = tbJobList.Text
                Row("Target") = tbTarget.Text
                Row("Remark") = tbRemarkJob.Text
                Row.EndEdit()
            Else
                'Insert
                If CekDtJob() = False Then
                    Exit Sub
                End If

                If CekExistData(ViewState("DtJob"), "ItemNo", tbItemNo.Text) Then
                    lbStatus.Text = "Job List " + tbJobList.Text + "' has been already exist"
                    Exit Sub
                End If

                Dim dr As DataRow
                dr = ViewState("DtJob").NewRow
                dr("ItemNo") = tbItemNo.Text
                dr("JobList") = tbJobList.Text
                dr("Target") = tbTarget.Text
                dr("Remark") = tbRemarkJob.Text
                ViewState("DtJob").Rows.Add(dr)
            End If
            MovePanel(PnlEditJobList, PnlJobList)
            EnableHd(GetCountRecord(ViewState("DtJob")) = 0)
            BindGridDt(ViewState("DtJob"), GridDtJob)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save Job List Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub
    Protected Sub btnSaveBudget_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveBudget.Click
        Dim Row As DataRow
        'Dim Result, SQLString As String
        Try
            If CekDtBudget() = False Then
                Exit Sub
            End If
            If ViewState("StateDtBudget") = "Edit" Then
                If ViewState("DtValueBudget") <> tbBudget.Text Then
                    If CekExistData(ViewState("DtBudget"), "BudgetCode", tbBudget.Text) Then
                        lbStatus.Text = "Budget " + tbBudgetName.Text + "' has been already exist"
                        Exit Sub
                    End If
                End If
                Row = ViewState("DtBudget").Select("BudgetCode = " + QuotedStr(ViewState("DtValueBudget")))(0)
                
                Row.BeginEdit()
                Row("BudgetCode") = tbBudget.Text
                Row("BudgetName") = tbBudgetName.Text
                Row("Description") = tbDescription.Text
                Row("QtyPlan") = FormatFloat(tbPlanQty.Text, ViewState("DigitQty"))
                'Row("PricePlan") = tbPlanPrice.Text
                Row("PricePlan") = FormatFloat(tbPlanPrice.Text, ViewState("DigitCurr"))
                Row("TotalPlan") = FormatFloat(tbPlanTotal.Text, ViewState("DigitCurr"))
                Row.EndEdit()
            Else
                'Insert
                If CekExistData(ViewState("DtBudget"), "BudgetCode", tbBudget.Text) Then
                    lbStatus.Text = "Budget " + tbBudgetName.Text + "' has been already exist"
                    Exit Sub
                End If

                Dim dr As DataRow
                dr = ViewState("DtBudget").NewRow
                dr("BudgetCode") = tbBudget.Text
                dr("BudgetName") = tbBudgetName.Text
                dr("Description") = tbDescription.Text
                dr("QtyPlan") = FormatFloat(tbPlanQty.Text, ViewState("DigitQty"))
                dr("PricePlan") = FormatFloat(tbPlanPrice.Text, ViewState("DigitCurr"))
                dr("TotalPlan") = FormatFloat(tbPlanTotal.Text, ViewState("DigitCurr"))
                ViewState("DtBudget").Rows.Add(dr)
            End If
            MovePanel(PnlEditBudget, PnlBudget)
            EnableHd(GetCountRecord(ViewState("DtBudget")) = 0)
            BindGridDt(ViewState("DtBudget"), GridDtBudget)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save Budget Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub
    Private Sub SaveAll()
        Dim SQLString As String
        'Dim I As Integer
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
                tbReference.Text = GetAutoNmbr("ESD", "N", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
                SQLString = "INSERT INTO PEDinasHD (TransNmbr, Status, TransDate, StartDate, StartHour, EndDate, EndHour, " + _
                            "DinasType, CarRequest, CarNo, Driver,Remark," + _
                            "UserPrep,DatePrep,ActStartDate, ActStartHour, ActEndDate, ActEndHour) " + _
                "SELECT " + QuotedStr(tbReference.Text) + ", 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + "," + _
                QuotedStr(tbStartTime.Text) + ", " + QuotedStr(Format(tbEndDate.SelectedValue, "yyyy-MM-dd")) + "," + _
                QuotedStr(tbEndTime.Text) + ", " + QuotedStr(ddlDinasType.Text) + ", " + QuotedStr(ddlCarRequest.Text) + ", " + _
                QuotedStr(tbCarNo.Text) + ", " + QuotedStr(tbDriver.Text) + "," + QuotedStr(tbRemark.Text) + "," + QuotedStr(ViewState("UserId").ToString) + ", getDate()," + _
                QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + "," + _
                QuotedStr(tbStartTime.Text) + ", " + QuotedStr(Format(tbEndDate.SelectedValue, "yyyy-MM-dd")) + "," + _
                QuotedStr(tbEndTime.Text) + ""
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM PEDinasHD WHERE TransNmbr = " + QuotedStr(tbReference.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE PEDinasHD SET TransDate = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "' " + _
                ", StartDate = " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + " " + _
                ", StartHour = " + QuotedStr(tbStartTime.Text) + _
                ", EndDate = " + QuotedStr(Format(tbEndDate.SelectedValue, "yyyy-MM-dd")) + _
                ", EndHour = " + QuotedStr(tbEndTime.Text) + _
                ", DinasType = " + QuotedStr(ddlDinasType.Text) + _
                ", CarRequest = " + QuotedStr(ddlCarRequest.Text) + _
                ", CarNo = " + QuotedStr(tbCarNo.Text) + _
                ", Driver = " + QuotedStr(tbDriver.Text) + _
                ", Remark = " + QuotedStr(tbRemark.Text) + ", UserPrep = " + QuotedStr(ViewState("UserId").ToString) + ", DatePrep = getDate()" + _
                " WHERE TransNmbr = " + QuotedStr(tbReference.Text) + ""
            End If

            SQLString = ChangeQuoteNull(SQLString)
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            If ViewState("StateDt") <> "View" Then
                SaveDtEmployee()
            End If

            If ViewState("StateDtJob") <> "View" Then
                SaveDtJob()
            End If

            If ViewState("StateDtBudget") <> "View" Then
                If GetCountRecord(ViewState("DtBudget")) > 0 Then
                    SaveDtBudget()
                End If
            End If


            ' update Primary Key on Dt
            'Dim Row As DataRow()

            'Row = ViewState("Dt").Select("TransNmbr IS NULL")
            'For I = 0 To Row.Length - 1
            '    Row(I).BeginEdit()
            '    Row(I)("TransNmbr") = tbReference.Text
            '    Row(I).EndEdit()
            'Next

            ''save dt
            'Dim ConnString As String = ViewState("DBConnection").ToString 'System.Configuration.ConfigurationManager.AppSettings.Get("ConString")
            'con = New SqlConnection(ConnString)
            'con.Open()
            'Dim cmdSql As New SqlCommand("SELECT EmpNumb,Destination FROM PEDinasDt WHERE TransNmbr = " + QuotedStr(tbReference.Text), con)
            'da = New SqlDataAdapter(cmdSql)
            'Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            'da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            'Dim Dt As New DataTable("{PEDinasDt")

            'Dt = ViewState("Dt")
            'da.Update(Dt)
            'Dt.AcceptChanges()
            'ViewState("Dt") = Dt


            'ConnString = ViewState("DBConnection").ToString 'System.Configuration.ConfigurationManager.AppSettings.Get("ConString")
            'con = New SqlConnection(ConnString)
            'con.Open()
            'cmdSql = New SqlCommand("SELECT Item,JobList,Target, Remark FROM PEDinasJob WHERE TransNmbr = " + QuotedStr(tbReference.Text), con)
            'da = New SqlDataAdapter(cmdSql)
            'dbcommandBuilder = New SqlCommandBuilder(da)

            'da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            'Dt = New DataTable("{PEDinasJob")

            'Dt = ViewState("DtJob")
            'da.Update(Dt)
            'Dt.AcceptChanges()
            'ViewState("DtJob") = Dt


            'ConnString = ViewState("DBConnection").ToString 'System.Configuration.ConfigurationManager.AppSettings.Get("ConString")
            'con = New SqlConnection(ConnString)
            'con.Open()
            'cmdSql = New SqlCommand("SELECT BudgetCode,Description,QtyPlan,PricePlan," + _
            '                        "TotalPlan,Remark FROM PEDinasBudget WHERE TransNmbr = " + QuotedStr(tbReference.Text), con)
            'da = New SqlDataAdapter(cmdSql)
            'dbcommandBuilder = New SqlCommandBuilder(da)

            'da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            'Dt = New DataTable("{PEDinasBudget")

            'Dt = ViewState("DtBudget")
            'da.Update(Dt)
            'Dt.AcceptChanges()
            'ViewState("DtBudget") = Dt

        Catch ex As Exception
            Throw New Exception("Save All Data Error : " + ex.ToString)
        End Try
    End Sub


    Private Sub SaveDtEmployee()
        Dim SQLString As String
        Try

            If ViewState("StateDt") = "Insert" Then
                SQLString = "INSERT INTO PEDinasDt(TransNmbr, EmpNumb, Destination) " + _
                "SELECT " + QuotedStr(tbReference.Text) + ", " + QuotedStr(tbEmployee.Text) + ", " + QuotedStr(tbDestination.Text)
            Else
                SQLString = "UPDATE PEDinasDt SET EmpNumb = " + QuotedStr(tbEmployee.Text) + ", Destination = " + QuotedStr(tbDestination.Text) + " " + _
                " WHERE TransNmbr = " + QuotedStr(tbReference.Text) + " AND EmpNumb = " + QuotedStr(tbEmployee.Text) + " "
            End If
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            pnlEditDt.Visible = False
            pnlDt.Visible = True
            EnableHd(GridDt.Rows.Count = 0)
            BindDataDt(QuotedStr(tbReference.Text))
            GridDt.DataSource = ViewState("Dt")
            GridDt.DataBind()
        Catch ex As Exception
            Throw New Exception("Save Employee Data Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub SaveDtJob()
        Dim SQLString As String
        Try

            If ViewState("StateDtJob") = "Insert" Then
                SQLString = "INSERT INTO PEDinasJob(TransNmbr,ItemNo,JobList,Target,Remark) " + _
                "SELECT " + QuotedStr(tbReference.Text) + ", " + QuotedStr(tbItemNo.Text) + ", " + QuotedStr(tbJobList.Text) + "," + QuotedStr(tbTarget.Text) + "," + QuotedStr(tbRemark.Text)
            Else
                SQLString = "UPDATE PEDinasJob SET JobList = " + QuotedStr(tbJobList.Text) + ", " + _
                " Target = " + QuotedStr(tbTarget.Text) + ", Remark = " + QuotedStr(tbRemarkJob.Text) + _
                " WHERE TransNmbr = " + QuotedStr(tbReference.Text) + " AND ItemNo = " + QuotedStr(tbItemNo.Text) + " "
            End If
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            PnlEditJobList.Visible = False
            PnlJobList.Visible = True
            EnableHd(GridDtJob.Rows.Count = 0)
            BindDataDtJob(QuotedStr(tbReference.Text))
            GridDtJob.DataSource = ViewState("DtJob")
            GridDtJob.DataBind()
        Catch ex As Exception
            Throw New Exception("Save Job Data Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub SaveDtBudget()
        Dim SQLString As String
        Try

            If ViewState("StateDtBudget") = "Insert" Then
                SQLString = "INSERT INTO PEDinasBudget(TransNmbr,BudgetCode,Description,QtyPlan,PricePlan,TotalPlan) " + _
                "SELECT " + QuotedStr(tbReference.Text) + ", " + QuotedStr(tbBudget.Text) + "," + QuotedStr(tbDescription.Text) + "," + tbPlanQty.Text.Replace(",", "") + "," + tbPlanPrice.Text.Replace(",", "") + "," + tbPlanTotal.Text.Replace(",", "")
            Else
                SQLString = "UPDATE PEDinasBudget SET BudgetCode = " + QuotedStr(tbBudget.Text) + ", Description = " + QuotedStr(tbDescription.Text) + ", " + _
                " QtyPlan = " + tbPlanQty.Text.Replace(",", "") + ", PricePlan = " + tbPlanPrice.Text.Replace(",", "") + ", TotalPlan = " + tbPlanTotal.Text.Replace(",", "") + _
                " WHERE TransNmbr = " + QuotedStr(tbReference.Text) + " AND BudgetCode = " + QuotedStr(tbBudget.Text) + " "
            End If
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            PnlEditBudget.Visible = False
            PnlBudget.Visible = True
            EnableHd(GridDtBudget.Rows.Count = 0)
            BindDataDtBudget(QuotedStr(tbReference.Text))
            GridDtBudget.DataSource = ViewState("DtBudget")
            GridDtBudget.DataBind()
        Catch ex As Exception
            Throw New Exception("Save Budget Data Error : " + ex.ToString)
        End Try
    End Sub


    Protected Sub btnSaveTrans_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveTrans.Click
        Dim CurrFilter, Value As String
        Try
            If CekHd() = False Then
                Exit Sub
            End If


            If ViewState("StateDt") <> "View" Then

                If GetCountRecord(ViewState("Dt")) = 0 Then
                    lbStatus.Text = MessageDlg("Detail must have at least 1 record")
                    Exit Sub
                End If

                For Each dr In ViewState("Dt").Rows
                    If CekDt(dr) = False Then
                        Exit Sub
                    End If
                Next
            End If


            If ViewState("StateDtJob") <> "View" Then
                If GetCountRecord(ViewState("DtJob")) = 0 Then
                    lbStatus.Text = MessageDlg("Detail Job must have at least 1 record")
                    Exit Sub
                End If

                For Each dr In ViewState("DtJob").Rows
                    If CekDtJob(dr) = False Then
                        Exit Sub
                    End If
                Next
            End If


            If ViewState("StateDtBudget") <> "View" Then
                'If GetCountRecord(ViewState("DtBudget")) = 0 Then
                '    lbStatus.Text = MessageDlg("Detail Budget must have at least 1 record")
                '    Exit Sub
                'End If
                If GetCountRecord(ViewState("DtBudget")) > 0 Then
                    For Each dr In ViewState("DtBudget").Rows
                        If CekDtBudget(dr) = False Then
                            Exit Sub
                        End If
                    Next
                End If
                
            End If

            SaveAll()


            MovePanel(pnlInput, PnlHd)
            CurrFilter = tbFilter.Text
            Value = ddlField.SelectedValue
            tbFilter.Text = tbReference.Text
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
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, pnlDt, GridDt)
            newTrans()
            btnHome.Visible = False
            btnadddt.Visible = True
            btnAddJobList.Visible = True
            btnAddBudget.Visible = True
            tbDate.Focus()

        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ClearHd()
            Cleardt()
            MultiView1.ActiveViewIndex = 0
            Menu1.Items.Item(0).Selected = True

            tbDate.SelectedDate = ViewState("ServerDate") 'Today
            pnlDt.Visible = True
            BindDataDt("")
            BindDataDtJob("")
            BindDataDtBudget("")
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbReference.Text = ""
            tbRemark.Text = ""
            tbStartDate.SelectedDate = ViewState("ServerDate")
            tbEndDate.SelectedDate = ViewState("ServerDate")
            tbStartTime.Text = "00:00"
            tbEndTime.Text = "00:00"
            ddlCarRequest.SelectedValue = "N"
            tbCarNo.Text = ""
            tbDriver.Text = ""
            tbRemark.Text = ""
            ddlCarRequest_SelectedIndexChanged(Nothing, Nothing)
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbEmployee.Text = ""
            tbEmpName.Text = ""
            tbJobTitle.Text = ""
            tbDepartment.Text = ""
            tbDestination.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub
    Private Sub CleardtJob()
        Try
            tbItemNo.Text = ""
            tbJobList.Text = ""
            tbTarget.Text = ""
            tbRemarkJob.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear job Error " + ex.ToString)
        End Try
    End Sub
    Private Sub CleardtBudget()
        Try
            tbBudget.Text = ""
            tbBudgetName.Text = ""
            tbDescription.Text = ""
            tbPlanQty.Text = "0"
            tbPlanPrice.Text = "0"
            tbPlanTotal.Text = "0"
        Catch ex As Exception
            Throw New Exception("Clear Budget Error " + ex.ToString)
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
                lbStatus.Text = MessageDlg("Detail Employee must have at least 1 record")
                Exit Sub
            End If
            For Each dr In ViewState("Dt").Rows
                If CekDt(dr) = False Then
                    Exit Sub
                End If
            Next

            If IsNothing(ViewState("DtJob")) Then
                lbStatus.Text = MessageDlg("Detail Job must have at least 1 record")
                Exit Sub
            End If
            For Each dr In ViewState("DtJob").Rows
                If CekDtJob(dr) = False Then
                    Exit Sub
                End If
            Next

            'If IsNothing(ViewState("DtBudget")) Then
            '    lbStatus.Text = MessageDlg("Detail Budget must have at least 1 record")
            '    Exit Sub
            'End If
            For Each dr In ViewState("DtBudget").Rows
                If CekDtBudget(dr) = False Then
                    Exit Sub
                End If
            Next

            SaveAll()
            newTrans()
        Catch ex As Exception
            lbStatus.Text = "btn save all error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnEmpNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEmployee.Click
        Dim ResultField As String
        Try

            Session("Filter") = " SELECT Emp_No, Emp_Name, Job_Title, Job_Title_Name, Department_Name " + _
                                " FROM V_MsEmployee WHERE Fg_Active = 'Y' "
            ResultField = "Emp_No,Emp_Name,Job_Title_Name,Department_Name"
            ViewState("Sender") = "BtnEmpNo"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Employee Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnBudget_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBudget.Click
        Dim ResultField As String
        Try

            Session("Filter") = " SELECT Budget_Code,Budget_Name" + _
                                " FROM VMsBudget "
            ResultField = "Budget_Code,Budget_Name"
            ViewState("Sender") = "BtnBudget"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Employee Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub tbBudget_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbBudget.TextChanged
        Dim Dr As DataRow
        Dim DT As DataTable
        Dim SQLString As String
        Try

            SQLString = " SELECT A.Budget_Code,A.Budget_Name" + _
                        " FROM VMsBudget A WHERE Budget_Code = " + QuotedStr(tbBudget.Text)
            DT = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If DT.Rows.Count > 0 Then
                Dr = DT.Rows(0)
            Else
                Dr = Nothing
            End If

            If Not Dr Is Nothing Then
                BindToText(tbBudget, Dr("Budget_Code").ToString)
                BindToText(tbBudgetName, Dr("Budget_Name").ToString)
            Else
                tbBudget.Text = ""
                tbBudgetName.Text = ""
            End If
            tbBudget.Focus()
        Catch ex As Exception
            Throw New Exception("tb Budget Code TextChanged : " + ex.ToString)
        End Try
    End Sub
    Protected Sub tbEmpNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbEmployee.TextChanged
        Dim Dr As DataRow
        Dim DT As DataTable
        Dim SQLString As String
        Try
            SQLString = " SELECT Emp_No, Emp_Name, Job_Title, Job_Title_Name, Department_Name" + _
                        " FROM V_MsEmployee WHERE Fg_Active = 'Y' AND Emp_No = " + QuotedStr(tbEmployee.Text)
            DT = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If DT.Rows.Count > 0 Then
                Dr = DT.Rows(0)
            Else
                Dr = Nothing
            End If

            If Not Dr Is Nothing Then
                BindToText(tbEmployee, Dr("Emp_No").ToString)
                BindToText(tbEmpName, Dr("Emp_Name").ToString)
                BindToText(tbJobTitle, Dr("Job_Title_Name").ToString)
                BindToText(tbDepartment, Dr("Department_Name").ToString)
            Else
                tbEmployee.Text = ""
                tbEmpName.Text = ""
                tbJobTitle.Text = ""
                tbDepartment.Text = ""
            End If
            tbEmployee.Focus()
        Catch ex As Exception
            Throw New Exception("tb Employee Code TextChanged : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnadddt.Click
        'Dim i As Integer
        'Dim dt As New DataTable

        Cleardt()
        If CekHd() = False Then
            '    Exit Sub
        End If
        MovePanel(pnlDt, pnlEditDt)
        EnableHd(False)

        'Dim Row As DataRow()
        'If ViewState("StateHd") = "Insert" Then
        '    Row = Nothing
        '    i = 0
        'Else
        '    Row = ViewState("Dt").select("")
        '    i = Row.Length
        'End If

        ViewState("StateDt") = "Insert"
        'If i > 0 Then
        '    tbItemNo.Text = (CInt(Row(i - 1)("ItemNo").ToString) + 1).ToString
        'Else
        '    tbItemNo.Text = "1"
        'End If
        StatusButtonSave(False)
    End Sub

    Protected Sub btnAddJobList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddJobList.Click

        'If CheckMenuLevel("Insert", ViewState("MenuLevel")) = False Then
        '    Exit Sub
        'End If
        CleardtJob()
        Dim maxItem As Integer
        maxItem = GetNewItemNo(ViewState("DtJob"))
        ViewState("StateDtJob") = "Insert"
        tbItemNo.Text = maxItem.ToString
        tbJobList.Focus()
        MovePanel(PnlJobList, PnlEditJobList)
        EnableHd(False)
        StatusButtonSave(False)
    End Sub

    Protected Sub btnAddBudget_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddBudget.Click

        '{If CheckMenuLevel("Insert", ViewState("MenuLevel")) = False Then
        'Exit Sub
        'End If}
        CleardtBudget()
        Dim i As Integer
        Dim dt As New DataTable

        Dim Row As DataRow()
        If ViewState("StateHd") = "Insert" Then
            Row = Nothing
            i = 0
        Else
            Row = ViewState("Dt").select("")
            i = Row.Length
        End If

        ViewState("StateDtBudget") = "Insert"
        MovePanel(PnlBudget, PnlEditBudget)
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
            'If CInt(ViewState("GLYear")) <> Year(tbDate.SelectedValue) Or CInt(ViewState("GLPeriod")) <> Month(tbDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(ViewState("GLPeriod")) + " " + ViewState("GLYear").ToString.Trim)
            '    Exit Function
            'End If
            If tbStartDate.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Start Date must have value")
                tbStartDate.Focus()
                Return False
            End If
            If tbEndDate.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("End Date must have value")
                tbEndDate.Focus()
                Return False
            End If
            If tbEndDate.SelectedDate < tbStartDate.SelectedDate Then
                lbStatus.Text = MessageDlg("End Date must Greater Or Equal To Start Date")
                tbEndDate.Focus()
                Return False
            End If
            If IsTime(tbStartTime.Text) = False Then
                lbStatus.Text = MessageDlg("Start Time must be input valid time")
                tbStartTime.Focus()
                Return False
            End If
            If IsTime(tbEndTime.Text.Trim) = False Then
                lbStatus.Text = MessageDlg("End Time must be input valid time")
                tbEndTime.Focus()
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
                    lbStatus.Text = MessageDlg("Employee Must Have Value")
                    Return False
                End If
                If Dr("Destination").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Destination Must Have Value")
                    Return False
                End If
            Else
                If tbEmployee.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Employee Must Have Value")
                    tbEmployee.Focus()
                    Return False
                End If
                If tbDestination.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Destination Must Have Value")
                    tbDestination.Focus()
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function

    Function CekDtJob(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not (Dr Is Nothing) Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("JobList").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Job List Must Have Value")
                    Return False
                End If
                If Dr("Target").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Target Must Have Value")
                    Return False
                End If
            Else
                If tbJobList.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Employee Must Have Value")
                    tbEmployee.Focus()
                    Return False
                End If
                If tbTarget.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Target Must Have Value")
                    tbDestination.Focus()
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Job Error : " + ex.ToString)
        End Try
    End Function

    Function CekDtBudget(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not (Dr Is Nothing) Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("BudgetCode").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Job List Must Have Value")
                    Return False
                End If
                If Dr("Description").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Target Must Have Value")
                    Return False
                End If
                If Dr("QtyPlan").ToString.Trim = "0" Then
                    lbStatus.Text = MessageDlg("Qty Plan Must Have Value")
                    Return False
                End If
                If Dr("PricePlan").ToString.Trim = "0" Then
                    lbStatus.Text = MessageDlg("Price Plan Must Have Value")
                    Return False
                End If

            Else
                If tbBudget.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Budget Must Have Value")
                    tbBudget.Focus()
                    Return False
                End If
                If tbDescription.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Description Must Have Value")
                    tbDestination.Focus()
                    Return False
                End If
                If CFloat(tbPlanQty.Text.Trim) = "0" Then
                    lbStatus.Text = MessageDlg("Plan Qty Must Have Value")
                    tbEmployee.Focus()
                    Return False
                End If
                If CFloat(tbPlanPrice.Text.Trim) = "0" Then
                    lbStatus.Text = MessageDlg("Plan Price Must Have Value")
                    tbEmployee.Focus()
                    Return False
                End If


            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Job Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        CheckAll(GridView1, sender)
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
                    FillTextBoxHd(ViewState("TransNmbr"))
                    BindDataDt(ViewState("TransNmbr"))
                    GridDt.PageIndex = 0
                    BindDataDtJob(ViewState("TransNmbr"))
                    GridDtJob.PageIndex = 0
                    BindDataDtBudget(ViewState("TransNmbr"))
                    GridDtBudget.PageIndex = 0
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, pnlDt, GridDt)
                    ModifyInput2(False, pnlInput, PnlJobList, GridDtJob)
                    ModifyInput2(False, pnlInput, PnlBudget, GridDtBudget)
                    Menu1.Visible = True
                    Menu1.Items.Item(0).Selected = True
                    MultiView1.Visible = True
                    MultiView1.ActiveViewIndex = 0
                    btnHome.Visible = True
                    btnadddt.Visible = False
                    btnAddJobList.Visible = False
                    btnAddBudget.Visible = False
                ElseIf DDL.SelectedValue = "Edit" Then
                    If GVR.Cells(3).Text = "H" Or GVR.Cells(3).Text = "G" Then
                        CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        ViewState("SetLocation") = True
                        MovePanel(PnlHd, pnlInput)
                        ViewState("TransNmbr") = GVR.Cells(2).Text
                        BindDataDt(ViewState("TransNmbr"))
                        GridDt.PageIndex = 0
                        BindDataDtJob(ViewState("TransNmbr"))
                        GridDtJob.PageIndex = 0
                        BindDataDtBudget(ViewState("TransNmbr"))
                        GridDtBudget.PageIndex = 0
                        FillTextBoxHd(ViewState("TransNmbr"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        ModifyInput2(True, pnlInput, PnlJobList, GridDtJob)
                        ModifyInput2(True, pnlInput, PnlBudget, GridDtBudget)
                        btnHome.Visible = False
                        btnadddt.Visible = True
                        btnAddJobList.Visible = True
                        btnAddBudget.Visible = True
                        ViewState("StateDt") = "View"
                        ViewState("StateDtJob") = "View"
                        ViewState("StateDtBudget") = "View"
                        'ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, ViewState("DBConnection").ToString)
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                        ddlCarRequest_SelectedIndexChanged(Nothing, Nothing)
                        Menu1.Visible = True
                        Menu1.Items.Item(0).Selected = True
                        MultiView1.Visible = True
                        MultiView1.ActiveViewIndex = 0
                    Else
                        lbStatus.Text = MessageDlg("Data must Hold or Get Approval to edit")
                        Exit Sub
                    End If
                ElseIf DDL.SelectedValue = "Complete" Then
                    If GVR.Cells(3).Text = "P" Then
                        CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        ViewState("TransNmbr") = GVR.Cells(2).Text
                        ViewState("SetLocation") = True
                        MovePanel(PnlHd, PnlCompleteProcess)

                        FillTextBoxComplete(ViewState("TransNmbr"))
                        'PnlCompleteProcess.Visible = True

                        'ViewState("TransNmbr") = GVR.Cells(2).Text
                        'BindDataDt(ViewState("TransNmbr"))
                        'GridDt.PageIndex = 0
                        'BindDataDtJob(ViewState("TransNmbr"))
                        'GridDtJob.PageIndex = 0
                        'BindDataDtBudget(ViewState("TransNmbr"))
                        'GridDtBudget.PageIndex = 0
                        'FillTextBoxHd(ViewState("TransNmbr"))
                        'ViewState("StateHd") = "Edit"
                        'ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        'ModifyInput2(True, pnlInput, PnlJobList, GridDtJob)
                        'ModifyInput2(True, pnlInput, PnlBudget, GridDtBudget)
                        'btnHome.Visible = False
                        'ViewState("StateDt") = "View"
                        'ViewState("StateDtJob") = "View"
                        'ViewState("StateDtBudget") = "View"
                        ''ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, ViewState("DBConnection").ToString)
                        'EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                        'ddlCarRequest_SelectedIndexChanged(Nothing, Nothing)
                        'Menu1.Visible = True
                        'Menu1.Items.Item(0).Selected = True
                        'MultiView1.Visible = True
                        'MultiView1.ActiveViewIndex = 0

                    Else
                        lbStatus.Text = MessageDlg("Data must Posted to Complete")
                        Exit Sub
                    End If
                ElseIf DDL.SelectedValue = "Print" Then
                    CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                    If CekMenu <> "" Then
                        lbStatus.Text = CekMenu
                        Exit Sub
                    End If
                    Dim QCNo, StatusPrint As String
                    Try
                        If GVR.Cells(3).Text = "P" Then
                            QCNo = GVR.Cells(7).Text.Replace("&nbsp;", "")
                            StatusPrint = GVR.Cells(3).Text.Replace("&nbsp;", "")
                            Session("SelectCommand") = "EXEC S_STFormRRPO ''" + QuotedStr(GVR.Cells(2).Text) + "''"
                            'If QCNo <> "" Then
                            '    Session("ReportFile") = ".../../../Rpt/FormRRPONonQC.frx"
                            'Else
                            Session("ReportFile") = ".../../../Rpt/FormRRPO.frx"
                            'End If
                            Session("DBConnection") = ViewState("DBConnection")
                            AttachScript("openprintdlg();", Page, Me.GetType)
                        Else
                            lbStatus.Text = "Cannot print or preview, status RR No " + QuotedStr(GVR.Cells(2).Text) + " must be posted"
                            Exit Sub
                        End If
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
    Protected Sub GridDtJob_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridDtJob.PageIndexChanging
        Try
            GridDtJob.PageIndex = e.NewPageIndex
            GridDtJob.DataSource = ViewState("DtJob")
            GridDtJob.DataBind()
        Catch ex As Exception
            lbStatus.Text = "Grid dT Job Page Index Change Error : " + ex.ToString + " page index : " + e.NewPageIndex.ToString
        End Try
    End Sub

    Protected Sub GridDtBudget_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridDtBudget.PageIndexChanging
        Try
            GridDtBudget.PageIndex = e.NewPageIndex
            GridDtBudget.DataSource = ViewState("DtBudget")
            GridDtBudget.DataBind()
        Catch ex As Exception
            lbStatus.Text = "Grid dT Budget Page Index Change Error : " + ex.ToString + " page index : " + e.NewPageIndex.ToString
        End Try
    End Sub

    Protected Sub GridDtJob_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridDtJob.RowCommand
        Try
            If e.CommandName = "Insert" Then
                btnAddDt_Click(Nothing, Nothing)
                'btnGetDt.Enabled = False
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDtBudget_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridDtBudget.RowCommand
        Try
            If e.CommandName = "Insert" Then
                btnAddDt_Click(Nothing, Nothing)
                'btnGetDt.Enabled = False
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridDt.RowCommand
        Try
            If e.CommandName = "Insert" Then
                btnAddDt_Click(Nothing, Nothing)
                'btnGetDt.Enabled = False
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Dim dr() As DataRow
        'Dim lb As Label
        Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
        'lb = GVR.FindControl("lbLocation")
        dr = ViewState("Dt").Select("EmpNumb = " + QuotedStr(GVR.Cells(1).Text))
        dr(0).Delete()
        'EnableHd(GetCountRecord(ViewState("Dt")) = 0)
        BindGridDt(ViewState("Dt"), GridDt)
        EnableHd(GetCountRecord(ViewState("Dt")) <> 0)
    End Sub

    Protected Sub GridDtJob_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDtJob.RowDeleting
        Dim dr() As DataRow
        Dim GVR As GridViewRow = GridDtJob.Rows(e.RowIndex)
        dr = ViewState("DtJob").Select("ItemNo = " + QuotedStr(GVR.Cells(1).Text))
        dr(0).Delete()
        BindGridDt(ViewState("DtJob"), GridDtJob)
        EnableHd(GetCountRecord(ViewState("DtJob")) <> 0)
    End Sub

    Protected Sub GridDtBudget_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDtBudget.RowDeleting
        Dim dr() As DataRow
        Dim GVR As GridViewRow = GridDtBudget.Rows(e.RowIndex)
        dr = ViewState("DtBudget").Select("BudgetCode = " + QuotedStr(GVR.Cells(1).Text))
        dr(0).Delete()
        BindGridDt(ViewState("DtBudget"), GridDtBudget)
        EnableHd(GetCountRecord(ViewState("DtBudget")) <> 0)
    End Sub
    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        'Dim lb As Label
        Try
            GVR = GridDt.Rows(e.NewEditIndex)

            ViewState("DtValue") = GVR.Cells(1).Text
            FillTextBoxDt(ViewState("DtValue"))
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"

            tbEmployee.Focus()
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDtJob_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDtJob.RowEditing
        Dim GVR As GridViewRow
        'Dim lb As Label
        Try
            GVR = GridDtJob.Rows(e.NewEditIndex)
            ViewState("DtValueJob") = GVR.Cells(1).Text
            FillTextBoxDtJob(ViewState("DtValueJob"))
            MovePanel(PnlJobList, PnlEditJobList)
            EnableHd(False)
            ViewState("StateDtJob") = "Edit"
            tbBudget.Focus()
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub GridDtBudget_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDtBudget.RowEditing
        Dim GVR As GridViewRow
        'Dim lb As Label
        Try
            GVR = GridDtBudget.Rows(e.NewEditIndex)
            ViewState("DtValueBudget") = GVR.Cells(1).Text
            FillTextBoxDtBudget(ViewState("DtValueBudget"))
            MovePanel(PnlBudget, PnlEditBudget)
            EnableHd(False)
            ViewState("StateDtBudget") = "Edit"
            tbBudget.Focus()
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub
   
    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            tbReference.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToDate(tbStartDate, Dt.Rows(0)("StartDate").ToString)
            BindToDate(tbEndDate, Dt.Rows(0)("EndDate").ToString)
            BindToText(tbStartTime, Dt.Rows(0)("StartHour").ToString)
            BindToText(tbEndTime, Dt.Rows(0)("EndHour").ToString)
            BindToDropList(ddlCarRequest, Dt.Rows(0)("CarRequest").ToString)
            BindToText(tbCarNo, Dt.Rows(0)("CarNo").ToString)
            BindToText(tbDriver, Dt.Rows(0)("Driver").ToString)
            'If ddlCarRequest.SelectedValue = "Y" Then
            '    tbCarNo.Visible = True
            '    tbDriver.Visible = True
            'Else
            '    tbCarNo.Visible = False
            '    tbDriver.Visible = False
            'End If
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxComplete(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringComplete, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            tbCompSuratDinas.Text = Nmbr


            dpCompDate.SelectedDate = ViewState("ServerDate")
            BindToDate(tbDateTrans, Dt.Rows(0)("Transdate").ToString)
            BindToText(tbCompJobList, Dt.Rows(0)("JobList").ToString)
            BindToText(tbCompTarget, Dt.Rows(0)("Target").ToString)
            BindToText(tbCompReal, Dt.Rows(0)("Realisasi").ToString)
            BindToText(tbCompBudget, Dt.Rows(0)("BudgetName").ToString)
            tbCompActQty.Text = "0"
            tbCompActPrice.Text = "0"
            tbCompActTotal.Text = "0"
            tbCompReal.Focus()

        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxDt(ByVal EmpNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("EmpNumb = " + QuotedStr(EmpNo))
            If Dr.Length > 0 Then
                BindToText(tbEmployee, Dr(0)("EmpNumb").ToString)
                BindToText(tbEmpName, Dr(0)("Emp_Name").ToString)
                BindToText(tbJobTitle, Dr(0)("Job_Title").ToString)
                BindToText(tbDepartment, Dr(0)("Department_Name").ToString)
                BindToText(tbDestination, Dr(0)("Destination").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxDtJob(ByVal Item As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("DtJob").select("ItemNo = " + QuotedStr(Item))
            If Dr.Length > 0 Then
                BindToText(tbItemNo, Dr(0)("ItemNo").ToString)
                BindToText(tbJobList, Dr(0)("JobList").ToString)
                BindToText(tbTarget, Dr(0)("Target").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxDtBudget(ByVal Product As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("DtBudget").select("BudgetCode = " + QuotedStr(Product))
            If Dr.Length > 0 Then
                BindToText(tbBudget, Dr(0)("BudgetCode").ToString)
                BindToText(tbBudgetName, Dr(0)("BudgetName").ToString)
                BindToText(tbDescription, Dr(0)("Description").ToString)
                BindToText(tbPlanQty, Dr(0)("QtyPlan").ToString)
                BindToText(tbPlanPrice, Dr(0)("PricePlan").ToString)
                BindToText(tbPlanTotal, Dr(0)("TotalPlan").ToString)
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




    Protected Sub Page_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        Try
            Session("SelectCommand") = Nothing
            Session("ReportFile") = Nothing
            Session("PrintType") = Nothing
            WebReport1.Dispose()
        Catch ex As Exception
            lbStatus.Text = "page disposed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        MultiView1.ActiveViewIndex = Int32.Parse(e.Item.Value)
        Dim i As Integer


        For i = 0 To Menu1.Items.Count - 1
            If i = e.Item.Value Then
           
            End If
        Next
    End Sub



    Protected Sub ddlCarRequest_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCarRequest.SelectedIndexChanged
        Try
            If ddlCarRequest.SelectedValue = "Y" Then
                tbCarNo.Enabled = True
                tbDriver.Enabled = True
            Else
                tbCarNo.Text = ""
                tbDriver.Text = ""
                tbCarNo.Enabled = False
                tbDriver.Enabled = False
            End If

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnCancelProcess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelProcess.Click
        Try
            MovePanel(PnlCompleteProcess, PnlHd)
            lbStatus.Text = "a"
            Exit Sub
        Catch ex As Exception
            lbStatus.Text = "btn back Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCompleteProcess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCompleteProcess.Click
        Dim Hasil As String
        Try
            Dim sqlstring As String

            If dpCompDate.SelectedDate < tbDateTrans.SelectedDate Then
                lbStatus.Text = "Complete Date Must Equal or Greater Than Transaction Date (" + Format(tbDateTrans.SelectedValue, "dd MMMM yyyy") + ")"
                Exit Sub
            End If

            If tbCompBudget.Text.Trim <> "" Then
                If CFloat(tbCompActQty.Text) = 0 Then
                    lbStatus.Text = "Actual Qty Must Greater Than 0"
                    tbCompActQty.Focus()
                    Exit Sub
                End If

                If CFloat(tbCompActPrice.Text) = 0 Then
                    lbStatus.Text = "Actual Price Must Greater Than 0"
                    tbCompActPrice.Focus()
                    Exit Sub
                End If
            End If
            


            'Update PEDinasJob
            sqlstring = "UPDATE PEDinasJob SET Realisasi = " + QuotedStr(tbCompReal.Text) + _
                         "WHERE Transnmbr = " + QuotedStr(tbCompSuratDinas.Text)
            SQLExecuteNonQuery(sqlstring, ViewState("DBConnection").ToString)


            If tbCompBudget.Text.Trim <> "" Then
                'Update PEDinasBudget
                sqlstring = "UPDATE PEDinasBudget SET QtyAct = " + tbCompActQty.Text.Replace(",", "") + _
                            ",PriceAct = " + tbCompActPrice.Text.Replace(",", "") + _
                            ",TotalAct = " + tbCompActTotal.Text.Replace(",", "") + _
                            " WHERE Transnmbr = " + QuotedStr(tbCompSuratDinas.Text)

                SQLExecuteNonQuery(sqlstring, ViewState("DBConnection").ToString)
            End If
            

            sqlstring = "DECLARE @A VARCHAR(255) " + _
                        "EXEC S_PESuratDinasComplete " + QuotedStr(tbCompSuratDinas.Text) + "," + QuotedStr(ViewState("ServerDate")) + "," + QuotedStr(ViewState("UserId")) + ", @A OUT " + _
                        "SELECT @A"

            Hasil = SQLExecuteScalar(sqlstring, ViewState("DBConnection"))
            If Hasil.Length > 1 Then
                lbStatus.Text = Hasil
                Exit Sub
            End If

            btnCancelProcess_Click(Nothing, Nothing)
            btnSearch_Click(Nothing, Nothing)

        Catch ex As Exception

        End Try
    End Sub

   
    Protected Sub tbPlanQty_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPlanQty.TextChanged
        Try


            tbPlanTotal.Text = FormatFloat((CFloat(tbPlanQty.Text) * CFloat(tbPlanPrice.Text)).ToString, ViewState("DigitCurr"))


        Catch ex As Exception
            lbStatus.Text = " Plan Qty Text Change Error"
        End Try

    End Sub

    Protected Sub tbPlanPrice_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPlanPrice.TextChanged
        Try


            tbPlanTotal.Text = FormatFloat((CFloat(tbPlanQty.Text) * CFloat(tbPlanPrice.Text)).ToString, ViewState("DigitCurr"))



        Catch ex As Exception
            lbStatus.Text = " Plan Price Text Change Error"
        End Try
    End Sub


    Protected Sub tbCompActQty_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCompActQty.TextChanged
        Try


            tbCompActTotal.Text = FormatFloat((CFloat(tbCompActQty.Text) * CFloat(tbCompActPrice.Text)).ToString, ViewState("DigitCurr"))


        Catch ex As Exception
            lbStatus.Text = "Actual Qty  Text Change Error"
        End Try

    End Sub



    Protected Sub tbCompActPrice_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCompActPrice.TextChanged
        Try


            tbCompActTotal.Text = FormatFloat((CFloat(tbCompActQty.Text) * CFloat(tbCompActPrice.Text)).ToString, ViewState("DigitCurr"))


        Catch ex As Exception
            lbStatus.Text = " Actual Price Text Change Error"
        End Try

    End Sub
End Class
