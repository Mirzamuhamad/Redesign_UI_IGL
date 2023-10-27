Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data
Imports DevExpress.Web.ASPxGridView

Partial Class Master_MsMachineMaintenance_MsMachineMaintenance
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()
            End If
            dsJob.ConnectionString = ViewState("DBConnection")
            dsMaintenanceItem.ConnectionString = ViewState("DBConnection")
            dsMaintenanceDuration.ConnectionString = ViewState("DBConnection")

            If Not IsPostBack Then
                If Not Request.QueryString("Code") Is Nothing Then
                    'FromMasterPage()
                End If
                ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            End If
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnSearch" Then
                    tbCode.Text = Session("Result")(0).ToString
                    tbName.Text = Session("Result")(1).ToString
                    bindDataGrid()
                End If
                Session("Result") = Nothing
                ViewState("Sender") = Nothing
                Session("filter") = Nothing
                Session("Column") = Nothing
            End If
            lbstatus.Text = ""
        Catch ex As Exception
            lbstatus.Text = "page load Error : " + ex.ToString
        End Try
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

    Private Sub FromMasterPage()
        Dim param() As String
        Try
            btnPrint.Visible = False
            param = Request.QueryString("Code").ToString.Split("|")
            tbCode.Text = param(0)
            tbName.Text = param(1)
            tbCode.Enabled = False
            btnSearch.Visible = False

        Catch ex As Exception
            Throw New Exception("Load Assigned Code Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT * FROM VMsMachine"
            ResultField = "Machine_Code, Machine_Name, Specification"
            'End If
            Session("DBConnection") = ViewState("DBConnection")
            ViewState("Sender") = "btnSearch"
            Session("Column") = ResultField.Split(",")

            If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenPopup();", True)
            End If
        Catch ex As Exception
            lbstatus.Text = "btn Search Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCode.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("SELECT * FROM VMsMachine WHERE Machine_Code = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbCode.Text = ""
                tbName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbCode.Text = dr("Machine_Code").ToString
                tbName.Text = dr("Machine_Name").ToString
            End If
            'If tbCode.Text <> "" Then
            '    btnNewSlip.Visible = True
            'Else
            '    btnNewSlip.Visible = False
            'End If
            bindDataGrid()
        Catch ex As Exception
            lbstatus.Text = "tb Code Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim SQLString As String
        Try
            SQLString = "EXEC S_MsMachineMaintenancePrint " + QuotedStr(ViewState("UserId").ToString)
            'End If
            Session("PrintType") = "Print"
            Session("DBConnection") = ViewState("DBConnection")
            Session("SelectCommand") = SQLString
            Session("ReportFile") = ".../../../Rpt/FormPayrollAllatus.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            lbstatus.Text = "Btn Print Error : " + ex.ToString
        End Try
    End Sub

    Private Sub bindDataGrid()
        Dim SqlString As String
        Try
            SqlString = "EXEC S_MsMachineMaintenanceView " + QuotedStr(tbCode.Text)
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "MaintenanceItem DESC"
            End If
            PnlAssign.Visible = True
            pnlNewSlip.Visible = False
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)

            'Dim ddlMaintenanceItem As DropDownList
            'ddlMaintenanceItem = DataGrid.FooterRow.FindControl("CurrencyAdd")
            'ddlMaintenanceItem.SelectedValue = ViewState("Currency")

        Catch ex As Exception
            lbstatus.Text = "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Private Sub bindDataGridDt()
        Dim SqlString As String
        Dim GVR As GridViewRow
        Dim ddlMainBy, ddlMaintenDuration As DropDownList
        Dim tbRunHour As TextBox
        Try
            SqlString = "EXEC S_MsMachineMaintenanceDtView " + QuotedStr(tbCode.Text) + "," + QuotedStr(tbMaintenanceCode.Text)
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "Job DESC"
            End If
            PnlAssign.Visible = False
            pnlNewSlip.Visible = True
            BindDataMaster(SqlString, DataGridDt, ViewState("SortExpression"), ViewState("DBConnection").ToString)
            'add

            GVR = DataGridDt.FooterRow
            ddlMainBy = GVR.FindControl("MaintenanceByAdd")
            ddlMaintenDuration = GVR.FindControl("MaintenanceDurationAdd")
            tbRunHour = GVR.FindControl("RunningHourAdd")
            tbRunHour.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbRunHour.Text = "0"
            ddlMainBy.SelectedIndex = 0
            ddlMaintenDuration.Enabled = ddlMainBy.SelectedValue = "Interval"
            tbRunHour.Enabled = ddlMainBy.SelectedValue <> "Interval"
            If tbRunHour.Enabled = False Then
                tbRunHour.Text = "0"
            End If
            If ddlMaintenDuration.Enabled = False Then
                ddlMaintenDuration.SelectedValue = ""
            End If
        Catch ex As Exception
            lbstatus.Text = "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Protected Sub btnCancelSlip_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelSlip.Click
        Try
            tbCode.Enabled = True
            btnSearch.Visible = True
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            pnlNewSlip.Visible = False
            PnlAssign.Visible = True
            tbCode.Focus()
        Catch ex As Exception
            lbstatus.Text = "btnCancelSlip_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGrid.PageIndexChanging
        Try
            DataGrid.PageIndex = e.NewPageIndex
            If DataGrid.EditIndex <> -1 Then
                DataGrid_RowCancelingEdit(Nothing, Nothing)
            End If
            bindDataGrid()
        Catch ex As Exception
            lbstatus.Text = "DataGrid_PageIndexChanging Error : " + vbCrLf + ex.ToString
        End Try

    End Sub

    Protected Sub DataGrid_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles DataGrid.RowCancelingEdit
        Try
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid()
        Catch ex As Exception
            lbstatus.Text = "DataGrid_Cancel Error : " + vbCrLf + ex.ToString
        End Try
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

    Protected Sub DataGrid_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGrid.RowCommand
        Dim SQLString As String
        Dim dbCurrency As DropDownList
        Dim tbItemNumber, tbItemLocation, tbItemSpecs, tbItemQty As TextBox
        Dim lbMaintenanceCode, lbMaintenanceName As Label
        Dim GVR As GridViewRow
        Try
            If e.CommandName = "Insert" Then
                dbCurrency = DataGrid.FooterRow.FindControl("MaintenanceItemAdd")
                tbItemSpecs = DataGrid.FooterRow.FindControl("ItemSpecsAdd")
                tbItemNumber = DataGrid.FooterRow.FindControl("ItemNumberAdd")
                tbItemLocation = DataGrid.FooterRow.FindControl("ItemLocationAdd")
                tbItemQty = DataGrid.FooterRow.FindControl("ItemQtyAdd")

                If dbCurrency.SelectedValue = "" Then
                    lbstatus.Text = "Maintenance Item Must Be filled."
                    dbCurrency.Focus()
                    Exit Sub
                End If
                If tbItemSpecs.Text.Trim = "" Then
                    lbstatus.Text = "Specification Must Be filled."
                    tbItemSpecs.Focus()
                    Exit Sub
                End If
                If tbItemNumber.Text.Trim = "" Then
                    lbstatus.Text = "Item Number Must Be filled."
                    tbItemNumber.Focus()
                    Exit Sub
                End If
                If tbItemLocation.Text.Trim = "" Then
                    lbstatus.Text = "Item Location Must Be filled."
                    tbItemLocation.Focus()
                    Exit Sub
                End If
                If tbItemQty.Text.Trim = "" Then
                    lbstatus.Text = "Item Qty Must Be filled."
                    tbItemQty.Focus()
                    Exit Sub
                End If

                If SQLExecuteScalar("SELECT Machine FROM MsMachineMaintenance WHERE Machine = " + QuotedStr(tbCode.Text) + " AND MaintenanceItem = " + QuotedStr(dbCurrency.SelectedValue), ViewState("DBConnection").ToString).Length > 0 Then
                    lbstatus.Text = "Machine " + tbCode.Text + " Maintenance " + dbCurrency.SelectedItem.Text + " has already exists"
                    Exit Sub
                End If
                'insert the new entry
                SQLString = "INSERT INTO MsMachineMaintenance (Machine, MaintenanceItem, ItemSpecs, ItemNumber, Itemlocation, ItemQty, UserId, UserDate)" + _
                        "Values ( " + QuotedStr(tbCode.Text) + ", " + QuotedStr(dbCurrency.SelectedValue) + ", " + QuotedStr(tbItemSpecs.Text) + ", " + _
                        QuotedStr(tbItemNumber.Text) + ", " + QuotedStr(tbItemLocation.Text) + ", " + _
                        QuotedStr(tbItemQty.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", getDate())"

                SQLString = ChangeQuoteNull(SQLString)
                SQLString = SQLString.Replace("'1900-01-01'", "NULL")
                SQLString = SQLString.Replace("'0001-01-01'", "NULL")
                SQLString = SQLString.Replace("''", "NULL")
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                bindDataGrid()
            ElseIf e.CommandName = "View" Then
                GVR = DataGrid.Rows(CInt(e.CommandArgument))
                tbCode.Enabled = False
                btnSearch.Visible = False
                btnPrint.Visible = False
                lbMaintenanceCode = GVR.FindControl("MaintenanceItemCode")
                lbMaintenanceName = GVR.FindControl("MaintenanceItem")
                tbMaintenanceCode.Text = lbMaintenanceCode.Text
                tbMaintenanceName.Text = lbMaintenanceName.Text
                'PnlAssign.Visible = False
                'pnlNewSlip.Visible = True
                bindDataGridDt()
            End If
        Catch ex As Exception
            lbstatus.Text = lbstatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGrid.RowDeleting
        Dim EffDate As Label
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            EffDate = DataGrid.Rows(e.RowIndex).FindControl("MaintenanceItemCode")
            SQLExecuteNonQuery("Delete from MsMachineMaintenanceJob where Machine = " + QuotedStr(tbCode.Text) + " AND MaintenanceItem = " + QuotedStr(EffDate.Text), ViewState("DBConnection").ToString)
            SQLExecuteNonQuery("Delete from MsMachineMaintenance where Machine = " + QuotedStr(tbCode.Text) + " AND MaintenanceItem = " + QuotedStr(EffDate.Text), ViewState("DBConnection").ToString)
            bindDataGrid()
        Catch ex As Exception
            lbstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        Dim tbMaintenanceItem As Label
        Dim tbItemSpecs As TextBox
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()
            obj = DataGrid.Rows(e.NewEditIndex)
            tbMaintenanceItem = obj.FindControl("MaintenanceItemEdit")
            tbItemSpecs = obj.FindControl("ItemSpecsEdit")
            tbItemSpecs.Focus()
        Catch ex As Exception
            lbstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim tbMaintenanceItem As Label
        Dim tbItemSpecs, tbItemNumber, tbItemLocation, tbItemQty As TextBox
        Dim GVR As GridViewRow
        Try
            GVR = DataGrid.Rows(e.RowIndex)
            tbItemSpecs = GVR.FindControl("ItemSpecsEdit")
            tbItemNumber = GVR.FindControl("ItemNumberEdit")
            tbItemLocation = GVR.FindControl("ItemLocationEdit")
            tbItemQty = GVR.FindControl("ItemQtyEdit")
            tbMaintenanceItem = GVR.FindControl("MaintenanceItemCodeEdit")

            If tbItemSpecs.Text.Trim = "" Then
                lbstatus.Text = MessageDlg("Spesification must have value")
                tbItemSpecs.Focus()
                Exit Sub
            End If
            If tbItemNumber.Text.Trim = "" Then
                lbstatus.Text = MessageDlg("Item Number must have value")
                tbItemNumber.Focus()
                Exit Sub
            End If
            If tbItemLocation.Text.Trim = "" Then
                lbstatus.Text = MessageDlg("Item Location must have value")
                tbItemLocation.Focus()
                Exit Sub
            End If
            If tbItemQty.Text.Trim = "" Then
                lbstatus.Text = MessageDlg("Item Qty must have value")
                tbItemQty.Focus()
                Exit Sub
            End If

            'IfExist = SQLExecuteScalar("SELECT MaintenanceItem FROM MsMachineMaintenance WHERE Machine = " + QuotedStr(tbCode.Text) + " AND MaintenanceItem = " + QuotedStr(tbMaintenanceItem.Text), ViewState("DBConnection").ToString)

            'If IfExist.Trim.Length > 0 Then
            '    lbstatus.Text = MessageDlg("Machine " + QuotedStr(tbCode.Text) + " has already Maintenance Item " + QuotedStr(IfExist))
            '    tbItemSpecs.Focus()
            '    Exit Sub
            'End If

            SQLString = "Update MsMachineMaintenance set ItemSpecs = " + QuotedStr(tbItemSpecs.Text) + _
            ", ItemNumber =" + QuotedStr(tbItemNumber.Text) + ", ItemLocation =" + QuotedStr(tbItemLocation.Text) + _
            ", ItemQty = " + QuotedStr(tbItemQty.Text) + _
            "  WHERE Machine = " + QuotedStr(tbCode.Text) + " AND MaintenanceItem = " + QuotedStr(tbMaintenanceItem.Text)

            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid()

        Catch ex As Exception
            lbstatus.Text = "DataGrid_Update Error: " & ex.ToString
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
            lbstatus.Text = "DataGrid_Sorting =" + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub DataGridDt_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGridDt.PageIndexChanging
        Try
            DataGridDt.PageIndex = e.NewPageIndex
            If DataGridDt.EditIndex <> -1 Then
                DataGridDt_RowCancelingEdit(Nothing, Nothing)
            End If
            bindDataGridDt()
        Catch ex As Exception
            lbstatus.Text = "DataGridDt_PageIndexChanging Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub DataGridDt_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles DataGridDt.RowCancelingEdit
        Try
            DataGridDt.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGridDt.EditIndex = -1
            bindDataGridDt()
        Catch ex As Exception
            lbstatus.Text = "DataGridDt_Cancel Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub DataGridDt_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGridDt.RowCommand
        Dim SQLString As String
        Dim dbJobName, dbMaintenanceBy, dbFgNeedMaterial, tbMaintenDuration As DropDownList
        Dim tbRunningHour As TextBox
        Try
            If e.CommandName = "Insert" Then
                dbJobName = DataGridDt.FooterRow.FindControl("JobNameAdd")
                dbMaintenanceBy = DataGridDt.FooterRow.FindControl("MaintenanceByAdd")
                dbFgNeedMaterial = DataGridDt.FooterRow.FindControl("FgNeedMaterialAdd")
                tbRunningHour = DataGridDt.FooterRow.FindControl("RunningHourAdd")
                tbMaintenDuration = DataGridDt.FooterRow.FindControl("MaintenanceDurationAdd")
                
                If dbJobName.SelectedValue = "" Then
                    lbstatus.Text = "Job Must Be filled."
                    dbJobName.Focus()
                    Exit Sub
                End If
                If dbMaintenanceBy.Text.Trim = "" Then
                    lbstatus.Text = "Maintenance By Must Be filled."
                    dbMaintenanceBy.Focus()
                    Exit Sub
                End If
                If (dbMaintenanceBy.SelectedIndex = 0) And CFloat(tbRunningHour.Text) <= 0 Then
                    lbstatus.Text = "Running Hour Must Be filled."
                    tbRunningHour.Focus()
                    Exit Sub
                End If
                If (dbMaintenanceBy.SelectedIndex = 1) And (tbMaintenDuration.SelectedValue.Trim = "") Then
                    lbstatus.Text = "Maintenance Duration Must Be filled."
                    tbMaintenDuration.Focus()
                    Exit Sub
                End If
                If dbFgNeedMaterial.Text.Trim = "" Then
                    lbstatus.Text = "Need Material Must Be filled."
                    dbFgNeedMaterial.Focus()
                    Exit Sub
                End If

                If SQLExecuteScalar("SELECT Machine FROM MsMachineMaintenanceJob WHERE Machine = " + QuotedStr(tbCode.Text) + " AND MaintenanceItem = " + QuotedStr(tbMaintenanceCode.Text) + " AND Job = " + QuotedStr(dbJobName.SelectedValue), ViewState("DBConnection").ToString).Length > 0 Then
                    lbstatus.Text = "Machine " + tbCode.Text + " Maintenance " + tbMaintenanceCode.Text + " Job " + dbJobName.SelectedValue + " has already exists"
                    Exit Sub
                End If

                'insert the new entry
                SQLString = "INSERT INTO MsMachineMaintenanceJob (Machine, MaintenanceItem, Job, MaintenanceBy, RunningHour, MaintenanceDuration, FgNeedMaterial, UserId, UserDate)" + _
                        "Values ( " + QuotedStr(tbCode.Text) + ", " + QuotedStr(tbMaintenanceCode.Text) + ", " + QuotedStr(dbJobName.SelectedValue) + ", " + _
                        QuotedStr(dbMaintenanceBy.SelectedValue) + ", " + tbRunningHour.Text.Replace(",", "") + ", " + _
                        QuotedStr(tbMaintenDuration.SelectedValue) + ", " + QuotedStr(dbFgNeedMaterial.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString) + ", getDate())"

                SQLString = ChangeQuoteNull(SQLString)
                SQLString = SQLString.Replace("'1900-01-01'", "NULL")
                SQLString = SQLString.Replace("'0001-01-01'", "NULL")
                SQLString = SQLString.Replace("''", "NULL")
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                bindDataGridDt()
            End If
        Catch ex As Exception
            lbstatus.Text = lbstatus.Text + "DataGridDt_RowCommand Error:" & ex.ToString
        End Try
    End Sub

    Protected Sub DataGridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGridDt.RowDeleting
        Dim EffDate As Label
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            EffDate = DataGridDt.Rows(e.RowIndex).FindControl("JobCode")
            SQLExecuteNonQuery("Delete from MsMachineMaintenanceJob where Machine = " + QuotedStr(tbCode.Text) + " AND MaintenanceItem = " + QuotedStr(tbMaintenanceCode.Text) + " AND Job = " + QuotedStr(EffDate.Text), ViewState("DBConnection").ToString)
            bindDataGridDt()
        Catch ex As Exception
            lbstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub DataGridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGridDt.RowEditing
        Dim obj As GridViewRow
        Dim dbMaintenanceBy As DropDownList
        Dim tbRunningHour As TextBox
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGridDt.EditIndex = e.NewEditIndex
            DataGridDt.ShowFooter = False
            bindDataGridDt()
            obj = DataGridDt.Rows(e.NewEditIndex)
            dbMaintenanceBy = obj.FindControl("MaintenanceByEdit")
            tbRunningHour = obj.FindControl("RunningHourEdit")
            tbRunningHour.Attributes.Add("OnKeyDown", "return PressNumeric();")
            dbMaintenanceBy.Focus()
        Catch ex As Exception
            lbstatus.Text = "DataGridDt_RowEditing exception : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGridDt_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGridDt.RowUpdating
        Dim SQLString As String
        Dim lbJob As Label
        Dim dbMaintenanceBy, dbFgNeedMaterial, tbMaintenDuration As DropDownList
        Dim tbRunningHour As TextBox
        Dim GVR As GridViewRow
        Try
            GVR = DataGridDt.Rows(e.RowIndex)
            lbJob = GVR.FindControl("JobCodeEdit")
            dbMaintenanceBy = GVR.FindControl("MaintenanceByEdit")
            tbRunningHour = GVR.FindControl("RunningHourEdit")
            tbMaintenDuration = GVR.FindControl("MaintenanceDurationEdit")
            dbFgNeedMaterial = GVR.FindControl("FgNeedMaterialEdit")


            If dbMaintenanceBy.Text.Trim = "" Then
                lbstatus.Text = "Maintenance By Must Be filled."
                dbMaintenanceBy.Focus()
                Exit Sub
            End If

            If (dbMaintenanceBy.SelectedIndex = 0) And CFloat(tbRunningHour.Text) <= 0 Then
                lbstatus.Text = "Running Hour Must Be filled."
                tbRunningHour.Focus()
                Exit Sub
            End If

            If (dbMaintenanceBy.SelectedIndex = 1) And (tbMaintenDuration.SelectedValue.Trim = "") Then
                lbstatus.Text = "Maintenance Duration Must Be filled."
                tbMaintenDuration.Focus()
                Exit Sub
            End If

            If dbFgNeedMaterial.Text.Trim = "" Then
                lbstatus.Text = "Need Material Must Be filled."
                dbFgNeedMaterial.Focus()
                Exit Sub
            End If

            'IfExist = SQLExecuteScalar("SELECT MaintenanceItem FROM MsMachineMaintenance WHERE Machine = " + QuotedStr(tbCode.Text) + " AND MaintenanceItem = " + QuotedStr(tbMaintenanceItem.Text), ViewState("DBConnection").ToString)

            'If IfExist.Trim.Length > 0 Then
            '    lbstatus.Text = MessageDlg("Machine " + QuotedStr(tbCode.Text) + " has already Maintenance Item " + QuotedStr(IfExist))
            '    tbItemSpecs.Focus()
            '    Exit Sub
            'End If

            SQLString = "Update MsMachineMaintenanceJob set MaintenanceBy = " + QuotedStr(dbMaintenanceBy.SelectedValue) + _
            ", RunningHour =" + tbRunningHour.Text.Replace(",", "") + ", MaintenanceDuration =" + QuotedStr(tbMaintenDuration.SelectedValue) + _
            ", FgNeedMaterial = " + QuotedStr(dbFgNeedMaterial.SelectedValue) + _
            "  WHERE Machine = " + QuotedStr(tbCode.Text) + " AND MaintenanceItem = " + QuotedStr(tbMaintenanceCode.Text) + " AND Job = " + QuotedStr(lbJob.Text)

            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            DataGridDt.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGridDt.EditIndex = -1
            bindDataGridDt()

        Catch ex As Exception
            lbstatus.Text = "DataGridDt_Update Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub DataGridDt_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles DataGridDt.Sorting
        Try
            If ViewState("SortOrder") = Nothing Or ViewState("SortOrder") = "DESC" Then
                ViewState("SortOrder") = "ASC"
            Else
                ViewState("SortOrder") = "DESC"
            End If
            ViewState("SortExpression") = e.SortExpression + " " + ViewState("SortOrder")
            bindDataGridDt()
        Catch ex As Exception
            lbstatus.Text = "DataGrid_Sorting =" + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub MaintenanceByAdd_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ddlMainBy, ddlMaintenDuration As DropDownList
        Dim tbRunHour As TextBox
        Dim GVR As GridViewRow

        Try
            GVR = DataGridDt.FooterRow
            ddlMainBy = GVR.FindControl("MaintenanceByAdd")
            ddlMaintenDuration = GVR.FindControl("MaintenanceDurationAdd")
            tbRunHour = GVR.FindControl("RunningHourAdd")
            ddlMaintenDuration.Enabled = ddlMainBy.SelectedValue = "Interval"
            tbRunHour.Enabled = ddlMainBy.SelectedValue <> "Interval"
            If tbRunHour.Enabled = False Then
                tbRunHour.Text = "0"
            End If
            If ddlMaintenDuration.Enabled = False Then
                ddlMaintenDuration.SelectedValue = ""
            End If
        Catch ex As Exception
            lbstatus.Text = "MaintenanceByAdd_SelectedIndexChanged Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub MaintenanceByEdit_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ddlMainBy, ddlMaintenDuration As DropDownList
        Dim tbRunHour As TextBox
        Dim GVR As GridViewRow
        Try
            GVR = DataGridDt.Rows(DataGridDt.EditIndex)
            ddlMainBy = GVR.FindControl("MaintenanceByEdit")
            ddlMaintenDuration = GVR.FindControl("MaintenanceDurationEdit")
            tbRunHour = GVR.FindControl("RunningHourEdit")
            ddlMaintenDuration.Enabled = ddlMainBy.SelectedValue = "Interval"
            tbRunHour.Enabled = ddlMainBy.SelectedValue <> "Interval"
            If tbRunHour.Enabled = False Then
                tbRunHour.Text = "0"
            End If
            If ddlMaintenDuration.Enabled = False Then
                ddlMaintenDuration.SelectedValue = ""
            End If
        Catch ex As Exception
            lbstatus.Text = "MaintenanceByEdit_SelectedIndexChanged Error : " + vbCrLf + ex.ToString
        End Try
    End Sub
End Class
