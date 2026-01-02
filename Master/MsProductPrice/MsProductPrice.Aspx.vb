Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Partial Class Master_MsProductPrice_MsProductPrice
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Try
            If Not IsPostBack Then
                InitProperty()
                FillCombo(ddlCategory, "Select Type_Code, Type_Name from V_MsProductType", False, "Type_Code", "Type_Name", ViewState("DBConnection"))
                DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
                tbDate.SelectedDate = ViewState("ServerDate") 'Date.Today
                BindData()
                Dim dt As DataTable
                dt = SQLExecuteQuery("Select RptTitle, MenuName, COALESCE(MenuParam,'') AS MenuParam, COALESCE(FastRpt,'') AS FastRpt, COALESCE(ReportType,'') AS ReportType, COALESCE(ReportType2,'') AS ReportType2, COALESCE(FgPrintValue,'N') AS FgPrintValue, COALESCE(FgForceNewPage,'N') AS FgForceNewPage from MsMenu WHERE MenuId = " + QuotedStr(Request.QueryString("ContainerId").ToString), ViewState("DBConnection").ToString).Tables(0)
                lblTitle.Text = dt.Rows(0)("MenuName").ToString

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
        Dim Price As TextBox
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            If StrFilter.Length > 5 Then
                StrFilter = StrFilter.Remove(1, 5)
                StrFilter = " And " + StrFilter
            End If

            tempDS = SQLExecuteQuery("EXEC S_MsProductPriceGetDt " + QuotedStr(Format(tbDate.SelectedDate, "yyyyMMdd")) + ", " + QuotedStr(ddlCategory.SelectedValue) + ", " + QuotedStr(StrFilter), ViewState("DBConnection").ToString)
            DataGrid.DataSource = tempDS.Tables(0)
            DataGrid.DataBind()
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "ProductCode DESC"
            End If

            For Each GVR In DataGrid.Rows
                Price = GVR.FindControl("Price")
                'TaxRate = GVR.FindControl("TaxRate")
                Price.Attributes.Add("OnKeyDown", "return PressNumeric();")
                ' TaxRate.Attributes.Add("OnKeyDown", "return PressNumeric();")
            Next

        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub


    Protected Sub BtnApply_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnApply.Click
        Dim SQLString As String
        Dim lbCode, lbLastPrice As Label
        Dim tbPrice As TextBox
        Dim GVR As GridViewRow
        Dim i As Integer
        Dim exe As Boolean
        Try
            exe = True
            For i = 0 To DataGrid.Rows.Count - 1
                GVR = DataGrid.Rows(i)
                lbCode = GVR.FindControl("ProductCode")
                tbPrice = GVR.FindControl("Price")
                tbPrice.Text = tbPrice.Text.Replace(",", "")
                lbLastPrice = GVR.FindControl("LastPrice")
                lbLastPrice.Text = lbLastPrice.Text.Replace(",", "")

                If lbLastPrice.Text.Trim = "" Then
                    lbLastPrice.Text = "0"
                End If

                If tbPrice.Text.Trim = "" Then
                    tbPrice.Text = "0"
                End If

                If Not IsNumeric(tbPrice.Text) Then
                    lbstatus.Text = "Price for " + lbCode.Text + " must in numeric format"
                    exe = False
                    tbPrice.Focus()
                    Exit For
                End If

                'If Not IsNumeric(tbLastPrice.Text) Then
                'lbstatus.Text = "Rate for " + lbCode.Text + " must in numeric format"
                'exe = False
                'tbLastPrice.Focus()
                ' Exit For
                ' End If
            Next
            If exe Then
                For i = 0 To DataGrid.Rows.Count - 1
                    GVR = DataGrid.Rows(i)

                    lbCode = GVR.FindControl("ProductCode")
                    tbPrice = GVR.FindControl("Price")
                    lbLastPrice = GVR.FindControl("LastPrice")

                    tbPrice.Text = tbPrice.Text.Replace(",", "")
                    lbLastPrice.Text = lbLastPrice.Text.Replace(",", "")

                    If Not ((tbPrice.Text = "" Or tbPrice.Text = "0")) And (tbPrice.Text <> lbLastPrice.Text) Then
                        SQLString = "EXEC S_MsProductPriceUpdate " + QuotedStr(Format(tbDate.SelectedDate, "yyyy-MM-dd")) + ", " + _
                            QuotedStr(lbCode.Text) + ", " + tbPrice.Text + ", " + QuotedStr(ViewState("UserId").ToString)
                        SQLExecuteScalar(SQLString, ViewState("DBConnection").ToString)
                    End If
                Next
                BindData()
            End If

        Catch ex As Exception
            lbstatus.Text = "btn apply error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbDate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbDate.SelectionChanged
        Try
            BindData()
        Catch ex As Exception
            lbstatus.Text = "Date Selection Changed Error : " + ex.ToString
        End Try
    End Sub

    ' Protected Sub Price_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    '    Dim tb As TextBox
    '    Dim Count As Integer
    '    Dim dgi As GridViewRow

    '      Try
    '         tb = sender
    '        If tb.ID = "Price" Then
    '           Count = DataGrid.EditIndex
    'lbstatus.Text = CStr(Count)
    'Exit Sub
    'dgi = DataGrid.Rows(Count) '-1 for allowpaging = False   - 2 allowpaging = True
    'tbRate = dgi.FindControl("CurrRate")
    'tbTaxRate = dgi.FindControl("TaxRate")
    'tbTaxRate.Text = tbRate.Text
    '      End If


    ' Catch ex As Exception
    '    lbstatus.Text = ex.ToString
    ' End Try
    'End Sub


    'Protected Sub CurrRate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CurrRate.TextChanged
    'Dim CurrRate, tb As TextBox
    'Dim dgi As GridViewRow
    'Dim Count As Integer
    '   Try
    '      tb = sender
    '     Count = DataGrid.SelectedValue
    '    dgi = DataGrid.Rows(Count)
    '   CurrRate = dgi.FindControl("CurrRate")

    'Catch ex As Exception
    '   lbstatus.Text = ex.ToString
    'End Try
    'End Sub


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

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try
            Dim tempDS As New DataSet()
            Dim StrFilter As String

            Try
                StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
                If StrFilter.Length > 5 Then
                    StrFilter = StrFilter.Remove(1, 5)
                    StrFilter = " And " + StrFilter
                End If
                tempDS = SQLExecuteQuery("EXEC S_MsProductPriceGetDt " + QuotedStr(Format(tbDate.SelectedDate, "yyyyMMdd")) + ", " + QuotedStr(ddlCategory.SelectedValue) + ", " + QuotedStr(StrFilter), ViewState("DBConnection").ToString)
                GridExport.DataSource = tempDS.Tables(0)
                GridExport.DataBind()
            Catch ex As Exception
                Throw New Exception("Bind Data Error : " + ex.ToString)
            End Try
            ExportGridToExcel("Price")
        Catch ex As Exception
            Throw New Exception("btn Export Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub ddlCategory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCategory.SelectedIndexChanged
        Try
            DataGrid.PageIndex = 0
            DataGrid.EditIndex = -1
            BindData()
        Catch ex As Exception
            'lstatus.Text = "ddlCategory_SelectedIndexChanged =" + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGrid.PageIndexChanging
        DataGrid.PageIndex = e.NewPageIndex
        If DataGrid.EditIndex <> -1 Then
            'DataGrid_RowCancelingEdit(Nothing, Nothing)
        End If
        BindData()
    End Sub
End Class
