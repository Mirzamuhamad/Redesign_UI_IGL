Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data

Partial Class Master_MsPLCategorySub_MsPLCategorySub
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            InitProperty()
            ViewState("SortExpression") = Nothing
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"

            'bindDataGrid()
        End If
        If Not Session("Result") Is Nothing Then
            If ViewState("Sender") = "SearchAdd" Or ViewState("Sender") = "SearchEdit" Then
                Dim CategoryCode As TextBox
                Dim CategoryName As Label
                If ViewState("Sender") = "SearchAdd" Then
                    CategoryCode = DataGrid.FooterRow.FindControl("PLCategoryCodeAdd")
                    CategoryName = DataGrid.FooterRow.FindControl("PLCategoryNameAdd")
                Else
                    CategoryCode = DataGrid.Rows(DataGrid.EditIndex).FindControl("PLCategoryCodeEdit")
                    CategoryName = DataGrid.Rows(DataGrid.EditIndex).FindControl("PLCategoryNameEdit")
                End If
                CategoryCode.Text = Session("Result")(0).ToString
                CategoryName.Text = Session("Result")(1).ToString
                CategoryCode.Focus()
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
            SqlString = "SELECT * FROM VMsPLCategorySub " + StrFilter

            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Protected Sub DataGrid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGrid.PageIndexChanging
        DataGrid.PageIndex = e.NewPageIndex
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
            lstatus.Text = "Datagrid Sort CommandError =" + vbCrLf + ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGrid.RowCommand
        Dim SQLString As String
        Dim dbCode, dbName, dbCategoryCode As TextBox

        Try
            If e.CommandName = "Insert" Then
                dbCode = DataGrid.FooterRow.FindControl("PLCategorySubCodeAdd")
                dbName = DataGrid.FooterRow.FindControl("PLCategorySubNameAdd")
                dbCategoryCode = DataGrid.FooterRow.FindControl("PLCategoryCodeAdd")

                If dbCode.Text.Trim.Length = 0 Then
                    lstatus.Text = "Code must be filled."
                    dbCode.Focus()
                    Exit Sub
                End If
                If dbName.Text.Trim.Length = 0 Then
                    lstatus.Text = "Description must be filled."
                    dbName.Focus()
                    Exit Sub
                End If

                If dbCategoryCode.Text.Trim.Length = 0 Then
                    lstatus.Text = "Category must be filled."
                    dbCategoryCode.Focus()
                    Exit Sub
                End If

                If SQLExecuteScalar("SELECT PLCategorySubCode From MsPLCategorySub  WHERE PLCategorySubCode = " + QuotedStr(dbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Code " + QuotedStr(dbCode.Text) + " has already been exist"
                    Exit Sub
                End If

                'insert the new entry
                SQLString = "Insert into MsPLCategorySub (PLCategorySubCode, PLCategorySubName, PLCategoryCode, UserId, UserDate ) " + _
                "SELECT " + QuotedStr(dbCode.Text) + ", " + QuotedStr(dbName.Text) + ", " + QuotedStr(dbCategoryCode.Text) + _
                "," + QuotedStr(ViewState("UserId").ToString) + ", GetDate()"
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                bindDataGrid()
            ElseIf e.CommandName = "SearchAdd" Or e.CommandName = "SearchEdit" Then
                Dim FieldResult As String
                Session("DBConnection") = ViewState("DBConnection")
                Session("filter") = "Select PLCategoryCode, PLCategoryName FROM VMsPLCategory"
                FieldResult = "PLCategoryCode, PLCategoryName"
                Session("Column") = FieldResult.Split(",")
                If e.CommandName = "SearchAdd" Then
                    ViewState("Sender") = "SearchAdd"
                Else
                    ViewState("Sender") = "SearchEdit"
                End If
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "tes", "OpenPopup();", True)
                'If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                '    Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenPopup();", True)
                'End If
            End If
        Catch ex As Exception
            lstatus.Text = "Row Command Error:" & ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGrid.RowDeleting
        Dim txtID As Label
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            txtID = DataGrid.Rows(e.RowIndex).FindControl("PLCategorySubCode")
            SQLExecuteNonQuery("Delete from MsPLCategorySub where PLCategorySubCode = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
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
            txt = obj.FindControl("PLCategorySubNameEdit")
            txt.Focus()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim dbName, dbCategoryCode As TextBox
        Dim lbCode As Label

        Try
            lbCode = DataGrid.Rows(e.RowIndex).FindControl("PLCategorySubCodeEdit")
            dbName = DataGrid.Rows(e.RowIndex).FindControl("PLCategorySubNameEdit")
            dbCategoryCode = DataGrid.Rows(e.RowIndex).FindControl("PLCategoryCodeEdit")

            If dbName.Text.Trim.Length = 0 Then
                lstatus.Text = "Description must be filled."
                dbName.Focus()
                Exit Sub
            End If

            If dbCategoryCode.Text.Trim.Length = 0 Then
                lstatus.Text = "Category must be filled."
                dbCategoryCode.Focus()
                Exit Sub
            End If

            SQLString = "Update MsPLCategorySub set PLCategorySubName = " + QuotedStr(dbName.Text) + "," & _
            "PLCategoryCode = " + QuotedStr(dbCategoryCode.Text) + " where PLCategorySubCode = " & QuotedStr(lbCode.Text)

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


    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim StrFilter As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("PrintType") = "Print"
            Session("SelectCommand") = "S_FormPrintMaster3 'VMsPLCategorySub A','PLCategorySubCode','PLCategorySubName', 'PLCategoryName', 'PL - Cost Center File','Code','Description', 'Category' ," + QuotedStr(StrFilter) + ", " + QuotedStr(ViewState("UserId"))
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster2.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)
            lstatus.Text = "<script language='javascript'> { window.open(""../../Rpt/PrintForm.Aspx"", ""List"",""scrollbars=yes, resizable=yes,width=600,height=500"");}</script>"
        Catch ex As Exception
            lstatus.Text = "btn print Error = " + ex.ToString
        End Try
    End Sub

    Protected Sub tbPLCategoryCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dr As DataRow
        Dim ds As DataSet
        Dim tbCode, tb As TextBox
        Dim tbName As Label

        Dim Count As Integer
        Dim dgi As GridViewRow
        Dim ddlCurr As DropDownList = New DropDownList
        Try
            tb = sender
            If tb.ID = "PLCategoryCodeAdd" Then
                ' masalah di sini
                Count = DataGrid.Controls(0).Controls.Count
                dgi = DataGrid.Controls(0).Controls(Count - 2) '-1 for allowpaging = False   - 2 allowpaging = True
                tbCode = dgi.FindControl("PLCategoryCodeAdd")
                tbName = dgi.FindControl("PLCategoryNameAdd")
            Else
                Count = DataGrid.EditIndex
                dgi = DataGrid.Rows(Count)
                tbCode = dgi.FindControl("PLCategoryCodeEdit")
                tbName = dgi.FindControl("PLCategoryNameEdit")
            End If
            ds = SQLExecuteQuery("Select PLCategoryCode, PLCategoryName FROM VMsPLCategory Where PLCategoryCode = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbCode.Text = ""
                tbName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbCode.Text = dr("PLCategoryCode").ToString
                tbName.Text = dr("PLCategoryName").ToString
            End If
        Catch ex As Exception
            lstatus.Text = "tb PLCategoryCode Changed Error : " + ex.ToString
        End Try
    End Sub
End Class
