Imports System.Data
Imports System.Data.SqlClient
'Imports System.Data.DataSet
Imports System.IO
Imports System.Web.Services
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls.HtmlInputFile

Partial Class Transaction_TrLandPurchaseAgremt_TrLandPurchaseAgremt
    Inherits System.Web.UI.Page

    Private Shared PageSize As Integer = 10
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "SELECT * FROM V_PRCLandPurchaseTJHd"

    Private Property PostedFile() As HttpPostedFile
        Get
            If Page.Session("postedFile") IsNot Nothing Then
                Return Page.Session("postedFile")
            End If
            Return Nothing
        End Get
        Set(ByVal value As HttpPostedFile)
            Page.Session("postedFile") = value
        End Set
    End Property

    'Private Function GetStringDt(ByVal Nmbr As String) As String
    '    Return "SELECT * FROM V_PRCIPRecvDocDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    'End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        'Dim Rev As Integer
        'Dim Tipe As String
        Dim Payment, PaidOff, LandPrice As String
        Try
            If Not IsPostBack Then
                InitProperty()
                SetInit()
                'BindGridListNoLS("", "")
                'BindGridListNoLS()
                BindGridPembeli()
                'BindGridDocument()
                Session("AdvanceFilter") = ""
            End If
            lbStatus.Text = ""
            'BtnGo.Visible = ddlCommand.Visible
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If

            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnNoLandSurvey" Then
                    tbNoLandSurvey.Text = Session("Result")(0).ToString
                    'BindToDropList(ddlCurrDt, Session("Result")(3).ToString)
                    BindToText(tbHrgM2, Session("Result")(2).ToString, ViewState("DigitHome"))
                    BindToText(tbLuas, Session("Result")(3).ToString, ViewState("DigitHome"))
                    BindToText(tbHrgTanah, Session("Result")(4).ToString, ViewState("DigitHome"))
                    'tbHrgM2.Text = Session("Result")(2).ToString
                    'tbLuas.Text = Session("Result")(3).ToString
                    'tbHrgTanah.Text = Session("Result")(4).ToString
                    tbSellerCode.Text = Session("Result")(5).ToString
                    tbSellerName.Text = Session("Result")(6).ToString
                    tbPembeli.Text = Session("Result")(7).ToString

                    'Rev = SQLExecuteScalar("SELECT MAX(Revisi)AS Revisi FROM PRCLandPurchaseTJHd WHERE Status<>'D' AND LSNo = " + QuotedStr(tbNoLandSurvey.Text), ViewState("DBConnection"))
                    'Tipe = SQLExecuteScalar("SELECT TypeTJ FROM PRCLandPurchaseTJHd WHERE Status='P' AND LSNo = " + QuotedStr(tbNoLandSurvey.Text), ViewState("DBConnection"))
                    ''If Rev > 0 Then
                    'If Tipe = "PL" Then
                    '    ddlType.SelectedValue = "PL"
                    '    tbPelunasan.Enabled = True
                    '    tbPembayaran.Enabled = False
                    'Else
                    '    ddlType.SelectedValue = "TJ"
                    '    tbPembayaran.Enabled = True
                    '    tbPelunasan.Enabled = False
                    'End If
                    LandPrice = SQLExecuteScalar("SELECT TtlHrgTanah FROM GLLandPurchaseReqHd WHERE Status='P' AND TransNmbr = " + QuotedStr(tbNoLandSurvey.Text), ViewState("DBConnection"))
                    Payment = SQLExecuteScalar("SELECT SUM(Pembayaran)AS Pembayaran FROM PRCLandPurchaseTJHd WHERE Status='P' AND LSNo = " + QuotedStr(tbNoLandSurvey.Text), ViewState("DBConnection"))
                    PaidOff = SQLExecuteScalar("SELECT SUM(PayPelunasan)AS Pelunasan FROM PRCLandPurchaseTJHd WHERE Status='P' AND LSNo = " + QuotedStr(tbNoLandSurvey.Text), ViewState("DBConnection"))
                    BindToText(tbPembayaran, Payment, ViewState("DigitHome"))
                    BindToText(tbPelunasan, LandPrice - Payment, ViewState("DigitHome"))

                    '    BindGridDt(ViewState("Dt"), GridDt)
                    '    ModifyInput2(True, pnlInput, PnlDt, GridDt)
                End If
                Session("Result") = Nothing
                ViewState("Sender") = Nothing
                Session("filter") = Nothing
                Session("Column") = Nothing
            End If
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

    Private Sub SetInit()
        Try
            FillRange(ddlRange)
            'FillCombo(ddlTerm, "EXEC S_GetTerm", True, "Term_Code", "Term_Name", ViewState("DBConnection"))
            'FillCombo(ddlUnit, "SELECT Unit_Code, Unit_Name FROM VMsUnit", True, "Unit_Code", "Unit_Code", ViewState("DBConnection"))
            'FillCombo(ddlWakilPembeli, "SELECT TugasCode, TugasName FROM MsPemberiTugas ORDER BY TugasName", True, "TugasCode", "TugasName", ViewState("DBConnection"))
            'FillCombo(ddlSubCategory, "SELECT SubCategoryCode, SubCategoryName FROM V_MsSubCategory ORDER BY SubCategoryName", True, "SubCategoryCode", "SubCategoryName", ViewState("DBConnection"))
            'FillCombo(ddlSubCategory, "SELECT SubCategoryCode, SubCategoryName FROM GetMsSubCategory", True, "SubCategoryCode", "SubCategoryName", ViewState("DBConnection"))
            'FillCombo(ddlProjectType, "SELECT ProjectType, ProjectTypeName FROM MsProjectType WHERE UserId = " + QuotedStr(ViewState("UserId").ToString) + " ORDER BY ProjectTypeName", True, "ProjectType", "ProjectTypeName", ViewState("DBConnection"))
            ViewState("SortExpression") = Nothing
            ViewState("DigitCurr") = 0
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
            If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
                'ddlCommand.Items.Add("Print")
                'ddlCommand2.Items.Add("Print")
            End If
            tbHrgTanah.Attributes.Add("OnBlur", "setformatforhd();")
            tbPembayaran.Attributes.Add("OnBlur", "setformatforhd();")
            tbPelunasan.Attributes.Add("OnBlur", "setformatforhd();")
            'tbRequestByName.Attributes.Add("ReadOnly", "True")
            'tbDocName.Attributes.Add("ReadOnly", "True")
            'tbPeriod.Attributes.Add("ReadOnly", "True")
            tbPembayaran.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbDokumen.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Catch ex As Exception
            Throw New Exception("Set Init Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub GetSelectedRecords()
        Dim Result As New DataTable
        Dim dt As New DataTable()
        Try
            ''dt.Columns.Add("ProductID", GetType(Integer))
            'dt.Columns.Add("DokCode", GetType(String))
            'dt.Columns.Add("DokName", GetType(String))
            'dt.Columns.Add("Remark", GetType(String))
            ''dt.Columns.Add("CategoryID", GetType(Integer))
            ''dt.Columns.Add("UnitPrice", GetType(Decimal))

            'For Each gvRow As GridViewRow In GVDocument.Rows
            '    Dim checkbox As CheckBox = DirectCast(gvRow.Cells(0).FindControl("checkAll"), CheckBox)
            '    If checkbox.Checked Then
            '        Dim Row As DataRow = dt.NewRow()
            '        Row("DokCode") = gvRow.Cells(1).Text
            '        Row("DokName") = gvRow.Cells(2).Text
            '        Row("Remark") = gvRow.Cells(3).Text
            '        'Row("UnitPrice") = gvRow.Cells(4).Text
            '        dt.Rows.Add(row)
            '    End If
            'Next
            'GridDt.DataSource = dt
            'GridDt.DataBind()
            '-------------------------------------------------------------------------------------'
            Result.Columns.AddRange(New DataColumn(2) {New DataColumn("DokCode"), New DataColumn("DokName"), New DataColumn("Remark")})
            For Each Row As GridViewRow In GVDocument.Rows
                If Row.RowType = DataControlRowType.DataRow Then
                    'Dim chkRow As CheckBox = TryCast(Row.Cells(0).FindControl("chkRow"), CheckBox)
                    Dim chkRow As CheckBox = TryCast(Row.FindControl("checkAll"), CheckBox)
                    If chkRow.Checked Then
                        Result.Rows.Add(Row.Cells(1).Text, Row.Cells(2).Text, Row.Cells(3).Text)
                    End If
                End If
            Next
            Session("Result") = Result
            Response.Write("<script language='javascript'> { window.opener.document.form1.submit();  window.close();}</script>")
            GridDt.DataSource = Result
            GridDt.DataBind()
            GridDt.Columns(0).Visible = True
            '    'Response.Write("<script language='javascript'> { window.opener.location.Reload();  window.close();}</script>")
            '    'DirectCast(GridView1.SelectedRow.FindControl("btnSelect"), Button).Attributes.Add("Onclick", "javascript:CloseWindow()")
            '------------------------------------------------------------------------------------------------'
            'Dim drResult, dr As DataRow
            'Dim RowData As DataRow()
            'For Each drResult In Session("Result").Rows
            '    If CekExistData(ViewState("Dt"), "DokCode", drResult("DokCode")) Then
            '        Response.Write("Document Name :  " + drResult("DokName") + " has been already exist")
            '        Exit Sub
            '    Else
            '        RowData = ViewState("Dt").Select("TransNmbr = " + QuotedStr(tbCode.Text))
            '        dr = ViewState("Dt").NewRow
            '        'dr("ItemNo") = drResult("ItemNo")
            '        dr("DokCode") = drResult("DokCode")
            '        dr("DokName") = drResult("DokName")
            '        dr("Remark") = "" 'drResult("Remark")
            '        dr("FgActive") = "Y"
            '        ViewState("Dt").Rows.Add(dr)
            '    End If
            'Next
            'Session("Result") = Result
            '------------------------------------------------------------------------------------------------'
            ''Dim dtSourceData As DataSet = DataSet.Clone()
            'Dim dtSourceData As New DataTable()
            'Dim dtClone As DataTable = dtSourceData.Clone()
            'For Each Row As GridViewRow In GVDocument.Rows
            '    Dim chk As CheckBox = TryCast(Row.FindControl("chkSelect"), CheckBox)
            '    Dim hdValue As HiddenField = TryCast(Row.FindControl("hdValue"), HiddenField)
            '    If (chk.Checked) Then
            '        Dim filteredRows As DataRow() = dtSourceData.Select("DokCode=" + hdValue.Value + "")
            '        For Each drx As DataRow In filteredRows
            '            dtClone.ImportRow(drx)
            '        Next
            '    End If
            'Next
            'GridDt.DataSource = dtClone
            'GridDt.DataBind()
        Catch ex As Exception
            lbStatus.Text = "btnApply_Click Error " + ex.ToString
        End Try
        'BindGridDt(ViewState("Dt"), GridDt)
    End Sub

    'Protected Sub GetSelectedRecords()
    '    Dim dt As New DataTable()
    '    'Dim dr As DataRow
    '    'Dim RowData As DataRow()
    '    dt.Columns.AddRange(New DataColumn(2) {New DataColumn("DokCode"), New DataColumn("DokName"), New DataColumn("Remark")})
    '    For Each Row As GridViewRow In GVDocument.Rows
    '        If Row.RowType = DataControlRowType.DataRow Then
    '            Dim chkRow As CheckBox = TryCast(Row.Cells(0).FindControl("chkRow"), CheckBox)
    '            If chkRow.Checked Then
    '                Dim sName As String = TryCast(Row.Cells(2).FindControl("lblDokCode"), Label).Text
    '                dt.Rows.Add(Row.Cells(1).Text, Row.Cells(2).Text, Row.Cells(3).Text)
    '            End If
    '        End If
    '        GridDt.DataSource = dt
    '        GridDt.DataBind()
    '    Next
    '    'BindGridDt(ViewState("Dt"), GridDt)
    'End Sub

    Private Sub BindGridNoLandSurvey(ByVal SearchTerm As String, ByVal PageIndex As Integer, ByVal PageSize As Integer)
        Dim adp As New SqlDataAdapter()
        'Dim dt As New DataTable()
        'Dim conStr As String = ViewState("DBConnection").ToString
        Try
            Dim strSQL = "S_PRCListNoLandSurvey"
            Dim conStr As String = ViewState("DBConnection").ToString
            Using con As New SqlConnection(conStr)
                'con.Open()
                Using cmd As New SqlCommand(strSQL, con)
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@SearchTerm", SearchTerm)
                        cmd.Parameters.AddWithValue("@PageIndex", PageIndex)
                        cmd.Parameters.AddWithValue("@PageSize", PageSize)
                        cmd.Parameters.Add("@RecordCount", SqlDbType.Int, 4).Direction = ParameterDirection.Output
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            GVListNoLS.DataSource = dt
                            GVListNoLS.DataBind()
                            'GVNoLandSurvey.DataSource = dt
                            'GVNoLandSurvey.DataBind()
                        End Using
                        'con.Close()
                    End Using
                End Using
            End Using
        Catch ex As Exception
            'ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "alert('Error occured : " & ex.Message.ToString() & "');", True)
        Finally
            'dt.Clear()
            'dt.Dispose()
            adp.Dispose()
            con.Close()
        End Try
    End Sub

    'Private Sub BindGridListNoLS(ByVal searchBy As String, ByVal searchVal As String)
    Private Sub BindGridListNoLS()
        'Dim conStr As String = ViewState("DBConnection").ToString
        'Using con As New SqlConnection(conStr)
        '    Using cmd As New SqlCommand("SELECT TransNmbr, TransDate, HrgTanah, LuasUkur, TtlHrgTanah, SellCode, SellName FROM V_GLLandPurchaseReqHd WHERE Status<>'D' ORDER BY TransNmbr") ' WHERE FgActive='Y' ")
        '        Using sda As New SqlDataAdapter()
        '            cmd.Connection = con
        '            sda.SelectCommand = cmd
        '            Using dt As New DataTable()
        '                sda.Fill(dt)
        '                GVKegiatan.DataSource = dt
        '                GVKegiatan.DataBind()
        '            End Using
        '        End Using
        '    End Using
        'End Using
        '-----------------------------------------------------------------------------'
        Dim strSQL = "S_PRCListNoLandSurvey"
        Dim conStr As String = ViewState("DBConnection").ToString
        Using con As New SqlConnection(conStr)
            'con.Open()
            Using cmd As New SqlCommand(strSQL, con)
                Using sda As New SqlDataAdapter()
                    cmd.Connection = con
                    cmd.CommandType = CommandType.StoredProcedure
                    'cmd.Parameters.AddWithValue("@SearchBy", searchBy)
                    'cmd.Parameters.AddWithValue("@SearchVal", searchVal)
                    sda.SelectCommand = cmd
                    Using dt As New DataTable()
                        sda.Fill(dt)
                        GVListNoLS.DataSource = dt
                        GVListNoLS.DataBind()
                    End Using
                    'con.Close()
                End Using
            End Using
        End Using
    End Sub

    Protected Sub Search_GridView()
        Dim adp As New SqlDataAdapter()
        Dim conStr As String = ViewState("DBConnection").ToString
        Try
            Dim strSQL = "S_PRCListNoLandSurvey"
            Using con As New SqlConnection(conStr)
                Using cmd As New SqlCommand(strSQL, con)
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@SearchTerm", txtSearchListNoLS.Text)
                        cmd.Parameters.AddWithValue("@PageIndex", 1)
                        cmd.Parameters.AddWithValue("@PageSize", 10)
                        cmd.Parameters.Add("@RecordCount", SqlDbType.Int, 4).Direction = ParameterDirection.Output
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            GVListNoLS.DataSource = dt
                            GVListNoLS.DataBind()
                        End Using
                    End Using
                End Using
            End Using
        Catch ex As Exception
            'ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "alert('Error occured : " & ex.Message.ToString() & "');", True)
        Finally
            adp.Dispose()
            con.Close()
        End Try
    End Sub

    Private Sub BindGridPembeli()
        Dim conStr As String = ViewState("DBConnection").ToString
        Using con As New SqlConnection(conStr)
            Using cmd As New SqlCommand("SELECT TugasCode,TugasName,Address1,Phone FROM MsPemberiTugas ORDER BY TugasName") ' WHERE FgActive='Y' ")
                Using sda As New SqlDataAdapter()
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    Using dt As New DataTable()
                        sda.Fill(dt)
                        GVPembeli.DataSource = dt
                        GVPembeli.DataBind()
                    End Using
                End Using
            End Using
        End Using
    End Sub

    'Protected Sub OnDataBoundActivity(ByVal sender As Object, ByVal e As EventArgs)
    '    Dim Row As New GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal)
    '    For i As Integer = 0 To GVListNoLS.Columns.Count - 1
    '        Dim cell As New TableHeaderCell()
    '        Dim txtSearchKegiatan As New TextBox()
    '        txtSearchKegiatan.Attributes("placeholder") = GVListNoLS.Columns(i).HeaderText
    '        txtSearchKegiatan.CssClass = "search_textbox"
    '        cell.Controls.Add(txtSearchKegiatan)
    '        Row.Controls.Add(cell)
    '    Next
    '    GVListNoLS.HeaderRow.Parent.Controls.AddAt(1, Row)
    'End Sub

    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue, ddlRange.SelectedValue)
            'If AdvanceFilter.Length > 1 And StrFilter.Length > 1 Then
            '    StrFilter = StrFilter + " And " + AdvanceFilter
            'ElseIf AdvanceFilter.Length > 1 And StrFilter.Length <= 1 Then
            '    StrFilter = AdvanceFilter
            'End If
            DT = BindDataTransaction(GetStringHd, StrFilter, ViewState("DBConnection").ToString)
            If DT.Rows.Count = 0 Then
                lbStatus.Text = "No Data"
                pnlNav.Visible = False
            End If
            ddlCommand.Visible = DT.Rows.Count > 0
            BtnGo.Visible = DT.Rows.Count > 0
            ddlCommand2.Visible = ddlCommand.Visible
            btnGo2.Visible = BtnGo.Visible
            btnAdd2.Visible = BtnGo.Visible
            DV = DT.DefaultView
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "TransDate DESC"
            End If
            DV.Sort = ViewState("SortExpression")
            GridView1.DataSource = DV
            GridView1.DataBind()
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub RefreshGridDetil()
        'Dim conStr As String = ViewState("DBConnection").ToString
        'Using con As New SqlConnection(conStr)
        '    Using cmd As New SqlCommand("SELECT * FROM PRCIPRecvDocDt")
        '        Using sda As New SqlDataAdapter()
        '            cmd.Connection = con
        '            sda.SelectCommand = cmd
        '            Using dt As New DataTable()
        '                sda.Fill(dt)
        '                GridDt.DataSource = dt
        '                GridDt.DataBind()
        '            End Using
        '        End Using
        '    End Using
        'End Using
        'Dim conStr As String = ViewState("DBConnection").ToString
        'Using con As New SqlConnection(conStr)
        '    Using cmd As New SqlCommand("SELECT * FROM PRCIPRecvDocDt")
        '        Using sda As New SqlDataAdapter()
        '            cmd.Connection = con
        '            sda.SelectCommand = cmd
        '            Using dt As New DataTable()
        '                '-----------------------------------------------------------------------------------------------
        '                For Each row As GridViewRow In GVDocument.Rows
        '                    If row.RowType = DataControlRowType.DataRow Then
        '                        Dim chkRow As CheckBox = TryCast(row.Cells(0).FindControl("chkRow"), CheckBox)
        '                        If chkRow.Checked Then
        '                            Dim name As String = row.Cells(1).Text
        '                            Dim country As String = TryCast(row.Cells(2).FindControl("lblCountry"), Label).Text
        '                            dt.Rows.Add(name, country)
        '                        End If
        '                    End If
        '                Next
        '                '-----------------------------------------------------------------------------------------------
        '                sda.Fill(dt)
        '                GridDt.DataSource = dt
        '                GridDt.DataBind()
        '            End Using
        '        End Using
        '    End Using
        'End Using
    End Sub

    Private Sub BindGridDocument()
        Dim conStr As String = ViewState("DBConnection").ToString
        Using con As New SqlConnection(conStr)
            Using cmd As New SqlCommand("SELECT DokCode,DokName FROM MsDokumen") 'SELECT DokCode,DokName,Remark FROM V_MsDokumen
                Using sda As New SqlDataAdapter()
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    Using dt As New DataTable()
                        sda.Fill(dt)
                        GVDocument.DataSource = dt
                        GVDocument.DataBind()
                    End Using
                End Using
            End Using
        End Using        
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        Try
            GridView1.PageIndex = 0
            BindData(Session("AdvanceFilter"))
            pnlNav.Visible = True
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))

        Catch ex As Exception
            lbStatus.Text = "Btn Search Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGo.Click, btnGo2.Click
        Dim Status As String
        Dim Result, ListSelectNmbr, ActionValue As String
        Dim Nmbr(100) As String
        Dim j As Integer
        Try
            If sender.ID.ToString = "BtnGo" Then
                ActionValue = ddlCommand.SelectedValue
            Else
                ActionValue = ddlCommand2.SelectedValue
            End If
            If ActionValue = "Print" Then

                Dim GVR As GridViewRow
                Dim CB As CheckBox
                Dim Pertamax As Boolean

                Pertamax = True
                Result = ""

                For Each GVR In GridView1.Rows
                    CB = GVR.FindControl("cbSelect")
                    If CB.Checked Then
                        ListSelectNmbr = GVR.Cells(2).Text + "|" + GVR.Cells(3).Text
                        If Pertamax Then
                            Result = "'''" + ListSelectNmbr + "''"
                            Pertamax = False
                        Else
                            Result = Result + ",''" + ListSelectNmbr + "''"
                        End If
                    End If
                Next
                Result = Result + "'"
                'Session("SelectCommand") = "EXEC S_PDFormIO " + Result
                'Session("ReportFile") = ".../../../Rpt/FormIO.frx"
                Session("DBConnection") = ViewState("DBConnection")
                AttachScript("openprintdlg();", Page, Me.GetType)
            Else

                Status = CekStatus(ActionValue)

                ListSelectNmbr = ""
                '3 = status, 2 & 3 = key, 
                GetListCommand(Status, GridView1, "4,2,3", ListSelectNmbr, Nmbr, lbStatus.Text)
                'GetListCommand(Status, GridView1, "3,2", ListSelectNmbr, Nmbr, lbStatus.Text)
                If ListSelectNmbr = "" Then Exit Sub
                For j = 0 To (Nmbr.Length - 1)
                    If Nmbr(j) = "" Then
                        Exit For
                    Else
                        Result = ExecSPCommandGo(ActionValue, "S_PRCLandPurchaseTJ", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                        If Trim(Result) <> "" Then
                            lbStatus.Text = lbStatus.Text + Result + " <br/>"
                        End If
                    End If
                Next
                BindData("TransNmbr IN (" + ListSelectNmbr + ")")
            End If
        Catch ex As Exception
            lbStatus.Text = "Go Command Error : " + ex.ToString
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
            lbStatus.Text = "btn Expand Error : " + ex.ToString
        End Try
    End Sub

    Private Sub EnableDt(ByVal State As Boolean)
        Try
            btnDocument.Visible = State
            'btnMaterial.Visible = State
            'btnSubcontractor.Visible = State
            'ddlCategory.Enabled = State
            'ddlSubCategory.Enabled = State
            'ddlUnit.Enabled = State
            'ddlDetailType.Enabled = State
            tbDocCode.Enabled = State
            tbDocName.Enabled = State
            'tbItemCode.Enabled = State
            'tbItemName.Enabled = State
            'tbSubcontCode.Enabled = State
            'tbSubcontName.Enabled = State
            'tbQty.Enabled = State
            'tbPriceDt.Enabled = State
        Catch ex As Exception
            Throw New Exception("Enable Dt Error " + ex.ToString)
        End Try
    End Sub

    Private Sub EnableHd(ByVal State As Boolean)
        Try
            'btnPeriod.Visible = State
            'tbManagerName.Enabled = State
            'btnRequestBy.Visible = State
            tbLuas.Enabled = False
            tbDateDokumen.Enabled = False
            tbHrgM2.Enabled = False
            tbHrgTanah.Enabled = False
            tbPelunasan.Enabled = False
        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDt(ByVal Nmbr As String)
        Try
            'Dim dt As New DataTable
            'ViewState("Dt") = Nothing
            'dt = SQLExecuteQuery(GetStringDt(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            'ViewState("Dt") = dt
            'BindGridDt(dt, GridDt)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnHome.Click
        Try
            lbStatus.Text = ""
            MovePanel(pnlInput, PnlHd)
        Catch ex As Exception
            lbStatus.Text = "btn back Error : " + ex.ToString
        End Try
    End Sub

    Private Sub StatusButtonSave(ByVal Bool As Boolean)
        Try
            btnSaveAll.Visible = Bool
            btnSaveTrans.Visible = Bool
            btnBack.Visible = Bool
        Catch ex As Exception
            Throw New Exception("Status Button Save Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnCancelDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelDt.Click
        Try
            lbStatus.Text = ""
            MovePanel(pnlEditDt, PnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            lbRevisi.Text = "0"
            tbCode.Text = ""
            tbSellerCode.Text = ""
            tbSellerName.Text = ""
            tbNoLandSurvey.Text = ""
            tbPembeli.Text = ""
            tbNamaWakilPenjual.Text = ""
            tbNoKTPWakilPenjual.Text = ""
            tbAddrWakilPenjual.Text = ""
            tbNoTlpWakilPenjual.Text = ""
            tbKodeWakilPembeli.Text = ""
            tbNamaWakilPembeli.Text = ""
            tbDokumen.Text = "0"
            tbDateDokumen.SelectedDate = ViewState("ServerDate") 'Now.Date
            'ddlReport.SelectedValue = "Y"
            'ddlType.SelectedValue = "Supplier"
            'tbApplfileDate.SelectedDate = Now.Date
            'tbBeaNotaris.Text = FormatFloat(0, ViewState("DigitCurr"))
            tbPembayaran.Text = "0"
            tbPelunasan.Text = "0"
            tbHrgM2.Text = "0"
            tbLuas.Text = "0"
            tbHrgTanah.Text = "0"
            tbRemark.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub Cleardt()
        Try
            'ddlCategory.SelectedIndex = 0
            'ddlSubCategory.SelectedIndex = 0
            tbDocCode.Text = ""
            tbDocName.Text = ""
            'ddlDetailType.SelectedIndex = 0
            'tbItemCode.Text = ""
            'tbItemName.Text = ""
            'tbSubcontCode.Text = ""
            'tbSubcontName.Text = ""
            'tbLaborName.Text = ""
            'tbQty.Text = "0"
            'tbPriceDt.Text = "0"
            'ddlUnit.SelectedIndex = 0
            tbRemarkDt.Text = ""
            'tbSpecification.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub

    Function CekHd() As Boolean
        'Dim sqlstring, Result As String
        'Dim Rev As Integer
        Dim Pembayaran, Pelunasan, TotalPembayaran, HargaTanah As String
        Try
            'If CInt(Session(Request.QueryString("KeyId"))("Year")) <> Year(tbDate.SelectedValue) Or CInt(Session(Request.QueryString("KeyId"))("Period")) <> Month(tbDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(Session(Request.QueryString("KeyId"))("Period")) + " " + Session(Request.QueryString("KeyId"))("Year").ToString.Trim)
            '    Return False
            'End If
            If tbDate.SelectedDate = Nothing Then
                lbStatus.Text = MessageDlg("Tgl. Transaksi must have value")
                tbDate.Focus()
                Return False
            End If
            If tbNoLandSurvey.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("No. Land Survey must have value")
                tbNoLandSurvey.Focus()
                Return False
            End If
            If tbHrgM2.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Harga/m2 must have value")
                tbHrgM2.Focus()
                Return False
            End If
            If tbLuas.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Luas tanah must have value")
                tbLuas.Focus()
                Return False
            End If
            If tbHrgTanah.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Harga tanah must have value")
                tbHrgTanah.Focus()
                Return False
            End If
            If tbPembayaran.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Nilai pembayaran must have value")
                tbPembayaran.Focus()
                Return False
            End If
            If tbSellerCode.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Kode Pemilik must have value")
                tbSellerCode.Focus()
                Return False
            End If
            If tbSellerName.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Nama Pemilik Hak must have value")
                tbSellerName.Focus()
                Return False
            End If
            'If tbRemark.Text.Trim = "" Then
            '    lbStatus.Text = MessageDlg("Remark must have value")
            '    tbRemark.Focus()
            '    Return False
            'End If

            'Rev = SQLExecuteScalar("SELECT MAX(Revisi)AS Revisi FROM PRCLandPurchaseTJHd WHERE Status<>'D' AND LSNo = " + QuotedStr(tbNoLandSurvey.Text), ViewState("DBConnection"))
            'Payment1 = SQLExecuteScalar("SELECT SUM(COALESCE(Pembayaran,0))AS Pembayaran FROM PRCLandPurchaseTJHd WHERE Status='P' AND Revisi= " + Rev + " AND LSNo = " + QuotedStr(tbNoLandSurvey.Text), ViewState("DBConnection"))
            HargaTanah = SQLExecuteScalar("SELECT COALESCE(TtlHrgTanah,0) FROM GLLandPurchaseReqHd WHERE Status='P' AND TransNmbr = " + QuotedStr(tbNoLandSurvey.Text), ViewState("DBConnection"))
            Pembayaran = SQLExecuteScalar("SELECT SUM(COALESCE(Pembayaran,0)) FROM PRCLandPurchaseTJHd WHERE Status='P' AND TransNmbr = " + QuotedStr(tbCode.Text) + " AND LSNo = " + QuotedStr(tbNoLandSurvey.Text), ViewState("DBConnection"))
            'Pelunasan = SQLExecuteScalar("SELECT SUM(COALESCE(PayPelunasan,0)) FROM PRCLandPurchaseTJHd WHERE Status='P' AND TransNmbr = " + QuotedStr(tbCode.Text) + " AND LSNo = " + QuotedStr(tbNoLandSurvey.Text), ViewState("DBConnection"))
            '+ Pembayaran + Pelunasan
            tbPelunasan.Text = CFloat(tbHrgTanah.Text) - CFloat(tbPembayaran.Text)
            TotalPembayaran = CFloat(tbPelunasan.Text) + CFloat(tbPembayaran.Text)
            'lbStatus.Text = TotalPembayaran
            'Exit Function
            If TotalPembayaran > HargaTanah Then
                'lbStatus.Text = MessageDlg("Payment or Paid Off is greater than the Price of Land")
                lbStatus.Text = MessageDlg("Payment 1 and Paymend 2 cannot greater than Land Price")
                tbPembayaran.Focus()
                Return False
            End If
            'If Pembayaran >= CFloat(tbPembayaran.Text) Then
            '    lbStatus.Text = MessageDlg("Payment 1 must be greater than the previous payment")
            '    tbPembayaran.Focus()
            '    Return False
            'End If

            If SQLExecuteScalar("SELECT TransNmbr FROM GLLandPurchaseHD WHERE Status<>'D' AND TJNo = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                lbStatus.Text = MessageDlg("Cannot Revision TJ Number" + QuotedStr(tbCode.Text) + ", because already use on Land Purchace Order")
                Exit Function
            End If

            'If SQLExecuteScalar("SELECT TransNmbr FROM PRCLandPurchaseTJHd WHERE TransNmbr = " + QuotedStr(tbNoLandSurvey.Text), ViewState("DBConnection").ToString).Length > 0 Then
            '    lbStatus.Text = "No. Land of Survey" + QuotedStr(tbNoLandSurvey.Text) + " has already been exist"
            '    Exit Function
            'End If

            'If tbEndDate.SelectedDate < tbStartDate.SelectedDate Then
            '    lbStatus.Text = MessageDlg("End Date must greater than Start Date")
            '    tbEndDate.Focus()
            '    Return False
            'End If
            'If tbStartDate.SelectedDate > tbEndDate.SelectedDate Then
            '    lbStatus.Text = MessageDlg("Start Date can not greater than End Date")
            '    tbStartDate.Focus()
            '    Return False
            'End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Hd Error : " + ex.ToString)
        End Try
    End Function

    Function CekDt(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not Dr Is Nothing Then

            Else
                'If tbDocCode.Text.Trim = "" Then
                '    lbStatus.Text = MessageDlg("Job Name Must Have Value")
                '    tbDocCode.Focus()
                '    Return False
                'End If
                'If CFloat(tbQty.Text) <= "0" Then
                '    lbStatus.Text = MessageDlg("Qty Must Have Value")
                '    tbQty.Focus()
                '    Return False
                'End If
                'If ddlUnit.SelectedValue.Trim = "" Then
                '    lbStatus.Text = MessageDlg("Unit Must Have Value")
                '    ddlUnit.Focus()
                '    Return False
                'End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub FillTextBoxHd(ByVal Nmbr As String, ByVal Revisi As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr) + " AND Revisi = " + Revisi, ViewState("DBConnection").ToString)
            tbCode.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            'BindToDropList(ddlTerm, Dt.Rows(0)("Term").ToString)
            'BindToDropList(ddlType, Dt.Rows(0)("UserType").ToString)
            'BindToDate(tbDueDate, Dt.Rows(0)("DueDate").ToString)
            BindToText(tbSellerCode, Dt.Rows(0)("SellerCode").ToString)
            BindToText(tbSellerName, Dt.Rows(0)("SellName").ToString)
            BindToText(tbHrgM2, Dt.Rows(0)("HargaPerM"), ViewState("DigitHome"))
            BindToText(tbLuas, Dt.Rows(0)("Luas"), ViewState("DigitHome"))
            BindToText(tbHrgTanah, Dt.Rows(0)("HargaTanah"), ViewState("DigitHome"))
            'BindToText(tbHrgM2, Dt.Rows(0)("HargaPerM").ToString)
            'BindToText(tbLuas, Dt.Rows(0)("Luas").ToString)
            'BindToText(tbHrgTanah, Dt.Rows(0)("HargaTanah").ToString)
            BindToText(tbNoLandSurvey, Dt.Rows(0)("LSNo").ToString)
            BindToText(tbPembayaran, Dt.Rows(0)("Pembayaran"), ViewState("DigitHome"))
            BindToText(tbPelunasan, Dt.Rows(0)("PayPelunasan"), ViewState("DigitHome"))
            BindToText(tbPembeli, Dt.Rows(0)("NamaPembeli").ToString)
            BindToText(tbKodeWakilPembeli, Dt.Rows(0)("TugasCode").ToString)
            BindToText(tbNamaWakilPembeli, Dt.Rows(0)("TugasName").ToString)
            BindToText(tbNamaWakilPenjual, Dt.Rows(0)("NamaWakilPenjual").ToString)
            BindToText(tbNoKTPWakilPenjual, Dt.Rows(0)("NoKTPWakilPenjual").ToString)
            BindToText(tbAddrWakilPenjual, Dt.Rows(0)("AddrWakilPenjual").ToString)
            BindToText(tbNoTlpWakilPenjual, Dt.Rows(0)("NoTelpWakilPenjual").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
            lbRevisi.Text = Dt.Rows(0)("Revisi").ToString
            BindToDropList(ddlPembayaran, Dt.Rows(0)("RencanaBayar").ToString)
            BindToText(tbDokumen, Dt.Rows(0)("BatasKelengkapan").ToString)
            BindToDate(tbDateDokumen, Dt.Rows(0)("DateKelengkapan").ToString)
            'fupSignRecv.Visible = False
            'If ddlPembayaran.SelectedValue = "B" Then
            '    Type = "Bank"
            'ElseIf ddlPembayaran.SelectedValue = "G" Then
            '    Type = "Giro"
            'ElseIf ddlPembayaran.SelectedValue = "T" Then
            '    Type = "Transfer"
            'ElseIf ddlPembayaran.SelectedValue = "C" Then
            '    Type = "Cash"
            'End If
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    'Protected Sub FillTextBoxDt(ByVal sJobCode As String, ByVal sMaterialCode As String)
    Protected Sub FillTextBoxDt(ByVal sDokCode As String)
        Dim Dr As DataRow()
        Try
            'Dr = ViewState("Dt").Select("JobCode = " + QuotedStr(sJobCode) + " AND MaterialCode = " + QuotedStr(sMaterialCode))
            Dr = ViewState("Dt").Select("DokCode = " + QuotedStr(sDokCode))
            If Dr.Length > 0 Then
                'BindToDropList(ddlCategory, Dr(0)("CategoryCode").ToString)
                'BindToDropList(ddlSubCategory, Dr(0)("SubCategoryCode").ToString)
                BindToText(tbDocCode, Dr(0)("DokCode").ToString)
                BindToText(tbDocName, Dr(0)("DokName").ToString)
                'BindToDropList(ddlDetailType, Dr(0)("DetailType").ToString)
                'BindToText(tbItemCode, Dr(0)("MaterialCode").ToString)
                'BindToText(tbItemName, Dr(0)("Description").ToString)
                'BindToText(tbLaborName, Dr(0)("LaborName").ToString)
                'BindToText(tbQty, Dr(0)("Qty").ToString)
                'BindToText(tbPriceDt, Dr(0)("Price").ToString)
                'BindToDropList(ddlUnit, Dr(0)("Unit").ToString)
                'BindToText(tbSubcontCode, Dr(0)("SubcontCode").ToString)
                'BindToText(tbSubcontName, Dr(0)("SubcontName").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt.Click
        'Dim SqlString As String
        'Dim strCategory, strSubCategory, strUnit As String
        Try
            If CekDt() = False Then
                btnSaveDt.Focus()
                Exit Sub
            End If
            If ViewState("StateDt") = "Edit" Then 'Ini untuk kondisi Edit
                Dim Row As DataRow
                'If ViewState("PKDt") <> tbProductCode.Text Then
                '    'If CekExistData(ViewState("Dt"), "ProductCode", tbProductCode.Text) Then
                'If CekExistData(ViewState("Dt"), "JobCode,MaterialCode", tbProductCode.Text + "|" + tbItemCode.Text) = True Then
                '    lbStatus.Text = "Job Name " + tbProductName.Text + " and Item Name " + tbItemName.Text + " has been already exist"
                '    Exit Sub
                'End If
                'End If
                'SqlString = "SELECT CategoryName FROM V_MsCategory WHERE CategoryCode = " + QuotedStr(ddlCategory.SelectedValue)
                'strCategory = SQLExecuteScalar(SqlString, ViewState("DBConnection").ToString)

                'SqlString = "SELECT SubCategoryName FROM GetMsSubCategory WHERE SubCategoryCode = " + QuotedStr(ddlSubCategory.SelectedValue)
                'strSubCategory = SQLExecuteScalar(SqlString, ViewState("DBConnection").ToString)  'V_MsSubCategory

                'SqlString = "SELECT UnitName FROM V_MsUnit WHERE UnitCode = " + QuotedStr(ddlUnit.SelectedValue)
                'strUnit = SQLExecuteScalar(SqlString, ViewState("DBConnection").ToString)

                'Row = ViewState("Dt").Select("ProductCode = " + QuotedStr(ViewState("PKDt")))(0)
                'Row = ViewState("Dt").Select("JobCode = " + QuotedStr(ViewState("PKDt")))(0)

                'Row = ViewState("Dt").Select("DokCode = " + QuotedStr(tbDocCode.Text) + " AND MaterialCode = " + QuotedStr(tbItemCode.Text))(0)
                Row = ViewState("Dt").Select("DokCode = " + QuotedStr(tbDocCode.Text))(0)
                If CekDt() = False Then
                    Exit Sub
                End If

                'Row.BeginEdit()
                'Row("CategoryCode") = ddlCategory.SelectedValue
                'Row("CategoryName") = strCategory
                'Row("SubCategoryCode") = ddlSubCategory.SelectedValue
                'Row("SubCategoryName") = strSubCategory
                Row("DokCode") = tbDocCode.Text
                Row("DokName") = tbDocName.Text
                'Row("DetailType") = ddlDetailType.SelectedValue
                'Row("MaterialCode") = tbItemCode.Text
                'Row("Description") = tbItemName.Text
                'Row("SubcontCode") = tbSubcontCode.Text
                'Row("SubcontName") = tbSubcontName.Text
                'Row("LaborName") = UCase(tbLaborName.Text)
                'Row("Qty") = tbQty.Text
                'Row("Unit") = ddlUnit.SelectedValue
                'Row("UnitName") = strUnit
                'Row("Price") = tbPriceDt.Text
                'Row("Amount") = tbQty.Text * tbPriceDt.Text
                Row("Remark") = tbRemarkDt.Text
                Row("FgActive") = "Y"
                'Row("DoneClosing") = "N"
                Row.EndEdit()
            Else
                'Ini untuk kondisi Insert/Input
                If CekDt() = False Then
                    Exit Sub
                End If
                'SqlString = "SELECT CategoryName FROM V_MsCategory WHERE CategoryCode = " + QuotedStr(ddlCategory.SelectedValue)
                'strCategory = SQLExecuteScalar(SqlString, ViewState("DBConnection").ToString)

                'SqlString = "SELECT SubCategoryName FROM GetMsSubCategory WHERE SubCategoryCode = " + QuotedStr(ddlSubCategory.SelectedValue)
                'strSubCategory = SQLExecuteScalar(SqlString, ViewState("DBConnection").ToString)   'V_MsSubCategory

                'SqlString = "SELECT UnitName FROM V_MsUnit WHERE UnitCode = " + QuotedStr(ddlUnit.SelectedValue)
                'strUnit = SQLExecuteScalar(SqlString, ViewState("DBConnection").ToString)

                'If CekExistData(ViewState("Dt"), "ProductCode", tbProductCode.Text) = True Then

                'If CekExistData(ViewState("Dt"), "CategoryCode,SubCategoryCode,JobCode,MaterialCode", ddlCategory.SelectedValue + "|" + ddlSubCategory.SelectedValue + "|" + tbProductCode.Text + "|" + tbItemCode.Text) = True Then
                '    lbStatus.Text = "Category (" + strCategory + "), Sub Category (" + strSubCategory + "), Job Name (" + tbProductName.Text + ") and Item Name (" + tbItemName.Text + ") has been already exist"
                '    Exit Sub
                'End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                'dr("CategoryCode") = ddlCategory.SelectedValue
                'dr("CategoryName") = strCategory
                'dr("SubCategoryCode") = ddlSubCategory.SelectedValue
                'dr("SubCategoryName") = strSubCategory
                'dr("ItemNo") = GetNewItemNo(ViewState("Dt"))
                dr("DokCode") = tbDocCode.Text
                dr("DokName") = tbDocName.Text
                'dr("DetailType") = ddlDetailType.SelectedValue
                'dr("MaterialCode") = tbItemCode.Text
                'dr("Description") = tbItemName.Text
                'dr("SubcontCode") = tbSubcontCode.Text
                'dr("SubcontName") = tbSubcontName.Text
                'dr("LaborName") = UCase(tbLaborName.Text)
                'dr("Qty") = tbQty.Text
                'dr("Unit") = ddlUnit.SelectedValue
                'dr("UnitName") = strUnit
                'dr("Price") = tbPriceDt.Text
                'dr("Amount") = tbQty.Text * tbPriceDt.Text
                dr("Remark") = tbRemarkDt.Text
                dr("FgActive") = "Y"
                'dr("DoneClosing") = "N"
                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, PnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            BindGridDt(ViewState("Dt"), GridDt)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save Dt Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub

    Private Sub SaveAll()
        Dim SQLString, Type As String
        Dim Pembayaran, Pelunasan, TotalPembayaran, HargaTanah As String
        Dim Result As Boolean
        Try
            'System.Threading.Thread.Sleep(7000)
            If Not (ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit") Then
                Exit Sub
            Else
                ViewState("StateSave") = ViewState("StateHd")
                ViewState("StateHd") = "View"
            End If

            'Save Hd
            If ViewState("StateSave") = "Insert" Then
                If SQLExecuteScalar("SELECT TransNmbr FROM PRCLandPurchaseTJHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lbStatus.Text = "No. Transaksi " + QuotedStr(tbCode.Text) + " has already been exist"
                    Exit Sub
                End If
               
                'ddlReport.SelectedValue
                tbCode.Text = GetAutoNmbr("TJ", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
                'If ddlType.SelectedValue = "TJ" Then
                '    Type = "Tanda Jadi"
                'ElseIf ddlType.SelectedValue = "PL" Then
                '    Type = "Pelunasan"
                'End If

                SQLString = "INSERT INTO PRCLandPurchaseTJHd(TransNmbr,TransDate,Status,Revisi,LSNo,SellerCode," + _
                            "HargaPerM,Luas,HargaTanah,Pembayaran,PayPelunasan,RencanaBayar,NamaPembeli,TugasCode,NamaWakilPenjual, " + _
                            "NoKTPWakilPenjual,AddrWakilPenjual,NoTelpWakilPenjual, DateKelengkapan, BatasKelengkapan, Remark,UserPrep,DatePrep,FgReport,FgActive) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", 'H', " + _
                QuotedStr("0") + ", " + QuotedStr(tbNoLandSurvey.Text) + ", " + QuotedStr(tbSellerCode.Text) + ", " + _
                QuotedStr(tbHrgM2.Text.Replace(",", "")) + ", " + QuotedStr(tbLuas.Text.Replace(",", "")) + ", " + _
                QuotedStr(tbHrgTanah.Text.Replace(",", "")) + ", " + QuotedStr(tbPembayaran.Text.Replace(",", "")) + ", " + _
                QuotedStr(tbPelunasan.Text.Replace(",", "")) + ", " + QuotedStr(ddlPembayaran.SelectedValue) + ", " + _
                QuotedStr(tbPembeli.Text) + ", " + QuotedStr(tbKodeWakilPembeli.Text) + ", " + QuotedStr(tbNamaWakilPenjual.Text) + ", " + _
                QuotedStr(tbNoKTPWakilPenjual.Text) + ", " + QuotedStr(tbAddrWakilPenjual.Text) + ", " + QuotedStr(tbNoTlpWakilPenjual.Text) + ", " + _
                QuotedStr(Format(tbDateDokumen.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(tbDokumen.Text) + ", " + _
                QuotedStr(tbRemark.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", GetDate()," + QuotedStr("0") + ", " + QuotedStr("Y")
                ViewState("TransNmbr") = tbCode.Text 'ddlUserType.SelectedValue, tbTotalSelisih.Text.Replace(",", "") 
                ViewState("Revisi") = "0"
            Else
                ''ElseIf ViewState("StateHd") = "Edit" Then 
                'Dim cekStatus As String
                'cekStatus = SQLExecuteScalar("SELECT Status FROM PRCLandPurchaseTJHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                'If cekStatus = "P" Then
                '    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                '    Exit Sub
                'End If

                'If ddlType.SelectedValue = "TJ" Then
                '    Type = "Tanda Jadi"
                'ElseIf ddlType.SelectedValue = "PL" Then
                '    Type = "Pelunasan"
                'End If

                SQLString = "UPDATE PRCLandPurchaseTJHd SET TransDate = " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + _
                ", LSNo =" + QuotedStr(tbNoLandSurvey.Text) + ", SellerCode = " + QuotedStr(tbSellerCode.Text) + _
                ", DateKelengkapan = " + QuotedStr(Format(tbDateDokumen.SelectedValue, "yyyy-MM-dd")) + _
                ", BatasKelengkapan = " + QuotedStr(tbDokumen.Text) + _
                ", HargaPerM = " + QuotedStr(tbHrgM2.Text.Replace(",", "")) + ", Luas = " + QuotedStr(tbLuas.Text.Replace(",", "")) + _
                ", HargaTanah = " + QuotedStr(tbHrgTanah.Text.Replace(",", "")) + ", Pembayaran = " + QuotedStr(tbPembayaran.Text.Replace(",", "")) + _
                ", PayPelunasan = " + QuotedStr(tbPelunasan.Text.Replace(",", "")) + ", RencanaBayar = " + QuotedStr(ddlPembayaran.SelectedValue) + _
                ", NamaPembeli = " + QuotedStr(tbPembeli.Text) + ", TugasCode = " + QuotedStr(tbKodeWakilPembeli.Text) + _
                ", NamaWakilPenjual = " + QuotedStr(tbNamaWakilPenjual.Text) + ", NoKTPWakilPenjual = " + QuotedStr(tbNoKTPWakilPenjual.Text) + _
                ", AddrWakilPenjual = " + QuotedStr(tbAddrWakilPenjual.Text) + ", NoTelpWakilPenjual = " + QuotedStr(tbNoTlpWakilPenjual.Text) + _
                ", Remark = " + QuotedStr(tbRemark.Text) + ", UserPrep= " + QuotedStr(ViewState("UserId").ToString) + ", DatePrep = GetDate()" + _
                " WHERE TransNmbr = " + QuotedStr(tbCode.Text) + " And Revisi = " + QuotedStr(lbRevisi.Text) 'ddlUserType.SelectedValue, tbTotalSelisih.Text.Replace(",", "")

            End If
            SQLString = ChangeQuoteNull(SQLString)
            SQLString = SQLString.Replace("'1900-01-01'", "NULL")
            SQLString = SQLString.Replace("'0001-01-01'", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            'update Primary Key on Dt
            'Dim Row As DataRow()

            'Row = ViewState("Dt").Select("TransNmbr IS NULL")
            'For I = 0 To Row.Length - 1
            '    Row(I).BeginEdit()
            '    Row(I)("TransNmbr") = tbCode.Text
            '    Row(I).EndEdit()
            'Next

            'save dt
            'SQLString = "DELETE FROM CNTCostPlanDt WHERE TransNmbr = " + QuotedStr(tbProjectCode.Text) + " AND JobCode = " + QuotedStr(tbProductCode.Text) + _
            '" AND MaterialCode = " + QuotedStr(tbItemCode.Text)
            'SQLExecuteScalar(SQLString, ViewState("DBConnection").ToString)

            'Dim ConnString As String = ViewState("DBConnection").ToString
            'con = New SqlConnection(ConnString)
            'con.Open()
            'SQLString = "SELECT TransNmbr,DokCode,Remark,FgActive FROM PRCIPRecvDocDt WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            'Dim cmdSql As New SqlCommand(SQLString, con)
            'da = New SqlDataAdapter(cmdSql)
            'Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)
            'da.InsertCommand = dbcommandBuilder.GetInsertCommand
            ''da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            ''da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            ''Dim param As SqlParameter
            ' '' Create the UpdateCommand.
            'SQLString = "UPDATE PRCIPRecvDocDt SET DokCode = @DokCode, Remark = @Remark " + _
            '" FROM PRCIPRecvDocDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "'"
            'Dim Update_Command = New SqlCommand(SQLString, con)
            '' Define output parameters.
            'Update_Command.Parameters.Add("@DokCode", SqlDbType.VarChar, 12, "DokCode")
            'Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 255, "Remark")
            '' Define intput (WHERE) parameters.
            ''param = Update_Command.Parameters.Add("@OldJobCode", SqlDbType.VarChar, 5, "JobCode")
            ''param.SourceVersion = DataRowVersion.Original
            ''param = Update_Command.Parameters.Add("@OldMaterialCode", SqlDbType.VarChar, 30, "MaterialCode")
            ''param.SourceVersion = DataRowVersion.Original
            '' Attach the update command to the DataAdapter.
            'da.UpdateCommand = Update_Command

            ' '' Create the DeleteCommand.
            'SQLString = "DELETE FROM PRCIPRecvDocDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "'"
            'Dim Delete_Command = New SqlCommand(SQLString, con) 'ViewState("Reference")
            '' Add the parameters for the DeleteCommand.
            ''param = Delete_Command.Parameters.Add("@TransNmbr", SqlDbType.VarChar, 20, "TransNmbr")
            ''param.SourceVersion = DataRowVersion.Original
            ''param = Delete_Command.Parameters.Add("@JobCode", SqlDbType.VarChar, 5, "JobCode")
            ''param.SourceVersion = DataRowVersion.Original
            ''param = Delete_Command.Parameters.Add("@MaterialCode", SqlDbType.VarChar, 30, "MaterialCode")
            ''param.SourceVersion = DataRowVersion.Original
            'da.DeleteCommand = Delete_Command

            'Dim Dt As New DataTable("PRCIPRecvDocDt")
            'Dt = ViewState("Dt")
            'da.Update(Dt)
            'Dt.AcceptChanges()
            'ViewState("Dt") = Dt
        Catch ex As Exception
            Throw New Exception("Save All Data Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSaveTrans_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveTrans.Click
        Dim CurrFilter, Value As String
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            'If GetCountRecord(ViewState("Dt")) = 0 Then
            '    lbStatus.Text = MessageDlg("Detail must have at least 1 record")
            '    Exit Sub
            'End If

            SaveAll()
            MovePanel(pnlInput, PnlHd)
            CurrFilter = tbFilter.Text
            Value = ddlField.SelectedValue
            tbFilter.Text = tbCode.Text
            ddlField.SelectedValue = "TransNmbr"
            btnSearch_Click(Nothing, Nothing)
            tbFilter.Text = CurrFilter
            ddlField.SelectedValue = Value
        Catch ex As Exception
            lbStatus.Text = "btnSaveTrans Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            lbStatus.Text = ""
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, PnlDt, GridDt)
            newTrans()
            btnHome.Visible = False
            EnableHd(True)
            'fupSignRecv.Visible = True
            'lbSignRecv.Visible = False
            'Session.Remove("FileUpload1")
            'Session.Clear()
            'fupSignRecv.Visible = True
            'GridDt.Columns(1).Visible = False
            'GridDt.Columns(8).Visible = False
            'GridDt.Columns(9).Visible = False
            'GridDt.Columns(10).Visible = False
            'GridDt.Columns(11).Visible = False
            'GridDt.Columns(12).Visible = False
            tbCode.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdddt.Click, btnAddDtke2.Click
    '    Try
    '        Cleardt()
    '        If CekHd() = False Then
    '            Exit Sub
    '        End If
    '        ViewState("StateDt") = "Insert"
    '        MovePanel(PnlDt, pnlEditDt)
    '        EnableHd(False)
    '        StatusButtonSave(False)
    '        tbDocCode.Focus()
    '    Catch ex As Exception
    '        lbStatus.Text = "btn add dt error : " + ex.ToString
    '    End Try
    'End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ViewState("DigitCurr") = 0
            ViewState("TransNmbr") = ""
            ViewState("Revisi") = "0"
            ClearHd()
            Cleardt()
            PnlDt.Visible = True
            'btnDeleteDoc.Visible = False
            BindDataDt("")
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            CheckAll(GridView1, sender)
        Catch ex As Exception
            lbStatus.Text = "cb Select Checked Changed Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub LbAdvSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LbAdvSearch.Click
    '    Dim FDateName, FDateValue, FilterName, FilterValue As String
    '    Try
    '        FDateName = "Date, Start Date, End Date"
    '        FDateValue = "TransDate, StartDate, EndDate"
    '        FilterName = "IO No, Status, Request By Code, Request By Name, Remark"
    '        FilterValue = "TransNmbr, Status, RequestBy, RequestByName, Remark"
    '        Session("DateFieldName") = FDateName.Split(",")
    '        Session("DateFieldValue") = FDateValue.Split(",")
    '        Session("FieldName") = FilterName.Split(",")
    '        Session("FieldValue") = FilterValue.Split(",")
    '        AttachScript("OpenFilterCriteria();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lbStatus.Text = "Advanced Search Click Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        GridView1.PageIndex = e.NewPageIndex
        BindData(Session("AdvanceFilter"))
    End Sub

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        Dim DDL As DropDownList
        Dim GVR As GridViewRow = Nothing
        Dim index As Integer
        Dim CekMenu As String
        Try
            If Not (e.CommandName = "Sort" Or e.CommandName = "Page") Then
                index = Convert.ToInt32(e.CommandArgument)
                GVR = GridView1.Rows(index)
            End If
            If e.CommandName = "Go" Then
                DDL = GridView1.Rows(index).FindControl("ddl")
                If DDL.SelectedValue = "View" Then
                    MovePanel(PnlHd, pnlInput)
                    ViewState("TransNmbr") = GVR.Cells(2).Text
                    ViewState("Status") = GVR.Cells(4).Text
                    ViewState("Revisi") = GVR.Cells(3).Text
                    GridDt.PageIndex = 0
                    FillTextBoxHd(ViewState("TransNmbr"), ViewState("Revisi"))
                    BindDataDt(ViewState("TransNmbr"))
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, PnlDt, GridDt)
                    btnHome.Visible = True
                    ViewState("Status") = GVR.Cells(4).Text
                    btnSaveTrans.Visible = False
                    ddlPembayaran.Enabled = False
                    EnableHd(False)
                ElseIf DDL.SelectedValue = "Edit" Then
                    If GVR.Cells(4).Text = "H" Or GVR.Cells(4).Text = "G" Then
                        CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        MovePanel(PnlHd, pnlInput)
                        ViewState("TransNmbr") = GVR.Cells(2).Text
                        ViewState("Status") = GVR.Cells(4).Text
                        ViewState("Revisi") = GVR.Cells(3).Text
                        GridDt.PageIndex = 0
                        BindDataDt(ViewState("TransNmbr"))
                        FillTextBoxHd(ViewState("TransNmbr"), ViewState("Revisi"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, PnlDt, GridDt)
                        btnHome.Visible = False
                        ddlPembayaran.Enabled = True
                        EnableHd(True)
                        'ElseIf GVR.Cells(4).Text = "P" Then
                        '    MovePanel(PnlHd, pnlInput)
                        '    ViewState("TransNmbr") = GVR.Cells(2).Text
                        '    ViewState("Status") = GVR.Cells(3).Text
                        '    ViewState("Revisi") = GVR.Cells(4).Text
                        '    GridDt.PageIndex = 0
                        '    BindDataDt(ViewState("TransNmbr"))
                        '    FillTextBoxHd(ViewState("TransNmbr"), ViewState("Revisi"))
                        '    ViewState("StateHd") = "Edit"
                        '    ModifyInput2(False, pnlInput, PnlDt, GridDt)
                        '    btnHome.Visible = True
                        '    btnSaveTrans.Visible = True
                        'btnSaveAll.Visible = True
                        'EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    Else
                        lbStatus.Text = MessageDlg("Data must be Hold or Get Approval to edit")
                        Exit Sub
                    End If
                    'GridDt.Columns(1).Visible = False
                    'GridDt.Columns(8).Visible = False
                    'GridDt.Columns(9).Visible = False
                    'GridDt.Columns(10).Visible = False
                    'GridDt.Columns(11).Visible = False
                    'GridDt.Columns(12).Visible = False
                    '----------------------Add by Chris --------------------------------------------------'
                ElseIf DDL.SelectedValue = "Revisi" Then
                    CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                    If CekMenu <> "" Then
                        lbStatus.Text = CekMenu
                        Exit Sub
                    End If
                    index = Convert.ToInt32(e.CommandArgument)
                    GVR = GridView1.Rows(index)

                    If Not GVR.Cells(4).Text = "P" Then
                        lbStatus.Text = MessageDlg("Data Must Post Before Create Revision")
                        Exit Sub
                    End If

                    Dim ResultCek, SqlStringCek, Result, SqlString, Value As String
                    'klik
                    If HiddenRemarkRevisi.Value <> "False Value" Then
                        AttachScript("revisi();", Page, Me.GetType)

                        'If HiddenRemarkRevisi.Value <> "" Then
                        SqlString = "DECLARE @A VarChar(255) EXEC S_PRCLandPurchaseTJCreateRevisi " + QuotedStr(GVR.Cells(2).Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", @A OUT SELECT @A"
                        Result = SQLExecuteScalar(SqlString, ViewState("DBConnection"))
                        Result = Result.Replace("0", "")

                        If Trim(Result) <> "1" Then
                            lbStatus.Text = MessageDlg("Create Revisi ( " + Result + " ) Sucess")

                        Else
                            lbStatus.Text = MessageDlg(Result)
                        End If
                        'Else
                        '    Exit Sub
                        'End If
                        btnSearch_Click(Nothing, Nothing)
                        'If Result.Length > 2 Then
                        '    lbStatus.Text = MessageDlg(Result)
                        'Else
                        '    BindDataDt(ViewState("TransNmbr"), ViewState("Revisi"))
                        'End If
                    End If
                    '------------------------------------------------------------------------'
                ElseIf DDL.SelectedValue = "Print" Then
                    Try
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        'lbStatus.Text = "EXEC S_PRCFormLandPurchaseTJ '''" + GVR.Cells(2).Text + "''', " + ViewState("UserId") + ", " + GVR.Cells(3).Text + ""
                        'Exit Sub
                        Session("SelectCommand") = "EXEC S_PRCFormLandPurchaseTJ '''" + GVR.Cells(2).Text + "''', '" + ViewState("UserId") + "', " + GVR.Cells(3).Text + ""
                        Session("ReportFile") = ".../../../Rpt/FormLandPurchaseTJ.frx"
                        Session("DBConnection") = ViewState("DBConnection")
                        AttachScript("openprintdlg();", Page, Me.GetType)
                    Catch ex As Exception
                        lbStatus.Text = "btn print Error = " + ex.ToString
                    End Try
                End If
            End If

        Catch ex As Exception
            lbStatus.Text = "Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridView1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        Try
            If ViewState("SortOrder") = Nothing Or ViewState("SortOrder") = "DESC" Then
                ViewState("SortOrder") = "ASC"
            Else
                ViewState("SortOrder") = "DESC"
            End If
            ViewState("SortExpression") = e.SortExpression + " " + ViewState("SortOrder")
            BindData(Session("AdvanceFilter"))
        Catch ex As Exception
            lbStatus.Text = "Grid View 1 Sorting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridDt.RowCommand
        Try
            If e.CommandName = "Closing" Then
                Dim GVR As GridViewRow
                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))
                If ViewState("Status") <> "P" Then
                    lbStatus.Text = MessageDlg("Status Terima Dokumen is not Post, cannot close product")
                    Exit Sub
                End If
                If GVR.Cells(12).Text = "Y" Then
                    lbStatus.Text = MessageDlg("Job Code Closed Already")
                    Exit Sub
                End If
                ViewState("ProductClose") = GVR.Cells(2).Text
                AttachScript("closing();", Page, Me.GetType)
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)

            'dr = ViewState("Dt").Select("JobCode = " + QuotedStr(GVR.Cells(3).Text) + " AND MaterialCode = " + QuotedStr(GVR.Cells(6).Text))
            dr = ViewState("Dt").Select("DokCode = " + QuotedStr(GVR.Cells(1).Text))
            dr(0).Delete()
            BindGridDt(ViewState("Dt"), GridDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            'FillTextBoxDt(GVR.Cells(3).Text, GVR.Cells(6).Text)
            FillTextBoxDt(GVR.Cells(1).Text)
            'If GVR.Cells(5).Text = "Material" Then
            '    tbLaborName.Visible = False
            '    lbSubcontractor.Visible = False
            '    lbTitikDua.Visible = False
            '    btnSubcontractor.Visible = True
            'Else
            '    tbLaborName.Visible = True
            '    lbSubcontractor.Visible = True
            '    lbTitikDua.Visible = True
            '    btnSubcontractor.Visible = True
            'End If

            MovePanel(PnlDt, pnlEditDt)
            EnableDt(False)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            ViewState("PKDt") = GVR.Cells(3).Text
            btnSaveDt.Focus()
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveAll.Click
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            'If IsNothing(ViewState("Dt")) Then
            '    lbStatus.Text = MessageDlg("Detail must have at least 1 record")
            '    Exit Sub
            'End If
            'If GetCountRecord(ViewState("Dt")) = 0 Then
            '    lbStatus.Text = MessageDlg("Detail must have at least 1 record")
            '    Exit Sub
            'End If
            'For Each dr In ViewState("Dt").Rows
            '    If CekDt(dr) = False Then
            '        Exit Sub
            '    End If
            'Next
            SaveAll()
            newTrans()
            EnableHd(True)
        Catch ex As Exception
            lbStatus.Text = "btn save all error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnDocument_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDocument.Click
        'Dim ResultField As String
        'Try
        '    'Session("filter") = "SELECT Product_Code, Product_Name, Specification, Unit FROM VMsProduct WHERE Fg_Active = 'Y' AND Fg_Stock = 'Y' AND ProductCategory = 'Finish Goods' "
        '    'ResultField = "Product_Code, Product_Name, Unit, Specification"
        '    Session("filter") = "SELECT DokCode, DokName FROM MsDokumen"
        '    ResultField = "DokCode, DokName"
        '    ViewState("Sender") = "btnDocument"
        '    Session("Column") = ResultField.Split(",")
        '    Session("DBConnection") = ViewState("DBConnection")
        '    'AttachScript("OpenSearchDlg();", Page, Me.GetType())
        '    'ScriptManager.RegisterClientScriptBlock(sender, Me.GetType(), "alert", "window.open('FindMultiDlg.aspx')", True)
        'Catch ex As Exception
        '    lbStatus.Text = "btnDocument_Click Error : " + ex.ToString
        'End Try
    End Sub

    'Protected Sub btnGetData_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetData.Click
    '    Dim ResultField, CriteriaField As String
    '    Try
    '        'Session("filter") = "EXEC S_CNTRABReff " + QuotedStr(tbCode.Text) 'WHERE Fg_Active = 'Y'
    '        Session("filter") = "EXEC S_CIPRecvMsDokumen "
    '        'Session("filter") = "SELECT Doc_Code,Document_Name,Remark FROM V_CIPRecvMsDokumen"
    '        ResultField = "Doc_Code,Document_Name,Remark"
    '        CriteriaField = "Doc_Code,Document_Name,Remark"
    '        ViewState("CriteriaField") = CriteriaField.Split(",")
    '        Session("Column") = ResultField.Split(",")
    '        ViewState("Sender") = "btnGetData"
    '        Session("DBConnection") = ViewState("DBConnection")
    '        'AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
    '        AttachScript("OpenPopup();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lbStatus.Text = "btnGetData_Click Error : " + ex.ToString
    '    End Try
    'End Sub

    'Protected Sub ddlCategory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCategory.SelectedIndexChanged
    '    'Dim SQLString, strCode As String
    '    Try
    '        'SQLString = "SELECT CategoryCode FROM V_MsCategory WHERE CategoryCode = " + QuotedStr(ddlCategory.SelectedValue)
    '        'strCode = SQLExecuteScalar(SQLString, ViewState("DBConnection").ToString)
    '        'lbGetCategoryCode.Text = strCode 'ddlCategory.SelectedItem.Value
    '        '--------------------------------------------------------------------------------------'
    '        'Dim dt As DataTable
    '        'Dim ConStr As String = System.ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString
    '        'Dim conStr As String = "Data Source=" + System.Configuration.ConfigurationManager.AppSettings.Get("ServerIP") + ";Initial Catalog=Contractor;User ID=sa"
    '        Dim strSQL = "S_GetMsSubCategory"
    '        Dim conStr As String = ViewState("DBConnection").ToString
    '        Using con As New SqlConnection(conStr)
    '            con.Open()
    '            Using cmd As New SqlCommand(strSQL, con)
    '                cmd.Connection = con
    '                cmd.CommandType = CommandType.StoredProcedure
    '                'cmd.Parameters.AddWithValue("@CategoryCode", ddlCategory.SelectedValue)
    '                cmd.ExecuteNonQuery()
    '                'ddlSubCategory.DataSource = cmd.ExecuteReader()
    '                'ddlSubCategory.DataTextField = "SubCategoryName"
    '                'ddlSubCategory.DataValueField = "SubCategoryCode"
    '                'ddlSubCategory.DataBind()
    '                con.Close()
    '            End Using
    '        End Using
    '        '--------------------------------------------------------------------------------------'
    '    Catch ex As Exception
    '        lbStatus.Text = "ddlCategory_SelectedIndexChanged Error : " + ex.ToString
    '    End Try
    'End Sub

    Private Sub BindGridDetilDocument()
        'Dim dt As New DataTable()
        Dim conStr As String = ViewState("DBConnection").ToString
        Using con As New SqlConnection(conStr)
            For Each row As GridViewRow In GVDocument.Rows
                If row.RowType = DataControlRowType.DataRow Then
                    Dim chkRow As CheckBox = TryCast(row.Cells(0).FindControl("chkRow"), CheckBox)
                    If chkRow.Checked Then
                        'Using cmd As New SqlCommand("INSERT INTO PRCIPRecvDocDt(DokCode) VALUES('" & row.Cells(1).Text & "','" & row.Cells(2).Text & "')")
                        Using cmd As New SqlCommand("INSERT INTO PRCIPRecvDocDt(DokCode) VALUES('" & row.Cells(1).Text & "')")
                            Using sda As New SqlDataAdapter()
                                cmd.Connection = con
                                sda.SelectCommand = cmd
                                Using dt As New DataTable()
                                    sda.Fill(dt)
                                    GridDt.DataSource = dt
                                    GridDt.DataBind()
                                End Using
                            End Using
                        End Using
                    End If
                End If
            Next
        End Using
    End Sub

    'Protected Sub GetSelectedRecords(ByVal sender As Object, ByVal e As EventArgs) Handles btnApply.Click
    'Dim dt As New DataTable()
    'Dim dr As DataRow
    'Dim RowData As DataRow()
    ''dt.Columns.AddRange(New DataColumn(1) {New DataColumn("DokCode"), New DataColumn("DokName")})
    'For Each Row As GridViewRow In GVDocument.Rows
    '    If Row.RowType = DataControlRowType.DataRow Then
    '        Dim chkRow As CheckBox = TryCast(Row.Cells(0).FindControl("chkRow"), CheckBox)
    '        If chkRow.Checked Then
    '            'Dim sCode As String = row.Cells(1).Text  'row.Cells(2).Text '
    '            'Dim sName As String = TryCast(row.Cells(2).FindControl("lblDokCode"), Label).Text
    '            'dt.Rows.Add(sCode, sName)
    '            'dt.Rows.Add("D0002", "Kartu Keluarga (KK)")
    '            'MessageDlg("Code : " + row.Cells(1).Text)

    '            'For Each RowData As GridViewRow In GridDt.Rows
    '            'RowData.Cells(2).Text = row.Cells(1).Text
    '            'RowData.Cells(3).Text = row.Cells(2).Text
    '            'Next
    '            'GridDt.SelectedRow.Cells(1).Text = row.Cells(1).Text
    '            'GridDt.SelectedRow.Cells(3).Text = row.Cells(2).Text
    '            RowData = ViewState("Dt").Select("TransNmbr = " + QuotedStr(tbCode.Text))
    '            dr = ViewState("Dt").NewRow
    '            dr("ItemNo") = 1 'drResult("ItemNo")
    '            dr("DokCode") = Row.Cells(1).Text
    '            dr("DocName") = Row.Cells(2).Text
    '            dr("Remark") = "" 'drResult("Remark")
    '            dr("FgActive") = "Y"
    '            ViewState("Dt").Rows.Add(dr)
    '        End If
    '    End If
    '    'GridDt.DataSource = dt
    '    'GridDt.DataBind()
    'Next
    'BindGridDt(ViewState("Dt"), GridDt)
    'GridDt.DataSource = dt
    'GridDt.DataBind()
    'End Sub

    Protected Sub GVDocument_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVDocument.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes("onmouseover") = "this.style.backgroundColor='#A1DCF2';this.style.cursor='hand';"
            'e.Row.Attributes("onmouseover") = "this.style.backgroundColor='aquamarine';"
            e.Row.Attributes("onmouseout") = "this.style.backgroundColor='white';"
            e.Row.Attributes("style") = "cursor:pointer"
            'e.Row.Attributes("ondblclick") = "javascript:ClosePopupDocument();"
        End If
    End Sub

    Protected Sub btnApply_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApply.Click
        GetSelectedRecords()
    End Sub

    Protected Sub GVListNoLS_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVListNoLS.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes("onmouseover") = "this.style.backgroundColor='#A1DCF2';this.style.cursor='hand';"
            'e.Row.Attributes("onmouseover") = "this.style.backgroundColor='aquamarine';"
            e.Row.Attributes("onmouseout") = "this.style.backgroundColor='white';"
            e.Row.Attributes("style") = "cursor:pointer"
            e.Row.Attributes("ondblclick") = "javascript:ClosePopupListNoLS();"
        End If
    End Sub

    Protected Sub GVPembeli_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVPembeli.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes("onmouseover") = "this.style.backgroundColor='#A1DCF2';this.style.cursor='hand';"
            'e.Row.Attributes("onmouseover") = "this.style.backgroundColor='aquamarine';"
            e.Row.Attributes("onmouseout") = "this.style.backgroundColor='white';"
            e.Row.Attributes("style") = "cursor:pointer"
            e.Row.Attributes("ondblclick") = "javascript:ClosePopupPembeli();"
        End If
    End Sub

    'Protected Sub btnDeleteAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDeleteAll.Click
    '    Try
    '        Dim dr() As DataRow
    '        dr = ViewState("Dt").Select()
    '        dr(0).Delete()
    '        BindGridDt(ViewState("Dt"), GridDt)
    '        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
    '    Catch ex As Exception
    '        lbStatus.Text = "Deleting All Item Error : " + ex.ToString
    '    End Try
    'End Sub

    'Protected Sub lbSeller_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSeller.Click
    '    Try
    '        AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsSeller')();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lbStatus.Text = "lbSeller_Click Error : " + ex.ToString
    '    End Try
    'End Sub

    'Protected Sub btnDeleteDoc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDeleteDoc.Click
    '    Dim strSQL As String
    '    strSQL = "UPDATE PRCIPRecvDocHd SET SignRecv=NULL WHERE TransNmbr = '" & ViewState("TransNmbr") & "'"
    '    SQLExecuteNonQuery(strSQL, ViewState("DBConnection").ToString)
    '    'MovePanel(pnlInput, PnlHd)
    '    'lbSignRecv.Text = ""
    '    ModifyInput2(False, pnlInput, PnlDt, GridDt)
    '    btnHome.Visible = True
    '    btnSaveTrans.Visible = True
    'End Sub

    Protected Sub btnNoLandSurvey_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNoLandSurvey.Click
        Dim ResultField, CriteriaField, strSQL As String
        Try
            'Session("filter") = "SELECT TransNmbr, TransDate, HrgTanah, LuasUkur, TtlHrgTanah, SellCode, SellName FROM V_GLLandPurchaseReqHd WHERE Status='P' "
            strSQL = "SELECT A.TransNmbr, A.TransDate, A.HrgTanah, A.HrgFix AS [LuasUkur], A.TtlHrgTanah AS [TotalHrgTanah], A.SellCode AS [SellerCode], A.SellName AS [SellerName], A.Pembeli " + _
                     "FROM V_GLLandPurchaseReqHd A WHERE A.Status='P' AND " + _
                     "A.TransNmbr NOT IN (SELECT B.LSNo FROM PRCLandPurchaseTJHd B WHERE B.Status<>'D')"
            Session("filter") = strSQL
            ResultField = "TransNmbr, TransDate, HrgTanah, LuasUkur, TotalHrgTanah, SellerCode, SellerName, Pembeli"
            CriteriaField = "TransNmbr, TransDate, HrgTanah, LuasUkur, TotalHrgTanah, SellerCode, SellerName, Pembeli"
            ViewState("CriteriaField") = CriteriaField.Split(",")
            Session("Column") = ResultField.Split(",")
            ViewState("Sender") = "btnNoLandSurvey"
            Session("DBConnection") = ViewState("DBConnection")
            'AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
            AttachScript("OpenPopup();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnNoLandSurvey_Click Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub ddlTerm_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTerm.SelectedIndexChanged
    '    tbDueDate.SelectedDate = FindDueDate(ddlTerm.SelectedValue, tbDate.SelectedDate, ViewState("DBConnection").ToString)
    'End Sub

    Protected Sub lbWakilPembeli_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbWakilPembeli.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsTugas')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lbWakilPembeli_Click Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub tbDokumen_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbDokumen.TextChanged, tbDate.SelectionChanged
        Dim Dokumen As Integer
        Try
            'tbDateDokumen.SelectedDate = tbDate.SelectedDate + tbDokumen.Text
            Dokumen = tbDokumen.Text
            tbDateDokumen.SelectedDate = Format(DateAdd(DateInterval.Day, Dokumen, tbDate.SelectedDate), "dd MMMM yyyy")

            'lbStatus.Text = Format(DateAdd(DateInterval.Day, Dokumen, tbDate.SelectedDate), "dd MMMM yyyy")

            'Exit Sub
        Catch ex As Exception
            lbStatus.Text = "ddlCategory_SelectedIndexChanged Error : " + ex.ToString
        End Try
    End Sub
End Class
