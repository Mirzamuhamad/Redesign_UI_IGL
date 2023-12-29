Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data

Partial Class MsPeriodClosing_MsPeriodClosing
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        If Not IsPostBack Then
            InitProperty()
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            FillCombo(ddlYear, "EXEC S_GetYear ", False, "Year", "Year", ViewState("DBConnection").ToString)
            BindToDropList(ddlYear, ViewState("GLYear").ToString)
            FillCombo(ddlYearGenerate, "EXEC S_GetYear ", False, "Year", "Year", ViewState("DBConnection").ToString)
            BindToDropList(ddlYearGenerate, ViewState("GLYear").ToString)
            'UserLevel
            'MenuParam            
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            'DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            'btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            Me.tbRange.Attributes.Add("OnKeyDown", "return PressNumeric();")
            Me.tbQtyPeriod.Attributes.Add("OnKeyDown", "return PressNumeric();")
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
            DataGrid.Visible = True
            PnlGenerate.Visible = False
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
        'Dim tbPeriodCode As TextBox
        'Dim tbStartDate, tbEndDate As BasicFrame.WebControls.BasicDatePicker
        Dim tempDS As New DataSet()
        Dim DV As DataView
        Dim Count As Integer
        Try

            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            'SqlString = "SELECT *  FROM(MsPeriod) WHERE YEAR(PeriodDate) = @Year ORDER BY PeriodDate  " + StrFilter

            If StrFilter = "" Then
                SqlString = "SELECT *  FROM VMsPeriod WHERE YEAR = " + QuotedStr(ddlYear.Text) + " AND FgClosing = 'N' ORDER BY PeriodCode ASC"
            Else
                StrFilter = StrFilter.Replace("Where", "")
                SqlString = "SELECT *  FROM VMsPeriod WHERE Year = " + QuotedStr(ddlYear.Text) + " AND FgClosing = 'N' AND (" + StrFilter + ") ORDER BY PeriodCode ASC"
            End If
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "PeriodCode DESC"
            End If

            'If Connection = "Nothing" Then
            '    tempDS = SQLExecuteQuery(SqlString)
            'Else
            tempDS = SQLExecuteQuery(SqlString, ViewState("DBConnection").ToString)

            DV = tempDS.Tables(0).DefaultView
            Count = tempDS.Tables(0).Rows.Count
            If DV.Count = 0 Then
                Dim DT As DataTable = tempDS.Tables(0)
                ShowGridViewIfEmpty(DT, DataGrid)
            Else
                DV.Sort = ViewState("SortExpression")
                DataGrid.DataSource = DV
                DataGrid.DataBind()
            End If
            'BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)
            'tbPeriodCode = DataGrid.FooterRow.FindControl("PeriodCodeAdd")
            'tbStartDate = DataGrid.FooterRow.FindControl("StartDateAdd")
            'tbEndDate = DataGrid.FooterRow.FindControl("EndDateAdd")
            'If Count = 0 Then
            '    tbStartDate.Enabled = True
            'Else
            '    tbStartDate.Enabled = False
            'End If
            'SqlString = "Declare @Nmbr	VarChar(20) EXEC [S_SAAutoNmbr] " + ddlYear.SelectedValue + ", 1, 'Y', 'PPD', '', @Nmbr OUT  Select @Nmbr "
            'ViewState("PeriodCodeAdd") = SQLExecuteScalar(SqlString, ViewState("DBConnection").ToString)

            'SqlString = "Declare @Date	DateTime, @Start	DateTime EXEC S_GetDateStart " + ddlYear.SelectedValue + ", 1, @Start OUT Select @Date = DATEADD(day,1,MAX(EndDate)) FROM MsPeriod WHERE Year = " + ddlYear.SelectedValue + " Select COALESCE(@Date,@Start) AS Tgl "
            'ViewState("StartDate") = SQLExecuteScalar(SqlString, ViewState("DBConnection").ToString)
            'tbPeriodCode.Text = ViewState("PeriodCodeAdd")
            'tbStartDate.SelectedDate = ViewState("StartDate")
            'tbEndDate.SelectedDate = ViewState("StartDate")
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    'Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
    '    Dim StrFilter As String
    '    Try
    '        StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
    '        If StrFilter.Length = 0 Then
    '            StrFilter = StrFilter + " WHERE YEAR(StartDate) = " + QuotedStr(ddlYear.Text)
    '        Else
    '            StrFilter = StrFilter + " AND YEAR(StartDate) = " + QuotedStr(ddlYear.Text)
    '        End If

    '        Session("DBConnection") = ViewState("DBConnection")
    '        Session("PrintType") = "Print"
    '        Session("SelectCommand") = "EXEC S_FormPrintMaster3 'VMsPeriod','PeriodCode','Start_Date','End_Date','Period File','Period','Start Date','End Date'," + QuotedStr(StrFilter) + ", " + QuotedStr(ViewState("UserId"))
    '        'lstatus.Text = Session("SelectCommand")
    '        'Exit Sub
    '        Session("ReportFile") = ".../../../Rpt/RptPrintMaster2.frx"
    '        AttachScript("openprintdlg();", Page, Me.GetType)
    '    Catch ex As Exception
    '        lstatus.Text = "btn print Error = " + ex.ToString
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
            lstatus.Text = "UserDG_SortCommandError =" + vbCrLf + ex.ToString
        End Try

    End Sub
    Protected Sub DataGrid_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGrid.RowCommand
        Dim SQLString As String
        Dim dbPeriodCode As TextBox
        Dim dbStartDate, dbEndDate As BasicFrame.WebControls.BasicDatePicker
        Try
            If e.CommandName = "Insert" Then
                dbStartDate = DataGrid.FooterRow.FindControl("StartDateAdd")
                dbEndDate = DataGrid.FooterRow.FindControl("EndDateAdd")
                dbPeriodCode = DataGrid.FooterRow.FindControl("PeriodCodeAdd")
                If dbPeriodCode.Text.Trim.Length = 0 Then
                    lstatus.Text = "Period Code must be filled."
                    dbPeriodCode.Focus()
                    Exit Sub
                End If
                If dbStartDate.SelectedValue = Nothing Then
                    lstatus.Text = "Start Date Must Be filled."
                    dbStartDate.Focus()
                    Exit Sub
                End If
                If dbEndDate.SelectedValue = Nothing Then
                    lstatus.Text = "Start Date Must Be filled."
                    dbEndDate.Focus()
                    Exit Sub
                End If
                If dbStartDate.SelectedValue > dbEndDate.SelectedValue Then
                    lstatus.Text = "End Date must be greater than Start Date"
                    dbEndDate.Focus()
                    Exit Sub
                End If
                If SQLExecuteScalar("SELECT PeriodCode From MsPeriod WHERE PeriodCode = " + QuotedStr(dbPeriodCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Period " + QuotedStr(dbPeriodCode.Text) + " has already been exist"
                    Exit Sub
                End If

                'insert the new entry
                SQLString = "Insert into MsPeriod (PeriodCode, Year, StartDate, EndDate,FgClosing,UserId, UserDate ) " + _
                "SELECT " + QuotedStr(dbPeriodCode.Text) + ", " + dbStartDate.SelectedDate.Year.ToString + ", " + QuotedStr(Format(dbStartDate.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(Format(dbEndDate.SelectedValue, "yyyy-MM-dd")) + _
                ", 'N'," + QuotedStr(ViewState("UserId").ToString) + ", getDate()"

                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

                bindDataGrid()
            End If
            If e.CommandName = "Closing" Then
                Dim GVR As GridViewRow = Nothing
                Dim lb As Label
                Dim Index As Integer
                Index = Convert.ToInt32(e.CommandArgument)
                GVR = DataGrid.Rows(Index)
                lb = GVR.FindControl("PeriodCode")
                SQLString = "Update MsPeriod SET FgClosing = 'Y' WHERE PeriodCode = " + QuotedStr(lb.Text)
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                bindDataGrid()
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGrid.RowDeleting
        Dim txtID, txtFg As Label
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            txtID = DataGrid.Rows(e.RowIndex).FindControl("PeriodCode")
            txtFg = DataGrid.Rows(e.RowIndex).FindControl("FgClosing")
            If txtFg.Text = "Y" Then
                lstatus.Text = "Delete Failed... Period has already closed"
                Exit Sub
            End If
            'lstatus.Text = "Delete from MsPeriod where PeriodDate = '" & txtID.Text & "'"
            'Exit Sub
            SQLExecuteNonQuery("Delete from MsPeriod where PeriodCode = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
            bindDataGrid()
        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        Dim txt As TextBox
        Dim lbFg As Label
        Dim tbStart, tbEnd As BasicFrame.WebControls.BasicDatePicker
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()
            obj = DataGrid.Rows(e.NewEditIndex)
            txt = obj.FindControl("PeriodCodeEdit")
            tbStart = obj.FindControl("StartDateEdit")
            tbEnd = obj.FindControl("EndDateEdit")
            lbFg = obj.FindControl("FgClosingEdit")
            If lbFg.Text = "Y" Then
                tbEnd.Enabled = False
            Else
                tbEnd.Enabled = True
            End If
            tbStart.Enabled = False
            ViewState("PeriodCode") = txt.Text
            ViewState("EndDate") = tbEnd.SelectedDate
            'txt.Focus()
        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim dbPeriodCode As TextBox
        Dim dbStartDate, dbEndDate As BasicFrame.WebControls.BasicDatePicker

        Try
            dbStartDate = DataGrid.Rows(e.RowIndex).FindControl("StartDateEdit")
            dbEndDate = DataGrid.Rows(e.RowIndex).FindControl("EndDateEdit")
            dbPeriodCode = DataGrid.Rows(e.RowIndex).FindControl("PeriodCodeEdit")
            If dbStartDate.SelectedValue > dbEndDate.SelectedValue Then
                lstatus.Text = "End Date must be greater than Start Date"
                dbEndDate.Focus()
                Exit Sub
            End If
            ' cek tidak overwrite dengan periode lain
            If dbEndDate.SelectedDate <> ViewState("EndDate") Then
                SQLString = "Select PeriodCode from MsPeriod WHERE " + QuotedStr(Format(dbEndDate.SelectedValue, "yyyy-MM-dd")) + " BETWEEN StartDate AND EndDate AND PeriodCode <> " + QuotedStr(ViewState("PeriodCode"))
                Dim hasil As String
                hasil = SQLExecuteScalar(SQLString, ViewState("DBConnection").ToString)
                If hasil.Length > 0 Then
                    lstatus.Text = "Cannot save data, end date overwrite with period '" + hasil + "'"
                    Exit Sub
                End If
            End If
            'insert the new entry
            SQLString = "UPDATE MsPeriod SET EndDate = " + QuotedStr(Format(dbEndDate.SelectedValue, "yyyy-MM-dd")) + _
            ",UserId = " + QuotedStr(ViewState("UserId").ToString) + _
            ",UserDate = getDate() WHERE PeriodCode = " + QuotedStr(ViewState("PeriodCode"))
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


    'Protected Sub BtnGenerateTop_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGenerateTop.Click
    '    Dim sqlstring As String
    '    Try
    '        PnlGenerate.Visible = True
    '        DataGrid.Visible = False
    '        pnlSearch.Visible = False
    '        sqlstring = "Declare @Date	DateTime, @Start	DateTime EXEC S_GetDateStart " + ddlYearGenerate.SelectedValue + ", 1, @Start OUT Select @Date = DATEADD(day,1,MAX(EndDate)) FROM MsPeriod WHERE Year = " + ddlYearGenerate.SelectedValue + " Select COALESCE(@Date,@Start) AS Tgl "
    '        tbStartGenerate.SelectedDate = SQLExecuteScalar(sqlstring, ViewState("DBConnection").ToString)
    '        tbEndGenerate.SelectedDate = tbStartGenerate.SelectedDate
    '        ddlRangeType.SelectedValue = "Daily"
    '        tbRange.Text = "1"
    '        tbQtyPeriod.Text = "1"
    '    Catch ex As Exception
    '        lstatus.Text = "Generate Error : " + vbCrLf + ex.ToString
    '    End Try
    'End Sub

    Protected Sub ddlyear_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlYear.SelectedIndexChanged
        DataGrid.Visible = False
    End Sub

    'Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
    '    Dim sqlstring, Hasil As String
    '    Try
    '        sqlstring = "Select dbo.FormatDate(MAX(EndDate)) from MsPeriod WHERE EndDate >= " + QuotedStr(Format(tbStartGenerate.SelectedValue, "yyyy-MM-dd"))
    '        Hasil = SQLExecuteScalar(sqlstring, ViewState("DBConnection"))
    '        If Hasil.Length > 1 Then
    '            lstatus.Text = "Generate Failed... Start Date must be greater than " + Hasil
    '            Exit Sub
    '        End If
    '        SQLExecuteScalar(" EXEC S_MsPeriodGenerate " + QuotedStr(Format(tbStartGenerate.SelectedValue, "yyyy-MM-dd")) + ", " + tbQtyPeriod.Text + "," + tbRange.Text + ", " + QuotedStr(ddlRangeType.SelectedValue) + "," + QuotedStr(ViewState("UserId").ToString), ViewState("DBConnection").ToString)
    '        bindDataGrid()
    '        btnCancel_Click(Nothing, Nothing)
    '    Catch ex As Exception
    '        lstatus.Text = "Generate Error : " + vbCrLf + ex.ToString
    '    End Try

    'End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            PnlGenerate.Visible = False
            DataGrid.Visible = True
            pnlSearch.Visible = True
        Catch ex As Exception
            lstatus.Text = "Cancel Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub ddlYearGenerate_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlYearGenerate.SelectedIndexChanged
        Dim sqlstring As String
        Try
            sqlstring = "Declare @Date	DateTime, @Start	DateTime EXEC S_GetDateStart " + ddlYearGenerate.SelectedValue + ", 1, @Start OUT Select @Date = DATEADD(day,1,MAX(EndDate)) FROM MsPeriod WHERE Year = " + ddlYearGenerate.SelectedValue + " Select COALESCE(@Date,@Start) AS Tgl "
            tbStartGenerate.SelectedDate = SQLExecuteScalar(sqlstring, ViewState("DBConnection").ToString)
            tbEndGenerate.SelectedDate = tbStartGenerate.SelectedDate
            ddlRangeType.SelectedValue = "Daily"
            tbRange.Text = "1"
            tbQtyPeriod.Text = "1"
        Catch ex As Exception
            lstatus.Text = "ddlYearGenerate Error : " + vbCrLf + ex.ToString
        End Try

    End Sub

    Protected Sub tbRange_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbRange.TextChanged, tbQtyPeriod.TextChanged
        Try
            If tbRange.Text.Trim = "" Or tbRange.Text = "0" Then
                tbRange.Text = "1"
            End If
            If tbQtyPeriod.Text.Trim = "" Or tbQtyPeriod.Text = "0" Then
                tbQtyPeriod.Text = "1"
            End If
            If ddlRangeType.SelectedValue = "Daily" Then
                tbEndGenerate.SelectedDate = tbStartGenerate.SelectedDate.AddDays((CInt(tbRange.Text) * CInt(tbQtyPeriod.Text)) - 1)
            ElseIf ddlRangeType.SelectedValue = "Weekly" Then
                tbEndGenerate.SelectedDate = tbStartGenerate.SelectedDate.AddDays((CInt(tbRange.Text) * 7 * CInt(tbQtyPeriod.Text)) - 1)
            Else
                tbEndGenerate.SelectedDate = tbStartGenerate.SelectedDate.AddMonths(CInt(tbRange.Text) * CInt(tbQtyPeriod.Text)).AddDays(-1)
            End If
        Catch ex As Exception
            lstatus.Text = "tbRange_TextChanged Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub ddlRangeType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlRangeType.SelectedIndexChanged
        Try
            If tbRange.Text.Trim = "" Or tbRange.Text = "0" Then
                tbRange.Text = "1"
            End If
            If tbQtyPeriod.Text.Trim = "" Or tbQtyPeriod.Text = "0" Then
                tbQtyPeriod.Text = "1"
            End If
            If ddlRangeType.SelectedValue = "Daily" Then
                tbEndGenerate.SelectedDate = tbStartGenerate.SelectedDate.AddDays(CInt(tbRange.Text) - 1)
            ElseIf ddlRangeType.SelectedValue = "Weekly" Then
                tbEndGenerate.SelectedDate = tbStartGenerate.SelectedDate.AddDays((CInt(tbRange.Text) * 7) - 1)
            Else
                tbEndGenerate.SelectedDate = tbStartGenerate.SelectedDate.AddMonths(CInt(tbRange.Text)).AddDays(-1)
            End If
        Catch ex As Exception
            lstatus.Text = "tbRange_TextChanged Error : " + vbCrLf + ex.ToString
        End Try
    End Sub
End Class
