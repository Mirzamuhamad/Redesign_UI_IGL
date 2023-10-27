Imports System.Data

Partial Class Master_MsProductMateri_MsProductMateri
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            InitProperty()
            FillCombo(ddlWrhsType, "EXEC S_GetWrhsType", False, "WrhsType", "WrhsType", ViewState("DBConnection"))
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            DataGridDt.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGridDt.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            Dim dt As DataTable
            dt = SQLExecuteQuery("Select RptTitle, MenuName, COALESCE(MenuParam,'') AS MenuParam, COALESCE(FastRpt,'') AS FastRpt, COALESCE(ReportType,'') AS ReportType, COALESCE(ReportType2,'') AS ReportType2, COALESCE(FgPrintValue,'N') AS FgPrintValue, COALESCE(FgForceNewPage,'N') AS FgForceNewPage from MsMenu WHERE MenuId = " + QuotedStr(Request.QueryString("ContainerId").ToString), ViewState("DBConnection").ToString).Tables(0)
            lblTitle.Text = dt.Rows(0)("MenuName").ToString


        End If
        dsProductMateri.ConnectionString = ViewState("DBConnection")
        dsProductCategory.ConnectionString = ViewState("DBConnection")


        If Not Session("Result") Is Nothing Then
            If ViewState("Sender") = "btnAccInvent" Then
                tbAccInvent.Text = Session("Result")(0).ToString
                tbAccInventName.Text = Session("Result")(1).ToString
                tbAccInvent.Focus()
            End If
            If ViewState("Sender") = "btnAccCOGS" Then
                tbAccCOGS.Text = Session("Result")(0).ToString
                tbAccCOGSName.Text = Session("Result")(1).ToString
                tbAccCOGS.Focus()
            End If
            If ViewState("Sender") = "btnAccTransitSJ" Then
                tbAccTransitSJ.Text = Session("Result")(0).ToString
                tbAccTransitSJName.Text = Session("Result")(1).ToString
                tbAccTransitSJ.Focus()
            End If
            If ViewState("Sender") = "btnAccSales" Then
                tbAccSales.Text = Session("Result")(0).ToString
                tbAccSalesName.Text = Session("Result")(1).ToString
                tbAccSales.Focus()
            End If
            If ViewState("Sender") = "btnAccTransitWrhs" Then
                tbAccTransitWrhs.Text = Session("Result")(0).ToString
                tbAccTransitWrhsName.Text = Session("Result")(1).ToString
                tbAccTransitWrhs.Focus()
            End If
            If ViewState("Sender") = "btnAccSRetur" Then
                tbAccSRetur.Text = Session("Result")(0).ToString
                tbAccSReturName.Text = Session("Result")(1).ToString
                tbAccTransitRetur.Focus()
            End If
            If ViewState("Sender") = "btnAccDisc" Then
                tbAccTransitReject.Text = Session("Result")(0).ToString
                tbAccTransitRejectname.Text = Session("Result")(1).ToString
                btnAccTransitReject.Focus()
            End If

            If ViewState("Sender") = "btnAccTransitRetur" Then
                tbAccTransitRetur.Text = Session("Result")(0).ToString
                tbAccTransitReturName.Text = Session("Result")(1).ToString
                tbAccTransitRetur.Focus()
            End If

            If ViewState("Sender") = "AccSRetur" Then
                tbAccSRetur.Text = Session("Result")(0).ToString
                tbAccSReturName.Text = Session("Result")(1).ToString
                tbAccSRetur.Focus()
            End If
            
            If ViewState("Sender") = "btnAccExpLoss" Then
                tbAccExpLoss.Text = Session("Result")(0).ToString
                tbAccExpLossName.Text = Session("Result")(1).ToString
                tbAccExpLoss.Focus()
            End If
            'If ViewState("Sender") = "btnAccSReturn" Then
            '    tbAccSReturn.Text = Session("Result")(0).ToString
            '    tbAccSReturnName.Text = Session("Result")(1).ToString
            '    tbAccSReturn.Focus()
            'End If
            'If ViewState("Sender") = "btnAccSTCAdjust" Then
            '    tbAccSTCAdjust.Text = Session("Result")(0).ToString
            '    tbAccSTCAdjustName.Text = Session("Result")(1).ToString
            '    tbAccSTCAdjust.Focus()
            'End If
            'If ViewState("Sender") = "btnAccSTCLost" Then
            '    tbAccSTCLost.Text = Session("Result")(0).ToString
            '    tbAccSTCLostName.Text = Session("Result")(1).ToString
            '    tbAccSTCLost.Focus()
            'End If
            'If ViewState("Sender") = "btnAccSampleExps" Then
            '    tbAccSampleExps.Text = Session("Result")(0).ToString
            '    tbAccSampleExpsName.Text = Session("Result")(1).ToString
            '    tbAccSampleExps.Focus()
            'End If
            'If ViewState("Sender") = "btnAccWIPLabor" Then
            '    tbAccWIPLabor.Text = Session("Result")(0).ToString
            '    tbAccWIPLaborName.Text = Session("Result")(1).ToString
            '    tbAccWIPLabor.Focus()
            'End If
            'If ViewState("Sender") = "btnAccWIPLabor2" Then
            '    tbAccWIPLabor2.Text = Session("Result")(0).ToString
            '    tbAccWIPLabor2Name.Text = Session("Result")(1).ToString
            '    tbAccWIPLabor2.Focus()
            'End If
            'If ViewState("Sender") = "btnAccWIPFOH" Then
            '    tbAccWIPFOH.Text = Session("Result")(0).ToString
            '    tbAccWIPFOHName.Text = Session("Result")(1).ToString
            '    tbAccWIPFOH.Focus()
            'End If

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
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            SqlString = "Select * from MsProductMateri " + StrFilter
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "MateriCode ASC"
            End If
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection"))
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    'Private Sub bindDataGrid()
    '    Dim StrFilter, SqlString As String
    '    Try
    '        StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
    '        SqlString = "SELECT A.MateriCode, A.MateriName, A.GroupType, A.FgStock, B.ProductCategory As GroupName FROM MsProductMateri A INNER JOIN MsProductType B ON A.GroupType = B.ProductTypeCode" + StrFilter + " Order By A.MateriCode "
    '        If ViewState("SortExpression") = Nothing Then
    '            ViewState("SortExpression") = "MateriCode ASC"
    '            ViewState("SortOrder") = "ASC"
    '        End If
    '        BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection"))
    '    Catch ex As Exception
    '        lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
    '    Finally
    '    End Try
    'End Sub

    Private Sub bindDataGridDt()
        Dim tempDS As New DataSet()
        Dim DV As DataView
        Try
            tempDS = SQLExecuteQuery("SELECT A.*, C.AccountName AS AccInventName, " + _
            "E.AccountName AS acccogsName, " + _
            "F.AccountName AS acctransitsjName, G.AccountName AS acctransitwrhsName, " + _
            "H.AccountName AS AccTransitReturName, I.AccountName AS AccTransitRejectName, " + _
            "L.AccountName AS AccSReturName, K.AccountName As AccSalesName, " + _
            "R.AccountName AS AccExpLossName  FROM MsProductMateriDt A LEFT OUTER JOIN " + _
            "MsAccount C ON A.AccInvent = C.Account LEFT OUTER JOIN " + _
            "MsAccount E ON A.acccogs = E.Account LEFT OUTER JOIN " + _
            "MsAccount F ON A.acctransitsj = F.Account LEFT OUTER JOIN " + _
            "MsAccount G ON A.acctransitwrhs = G.Account  LEFT OUTER JOIN " + _
            "MsAccount H ON A.AccTransitRetur = H.Account LEFT OUTER JOIN " + _
            "MsAccount I ON A.AccTransitReject = I.Account LEFT OUTER JOIN " + _
            "MsAccount K ON A.AccSales = K.Account LEFT OUTER JOIN " + _
            "MsAccount L ON A.AccSRetur = L.Account LEFT OUTER JOIN " + _
            "MsAccount R ON A.AccExpLoss = R.Account WHERE A.ProductMateri =" + QuotedStr(ViewState("Nmbr")), ViewState("DBConnection").ToString)

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
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Public Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        Dim txt As TextBox
        Dim ddlFgStock, dbGroupType As DropDownList

        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()
            obj = DataGrid.Rows(e.NewEditIndex)
            txt = obj.FindControl("MateriNameEdit")
            dbGroupType = obj.FindControl("GroupTypeEdit")
            ddlFgStock = obj.FindControl("FgStockEdit")

            'If dbGroupType.SelectedIndex = 4 Then
            '    ddlFgStock.SelectedIndex = 1
            '    ddlFgStock.Enabled = False
            'Else
            '    ddlFgStock.SelectedIndex = 0
            '    ddlFgStock.Enabled = True
            'End If

            txt.Focus()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
        'Dim tbName As TextBox
        ''Dim dbGroupType As Label
        'Try
        '    If CheckMenuLevel("Edit") = False Then
        '        Exit Sub
        '    End If
        '    DataGrid.EditIndex = e.NewEditIndex
        '    DataGrid.ShowFooter = False
        '    bindDataGrid()
        '    tbName = DataGrid.Rows(e.NewEditIndex).FindControl("MateriNameEdit")
        '    'dbGroupType = DataGrid.Rows(e.NewEditIndex).FindControl("Grouptype")
        '    'lstatus.Text = dbGroupType.Text
        '    'Exit Sub
        '    tbName.Focus()
        'Catch ex As Exception
        '    lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        'End Try
        'Dim obj As GridViewRow
        'Dim txt As TextBox
        'Try
        '    If CheckMenuLevel("Edit") = False Then
        '        Exit Sub
        '    End If
        '    DataGrid.EditIndex = e.NewEditIndex
        '    DataGrid.ShowFooter = False
        '    bindDataGrid()
        '    obj = DataGrid.Rows(e.NewEditIndex)
        '    txt = obj.FindControl("MateriNameEdit")
        '    txt.Focus()

        'Catch ex As Exception
        '    lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        'End Try
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
    Private Sub DataGrid_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles DataGrid.RowCancelingEdit
        Try
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid()
        Catch ex As Exception
            lstatus.Text = "DataGrid_Cancel Error : " + vbCrLf + ex.ToString
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

    Protected Sub DataGrid_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGrid.RowCommand
        Dim SQLString As String
        Dim dbName, dbCode As TextBox
        Dim dbGroupType, ddlFgStock As DropDownList
        Dim GVR As GridViewRow
        Try
            If e.CommandName = "Insert" Then
                GVR = DataGrid.FooterRow
                dbCode = GVR.FindControl("MateriCodeAdd")
                dbName = GVR.FindControl("MateriNameAdd")
                dbGroupType = GVR.FindControl("GroupTypeAdd")
                ddlFgStock = GVR.FindControl("FgStockAdd")

                If dbCode.Text.Trim = "" Then
                    lstatus.Text = "<script language='javascript'>alert('Product Materi Code must be filled');</script>"
                    dbCode.Focus()
                    Exit Sub
                End If

                If dbGroupType.SelectedValue.Trim = "" Then
                    lstatus.Text = "<script language='javascript'>alert('Product Type Group must be filled');</script>"
                    dbGroupType.Focus()
                    Exit Sub
                End If

                If dbName.Text.Trim.Length = 0 Then
                    lstatus.Text = "<script language='javascript'>alert('Product Materi Name must be filled');</script>"
                    dbName.Focus()
                    Exit Sub
                End If

                If SQLExecuteScalar("SELECT Product_Materi_Code From VMsProductMateri WHERE Product_Materi_Code = " + QuotedStr(dbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Product Materi " + QuotedStr(dbCode.Text) + " has already been exist"
                    Exit Sub
                End If

                'insert the new entry
                SQLString = "Insert into MsProductMateri (MateriCode, MateriName, Grouptype, FgStock, UserID, UserDate) " + _
                "SELECT " + QuotedStr(dbCode.Text) + "," + QuotedStr(dbName.Text) + ", " + QuotedStr(dbGroupType.SelectedValue) + "," + QuotedStr(ddlFgStock.SelectedValue) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", GetDate()"
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                bindDataGrid()
            ElseIf e.CommandName = "View" Then
                Dim lbCode, lbName As Label
                GVR = DataGrid.Rows(CInt(e.CommandArgument))
                lbCode = GVR.FindControl("MateriCode")
                lbName = GVR.FindControl("MateriName")
                ViewState("Nmbr") = lbCode.Text
                lbProductMateriCode.Text = lbCode.Text
                '+ " - " + lbName.Text
                pnlHd.Visible = False
                pnlDt.Visible = True
                PanelInfo.Visible = True
                bindDataGridDt()
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub
    Public Sub DataGridDt_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGridDt.RowCommand
        Dim DDL As DropDownList
        Dim GVR As GridViewRow = Nothing
        Dim index As Integer
        Try
            If Not (e.CommandName = "Sort" Or e.CommandName = "Page") Then
                index = Convert.ToInt32(e.CommandArgument)
                GVR = DataGridDt.Rows(index)
            End If
            If e.CommandName = "Go" Then
                DDL = DataGridDt.Rows(index).FindControl("ddl")
                If DDL.SelectedValue = "Edit" Then
                    ViewState("State") = "Edit"
                    FillTextBoxDt(ViewState("Nmbr"), GVR.Cells(1).Text)
                    ModifyInputDt(True)
                    ddlWrhsType.Enabled = False
                    btnSave.Visible = True
                    btnReset.Visible = True
                    pnlDt.Visible = False
                    PanelInfo.Visible = True
                    pnlInputDt.Visible = True
                End If
                If DDL.SelectedValue = "Delete" Then
                    Try
                        If CheckMenuLevel("Delete") = False Then
                            Exit Sub
                        End If

                        SQLExecuteNonQuery("Delete from MsProductMateriDt where ProductMateri = " + QuotedStr(ViewState("Nmbr")) + " AND WrhsType =" + QuotedStr(GVR.Cells(1).Text), ViewState("DBConnection").ToString)
                        bindDataGridDt()

                    Catch ex As Exception
                        lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
                    End Try
                End If

            End If


        Catch ex As Exception
            lstatus.Text = "Item Command Dt Error" + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim dbName As TextBox
        Dim lbCode As Label
        Dim ddlFgStock, dbGroupType As DropDownList
        Dim GVR As GridViewRow
        Try
            GVR = DataGrid.Rows(e.RowIndex)
            lbCode = GVR.FindControl("MateriCodeEdit")
            dbName = GVR.FindControl("MateriNameEdit")
            dbGroupType = GVR.FindControl("GroupTypeEdit")
            ddlFgStock = GVR.FindControl("FgStockEdit")

            If dbName.Text.Trim.Length = 0 Then
                lstatus.Text = "<script language='javascript'>alert('Product Materi Name must be filled.');</script>"
                dbName.Focus()
                Exit Sub
            End If

            SQLString = "Update MsProductMateri set MateriName = " + QuotedStr(dbName.Text) + _
            ", Grouptype =" + QuotedStr(dbGroupType.SelectedValue) + ", FgStock =" + QuotedStr(ddlFgStock.SelectedValue) + _
            "  where MateriCode = '" & lbCode.Text + "'"

            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = lstatus.Text + "DataGrid_Update Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub DataGridDt_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGridDt.RowUpdating
        Dim tbAccInvent, tbAccCOGS, tbAccTransitSJ, tbAccSales, tbAccTransitWrhs, tbAccTransitReject, tbAccTransitRetur, tbAccExpLoss As TextBox
        Dim lbWrhsType As Label
        Dim SQLString As String
        Dim GVR As GridViewRow
        Try
            GVR = DataGridDt.Rows(e.RowIndex)
            lbWrhsType = GVR.FindControl("WrhsTypeEdit")
            tbAccInvent = GVR.FindControl("AccInventEdit")
            tbAccCOGS = GVR.FindControl("AccCogsEdit")
            tbAccSales = GVR.FindControl("AccSalesEdit")
            tbAccTransitWrhs = GVR.FindControl("AccTransitWrhsEdit")
            tbAccTransitRetur = GVR.FindControl("AccTransitReturEdit")
            tbAccTransitSJ = GVR.FindControl("AccTransitSJEdit")
            tbAccCOGS = GVR.FindControl("AccCOGSEdit")
            tbAccExpLoss = GVR.FindControl("AccExpLossEdit")
            tbAccSRetur = GVR.FindControl("AccSReturEdit")
            tbAccTransitReject = GVR.FindControl("AccTransitRejectEdit")

            SQLString = "UPDATE MsProductMateriDt SET ACCInvent = " + QuotedStr(tbAccInvent.Text) + _
            ", AccCogs= " + QuotedStr(tbAccCOGS.Text) + _
            ", AccTransitSj= " + QuotedStr(tbAccTransitSJ.Text) + _
            ", AccTransitWrhs= " + QuotedStr(tbAccTransitWrhs.Text) + _
            ", AccSales= " + QuotedStr(tbAccSales.Text) + _
            ", AccTransitRetur = " + QuotedStr(tbAccTransitRetur.Text) + _
            ", AccCOGS = " + QuotedStr(tbAccCOGS.Text) + _
            ", AccExpLoss = " + QuotedStr(tbAccExpLoss.Text) + _
            ", AccSRetur = " + QuotedStr(tbAccSRetur.Text) + _
            ", AccTransitReject = " + QuotedStr(tbAccTransitReject.Text) + _
            " WHERE WrhsType = " + QuotedStr(lbWrhsType.Text) + _
            " AND ProductType =" + QuotedStr(ViewState("Nmbr"))

            SQLString = Replace(SQLString, "''", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            DataGridDt.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGridDt.EditIndex = -1
            bindDataGridDt()
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "Datagrid dt update Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGrid.RowDeleting
        Dim txtID As Label
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            txtID = DataGrid.Rows(e.RowIndex).FindControl("MateriCode")

            SQLExecuteNonQuery("Delete from MsProductMateriDt where ProductMateri = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
            SQLExecuteNonQuery("Delete from MsProductMateri where MateriCode = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub DataGridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGridDt.RowDeleting
        'Dim txtID As Label
        'Try
        '    If CheckMenuLevel("Delete") = False Then
        '        Exit Sub
        '    End If
        '    txtID = DataGridDt.Rows(e.RowIndex).FindControl("WrhsType")

        '    SQLExecuteNonQuery("Delete from MsProductMateriDt where ProductType = " + QuotedStr(ViewState("Nmbr")) + " AND WrhsType =" + QuotedStr(txtID.Text))
        '    bindDataGridDt()

        'Catch ex As Exception
        '    lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        'End Try
    End Sub

    Protected Sub DataGrid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGrid.PageIndexChanging
        DataGrid.PageIndex = e.NewPageIndex
        If DataGrid.EditIndex <> -1 Then
            DataGrid_RowCancelingEdit(Nothing, Nothing)
        End If
        bindDataGrid()
    End Sub
    Protected Sub DataGridDt_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGridDt.PageIndexChanging
        DataGridDt.PageIndex = e.NewPageIndex
        If DataGridDt.EditIndex <> -1 Then
            DataGridDt_RowCancelingEdit(Nothing, Nothing)
        End If
        bindDataGridDt()
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
    Protected Sub DataGridDt_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles DataGridDt.Sorting
        Try
            ViewState("SortExpressionDt") = e.SortExpression
            bindDataGridDt()
        Catch ex As Exception
            lstatus.Text = "UserDG_SortCommandError =" + vbCrLf + ex.ToString
        End Try
    End Sub



    Protected Sub btnBackDtTop_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBackDtTop.Click, btnBack2.Click
        Try
            pnlHd.Visible = True
            pnlDt.Visible = False
            PanelInfo.Visible = False
        Catch ex As Exception
            lstatus.Text = "btn Back Dt Top Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub WrhsTypeAdd_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Count As Integer
        Dim dgi As GridViewRow
        Dim tbAccInvent, tbAccSales, tbAccCogs, tbAccsj, tbAccWrhs, tbAccTransitReject, tbAccTransitRetur, tbAccSRetur, tbAccExpLoss As TextBox
        Dim lbAccInventName, lbAccSalesName, lbAccCogsName, lbAccsjName, lbAccWrhsName, lbtbAccTransitRejectName, lbAccTransitReturName, lbAccSReturName, lbAccExpLossName As Label
        Try
            Count = DataGridDt.Controls(0).Controls.Count
            'dgi = DataGridDt.Controls(0).Controls(Count - 2) '-1 for allowpaging = False
            dgi = DataGridDt.FooterRow

            tbAccInvent = dgi.FindControl("AccInventAdd")
            lbAccInventName = dgi.FindControl("AccInventNameAdd")

            tbAccSales = dgi.FindControl("AccSalesAdd")
            lbAccSalesName = dgi.FindControl("AccSalesNameAdd")

            tbAccCogs = dgi.FindControl("AccCogsAdd")
            lbAccCogsName = dgi.FindControl("AccCogsNameAdd")

            tbAccsj = dgi.FindControl("AccsjAdd")
            lbAccsjName = dgi.FindControl("AccsjNameAdd")

            tbAccWrhs = dgi.FindControl("AccWrhsAdd")
            lbAccWrhsName = dgi.FindControl("AccWrhsNameAdd")

            tbAccTransitRetur = dgi.FindControl("AccTransitReturAdd")
            lbAccTransitReturName = dgi.FindControl("AccTransitReturNameAdd")

            tbAccTransitReject = dgi.FindControl("bAccTransitRejectAdd")
            lbtbAccTransitRejectName = dgi.FindControl("bAccTransitRejectNameAdd")

            tbAccSRetur = dgi.FindControl("AccSReturAdd")
            lbAccSReturName = dgi.FindControl("AccSReturNameAdd")

            tbAccExpLoss = dgi.FindControl("AccExpLossAdd")
            lbAccExpLossName = dgi.FindControl("AccExpLossNameAdd")

            tbAccInvent.Text = ""
            lbAccInventName.Text = ""
            tbAccSales.Text = ""
            lbAccSalesName.Text = ""
            tbAccCogs.Text = ""
            lbAccCogsName.Text = ""
            tbAccsj.Text = ""
            lbAccsjName.Text = ""
            tbAccWrhs.Text = ""
            lbAccWrhsName.Text = ""
            tbAccTransitRetur.Text = ""
            lbAccTransitReturName.Text = ""
            tbAccTransitReject.Text = ""
            lbtbAccTransitRejectName.Text = ""
            tbAccSRetur.Text = ""
            lbAccExpLossName.Text = ""
            tbAccExpLoss.Text = ""
            lbAccExpLossName.Text = ""


        Catch ex As Exception
            lstatus.Text = "Warehouse Type Add Index Changed Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub GroupTypeEdit_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dgi As GridViewRow
        Dim dbGroupType, ddlFgStock As DropDownList
        Try
            dgi = DataGrid.Rows(DataGrid.EditIndex)
            dbGroupType = dgi.FindControl("GroupTypeEdit")
            ddlFgStock = dgi.FindControl("FgStockEdit")

            If dbGroupType.SelectedValue = "Service" Or dbGroupType.SelectedValue = "Fixed Asset" Or dbGroupType.SelectedValue = "Expendable Asset" Then
                ddlFgStock.SelectedValue = "N"
                ddlFgStock.Enabled = False
            Else
                ddlFgStock.SelectedValue = "Y"
                ddlFgStock.Enabled = True
            End If

        Catch ex As Exception
            lstatus.Text = "Product Category Edit Index Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GrouptypeAdd_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dgi As GridViewRow
        Dim dbGroupType, ddlFgStock As DropDownList
        Try
            dgi = DataGrid.FooterRow
            dbGroupType = dgi.FindControl("GrouptypeAdd")
            ddlFgStock = dgi.FindControl("FgStockAdd")

            If dbGroupType.SelectedValue = "Service" Or dbGroupType.SelectedValue = "Fixed Asset" Or dbGroupType.SelectedValue = "Expendable Asset" Then
                ddlFgStock.SelectedValue = "N"
                ddlFgStock.Enabled = False
            Else
                ddlFgStock.SelectedValue = "Y"
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
            Session("SelectCommand") = "EXEC S_FormPrintMaster4 'VMsProductMateriNew','MateriCode','MateriName','GroupName','FgStock'," + QuotedStr(lblTitle.Text) + ",'Product Materi Code','Product Materi Name','Product Category','Stock'," + QuotedStr(StrFilter) + "," + QuotedStr(ViewState("UserId"))
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster3.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            lstatus.Text = "Btn Print Error : " + ex.ToString
        End Try

    End Sub
    Protected Sub FillTextBoxDt(ByVal Nmbr As String, ByVal Code As String)
        Try
            Dim dr As DataRow
            dr = SQLExecuteQuery("SELECT A.*, C.AccountName AS AccInventName, " + _
            "E.AccountName AS acccogsName, " + _
            "F.AccountName AS acctransitsjName, G.AccountName AS acctransitwrhsName, " + _
            "H.AccountName AS AccTransitReturName, I.AccountName AS AccTransitRejectName, " + _
            "L.AccountName AS AccSReturName, K.AccountName As AccSalesName, " + _
            "R.AccountName AS AccExpLossName  FROM MsProductMateriDt A LEFT OUTER JOIN " + _
            "MsAccount C ON A.AccInvent = C.Account LEFT OUTER JOIN " + _
            "MsAccount E ON A.acccogs = E.Account LEFT OUTER JOIN " + _
            "MsAccount F ON A.acctransitsj = F.Account LEFT OUTER JOIN " + _
            "MsAccount G ON A.acctransitwrhs = G.Account  LEFT OUTER JOIN " + _
            "MsAccount H ON A.AccTransitRetur = H.Account LEFT OUTER JOIN " + _
            "MsAccount I ON A.AccTransitReject = I.Account LEFT OUTER JOIN " + _
            "MsAccount K ON A.AccSales = K.Account LEFT OUTER JOIN " + _
            "MsAccount L ON A.AccSRetur = L.Account LEFT OUTER JOIN " + _
            "MsAccount R ON A.AccExpLoss = R.Account WHERE A.ProductMateri =" + QuotedStr(Nmbr) + " AND WrhsType = " + QuotedStr(Code), ViewState("DBConnection").ToString).Tables(0).Rows(0)

            ClearDt()

            BindToText(tbAccInvent, dr("AccInvent").ToString)
            BindToText(tbAccInventName, dr("AccInventName").ToString)

            BindToText(tbAccCOGS, dr("acccogs").ToString)
            BindToText(tbAccCOGSName, dr("acccogsName").ToString)

            BindToText(tbAccTransitSJ, dr("acctransitsj").ToString)
            BindToText(tbAccTransitSJName, dr("acctransitsjName").ToString)

            BindToText(tbAccTransitWrhs, dr("acctransitwrhs").ToString)
            BindToText(tbAccTransitWrhsName, dr("acctransitwrhsName").ToString)

            BindToText(tbAccSales, dr("AccSales").ToString)
            BindToText(tbAccSalesName, dr("AccSalesName").ToString)

            BindToText(tbAccTransitRetur, dr("AccTransitRetur").ToString)
            BindToText(tbAccTransitReturName, dr("AccTransitReturName").ToString)

            BindToText(tbAccTransitReject, dr("AccTransitReject").ToString)
            BindToText(tbAccTransitRejectName, dr("AccTransitRejectName").ToString)

            BindToText(tbAccSRetur, dr("AccSRetur").ToString)
            BindToText(tbAccSReturName, dr("AccSReturName").ToString)

            BindToText(tbAccExpLoss, dr("AccExpLoss").ToString)
            BindToText(tbAccExpLossName, dr("AccExpLossName").ToString)

            BindToDropList(ddlWrhsType, dr("WrhsType").ToString)

            ddlWrhsType.Focus()

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt.Click, btnAddDt2.Click
        ViewState("State") = "Insert"
        ClearDt()
        ModifyInputDt(True)
        pnlDt.Visible = False
        PanelInfo.Visible = True
        pnlInputDt.Visible = True
        btnSave.Visible = True
        btnReset.Visible = True
        ddlWrhsType.Focus()
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ClearDt()
        pnlDt.Visible = True
        PanelInfo.Visible = True
        pnlInputDt.Visible = False
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        ClearDt()
    End Sub
    Private Sub ClearDt()
        Try
            ddlWrhsType.SelectedIndex = 0

            tbAccInvent.Text = ""
            tbAccInventName.Text = ""

            tbAccCOGS.Text = ""
            tbAccCOGSName.Text = ""

            tbAccTransitSJ.Text = ""
            tbAccTransitSJName.Text = ""

            tbAccSales.Text = ""
            tbAccSalesName.Text = ""

            tbAccTransitWrhs.Text = ""
            tbAccTransitWrhsName.Text = ""

            tbAccTransitRetur.Text = ""
            tbAccTransitReturName.Text = ""

            tbAccTransitReject.Text = ""
            tbAccTransitRejectName.Text = ""

            tbAccSRetur.Text = ""
            tbAccSReturName.Text = ""

            tbAccExpLoss.Text = ""
            tbAccExpLossName.Text = ""

        Catch ex As Exception
            lstatus.Text = "ClearDt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccInvent_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccInvent.Click
        Dim FieldResult As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            If ddlWrhsType.SelectedValue = "Reject" Then
                Session("filter") = "SELECT * From VMsAccount Where FgType ='PL' And FgSubled IN ('N','P') AND FgNormal = 'D' AND FgActive='Y' AND Currency In (" + QuotedStr(ViewState("Currency")) + ") "
            Else
                Session("filter") = "SELECT * From VMsAccount Where FgSubled IN ('N','P') AND FgNormal = 'D' AND FgActive='Y' AND Currency In (" + QuotedStr(ViewState("Currency")) + ") "
            End If

            FieldResult = "Account, Description"
            Session("Column") = FieldResult.Split(",")
            AttachScript("OpenSearchGrid();", Page, Me.GetType())
            ViewState("Sender") = "btnAccInvent"
        Catch ex As Exception
            lstatus.Text = "btnAccInvent Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccCOGS_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccCOGS.Click
        Dim FieldResult As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT * FROM VMsAccount Where FgSubled IN ('N','P') and FgType = 'PL' and FgNormal = 'D' AND FgActive='Y' AND Currency In (" + QuotedStr(ViewState("Currency")) + ") "
            FieldResult = "Account, Description"
            Session("Column") = FieldResult.Split(",")
            AttachScript("OpenSearchGrid();", Page, Me.GetType())
            ViewState("Sender") = "btnAccCOGS"
        Catch ex As Exception
            lstatus.Text = "btnAccCOGS Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccTransitSJ_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccTransitSJ.Click
        Dim FieldResult As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT * FROM VMsAccount Where FgSubled IN ('N','P') and FgType = 'BS' and FgNormal = 'D' AND FgActive='Y' AND Currency In (" + QuotedStr(ViewState("Currency")) + ") "
            FieldResult = "Account, Description"
            Session("Column") = FieldResult.Split(",")
            AttachScript("OpenSearchGrid();", Page, Me.GetType())
            ViewState("Sender") = "btnAccTransitSJ"
        Catch ex As Exception
            lstatus.Text = "btnAccTransitSJ Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccSales_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccSales.Click
        Dim FieldResult As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT * FROM VMsAccount Where FgSubled IN ('N','P') and FgType = 'PL' and FgNormal = 'C' AND FgActive='Y' AND Currency In (" + QuotedStr(ViewState("Currency")) + ") "
            FieldResult = "Account, Description"
            Session("Column") = FieldResult.Split(",")
            AttachScript("OpenSearchGrid();", Page, Me.GetType())
            ViewState("Sender") = "btnAccSales"
        Catch ex As Exception
            lstatus.Text = "btnAccSales Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccTransitReject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccTransitReject.Click
        Dim FieldResult As String
        Try
            Session("filter") = "SELECT Account, Description From VMsAccount Where Currency In (" + QuotedStr(ViewState("Currency")) + ") And FgType = 'PL' and FgNormal = 'C' AND FgSubled IN ('N', 'P')"
            FieldResult = "Account, Description"
            Session("DBConnection") = ViewState("DBConnection")
            Session("Column") = FieldResult.Split(",")
            ViewState("Sender") = "btnAccTransitReject"
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lstatus.Text = "btn Search Product Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccTransitWrhs_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccTransitWrhs.Click
        Dim FieldResult As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT * FROM VMsAccount Where Currency In (" + QuotedStr(ViewState("Currency")) + ") AND FgSubled IN ('N','P') And FgType = 'BS' and FgNormal = 'D' AND FgActive='Y'"
            FieldResult = "Account, Description"
            Session("Column") = FieldResult.Split(",")
            AttachScript("OpenSearchGrid();", Page, Me.GetType())
            ViewState("Sender") = "btnAccTransitWrhs"
        Catch ex As Exception
            lstatus.Text = "btnAccTransitWrhs Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccTransitRetur_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccTransitRetur.Click
        Dim FieldResult As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT * FROM VMsAccount Where Currency In (" + QuotedStr(ViewState("Currency")) + ") And FgSubled IN ('N','P') and FgType = 'BS' and FgNormal = 'D' AND FgActive='Y'"
            FieldResult = "Account, Description"
            Session("Column") = FieldResult.Split(",")
            AttachScript("OpenSearchGrid();", Page, Me.GetType())
            ViewState("Sender") = "btnAccTransitRetur"
        Catch ex As Exception
            lstatus.Text = "btnAccTransitPRetur Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccSRetur_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccSRetur.Click
        Dim FieldResult As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT * FROM VMsAccount Where Currency In (" + QuotedStr(ViewState("Currency")) + ") And FgSubled IN ('N','P') and FgType = 'BS' and FgNormal = 'D' AND FgActive='Y'"
            FieldResult = "Account, Description"
            Session("Column") = FieldResult.Split(",")
            AttachScript("OpenSearchGrid();", Page, Me.GetType())
            ViewState("Sender") = "btnAccSRetur"
        Catch ex As Exception
            lstatus.Text = "btnAccTransitSRetur Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccExpLoss_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccExpLoss.Click
        Dim FieldResult As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT * FROM VMsAccount Where Currency In (" + QuotedStr(ViewState("Currency")) + ") And FgSubled IN ('N','P') and FgType = 'PL' AND FgActive='Y'"
            FieldResult = "Account, Description"
            Session("Column") = FieldResult.Split(",")
            AttachScript("OpenSearchGrid();", Page, Me.GetType())
            ViewState("Sender") = "btnAccExpLoss"
        Catch ex As Exception
            lstatus.Text = "btnAccSReturn Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccInvent_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccInvent.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            If ddlWrhsType.SelectedValue = "Reject" Then
                ds = SQLExecuteQuery("SELECT * From VMsAccount Where FgType = 'PL' AND FgSubled IN ('N','P') AND FgNormal = 'D' AND FgActive='Y' AND Currency In (" + QuotedStr(ViewState("Currency")) + ") AND Account = " + QuotedStr(tbAccInvent.Text), ViewState("DBConnection").ToString)
            Else
                ds = SQLExecuteQuery("SELECT * From VMsAccount Where FgSubled IN ('N','P') AND FgNormal = 'D' AND FgActive='Y' AND Currency In (" + QuotedStr(ViewState("Currency")) + ") AND Account = " + QuotedStr(tbAccInvent.Text), ViewState("DBConnection").ToString)
            End If

            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccInvent.Text = ""
                tbAccInventName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                BindToText(tbAccInvent, dr("Account").ToString)
                BindToText(tbAccInventName, dr("Description").ToString)
            End If

        Catch ex As Exception
            lstatus.Text = "tbAccInvent Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccCOGS_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccCOGS.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("SELECT * FROM VMsAccount Where FgSubled IN ('N','P') and FgType = 'PL' and FgNormal = 'D' AND FgActive='Y' AND Currency In (" + QuotedStr(ViewState("Currency")) + ") AND Account = " + QuotedStr(tbAccCOGS.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccCOGS.Text = ""
                tbAccCOGSName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                BindToText(tbAccCOGS, dr("Account").ToString)
                BindToText(tbAccCOGSName, dr("Description").ToString)
            End If

        Catch ex As Exception
            lstatus.Text = "tbAccCOGS Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccTransitSJ_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccTransitSJ.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("SELECT * FROM VMsAccount Where FgSubled IN ('N','P') and FgType = 'BS' and FgNormal = 'D' AND FgActive='Y' AND Currency In (" + QuotedStr(ViewState("Currency")) + ") AND Account = " + QuotedStr(tbAccTransitSJ.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccTransitSJ.Text = ""
                tbAccTransitSJName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                BindToText(tbAccTransitSJ, dr("Account").ToString)
                BindToText(tbAccTransitSJName, dr("Description").ToString)
            End If

        Catch ex As Exception
            lstatus.Text = "tbAccTransitSJ Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccSales_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccSales.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("SELECT * FROM VMsAccount Where FgSubled IN ('N','P') and FgType = 'PL' and FgNormal = 'C' AND FgActive='Y' AND Currency In (" + QuotedStr(ViewState("Currency")) + ") AND Account = " + QuotedStr(tbAccSales.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccSales.Text = ""
                tbAccSalesName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                BindToText(tbAccSales, dr("Account").ToString)
                BindToText(tbAccSalesName, dr("Description").ToString)
            End If

        Catch ex As Exception
            lstatus.Text = "tbAccSales Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccTransitWrhs_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccTransitWrhs.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("SELECT * FROM VMsAccount Where Currency In (" + QuotedStr(ViewState("Currency")) + ") AND FgSubled IN ('N','P') And FgType = 'BS' and FgNormal = 'D' AND FgActive='Y' AND Account = " + QuotedStr(tbAccTransitWrhs.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccTransitWrhs.Text = ""
                tbAccTransitWrhsName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                BindToText(tbAccTransitWrhs, dr("Account").ToString)
                BindToText(tbAccTransitWrhsName, dr("Description").ToString)
            End If

        Catch ex As Exception
            lstatus.Text = "tbAccTransitWrhs Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccTransitRetur_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccTransitRetur.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("SELECT * FROM VMsAccount Where Currency In (" + QuotedStr(ViewState("Currency")) + ") And FgSubled IN ('N','P') and FgType = 'BS' and FgNormal = 'D' AND FgActive='Y' AND Account = " + QuotedStr(tbAccTransitRetur.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccTransitRetur.Text = ""
                tbAccTransitReturName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                BindToText(tbAccTransitRetur, dr("Account").ToString)
                BindToText(tbAccTransitReturName, dr("Description").ToString)
            End If

        Catch ex As Exception
            lstatus.Text = "tbAccTransitRetur Changed Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub tbAccSRetur_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccSRetur.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("SELECT * FROM VMsAccount Where FgSubled IN ('N','P') and FgType = 'PL' and FgNormal = 'D' AND FgActive='Y' AND Currency In (" + QuotedStr(ViewState("Currency")) + ") AND Account = " + QuotedStr(tbAccSRetur.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccSRetur.Text = ""
                tbAccSReturName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                BindToText(tbAccSRetur, dr("Account").ToString)
                BindToText(tbAccSReturName, dr("Description").ToString)
            End If

        Catch ex As Exception
            lstatus.Text = "tbAccPReturn Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccExpLoss_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccExpLoss.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("SELECT * FROM VMsAccount Where Currency In (" + QuotedStr(ViewState("Currency")) + ") And FgSubled IN ('N','P') and FgType = 'PL' AND FgActive='Y' AND Account = " + QuotedStr(tbAccExpLoss.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccExpLoss.Text = ""
                tbAccExpLossName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                BindToText(tbAccExpLoss, dr("Account").ToString)
                BindToText(tbAccExpLossName, dr("Description").ToString)
            End If

        Catch ex As Exception
            lstatus.Text = "tbAccSTCAdjust Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim SQLString As String = ""
        Try
            If tbAccInvent.Text.Trim = "" Then
                lstatus.Text = "<script language='javascript'>alert('Account Invent must be filled');</script>"
                tbAccInvent.Focus()
                Exit Sub
            End If


            If ViewState("State") = "Insert" Then
                If SQLExecuteScalar("SELECT WrhsType From MsProductMateriDt WHERE ProductMateri = " + QuotedStr(ViewState("Nmbr")) + " AND WrhsType =" + QuotedStr(ddlWrhsType.SelectedValue), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Product Materi" + QuotedStr(ViewState("Nmbr")) + " Warehouse Type " + QuotedStr(ddlWrhsType.Text) + " has already been exist"
                    Exit Sub
                End If



                SQLString = "Insert Into MsProductMateriDt (ProductMateri, WrhsType, AccInvent, AccSales, AccCOGS, AccTransitSJ, AccTransitWrhs, AccTransitReject, AccTransitRetur, AccSRetur, AccExpLoss, UserId, UserDate) " + _
                "SELECT " + QuotedStr(ViewState("Nmbr")) + "," + QuotedStr(ddlWrhsType.SelectedValue) + "," + _
                QuotedStr(tbAccInvent.Text) + "," + QuotedStr(tbAccSales.Text) + "," + _
                QuotedStr(tbAccCOGS.Text) + "," + QuotedStr(tbAccTransitSJ.Text) + "," + _
                QuotedStr(tbAccTransitWrhs.Text) + "," + QuotedStr(tbAccTransitReject.Text) + "," + _
                QuotedStr(tbAccTransitRetur.Text) + "," + QuotedStr(tbAccSRetur.Text) + "," + _
                QuotedStr(tbAccExpLoss.Text) + "," + _
                QuotedStr(ViewState("UserId")) + ", GetDate()"

            ElseIf ViewState("State") = "Edit" Then
                SQLString = "UPDATE MsProductMateriDt SET AccInvent = " + QuotedStr(tbAccInvent.Text) + _
                ", AccSales= " + QuotedStr(tbAccSales.Text) + _
                ", AccCogs= " + QuotedStr(tbAccCOGS.Text) + _
                ", AccTransitSj= " + QuotedStr(tbAccTransitSJ.Text) + _
                ", AccTransitWrhs= " + QuotedStr(tbAccTransitWrhs.Text) + _
                ", AccTransitReject= " + QuotedStr(tbAccTransitReject.Text) + _
                ", AccTransitRetur= " + QuotedStr(tbAccTransitRetur.Text) + _
                ", AccSRetur= " + QuotedStr(tbAccSRetur.Text) + _
                ", AccExpLoss = " + QuotedStr(tbAccExpLoss.Text) + _
                " WHERE WrhsType = " + QuotedStr(ddlWrhsType.Text) + _
                " AND ProductMateri =" + QuotedStr(ViewState("Nmbr"))
            End If

            SQLString = Replace(SQLString, "''", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            bindDataGridDt()
            pnlInputDt.Visible = False
            pnlDt.Visible = True
            PanelInfo.Visible = True
        Catch ex As Exception
            lstatus.Text = "btn Save Error : " + ex.ToString
        End Try
    End Sub
    Private Sub ModifyInputDt(ByVal State As Boolean)
        ddlWrhsType.Enabled = State
        tbAccInvent.Enabled = State
        tbAccCOGS.Enabled = State
        tbAccTransitSJ.Enabled = State
        tbAccSales.Enabled = State
        tbAccTransitWrhs.Enabled = State
        tbAccTransitRetur.Enabled = State
        tbAccSRetur.Enabled = State
        'tbAccExpLoss.Enabled = State
        'tbAccSReturn.Enabled = State
        'tbAccSTCAdjust.Enabled = State
        'tbAccSTCLost.Enabled = State
        'tbAccSampleExps.Enabled = State
    End Sub

End Class

