Imports System.Data

Partial Class Master_MsBedeng_MsBedeng
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
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
        End If
        If Not Session("Result") Is Nothing Then
            Dim PIC As TextBox
            Dim PICName As Label
            If ViewState("Sender") = "SearchPICAdd" Or ViewState("Sender") = "SearchPICEdit" Then
                If ViewState("Sender") = "SearchPICAdd" Then
                    PIC = DataGrid.FooterRow.FindControl("PICAdd")
                    PICName = DataGrid.FooterRow.FindControl("PICNameAdd")
                Else
                    PIC = DataGrid.Rows(DataGrid.EditIndex).FindControl("PICEdit")
                    PICName = DataGrid.Rows(DataGrid.EditIndex).FindControl("PICNameEdit")
                End If

                PIC.Text = Session("Result")(0).ToString
                PICName.Text = Session("Result")(1).ToString
                PIC.Focus()
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
        Dim MaxCap As TextBox
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            SqlString = "Select BedengCode, BedengName, MaxCap, PIC, PICName from V_MsBedengView " + StrFilter
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "BedengCode"
                ViewState("SortOrder") = "ASC"
            End If
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection"))

            GVR = DataGrid.FooterRow
            MaxCap = GVR.FindControl("MaxCapAdd")
            MaxCap.Attributes.Add("OnKeyDown", "return PressNumeric();")

        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub


    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("DBConnection") = ViewState("DBConnection")
            Session("PrintType") = "Print"
            'Session("DBConnection") = Session("DBMaster")
            Session("SelectCommand") = "S_FormPrintMaster4 'V_MsBedengView','BedengCode','BedengName','PICName','MaxCap','Bedeng File','Code','Description','PIC','MaxCap'," + QuotedStr(StrFilter) + "," + QuotedStr(ViewState("UserId"))

            Session("ReportFile") = ".../../../Rpt/RptPrintMaster3.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)

        Catch ex As Exception
            lstatus.Text = "btn Print Error : " + ex.ToString
        End Try


    End Sub

    Protected Sub DataGrid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGrid.PageIndexChanging
        DataGrid.PageIndex = e.NewPageIndex
        If DataGrid.EditIndex <> -1 Then
            DataGrid_RowCancelingEdit(Nothing, Nothing)
        End If
        bindDataGrid()
    End Sub

    Protected Sub DataGrid_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGrid.RowCommand
        Dim SQLString As String
        Dim dbCode, dbName As TextBox
        Dim cbxPIC As TextBox
        Dim dbMaxCap As TextBox
        Dim Validate As String = "^[a-zA-Z0-9 ]*$"
        Dim r As New Regex(Validate)

        Try
            If e.CommandName = "Insert" Then
                dbCode = DataGrid.FooterRow.FindControl("BedengCodeAdd")
                dbName = DataGrid.FooterRow.FindControl("BedengNameAdd")
                cbxPIC = DataGrid.FooterRow.FindControl("PICAdd")
                dbMaxCap = DataGrid.FooterRow.FindControl("MaxCapAdd")



                If dbCode.Text.Trim.Length = 0 Then
                    lstatus.Text = "Bedeng Code must be filled."
                    dbCode.Focus()
                    Exit Sub
                End If
                If dbName.Text.Trim.Length = 0 Then
                    lstatus.Text = "Bedeng Name must be filled."
                    dbName.Focus()
                    Exit Sub
                End If
                If cbxPIC.Text.Trim.Length = 0 Then
                    lstatus.Text = "PIC must be filled."
                    cbxPIC.Focus()
                    Exit Sub
                End If
                If dbMaxCap.Text.Trim.Length = 0 Then
                    lstatus.Text = "Max Capacity must be filled."
                    cbxPIC.Focus()
                    Exit Sub
                End If
                If SQLExecuteScalar("SELECT BedengCode From V_MsBedeng WHERE BedengCode = " + QuotedStr(dbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Bedeng " + QuotedStr(dbCode.Text) + " has already been exist"
                    Exit Sub
                End If

                'insert the new entry
                SQLString = "Insert into MsBedeng (BedengCode, BedengName, PIC, MaxCap, UserId, UserDate ) " + _
                "SELECT '" + dbCode.Text.Replace("'", "''") + "', '" + dbName.Text.Replace("'", "''") + "', '" + cbxPIC.Text.Replace("'", "''") + "','" + dbMaxCap.Text.Replace("'", "''") + "','" + ViewState("UserId").ToString + "', getDate()"
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                bindDataGrid()
            ElseIf e.CommandName = "SearchPICEdit" Or e.CommandName = "SearchPICAdd" Then
                Dim FieldResult As String
                Session("DBConnection") = ViewState("DBConnection")
                Session("filter") = "Select * from V_MsEmployee WHERE Fg_Active = 'Y'"
                FieldResult = "Emp_No, Emp_Name"
                Session("Column") = FieldResult.Split(",")
                If e.CommandName = "SearchPICAdd" Then
                    ViewState("Sender") = "SearchPICAdd"
                Else
                    ViewState("Sender") = "SearchPICEdit"
                End If
                If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenPopup();", True)
                End If
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGrid.RowDeleting
        Dim txtID As Label

        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            txtID = DataGrid.Rows(e.RowIndex).FindControl("BedengCode")
            SQLExecuteNonQuery("Delete from MsBedeng where BedengCode = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        Dim txt, maxCap As TextBox
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()
            obj = DataGrid.Rows(e.NewEditIndex)
            txt = obj.FindControl("BedengNameEdit")
            maxCap = obj.FindControl("MaxCapEdit")
            maxCap.Attributes.Add("OnKeyDown", "return PressNumeric();")
            txt.Focus()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim dbName, dbPIC, dbMaxCap As TextBox
        Dim lbCode As Label
        'Dim cbxAccount As TextBox
        Dim Validate As String = "^[a-zA-Z0-9 ]*$"
        Dim r As New Regex(Validate)

        Try
            lbCode = DataGrid.Rows(e.RowIndex).FindControl("BedengCodeEdit")
            dbName = DataGrid.Rows(e.RowIndex).FindControl("BedengNameEdit")
            dbPIC = DataGrid.Rows(e.RowIndex).FindControl("PICEdit")
            dbMaxCap = DataGrid.Rows(e.RowIndex).FindControl("MaxCapEdit")


            If dbName.Text.Trim.Length = 0 Then
                lstatus.Text = "Bedeng Name must be filled."
                dbName.Focus()
                Exit Sub
            End If

            If dbPIC.Text.Trim.Length = 0 Then
                lstatus.Text = "PIC must be filled."
                dbPIC.Focus()
                Exit Sub
            End If

            If dbMaxCap.Text.Length = 0 Then
                lstatus.Text = "Max Capacity must be filled."
                dbMaxCap.Focus()
                Exit Sub
            End If

      

            'If r.IsMatch(dbName.Text) = False Then
            '    lstatus.Text = "Please enter valid Characters in to Purpose Use Name"
            '    dbName.Focus()
            '    Exit Sub
            'End If


            'If r.IsMatch(cbxAccount.Text) = False Then
            '    lstatus.Text = "Please enter valid Characters in to Account"
            '    cbxAccount.Focus()
            '    Exit Sub
            'End If


            SQLString = "Update MsBedeng set BedengName= " + QuotedStr(dbName.Text) + "," & _
            "MaxCap = " + QuotedStr(dbMaxCap.Text) + _
            ",PIC = " + QuotedStr(dbPIC.Text) + _
            " where BedengCode = " & QuotedStr(lbCode.Text)
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = lstatus.Text + "DataGrid_Update Error: " & ex.ToString
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

    Protected Sub tbPIC_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dr As DataRow
        Dim ds As DataSet
        Dim Acc, tb As TextBox
        Dim AccName As Label
        Dim Count As Integer
        Dim dgi As GridViewRow
        Try
            tb = sender
            If tb.ID = "PICAdd" Then
                Count = DataGrid.Controls(0).Controls.Count
                dgi = DataGrid.Controls(0).Controls(Count - 2) '-1 for allowpaging = False
                Acc = dgi.FindControl("PICAdd")
                AccName = dgi.FindControl("PICNameAdd")
            Else
                Count = DataGrid.EditIndex
                dgi = DataGrid.Rows(Count)
                Acc = dgi.FindControl("PICEdit")
                AccName = dgi.FindControl("PICNameEdit")
            End If
            ds = SQLExecuteQuery("Select Emp_No, Emp_Name FROM V_MsEmployee WHERE Fg_Active = 'Y' AND Emp_No = " + QuotedStr(Acc.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                Acc.Text = ""
                AccName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                Acc.Text = dr("Emp_No").ToString
                AccName.Text = dr("Emp_Name").ToString
            End If
        Catch ex As Exception
            lstatus.Text = "tb PIC Changed Error : " + ex.ToString
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

End Class
