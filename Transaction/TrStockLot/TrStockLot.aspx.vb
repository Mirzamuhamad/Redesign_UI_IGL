Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data
Imports System.Data.SqlClient

Partial Class Transaction_TrStockLot_TrStockLot
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            InitProperty()
            btnHome.Visible = False
            btnHome2.Visible = False

            ViewState("SortExpression") = Nothing
            'GridDtLine.PageSize = CInt(Session("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            'btnSave.Visible = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            'btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            FillCombo(ddlWarehouse, "EXEC S_GetWarehouse", True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection"))
            'FillCombo(ddlProduct, "SELECT * FROM V_STStockLotReffDt", True, "ProLoc", "ProLoc_Name", ViewState("DBConnection"))
            tbTransType.Attributes.Add("ReadOnly", "True")
            tbTransNo.Attributes.Add("ReadOnly", "True")
            tbReff.Attributes.Add("ReadOnly", "True")
            tbProductName.Attributes.Add("ReadOnly", "True")
            tbQty1.Attributes.Add("ReadOnly", "True")
            tbQty2.Attributes.Add("ReadOnly", "True")
            tbQty3.Attributes.Add("ReadOnly", "True")
            tbGenerateNoEnd.Attributes.Add("ReadOnly", "True")
            tbGenerateQty.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbGeneretDigit.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbGenerateNoStart.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbGenerateNoEnd.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbGenerateQtyPkg.Attributes.Add("OnKeyDown", "return PressNumeric();")

            tbGenerateNoStart.Attributes.Add("OnBlur", "setformatHd('LotNo');")
            tbGenerateQtyPkg.Attributes.Add("OnBlur", "setformatHd('LotNo');")
            ddlProduct.Enabled = False
            If Not Request.QueryString("Code") Is Nothing Then
                FromTransaction()
            End If
            VisibleGrid()
            MultiView1.ActiveViewIndex = 0
            Menu1.Items.Item(0).Selected = True
            If MultiView1.ActiveViewIndex = 0 Then
                VisibleGrid()
                bindDataGrid(tbTransNo.Text, tbTransType.Text, tbProduct.Text, ddlWarehouse.SelectedValue)
            End If
            Session("AdvanceFilter") = ""
        End If

        If Not Session("Result") Is Nothing Then

            If ViewState("Sender") = "btnTrans" Then
                tbTransType.Text = Session("Result")(0).ToString
                tbTransNo.Text = Session("Result")(1).ToString
                BindToDate(tbDate, Session("Result")(2).ToString)
                ddlWarehouse.SelectedValue = Session("Result")(3).ToString
                tbReff.Text = Session("Result")(4).ToString
                tbDoneLot.Text = Session("Result")(5).ToString
                FillCombo(ddlProduct, "SELECT DISTINCT Product, Product_Name FROM V_STStockLotReffDt WHERE Type = " + QuotedStr(tbTransType.Text) + " AND  TransNmbr = " + QuotedStr(tbTransNo.Text), True, "Product", "Product_Name", ViewState("DBConnection"))
                ddlProduct.Enabled = True
                ddlProduct_SelectedIndexChanged(Nothing, Nothing)
            ElseIf ViewState("Sender") = "btnGetFromLot" Then
                Dim drResult, dr As DataRow
                Dim Row As DataRow()
                Dim sqlString As String
                Dim FgValue, FlowType As String
                Dim ExistRow As DataRow()
                'CountLen = ViewState("Dt").Select("(LEN(" + QuotedStr(DrResult("Hasil")) + ") > 20) OR (LEN(" + QuotedStr(DrResult("Hasil")) + ") ")
                For Each drResult In Session("Result").Rows
                    ExistRow = ViewState("Dt").Select("TransNmbr = " + QuotedStr(tbTransNo.Text) + " AND TransType = " + QuotedStr(tbTransType.Text) + " AND Product = " + QuotedStr(tbProduct.Text) + " AND Warehouse = " + QuotedStr(ddlWarehouse.SelectedValue)) ' + " AND LotNo = " + QuotedStr(drResult("LotNo").ToString) + " AND Qty = " + drResult("Qty").ToString)
                    If (ExistRow.Count) = 0 Then
                        If drResult("FgValue") = 1 Then
                            FlowType = "IN"
                        Else
                            FlowType = "OUT"
                        End If
                        If tbFgMove.Text = "OUT" Then
                            FgValue = -1
                        ElseIf tbFgMove.Text = "IN" Then
                            FgValue = 1
                        Else
                            FgValue = 0
                        End If
                        sqlString = "INSERT INTO STCStockLot (TransNmbr, TransType, Product, Warehouse, FlowType, LotNo, Qty, FgValue, QtyPackage, ExpireDate, PalletNo) " + _
                        "SELECT " + QuotedStr(tbTransNo.Text) + ", " + QuotedStr(tbTransType.Text) + ", " + QuotedStr(tbProduct.Text) + ", " + _
                        QuotedStr(ddlWarehouse.SelectedValue) + ", " + QuotedStr(FlowType) + ", '" + drResult("LotNo").ToString + "', " + drResult("Qty").ToString + ", " + CStr(FgValue) + ", " + _
                        drResult("QtyPackage").ToString.Replace(",", "") + ", " + QuotedStr(drResult("ExpireDate")) + ", " + QuotedStr(drResult("PalletNo").ToString)

                        sqlString = ChangeQuoteNull(sqlString)
                        sqlString = sqlString.Replace("'1900-01-01'", "NULL")
                        sqlString = sqlString.Replace("'0001-01-01'", "NULL")
                        sqlString = sqlString.Replace("''", "NULL")

                        SQLExecuteNonQuery(sqlString, ViewState("DBConnection").ToString)
                    End If

                Next
                bindDataGrid(tbTransNo.Text, tbTransType.Text, tbProduct.Text, ddlWarehouse.SelectedValue)
                BindGridDt(ViewState("Dt"), DataGrid)
            End If
            Session("Result") = Nothing
            ViewState("Sender") = Nothing
            Session("filter") = Nothing
            Session("Column") = Nothing
        End If

        lstatus.Text = ""
    End Sub

    Private Sub FromTransaction()
        Dim param() As String
        Try
            param = Request.QueryString("Code").ToString.Split("|")
            tbTransType.Text = param(0)
            tbTransNo.Text = param(1)
            FillTextBoxHd(tbTransNo.Text, tbTransType.Text)
            If Not Session("PrevPageStock") Is Nothing Then
                ViewState("PrevPageStock") = Session("PrevPageStock")
                Session("PrevPageStock") = Nothing
            End If
            btnHome.Visible = True
            btnHome2.Visible = True
        Catch ex As Exception
            Throw New Exception("Load Assigned Code Error : " + ex.ToString)
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
            'If ViewState("FgInsert") = "N" Then
            'lstatus.Text = "<script language='javascript'> {alert('You are not authorized to insert record. Please contact administrator')}</script>"
            'Return False
            'Exit Function
            'End If
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

    Private Sub bindDataGrid(ByVal TransNo As String, ByVal TransType As String, ByVal Product As String, ByVal Warehouse As String)
        Dim SqlString As String
        Dim tempDS As New DataSet()
        Dim DV As DataView
        Try
            SqlString = "SELECT * FROM V_STStockLot WHERE TransNmbr = " + QuotedStr(TransNo) + " AND TransType = " + QuotedStr(TransType) + " AND Product = " + QuotedStr(Product) + " AND Warehouse = " + QuotedStr(Warehouse)


            tempDS = SQLExecuteQuery(SqlString, ViewState("DBConnection"))
            ViewState("Dt") = tempDS.Tables(0)
            DV = tempDS.Tables(0).DefaultView
            If DV.Count = 0 Then
                Dim DT As DataTable = tempDS.Tables(0)
                ShowGridViewIfEmpty(DT, DataGrid)
                ShowGridViewIfEmpty(DT, DataGridView)
            Else
                DataGrid.DataSource = DV
                DataGrid.DataBind()
                DataGridView.DataSource = DV
                DataGridView.DataBind()
            End If
            Dim cbFlowType As DropDownList
            Dim tbQty, tbQtyPkg As TextBox
            Dim tbExpireDate As BasicFrame.WebControls.BasicDatePicker
            cbFlowType = DataGrid.FooterRow.FindControl("FlowTypeAdd")
            tbQty = DataGrid.FooterRow.FindControl("QtyAdd")
            tbQtyPkg = DataGrid.FooterRow.FindControl("QtyPackageAdd")
            tbExpireDate = DataGrid.FooterRow.FindControl("ExpireDateAdd")
            tbQty.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbQtyPkg.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbQtyPkg.Text = "1"
            If tbFgLifeTime.Text = "Y" And tbFgMove.Text = "IN" Then
                tbExpireDate.SelectedDate = tbGenerateExpire.SelectedDate
            Else
                tbExpireDate.Clear()
            End If
            If tbFgMove.Text = "IN" Then
                cbFlowType.SelectedValue = "IN"
                cbFlowType.Enabled = False
            Else
                cbFlowType.SelectedValue = "OUT"
                cbFlowType.Enabled = True
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "bindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Protected Sub DataGrid_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGrid.RowCommand
        Dim SQLString As String
        Dim CbFlowType As DropDownList
        Dim tbLotNo, tbPalletNo, tbQty, tbQtyPkg As TextBox
        Dim tbExpireDate As BasicFrame.WebControls.BasicDatePicker
        Dim FgMove As Integer
        Dim GVR As GridViewRow = Nothing
        Try

            If e.CommandName = "Insert" Then
                CbFlowType = DataGrid.FooterRow.FindControl("FlowTypeAdd")
                tbLotNo = DataGrid.FooterRow.FindControl("LotNoAdd")
                tbPalletNo = DataGrid.FooterRow.FindControl("PalletNoAdd")
                tbQty = DataGrid.FooterRow.FindControl("QtyAdd")
                tbQtyPkg = DataGrid.FooterRow.FindControl("QtyPackageAdd")
                tbExpireDate = DataGrid.FooterRow.FindControl("ExpireDateAdd")


                If tbLotNo.Text.Trim = "" Then
                    lstatus.Text = MessageDlg("Lot No must be filled.")
                    tbLotNo.Focus()
                    Exit Sub
                End If
                If CFloat(tbQty.Text) = 0 Then
                    lstatus.Text = MessageDlg("Qty / Roll must be filled.")
                    tbQty.Focus()
                    Exit Sub
                End If
                If IsNumeric(tbQty.Text.Replace(",", "")) = 0 Then
                    lstatus.Text = MessageDlg("Qty / Roll must be in numeric.")
                    tbQty.Focus()
                    Exit Sub
                End If
                If CFloat(tbQtyPkg.Text) = 0 Then
                    lstatus.Text = MessageDlg("Qty Pkg must be filled.")
                    tbQtyPkg.Focus()
                    Exit Sub
                End If
                If IsNumeric(tbQtyPkg.Text.Replace(",", "")) = 0 Then
                    lstatus.Text = MessageDlg("Qty Pkg must be in numeric.")
                    tbQtyPkg.Focus()
                    Exit Sub
                End If
                If tbFgMove.Text = "IN" Then
                    If tbFgLifeTime.Text = "Y" Then
                        If tbExpireDate.IsNull Then
                            lstatus.Text = MessageDlg("Expire Date must be filled.")
                            tbExpireDate.Focus()
                            Exit Sub
                        End If
                    End If
                    FgMove = 1
                ElseIf tbFgMove.Text = "OUT" Then
                    FgMove = -1
                Else
                    FgMove = 0
                End If

                If SQLExecuteScalar("SELECT LotNo FROM STCStockLot WHERE TransNmbr = " + QuotedStr(tbTransNo.Text) + " AND TransType = " + QuotedStr(tbTransType.Text) + " AND Product = " + QuotedStr(tbProduct.Text) + " AND Warehouse = " + QuotedStr(ddlWarehouse.SelectedValue) + " AND FlowType = " + QuotedStr(CbFlowType.SelectedValue) + " AND LotNo = " + QuotedStr(tbLotNo.Text) + " AND Qty = " + tbQty.Text.Replace(",", ""), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = MessageDlg("LotNo = " + QuotedStr(tbLotNo.Text) + " Qty " + tbQty.Text.Replace(",", "") + " Already Exists")
                    Exit Sub
                End If

                'cek exists data
                'Dim ExistRow As DataRow()
                'ExistRow = ViewState("Dt").Select("TransNmbr = " + QuotedStr(tbTransNo.Text) + " AND TransType = " + QuotedStr(tbTransType.Text) + " AND Product = " + QuotedStr(tbProduct.Text) + " AND Warehouse = " + QuotedStr(ddlWarehouse.SelectedValue) + " AND LotNo = " + QuotedStr(tbLotNo.Text)) + " AND Qty = " + tbQty.Text.Replace(",", "")
                'If ExistRow.Count = 0 Then
                SQLString = "INSERT INTO STCStockLot (TransNmbr, TransType, Product, Warehouse, FlowType, LotNo, PalletNo, Qty, FgValue, QtyPackage, ExpireDate) " + _
                "SELECT " + QuotedStr(tbTransNo.Text) + ", " + QuotedStr(tbTransType.Text) + ", " + QuotedStr(tbProduct.Text) + ", " + _
                QuotedStr(ddlWarehouse.SelectedValue) + ", " + QuotedStr(CbFlowType.SelectedValue) + ", " + QuotedStr(tbLotNo.Text) + ", " + QuotedStr(tbPalletNo.Text) + ", " + tbQty.Text.Replace(",", "") + ", " + FgMove.ToString + ", " + tbQtyPkg.Text.Replace(",", "") + _
                ", " + QuotedStr(Format(tbExpireDate.SelectedValue, "yyyy-MM-dd"))

                SQLString = ChangeQuoteNull(SQLString)
                SQLString = SQLString.Replace("'1900-01-01'", "NULL")
                SQLString = SQLString.Replace("'0001-01-01'", "NULL")
                SQLString = SQLString.Replace("''", "NULL")

                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                bindDataGrid(tbTransNo.Text, tbTransType.Text, tbProduct.Text, ddlWarehouse.SelectedValue)
                'Else
                'lstatus.Text = MessageDlg("Data Already Exists")
                'End If

            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGrid.RowDeleting
        Dim txtFlowType, txtID, txtQty As Label
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            txtFlowType = DataGrid.Rows(e.RowIndex).FindControl("FlowType")
            txtID = DataGrid.Rows(e.RowIndex).FindControl("LotNo")
            txtQty = DataGrid.Rows(e.RowIndex).FindControl("Qty")

            Dim dr() As DataRow
            dr = ViewState("Dt").Select("LotNo = " + QuotedStr(txtID.Text) + " AND Qty = " + txtQty.Text.Replace(",", "") + " AND FlowType = " + QuotedStr(txtFlowType.Text.Trim) + " AND TransNmbr = " + QuotedStr(tbTransNo.Text) + " AND TransType = " + QuotedStr(tbTransType.Text) + " AND Product = " + QuotedStr(tbProduct.Text) + " AND Warehouse = " + QuotedStr(ddlWarehouse.SelectedValue))
            dr(0).Delete()
            SQLExecuteNonQuery("DELETE FROM STCStockLot WHERE LotNo = " + QuotedStr(txtID.Text) + " AND Qty = " + txtQty.Text.Replace(",", "") + " AND FlowType = " + QuotedStr(txtFlowType.Text.Trim) + " AND TransNmbr = " + QuotedStr(tbTransNo.Text) + " AND TransType = " + QuotedStr(tbTransType.Text) + " AND Product = " + QuotedStr(tbProduct.Text) + " AND Warehouse = " + QuotedStr(ddlWarehouse.SelectedValue), ViewState("DBConnection").ToString)

            bindDataGrid(tbTransNo.Text, tbTransType.Text, tbProduct.Text, ddlWarehouse.SelectedValue)
        Catch ex As Exception
            lstatus.Text = "DataGrid_RowDeleting Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        Dim txtQtyPack, txtlotNo, txtQty, txtPallet As TextBox
        Dim txtExpireDate As BasicFrame.WebControls.BasicDatePicker
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid(tbTransNo.Text, tbTransType.Text, tbProduct.Text, ddlWarehouse.SelectedValue)
            obj = DataGrid.Rows(e.NewEditIndex)
            txtQty = obj.FindControl("QtyEdit")
            txtQtyPack = obj.FindControl("QtyPackageEdit")
            txtlotNo = obj.FindControl("LotNoEdit")
            txtPallet = obj.FindControl("PalletNoEdit")
            txtExpireDate = obj.FindControl("ExpireDateEdit")

            txtQty.Attributes.Add("OnKeyDown", "return PressNumeric();")
            txtQtyPack.Attributes.Add("OnKeyDown", "return PressNumeric();")
            
            If tbFgMove.Text = "IN" Then
                txtPallet.Enabled = True
            ElseIf tbFgMove.Text = "OUT" Then
                txtPallet.Enabled = False
            Else
                txtPallet.Enabled = True
            End If
            txtExpireDate.Enabled = False
            txtQty.Enabled = False
            txtlotNo.Enabled = False

            txtQtyPack.Focus()
            ViewState("Qty") = txtQty.Text
            ViewState("LotNo") = txtlotNo.Text
        Catch ex As Exception
            lstatus.Text = "DataGrid_RowEditing exception : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim tbFlowType, tbLotNo, tbQty, tbPalletNo, tbQtyPkg As TextBox
        Dim FgMove As Integer
        Dim tbExpireDate As BasicFrame.WebControls.BasicDatePicker
        Try
            tbFlowType = DataGrid.Rows(e.RowIndex).FindControl("FlowTypeEdit")
            tbQty = DataGrid.Rows(e.RowIndex).FindControl("QtyEdit")
            tbLotNo = DataGrid.Rows(e.RowIndex).FindControl("LotNoEdit")
            tbPalletNo = DataGrid.Rows(e.RowIndex).FindControl("PalletNoEdit")
            tbQtyPkg = DataGrid.Rows(e.RowIndex).FindControl("QtyPackageEdit")
            tbExpireDate = DataGrid.Rows(e.RowIndex).FindControl("ExpireDateEdit")

            If CFloat(tbQty.Text) = 0 Then
                lstatus.Text = MessageDlg("Qty must be filled.")
                tbQty.Focus()
                Exit Sub
            End If
            If IsNumeric(tbQty.Text.Replace(",", "")) = 0 Then
                lstatus.Text = MessageDlg("Qty must be in numeric.")
                tbQty.Focus()
                Exit Sub
            End If
            If CFloat(tbQtyPkg.Text) = 0 Then
                lstatus.Text = MessageDlg("Qty Pkg must be filled.")
                tbQtyPkg.Focus()
                Exit Sub
            End If
            If IsNumeric(tbQtyPkg.Text.Replace(",", "")) = 0 Then
                lstatus.Text = MessageDlg("Qty Pkg must be in numeric.")
                tbQtyPkg.Focus()
                Exit Sub
            End If
            If tbFgMove.Text = "IN" Then
                If tbFgLifeTime.Text = "Y" Then
                    If tbExpireDate.IsNull Then
                        lstatus.Text = MessageDlg("Expire Date must be filled.")
                        tbExpireDate.Focus()
                        Exit Sub
                    End If
                End If
                FgMove = 1
            ElseIf tbFgMove.Text = "OUT" Then
                FgMove = -1
            Else
                FgMove = 0
            End If

            'If ViewState("Qty") <> tbQty.Text Then
            '    If SQLExecuteScalar("SELECT LotNo FROM STCStockLot WHERE TransNmbr = " + QuotedStr(tbTransNo.Text) + " AND TransType = " + QuotedStr(tbTransType.Text) + " AND Product = " + QuotedStr(tbProduct.Text) + " AND Warehouse = " + QuotedStr(ddlWarehouse.SelectedValue) + " AND LotNo = " + QuotedStr(tbLotNo.Text) + " AND Qty = " + tbQty.Text.Replace(",", ""), ViewState("DBConnection").ToString).Length > 0 Then
            '        lstatus.Text = "Lot No " + QuotedStr(tbLotNo.Text) + " Qty " + tbQty.Text.Replace(",", "") + " has already been exist"
            '        Exit Sub
            '    End If
            'End If

            SQLString = "UPDATE STCStockLot SET FgValue = " + FgMove.ToString + ", PalletNo = " + QuotedStr(tbPalletNo.Text) + _
            ", QtyPackage = " + tbQtyPkg.Text.Replace(",", "") + ", ExpireDate = " + QuotedStr(Format(tbExpireDate.SelectedValue, "yyyy-MM-dd")) + _
            " WHERE TransNmbr = " + QuotedStr(tbTransNo.Text) + " AND TransType =  " + QuotedStr(tbTransType.Text) + " AND Product = " + QuotedStr(tbProduct.Text) + " AND Warehouse = " + QuotedStr(ddlWarehouse.SelectedValue) + " AND FlowType = " + QuotedStr(tbFlowType.Text) + " AND LotNo = " + QuotedStr(tbLotNo.Text) + " AND Qty = " + tbQty.Text.Replace(",", "")


            SQLString = ChangeQuoteNull(SQLString)
            SQLString = SQLString.Replace("'1900-01-01'", "NULL")
            SQLString = SQLString.Replace("'0001-01-01'", "NULL")
            SQLString = SQLString.Replace("''", "NULL")

            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid(tbTransNo.Text, tbTransType.Text, tbProduct.Text, ddlWarehouse.SelectedValue)
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "DataGrid_RowUpdating Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles DataGrid.RowCancelingEdit
        Try
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid(tbTransNo.Text, tbTransType.Text, tbProduct.Text, ddlWarehouse.SelectedValue)
        Catch ex As Exception
            lstatus.Text = "DataGrid_RowCancelingEdit Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        Dim index As Integer
        Try
            index = Int32.Parse(e.Item.Value)
            MultiView1.ActiveViewIndex = index
            If MultiView1.ActiveViewIndex = 0 Then
                VisibleGrid()
                bindDataGrid(tbTransNo.Text, tbTransType.Text, tbProduct.Text, ddlWarehouse.SelectedValue)
            End If
        Catch ex As Exception
            lstatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles DataGrid.Sorting, DataGridView.Sorting
        Try
            If ViewState("SortOrder") = Nothing Or ViewState("SortOrder") = "DESC" Then
                ViewState("SortOrder") = "ASC"
            Else
                ViewState("SortOrder") = "DESC"
            End If
            ViewState("SortExpression") = e.SortExpression + " " + ViewState("SortOrder")
            bindDataGrid(tbTransNo.Text, tbTransType.Text, tbProduct.Text, ddlWarehouse.SelectedValue)
        Catch ex As Exception
            lstatus.Text = "DataGrid_Sorting =" + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGrid.PageIndexChanging, DataGridView.PageIndexChanging
        DataGrid.PageIndex = e.NewPageIndex

        If DataGrid.EditIndex <> -1 Then
            DataGrid_RowCancelingEdit(Nothing, Nothing)
        End If
        bindDataGrid(tbTransNo.Text, tbTransType.Text, tbProduct.Text, ddlWarehouse.SelectedValue)
    End Sub

    Protected Sub btnTrans_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTrans.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT * FROM V_STStockLotReffHd"
            ResultField = "Type, Transnmbr, TransDate, Warehouse, Reference, DoneLot"
            ViewState("Sender") = "btnTrans"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lstatus.Text = "btnTrans Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlProduct_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlProduct.SelectedIndexChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Dim SQLString As String
        Try
            SQLString = "SELECT * FROM V_STStockLotReffGetDt WHERE Product = " + QuotedStr(ddlProduct.SelectedValue) + " AND TransNmbr = " + QuotedStr(tbTransNo.Text)
            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbProduct.Text = Dr("Product")
                tbProductName.Text = Dr("Product_Name")
                tbQty1.Text = "1"
                tbQty2.Text = "0"
                tbQty3.Text = FormatNumber(Dr("Qty"), ViewState("DigitCurr"))
                lblUnit1.Text = "Roll"
                lblUnit2.Text = "M2"
                lblUnit3.Text = Dr("Unit")
                tbFgMove.Text = Dr("FgMove")
                tbFgLifeTime.Text = Dr("FgLifeTime")
                tbLifeTime.Text = Dr("LifeTime")
            Else
                tbProduct.Text = ""
                tbProductName.Text = ""
                tbQty1.Text = "0"
                tbQty2.Text = "0"
                tbQty3.Text = "0"
                lblUnit1.Text = ""
                lblUnit2.Text = ""
                lblUnit3.Text = ""
                tbFgMove.Text = ""
                tbFgLifeTime.Text = "N"
                tbLifeTime.Text = "0"
            End If
            tbGenerateNoEnd.Text = "1"
            tbGenerateNoStart.Text = "1"
            tbGenerateQty.Text = "0" 'CFloat(tbQty3.Text) / CFloat(tbQty1.Text)
            tbGeneratePerfix.Text = ""
            tbGeneretDigit.Text = "0"
            tbGenerateQtyPkg.Text = tbQty1.Text
            tbGenerateQtyLot.Text = "1"

            VisibleGrid()

            lblUnit4.Text = lblUnit3.Text
            lblUnit5.Text = lblUnit1.Text
            lblUnit6.Text = lblUnit1.Text '+ "/Lot"

            DataGrid.Columns(3).HeaderText = "Qty Wrhs" '+ lblUnit4.Text + "/" + lblUnit5.Text
            DataGrid.Columns(4).HeaderText = "Pallet No"
            DataGrid.Columns(5).HeaderText = "Qty Packing" ' + lblUnit1.Text

            lblUnit9.Text = lblUnit1.Text
            lblUnit10.Text = lblUnit4.Text
            lblUnit8.Visible = True
            tbGenerateExpire.Visible = True
            lblUnit7.Text = "Expire Date"
            If tbFgMove.Text = "IN" Then
                tbGenerateExpire.Visible = True
                If tbFgLifeTime.Text = "Y" Then
                    tbGenerateExpire.SelectedDate = tbDate.SelectedDate.AddDays(CInt(tbLifeTime.Text))
                Else
                    tbGenerateExpire.SelectedDate = Nothing
                End If
                pnlOut.Visible = True
                'DataGrid.Columns(5).Visible = True
                'DataGrid.Columns(6).Visible = True
                btnOut.Visible = False
            ElseIf tbFgMove.Text = "OUT" Then
                tbGenerateExpire.Visible = False
                pnlOut.Visible = False
                'DataGrid.Columns(6).Visible = True
                'DataGrid.Columns(7).Visible = True
                tbGenerateExpire.Clear()
                btnOut.Visible = True
            ElseIf tbFgMove.Text = "TT" Then
                tbGenerateExpire.Visible = True
                pnlOut.Visible = True
                'DataGrid.Columns(5).Visible = True
                'DataGrid.Columns(6).Visible = True

                btnOut.Visible = True
                tbGenerateExpire.Clear()
            Else
                lblUnit7.Text = ""
                lblUnit8.Visible = False
                tbGenerateExpire.Visible = False
                pnlOut.Visible = False
                'DataGrid.Columns(5).Visible = False
                'DataGrid.Columns(6).Visible = False
                btnOut.Visible = False
                tbGenerateExpire.Clear()
            End If
            bindDataGrid(tbTransNo.Text, tbTransType.Text, tbProduct.Text, ddlWarehouse.SelectedValue)
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "ddlProduct_SelectedIndexChanged Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub VisibleGrid()
        Try
            If (ddlProduct.SelectedIndex <> 0) And (tbDoneLot.Text = "N") Then
                Panel2.Visible = True
                Panel3.Visible = False
            ElseIf (ddlProduct.SelectedIndex <> 0) And (tbDoneLot.Text = "Y") Then
                Panel2.Visible = False
                Panel3.Visible = True
            Else
                Panel2.Visible = False
                Panel3.Visible = False
            End If
        Catch ex As Exception
            Throw New Exception("VisibleGrid Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
        Try
            If (CInt(tbGenerateNoStart.Text) = 0) Or (CInt(tbGenerateNoEnd.Text) = 0) Or (CFloat(tbGenerateQty.Text) = 0) Or (CInt(tbGeneretDigit.Text) = 0) Or (CInt(tbGenerateQtyLot.Text) = 0) Or (CInt(tbGenerateQtyPkg.Text) = 0) Then
                lstatus.Text = "Set Lot No must be complete"
                tbGenerateNoStart.Focus()
                Exit Sub
            End If
            If CInt(tbGeneratePerfix.Text.Length) >= CInt(tbGeneretDigit.Text) Then
                lstatus.Text = "Prefix can not greater than Digit"
                tbGeneratePerfix.Focus()
                Exit Sub
            End If
            If tbFgMove.Text = "IN" Then
                If tbFgLifeTime.Text = "Y" Then
                    If tbGenerateExpire.IsNull Then
                        lstatus.Text = "Expire Date must have value"
                        tbGenerateExpire.Focus()
                        Exit Sub
                    End If
                End If
            ElseIf tbFgMove.Text = "OUT" Then

            End If
            bindDataSetNoLot()
        Catch ex As Exception
            Throw New Exception("btnGenerate_Click Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub bindDataSetNoLot()
        Dim FlowType, SQLString As String
        Dim Dt As DataTable
        Dim DrResult As DataRow
        Try
            SQLString = "EXEC S_STCGetNoLot " + QuotedStr(tbGeneratePerfix.Text) + ", " + tbGenerateQty.Text + ", " + tbGenerateNoStart.Text + ", " + tbGenerateNoEnd.Text + ", " + tbGeneretDigit.Text + ", " + QuotedStr(tbGenerateSufix.Text)
            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)

            If Dt.Rows.Count > 0 Then
                For Each DrResult In Dt.Rows
                    'insert
                    Dim FgMove As Integer

                    If tbFgMove.Text = "IN" Then
                        FgMove = 1
                        FlowType = "IN"
                    ElseIf tbFgMove.Text = "OUT" Then
                        FgMove = -1
                        FlowType = "OUT"
                    Else
                        FgMove = 0
                        FlowType = "OUT"
                    End If

                    Dim ExistRow As DataRow()
                    'CountLen = ViewState("Dt").Select("(LEN(" + QuotedStr(DrResult("Hasil")) + ") > 20) OR (LEN(" + QuotedStr(DrResult("Hasil")) + ") ")
                    ExistRow = ViewState("Dt").Select("TransNmbr = " + QuotedStr(tbTransNo.Text) + " AND TransType = " + QuotedStr(tbTransType.Text) + " AND Product = " + QuotedStr(tbProduct.Text) + " AND Warehouse = " + QuotedStr(ddlWarehouse.SelectedValue) + " AND LotNo = " + QuotedStr(DrResult("Hasil")) + " AND Qty = " + DrResult("Qty").ToString)
                    If (ExistRow.Count) = 0 Then
                        SQLString = "INSERT INTO STCStockLot (TransNmbr, TransType, Product, Warehouse, FlowType, LotNo, Qty, FgValue, QtyPackage, ExpireDate, PalletNo) " + _
                        "SELECT " + QuotedStr(tbTransNo.Text) + ", " + QuotedStr(tbTransType.Text) + ", " + QuotedStr(tbProduct.Text) + ", " + _
                        QuotedStr(ddlWarehouse.SelectedValue) + ", " + QuotedStr(FlowType) + ", '" + DrResult("Hasil") + "', " + DrResult("Qty").ToString + ", " + FgMove.ToString + ", " + _
                        tbGenerateQtyLot.Text.Replace(",", "") + ", " + QuotedStr(Format(tbGenerateExpire.SelectedValue, "yyy-MM-dd")) + ", " + QuotedStr(tbPalletNo.Text)

                        SQLString = ChangeQuoteNull(SQLString)
                        SQLString = SQLString.Replace("'1900-01-01'", "NULL")
                        SQLString = SQLString.Replace("'0001-01-01'", "NULL")
                        SQLString = SQLString.Replace("''", "NULL")

                        SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                    End If
                Next
            End If
            lstatus.Text = "Set Success for No Lot"
            tbGenerateNoEnd.Text = "1"
            tbGenerateNoStart.Text = "1"
            tbGenerateQty.Text = "0"
            'tbGeneratePerfix.Text = ""
            'tbGeneretDigit.Text = "0"
            tbGenerateQtyLot.Text = "1"
            tbGenerateQtyPkg.Text = "1"
            'tbGenerateExpire.Clear()
            'tbGenerateSufix.Text = ""
            tbPalletNo.Text = ""
            bindDataGrid(tbTransNo.Text, tbTransType.Text, tbProduct.Text, ddlWarehouse.SelectedValue)
        Catch ex As Exception
            Throw New Exception("bindDataSetNoLot Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxHd(ByVal Nmbr As String, ByVal Type As String)
        Dim Dt As DataTable
        Dim Dr As DataRow
        Dim SQLString As String
        Try
            SQLString = "SELECT * FROM V_STStockLotReffHd WHERE TransNmbr = " + QuotedStr(Nmbr) + " AND Type = " + QuotedStr(Type)
            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbDate.SelectedDate = Dr("TransDate")
                tbDoneLot.Text = Dr("DoneLot")
                ddlWarehouse.SelectedValue = Dr("Warehouse")
                tbReff.Text = Dr("Reference")
            Else
                tbDate.SelectedDate = ViewState("ServerDate")
                tbDoneLot.Text = ""
                ddlWarehouse.SelectedIndex = 0
                tbReff.Text = ""
            End If

            btnTrans.Visible = False
            FillCombo(ddlProduct, "SELECT DISTINCT Product, Product_Name FROM V_STStockLotReffDt WHERE TransNmbr = " + QuotedStr(Nmbr) + " AND Type = " + QuotedStr(tbTransType.Text), True, "Product", "Product_Name", ViewState("DBConnection"))
            ddlProduct.Enabled = True
            ddlProduct_SelectedIndexChanged(Nothing, Nothing)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnHome_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHome.Click, btnHome2.Click
        Try
            'lstatus.Text = ViewState("PrevPageStock")
            'Exit Sub
            If Not ViewState("PrevPageStock") Is Nothing Then
                Response.Redirect(ViewState("PrevPageStock") + "&transid=" + tbTransNo.Text)
                ViewState("PrevPageStock") = Nothing
            End If
        Catch ex As Exception
            Throw New Exception("btnHome_Click error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnOut_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOut.Click
        Dim ResultField, CriteriaField As String 'ResultSame 
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT LotNo, Qty, PalletNo, QtyPackage, ExpireDate, FgValue FROM V_STStockLotNoForIssue WHERE Product = " + QuotedStr(ddlProduct.SelectedValue)
            ResultField = "LotNo, PalletNo, QtyPackage, Qty, ExpireDate, FgValue"
            CriteriaField = "LotNo, PalletNo, QtyPackage, Qty, ExpireDate, FgValue"
            ViewState("CriteriaField") = CriteriaField.Split(",")
            Session("Column") = ResultField.Split(",")
            ViewState("Sender") = "btnGetFromLot"
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lstatus.Text = "btnOut_Click Error : " + ex.ToString
        End Try
    End Sub
    Dim QtyPack As Decimal = 0
    Dim QtyRoll As Decimal = 0
    Protected Sub DataGrid_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles DataGrid.RowDataBound
        Try
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "Product")) Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                    '' add the UnitPrice and QuantityTotal to the running total variables
                    'CrHome += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CreditHome"))
                    ''CrForex += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CreditForex"))
                    If tbFgMove.Text = "IN" Then
                        QtyPack += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "QtyPackage"))
                        QtyRoll += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Qty") * DataBinder.Eval(e.Row.DataItem, "QtyPackage"))
                    Else
                        If DataBinder.Eval(e.Row.DataItem, "FlowType") = "OUT" Then
                            QtyPack += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "QtyPackage"))
                            QtyRoll += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Qty") * DataBinder.Eval(e.Row.DataItem, "QtyPackage"))
                        Else
                            QtyRoll -= Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Qty") * DataBinder.Eval(e.Row.DataItem, "QtyPackage"))
                        End If
                    End If
                ElseIf e.Row.RowType = DataControlRowType.Footer Then
                                    End If
                tbTQtyLot.Text = FormatNumber(QtyPack, ViewState("DigitQty"))
                tbTQtyLot2.Text = FormatNumber(QtyRoll, ViewState("DigitQty"))
            End If
        Catch ex As Exception
            lstatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub FlowType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        
    End Sub

    Protected Sub LotNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        
    End Sub

    Protected Sub Qty_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'Dim dr As DataRow
        'Dim ds As DataSet
        'Dim tbCode, tb As TextBox
        'Dim tbPallet, tbQty, tbQtyPkg As TextBox
        'Dim tbExpireDate As BasicFrame.WebControls.BasicDatePicker
        'Dim Count As Integer
        'Dim dgi As GridViewRow
        'Dim ddlCurr As DropDownList = New DropDownList
        'Try
        '    tb = sender

        '    If tb.ID = "QtyAdd" Then
        '        Count = DataGrid.Controls(0).Controls.Count
        '        dgi = DataGrid.Controls(0).Controls(Count - 1) '-1 for allowpaging = False   - 2 allowpaging = True
        '        tbCode = dgi.FindControl("LotNoAdd")
        '        tbPallet = dgi.FindControl("PalletNoAdd")
        '        tbQty = dgi.FindControl("QtyAdd")
        '        tbQtyPkg = dgi.FindControl("QtyPackageAdd")
        '        tbExpireDate = dgi.FindControl("ExpireDateAdd")
        '    Else
        '        Count = DataGrid.EditIndex
        '        dgi = DataGrid.Rows(Count)
        '        tbCode = dgi.FindControl("LotNoEdit")
        '        tbPallet = dgi.FindControl("PalletNoEdit")
        '        tbQty = dgi.FindControl("QtyEdit")
        '        tbQtyPkg = dgi.FindControl("QtyPackageEdit")
        '        tbExpireDate = dgi.FindControl("ExpireDateEdit")
        '    End If
        '    If tbFgMove.Text = "OUT" Then
        '        'ds = SQLExecuteQuery("Select * From V_STStockLotNoForIssue WHERE Product = " + QuotedStr(ddlProduct.SelectedValue) + " AND LotNo = " + QuotedStr(tbCode.Text) + " And Qty = " + tbQty.Text.Replace(",", ""), ViewState("DBConnection").ToString)
        '        ds = SQLExecuteQuery("Select * From V_STStockLotNoForIssue WHERE Product = " + QuotedStr(ddlProduct.SelectedValue) + " AND LotNo = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
        '        If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
        '            tbPallet.Enabled = True
        '        Else
        '            dr = ds.Tables(0).Rows(0)
        '            tbCode.Text = dr("LotNo").ToString
        '            tbPallet.Text = dr("PalletNo").ToString
        '            tbQty.Text = dr("Qty").ToString
        '            tbQtyPkg.Text = dr("QtyPackage").ToString
        '            tbExpireDate.SelectedDate = dr("ExpireDate").ToString
        '            tbPallet.Enabled = False
        '        End If
        '        tbExpireDate.Enabled = tbPallet.Enabled
        '    ElseIf tbFgMove.Text = "IN" Then
        '        tbPallet.Enabled = True
        '        tbExpireDate.Enabled = True
        '    Else
        '        tbPallet.Enabled = True
        '        tbExpireDate.Enabled = True
        '    End If
        'Catch ex As Exception
        '    lstatus.Text = "Qty_TextChanged Error : " + ex.ToString
        'End Try
    End Sub
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
                    btnProcessDel.Visible = True
                End If
            Next
        Catch ex As Exception
            Throw New Exception("Check All Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnProcessDel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcessDel.Click
        Try
            Dim GVR As GridViewRow
            Dim CB As CheckBox

            Dim Row As DataRow
            Dim LotNo As String
            Dim lb, lbQty As Label
            For Each GVR In DataGrid.Rows
                CB = GVR.FindControl("cbSelect")
                lb = GVR.FindControl("LotNo")
                lbQty = GVR.FindControl("Qty")
                If CB.Checked Then
                    LotNo = lb.Text
                    Row = ViewState("Dt").Select("LotNo = " + QuotedStr(LotNo))(0)
                    Row.Delete()
                    SQLExecuteNonQuery("DELETE FROM STCStockLot WHERE LotNo = " + QuotedStr(lb.Text) + " AND Qty = " + lbQty.Text.Replace(",", "") + " AND TransNmbr = " + QuotedStr(tbTransNo.Text) + " AND TransType = " + QuotedStr(tbTransType.Text) + " AND Product = " + QuotedStr(tbProduct.Text) + " AND Warehouse = " + QuotedStr(ddlWarehouse.SelectedValue), ViewState("DBConnection").ToString)
                End If
            Next
            lstatus.Text = MessageDlg("Delete Selected Data Success")
            'bindDataDeleteAll()
            bindDataGrid(tbTransNo.Text, tbTransType.Text, tbProduct.Text, ddlWarehouse.SelectedValue)
        Catch ex As Exception
            Throw New Exception("btnProcessDel_Click Error : " + ex.ToString)
        End Try
    End Sub
End Class
