Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Partial Class Master_MsProcessPremiHadir_MsProcessPremiHadir
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                FillCombo(ddlYear, "S_PLPanenPremiHadirGetYear", False, "Year", "Year", ViewState("DBConnection"))
                DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))

                BindData()
            End If
            lbstatus.Text = ""
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

    Sub BindData()
        Dim tempDS As New DataSet()
        Dim StrFilter As String

        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            If StrFilter.Length > 5 Then
                StrFilter = StrFilter.Remove(1, 5)
                StrFilter = " And " + StrFilter
            End If
            tempDS = SQLExecuteQuery("EXEC S_PLPanenPremiHadir " + QuotedStr(ddlYear.SelectedValue) + ", " + QuotedStr(StrFilter), ViewState("DBConnection").ToString)
            DataGrid.DataSource = tempDS.Tables(0)
            DataGrid.DataBind()
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "WeekNo DESC"
            End If

            'For Each GVR In DataGrid.Rows
            '    Price = GVR.FindControl("Price")
            '    'TaxRate = GVR.FindControl("TaxRate")
            '    Price.Attributes.Add("OnKeyDown", "return PressNumeric();")
            '    ' TaxRate.Attributes.Add("OnKeyDown", "return PressNumeric();")
            'Next
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub BtnApply_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnApply.Click
        Try
            bindDataProcess()
            BindData()
        Catch ex As Exception
            lbstatus.Text = "btn apply error : " + ex.ToString
        End Try
    End Sub
    Protected Sub BtnUnApply_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnUnApply.Click
        Try
            bindDataUnProcess()
            BindData()
        Catch ex As Exception
            lbstatus.Text = "btn apply error : " + ex.ToString
        End Try
    End Sub
    Private Sub bindDataProcess()
        Try
            Dim GVR As GridViewRow
            Dim CB As CheckBox
            Dim lbWeekNo As Label
            Dim HaveSelect As Boolean

            Dim SQLString, Hasil As String
            HaveSelect = False
            For Each GVR In DataGrid.Rows
                CB = GVR.FindControl("cbSelect")
                lbWeekNo = GVR.FindControl("WeekNo")

                If CB.Checked Then
                    HaveSelect = True
                    SQLString = "DECLARE @A VARCHAR(255) EXEC S_PLPanenProcessPremiHadir " + QuotedStr(lbWeekNo.Text) + _
                    ", " + QuotedStr(ViewState("UserId").ToString) + ", @A OUT SELECT @A "
                    ' lbstatus.Text = SQLString
                    'Exit For
                    'SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                    Hasil = SQLExecuteScalar(SQLString, ViewState("DBConnection").ToString)
                    Hasil = Replace(Hasil, "0", "")

                    If Trim(Hasil) <> "" Then
                        lbstatus.Text = MessageDlg(Hasil)
                        '  Exit For
                    End If
                End If
            Next
            BindData()
            If HaveSelect = False Then
                lbstatus.Text = "Please Check Week for Process Premi"
                Exit Sub
                'Else
                '   lbstatus.Text = Hasil + "Process Success "
            End If

        Catch ex As Exception
            Throw New Exception("bindDataSetLF Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub bindDataUnProcess()
        Try
            Dim GVR As GridViewRow
            Dim CB As CheckBox
            Dim lbWeekNo As Label
            Dim HaveSelect As Boolean

            Dim SQLString, Hasil As String
            HaveSelect = False
            For Each GVR In DataGrid.Rows
                CB = GVR.FindControl("cbSelect")
                lbWeekNo = GVR.FindControl("WeekNo")

                If CB.Checked Then
                    HaveSelect = True
                    SQLString = "DECLARE @A VARCHAR(255) EXEC S_PLPanenUnProcessPremiHadir " + QuotedStr(lbWeekNo.Text) + _
                    ", " + QuotedStr(ViewState("UserId").ToString) + ", @A OUT SELECT @A "

                    'SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                    Hasil = SQLExecuteScalar(SQLString, ViewState("DBConnection").ToString)
                    Hasil = Replace(Hasil, "0", "")

                    If Trim(Hasil) <> "" Then
                        lbstatus.Text = MessageDlg(Hasil)
                        ' Exit For
                    End If
                End If
            Next
            BindData()
            If HaveSelect = False Then
                lbstatus.Text = "Please Check Week for Process Premi"
                Exit Sub
                'Else
                'lbstatus.Text = "Un Process Success "
            End If

        Catch ex As Exception
            Throw New Exception("bindDataSetLF Error : " + ex.ToString)
        End Try
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

    Public Sub ExportGridToExcel(ByVal filenamevalue As String)
        Dim form As HtmlForm = New HtmlForm()
        Dim worksheetname As String
        worksheetname = Left(filenamevalue, 31)
        Dim attachment As String '= "attachment; filename=PrintDetails.xls"
        attachment = "attachment; filename=" + filenamevalue + ".xls"
        Response.ClearContent()
        Response.AddHeader("content-disposition", attachment)
        Response.ContentType = "application/ms-excel"
        'namespace (using system.IO)      
        Dim stw As StringWriter = New StringWriter()
        Dim htextw As HtmlTextWriter = New HtmlTextWriter(stw)
        GridExport.Parent.Controls.Add(form)
        form.Attributes("runat") = "server"
        form.Controls.Add(GridExport)
        Me.Controls.Add(form)
        form.RenderControl(htextw)
        Response.Write(stw.ToString())
        Response.End()
    End Sub

    'Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
    '    Try
    '        Dim tempDS As New DataSet()
    '        Dim StrFilter As String
    '        Try
    '            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
    '            If StrFilter.Length > 5 Then
    '                StrFilter = StrFilter.Remove(1, 5)
    '                StrFilter = " And " + StrFilter
    '            End If
    '            tempDS = SQLExecuteQuery("EXEC S_MsProductPriceGetDt " + QuotedStr(Format(tbDate.SelectedDate, "yyyyMMdd")) + ", " + QuotedStr(StrFilter), ViewState("DBConnection").ToString)
    '            GridExport.DataSource = tempDS.Tables(0)
    '            GridExport.DataBind()
    '        Catch ex As Exception
    '            Throw New Exception("Bind Data Error : " + ex.ToString)
    '        End Try
    '        ExportGridToExcel("Price")
    '    Catch ex As Exception
    '        Throw New Exception("btn Export Error : " + ex.ToString)
    '    End Try
    'End Sub
    Protected Sub ddlyear_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlYear.SelectedIndexChanged
        Try
            DataGrid.PageIndex = 0
            DataGrid.EditIndex = -1
            BindData()
        Catch ex As Exception
            'lstatus.Text = "ddlCategory_SelectedIndexChanged =" + vbCrLf + ex.ToString
        End Try
    End Sub
    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        CheckAllDt(DataGrid, sender)
    End Sub
    Public Sub CheckAllDt(ByRef gd As GridView, ByVal sender As CheckBox)
        Dim cb, cbselek As CheckBox
        Dim GRW As GridViewRow
        Try
            cb = sender
            For Each GRW In gd.Rows
                cbselek = GRW.FindControl("cbSelect")
                cbselek.Checked = cb.Checked
                If cb.Checked = False Then
                    'btnGetSetZero.Visible = True
                End If
            Next
        Catch ex As Exception
            Throw New Exception("Check All Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub DataGrid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGrid.PageIndexChanging
        DataGrid.PageIndex = e.NewPageIndex
        BindData()
    End Sub

    
End Class
