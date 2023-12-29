Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data

Partial Class Execute_Master_MsCashFlowMonth_MsCashFlowMonth
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        
        If Not IsPostBack Then
            InitProperty()
            ViewState("SortExpression") = Nothing
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            'dsYear.ConnectionString = ViewState("DBConnection")
            'dsPeriod.ConnectionString = ViewState("DBConnection")
            dsCurrency.ConnectionString = ViewState("DBConnection")

            FillCombo(ddlYear, "EXEC S_GetYeaR", False, "Year", "Year", ViewState("DBConnection"))
            FillCombo(ddlPeriod, "EXEC S_GetPeriod", False, "Period", "Description", ViewState("DBConnection"))

            If ddlYear.Items.Contains(ddlYear.Items.FindByValue(ViewState("GLYear").ToString.Trim)) Then
                ddlYear.SelectedValue = ViewState("GLYear").ToString.Trim
            End If
            If ddlPeriod.Items.Contains(ddlPeriod.Items.FindByValue(ViewState("GLPeriod").ToString.Trim)) Then
                ddlPeriod.SelectedValue = ViewState("GLPeriod").ToString.Trim
            End If

            'TbSaldoBegin.Attributes.Add("OnKeyDown", "return PressNumeric();")
            'GetDataSaldo()
            bindDataGrid()
            bindDataGridBeginning()

            MultiView1.Visible = True
            MultiView1.ActiveViewIndex = 0

            Menu1.Visible = True
            Menu1.Items.Item(0).Enabled = True
            Menu1.Items.Item(0).Selected = True

            'btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
        End If

        If Not Session("Result") Is Nothing Then
            If ViewState("Sender") = "SearchAdd" Or ViewState("Sender") = "SearchEdit" Then
                Dim tbcashFlow, tbdays, tbTotal As TextBox
                Dim lbCashFlowName As Label
                Dim ddlCurrency As DropDownList
                If ViewState("Sender") = "SearchAdd" Then
                    tbcashFlow = DataGrid.FooterRow.FindControl("CashFlowAdd")
                    lbCashFlowName = DataGrid.FooterRow.FindControl("CashFlowNameAdd")
                    tbdays = DataGrid.FooterRow.FindControl("DaysAdd")
                    ddlCurrency = DataGrid.FooterRow.FindControl("CurrencyAdd")
                    tbTotal = DataGrid.FooterRow.FindControl("TotalAdd")
                Else
                    tbcashFlow = DataGrid.Rows(DataGrid.EditIndex).FindControl("CashFlowEdit")
                    lbCashFlowName = DataGrid.Rows(DataGrid.EditIndex).FindControl("CashFlowNameEdit")
                    tbdays = DataGrid.FooterRow.FindControl("DaysEdit")
                    ddlCurrency = DataGrid.FooterRow.FindControl("CurrencyEdit")
                    tbTotal = DataGrid.FooterRow.FindControl("TotalEdit")
                End If
                tbcashFlow.Text = Session("Result")(0).ToString
                lbCashFlowName.Text = Session("Result")(1).ToString
                tbdays.Text = Session("Result")(2).ToString
                ddlCurrency.SelectedValue = Session("Result")(3).ToString
                tbTotal.Text = Session("Result")(4).ToString
                tbcashFlow.Focus()
            End If
            Session("Result") = Nothing
            ViewState("Sender") = Nothing
            Session("filter") = Nothing
            Session("Column") = Nothing
        End If
        'dsYear.ConnectionString = ViewState("DBConnection")
        'dsPeriod.ConnectionString = ViewState("DBConnection")
        dsCurrency.ConnectionString = ViewState("DBConnection")

        'lblHome.Text = ViewState("Currency")
        lblStatus.Text = ""
    End Sub

    'Sub GetDataSaldo()
    '    Dim Dr As DataRow
    '    Dim DT As DataTable

    '    Try
    '        DT = SQLExecuteQuery("select * from MsCashFlowBegin Where Year =" + ddlYear.SelectedValue.ToString + " And Month = " + ddlPeriod.SelectedValue.ToString, ViewState("DBConnection").ToString).Tables(0)
    '        If DT.Rows.Count > 0 Then
    '            Dr = DT.Rows(0)
    '            TbSaldoBegin.Text = FormatFloat(Dr("Total"), ViewState("DigitHome"))
    '        Else
    '            TbSaldoBegin.Text = FormatFloat("0", ViewState("DigitHome"))
    '        End If
    '    Catch ex As Exception
    '        lblStatus.Text = "Get Saldo Amount Error : " + ex.ToString
    '    End Try
    'End Sub

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
                    lblStatus.Text = "<script language='javascript'> {alert('You are not authorized to edit record. Please contact administrator')}</script>"
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
                    lblStatus.Text = "<script language='javascript'> {alert('You are not authorized to delete record. Please contact administrator')}</script>"
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Check Menu Level Error : " + ex.ToString)
        End Try
    End Function

    'Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
    '    Try
    '        DataGrid.PageIndex = 0
    '        DataGrid.EditIndex = -1
    '        bindDataGrid()
    '        'tbFilter.Text = ""
    '        'tbfilter2.Text = ""
    '    Catch ex As Exception
    '        lstatus.Text = "Btn Search Error : " + vbCrLf + ex.ToString
    '    End Try
    'End Sub

    'Protected Sub btnExpand_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExpand.Click
    '    Try
    '        tbfilter2.Text = ""
    '        If pnlSearch.Visible Then
    '            pnlSearch.Visible = False
    '        Else
    '            pnlSearch.Visible = True
    '        End If
    '    Catch ex As Exception
    '        lstatus.Text = "btn Expand Error : " + ex.ToString
    '    End Try
    'End Sub

    Private Sub bindDataGrid()
        Dim SqlString As String
        Dim GVR As GridViewRow
        Dim total, days As TextBox
        Dim Currency As DropDownList

        Try
            'SqlString = "Select * from VMsWIP " + StrFilter
            'StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            SqlString = "Select CashFlow, CashFlowName, Days, Currency, dbo.FormatFloat(Total, dbo.DigitCurrForex(Currency)) As Total from V_MsCashFlowMonth WHERE Year = " + ddlYear.SelectedValue.ToString + " and Month = " + ddlPeriod.SelectedValue.ToString
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "CashFlow Asc"
            End If
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)

            'GetDataSaldo()

            GVR = DataGrid.FooterRow
            total = GVR.FindControl("TotalAdd")
            days = GVR.FindControl("DaysAdd")
            Currency = GVR.FindControl("CurrencyAdd")
            Currency.SelectedValue = ViewState("Currency").ToString
            total.Attributes.Add("OnKeyDown", "return PressNumeric();")
            days.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Catch ex As Exception
            lblStatus.Text = lblStatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Private Sub bindDataGridBeginning()
        Dim SqlString As String
        Dim tempDS As New DataSet()
        Dim GVR As GridViewRow
        Dim total As TextBox
        Dim Currency As DropDownList

        Try
            SqlString = "Select Currency, Total From MsCashFlowBegin WHERE Year = " + ddlYear.SelectedValue.ToString + " and Month = " + ddlPeriod.SelectedValue.ToString
            If ViewState("SortExpression2") = Nothing Then
                ViewState("SortExpression2") = "Currency Asc"
            End If

            BindDataMaster(SqlString, DataGridBegin, ViewState("SortExpression2"), ViewState("DBConnection").ToString)

            'GetDataSaldo()

            GVR = DataGridBegin.FooterRow

            total = GVR.FindControl("TotalAdd")
            total.Attributes.Add("OnKeyDown", "return PressNumeric();")

            Currency = GVR.FindControl("CurrencyAdd")
            Currency.SelectedValue = ViewState("Currency").ToString
        Catch ex As Exception
            lblStatus.Text = lblStatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    'Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click

    '    Dim StrFilter As String
    '    Try
    '        StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
    '        Session("PrintType") = "Print"
    '        Session("DBConnection") = ViewState("DBConnection")
    '        Session("SelectCommand") = "S_FormPrintMaster3 'MsCashFlowBegin','Year','Month','Total','Cash Flow Begin','Year','Month','Total'," + QuotedStr(StrFilter) + "," + QuotedStr(ViewState("UserId"))
    '        Session("ReportFile") = ".../../../Rpt/RptPrintMaster2.frx"
    '        AttachScript("openprintdlg();", Page, Me.GetType)
    '    Catch ex As Exception
    '        lstatus.Text = "btn Print Error : " + ex.ToString
    '    End Try
    'End Sub

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
            lblStatus.Text = "UserDG_SortCommandError =" + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGrid.RowCommand
        Dim SQLString As String
        Dim ddlCurrency As DropDownList
        Dim tbCashFlow, tbDays, tbtotal As TextBox

        Try
            If e.CommandName = "Insert" Then
                tbCashFlow = DataGrid.FooterRow.FindControl("CashFlowAdd")
                tbDays = DataGrid.FooterRow.FindControl("DaysAdd")
                tbtotal = DataGrid.FooterRow.FindControl("TotalAdd")
                ddlCurrency = DataGrid.FooterRow.FindControl("CurrencyAdd")
                If tbCashFlow.Text.Trim.Length = 0 Then
                    lblStatus.Text = "Cash Flow must be filled."
                    tbCashFlow.Focus()
                    Exit Sub
                End If
                If tbDays.Text.Trim.Length = 0 Then
                    lblStatus.Text = "Days must be filled."
                    tbDays.Focus()
                    Exit Sub
                End If
                If Not (CInt(tbDays.Text) >= 1 And CInt(tbDays.Text) <= 31) Then
                    lblStatus.Text = "Days must be in range 1 .. 31"
                    tbDays.Focus()
                    Exit Sub
                End If
                If ddlCurrency.SelectedValue.Trim = "" Then
                    lblStatus.Text = "Currency must be filled."
                    ddlCurrency.Focus()
                    Exit Sub
                End If
                If tbtotal.Text.Trim.Length = 0 Then
                    lblStatus.Text = "Total must be filled."
                    tbtotal.Focus()
                    Exit Sub
                End If

                If SQLExecuteScalar("SELECT CashFlow From MsCashFlowMonth WHERE Year = " + ddlYear.SelectedValue.ToString + " And Month = " + ddlPeriod.SelectedValue.ToString + " and CashFlow = " + QuotedStr(tbCashFlow.Text.Trim), ViewState("DBConnection").ToString).Length > 0 Then
                    lblStatus.Text = "Cash Flow '" + tbCashFlow.Text.Trim + "' has already been exist"
                    Exit Sub
                End If

                'insert the new entry
                SQLString = "Insert into MsCashFlowMonth (Year, Month, CashFlow, Days, Currency, Total, UserId, UserDate) " + _
                "SELECT " + ddlYear.SelectedValue.ToString + ", " + ddlPeriod.SelectedValue.ToString + ", " + _
                QuotedStr(tbCashFlow.Text.Trim) + "," + tbDays.Text.Replace(",", "") + ", " + QuotedStr(ddlCurrency.SelectedValue) + ", " + tbtotal.Text.Replace(",", "") + _
                ", " + QuotedStr(ViewState("UserId").ToString) + ", GetDate()"

                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                bindDataGrid()
            ElseIf e.CommandName = "SearchAdd" Or e.CommandName = "SearchEdit" Then
                Dim FieldResult As String
                Session("DBConnection") = ViewState("DBConnection")
                Session("filter") = "Select CashFlowCode, CashFlowName, Days, Currency, dbo.FormatFloat(Nominal,2) As Total FROM MsCashFlow "
                FieldResult = "CashFlowCode, CashFlowName, Days, Currency, Total"
                Session("Column") = FieldResult.Split(",")
                If e.CommandName = "SearchAdd" Then
                    ViewState("Sender") = "SearchAdd"
                Else
                    ViewState("Sender") = "SearchEdit"
                End If

                If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenPopup();", True)
                End If
            End If
        Catch ex As Exception
            lblStatus.Text = lblStatus.Text + "Insert Command Error:" & ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGrid.RowDeleting
        Dim txtCode As Label

        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            txtCode = DataGrid.Rows(e.RowIndex).FindControl("CashFlow")
            SQLExecuteNonQuery("Delete MsCashFlowMonth where Year = " & ddlYear.SelectedValue.ToString & " And Month = " & ddlPeriod.SelectedValue.ToString & " AND CashFlow = " + QuotedStr(txtCode.Text.Trim), ViewState("DBConnection").ToString)
            bindDataGrid()
        Catch ex As Exception
            lblStatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        Dim tbCashFlow, tbDays, tbtotal As TextBox
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()
            obj = DataGrid.Rows(e.NewEditIndex)
            tbCashFlow = obj.FindControl("CashFlowEdit")
            tbCashFlow.Enabled = False

            tbDays = DataGrid.Rows(e.NewEditIndex).FindControl("DaysEdit")
            tbDays.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbDays.Focus()

            tbtotal = DataGrid.Rows(e.NewEditIndex).FindControl("TotalEdit")
            tbtotal.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Catch ex As Exception
            lblStatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim ddlCurrency As DropDownList
        Dim tbCashFlow, tbDays, tbtotal As TextBox

        Try
            tbCashFlow = DataGrid.Rows(e.RowIndex).FindControl("CashFlowEdit")
            ddlCurrency = DataGrid.Rows(e.RowIndex).FindControl("CurrencyEdit")
            tbDays = DataGrid.Rows(e.RowIndex).FindControl("DaysEdit")
            tbtotal = DataGrid.Rows(e.RowIndex).FindControl("TotalEdit")

            If tbDays.Text.Trim.Length = 0 Then
                lblStatus.Text = "Days must be filled."
                tbDays.Focus()
                Exit Sub
            End If
            If Not (CInt(tbDays.Text) >= 1 And CInt(tbDays.Text) <= 31) Then
                lblStatus.Text = "Days must be in range 1 .. 31"
                tbDays.Focus()
                Exit Sub
            End If
            If ddlCurrency.SelectedValue.Trim = "" Then
                lblStatus.Text = "Currency must be filled."
                ddlCurrency.Focus()
                Exit Sub
            End If
            If tbtotal.Text.Trim.Length = 0 Then
                lblStatus.Text = "Total must be filled."
                tbtotal.Focus()
                Exit Sub
            End If

            SQLString = "Update MsCashFlowMonth set Days= " + tbDays.Text.Trim + ", Currency = " + QuotedStr(ddlCurrency.SelectedValue) + ", Total = " + tbtotal.Text.Replace(",", "") + _
            " where Year = " & QuotedStr(ddlYear.SelectedValue) & " And Month = " & QuotedStr(ddlPeriod.SelectedValue) + " and CashFlow = " + QuotedStr(tbCashFlow.Text.Trim)
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid()
        Catch ex As Exception
            lblStatus.Text = lblStatus.Text + "DataGrid_Update Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles DataGrid.RowCancelingEdit
        Try
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid()
        Catch ex As Exception
            lblStatus.Text = "DataGrid_Cancel Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub ddlYear_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlYear.SelectedIndexChanged
        bindDataGrid()
        bindDataGridBeginning()
    End Sub

    Protected Sub ddlPeriod_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPeriod.SelectedIndexChanged
        bindDataGrid()
        bindDataGridBeginning()
    End Sub

    'Protected Sub btnApply_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApply.Click
    '    Try
    '        SQLExecuteNonQuery("S_MsCashFlowMonthApply " + ddlYear.SelectedValue.ToString + ", " + ddlPeriod.SelectedValue.ToString + ", " + TbSaldoBegin.Text.Replace(",", "") + ", " + QuotedStr(ViewState("UserId")), ViewState("DBConnection").ToString)
    '    Catch ex As Exception
    '        lblStatus.Text = "btnApply_Click : " + vbCrLf + ex.ToString
    '    End Try
    'End Sub

    Protected Sub BtnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGo.Click
        Try
            'lstatus.Text = "EXEC S_MsCashFlowMonthGetData " + ddlYear.SelectedValue.ToString + ", " + ddlPeriod.SelectedValue.ToString + ", " + QuotedStr(ddlOverwrite.SelectedValue.ToString) + ", " + QuotedStr(ddlDataFrom.SelectedValue.ToString) + ", " + QuotedStr(ViewState("UserId"))
            'Exit Sub
            SQLExecuteNonQuery("EXEC S_MsCashFlowMonthGetData " + ddlYear.SelectedValue.ToString + ", " + ddlPeriod.SelectedValue.ToString + ", " + QuotedStr(ddlOverwrite.SelectedValue.ToString) + ", " + QuotedStr(ddlDataFrom.SelectedValue.ToString) + ", " + QuotedStr(ViewState("UserId")), ViewState("DBConnection").ToString)
            bindDataGrid()
        Catch ex As Exception
            lblStatus.Text = "btnApply_Click : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub tbCashFlow_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dr As DataRow
        Dim ds As DataSet
        Dim tbCode, tb, tbDays, tbTotal As TextBox
        Dim tbName As Label
        Dim Count As Integer
        Dim dgi As GridViewRow
        Dim ddlCurr As DropDownList
        Try
            tb = sender
            If tb.ID = "CashFlowAdd" Then
                ' masalah di sini
                Count = DataGrid.Controls(0).Controls.Count
                dgi = DataGrid.Controls(0).Controls(Count - 2) '-1 for allowpaging = False   - 2 allowpaging = True
                tbCode = dgi.FindControl("CashFlowAdd")
                tbName = dgi.FindControl("CashFlowNameAdd")
                tbDays = dgi.FindControl("DaysAdd")
                ddlCurr = dgi.FindControl("CurrencyAdd")
                tbTotal = dgi.FindControl("TotalAdd")
            Else
                Count = DataGrid.EditIndex
                dgi = DataGrid.Rows(Count)
                tbCode = dgi.FindControl("CashFlowEdit")
                tbName = dgi.FindControl("CashFlowNameEdit")
                tbDays = dgi.FindControl("DaysEdit")
                ddlCurr = dgi.FindControl("CurrencyEdit")
                tbTotal = dgi.FindControl("TotalEdit")
            End If
            ds = SQLExecuteQuery("Select CashFlowCode, CashFlowName, Days, Currency, Nominal from MsCashFlow Where CashFlowCode = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbCode.Text = ""
                tbName.Text = ""
                ddlCurr.SelectedValue = ViewState("Currency").ToString
                tbDays.Text = "1"
                tbTotal.Text = "0"
            Else
                dr = ds.Tables(0).Rows(0)
                tbCode.Text = dr("CashFlowCode").ToString
                tbName.Text = dr("CashFlowName").ToString
                ddlCurr.SelectedValue = dr("Currency").ToString
                tbDays.Text = dr("Days").ToString
                tbTotal.Text = FormatFloat(dr("Nominal").ToString, ViewState("DigitHome"))
            End If
        Catch ex As Exception
            lblStatus.Text = "tb tbCashFlow_TextChanged Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        MultiView1.ActiveViewIndex = Int32.Parse(e.Item.Value)
        Dim i As Integer
        For i = 0 To Menu1.Items.Count - 1
            If i = e.Item.Value Then
            ElseIf Menu1.Items(1).Selected = True Then
                Dim GVR As GridViewRow = Nothing
                Try
                    If CheckMenuLevell("Insert") = False Then
                        Exit Sub
                    End If
                Catch ex As Exception
                    lblStatus.Text = "Menu1_MenuItemClick Error : " + ex.ToString
                End Try
            End If
        Next
    End Sub

    Function CheckMenuLevell(ByVal CommandName As String) As Boolean
        Try
            If CommandName = "Insert" Then
                If ViewState("MenuLevel").Rows(0)("FgInsert") = "N" Then
                    lblStatus.Text = MessageDlg("You are not authorized to edit record. Please contact administrator")
                    Return False
                    Exit Function
                End If
            End If
            If CommandName = "Edit" Then
                If ViewState("MenuLevel").Rows(0)("FgEdit") = "N" Then
                    lblStatus.Text = MessageDlg("You are not authorized to edit record. Please contact administrator")
                    Return False
                    Exit Function
                End If
            End If
            If CommandName = "Delete" Then
                If ViewState("MenuLevel").Rows(0)("FgDelete") = "N" Then
                    lblStatus.Text = MessageDlg("You are not authorized to delete record. Please contact administrator")
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Check Menu Level Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub DataGridBegin_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGridBegin.PageIndexChanging
        DataGridBegin.PageIndex = e.NewPageIndex
        'If DataGridBegin.EditIndex <> -1 Then
        '    DataGrid2_RowCancelingEdit(Nothing, Nothing)
        'End If
        bindDataGridBeginning()
    End Sub

    Protected Sub DataGridBegin_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles DataGridBegin.RowCancelingEdit
        Try
            DataGridBegin.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGridBegin.EditIndex = -1
            bindDataGridBeginning()
        Catch ex As Exception
            lblStatus.Text = "DataGridBegin_RowCancelingEdit Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub DataGridBegin_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGridBegin.RowCommand
        Dim curr As DropDownList
        Dim total As TextBox
        Dim SQLString As String

        Try
            If e.CommandName = "Insert" Then
                curr = DataGridBegin.FooterRow.FindControl("CurrencyAdd")
                total = DataGridBegin.FooterRow.FindControl("TotalAdd")

                If SQLExecuteScalar("SELECT Year, Month, Currency From MsCashFlowBegin WHERE Year = " + ddlYear.SelectedValue.ToString + " and Month = " + ddlPeriod.SelectedValue.ToString + " And Currency = " + QuotedStr(curr.SelectedValue), ViewState("DBConnection").ToString).Length > 0 Then
                    lblStatus.Text = MessageDlg("Year " + ddlYear.SelectedValue.ToString + " Month " + ddlPeriod.SelectedItem.ToString + " And Currency " + curr.SelectedValue + " has already been exist")
                    Exit Sub
                End If

                SQLString = "Insert into MsCashFlowBegin (Year, Month, Currency, Total, UserId, UserDate ) " + _
                            "SELECT " + QuotedStr(ddlYear.SelectedValue.ToString) + ", " + QuotedStr(ddlPeriod.SelectedValue) + _
                            "," + QuotedStr(curr.SelectedValue) + _
                            "," + total.Text.Replace(",", "") + _
                            "," + QuotedStr(ViewState("UserId").ToString) + ", getDate()"

                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                bindDataGridBeginning()
            End If
        Catch ex As Exception
            lblStatus.Text = "DataGridBegin_RowCommand Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGridBegin_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGridBegin.RowDeleting
        Dim curr As Label

        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            curr = DataGridBegin.Rows(e.RowIndex).FindControl("Currency")
            SQLExecuteNonQuery("Delete MsCashFlowBegin where Year = " & ddlYear.SelectedValue.ToString & " And Month = " & ddlPeriod.SelectedValue.ToString & " And Currency = " + QuotedStr(curr.Text), ViewState("DBConnection").ToString)
            bindDataGridBeginning()
        Catch ex As Exception
            lblStatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub DataGridBegin_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGridBegin.RowEditing
        Dim total As TextBox

        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If

            DataGridBegin.EditIndex = e.NewEditIndex
            DataGridBegin.ShowFooter = False
            bindDataGridBeginning()

            total = DataGridBegin.Rows(e.NewEditIndex).FindControl("TotalEdit")
            total.Attributes.Add("OnKeyDown", "return PressNumeric();")
            total.Focus()
        Catch ex As Exception
            lblStatus.Text = "DataGridBegin_RowEditing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGridBegin_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGridBegin.RowUpdating
        Dim SQLString As String
        Dim curr, currTemp As DropDownList
        Dim total As TextBox

        Try
            curr = DataGridBegin.Rows(e.RowIndex).FindControl("CurrencyEdit")
            currTemp = DataGridBegin.Rows(e.RowIndex).FindControl("CurrencyEditTemp")
            total = DataGridBegin.Rows(e.RowIndex).FindControl("TotalEdit")

            If curr.SelectedValue.Trim = "" Then
                lblStatus.Text = "Currency must be filled."
                curr.Focus()
                Exit Sub
            End If
            If total.Text.Trim.Length = 0 Then
                lblStatus.Text = "Total must be filled."
                total.Focus()
                Exit Sub
            End If

            If curr.SelectedValue <> currTemp.SelectedValue Then
                If SQLExecuteScalar("SELECT Year, Month, Currency From MsCashFlowBegin WHERE Year = " + ddlYear.SelectedValue.ToString + " and Month = " + ddlPeriod.SelectedValue.ToString + " And Currency = " + QuotedStr(curr.SelectedValue), ViewState("DBConnection").ToString).Length > 0 Then
                    lblStatus.Text = MessageDlg("Year " + ddlYear.SelectedValue.ToString + " Month " + ddlPeriod.SelectedItem.ToString + " And Currency " + curr.SelectedValue + " has already been exist")
                    Exit Sub
                End If
            End If

            SQLString = "Update MsCashFlowBegin Set Currency= " + QuotedStr(curr.SelectedValue) + ", Total = " + total.Text.Replace(",", "") + _
            " where Year = " & QuotedStr(ddlYear.SelectedValue) & " And Month = " & QuotedStr(ddlPeriod.SelectedValue) + " And Currency = " + QuotedStr(currTemp.SelectedValue)

            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            DataGridBegin.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGridBegin.EditIndex = -1
            bindDataGridBeginning()
        Catch ex As Exception
            lblStatus.Text = "DataGridBegin_RowUpdating Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub DataGridBegin_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles DataGridBegin.Sorting
        Try
            If ViewState("SortOrder2") = Nothing Or ViewState("SortOrder2") = "DESC" Then
                ViewState("SortOrder2") = "ASC"
            Else
                ViewState("SortOrder2") = "DESC"
            End If
            ViewState("SortExpression2") = e.SortExpression + " " + ViewState("SortOrder2")
            bindDataGridBeginning()
        Catch ex As Exception
            lblStatus.Text = "DataGridBegin_Sorting Error =" + vbCrLf + ex.ToString
        End Try
    End Sub
End Class
