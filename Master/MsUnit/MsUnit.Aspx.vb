Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data

Partial Class Master_MsUnit_MsUnit
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
            DataGridDt.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGridDt.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            'bindDataGrid()
        End If
        If Not Session("Result") Is Nothing Then
            'Dim Acc As New TextBox
            'Dim AccName As New Label
            'Dim Count As Integer
            'Dim dgi As GridViewRow


            'Acc.Text = Session("Result")(0).ToString
            'AccName.Text = Session("Result")(1).ToString
            'Acc.Focus()

            'Session("Result") = Nothing
            'ViewState("Sender") = Nothing
            'Session("filter") = Nothing
            'Session("Criteria") = Nothing
            'Session("Column") = Nothing
        End If
        dsUnitConvert.ConnectionString = ViewState("DBConnection")
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
                    lstatus.Text = MessageDlg("You are not authorized to edit record. Please contact administrator")
                    Return False
                    Exit Function
                End If
            End If
            If CommandName = "Delete" Then
                If ViewState("MenuLevel").Rows(0)("FgDelete") = "N" Then
                    lstatus.Text = MessageDlg("You are not authorized to delete record. Please contact administrator")
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
        Dim tempDS As New DataSet()
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            SqlString = "Select * from MsUnit " + StrFilter + " Order By UnitName "
            tempDS = SQLExecuteQuery(SqlString, ViewState("DBConnection").ToString)
            ViewState("Hd") = tempDS.Tables(0)
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "UnitCode ASC"
                ViewState("SortOrder") = "ASC"
            End If
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub
    Private Sub bindDataGridDt()
        Dim tempDS As New DataSet()
        Dim DV As DataView
        Try
            tempDS = SQLExecuteQuery("Select UnitCode, Operator, dbo.FORMATFloat(Rate, dbo.DigitQty()) AS Rate, UnitConvert FROM MsConvertion WHERE UnitCode =" + QuotedStr(ViewState("Nmbr")), ViewState("DBConnection").ToString)
            'lstatus.Text = "SOO2"
            'Exit Sub
            ViewState("Dt") = tempDS.Tables(0)
            
            DV = tempDS.Tables(0).DefaultView
            
            If DV.Count = 0 Then
                Dim DT As DataTable = tempDS.Tables(0)
                ShowGridViewIfEmpty(DT, DataGridDt)
                DV = DT.DefaultView
            Else
                DV.Sort = ViewState("SortExpressionDt")
                DataGridDt.DataSource = DV
                DataGridDt.DataBind()
            End If
            
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Public Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim tbName As TextBox
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()
            tbName = DataGrid.Rows(e.NewEditIndex).FindControl("UnitNameEdit")
            tbName.Focus()
        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub
    Public Sub DataGridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGridDt.RowEditing
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
        Dim dbCode, dbName As TextBox
        Dim GVR As GridViewRow
        Try
            If e.CommandName = "Insert" Then
                GVR = DataGrid.FooterRow
                dbCode = GVR.FindControl("UnitCodeAdd")
                dbName = GVR.FindControl("UnitNameAdd")

                If CekExistData(ViewState("Hd"), "UnitCode", dbCode.Text) Then
                    lstatus.Text = "Unit " + dbCode.Text + " has been already exist"
                    Exit Sub
                End If

                If dbCode.Text.Trim.Length = 0 Then
                    lstatus.Text = MessageDlg("Unit Code must be filled")
                    dbCode.Focus()
                    Exit Sub
                End If
                If dbName.Text.Trim.Length = 0 Then
                    lstatus.Text = MessageDlg("Unit Name must be filled")
                    dbName.Focus()
                    Exit Sub
                End If

                If SQLExecuteScalar("SELECT Unit_Code From VMsUnit WHERE Unit_Code = " + QuotedStr(dbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Unit " + QuotedStr(dbCode.Text) + " has already been exist"
                    Exit Sub
                End If

                'insert the new entry
                SQLString = "Insert into MsUnit (UnitCode, UnitName, UserID, UserDate) " + _
                "SELECT " + QuotedStr(dbCode.Text) + "," + QuotedStr(dbName.Text) + ", " + _
                QuotedStr(ViewState("UserId").ToString) + ", getDate()"
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                bindDataGrid()
            ElseIf e.CommandName = "View" Then
                Dim lbCode, lbName As Label
                GVR = DataGrid.Rows(CInt(e.CommandArgument))
                lbCode = GVR.FindControl("UnitCode")
                lbName = GVR.FindControl("UnitName")
                ViewState("Nmbr") = lbCode.Text
                lbUnitCode.Text = lbCode.Text + " - " + lbName.Text
                pnlHd.Visible = False
                pnlDt.Visible = True
                bindDataGridDt()
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub
    Public Sub DataGridDt_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGridDt.RowCommand
        Dim tbRate As TextBox
        Dim ddlUnitConvert, ddlOperator As DropDownList
        Dim SQLString As String
        Dim GVR As GridViewRow
        Try
            If e.CommandName = "Insert" Then
                GVR = DataGridDt.FooterRow
                tbRate = GVR.FindControl("RateAdd")
                ddlUnitConvert = GVR.FindControl("UnitConvertAdd")
                ddlOperator = GVR.FindControl("OperatorAdd")

                If CekExistData(ViewState("Dt"), "UnitConvert", ddlUnitConvert.SelectedValue) Then
                    lstatus.Text = "Unit " + QuotedStr(ViewState("Nmbr")) + " Unit Convert " + QuotedStr(ddlUnitConvert.SelectedValue) + " has been already exist"
                    Exit Sub
                End If

                If tbRate.Text.Trim = "" Then
                    lstatus.Text = "<script language='javascript'>alert('Rate must be filled');</script>"
                    tbRate.Focus()
                    Exit Sub
                End If

                If IsNumeric(tbRate.Text.Replace(",", "")) = 0 Then
                    lstatus.Text = MessageDlg("Expense Rate must be in numeric.")
                    tbRate.Focus()
                    Exit Sub
                End If

                If ViewState("Nmbr") = ddlUnitConvert.Text Then
                    lstatus.Text = "<script language='javascript'>alert('Unit Convert must be different from Unit Code');</script>"
                    ddlUnitConvert.Focus()
                    Exit Sub
                End If

                If SQLExecuteScalar("SELECT UnitCode From VMsConvertion WHERE UnitCode =" + QuotedStr(ViewState("Nmbr")) + " AND UnitConvert =" + QuotedStr(ddlUnitConvert.SelectedValue), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "UnitCode " + QuotedStr(ViewState("Nmbr")) + " Unit Convert " + QuotedStr(ddlUnitConvert.SelectedValue) + " has already been exist"
                    Exit Sub
                End If

                SQLString = "Insert Into MsConvertion (UnitCode, UnitConvert, Operator, Rate, UserId, UserDate) " + _
                "SELECT " + QuotedStr(ViewState("Nmbr")) + "," + QuotedStr(ddlUnitConvert.SelectedValue) + "," + _
                QuotedStr(ddlOperator.SelectedValue) + "," + tbRate.Text.Replace(",", "") + "," + QuotedStr(ViewState("UserId")) + ", getDate()"

                SQLString = Replace(SQLString, "''", "NULL")
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

                If ddlOperator.Text = "/" Then
                    SQLString = "Insert Into MsConvertion (UnitConvert, UnitCode, Operator, Rate, UserId, UserDate) " + _
                    "SELECT " + QuotedStr(ViewState("Nmbr")) + "," + QuotedStr(ddlUnitConvert.SelectedValue) + "," + _
                    QuotedStr("X") + "," + tbRate.Text.Replace(",", "") + "," + QuotedStr(ViewState("UserId")) + ", getDate()"

                    SQLString = Replace(SQLString, "''", "NULL")
                    SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                Else
                    SQLString = "Insert Into MsConvertion (UnitConvert, UnitCode, Operator, Rate, UserId, UserDate) " + _
                    "SELECT " + QuotedStr(ViewState("Nmbr")) + "," + QuotedStr(ddlUnitConvert.SelectedValue) + "," + _
                    QuotedStr("/") + "," + tbRate.Text.Replace(",", "") + "," + QuotedStr(ViewState("UserId")) + ", getDate()"

                    SQLString = Replace(SQLString, "''", "NULL")
                    SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                End If

                bindDataGridDt()
            End If
        Catch ex As Exception
            lstatus.Text = "Item Command Dt Error" + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim dbName As TextBox
        Dim lbCode As Label
        Dim GVR As GridViewRow
        Try
            GVR = DataGrid.Rows(e.RowIndex)
            lbCode = GVR.FindControl("UnitCodeEdit")
            dbName = GVR.FindControl("UnitNameEdit")
            

            If dbName.Text.Trim.Length = 0 Then
                lstatus.Text = "<script language='javascript'>alert('Unit Name must be filled.');</script>"
                dbName.Focus()
                Exit Sub
            End If

            SQLString = "Update MSUnit set UnitName = " + QuotedStr(dbName.Text) + _
            " where UnitCode = '" & lbCode.Text + "'"

            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = lstatus.Text + "DataGrid_Update Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub DataGridDt_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGridDt.RowUpdating
        Dim lbCode As Label
        Dim tbRate As TextBox
        Dim ddlOperator As DropDownList
        Dim SQLString As String
        Dim GVR As GridViewRow
        Try
            GVR = DataGridDt.Rows(e.RowIndex)
            lbCode = GVR.FindControl("UnitConvertEdit")
            tbRate = GVR.FindControl("RateEdit")
            ddlOperator = GVR.FindControl("OperatorEdit")

            If IsNumeric(tbRate.Text.Replace(",", "")) = 0 Then
                lstatus.Text = MessageDlg("Expense Rate must be in numeric.")
                tbRate.Focus()
                Exit Sub
            End If

            SQLString = "UPDATE MsConvertion SET Rate =" + tbRate.Text.Replace(",", "") + ", Operator =" + QuotedStr(ddlOperator.SelectedValue) + _
            " WHERE UnitCode =" + QuotedStr(ViewState("Nmbr")) + " AND UnitConvert =" + QuotedStr(lbCode.Text)

            SQLString = Replace(SQLString, "''", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            If ddlOperator.Text = "/" Then
                SQLString = "UPDATE MsConvertion SET Rate =" + tbRate.Text.Replace(",", "") + ", Operator =" + QuotedStr("X") + _
                " WHERE UnitCode =" + QuotedStr(lbCode.Text) + " AND UnitConvert =" + QuotedStr(ViewState("Nmbr"))

                SQLString = Replace(SQLString, "''", "NULL")
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            Else
                SQLString = "UPDATE MsConvertion SET Rate =" + tbRate.Text.Replace(",", "") + ", Operator =" + QuotedStr("/") + _
                " WHERE UnitCode =" + QuotedStr(lbCode.Text) + " AND UnitConvert =" + QuotedStr(ViewState("Nmbr"))

                SQLString = Replace(SQLString, "''", "NULL")
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            End If

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
            txtID = DataGrid.Rows(e.RowIndex).FindControl("UnitCode")

            SQLExecuteNonQuery("Delete from MsConvertion where UnitCode = '" & txtID.Text & "' ", ViewState("DBConnection").ToString)
            SQLExecuteNonQuery("Delete from MsConvertion where UnitConvert = '" & txtID.Text & "' ", ViewState("DBConnection").ToString)
            SQLExecuteNonQuery("Delete from MSUnit where UnitCode = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
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
            txtID = DataGridDt.Rows(e.RowIndex).FindControl("UnitConvert")

            SQLExecuteNonQuery("Delete from MsConvertion where UnitCode = " + QuotedStr(ViewState("Nmbr")) + " AND UnitConvert =" + QuotedStr(txtID.Text), ViewState("DBConnection").ToString)
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
            ViewState("SortExpressionDt") = e.SortExpression
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

    Protected Sub UnitConvertAdd_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Count As Integer
        Dim dgi As GridViewRow
        Try
            Count = DataGridDt.Controls(0).Controls.Count
            'dgi = DataGridDt.Controls(0).Controls(Count - 2) '-1 for allowpaging = False
            dgi = DataGridDt.FooterRow

        Catch ex As Exception
            lstatus.Text = "UnitConvert Add Index Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("PrintType") = "Print"
            Session("DBConnection") = ViewState("DBConnection")
            Session("SelectCommand") = "S_FormPrintMaster 'MsUnit','UnitCode','UnitName','Unit File','Unit Code','Unit Name'," + QuotedStr(StrFilter) + "," + QuotedStr(ViewState("UserId"))
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)
            lstatus.Text = "<script language='javascript'> { window.open(""../../Rpt/PrintForm.Aspx"", ""List"",""scrollbars=yes, resizable=yes,width=600,height=500"");}</script>"
        Catch ex As Exception
            lstatus.Text = "btn Print Error : " + ex.ToString
        End Try

    End Sub

    Protected Sub Page_SaveStateComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.SaveStateComplete

    End Sub
End Class
