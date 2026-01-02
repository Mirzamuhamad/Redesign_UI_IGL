Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data

Partial Class MsTerminateReason_MsTerminateReason
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        If Not IsPostBack Then
            InitProperty()
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
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
        Dim SevVar, ServiceVar As TextBox

        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            SqlString = "SELECT * FROM vMsPHKReason " + StrFilter
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "PHKCode ASC"
            End If
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)

            GVR = DataGrid.FooterRow
            SevVar = GVR.FindControl("SeveranceVariableAdd")
            SevVar.Attributes.Add("OnKeyDown", "return PressNumeric();")

            ServiceVar = GVR.FindControl("ServiceVariableAdd")
            ServiceVar.Attributes.Add("OnKeyDown", "return PressNumeric();")
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
            Session("SelectCommand") = "EXEC S_FormPrintMaster5 'VMsPHKReason','PHKCode','PHKName','fgBlacklist','Variable1','Variable2','Terminate Reason File','PHK Code','PHK Name','Blacklist','Severance Variable','Service Variable'," + QuotedStr(StrFilter) + "," + QuotedStr(ViewState("UserId"))
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster4.frx"

            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            lstatus.Text = "Btn Print Error : " + ex.ToString
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
        Dim dbCode, dbName As TextBox
        Dim ddlBlacklist As DropDownList
        Dim dbSeverance As TextBox
        Dim dbService As TextBox
        Try

            dbCode = DataGrid.FooterRow.FindControl("ReasonCodeAdd")
            dbName = DataGrid.FooterRow.FindControl("ReasonNameAdd")
            ddlBlacklist = DataGrid.FooterRow.FindControl("BlacklistAdd")
            dbSeverance = DataGrid.FooterRow.FindControl("SeveranceVariableAdd")
            dbService = DataGrid.FooterRow.FindControl("ServiceVariableAdd")
            dbSeverance.Text = dbSeverance.Text.Replace(",", "")
            dbService.Text = dbService.Text.Replace(",", "")

            If e.CommandName = "Insert" Then

                If dbCode.Text.Trim.Length = 0 Then
                    lstatus.Text = "Reason Status Code must be filled."
                    dbCode.Focus()
                    Exit Sub
                End If

                If dbName.Text.Trim.Length = 0 Then
                    lstatus.Text = "Reason Status Name must be filled."
                    dbName.Focus()
                    Exit Sub
                End If

                If ddlBlacklist.Text.Trim.Length = 0 Then
                    lstatus.Text = "Blacklist must be filled."
                    dbName.Focus()
                    Exit Sub
                End If

                If Not IsNumeric(dbSeverance.Text) Then
                    lstatus.Text = "Severance Variable must be filled, and must numeric value"
                    dbSeverance.Focus()
                    Exit Sub
                End If

                If Not IsNumeric(dbService.Text) Then
                    lstatus.Text = "Service Variable must be filled, and must numeric value"
                    dbService.Focus()
                    Exit Sub
                End If

                If Convert.ToInt64(dbSeverance.Text) < 0 Then
                    lstatus.Text = " Severance Variable must be Greater than 0."
                    dbSeverance.Focus()
                    Exit Sub
                End If

                If Convert.ToInt64(dbService.Text) < 0 Then
                    lstatus.Text = " Severance Variable must be Greater than 0."
                    dbService.Focus()
                    Exit Sub
                End If

                dbSeverance.Text = dbSeverance.Text.Replace(",", "")
                dbService.Text = dbService.Text.Replace(",", "")

                If SQLExecuteScalar("SELECT PHKCode From VMsPHKReason WHERE PHKCode  = " + QuotedStr(dbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Terminate Reason " + QuotedStr(dbCode.Text) + " has already been exist"
                    Exit Sub
                End If

                'insert the new entry
                SQLString = "Insert into MsPHKReason (PHKCode, PHKName,FgBlacklist,Variable1,Variable2,UserId, UserDate ) " + _
                "SELECT " + QuotedStr(dbCode.Text) + ", " + QuotedStr(dbName.Text) + ", " + QuotedStr(ddlBlacklist.SelectedValue) + "," + QuotedStr(dbSeverance.Text.Replace(",", "")) + _
                "," + QuotedStr(dbService.Text.Replace(",", "")) + "," + QuotedStr(ViewState("UserId").ToString) + ", getDate()"

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
            txtID = DataGrid.Rows(e.RowIndex).FindControl("ReasonCode")

            SQLExecuteNonQuery("Delete from MsPHKReason where PHKCode = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        Dim txt, SevVar, ServiceVar As TextBox

        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()
            obj = DataGrid.Rows(e.NewEditIndex)
            txt = obj.FindControl("ReasonNameEdit")
            txt.Focus()

            SevVar = DataGrid.Rows(e.NewEditIndex).FindControl("SeveranceVariableEdit")
            SevVar.Attributes.Add("OnKeyDown", "return PressNumeric();")

            ServiceVar = DataGrid.Rows(e.NewEditIndex).FindControl("ServiceVariableEdit")
            ServiceVar.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim dbName As TextBox
        Dim lbCode As Label
        Dim ddlBlacklist As DropDownList
        Dim dbSeverance As TextBox
        Dim dbService As TextBox

        Try
            lbCode = DataGrid.Rows(e.RowIndex).FindControl("ReasonCodeEdit")
            dbName = DataGrid.Rows(e.RowIndex).FindControl("ReasonNameEdit")
            ddlBlacklist = DataGrid.Rows(e.RowIndex).FindControl("BlacklistEdit")
            dbSeverance = DataGrid.Rows(e.RowIndex).FindControl("SeveranceVariableEdit")
            dbService = DataGrid.Rows(e.RowIndex).FindControl("ServiceVariableEdit")

            If dbName.Text.Trim.Length = 0 Then
                lstatus.Text = "Reason Status Name must be filled."
                dbName.Focus()
                Exit Sub
            End If

            If ddlBlacklist.Text.Trim.Length = 0 Then
                lstatus.Text = "Blacklist must be filled."
                dbName.Focus()
                Exit Sub
            End If

            If Not IsNumeric(dbSeverance.Text) Then
                lstatus.Text = "Severance Variable must be filled, and must numeric value"
                dbSeverance.Focus()
                Exit Sub
            End If

            If Not IsNumeric(dbService.Text) Then
                lstatus.Text = "Service Variable must be filled, and must numeric value"
                dbService.Focus()
                Exit Sub
            End If

            If CInt(dbSeverance.Text) < 0 Then
                lstatus.Text = " Severance Variable must be Greater than 0."
                dbSeverance.Focus()
                Exit Sub
            End If

            If CInt(dbService.Text) < 0 Then
                lstatus.Text = " Severance Variable must be Greater than 0."
                dbService.Focus()
                Exit Sub
            End If

            dbSeverance.Text = dbSeverance.Text.Replace(",", "")
            dbService.Text = dbService.Text.Replace(",", "")


            SQLString = "Update MsPHKReason set PHKName= " + QuotedStr(dbName.Text) + ", FgBlackList = " + QuotedStr(ddlBlacklist.SelectedValue) + ",Variable1 = " + QuotedStr(dbSeverance.Text.Replace(",", "")) + "" & _
            ",Variable2 = " + QuotedStr(dbService.Text.Replace(",", "")) + " where PHKCode = " & QuotedStr(lbCode.Text)

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
