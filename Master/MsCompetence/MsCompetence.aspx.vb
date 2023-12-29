Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data

Partial Class Master_MsCompetence_MsCompetence
    Inherits System.Web.UI.Page
    Protected GetStringHd As String = "SELECT * From MsCompetence"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        If Not IsPostBack Then
            InitProperty()
            ViewState("SortExpression") = Nothing
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            DataGridDt.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGridDt.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            btnAdd.Visible = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnAdd2.Visible = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
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
        Dim Dt As DataTable
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            SqlString = "SELECT * FROM MsCompetence " + StrFilter + " Order By CompetenceCode "
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "CompetenceCode ASC"
                ViewState("SortOrder") = "ASC"
            End If
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)
            Dt = BindDataTransaction(SqlString, StrFilter, ViewState("DBConnection").ToString)
            btnAdd2.Visible = Dt.Rows.Count > 0
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
            Session("SelectCommand") = "S_FormPrintMaster5 'MsCompetence','CompetenceCode','CompetenceName','Description1','Description2','Type','Competence File','Competence Code','Competence Name','Description1','Description2','Type'," + QuotedStr(StrFilter) + ", " + QuotedStr(ViewState("UserId"))
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster4.frx"
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
        Dim index As Integer
        Dim DDL As DropDownList
        Dim GVR As GridViewRow = Nothing

        Try
            If Not (e.CommandName = "Sort" Or e.CommandName = "Page") Then
                index = Convert.ToInt32(e.CommandArgument)
                GVR = DataGrid.Rows(index)
                'lstatus.Text = index
                'Exit Sub
            End If

            If e.CommandName = "Go" Then
                DDL = DataGrid.Rows(index).FindControl("ddl")
                Dim lbCode, lbName As Label
                GVR = DataGrid.Rows(CInt(e.CommandArgument))
                lbCode = GVR.FindControl("CompetenceCode")
                lbName = GVR.FindControl("CompetenceName")
                If DDL.SelectedValue = "View" Then
                    pnlHd.Visible = False
                    pnlView.Visible = True
                    pnlDt.Visible = False
                    ViewState("StateHd") = "View"
                    ViewState("Nmbr") = lbCode.Text
                    FillTextBoxHd(ViewState("Nmbr"))
                    ModifyInput(False)
                    tbCode.Enabled = False
                    btnSaveHd.Visible = False
                    btnCancelHd.Visible = False
                    btnReset.Visible = False
                ElseIf DDL.SelectedValue = "Edit" Then
                    If CheckMenuLevel("Edit") = False Then
                        Exit Sub
                    End If
                    pnlHd.Visible = False
                    pnlView.Visible = True
                    pnlDt.Visible = False
                    ViewState("Nmbr") = lbCode.Text
                    FillTextBoxHd(ViewState("Nmbr"))
                    ViewState("StateHd") = "Edit"
                    ModifyInput(True)
                    bindDataGrid()
                    btnSaveHd.Visible = True
                    btnCancelHd.Visible = True
                    btnReset.Visible = False
                ElseIf DDL.SelectedValue = "Delete" Then
                    Try
                        If CheckMenuLevel("Delete") = False Then
                            Exit Sub
                        End If
                        ViewState("Nmbr") = lbCode.Text
                        SQLExecuteNonQuery("DELETE FROM MsCompetenceDt WHERE CompetenceCode = '" & ViewState("Nmbr") & "'", ViewState("DBConnection").ToString)
                        SQLExecuteNonQuery("DELETE FROM MsCompetence WHERE CompetenceCode = '" & ViewState("Nmbr") & "'", ViewState("DBConnection").ToString)
                        bindDataGrid()
                    Catch ex As Exception
                        lstatus.Text = "DDL.SelectedValue = Delete Error : " + ex.ToString
                    End Try
                ElseIf DDL.SelectedValue = "Detail" Then
                    Try
                        lbCompetenceCode.Text = lbCode.Text
                        lbCompetenceName.Text = lbName.Text
                        ViewState("Nmbr") = lbCode.Text
                        pnlHd.Visible = False
                        pnlView.Visible = False
                        pnlDt.Visible = True
                        bindDataGridDt()
                    Catch ex As Exception
                        lstatus.Text = "DDL.SelectedValue = Delete Error : " + ex.ToString
                    End Try
                End If
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub

    Private Sub bindDataGridDt()
        Dim tempDS As New DataSet()
        Dim DV As DataView
        Try
            tempDS = SQLExecuteQuery("SELECT * FROM VMsCompetenceDt " + _
            " WHERE CompetenceCode =" + QuotedStr(ViewState("Nmbr").ToString), ViewState("DBConnection").ToString)

            ViewState("Dt") = tempDS.Tables(0)
            DV = tempDS.Tables(0).DefaultView

            If ViewState("SortExpressionDt") = Nothing Then
                ViewState("SortExpressionDt") = "ItemNo ASC"
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
        Dim dbDesc1, dbDesc2 As TextBox
        Dim lbItemNo As Label
        Dim SQLString As String
        Dim GVR As GridViewRow
        Try
            If e.CommandName = "Insert" Then
                GVR = DataGridDt.FooterRow
                dbDesc1 = GVR.FindControl("Description1Add")
                dbDesc2 = GVR.FindControl("Description2Add")
                lbItemNo = GVR.FindControl("ItemNoAdd")

                If dbDesc1.Text.Trim = "" Then
                    lstatus.Text = MessageDlg("Description1 must be filled")
                    dbDesc1.Focus()
                    Exit Sub
                End If

                Dim i As Integer
                Dim dt As New DataTable
                Dim Cek As String
                Dim Row As DataRow()

                Cek = "SELECT * FROM MsCompetenceDt WHERE CompetenceCode = " + QuotedStr(ViewState("Nmbr")) + " Order By ItemNo"
                dt = SQLExecuteQuery(Cek, ViewState("DBConnection")).Tables(0)


                If dt.Rows.Count > 0 Then
                    Row = ViewState("Dt").select("")
                    i = Row.Length
                Else
                    Row = Nothing
                    i = 0
                End If

                If i > 0 Then
                    lbItemNo.Text = (CInt(Row(i - 1)("ItemNo").ToString) + 1).ToString
                Else
                    lbItemNo.Text = "1"
                End If

                If CekExistData(ViewState("Dt"), "CompetenceCode, ItemNo", ViewState("Nmbr").ToString + "|" + lbItemNo.Text) Then
                    lstatus.Text = "Competence " + QuotedStr(ViewState("Nmbr")) + " Item No" + QuotedStr(lbItemNo.Text) + " has been already exist"
                    Exit Sub
                End If

                SQLString = "INSERT INTO MsCompetenceDt (CompetenceCode, ItemNo, Description1, Description2, UserId, UserDate) " + _
                "SELECT " + QuotedStr(ViewState("Nmbr")) + "," + lbItemNo.Text + "," + QuotedStr(dbDesc1.Text) + "," + QuotedStr(dbDesc2.Text) + ", " + QuotedStr(ViewState("UserId")) + ", GetDate() "

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
            txtID = DataGridDt.Rows(e.RowIndex).FindControl("ItemNo")
            If txtID.Text.ToString = "" Then
                Exit Sub
            End If
            SQLExecuteNonQuery("DELETE FROM MsCompetenceDt WHERE CompetenceCode = " + QuotedStr(ViewState("Nmbr").ToString) + " AND ItemNo = " + txtID.Text, ViewState("DBConnection").ToString)
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
            TxtID = DataGridDt.Rows(e.NewEditIndex).FindControl("ItemNo")

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
        Dim dbDesc1, dbDesc2 As TextBox
        Dim lbItemNo As Label
        Dim SQLString As String
        Dim GVR As GridViewRow
        Try
            GVR = DataGridDt.Rows(e.RowIndex)
            lbItemNo = GVR.FindControl("ItemNoEdit")
            dbDesc1 = GVR.FindControl("Description1Edit")
            dbDesc2 = GVR.FindControl("Description2Edit")

            If dbDesc1.Text.Trim = "" Then
                lstatus.Text = MessageDlg("Description1 Value must be filled")
                dbDesc1.Focus()
                Exit Sub
            End If

            SQLString = "UPDATE MsCompetencedt SET Description1 = " + QuotedStr(dbDesc1.Text) + ", Description2 = " + QuotedStr(dbDesc2.Text) + _
            " WHERE CompetenceCode = " + QuotedStr(ViewState("Nmbr").ToString) + " AND ItemNo =" + QuotedStr(lbItemNo.Text)

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

    Private Sub ModifyInput(ByVal State As Boolean)
        Try
            If ViewState("StateHd") = "Edit" Then
                tbCode.Enabled = False
            Else
                tbCode.Enabled = True
            End If

            tbName.Enabled = State
            tbDescription1.Enabled = State
            tbDescription2.Enabled = State
            ddlType.Enabled = State
            btnSaveHd.Enabled = State
            btnCancelHd.Enabled = State
            btnReset.Enabled = State
        Catch ex As Exception
            Throw New Exception("Modify Input Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "CompetenceCode = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            tbCode.Text = Nmbr
            BindToText(tbName, Dt.Rows(0)("CompetenceName").ToString)
            BindToText(tbDescription1, Dt.Rows(0)("Description1").ToString)
            BindToText(tbDescription2, Dt.Rows(0)("Description2").ToString)
            BindToDropList(ddlType, Dt.Rows(0)("Type").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click, btnAdd2.Click
        Try
            ViewState("StateHd") = "Insert"
            ClearHd()
            pnlHd.Visible = False
            pnlView.Visible = True
            ModifyInput(True)
        Catch ex As Exception
            lstatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbCode.Text = ""
            tbName.Text = ""
            tbDescription1.Text = ""
            tbDescription2.Text = ""
            ddlType.SelectedIndex = 0
             btnSaveHd.Visible = True
            btnCancelHd.Visible = True
            btnReset.Visible = True
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Try
            pnlView.Visible = False
            pnlHd.Visible = True
        Catch ex As Exception
            lstatus.Text = "btn back Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancelHd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelHd.Click
        Try
            pnlView.Visible = False
            pnlHd.Visible = True
        Catch ex As Exception
            lstatus.Text = "btn Cancel Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Try
            If tbCode.Enabled Then
                tbCode.Text = ""
            End If
            ClearHd()
        Catch ex As Exception
            lstatus.Text = "btn Reset Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveHd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveHd.Click
        Try
            SaveHd()
        Catch ex As Exception
            lstatus.Text = "Save Hd Error : " + ex.ToString
        End Try
    End Sub

    Private Sub SaveHd()
        Dim SQLString As String
        Try

            If tbCode.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("Competence Code must be filled.")
                tbCode.Focus()
                Exit Sub
            End If
            If tbName.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("Competence Name must be filled.")
                tbName.Focus()
                Exit Sub
            End If

            If ViewState("StateHd") = "Insert" Then
                If SQLExecuteScalar("SELECT CompetenceCode FROM MsCompetence WHERE CompetenceCode = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Competence " + QuotedStr(tbCode.Text) + " has already been exist"
                    Exit Sub
                End If

                'insert the new entry
                SQLString = "INSERT INTO MsCompetence (CompetenceCode, CompetenceName, Description1, Description2, Type, UserId, UserDate)" + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(tbName.Text) + "," + QuotedStr(tbDescription1.Text) + ", " + QuotedStr(tbDescription2.Text) + ", " + QuotedStr(ddlType.SelectedValue) + ", " + _
                QuotedStr(ViewState("UserId").ToString) + ", GetDate()"

            Else
                SQLString = "UPDATE MsCompetence SET CompetenceName = " + QuotedStr(tbName.Text) + ", Description1 = " + QuotedStr(tbDescription1.Text) + ", Description2 = " + QuotedStr(tbDescription2.Text) + ", " + _
                "Type = " + QuotedStr(ddlType.SelectedValue) + " WHERE CompetenceCode = '" & tbCode.Text + "'"
            End If

            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            bindDataGrid()
            pnlView.Visible = False
            PnlHd.Visible = True
        Catch ex As Exception
            Throw New Exception("Save Hd Data Error : " + ex.ToString)
        End Try
    End Sub
End Class

