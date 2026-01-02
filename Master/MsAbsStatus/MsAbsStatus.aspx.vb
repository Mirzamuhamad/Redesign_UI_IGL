Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data

Partial Class MsAbsStatus_MsAbsStatus
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
        Dim MinAbs As TextBox

        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            SqlString = "SELECT * FROM v_MsAbsStatus " + StrFilter
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "AbsStatusCode ASC"
            End If
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)

            GVR = DataGrid.FooterRow
            MinAbs = GVR.FindControl("MinusAbsensiAdd")
            MinAbs.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    'Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
    '    Dim StrFilter As String
    '    Try
    '        StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
    '        ViewState("DBConnection") = ViewState("DBConnection")
    '        ViewState("PrintType") = "Print"
    '        ViewState("SelectCommand") = "S_FormPrintMaster 'vMsAbsStatus','AbsStatusCode','AbsStatusName','FgAbSence','MinusAbsensi','Absence Status File','Absence Status Code','Absence Status Name','Have Absence','Minus Absence'," + QuotedStr(StrFilter) + "," + QuotedStr(ViewState("UserId"))
    '        'lstatus.Text = ViewState("SelectCommand")
    '        'Exit Sub
    '        Session("ReportFile") = ".../../../Rpt/RptPrintMasterAbsStatus.frx"
    '        'iewState("ReportFile") = ".../../../Rpt/RptPrintMasterAbsStatus.frx"
    '        'lstatus.Text = ViewState("SelectCommand")
    '        'Exit Sub
    '        AttachScript("openprintdlg();", Page, Me.GetType)
    '    Catch ex As Exception
    '        lstatus.Text = "Btn Print Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("PrintType") = "Print"
            Session("DBConnection") = ViewState("DBConnection")
            Session("SelectCommand") = "S_FormPrintMaster4 'vMsAbsStatus','AbsStatusCode','AbsStatusName','FgAbSence','MinusAbsensi','Absence Status File','Absence Status Code','Absence Status Name','Absence Status Code','Minus Absence'," + QuotedStr(StrFilter) + ", " + QuotedStr(ViewState("UserId"))
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster3.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)

            lstatus.Text = "<script language='javascript'> { window.open(""../../Rpt/PrintMaster.Aspx"", ""List"",""scrollbars=yes, resizable=yes,width=600,height=500"");}</script>"
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
        Dim dbCode, dbName, dbMinusAbsence As TextBox
        Dim ddlAbsence As DropDownList
        Try

            dbCode = DataGrid.FooterRow.FindControl("AbsStatusCodeAdd")
            dbName = DataGrid.FooterRow.FindControl("AbsStatusNameAdd")
            dbMinusAbsence = DataGrid.FooterRow.FindControl("MinusAbsensiAdd")
            ddlAbsence = DataGrid.FooterRow.FindControl("AbsenceAdd")

            dbMinusAbsence.Text = dbMinusAbsence.Text.Replace(",", "")


            If e.CommandName = "Insert" Then

                If dbCode.Text.Trim.Length = 0 Then
                    lstatus.Text = "Absence Status Code must be filled."
                    dbCode.Focus()
                    Exit Sub
                End If

                If dbName.Text.Trim.Length = 0 Then
                    lstatus.Text = "Absence Status Name must be filled."
                    dbName.Focus()
                    Exit Sub
                End If

                If ddlAbsence.Text.Trim.Length = 0 Then
                    lstatus.Text = "Absence must be filled."
                    dbName.Focus()
                    Exit Sub
                End If

                If Not IsNumeric(dbMinusAbsence.Text) Then
                    lstatus.Text = "Minus Absence must be filled, and must numeric value"
                    dbMinusAbsence.Focus()
                    Exit Sub
                End If

                If Convert.ToInt64(dbMinusAbsence.Text) < 0 Then
                    lstatus.Text = "  Range Period must be Equal Or Greater than 0."
                    dbMinusAbsence.Focus()
                    Exit Sub
                End If

                If SQLExecuteScalar("SELECT AbsStatusCode From VMsAbsStatus WHERE AbsStatusCode  = " + QuotedStr(dbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Absence Status " + QuotedStr(dbCode.Text) + " has already been exist"
                    Exit Sub
                End If



                'insert the new entry
                SQLString = "Insert into MsAbsStatus (AbsStatusCode, AbsStatusName,FgAbsence,MinusAbsensi,UserId, UserDate ) " + _
                "SELECT " + QuotedStr(dbCode.Text) + ", " + QuotedStr(dbName.Text) + ", " + QuotedStr(ddlAbsence.SelectedValue) + "," + QuotedStr(dbMinusAbsence.Text.Replace(",", "")) + _
                "," + QuotedStr(ViewState("UserId").ToString) + ", getDate()"

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
            txtID = DataGrid.Rows(e.RowIndex).FindControl("AbsStatusCode")

            SQLExecuteNonQuery("Delete from MsAbsStatus where AbsStatusCode = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        Dim txt, MinAbs As TextBox

        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()
            obj = DataGrid.Rows(e.NewEditIndex)
            txt = obj.FindControl("AbsStatusNameEdit")
            txt.Focus()

            MinAbs = DataGrid.Rows(e.NewEditIndex).FindControl("MinusAbsensiEdit")
            MinAbs.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim dbName As TextBox
        Dim lbCode As Label
        Dim dbMinusAbsence As TextBox
        Dim ddlAbsence As DropDownList

        Try
            lbCode = DataGrid.Rows(e.RowIndex).FindControl("AbsStatusCodeEdit")
            dbName = DataGrid.Rows(e.RowIndex).FindControl("AbsStatusNameEdit")
            dbMinusAbsence = DataGrid.Rows(e.RowIndex).FindControl("MinusAbsensiEdit")
            ddlAbsence = DataGrid.Rows(e.RowIndex).FindControl("AbsenceEdit")

            dbMinusAbsence.Text = dbMinusAbsence.Text.Replace(",", "")
          
            If dbName.Text.Trim.Length = 0 Then
                lstatus.Text = "Absence Status Name must be filled."
                dbName.Focus()
                Exit Sub
            End If

            If ddlAbsence.Text.Trim.Length = 0 Then
                lstatus.Text = "Absence must be filled."
                dbName.Focus()
                Exit Sub
            End If

            If Not IsNumeric(dbMinusAbsence.Text) Then
                lstatus.Text = "Minus Absence must be filled, and must numeric value"
                dbMinusAbsence.Focus()
                Exit Sub
            End If

            If CInt(dbMinusAbsence.Text) < 0 Then
                lstatus.Text = "  Range Period must be Equal Or Greater than 0."
                dbMinusAbsence.Focus()
                Exit Sub
            End If

            SQLString = "Update MsAbsStatus set AbsStatusName= " + QuotedStr(dbName.Text) + ", FgAbsence = " + QuotedStr(ddlAbsence.SelectedValue) + ",MinusAbsensi = " + QuotedStr(dbMinusAbsence.Text.Replace(",", "")) + "" & _
            "where AbsStatusCode = " & QuotedStr(lbCode.Text)

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
