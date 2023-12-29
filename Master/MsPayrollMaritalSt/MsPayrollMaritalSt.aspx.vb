Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data
Imports DevExpress.Web.ASPxGridView

Partial Class Assign_MsPayrollMaritalSt_MsPayrollMaritalSt
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
            dsAvailable.ConnectionString = ViewState("DBConnection")
            dsAdjust.ConnectionString = ViewState("DBConnection")

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
                    btnAdjustSlip.Visible = btnNewSlip.Visible
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
        Me.tbAmountAdjust.Attributes.Add("OnKeyDown", "return PressNumericMinus();")
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
            SQLString = "EXEC S_MsPayrollMaritalStAddRemove '" + tbCode.Text.Trim + "', '" + Format(tbEffDate.SelectedDate, "yyyy-MM-dd") + "', '" + ddlCurr.SelectedValue + "'," + tbAmount.Text.ToString.Replace(",", "") + ",'" + ddlFormula.SelectedValue + "','" + tbRemark.Text + "', '" + StrSelected + "', '" + ViewState("UserId").ToString + "', " + Flag.ToString
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
        Catch ex As Exception
            Throw New Exception("Execute sp Add Remove Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub ExecSPAddRemoveAdjust(ByVal StrSelected As String, ByVal Flag As Integer)
        Dim SQLString As String
        Try
            SQLString = "EXEC S_MsPayrollMaritalStAddAdjust '" + tbCode.Text.Trim + "', '" + Format(tbEffAdjust.SelectedDate, "yyyy-MM-dd") + "', '" + ddlAdjust.SelectedValue + "'," + tbAmountAdjust.Text.ToString.Replace(",", "") + ",'" + tbRemarkAdjust.Text + "', '" + StrSelected + "', '" + ViewState("UserId").ToString + "', " + Flag.ToString
            'lbstatus.Text = SQLString
            'Exit Sub
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
        Catch ex As Exception
            Throw New Exception("Execute sp Add Adjust Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT * FROM V_MsPayroll Where Payroll_Pref = 'P' AND Payroll_Type IN ('GP','TT','TTT','POT') AND GroupBy = 'Marital Status'"
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
            ds = SQLExecuteQuery("SELECT * FROM V_MsPayroll WHERE Payroll_Pref = 'P' AND Payroll_Type IN ('GP','TT','TTT','POT') AND Payroll_Code = " + QuotedStr(tbCode.Text) + " AND GroupBy = 'Marital Status'", ViewState("DBConnection").ToString)
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
            btnAdjustSlip.Visible = btnNewSlip.Visible
            bindDataGrid()
        Catch ex As Exception
            lbstatus.Text = "tb Code Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim SQLString As String
        Try
            SQLString = "EXEC S_MsPayrollMaritalStPrint " + QuotedStr(ViewState("UserId").ToString)
            'End If
            Session("PrintType") = "Print"
            Session("DBConnection") = ViewState("DBConnection")
            Session("SelectCommand") = SQLString
            Session("ReportFile") = ".../../../Rpt/FormPayrollMaritalStatus.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            lbstatus.Text = "Btn Print Error : " + ex.ToString
        End Try
    End Sub

    Private Sub bindDataGrid()
        Dim SqlString As String
        Try
            SqlString = "EXEC S_MsPayrollMaritalStView " + QuotedStr(tbCode.Text)
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "MaritalStatus ASC"
            End If
            PnlAssign.Visible = True
            pnlNewSlip.Visible = False
            pnlNewAdjust.Visible = False
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection").ToString)
        Catch ex As Exception
            lbstatus.Text = "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub


    Private Sub LoadDataAvailable()
        Try
            Dim dt As New DataTable
            ViewState("Dt") = Nothing
            dt = SQLExecuteQuery("EXEC S_MsPayrollMaritalStAvailable " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Format(tbEffDate.SelectedDate, "yyyy-MM-dd")), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt") = dt
            'If tbCode.Text.Trim <> "" Then
            'AvailableGrid.Selection.UnselectAll()
            dsAvailable.SelectCommand = "EXEC S_MsPayrollMaritalStAvailable " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Format(tbEffDate.SelectedDate, "yyyy-MM-dd"))
            'Else
            'dsAvailable.SelectCommand = "Select '' as Emp_No, '' as Emp_Name, '' AS Work_Place_Name, '' AS Section_Name"
            'End If
            AvailableGrid.DataBind()
        Catch ex As Exception
            Throw New Exception("LoadDataAvailable Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub LoadDataAdjust()
        Try
            Dim dt As New DataTable
            ViewState("DtAdjust") = Nothing
            dt = SQLExecuteQuery("EXEC S_MsPayrollMaritalStAdjust " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Format(tbEffAdjust.SelectedDate, "yyyy-MM-dd")), ViewState("DBConnection").ToString).Tables(0)
            ViewState("DtAdjust") = dt
            'GridAdjust.Selection.UnselectAll()
            dsAdjust.SelectCommand = "EXEC S_MsPayrollMaritalStAdjust " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Format(tbEffAdjust.SelectedDate, "yyyy-MM-dd"))
            GridAdjust.DataBind()
        Catch ex As Exception
            Throw New Exception("LoadDataAdjust Error : " + ex.ToString)
        End Try
    End Sub

    'Protected Sub AssignedGrid_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles AssignedGrid.PageIndexChanged
    '    'New
    '    Try
    '        LoadDataAssign()
    '    Catch ex As Exception
    '        lbstatus.Text = ex.ToString
    '    End Try
    'End Sub

    'Protected Sub AssignedGrid_ProcessColumnAutoFilter(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridView.ASPxGridViewAutoFilterEventArgs) Handles AssignedGrid.ProcessColumnAutoFilter
    '    'New
    '    Try
    '        LoadDataAssign()
    '    Catch ex As Exception
    '        lbstatus.Text = ex.ToString
    '    End Try
    'End Sub

    'Protected Sub AssignedGrid_BeforeColumnSortingGrouping(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridView.ASPxGridViewBeforeColumnGroupingSortingEventArgs) Handles AssignedGrid.BeforeColumnSortingGrouping
    '    'New
    '    Try
    '        LoadDataAssign()
    '    Catch ex As Exception
    '        lbstatus.Text = ex.ToString
    '    End Try
    'End Sub


    Protected Sub AvailableGrid_BeforeColumnSortingGrouping(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridView.ASPxGridViewBeforeColumnGroupingSortingEventArgs) Handles AvailableGrid.BeforeColumnSortingGrouping
        'New
        Try
            LoadDataAvailable()
        Catch ex As Exception
            lbstatus.Text = ex.ToString
        End Try
    End Sub

    Protected Sub GridAdjust_BeforeColumnSortingGrouping(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridView.ASPxGridViewBeforeColumnGroupingSortingEventArgs) Handles GridAdjust.BeforeColumnSortingGrouping
        'New
        Try
            LoadDataAdjust()
        Catch ex As Exception
            lbstatus.Text = ex.ToString
        End Try
    End Sub

    Protected Sub AvailableGrid_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles AvailableGrid.PageIndexChanged
        'New
        Try
            LoadDataAvailable()
        Catch ex As Exception
            lbstatus.Text = ex.ToString
        End Try
    End Sub

    Protected Sub GridAdjust_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridAdjust.PageIndexChanged
        'New
        Try
            LoadDataAdjust()
        Catch ex As Exception
            lbstatus.Text = ex.ToString
        End Try
    End Sub

    Protected Sub AvailableGrid_ProcessColumnAutoFilter(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridView.ASPxGridViewAutoFilterEventArgs) Handles AvailableGrid.ProcessColumnAutoFilter
        'New
        Try
            LoadDataAvailable()
        Catch ex As Exception
            lbstatus.Text = ex.ToString
        End Try
    End Sub

    Protected Sub GridAdjust_ProcessColumnAutoFilter(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridView.ASPxGridViewAutoFilterEventArgs) Handles GridAdjust.ProcessColumnAutoFilter
        'New
        Try
            LoadDataAdjust()
        Catch ex As Exception
            lbstatus.Text = ex.ToString
        End Try
    End Sub

    Protected Sub btnNewSlip_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewSlip.Click
        Try
            PnlAssign.Visible = False
            pnlNewSlip.Visible = True
            pnlNewAdjust.Visible = False
            tbCode.Enabled = False
            btnSearch.Visible = False
            btnPrint.Visible = False
            btnNewSlip.Visible = False
            btnAdjustSlip.Visible = False
            LoadDataAvailable()
        Catch ex As Exception
            lbstatus.Text = "btnNewSlip_Click error " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancelSlip_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelSlip.Click, btnCancelAdjust.Click
        Try
            tbCode.Enabled = True
            btnSearch.Visible = True
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            pnlNewSlip.Visible = False
            pnlNewAdjust.Visible = False
            PnlAssign.Visible = True
            btnNewSlip.Visible = True
            btnAdjustSlip.Visible = True
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
            'DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
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

    Protected Sub DataGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGrid.RowDeleting
        Dim MaritalStatusCode, EffDate As Label
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            MaritalStatusCode = DataGrid.Rows(e.RowIndex).FindControl("MaritalStatus")
            EffDate = DataGrid.Rows(e.RowIndex).FindControl("EffDate")
            SQLExecuteNonQuery("Delete from MsPayrollMaritalSt where Payroll = " + QuotedStr(tbCode.Text) + " AND MaritalStatus = " + QuotedStr(MaritalStatusCode.Text) + " AND StartDate = " + QuotedStr(EffDate.Text), ViewState("DBConnection").ToString)
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
        Dim lbCode As Label
        Dim tbAmount, tbRemark As TextBox
        Dim ddlFormula, ddlCurr As DropDownList
        Dim tbDate As BasicFrame.WebControls.BasicDatePicker
        Dim GVR As GridViewRow
        Try
            GVR = DataGrid.Rows(e.RowIndex)
            lbCode = GVR.FindControl("MaritalStatusEdit")
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
            IfExist = SQLExecuteScalar("SELECT dbo.FormatDate(StartDate) FROM MsPayrollMaritalSt WHERE Payroll = " + QuotedStr(tbCode.Text) + " AND MaritalStatus = " + QuotedStr(lbCode.Text) + " AND StartDate <> '" + Format(ViewState("StartDate"), "yyyy-MM-dd") + "' AND StartDate >= " + QuotedStr(Format(tbDate.SelectedDate, "yyyy-MM-dd")), ViewState("DBConnection").ToString)

            If IfExist.Trim.Length > 0 Then
                lbstatus.Text = MessageDlg("Payroll " + QuotedStr(tbCode.Text) + " AND MaritalStatus " + QuotedStr(lbCode.Text) + " has already have New Eff. Date " + QuotedStr(IfExist))
                tbDate.Focus()
                Exit Sub
            End If
            SQLString = "Update MsPayrollMaritalSt set StartDate = " + QuotedStr(tbDate.Text) + _
            ", Currency =" + QuotedStr(ddlCurr.SelectedValue) + ", Amount =" + tbAmount.Text.Replace(",", "") + _
            ", Formula =" + QuotedStr(ddlFormula.SelectedValue) + ", Remark =" + QuotedStr(tbRemark.Text) + _
            "  WHERE Payroll = " + QuotedStr(tbCode.Text) + " AND MaritalStatus = " + QuotedStr(lbCode.Text) + " AND StartDate = '" + Format(ViewState("StartDate"), "yyyy-MM-dd") + "'"
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            'DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
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

    Function SelectedGridPay(ByRef GV As ASPxGridView, ByVal ColumnName As String) As String
        Dim Str1 As String
        Dim First As Boolean
        Dim i As Integer
        Dim Result As List(Of Object)
        Try
            Str1 = ""
            First = True

            Result = GV.GetSelectedFieldValues("MaritalStatus")

            For i = 0 To Result.Count - 1
                If First Then
                    Str1 = " AND " + ColumnName + " In(''" + Result(i).ToString + "''"
                    First = False
                Else
                    Str1 = Str1 + ",''" + Result(i).ToString + "''"
                End If
            Next

            If Str1.Length > 1 Then
                Str1 = Str1 + ")"
            End If

            If First Then
                Return ""
            Else
                Return Str1
            End If
        Catch ex As Exception
            Throw New Exception("Selected Grid Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub btnOKSlip_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOKSlip.Click
        Dim StrSelected As String
        Dim P As Integer
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
            If rbEmp.SelectedIndex = 0 Then
                If AvailableGrid.FilterEnabled = True And AvailableGrid.FilterExpression.Length > 0 Then
                    StrSelected = QuotedStr(AvailableGrid.FilterExpression)
                    P = StrSelected.Length
                    StrSelected = StrSelected.Substring(1, P - 2)
                Else
                    StrSelected = ""
                End If
                If GetCountRecord(ViewState("Dt")) <= 0 Then
                    lbstatus.Text = "Data is Empty!"
                    Exit Sub
                End If

            Else
                StrSelected = SelectedGridPay(AvailableGrid, "MaritalStatus")
                If StrSelected.Trim = "" Then
                    lbstatus.Text = "Please select the Marital Status!"
                    Exit Sub
                End If

            End If

            'IfExist = SQLExecuteScalar("SELECT Payroll FROM MsPayrollMaritalSt WHERE Payroll = " + QuotedStr(tbCode.Text) + " AND StartDate = " + QuotedStr(tbEffDate.SelectedDate) + StrExist, ViewState("DBConnection").ToString)
            'If IfExist.Trim.Length > 0 Then
            '    lbstatus.Text = "<script language='javascript'>alert('Payroll " + QuotedStr(tbCode.Text) + " AND Eff. Date " + QuotedStr(tbEffDate.SelectedDate) + StrExist + " has already been exist);</script>"
            '    tbEffDate.Focus()
            'End If

            ExecSPAddRemove(StrSelected, rbEmp.SelectedValue)
            AvailableGrid.Selection.UnselectAll()
            pnlNewSlip.Visible = False
            pnlNewAdjust.Visible = False
            PnlAssign.Visible = True
            tbCode.Enabled = True
            btnSearch.Visible = True
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            btnNewSlip.Visible = True
            btnAdjustSlip.Visible = True
            bindDataGrid()
        Catch ex As Exception
            lbstatus.Text = "btn Add Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnOKAdjust_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOKAdjust.Click
        Dim StrSelected As String
        Dim P As Integer
        Try
            If tbCode.Text = "" Then Exit Sub
            If tbAmountAdjust.Text.Trim = "" Then
                lbstatus.Text = "Amount must have value"
                tbAmount.Focus()
                Exit Sub
            End If
            If IsNumeric(tbAmountAdjust.Text.Replace(",", "")) = False Then
                lbstatus.Text = "Amount Adjust must be inputed tipe numeric"
                tbAmountAdjust.Focus()
                Exit Sub
            End If
            StrSelected = ""
            If rbEmpAdjust.SelectedIndex = 0 Then
                If GridAdjust.FilterEnabled = True And GridAdjust.FilterExpression.Length > 0 Then
                    StrSelected = QuotedStr(GridAdjust.FilterExpression)
                    P = StrSelected.Length
                    StrSelected = StrSelected.Substring(1, P - 2)
                Else
                    StrSelected = ""
                End If
                If GetCountRecord(ViewState("DtAdjust")) <= 0 Then
                    lbstatus.Text = "Data is Empty!"
                    Exit Sub
                End If

            Else
                StrSelected = SelectedGridPay(GridAdjust, "MaritalStatus")
                'StrExist = StrSelected.Replace("''", "'")
                'StrExist = StrExist.Replace("Work_Place_Code", "MaritalStatus")

                If StrSelected.Trim = "" Then
                    lbstatus.Text = "Please select the Marital Status!"
                    Exit Sub
                End If
            End If

            ExecSPAddRemoveAdjust(StrSelected, rbEmpAdjust.SelectedValue)
            GridAdjust.Selection.UnselectAll()
            pnlNewSlip.Visible = False
            pnlNewAdjust.Visible = False
            PnlAssign.Visible = True
            tbCode.Enabled = True
            btnSearch.Visible = True
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            btnNewSlip.Visible = True
            btnAdjustSlip.Visible = True
            bindDataGrid()
        Catch ex As Exception
            lbstatus.Text = "btn Add Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbEffDate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbEffDate.SelectionChanged
        Try
            LoadDataAvailable()
        Catch ex As Exception
            lbstatus.Text = "tbEffDate_SelectionChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbEffAdjust_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbEffAdjust.SelectionChanged
        Try
            LoadDataAdjust()
        Catch ex As Exception
            lbstatus.Text = "tbEffAdjust_SelectionChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAdjustSlip_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdjustSlip.Click
        Try
            PnlAssign.Visible = False
            pnlNewSlip.Visible = False
            pnlNewAdjust.Visible = True
            tbCode.Enabled = False
            btnSearch.Visible = False
            btnPrint.Visible = False
            btnNewSlip.Visible = False
            btnAdjustSlip.Visible = False
            tbEffAdjust.SelectedDate = ViewState("ServerDate")
            tbAmountAdjust.Text = "0"
            LoadDataAdjust()
        Catch ex As Exception
            lbstatus.Text = "btnNewSlip_Click error " + ex.ToString
        End Try
    End Sub
End Class
