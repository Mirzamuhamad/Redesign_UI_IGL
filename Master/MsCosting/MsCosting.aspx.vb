Imports System.Data

Partial Class Master_MsCosting_MsCosting
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            InitProperty()
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            'DataGridDt.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            'btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
        End If

        dsMaterialType.ConnectionString = ViewState("DBConnection")
        dsAssigned.ConnectionString = ViewState("DBConnection")
        dsAvailable.ConnectionString = ViewState("DBConnection")

        If Not Session("Result") Is Nothing Then
            'If ViewState("Sender") = "btnSearch" Then
            '    tbCode.Text = Session("Result")(0).ToString
            '    tbName.Text = Session("Result")(1).ToString
            '    bindDataGrid()
            'End If

            Session("Result") = Nothing
            ViewState("Sender") = Nothing
            Session("filter") = Nothing
            Session("Column") = Nothing
        End If
        lbstatus.Text = ""
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
                    lbstatus.Text = "<script language='javascript'> {alert('You are not authorized to edit record. Please contact administrator')}</script>"
                    Return False
                    Exit Function
                End If
            End If
            If CommandName = "Delete" Then
                If ViewState("MenuLevel").Rows(0)("FgDelete") = "N" Then
                    lbstatus.Text = "<script language='javascript'> {alert('You are not authorized to delete record. Please contact administrator')}</script>"
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
            lbstatus.Text = "Btn Search Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Private Sub bindDataGrid()
        Dim StrFilter, SqlString As String
        'Dim GVR As GridViewRow
        'Dim Qty, XFreqService As TextBox

        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            SqlString = "Select CostingCode, CostingName, CostingType from MsCosting " + StrFilter

            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "CostingCode ASC"
            End If
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection"))

            'GVR = DataGrid.FooterRow
            'Qty = GVR.FindControl("QuantityAdd")
            'Qty.Attributes.Add("OnKeyDown", "return PressNumeric();")

            'XFreqService = GVR.FindControl("XFreqServiceAdd")
            'XFreqService.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Catch ex As Exception
            lbstatus.Text = lbstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Public Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        'Dim Qty, XFreqService As TextBox
        Dim txt As Label

        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If

            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()

            obj = DataGrid.Rows(e.NewEditIndex)
            txt = obj.FindControl("CostingCodeTemp")
            txt.Focus()

            'Qty = DataGrid.Rows(e.NewEditIndex).FindControl("QuantityEdit")
            'Qty.Attributes.Add("OnKeyDown", "return PressNumeric();")

            'XFreqService = DataGrid.Rows(e.NewEditIndex).FindControl("XFreqServiceEdit")
            'XFreqService.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Catch ex As Exception
            lbstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub

    Private Sub DataGrid_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles DataGrid.RowCancelingEdit
        Try
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid()
        Catch ex As Exception
            lbstatus.Text = "DataGrid_Cancel Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGrid.RowCommand
        'Dim GVR As GridViewRow = Nothing
        Dim costCode, costName As TextBox
        Dim costType As DropDownList
        Dim SQLString As String
        Dim lbCode, lbName As Label
        'Dim DDL As DropDownList
        'Dim index As Integer

        Try
            'If Not (e.CommandName = "Sort" Or e.CommandName = "Page") Then
            '    index = Convert.ToInt32(e.CommandArgument)
            '    GVR = DataGrid.Rows(index)
            'End If

            If e.CommandName = "Insert" Then
                costCode = DataGrid.FooterRow.FindControl("CostingCodeAdd")
                costName = DataGrid.FooterRow.FindControl("CostingNameAdd")
                costType = DataGrid.FooterRow.FindControl("CostingTypeAdd")

                If costCode.Text.Trim.Length = 0 Then
                    lbstatus.Text = "Costiing Code must be filled."
                    costCode.Focus()
                    Exit Sub
                End If

                If costName.Text.Trim.Length = 0 Then
                    lbstatus.Text = "Costing Name must be filled."
                    costName.Focus()
                    Exit Sub
                End If

                If SQLExecuteScalar("SELECT * From MsCosting WHERE CostingCode=" + QuotedStr(costCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lbstatus.Text = "Costing Code " + QuotedStr(costCode.Text) + " has already been exist"
                    Exit Sub
                End If

                SQLString = "Insert into MsCosting (CostingCode, CostingName, CostingType, UserId, UserDate) " + _
                "SELECT " + QuotedStr(costCode.Text) + ", " + QuotedStr(costName.Text) + _
                "," + QuotedStr(costType.SelectedValue) + _
                "," + QuotedStr(ViewState("UserId").ToString) + ", getDate()"

                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

                bindDataGrid()
            ElseIf e.CommandName = "Account" Then
                'GVR = DataGrid.Rows(CInt(e.CommandArgument))
                lbCode = DataGrid.Rows(CInt(e.CommandArgument)).FindControl("CostingCode")
                lbName = DataGrid.Rows(CInt(e.CommandArgument)).FindControl("CostingName")
                'ViewState("Nmbr") = GVR.Cells(0).Text

                'tbRental.Text = tbCode.Text
                'TbMaterialType.Text = lbCode.Text
                tbCode.Text = lbCode.Text
                tbName.Text = lbName.Text

                pnlHd.Visible = False
                PnlAssign.Visible = True

                LoadDataGrid()
                'bindDataGridDt()
            ElseIf e.CommandName = "Assign" Then
                'Dim paramgo As String
                'GVR = DataGrid.Rows(CInt(e.CommandArgument))
                'paramgo = GVR.Cells(0).Text + "|" + GVR.Cells(1).Text
                'If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                '    Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenAssign('Pay Type', '" + Request.QueryString("KeyId") + "', '" + paramgo + "','AssMsPayTypeUser');", True)
                'End If
            End If
        Catch ex As Exception
            lbstatus.Text = lbstatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim costCode, costName As TextBox
        Dim costCodeTemp As Label
        Dim costType As DropDownList
        Dim SQLString As String
        'Dim Validate As String = "^[a-zA-Z0-9 ]*$"
        'Dim r As New Regex(Validate)

        Try
            'costCode = DataGrid.Rows(e.RowIndex).FindControl("CostingCodeTemp")
            costCodeTemp = DataGrid.Rows(e.RowIndex).FindControl("CostingCodeTemp")
            costName = DataGrid.Rows(e.RowIndex).FindControl("CostingNameEdit")
            costType = DataGrid.Rows(e.RowIndex).FindControl("CostingTypeEdit")

            'If costCode.Text.Trim.Length = 0 Then
            '    lbstatus.Text = "Costing Code must be filled."
            '    costCode.Focus()
            '    Exit Sub
            'End If

            If costName.Text.Trim.Length = 0 Then
                lbstatus.Text = "Costing Name must be filled."
                costName.Focus()
                Exit Sub
            End If

            'If r.IsMatch(dbName.Text) = False Then
            '    lstatus.Text = "Please enter valid Characters"
            '    dbName.Focus()
            '    Exit Sub
            'End If

            'SQLString = "Update MsCosting set CostingCode='" + costCode.Text.Replace("'", "''") + "' " + _
            '", CostingName='" + costName.Text.Replace("'", "''") + "'" + _
            '", CostingType='" + costType.SelectedValue.Replace("'", "''") + "'" + _
            '" where CostingCode = '" + costCodeTemp.Text + "' "
            SQLString = "Update MsCosting set CostingName='" + costName.Text.Replace("'", "''") + "'" + _
            ", CostingType='" + costType.SelectedValue.Replace("'", "''") + "'" + _
            " where CostingCode = '" + costCodeTemp.Text + "' "

            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid()

        Catch ex As Exception
            lbstatus.Text = lbstatus.Text + "DataGrid_Update Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGrid.RowDeleting
        Dim GVR As GridViewRow = DataGrid.Rows(e.RowIndex)
        Dim txtID As Label

        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If

            txtID = DataGrid.Rows(e.RowIndex).FindControl("CostingCode")
            SQLExecuteNonQuery("Delete from MsCostingDt where Costing = '" & txtID.Text & "'", ViewState("DBConnection").ToString)

            SQLExecuteNonQuery("Delete from MsCosting where CostingCode = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
            bindDataGrid()

        Catch ex As Exception
            lbstatus.Text = "DataGrid_Delete Error: " & ex.ToString
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
            lbstatus.Text = "UserDG_SortCommandError =" + vbCrLf + ex.ToString
        End Try
    End Sub

    Private Sub LoadDataAssign()
        Try
            If tbCode.Text.Trim <> "" Then
                'dsAssigned.SelectCommand = "EXEC S_MsMethodUserAssign " + QuotedStr(tbCode.Text) + ", " + ddlGroupBy.SelectedIndex.ToString
                dsAssigned.SelectCommand = "EXEC S_MsCostingAssign " + QuotedStr(tbCode.Text) '+ ", " + ddlGroupBy.SelectedIndex.ToString
            Else
                dsAssigned.SelectCommand = "SELECT '' AS CODE, '' AS NAME"
            End If
            AssignedGrid.DataBind()
        Catch ex As Exception
            Throw New Exception("Assign Grid Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub LoadDataAvailable()
        Try
            If tbCode.Text.Trim <> "" Then
                'dsAvailable.SelectCommand = "EXEC S_MsMethodUserAvailable " + QuotedStr(tbCode.Text) + ", " + ddlGroupBy.SelectedIndex.ToString
                dsAvailable.SelectCommand = "EXEC S_MsCostingAvailable " + QuotedStr(tbCode.Text) '+ ", " + ddlGroupBy.SelectedIndex.ToString
            Else
                dsAvailable.SelectCommand = "SELECT '' AS CODE, '' AS NAME"
            End If
            AvailableGrid.DataBind()
        Catch ex As Exception
            Throw New Exception("Assign Grid Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub ExecSPAddRemove(ByVal GroupValue As String, ByVal StrSelected As String, ByVal Flag As Integer)
        Dim SQLString As String
        Try
            'SQLString = "EXEC S_MsMethodUserAddRemove '" + GroupValue + "', " + Page.ToString + ", '" + StrSelected + "', '" + ViewState("UserId").ToString + "', " + Flag.ToString
            'SQLString = "EXEC S_MsRentalBOMDtAddRemove '" + GroupValue + "','" + MaterialType + "', '" + StrSelected + "', '" + ViewState("UserId").ToString + "', " + Flag.ToString
            SQLString = "EXEC S_MsCostingDtAddRemove '" + GroupValue + "','" + StrSelected + "', '" + ViewState("UserId").ToString + "', " + Flag.ToString
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            LoadDataGrid()
        Catch ex As Exception
            Throw New Exception("Execute sp Add Remove Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnAddAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddAll.Click
        Dim StrFilter As String
        Dim P As Integer
        Try
            If tbCode.Text = "" Then Exit Sub
            ' 0 = Warehouse   1 = User
            If AvailableGrid.FilterEnabled = True And AvailableGrid.FilterExpression.Length > 0 Then
                StrFilter = QuotedStr(AvailableGrid.FilterExpression)
                P = StrFilter.Length
                StrFilter = StrFilter.Substring(1, P - 2)
            Else
                StrFilter = ""
            End If
            'lbstatus.Text = StrFilter
            'Exit Sub

            'ExecSPAddRemove(tbCode.Text, ddlGroupBy.SelectedIndex, StrFilter, 0)
            ExecSPAddRemove(tbCode.Text, StrFilter, 0) 'TbMaterialType.Text,

            AvailableGrid.Selection.UnselectAll()
        Catch ex As Exception
            lbstatus.Text = "btn add all acc error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdd.Click
        Dim StrSelected As String
        Try
            If tbCode.Text = "" Then Exit Sub
            StrSelected = ""
            'If ddlGroupBy.SelectedValue = "Method" Then
            '    StrSelected = SelectedGridDev(AvailableGrid, "UserID")
            'ElseIf ddlGroupBy.SelectedValue = "User" Then
            '    StrSelected = SelectedGridDev(AvailableGrid, "MethodCode")
            'End If

            StrSelected = SelectedGridDev(AvailableGrid, "Account")
            If StrSelected.Length <> 0 Then
                StrSelected = Replace(StrSelected, "AND", "")
            End If

            If StrSelected.Trim <> "" Then
                ' 0 = Warehouse   1 = User
                'ExecSPAddRemove(tbCode.Text, ddlGroupBy.SelectedIndex, StrSelected, 1)
                'ExecSPAddRemove(tbCode.Text, TbMaterialType.Text, StrSelected, 1)
                ExecSPAddRemove(tbCode.Text, StrSelected, 1)
            End If
            AvailableGrid.Selection.UnselectAll()
        Catch ex As Exception
            lbstatus.Text = "btn Add Acc Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnRemove_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRemove.Click
        Dim StrSelected As String
        Try
            If tbCode.Text = "" Then Exit Sub
            StrSelected = ""
            'If ddlGroupBy.SelectedValue = "Method" Then
            '    StrSelected = SelectedGridDev(AssignedGrid, "UserID")
            'ElseIf ddlGroupBy.SelectedValue = "User" Then
            '    StrSelected = SelectedGridDev(AssignedGrid, "Method")
            'End If

            StrSelected = SelectedGridDev(AssignedGrid, "Account")

            If StrSelected.Trim <> "" Then
                ' 0 = Warehouse   1 = User
                'ExecSPAddRemove(tbCode.Text, ddlGroupBy.SelectedIndex, StrSelected, 2)
                'ExecSPAddRemove(tbCode.Text, TbMaterialType.Text, StrSelected, 2)
                ExecSPAddRemove(tbCode.Text, StrSelected, 2)
            End If
            AssignedGrid.Selection.UnselectAll()
        Catch ex As Exception
            lbstatus.Text = "btn Remove Acc Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnRemoveAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRemoveAll.Click
        Dim StrFilter As String
        Dim P As Integer
        Try
            If tbCode.Text = "" Then Exit Sub
            ' 0 = Warehouse   1 = User
            If AssignedGrid.FilterEnabled = True And AssignedGrid.FilterExpression.Length > 0 Then
                StrFilter = QuotedStr(AssignedGrid.FilterExpression)
                P = StrFilter.Length
                StrFilter = StrFilter.Substring(1, P - 2)
            Else
                StrFilter = ""
            End If

            'ExecSPAddRemove(tbCode.Text, ddlGroupBy.SelectedIndex, StrFilter, 3)
            'ExecSPAddRemove(tbCode.Text, TbMaterialType.Text, StrFilter, 3)
            ExecSPAddRemove(tbCode.Text, StrFilter, 3)
            AssignedGrid.Selection.UnselectAll()
        Catch ex As Exception
            lbstatus.Text = "btn Remove All Error : " + ex.ToString
        End Try
    End Sub

    Private Sub LoadDataGrid()
        Try
            '0 = Format JE, 1 = Account
            If tbCode.Text.Trim <> "" Then
                'dsAssigned.SelectCommand = "EXEC S_MsMethodUserAssign " + QuotedStr(tbCode.Text) + ", " + ddlGroupBy.SelectedIndex.ToString
                'dsAvailable.SelectCommand = "EXEC S_MsMethodUserAvailable " + QuotedStr(tbCode.Text) + ", " + ddlGroupBy.SelectedIndex.ToString

                dsAssigned.SelectCommand = "EXEC S_MsCostingAssign " + QuotedStr(tbCode.Text) '+ ", " + QuotedStr(TbMaterialType.Text)
                dsAvailable.SelectCommand = "EXEC S_MsCostingAvailable " + QuotedStr(tbCode.Text) '+ ", " + QuotedStr(TbMaterialType.Text)
            Else
                dsAssigned.SelectCommand = "Select '' as CODE, '' as NAME"
                dsAvailable.SelectCommand = "Select '' as CODE, '' as NAME"
            End If

            AssignedGrid.Selection.UnselectAll()
            AvailableGrid.Selection.UnselectAll()
            AssignedGrid.DataBind()
            AvailableGrid.DataBind()
        Catch ex As Exception
            Throw New Exception("Assign Grid JE Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'tbRental.Text = ""
        'TbMaterialType.Text = ""

        pnlHd.Visible = True
        PnlAssign.Visible = False
    End Sub

    Protected Sub AvailableGrid_BeforeColumnSortingGrouping(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridView.ASPxGridViewBeforeColumnGroupingSortingEventArgs) Handles AvailableGrid.BeforeColumnSortingGrouping
        'New
        Try
            LoadDataAvailable()
        Catch ex As Exception
            lbstatus.Text = ex.ToString
        End Try
    End Sub

    Protected Sub AvailableGrid_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles AvailableGrid.PageIndexChanged
        'New
        Try
            LoadDataAvailable()
        Catch ex As Exception
            lbstatus.Text = ex.ToString
        End Try
    End Sub

    Protected Sub AvailableGrid_ProcessColumnAutoFilter(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridView.ASPxGridViewAutoFilterEventArgs) Handles AvailableGrid.ProcessColumnAutoFilter
        'New
        Try
            LoadDataAvailable()
        Catch ex As Exception
            lbstatus.Text = ex.ToString
        End Try
    End Sub

    Protected Sub AssignedGrid_BeforeColumnSortingGrouping(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridView.ASPxGridViewBeforeColumnGroupingSortingEventArgs) Handles AssignedGrid.BeforeColumnSortingGrouping
        'New
        Try
            LoadDataAssign()
        Catch ex As Exception
            lbstatus.Text = ex.ToString
        End Try
    End Sub

    Protected Sub AssignedGrid_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles AssignedGrid.PageIndexChanged
        'New
        Try
            LoadDataAssign()
        Catch ex As Exception
            lbstatus.Text = ex.ToString
        End Try
    End Sub

    Protected Sub AssignedGrid_ProcessColumnAutoFilter(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridView.ASPxGridViewAutoFilterEventArgs) Handles AssignedGrid.ProcessColumnAutoFilter
        'New
        Try
            LoadDataAssign()
        Catch ex As Exception
            lbstatus.Text = ex.ToString
        End Try
    End Sub
End Class
