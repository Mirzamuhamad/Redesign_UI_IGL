Imports System.Data

Partial Class Master_MsShift_MsShift
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        If Not IsPostBack Then
            InitProperty()
            FillCombo(ddlDay, "EXEC S_GetDayinEnglish", False, "DayCode", "DayName", ViewState("DBConnection").ToString)
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            DataGridDt.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGridDt.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
        End If
        dsShiftGroup.ConnectionString = ViewState("DBConnection")
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
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            SqlString = "Select A.*, B.ShiftGrpName from MsShift A INNER JOIN MsShiftGroup B ON A.ShiftGroup = B.ShiftGrpCode " + StrFilter
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "ShiftCode ASC"
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
            tempDS = SQLExecuteQuery("SELECT A.*, C.ShiftName FROM MsShiftDt A LEFT OUTER JOIN MsShift C ON A.ShiftCode = C.ShiftCode WHERE A.ShiftCode =" + QuotedStr(ViewState("Nmbr")), ViewState("DBConnection").ToString)

            DV = tempDS.Tables(0).DefaultView

            If DV.Count = 0 Then
                Dim DT As DataTable = tempDS.Tables(0)
                ShowGridViewIfEmpty(DT, DataGridDt)
                DataGridDt.Columns(0).Visible = False
                DV = DT.DefaultView
            Else
                DV.Sort = ViewState("SortExpressionDt")
                DataGridDt.DataSource = DV
                DataGridDt.DataBind()
                DataGridDt.Columns(0).Visible = True
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Public Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        Dim txt As TextBox
        Dim ddlShiftGroup, ddlFgActive, ddlFgOff As DropDownList

        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()
            obj = DataGrid.Rows(e.NewEditIndex)
            txt = obj.FindControl("ShiftNameEdit")
            ddlShiftGroup = obj.FindControl("ShiftGroupEdit")
            ddlFgOff = obj.FindControl("FgOffEdit")
            ddlFgActive = obj.FindControl("FgActiveEdit")

            'If ddlShiftGroup.SelectedIndex = 4 Then
            '    ddlFgActive.SelectedIndex = 1
            '    ddlFgActive.Enabled = False
            'Else
            '    ddlFgActive.SelectedIndex = 0
            '    ddlFgActive.Enabled = True
            'End If

            txt.Focus()

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
        Dim ddlGroup, ddlFgActive, ddlFgOff As DropDownList
        Dim GVR As GridViewRow
        Try
            If e.CommandName = "Insert" Then
                GVR = DataGrid.FooterRow
                dbCode = GVR.FindControl("ShiftCodeAdd")
                dbName = GVR.FindControl("ShiftNameAdd")
                ddlGroup = GVR.FindControl("ShiftGroupAdd")
                ddlFgOff = GVR.FindControl("FgOffAdd")
                ddlFgActive = GVR.FindControl("FgActiveAdd")

                If dbCode.Text.Trim.Length = 0 Then
                    lstatus.Text = "<script language='javascript'>alert('Shift Code must be filled');</script>"
                    dbCode.Focus()
                    Exit Sub
                End If
                If dbName.Text.Trim.Length = 0 Then
                    lstatus.Text = "<script language='javascript'>alert('Shift Name must be filled');</script>"
                    dbName.Focus()
                    Exit Sub
                End If

                If SQLExecuteScalar("SELECT Shift_Code From V_MsShift WHERE Shift_Code = " + QuotedStr(dbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Shift " + QuotedStr(dbCode.Text) + " has already been exist"
                    Exit Sub
                End If

                'insert the new entry
                SQLString = "Insert into MsShift (ShiftCode, ShiftName, ShiftGroup, FgOff, FgActive, UserID, UserDate) " + _
                "SELECT " + QuotedStr(dbCode.Text) + "," + QuotedStr(dbName.Text) + ", " + QuotedStr(ddlGroup.SelectedValue) + "," + QuotedStr(ddlFgOff.SelectedValue) + "," + QuotedStr(ddlFgActive.SelectedValue) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", GetDate()"
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                bindDataGrid()
            ElseIf e.CommandName = "View" Then
                Dim lbCode, lbName As Label
                GVR = DataGrid.Rows(CInt(e.CommandArgument))
                lbCode = GVR.FindControl("ShiftCode")
                lbName = GVR.FindControl("ShiftName")
                ViewState("Nmbr") = lbCode.Text
                lbShiftCode.Text = lbCode.Text + " - " + lbName.Text
                pnlHd.Visible = False
                pnlDt.Visible = True
                bindDataGridDt()
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub
    Public Sub DataGridDt_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGridDt.RowCommand
        Dim DDL As DropDownList
        Dim GVR As GridViewRow = Nothing
        Dim index As Integer
        Try
            If Not (e.CommandName = "Sort" Or e.CommandName = "Page") Then
                index = Convert.ToInt32(e.CommandArgument)
                GVR = DataGridDt.Rows(index)
            End If
            If e.CommandName = "Go" Then
                DDL = DataGridDt.Rows(index).FindControl("ddl")
                If DDL.SelectedValue = "Edit" Then
                    ViewState("State") = "Edit"
                    FillTextBoxDt(ViewState("Nmbr"), GVR.Cells(1).Text)
                    ModifyInputDt(True)
                    ddlDay.Enabled = False
                    btnSave.Visible = True
                    btnReset.Visible = True
                    pnlDt.Visible = False
                    pnlInputDt.Visible = True
                ElseIf DDL.SelectedValue = "Copy New" Then
                    ViewState("State") = "Insert"
                    FillTextBoxDt(ViewState("Nmbr"), GVR.Cells(1).Text)
                    ModifyInputDt(True)
                    ddlDay.Enabled = True
                    ddlDay.SelectedIndex = CInt(GVR.Cells(1).Text)
                    btnSave.Visible = True
                    btnReset.Visible = True
                    pnlDt.Visible = False
                    pnlInputDt.Visible = True
                ElseIf DDL.SelectedValue = "Delete" Then
                    Try
                        If CheckMenuLevel("Delete") = False Then
                            Exit Sub
                        End If

                        SQLExecuteNonQuery("Delete from MsShiftDt where ShiftCode = " + QuotedStr(ViewState("Nmbr")) + " AND Day =" + QuotedStr(GVR.Cells(1).Text), ViewState("DBConnection").ToString)
                        bindDataGridDt()

                    Catch ex As Exception
                        lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
                    End Try
                End If

            End If


        Catch ex As Exception
            lstatus.Text = "Item Command Dt Error" + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim dbName As TextBox
        Dim lbCode As Label
        Dim ddlShiftGroup, ddlFgActive, ddlFgOff As DropDownList
        Dim GVR As GridViewRow
        Try
            GVR = DataGrid.Rows(e.RowIndex)
            lbCode = GVR.FindControl("ShiftCodeEdit")
            dbName = GVR.FindControl("ShiftNameEdit")
            ddlShiftGroup = GVR.FindControl("ShiftGroupEdit")
            ddlFgOff = GVR.FindControl("FgOffEdit")
            ddlFgActive = GVR.FindControl("FgActiveEdit")

            If dbName.Text.Trim.Length = 0 Then
                lstatus.Text = "<script language='javascript'>alert('Shift Name must be filled.');</script>"
                dbName.Focus()
                Exit Sub
            End If

            SQLString = "Update MsShift set ShiftName = " + QuotedStr(dbName.Text) + _
            ", ShiftGroup =" + QuotedStr(ddlShiftGroup.SelectedValue) + ", FgOff =" + QuotedStr(ddlFgOff.SelectedValue) + ", FgActive =" + QuotedStr(ddlFgActive.SelectedValue) + _
            "  where ShiftCode = '" & lbCode.Text + "'"

            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = lstatus.Text + "DataGrid_Update Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub DataGridDt_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGridDt.RowUpdating
        '    Dim tbAccInvent, tbAccCogs, tbAccsj, tbAccWrhs, tbAccTransitPRetur, tbAccTransitSRetur, tbAccPReturn, tbAccSales, tbAccSReturn, tbAccSTCAdjust, tbAccSTCLost, tbAccSampleExps As TextBox
        '    Dim lbWrhsType As Label
        '    Dim SQLString As String
        '    Dim GVR As GridViewRow
        '    Try
        '        GVR = DataGridDt.Rows(e.RowIndex)
        '        lbWrhsType = GVR.FindControl("WrhsTypeEdit")
        '        tbAccInvent = GVR.FindControl("AccInventEdit")
        '        tbAccCogs = GVR.FindControl("AccCogsEdit")
        '        tbAccsj = GVR.FindControl("AccsjEdit")
        '        tbAccSales = GVR.FindControl("AccSalesEdit")
        '        tbAccWrhs = GVR.FindControl("AccWrhsEdit")
        '        tbAccTransitPRetur = GVR.FindControl("AccTransitPReturEdit")
        '        tbAccTransitSRetur = GVR.FindControl("AccTransitSReturEdit")
        '        tbAccPReturn = GVR.FindControl("AccPReturnEdit")
        '        tbAccSReturn = GVR.FindControl("AccSReturnEdit")
        '        tbAccSTCAdjust = GVR.FindControl("AccSTCAdjustEdit")
        '        tbAccSTCLost = GVR.FindControl("AccSTCLostEdit")
        '        tbAccSampleExps = GVR.FindControl("AccSampleExpsEdit")

        '        SQLString = "UPDATE MsShiftDt SET ACCInvent = " + QuotedStr(tbAccInvent.Text) + _
        '        ", AccCogs= " + QuotedStr(tbAccCogs.Text) + ", AccTransitSj= " + QuotedStr(tbAccsj.Text) + _
        '        ", AccTransitWrhs= " + QuotedStr(tbAccWrhs.Text) + _
        '        ", AccTransitPRetur= " + QuotedStr(tbAccTransitPRetur.Text) + ", AccTransitSRetur= " + QuotedStr(tbAccTransitSRetur.Text) + _
        '        ", AccSales= " + QuotedStr(tbAccSales.Text) + _
        '        ", AccPReturn= " + QuotedStr(tbAccPReturn.Text) + _
        '        ", AccSReturn = " + QuotedStr(tbAccSReturn.Text) + _
        '        ", AccSTCAdjust = " + QuotedStr(tbAccSTCAdjust.Text) + _
        '        ", AccSTCLost = " + QuotedStr(tbAccSTCLost.Text) + _
        '        ", AccSampleExps = " + QuotedStr(tbAccSampleExps.Text) + _
        '        " WHERE WrhsType = " + QuotedStr(lbWrhsType.Text) + _
        '        " AND ShiftCode =" + QuotedStr(ViewState("Nmbr"))

        '        SQLString = Replace(SQLString, "''", "NULL")
        '        SQLExecuteNonQuery(SQLString)

        '        DataGridDt.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
        '        DataGridDt.EditIndex = -1
        '        bindDataGridDt()
        '    Catch ex As Exception
        '        lstatus.Text = lstatus.Text + "Datagrid dt update Error : " + ex.ToString
        '    End Try
    End Sub

    Protected Sub DataGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGrid.RowDeleting
        Dim txtID As Label
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            txtID = DataGrid.Rows(e.RowIndex).FindControl("ShiftCode")

            SQLExecuteNonQuery("Delete from MsShiftDt where ShiftCode = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
            SQLExecuteNonQuery("Delete from MsShift where ShiftCode = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub DataGridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGridDt.RowDeleting
        'Dim txtID As Label
        'Try
        '    If CheckMenuLevel("Delete") = False Then
        '        Exit Sub
        '    End If
        '    txtID = DataGridDt.Rows(e.RowIndex).FindControl("WrhsType")

        '    SQLExecuteNonQuery("Delete from MsShiftDt where ShiftCode = " + QuotedStr(ViewState("Nmbr")) + " AND WrhsType =" + QuotedStr(txtID.Text))
        '    bindDataGridDt()

        'Catch ex As Exception
        '    lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        'End Try
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
            lstatus.Text = "DataGrid_Sorting =" + vbCrLf + ex.ToString
        End Try
    End Sub
    Protected Sub DataGridDt_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles DataGridDt.Sorting
        Try
            ViewState("SortExpressionDt") = e.SortExpression
            bindDataGridDt()
        Catch ex As Exception
            lstatus.Text = "DataGridDt_Sorting =" + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub btnBackDtTop_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBackDtTop.Click, btnBack2.Click
        Try
            pnlHd.Visible = True
            pnlDt.Visible = False
        Catch ex As Exception
            lstatus.Text = "btn Back Dt Top Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            'lstatus.Text = StrFilter
            'Exit Sub
            Session("DBConnection") = ViewState("DBConnection")
            Session("PrintType") = "Print"
            Session("SelectCommand") = "EXEC S_FormPrintMaster4 'MsShift','ShiftCode', 'ShiftName','ShiftGroup','FgActive','Shift File','Shift Code','Shift Name','Shift Group','Active'," + QuotedStr(StrFilter) + "," + QuotedStr(ViewState("UserId"))
            Session("ReportFile") = Server.MapPath("~\Rpt\RptPrintMaster3.frx")
            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            lstatus.Text = "Btn Print Error : " + ex.ToString
        End Try

    End Sub
    Protected Sub FillTextBoxDt(ByVal Nmbr As String, ByVal Code As String)
        Try
            Dim dr As DataRow
            dr = SQLExecuteQuery("SELECT A.*, C.ShiftName FROM MsShiftDt A " + _
            "LEFT OUTER JOIN MsShift C ON A.ShiftCode = C.ShiftCode " + _
            "WHERE A.ShiftCode =" + QuotedStr(Nmbr) + " AND Day = " + QuotedStr(Code), ViewState("DBConnection").ToString).Tables(0).Rows(0)

            ClearDt()

            BindToText(tbShiftIn, dr("ShiftIn").ToString)
            BindToText(tbShiftOut, dr("ShiftOut").ToString)
            BindToText(tbIdleIn1, dr("IdleIn1").ToString)
            BindToText(tbIdleOut1, dr("IdleOut1").ToString)
            BindToText(tbIdleIn2, dr("IdleIn2").ToString)
            BindToText(tbIdleOut2, dr("IdleOut2").ToString)
            BindToText(tbIdleIn3, dr("IdleIn3").ToString)
            BindToText(tbIdleOut3, dr("IdleOut3").ToString)
            BindToDropList(ddlDay, dr("Day").ToString)
            ddlDay.Focus()

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt.Click, btnAddDt2.Click
        ViewState("State") = "Insert"
        ClearDt()
        ModifyInputDt(True)
        pnlDt.Visible = False
        pnlInputDt.Visible = True
        btnSave.Visible = True
        btnReset.Visible = True
        ddlDay.Focus()
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ClearDt()
        pnlDt.Visible = True
        pnlInputDt.Visible = False
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        ClearDt()
    End Sub
    Private Sub ClearDt()
        Try

            ddlDay.SelectedIndex = 0
            tbShiftIn.Text = "00:00"
            tbShiftOut.Text = "00:00"
            tbIdleIn1.Text = "00:00"
            tbIdleOut1.Text = "00:00"
            tbIdleIn2.Text = "00:00"
            tbIdleOut2.Text = "00:00"
            tbIdleIn3.Text = "00:00"
            tbIdleOut3.Text = "00:00"

        Catch ex As Exception
            lstatus.Text = "ClearDt Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim SQLString As String = ""
        Try
            If tbShiftIn.Text.Trim = "" Then
                lstatus.Text = "<script language='javascript'>alert('Shift In must be filled');</script>"
                tbShiftIn.Focus()
                Exit Sub
            End If
            If tbShiftOut.Text.Trim = "" Then
                lstatus.Text = "<script language='javascript'>alert('Shift Out must be filled');</script>"
                tbShiftOut.Focus()
                Exit Sub
            End If
            If tbIdleIn1.Text.Trim = "" Then
                lstatus.Text = "<script language='javascript'>alert('Idle In 1 must be filled');</script>"
                tbIdleIn1.Focus()
                Exit Sub
            End If
            If tbIdleOut1.Text.Trim = "" Then
                lstatus.Text = "<script language='javascript'>alert('Idle Out 1 must be filled');</script>"
                tbIdleOut1.Focus()
                Exit Sub
            End If
            If tbIdleIn2.Text.Trim = "" Then
                lstatus.Text = "<script language='javascript'>alert('Idle In 2 must be filled');</script>"
                tbIdleIn2.Focus()
                Exit Sub
            End If
            If tbIdleOut2.Text.Trim = "" Then
                lstatus.Text = "<script language='javascript'>alert('Idle Out 2 must be filled');</script>"
                tbIdleOut2.Focus()
                Exit Sub
            End If
            If tbIdleIn3.Text.Trim = "" Then
                lstatus.Text = "<script language='javascript'>alert('Idle In 3 must be filled');</script>"
                tbIdleIn3.Focus()
                Exit Sub
            End If
            If tbIdleOut3.Text.Trim = "" Then
                lstatus.Text = "<script language='javascript'>alert('Idle Out 3 must be filled');</script>"
                tbIdleOut3.Focus()
                Exit Sub
            End If


            If ViewState("State") = "Insert" Then
                If SQLExecuteScalar("SELECT Day From MsShiftDt WHERE ShiftCode = " + QuotedStr(ViewState("Nmbr")) + " AND Day =" + QuotedStr(ddlDay.SelectedValue), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Shift " + QuotedStr(ViewState("Nmbr")) + " Day " + QuotedStr(ddlDay.SelectedItem.ToString) + " has already been exist"
                    Exit Sub
                End If

                SQLString = "Insert Into MsShiftDt (ShiftCode, Day, ShiftIn, ShiftOut, " + _
                "IdleIn1, IdleOut1, IdleIn2, IdleOut2, IdleIn3, IdleOut3, UserId, UserDate) " + _
                "SELECT " + QuotedStr(ViewState("Nmbr")) + "," + QuotedStr(ddlDay.SelectedValue) + "," + _
                QuotedStr(tbShiftIn.Text) + "," + QuotedStr(tbShiftOut.Text) + "," + _
                QuotedStr(tbIdleIn1.Text) + "," + QuotedStr(tbIdleOut1.Text) + "," + _
                QuotedStr(tbIdleIn2.Text) + "," + QuotedStr(tbIdleOut2.Text) + "," + _
                QuotedStr(tbIdleIn3.Text) + "," + QuotedStr(tbIdleOut3.Text) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", GetDate()"

            ElseIf ViewState("State") = "Edit" Then
                SQLString = "UPDATE MsShiftDt SET ShiftIn = " + QuotedStr(tbShiftIn.Text) + ", ShiftOut= " + QuotedStr(tbShiftOut.Text) + _
                ", IdleIn1= " + QuotedStr(tbIdleIn1.Text) + ", IdleOut1= " + QuotedStr(tbIdleOut1.Text) + _
                ", IdleIn2= " + QuotedStr(tbIdleIn2.Text) + ", IdleOut2= " + QuotedStr(tbIdleOut2.Text) + _
                ", IdleIn3= " + QuotedStr(tbIdleIn3.Text) + ", IdleOut3= " + QuotedStr(tbIdleOut3.Text) + _
                " WHERE Day = " + QuotedStr(ddlDay.Text) + _
                " AND ShiftCode =" + QuotedStr(ViewState("Nmbr"))
            End If

            SQLString = Replace(SQLString, "''", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            bindDataGridDt()
            pnlInputDt.Visible = False
            pnlDt.Visible = True
        Catch ex As Exception
            lstatus.Text = "btn Save Error : " + ex.ToString
        End Try
    End Sub
    Private Sub ModifyInputDt(ByVal State As Boolean)
        ddlDay.Enabled = State
        tbShiftIn.Enabled = State
        tbShiftOut.Enabled = State
        tbIdleIn1.Enabled = State
        tbIdleOut1.Enabled = State
        tbIdleIn2.Enabled = State
        tbIdleOut2.Enabled = State
        tbIdleIn3.Enabled = State
        tbIdleOut3.Enabled = State        
    End Sub

End Class

