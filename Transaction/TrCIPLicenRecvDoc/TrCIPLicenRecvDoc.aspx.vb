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

    Private Function GetStringDtTahapan(ByVal Nmbr As String) As String
        Return "SELECT * From V_PRCIPRecvDocDt3 WHERE TransNmbr = " + QuotedStr(Nmbr) + " ORDER BY ItemNo ASC"
    End Function

    Private Function GetStringDt2Tahapan(ByVal Nmbr As String) As String
        Return "SELECT * From V_PRCIPRecvDocDt4 WHERE TransNmbr = " + QuotedStr(Nmbr) + " ORDER BY ItemNoDt2 ASC"
    End Function

    Private Function GetStringDt5(ByVal Nmbr As String) As String
        Return "SELECT * From V_PRCIPRecvDocDt5 WHERE TransNmbr = " + QuotedStr(Nmbr) + " ORDER BY ItemNo ASC"
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

                If ViewState("Sender") = "btnPIC" Then
                    tbPIC.Text = Session("Result")(0).ToString
                    tbPICNameTahapan.Text = Session("Result")(1).ToString
                End If


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

                If ViewState("Sender") = "GetDokReceive" Then
                    Dim drResult As DataRow
                    Dim ExistRow As DataRow()
                    For Each drResult In Session("Result").Rows
                        ExistRow = ViewState("Dt5").Select("Reference+'|'+NoDokumen = " + QuotedStr(drResult("No_Dokumen_Recieve").ToString + "|" + drResult("No_Dokumen").ToString))
                        If ExistRow.Count = 0 Then
                            'insert
                            Dim dr As DataRow
                            dr = ViewState("Dt5").NewRow
                            dr("ItemNo") = ViewState("Dt5").Rows.Count + 1
                            dr("Reference") = drResult("No_Dokumen_Recieve").ToString
                            dr("NoDokumen") = drResult("No_Dokumen").ToString
                            ViewState("Dt5").Rows.Add(dr)
                        End If
                    Next

                    BindGridDt(ViewState("Dt5"), GridDt5)
                    EnableHd(GetCountRecord(ViewState("Dt5")) = 0)
                End If

                If ViewState("Sender") = "GetTahapan" Then
                    Dim drResult As DataRow
                    Dim ExistRow As DataRow()
                    Dim SqlTahapan As String
                    Dim DtTahapan As DataTable

                    ' === CLEAR DATA TABLE SEBELUM ISI BARU ===
                    If ViewState("DtTahapan") IsNot Nothing Then
                        CType(ViewState("DtTahapan"), DataTable).Rows.Clear()
                        '' === CLEAR Tahapan lama di DATABASE agar tidak bentrok saat save Edit data ===
                        'Dim SqlClear As String = "EXEC S_ClearTahapan " + QuotedStr(tbCode.Text)
                        'SQLExecuteNonQuery(SqlClear, ViewState("DBConnection"))
                    End If

                    If ViewState("Dt2Tahapan") IsNot Nothing Then
                        CType(ViewState("Dt2Tahapan"), DataTable).Rows.Clear()
                    End If

                    ''Insert Detail
                    SqlTahapan = "SELECT * FROM V_PRCIPTemplatedt WHERE TransNmbr = " + QuotedStr(Session("Result")(0).ToString)
                    DtTahapan = SQLExecuteQuery(SqlTahapan, ViewState("DBConnection")).Tables(0)

                    'For Each drResult In Session("Result").Rows
                    For Each drResult In DtTahapan.Rows
                        ExistRow = ViewState("DtTahapan").Select("ItemNo= " + QuotedStr(drResult("ItemNo")))
                        If ExistRow.Count = 0 Then
                            ''insert
                            Dim dr As DataRow
                            dr = ViewState("DtTahapan").NewRow
                            dr("ItemNo") = drResult("ItemNo")
                            dr("Tahapan") = drResult("Tahapan")
                            dr("TransNmbrTahapan") = drResult("TransNmbr")
                            dr("Percen") = drResult("Percen")
                            dr("TargetWaktu") = drResult("TargetWaktu")
                            dr("Biaya1") = drResult("Biaya1")
                            dr("Biaya2") = drResult("Biaya2")
                            dr("PIC") = drResult("PIC")
                            dr("PICName") = drResult("PICName")
                            dr("SPV1") = drResult("SPV1")
                            dr("SPVName1") = drResult("SPVName1")
                            dr("SPV2") = drResult("SPV2")
                            dr("SPVName2") = drResult("SPVName2")
                            dr("Direksi") = drResult("Direksi")
                            dr("DireksiName") = drResult("DireksiName")
                            dr("QcVerified") = drResult("QcVerified")
                            dr("QcVerifiedName") = drResult("QcVerifiedName")
                            dr("FgPermanent") = drResult("FgPermanent")
                            dr("Remark") = drResult("Remark")
                            ViewState("DtTahapan").Rows.Add(dr)
                        End If


                        'Insert Sub Detail
                        lbItem.Text = drResult("TransNmbr").ToString
                        lbTahapan.Text = drResult("Tahapan").ToString

                        'MultiView1.ActiveViewIndex = 1

                        Dim drDtResult As DataRow
                        Dim ExistRowDT As DataRow()
                        Dim MaxItem As String
                        Dim DtPekerjaan As DataTable
                        Dim SQLString As String

                        'SQLString = "EXEC S_GetBAPDetail " + QuotedStr(drResult("BAP_No").ToString)
                        SQLString = "SELECT * FROM V_PRCIPTemplatedt2 WHERE TransNmbr = " + QuotedStr(drResult("TransNmbr").ToString)  + " AND ItemNo = " + QuotedStr(drResult("ItemNo").ToString) 
                        DtPekerjaan = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)

                        For Each drDtResult In DtPekerjaan.Rows
                            ExistRowDT = ViewState("Dt2Tahapan").Select("ItemNo+'|'+ItemNoDt2 = " + QuotedStr(drDtResult("ItemNo").ToString + "|" + drDtResult("ItemNoDt2").ToString))
                            If ExistRowDT.Count = 0 Then
                                Dim Dtdr As DataRow
                                Dtdr = ViewState("Dt2Tahapan").NewRow
                                Dtdr("ItemNo") = drDtResult("ItemNo")
                                Dtdr("ItemNoDt2") = drDtResult("ItemNoDt2")
                                Dtdr("Tahapan") = drDtResult("Tahapan")
                                Dtdr("TransNmbrTahapan") = drDtResult("TransNmbr")
                                Dtdr("Percen") = drDtResult("Percen")
                                Dtdr("TargetWaktu") = drDtResult("TargetWaktu")
                                Dtdr("Biaya1") = drDtResult("Biaya1")
                                Dtdr("Biaya2") = drDtResult("Biaya2")
                                Dtdr("PIC") = drDtResult("PIC")
                                Dtdr("PICName") = drDtResult("PICName")
                                Dtdr("SPV1") = drDtResult("SPV1")
                                Dtdr("SPVName1") = drDtResult("SPVName1")
                                Dtdr("SPV2") = drDtResult("SPV2")
                                Dtdr("SPVName2") = drDtResult("SPVName2")
                                Dtdr("Direksi") = drDtResult("Direksi")
                                Dtdr("DireksiName") = drDtResult("DireksiName")
                                Dtdr("QcVerified") = drDtResult("QcVerified")
                                Dtdr("QcVerifiedName") = drDtResult("QcVerifiedName")
                                Dtdr("FgPermanent") = drDtResult("FgPermanent")
                                Dtdr("Remark") = drDtResult("Remark")
                                ViewState("Dt2Tahapan").Rows.Add(Dtdr)
                            End If
                        Next
                    Next

                    BindGridDt(ViewState("DtTahapan"), GridDtTahapan)
                    'EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
                    StatusButtonSave(True)
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

            If MultiView1.ActiveViewIndex = 2 Or MultiView1.ActiveViewIndex = 3 Then

                lbStatus.Text = ViewState("StateHD") 
                pnlDt3.Visible = True
                pnlEditDt3.Visible = False
                BindDataDt3(ViewState("TransNmbr"))
                If ViewState("StateHd")  = "Edit" OR ViewState("StateHd")  = "Insert" Then
                   GridDt3.Columns(0).Visible = True
                   btnGetReference.Visible = True
                   GridDtTahapan.Columns(0).Visible = True
                   btnGetTahapan.Visible = True
                   Else
                   GridDt3.Columns(0).Visible = False
                    btnGetReference.Visible = False
