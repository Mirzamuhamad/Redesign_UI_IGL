Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Partial Class Transaction_TrRKBulanan_TrRKBulanan
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Try
            If Not IsPostBack Then
                MV1.Visible = True
                MV1.ActiveViewIndex = 0
                Menu1.Items.Item(0).Selected = True
                BtnUnFixMonth.Visible = False
                'BtnFixMonth.Visible = False
                InitProperty()
                Dim dt As DataTable
                dt = SQLExecuteQuery("Select RptTitle, MenuName, COALESCE(MenuParam,'') AS MenuParam, COALESCE(FastRpt,'') AS FastRpt, COALESCE(ReportType,'') AS ReportType, COALESCE(ReportType2,'') AS ReportType2, COALESCE(FgPrintValue,'N') AS FgPrintValue, COALESCE(FgForceNewPage,'N') AS FgForceNewPage from MsMenu WHERE MenuId = " + QuotedStr(Request.QueryString("ContainerId").ToString), ViewState("DBConnection").ToString).Tables(0)
                lbltitle.Text = dt.Rows(0)("MenuName").ToString
                ViewState("GLYear") = SQLExecuteScalar("SELECT Year FROM GLYear WHERE CurrentYear='Y'", ViewState("DBConnection").ToString)
                FillCombo(ddlYear, "EXEC S_GetYear", False, "Year", "Year", ViewState("DBConnection"))
                ddlYear.SelectedValue = ViewState("GLYear")
                FillCombo(ddlMonth, "EXEC S_GetPeriod", False, "Period", "Description", ViewState("DBConnection"))
                DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
                DataGrid1.PageSize = CInt(ViewState("PageSizeGrid"))
                ddlYear.SelectedValue = ViewState("GLYear")
                ddlMonth.SelectedValue = ViewState("GLPeriod")
                BindData()
                BindDataAvailable()
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
        Dim SQLString As String
        Try
            
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            StrFilter = StrFilter

            If StrFilter.Length > 5 Then
                StrFilter = StrFilter.Remove(1, 5)
                StrFilter = " And " + StrFilter
            End If

            SQLString = "EXEC S_PLRKBulananAssign " + QuotedStr(ddlYear.SelectedValue) + ", " + QuotedStr(ddlMonth.SelectedValue) + ", " + QuotedStr(StrFilter)
            tempDS = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString)

            DataGrid.DataSource = tempDS.Tables(0)
            DataGrid.DataBind()

            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "Reference DESC"
            End If
            pnlAssign.Visible = True

        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

    Sub BindDataAvailable()
        Dim tempDS As New DataSet()
        Dim StrFilter As String
        Dim SQLString As String
        Try

            'StrFilter1 = " Where YEAR(EndDate) = " + QuotedStr(ddlYear.SelectedValue)
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            StrFilter = StrFilter

            If StrFilter.Length > 5 Then
                StrFilter = StrFilter.Remove(1, 5)
                StrFilter = " And " + StrFilter
            End If

            SQLString = "EXEC S_PLRKBulananAvailable " + QuotedStr(StrFilter)
            ' lbstatus.Text = "disni" + SQLString
            ' Exit Sub 
            tempDS = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString)

            DataGrid1.DataSource = tempDS.Tables(0)
            DataGrid1.DataBind()

            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "Reference DESC"
            End If
            PnlAvailable.Visible = True

        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

    Sub BindDataFixedMonth()
        Dim tempDS As New DataSet()
        Dim SQLString As String
        Try

            SQLString = "EXEC S_PLRKBulananFixedMonth " + QuotedStr(ddlMonth.SelectedValue) + ", " + QuotedStr(ddlYear.SelectedValue)
            ' lbstatus.Text = "disni" + SQLString
            ' Exit Sub
            tempDS = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString)

            DataGrid1.DataSource = tempDS.Tables(0)
            DataGrid1.DataBind()

            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "Reference DESC"
            End If
            PnlAvailable.Visible = True

            Dim GVR As GridViewRow
            Dim CB As CheckBox

            For Each GVR In DataGrid1.Rows
                CB = GVR.FindControl("cbSelect")
                CB.Checked = True
            Next

        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub


    Private Sub bindDataRemove()
        Try
            Dim GVR As GridViewRow
            Dim CB As CheckBox
            Dim lbType, lbReference, lbBlock, lbJob, lbRotasi As Label
            Dim HaveSelect As Boolean

            Dim SQLString, Hasil As String
            HaveSelect = False
            For Each GVR In DataGrid.Rows
                CB = GVR.FindControl("cbSelect")
                lbType = GVR.FindControl("Type")
                lbReference = GVR.FindControl("Reference")
                lbJob = GVR.FindControl("Job")
                lbBlock = GVR.FindControl("DivisiBlok")
                lbRotasi = GVR.FindControl("Rotasi")

                If CB.Checked Then
                    HaveSelect = True
                    SQLString = "DECLARE @A VARCHAR(255) EXEC S_PLRKBulananRemove " + QuotedStr(lbType.Text) + _
                    ", " + QuotedStr(ddlYear.SelectedValue) + ", " + QuotedStr(ddlMonth.SelectedValue) + ", " + QuotedStr(lbReference.Text) + _
                    ", " + QuotedStr(lbJob.Text) + ", " + QuotedStr(lbRotasi.Text) + ", " + QuotedStr(lbBlock.Text) + _
                    ", @A OUT SELECT @A "
                    SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
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
                lbstatus.Text = MessageDlg("Please Check selected Reference")
                Exit Sub
            Else
                lbstatus.Text = MessageDlg("Process Success ")
            End If

        Catch ex As Exception
            Throw New Exception("bindDataProcess Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub bindDataAdd()
        Try
            Dim GVR As GridViewRow
            Dim CB As CheckBox
            Dim lbType, lbReference, lbBlock, lbJob, lbRotasi As Label
            Dim HaveSelect As Boolean

            Dim SQLString, Hasil As String
            HaveSelect = False
            For Each GVR In DataGrid1.Rows
                CB = GVR.FindControl("cbSelect")
                lbType = GVR.FindControl("Type")
                lbReference = GVR.FindControl("Reference")
                lbJob = GVR.FindControl("Job")
                lbBlock = GVR.FindControl("DivisiBlok")
                lbRotasi = GVR.FindControl("Rotasi")

                If CB.Checked Then
                    HaveSelect = True
                    SQLString = "EXEC S_PLRKBulananAdd " + QuotedStr(lbType.Text) + _
                    ", " + QuotedStr(ddlYear.SelectedValue) + ", " + QuotedStr(ddlMonth.SelectedValue) + ", " + QuotedStr(lbReference.Text) + _
                    ", " + QuotedStr(lbJob.Text) + ", " + QuotedStr(lbBlock.Text) + ", " + QuotedStr(lbRotasi.Text)

                    SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                    Hasil = SQLExecuteScalar(SQLString, ViewState("DBConnection").ToString)
                    Hasil = Replace(Hasil, "0", "")

                    If Trim(Hasil) <> "" Then
                        lbstatus.Text = MessageDlg(Hasil)
                        ' Exit For
                    End If

                End If
            Next

            If BtnFixMonth.Visible = False Then
                BindDataFixedMonth()
            Else
                BindDataAvailable()
            End If


            If HaveSelect = False Then
                lbstatus.Text = MessageDlg("Please Check selected Reference")
                Exit Sub
            Else
                lbstatus.Text = MessageDlg("Process Success ")
            End If

        Catch ex As Exception
            Throw New Exception("bindDataAdd Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            If Menu1.Items.Item(0).Selected = True Then
                DataGrid.PageIndex = 0
                DataGrid.EditIndex = -1
                BindData()
            ElseIf BtnFixMonth.Visible = False Then
                DataGrid1.PageIndex = 0
                DataGrid1.EditIndex = -1
                BindDataFixedMonth()
            Else
                DataGrid1.PageIndex = 0
                DataGrid1.EditIndex = -1
                BindDataAvailable()
            End If
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
        CheckAllDt(DataGrid1, sender)
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

    Protected Sub DataGrid1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGrid1.PageIndexChanging
        DataGrid1.PageIndex = e.NewPageIndex
        BindDataAvailable()
    End Sub


    Protected Sub ddlMonth_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMonth.SelectedIndexChanged
        Try
            If BtnFixMonth.Visible = False Then
                BindDataFixedMonth()
            Else
                BindDataAvailable()
            End If
            'BindDataAvailable()
        Catch ex As Exception
            lbstatus.Text = "ddlYear_SelectedIndexChanged " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub ddlYear_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlYear.SelectedIndexChanged
        Try
            If BtnFixMonth.Visible = False Then
                BindDataFixedMonth()
            Else
                BindDataAvailable()
            End If
            'BindDataAvailable()
        Catch ex As Exception
            lbstatus.Text = "ddlYear_SelectedIndexChanged " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Try
            bindDataAdd()
        Catch ex As Exception
            lbstatus.Text = "btnAdd_Click " + vbCrLf + ex.ToString
        End Try

    End Sub

    Protected Sub BtnRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnRemove.Click
        Try
            bindDataRemove()
        Catch ex As Exception
            lbstatus.Text = "BtnRemove_Click " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        MV1.ActiveViewIndex = Int32.Parse(e.Item.Value)
        If Menu1.Items(0).Selected = True Then
            BindData()
            '   BtnRemove.Visible = True
        ElseIf BtnFixMonth.Visible = False Then
            BindDataFixedMonth()
            '  btnAdd.Visible = True
        Else
            BindDataAvailable()
            '  btnAdd.Visible = True
        End If


    End Sub

    Protected Sub BtnFixMonth_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnFixMonth.Click

        Try
            BtnFixMonth.Visible = False
            BtnUnFixMonth.Visible = True

            BindDataFixedMonth()

        Catch ex As Exception
        End Try

    End Sub


    Protected Sub BtnUnFixMonth_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnUnFixMonth.Click
        Try
            BtnFixMonth.Visible = True
            BtnUnFixMonth.Visible = False

            BindDataAvailable()
        Catch ex As Exception

        End Try
    End Sub

End Class