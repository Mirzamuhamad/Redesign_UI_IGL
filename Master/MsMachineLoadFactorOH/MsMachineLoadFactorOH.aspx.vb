Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data

Partial Class MsMachineLoadFactorOH_MsMachineLoadFactorOH
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            InitProperty()
            'tbLaborRate.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbCapacity.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbHour.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbManPower.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbFactorOH.Attributes.Add("OnKeyDown", "return PressNumeric();")
            'tbOHRate.Attributes.Add("OnKeyDown", "return PressNumeric();")
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            'bindDataGrid()

        End If

        'dsUnit.ConnectionString = ViewState("DBConnection")

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
            SqlString = "Select Product, ProductName, Process, processName, ProductOutput, ProductOutputName, Capacity, Hours, ManPower, FactorOH, LaborRate, OHRate from V_MsProductProcessRate " + StrFilter
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "Product Asc "
            End If
            'tbLaborRate.Text = "0"
            tbCapacity.Text = "0"
            tbHour.Text = "0"
            tbManPower.Text = "0"
            tbFactorOH.Text = "0"
            'tbOHRate.Text = "0"
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("PrintType") = "Print"
            Session("DBConnection") = ViewState("DBConnection")
            Session("SelectCommand") = "S_FormPrintMaster6 'V_MsProductProcessRate','Product','ProductName','processName','ProductOutputName','LaborRate','OHRate','Capacity Load Factor File','Product Code','Product Name','Process','Product Output','Qty/Man Hour','Qty/Machine Hour'," + QuotedStr(StrFilter) + "," + QuotedStr(ViewState("UserId"))
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster6.frx"
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
        Dim txt, LoadFactor, ManPower As TextBox
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()
            obj = DataGrid.Rows(e.NewEditIndex)
            txt = obj.FindControl("LoadRateEdit")
            txt.Focus()

            ManPower = DataGrid.Rows(e.NewEditIndex).FindControl("ManPowerEdit")
            ManPower.Attributes.Add("OnKeyDown", "return PressNumeric();")
            LoadFactor = DataGrid.Rows(e.NewEditIndex).FindControl("LoadFactorEdit")
            LoadFactor.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim dbLaborRate, dbOHRate As TextBox
        Dim lbCode, lbProcess As Label

        Try
            lbCode = DataGrid.Rows(e.RowIndex).FindControl("ProductEdit")
            lbProcess = DataGrid.Rows(e.RowIndex).FindControl("ProcessEdit")
            dbLaborRate = DataGrid.Rows(e.RowIndex).FindControl("LaborRateEdit")
            dbOHRate = DataGrid.Rows(e.RowIndex).FindControl("OHRateEdit")

            If CFloat(dbOHRate.Text.Replace(",", "")) <= 0 Then
                lstatus.Text = MessageDlg("OH Rate Must Have Value")
                dbOHRate.Focus()
                Exit Sub
            End If
            If CFloat(dbLaborRate.Text.Replace(",", "")) <= 0 Then
                lstatus.Text = MessageDlg("Labor Rate Must Have Value")
                dbLaborRate.Focus()
                Exit Sub
            End If

            SQLString = "EXEC S_MsProductProcessRateUpdate " + QuotedStr(lbCode.Text) + _
            ", " + QuotedStr(lbProcess.Text) + _
            ", " + dbLaborRate.Text.Replace(",", "") + _
            ", " + dbOHRate.Text.Replace(",", "")
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


    Protected Sub btnApply_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApply.Click
        Try
            If CFloat(tbCapacity.Text) < 0 Or tbCapacity.Text = "" Then
                lstatus.Text = MessageDlg("Capacity must have value")
                tbCapacity.Focus()
                Exit Sub
            End If
            If CFloat(tbHour.Text) < 0 Or tbHour.Text = "" Then
                lstatus.Text = MessageDlg("Hour must have value")
                tbHour.Focus()
                Exit Sub
            End If
            If CFloat(tbManPower.Text) < 0 Or tbManPower.Text = "" Then
                lstatus.Text = MessageDlg("Man Power must have value")
                tbManPower.Focus()
                Exit Sub
            End If
            If CFloat(tbFactorOH.Text) < 0 Or tbFactorOH.Text = "" Then
                lstatus.Text = MessageDlg("Factor OH must have value")
                tbFactorOH.Focus()
                Exit Sub
            End If

            bindDataSetLF()
        Catch ex As Exception
            Throw New Exception("btnProcess_Click Error : " + ex.ToString)
        End Try
    End Sub
    Private Sub bindDataSetLF()
        Try
            Dim GVR As GridViewRow
            Dim CB As CheckBox
            Dim lbProduct, lbProcess As Label
            Dim HaveSelect As Boolean
            'Dim Row As DataRow
            Dim SQLString As String
            HaveSelect = False
            For Each GVR In DataGrid.Rows
                CB = GVR.FindControl("cbSelect")
                lbProduct = GVR.FindControl("Product")
                lbProcess = GVR.FindControl("Process")
                If CB.Checked Then
                    HaveSelect = True
                    'SQLString = "Update MsMachine set LoadFactorLB = " + tbLoadFactor.Text.Replace(",", "") + " WHERE MachineCode = " + QuotedStr(lb.Text.Trim)
                    SQLString = "EXEC S_MsProductProcessRateUpdate " + QuotedStr(lbProduct.Text) + _
                    ", " + QuotedStr(lbProcess.Text) + _
                    ", " + tbCapacity.Text.Replace(",", "") + _
                    ", " + tbHour.Text.Replace(",", "") + _
                    ", " + tbManPower.Text.Replace(",", "") + _
                    ", " + tbFactorOH.Text.Replace(",", "")

                    SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                End If
            Next
            If HaveSelect = False Then
                lstatus.Text = "Please Check Machine for Setting Load Factor Over Head"
                Exit Sub
            Else
                lstatus.Text = "Setting Labor Rate Success for Selected Machine"
            End If
            bindDataGrid()
        Catch ex As Exception
            Throw New Exception("bindDataSetLF Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub bindDataSetOHRate()
        Try
            Dim GVR As GridViewRow
            Dim CB As CheckBox
            Dim lbProduct, lbProcess As Label
            Dim HaveSelect As Boolean
            'Dim Row As DataRow
            Dim SQLString As String
            HaveSelect = False
            For Each GVR In DataGrid.Rows
                CB = GVR.FindControl("cbSelect")
                lbProduct = GVR.FindControl("Product")
                lbProcess = GVR.FindControl("Process")
                If CB.Checked Then
                    HaveSelect = True
                    SQLString = "EXEC S_MsProductProcessRateUpdate " + QuotedStr(lbProduct.Text) + _
                    ", " + QuotedStr(lbProcess.Text) + _
                    ", " + tbCapacity.Text.Replace(",", "") + _
                    ", " + tbHour.Text.Replace(",", "") + _
                    ", " + tbManPower.Text.Replace(",", "") + _
                    ", " + tbFactorOH.Text.Replace(",", "")
                    SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                End If
            Next
            If HaveSelect = False Then
                lstatus.Text = "Please Check Machine for Setting OH Rate"
                Exit Sub
            Else
                lstatus.Text = "Setting OH Rate Success for Selected OH Rate"
            End If
            bindDataGrid()
        Catch ex As Exception
            Throw New Exception("bindDataSetOHRate Error : " + ex.ToString)
        End Try
    End Sub

    'Protected Sub btnApply2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApply2.Click
    '    Try
    '        If CFloat(tbCapacity.Text) < 0 Or tbCapacity.Text = "" Then
    '            lstatus.Text = MessageDlg("Capacity must have value")
    '            tbCapacity.Focus()
    '            Exit Sub
    '        End If
    '        If CFloat(tbHour.Text) < 0 Or tbHour.Text = "" Then
    '            lstatus.Text = MessageDlg("Hour must have value")
    '            tbHour.Focus()
    '            Exit Sub
    '        End If
    '        If CFloat(tbManPower.Text) < 0 Or tbManPower.Text = "" Then
    '            lstatus.Text = MessageDlg("Man Power must have value")
    '            tbManPower.Focus()
    '            Exit Sub
    '        End If
    '        If CFloat(tbFactorOH.Text) < 0 Or tbFactorOH.Text = "" Then
    '            lstatus.Text = MessageDlg("Factor OH must have value")
    '            tbFactorOH.Focus()
    '            Exit Sub
    '        End If

    '        bindDataSetOHRate()
    '    Catch ex As Exception
    '        Throw New Exception("btnProcess_Click Error : " + ex.ToString)
    '    End Try
    'End Sub
End Class
