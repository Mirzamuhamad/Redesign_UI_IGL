Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data
Partial Class MsWarehouse_MsWarehouse
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            InitProperty()
            ViewState("SortExpression") = Nothing
            FillCombo(ddlWrhsArea, "SELECT Wrhs_Area_Code, Wrhs_Area_Name FROM VMsWrhsArea", True, "Wrhs_Area_Code", "Wrhs_Area_Name", ViewState("DBConnection"))
            FillCombo(ddlWrhsGroup, "SELECT WrhsGroupCode, WrhsGroupName FROM VMsWrhsGroup", True, "WrhsGroupCode", "WrhsGroupName", ViewState("DBConnection"))
            FillCombo(ddlWrhsType, "EXEC S_GetWrhsType", True, "WrhsType", "WrhsType", ViewState("DBConnection"))
            'FillCombo(ddlWrhsClass, "SELECT WrhsCondition FROM VMsWrhsCondition", True, "WrhsCondition", "WrhsCondition", ViewState("DBConnection"))
            FillCombo(ddlWorkCtr, "EXEC S_GetWorkCtr", True, "WorkCtr_Code", "WorkCtr_Name", ViewState("DBConnection"))
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            Session("SelectWrhsCondition") = "SELECT WrhsCondition, FgSubled FROM MsWrhsCondition"
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
            If tbCode.Enabled Then
                tbCode.Text = ""
            End If

            tbName.Text = ""
            tbContactPerson.Text = ""
            ddlWrhsType.SelectedIndex = 0
            ddlWrhsGroup.SelectedIndex = 0
            ddlWrhsArea.SelectedIndex = 0
            ddlWorkCtr.SelectedIndex = 0
            ddlActive.SelectedIndex = 0
            filterCondition()
        Catch ex As Exception
            Throw New Exception("Clear Input Error : " + ex.ToString)
        End Try
    End Sub

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

    'Private Sub bindDataGrid()
    '    Dim StrFilter, SqlString As String
    '    Try
    '        StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
    '        SqlString = "SELECT A.*, B.WrhsGroupName, C.WrhsAreaName, W.WorkCtrName FROM MsWarehouse A INNER JOIN MsWrhsGroup B ON A.WrhsGroup = B.WrhsGroupCode INNER JOIN MsWrhsArea C ON A.WrhsArea = C.WrhsAreaCode LEFT OUTER JOIN MsWorkCtr W ON A.WorkCtr = W.WorkCtrCode " + StrFilter + " ORDER BY A.WrhsName "
    '        If ViewState("SortExpression") = Nothing Then
    '            ViewState("SortExpression") = "WrhsCode ASC"
    '            ViewState("SortOrder") = "ASC"
    '        End If
    '        BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)
    '    Catch ex As Exception
    '        lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
    '    Finally
    '    End Try

    'End Sub

    Private Sub bindDataGrid()
        Dim StrFilter, SqlString As String
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            SqlString = "SELECT A.*, B.WrhsGroupName, C.WrhsAreaName, W.WorkCtrName FROM MsWarehouse A INNER JOIN MsWrhsGroup B ON A.WrhsGroup = B.WrhsGroupCode INNER JOIN MsWrhsArea C ON A.WrhsArea = C.WrhsAreaCode LEFT OUTER JOIN MsWorkCtr W ON A.WorkCtr = W.WorkCtrCode " + StrFilter + " ORDER BY A.WrhsName "
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "WrhsCode ASC"
                ViewState("SortOrder") = "ASC"
            End If
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)
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
            Session("SelectCommand") = "S_FormPrintMaster6 'VMsWarehouse A','Wrhs_Code','Wrhs_Name','Wrhs_Group_Name','Wrhs_Area_Name','Wrhs_type', 'WorkCtrName', 'Warehouse File','Warehouse Code','Warehouse Name','Warehouse Group','Warehouse Area','Warehouse Type','Work Center'," + QuotedStr(StrFilter) + ", " + QuotedStr(ViewState("UserId"))
            'lstatus.Text = Session("SelectCommand")
            'Exit Sub
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster6.frx"
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
        Dim GVR As GridViewRow = Nothing
        Dim DDL As DropDownList
        Dim index As Integer

        Try
            If Not (e.CommandName = "Sort" Or e.CommandName = "Page") Then
                index = Convert.ToInt32(e.CommandArgument)
                GVR = DataGrid.Rows(index)
            End If
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

                'If SQLExecuteScalar("SELECT Wrhs_Code From VMsWarehouse WHERE Wrhs_Code = " + QuotedStr(dbCode.Text), Session("DBConnection").ToString).Length > 0 Then
                '    lstatus.Text = "Warehouse " + QuotedStr(dbCode.Text) + " has already been exist"
                '    Exit Sub
                'End If

                ''insert the new entry
                'SQLString = "Insert into MsWarehouse (WrhsCode, WrhsName, WrhsGroup, WrhsArea, WrhsType, WrhsCondition, FgActive, UserId, UserDate ) " + _
                '"SELECT " + QuotedStr(dbCode.Text) + ", " + QuotedStr(dbName.Text) + ", " & _
                'QuotedStr(cbxWrhsGroup.SelectedValue) + ", " + QuotedStr(cbxWrhsArea.SelectedValue) + ", " & _
                'QuotedStr(cbxWrhsType.SelectedItem.ToString) + ", " + QuotedStr(cbxWrhsCondition.SelectedValue) + ", " & _
                'QuotedStr(cbxFgActive.SelectedValue) + ", " & _
                'QuotedStr(Session("userId").ToString) + ", getDate()"
                'SQLExecuteNonQuery(SQLString)
                'bindDataGrid()
            ElseIf e.CommandName = "User" Then
                'Dim gvr As GridViewRow
                GVR = DataGrid.Rows(CInt(e.CommandArgument))
                If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenAssign('Warehouse', '" + Request.QueryString("KeyId") + "','" + GVR.Cells(0).Text + "|" + GVR.Cells(1).Text + "','AssWrhsUser');", True)
                End If
            ElseIf e.CommandName = "Location" Then
                'Dim gvr As GridViewRow
                GVR = DataGrid.Rows(CInt(e.CommandArgument))
                If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenAssign('Warehouse', '" + Request.QueryString("KeyId") + "','" + GVR.Cells(0).Text + "|" + GVR.Cells(1).Text + "','AssMsWarehouseLoc');", True)
                End If
            ElseIf e.CommandName = "Go" Then
                DDL = DataGrid.Rows(index).FindControl("ddl")
                ViewState("Nmbr") = GVR.Cells(1).Text
                If DDL.SelectedValue = "View" Then
                    ViewState("State") = "View"
                    FillTextBox(ViewState("Nmbr"))
                    BtnSave.Visible = False
                    btnReset.Visible = False
                    ModifyInput(False)
                    pnlHd.Visible = False
                    pnlInput.Visible = True
                ElseIf DDL.SelectedValue = "Edit" Then
                    ViewState("State") = "Edit"
                    FillTextBox(ViewState("Nmbr"))
                    ModifyInput(True)
                    pnlHd.Visible = False
                    pnlInput.Visible = True
                    tbCode.Enabled = False
                    ddlCustomer.Enabled = False
                    ddlBlock.Enabled = False
                    ddlSuplier.Enabled = False
                    BtnSave.Visible = True
                    btnReset.Visible = True
                ElseIf DDL.SelectedValue = "User" Then
                    If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                        Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenAssign('Warehouse', '" + Request.QueryString("KeyId") + "', '" + GVR.Cells(1).Text + "|" + GVR.Cells(2).Text + "','AssWrhsUser');", True)
                    End If
                ElseIf DDL.SelectedValue = "Location" Then
                    If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                        Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenAssign('Warehouse', '" + Request.QueryString("KeyId") + "', '" + GVR.Cells(1).Text + "|" + GVR.Cells(2).Text + "','AssMsWarehouseLoc');", True)
                    End If
                ElseIf DDL.SelectedValue = "Delete" Then
                    Try
                        If CheckMenuLevel("Delete") = False Then
                            Exit Sub
                        End If
                        SQLExecuteNonQuery("Delete from MsWarehouseLoc where WrhsCode = '" & GVR.Cells(1).Text & "'", ViewState("DBConnection").ToString)
                        SQLExecuteNonQuery("Delete from MsWrhsUser where WrhsCode = '" & GVR.Cells(1).Text & "'", ViewState("DBConnection").ToString)
                        SQLExecuteNonQuery("Delete from MsWarehouse where WrhsCode = '" & GVR.Cells(1).Text & "'", ViewState("DBConnection").ToString)
                        bindDataGrid()
                    Catch ex As Exception
                        lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
                    End Try
                End If
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGrid.RowDeleting
        Dim txtID As Label
        Dim GVR As GridViewRow = DataGrid.Rows(e.RowIndex)
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            txtID = DataGrid.Rows(e.RowIndex).FindControl("WrhsCode")
            'SQLExecuteNonQuery("Delete from MsWarehouse where WrhsCode = '" & txtID.Text & "'")
            SQLExecuteNonQuery("Delete from MsWarehouse where WrhsCode = '" & GVR.Cells(0).Text & "'", ViewState("DBConnection").ToString)
            bindDataGrid()
            'Dim dr() As DataRow
            'Dim GVR As GridViewRow = DataGrid.Rows(e.RowIndex)
            'dr = ViewState("Dt").Select("WrhsCode = " + QuotedStr(GVR.Cells(1).Text))
            'dr(0).Delete()
            'BindGridDt(ViewState("Dt"), GridDt)
            'EnableHd(GetCountRecord(ViewState("Dt")) = 0)
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
            tbCode.Enabled = False

            tbCode.Text = obj.Cells(0).Text
            tbName.Text = obj.Cells(1).Text
            BindToText(tbContactPerson, obj.Cells(8).Text)

            ddlWrhsGroup.SelectedValue = obj.Cells(2).Text
            ddlWrhsArea.SelectedValue = obj.Cells(4).Text

            ddlWrhsType.SelectedValue = obj.Cells(6).Text
            filterCondition()
            ddlWrhsClass.SelectedValue = obj.Cells(7).Text
            ddlWrhsType.SelectedValue = obj.Cells(6).Text
            'ddlActive.SelectedValue = obj.Cells(9).Text
            ddlWorkCtr.SelectedValue = obj.Cells(9).Text
            ddlCustomer.SelectedValue = obj.Cells(10).Text
            ddlBuffer.SelectedValue = obj.Cells(10).Text
            ddlBlock.SelectedValue = obj.Cells(10).Text
            ddlSuplier.SelectedValue = obj.Cells(10).Text
            ddlActive.SelectedValue = obj.Cells(10).Text
            ViewState("State") = "Edit"
        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim dbName As TextBox
        Dim lbCode As Label
        Dim CbxWrhsGroup As DropDownList
        Dim CbxWrhsArea As DropDownList
        Dim CbxWrhsType As DropDownList
        Dim CbxFgActive As DropDownList
        Dim CbxWorkCtr As DropDownList
        Dim CbxFgBuffer As DropDownList
        'Dim ddlActive As DropDownList

        Try
            lbCode = DataGrid.Rows(e.RowIndex).FindControl("WrhsCodeEdit")
            dbName = DataGrid.Rows(e.RowIndex).FindControl("WrhsNameEdit")
            CbxWrhsGroup = DataGrid.Rows(e.RowIndex).FindControl("WrhsGroupEdit")
            CbxWrhsArea = DataGrid.Rows(e.RowIndex).FindControl("WrhsAreaEdit")
            CbxWrhsType = DataGrid.Rows(e.RowIndex).FindControl("WrhsTypeEdit")
            CbxFgActive = DataGrid.Rows(e.RowIndex).FindControl("FgActiveEdit")
            CbxFgBuffer = DataGrid.Rows(e.RowIndex).FindControl("CbxFgBufferEdit")
            CbxWorkCtr = DataGrid.Rows(e.RowIndex).FindControl("WorkCtrEdit")

            If dbName.Text.Trim.Length = 0 Then
                lstatus.Text = " Wrhs Name must be filled."
                dbName.Focus()
                Exit Sub
            End If

            SQLString = "Update MsWarehouse set WrhsName= " + QuotedStr(dbName.Text) + "," & _
            "WrhsGroup = " + QuotedStr(CbxWrhsGroup.SelectedValue) & _
            "WrhsArea = " + QuotedStr(CbxWrhsArea.SelectedValue) & _
            "WrhsType = " + QuotedStr(CbxWrhsType.SelectedItem.ToString) & _
            "FgActive = " + QuotedStr(CbxFgActive.SelectedValue) & _
            "FgBuffer = " + QuotedStr(CbxFgBuffer.SelectedValue) & _
            "WorkCtr = " + QuotedStr(CbxWorkCtr.SelectedValue) & _
            " where WrhsCode = " & QuotedStr(lbCode.Text)

            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = lstatus.Text + "DataGrid_Update Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click, btnAdd2.Click
        Try
            pnlHd.Visible = False
            pnlInput.Visible = True
            ViewState("State") = "Insert"
            tbCode.Enabled = True
            ClearInput()
            ModifyInput(True)
        Catch ex As Exception
            lstatus.Text = "btn Add Error : " + ex.ToString
        End Try
    End Sub
    Private Function cekInput() As Boolean
        Try
            If tbCode.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("Warehouse Code cannot empty")
                tbCode.Focus()
                Return False
            End If
            If tbName.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("Warehouse Name cannot empty")
                tbName.Focus()
                Return False
            End If
            If ddlWrhsGroup.SelectedValue.Trim = "" Then
                lstatus.Text = MessageDlg("Warehouse Group cannot empty")
                ddlWrhsGroup.Focus()
                Return False
            End If
            If ddlWrhsArea.SelectedValue.Trim = "" Then
                lstatus.Text = MessageDlg("Warehouse Area cannot empty")
                ddlWrhsArea.Focus()
                Return False
            End If
            If ddlWrhsType.SelectedValue.Trim = "" Then
                lstatus.Text = MessageDlg("Warehouse Type cannot empty")
                ddlWrhsType.Focus()
                Return False
            End If
            If ddlWrhsClass.SelectedValue.Trim = "" Then
                lstatus.Text = MessageDlg("Warehouse Class cannot empty")
                ddlWrhsClass.Focus()
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
                If SQLExecuteScalar("SELECT Wrhs_Code From VMsWarehouse WHERE Wrhs_Code = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Warehouse " + QuotedStr(tbCode.Text) + " has already been exist"
                    Exit Sub
                End If

                SqlString = "Insert into MsWarehouse (WrhsCode, WrhsName, WrhsGroup, WrhsArea, WrhsType, WrhsCondition, FgActive, FgCustomer, FgBuffer, FgBlock, FgSuplier, ContactPerson, UserId, UserDate, WorkCtr ) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(tbName.Text) + ", " & _
                QuotedStr(ddlWrhsGroup.SelectedValue) + ", " + QuotedStr(ddlWrhsArea.SelectedValue) + ", " & _
                QuotedStr(ddlWrhsType.SelectedItem.ToString) + ", " + QuotedStr(ddlWrhsClass.SelectedValue) + ", " & _
                QuotedStr(ddlActive.SelectedValue) + "," + QuotedStr(ddlCustomer.SelectedValue) + ", " & _
                QuotedStr(ddlBuffer.SelectedValue) + "," + QuotedStr(ddlBlock.SelectedValue) + ", " & _
                QuotedStr(ddlSuplier.SelectedValue) + "," + QuotedStr(tbContactPerson.Text) + ", " & _
                QuotedStr(ViewState("UserId").ToString) + ", getDate() " + " , " & _
                QuotedStr(ddlWorkCtr.SelectedValue)

            Else
                SqlString = "Update MsWarehouse set WrhsName= " + QuotedStr(tbName.Text) & _
                            ", WrhsGroup = " + QuotedStr(ddlWrhsGroup.SelectedValue) & _
                            ", WrhsArea = " + QuotedStr(ddlWrhsArea.SelectedValue) & _
                            ", WrhsType = " + QuotedStr(ddlWrhsType.SelectedValue) & _
                            ", WrhsCondition = " + QuotedStr(ddlWrhsClass.SelectedValue) & _
                            ", ContactPerson = " + QuotedStr(tbContactPerson.Text) & _
                            ", FgActive = " + QuotedStr(ddlActive.SelectedValue) & _
                            ", FgCustomer = " + QuotedStr(ddlCustomer.SelectedValue) & _
                            ", FgBuffer = " + QuotedStr(ddlBuffer.SelectedValue) & _
                            ", FgBlock = " + QuotedStr(ddlBlock.SelectedValue) & _
                            ", FgSuplier = " + QuotedStr(ddlSuplier.SelectedValue) & _
                            ", WorkCtr = " + QuotedStr(ddlWorkCtr.SelectedValue) & _
                            " where WrhsCode = " & QuotedStr(tbCode.Text)
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
            tbName.Focus()
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

    Sub filterCondition()
        Dim Filter As String
        Try
            If SQLExecuteScalar("Select HaveSubled From MsWrhsType WHERE WrhsType = " + QuotedStr(ddlWrhsType.SelectedValue), ViewState("DBConnection").ToString) = "Y" Then
                Filter = " WHERE FgSubled <> 'N'"
            Else
                Filter = " WHERE FgSubled = 'N'"
            End If
            'FillCombo(ddlWrhsClass, "SELECT * FROM VMsWrhsCondition " + Filter, True, "WrhsCondition", "WrhsCondition", ViewState("DBConnection"))
        Catch ex As Exception
            Throw New Exception("Filter Condition Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlWrhsType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlWrhsType.SelectedIndexChanged
        Try
            filterCondition()
        Catch ex As Exception
            lstatus.Text = "Wrhs Type Select Index Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub FillTextBox(ByVal WrhsCode As String)
        Dim SqlString As String
        Dim DT As DataTable
        Try
            SqlString = "SELECT * FROM VMsWarehouse"

            DT = BindDataTransaction(SqlString, " Wrhs_Code = " + QuotedStr(WrhsCode), ViewState("DBConnection").ToString)

            BindToText(tbCode, DT.Rows(0)("Wrhs_Code").ToString)
            BindToText(tbName, DT.Rows(0)("Wrhs_Name").ToString)
            BindToDropList(ddlWrhsGroup, DT.Rows(0)("Wrhs_Group").ToString)
            BindToDropList(ddlWrhsArea, DT.Rows(0)("Wrhs_Area").ToString)
            BindToDropList(ddlWrhsType, DT.Rows(0)("Wrhs_Type").ToString)
            BindToDropList(ddlWrhsClass, DT.Rows(0)("Wrhs_Condition").ToString)
            BindToText(tbContactPerson, DT.Rows(0)("ContactPerson").ToString)
            BindToDropList(ddlWorkCtr, DT.Rows(0)("WorkCtr").ToString)
            BindToDropList(ddlActive, DT.Rows(0)("FgActive").ToString)
            'BindToDropList(ddlCustomer, DT.Rows(0)("FgCustomer").ToString)
            ''BindToDropList(ddlBuffer, DT.Rows(0)("FgBuffer").ToString)
            'BindToDropList(ddlBlock, DT.Rows(0)("FgBlock").ToString)
            'BindToDropList(ddlSuplier, DT.Rows(0)("FgSuplier").ToString)
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "FillTextBox error: " & ex.ToString
        End Try
    End Sub

    Private Sub ModifyInput(ByVal State As Boolean)
        tbCode.Enabled = State
        tbName.Enabled = State
        ddlWrhsGroup.Enabled = State
        ddlWrhsArea.Enabled = State
        ddlWrhsType.Enabled = State
        ddlWrhsClass.Enabled = State
        tbContactPerson.Enabled = State
        ddlWorkCtr.Enabled = State
        ddlActive.Enabled = State
        ddlBuffer.Enabled = State
        ddlCustomer.Enabled = State
        ddlBlock.Enabled = State
        ddlBuffer.Enabled = State
        ddlSuplier.Enabled = State
        BtnSave.Visible = State
        btnReset.Visible = State
        btnCancel.Visible = True
    End Sub

End Class
