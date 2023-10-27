Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data
Imports System.Data.SqlClient

Partial Class Master_Setting
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim SQLString As String
        Dim dbCode, dbName As TextBox

        If Not IsPostBack Then
            InitProperty()
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            'bindDataGrid()
            Dim dt As DataTable
            dt = SQLExecuteQuery("Select RptTitle, MenuName, COALESCE(MenuParam,'') AS MenuParam, COALESCE(FastRpt,'') AS FastRpt, COALESCE(ReportType,'') AS ReportType, COALESCE(ReportType2,'') AS ReportType2, COALESCE(FgPrintValue,'N') AS FgPrintValue, COALESCE(FgForceNewPage,'N') AS FgForceNewPage from MsMenu WHERE MenuId = " + QuotedStr(Request.QueryString("ContainerId").ToString), ViewState("DBConnection").ToString).Tables(0)
            lblTitle.Text = dt.Rows(0)("MenuName").ToString

         
            If Not IsPostBack Then
                bindDataGrid()
            End If
        End If

        If Not Session("Result") Is Nothing Then

            If ViewState("Sender") = "btnOut" Then
                Dim drResult As DataRow
                Dim FirstTime As Boolean = True
                For Each drResult In Session("Result").Rows

                    If SQLExecuteScalar("SELECT MenuID FROM SAUserSetting WHERE UserID = " + QuotedStr(ViewState("UserId")) + " AND MenuId = " + QuotedStr(drResult("MenuId").ToString), ViewState("DBConnection").ToString).Length > 0 Then
                        lstatus.Text = "Menu " + QuotedStr(drResult("MenuId").ToString) + " has already been exist"
                        Exit Sub
                    End If

                    'insert the new entry
                    SQLString = "Insert into SAUserSetting SELECT '" + ViewState("UserId").ToString + _
                    "', '" + drResult("MenuId").ToString + "', '" + ViewState("UserId").ToString + "', getDate()"

                    SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

                    bindDataGrid()

                    ''insert
                    'If FirstTime Then

                    '    BindToText(tbSuppCode, drResult("Supplier_Code").ToString)
                    '    BindToText(tbSuppName, drResult("Supplier_Name").ToString)
                    '    tbSuppCode_TextChanged(Nothing, Nothing)
                    '    BindToText(tbPONo, drResult("PO_No").ToString)
                    '    BindToDropList(ddlFgPriceInclude, drResult("FgPriceIncludePPN"))
                    '    BindToText(tbPPN, drResult("PPn"))
                    '    BindToText(tbRemark, drResult("RemarkHd"))


                    'End If

                    'If CekExistData(ViewState("Dt"), "ReffType,ReffNmbr,Product,CostCtr", drResult("RR_Type") + "|" + drResult("RR_No") + "|" + drResult("Product_Code") + "|" + drResult("Cost_Ctr")) = False Then
                    '    Dim dr As DataRow
                    '    dr = ViewState("Dt").NewRow
                    '    dr("ReffType") = drResult("RR_Type")
                    '    dr("ReffNmbr") = drResult("RR_No")
                    '    ViewState("FgReport") = drResult("Report")
                    '    dr("Product") = drResult("Product_Code")
                    '    dr("Product_Name") = drResult("Product_Name")
                    '    dr("CostCtr") = drResult("Cost_Ctr")
                    '    dr("UnitOrder") = drResult("Unit_Order")
                    '    dr("Unit") = drResult("Unit")
                    '    dr("QtyOrder") = drResult("Qty_Order")
                    '    dr("Qty") = drResult("Qty")
                    '    dr("PriceForex") = drResult("Price_Forex")
                    '    dr("BrutoForex") = drResult("Bruto_Forex")
                    '    dr("Disc") = drResult("Disc")
                    '    dr("DiscForex") = drResult("Disc_Forex")
                    '    dr("NettoForex") = drResult("Netto_Forex")
                    '    dr("PPh") = drResult("PPh")
                    '    dr("PPhForex") = drResult("PPh_Forex")
                    '    ViewState("Dt").Rows.Add(dr)
                    'End If

                Next


                bindDataGrid()

            End If
            Session("Result") = Nothing
            ViewState("Sender") = Nothing
            Session("filter") = Nothing
            Session("Column") = Nothing
            'Session("CriteriaField") = Nothing                
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

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        Try
            DataGrid.PageIndex = 0
            DataGrid.EditIndex = -1
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
            If StrFilter <> "" Then
                StrFilter = StrFilter + " AND UserID = " + QuotedStr(ViewState("UserId").ToString)
            Else
                StrFilter = " Where UserID = " + QuotedStr(ViewState("UserId").ToString)
            End If


            SqlString = "Select * from V_SAUserSetting  " + StrFilter
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "MenuName ASC"
            End If
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim ResultField, ResultSame, Filter As String
        Dim CriteriaField As String
        Try
            Session("Result") = Nothing
            Session("Filter") = "EXEC S_GetMenuDashBoard 'Y'," + QuotedStr(ViewState("UserId").ToString)
            ResultField = "MenuID, MenuName"
            CriteriaField = "MenuID, MenuName"
            Session("Column") = ResultField.Split(",")
            Session("CriteriaField") = CriteriaField.Split(",")
            ResultSame = "PO_No"
            Session("ResultSame") = ResultSame.Split(",")
            ViewState("Sender") = "btnOut"
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
            'AttachScript("OpenSearchMultiDlg2();", Page, Me.GetType())

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

    Protected Sub DataGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGrid.RowDeleting
        Dim txtID, txtMID As Label

        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If

            txtID = DataGrid.Rows(e.RowIndex).FindControl("UserID")
            txtMID = DataGrid.Rows(e.RowIndex).FindControl("MenuID")

            SQLExecuteNonQuery("Delete from SAUserSetting where UserID = '" & txtID.Text & "' AND MenuID = '" & txtMID.Text & "'", ViewState("DBConnection").ToString)
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        Dim txt As TextBox
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()
            obj = DataGrid.Rows(e.NewEditIndex)
            txt = obj.FindControl("DbNameEdit")
            txt.Focus()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
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
End Class
