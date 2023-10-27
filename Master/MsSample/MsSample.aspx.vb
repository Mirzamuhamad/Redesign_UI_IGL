Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data

Partial Class Master_MsSample_MsSample
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
            ddlRow.SelectedValue = "10"
            'bindDataGrid()
        End If
        dsParamTrial.ConnectionString = ViewState("DBConnection")
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
            DataGrid.PageSize = ddlRow.SelectedValue
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
            SqlString = "SELECT * FROM MsSample " + StrFilter + " Order By SampleCode "
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "SampleCode ASC"
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
            Session("SelectCommand") = "S_FormPrintMaster 'MsSample','SampleCode','SampleName','Sample File','Sample Code','Sample Name'," + QuotedStr(StrFilter) + ", " + QuotedStr(ViewState("UserId"))
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
                dbCode = DataGrid.FooterRow.FindControl("SampleCodeAdd")
                dbName = DataGrid.FooterRow.FindControl("SampleNameAdd")

                If dbCode.Text.Trim.Length = 0 Then
                    lstatus.Text = MessageDlg("Sample Code must be filled.")
                    dbCode.Focus()
                    Exit Sub
                End If
                If dbName.Text.Trim.Length = 0 Then
                    lstatus.Text = MessageDlg("Sample Name must be filled.")
                    dbName.Focus()
                    Exit Sub
                End If


                If SQLExecuteScalar("SELECT SampleCode FROM VMsSample WHERE SampleCode = " + QuotedStr(dbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Sample " + QuotedStr(dbCode.Text) + " has already been exist"
                    Exit Sub
                End If

                'insert the new entry
                SQLString = "Insert into MsSample (SampleCode, SampleName,UserId, UserDate)" + _
                "Select " + QuotedStr(dbCode.Text) + ", " + QuotedStr(dbName.Text) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", GetDate()"

                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                bindDataGrid()
            ElseIf e.CommandName = "Detail" Then
                Dim lbCode, lbName As Label
                Dim gvr As GridViewRow
                gvr = DataGrid.Rows(CInt(e.CommandArgument))
                lbCode = gvr.FindControl("SampleCode")
                lbName = gvr.FindControl("SampleName")
                lbSampleCode.Text = lbCode.Text
                lbSampleName.Text = lbName.Text
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
            tempDS = SQLExecuteQuery("SELECT A.SampleCode, B.SampleName, A.CriteriaCode, A.CriteriaName, A.Standard, A.ParamTrial, P.ParamTrialName, A.UserId, A.UserDate " + _
            " FROM MsSampleCriteria A INNER JOIN VMsSample B ON A.SampleCode = B.SampleCode INNER JOIN MsParamTrial P ON A.ParamTrial = P.ParamTrialCode " + _
            " WHERE A.SampleCode =" + QuotedStr(ViewState("Nmbr").ToString), ViewState("DBConnection").ToString)

            ViewState("Dt") = tempDS.Tables(0)
            DV = tempDS.Tables(0).DefaultView

            'If DV.Count = 0 Then
            '    tempDS = SQLExecuteQuery("SELECT " + QuotedStr(ViewState("Nmbr").ToString) + " AS SampleCode, '' AS CriteriaCode, '' AS CriteriaName")
            '    DV = tempDS.Tables(0).DefaultView
            'End If

            If ViewState("SortExpressionDt") = Nothing Then
                ViewState("SortExpressionDt") = "CriteriaCode ASC"
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
            txtID = DataGrid.Rows(e.RowIndex).FindControl("SampleCode")

            SQLExecuteNonQuery("DELETE FROM MsSampleCriteria WHERE SampleCode = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
            SQLExecuteNonQuery("DELETE FROM MsSample WHERE SampleCode = '" & txtID.Text & "'", ViewState("DBConnection").ToString)
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
            txt = obj.FindControl("SampleNameEdit")
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
            lbCode = DataGrid.Rows(e.RowIndex).FindControl("SampleCodeEdit")
            dbName = DataGrid.Rows(e.RowIndex).FindControl("SampleNameEdit")
            
            If dbName.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("Sample Name must be filled.")
                dbName.Focus()
                Exit Sub
            End If

            SQLString = "UPDATE MsSample SET SampleName = " + QuotedStr(dbName.Text) + _
            "WHERE SampleCode = '" & lbCode.Text + "'"

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

    Protected Sub DataGridDt_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGridDt.PageIndexChanging
        DataGridDt.PageIndex = e.NewPageIndex
        If DataGridDt.EditIndex <> -1 Then
            DataGridDt_RowCancelingEdit(Nothing, Nothing)
        End If
        bindDataGridDt()
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
        Dim tbCriteriaCode, tbCriteriaName, tbStandard As TextBox
        Dim ddlParamTrial As DropDownList
        Dim SQLString As String
        Dim GVR As GridViewRow
        Try
            If e.CommandName = "Insert" Then
                GVR = DataGridDt.FooterRow
                tbCriteriaCode = GVR.FindControl("CriteriaCodeAdd")
                tbCriteriaName = GVR.FindControl("CriteriaNameAdd")
                tbStandard = GVR.FindControl("StandardAdd")
                ddlParamTrial = GVR.FindControl("ParamTrialAdd")

                If CekExistData(ViewState("Dt"), "CriteriaCode", tbCriteriaCode.Text) Then
                    lstatus.Text = "Sanmple " + QuotedStr(ViewState("Nmbr")) + " Criteria " + QuotedStr(tbCriteriaCode.Text) + " has been already exist"
                    Exit Sub
                End If
                If tbCriteriaCode.Text.Trim = "" Then
                    lstatus.Text = MessageDlg("Criteria Code must be filled")
                    tbCriteriaCode.Focus()
                    Exit Sub
                End If
                If tbCriteriaName.Text.Trim = "" Then
                    lstatus.Text = MessageDlg("Criteria Name must be filled")
                    tbCriteriaName.Focus()
                    Exit Sub
                End If
                If tbStandard.Text.Trim = "" Then
                    lstatus.Text = MessageDlg("Standard must be filled")
                    tbStandard.Focus()
                    Exit Sub
                End If
                If ddlParamTrial.SelectedValue = "" Then
                    lstatus.Text = MessageDlg("Param Trial must be filled")
                    ddlParamTrial.Focus()
                    Exit Sub
                End If

                SQLString = "INSERT INTO MsSampleCriteria (SampleCode, CriteriaCode, CriteriaName, Standard, ParamTrial, UserId, UserDate) " + _
                "SELECT " + QuotedStr(ViewState("Nmbr")) + "," + QuotedStr(tbCriteriaCode.Text) + "," + QuotedStr(tbCriteriaName.Text) + "," + QuotedStr(tbStandard.Text) + ", " + QuotedStr(ddlParamTrial.SelectedValue) + ", " + QuotedStr(ViewState("UserId")) + ", GetDate()"
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
            txtID = DataGridDt.Rows(e.RowIndex).FindControl("CriteriaCode")
            If txtID.Text.ToString = "" Then
                Exit Sub
            End If
            SQLExecuteNonQuery("DELETE FROM MsSampleCriteria WHERE SampleCode = " + QuotedStr(ViewState("Nmbr").ToString) + " AND CriteriaCode =" + QuotedStr(txtID.Text), ViewState("DBConnection").ToString)
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
            TxtID = DataGridDt.Rows(e.NewEditIndex).FindControl("CriteriaCode")

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
        Dim tbCriteriaName, tbStandard As TextBox
        Dim lbCriteriaCode As Label
        Dim ddlParamTrial As DropDownList
        Dim SQLString As String
        Dim GVR As GridViewRow
        Try
            GVR = DataGridDt.Rows(e.RowIndex)
            lbCriteriaCode = GVR.FindControl("CriteriaCodeEdit")
            tbCriteriaName = GVR.FindControl("CriteriaNameEdit")
            tbStandard = GVR.FindControl("StandardEdit")
            ddlParamTrial = GVR.FindControl("ParamTrialEdit")

            If tbCriteriaName.Text.Trim = "" Then
                lstatus.Text = MessageDlg("Criteria Name must be filled")
                tbCriteriaName.Focus()
                Exit Sub
            End If
            If tbStandard.Text.Trim = "" Then
                lstatus.Text = MessageDlg("Standard must be filled")
                tbStandard.Focus()
                Exit Sub
            End If
            If ddlParamTrial.SelectedValue = "" Then
                lstatus.Text = MessageDlg("Param Trial must be filled")
                ddlParamTrial.Focus()
                Exit Sub
            End If

            SQLString = "UPDATE MsSampleCriteria SET CriteriaName = " + QuotedStr(tbCriteriaName.Text) + ", Standard = " + QuotedStr(tbStandard.Text) + ", ParamTrial = " + QuotedStr(ddlParamTrial.SelectedValue) + _
            " WHERE SampleCode = " + QuotedStr(lbSampleCode.Text) + " AND CriteriaCode =" + QuotedStr(lbCriteriaCode.Text)

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

    Protected Sub ddlRow_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlRow.SelectedIndexChanged
        Try
            DataGrid.PageIndex = 0
            DataGrid.EditIndex = -1
            DataGrid.PageSize = ddlRow.SelectedValue
            bindDataGrid()
        Catch ex As Exception
            lstatus.Text = "ddlRow_SelectedIndexChanged =" + vbCrLf + ex.ToString
        End Try
    End Sub
End Class

