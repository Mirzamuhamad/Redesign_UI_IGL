Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data
Imports System.IO

Partial Class MsMachineLoadFactorLB_MsMachineLoadFactorLB
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            InitProperty()
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            'bindDataGrid()
            SQLExecuteNonQuery("EXEC S_PDProductionSchedule " + QuotedStr(ViewState("UserId")), ViewState("DBConnection").ToString)
            tbDeliveryRevDays.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbDeliveryRevDate.SelectedDate = ViewState("ServerDate")
            tbDeliveryRevDays.Text = "0"
            tbProdDate.SelectedDate = ViewState("ServerDate")
            tbProdDays.Text = "0"
            FillCombo(ddlMachine, "EXEC S_GetMachineForProdPlan", True, "Machine_Code", "Machine_Name", ViewState("DBConnection"))
            Dim dt As DataTable
            dt = SQLExecuteQuery("Select RptTitle, MenuName, COALESCE(MenuParam,'') AS MenuParam, COALESCE(FastRpt,'') AS FastRpt, COALESCE(ReportType,'') AS ReportType, COALESCE(ReportType2,'') AS ReportType2, COALESCE(FgPrintValue,'N') AS FgPrintValue, COALESCE(FgForceNewPage,'N') AS FgForceNewPage from MsMenu WHERE MenuId = " + QuotedStr(Request.QueryString("ContainerId").ToString), ViewState("DBConnection").ToString).Tables(0)
            lblTitle.Text = dt.Rows(0)("MenuName").ToString

        End If

        dsMachine.ConnectionString = ViewState("DBConnection")

        If Not Session("Result") Is Nothing Then

            Session("Result") = Nothing
            ViewState("Sender") = Nothing
            Session("filter") = Nothing
            Session("Column") = Nothing
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
    Protected Sub cbSelectHd_CheckedChanged1(ByVal sender As Object, ByVal e As System.EventArgs)
        CheckAllDt(DataGrid, sender)
    End Sub
    Public Sub CheckAllDt(ByRef gd As GridView, ByVal sender As CheckBox)
        Dim cb, cbselek As CheckBox
        Dim GRW As GridViewRow
        Try
            cb = sender
            For Each GRW In gd.Rows
                cbselek = GRW.FindControl("cbSelect")
                cbselek.Checked = cb.Checked
                If cb.Checked = False Then
                    'btnGetSetZero.Visible = True
                End If
            Next
        Catch ex As Exception
            Throw New Exception("Check All Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        Try
            bindDataGrid()
            'tbFilter.Text = ""
            'tbfilter2.Text = ""
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
            SqlString = "Select * from V_PDProductionPlanView " + StrFilter
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "ReffType, Reference, Product, DeliveryDate, ProductionDate Asc "
            End If
            tbDeliveryRevDays.Text = "0"
            tbDeliveryRevDate.SelectedDate = ViewState("ServerDate")

            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub
    Public Function GenerateFilterMs2(ByVal Field1 As String, ByVal Field2 As String, ByVal Filter1 As String, ByVal Filter2 As String, ByVal Notasi As String) As String
        Dim StrFilter As String
        Try
            StrFilter = ""
            If Filter1.Trim.Length > 0 Then
                If Filter2.Trim.Length > 0 Then
                    StrFilter = Field1.Replace(" ", "_") + " like '%" + Filter1 + "%' " + _
                    Notasi + " " + Field2.Replace(" ", "_") + " like '%" + Filter2 + "%'"
                Else
                    StrFilter = Field1.Replace(" ", "_") + " like '%" + Filter1 + "%'"
                End If
            Else
                StrFilter = ""
            End If

            If StrFilter <> "" Then
                StrFilter = " AND " + StrFilter
            End If
            Return StrFilter
        Catch ex As Exception
            Throw New Exception("GenerateFilterMs Error : " + ex.ToString)
        End Try
    End Function
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterMs2(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            'lstatus.Text = "EXEC S_PDProdSchedulePrint " + QuotedStr(ViewState("UserId")) + "," + QuotedStr(StrFilter)
            'Exit Sub
            Session("PrintType") = "Print"
            Session("DBConnection") = ViewState("DBConnection")
            Session("SelectCommand") = "EXEC S_PDProdSchedulePrint " + QuotedStr(ViewState("UserId")) + "," + QuotedStr(StrFilter)
            Session("ReportFile") = ".../../../Rpt/FormProductionSchedule.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)
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
    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        Dim txt As BasicFrame.WebControls.BasicDatePicker
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()
            obj = DataGrid.Rows(e.NewEditIndex)
            txt = obj.FindControl("DeliveryDateRevEdit")
            txt.Focus()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim ReffType, Reference, Product, DeliveryDate As Label
        Dim DeliveryDateRev, ProductionDate As BasicFrame.WebControls.BasicDatePicker
        Dim Machine As DropDownList

        Try
            ReffType = DataGrid.Rows(e.RowIndex).FindControl("ReffTypeEdit")
            Reference = DataGrid.Rows(e.RowIndex).FindControl("ReferenceEdit")
            Product = DataGrid.Rows(e.RowIndex).FindControl("ProductEdit")
            DeliveryDate = DataGrid.Rows(e.RowIndex).FindControl("DeliveryDateEdit")
            DeliveryDateRev = DataGrid.Rows(e.RowIndex).FindControl("DeliveryDateRevEdit")
            ProductionDate = DataGrid.Rows(e.RowIndex).FindControl("ProductionDateEdit")
            Machine = DataGrid.Rows(e.RowIndex).FindControl("MachineEdit")

            If DeliveryDateRev.Text.Trim.Length = 0 Then
                lstatus.Text = "Delivery Date Revisi Must Be filled."
                DeliveryDateRev.Focus()
                Exit Sub
            End If

            If ProductionDate.Text.Trim.Length = 0 Then
                lstatus.Text = "Production Date #1 Must Be filled."
                ProductionDate.Focus()
                Exit Sub
            End If

            SQLString = "Update PROProductionPlan set DeliveryDateRev = " + QuotedStr(Format(DeliveryDateRev.SelectedValue, "yyyy-MM-dd")) + _
            ", ProductionDate = " + QuotedStr(Format(ProductionDate.SelectedValue, "yyyy-MM-dd")) + _
            ", Machine = " + QuotedStr(Machine.SelectedValue) + _
            " where ReffType =  " + QuotedStr(ReffType.Text) + " AND Reference =  " + QuotedStr(Reference.Text) + _
            " AND Product =  " + QuotedStr(Product.Text) + " AND DeliveryDate =  " + QuotedStr(DeliveryDate.Text)
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


    Protected Sub btnApplyDelRevDate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApplyDelRevDate.Click
        Try
            If (cbDeliveryRevDate.Checked = False) And (cbDeliveryRevDays.Checked = False) Then
                lstatus.Text = "Please select one option!"
                cbDeliveryRevDate.Checked = True
                Exit Sub
            End If
            If (cbDeliveryRevDate.Checked = True) And (cbDeliveryRevDays.Checked = True) Then
                lstatus.Text = "Select one option only!"
                cbDeliveryRevDays.Checked = False
                Exit Sub
            End If
            If (cbDeliveryRevDays.Checked = True) And (CFloat(tbDeliveryRevDays.Text) <= 0) Then
                lstatus.Text = "Delivery Revision Days must have value!"
                cbDeliveryRevDays.Checked = False
                Exit Sub
            End If
            bindDataSetLF()
        Catch ex As Exception
            Throw New Exception("btnApplyDelRevDate_Click Error : " + ex.ToString)
        End Try
    End Sub
    Private Sub bindDataSetLF()
        Try
            Dim GVR As GridViewRow
            Dim CB As CheckBox
            Dim ReffType, Reference, Product, DeliveryDate As Label
            Dim HaveSelect As Boolean
            Dim DeliveryRevDate, FgRev As String
            'Dim Row As DataRow
            Dim SQLString As String
            HaveSelect = False
            DeliveryRevDate = ""
            FgRev = 0
            If (cbDeliveryRevDate.Checked = True) Then
                DeliveryRevDate = Format(tbDeliveryRevDate.SelectedValue, "yyyy-MM-dd")
                FgRev = "0"
            Else                
                FgRev = "1"
            End If
            For Each GVR In DataGrid.Rows
                CB = GVR.FindControl("cbSelect")
                ReffType = GVR.FindControl("ReffType")
                Reference = GVR.FindControl("Reference")
                Product = GVR.FindControl("Product")
                DeliveryDate = GVR.FindControl("DeliveryDate")
                If CB.Checked Then
                    HaveSelect = True
                    SQLString = "EXEC S_PDProdScheduleUpdRevDate '" + ReffType.Text + "','" + Reference.Text + "','" + _
                    Product.Text + "','" + DeliveryDate.Text + "','" + DeliveryRevDate + "'," + tbDeliveryRevDays.Text + _
                    "," + FgRev                    
                    SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                End If
            Next
            'QuotedStr(Format(DeliveryDateRev.SelectedValue, "yyyy-MM-dd")) + _
            If HaveSelect = False Then
                lstatus.Text = "Please Check Item for Setting Production Schedule"
                Exit Sub
            Else
                lstatus.Text = "Setting Delivery Date Revision Success for Selected Item"
            End If
            bindDataGrid()
        Catch ex As Exception
            Throw New Exception("bindDataSetLF Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub bindDataSetProd()
        Try
            Dim GVR As GridViewRow
            Dim CB As CheckBox
            Dim ReffType, Reference, Product, DeliveryDate As Label
            Dim HaveSelect As Boolean
            Dim ProductionDate, FgRev As String
            'Dim Row As DataRow
            Dim SQLString As String
            HaveSelect = False
            ProductionDate = ""
            If (cbProdDate.Checked = True) Then
                ProductionDate = Format(tbProdDate.SelectedValue, "yyyy-MM-dd")
                FgRev = "0"
            Else
                FgRev = "1"
            End If
            For Each GVR In DataGrid.Rows
                CB = GVR.FindControl("cbSelect")
                ReffType = GVR.FindControl("ReffType")
                Reference = GVR.FindControl("Reference")
                Product = GVR.FindControl("Product")
                DeliveryDate = GVR.FindControl("DeliveryDate")
                If CB.Checked Then
                    HaveSelect = True
                    SQLString = "EXEC S_PDProdScheduleUpdProdDate '" + ReffType.Text + "','" + Reference.Text + "','" + _
                    Product.Text + "','" + DeliveryDate.Text + "','" + ProductionDate + "'," + tbProdDays.Text + _
                    "," + FgRev
                    SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                End If
            Next
            'QuotedStr(Format(DeliveryDateRev.SelectedValue, "yyyy-MM-dd")) + _
            If HaveSelect = False Then
                lstatus.Text = "Please Check Item for Setting Production Schedule"
                Exit Sub
            Else
                lstatus.Text = "Setting Production Date Success for Selected Item"
            End If
            bindDataGrid()
        Catch ex As Exception
            Throw New Exception("bindDataSetProd Error : " + ex.ToString)
        End Try
    End Sub

    
    Protected Sub btnApplyProdDate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApplyProdDate.Click
        Try
            If (cbProdDate.Checked = False) And (cbProdDays.Checked = False) Then
                lstatus.Text = "Please select one option!"
                cbProdDate.Checked = True
                Exit Sub
            End If
            If (cbProdDate.Checked = True) And (cbProdDays.Checked = True) Then
                lstatus.Text = "Select one option only!"
                cbProdDays.Checked = False
                Exit Sub
            End If
            If (cbProdDays.Checked = True) And (CFloat(tbProdDays.Text) <= 0) Then
                lstatus.Text = "Delivery Revision Days must have value!"
                cbProdDays.Checked = False
                Exit Sub
            End If

            bindDataSetProd()
        Catch ex As Exception
            Throw New Exception("btnApplyProdDate_Click Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub bindDataSetMachine()
        Try
            Dim GVR As GridViewRow
            Dim CB As CheckBox
            Dim ReffType, Reference, Product, DeliveryDate As Label
            Dim HaveSelect As Boolean
            'Dim Row As DataRow
            Dim SQLString As String
            HaveSelect = False
            
            For Each GVR In DataGrid.Rows
                CB = GVR.FindControl("cbSelect")
                ReffType = GVR.FindControl("ReffType")
                Reference = GVR.FindControl("Reference")
                Product = GVR.FindControl("Product")
                DeliveryDate = GVR.FindControl("DeliveryDate")
                If CB.Checked Then
                    HaveSelect = True
                    SQLString = "Update PROProductionPlan set Machine = " + QuotedStr(ddlMachine.SelectedValue) + _
                    " where ReffType =  " + QuotedStr(ReffType.Text) + " AND Reference =  " + QuotedStr(Reference.Text) + _
                    " AND Product =  " + QuotedStr(Product.Text) + " AND DeliveryDate =  " + QuotedStr(DeliveryDate.Text)
                    SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                End If
            Next
            'QuotedStr(Format(DeliveryDateRev.SelectedValue, "yyyy-MM-dd")) + _
            If HaveSelect = False Then
                lstatus.Text = "Please Check Item for Setting Production Schedule"
                Exit Sub
            Else
                lstatus.Text = "Setting Machine Success for Selected Item"
            End If
            bindDataGrid()
        Catch ex As Exception
            Throw New Exception("bindDataSetMachine Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnApplyMachine_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApplyMachine.Click
        Try
            If ddlMachine.SelectedValue = "" Then
                lstatus.Text = "Machine must have value!"
                ddlMachine.Focus()
                Exit Sub
            End If
            
            bindDataSetMachine()
        Catch ex As Exception
            Throw New Exception("btnApplyMachine_Click Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Dim dt As DataTable
        Dim SQLString As String
        Try
            SQLString = "EXEC S_PDProdSchedulePrint " + QuotedStr(ViewState("UserId"))
            dt = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)
            GridExport.DataSource = dt
            GridExport.DataBind()
            ExportGridToExcel("FormProductionSchedule")
        Catch ex As Exception
            Throw New Exception("btn Export Error : " + ex.ToString)
        End Try
    End Sub
    Public Sub ExportGridToExcel(ByVal filenamevalue As String)
        Dim form As HtmlForm = New HtmlForm()
        Dim worksheetname As String
        worksheetname = Left(filenamevalue, 31)
        Dim attachment As String '= "attachment; filename=PrintDetails.xls"
        attachment = "attachment; filename=" + filenamevalue + ".xls"
        Response.ClearContent()
        Response.AddHeader("content-disposition", attachment)
        Response.ContentType = "application/ms-excel"
        'namespace (using system.IO)      
        Dim stw As StringWriter = New StringWriter()
        Dim htextw As HtmlTextWriter = New HtmlTextWriter(stw)
        GridExport.Parent.Controls.Add(form)
        form.Attributes("runat") = "server"
        form.Controls.Add(GridExport)
        Me.Controls.Add(form)
        form.RenderControl(htextw)
        Response.Write(stw.ToString())
        Response.End()
    End Sub

    Protected Sub ddlShowRecord_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlShowRecord.SelectedIndexChanged
        DataGrid.PageIndex = 0
        DataGrid.EditIndex = -1
        DataGrid.PageSize = ddlShowRecord.SelectedValue
        bindDataGrid()
    End Sub
End Class
