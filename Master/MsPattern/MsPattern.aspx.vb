Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data

Partial Class Master_MsPattern_MsPattern
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
        dsShift.ConnectionString = ViewState("DBConnection")
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
            bindDataGrid()
            'tbFilter.Text = ""
            'tbfilter2.Text = ""
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
            SqlString = "Select * from VMsPattern " + StrFilter
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "PatternCode ASC"
            End If

            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)

            Dim obj As TextBox
            obj = DataGrid.FooterRow.FindControl("PatternShiftAdd")
            obj.Attributes.Add("OnKeyDown", "return PressShift();")
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("DBConnection") = ViewState("DBConnection")
            Session("PrintType") = "Print"
            Session("SelectCommand") = "S_FormPrintMaster3 'MsPattern', 'Patterncode', 'PatternName', 'PatternShift', 'Pattern Shift File', 'Pattern Code', 'Pattern Name', 'Pattern Formula', " + QuotedStr(StrFilter) + ", " + QuotedStr(ViewState("UserId"))
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster2.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)
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
        Dim dbCode, dbName, dbFormula As TextBox
        Dim cbshiftA, cbshiftB, cbshiftC, cbshiftD, cbshiftE, cbshiftF, cbshiftX As DropDownList
        Try
            If e.CommandName = "Insert" Then
                dbCode = DataGrid.FooterRow.FindControl("PatternCodeAdd")
                dbName = DataGrid.FooterRow.FindControl("PatternNameAdd")
                dbFormula = DataGrid.FooterRow.FindControl("PatternShiftAdd")
                cbshiftA = DataGrid.FooterRow.FindControl("ShiftAAdd")
                cbshiftB = DataGrid.FooterRow.FindControl("ShiftBAdd")
                cbshiftC = DataGrid.FooterRow.FindControl("ShiftCAdd")
                cbshiftD = DataGrid.FooterRow.FindControl("ShiftDAdd")
                cbshiftE = DataGrid.FooterRow.FindControl("ShiftEAdd")
                cbshiftF = DataGrid.FooterRow.FindControl("ShiftFAdd")
                cbshiftX = DataGrid.FooterRow.FindControl("ShiftXAdd")
                If dbCode.Text.Trim.Length = 0 Then
                    lstatus.Text = "Pattern Code must be filled."
                    dbCode.Focus()
                    Exit Sub
                End If
                If dbName.Text.Trim.Length = 0 Then
                    lstatus.Text = "Pattern Name must be filled."
                    dbName.Focus()
                    Exit Sub
                End If
                If dbFormula.Text.Trim.Length = 0 Then
                    lstatus.Text = "Pattern Formula must be filled."
                    dbFormula.Focus()
                    Exit Sub
                End If
                If SQLExecuteScalar("Select PatternCode From VMsPattern WHERE PatternCode = " + QuotedStr(dbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Pattern " + QuotedStr(dbCode.Text) + " has already been exist"
                    Exit Sub
                End If
                dbFormula.Text = dbFormula.Text.ToUpper
                'insert the new entry
                SQLString = "Insert into MsPattern (PatternCode, PatternName, PatternShift, ShiftA, ShiftB, ShiftC, ShiftD, ShiftE, ShiftF, ShiftX, UserId, UserDate) " + _
                "SELECT '" + dbCode.Text.Replace("'", "''") + _
                "', '" + dbName.Text.Replace("'", "''") + "', '" + dbFormula.Text.Replace(",", "''") + "', " + QuotedStr(cbshiftA.SelectedValue) + "," + QuotedStr(cbshiftB.SelectedValue) + "," + QuotedStr(cbshiftC.SelectedValue) + "," + QuotedStr(cbshiftD.SelectedValue) + "," + QuotedStr(cbshiftE.SelectedValue) + "," + QuotedStr(cbshiftF.SelectedValue) + "," + QuotedStr(cbshiftX.SelectedValue) + ",'" + ViewState("UserId").ToString + "', getDate()"
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

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
            txtID = DataGrid.Rows(e.RowIndex).FindControl("PatternCode")

            SQLExecuteNonQuery("Delete from MsPattern where PatternCode = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        Dim txt, txtShift As TextBox
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()
            obj = DataGrid.Rows(e.NewEditIndex)
            txt = obj.FindControl("PatternNameEdit")
            txtShift = obj.FindControl("PatternShiftEdit")
            txtShift.Attributes.Add("OnKeyDown", "return PressShift();")
            txt.Focus()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim dbName, dbShift As TextBox
        Dim cbshiftA, cbshiftB, cbshiftC, cbshiftD, cbshiftE, cbshiftF, cbshiftX As DropDownList
        Dim lbCode As Label

        Try
            lbCode = DataGrid.Rows(e.RowIndex).FindControl("PatternCodeEdit")
            dbName = DataGrid.Rows(e.RowIndex).FindControl("PatternNameEdit")
            dbShift = DataGrid.Rows(e.RowIndex).FindControl("PatternShiftEdit")
            cbshiftA = DataGrid.Rows(e.RowIndex).FindControl("ShiftAEdit")
            cbshiftB = DataGrid.Rows(e.RowIndex).FindControl("ShiftBEdit")
            cbshiftC = DataGrid.Rows(e.RowIndex).FindControl("ShiftCEdit")
            cbshiftD = DataGrid.Rows(e.RowIndex).FindControl("ShiftDEdit")
            cbshiftE = DataGrid.Rows(e.RowIndex).FindControl("ShiftEEdit")
            cbshiftF = DataGrid.Rows(e.RowIndex).FindControl("ShiftFEdit")
            cbshiftX = DataGrid.Rows(e.RowIndex).FindControl("ShiftXEdit")
            If dbName.Text.Trim.Length = 0 Then
                lstatus.Text = "Pattern Name must be filled."
                dbName.Focus()
                Exit Sub
            End If
            If dbShift.Text.Trim.Length = 0 Then
                lstatus.Text = "Pattern Shift must be filled."
                dbShift.Focus()
                Exit Sub
            End If
            dbShift.Text = dbShift.Text.ToUpper
            SQLString = "Update MsPattern set PatternName= '" + dbName.Text.Replace("'", "''") + "', PatternShift= " + QuotedStr(dbShift.Text.Replace("'", "''")) + _
            ", ShiftA = " + QuotedStr(cbshiftA.SelectedValue) + ", ShiftB = " + QuotedStr(cbshiftB.SelectedValue) + ", ShiftC = " + QuotedStr(cbshiftC.SelectedValue) + ", ShiftD = " + QuotedStr(cbshiftD.SelectedValue) + _
            ", ShiftE = " + QuotedStr(cbshiftE.SelectedValue) + ", ShiftF = " + QuotedStr(cbshiftF.SelectedValue) + ", ShiftX = " + QuotedStr(cbshiftX.SelectedValue) + _
            " where PatternCode = '" & lbCode.Text + "'"
            'lstatus.Text = SQLString
            'Exit Sub
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
