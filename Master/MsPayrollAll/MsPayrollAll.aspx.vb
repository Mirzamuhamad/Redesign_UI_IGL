Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data
Imports DevExpress.Web.ASPxGridView

Partial Class Assign_MsPayrollAll_MsPayrollAll
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Try
            If Not IsPostBack Then
                InitProperty()
            End If
            dsCurrency.ConnectionString = ViewState("DBConnection")
            dsFormula.ConnectionString = ViewState("DBConnection")
            
            If Not IsPostBack Then
                If Not Request.QueryString("Code") Is Nothing Then
                    'FromMasterPage()
                End If
                ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                'btnAdd.Visible = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
                'btnAddAll.Visible = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
                'btnRemove.Visible = ViewState("MenuLevel").Rows(0)("FgDelete") = "Y"
                'btnRemoveAll.Visible = ViewState("MenuLevel").Rows(0)("FgDelete") = "Y"
                btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
                FillCombo(ddlFormula, "EXEC S_GetMsFormula", False, "Formula_Code", "Formula_Name", ViewState("DBConnection"))
                FillCombo(ddlCurr, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
            End If
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnSearch" Then
                    tbCode.Text = Session("Result")(0).ToString
                    tbName.Text = Session("Result")(1).ToString
                    If tbCode.Text <> "" Then
                        btnNewSlip.Visible = True
                    Else
                        btnNewSlip.Visible = False
                    End If
                    ddlFormula.SelectedValue = Session("Result")(2).ToString
                    tbAmount.Text = FormatFloat(Session("Result")(4).ToString, ViewState("DigitHome"))
                    tbEffDate.SelectedDate = ViewState("ServerDate")
                    ddlCurr.SelectedValue = ViewState("Currency")
                    bindDataGrid()
                End If
                Session("Result") = Nothing
                ViewState("Sender") = Nothing
                Session("filter") = Nothing
                Session("Column") = Nothing
            End If
            lbstatus.Text = ""
        Catch ex As Exception
            lbstatus.Text = "page load Error : " + ex.ToString
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
        Me.tbAmount.Attributes.Add("OnKeyDown", "return PressNumeric();")
    End Sub

    Private Sub FromMasterPage()
        Dim param() As String
        Try
            btnPrint.Visible = False
            param = Request.QueryString("Code").ToString.Split("|")
            tbCode.Text = param(0)
            tbName.Text = param(1)
            tbCode.Enabled = False
            btnSearch.Visible = False

        Catch ex As Exception
            Throw New Exception("Load Assigned Code Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub ExecSPAddRemove(ByVal StrSelected As String, ByVal Flag As Integer)
        Dim SQLString As String
        Try
            SQLString = "EXEC S_MsPayrollAllAddRemove '" + tbCode.Text.Trim + "', '" + Format(tbEffDate.SelectedDate, "yyyy-MM-dd") + "', '" + ddlCurr.SelectedValue + "'," + tbAmount.Text.ToString.Replace(",", "") + ",'" + ddlFormula.SelectedValue + "','" + tbRemark.Text + "', '" + StrSelected + "', '" + ViewState("UserId").ToString + "', " + Flag.ToString
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
        Catch ex As Exception
            Throw New Exception("Execute sp Add Remove Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT * FROM V_MsPayroll Where Payroll_Pref = 'P' AND Payroll_Type IN ('GP','TT','TTT','POT') AND GroupBy = 'All'"
            ResultField = "Payroll_Code, Payroll_Name, Formula_Code, Formula_Name, Amount"
            'End If
            Session("DBConnection") = ViewState("DBConnection")
            ViewState("Sender") = "btnSearch"
            Session("Column") = ResultField.Split(",")

            If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenPopup();", True)
            End If
        Catch ex As Exception
            lbstatus.Text = "btn Search Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCode.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("SELECT * FROM V_MsPayroll WHERE Payroll_Pref = 'P' AND Payroll_Type IN ('GP','TT','TTT','POT') AND Payroll_Code = " + QuotedStr(tbCode.Text) + " AND GroupBy = 'All'", ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbCode.Text = ""
                tbName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbCode.Text = dr("Payroll_Code").ToString
                tbName.Text = dr("Payroll_Name").ToString
                ddlFormula.SelectedValue = dr("Formula_Code").ToString
                tbAmount.Text = FormatFloat(dr("Amount").ToString, ViewState("DigitHome"))
                tbEffDate.SelectedDate = ViewState("ServerDate")
                ddlCurr.SelectedValue = ViewState("Currency")
            End If
            If tbCode.Text <> "" Then
                btnNewSlip.Visible = True
            Else
                btnNewSlip.Visible = False
            End If
            bindDataGrid()
        Catch ex As Exception
            lbstatus.Text = "tb Code Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim SQLString As String
        Try
            SQLString = "EXEC S_MsPayrollAllPrint " + QuotedStr(ViewState("UserId").ToString)
            'End If
            Session("PrintType") = "Print"
            Session("DBConnection") = ViewState("DBConnection")
            Session("SelectCommand") = SQLString
            Session("ReportFile") = ".../../../Rpt/FormPayrollAllatus.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            lbstatus.Text = "Btn Print Error : " + ex.ToString
        End Try
    End Sub

    Private Sub bindDataGrid()
        Dim SqlString As String
        Try
            SqlString = "EXEC S_MsPayrollAllView " + QuotedStr(tbCode.Text)
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "StartDate DESC"
            End If
            PnlAssign.Visible = True
            pnlNewSlip.Visible = False
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)

            Dim tbdate As BasicFrame.WebControls.BasicDatePicker
            tbdate = DataGrid.FooterRow.FindControl("EffDateAdd")
            tbdate.SelectedDate = ViewState("ServerDate")

            Dim ddlCurr As DropDownList
            ddlCurr = DataGrid.FooterRow.FindControl("CurrencyAdd")
            ddlCurr.SelectedValue = ViewState("Currency")

            Dim tbAmount As TextBox
            tbAmount = DataGrid.FooterRow.FindControl("AmountAdd")
            tbAmount.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Catch ex As Exception
            lbstatus.Text = "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Protected Sub btnNewSlip_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewSlip.Click
        Try
            PnlAssign.Visible = False
            pnlNewSlip.Visible = True
            tbCode.Enabled = False
            btnSearch.Visible = False
            btnPrint.Visible = False
            btnNewSlip.Visible = False
        Catch ex As Exception
            lbstatus.Text = "btnNewSlip_Click error " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancelSlip_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelSlip.Click
        Try
            tbCode.Enabled = True
            btnSearch.Visible = True
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            pnlNewSlip.Visible = False
            PnlAssign.Visible = True
            btnNewSlip.Visible = True
            tbCode.Focus()
        Catch ex As Exception
            lbstatus.Text = "btnCancelSlip_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGrid.PageIndexChanging
        Try
            DataGrid.PageIndex = e.NewPageIndex
            If DataGrid.EditIndex <> -1 Then
                DataGrid_RowCancelingEdit(Nothing, Nothing)
            End If
            bindDataGrid()
        Catch ex As Exception
            lbstatus.Text = "DataGrid_PageIndexChanging Error : " + vbCrLf + ex.ToString
        End Try

    End Sub

    Protected Sub DataGrid_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles DataGrid.RowCancelingEdit
        Try
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid()
        Catch ex As Exception
            lbstatus.Text = "DataGrid_Cancel Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Function CheckMenuLevel(ByVal CommandName As String) As Boolean
        Try
            If CommandName = "Edit" Then
                If ViewState("MenuLevel").Rows(0)("FgEdit") = "N" Then
                    lbstatus.Text = "<script language='javascript'> {alert('You are not authorized to edit record. Please contact administrator')}</script>"
                    Return False
                    Exit Function
                End If
            End If
            If CommandName = "Delete" Then
                If ViewState("MenuLevel").Rows(0)("FgDelete") = "N" Then
                    lbstatus.Text = "<script language='javascript'> {alert('You are not authorized to delete record. Please contact administrator')}</script>"
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Check Menu Level Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub DataGrid_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGrid.RowCommand
        Dim SQLString As String
        Dim dbCurrency, dbFormula As DropDownList
        Dim tbAmount, tbRemark As TextBox
        Dim dbEffDate As BasicFrame.WebControls.BasicDatePicker
        Try
            If e.CommandName = "Insert" Then
                dbEffDate = DataGrid.FooterRow.FindControl("EffDateAdd")
                dbCurrency = DataGrid.FooterRow.FindControl("CurrencyAdd")
                dbFormula = DataGrid.FooterRow.FindControl("FormulaAdd")
                tbAmount = DataGrid.FooterRow.FindControl("AmountAdd")
                tbRemark = DataGrid.FooterRow.FindControl("RemarkAdd")

                If dbEffDate.Text.Trim.Length = 0 Then
                    lbstatus.Text = "Effective Date Must Be filled."
                    dbEffDate.Focus()
                    Exit Sub
                End If

                If dbCurrency.SelectedValue = "" Then
                    lbstatus.Text = "Currency Must Be filled."
                    dbCurrency.Focus()
                    Exit Sub
                End If
                If dbFormula.SelectedValue = "" Then
                    lbstatus.Text = "Formula Must Be filled."
                    dbCurrency.Focus()
                    Exit Sub
                End If
                If tbAmount.Text = "" Or CFloat(tbAmount.Text) = 0 Then
                    lbstatus.Text = "Amount Must Be filled."
                    tbAmount.Focus()
                    Exit Sub
                End If

                If SQLExecuteScalar("SELECT Payroll FROM MsPayrollAll WHERE Payroll = " + QuotedStr(tbCode.Text) + " AND Startdate >= '" + Format(dbEffDate.SelectedValue, "yyyy-MM-dd") + "'", ViewState("DBConnection").ToString).Length > 0 Then
                    lbstatus.Text = "Payroll " + tbCode.Text + " has already have new effective"
                    Exit Sub
                End If
                'insert the new entry
                SQLString = "INSERT INTO MsPayrollAll (Payroll, StartDate, Currency, Amount, Formula, Remark, UserId, UserDate, CanUpdate )" + _
                        "Values ( " + QuotedStr(tbCode.Text) + ", '" + Format(dbEffDate.SelectedValue, "yyyy-MM-dd") + "', " + QuotedStr(dbCurrency.SelectedValue) + ", " + tbAmount.Text.Replace(",", "") + ", " + QuotedStr(dbFormula.SelectedValue) + ", " + _
                        QuotedStr(tbRemark.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", getDate(), 'Y' )"
                SQLString = ChangeQuoteNull(SQLString)
                SQLString = SQLString.Replace("'1900-01-01'", "NULL")
                SQLString = SQLString.Replace("'0001-01-01'", "NULL")
                SQLString = SQLString.Replace("''", "NULL")
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                bindDataGrid()
            End If
        Catch ex As Exception
            lbstatus.Text = lbstatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGrid.RowDeleting
        Dim EffDate As Label
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            EffDate = DataGrid.Rows(e.RowIndex).FindControl("EffDate")
            SQLExecuteNonQuery("Delete from MsPayrollAll where Payroll = " + QuotedStr(tbCode.Text) + " AND StartDate = " + QuotedStr(EffDate.Text), ViewState("DBConnection").ToString)
            bindDataGrid()
        Catch ex As Exception
            lbstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        Dim tbDate As BasicFrame.WebControls.BasicDatePicker
        Dim tbAmount As TextBox
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            DataGrid.ShowFooter = False
            bindDataGrid()
            obj = DataGrid.Rows(e.NewEditIndex)
            tbDate = obj.FindControl("EffDateEdit")
            tbAmount = obj.FindControl("AmountEdit")
            ViewState("StartDate") = tbDate.SelectedDate
            tbAmount.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbDate.Focus()
        Catch ex As Exception
            lbstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString, IfExist As String
        Dim tbAmount, tbRemark As TextBox
        Dim ddlFormula, ddlCurr As DropDownList
        Dim tbDate As BasicFrame.WebControls.BasicDatePicker
        Dim GVR As GridViewRow
        Try
            GVR = DataGrid.Rows(e.RowIndex)
            tbDate = GVR.FindControl("EffDateEdit")
            tbAmount = GVR.FindControl("AmountEdit")
            tbRemark = GVR.FindControl("RemarkEdit")
            ddlCurr = GVR.FindControl("CurrencyEdit")
            ddlFormula = GVR.FindControl("FormulaEdit")

            If tbDate.Text.Trim.Length = 0 Then
                lbstatus.Text = MessageDlg("Effective Date must be filled")
                tbDate.Focus()
                Exit Sub
            End If
            If CFloat(tbAmount.Text.Trim) <= 0 Then
                lbstatus.Text = MessageDlg("Amount must have value")
                tbAmount.Focus()
                Exit Sub
            End If
            IfExist = SQLExecuteScalar("SELECT dbo.FormatDate(StartDate) FROM MsPayrollAll WHERE Payroll = " + QuotedStr(tbCode.Text) + " AND StartDate <> '" + Format(ViewState("StartDate"), "yyyy-MM-dd") + "' AND StartDate >= " + QuotedStr(Format(tbDate.SelectedDate, "yyyy-MM-dd")), ViewState("DBConnection").ToString)

            If IfExist.Trim.Length > 0 Then
                lbstatus.Text = MessageDlg("Payroll " + QuotedStr(tbCode.Text) + " has already have New Eff. Date " + QuotedStr(IfExist))
                tbDate.Focus()
                Exit Sub
            End If
            SQLString = "Update MsPayrollAll set StartDate = " + QuotedStr(tbDate.Text) + _
            ", Currency =" + QuotedStr(ddlCurr.SelectedValue) + ", Amount =" + tbAmount.Text.Replace(",", "") + _
            ", Formula =" + QuotedStr(ddlFormula.SelectedValue) + ", Remark =" + QuotedStr(tbRemark.Text) + _
            "  WHERE Payroll = " + QuotedStr(tbCode.Text) + " AND StartDate = '" + Format(ViewState("StartDate"), "yyyy-MM-dd") + "'"
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid()

        Catch ex As Exception
            lbstatus.Text = "DataGrid_Update Error: " & ex.ToString
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
            lbstatus.Text = "DataGrid_Sorting =" + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub btnOKSlip_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOKSlip.Click
        Dim StrSelected As String
        'Dim P As Integer
        Try
            If tbCode.Text = "" Then Exit Sub
            If ddlCurr.SelectedValue = "" Then
                lbstatus.Text = "Currency must have value"
                ddlCurr.Focus()
                Exit Sub
            End If
            If ddlFormula.SelectedValue = "" Then
                lbstatus.Text = "Formula must have value"
                ddlFormula.Focus()
                Exit Sub
            End If
            If tbAmount.Text.Trim = "" Then
                lbstatus.Text = "Amount must have value"
                tbAmount.Focus()
                Exit Sub
            End If
            If IsNumeric(tbAmount.Text.Replace(",", "")) = False Then
                lbstatus.Text = "Amount must be inputed tipe numeric"
                tbAmount.Focus()
                Exit Sub
            End If
            StrSelected = ""
            'IfExist = SQLExecuteScalar("SELECT Payroll FROM MsPayrollAll WHERE Payroll = " + QuotedStr(tbCode.Text) + " AND StartDate = " + QuotedStr(tbEffDate.SelectedDate) + StrExist, ViewState("DBConnection").ToString)
            'If IfExist.Trim.Length > 0 Then
            '    lbstatus.Text = "<script language='javascript'>alert('Payroll " + QuotedStr(tbCode.Text) + " AND Eff. Date " + QuotedStr(tbEffDate.SelectedDate) + StrExist + " has already been exist);</script>"
            '    tbEffDate.Focus()
            'End If

            'ExecSPAddRemove(StrSelected, rbEmp.SelectedValue)
            pnlNewSlip.Visible = False
            PnlAssign.Visible = True
            tbCode.Enabled = True
            btnSearch.Visible = True
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            btnNewSlip.Visible = True
            bindDataGrid()
        Catch ex As Exception
            lbstatus.Text = "btn Add Error : " + ex.ToString
        End Try
    End Sub
End Class
