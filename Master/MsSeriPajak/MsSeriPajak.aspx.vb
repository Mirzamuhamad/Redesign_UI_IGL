Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data

Partial Class Master_MsSeriPajak_MsSeriPajak
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            InitProperty()
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
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
        'Dim GVR As GridViewRow
        'Dim StartNo, EndNo, LengthDigit As TextBox

        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            SqlString = "SELECT A.StartDate, A.EndDate, A.Prefix, A.StartNo, A.EndNo, A.LengthDigit, A.FgActive, A.UserId, A.UserDate FROM VMsSeriPajak A " + StrFilter
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "StartDate ASC"
            End If
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)

            'GVR = DataGrid.FooterRow
            'StartNo = GVR.FindControl("StartNoAdd")
            'EndNo = GVR.FindControl("EndNoAdd")
            'LengthDigit = GVR.FindControl("LengthDigitAdd")
            'StartNo.Attributes.Add("OnKeyDown", "return PressNumeric();")
            'EndNo.Attributes.Add("OnKeyDown", "return PressNumeric();")
            'LengthDigit.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub
    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        Dim tbEndDate As BasicFrame.WebControls.BasicDatePicker
        Dim StartNo, EndNo, LengthDigit As TextBox

        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()
            obj = DataGrid.Rows(e.NewEditIndex)
            tbEndDate = obj.FindControl("EndDateEdit")
            tbEndDate.Focus()

            StartNo = DataGrid.Rows(e.NewEditIndex).FindControl("StartNoEdit")
            EndNo = DataGrid.Rows(e.NewEditIndex).FindControl("EndNoEdit")
            LengthDigit = DataGrid.Rows(e.NewEditIndex).FindControl("LengthDigitEdit")
            StartNo.Attributes.Add("OnKeyDown", "return PressNumeric();")
            EndNo.Attributes.Add("OnKeyDown", "return PressNumeric();")
            LengthDigit.Attributes.Add("OnKeyDown", "return PressNumeric();")

        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim dbStartNo, dbEndNo, dbPrefix, dbLengthDigit As TextBox
        Dim FgActive As DropDownList
        Dim tbEndDate As BasicFrame.WebControls.BasicDatePicker
        Dim tbStartDate As Label
        Dim GVR As GridViewRow
        Try            
            GVR = DataGrid.Rows(e.RowIndex)
            dbStartNo = GVR.FindControl("StartNoEdit")
            dbEndNo = GVR.FindControl("EndNoEdit")
            dbPrefix = GVR.FindControl("PrefixEdit")
            dbLengthDigit = GVR.FindControl("LengthDigitEdit")
            FgActive = GVR.FindControl("FgActiveEdit")
            tbStartDate = GVR.FindControl("StartDateEdit")
            tbEndDate = GVR.FindControl("EndDateEdit")

            If dbStartNo.Text.Trim.Length = 0 Then
                lstatus.Text = "Start No must be filled."
                dbStartNo.Focus()
                Exit Sub
            End If
            If dbEndNo.Text.Trim.Length = 0 Then
                lstatus.Text = "End No must be filled."
                dbEndNo.Focus()
                Exit Sub
            End If
            If dbPrefix.Text.Trim.Length = 0 Then
                lstatus.Text = "Prefix must be filled."
                dbPrefix.Focus()
                Exit Sub
            End If

            'If SQLExecuteScalar("SELECT StartDate From VMsSeriPajak WHERE StartDate = " + QuotedStr(tbStartDate.Text), ViewState("DBConnection").ToString).Length > 0 Then
            '    lstatus.Text = "StartDate " + QuotedStr(tbStartDate.Text) + " has already been exist"
            '    Exit Sub
            'End If

            SQLString = "Update MsSeriPajak set EndDate= " + QuotedStr(Format(tbEndDate.SelectedValue, "yyyy-MM-dd")) + _
            ", Prefix=" + QuotedStr(dbPrefix.Text) + ", StartNo=" + dbStartNo.Text + ", EndNo =" + dbEndNo.Text + _
            ", LengthDigit=" + dbLengthDigit.Text + ", FgActive =" + QuotedStr(FgActive.SelectedValue) + _
            " where StartDate = " & QuotedStr(tbStartDate.Text)

            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = lstatus.Text + "DataGrid_Update Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGrid.RowCommand
        Dim SQLString As String
        Dim dbStartNo, dbEndNo, dbPrefix, dbLengthDigit As TextBox
        Dim FgActive As DropDownList
        Dim tbStartDate, tbEndDate As BasicFrame.WebControls.BasicDatePicker
        Dim GVR As GridViewRow
        Try
            If e.CommandName = "Insert" Then
                GVR = DataGrid.FooterRow
                dbStartNo = GVR.FindControl("StartNoAdd")
                dbEndNo = GVR.FindControl("EndNoAdd")
                dbPrefix = GVR.FindControl("PrefixAdd")
                dbLengthDigit = GVR.FindControl("LengthDigitAdd")
                FgActive = GVR.FindControl("FgActiveAdd")
                tbStartDate = GVR.FindControl("StartDateAdd")
                tbEndDate = GVR.FindControl("EndDateAdd")

                If dbStartNo.Text.Trim.Length = 0 Then
                    lstatus.Text = "Start No must be filled."
                    dbStartNo.Focus()
                    Exit Sub
                End If
                If dbEndNo.Text.Trim.Length = 0 Then
                    lstatus.Text = "End No must be filled."
                    dbEndNo.Focus()
                    Exit Sub
                End If
                If dbPrefix.Text.Trim.Length = 0 Then
                    lstatus.Text = "Prefix must be filled."
                    dbPrefix.Focus()
                    Exit Sub
                End If

                If SQLExecuteScalar("SELECT StartDate From VMsSeriPajak WHERE StartDate = " + QuotedStr(tbStartDate.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "StartDate " + QuotedStr(tbStartDate.Text) + " has already been exist"
                    Exit Sub
                End If

                'insert the new entry
                SQLString = "Insert into MsSeriPajak(StartDate, EndDate, Prefix, StartNo, EndNo, LengthDigit, FgActive, UserId, UserDate) " + _
                "SELECT " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + ", " + _
                QuotedStr(Format(tbEndDate.SelectedValue, "yyyy-MM-dd")) + ", " + _
                QuotedStr(dbPrefix.Text) + ", " + dbStartNo.Text + ", " + dbEndNo.Text + ", " + _
                dbLengthDigit.Text + ", " + QuotedStr(FgActive.SelectedValue) + ", " + _
                QuotedStr(ViewState("UserId").ToString) + ", getDate() "

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
            txtID = DataGrid.Rows(e.RowIndex).FindControl("StartDate")

            SQLExecuteNonQuery("Delete from MsSeriPajak where StartDate = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
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
    Protected Sub DataGrid_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles DataGrid.RowCancelingEdit
        Try
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid()
        Catch ex As Exception
            lstatus.Text = "DataGrid_Cancel Error : " + vbCrLf + ex.ToString
        End Try
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
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("PrintType") = "Print"
            Session("DBConnection") = ViewState("DBConnection")
            'Session("SelectCommand") = "EXEC S_FormMsCountry " + StrFilter
            Session("SelectCommand") = "EXEC S_FormPrintMaster5 'VMsSeriPajak A','StartDate','EndDate','Prefix','StartNo','EndNo','Seri Pajak File','Start Date','End Date','Prefix','Start No','End No'," + QuotedStr(StrFilter) + "," + QuotedStr(ViewState("UserId"))
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster4.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            lstatus.Text = "Btn Print Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub DataGrid_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGrid.SelectedIndexChanged

    End Sub
End Class
