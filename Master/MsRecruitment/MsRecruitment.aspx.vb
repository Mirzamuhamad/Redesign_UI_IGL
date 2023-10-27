Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data

Partial Class Master_MsRecruitment_MsRecruitment
    Inherits System.Web.UI.Page

    Protected Sub Page_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.DataBinding

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            InitProperty()
            ViewState("SortExpression") = Nothing
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            DataGridDt.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGridDt.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
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
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            SqlString = "SELECT * FROM MsRecruitment " + StrFilter + " Order By RecruitmentCode "
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "RecruitmentCode ASC"
                ViewState("SortOrder") = "ASC"
            End If
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
            Session("SelectCommand") = "S_FormPrintMaster 'MsRecruitment','RecruitmentCode','RecruitmentName','Recruitment File','Recruitment Code','Recruitment Name'," + QuotedStr(StrFilter) + ", " + QuotedStr(ViewState("UserId"))
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)
            lstatus.Text = "<script language='javascript'> { window.open(""../../Rpt/PrintForm.Aspx"", ""List"",""scrollbars=yes, resizable=yes,width=600,height=500"");}</script>"
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
        Dim SQLString As String
        Dim dbCode, dbName As TextBox

        Try
            If e.CommandName = "Insert" Then
                dbCode = DataGrid.FooterRow.FindControl("RecruitmentCodeAdd")
                dbName = DataGrid.FooterRow.FindControl("RecruitmentNameAdd")

                If dbCode.Text.Trim.Length = 0 Then
                    lstatus.Text = MessageDlg("Recruitment Code must be filled.")
                    dbCode.Focus()
                    Exit Sub
                End If
                If dbName.Text.Trim.Length = 0 Then
                    lstatus.Text = MessageDlg("Recruitment Name must be filled.")
                    dbName.Focus()
                    Exit Sub
                End If


                If SQLExecuteScalar("SELECT RecruitmentCode FROM MsRecruitment WHERE RecruitmentCode = " + QuotedStr(dbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Recruitment " + QuotedStr(dbCode.Text) + " has already been exist"
                    Exit Sub
                End If

                'insert the new entry
                SQLString = "INSERT INTO MsRecruitment (RecruitmentCode, RecruitmentName, UserId, UserDate)" + _
                "SELECT " + QuotedStr(dbCode.Text) + ", " + QuotedStr(dbName.Text) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", GetDate()"

                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                bindDataGrid()
            ElseIf e.CommandName = "Detail" Then
                Dim lbCode, lbName As Label
                Dim gvr As GridViewRow
                gvr = DataGrid.Rows(CInt(e.CommandArgument))
                lbCode = gvr.FindControl("RecruitmentCode")
                lbName = gvr.FindControl("RecruitmentName")
                lbRecruitmentCode.Text = lbCode.Text
                lbRecruitmentName.Text = lbName.Text
                ViewState("Nmbr") = lbCode.Text
                pnlHd.Visible = False
                pnlDt.Visible = True
                bindDataGridDt()
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub

    Private Sub bindDataGridDt()
        Dim tempDS As New DataSet()
        Dim DV As DataView
        Try
            tempDS = SQLExecuteQuery("SELECT * FROM VMsRecruitmentGrade " + _
            " WHERE RecruitmentCode =" + QuotedStr(ViewState("Nmbr").ToString), ViewState("DBConnection").ToString)

            ViewState("Dt") = tempDS.Tables(0)
            DV = tempDS.Tables(0).DefaultView

            If ViewState("SortExpressionDt") = Nothing Then
                ViewState("SortExpressionDt") = "Grade ASC"
                ViewState("SortOrder") = "ASC"
            End If

            If DV.Count = 0 Then
                Dim DT As DataTable = tempDS.Tables(0)
                ShowGridViewIfEmpty(DT, DataGridDt)
                DV = DT.DefaultView
            Else
                DV.Sort = ViewState("SortExpressionDt")
                DataGridDt.DataSource = DV
                DataGridDt.DataBind()
            End If

            'DV.Sort = Session("SortExpressionDt")
            'DataGridDt.DataSource = DV
            'DataGridDt.DataBind()
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Protected Sub DataGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGrid.RowDeleting
        Dim txtID As Label

        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            txtID = DataGrid.Rows(e.RowIndex).FindControl("RecruitmentCode")

            SQLExecuteNonQuery("DELETE FROM MsRecruitmentGrade WHERE RecruitmentCode = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
            SQLExecuteNonQuery("DELETE FROM MsRecruitment WHERE RecruitmentCode = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
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
            txt = obj.FindControl("RecruitmentNameEdit")
            txt.Focus()
        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim dbName As TextBox
        Dim lbCode As Label

        Try
            lbCode = DataGrid.Rows(e.RowIndex).FindControl("RecruitmentCodeEdit")
            dbName = DataGrid.Rows(e.RowIndex).FindControl("RecruitmentNameEdit")

            If dbName.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("Recruitment Name must be filled.")
                dbName.Focus()
                Exit Sub
            End If

            SQLString = "UPDATE MsRecruitment SET RecruitmentName = " + QuotedStr(dbName.Text) + _
            "WHERE RecruitmentCode = '" & lbCode.Text + "'"

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
    Protected Sub btnBackDtTop_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBackDtTop.Click, btnBack2.Click
        Try
            pnlHd.Visible = True
            pnlDt.Visible = False
        Catch ex As Exception
            lstatus.Text = "btn Back Dt Top Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            pnlHd.Visible = True
            pnlDt.Visible = False
        Catch ex As Exception
            lstatus.Text = "btn Back Dt Bottom Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGridDt_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles DataGridDt.RowCancelingEdit
        Try
            DataGridDt.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGridDt.EditIndex = -1
            bindDataGridDt()
        Catch ex As Exception
            lstatus.Text = "DataGridDt_Cancel Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Public Sub DataGridDt_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGridDt.RowCommand
        Dim tbRangeValue As TextBox
        Dim ddlGrade, ddlFgLulus As DropDownList
        Dim SQLString As String
        Dim GVR As GridViewRow
        Try
            If e.CommandName = "Insert" Then
                GVR = DataGridDt.FooterRow
                ddlGrade = GVR.FindControl("GradeAdd")
                tbRangeValue = GVR.FindControl("RangeValueAdd")
                ddlFgLulus = GVR.FindControl("FgLulusAdd")

                If CekExistData(ViewState("Dt"), "RecruitmentCode, Grade", ViewState("Nmbr").ToString + "|" + ddlGrade.SelectedValue) Then
                    lstatus.Text = "Recruitment " + QuotedStr(ViewState("Nmbr")) + " Grade " + QuotedStr(ddlGrade.SelectedValue) + " has been already exist"
                    Exit Sub
                End If


                If ddlGrade.SelectedValue.Trim = "" Then
                    lstatus.Text = MessageDlg("Grade must be filled")
                    ddlGrade.Focus()
                    Exit Sub
                End If
                If tbRangeValue.Text.Trim = "" Then
                    lstatus.Text = MessageDlg("Range Value must be filled")
                    tbRangeValue.Focus()
                    Exit Sub
                End If
                If ddlFgLulus.SelectedValue.Trim = "" Then
                    lstatus.Text = MessageDlg("Lulus must be filled")
                    ddlFgLulus.Focus()
                    Exit Sub
                End If

                SQLString = "INSERT INTO MsRecruitmentGrade (RecruitmentCode, Grade, RangeValue, FgLulus) " + _
                "SELECT " + QuotedStr(ViewState("Nmbr")) + "," + QuotedStr(ddlGrade.SelectedValue) + "," + QuotedStr(tbRangeValue.Text) + "," + _
                QuotedStr(ddlFgLulus.SelectedValue)

                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                bindDataGridDt()
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "InsertCommand Error:" & ex.ToString
        End Try


    End Sub

    Dim Dr, Cr As Double

    Protected Sub DataGridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGridDt.RowDeleting
        Dim txtID As Label
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            txtID = DataGridDt.Rows(e.RowIndex).FindControl("Grade")
            If txtID.Text.ToString = "" Then
                Exit Sub
            End If
            SQLExecuteNonQuery("DELETE FROM MsRecruitmentGrade WHERE RecruitmentCode = " + QuotedStr(ViewState("Nmbr").ToString) + " AND Grade =" + QuotedStr(txtID.Text), ViewState("DBConnection").ToString)
            bindDataGridDt()
        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub

    Public Sub DataGridDt_RowEditing(ByVal sender As Object, ByVal e As GridViewEditEventArgs) Handles DataGridDt.RowEditing
        Dim TxtID As Label
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            TxtID = DataGridDt.Rows(e.NewEditIndex).FindControl("Grade")

            If TxtID.Text.ToString = "" Then
                Exit Sub
            End If
            DataGridDt.EditIndex = e.NewEditIndex
            DataGridDt.ShowFooter = False
            bindDataGridDt()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try

    End Sub

    Protected Sub DataGridDt_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGridDt.RowUpdating
        Dim tbRangeValue As TextBox
        Dim lbGrade As Label
        Dim ddlFgLulus As DropDownList
        Dim SQLString As String
        Dim GVR As GridViewRow
        Try
            GVR = DataGridDt.Rows(e.RowIndex)
            lbGrade = GVR.FindControl("GradeEdit")
            tbRangeValue = GVR.FindControl("RangeValueEdit")
            ddlFgLulus = GVR.FindControl("FgLulusEdit")

            If tbRangeValue.Text.Trim = "" Then
                lstatus.Text = MessageDlg("Range Value must be filled")
                tbRangeValue.Focus()
                Exit Sub
            End If

            If ddlFgLulus.SelectedValue.Trim = "" Then
                lstatus.Text = MessageDlg("Lulus must be filled")
                ddlFgLulus.Focus()
                Exit Sub
            End If

            SQLString = "UPDATE MsRecruitmentGrade SET RangeValue = " + QuotedStr(tbRangeValue.Text) + ", FgLulus = " + QuotedStr(ddlFgLulus.SelectedValue) + _
            " WHERE RecruitmentCode = " + QuotedStr(ViewState("Nmbr").ToString) + " AND Grade =" + QuotedStr(lbGrade.Text)

            SQLString = Replace(SQLString, "''", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            DataGridDt.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGridDt.EditIndex = -1
            bindDataGridDt()
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "Datagrid dt update Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGridDt_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles DataGridDt.Sorting
        Try
            If ViewState("SortOrder") = Nothing Or ViewState("SortOrder") = "DESC" Then
                ViewState("SortOrder") = "ASC"
            Else
                ViewState("SortOrder") = "DESC"
            End If
            ViewState("SortExpressionDt") = e.SortExpression + " " + ViewState("SortOrder")
            bindDataGridDt()
        Catch ex As Exception
            lstatus.Text = "UserDG_SortCommandError =" + vbCrLf + ex.ToString
        End Try
    End Sub
End Class

