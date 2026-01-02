Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data

Partial Class Master_MsDelivery_MsDelivery
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        If Not IsPostBack Then
            InitProperty()
            Session("SortExpression") = Nothing
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            'btnAdd.Visible = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            'btnAdd2.Visible = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            Dim dt As DataTable
            dt = SQLExecuteQuery("Select RptTitle, MenuName, COALESCE(MenuParam,'') AS MenuParam, COALESCE(FastRpt,'') AS FastRpt, COALESCE(ReportType,'') AS ReportType, COALESCE(ReportType2,'') AS ReportType2, COALESCE(FgPrintValue,'N') AS FgPrintValue, COALESCE(FgForceNewPage,'N') AS FgForceNewPage from MsMenu WHERE MenuId = " + QuotedStr(Request.QueryString("ContainerId").ToString), ViewState("DBConnection").ToString).Tables(0)
            lblTitle.Text = dt.Rows(0)("MenuName").ToString

        End If
        If Not Session("Result") Is Nothing Then
            If ViewState("Sender") = "btnSearchWrhs" Then
                tbWrhs.Text = Session("Result")(0).ToString
                tbWrhsName.Text = Session("Result")(1).ToString
            End If
            Session("Result") = Nothing
            ViewState("Sender") = Nothing
            Session("filter") = Nothing
            Session("Criteria") = Nothing
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
            btnAdd.Visible = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnAdd2.Visible = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
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
            SqlString = "Select A.*, B.WrhsName from MsDelivery A LEFT OUTER JOIN MsWarehouse B ON A.WrhsCode = B.WrhsCode " + StrFilter
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "DeliveryCode"
                ViewState("SortOrder") = "ASC"
            End If
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub


    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim GVR As GridViewRow
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            GVR = DataGrid.Rows(e.NewEditIndex)
            ViewState("State") = "Edit"
            PnlMain.Visible = False
            pnlInput.Visible = True
            tbCode.Enabled = False
            Dim lbCode, lbName, lbWrhs, lbWrhsName, lbAddress1, lbAddress2, lbCity As Label
            Dim lbZipCode, lbPhone, lbFax, lbContactName, lbContactTitle, lbContactHp As Label
            lbCode = GVR.FindControl("DeliveryCode")
            lbName = GVR.FindControl("DeliveryName")
            lbWrhs = GVR.FindControl("WrhsCode")
            lbWrhsName = GVR.FindControl("WrhsName")
            lbAddress1 = GVR.FindControl("Address1")
            lbAddress2 = GVR.FindControl("Address2")
            lbCity = GVR.FindControl("City")
            lbZipCode = GVR.FindControl("ZipCode")
            lbPhone = GVR.FindControl("Phone")
            lbFax = GVR.FindControl("Fax")
            lbContactName = GVR.FindControl("ContactName")
            lbContactTitle = GVR.FindControl("ContactTitle")
            lbContactHp = GVR.FindControl("ContactHp")
            tbCode.Text = lbCode.Text
            tbName.Text = lbName.Text
            tbWrhs.Text = lbWrhs.Text
            tbWrhsName.Text = lbWrhsName.Text
            tbAddress1.Text = lbAddress1.Text
            tbAddress2.Text = lbAddress2.Text
            tbCity.Text = lbCity.Text
            tbZipCode.Text = lbZipCode.Text
            tbPhone.Text = lbPhone.Text
            tbFax.Text = lbFax.Text
            tbContactName.Text = lbContactName.Text
            tbContactTitle.Text = lbContactTitle.Text
            tbContactHp.Text = lbContactHp.Text
        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub


    Protected Sub DataGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGrid.RowDeleting
        Dim txtID As Label
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            txtID = DataGrid.Rows(e.RowIndex).FindControl("DeliveryCode")

            SQLExecuteNonQuery("Delete from MsDelivery where DeliveryCode = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
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
            'DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            'btnAdd.Visible = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            'btnAdd2.Visible = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
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

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("DBConnection") = ViewState("DBConnection")
            Session("PrintType") = "Print"
            Session("SelectCommand") = "EXEC S_FormPrintMasterDelivery " + QuotedStr(StrFilter) + ", " + QuotedStr(ViewState("UserId").ToString)
            Session("ReportFile") = ".../../../Rpt/RptPrintMasterDelivery.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)

        Catch ex As Exception
            lstatus.Text = "btn print Error = " + ex.ToString
        End Try

    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            pnlInput.Visible = False
            PnlMain.Visible = True
        Catch ex As Exception
            lstatus.Text = "btn Cancel Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Try
            If tbCode.Enabled Then
                tbCode.Text = ""
            End If
            tbName.Text = ""
            tbWrhs.Text = ""
            tbWrhsName.Text = ""
            tbAddress1.Text = ""
            tbAddress2.Text = ""
            tbCity.Text = ""
            tbZipCode.Text = ""
            tbPhone.Text = ""
            tbFax.Text = ""
            tbContactName.Text = ""
            tbContactTitle.Text = ""
            tbContactHp.Text = ""
        Catch ex As Exception
            lstatus.Text = "btn Reset Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim SQLString As String = ""
        Dim Validate As String = "^[a-zA-Z0-9 ]*$"
        Dim r As New Regex(Validate)
        Try
            If tbCode.Text.Trim = "" Then
                lstatus.Text = "Code Must Have Value."
                tbCode.Focus()
                Exit Sub
            End If
            If tbName.Text.Trim = "" Then
                lstatus.Text = "Name Must Have Value."
                tbName.Focus()
                Exit Sub
            End If
            If tbAddress1.Text.Trim = "" Then
                lstatus.Text = "Address 1 Must Have Value."
                tbAddress1.Focus()
                Exit Sub
            End If
            If tbAddress2.Text.Trim = "" Then
                lstatus.Text = "Address 2 Must Have Value."
                tbAddress2.Focus()
                Exit Sub
            End If
            If tbCity.Text.Trim = "" Then
                lstatus.Text = "City Must Have Value."
                tbCity.Focus()
                Exit Sub
            End If
            If tbZipCode.Text.Trim = "" Then
                lstatus.Text = "Zip Code Must Have Value."
                tbZipCode.Focus()
                Exit Sub
            End If
            If tbPhone.Text.Trim = "" Then
                lstatus.Text = "Phone No Must Have Value."
                tbPhone.Focus()
                Exit Sub
            End If
            If tbFax.Text.Trim = "" Then
                lstatus.Text = "Fax No Must Have Value."
                tbFax.Focus()
                Exit Sub
            End If
            If tbWrhsName.Text.Trim = "" Then
                lstatus.Text = "Warehouse Must Have Value."
                tbWrhs.Focus()
                Exit Sub
            End If
            If tbContactName.Text.Trim = "" Then
                lstatus.Text = "Contact Name Must Have Value."
                tbContactName.Focus()
                Exit Sub
            End If
            If tbContactTitle.Text.Trim = "" Then
                lstatus.Text = "Contact Title Must Have Value."
                tbContactTitle.Focus()
                Exit Sub
            End If
            If tbContactHp.Text.Trim = "" Then
                lstatus.Text = "Contact Hp Must Have Value."
                tbContactHp.Focus()
                Exit Sub
            End If

            If ViewState("State") = "Insert" Then

                If SQLExecuteScalar("SELECT DeliveryCode From VMsdelivery WHERE DeliveryCode = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Delivery " + QuotedStr(tbCode.Text) + " has already been exist"
                    Exit Sub
                End If

                SQLString = "Insert into MsDelivery (DeliveryCode, DeliveryName, WrhsCode, Address1, Address2, City, ZipCode, Phone," + _
                " Fax, ContactName, ContactTitle, ContactHp, UserId, UserDate ) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(tbName.Text) + ", " + QuotedStr(tbWrhs.Text) + ", " + _
                QuotedStr(tbAddress1.Text) + "," + QuotedStr(tbAddress2.Text) + "," + QuotedStr(tbCity.Text) + "," + _
                QuotedStr(tbZipCode.Text) + "," + QuotedStr(tbPhone.Text) + "," + QuotedStr(tbFax.Text) + "," + _
                QuotedStr(tbContactName.Text) + "," + QuotedStr(tbContactTitle.Text) + "," + QuotedStr(tbContactHp.Text) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", getDate()"
            ElseIf ViewState("State") = "Edit" Then
                SQLString = "Update MsDelivery set DeliveryName= " + QuotedStr(tbName.Text) + "," & _
                "WrhsCode = " + QuotedStr(tbWrhs.Text) + ", Address1 = " + QuotedStr(tbAddress1.Text) + ", " + _
                "Address2 = " + QuotedStr(tbAddress2.Text) + ", ZipCode = " + QuotedStr(tbZipCode.Text) + ", " + _
                "Phone = " + QuotedStr(tbPhone.Text) + ", Fax = " + QuotedStr(tbFax.Text) + ", " + _
                "ContactName = " + QuotedStr(tbContactName.Text) + ", ContactTitle = " + QuotedStr(tbContactTitle.Text) + ", " + _
                "ContactHp = " + QuotedStr(tbContactHp.Text) + " where DeliveryCode = " & QuotedStr(tbCode.Text)
            End If
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection"))
            bindDataGrid()
            pnlInput.Visible = False
            PnlMain.Visible = True
        Catch ex As Exception
            lstatus.Text = "btn Save Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSearchWrhs_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearchWrhs.Click
        Dim FieldResult As String
        Try
            Session("filter") = "Select * FROM VMsWarehouse"
            FieldResult = "Wrhs_Code, Wrhs_Name"
            Session("Column") = FieldResult.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            ViewState("Sender") = "btnSearchWrhs"
            If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenPopup();", True)
            End If
        Catch ex As Exception
            lstatus.Text = "btn Search Warehouse Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbWrhs_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbWrhs.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("Select WrhsCode, WrhsName From MsWarehouse WHERE WrhsCode = " + QuotedStr(tbWrhs.Text), ViewState("DBConnection"))
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbWrhs.Text = ""
                tbWrhsName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbWrhs.Text = dr("WrhsCode").ToString
                tbWrhsName.Text = dr("WrhsName").ToString
            End If

        Catch ex As Exception
            lstatus.Text = "tb Warehouse Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Try
            ViewState("State") = "Insert"
            PnlMain.Visible = False
            pnlInput.Visible = True
            tbCode.Enabled = True
            tbCode.Text = ""
            tbName.Text = ""
            tbWrhs.Text = ""
            tbWrhsName.Text = ""
            tbAddress1.Text = ""
            tbAddress2.Text = ""
            tbCity.Text = ""
            tbZipCode.Text = ""
            tbPhone.Text = ""
            tbFax.Text = ""
            tbContactName.Text = ""
            tbContactTitle.Text = ""
            tbContactHp.Text = ""
            tbCode.Focus()
        Catch ex As Exception
            lstatus.Text = "btn add Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAdd2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd2.Click
        btnAdd_Click(sender, e)
    End Sub


    'Private Sub DataGrid_Update(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid.UpdateCommand
    '    Dim SQLString As String
    '    Dim dbName, dbAddress1, dbAddress2, dbCity As TextBox
    '    Dim dbZipCode, dbPhone, dbFax, dbContactName, dbContactTitle, dbContactHp As TextBox
    '    Dim lbCode As Label
    '    Dim ddlWrhs As DropDownList

    '    Try
    '        If CheckMenuLevel("Edit") = False Then
    '            Exit Sub
    '        End If

    '        lbCode = e.Item.FindControl("DeliveryCodeEdit")
    '        dbName = e.Item.FindControl("DeliveryNameEdit")
    '        dbAddress1 = e.Item.FindControl("Address1Edit")
    '        dbAddress2 = e.Item.FindControl("Address2Edit")
    '        dbCity = e.Item.FindControl("CityEdit")
    '        dbZipCode = e.Item.FindControl("ZipCodeEdit")
    '        dbPhone = e.Item.FindControl("PhoneEdit")
    '        dbFax = e.Item.FindControl("FaxEdit")
    '        dbContactName = e.Item.FindControl("ContactNameEdit")
    '        dbContactTitle = e.Item.FindControl("ContactTitleEdit")
    '        dbContactHp = e.Item.FindControl("ContactHpEdit")
    '        ddlWrhs = e.Item.FindControl("WrhsCodeEdit")

    '        If dbName.Text.Trim.Length = 0 Then
    '            lstatus.Text = "Name must be filled."
    '            dbName.Focus()
    '            Exit Sub
    '        End If

    '        SQLString = "Update MsDelivery set DeliveryName= " + QuotedStr(dbName.Text) + "," & _
    '        "WrhsCode = " + QuotedStr(ddlWrhs.SelectedValue) + ", Address1 = " + QuotedStr(dbAddress1.Text) + ", " + _
    '        "Address2 = " + QuotedStr(dbAddress2.Text) + ", City = " + QuotedStr(dbCity.Text) + ", " + _
    '        "ZipCode = " + QuotedStr(dbZipCode.Text) + ", Phone = " + QuotedStr(dbPhone.Text) + ", " + _
    '        "Fax = " + QuotedStr(dbZipCode.Text) + ", ContactName = " + QuotedStr(dbContactName.Text) + ", " + _
    '        "ContactTitle = " + QuotedStr(dbContactTitle.Text) + ", ContactHp = " + QuotedStr(dbContactHp.Text) + _
    '        " where DeliveryCode = " & QuotedStr(lbCode.Text)

    '        SQLExecuteNonQuery(SQLString)

    '        DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
    '        DataGrid.EditIndex = -1
    '        bindDataGrid()

    '    Catch ex As Exception
    '        lstatus.Text = lstatus.Text + "DataGrid_Update Error: " & ex.ToString
    '    End Try
    'End Sub

End Class
