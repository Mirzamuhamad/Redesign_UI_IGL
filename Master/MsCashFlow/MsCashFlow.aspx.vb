Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data

Partial Class MsCashFlow_MsCashFlow
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        If Not IsPostBack Then
            InitProperty()
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            'DataGrid.PageSize = CInt(Session("PageSizeGrid"))
            'UserLevel
            'MenuParam            
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
        End If
        If Not Session("Result") Is Nothing Then
            If ViewState("Sender") = "SearchAdd" Or ViewState("Sender") = "SearchEdit" Then
                Dim Acc As TextBox
                Dim AccName As Label
                If ViewState("Sender") = "SearchAdd" Then
                    Acc = DataGrid.FooterRow.FindControl("AccountAdd")
                    AccName = DataGrid.FooterRow.FindControl("AccountNameAdd")
                Else
                    Acc = DataGrid.Rows(DataGrid.EditIndex).FindControl("AccountEdit")
                    AccName = DataGrid.Rows(DataGrid.EditIndex).FindControl("AccountNameEdit")
                End If
                Acc.Text = Session("Result")(0).ToString
                AccName.Text = Session("Result")(1).ToString
                Acc.Focus()
            End If

            Session("Result") = Nothing
            ViewState("Sender") = Nothing
            Session("filter") = Nothing
            Session("Column") = Nothing
        End If
        dsCashFlowGroup.ConnectionString = ViewState("DBConnection")
        dsCashFlowType.ConnectionString = ViewState("DBConnection")
        dsCurrency.ConnectionString = ViewState("DBConnection")
        dsstartMonth.ConnectionString = ViewState("DBConnection")
        dsYear.ConnectionString = ViewState("DBConnection")
        lstatus.Text = ""
    End Sub
    Protected Sub AccountEdit_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dr As DataRow
        Dim ds As DataSet
        Dim Acc As TextBox
        Dim AccName As Label
        Dim Count As Integer
        Dim dgi As GridViewRow
        Try
            Count = DataGrid.EditIndex
            dgi = DataGrid.Rows(Count)
            Acc = dgi.FindControl("AccountEdit")
            AccName = dgi.FindControl("AccountNameEdit")


            ds = SQLExecuteQuery("Select Account, Description From VMsAccount WHERE Account = " + QuotedStr(Acc.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                Acc.Text = ""
                AccName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                Acc.Text = dr("Account").ToString
                AccName.Text = dr("Description").ToString
            End If
        Catch ex As Exception
            lstatus.Text = "tb Acc Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub AccountAdd_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dr As DataRow
        Dim ds As DataSet
        Dim Acc, tb As TextBox
        Dim AccName As Label
        Dim Count As Integer
        Dim dgi As GridViewRow
        Try
            tb = sender
            If tb.ID = "AccountAdd" Then
                Count = DataGrid.Controls(0).Controls.Count
                dgi = DataGrid.Controls(0).Controls(Count - 2) '-1 for allowpaging = False
                Acc = dgi.FindControl("AccountAdd")
                AccName = dgi.FindControl("AccountNameAdd")
            Else
                Count = DataGrid.EditIndex
                dgi = DataGrid.Rows(Count)
                Acc = dgi.FindControl("AccountEdit")
                AccName = dgi.FindControl("AccountNameEdit")
            End If
            ds = SQLExecuteQuery("Select Account, Description AS AccountName From VMsAccount WHERE Account = " + QuotedStr(Acc.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                Acc.Text = ""
                AccName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                Acc.Text = dr("Account").ToString
                AccName.Text = dr("AccountName").ToString
            End If
        Catch ex As Exception
            lstatus.Text = "tb Acc Changed Error : " + ex.ToString
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

    Function CheckMenuLevel(ByVal CommandName As String) As Boolean
        Try
            If CommandName = "Edit" Then
                If ViewState("MenuLevel").Rows(0)("FgEdit") = "N" Then
                    lstatus.Text = "<script language='javascript'> {alert('You are not authorized to edit record. Please contact administrator')}</script>"
                    Return False
                    Exit Function
                End If
            End If
            'If CommandName = "Insert" Then
            '    If ViewState("FgInsert") = "N" Then
            '        lstatus.Text = "<script language='javascript'> {alert('You are not authorized to insert record. Please contact administrator')}</script>"
            '        Return False
            '        Exit Function
            '    End If
            'End If

            If CommandName = "Delete" Then
                If ViewState("MenuLevel").Rows(0)("FgDelete") = "N" Then
                    lstatus.Text = "<script language='javascript'> {alert('You are not authorized to delete record. Please contact administrator')}</script>"
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Check Menu Level Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        Try
            DataGrid.PageIndex = 0
            DataGrid.EditIndex = -1
            bindDataGrid()
        Catch ex As Exception
            lstatus.Text = "Btn Search Error : " + vbCrLf + ex.ToString
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
            lstatus.Text = "btn Expand Error : " + ex.ToString
        End Try
    End Sub
    Private Sub bindDataGrid()
        Dim StrFilter, SqlString As String
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            SqlString = "SELECT * FROM VMsCashFlow " + StrFilter
            
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "CashFlowCode ASC"
            End If
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection"))

        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub
    Private Sub bindDataGridDt()
        Dim tempDS As New DataSet()
        Dim DV As DataView
        Dim SqlString As String
        Try
            SqlString = "SELECT * FROM VMsCashFlowDt WHERE CashFlow = " + QuotedStr(ViewState("Nmbr"))

            tempDS = SQLExecuteQuery(SqlString, ViewState("DBConnection").ToString)
            DV = tempDS.Tables(0).DefaultView
            
            If DV.Count = 0 Then
                Dim DT As DataTable = tempDS.Tables(0)
                ShowGridViewIfEmpty(DT, DataGridDt)
                DV = DT.DefaultView
            Else
                DV.Sort = ViewState("SortExpressionDt")
                DataGridDt.DataSource = DV

                DataGridDt.DataBind()
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "bindDataGridDt Error: " & ex.ToString
        Finally
        End Try
    End Sub
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("DBConnection") = ViewState("DBConnection")
            Session("PrintType") = "Print"
            Session("SelectCommand") = "S_FormprintMaster5 'VMsCashFlow', 'CashFlowCode +''-''+ CashFlowName ', 'CashFlowGroup+ ''- '' + CashFlowType', 'Days', 'Nominal', 'AccountName', 'Cash Flow', 'Cash Flow Code - Name', 'Cash Flow Type', 'days', 'Nominal', 'Account', '" + StrFilter + "', " + QuotedStr(ViewState("UserId"))

            Session("ReportFile") = ".../../../Rpt/RptPrintMaster5.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            lstatus.Text = "Btn Print Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGrid.PageIndexChanging
        DataGrid.PageIndex = e.NewPageIndex
        If DataGrid.EditIndex <> -1 Then
            DataGrid_RowCancelingEdit(Nothing, Nothing)
        End If
        bindDataGrid()
    End Sub
    Protected Sub DataGrid_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles DataGrid.Sorting
        Try
            If ViewState("SortOrder") = Nothing Or ViewState("SortOrder") = "DESC" Then
                ViewState("SortOrder") = "ASC"
            Else
                ViewState("SortOrder") = "DESC"
            End If
            ViewState("SortExpression") = e.SortExpression + " " + ViewState("SortOrder")
            bindDataGrid()
        Catch ex As Exception
            lstatus.Text = "UserDG_SortCommandError =" + vbCrLf + ex.ToString
        End Try

    End Sub
    Protected Sub DataGrid_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGrid.RowCommand
        Dim SQLString As String
        'Disini
        Dim dbCode, dbName, tbAcc, tbDays, tbNominal, tbStartYear, tbEndYear As TextBox
        Dim ddlCashFlowGroup, ddlCashFlowType, ddlYearency, ddlStartMonth, ddlEndMonth, ddlYear As DropDownList
        Dim GVR As GridViewRow = Nothing
        Try
            If e.CommandName = "Insert" Then
                dbCode = DataGrid.FooterRow.FindControl("CashFlowCodeAdd")
                dbName = DataGrid.FooterRow.FindControl("CashFlowNameAdd")
                tbDays = DataGrid.FooterRow.FindControl("DaysAdd")
                tbNominal = DataGrid.FooterRow.FindControl("NominalAdd")
                tbStartYear = DataGrid.FooterRow.FindControl("StartYearAdd")
                tbEndYear = DataGrid.FooterRow.FindControl("EndYearAdd")
                tbAcc = DataGrid.FooterRow.FindControl("AccountAdd")
                ddlCashFlowType = DataGrid.FooterRow.FindControl("CashFlowTypeAdd")
                ddlCashFlowGroup = DataGrid.FooterRow.FindControl("CashFlowGroupAdd")
                ddlYearency = DataGrid.FooterRow.FindControl("CurrencyAdd")
                ddlStartMonth = DataGrid.FooterRow.FindControl("StartMonthAdd")
                ddlEndMonth = DataGrid.FooterRow.FindControl("EndMonthAdd")

                If dbCode.Text.Trim.Length = 0 Then
                    lstatus.Text = "Code must be filled."
                    dbCode.Focus()
                    Exit Sub
                End If
                If dbName.Text.Trim.Length = 0 Then
                    lstatus.Text = "Name must be filled."
                    dbName.Focus()
                    Exit Sub
                End If
                If ddlCashFlowGroup.Text.Trim.Length = 0 Then
                    lstatus.Text = "Cash Flow Group Must Be filled."
                    ddlCashFlowGroup.Focus()
                    Exit Sub
                End If
                If ddlCashFlowType.Text.Trim.Length = 0 Then
                    lstatus.Text = "Cash Flow Type  Must Be filled."
                    ddlCashFlowType.Focus()
                    Exit Sub
                End If

                If CInt(tbDays.Text) > 31 Then
                    lstatus.Text = "Days must be < 32."
                    tbDays.Focus()
                    Exit Sub
                End If

                If tbNominal.Text.Trim.Length = 0 Then
                    lstatus.Text = "Nominal Must Be filled."
                    tbNominal.Focus()
                    Exit Sub
                End If
                If tbDays.Text.Trim.Length = 0 Then
                    lstatus.Text = "Days Must Be filled."
                    tbDays.Focus()
                    Exit Sub
                End If


                If CInt(tbStartYear.Text) > 3000 Or CInt(tbStartYear.Text) < 2000 Then
                    lstatus.Text = "Start Year must be 2000 - 3000."
                    tbStartYear.Focus()
                    Exit Sub
                End If
                If CInt(tbEndYear.Text) > 3000 Or CInt(tbEndYear.Text) < 2000 Then
                    lstatus.Text = "End Year must be 2000 - 3000."
                    tbEndYear.Focus()
                    Exit Sub
                End If

                If SQLExecuteScalar("SELECT CashFlowCode From VMsCashFlow WHERE CashFlowCode  = " + QuotedStr(dbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Cash Flow " + QuotedStr(dbCode.Text) + " has already been exist"
                    Exit Sub
                End If

                'insert the new entry
                SQLString = "Insert into MsCashFlow (CashFlowCode, CashFlowName, CashFlowGroup, CashFlowType, Days, Currency, Nominal,Account,StartYear, EndYear, StartMonth, EndMonth, UserId, UserDate ) " + _
                "SELECT " + QuotedStr(dbCode.Text) + ", " + QuotedStr(dbName.Text) + _
                "," + QuotedStr(ddlCashFlowGroup.SelectedValue) + "," + QuotedStr(ddlCashFlowType.SelectedValue) + ", " + tbDays.Text + ", " + QuotedStr(ddlYearency.SelectedValue) + ", " + tbNominal.Text.Replace(",", "") + _
                ", " + QuotedStr(tbAcc.Text) + ", " + tbStartYear.Text + ", " + tbEndYear.Text + _
                ", " + QuotedStr(ddlStartMonth.SelectedValue) + " , " + QuotedStr(ddlEndMonth.SelectedValue) + _
                ", " + QuotedStr(ViewState("UserId").ToString) + ", getDate()"

                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

                bindDataGrid()
            ElseIf e.CommandName = "SearchAdd" Or e.CommandName = "SearchEdit" Then
                Dim FieldResult As String
                Session("DBConnection") = ViewState("DBConnection")
                Session("filter") = "Select * FROM VMsAccount"
                FieldResult = "Account, Description"
                Session("Column") = FieldResult.Split(",")
                If e.CommandName = "SearchAdd" Then
                    ViewState("Sender") = "SearchAdd"
                Else
                    ViewState("Sender") = "SearchEdit"
                End If
                If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenPopup();", True)
                End If
            ElseIf e.CommandName = "Detail" Then
                Dim lbCode, lbName, lbSYear, lbEYear, lbSMonth, lbEMonth, lbNominal As Label
                Dim gvr2 As GridViewRow
                gvr2 = DataGrid.Rows(CInt(e.CommandArgument))
                lbCode = gvr2.FindControl("CashFlowCode")
                lbName = gvr2.FindControl("CashFlowName")
                lbSYear = gvr2.FindControl("StartYear")
                lbEYear = gvr2.FindControl("EndYear")
                lbSMonth = gvr2.FindControl("StartMonth")
                lbEMonth = gvr2.FindControl("EndMonth")
                lbNominal = gvr2.FindControl("Nominal")
                ViewState("Nmbr") = lbCode.Text
                lbCashFlow.Text = lbCode.Text
                lbCashFlowName.Text = lbName.Text
                lbStartYear.Text = lbSYear.Text
                lbEndYear.Text = lbEYear.Text
                lbStartMonth.Text = lbSMonth.Text
                lbEndMonth.Text = lbEMonth.Text
                lbAmount.Text = lbNominal.Text
                pnlHd.Visible = False
                pnlDt.Visible = True
                bindDataGridDt()
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGrid.RowDeleting
        Dim txtID As Label
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            txtID = DataGrid.Rows(e.RowIndex).FindControl("CashFlowCode")

            SQLExecuteNonQuery("Delete from MsCashFlow where CashFlowCode = '" & txtID.Text & "'", ViewState("DBConnection"))
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        Dim txt As TextBox
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()
            obj = DataGrid.Rows(e.NewEditIndex)
            txt = obj.FindControl("CashFlowNameEdit")
            txt.Focus()
        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim dbName, tbAcc, tbDays, tbStartYear, tbEndYear, tbNominal As TextBox
        Dim ddlCashFlowGroup, ddlCashFlowType, ddlYearency, ddlStartMonth, ddlEndMonth As DropDownList
        Dim lbCode As Label
        'taro
        Try
            lbCode = DataGrid.Rows(e.RowIndex).FindControl("CashFlowCodeEdit")
            dbName = DataGrid.Rows(e.RowIndex).FindControl("CashFlowNameEdit")
            tbDays = DataGrid.Rows(e.RowIndex).FindControl("DaysEdit")
            tbNominal = DataGrid.Rows(e.RowIndex).FindControl("NominalEdit")
            tbStartYear = DataGrid.Rows(e.RowIndex).FindControl("StartYearEdit")
            tbEndYear = DataGrid.Rows(e.RowIndex).FindControl("EndYearEdit")
            tbAcc = DataGrid.Rows(e.RowIndex).FindControl("AccountEdit")
            ddlCashFlowGroup = DataGrid.Rows(e.RowIndex).FindControl("CashFlowGroupEdit")
            ddlCashFlowType = DataGrid.Rows(e.RowIndex).FindControl("CashFlowTypeEdit")
            ddlYearency = DataGrid.Rows(e.RowIndex).FindControl("CurrencyEdit")
            ddlStartMonth = DataGrid.Rows(e.RowIndex).FindControl("StartMonthEdit")
            ddlEndMonth = DataGrid.Rows(e.RowIndex).FindControl("EndMonthEdit")

            'If dbCode.Text.Trim.Length = 0 Then
            'lstatus.Text = "Code must be filled."
            'dbCode.Focus()
            'Exit Sub
            'End If

            If CInt(tbDays.Text) > 31 Then
                lstatus.Text = "Days must be < 32."
                tbDays.Focus()
                Exit Sub
            End If
            If CInt(tbStartYear.Text) > 3000 Or CInt(tbStartYear.Text) < 2000 Then
                lstatus.Text = "Start Year must be 2000 - 3000."
                tbStartYear.Focus()
                Exit Sub
            End If
            If CInt(tbEndYear.Text) > 3000 Or CInt(tbEndYear.Text) < 2000 Then
                lstatus.Text = "End Year must be 2000 - 3000."
                tbEndYear.Focus()
                Exit Sub
            End If
            If dbName.Text.Trim.Length = 0 Then
                lstatus.Text = "Name must be filled."
                dbName.Focus()
                Exit Sub
            End If
            If ddlCashFlowGroup.Text.Trim.Length = 0 Then
                lstatus.Text = "Cash Flow Group Must Be filled."
                ddlCashFlowGroup.Focus()
                Exit Sub
            End If
            If ddlCashFlowType.Text.Trim.Length = 0 Then
                lstatus.Text = "Cash Flow Type  Must Be filled."
                ddlCashFlowType.Focus()
                Exit Sub
            End If

            If tbNominal.Text.Trim.Length = 0 Then
                lstatus.Text = "Nominal Must Be filled."
                tbNominal.Focus()
                Exit Sub
            End If

            If tbDays.Text.Trim.Length = 0 Then
                lstatus.Text = "Days Must Be filled."
                tbDays.Focus()
                Exit Sub
            End If

            SQLString = "Update MsCashFlow set CashFlowName= " + QuotedStr(dbName.Text) + _
            ",CashFlowGroup = " + QuotedStr(ddlCashFlowGroup.SelectedValue) + _
            ",CashFlowType = " + QuotedStr(ddlCashFlowType.SelectedValue) + _
            ",Days = " + tbDays.Text + _
            ",Nominal = " + tbNominal.Text.Replace(",", "") + _
            ",Currency = " + QuotedStr(ddlYearency.SelectedValue) + _
            ",Account= " + QuotedStr(tbAcc.Text) + _
            ",startYear = " + tbStartYear.Text + _
            ",EndYear = " + tbEndYear.Text + _
            ",startMonth = " + QuotedStr(ddlStartMonth.SelectedValue) + _
            ",EndMonth = " + (ddlEndMonth.SelectedValue) + _
                     " where CashFlowCode = '" & lbCode.Text + "'"

            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = lstatus.Text + "DataGrid_Update Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles DataGrid.RowCancelingEdit
        Try
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid()
        Catch ex As Exception
            lstatus.Text = "DataGrid_Cancel Error : " + vbCrLf + ex.ToString
        End Try
    End Sub
    Protected Sub DaysAdd_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim TbDays, tb As TextBox
        Dim Count As Integer
        Dim dgi As GridViewRow
        Try
            tb = sender
            If tb.ID = "DaysAdd" Then
                Count = DataGrid.Controls(0).Controls.Count
                dgi = DataGrid.Controls(0).Controls(Count - 2) '-1 for allowpaging = False
                TbDays = dgi.FindControl("DaysAdd")
            Else
                Count = DataGrid.EditIndex
                dgi = DataGrid.Rows(Count)
                TbDays = dgi.FindControl("DaysEdit")
            End If
            If CInt(TbDays.Text) > 31 Then
                lstatus.Text = "Days must be < 32"
                TbDays.Focus()
            End If
        Catch ex As Exception
            lstatus.Text = "DaysAdd Changed Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub DaysEdit_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim TbDays As TextBox
        Dim Count As Integer
        Dim dgi As GridViewRow

        Count = DataGrid.EditIndex
        dgi = DataGrid.Rows(Count)
        TbDays = dgi.FindControl("DaysEdit")
        If CInt(TbDays.Text) > 31 Then
            lstatus.Text = "Days must be < 32"
            TbDays.Focus()
        End If

    End Sub

    Protected Sub StartYearAdd_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim TbStartYear, tb As TextBox
        Dim Count As Integer
        Dim dgi As GridViewRow
        Try
            tb = sender
            If tb.ID = "StartYearAdd" Then
                Count = DataGrid.Controls(0).Controls.Count
                dgi = DataGrid.Controls(0).Controls(Count - 2) '-1 for allowpaging = False
                TbStartYear = dgi.FindControl("StartYearAdd")
            Else
                Count = DataGrid.EditIndex
                dgi = DataGrid.Rows(Count)
                TbStartYear = dgi.FindControl("StartYearEdit")
            End If
            If CInt(TbStartYear.Text) < 2000 Or CInt(TbStartYear.Text) > 3000 Then
                lstatus.Text = "Range Year 2000-3000"
                TbStartYear.Focus()
            End If
        Catch ex As Exception
            lstatus.Text = "StartYear Changed Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub StartYearEdit_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim TbStartYear As TextBox
        Dim Count As Integer
        Dim dgi As GridViewRow

        Count = DataGrid.EditIndex
        dgi = DataGrid.Rows(Count)
        TbStartYear = dgi.FindControl("StartYearEdit")
        If CInt(TbStartYear.Text) < 2000 Or CInt(TbStartYear.Text) > 3000 Then
            lstatus.Text = "Range Year 2000-3000"
            TbStartYear.Focus()
        End If

    End Sub

    Protected Sub DataGridDt_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGridDt.PageIndexChanging
        DataGridDt.PageIndex = e.NewPageIndex
        If DataGridDt.EditIndex <> -1 Then
            DataGridDt_RowCancelingEdit(Nothing, Nothing)
        End If
        bindDataGridDt()
    End Sub

    Protected Sub DataGridDt_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles DataGridDt.RowCancelingEdit
        Try
            DataGridDt.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGridDt.EditIndex = -1
            bindDataGridDt()
        Catch ex As Exception
            lstatus.Text = "DataGridDt_Cancel Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub DataGridDt_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGridDt.RowCommand
        Dim tbAmount As TextBox
        Dim ddlYear, ddlMonth As DropDownList
        Dim SQLString As String
        Dim GVR As GridViewRow
        Try
            If e.CommandName = "Insert" Then
                GVR = DataGridDt.FooterRow
                ddlYear = GVR.FindControl("YearDtAdd")
                ddlMonth = GVR.FindControl("MonthAdd")
                tbAmount = GVR.FindControl("AmountAdd")

                If IsNumeric(tbAmount.Text.Replace(",", "")) = 0 Then
                    lstatus.Text = MessageDlg("Amount must be in numeric.")
                    tbAmount.Focus()
                    Exit Sub
                End If
                If CFloat(tbAmount.Text) <= 0 Then
                    lstatus.Text = MessageDlg("Amount must be filled.")
                    tbAmount.Focus()
                    Exit Sub
                End If

                If SQLExecuteScalar("SELECT CashFlow, Year, Month From MsCashFlowDt WHERE CashFlow = " + QuotedStr(ViewState("Nmbr")) + " AND Year = " + QuotedStr(ddlYear.SelectedValue) + " AND Month = " + QuotedStr(ddlMonth.SelectedValue), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = MessageDlg("Year-Month " + QuotedStr(ddlYear.SelectedValue) + "-" + QuotedStr(ddlMonth.SelectedValue) + " has already been exist")
                    Exit Sub
                End If

                SQLString = "Insert Into MsCashFlowDt (CashFlow, Year, Month, Amount, UserId, UserDate) " + _
                "SELECT " + QuotedStr(ViewState("Nmbr")) + "," + QuotedStr(ddlYear.SelectedValue) + "," + _
                QuotedStr(ddlMonth.SelectedValue) + "," + tbAmount.Text.Replace(",", "") + "," + _
                QuotedStr(ViewState("UserId")) + ", getDate()"

                SQLString = Replace(SQLString, "''", "NULL")
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                bindDataGridDt()
            End If
        Catch ex As Exception
            lstatus.Text = "Item Command Dt Error" + ex.ToString
        End Try
    End Sub

    Protected Sub DataGridDt_RowDeleted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGridDt.RowDeleting
        Dim txtYear, txtMonth As Label
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            txtYear = DataGridDt.Rows(e.RowIndex).FindControl("Year")
            txtMonth = DataGridDt.Rows(e.RowIndex).FindControl("Month")

            SQLExecuteNonQuery("Delete from MsCashFlowDt where CashFlow = " + QuotedStr(ViewState("Nmbr")) + " AND Year =" + QuotedStr(txtYear.Text) + " AND Month =" + QuotedStr(txtMonth.Text), ViewState("DBConnection").ToString)
            bindDataGridDt()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub DataGridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGridDt.RowEditing
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGridDt.EditIndex = e.NewEditIndex
            DataGridDt.ShowFooter = False
            bindDataGridDt()
        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGridDt_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGridDt.RowUpdating
        Dim tbAmount As TextBox
        Dim ddlYear, ddlMonth As DropDownList
        Dim ddlYearTemp, ddlMonthTemp As Label
        Dim SQLString As String
        Dim GVR As GridViewRow
        Try
            GVR = DataGridDt.Rows(e.RowIndex)
            ddlYear = GVR.FindControl("YearDtEdit")
            ddlMonth = GVR.FindControl("MonthEdit")

            ddlYearTemp = GVR.FindControl("YearTemp")
            ddlMonthTemp = GVR.FindControl("MonthTemp")

            tbAmount = GVR.FindControl("AmountEdit")

            If IsNumeric(tbAmount.Text.Replace(",", "")) = 0 Then
                lstatus.Text = MessageDlg("Expense Charge must be in numeric.")
                tbAmount.Focus()
                Exit Sub
            End If
            If CFloat(tbAmount.Text) <= 0 Then
                lstatus.Text = MessageDlg("Expense Charge must be filled.")
                tbAmount.Focus()
                Exit Sub
            End If

            If SQLExecuteScalar("SELECT CashFlow, Year, Month From MsCashFlowDt WHERE CashFlow = " + QuotedStr(ViewState("Nmbr")) + " AND Year = " + QuotedStr(ddlYear.SelectedValue) + " AND Month = " + QuotedStr(ddlMonth.SelectedValue), ViewState("DBConnection").ToString).Length > 0 Then
                lstatus.Text = MessageDlg("Year-Month " + QuotedStr(ddlYear.SelectedValue) + "-" + QuotedStr(ddlMonth.SelectedValue) + " has already been exist")
                Exit Sub
            End If

            SQLString = "UPDATE MsCashFlowDt SET Year = " + QuotedStr(ddlYear.SelectedValue) + _
            ", Amount= " + tbAmount.Text.Replace(",", "") + _
            ", Month= " + QuotedStr(ddlMonth.SelectedValue) + " WHERE Year = " + QuotedStr(ddlYearTemp.Text) + _
            " AND Month= " + QuotedStr(ddlMonthTemp.Text) + " AND CashFlow =" + QuotedStr(ViewState("Nmbr"))

            SQLString = Replace(SQLString, "''", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            DataGridDt.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGridDt.EditIndex = -1
            bindDataGridDt()
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "Datagrid dt update Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGridDt_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles DataGridDt.Sorting
        Try
            If ViewState("SortOrder") = Nothing Or ViewState("SortOrder") = "DESC" Then
                ViewState("SortOrder") = "ASC"
            Else
                ViewState("SortOrder") = "DESC"
            End If
            ViewState("SortExpressionDt") = e.SortExpression + " " + ViewState("SortOrder")
            bindDataGridDt()
        Catch ex As Exception
            lstatus.Text = "UserDG_SortCommandError =" + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub btnBackDtTop_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBackDtTop.Click
        Try
            pnlHd.Visible = True
            pnlDt.Visible = False
        Catch ex As Exception
            lstatus.Text = "btn Back Dt Top Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        Try
            pnlHd.Visible = True
            pnlDt.Visible = False
        Catch ex As Exception
            lstatus.Text = "btn Back Dt Bottom Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
        Dim SQLString As String

        SQLString = "EXEC S_MsCashFlowDt " + QuotedStr(lbCashFlow.Text) + ", " + lbStartYear.Text + ", " + lbEndYear.Text + ", " + lbStartMonth.Text + ", " + lbEndMonth.Text + ", " + lbAmount.Text.Replace(",","") + ", " + QuotedStr(ViewState("UserId"))

        SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

        bindDataGridDt()
    End Sub

End Class
