Imports System.Data
Imports System.Data.SqlClient
'Imports System.Data.Odbc
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc


Partial Class BeginLicenseAdm
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select * From V_PRCLicenAdmBeginHD"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Try
            If Not IsPostBack Then
                InitProperty()
                SetInit()
                FillCombo(ddlCurr, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
                FillCombo(ddlUnit, "SELECT Unit_Code, Unit_Name FROM VMsUnit", True, "Unit_Code", "Unit_Name", ViewState("DBConnection"))
                Session("AdvanceFilter") = ""
            End If
            lbStatus.Text = ""
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If

            ' hasil dari search dialog
            If Not Session("Result") Is Nothing Then

                If ViewState("Sender") = "btnKegiatan" Then
                    tbKegiatanCode.Text = Session("Result")(0).ToString
                    tbKegiatanName.Text = Session("Result")(1).ToString
                End If

                If ViewState("Sender") = "btnArea" Then
                    tbAreaCode.Text = Session("Result")(0).ToString
                    tbAreaName.Text = Session("Result")(1).ToString
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
            path2 = Server.MapPath("~/Image/DokumenBeginLicense/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubInv.FileName
            namafile2 = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubInv.FileName

            SQLString1 = "UPDATE PRCLicenAdmBeginHD SET FilDok_1 = " + QuotedStr(namafile2) + _
            " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            FubInv.SaveAs(path2)
            SQLExecuteNonQuery(SQLString1, ViewState("DBConnection").ToString)

            dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            lbDokInv.Text = dt.Rows(0)("FilDok_1").ToString
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

            If dr.Rows(0)("FilDok_1").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            filePath = dr.Rows(0)("FilDok_1").ToString
            URL = ResolveUrl("~/Image/DokumenBeginLicense/" + filePath)
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

            Path2KK = Server.MapPath("~/Image/DokumenBeginLicense/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubFaktur.FileName
            NameFile2KK = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubFaktur.FileName
            SQLString2 = "UPDATE PRCLicenAdmBeginHD SET FileDok_2 = " + QuotedStr(NameFile2KK) + _
            " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            FubFaktur.SaveAs(Path2KK)
            SQLExecuteNonQuery(SQLString2, ViewState("DBConnection").ToString)


            dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            lbFaktur.Text = dt.Rows(0)("FileDok_2").ToString
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

            If dr.Rows(0)("FileDok_2").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            filePath = dr.Rows(0)("FileDok_2").ToString
            URL = ResolveUrl("~/Image/DokumenBeginLicense/" + filePath)
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
            Path2SPPT = Server.MapPath("~/Image/DokumenBeginLicense/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubDokLain.FileName
            NameFile2SPPT = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubDokLain.FileName

            SQLString3 = "UPDATE PRCLicenAdmBeginHD SET DokLain = " + QuotedStr(NameFile2SPPT) + _
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
            URL = ResolveUrl("~/Image/DokumenBeginLicense/" + filePath)
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
            Path2STTS = Server.MapPath("~/Image/DokumenBeginLicense/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubBAPExt.FileName
            NameFile2STTS = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubBAPExt.FileName

            SQLString4 = "UPDATE PRCLicenAdmBeginHD SET DokBAP = " + QuotedStr(NameFile2STTS) + _
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
            URL = ResolveUrl("~/Image/DokumenBeginLicense/" + filePath)
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
            filePath = dr.Rows(0)("FilDok_1").ToString


            If File.Exists(Server.MapPath("~/Image/DokumenBeginLicense/" + filePath)) = True Then
                File.Delete(Server.MapPath("~/Image/DokumenBeginLicense/" + filePath))
                SQLExecuteNonQuery("UPDATE PRCLicenAdmBeginHD Set FilDok_1 = '' WHERE TransNmbr = '" + tbCode.Text + "' ", ViewState("DBConnection").ToString)

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
            filePath = dr.Rows(0)("FileDok_2").ToString
            If File.Exists(Server.MapPath("~/Image/DokumenBeginLicense/" + filePath)) = True Then
                File.Delete(Server.MapPath("~/Image/DokumenBeginLicense/" + filePath))
                SQLExecuteNonQuery("UPDATE PRCLicenAdmBeginHD Set FileDok_2 = '' WHERE TransNmbr = '" + tbCode.Text + "' ", ViewState("DBConnection").ToString)

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

            If File.Exists(Server.MapPath("~/Image/DokumenBeginLicense/" + filePath)) = True Then
                File.Delete(Server.MapPath("~/Image/DokumenBeginLicense/" + filePath))
                SQLExecuteNonQuery("UPDATE PRCLicenAdmBeginHD Set DokBAP = '' WHERE TransNmbr = '" + tbCode.Text + "' ", ViewState("DBConnection").ToString)
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
            If File.Exists(Server.MapPath("~/Image/DokumenBeginLicense/" + filePath)) = True Then
                File.Delete(Server.MapPath("~/Image/DokumenBeginLicense/" + filePath))
                SQLExecuteNonQuery("UPDATE PRCLicenAdmBeginHD Set DokLain = '' WHERE TransNmbr = '" + tbCode.Text + "' ", ViewState("DBConnection").ToString)
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
                    NialiBAP = NialiBAP + CFloat(Dr("BeaForex").ToString)
                End If
            Next
            tbOriginalAmount.Text = FormatNumber(NialiBAP, ViewState("DigitHome"))
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


        tbAmountUnallocated.Attributes.Add("OnBlur", "setformathd();")



        Me.tbOriginalAmount.Attributes.Add("ReadOnly", "True")
        'Proteksi agar hanya angka saja yang bisa di input
        tbQty.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbPriceForex.Attributes.Add("OnKeyDown", "return PressNumeric();")

        tbTotalBiaya.Attributes.Add("OnKeyDown", "return PressNumeric();")
  

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
        Return "SELECT * From V_PRCLicenAdmBeginDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function
    Private Function GetStringDt2(ByVal Nmbr As String) As String
        Return "SELECT * From V_PRCLicenAdmBeginDt2 WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function


    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            GridView1.PageIndex = 0
            Session("AdvanceFilter") = ""
            BindData(Session("AdvanceFilter"))
            pnlNav.Visible = True
            'ddlCommand.Visible = True
            'BtnGo.Visible = True
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
                lbStatus.Text = MessageDlg("STILL ON DEVELOPMENT")
                Exit Sub
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
                        Result = ExecSPCommandGo(ActionValue, "S_PPRCLicenAdmBegin", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            'tbSuppCode.Enabled = State
            'tbType.Enabled = False
            'tbTotalBAP.Enabled = State
            'tbppn.Enabled = State
            'tbppnValue.Enabled = State
            'tbpph.Enabled = State
            'tbPphValue.Enabled = State
            'tbTotalAmount.Enabled = State
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
    'Private Sub BindDataDt2(ByVal Nmbr As String)
    '    Try
    '        Dim dt As New DataTable
    '        ViewState("Dt2") = Nothing
    '        dt = SQLExecuteQuery(GetStringDt2(Nmbr), ViewState("DBConnection").ToString).Tables(0)
    '        ViewState("Dt2") = dt
    '        BindGridDt(dt, GridDt2)
    '    Catch ex As Exception
    '        Throw New Exception("Bind Data Dt2 Error : " + ex.ToString)
    '    End Try
    'End Sub
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
            EnableHd(GetCountRecord(ViewState("Dt")) = 0) ' And GetCountRecord(ViewState("Dt2")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub
    'Protected Sub btnCancelDt2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelDt2.Click
    '    Try
    '        MovePanel(pnlEditDt2, pnlDt2)
    '        EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
    '        StatusButtonSave(True)
    '    Catch ex As Exception
    '        lbStatus.Text = "btn Cancel Dt2  Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt.Click

        Try
            If ViewState("StateDt") = "Edit" Then
                Dim Row As DataRow
                Row = ViewState("Dt").Select("ItemNo = " + lbItemNo.Text)(0)
                Row.BeginEdit()
                'Row("CIPAI") = ddlUserType.SelectedValue
                Row("JobName") = tbJobName.Text 'ddlStructureCode.SelectedValue
                Row("Qty") = Val(tbQty.Text)
                Row("UnitCode") = ddlUnit.SelectedValue
                Row("PriceForex") = tbPriceForex.Text
                Row("LocationName") = tbLocationName.Text
                Row("UnitCode") = ddlUnit.SelectedValue
                Row("BeaForex") = tbTotalBiaya.Text
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                'dr("CIPAI") = ddlUserType.SelectedValue
                dr("ItemNo") = CInt(lbItemNo.Text)
                dr("JobName") = tbJobName.Text 'ddlStructureCode.SelectedValue
                dr("Qty") = Val(tbQty.Text)
                dr("UnitCode") = ddlUnit.SelectedValue
                dr("PriceForex") = tbPriceForex.Text
                dr("LocationName") = tbLocationName.Text
                dr("BeaForex") = tbTotalBiaya.Text
                dr("Remark") = tbRemarkDt.Text
                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0) ' And GetCountRecord(ViewState("Dt2")) = 0)
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
    'Protected Sub btnSaveDt2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt2.Click
    '    Try
    '        If CekDt2() = False Then
    '            btnSaveDt2.Focus()
    '            Exit Sub
    '        End If
    '        If ViewState("StateDt2") = "Edit" Then
    '            Dim Row As DataRow
    '            Row = ViewState("Dt2").Select("ItemNo'+|+'BAP_No = " + QuotedStr(lbItemNo.Text + "|" + lblBAPNumber.Text))(0)
    '            Row.BeginEdit()
    '            Row("UraianPekerjaan") = tbUraian.Text
    '            Row("BAP_No") = lblBAPNumber.Text
    '            Row("Luas") = tbLuas.Text
    '            Row("Biaya") = tbBiaya.Text
    '            Row("BAPPersen") = tbBAPPersen.Text
    '            Row("BAP") = tbBAP.Text
    '            Row("BAPSebelumPersen") = tbBAPSebelumPersen.Text
    '            Row("BAPSebelum") = tbBAPSebelum.Text
    '            Row("TagihanBAPPersen") = tbBAPnowPersen.Text
    '            Row("TagihanBAP") = tbBAPnow.Text
    '            Row("SisaBAP") = tbSisaBAP.Text
    '            Row("Remark") = tbRemarkDt2.Text
    '            Row.EndEdit()
    '        Else
    '            'Insert
    '            Dim dr As DataRow
    '            dr = ViewState("Dt2").NewRow
    '            dr("BAP_No") = lblBAPNumber.Text
    '            dr("ItemNo") = CInt(lbItemNo.Text)
    '            dr("UraianPekerjaan") = tbUraian.Text
    '            dr("Luas") = tbLuas.Text
    '            dr("Biaya") = tbBiaya.Text
    '            dr("BAPPersen") = tbBAPPersen.Text
    '            dr("BAP") = tbBAP.Text
    '            dr("BAPSebelumPersen") = tbBAPSebelumPersen.Text
    '            dr("BAPSebelum") = tbBAPSebelum.Text
    '            dr("TagihanBAPPersen") = tbBAPnowPersen.Text
    '            dr("TagihanBAP") = tbBAPnow.Text
    '            dr("SisaBAP") = tbSisaBAP.Text
    '            dr("Remark") = tbRemarkDt2.Text
    '            ViewState("Dt2").Rows.Add(dr)
    '        End If
    '        MovePanel(pnlEditDt2, pnlDt2)
    '        Dim drow As DataRow()
    '        drow = ViewState("Dt2").Select("ItemNo = " + QuotedStr(TrimStr(lbItemNo.Text)))
    '        If drow.Length > 0 Then
    '            BindGridDt(drow.CopyToDataTable, GridDt2)
    '            GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
    '        Else
    '            Dim DtTemp As New DataTable
    '            DtTemp = ViewState("Dt2").Clone
    '            DtTemp.Rows.Add(DtTemp.NewRow()) ' create a new blank row to the DataTable
    '            GridDt2.DataSource = DtTemp
    '            GridDt2.DataBind()
    '            GridDt2.Columns(0).Visible = False
    '        End If

    '        CountTotalDt2()
    '        btnCancelDt2.Visible = True
    '        btnSaveDt2.Visible = True

    '        'EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
    '        'BindGridDt(ViewState("Dt2"), GridDt2)
    '        StatusButtonSave(True)
    '    Catch ex As Exception
    '        lbStatus.Text = "btn save dt2 Error : " + ex.ToString
    '    Finally
    '        If Not con Is Nothing Then con.Dispose()
    '        If Not da Is Nothing Then da.Dispose()
    '    End Try
    'End Sub
    'Private Sub CountTotalDt()
    '    'Dim QtyTotal As Double
    '    'Dim Dr As DataRow
    '    'Dim drow As DataRow()
    '    'Dim havedetail As Boolean
    '    'Try
    '    '    drow = ViewState("Dt2").Select("Bap = " + QuotedStr(TrimStr(lbFADt2.Text)))
    '    '    QtyTotal = 0
    '    '    If drow.Length > 0 Then
    '    '        havedetail = False
    '    '        For Each Dr In drow.CopyToDataTable.Rows
    '    '            If Not Dr.RowState = DataRowState.Deleted Then
    '    '                QtyTotal = QtyTotal + CFloat(Dr("Qty").ToString)
    '    '            End If
    '    '        Next

    '    '    End If
    '    '    Dr = ViewState("Dt").Select("FixedAsset = " + QuotedStr(TrimStr(lbFADt2.Text)))(0)

    '    '    Dr.BeginEdit()
    '    '    Dr("Qty") = QtyTotal 'FormatNumber(QtyTotal, ViewState("DigitQty"))
    '    '    'Dr("Total") = FormatNumber(QtyTotal * price, ViewState("DigitHome"))
    '    '    Dr.EndEdit()
    '    '    BindGridDt(ViewState("Dt"), GridDt)
    '    '    'lbQtyTotal.Text = FormatNumber(QtyTotal, ViewState("DigitQty"))
    '    Dim dr As DataRow
    '    Dim BeaForex As Double
    '    Try

    '        BeaForex = 0
    '        For Each dr In ViewState("Dt").Select("BAP_No = " + QuotedStr(tbBAPNo.Text.Trim))
    '            If Not dr.RowState = DataRowState.Deleted Then
    '                BeaForex = BeaForex + CFloat(dr("BeaForex").ToString)
    '            End If
    '        Next
    '        tbTotalBAP.Text = FormatFloat(BeaForex, ViewState("DigitHome"))
    '        tbppnValue.Text = (CFloat(tbTotalBAP.Text) * CFloat(tbppn.Text)) / 100
    '        tbPphValue.Text = (CFloat(tbTotalBAP.Text) * CFloat(tbpph.Text)) / 100
    '        tbTotalAmount.Text = (CFloat(tbTotalBAP.Text) + CFloat(tbppnValue.Text)) - CFloat(tbPphValue.Text)
    '        AttachScript("setformathd();", Page, Me.GetType())
    '    Catch ex As Exception
    '        Throw New Exception("Count Total Dt Error : " + ex.ToString)
    '    End Try
    'End Sub
    Private Sub SaveAll()
        Dim SQLString As String
        Dim I As Integer
        Try
            If Not (ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit") Then
                Exit Sub
            Else
                ViewState("StateSave") = ViewState("StateHd")
                ViewState("StateHd") = "View"
            End If
            'Save Hd
            If ViewState("StateSave") = "Insert" Then
                tbCode.Text = GetAutoNmbr("LSA", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)


                SQLString = "INSERT INTO PRCLicenAdmBeginHd (TransNmbr,Status,TransDate,TglRegistrasi,KegiatanCode,AreaCode,AlasHak,NoBrks_Permohonan,TglBrks_Permohonan,PIC,Pejabat,NoTelp_Pejabat,Perantara,NoTelp_Perantara,  " + _
                    "DokumenNo_1,DokumenNo_2,CipType,SpsDate,Currency,ForexRate,OriginalAmount,AllocAmount,Remark,UserPrep,DatePrep) " + _
                "SELECT '" + tbCode.Text + "', 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', '" + Format(tbDateRegistrasi.SelectedValue, "yyyy-MM-dd") + "', " + _
                QuotedStr(tbKegiatanCode.Text) + ", " + QuotedStr(tbAreaCode.Text) + "," + QuotedStr(tbAlasHak.Text) + "," + QuotedStr(tbNoPermohonan.Text) + ",'" + Format(tbDatePermohonan.SelectedValue, "yyyy-MM-dd") + "' ," + _
                QuotedStr(tbPIC.Text) + ", " + QuotedStr(tbPejabat.Text) + ",  " + QuotedStr(tbTelpPejabat.Text) + ", " + QuotedStr(tbPerantara.Text) + ",  " + QuotedStr(tbTelpPerantara.Text) + "," + _
                QuotedStr(tbNoDok1.Text) + ", " + QuotedStr(tbNoDok2.Text) + ", " + QuotedStr(ddlCIP.SelectedValue) + ",'" + Format(tbSPSDate.SelectedValue, "yyyy-MM-dd") + "'," + _
                QuotedStr(ddlCurr.Text) + ", " + QuotedStr(tbRate.Text) + ", " + _
                QuotedStr(tbOriginalAmount.Text.Replace(",", "")) + ", " + QuotedStr(tbAmountUnallocated.Text.Replace(",", "")) + "," + _
                QuotedStr(tbRemark.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", getDate()"

            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM PRCLicenAdmBeginHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE PRCLicenAdmBeginHd SET Transdate = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "' " + _
                ", TglRegistrasi = '" + Format(tbDateRegistrasi.SelectedValue, "yyyy-MM-dd") + "' " + _
                ", KegiatanCode = " + QuotedStr(tbKegiatanCode.Text) + _
                ", AreaCode = " + QuotedStr(tbAreaCode.Text) + _
                ", AlasHak = " + QuotedStr(tbAlasHak.Text) + _
                ", NoBrks_Permohonan = " + QuotedStr(tbNoPermohonan.Text) + _
                ", TglBrks_Permohonan = '" + Format(tbDatePermohonan.SelectedValue, "yyyy-MM-dd") + "' " + _
                ", PIC = " + QuotedStr(tbPIC.Text) + _
                ", Currency = " + QuotedStr(ddlCurr.Text) + _
                ", ForexRate = " + QuotedStr(tbRate.Text) + _
                ", Pejabat = " + QuotedStr(tbPejabat.Text) + _
                ", NoTelp_Pejabat = " + QuotedStr(tbTelpPejabat.Text) + _
                ", Perantara = " + QuotedStr(tbPerantara.Text) + _
                ", NoTelp_Perantara = " + QuotedStr(tbTelpPerantara.Text) + _
                ", DokumenNo_1 = " + QuotedStr(tbNoDok1.Text) + _
                ", DokumenNo_2 = " + QuotedStr(tbNoDok2.Text) + _
                ", CipType = " + QuotedStr(ddlCIP.SelectedValue) + _
                ", SpsDate = '" + Format(tbSPSDate.SelectedValue, "yyyy-MM-dd") + "' " + _
                ", OriginalAmount= " + QuotedStr(tbOriginalAmount.Text.Replace(",", "")) + _
                ", AllocAmount= " + QuotedStr(tbAmountUnallocated.Text.Replace(",", "")) + _
                ", Remark = " + QuotedStr(tbRemark.Text) + _
                ", UserPrep = " + QuotedStr(ViewState("UserId").ToString) + _
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

            'Row = ViewState("Dt2").Select("TransNmbr IS NULL")
            'For I = 0 To Row.Length - 1
            '    Row(I).BeginEdit()
            '    Row(I)("TransNmbr") = tbCode.Text
            '    Row(I).EndEdit()
            'Next

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString 'System.Configuration.ConfigurationManager.AppSettings.Get("ConString")
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand(" SELECT TransNmbr,ItemNo,JobCode,JobName,Qty,UnitCode,PriceForex,LocationName,BeaForex,Remark FROM PRCLicenAdmBeginDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand


            Dim Dt As New DataTable("PRCLicenAdmBeginDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt

            ''save dt2
            'cmdSql = New SqlCommand("SELECT TransNmbr, ItemNo, BAP_No, UraianPekerjaan, Luas, Biaya,BAPPersen,BAP,BAPSebelumPersen,BAPSebelum,TagihanBAPPersen,TagihanBAP, SisaBAP,  Remark FROM PRCLicenAdmBeginDt2 WHERE TransNmbr  = " + QuotedStr(ViewState("TransNmbr")), con)
            'da = New SqlDataAdapter(cmdSql)
            'dbcommandBuilder = New SqlCommandBuilder(da)
            'da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            'Dim Dt2 As New DataTable("PRCLicenAdmBeginDt2")

            'Dt2 = ViewState("Dt2")
            'da.Update(Dt2)
            'Dt2.AcceptChanges()
            'ViewState("Dt2") = Dt2
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

                ''If GetCountRecord(ViewState("Dt2")) = 0 Then
                ''    lbStatus.Text = MessageDlg("Detail Payment must have at least 1 record")
                ''    Exit Sub
                ''End If

                For Each dr In ViewState("Dt").Rows
                    If CekDt(dr) = False Then
                        Exit Sub
                    End If
                Next

                SaveAll()
                ModifyInput2(False, pnlInput, pnlDt, GridDt)
                ' ModifyInput2(False, pnlInput, pnlDt2, GridDt2)
                btnGoEdit.Visible = True
                Menu2.Items.Item(1).Enabled = True
                MultiView2.ActiveViewIndex = 1
                Menu2.Items.Item(1).Selected = True
                'btnGoEdit.Visible = True
                'btnGetBAP.Visible = False
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
                'If GetCountRecord(ViewState("Dt2")) = 0 Then
                '    lbStatus.Text = MessageDlg("Detail Payment must have at least 1 record")
                '    Exit Sub
                'End If
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
            btnAddDt.Visible = True
            btnAddDtKe2.Visible = True
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

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ViewState("DigitCurr") = ViewState("DigitHome")
            ViewState("TransNmbr") = ""
            ClearHd()
            Cleardt()
            'Cleardt2()
            ViewState("DigitCurr") = 2
            pnlDt.Visible = True
            BindDataDt("")
            'BindDataDt2("")
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbCode.Text = ""
            tbDate.SelectedDate = ViewState("ServerDate") 'Today
            tbDatePermohonan.SelectedDate = ViewState("ServerDate")
            tbDateRegistrasi.SelectedDate = ViewState("ServerDate") 'Today
            tbKegiatanCode.Text = ""
            tbKegiatanName.Text = ""
            tbAreaCode.Text = ""
            tbAreaName.Text = ""
            tbRemark.Text = ""
            MultiView1.ActiveViewIndex = 0
            tbAmountUnallocated.Text = "0"
            tbOriginalAmount.Text = "0"
          
            tbSPSDate.SelectedDate = ViewState("ServerDate") 'Today
            tbNoDok1.Text = ""
            tbNoDok2.Text = ""
            tbAlasHak.Text = ""
            tbPerantara.Text = ""
            tbNoDok2.Text = ""
            tbTelpPerantara.Text = ""
            tbPejabat.Text = ""
            tbTelpPejabat.Text = ""
            ddlCurr.SelectedValue = ViewState("Currency")

            ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try

            tbJobName.Text = ""
            tbQty.Text = "0"
            ddlUnit.SelectedIndex = 0
            tbPriceForex.Text = "0"
            tbLocationName.Text = ""
            tbTotalBiaya.Text = "0"
            tbRemarkDt.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub
    'Private Sub Cleardt2()
    '    Try
    '        tbUraian.Text = ""
    '        tbLuas.Text = 0
    '        tbBiaya.Text = 0
    '        tbBAPPersen.Text = 0
    '        tbBAP.Text = 0
    '        tbBAPSebelumPersen.Text = 0
    '        tbBAPSebelum.Text = 0
    '        tbBAPnowPersen.Text = 0
    '        tbBAPnow.Text = 0
    '        tbSisaBAP.Text = 0
    '        tbRemarkDt2.Text = ""
    '    Catch ex As Exception
    '        Throw New Exception("Clear Dt 2 Error " + ex.ToString)
    '    End Try
    'End Sub

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

    Protected Sub ddlShowRecord_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlShowRecord.SelectedIndexChanged
        GridView1.PageIndex = 0
        GridView1.EditIndex = -1
        GridView1.PageSize = ddlShowRecord.SelectedValue
        BindData()
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
            'If GetCountRecord(ViewState("Dt2")) = 0 Then
            '    lbStatus.Text = MessageDlg("Detail Payment must have at least 1 record")
            '    Exit Sub
            'End If
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
        lbItemNo.Text = GetNewItemNo(ViewState("Dt"))
        MovePanel(pnlDt, pnlEditDt)
        EnableHd(False)
        StatusButtonSave(False)
    End Sub

    'Public Function GetNewItemNo()
    '    Dim Row As DataRow()
    '    Dim R As DataRow
    '    Dim MaxItem As Integer = 0
    '    Row = ViewState("Dt2").Select("BAPNo = " + QuotedStr(TrimStr(lblBAPNumber.Text)))

    '    For Each R In Row
    '        If CInt(R("ItemNo").ToString) > MaxItem Then
    '            MaxItem = CInt(R("ItemNo").ToString)
    '        End If
    '    Next
    '    MaxItem = MaxItem + 1
    '    Return CStr(MaxItem)
    'End Function


    'Protected Sub btnAddDt2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt2Ke2.Click, btnAddDt2.Click
    '    Try
    '        Cleardt2()
    '        If CekHd() = False Then
    '            Exit Sub
    '        End If
    '        ViewState("StateDt2") = "Insert"
    '        MovePanel(pnlDt2, pnlEditDt2)
    '        EnableHd(False)
    '        StatusButtonSave(False)
    '        lbItemNo.Text = GetNewItemNo()
    '    Catch ex As Exception
    '        lbStatus.Text = "btn add dt error : " + ex.ToString
    '    End Try
    'End Sub
    Function CekHd() As Boolean
        Try

            If tbDate.IsNull Then
                lbStatus.Text = MessageDlg("Date must have value")
                tbDate.Focus()
                Return False
            End If
            If tbKegiatanCode.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Kagiatan must have value")
                tbKegiatanCode.Focus()
                Return False
            End If

            If tbAreaCode.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Area must have value")
                tbAreaCode.Focus()
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
                'If Dr("BAP_No").ToString.Trim = "" Then
                '    lbStatus.Text = "Process Must Have Value"
                '    Return False
                'End If


                'If Dr("BeaForex").ToString = 0 Or Dr("BeaForex").ToString = "" Then
                '    lbStatus.Text = "Qty Output Must Have Value"
                '    Return False
                'End If



            Else
                If tbJobName.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Job Name Must Have Value")
                    tbJobName.Focus()
                    Return False
                End If
                If CFloat(tbQty.Text) <= "0" Then
                    lbStatus.Text = MessageDlg("Qty Must Have Value")
                    tbQty.Focus()
                    Return False
                End If
                If ddlUnit.SelectedValue.Trim = "" Then
                    lbStatus.Text = MessageDlg("Satuan Must Have Value")
                    ddlUnit.Focus()
                    Return False
                End If
                If tbLocationName.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Location Name Must Have Value")
                    tbLocationName.Focus()
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function
    'Function CekDt2(Optional ByVal Dr As DataRow = Nothing) As Boolean
    '    Try
    '        If Not Dr Is Nothing Then
    '            If Dr.RowState = DataRowState.Deleted Then
    '                Return True
    '            End If

    '            If Dr("TagihanBAPPersen").ToString = "0" Or Dr("TagihanBAPPersen").ToString = "" Then
    '                lbStatus.Text = MessageDlg(" BAP %  Must Have Value")
    '                Return False
    '            End If


    '            If Dr("TagihanBAP").ToString = "0" Or Dr("TagihanBAP").ToString = "" Then
    '                lbStatus.Text = MessageDlg("BAP Saat Ini Must Have Value")
    '                Return False
    '            End If

    '            If Dr("Biaya").ToString = "0" Or Dr("Biaya").ToString = "" Then
    '                lbStatus.Text = MessageDlg("Biaya Must Have Value")
    '                Return False
    '            End If

    '        Else

    '            If tbBAPnowPersen.Text = "" Or tbBAPnowPersen.Text = "0" Then
    '                lbStatus.Text = MessageDlg("BAP % Must Have Value")
    '                tbLuas.Focus()
    '                Return False
    '            End If

    '            If tbBAPnow.Text = "" Or tbBAPnow.Text = "0" Then
    '                lbStatus.Text = MessageDlg("BAP Saat Ini Must Have Value")
    '                tbBAPnow.Focus()
    '                Return False
    '            End If

    '            If tbLuas.Text = "" Or tbLuas.Text = "0" Then
    '                lbStatus.Text = MessageDlg("Luas Must Have Value")
    '                tbLuas.Focus()
    '                Return False
    '            End If

    '            If tbBiaya.Text = "" Or tbBiaya.Text = "0" Then
    '                lbStatus.Text = MessageDlg("Biaya Must Have Value")
    '                tbBiaya.Focus()
    '                Return False
    '            End If

    '        End If
    '        Return True
    '    Catch ex As Exception
    '        Throw New Exception("Cek Dt Error : " + ex.ToString)
    '    End Try
    'End Function
    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        CheckAll(GridView1, sender)
    End Sub

    Protected Sub LbAdvSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LbAdvSearch.Click
        Dim FDateName, FDateValue, FilterName, FilterValue As String
        Try
            FDateName = "Date, Invoice Date"
            FDateValue = "TransDate, SuppInvDate"
            FilterName = "Reference, Date, Status, Kagiatan Code, Area Name,  Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), Status, KegiatanCode, AreaName, Remark"
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
                    'BindDataDt2(ViewState("TransNmbr"))
                    EnableHd(False)
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, pnlDt, GridDt)
                    'ModifyInput2(False, pnlInput, pnlDt2, GridDt2)
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
                        'BindDataDt2(ViewState("TransNmbr"))

                        ViewState("StateHd") = "Edit"

                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        FillTextBoxHd(ViewState("TransNmbr"))
                        'ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
                        btnHome.Visible = False
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0) 'And GetCountRecord(ViewState("Dt2")) = 0)
                        MultiView1.ActiveViewIndex = 0
                        Menu1.Items.Item(0).Selected = True
                        EnableHd(True)
                        'btnAddDt.Visible = False
                        'btnAddDtKe2.Visible = False


                    Else
                        lbStatus.Text = MessageDlg("Data must Hold or Get Approval to edit")
                        Exit Sub
                    End If

                ElseIf DDL.SelectedValue = "Print" Then
                    lbStatus.Text = MessageDlg("STILL ON DEVELOPMENT")
                    Exit Sub
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

    'Protected Sub GridDt_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridDt.RowCommand
    '    Try
    '        If e.CommandName = "Insert" Then
    '            btnAddDt_Click(Nothing, Nothing)
    '        End If
    '        If e.CommandName = "View" Then
    '            Dim GVR As GridViewRow
    '            GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))

    '            If GVR.Cells(2).Text = "&nbsp;" Then
    '                Exit Sub
    '            End If
    '            Dim lbFA As Label

    '            lbFA = GVR.FindControl("lbFa")

    '            lblBAPNumber.Text = GVR.Cells(2).Text
    '            lblSPKNumber.Text = GVR.Cells(3).Text

    '            MultiView1.ActiveViewIndex = 1

    '            If ViewState("StateHd") = "View" Then
    '                ModifyInput2(False, pnlInput, pnlDt2, GridDt2)
    '            Else
    '                ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
    '            End If
    '            GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
    '            If ViewState("Dt2") Is Nothing Then
    '                BindDataDt2(ViewState("TransNmbr"))
    '            End If
    '            Dim drow As DataRow()
    '            drow = ViewState("Dt2").Select("BAP_No = " + QuotedStr(TrimStr(lblBAPNumber.Text))) '+ " AND FAName = " + QuotedStr(TrimStr(lbFANameDt2.Text)) + " AND FAStatus = " + QuotedStr(TrimStr(lbStatusFA.Text)))
    '            '                drow = ViewState("Dt2").Select("FixedAsset = " + QuotedStr(TrimStr(lbFADt2.Text)))
    '            If drow.Length > 0 Then
    '                BindGridDt(drow.CopyToDataTable, GridDt2)
    '                GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
    '                GridDt2.Columns(0).Visible = False
    '                btnAddDt2.Visible = False
    '                btnAddDt2Ke2.Visible = False
    '            Else
    '                Dim DtTemp As DataTable
    '                DtTemp = ViewState("Dt2").Clone
    '                DtTemp.Rows.Add(DtTemp.NewRow())
    '                GridDt2.DataSource = DtTemp
    '                GridDt2.DataBind()
    '                GridDt2.Columns(0).Visible = False
    '            End If

    '        End If
    '    Catch ex As Exception
    '        lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow
            'If GetCountRecord(ViewState("Dt2")) <> 0 Then
            '    lbStatus.Text = " Data Detail exist"
            '    Exit Sub
            'Else
            GVR = GridDt.Rows(e.RowIndex)
            dr = ViewState("Dt").Select("ItemNo = " + QuotedStr(GVR.Cells(1).Text))

            dr(0).Delete()
            BindGridDt(ViewState("Dt"), GridDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0) ' And GetCountRecord(ViewState("Dt2")) = 0)
            'End If

        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Dim TotalQty As Decimal = 0

    'Protected Sub GridDt2_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt2.RowDeleting
    '    Try
    '        Dim dr() As DataRow
    '        Dim GVR As GridViewRow
    '        GVR = GridDt2.Rows(e.RowIndex)
    '        dr = ViewState("Dt2").Select("ItemNo = " + QuotedStr(GVR.Cells(1).Text))
    '        dr(0).Delete()

    '        Dim drow As DataRow()
    '        drow = ViewState("Dt2").Select("BAP_No = " + QuotedStr(TrimStr(lblBAPNumber.Text)))
    '        If drow.Length > 0 Then
    '            BindGridDt(drow.CopyToDataTable, GridDt2)
    '            GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
    '        Else
    '            Dim DtTemp As New DataTable
    '            DtTemp = ViewState("Dt2").Clone
    '            DtTemp.Rows.Add(DtTemp.NewRow()) ' create a new blank row to the DataTable
    '            GridDt2.DataSource = DtTemp
    '            GridDt2.DataBind()
    '            GridDt2.Columns(0).Visible = False
    '        End If

    '        'CountTotalDt()

    '        'BindGridDt(ViewState("Dt2"), GridDt2)
    '        'EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
    '    Catch ex As Exception
    '        lbStatus.Text = "Grid Dt 2 Row Deleting Error : " + ex.ToString
    '    End Try
    'End Sub
    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            ViewState("DtValue") = GVR.Cells(1).Text
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


    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            tbCode.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToText(tbKegiatanCode, Dt.Rows(0)("KegiatanCode").ToString)
            BindToText(tbKegiatanName, Dt.Rows(0)("KegiatanName").ToString)
            BindToDropList(ddlCIP, Dt.Rows(0)("CipType").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
            BindToText(tbAreaCode, Dt.Rows(0)("AreaCode").ToString)
            BindToText(tbAreaName, Dt.Rows(0)("AreaName").ToString)
            BindToText(tbOriginalAmount, Dt.Rows(0)("OriginalAmount").ToString, ViewState("DigitCurr"))
            BindToText(tbAmountUnallocated, Dt.Rows(0)("AllocAmount").ToString, ViewState("DigitCurr"))
            BindToText(tbNoPermohonan, Dt.Rows(0)("NoBrks_Permohonan").ToString)
            BindToDate(tbDatePermohonan, Dt.Rows(0)("TglBrks_Permohonan").ToString)
            BindToText(tbPejabat, Dt.Rows(0)("Pejabat").ToString)
            BindToText(tbPIC, Dt.Rows(0)("PIC").ToString)
            BindToDate(tbDateRegistrasi, Dt.Rows(0)("TglRegistrasi").ToString)
            BindToText(tbRate, Dt.Rows(0)("ForexRate").ToString, ViewState("DigitCurr"))
            BindToDropList(ddlCurr, Dt.Rows(0)("Currency").ToString)
            BindToText(tbTelpPejabat, Dt.Rows(0)("NoTelp_Pejabat").ToString)
            BindToText(tbPerantara, Dt.Rows(0)("Perantara").ToString)
            BindToText(tbTelpPerantara, Dt.Rows(0)("NoTelp_Perantara").ToString)
            BindToText(tbAlasHak, Dt.Rows(0)("AlasHak").ToString)
            BindToText(tbNoDok1, Dt.Rows(0)("DokumenNo_1").ToString)
            BindToText(tbNoDok1, Dt.Rows(0)("DokumenNo_1").ToString)
            BindToDate(tbSPSDate, Dt.Rows(0)("SpsDate").ToString)
            '1
            If Dt.Rows(0)("FilDok_1").ToString = "" Then
                'cbKtp.Checked = False
                lbDokInv.Text = "Not Yet Uploaded"
            Else
                lbDokInv.Text = Dt.Rows(0)("FilDok_1").ToString
                'cbKtp.Checked = True
            End If

            '2
            If Dt.Rows(0)("FileDok_2").ToString = "" Then
                lbFaktur.Text = "Not Yet Uploaded"
            Else
                lbFaktur.Text = Dt.Rows(0)("FileDok_2").ToString
            End If

            ''3
            'If Dt.Rows(0)("DokBAP").ToString = "" Then
            '    lbBAP.Text = "Not Yet Uploaded"
            'Else
            '    lbBAP.Text = Dt.Rows(0)("DokBAP").ToString

            'End If

            ''4
            'If Dt.Rows(0)("DokLain").ToString = "" Then

            '    lbDokLain.Text = "Not Yet Uploaded"
            'Else
            '    lbDokLain.Text = Dt.Rows(0)("DokLain").ToString

            'End If
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("ItemNo = " + ItemNo)
            If Dr.Length > 0 Then
                lbItemNo.Text = ItemNo.ToString
                BindToText(tbJobName, Dr(0)("JobName").ToString)
                BindToText(tbQty, Dr(0)("Qty").ToString, ViewState("DigitCurr"))
                BindToDropList(ddlUnit, Dr(0)("UnitCode").ToString)
                BindToText(tbLocationName, Dr(0)("LocationName").ToString)
                BindToText(tbPriceForex, Dr(0)("PriceForex").ToString, ViewState("DigitCurr"))
                BindToText(tbTotalBiaya, Dr(0)("BeaForex").ToString, ViewState("DigitCurr"))
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
                
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub

    'Protected Sub FillTextBoxDt2(ByVal FALoc As String)
    '    Dim Dr As DataRow()
    '    Try
    '        Dr = ViewState("Dt2").select("ItemNo+'|'+ BAP_No= " + QuotedStr(FALoc))
    '        If Dr.Length > 0 Then

    '            lbItemNo.Text = FALoc.ToString
    '            BindToText(tbUraian, Dr(0)("UraianPekerjaan").ToString)
    '            BindToText(tbLuas, Dr(0)("Luas").ToString, ViewState("DigitHome"))
    '            BindToText(tbBiaya, Dr(0)("Biaya").ToString, ViewState("DigitHome"))
    '            BindToText(tbBAPPersen, Dr(0)("BAPPersen").ToString, ViewState("DigitHome"))
    '            BindToText(tbBAP, Dr(0)("BAP").ToString, ViewState("DigitHome"))
    '            BindToText(tbBAPSebelumPersen, Dr(0)("BAPSebelumPersen").ToString, ViewState("DigitHome"))
    '            BindToText(tbBAPSebelum, Dr(0)("BAPSebelum").ToString, ViewState("DigitHome"))
    '            BindToText(tbBAPnowPersen, Dr(0)("TagihanBAPPersen").ToString, ViewState("DigitHome"))
    '            BindToText(tbBAPnow, Dr(0)("TagihanBAP").ToString, ViewState("DigitHome"))
    '            BindToText(tbSisaBAP, Dr(0)("SisaBAP").ToString, ViewState("DigitHome"))
    '            BindToText(tbRemarkdt, Dr(0)("Remark").ToString)
    '        End If
    '    Catch ex As Exception
    '        Throw New Exception("fill text box detail 2 error : " + ex.ToString)
    '    End Try
    'End Sub

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


    'Protected Sub GridDt2_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt2.RowEditing
    '    Dim GVR As GridViewRow
    '    Try
    '        GVR = GridDt2.Rows(e.NewEditIndex)
    '        FillTextBoxDt2(GVR.Cells(1).Text + "|" + lblBAPNumber.Text)
    '        MovePanel(pnlDt2, pnlEditDt2)
    '        EnableHd(False)
    '        ViewState("StateDt2") = "Edit"
    '        StatusButtonSave(False)
    '        btnSaveDt2.Focus()
    '    Catch ex As Exception
    '        lbStatus.Text = "Grid dt2 Editing Error : " + ex.ToString
    '    End Try
    'End Sub
    'Protected Sub btnBackDt2ke1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBackDt2ke1.Click, btnBackDt2ke2.Click
    '    Try
    '        MultiView1.ActiveViewIndex = 0
    '    Catch ex As Exception
    '        lbStatus.Text = "btn back Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub btnKegiatan_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnKegiatan.Click
        Dim ResultField As String
        Try
            Session("filter") = "select KegiatanCode, KegiatanName from MsKegiatan"
            ResultField = "KegiatanCode, KegiatanName"
            ViewState("Sender") = "btnKegiatan"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchGrid();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Supp Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnArea_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnArea.Click
        Dim ResultField As String
        Try
            Session("filter") = "select AreaCode, AreaName from MsArea"
            ResultField = "AreaCode, AreaName"
            ViewState("Sender") = "btnArea"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchGrid();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Supp Error : " + ex.ToString
        End Try
    End Sub



  


    'Protected Sub tbSuppCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbSuppCode.TextChanged
    '    Dim Dr As DataRow
    '    Try
    '        Dr = FindMaster("Supplier", tbSuppCode.Text, ViewState("DBConnection").ToString)
    '        If Not Dr Is Nothing Then
    '            tbSuppCode.Text = Dr("Supplier_Code")
    '            tbSuppName.Text = Dr("Supplier_Name")
    '            BindToDropList(ddlCurr, Dr("Currency"))
    '            BindToDropList(ddlTerm, Dr("Term"))
    '            tbDueDate.SelectedDate = FindDueDate(ddlTerm.SelectedValue, tbDate.SelectedDate, ViewState("DBConnection").ToString)
    '        Else
    '            tbSuppCode.Text = ""
    '            tbSuppName.Text = ""
    '            ddlCurr.SelectedValue = Session("Currency")
    '            tbppn.Text = ViewState("PPn")
    '        End If
    '        ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
    '        'AttachScript("setformat();", Page, Me.GetType())
    '        tbSuppCode.Focus()
    '    Catch ex As Exception
    '        Throw New Exception("tb SuppCode Error : " + ex.ToString)
    '    End Try
    'End Sub




    Protected Sub ddlCurr_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCurr.SelectedIndexChanged
        If ViewState("InputCurrency") = "Y" Then
            RefreshMaster("S_GetCurrency", "Currency", "Currency", ddlCurr, ViewState("DBConnection"))
            ViewState("InputCurrency") = Nothing
        End If
        ChangeCurrency(ddlCurr, tbDate, tbRate, Session("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))

    End Sub


End Class
