Imports System.Data
Imports System.Data.SqlClient
'Imports System.Data.Odbc
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc

Partial Class Penawaran
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select * From V_MKTSPHD "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                'FillCombo(ddlLokasi, "SELECT AreaCode, AreaName FROM V_MsArea ", True, "AreaCode", "AreaName", ViewState("DBConnection"))
                SetInit()
                Session("AdvanceFilter") = ""
                'lbCount.Text = SQLExecuteScalar("SELECT COUNT(No_Spk) FROM V_GetPemenang ", ViewState("DBConnection").ToString)
            End If
            lbStatus.Text = ""
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If

            ' hasil dari search dialog
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnArea" Then
                    tbArea.Text = Session("Result")(0).ToString
                    tbAreaName.Text = Session("Result")(1).ToString
                End If


                If ViewState("Sender") = "btnUnit" Then
                    TbUnit.Text = Session("Result")(0).ToString
                    tbUnitName.Text = Session("Result")(1).ToString
                End If


                Session("Result") = Nothing
                ViewState("Sender") = Nothing
                'Session("filter") = Nothing
                'Session("Column") = Nothing
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
        ViewState("PPN") = SQLExecuteScalar("Select Max(PPN) FROM MsPPN ", ViewState("DBConnection"))
    End Sub

    Private Sub SetInit()
        FillRange(ddlRange)
        ViewState("SortExpression") = Nothing
        ViewState("DigitCurr") = 2
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
        If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
            ddlCommand.Items.Add("Print")
            ddlCommand2.Items.Add("Print")
        End If


        'tbPrice.Attributes.Add("OnBlur", "setformatfordt();")
        'tbDisc.Attributes.Add("OnBlur", "setformatfordt();")
        'tbDiscValue.Attributes.Add("OnBlur", "setformatfordt();")
        'tbTandaJadi.Attributes.Add("OnBlur", "setformatfordt();")
        'tbDpp.Attributes.Add("OnBlur", "setformatfordt();")
        'tbDP.Attributes.Add("OnBlur", "setformatfordt();")
        'tbDPValue.Attributes.Add("OnBlur", "setformatfordt();")
        'tbTotalAmount.Attributes.Add("OnBlur", "setformatfordt();")


        tbTotHarga.Attributes.Add("OnBlur", "setformatfordt();")
        tbDiscHd.Attributes.Add("OnBlur", "setformatfordt();")
        tbTotDisc.Attributes.Add("OnBlur", "setformatfordt();")
        tbTotDPP.Attributes.Add("OnBlur", "setformatfordt();")
        tbDpHd.Attributes.Add("OnBlur", "setformatfordt();")
        tbTotDP.Attributes.Add("OnBlur", "setformatfordt();")
        tbTotTJ.Attributes.Add("OnBlur", "setformatfordt();")
        tbTotAmount.Attributes.Add("OnBlur", "setformatfordt();")


        tbPrice.Attributes.Add("OnBlur", "setformatdtprice();")
        tbLuasTanah.Attributes.Add("OnBlur", "setformatdtprice();")
        tbPricePerm2.Attributes.Add("OnBlur", "setformatdtprice();")


        'Me.tbTotalAmount.Attributes.Add("ReadOnly", "True")
        Me.tbTotAmount.Attributes.Add("ReadOnly", "True")
        'Me.tbTotDisc.Attributes.Add("ReadOnly", "True")
        Me.tbTotDPP.Attributes.Add("ReadOnly", "True")
        'Me.tbTotTJ.Attributes.Add("ReadOnly", "True")
        Me.tbTotDP.Attributes.Add("ReadOnly", "True")
        ' Me.tbDPValue.Attributes.Add("ReadOnly", "True")
        ' Me.tbDpp.Attributes.Add("ReadOnly", "True")
        'Me.tbDiscValue.Attributes.Add("ReadOnly", "True")
        Me.tbTotHarga.Attributes.Add("ReadOnly", "True")



        Me.tbPrice.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbTotTJ.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbDiscHd.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbDpHd.Attributes.Add("OnKeyDown", "return PressNumeric();")
        'Me.tbDisc.Attributes.Add("OnKeyDown", "return PressNumeric();")
        'Me.tbDiscValue.Attributes.Add("OnKeyDown", "return PressNumeric();")
        'Me.tbDpp.Attributes.Add("OnKeyDown", "return PressNumeric();")
        'Me.tbDP.Attributes.Add("OnKeyDown", "return PressNumeric();")

        Me.tbLuasTanah.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbLuasBangunan.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbKtinggianDPL.Attributes.Add("OnKeyDown", "return PressNumeric();")


    End Sub

    Protected Sub Menu2_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu2.MenuItemClick
        MultiView2.ActiveViewIndex = Int32.Parse(e.Item.Value)
        If Menu2.Items.Item(0).Selected = True Then
            If ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit" Then
                GridDt.Columns(0).Visible = True
            End If
        End If
        If Menu2.Items.Item(1).Selected = True Then
            If ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit" Then
                GridDt.Columns(0).Visible = False
            End If

        End If
    End Sub


    Protected Sub btnsaveINV_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsaveINV.Click
        Try

            Dim dr As DataTable
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbRef.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("save this transaction first, to upload the dokumen")
                Exit Sub
            End If

            If FubInv.FileBytes.Length > 3500000 Then
                lbStatus.Text = MessageDlg("Ukuran File Terlalu Besar. !! Max Upload 3.5Mb")
                Exit Sub
            End If

            If Right(FubInv.FileName, 4) <> ".pdf" Then
                lbStatus.Text = MessageDlg("Upload Pdf File Only !")
                Exit Sub
            End If
            Dim path2, namafile2, SQLString1 As String
            Dim dt As DataTable
            path2 = Server.MapPath("~/Image/TrPenawaranPDF/") + tbRef.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubInv.FileName
            namafile2 = tbRef.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubInv.FileName

            SQLString1 = "UPDATE MKTSPHD SET FileUpLayout = " + QuotedStr(namafile2) + _
            " WHERE TransNmbr = " + QuotedStr(tbRef.Text)
            FubInv.SaveAs(path2)
            SQLExecuteNonQuery(SQLString1, ViewState("DBConnection").ToString)

            dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbRef.Text), ViewState("DBConnection").ToString)
            lbDokInv.Text = dt.Rows(0)("FileUpLayout").ToString
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
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbRef.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            If dr.Rows(0)("FileUpLayout").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            filePath = dr.Rows(0)("FileUpLayout").ToString
            URL = ResolveUrl("~/Image/TrPenawaranPDF/" + filePath)
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
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbRef.Text), ViewState("DBConnection").ToString)
            filePath = dr.Rows(0)("FileUpLayout").ToString


            If File.Exists(Server.MapPath("~/Image/TrPenawaranPDF/" + filePath)) = True Then
                File.Delete(Server.MapPath("~/Image/TrPenawaranPDF/" + filePath))
                SQLExecuteNonQuery("UPDATE MKTSPHD Set FileUpLayout = '' WHERE TransNmbr = '" + tbRef.Text + "' ", ViewState("DBConnection").ToString)

                lbDokInv.Text = "Not yet uploaded"
                FubInv.Visible = True
                btnClearInv.Visible = False
            End If



        Catch ex As Exception
            lbStatus.Text = "lbBAP_Click Error : " + ex.ToString
        End Try
    End Sub

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
            If AdvanceFilter.Length > 1 Then
                StrFilter = AdvanceFilter
            End If
            DT = BindDataTransaction(GetStringHd, StrFilter, ViewState("DBConnection").ToString)
            If DT.Rows.Count = 0 Then
                lbStatus.Text = "No Data"
                pnlNav.Visible = False
                'ddlCommand.Visible = False
                'BtnGo.Visible = False
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

    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT * From V_MKTSPDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            GridView1.PageIndex = 0
            Session("AdvanceFilter") = ""
            BindData(Session("AdvanceFilter"))
            pnlNav.Visible = True
            'ddlCommand.Visible = True
            'BtnGo.Visible = True
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
                        ListSelectNmbr = GVR.Cells(2).Text
                        If Pertamax Then
                            Result = "'''" + ListSelectNmbr + "''"
                            Pertamax = False
                        Else
                            Result = Result + ",''" + ListSelectNmbr + "''"
                        End If
                    End If
                Next

                If Result.Trim = "" Then
                    lbStatus.Text = MessageDlg("No Data Selected")
                    Exit Sub
                End If

                Result = Result + "'"
                Session("SelectCommand") = "EXEC S_FNFormPenawaran " + Result + "," + QuotedStr(ViewState("UserId"))
                Session("ReportFile") = ".../../../Rpt/FormPenawaran.frx"
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
                        Result = ExecSPCommandGo(ActionValue, "S_MKTSP", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                        If Trim(Result) <> "" Then
                            lbStatus.Text = lbStatus.Text + Result + " <br/>"
                        End If
                    End If
                Next
                BindData("TransNmbr in (" + ListSelectNmbr + ")")
            End If
        Catch ex As Exception
            lbStatus.Text = "Go Command Error : " + ex.ToString
        End Try
    End Sub

    Private Sub EnableHd(ByVal State As Boolean)
        Try
            tbDate.Enabled = State
            btnArea.Enabled = State
            btnArea.Visible = State
            tbArea.Enabled = False

            'tbRemark.Enabled = State

        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub



    Private Sub EnableDt(ByVal State As Boolean)
        Try
            'tbUraian.Enabled = State

        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDt(ByVal Referens As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt") = Nothing
            dt = SQLExecuteQuery(GetStringDt(Referens), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt") = dt
            BindGridDt(dt, GridDt)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnHome.Click
        Try
            MovePanel(pnlInput, PnlHd)
        Catch ex As Exception
            lbStatus.Text = "btn back Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancelDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelDt.Click
        Try
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            'EnableHd(GridDt.Rows.Count = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt.Click
        Dim Row As DataRow
        Try
            If ViewState("StateDt") = "Edit" Then
                If ViewState("DtValue") <> TbUnit.Text Then
                    If CekExistData(ViewState("Dt"), "UnitCode", TbUnit.Text) Then
                        lbStatus.Text = "Unit No " + TbUnit.Text + " has been already exist"
                        Exit Sub
                    End If
                End If

                Row = ViewState("Dt").Select("UnitCode = " + QuotedStr(ViewState("DtValue")))(0)
                If CekDt() = False Then
                    Exit Sub
                End If

                Row.BeginEdit()
                Row("UnitCode") = TbUnit.Text
                Row("UnitName") = tbUnitName.Text
                Row("LuasTanah") = tbLuasTanah.Text
                Row("LuasBangunan") = tbLuasBangunan.Text
                Row("ArahKavling") = tbArahKav.Text
                Row("KtinggianDPL") = tbKtinggianDPL.Text
                Row("Price") = tbPrice.Text
                'Row("Disc") = tbDisc.Text
                'Row("DiscValue") = tbDiscValue.Text
                'Row("Dpp") = tbDpp.Text
                'Row("TJ") = tbTandaJadi.Text
                'Row("DP") = tbDP.Text
                'Row("DpValue") = tbDPValue.Text
                Row("HargaPer_m2") = tbPricePerm2.Text
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()

            Else

                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If

                If CekExistData(ViewState("Dt"), "UnitCode", TbUnit.Text) = True Then
                    lbStatus.Text = "Unit Code No " + TbUnit.Text + " has already been exist"
                    Exit Sub
                End If



                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("ItemNo") = CInt(lbItemNo.Text)
                dr("UnitCode") = TbUnit.Text
                dr("UnitName") = tbUnitName.Text
                dr("LuasTanah") = tbLuasTanah.Text
                dr("LuasBangunan") = tbLuasBangunan.Text
                dr("ArahKavling") = tbArahKav.Text
                dr("KtinggianDPL") = tbKtinggianDPL.Text
                dr("Price") = tbPrice.Text
                'dr("Disc") = tbDisc.Text
                'dr("DiscValue") = tbDiscValue.Text
                'dr("Dpp") = tbDpp.Text
                'dr("TJ") = tbTandaJadi.Text
                'dr("DP") = tbDP.Text
                'dr("DpValue") = tbDPValue.Text
                dr("HargaPer_m2") = tbPricePerm2.Text
                dr("Remark") = tbRemarkDt.Text
                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            BindGridDt(ViewState("Dt"), GridDt)
            StatusButtonSave(True)

            CountTotalDt()
            'AttachScript("setformatfordt();", Page, Me.GetType())

        Catch ex As Exception
            lbStatus.Text = "btn save Dt Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub

    Private Sub SaveAll()
        Dim SQLString, UpdateSPK As String
        Dim I As Integer
        Try
            If pnlDt.Visible = False Then
                lbStatus.Text = "Detail Data must be saved first"
                Exit Sub
            End If

            If Not (ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit") Then
                Exit Sub
            Else
                ViewState("StateSave") = ViewState("StateHd")
                ViewState("StateHd") = "View"
            End If

            'Save Hd
            If ViewState("StateSave") = "Insert" Then

                tbRef.Text = GetAutoNmbr("SP", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)


                SQLString = "INSERT INTO MKTSPHd (TransNmbr, Status, TransDate,FgSewa,SystemPembayaran, Lokasi,Fasilitas_Akses, NamaSales, PhoneSales, EmailSales, NoHotlineIGL, Perantara1, " + _
                "Perantara2, NamaCPembeli, PhoneCPembeli,EmailCPembeli, Alamat, TotalHarga, TotalDisc, TotalDPP, TotalTJ, TotalDP, TotalAmount, " + _
                "Remark, MasaAngsuran, MasaBerlaku,Disc, DP,  UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbRef.Text) + ", 'H', '" + Format(tbDate.SelectedDate, "yyyy-MM-dd") + "', " + _
                QuotedStr(ddlFgsewa.SelectedValue) + ", " + QuotedStr(ddlSistemBayar.Text) + ", " + _
                QuotedStr(tbArea.Text) + "," + QuotedStr(tbFasilitas.Text) + "," + QuotedStr(tbNamaSales.Text) + "," + QuotedStr(tbTelpSales.Text) + ", " + _
                QuotedStr(tbEmailSales.Text) + "," + QuotedStr(tbNoHotline.Text) + "," + QuotedStr(tbPerantara1.Text) + "," + QuotedStr(tbPerantara2.Text) + "," + QuotedStr(tbCalonPembeli.Text) + ", " + _
                QuotedStr(tbTlp.Text) + "," + QuotedStr(tbEmail.Text) + "," + QuotedStr(tbAlamat.Text) + ", " + _
                QuotedStr(tbTotHarga.Text.Replace(",", "")) + ", " + QuotedStr(tbTotDisc.Text.Replace(",", "")) + ", " + _
                QuotedStr(tbTotDPP.Text.Replace(",", "")) + ", " + QuotedStr(tbTotTJ.Text.Replace(",", "")) + "," + QuotedStr(tbTotDP.Text.Replace(",", "")) + ", " + _
                QuotedStr(tbTotAmount.Text.Replace(",", "")) + ", " + _
                QuotedStr(tbRemark.Text) + "," + QuotedStr(tbAngsuran.Text) + ",'" + Format(tbMasaBerlaku.SelectedDate, "yyyy-MM-dd") + "', " + _
                QuotedStr(tbDiscHd.Text.Replace(",", "")) + ", " + QuotedStr(tbDpHd.Text.Replace(",", "")) + ", " + QuotedStr(ViewState("UserId").ToString) + ", getDate()"


            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM MKTSPHd WHERE TransNmbr = " + QuotedStr(tbRef.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If

                SQLString = "UPDATE MKTSPHd SET TransDate = '" + Format(tbDate.SelectedDate, "yyyy-MM-dd") + "', FgSewa = " + QuotedStr(ddlFgsewa.SelectedValue) + _
                ", MasaAngsuran = " + QuotedStr(tbAngsuran.Text) + _
                ", MasaBerlaku = '" + Format(tbMasaBerlaku.SelectedDate, "yyyy-MM-dd") + _
                "', SystemPembayaran = " + QuotedStr(ddlSistemBayar.Text) + _
                ", Lokasi = " + QuotedStr(tbArea.Text) + _
                ", Fasilitas_Akses = " + QuotedStr(tbFasilitas.Text) + _
                ", NamaSales = " + QuotedStr(tbNamaSales.Text) + _
                ", PhoneSales = " + QuotedStr(tbTelpSales.Text) + _
                ", EmailSales = " + QuotedStr(tbEmailSales.Text) + _
                ", NoHotlineIGL = " + QuotedStr(tbNoHotline.Text) + _
                ", Perantara1 = " + QuotedStr(tbPerantara1.Text) + _
                ", Perantara2 = " + QuotedStr(tbPerantara2.Text) + _
                ", NamaCPembeli = " + QuotedStr(tbCalonPembeli.Text) + _
                ", PhoneCPembeli = " + QuotedStr(tbTlp.Text) + _
                ", EmailCPembeli = " + QuotedStr(tbEmail.Text) + _
                ", Alamat = " + QuotedStr(tbAlamat.Text) + _
                ", TotalHarga = " + QuotedStr(tbTotHarga.Text.Replace(",", "")) + _
                ", TotalDisc = " + QuotedStr(tbTotDisc.Text.Replace(",", "")) + _
                ", TotalDPP = " + QuotedStr(tbTotDPP.Text.Replace(",", "")) + _
                ", TotalTJ = " + QuotedStr(tbTotTJ.Text.Replace(",", "")) + _
                ", TotalDP = " + QuotedStr(tbTotDP.Text.Replace(",", "")) + _
                ", TotalAmount = " + QuotedStr(tbTotAmount.Text.Replace(",", "")) + _
                ", Disc = " + QuotedStr(tbDiscHd.Text.Replace(",", "")) + _
                ", Dp = " + QuotedStr(tbDpHd.Text.Replace(",", "")) + _
                ", Remark = " + QuotedStr(tbRemark.Text) + _
                ", DatePrep = getDate()" + _
                " WHERE TransNmbr = " + QuotedStr(tbRef.Text) + " "
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
                Row(I)("TransNmbr") = tbRef.Text
                Row(I).EndEdit()
            Next

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString 'System.Configuration.ConfigurationManager.AppSettings.Get("ConString")
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, ItemNo, UnitCode, LuasTanah, LuasBangunan,ArahKavling,KtinggianDPL,Price,HargaPer_m2, Remark FROM MKTSPDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            'Dim param As SqlParameter
            '' Create the UpdateCommand.
            'Dim Update_Command = New SqlCommand( _
            '"UPDATE MKTSPDt SET ItemNo = @ItemNo, InvoiceNo = @InvoiceNo, " + _
            '"PONo = @PONo, Invoice = @Invoice, Potongan = @Potongan, InvoiceDate = @InvoiceDate, " + _
            '"DPP = @DPP, PPn = @PPn, PPnInvoice = @PPnInvoice, PPh = @PPh, " + _
            '"PPhInvoice = @PPhInvoice, TotalAmount = @TotalAmount, " + _
            '"Remark = @Remark " + _
            '"WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND ItemNo = @OldItemNo", con)

            '' Define output parameters.
            'Update_Command.Parameters.Add("@ItemNo", SqlDbType.VarChar, 5, "ItemNo")
            'Update_Command.Parameters.Add("@InvoiceNo", SqlDbType.VarChar, 12, "InvoiceNo")
            'Update_Command.Parameters.Add("@InvoiceDate", SqlDbType.DateTime, "InvoiceDate")
            'Update_Command.Parameters.Add("@PONo", SqlDbType.VarChar, 30, "PONo")
            'Update_Command.Parameters.Add("@CostCtr", SqlDbType.VarChar, 5, "CostCtr")
            'Update_Command.Parameters.Add("@Invoice", SqlDbType.Float, 22, "Invoice")
            'Update_Command.Parameters.Add("@Potongan", SqlDbType.Float, 22, "Potongan")
            'Update_Command.Parameters.Add("@DPP", SqlDbType.Float, 22, "DPP")
            'Update_Command.Parameters.Add("@PPn", SqlDbType.Float, 22, "PPn")
            'Update_Command.Parameters.Add("@PPnInvoice", SqlDbType.Float, 22, "PPnInvoice")
            'Update_Command.Parameters.Add("@PPh", SqlDbType.Float, 22, "PPh")
            'Update_Command.Parameters.Add("@PPhInvoice", SqlDbType.Float, 22, "PPhInvoice")
            'Update_Command.Parameters.Add("@TotalAmount", SqlDbType.Float, 22, "TotalAmount")
            'Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 255, "Remark")

            ' '' Define intput (WHERE) parameters.
            'param = Update_Command.Parameters.Add("@OldItemNo", SqlDbType.Int, 4, "ItemNo")
            'param.SourceVersion = DataRowVersion.Original
            '' Attach the update command to the DataAdapter.
            'da.UpdateCommand = Update_Command

            '' Create the DeleteCommand.
            'Dim Delete_Command = New SqlCommand( _
            '    "DELETE FROM MKTSPDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND ItemNo = @ItemNo ", con)
            '' Add the parameters for the DeleteCommand.
            'param = Delete_Command.Parameters.Add("@ItemNo", SqlDbType.Int, 4, "ItemNo")
            'param.SourceVersion = DataRowVersion.Original
            'da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("MKTSPDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt
        Catch ex As Exception
            Throw New Exception("Save All Data Error : " + ex.ToString)
        End Try
    End Sub


    Protected Sub btnSaveTrans_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveTrans.Click
        Dim CurrFilter, Value, UpdateSPK As String
        Try
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
            tbFilter.Text = tbRef.Text
            ddlField.SelectedValue = "TransNmbr"
            btnSearch_Click(Nothing, Nothing)
            tbFilter.Text = CurrFilter
            ddlField.SelectedValue = Value
        Catch ex As Exception
            lbStatus.Text = "Save All Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, pnlDt, GridDt)
            newTrans()
            btnHome.Visible = False
            'ddlReport.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ClearHd()
            Cleardt()
            pnlDt.Visible = True
            BindDataDt("")
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbRef.Text = ""
            tbDate.SelectedDate = ViewState("ServerDate") 'Today
            tbArea.Text = ""
            tbAngsuran.Text = ""
            tbMasaBerlaku.SelectedDate = ViewState("ServerDate") 'Today
            tbAreaName.Text = ""
            tbFasilitas.Text = ""
            tbNamaSales.Text = ""
            tbTelpSales.Text = ""
            tbEmailSales.Text = ""
            tbNoHotline.Text = ""
            tbCalonPembeli.Text = ""
            tbTlp.Text = ""
            tbEmail.Text = ""
            tbAlamat.Text = ""
            tbPerantara1.Text = ""
            tbPerantara2.Text = ""
            tbTotHarga.Text = 0
            tbTotDisc.Text = 0
            tbTotDPP.Text = 0
            tbTotTJ.Text = 0
            tbTotDP.Text = 0
            tbDiscHd.Text = 0
            tbDpHd.Text = 0
            tbTotAmount.Text = 0
            tbRemark.Text = ""
            EnableHd(True)
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub Cleardt()
        Try
            TbUnit.Text = ""
            tbUnitName.Text = ""
            tbLuasTanah.Text = 0
            tbArahKav.Text = ""
            tbKtinggianDPL.Text = 0
            tbPrice.Text = 0
            tbPricePerm2.Text = 0
            'tbDisc.Text = 0
            'tbDiscValue.Text = 0
            'tbDpp.Text = 0
            'tbTandaJadi.Text = 0
            'tbDP.Text = 0
            'tbDPValue.Text = 0
            'tbTotalAmount.Text = 0
            tbRemarkDt.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
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
            lbStatus.Text = "btn Expand Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveAll.Click
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            If IsNothing(ViewState("Dt")) Then
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
        Catch ex As Exception
            lbStatus.Text = "btn save all error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnArea_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnArea.Click
        Dim ResultField As String
        Try
            Session("filter") = "select ID, StructureName from V_MsStructure WHERE LevelCode = '01' "
            ResultField = "ID, StructureName"
            ViewState("Sender") = "btnArea"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchGrid();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Supp Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnUnit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUnit.Click
        Dim ResultField, CriteriaField, sqlstring As String
        Try

            sqlstring = "EXEC S_GetStructure " + QuotedStr(tbArea.Text)
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = sqlstring
            ResultField = "StructureCode, StructureName, Account, AccountNAme, ID"
            CriteriaField = "StructureCode, StructureName, Account, AccountNAme,ID"
            Session("CriteriaField") = CriteriaField.Split(",")
            ViewState("Sender") = "btnUnit"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchGrid();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Supp Error : " + ex.ToString
        End Try
    End Sub



    Protected Sub TbUnit_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TbUnit.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Structure", TbUnit.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                TbUnit.Text = Dr("Supplier_Code")
                tbUnitName.Text = Dr("Supplier_Name")
            Else
                TbUnit.Text = ""
                tbUnitName.Text = ""
            End If
            TbUnit.Focus()
        Catch ex As Exception
            Throw New Exception("tb SuppCode Error : " + ex.ToString)
        End Try
    End Sub



    Private Sub CountTotalDt()
        Dim TotHarga, TotDisc, TotDPP, TotTJ, TotDP, TotAmount As Double
        Dim Dr As DataRow
        Try

            TotHarga = 0
            TotDisc = 0
            TotDPP = 0
            TotTJ = 0
            TotDP = 0
            TotAmount = 0
            For Each Dr In ViewState("Dt").Rows
                If Not Dr.RowState = DataRowState.Deleted Then
                    TotHarga = TotHarga + CFloat(Dr("Price").ToString)
                    'TotDisc = TotDisc + CFloat(Dr("DiscValue").ToString)
                    'TotDPP = TotDPP + CFloat(Dr("DPP").ToString)
                    'TotTJ = TotTJ + CFloat(Dr("TJ").ToString)
                    'TotDP = TotDP + CFloat(Dr("DPValue").ToString)
                    'TotAmount = TotAmount + CFloat(Dr("AmountForex").ToString)
                End If
            Next
            tbTotHarga.Text = FormatNumber(TotHarga, ViewState("DigitHome"))
            tbTotDisc.Text = (CFloat(tbTotHarga.Text) * CFloat(tbDiscHd.Text)) / 100
            tbTotDPP.Text = CFloat(tbTotHarga.Text)
            tbTotDP.Text = ((CFloat(tbTotDPP.Text) * CFloat(tbDpHd.Text)) / 100) - CFloat(tbTotTJ.Text)
            tbTotAmount.Text = CFloat(tbTotDPP.Text) - CFloat(tbTotDP.Text)


            tbTotDisc.Text = FormatNumber(tbTotDisc.Text, ViewState("DigitHome"))
            tbTotDPP.Text = FormatNumber(tbTotDPP.Text, ViewState("DigitHome"))
            tbTotDP.Text = FormatNumber(tbTotDP.Text, ViewState("DigitHome"))
            tbTotAmount.Text = FormatNumber(tbTotAmount.Text, ViewState("DigitHome"))
            AttachScript("setformathd();", Page, Me.GetType())


        Catch ex As Exception
            Throw New Exception("Count Total Dt Error : " + ex.ToString)
        End Try
    End Sub


    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt.Click, btnAddDt2.Click
        Try
            Cleardt()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt") = "Insert"
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            StatusButtonSave(False)
            lbItemNo.Text = GetNewItemNo(ViewState("Dt"))
            'btnAccount.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt error : " + ex.ToString
        End Try
    End Sub

    Function CekHd() As Boolean
        Dim CekDate As Date
        Try
            If tbDate.IsNull Then
                lbStatus.Text = MessageDlg("Transaction Date must have value")
                tbDate.Focus()
                Return False
            End If

            If tbDpHd.Text = "" Or tbDpHd.Text = 0 Or tbDpHd.Text < 0 Then
                lbStatus.Text = MessageDlg("DP Must Have Value")
                tbDpHd.Focus()
                Return False
            End If


            'If CInt(Session(Request.QueryString("KeyId"))("Year")) <> Year(tbDate.SelectedValue) Or CInt(Session(Request.QueryString("KeyId"))("Period")) <> Month(tbDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(Session(Request.QueryString("KeyId"))("Period")) + " " + Session(Request.QueryString("KeyId"))("Year").ToString.Trim)
            '    Return False
            'End If

            'If tbSuppCode.Text.Trim = "" Then
            '    lbStatus.Text = MessageDlg("Supplier must have value")
            '    btnSupp.Focus()
            '    Return False
            'End If

            'If tbNoSPK.Text = "" Then
            '    lbStatus.Text = MessageDlg("SPK No must have value")
            '    tbNoSPK.Focus()
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
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If

                'If Dr("TagihanBAPPersen").ToString = 0 Or Dr("TagihanBAPPersen").ToString = "" Then
                '    lbStatus.Text = MessageDlg(" BAP %  Must Have Value On Item '" + Dr("ItemNo").ToString + "'")
                '    Return False
                'End If


                'If Dr("TagihanBAP").ToString = 0 Or Dr("TagihanBAP").ToString = "" Then
                '    lbStatus.Text = MessageDlg("BAP Saat Ini Must Have Value")
                '    Return False
                'End If

                If Dr("Price").ToString = "0" Or Dr("Price").ToString = "" Then
                    lbStatus.Text = MessageDlg("Price Must Have Value")
                    Return False
                End If





            Else

                'If tbBAPnowPersen.Text = "" Or tbBAPnowPersen.Text = "0" Then
                '    lbStatus.Text = MessageDlg("BAP % Must Have Value")
                '    tbLuas.Focus()
                '    Return False
                'End If

                'If tbBAPnow.Text = "" Or tbBAPnow.Text = "0" Then
                '    lbStatus.Text = MessageDlg("BAP Saat Ini Must Have Value")
                '    tbBAPnow.Focus()
                '    Return False
                'End If



                If tbLuasBangunan.Text = "" Then
                    lbStatus.Text = MessageDlg("Luas Bangunan Must Have Value")
                    tbLuasBangunan.Focus()
                    Return False
                End If

                If tbKtinggianDPL.Text = "" Or tbKtinggianDPL.Text = "0" Then
                    lbStatus.Text = MessageDlg("Ketingian DPL Must Have Value")
                    tbKtinggianDPL.Focus()
                    Return False
                End If

                'If tbDP.Text = "" Or tbDP.Text = "0" Then
                '    lbStatus.Text = MessageDlg("DP Must Have Value")
                '    tbDP.Focus()
                '    Return False
                'End If

                If TbUnit.Text = "" Then
                    lbStatus.Text = MessageDlg("Unit Must Have Value")
                    TbUnit.Focus()
                    Return False
                End If

            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        CheckAll(GridView1, sender)
    End Sub



    Protected Sub LbAdvSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LbAdvSearch.Click
        Dim FDateName, FDateValue, FilterName, FilterValue As String
        Try
            FDateName = "Date, Invoice Date"
            FDateValue = "TransDate, SuppInvDate"
            FilterName = "Reference, Date, Status,  Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), Status,  Remark"
            Session("DateFieldName") = FDateName.Split(",")
            Session("DateFieldValue") = FDateValue.Split(",")
            Session("FieldName") = FilterName.Split(",")
            Session("FieldValue") = FilterValue.Split(",")
            AttachScript("OpenFilterCriteria();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "Advanced Search Click Error : " + ex.ToString
        End Try
    End Sub

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
                    'ChangeReport("View", "Y", ddlCurr.SelectedValue = ViewState("Currency"), tbDate, tbRate, tbPPnNo, tbPPndate, tbPpnRate) 'ddlReport.SelectedValue
                    BindDataDt(ViewState("TransNmbr"))
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, pnlDt, GridDt)
                    btnHome.Visible = True
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
                        FillTextBoxHd(ViewState("TransNmbr"))
                        BindDataDt(ViewState("TransNmbr"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        btnHome.Visible = False
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                        FillTextBoxHd(ViewState("TransNmbr"))
                        EnableHd(True)


                    Else
                        lbStatus.Text = MessageDlg("Data must Hold or Get Approval to edit")
                        Exit Sub
                    End If
                ElseIf DDL.SelectedValue = "Print" Then
                    Try
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        Session("SelectCommand") = "EXEC S_FNFormPenawaran '''" + GVR.Cells(2).Text + "'''," + QuotedStr(ViewState("UserId"))
                        Session("ReportFile") = ".../../../Rpt/FormPenawaran.frx"
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

    Protected Sub GridDt_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridDt.PageIndexChanging
        Try
            GridDt.PageIndex = e.NewPageIndex
            GridDt.DataSource = ViewState("Dt")
            GridDt.DataBind()
        Catch ex As Exception
            lbStatus.Text = "Grid dT Page Index Change Error : " + ex.ToString + " page index : " + e.NewPageIndex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridDt.RowCommand
        Try
            If e.CommandName = "Insert" Then
                btnAddDt_Click(Nothing, Nothing)
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
            dr = ViewState("Dt").Select("UnitCode = " + GVR.Cells(2).Text)
            dr(0).Delete()
            BindGridDt(ViewState("Dt"), GridDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            CountTotalDt()

        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        'Dim Dr As DataRow
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            FillTextBoxDt(GVR.Cells(2).Text)
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)

            ViewState("StateDt") = "Edit"
            ViewState("DtValue") = GVR.Cells(2).Text

            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub lbSupp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSupp.Click
    '    Try
    '        AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsSupplier')();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lbStatus.Text = "lb Supplier Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            tbRef.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToDropList(ddlFgsewa, Dt.Rows(0)("FgSewa").ToString)
            BindToDropList(ddlSistemBayar, Dt.Rows(0)("SystemPembayaran").ToString)
            BindToText(tbArea, Dt.Rows(0)("Lokasi").ToString)
            BindToText(tbAreaName, Dt.Rows(0)("LokasiName").ToString)
            BindToText(tbFasilitas, Dt.Rows(0)("fasilitas_Akses").ToString)
            BindToText(tbNamaSales, Dt.Rows(0)("NamaSales").ToString)
            BindToText(tbEmailSales, Dt.Rows(0)("EmailSales").ToString)
            BindToText(tbTelpSales, Dt.Rows(0)("NoHotlineIGL").ToString)
            BindToText(tbNoHotline, Dt.Rows(0)("NoHotlineIGL").ToString)
            BindToText(tbPerantara1, Dt.Rows(0)("Perantara1").ToString)
            BindToText(tbPerantara2, Dt.Rows(0)("Perantara2").ToString)
            BindToText(tbCalonPembeli, Dt.Rows(0)("NamaCPembeli").ToString)
            BindToText(tbTlp, Dt.Rows(0)("PhoneCPembeli").ToString)
            BindToText(tbEmail, Dt.Rows(0)("EmailCPembeli").ToString)
            BindToText(tbAlamat, Dt.Rows(0)("Alamat").ToString)
            BindToText(tbTotHarga, Dt.Rows(0)("TotalHarga").ToString, ViewState("DigitCurr"))
            BindToText(tbTotDisc, Dt.Rows(0)("TotalDisc").ToString, ViewState("DigitCurr"))
            BindToText(tbTotDPP, Dt.Rows(0)("TotalDPP").ToString, ViewState("DigitCurr"))
            BindToText(tbTotTJ, Dt.Rows(0)("TotalTJ").ToString, ViewState("DigitCurr"))
            BindToText(tbTotDP, Dt.Rows(0)("TotalDP").ToString, ViewState("DigitCurr"))
            BindToText(tbTotAmount, Dt.Rows(0)("TotalAmount").ToString, ViewState("DigitCurr"))
            BindToText(tbDiscHd, Dt.Rows(0)("Disc").ToString, ViewState("DigitCurr"))
            BindToText(tbDpHd, Dt.Rows(0)("Dp").ToString, ViewState("DigitCurr"))
            BindToText(tbAngsuran, Dt.Rows(0)("MasaAngsuran").ToString)
            BindToDate(tbMasaBerlaku, Dt.Rows(0)("MasaBerlaku").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)

            If Dt.Rows(0)("FileUpLayout").ToString = "" Then
                'cbKtp.Checked = False
                lbDokInv.Text = "Not Yet Uploaded"
            Else
                lbDokInv.Text = Dt.Rows(0)("FileUpLayout").ToString
                'cbKtp.Checked = True
            End If

        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub



    Protected Sub FillTextBoxDt(ByVal UnitCode As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("UnitCode = " + UnitCode)
            If Dr.Length > 0 Then
                lbItemNo.Text = Dr(0)("ItemNo").ToString
                BindToText(TbUnit, Dr(0)("UnitCode").ToString)
                BindToText(tbLuasTanah, Dr(0)("LuasTanah").ToString)
                BindToText(tbLuasBangunan, Dr(0)("LuasBangunan").ToString)
                BindToText(tbArahKav, Dr(0)("arahKavling").ToString)
                BindToText(tbKtinggianDPL, Dr(0)("KtinggianDPL").ToString)
                BindToText(tbPrice, Dr(0)("Price").ToString, ViewState("DigitHome"))
                BindToText(tbPricePerm2, Dr(0)("HargaPer_m2").ToString, ViewState("DigitHome"))
                'BindToText(tbDisc, Dr(0)("Disc").ToString, ViewState("DigitHome"))
                'BindToText(tbDiscValue, Dr(0)("DiscValue").ToString, ViewState("DigitHome"))
                'BindToText(tbDpp, Dr(0)("Dpp").ToString, ViewState("DigitHome"))
                'BindToText(tbTandaJadi, Dr(0)("TJ").ToString, ViewState("DigitHome"))
                'BindToText(tbDP, Dr(0)("DP").ToString, ViewState("DigitHome"))
                'BindToText(tbDPValue, Dr(0)("DpValue").ToString, ViewState("DigitHome"))
                'BindToText(tbTotalAmount, Dr(0)("AmountForex").ToString, ViewState("DigitHome"))
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub

    Private Sub StatusButtonSave(ByVal Bool As Boolean)
        btnSaveAll.Visible = Bool
        btnSaveTrans.Visible = Bool
        btnBack.Visible = Bool
    End Sub


End Class
