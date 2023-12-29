Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data
Imports CrystalDecisions.Web.Design
Imports System.IO
Imports System.Data.SqlClient

Partial Class Master_MsProduct_MsProduct
    Inherits System.Web.UI.Page
    'Protected GetStringHd As String = "SELECT Product_Code, Product_Name, Product_Materi, Product_Materi_Name, " + _
    '            "ProductCategory, Product_Jenis, Product_Jenis_Name, JenisSize, Product_Size, " + _
    '            "Product_Size_Name, JenisBentuk, Product_Bentuk, Product_Bentuk_Name, JenisType, " + _
    '            "ProductDetail, PartNo, Product_Type, Product_Type_Name, Product_Seri, WorkCenter, CostCtr,  " + _
    '            "Merk, MinQty, MaxQty, Unit,  Specification1, Specification2, Specification3, Specification4, " + _
    '            "ReorderQty, UnitOrder, PurchaseCurr, PurchasePrice, SalesCurr, SalesPrice, BufferQty, " + _
    '            "COGSPrice, HavePart, HaveBarCode,FgKonsinyasi,FgBibit,FgGift, Volume, FgStock, FgPackages, FgActive, ToleranceTT, ToleranceRR, TolerancePO, " + _
    '            "Qty_Lot, Qty_Sample, Percent_Sample, FgProduce, Merk_Name, Fg_QC, FgQA, Fg_Bibit, Length, Height, Width FROM VMsProduct"

    Protected GetStringHd As String = "SELECT * FROM VMsProduct"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        If Not IsPostBack Then
            InitProperty()
            FillCombo(ddlUnit, "EXEC S_GetUnit", True, "Unit_Code", "Unit_Name", ViewState("DBConnection"))
            FillCombo(ddlPJenis, "EXEC S_GetProductJenis", True, "Product_Jenis_Code", "Product_Jenis_Name", ViewState("DBConnection"))
            FillCombo(ddlUnitOrder, "EXEC S_GetUnit", True, "Unit_Code", "Unit_Name", ViewState("DBConnection"))
            FillCombo(ddlPMateri, "EXEC S_GetProductMateri", True, "Product_Materi_Code", "Product_Materi_Name", ViewState("DBConnection"))
            FillCombo(ddlWorkCtr, "EXEC S_GetWorkCtr", False, "WorkCtr_Code", "WorkCtr_Name", ViewState("DBConnection"))
            FillCombo(ddlCostCtr, "EXEC S_GetCostCtr", True, "Cost_Ctr_Code", "Cost_Ctr_Name", ViewState("DBConnection"))
            FillCombo(ddlPSize, "EXEC S_GetProductSize", True, "ProductSize", "SizeName", ViewState("DBConnection"))
            FillCombo(ddlPBentuk, "EXEC S_GetProductBentuk", True, "ProductBentuk", "BentukName", ViewState("DBConnection"))
            FillCombo(ddlPType, "EXEC S_GetProductType", True, "ProductType", "TypeName", ViewState("DBConnection"))
            FillCombo(ddlPSeri, "Select ProductSeri, SeriName From MsProductSeri", True, "ProductSeri", "SeriName", ViewState("DBConnection"))

            SetInit()
            Dim dt As DataTable
            dt = SQLExecuteQuery("Select RptTitle, MenuName, COALESCE(MenuParam,'') AS MenuParam, COALESCE(FastRpt,'') AS FastRpt, COALESCE(ReportType,'') AS ReportType, COALESCE(ReportType2,'') AS ReportType2, COALESCE(FgPrintValue,'N') AS FgPrintValue, COALESCE(FgForceNewPage,'N') AS FgForceNewPage from MsMenu WHERE MenuId = " + QuotedStr(Request.QueryString("ContainerId").ToString), ViewState("DBConnection").ToString).Tables(0)
            lblTitle.Text = dt.Rows(0)("MenuName").ToString

        End If
        If Not Session("Result") Is Nothing Then
            If ViewState("Sender") = "btnPSubGroup" Then
                'FieldResult = "Product_SubGroup_Code, Product_SubGroup_Name, Product_Group_Code, Product_Group_Name"
                tbPSubGroup.Text = Session("Result")(0).ToString
                tbPSubGroupName.Text = Session("Result")(1).ToString
                tbProductGroup.Text = Session("Result")(3).ToString
                Code()
            End If

            If Session("Sender") = "btnSameAdd" Or Session("Sender") = "btnSameEdit" Then
                Dim Same As TextBox
                Dim SameName As TextBox
                If Session("Sender") = "btnSameAdd" Then
                    Same = GridSame.FooterRow.FindControl("ProductSameAdd")
                    SameName = GridSame.FooterRow.FindControl("ProductSameNameAdd")
                Else
                    Same = GridSame.Rows(DataGrid.EditIndex).FindControl("ProductSameEdit")
                    SameName = GridSame.Rows(DataGrid.EditIndex).FindControl("ProductSameNameEdit")
                End If
                Same.Text = Session("Result")(0).ToString
                SameName.Text = Session("Result")(1).ToString
                Same.Focus()
            End If

            If Session("Sender") = "btnProductDtAdd" Or Session("Sender") = "btnProductDtEdit" Then
                Dim ProductDt As TextBox
                Dim ProductDtName, tbUnit As TextBox
                If Session("Sender") = "btnProductDtAdd" Then
                    ProductDt = GridPackages.FooterRow.FindControl("ProductDtAdd")
                    ProductDtName = GridPackages.FooterRow.FindControl("ProductDtNameAdd")
                    tbUnit = GridPackages.FooterRow.FindControl("UnitAdd")
                Else
                    ProductDt = GridPackages.Rows(DataGrid.EditIndex).FindControl("ProductDtEdit")
                    ProductDtName = GridPackages.Rows(DataGrid.EditIndex).FindControl("ProductDtNameEdit")
                    tbUnit = GridPackages.Rows(DataGrid.EditIndex).FindControl("UnitEdit")
                End If
                ProductDt.Text = Session("Result")(0).ToString
                ProductDtName.Text = Session("Result")(1).ToString
                tbUnit.Text = Session("Result")(2).ToString
                ProductDt.Focus()
            End If


            If Session("Sender") = "btnProductPartAdd" Or Session("Sender") = "btnProductPartEdit" Then
                Dim ProductPart As TextBox
                Dim ProductPartName As TextBox
                If Session("Sender") = "btnProductPartAdd" Then
                    ProductPart = GridPackages.FooterRow.FindControl("ProductPartAdd")
                    ProductPartName = GridPackages.FooterRow.FindControl("ProductPartNameAdd")

                Else
                    ProductPart = GridPackages.Rows(DataGrid.EditIndex).FindControl("ProductPartEdit")
                    ProductPartName = GridPackages.Rows(DataGrid.EditIndex).FindControl("ProductPartNameEdit")

                End If
                ProductPart.Text = Session("Result")(0).ToString
                ProductPartName.Text = Session("Result")(1).ToString

                ProductPart.Focus()
            End If

            Session("Result") = Nothing
            ViewState("Sender") = Nothing
            Session("filter") = Nothing
            Session("Column") = Nothing
        End If

            dsLevelBonus.ConnectionString = ViewState("DBConnection")
        dsUnitConvert.ConnectionString = ViewState("DBConnection")
        dsWrhsArea.ConnectionString = ViewState("DBConnection")
            tbLength.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbWidth.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbHeight.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbNetWeight.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbGrossWeight.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbQtyBuffer.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbGramasi.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbMinOrder.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbMultiple.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbTolerance.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbSquare.Attributes.Add("ReadOnly", "True")
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

    Private Sub SetInit()
        ViewState("SortExpression") = Nothing
        DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
        btnAdd.Visible = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
        btnAdd2.Visible = btnAdd.Visible
        'tbLength.Attributes.Add("OnBlur", "setformat(); ")
        'tbWidth.Attributes.Add("OnBlur", "setformat(); ")
        tbLength.Attributes.Add("OnBlur", "kali(" + Me.tbLength.ClientID + "," + Me.tbWidth.ClientID + "," + Me.tbSquare.ClientID + "); ")
        tbWidth.Attributes.Add("OnBlur", "kali(" + Me.tbLength.ClientID + "," + Me.tbWidth.ClientID + "," + Me.tbSquare.ClientID + "); ")
    End Sub


    Function CheckMenuLevel(ByVal CommandName As String) As Boolean
        Try
            If CommandName = "Insert" Then
                If ViewState("MenuLevel").Rows(0)("FgInsert") = "N" Then
                    lstatus.Text = "<script language='javascript'> {alert('You are not authorized to insert record. Please contact administrator')}</script>"
                    Return False
                    Exit Function
                End If
            End If


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
            SqlString = GetStringHd + StrFilter + " ORDER BY Product_Code"
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "Product_Code ASC"
                ViewState("SortOrder") = "ASC"
            End If
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub


    Private Sub bindDataGridDt()
        Dim tempDS As New DataSet()
        Dim DV As DataView
        Dim tbQtyConvert As TextBox
        Dim ddlUnitConvert, ddlUnitEquivalen As DropDownList
        Dim GVR As GridViewRow
        Try
            tempDS = SQLExecuteQuery("Select A.* FROM V_MsProductConvert A WHERE A.ProductCode =" + QuotedStr(ViewState("Nmbr")), ViewState("DBConnection"))

            DV = tempDS.Tables(0).DefaultView

            If DV.Count = 0 Then
                Dim DT As DataTable = tempDS.Tables(0)
                ShowGridViewIfEmpty(DT, DataGridDt)
                DV = DT.DefaultView
            Else
                DV.Sort = ViewState("SortExpressionDt")
                DataGridDt.DataSource = DV
                DataGridDt.DataBind()
            End If
            GVR = DataGridDt.FooterRow
            tbQtyConvert = GVR.FindControl("RateAdd")
            ddlUnitConvert = GVR.FindControl("UnitConvertAdd")
            ddlUnitEquivalen = GVR.FindControl("UnitCodeAdd")
            tbQtyConvert.Text = "0"

            ddlUnitConvert.SelectedValue = ddlUnit.SelectedValue
            ddlUnitEquivalen.SelectedValue = ddlUnit.SelectedValue
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Dt Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Private Sub bindDataGridDtBonus()
        Dim tempDS As New DataSet()
        Dim DV As DataView
        Dim tbPercentage As TextBox
        Dim ddlLevelBonus As DropDownList
        Dim GVR As GridViewRow
        Try
            tempDS = SQLExecuteQuery("Select A.* FROM V_MsProductBonus A WHERE A.ProductCode =" + QuotedStr(ViewState("Nmbr")), ViewState("DBConnection"))

            DV = tempDS.Tables(0).DefaultView

            If DV.Count = 0 Then
                Dim DT As DataTable = tempDS.Tables(0)
                ShowGridViewIfEmpty(DT, GridBonus)
                DV = DT.DefaultView
            Else
                DV.Sort = ViewState("SortExpressionDt")
                GridBonus.DataSource = DV
                GridBonus.DataBind()
            End If
            GVR = GridBonus.FooterRow
            tbPercentage = GVR.FindControl("PercentageAdd")
            ddlLevelBonus = GVR.FindControl("LevelBonusAdd")
            tbPercentage.Text = "0"

        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Dt Error: " & ex.ToString
        Finally
        End Try
    End Sub
    Private Sub bindDataGridDtSame()
        Dim tempDS As New DataSet()
        Dim DV As DataView
        Dim tbSame, tbSameName As TextBox

        Dim GVR As GridViewRow
        Try
            tempDS = SQLExecuteQuery("Select A.* FROM V_MsProductSame A WHERE A.ProductCode =" + QuotedStr(ViewState("Nmbr")), ViewState("DBConnection"))

            DV = tempDS.Tables(0).DefaultView

            If DV.Count = 0 Then
                Dim DT As DataTable = tempDS.Tables(0)
                ShowGridViewIfEmpty(DT, GridSame)
                DV = DT.DefaultView
            Else
                DV.Sort = ViewState("SortExpressionDt")
                GridSame.DataSource = DV
                GridSame.DataBind()
            End If
            GVR = GridSame.FooterRow
            tbSame = GVR.FindControl("ProductSameAdd")
            tbSameName = GVR.FindControl("ProductSameNameAdd")
            tbSame.Text = ""

        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Dt Error: " & ex.ToString
        Finally
        End Try
    End Sub
    Private Sub bindGridPackagesDtProductDt()
        Dim tempDS As New DataSet()
        Dim DV As DataView
        Dim tbProductdt, tbProductdtName As TextBox

        Dim GVR As GridViewRow
        Try
            tempDS = SQLExecuteQuery("Select A.* FROM V_MsProductPackage A WHERE A.ProductCode =" + QuotedStr(ViewState("Nmbr")), ViewState("DBConnection"))

            DV = tempDS.Tables(0).DefaultView

            If DV.Count = 0 Then
                Dim DT As DataTable = tempDS.Tables(0)
                ShowGridViewIfEmpty(DT, GridPackages)
                DV = DT.DefaultView
            Else
                DV.Sort = ViewState("SortExpressionDt")
                GridPackages.DataSource = DV
                GridPackages.DataBind()
            End If
            GVR = GridPackages.FooterRow
            tbProductdt = GVR.FindControl("ProductdtAdd")
            tbProductdtName = GVR.FindControl("ProductdtNameAdd")
            tbProductdt.Text = ""

        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Dt Error: " & ex.ToString
        Finally
        End Try
    End Sub
    Private Sub bindDataGridPart()
        Dim tempDS As New DataSet()
        Dim DV As DataView
        Dim tbPartNo, tbPartName As TextBox
        Dim GVR As GridViewRow
        Try
            tempDS = SQLExecuteQuery("Select A.* FROM V_MsProductPart A WHERE A.ProductCode =" + QuotedStr(ViewState("Nmbr")), ViewState("DBConnection"))

            DV = tempDS.Tables(0).DefaultView

            If DV.Count = 0 Then
                Dim DT As DataTable = tempDS.Tables(0)
                ShowGridViewIfEmpty(DT, GridPart)
                DV = DT.DefaultView
            Else
                DV.Sort = ViewState("SortExpressionDt")
                GridPart.DataSource = DV
                GridPart.DataBind()
            End If
            GVR = GridPart.FooterRow
            tbPartNo = GVR.FindControl("PartNoAdd")
            tbPartName = GVR.FindControl("PartNameAdd")
            tbPartNo.Text = ""

        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Dt Error: " & ex.ToString
        Finally
        End Try
    End Sub
    Private Sub bindDataGridWrhsArea()
        Dim tempDS As New DataSet()
        Dim DV As DataView
        Dim tbQtyMin, tbQtyMax As TextBox
        Dim ddlWrhsArea, ddltransferType As DropDownList
        Dim GVR As GridViewRow
        Try
            tempDS = SQLExecuteQuery("Select A.* FROM V_MsProductWrhsArea A WHERE A.ProductCode =" + QuotedStr(ViewState("Nmbr")), ViewState("DBConnection"))

            DV = tempDS.Tables(0).DefaultView

            If DV.Count = 0 Then
                Dim DT As DataTable = tempDS.Tables(0)
                ShowGridViewIfEmpty(DT, GridWrhsArea)
                DV = DT.DefaultView
            Else
                DV.Sort = ViewState("SortExpressionDt")
                GridWrhsArea.DataSource = DV
                GridWrhsArea.DataBind()
            End If
            GVR = GridWrhsArea.FooterRow
            tbQtyMin = GVR.FindControl("QtyMinAdd")
            tbQtyMax = GVR.FindControl("QtyMaxAdd")
            ddlWrhsArea = GVR.FindControl("WrhsAreaAdd")
            ddltransferType = GVR.FindControl("transfertypeAdd")
            tbQtyMin.Text = "0"
            tbQtyMax.Text = "0"
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Dt Error: " & ex.ToString
        Finally
        End Try
    End Sub
    Protected Sub DataGrid_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGrid.RowCommand
        Dim GVR As GridViewRow = Nothing
        Dim index As Integer
        Dim DDL As DropDownList
        Try
            If Not (e.CommandName = "Sort" Or e.CommandName = "Page") Then
                index = Convert.ToInt32(e.CommandArgument)
                GVR = DataGrid.Rows(index)
            End If
            If e.CommandName = "Go" Then
                DDL = DataGrid.Rows(index).FindControl("ddl")
                ViewState("Nmbr") = GVR.Cells(1).Text
                If DDL.SelectedValue = "View" Then
                    ViewState("State") = "View"
                    FillTextBoxHd(ViewState("Nmbr"))
                    'lbDetailProduct.Text = GVR.Cells(1).Text + " - " + GVR.Cells(2).Text
                    'lbDetailUnit.Text = GVR.Cells(18).Text 'Unit Warehouse                                    
                    btnSave.Visible = False
                    btnReset.Visible = False
                    Menu1.Visible = True
                    Menu1.Items.Item(0).Selected = True
                    MultiView1.Visible = True
                    MultiView1.ActiveViewIndex = 0

                    ModifyInput(False)
                    PnlMain.Visible = False
                    pnlInput.Visible = True
                    pnlDetail.Visible = True
                    ddlFgActive.Enabled = False
                    bindDataGridDt()
                    bindDataGridDtBonus()
                    bindDataGridDtSame()
                    bindGridPackagesDtProductDt()
                    bindDataGridPart()
                    bindDataGridWrhsArea()
                ElseIf DDL.SelectedValue = "Edit" Then
                    Try
                        If CheckMenuLevel("Edit") = False Then
                            Exit Sub
                        End If
                        ViewState("State") = "Edit"
                        ddlFgActive.Enabled = True
                        FillTextBoxHd(ViewState("Nmbr"))
                        If GVR.Cells(26).Text = "Y" Then 'FgActive
                            ModifyInput(True)
                            'tbCode.Enabled = False
                            btnSave.Visible = True
                            btnReset.Visible = True
                        Else
                            ModifyInput(False)
                            btnSave.Visible = True
                            btnReset.Visible = False
                        End If
                        PnlMain.Visible = False
                        pnlInput.Visible = True
                        pnlDetail.Visible = False
                        Menu1.Visible = False
                    Catch ex As Exception
                        lstatus.Text = "DataGrid_Edit Error: " & ex.ToString
                    End Try
                ElseIf DDL.SelectedValue = "Copy New" Then
                    Try
                        If CheckMenuLevel("Insert") = False Then
                            Exit Sub
                        End If
                        ViewState("State") = "Insert"
                        FillTextBoxHd(ViewState("Nmbr"))
                        PnlMain.Visible = False
                        pnlInput.Visible = True
                        btnSave.Visible = True
                        btnReset.Visible = True
                        ddlFgActive.Enabled = False
                        Menu1.Visible = True
                        Menu1.Items.Item(0).Selected = True
                        MultiView1.Visible = True
                        MultiView1.ActiveViewIndex = 0
                        ModifyInput(True)
                    Catch ex As Exception
                        lstatus.Text = "DataGrid_Insert Error: " & ex.ToString
                    End Try
                ElseIf DDL.SelectedValue = "Delete" Then
                    Try
                        If CheckMenuLevel("Delete") = False Then
                            Exit Sub
                        End If

                        SQLExecuteNonQuery("Update MsProduct Set Fgactive = 'N' where ProductCode = '" & ViewState("Nmbr") & "'", ViewState("DBConnection").ToString)
                        bindDataGrid()
                    Catch ex As Exception
                        lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
                    End Try
                ElseIf DDL.SelectedValue = "Photo" Then
                    Try
                        Dim paramgo As String
                        paramgo = GVR.Cells(1).Text + "|" + GVR.Cells(2).Text + "|Product"
                        'If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                        '    Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenTransaction('TrUploadImage', '" + paramgo + "' );", True)
                        'End If
                        AttachScript("OpenTransaction('TrUploadImage', '" + Request.QueryString("KeyId") + "', '" + paramgo + "' );", Page, Me.GetType())
                    Catch ex As Exception
                        lstatus.Text = "DDL.SelectedValue = Menu Error : " + ex.ToString
                    End Try
                End If
            End If

        Catch ex As Exception
            lstatus.Text = lstatus.Text + " DataGrid_RowCommand Error: " & ex.ToString
        End Try
    End Sub


    Protected Sub DataGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGrid.RowDeleting
        'Dim txtID, SqlString As String
        Dim txtID As String
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            'If DataGrid.Rows(e.RowIndex).Cells(8).Text = "N" Then
            '    lstatus.Text = "<script language='javascript'> {alert('Product closed already')}</script>"
            '    Exit Sub
            'End If
            txtID = DataGrid.Rows(e.RowIndex).Cells(0).Text
            SQLExecuteNonQuery("Delete from MsProduct where ProductCode = '" & txtID & "'", ViewState("DBConnection").ToString)
            'SqlString = "Update MsProduct Set FgActive = 'N', UserClose = " + QuotedStr(Session("UserId").ToString) + ", CloseDate = getDate() " + _
            '"Where Account = " + QuotedStr(txtID)
            'SQLExecuteNonQuery(SqlString)
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
    Protected Sub DataGrid_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles DataGrid.RowCancelingEdit
        Try
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid()
        Catch ex As Exception
            lstatus.Text = "DataGrid_Cancel Error : " + vbCrLf + ex.ToString
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
            lstatus.Text = "UserDG_SortCommandError =" + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            pnlInput.Visible = False
            PnlMain.Visible = True
            pnlDetail.Visible = False
        Catch ex As Exception
            lstatus.Text = "btn Cancel Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Try
            ClearHd()
        Catch ex As Exception
            lstatus.Text = "btn Reset Error : " + ex.ToString
        End Try
    End Sub

    Function CekHd() As Boolean
        Try

            If tbCode.Text.Trim = "" Then
                lstatus.Text = MessageDlg("Code Must Have Value")
                tbCode.Focus()
                Exit Function
            End If
            If tbName.Text.Trim = "" Then
                lstatus.Text = MessageDlg("Name Must Have Value")
                tbName.Focus()
                Exit Function
            End If
            If ddlPMateri.SelectedValue.Trim = "" Then
                lstatus.Text = MessageDlg("Product Type Must Have Value")
                ddlPMateri.Focus()
                Exit Function
            End If

            If tbPSubGroup.Text = "" Then
                lstatus.Text = MessageDlg("Sub Group Must Have Value")
                tbPSubGroup.Focus()
                Exit Function
            End If

            If ddlProductMerk.Text = "" Then
                lstatus.Text = MessageDlg("Product Detail Must Have Value")
                ddlProductMerk.Focus()
                Exit Function
            End If

            'If ddlPType.SelectedValue.Trim = "" Then
            '    lstatus.Text = "Product Type Must Have Value"
            '    ddlPType.Focus()
            '    Exit Sub
            'End If

            If ddlWorkCtr.SelectedValue.Trim = "" Then
                lstatus.Text = MessageDlg("Work Center Must Have Value")
                ddlWorkCtr.Focus()
                Exit Function
            End If

            If ddlUnit.SelectedValue.Trim = "" Then
                lstatus.Text = MessageDlg("Unit Must Have Value")
                ddlUnit.Focus()
                Exit Function
            End If

            If ddlUnitOrder.SelectedValue.Trim = "" Then
                lstatus.Text = MessageDlg("Unit Order Must Have Value")
                ddlUnitOrder.Focus()
                Exit Function
            End If

            If ddlFgActive.SelectedValue.Trim = "" Then
                lstatus.Text = MessageDlg("Active Must Have Value")
                ddlFgActive.Focus()
                Exit Function
            End If


            'If ddlCostCtr.SelectedValue.Trim = "" Then
            '    lstatus.Text = "Cost Ctr Must Have Value"
            '    ddlCostCtr.Focus()
            '    Exit Sub
            'End If


            Return True
        Catch ex As Exception
            Throw New Exception("Cek Hd Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim SQLString As String = ""
        Dim CodeName As String = ""
        Try

            If CekHd() = False Then
                Exit Sub
            End If


            If ViewState("State") = "Insert" Then
                If SQLExecuteScalar("SELECT Product_Code From VMsProduct WHERE Product_Code = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Product " + QuotedStr(tbCode.Text) + " has already been exist"
                    Exit Sub
                End If

                'If SQLExecuteScalar("EXEC S_CekProductNameSpec " + QuotedStr(tbName.Text) + "," + QuotedStr(tbSpecification.Text), Session("DBConnection").ToString).Length > 0 Then
                '    lstatus.Text = "Product Name " + QuotedStr(tbName.Text) + " and Specification " + QuotedStr(tbSpecification.Text) + " has already been exist"
                '    Exit Sub
                'End If

                SQLString = "Insert into MsProduct (ProductCode, ProductName, " + _
                "Specification1, Specification2, Specification3, Specification4, ProductType, " + _
                " FgQA, WorkCenter, " + _
                "ProductJenis, ProductMateri, Length, Width, Height, Volume, Weight,  " + _
                "Unit, UnitOrder, QtyLot, QtySample, PercentSample, " + _
                "BufferQty, MinQty, MaxQty, ReorderQty, ToleranceTT, " + _
                "CostCtr, ProductSize, ProductBentuk, ProductSeri, Merk, ProductDetail, PartNo, " + _
                "HaveBarcode, HavePart, FgPackages, FgGift, FgQC, Duty, PPnBM, " + _
                "ToleranceRR, TolerancePO, FgProduce, FgKonsinyasi, FgBibit, COGSPrice, FgStock, " + _
                "FgActive, ProductSubGroup, UserId, UserDate) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(tbName.Text) + ", " + _
                QuotedStr(tbSpecification.Text) + ", " + QuotedStr(tbSpecification2.Text) + ", " + QuotedStr(tbSpecification3.Text) + ", " + QuotedStr(tbSpecification4.Text) + ", " + _
                QuotedStr(ddlPType.SelectedValue) + ", " + _
                QuotedStr(ddlfgLiquid.SelectedValue) + "," + QuotedStr(ddlWorkCtr.SelectedValue) + "," + _
                QuotedStr(ddlPJenis.SelectedValue) + "," + QuotedStr(ddlPMateri.SelectedValue) + "," + _
                (CFloat(tbLength.Text)).ToString + ", " + (CFloat(tbWidth.Text)).ToString + ", " + (CFloat(tbHeight.Text)).ToString + ", " + (CFloat(tbSquare.Text)).ToString + ", " + _
                QuotedStr(CFloat(tbNetWeight.Text)) + ", " + QuotedStr(ddlUnit.SelectedValue) + ", " + QuotedStr(ddlUnitOrder.SelectedValue) + ", " + _
                (CFloat(tbQtyLot.Text)).ToString + ", " + (CFloat(tbGrossWeight.Text)).ToString + ", " + (CFloat(tbpercentsample.Text)).ToString + ", " + _
                (CFloat(tbQtyBuffer.Text)).ToString + ", " + (CFloat(tbMinOrder.Text)).ToString + ", " + _
                (CFloat(tbMultiple.Text)).ToString + ", " + (CFloat(tbTolerance.Text)).ToString + ", " + (CFloat(tbQtyMax.Text)).ToString + ", " + _
                QuotedStr(ddlCostCtr.SelectedValue) + ", " + QuotedStr(ddlPSize.SelectedValue) + ", " + _
                QuotedStr(ddlPBentuk.SelectedValue) + ", " + QuotedStr(ddlPSeri.SelectedValue) + ", " + _
                QuotedStr(ddlProductMerk.Text) + ", " + _
                QuotedStr(ddlProductMerk.Text) + ", " + QuotedStr(tbPartNo.Text) + ", " + _
                QuotedStr(ddlFgHaveBarcode.SelectedValue) + "," + QuotedStr(ddlFgHavePart.SelectedValue) + "," + _
                QuotedStr(ddlfgpackages.SelectedValue) + "," + QuotedStr(ddlfgGift.SelectedValue) + "," + _
                QuotedStr(ddlFgQC.SelectedValue) + ", " + QuotedStr(tbduty.Text) + "," + QuotedStr(tbGramasi.Text) + "," + _
                QuotedStr(tbTRR.Text) + "," + QuotedStr(tbTPO.Text) + "," + _
                QuotedStr(ddlFgProduce.SelectedValue) + "," + _
                QuotedStr(ddlFgKonsiyasi.SelectedValue) + "," + QuotedStr(ddlfgbibit.SelectedValue) + "," + _
                CFloat(tbCOGS.Text).ToString + "," + QuotedStr(ddlFgStock.SelectedValue) + "," + QuotedStr(ddlFgActive.Text) + ", " + QuotedStr(tbPSubGroup.Text) + ", " + _
                QuotedStr(ViewState("UserId").ToString) + ", getDate()"

            ElseIf ViewState("State") = "Edit" Then
                '", ProductSize = " + QuotedStr(ddlPSize.SelectedValue) + ", ProductBentuk = " + QuotedStr(ddlPBentuk.SelectedValue) + _
                '", ProductSeri = " + QuotedStr(ddlPSeri.SelectedValue) + ", 
                '", ProductJenis = " + QuotedStr(ddlPJenis.SelectedValue) + ", ProductType = " + QuotedStr(ddlPType.SelectedValue) + _
                SQLString = "Update MsProduct set Specification1= " + QuotedStr(tbSpecification.Text) + _
                ", ProductName = " + QuotedStr(tbName.Text) + _
                ", Merk = " + QuotedStr(ddlProductMerk.Text) + _
                ", Specification2= " + QuotedStr(tbSpecification2.Text) + _
                ", Specification3= " + QuotedStr(tbSpecification3.Text) + _
                ", Specification4= " + QuotedStr(tbSpecification4.Text) + _
                ", ProductSubGroup= " + QuotedStr(tbPSubGroup.Text) + _
                ", ProductMateri = " + QuotedStr(ddlPMateri.SelectedValue) + _
                ", FgQA = " + QuotedStr(ddlfgLiquid.SelectedValue) + _
                ", WorkCenter = " + QuotedStr(ddlWorkCtr.SelectedValue) + ", CostCtr = " + QuotedStr(ddlCostCtr.SelectedValue) + _
                ", ProductDetail = " + QuotedStr(ddlProductMerk.Text) + ", PartNo = " + QuotedStr(tbPartNo.Text) + _
                ", Length = " + (CFloat(tbLength.Text)).ToString + ", Width = " + (CFloat(tbWidth.Text)).ToString + ", Weight = " + (CFloat(tbNetWeight.Text)).ToString + _
                ", Height = " + (CFloat(tbHeight.Text)).ToString + ", Volume = " + (CFloat(tbSquare.Text)).ToString + _
                ", Unit = " + QuotedStr(ddlUnit.SelectedValue) + ", UnitOrder = " + QuotedStr(ddlUnitOrder.SelectedValue) + _
                ", QtyLot = " + (CFloat(tbQtyLot.Text)).ToString + ", QtySample = " + (CFloat(tbGrossWeight.Text)).ToString + _
                ", PercentSample = " + (CFloat(tbpercentsample.Text)).ToString + _
                ", BufferQty = " + (CFloat(tbQtyBuffer.Text)).ToString + ", MinQty = " + (CFloat(tbMinOrder.Text)).ToString + _
                ", MaxQty = " + (CFloat(tbQtyMax.Text)).ToString + ", ReorderQty = " + (CFloat(tbMultiple.Text)).ToString + _
                ", ToleranceTT = " + (CFloat(tbTolerance.Text)).ToString + ", ToleranceRR = " + (tbTRR.Text) + _
                ", HaveBarcode = " + QuotedStr(ddlFgHaveBarcode.SelectedValue) + ", HavePart = " + QuotedStr(ddlFgHavePart.SelectedValue) + _
                ", FgPackages= " + QuotedStr(ddlfgpackages.SelectedValue) + ", FgGift= " + QuotedStr(ddlfgGift.SelectedValue) + _
                ", FgQC= " + QuotedStr(ddlFgQC.SelectedValue) + ", Duty= " + QuotedStr(tbduty.Text) + ", PPnBM= " + QuotedStr(tbGramasi.Text) + _
                ", TolerancePO = " + (tbTPO.Text) + _
                ", FgProduce= " + QuotedStr(ddlFgProduce.SelectedValue) + ", FgKonsinyasi= " + QuotedStr(ddlFgKonsiyasi.SelectedValue) + ", FgBibit= " + QuotedStr(ddlfgbibit.SelectedValue) + _
                ", FgActive = " + QuotedStr(ddlFgActive.SelectedValue) + _
                ", FgStock= " + QuotedStr(ddlFgStock.SelectedValue) + ", COGSPrice= " + (CFloat(tbCOGS.Text)).ToString + _
                ", UserDate = getDate() where ProductCode = " + QuotedStr(tbCode.Text)
                '", FgQC = " + QuotedStr(ddlFgQC.SelectedValue) + ", FgQCMikro = " ++ QuotedStr(ddlFgMikro.SelectedValue) ", Customer = " + QuotedStr(tbCustCode.Text) +
            End If
            SQLString = Replace(SQLString, "=''", "=NULL")
            SQLString = Replace(SQLString, "= ''", "= NULL")
            SQLString = Replace(SQLString, ",'',", ",NULL,")
            SQLString = Replace(SQLString, ", '',", ",NULL,")
            SQLString = Replace(SQLString, ",'' ,", ",NULL,")
            SQLString = Replace(SQLString, ", '' ,", ",NULL,")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            bindDataGrid()
            ' SQLExecuteNonQuery("EXEC S_MsProductUnitUpdate " + QuotedStr(tbCode.Text), ViewState("DBConnection"))
            pnlInput.Visible = False
            PnlMain.Visible = True
            tbFilter.Text = tbCode.Text
            ddlField.SelectedValue = "Product_Code"
            btnSearch_Click(Nothing, Nothing)
            tbFilter.Text = ""
        Catch ex As Exception
            lstatus.Text = "btn Save Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click, btnAdd2.Click
        Try
            ViewState("State") = "Insert"
            ModifyInput(True)
            PnlMain.Visible = False
            pnlInput.Visible = True
            btnSave.Visible = True
            btnReset.Visible = True
            MultiView1.Visible = False
            Menu1.Visible = False
            ClearHd()
            ddlPJenis.Focus()
        Catch ex As Exception
            lstatus.Text = "btn add Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        Try
            DataGrid.PageIndex = 0
            DataGrid.EditIndex = -1
            bindDataGrid()
            pnlNav.Visible = True
        Catch ex As Exception
            lstatus.Text = "Btn Search Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbCode.Text = ""
            tbName.Text = ""
            tbSpecification.Text = ""
            tbSpecification2.Text = ""
            tbLength.Text = "0"
            tbWidth.Text = "0"
            tbHeight.Text = "0"
            tbSquare.Text = "0"
            tbNetWeight.Text = "0"
            tbGrossWeight.Text = "0"
            tbMinOrder.Text = "0"
            tbQtyBuffer.Text = "0"
            tbGramasi.Text = "0"
            tbMultiple.Text = "0"
            tbTolerance.Text = "0"
            tbTPO.Text = "0"
            tbTRR.Text = "0"
            ddlPType.SelectedIndex = 0
            ddlUnit.SelectedValue = ""
            ddlUnitOrder.SelectedValue = ""
            ddlPJenis.SelectedValue = ""
            ddlPBentuk.SelectedValue = ""
            ddlWorkCtr.SelectedIndex = 0
            ddlPSeri.SelectedIndex = 0
            ddlPSize.SelectedIndex = 0
            ddlPMateri.SelectedValue = ""
            ddlProductMerk.Text = ""
            ddlCostCtr.SelectedIndex = 0
            ddlFgStock.SelectedValue = "N"
            ddlfgLiquid.SelectedValue = "N"
            ddlFgTransfer.SelectedValue = "N"
            ddlFgProduce.SelectedValue = "N"
            ddlFgActive.SelectedValue = "Y"
            ddlFgQC.SelectedValue = "N"
            ddlfgpackages.SelectedValue = "N"
            ddlfgbibit.SelectedValue = "N"
            ddlFgKonsiyasi.SelectedValue = "N"
            ddlFgHaveBarcode.SelectedValue = "N"
            ddlFgHavePart.SelectedValue = "N"
            tbQtyLot.Text = "0"
            tbpercentsample.Text = "0"
            tbCOGS.Text = "0"
            ddlfgGift.SelectedValue = "N"
            tbduty.Text = "0"
            tbQtyMax.Text = "0"
            tbSpecification3.Text = ""
            tbSpecification4.Text = ""
            tbPartNo.Text = ""
            tbProductGroup.Text = ""
            tbPSubGroup.Text = ""
            tbPSubGroupName.Text = ""



        Catch ex As Exception
            Throw New Exception("Clear HD error : " + ex.ToString)
        End Try
    End Sub

    'Protected Sub btnBackDtTop_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnBackDtTop.Click, button2.Click
    '    Try
    '        PnlMain.Visible = True
    '        pnlDetail.Visible = False
    '    Catch ex As Exception
    '        lstatus.Text = "btn Back Dt Error : " + ex.ToString
    '    End Try
    'End Sub
    Protected Sub UnitConvertAdd_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Count As Integer
        Dim dgi As GridViewRow
        Try
            Count = DataGridDt.Controls(0).Controls.Count
            'dgi = DataGridDt.Controls(0).Controls(Count - 2) '-1 for allowpaging = False
            dgi = DataGridDt.FooterRow

        Catch ex As Exception
            lstatus.Text = "UnitConvert Add Index Changed Error : " + ex.ToString
        End Try
    End Sub
    Public Sub DataGridDt_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGridDt.RowCommand
        Dim tbQtyConvert As TextBox
        Dim ddlUnitConvert, ddlUnitEquivalen As DropDownList
        Dim SQLString As String
        Dim GVR As GridViewRow
        Try
            If e.CommandName = "Insert" Then
                GVR = DataGridDt.FooterRow
                tbQtyConvert = GVR.FindControl("RateAdd")
                ddlUnitConvert = GVR.FindControl("UnitConvertAdd")
                ddlUnitEquivalen = GVR.FindControl("UnitCodeAdd")
                
                If CFloat(tbQtyConvert.Text.Trim) = 0 Then
                    lstatus.Text = "<script language='javascript'>alert('Qty must be filled');</script>"
                    tbQtyConvert.Focus()
                    Exit Sub
                End If
                If ddlUnitConvert.SelectedValue = "" Then
                    lstatus.Text = "<script language='javascript'>alert('Unit Convert must be filled');</script>"
                    ddlUnitConvert.Focus()
                    Exit Sub
                End If
                If ddlUnitEquivalen.SelectedValue = "" Then
                    lstatus.Text = "<script language='javascript'>alert('Unit must be filled');</script>"
                    ddlUnitEquivalen.Focus()
                    Exit Sub
                End If
                If ddlUnitConvert.SelectedValue = ddlUnitEquivalen.SelectedValue Then
                    lstatus.Text = "<script language='javascript'>alert('Unit Convert cannot same with Unit');</script>"
                    ddlUnitEquivalen.Focus()
                    Exit Sub
                End If
                
                If Not (ddlUnit.SelectedValue = ddlUnitConvert.SelectedValue Or ddlUnit.SelectedValue = ddlUnitEquivalen.SelectedValue) Then
                    'If Not ((ddlUnitPack.SelectedValue = ddlUnitConvert.SelectedValue And ddlUnitM2.SelectedValue = ddlUnitEquivalen.SelectedValue) Or (ddlUnitPack.SelectedValue = ddlUnitEquivalen.SelectedValue And ddlUnitM2.SelectedValue = ddlUnitConvert.SelectedValue)) Then
                    '    If ddlUnitM2.SelectedValue <> "" And ddlUnitPack.SelectedValue <> "" Then
                    '        lstatus.Text = "Unit or Unit Equivalen must be contain " + ddlUnit.SelectedValue + " or converting " + ddlUnitM2.SelectedValue + " to " + ddlUnitPack.SelectedValue
                    '    Else
                    '        lstatus.Text = "Unit or Unit Equivalen must be contain " + ddlUnit.SelectedValue
                    '    End If
                    '    Exit Sub
                    'End If
                End If

                If SQLExecuteScalar("SELECT ProductCode From MsProductConvert WHERE ProductCode = " + QuotedStr(ViewState("Nmbr")) + " AND UnitConVert = " + QuotedStr(ddlUnitConvert.SelectedValue) + " AND UnitCode = " + QuotedStr(ddlUnitEquivalen.SelectedValue), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Product " + QuotedStr(ViewState("Nmbr")) + " Unit Convert " + QuotedStr(ddlUnitConvert.SelectedValue) + " with Unit " + ddlUnitEquivalen.SelectedValue + " has already been exist"
                    Exit Sub
                End If
                If SQLExecuteScalar("SELECT ProductCode From MsProductConvert WHERE ProductCode = " + QuotedStr(ViewState("Nmbr")) + " AND UnitConVert = " + QuotedStr(ddlUnitEquivalen.SelectedValue) + " AND UnitCode = " + QuotedStr(ddlUnitConvert.SelectedValue), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Product " + QuotedStr(ViewState("Nmbr")) + " Unit Convert " + QuotedStr(ddlUnitEquivalen.SelectedValue) + " with Unit " + ddlUnitConvert.SelectedValue + " has already been exist"
                    Exit Sub
                End If
                SQLString = "Insert Into MsProductConvert (ProductCode, UnitConvert, Rate, UnitCode) " + _
                "SELECT " + QuotedStr(ViewState("Nmbr")) + "," + QuotedStr(ddlUnitConvert.SelectedValue) + "," + _
                tbQtyConvert.Text.Replace(",", "") + "," + QuotedStr(ddlUnitEquivalen.SelectedValue)

                SQLString = Replace(SQLString, "''", "NULL")
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

                bindDataGridDt()
                'SQLExecuteNonQuery("EXEC S_MsProductUnitUpdate " + QuotedStr(ViewState("Nmbr")), ViewState("DBConnection"))
            End If
        Catch ex As Exception
            lstatus.Text = "Item Command Dt Error" + ex.ToString
        End Try
    End Sub
    Public Sub DataGridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGridDt.RowEditing
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGridDt.EditIndex = e.NewEditIndex
            DataGridDt.ShowFooter = False
            bindDataGridDt()
        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try

    End Sub
    Private Sub DataGridDt_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles DataGridDt.RowCancelingEdit
        Try
            DataGridDt.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGridDt.EditIndex = -1
            bindDataGridDt()
        Catch ex As Exception
            lstatus.Text = "DataGridDt_Cancel Error : " + vbCrLf + ex.ToString
        End Try

    End Sub
    Protected Sub DataGridDt_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGridDt.RowUpdating
        Dim tbQty As TextBox
        Dim lbUnitConvert, lbUnitCode As Label
        Dim SQLString As String
        Dim GVR As GridViewRow
        Try
            GVR = DataGridDt.Rows(e.RowIndex)
            tbQty = GVR.FindControl("RateEdit")
            lbUnitConvert = GVR.FindControl("UnitConvertEdit")
            lbUnitCode = GVR.FindControl("UnitCodeEdit")
            
            SQLString = "UPDATE MsProductConvert SET Rate =" + tbQty.Text.Replace(",", "") + _
            " WHERE ProductCode =" + QuotedStr(ViewState("Nmbr")) + " AND UnitConvert =" + QuotedStr(lbUnitConvert.Text) + " AND UnitCode =" + QuotedStr(lbUnitCode.Text)

            SQLString = Replace(SQLString, "''", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            DataGridDt.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGridDt.EditIndex = -1
            bindDataGridDt()
            'SQLExecuteNonQuery("EXEC S_MsProductUnitUpdate " + QuotedStr(ViewState("Nmbr")), ViewState("DBConnection"))

        Catch ex As Exception
            lstatus.Text = lstatus.Text + "Datagrid dt update Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub DataGridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGridDt.RowDeleting
        Dim txtID, txtUnitEquip As Label
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            txtID = DataGridDt.Rows(e.RowIndex).FindControl("UnitConvert") '
            txtUnitEquip = DataGridDt.Rows(e.RowIndex).FindControl("UnitCode")
            SQLExecuteNonQuery("Delete from MsProductConvert where ProductCode = " + QuotedStr(ViewState("Nmbr")) + " AND UnitConvert =" + QuotedStr(txtID.Text) + " and UnitCode = " + QuotedStr(txtUnitEquip.Text), ViewState("DBConnection").ToString)
            bindDataGridDt()
            'SQLExecuteNonQuery("EXEC S_MsProductUnitUpdate " + QuotedStr(ViewState("Nmbr")), ViewState("DBConnection"))
        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub


    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Try
            Dim dr As DataRow
            dr = SQLExecuteQuery(GetStringHd + " WHERE Product_Code = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString).Tables(0).Rows(0)

            ClearHd()

            BindToText(tbCode, dr("Product_Code").ToString)
            BindToText(tbName, dr("Product_Name").ToString)
            BindToText(tbSpecification, dr("Specification1").ToString)
            BindToText(tbSpecification2, dr("Specification2").ToString)
            BindToText(tbSpecification3, dr("Specification3").ToString)
            BindToText(tbSpecification4, dr("Specification4").ToString)
            BindToText(ddlProductMerk, dr("Merk").ToString)
            BindToText(tbPartNo, dr("PartNo").ToString)
            BindToText(tbPSubGroup, dr("Product_Group").ToString)
            BindToText(tbPSubGroupName, dr("Product_GroupName").ToString)
            BindToText(tbProductGroup, dr("ProductGrp_Name").ToString)
            'BindToDropList(ddlPType, dr("Product_Type").ToString)
            'BindToDropList(ddlPJenis, dr("Product_Jenis").ToString)
            'BindToDropList(ddlPSeri, dr("Product_Seri").ToString)
            'BindToDropList(ddlPSize, dr("Product_Size").ToString)
            BindToDropList(ddlPMateri, dr("Product_Materi").ToString)
            'BindToDropList(ddlPMerk, dr("Merk").ToString)
            'BindToDropList(ddlPBentuk, dr("Product_Bentuk").ToString)
            BindToDropList(ddlUnit, dr("Unit").ToString)
            BindToDropList(ddlUnitOrder, dr("UnitOrder").ToString)
            BindToDropList(ddlFgStock, dr("FgStock").ToString)
            BindToDropList(ddlFgProduce, dr("FgProduce").ToString)
            BindToDropList(ddlFgTransfer, dr("FgPackages").ToString)
            BindToDropList(ddlfgLiquid, dr("FgQA").ToString)
            BindToDropList(ddlFgHaveBarcode, dr("HaveBarcode").ToString)
            BindToDropList(ddlFgHavePart, dr("HavePart").ToString)
            BindToDropList(ddlfgpackages, dr("FgPackages").ToString)
            BindToDropList(ddlFgQC, dr("Fg_QC").ToString)
            BindToDropList(ddlFgKonsiyasi, dr("FgKonsinyasi").ToString)
            BindToDropList(ddlfgbibit, dr("FgBibit").ToString)
            BindToDropList(ddlfgGift, dr("FgGift").ToString)
            BindToDropList(ddlWorkCtr, dr("WorkCenter").ToString)
            BindToDropList(ddlCostCtr, dr("CostCtr").ToString)

            If dr("Length").ToString = "" Then
                tbLength.Text = "0"
            Else : BindToText(tbLength, dr("Length").ToString)
            End If
            If dr("Height").ToString = "" Then
                tbHeight.Text = "0"
            Else : BindToText(tbHeight, dr("Height").ToString)
            End If
            If dr("Width").ToString = "" Then
                tbWidth.Text = "0"
            Else : BindToText(tbWidth, dr("Width").ToString)
            End If
            
            If dr("Volume").ToString = "" Then
                tbSquare.Text = "0"
            Else : BindToText(tbSquare, dr("Volume").ToString)
            End If
            If dr("BufferQty").ToString = "" Then
                tbQtyBuffer.Text = "0"
            Else : BindToText(tbQtyBuffer, dr("BufferQty").ToString)
            End If
            If dr("ReorderQty").ToString = "" Then
                tbGramasi.Text = "0"
            Else : BindToText(tbGramasi, dr("ReorderQty").ToString)
            End If
            If dr("COGSPrice").ToString = "" Then
                tbGrossWeight.Text = "0"
            Else : BindToText(tbGrossWeight, dr("COGSPrice").ToString)
            End If
            If dr("MinQty").ToString = "" Then
                tbMinOrder.Text = "0"
            Else : BindToText(tbMinOrder, dr("MinQty").ToString)
            End If
            If dr("MaxQty").ToString = "" Then
                tbMultiple.Text = "0"
            Else : BindToText(tbMultiple, dr("MaxQty").ToString)
            End If
            If dr("ToleranceTT").ToString = "" Then
                tbTolerance.Text = "0"
            Else : BindToText(tbTolerance, dr("ToleranceTT").ToString)
            End If
            BindToDropList(ddlFgActive, dr("FgActive").ToString)
            If ddlFgActive.SelectedValue = "Y" Then
                ddlFgActive.Enabled = False
            Else : ddlFgActive.Enabled = True
            End If
            ddlPJenis.Focus()
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub
    Private Sub ModifyInput(ByVal State As Boolean)
        tbCode.Enabled = State
        tbName.Enabled = State
        tbSpecification.Enabled = State
        tbSpecification2.Enabled = State
        ddlPType.Enabled = State
        ddlFgProduce.Enabled = State
        ddlFgTransfer.Enabled = State
        ddlfgLiquid.Enabled = State
        ddlWorkCtr.Enabled = State
        tbLength.Enabled = State
        tbWidth.Enabled = State
        tbHeight.Enabled = State
        tbNetWeight.Enabled = State
        tbGrossWeight.Enabled = State
        ddlUnit.Enabled = State
        ddlUnitOrder.Enabled = State
        tbMinOrder.Enabled = State
        tbQtyBuffer.Enabled = State
        tbGramasi.Enabled = State
        tbMultiple.Enabled = State
        tbTolerance.Enabled = State
        tbTPO.Enabled = State
        tbTRR.Enabled = State
        ddlPJenis.Enabled = State
        ddlPBentuk.Enabled = State
        ddlPSeri.Enabled = State
        ddlPSize.Enabled = State
        ddlPMateri.Enabled = State
        ddlProductMerk.Enabled = State
        ddlFgQC.Enabled = State
        ddlfgpackages.Enabled = State
        ddlfgbibit.Enabled = State
        ddlFgKonsiyasi.Enabled = State
        ddlFgHaveBarcode.Enabled = State
        ddlFgHavePart.Enabled = State
        tbQtyLot.Enabled = State
        tbpercentsample.Enabled = State
        tbCOGS.Enabled = State
        ddlfgGift.Enabled = State
        tbduty.Enabled = State
        tbQtyMax.Enabled = State
        tbSpecification3.Enabled = State
        tbSpecification4.Enabled = State
        tbPartNo.Enabled = State

    End Sub
   

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim StrFilter As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("PrintType") = "Print"
            Session("SelectCommand") = "EXEC S_FormPrintMaster5 'VMsProduct','Product_Code', 'Product_Name', 'ProductType_Code +'' - ''+ ProductType_Name', 'ProductCategory', 'Product_Group +'' - ''+ Product_GroupName +'' - ''+ ProductGrp_Name', " + QuotedStr(lblTitle.Text) + ", 'Product Code', 'Product Name', 'Product Type', 'Product Category', 'Product Sub Group', " + QuotedStr(StrFilter) + "," + QuotedStr(ViewState("UserId").ToString)
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster5.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            lstatus.Text = "btn print Error = " + ex.ToString
        End Try
    End Sub

    'Protected Sub ddlFgQC_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFgQC.TextChanged
    '    If ddlFgQC.Text = "Y" Then
    '        ddlFgMikro.Enabled = True
    '    Else
    '        ddlFgMikro.Text = "N"
    '        ddlFgMikro.Enabled = False
    '    End If
    'End Sub


    Protected Sub ddlPType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPType.TextChanged
        Try
            Code()
            'ddlFgStock.SelectedValue = SQLExecuteScalar("SELECT FgStock FROM VMsProductType WHERE Type_Code = " + QuotedStr(ddlPType.SelectedValue), ViewState("DBConnection"))
        Catch ex As Exception
            lstatus.Text = "ddlPType_TextChanged Error = " + ex.ToString
        End Try
    End Sub

    Protected Sub tbHeight_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbHeight.TextChanged
        ' tbSquare2.Text = (CFloat(tbHeight.Text) * CFloat(tbLength.Text)).ToString
    End Sub

    Protected Sub tbLength_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbLength.TextChanged
        ' tbSquare2.Text = (CFloat(tbHeight.Text) * CFloat(tbLength.Text)).ToString
    End Sub

    Protected Sub ddlQC_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFgQC.SelectedIndexChanged

    End Sub

    Protected Sub ddlPJenis_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPJenis.SelectedIndexChanged
        Dim SqlString, SqlString1, SqlString2, SqlString3, SqlString4, SqlString5, SqlString6 As String
        Try
            '  ddlPMateri.SelectedValue = SQLExecuteScalar("SELECT Materi FROM MsProductJenis WHERE JenisCode = " + QuotedStr(ddlPJenis.SelectedValue), ViewState("DBConnection"))
            Code()
            'FillCombo(ddlPMateri, "EXEC S_GetProductJenisMateri " + QuotedStr(ddlPJenis.SelectedValue), True, "Materi", "MateriName", ViewState("DBConnection"))
            'FillCombo(ddlPSize, "Select ProductSize FROM MsProductSize Where ProductJenis = " + QuotedStr(ddlPJenis.SelectedValue), True, "ProductSize", "SizeName", ViewState("DBConnection"))
            ' FillCombo(ddlPBentuk, "Select ProductBentuk FROM MsProductBentuk Where ProductJenis= " + QuotedStr(ddlPJenis.SelectedValue), True, "ProductBentuk", "BentukName", ViewState("DBConnection"))
            ' FillCombo(ddlPType, "Select ProductType FROM MsProductType Where ProductJenis= " + QuotedStr(ddlPJenis.SelectedValue), True, "ProductType", "TypeName", ViewState("DBConnection"))
            ' FillCombo(ddlPSeri, "Select ProductSeri FROM MsProductSeri Where ProductJenis= " + QuotedStr(ddlPJenis.SelectedValue) + " AND ProductType = " + QuotedStr(ddlPType.SelectedValue), True, "ProductSeri", "SeriName", ViewState("DBConnection"))

            SqlString = SQLExecuteScalar("SELECT Materi FROM MsProductJenis WHERE JenisCode = " + QuotedStr(ddlPJenis.SelectedValue), ViewState("DBConnection"))
            SqlString2 = SQLExecuteScalar("Select ProductSize FROM MsProductSize Where ProductJenis = " + QuotedStr(ddlPJenis.SelectedValue), ViewState("DBConnection"))
            SqlString3 = SQLExecuteScalar("Select ProductBentuk FROM MsProductBentuk Where ProductJenis= " + QuotedStr(ddlPJenis.SelectedValue), ViewState("DBConnection"))
            SqlString4 = SQLExecuteScalar("Select ProductType FROM MsProductType Where ProductJenis= " + QuotedStr(ddlPJenis.SelectedValue), ViewState("DBConnection"))
            SqlString5 = SQLExecuteScalar("Select ProductSeri FROM MsProductSeri Where ProductJenis= " + QuotedStr(ddlPJenis.SelectedValue), ViewState("DBConnection"))
            SqlString6 = SQLExecuteScalar("SELECT FgStock FROM MsProductMateri WHERE MateriCode = " + QuotedStr(SqlString), ViewState("DBConnection"))
            'lstatus.Text = SqlString3
            ' Exit Sub

            BindToDropList(ddlPMateri, SqlString)
            BindToDropList(ddlPSize, SqlString2)
            BindToDropList(ddlPBentuk, SqlString3)
            BindToDropList(ddlPType, SqlString4)
            BindToDropList(ddlPSeri, SqlString5)
            BindToDropList(ddlFgStock, SqlString6)

        Catch ex As Exception
            lstatus.Text = "ddlPJenis_SelectedIndexChanged Error = " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlPMateri_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPMateri.SelectedIndexChanged
        Dim FgStock As String
        Try

            FgStock = SQLExecuteScalar("SELECT FgStock FROM MsProductMateri WHERE MateriCode = " + QuotedStr(ddlPMateri.SelectedValue), ViewState("DBConnection"))
            ddlFgStock.SelectedValue = FgStock
            Code()

        Catch ex As Exception
            lstatus.Text = "ddlPJenis_SelectedIndexChanged Error = " + ex.ToString
        End Try
    End Sub



    Protected Sub Code()
        Dim Code, Name As String
        Try

            'Code = ddlPJenis.SelectedValue + ddlPSize.SelectedValue + ddlPBentuk.SelectedValue + ddlPType.SelectedValue + ddlPSeri.SelectedValue + ddlProductMerk.Text
            'Name = ddlPJenis.SelectedItem.Text + ddlPSize.SelectedItem.Text + ddlPBentuk.SelectedItem.Text + ddlPType.SelectedItem.Text + ddlPSeri.SelectedItem.Text + ddlProductMerk.Text

            Code = ddlPMateri.SelectedValue + "" + tbPSubGroup.Text + "" + ddlProductMerk.Text

            If ViewState("State") = "Insert" Then
                tbCode.Text = Code
                'tbName.Text = Name
            End If
        Catch ex As Exception
            lstatus.Text = "Code Error = " + ex.ToString
        End Try
        
    End Sub

    Protected Sub ddlPSize_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPSize.SelectedIndexChanged
        Try
            Code()
        Catch ex As Exception
            lstatus.Text = "ddlPSize_SelectedIndexChanged Error = " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlPBentuk_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPBentuk.SelectedIndexChanged
        Try
            Code()
        Catch ex As Exception
            lstatus.Text = "ddlPBentuk_SelectedIndexChanged Error = " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlPSeri_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPSeri.TextChanged
        Try
            Code()
        Catch ex As Exception
            lstatus.Text = "ddlPSeri_TextChanged Error = " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlProductMerk_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlProductMerk.TextChanged
        Try
            Code()
        Catch ex As Exception
            lstatus.Text = "ddlProductMerk_TextChanged Error = " + ex.ToString
        End Try
    End Sub
    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        MultiView1.ActiveViewIndex = Int32.Parse(e.Item.Value)
        Dim i As Integer
        Dim paramgo As String
        'Make the selected menu item reflect the correct imageurl

        For i = 0 To Menu1.Items.Count - 1
            If i = e.Item.Value Then
                'Menu1.Items(i).ImageUrl = "selectedtab.gif"
            ElseIf Menu1.Items(7).Selected = True Then
                Dim GVR As GridViewRow = Nothing
                Try
                    If CheckMenuLevel("Insert") = False Then
                        Exit Sub
                    End If
                    paramgo = tbCode.Text + "|" + tbName.Text
                    If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                        Session("DBConnection") = ViewState("DBConnection")
                        Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenAssign('Product', '" + Request.QueryString("KeyId") + "','" + tbCode.Text + "|" + tbName.Text + "','AssCustProduct');", True)
                    End If
                Catch ex As Exception
                    lstatus.Text = "DDL.SelectedValue = 4 Error : " + ex.ToString
                End Try
            ElseIf Menu1.Items(8).Selected = True Then
                Dim GVR As GridViewRow = Nothing
                Try
                    If CheckMenuLevel("Insert") = False Then
                        Exit Sub
                    End If
                    paramgo = tbCode.Text + "|" + tbName.Text
                    If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                        Session("DBConnection") = ViewState("DBConnection")
                        Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenAssign('Product', '" + Request.QueryString("KeyId") + "','" + tbCode.Text + "|" + tbName.Text + "','AssSuppProduct');", True)
                    End If
                Catch ex As Exception
                    lstatus.Text = "DDL.SelectedValue = 4 Error : " + ex.ToString
                End Try
            End If
        Next

        'For i = 0 To Menu1.Items.Count - 1
        '    If i = e.Item.Value Then
        '        'Menu1.Items(i).ImageUrl = "selectedtab.gif"
        '    ElseIf Menu1.Items(6).Selected = True Then
        '        Dim GVR As GridViewRow = Nothing
        '        Try
        '            If CheckMenuLevel("Insert") = False Then
        '                Exit Sub
        '            End If
        '            paramgo = tbCode.Text + "|" + tbName.Text
        '            If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
        '                Session("DBConnection") = ViewState("DBConnection")
        '                Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenAssign('Product', '" + Request.QueryString("KeyId") + "','" + tbCode.Text + "|" + tbName.Text + "','AssCustProduct');", True)
        '            End If
        '        Catch ex As Exception
        '            lstatus.Text = "DDL.SelectedValue = 5 Error : " + ex.ToString
        '        End Try
        '    ElseIf Menu1.Items(7).Selected = True Then
        '        Dim GVR As GridViewRow = Nothing
        '        Try
        '            If CheckMenuLevel("Insert") = False Then
        '                Exit Sub
        '            End If
        '            paramgo = tbCode.Text + "|" + tbName.Text
        '            If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
        '                Session("DBConnection") = ViewState("DBConnection")
        '                Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenAssign('Product', '" + Request.QueryString("KeyId") + "','" + tbCode.Text + "|" + tbName.Text + "','AssSuppProduct');", True)
        '            End If
        '        Catch ex As Exception
        '            lstatus.Text = "DDL.SelectedValue = 4 Error : " + ex.ToString
        '        End Try
        '    End If

        '  Next
    End Sub

    Protected Sub GridBonus_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles GridBonus.RowCancelingEdit
        Try
            GridBonus.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            GridBonus.EditIndex = -1
            bindDataGridDtBonus()
        Catch ex As Exception
            lstatus.Text = "GridBonus_RowCancelingEdit Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub GridBonus_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridBonus.RowCommand
        Dim tbPercentage As TextBox
        Dim ddlLevelBonus As DropDownList
        Dim SQLString As String
        Dim GVR As GridViewRow
        Try
            If e.CommandName = "Insert" Then
                GVR = GridBonus.FooterRow
                tbPercentage = GVR.FindControl("PercentageAdd")
                ddlLevelBonus = GVR.FindControl("LevelBonusAdd")

                If CFloat(tbPercentage.Text.Trim) = 0 Then
                    lstatus.Text = "<script language='javascript'>alert('Percentage must be filled');</script>"
                    tbPercentage.Focus()
                    Exit Sub
                End If
                If ddlLevelBonus.SelectedValue = "" Then
                    lstatus.Text = "<script language='javascript'>alert('Bonus must be filled');</script>"
                    ddlLevelBonus.Focus()
                    Exit Sub
                End If

                If SQLExecuteScalar("SELECT ProductCode From MsProductBonus WHERE ProductCode = " + QuotedStr(ViewState("Nmbr")) + " AND LevelBonus = " + QuotedStr(ddlLevelBonus.SelectedValue), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Product " + QuotedStr(ViewState("Nmbr")) + " Unit Bonus " + QuotedStr(ddlLevelBonus.SelectedValue) + " has already been exist"
                    Exit Sub
                End If
                SQLString = "Insert Into MsProductBonus (ProductCode, LevelBonus, Percentage) " + _
                "SELECT " + QuotedStr(ViewState("Nmbr")) + "," + QuotedStr(ddlLevelBonus.SelectedValue) + "," + _
                tbPercentage.Text.Replace(",", "")

                SQLString = Replace(SQLString, "''", "NULL")
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

                bindDataGridDtBonus()

            End If
        Catch ex As Exception
            lstatus.Text = "GridBonus_RowCommand Error" + ex.ToString
        End Try
    End Sub

    Protected Sub GridBonus_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridBonus.RowDeleting
        Dim txtID As Label
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            txtID = GridBonus.Rows(e.RowIndex).FindControl("LevelBonus")
            SQLExecuteNonQuery("Delete from MsProductBonus where ProductCode = " + QuotedStr(ViewState("Nmbr")) + " AND LevelBonus =" + QuotedStr(txtID.Text), ViewState("DBConnection").ToString)
            bindDataGridDtBonus()

        Catch ex As Exception
            lstatus.Text = "GridBonus_RowDeleting Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub GridBonus_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridBonus.RowEditing
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            GridBonus.EditIndex = e.NewEditIndex
            GridBonus.ShowFooter = False
            bindDataGridDtBonus()
        Catch ex As Exception
            lstatus.Text = "GridBonus_RowEditing exception : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridBonus_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles GridBonus.RowUpdating
        Dim tbPercentage As TextBox
        Dim lbLevelBonus As Label
        Dim SQLString As String
        Dim GVR As GridViewRow
        Try
            GVR = GridBonus.Rows(e.RowIndex)
            tbPercentage = GVR.FindControl("PercentageEdit")
            lbLevelBonus = GVR.FindControl("LevelBonusEdit")

            SQLString = "UPDATE MsProductBonus SET Percentage =" + tbPercentage.Text.Replace(",", "") + _
            " WHERE ProductCode =" + QuotedStr(ViewState("Nmbr")) + " AND LevelBonus =" + QuotedStr(lbLevelBonus.Text)
            
            SQLString = Replace(SQLString, "''", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            GridBonus.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            GridBonus.EditIndex = -1
            bindDataGridDtBonus()

        Catch ex As Exception
            lstatus.Text = lstatus.Text + "Datagrid dt update Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridSame_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles GridSame.RowCancelingEdit
        Try
            GridSame.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            GridSame.EditIndex = -1
            bindDataGridDtSame()
        Catch ex As Exception
            lstatus.Text = "GridSame_RowCancelingEdit Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub GridSame_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridSame.RowCommand
        Dim tbProductSame As TextBox
        Dim tbProductSameName As TextBox
        Dim SQLString As String
        Dim GVR As GridViewRow
        Try
            If e.CommandName = "Insert" Then
                GVR = GridSame.FooterRow
                tbProductSame = GVR.FindControl("ProductSameAdd")
                tbProductSameName = GVR.FindControl("ProductSameNameAdd")

                If CFloat(tbProductSame.Text.Trim) = 0 Then
                    lstatus.Text = "<script language='javascript'>alert('Product must be filled');</script>"
                    tbProductSame.Focus()
                    Exit Sub
                End If
                If tbProductSameName.Text = "" Then
                    lstatus.Text = "<script language='javascript'>alert('Product Same Name must be filled');</script>"
                    tbProductSameName.Focus()
                    Exit Sub
                End If

                If SQLExecuteScalar("SELECT ProductCode From MsProductSame WHERE ProductCode = " + QuotedStr(ViewState("Nmbr")) + " AND ProductSame = " + QuotedStr(tbProductSame.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Product " + QuotedStr(ViewState("Nmbr")) + " Product Same " + QuotedStr(tbProductSameName.Text) + " has already been exist"
                    Exit Sub
                End If
                SQLString = "Insert Into MsProductSame (ProductCode, ProductSame, Userid, UserDate) " + _
                "SELECT " + QuotedStr(ViewState("Nmbr")) + "," + QuotedStr(tbProductSame.Text) + "," + _
                QuotedStr(ViewState("UserId")) + ", Getdate() "

                SQLString = Replace(SQLString, "''", "NULL")
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

                bindDataGridDtSame()
            ElseIf e.CommandName = "btnSameAdd" Or e.CommandName = "btnSameEdit" Then
                Dim FieldResult As String
                Session("DBConnection") = ViewState("DBConnection")
                If e.CommandName = "btnSameAdd" Then
                    Session("filter") = "Select * FROM VMsProduct "
                    Session("Sender") = "btnSameAdd"
                Else
                    Session("filter") = "Select * FROM VMsProduct"
                    Session("Sender") = "btnSameEdit"
                End If

                FieldResult = "Product_Code, Product_Name"
                Session("Column") = FieldResult.Split(",")
                AttachScript("OpenSearchDlg();", Page, Me.GetType())
            End If
        Catch ex As Exception
            lstatus.Text = "GridSame_RowCommand Error" + ex.ToString
        End Try
    End Sub

    Protected Sub GridSame_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridSame.RowDeleting
        Dim txtID As Label
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            txtID = GridSame.Rows(e.RowIndex).FindControl("ProductSame")
            SQLExecuteNonQuery("Delete from MsProductSame where ProductCode = " + QuotedStr(ViewState("Nmbr")) + " AND ProductSame =" + QuotedStr(txtID.Text), ViewState("DBConnection").ToString)
            bindDataGridDtSame()

        Catch ex As Exception
            lstatus.Text = "GridSame_RowDeleting Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub GridSame_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridSame.RowEditing
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            GridSame.EditIndex = e.NewEditIndex
            GridSame.ShowFooter = False
            bindDataGridDtSame()
        Catch ex As Exception
            lstatus.Text = "GridSame_RowEditing exception : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridSame_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles GridSame.RowUpdating
        Dim tbProductSame As TextBox

        Dim SQLString As String
        Dim GVR As GridViewRow
        Try
            GVR = GridSame.Rows(e.RowIndex)
            tbProductSame = GVR.FindControl("ProductSameEdit")


            SQLString = "UPDATE MsProductSame SET ProductSame =" + tbProductSame.Text.Replace(",", "") + _
            " WHERE ProductCode =" + QuotedStr(ViewState("Nmbr")) + " AND ProductSame =" + QuotedStr(tbProductSame.Text)

            SQLString = Replace(SQLString, "''", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            GridSame.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            GridSame.EditIndex = -1
            bindDataGridDtSame()

        Catch ex As Exception
            lstatus.Text = lstatus.Text + "Datagrid dt update Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub ProductSameEdit_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dr As DataRow
        Dim ds As DataSet
        Dim Same As TextBox
        Dim SameName As Label
        Dim Count As Integer
        Dim dgi As GridViewRow
        Try
            Count = DataGrid.EditIndex
            dgi = DataGrid.Rows(Count)
            Same = dgi.FindControl("ProductSameEdit")
            SameName = dgi.FindControl("ProductSameNameEdit")


            ds = SQLExecuteQuery("Select Product_Code, Product_Name From VMsProductSame WHERE ProductSame = " + QuotedStr(Same.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                Same.Text = ""
                SameName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                Same.Text = dr("Product_Code").ToString
                SameName.Text = dr("Product_Name").ToString
            End If
        Catch ex As Exception
            lstatus.Text = "tb Same Changed Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub ProductSameAdd_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dr As DataRow
        Dim ds As DataSet
        Dim Same, tb As TextBox
        Dim SameName As Label
        Dim Count As Integer
        Dim dgi As GridViewRow
        Try
            tb = sender
            If tb.ID = "ProductSameAdd" Then
                Count = DataGrid.Controls(0).Controls.Count
                dgi = DataGrid.Controls(0).Controls(Count - 2) '-1 for allowpaging = False
                Same = dgi.FindControl("ProductSameAdd")
                SameName = dgi.FindControl("ProductSameNameAdd")
            Else
                Count = DataGrid.EditIndex
                dgi = DataGrid.Rows(Count)
                Same = dgi.FindControl("ProductSameEdit")
                SameName = dgi.FindControl("ProductSameNameEdit")
            End If
            ds = SQLExecuteQuery("Select Product_Code, Product_Name AS ProductSameName From VMsProduct WHERE ProductSame = " + QuotedStr(Same.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                Same.Text = ""
                SameName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                Same.Text = dr("Product_Code").ToString
                SameName.Text = dr("Product_Name").ToString
            End If
        Catch ex As Exception
            lstatus.Text = "ProductSameAdd_TextChanged Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub GridPackages_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles GridPackages.RowCancelingEdit
        Try
            GridPackages.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            GridPackages.EditIndex = -1
            bindGridPackagesDtProductDt()
        Catch ex As Exception
            lstatus.Text = "GridPackages_RowCancelingEdit Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub GridPackages_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridPackages.RowCommand
        Dim tbProductDt, tbProductPart As TextBox
        Dim tbProductDtName, tbProductPartName, tbQty, tbunit, tbpercentage As TextBox
        Dim ddlFgMain As DropDownList
        Dim SQLString As String
        Dim GVR As GridViewRow
        Try
            If e.CommandName = "Insert" Then
                GVR = GridPackages.FooterRow
                tbProductDt = GVR.FindControl("ProductDtAdd")
                tbProductDtName = GVR.FindControl("ProductDtNameAdd")
                tbProductPart = GVR.FindControl("ProductPartAdd")
                tbProductPartName = GVR.FindControl("ProductPartNameAdd")
                tbQty = GVR.FindControl("QtyAdd")
                tbunit = GVR.FindControl("UnitAdd")
                tbpercentage = GVR.FindControl("PercentageAdd")
                ddlFgMain = GVR.FindControl("FgMainAdd")

                If CFloat(tbQty.Text.Trim) = 0 Then
                    lstatus.Text = "<script language='javascript'>alert('Qty must be filled');</script>"
                    tbProductDt.Focus()
                    Exit Sub
                End If
                If tbProductPartName.Text = "" Then
                    lstatus.Text = "<script language='javascript'>alert('Part Name must be filled');</script>"
                    tbProductDtName.Focus()
                    Exit Sub
                End If
                If tbProductDtName.Text = "" Then
                    lstatus.Text = "<script language='javascript'>alert('Item Name must be filled');</script>"
                    tbProductDtName.Focus()
                    Exit Sub
                End If

                If SQLExecuteScalar("SELECT ProductCode From MsProductPackage WHERE ProductCode = " + QuotedStr(ViewState("Nmbr")) + " AND ProductDt = " + QuotedStr(tbProductDt.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Product " + QuotedStr(ViewState("Nmbr")) + " Item " + QuotedStr(tbProductDtName.Text) + " has already been exist"
                    Exit Sub
                End If
                SQLString = "Insert Into MsProductPackage (ProductCode, ProductDt, PartDt, Qty, Unit, Percentage, FgMain) " + _
                "SELECT " + QuotedStr(ViewState("Nmbr")) + "," + QuotedStr(tbProductDt.Text) + "," + QuotedStr(tbProductPart.Text) + "," + _
                QuotedStr(tbQty.Text) + "," + QuotedStr(tbunit.Text) + "," + tbpercentage.Text.Replace(",", "") + "," + QuotedStr(ddlFgMain.SelectedValue)

                SQLString = Replace(SQLString, "''", "NULL")
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

                bindGridPackagesDtProductDt()
            ElseIf e.CommandName = "btnProductDtAdd" Or e.CommandName = "btnProductDtEdit" Then
                Dim FieldResult As String
                Session("DBConnection") = ViewState("DBConnection")
                If e.CommandName = "btnProductDtAdd" Then
                    Session("filter") = "Select * FROM VMsProduct "
                    Session("Sender") = "btnProductDtAdd"
                Else
                    Session("filter") = "Select * FROM VMsProduct"
                    Session("Sender") = "btnProductDtEdit"
                End If

                FieldResult = "Product_Code, Product_Name, Unit"
                Session("Column") = FieldResult.Split(",")
                AttachScript("OpenSearchDlg();", Page, Me.GetType())
            ElseIf e.CommandName = "btnProductPartAdd" Or e.CommandName = "btnProductPartEdit" Then
                Dim FieldResult As String
                Session("DBConnection") = ViewState("DBConnection")
                If e.CommandName = "btnProductPartAdd" Then
                    Session("filter") = "Select * FROM VMsProduct "
                    Session("Sender") = "btnProductPartAdd"
                Else
                    Session("filter") = "Select * FROM VMsProduct"
                    Session("Sender") = "btnProductPartEdit"
                End If

                FieldResult = "Product_Code, Product_Name"
                Session("Column") = FieldResult.Split(",")
                AttachScript("OpenSearchDlg();", Page, Me.GetType())
            End If
        Catch ex As Exception
            lstatus.Text = "GridPackages_RowCommand Error" + ex.ToString
        End Try
    End Sub

    Protected Sub GridPackages_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridPackages.RowDeleting
        Dim txtID As Label
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            txtID = GridPackages.Rows(e.RowIndex).FindControl("ProductDt")
            SQLExecuteNonQuery("Delete from MsProductPackage where ProductCode = " + QuotedStr(ViewState("Nmbr")) + " AND ProductDt =" + QuotedStr(txtID.Text), ViewState("DBConnection").ToString)
            bindGridPackagesDtProductDt()

        Catch ex As Exception
            lstatus.Text = "GridPackages_RowDeleting Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub GridPackages_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridPackages.RowEditing
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            GridPackages.EditIndex = e.NewEditIndex
            GridPackages.ShowFooter = False
            bindGridPackagesDtProductDt()
        Catch ex As Exception
            lstatus.Text = "GridPackages_RowEditing exception : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridPackages_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles GridPackages.RowUpdating
        Dim tbProductDt, tbproductPart, tbQty, tbUnit, tbprecentage As TextBox
        Dim DdlFgMAin As DropDownList

        Dim SQLString As String
        Dim GVR As GridViewRow
        Try
            GVR = GridPackages.Rows(e.RowIndex)
            tbProductDt = GVR.FindControl("ProductDtEdit")
            tbproductPart = GVR.FindControl("ProductPartEdit")
            tbQty = GVR.FindControl("QtyEdit")
            tbUnit = GVR.FindControl("UnitEdit")
            DdlFgMAin = GVR.FindControl("FgMainEdit")
            tbprecentage = GVR.FindControl("PrecentageEdit")

            SQLString = "UPDATE MsProductPackage SET PartDt =" + QuotedStr(tbproductPart.Text) + _
            ", Qty =" + tbQty.Text.Replace(",", "") + ", Unit= " + QuotedStr(tbUnit.Text) + ", " + _
            ", Precentage= " + tbprecentage.Text.Replace(",", "") + ", FgMain = " + QuotedStr(DdlFgMAin.SelectedValue) + _
            "WHERE ProductCode =" + QuotedStr(ViewState("Nmbr")) + " AND ProductDt =" + QuotedStr(tbProductDt.Text)

            SQLString = Replace(SQLString, "''", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            GridPackages.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            GridPackages.EditIndex = -1
            bindGridPackagesDtProductDt()

        Catch ex As Exception
            lstatus.Text = lstatus.Text + "GridPackages dt update Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub ProductDtEdit_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dr As DataRow
        Dim ds As DataSet
        Dim ProductDt As TextBox
        Dim ProductDtName As Label
        Dim Count As Integer
        Dim dgi As GridViewRow
        Try
            Count = GridPackages.EditIndex
            dgi = GridPackages.Rows(Count)
            ProductDt = dgi.FindControl("ProductDtEdit")
            ProductDtName = dgi.FindControl("ProductDtNameEdit")


            ds = SQLExecuteQuery("Select Product_Code, Product_Name From VMsProduct WHERE ProductDt = " + QuotedStr(ProductDt.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                ProductDt.Text = ""
                ProductDtName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                ProductDt.Text = dr("Product_Code").ToString
                ProductDtName.Text = dr("Product_Name").ToString
            End If
        Catch ex As Exception
            lstatus.Text = "tb ProductDt Changed Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub ProductDtAdd_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dr As DataRow
        Dim ds As DataSet
        Dim ProductDt, tb As TextBox
        Dim ProductDtName As Label
        Dim Count As Integer
        Dim dgi As GridViewRow
        Try
            tb = sender
            If tb.ID = "ProductDtAdd" Then
                Count = GridPackages.Controls(0).Controls.Count
                dgi = GridPackages.Controls(0).Controls(Count - 2) '-1 for allowpaging = False
                ProductDt = dgi.FindControl("ProductDtAdd")
                ProductDtName = dgi.FindControl("ProductDtNameAdd")
            Else
                Count = GridPackages.EditIndex
                dgi = GridPackages.Rows(Count)
                ProductDt = dgi.FindControl("ProductDtEdit")
                ProductDtName = dgi.FindControl("ProductDtNameEdit")
            End If
            ds = SQLExecuteQuery("Select Product_Code, Product_Name AS ProductDtName From VMsProduct WHERE ProductDt = " + QuotedStr(ProductDt.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                ProductDt.Text = ""
                ProductDtName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                ProductDt.Text = dr("Product_Code").ToString
                ProductDtName.Text = dr("Product_Name").ToString
            End If
        Catch ex As Exception
            lstatus.Text = "ProductDtAdd_TextChanged Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub ProductPartEdit_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dr As DataRow
        Dim ds As DataSet
        Dim ProductPart As TextBox
        Dim ProductPartName As Label
        Dim Count As Integer
        Dim dgi As GridViewRow
        Try
            Count = GridPackages.EditIndex
            dgi = GridPackages.Rows(Count)
            ProductPart = dgi.FindControl("ProductPartEdit")
            ProductPartName = dgi.FindControl("ProductPartNameEdit")


            ds = SQLExecuteQuery("Select Product_Code, Product_Name From VMsProduct WHERE ProductPart = " + QuotedStr(ProductPart.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                ProductPart.Text = ""
                ProductPartName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                ProductPart.Text = dr("Product_Code").ToString
                ProductPartName.Text = dr("Product_Name").ToString
            End If
        Catch ex As Exception
            lstatus.Text = "tb ProductPart Changed Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub ProductPartAdd_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dr As DataRow
        Dim ds As DataSet
        Dim ProductPart, tb As TextBox
        Dim ProductPartName As Label
        Dim Count As Integer
        Dim dgi As GridViewRow
        Try
            tb = sender
            If tb.ID = "ProductPartAdd" Then
                Count = GridPackages.Controls(0).Controls.Count
                dgi = GridPackages.Controls(0).Controls(Count - 2) '-1 for allowpaging = False
                ProductPart = dgi.FindControl("ProductPartAdd")
                ProductPartName = dgi.FindControl("ProductPartNameAdd")
            Else
                Count = GridPackages.EditIndex
                dgi = GridPackages.Rows(Count)
                ProductPart = dgi.FindControl("ProductPartEdit")
                ProductPartName = dgi.FindControl("ProductPartNameEdit")
            End If
            ds = SQLExecuteQuery("Select Product_Code, Product_Name AS ProductPartName From VMsProduct WHERE ProductPart = " + QuotedStr(ProductPart.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                ProductPart.Text = ""
                ProductPartName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                ProductPart.Text = dr("Product_Code").ToString
                ProductPartName.Text = dr("Product_Name").ToString
            End If
        Catch ex As Exception
            lstatus.Text = "ProductPartAdd_TextChanged Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub GridPart_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles GridPart.RowCancelingEdit
        Try
            GridPart.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            GridPart.EditIndex = -1
            bindDataGridPart()
        Catch ex As Exception
            lstatus.Text = "GridPart_RowCancelingEdit Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub GridPart_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridPart.RowCommand
        Dim tbPartNo As TextBox
        Dim tbPartName As TextBox
        Dim SQLString As String
        Dim GVR As GridViewRow
        Try
            If e.CommandName = "Insert" Then
                GVR = GridPart.FooterRow
                tbPartNo = GVR.FindControl("PartNoAdd")
                tbPartName = GVR.FindControl("PartNameAdd")

                If CFloat(tbPartNo.Text.Trim) = 0 Then
                    lstatus.Text = "<script language='javascript'>alert('Part must be filled');</script>"
                    tbPartNo.Focus()
                    Exit Sub
                End If
                If tbPartName.Text = "" Then
                    lstatus.Text = "<script language='javascript'>alert('Part Name must be filled');</script>"
                    tbPartName.Focus()
                    Exit Sub
                End If

                If SQLExecuteScalar("SELECT ProductCode From MsProductPart WHERE ProductCode = " + QuotedStr(ViewState("Nmbr")) + " AND PartNo = " + QuotedStr(tbPartNo.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Product " + QuotedStr(ViewState("Nmbr")) + " Part No " + QuotedStr(tbPartName.Text) + " has already been exist"
                    Exit Sub
                End If
                SQLString = "Insert Into MsProductPart (ProductCode, PartName, PartNo) " + _
                "SELECT " + QuotedStr(ViewState("Nmbr")) + "," + QuotedStr(tbPartName.Text) + "," + _
                tbPartNo.Text.Replace(",", "")

                SQLString = Replace(SQLString, "''", "NULL")
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

                bindDataGridPart()

            End If
        Catch ex As Exception
            lstatus.Text = "GridPart_RowCommand Error" + ex.ToString
        End Try
    End Sub

    Protected Sub GridPart_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridPart.RowDeleting
        Dim txtID As Label
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            txtID = GridPart.Rows(e.RowIndex).FindControl("PartNo")
            SQLExecuteNonQuery("Delete from MsProductPart where ProductCode = " + QuotedStr(ViewState("Nmbr")) + " AND PartNo =" + QuotedStr(txtID.Text), ViewState("DBConnection").ToString)
            bindDataGridPart()

        Catch ex As Exception
            lstatus.Text = "GridPart_RowDeleting Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub GridPart_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridPart.RowEditing
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            GridPart.EditIndex = e.NewEditIndex
            GridPart.ShowFooter = False
            bindDataGridPart()
        Catch ex As Exception
            lstatus.Text = "GridPart_RowEditing exception : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridPart_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles GridPart.RowUpdating
        Dim tbPartName As TextBox
        Dim lbPartNo As Label
        Dim SQLString As String
        Dim GVR As GridViewRow
        Try
            GVR = GridPart.Rows(e.RowIndex)
            lbPartNo = GVR.FindControl("PartNoEdit")
            tbPartName = GVR.FindControl("PartNameEdit")

            SQLString = "UPDATE MsProductPart SET PartName =" + tbPartName.Text.Replace(",", "") + _
            " WHERE ProductCode =" + QuotedStr(ViewState("Nmbr")) + " AND PartNo =" + QuotedStr(lbPartNo.Text)

            SQLString = Replace(SQLString, "''", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            GridPart.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            GridPart.EditIndex = -1
            bindDataGridPart()

        Catch ex As Exception
            lstatus.Text = lstatus.Text + "Datagrid dt update Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub GridWrhsArea_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles GridWrhsArea.RowCancelingEdit
        Try
            GridWrhsArea.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            GridWrhsArea.EditIndex = -1
            bindDataGridWrhsArea()
        Catch ex As Exception
            lstatus.Text = "GridWrhsArea_RowCancelingEdit Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub GridWrhsArea_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridWrhsArea.RowCommand
        Dim tbQtyMin, tbQtyMax As TextBox
        Dim ddlWrhsArea, ddltransferType As DropDownList
        Dim SQLString As String
        Dim GVR As GridViewRow
        Try
            If e.CommandName = "Insert" Then
                GVR = GridWrhsArea.FooterRow
                tbQtyMin = GVR.FindControl("QtyMinAdd")
                tbQtyMax = GVR.FindControl("QtyMaxAdd")
                ddlWrhsArea = GVR.FindControl("WrhsAreaAdd")
                ddltransferType = GVR.FindControl("transfertypeAdd")

                If CFloat(tbQtyMin.Text.Trim) = 0 Then
                    lstatus.Text = "<script language='javascript'>alert('QtyMin must be filled');</script>"
                    tbQtyMin.Focus()
                    Exit Sub
                End If
                If CFloat(tbQtyMax.Text.Trim) = 0 Then
                    lstatus.Text = "<script language='javascript'>alert('QtyMax must be filled');</script>"
                    tbQtyMax.Focus()
                    Exit Sub
                End If
                If ddlWrhsArea.SelectedValue = "" Then
                    lstatus.Text = "<script language='javascript'>alert('WrhsArea must be filled');</script>"
                    ddlWrhsArea.Focus()
                    Exit Sub
                End If

                If SQLExecuteScalar("SELECT ProductCode From MsProductWrhsArea WHERE ProductCode = " + QuotedStr(ViewState("Nmbr")) + " AND WrhsArea = " + QuotedStr(ddlWrhsArea.SelectedValue), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Product " + QuotedStr(ViewState("Nmbr")) + " Unit WrhsArea " + QuotedStr(ddlWrhsArea.SelectedValue) + " has already been exist"
                    Exit Sub
                End If
                SQLString = "Insert Into MsProductWrhsArea (ProductCode, WrhsArea, QtyMin, QtyMax, TransferType) " + _
                "SELECT " + QuotedStr(ViewState("Nmbr")) + "," + QuotedStr(ddlWrhsArea.SelectedValue) + "," + QuotedStr(tbQtyMin.text.Replace(",", "")) + "," + _
                tbQtyMax.Text.Replace(",", "") + ", " + QuotedStr(ddltransferType.SelectedValue)

                SQLString = Replace(SQLString, "''", "NULL")
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

                bindDataGridWrhsArea()

            End If
        Catch ex As Exception
            lstatus.Text = "GridWrhsArea_RowCommand Error" + ex.ToString
        End Try
    End Sub

    Protected Sub GridWrhsArea_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridWrhsArea.RowDeleting
        Dim txtID As Label
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            txtID = GridWrhsArea.Rows(e.RowIndex).FindControl("WrhsArea")
            SQLExecuteNonQuery("Delete from MsProductWrhsArea where ProductCode = " + QuotedStr(ViewState("Nmbr")) + " AND WrhsArea =" + QuotedStr(txtID.Text), ViewState("DBConnection").ToString)
            bindDataGridWrhsArea()

        Catch ex As Exception
            lstatus.Text = "GridWrhsArea_RowDeleting Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub GridWrhsArea_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridWrhsArea.RowEditing
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            GridWrhsArea.EditIndex = e.NewEditIndex
            GridWrhsArea.ShowFooter = False
            bindDataGridWrhsArea()
        Catch ex As Exception
            lstatus.Text = "GridWrhsArea_RowEditing exception : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridWrhsArea_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles GridWrhsArea.RowUpdating
        Dim tbQtyMin, tbQtyMax As TextBox
        Dim ddltransferType As DropDownList
        Dim lbWrhsArea As Label
        Dim SQLString As String
        Dim GVR As GridViewRow
        Try
            GVR = GridWrhsArea.Rows(e.RowIndex)
            tbQtyMin = GVR.FindControl("QtyMinEdit")
            tbQtyMax = GVR.FindControl("QtyMaxEdit")
            lbWrhsArea = GVR.FindControl("WrhsAreaEdit")
            ddltransferType = GVR.FindControl("transferTypeEdit")

            SQLString = "UPDATE MsProductWrhsArea SET QtyMin =" + tbQtyMin.Text.Replace(",", "") + _
                ", QtyMax = " + tbQtyMax.Text.Replace(",", "") + " , TransferType =" + QuotedStr(ddltransferType.SelectedValue) + _
            " WHERE ProductCode =" + QuotedStr(ViewState("Nmbr")) + " AND WrhsArea =" + QuotedStr(lbWrhsArea.Text)

            SQLString = Replace(SQLString, "''", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            GridWrhsArea.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            GridWrhsArea.EditIndex = -1
            bindDataGridWrhsArea()

        Catch ex As Exception
            lstatus.Text = lstatus.Text + "Datagrid dt update Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnPSubGroup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPSubGroup.Click
        Dim FieldResult As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "Select * FROM VMsProductGroupSub"
            FieldResult = "Product_SubGroup_Code, Product_SubGroup_Name, Product_Group_Code, Product_Group_Name"
            ViewState("Sender") = "btnPSubGroup"
            Session("Column") = FieldResult.Split(",")
            AttachScript("OpenSearchGrid();", Page, Me.GetType())
        Catch ex As Exception
            lstatus.Text = "btn btnPSubGroup Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbPSubGroup_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPSubGroup.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("Select * From VMsProductGroupSub WHERE Product_SubGroup_Code = " + QuotedStr(tbPSubGroup.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbPSubGroup.Text = ""
                tbPSubGroupName.Text = ""
                tbProductGroup.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                BindToText(tbPSubGroup, dr("Product_SubGroup_Code").ToString)
                BindToText(tbPSubGroupName, dr("Product_SubGroup_Name").ToString)
                BindToText(tbProductGroup, dr("Product_Group_Name").ToString)
            End If
            Code()
        Catch ex As Exception
            lstatus.Text = "tb PSubGroup Changed Error : " + ex.ToString
        End Try
    End Sub


End Class
