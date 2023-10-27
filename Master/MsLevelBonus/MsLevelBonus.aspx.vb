Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data
Partial Class MsLevelBonus_MsLevelBonus
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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
        'dsLevelCode.ConnectionString = ViewState("DBConnection")
        lstatus.Text = ""
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
            SqlString = "SELECT LevelCode, LevelName, Percentage FROM MsLevelBonus " + StrFilter
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "LevelCode ASC"
                ViewState("SortOrder") = "ASC"
            End If
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    'Private Sub bindDataGrid()
    '    Dim StrFilter, SqlString As String
    '    Try
    '        StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
    '        SqlString = "SELECT A.LevelCode, A.LevelName,A.Percentage, B.JenisName FROM MsLevelBonus A INNER JOIN MsLevelCode B ON A.LevelCode = B.JenisCode" + StrFilter + " Order By A.Percentage "
    '        If ViewState("SortExpression") = Nothing Then
    '            ViewState("SortExpression") = "LevelCode ASC"
    '            ViewState("SortOrder") = "ASC"
    '        End If
    '        BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection"))
    '    Catch ex As Exception
    '        lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
    '    Finally
    '    End Try
    'End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("DBConnection") = ViewState("DBConnection")
            Session("PrintType") = "Print"
            'Session("SelectCommand") = "EXEC S_FormMsProductSubGroup"
            Session("SelectCommand") = "EXEC S_FormPrintMaster3 'VMsLevelBonus','LevelCode','LevelName','Percentage'," + QuotedStr(lblTitle.Text) + ",'Product Jenis Code','Product Size','Product Size Name'," + QuotedStr(StrFilter) + "," + QuotedStr(ViewState("UserId").ToString)
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster2.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)

            'Dim ReportGw As New ReportDocument
            'Dim Ds As DataSet
            'Dim crParameterFieldDefinitnions As ParameterFieldDefinitions
            'Dim crparameter1, crprmColumn As ParameterFieldDefinition
            'Dim crparameter1values, crprmColumnvalues As ParameterValues
            'Dim crDiscrete1Value As New ParameterDiscreteValue
            'Try
            'Ds = SQLExecuteQuery("Select A.ProductSubGrpCode AS Code, A.ProductSubGrpName AS Name, B.ProductGrpName AS Col3 from MsProductGroupSub A INNER JOIN MsProductGroup B ON A.ProductGroup = B.ProductCatCode ")
            'ReportGw.Load(Server.MapPath("~\Rpt\PrintMaster3.Rpt"))

            'ReportGw.SetDataSource(Ds.Tables(0))
            'crParameterFieldDefinitnions = ReportGw.DataDefinition.ParameterFields
            'crparameter1 = crParameterFieldDefinitnions.Item("Title")
            'crparameter1values = crparameter1.CurrentValues
            'crDiscrete1Value.Value = "Product Sub Group File"
            'crparameter1values.Add(crDiscrete1Value)
            'crparameter1.ApplyCurrentValues(crparameter1values)

            'crprmColumn = crParameterFieldDefinitnions.Item("Col3Title")
            'crprmColumnvalues = crparameter1.CurrentValues
            'crDiscrete1Value.Value = "Product Group"
            'crprmColumnvalues.Add(crDiscrete1Value)
            'crprmColumn.ApplyCurrentValues(crprmColumnvalues)

            'Session("Report") = ReportGw
            'Response.Write("<script language='javascript'> { window.open(""../../Rpt/PrintMaster.Aspx"", ""List"",""scrollbars=yes, resizable=yes,width=600,height=500"");}</script>")
            lstatus.Text = "<script language='javascript'> { window.open(""../../Rpt/PrintMaster.Aspx"", ""List"",""scrollbars=yes, resizable=yes,width=600,height=500"");}</script>"
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
        Dim dbLevelCode, dbPercentage, dbName As TextBox
        Dim Validate As String = "^[0-9-. ]*$"
        Dim r As New Regex(Validate)

        Try

            If e.CommandName = "Insert" Then
                dbPercentage = DataGrid.FooterRow.FindControl("PercentageAdd")
                dbName = DataGrid.FooterRow.FindControl("LevelNameAdd")
                dbLevelCode = DataGrid.FooterRow.FindControl("LevelCodeAdd")

                If dbLevelCode.Text.Trim.Length = 0 Then
                    lstatus.Text = "Level Code Code must be filled."
                    dbLevelCode.Focus()
                    Exit Sub
                End If

                If dbPercentage.Text.Trim.Length = 0 Then
                    lstatus.Text = "Percentage must be filled."
                    dbPercentage.Focus()
                    Exit Sub
                End If

                If dbName.Text.Trim.Length = 0 Then
                    lstatus.Text = "Level Name must be filled."
                    dbName.Focus()
                    Exit Sub
                End If

                If r.IsMatch(dbPercentage.Text) = False Then
                    lstatus.Text = "Please enter valid Characters in to Percentage"
                    dbPercentage.Focus()
                    Exit Sub
                End If
                If r.IsMatch(dbLevelCode.Text) = False Then
                    lstatus.Text = "Please enter valid Characters in to Level Code"
                    dbLevelCode.Focus()
                    Exit Sub
                End If

                If SQLExecuteScalar("SELECT LevelCode From MsLevelBonus WHERE LevelCode = " + QuotedStr(dbLevelCode.Text), ViewState("DBConnection")).Length > 0 Then
                    lstatus.Text = "Level Code Bonus " + QuotedStr(dbLevelCode.Text) + " has already been exist"
                    Exit Sub
                End If

                'insert the new entry
                SQLString = "Insert into MsLevelBonus (LevelCode, LevelName, Percentage, UserId, UserDate ) " + _
                "SELECT " + QuotedStr(dbLevelCode.Text) + ", " + QuotedStr(dbName.Text) + ", " + QuotedStr(dbPercentage.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", getDate()"
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection"))
                bindDataGrid()

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
            txtID = DataGrid.Rows(e.RowIndex).FindControl("LevelCode")

            SQLExecuteNonQuery("Delete from MsLevelBonus where LevelCode = '" & txtID.Text & "'", ViewState("DBConnection"))
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
            txt = obj.FindControl("LevelNameEdit")
            txt.Focus()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim dbName, dbPercentage As TextBox
        Dim lbCode As Label

        Try
            lbCode = DataGrid.Rows(e.RowIndex).FindControl("LevelCodeEdit")
            dbName = DataGrid.Rows(e.RowIndex).FindControl("LevelNameEdit")
            dbPercentage = DataGrid.Rows(e.RowIndex).FindControl("PercentageEdit")

            If dbPercentage.Text.Trim.Length = 0 Then
                lstatus.Text = "Product Group Name must be filled."
                dbName.Focus()
                Exit Sub
            End If

            SQLString = "Update MsLevelBonus set LevelName= " + QuotedStr(dbName.Text) + ", Percentage= " + QuotedStr(dbPercentage.Text) + _
            " where LevelCode = '" & lbCode.Text + "'"

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
End Class
