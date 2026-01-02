Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data

Partial Class MsLoadingPrice_MsLoadingPrice
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
        Dim PriceBongkar, PriceMuat As TextBox
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            'SqlString = "SELECT *  FROM(MsHoliday) WHERE YEAR(HolidayDate) = @Year ORDER BY HolidayDate  " + StrFilter

            'If StrFilter = "" Then
            SqlString = "SELECT *  FROM V_MsLoadingPriceView " + StrFilter
            ' Else
            '    StrFilter = StrFilter.Replace("Where", "")
            '   SqlString = "SELECT *  FROM MsLoadingPrice WHERE YEAR(EffectiveDate) = " + QuotedStr(ddlYear.Text) + " AND (" + StrFilter + ") ORDER BY EffectiveDate ASC"
            'End If


            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "EffectiveDate DESC"
            End If
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)

            GVR = DataGrid.FooterRow
            PriceBongkar = GVR.FindControl("PriceBongkarAdd")
            PriceMuat = GVR.FindControl("PriceMuatAdd")
            PriceBongkar.Attributes.Add("OnKeyDown", "return PressNumeric();")
            PriceMuat.Attributes.Add("OnKeyDown", "return PressNumeric();")

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
            Session("SelectCommand") = "EXEC S_FormPrintMaster3 'V_MsLoadingPriceView','Effective_Date','PriceBongkar','PriceMuat','Loading Price File','Effective Date','Price Bongkar','Price Muat'," + QuotedStr(StrFilter) + ", " + QuotedStr(ViewState("UserId"))
            'lstatus.Text = Session("SelectCommand")
            'Exit Sub
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
                '   ViewState("SortOrder") = "ASC"
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
        Dim dbPriceMuat As TextBox
        Dim dbPriceBongkar As TextBox
        Dim dbEffectiveDate As BasicFrame.WebControls.BasicDatePicker
        Try
            If e.CommandName = "Insert" Then
                dbEffectiveDate = DataGrid.FooterRow.FindControl("EffectiveDateAdd")
                dbPriceBongkar = DataGrid.FooterRow.FindControl("PriceBongkarAdd")
                dbPriceMuat = DataGrid.FooterRow.FindControl("PriceMuatAdd")


                If dbEffectiveDate.Text.Trim.Length = 0 Then
                    lstatus.Text = "Effective Date Must Be filled."
                    dbEffectiveDate.Focus()
                    Exit Sub
                End If

                If dbPriceBongkar.Text.Trim.Length = 0 Then
                    lstatus.Text = "Price Bongkar must be filled."
                    dbPriceBongkar.Focus()
                    Exit Sub
                End If

                If dbPriceMuat.Text.Trim.Length = 0 Then
                    lstatus.Text = "Price Muat must be filled."
                    dbPriceMuat.Focus()
                    Exit Sub
                End If

                If SQLExecuteScalar("SELECT EffectiveDate From MsLoadingPrice WHERE EffectiveDate = " + QuotedStr(dbEffectiveDate.SelectedValue), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Effective Date " + QuotedStr(dbEffectiveDate.SelectedDateFormatted) + " has already been exist"
                    Exit Sub
                End If

                'insert the new entry
                SQLString = "Insert into MsLoadingPrice (EffectiveDate, PriceBongkar, PriceMuat,UserId, UserDate ) " + _
                "SELECT " + QuotedStr(dbEffectiveDate.SelectedValue) + ", " + QuotedStr(dbPriceBongkar.Text) + _
                "," + QuotedStr(dbPriceMuat.Text) + "," + QuotedStr(ViewState("UserId").ToString) + ", getDate()"

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

            SQLExecuteNonQuery("Delete from MsLoadingPrice where dbo.FormatDate(EffectiveDate) = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        Dim txt As BasicFrame.WebControls.BasicDatePicker
        Dim PriceBongkar, PriceMuat As TextBox
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()
            obj = DataGrid.Rows(e.NewEditIndex)
            txt = obj.FindControl("EffectiveDateEdit")
            PriceBongkar = obj.FindControl("PriceBongkarEdit")
            PriceMuat = obj.FindControl("PriceMuatEdit")
            PriceBongkar.Attributes.Add("OnKeyDown", "return PressNumeric();")
            PriceMuat.Attributes.Add("OnKeyDown", "return PressNumeric();")
            txt.Focus()
        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim dbPriceMuat As TextBox
        Dim dbPriceBongkar As TextBox
        Dim dbEffectiveDate As BasicFrame.WebControls.BasicDatePicker

        Try
            dbEffectiveDate = DataGrid.Rows(e.RowIndex).FindControl("EffectiveDateEdit")
            dbPriceBongkar = DataGrid.Rows(e.RowIndex).FindControl("PriceBongkarEdit")
            dbPriceMuat = DataGrid.Rows(e.RowIndex).FindControl("PriceMuatEdit")




            If dbPriceBongkar.Text.Trim.Length = 0 Then
                lstatus.Text = "Price Bongkar must be filled."
                dbPriceBongkar.Focus()
                Exit Sub
            End If

            ' If dbEffectiveDate.SelectedDate <> HolidayDateTemp.SelectedDate Then

            'If SQLExecuteScalar("SELECT EffectiveDate From MsLoadingPrice WHERE EffectiveDate = " + QuotedStr(dbEffectiveDate.SelectedValue), ViewState("DBConnection").ToString).Length > 0 Then
            'lstatus.Text = "Holiday Date " + QuotedStr(dbEffectiveDate.SelectedDateFormatted) + " has already been exist"
            'Exit Sub
            'End If

            ' End If

            'insert the new entry
            SQLString = "UPDATE MsLoadingPrice SET EffectiveDate = " + QuotedStr(dbEffectiveDate.SelectedDate) + _
            ", PriceBongkar = " + QuotedStr(dbPriceBongkar.Text) + _
            ", PriceMuat = " + QuotedStr(dbPriceMuat.Text) + _
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
