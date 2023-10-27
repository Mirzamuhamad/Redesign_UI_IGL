'Imports CrystalDecisions.Shared
'Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data

Partial Class TrMTNSchedule_TrMTNSchedule
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            InitProperty()
            SetInit()
            Session("AdvanceFilter") = ""
        End If

        If Not Session("Result") Is Nothing Then
            If ViewState("Sender") = "btnSection" Then
                tbSection.Text = Session("Result")(0).ToString
                tbSectionName.Text = Session("Result")(1).ToString
                lbCode.Text = Session("Result")(0).ToString
                lbName.Text = Session("Result")(1).ToString
            End If
            bindDataGrid()
            Session("Result") = Nothing
            ViewState("Sender") = Nothing
            Session("filter") = Nothing
            Session("Column") = Nothing
        End If

        lstatus.Text = ""
    End Sub

    Private Sub SetInit()
        Try
            FillCombo(ddlYear, "EXEC S_GetYear", True, "Year", "Year", ViewState("DBConnection"))
            FillCombo(ddlYearFrom, "EXEC S_GetYear", True, "Year", "Year", ViewState("DBConnection"))
            FillCombo(ddlYearTo, "EXEC S_GetYear", True, "Year", "Year", ViewState("DBConnection"))
            FillCombo(ddlPattern, "Select PatternCode, PatternName from MsMaintenancePattern", True, "PatternCode", "PatternName", ViewState("DBConnection"))
            ViewState("SortExpression") = Nothing
            ViewState("DigitCurr") = 0
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            'FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
            If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
                'ddlCommand.Items.Add("Print")
                'ddlCommand2.Items.Add("Print")
            End If
            ddlYear.SelectedValue = ViewState("GLYear")
            ddlYearFrom.SelectedValue = ViewState("GLYear")
            ddlYearTo.SelectedValue = ViewState("GLYear")

        Catch ex As Exception
            Throw New Exception("Set Init Error : " + ex.ToString)
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

        'tbmonth.Attributes.Add("OnKeyDown", "return PressMonth();")
        'tbweek.Attributes.Add("OnKeyDown", "return PressWeek();")
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
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)

            If Trim(StrFilter) <> "" Then
                StrFilter = Replace(StrFilter, "Where", "and (")
                StrFilter = StrFilter + ")"
            End If
            
            SqlString = "EXEC S_MTNScheduleView " + QuotedStr(ddlYear.SelectedValue) + "," + QuotedStr(tbSection.Text) + ", " + QuotedStr(StrFilter) + ", " + QuotedStr(ViewState("UserId").ToString)

            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "MTNItem ASC"
            End If
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection"))
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    'Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
    '    Dim StrFilter As String
    '    Try
    '        StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
    '        Session("DBConnection") = ViewState("DBConnection")
    '        Session("PrintType") = "Print"
    '        Session("SelectCommand") = "S_FormPrintMaster4 'VMsFrequency','FrequencyCode','FrequencyName','XTime','InMonth','Frequency','Frequency Code','Frequency Name','XTime','InMonth'," + QuotedStr(StrFilter) + "," + QuotedStr(ViewState("UserId"))
    '        Session("ReportFile") = ".../../../Rpt/RptPrintMaster3.frx"
    '        AttachScript("openprintdlg();", Page, Me.GetType)
    '    Catch ex As Exception
    '        lstatus.Text = "Btn Print Error : " + ex.ToString
    '    End Try
    'End Sub

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
            lstatus.Text = "DataGrid_Sorting Error =" + vbCrLf + ex.ToString
        End Try

    End Sub

    Protected Friend Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        Dim txstatus As Label
        Dim Januari1, Januari2, Januari3, Januari4, Januari5 As TextBox
        Dim Febuari1, Febuari2, Febuari3, Febuari4, Febuari5 As TextBox
        Dim Maret1, Maret2, Maret3, Maret4, Maret5 As TextBox
        Dim April1, April2, April3, April4, April5 As TextBox
        Dim Mei1, Mei2, Mei3, Mei4, Mei5 As TextBox
        Dim Juni1, Juni2, Juni3, Juni4, Juni5 As TextBox
        Dim Juli1, Juli2, Juli3, Juli4, Juli5 As TextBox
        Dim Agustus1, Agustus2, Agustus3, Agustus4, Agustus5 As TextBox
        Dim September1, September2, September3, September4, September5 As TextBox
        Dim Oktober1, Oktober2, Oktober3, Oktober4, Oktober5 As TextBox
        Dim November1, November2, November3, November4, November5 As TextBox
        Dim Desember1, Desember2, Desember3, Desember4, Desember5 As TextBox

        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGrid.EditIndex = e.NewEditIndex
            'DataGrid.ShowFooter = False
            bindDataGrid()
            obj = DataGrid.Rows(e.NewEditIndex)
            Januari1 = obj.FindControl("Januari1Edit")
            Januari1.Focus()

            txstatus = obj.FindControl("StatusEdit")
            
            If txstatus.Text = "Actual" Then
                DataGrid.EditIndex = -1
                bindDataGrid()
                Exit Sub
            End If
            Januari1.Attributes.Add("OnKeyDown", "return PressFlag();")
            Januari2 = DataGrid.Rows(e.NewEditIndex).FindControl("Januari2Edit")
            Januari2.Attributes.Add("OnKeyDown", "return PressFlag();")
            Januari3 = DataGrid.Rows(e.NewEditIndex).FindControl("Januari3Edit")
            Januari3.Attributes.Add("OnKeyDown", "return PressFlag();")
            Januari4 = DataGrid.Rows(e.NewEditIndex).FindControl("Januari4Edit")
            Januari4.Attributes.Add("OnKeyDown", "return PressFlag();")
            Januari5 = DataGrid.Rows(e.NewEditIndex).FindControl("Januari5Edit")
            Januari5.Attributes.Add("OnKeyDown", "return PressFlag();")

            Febuari1 = DataGrid.Rows(e.NewEditIndex).FindControl("Febuari1Edit")
            Febuari1.Attributes.Add("OnKeyDown", "return PressFlag();")
            Febuari2 = DataGrid.Rows(e.NewEditIndex).FindControl("Febuari2Edit")
            Febuari2.Attributes.Add("OnKeyDown", "return PressFlag();")
            Febuari3 = DataGrid.Rows(e.NewEditIndex).FindControl("Febuari3Edit")
            Febuari3.Attributes.Add("OnKeyDown", "return PressFlag();")
            Febuari4 = DataGrid.Rows(e.NewEditIndex).FindControl("Febuari4Edit")
            Febuari4.Attributes.Add("OnKeyDown", "return PressFlag();")
            Febuari5 = DataGrid.Rows(e.NewEditIndex).FindControl("Febuari5Edit")
            Febuari5.Attributes.Add("OnKeyDown", "return PressFlag();")

            Maret1 = DataGrid.Rows(e.NewEditIndex).FindControl("Maret1Edit")
            Maret1.Attributes.Add("OnKeyDown", "return PressFlag();")
            Maret2 = DataGrid.Rows(e.NewEditIndex).FindControl("Maret2Edit")
            Maret2.Attributes.Add("OnKeyDown", "return PressFlag();")
            Maret3 = DataGrid.Rows(e.NewEditIndex).FindControl("Maret3Edit")
            Maret3.Attributes.Add("OnKeyDown", "return PressFlag();")
            Maret4 = DataGrid.Rows(e.NewEditIndex).FindControl("Maret4Edit")
            Maret4.Attributes.Add("OnKeyDown", "return PressFlag();")
            Maret5 = DataGrid.Rows(e.NewEditIndex).FindControl("Maret5Edit")
            Maret5.Attributes.Add("OnKeyDown", "return PressFlag();")

            April1 = DataGrid.Rows(e.NewEditIndex).FindControl("April1Edit")
            April1.Attributes.Add("OnKeyDown", "return PressFlag();")
            April2 = DataGrid.Rows(e.NewEditIndex).FindControl("April2Edit")
            April2.Attributes.Add("OnKeyDown", "return PressFlag();")
            April3 = DataGrid.Rows(e.NewEditIndex).FindControl("April3Edit")
            April3.Attributes.Add("OnKeyDown", "return PressFlag();")
            April4 = DataGrid.Rows(e.NewEditIndex).FindControl("April4Edit")
            April4.Attributes.Add("OnKeyDown", "return PressFlag();")
            April5 = DataGrid.Rows(e.NewEditIndex).FindControl("April5Edit")
            April5.Attributes.Add("OnKeyDown", "return PressFlag();")

            Mei1 = DataGrid.Rows(e.NewEditIndex).FindControl("Mei1Edit")
            Mei1.Attributes.Add("OnKeyDown", "return PressFlag();")
            Mei2 = DataGrid.Rows(e.NewEditIndex).FindControl("Mei2Edit")
            Mei2.Attributes.Add("OnKeyDown", "return PressFlag();")
            Mei3 = DataGrid.Rows(e.NewEditIndex).FindControl("Mei3Edit")
            Mei3.Attributes.Add("OnKeyDown", "return PressFlag();")
            Mei4 = DataGrid.Rows(e.NewEditIndex).FindControl("Mei4Edit")
            Mei4.Attributes.Add("OnKeyDown", "return PressFlag();")
            Mei5 = DataGrid.Rows(e.NewEditIndex).FindControl("Mei5Edit")
            Mei5.Attributes.Add("OnKeyDown", "return PressFlag();")

            Juni1 = DataGrid.Rows(e.NewEditIndex).FindControl("Juni1Edit")
            Juni1.Attributes.Add("OnKeyDown", "return PressFlag();")
            Juni2 = DataGrid.Rows(e.NewEditIndex).FindControl("Juni2Edit")
            Juni2.Attributes.Add("OnKeyDown", "return PressFlag();")
            Juni3 = DataGrid.Rows(e.NewEditIndex).FindControl("Juni3Edit")
            Juni3.Attributes.Add("OnKeyDown", "return PressFlag();")
            Juni4 = DataGrid.Rows(e.NewEditIndex).FindControl("Juni4Edit")
            Juni4.Attributes.Add("OnKeyDown", "return PressFlag();")
            Juni5 = DataGrid.Rows(e.NewEditIndex).FindControl("Juni5Edit")
            Juni5.Attributes.Add("OnKeyDown", "return PressFlag();")

            Juli1 = DataGrid.Rows(e.NewEditIndex).FindControl("Juli1Edit")
            Juli1.Attributes.Add("OnKeyDown", "return PressFlag();")
            Juli2 = DataGrid.Rows(e.NewEditIndex).FindControl("Juli2Edit")
            Juli2.Attributes.Add("OnKeyDown", "return PressFlag();")
            Juli3 = DataGrid.Rows(e.NewEditIndex).FindControl("Juli3Edit")
            Juli3.Attributes.Add("OnKeyDown", "return PressFlag();")
            Juli4 = DataGrid.Rows(e.NewEditIndex).FindControl("Juli4Edit")
            Juli4.Attributes.Add("OnKeyDown", "return PressFlag();")
            Juli5 = DataGrid.Rows(e.NewEditIndex).FindControl("Juli5Edit")
            Juli5.Attributes.Add("OnKeyDown", "return PressFlag();")

            Agustus1 = DataGrid.Rows(e.NewEditIndex).FindControl("Agustus1Edit")
            Agustus1.Attributes.Add("OnKeyDown", "return PressFlag();")
            Agustus2 = DataGrid.Rows(e.NewEditIndex).FindControl("Agustus2Edit")
            Agustus2.Attributes.Add("OnKeyDown", "return PressFlag();")
            Agustus3 = DataGrid.Rows(e.NewEditIndex).FindControl("Agustus3Edit")
            Agustus3.Attributes.Add("OnKeyDown", "return PressFlag();")
            Agustus4 = DataGrid.Rows(e.NewEditIndex).FindControl("Agustus4Edit")
            Agustus4.Attributes.Add("OnKeyDown", "return PressFlag();")
            Agustus5 = DataGrid.Rows(e.NewEditIndex).FindControl("Agustus5Edit")
            Agustus5.Attributes.Add("OnKeyDown", "return PressFlag();")

            September1 = DataGrid.Rows(e.NewEditIndex).FindControl("September1Edit")
            September1.Attributes.Add("OnKeyDown", "return PressFlag();")
            September2 = DataGrid.Rows(e.NewEditIndex).FindControl("September2Edit")
            September2.Attributes.Add("OnKeyDown", "return PressFlag();")
            September3 = DataGrid.Rows(e.NewEditIndex).FindControl("September3Edit")
            September3.Attributes.Add("OnKeyDown", "return PressFlag();")
            September4 = DataGrid.Rows(e.NewEditIndex).FindControl("September4Edit")
            September4.Attributes.Add("OnKeyDown", "return PressFlag();")
            September5 = DataGrid.Rows(e.NewEditIndex).FindControl("September5Edit")
            September5.Attributes.Add("OnKeyDown", "return PressFlag();")

            Oktober1 = DataGrid.Rows(e.NewEditIndex).FindControl("Oktober1Edit")
            Oktober1.Attributes.Add("OnKeyDown", "return PressFlag();")
            Oktober2 = DataGrid.Rows(e.NewEditIndex).FindControl("Oktober2Edit")
            Oktober2.Attributes.Add("OnKeyDown", "return PressFlag();")
            Oktober3 = DataGrid.Rows(e.NewEditIndex).FindControl("Oktober3Edit")
            Oktober3.Attributes.Add("OnKeyDown", "return PressFlag();")
            Oktober4 = DataGrid.Rows(e.NewEditIndex).FindControl("Oktober4Edit")
            Oktober4.Attributes.Add("OnKeyDown", "return PressFlag();")
            Oktober5 = DataGrid.Rows(e.NewEditIndex).FindControl("Oktober5Edit")
            Oktober5.Attributes.Add("OnKeyDown", "return PressFlag();")

            November1 = DataGrid.Rows(e.NewEditIndex).FindControl("November1Edit")
            November1.Attributes.Add("OnKeyDown", "return PressFlag();")
            November2 = DataGrid.Rows(e.NewEditIndex).FindControl("November2Edit")
            November2.Attributes.Add("OnKeyDown", "return PressFlag();")
            November3 = DataGrid.Rows(e.NewEditIndex).FindControl("November3Edit")
            November3.Attributes.Add("OnKeyDown", "return PressFlag();")
            November4 = DataGrid.Rows(e.NewEditIndex).FindControl("November4Edit")
            November4.Attributes.Add("OnKeyDown", "return PressFlag();")
            November5 = DataGrid.Rows(e.NewEditIndex).FindControl("November5Edit")
            November5.Attributes.Add("OnKeyDown", "return PressFlag();")

            Desember1 = DataGrid.Rows(e.NewEditIndex).FindControl("Desember1Edit")
            Desember1.Attributes.Add("OnKeyDown", "return PressFlag();")
            Desember2 = DataGrid.Rows(e.NewEditIndex).FindControl("Desember2Edit")
            Desember2.Attributes.Add("OnKeyDown", "return PressFlag();")
            Desember3 = DataGrid.Rows(e.NewEditIndex).FindControl("Desember3Edit")
            Desember3.Attributes.Add("OnKeyDown", "return PressFlag();")
            Desember4 = DataGrid.Rows(e.NewEditIndex).FindControl("Desember4Edit")
            Desember4.Attributes.Add("OnKeyDown", "return PressFlag();")
            Desember5 = DataGrid.Rows(e.NewEditIndex).FindControl("Desember5Edit")
            Desember5.Attributes.Add("OnKeyDown", "return PressFlag();")
        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        Dim SQLString As String
        Dim Januari1, Januari2, Januari3, Januari4, Januari5 As TextBox
        Dim Febuari1, Febuari2, Febuari3, Febuari4, Febuari5 As TextBox
        Dim Maret1, Maret2, Maret3, Maret4, Maret5 As TextBox
        Dim April1, April2, April3, April4, April5 As TextBox
        Dim Mei1, Mei2, Mei3, Mei4, Mei5 As TextBox
        Dim Juni1, Juni2, Juni3, Juni4, Juni5 As TextBox
        Dim Juli1, Juli2, Juli3, Juli4, Juli5 As TextBox
        Dim Agustus1, Agustus2, Agustus3, Agustus4, Agustus5 As TextBox
        Dim September1, September2, September3, September4, September5 As TextBox
        Dim Oktober1, Oktober2, Oktober3, Oktober4, Oktober5 As TextBox
        Dim November1, November2, November3, November4, November5 As TextBox
        Dim Desember1, Desember2, Desember3, Desember4, Desember5 As TextBox

        Dim lbMtnItem, lbJob As Label
        Try
            lbMtnItem = DataGrid.Rows(e.RowIndex).FindControl("MtnItemEdit")
            lbJob = DataGrid.Rows(e.RowIndex).FindControl("JobEdit")

            Januari1 = DataGrid.Rows(e.RowIndex).FindControl("Januari1Edit")
            Januari2 = DataGrid.Rows(e.RowIndex).FindControl("Januari2Edit")
            Januari3 = DataGrid.Rows(e.RowIndex).FindControl("Januari3Edit")
            Januari4 = DataGrid.Rows(e.RowIndex).FindControl("Januari4Edit")
            Januari5 = DataGrid.Rows(e.RowIndex).FindControl("Januari5Edit")

            Febuari1 = DataGrid.Rows(e.RowIndex).FindControl("Febuari1Edit")
            Febuari2 = DataGrid.Rows(e.RowIndex).FindControl("Febuari2Edit")
            Febuari3 = DataGrid.Rows(e.RowIndex).FindControl("Febuari3Edit")
            Febuari4 = DataGrid.Rows(e.RowIndex).FindControl("Febuari4Edit")
            Febuari5 = DataGrid.Rows(e.RowIndex).FindControl("Febuari5Edit")

            Maret1 = DataGrid.Rows(e.RowIndex).FindControl("Maret1Edit")
            Maret2 = DataGrid.Rows(e.RowIndex).FindControl("Maret2Edit")
            Maret3 = DataGrid.Rows(e.RowIndex).FindControl("Maret3Edit")
            Maret4 = DataGrid.Rows(e.RowIndex).FindControl("Maret4Edit")
            Maret5 = DataGrid.Rows(e.RowIndex).FindControl("Maret5Edit")

            April1 = DataGrid.Rows(e.RowIndex).FindControl("April1Edit")
            April2 = DataGrid.Rows(e.RowIndex).FindControl("April2Edit")
            April3 = DataGrid.Rows(e.RowIndex).FindControl("April3Edit")
            April4 = DataGrid.Rows(e.RowIndex).FindControl("April4Edit")
            April5 = DataGrid.Rows(e.RowIndex).FindControl("April5Edit")

            Mei1 = DataGrid.Rows(e.RowIndex).FindControl("Mei1Edit")
            Mei2 = DataGrid.Rows(e.RowIndex).FindControl("Mei2Edit")
            Mei3 = DataGrid.Rows(e.RowIndex).FindControl("Mei3Edit")
            Mei4 = DataGrid.Rows(e.RowIndex).FindControl("Mei4Edit")
            Mei5 = DataGrid.Rows(e.RowIndex).FindControl("Mei5Edit")

            Juni1 = DataGrid.Rows(e.RowIndex).FindControl("Juni1Edit")
            Juni2 = DataGrid.Rows(e.RowIndex).FindControl("Juni2Edit")
            Juni3 = DataGrid.Rows(e.RowIndex).FindControl("Juni3Edit")
            Juni4 = DataGrid.Rows(e.RowIndex).FindControl("Juni4Edit")
            Juni5 = DataGrid.Rows(e.RowIndex).FindControl("Juni5Edit")

            Juli1 = DataGrid.Rows(e.RowIndex).FindControl("Juli1Edit")
            Juli2 = DataGrid.Rows(e.RowIndex).FindControl("Juli2Edit")
            Juli3 = DataGrid.Rows(e.RowIndex).FindControl("Juli3Edit")
            Juli4 = DataGrid.Rows(e.RowIndex).FindControl("Juli4Edit")
            Juli5 = DataGrid.Rows(e.RowIndex).FindControl("Juli5Edit")

            Agustus1 = DataGrid.Rows(e.RowIndex).FindControl("Agustus1Edit")
            Agustus2 = DataGrid.Rows(e.RowIndex).FindControl("Agustus2Edit")
            Agustus3 = DataGrid.Rows(e.RowIndex).FindControl("Agustus3Edit")
            Agustus4 = DataGrid.Rows(e.RowIndex).FindControl("Agustus4Edit")
            Agustus5 = DataGrid.Rows(e.RowIndex).FindControl("Agustus5Edit")

            September1 = DataGrid.Rows(e.RowIndex).FindControl("September1Edit")
            September2 = DataGrid.Rows(e.RowIndex).FindControl("September2Edit")
            September3 = DataGrid.Rows(e.RowIndex).FindControl("September3Edit")
            September4 = DataGrid.Rows(e.RowIndex).FindControl("September4Edit")
            September5 = DataGrid.Rows(e.RowIndex).FindControl("September5Edit")

            Oktober1 = DataGrid.Rows(e.RowIndex).FindControl("Oktober1Edit")
            Oktober2 = DataGrid.Rows(e.RowIndex).FindControl("Oktober2Edit")
            Oktober3 = DataGrid.Rows(e.RowIndex).FindControl("Oktober3Edit")
            Oktober4 = DataGrid.Rows(e.RowIndex).FindControl("Oktober4Edit")
            Oktober5 = DataGrid.Rows(e.RowIndex).FindControl("Oktober5Edit")

            November1 = DataGrid.Rows(e.RowIndex).FindControl("November1Edit")
            November2 = DataGrid.Rows(e.RowIndex).FindControl("November2Edit")
            November3 = DataGrid.Rows(e.RowIndex).FindControl("November3Edit")
            November4 = DataGrid.Rows(e.RowIndex).FindControl("November4Edit")
            November5 = DataGrid.Rows(e.RowIndex).FindControl("November5Edit")

            Desember1 = DataGrid.Rows(e.RowIndex).FindControl("Desember1Edit")
            Desember2 = DataGrid.Rows(e.RowIndex).FindControl("Desember2Edit")
            Desember3 = DataGrid.Rows(e.RowIndex).FindControl("Desember3Edit")
            Desember4 = DataGrid.Rows(e.RowIndex).FindControl("Desember4Edit")
            Desember5 = DataGrid.Rows(e.RowIndex).FindControl("Desember5Edit")

            SQLString = "EXEC S_MTNScheduleApply  " + QuotedStr(ddlYear.SelectedValue) + "," + QuotedStr(lbMtnItem.Text) + ", " + QuotedStr(lbJob.Text) + ", " + _
            QuotedStr(Januari1.Text) + ", " + QuotedStr(Januari2.Text) + ", " + QuotedStr(Januari3.Text) + ", " + QuotedStr(Januari4.Text) + ", " + QuotedStr(Januari5.Text) + ", " + _
            QuotedStr(Febuari1.Text) + ", " + QuotedStr(Febuari2.Text) + ", " + QuotedStr(Febuari3.Text) + ", " + QuotedStr(Febuari4.Text) + ", " + QuotedStr(Febuari5.Text) + ", " + _
            QuotedStr(Maret1.Text) + ", " + QuotedStr(Maret2.Text) + ", " + QuotedStr(Maret3.Text) + ", " + QuotedStr(Maret4.Text) + ", " + QuotedStr(Maret5.Text) + ", " + _
            QuotedStr(April1.Text) + ", " + QuotedStr(April2.Text) + ", " + QuotedStr(April3.Text) + ", " + QuotedStr(April4.Text) + ", " + QuotedStr(April5.Text) + ", " + _
            QuotedStr(Mei1.Text) + ", " + QuotedStr(Mei2.Text) + ", " + QuotedStr(Mei3.Text) + ", " + QuotedStr(Mei4.Text) + ", " + QuotedStr(Mei5.Text) + ", " + _
            QuotedStr(Juni1.Text) + ", " + QuotedStr(Juni2.Text) + ", " + QuotedStr(Juni3.Text) + ", " + QuotedStr(Juni4.Text) + ", " + QuotedStr(Juni5.Text) + ", " + _
            QuotedStr(Juli1.Text) + ", " + QuotedStr(Juli2.Text) + ", " + QuotedStr(Juli3.Text) + ", " + QuotedStr(Juli4.Text) + ", " + QuotedStr(Juli5.Text) + ", " + _
            QuotedStr(Agustus1.Text) + ", " + QuotedStr(Agustus2.Text) + ", " + QuotedStr(Agustus3.Text) + ", " + QuotedStr(Agustus4.Text) + ", " + QuotedStr(Agustus5.Text) + ", " + _
            QuotedStr(September1.Text) + ", " + QuotedStr(September2.Text) + ", " + QuotedStr(September3.Text) + ", " + QuotedStr(September4.Text) + ", " + QuotedStr(September5.Text) + ", " + _
            QuotedStr(Oktober1.Text) + ", " + QuotedStr(Oktober2.Text) + ", " + QuotedStr(Oktober3.Text) + ", " + QuotedStr(Oktober4.Text) + ", " + QuotedStr(Oktober5.Text) + ", " + _
            QuotedStr(November1.Text) + ", " + QuotedStr(November2.Text) + ", " + QuotedStr(November3.Text) + ", " + QuotedStr(November4.Text) + ", " + QuotedStr(November5.Text) + ", " + _
            QuotedStr(Desember1.Text) + ", " + QuotedStr(Desember2.Text) + ", " + QuotedStr(Desember3.Text) + ", " + QuotedStr(Desember4.Text) + ", " + QuotedStr(Desember5.Text) + ", " + QuotedStr(ViewState("UserId").ToString)

            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            'DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
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

    Protected Sub tbSection_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbSection.TextChanged
        Dim Dr As DataRow
        Dim DT As DataTable

        Try

            DT = SQLExecuteQuery("SELECT * From MsMaintenanceSection Where MTNSectionCode = " + QuotedStr(tbSection.Text), ViewState("DBConnection").ToString).Tables(0)
            If DT.Rows.Count > 0 Then
                Dr = DT.Rows(0)
            Else
                Dr = Nothing
            End If

            If Not Dr Is Nothing Then
                tbSection.Text = Dr("MTNSectionCode")
                tbSectionName.Text = Dr("MTNSectionName")
                lbCode.Text = Dr("MTNSectionCode")
                lbName.Text = Dr("MTNSectionName")
            Else
                tbSection.Text = ""
                tbSectionName.Text = ""
                lbCode.Text = ""
                lbName.Text = ""
            End If
            bindDataGrid()
        Catch ex As Exception
            Throw New Exception("tbSection_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSection.Click
        Dim ResultField As String 'ResultSame 
        Try
            Session("Result") = Nothing
            Session("Filter") = "SELECT * From MsMaintenanceSection "

            ResultField = "MTNSectionCode, MTNSectionName"
            Session("Column") = ResultField.Split(",")
            ViewState("Sender") = "btnSection"
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())


        Catch ex As Exception
            lstatus.Text = "btnSection_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnGetData_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetData.Click
        Dim GVR As GridViewRow
        Dim lblMTNItem, lblJob, lblStatus As Label
        Dim CB As CheckBox
        Dim Nmbr(100) As String
        Dim sqlstring As String
        Dim DoneExec As Boolean
        Try
            If ddlPattern.SelectedValue.Trim = "" Then
                lstatus.Text = "Pattern must be selected"
                ddlPattern.Focus()
                Exit Sub
            End If
            DoneExec = False
            For Each GVR In DataGrid.Rows
                CB = GVR.FindControl("cbSelect")
                lblMTNItem = GVR.FindControl("MTNItem")
                lblJob = GVR.FindControl("Job")
                lblStatus = GVR.FindControl("Status")
                If CB.Checked And lblStatus.Text = "Schedule" Then
                    sqlstring = "EXEC S_MTNScheduleGenerate  " + QuotedStr(ddlYear.SelectedValue) + "," + QuotedStr(lblMTNItem.Text) + "," + QuotedStr(lblJob.Text) + "," + QuotedStr(ddlPattern.SelectedValue.ToString) + "," + QuotedStr(ViewState("UserId").ToString)
                    SQLExecuteNonQuery(sqlstring, ViewState("DBConnection"))
                    DoneExec = True
                End If
            Next
            If DoneExec Then
                bindDataGrid()
            End If
        Catch ex As Exception
            lstatus.Text = "btnGetData_Click Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            CheckAll(DataGrid, sender)
        Catch ex As Exception
            lstatus.Text = "cb Select Checked Changed Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub DataGrid_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGrid.RowCommand
        Try
            
        Catch ex As Exception
            lstatus.Text = "DataGrid_RowCommand Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlYear_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlYear.SelectedIndexChanged
        Try
            tbSection_TextChanged(Nothing, Nothing)
        Catch ex As Exception
            lstatus.Text = "ddlYear_SelectedIndexChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGrid.RowDeleting
        Dim txtStatus, txtMtnItem, txtJob As Label
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            txtStatus = DataGrid.Rows(e.RowIndex).FindControl("MTNItem")
            txtMtnItem = DataGrid.Rows(e.RowIndex).FindControl("MTNItem")
            txtMtnItem = DataGrid.Rows(e.RowIndex).FindControl("MTNItem")
            txtJob = DataGrid.Rows(e.RowIndex).FindControl("Job")
            'If txtStatus.Text = "Schedule" Then
            SQLExecuteNonQuery("EXEC S_MTNScheduleDelete " + ddlYear.SelectedValue + ", " + QuotedStr(txtMtnItem.Text) + ", " + QuotedStr(txtJob.Text) + ", " + QuotedStr(ViewState("UserId").ToString), ViewState("DBConnection").ToString)
            bindDataGrid()
            'End If
        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub Datagrid_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles DataGrid.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim TypeStatus = DirectCast(e.Row.DataItem, DataRowView)("Status").ToString()

                Dim Type011 = DirectCast(e.Row.DataItem, DataRowView)("0101").ToString()
                Dim Type012 = DirectCast(e.Row.DataItem, DataRowView)("0102").ToString()
                Dim Type013 = DirectCast(e.Row.DataItem, DataRowView)("0103").ToString()
                Dim Type014 = DirectCast(e.Row.DataItem, DataRowView)("0104").ToString()
                Dim Type015 = DirectCast(e.Row.DataItem, DataRowView)("0105").ToString()

                Dim Type021 = DirectCast(e.Row.DataItem, DataRowView)("0201").ToString()
                Dim Type022 = DirectCast(e.Row.DataItem, DataRowView)("0202").ToString()
                Dim Type023 = DirectCast(e.Row.DataItem, DataRowView)("0203").ToString()
                Dim Type024 = DirectCast(e.Row.DataItem, DataRowView)("0204").ToString()
                Dim Type025 = DirectCast(e.Row.DataItem, DataRowView)("0205").ToString()

                Dim Type031 = DirectCast(e.Row.DataItem, DataRowView)("0301").ToString()
                Dim Type032 = DirectCast(e.Row.DataItem, DataRowView)("0302").ToString()
                Dim Type033 = DirectCast(e.Row.DataItem, DataRowView)("0303").ToString()
                Dim Type034 = DirectCast(e.Row.DataItem, DataRowView)("0304").ToString()
                Dim Type035 = DirectCast(e.Row.DataItem, DataRowView)("0305").ToString()

                Dim Type041 = DirectCast(e.Row.DataItem, DataRowView)("0401").ToString()
                Dim Type042 = DirectCast(e.Row.DataItem, DataRowView)("0402").ToString()
                Dim Type043 = DirectCast(e.Row.DataItem, DataRowView)("0403").ToString()
                Dim Type044 = DirectCast(e.Row.DataItem, DataRowView)("0404").ToString()
                Dim Type045 = DirectCast(e.Row.DataItem, DataRowView)("0405").ToString()

                Dim Type051 = DirectCast(e.Row.DataItem, DataRowView)("0501").ToString()
                Dim Type052 = DirectCast(e.Row.DataItem, DataRowView)("0502").ToString()
                Dim Type053 = DirectCast(e.Row.DataItem, DataRowView)("0503").ToString()
                Dim Type054 = DirectCast(e.Row.DataItem, DataRowView)("0504").ToString()
                Dim Type055 = DirectCast(e.Row.DataItem, DataRowView)("0505").ToString()

                Dim Type061 = DirectCast(e.Row.DataItem, DataRowView)("0601").ToString()
                Dim Type062 = DirectCast(e.Row.DataItem, DataRowView)("0602").ToString()
                Dim Type063 = DirectCast(e.Row.DataItem, DataRowView)("0603").ToString()
                Dim Type064 = DirectCast(e.Row.DataItem, DataRowView)("0604").ToString()
                Dim Type065 = DirectCast(e.Row.DataItem, DataRowView)("0605").ToString()

                Dim Type071 = DirectCast(e.Row.DataItem, DataRowView)("0701").ToString()
                Dim Type072 = DirectCast(e.Row.DataItem, DataRowView)("0702").ToString()
                Dim Type073 = DirectCast(e.Row.DataItem, DataRowView)("0703").ToString()
                Dim Type074 = DirectCast(e.Row.DataItem, DataRowView)("0704").ToString()
                Dim Type075 = DirectCast(e.Row.DataItem, DataRowView)("0705").ToString()

                Dim Type081 = DirectCast(e.Row.DataItem, DataRowView)("0801").ToString()
                Dim Type082 = DirectCast(e.Row.DataItem, DataRowView)("0802").ToString()
                Dim Type083 = DirectCast(e.Row.DataItem, DataRowView)("0803").ToString()
                Dim Type084 = DirectCast(e.Row.DataItem, DataRowView)("0804").ToString()
                Dim Type085 = DirectCast(e.Row.DataItem, DataRowView)("0805").ToString()

                Dim Type091 = DirectCast(e.Row.DataItem, DataRowView)("0901").ToString()
                Dim Type092 = DirectCast(e.Row.DataItem, DataRowView)("0902").ToString()
                Dim Type093 = DirectCast(e.Row.DataItem, DataRowView)("0903").ToString()
                Dim Type094 = DirectCast(e.Row.DataItem, DataRowView)("0904").ToString()
                Dim Type095 = DirectCast(e.Row.DataItem, DataRowView)("0905").ToString()

                Dim Type101 = DirectCast(e.Row.DataItem, DataRowView)("1001").ToString()
                Dim Type102 = DirectCast(e.Row.DataItem, DataRowView)("1002").ToString()
                Dim Type103 = DirectCast(e.Row.DataItem, DataRowView)("1003").ToString()
                Dim Type104 = DirectCast(e.Row.DataItem, DataRowView)("1004").ToString()
                Dim Type105 = DirectCast(e.Row.DataItem, DataRowView)("1005").ToString()

                Dim Type111 = DirectCast(e.Row.DataItem, DataRowView)("1101").ToString()
                Dim Type112 = DirectCast(e.Row.DataItem, DataRowView)("1102").ToString()
                Dim Type113 = DirectCast(e.Row.DataItem, DataRowView)("1103").ToString()
                Dim Type114 = DirectCast(e.Row.DataItem, DataRowView)("1104").ToString()
                Dim Type115 = DirectCast(e.Row.DataItem, DataRowView)("1105").ToString()

                Dim Type121 = DirectCast(e.Row.DataItem, DataRowView)("1201").ToString()
                Dim Type122 = DirectCast(e.Row.DataItem, DataRowView)("1202").ToString()
                Dim Type123 = DirectCast(e.Row.DataItem, DataRowView)("1203").ToString()
                Dim Type124 = DirectCast(e.Row.DataItem, DataRowView)("1204").ToString()
                Dim Type125 = DirectCast(e.Row.DataItem, DataRowView)("1205").ToString()

                Dim btnEdit, btnDelete As Button
                Dim check As CheckBox
                check = e.Row.Cells(0).FindControl("cbSelect")
                btnDelete = e.Row.Cells(1).FindControl("btnDelete")
                btnEdit = e.Row.Cells(1).FindControl("btnEdit")
                If TypeStatus = "Schedule" Then
                    check.Visible = True
                    If Not btnEdit Is Nothing Then
                        btnEdit.Visible = True
                    End If
                    If Not btnDelete Is Nothing Then
                        btnDelete.Visible = True
                    End If
                    e.Row.Cells(0).Enabled = True
                    e.Row.Cells(1).Enabled = True
                    e.Row.Cells(2).BackColor = Drawing.Color.Transparent
                    e.Row.Cells(2).ForeColor = Drawing.Color.Black
                    e.Row.Cells(3).BackColor = Drawing.Color.Transparent
                    e.Row.Cells(3).ForeColor = Drawing.Color.Black
                    e.Row.Cells(4).BackColor = Drawing.Color.Transparent
                    e.Row.Cells(4).ForeColor = Drawing.Color.Black
                    e.Row.Cells(5).BackColor = Drawing.Color.Transparent
                    e.Row.Cells(5).ForeColor = Drawing.Color.Black
                    e.Row.Cells(6).BackColor = Drawing.Color.Transparent
                    e.Row.Cells(6).ForeColor = Drawing.Color.Black
                    e.Row.Cells(7).BackColor = Drawing.Color.Transparent
                    e.Row.Cells(7).ForeColor = Drawing.Color.Black
                Else
                    check.Visible = False
                    If Not btnEdit Is Nothing Then
                        btnEdit.Visible = False
                    End If
                    If Not btnDelete Is Nothing Then
                        btnDelete.Visible = False
                    End If
                    e.Row.Cells(0).Enabled = False
                    e.Row.Cells(1).Enabled = False
                    e.Row.Cells(2).BackColor = Drawing.Color.Transparent
                    e.Row.Cells(2).ForeColor = Drawing.Color.Transparent
                    e.Row.Cells(3).BackColor = Drawing.Color.Transparent
                    e.Row.Cells(3).ForeColor = Drawing.Color.Transparent
                    e.Row.Cells(4).BackColor = Drawing.Color.Transparent
                    e.Row.Cells(4).ForeColor = Drawing.Color.Transparent
                    e.Row.Cells(5).BackColor = Drawing.Color.Transparent
                    e.Row.Cells(5).ForeColor = Drawing.Color.Transparent
                    e.Row.Cells(6).BackColor = Drawing.Color.Transparent
                    e.Row.Cells(6).ForeColor = Drawing.Color.Transparent
                    e.Row.Cells(7).BackColor = Drawing.Color.Transparent
                    e.Row.Cells(7).ForeColor = Drawing.Color.Transparent
                End If
                DrawingCell(10, Type011, e)
                DrawingCell(11, Type012, e)
                DrawingCell(12, Type013, e)
                DrawingCell(13, Type014, e)
                DrawingCell(14, Type015, e)

                DrawingCell(15, Type021, e)
                DrawingCell(16, Type022, e)
                DrawingCell(17, Type023, e)
                DrawingCell(18, Type024, e)
                DrawingCell(19, Type025, e)

                DrawingCell(20, Type031, e)
                DrawingCell(21, Type032, e)
                DrawingCell(22, Type033, e)
                DrawingCell(23, Type034, e)
                DrawingCell(24, Type035, e)

                DrawingCell(25, Type041, e)
                DrawingCell(26, Type042, e)
                DrawingCell(27, Type043, e)
                DrawingCell(28, Type044, e)
                DrawingCell(29, Type045, e)

                DrawingCell(30, Type051, e)
                DrawingCell(31, Type052, e)
                DrawingCell(32, Type053, e)
                DrawingCell(33, Type054, e)
                DrawingCell(34, Type055, e)

                DrawingCell(35, Type061, e)
                DrawingCell(36, Type062, e)
                DrawingCell(37, Type063, e)
                DrawingCell(38, Type064, e)
                DrawingCell(39, Type065, e)

                DrawingCell(40, Type071, e)
                DrawingCell(41, Type072, e)
                DrawingCell(42, Type073, e)
                DrawingCell(43, Type074, e)
                DrawingCell(44, Type075, e)

                DrawingCell(45, Type081, e)
                DrawingCell(46, Type082, e)
                DrawingCell(47, Type083, e)
                DrawingCell(48, Type084, e)
                DrawingCell(49, Type085, e)

                DrawingCell(50, Type091, e)
                DrawingCell(51, Type092, e)
                DrawingCell(52, Type093, e)
                DrawingCell(53, Type094, e)
                DrawingCell(54, Type095, e)

                DrawingCell(55, Type101, e)
                DrawingCell(56, Type102, e)
                DrawingCell(57, Type103, e)
                DrawingCell(58, Type104, e)
                DrawingCell(59, Type105, e)

                DrawingCell(60, Type111, e)
                DrawingCell(61, Type112, e)
                DrawingCell(62, Type113, e)
                DrawingCell(63, Type114, e)
                DrawingCell(64, Type115, e)

                DrawingCell(65, Type121, e)
                DrawingCell(66, Type122, e)
                DrawingCell(67, Type123, e)
                DrawingCell(68, Type124, e)
                DrawingCell(69, Type125, e)
            End If
        Catch ex As Exception
            lstatus.Text = "Datagrid_RowDataBound Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub DrawingCell(ByVal cell As Integer, ByVal Type As String, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        Select Case TrimStr(Type)
            Case "Y"
                e.Row.Cells(cell).BackColor = System.Drawing.Color.Brown
                e.Row.Cells(cell).ForeColor = System.Drawing.Color.White
            Case "N"
                e.Row.Cells(cell).BackColor = System.Drawing.Color.YellowGreen
                e.Row.Cells(cell).ForeColor = System.Drawing.Color.Black
            Case ""
                e.Row.Cells(cell).BackColor = System.Drawing.Color.Transparent
                e.Row.Cells(cell).ForeColor = System.Drawing.Color.Black
            Case Else
                e.Row.Cells(cell).BackColor = System.Drawing.Color.Violet
                e.Row.Cells(cell).ForeColor = System.Drawing.Color.Black
        End Select
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Dim StrFilter As String
        Try
            If tbSection.Text.Trim = "" Then
                lstatus.Text = "MTN Section must be fill"
                Exit Sub
            End If
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)

            If Trim(StrFilter) <> "" Then
                StrFilter = Replace(StrFilter, "Where", "and (")
                StrFilter = StrFilter + ")"
            End If
            SQLExecuteNonQuery("EXEC S_MTNScheduleGenerateDefault " + ddlYear.SelectedValue.ToString + ", " + QuotedStr(tbSection.Text.Trim) + ", " + QuotedStr(StrFilter) + ", " + QuotedStr(ViewState("UserId").ToString), ViewState("DBConnection").ToString)
            bindDataGrid()
        Catch ex As Exception
            lstatus.Text = "btnReset_Click Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub ddlPattern_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPattern.SelectedIndexChanged
        Try
            tbPattern.Text = ddlPattern.SelectedValue.ToString
        Catch ex As Exception
            lstatus.Text = "ddlPattern_SelectedIndexChanged Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub btnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGo.Click
        Dim SQLString As String
        If ddlYearFrom.SelectedIndex < 1 Then
            lstatus.Text = "Year From must be select"
            Exit Sub
        End If
        If ddlYearTo.SelectedIndex < 1 Then
            lstatus.Text = "Year To must be select"
            Exit Sub
        End If
        If lbCode.Text.Trim = "" Then
            lstatus.Text = "MTN Section must be fill"
            Exit Sub
        End If
        SQLString = "EXEC S_MTNScheduleCopyTo  " + QuotedStr(lbCode.Text) + "," + QuotedStr(ddlYearFrom.SelectedValue) + ", " + QuotedStr(ddlYearTo.SelectedValue) + ", " + QuotedStr(ViewState("UserId").ToString)

        SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
    End Sub
End Class
