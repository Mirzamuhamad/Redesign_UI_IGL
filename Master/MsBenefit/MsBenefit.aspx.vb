Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data
Partial Class MsBenefit_MsBenefit
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        If Not IsPostBack Then
            InitProperty()
            ViewState("SortExpression") = Nothing
            FillCombo(ddlFormula, "EXEC S_GetFormula", True, "FormulaCode", "FormulaName", ViewState("DBConnection"))
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            lbCurr.Text = ViewState("Currency")
            'bindDataGrid()
            tbAmount.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbAmount.Attributes.Add("OnBlur", "setformat();")
            tbPriority.Attributes.Add("OnKeyDown", "return PressNumeric();")
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


    Private Sub ClearInput()
        Try
            If tbNo.Enabled Then
                tbNo.Text = ""
            End If

            tbName.Text = ""
            ddlFormula.SelectedIndex = 0
            ddlPPh.SelectedIndex = 0
            tbAmount.Text = 0
            tbPriority.Text = 0
            ddlResponsibility.SelectedValue = "Employee"
            ddlSlip.SelectedValue = "N"
            ddlAnnual.SelectedValue = "N"

        Catch ex As Exception
            Throw New Exception("Clear Input Error : " + ex.ToString)
        End Try
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
        Dim StrFilter, SqlString, Filter As String
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            If StrFilter = "" Then
                Filter = " WHERE PayrollPref = 'B' "
            Else
                Filter = StrFilter + " AND PayrollPref = 'B' "
            End If
            SqlString = "SELECT * FROM VMsPayroll " + Filter + " ORDER BY PayrollCode "
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "PayrollCode ASC"
                ViewState("SortOrder") = "ASC"
            End If
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try

    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim StrFilter, SQLString As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            If StrFilter = "" Then
                StrFilter = " WHERE PayrollPref = 'B'"
            Else
                StrFilter = StrFilter + " AND PayrollPref = 'B'"
            End If
            SQLString = "S_FormPrintMaster5 'VMsPayroll', 'PayrollCode', 'PayrollName', 'FormulaName', 'PayrollTypeName', 'Tertanggung', 'Benefit File', 'Benefit Code', 'Benefit Name', 'Formula', 'Payroll Type', 'Responsibility By', " + QuotedStr(StrFilter) + ", " + QuotedStr(ViewState("UserId"))
            Session("SelectCommand") = SQLString
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster5.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)

        Catch ex As Exception
            lstatus.Text = "btn Print Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGrid.PageIndexChanging
        DataGrid.PageIndex = e.NewPageIndex
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
        'Dim dbCode, dbName As TextBox
        'Dim cbxWrhsGroup As DropDownList
        'Dim cbxWrhsArea As DropDownList
        'Dim cbxWrhsType As DropDownList
        'Dim cbxWrhsCondition As DropDownList
        'Dim cbxFgActive As DropDownList

        Try
            If e.CommandName = "Insert" Then
                'dbCode = DataGrid.FooterRow.FindControl("WrhsCodeAdd")
                'dbName = DataGrid.FooterRow.FindControl("WrhsNameAdd")
                'cbxWrhsGroup = DataGrid.FooterRow.FindControl("WrhsGroupAdd")
                'cbxWrhsArea = DataGrid.FooterRow.FindControl("WrhsAreaAdd")
                'cbxWrhsType = DataGrid.FooterRow.FindControl("WrhsTypeAdd")
                'cbxWrhsCondition = DataGrid.FooterRow.FindControl("WrhsConditionAdd")
                'cbxFgActive = DataGrid.FooterRow.FindControl("FgActiveAdd")

                'If dbCode.Text.Trim.Length = 0 Then
                '    lstatus.Text = " Wrhs Code must be filled."
                '    dbCode.Focus()
                '    Exit Sub
                'End If
                'If tbName.Text.Trim.Length = 0 Then
                '    lstatus.Text = " Wrhs Name must be filled."
                '    tbName.Focus()
                '    Exit Sub
                'End If

                'If SQLExecuteScalar("SELECT Wrhs_Code From VMsBenefit WHERE Wrhs_Code = " + QuotedStr(dbCode.Text), Session("DBConnection").ToString).Length > 0 Then
                '    lstatus.Text = "Warehouse " + QuotedStr(dbCode.Text) + " has already been exist"
                '    Exit Sub
                'End If

                ''insert the new entry
                'SQLString = "Insert into MsBenefit (WrhsCode, WrhsName, WrhsGroup, WrhsArea, WrhsType, WrhsCondition, FgActive, UserId, UserDate ) " + _
                '"SELECT " + QuotedStr(dbCode.Text) + ", " + QuotedStr(dbName.Text) + ", " & _
                'QuotedStr(cbxWrhsGroup.SelectedValue) + ", " + QuotedStr(cbxWrhsArea.SelectedValue) + ", " & _
                'QuotedStr(cbxWrhsType.SelectedItem.ToString) + ", " + QuotedStr(cbxWrhsCondition.SelectedValue) + ", " & _
                'QuotedStr(cbxFgActive.SelectedValue) + ", " & _
                'QuotedStr(Session("userId").ToString) + ", getDate()"
                'SQLExecuteNonQuery(SQLString)
                'bindDataGrid()
            ElseIf e.CommandName = "User" Then
                'Dim gvr As GridViewRow
                'gvr = DataGrid.Rows(CInt(e.CommandArgument))
                'If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                '    Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenAssign('Warehouse', '" + gvr.Cells(0).Text + "|" + gvr.Cells(1).Text + "','AssWrhsUser');", True)
                'End If
            ElseIf e.CommandName = "Location" Then
                'Dim gvr As GridViewRow
                'gvr = DataGrid.Rows(CInt(e.CommandArgument))
                'If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                '    Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenAssign('Warehouse', '" + gvr.Cells(0).Text + "|" + gvr.Cells(1).Text + "','AssMsBenefitLoc');", True)
                'End If
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGrid.RowDeleting
        Dim txtID As Label
        Dim GVR As GridViewRow = DataGrid.Rows(e.RowIndex)
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            txtID = DataGrid.Rows(e.RowIndex).FindControl("PayrollCode")
            'SQLExecuteNonQuery("Delete from MsBenefit where WrhsCode = '" & txtID.Text & "'")
            SQLExecuteNonQuery("Delete from MsPayroll where PayrollCode = '" & GVR.Cells(1).Text & "'", ViewState("DBConnection").ToString)
            bindDataGrid()
            'Dim dr() As DataRow
            'Dim GVR As GridViewRow = DataGrid.Rows(e.RowIndex)
            'dr = ViewState("Dt").Select("WrhsCode = " + QuotedStr(GVR.Cells(1).Text))
            'dr(0).Delete()
            'BindGridDt(ViewState("Dt"), GridDt)
            'EnableHd(GetCountRecord(ViewState("Dt")) = 0)
        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            obj = DataGrid.Rows(e.NewEditIndex)

            pnlHd.Visible = False
            pnlInput.Visible = True
            tbNo.Enabled = False

            tbNo.Visible = False
            tbNoView.Visible = True
            lblType.Visible = False

            tbNo.Text = obj.Cells(1).Text
            tbNoView.Text = obj.Cells(1).Text
            tbName.Text = obj.Cells(2).Text
            BindToDropList(ddlFormula, obj.Cells(3).Text)
            BindToDropList(ddlPPh, obj.Cells(5).Text)
            BindToText(tbAmount, obj.Cells(6).Text)
            BindToText(tbPriority, obj.Cells(7).Text)
            BindToDropList(ddlResponsibility, obj.Cells(8).Text)
            BindToDropList(ddlSlip, obj.Cells(9).Text)
            BindToDropList(ddlAnnual, obj.Cells(10).Text)
            ViewState("State") = "Edit"
        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        'Dim SQLString As String
        'Dim dbName As TextBox
        'Dim lbCode As Label
        'Dim CbxWrhsGroup As DropDownList
        'Dim CbxWrhsArea As DropDownList
        'Dim CbxWrhsType As DropDownList
        'Dim CbxFgActive As DropDownList

        'Try
        '    lbCode = DataGrid.Rows(e.RowIndex).FindControl("WrhsCodeEdit")
        '    dbName = DataGrid.Rows(e.RowIndex).FindControl("WrhsNameEdit")
        '    CbxWrhsGroup = DataGrid.Rows(e.RowIndex).FindControl("WrhsGroupEdit")
        '    CbxWrhsArea = DataGrid.Rows(e.RowIndex).FindControl("WrhsAreaEdit")
        '    CbxWrhsType = DataGrid.Rows(e.RowIndex).FindControl("WrhsTypeEdit")
        '    CbxFgActive = DataGrid.Rows(e.RowIndex).FindControl("FgActiveEdit")

        '    If dbName.Text.Trim.Length = 0 Then
        '        lstatus.Text = " Wrhs Name must be filled."
        '        dbName.Focus()
        '        Exit Sub
        '    End If

        '    SQLString = "Update MsBenefit set WrhsName= " + QuotedStr(dbName.Text) + "," & _
        '    "WrhsGroup = " + QuotedStr(CbxWrhsGroup.SelectedValue) & _
        '    "WrhsArea = " + QuotedStr(CbxWrhsArea.SelectedValue) & _
        '    "WrhsType = " + QuotedStr(CbxWrhsType.SelectedItem.ToString) & _
        '    "FgActive = " + QuotedStr(CbxFgActive.SelectedValue) & _
        '    " where WrhsCode = " & QuotedStr(lbCode.Text)

        '    SQLExecuteNonQuery(SQLString)

        '    DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
        '    DataGrid.EditIndex = -1
        '    bindDataGrid()

        'Catch ex As Exception
        '    lstatus.Text = lstatus.Text + "DataGrid_Update Error: " & ex.ToString
        'End Try
    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click, btnAdd2.Click
        Try
            pnlHd.Visible = False
            pnlInput.Visible = True
            ViewState("State") = "Insert"
            tbNo.Enabled = True
            tbNo.Visible = True
            tbNoView.Visible = False
            lblType.Visible = True
            lblType.Text = "B"
            ClearInput()
        Catch ex As Exception
            lstatus.Text = "btn Add Error : " + ex.ToString
        End Try
    End Sub
    Private Function cekInput() As Boolean
        Try
            If tbNo.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("Benefit Code cannot empty")
                tbNo.Focus()
                Return False
            End If
            If tbName.Text.Trim.Length = 0 Then
                lstatus.Text = MessageDlg("Benefit Name cannot empty")
                tbName.Focus()
                Return False
            End If
            If ddlFormula.SelectedValue.Trim = "" Then
                lstatus.Text = MessageDlg("Formula cannot empty")
                ddlFormula.Focus()
                Return False
            End If
            If (CFloat(tbPriority.Text) <= "0") Then
                lstatus.Text = MessageDlg("Priority cannot empty")
                tbPriority.Focus()
                Return False
            End If
            If (CFloat(tbAmount.Text) <= "0") Then
                lstatus.Text = MessageDlg("Amount cannot empty")
                tbAmount.Focus()
                Return False
            End If

            Return True
        Catch ex As Exception
            Throw New Exception("Cek Input Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim SqlString, PayrollCode, PayrollPref As String
        Try
            If cekInput() = False Then
                Exit Sub
            End If

            If ViewState("State") = "Insert" Then
                'CekPK()

                PayrollPref = "B"
                PayrollCode = PayrollPref + tbNo.Text
                'Sampai Sini
                If SQLExecuteScalar("SELECT PayrollNo From VMsPayroll WHERE PayrollPref = 'B' AND PayrollCode = " + QuotedStr(PayrollCode), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Benefit " + QuotedStr(tbNo.Text) + " has already been exist"
                    Exit Sub
                End If
                SqlString = "INSERT INTO MsPayroll (PayrollCode, PayrollPref, PayrollNo, PayrollName, Formula, Amount, PayrollType, Tertanggung, GroupBy, Prioritas, FgSlip, FgPPh, FgCheckAbsen, FgRapel, FgJamsostek, FgAnnual, FgValue, UserId, UserDate, FgUpdate ) " + _
                "SELECT " + QuotedStr(PayrollCode) + ", " + QuotedStr(PayrollPref) + ", " + QuotedStr(tbNo.Text) + ", " + QuotedStr(tbName.Text) + ", " + QuotedStr(ddlFormula.SelectedValue) + ", " & _
                tbAmount.Text.Replace(",", "") + ", 'TTT', " + QuotedStr(ddlResponsibility.SelectedValue) + ", 'All', " & _
                QuotedStr(tbPriority.Text) + ", " + QuotedStr(ddlSlip.SelectedValue) + ", " + QuotedStr(ddlPPh.SelectedValue) + ", 'N', 'N', " & _
                "'N' ," + QuotedStr(ddlAnnual.SelectedValue) + ", 1, " & _
                QuotedStr(ViewState("UserId").ToString) + ", GetDate(), 'Y'"
            Else
                SqlString = "UPDATE MsPayroll SET PayrollName= " + QuotedStr(tbName.Text) & _
                            ", Formula = " + QuotedStr(ddlFormula.SelectedValue) & _
                            ", Amount = " + tbAmount.Text.Replace(",", "") & _
                            ", Tertanggung = " + QuotedStr(ddlResponsibility.SelectedValue) & _
                            ", Prioritas = " + QuotedStr(tbPriority.Text) & _
                            ", FgSlip = " + QuotedStr(ddlSlip.SelectedValue) & _
                            ", FgPPH = " + QuotedStr(ddlPPh.SelectedValue) & _
                            ", FgAnnual = " + QuotedStr(ddlAnnual.SelectedValue) & _
                            " WHERE PayrollCode = " & QuotedStr(tbNo.Text)
            End If
            SQLExecuteNonQuery(SqlString, ViewState("DBConnection").ToString)
            bindDataGrid()
            pnlInput.Visible = False
            pnlHd.Visible = True
        Catch ex As Exception
            lstatus.Text = "Btn Save Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Try
            ClearInput()
            tbName.Focus()
        Catch ex As Exception
            lstatus.Text = "Btn Reset Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            pnlInput.Visible = False
            pnlHd.Visible = True
        Catch ex As Exception
            lstatus.Text = "Btn Cancel Error : " + ex.ToString
        End Try
    End Sub

    Sub CekPK()
        Dim Hasil As String
        Dim sqlstring As String
        Try
            sqlstring = ""
            sqlstring = "DECLARE @A VARCHAR(255) " + _
                        "EXEC S_PYCekPKPriority " + QuotedStr(tbNo.Text) + ", " + QuotedStr(tbPriority.Text) + ", @A OUT " + _
                        "SELECT @A"
            Hasil = SQLExecuteScalar(sqlstring, ViewState("DBConnection").ToString)
            If Hasil.Length > 1 Then
                lstatus.Text = lstatus.Text + Hasil + " <br/>"
            Else
                lstatus.Text = lstatus.Text + Hasil.Replace("0", "") + " <br/>"
            End If
        Catch ex As Exception
            lstatus.Text = "CekPK Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbPriority_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPriority.TextChanged
        Try
            'CekPK()
        Catch ex As Exception
            lstatus.Text = "tbPriority_TextChanged Error : " + ex.ToString
        End Try
    End Sub
End Class
