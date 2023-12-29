Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data
Partial Class Master_MsPPHRate_MsPPHRate
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
        Dim GVR As GridViewRow
        Dim Code, StartVal, EndVal, RateNPWP, RateNonNPWP As TextBox

        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            SqlString = "Select Code, dbo.FORMATFloat(StartValue,0) As StartValue, dbo.FORMATFloat(EndValue,0) As EndValue, " + _
                     " dbo.FORMATFloat(RateNPWP,0) As RateNPWP, dbo.FORMATFloat(RateNonNPWP,0) As RateNonNPWP from MsPPHRate " + StrFilter
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "StartValue ASC"
            End If

            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)

            GVR = DataGrid.FooterRow
            Code = GVR.FindControl("CodeAdd")
            Code.Attributes.Add("OnKeyDown", "return PressNumeric();")

            StartVal = GVR.FindControl("StartValueAdd")
            StartVal.Attributes.Add("OnKeyDown", "return PressNumeric();")

            EndVal = GVR.FindControl("EndValueAdd")
            EndVal.Attributes.Add("OnKeyDown", "return PressNumeric();")

            RateNPWP = GVR.FindControl("RateNPWPAdd")
            RateNPWP.Attributes.Add("OnKeyDown", "return PressNumeric();")

            RateNonNPWP = GVR.FindControl("RateNonNPWPAdd")
            RateNonNPWP.Attributes.Add("OnKeyDown", "return PressNumeric();")
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
            Session("SelectCommand") = "EXEC S_FormPrintMaster5 'MsPPHRate','Code','StartValue', 'EndValue', 'RateNPWP', 'RAteNonNPWP','Rate PPH Gaji','Code','Start Value','End Value', 'Rate NPWP', 'Rate Non NPWP'," + QuotedStr(StrFilter) + "," + QuotedStr(ViewState("UserId").ToString)
            Session("ReportFile") = Server.MapPath("~\Rpt\RptPrintMaster5.frx")
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
        Dim dbCode, dbStartValue, dbEndValue, dbRateNPWP, dbRateNonNPWP As TextBox


        Try
            If e.CommandName = "Insert" Then
                dbCode = DataGrid.FooterRow.FindControl("CodeAdd")
                dbStartValue = DataGrid.FooterRow.FindControl("StartValueAdd")
                dbEndValue = DataGrid.FooterRow.FindControl("EndValueAdd")
                dbRateNPWP = DataGrid.FooterRow.FindControl("RateNPWPAdd")
                dbRateNonNPWP = DataGrid.FooterRow.FindControl("RateNonNPWPAdd")

                If dbCode.Text.Trim.Length = 0 Then
                    lstatus.Text = "Code must be filled."
                    dbCode.Focus()
                    Exit Sub
                End If
                If Not IsNumeric(dbCode.Text) Then
                    lstatus.Text = "Code must in correct int"
                    dbCode.Focus()
                    Exit Sub
                End If
                If dbStartValue.Text.Trim.Length = 0 Then
                    lstatus.Text = "Start Value must be filled."
                    dbStartValue.Focus()
                    Exit Sub
                End If
                If Not IsNumeric(dbStartValue.Text) Then
                    lstatus.Text = "Start Value must be numeric type."
                    dbStartValue.Focus()
                    Exit Sub
                End If
                If dbEndValue.Text.Trim.Length = 0 Then
                    lstatus.Text = "End Value must be filled."
                    dbEndValue.Focus()
                    Exit Sub
                End If
                If Not IsNumeric(dbEndValue.Text) Then
                    lstatus.Text = "End Value must be numeric type."
                    dbEndValue.Focus()
                    Exit Sub
                End If
                If dbRateNPWP.Text.Trim.Length = 0 Then
                    lstatus.Text = "Rate NPWP must be filled."
                    dbRateNPWP.Focus()
                    Exit Sub
                End If
                If dbRateNonNPWP.Text.Trim.Length = 0 Then
                    lstatus.Text = "Rate Non NPWP must be filled."
                    dbRateNonNPWP.Focus()
                    Exit Sub
                End If
                If Not IsNumeric(dbStartValue.Text) Then
                    lstatus.Text = "Start Value must in correct format."
                    dbStartValue.Focus()
                    Exit Sub
                End If
                If Not IsNumeric(dbEndValue.Text) Then
                    lstatus.Text = "End Value must in correct format."
                    dbEndValue.Focus()
                    Exit Sub
                End If

                If Not IsNumeric(dbRateNPWP.Text) Then
                    lstatus.Text = "Rate NPWP must in correct format."
                    dbRateNPWP.Focus()
                    Exit Sub
                End If
                If Not IsNumeric(dbRateNonNPWP.Text) Then
                    lstatus.Text = "Rate Non NPWP must in correct format."
                    dbRateNonNPWP.Focus()
                    Exit Sub
                End If

                If SQLExecuteScalar("Select Code From MsPPHRate WHERE Code = " + QuotedStr(dbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Code " + QuotedStr(dbCode.Text) + " has already been exist"
                    Exit Sub
                End If

                'insert the new entry
                SQLString = "Insert into MsPPHRate SELECT " + QuotedStr(dbCode.Text) + _
                ", " + QuotedStr(dbStartValue.Text.Replace(",", "")) + ", " + QuotedStr(dbEndValue.Text.Replace(",", "")) + ", " + QuotedStr(dbRateNPWP.Text.Replace(",", "")) + _
                ", " + QuotedStr(dbRateNonNPWP.Text.Replace(",", ""))
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
            txtID = DataGrid.Rows(e.RowIndex).FindControl("Code")

            SQLExecuteNonQuery("Delete from MsPPHRate where Code = " + QuotedStr(txtID.Text), ViewState("DBConnection").ToString)
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        Dim txtStartValue, txtEndValue, txtRateNPWP, txtRateNonNPWP As TextBox
        Dim StartVal, EndVal, RateNPWP, RateNonNPWP As TextBox

        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()
            obj = DataGrid.Rows(e.NewEditIndex)
            txtStartValue = obj.FindControl("StartValueEdit")
            txtEndValue = obj.FindControl("EndValueEdit")
            txtRateNPWP = obj.FindControl("RateNPWPEdit")
            txtRateNonNPWP = obj.FindControl("RateNonNPWPEdit")
            txtStartValue.Focus()

            StartVal = DataGrid.Rows(e.NewEditIndex).FindControl("StartValueEdit")
            StartVal.Attributes.Add("OnKeyDown", "return PressNumeric();")

            EndVal = DataGrid.Rows(e.NewEditIndex).FindControl("EndValueEdit")
            EndVal.Attributes.Add("OnKeyDown", "return PressNumeric();")

            RateNPWP = DataGrid.Rows(e.NewEditIndex).FindControl("RateNPWPEdit")
            RateNPWP.Attributes.Add("OnKeyDown", "return PressNumeric();")

            RateNonNPWP = DataGrid.Rows(e.NewEditIndex).FindControl("RateNonNPWPEdit")
            RateNonNPWP.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim dbStartValue, dbEndValue, dbRateNPWP, dbRateNonNPWP As TextBox
        Dim lbCode As Label

        Try
            lbCode = DataGrid.Rows(e.RowIndex).FindControl("CodeEdit")
            dbStartValue = DataGrid.Rows(e.RowIndex).FindControl("StartValueEdit")
            dbEndValue = DataGrid.Rows(e.RowIndex).FindControl("EndValueEdit")
            dbRateNPWP = DataGrid.Rows(e.RowIndex).FindControl("RateNPWPEdit")
            dbRateNonNPWP = DataGrid.Rows(e.RowIndex).FindControl("RateNonNPWPEdit")

            If dbStartValue.Text.Trim.Length = 0 Then
                lstatus.Text = "Start Value must be filled."
                dbStartValue.Focus()
                Exit Sub
            End If
            If Not IsNumeric(dbStartValue.Text) Then
                lstatus.Text = "Start Value must in correct format."
                dbStartValue.Focus()
                Exit Sub
            End If
            If dbEndValue.Text.Trim.Length = 0 Then
                lstatus.Text = "End Value must be filled."
                dbEndValue.Focus()
                Exit Sub
            End If
            If Not IsNumeric(dbEndValue.Text) Then
                lstatus.Text = "End Value must in correct format."
                dbEndValue.Focus()
                Exit Sub
            End If
            If dbRateNPWP.Text.Trim.Length = 0 Then
                lstatus.Text = "Rate NPWP must be filled."
                dbRateNPWP.Focus()
                Exit Sub
            End If
            If Not IsNumeric(dbRateNPWP.Text) Then
                lstatus.Text = "Rate NPWP must in correct format."
                dbRateNPWP.Focus()
                Exit Sub
            End If
            If dbRateNonNPWP.Text.Trim.Length = 0 Then
                lstatus.Text = "Rate Non NPWP must be filled."
                dbRateNonNPWP.Focus()
                Exit Sub
            End If
            If Not IsNumeric(dbStartValue.Text) Then
                lstatus.Text = "Rate Non NPWP must in correct format."
                dbRateNonNPWP.Focus()
                Exit Sub
            End If

            SQLString = "Update MsPPHRate SET StartValue = " + QuotedStr(dbStartValue.Text.Replace(",", "")) + _
            ", EndValue = " + QuotedStr(dbEndValue.Text.Replace(",", "")) + ", RateNPWP = " + QuotedStr(dbRateNPWP.Text.Replace(",", "")) + _
            ", RateNonNPWP = " + QuotedStr(dbRateNonNPWP.Text.Replace(",", "")) + " where Code = " + QuotedStr(lbCode.Text)

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
