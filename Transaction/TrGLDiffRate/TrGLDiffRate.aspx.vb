Imports System.Data
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO

Partial Class Transaction_TrGLDiffRate_TrGLDiffRate
    Inherits System.Web.UI.Page
    Protected con, con2 As New SqlConnection
    Protected da, da2 As New SqlDataAdapter
    Protected GetStringHd As String = "Select * From V_GLDiffRateHd"

    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT * From V_GLDiffRateDt WHERE TransNmbr = " + QuotedStr(Nmbr) + " "
    End Function

    Private Function GetStringDt2(ByVal Nmbr As String) As String
        Return "SELECT * From V_GLDiffRateDtAcc WHERE TransNmbr = " + QuotedStr(Nmbr) + " "
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                SetInit()
                Session("AdvanceFilter") = ""
                FillCombo(ddlCurrency, "Select Currency from VmsCurrency where Currency <> " + QuotedStr(ViewState("Currency")), False, "Currency", "Currency", ViewState("DBConnection"))
            End If
            lbStatus.Text = ""
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If
            ' hasil dari search dialog
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnGetDt" Then
                    Dim drResult As DataRow
                    For Each drResult In Session("Result").Rows
                        'insert
                        'Dim dr As DataRow
                        'Dim dt As SqlDataReader
                        'dr = ViewState("Dt").NewRow
                        'dr("Location") = ""
                        'dr("Product") = drResult("Product_Code")
                        'dr("Product_Name") = drResult("Product_Name")
                        'dr("Specification") = drResult("Specification")
                        'dr("Unit") = drResult("Unit")
                        'dr("QtyOpname") = 0
                        'dt = SQLExecuteReader("EXEC S_GLDiffRateCalculate " + QuotedStr(ViewState("Reference")), ViewState("DBConnection"))
                        'dt.Read()
                        'dr("OldLimit") = FormatNumber(dt("TotalLimit"), ViewState("DigitCurr"))
                        'dr("NewLimit") = FormatNumber(dt("TotalLimit"), ViewState("DigitCurr"))
                        'dr("UseLimit") = FormatNumber(dt("UsedLimit"), ViewState("DigitCurr"))
                        'dr("SaldoLimit") = FormatNumber(dt("TotalLimit") - dt("UsedLimit"), ViewState("DigitCurr"))
                        'dt.Close()
                        'ViewState("Dt").Rows.Add(dr)
                        'ViewState("Dt").Rows.Add(dr)
                    Next
                    BindGridDt(ViewState("Dt2"), GridDt2)
                    EnableHd(GetCountRecord(ViewState("Dt2")) <> 0)
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
        ViewState("SortExpression") = Nothing
        ViewState("DigitCurr") = ViewState("DigitHome")
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
        BtnGo.Visible = False
        lblJudul.Text = "Revaluasi"
        If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then

        End If
        'Me.tbRate.Attributes.Add("OnBlur", "kurang(" + tbRate.ClientID + "," + tbOldUsed.ClientID + "," + tbSaldo.ClientID + "); setformat();")
        'Me.tbRate.Attributes.Add("OnBlur", "setformat();")
    End Sub

    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue, ddlRange.SelectedValue)
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
                ViewState("SortExpression") = "TransDate DESC"
            End If
            DV.Sort = ViewState("SortExpression")
            GridView1.DataSource = DV
            GridView1.DataBind()
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

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
                    Result = ExecSPCommandGo(ActionValue, "S_GLDiffRate", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                    If Trim(Result) <> "" Then
                        lbStatus.Text = lbStatus.Text + Result + " <br/>"

                    End If
                End If
            Next
            BindData("Reference in (" + ListSelectNmbr + ")")
        Catch ex As Exception
            lbStatus.Text = "Go Command Error : " + ex.ToString
        End Try
    End Sub

    Private Sub EnableHd(ByVal State As Boolean)
        Try
            btnGetDt.Visible = State
            tbDiffDate.Enabled = State
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
                tbRef.Text = GetAutoNmbr("DFR", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
                SQLString = "INSERT INTO GLDiffRateHd (TransNmbr, Status, TransDate, DiffDate, Remark, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbRef.Text) + ", 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', '" + Format(tbDiffDate.SelectedValue, "yyyy-MM-dd") + "', " + _
                QuotedStr(tbRemark.Text) + "," + QuotedStr(ViewState("UserId").ToString) + ", getDate()"
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM GLDiffRateHd WHERE TransNmbr = " + QuotedStr(tbRef.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE GLDiffRateHd SET DiffDate = '" + Format(tbDiffDate.SelectedValue, "yyyy-MM-dd") + _
                "', Remark = '" + tbRemark.Text + "'," + _
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
                Row(I)("Transnmbr") = tbRef.Text
                Row(I).EndEdit()
            Next

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString 'System.Configuration.ConfigurationManager.AppSettings.Get("ConString")
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT * FROM GLDiffRateDt WHERE TransNmbr = '" & ViewState("Reference") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand

            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
                    "UPDATE GLDiffRateDt SET Currency = @Currency, NewRate = @NewRate, Remark = @Remark WHERE TransNmbr = '" & ViewState("Reference") & "' AND Currency = @OldCurrency AND NewRate = @OldNewRate", con)
            ' Define output parameters.
            Update_Command.Parameters.Add("@Currency", SqlDbType.VarChar, 5, "Currency")
            Update_Command.Parameters.Add("@NewRate", SqlDbType.Float, 18, "NewRate")
            Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 60, "Remark")
            ' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@OldCurrency", SqlDbType.VarChar, 5, "Currency")
            param.SourceVersion = DataRowVersion.Original
            param = Update_Command.Parameters.Add("@OldNewRate", SqlDbType.Float, 18, "NewRate")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.

            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM GLDiffRateDt WHERE TransNmbr = '" & ViewState("Reference") & "' AND Currency = @Currency AND NewRate = @NewRate", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@Currency", SqlDbType.VarChar, 5, "Currency")
            param = Delete_Command.Parameters.Add("@NewRate", SqlDbType.Float, 18, "NewRate")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("GLDiffRateDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt
        Catch ex As Exception
            Throw New Exception("Save All Data Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, pnlDt, GridDt)
            newTrans()
            btnHome.Visible = False
            btnGetDt.Visible = False
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
            tbDiffDate.SelectedDate = ViewState("ServerDate") 'Today
            pnlDt.Visible = True
            PnlDt2.Visible = False
            BindDataDt("")
            'BindDataDt2("")
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbRef.Text = ""
            tbRemark.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub Cleardt()
        Try
            tbRemarkDt.Text = ""
            tbRate.Text = "0"
            'ddlCurrency.SelectedValue = ViewState("Currency").ToString
            'ChangeCurrency(ddlCurrency, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
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
                If CekDt(dr, "Currency") = False Then
                    Exit Sub
                End If
            Next
            SaveAll()
            BindDataDt2("")
            'btnGetDt.Visible = True
            newTrans()
        Catch ex As Exception
            lbStatus.Text = "btn save all error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveTrans_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveTrans.Click
        Dim CurrFilter, Value As String
        Try
            'If CekHd() = False Then
            '    Exit Sub
            'End If
            'If IsNothing(ViewState("Dt")) Then
            '    lbStatus.Text = MessageDlg("Detail must have at least 1 record")
            '    Exit Sub
            'End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
                Exit Sub
            End If
            For Each dr In ViewState("Dt").Rows
                If CekDt(dr, "Currency") = False Then
                    Exit Sub
                End If
            Next
            SaveAll()
            MovePanel(pnlInput, PnlHd)
            CurrFilter = tbFilter.Text
            Value = ddlField.SelectedValue
            tbFilter.Text = tbRef.Text
            ddlField.SelectedValue = "Reference"
            btnSearch_Click(Nothing, Nothing)
            tbFilter.Text = CurrFilter
            ddlField.SelectedValue = Value
        Catch ex As Exception
            lbStatus.Text = "Save All Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdddt.Click, btnAddDt2.Click
        Cleardt()
        'If CekHd() = False Then
        '    Exit Sub
        'End If
        ViewState("StateDt") = "Insert"
        MovePanel(pnlDt, pnlEditDt)
        EnableHd(False)
        ChangeCurrency(ddlCurrency, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
        StatusButtonSave(False)
    End Sub

    Function CekHd() As Boolean
        Try
            If tbDiffDate.SelectedDate < tbDate.SelectedDate Then
                lbStatus.Text = MessageDlg("Differance Date must be greater than " + tbDate.SelectedDate.ToString)
                tbDiffDate.Focus()
                Return False
            End If
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
                If Dr("Currency").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Currency Must Have Value")
                    Return False
                End If
                If CFloat(Dr("NewRate").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("New Rate Must Have Value")
                    Return False
                End If
            Else
                If ddlCurrency.SelectedValue = "" Then
                    lbStatus.Text = MessageDlg("Currency must have value")
                    ddlCurrency.Focus()
                    Return False
                End If
                If CFloat(tbRate.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Rate must have value")
                    tbRate.Focus()
                    Return False
                End If
                If ddlCurrency.SelectedValue <> ViewState("Currency") And CFloat(tbRate.Text) = 1 Then
                    lbStatus.Text = MessageDlg("Rate must have value")
                    tbRate.Focus()
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
            FilterName = "Reference, Date, Status, Difference Date, Remark"
            FilterValue = "Reference, Trans_Date, Status, Diff_Date, Remark"
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
                    BindDataDt2(ViewState("Reference"))

                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, pnlDt, GridDt)
                    'btnGetDt.Visible = GVR.Cells(3).Text <> "P"
                    PnlDt2.Visible = True 'GVR.Cells(3).Text <> "P"

                    btnHome.Visible = True
                    btnGetData.Visible = False
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
                        BindDataDt2(ViewState("Reference"))

                        FillTextBoxHd(ViewState("Reference"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)

                        btnHome.Visible = False
                        EnableHd(GetCountRecord(ViewState("Dt")) <> 0)

                        PnlDt2.Visible = False
                        btnGetDt.Visible = False
                        btnGetData.Visible = True
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
                    'Dim ReportGw As New ReportDocument
                    'Dim Reportds As DataSet

                    'Reportds = SQLExecuteQuery("EXEC S_GLFormDiffRate " + QuotedStr(GVR.Cells(2).Text), ViewState("DBConnection").ToString)

                    ''ReportGw.SetParameterValue("@Nmbr", e.Item.Cells(2).Text)
                    'ReportGw.Load(Server.MapPath("~\Rpt\FormJEntry.Rpt"))
                    'ReportGw.SetDataSource(Reportds.Tables(0))

                    'Session("Report") = ReportGw
                    'Response.Write("<script language='javascript'> { window.open(""../../Rpt/PrintMaster.Aspx"", ""List"",""scrollbars=yes, resizable=yes,width=600,height=500"");}</script>")

                    ''CrystalReportViewer1.ReportSource = ReportGw
                    ''PnlHd.Visible = False
                    ''pnlPrint.Visible = True
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
                'btnGetDt.Enabled = False
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Dim dr() As DataRow
        Dim lb As Label
        Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
        lb = GVR.FindControl("lbLocation")
        'dr = ViewState("Dt").Select("Currency = " + QuotedStr(GVR.Cells(1).Text + "|" + TrimStr(lb.Text)))
        dr = ViewState("Dt").Select("Currency = " + QuotedStr(GVR.Cells(1).Text))
        dr(0).Delete()
        ' ViewState("Dt").AcceptChanges()
        BindGridDt(ViewState("Dt"), GridDt)
        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Dim lb As Label
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            FillTextBoxDt(GVR.Cells(1).Text)
            ViewState("DigitCurr") = CInt(SQLExecuteScalar("Select DigitDecimal FROM MsCurrency WHERE CurrCode = " + QuotedStr(ddlCurrency.SelectedValue), ViewState("DBConnection").ToString))
            lb = GVR.FindControl("lbLocation")
            ViewState("DtValue") = GVR.Cells(1).Text '+ "|" + TrimStr(lb.Text)
            FillTextBoxDt(ViewState("DtValue"))
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"

            ddlCurrency.Focus()
            'btnGetDt.Enabled = False
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "Reference = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            'newTrans()
            tbRef.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToDate(tbDiffDate, Dt.Rows(0)("DiffDate").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
            btnGetDt.Visible = (Dt.Rows(0)("Status").ToString = "H") Or (Dt.Rows(0)("Status").ToString = "G")
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("Currency = " + QuotedStr(ItemNo))
            If Dr.Length > 0 Then
                BindToDropList(ddlCurrency, Dr(0)("Currency").ToString)
                BindToText(tbRate, Dr(0)("NewRate").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
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

    Private Function GetQtySystem(ByVal tgl As DateTime, ByVal wrhs As String, ByVal subled As String, ByVal product As String, ByVal location As String) As Double
        'Dim dr As SqlDataReader
        'dr = SQLExecuteReader("EXEC S_STOpnameGetQtySystem " + QuotedStr(ddlWrhs.SelectedValue) + ", " + QuotedStr(tbSubled.Text) + " , " + QuotedStr(tbCode.Text) + ", " + QuotedStr(ddlLocation.SelectedValue) + ", '" + Format(tbPositionDate.SelectedValue, "yyyy-MM-dd") + "'", ViewState("DBConnection").ToString)
        'dr.Read()
        'Return dr("QtySystem")
    End Function

    Protected Sub lbCurrency_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCurrency.Click
        Try
            ViewState("InputLocation") = "Y"
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsCurrency')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb Warehouse location Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlCurrency_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCurrency.SelectedIndexChanged
        If ViewState("InputCurrency") = "Y" Then
            RefreshMaster("S_GetCurrency", "Currency", "Currency", ddlCurrency, ViewState("DBConnection"))
            ViewState("InputCurrency") = Nothing
        End If
        ChangeCurrency(ddlCurrency, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
        tbRate.Focus()
    End Sub

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt.Click
        Dim Row As DataRow
        Try
            If ViewState("StateDt") = "Edit" Then
                If ViewState("DtValue") <> ddlCurrency.SelectedValue Then
                    If CekExistData(ViewState("Dt"), "Currency", ddlCurrency.SelectedValue) Then
                        lbStatus.Text = "Currency '" + ddlCurrency.SelectedItem.Text + "' has already exists"
                        Exit Sub
                    End If
                End If
                Row = ViewState("Dt").Select("Currency = " + QuotedStr(ViewState("DtValue")))(0)
                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                Row("Currency") = ddlCurrency.SelectedValue
                Row("Remark") = tbRemarkDt.Text
                Row("NewRate") = FormatNumber(tbRate.Text, ViewState("DigitRate"))
                Row.EndEdit()
                'ViewState("Dt").AcceptChanges()
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If
                If CekExistData(ViewState("Dt"), "Currency", ddlCurrency.SelectedValue) Then
                    lbStatus.Text = "Currency '" + ddlCurrency.SelectedItem.Text + "' has already exists"
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("Currency") = ddlCurrency.SelectedValue
                dr("Remark") = tbRemarkDt.Text
                dr("NewRate") = FormatNumber(tbRate.Text, ViewState("DigitRate"))
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
            'EnableHd(GridDt.Rows.Count = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnGetDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetDt.Click
        'Dim ResultField As String 'ResultSame 
        Try
            'If CekHd() = False Then
            '    Exit Sub
            'End If
            
            Dim dataQC As DataTable
            Dim drResult As DataRow
            Dim SqlString As String
            SqlString = "EXEC S_GLDiffRateCalculate " + QuotedStr(tbRef.Text)
            
            dataQC = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)

            If dataQC.Rows.Count > 0 Then
                drResult = dataQC.Rows(0)
            Else
                drResult = Nothing
            End If

            Dim drH As DataRow()
            Dim r As DataRow
            drH = ViewState("Dt2").Select("")
            For Each r In drH
                r.Delete()
            Next

            If Not drResult Is Nothing Then
                For Each drResult In dataQC.Rows
                    Dim dr As DataRow

                    dr = ViewState("Dt2").NewRow
                    dr("Currency") = drResult("CurrCode")
                    ' dr("NewRate") = "1" 'drResult("NewRate")
                    dr("FgSubled") = drResult("FgSubled")
                    dr("Subled") = drResult("Subled")
                    dr("SubledName") = drResult("SubledName")
                    dr("Account") = drResult("Account")
                    dr("AccountName") = drResult("AccountName")
                    dr("AmountForex") = FormatNumber(drResult("AmountForex"), Session("DigitCurr"))
                    dr("AmountHome") = FormatNumber(drResult("AmountHome"), Session("DigitCurr"))
                    dr("NewAmountHome") = FormatNumber(drResult("NewAmountHome"), Session("DigitCurr"))
                    dr("AmountAdjust") = FormatNumber(drResult("AmountAdjust"), Session("DigitCurr"))
                    ViewState("Dt2").Rows.Add(dr)
                Next
                BindGridDt(ViewState("Dt2"), GridDt2)
                EnableHd(GetCountRecord(ViewState("Dt2")) = 0)
                StatusButtonSave(True)
                'Dim Row As DataRow()
                'Row = ViewState("Dt2").Select("TransNmbr IS NULL")
                'For I = 0 To Row.Length - 1
                '    Row(I).BeginEdit()
                '    Row(I)("TransNmbr") = tbRef.Text
                '    Row(I).EndEdit()
                'Next

                ''save dt2
                'Dim ConnString2 As String = ViewState("DBConnection").ToString 'System.Configuration.ConfigurationManager.AppSettings.Get("ConString")
                'con2 = New SqlConnection(ConnString2)
                'con2.Open()
                'Dim cmdSql2 As New SqlCommand("SELECT TransNmbr, Currency, Account, FgSubLed, SubLed, AmountForex, AmountHome, NewAmountHome, AmountAdjust From GLDiffrateDtAcc WHERE TransNmbr = '" & ViewState("TransNmbr") & "'", con2)
                'da2 = New SqlDataAdapter(cmdSql2)
                'Dim dbcommandBuilder2 As SqlCommandBuilder = New SqlCommandBuilder(da2)

                'da2.InsertCommand = dbcommandBuilder2.GetInsertCommand
                'da2.DeleteCommand = dbcommandBuilder2.GetDeleteCommand
                'da2.UpdateCommand = dbcommandBuilder2.GetUpdateCommand

                'Dim Dt2 As New DataTable("GLDiffrateDtAcc")

                'Dt2 = ViewState("Dt2")
                'da2.Update(Dt2)
                'Dt2.AcceptChanges()
                'ViewState("Dt2") = Dt2
            End If
        Catch ex As Exception
            lbStatus.Text = "btn get Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnGetData_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetData.Click
        Dim Dr, CurDr As DataRow
        Dim ds As DataSet
        Dim dt As DataTable

        Try
            If GetCountRecord(ViewState("Dt")) > 0 Then
                lbStatus.Text = MessageDlg("Data Detail Account not empty")
                Exit Sub
            End If
            BindDataDt("")

            ds = SQLExecuteQuery("EXEC S_GLDiffRateGetCurrency " + QuotedStr(Format(tbDiffDate.SelectedValue, "yyyy-MM-dd")), ViewState("DBConnection").ToString)
            dt = ds.Tables(0)

            If dt.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("No Data")
                Exit Sub
            End If

            Dim Row As DataRow()
            For Each CurDr In dt.Rows
                Dr = ViewState("Dt").NewRow
                Dr("Currency") = CurDr("Currency")
                Dr("NewRate") = FormatFloat(CurDr("Rate"), ViewState("DigitHome"))
                ViewState("Dt").Rows.Add(Dr)
            Next

            BindGridDt(ViewState("Dt"), GridDt)
        Catch ex As Exception
            lbStatus.Text = "btnGetData_Click Error : " + ex.ToString
        End Try
    End Sub
End Class
