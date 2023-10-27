Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data
Imports DevExpress.Web.ASPxGridView

Partial Class Master_MsMaintenanceItemJob_MsMaintenanceItemJob
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                ddlRow.SelectedValue = "15"
            End If
            'dsMaintenanceDuration.ConnectionString = ViewState("DBConnection")
            dsUnit.ConnectionString = ViewState("DBConnection")
            dsMTNPattern.ConnectionString = ViewState("DBConnection")
            If Not IsPostBack Then
                If Not Request.QueryString("Code") Is Nothing Then
                    'FromMasterPage()
                End If
                ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            End If
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnItemAdd" Or ViewState("Sender") = "btnItemEdit" Then
                    Dim ItemNo As TextBox
                    Dim ItemName As Label
                    If ViewState("Sender") = "btnItemAdd" Then
                        ItemNo = DataGrid.FooterRow.FindControl("ItemNoAdd")
                        ItemName = DataGrid.FooterRow.FindControl("ItemNameAdd")
                    Else
                        ItemNo = DataGrid.Rows(DataGrid.EditIndex).FindControl("ItemNoEdit")
                        ItemName = DataGrid.Rows(DataGrid.EditIndex).FindControl("ItemNameEdit")
                    End If
                    ItemNo.Text = Session("Result")(0).ToString
                    ItemName.Text = Session("Result")(1).ToString
                    ItemNo.Focus()
                End If
                If ViewState("Sender") = "btnMaterialAdd" Or ViewState("Sender") = "btnMaterialEdit" Then
                    Dim Material As TextBox
                    Dim MaterialName, Specification As Label
                    Dim ddlUnit As DropDownList
                    If ViewState("Sender") = "btnMaterialAdd" Then
                        Material = DataGridDt.FooterRow.FindControl("MaterialAdd")
                        MaterialName = DataGridDt.FooterRow.FindControl("MaterialNameAdd")
                        ddlUnit = DataGridDt.FooterRow.FindControl("UnitAdd")
                        Specification = DataGridDt.FooterRow.FindControl("SpecificationAdd")
                    Else
                        Material = DataGridDt.Rows(DataGridDt.EditIndex).FindControl("MaterialEdit")
                        MaterialName = DataGridDt.Rows(DataGridDt.EditIndex).FindControl("MaterialNameEdit")
                        ddlUnit = DataGridDt.Rows(DataGridDt.EditIndex).FindControl("UnitEdit")
                        Specification = DataGridDt.Rows(DataGridDt.EditIndex).FindControl("SpecificationEdit")
                    End If
                    Material.Text = Session("Result")(0).ToString
                    MaterialName.Text = Session("Result")(1).ToString
                    Specification.Text = Session("Result")(2).ToString
                    ddlUnit.SelectedValue = Session("Result")(3).ToString
                    Material.Focus()
                End If
                If ViewState("Sender") = "btnSearch" Then
                    tbCode.Text = Session("Result")(0).ToString
                    tbName.Text = Session("Result")(1).ToString
                    'bindDataGrid()
                    bindDataGridSearch()
                End If
                If ViewState("Sender") = "btnItemFrom" Then
                    tbItemFrom.Text = Session("Result")(0).ToString
                    tbItemNameFrom.Text = Session("Result")(1).ToString
                End If
                If ViewState("Sender") = "btnItemTo" Then
                    tbItemTo.Text = Session("Result")(0).ToString
                    tbItemNameTo.Text = Session("Result")(1).ToString
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
            Session("filter") = "SELECT * FROM VMsMaintenanceSection"
            ResultField = "MTNSectionCode, MTNSectionName"
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
            ds = SQLExecuteQuery("SELECT * FROM VMsMaintenanceSection WHERE MTNSectionCode = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbCode.Text = ""
                tbName.Text = ""
                pnlHd.Visible = False
            Else
                dr = ds.Tables(0).Rows(0)
                tbCode.Text = dr("MTNSectionCode").ToString
                tbName.Text = dr("MTNSectionName").ToString
                pnlHd.Visible = True
            End If
            'If tbCode.Text <> "" Then
            '    btnNewSlip.Visible = True
            'Else
            '    btnNewSlip.Visible = False
            'End If
            '
            'bindDataGrid()
            bindDataGridSearch()
        Catch ex As Exception
            lbstatus.Text = "tb Code Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim SQLString As String
        Try
            SQLString = "EXEC S_MsMaintenanceItemJobPrint " + QuotedStr(ViewState("UserId").ToString)
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
            SqlString = "EXEC S_MsMaintenanceItemJobView " + QuotedStr(tbCode.Text) + ", '' "
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "MaintenanceItem DESC"
            End If
            PnlAssign.Visible = True
            pnlHd.Visible = True
            btnCopy.Visible = True
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
        Dim tbQty As TextBox
        Try
            SqlString = "EXEC S_MsMaintenanceItemJobRMView " + QuotedStr(tbMaintenanceCode.Text) + "," + QuotedStr(tbJobCode.Text)
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "Material DESC"
            End If
            PnlAssign.Visible = False
            pnlHd.Visible = False
            btnCopy.Visible = False
            pnlNewSlip.Visible = True
            BindDataMaster(SqlString, DataGridDt, ViewState("SortExpression"), ViewState("DBConnection").ToString)
            'add

            GVR = DataGridDt.FooterRow
            tbQty = GVR.FindControl("QtyAdd")
            tbQty.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbQty.Text = "0"
            
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
            pnlHd.Visible = True
            btnCopy.Visible = True
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
            'bindDataGrid()
            bindDataGridSearch()
        Catch ex As Exception
            lbstatus.Text = "DataGrid_PageIndexChanging Error : " + vbCrLf + ex.ToString
        End Try

    End Sub

    Protected Sub DataGrid_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles DataGrid.RowCancelingEdit
        Try
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            'bindDataGrid()
            bindDataGridSearch()
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
        Dim FgActive, ddlPattern As DropDownList 'ddlDuration, 
        Dim tbItemNo, tbJob, tbJobName, tbJobDesc As TextBox
        Dim lbItemName, lbMtnItem, lblMtnName, lbJob, lbJobName, lbJobDesc, lbActive, lbPattern, lbFirstSchedule As Label 'lbDuration, 
        Dim tbFirstSchedule As BasicFrame.WebControls.BasicDatePicker
        Dim GVR As GridViewRow
        Try
            If e.CommandName = "Insert" Then
                tbItemNo = DataGrid.FooterRow.FindControl("ItemNoAdd")
                tbJob = DataGrid.FooterRow.FindControl("JobAdd")
                tbJobName = DataGrid.FooterRow.FindControl("JobNameAdd")
                tbJobDesc = DataGrid.FooterRow.FindControl("JobDescriptionAdd")
                FgActive = DataGrid.FooterRow.FindControl("FgActiveAdd")
                'ddlDuration = DataGrid.FooterRow.FindControl("MaintenanceDurationAdd")
                lbItemName = DataGrid.FooterRow.FindControl("ItemNameAdd")
                tbFirstSchedule = DataGrid.FooterRow.FindControl("FirstScheduleAdd")
                ddlPattern = DataGrid.FooterRow.FindControl("MTNPatternAdd")

                If tbCode.Text.Trim = "" Then
                    lbstatus.Text = "Maintenance Section Must Be Filled."
                    tbCode.Focus()
                    Exit Sub
                End If

                If tbItemNo.Text.Trim = "" Then
                    lbstatus.Text = "Maintenance Item Must Be filled."
                    tbItemNo.Focus()
                    Exit Sub
                End If

                If tbJob.Text.Trim = "" Then
                    lbstatus.Text = "Job Must Be filled."
                    tbJob.Focus()
                    Exit Sub
                End If

                If tbJobName.Text.Trim = "" Then
                    lbstatus.Text = "Job Name Must Be filled."
                    tbJobName.Focus()
                    Exit Sub
                End If

                If tbFirstSchedule.SelectedValue Is Nothing Then
                    lbstatus.Text = "First Schedule Must Be filled."
                    tbFirstSchedule.Focus()
                    Exit Sub
                End If

                If SQLExecuteScalar("SELECT MaintenanceItem, Job FROM MsMaintenanceItemJob WHERE MaintenanceItem = " + QuotedStr(tbItemNo.Text) + " AND Job = " + QuotedStr(tbJob.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lbstatus.Text = "Maintenance Item " + lbItemName.Text + ", Job " + tbJob.Text + " has already exists"
                    Exit Sub
                End If

                Dim FirstSchedule As String
                If Not tbFirstSchedule.IsNull Then
                    FirstSchedule = (Format(tbFirstSchedule.SelectedValue, "yyyy-MM-dd"))
                Else
                    FirstSchedule = ""
                End If
                'insert the new entry
                SQLString = "INSERT INTO MsMaintenanceItemJob (MaintenanceItem, Job, JobName, JobDescription, FgActive, UserId, UserDate, FirstSchedule, MTNPattern)" + _
                        "Values ( " + QuotedStr(tbItemNo.Text) + ", " + QuotedStr(tbJob.Text) + ", " + QuotedStr(tbJobName.Text) + ", " + _
                        QuotedStr(tbJobDesc.Text) + ", " + QuotedStr(FgActive.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString) + ", GetDate(), " + QuotedStr(FirstSchedule) + ", " + QuotedStr(ddlPattern.SelectedValue) + ")"

                SQLString = ChangeQuoteNull(SQLString)
                SQLString = SQLString.Replace("'1900-01-01'", "NULL")
                SQLString = SQLString.Replace("'0001-01-01'", "NULL")
                SQLString = SQLString.Replace("''", "NULL")
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                'bindDataGrid()
                bindDataGridSearch()
            ElseIf e.CommandName = "View" Then
                GVR = DataGrid.Rows(CInt(e.CommandArgument))
                tbCode.Enabled = False
                btnSearch.Visible = False
                btnPrint.Visible = False
                lbMtnItem = GVR.FindControl("ItemNo")
                lblMtnName = GVR.FindControl("ItemName")
                lbJob = GVR.FindControl("Job")
                lbJobName = GVR.FindControl("JobName")

                tbMaintenanceCode.Text = lbMtnItem.Text
                tbMaintenanceName.Text = lblMtnName.Text
                tbJobCode.Text = lbJob.Text
                tbJobNameV.Text = lbJobName.Text

                PanelCopy.Visible = False
                tbItemFrom.Text = ""
                tbItemNameFrom.Text = ""
                tbItemTo.Text = ""
                tbItemNameTo.Text = ""

                bindDataGridDt()
            ElseIf e.CommandName = "btnItemAdd" Or e.CommandName = "btnItemEdit" Then
                Dim FieldResult As String
                Session("DBConnection") = ViewState("DBConnection")
                If e.CommandName = "btnItemAdd" Then
                    Session("filter") = "Select * FROM V_MsMaintenanceItem where MTN_Section = " + QuotedStr(tbCode.Text)
                    ViewState("Sender") = "btnItemAdd"
                Else
                    Session("filter") = "Select * FROM V_MsMaintenanceItem where MTN_Section = " + QuotedStr(tbCode.Text)
                    ViewState("Sender") = "btnItemEdit"
                End If

                FieldResult = "MTN_Item, MTN_Item_Name"
                Session("Column") = FieldResult.Split(",")
                AttachScript("OpenSearchDlg();", Page, Me.GetType())
            ElseIf e.CommandName = "Copy" Then
                GVR = DataGrid.Rows(CInt(e.CommandArgument))
                lbMtnItem = GVR.FindControl("ItemNo")
                lblMtnName = GVR.FindControl("ItemName")
                lbJob = GVR.FindControl("Job")
                lbJobName = GVR.FindControl("JobName")
                lbJobDesc = GVR.FindControl("JobDescription")
                lbFirstSchedule = GVR.FindControl("FirstSchedule")
                'lbDuration = GVR.FindControl("MaintenanceDuration")
                lbActive = GVR.FindControl("FgActive")
                lbPattern = GVR.FindControl("MTNPattern")

                Dim tbItemNoNew, tbJobNew, tbJobNameNew, tbJobDescNew As TextBox
                Dim ddlFgActiveNew, ddlPatternNew As DropDownList 'ddlDurationNew, 
                Dim lbItemNameNew As Label
                Dim tbFirstSchedulNew As BasicFrame.WebControls.BasicDatePicker

                tbItemNoNew = DataGrid.FooterRow.FindControl("ItemNoAdd")
                lbItemNameNew = DataGrid.FooterRow.FindControl("ItemNameAdd")
                tbJobNew = DataGrid.FooterRow.FindControl("JobAdd")
                tbJobNameNew = DataGrid.FooterRow.FindControl("JobNameAdd")
                tbJobDescNew = DataGrid.FooterRow.FindControl("JobDescriptionAdd")
                'ddlDurationNew = DataGrid.FooterRow.FindControl("MaintenanceDurationAdd")
                ddlFgActiveNew = DataGrid.FooterRow.FindControl("FgActiveAdd")
                ddlPatternNew = DataGrid.FooterRow.FindControl("MTNPatternAdd")
                tbFirstSchedulNew = DataGrid.FooterRow.FindControl("FirstScheduleAdd")

                tbItemNoNew.Text = lbMtnItem.Text
                lbItemNameNew.Text = lblMtnName.Text
                tbJobNew.Text = lbJob.Text
                tbJobNameNew.Text = lbJobName.Text
                tbJobDescNew.Text = lbJobDesc.Text
                'ddlDurationNew.SelectedItem.Text = lbDuration.Text
                ddlFgActiveNew.SelectedValue = lbActive.Text
                ddlPatternNew.SelectedItem.Text = lbPattern.Text

                If lbFirstSchedule.Text <> "" Then
                    tbFirstSchedulNew.SelectedDate = lbFirstSchedule.Text
                End If

            End If
        Catch ex As Exception
            lbstatus.Text = lbstatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGrid.RowDeleting
        Dim MtnItem, Job As Label
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            MtnItem = DataGrid.Rows(e.RowIndex).FindControl("ItemNo")
            Job = DataGrid.Rows(e.RowIndex).FindControl("Job")
            SQLExecuteNonQuery("Delete from MsMaintenanceItemJob where MaintenanceItem = " + QuotedStr(MtnItem.Text) + " AND Job = " + QuotedStr(Job.Text), ViewState("DBConnection").ToString)
            SQLExecuteNonQuery("Delete from MsMaintenanceItemJobRM where MaintenanceItem = " + QuotedStr(MtnItem.Text) + " AND Job = " + QuotedStr(Job.Text), ViewState("DBConnection").ToString)
            'bindDataGrid()
            bindDataGridSearch()
        Catch ex As Exception
            lbstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        Dim tbMtnItem, tbJob As TextBox
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            'bindDataGrid()
            bindDataGridSearch()
            obj = DataGrid.Rows(e.NewEditIndex)
            tbMtnItem = obj.FindControl("ItemNoEdit")
            tbJob = obj.FindControl("JobEdit")
            ViewState("Maintenance") = tbMtnItem.Text
            ViewState("Job") = tbJob.Text
            tbMtnItem.Focus()
        Catch ex As Exception
            lbstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim lbItemName As Label
        Dim tbItemNo, tbJob, tbJobName, tbJobDesc As TextBox
        Dim ddlFgActive, ddlPattern As DropDownList 'ddlDuration, 
        Dim GVR As GridViewRow
        Dim tbFirstSchedule As BasicFrame.WebControls.BasicDatePicker
        Try
            GVR = DataGrid.Rows(e.RowIndex)
            lbItemName = GVR.FindControl("ItemNameEdit")
            tbItemNo = GVR.FindControl("ItemNoEdit")
            tbJob = GVR.FindControl("JobEdit")
            tbJobName = GVR.FindControl("JobNameEdit")
            tbJobDesc = GVR.FindControl("JobDescriptionEdit")
            ddlFgActive = GVR.FindControl("FgActiveEdit")
            'ddlDuration = GVR.FindControl("MaintenanceDurationEdit")
            tbFirstSchedule = GVR.FindControl("FirstScheduleEdit")
            ddlPattern = GVR.FindControl("MTNPatternEdit")

            If tbItemNo.Text.Trim = "" Then
                lbstatus.Text = MessageDlg("Maintenance Item must have value")
                tbItemNo.Focus()
                Exit Sub
            End If
            If tbJob.Text.Trim = "" Then
                lbstatus.Text = MessageDlg("Job must have value")
                tbJob.Focus()
                Exit Sub
            End If
            If tbJobName.Text.Trim = "" Then
                lbstatus.Text = MessageDlg("Job Name must have value")
                tbJobName.Focus()
                Exit Sub
            End If
            If tbFirstSchedule.SelectedValue Is Nothing Then
                lbstatus.Text = MessageDlg("First Schedule must have value")
                tbFirstSchedule.Focus()
                Exit Sub
            End If

            If tbItemNo.Text + "|" + tbJob.Text <> ViewState("Maintenance") + "|" + ViewState("Job") Then
                If SQLExecuteScalar("SELECT Maintenanceitem, Job FROM MsmaintenanceItemJob WHERE MaintenanceItem = " + QuotedStr(tbItemNo.Text) + " AND Job = " + QuotedStr(tbJob.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lbstatus.Text = "Maintenance Item " + lbItemName.Text + ", Job " + tbJob.Text + " has already been exist"
                    Exit Sub
                End If
            End If

            Dim FirstSchedule As String
            If Not tbFirstSchedule.IsNull Then
                FirstSchedule = (Format(tbFirstSchedule.SelectedValue, "yyyy-MM-dd"))
            Else
                FirstSchedule = ""
            End If

            SQLString = "Update MsMaintenanceItemJob set MaintenanceItem = " + QuotedStr(tbItemNo.Text) + _
            ", Job =" + QuotedStr(tbJob.Text) + ", JobName =" + QuotedStr(tbJobName.Text) + _
            ", JobDescription = " + QuotedStr(tbJobDesc.Text) + _
            ", FgActive = " + QuotedStr(ddlFgActive.SelectedValue) + _
            ", UserDate = GetDate()" + _
            ", FirstSchedule = " + QuotedStr(FirstSchedule) + _
            ", MTNPattern = " + QuotedStr(ddlPattern.SelectedValue) + _
            "  WHERE MaintenanceItem = " + QuotedStr(ViewState("Maintenance")) + " AND Job = " + QuotedStr(ViewState("Job"))

            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            'bindDataGrid()
            bindDataGridSearch()

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
            'bindDataGrid()
            bindDataGridSearch()
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
        Dim ddlUnit As DropDownList
        Dim tbMaterial, tbQty As TextBox
        Dim lbMaterialName As Label
        Try
            If e.CommandName = "Insert" Then
                tbMaterial = DataGridDt.FooterRow.FindControl("MaterialAdd")
                tbQty = DataGridDt.FooterRow.FindControl("QtyAdd")
                ddlUnit = DataGridDt.FooterRow.FindControl("UnitAdd")
                lbMaterialName = DataGridDt.FooterRow.FindControl("MaterialNameAdd")

                If tbMaterial.Text.Trim = "" Then
                    lbstatus.Text = "Spare Part Must Be filled."
                    tbMaterial.Focus()
                    Exit Sub
                End If
                If CFloat(tbQty.Text.Trim) <= 0 Then
                    lbstatus.Text = "Qty Must Be filled."
                    tbQty.Focus()
                    Exit Sub
                End If

                If SQLExecuteScalar("SELECT MaintenanceItem, Job, Material FROM MsMaintenanceItemJobRM WHERE MaintenanceItem = " + QuotedStr(tbMaintenanceCode.Text) + " AND Job = " + QuotedStr(tbJobCode.Text) + " AND Material = " + QuotedStr(tbMaterial.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lbstatus.Text = "Maintenance Item " + tbMaintenanceName.Text + ",  Job " + tbJobNameV.Text + ", Material " + lbMaterialName.Text + " has already exists"
                    Exit Sub
                End If

                'insert the new entry
                SQLString = "INSERT INTO MsMaintenanceItemJobRM (MaintenanceItem, Job, Material, Qty, Unit, UserId, UserDate)" + _
                        "Values ( " + QuotedStr(tbMaintenanceCode.Text) + ", " + QuotedStr(tbJobCode.Text) + ", " + QuotedStr(tbMaterial.Text) + ", " + _
                        tbQty.Text.Replace(",", "") + ", " + QuotedStr(ddlUnit.SelectedValue) + ", " + _
                        QuotedStr(ViewState("UserId").ToString) + ", getDate())"

                SQLString = ChangeQuoteNull(SQLString)
                SQLString = SQLString.Replace("'1900-01-01'", "NULL")
                SQLString = SQLString.Replace("'0001-01-01'", "NULL")
                SQLString = SQLString.Replace("''", "NULL")
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                bindDataGridDt()
            ElseIf e.CommandName = "btnMaterialAdd" Or e.CommandName = "btnMaterialEdit" Then
                Dim FieldResult As String
                Session("DBConnection") = ViewState("DBConnection")
                If e.CommandName = "btnMaterialAdd" Then
                    Session("filter") = "Select * FROM VMsMaterial where Fg_Active = 'Y' "
                    ViewState("Sender") = "btnMaterialAdd"
                Else
                    Session("filter") = "Select * FROM VMsMaterial where Fg_Active = 'Y' "
                    ViewState("Sender") = "btnMaterialEdit"
                End If

                FieldResult = "Material_Code, Material_Name, Specification, Unit_Order"
                Session("Column") = FieldResult.Split(",")
                AttachScript("OpenSearchDlg();", Page, Me.GetType())
            End If
        Catch ex As Exception
            lbstatus.Text = lbstatus.Text + "DataGridDt_RowCommand Error:" & ex.ToString
        End Try
    End Sub

    Protected Sub DataGridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGridDt.RowDeleting
        Dim Material As Label
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            Material = DataGridDt.Rows(e.RowIndex).FindControl("Material")
            SQLExecuteNonQuery("Delete from MsMaintenanceItemJobRM where MaintenanceItem = " + QuotedStr(tbMaintenanceCode.Text) + " AND Job = " + QuotedStr(tbJobCode.Text) + " AND Material = " + QuotedStr(Material.Text), ViewState("DBConnection").ToString)
            bindDataGridDt()
        Catch ex As Exception
            lbstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub DataGridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGridDt.RowEditing
        Dim obj As GridViewRow
        Dim tbMaterial, tbQty As TextBox
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGridDt.EditIndex = e.NewEditIndex
            DataGridDt.ShowFooter = False
            bindDataGridDt()
            obj = DataGridDt.Rows(e.NewEditIndex)
            tbMaterial = obj.FindControl("MaterialEdit")
            tbQty = obj.FindControl("QtyEdit")
            tbQty.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbMaterial.Focus()
            ViewState("Material") = tbMaterial.Text
        Catch ex As Exception
            lbstatus.Text = "DataGridDt_RowEditing exception : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGridDt_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGridDt.RowUpdating
        Dim SQLString As String
        Dim lbMaterialName As Label
        Dim ddlUnit As DropDownList
        Dim tbMaterial, tbQty As TextBox
        Dim GVR As GridViewRow
        Try
            GVR = DataGridDt.Rows(e.RowIndex)
            lbMaterialName = GVR.FindControl("MaterialNameEdit")
            ddlUnit = GVR.FindControl("UnitEdit")
            tbMaterial = GVR.FindControl("MaterialEdit")
            tbQty = GVR.FindControl("QtyEdit")


            If tbMaterial.Text.Trim = "" Then
                lbstatus.Text = "Spare Part Must Be filled."
                tbMaterial.Focus()
                Exit Sub
            End If

            If CFloat(tbQty.Text.Trim) <= 0 Then
                lbstatus.Text = "Qty Must Be filled."
                tbQty.Focus()
                Exit Sub
            End If

            If tbMaintenanceCode.Text + "|" + tbJobCode.Text + "|" + tbMaterial.Text <> tbMaintenanceCode.Text + "|" + tbJobCode.Text + "|" + ViewState("Material") Then
                If SQLExecuteScalar("SELECT MaintenanceItem, Job, Material FROM MsMaintenanceItemJobRM WHERE MaintenanceItem = " + QuotedStr(tbMaintenanceCode.Text) + " AND Job = " + QuotedStr(tbJobCode.Text) + " AND Material = " + QuotedStr(tbMaterial.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lbstatus.Text = "Maintenance Item " + tbMaintenanceName.Text + ",  Job " + tbJobNameV.Text + ", Material " + lbMaterialName.Text + " has already exists"
                    Exit Sub
                End If
            End If

            SQLString = "Update MsMaintenanceItemJobRM set Material = " + QuotedStr(tbMaterial.Text) + _
            ", Qty =" + tbQty.Text.Replace(",", "") + ", Unit =" + QuotedStr(ddlUnit.SelectedValue) + _
            ", UserDate = GetDate()" + _
            "  WHERE MaintenanceItem = " + QuotedStr(tbMaintenanceCode.Text) + " AND Job = " + QuotedStr(tbJobCode.Text) + " AND Material = " + QuotedStr(ViewState("Material"))

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

    Protected Sub ItemNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dr As DataRow
        Dim ds As DataSet
        Dim MtnNo, tb As TextBox
        Dim MtnName As Label
        Dim Count As Integer
        Dim dgi As GridViewRow
        Try
            tb = sender
            If tb.ID = "ItemNoAdd" Then
                Count = DataGrid.Controls(0).Controls.Count
                dgi = DataGrid.Controls(0).Controls(Count - 2) '-1 for allowpaging = False
                MtnNo = dgi.FindControl("ItemNoAdd")
                MtnName = dgi.FindControl("ItemNameAdd")
            Else
                Count = DataGrid.EditIndex
                dgi = DataGrid.Rows(Count)
                MtnNo = dgi.FindControl("ItemNoEdit")
                MtnName = dgi.FindControl("ItemNameEdit")
            End If
            ds = SQLExecuteQuery("Select * From V_MsMaintenanceItem WHERE MTN_Section = " + QuotedStr(tbCode.Text) + " AND MTN_Item = " + QuotedStr(MtnNo.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                MtnNo.Text = ""
                MtnName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                MtnNo.Text = dr("MTN_Item").ToString
                MtnName.Text = dr("MTN_Item_Name").ToString
            End If
        Catch ex As Exception
            lbstatus.Text = "ItemNo_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub Material_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dr As DataRow
        Dim ds As DataSet
        Dim Material, tb As TextBox
        Dim MaterialName, Specification As Label
        Dim Unit As DropDownList
        Dim Count As Integer
        Dim dgi As GridViewRow
        Try
            tb = sender
            If tb.ID = "MaterialAdd" Then
                Count = DataGridDt.Controls(0).Controls.Count
                dgi = DataGridDt.Controls(0).Controls(Count - 2) '-1 for allowpaging = False
                Material = dgi.FindControl("MaterialAdd")
                MaterialName = dgi.FindControl("MaterialNameAdd")
                Specification = dgi.FindControl("SpecificationAdd")
                Unit = dgi.FindControl("UnitAdd")
            Else
                Count = DataGridDt.EditIndex
                dgi = DataGridDt.Rows(Count)
                Material = dgi.FindControl("MaterialEdit")
                MaterialName = dgi.FindControl("MaterialNameEdit")
                Specification = dgi.FindControl("SpecificationEdit")
                Unit = dgi.FindControl("UnitEdit")
            End If
            ds = SQLExecuteQuery("Select * From VMsMaterial WHERE Material_Code = " + QuotedStr(Material.Text) + " AND Fg_Active = 'Y' ", ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                Material.Text = ""
                MaterialName.Text = ""
                Specification.Text = ""
                Unit.SelectedIndex = 0
            Else
                dr = ds.Tables(0).Rows(0)
                Material.Text = dr("Material_Code").ToString
                MaterialName.Text = dr("Material_Name").ToString
                Specification.Text = dr("Specification").ToString
                Unit.SelectedValue = dr("Unit_Order").ToString
            End If
        Catch ex As Exception
            lbstatus.Text = "Material_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSearch2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch2.Click
        Try
            DataGrid.PageIndex = 0
            DataGrid.EditIndex = -1
            DataGrid.PageSize = ddlRow.SelectedValue
            bindDataGridSearch()
        Catch ex As Exception
            lbstatus.Text = "Btn Search Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub btnExpand_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExpand.Click
        Try
            tbfilter2.Text = ""
            If pnlSearch.Visible Then
                pnlSearch.Visible = False
            Else
                pnlSearch.Visible = True
            End If
        Catch ex As Exception
            lbstatus.Text = "btn Expand Error : " + ex.ToString
        End Try
    End Sub

    Private Sub bindDataGridSearch()
        Dim StrFilter, SqlString As String
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            SqlString = "EXEC S_MsMaintenanceItemJobView " + QuotedStr(tbCode.Text) + ", " + QuotedStr(StrFilter)
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "MaintenanceItem ASC"
                ViewState("SortOrder") = "ASC"
            End If

            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)

            Dim dt As DataTable
            dt = SQLExecuteQuery(SqlString, ViewState("DBConnection").ToString).Tables(0)

            If dt.Rows.Count = 0 Then
                lbstatus.Text = "No Data"
                'DataGrid.Visible = False
            Else
                'DataGrid.Visible = True
            End If

        Catch ex As Exception
            lbstatus.Text = lbstatus.Text + "bindDataGridSearch Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Protected Sub ddlRow_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlRow.SelectedIndexChanged
        Try
            DataGrid.PageIndex = 0
            DataGrid.EditIndex = -1
            DataGrid.PageSize = ddlRow.SelectedValue
            'bindDataGrid()
            bindDataGridSearch()
        Catch ex As Exception
            lbstatus.Text = "ddlRow_SelectedIndexChanged =" + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub btnCopy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCopy.Click
        Try
            If PanelCopy.Visible Then
                PanelCopy.Visible = False
            Else
                PanelCopy.Visible = True
            End If

            tbItemFrom.Text = ""
            tbItemNameFrom.Text = ""
            tbItemTo.Text = ""
            tbItemNameTo.Text = ""
        Catch ex As Exception
            lbstatus.Text = "btnCopy_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnItemFrom_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnItemFrom.Click
        Dim ResultField As String
        Try
            If tbItemTo.Text.Trim <> "" Then
                Session("filter") = "SELECT MTN_Item, MTN_Item_Name, MTN_Type, MTN_Type_Name, MTN_Section, MTN_Section_Name, Account, AccountName FROM V_MsMaintenanceItem WHERE MTN_Item <> " + QuotedStr(tbItemTo.Text)
            Else
                Session("filter") = "SELECT MTN_Item, MTN_Item_Name, MTN_Type, MTN_Type_Name, MTN_Section, MTN_Section_Name, Account, AccountName FROM V_MsMaintenanceItem "
            End If
            ResultField = "MTN_Item, MTN_Item_Name"

            Session("DBConnection") = ViewState("DBConnection")
            ViewState("Sender") = "btnItemFrom"
            Session("Column") = ResultField.Split(",")

            If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenPopup();", True)
            End If
        Catch ex As Exception
            lbstatus.Text = "btnItemFrom_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnItemTo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnItemTo.Click
        Dim ResultField As String
        Try
            If tbItemFrom.Text.Trim <> "" Then
                Session("filter") = "SELECT MTN_Item, MTN_Item_Name, MTN_Type, MTN_Type_Name, MTN_Section, MTN_Section_Name, Account, AccountName FROM V_MsMaintenanceItem WHERE MTN_Item <> " + QuotedStr(tbItemFrom.Text)
            Else
                Session("filter") = "SELECT MTN_Item, MTN_Item_Name, MTN_Type, MTN_Type_Name, MTN_Section, MTN_Section_Name, Account, AccountName FROM V_MsMaintenanceItem "
            End If
            ResultField = "MTN_Item, MTN_Item_Name"

            Session("DBConnection") = ViewState("DBConnection")
            ViewState("Sender") = "btnItemTo"
            Session("Column") = ResultField.Split(",")

            If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenPopup();", True)
            End If
        Catch ex As Exception
            lbstatus.Text = "btnItemFrom_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbItemFrom_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbItemFrom.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            If tbItemTo.Text.Trim <> "" Then
                ds = SQLExecuteQuery("SELECT MTN_Item, MTN_Item_Name, MTN_Type, MTN_Type_Name, MTN_Section, MTN_Section_Name, Account, AccountName FROM V_MsMaintenanceItem WHERE MTN_Item = " + QuotedStr(tbItemFrom.Text) + " AND MTN_Item <> " + QuotedStr(tbItemTo.Text), ViewState("DBConnection").ToString)
            Else
                ds = SQLExecuteQuery("SELECT MTN_Item, MTN_Item_Name, MTN_Type, MTN_Type_Name, MTN_Section, MTN_Section_Name, Account, AccountName FROM V_MsMaintenanceItem WHERE MTN_Item = " + QuotedStr(tbItemFrom.Text), ViewState("DBConnection").ToString)
            End If

            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbItemFrom.Text = ""
                tbItemNameFrom.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbItemFrom.Text = dr("MTN_Item").ToString
                tbItemNameFrom.Text = dr("MTN_Item_Name").ToString
            End If            
        Catch ex As Exception
            lbstatus.Text = "tbItemFrom_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbItemTo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbItemTo.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            If tbItemFrom.Text.Trim <> "" Then
                ds = SQLExecuteQuery("SELECT MTN_Item, MTN_Item_Name, MTN_Type, MTN_Type_Name, MTN_Section, MTN_Section_Name, Account, AccountName FROM V_MsMaintenanceItem WHERE MTN_Item = " + QuotedStr(tbItemTo.Text) + " AND MTN_Item <> " + QuotedStr(tbItemFrom.Text), ViewState("DBConnection").ToString)
            Else
                ds = SQLExecuteQuery("SELECT MTN_Item, MTN_Item_Name, MTN_Type, MTN_Type_Name, MTN_Section, MTN_Section_Name, Account, AccountName FROM V_MsMaintenanceItem WHERE MTN_Item = " + QuotedStr(tbItemTo.Text), ViewState("DBConnection").ToString)
            End If

            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbItemTo.Text = ""
                tbItemNameTo.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbItemTo.Text = dr("MTN_Item").ToString
                tbItemNameTo.Text = dr("MTN_Item_Name").ToString
            End If
        Catch ex As Exception
            lbstatus.Text = "tbItemTo_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCopyOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCopyOK.Click
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            If tbItemFrom.Text.Trim = "" Then
                lbstatus.Text = MessageDlg("From Maintenance Item must filled!")
                tbItemFrom.Focus()
                Exit Sub
            End If

            If tbItemTo.Text.Trim = "" Then
                lbstatus.Text = MessageDlg("To Maintenance Item must filled!")
                tbItemTo.Focus()
                Exit Sub
            End If

            ds = SQLExecuteQuery("EXEC S_MsMTNItemJobCopy " + QuotedStr(ViewState("UserId")) + ", " + QuotedStr(tbItemFrom.Text) + ", " + QuotedStr(tbItemTo.Text), ViewState("DBConnection").ToString)

            lbstatus.Text = MessageDlg("Copy Success!")

            'bindDataGrid()
            bindDataGridSearch()
        Catch ex As Exception
            lbstatus.Text = "btnCopyOK_Click Error : " + ex.ToString
        End Try
    End Sub
End Class
