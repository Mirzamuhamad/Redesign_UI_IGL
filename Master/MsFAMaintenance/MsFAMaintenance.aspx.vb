Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data

Partial Class Master_MsFAMaintenance_MsFAMaintenance
    Inherits System.Web.UI.Page

    Protected Sub Page_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.DataBinding

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        If Not IsPostBack Then
            InitProperty()
            ViewState("SortExpression") = Nothing
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            DataGridDt.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            'DataGrid.ShowFooter = True
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGridDt.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            'bindDataGrid()
            Dim dt As DataTable
            dt = SQLExecuteQuery("Select RptTitle, MenuName, COALESCE(MenuParam,'') AS MenuParam, COALESCE(FastRpt,'') AS FastRpt, COALESCE(ReportType,'') AS ReportType, COALESCE(ReportType2,'') AS ReportType2, COALESCE(FgPrintValue,'N') AS FgPrintValue, COALESCE(FgForceNewPage,'N') AS FgForceNewPage from MsMenu WHERE MenuId = " + QuotedStr(Request.QueryString("ContainerId").ToString), ViewState("DBConnection").ToString).Tables(0)
            lblTitle.Text = dt.Rows(0)("MenuName").ToString

        End If



        If Not Session("Result") Is Nothing Then
            If ViewState("Sender") = "SearchAccountAdd" Or ViewState("Sender") = "SearchAccountEdit" Then
                Dim Acc, Subled As New TextBox
                Dim AccName, SubledName As New Label
                Dim FgSubled As New Label
                Dim btnSubled As New Button
                If ViewState("Sender") = "SearchAccountAdd" Then
                    Acc = DataGridDt.FooterRow.FindControl("AccountAdd")
                    AccName = DataGridDt.FooterRow.FindControl("AccountNameAdd")
                Else
                    Acc = DataGridDt.Rows(DataGridDt.EditIndex).FindControl("AccountEdit")
                    AccName = DataGridDt.Rows(DataGridDt.EditIndex).FindControl("AccountNameEdit")
                End If
                Acc.Text = Session("Result")(0).ToString
                AccName.Text = Session("Result")(1).ToString
                FgSubled.Text = Session("Result")(2).ToString
                Subled.Enabled = FgSubled.Text <> "N"
                btnSubled.Visible = Subled.Enabled
                Subled.Text = ""
                SubledName.Text = ""
                Acc.Focus()
            End If

            Session("Result") = Nothing
            ViewState("Sender") = Nothing
            Session("filter") = Nothing
            Session("Column") = Nothing
        End If
        lstatus.Text = ""
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
            SqlString = "Select * from MsFAMaintenance " + StrFilter + " Order By FAMaintenanceCode "
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "FAMaintenanceCode ASC"
                ViewState("SortOrder") = "ASC"
            End If
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Protected Sub tbAccount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dr As DataRow
        Dim ds As DataSet
        Dim Acc, tb As TextBox
        Dim AccName As Label
        Dim Count As Integer
        Dim dgi As GridViewRow
        Try
            tb = sender
            If tb.ID = "AccountAdd" Then
                Count = DataGridDt.Controls(0).Controls.Count
                dgi = DataGridDt.Controls(0).Controls(Count - 2) '-1 for allowpaging = False
                Acc = dgi.FindControl("AccountAdd")
                AccName = dgi.FindControl("AccountNameAdd")
                ds = SQLExecuteQuery("SELECT * From VMsAccount Where Account = " + QuotedStr(Acc.Text), ViewState("DBConnection"))
            Else
                Count = DataGridDt.EditIndex
                dgi = DataGridDt.Rows(Count)
                Acc = dgi.FindControl("AccountEdit")
                AccName = dgi.FindControl("AccountNameEdit")
                ds = SQLExecuteQuery("SELECT * From VMsAccount Where FgType = 'PL' AND Account = " + QuotedStr(Acc.Text), ViewState("DBConnection"))
            End If
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                Acc.Text = ""
                AccName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                Acc.Text = dr("Account").ToString
                AccName.Text = dr("Description").ToString
            End If
        Catch ex As Exception
            lstatus.Text = "tb Account Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("DBConnection") = ViewState("DBConnection")
            Session("PrintType") = "Print"
            Session("SelectCommand") = "S_FormPrintMaster3 'VMsFAMaintenance','FAMaintenanceCode','FAMaintenanceName','fgAddValue'," + QuotedStr(lblTitle.Text) + ",'FA Maintenance Code','FA Maintenance  Name','Add Value'," + QuotedStr(StrFilter) + "," + QuotedStr(ViewState("UserId"))
            'lstatus.Text = Session("SelectCommand")
            'Exit Sub


            Session("ReportFile") = ".../../../Rpt/RptPrintMaster2.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)
            'lstatus.Text = "<script language='javascript'> { window.open(""../../Rpt/PrintForm.Aspx"", ""List"",""scrollbars=yes, resizable=yes,width=600,height=500"");}</script>"
        Catch ex As Exception
            lstatus.Text = "btn Print Error : " + ex.ToString
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

    Protected Sub DataGriddt_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles DataGridDt.Sorting

        Try
            If ViewState("SortOrder") = Nothing Or ViewState("SortOrder") = "DESC" Then
                ViewState("SortOrder") = "ASC"
            Else
                ViewState("SortOrder") = "DESC"
            End If
            ViewState("SortExpressiondt") = e.SortExpression + " " + ViewState("SortOrder")
            bindDataGridDt()


        Catch ex As Exception
            lstatus.Text = "UserDG_SortCommandError =" + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGrid.RowCommand
        Dim SQLString As String
        Dim dbCode, dbName As TextBox
        Dim ddlAddValue As DropDownList

        Try
            If e.CommandName = "Insert" Then
                dbCode = DataGrid.FooterRow.FindControl("FAMaintenanceCodeAdd")
                dbName = DataGrid.FooterRow.FindControl("FAMaintenanceNameAdd")
                ddlAddValue = DataGrid.FooterRow.FindControl("ValueAdd")

                If dbCode.Text.Trim.Length = 0 Then
                    lstatus.Text = "FA Maintenance Code must be filled."
                    dbCode.Focus()
                    Exit Sub
                End If
                If dbName.Text.Trim.Length = 0 Then
                    lstatus.Text = "FA Maintenance Name must be filled."
                    dbName.Focus()
                    Exit Sub
                End If



                If SQLExecuteScalar("SELECT FAMaintenanceCode From VMsFAMaintenance WHERE FAMaintenanceCode = " + QuotedStr(dbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "FA Maintenance " + QuotedStr(dbCode.Text) + " has already been exist"
                    Exit Sub
                End If

                'insert the new entry
                SQLString = "Insert into MsFAMaintenance (FAMaintenanceCode, FAMaintenanceName,FgAddValue,UserId, UserDate)" + _
                "Select " + QuotedStr(dbCode.Text) + ", " + QuotedStr(dbName.Text) + "," + QuotedStr(ddlAddValue.Text) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", getDate()"


                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                bindDataGrid()
            ElseIf e.CommandName = "Detail" Then
                Dim lbCode, lbName As Label
                Dim gvr As GridViewRow
                gvr = DataGrid.Rows(CInt(e.CommandArgument))
                lbCode = gvr.FindControl("FAMaintenanceCode")
                lbName = gvr.FindControl("FAMaintenanceName")
                'lbFormatJECode.Text = lbCode.Text
                'lbFormatJEName.Text = lbName.Text
                lbFAMaintainCode.Text = lbCode.Text
                lbFAMaintainName.Text = lbName.Text

                ViewState("Nmbr") = lbCode.Text
                pnlHd.Visible = False
                pnlDt.Visible = True
                bindDataGridDt()

            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub

    Private Sub bindDataGridDt()
        Dim tempDS As New DataSet()
        Dim DV As DataView
        Dim txt As DropDownList

        Try
            dsCurrency.ConnectionString = ViewState("DBConnection")
            tempDS = SQLExecuteQuery("SELECT A.FAMaintenance, A.Currency, A.Account,A.UserId, A.UserDate, B.CurrName,C.Description AS AccountName " + _
            " FROM MsFAMaintenanceAcc A INNER JOIN MsCurrency B ON A.Currency = B.CurrCode INNER JOIN VMsAccount C ON A.Account = C.Account " + _
            " WHERE A.FAMaintenance =" + QuotedStr(ViewState("Nmbr").ToString) + " Order By A.Currency", ViewState("DBConnection"))

            DV = tempDS.Tables(0).DefaultView

            If DV.Count = 0 Then
                Dim DT As DataTable = tempDS.Tables(0)
                ShowGridViewIfEmpty(DT, DataGridDt)
                DV = DT.DefaultView
                txt = DataGridDt.FooterRow.FindControl("ddlCurrencyAdd")
                txt.SelectedValue = ViewState("Currency")
                'Exit Sub
            Else
                DV.Sort = Session("SortExpressionDt")
                DataGridDt.DataSource = DV
                DataGridDt.DataBind()
                txt = DataGridDt.FooterRow.FindControl("ddlCurrencyAdd")
                txt.SelectedValue = ViewState("Currency")
            End If

        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Protected Sub DataGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGrid.RowDeleting
        Dim txtID As Label

        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            txtID = DataGrid.Rows(e.RowIndex).FindControl("FAMaintenanceCode")

            SQLExecuteNonQuery("Delete from MsFAMaintenance where FAMaintenanceCode = '" & txtID.Text & "'", ViewState("DBConnection"))
            SQLExecuteNonQuery("Delete from MsFAMaintenanceAcc where FAMaintenance = '" & txtID.Text & "'", ViewState("DBConnection"))

            bindDataGrid()

            'bindDataGridDt()

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
            txt = obj.FindControl("FAMaintenanceNameEdit")
            txt.Focus()
        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim dbName As TextBox
        Dim lbCode As Label
        Dim cbxType As DropDownList

        Try
            lbCode = DataGrid.Rows(e.RowIndex).FindControl("FAMaintenanceCodeEdit")
            dbName = DataGrid.Rows(e.RowIndex).FindControl("FAMaintenanceNameEdit")
            cbxType = DataGrid.Rows(e.RowIndex).FindControl("ValueEdit")

            If dbName.Text.Trim.Length = 0 Then
                lstatus.Text = "FA Maintenance Name must be filled."
                dbName.Focus()
                Exit Sub
            End If


            SQLString = "Update MsFAMaintenance set FAMaintenanceName = " + QuotedStr(dbName.Text) + _
            ",FgAddValue = " + QuotedStr(cbxType.Text) + " where FAMaintenanceCode = '" & lbCode.Text + "'"

            SQLExecuteNonQuery(SQLString, ViewState("DBConnection"))

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
    Protected Sub btnBackDtTop_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBackDtTop.Click, btnBack2.Click
        Try
            pnlHd.Visible = True
            pnlDt.Visible = False
        Catch ex As Exception
            lstatus.Text = "btn Back Dt Top Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            pnlHd.Visible = True
            pnlDt.Visible = False
        Catch ex As Exception
            lstatus.Text = "btn Back Dt Bottom Error : " + ex.ToString
        End Try
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

    Public Sub DataGridDt_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGridDt.RowCommand
        Dim tbAccount As TextBox
        Dim ddlCurrency As DropDownList
        Dim SQLString As String
        Dim GVR As GridViewRow

        Try
            If e.CommandName = "Insert" Then
                'Session("DBConnection") = Session("DBMaster")
                ViewState("StateDt") = "Insert"

                GVR = DataGridDt.FooterRow
                tbAccount = GVR.FindControl("AccountAdd")
                ddlCurrency = GVR.FindControl("ddlCurrencyAdd")

                If tbAccount.Text.Trim = "" Then
                    lstatus.Text = MessageDlg("Account must be filled")
                    tbAccount.Focus()
                    Exit Sub
                End If


                If SQLExecuteScalar("SELECT FAMaintenance From VMsFAMaintenanceAcc WHERE FAMaintenance = " + QuotedStr(lbFAMaintainCode.Text) + " AND Currency = " + QuotedStr(ddlCurrency.SelectedValue), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Currency " + QuotedStr(ddlCurrency.Text) + " has already been exist"
                    Exit Sub
                End If


                SQLString = "Insert Into MsFAMaintenanceAcc (FAMaintenance, Currency, Account, UserId, UserDate) " + _
                "SELECT " + QuotedStr(ViewState("Nmbr")) + _
                "," + QuotedStr(ddlCurrency.SelectedValue) + "," + QuotedStr(tbAccount.Text) + _
                "," + QuotedStr(ViewState("UserId")) + ", GetDate()"

                SQLExecuteNonQuery(SQLString, ViewState("DBConnection"))
                bindDataGridDt()


            ElseIf e.CommandName = "SearchAccountEdit" Or e.CommandName = "SearchAccountAdd" Then
                Dim FieldResult As String
                Session("filter") = "SELECT * From VMsAccount WHERE FgType = 'PL'"
                FieldResult = "Account, Description, FgSubled"
                Session("DBConnection") = ViewState("DBConnection")
                Session("Column") = FieldResult.Split(",")
                If e.CommandName = "SearchAccountAdd" Then
                    ViewState("Sender") = "SearchAccountAdd"
                Else
                    ViewState("Sender") = "SearchAccountEdit"
                End If
                If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenPopup();", True)
                End If

            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "InsertCommand Error:" & ex.ToString
        End Try


    End Sub

    Dim Dr, Cr As Double
    
    Protected Sub DataGridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGridDt.RowDeleting
        Dim txtID As Label
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            txtID = DataGridDt.Rows(e.RowIndex).FindControl("CurrencyCode")
            If txtID.Text.ToString = "" Then
                Exit Sub
            End If
            SQLExecuteNonQuery("Delete from MsFAMaintenanceAcc where FAMaintenance = " + QuotedStr(ViewState("Nmbr").ToString) + " AND Currency =" + QuotedStr(txtID.Text), ViewState("DBConnection"))
            bindDataGridDt()
        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub


    Public Sub DataGridDt_RowEditing(ByVal sender As Object, ByVal e As GridViewEditEventArgs) Handles DataGridDt.RowEditing

        Dim TxtID As Label
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            TxtID = DataGridDt.Rows(e.NewEditIndex).FindControl("Currency")

            If TxtID.Text.ToString = "" Then
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
        Dim tbAccount As TextBox
        Dim Currency As Label
        Dim SQLString As String
        Dim GVR As GridViewRow
        Try
            GVR = DataGridDt.Rows(e.RowIndex)
            Currency = GVR.FindControl("ddlCurrencyEdit2")
            tbAccount = GVR.FindControl("AccountEdit")

            If tbAccount.Text.Trim = "" Then
                lstatus.Text = MessageDlg("Account must be filled")
                tbAccount.Focus()
                Exit Sub
            End If

            'If SQLExecuteScalar("SELECT FAMaintenance From VMsFAMaintenanceAcc WHERE FAMaintenance = " + QuotedStr(lbFAMaintainCode.Text) + " AND Currency = " + QuotedStr(Currency.SelectedValue), Session("DBConnection").ToString).Length > 0 Then
            '    lstatus.Text = "Currency " + QuotedStr(Currency.Text) + " has already been exist"
            '    Exit Sub
            'End If          


            SQLString = "UPDATE MsFAMaintenanceAcc SET Account = " + QuotedStr(tbAccount.Text) + _
            " WHERE FAMaintenance = " + QuotedStr(lbFAMaintainCode.Text) + " AND Currency =" + QuotedStr(Currency.Text)

            SQLString = Replace(SQLString, "''", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection"))
            'lstatus.Text = SQLString

            DataGridDt.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGridDt.EditIndex = -1
            bindDataGridDt()
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "Datagrid dt update Error : " + ex.ToString
        End Try
    End Sub




End Class

