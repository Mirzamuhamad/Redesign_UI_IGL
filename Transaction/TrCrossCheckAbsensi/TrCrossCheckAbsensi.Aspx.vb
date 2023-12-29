Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Partial Class Transaction_TrCrossCheckAbsensi
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
                btnAdd.Visible = False
                InitProperty()
                Dim dt As DataTable
                dt = SQLExecuteQuery("Select RptTitle, MenuName, COALESCE(MenuParam,'') AS MenuParam, COALESCE(FastRpt,'') AS FastRpt, COALESCE(ReportType,'') AS ReportType, COALESCE(ReportType2,'') AS ReportType2, COALESCE(FgPrintValue,'N') AS FgPrintValue, COALESCE(FgForceNewPage,'N') AS FgForceNewPage from MsMenu WHERE MenuId = " + QuotedStr(Request.QueryString("ContainerId").ToString), ViewState("DBConnection").ToString).Tables(0)
                lbltitle.Text = dt.Rows(0)("MenuName").ToString
                FillCombo(ddlDivisi, "EXEC S_GetPlantDivision", False, "DivisionCode", "DivisionName", ViewState("DBConnection"))
                tbDate.SelectedDate = ViewState("ServerDate") 'Today
                tbDate2.SelectedDate = ViewState("ServerDate") 'Today

                DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
                DataGrid1.PageSize = CInt(ViewState("PageSizeGrid"))

            End If
            If Not Session("Result") Is Nothing Then
                'If ViewState("Sender") = "btnweek" Then
                '    'TransNmbr, JobPlant, JobPlant_Name, Rotasi, Team, Person
                '    BindToText(tbWeek, Session("Result")(0).ToString)
                'End If
                BindData()
                BindDataViewBottom()
                Session("filter") = Nothing
                Session("Column") = Nothing
                Session("Result") = Nothing
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
            'SQLString = "EXEC S_PLWOPriorityDtTop " + QuotedStr(ddlDivisi.SelectedValue) + ", '', " + QuotedStr(StrFilter)
            SQLString = "EXEC S_PLCrossCheckAbsensi  " + QuotedStr(tbDate.SelectedDate) + ", " + QuotedStr(tbDate2.SelectedDate) + ", " + QuotedStr(ddlDivisi.SelectedValue) + ", 'N' "

            tempDS = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString)

            DataGrid.DataSource = tempDS.Tables(0)
            DataGrid.DataBind()

            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "WONo DESC"
            End If
            PnlTop.Visible = True

        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

    Sub BindDataViewBottom()
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
            'SQLString = "EXEC S_PLWOPriorityDtTop " + QuotedStr(ddlDivisi.SelectedValue) + ", '', " + QuotedStr(StrFilter)
            SQLString = "EXEC S_PLCrossCheckAbsensi  " + QuotedStr(tbDate.SelectedDate) + ", " + QuotedStr(tbDate2.SelectedDate) + ", " + QuotedStr(ddlDivisi.SelectedValue) + ", 'Y' "
            tempDS = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString)

            DataGrid1.DataSource = tempDS.Tables(0)
            DataGrid1.DataBind()

            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "WONo DESC"
            End If
            PnlBottom.Visible = True
            btnAdd.Visible = False

        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub bindDataGetTop()
        Try
            Dim GVR As GridViewRow
            Dim CB As CheckBox
            Dim lbWONo, lbTopWeek As Label
            Dim HaveSelect As Boolean

            Dim SQLString, Hasil As String
            HaveSelect = False
            For Each GVR In DataGrid.Rows
                CB = GVR.FindControl("cbSelect")
                lbWONo = GVR.FindControl("WONo")
                lbTopWeek = GVR.FindControl("TopWeek")

                If CB.Checked Then
                    HaveSelect = True
                    SQLString = "DECLARE @A VARCHAR(255) EXEC S_PLWOPriorityTOPPriority " + QuotedStr(lbWONo.Text) + _
                    ", " + QuotedStr(lbTopWeek.Text) + ", " + QuotedStr(ViewState("UserId")) + ", ''" + _
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
                lbstatus.Text = "Please Check selected Reference"
                Exit Sub
            Else
                lbstatus.Text = "Process Success "
            End If

        Catch ex As Exception
            Throw New Exception("bindDataProcess Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub bindDataAdd()
        Try
            Dim GVR As GridViewRow
            Dim CB As CheckBox
            Dim lbWONo, lbTopWeek, lbBlock, lbJob, lbRotasi As Label
            Dim HaveSelect As Boolean

            Dim SQLString, Hasil As String
            HaveSelect = False
            For Each GVR In DataGrid1.Rows
                CB = GVR.FindControl("cbSelect")
                lbWONo = GVR.FindControl("WONo")
                lbTopWeek = GVR.FindControl("TopWeek")

                If CB.Checked Then
                    HaveSelect = True
                    SQLString = "DECLARE @A VARCHAR(255) EXEC S_PLWOPriorityTOPPriority " + QuotedStr(lbWONo.Text) + _
                    ", " + QuotedStr(lbTopWeek.Text) + ", " + QuotedStr(ViewState("UserId")) + ", ''" + _
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
            BindDataViewBottom()
            If HaveSelect = False Then
                lbstatus.Text = "Please Check selected Reference"
                Exit Sub
            Else
                lbstatus.Text = "Process Success "
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

            Else
                DataGrid1.PageIndex = 0
                DataGrid1.EditIndex = -1
                BindDataViewBottom()
            End If
            'BindDataViewBottom()
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

    Protected Sub DataGrid1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGrid.PageIndexChanging
        DataGrid1.PageIndex = e.NewPageIndex
        BindDataViewBottom()
    End Sub


    Protected Sub ddlYear_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDivisi.SelectedIndexChanged
        Try
            If Menu1.Items.Item(0).Selected = True Then
                BindData()

            Else

                BindDataViewBottom()
            End If

            'BindDataViewBottom()
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

    Protected Sub btnConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConfirm.Click
        Dim GVR As GridViewRow
        Dim CB As CheckBox
        Dim LKM1, LKM2, LKM3, LKM4, LKM5, LKM6, LKM7, LKM8, LKM9, LKM10, LKM11, LKM12, LKM13, LKM14 As DropDownList
        Dim Abs1, Abs2, Abs3, Abs4, Abs5, Abs6, Abs7, Abs8, Abs9, Abs10, Abs11, Abs12, Abs13, Abs14 As Label
        Dim EmpNumb As Label
        Dim HaveSelect As Boolean

        Dim SQLString, Hasil As String
        Try
            HaveSelect = False
            For Each GVR In DataGrid.Rows
                CB = GVR.FindControl("cbSelect")
                EmpNumb = GVR.FindControl("Employee")

                Abs1 = GVR.FindControl("Decision1")
                Abs2 = GVR.FindControl("Decision2")
                Abs3 = GVR.FindControl("Decision3")
                Abs4 = GVR.FindControl("Decision4")
                Abs5 = GVR.FindControl("Decision5")
                Abs6 = GVR.FindControl("Decision6")
                Abs7 = GVR.FindControl("Decision7")
                Abs8 = GVR.FindControl("Decision8")
                Abs9 = GVR.FindControl("Decision9")
                Abs10 = GVR.FindControl("Decision10")
                Abs11 = GVR.FindControl("Decision11")
                Abs12 = GVR.FindControl("Decision12")
                Abs13 = GVR.FindControl("Decision13")
                Abs14 = GVR.FindControl("Decision14")

                LKM1 = GVR.FindControl("ddlLKM1")
                LKM2 = GVR.FindControl("ddlLKM2")
                LKM3 = GVR.FindControl("ddlLKM3")
                LKM4 = GVR.FindControl("ddlLKM4")
                LKM5 = GVR.FindControl("ddlLKM5")
                LKM6 = GVR.FindControl("ddlLKM6")
                LKM7 = GVR.FindControl("ddlLKM7")
                LKM8 = GVR.FindControl("ddlLKM8")
                LKM9 = GVR.FindControl("ddlLKM9")
                LKM10 = GVR.FindControl("ddlLKM10")
                LKM11 = GVR.FindControl("ddlLKM11")
                LKM12 = GVR.FindControl("ddlLKM12")
                LKM13 = GVR.FindControl("ddlLKM13")
                LKM14 = GVR.FindControl("ddlLKM14")
 

                If CB.Checked Then
                    HaveSelect = True
                    SQLString = "EXEC S_PLCrossCheckInput  " + QuotedStr(EmpNumb.Text) + ", " + QuotedStr(tbDate.SelectedDate) + ", " + QuotedStr(Abs1.Text) + ", " + QuotedStr(Abs2.Text) + _
                    ", " + QuotedStr(Abs3.Text) + ", " + QuotedStr(Abs4.Text) + ", " + QuotedStr(Abs5.Text) + "," + QuotedStr(Abs6.Text) + _
                    ", " + QuotedStr(Abs7.Text) + ", " + QuotedStr(Abs8.Text) + ", " + QuotedStr(Abs9.Text) + "," + QuotedStr(Abs10.Text) + _
                    ", " + QuotedStr(Abs11.Text) + ", " + QuotedStr(Abs12.Text) + ", " + QuotedStr(Abs13.Text) + "," + QuotedStr(Abs14.Text) + _
                    ", " + QuotedStr(LKM1.SelectedValue) + ", " + QuotedStr(LKM2.SelectedValue) + ", " + QuotedStr(LKM3.SelectedValue) + "," + QuotedStr(LKM4.SelectedValue) + _
                    ", " + QuotedStr(LKM5.SelectedValue) + ", " + QuotedStr(LKM6.SelectedValue) + ", " + QuotedStr(LKM7.SelectedValue) + "," + QuotedStr(LKM8.SelectedValue) + _
                    ", " + QuotedStr(LKM9.SelectedValue) + ", " + QuotedStr(LKM10.SelectedValue) + ", " + QuotedStr(LKM11.SelectedValue) + "," + QuotedStr(LKM12.SelectedValue) + _
                    ", " + QuotedStr(LKM13.SelectedValue) + ", " + QuotedStr(LKM14.SelectedValue) + _
                    "," + QuotedStr(ViewState("UserId")) + " "
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
                lbstatus.Text = "Please Check selected Reference"
                Exit Sub
            Else
                lbstatus.Text = "Process Success "
            End If


        Catch ex As Exception
            lbstatus.Text = "btnConfirm_Click " + vbCrLf + ex.ToString
        End Try

    End Sub

    Protected Sub btnUnConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUnConfirm.Click
        

        Dim GVR As GridViewRow
        Dim CB As CheckBox
        Dim LKM1, LKM2, LKM3, LKM4, LKM5, LKM6, LKM7, LKM8, LKM9, LKM10, LKM11, LKM12, LKM13, LKM14 As DropDownList
        Dim Abs1, Abs2, Abs3, Abs4, Abs5, Abs6, Abs7, Abs8, Abs9, Abs10, Abs11, Abs12, Abs13, Abs14 As Label
        Dim EmpNumb As Label
        Dim HaveSelect As Boolean

        Dim SQLString, Hasil As String
        Try
            HaveSelect = False
            For Each GVR In DataGrid1.Rows
                CB = GVR.FindControl("cbSelect")
                EmpNumb = GVR.FindControl("Employee")

                Abs1 = GVR.FindControl("Decision1")
                Abs2 = GVR.FindControl("Decision2")
                Abs3 = GVR.FindControl("Decision3")
                Abs4 = GVR.FindControl("Decision4")
                Abs5 = GVR.FindControl("Decision5")
                Abs6 = GVR.FindControl("Decision6")
                Abs7 = GVR.FindControl("Decision7")
                Abs8 = GVR.FindControl("Decision8")
                Abs9 = GVR.FindControl("Decision9")
                Abs10 = GVR.FindControl("Decision10")
                Abs11 = GVR.FindControl("Decision11")
                Abs12 = GVR.FindControl("Decision12")
                Abs13 = GVR.FindControl("Decision13")
                Abs14 = GVR.FindControl("Decision14")

                LKM1 = GVR.FindControl("ddlLKM1")
                LKM2 = GVR.FindControl("ddlLKM2")
                LKM3 = GVR.FindControl("ddlLKM3")
                LKM4 = GVR.FindControl("ddlLKM4")
                LKM5 = GVR.FindControl("ddlLKM5")
                LKM6 = GVR.FindControl("ddlLKM6")
                LKM7 = GVR.FindControl("ddlLKM7")
                LKM8 = GVR.FindControl("ddlLKM8")
                LKM9 = GVR.FindControl("ddlLKM9")
                LKM10 = GVR.FindControl("ddlLKM10")
                LKM11 = GVR.FindControl("ddlLKM11")
                LKM12 = GVR.FindControl("ddlLKM12")
                LKM13 = GVR.FindControl("ddlLKM13")
                LKM14 = GVR.FindControl("ddlLKM14")

                If CB.Checked Then
                    HaveSelect = True
                    SQLString = "EXEC S_PLCrossCheckInput " + QuotedStr(EmpNumb.Text) + ", " + QuotedStr(tbDate.SelectedDate) + _
                    ",'N', 'N','N' ,'N' ,'N' ,'N' ,'N' ,'N' ,'N' ,'N' ,'N' ,'N' ,'N' ,'N', 'N', 'N','N' ,'N' ,'N' ,'N' ,'N' ,'N' ,'N' ,'N' ,'N' ,'N' ,'N' ,'N', " + QuotedStr(ViewState("UserId")) + "  "
                    SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                    Hasil = SQLExecuteScalar(SQLString, ViewState("DBConnection").ToString)
                    Hasil = Replace(Hasil, "0", "")

                    If Trim(Hasil) <> "" Then
                        lbstatus.Text = MessageDlg(Hasil)
                        ' Exit For
                    End If
                End If
            Next
            BindDataViewBottom()
            If HaveSelect = False Then
                lbstatus.Text = "Please Check selected Reference"
                Exit Sub
            Else
                lbstatus.Text = "Process Success"
            End If

        Catch ex As Exception
            lbstatus.Text = "btnUnConfirm_Click " + vbCrLf + ex.ToString
        End Try

    End Sub

    'Protected Sub BtnBottom_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnBottom.Click
    '    Try
    '        bindDataBottom()
    '    Catch ex As Exception
    '        lbstatus.Text = "BtnBottom_Click " + vbCrLf + ex.ToString
    '    End Try
    'End Sub

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        MV1.ActiveViewIndex = Int32.Parse(e.Item.Value)
        If Menu1.Items(0).Selected = True Then
            ' BtnBottom.Visible = True
            BindData()

        Else
            '  btnAdd.Visible = True
            BindDataViewBottom()

        End If


    End Sub

    'Protected Sub btnweek_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnweek.Click
    '    Dim ResultField As String
    '    Try
    '        Session("filter") = "SELECT * FROM MsWeek"
    '        ResultField = "WeekNo"
    '        ViewState("Sender") = "btnweek"
    '        Session("Column") = ResultField.Split(",")
    '        Session("DBConnection") = ViewState("DBConnection")
    '        AttachScript("OpenSearchDlg();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lbstatus.Text = "btnweek_Click Error : " + ex.ToString
    '    End Try
    'End Sub

End Class