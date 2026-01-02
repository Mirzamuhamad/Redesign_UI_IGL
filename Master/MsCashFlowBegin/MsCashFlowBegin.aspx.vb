Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data

Partial Class Execute_Master_MsCashFlowBegin_MsCashFlowBegin
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
            'bindDataGrid()
        End If

        dsYear.ConnectionString = ViewState("DBConnection")
        dsPeriod.ConnectionString = ViewState("DBConnection")
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
        Dim GVR As GridViewRow
        Dim total As TextBox

        Try
            'SqlString = "Select * from VMsWIP " + StrFilter
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            SqlString = "Select * from V_MsCashFlowBegin " + StrFilter
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "Year Asc"
            End If
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)

            GVR = DataGrid.FooterRow
            total = GVR.FindControl("TotalAdd")
            total.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click

        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("PrintType") = "Print"
            Session("DBConnection") = ViewState("DBConnection")
            Session("SelectCommand") = "S_FormPrintMaster3 'MsCashFlowBegin','Year','Month','Total','Cash Flow Begin','Year','Month','Total'," + QuotedStr(StrFilter) + "," + QuotedStr(ViewState("UserId"))
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster2.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)
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

    Protected Sub DataGrid_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGrid.RowCommand
        Dim SQLString As String
        Dim year, period, yearTemp, periodTemp As DropDownList
        Dim total As TextBox

        Try
            If e.CommandName = "Insert" Then
                year = DataGrid.FooterRow.FindControl("ddlYearAdd")
                period = DataGrid.FooterRow.FindControl("ddlPeriodAdd")
                total = DataGrid.FooterRow.FindControl("TotalAdd")

                If total.Text.Trim.Length = 0 Then
                    lstatus.Text = "Total must be filled."
                    total.Focus()
                    Exit Sub
                End If

                If SQLExecuteScalar("SELECT Year, Month From MsCashFlowBegin WHERE Year = " + QuotedStr(year.SelectedValue) + " And Month = " + QuotedStr(period.SelectedValue), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Year " + QuotedStr(year.SelectedValue) + " And Period " + QuotedStr(period.SelectedItem.ToString) + " has already been exist"
                    Exit Sub
                End If

                'insert the new entry
                SQLString = "Insert into MsCashFlowBegin (Year, Month, Total, UserId, UserDate) " + _
                "SELECT " + QuotedStr(year.SelectedValue) + ", " + _
                QuotedStr(period.SelectedValue) + ", " + _
                QuotedStr(total.Text) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", getDate()"

                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                bindDataGrid()
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "Insert Command Error:" & ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGrid.RowDeleting
        Dim txtID, txtID2 As Label

        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            txtID = DataGrid.Rows(e.RowIndex).FindControl("Year")
            txtID2 = DataGrid.Rows(e.RowIndex).FindControl("PeriodTemp")

            SQLExecuteNonQuery("Delete from MsCashFlowBegin where Year = '" & txtID.Text & "' And Month = '" & txtID2.Text & "'", ViewState("DBConnection").ToString)
            bindDataGrid()
        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        Dim txt As DropDownList
        Dim total As TextBox

        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If

            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()
            obj = DataGrid.Rows(e.NewEditIndex)
            txt = obj.FindControl("ddlYearEdit")
            txt.Focus()

            total = DataGrid.Rows(e.NewEditIndex).FindControl("TotalEdit")
            total.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim year, month, yearTemp, monthTemp As DropDownList
        Dim total As TextBox

        Try
            year = DataGrid.Rows(e.RowIndex).FindControl("ddlYearEdit")
            month = DataGrid.Rows(e.RowIndex).FindControl("ddlPeriodEdit")
            yearTemp = DataGrid.Rows(e.RowIndex).FindControl("ddlYearEditTemp")
            monthTemp = DataGrid.Rows(e.RowIndex).FindControl("ddlPeriodEditTemp")
            total = DataGrid.Rows(e.RowIndex).FindControl("TotalEdit")

            If total.Text.Trim.Length = 0 Then
                lstatus.Text = "Total must be filled."
                total.Focus()
                Exit Sub
            End If

            SQLString = "Update MsCashFlowBegin set Year= " + QuotedStr(year.SelectedValue) + "," & _
            " Month = " + QuotedStr(month.SelectedValue) + _
            ", Total = " + QuotedStr(total.Text) + _
            " where Year = " & QuotedStr(yearTemp.SelectedValue) & " And Month = " & QuotedStr(monthTemp.SelectedValue)

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