GridDtTahapan.Columns(0).Visible = False
                   btnGetTahapan.Visible = False
                End If

                'If lbStatus.Text = "False" Then
                '    GridDt3.Columns(0).Visible = False
                'End If
            End If

            If GetCountRecord(ViewState("DtTahapan")) = 0 Then
                GridDtTahapan.Columns(0).Visible = False
            Else
                GridDtTahapan.Columns(0).Visible = True
            End If

            If GetCountRecord(ViewState("Dt5")) = 0 Then
                GridDt5.Columns(0).Visible = False
            Else
                GridDt5.Columns(0).Visible = True
            End If

            If GetCountRecord(ViewState("Dt3")) = 0 Then
                GridDt3.Columns(0).Visible = False
            Else
                GridDt3.Columns(0).Visible = True
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


    Private Sub BindDataDtTahapan(ByVal Nmbr As String)
        Try
            Dim dt As New DataTable
            ViewState("DtTahapan") = Nothing
            dt = SQLExecuteQuery(GetStringDtTahapan(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("DtTahapan") = dt
            BindGridDt(dt, GridDtTahapan)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDt2Tahapan(ByVal Nmbr As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt2Tahapan") = Nothing
            dt = SQLExecuteQuery(GetStringDt2Tahapan(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt2Tahapan") = dt
            BindGridDt(dt, GridDt2Tahapan)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt2 Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDt5(ByVal Nmbr As String)
        Dim Drow As DataRow()
        Try
            Dim dt As New DataTable
            ViewState("Dt5") = Nothing
            dt = SQLExecuteQuery(GetStringDt5(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt5") = dt
            BindGridDt(dt, GridDt5)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt3 Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnCancelDtTahapan_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelDtTahapan.Click
        Try
            MovePanel(pnlEditDtTahapan, pnlDtTahapan)
            EnableHd(GetCountRecord(ViewState("DtTahapan")) = 0 And GetCountRecord(ViewState("Dt2Tahapan")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btncancelDt2Tahapan_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncancelDt2Tahapan.Click
        Try
            MovePanel(pnlEditDt2Tahapan, pnlDt2Tahapan)
            EnableHd(GetCountRecord(ViewState("DtTahapan")) = 0 And GetCountRecord(ViewState("Dt2Tahapan")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt2  Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancelDt5_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelDt5.Click
        Try
            MovePanel(pnlEditDt5, pnlDt5)
            EnableHd(GetCountRecord(ViewState("Dt5")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt2  Error : " + ex.ToString
        End Try
    End Sub


    Private Sub CleardtTahapan()
        Try
            tbTahapan.Text = ""
            tbPercen.Text = 0
            tbTargetWaktu.Text = 0
            tbBiaya1.Text = 0
            tbBiaya2.Text = 0
            tbPIC.Text = ""
            tbPICNameTahapan.Text = ""
            tbSpv1.Text = ""
            tbSpv1Name.Text = ""
            tbSpv2.Text = ""
            tbSpv2Name.Text = ""
            tbDireksi.Text = ""
            tbDireksiName.Text = ""
            tbQc.Text = ""
            tbQcName.Text = ""
            tbRemarkDtTahapan.Text = ""

        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub

    Private Sub Cleardt2Tahapan()
        Try
            tbTahapanDt2.Text = ""
            tbPercenDt2.Text = 0
            tbTargetWaktuDt2.Text = 0
            tbBiaya1Dt2.Text = 0
            tbBiaya2Dt2.Text = 0
            tbPICDt2.Text = ""
            tbPICNameDt2.Text = ""
            tbSpv1Dt2.Text = ""
            tbSpv1NameDt2.Text = ""
            tbSpv2Dt2.Text = ""
            tbSpv2NameDt2.Text = ""
            tbDireksiDt2.Text = ""
            tbDireksiNameDt2.Text = ""
            tbQcDt2.Text = ""
            tbQcNameDt2.Text = ""
            tbRemarkDt2Tahapan.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt 2 Error " + ex.ToString)
        End Try
    End Sub


    Function CekDtTahapan(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not Dr Is Nothing Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("Tahapan").ToString = "" Then
                    lbStatus.Text = MessageDlg("Dokumen Must Have Value")
                    tbTahapan.Focus()
                    Return False
                End If

            Else

                If tbTahapan.Text = "" Then
                    lbStatus.Text = MessageDlg("Tahapan must Have Value")
                    tbTahapan.Focus()
                    Return False
                End If


            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function

    Function CekDt2Tahapan(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not Dr Is Nothing Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("Tahapan").ToString = "" Then
                    tbTahapanDt2.Text = MessageDlg("Tahapan Must Have Value")
                    Return False
                End If


            Else
                If tbTahapanDt2.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Tahapan Must Have Value")
                    tbTahapanDt2.Focus()
                    Return False
                End If



            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function


    Protected Sub FillTextBoxDtTahapan(ByVal NoItem As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("DtTahapan").select("ItemNo = " + QuotedStr(NoItem))
            If Dr.Length > 0 Then
                lbItemNo.Text = Dr(0)("ItemNo").ToString
                BindToText(tbTahapan, Dr(0)("Tahapan").ToString)
                BindToText(tbPercen, Dr(0)("Percen").ToString)
                BindToText(tbTargetWaktu, Dr(0)("TargetWaktu").ToString)
                BindToText(tbBiaya1, Dr(0)("Biaya1").ToString)
                BindToText(tbBiaya2, Dr(0)("Biaya2").ToString)
                BindToText(tbPIC, Dr(0)("PIC").ToString)
                BindToText(tbPICNameTahapan, Dr(0)("PICName").ToString)
                BindToText(tbSpv1, Dr(0)("SPV1").ToString)
                BindToText(tbSpv1Name, Dr(0)("SPVName1").ToString)
                BindToText(tbSpv2, Dr(0)("SPV2").ToString)
                BindToText(tbSpv2Name, Dr(0)("SPVName2").ToString)
                BindToText(tbDireksi, Dr(0)("Direksi").ToString)
                BindToText(tbDireksiName, Dr(0)("DireksiName").ToString)
                BindToText(tbQc, Dr(0)("QcVerified").ToString)
                BindToText(tbQcName, Dr(0)("QcVerifiedName").ToString)
                BindToDropList(ddlExtDateUpdate, Dr(0)("FgPermanent").ToString)
                BindToText(tbRemarkDtTahapan, Dr(0)("Remark").ToString)


                tbPercen.Enabled = False
                tbTargetWaktu.Enabled = False
                tbBiaya1.Enabled = False
                tbBiaya2.Enabled = False
                tbPICNameTahapan.Enabled = False
                tbSpv1.Enabled = False
                tbSpv1Name.Enabled = False
                tbSpv2.Enabled = False
                tbSpv2Name.Enabled = False
                tbDireksi.Enabled = False
                tbDireksiName.Enabled = False
                tbQc.Enabled = False
                tbQcName.Enabled = False
                tbRemarkDtTahapan.Enabled = False
                btnspv1.Visible = False
                btnSpv2.Visible = False
                btnDireksi.Visible = False
                btnQc.Visible = False
                ddlExtDateUpdate.Enabled = False

            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub


    Protected Sub FillTextBoxDt2Tahapan(ByVal Dok As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt2Tahapan").select("ItemNo+'|'+ItemNoDt2 = " + QuotedStr(Dok))
            If Dr.Length > 0 Then
                lbItemNoDt2.Text = Dr(0)("ItemNoDt2").ToString
                BindToText(tbTahapanDt2, Dr(0)("Tahapan").ToString)
                BindToText(tbPercenDt2, Dr(0)("Percen").ToString)
                BindToText(tbTargetWaktuDt2, Dr(0)("TargetWaktu").ToString)
                BindToText(tbBiaya1Dt2, Dr(0)("Biaya1").ToString)
                BindToText(tbBiaya2Dt2, Dr(0)("Biaya2").ToString)
                BindToText(tbPICDt2, Dr(0)("PIC").ToString)
                BindToText(tbPICNameDt2, Dr(0)("PICName").ToString)
                BindToText(tbSpv1Dt2, Dr(0)("SPV1").ToString)
                BindToText(tbSpv1NameDt2, Dr(0)("SPVName1").ToString)
                BindToText(tbSpv2Dt2, Dr(0)("SPV2").ToString)
                BindToText(tbSpv2NameDt2, Dr(0)("SPVName2").ToString)
                BindToText(tbDireksiDt2, Dr(0)("Direksi").ToString)
                BindToText(tbDireksiNameDt2, Dr(0)("DireksiName").ToString)
                BindToText(tbQcDt2, Dr(0)("QcVerified").ToString)
                BindToText(tbQcNameDt2, Dr(0)("QcVerifiedName").ToString)
                BindToDropList(ddlExtDateUpdateDt2, Dr(0)("FgPermanent").ToString)
                BindToText(tbRemarkDt2Tahapan, Dr(0)("Remark").ToString)



                tbPercenDt2.Enabled = False
                tbTargetWaktuDt2.Enabled = False
                tbBiaya1Dt2.Enabled = False
                tbBiaya2Dt2.Enabled = False
                tbPICNameDt2.Enabled = False
                tbSpv1Dt2.Enabled = False
                tbSpv1NameDt2.Enabled = False
                tbSpv2Dt2.Enabled = False
                tbSpv2NameDt2.Enabled = False
                tbDireksiDt2.Enabled = False
                tbDireksiNameDt2.Enabled = False
                tbQcDt2.Enabled = False
                tbQcNameDt2.Enabled = False
                tbRemarkDt2Tahapan.Enabled = False
                btnspv1Dt2.Visible = False
                btnSpv2Dt2.Visible = False
                btnDireksiDt2.Visible = False
                btnQcDt2.Visible = False
                ddlExtDateUpdateDt2.Enabled = False
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail 2 error : " + ex.ToString)
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
    Protected Sub btnsavedtTahapan_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsavedtTahapan.Click
        Dim tbTotal As Double
        Try

            

            If ViewState("StateDt") = "Edit" Then
                Dim Row As DataRow
                Row = ViewState("DtTahapan").Select("ItemNo = " + QuotedStr(ViewState("DtValue")))(0)
                If CekDtTahapan() = False Then
                btnSaveDt.Focus()
                Exit Sub
                End If
                Row.BeginEdit()
                'Row("ItemNo") = lbItemNo.Text
                Row("Tahapan") = tbTahapan.Text
                Row("Percen") = tbPercen.Text
                Row("TargetWaktu") = tbTargetWaktu.Text
                Row("Biaya1") = tbBiaya1.Text
                Row("Biaya2") = tbBiaya2.Text
                Row("PIC") = tbPIC.Text
                Row("PICName") = tbPICNameTahapan.Text
                Row("SPV1") = tbSpv1.Text
                Row("SPVName1") = tbSpv1Name.Text
                Row("SPV2") = tbSpv2.Text
                Row("SPVName2") = tbSpv2Name.Text
                Row("Direksi") = tbDireksi.Text
                Row("DireksiName") = tbDireksiName.Text
                Row("QcVerified") = tbQc.Text
                Row("QcVerifiedName") = tbQcName.Text
                Row("FgPermanent") = ddlExtDateUpdate.SelectedValue
                Row("Remark") = tbRemarkDtTahapan.Text

                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                dr = ViewState("DtTahapan").NewRow
                dr("ItemNo") = lbItemNo.Text
                dr("Tahapan") = tbTahapan.Text
                dr("Percen") = tbPercen.Text
                dr("TargetWaktu") = tbTargetWaktu.Text
                dr("Biaya1") = tbBiaya1.Text
                dr("Biaya2") = tbBiaya2.Text
                dr("PIC") = tbPIC.Text
                dr("PICName") = tbPICNameTahapan.Text
                dr("SPV1") = tbSpv1.Text
                dr("SPVName1") = tbSpv1Name.Text
                dr("SPV2") = tbSpv2.Text
                dr("SPVName2") = tbSpv2Name.Text
                dr("Direksi") = tbDireksi.Text
                dr("DireksiName") = tbDireksiName.Text
                dr("QcVerified") = tbQc.Text
                dr("QcVerifiedName") = tbQcName.Text
                dr("FgPermanent") = ddlExtDateUpdate.SelectedValue
                dr("Remark") = tbRemarkDtTahapan.Text
                ViewState("DtTahapan").Rows.Add(dr)
            End If

            MovePanel(pnlEditDtTahapan, pnlDtTahapan)
            'EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
            BindGridDt(ViewState("DtTahapan"), GridDtTahapan)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save Dt Tahapan Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub

    Protected Sub btnsaveDt2Tahapan_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsaveDt2Tahapan.Click
        Try
            If CekDt2Tahapan() = False Then
                btnSaveDt2.Focus()
                Exit Sub
            End If
            If ViewState("StateDt2") = "Edit" Then
                Dim Row As DataRow
                Row = ViewState("Dt2Tahapan").Select("ItemNo+'|'+ItemNoDt2 = " + QuotedStr(lbItem.Text + "|" + lbItemNoDt2.Text))(0)
                Row.BeginEdit()
                Row("ItemNo") = lbItem.Text
                Row("ItemNoDt2") = lbItemNoDt2.Text
                Row("Tahapan") = tbTahapanDt2.Text
                Row("Percen") = tbPercenDt2.Text
                Row("TargetWaktu") = tbTargetWaktuDt2.Text
                Row("Biaya1") = tbBiaya1Dt2.Text
                Row("Biaya2") = tbBiaya2Dt2.Text
                Row("PIC") = tbPICDt2.Text
                Row("PICName") = tbPICNameDt2.Text
                Row("SPV1") = tbSpv1Dt2.Text
                Row("SPVName1") = tbSpv1NameDt2.Text
                Row("SPV2") = tbSpv2Dt2.Text
                Row("SPVName2") = tbSpv2NameDt2.Text
                Row("Direksi") = tbDireksiDt2.Text
                Row("DireksiName") = tbDireksiNameDt2.Text
                Row("QcVerified") = tbQcDt2.Text
                Row("QcVerifiedName") = tbQcNameDt2.Text
                Row("FgPermanent") = ddlExtDateUpdateDt2.SelectedValue
                Row("Remark") = tbRemarkDt2Tahapan.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt2Tahapan").NewRow
                dr("ItemNo") = lbItem.Text
                dr("ItemNoDt2") = lbItemNoDt2.Text
                dr("Tahapan") = tbTahapanDt2.Text
                dr("Percen") = tbPercenDt2.Text
                dr("TargetWaktu") = tbTargetWaktuDt2.Text
                dr("Biaya1") = tbBiaya1Dt2.Text
                dr("Biaya2") = tbBiaya2Dt2.Text
                dr("PIC") = tbPICDt2.Text
                dr("PICName") = tbPICNameDt2.Text
                dr("SPV1") = tbSpv1Dt2.Text
                dr("SPVName1") = tbSpv1NameDt2.Text
                dr("SPV2") = tbSpv2Dt2.Text
                dr("SPVName2") = tbSpv2NameDt2.Text
                dr("Direksi") = tbDireksiDt2.Text
                dr("DireksiName") = tbDireksiNameDt2.Text
                dr("QcVerified") = tbQcDt2.Text
                dr("QcVerifiedName") = tbQcNameDt2.Text
                dr("FgPermanent") = ddlExtDateUpdateDt2.SelectedValue
                dr("Remark") = tbRemarkDt2Tahapan.Text
                ViewState("Dt2Tahapan").Rows.Add(dr)


                ' Rebind grid setelah perubahan
                BindGridDt(ViewState("DtTahapan"), GridDtTahapan)
                BindGridDt(ViewState("Dt2Tahapan"), GridDt2Tahapan)
            End If
            MovePanel(pnlEditDt2Tahapan, pnlDt2Tahapan)
            Dim drow As DataRow()
            drow = ViewState("Dt2Tahapan").Select("ItemNo = " + QuotedStr(lbItem.Text))
            If drow.Length > 0 Then
                BindGridDt(drow.CopyToDataTable, GridDt2Tahapan)
                GridDt2Tahapan.Columns(0).Visible = Not ViewState("StateHd") = "View"
            Else
                Dim DtTemp As New DataTable
                DtTemp = ViewState("Dt2Tahapan").Clone
                DtTemp.Rows.Add(DtTemp.NewRow()) ' create a new blank row to the DataTable
                GridDt2Tahapan.DataSource = DtTemp
                GridDt2Tahapan.DataBind()
                GridDt2Tahapan.Columns(0).Visible = False
            End If

            'CountTotalDt()
            btncancelDt2Tahapan.Visible = True
            btnsaveDt2Tahapan.Visible = True

            'EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
            'BindGridDt(ViewState("Dt2"), GridDt2)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save dt2tahapan Error : " + ex.ToString
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
                "BrokerPhone,RelatedOffcName,RelatedOffcPhone,Remark,UserPrep,DatePrep,FgReport,FgActive, TypeCIP) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", 'H', " + _
                QuotedStr(tbApplfileNo.Text) + ", " + QuotedStr(Format(tbApplfileDate.SelectedValue, "yyyy-MM-dd")) + ", " + _
                QuotedStr(tbAlasHak.Text) + ", " + QuotedStr(tbKegiatanCode.Text) + ", " + QuotedStr(tbAreaCode.Text) + ", " + _
                QuotedStr(tbPICName.Text) + ", " + QuotedStr(tbBrokerName.Text) + ", " + QuotedStr(tbBrokerPhone.Text) + ", " + _
                QuotedStr(tbRelatedOffcName.Text) + ", " + QuotedStr(tbRelatedOffcPhone.Text) + ", " + _
                QuotedStr(tbRemark.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", GetDate()," + QuotedStr("0") + ", " + QuotedStr("Y") + ", " + _
                QuotedStr(ddlTypeCIP.SelectedValue)
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
                ", TypeCIP = " + QuotedStr(ddlTypeCIP.SelectedValue) + _
                " WHERE TransNmbr = " + QuotedStr(tbCode.Text) 'ddlUserType.SelectedValue, tbAttn.Text, tbTotalSelisih.Text.Replace(",", "")
                ' fupSignRecv.SaveAs(path)
            End If
            SQLString = ChangeQuoteNull(SQLString)
            SQLString = SQLString.Replace("'1900-01-01'", "NULL")
            SQLString = SQLString.Replace("'0001-01-01'", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            ' === CLEAR Tahapan lama di DATABASE agar tidak bentrok saat save Edit data ===
            Dim SqlClear As String = "EXEC S_ClearTahapan " + QuotedStr(tbCode.Text)
            SQLExecuteNonQuery(SqlClear, ViewState("DBConnection"))

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

            Row = ViewState("DtTahapan").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbCode.Text
                Row(I).EndEdit()
            Next

            Row = ViewState("Dt2Tahapan").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbCode.Text
                Row(I).EndEdit()
            Next

            Row = ViewState("Dt5").Select("TransNmbr IS NULL")
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


            'save dtTahapan
            Dim cmdSqlTahapan As New SqlCommand("SELECT TransNmbr,ItemNo,Tahapan,TransNmbrTahapan,Percen,TargetWaktu,Biaya1,Biaya2,PIC,Spv1,Spv2,Direksi,QcVerified,FgPermanent,Remark FROM PRCIPRecvDocDt3 WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSqlTahapan)
            Dim dbcommandBuilderTahapan As SqlCommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilderTahapan.GetInsertCommand
            da.DeleteCommand = dbcommandBuilderTahapan.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilderTahapan.GetUpdateCommand

            Dim DtTahapan As New DataTable("PRCIPRecvDocDt3")

            DtTahapan = ViewState("DtTahapan")
            da.Update(DtTahapan)
            DtTahapan.AcceptChanges()
            ViewState("DtTahapan") = DtTahapan



            'save dt2Tahapan
            cmdSql = New SqlCommand("SELECT TransNmbr,ItemNo,ItemNoDt2,Tahapan,TransNmbrTahapan,Percen,TargetWaktu,Biaya1,Biaya2,PIC,Spv1,Spv2,Direksi,QcVerified,FgPermanent,Remark FROM PRCIPRecvDocDt4 WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            dbcommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt2Tahapan As New DataTable("PRCIPRecvDocDt4")

            Dt2Tahapan = ViewState("Dt2Tahapan")
            da.Update(Dt2Tahapan)
            Dt2Tahapan.AcceptChanges()
            ViewState("DtTahapan") = Dt2Tahapan


            'save dt5
            cmdSql = New SqlCommand("SELECT TransNmbr, ItemNo, Reference, NoDokumen FROM PRCIPRecvDocDt5 WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            dbcommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt5 As New DataTable("PRCIPRecvDocDt5")

            Dt5 = ViewState("Dt5")
            da.Update(Dt5)
            Dt5.AcceptChanges()
            ViewState("Dt5") = Dt5

            

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
            ModifyInput2(True, pnlInput, pnlDtTahapan, GridDtTahapan)
            ModifyInput2(True, pnlInput, pnlDt2Tahapan, GridDt2Tahapan)
            ModifyInput2(True, pnlInput, pnlDt5, GridDt5)
            newTrans()
            MultiView2.ActiveViewIndex = 0
            Menu2.Items.Item(0).Selected = True

            MultiView1.ActiveViewIndex = 0
            Menu1.Items.Item(0).Selected = True

            btnHome.Visible = False
            fupSignRecv.Visible = True
            btnAdddtTahapan.Visible = False
            btnAddDtke2Tahapan.Visible = False
            btnAddDt5.Visible = False
            btnAddDt5ke2.Visible = False

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


    Protected Sub btnAddDtTahapan_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdddtTahapan.Click, btnAddDtke2Tahapan.Click
        Try
            CleardtTahapan()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt") = "Insert"
            MovePanel(pnlDtTahapan, pnlEditDtTahapan)
            'EnableHd(False)
            StatusButtonSave(False)
            lbItemNo.Text = GetNewItemNo(ViewState("DtTahapan"))
        Catch ex As Exception
            lbStatus.Text = "btn add dt error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddDt2ahapan_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnaddDt2Tahapan.Click, btnaddDt2Tahapanke2.Click
        Try
            Cleardt2Tahapan()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt2") = "Insert"
            MovePanel(pnlDt2Tahapan, pnlEditDt2Tahapan)
            EnableHd(False)
            StatusButtonSave(False)
            ' === Tambahkan logika nomor urut otomatis di sini ===
            Dim dt As DataTable = CType(ViewState("Dt2Tahapan"), DataTable)
            Dim nextNumber As Integer = 1

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                nextNumber = dt.Rows.Count + 1
            End If

            lbItemNoDt2.Text = nextNumber.ToString()
            ' ====================================================

            'lbItemNoDt2.Text = GetNewItemNo(ViewState("Dt2"))
        Catch ex As Exception
            lbStatus.Text = "btn add dt error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ViewState("DigitCurr") = 0
            ViewState("TransNmbr") = ""
            ClearHd()
            Cleardt()
            CleardtTahapan()
            Cleardt2Tahapan()
            PnlDt.Visible = True
            btnDeleteDoc.Visible = False
            BindDataDt("")
            BindDataDt3("")
            BindDataDtTahapan("")
            BindDataDt2Tahapan("")
            BindDataDt5("")
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
                    BindDataDtTahapan(ViewState("TransNmbr"))
                    BindDataDt2Tahapan(ViewState("TransNmbr"))
                    BindDataDt5(ViewState("TransNmbr"))
                    ViewState("StateHd")   = "View"
                    ModifyInput2(False, pnlInput, PnlDt, GridDt)
                    ModifyInput2(False, pnlInput, pnlDt3, GridDt3)

                    ModifyInput2(False, pnlInput, pnlDtTahapan, GridDtTahapan)
                    ModifyInput2(False, pnlInput, pnlDt2Tahapan, GridDt2Tahapan)
                    ModifyInput2(False, pnlInput, pnlDt5, GridDt5)

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
                        BindDataDtTahapan(ViewState("TransNmbr"))
                        BindDataDt2Tahapan(ViewState("TransNmbr"))
                        BindDataDt5(ViewState("TransNmbr"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, PnlDt, GridDt)
                        ModifyInput2(True, pnlInput, pnlDt3, GridDt3)

                        ModifyInput2(True, pnlInput, pnlDtTahapan, GridDtTahapan)
                        ModifyInput2(True, pnlInput, pnlDt2Tahapan, GridDt2Tahapan)
                        ModifyInput2(True, pnlInput, pnlDt5, GridDt5)

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
                        btnAdddtTahapan.Visible = False
                        btnAddDtke2Tahapan.Visible = False
                        btnAddDt5.Visible = False
                        btnAddDt5ke2.Visible = False
                        btnHome.Visible = False



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
                        BindDataDtTahapan(ViewState("TransNmbr"))
                        BindDataDt2Tahapan(ViewState("TransNmbr"))
                        BindDataDt5(ViewState("TransNmbr"))
                        FillTextBoxHd(ViewState("TransNmbr"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(False, pnlInput, PnlDt, GridDt)
                        ModifyInput2(False, pnlInput, pnlDt3, GridDt3)
                        ModifyInput2(False, pnlInput, pnlDtTahapan, GridDtTahapan)
                        ModifyInput2(False, pnlInput, pnlDt2Tahapan, GridDt2Tahapan)
                        ModifyInput2(False, pnlInput, pnlDt5, GridDt5)
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
                    lbStatus.Text = MessageDlg("Status Terima Dokumen is not Post, cannot close Dokumen")
                    Exit Sub
                End If
                If GVR.Cells(12).Text = "Y" Then
                    lbStatus.Text = MessageDlg("Status Terima Dokumen Closed Already")
                    Exit Sub
                End If
                ViewState("ProductClose") = GVR.Cells(2).Text
                AttachScript("closing();", Page, Me.GetType)
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Command Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub GridDtTahapan_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridDtTahapan.RowCommand
        Try
            If e.CommandName = "Insert" Then
                btnAddDtTahapan_Click(Nothing, Nothing)
            End If

            If e.CommandName = "View" Then
                Dim GVR As GridViewRow
                GVR = GridDtTahapan.Rows(Convert.ToInt32(e.CommandArgument))

                If GVR.Cells(2).Text = "&nbsp;" Then
                    Exit Sub
                End If
                Dim lbFA As Label

                lbFA = GVR.FindControl("lbFa")

                lbItem.Text = GVR.Cells(2).Text
                lbTahapan.Text = GVR.Cells(3).Text

                MultiView1.ActiveViewIndex = 4


                If ViewState("StateHd") = "View" Then
                    ModifyInput2(False, pnlInput, pnlDt2Tahapan, GridDt2Tahapan)
                Else
                    ModifyInput2(True, pnlInput, pnlDt2Tahapan, GridDt2Tahapan)
                End If

                GridDt2Tahapan.Columns(0).Visible = Not ViewState("StateHd") = "View"
                If ViewState("Dt2Tahapan") Is Nothing Then
                    BindDataDt2Tahapan(ViewState("TransNmbr"))
                End If

                Dim drow As DataRow()
                drow = ViewState("Dt2Tahapan").Select("ItemNo = " + QuotedStr(TrimStr(lbItem.Text)))
                If drow.Length > 0 Then
                    BindGridDt(drow.CopyToDataTable, GridDt2Tahapan)
                    GridDt2Tahapan.Columns(0).Visible = Not ViewState("StateHd") = "View"
                Else
                    Dim DtTemp As DataTable
                    DtTemp = ViewState("Dt2Tahapan").Clone
                    DtTemp.Rows.Add(DtTemp.NewRow())
                    GridDt2Tahapan.DataSource = DtTemp
                    GridDt2Tahapan.DataBind()
                    GridDt2Tahapan.Columns(0).Visible = False
                End If


            End If
            btnBackDt2Tahapanke2.Visible = False
            btnaddDt2Tahapanke2.Visible = False
            btnaddDt2Tahapan.Visible = False

        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
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

    Protected Sub GridDtTahapan_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDtTahapan.RowDeleting
        Try
            Dim dr(), drCek() As DataRow
            Dim GVR As GridViewRow = GridDtTahapan.Rows(e.RowIndex)

            ' Cek apakah GridDt2 punya anak dari item ini
            drCek = ViewState("Dt2Tahapan").Select("ItemNo = " + QuotedStr(GVR.Cells(2).Text))

            If drCek.Length > 0 Then
                '=== Delete semua Detail 2 yang terkait ===
                dr = ViewState("Dt2Tahapan").Select("ItemNo = " + QuotedStr(GVR.Cells(2).Text))
                For Each d As DataRow In dr
                    d.Delete()
                Next
                ViewState("Dt2Tahapan").AcceptChanges()
                BindGridDt(ViewState("Dt2Tahapan"), GridDt2Tahapan)
            End If

            '=== Delete Detail 1 ===
            dr = ViewState("DtTahapan").Select("ItemNo = " + QuotedStr(GVR.Cells(2).Text))
            For Each d As DataRow In dr
                d.Delete()
            Next
            ViewState("DtTahapan").AcceptChanges()
            BindGridDt(ViewState("DtTahapan"), GridDtTahapan)

            '=== Update Enable Header ===
            'EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)

        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        End Try
    End Sub




    Protected Sub GridDt2_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt2.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow
            GVR = GridDt2Tahapan.Rows(e.RowIndex)
            dr = ViewState("Dt2Tahapan").Select("ItemNo+'|'+ItemNoDt2 = " + QuotedStr(lbItem.Text + "|" + GVR.Cells(1).Text))
            dr(0).Delete()

            Dim drow As DataRow()
            drow = ViewState("Dt2Tahapan").Select("ItemNo = " + QuotedStr(TrimStr(lbItem.Text)))
            If drow.Length > 0 Then
                BindGridDt(drow.CopyToDataTable, GridDt2Tahapan)
                GridDt2Tahapan.Columns(0).Visible = Not ViewState("StateHd") = "View"
            Else
                Dim DtTemp As New DataTable
                DtTemp = ViewState("Dt2Tahapan").Clone
                DtTemp.Rows.Add(DtTemp.NewRow()) ' create a new blank row to the DataTable
                GridDt2Tahapan.DataSource = DtTemp
                GridDt2Tahapan.DataBind()
                GridDt2Tahapan.Columns(0).Visible = False
            End If

        Catch ex As Exception
            lbStatus.Text = "Grid Dt 2 Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt5_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt5.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow
            GVR = GridDt5.Rows(e.RowIndex)
            dr = ViewState("Dt5").Select("Reference = " + QuotedStr(GVR.Cells(2).Text))
            dr(0).Delete()
            BindGridDt(ViewState("Dt5"), GridDt5)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt 5 Row Deleting Error : " + ex.ToString
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

    Protected Sub GridDtTahapan_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDtTahapan.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDtTahapan.Rows(e.NewEditIndex)
            ViewState("DtValue") = GVR.Cells(2).Text
            FillTextBoxDtTahapan(GVR.Cells(2).Text)

            MovePanel(pnlDtTahapan, pnlEditDtTahapan)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            btnsavedtTahapan.Focus()
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt2Tahapan_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt2Tahapan.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt2Tahapan.Rows(e.NewEditIndex)
            FillTextBoxDt2Tahapan(lbItem.Text + "|" + GVR.Cells(1).Text)
            MovePanel(pnlDt2Tahapan, pnlEditDt2Tahapan)
            EnableHd(False)
            ViewState("StateDt2") = "Edit"
            StatusButtonSave(False)
            btnsaveDt2Tahapan.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid dt2 Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnBackDt2Tahapan_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBackDt2Tahapan.Click, btnBackDt2Tahapanke2.Click
        Try
            MultiView1.ActiveViewIndex = 3 
        Catch ex As Exception
            lbStatus.Text = "btn back Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnPIC_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPIC.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT User_Id,User_Name  FROM VSAUsers"
            ResultField = "User_Id,User_Name"
            ViewState("Sender") = "btnPIC"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchGrid();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Supp Error : " + ex.ToString
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

    Protected Sub btnGetRefDoc_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGetRefDoc.Click
        Dim ResultField, CriteriaField As String
        Try
            Session("filter") = "SELECT No_Dokumen_Recieve, No_Dokumen, Type_Ijin FROM S_GetDokumenReceive"               
            ResultField = "No_Dokumen_Recieve, No_Dokumen, Type_Ijin"
            CriteriaField = "No_Dokumen_Recieve, No_Dokumen, Type_Ijin"
            ViewState("CriteriaField") = CriteriaField.Split(",")
            Session("Column") = ResultField.Split(",")
            ViewState("Sender") = "GetDokReceive"
            Session("DBConnection") = ViewState("DBConnection")
            'AttachScript("SearchMultiDlg();", Page, Me.GetType())
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "BtnRecvDoc Click Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnGetTahapan_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetTahapan.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT TransNmbr, Remark As NamaTahapan FROM V_PRCIPTemplateHd"
            ResultField = "TransNmbr, NamaTahapan"
            ViewState("Sender") = "GetTahapan"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchGrid();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Supp Error : " + ex.ToString
        End Try
    End Sub
End Class
