Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data
Imports System.Data.SqlClient

Partial Class Master_MsMethod_MsMethod
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            InitProperty()
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
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
        Dim GVR As GridViewRow
        Dim RangeDay, OneYear As TextBox

        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            SqlString = " SELECT * FROM MsMethod " + StrFilter
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "MethodCode ASC"
            End If
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)

            GVR = DataGrid.FooterRow
            RangeDay = GVR.FindControl("RangeDayAdd")
            RangeDay.Attributes.Add("OnKeyDown", "return PressNumeric();")

            OneYear = GVR.FindControl("xPeriodAdd")
            OneYear.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Protected Sub DataGrid_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles DataGrid.RowDataBound

    End Sub

    Public Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim tbName, RangeDay, OneYear As TextBox

        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()
            tbName = DataGrid.Rows(e.NewEditIndex).FindControl("MethodNameEdit")
            tbName.Focus()
            Dim dgi As GridViewRow
            dgi = DataGrid.Rows(e.NewEditIndex)

            RangeDay = DataGrid.Rows(e.NewEditIndex).FindControl("RangeDayEdit")
            RangeDay.Attributes.Add("OnKeyDown", "return PressNumeric();")

            OneYear = DataGrid.Rows(e.NewEditIndex).FindControl("xPeriodEdit")
            OneYear.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub
    
    Private Sub DataGrid_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles DataGrid.RowCancelingEdit
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
        Dim dbCode, dbName, dbXPeriod, dbRangeDay As TextBox
        Dim ddlMethodRange As DropDownList
        Dim GVR As GridViewRow
        Try
            If e.CommandName = "Insert" Then
                GVR = DataGrid.FooterRow
                dbCode = GVR.FindControl("MethodCodeAdd")
                dbName = GVR.FindControl("MethodNameAdd")
                ddlMethodRange = GVR.FindControl("MethodRangeAdd")
                dbRangeDay = GVR.FindControl("RangeDayAdd")
                dbXPeriod = GVR.FindControl("XPeriodAdd")

                If dbCode.Text.Trim.Length = 0 Then
                    lstatus.Text = MessageDlg("Method Code must be filled ")
                    dbCode.Focus()
                    Exit Sub
                End If
                If dbName.Text.Trim.Length = 0 Then
                    lstatus.Text = MessageDlg("Method Name must be filled ")
                    dbName.Focus()
                    Exit Sub
                End If
                If dbRangeDay.Text.Trim.Length = 0 Then
                    lstatus.Text = MessageDlg("Range Period must be filled.")
                    dbRangeDay.Focus()
                    Exit Sub
                End If
                If dbXPeriod.Text.Trim.Length = 0 Then
                    lstatus.Text = MessageDlg("1 Year = must be filled.")
                    dbRangeDay.Focus()
                    Exit Sub
                End If

                If IsNumeric(dbRangeDay.Text.Replace(",", "")) = 0 Then
                    lstatus.Text = MessageDlg("Range Period must be in numeric.")
                    dbRangeDay.Focus()
                    Exit Sub
                End If
                If IsNumeric(dbXPeriod.Text.Replace(",", "")) = 0 Then
                    lstatus.Text = MessageDlg("1 Year = must be in numeric.")
                    dbXPeriod.Focus()
                    Exit Sub
                End If

                If SQLExecuteScalar("SELECT MethodCode From VMsMethod WHERE MethodCode = " + QuotedStr(dbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Method " + QuotedStr(dbCode.Text) + " has already been exist"
                    Exit Sub
                End If

                'insert the new entry
                SQLString = "Insert into MsMethod (MethodCode, MethodName, MethodRange, XPeriod, RangeDay, UserID, UserDate) " + _
                "SELECT " + QuotedStr(dbCode.Text) + "," + QuotedStr(dbName.Text) + "," + QuotedStr(ddlMethodRange.SelectedValue) + "," + _
                QuotedStr(dbXPeriod.Text.Replace(",", "")) + ", " + QuotedStr(dbRangeDay.Text.Replace(",", "")) + ", " + _
                QuotedStr(ViewState("UserId").ToString) + ", GetDate()"
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                bindDataGrid()

            ElseIf e.CommandName = "AssUser" Then
                Dim lbCode, lbName As Label
                GVR = DataGrid.Rows(CInt(e.CommandArgument))
                lbCode = GVR.FindControl("MethodCode")
                lbName = GVR.FindControl("MethodName")
                If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenAssign('Method User', '" + Request.QueryString("KeyId") + "','" + lbCode.Text + " | " + lbName.Text + "','AssMsMethodUser');", True)
                End If
                'ElseIf e.CommandName = "AssJbt" Then
                '    Dim lbCode, lbName As Label
                '    GVR = DataGrid.Rows(CInt(e.CommandArgument))
                '    lbCode = GVR.FindControl("MethodCode")
                '    lbName = GVR.FindControl("MethodName")
                '    If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                '        Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenAssign('Method Job Title', '" + Request.QueryString("KeyId") + "','" + lbCode.Text + " | " + lbName.Text + "','AssMsMethodJbt');", True)
                '    End If
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim dbName, dbXPeriod, dbRangeDay As TextBox
        Dim ddlMethodRange As DropDownList
        Dim lbCode As Label
        Dim GVR As GridViewRow
        Try
            GVR = DataGrid.Rows(e.RowIndex)
            lbCode = GVR.FindControl("MethodCodeEdit")
            dbName = GVR.FindControl("MethodNameEdit")
            ddlMethodRange = GVR.FindControl("MethodRangeEdit")
            dbXPeriod = GVR.FindControl("XPeriodEdit")
            dbRangeDay = GVR.FindControl("RangeDayEdit")
            
            If dbName.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("Method Name must be filled.")
                dbName.Focus()
                Exit Sub
            End If

            If dbRangeDay.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("Range Period must be filled.")
                dbRangeDay.Focus()
                Exit Sub
            End If

            If IsNumeric(dbRangeDay.Text.Replace(",", "")) = 0 Then
                lstatus.Text = MessageDlg("Range Period must be in numeric.")
                dbRangeDay.Focus()
                Exit Sub
            End If
            If IsNumeric(dbXPeriod.Text.Replace(",", "")) = 0 Then
                lstatus.Text = MessageDlg("1 Year = must be in numeric.")
                dbXPeriod.Focus()
                Exit Sub
            End If


            SQLString = "Update MsMethod set MethodName = " + QuotedStr(dbName.Text) + _
            ", MethodRange =" + QuotedStr(ddlMethodRange.SelectedValue) + _
            ", XPeriod =" + QuotedStr(dbXPeriod.Text.Replace(",", "")) + ", RangeDay =" + QuotedStr(dbRangeDay.Text.Replace(",", "")) + _
            " where MethodCode = " + QuotedStr(lbCode.Text)
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
            txtID = DataGrid.Rows(e.RowIndex).FindControl("MethodCode")

            SQLExecuteNonQuery("Delete from MsMethodUser where Method = " + QuotedStr(txtID.Text), ViewState("DBConnection").ToString)
           ' SQLExecuteNonQuery("Delete from MsMethodJbt where Method = " + QuotedStr(txtID.Text), ViewState("DBConnection").ToString)
            SQLExecuteNonQuery("Delete from MsMethod where MethodCode = " + QuotedStr(txtID.Text), ViewState("DBConnection").ToString)

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
            lstatus.Text = "DataGrid_SortingError =" + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim StrFilter, SQLString As String
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("PrintType") = "Print"
            Session("DBConnection") = ViewState("DBConnection")
            SQLString = "S_FormPrintMaster3 'VMsMethod', 'MethodCode', 'MethodName', 'CONVERT(VARCHAR(3),XPeriod)+  ''-''  + MethodRange+ ''-'' + CONVERT(VARCHAR(8),RangeDay)', 'Method File', 'Method Code', 'Method Name', 'XPeriod - Range', " + QuotedStr(StrFilter) + ", " + QuotedStr(ViewState("UserId"))
            Session("SelectCommand") = SQLString
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster2.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)

        Catch ex As Exception
            lstatus.Text = "btn Print Error : " + ex.ToString
        End Try
    End Sub

End Class
