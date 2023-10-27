Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data
Imports System.Data.SqlClient

Partial Class Master_MsTerm_MsTerm
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            InitProperty()
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            DataGridDt.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)

            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGridDt.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
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
        Dim XPeriod, RangePeriod As TextBox

        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            SqlString = " SELECT * FROM VMsTerm " + StrFilter
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "Term_Code ASC"
                ViewState("SortOrder") = "ASC"
            End If
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection"))

            GVR = DataGrid.FooterRow
            XPeriod = GVR.FindControl("XPeriodAdd")
            XPeriod.Attributes.Add("OnKeyDown", "return PressNumeric();")

            RangePeriod = GVR.FindControl("XRangeAdd")
            RangePeriod.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Private Sub bindDataGridDt()
        Dim tempDS As New DataSet()
        Dim DV As DataView
        Try
            tempDS = SQLExecuteQuery("SELECT * FROM MsTermDT " + _
            "WHERE TermCode = " + QuotedStr(ViewState("Nmbr")), ViewState("DBConnection"))

            DV = tempDS.Tables(0).DefaultView

            If DV.Count = 0 Then
                Dim DT As DataTable = tempDS.Tables(0)
                ShowGridViewIfEmpty(DT, DataGridDt)
                DV = DT.DefaultView
            Else
                DV.Sort = Session("SortExpressionDt")
                DataGridDt.DataSource = DV
                DataGridDt.DataBind()
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Dt Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Protected Sub DataGrid_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles DataGrid.RowDataBound

    End Sub

    Public Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim tbName, XPeriod, RangePeriod As TextBox
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()
            tbName = DataGrid.Rows(e.NewEditIndex).FindControl("TermNameEdit")

            XPeriod = DataGrid.Rows(e.NewEditIndex).FindControl("XPeriodEdit")
            XPeriod.Attributes.Add("OnKeyDown", "return PressNumeric();")

            RangePeriod = DataGrid.Rows(e.NewEditIndex).FindControl("XRangeEdit")
            RangePeriod.Attributes.Add("OnKeyDown", "return PressNumeric();")

            tbName.Focus()
            Dim dgi As GridViewRow
            dgi = DataGrid.Rows(e.NewEditIndex)
        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles DataGridDt.RowDataBound

    End Sub
    Public Sub DataGridDt_RowEditing(ByVal sender As Object, ByVal e As GridViewEditEventArgs) Handles DataGridDt.RowEditing
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGridDt.EditIndex = e.NewEditIndex
            DataGridDt.ShowFooter = False
            bindDataGridDt()
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
    Private Sub DataGridDt_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles DataGridDt.RowCancelingEdit
        Try
            DataGridDt.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGridDt.EditIndex = -1
            bindDataGridDt()
        Catch ex As Exception
            lstatus.Text = "DataGridDt_Cancel Error : " + vbCrLf + ex.ToString
        End Try

    End Sub

    Protected Sub DataGrid_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGrid.RowCommand
        Dim SQLString As String
        Dim dbCode, dbName, dbXPeriod, dbXRange, dbPercentage, dbPPN As TextBox
        Dim ddlTypeRange As DropDownList
        Dim ddlFgCBD As DropDownList
        Dim GVR As GridViewRow
        Try
            If e.CommandName = "Insert" Then
                GVR = DataGrid.FooterRow
                dbCode = GVR.FindControl("TermCodeAdd")
                dbName = GVR.FindControl("TermNameAdd")
                dbXPeriod = GVR.FindControl("XPeriodAdd")
                dbXRange = GVR.FindControl("XRangeAdd")
                dbPercentage = GVR.FindControl("PercentageAdd")
                dbPPN = GVR.FindControl("PPNAdd")
                ddlTypeRange = GVR.FindControl("TypeRangeAdd")
                ddlFgCBD = GVR.FindControl("FgCBDAdd")

                If dbCode.Text.Trim.Length = 0 Then
                    lstatus.Text = "Term Code must be filled "
                    dbCode.Focus()
                    Exit Sub
                End If
                If dbName.Text.Trim.Length = 0 Then
                    lstatus.Text = "Term Name must be filled "
                    dbName.Focus()
                    Exit Sub
                End If

                If dbXRange.Text.Trim.Length = 0 Then
                    lstatus.Text = "Range Period must be filled."
                    dbXRange.Focus()
                    Exit Sub
                End If

                If Not IsNumeric(dbXPeriod.Text) Then
                    lstatus.Text = "Period must be numeric values."
                    dbXPeriod.Focus()
                    Exit Sub
                End If

                If dbXPeriod.Text < 1 Then
                    lstatus.Text = " Period must be greater than 0."
                    dbXPeriod.Focus()
                    Exit Sub
                End If

                If Not IsNumeric(dbXRange.Text) Then
                    lstatus.Text = "Range Period must be numeric values."
                    dbXRange.Focus()
                    Exit Sub
                End If

                'If dbXRange.Text < 1 Then
                '    lstatus.Text = "  Range Period must be equal greater than 0."
                '    dbXRange.Focus()
                '    Exit Sub
                'End If

                dbXRange.Text = dbXRange.Text.Replace(",", "")
                dbXPeriod.Text = dbXPeriod.Text.Replace(",", "")

                If SQLExecuteScalar("SELECT Term_Code From VMsTerm WHERE Term_Code = " + QuotedStr(dbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Term " + QuotedStr(dbCode.Text) + " has already been exist"
                    Exit Sub
                End If

                'insert the new entry
                SQLString = "Insert into MsTerm (TermCode, TermName, XPeriod, TypeRange, FgCBD, XRange, UserID, UserDate) " + _
                "SELECT " + QuotedStr(dbCode.Text) + "," + QuotedStr(dbName.Text) + "," + _
                dbXPeriod.Text.Replace(",", "") + "," + QuotedStr(ddlTypeRange.SelectedValue) + ", " + QuotedStr(ddlFgCBD.SelectedValue) + ", " + dbXRange.Text.Replace(",", "") + ", " + _
                QuotedStr(ViewState("UserId").ToString) + ", GetDate()"
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                bindDataGrid()

            ElseIf e.CommandName = "View" Then
                Dim lbCode, lbName, Tlbperiod, Tlbxrange, Tlbtyperange, TlbFgCBD As Label
                GVR = DataGrid.Rows(CInt(e.CommandArgument))
                lbCode = GVR.FindControl("TermCode")
                lbName = GVR.FindControl("TermName")
                Tlbperiod = GVR.FindControl("XPeriod")
                Tlbxrange = GVR.FindControl("XRange")
                Tlbtyperange = GVR.FindControl("TypeRange")
                TlbFgCBD = GVR.FindControl("FgCBD")

                ViewState("Nmbr") = lbCode.Text
                lbTerm.Text = lbCode.Text
                lbTermName.Text = lbName.Text
                lbPeriod.Text = Tlbperiod.Text
                lbXRange.Text = Tlbxrange.Text
                lbTypeRange.Text = Tlbtyperange.Text
                lbFgCBD.Text = TlbFgCBD.Text

                pnlHd.Visible = False
                pnlDt.Visible = True
                bindDataGridDt()
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub
    Public Sub DataGridDt_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGridDt.RowCommand
        Dim tbXRange, tbPercentage, tbPPN As TextBox
        Dim ddlTypeRange As DropDownList
        Dim ddlFgCBD As DropDownList
        Dim lbPeriod As Label
        Dim SQLString As String
        Dim GVR As GridViewRow
        Try
            If e.CommandName = "Insert" Then
                GVR = DataGridDt.FooterRow
                ddlTypeRange = GVR.FindControl("TypeRangeAdd")
                ddlFgCBD = GVR.FindControl("FgCBDAdd")
                lbPeriod = GVR.FindControl("PeriodAdd")
                tbXRange = GVR.FindControl("XRangeAdd")
                lbPeriod.Text = lbPeriod.Text.Replace(",", "")
                tbXRange.Text = tbXRange.Text.Replace(",", "")
                tbPercentage = GVR.FindControl("PercentageAdd")
                tbPPN = GVR.FindControl("PPNAdd")
                If lbPeriod.Text.Trim = "" Then
                    lstatus.Text = "<script language='javascript'>alert('Type Range Charge must be filled');</script>"
                    lbPeriod.Focus()
                    Exit Sub
                End If

                SQLString = "Insert Into MsTermDt (TermCode, Period, TypeRange, FgCBD, XRange, Percentage, PPN, UserId, UserDate) " + _
                "SELECT " + QuotedStr(ViewState("Nmbr")) + "," + QuotedStr(lbPeriod.Text) + "," + QuotedStr(ddlTypeRange.SelectedValue) + "," + QuotedStr(ddlFgCBD.SelectedValue) + "," + _
                QuotedStr(tbPercentage.Text) + "," + QuotedStr(tbPercentage.Text) + "," + QuotedStr(tbPPN.Text) + "," + _
                QuotedStr(ViewState("UserId")) + ", getDate()"

                SQLString = Replace(SQLString, "''", "NULL")
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                bindDataGridDt()
            End If
        Catch ex As Exception
            lstatus.Text = "Item Command Dt Error" + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim dbName, dbXPeriod, dbXRange As TextBox
        Dim lbCode As Label
        Dim ddlTypeRange As DropDownList
        Dim ddlFgCBD As DropDownList
        Dim GVR As GridViewRow
        Try
            GVR = DataGrid.Rows(e.RowIndex)
            lbCode = GVR.FindControl("TermCodeEdit")
            dbName = GVR.FindControl("TermNameEdit")
            dbXPeriod = GVR.FindControl("XPeriodEdit")
            dbXRange = GVR.FindControl("XRangeEdit")
            dbXPeriod.Text = dbXPeriod.Text.Replace(",", "")
            dbXRange.Text = dbXRange.Text.Replace(",", "")
            ddlTypeRange = GVR.FindControl("TypeRangeEdit")
            ddlFgCBD = GVR.FindControl("FgCBDEdit")

            If dbName.Text.Trim.Length = 0 Then
                lstatus.Text = "Term Name must be filled."
                dbName.Focus()
                Exit Sub
            End If

            If dbXRange.Text.Trim.Length = 0 Then
                lstatus.Text = "Range Period must be filled."
                dbXRange.Focus()
                Exit Sub
            End If

            If Not IsNumeric(dbXPeriod.Text) Then
                lstatus.Text = "Period must be numeric values."
                dbXPeriod.Focus()
                Exit Sub
            End If

            If dbXPeriod.Text < 1 Then
                lstatus.Text = " Period must be greater than 0."
                dbXPeriod.Focus()
                Exit Sub
            End If

            If Not IsNumeric(dbXRange.Text) Then
                lstatus.Text = "Range Period must be numeric values."
                dbXRange.Focus()
                Exit Sub
            End If

            'If dbXRange.Text < 1 Then
            '    lstatus.Text = "  Range Period must be greater than 0."
            '    dbXRange.Focus()
            '    Exit Sub
            'End If

            SQLString = "Update MsTerm set TermName = " + QuotedStr(dbName.Text) + _
            ", XPeriod =" + dbXPeriod.Text.Replace(",", "") + ", XRange =" + dbXRange.Text.Replace(",", "") + _
            ", TypeRange =" + QuotedStr(ddlTypeRange.SelectedValue) + ", FgCBD =" + QuotedStr(ddlFgCBD.SelectedValue) + _
            " where TermCode = '" & lbCode.Text + "'"
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid()
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "DataGrid_Update Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub DataGridDt_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGridDt.RowUpdating
        Dim tbXRange, tbPercentage, tbPPN As TextBox
        Dim lbPeriod As Label
        Dim ddlTypeRange As DropDownList
        Dim ddlFgCBD As DropDownList
        Dim SQLString As String
        Dim GVR As GridViewRow
        Try
            GVR = DataGridDt.Rows(e.RowIndex)
            lbPeriod = GVR.FindControl("Period")
            ddlTypeRange = GVR.FindControl("TypeRangeEdit")
            ddlFgCBD = GVR.FindControl("FgCBDEdit")
            tbXRange = GVR.FindControl("XRangeEdit")
            tbPercentage = GVR.FindControl("PercentageEdit")
            tbPPN = GVR.FindControl("PPNEdit")

            SQLString = "UPDATE MsTermDt SET TypeRange = " + QuotedStr(ddlTypeRange.SelectedValue) + ", FgCBD = " + QuotedStr(ddlFgCBD.SelectedValue) + _
            ", XRange = " + QuotedStr(tbXRange.Text) + ", Percentage = " + QuotedStr(tbPercentage.Text) + _
            ", PPN = " + QuotedStr(tbPPN.Text) + " WHERE Period = " + QuotedStr(lbPeriod.Text) + _
            " AND TermCode = " + QuotedStr(ViewState("Nmbr"))

            SQLString = Replace(SQLString, "''", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            DataGridDt.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGridDt.EditIndex = -1
            bindDataGridDt()
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "Datagrid dt update Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGrid.RowDeleting
        Dim txtID As Label
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            txtID = DataGrid.Rows(e.RowIndex).FindControl("TermCode")

            SQLExecuteNonQuery("Delete from MsTerm where TermCode = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
            SQLExecuteNonQuery("Delete from MsTermDt where TermCode = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub DataGridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGridDt.RowDeleting
        Dim txtID As Label
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            txtID = DataGridDt.Rows(e.RowIndex).FindControl("Period")

            SQLExecuteNonQuery("Delete from MsTermDt where TermCode = " + QuotedStr(ViewState("Nmbr")) + " AND Period =" + QuotedStr(txtID.Text), ViewState("DBConnection").ToString)
            bindDataGridDt()

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
    Protected Sub DataGridDt_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGridDt.PageIndexChanging
        DataGridDt.PageIndex = e.NewPageIndex
        If DataGridDt.EditIndex <> -1 Then
            DataGridDt_RowCancelingEdit(Nothing, Nothing)
        End If
        bindDataGridDt()
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
    Protected Sub DataGridDt_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles DataGridDt.Sorting
        Try
            Session("SortExpressionDt") = e.SortExpression
            bindDataGridDt()
        Catch ex As Exception
            lstatus.Text = "UserDG_SortCommandError =" + vbCrLf + ex.ToString
        End Try
    End Sub
    Protected Sub btnBackDtTop_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBackDtTop.Click
        Try
            pnlHd.Visible = True
            pnlDt.Visible = False
        Catch ex As Exception
            lstatus.Text = "btn Back Dt Top Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        Try
            pnlHd.Visible = True
            pnlDt.Visible = False
        Catch ex As Exception
            lstatus.Text = "btn Back Dt Bottom Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnApply_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApply.Click
        Dim lbTypeRange, lbPeriod, lbFgCBD As Label
        Dim tbxrange, tbpercentage, tbppn As TextBox
        Dim SQLString As String
        Dim Percent, PPn As Double
        Dim GVR As GridViewRow
        Dim i As Integer
        Dim exe As Boolean
        Try
            exe = True
            Percent = 0
            PPn = 0
            For i = 0 To DataGridDt.Rows.Count - 1
                GVR = DataGridDt.Rows(i)
                lbPeriod = GVR.FindControl("Period")
                lbTypeRange = GVR.FindControl("TypeRange")
                lbFgCBD = GVR.FindControl("FgCBD")
                tbxrange = GVR.FindControl("XRange")
                tbpercentage = GVR.FindControl("Percentage")
                tbppn = GVR.FindControl("PPN")

                lbPeriod.Text = lbPeriod.Text.Replace(",", "")
                lbXRange.Text = lbXRange.Text.Replace(",", "")

                tbxrange.Text = tbxrange.Text.Replace(",", "")
                tbpercentage.Text = tbpercentage.Text.Replace(",", "")
                tbppn.Text = tbppn.Text.Replace(",", "")

                If tbpercentage.Text.Trim = "" Then
                    tbpercentage.Text = "0"
                End If
                If tbppn.Text.Trim = "" Then
                    tbppn.Text = "0"
                End If
                If Not IsNumeric(tbxrange.Text) Then
                    lstatus.Text = "X Range for " + lbTerm.Text + " Period " + lbPeriod.Text + " must in numeric format"
                    exe = False
                    tbxrange.Focus()
                    Exit For
                End If
                If Not IsNumeric(tbpercentage.Text) Then
                    lstatus.Text = "Percentage for " + lbTerm.Text + " Period " + lbPeriod.Text + " must in numeric format"
                    exe = False
                    tbpercentage.Focus()
                    Exit For
                End If
                If Not IsNumeric(tbppn.Text) Then
                    lstatus.Text = "PPN for " + lbTerm.Text + " Period " + lbPeriod.Text + " must in numeric format"
                    exe = False
                    tbppn.Focus()
                    Exit For
                End If
                Percent = Percent + CFloat(tbpercentage.Text)
                PPn = PPn + CFloat(tbppn.Text)
            Next
            If exe Then
                If Percent <> 100 Then
                    lstatus.Text = "Percentage (%) must equal to 100 "
                    Exit Sub
                End If
                If PPn <> 100 Then
                    lstatus.Text = "PPN (%) must equal to 100 "
                    Exit Sub
                End If
                ' simpan ke database
                For i = 0 To DataGridDt.Rows.Count - 1
                    GVR = DataGridDt.Rows(i)
                    lbPeriod = GVR.FindControl("Period")
                    lbTypeRange = GVR.FindControl("TypeRange")
                    tbxrange = GVR.FindControl("XRange")
                    tbpercentage = GVR.FindControl("Percentage")
                    tbppn = GVR.FindControl("PPN")

                    lbPeriod.Text = lbPeriod.Text.Replace(",", "")
                    lbXRange.Text = lbXRange.Text.Replace(",", "")

                    tbxrange.Text = tbxrange.Text.Replace(",", "")
                    tbpercentage.Text = tbpercentage.Text.Replace(",", "")
                    tbppn.Text = tbppn.Text.Replace(",", "")

                    SQLString = "UPDATE MsTermDt SET TypeRange = " + QuotedStr(lbTypeRange.Text) + ", XRange = " + lbXRange.Text + ", Percentage = " + tbpercentage.Text + ", PPn = " + tbppn.Text + " from MsTermDt WHERE TermCode=" + QuotedStr(lbTerm.Text) + " AND Period = " + lbPeriod.Text
                    SQLExecuteScalar(SQLString, ViewState("DBConnection").ToString)
                Next
            End If
        Catch ex As Exception
            lstatus.Text = " btn apply error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim StrFilter, SQLString As String
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("DBConnection") = ViewState("DBConnection")
            Session("PrintType") = "Print"
            SQLString = "S_FormPrintMaster4 'VMsTerm','Term_Code','Term_Name', 'CONVERT(VARCHAR(3),XPeriod)+  ''-''  + TypeRange+ ''-'' + CONVERT(VARCHAR(8),Xrange)','FgCBD', 'Term File', 'Term Code', 'Term Name', 'XPeriod - Range','FgCBD', " + QuotedStr(StrFilter) + "," + QuotedStr(ViewState("UserId"))
            Session("SelectCommand") = SQLString
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster3.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)

        Catch ex As Exception
            lstatus.Text = "btn Print Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnGet_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGet.Click
        'If DataGridDt.Rows.Count > 0 Then
        '    lstatus.Text = ConfirmDlg("test ?") 
        'End If
        'If DataGridDt.Rows.Count = 1 Then
        btnSearch_Click(Nothing, Nothing)

        lbPeriod.Text = lbPeriod.Text.Replace(",", "")
        lbXRange.Text = lbXRange.Text.Replace(",", "")
        SQLExecuteNonQuery("EXEC S_MsTermCreateDt " + QuotedStr(lbTerm.Text) + ", " + lbPeriod.Text + ", " + lbXRange.Text + ", " + QuotedStr(lbTypeRange.Text), ViewState("DBConnection").ToString)
        'lstatus.Text = "EXEC S_MsTermCreateDt " + QuotedStr(lbTerm.Text) + ", " + lbPeriod.Text + ", " + lbXRange.Text + ", " + QuotedStr(lbTypeRange.Text)
        'Exit Sub

        bindDataGridDt()
        'End If

        'Dim drResult As DataRow
        'If IsNothing(Session("Result")) Then
        '    lstatus.Text = "Session is empty"
        '    Exit Sub
        'End If
        'For Each drResult In Session("Result").Rows
        '    'insert
        '    Dim dr As DataRow
        '    dr = ViewState("Dt").NewRow
        '    dr("Period") = drResult("Period")
        '    dr("TyepRange") = drResult("Typerange")
        '    dr("UnitOrder") = ""
        '    dr("Unit") = ""
        '    dr("PriceForex") = 0
        '    dr("AmountForex") = 0
        '    dr("QtyOrder") = 0
        '    dr("Qty") = 0
        '    ViewState("Dt").Rows.Add(dr)
        'Next
        'bindDataGridDt()
        '' EnableHd(GetCountRecord(ViewState("Dt")) = 0)

        'Session("ResultSame") = Nothing
        'Dim DsCreate As DataSet
        'Dim DrCreate As DataRow
        'Dim tbXRange, tbPercentage, tbPPN As TextBox
        'Dim ddlTypeRange As DropDownList
        'Dim lbPeriod, lbXRange, lbPercentage, lbPPN, lbTypeRange As Label
        'Dim SQLString As String
        'Dim GVR As GridViewRow
        'Try
        '    If DataGridDt.Rows.Count <> 0 Then
        '        lstatus.Text = "Data Exist, can''t get data"
        '    End If
        '    ' If CheckMenuLevel("Insert") = False Then
        '    GVR = DataGrid.FooterRow
        '    lbTerm = DataGrid.FindControl("TermCode")
        '    lbTypeRange = DataGrid.FindControl("TypeRange")
        '    lbPeriod = DataGrid.FindControl("Period")
        '    tbXRange = DataGrid.FindControl("XRange")
        '    'tbPercentage = DataGridDt.FindControl("Percentage")
        '    'tbPPN = DataGridDt.FindControl("PPN")

        '    'If lbPeriod.Text.Trim = "" Then
        '    '    lstatus.Text = "<script language='javascript'>alert('Period must be filled');</script>"
        '    '    lbPeriod.Focus()
        '    '    Exit Sub
        '    'End If

        '    DsCreate = SQLExecuteQuery("EXEC S_MsTermCreateDt '" + lbTerm.Text + "', '" + lbPeriod.Text + "', '" + tbXRange.Text + "', '" + lbTypeRange.Text + "'")


    End Sub

End Class
