Imports System.Data
Imports System.Data.SqlClient
'Imports System.Data.DataSet
Imports System.IO
Imports System.Web.UI.WebControls


Partial Class Transaction_TrCIPLicenRecvDoc_TrCIPLicenRecvDoc
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "SELECT * FROM V_PRCIPRecvDocHd"

    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT * FROM V_PRCIPRecvDocDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Private Function GetStringDt3(ByVal Nmbr As String) As String
        Return "SELECT * From V_PRCIPRecvDocDt2 WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Try
            If Not IsPostBack Then
                InitProperty()
                SetInit()
                BindGridKegiatan()
                BindGridArea()
                BindGridDocument()
                'RefreshGridDetil()
                'fupSignRecv.Attributes.Add("onchange", "document.getElementById('" + lblSignRecv.ClientID + "').value=document.getElementById('" + fupSignRecv.ClientID + "').value")
                'fupSignRecv.Attributes.Add("onchange", "document.getElementById('" + fupSignRecv.ClientID + "').value = document.getElementById('" + tbApplfileNo.ClientID + "').value")
                Session("AdvanceFilter") = ""
            End If
            lbStatus.Text = ""
            'BtnGo.Visible = ddlCommand.Visible
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If

            If Not Session("Result") Is Nothing Then
                 

                If ViewState("Sender") = "btnGetData" Then
                    Dim drResult, dr As DataRow
                    Dim Row As DataRow()
                    For Each drResult In Session("Result").Rows
                        'Row = ViewState("Dt").Select("Product = " + QuotedStr(drResult("Product")) + " AND RequestNo = " + QuotedStr(drResult("PR_No")) + " AND PRDelivery = " + QuotedStr(drResult("DeliveryDate")) + " AND Delivery = " + QuotedStr(drResult("DeliveryDate")))
                        If CekExistData(ViewState("Dt"), "DokCode", drResult("DokCode")) Then
                            'lbStatus.Text = "Document Name :  " + drResult("DokName") + " has been already exist"
                            'Response.Write("Document Name :  " + drResult("DokName") + " has been already exist")
                            'Response.Write("<script language='javascript'>{alert('Document Name has been already exist'); }</script>")
                            Exit Sub
                        Else
                            Row = ViewState("Dt").Select("TransNmbr = " + QuotedStr(tbCode.Text))
                            'If Row.Count = 0 Then
                            dr = ViewState("Dt").NewRow
                            'dr("ItemNo") = drResult("ItemNo")
                            dr("DokCode") = drResult("DokCode")
                            dr("DokName") = drResult("DokName")
                            dr("Remark") = "" 'drResult("Remark")
                            dr("FgActive") = "Y"
                            ViewState("Dt").Rows.Add(dr)
                        End If
                        'Else
                        'dr = ViewState("Dt").Select("Product = " + QuotedStr(drResult("Product")) + " AND RequestNo = " + QuotedStr(drResult("PR_No")))(0)
                        'dr.BeginEdit()
                        'If drResult("QtySchedule") <> dr("Qty") Then
                        '    dr("Qty") = dr("Qty") + CFloat(drResult("QtySchedule"))
                        'End If
                        'End If
                    Next
                'GenerateDt2()
                    'GenerateDtDelivery()
                    'btnHome.Visible = True
                    'btnSaveTrans.Visible = True
                    'btnSaveAll.Visible = True
                    BindGridDt(ViewState("Dt"), GridDt)
                    ModifyInput2(True, pnlInput, PnlDt, GridDt)
                    'EnableHd(GetCountRecord(ViewState("Dt")) <> 0)
                'ModifyDt()   'dr("SubCategoryCode") = TrimStr(drResult("SubCategoryCode"))
                End If

                If ViewState("Sender") = "btnReference" Then
                    Dim drResult As DataRow
                    Dim ExistRow As DataRow()
                    For Each drResult In Session("Result").Rows
                        ExistRow = ViewState("Dt3").Select("Reference = " + QuotedStr(drResult("Reference").ToString))
                        If ExistRow.Count = 0 Then
                            'insert
                            Dim dr As DataRow
                            dr = ViewState("Dt3").NewRow
                            dr("ItemNo") = ViewState("Dt3").Rows.Count + 1
                            dr("Reference") = drResult("Reference")
                            ViewState("Dt3").Rows.Add(dr)
                        End If
                    Next
                    BindGridDt(ViewState("Dt3"), GridDt3)                    
                    EnableHd(GetCountRecord(ViewState("Dt3")) = 0)
                End If

            Session("Result") = Nothing
            ViewState("Sender") = Nothing
            Session("filter") = Nothing
            Session("Column") = Nothing
            End If

        FubInv.Attributes("onchange") = "UploadInvoice(this)"

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
            ViewState("SortExpression") = Nothing
            ViewState("DigitCurr") = 2
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
            If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
                'ddlCommand.Items.Add("Print")
                'ddlCommand2.Items.Add("Print")
            End If
            'tbRequestByName.Attributes.Add("ReadOnly", "True")
            tbDocName.Attributes.Add("ReadOnly", "True")
            'tbPeriod.Attributes.Add("ReadOnly", "True")
            'tbQty.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Catch ex As Exception
            Throw New Exception("Set Init Error : " + ex.ToString)
        End Try
    End Sub



    Protected Sub btnsaveINV_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsaveINV.Click
        Try

            Dim dr As DataTable
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("save this transaction first, to upload the dokumen")
                Exit Sub
            End If

            If FubInv.FileBytes.Length > 10000000 Then
                lbStatus.Text = MessageDlg("Ukuran File Terlalu Besar. !! Max Upload 10Mb")
                Exit Sub
            End If

            If Right(FubInv.FileName, 4) <> ".pdf" Then
                lbStatus.Text = MessageDlg("Upload Pdf File Only !")
                Exit Sub
            End If
            Dim path2, namafile2, SQLString1 As String
            Dim dt As DataTable
            path2 = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubInv.FileName
            namafile2 = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubInv.FileName

            SQLString1 = "UPDATE PRCIPRecvDocHd SET SignRecv = " + QuotedStr(namafile2) + _
            " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            FubInv.SaveAs(path2)
            SQLExecuteNonQuery(SQLString1, ViewState("DBConnection").ToString)

            dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            lbDokInv.Text = dt.Rows(0)("SignRecv").ToString
            'lblmassageKTP.Visible = True
            FubInv.Visible = False
            btnClearInv.Visible = True

        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbDokInv_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbDokInv.Click
        Try
            Dim dr As DataTable
            Dim filePath, URL As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            If dr.Rows(0)("SignRecv").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            filePath = dr.Rows(0)("SignRecv").ToString
            URL = ResolveUrl("~/Dokumen/" + filePath)
            Dim s As String = "window.open('" & URL & "', '_blank');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)

        Catch ex As Exception
            lbStatus.Text = "lbModerator_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnClearInv_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClearInv.Click
        Try
            Dim dr As DataTable
            Dim filePath As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            filePath = dr.Rows(0)("SignRecv").ToString


            If File.Exists(Server.MapPath("~/Dokumen/" + filePath)) = True Then
                File.Delete(Server.MapPath("~/Dokumen/" + filePath))
                SQLExecuteNonQuery("UPDATE PRCIPRecvDocHd Set SignRecv = '' WHERE TransNmbr = '" + tbCode.Text + "' ", ViewState("DBConnection").ToString)

                lbDokInv.Text = "Not yet uploaded"
                FubInv.Visible = True
                btnClearInv.Visible = False
            End If

           

        Catch ex As Exception
            lbStatus.Text = "lbBAP_Click Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        Dim index As Integer
        Try
            index = Int32.Parse(e.Item.Value)
            MultiView1.ActiveViewIndex = index
            btnSaveTrans.Focus()

            

            If MultiView1.ActiveViewIndex = 2 Then

                lbStatus.Text = ViewState("StateHD") 
                pnlDt3.Visible = True
                pnlEditDt3.Visible = False
                BindDataDt3(ViewState("TransNmbr"))
                If ViewState("StateHd")  = "Edit" OR ViewState("StateHd")  = "Insert" Then
                   GridDt3.Columns(0).Visible = True
                   btnGetReference.Visible = True
                   Else
                   GridDt3.Columns(0).Visible = False
                    btnGetReference.Visible = False
                End If

                'If lbStatus.Text = "False" Then
                '    GridDt3.Columns(0).Visible = False
                'End If
            End If

        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
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

    Private Sub BindGridKegiatan()
        Dim conStr As String = ViewState("DBConnection").ToString
        Using con As New SqlConnection(conStr)
            Using cmd As New SqlCommand("SELECT KegiatanCode, KegiatanName FROM MsKegiatan ORDER BY KegiatanCode") ' WHERE FgActive='Y' ")
                Using sda As New SqlDataAdapter()
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    Using dt As New DataTable()
                        sda.Fill(dt)
                        GVKegiatan.DataSource = dt
                        GVKegiatan.DataBind()
                    End Using
                End Using
            End Using
        End Using
    End Sub

    Private Sub BindGridArea()
        Dim conStr As String = ViewState("DBConnection").ToString
        Using con As New SqlConnection(conStr)
            Using cmd As New SqlCommand("SELECT AreaCode, AreaName FROM MsArea ORDER BY AreaCode") ' WHERE FgActive='Y' ")
                Using sda As New SqlDataAdapter()
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    Using dt As New DataTable()
                        sda.Fill(dt)
                        GVArea.DataSource = dt
                        GVArea.DataBind()
                    End Using
                End Using
            End Using
        End Using
    End Sub

    Protected Sub OnDataBoundActivity(ByVal sender As Object, ByVal e As EventArgs)
        Dim Row As New GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal)
        For i As Integer = 0 To GVKegiatan.Columns.Count - 1
            Dim cell As New TableHeaderCell()
            Dim txtSearchKegiatan As New TextBox()
            txtSearchKegiatan.Attributes("placeholder") = GVKegiatan.Columns(i).HeaderText
            txtSearchKegiatan.CssClass = "search_textbox"
            cell.Controls.Add(txtSearchKegiatan)
            Row.Controls.Add(cell)
        Next
        GVKegiatan.HeaderRow.Parent.Controls.AddAt(1, Row)
    End Sub

    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue, ddlRange.SelectedValue)
            If AdvanceFilter.Length > 1 And StrFilter.Length > 1 Then
                StrFilter = StrFilter + " And " + AdvanceFilter
            ElseIf AdvanceFilter.Length > 1 And StrFilter.Length <= 1 Then
                StrFilter = AdvanceFilter
            End If
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

     Private Sub BindDataDt3(ByVal Nmbr As String)
        Dim Drow As DataRow()
        Try
            Dim dt As New DataTable
            ViewState("Dt3") = Nothing
            dt = SQLExecuteQuery(GetStringDt3(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt3") = dt
            BindGridDt(dt, GridDt3)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt3 Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub RefreshGridDetil()
        Dim conStr As String = ViewState("DBConnection").ToString
        Using con As New SqlConnection(conStr)
            Using cmd As New SqlCommand("SELECT * FROM PRCIPRecvDocDt")
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
        End Using
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
        '-----------------------------------------------------------------------------'
        'Dim strSQL = "S_FINPayGetLandPurchase"
        'Dim conStr As String = ViewState("DBConnection").ToString
        'Using con As New SqlConnection(conStr)
        '    'con.Open()
        '    Using cmd As New SqlCommand(strSQL, con)
        '        Using sda As New SqlDataAdapter()
        '            cmd.Connection = con
        '            cmd.CommandType = CommandType.StoredProcedure
        '            sda.SelectCommand = cmd
        '            Using dt As New DataTable()
        '                sda.Fill(dt)
        '                GVNoLandSurvey.DataSource = dt
        '                GVNoLandSurvey.DataBind()
        '            End Using
        '            'con.Close()
        '        End Using
        '    End Using
        'End Using
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        Try
            GridView1.PageIndex = 0
            BindData(Session("AdvanceFilter"))
            pnlNav.Visible = True
            'FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))

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
                        ListSelectNmbr = GVR.Cells(2).Text
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
                GetListCommand(Status, GridView1, "3,2", ListSelectNmbr, Nmbr, lbStatus.Text)
                If ListSelectNmbr = "" Then Exit Sub
                For j = 0 To (Nmbr.Length - 1)
                    If Nmbr(j) = "" Then
                        Exit For
                    Else
                        Result = ExecSPCommandGo(ActionValue, "S_PRCIPRecvDoc", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDt(ByVal Nmbr As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt") = Nothing
            dt = SQLExecuteQuery(GetStringDt(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt") = dt
            BindGridDt(dt, GridDt)
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
            tbCode.Text = ""
            tbKegiatanCode.Text = ""
            tbKegiatanName.Text = ""
            tbAreaCode.Text = ""
            tbAreaName.Text = ""
            tbPICName.Text = ""
            tbApplfileNo.Text = ""
            tbBrokerName.Text = ""
            tbBrokerPhone.Text = ""
            'ddlReport.SelectedValue = "Y"
            'ddlUserType.SelectedValue = "Supplier"
            tbDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            tbApplfileDate.SelectedDate = Now.Date
            'tbBeaNotaris.Text = FormatFloat(0, ViewState("DigitCurr"))
            tbRelatedOffcName.Text = ""
            tbRelatedOffcPhone.Text = ""
            tbAlasHak.Text = ""
            'tbBeaTanah.Text = "0"
            'tbTotalSelisih.Text = "0"
            tbRemark.Text = ""
            lbSignRecv.Text = ""
            tbReference.text = ""
            ddlTypeCIP.SelectedIndex = 2
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
        Try
            'If CInt(Session(Request.QueryString("KeyId"))("Year")) <> Year(tbDate.SelectedValue) Or CInt(Session(Request.QueryString("KeyId"))("Period")) <> Month(tbDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(Session(Request.QueryString("KeyId"))("Period")) + " " + Session(Request.QueryString("KeyId"))("Year").ToString.Trim)
            '    Return False
            'End If
            If tbDate.SelectedDate = Nothing Then
                lbStatus.Text = MessageDlg("Tgl. Terima Dokumen must have value")
                tbDate.Focus()
                Return False
            End If
            If tbApplfileNo.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("No. Berkas Permohonan must have value")
                tbApplfileNo.Focus()
                Return False
            End If
            If tbApplfileDate.SelectedDate = Nothing Then
                lbStatus.Text = MessageDlg("Tgl. Berkas Permohonan must have value")
                tbApplfileDate.Focus()
                Return False
            End If
            If tbPICName.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("PIC must have value")
                tbPICName.Focus()
                Return False
            End If
            If tbBrokerName.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Nama Perantara must have value")
                tbBrokerName.Focus()
                Return False
            End If
            If tbBrokerPhone.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("No. Telp Perantara must have value")
                tbBrokerPhone.Focus()
                Return False
            End If
            If tbRelatedOffcName.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Nama Pejabat Terkait must have value")
                tbRelatedOffcName.Focus()
                Return False
            End If
            If tbRelatedOffcPhone.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("No. Telp Pejabat Terkait must have value")
                tbRelatedOffcPhone.Focus()
                Return False
            End If
            If tbAlasHak.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Alas Hak must have value")
                tbAlasHak.Focus()
                Return False
            End If
            ' If tbRemark.Text.Trim = "" Then
            '     lbStatus.Text = MessageDlg("Remark must have value")
            '     tbRemark.Focus()
            '     Return False
            ' End If
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
                If tbDocCode.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Job Name Must Have Value")
                    tbDocCode.Focus()
                    Return False
                End If
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

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            tbCode.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            'BindToDropList(ddlReport, Dt.Rows(0)("FgReport").ToString)
            'BindToDropList(ddlUserType, Dt.Rows(0)("UserType").ToString)
            BindToDate(tbApplfileDate, Dt.Rows(0)("ApplfileDate").ToString)
            BindToText(tbKegiatanCode, Dt.Rows(0)("KegiatanCode").ToString)
            BindToText(tbKegiatanName, Dt.Rows(0)("KegiatanName").ToString)
            BindToText(tbAreaCode, Dt.Rows(0)("AreaCode").ToString)
            BindToText(tbAreaName, Dt.Rows(0)("AreaName").ToString)
            BindToText(tbPICName, Dt.Rows(0)("PICName").ToString)
            BindToText(tbApplfileNo, Dt.Rows(0)("ApplfileNo").ToString)
            BindToText(tbAlasHak, Dt.Rows(0)("HGBNo").ToString)
            BindToText(tbBrokerName, Dt.Rows(0)("BrokerName").ToString)
            BindToText(tbBrokerPhone, Dt.Rows(0)("BrokerPhone").ToString)
            BindToText(tbRelatedOffcName, Dt.Rows(0)("RelatedOffcName").ToString)
            BindToText(tbRelatedOffcPhone, Dt.Rows(0)("RelatedOffcPhone").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
            BindToText(tbReference, Dt.Rows(0)("Reference").ToString)
            BindToDropList(ddlTypeCIP, Dt.Rows(0)("TypeCIP").ToString)
            ' lbSignRecv.Text = Dt.Rows(0)("SignRecv").ToString

            
            If Dt.Rows(0)("SignRecv").ToString = "" Then
                'cbKtp.Checked = False
                lbDokInv.Text = "Not Yet Uploaded - Max Upload 10Mb"
                FubInv.Visible = True
            Else
                lbDokInv.Text = Dt.Rows(0)("SignRecv").ToString
                'cbKtp.Checked = True
                 FubInv.Visible = False
            End If

            'fupSignRecv.Visible = False
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
                
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow               
                dr("DokCode") = tbDocCode.Text
                dr("DokName") = tbDocName.Text             
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
        Dim SQLString As String
        Dim I As Integer
        Try
            ' System.Threading.Thread.Sleep(7000)
            If Not (ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit") Then
                Exit Sub
            Else
                ViewState("StateSave") = ViewState("StateHd")
                ViewState("StateHd") = "View"
            End If
            'Save Hd
            If ViewState("StateSave") = "Insert" Then
                If SQLExecuteScalar("SELECT TransNmbr FROM PRCIPRecvDocHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lbStatus.Text = "No. Terima Dokumen" + QuotedStr(tbCode.Text) + " has already been exist"
                    Exit Sub
                End If

                'ddlReport.SelectedValue
                tbCode.Text = GetAutoNmbr("CTD", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)

                ' Dim path, namafile As String
                ' path = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + fupSignRecv.FileName
                ' namafile = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + fupSignRecv.FileName

                SQLString = "INSERT INTO PRCIPRecvDocHd(TransNmbr,TransDate,Status,ApplfileNo,ApplfileDate,HGBNo,KegiatanCode,AreaCode,PICName,BrokerName, " + _
                "BrokerPhone,RelatedOffcName,RelatedOffcPhone,Remark,UserPrep,DatePrep,FgReport,FgActive, Reference, TypeCIP) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", 'H', " + _
                QuotedStr(tbApplfileNo.Text) + ", " + QuotedStr(Format(tbApplfileDate.SelectedValue, "yyyy-MM-dd")) + ", " + _
                QuotedStr(tbAlasHak.Text) + ", " + QuotedStr(tbKegiatanCode.Text) + ", " + QuotedStr(tbAreaCode.Text) + ", " + _
                QuotedStr(tbPICName.Text) + ", " + QuotedStr(tbBrokerName.Text) + ", " + QuotedStr(tbBrokerPhone.Text) + ", " + _
                QuotedStr(tbRelatedOffcName.Text) + ", " + QuotedStr(tbRelatedOffcPhone.Text) + ", " + _
                QuotedStr(tbRemark.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", GetDate()," + QuotedStr("0") + ", " + QuotedStr("Y") + ", " + _
                QuotedStr(tbReference.Text) + ", " + QuotedStr(ddlTypeCIP.SelectedValue)
                ViewState("TransNmbr") = tbCode.Text 'ddlUserType.SelectedValue, tbAttn.Text, tbTotalSelisih.Text.Replace(",", "")
                ' fupSignRecv.SaveAs(path)
            Else
                ''ElseIf ViewState("StateHd") = "Edit" Then
                'Dim cekStatus As String
                'cekStatus = SQLExecuteScalar("SELECT Status FROM PRCIPRecvDocHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                'If cekStatus = "P" Then
                '    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                '    Exit Sub
                'End If

                ' Dim path, namafile As String
                ' path = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + fupSignRecv.FileName
                ' namafile = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + fupSignRecv.FileName

                SQLString = "UPDATE PRCIPRecvDocHd SET TransDate = " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + _
                ", ApplfileNo =" + QuotedStr(tbApplfileNo.Text) + ", ApplfileDate = " + QuotedStr(Format(tbApplfileDate.SelectedValue, "yyyy-MM-dd")) + _
                ", HGBNo = " + QuotedStr(tbAlasHak.Text) + ", KegiatanCode = " + QuotedStr(tbKegiatanCode.Text) + ", AreaCode = " + QuotedStr(tbAreaCode.Text) + _
                ", PICName = " + QuotedStr(tbPICName.Text) + ", BrokerName = " + QuotedStr(tbBrokerName.Text) + ", BrokerPhone = " + QuotedStr(tbBrokerPhone.Text) + _
                ", RelatedOffcName = " + QuotedStr(tbRelatedOffcName.Text) + ", RelatedOffcPhone = " + QuotedStr(tbRelatedOffcPhone.Text) + _
                ", Remark = " + QuotedStr(tbRemark.Text) + ", UserPrep= " + QuotedStr(ViewState("UserId").ToString) + ", DatePrep = GetDate()" + _
                ", Reference = " + QuotedStr(tbReference.text) + ", TypeCIP = " + QuotedStr(ddlTypeCIP.SelectedValue) + _
                " WHERE TransNmbr = " + QuotedStr(tbCode.Text) 'ddlUserType.SelectedValue, tbAttn.Text, tbTotalSelisih.Text.Replace(",", "")
                ' fupSignRecv.SaveAs(path)
            End If
            SQLString = ChangeQuoteNull(SQLString)
            SQLString = SQLString.Replace("'1900-01-01'", "NULL")
            SQLString = SQLString.Replace("'0001-01-01'", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            'update Primary Key on Dt
            Dim Row As DataRow()

            Row = ViewState("Dt").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbCode.Text
                Row(I).EndEdit()
            Next

            Row = ViewState("Dt3").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbCode.Text
                Row(I).EndEdit()
            Next


            'save dt
            'SQLString = "DELETE FROM CNTCostPlanDt WHERE TransNmbr = " + QuotedStr(tbProjectCode.Text) + " AND JobCode = " + QuotedStr(tbProductCode.Text) + _
            '" AND MaterialCode = " + QuotedStr(tbItemCode.Text)
            'SQLExecuteScalar(SQLString, ViewState("DBConnection").ToString)

            Dim ConnString As String = ViewState("DBConnection").ToString
            con = New SqlConnection(ConnString)
            con.Open()                   'ItemNo,            
            SQLString = "SELECT TransNmbr,DokCode,Remark,FgActive FROM PRCIPRecvDocDt WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            '            SQLString = "SELECT TransNmbr,ItemNo,JobCode,Qty,Unit,Price,Amount,Remark,FgActive,DoneClosing FROM CNTRABDt WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr"))
            Dim cmdSql As New SqlCommand(SQLString, con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            '' Create the UpdateCommand.
            SQLString = "UPDATE PRCIPRecvDocDt SET DokCode = @DokCode, Remark = @Remark " + _
            " FROM PRCIPRecvDocDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "'"
            Dim Update_Command = New SqlCommand(SQLString, con)
            '             "FgActive = @FgActive FROM CNTCostPlanDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "'", con) @FgActive
            ' Define output parameters.
            Update_Command.Parameters.Add("@DokCode", SqlDbType.VarChar, 12, "DokCode")
            Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 255, "Remark")
            '
            da.UpdateCommand = Update_Command

            '' Create the DeleteCommand.
            SQLString = "DELETE FROM PRCIPRecvDocDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "'"
            Dim Delete_Command = New SqlCommand(SQLString, con) 'ViewState("Reference")
          
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("PRCIPRecvDocDt")
            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt

            'save dt3
            cmdSql = New SqlCommand("SELECT TransNmbr, ItemNo, Reference FROM PRCIPRecvDocDt2 WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            dbcommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

           

            Dim Dt3 As New DataTable("PRCIPRecvDocDt2")

            Dt3 = ViewState("Dt3")
            da.Update(Dt3)
            Dt3.AcceptChanges()
            ViewState("Dt3") = Dt3

            
        Catch ex As Exception
            Throw New Exception("Save All Data Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSaveTrans_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveTrans.Click
        Dim CurrFilter, Value As String
        Try

        Dim confirmValue As String = Request.Form("confirm_value")
            If confirmValue = "Yes" Then

            If CekHd() = False Then
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
                Exit Sub
            End If
            For Each dr In ViewState("Dt").Rows
                If CekDt(dr) = False Then
                    Exit Sub
                End If
            Next

             SaveAll()
                ModifyInput2(False, pnlInput, pnlDt, GridDt)
                btnGoEdit.Visible = True
                Menu2.Items.Item(1).Enabled = True
                MultiView2.ActiveViewIndex = 1
                Menu2.Items.Item(1).Selected = True
                'btnGoEdit.Visible = True
                ' btnGetBAP.Visible = False
                GridDt.Columns(0).Visible = False

            Else
            If CekHd() = False Then
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
                Exit Sub
            End If
            For Each dr In ViewState("Dt").Rows
                If CekDt(dr) = False Then
                    Exit Sub
                End If
            Next

            SaveAll()
            MovePanel(pnlInput, PnlHd)
            CurrFilter = tbFilter.Text
            Value = ddlField.SelectedValue
            tbFilter.Text = tbCode.Text
            ddlField.SelectedValue = "TransNmbr"
            btnSearch_Click(Nothing, Nothing)
            tbFilter.Text = CurrFilter
            ddlField.SelectedValue = Value
            End If
            ' If CekHd() = False Then
            '     Exit Sub
            ' End If
            ' 'If GetCountRecord(ViewState("Dt")) = 0 Then
            ' '    lbStatus.Text = MessageDlg("Detail must have at least 1 record")
            ' '    Exit Sub
            ' 'End If

            ' SaveAll()
            ' MovePanel(pnlInput, PnlHd)
            ' CurrFilter = tbFilter.Text
            ' Value = ddlField.SelectedValue
            ' tbFilter.Text = tbCode.Text
            ' ddlField.SelectedValue = "TransNmbr"
            ' btnSearch_Click(Nothing, Nothing)
            ' tbFilter.Text = CurrFilter
            ' ddlField.SelectedValue = Value
        Catch ex As Exception
            lbStatus.Text = "btnSaveTrans Error : " + ex.ToString
        End Try
    End Sub
    

    
        Protected Sub btnGoEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGoEdit.Click
        Dim CurrFilter, Value As String
        Try
            MovePanel(pnlInput, PnlHd)
            CurrFilter = tbFilter.Text
            Value = ddlField.SelectedValue
            tbFilter.Text = tbCode.Text
            ddlField.SelectedValue = "TransNmbr"
            btnSearch_Click(Nothing, Nothing)
            tbFilter.Text = CurrFilter
            ddlField.SelectedValue = Value
            btnGoEdit.Visible = False
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub
    
    Protected Sub Menu2_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu2.MenuItemClick
        MultiView2.ActiveViewIndex = Int32.Parse(e.Item.Value)
        If Menu2.Items.Item(0).Selected = True Then
            If ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit" Then
                'btnGoEdit.Visible = False
                'btnGetBAP.Visible = True
                GridDt.Columns(0).Visible = True
            End If
        End If
        If Menu2.Items.Item(1).Selected = True Then
            If ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit" Then
                'btnGoEdit.Visible = True
                'btnGetBAP.Visible = False
                GridDt.Columns(0).Visible = False
            End If

        End If
    End Sub


    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            lbStatus.Text = ""
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, PnlDt, GridDt)
            newTrans()
            MultiView2.ActiveViewIndex = 0
            Menu2.Items.Item(0).Selected = True
            btnHome.Visible = False
            fupSignRecv.Visible = True
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
            ClearHd()
            Cleardt()
            PnlDt.Visible = True
            btnDeleteDoc.Visible = False
            BindDataDt("")
            BindDataDt3("")
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
                    GridDt.PageIndex = 0
                    FillTextBoxHd(ViewState("TransNmbr"))
                    BindDataDt(ViewState("TransNmbr"))
                    BindDataDt3(ViewState("TransNmbr"))                    
                    ViewState("StateHd")   = "View"
                    ModifyInput2(False, pnlInput, PnlDt, GridDt)
                    ModifyInput2(False, pnlInput, pnlDt3, GridDt3)                   

                    MultiView1.ActiveViewIndex = 0
                    Menu1.Items.Item(0).Selected = True
                    btnHome.Visible = True
                     MultiView2.ActiveViewIndex = 0
                    Menu2.Items.Item(0).Selected = True
                    If GVR.Cells(3).Text = "D" Then
                        Menu2.Items.Item(1).Enabled = False
                    Else
                        Menu2.Items.Item(1).Enabled = True
                    End If
                    ViewState("Status") = GVR.Cells(3).Text
                    fupSignRecv.Visible = False
                    fupSignRecv.Enabled = False
                    lbSignRecv.Visible = False
                    lbSignRecv.Enabled = False
                    btnDeleteDoc.Visible = False
                    btnSaveTrans.Visible = False


                ElseIf DDL.SelectedValue = "Edit" Then
                    If GVR.Cells(3).Text = "H" Or GVR.Cells(3).Text = "G" Then
                        CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        MovePanel(PnlHd, pnlInput)
                        ViewState("TransNmbr") = GVR.Cells(2).Text
                        GridDt.PageIndex = 0
                        BindDataDt(ViewState("TransNmbr"))
                        FillTextBoxHd(ViewState("TransNmbr"))
                        BindDataDt3(ViewState("TransNmbr"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, PnlDt, GridDt)
                         ModifyInput2(True, pnlInput, pnlDt3, GridDt3)
                         btnAddDt3ke2.Visible = False
                         btnAddDt3.Visible  = False
                        MultiView1.ActiveViewIndex = 0
                        Menu1.Items.Item(0).Selected = True
                        btnHome.Visible = True
                        
                        fupSignRecv.Visible = False
                        lbSignRecv.Visible = False
                        btnDeleteDoc.Visible = False
                        btnSaveTrans.Visible = True
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    ElseIf GVR.Cells(3).Text = "P" Then
                        CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        MovePanel(PnlHd, pnlInput)
                        ViewState("TransNmbr") = GVR.Cells(2).Text
                        GridDt.PageIndex = 0
                        BindDataDt(ViewState("TransNmbr"))
                        BindDataDt3(ViewState("TransNmbr"))
                        FillTextBoxHd(ViewState("TransNmbr"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(False, pnlInput, PnlDt, GridDt)
                         ModifyInput2(False, pnlInput, pnlDt3, GridDt3)
                        btnHome.Visible = True
                        btnSaveTrans.Visible = True
                        'btnSaveAll.Visible = True
                        fupSignRecv.Visible = False
                        lbSignRecv.Visible = False
                        lbSignRecv.Enabled = False
                        btnDeleteDoc.Visible = False
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                        'Else
                        '    lbStatus.Text = MessageDlg("Data must be Hold or Get Approval to edit")
                        '    Exit Sub
                    End If
                    'GridDt.Columns(1).Visible = False
                    'GridDt.Columns(8).Visible = False
                    'GridDt.Columns(9).Visible = False
                    'GridDt.Columns(10).Visible = False
                    'GridDt.Columns(11).Visible = False
                    'GridDt.Columns(12).Visible = False
                ElseIf DDL.SelectedValue = "Print" Then
                    Try
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        Session("SelectCommand") = "EXEC S_PDFormIO '''" + GVR.Cells(2).Text + "'''"
                        Session("ReportFile") = ".../../../Rpt/FormIO.frx"
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

     Protected Sub GridDt3_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt3.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow
            GVR = GridDt3.Rows(e.RowIndex)
            dr = ViewState("Dt3").Select("Reference = " + QuotedStr(GVR.Cells(2).Text))
            dr(0).Delete()
            BindGridDt(ViewState("Dt3"), GridDt3)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt 3 Row Deleting Error : " + ex.ToString
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
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
                Exit Sub
            End If
            For Each dr In ViewState("Dt").Rows
                If CekDt(dr) = False Then
                    Exit Sub
                End If
            Next
            SaveAll()
            newTrans()
            EnableHd(True)
        Catch ex As Exception
            lbStatus.Text = "btn save all error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub btnRequestBy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRequestBy.Click
    '    Dim ResultField As String
    '    Try
    '        Session("filter") = "SELECT Emp_No, Emp_Name FROM V_MsEmployee WHERE Fg_Active = 'Y' "
    '        ResultField = "Emp_No, Emp_Name"
    '        ViewState("Sender") = "btnRequestBy"
    '        Session("Column") = ResultField.Split(",")
    '        Session("DBConnection") = ViewState("DBConnection")
    '        AttachScript("OpenSearchDlg();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lbStatus.Text = "btnRequestBy_Click Error : " + ex.ToString
    '    End Try
    'End Sub

    'Protected Sub tbRequestBy_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbRequestBy.TextChanged
    '    Dim Dt As DataTable
    '    Dim Dr As DataRow
    '    Dim SQLString As String
    '    Try
    '        SQLString = "SELECT Emp_No, Emp_Name FROM V_MsEmployee WHERE Emp_No = " + QuotedStr(tbRequestBy.Text) + " AND Fg_Active = 'Y' "
    '        Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
    '        If Dt.Rows.Count > 0 Then
    '            Dr = Dt.Rows(0)
    '            tbRequestBy.Text = Dr("Emp_No")
    '            tbRequestByName.Text = Dr("Emp_Name")
    '        Else
    '            tbRequestBy.Text = ""
    '            tbRequestByName.Text = ""
    '        End If

    '    Catch ex As Exception
    '        Throw New Exception("tbRequestBy_TextChanged error: " + ex.ToString)
    '    End Try
    'End Sub

    'Protected Sub btnPeriod_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPeriod.Click
    '    Dim ResultField As String
    '    Try
    '        Session("filter") = "SELECT PeriodCode, Year, Start_Date, End_Date FROM VMsPeriod WHERE  FgClosing = 'N'"
    '        ResultField = "PeriodCode, Start_Date, End_Date"
    '        ViewState("Sender") = "btnPeriod"
    '        Session("Column") = ResultField.Split(",")
    '        Session("DBConnection") = ViewState("DBConnection")
    '        AttachScript("OpenSearchDlg();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lbStatus.Text = "btnResult_Click Error : " + ex.ToString
    '    End Try
    'End Sub

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

    Protected Sub tbProductCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbDocCode.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Dim SQLString As String
        Try
            'SQLString = "SELECT Product_Code, Product_Name, Specification, Unit FROM VMsProduct WHERE Fg_Active = 'Y' AND Fg_Stock = 'Y' AND ProductCategory = 'Finish Good' AND Product_Code = " + QuotedStr(tbProductCode.Text)
            'Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            'If Dt.Rows.Count > 0 Then
            '    Dr = Dt.Rows(0)
            '    tbProductCode.Text = Dr("Product_Code")
            '    tbProductName.Text = Dr("Product_Name")
            '    ddlUnit.SelectedValue = Dr("Unit")
            '    'tbSpecification.Text = Dr("Specification")
            'Else
            '    tbProductCode.Text = ""
            '    tbProductName.Text = ""
            '    ddlUnit.SelectedIndex = 0
            '    'tbSpecification.Text = ""
            'End If
            SQLString = "SELECT JobCode, JobName, Unit, Qty, Price, Remark FROM V_CNTRABDt WHERE Fg_Active = 'Y' AND Fg_Stock = 'Y' AND ProductCategory = 'Finish Good' AND Product_Code = " + QuotedStr(tbDocCode.Text)
            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbDocCode.Text = Dr("JobCode")
                tbDocName.Text = Dr("JobName")
                'ddlUnit.SelectedValue = Dr("Unit")
                'tbQty.Text = Dr("Qty")
                'tbPriceDt.Text = Dr("Price")
                tbRemarkDt.Text = Dr("Remark")
            Else
                tbDocCode.Text = ""
                tbDocName.Text = ""
                'ddlUnit.SelectedIndex = 0
                'tbQty.Text = 0
                'tbPriceDt.Text = 0
                tbRemarkDt.Text = ""
            End If

        Catch ex As Exception
            Throw New Exception("tbProductCode_TextChanged error: " + ex.ToString)
        End Try
    End Sub

    'Protected Sub btnProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProject.Click
    '    Dim ResultField As String
    '    Try
    '        Session("filter") = "SELECT TransNmbr AS ProjectNo, ProjectName, ContractNo, SPKNo, CustName AS CustomerName, Location, CityName AS City, ManagerName FROM V_MsProject WHERE FgActive = 'Y'"
    '        ResultField = "ProjectNo, ProjectName"
    '        'Session("Result") = Nothing

    '        ViewState("Sender") = "btnProject"
    '        Session("Column") = ResultField.Split(",")
    '        Session("DBConnection") = ViewState("DBConnection")
    '        AttachScript("OpenSearchDlg();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lbStatus.Text = "btnProject_Click Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub btnGetData_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetData.Click
        Dim ResultField, CriteriaField As String
        Try
            'fupSignRecv.Visible = True
            'lbSignRecv.Visible = True
            'Session("filter") = "EXEC S_CNTRABReff " + QuotedStr(tbCode.Text) 'WHERE Fg_Active = 'Y'
            Session("filter") = "EXEC S_CIPRecvMsDokumen "
            'Session("filter") = "SELECT Doc_Code,Document_Name,Remark FROM V_CIPRecvMsDokumen"
            ResultField = "Doc_Code,Document_Name,Remark"
            CriteriaField = "Doc_Code,Document_Name,Remark"
            ViewState("CriteriaField") = CriteriaField.Split(",")
            Session("Column") = ResultField.Split(",")
            ViewState("Sender") = "btnGetData"
            Session("DBConnection") = ViewState("DBConnection")
            'AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
            AttachScript("OpenPopup();", Page, Me.GetType())
            'ScriptManager.RegisterClientScriptBlock(sender, Me.GetType(), "alert", "window.open('FindMultiDlg.aspx')", True)
            'ScriptManager.RegisterStartupScript()
            'Response.Redirect()
            'Response.Write("<script>;location.href='" + url + "'</script>");
            'Response.Redirect("<script language='javascript'>window.open('../../FindMultiDlg.Aspx','Popup', 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=yes, copyhistory=no, width=' + 600 + ', height=' + 600 + ', top=' + top + ', left=' + left)</script>")
            'Page.ClientScript.RegisterStartupScript(Me.GetType(), "Popup", "window.open('../../FindMultiDlg.Aspx','Height=300px,Width=700px,menubar=No,toolbar=no,scrollbars=yes'); window.parent.location.reload(true)", True)
            'fupSignRecv.Attributes.Add("onclick", "document.getElementById('" + lblSignRecv.ClientID + "').value=document.getElementById('" + fupSignRecv.ClientID + "').value")
            'lblSignRecv.Visible = True
            'lblSignRecv.Text = lbSignRecv.Text
            'Session.Clear()
            'fupSignRecv.Attributes.Add("onchange", "document.getElementById('" + fupSignRecv.ClientID + "').value = document.getElementById('" + fupSignRecv.ClientID + "').value")
        Catch ex As Exception
            lbStatus.Text = "btnGetData_Click Error : " + ex.ToString
        End Try
    End Sub

    

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

    Protected Sub GVKegiatan_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVKegiatan.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes("onmouseover") = "this.style.backgroundColor='#A1DCF2';this.style.cursor='hand';"
            'e.Row.Attributes("onmouseover") = "this.style.backgroundColor='aquamarine';"
            e.Row.Attributes("onmouseout") = "this.style.backgroundColor='white';"
            e.Row.Attributes("style") = "cursor:pointer"
            e.Row.Attributes("ondblclick") = "javascript:ClosePopupKegiatan();"
        End If
    End Sub

    Protected Sub GVArea_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVArea.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes("onmouseover") = "this.style.backgroundColor='#A1DCF2';this.style.cursor='hand';"
            'e.Row.Attributes("onmouseover") = "this.style.backgroundColor='aquamarine';"
            e.Row.Attributes("onmouseout") = "this.style.backgroundColor='white';"
            e.Row.Attributes("style") = "cursor:pointer"
            e.Row.Attributes("ondblclick") = "javascript:ClosePopupArea();"
        End If
    End Sub

    Protected Sub lbSignRecv_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSignRecv.Click
        Try
            Dim dr As DataTable
            Dim filePath, URL As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If
            If dr.Rows(0)("SignRecv").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If
            filePath = dr.Rows(0)("SignRecv").ToString
            URL = ResolveUrl("~/Dokumen/" + filePath)
            Dim s As String = "window.open('" & URL & "', '_blank');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)

        Catch ex As Exception
            lbStatus.Text = "lbSignRecv_Click Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub lbKegiatan_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbKegiatan.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsKegiatan')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lbKegiatan_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbArea_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbArea.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsArea')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lbArea_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnDeleteDoc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDeleteDoc.Click
        Dim strSQL As String
        strSQL = "UPDATE PRCIPRecvDocHd SET SignRecv=NULL WHERE TransNmbr = '" & ViewState("TransNmbr") & "'"
        SQLExecuteNonQuery(strSQL, ViewState("DBConnection").ToString)
        'MovePanel(pnlInput, PnlHd)
        lbSignRecv.Text = ""
        ModifyInput2(False, pnlInput, PnlDt, GridDt)
        btnHome.Visible = True
        btnSaveTrans.Visible = True
    End Sub

    Protected Sub btnGetReference_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGetReference.Click
        Dim ResultField, CriteriaField As String
        Try
            Session("filter") = "SELECT DISTINCT Reference,TransDate,NoDokumen,JenisDoc,Luas,SellCode,SellName,Nilai,PBBNo,Remark FROM S_GetReferenceALL" 'WHERE User_Type = " + QuotedStr(ddlUserType.SelectedValue)
           ' Session("filter") = "EXEC S_GetReferenceALL"
            ResultField = "Reference, TransDate, PBBNo, JenisDoc, Luas, Remark"
            CriteriaField = "Reference, TransDate, PBBNo, JenisDoc, Luas, Remark"
            ViewState("CriteriaField") = CriteriaField.Split(",")
            Session("Column") = ResultField.Split(",")
            ViewState("Sender") = "btnReference"
            Session("DBConnection") = ViewState("DBConnection")
            'AttachScript("SearchMultiDlg();", Page, Me.GetType())
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "BtnRecvDoc Click Error : " + ex.ToString
        End Try
    End Sub
End Class
