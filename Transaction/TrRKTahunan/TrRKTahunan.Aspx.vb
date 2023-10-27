Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Partial Class Transaction_TrRKTahunan_TrRKTahunan
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                Dim dt As DataTable
                dt = SQLExecuteQuery("Select RptTitle, MenuName, COALESCE(MenuParam,'') AS MenuParam, COALESCE(FastRpt,'') AS FastRpt, COALESCE(ReportType,'') AS ReportType, COALESCE(ReportType2,'') AS ReportType2, COALESCE(FgPrintValue,'N') AS FgPrintValue, COALESCE(FgForceNewPage,'N') AS FgForceNewPage from MsMenu WHERE MenuId = " + QuotedStr(Request.QueryString("ContainerId").ToString), ViewState("DBConnection").ToString).Tables(0)
                lbltitle.Text = dt.Rows(0)("MenuName").ToString
                ViewState("GLYear") = SQLExecuteScalar("SELECT Year FROM GLYear WHERE CurrentYear='Y'", ViewState("DBConnection").ToString)
                FillCombo(ddlYear, "EXEC S_GetYear", False, "Year", "Year", ViewState("DBConnection"))
                ddlYear.SelectedValue = ViewState("GLYear")
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
        Dim StrFilter, StrFilter1 As String

        Try
            If ddlOpsi.SelectedValue = "All" Then
                StrFilter1 = " Where YEAR(EndDate) = " + QuotedStr(ddlYear.SelectedValue)
            Else
                StrFilter1 = " Where YEAR(EndDate) = " + QuotedStr(ddlYear.SelectedValue) + " AND DoneClosing = " + QuotedStr(ddlOpsi.SelectedValue)
            End If

            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            StrFilter = StrFilter
            
            If StrFilter.Length > 5 Then
                StrFilter1 = StrFilter1.Remove(1, 5)
                StrFilter = StrFilter + " And " + StrFilter1
                If ddlOpsi.SelectedValue = "All" Then
                    tempDS = SQLExecuteQuery("EXEC S_PLPlanOutstanding " + QuotedStr(StrFilter), ViewState("DBConnection").ToString)
                Else
                    tempDS = SQLExecuteQuery("EXEC S_PLPlanOutstanding " + QuotedStr(StrFilter), ViewState("DBConnection").ToString)
                End If
            Else
                If ddlOpsi.SelectedValue = "All" Then
                    tempDS = SQLExecuteQuery("EXEC S_PLPlanOutstanding " + QuotedStr(StrFilter1), ViewState("DBConnection").ToString)
                Else
                    tempDS = SQLExecuteQuery("EXEC S_PLPlanOutstanding " + QuotedStr(StrFilter1), ViewState("DBConnection").ToString)
                End If
            End If

            DataGrid.DataSource = tempDS.Tables(0)
            DataGrid.DataBind()

            'WHERE YEAR(EndDate) = " + Trim(ddlYear.SelectedValue) + ", AND DoneClosing = " + Trim(ddlOpsi.SelectedValue) + ", " 
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "Reference DESC"
            End If
            pnlClosing.Visible = True

        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub BtnApply_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnApply.Click
        Try
            bindDataProcess()
            ' BindData(ddlComp.SelectedValue )
        Catch ex As Exception
            lbstatus.Text = "btn apply error : " + ex.ToString
        End Try
    End Sub
    'Protected Sub BtnUnApply_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnUnApply.Click
    '    Try
    '        If ddlComp.SelectedValue = "Asisten" Then
    '            bindDataUnProcess()
    '        ElseIf ddlComp.SelectedValue = "Audit" Then
    '            bindDataUnProcessAudit()
    '        Else
    '            bindDataUnProcessDenda()
    '        End If

    '        ' BindData(ddlComp.SelectedValue )
    '    Catch ex As Exception
    '        lbstatus.Text = "btn apply error : " + ex.ToString
    '    End Try
    'End Sub
    Private Sub bindDataProcess()
        Try
            Dim GVR As GridViewRow
            Dim CB As CheckBox
            Dim lbType, lbReference, lbBlock, lbJob, lbRotasi, lbRemark As Label
            Dim HaveSelect As Boolean

            Dim SQLString, Hasil As String
            HaveSelect = False
            For Each GVR In DataGrid.Rows
                CB = GVR.FindControl("cbSelect")
                lbtype = GVR.FindControl("Type")
                lbReference = GVR.FindControl("Reference")
                lbJob = GVR.FindControl("Job")
                lbBlock = GVR.FindControl("DivisiBlok")
                lbRotasi = GVR.FindControl("Rotasi")
                'lbRemark = GVR.FindControl("Remark")

                If CB.Checked Then
                    HaveSelect = True
                    SQLString = "DECLARE @A VARCHAR(255) EXEC S_PLPlanOutClosing " + QuotedStr(lbType.Text) + _
                    ", " + QuotedStr(lbReference.Text) + ", " + QuotedStr(lbJob.Text) + ", " + QuotedStr(lbBlock.Text) + _
                    ", " + QuotedStr(lbRotasi.Text) + ", ''" + _
                    ", " + QuotedStr(ViewState("UserId").ToString) + ", @A OUT SELECT @A "

                    SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                    Hasil = SQLExecuteScalar(SQLString, ViewState("DBConnection").ToString)
                    Hasil = Replace(Hasil, "0", "")
                    'lbstatus.Text = MessageDlg(Hasil)
                    'Exit Sub

                    If Trim(Hasil) <> "" Then
                        lbstatus.Text = MessageDlg(Hasil)
                        Exit For
                    End If
                End If
            Next
            BindData()
            If HaveSelect = False Then
                lbstatus.Text = "Please Check selected Reference"
                Exit Sub
            
            End If

        Catch ex As Exception
            Throw New Exception("bindDataProcess Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub bindDataUnProcess()
        Try
            'Dim GVR As GridViewRow
            'Dim CB As CheckBox
            'Dim lbTrans, lbWONO, lbtype, lbBlock, lbQtyLKM As Label
            'Dim HaveSelect As Boolean

            'Dim SQLString, Hasil As String
            'HaveSelect = False
            'For Each GVR In DataGrid.Rows
            '    CB = GVR.FindControl("cbSelect")
            '    lbTrans = GVR.FindControl("TransNmbr")
            '    lbWONO = GVR.FindControl("WONo")
            '    lbtype = GVR.FindControl("Type")
            '    lbBlock = GVR.FindControl("DivisiBlock")
            '    lbQtyLKM = GVR.FindControl("QtyLKM")


            '    If CB.Checked Then
            '        HaveSelect = True
            '        SQLString = "DECLARE @A VARCHAR(255) EXEC S_PLWOResultUnCompleteAsisten " + QuotedStr(lbTrans.Text) + _
            '        ", " + QuotedStr(lbWONO.Text) + ", " + QuotedStr(lbtype.Text) + ", " + QuotedStr(lbBlock.Text) + _
            '        ", " + ViewState("GLYear").ToString + ", " + ViewState("GLPeriod").ToString + _
            '        ", " + QuotedStr(ViewState("UserId").ToString) + ", @A OUT SELECT @A "

            '        ' SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            '        Hasil = SQLExecuteScalar(SQLString, ViewState("DBConnection").ToString)
            '        Hasil = Replace(Hasil, "0", "")

            '        If Trim(Hasil) <> "" Then
            '            lbstatus.Text = MessageDlg(Hasil)
            '            Exit For
            '        End If
            '    End If
            'Next
            'BindData()
            'If HaveSelect = False Then
            '    lbstatus.Text = "Please Check Un Complete"
            '    Exit Sub
            'Else
            '    lbstatus.Text = "Un Process Success "
            'End If

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

    Protected Sub ddlOpsi_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOpsi.SelectedIndexChanged
        Try
            BindData()
        Catch ex As Exception
            lbstatus.Text = "ddlOpsi_SelectedIndexChanged " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub ddlYear_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlYear.SelectedIndexChanged
        Try
            BindData()
        Catch ex As Exception
            lbstatus.Text = "ddlYear_SelectedIndexChanged " + vbCrLf + ex.ToString
        End Try
    End Sub
End Class