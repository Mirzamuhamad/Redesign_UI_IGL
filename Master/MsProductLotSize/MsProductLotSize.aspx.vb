Imports System.Data

Partial Class Master_MsProductLotSize_MsProductLotSize
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            InitProperty()
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            Dim dt As DataTable
            dt = SQLExecuteQuery("Select RptTitle, MenuName, COALESCE(MenuParam,'') AS MenuParam, COALESCE(FastRpt,'') AS FastRpt, COALESCE(ReportType,'') AS ReportType, COALESCE(ReportType2,'') AS ReportType2, COALESCE(FgPrintValue,'N') AS FgPrintValue, COALESCE(FgForceNewPage,'N') AS FgForceNewPage from MsMenu WHERE MenuId = " + QuotedStr(Request.QueryString("ContainerId").ToString), ViewState("DBConnection").ToString).Tables(0)
            lblTitle.Text = dt.Rows(0)("MenuName").ToString

        End If
        dsShift.ConnectionString = ViewState("DBConnection")

        If Not Session("Result") Is Nothing Then
            If ViewState("Sender") = "btnProduct" Then
                tbProductCode.Text = Session("Result")(0).ToString
                BindToText(tbProductName, Session("Result")(1).ToString)
                bindDataGrid()
            End If
            If ViewState("Sender") = "btnSearchFrom" Then
                tbFromCode.Text = Session("Result")(0).ToString
                BindToText(tbFromName, Session("Result")(1).ToString)
            End If
            If ViewState("Sender") = "btnSearchTo" Then
                tbToCode.Text = Session("Result")(0).ToString
                BindToText(tbToName, Session("Result")(1).ToString)
            End If
            Session("Result") = Nothing
            ViewState("Sender") = Nothing
            Session("filter") = Nothing
            Session("Column") = Nothing
        End If
        lstatus.Text = ""
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
        Dim GVR As GridViewRow
        Dim LotQty As TextBox

        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            SqlString = "SELECT ProductCode, Day, Shift, LotQty, UserId, UserDate from MsProductLotSize where ProductCode = " + QuotedStr(tbProductCode.Text) + StrFilter
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "ProductCode ASC"
            End If
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection"))

            GVR = DataGrid.FooterRow
            LotQty = GVR.FindControl("LotQtyAdd")
            LotQty.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Public Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        Dim txt, LotQty As TextBox
        Dim ddlDay, ddlShift As Label

        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()
            obj = DataGrid.Rows(e.NewEditIndex)
            ddlDay = obj.FindControl("DayEdit")
            ddlShift = obj.FindControl("ShiftEdit")
            txt = obj.FindControl("LotQtyEdit")

            ddlDay.Focus()

            LotQty = DataGrid.Rows(e.NewEditIndex).FindControl("LotQtyEdit")
            LotQty.Attributes.Add("OnKeyDown", "return PressNumeric();")
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

    Protected Sub DataGrid_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGrid.RowCommand
        Dim SQLString As String
        Dim dbLotQty As TextBox
        Dim ddlDay, ddlShift As DropDownList
        Dim GVR As GridViewRow
        Try
            If e.CommandName = "Insert" Then
                GVR = DataGrid.FooterRow
                ddlDay = GVR.FindControl("DayAdd")
                ddlShift = GVR.FindControl("ShiftAdd")
                dbLotQty = GVR.FindControl("LotQtyAdd")

                If tbProductName.Text.Trim.Length = 0 Then
                    lstatus.Text = "<script language='javascript'>alert('Product must be filled');</script>"
                    tbProductCode.Focus()
                    Exit Sub
                End If
                If ddlShift.Text.Trim.Length = 0 Then
                    lstatus.Text = "<script language='javascript'>alert('Shift must be filled');</script>"
                    ddlShift.Focus()
                    Exit Sub
                End If
                If CFloat(dbLotQty.Text.Trim) <= 0 Then
                    lstatus.Text = "<script language='javascript'>alert('Lot Qty must be filled');</script>"
                    dbLotQty.Focus()
                    Exit Sub
                End If

                If SQLExecuteScalar("SELECT ProductCode From VMsProductLotSize WHERE ProductCode = " + QuotedStr(tbProductCode.Text) + " AND Day = " + QuotedStr(ddlDay.SelectedValue) + " AND Shift = " + QuotedStr(ddlShift.SelectedValue), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Product " + QuotedStr(tbProductCode.Text) + " AND Day " + QuotedStr(ddlDay.SelectedValue) + " AND Shift " + QuotedStr(ddlShift.SelectedValue) + " has already been exist"
                    Exit Sub
                End If

                'insert the new entry
                SQLString = "Insert into MsProductLotSize (ProductCode, Day, Shift, LotQty, UserID, UserDate) " + _
                "SELECT " + QuotedStr(tbProductCode.Text) + "," + QuotedStr(ddlDay.SelectedValue) + ", " + QuotedStr(ddlShift.SelectedValue) + "," + dbLotQty.Text.ToString + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", GetDate()"
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                bindDataGrid()
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim tbLotQty As TextBox
        Dim ddlDay, ddlShift As Label
        Dim GVR As GridViewRow
        Try
            GVR = DataGrid.Rows(e.RowIndex)
            ddlDay = GVR.FindControl("DayEdit")
            ddlShift = GVR.FindControl("ShiftEdit")
            tbLotQty = GVR.FindControl("LotQtyEdit")

            If CFloat(tbLotQty.Text.Trim) <= 0 Then
                lstatus.Text = "<script language='javascript'>alert('Lot Qty must be filled.');</script>"
                tbLotQty.Focus()
                Exit Sub
            End If

            SQLString = "Update MsProductLotSize set LotQty = " + tbLotQty.Text + _
            "  where ProductCode = " + QuotedStr(tbProductCode.Text) + " AND Day = " + ddlDay.Text + " AND Shift = " + QuotedStr(ddlShift.Text)

            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = lstatus.Text + "DataGrid_Update Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGrid.RowDeleting
        Dim ddlDay, ddlShift As Label
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            ddlDay = DataGrid.Rows(e.RowIndex).FindControl("Day")
            ddlShift = DataGrid.Rows(e.RowIndex).FindControl("Shift")

            SQLExecuteNonQuery("Delete from MsProductLotSize where ProductCode = " + QuotedStr(tbProductCode.Text) + " AND Day = " + ddlDay.Text + " AND Shift = " + QuotedStr(ddlShift.Text), ViewState("DBConnection").ToString)
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
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

    Protected Sub ProductCategoryEdit_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dgi As GridViewRow
        Dim ddlProductCategory, ddlFgStock As DropDownList
        Try
            dgi = DataGrid.Rows(DataGrid.EditIndex)
            ddlProductCategory = dgi.FindControl("ProductCategoryEdit")
            ddlFgStock = dgi.FindControl("FgStockEdit")

            If ddlProductCategory.SelectedValue = "Service" Or ddlProductCategory.SelectedValue = "Fixed Asset" Then
                ddlFgStock.SelectedIndex = 1
                ddlFgStock.Enabled = False
            Else
                ddlFgStock.SelectedIndex = 0
                ddlFgStock.Enabled = True
            End If

        Catch ex As Exception
            lstatus.Text = "Product Category Edit Index Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ProductCategoryAdd_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dgi As GridViewRow
        Dim ddlProductCategory, ddlFgStock As DropDownList
        Try
            dgi = DataGrid.FooterRow
            ddlProductCategory = dgi.FindControl("ProductCategoryAdd")
            ddlFgStock = dgi.FindControl("FgStockAdd")

            If ddlProductCategory.SelectedValue = "Service" Or ddlProductCategory.SelectedValue = "Fixed Asset" Then
                ddlFgStock.SelectedIndex = 1
                ddlFgStock.Enabled = False
            Else
                ddlFgStock.SelectedIndex = 0
                ddlFgStock.Enabled = True
            End If

        Catch ex As Exception
            lstatus.Text = "Product Category Add Index Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("DBConnection") = ViewState("DBConnection")
            Session("PrintType") = "Print"
            Session("SelectCommand") = "EXEC S_FormPrintMaster5 'VMsProductLotSize','ProductCode','ProductName', 'DayName', 'ShiftName', 'LotQty'," + QuotedStr(lblTitle.Text) + ",'Product Code','Product Name','Day','Shift', 'Lot Qty'," + QuotedStr(StrFilter) + "," + QuotedStr(ViewState("UserId"))
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster4.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            lstatus.Text = "Btn Print Error : " + ex.ToString
        End Try

    End Sub


    Protected Sub btnProduct_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProduct.Click
        Try
            Dim ResultField As String
            Session("filter") = "Select Product_Code, Product_Name From VMsProduct"
            Session("DBConnection") = ViewState("DBConnection")
            ResultField = "Product_Code, Product_Name"
            ViewState("Sender") = "btnProduct"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType)
            DataGrid.PageIndex = 0
            DataGrid.EditIndex = -1
            bindDataGrid()
        Catch ex As Exception
            lstatus.Text = "Btn Product Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub tbProductCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbProductCode.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Product", tbProductCode.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbProductCode.Text = Dr("Product_Code")
                tbProductName.Text = Dr("Product_Name")
                tbProductCode.Focus()
            Else
                tbProductCode.Text = ""
                tbProductName.Text = ""
                tbProductCode.Focus()
            End If
            bindDataGrid()
        Catch ex As Exception
            Throw New Exception("tb Product change Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnCopyTo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCopyTo.Click
        Try
            pnlHd.Visible = False
            pnlCopy.Visible = True
            btnCopyTo.Enabled = False
            'ddlGroupByCopy.SelectedValue = ddlGroupBy.SelectedValue
            If tbProductCode.Text.Trim <> "" Then
                tbFromCode.Text = tbProductCode.Text
                tbFromName.Text = tbProductName.Text
            Else
                tbFromCode.Text = ""
                tbFromName.Text = ""
            End If
            tbToCode.Text = ""
            tbToName.Text = ""
        Catch ex As Exception
            Throw New Exception("btnCopyTo_Click Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            pnlCopy.Visible = False
            pnlHd.Visible = True
            btnCopyTo.Enabled = True
        Catch ex As Exception
            Throw New Exception("btnCancel_Click Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSearchFrom_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearchFrom.Click
        Try
            Dim ResultField As String
            Session("filter") = "Select DISTINCT ProductCode, ProductName From VMsProductLotSize WHERE ProductCode <> " + QuotedStr(tbToCode.Text)
            Session("DBConnection") = ViewState("DBConnection")
            ResultField = "ProductCode, ProductName"
            ViewState("Sender") = "btnSearchFrom"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType)            
        Catch ex As Exception
            lstatus.Text = "btnSearchFrom Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub btnSearchTo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearchTo.Click
        Try
            Dim ResultField As String
            Session("filter") = "Select Product_Code, Product_Name From VMsProduct WHERE Product_Code <> " + QuotedStr(tbFromCode.Text)
            Session("DBConnection") = ViewState("DBConnection")
            ResultField = "Product_Code, Product_Name"
            ViewState("Sender") = "btnSearchTo"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType)
        Catch ex As Exception
            lstatus.Text = "btnSearchTo Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub tbFromCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbFromCode.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("ProductLotSizeCopy", tbToCode.Text + "|" + tbFromCode.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbFromCode.Text = Dr("Product_Code")
                tbFromName.Text = Dr("Product_Name")
                tbFromCode.Focus()
            Else
                tbFromCode.Text = ""
                tbFromName.Text = ""
                tbFromCode.Focus()
            End If
            bindDataGrid()
        Catch ex As Exception
            Throw New Exception("tbFromCode change Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbToCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbToCode.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("ProductCopy", tbToCode.Text + "|" + tbFromCode.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbToCode.Text = Dr("Product_Code")
                tbToName.Text = Dr("Product_Name")
                tbToCode.Focus()
            Else
                tbToCode.Text = ""
                tbToName.Text = ""
                tbToCode.Focus()
            End If
            bindDataGrid()
        Catch ex As Exception
            Throw New Exception("tbFromCode change Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnCopy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCopy.Click
        Try
            If tbFromCode.Text = tbToCode.Text Then
                lstatus.Text = "<script language='javascript'>alert('Cannot copy to the same source');</script>"
                Exit Sub
            End If
            If tbFromName.Text.Trim.Length = 0 Then
                lstatus.Text = "<script language='javascript'>alert('Product From must be filled');</script>"
                tbFromCode.Focus()
                Exit Sub
            End If
            If tbToName.Text.Trim.Length = 0 Then
                lstatus.Text = "<script language='javascript'>alert('Product To must be filled');</script>"
                tbToCode.Focus()
                Exit Sub
            End If
            SQLExecuteNonQuery("EXEC S_MsProductLotSizeCopyFrom " + QuotedStr(tbFromCode.Text) + "," + QuotedStr(tbToCode.Text) + "," + ddlGroupByCopy.SelectedIndex.ToString + "," + QuotedStr(ViewState("UserId").ToString), ViewState("DBConnection").ToString)

            pnlCopy.Visible = False
            pnlHd.Visible = True
            btnCopyTo.Enabled = True
            tbProductCode.Text = tbToCode.Text
            tbProductName.Text = tbToName.Text
            bindDataGrid()
        Catch ex As Exception
            Throw New Exception("btnCopy_Click change Error : " + ex.ToString)
        End Try
    End Sub
End Class

