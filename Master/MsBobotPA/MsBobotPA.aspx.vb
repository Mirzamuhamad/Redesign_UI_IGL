Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data

Partial Class Master_MsBobotPA_MsBobotPA
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        If Not IsPostBack Then
            InitProperty()
            ViewState("SortExpression") = Nothing
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"

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
        Dim dt As New DataTable
        Dim GVR As GridViewRow
        Dim KPIQual, QualGen, QualFunc As TextBox

        Try
            StrFilter = GenerateFilter(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)

            SqlString = "SELECT CompetenceType, dbo.FormatFloat(Bobot1, 0) AS Bobot1, dbo.FormatFloat(Bobot2, 0) AS Bobot2, dbo.FormatFloat(Bobot3, 0) AS Bobot3 FROM MsBobotPA " + StrFilter + " Order By CompetenceType"
            dt = SQLExecuteQuery(SqlString, ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt") = dt

            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)

            GVR = DataGrid.FooterRow
            KPIQual = GVR.FindControl("Bobot1Add")
            KPIQual.Attributes.Add("OnKeyDown", "return PressNumeric();")

            QualGen = GVR.FindControl("Bobot2Add")
            QualGen.Attributes.Add("OnKeyDown", "return PressNumeric();")

            QualFunc = GVR.FindControl("Bobot3Add")
            QualFunc.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub
    Protected Sub DataGrid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGrid.PageIndexChanging
        DataGrid.PageIndex = e.NewPageIndex
        bindDataGrid()
    End Sub
    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        Dim txt, KPIQual, QualGen, QualFunc As TextBox
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()
            obj = DataGrid.Rows(e.NewEditIndex)
            txt = obj.FindControl("Bobot1Edit")
            txt.Focus()

            KPIQual = DataGrid.Rows(e.NewEditIndex).FindControl("Bobot1Edit")
            KPIQual.Attributes.Add("OnKeyDown", "return PressNumeric();")

            QualGen = DataGrid.Rows(e.NewEditIndex).FindControl("Bobot2Edit")
            QualGen.Attributes.Add("OnKeyDown", "return PressNumeric();")

            QualFunc = DataGrid.Rows(e.NewEditIndex).FindControl("Bobot3Edit")
            QualFunc.Attributes.Add("OnKeyDown", "return PressNumeric();")
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
    Protected Sub DataGrid_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGrid.RowCommand
        Dim SQLString As String
        Dim dbType, dbBobot1, dbBobot2, dbBobot3 As TextBox
        Try
            If e.CommandName = "Insert" Then
                dbType = DataGrid.FooterRow.FindControl("CompetenceTypeAdd")
                dbBobot1 = DataGrid.FooterRow.FindControl("Bobot1Add")
                dbBobot2 = DataGrid.FooterRow.FindControl("Bobot2Add")
                dbBobot3 = DataGrid.FooterRow.FindControl("Bobot3Add")

                If dbType.Text.Trim.Length = 0 Then
                    lstatus.Text = MessageDlg("Competence Type must be filled.")
                    dbType.Focus()
                    Exit Sub
                End If

                If Not IsNumeric(dbBobot1.Text) Then
                    lstatus.Text = "KPI Quality must in numeric format"
                    dbBobot1.Focus()
                    Exit Sub
                End If

                If CFloat(dbBobot1.Text) < 0 Then
                    lstatus.Text = MessageDlg("KPI Quality must be filled.")
                    dbBobot1.Focus()
                    Exit Sub
                End If

                If Not IsNumeric(dbBobot2.Text) Then
                    lstatus.Text = "Quality Of General Comptence must in numeric format"
                    dbBobot2.Focus()
                    Exit Sub
                End If

                If CFloat(dbBobot2.Text) < 0 Then
                    lstatus.Text = MessageDlg("Quality Of General Comptence must be filled.")
                    dbBobot2.Focus()
                    Exit Sub
                End If

                If Not IsNumeric(dbBobot3.Text) Then
                    lstatus.Text = "Quality Of Functional Comptencies must in numeric format"
                    dbBobot3.Focus()
                    Exit Sub
                End If

                If CFloat(dbBobot3.Text) < 0 Then
                    lstatus.Text = MessageDlg("Quality Of Functional Comptencies must be filled.")
                    dbBobot3.Focus()
                    Exit Sub
                End If

                If SQLExecuteScalar("SELECT CompetenceType From MsBobotPA  WHERE CompetenceType = " + QuotedStr(dbType.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Competence Type " + QuotedStr(dbType.Text) + " has already been exist"
                    Exit Sub
                End If

                'insert the new entry
                SQLString = "INSERT INTO MsBobotPA (CompetenceType, Bobot1, Bobot2, Bobot3, UserId, UserDate) " + _
                "SELECT " + QuotedStr(dbType.Text) + ", " + dbBobot1.Text.Replace(",", "") + ", " + dbBobot2.Text.Replace(",", "") + ", " + dbBobot3.Text.Replace(",", "") + ", " + QuotedStr(ViewState("UserId").ToString) + ", GetDate() "
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                bindDataGrid()
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim dbBobot1, dbBobot2, dbBobot3 As TextBox
        Dim lbType As Label
        
        Try
            lbType = DataGrid.Rows(e.RowIndex).FindControl("CompetenceTypeEdit")
            dbBobot1 = DataGrid.Rows(e.RowIndex).FindControl("Bobot1Edit")
            dbBobot2 = DataGrid.Rows(e.RowIndex).FindControl("Bobot2Edit")
            dbBobot3 = DataGrid.Rows(e.RowIndex).FindControl("Bobot3Edit")

            If Not IsNumeric(dbBobot1.Text) Then
                lstatus.Text = "KPI Quality must in numeric format"
                dbBobot1.Focus()
                Exit Sub
            End If

            If CFloat(dbBobot1.Text) < 0 Then
                lstatus.Text = MessageDlg("KPI Quality must be filled.")
                dbBobot1.Focus()
                Exit Sub
            End If

            If Not IsNumeric(dbBobot2.Text) Then
                lstatus.Text = "Quality Of General Comptence must in numeric format"
                dbBobot2.Focus()
                Exit Sub
            End If

            If CFloat(dbBobot2.Text) < 0 Then
                lstatus.Text = MessageDlg("Quality Of General Comptence must be filled.")
                dbBobot2.Focus()
                Exit Sub
            End If

            If Not IsNumeric(dbBobot3.Text) Then
                lstatus.Text = "Quality Of Functional Comptencies must in numeric format"
                dbBobot3.Focus()
                Exit Sub
            End If

            If CFloat(dbBobot3.Text) < 0 Then
                lstatus.Text = MessageDlg("Quality Of Functional Comptencies must be filled.")
                dbBobot3.Focus()
                Exit Sub
            End If

            SQLString = "UPDATE MsBobotPA SET Bobot1 = " + dbBobot1.Text.Replace(",", "") + ", " + _
            " Bobot2 = " + dbBobot2.Text.Replace(",", "") + ", Bobot3 = " + dbBobot3.Text.Replace(",", "") + _
            " WHERE CompetenceType = " + QuotedStr(lbType.Text)

            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = lstatus.Text + "DataGrid_Update Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGrid.RowDeleting
        Dim txtID As Label

        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            txtID = DataGrid.Rows(e.RowIndex).FindControl("CompetenceType")

            SQLExecuteNonQuery("DELETE FROM MsBobotPA WHERE CompetenceType = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
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
        Dim SqlString As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("PrintType") = "Print"
            SqlString = "S_FormPrintMaster4 'VMsBobotPA','CompetenceType', 'Bobot1', 'Bobot2', 'Bobot3', 'Bobot PA File','Competence Type','KPI Quality (%)', 'Quality Of General Comptence(%)', 'Quality Of Functional Comptencies(%)' ," + QuotedStr(StrFilter) + ", " + QuotedStr(ViewState("UserId"))
            Session("SelectCommand") = SqlString
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster3.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)

        Catch ex As Exception
            lstatus.Text = "btn Print Error : " + ex.ToString
        End Try
    End Sub

    Public Function GenerateFilter(ByVal Field1 As String, ByVal Field2 As String, ByVal Filter1 As String, ByVal Filter2 As String, ByVal Notasi As String) As String
        Dim StrFilter As String
        Try
            StrFilter = ""
            If Filter1.Trim.Length > 0 Then
                If Filter2.Trim.Length > 0 Then
                    StrFilter = Field1.Replace(" ", "_") + " like '%" + Filter1 + "%' " + _
                    Notasi + " " + Field2.Replace(" ", "_") + " like '%" + Filter2 + "%'"
                Else
                    StrFilter = Field1.Replace(" ", "_") + " like '%" + Filter1 + "%'"
                End If
            Else
                StrFilter = ""
            End If

            If StrFilter <> "" Then
                StrFilter = " Where (" + StrFilter + ")"
            End If
            Return StrFilter
        Catch ex As Exception
            Throw New Exception("GenerateFilterMs Error : " + ex.ToString)
        End Try
    End Function


End Class
