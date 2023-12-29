Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data

Partial Class MsPuasa_MsPuasa
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        If Not IsPostBack Then
            InitProperty()
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
        End If
        dsYear.ConnectionString = ViewState("DBConnection")
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
            DataGrid.Visible = True
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
            SqlString = "SELECT *  FROM  VMsPuasa " + StrFilter + " ORDER BY Year"

            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "Year ASC"
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
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            
            Session("DBConnection") = ViewState("DBConnection")
            Session("PrintType") = "Print"
            Session("SelectCommand") = "EXEC S_FormPrintMaster5 'VMsPuasa','Year','Start1','End1','Start2','End2','Setting Puasa','Year','Start Date #1','End Date #1','Start Date #2','End Date #2'," + QuotedStr(StrFilter) + ", " + QuotedStr(ViewState("UserId"))
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster4.frx"
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
        Dim dbYear As DropDownList
        Dim dbStart1Date, dbEnd1Date, dbStart2Date, dbEnd2Date As BasicFrame.WebControls.BasicDatePicker
        Try
            If e.CommandName = "Insert" Then
                dbYear = DataGrid.FooterRow.FindControl("YearAdd")
                dbStart1Date = DataGrid.FooterRow.FindControl("Start1DateAdd")
                dbEnd1Date = DataGrid.FooterRow.FindControl("End1DateAdd")
                dbStart2Date = DataGrid.FooterRow.FindControl("Start2DateAdd")
                dbEnd2Date = DataGrid.FooterRow.FindControl("End2DateAdd")

                If dbYear.SelectedValue.Trim.Length = 0 Then
                    lstatus.Text = "Year Must Be filled."
                    dbYear.Focus()
                    Exit Sub
                End If

                If dbStart1Date.Text.Trim.Length = 0 Then
                    lstatus.Text = "Start Date #1 Must Be filled."
                    dbStart1Date.Focus()
                    Exit Sub
                End If

                If dbEnd1Date.Text.Trim.Length = 0 Then
                    lstatus.Text = "End Date #1 Must Be filled."
                    dbEnd1Date.Focus()
                    Exit Sub
                End If

                If dbStart1Date.SelectedDate > dbEnd1Date.SelectedDate Then
                    lstatus.Text = "Start Date #1 can not greater than End Date #1."
                    dbStart1Date.Focus()
                    Exit Sub
                End If

                If (Not dbStart2Date.IsNull) Or (Not dbEnd2Date.IsNull) Then
                    If dbStart2Date.IsNull Then
                        lstatus.Text = "Start Date #2 must be filled."
                        dbStart2Date.Focus()
                        Exit Sub
                    End If

                    If dbEnd2Date.IsNull Then
                        lstatus.Text = "End Date #2 must be filled."
                        dbEnd2Date.Focus()
                        Exit Sub
                    End If

                    If dbStart2Date.SelectedDate > dbEnd2Date.SelectedDate Then
                        lstatus.Text = "Start Date #2 can not greater than End Date #2."
                        dbEnd2Date.Focus()
                        Exit Sub
                    End If

                    If ((dbStart1Date.SelectedDate >= dbStart2Date.SelectedDate) And (dbStart1Date.SelectedDate <= dbEnd2Date.SelectedDate)) Then
                        lstatus.Text = "Start Date #1 can not between on Start Date #2 and End Date #2."
                        dbStart1Date.Focus()
                        Exit Sub
                    End If

                    If ((dbEnd1Date.SelectedDate >= dbStart2Date.SelectedDate) And (dbEnd1Date.SelectedDate <= dbEnd2Date.SelectedDate)) Then
                        lstatus.Text = "End Date #1 can not between on Start Date #2 and End Date #2."
                        dbStart1Date.Focus()
                        Exit Sub
                    End If
                End If

                If SQLExecuteScalar("SELECT Year FROM MsPuasa WHERE Year = " + dbYear.SelectedValue, ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Year " + dbYear.SelectedValue + " has already been exist"
                    Exit Sub
                End If

                'insert the new entry
                SQLString = "INSERT INTO MsPuasa (Year, Start1Date,End1Date, Start2Date, End2Date, UserId, UserDate ) " + _
                "SELECT " + dbYear.SelectedValue + ", '" + Format(dbStart1Date.SelectedValue, "yyyy-MM-dd") + "', '" + Format(dbEnd1Date.SelectedValue, "yyyy-MM-dd") + "', '" + Format(dbStart2Date.SelectedValue, "yyyy-MM-dd") + "', '" + _
                 Format(dbEnd2Date.SelectedValue, "yyyy-MM-dd") + "', " + QuotedStr(ViewState("UserId").ToString) + ", getDate()"

                SQLString = ChangeQuoteNull(SQLString)
                SQLString = SQLString.Replace("'1900-01-01'", "NULL")
                SQLString = SQLString.Replace("'0001-01-01'", "NULL")
                SQLString = SQLString.Replace("''", "NULL")

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
            txtID = DataGrid.Rows(e.RowIndex).FindControl("Year")

            SQLExecuteNonQuery("DELETE FROM MsPuasa WHERE Year = " & txtID.Text, ViewState("DBConnection").ToString)
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        Dim txt As BasicFrame.WebControls.BasicDatePicker
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()
            obj = DataGrid.Rows(e.NewEditIndex)
            txt = obj.FindControl("Start1DateEdit")
            txt.Focus()
        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim dbYear As DropDownList
        Dim dbStart1Date, dbEnd1Date, dbStart2Date, dbEnd2Date As BasicFrame.WebControls.BasicDatePicker

        Try
            dbYear = DataGrid.Rows(e.RowIndex).FindControl("YearEdit")
            dbStart1Date = DataGrid.Rows(e.RowIndex).FindControl("Start1DateEdit")
            dbEnd1Date = DataGrid.Rows(e.RowIndex).FindControl("End1DateEdit")
            dbStart2Date = DataGrid.Rows(e.RowIndex).FindControl("Start2DateEdit")
            dbEnd2Date = DataGrid.Rows(e.RowIndex).FindControl("End2DateEdit")

            If dbStart1Date.Text.Trim.Length = 0 Then
                lstatus.Text = "Start Date #1 Must Be filled."
                dbStart1Date.Focus()
                Exit Sub
            End If

            If dbEnd1Date.Text.Trim.Length = 0 Then
                lstatus.Text = "End Date #1 Must Be filled."
                dbEnd1Date.Focus()
                Exit Sub
            End If

            'insert the new entry
            SQLString = "UPDATE MsPuasa SET Start1Date = " + QuotedStr(Format(dbStart1Date.SelectedValue, "yyyy-MM-dd")) + _
            ", End1Date = " + QuotedStr(Format(dbEnd1Date.SelectedValue, "yyyy-MM-dd")) + _
            ", Start2Date = " + QuotedStr(Format(dbStart2Date.SelectedValue, "yyyy-MM-dd")) + _
            ", End2Date = " + QuotedStr(Format(dbEnd2Date.SelectedValue, "yyyy-MM-dd")) + _
            ",UserId = " + QuotedStr(ViewState("UserId").ToString) + _
            ",UserDate = GetDate() WHERE Year = " + dbYear.SelectedValue

            SQLString = ChangeQuoteNull(SQLString)
            SQLString = SQLString.Replace("'1900-01-01'", "NULL")
            SQLString = SQLString.Replace("'0001-01-01'", "NULL")
            SQLString = SQLString.Replace("''", "NULL")

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
