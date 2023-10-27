Imports System.Data
Imports System.Data.SqlClient

Partial Class Transaction_MsWeek_MsWeek
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dr As DataRow
        Dim ds As DataSet

        Try
        
            If Not IsPostBack Then
                InitProperty()
                DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
                ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                pnlService.Visible = True
                BindData()
            End If

            If Not Session("Result") Is Nothing Then

                Session("Result") = Nothing
                ViewState("Sender") = Nothing
                Session("filter") = Nothing
                Session("Column") = Nothing
            End If
            TbYear.Attributes.Add("OnKeyDown", "return PressNumeric();")
            lbstatus.Text = ""
            'getValuePost = ""
        Catch ex As Exception
            lbstatus.Text = "Page Load Error : " + ex.ToString
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
            'If CommandName = "Insert" Then
            '    If ViewState("FgInsert") = "N" Then
            '        lstatus.Text = "<script language='javascript'> {alert('You are not authorized to insert record. Please contact administrator')}</script>"
            '        Return False
            '        Exit Function
            '    End If
            'End If

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

    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim StrFilter, SqlString As String
        Dim GVR As GridViewRow
        Dim XPeriod, RangePeriod As TextBox

        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            SqlString = " select WeekNo, Year, Week, StartDate, EndDate FROM MsWeek  " + StrFilter
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "WeekNo ASC"
                ViewState("SortOrder") = "ASC"
            End If
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection"))

            GVR = DataGrid.FooterRow
            ' XPeriod = GVR.FindControl("XPeriodAdd")
            ' XPeriod.Attributes.Add("OnKeyDown", "return PressNumeric();")

            'RangePeriod = GVR.FindControl("XRangeAdd")
            ' RangePeriod.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Catch ex As Exception
            lbstatus.Text = lbstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
        'Dim tempDS As New DataSet()
        'Dim StrFilter As String

        'Try
        '    StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
        '    If AdvanceFilter.Length > 1 And StrFilter.Length > 1 Then
        '        StrFilter = StrFilter + " And " + AdvanceFilter
        '    ElseIf AdvanceFilter.Length > 1 And StrFilter.Length <= 1 Then
        '        StrFilter = AdvanceFilter
        '    End If

        '    'lbstatus.Text = "EXEC S_MsPriceListViewService " + QuotedStr(Format(tbDate.SelectedDate, "yyyy-MM-dd")) + "," + QuotedStr(StrFilter) + "," + QuotedStr(Request.QueryString("MenuParam"))
        '    'Exit Sub

        '    tempDS = SQLExecuteQuery("select WeekNo, Year, Week, StartDate, EndDate FROM MsWeek Where WeekNo IS NOT NULL ", ViewState("DBConnection").ToString)
        '    'QuotedStr(Format(tbDate.SelectedDate, "yyyyMMdd"))
        '    DataGrid.DataSource = tempDS.Tables(0)
        '    DataGrid.DataBind()

        '    'GVR = DataGrid.FindControl("Price")
        '    'Price = GVR.FindControl("Price")
        '    'Price.Attributes.Add("OnKeyDown", "return PressNumeric();")

        '    'For Each GVR In DataGrid.Rows
        '    '    Price = GVR.FindControl("Price")
        '    '    Price.Attributes.Add("OnKeyDown", "return PressNumeric();")
        '    'Next
        'Catch ex As Exception
        '    Throw New Exception("Bind Data Error : " + ex.ToString)
        'End Try
    End Sub

    Protected Sub BtnApply_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnApply.Click
        Dim WeekNo, Year, SQLString, SQLString2 As String
        Dim dr As DataRow
        Dim dt As DataTable
        Dim exe As Boolean

        Try
            exe = True

            If TbYear.Text = "" Then
                lbstatus.Text = MessageDlg("Year Must Have Value")
                Exit Sub
                TbYear.Focus()
            End If

            If Not IsNumeric(TbYear.Text) Then
                lbstatus.Text = "Year for " + TbYear.Text + " must in numeric format"
                TbYear.Focus()
            End If

            SQLString = "EXEC S_GenerateWeek " + QuotedStr(TbYear.Text)
            dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
    
            Dim cekStatus As String
            Year = SQLExecuteScalar("Select year FROM MsWeek WHERE year = " + QuotedStr(TbYear.Text), ViewState("DBConnection").ToString)
            WeekNo = SQLExecuteScalar("Select year FROM MsWeek ", ViewState("DBConnection").ToString)
            cekStatus = SQLExecuteScalar("Select WeekNo FROM MsWeek WHERE WeekNo = " + QuotedStr(WeekNo), ViewState("DBConnection").ToString)
            'lbstatus.Text = MessageDlg(WeekNo)
            'Exit Sub
            If Year = TbYear.Text Then
                lbstatus.Text = MessageDlg("Year is already exist")
                Exit Sub
            End If

            For Each dr In dt.Rows()
                If CInt(dr("WK")) < 10 Then
                    WeekNo = Str(dr("Yr")) + "0" + Trim(Str(dr("WK")))
                Else
                    WeekNo = Str(dr("Yr")) + "" + Trim(Str(dr("WK")))
                End If

                SQLString2 = "INSERT INTO MsWeek (WeekNo, Year, Week, StartDate, EndDate) SELECT " + QuotedStr(WeekNo) + ", " + Str(dr("Yr")) + ", " + _
                  Str(dr("WK")) + ", " + QuotedStr(Format(dr("StartDate"), "yyyy-MM-dd")) + ", " + QuotedStr(Format(dr("EndDate"), "yyyy-MM-dd")) + ""
                SQLExecuteScalar(SQLString2, ViewState("DBConnection").ToString)
                'lbstatus.Text = MessageDlg(SQLString2)
            Next

            BindData()
        Catch ex As Exception
            lbstatus.Text = "btn apply error : " + ex.ToString
        End Try
    End Sub


    Protected Sub BtnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnPrint.Click
        Dim SQLString As String
        Dim StrFilter As String

        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("DBConnection") = ViewState("DBConnection")
            Session("PrintType") = "Print"
            SQLString = "Select WeekNo, Year, Week, StartDate, EndDate MsWeek " + QuotedStr(TbYear.Text)
            Session("SelectCommand") = SQLString
            Session("ReportFile") = ".../../../Rpt/RptPrintMsPriceService.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)

        Catch ex As Exception
            lbstatus.Text = "BtnPrint_Click Error : " + ex.ToString
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

    Protected Sub DataGrid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGrid.PageIndexChanging
        DataGrid.PageIndex = e.NewPageIndex
        BindData()
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            DataGrid.PageIndex = 0
            DataGrid.EditIndex = -1
            BindData()
                   Catch ex As Exception
            lbstatus.Text = "Btn Search Error : " + vbCrLf + ex.ToString
        End Try
    End Sub




End Class
