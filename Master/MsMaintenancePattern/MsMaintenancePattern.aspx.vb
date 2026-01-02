Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data

Partial Class Master_MsMaintenancePattern_MsMaintenancePattern
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
            ddlRow.SelectedValue = "15"

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
            DataGrid.PageIndex = 0
            DataGrid.EditIndex = -1
            DataGrid.PageSize = ddlRow.SelectedValue
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
        Dim ddlPatternType As DropDownList
        Dim tbXMonth, tbEveryMonth, tbEveryWeek As TextBox
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            SqlString = "SELECT * FROM MsMaintenancePattern " + StrFilter
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)

            GVR = DataGrid.FooterRow
            ddlPatternType = GVR.FindControl("PatternTypeAdd")
            tbXMonth = GVR.FindControl("tbXMonthAdd")
            tbXMonth.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbEveryMonth = GVR.FindControl("tbEveryMonthAdd")
            tbEveryMonth.Attributes.Add("OnKeyDown", "return PressMonth();")
            tbEveryWeek = GVR.FindControl("EveryWeekAdd")
            tbEveryWeek.Attributes.Add("OnKeyDown", "return PressWeek();")

            If ddlPatternType.SelectedIndex = 0 Then
                tbEveryMonth.Enabled = False
                tbXMonth.Enabled = True
                tbEveryWeek.Enabled = False
                tbEveryWeek.Text = ""
                tbEveryMonth.Text = ""
            Else
                tbXMonth.Text = "0"
                tbEveryMonth.Enabled = True
                tbXMonth.Enabled = False
                tbEveryWeek.Enabled = True
            End If

        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub
    Protected Sub DataGrid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGrid.PageIndexChanging
        DataGrid.PageIndex = e.NewPageIndex
        bindDataGrid()
    End Sub
    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        Dim txt, tbxmonth, tbeverymonth, tbeveryweek As TextBox
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()
            obj = DataGrid.Rows(e.NewEditIndex)
            txt = obj.FindControl("PatternNameEdit")

            tbxmonth = obj.FindControl("tbXMonthEdit")
            tbxmonth.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbeverymonth = obj.FindControl("tbEveryMonthEdit")
            tbeverymonth.Attributes.Add("OnKeyDown", "return PressMonth();")
            tbeveryweek = obj.FindControl("EveryWeekEdit")
            tbeveryweek.Attributes.Add("OnKeyDown", "return PressWeek();")

            txt.Focus()

            Dim GVR As GridViewRow
            Dim ddlPatternType As DropDownList


            GVR = DataGrid.Rows(DataGrid.EditIndex)
            ddlPatternType = GVR.FindControl("PatternTypeEdit")
            tbxmonth = GVR.FindControl("tbXMonthEdit")
            tbeverymonth = GVR.FindControl("tbEveryMonthEdit")

            If ddlPatternType.SelectedIndex = 0 Then
                tbeverymonth.Enabled = False
                tbxmonth.Enabled = True
                tbeveryweek.Enabled = False
                tbeveryweek.Text = ""
                tbeverymonth.Text = ""
            Else
                tbxmonth.Text = "0"
                tbeverymonth.Enabled = True
                tbxmonth.Enabled = False
                tbeveryweek.Enabled = True
            End If

        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
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
    Protected Sub DataGrid_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGrid.RowCommand
        Dim SQLString As String
        Dim ddlType As DropDownList
        Dim dbCode, dbName, dbxmonth, dbeverymonth, dbeveryweek As TextBox
        Try
            If e.CommandName = "Insert" Then
                dbCode = DataGrid.FooterRow.FindControl("PatternCodeAdd")
                dbName = DataGrid.FooterRow.FindControl("PatternNameAdd")
                ddlType = DataGrid.FooterRow.FindControl("PatternTypeAdd")
                dbxmonth = DataGrid.FooterRow.FindControl("tbXMonthAdd")
                dbeverymonth = DataGrid.FooterRow.FindControl("tbEveryMonthAdd")
                dbeveryweek = DataGrid.FooterRow.FindControl("EveryWeekAdd")

                If dbCode.Text.Trim.Length = 0 Then
                    lstatus.Text = MessageDlg("Pattern Code must be filled.")
                    dbCode.Focus()
                    Exit Sub
                End If
                If dbName.Text.Trim.Length = 0 Then
                    lstatus.Text = MessageDlg("Pattern Name must be filled.")
                    dbName.Focus()
                    Exit Sub
                End If


                If IsNumeric(dbxmonth.Text.Replace(",", "")) = 0 Then
                    lstatus.Text = MessageDlg("Range Month must be in numeric.")
                    dbxmonth.Focus()
                    Exit Sub
                End If

                If ddlType.SelectedIndex = 0 Then
                    If CFloat(dbxmonth.Text) < 1 Then
                        lstatus.Text = MessageDlg("Range Month must be > 0.")
                        dbxmonth.Focus()
                        Exit Sub
                    End If
                Else
                    If CBool(InStr(dbeveryweek.Text, ".")) Then
                        lstatus.Text = MessageDlg("use ','.")
                        dbeveryweek.Focus()
                        Exit Sub
                    End If
                    If dbeveryweek.Text.Trim.Length = 0 Then
                        lstatus.Text = MessageDlg("@ Week must be filled.")
                        dbeveryweek.Focus()
                        Exit Sub
                    End If
                End If

                If CBool(InStr(dbeverymonth.Text, ".")) Then
                    lstatus.Text = MessageDlg("use ','.")
                    dbeverymonth.Focus()
                    Exit Sub
                End If

                If SQLExecuteScalar("SELECT PatternCode From VMsMaintenancePattern WHERE PatternCode = " + QuotedStr(dbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Pattern " + QuotedStr(dbCode.Text) + " has already been exist"
                    Exit Sub
                End If


                'insert the new entry
                SQLString = "Insert into MsMaintenancePattern (PatternCode, PatternName, PatternType, XMonth, EveryMonth, EveryWeek , UserId, UserDate) " + _
                "SELECT " + QuotedStr(dbCode.Text) + ", " + QuotedStr(dbName.Text) + ", " + QuotedStr(ddlType.Text) + ", " + dbxmonth.Text.Replace(",", "") + ", " + _
                QuotedStr(dbeverymonth.Text) + ", " + QuotedStr(dbeveryweek.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", GetDate() "
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                bindDataGrid()
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim ddlType As DropDownList
        Dim dbName, dbxmonth, dbeverymonth, dbeveryweek As TextBox
        Dim lbCode As Label

        Try
            lbCode = DataGrid.Rows(e.RowIndex).FindControl("PatternCodeEdit")
            dbName = DataGrid.Rows(e.RowIndex).FindControl("PatternNameEdit")
            ddlType = DataGrid.Rows(e.RowIndex).FindControl("PatternTypeEdit")
            dbxmonth = DataGrid.Rows(e.RowIndex).FindControl("tbXMonthEdit")
            dbeverymonth = DataGrid.Rows(e.RowIndex).FindControl("tbEveryMonthEdit")
            dbeveryweek = DataGrid.Rows(e.RowIndex).FindControl("EveryWeekEdit")

            If dbName.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("Pattern Name must be filled.")
                dbName.Focus()
                Exit Sub
            End If
            If IsNumeric(dbxmonth.Text.Replace(",", "")) = 0 Then
                lstatus.Text = MessageDlg("Range Month must be in numeric.")
                dbxmonth.Focus()
                Exit Sub
            End If

            If ddlType.SelectedIndex = 0 Then
                If CFloat(dbxmonth.Text) < 1 Then
                    lstatus.Text = MessageDlg("Range Month must be > 0.")
                    dbxmonth.Focus()
                    Exit Sub
                End If
            Else
                If CBool(InStr(dbeveryweek.Text, ".")) Then
                    lstatus.Text = MessageDlg("use ','.")
                    dbeveryweek.Focus()
                    Exit Sub
                End If
                If CBool(InStr(dbeverymonth.Text, ".")) Then
                    lstatus.Text = MessageDlg("use ','.")
                    dbeverymonth.Focus()
                    Exit Sub
                End If
                If dbeverymonth.Text.Trim.Length = 0 Then
                    lstatus.Text = MessageDlg("Maintenance Every Month must be filled.")
                    dbeverymonth.Focus()
                    Exit Sub
                End If
                If dbeveryweek.Text.Trim.Length = 0 Then
                    lstatus.Text = MessageDlg("@ Week must be filled.")
                    dbeveryweek.Focus()
                    Exit Sub
                End If

            End If

            SQLString = "UPDATE MsMaintenancePattern SET PatternName= " + QuotedStr(dbName.Text) + ", PatternType= " + QuotedStr(ddlType.Text) + ", " & _
            " XMonth = " + dbxmonth.Text.Replace(",", "") + ", EveryMonth= " + QuotedStr(dbeverymonth.Text) + ", EveryWeek= " + QuotedStr(dbeveryweek.Text) + " WHERE PatternCode = " & QuotedStr(lbCode.Text)

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
            txtID = DataGrid.Rows(e.RowIndex).FindControl("PatternCode")

            SQLExecuteNonQuery("DELETE FROM MsMaintenancePattern WHERE PatternCode = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
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
            Session("DBConnection") = ViewState("DBConnection")
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("PrintType") = "Print"
            Session("SelectCommand") = "exec S_FormPrintMaster6 'MsMaintenancePattern','PatternCode','PatternName','PatternType','XMonth','EveryMonth','EveryWeek','Maintenance Pattern File','Pattern Code','Pattern Name','PatternType','XMonth','EveryMonth','EveryWeek'," + QuotedStr(StrFilter) + ", " + QuotedStr(ViewState("UserId"))
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster6.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            lstatus.Text = "btn Print Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlPatternTypeadd_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dgi As GridViewRow
        Dim ddltype As DropDownList
        Dim tbXMonth, tbEveryMonth, tbxEveryWeek As TextBox

        Try

            dgi = DataGrid.FooterRow

            ddltype = dgi.FindControl("PatternTypeAdd")
            tbXMonth = dgi.FindControl("tbXMonthAdd")
            tbEveryMonth = dgi.FindControl("tbEveryMonthAdd")
            tbxEveryWeek = dgi.FindControl("EveryWeekAdd")

            If ddltype.SelectedIndex = 0 Then
                tbXMonth.Text = ""
                tbEveryMonth.Enabled = False
                tbXMonth.Enabled = True
                tbxEveryWeek.Enabled = False
                tbxEveryWeek.Text = ""
                tbEveryMonth.Text = ""
            Else
                tbXMonth.Text = "0"
                tbEveryMonth.Enabled = True
                tbXMonth.Enabled = False
                tbxEveryWeek.Enabled = True
            End If

        Catch ex As Exception
            lstatus.Text = "Belongs To Index Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlPatternTypeEdit_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dgi As GridViewRow
        Dim ddltype As DropDownList
        Dim tbXMonth, tbEveryMonth, tbEveryWeek As TextBox

        Try

            dgi = DataGrid.Rows(DataGrid.EditIndex)

            ddltype = dgi.FindControl("PatternTypeEdit")
            tbXMonth = dgi.FindControl("tbXMonthEdit")
            tbEveryMonth = dgi.FindControl("tbEveryMonthEdit")
            tbEveryWeek = dgi.FindControl("EveryWeekEdit")

            If ddltype.SelectedIndex = 0 Then
                tbXMonth.Text = ""
                tbEveryMonth.Enabled = False
                tbXMonth.Enabled = True
                tbEveryWeek.Enabled = False
                tbEveryWeek.Text = ""
                tbEveryMonth.Text = ""
            Else
                tbXMonth.Text = "0"
                tbEveryMonth.Enabled = True
                tbXMonth.Enabled = False
                tbEveryWeek.Enabled = True
            End If
            

        Catch ex As Exception
            lstatus.Text = "Belongs To Index Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlRow_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlRow.SelectedIndexChanged
        Try
            DataGrid.PageIndex = 0
            DataGrid.EditIndex = -1
            DataGrid.PageSize = ddlRow.SelectedValue
            bindDataGrid()
        Catch ex As Exception
            lstatus.Text = "ddlRow_SelectedIndexChanged =" + vbCrLf + ex.ToString
        End Try
    End Sub

End Class
