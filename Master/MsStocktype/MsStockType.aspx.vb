Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data
Partial Class MsStockType_MsStockType
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
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            Dim dt As DataTable
            dt = SQLExecuteQuery("Select RptTitle, MenuName, COALESCE(MenuParam,'') AS MenuParam, COALESCE(FastRpt,'') AS FastRpt, COALESCE(ReportType,'') AS ReportType, COALESCE(ReportType2,'') AS ReportType2, COALESCE(FgPrintValue,'N') AS FgPrintValue, COALESCE(FgForceNewPage,'N') AS FgForceNewPage from MsMenu WHERE MenuId = " + QuotedStr(Request.QueryString("ContainerId").ToString), ViewState("DBConnection").ToString).Tables(0)
            lblTitle.Text = dt.Rows(0)("MenuName").ToString

            'bindDataGrid()
        End If
        If Not Session("Result") Is Nothing Then
            If Session("Sender") = "btnAccAdd" Or Session("Sender") = "btnAccEdit" Then
                Dim Account As TextBox
                Dim AccountName As Label
                If Session("Sender") = "btnAccAdd" Then
                    Account = DataGrid.FooterRow.FindControl("AccountAdd")
                    AccountName = DataGrid.FooterRow.FindControl("AccountNameAdd")
                Else
                    Account = DataGrid.Rows(DataGrid.EditIndex).FindControl("AccountEdit")
                    AccountName = DataGrid.Rows(DataGrid.EditIndex).FindControl("AccountNameEdit")
                End If
                Account.Text = Session("Result")(0).ToString
                AccountName.Text = Session("Result")(1).ToString
                Account.Focus()
            End If
            If Session("Sender") = "btnAccIncomeAdd" Or Session("Sender") = "btnAccIncomeEdit" Then
                Dim Account2 As TextBox
                Dim AccountName2 As Label
                If Session("Sender") = "btnAccIncomeAdd" Then
                    Account2 = DataGrid.FooterRow.FindControl("AccountIncomeAdd")
                    AccountName2 = DataGrid.FooterRow.FindControl("AccountNameIncomeAdd")
                Else
                    Account2 = DataGrid.Rows(DataGrid.EditIndex).FindControl("AccountIncomeEdit")
                    AccountName2 = DataGrid.Rows(DataGrid.EditIndex).FindControl("AccountNameIncomeEdit")
                End If
                Account2.Text = Session("Result")(0).ToString
                AccountName2.Text = Session("Result")(1).ToString
                Account2.Focus()
            End If
            Session("Result") = Nothing
            Session("Sender") = Nothing
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
            SqlString = "SELECT * FROM VMsStockType " + StrFilter + " Order By Stock_Type_Name "
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "Stock_Type_Code ASC"
                ViewState("SortOrder") = "ASC"
            End If
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try

    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim StrFilter As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("PrintType") = "Print"
            Session("SelectCommand") = "S_FormPrintMaster4 'VMsStockType','Stock_Type_Code','Stock_Type_Name','AccountExpenseName','AccountIncomeName'," + QuotedStr(lblTitle.Text) + ",'Stock Type Code','Stock Type Name','Account Expese','Account Income'," + QuotedStr(StrFilter) + ", " + QuotedStr(ViewState("UserId"))
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster3.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)
            lstatus.Text = "<script language='javascript'> { window.open(""../../Rpt/PrintForm.Aspx"", ""List"",""scrollbars=yes, resizable=yes,width=600,height=500"");}</script>"
        Catch ex As Exception
            lstatus.Text = "btn print Error = " + ex.ToString
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
        Dim dbCode, dbName, cbxAccount, cbxAccountIncome As TextBox
        
        Try
            If e.CommandName = "Insert" Then
                dbCode = DataGrid.FooterRow.FindControl("StockTypeCodeAdd")
                dbName = DataGrid.FooterRow.FindControl("StockTypeNameAdd")
                cbxAccount = DataGrid.FooterRow.FindControl("AccountAdd")
                cbxAccountIncome = DataGrid.FooterRow.FindControl("AccountIncomeAdd")
    
                If dbCode.Text.Trim.Length = 0 Then
                    lstatus.Text = "Stock Type Code must be filled."
                    dbCode.Focus()
                    Exit Sub
                End If
                If dbName.Text.Trim.Length = 0 Then
                    lstatus.Text = "Stock Type Name must be filled."
                    dbName.Focus()
                    Exit Sub
                End If

                If SQLExecuteScalar("SELECT Stock_Type_Code From VMsStockType WHERE Stock_Type_Code = " + QuotedStr(dbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Stock Type " + QuotedStr(dbCode.Text) + " has already been exist"
                    Exit Sub
                End If

                'insert the new entry
                SQLString = "Insert into MsStockType (StockTypeCode, StockTypeName, Account, AccountIncome, UserId, UserDate ) " + _
                "SELECT " + QuotedStr(dbCode.Text) + ", " + QuotedStr(dbName.Text) + ", " + QuotedStr(cbxAccount.Text) + ", " + QuotedStr(cbxAccountIncome.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", getDate()"
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                bindDataGrid()

            ElseIf e.CommandName = "btnAccAdd" Or e.CommandName = "btnAccEdit" Then
                Dim FieldResult As String
                Session("DBConnection") = ViewState("DBConnection")
                If e.CommandName = "btnAccAdd" Then
                    Session("filter") = "Select * FROM VMsAccount where FgNormal = 'D' "
                    Session("Sender") = "btnAccAdd"
                Else
                    Session("filter") = "Select * FROM VMsAccount where FgNormal = 'D' "
                    Session("Sender") = "btnAccEdit"
                End If

                FieldResult = "Account, Description"
                Session("Column") = FieldResult.Split(",")
                AttachScript("OpenSearchDlg();", Page, Me.GetType())
            ElseIf e.CommandName = "btnAccIncomeAdd" Or e.CommandName = "btnAccIncomeEdit" Then
                Dim FieldResult As String
                Session("DBConnection") = ViewState("DBConnection")
                If e.CommandName = "btnAccIncomeAdd" Then
                    Session("filter") = "Select * FROM VMsAccount where FgNormal = 'C' "
                    Session("Sender") = "btnAccIncomeAdd"
                Else
                    Session("filter") = "Select * FROM VMsAccount where FgNormal = 'C' "
                    Session("Sender") = "btnAccIncomeEdit"
                End If

                FieldResult = "Account, Description"
                Session("Column") = FieldResult.Split(",")
                AttachScript("OpenSearchDlg();", Page, Me.GetType())
            ElseIf e.CommandName = "Detail" Then
                Dim gvr As GridViewRow
                Dim lbCode, lbName As Label
                Dim paramgo As String
                gvr = DataGrid.Rows(CInt(e.CommandArgument))
                lbCode = gvr.FindControl("StockTypeCode")
                lbName = gvr.FindControl("StockTypeName")
                If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                    paramgo = lbCode.Text + "|" + lbName.Text
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenAssign('Stock Type',  '" + Request.QueryString("KeyId") + "','" + paramgo + "','AssMsStockTypeDt');", True)
                End If
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
            txtID = DataGrid.Rows(e.RowIndex).FindControl("StockTypeCode")

            SQLExecuteNonQuery("Delete from MsStockType where StockTypeCode = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
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
            txt = obj.FindControl("StockTypeNameEdit")
            txt.Focus()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim dbName, cbxAccount, cbxAccountIncome As TextBox
        Dim lbCode As Label

        Try
            lbCode = DataGrid.Rows(e.RowIndex).FindControl("StockTypeCodeEdit")
            dbName = DataGrid.Rows(e.RowIndex).FindControl("StockTypeNameEdit")
            cbxAccount = DataGrid.Rows(e.RowIndex).FindControl("AccountEdit")
            cbxAccountIncome = DataGrid.Rows(e.RowIndex).FindControl("AccountIncomeEdit")

            If dbName.Text.Trim.Length = 0 Then
                lstatus.Text = "Stock Type Name must be filled."
                dbName.Focus()
                Exit Sub
            End If

            SQLString = "Update MsStockType set StockTypeName= " + QuotedStr(dbName.Text) + "," & _
            "AccountIncome = " + QuotedStr(cbxAccountIncome.Text) + ", " & _
            "Account = " + QuotedStr(cbxAccount.Text) + " where StockTypeCode = " & QuotedStr(lbCode.Text)

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

    Protected Sub AccountIncomeEdit_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dr As DataRow
        Dim ds As DataSet
        Dim Acc As TextBox
        Dim AccName As Label
        Dim Count As Integer
        Dim dgi As GridViewRow
        Try
            Count = DataGrid.EditIndex
            dgi = DataGrid.Rows(Count)
            Acc = dgi.FindControl("AccountIncomeEdit")
            AccName = dgi.FindControl("AccountNameIncomeEdit")


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

    Protected Sub AccountIncomeAdd_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dr As DataRow
        Dim ds As DataSet
        Dim Acc, tb As TextBox
        Dim AccName As Label
        Dim Count As Integer
        Dim dgi As GridViewRow
        Try
            tb = sender
            If tb.ID = "AccountIncomeAdd" Then
                Count = DataGrid.Controls(0).Controls.Count
                dgi = DataGrid.Controls(0).Controls(Count - 2) '-1 for allowpaging = False
                Acc = dgi.FindControl("AccountIncomeAdd")
                AccName = dgi.FindControl("AccountNameIncomeAdd")
            Else
                Count = DataGrid.EditIndex
                dgi = DataGrid.Rows(Count)
                Acc = dgi.FindControl("AccountIncomeEdit")
                AccName = dgi.FindControl("AccountNameIncomeEdit")
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
End Class
