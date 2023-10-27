Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Partial Class Master_MsExpPrice_MsExpPrice
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
                tbDate.SelectedDate = ViewState("ServerDate") 'Date.Today
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
        Dim PriceEmp, PriceContractor As TextBox
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            If StrFilter.Length > 5 Then
                StrFilter = StrFilter.Remove(1, 5)
                StrFilter = " And " + StrFilter
            End If
            tempDS = SQLExecuteQuery("EXEC S_MsExpedisiPriceGetDt " + QuotedStr(Format(tbDate.SelectedDate, "yyyyMMdd")) + ", " + QuotedStr(StrFilter), ViewState("DBConnection").ToString)
            DataGrid.DataSource = tempDS.Tables(0)
            DataGrid.DataBind()

            For Each GVR In DataGrid.Rows
                PriceEmp = GVR.FindControl("PriceEmp")
                PriceContractor = GVR.FindControl("PriceContractor")
                PriceEmp.Attributes.Add("OnKeyDown", "return PressNumeric();")
                PriceContractor.Attributes.Add("OnKeyDown", "return PressNumeric();")
            Next
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub BtnApply_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnApply.Click
        Dim SQLString As String
        Dim lbCode, lbLastPriceEmp, lbLastPriceContractor As Label
        Dim tbPriceEmp, tbPriceContractor As TextBox
        Dim GVR As GridViewRow
        Dim i As Integer
        Dim exe As Boolean
        Try
            exe = True

            For i = 0 To DataGrid.Rows.Count - 1
                GVR = DataGrid.Rows(i)
                lbCode = GVR.FindControl("Customer")
                tbPriceEmp = GVR.FindControl("PriceEmp")
                tbPriceContractor = GVR.FindControl("PriceContractor")
                tbPriceEmp.Text = tbPriceEmp.Text.Replace(",", "")
                tbPriceContractor.Text = tbPriceContractor.Text.Replace(",", "")
                lbLastPriceEmp = GVR.FindControl("LastPriceEmp")
                lbLastPriceContractor = GVR.FindControl("LastPriceContractor")
                lbLastPriceEmp.Text = lbLastPriceEmp.Text.Replace(",", "")
                lbLastPriceContractor.Text = lbLastPriceContractor.Text.Replace(",", "")

                If lbLastPriceEmp.Text.Trim = "" Then
                    lbLastPriceEmp.Text = "0"
                End If
                If lbLastPriceContractor.Text.Trim = "" Then
                    lbLastPriceContractor.Text = "0"
                End If
                If tbPriceEmp.Text.Trim = "" Then
                    tbPriceEmp.Text = "0"
                End If
                If tbPriceContractor.Text.Trim = "" Then
                    tbPriceContractor.Text = "0"
                End If
                If Not IsNumeric(tbPriceEmp.Text) Then
                    lbstatus.Text = "Price Employee for " + lbCode.Text + " must in numeric format"
                    exe = False
                    tbPriceEmp.Focus()
                    Exit For
                End If
                If Not IsNumeric(tbPriceContractor.Text) Then
                    lbstatus.Text = "Price Contractor for " + lbCode.Text + " must in numeric format"
                    exe = False
                    tbPriceContractor.Focus()
                    Exit For
                End If
            Next
            If exe Then
                For i = 0 To DataGrid.Rows.Count - 1
                    GVR = DataGrid.Rows(i)

                    lbCode = GVR.FindControl("Customer")
                    tbPriceEmp = GVR.FindControl("PriceEmp")
                    tbPriceContractor = GVR.FindControl("PriceContractor")
                    lbLastPriceEmp = GVR.FindControl("LastPriceEmp")
                    lbLastPriceContractor = GVR.FindControl("LastPriceContractor")

                    tbPriceEmp.Text = tbPriceEmp.Text.Replace(",", "")
                    tbPriceContractor.Text = tbPriceContractor.Text.Replace(",", "")
                    lbLastPriceEmp.Text = lbLastPriceEmp.Text.Replace(",", "")
                    lbLastPriceContractor.Text = lbLastPriceContractor.Text.Replace(",", "")

                    If Not ((tbPriceEmp.Text = "" Or tbPriceEmp.Text = "0")) And ((tbPriceEmp.Text <> lbLastPriceEmp.Text) Or (tbPriceContractor.Text <> lbLastPriceContractor.Text)) Then
                        SQLString = "EXEC S_MsExpedisiPriceUpdate " + QuotedStr(Format(tbDate.SelectedDate, "yyyy-MM-dd")) + ", " + _
                            QuotedStr(lbCode.Text) + ", " + tbPriceEmp.Text + ", " + tbPriceContractor.Text + ", " + QuotedStr(ViewState("UserId").ToString)
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
                tempDS = SQLExecuteQuery("EXEC S_MsExpedisiPriceGetDt " + QuotedStr(Format(tbDate.SelectedDate, "yyyyMMdd")) + ", " + QuotedStr(StrFilter), ViewState("DBConnection").ToString)
                GridExport.DataSource = tempDS.Tables(0)
                GridExport.DataBind()
            Catch ex As Exception
                Throw New Exception("Bind Data Error : " + ex.ToString)
            End Try
            ExportGridToExcel("Expedition Price")
        Catch ex As Exception
            Throw New Exception("btn Export Error : " + ex.ToString)
        End Try
    End Sub
End Class
