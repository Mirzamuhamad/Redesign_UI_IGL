Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data
Imports System.Data.SqlClient

Partial Class Master_MsFormula_MsFormula
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            InitProperty()
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            'bindDataGrid()
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
        Dim GVR As GridViewRow
        Dim Nilai As TextBox

        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            SqlString = " SELECT * FROM VMsFormula " + StrFilter
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "FormulaCode ASC"
            End If
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)

            GVR = DataGrid.FooterRow
            Nilai = GVR.FindControl("ValuesAdd")
            Nilai.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Protected Sub DataGrid_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles DataGrid.RowDataBound

    End Sub

    Public Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim tbName, Nilai As TextBox

        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()
            tbName = DataGrid.Rows(e.NewEditIndex).FindControl("FormulaNameEdit")
            tbName.Focus()
            Dim dgi As GridViewRow
            dgi = DataGrid.Rows(e.NewEditIndex)

            Nilai = DataGrid.Rows(e.NewEditIndex).FindControl("ValuesEdit")
            Nilai.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub

   
   
    Private Sub DataGrid_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles DataGrid.RowCancelingEdit
        Try
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid()
        Catch ex As Exception
            lstatus.Text = "DataGrid_Cancel Error : " + vbCrLf + ex.ToString
        End Try
    End Sub
   

    Protected Sub DataGrid_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGrid.RowCommand
        Dim SQLString As String
        Dim dbCode, dbName, dbValues, dbDesc As TextBox
        'Dim ddlTypeRange As DropDownList
        Dim GVR As GridViewRow
        Try
            If e.CommandName = "Insert" Then
                GVR = DataGrid.FooterRow
                dbCode = GVR.FindControl("FormulaCodeAdd")
                dbName = GVR.FindControl("FormulaNameAdd")
                dbValues = GVR.FindControl("ValuesAdd")
                dbDesc = GVR.FindControl("FormulaDescAdd")

                
                If dbCode.Text.Trim.Length = 0 Then
                    lstatus.Text = "Formula Code must be filled "
                    dbCode.Focus()
                    Exit Sub
                End If
                If dbName.Text.Trim.Length = 0 Then
                    lstatus.Text = "Formula Name must be filled "
                    dbName.Focus()
                    Exit Sub
                End If
                
                If dbValues.Text.Trim.Length = 0 Then
                    lstatus.Text = "Value must be filled."
                    dbValues.Focus()
                    Exit Sub
                End If

                If Not IsNumeric(dbValues.Text) Then
                    lstatus.Text = "Value must be numeric values."
                    dbValues.Focus()
                    Exit Sub
                End If

                If dbDesc.Text.Trim.Length = 0 Then
                    lstatus.Text = "Formula Desc must be filled "
                    dbDesc.Focus()
                    Exit Sub
                End If

                'If dbXPeriod.Text < 1 Then
                '    lstatus.Text = " Period must be greater than 0."
                '    dbXPeriod.Focus()
                '    Exit Sub
                'End If



                If SQLExecuteScalar("SELECT FormulaCode From VMsFormula WHERE FormulaCode = " + QuotedStr(dbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Formula " + QuotedStr(dbCode.Text) + " has already been exist"
                    Exit Sub
                End If

                'insert the new entry
                SQLString = "Insert into MsFormula (FormulaCode, FormulaName, FormulaValues, FormulaDesc, FgUpdate,FgGP,UserID, UserDate) " + _
                "SELECT " + QuotedStr(dbCode.Text) + "," + QuotedStr(dbName.Text) + "," + _
                QuotedStr(dbValues.Text) + "," + QuotedStr(dbDesc.Text) + ",'Y','N'," + _
                QuotedStr(ViewState("UserId").ToString) + ", GetDate()"
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                bindDataGrid()

            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub
   

    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim dbName, dbValues, dbDesc As TextBox
        Dim lbCode As Label
        Dim GVR As GridViewRow
        Try
            GVR = DataGrid.Rows(e.RowIndex)
            lbCode = GVR.FindControl("FormulaCodeEdit")
            dbName = GVR.FindControl("FormulaNameEdit")
            dbValues = GVR.FindControl("ValuesEdit")
            dbDesc = GVR.FindControl("FormulaDescEdit")
            dbValues.Text = dbValues.Text.Replace(",", "")

            If dbName.Text.Trim.Length = 0 Then
                lstatus.Text = "Formula Name must be filled."
                dbName.Focus()
                Exit Sub
            End If

            If dbValues.Text.Trim.Length = 0 Then
                lstatus.Text = "Values must be filled."
                dbValues.Focus()
                Exit Sub
            End If

            If Not IsNumeric(dbValues.Text) Then
                lstatus.Text = "Values must be numeric values."
                dbValues.Focus()
                Exit Sub
            End If

            If dbDesc.Text.Trim.Length = 0 Then
                lstatus.Text = "Formula Description must be filled."
                dbDesc.Focus()
                Exit Sub
            End If

            SQLString = "Update MsFormula set FormulaName = " + QuotedStr(dbName.Text) + _
            ", FormulaValues =" + dbValues.Text + _
            ", FormulaDesc =" + QuotedStr(dbDesc.Text) + _
            " where FormulaCode = '" & lbCode.Text + "'"
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = lstatus.Text + "DataGrid_Update Error: " & ex.ToString
        End Try
    End Sub
    

    Protected Sub DataGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGrid.RowDeleting
        Dim txtID As Label
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            txtID = DataGrid.Rows(e.RowIndex).FindControl("FormulaCode")

            SQLExecuteNonQuery("Delete from MsFormula where FormulaCode = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
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

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim StrFilter, SQLString As String
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("PrintType") = "Print"
            Session("DBConnection") = ViewState("DBConnection")
            SQLString = "S_FormPrintMaster4 'VMsFormula', 'FormulaCode', 'FormulaName', 'CONVERT(VARCHAR(3),FormulaValues)','FormulaDesc','Formula File', 'Formula Code', 'Formula Name', 'Values','Description', " + QuotedStr(StrFilter) + "," + QuotedStr(ViewState("UserId"))
            Session("SelectCommand") = SQLString
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster3.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)

        Catch ex As Exception
            lstatus.Text = "btn Print Error : " + ex.ToString
        End Try
    End Sub

    

End Class
