
Partial Class Rpt_RptPenjualanBulanan_RptPenjualanBulanan
    Inherits System.Web.UI.Page
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Try
            Session("PrintType") = "Print"
            print()
        Catch ex As Exception
            lbStatus.Text = "btn Print Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Load
        Try

            If Not IsPostBack Then
                InitProperty()
                ReportGrid.setConnection(ViewState("DBConnection"), ViewState("UserId"))
                ReportGrid.SetRpt("RptPenjualanBulanan")
                FillCombo(ddlMonthStart, "EXEC S_GetPeriod", False, "Period", "Description", ViewState("DBConnection"))
                FillCombo(ddlYearStart, "EXEC S_GetYear", False, "Year", "Year", ViewState("DBConnection"))
                FillCombo(ddlMonthEnd, "EXEC S_GetPeriod", False, "Period", "Description", ViewState("DBConnection"))
                FillCombo(ddlYearEnd, "EXEC S_GetYear", False, "Year", "Year", ViewState("DBConnection"))

                If ddlYearStart.Items.Contains(ddlYearStart.Items.FindByValue(CStr(Now.Year).Trim)) Then
                    ddlYearStart.SelectedValue = CStr(Now.Year).Trim
                End If
                If ddlMonthStart.Items.Contains(ddlMonthStart.Items.FindByValue(CStr(Now.Month).Trim)) Then
                    ddlMonthStart.SelectedValue = CStr(Now.Month).Trim
                End If
                If ddlYearEnd.Items.Contains(ddlYearEnd.Items.FindByValue(CStr(Now.Year).Trim)) Then
                    ddlYearEnd.SelectedValue = CStr(Now.Year).Trim
                End If
                If ddlMonthEnd.Items.Contains(ddlMonthEnd.Items.FindByValue(CStr(Now.Month).Trim)) Then
                    ddlMonthEnd.SelectedValue = CStr(Now.Month).Trim
                End If
            End If
            ReportGrid.setConnection(ViewState("DBConnection"), ViewState("UserId"))
            lbStatus.Text = ""
        Catch ex As Exception
            lbStatus.Text = "Page Load Error : " + ex.ToString
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

    Private Sub print()
        Dim Result, CekPeriod As String
        Dim vQty, vM2, vRoll, vNominal As String
        Try
            CekPeriod = CekRangePeriodSelected(ddlYearStart.SelectedValue, ddlMonthStart.SelectedValue, ddlYearEnd.SelectedValue, ddlMonthEnd.SelectedValue)
            If CekPeriod <> "" Then
                lbStatus.Text = MessageDlg(CekPeriod)
                Exit Sub
            End If
            Result = ReportGrid.ResultString
            vQty = "0"
            vM2 = "0"
            vRoll = "0"
            vNominal = "0"
            
            Dim PrintData As String
            Dim vCount As Integer
            vCount = 0
            For Each Data As ListItem In cbPrintData.Items()
                If (Data.Selected) Then
                    vCount = 1
                    PrintData = Data.Value.ToString
                    If PrintData = "Qty" Then
                        vQty = "1"
                    ElseIf PrintData = "M2" Then
                        vM2 = "1"
                    ElseIf PrintData = "Roll" Then
                        vRoll = "1"
                    ElseIf PrintData = "Nominal" Then
                        vNominal = "1"
                    End If
                End If
            Next

            If vCount = 0 Then
                lbStatus.Text = MessageDlg("Print Data Must Selected")
                Exit Sub
            End If

            Session("DBConnection") = ViewState("DBConnection")
            Session("SelectCommand") = "EXEC S_FNRptCustInvMonth " + ddlYearStart.SelectedValue + ", " + ddlMonthStart.SelectedValue + " , " + ddlYearEnd.SelectedValue + ", " + ddlMonthEnd.SelectedValue + ",'" + Result + "','','',''," + vNominal + "," + vQty + "," + vM2 + ", " + vRoll + "," + rbGroup.SelectedIndex.ToString + "," + QuotedStr(ViewState("UserId").ToString)
   

            If rbGroup.SelectedIndex.ToString = "0" Then
                'Session("SelectCommand") = "EXEC S_FNRptCustInvMonth " + ddlYearStart.SelectedValue + ", " + ddlMonthStart.SelectedValue + " , " + ddlYearEnd.SelectedValue + ", " + ddlMonthEnd.SelectedValue + ",'" + Result + "','','','', " + cbPrintData.SelectedIndex.ToString + "," + cbPrintData.SelectedIndex.ToString + ", " + cbPrintData.SelectedIndex.ToString + "," + cbPrintData.SelectedIndex.ToString + "," + rbGroup.SelectedIndex.ToString + "," + QuotedStr(ViewState("UserId").ToString)
                Session("ReportFile") = Server.MapPath("~\Rpt\RptPenjualanBulanan.frx")
            Else
                'Session("SelectCommand") = "EXEC S_FNRptCustInvMonth " + ddlYearStart.SelectedValue + ", " + ddlMonthStart.SelectedValue + " , " + ddlYearEnd.SelectedValue + ", " + ddlMonthEnd.SelectedValue + ",'" + Result + "','','', '', " + rbGroup.SelectedIndex.ToString + "," + QuotedStr(ViewState("UserId").ToString)
                Session("ReportFile") = Server.MapPath("~\Rpt\RptPenjualanBulananDetail.frx")
            End If
            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            Throw New Exception("print error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPreview.Click
        Try
            Session("PrintType") = "Preview"
            print()
        Catch ex As Exception
            lbStatus.Text = "btn Print Error : " + ex.ToString
        End Try
    End Sub


End Class
