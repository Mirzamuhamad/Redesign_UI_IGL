Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data
Partial Class MsMaintenanceItem_MsMaintenanceItem
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            InitProperty()
            ViewState("SortExpression") = Nothing
            FillCombo(ddlMtnType, "SELECT MaintenanceTypeCode, MaintenanceTypeName, MTNSectionName FROM VMsMaintenanceType", True, "MaintenanceTypeCode", "MaintenanceTypeName", ViewState("DBConnection"))
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"

            ddlRow.SelectedValue = "15"

            'bindDataGrid()

            'tbItemNo.Attributes.Add("OnKeyDown", "return PressNumeric();")
        End If

        If Not Session("Result") Is Nothing Then
            'If ViewState("Sender") = "btnReff" Then
            '    tbReffCode.Text = Session("Result")(0).ToString
            '    tbItemName.Text = Session("Result")(1).ToString
            'Else
            If ViewState("Sender") = "btnAcc" Then
                tbAccount.Text = Session("Result")(0).ToString
                tbAccountName.Text = Session("Result")(1).ToString
            End If
            Session("Result") = Nothing
            ViewState("Sender") = Nothing
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


    Private Sub ClearInput()
        Try
            If tbItemNo.Enabled Then
                tbItemNo.Text = ""
            End If

            tbItemName.Text = ""
            'tbReffCode.Text = ""
            ddlMtnType.SelectedIndex = 0
            tbMtnSection.Text = ""
            tbAccount.Text = ""
            tbAccountName.Text = ""
            tbLocation.Text = ""
            tbPIC.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Input Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        Try
            DataGrid.PageIndex = 0
            DataGrid.EditIndex = -1
            DataGrid.PageSize = ddlRow.SelectedValue
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
            SqlString = "SELECT A.*, B.MaintenanceTypeName, B.MTNSectionName, C.Description FROM MsMaintenanceItem A LEFT OUTER JOIN VMsMaintenanceType B ON A.MaintenanceType = B.MaintenanceTypeCode LEFT OUTER JOIN VMsAccount C ON A.AccMtnExpense = C.Account " + StrFilter + " ORDER BY A.ItemNo "
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "ItemNo ASC"
                ViewState("SortOrder") = "ASC"
            End If

            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)

            Dim dt As DataTable
            dt = SQLExecuteQuery(SqlString, ViewState("DBConnection").ToString).Tables(0)

            If dt.Rows.Count = 0 Then
                lstatus.Text = "No Data"
                DataGrid.Visible = False
                btnAdd2.Visible = False
            Else
                DataGrid.Visible = True
                btnAdd2.Visible = True
            End If

        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try

    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim StrFilter As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("PrintType") = "Print"
            Dim SQL As String
            SQL = "S_FormPrintMaster4 'VMsMaintenanceItem ','Item_No','Item_Name','Maintenance_Type_Name','Acc_MtnExpense_Name','Maintenance Item File','Maintenance Item','Maintenance Item Name','Maintenance Type','Account'," + QuotedStr(StrFilter) + ", " + QuotedStr(ViewState("UserId"))
            SQL = SQL.Replace("ItemNo", "Item_No")
            SQL = SQL.Replace("ItemName", "Item_Name")
            SQL = SQL.Replace("ReffCode", "Reff_Code")
            SQL = SQL.Replace("MaintenanceType", "Maintenance_Type")
            SQL = SQL.Replace("MaintenanceTypeName", "Maintenance_Type_Name")
            SQL = SQL.Replace("Account", "Acc_MtnExpense")
            SQL = SQL.Replace("Description", "Acc_MtnExpense_Name")
            SQL = SQL.Replace("Maintenance_TypeName", "Maintenance_Type_Name")
            Session("SelectCommand") = SQL
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster3.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            lstatus.Text = "btn print Error = " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGrid.PageIndexChanging
        DataGrid.PageIndex = e.NewPageIndex
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
        'Dim SQLString As String
        'Dim dbCode, dbName As TextBox
        'Dim cbxWrhsGroup As DropDownList
        'Dim cbxWrhsArea As DropDownList
        'Dim cbxWrhsType As DropDownList
        'Dim cbxWrhsCondition As DropDownList
        'Dim cbxFgActive As DropDownList

        Try
            If e.CommandName = "Insert" Then
                'dbCode = DataGrid.FooterRow.FindControl("WrhsCodeAdd")
                'dbName = DataGrid.FooterRow.FindControl("WrhsNameAdd")
                'cbxWrhsGroup = DataGrid.FooterRow.FindControl("WrhsGroupAdd")
                'cbxWrhsArea = DataGrid.FooterRow.FindControl("WrhsAreaAdd")
                'cbxWrhsType = DataGrid.FooterRow.FindControl("WrhsTypeAdd")
                'cbxWrhsCondition = DataGrid.FooterRow.FindControl("WrhsConditionAdd")
                'cbxFgActive = DataGrid.FooterRow.FindControl("FgActiveAdd")

                'If dbCode.Text.Trim.Length = 0 Then
                '    lstatus.Text = " Wrhs Code must be filled."
                '    dbCode.Focus()
                '    Exit Sub
                'End If
                'If tbName.Text.Trim.Length = 0 Then
                '    lstatus.Text = " Wrhs Name must be filled."
                '    tbName.Focus()
                '    Exit Sub
                'End If

                'If SQLExecuteScalar("SELECT Wrhs_Code From VMsMaintenanceItem WHERE Wrhs_Code = " + QuotedStr(dbCode.Text), Session("DBConnection").ToString).Length > 0 Then
                '    lstatus.Text = "Warehouse " + QuotedStr(dbCode.Text) + " has already been exist"
                '    Exit Sub
                'End If

                ''insert the new entry
                'SQLString = "Insert into MsMaintenanceItem (WrhsCode, WrhsName, WrhsGroup, WrhsArea, WrhsType, WrhsCondition, FgActive, UserId, UserDate ) " + _
                '"SELECT " + QuotedStr(dbCode.Text) + ", " + QuotedStr(dbName.Text) + ", " & _
                'QuotedStr(cbxWrhsGroup.SelectedValue) + ", " + QuotedStr(cbxWrhsArea.SelectedValue) + ", " & _
                'QuotedStr(cbxWrhsType.SelectedItem.ToString) + ", " + QuotedStr(cbxWrhsCondition.SelectedValue) + ", " & _
                'QuotedStr(cbxFgActive.SelectedValue) + ", " & _
                'QuotedStr(Session("userId").ToString) + ", getDate()"
                'SQLExecuteNonQuery(SQLString)
                'bindDataGrid()
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGrid.RowDeleting
        'Dim txtID As Label
        Dim GVR As GridViewRow = DataGrid.Rows(e.RowIndex)
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            'txtID = DataGrid.Rows(e.RowIndex).FindControl("ItemNo")

            SQLExecuteNonQuery("DELETE FROM MsMaintenanceItem WHERE ItemNo = " & QuotedStr(GVR.Cells(0).Text) & " ", ViewState("DBConnection").ToString)
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            obj = DataGrid.Rows(e.NewEditIndex)

            pnlHd.Visible = False
            pnlInput.Visible = True
            tbItemNo.Enabled = False

            tbItemNo.Text = obj.Cells(0).Text
            tbItemName.Text = obj.Cells(1).Text
            'tbReffCode.Text = obj.Cells(2).Text
            'BindToText(tbReffCode, obj.Cells(2).Text)
            ddlMtnType.SelectedValue = obj.Cells(2).Text
            tbMtnSection.Text = obj.Cells(4).Text
            tbAccount.Text = obj.Cells(5).Text
            tbAccountName.Text = obj.Cells(6).Text
            tbLocation.Text = obj.Cells(7).Text
            tbPIC.Text = obj.Cells(8).Text
            ViewState("State") = "Edit"
        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        'Dim SQLString As String
        'Dim dbName As TextBox
        'Dim lbCode As Label
        'Dim CbxWrhsGroup As DropDownList
        'Dim CbxWrhsArea As DropDownList
        'Dim CbxWrhsType As DropDownList
        'Dim CbxFgActive As DropDownList

        'Try
        '    lbCode = DataGrid.Rows(e.RowIndex).FindControl("WrhsCodeEdit")
        '    dbName = DataGrid.Rows(e.RowIndex).FindControl("WrhsNameEdit")
        '    CbxWrhsGroup = DataGrid.Rows(e.RowIndex).FindControl("WrhsGroupEdit")
        '    CbxWrhsArea = DataGrid.Rows(e.RowIndex).FindControl("WrhsAreaEdit")
        '    CbxWrhsType = DataGrid.Rows(e.RowIndex).FindControl("WrhsTypeEdit")
        '    CbxFgActive = DataGrid.Rows(e.RowIndex).FindControl("FgActiveEdit")

        '    If dbName.Text.Trim.Length = 0 Then
        '        lstatus.Text = " Wrhs Name must be filled."
        '        dbName.Focus()
        '        Exit Sub
        '    End If

        '    SQLString = "Update MsMaintenanceItem set WrhsName= " + QuotedStr(dbName.Text) + "," & _
        '    "WrhsGroup = " + QuotedStr(CbxWrhsGroup.SelectedValue) & _
        '    "WrhsArea = " + QuotedStr(CbxWrhsArea.SelectedValue) & _
        '    "WrhsType = " + QuotedStr(CbxWrhsType.SelectedItem.ToString) & _
        '    "FgActive = " + QuotedStr(CbxFgActive.SelectedValue) & _
        '    " where WrhsCode = " & QuotedStr(lbCode.Text)

        '    SQLExecuteNonQuery(SQLString)

        '    DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
        '    DataGrid.EditIndex = -1
        '    bindDataGrid()

        'Catch ex As Exception
        '    lstatus.Text = lstatus.Text + "DataGrid_Update Error: " & ex.ToString
        'End Try
    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click, btnAdd2.Click
        Try
            pnlHd.Visible = False
            pnlInput.Visible = True
            ViewState("State") = "Insert"
            tbItemNo.Enabled = True
            ClearInput()
        Catch ex As Exception
            lstatus.Text = "btn Add Error : " + ex.ToString
        End Try
    End Sub
    Private Function cekInput() As Boolean
        Try
            'If IsNumeric(tbItemNo.Text.Replace(",", "")) = 0 Then
            '    lstatus.Text = MessageDlg("Item No must be in numeric.")
            '    tbItemNo.Focus()
            '    Exit Function
            'End If
            If tbItemNo.Text = "" Then
                lstatus.Text = MessageDlg("Item No must be filled.")
                tbItemNo.Focus()
                Exit Function
            End If
            If tbItemName.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("Item Name must be filled.")
                tbItemName.Focus()
                Return False
            End If
            If ddlMtnType.SelectedValue.Trim = "" Then
                lstatus.Text = MessageDlg("Maintenance Type must be filled.")
                ddlMtnType.Focus()
                Return False
            End If
            If tbAccount.Text.Trim = "" Then
                lstatus.Text = MessageDlg("Account must be filled.")
                tbAccount.Focus()
                Return False
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Input Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim SqlString As String
        Try
            If cekInput() = False Then
                Exit Sub
            End If

            If ViewState("State") = "Insert" Then
                If SQLExecuteScalar("SELECT ItemNo FROM MsMaintenanceItem WHERE ItemNo = " + QuotedStr(tbItemNo.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Item No " + tbItemNo.Text + " has already been exist"
                    Exit Sub
                End If

                SqlString = "INSERT INTO MsMaintenanceItem (ItemNo, ItemName, MaintenanceType, AccMtnExpense, UserId, UserDate, Location, PIC ) " + _
                "SELECT " + QuotedStr(tbItemNo.Text) + ", " + QuotedStr(tbItemName.Text) + ", " & _
                QuotedStr(ddlMtnType.SelectedValue) + ", " & _
                QuotedStr(tbAccount.Text) + ", " & _
                QuotedStr(ViewState("UserId").ToString) + ", GetDate(), " + QuotedStr(tbLocation.Text) + ", " + QuotedStr(tbPIC.Text)
            Else
                SqlString = "UPDATE MsMaintenanceItem SET ItemName= " + QuotedStr(tbItemName.Text) & _
                            ", MaintenanceType = " + QuotedStr(ddlMtnType.SelectedValue) & _
                            ", AccMtnExpense = " + QuotedStr(tbAccount.Text) & _
                            ", Location = " + QuotedStr(tbLocation.Text) & _
                            ", PIC = " + QuotedStr(tbPIC.Text) & _
                            " WHERE ItemNo = " + QuotedStr(tbItemNo.Text)
            End If
            SQLExecuteNonQuery(SqlString, ViewState("DBConnection").ToString)
            bindDataGrid()
            pnlInput.Visible = False
            pnlHd.Visible = True
        Catch ex As Exception
            lstatus.Text = "Btn Save Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Try
            ClearInput()
            tbItemName.Focus()
        Catch ex As Exception
            lstatus.Text = "Btn Reset Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            pnlInput.Visible = False
            pnlHd.Visible = True
        Catch ex As Exception
            lstatus.Text = "Btn Cancel Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub btnReff_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReff.Click
    '    Dim ResultField As String
    '    Try
    '        Session("DBConnection") = ViewState("DBConnection")
    '        Session("filter") = "SELECT * FROM V_GetMaintenanceItem "
    '        ResultField = "Reff_Code, Maintenance_item"
    '        ViewState("Sender") = "btnReff"
    '        Session("Column") = ResultField.Split(",")
    '        AttachScript("OpenSearchDlg();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lstatus.Text = "btn Reff Click Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub btnAcc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAcc.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT Account, Description FROM VMsAccount"
            ResultField = "Account, Description"
            ViewState("Sender") = "btnAcc"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lstatus.Text = "btn Account Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccount.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("SELECT Account, Description FROM VMsAccount WHERE Account = " + QuotedStr(tbAccount.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccount.Text = ""
                tbAccountName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbAccount.Text = dr("Account").ToString
                tbAccountName.Text = dr("Description").ToString
            End If

        Catch ex As Exception
            lstatus.Text = "tbAccount_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbItemName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbItemName.TextChanged
        'Dim dr As DataRow
        'Dim ds As DataSet
        'Try
        '    ds = SQLExecuteQuery("SELECT Reff_Code, Maintenance_Item FROM V_GetMaintenanceItem WHERE Maintenance_Item = " + QuotedStr(tbItemName.Text), ViewState("DBConnection").ToString)
        '    If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
        '        tbReffCode.Text = ""
        '    Else
        '        dr = ds.Tables(0).Rows(0)
        '        tbReffCode.Text = dr("Reff_Code").ToString
        '        tbItemName.Text = dr("Maintenance_Item").ToString
        '    End If

        'Catch ex As Exception
        '    lstatus.Text = "tbItemName_TextChanged Error : " + ex.ToString
        'End Try
    End Sub

    Protected Sub ddlMtnType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMtnType.SelectedIndexChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("SELECT MaintenanceTypeCode, MaintenanceTypeName, MTNSectionCode, MTNSectionName FROM VMsMaintenanceType WHERE MaintenanceTypeCode = " + QuotedStr(ddlMtnType.SelectedValue), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbMtnSection.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbMtnSection.Text = dr("MTNSectionName").ToString
            End If

        Catch ex As Exception
            lstatus.Text = "ddlMtnType_SelectedIndexChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlRow_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlRow.SelectedIndexChanged
        Try
            DataGrid.PageIndex = 0
            DataGrid.EditIndex = -1
            DataGrid.PageSize = ddlRow.SelectedValue
            bindDataGrid()
        Catch ex As Exception
            lstatus.Text = "ddlRow_SelectedIndexChanged =" + vbCrLf + ex.ToString
        End Try
    End Sub
End Class
