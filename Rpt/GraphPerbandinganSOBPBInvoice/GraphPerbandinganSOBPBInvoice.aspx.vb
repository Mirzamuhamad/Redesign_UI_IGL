
Partial Class Rpt_GraphPerbandinganSOBPBInvoice_GraphPerbandinganSOBPBInvoice
    Inherits System.Web.UI.Page
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnPrint.Click
        Try
            Session("PrintType") = "Print"
            print()
        Catch ex As Exception
            lbStatus.Text = "btn Print Error : " + ex.ToString
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
        ViewState("ServerDate") = Session(Request.QueryString("KeyId"))("ServerDate")
    End Sub

    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Load

        Try
            If Not IsPostBack Then
                InitProperty()
                ReportGrid.setConnection(ViewState("DBConnection"), ViewState("UserId"))
                FillCombo(ddlStartYear, "EXEC S_GetYear ", False, "Year", "Year", ViewState("DBConnection").ToString)
                FillCombo(ddlEndYear, "EXEC S_GetYear ", False, "Year", "Year", ViewState("DBConnection").ToString)
                FillCombo(ddlStartMonth, "SELECT [Period], CONVERT(VARCHAR(2),[Period]) + ' - ' + [Description] AS Description From GLPeriod WHERE [Period] NOT IN (0,13)", False, "Period", "Description", ViewState("DBConnection").ToString)
                FillCombo(ddlEndMonth, "SELECT [Period], CONVERT(VARCHAR(2),[Period]) + ' - ' + [Description] AS Description From GLPeriod WHERE [Period] NOT IN (0,13)", False, "Period", "Description", ViewState("DBConnection").ToString)
                ReportGrid.SetRpt("GraphPerbandinganSOBPBInvoice")
                BindToDropList(ddlStartYear, ViewState("GLYear").ToString)
                BindToDropList(ddlEndYear, ViewState("GLYear").ToString)
                BindToDropList(ddlStartMonth, ViewState("GLPeriod").ToString)
                BindToDropList(ddlEndMonth, ViewState("GLPeriod").ToString)
                Me.tbPeriod.Attributes.Add("OnKeyDown", "return PressNumeric();")
            End If
            ReportGrid.setConnection(ViewState("DBConnection"), ViewState("UserId"))
            lbStatus.Text = ""
        Catch ex As Exception
            lbStatus.Text = "Page Load Error : " + ex.ToString
        End Try


    End Sub

    Protected Sub btnPreview_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnPreview.Click
        Try
            Session("PrintType") = "Preview"
            print()
        Catch ex As Exception
            lbStatus.Text = "btn Print Error : " + ex.ToString
        End Try
    End Sub
    Private Sub print()
        Dim Result As String
        Try
            Result = ReportGrid.ResultString
            Session("DBConnection") = ViewState("DBConnection")
            Session("SelectCommand") = "EXEC S_EXERptMNGSOBPBInvoice " + ddlStartYear.SelectedValue + ", " + ddlStartMonth.SelectedValue + "," + ddlPeriod.SelectedValue.ToString + ", " + tbPeriod.Text + "," + ddlEndYear.SelectedValue + "," + ddlEndMonth.SelectedValue + ",'" + Result + "','',''," + RBType.SelectedIndex.ToString + "," + QuotedStr(ViewState("UserId").ToString)
            'lbStatus.Text = Session("SelectCommand")
            'Exit Sub
            Session("ReportFile") = Server.MapPath("~\Rpt\RptPrintGraph2.frx")
            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub tbPeriod_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPeriod.TextChanged, ddlPeriod.SelectedIndexChanged, ddlStartMonth.SelectedIndexChanged, ddlStartYear.SelectedIndexChanged
        Dim range, tahun, bulan, tahun2, bulan2 As Integer
        Try
            If (CInt(tbPeriod.Text) = 0) Then
                tbPeriod.Text = "1"
            End If
            range = (CInt(ddlPeriod.SelectedValue) * CInt(tbPeriod.Text)) - 1
            tahun = CInt(ddlStartYear.SelectedValue)
            bulan = CInt(ddlStartMonth.SelectedValue)
            bulan2 = bulan + range
            If bulan2 > 24 Then
                bulan2 = bulan2 - 24
                tahun2 = tahun + 2
            ElseIf bulan2 > 12 Then
                bulan2 = bulan2 - 12
                tahun2 = tahun + 1
            Else
                tahun2 = tahun
            End If
            ddlEndYear.SelectedValue = tahun2.ToString
            ddlEndMonth.SelectedValue = bulan2.ToString
        Catch ex As Exception
            lbStatus.Text = "tbPeriod_TextChanged Error : " + ex.ToString
        End Try
    End Sub
End Class
