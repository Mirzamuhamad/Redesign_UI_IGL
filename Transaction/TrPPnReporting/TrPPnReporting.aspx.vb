Imports System.Data
Imports System.IO

Partial Class Master_TrPPnReporting_TrPPnReporting
    Inherits System.Web.UI.Page

    Private Function GetStringReport(ByVal StrFilter As String) As String
        If StrFilter.Length > 0 Then
            Return "Exec S_GLPPnReportingAssign " + ddlYear.SelectedValue + "," + ddlPeriod.SelectedValue + "," + QuotedStr(StrFilter)
        Else
            Return "Exec S_GLPPnReportingAssign " + ddlYear.SelectedValue + "," + ddlPeriod.SelectedValue + ",''"
        End If
    End Function
    Private Function GetStringReport1(ByVal StrFilter As String) As String
        Return "Select InvoiceNo, InvoiceDate, SONo, CustPONo, Customer, PPnNo, PPnDate, PPnRate, Currency, ForexRate, Base, PPn, " + _
               "PPn * CASE WHEN COALESCE(PPnRate,0) =  0 THEN ForexRate ELSE PPnRate END AS PPnRp, Remark " + _
               "FROM V_FNPPnKeluaran " + _
               "WHERE YEAR(InvoiceDate) = '" + Replace(ddlYear.SelectedValue, "'", "''") + "' And Month(InvoiceDate) = '" + Replace(ddlPeriod.SelectedValue, "'", "''") + "'"
        'If StrFilter.Length > 0 Then
        '    Return "Exec S_GLPPnReportingAssign " + ddlYear.SelectedValue + "," + ddlPeriod.SelectedValue + "," + QuotedStr(StrFilter)
        'Else
        '    Return "Exec S_GLPPnReportingAssign " + ddlYear.SelectedValue + "," + ddlPeriod.SelectedValue + ",''"
        'End If
    End Function

    Private Function GetStringRemove(ByVal StrFilter As String) As String
        'Return "Select B.Supplier_Code AS UserCode, B.Supplier_Name AS UserName, A.InvoiceNo, A.InvoiceDate, A.PPNNo, A.PPNDate, A.Currency, A.PPNRate, A.Amount, (A.AmountPPN * A.FgValue) as AmountPPN " + _
        '       "from FINAPPosting A LEFT OUTER JOIN VMsSupplier B ON A.Supplier = B.Supplier_Code " + _
        '       "WHERE A.AmountPPn <> 0 And A.YearPPn = '" + Replace(ddlYear.SelectedValue, "'", "''") + "' And A.MonthPPn = '" + Replace(ddlPeriod.SelectedValue, "'", "''") + "'"
        If StrFilter.Length > 0 Then
            Return "Exec S_GLPPnReportingRemove " + ddlYear.SelectedValue + "," + ddlPeriod.SelectedValue + "," + QuotedStr(StrFilter)
        Else
            Return "Exec S_GLPPnReportingRemove " + ddlYear.SelectedValue + "," + ddlPeriod.SelectedValue + ",''"
        End If
    End Function

    Private Function GetStringNonReport(ByVal StrFilter As String) As String
        'Return "Select B.Supplier_Code AS UserCode, B.Supplier_Name AS UserName, A.InvoiceNo, A.InvoiceDate, A.PPNNo, A.PPNDate, A.Currency, A.PPNRate, A.Amount, (A.AmountPPN * A.FgValue) as AmountPPN " + _
        '       "from FINAPPosting A LEFT OUTER JOIN VMsSupplier B ON A.Supplier = B.Supplier_Code " + _
        '       "WHERE A.AmountPPn <> 0 And A.YearPPn Is NULL AND (Year(COALESCE(A.PPnDate,A.InvoiceDate)) * 12)+Month(COALESCE(A.PPnDate,A.InvoiceDate)) < " + CStr((CInt(ddlYear.SelectedValue) * 12) + CInt(ddlPeriod.SelectedValue))
        If StrFilter.Length > 0 Then
            Return "Exec S_GLPPnReportingAvailable " + ddlYear.SelectedValue + "," + ddlPeriod.SelectedValue + "," + QuotedStr(StrFilter)
        Else
            Return "Exec S_GLPPnReportingAvailable " + ddlYear.SelectedValue + "," + ddlPeriod.SelectedValue + ",''"
        End If
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        If Not IsPostBack Then
            InitProperty()
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillCombo(ddlYear, "EXEC S_GetYeaR", False, "Year", "Year", ViewState("DBConnection"))
            FillCombo(ddlPeriod, "EXEC S_GetPeriod", False, "Period", "Description", ViewState("DBConnection"))

            If ddlYear.Items.Contains(ddlYear.Items.FindByValue(ViewState("GLYear").ToString.Trim)) Then
                ddlYear.SelectedValue = ViewState("GLYear").ToString.Trim
            End If
            If ddlPeriod.Items.Contains(ddlPeriod.Items.FindByValue(ViewState("GLPeriod").ToString.Trim)) Then
                ddlPeriod.SelectedValue = ViewState("GLPeriod").ToString.Trim
            End If

            PPn()

            Menu1.Items.Item(0).Selected = True

            GridReport.PageIndex = 0
            BindDataReport()

            GridRemove.PageIndex = 0
            BindDataRemove()

            GridNonReport.PageIndex = 0
            BindDataNonReport()
        End If

        lblHome1.Text = ViewState("Currency")
        lblHome2.Text = ViewState("Currency")
        lbstatus.Text = ""
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

    Sub PPn()
        Dim Dr As DataRow
        Dim DT As DataTable

        Try
            lblMasukan.Text = "_"
            lblKeluaran.Text = "_"
            DT = SQLExecuteQuery("Exec S_GLPPnReportingInfo " + ddlYear.SelectedValue + "," + ddlPeriod.SelectedValue, ViewState("DBConnection").ToString).Tables(0)
            'lbstatus.Text = "Exec S_GLPPnReportingInfo " + ddlYear.SelectedValue + "," + ddlPeriod.SelectedValue
            'Exit Sub

            If DT.Rows.Count > 0 Then
                Dr = DT.Rows(0)
            Else
                Dr = Nothing
            End If

            If Not Dr Is Nothing Then
                lblMasukan.Text = FormatFloat(Dr("VATIn"), ViewState("DigitHome"))
                lblKeluaran.Text = FormatFloat(Dr("VATOut"), ViewState("DigitHome"))
            Else
                lblMasukan.Text = ""
                lblKeluaran.Text = ""
            End If
        Catch ex As Exception
            lbstatus.Text = "PPn Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        MultiView1.ActiveViewIndex = Int32.Parse(e.Item.Value)
        Dim i As Integer
        For i = 0 To Menu1.Items.Count - 1
            If i = e.Item.Value Then
            ElseIf Menu1.Items(1).Selected = True Then
                Dim GVR As GridViewRow = Nothing
                Try
                    If CheckMenuLevel("Insert") = False Then
                        Exit Sub
                    End If
                Catch ex As Exception
                    lbstatus.Text = "Customer Error : " + ex.ToString
                End Try
            End If
        Next
    End Sub

    Private Sub BindDataReport()
        Dim DV As DataView
        Dim StrFilter As String
        Try
            Dim dt As New DataTable
            ViewState("Report") = Nothing
            StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            If Len(StrFilter) > 10 Then
                StrFilter = " and " + StrFilter
            End If
            dt = SQLExecuteQuery(GetStringReport(StrFilter), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Report") = dt

            DV = dt.DefaultView
            DV.Sort = ViewState("SortExpressionReport")
            GridReport.DataSource = dt
            GridReport.DataBind()
            BindGridDt(dt, GridReport)

            PPn()
        Catch ex As Exception
            Throw New Exception("BindDataReport Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataReportExport()
        Dim DV As DataView
        Dim StrFilter As String

        Try
            Dim dt As New DataTable
            'ViewState("Report") = Nothing
            StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            
            If Len(StrFilter) > 10 Then
                StrFilter = " and " + StrFilter
            End If
            dt = SQLExecuteQuery(GetStringReport(StrFilter), ViewState("DBConnection").ToString).Tables(0)
            'ViewState("Report") = dt

            DV = dt.DefaultView
            'DV.Sort = ViewState("SortExpression")
            GridReportTemp.DataSource = dt
            GridReportTemp.DataBind()
            BindGridDt(dt, GridReportTemp)
        Catch ex As Exception
            Throw New Exception("BindDataReportExport Error : " + ex.ToString)
        End Try
    End Sub
    Private Sub BindDataReportExport1()
        Dim DV As DataView
        Dim StrFilter As String

        Try
            Dim dt As New DataTable
            'ViewState("Report") = Nothing
            StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)

            If Len(StrFilter) > 10 Then
                StrFilter = " and " + StrFilter
            End If
            dt = SQLExecuteQuery(GetStringReport1(StrFilter), ViewState("DBConnection").ToString).Tables(0)
            'ViewState("Report") = dt

            DV = dt.DefaultView
            'DV.Sort = ViewState("SortExpression")
            GridReportTemp1.DataSource = dt
            GridReportTemp1.DataBind()
            BindGridDt(dt, GridReportTemp1)
        Catch ex As Exception
            Throw New Exception("BindDataReportExport Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataRemove()
        Dim DV As DataView
        Dim StrFilter As String

        Try
            Dim dt As New DataTable
            ViewState("Remove") = Nothing
            StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            If Len(StrFilter) > 10 Then
                StrFilter = " and " + StrFilter
            End If
            dt = SQLExecuteQuery(GetStringRemove(StrFilter), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Remove") = dt

            DV = dt.DefaultView
            DV.Sort = ViewState("SortExpression")
            GridRemove.DataSource = dt
            GridRemove.DataBind()
            BindGridDt(dt, GridRemove)
        Catch ex As Exception
            Throw New Exception("BindDataRemove Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataNonReport()
        Dim DV As DataView
        Dim StrFilter As String

        Try
            Dim dt As New DataTable
            ViewState("NonReport") = Nothing
            StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            If Len(StrFilter) > 10 Then
                StrFilter = " and " + StrFilter
            End If
            dt = SQLExecuteQuery(GetStringNonReport(StrFilter), ViewState("DBConnection").ToString).Tables(0)
            ViewState("NonReport") = dt

            DV = dt.DefaultView
            DV.Sort = ViewState("SortExpressionNonReport")
            GridNonReport.DataSource = dt
            GridNonReport.DataBind()
            BindGridDt(dt, GridNonReport)
        Catch ex As Exception
            Throw New Exception("BindDataNonReport Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub GridReport_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridReport.PageIndexChanging
        Try
            GridReport.PageIndex = e.NewPageIndex
            GridReport.DataSource = ViewState("Report")
            GridReport.DataBind()
        Catch ex As Exception
            lbstatus.Text = "Grid Report Page Index Change Error : " + ex.ToString + " page index : " + e.NewPageIndex.ToString
        End Try
    End Sub

    Protected Sub GridReport_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridReport.RowDeleting
        If CheckMenuLevel("Delete") = False Then
            Exit Sub
        End If
        'Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)

        'SQLExecuteNonQuery("Delete from MsBuildingContact where BuildingCode = " + QuotedStr(tbBuildingCode.Text) + " AND ItemNo = " + GVR.Cells(2).Text, ViewState("DBConnection").ToString)
        'BindDataDt(tbBuildingCode.Text)
        'GridDt.DataSource = ViewState("Dt")
        'lbstatus.Text = MessageDlg("Data Deleted ")
        'EnableHd(GridDt.Rows.Count = 0)
    End Sub

    Protected Sub GridReport_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridReport.RowEditing
        'Dim GVR As GridViewRow
        'Try
        '    If CheckMenuLevel("Edit") = False Then
        '        Exit Sub
        '    End If
        '    GVR = GridDt.Rows(e.NewEditIndex)

        '    lbItemNo.Text = GVR.Cells(2).Text
        '    BindToText(tbContactName, GVR.Cells(3).Text)
        '    BindToText(tbContactTitle, GVR.Cells(4).Text)
        '    BindToText(tbPhone2, GVR.Cells(5).Text)
        '    BindToText(tbHandphone, GVR.Cells(6).Text)
        '    BindToText(tbEmail, GVR.Cells(7).Text)
        '    BindToDropList(ddlReligion, GVR.Cells(8).Text)
        '    BindToDate(tbBirthDate, GVR.Cells(9).Text)

        '    pnlEditDt.Visible = True
        '    pnlDt.Visible = False
        '    ViewState("StateDt") = "Edit"
        '    tbContactName.Focus()
        'Catch ex As Exception
        '    lbstatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        'End Try
    End Sub

    Protected Sub GridReport_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridReport.Sorting
        Try
            If ViewState("SortOrderReport") = Nothing Or ViewState("SortOrderReport") = "DESC" Then
                ViewState("SortOrderReport") = "ASC"
            Else
                ViewState("SortOrderReport") = "DESC"
            End If
            ViewState("SortExpressionReport") = e.SortExpression + " " + ViewState("SortOrderReport")
            BindDataReport()
        Catch ex As Exception
            lbstatus.Text = "GridReport_Sorting =" + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'CheckAll(GridReport, sender)
        Dim cb, cbselek As CheckBox
        Dim GRW As GridViewRow

        Try
            cb = sender
            For Each GRW In GridReport.Rows
                cbselek = GRW.FindControl("cbSelect")
                cbselek.Checked = cb.Checked
            Next
        Catch ex As Exception
            lbstatus.Text = "cbSelectHd_CheckedChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub cbSelectHd2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'CheckAll(GridNonReport, sender)
        Dim cb, cbselek As CheckBox
        Dim GRW As GridViewRow

        Try
            cb = sender
            For Each GRW In GridNonReport.Rows
                cbselek = GRW.FindControl("cbSelect2")
                cbselek.Checked = cb.Checked
            Next
        Catch ex As Exception
            lbstatus.Text = "cbSelectHd2_CheckedChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub cbSelectHd3_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'CheckAll(GridNonReport, sender)
        Dim cb, cbselek As CheckBox
        Dim GRW As GridViewRow

        Try
            cb = sender
            For Each GRW In GridRemove.Rows
                cbselek = GRW.FindControl("cbSelect3")
                cbselek.Checked = cb.Checked
            Next
        Catch ex As Exception
            lbstatus.Text = "cbSelectHd3_CheckedChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridNonReport_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridNonReport.PageIndexChanging
        Try
            GridNonReport.PageIndex = e.NewPageIndex
            GridNonReport.DataSource = ViewState("NonReport")
            GridNonReport.DataBind()
        Catch ex As Exception
            lbstatus.Text = "Grid NonReport Page Index Change Error : " + ex.ToString + " page index : " + e.NewPageIndex.ToString
        End Try
    End Sub

    Protected Sub GridNonReport_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridNonReport.Sorting
        Try
            If ViewState("SortOrderNonReport") = Nothing Or ViewState("SortOrderNonReport") = "DESC" Then
                ViewState("SortOrderNonReport") = "ASC"
            Else
                ViewState("SortOrderNonReport") = "DESC"
            End If
            ViewState("SortExpressionNonReport") = e.SortExpression + " " + ViewState("SortOrderNonReport")
            BindDataNonReport()
        Catch ex As Exception
            lbstatus.Text = "GridNonReport_Sorting =" + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Dim Result, ListSelectNmbr As String
        Dim SQLString As String

        Try
            Dim GVR As GridViewRow
            Dim CB As CheckBox
            Dim Pertamax As Boolean

            Pertamax = True
            Result = ""

            For Each GVR In GridNonReport.Rows
                CB = GVR.FindControl("cbSelect2")
                If CB.Checked Then
                    ListSelectNmbr = GVR.Cells(1).Text
                    If Pertamax Then
                        Result = "'" + ListSelectNmbr + "'"
                        Pertamax = False
                    Else
                        Result = Result + ",'" + ListSelectNmbr + "'"
                    End If
                End If
            Next

            If Result = "" Then Exit Sub

            SQLString = "EXEC S_GLPPnReportingAddRemove " + ddlYear.SelectedValue.ToString + ", " + ddlPeriod.SelectedValue.ToString + ", 1, " + QuotedStr(Result) + ", " + QuotedStr(ViewState("UserId").ToString)
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            'SQLString3 = "Update FINSIFreightDt Set YearPPn=" + ddlYear.SelectedValue + ", MonthPPn=" + ddlPeriod.SelectedValue + " Where TransNmbr+''-''+CostItem In (" + Result + ")"
            'SQLExecuteNonQuery(SQLString3, ViewState("DBConnection").ToString)

            GridReport.PageIndex = 0
            BindDataReport()

            GridRemove.PageIndex = 0
            BindDataRemove()

            GridNonReport.PageIndex = 0
            BindDataNonReport()

            PPn()
        Catch ex As Exception
            lbstatus.Text = "Add Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddAll.Click
        Dim SQLString As String

        Try
            SQLString = "EXEC S_GLPPnReportingAddRemove " + ddlYear.SelectedValue.ToString + ", " + ddlPeriod.SelectedValue.ToString + ", 0, '''', " + QuotedStr(ViewState("UserId").ToString)
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            GridReport.PageIndex = 0
            BindDataReport()

            GridRemove.PageIndex = 0
            BindDataRemove()

            GridNonReport.PageIndex = 0
            BindDataNonReport()

            PPn()
        Catch ex As Exception
            lbstatus.Text = "Add All Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnRmv_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRmv.Click
        Dim Result, ListSelectNmbr As String
        Dim SQLString As String

        Try
            Dim GVR As GridViewRow
            Dim CB As CheckBox
            Dim Pertamax As Boolean

            Pertamax = True
            Result = ""

            For Each GVR In GridNonReport.Rows
                CB = GVR.FindControl("cbSelect2")
                If CB.Checked Then
                    ListSelectNmbr = GVR.Cells(1).Text
                    If Pertamax Then
                        Result = "'" + ListSelectNmbr + "'"
                        Pertamax = False
                    Else
                        Result = Result + ",'" + ListSelectNmbr + "'"
                    End If
                End If
            Next

            If Result = "" Then Exit Sub

            SQLString = "EXEC S_GLPPnReportingCancel " + ddlYear.SelectedValue.ToString + ", " + ddlPeriod.SelectedValue.ToString + ", 1, " + QuotedStr(Result) + ", " + QuotedStr(ViewState("UserId").ToString)
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            GridReport.PageIndex = 0
            BindDataReport()

            GridRemove.PageIndex = 0
            BindDataRemove()

            GridNonReport.PageIndex = 0
            BindDataNonReport()

            PPn()
        Catch ex As Exception
            lbstatus.Text = "Add Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnRmvAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRmvAll.Click
        Dim SQLString As String

        Try
            SQLString = "EXEC S_GLPPnReportingCancel " + ddlYear.SelectedValue.ToString + ", " + ddlPeriod.SelectedValue.ToString + ", 0, '''', " + QuotedStr(ViewState("UserId").ToString)
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            GridReport.PageIndex = 0
            BindDataReport()

            GridRemove.PageIndex = 0
            BindDataRemove()

            GridNonReport.PageIndex = 0
            BindDataNonReport()

            PPn()
        Catch ex As Exception
            lbstatus.Text = "Remove All Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnRemoveCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemoveCancel.Click
        Dim Result, ListSelectNmbr As String
        Dim SQLString As String

        Try
            Dim GVR As GridViewRow
            Dim CB As CheckBox
            Dim Pertamax As Boolean

            Pertamax = True
            Result = ""

            For Each GVR In GridRemove.Rows
                CB = GVR.FindControl("cbSelect3")
                If CB.Checked Then
                    ListSelectNmbr = GVR.Cells(3).Text
                    If Pertamax Then
                        Result = "'" + ListSelectNmbr + "'"
                        Pertamax = False
                    Else
                        Result = Result + ",'" + ListSelectNmbr + "'"
                    End If
                End If
            Next

            If Result = "" Then Exit Sub

            SQLString = "EXEC S_GLPPnReportingCancel " + ddlYear.SelectedValue.ToString + ", " + ddlPeriod.SelectedValue.ToString + ", 2, " + QuotedStr(Result) + ", " + QuotedStr(ViewState("UserId").ToString)
            'lbstatus.Text = SQLString
            'Exit Sub
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            GridReport.PageIndex = 0
            BindDataReport()

            GridRemove.PageIndex = 0
            BindDataRemove()

            GridNonReport.PageIndex = 0
            BindDataNonReport()

            PPn()
        Catch ex As Exception
            lbstatus.Text = "Remove Cancel Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnRemoveCancelAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemoveCancelAll.Click
        Dim SQLString As String

        Try
            
            SQLString = "EXEC S_GLPPnReportingCancel " + ddlYear.SelectedValue.ToString + ", " + ddlPeriod.SelectedValue.ToString + ", 3, '''', " + QuotedStr(ViewState("UserId").ToString)
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            GridReport.PageIndex = 0
            BindDataReport()

            GridRemove.PageIndex = 0
            BindDataRemove()

            GridNonReport.PageIndex = 0
            BindDataNonReport()

            PPn()
        Catch ex As Exception
            lbstatus.Text = "Remove Cancel All Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        Dim Result, ListSelectNmbr As String
        Dim SQLString As String

        Try
            Dim GVR As GridViewRow
            Dim CB As CheckBox
            Dim Pertamax As Boolean

            Pertamax = True
            Result = ""

            For Each GVR In GridReport.Rows
                CB = GVR.FindControl("cbSelect")
                If CB.Checked Then
                    ListSelectNmbr = GVR.Cells(1).Text
                    If Pertamax Then
                        Result = "'" + ListSelectNmbr + "'"
                        Pertamax = False
                    Else
                        Result = Result + ",'" + ListSelectNmbr + "'"
                    End If
                End If
            Next

            If Result = "" Then Exit Sub

            SQLString = "EXEC S_GLPPnReportingAddRemove " + ddlYear.SelectedValue.ToString + ", " + ddlPeriod.SelectedValue.ToString + ", 2, " + QuotedStr(Result) + ", " + QuotedStr(ViewState("UserId").ToString)
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            GridReport.PageIndex = 0
            BindDataReport()

            GridRemove.PageIndex = 0
            BindDataRemove()

            GridNonReport.PageIndex = 0
            BindDataNonReport()

            PPn()
        Catch ex As Exception
            lbstatus.Text = "Remove Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnRemoveAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemoveAll.Click
        Dim SQLString As String

        Try
            SQLString = "EXEC S_GLPPnReportingAddRemove " + ddlYear.SelectedValue.ToString + ", " + ddlPeriod.SelectedValue.ToString + ", 3, '''', " + QuotedStr(ViewState("UserId").ToString)
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            GridReport.PageIndex = 0
            BindDataReport()

            GridRemove.PageIndex = 0
            BindDataRemove()

            GridNonReport.PageIndex = 0
            BindDataNonReport()

            PPn()
        Catch ex As Exception
            lbstatus.Text = "Remove All Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlYear_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlYear.SelectedIndexChanged, ddlPeriod.SelectedIndexChanged
        Try
            GridReport.PageIndex = 0
            BindDataReport()
            GridNonReport.PageIndex = 0
            BindDataNonReport()
        Catch ex As Exception
            lbstatus.Text = "Period SelectedIndexChange Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnPeriod_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPeriod.Click
        ddlYear_SelectedIndexChanged(Nothing, Nothing)
    End Sub

    Protected Sub ddlShowRecord_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlShowRecord.SelectedIndexChanged
        GridReport.PageIndex = 0
        GridReport.EditIndex = -1
        GridReport.PageSize = ddlShowRecord.SelectedValue
        BindDataReport()
    End Sub

    Protected Sub ddlShowRecord2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlShowRecord2.SelectedIndexChanged
        GridNonReport.PageIndex = 0
        GridNonReport.EditIndex = -1
        GridNonReport.PageSize = ddlShowRecord2.SelectedValue
        BindDataNonReport()
    End Sub

    Protected Sub ddlShowRecord3_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlShowRecord3.SelectedIndexChanged
        GridRemove.PageIndex = 0
        GridRemove.EditIndex = -1
        GridRemove.PageSize = ddlShowRecord3.SelectedValue
        BindDataRemove()
    End Sub

    Protected Sub btnImport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImport.Click
        Try
            'GridReportTemp.PageIndex = 0
            GridReportTemp.Visible = True
            BindDataReportExport()
            ExportGridToExcel()
            GridReportTemp.Visible = False

            'GridReport.PageIndex = 0
            'BindDataReport()
            'ExportGridToExcel()
        Catch ex As Exception
            lbstatus.Text = "btnImport_Click =" + vbCrLf + ex.ToString
        End Try
    End Sub

    Public Sub ExportGridToExcel()
        Dim form As HtmlForm = New HtmlForm()
        Dim worksheetname, StrWorkname As String

        Try
            StrWorkname = Trim("PPn Reporting")

            worksheetname = Left(StrWorkname, 31)

            Dim attachment As String '= "attachment; filename=PrintDetails.xls"
            attachment = "attachment; filename=" + worksheetname + ".xls"

            'If CekDt() = False Then
            '    Exit Sub
            'End If

            Response.ClearContent()
            Response.AddHeader("content-disposition", attachment)
            Response.ContentType = "application/ms-excel"
            'namespace (using system.IO)      

            Dim stw As StringWriter = New StringWriter()
            Dim htextw As HtmlTextWriter = New HtmlTextWriter(stw)

            GridReportTemp.Parent.Controls.Add(form)
            form.Attributes("runat") = "server"
            form.Controls.Add(GridReportTemp)

            Me.Controls.Add(form)

            form.RenderControl(htextw)
            Response.Write(stw.ToString())
            Response.End()
        Catch ex As Exception
            lbstatus.Text = "ExportGridToExcel =" + vbCrLf + ex.ToString
        End Try
    End Sub

    Public Sub ExportGridToExcel1()
        Dim form As HtmlForm = New HtmlForm()
        Dim worksheetname, StrWorkname As String

        Try
            StrWorkname = Trim("PPn Reporting")

            worksheetname = Left(StrWorkname, 31)

            Dim attachment As String '= "attachment; filename=PrintDetails.xls"
            attachment = "attachment; filename=" + worksheetname + ".xls"

            'If CekDt() = False Then
            '    Exit Sub
            'End If

            Response.ClearContent()
            Response.AddHeader("content-disposition", attachment)
            Response.ContentType = "application/ms-excel"
            'namespace (using system.IO)      

            Dim stw As StringWriter = New StringWriter()
            Dim htextw As HtmlTextWriter = New HtmlTextWriter(stw)

            GridReportTemp1.Parent.Controls.Add(form)
            form.Attributes("runat") = "server"
            form.Controls.Add(GridReportTemp1)

            Me.Controls.Add(form)

            form.RenderControl(htextw)
            Response.Write(stw.ToString())
            Response.End()
        Catch ex As Exception
            lbstatus.Text = "ExportGridToExcel =" + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            GridReport.PageIndex = 0
            ViewState("AdvanceFilter") = ""
            BindDataReport()
            BindDataNonReport()
            '  pnlNav.Visible = True
        Catch ex As Exception
            lbstatus.Text = "Btn Search Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnExpand_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExpand.Click
        Try
            tbfilter2.Text = ""
            If pnlSearch.Visible Then
                pnlSearch.Visible = False
            Else
                pnlSearch.Visible = True
            End If
        Catch ex As Exception
            lbstatus.Text = "btn Expand Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridRemove_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridRemove.PageIndexChanging
        Try
            GridRemove.PageIndex = e.NewPageIndex
            GridRemove.DataSource = ViewState("Remove")
            GridRemove.DataBind()
        Catch ex As Exception
            lbstatus.Text = "GridRemove_PageIndexChanging Error : " + ex.ToString + " page index : " + e.NewPageIndex.ToString
        End Try
    End Sub

    Protected Sub GridRemove_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridRemove.Sorting
        Try
            If ViewState("SortOrderRemove") = Nothing Or ViewState("SortOrderRemove") = "DESC" Then
                ViewState("SortOrderRemove") = "ASC"
            Else
                ViewState("SortOrderRemove") = "DESC"
            End If
            ViewState("SortExpressionRemove") = e.SortExpression + " " + ViewState("SortOrderRemove")
            BindDataNonReport()
        Catch ex As Exception
            lbstatus.Text = "GridRemove_Sorting Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub btnImportKeluaran_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImportKeluaran.Click
        Try
            'GridReportTemp.PageIndex = 0
            GridReportTemp1.Visible = True
            BindDataReportExport1()
            ExportGridToExcel1()
            GridReportTemp1.Visible = False

            'GridReport.PageIndex = 0
            'BindDataReport()
            'ExportGridToExcel()
        Catch ex As Exception
            lbstatus.Text = "btnImport_Click =" + vbCrLf + ex.ToString
        End Try
    End Sub
End Class
