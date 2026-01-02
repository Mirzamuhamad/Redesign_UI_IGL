Imports System.Data
Imports System.Data.SqlClient
'Imports System.Data.Odbc
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc


Partial Class INVInfrastruktur
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select * From V_FININVInfHD"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Try
            If Not IsPostBack Then
                InitProperty()
                SetInit()
                FillCombo(ddlpph, "SELECT PPHCode, PPHName FROM MsPPH", True, "PPHCode", "PPHName", ViewState("DBConnection"))
                FillCombo(ddlTerm, "EXEC S_GetTerm", False, "Term_Code", "Term_Name", ViewState("DBConnection"))
                FillCombo(ddlCurr, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
                Session("AdvanceFilter") = ""
            End If
            lbStatus.Text = ""
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If

            ' hasil dari search dialog
            If Not Session("Result") Is Nothing Then

                If ViewState("Sender") = "btnSupp" Then
                    tbSuppCode.Text = Session("Result")(0).ToString
                    tbSuppName.Text = Session("Result")(1).ToString
                End If

                If ViewState("Sender") = "btnGetBAP" Then
                    Dim drResult As DataRow
                    Dim ExistRow As DataRow()
                    ''Insert Detail
                    For Each drResult In Session("Result").Rows
                        ExistRow = ViewState("Dt").Select("BAP_No = " + QuotedStr(drResult("BAP_No").ToString))
                        If ExistRow.Count = 0 Then
                            'insert
                            If tbSuppCode.Text.Trim = "" Then
                                BindToText(tbSuppCode, drResult("SuppCode").ToString)
                                BindToText(tbSuppName, drResult("Supplier_Name").ToString)
                            End If

                            Dim dr As DataRow
                            dr = ViewState("Dt").NewRow
                            dr("BAP_No") = drResult("BAP_No").ToString
                            dr("SPK_No") = drResult("No_SPK").ToString
                            dr("NilaiBAP") = drResult("TotalBAP").ToString

                            ViewState("Dt").Rows.Add(dr)
                        End If


                        'Insert Sub Detail
                        lblBAPNumber.Text = drResult("BAP_No").ToString
                        lblSPKNumber.Text = drResult("No_SPK").ToString

                        'MultiView1.ActiveViewIndex = 1

                        Dim drDtResult As DataRow
                        Dim ExistRowDT As DataRow()
                        Dim MaxItem As String
                        Dim DtPekerjaan As DataTable
                        Dim SQLString As String

                        SQLString = "EXEC S_GetBAPDetail " + QuotedStr(drResult("BAP_No").ToString)
                        DtPekerjaan = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)

                        For Each drDtResult In DtPekerjaan.Rows
                            ExistRowDT = ViewState("Dt2").Select("ItemNo+'|'+BAP_No = " + QuotedStr(drDtResult("ItemNo").ToString + "|" + lblBAPNumber.Text))
                            If ExistRowDT.Count = 0 Then
                                Dim Dtdr As DataRow
                                Dtdr = ViewState("Dt2").NewRow
                                Dtdr("ItemNo") = drDtResult("ItemNo")
                                Dtdr("BAP_No") = lblBAPNumber.Text
                                Dtdr("UraianPekerjaan") = drDtResult("UraianPekerjaan")
                                Dtdr("Luas") = drDtResult("Luas")
                                Dtdr("Biaya") = drDtResult("Biaya")
                                Dtdr("BAPPersen") = drDtResult("BAPPersen")
                                Dtdr("BAP") = drDtResult("BAP")
                                Dtdr("BAPSebelumPersen") = drDtResult("BAPSebelumPersen")
                                Dtdr("BAPSebelum") = drDtResult("BAPSebelum")
                                Dtdr("TagihanBAPPersen") = drDtResult("TagihanBAPPersen")
                                Dtdr("TagihanBAP") = drDtResult("TagihanBAP")
                                Dtdr("SisaBAP") = drDtResult("SisaBAP")
                                Dtdr("Remark") = drDtResult("Remark")
                                ViewState("Dt2").Rows.Add(Dtdr)
                            End If
                        Next
                    Next

                    BindGridDt(ViewState("Dt"), GridDt)
                    'EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
                    StatusButtonSave(True)
                    CountTotalDt2()
                End If

                Session("Result") = Nothing
                ViewState("Sender") = Nothing

            End If

            FubInv.Attributes("onchange") = "UploadInvoice(this)"

            FubBAPExt.Attributes("onchange") = "UploadBAP(this)"

            FubFaktur.Attributes("onchange") = "UploadFaktur(this)"

            FubDokLain.Attributes("onchange") = "UploadLain(this)"

        Catch ex As Exception
            lbStatus.Text = "Page Load Error : " + ex.ToString
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

            If FubFaktur.FileBytes.Length > 3500000 Then
                lbStatus.Text = MessageDlg("Ukuran File Terlalu Besar. !! Max Upload 3.5Mb")
                Exit Sub
            End If

            If Right(FubInv.FileName, 4) <> ".pdf" Then
                lbStatus.Text = MessageDlg("Upload Pdf File Only !")
                Exit Sub
            End If
            Dim path2, namafile2, SQLString1 As String
            Dim dt As DataTable
            path2 = Server.MapPath("~/DokumenINVInfra/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubInv.FileName
            namafile2 = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubInv.FileName

            SQLString1 = "UPDATE FININVInfHD SET DokInvoice = " + QuotedStr(namafile2) + _
            " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            FubInv.SaveAs(path2)
            SQLExecuteNonQuery(SQLString1, ViewState("DBConnection").ToString)

            dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            lbDokInv.Text = dt.Rows(0)("DokInvoice").ToString
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

            If dr.Rows(0)("DokInvoice").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            filePath = dr.Rows(0)("DokInvoice").ToString
            URL = ResolveUrl("~/DokumenINVInfra/" + filePath)
            Dim s As String = "window.open('" & URL & "', '_blank');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)

        Catch ex As Exception
            lbStatus.Text = "lbModerator_Click Error : " + ex.ToString
        End Try
    End Sub

  
    Protected Sub btnsaveFaktur_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsaveFaktur.Click

        Try

            Dim dr As DataTable
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("save this transaction first, to upload the dokumen")
                Exit Sub
            End If

            If FubFaktur.FileBytes.Length > 3500000 Then
                lbStatus.Text = MessageDlg("Ukuran File Terlalu Besar. !! Max Upload 3.5Mb")
                Exit Sub
            End If

            If Right(FubFaktur.FileName, 4) <> ".pdf" Then
                lbStatus.Text = MessageDlg("Upload Pdf File Only !")
                Exit Sub
            End If

            Dim Path2KK, NameFile2KK, SQLString2 As String
            Dim dt As DataTable

            Path2KK = Server.MapPath("~/DokumenINVInfra/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubFaktur.FileName
            NameFile2KK = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubFaktur.FileName
            SQLString2 = "UPDATE FININVInfHD SET DokFaktur = " + QuotedStr(NameFile2KK) + _
            " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            FubFaktur.SaveAs(Path2KK)
            SQLExecuteNonQuery(SQLString2, ViewState("DBConnection").ToString)


            dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            lbFaktur.Text = dt.Rows(0)("DokFaktur").ToString
            'lblmassageKK.Visible = True
            FubFaktur.Visible = False
            btnClearFaktur.Visible = True

        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub lbFaktur_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbFaktur.Click
        Try
            Dim dr As DataTable
            Dim filePath, URL As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            If dr.Rows(0)("DokFaktur").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            filePath = dr.Rows(0)("DokFaktur").ToString
            URL = ResolveUrl("~/DokumenINVInfra/" + filePath)
            Dim s As String = "window.open('" & URL & "', '_blank');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)

        Catch ex As Exception
            lbStatus.Text = "lbModerator_Click Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnsaveLain_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsaveLain.Click

        Try

            Dim dr As DataTable
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("save this transaction first, to upload the dokumen")
                Exit Sub
            End If

            If FubBAPExt.FileBytes.Length > 3500000 Then
                lbStatus.Text = MessageDlg("Ukuran File Terlalu Besar. !! Max Upload 3.5Mb")
                Exit Sub
            End If

            If Right(FubDokLain.FileName, 4) <> ".pdf" Then
                lbStatus.Text = MessageDlg("Upload Pdf File Only !")
                Exit Sub
            End If

            Dim dt As DataTable
            Dim Path2SPPT, NameFile2SPPT, SQLString3 As String
            Path2SPPT = Server.MapPath("~/DokumenINVInfra/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubDokLain.FileName
            NameFile2SPPT = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubDokLain.FileName

            SQLString3 = "UPDATE FININVInfHD SET DokLain = " + QuotedStr(NameFile2SPPT) + _
            " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            FubDokLain.SaveAs(Path2SPPT)
            SQLExecuteNonQuery(SQLString3, ViewState("DBConnection").ToString)


            dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            lbDokLain.Text = dt.Rows(0)("DokLain").ToString
            'lblmassageSPPT.Visible = True
            FubDokLain.Visible = False
            btnClearLain.Visible = True

        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbDokLain_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbDokLain.Click
        Try
            Dim dr As DataTable
            Dim filePath, URL As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            If dr.Rows(0)("DokLain").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            filePath = dr.Rows(0)("DokLain").ToString
            URL = ResolveUrl("~/DokumenINVInfra/" + filePath)
            Dim s As String = "window.open('" & URL & "', '_blank');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)

        Catch ex As Exception
            lbStatus.Text = "lbDokLain_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnsaveBAP_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsaveBAP.Click

        Try
            Dim dr As DataTable
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("save this transaction first, to upload the dokumen")
                Exit Sub
            End If

            If FubBAPExt.FileBytes.Length > 3500000 Then
                lbStatus.Text = MessageDlg("Ukuran File Terlalu Besar. !! Max Upload 3.5Mb")
                Exit Sub
            End If

            If Right(FubBAPExt.FileName, 4) <> ".pdf" Then
                lbStatus.Text = MessageDlg("Upload Pdf File Only !")
                Exit Sub
            End If

            Dim dt As DataTable
            Dim Path2STTS, NameFile2STTS, SQLString4 As String
            Path2STTS = Server.MapPath("~/DokumenINVInfra/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubBAPExt.FileName
            NameFile2STTS = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubBAPExt.FileName

            SQLString4 = "UPDATE FININVInfHD SET DokBAP = " + QuotedStr(NameFile2STTS) + _
            " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            FubBAPExt.SaveAs(Path2STTS)
            SQLExecuteNonQuery(SQLString4, ViewState("DBConnection").ToString)


            dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            lbBAP.Text = dt.Rows(0)("DokBAP").ToString
            'lblmassageSTTS.Visible = True
            FubBAPExt.Visible = False
            btnClearBAP.Visible = True

        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbBAP_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbBAP.Click
        Try
            Dim dr As DataTable
            Dim filePath, URL As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            If dr.Rows(0)("DokBAP").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            filePath = dr.Rows(0)("DokBAP").ToString
            URL = ResolveUrl("~/DokumenINVInfra/" + filePath)
            Dim s As String = "window.open('" & URL & "', '_blank');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)

        Catch ex As Exception
            lbStatus.Text = "lbBAP_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnClearInv_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClearInv.Click
        Try
            Dim dr As DataTable
            Dim filePath As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            filePath = dr.Rows(0)("DokInvoice").ToString


            If File.Exists(Server.MapPath("~/DokumenINVInfra/" + filePath)) = True Then
                File.Delete(Server.MapPath("~/DokumenINVInfra/" + filePath))
                SQLExecuteNonQuery("UPDATE FININVInfHD Set DokInvoice = '' WHERE TransNmbr = '" + tbCode.Text + "' ", ViewState("DBConnection").ToString)

                lbDokInv.Text = "Not yet uploaded"
                FubInv.Visible = True
                btnClearInv.Visible = False
            End If

           

        Catch ex As Exception
            lbStatus.Text = "lbBAP_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnClearFaktur_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClearFaktur.Click
        Try
            Dim dr As DataTable
            Dim filePath As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            filePath = dr.Rows(0)("DokFaktur").ToString
            If File.Exists(Server.MapPath("~/DokumenINVInfra/" + filePath)) = True Then
                File.Delete(Server.MapPath("~/DokumenINVInfra/" + filePath))
                SQLExecuteNonQuery("UPDATE FININVInfHD Set DokFaktur = '' WHERE TransNmbr = '" + tbCode.Text + "' ", ViewState("DBConnection").ToString)

                lbFaktur.Text = "Not yet uploaded"
                FubFaktur.Visible = True
                btnClearFaktur.Visible = False
            End If

        Catch ex As Exception
            lbStatus.Text = "lbBAP_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnClearBAP_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClearBAP.Click
        Try
            Dim dr As DataTable
            Dim filePath As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            filePath = dr.Rows(0)("DokBAP").ToString

            If File.Exists(Server.MapPath("~/DokumenINVInfra/" + filePath)) = True Then
                File.Delete(Server.MapPath("~/DokumenINVInfra/" + filePath))
                SQLExecuteNonQuery("UPDATE FININVInfHD Set DokBAP = '' WHERE TransNmbr = '" + tbCode.Text + "' ", ViewState("DBConnection").ToString)
                FubBAPExt.Visible = True
                lbBAP.Text = "Not yet uploaded"
                btnClearBAP.Visible = False
            End If

        Catch ex As Exception
            lbStatus.Text = "lbBAP_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnClearLain_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClearLain.Click
        Try
            Dim dr As DataTable
            Dim filePath As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            filePath = dr.Rows(0)("DokLain").ToString
            If File.Exists(Server.MapPath("~/DokumenINVInfra/" + filePath)) = True Then
                File.Delete(Server.MapPath("~/DokumenINVInfra/" + filePath))
                SQLExecuteNonQuery("UPDATE FININVInfHD Set DokLain = '' WHERE TransNmbr = '" + tbCode.Text + "' ", ViewState("DBConnection").ToString)
                lbDokLain.Text = "Not yet uploaded"
                FubDokLain.Visible = True
                btnClearLain.Visible = False
            End If

        Catch ex As Exception
            lbStatus.Text = "btnClearLain_Click Error : " + ex.ToString
        End Try
    End Sub


    Private Sub CountTotalDt2()
        Dim NialiBAP As Double
        Dim Dr As DataRow
        Try


            NialiBAP = 0
            For Each Dr In ViewState("Dt").Rows
                If Not Dr.RowState = DataRowState.Deleted Then
                    NialiBAP = NialiBAP + CFloat(Dr("NilaiBAP").ToString)
                End If
            Next
            tbTotalBAP.Text = FormatNumber(NialiBAP, ViewState("DigitHome"))
            tbppnValue.Text = Math.Floor(CFloat(tbppn.Text) * CFloat(tbTotalBAP.Text) / 100)
            tbPphValue.Text = Math.Floor(CFloat(tbpph.Text) * CFloat(tbTotalBAP.Text) / 100)

            tbTotalAmount.Text = (CFloat(tbTotalBAP.Text) + CFloat(tbppnValue.Text)) - CFloat(tbPphValue.Text)

            tbTotalAmount.Text = FormatNumber(tbTotalAmount.Text, ViewState("DigitHome"))
            tbppnValue.Text = FormatNumber(tbppnValue.Text, ViewState("DigitHome"))
            tbPphValue.Text = FormatNumber(tbPphValue.Text, ViewState("DigitHome"))
            AttachScript("setformathd();", Page, Me.GetType())
        Catch ex As Exception
            Throw New Exception("Count Total Dt Error : " + ex.ToString)
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
            'ddlCommand.Items.Add("Print")
            'ddlCommand2.Items.Add("Print")
        End If

        tbppn.Attributes.Add("OnBlur", "setformathd();")
        tbppnValue.Attributes.Add("OnBlur", "setformathd();")
        tbTotalBAP.Attributes.Add("OnBlur", "setformathd();")
        tbTotalAmount.Attributes.Add("OnBlur", "setformathd();")
        tbpph.Attributes.Add("OnBlur", "setformathd();")
        tbPphValue.Attributes.Add("OnBlur", "setformathd();")
        tbType.Attributes.Add("OnBlur", "setformathd();")


        Me.tbTotalAmount.Attributes.Add("ReadOnly", "True")
        Me.tbppnValue.Attributes.Add("ReadOnly", "True")
        Me.tbTotalBAP.Attributes.Add("ReadOnly", "True")
        'Proteksi agar hanya angka saja yang bisa di input
        tbNilaiBAP.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbTotalBAP.Attributes.Add("OnKeyDown", "return PressNumeric();")

        tbppn.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbppnValue.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbpph.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbPphValue.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbTotalAmount.Attributes.Add("OnKeyDown", "return PressNumeric();")

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
        Return "SELECT * From V_FININVInfDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function
    Private Function GetStringDt2(ByVal Nmbr As String) As String
        Return "SELECT * From V_FININVInfDt2 WHERE TransNmbr = " + QuotedStr(Nmbr)
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
                        If (GVR.Cells(3).Text = "P") Or (GVR.Cells(3).Text = "G") Or (GVR.Cells(3).Text = "H") Then
                            ListSelectNmbr = GVR.Cells(2).Text
                            If Pertamax Then
                                Result = "'''" + ListSelectNmbr + "''"
                                Pertamax = False
                            Else
                                Result = Result + ",''" + ListSelectNmbr + "''"
                            End If
                        End If
                    End If
                Next
                Result = Result + "'"
                Session("DBConnection") = ViewState("DBConnection")
                Session("SelectCommand") = "EXEC S_FNFormCIPBAPInv " + Result + ", " + QuotedStr(ViewState("UserId").ToString) + ""
                Session("ReportFile") = ".../../../Rpt/FormSuppBAPInv.frx"
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
                        Result = ExecSPCommandGo(ActionValue, "S_FNINVInf", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            tbSuppCode.Enabled = State
            tbType.Enabled = False
            tbTotalBAP.Enabled = State
            tbppn.Enabled = State
            tbppnValue.Enabled = State
            tbpph.Enabled = State
            tbPphValue.Enabled = State
            tbTotalAmount.Enabled = State
            'tbRemark.Enabled = State
            'ddlpph.Enabled = State

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
    Private Sub BindDataDt2(ByVal Nmbr As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt2") = Nothing
            dt = SQLExecuteQuery(GetStringDt2(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt2") = dt
            BindGridDt(dt, GridDt2)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt2 Error : " + ex.ToString)
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
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnCancelDt2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelDt2.Click
        Try
            MovePanel(pnlEditDt2, pnlDt2)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt2  Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt.Click
        Dim Row As DataRow
        Try
            If ViewState("StateDt") = "Edit" Then
                If ViewState("DtValue") <> tbBAPNo.Text Then
                    If CekExistData(ViewState("Dt"), "BAP_No", tbBAPNo.Text) Then
                        lbStatus.Text = "BAP No " + tbBAPNo.Text + " has been already exist"
                        Exit Sub
                    End If
                End If
                Row = ViewState("Dt").Select("BAP_No = " + QuotedStr(ViewState("DtValue")))(0)
                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                Row("BAP_No") = tbBAPNo.Text
                Row("SPK_No") = tbSPKNo.Text
                Row("NilaiBAP") = tbNilaiBAP.Text
                Row("Remark") = tbRemarkdt.Text
                Row.EndEdit()
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If
                If CekExistData(ViewState("Dt"), "BAP_No", tbBAPNo.Text) Then
                    lbStatus.Text = "BAP NO " + tbBAPNo.Text + " has been already exist"
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("BAP_No") = tbBAPNo.Text
                dr("SPK_No") = tbSPKNo.Text
                dr("NilaiBAP") = tbNilaiBAP.Text
                dr("Remark") = tbRemarkdt.Text
                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
            BindGridDt(ViewState("Dt"), GridDt)
            StatusButtonSave(True)
            CountTotalDt2()
        Catch ex As Exception
            lbStatus.Text = "btn save Dt Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub
    Protected Sub btnSaveDt2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt2.Click
        Try
            If CekDt2() = False Then
                btnSaveDt2.Focus()
                Exit Sub
            End If
            If ViewState("StateDt2") = "Edit" Then
                Dim Row As DataRow
                Row = ViewState("Dt2").Select("ItemNo'+|+'BAP_No = " + QuotedStr(lbItemNo.Text + "|" + lblBAPNumber.Text))(0)
                Row.BeginEdit()
                Row("UraianPekerjaan") = tbUraian.Text
                Row("BAP_No") = lblBAPNumber.Text
                Row("Luas") = tbLuas.Text
                Row("Biaya") = tbBiaya.Text
                Row("BAPPersen") = tbBAPPersen.Text
                Row("BAP") = tbBAP.Text
                Row("BAPSebelumPersen") = tbBAPSebelumPersen.Text
                Row("BAPSebelum") = tbBAPSebelum.Text
                Row("TagihanBAPPersen") = tbBAPnowPersen.Text
                Row("TagihanBAP") = tbBAPnow.Text
                Row("SisaBAP") = tbSisaBAP.Text
                Row("Remark") = tbRemarkDt2.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt2").NewRow
                dr("BAP_No") = lblBAPNumber.Text
                dr("ItemNo") = CInt(lbItemNo.Text)
                dr("UraianPekerjaan") = tbUraian.Text
                dr("Luas") = tbLuas.Text
                dr("Biaya") = tbBiaya.Text
                dr("BAPPersen") = tbBAPPersen.Text
                dr("BAP") = tbBAP.Text
                dr("BAPSebelumPersen") = tbBAPSebelumPersen.Text
                dr("BAPSebelum") = tbBAPSebelum.Text
                dr("TagihanBAPPersen") = tbBAPnowPersen.Text
                dr("TagihanBAP") = tbBAPnow.Text
                dr("SisaBAP") = tbSisaBAP.Text
                dr("Remark") = tbRemarkDt2.Text
                ViewState("Dt2").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt2, pnlDt2)
            Dim drow As DataRow()
            drow = ViewState("Dt2").Select("ItemNo = " + QuotedStr(TrimStr(lbItemNo.Text)))
            If drow.Length > 0 Then
                BindGridDt(drow.CopyToDataTable, GridDt2)
                GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
            Else
                Dim DtTemp As New DataTable
                DtTemp = ViewState("Dt2").Clone
                DtTemp.Rows.Add(DtTemp.NewRow()) ' create a new blank row to the DataTable
                GridDt2.DataSource = DtTemp
                GridDt2.DataBind()
                GridDt2.Columns(0).Visible = False
            End If

            CountTotalDt2()
            btnCancelDt2.Visible = True
            btnSaveDt2.Visible = True

            'EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
            'BindGridDt(ViewState("Dt2"), GridDt2)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save dt2 Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub
    Private Sub CountTotalDt()
        'Dim QtyTotal As Double
        'Dim Dr As DataRow
        'Dim drow As DataRow()
        'Dim havedetail As Boolean
        'Try
        '    drow = ViewState("Dt2").Select("Bap = " + QuotedStr(TrimStr(lbFADt2.Text)))
        '    QtyTotal = 0
        '    If drow.Length > 0 Then
        '        havedetail = False
        '        For Each Dr In drow.CopyToDataTable.Rows
        '            If Not Dr.RowState = DataRowState.Deleted Then
        '                QtyTotal = QtyTotal + CFloat(Dr("Qty").ToString)
        '            End If
        '        Next

        '    End If
        '    Dr = ViewState("Dt").Select("FixedAsset = " + QuotedStr(TrimStr(lbFADt2.Text)))(0)

        '    Dr.BeginEdit()
        '    Dr("Qty") = QtyTotal 'FormatNumber(QtyTotal, ViewState("DigitQty"))
        '    'Dr("Total") = FormatNumber(QtyTotal * price, ViewState("DigitHome"))
        '    Dr.EndEdit()
        '    BindGridDt(ViewState("Dt"), GridDt)
        '    'lbQtyTotal.Text = FormatNumber(QtyTotal, ViewState("DigitQty"))
        Dim dr As DataRow
        Dim NilaiBAP As Double
        Try

            NilaiBAP = 0
            For Each dr In ViewState("Dt").Select("BAP_No = " + QuotedStr(tbBAPNo.Text.Trim))
                If Not dr.RowState = DataRowState.Deleted Then
                    NilaiBAP = NilaiBAP + CFloat(dr("NilaiBAP").ToString)
                End If
            Next
            tbTotalBAP.Text = FormatFloat(NilaiBAP, ViewState("DigitHome"))
            tbppnValue.Text = (CFloat(tbTotalBAP.Text) * CFloat(tbppn.Text)) / 100
            tbPphValue.Text = (CFloat(tbTotalBAP.Text) * CFloat(tbpph.Text)) / 100
            tbTotalAmount.Text = (CFloat(tbTotalBAP.Text) + CFloat(tbppnValue.Text)) - CFloat(tbPphValue.Text)
            AttachScript("setformathd();", Page, Me.GetType())
        Catch ex As Exception
            Throw New Exception("Count Total Dt Error : " + ex.ToString)
        End Try
    End Sub
    Private Sub SaveAll()
        Dim SQLString, Path1, Path2, Path3, Path4, namafile1, namafile2, namafile3, namafile4 As String
        Dim I As Integer
        Dim CekMenu As String
        Try

            CekMenu = CheckMenuLevel("Insert", ViewState("MenuLevel").Rows(0))
            If CekMenu <> "" Then
                lbStatus.Text = CekMenu
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
                tbCode.Text = GetAutoNmbr("CII", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)

                'Path1 = Server.MapPath("~/DokumenINVInfra/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubInv.FileName
                'namafile1 = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubInv.FileName

                'Path2 = Server.MapPath("~/DokumenINVInfra/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubFaktur.FileName
                'namafile2 = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubFaktur.FileName

                'Path3 = Server.MapPath("~/DokumenINVInfra/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubBAPExt.FileName
                'namafile3 = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubBAPExt.FileName


                'Path4 = Server.MapPath("~/DokumenINVInfra/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubDokLain.FileName
                'namafile4 = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubDokLain.FileName


                SQLString = "INSERT INTO FININVInfHd (TransNmbr,Status,TransDate,SuppCode,PPHCode,SupplierINV_No,TotalBAP,PPN,PPNValue,PPH,PPHValue,TotalAmount, FakturPajakNo, InvoiceDate,BapExternal,PPNDate,DokumenLain, Term, DueDate, Currency, ForexRate,  " + _
                 "Remark, UserPrep, DatePrep) " + _
                "SELECT '" + tbCode.Text + "', 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', " + _
                QuotedStr(tbSuppCode.Text) + ", " + QuotedStr(ddlpph.SelectedValue) + ", " + _
                QuotedStr(tbSuppInv.Text) + "," + _
                QuotedStr(tbTotalBAP.Text.Replace(",", "")) + ", " + QuotedStr(tbppn.Text.Replace(",", "")) + ", " + QuotedStr(tbppnValue.Text.Replace(",", "")) + "," + _
                QuotedStr(tbpph.Text.Replace(",", "")) + ", " + QuotedStr(tbPphValue.Text.Replace(",", "")) + "," + QuotedStr(tbTotalAmount.Text.Replace(",", "")) + ", " + _
                QuotedStr(tbFakturPajak.Text) + ",'" + Format(tbInvoiceDate.SelectedValue, "yyyy-MM-dd") + "', " + _
                QuotedStr(tbBapExt.Text) + ",'" + Format(tbPpnDate.SelectedValue, "yyyy-MM-dd") + "', " + _
                QuotedStr(tbDokumenLain.Text) + ", " + _
                QuotedStr(ddlTerm.Text) + ",'" + Format(tbDueDate.SelectedValue, "yyyy-MM-dd") + "', " + _
                QuotedStr(ddlCurr.Text) + ", " + QuotedStr(tbRate.Text) + ", " + _
                QuotedStr(tbRemark.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", getDate()"
                '  QuotedStr(namafile1) + ", " + QuotedStr(namafile2) + "," + QuotedStr(namafile3) + "," + QuotedStr(namafile4) + ", " + _



                'FubInv.SaveAs(path1)
                'FubFaktur.SaveAs(path2)
                'FubBAPExt.SaveAs(path3)
                'FubDokLain.SaveAs(path4)

            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM FININVInfHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE FININVInfHd SET Transdate = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "' " + _
                ", InvoiceDate = '" + Format(tbInvoiceDate.SelectedValue, "yyyy-MM-dd") + "' " + _
                ", PpnDate = '" + Format(tbPpnDate.SelectedValue, "yyyy-MM-dd") + "' " + _
                ", FakturPajakNo = " + QuotedStr(tbFakturPajak.Text) + _
                ", BAPExternal = " + QuotedStr(tbBapExt.Text) + _
                ", DokumenLain = " + QuotedStr(tbDokumenLain.Text) + _
                ", Term = " + QuotedStr(ddlTerm.Text) + _
                ", DueDate = '" + Format(tbDueDate.SelectedValue, "yyyy-MM-dd") + "' " + _
                ", Currency = " + QuotedStr(ddlCurr.Text) + _
                ", ForexRate = " + QuotedStr(tbRate.Text) + _
                ", SuppCode = " + QuotedStr(tbSuppCode.Text) + _
                ", PPHCode = " + QuotedStr(ddlpph.SelectedValue) + _
                ", SupplierINV_No = " + QuotedStr(tbSuppInv.Text) + _
                ", TotalBAP = " + QuotedStr(tbTotalBAP.Text.Replace(",", "")) + _
                ", Ppn = " + QuotedStr(tbppn.Text.Replace(",", "")) + _
                ", PpnValue= " + QuotedStr(tbppnValue.Text.Replace(",", "")) + _
                ", PPh = " + QuotedStr(tbpph.Text.Replace(",", "")) + _
                ", PphValue= " + QuotedStr(tbPphValue.Text.Replace(",", "")) + _
                ", TotalAmount= " + QuotedStr(tbTotalAmount.Text.Replace(",", "")) + _
                ", Remark = " + QuotedStr(tbRemark.Text) + _
                ", DatePrep = getDate()" + _
                " WHERE TransNmbr = '" + tbCode.Text + "'"
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

            Row = ViewState("Dt2").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbCode.Text
                Row(I).EndEdit()
            Next

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString 'System.Configuration.ConfigurationManager.AppSettings.Get("ConString")
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand(" SELECT TransNmbr, BAP_No, SPK_No, NilaiBAP, Remark FROM FININVInfDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand


            Dim Dt As New DataTable("FININVInfDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt

            'save dt2
            cmdSql = New SqlCommand("SELECT TransNmbr, ItemNo, BAP_No, UraianPekerjaan, Luas, Biaya,BAPPersen,BAP,BAPSebelumPersen,BAPSebelum,TagihanBAPPersen,TagihanBAP, SisaBAP,  Remark FROM FININVInfDt2 WHERE TransNmbr  = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            dbcommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt2 As New DataTable("FININVInfDt2")

            Dt2 = ViewState("Dt2")
            da.Update(Dt2)
            Dt2.AcceptChanges()
            ViewState("Dt2") = Dt2
                
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

                If GetCountRecord(ViewState("Dt2")) = 0 Then
                    lbStatus.Text = MessageDlg("Detail Payment must have at least 1 record")
                    Exit Sub
                End If

                For Each dr In ViewState("Dt").Rows
                    If CekDt(dr) = False Then
                        Exit Sub
                    End If
                Next

                SaveAll()
                ModifyInput2(False, pnlInput, pnlDt, GridDt)
                ModifyInput2(False, pnlInput, pnlDt2, GridDt2)
                btnGoEdit.Visible = True
                Menu2.Items.Item(1).Enabled = True
                MultiView2.ActiveViewIndex = 1
                Menu2.Items.Item(1).Selected = True
                'btnGoEdit.Visible = True
                btnGetBAP.Visible = False
                GridDt.Columns(0).Visible = False
                'MovePanel(pnlInput, PnlHd)
                'CurrFilter = tbFilter.Text
                'Value = ddlField.SelectedValue
                'tbFilter.Text = tbCode.Text
                'ddlField.SelectedValue = "TransNmbr"
                'btnSearch_Click(Nothing, Nothing)
                'tbFilter.Text = CurrFilter
                'ddlField.SelectedValue = Value

                ' ClientScript.RegisterStartupScript(Me.[GetType](), "alert", "alert('You clicked YES!')", True)
            Else

                'If pnlDt.Visible = False Then
                '    lbStatus.Text = "Detail Data must be saved first"
                '    Exit Sub
                'End If
                If CekHd() = False Then
                    Exit Sub
                End If
                If GetCountRecord(ViewState("Dt")) = 0 Then
                    lbStatus.Text = MessageDlg("Detail must have at least 1 record")
                    Exit Sub
                End If
                If GetCountRecord(ViewState("Dt2")) = 0 Then
                    lbStatus.Text = MessageDlg("Detail Payment must have at least 1 record")
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
                'ClientScript.RegisterStartupScript(Me.[GetType](), "alert", "alert('You clicked NO!')", True)
            End If


        Catch ex As Exception
            lbStatus.Text = "Save All Dt Error : " + ex.ToString
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

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try

            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, pnlDt, GridDt)
            newTrans()
            btnHome.Visible = False
            MultiView1.ActiveViewIndex = 0
            Menu1.Items.Item(0).Selected = True
            MultiView2.ActiveViewIndex = 0
            Menu2.Items.Item(0).Selected = True
            GridDt.Columns(0).Visible = True
            btnAddDt.Visible = False
            btnAddDtKe2.Visible = False
            EnableHd(True)

            tbCode.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub Menu2_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu2.MenuItemClick
        MultiView2.ActiveViewIndex = Int32.Parse(e.Item.Value)
        If Menu2.Items.Item(0).Selected = True Then
            If ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit" Then
                'btnGoEdit.Visible = False
                btnGetBAP.Visible = True
                GridDt.Columns(0).Visible = True
            End If
        End If
        If Menu2.Items.Item(1).Selected = True Then
            If ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit" Then
                'btnGoEdit.Visible = True
                btnGetBAP.Visible = False
                GridDt.Columns(0).Visible = False
            End If

        End If
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ViewState("DigitCurr") = ViewState("DigitHome")
            ViewState("TransNmbr") = ""
            ClearHd()
            Cleardt()
            Cleardt2()
            ViewState("DigitCurr") = 2
            pnlDt.Visible = True
            BindDataDt("")
            BindDataDt2("")
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbCode.Text = ""
            tbDate.SelectedDate = ViewState("ServerDate") 'Today
            tbDueDate.SelectedDate = ViewState("ServerDate") 'Today
            tbSuppCode.Text = ""
            tbSuppName.Text = ""
            tbSuppInv.Text = ""
            tbType.Text = ""
            tbRemark.Text = ""
            ddlpph.SelectedValue = ""
            MultiView1.ActiveViewIndex = 0
            tbTotalBAP.Text = "0"
            tbTotalAmount.Text = "0"
            tbppn.Text = ViewState("PPN")
            tbppnValue.Text = "0"
            tbpph.Text = "0"
            tbPphValue.Text = "0"
            tbTotalAmount.Text = "0"
            tbInvoiceDate.SelectedDate = ViewState("ServerDate") 'Today
            tbPpnDate.SelectedDate = ViewState("ServerDate") 'Today
            tbFakturPajak.Text = ""
            tbBapExt.Text = ""
            tbDokumenLain.Text = ""
            ddlCurr.SelectedValue = ViewState("Currency")

            ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try

            tbBAPNo.Text = ""
            tbSPKNo.Text = ""
            tbNilaiBAP.Text = "0"
            tbRemarkdt.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt2()
        Try
            tbUraian.Text = ""
            tbLuas.Text = 0
            tbBiaya.Text = 0
            tbBAPPersen.Text = 0
            tbBAP.Text = 0
            tbBAPSebelumPersen.Text = 0
            tbBAPSebelum.Text = 0
            tbBAPnowPersen.Text = 0
            tbBAPnow.Text = 0
            tbSisaBAP.Text = 0
            tbRemarkDt2.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt 2 Error " + ex.ToString)
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
            'If IsNothing(ViewState("Dt")) Then
            '    lbStatus.Text = MessageDlg("Detail must have at least 1 record")
            '    Exit Sub
            'End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt2")) = 0 Then
                lbStatus.Text = MessageDlg("Detail Payment must have at least 1 record")
                Exit Sub
            End If
            For Each dr In ViewState("Dt").Rows
                If CekDt(dr) = False Then
                    Exit Sub
                End If
            Next
            SaveAll()
            newTrans()
            MultiView1.ActiveViewIndex = 0
        Catch ex As Exception
            lbStatus.Text = "btn save all error : " + ex.ToString
        End Try
    End Sub




    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt.Click, btnAddDtKe2.Click
        Cleardt()
        If CekHd() = False Then
            Exit Sub
        End If
        ViewState("StateDt") = "Insert"
        MovePanel(pnlDt, pnlEditDt)
        EnableHd(False)
        StatusButtonSave(False)
    End Sub

    Public Function GetNewItemNo()
        Dim Row As DataRow()
        Dim R As DataRow
        Dim MaxItem As Integer = 0
        Row = ViewState("Dt2").Select("BAPNo = " + QuotedStr(TrimStr(lblBAPNumber.Text)))

        For Each R In Row
            If CInt(R("ItemNo").ToString) > MaxItem Then
                MaxItem = CInt(R("ItemNo").ToString)
            End If
        Next
        MaxItem = MaxItem + 1
        Return CStr(MaxItem)
    End Function


    Protected Sub btnAddDt2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt2Ke2.Click, btnAddDt2.Click
        Try
            Cleardt2()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt2") = "Insert"
            MovePanel(pnlDt2, pnlEditDt2)
            EnableHd(False)
            StatusButtonSave(False)
            lbItemNo.Text = GetNewItemNo()
        Catch ex As Exception
            lbStatus.Text = "btn add dt error : " + ex.ToString
        End Try
    End Sub
    Function CekHd() As Boolean
        Try

            If tbDate.IsNull Then
                lbStatus.Text = MessageDlg("Date must have value")
                tbDate.Focus()
                Return False
            End If
            If tbSuppCode.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Supplier must have value")
                tbSuppCode.Focus()
                Return False
            End If


            If tbSuppInv.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Supplier Invoice must have value")
                tbSuppInv.Focus()
                CountTotalDt2()
                Return False
            End If

            If tbBapExt.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("BAP External must have value")
                tbBapExt.Focus()
                CountTotalDt2()
                Return False
            End If

            If FubInv.FileBytes.Length > 3500000 Then
                lbStatus.Text = MessageDlg("Ukuran File Terlalu Besar. !! Max Upload 3.5Mb")
                Return False
            End If

            If FubFaktur.FileBytes.Length > 3500000 Then
                lbStatus.Text = MessageDlg("Ukuran File Terlalu Besar. !! Max Upload 3.5Mb")
                Return False
            End If


            If FubBAPExt.FileBytes.Length > 3500000 Then
                lbStatus.Text = MessageDlg("Ukuran File Terlalu Besar. !! Max Upload 3.5Mb")
                Return False
            End If


            If FubDokLain.FileBytes.Length > 3500000 Then
                lbStatus.Text = MessageDlg("Ukuran File Terlalu Besar. !! Max Upload 3.5Mb")
                Return False
            End If

            If FubFaktur.FileName <> "" Then
                If Right(FubFaktur.FileName, 4) <> ".pdf" Then
                    lbStatus.Text = MessageDlg("Upload Pdf File Only !")
                    Return False
                End If
            End If

            If FubBAPExt.FileName <> "" Then
                If Right(FubBAPExt.FileName, 4) <> ".pdf" Then
                    lbStatus.Text = MessageDlg("Upload Pdf File Only !")
                    Return False
                End If
            End If

            If FubDokLain.FileName <> "" Then
                If Right(FubDokLain.FileName, 4) <> ".pdf" Then
                    lbStatus.Text = MessageDlg("Upload Pdf File Only !")
                    Return False
                End If
            End If

            If FubInv.FileName <> "" Then
                If Right(FubInv.FileName, 4) <> ".pdf" Then
                    lbStatus.Text = MessageDlg("Upload Pdf File Only !")
                    Return False
                End If
            End If


            Return True

        Catch ex As Exception
            Throw New Exception("Ceh Hd Error : " + ex.ToString)
        End Try
    End Function

    Function CekDt(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not (Dr Is Nothing) Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("BAP_No").ToString.Trim = "" Then
                    lbStatus.Text = "Process Must Have Value"
                    Return False
                End If


                If Dr("NilaiBAP").ToString = 0 Or Dr("NilaiBAP").ToString = "" Then
                    lbStatus.Text = "Qty Output Must Have Value"
                    Return False
                End If



            Else
                If tbBAPNo.Text.Trim = "" Then
                    lbStatus.Text = "BAP No Must Have Value"
                    tbBAPNo.Focus()
                    Return False
                End If

                If tbSPKNo.Text.Trim = "" Then
                    lbStatus.Text = "SPK No Must Have Value"
                    tbSPKNo.Focus()
                    Return False
                End If

                If tbNilaiBAP.Text = "0" Or tbNilaiBAP.Text = "" Then
                    lbStatus.Text = "Nilai Must Have Value"
                    tbNilaiBAP.Focus()
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function
    Function CekDt2(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not Dr Is Nothing Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If

                If Dr("TagihanBAPPersen").ToString = "0" Or Dr("TagihanBAPPersen").ToString = "" Then
                    lbStatus.Text = MessageDlg(" BAP %  Must Have Value")
                    Return False
                End If


                If Dr("TagihanBAP").ToString = "0" Or Dr("TagihanBAP").ToString = "" Then
                    lbStatus.Text = MessageDlg("BAP Saat Ini Must Have Value")
                    Return False
                End If

                If Dr("Biaya").ToString = "0" Or Dr("Biaya").ToString = "" Then
                    lbStatus.Text = MessageDlg("Biaya Must Have Value")
                    Return False
                End If

            Else

                If tbBAPnowPersen.Text = "" Or tbBAPnowPersen.Text = "0" Then
                    lbStatus.Text = MessageDlg("BAP % Must Have Value")
                    tbLuas.Focus()
                    Return False
                End If

                If tbBAPnow.Text = "" Or tbBAPnow.Text = "0" Then
                    lbStatus.Text = MessageDlg("BAP Saat Ini Must Have Value")
                    tbBAPnow.Focus()
                    Return False
                End If

                If tbLuas.Text = "" Or tbLuas.Text = "0" Then
                    lbStatus.Text = MessageDlg("Luas Must Have Value")
                    tbLuas.Focus()
                    Return False
                End If

                If tbBiaya.Text = "" Or tbBiaya.Text = "0" Then
                    lbStatus.Text = MessageDlg("Biaya Must Have Value")
                    tbBiaya.Focus()
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
            FilterName = "Reference, Date, Status, Supplier Code, Supplier Name,  Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), Status, SuppCode, Supplier_Name, Remark"
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

                    BindDataDt(ViewState("TransNmbr"))
                    BindDataDt2(ViewState("TransNmbr"))
                    EnableHd(False)
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, pnlDt, GridDt)
                    ModifyInput2(False, pnlInput, pnlDt2, GridDt2)
                    btnHome.Visible = True
                    MultiView1.ActiveViewIndex = 0
                    Menu1.Items.Item(0).Selected = True
                    If GVR.Cells(3).Text = "D" Then
                        Menu2.Items.Item(1).Enabled = False
                    Else
                        Menu2.Items.Item(1).Enabled = True
                    End If

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
                        BindDataDt2(ViewState("TransNmbr"))

                        ViewState("StateHd") = "Edit"

                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        FillTextBoxHd(ViewState("TransNmbr"))
                        ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
                        btnHome.Visible = False
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
                        MultiView1.ActiveViewIndex = 0
                        Menu1.Items.Item(0).Selected = True
                        EnableHd(True)
                        btnAddDt.Visible = False
                        btnAddDtKe2.Visible = False


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

                        Session("DBConnection") = ViewState("DBConnection")
                        Session("SelectCommand") = "EXEC S_FNFormCIPBAPInv '" + GVR.Cells(2).Text + "', " + QuotedStr(ViewState("UserId").ToString) + " "
                        Session("ReportFile") = ".../../../Rpt/FormSuppBAPINV.frx"
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
            If e.CommandName = "View" Then
                Dim GVR As GridViewRow
                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))

                If GVR.Cells(2).Text = "&nbsp;" Then
                    Exit Sub
                End If
                Dim lbFA As Label

                lbFA = GVR.FindControl("lbFa")

                lblBAPNumber.Text = GVR.Cells(2).Text
                lblSPKNumber.Text = GVR.Cells(3).Text

                MultiView1.ActiveViewIndex = 1

                If ViewState("StateHd") = "View" Then
                    ModifyInput2(False, pnlInput, pnlDt2, GridDt2)
                Else
                    ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
                End If
                GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
                If ViewState("Dt2") Is Nothing Then
                    BindDataDt2(ViewState("TransNmbr"))
                End If
                Dim drow As DataRow()
                drow = ViewState("Dt2").Select("BAP_No = " + QuotedStr(TrimStr(lblBAPNumber.Text))) '+ " AND FAName = " + QuotedStr(TrimStr(lbFANameDt2.Text)) + " AND FAStatus = " + QuotedStr(TrimStr(lbStatusFA.Text)))
                '                drow = ViewState("Dt2").Select("FixedAsset = " + QuotedStr(TrimStr(lbFADt2.Text)))
                If drow.Length > 0 Then
                    BindGridDt(drow.CopyToDataTable, GridDt2)
                    GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
                    GridDt2.Columns(0).Visible = False
                    btnAddDt2.Visible = False
                    btnAddDt2Ke2.Visible = False
                Else
                    Dim DtTemp As DataTable
                    DtTemp = ViewState("Dt2").Clone
                    DtTemp.Rows.Add(DtTemp.NewRow())
                    GridDt2.DataSource = DtTemp
                    GridDt2.DataBind()
                    GridDt2.Columns(0).Visible = False
                End If

            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow
            'If GetCountRecord(ViewState("Dt2")) <> 0 Then
            '    lbStatus.Text = " Data Detail exist"
            '    Exit Sub
            'Else
            GVR = GridDt.Rows(e.RowIndex)
            dr = ViewState("Dt").Select("BAP_No = " + QuotedStr(GVR.Cells(2).Text))

            dr(0).Delete()
            BindGridDt(ViewState("Dt"), GridDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
            'End If

        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Dim TotalQty As Decimal = 0

    Protected Sub GridDt2_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt2.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow
            GVR = GridDt2.Rows(e.RowIndex)
            dr = ViewState("Dt2").Select("ItemNo = " + QuotedStr(GVR.Cells(1).Text))
            dr(0).Delete()

            Dim drow As DataRow()
            drow = ViewState("Dt2").Select("BAP_No = " + QuotedStr(TrimStr(lblBAPNumber.Text)))
            If drow.Length > 0 Then
                BindGridDt(drow.CopyToDataTable, GridDt2)
                GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
            Else
                Dim DtTemp As New DataTable
                DtTemp = ViewState("Dt2").Clone
                DtTemp.Rows.Add(DtTemp.NewRow()) ' create a new blank row to the DataTable
                GridDt2.DataSource = DtTemp
                GridDt2.DataBind()
                GridDt2.Columns(0).Visible = False
            End If

            'CountTotalDt()

            'BindGridDt(ViewState("Dt2"), GridDt2)
            'EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt 2 Row Deleting Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            ViewState("DtValue") = GVR.Cells(2).Text
            FillTextBoxDt(ViewState("DtValue"))
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            btnSaveDt.Focus()
            StatusButtonSave(False)
            AttachScript("setformathd();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    'Dim BaseForex As Decimal = 0
    'Dim PPnForex As Decimal = 0
    'Dim TotalForex As Decimal = 0

    '' untuk tampilkan data total di grid
    'Protected Sub GridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt.RowDataBound
    '    Try
    '        If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "FixedAsset")) Then
    '            If e.Row.RowType = DataControlRowType.DataRow Then
    '                BaseForex += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "AmountForex"))
    '            ElseIf e.Row.RowType = DataControlRowType.Footer Then
    '                tbBaseForex.Text = FormatNumber(BaseForex, ViewState("DigitCurr"))
    '                tbPPNForex.Text = FormatNumber(((CFloat(tbBaseForex.Text) * CFloat(tbPPN.Text)) / 100).ToString, ViewState("DigitCurr").ToString)
    '                tbTotalForex.Text = FormatNumber(CFloat(tbBaseForex.Text) + CFloat(tbPPNForex.Text), ViewState("DigitCurr").ToString)
    '            End If

    '        End If
    '    Catch ex As Exception
    '        lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            tbCode.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToText(tbSuppCode, Dt.Rows(0)("suppCode").ToString)
            BindToText(tbSuppName, Dt.Rows(0)("Supplier_Name").ToString)
            BindToDropList(ddlpph, Dt.Rows(0)("PphCode").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
            BindToText(tbType, Dt.Rows(0)("PphType").ToString)
            BindToText(tbTotalBAP, Dt.Rows(0)("TotalBAP").ToString, ViewState("DigitCurr"))
            BindToText(tbppn, Dt.Rows(0)("Ppn").ToString, ViewState("DigitCurr"))
            BindToText(tbppnValue, Dt.Rows(0)("PpnValue").ToString, ViewState("DigitCurr"))
            BindToText(tbpph, Dt.Rows(0)("Pph").ToString, ViewState("DigitCurr"))
            BindToText(tbPphValue, Dt.Rows(0)("PphValue").ToString, ViewState("DigitCurr"))
            BindToText(tbTotalAmount, Dt.Rows(0)("TotalAmount").ToString, ViewState("DigitCurr"))
            BindToText(tbFakturPajak, Dt.Rows(0)("FakturPajakNo").ToString)
            BindToDropList(ddlTerm, Dt.Rows(0)("Term").ToString)
            BindToDate(tbDueDate, Dt.Rows(0)("Duedate").ToString)
            BindToText(tbRate, Dt.Rows(0)("ForexRate").ToString)
            BindToDropList(ddlCurr, Dt.Rows(0)("Currency").ToString)
            BindToText(tbBapExt, Dt.Rows(0)("BAPExternal").ToString)
            BindToText(tbDokumenLain, Dt.Rows(0)("DokumenLain").ToString)
            BindToText(tbSuppInv, Dt.Rows(0)("SupplierINV_No").ToString)
            BindToDate(tbInvoiceDate, Dt.Rows(0)("InvoiceDate").ToString)
            BindToDate(tbPpnDate, Dt.Rows(0)("PPNDate").ToString)
            '1
            If Dt.Rows(0)("DokInvoice").ToString = "" Then
                'cbKtp.Checked = False
                lbDokInv.Text = "Not Yet Uploaded"
            Else
                lbDokInv.Text = Dt.Rows(0)("DokInvoice").ToString
                'cbKtp.Checked = True
            End If

            '2
            If Dt.Rows(0)("DokFaktur").ToString = "" Then
                lbFaktur.Text = "Not Yet Uploaded"
            Else
                lbFaktur.Text = Dt.Rows(0)("DokFaktur").ToString
            End If

            '3
            If Dt.Rows(0)("DokBAP").ToString = "" Then
                lbBAP.Text = "Not Yet Uploaded"
            Else
                lbBAP.Text = Dt.Rows(0)("DokBAP").ToString

            End If

            '4
            If Dt.Rows(0)("DokLain").ToString = "" Then

                lbDokLain.Text = "Not Yet Uploaded"
            Else
                lbDokLain.Text = Dt.Rows(0)("DokLain").ToString

            End If
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal FixedAsset As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("BAP_No= " + QuotedStr(FixedAsset))
            If Dr.Length > 0 Then
                BindToText(tbBAPNo, Dr(0)("BAP_No").ToString)
                BindToText(tbSPKNo, Dr(0)("SPK_No").ToString)
                BindToText(tbNilaiBAP, Dr(0)("NilaiBAP").ToString, ViewState("DigitHome"))
                BindToText(tbRemarkdt, Dr(0)("Remark").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt2(ByVal FALoc As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt2").select("ItemNo+'|'+ BAP_No= " + QuotedStr(FALoc))
            If Dr.Length > 0 Then

                lbItemNo.Text = FALoc.ToString
                BindToText(tbUraian, Dr(0)("UraianPekerjaan").ToString)
                BindToText(tbLuas, Dr(0)("Luas").ToString, ViewState("DigitHome"))
                BindToText(tbBiaya, Dr(0)("Biaya").ToString, ViewState("DigitHome"))
                BindToText(tbBAPPersen, Dr(0)("BAPPersen").ToString, ViewState("DigitHome"))
                BindToText(tbBAP, Dr(0)("BAP").ToString, ViewState("DigitHome"))
                BindToText(tbBAPSebelumPersen, Dr(0)("BAPSebelumPersen").ToString, ViewState("DigitHome"))
                BindToText(tbBAPSebelum, Dr(0)("BAPSebelum").ToString, ViewState("DigitHome"))
                BindToText(tbBAPnowPersen, Dr(0)("TagihanBAPPersen").ToString, ViewState("DigitHome"))
                BindToText(tbBAPnow, Dr(0)("TagihanBAP").ToString, ViewState("DigitHome"))
                BindToText(tbSisaBAP, Dr(0)("SisaBAP").ToString, ViewState("DigitHome"))
                BindToText(tbRemarkdt, Dr(0)("Remark").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail 2 error : " + ex.ToString)
        End Try
    End Sub

    Private Sub StatusButtonSave(ByVal Bool As Boolean)
        btnSaveAll.Visible = Bool
        btnSaveTrans.Visible = Bool
        btnBack.Visible = Bool
    End Sub



    Private Function cekValue(ByVal val As String) As String
        If val.Trim = "" Then
            Return "0"
        Else
            Return val
        End If
    End Function


    Protected Sub GridDt2_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt2.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt2.Rows(e.NewEditIndex)
            FillTextBoxDt2(GVR.Cells(1).Text + "|" + lblBAPNumber.Text)
            MovePanel(pnlDt2, pnlEditDt2)
            EnableHd(False)
            ViewState("StateDt2") = "Edit"
            StatusButtonSave(False)
            btnSaveDt2.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid dt2 Editing Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnBackDt2ke1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBackDt2ke1.Click, btnBackDt2ke2.Click
        Try
            MultiView1.ActiveViewIndex = 0
        Catch ex As Exception
            lbStatus.Text = "btn back Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSupp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSupp.Click
        Dim ResultField As String
        Try
            Session("filter") = "select * from V_GetSupplier"
            ResultField = "Supplier_Code, Supplier_Name, Supplier_Type"
            ViewState("Sender") = "btnSupp"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchGrid();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Supp Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnGetInv_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGetBAP.Click
        Dim ResultField, CriteriaField, sqlstring, ResultSame As String
        Try
            If tbSuppCode.Text = "" Then
                sqlstring = "SELECT * FROM V_GetBAP "
            Else
                sqlstring = "SELECT * FROM V_GetBAP WHERE SuppCode =  " + QuotedStr(tbSuppCode.Text)
            End If
            '" + tbLpCode.Text.Trim + "','Y'," + QuotedStr(ViewState("TransNmbr")) '" + ddlReport.SelectedValue.ToString + "
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = sqlstring
            ResultField = "BAP_No, No_SPK, SuppCode, Supplier_Name, TotalBAP"
            CriteriaField = "BAP_No, No_SPK, SuppCode, Supplier_Name, TotalBAP"
            'Session("ClickSame") = "Bill_To"
            Session("Column") = ResultField.Split(",")
            Session("CriteriaField") = CriteriaField.Split(",")
            ResultSame = "SuppCode, Supplier_Name"
            Session("ResultSame") = ResultSame.Split(",")
            ViewState("Sender") = "btnGetBAP"
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
            'AttachScript("FindMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Customer Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlpph_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlpph.SelectedIndexChanged
        Dim Type As String
        Try

            Type = SQLExecuteScalar("SELECT Type FROM MsPPH WHERE PphCode = '" + ddlpph.SelectedValue + "'", ViewState("DBConnection"))
            tbType.Text = Type

            If ddlpph.SelectedValue = "" Then
                tbpph.Enabled = False
                tbPphValue.Enabled = False
                tbpph.Text = 0
                tbPphValue.Text = 0
                tbTotalAmount.Text = FormatFloat(CFloat(tbTotalBAP.Text) + CFloat(tbppnValue.Text), ViewState("DigitCurr"))
            Else
                tbpph.Enabled = True
                tbPphValue.Enabled = True
                If Type = "-" Then
                    tbTotalAmount.Text = FormatFloat(CFloat(tbTotalBAP.Text) + CFloat(tbppnValue.Text) - CFloat(tbPphValue.Text), ViewState("DigitCurr"))
                Else
                    tbTotalAmount.Text = FormatFloat(CFloat(tbTotalBAP.Text) + CFloat(tbppnValue.Text) + CFloat(tbPphValue.Text), ViewState("DigitCurr"))
                End If

            End If

        Catch ex As Exception

        End Try
    End Sub


    Protected Sub tbSuppCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbSuppCode.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Supplier", tbSuppCode.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbSuppCode.Text = Dr("Supplier_Code")
                tbSuppName.Text = Dr("Supplier_Name")
                BindToDropList(ddlCurr, Dr("Currency"))
                BindToDropList(ddlTerm, Dr("Term"))
                tbDueDate.SelectedDate = FindDueDate(ddlTerm.SelectedValue, tbDate.SelectedDate, ViewState("DBConnection").ToString)
            Else
                tbSuppCode.Text = ""
                tbSuppName.Text = ""
                ddlCurr.SelectedValue = Session("Currency")
                tbppn.Text = ViewState("PPn")
            End If
            ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
            'AttachScript("setformat();", Page, Me.GetType())
            tbSuppCode.Focus()
        Catch ex As Exception
            Throw New Exception("tb SuppCode Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlTerm_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTerm.SelectedIndexChanged
        tbDueDate.SelectedDate = FindDueDate(ddlTerm.SelectedValue, tbDate.SelectedDate, ViewState("DBConnection").ToString)

    End Sub

    Protected Sub tbDate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbDate.SelectionChanged

        tbDueDate.SelectedDate = FindDueDate(ddlTerm.SelectedValue, tbDate.SelectedDate, ViewState("DBConnection").ToString)

    End Sub

    Protected Sub ddlCurr_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCurr.SelectedIndexChanged
        If ViewState("InputCurrency") = "Y" Then
            RefreshMaster("S_GetCurrency", "Currency", "Currency", ddlCurr, ViewState("DBConnection"))
            ViewState("InputCurrency") = Nothing
        End If
        ChangeCurrency(ddlCurr, tbDate, tbRate, Session("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
        'ChangeReport("Edit", "Y", ddlCurr.SelectedValue = ViewState("Currency"), tbDate, tbRate, tbPPnNo, tbPPndate, tbPpnRate) 'ddlReport.SelectedValue

        'tbPPndate.Enabled = CFloat(tbPPN.Text) > 0
        'If Not (tbPPndate.IsNull) Then
        '    If tbPPndate.SelectedValue.ToString <> "" Then
        '        tbPpnRate.Text = FormatNumber(FindTaxRate(ddlCurr.SelectedValue, tbPPndate.SelectedValue, ViewState("DBConnection").ToString), ViewState("DigitCurr"))
        '    End If
        'End If

        'AttachScript("setformat();", Page, Me.GetType())
        tbRate.Focus()
    End Sub


End Class
