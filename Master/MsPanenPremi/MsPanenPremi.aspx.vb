Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data

Partial Class MsPanenPremi_MsPanenPremi
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        If Not IsPostBack Then
            InitProperty()
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            'FillCombo(ddlYear, "EXEC S_GetYear ", False, "Year", "Year", ViewState("DBConnection").ToString)
            'BindToDropList(ddlYear, ViewState("GLYear").ToString)
            ' FillCombo(ddlSelectYear, "EXEC S_GetYear ", False, "Year", "Year", ViewState("DBConnection").ToString)
            'BindToDropList(ddlSelectYear, ViewState("GLYear").ToString)
            'UserLevel
            'MenuParam            
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
            'PnlGenerate.Visible = False
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
        Dim PremiMandor, PremiKrani, MinHKInTeam As TextBox
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            SqlString = "SELECT *  FROM V_MsPanenPremiView " + StrFilter
     


            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "EffectiveDate DESC"
            End If
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)

            GVR = DataGrid.FooterRow
            PremiMandor = GVR.FindControl("PremiMandorAdd")
            PremiKrani = GVR.FindControl("PremiKraniAdd")
            MinHKInTeam = GVR.FindControl("MinHKInTeamAdd")
            PremiMandor.Attributes.Add("OnKeyDown", "return PressNumeric();")
            PremiKrani.Attributes.Add("OnKeyDown", "return PressNumeric();")
            MinHKInTeam.Attributes.Add("OnKeyDown", "return PressNumeric();")

        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            'If StrFilter.Length = 0 Then
            ' StrFilter = StrFilter + " WHERE YEAR(EffectiveDate) = " + QuotedStr(ddlYear.Text)
            ' Else
            ' StrFilter = StrFilter + " AND YEAR(EffectiveDate) = " + QuotedStr(ddlYear.Text)
            'End If

            Session("DBConnection") = ViewState("DBConnection")
            Session("PrintType") = "Print"
            Session("SelectCommand") = "EXEC S_FormPrintMaster4 'V_MsPanenPremiView','Effective_Date','PremiMandor','PremiKrani','MinHKInTeam','Premi Panen File','Effective Date','Premi Mandor (%)','Premi Krani (%)','Min Person In Team'," + QuotedStr(StrFilter) + ", " + QuotedStr(ViewState("UserId"))
            'lstatus.Text = Session("SelectCommand")
            'Exit Sub
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster3.frx"
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
        Dim dbPremiMandor, dbPremiKrani, dbMinHKInTeam As TextBox
        Dim dbEffectiveDate As BasicFrame.WebControls.BasicDatePicker
        Try
            If e.CommandName = "Insert" Then
                dbEffectiveDate = DataGrid.FooterRow.FindControl("EffectiveDateAdd")
                dbPremiMandor = DataGrid.FooterRow.FindControl("PremiMandorAdd")
                dbPremiKrani = DataGrid.FooterRow.FindControl("PremiKraniAdd")
                dbMinHKInTeam = DataGrid.FooterRow.FindControl("MinHKInTeamAdd")


                If dbEffectiveDate.Text.Trim.Length = 0 Then
                    lstatus.Text = "Effective Date Must Be filled."
                    dbEffectiveDate.Focus()
                    Exit Sub
                End If

                If dbPremiMandor.Text.Trim.Length = 0 Then
                    lstatus.Text = "Premi Mandor must be filled."
                    dbPremiMandor.Focus()
                    Exit Sub
                End If

                If dbPremiKrani.Text.Trim.Length = 0 Then
                    lstatus.Text = "Premi Krani must be filled."
                    dbPremiKrani.Focus()
                    Exit Sub
                End If
                If dbMinHKInTeam.Text.Trim.Length = 0 Then
                    lstatus.Text = "Min Person In Team must be filled."
                    dbMinHKInTeam.Focus()
                    Exit Sub
                End If

                If SQLExecuteScalar("SELECT EffectiveDate From MsPanenPremi WHERE EffectiveDate = " + QuotedStr(dbEffectiveDate.SelectedValue), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Effective Date " + QuotedStr(dbEffectiveDate.SelectedDateFormatted) + " has already been exist"
                    Exit Sub
                End If

                'insert the new entry
                SQLString = "Insert into MsPanenPremi (EffectiveDate, PremiMandor, PremiKrani, MinHKInTeam,UserId, UserDate ) " + _
                "SELECT " + QuotedStr(dbEffectiveDate.SelectedValue) + ", " + QuotedStr(dbPremiMandor.Text) + _
                "," + QuotedStr(dbPremiKrani.Text) + "," + QuotedStr(dbMinHKInTeam.Text) + "," + QuotedStr(ViewState("UserId").ToString) + ", getDate()"

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
            txtID = DataGrid.Rows(e.RowIndex).FindControl("EffectiveDate")

            SQLExecuteNonQuery("Delete from MsPanenPremi where dbo.FormatDate(EffectiveDate) = " + QuotedStr(txtID.Text) + " ", ViewState("DBConnection").ToString)
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        Dim txt As BasicFrame.WebControls.BasicDatePicker
        Dim PremiMandor, PremiKrani, MinHKInTeam As TextBox
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()
            obj = DataGrid.Rows(e.NewEditIndex)
            txt = obj.FindControl("EffectiveDateEdit")
            PremiMandor = obj.FindControl("PremiMandorEdit")
            PremiKrani = obj.FindControl("PremiKraniEdit")
            MinHKInTeam = obj.FindControl("MinHKInTeamEdit")
            PremiMandor.Attributes.Add("OnKeyDown", "return PressNumeric();")
            PremiKrani.Attributes.Add("OnKeyDown", "return PressNumeric();")
            MinHKInTeam.Attributes.Add("OnKeyDown", "return PressNumeric();")
            txt.Focus()
        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim dbPremiMandor, dbPremiKrani, dbMinHKInTeam As TextBox
        Dim dbEffectiveDate As BasicFrame.WebControls.BasicDatePicker

        Try
            dbEffectiveDate = DataGrid.Rows(e.RowIndex).FindControl("EffectiveDateEdit")
            dbPremiMandor = DataGrid.Rows(e.RowIndex).FindControl("PremiMandorEdit")
            dbPremiKrani = DataGrid.Rows(e.RowIndex).FindControl("PremiKraniEdit")
            dbMinHKInTeam = DataGrid.Rows(e.RowIndex).FindControl("MinHKInTeamEdit")


            If dbPremiMandor.Text.Trim.Length = 0 Then
                lstatus.Text = "Premi Mandor must be filled."
                dbPremiMandor.Focus()
                Exit Sub
            End If
            If dbPremiKrani.Text.Trim.Length = 0 Then
                lstatus.Text = "Premi Krani must be filled."
                dbPremiKrani.Focus()
                Exit Sub
            End If
            If dbMinHKInTeam.Text.Trim.Length = 0 Then
                lstatus.Text = "Min Person In Team must be filled."
                dbMinHKInTeam.Focus()
                Exit Sub
            End If

            ' If dbEffectiveDate.SelectedDate <> HolidayDateTemp.SelectedDate Then

            'If SQLExecuteScalar("SELECT EffectiveDate From MsLoadingPrice WHERE EffectiveDate = " + QuotedStr(dbEffectiveDate.SelectedValue), ViewState("DBConnection").ToString).Length > 0 Then
            'lstatus.Text = "Holiday Date " + QuotedStr(dbEffectiveDate.SelectedDateFormatted) + " has already been exist"
            'Exit Sub
            'End If

            ' End If

            'insert the new entry
            SQLString = "UPDATE MsPanenPremi SET EffectiveDate = " + QuotedStr(dbEffectiveDate.SelectedDate) + _
            ", PremiMandor = " + QuotedStr(dbPremiMandor.Text) + _
            ", PremiKrani = " + QuotedStr(dbPremiKrani.Text) + _
            ", MinHKInTeam = " + QuotedStr(dbMinHKInTeam.Text) + _
            ",UserId = " + QuotedStr(ViewState("UserId").ToString) + _
            ",UserDate = getDate() WHERE EffectiveDate = " + QuotedStr(dbEffectiveDate.SelectedDate)

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


    '  Protected Sub BtnGenerateTop_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGenerateTop.Click
    ' Try
    'PnlGenerate.Visible = True
    'DataGrid.Visible = False
    ' pnlSearch.Visible = False


    ' Catch ex As Exception
    '    lstatus.Text = "Generate Error : " + vbCrLf + ex.ToString
    ' End Try
    ' End Sub



    ' Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
    'Try
    '   SQLExecuteScalar(" EXEC S_MsHolidayGenerate " + QuotedStr(ddlOffDay.SelectedValue) + "," + QuotedStr(ddlSelectYear.SelectedValue) + " , " + QuotedStr(tbDescription.Text) + "," + ViewState("UserId").ToString, ViewState("DBConnection").ToString)
    'lstatus.Text = " EXEC S_MsHolidayGenerate " + QuotedStr(ddlOffDay.SelectedValue) + "," + QuotedStr(ddlSelectYear.SelectedValue) + " , " + QuotedStr(tbDescription.Text) + "," + Session("Userid").ToString
    '      bindDataGrid()

    ' btnCancel_Click(Nothing, Nothing)
    ' Catch ex As Exception
    '    lstatus.Text = "Generate Error : " + vbCrLf + ex.ToString
    '  End Try

    '  End Sub

    ' Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
    '   Try
    '      PnlGenerate.Visible = False
    '       DataGrid.Visible = True
    '     pnlSearch.Visible = True
    ' Catch ex As Exception
    '     lstatus.Text = "Cancel Error : " + vbCrLf + ex.ToString
    '    End Try
    ' End Sub
End Class
