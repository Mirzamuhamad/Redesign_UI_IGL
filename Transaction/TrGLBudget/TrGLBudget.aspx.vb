Imports System.Data
Imports System.Data.SqlClient
Partial Class Transaction_TrGLBudget_TrGLBudget
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select * From V_GLBudgetHd"


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
                If ViewState("Sender") = "btnAccount" Then
                    tbAccount.Text = Session("Result")(0).ToString
                    tbDescription.Text = Session("Result")(1).ToString
                    ddlCurrency.SelectedValue = Session("Result")(2).ToString
                    If Session("Result")(3).ToString = "Y" Then
                        ddlCostCtr.Enabled = True
                        ddlCostCtr.SelectedIndex = 1
                    Else
                        ddlCostCtr.Enabled = False
                        ddlCostCtr.SelectedIndex = 0
                    End If
                    GetPrevAmount(ddlYear.SelectedValue, ddlPeriod.SelectedValue, tbAccount.Text)
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
            FillCombo(ddlPeriod, "EXEC S_GetPeriod", False, "Period", "Description", ViewState("DBConnection"))
            FillCombo(ddlCostCtr, "EXEC S_GetCostCtr", True, "Cost_Ctr_Code", "Cost_Ctr_Name", ViewState("DBConnection"))
            FillCombo(ddlCurrency, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
            ViewState("SortExpression") = Nothing
            ViewState("DigitCurr") = 0
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
            If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
                'ddlCommand.Items.Add("Print")
                'ddlCommand2.Items.Add("Print")
            End If

            tbAmountPrev.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbActualPrev.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbAmount.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbAmountPrev.Attributes.Add("OnBlur", "setformatdt();")
            tbActualPrev.Attributes.Add("OnBlur", "setformatdt();")
            tbAmount.Attributes.Add("OnBlur", "setformatdt();")

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

    Private Function GetStringDt(ByVal Year As String, ByVal Period As String, ByVal Revisi As String) As String
        Return "SELECT * From V_GLBudgetDt WHERE Year = " + Year + " AND Period =" + Period + " AND Revisi =" + Revisi
    End Function


    Private Sub EnableHd(ByVal State As Boolean)
        Try
            ddlYear.Enabled = State
            ddlPeriod.Enabled = State
            btnGetData.Enabled = State
        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub BindDataDt(ByVal Year As String, ByVal Period As String, ByVal Revisi As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt") = Nothing
            dt = SQLExecuteQuery(GetStringDt(Year, Period, Revisi), ViewState("DBConnection").ToString).Tables(0)
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
            If ViewState("Account") = tbAccount.Text Then
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

                CekTrans = SQLExecuteScalar("SELECT COUNT(YEAR) FROM GLBudgetHd WHERE YEAR = " + ddlYear.SelectedValue + " AND Period = " + ddlPeriod.SelectedValue, ViewState("DBConnection").ToString)
                If CekTrans <> "0" Then
                    lbStatus.Text = MessageDlg("Budget for " + ddlYear.SelectedValue + " and Period " + ddlPeriod.SelectedItem.Text + " exist, cannot save data")
                    Exit Sub
                End If

                SQLString = "INSERT INTO GLBudgetHd (Year, Period, Status, Revisi, Remark, UserPrep, DatePrep, FgActive) " + _
                "SELECT " + ddlYear.SelectedValue + "," + ddlPeriod.SelectedValue + ", 'H', 0, " + _
                QuotedStr(tbRemark.Text) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", getDate(), 'Y'"

                ViewState("Year") = ddlYear.SelectedValue
                ViewState("Period") = ddlPeriod.SelectedValue
                ViewState("Revisi") = "0"
            Else
                SQLString = "UPDATE GLBudgetHd SET Remark = " + QuotedStr(tbRemark.Text) + _
                ", DatePrep = getDate()" + _
                " WHERE Year = " + ddlYear.SelectedValue + " AND Period = " + ddlPeriod.SelectedValue + " And Revisi = " + lbRevisi.Text
            End If
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            'update Primary Key on Dt
            Dim Row As DataRow()

            Row = ViewState("Dt").Select("Year IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("Year") = ddlYear.SelectedValue
                Row(I)("Period") = ddlPeriod.SelectedValue
                Row(I)("Revisi") = lbRevisi.Text
                Row(I).EndEdit()
            Next

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT Year, Period, Revisi, Account, Currency, CostCtr, AmountPrev, ActualPrev, Amount " + _
                                         " FROM GLBudgetDt WHERE Year = " & ViewState("Year") & " AND Period = " & ViewState("Period") & " AND Revisi = " & ViewState("Revisi"), con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt As New DataTable("GLBudgetDt")

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
            ViewState("Period") = Now.Month.ToString
            ViewState("Revisi") = "0"
            ClearHd()
            Cleardt()
            pnlDt.Visible = True
            BindDataDt("1", "1", "0")

        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            BindToDropList(ddlYear, Now.Year.ToString)
            BindToDropList(ddlPeriod, Now.Month.ToString)
            lbRevisi.Text = "0"
            tbRemark.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbAccount.Text = ""
            tbDescription.Text = ""
            ddlCurrency.SelectedValue = ViewState("Currency")
            tbAmountPrev.Text = "0"
            tbActualPrev.Text = "0"
            tbAmount.Text = "0"

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

                
            Else
                If tbAccount.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Account Must Have Value")
                    tbAccount.Focus()
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
                    ViewState("Period") = GVR.Cells(3).Text
                    ViewState("Revisi") = GVR.Cells(5).Text
                    GridDt.PageIndex = 0
                    FillTextBoxHd(ViewState("Year"), ViewState("Revisi"))
                    BindDataDt(ViewState("Year"), ViewState("Period"), ViewState("Revisi"))
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, pnlDt, GridDt)
                    btnHome.Visible = True
                    btnGetData.Visible = False
                ElseIf DDL.SelectedValue = "Edit" Then
                    If GVR.Cells(4).Text = "H" Or GVR.Cells(4).Text = "G" Then
                        CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        MovePanel(PnlHd, pnlInput)
                        pnlDt.Visible = True
                        ViewState("Year") = GVR.Cells(2).Text
                        ViewState("Period") = GVR.Cells(3).Text
                        ViewState("Revisi") = GVR.Cells(5).Text
                        GridDt.PageIndex = 0
                        BindDataDt(ViewState("Year"), ViewState("Period"), ViewState("Revisi"))
                        FillTextBoxHd(ViewState("Year"), ViewState("Revisi"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        btnHome.Visible = False
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                        btnGetData.Visible = True
                    Else
                        lbStatus.Text = MessageDlg("Data must be Hold or Get Approval to edit")
                        Exit Sub
                    End If
                ElseIf DDL.SelectedValue = "Revisi" Then
                    CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                    If CekMenu <> "" Then
                        lbStatus.Text = CekMenu
                        Exit Sub
                    End If
                    If Not GVR.Cells(4).Text = "P" Then
                        lbStatus.Text = MessageDlg("Data Must be Posted Before Create Revision")
                        Exit Sub
                    End If

                    Dim Result, SqlString, CurrFilter, Value As String

                    SqlString = "Declare @A VarChar(255) EXEC S_GLBudgetCreateRevisi " + GVR.Cells(2).Text + ", " + GVR.Cells(3).Text + ", " + QuotedStr(ViewState("UserId").ToString) + ", @A SELECT @A "
                    Result = SQLExecuteScalar(SqlString, ViewState("DBConnection"))
                    Result = Result.Replace("0", "")
                    If Trim(Result) <> "" Then
                        lbStatus.Text = MessageDlg(Result)
                    End If
                    CurrFilter = tbFilter.Text

                    Value = ddlField.SelectedValue
                    tbFilter.Text = GVR.Cells(2).Text
                    ddlField.SelectedValue = "Year"
                    btnSearch_Click(Nothing, Nothing)
                    tbFilter.Text = CurrFilter
                    ddlField.SelectedValue = Value
                ElseIf DDL.SelectedValue = "Print" Then
                    Try
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        Session("DBConnection") = ViewState("DBConnection")
                        Session("SelectCommand") = "EXEC S_GLBudgetForm '" + GVR.Cells(2).Text + "','" + GVR.Cells(3).Text + "','" + GVR.Cells(5).Text + "'"
                        'lbStatus.Text = Session("SelectCommand")
                        'Exit Sub
                        Session("ReportFile") = ".../../../Rpt/FormTrGLBudget.frx"
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

            dr = ViewState("Dt").Select("Account = " + QuotedStr(GVR.Cells(1).Text))
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
            ViewState("Account") = GVR.Cells(1).Text
            FillTextBoxDt(GVR.Cells(1).Text)
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            tbAccount.Focus()
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub FillTextBoxHd(ByVal Taon As String, ByVal Revisi As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "Year = " + Taon + " AND Revisi = " + Revisi, ViewState("DBConnection").ToString)
            ddlYear.SelectedValue = Taon
            lbRevisi.Text = Revisi
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxDt(ByVal Account As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("Account = " + QuotedStr(Account))

            If Dr.Length > 0 Then
                BindToText(tbAccount, Dr(0)("Account").ToString)
                BindToText(tbDescription, Dr(0)("Description").ToString)
                BindToDropList(ddlCurrency, Dr(0)("Currency").ToString)
                BindToDropList(ddlCostCtr, Dr(0)("CostCtr").ToString)

                BindToText(tbAmountPrev, Dr(0)("AmountPrev").ToString)
                BindToText(tbActualPrev, Dr(0)("ActualPrev").ToString)
                BindToText(tbAmount, Dr(0)("Amount").ToString)
                
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbaccount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccount.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Dim SQLString, FgCostCtr As String
        Try
            SQLString = "Select Account, Description, Currency, COALESCE(FgCostCtr,'N') AS FgCostCtr from VMsAccount WHERE Account = " + QuotedStr(tbAccount.Text)
            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbAccount.Text = Dr("Account")
                tbDescription.Text = Dr("Description")
                ddlCurrency.SelectedValue = Dr("Currency")
                FgCostCtr = Dr("FgCostCtr")
            Else
                tbAccount.Text = ""
                tbDescription.Text = ""
                FgCostCtr = "N"
            End If

            If FgCostCtr = "Y" Then
                ddlCostCtr.Enabled = True
                ddlCostCtr.SelectedIndex = 1
            Else
                ddlCostCtr.Enabled = False
                ddlCostCtr.SelectedIndex = 0
            End If
            GetPrevAmount(ddlYear.SelectedValue, ddlPeriod.SelectedValue, tbAccount.Text)
        Catch ex As Exception
            Throw New Exception("tb Factory change Error : " + ex.ToString)
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
            result = SQLExecuteScalar("Select Count([year]) from GLBudgetHd where [year] = " + ddlYear.SelectedValue + " and period = " + ddlPeriod.SelectedValue, ViewState("DBConnection"))
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
                Session("SelectCommand") = "EXEC S_GLBudgetForm '" + Result + "'"
                lbStatus.Text = Session("SelectCommand")

                Session("ReportFile") = ".../../../Rpt/FormGLBudget.frx"
                Session("DBConnection") = ViewState("DBConnection")
                AttachScript("openprintdlg();", Page, Me.GetType)
            Else
                Status = CekStatus(ActionValue)

                ListSelectNmbr = ""
                '3 = status, 2 & 3 = key, 
                GetListCommand(Status, GridView1, "4,2,3,5", ListSelectNmbr, Nmbr, lbStatus.Text)

                If ListSelectNmbr = "" Then Exit Sub
                For j = 0 To (Nmbr.Length - 1)
                    If Nmbr(j) = "" Then
                        Exit For
                    Else

                        Result = ExecSPCommandGo(ActionValue, "S_GLBudget", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)

                        If Trim(Result) <> "" Then
                            lbStatus.Text = lbStatus.Text + Result + " <br/>"
                        End If
                    End If
                Next
                BindData("LTRIM(STR(Year))+'|'+LTRIM(STR(Period))+'|'+LTRIM(STR(Revisi)) in (" + ListSelectNmbr + ")")
            End If
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
                lbStatus.Text = MessageDlg("Data for year " + ddlYear.SelectedValue + " and period " + ddlPeriod.SelectedItem.Text + " exist")
                Exit Sub
            End If

            ds = SQLExecuteQuery("EXEC S_GLBudgetGetLastData " + ddlYear.SelectedValue + "," + ddlPeriod.SelectedValue, ViewState("DBConnection").ToString)

            dt = ds.Tables(0)

            If dt.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("No Data for Last Period")
                Exit Sub
            End If

            For Each CurDr In dt.Rows
                DR = ViewState("Dt").NewRow
                DR("Account") = CurDr("Account")
                DR("Description") = CurDr("Description")
                DR("Currency") = CurDr("Currency")
                DR("CostCtr") = CurDr("CostCtr")
                DR("CostCtrName") = CurDr("CostCtrName")
                DR("AmountPrev") = CurDr("AmountPrev")
                DR("ActualPrev") = CurDr("ActualPrev")
                DR("Amount") = CurDr("Amount")
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
                lbStatus.Text = MessageDlg("Data for year " + ddlYear.SelectedValue + " and period " + ddlPeriod.SelectedItem.Text + " exist")
                Exit Sub
            End If
            ViewState("StateDt") = "Insert"
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            StatusButtonSave(False)
            tbAccount.Focus()
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
            ExistRow = ViewState("Dt").Select("Account = " + QuotedStr(tbAccount.Text))

            If ViewState("StateDt") = "Edit" Then
                Dim Row As DataRow

                'If ExistRow.Count > AllowedRecord() Then
                '    lbStatus.Text = MessageDlg("Data Exist, Cannot Edit Data")
                '    Exit Sub
                'End If

                Row = ViewState("Dt").Select("Account = " + QuotedStr(ViewState("Account")))(0)

                If CekDt() = False Then
                    btnSaveDt.Focus()
                    Exit Sub
                End If

                Row.BeginEdit()
                Row("Account") = tbAccount.Text
                Row("Description") = tbDescription.Text
                Row("Currency") = ddlCurrency.SelectedValue
                Row("CostCtr") = ddlCostCtr.SelectedValue
                Row("CostCtrName") = ddlCostCtr.SelectedItem.Text
                Row("AmountPrev") = tbAmountPrev.Text
                Row("ActualPrev") = tbActualPrev.Text
                Row("Amount") = tbAmount.Text                                
                Row.EndEdit()
                ViewState("Account") = Nothing
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
                dr("Account") = tbAccount.Text
                dr("Description") = tbDescription.Text
                dr("Currency") = ddlCurrency.SelectedValue
                dr("CostCtr") = ddlCostCtr.SelectedValue
                dr("CostCtrName") = ddlCostCtr.SelectedItem.Text
                dr("AmountPrev") = tbAmountPrev.Text
                dr("ActualPrev") = tbActualPrev.Text
                dr("Amount") = tbAmount.Text

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
            ddlField.SelectedValue = "Year"
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

    Protected Sub btnAccount_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccount.Click
        Dim ResultField As String
        Try
            Session("filter") = "Select Account, Description, Currency, Class_Account, Sub_Group_Account, Group_Account, FgCostCtr from VMsAccount "
            Session("DBConnection") = ViewState("DBConnection")
            ResultField = "Account, Description, Currency, FgCostCtr"
            ViewState("Sender") = "btnAccount"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Factory Click Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub GetPrevAmount(ByVal Year As String, ByVal Period As String, ByVal Account As String)
        Dim CurDr As DataRow
        Dim ds As DataSet
        Dim dt As DataTable
        Try

            ds = SQLExecuteQuery("EXEC S_GLBudgetGetPrevAmount " + Year + "," + Period + "," + QuotedStr(Account), ViewState("DBConnection").ToString)

            dt = ds.Tables(0)

            If dt.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("No Data for Last Period")
                Exit Sub
            End If

            For Each CurDr In dt.Rows
                tbAmountPrev.Text = CurDr("AmountPrev")
                tbActualPrev.Text = CurDr("ActualPrev")
            Next

        Catch ex As Exception
            lbStatus.Text = "GetPrevAmount Error : " + ex.ToString
        End Try
    End Sub
End Class