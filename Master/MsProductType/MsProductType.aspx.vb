Imports System.Data

Partial Class Master_MsProductType_MsProductType
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
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
        dsProductJenis.ConnectionString = ViewState("DBConnection")


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
            If ViewState("Sender") = "btnAccDisc" Then
                tbAccDisc.Text = Session("Result")(0).ToString
                tbAccDiscName.Text = Session("Result")(1).ToString
                tbAccDisc.Focus()
            End If
            If ViewState("Sender") = "btnAccTransitWrhs" Then
                tbAccTransitWrhs.Text = Session("Result")(0).ToString
                tbAccTransitWrhsname.Text = Session("Result")(1).ToString
                tbAccTransitWrhs.Focus()
            End If
            If ViewState("Sender") = "btnAccTransitPRetur" Then
                tbAccTransitPRetur.Text = Session("Result")(0).ToString
                tbAccTransitPReturName.Text = Session("Result")(1).ToString
                tbAccTransitPRetur.Focus()
            End If
            If ViewState("Sender") = "btnAccTransitSRetur" Then
                tbAccTransitSRetur.Text = Session("Result")(0).ToString
                tbAccTransitSReturname.Text = Session("Result")(1).ToString
                tbAccTransitSRetur.Focus()
            End If
            If ViewState("Sender") = "btnAccPReturn" Then
                tbAccPReturn.Text = Session("Result")(0).ToString
                tbAccPReturnName.Text = Session("Result")(1).ToString
                tbAccPReturn.Focus()
            End If
            If ViewState("Sender") = "btnAccSReturn" Then
                tbAccSReturn.Text = Session("Result")(0).ToString
                tbAccSReturnName.Text = Session("Result")(1).ToString
                tbAccSReturn.Focus()
            End If
            If ViewState("Sender") = "btnAccSTCAdjust" Then
                tbAccSTCAdjust.Text = Session("Result")(0).ToString
                tbAccSTCAdjustName.Text = Session("Result")(1).ToString
                tbAccSTCAdjust.Focus()
            End If
            If ViewState("Sender") = "btnAccSTCLost" Then
                tbAccSTCLost.Text = Session("Result")(0).ToString
                tbAccSTCLostName.Text = Session("Result")(1).ToString
                tbAccSTCLost.Focus()
            End If
            If ViewState("Sender") = "btnAccSampleExps" Then
                tbAccSampleExps.Text = Session("Result")(0).ToString
                tbAccSampleExpsName.Text = Session("Result")(1).ToString
                tbAccSampleExps.Focus()
            End If
            If ViewState("Sender") = "btnAccWIPLabor" Then
                tbAccWIPLabor.Text = Session("Result")(0).ToString
                tbAccWIPLaborName.Text = Session("Result")(1).ToString
                tbAccWIPLabor.Focus()
            End If
            If ViewState("Sender") = "btnAccWIPLabor2" Then
                tbAccWIPLabor2.Text = Session("Result")(0).ToString
                tbAccWIPLabor2Name.Text = Session("Result")(1).ToString
                tbAccWIPLabor2.Focus()
            End If
            If ViewState("Sender") = "btnAccWIPFOH" Then
                tbAccWIPFOH.Text = Session("Result")(0).ToString
                tbAccWIPFOHName.Text = Session("Result")(1).ToString
                tbAccWIPFOH.Focus()
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
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            SqlString = "Select * from VMsProductTypeView " + StrFilter
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "ProductType ASC"
            End If
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection"))
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub
    Private Sub bindDataGridDt()
        Dim tempDS As New DataSet()
        Dim DV As DataView
        Try
            tempDS = SQLExecuteQuery("SELECT A.*, C.AccountName AS AccInventName, " + _
            "E.AccountName AS acccogsName, P.AccountName AS AccDiscName, " + _
            "F.AccountName AS acctransitsjName, G.AccountName AS acctransitwrhsName, " + _
            "H.AccountName AS AccTransitPReturName, I.AccountName AS AccTransitSReturName, " + _
            "J.AccountName AS AccPReturnName, K.AccountName As AccSalesName, " + _
            "L.AccountName AS AccSReturnName, M.AccountName AS AccSTCAdjustName, " + _
            "N.AccountName AS AccSTCLostName, O.AccountName AS AccSampleExpsName, " + _
            "S.AccountName AS AccWIPLaborName, Q.AccountName AS AccWIPLabor2Name, " + _
            "R.AccountName AS AccWIPFOHName" + _
            " FROM MsProductTypeDt A LEFT OUTER JOIN " + _
            "MsAccount C ON A.AccInvent = C.Account LEFT OUTER JOIN " + _
            "MsAccount E ON A.acccogs = E.Account LEFT OUTER JOIN " + _
            "MsAccount F ON A.acctransitsj = F.Account LEFT OUTER JOIN " + _
            "MsAccount G ON A.acctransitwrhs = G.Account  LEFT OUTER JOIN " + _
            "MsAccount H ON A.AccTransitPRetur = H.Account LEFT OUTER JOIN " + _
            "MsAccount I ON A.AccTransitSRetur = I.Account LEFT OUTER JOIN " + _
            "MsAccount J ON A.AccPReturn = J.Account LEFT OUTER JOIN " + _
            "MsAccount K ON A.AccSales = K.Account LEFT OUTER JOIN " + _
            "MsAccount P ON A.AccDisc = P.Account LEFT OUTER JOIN " + _
            "MsAccount L ON A.AccSReturn = L.Account LEFT OUTER JOIN " + _
            "MsAccount M ON A.AccSTCAdjust = M.Account LEFT OUTER JOIN " + _
            "MsAccount N ON A.AccSTCLost = N.Account LEFT OUTER JOIN " + _
            "MsAccount O ON A.AccSampleExps = O.Account LEFT OUTER JOIN " + _
            "MsAccount S ON A.AccWIPLabor = S.Account LEFT OUTER JOIN " + _
            "MsAccount Q ON A.AccWIPLabor2 = Q.Account LEFT OUTER JOIN " + _
            "MsAccount R ON A.AccWIPFOH = R.Account " + _
            "WHERE A.ProductType =" + QuotedStr(ViewState("Nmbr")), ViewState("DBConnection").ToString)

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
        Dim ddlProductJenis As DropDownList

        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()
            obj = DataGrid.Rows(e.NewEditIndex)
            txt = obj.FindControl("TypeNameEdit")
            ddlProductJenis = obj.FindControl("ProductJenisEdit")


            txt.Focus()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
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
        Dim dbCode, dbName As TextBox
        Dim ddlProductJenis, ddlFgStock As DropDownList
        Dim GVR As GridViewRow
        Try
            If e.CommandName = "Insert" Then
                GVR = DataGrid.FooterRow
                dbCode = GVR.FindControl("ProductTypeAdd")
                dbName = GVR.FindControl("TypeNameAdd")
                ddlProductJenis = GVR.FindControl("ProductJenisAdd")
                ddlFgStock = GVR.FindControl("FgStockAdd")

                If ddlProductJenis.Text.Trim.Length = 0 Then
                    lstatus.Text = "<script language='javascript'>alert('Product Type Code must be filled');</script>"
                    dbCode.Focus()
                    Exit Sub
                End If
                If dbCode.Text.Trim.Length = 0 Then
                    lstatus.Text = "<script language='javascript'>alert('Product Type Code must be filled');</script>"
                    dbCode.Focus()
                    Exit Sub
                End If
                If dbName.Text.Trim.Length = 0 Then
                    lstatus.Text = "<script language='javascript'>alert('Product Type Name must be filled');</script>"
                    dbName.Focus()
                    Exit Sub
                End If

                If SQLExecuteScalar("SELECT Type_Code From VMsProductType WHERE Type_Code = " + QuotedStr(dbCode.Text) + "AND ProductJenis = " + ddlProductJenis.SelectedValue, ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Product Type " + QuotedStr(ddlProductJenis.SelectedValue) + " has already been exist"
                    Exit Sub
                End If

                'insert the new entry
                SQLString = "Insert into MsProductType (ProductType, TypeName, ProductJenis, UserID, UserDate) " + _
                "SELECT " + QuotedStr(dbCode.Text) + "," + QuotedStr(dbName.Text) + ", " + QuotedStr(ddlProductJenis.SelectedValue) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", GetDate()"
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                bindDataGrid()
            ElseIf e.CommandName = "View" Then
                Dim lbCode, lbName As Label
                GVR = DataGrid.Rows(CInt(e.CommandArgument))
                lbCode = GVR.FindControl("ProductType")
                lbName = GVR.FindControl("TypeName")
                ViewState("Nmbr") = lbCode.Text
                lbProductType.Text = lbCode.Text + " - " + lbName.Text
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

                        SQLExecuteNonQuery("Delete from MsProductTypeDt where ProductType = " + QuotedStr(ViewState("Nmbr")) + " AND WrhsType =" + QuotedStr(GVR.Cells(1).Text), ViewState("DBConnection").ToString)
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
        Dim ddlProductJenis As DropDownList
        Dim GVR As GridViewRow
        Try
            GVR = DataGrid.Rows(e.RowIndex)
            lbCode = GVR.FindControl("ProductTypeEdit")
            dbName = GVR.FindControl("TypeNameEdit")
            ddlProductJenis = GVR.FindControl("ProductJenisEdit")


            If dbName.Text.Trim.Length = 0 Then
                lstatus.Text = "<script language='javascript'>alert('Product Type Name must be filled.');</script>"
                dbName.Focus()
                Exit Sub
            End If

            SQLString = "Update MsProductType set TypeName = " + QuotedStr(dbName.Text) + _
            ", ProductJenis =" + QuotedStr(ddlProductJenis.SelectedValue) + _
            "  where ProductType = '" & lbCode.Text + "'"

            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = lstatus.Text + "DataGrid_Update Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub DataGridDt_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGridDt.RowUpdating
        Dim tbAccInvent, tbAccCogs, tbAccsj, tbAccWrhs, tbAccTransitPRetur, tbAccTransitSRetur, tbAccPReturn, tbAccSales, tbAccDisc, tbAccSReturn, tbAccSTCAdjust, tbAccSTCLost, tbAccSampleExps As TextBox
        Dim lbWrhsType As Label
        Dim SQLString As String
        Dim GVR As GridViewRow
        Try
            GVR = DataGridDt.Rows(e.RowIndex)
            lbWrhsType = GVR.FindControl("WrhsTypeEdit")
            tbAccInvent = GVR.FindControl("AccInventEdit")
            tbAccCogs = GVR.FindControl("AccCogsEdit")
            tbAccsj = GVR.FindControl("AccsjEdit")
            tbAccSales = GVR.FindControl("AccSalesEdit")
            tbAccDisc = GVR.FindControl("AccDiscEdit")
            tbAccWrhs = GVR.FindControl("AccWrhsEdit")
            tbAccTransitPRetur = GVR.FindControl("AccTransitPReturEdit")
            tbAccTransitSRetur = GVR.FindControl("AccTransitSReturEdit")
            tbAccPReturn = GVR.FindControl("AccPReturnEdit")
            tbAccSReturn = GVR.FindControl("AccSReturnEdit")
            tbAccSTCAdjust = GVR.FindControl("AccSTCAdjustEdit")
            tbAccSTCLost = GVR.FindControl("AccSTCLostEdit")
            tbAccSampleExps = GVR.FindControl("AccSampleExpsEdit")

            SQLString = "UPDATE MsProductTypeDt SET ACCInvent = " + QuotedStr(tbAccInvent.Text) + _
            ", AccCogs= " + QuotedStr(tbAccCogs.Text) + ", AccTransitSj= " + QuotedStr(tbAccsj.Text) + _
            ", AccTransitWrhs= " + QuotedStr(tbAccWrhs.Text) + _
            ", AccTransitPRetur= " + QuotedStr(tbAccTransitPRetur.Text) + ", AccTransitSRetur= " + QuotedStr(tbAccTransitSRetur.Text) + _
            ", AccSales= " + QuotedStr(tbAccSales.Text) + _
            ", AccDisc= " + QuotedStr(tbAccDisc.Text) + _
            ", AccPReturn= " + QuotedStr(tbAccPReturn.Text) + _
            ", AccSReturn = " + QuotedStr(tbAccSReturn.Text) + _
            ", AccSTCAdjust = " + QuotedStr(tbAccSTCAdjust.Text) + _
            ", AccSTCLost = " + QuotedStr(tbAccSTCLost.Text) + _
            ", AccSampleExps = " + QuotedStr(tbAccSampleExps.Text) + _
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
        Dim ddlJenis As DropDownList
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            txtID = DataGrid.Rows(e.RowIndex).FindControl("ProductType")
            ddlJenis = DataGrid.Rows(e.RowIndex).FindControl("ProductJenis")

            SQLExecuteNonQuery("Delete from MsProductTypeDt where ProductType = '" & txtID.Text & "' ", ViewState("DBConnection").ToString)
            SQLExecuteNonQuery("Delete from MsProductType where ProductType = '" & txtID.Text & "' AND ProductJenis = " + QuotedStr(ddlJenis.SelectedValue), ViewState("DBConnection").ToString)
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

        '    SQLExecuteNonQuery("Delete from MsProductTypeDt where ProductType = " + QuotedStr(ViewState("Nmbr")) + " AND WrhsType =" + QuotedStr(txtID.Text))
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
        Dim tbAccInvent, tbAccCogs, tbAccsj, tbAccSales, tbAccDisc, tbAccWrhs, tbAccTransitPRetur, tbAccTransitSRetur, tbAccPReturn, tbAccSReturn, tbAccSTCAdjust, tbAccSTCLost, tbAccSampleExps As TextBox
        Dim lbAccInventName, lbAccCogsName, lbAccsjName, lbAccSalesName, lbAccDiscName, lbAccWrhsName, lbAccTransitPReturName, lbAccTransitSReturName, lbAccPReturnName, lbAccSReturnName, lbAccSTCAdjustName, lbAccSTCLostName, lbAccSampleExpsName As Label
        Try
            Count = DataGridDt.Controls(0).Controls.Count
            'dgi = DataGridDt.Controls(0).Controls(Count - 2) '-1 for allowpaging = False
            dgi = DataGridDt.FooterRow

            tbAccInvent = dgi.FindControl("AccInventAdd")
            lbAccInventName = dgi.FindControl("AccInventNameAdd")
            tbAccCogs = dgi.FindControl("AccCogsAdd")
            lbAccCogsName = dgi.FindControl("AccCogsNameAdd")
            tbAccsj = dgi.FindControl("AccsjAdd")
            lbAccsjName = dgi.FindControl("AccsjNameAdd")
            tbAccSales = dgi.FindControl("AccSalesAdd")
            lbAccSalesName = dgi.FindControl("AccSalesNameAdd")
            tbAccDisc = dgi.FindControl("AccDiscAdd")
            lbAccDiscName = dgi.FindControl("AccDiscNameAdd")
            tbAccWrhs = dgi.FindControl("AccWrhsAdd")
            lbAccWrhsName = dgi.FindControl("AccWrhsNameAdd")
            tbAccTransitPRetur = dgi.FindControl("AccTransitPReturAdd")
            lbAccTransitPReturName = dgi.FindControl("AccTransitPReturNameAdd")
            tbAccTransitSRetur = dgi.FindControl("AccTransitSReturAdd")
            lbAccTransitSReturName = dgi.FindControl("AccTransitSReturNameAdd")
            tbAccPReturn = dgi.FindControl("AccPReturnAdd")
            lbAccPReturnName = dgi.FindControl("AccPReturnNameAdd")
            tbAccSReturn = dgi.FindControl("AccSReturnAdd")
            lbAccSReturnName = dgi.FindControl("AccSReturnNameAdd")
            tbAccSTCAdjust = dgi.FindControl("AccSTCAdjustAdd")
            lbAccSTCAdjustName = dgi.FindControl("AccSTCAdjustNameAdd")
            tbAccSTCLost = dgi.FindControl("AccSTCLostAdd")
            lbAccSTCLostName = dgi.FindControl("AccSTCLostNameAdd")
            tbAccSampleExps = dgi.FindControl("AccSampleExpsAdd")
            lbAccSampleExpsName = dgi.FindControl("AccSampleExpsNameAdd")

            tbAccInvent.Text = ""
            lbAccInventName.Text = ""
            tbAccCogs.Text = ""
            lbAccCogsName.Text = ""
            tbAccsj.Text = ""
            lbAccsjName.Text = ""
            tbAccSales.Text = ""
            lbAccSalesName.Text = ""
            tbAccDisc.Text = ""
            lbAccDiscName.Text = ""
            tbAccWrhs.Text = ""
            lbAccWrhsName.Text = ""
            tbAccTransitPRetur.Text = ""
            lbAccTransitPReturName.Text = ""
            tbAccTransitSRetur.Text = ""
            lbAccTransitSReturName.Text = ""
            tbAccPReturn.Text = ""
            lbAccPReturnName.Text = ""
            tbAccSReturn.Text = ""
            lbAccSReturnName.Text = ""
            tbAccSTCAdjust.Text = ""
            lbAccSTCAdjustName.Text = ""
            tbAccSTCLost.Text = ""
            lbAccSTCLostName.Text = ""
            tbAccSampleExps.Text = ""
            lbAccSampleExpsName.Text = ""

        Catch ex As Exception
            lstatus.Text = "Warehouse Type Add Index Changed Error : " + ex.ToString
        End Try
    End Sub


    

    

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("DBConnection") = ViewState("DBConnection")
            Session("PrintType") = "Print"
            Session("SelectCommand") = "EXEC S_FormPrintMaster3 'MsProductType','ProductJenis','producttype','typename'," + QuotedStr(lblTitle.Text) + ",'Product Jenis','Product Type Code','Product Type Name'," + QuotedStr(StrFilter) + "," + QuotedStr(ViewState("UserId"))
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster2.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            lstatus.Text = "Btn Print Error : " + ex.ToString
        End Try

    End Sub
    Protected Sub FillTextBoxDt(ByVal Nmbr As String, ByVal Code As String)
        Try
            Dim dr As DataRow
            dr = SQLExecuteQuery("SELECT A.*, C.AccountName AS AccInventName, " + _
            "E.AccountName AS acccogsName, P.AccountName AS AccDiscName, " + _
            "F.AccountName AS acctransitsjName, G.AccountName AS acctransitwrhsName, " + _
            "H.AccountName AS AccTransitPReturName, I.AccountName AS AccTransitSReturName, " + _
            "J.AccountName AS AccPReturnName, K.AccountName As AccSalesName, " + _
            "L.AccountName AS AccSReturnName, M.AccountName AS AccSTCAdjustName, " + _
            "S.AccountName AS AccWIPLaborName, Q.AccountName AS AccWIPLabor2Name, " + _
            "R.AccountName AS AccWIPFOHName, " + _
            "N.AccountName AS AccSTCLostName, O.AccountName AS AccSampleExpsName FROM MsProductTypeDt A LEFT OUTER JOIN " + _
            "MsAccount C ON A.AccInvent = C.Account LEFT OUTER JOIN " + _
            "MsAccount E ON A.acccogs = E.Account LEFT OUTER JOIN " + _
            "MsAccount F ON A.acctransitsj = F.Account LEFT OUTER JOIN " + _
            "MsAccount G ON A.acctransitwrhs = G.Account  LEFT OUTER JOIN " + _
            "MsAccount H ON A.AccTransitPRetur = H.Account LEFT OUTER JOIN " + _
            "MsAccount I ON A.AccTransitSRetur = I.Account LEFT OUTER JOIN " + _
            "MsAccount K ON A.AccSales = K.Account LEFT OUTER JOIN " + _
            "MsAccount P ON A.AccDisc = P.Account LEFT OUTER JOIN " + _
            "MsAccount J ON A.AccPReturn = J.Account LEFT OUTER JOIN " + _
            "MsAccount L ON A.AccSReturn = L.Account LEFT OUTER JOIN " + _
            "MsAccount M ON A.AccSTCAdjust = M.Account LEFT OUTER JOIN " + _
            "MsAccount N ON A.AccSTCLost = N.Account LEFT OUTER JOIN " + _
            "MsAccount S ON A.AccWIPLabor = S.Account LEFT OUTER JOIN " + _
            "MsAccount Q ON A.AccWIPLabor2 = Q.Account LEFT OUTER JOIN " + _
            "MsAccount R ON A.AccWIPFOH = R.Account LEFT OUTER JOIN " + _
            "MsAccount O ON A.AccSampleExps = O.Account WHERE A.ProductType =" + QuotedStr(Nmbr) + " AND WrhsType = " + QuotedStr(Code), ViewState("DBConnection").ToString).Tables(0).Rows(0)

            ClearDt()

            BindToText(tbAccInvent, dr("AccInvent").ToString)
            BindToText(tbAccInventName, dr("AccInventName").ToString)
            BindToText(tbAccCOGS, dr("acccogs").ToString)
            BindToText(tbAccCOGSName, dr("acccogsName").ToString)
            BindToText(tbAccTransitSJ, dr("acctransitsj").ToString)
            BindToText(tbAccTransitSJName, dr("acctransitsjName").ToString)
            BindToText(tbAccTransitWrhs, dr("acctransitwrhs").ToString)
            BindToText(tbAccTransitWrhsname, dr("acctransitwrhsName").ToString)
            BindToText(tbAccSales, dr("AccSales").ToString)
            BindToText(tbAccSalesName, dr("AccSalesName").ToString)
            BindToText(tbAccDisc, dr("AccDisc").ToString)
            BindToText(tbAccDiscName, dr("AccDiscName").ToString)
            BindToText(tbAccTransitPRetur, dr("AccTransitPRetur").ToString)
            BindToText(tbAccTransitPReturName, dr("AccTransitPReturName").ToString)
            BindToText(tbAccTransitSRetur, dr("AccTransitSRetur").ToString)
            BindToText(tbAccTransitSReturname, dr("AccTransitSReturName").ToString)
            BindToText(tbAccPReturn, dr("AccPReturn").ToString)
            BindToText(tbAccPReturnName, dr("AccPReturnName").ToString)
            BindToText(tbAccSReturn, dr("AccSReturn").ToString)
            BindToText(tbAccSReturnName, dr("AccSReturnName").ToString)
            BindToText(tbAccSTCAdjust, dr("AccSTCAdjust").ToString)
            BindToText(tbAccSTCAdjustName, dr("AccSTCAdjustName").ToString)
            BindToText(tbAccSTCLost, dr("AccSTCLost").ToString)
            BindToText(tbAccSTCLostName, dr("AccSTCLostName").ToString)
            BindToText(tbAccSampleExps, dr("AccSampleExps").ToString)
            BindToText(tbAccSampleExpsName, dr("AccSampleExpsName").ToString)
            BindToText(tbAccWIPLabor, dr("AccWIPLabor").ToString)
            BindToText(tbAccWIPLaborName, dr("AccWIPLaborName").ToString)
            BindToText(tbAccWIPLabor2, dr("AccWIPLabor2").ToString)
            BindToText(tbAccWIPLabor2Name, dr("AccWIPLabor2Name").ToString)
            BindToText(tbAccWIPFOH, dr("AccWIPFOH").ToString)
            BindToText(tbAccWIPFOHName, dr("AccWIPFOHName").ToString)
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
            tbAccDisc.Text = ""
            tbAccDiscName.Text = ""
            tbAccTransitWrhs.Text = ""
            tbAccTransitWrhsname.Text = ""
            tbAccTransitPRetur.Text = ""
            tbAccTransitPReturName.Text = ""
            tbAccTransitSRetur.Text = ""
            tbAccTransitSReturname.Text = ""
            tbAccPReturn.Text = ""
            tbAccPReturnName.Text = ""
            tbAccSReturn.Text = ""
            tbAccSReturnName.Text = ""
            tbAccSTCAdjust.Text = ""
            tbAccSTCAdjustName.Text = ""
            tbAccSTCLost.Text = ""
            tbAccSTCLostName.Text = ""
            tbAccSampleExps.Text = ""
            tbAccSampleExpsName.Text = ""
            tbAccWIPLabor.Text = ""
            tbAccWIPLaborName.Text = ""
            tbAccWIPLabor2.Text = ""
            tbAccWIPLabor2Name.Text = ""
            tbAccWIPFOH.Text = ""
            tbAccWIPFOHName.Text = ""

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
            AttachScript("OpenPopup();", Page, Me.GetType())
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
            AttachScript("OpenPopup();", Page, Me.GetType())
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
            AttachScript("OpenPopup();", Page, Me.GetType())
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
            AttachScript("OpenPopup();", Page, Me.GetType())
            ViewState("Sender") = "btnAccSales"
        Catch ex As Exception
            lstatus.Text = "btnAccSales Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccDisc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccDisc.Click
        Dim FieldResult As String
        Try
            Session("filter") = "SELECT Account, Description From VMsAccount Where Currency In (" + QuotedStr(ViewState("Currency")) + ") And FgType = 'PL' and FgNormal = 'C' AND FgSubled IN ('N', 'P')"
            FieldResult = "Account, Description"
            Session("DBConnection") = ViewState("DBConnection")
            Session("Column") = FieldResult.Split(",")
            ViewState("Sender") = "btnAccDisc"
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
            AttachScript("OpenPopup();", Page, Me.GetType())
            ViewState("Sender") = "btnAccTransitWrhs"
        Catch ex As Exception
            lstatus.Text = "btnAccTransitWrhs Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccTransitPRetur_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccTransitPRetur.Click
        Dim FieldResult As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT * FROM VMsAccount Where Currency In (" + QuotedStr(ViewState("Currency")) + ") And FgSubled IN ('N','P') and FgType = 'BS' and FgNormal = 'D' AND FgActive='Y'"
            FieldResult = "Account, Description"
            Session("Column") = FieldResult.Split(",")
            AttachScript("OpenPopup();", Page, Me.GetType())
            ViewState("Sender") = "btnAccTransitPRetur"
        Catch ex As Exception
            lstatus.Text = "btnAccTransitPRetur Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccTransitSRetur_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccTransitSRetur.Click
        Dim FieldResult As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT * FROM VMsAccount Where Currency In (" + QuotedStr(ViewState("Currency")) + ") And FgSubled IN ('N','P') and FgType = 'BS' and FgNormal = 'D' AND FgActive='Y'"
            FieldResult = "Account, Description"
            Session("Column") = FieldResult.Split(",")
            AttachScript("OpenPopup();", Page, Me.GetType())
            ViewState("Sender") = "btnAccTransitSRetur"
        Catch ex As Exception
            lstatus.Text = "btnAccTransitSRetur Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccSReturn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccSReturn.Click
        Dim FieldResult As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT * FROM VMsAccount Where Currency In (" + QuotedStr(ViewState("Currency")) + ") And FgSubled IN ('N','P') and FgType = 'PL' AND FgActive='Y'"
            FieldResult = "Account, Description"
            Session("Column") = FieldResult.Split(",")
            AttachScript("OpenPopup();", Page, Me.GetType())
            ViewState("Sender") = "btnAccSReturn"
        Catch ex As Exception
            lstatus.Text = "btnAccSReturn Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccPReturn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccPReturn.Click
        Dim FieldResult As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT * FROM VMsAccount Where FgSubled IN ('N','P') and FgType = 'PL' and FgNormal = 'D' AND FgActive='Y' AND Currency In (" + QuotedStr(ViewState("Currency")) + ") "
            FieldResult = "Account, Description"
            Session("Column") = FieldResult.Split(",")
            AttachScript("OpenPopup();", Page, Me.GetType())
            ViewState("Sender") = "btnAccPReturn"
        Catch ex As Exception
            lstatus.Text = "btnAccPReturn Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccSTCAdjust_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccSTCAdjust.Click
        Dim FieldResult As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT * FROM VMsAccount Where Currency In (" + QuotedStr(ViewState("Currency")) + ") And FgSubled IN ('N','P') and FgType = 'PL' AND FgActive='Y'"
            FieldResult = "Account, Description"
            Session("Column") = FieldResult.Split(",")
            AttachScript("OpenPopup();", Page, Me.GetType())
            ViewState("Sender") = "btnAccSTCAdjust"
        Catch ex As Exception
            lstatus.Text = "btnAccSTCAdjust Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccSTCLost_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccSTCLost.Click
        Dim FieldResult As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            'Session("filter") = "SELECT * FROM VMsAccount Where Currency In (" + QuotedStr(ViewState("Currency")) + ") And FgSubled IN ('N','P') and FgType = 'BS' and FgNormal = 'D' AND FgActive='Y'"
            Session("filter") = "SELECT * FROM VMsAccount Where Currency In (" + QuotedStr(ViewState("Currency")) + ") And FgSubled IN ('N','P') and FgType = 'PL' AND FgActive='Y'"
            FieldResult = "Account, Description"
            Session("Column") = FieldResult.Split(",")
            AttachScript("OpenPopup();", Page, Me.GetType())
            ViewState("Sender") = "btnAccSTCLost"
        Catch ex As Exception
            lstatus.Text = "btnAccSTCLost Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccSampleExps_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccSampleExps.Click
        Dim FieldResult As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT * FROM VMsAccount Where Currency In (" + QuotedStr(ViewState("Currency")) + ") And FgSubled IN ('N','P') and FgType = 'BS' and FgNormal = 'D' AND FgActive='Y'"
            FieldResult = "Account, Description"
            Session("Column") = FieldResult.Split(",")
            AttachScript("OpenPopup();", Page, Me.GetType())
            ViewState("Sender") = "btnAccSampleExps"
        Catch ex As Exception
            lstatus.Text = "btnAccSampleExps Error : " + ex.ToString
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

    Protected Sub tbAccDisc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccDisc.TextChanged

        Dim DsAccDisc As DataSet
        Dim DrAccDisc As DataRow
        Try
		'diskon tdnya N dan F
            'DsAccDisc = SQLExecuteQuery("SELECT Account, Description From VMsAccount Where Currency In (" + QuotedStr(ddlCurrency.SelectedValue) + "," + QuotedStr(ViewState("Currency")) + ") And FgType = 'PL' and FgNormal = 'C' AND FgSubled IN ('N', 'P') AND Account = " + QuotedStr(tbAccDisc.Text), ViewState("DBConnection").ToString)
            DsAccDisc = SQLExecuteQuery("SELECT Account, Description From VMsAccount Where Currency In (" + QuotedStr(ViewState("Currency")) + ") And FgType = 'PL' and FgNormal = 'C' AND FgSubled IN ('N', 'P') AND Account = " + QuotedStr(tbAccDisc.Text), ViewState("DBConnection").ToString)
            If DsAccDisc.Tables(0).Rows.Count = 1 Then
                DrAccDisc = DsAccDisc.Tables(0).Rows(0)
                tbAccDisc.Text = DrAccDisc("Account")
                tbAccDiscName.Text = DrAccDisc("Description")
            Else
                tbAccDisc.Text = ""
                tbAccDiscName.Text = ""
            End If
            tbAccDisc.Focus()
        Catch ex As Exception
            Throw New Exception("tb Acc Disc To Error : " + ex.ToString)
        End Try


    End Sub
    Protected Sub tbAccTransitWrhs_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccTransitWrhs.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("SELECT * FROM VMsAccount Where Currency In (" + QuotedStr(ViewState("Currency")) + ") AND FgSubled IN ('N','P') And FgType = 'BS' and FgNormal = 'D' AND FgActive='Y' AND Account = " + QuotedStr(tbAccTransitWrhs.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccTransitWrhs.Text = ""
                tbAccTransitWrhsname.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                BindToText(tbAccTransitWrhs, dr("Account").ToString)
                BindToText(tbAccTransitWrhsname, dr("Description").ToString)
            End If

        Catch ex As Exception
            lstatus.Text = "tbAccTransitWrhs Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccTransitPRetur_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccTransitPRetur.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("SELECT * FROM VMsAccount Where Currency In (" + QuotedStr(ViewState("Currency")) + ") And FgSubled IN ('N','P') and FgType = 'BS' and FgNormal = 'D' AND FgActive='Y' AND Account = " + QuotedStr(tbAccTransitPRetur.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccTransitPRetur.Text = ""
                tbAccTransitPReturName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                BindToText(tbAccTransitPRetur, dr("Account").ToString)
                BindToText(tbAccTransitPReturName, dr("Description").ToString)
            End If

        Catch ex As Exception
            lstatus.Text = "tbAccTransitPRetur Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccTransitSRetur_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccTransitSRetur.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("SELECT * FROM VMsAccount Where Currency In (" + QuotedStr(ViewState("Currency")) + ") And FgSubled IN ('N','P') and FgType = 'BS' and FgNormal = 'D' AND FgActive='Y' AND Account = " + QuotedStr(tbAccTransitSRetur.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccTransitSRetur.Text = ""
                tbAccTransitSReturname.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                BindToText(tbAccTransitSRetur, dr("Account").ToString)
                BindToText(tbAccTransitSReturname, dr("Description").ToString)
            End If

        Catch ex As Exception
            lstatus.Text = "tbAccTransitSRetur Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccPReturn_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccPReturn.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("SELECT * FROM VMsAccount Where FgSubled IN ('N','P') and FgType = 'PL' and FgNormal = 'D' AND FgActive='Y' AND Currency In (" + QuotedStr(ViewState("Currency")) + ") AND Account = " + QuotedStr(tbAccPReturn.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccPReturn.Text = ""
                tbAccPReturnName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                BindToText(tbAccPReturn, dr("Account").ToString)
                BindToText(tbAccPReturnName, dr("Description").ToString)
            End If

        Catch ex As Exception
            lstatus.Text = "tbAccPReturn Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccSTCAdjust_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccSTCAdjust.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("SELECT * FROM VMsAccount Where Currency In (" + QuotedStr(ViewState("Currency")) + ") And FgSubled IN ('N','P') and FgType = 'PL' AND FgActive='Y' AND Account = " + QuotedStr(tbAccSTCAdjust.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccSTCAdjust.Text = ""
                tbAccSTCAdjustName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                BindToText(tbAccSTCAdjust, dr("Account").ToString)
                BindToText(tbAccSTCAdjustName, dr("Description").ToString)
            End If

        Catch ex As Exception
            lstatus.Text = "tbAccSTCAdjust Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccSTCLost_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccSTCLost.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("SELECT * FROM VMsAccount Where Currency In (" + QuotedStr(ViewState("Currency")) + ") And FgSubled IN ('N','P') and FgType = 'BS' and FgNormal = 'D' AND FgActive='Y' AND Account = " + QuotedStr(tbAccSTCLost.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccSTCLost.Text = ""
                tbAccSTCLostName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                BindToText(tbAccSTCLost, dr("Account").ToString)
                BindToText(tbAccSTCLostName, dr("Description").ToString)
            End If

        Catch ex As Exception
            lstatus.Text = "tbAccSTCLost Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccSampleExps_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccSampleExps.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("SELECT * FROM VMsAccount Where Currency In (" + QuotedStr(ViewState("Currency")) + ") And FgSubled IN ('N','P') and FgType = 'BS' and FgNormal = 'D' AND FgActive='Y' AND Account = " + QuotedStr(tbAccSampleExps.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccSampleExps.Text = ""
                tbAccSampleExpsName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                BindToText(tbAccSampleExps, dr("Account").ToString)
                BindToText(tbAccSampleExpsName, dr("Description").ToString)
            End If

        Catch ex As Exception
            lstatus.Text = "tbAccSampleExps Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccSReturn_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccSReturn.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("SELECT * FROM VMsAccount Where Currency In (" + QuotedStr(ViewState("Currency")) + ") And FgSubled IN ('N','P') and FgType = 'BS' and FgNormal = 'D' AND FgActive='Y' AND Account = " + QuotedStr(tbAccSReturn.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccSReturn.Text = ""
                tbAccSReturnName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                BindToText(tbAccSReturn, dr("Account").ToString)
                BindToText(tbAccSReturnName, dr("Description").ToString)
            End If

        Catch ex As Exception
            lstatus.Text = "tbAccSReturn Changed Error : " + ex.ToString
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
                If SQLExecuteScalar("SELECT WrhsType From MsProductTypeDt WHERE ProductType = " + QuotedStr(ViewState("Nmbr")) + " AND WrhsType =" + QuotedStr(ddlWrhsType.SelectedValue), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Product Type" + QuotedStr(ViewState("Nmbr")) + " Warehouse Type " + QuotedStr(ddlWrhsType.Text) + " has already been exist"
                    Exit Sub
                End If



                SQLString = "Insert Into MsProductTypeDt (ProductType, WrhsType, AccInvent, AccCogs,  AcctransitSJ, AccSales, AccDisc, acctransitwrhs, AccTransitPRetur, " + _
                "AccTransitSRetur, AccPReturn, AccSReturn, AccSTCAdjust, AccSTCLost, AccSampleExps, AccWIPLabor, AccWIPLabor2, AccWIPFOH, UserId, UserDate) " + _
                "SELECT " + QuotedStr(ViewState("Nmbr")) + "," + QuotedStr(ddlWrhsType.SelectedValue) + "," + _
                QuotedStr(tbAccInvent.Text) + "," + QuotedStr(tbAccCOGS.Text) + "," + _
                QuotedStr(tbAccTransitSJ.Text) + "," + QuotedStr(tbAccSales.Text) + "," + QuotedStr(tbAccDisc.Text) + "," + _
                QuotedStr(tbAccTransitWrhs.Text) + "," + QuotedStr(tbAccTransitPRetur.Text) + "," + _
                QuotedStr(tbAccTransitSRetur.Text) + "," + QuotedStr(tbAccPReturn.Text) + "," + _
                QuotedStr(tbAccSReturn.Text) + "," + QuotedStr(tbAccSTCAdjust.Text) + "," + _
                QuotedStr(tbAccSTCLost.Text) + "," + QuotedStr(tbAccSampleExps.Text) + "," + _
                QuotedStr(tbAccWIPLabor.Text) + "," + QuotedStr(tbAccWIPLabor2.Text) + "," + _
                QuotedStr(tbAccWIPFOH.Text) + "," + _
                QuotedStr(ViewState("UserId")) + ", GetDate()"
            ElseIf ViewState("State") = "Edit" Then
                SQLString = "UPDATE MsProductTypeDt SET ACCInvent = " + QuotedStr(tbAccInvent.Text) + _
                ", AccCogs= " + QuotedStr(tbAccCOGS.Text) + ", AccTransitSj= " + QuotedStr(tbAccTransitSJ.Text) + _
                ", AccTransitWrhs= " + QuotedStr(tbAccTransitWrhs.Text) + _
                ", AccTransitPRetur= " + QuotedStr(tbAccTransitPRetur.Text) + ", AccTransitSRetur= " + QuotedStr(tbAccTransitSRetur.Text) + _
                ", AccSales= " + QuotedStr(tbAccSales.Text) + _
                ", AccDisc= " + QuotedStr(tbAccDisc.Text) + _
                ", AccPReturn= " + QuotedStr(tbAccPReturn.Text) + _
                ", AccSReturn = " + QuotedStr(tbAccSReturn.Text) + _
                ", AccSTCAdjust = " + QuotedStr(tbAccSTCAdjust.Text) + _
                ", AccSTCLost = " + QuotedStr(tbAccSTCLost.Text) + _
                ", AccSampleExps = " + QuotedStr(tbAccSampleExps.Text) + _
                ", AccWIPLabor = " + QuotedStr(tbAccWIPLabor.Text) + _
                ", AccWIPLabor2 = " + QuotedStr(tbAccWIPLabor2.Text) + _
                ", AccWIPFOH = " + QuotedStr(tbAccWIPFOH.Text) + _
                " WHERE WrhsType = " + QuotedStr(ddlWrhsType.Text) + _
                " AND ProductType =" + QuotedStr(ViewState("Nmbr"))
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
        tbAccTransitPRetur.Enabled = State
        tbAccTransitSRetur.Enabled = State
        tbAccPReturn.Enabled = State
        tbAccSReturn.Enabled = State
        tbAccSTCAdjust.Enabled = State
        tbAccSTCLost.Enabled = State
        tbAccSampleExps.Enabled = State        
    End Sub
    
    Protected Sub btnAccWIPFOH_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccWIPFOH.Click
        Dim FieldResult As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT * FROM VMsAccount Where Currency In (" + QuotedStr(ViewState("Currency")) + ") AND FgActive='Y'"
            FieldResult = "Account, Description"
            Session("Column") = FieldResult.Split(",")
            AttachScript("OpenPopup();", Page, Me.GetType())
            ViewState("Sender") = "btnAccWIPFOH"
        Catch ex As Exception
            lstatus.Text = "btnAccWIPFOH Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccWIPLabor_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccWIPLabor.Click
        Dim FieldResult As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT * FROM VMsAccount Where Currency In (" + QuotedStr(ViewState("Currency")) + ") AND FgActive='Y'"
            FieldResult = "Account, Description"
            Session("Column") = FieldResult.Split(",")
            AttachScript("OpenPopup();", Page, Me.GetType())
            ViewState("Sender") = "btnAccWIPLabor"
        Catch ex As Exception
            lstatus.Text = "btnAccWIPLabor Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAccWIPLabor2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccWIPLabor2.Click
        Dim FieldResult As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT * FROM VMsAccount Where Currency In (" + QuotedStr(ViewState("Currency")) + ") AND FgActive='Y'"
            FieldResult = "Account, Description"
            Session("Column") = FieldResult.Split(",")
            AttachScript("OpenPopup();", Page, Me.GetType())
            ViewState("Sender") = "btnAccWIPLabor2"
        Catch ex As Exception
            lstatus.Text = "btnAccWIPLabor2 Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccWIPFOH_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccWIPFOH.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("SELECT * FROM VMsAccount Where FgActive='Y' AND Currency In (" + QuotedStr(ViewState("Currency")) + ") AND Account = " + QuotedStr(tbAccWIPFOH.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccWIPFOH.Text = ""
                tbAccWIPFOHName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                BindToText(tbAccWIPFOH, dr("Account").ToString)
                BindToText(tbAccWIPFOHName, dr("Description").ToString)
            End If

        Catch ex As Exception
            lstatus.Text = "tbAccWIPFOH Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccWIPLabor_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccWIPLabor.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("SELECT * FROM VMsAccount Where FgActive='Y' AND Currency In (" + QuotedStr(ViewState("Currency")) + ") AND Account = " + QuotedStr(tbAccWIPLabor.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccWIPLabor.Text = ""
                tbAccWIPLaborName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                BindToText(tbAccWIPLabor, dr("Account").ToString)
                BindToText(tbAccWIPLaborName, dr("Description").ToString)
            End If

        Catch ex As Exception
            lstatus.Text = "tbAccWIPLabor Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccWIPLabor2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccWIPLabor2.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("SELECT * FROM VMsAccount Where FgActive='Y' AND Currency In (" + QuotedStr(ViewState("Currency")) + ") AND Account = " + QuotedStr(tbAccWIPLabor2.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccWIPLabor2.Text = ""
                tbAccWIPLabor2Name.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                BindToText(tbAccWIPLabor2, dr("Account").ToString)
                BindToText(tbAccWIPLabor2Name, dr("Description").ToString)
            End If

        Catch ex As Exception
            lstatus.Text = "tbAccWIPLabor Changed Error : " + ex.ToString
        End Try
    End Sub
End Class

