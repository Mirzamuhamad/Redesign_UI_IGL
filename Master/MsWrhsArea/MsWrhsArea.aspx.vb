Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data

Partial Class Master_MsWrhsArea_MsWrhsArea
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                ViewState("SortExpression") = Nothing
                DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
                ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
                btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
                'bindDataGrid()
                ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                btnAdd.Visible = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
                btnAdd2.Visible = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"

            End If
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnCluster" Then
                    tbCluster.Text = Session("Result")(0).ToString
                    BindToText(tbClusterName, Session("Result")(1).ToString)
                End If

                Session("Result") = Nothing
                ViewState("Sender") = Nothing
            End If

        Catch ex As Exception
            lstatus.Text = "Page Load Error : " + ex.ToString
        End Try


            lstatus.Text = ""
    End Sub

    Protected Sub btnCluster_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCluster.Click
        Dim ResultField, CriteriaField As String
        Try

            Session("filter") = "SELECT ClusterCode, ClusterName, City From V_MsCluster " '" + QuotedStr(ViewState("UserId").ToString) + ", " + QuotedStr(ddlDept.SelectedValue)
            CriteriaField = "ClusterCode, ClusterName, City"
            ResultField = "ClusterCode, ClusterName, City"

            ViewState("Sender") = "btnCluster"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lstatus.Text = "btn Cluster Panen Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub tbCluster_TextChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCluster.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try
            Dt = SQLExecuteQuery("SELECT ClusterCode, ClusterName, City From V_MsCluster Where ClusterCode = '" + tbCluster.Text + "'", ViewState("DBConnection").ToString).Tables(0)

            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbCluster.Text = Dr("ClusterCode").ToString
                tbClusterName.Text = Dr("ClusterName").ToString
            Else
                tbCluster.Text = ""
                tbClusterName.Text = ""
            End If
            AttachScript("setformatdt();", Page, Me.GetType())
            tbCode.Focus()
        Catch ex As Exception
            Throw New Exception("tb TK PanenCode Error : " + ex.ToString)
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
            SqlString = "Select * from MsWrhsArea " + StrFilter
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
            Session("SelectCommand") = "S_FormPrintMaster5 'MsWrhsArea A','WrhsAreaCode','WrhsAreaName','Address1','Address2','Phone','Warehouse Area File','Warehouse Area Code','Warehouse Area Name','Address 1','Address 2','Phone'," + QuotedStr(StrFilter) + ", " + QuotedStr(ViewState("UserId"))
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster5.frx"
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
    Protected Sub DataGrid_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGrid.RowCommand
        'Dim SQLString As String
        'Dim dbCode, dbName, dbAddress1, dbAddress2, dbZipCode, dbPhone, dbFax As TextBox
        'Dim dbContactPerson, dbContactTitle, dbContactPhone, dbContactEmail As TextBox

        'Try
        '    If e.CommandName = "Insert" Then
        '        dbCode = DataGrid.FooterRow.FindControl("WrhsAreaCodeAdd")
        '        dbName = DataGrid.FooterRow.FindControl("WrhsAreaNameAdd")
        '        dbAddress1 = DataGrid.FooterRow.FindControl("Address1Add")
        '        dbAddress2 = DataGrid.FooterRow.FindControl("Address2Add")
        '        dbZipCode = DataGrid.FooterRow.FindControl("ZipCodeAdd")
        '        dbPhone = DataGrid.FooterRow.FindControl("PhoneAdd")
        '        dbFax = DataGrid.FooterRow.FindControl("FaxAdd")
        '        dbContactPerson = DataGrid.FooterRow.FindControl("ContactPersonAdd")
        '        dbContactTitle = DataGrid.FooterRow.FindControl("ContactTitleAdd")
        '        dbContactPhone = DataGrid.FooterRow.FindControl("ContactPhoneAdd")
        '        dbContactEmail = DataGrid.FooterRow.FindControl("ContactEmailAdd")

        '        If dbCode.Text.Trim.Length = 0 Then
        '            lstatus.Text = "Code must be filled."
        '            dbCode.Focus()
        '            Exit Sub
        '        End If
        '        If dbName.Text.Trim.Length = 0 Then
        '            lstatus.Text = "Name must be filled."
        '            dbName.Focus()
        '            Exit Sub
        '        End If

        '        'insert the new entry
        '        SQLString = "Insert into MsWrhsArea (WrhsAreaCode, WrhsAreaName, Address1, Address2, ZipCode, Phone, Fax, " + _
        '        " ContactPerson, ContactTitle, ContactPhone, ContactEmail, UserId, UserDate ) " + _
        '        "SELECT " + QuotedStr(dbCode.Text) + ", " + QuotedStr(dbName.Text) + ", " + QuotedStr(dbAddress1.Text) + ", " + _
        '        QuotedStr(dbAddress2.Text) + ", " + QuotedStr(dbZipCode.Text) + ", " + QuotedStr(dbPhone.Text) + ", " + _
        '        QuotedStr(dbFax.Text) + ", " + QuotedStr(dbContactPerson.Text) + ", " + QuotedStr(dbContactTitle.Text) + ", " + _
        '        QuotedStr(dbContactPhone.Text) + ", " + QuotedStr(dbContactEmail.Text) + ", " + QuotedStr(Session("userId").ToString) + ", getDate()"
        '        SQLExecuteNonQuery(SQLString)
        '        bindDataGrid()

        '    End If
        'Catch ex As Exception
        '    lstatus.Text = lstatus.Text + "InsertCommand Error:" & ex.ToString
        'End Try
    End Sub
    Protected Sub DataGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGrid.RowDeleting
        Dim txtID As Label

        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            txtID = DataGrid.Rows(e.RowIndex).FindControl("WrhsAreaCode")

            SQLExecuteNonQuery("Delete from MsWrhsArea where WrhsAreaCode = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
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

            Dim lbCode, lbName, lbAddress1, lbAddress2, lbZipCode, lbPhone, lbFax, lbCluster As Label
            Dim lbCPerson, lbCTitle, lbCPhone, lbCEmail As Label
            lbCode = GVR.FindControl("WrhsAreaCode")
            lbName = GVR.FindControl("WrhsAreaName")
            'lbCluster = GVR.FindControl("Cluster")
            lbAddress1 = GVR.FindControl("Address1")
            lbAddress2 = GVR.FindControl("Address2")
            lbZipCode = GVR.FindControl("ZipCode")
            lbPhone = GVR.FindControl("Phone")
            lbFax = GVR.FindControl("Fax")
            lbCPerson = GVR.FindControl("ContactPerson")
            lbCTitle = GVR.FindControl("ContactTitle")
            lbCPhone = GVR.FindControl("ContactPhone")
            lbCEmail = GVR.FindControl("ContactEmail")
            tbCode.Text = lbCode.Text
            tbName.Text = lbName.Text
            tbAddress1.Text = lbAddress1.Text
            tbAddress2.Text = lbAddress2.Text
            tbZipCode.Text = lbZipCode.Text
            tbPhone.Text = lbPhone.Text
            tbFax.Text = lbFax.Text
            tbCPerson.Text = lbCPerson.Text
            tbCTitle.Text = lbCTitle.Text
            tbCPhone.Text = lbCPhone.Text
            tbCEmail.Text = lbCEmail.Text
            tbName.Focus()
        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        'Dim SQLString As String
        'Dim dbName, dbAddress1, dbAddress2, dbZipCode, dbPhone, dbFax As TextBox
        'Dim dbContactPerson, dbContactTitle, dbContactPhone, dbContactEmail As TextBox
        'Dim lbCode As Label

        'Try
        '    lbCode = DataGrid.Rows(e.RowIndex).FindControl("WrhsAreaCodeEdit")
        '    dbName = DataGrid.Rows(e.RowIndex).FindControl("WrhsAreaNameEdit")
        '    dbAddress1 = DataGrid.Rows(e.RowIndex).FindControl("Address1Edit")
        '    dbAddress2 = DataGrid.Rows(e.RowIndex).FindControl("Address2Edit")
        '    dbZipCode = DataGrid.Rows(e.RowIndex).FindControl("ZipCodeEdit")
        '    dbPhone = DataGrid.Rows(e.RowIndex).FindControl("PhoneEdit")
        '    dbFax = DataGrid.Rows(e.RowIndex).FindControl("FaxEdit")
        '    dbContactPerson = DataGrid.Rows(e.RowIndex).FindControl("ContactPersonEdit")
        '    dbContactTitle = DataGrid.Rows(e.RowIndex).FindControl("ContactTitleEdit")
        '    dbContactPhone = DataGrid.Rows(e.RowIndex).FindControl("ContactPhoneEdit")
        '    dbContactEmail = DataGrid.Rows(e.RowIndex).FindControl("ContactEmailEdit")

        '    If dbName.Text.Trim.Length = 0 Then
        '        lstatus.Text = "Name must be filled."
        '        dbName.Focus()
        '        Exit Sub
        '    End If

        '    SQLString = "Update MsWrhsArea set WrhsAreaName= " + QuotedStr(dbName.Text) + ", " + _
        '    " Address1 = " + QuotedStr(dbAddress1.Text) + ", " + "Address2 = " + QuotedStr(dbAddress2.Text) + ", " + " ZipCode = " + QuotedStr(dbZipCode.Text) + ", Phone=" + QuotedStr(dbPhone.Text) + ", " + _
        '    " Fax = " + QuotedStr(dbFax.Text) + ", " + "ContactPerson = " + QuotedStr(dbContactPerson.Text) + ", " + " ContactTitle = " + QuotedStr(dbContactTitle.Text) + ", " + _
        '    " ContactPhone = " + QuotedStr(dbContactPhone.Text) + ", " + " ContactEmail = " + QuotedStr(dbContactEmail.Text) + _
        '    " where WrhsAreaCode = " & QuotedStr(lbCode.Text)
        '    SQLExecuteNonQuery(SQLString)

        '    DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
        '    DataGrid.EditIndex = -1
        '    bindDataGrid()

        'Catch ex As Exception
        '    lstatus.Text = lstatus.Text + "DataGrid_Update Error: " & ex.ToString
        'End Try
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

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Try
            If tbCode.Enabled Then
                tbCode.Text = ""
            End If
            tbName.Text = ""
            tbCluster.Text = ""
            tbClusterName.Text = ""
            tbAddress1.Text = ""
            tbAddress2.Text = ""
            tbZipCode.Text = ""
            tbPhone.Text = ""
            tbFax.Text = ""
            tbCPerson.Text = ""
            tbCTitle.Text = ""
            tbCPhone.Text = ""
            tbCEmail.Text = ""
        Catch ex As Exception
            lstatus.Text = "btn Reset Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim SQLString As String = ""
        Try
            If tbCode.Text.Trim.Length = 0 Then
                lstatus.Text = "Code must be filled."
                tbCode.Focus()
                Exit Sub
            End If
            If tbName.Text.Trim.Length = 0 Then
                lstatus.Text = "Name must be filled."
                tbName.Focus()
                Exit Sub
            End If

            If ViewState("State") = "Insert" Then
                If SQLExecuteScalar("SELECT WrhsAreaCode From MsWrhsArea WHERE WrhsAreaCode = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Warehouse Area " + QuotedStr(tbCode.Text) + " has already been exist"
                    Exit Sub
                End If

                'insert the new entry
                SQLString = "Insert into MsWrhsArea (WrhsAreaCode, WrhsAreaName, Cluster, Address1, Address2, ZipCode, Phone, Fax, " + _
                " ContactPerson, ContactTitle, ContactPhone, ContactEmail, UserId, UserDate ) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(tbName.Text) + "," + QuotedStr(tbCluster.Text) + ", " + QuotedStr(tbAddress1.Text) + ", " + _
                QuotedStr(tbAddress2.Text) + ", " + QuotedStr(tbZipCode.Text) + ", " + QuotedStr(tbPhone.Text) + ", " + _
                QuotedStr(tbFax.Text) + ", " + QuotedStr(tbCPerson.Text) + ", " + QuotedStr(tbCTitle.Text) + ", " + _
                QuotedStr(tbCPhone.Text) + ", " + QuotedStr(tbCEmail.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", getDate()"
            ElseIf ViewState("State") = "Edit" Then
                SQLString = "Update MsWrhsArea set WrhsAreaName= " + QuotedStr(tbName.Text) + ", Cluster= " + QuotedStr(tbCluster.Text) + ", " + _
                            " Address1 = " + QuotedStr(tbAddress1.Text) + ", " + "Address2 = " + QuotedStr(tbAddress2.Text) + ", " + " ZipCode = " + QuotedStr(tbZipCode.Text) + ", Phone=" + QuotedStr(tbPhone.Text) + ", " + _
                            " Fax = " + QuotedStr(tbFax.Text) + ", " + "ContactPerson = " + QuotedStr(tbCPerson.Text) + ", " + " ContactTitle = " + QuotedStr(tbCTitle.Text) + ", " + _
                            " ContactPhone = " + QuotedStr(tbCPhone.Text) + ", " + " ContactEmail = " + QuotedStr(tbCEmail.Text) + _
                            " where WrhsAreaCode = " & QuotedStr(tbCode.Text)
            End If
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            bindDataGrid()
            pnlInput.Visible = False
            PnlMain.Visible = True
        Catch ex As Exception
            lstatus.Text = "btn Save Error : " + ex.ToString
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
            tbCluster.Text = ""
            tbClusterName.Text = ""
            tbAddress1.Text = ""
            tbAddress2.Text = ""
            tbZipCode.Text = ""
            tbPhone.Text = ""
            tbFax.Text = ""
            tbCPerson.Text = ""
            tbCTitle.Text = ""
            tbCPhone.Text = ""
            tbCEmail.Text = ""
            tbCode.Focus()
        Catch ex As Exception
            lstatus.Text = "btn add Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAdd2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd2.Click
        btnAdd_Click(sender, e)
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            pnlInput.Visible = False
            PnlMain.Visible = True
        Catch ex As Exception
            lstatus.Text = "btn Cancel Error : " + ex.ToString
        End Try
    End Sub
End Class
