Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data
Imports DevExpress.Web.ASPxGridView

Partial Class Master_MsMachineMaintenanceJobRM_MsMachineMaintenanceJobRM
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()
            End If
            dsUnit.ConnectionString = ViewState("DBConnection")


            If Not IsPostBack Then
                If Not Request.QueryString("Code") Is Nothing Then
                    'FromMasterPage()
                End If
                ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
                FillCombo(ddlMaintenanceItem, "EXEC S_FindMachineMaintenanceItem " + QuotedStr(tbCode.Text), True, "MaintenanceItem", "MaintenanceItem_Name", ViewState("DBConnection"))
                FillCombo(ddlMaintenanceItemJob, "EXEC S_FindMachineMaintenanceItemJob" + QuotedStr(tbCode.Text) + "," + QuotedStr(ddlMaintenanceItem.SelectedValue), True, "Job", "Job_Name", ViewState("DBConnection"))
            End If
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnSearch" Then
                    tbCode.Text = Session("Result")(0).ToString
                    tbName.Text = Session("Result")(1).ToString
                    FillCombo(ddlMaintenanceItem, "EXEC S_FindMachineMaintenanceItem " + QuotedStr(tbCode.Text), True, "MaintenanceItem", "MaintenanceItem_Name", ViewState("DBConnection"))
                    ddlMaintenanceItem_SelectedIndexChanged(Nothing, Nothing)
                    'bindDataGrid()
                End If
                Dim tbMCode As TextBox
                Dim lbMName, lbMSpec As Label
                Dim dblUnit As DropDownList
                If ViewState("Sender") = "SearchMaterialAdd" Then
                    tbMCode = DataGrid.FooterRow.FindControl("MaterialAdd")
                    lbMName = DataGrid.FooterRow.FindControl("MaterialNameAdd")
                    lbMSpec = DataGrid.FooterRow.FindControl("SpecificationAdd")
                    dblUnit = DataGrid.FooterRow.FindControl("UnitAdd")
                    tbMCode.Text = Session("Result")(0).ToString
                    lbMName.Text = Session("Result")(1).ToString
                    lbMSpec.Text = Session("Result")(2).ToString
                    dblUnit.SelectedValue = Session("Result")(3).ToString
                    tbMCode.Focus()
                End If
                'If ViewState("Sender") = "SearchMaterialEdit" Then
                '    tbMCode = DataGrid.Rows(DataGrid.EditIndex).FindControl("MaterialEdit")
                '    lbMName = DataGrid.Rows(DataGrid.EditIndex).FindControl("MaterialNameEdit")
                '    lbMSpec = DataGrid.Rows(DataGrid.EditIndex).FindControl("SpecificationEdit")
                '    dblUnit = DataGrid.Rows(DataGrid.EditIndex).FindControl("UnitEdit")
                '    tbMCode.Text = Session("Result")(0).ToString
                '    lbMName.Text = Session("Result")(1).ToString
                '    lbMSpec.Text = Session("Result")(2).ToString
                '    dblUnit.SelectedValue = Session("Result")(3).ToString
                '    tbMCode.Focus()
                'End If
                
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
            Session("filter") = "SELECT Machine_Code, Machine_Name, Specification FROM VMsMachine"
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
            ds = SQLExecuteQuery("SELECT Machine_Code, Machine_Name, Specification FROM VMsMachine WHERE Machine_Code = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbCode.Text = ""
                tbName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbCode.Text = dr("Machine_Code").ToString
                tbName.Text = dr("Machine_Name").ToString
                
            End If
            bindDataGrid()
        Catch ex As Exception
            lbstatus.Text = "tb Code Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim SQLString As String
        Try
            SQLString = "EXEC S_MsMachineMaintenanceJobRMPrint " + QuotedStr(ViewState("UserId").ToString)
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
            SqlString = "EXEC S_MsMachineMaintenanceJobRMView " + QuotedStr(tbCode.Text) + "," + QuotedStr(ddlMaintenanceItem.SelectedValue) + "," + QuotedStr(ddlMaintenanceItemJob.SelectedValue)
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "Material DESC"
            End If
            PnlAssign.Visible = True
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)

            Dim tbQty As TextBox
            tbQty = DataGrid.FooterRow.FindControl("QtyAdd")
            tbQty.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Catch ex As Exception
            lbstatus.Text = "BindDataGrid Error: " & ex.ToString
        Finally
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
        Dim dbUnit As DropDownList
        Dim tbMaterial, tbQty As TextBox
        Try
            If e.CommandName = "Insert" Then
                dbUnit = DataGrid.FooterRow.FindControl("UnitAdd")
                tbQty = DataGrid.FooterRow.FindControl("QtyAdd")
                tbMaterial = DataGrid.FooterRow.FindControl("MaterialAdd")

                If tbCode.Text = "" Then
                    lbstatus.Text = "Machine Must Be filled."
                    tbCode.Focus()
                    Exit Sub
                End If
                If ddlMaintenanceItem.SelectedValue = "" Then
                    lbstatus.Text = "Maintenance Item Must Be filled."
                    ddlMaintenanceItem.Focus()
                    Exit Sub
                End If
                If ddlMaintenanceItemJob.SelectedValue = "" Then
                    lbstatus.Text = "Job Must Be filled."
                    ddlMaintenanceItemJob.Focus()
                    Exit Sub
                End If

                If tbMaterial.Text = "" Then
                    lbstatus.Text = "Material Must Be filled."
                    tbMaterial.Focus()
                    Exit Sub
                End If
                If tbQty.Text = "" Or CFloat(tbQty.Text) = 0 Then
                    lbstatus.Text = "Qty Must Be filled."
                    tbQty.Focus()
                    Exit Sub
                End If
                If dbUnit.SelectedValue = "" Then
                    lbstatus.Text = "Unit Must Be filled."
                    dbUnit.Focus()
                    Exit Sub
                End If

                If SQLExecuteScalar("SELECT Machine FROM MsMachineMaintenanceJobRM WHERE Machine = " + QuotedStr(tbCode.Text) + " AND MaintenanceItem = " + QuotedStr(ddlMaintenanceItem.SelectedValue) + " AND Job = " + QuotedStr(ddlMaintenanceItemJob.SelectedValue) + " AND Material = " + QuotedStr(tbMaterial.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lbstatus.Text = "Machine " + tbCode.Text + " MaintenanceItem " + QuotedStr(ddlMaintenanceItem.SelectedValue) + " Job " + QuotedStr(ddlMaintenanceItemJob.SelectedValue) + " Material " + QuotedStr(tbMaterial.Text) + " has already Exists"
                    Exit Sub
                End If
                'insert the new entry
                SQLString = "INSERT INTO MsMachineMaintenanceJobRM (Machine, MaintenanceItem, Job, Material, Qty, Unit, UserId, UserDate)" + _
                        "Values ( " + QuotedStr(tbCode.Text) + ", " + QuotedStr(ddlMaintenanceItem.SelectedValue) + ", " + QuotedStr(ddlMaintenanceItemJob.SelectedValue) + _
                        "," + QuotedStr(tbMaterial.Text) + "," + tbQty.Text.Replace(",", "") + "," + QuotedStr(dbUnit.SelectedValue) + _
                        ", " + QuotedStr(ViewState("UserId").ToString) + ", getDate() )"
                SQLString = ChangeQuoteNull(SQLString)
                SQLString = SQLString.Replace("'1900-01-01'", "NULL")
                SQLString = SQLString.Replace("'0001-01-01'", "NULL")
                SQLString = SQLString.Replace("''", "NULL")
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                bindDataGrid()
            ElseIf e.CommandName = "SearchMaterialAdd" Then
                Dim FieldResult As String

                If tbCode.Text = "" Then
                    lbstatus.Text = "Machine Must Be filled."
                    tbCode.Focus()
                    Exit Sub
                End If
                If ddlMaintenanceItem.SelectedValue = "" Then
                    lbstatus.Text = "Maintenance Item Must Be filled."
                    ddlMaintenanceItem.Focus()
                    Exit Sub
                End If
                If ddlMaintenanceItemJob.SelectedValue = "" Then
                    lbstatus.Text = "Job Must Be filled."
                    ddlMaintenanceItemJob.Focus()
                    Exit Sub
                End If

                Session("DBConnection") = ViewState("DBConnection")
                Session("filter") = "SELECT Material_Code, Material_Name, Specification, Unit, Unit_Order FROM VMsMaterial WHERE Fg_Active ='Y'"
                FieldResult = "Material_Code, Material_Name, Specification, Unit_Order"
                Session("Column") = FieldResult.Split(",")
                ViewState("Sender") = "SearchMaterialAdd"
                If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenPopup();", True)
                End If
                'ElseIf e.CommandName = "SearchMaterialEdit" Then
                '    Dim FieldResult As String
                '    Session("DBConnection") = ViewState("DBConnection")
                '    Session("filter") = "SELECT Material_Code, Material_Name, Specification, Unit, Unit_Order FROM VMsMaterial WHERE Fg_Active ='Y'"
                '    FieldResult = "Material_Code, Material_Name, Specification, Unit_Order"
                '    Session("Column") = FieldResult.Split(",")
                '    ViewState("Sender") = "SearchMaterialEdit"
                '    If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                '        Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenPopup();", True)
                '    End If
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
            EffDate = DataGrid.Rows(e.RowIndex).FindControl("Material")
            SQLExecuteNonQuery("Delete from MsMachineMaintenanceJobRM where Machine = " + QuotedStr(tbCode.Text) + " AND MaintenanceItem = " + QuotedStr(ddlMaintenanceItem.SelectedValue) + " AND Job = " + QuotedStr(ddlMaintenanceItemJob.SelectedValue) + " AND Material = " + QuotedStr(EffDate.Text), ViewState("DBConnection").ToString)
            bindDataGrid()
        Catch ex As Exception
            lbstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        'Dim tbDate As TextBox
        Dim tbAmount As TextBox
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()
            obj = DataGrid.Rows(e.NewEditIndex)
            'tbDate = obj.FindControl("MaterialEdit")

            tbCode.Enabled = False
            ddlMaintenanceItem.Enabled = False
            ddlMaintenanceItemJob.Enabled = False
            btnSearch.Visible = False

            tbAmount = obj.FindControl("QtyEdit")
            tbAmount.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbAmount.Focus()
        Catch ex As Exception
            lbstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim tbAmount As TextBox
        Dim ddlFormula As DropDownList
        Dim lbMaterial As Label
        Dim GVR As GridViewRow
        Try
            GVR = DataGrid.Rows(e.RowIndex)
            tbAmount = GVR.FindControl("QtyEdit")
            ddlFormula = GVR.FindControl("UnitEdit")
            lbMaterial = GVR.FindControl("MaterialEdit")

            If tbCode.Text = "" Then
                lbstatus.Text = "Machine Must Be filled."
                tbCode.Focus()
                Exit Sub
            End If
            If ddlMaintenanceItem.SelectedValue = "" Then
                lbstatus.Text = "Maintenance Item Must Be filled."
                ddlMaintenanceItem.Focus()
                Exit Sub
            End If
            If ddlMaintenanceItemJob.SelectedValue = "" Then
                lbstatus.Text = "Job Must Be filled."
                ddlMaintenanceItemJob.Focus()
                Exit Sub
            End If
            
            If CFloat(tbAmount.Text.Trim) <= 0 Then
                lbstatus.Text = MessageDlg("Amount must have value")
                tbAmount.Focus()
                Exit Sub
            End If

            'IfExist = SQLExecuteScalar("SELECT dbo.FormatDate(StartDate) FROM MsMachineMaintenanceJobRM WHERE Payroll = " + QuotedStr(tbCode.Text) + " AND StartDate <> '" + Format(ViewState("StartDate"), "yyyy-MM-dd") + "' AND StartDate >= " + QuotedStr(Format(tbDate.SelectedDate, "yyyy-MM-dd")), ViewState("DBConnection").ToString)

            'If IfExist.Trim.Length > 0 Then
            '    lbstatus.Text = MessageDlg("Payroll " + QuotedStr(tbCode.Text) + " has already have New Eff. Date " + QuotedStr(IfExist))
            '    tbDate.Focus()
            '    Exit Sub
            'End If
            SQLString = "Update MsMachineMaintenanceJobRM set Qty =" + tbAmount.Text.Replace(",", "") + _
            ", Unit =" + QuotedStr(ddlFormula.SelectedValue) + _
            "  WHERE Machine = " + QuotedStr(tbCode.Text) + " AND MaintenanceItem = " + QuotedStr(ddlMaintenanceItem.SelectedValue) + _
            " AND Job = " + QuotedStr(ddlMaintenanceItemJob.SelectedValue) + " AND Material = " + QuotedStr(lbMaterial.Text)
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            tbCode.Enabled = True
            ddlMaintenanceItem.Enabled = True
            ddlMaintenanceItemJob.Enabled = True
            btnSearch.Visible = True
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

    
    Protected Sub ddlMaintenanceItem_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMaintenanceItem.SelectedIndexChanged
        FillCombo(ddlMaintenanceItemJob, "EXEC S_FindMachineMaintenanceItemJob" + QuotedStr(tbCode.Text) + "," + QuotedStr(ddlMaintenanceItem.SelectedValue), True, "Job", "Job_Name", ViewState("DBConnection"))
        bindDataGrid()
    End Sub

    Protected Sub ddlMaintenanceItemJob_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMaintenanceItemJob.SelectedIndexChanged
        bindDataGrid()
    End Sub

    Protected Sub MaterialAdd_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dr As DataRow
        Dim ds As DataSet
        Dim Acc As TextBox
        Dim AccName, Spec As Label
        Dim DbUnit As DropDownList
        Dim Count As Integer
        Dim dgi As GridViewRow
        Try
            
            Count = DataGrid.Controls(0).Controls.Count
            dgi = DataGrid.Controls(0).Controls(Count - 2) '-1 for allowpaging = False
            Acc = dgi.FindControl("MaterialAdd")
            AccName = dgi.FindControl("MaterialNameAdd")
            Spec = dgi.FindControl("SpecificationAdd")
            DbUnit = dgi.FindControl("UnitAdd")

            ds = SQLExecuteQuery("SELECT Material_Code, Material_Name, Specification, Unit, Unit_Order FROM VMsMaterial WHERE Fg_Active ='Y' AND Material_Code = " + QuotedStr(Acc.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                Acc.Text = ""
                AccName.Text = ""
                Spec.Text = ""
                DbUnit.SelectedValue = ""
            Else
                dr = ds.Tables(0).Rows(0)
                Acc.Text = dr("Material_Code").ToString
                AccName.Text = dr("Material_Name").ToString
                Spec.Text = dr("Specification").ToString
                DbUnit.SelectedValue = dr("Unit_Order").ToString
            End If

        Catch ex As Exception
            lbstatus.Text = "MaterialAdd_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub MaterialEdit_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Dim dr As DataRow
    '    Dim ds As DataSet
    '    Dim Acc As TextBox
    '    Dim AccName, Spec As Label
    '    Dim dbUnit As DropDownList
    '    Dim Count As Integer
    '    Dim dgi As GridViewRow
    '    Try
    '        Count = DataGrid.EditIndex
    '        dgi = DataGrid.Rows(Count)
    '        Acc = dgi.FindControl("MaterialEdit")
    '        AccName = dgi.FindControl("MaterialNameEdit")
    '        Spec = dgi.FindControl("SpecificationEdit")
    '        dbUnit = dgi.FindControl("UnitEdit")

    '        ds = SQLExecuteQuery("SELECT Material_Code, Material_Name, Specification, Unit, Unit_Order FROM VMsMaterial WHERE Fg_Active ='Y' AND Material_Code = " + QuotedStr(Acc.Text), ViewState("DBConnection").ToString)
    '        If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
    '            Acc.Text = ""
    '            AccName.Text = ""
    '            Spec.Text = ""
    '            dbUnit.SelectedValue = ""
    '        Else
    '            dr = ds.Tables(0).Rows(0)
    '            Acc.Text = dr("Material_Code").ToString
    '            AccName.Text = dr("Material_Name").ToString
    '            Spec.Text = dr("Specification").ToString
    '            dbUnit.SelectedValue = dr("Unit_Order").ToString
    '        End If
    '    Catch ex As Exception
    '        lbstatus.Text = "MaterialEdit_TextChanged Error : " + ex.ToString
    '    End Try
    'End Sub
End Class
