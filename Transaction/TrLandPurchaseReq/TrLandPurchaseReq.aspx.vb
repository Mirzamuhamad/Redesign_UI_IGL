Imports System.Data
Imports System.Data.SqlClient
Imports System.Runtime.Serialization
Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Collections.Generic
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.IO
Partial Class Transaction_TrLandPurchaseReq_TrLandPurchaseReq
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "SELECT * FROM V_GLLandPurchaseReqHd"


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If

        Try
            If Not IsPostBack Then
                InitProperty()
                SetInit()

                tbSPPT.Attributes.Add("OnKeyDown", "return PressNumeric();")
                tbAjbSphShm.Attributes.Add("OnKeyDown", "return PressNumeric();")
                tbLuasUkur.Attributes.Add("OnKeyDown", "return PressNumeric();")
                tbNilai.Attributes.Add("OnKeyDown", "return PressNumeric();")
                tbTotal.Attributes.Add("OnKeyDown", "return PressNumeric();")
             
            End If

            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If
            If Not Session("Result") Is Nothing Then


                If ViewState("Sender") = "btnSeller" Then
                    BindToText(tbSeller, Session("Result")(0).ToString)
                    BindToText(tbSellerName, Session("Result")(1).ToString)
                End If

                If ViewState("Sender") = "btnModerator" Then
                    BindToText(tbModerator, Session("Result")(0).ToString)
                    BindToText(tbModeratorName, Session("Result")(1).ToString)
                End If

                If ViewState("Sender") = "btnArea" Then
                    BindToText(tbArea, Session("Result")(0).ToString)
                    BindToText(tbAreaName, Session("Result")(1).ToString)
                End If

                Session("filter") = Nothing
                Session("Column") = Nothing
                Session("Result") = Nothing

            End If
            'dsUseRollNo.ConnectionString = ViewState("DBConnection")



            FupMain.Attributes("onchange") = "UploadKtp(this)"

            FubKK.Attributes("onchange") = "UploadKK(this)"

            FubSPPT.Attributes("onchange") = "UploadSPPT(this)"

            FubSTTS.Attributes("onchange") = "UploadSTTS(this)"

            FubTTD.Attributes("onchange") = "UploadTTD(this)"

            FubAJB.Attributes("onchange") = "UploadAJB(this)"

            FubAJB2.Attributes("onchange") = "UploadAJB2(this)"

            FubSSP.Attributes("onchange") = "UploadSSP(this)"

            FubSSD.Attributes("onchange") = "UploadSSD(this)"

            FubSKTS.Attributes("onchange") = "UploadSKTS(this)"

            FubSPBT.Attributes("onchange") = "UploadSPBT(this)"

            FubSPKTT.Attributes("onchange") = "UploadSPKTT(this)"

            FubBAM.Attributes("onchange") = "UploadBAM(this)"

            FubBAPL.Attributes("onchange") = "UploadBAPL(this)"

            FubPTPP.Attributes("onchange") = "UploadPTPP(this)"

            FubPTPP2.Attributes("onchange") = "UploadPTPP2(this)"

            FubSKRT.Attributes("onchange") = "UploadSKRT(this)"

            FubSKD.Attributes("onchange") = "UploadSKD(this)"
            FubFCGirik.Attributes("onchange") = "UploadFcGirik(this)"
            FubFDP.Attributes("onchange") = "UploadFDP(this)"
            FubPatok.Attributes("onchange") = "UploadPatok(this)"
            FubSporadik.Attributes("onchange") = "UploadSporadik(this)"
            FubAHU.Attributes("onchange") = "UploadAHU(this)"
            FubSejarah.Attributes("onchange") = "UploadSejarah(this)"
            FubSPJH.Attributes("onchange") = "UploadSPJH(this)"
            FubLainLain.Attributes("onchange") = "UploadLainLain(this)"
            FupKTPW.Attributes("onchange") = "UploadKTPW(this)"
            FubSPW.Attributes("onchange") = "UploadSPW(this)"
            FubSPKW.Attributes("onchange") = "UploadSPKW(this)"
            FubBPJS.Attributes("onchange") = "UploadBPJS(this)"

            FubLainLain2.Attributes("onchange") = "UploadLainLain2(this)"
            FubLainLain3.Attributes("onchange") = "UploadLainLain3(this)"
            'If Not (ViewState("StateHd") <> "Insert" Or ViewState("StateHd") <> "Edit") Then

            If HiddenRemarkReject.Value = "" Then
                lbStatus.Text = ""
                Exit Sub
            End If

            If Not ViewState("TransNmbr") Is Nothing Then
                If HiddenRemarkReject.Value <> "False Value" Then
                    lbStatus.Text = ViewState("TransNmbr").ToString
                    Dim sqlstring, result, Value As String
                    'SP Reject Taro di sini'
                    'sqlstring = "Update GLLandPurchaseReqHd Set Status = 'R', remarkReject = " + QuotedStr(HiddenRemarkReject.Value) + " WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr").ToString) + "  "
                    'SQLExecuteNonQuery(sqlstring, ViewState("DBConnection").ToString)
                    sqlstring = "Declare @A VarChar(255) EXEC S_PLGLLandPurchaseReqReject " + QuotedStr(ViewState("TransNmbr").ToString) + "," + CInt(Session(Request.QueryString("KeyId"))("Year")).ToString + ", " + CInt(Session(Request.QueryString("KeyId"))("Period")).ToString + "," + QuotedStr(HiddenRemarkReject.Value) + "," + QuotedStr(ViewState("UserId")) + ", @A OUT SELECT @A"
                    result = SQLExecuteScalar(sqlstring, ViewState("DBConnection"))
                    result = result.Replace("0", "")

                    If Trim(result) <> "" Then
                        lbStatus.Text = MessageDlg(result)
                    End If
                    Value = ddlField.SelectedValue
                    tbFilter.Text = ViewState("TransNmbr").ToString
                    ddlField.SelectedValue = "TransNmbr"
                    btnSearch_Click(Nothing, Nothing)
                    ddlField.SelectedValue = Value
                    tbFilter.Text = ""
                End If
                ViewState("TransNmbr") = Nothing
                HiddenRemarkReject.Value = "False Value"
            End If
            'End If
            lbStatus.Text = ""
           
        Catch ex As Exception
            lbStatus.Text = "Page Load Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSeller_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSeller.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT SellCode, SellName, Gender, TypeID FROM V_MsSeller "
            ResultField = "SellCode, SellName, Gender, TypeID"
            ViewState("Sender") = "btnSeller"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("myPopup();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Cust Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnModerator_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnModerator.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT ModCode, ModName, Gender, TypeID FROM V_MsModerator "
            ResultField = "ModCode, ModName, Gender, TypeID"
            ViewState("Sender") = "btnModerator"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("myPopup();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Cust Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnArea_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnArea.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT AreaCode, AreaName FROM V_MsArea "
            ResultField = "AreaCode, AreaName"
            ViewState("Sender") = "btnArea"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("myPopup();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Cust Error : " + ex.ToString
        End Try
    End Sub




    Protected Sub btnKtp_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnKtp.Click
        Try

            Dim dr As DataTable
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("save this transaction first, to upload the dokumen")
                Exit Sub
            End If

            If FupMain.FileBytes.Length > 3500000 Then
                lbStatus.Text = MessageDlg("Ukuran File Terlalu Besar. !! Max Upload 3.5Mb")
                Exit Sub
            End If

            If Right(FupMain.FileName, 4) <> ".pdf" Then
                lbStatus.Text = MessageDlg("Upload Pdf File Only !")
                Exit Sub
            End If
            Dim path2, namafile2, SQLString1 As String
            Dim dt As DataTable
            path2 = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FupMain.FileName
            namafile2 = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FupMain.FileName

            SQLString1 = "UPDATE GLLandPurchaseReqHd SET DocKTP = " + QuotedStr(namafile2) + _
            " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            FupMain.SaveAs(path2)
            SQLExecuteNonQuery(SQLString1, ViewState("DBConnection").ToString)

            dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            lbKtp.Text = dt.Rows(0)("DocKTP").ToString
            lblmassageKTP.Visible = True
            FupMain.Visible = False

        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnsaveKK_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsaveKK.Click

        Try

            Dim dr As DataTable
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("save this transaction first, to upload the dokumen")
                Exit Sub
            End If

            If FubKK.FileBytes.Length > 3500000 Then
                lbStatus.Text = MessageDlg("Ukuran File Terlalu Besar. !! Max Upload 3.5Mb")
                Exit Sub
            End If

            If Right(FubKK.FileName, 4) <> ".pdf" Then
                lbStatus.Text = MessageDlg("Upload Pdf File Only !")
                Exit Sub
            End If

            Dim Path2KK, NameFile2KK, SQLString2 As String
            Dim dt As DataTable

            Path2KK = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubKK.FileName
            NameFile2KK = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubKK.FileName
            SQLString2 = "UPDATE GLLandPurchaseReqHd SET DocKK = " + QuotedStr(NameFile2KK) + _
            " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            FubKK.SaveAs(Path2KK)
            SQLExecuteNonQuery(SQLString2, ViewState("DBConnection").ToString)


            dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            lbKK.Text = dt.Rows(0)("DocKK").ToString
            lblmassageKK.Visible = True
            FubKK.Visible = False

        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnsaveSPPT_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsaveSPPT.Click

        Try

            Dim dr As DataTable
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("save this transaction first, to upload the dokumen")
                Exit Sub
            End If

            If FubSPPT.FileBytes.Length > 3500000 Then
                lbStatus.Text = MessageDlg("Ukuran File Terlalu Besar. !! Max Upload 3.5Mb")
                Exit Sub
            End If

            If Right(FubSPPT.FileName, 4) <> ".pdf" Then
                lbStatus.Text = MessageDlg("Upload Pdf File Only !")
                Exit Sub
            End If

            Dim dt As DataTable
            Dim Path2SPPT, NameFile2SPPT, SQLString3 As String
            Path2SPPT = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubSPPT.FileName
            NameFile2SPPT = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubSPPT.FileName

            SQLString3 = "UPDATE GLLandPurchaseReqHd SET DocSPPT = " + QuotedStr(NameFile2SPPT) + _
            " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            FubSPPT.SaveAs(Path2SPPT)
            SQLExecuteNonQuery(SQLString3, ViewState("DBConnection").ToString)


            dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            lbSPPT.Text = dt.Rows(0)("DocSPPT").ToString
            lblmassageSPPT.Visible = True
            FubSPPT.Visible = False

        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnsaveSTTS_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsaveSTTS.Click

        Try
            Dim dr As DataTable
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("save this transaction first, to upload the dokumen")
                Exit Sub
            End If

            If FubSTTS.FileBytes.Length > 3500000 Then
                lbStatus.Text = MessageDlg("Ukuran File Terlalu Besar. !! Max Upload 3.5Mb")
                Exit Sub
            End If

            If Right(FubSTTS.FileName, 4) <> ".pdf" Then
                lbStatus.Text = MessageDlg("Upload Pdf File Only !")
                Exit Sub
            End If

            Dim dt As DataTable
            Dim Path2STTS, NameFile2STTS, SQLString4 As String
            Path2STTS = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubSTTS.FileName
            NameFile2STTS = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubSTTS.FileName

            SQLString4 = "UPDATE GLLandPurchaseReqHd SET DocSTTS = " + QuotedStr(NameFile2STTS) + _
            " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            FubSTTS.SaveAs(Path2STTS)
            SQLExecuteNonQuery(SQLString4, ViewState("DBConnection").ToString)


            dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            lbSTTS.Text = dt.Rows(0)("DocSTTS").ToString
            lblmassageSTTS.Visible = True
            FubSTTS.Visible = False

        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnsaveTTD_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsaveTTD.Click

        Try
            Dim dr As DataTable
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("save this transaction first, to upload the dokumen")
                Exit Sub
            End If

            If FubTTD.FileBytes.Length > 3500000 Then
                lbStatus.Text = MessageDlg("Ukuran File Terlalu Besar. !! Max Upload 3.5Mb")
                Exit Sub
            End If

            If Right(FubTTD.FileName, 4) <> ".pdf" Then
                lbStatus.Text = MessageDlg("Upload Pdf File Only !")
                Exit Sub
            End If

            Dim dt As DataTable
            Dim Path2TTD, NameFile2TTD, SQLString5 As String
            Path2TTD = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubTTD.FileName
            NameFile2TTD = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubTTD.FileName

            SQLString5 = "UPDATE GLLandPurchaseReqHd SET DocTTD = " + QuotedStr(NameFile2TTD) + _
            " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            FubTTD.SaveAs(Path2TTD)
            SQLExecuteNonQuery(SQLString5, ViewState("DBConnection").ToString)


            dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            lbTTD.Text = dt.Rows(0)("DocTTD").ToString
            lblmassageTTD.Visible = True
            FubTTD.Visible = False

        Catch ex As Exception
            lbStatus.Text = "btnsaveTTD_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnsaveAJB_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsaveAJB.Click

        Try
            Dim dr As DataTable
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("save this transaction first, to upload the dokumen")
                Exit Sub
            End If

            If FubAJB.FileBytes.Length > 3500000 Then
                lbStatus.Text = MessageDlg("Ukuran File Terlalu Besar. !! Max Upload 3.5Mb")
                Exit Sub
            End If

            If Right(FubAJB.FileName, 4) <> ".pdf" Then
                lbStatus.Text = MessageDlg("Upload Pdf File Only !")
                Exit Sub
            End If

            Dim dt As DataTable
            Dim Path2AJB, NameFile2AJB, SQLString6 As String
            Path2AJB = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubAJB.FileName
            NameFile2AJB = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubAJB.FileName

            SQLString6 = "UPDATE GLLandPurchaseReqHd SET DocAJB = " + QuotedStr(NameFile2AJB) + _
            " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            FubAJB.SaveAs(Path2AJB)
            SQLExecuteNonQuery(SQLString6, ViewState("DBConnection").ToString)


            dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            lbAJB.Text = dt.Rows(0)("DocAJB").ToString
            lblmassageAJB.Visible = True
            FubAJB.Visible = False

        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnsaveAJB2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsaveAJB2.Click

        Try
            Dim dr As DataTable
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("save this transaction first, to upload the dokumen")
                Exit Sub
            End If

            If FubAJB2.FileBytes.Length > 3500000 Then
                lbStatus.Text = MessageDlg("Ukuran File Terlalu Besar. !! Max Upload 3.5Mb")
                Exit Sub
            End If

            If Right(FubAJB2.FileName, 4) <> ".pdf" Then
                lbStatus.Text = MessageDlg("Upload Pdf File Only !")
                Exit Sub
            End If

            Dim dt As DataTable
            Dim Path2AJB2, NameFile2AJB2, SQLString7 As String
            Path2AJB2 = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubAJB2.FileName
            NameFile2AJB2 = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubAJB2.FileName

            SQLString7 = "UPDATE GLLandPurchaseReqHd SET DocAJB2 = " + QuotedStr(NameFile2AJB2) + _
            " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            FubAJB2.SaveAs(Path2AJB2)
            SQLExecuteNonQuery(SQLString7, ViewState("DBConnection").ToString)


            dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            lbAJB2.Text = dt.Rows(0)("DocAJB2").ToString
            lblmassageAJB2.Visible = True
            FubAJB2.Visible = False

        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnsaveSSP_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsaveSSP.Click

        Try
            Dim dr As DataTable
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("save this transaction first, to upload the dokumen")
                Exit Sub
            End If

            If FubSSP.FileBytes.Length > 3500000 Then
                lbStatus.Text = MessageDlg("Ukuran File Terlalu Besar. !! Max Upload 3.5Mb")
                Exit Sub
            End If

            If Right(FubSSP.FileName, 4) <> ".pdf" Then
                lbStatus.Text = MessageDlg("Upload Pdf File Only !")
                Exit Sub
            End If

            Dim dt As DataTable
            Dim Path2SSP, NameFile2SSP, SQLString8 As String
            Path2SSP = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubSSP.FileName
            NameFile2SSP = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubSSP.FileName

            SQLString8 = "UPDATE GLLandPurchaseReqHd SET DocSSP = " + QuotedStr(NameFile2SSP) + _
            " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            FubSSP.SaveAs(Path2SSP)
            SQLExecuteNonQuery(SQLString8, ViewState("DBConnection").ToString)


            dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            lbSSP.Text = dt.Rows(0)("DocSSP").ToString
            lblmassageSSP.Visible = True
            FubSSP.Visible = False

        Catch ex As Exception
            lbStatus.Text = "btnsaveTTD_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnsaveSSD_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsaveSSD.Click

        Try
            Dim dr As DataTable
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("save this transaction first, to upload the dokumen")
                Exit Sub
            End If

            If FubSSD.FileBytes.Length > 3500000 Then
                lbStatus.Text = MessageDlg("Ukuran File Terlalu Besar. !! Max Upload 3.5Mb")
                Exit Sub
            End If

            If Right(FubSSD.FileName, 4) <> ".pdf" Then
                lbStatus.Text = MessageDlg("Upload Pdf File Only !")
                Exit Sub
            End If

            Dim dt As DataTable
            Dim Path2SSD, NameFile2SSD, SQLString9 As String
            Path2SSD = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubSSD.FileName
            NameFile2SSD = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubSSD.FileName

            SQLString9 = "UPDATE GLLandPurchaseReqHd SET DocSSD = " + QuotedStr(NameFile2SSD) + _
            " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            FubSSD.SaveAs(Path2SSD)
            SQLExecuteNonQuery(SQLString9, ViewState("DBConnection").ToString)


            dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            lbSSD.Text = dt.Rows(0)("DocSSD").ToString
            lblmassageSSD.Visible = True
            FubSSD.Visible = False

        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnsaveSKTS_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsaveSKTS.Click

        Try
            Dim dr As DataTable
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("save this transaction first, to upload the dokumen")
                Exit Sub
            End If

            If FubSKTS.FileBytes.Length > 3500000 Then
                lbStatus.Text = MessageDlg("Ukuran File Terlalu Besar. !! Max Upload 3.5Mb")
                Exit Sub
            End If

            If Right(FubSKTS.FileName, 4) <> ".pdf" Then
                lbStatus.Text = MessageDlg("Upload Pdf File Only !")
                Exit Sub
            End If

            Dim dt As DataTable
            Dim Path2SKTS, NameFile2SKTS, SQLString10 As String
            Path2SKTS = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubSKTS.FileName
            NameFile2SKTS = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubSKTS.FileName

            SQLString10 = "UPDATE GLLandPurchaseReqHd SET DocKTS = " + QuotedStr(NameFile2SKTS) + _
            " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            FubSKTS.SaveAs(Path2SKTS)
            SQLExecuteNonQuery(SQLString10, ViewState("DBConnection").ToString)


            dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            lbSKTS.Text = dt.Rows(0)("DocKTS").ToString
            lblmassageSKTS.Visible = True
            FubSKTS.Visible = False

        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnsaveSPBT_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsaveSPBT.Click

        Try
            Dim dr As DataTable
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("save this transaction first, to upload the dokumen")
                Exit Sub
            End If

            If FubSPBT.FileBytes.Length > 3500000 Then
                lbStatus.Text = MessageDlg("Ukuran File Terlalu Besar. !! Max Upload 3.5Mb")
                Exit Sub
            End If

            If Right(FubSPBT.FileName, 4) <> ".pdf" Then
                lbStatus.Text = MessageDlg("Upload Pdf File Only !")
                Exit Sub
            End If

            Dim dt As DataTable
            Dim Path2SPBT, NameFile2SPBT, SQLString11 As String
            Path2SPBT = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubSPBT.FileName
            NameFile2SPBT = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubSPBT.FileName

            SQLString11 = "UPDATE GLLandPurchaseReqHd SET DocSBPT = " + QuotedStr(NameFile2SPBT) + _
            " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            FubSPBT.SaveAs(Path2SPBT)
            SQLExecuteNonQuery(SQLString11, ViewState("DBConnection").ToString)


            dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            lbSPBT.Text = dt.Rows(0)("DocSBPT").ToString
            lblmassageSPBT.Visible = True
            FubSPBT.Visible = False

        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnsaveSPKTT_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsaveSPKTT.Click

        Try
            Dim dr As DataTable
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("save this transaction first, to upload the dokumen")
                Exit Sub
            End If

            If FubSPKTT.FileBytes.Length > 3500000 Then
                lbStatus.Text = MessageDlg("Ukuran File Terlalu Besar. !! Max Upload 3.5Mb")
                Exit Sub
            End If

            If Right(FubSPKTT.FileName, 4) <> ".pdf" Then
                lbStatus.Text = MessageDlg("Upload Pdf File Only !")
                Exit Sub
            End If

            Dim dt As DataTable
            Dim Path2SPKTT, NameFile2SPKTT, SQLString12 As String
            Path2SPKTT = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubSPKTT.FileName
            NameFile2SPKTT = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubSPKTT.FileName

            SQLString12 = "UPDATE GLLandPurchaseReqHd SET DocSPKTT = " + QuotedStr(NameFile2SPKTT) + _
            " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            FubSPKTT.SaveAs(Path2SPKTT)
            SQLExecuteNonQuery(SQLString12, ViewState("DBConnection").ToString)


            dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            lbSPKTT.Text = dt.Rows(0)("DocSPKTT").ToString
            lblmassageSPKTT.Visible = True
            FubSPKTT.Visible = False

        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnsaveBAM_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsaveBAM.Click

        Try
            Dim dr As DataTable
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("save this transaction first, to upload the dokumen")
                Exit Sub
            End If

            If FubBAM.FileBytes.Length > 3500000 Then
                lbStatus.Text = MessageDlg("Ukuran File Terlalu Besar. !! Max Upload 3.5Mb")
                Exit Sub
            End If

            If Right(FubBAM.FileName, 4) <> ".pdf" Then
                lbStatus.Text = MessageDlg("Upload Pdf File Only !")
                Exit Sub
            End If

            Dim dt As DataTable
            Dim Path2BAM, NameFile2BAM, SQLString13 As String
            Path2BAM = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubBAM.FileName
            NameFile2BAM = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubBAM.FileName

            SQLString13 = "UPDATE GLLandPurchaseReqHd SET DocBAM = " + QuotedStr(NameFile2BAM) + _
            " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            FubBAM.SaveAs(Path2BAM)
            SQLExecuteNonQuery(SQLString13, ViewState("DBConnection").ToString)


            dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            lbBAM.Text = dt.Rows(0)("DocBAM").ToString
            lblmassageBAM.Visible = True
            FubBAM.Visible = False

        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnsaveBAPL_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsaveBAPL.Click

        Try
            Dim dr As DataTable
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("save this transaction first, to upload the dokumen")
                Exit Sub
            End If

            If FubBAPL.FileBytes.Length > 3500000 Then
                lbStatus.Text = MessageDlg("Ukuran File Terlalu Besar. !! Max Upload 3.5Mb")
                Exit Sub
            End If

            If Right(FubBAPL.FileName, 4) <> ".pdf" Then
                lbStatus.Text = MessageDlg("Upload Pdf File Only !")
                Exit Sub
            End If

            Dim dt As DataTable
            Dim Path2BAPL, NameFile2BAPL, SQLString14 As String
            Path2BAPL = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubBAPL.FileName
            NameFile2BAPL = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubBAPL.FileName

            SQLString14 = "UPDATE GLLandPurchaseReqHd SET DocBAPL = " + QuotedStr(NameFile2BAPL) + _
            " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            FubBAPL.SaveAs(Path2BAPL)
            SQLExecuteNonQuery(SQLString14, ViewState("DBConnection").ToString)


            dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            lbBAPL.Text = dt.Rows(0)("DocBAPL").ToString
            lblmassageBAPL.Visible = True
            FubBAPL.Visible = False

        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnsavePTPP_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsavePTPP.Click

        Try
            Dim dr As DataTable
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("save this transaction first, to upload the dokumen")
                Exit Sub
            End If

            If FubPTPP.FileBytes.Length > 3500000 Then
                lbStatus.Text = MessageDlg("Ukuran File Terlalu Besar. !! Max Upload 3.5Mb")
                Exit Sub
            End If

            If Right(FubPTPP.FileName, 4) <> ".pdf" Then
                lbStatus.Text = MessageDlg("Upload Pdf File Only !")
                Exit Sub
            End If

            Dim dt As DataTable
            Dim Path2PTPP, NameFile2PTPP, SQLString15 As String
            Path2PTPP = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubPTPP.FileName
            NameFile2PTPP = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubPTPP.FileName

            SQLString15 = "UPDATE GLLandPurchaseReqHd SET DocPTPP = " + QuotedStr(NameFile2PTPP) + _
            " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            FubPTPP.SaveAs(Path2PTPP)
            SQLExecuteNonQuery(SQLString15, ViewState("DBConnection").ToString)


            dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            lbPTPP.Text = dt.Rows(0)("DocPTPP").ToString
            lblmassagePTPP.Visible = True
            FubPTPP.Visible = False

        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnsavePTPP2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsavePTPP2.Click

        Try
            Dim dr As DataTable
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("save this transaction first, to upload the dokumen")
                Exit Sub
            End If

            If FubPTPP2.FileBytes.Length > 3500000 Then
                lbStatus.Text = MessageDlg("Ukuran File Terlalu Besar. !! Max Upload 3.5Mb")
                Exit Sub
            End If

            If Right(FubPTPP2.FileName, 4) <> ".pdf" Then
                lbStatus.Text = MessageDlg("Upload Pdf File Only !")
                Exit Sub
            End If

            Dim dt As DataTable
            Dim Path2PTPP2, NameFile2PTPP2, SQLString16 As String
            Path2PTPP2 = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubPTPP2.FileName
            NameFile2PTPP2 = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubPTPP2.FileName

            SQLString16 = "UPDATE GLLandPurchaseReqHd SET DocPTPP2 = " + QuotedStr(NameFile2PTPP2) + _
            " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            FubPTPP2.SaveAs(Path2PTPP2)
            SQLExecuteNonQuery(SQLString16, ViewState("DBConnection").ToString)


            dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            lbPtPP2.Text = dt.Rows(0)("DocPTPP2").ToString
            lblmassagePTPP2.Visible = True
            FubPTPP2.Visible = False

        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnsaveSKRT_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsaveSKRT.Click

        Try
            Dim dr As DataTable
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("save this transaction first, to upload the dokumen")
                Exit Sub
            End If

            If FubSKRT.FileBytes.Length > 3500000 Then
                lbStatus.Text = MessageDlg("Ukuran File Terlalu Besar. !! Max Upload 3.5Mb")
                Exit Sub
            End If

            If Right(FubSKRT.FileName, 4) <> ".pdf" Then
                lbStatus.Text = MessageDlg("Upload Pdf File Only !")
                Exit Sub
            End If

            Dim dt As DataTable
            Dim Path2SKRT, NameFile2SKRT, SQLString17 As String
            Path2SKRT = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubSKRT.FileName
            NameFile2SKRT = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubSKRT.FileName

            SQLString17 = "UPDATE GLLandPurchaseReqHd SET DocSKRT = " + QuotedStr(NameFile2SKRT) + _
            " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            FubSKRT.SaveAs(Path2SKRT)
            SQLExecuteNonQuery(SQLString17, ViewState("DBConnection").ToString)


            dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            lbSKRT.Text = dt.Rows(0)("DocSKRT").ToString
            lblmassageSKRT.Visible = True
            FubSKRT.Visible = False

        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnsaveSKD_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsaveSKD.Click

        Try
            Dim dr As DataTable
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("save this transaction first, to upload the dokumen")
                Exit Sub
            End If

            If FubSKD.FileBytes.Length > 3500000 Then
                lbStatus.Text = MessageDlg("Ukuran File Terlalu Besar. !! Max Upload 3.5Mb")
                Exit Sub
            End If

            If Right(FubSKD.FileName, 4) <> ".pdf" Then
                lbStatus.Text = MessageDlg("Upload Pdf File Only !")
                Exit Sub
            End If

            Dim dt As DataTable
            Dim Path2SKD, NameFile2SKD, SQLString18 As String
            Path2SKD = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubSKD.FileName
            NameFile2SKD = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubSKD.FileName

            SQLString18 = "UPDATE GLLandPurchaseReqHd SET DocSKD = " + QuotedStr(NameFile2SKD) + _
            " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            FubSKD.SaveAs(Path2SKD)
            SQLExecuteNonQuery(SQLString18, ViewState("DBConnection").ToString)


            dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            lbSKD.Text = dt.Rows(0)("DocSKD").ToString
            lblmassageSKD.Visible = True
            FubSKD.Visible = False

        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnsaveFcGirik_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsaveFCGirik.Click

        Try
            Dim dr As DataTable
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("save this transaction first, to upload the dokumen")
                Exit Sub
            End If

            If FubFCGirik.FileBytes.Length > 3500000 Then
                lbStatus.Text = MessageDlg("Ukuran File Terlalu Besar. !! Max Upload 3.5Mb")
                Exit Sub
            End If

            If Right(FubFCGirik.FileName, 4) <> ".pdf" Then
                lbStatus.Text = MessageDlg("Upload Pdf File Only !")
                Exit Sub
            End If

            Dim dt As DataTable
            Dim Path2FcGirik, NameFile2FcGirik, SQLString19 As String
            Path2FcGirik = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubFCGirik.FileName
            NameFile2FcGirik = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubFCGirik.FileName

            SQLString19 = "UPDATE GLLandPurchaseReqHd SET DocFcGirik = " + QuotedStr(NameFile2FcGirik) + _
            " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            FubFCGirik.SaveAs(Path2FcGirik)
            SQLExecuteNonQuery(SQLString19, ViewState("DBConnection").ToString)


            dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            lbFCGirik.Text = dt.Rows(0)("DocFcGirik").ToString
            lblmassageFcGirik.Visible = True
            FubFCGirik.Visible = False

        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnsaveFDP_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsaveFDP.Click

        Try

            Dim dr As DataTable
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("save this transaction first, to upload the dokumen")
                Exit Sub
            End If

            If FubFDP.FileBytes.Length > 3500000 Then
                lbStatus.Text = MessageDlg("Ukuran File Terlalu Besar. !! Max Upload 3.5Mb")
                Exit Sub
            End If

            If Right(FubFDP.FileName, 4) <> ".pdf" Then
                lbStatus.Text = MessageDlg("Upload Pdf File Only !")
                Exit Sub
            End If

            Dim dt As DataTable
            Dim Path2FDP, NameFile2FDP, SQLString20 As String
            Path2FDP = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubFDP.FileName
            NameFile2FDP = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubFDP.FileName

            SQLString20 = "UPDATE GLLandPurchaseReqHd SET DocFDP = " + QuotedStr(NameFile2FDP) + _
            " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            FubFDP.SaveAs(Path2FDP)
            SQLExecuteNonQuery(SQLString20, ViewState("DBConnection").ToString)


            dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            lbFDP.Text = dt.Rows(0)("DocFDP").ToString
            lblmassageFDP.Visible = True
            FubFDP.Visible = False

        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnsavePatok_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsavePatok.Click

        Try
            Dim dr As DataTable
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("save this transaction first, to upload the dokumen")
                Exit Sub
            End If

            If FubPatok.FileBytes.Length > 3500000 Then
                lbStatus.Text = MessageDlg("Ukuran File Terlalu Besar. !! Max Upload 3.5Mb")
                Exit Sub
            End If

            If Right(FubPatok.FileName, 4) <> ".pdf" Then
                lbStatus.Text = MessageDlg("Upload Pdf File Only !")
                Exit Sub
            End If

            Dim dt As DataTable
            Dim Path2Patok, NameFile2Patok, SQLString21 As String
            Path2Patok = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubPatok.FileName
            NameFile2Patok = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubPatok.FileName

            SQLString21 = "UPDATE GLLandPurchaseReqHd SET DocPatok = " + QuotedStr(NameFile2Patok) + _
            " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            FubPatok.SaveAs(Path2Patok)
            SQLExecuteNonQuery(SQLString21, ViewState("DBConnection").ToString)


            dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            lbPatok.Text = dt.Rows(0)("DocPatok").ToString
            lblmassagePatok.Visible = True
            FubPatok.Visible = False

        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnsaveSporadik_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsaveSporadik.Click

        Try

            Dim dr As DataTable
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("save this transaction first, to upload the dokumen")
                Exit Sub
            End If

            If FubSporadik.FileBytes.Length > 3500000 Then
                lbStatus.Text = MessageDlg("Ukuran File Terlalu Besar. !! Max Upload 3.5Mb")
                Exit Sub
            End If

            If Right(FubSporadik.FileName, 4) <> ".pdf" Then
                lbStatus.Text = MessageDlg("Upload Pdf File Only !")
                Exit Sub
            End If

            Dim dt As DataTable
            Dim Path2Sporadik, NameFile2Sporadik, SQLString22 As String
            Path2Sporadik = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubSporadik.FileName
            NameFile2Sporadik = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubSporadik.FileName

            SQLString22 = "UPDATE GLLandPurchaseReqHd SET DocSporadik = " + QuotedStr(NameFile2Sporadik) + _
            " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            FubSporadik.SaveAs(Path2Sporadik)
            SQLExecuteNonQuery(SQLString22, ViewState("DBConnection").ToString)


            dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            lbSporadik.Text = dt.Rows(0)("DocSporadik").ToString
            lblmassageSporadik.Visible = True
            FubSporadik.Visible = False

        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnsaveAHU_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsaveAHU.Click

        Try

            Dim dr As DataTable
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("save this transaction first, to upload the dokumen")
                Exit Sub
            End If

            If FubAHU.FileBytes.Length > 3500000 Then
                lbStatus.Text = MessageDlg("Ukuran File Terlalu Besar. !! Max Upload 3.5Mb")
                Exit Sub
            End If

            If Right(FubAHU.FileName, 4) <> ".pdf" Then
                lbStatus.Text = MessageDlg("Upload Pdf File Only !")
                Exit Sub
            End If

            Dim dt As DataTable
            Dim Path2AHU, NameFile2AHU, SQLString23 As String
            Path2AHU = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubAHU.FileName
            NameFile2AHU = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubAHU.FileName

            SQLString23 = "UPDATE GLLandPurchaseReqHd SET DocAHU = " + QuotedStr(NameFile2AHU) + _
            " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            FubAHU.SaveAs(Path2AHU)
            SQLExecuteNonQuery(SQLString23, ViewState("DBConnection").ToString)

            dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            lbAHU.Text = dt.Rows(0)("DocAHU").ToString
            lblmassageAHU.Visible = True
            FubAHU.Visible = False

        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnsaveSejarah_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsaveSejarah.Click

        Try
            Dim dr As DataTable
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("save this transaction first, to upload the dokumen")
                Exit Sub
            End If

            If FubSejarah.FileBytes.Length > 3500000 Then
                lbStatus.Text = MessageDlg("Ukuran File Terlalu Besar. !! Max Upload 3.5Mb")
                Exit Sub
            End If

            If Right(FubSejarah.FileName, 4) <> ".pdf" Then
                lbStatus.Text = MessageDlg("Upload Pdf File Only !")
                Exit Sub
            End If

            Dim dt As DataTable
            Dim Path2Sporadik, NameFile2Sporadik, SQLString24 As String
            Path2Sporadik = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubSejarah.FileName
            NameFile2Sporadik = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubSejarah.FileName

            SQLString24 = "UPDATE GLLandPurchaseReqHd SET DocSejarah = " + QuotedStr(NameFile2Sporadik) + _
            " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            FubSejarah.SaveAs(Path2Sporadik)
            SQLExecuteNonQuery(SQLString24, ViewState("DBConnection").ToString)

            dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            lbSejarah.Text = dt.Rows(0)("DocSejarah").ToString
            lblmassageSejarah.Visible = True
            FubSejarah.Visible = False

        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnsaveSPJH_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsaveSPJH.Click

        Try

            Dim dr As DataTable
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("save this transaction first, to upload the dokumen")
                Exit Sub
            End If

            If FubSPJH.FileBytes.Length > 3500000 Then
                lbStatus.Text = MessageDlg("Ukuran File Terlalu Besar. !! Max Upload 3.5Mb")
                Exit Sub
            End If

            If Right(FubSPJH.FileName, 4) <> ".pdf" Then
                lbStatus.Text = MessageDlg("Upload Pdf File Only !")
                Exit Sub
            End If

            Dim dt As DataTable
            Dim Path2SPJH, NameFile2SPJH, SQLString25 As String
            Path2SPJH = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubSPJH.FileName
            NameFile2SPJH = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubSPJH.FileName

            SQLString25 = "UPDATE GLLandPurchaseReqHd SET DocSPJH = " + QuotedStr(NameFile2SPJH) + _
            " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            FubAHU.SaveAs(Path2SPJH)
            SQLExecuteNonQuery(SQLString25, ViewState("DBConnection").ToString)

            dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            lbSPJH.Text = dt.Rows(0)("DocSPJH").ToString
            lblmassageSPJH.Visible = True
            FubSPJH.Visible = False

        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnsaveLainLain_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsaveLainLain.Click

        Try
            Dim dr As DataTable
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("save this transaction first, to upload the dokumen")
                Exit Sub
            End If

            If FubLainLain.FileBytes.Length > 3500000 Then
                lbStatus.Text = MessageDlg("Ukuran File Terlalu Besar. !! Max Upload 3.5Mb")
                Exit Sub
            End If

            If Right(FubLainLain.FileName, 4) <> ".pdf" Then
                lbStatus.Text = MessageDlg("Upload Pdf File Only !")
                Exit Sub
            End If

            Dim dt As DataTable
            Dim Path2LainLain, NameFile2FubLainLain, SQLString26 As String
            Path2LainLain = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubLainLain.FileName
            NameFile2FubLainLain = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubLainLain.FileName

            SQLString26 = "UPDATE GLLandPurchaseReqHd SET DocLainLain = " + QuotedStr(NameFile2FubLainLain) + _
            ", LainLain = " + QuotedStr(tbLainLain.Text) + _
            " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            FubLainLain.SaveAs(Path2LainLain)
            SQLExecuteNonQuery(SQLString26, ViewState("DBConnection").ToString)

            dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            lbLainLain.Text = dt.Rows(0)("DocLainLain").ToString
            lblmassageLainLain.Visible = True
            FubLainLain.Visible = False

        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnsaveKTPW_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsaveKtpW.Click

        Try
            Dim dr As DataTable
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("save this transaction first, to upload the dokumen")
                Exit Sub
            End If

            If FupKTPW.FileBytes.Length > 3500000 Then
                lbStatus.Text = MessageDlg("Ukuran File Terlalu Besar. !! Max Upload 3.5Mb")
                Exit Sub
            End If

            If Right(FupKTPW.FileName, 4) <> ".pdf" Then
                lbStatus.Text = MessageDlg("Upload Pdf File Only !")
                Exit Sub
            End If

            Dim dt As DataTable
            Dim Path2KTPW, NameFile2KTPW, SQLString26 As String
            Path2KTPW = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FupKTPW.FileName
            NameFile2KTPW = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FupKTPW.FileName

            SQLString26 = "UPDATE GLLandPurchaseReqHd SET DocKTPW = " + QuotedStr(NameFile2KTPW) + _
            " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            FupKTPW.SaveAs(Path2KTPW)
            SQLExecuteNonQuery(SQLString26, ViewState("DBConnection").ToString)

            dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            lbKtpW.Text = dt.Rows(0)("DocKTPW").ToString
            lblmassageKTPW.Visible = True
            FupKTPW.Visible = False

        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnsaveSPW_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsaveSPW.Click

        Try
            Dim dr As DataTable
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("save this transaction first, to upload the dokumen")
                Exit Sub
            End If

            If FubSPW.FileBytes.Length > 3500000 Then
                lbStatus.Text = MessageDlg("Ukuran File Terlalu Besar. !! Max Upload 3.5Mb")
                Exit Sub
            End If

            If Right(FubSPW.FileName, 4) <> ".pdf" Then
                lbStatus.Text = MessageDlg("Upload Pdf File Only !")
                Exit Sub
            End If

            Dim dt As DataTable
            Dim Path2SPW, NameFile2SPW, SQLString26 As String
            Path2SPW = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubSPW.FileName
            NameFile2SPW = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubSPW.FileName

            SQLString26 = "UPDATE GLLandPurchaseReqHd SET DocSPW = " + QuotedStr(NameFile2SPW) + _
            " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            FubSPW.SaveAs(Path2SPW)
            SQLExecuteNonQuery(SQLString26, ViewState("DBConnection").ToString)

            dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            lbSPW.Text = dt.Rows(0)("DocSPW").ToString
            lblmassageSPW.Visible = True
            FubSPW.Visible = False

        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnsaveSPKW_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsaveSPKW.Click

        Try
            Dim dr As DataTable
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("save this transaction first, to upload the dokumen")
                Exit Sub
            End If

            If FubSPKW.FileBytes.Length > 3500000 Then
                lbStatus.Text = MessageDlg("Ukuran File Terlalu Besar. !! Max Upload 3.5Mb")
                Exit Sub
            End If

            If Right(FubSPKW.FileName, 4) <> ".pdf" Then
                lbStatus.Text = MessageDlg("Upload Pdf File Only !")
                Exit Sub
            End If

            Dim dt As DataTable
            Dim Path2SPKW, NameFile2SPKW, SQLString26 As String
            Path2SPKW = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubSPKW.FileName
            NameFile2SPKW = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubSPKW.FileName

            SQLString26 = "UPDATE GLLandPurchaseReqHd SET DocSPKW = " + QuotedStr(NameFile2SPKW) + _
            " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            FubSPKW.SaveAs(Path2SPKW)
            SQLExecuteNonQuery(SQLString26, ViewState("DBConnection").ToString)

            dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            lbSPKW.Text = dt.Rows(0)("DocSPKW").ToString
            lblmassageSPKW.Visible = True
            FubSPKW.Visible = False

        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnsaveBPJS_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveBPJS.Click

        Try
            Dim dr As DataTable
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("save this transaction first, to upload the dokumen")
                Exit Sub
            End If

            If FubBPJS.FileBytes.Length > 3500000 Then
                lbStatus.Text = MessageDlg("Ukuran File Terlalu Besar. !! Max Upload 3.5Mb")
                Exit Sub
            End If

            If Right(FubBPJS.FileName, 4) <> ".pdf" Then
                lbStatus.Text = MessageDlg("Upload Pdf File Only !")
                Exit Sub
            End If

            Dim dt As DataTable
            Dim Path2BPJS, NameFile2BPJS, SQLString26 As String
            Path2BPJS = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubBPJS.FileName
            NameFile2BPJS = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubBPJS.FileName

            SQLString26 = "UPDATE GLLandPurchaseReqHd SET DocBPJS = " + QuotedStr(NameFile2BPJS) + _
            " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            FubBPJS.SaveAs(Path2BPJS)
            SQLExecuteNonQuery(SQLString26, ViewState("DBConnection").ToString)

            dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            lbBPJS.Text = dt.Rows(0)("DocBPJS").ToString
            lblmassageBPJS.Visible = True
            FubBPJS.Visible = False

        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnsaveLainLain2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsaveLainLain2.Click

        Try
            Dim dr As DataTable
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("save this transaction first, to upload the dokumen")
                Exit Sub
            End If

            If FubLainLain.FileBytes.Length > 3500000 Then
                lbStatus.Text = MessageDlg("Ukuran File Terlalu Besar. !! Max Upload 3.5Mb")
                Exit Sub
            End If

            If Right(FubLainLain2.FileName, 4) <> ".pdf" Then
                lbStatus.Text = MessageDlg("Upload Pdf File Only !")
                Exit Sub
            End If

            Dim dt As DataTable
            Dim Path2LainLain, NameFile2FubLainLain, SQLString26 As String
            Path2LainLain = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubLainLain2.FileName
            NameFile2FubLainLain = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubLainLain2.FileName

            SQLString26 = "UPDATE GLLandPurchaseReqHd SET DocLainLain2 = " + QuotedStr(NameFile2FubLainLain) + _
            ", LainLain2 = " + QuotedStr(tbLainLain2.Text) + _
            " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            FubLainLain2.SaveAs(Path2LainLain)
            SQLExecuteNonQuery(SQLString26, ViewState("DBConnection").ToString)

            dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            lbLainLain2.Text = dt.Rows(0)("DocLainLain2").ToString
            lblmassageLainLain2.Visible = True
            FubLainLain2.Visible = False

        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnsaveLainLain3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsaveLainLain3.Click

        Try
            Dim dr As DataTable
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("save this transaction first, to upload the dokumen")
                Exit Sub
            End If

            If FubLainLain.FileBytes.Length > 3500000 Then
                lbStatus.Text = MessageDlg("Ukuran File Terlalu Besar. !! Max Upload 3.5Mb")
                Exit Sub
            End If

            If Right(FubLainLain3.FileName, 4) <> ".pdf" Then
                lbStatus.Text = MessageDlg("Upload Pdf File Only !")
                Exit Sub
            End If

            Dim dt As DataTable
            Dim Path2LainLain, NameFile2FubLainLain, SQLString26 As String
            Path2LainLain = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubLainLain3.FileName
            NameFile2FubLainLain = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubLainLain3.FileName

            SQLString26 = "UPDATE GLLandPurchaseReqHd SET DocLainLain3 = " + QuotedStr(NameFile2FubLainLain) + _
            ", LainLain3 = " + QuotedStr(tbLainLain3.Text) + _
            " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            FubLainLain3.SaveAs(Path2LainLain)
            SQLExecuteNonQuery(SQLString26, ViewState("DBConnection").ToString)

            dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            lbLainLain3.Text = dt.Rows(0)("DocLainLain3").ToString
            lblmassageLainLain3.Visible = True
            FubLainLain3.Visible = False

        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnGoEdit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGoEdit.Click

        Dim dt As DataTable
        Dim CekMenu As String
        Try
            dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dt.Rows(0)("Status").ToString = "H" Or dt.Rows(0)("Status").ToString = "G" Or dt.Rows(0)("Status").ToString = "I" Then
                CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                If CekMenu <> "" Then
                    lbStatus.Text = CekMenu
                    Exit Sub
                End If
                MovePanel(PnlHd, pnlInput)
                ViewState("StateHd") = "Edit"
                ModifyInput2(True, pnlInput, pnlDt, GridDt)
                ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
                ModifyInput2(True, pnlInput, pnlDt3, GridDt3)
                ModifyInput2(True, pnlInput, PnlDt4, GridDt4)

                btnHome.Visible = False
                'MultiView1.ActiveViewIndex = 0
                'Menu1.Items.Item(0).Selected = True
                EnableHd(True)

                Dim Dr As DataRow()

                Dr = ViewState("Dt4").Select("NoWl = " + QuotedStr(lbNoWl.Text))

                If Dr.Length = 0 Then
                    GridDt4.Columns(0).Visible = False
                Else

                    GridDt4.Columns(0).Visible = True
                End If

            ElseIf dt.Rows(0)("Status").ToString = "P" Then
                EnableFub(True)
            Else
                lbStatus.Text = MessageDlg("Data must be Hold or Get Approval to edit")
                Exit Sub
            End If


            
            
           
            

        Catch ex As Exception
            lbStatus.Text = "btnGoEdit_Click Error : " + ex.ToString
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

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        Dim index As Integer
        Try
            index = Int32.Parse(e.Item.Value)
            MultiView1.ActiveViewIndex = index
            If MultiView1.ActiveViewIndex = 1 Then
                pnlDt2.Visible = True
                pnlEditDt2.Visible = False
                GridDt2.Columns(0).Visible = GetCountRecord(ViewState("Dt2")) > 0

            ElseIf MultiView1.ActiveViewIndex = 2 Then
                pnlDt3.Visible = True
                pnlEditDt3.Visible = False
                GridDt3.Columns(0).Visible = GetCountRecord(ViewState("Dt3")) > 0
                GridDt3.Columns(1).Visible = True
                If (ViewState("StateHd") = "View" Or ViewState("StateHd") = "Insert" Or GetCountRecord(ViewState("Dt3")) = 0) Then
                    GridDt3.Columns(0).Visible = False
                    GridDt3.Columns(1).Visible = False
                Else
                    GridDt3.Columns(0).Visible = True
                    GridDt3.Columns(1).Visible = True
                End If

                If (GetCountRecord(ViewState("Dt3")) > 0) Then
                    GridDt3.Columns(1).Visible = True
                End If

                'BindDataDt3(ViewState("TransNmbr"))
            ElseIf MultiView1.ActiveViewIndex = 3 Then
                PnlDt4.Visible = True
                pnlEditDt4.Visible = False
                GridDt4.Columns(0).Visible = GetCountRecord(ViewState("Dt4")) > 0
                'BindDataDt4(ViewState("TransNmbr"))
            Else
                pnlDt.Visible = True
                pnlEditDt.Visible = False
            End If
            btnSaveTrans.Focus()
        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub Menu2_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu2.MenuItemClick
        MultiView2.ActiveViewIndex = Int32.Parse(e.Item.Value)


    End Sub


    Private Sub SetInit()
        Try
            FillRange(ddlRange)

            FillCombo(ddlseller, "SELECT SellCode, SellName FROM V_MsSeller", True, "SellCode", "SellName", ViewState("DBConnection"))
            FillCombo(ddlModerator, "SELECT ModCode, ModName FROM V_MsModerator", True, "ModCode", "ModName", ViewState("DBConnection"))
            FillCombo(ddlArea, "SELECT AreaCode, AreaName FROM V_MsArea", True, "AreaCode", "AreaName", ViewState("DBConnection"))

            ViewState("SortExpression") = Nothing
            ViewState("SortExpressionOut") = Nothing
            ViewState("SortExpressionUse") = Nothing
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
            'If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
            '    'ddlCommand.Items.Add("Print")
            '    'ddlCommand2.Items.Add("Print")
            '    ddlCommand.Items.Add("Reject")
            '    ddlCommand2.Items.Add("Reject")
            'End If


            ' tbQtyMaterial.Attributes.Add("OnKeyDown", "return PressNumeric()")
            ' tbQtyMaterial.Attributes.Add("OnBlur", "setformatdt()")
            ' tbQtyTotal.Attributes.Add("ReadOnly", "True")
            'tbQtyTotal.Attributes.Add("OnBlur", "setformatdt()")
            ' tbQtyE.Attributes.Add("OnKeyDown", "return PressNumeric()")
            ' tbQtyWeek.Attributes.Add("OnKeyDown", "return PressNumeric()")
            ' tbQtyWeek.Attributes.Add("OnBlur", "setformatdt()"


            tbTotal.Attributes.Add("OnBlur", "setformathd();")
            tbNilai.Attributes.Add("OnBlur", "setformathd();")
            tbSPPT.Attributes.Add("OnBlur", "setformathd();")
            tbHrgFix.Attributes.Add("OnBlur", "setformathd();")

            tbAjbSphShm.Attributes.Add("OnBlur", "setformathd();")
            tbLuasUkur.Attributes.Add("OnBlur", "setformathd();")

            tbLuasDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbLuasAwal.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbLuaslvl1.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbLuaslvl2.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbLuaslvl3.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbLuaslvl4.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbLuaslvl5.Attributes.Add("OnKeyDown", "return PressNumeric();")

            tbLuasSubDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbLuasSubDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbLuasSubDt3.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbLuasSubDt4.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbLuasSubDt5.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbSisaSubDt.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbSisaSubDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbSisaSubDt3.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbSisaSubDt4.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbSisaSubDt5.Attributes.Add("OnKeyDown", "return PressNumeric();")



        Catch ex As Exception
            Throw New Exception("Set Init Error : " + ex.ToString)
        End Try
    End Sub
    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim SQLString, StrFilter As String
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
            'DT = BindDataTransaction(GetStringHd(), StrFilter, ViewState("DBConnection").ToString)
            If DT.Rows.Count = 0 Then
                lbStatus.Text = "No Data"
                pnlNav.Visible = False
                'ddlCommand.Visible = False
                BtnGo.Visible = False
            End If
            ddlCommand.Visible = DT.Rows.Count > 0
            BtnGo.Visible = DT.Rows.Count > 0
            ddlCommand2.Visible = ddlCommand.Visible
            btnGo2.Visible = BtnGo.Visible
            ' btnAdd2.Visible = BtnGo.Visible
            DV = DT.DefaultView

            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "TransNmbr DESC"
            End If

            DV.Sort = ViewState("SortExpression")
            GridView1.DataSource = DV
            GridView1.DataBind()

        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub
    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT * From V_GLLandPurchaseReqDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function
    Private Function GetStringDt2(ByVal Nmbr As String) As String
        Return "SELECT * From V_PLPlanLandDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Private Function GetStringDt3(ByVal Nmbr As String) As String
        Return "SELECT * From V_GLLandPurchaseReqDt2 WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Private Function GetStringDt4(ByVal Nmbr As String) As String
        Return "SELECT * From V_GLLandPurchaseReqSubDt2 WHERE TransNmbr = " + QuotedStr(Nmbr) + " ORDER BY DocDate ASC "
    End Function

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
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

    Protected Sub BtnGo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnGo.Click, btnGo2.Click
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

            Status = CekStatus(ActionValue)

            ListSelectNmbr = ""
            GetListCommand(Status, GridView1, "3,2", ListSelectNmbr, Nmbr, lbStatus.Text)
            If ListSelectNmbr = "" Then Exit Sub
            For j = 0 To (Nmbr.Length - 1)
                If Nmbr(j) = "" Then
                    Exit For
                Else
                    Result = ExecSPCommandGo(ActionValue, "S_PLGLLandPurchaseReq", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                    If Trim(Result) <> "" Then
                        lbStatus.Text = lbStatus.Text + Result + " <br/>"
                    End If
                End If
            Next
            BindData("TransNmbr in (" + ListSelectNmbr + ")")

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

    Private Sub EnableButtonView(ByVal State As Boolean)
        Try

        Catch ex As Exception
            Throw New Exception("Enable Button Error " + ex.ToString)
        End Try
    End Sub

    Private Sub EnableHd(ByVal State As Boolean)
        Try
            tbCode.Enabled = False
            tbDate.Enabled = State
            tbBlok.Enabled = State
            tbKohir.Enabled = State
            tbPercil.Enabled = State
            tbAJB.Enabled = State
            tbSPH.Enabled = State
            tbSHM.Enabled = State
            ddlJenisDokumen.Enabled = State

            ddlseller.Enabled = State
            ddlModerator.Enabled = State
            ddlHitungTotal.Enabled = State
            ddlLandType.Enabled = State
            ddlJenisDok.Enabled = State

            tbSptPbb.Enabled = State
            tbSPPT.Enabled = State
            tbAjbSphShm.Enabled = State
            tbLuasUkur.Enabled = State
            tbNilai.Enabled = State
            tbTotal.Enabled = False
            tbAddress.Enabled = State
            tbProvinsi.Enabled = State
            tbKec.Enabled = State
            tbKab.Enabled = State
            tbDesa.Enabled = State
            tbPetaRincikNo.Enabled = State
            tbNamaPembeli.Enabled = State
            tbRemark.Enabled = State
            tbLainLain.Enabled = State
            tbNoDocHD.Enabled = State
            btnSeller.Enabled = State
            btnModerator.Enabled = State
            btnArea.Enabled = State


            FupMain.Visible = State
            FubKK.Visible = State
            FubSPPT.Visible = State
            FubSTTS.Visible = State
            FubTTD.Visible = State
            FubAJB.Visible = State
            FubAJB2.Visible = State
            FubSSP.Visible = State
            FubSSD.Visible = State
            FubSKTS.Visible = State
            FubSPBT.Visible = State
            FubSPKTT.Visible = State
            FubBAM.Visible = State
            FubBAPL.Visible = State
            FubPTPP.Visible = State
            FubPTPP2.Visible = State
            FubSKRT.Visible = State
            FubSKD.Visible = State
            FubFCGirik.Visible = State
            FubFDP.Visible = State
            FubPatok.Visible = State
            FubSporadik.Visible = State
            FubAHU.Visible = State
            FubSejarah.Visible = State
            FubSPJH.Visible = State
            FubLainLain.Visible = State
            FupKTPW.Visible = State
            FubSPW.Visible = State
            FubSPKW.Visible = State
            FubBPJS.Visible = State

            FubLainLain2.Visible = State
            FubLainLain3.Visible = State

            If ddlJenisDokumen.SelectedValue = "SHGB" Or ddlJenisDokumen.SelectedValue = "SHGU" Then
                tbMasaBerlaku.Enabled = State

            End If


            lblmassageKTP.Visible = False
            lblmassageKK.Visible = False
            lblmassageSPPT.Visible = False
            lblmassageSTTS.Visible = False
            lblmassageTTD.Visible = False
            lblmassageAJB.Visible = False
            lblmassageAJB2.Visible = False
            lblmassageSSP.Visible = False
            lblmassageSSD.Visible = False
            lblmassageSKTS.Visible = False
            lblmassageSPBT.Visible = False
            lblmassageSPKTT.Visible = False
            lblmassageBAM.Visible = False
            lblmassageBAPL.Visible = False
            lblmassagePTPP.Visible = False
            lblmassagePTPP2.Visible = False
            lblmassageSKRT.Visible = False
            lblmassageSKD.Visible = False
            lblmassageFcGirik.Visible = False
            lblmassageFDP.Visible = False
            lblmassagePatok.Visible = False
            lblmassageSporadik.Visible = False
            lblmassageAHU.Visible = False
            lblmassageSejarah.Visible = False
            lblmassageSPJH.Visible = False
            lblmassageLainLain.Visible = False
            lblmassageKTPW.Visible = False
            lblmassageSPW.Visible = False
            lblmassageSPKW.Visible = False
            lblmassageBPJS.Visible = False

            lblmassageLainLain2.Visible = False
            lblmassageLainLain3.Visible = False



        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub


    Private Sub EnableFub(ByVal State As Boolean)
        Try

            FupMain.Visible = State
            FubKK.Visible = State
            FubSPPT.Visible = State
            FubSTTS.Visible = State
            FubTTD.Visible = State
            FubAJB.Visible = State
            FubAJB2.Visible = State
            FubSSP.Visible = State
            FubSSD.Visible = State
            FubSKTS.Visible = State
            FubSPBT.Visible = State
            FubSPKTT.Visible = State
            FubBAM.Visible = State
            FubBAPL.Visible = State
            FubPTPP.Visible = State
            FubPTPP2.Visible = State
            FubSKRT.Visible = State
            FubSKD.Visible = State
            FubFCGirik.Visible = State
            FubFDP.Visible = State
            FubPatok.Visible = State
            FubSporadik.Visible = State
            FubAHU.Visible = State
            FubSejarah.Visible = State
            FubSPJH.Visible = State
            FubLainLain.Visible = State
            FupKTPW.Visible = State
            FubSPW.Visible = State
            FubSPKW.Visible = State
            FubBPJS.Visible = State

            FubLainLain2.Visible = State
            FubLainLain3.Visible = State



            lblmassageKTP.Visible = False
            lblmassageKK.Visible = False
            lblmassageSPPT.Visible = False
            lblmassageSTTS.Visible = False
            lblmassageTTD.Visible = False
            lblmassageAJB.Visible = False
            lblmassageAJB2.Visible = False
            lblmassageSSP.Visible = False
            lblmassageSSD.Visible = False
            lblmassageSKTS.Visible = False
            lblmassageSPBT.Visible = False
            lblmassageSPKTT.Visible = False
            lblmassageBAM.Visible = False
            lblmassageBAPL.Visible = False
            lblmassagePTPP.Visible = False
            lblmassagePTPP2.Visible = False
            lblmassageSKRT.Visible = False
            lblmassageSKD.Visible = False
            lblmassageFcGirik.Visible = False
            lblmassageFDP.Visible = False
            lblmassagePatok.Visible = False
            lblmassageSporadik.Visible = False
            lblmassageAHU.Visible = False
            lblmassageSejarah.Visible = False
            lblmassageSPJH.Visible = False
            lblmassageLainLain.Visible = False
            lblmassageKTPW.Visible = False
            lblmassageSPW.Visible = False
            lblmassageSPKW.Visible = False
            lblmassageBPJS.Visible = False

            lblmassageLainLain2.Visible = False
            lblmassageLainLain3.Visible = False



        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub EnableDt3(ByVal State As Boolean)
        Try

        Catch ex As Exception
            Throw New Exception("Enable Dt3 Error " + ex.ToString)
        End Try
    End Sub
    Private Sub EnableDt4(ByVal State As Boolean)
        Try
    

        Catch ex As Exception
            Throw New Exception("Enable Dt4 Error " + ex.ToString)
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
    Private Sub BindDataDt2(ByVal Nmbr As String)
        Dim Drow As DataRow()
        Try
            Dim dt As New DataTable
            ViewState("Dt2") = Nothing
            dt = SQLExecuteQuery(GetStringDt2(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt2") = dt
            BindGridDt(dt, GridDt2)
            ' Drow = ViewState("Dt2").Select("Block=" + QuotedStr(GVR.cell().text))
            ' If Drow.Length > 0 Then
            ' BindGridDt(Drow.CopyToDataTable, GridDt2)
            ' GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "ViewA"
            ' Else
            ' Dim DtTemp As DataTable
            ' DtTemp = ViewState("Dt2").Clone
            ' DtTemp.Rows.Add(DtTemp.NewRow())
            ' GridDt2.DataSource = DtTemp
            ' GridDt2.DataBind()
            'GridDt2.Columns(0).Visible = False
            ' End If
        Catch ex As Exception
            Throw New Exception("Bind Data Dt2 Error : " + ex.ToString)
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
            ' Drow = ViewState("Dt3").Select("Type+'|'+DivisiBlok=" + QuotedStr(LblTypeE.Text + "|" + LblMaterialE.Text))
            ' If Drow.Length > 0 Then
            ' BindGridDt(Drow.CopyToDataTable, GridDt3)
            ' GridDt3.Columns(0).Visible = Not ViewState("StateHd") = "ViewE"
            'Else
            'Dim DtTemp As DataTable
            'DtTemp = ViewState("Dt3").Clone
            'DtTemp.Rows.Add(DtTemp.NewRow())
            'GridDt3.DataSource = DtTemp
            'GridDt3.DataBind()
            'GridDt3.Columns(0).Visible = False
            'End If
        Catch ex As Exception
            Throw New Exception("Bind Data Dt3 Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDt4(ByVal Nmbr As String)
        Dim Drow As DataRow()
        Try
            Dim dt As New DataTable
            ViewState("Dt4") = Nothing
            dt = SQLExecuteQuery(GetStringDt4(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt4") = dt
            BindGridDt(dt, GridDt4)
            'Drow = ViewState("Dt4").Select("BlockPlant=" + QuotedStr(LblTypeMT.Text))
            'If Drow.Length > 0 Then
            '    BindGridDt(Drow.CopyToDataTable, GridDt4)
            '    GridDt4.Columns(0).Visible = Not ViewState("StateHd") = "ViewM"
            'Else
            '    Dim DtTemp As DataTable
            '    DtTemp = ViewState("Dt4").Clone
            '    DtTemp.Rows.Add(DtTemp.NewRow())
            '    GridDt4.DataSource = DtTemp
            '    GridDt4.DataBind()
            '    GridDt4.Columns(0).Visible = False
            'End If
        Catch ex As Exception
            Throw New Exception("Bind Data Dt4 Error : " + ex.ToString)
        End Try
    End Sub

    Sub BindGridDt2(ByVal source As DataTable, ByVal gv As GridView)
        Dim IsEmpty As Boolean
        Dim DtTemp As DataTable
        Dim dr As DataRow()
        Try
            IsEmpty = False
            dr = source.Select("", "", DataViewRowState.CurrentRows)
            If dr.Count = 0 Then
                'If source.Rows.Count = 0 Then
                DtTemp = source.Clone
                DtTemp.Rows.Add(DtTemp.NewRow()) ' create a new blank row to the DataTable
                IsEmpty = True
                gv.DataSource = DtTemp
            Else
                gv.DataSource = source
            End If
            gv.DataBind()
            If IsEmpty = True Then
                gv.Columns(0).Visible = False
            Else
                gv.Columns(0).Visible = True
            End If

        Catch ex As Exception
            Throw New Exception("ShowGridDtIfEmpty Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBack.Click, btnHome.Click
        Try
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

    Protected Sub btnCancelDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelDt.Click
        Try
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt3")) = 0 And GetCountRecord(ViewState("Dt4")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnCancelDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelDt2.Click
        Try
            MovePanel(pnlEditDt2, pnlDt2)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt3")) = 0 And GetCountRecord(ViewState("Dt4")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt2  Error : " + ex.ToString
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbCode.Text = ""
            tbDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            tbTglTerbit.SelectedDate = ViewState("ServerDate") 'Now.Date
            tbMasaBerlaku.SelectedDate = ViewState("ServerDate") 'Now.Date
            ddlJenisDokumen.SelectedValue = "Choose One"
            tbNoDokumen.Text = ""
            tbBlok.Text = ""
            tbKohir.Text = ""
            tbPercil.Text = ""
            tbAJB.Text = ""
            tbSPH.Text = ""
            tbSHM.Text = ""
            tbBNIB.Text = ""
            tbNoSuratUkur.Text = ""
            tbNoLainLian.Text = ""

            ddlseller.SelectedIndex = 0
            ddlModerator.SelectedIndex = 0
            ddlArea.SelectedIndex = 0
            ddlHitungTotal.SelectedValue = "Choose One"

            tbSptPbb.Text = ""
            tbSPPT.Text = "0"
            tbHrgFix.Text = "0"
            tbAjbSphShm.Text = "0"
            tbLuasUkur.Text = "0"
            tbNilai.Text = "0"
            tbTotal.Text = "0"
            tbAddress.Text = ""
            tbProvinsi.Text = ""
            tbKec.Text = ""
            tbKab.Text = ""
            tbDesa.Text = ""
            tbPetaRincikNo.Text = ""
            tbNamaPembeli.Text = ""
            tbNoDocHD.Text = ""
            tbLainLain.Text = "Lain - Lain"
            tbLainLain2.Text = "Lain - Lain"
            tbLainLain3.Text = "Lain - Lain"
            tbSeller.Text = ""
            tbSellerName.Text = ""
            tbModerator.Text = ""
            tbModeratorName.Text = ""
            tbArea.Text = ""
            tbAreaName.Text = ""
            tbRemark.Text = ""

            cbKtp.Checked = False
            cbKk.Checked = False
            cbSPBT.Checked = False
            cbSTTS.Checked = False
            cbTTD.Checked = False
            cbAJB.Checked = False
            cbAJB2.Checked = False
            cbSSP.Checked = False
            cbSSD.Checked = False
            cbSKTS.Checked = False
            cbSPBT.Checked = False
            cbSPKTT.Checked = False
            cbBAM.Checked = False
            cbBAPL.Checked = False
            cbPTPP.Checked = False
            cbPTPP2.Checked = False
            cbSKRT.Checked = False
            cbSKD.Checked = False
            cbFCGirik.Checked = False
            cbFDP.Checked = False
            cbKtpW.Checked = False
            cbSPW.Checked = False
            cbSPKW.Checked = False
            cbPatok.Checked = False
            cbSporadik.Checked = False
            cbAHU.Checked = False
            cbSejarah.Checked = False
            cbSPJH.Checked = False
            cbLainLain.Checked = False
            cbSPPT.Checked = False
            cbBPJS.Checked = False

            cbLainLain2.Checked = False
            cbLainLain3.Checked = False




            lbKtp.Text = "Not Yet Uploaded"
            lbKK.Text = "Not Yet Uploaded"
            lbSPBT.Text = "Not Yet Uploaded"
            lbSTTS.Text = "Not Yet Uploaded"
            lbTTD.Text = "Not Yet Uploaded"
            lbAJB.Text = "Not Yet Uploaded"
            lbAJB2.Text = "Not Yet Uploaded"
            lbSSP.Text = "Not Yet Uploaded"
            lbSSD.Text = "Not Yet Uploaded"
            lbSKTS.Text = "Not Yet Uploaded"
            lbSPBT.Text = "Not Yet Uploaded"
            lbSPKTT.Text = "Not Yet Uploaded"
            lbBAM.Text = "Not Yet Uploaded"
            lbBAPL.Text = "Not Yet Uploaded"
            lbPTPP.Text = "Not Yet Uploaded"
            lbPtPP2.Text = "Not Yet Uploaded"
            lbSKRT.Text = "Not Yet Uploaded"
            lbSKD.Text = "Not Yet Uploaded"
            lbFCGirik.Text = "Not Yet Uploaded"
            lbFDP.Text = "Not Yet Uploaded"
            lbKtpW.Text = "Not Yet Uploaded"
            lbSPW.Text = "Not Yet Uploaded"
            lbSPKW.Text = "Not Yet Uploaded"
            lbPatok.Text = "Not Yet Uploaded"
            lbSporadik.Text = "Not Yet Uploaded"
            lbAHU.Text = "Not Yet Uploaded"
            lbSejarah.Text = "Not Yet Uploaded"
            lbSPJH.Text = "Not Yet Uploaded"
            lbLainLain.Text = "Not Yet Uploaded"
            lbSPPT.Text = "Not Yet Uploaded"
            lbBPJS.Text = "Not Yet Uploaded"
            lbLainLain2.Text = "Not Yet Uploaded"
            lbLainLain3.Text = "Not Yet Uploaded"




        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbKetKegiatan.Text = ""
            tbNoSurat.Text = ""
            tbDateSurat.SelectedDate = ViewState("ServerDate") 'Now.Date
            tbLuasDt.Text = "0"
            tbPemilikAkhir.Text = ""
            tbPemilikAwal.Text = ""
            tbRemarkDt.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub

    Private Sub Cleardt2()
        Try
            tbEquip.Text = ""
            tbEquipName.Text = ""
            tbQty.Text = "0"
            lblUnit.Text = ""
            tbremarkEquip.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt 2 Error " + ex.ToString)
        End Try
    End Sub

    Private Sub Cleardt3()
        Try
            tbWlNo.Text = ""
            tbPercilNoDt.Text = ""
            tbNameAwal.Text = ""
            tbWarisLevelNo.Text = ""
            tbLuasAwal.Text = "0"

            tbNamelvl1.Text = ""
            tbLuaslvl1.Text = "0"

            tbNamelvl2.Text = ""
            tbLuaslvl2.Text = "0"

            tbNamelvl3.Text = ""
            tbLuaslvl3.Text = "0"

            tbNamelvl4.Text = ""
            tbLuaslvl4.Text = "0"

            tbNamelvl5.Text = ""
            tbLuaslvl5.Text = "0"

            tbRemarkdt2.Text = ""

        Catch ex As Exception
            Throw New Exception("Clear Dt 3 Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt4()
        Try
            tbDatedt2.SelectedDate = ViewState("ServerDate")
            tbDescriptiondt2.Text = ""
            tbNoDok.Text = ""
            tbNameSubDt.Text = ""
            tbNameSubDt2.Text = ""
            tbNameSubDt3.Text = ""
            tbNameSubDt4.Text = ""
            tbNameSubDt5.Text = ""
            tbAJBSubDt.Text = ""
            tbAJBSubDt2.Text = ""
            tbAJBSubDt3.Text = ""
            tbAJBSubDt4.Text = ""
            tbAJBSubDt5.Text = ""
            tbLuasSubDt.Text = "0"
            tbLuasSubDt2.Text = "0"
            tbLuasSubDt3.Text = "0"
            tbLuasSubDt4.Text = "0"
            tbLuasSubDt5.Text = "0"
            tbSisaSubDt.Text = "0"
            tbSisaSubDt2.Text = "0"
            tbSisaSubDt3.Text = "0"
            tbSisaSubDt4.Text = "0"
            tbSisaSubDt5.Text = "0"
            tbremarkSubDt.Text = ""

        Catch ex As Exception
            Throw New Exception("Clear Dt 4 Error " + ex.ToString)
        End Try
    End Sub



    Function CekHd() As Boolean
        Try
            If tbDate.SelectedDate = Nothing Then
                lbStatus.Text = MessageDlg("Date must have value")
                tbDate.Focus()
                Return False
            End If
            If tbBlok.Text = "" Then
                lbStatus.Text = MessageDlg("Nomor Girik Blok must have value")
                tbBlok.Focus()
                Return False
            End If

            If tbKohir.Text = "" Then
                lbStatus.Text = MessageDlg("Nomor Girik Kohir must have value")
                tbKohir.Focus()
                Return False
            End If

            If tbPercil.Text = "" Then
                lbStatus.Text = MessageDlg("Nomor Girik Percil must have value")
                tbPercil.Focus()
                Return False
            End If

            'If tbAJB.Text = "" Then
            '    lbStatus.Text = MessageDlg("Nomor AJB must have value")
            '    tbAJB.Focus()
            '    Return False
            'End If

            'If tbSPH.Text = "" Then
            '    lbStatus.Text = MessageDlg("Nomor SPH must have value")
            '    tbSPH.Focus()
            '    Return False
            'End If

            'If tbSHM.Text = "" Then
            '    lbStatus.Text = MessageDlg("Nomor SHM must have value")
            '    tbSHM.Focus()
            '    Return False
            'End If

            'If ddlseller.SelectedValue = "" Then
            '    lbStatus.Text = MessageDlg("Seller must have value")
            '    ddlseller.Focus()
            '    Return False
            'End If


            If tbNoDokumen.Text = "" Then
                lbStatus.Text = MessageDlg("Nomor Sertifikat must have value")
                tbNoDokumen.Focus()
                Return False
            End If

            If ddlJenisDokumen.SelectedValue = "Choose One" Then
                lbStatus.Text = MessageDlg("Jenis Dokumen/Sertifikat not been selected")
                ddlJenisDokumen.Focus()
                Return False
            End If

            If tbTglTerbit.SelectedDate = Nothing Then
                lbStatus.Text = MessageDlg("Date of issue is not valid, please use this format (dd/mm/yyyy) OR (dd mm yyyy)")
                tbTglTerbit.Focus()
                Return False
            End If

            If ddlJenisDokumen.SelectedValue = "SHGB" Or ddlJenisDokumen.SelectedValue = "SHGU" Then
                If tbMasaBerlaku.SelectedDate = Nothing Then
                    lbStatus.Text = MessageDlg("Validity Period is not valid, please use this format (dd/mm/yyyy) OR (dd mm yyyy)")
                    tbMasaBerlaku.Focus()
                    Return False
                End If

            End If
          
            If tbSeller.Text = "" Then
                lbStatus.Text = MessageDlg("Seller must have value")
                tbSeller.Focus()
                Return False
            End If


            'If ddlModerator.SelectedValue = "" Then
            '    lbStatus.Text = MessageDlg("Modertor must have value")
            '    ddlModerator.Focus()
            '    Return False
            'End If

            If tbModerator.Text = "" Then
                lbStatus.Text = MessageDlg("Modertor must have value")
                tbModerator.Focus()
                Return False
            End If

            If tbArea.Text = "" Then
                lbStatus.Text = MessageDlg("Modertor must have value")
                tbArea.Focus()
                Return False
            End If

            If tbSPPT.Text = "" Then
                lbStatus.Text = MessageDlg("Luas SPPT must have value")
                tbSPPT.Focus()
                Return False
            End If

            If tbAjbSphShm.Text = "" Then
                lbStatus.Text = MessageDlg("Luas AJB/SPH/SHM must have value")
                tbAjbSphShm.Focus()
                Return False
            End If

            If tbNilai.Text = "" Then
                lbStatus.Text = MessageDlg("NIlai must have value")
                tbNilai.Focus()
                Return False
            End If


            If tbLuasUkur.Text = "" Then
                lbStatus.Text = MessageDlg("Luas Ukur must have value")
                tbLuasUkur.Focus()
                Return False
            End If

            'If tbNoDocHD.Text = "" Then
            '    lbStatus.Text = MessageDlg("No Dokumen must have value")
            '    tbNoDocHD.Focus()
            '    Return False
            'End If

            If tbPetaRincikNo.Text = "" Then
                lbStatus.Text = MessageDlg("No Peta Rincik must have value")
                tbPetaRincikNo.Focus()
                Return False
            End If


            If ddlHitungTotal.SelectedValue = "Choose One" Then
                lbStatus.Text = MessageDlg("Luas has not been selected")
                ddlHitungTotal.Focus()
                Return False
            End If

            If tbSptPbb.Text = "" Then
                lbStatus.Text = MessageDlg("SPPT/PBB must have value")
                ddlHitungTotal.Focus()
                Return False
            End If


            If ddlHitungTotal.SelectedValue = "SPPT" Then
                If tbTotal.Text.Replace(",", "") <> Val(tbSPPT.Text.Replace(",", "") * Val(tbNilai.Text.Replace(",", ""))) Then
                    lbStatus.Text = MessageDlg("Total must be the same from (Nilai * SPPT) Value")
                    tbTotal.Focus()
                    Return False
                End If
            End If


            If ddlHitungTotal.SelectedValue = "AJB/SPH/SHM" Then
                If tbTotal.Text.Replace(",", "") <> Val(tbAjbSphShm.Text.Replace(",", "") * Val(tbNilai.Text.Replace(",", ""))) Then
                    lbStatus.Text = MessageDlg("Total must be the same from (Nilai * AJB/SPH/SHM) Value")
                    tbTotal.Focus()
                    Return False
                End If
            End If

            If ddlHitungTotal.SelectedValue = "Luas Ukur" Then
                If tbTotal.Text.Replace(",", "") <> Val(tbLuasUkur.Text.Replace(",", "") * Val(tbNilai.Text.Replace(",", ""))) Then
                    lbStatus.Text = MessageDlg("Total must be the same from (Nilai * Luas Ukur) Value")
                    tbTotal.Focus()
                    Return False
                End If
            End If


            'If Right(FupMain.FileName, 4) <> ".pdf" Then
            '    lbStatus.Text = MessageDlg("Upload File PDF only .!!")
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
                If Dr("KetKegiatan").ToString = "" Then
                    lbStatus.Text = MessageDlg("Keterangan Kegiatan Must Have Value")
                    Return False
                End If
                If LTrim(Dr("Luas").ToString) = "" Then
                    lbStatus.Text = MessageDlg("Luas Must Have Value")
                    Return False
                End If

            Else
                If tbDateSurat.SelectedDate = Nothing Then
                    lbStatus.Text = MessageDlg("Date is not valid, please use this format (dd/mm/yyyy)")
                    tbDateSurat.Focus()
                    Return False
                End If

                If tbLuasDt.Text.Trim.Length = 0 Then
                    lbStatus.Text = MessageDlg("Luas must have value")
                    tbLuasDt.Focus()
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
                If Dr("Equipment").ToString = "" Then
                    lbStatus.Text = MessageDlg("Equipment Must Have Value")
                    Return False
                End If
                If Len(Dr("Qty").ToString) < 0 Then
                    lbStatus.Text = MessageDlg("Qty Equipment Must Have Value")
                    Return False
                End If

            Else
                If tbEquip.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Equipment must have value")
                    tbEquip.Focus()
                    Return False
                End If
                If tbQty.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Qty must have value")
                    tbQty.Focus()
                    Return False
                End If

            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt2 Error : " + ex.ToString)
        End Try
    End Function

    Function CekDt4(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not Dr Is Nothing Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("NoWl").ToString = "" Then
                    lbStatus.Text = MessageDlg("NoWl Must Have Value")
                    Return False
                End If
                ''If Dr("StartTime").ToString = "00:00" Then
                ''    lbStatus.Text = MessageDlg("Start Time Must Have Value")
                ''    Return False
                ''End If
                'If CFloat(Dr("Duration").ToString) <= 0 Then
                '    lbStatus.Text = MessageDlg("Duration Must Have Value")
                '    Return False
                'End If                                                                                                


            Else

                If tbDatedt2.SelectedDate = Nothing Then
                    lbStatus.Text = MessageDlg("Date is not valid, please use this format (dd/mm/yyyy)")
                    tbDatedt2.Focus()
                    Return False
                End If

                If tbDescriptiondt2.Text = "" Then
                    lbStatus.Text = MessageDlg("Description must have value")
                    tbDescriptiondt2.Focus()
                    Return False
                End If

                If tbNoDok.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("No Dokumen Must Have Value")
                    tbNoDok.Focus()
                    Return False
                End If

                If tbLuasSubDt.Text = "" Then
                    lbStatus.Text = MessageDlg("Luas must have value")
                    tbLuasSubDt.Focus()
                    Return False
                End If

                If tbLuasSubDt2.Text = "" Then
                    lbStatus.Text = MessageDlg("Luas must have value")
                    tbLuasSubDt2.Focus()
                    Return False
                End If

                If tbLuasSubDt3.Text = "" Then
                    lbStatus.Text = MessageDlg("Luas must have value")
                    tbLuasSubDt3.Focus()
                    Return False
                End If
                If tbLuasSubDt4.Text = "" Then
                    lbStatus.Text = MessageDlg("Luas  must have value")
                    tbLuasSubDt4.Focus()
                    Return False
                End If
                If tbLuasSubDt5.Text = "" Then
                    lbStatus.Text = MessageDlg("Luas must have value")
                    tbLuasSubDt5.Focus()
                    Return False
                End If

                If tbSisaSubDt.Text = "" Then
                    lbStatus.Text = MessageDlg("Sisa must have value")
                    tbSisaSubDt.Focus()
                    Return False
                End If

                If tbSisaSubDt2.Text = "" Then
                    lbStatus.Text = MessageDlg("Sisa must have value")
                    tbSisaSubDt2.Focus()
                    Return False
                End If
                If tbSisaSubDt3.Text = "" Then
                    lbStatus.Text = MessageDlg("Sisa must have value")
                    tbSisaSubDt3.Focus()
                    Return False
                End If
                If tbSisaSubDt4.Text = "" Then
                    lbStatus.Text = MessageDlg("Sisa must have value")
                    tbSisaSubDt4.Focus()
                    Return False
                End If
                If tbSisaSubDt5.Text = "" Then
                    lbStatus.Text = MessageDlg("Sisa must have value")
                    tbSisaSubDt5.Focus()
                    Return False
                End If
                'If CFloat(tbDuration.Text.Trim) <= 0 Then
                '    lbStatus.Text = MessageDlg("Duration Must Have Value")
                '    tbDuration.Focus()
                '    Return False
                'End If

            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt4 Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try

            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            tbCode.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToText(tbBlok, Dt.Rows(0)("Block").ToString)
            BindToText(tbKohir, Dt.Rows(0)("Kohir").ToString)
            BindToText(tbPercil, Dt.Rows(0)("Persil").ToString)
            BindToText(tbBNIB, Dt.Rows(0)("NoBNIB").ToString)
            BindToText(tbNoSuratUkur, Dt.Rows(0)("NoSuratUkur").ToString)
            BindToText(tbNoLainLian, Dt.Rows(0)("NoLainLain").ToString)
            BindToDropList(ddlJenisDokumen, Dt.Rows(0)("JnsDocSertifikat").ToString)
            BindToText(tbNoDokumen, Dt.Rows(0)("NoDocSertifikat").ToString)
            BindToDate(tbTglTerbit, Dt.Rows(0)("TglTerbit").ToString)
            BindToDate(tbMasaBerlaku, Dt.Rows(0)("MasaBerlaku").ToString)
            BindToDropList(ddlArea, Dt.Rows(0)("AreaCode").ToString)
            BindToDropList(ddlseller, Dt.Rows(0)("SellCode").ToString)
            BindToDropList(ddlHitungTotal, Dt.Rows(0)("LuasFrom").ToString)
            BindToDropList(ddlLandType, Dt.Rows(0)("LandType").ToString)
            BindToDropList(ddlModerator, Dt.Rows(0)("ModCode").ToString)
            BindToText(tbSptPbb, Dt.Rows(0)("PBBNo").ToString)
            BindToText(tbSPPT, Dt.Rows(0)("SPPT").ToString, ViewState("DigitCurr"))
            BindToText(tbAjbSphShm, Dt.Rows(0)("AjbSphShm").ToString, ViewState("DigitCurr"))
            BindToText(tbLuasUkur, Dt.Rows(0)("LuasUkur").ToString, ViewState("DigitCurr"))
            BindToText(tbNilai, Dt.Rows(0)("HrgTanah").ToString, ViewState("DigitCurr"))
            BindToText(tbHrgFix, Dt.Rows(0)("HrgFix").ToString, ViewState("DigitCurr"))
            BindToText(tbSPPT, Dt.Rows(0)("SPPT").ToString, ViewState("DigitCurr"))
            BindToText(tbTotal, Dt.Rows(0)("TtlHrgTanah").ToString, ViewState("DigitCurr"))
            BindToText(tbAddress, Dt.Rows(0)("Address").ToString)
            BindToText(tbProvinsi, Dt.Rows(0)("Provinsi").ToString)
            BindToText(tbKab, Dt.Rows(0)("Kab").ToString)
            BindToText(tbKec, Dt.Rows(0)("Kec").ToString)
            BindToText(tbDesa, Dt.Rows(0)("Desa").ToString)
            BindToText(tbPetaRincikNo, Dt.Rows(0)("PetaPercik").ToString)
            BindToDropList(ddlJenisDok, Dt.Rows(0)("JenisDoc").ToString)
            BindToText(tbNamaPembeli, Dt.Rows(0)("Pembeli").ToString)
            BindToText(tbNoDocHD, Dt.Rows(0)("NoDocHd").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
            BindToText(tbRemarkReject, Dt.Rows(0)("RemarkReject").ToString)

            BindToText(tbSeller, Dt.Rows(0)("SellCode").ToString)
            BindToText(tbSellerName, Dt.Rows(0)("SellName").ToString)

            BindToText(tbArea, Dt.Rows(0)("AreaCode").ToString)
            BindToText(tbAreaName, Dt.Rows(0)("AreaName").ToString)

            BindToText(tbModerator, Dt.Rows(0)("ModCode").ToString)
            BindToText(tbModeratorName, Dt.Rows(0)("ModName").ToString)


            If Dt.Rows(0)("LainLain").ToString = "" Then
                tbLainLain.Text = "Lain - Lain"
            Else
                BindToText(tbLainLain, Dt.Rows(0)("LainLain").ToString)
            End If

            If Dt.Rows(0)("LainLain2").ToString = "" Then
                tbLainLain2.Text = "Lain - Lain"
            Else
                BindToText(tbLainLain2, Dt.Rows(0)("LainLain2").ToString)
            End If

            If Dt.Rows(0)("LainLain3").ToString = "" Then
                tbLainLain3.Text = "Lain - Lain"
            Else
                BindToText(tbLainLain3, Dt.Rows(0)("LainLain3").ToString)
            End If

            '1
            If Dt.Rows(0)("DocKtp").ToString = "" Then
                cbKtp.Checked = False
                lbKtp.Text = "Not Yet Uploaded"
            Else
                lbKtp.Text = Dt.Rows(0)("DocKtp").ToString
                cbKtp.Checked = True
            End If

            '2
            If Dt.Rows(0)("DocKK").ToString = "" Then
                cbKk.Checked = False
                lbKK.Text = "Not Yet Uploaded"
            Else
                lbKK.Text = Dt.Rows(0)("DocKK").ToString
                cbKk.Checked = True
            End If

            '3
            If Dt.Rows(0)("DocSPPT").ToString = "" Then
                cbSPPT.Checked = False
                lbSPPT.Text = "Not Yet Uploaded"
            Else
                lbSPPT.Text = Dt.Rows(0)("DocSPPT").ToString
                cbSPPT.Checked = True
            End If

            '4
            If Dt.Rows(0)("DocSTTS").ToString = "" Then
                cbSTTS.Checked = False
                lbSTTS.Text = "Not Yet Uploaded"
            Else
                lbSTTS.Text = Dt.Rows(0)("DocSTTS").ToString
                cbSTTS.Checked = True
            End If

            '5
            If Dt.Rows(0)("DocTTD").ToString = "" Then
                cbTTD.Checked = False
                lbTTD.Text = "Not Yet Uploaded"
            Else
                lbTTD.Text = Dt.Rows(0)("DocTTD").ToString
                cbTTD.Checked = True
            End If

            '6
            If Dt.Rows(0)("DocAJB").ToString = "" Then
                cbAJB.Checked = False
                lbAJB.Text = "Not Yet Uploaded"
            Else
                lbAJB.Text = Dt.Rows(0)("DocAJB").ToString
                cbAJB.Checked = True
            End If

            '6
            If Dt.Rows(0)("DocAJB2").ToString = "" Then
                cbAJB2.Checked = False
                lbAJB2.Text = "Not Yet Uploaded"
            Else
                lbAJB2.Text = Dt.Rows(0)("DocAJB2").ToString
                cbAJB2.Checked = True
            End If

            '7
            If Dt.Rows(0)("DocSSP").ToString = "" Then
                cbSSP.Checked = False
                lbSSP.Text = "Not Yet Uploaded"
            Else
                lbSSP.Text = Dt.Rows(0)("DocSSP").ToString
                cbSSP.Checked = True
            End If

            '8
            If Dt.Rows(0)("DocSSD").ToString = "" Then
                cbSSD.Checked = False
                lbSSD.Text = "Not Yet Uploaded"
            Else
                lbSSD.Text = Dt.Rows(0)("DocSSD").ToString
                cbSSD.Checked = True
            End If

            '9
            If Dt.Rows(0)("DocKTS").ToString = "" Then
                cbSKTS.Checked = False
                lbSKTS.Text = "Not Yet Uploaded"
            Else
                lbSKTS.Text = Dt.Rows(0)("DocKTS").ToString
                cbSKTS.Checked = True
            End If

            '10
            If Dt.Rows(0)("DocSBPT").ToString = "" Then
                cbSPBT.Checked = False
                lbSPBT.Text = "Not Yet Uploaded"
            Else
                lbSPBT.Text = Dt.Rows(0)("DocSBPT").ToString
                cbSPBT.Checked = True
            End If

            '11
            If Dt.Rows(0)("DocSPKTT").ToString = "" Then
                cbSPKTT.Checked = False
                lbSPKTT.Text = "Not Yet Uploaded"
            Else
                lbSPKTT.Text = Dt.Rows(0)("DocSPKTT").ToString
                cbSPKTT.Checked = True
            End If

            '12
            If Dt.Rows(0)("DocBAM").ToString = "" Then
                cbBAM.Checked = False
                lbBAM.Text = "Not Yet Uploaded"
            Else
                lbBAM.Text = Dt.Rows(0)("DocBAM").ToString
                cbBAM.Checked = True
            End If

            '13
            If Dt.Rows(0)("DocBAPL").ToString = "" Then
                cbBAPL.Checked = False
                lbBAPL.Text = "Not Yet Uploaded"
            Else
                lbBAPL.Text = Dt.Rows(0)("DocBAPL").ToString
                cbBAPL.Checked = True
            End If

            '14
            If Dt.Rows(0)("DocPTPP").ToString = "" Then
                cbPTPP.Checked = False
                lbPTPP.Text = "Not Yet Uploaded"
            Else
                lbPTPP.Text = Dt.Rows(0)("DocPTPP").ToString
                cbPTPP.Checked = True
            End If

            '15
            If Dt.Rows(0)("DocPTPP2").ToString = "" Then
                cbPTPP2.Checked = False
                lbPtPP2.Text = "Not Yet Uploaded"
            Else
                lbPtPP2.Text = Dt.Rows(0)("DocPTPP2").ToString
                cbPTPP2.Checked = True
            End If

            '16
            If Dt.Rows(0)("DocSKRT").ToString = "" Then
                cbSKRT.Checked = False
                lbSKRT.Text = "Not Yet Uploaded"
            Else
                lbSKRT.Text = Dt.Rows(0)("DocSKRT").ToString
                cbSKRT.Checked = True
            End If

            '17
            If Dt.Rows(0)("DocSKD").ToString = "" Then
                cbSKD.Checked = False
                lbSKD.Text = "Not Yet Uploaded"
            Else
                lbSKD.Text = Dt.Rows(0)("DocSKD").ToString
                cbSKD.Checked = True
            End If

            '18
            If Dt.Rows(0)("DocFcGirik").ToString = "" Then
                cbFCGirik.Checked = False
                lbFCGirik.Text = "Not Yet Uploaded"
            Else
                lbFCGirik.Text = Dt.Rows(0)("DocFcGirik").ToString
                cbFCGirik.Checked = True
            End If

            '19
            If Dt.Rows(0)("DocFDP").ToString = "" Then
                cbFDP.Checked = False
                lbFDP.Text = "Not Yet Uploaded"
            Else
                lbFDP.Text = Dt.Rows(0)("DocFDP").ToString
                cbFDP.Checked = True
            End If

            '20
            If Dt.Rows(0)("DocPatok").ToString = "" Then
                cbPatok.Checked = False
                lbPatok.Text = "Not Yet Uploaded"
            Else
                lbPatok.Text = Dt.Rows(0)("DocPatok").ToString
                cbPatok.Checked = True
            End If

            '21
            If Dt.Rows(0)("DocSporadik").ToString = "" Then
                cbSporadik.Checked = False
                lbSporadik.Text = "Not Yet Uploaded"
            Else
                lbSporadik.Text = Dt.Rows(0)("DocSporadik").ToString
                cbSporadik.Checked = True
            End If

            '22
            If Dt.Rows(0)("DocAHU").ToString = "" Then
                cbAHU.Checked = False
                lbAHU.Text = "Not Yet Uploaded"
            Else
                lbAHU.Text = Dt.Rows(0)("DocAHU").ToString
                cbAHU.Checked = True
            End If

            '23
            If Dt.Rows(0)("DoCSejarah").ToString = "" Then
                cbSejarah.Checked = False
                lbSejarah.Text = "Not Yet Uploaded"
            Else
                lbSejarah.Text = Dt.Rows(0)("DocAHU").ToString
                cbSejarah.Checked = True
            End If

            '24
            If Dt.Rows(0)("DocPTPP2").ToString = "" Then
                lbPtPP2.Text = "Not Yet Uploaded"
                'lbPtPP2.ForeColor = Drawing.Color.Red
            Else

                lbPtPP2.Text = Dt.Rows(0)("DocPTPP2").ToString
            End If


            '25
            If Dt.Rows(0)("DoCSPJH").ToString = "" Then
                cbSPJH.Checked = False
                lbSPJH.Text = "Not Yet Uploaded"
            Else
                lbSPJH.Text = Dt.Rows(0)("DoCSPJH").ToString
                cbSPJH.Checked = True
            End If

            '26
            If Dt.Rows(0)("DoclainLain").ToString = "" Then
                cbLainLain.Checked = False
                lbLainLain.Text = "Not Yet Uploaded"
            Else
                lbLainLain.Text = Dt.Rows(0)("DoclainLain").ToString
                cbLainLain.Checked = True
            End If


            '27
            If Dt.Rows(0)("DoCKTPW").ToString = "" Then
                cbKtpW.Checked = False
                lbKtpW.Text = "Not Yet Uploaded"
            Else
                lbKtpW.Text = Dt.Rows(0)("DoCKTPW").ToString
                cbKtpW.Checked = True
            End If

            '28
            If Dt.Rows(0)("DoCSPW").ToString = "" Then
                cbSPW.Checked = False
                lbSPW.Text = "Not Yet Uploaded"
            Else
                lbSPW.Text = Dt.Rows(0)("DoCSPW").ToString
                cbSPW.Checked = True
            End If

            '29
            If Dt.Rows(0)("DoCSPKW").ToString = "" Then
                cbSPKW.Checked = False
                lbSPKW.Text = "Not Yet Uploaded"
            Else
                lbSPKW.Text = Dt.Rows(0)("DoCSPKW").ToString
                cbSPKW.Checked = True
            End If

            '30
            If Dt.Rows(0)("DocBPJS").ToString = "" Then
                cbBPJS.Checked = False
                lbBPJS.Text = "Not Yet Uploaded"
            Else
                lbBPJS.Text = Dt.Rows(0)("DocBPJS").ToString
                cbBPJS.Checked = True
            End If


            If Dt.Rows(0)("DoclainLain2").ToString = "" Then
                cbLainLain2.Checked = False
                lbLainLain2.Text = "Not Yet Uploaded"
            Else
                lbLainLain2.Text = Dt.Rows(0)("DoclainLain2").ToString
                cbLainLain2.Checked = True
            End If

            If Dt.Rows(0)("DoclainLain3").ToString = "" Then
                cbLainLain3.Checked = False
                lbLainLain3.Text = "Not Yet Uploaded"
            Else
                lbLainLain3.Text = Dt.Rows(0)("DoclainLain3").ToString
                cbLainLain3.Checked = True
            End If





        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal RRNo As String, ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("ItemNo = " + QuotedStr(ItemNo))
            If Dr.Length > 0 Then
                lbItemNo.Text = ItemNo.ToString
                BindToText(tbKetKegiatan, Dr(0)("KetKegiatan").ToString)
                BindToText(tbNoSurat, Dr(0)("NoSurat").ToString)
                BindToDate(tbDateSurat, Dr(0)("DateSurat").ToString)
                BindToText(tbLuasDt, Dr(0)("Luas").ToString, ViewState("DigitCurr"))
                BindToText(tbPemilikAkhir, Dr(0)("NameAkhir").ToString)
                BindToText(tbPemilikAwal, Dr(0)("NameAwal").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt2(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt2").select("Equipment = " + QuotedStr(ItemNo))
            If Dr.Length > 0 Then
                BindToText(tbEquip, Dr(0)("Equipment").ToString)
                BindToText(tbEquipName, Dr(0)("EquipmentName").ToString)
                BindToText(tbQty, Dr(0)("Qty").ToString)
                lblUnit.Text = Dr(0)("Unit").ToString
                BindToText(tbremarkEquip, Dr(0)("Remark").ToString)

            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail 2 error : " + ex.ToString)
        End Try
    End Sub


    Protected Sub FillTextBoxDt3(ByVal NoWl As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt3").select("NoWl = " + QuotedStr(NoWl))
            If Dr.Length > 0 Then
                BindToText(tbWlNo, Dr(0)("NoWl").ToString)
                BindToText(tbPercilNoDt, Dr(0)("NoPercilDt2").ToString)
                BindToText(tbWarisLevelNo, Dr(0)("NoWarisLevel").ToString)
                BindToText(tbNameAwal, Dr(0)("AwalName").ToString)
                BindToText(tbLuasAwal, Dr(0)("LuasAwal").ToString, ViewState("FormatNumber"))
                BindToText(tbNamelvl1, Dr(0)("WlLvlName1").ToString)
                BindToText(tbNamelvl2, Dr(0)("WlLvlName2").ToString)
                BindToText(tbNamelvl3, Dr(0)("WlLvlName3").ToString)
                BindToText(tbNamelvl4, Dr(0)("WlLvlName4").ToString)
                BindToText(tbNamelvl5, Dr(0)("WlLvlName5").ToString)

                BindToText(tbLuaslvl1, Dr(0)("LuasLvlNo1").ToString, ViewState("FormatNumber"))
                BindToText(tbLuaslvl2, Dr(0)("LuasLvlNo2").ToString, ViewState("FormatNumber"))
                BindToText(tbLuaslvl3, Dr(0)("LuasLvlNo3").ToString, ViewState("FormatNumber"))
                BindToText(tbLuaslvl4, Dr(0)("LuasLvlNo4").ToString, ViewState("FormatNumber"))
                BindToText(tbLuaslvl5, Dr(0)("LuasLvlNo5").ToString, ViewState("FormatNumber"))
                BindToText(tbRemarkdt2, Dr(0)("Remark").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail 3 error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxDt4(ByVal NoDok As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt4").select("NoDok = " + QuotedStr(NoDok))
            If Dr.Length > 0 Then
                BindToText(tbNoDok, Dr(0)("NoDok").ToString)
                BindToText(tbDescriptiondt2, Dr(0)("Descript").ToString)
                BindToDate(tbDatedt2, Dr(0)("DocDate").ToString)
                BindToText(tbNameSubDt, Dr(0)("Name").ToString)
                BindToText(tbAJBSubDt, Dr(0)("NoAjb").ToString)
                BindToText(tbLuasSubDt, Dr(0)("Luas").ToString, ViewState("Formatstring"))
                BindToText(tbSisaSubDt, Dr(0)("Sisa").ToString, ViewState("Formatstring"))

                BindToText(tbNameSubDt2, Dr(0)("Name2").ToString)
                BindToText(tbAJBSubDt2, Dr(0)("NoAjb2").ToString)
                BindToText(tbLuasSubDt2, Dr(0)("Luas2").ToString, ViewState("Formatstring"))
                BindToText(tbSisaSubDt2, Dr(0)("Sisa2").ToString, ViewState("Formatstring"))

                BindToText(tbNameSubDt3, Dr(0)("Name3").ToString)
                BindToText(tbAJBSubDt3, Dr(0)("NoAjb3").ToString)
                BindToText(tbLuasSubDt3, Dr(0)("Luas3").ToString, ViewState("Formatstring"))
                BindToText(tbSisaSubDt3, Dr(0)("Sisa3").ToString, ViewState("Formatstring"))

                BindToText(tbNameSubDt4, Dr(0)("Name4").ToString)
                BindToText(tbAJBSubDt4, Dr(0)("NoAjb4").ToString)
                BindToText(tbLuasSubDt4, Dr(0)("Luas4").ToString, ViewState("Formatstring"))
                BindToText(tbSisaSubDt4, Dr(0)("Sisa4").ToString, ViewState("Formatstring"))

                BindToText(tbNameSubDt5, Dr(0)("Name5").ToString)
                BindToText(tbAJBSubDt5, Dr(0)("NoAjb5").ToString)
                BindToText(tbLuasSubDt5, Dr(0)("Luas5").ToString, ViewState("Formatstring"))
                BindToText(tbSisaSubDt5, Dr(0)("Sisa5").ToString, ViewState("Formatstring"))

                BindToText(tbremarkSubDt, Dr(0)("Remark").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail 4 error : " + ex.ToString)
        End Try
    End Sub


    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveDt.Click
        Try
            If CekDt() = False Then
                btnSaveDt.Focus()
                Exit Sub
            End If

            If ViewState("StateDt") = "Edit" Then
                Dim Row As DataRow
                Row = ViewState("Dt").Select("ItemNo = " + lbItemNo.Text)(0)
                Row.BeginEdit()
                Row("KetKegiatan") = tbKetKegiatan.Text
                Row("NoSurat") = tbNoSurat.Text
                Row("DateSurat") = tbDateSurat.Text
                Row("Luas") = CInt(tbLuasDt.Text)
                Row("NameAwal") = tbPemilikAwal.Text
                Row("NameAkhir") = tbPemilikAkhir.Text
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()
            Else
                If CekExistData(ViewState("Dt"), "ItemNo", lbItemNo.Text) = True Then
                    lbStatus.Text = "ItemNo " + lbItemNo.Text + " has been already exist"
                    Exit Sub
                End If
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("ItemNo") = CInt(lbItemNo.Text)
                dr("KetKegiatan") = tbKetKegiatan.Text
                dr("NoSurat") = tbNoSurat.Text
                dr("DateSurat") = tbDateSurat.Text
                dr("Luas") = CInt(tbLuasDt.Text)
                dr("NameAwal") = tbPemilikAwal.Text
                dr("NameAkhir") = tbPemilikAkhir.Text
                dr("Remark") = tbRemarkDt.Text
                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, pnlDt)
            'GridDt.Columns(1).Visible = True

            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0)
            BindGridDt(ViewState("Dt"), GridDt)
            StatusButtonSave(True)

        Catch ex As Exception
            lbStatus.Text = "btn save Dt Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub

    Protected Sub btnSaveDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveDt2.Click
        Try

            If CekDt2() = False Then
                btnSaveDt2.Focus()
                Exit Sub
            End If

            If ViewState("StateDt2") = "Edit" Then
                Dim Row As DataRow
                Row = ViewState("Dt2").Select("Equipment = " + QuotedStr(ViewState("Dt2Value")))(0)
                Row.BeginEdit()
                Row("Equipment") = tbEquip.Text
                Row("EquipmentName") = tbEquipName.Text
                Row("Qty") = FormatFloat(tbQty.Text, ViewState("DigitQty"))
                Row("Unit") = lblUnit.Text
                Row("Remark") = tbremarkEquip.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt2").NewRow
                dr("Equipment") = tbEquip.Text
                dr("EquipmentName") = tbEquipName.Text
                dr("Qty") = FormatFloat(tbQty.Text, ViewState("DigitQty"))
                dr("Unit") = lblUnit.Text
                dr("Remark") = tbremarkEquip.Text
                ViewState("Dt2").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt2, pnlDt2)
            'EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0 = 0)
            BindGridDt(ViewState("Dt2"), GridDt2)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save dt2 Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub

    Protected Sub btnSaveDt3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt3.Click
        Try
            If CekDt3() = False Then
                btnSaveDt3.Focus()
                Exit Sub
            End If

            Dim Total As String
            Total = Val(tbLuaslvl1.Text.Replace(",", "")) + Val(tbLuaslvl2.Text.Replace(",", "")) + Val(tbLuaslvl3.Text.Replace(",", "")) + Val(tbLuaslvl4.Text.Replace(",", "")) + Val(tbLuaslvl5.Text.Replace(",", ""))

            If tbLuasAwal.Text.Replace(",", "") <> Total Then
                lbStatus.Text = MessageDlg("Total luas waris Level " + Total + ",  must be the same with Luas Awal " + tbLuasAwal.Text + "")
                btnSaveDt3.Focus()
                Exit Sub

            End If

            'JobPlant, Team, StartDate,EndDate, Qty, Unit, WorkDay, Capacity, Person
            If ViewState("StateDt3") = "Edit" Then
                Dim Row As DataRow
                Row = ViewState("Dt3").Select("NoWl = " + QuotedStr(ViewState("Dt3Value")))(0)
                Row.BeginEdit()
                Row("NoWl") = tbWlNo.Text
                Row("NoPercilDt2") = tbPercilNoDt.Text
                Row("NoWarisLevel") = tbWarisLevelNo.Text
                Row("AwalName") = tbNameAwal.Text
                Row("LuasAwal") = tbLuasAwal.Text
                Row("WlLvlName1") = tbNamelvl1.Text
                Row("WlLvlName2") = tbNamelvl2.Text
                Row("WlLvlName3") = tbNamelvl3.Text
                Row("WlLvlName4") = tbNamelvl4.Text
                Row("WlLvlName5") = tbNamelvl5.Text
                Row("LuasLvlNo1") = tbLuaslvl1.Text
                Row("LuasLvlNo2") = tbLuaslvl2.Text
                Row("LuasLvlNo3") = tbLuaslvl3.Text
                Row("LuasLvlNo4") = tbLuaslvl4.Text
                Row("LuasLvlNo5") = tbLuaslvl5.Text
                Row("Remark") = tbRemarkdt2.Text
                Row.EndEdit()
            Else


                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt3").NewRow
                dr("NoWl") = tbWlNo.Text
                dr("NoPercilDt2") = tbPercilNoDt.Text
                dr("NoWarisLevel") = tbWarisLevelNo.Text
                dr("AwalName") = tbNameAwal.Text
                dr("LuasAwal") = tbLuasAwal.Text
                dr("WlLvlName1") = tbNamelvl1.Text
                dr("WlLvlName2") = tbNamelvl2.Text
                dr("WlLvlName3") = tbNamelvl3.Text
                dr("WlLvlName4") = tbNamelvl4.Text
                dr("WlLvlName5") = tbNamelvl5.Text
                dr("LuasLvlNo1") = tbLuaslvl1.Text
                dr("LuasLvlNo2") = tbLuaslvl2.Text
                dr("LuasLvlNo3") = tbLuaslvl3.Text
                dr("LuasLvlNo4") = tbLuaslvl4.Text
                dr("LuasLvlNo5") = tbLuaslvl5.Text
                dr("Remark") = tbRemarkdt2.Text
                ViewState("Dt3").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt3, pnlDt3)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
            GridDt3.Columns(1).Visible = GetCountRecord(ViewState("Dt3")) > 0
            BindGridDt(ViewState("Dt3"), GridDt3)
            StatusButtonSave(True)
            btnSaveAll.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn save Dt3 Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub

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
                tbCode.Text = GetAutoNmbr("LS", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)

                'Dim path, namafile As String
                'path = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + Format(Now, "-yyMMddHHmmss-") + FupMain.FileName
                'namafile = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + Format(Now, "-yyMMddHHmmss-") + FupMain.FileName


                SQLString = "INSERT INTO GLLandPurchaseReqHd (TransNmbr,	TransDate,	Status,	Block,	Kohir ,	Persil , AJBNo, SPHNo, SHMNo, SellCode, ModCode,  " + _
                "SPPT, AjbSphShm, LuasUkur, PBBNo,HrgFix, HrgTanah, TtlHrgTanah, Address, Provinsi, Kab, Kec, Desa, PetaPercik, JenisDoc, Pembeli, NoDocHd, " + _
                "Remark,AreaCode, LuasFrom, LandType, JnsDocSertifikat, NoDocSertifikat,TglTerbit, MasaBerlaku,NoBNIB,NoSuratUkur, NoLainLain, UserPrep, DatePrep ) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", 'H', " + _
                QuotedStr(tbBlok.Text) + "," + QuotedStr(tbKohir.Text) + ", " + QuotedStr(tbPercil.Text) + ", " + _
                QuotedStr(tbAJB.Text) + "," + QuotedStr(tbSPH.Text) + ", " + QuotedStr(tbSHM.Text) + ", " + _
                QuotedStr(tbSeller.Text) + "," + QuotedStr(tbModerator.Text) + "," + _
                QuotedStr(tbSPPT.Text.Replace(",", "")) + "," + QuotedStr(tbAjbSphShm.Text.Replace(",", "")) + ", " + QuotedStr(tbLuasUkur.Text.Replace(",", "")) + "," + QuotedStr(tbSptPbb.Text) + ", " + _
                QuotedStr(tbHrgFix.Text.Replace(",", "")) + "," + QuotedStr(tbNilai.Text.Replace(",", "")) + "," + QuotedStr(tbTotal.Text.Replace(",", "")) + ", " + QuotedStr(tbAddress.Text) + ", " + _
                QuotedStr(tbProvinsi.Text) + ", " + QuotedStr(tbKab.Text) + "," + QuotedStr(tbKec.Text) + "," + QuotedStr(tbDesa.Text) + ", " + _
                QuotedStr(tbPetaRincikNo.Text) + ", " + QuotedStr(ddlJenisDok.SelectedValue) + "," + QuotedStr(tbNamaPembeli.Text) + "," + QuotedStr(tbNoDocHD.Text) + ", " + _
                QuotedStr(tbRemark.Text) + "," + QuotedStr(tbArea.Text) + "," + QuotedStr(ddlHitungTotal.SelectedValue) + "," + QuotedStr(ddlLandType.SelectedValue) + "," + _
                QuotedStr(ddlJenisDokumen.SelectedValue) + "," + QuotedStr(tbNoDokumen.Text) + "," + QuotedStr(Format(tbTglTerbit.SelectedValue, "yyyy-MM-dd")) + "," + QuotedStr(Format(tbMasaBerlaku.SelectedValue, "yyyy-MM-dd")) + "," + _
                QuotedStr(tbBNIB.Text) + "," + QuotedStr(tbNoSuratUkur.Text) + "," + QuotedStr(tbNoLainLian.Text) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", GetDate()"
                ViewState("TransNmbr") = tbCode.Text

                'FupMain.SaveAs(path)

            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM GLLandPurchaseReqHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed. Data has already posted by another user")
                    Exit Sub
                End If


                SQLString = "UPDATE GLLandPurchaseReqHd SET TransDate = " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + _
                ", Block = " + QuotedStr(tbBlok.Text) + _
                ", LandType = " + QuotedStr(ddlLandType.SelectedValue) + _
                ", Kohir = " + QuotedStr(tbKohir.Text) + _
                ", Persil = " + QuotedStr(tbPercil.Text) + _
                ", JnsDocSertifikat = " + QuotedStr(ddlJenisDokumen.SelectedValue) + _
                ", NoDocSertifikat = " + QuotedStr(tbNoDokumen.Text) + _
                ", TglTerbit = " + QuotedStr(Format(tbTglTerbit.SelectedValue, "yyyy-MM-dd")) + _
                ", MasaBerlaku = " + QuotedStr(Format(tbMasaBerlaku.SelectedValue, "yyyy-MM-dd")) + _
                ", NoBNIB = " + QuotedStr(tbBNIB.Text) + _
                ", NoSuratUkur = " + QuotedStr(tbNoSuratUkur.Text) + _
                ", NoLainLain = " + QuotedStr(tbNoLainLian.Text) + _
                ", AreaCode = " + QuotedStr(tbArea.Text) + _
                ", SellCode = " + QuotedStr(tbSeller.Text) + _
                ", ModCode = " + QuotedStr(tbModerator.Text) + _
                ", PBBNo = " + QuotedStr(tbSptPbb.Text) + _
                ", SPPT = " + QuotedStr(tbSPPT.Text.Replace(",", "")) + _
                ", AjbSphShm = " + QuotedStr(tbAjbSphShm.Text.Replace(",", "")) + _
                ", LuasUkur = " + QuotedStr(tbLuasUkur.Text.Replace(",", "")) + _
                ", HrgFix = " + QuotedStr(tbHrgFix.Text.Replace(",", "")) + _
                ", HrgTanah = " + QuotedStr(tbNilai.Text.Replace(",", "")) + _
                ", TtlHrgTanah = " + QuotedStr(tbTotal.Text.Replace(",", "")) + _
                ", Address = " + QuotedStr(tbAddress.Text) + _
                ", Provinsi = " + QuotedStr(tbProvinsi.Text) + _
                ", Kab = " + QuotedStr(tbKab.Text) + _
                ", Kec = " + QuotedStr(tbKec.Text) + _
                ", Desa = " + QuotedStr(tbDesa.Text) + _
                ", PetaPercik = " + QuotedStr(tbPetaRincikNo.Text) + _
                ", JenisDoc = " + QuotedStr(ddlJenisDok.Text) + _
                ", Pembeli = " + QuotedStr(tbNamaPembeli.Text) + _
                ", NoDocHD = " + QuotedStr(tbNoDocHD.Text) + _
                ", Remark = " + QuotedStr(tbRemark.Text) + _
                ", LuasFrom = " + QuotedStr(ddlHitungTotal.SelectedValue) + _
                ", DatePrep = GetDate()" + _
                " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            End If

            SQLString = Replace(SQLString, "''", "NULL")
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

            Row = ViewState("Dt3").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbCode.Text
                Row(I).EndEdit()
            Next

            Row = ViewState("Dt4").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbCode.Text
                Row(I).EndEdit()
            Next


            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, ItemNo, KetKegiatan, NoSurat, DateSurat, Luas, NameAwal, NameAkhir, Remark FROM GLLandPurchaseReqDt WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
                    "UPDATE GLLandPurchaseReqDt SET KetKegiatan = @KetKegiatan, NoSurat = @NoSurat, DateSurat = @DateSurat, Luas = @Luas, NameAwal = @NameAwal, NameAkhir = @NameAkhir, Remark = @Remark " + _
                    "WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND ItemNo = @OldItemNo  ", con)
            ' Define output parameters.
            Update_Command.Parameters.Add("@KetKegiatan", SqlDbType.VarChar, 30, "KetKegiatan")
            Update_Command.Parameters.Add("@NoSurat", SqlDbType.VarChar, 30, "NoSurat")
            Update_Command.Parameters.Add("@DateSurat", SqlDbType.DateTime, 8, "DateSurat")
            Update_Command.Parameters.Add("@Luas", SqlDbType.Float, 18, "Luas")
            Update_Command.Parameters.Add("@NameAwal", SqlDbType.VarChar, 500, "NameAwal")
            Update_Command.Parameters.Add("@NameAkhir", SqlDbType.VarChar, 500, "NameAkhir")
            Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 225, "Remark")

            ' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@OldItemNo", SqlDbType.Int, 5, "ItemNo")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM GLLandPurchaseReqDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND ItemNo = @ItemNo", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@ItemNo", SqlDbType.VarChar, 5, "ItemNo")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("GLLandPurchaseReqDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt

            'save dt2
            'cmdSql = New SqlCommand("SELECT TransNmbr, Equipment, Qty, Unit, Remark FROM PLPlanLandDt WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            'da = New SqlDataAdapter(cmdSql)
            'dbcommandBuilder = New SqlCommandBuilder(da)
            'da.InsertCommand = dbcommandBuilder.GetInsertCommand
            ''da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            ''da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            'Dim param2 As SqlParameter
            '' Create the UpdateCommand.
            'Dim Update_Command2 = New SqlCommand( _
            '        "UPDATE PLPlanLandDt SET Equipment = @Equipment, Qty = @Qty, Unit = @Unit, Remark = @Remark  " + _
            '        " WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND Equipment = @OldEquipment ", con)
            '' Define output parameters.
            'Update_Command2.Parameters.Add("@Equipment", SqlDbType.VarChar, 20, "Equipment")
            'Update_Command2.Parameters.Add("@Qty", SqlDbType.Float, 18, "Qty")
            'Update_Command2.Parameters.Add("@Unit", SqlDbType.VarChar, 5, "Unit")
            'Update_Command2.Parameters.Add("@Remark", SqlDbType.VarChar, 60, "Remark")

            '' Define intput (WHERE) parameters.
            'param2 = Update_Command2.Parameters.Add("@OldEquipment", SqlDbType.VarChar, 20, "Equipment")
            'param2.SourceVersion = DataRowVersion.Original
            '' Attach the update command to the DataAdapter.
            'da.UpdateCommand = Update_Command2

            '' Create the DeleteCommand.
            'Dim Delete_Command2 = New SqlCommand( _
            '    "DELETE FROM PLPlanLandDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND Equipment = @Equipment", con)
            '' Add the parameters for the DeleteCommand.
            'param2 = Delete_Command2.Parameters.Add("@Equipment", SqlDbType.VarChar, 20, "Equipment")
            'param2.SourceVersion = DataRowVersion.Original
            'da.DeleteCommand = Delete_Command2

            'Dim Dt2 As New DataTable("PLPlanLandDt")

            'Dt2 = ViewState("Dt2")
            'da.Update(Dt2)
            'Dt2.AcceptChanges()
            'ViewState("Dt2") = Dt2

            'save dt3
            cmdSql = New SqlCommand("SELECT TransNmbr, NoWL, NoPercilDt2, AwalName, LuasAwal, WlLvlName1, WlLvlName2, WlLvlName3, WlLvlName4, WlLvlName5, LuasLvlNo1, LuasLvlNo2, LuasLvlNo3, LuasLvlNo4, LuasLvlNo5, NoWarisLevel, Remark FROM GLLandPurchaseReqDt2 WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            dbcommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim param3 As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command3 = New SqlCommand( _
                    "UPDATE GLLandPurchaseReqDt2 SET NoPercilDt2 = @NoPercilDt2, AwalName = @AwalName, LuasAwal = @LuasAwal, " + _
                    "WlLvlName1 = @WlLvlName1 , WlLvlName2 = @WlLvlName2, WlLvlName3 = @WlLvlName3, WlLvlName4 =@WlLvlName4, WlLvlName5 = @WlLvlName5, " + _
                    "LuasLvlNo1=@LuasLvlNo1, LuasLvlNo2=@LuasLvlNo2, LuasLvlNo3=@LuasLvlNo3, LuasLvlNo4 = @LuasLvlNo4, LuasLvlNo5 = @LuasLvlNo5 , " + _
                    "NoWarisLevel=@NoWarisLevel , " + _
                    "Remark = @Remark " + _
                    "WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND NoWL = @OldNoWL ", con)

            ' Define output parameters.
            Update_Command3.Parameters.Add("@NoPercilDt2", SqlDbType.VarChar, 30, "NoPercilDt2")
            Update_Command3.Parameters.Add("@AwalName", SqlDbType.VarChar, 50, "AwalName")
            Update_Command3.Parameters.Add("@LuasAwal", SqlDbType.Float, 18, "LuasAwal")
            Update_Command3.Parameters.Add("@WlLvlName1", SqlDbType.VarChar, 50, "WlLvlName1")
            Update_Command3.Parameters.Add("@WlLvlName2", SqlDbType.VarChar, 50, "WlLvlName2")
            Update_Command3.Parameters.Add("@WlLvlName3", SqlDbType.VarChar, 50, "WlLvlName3")
            Update_Command3.Parameters.Add("@WlLvlName4", SqlDbType.VarChar, 50, "WlLvlName4")
            Update_Command3.Parameters.Add("@WlLvlName5", SqlDbType.VarChar, 50, "WlLvlName5")
            Update_Command3.Parameters.Add("@LuasLvlNo1", SqlDbType.Float, 18, "LuasLvlNo1")
            Update_Command3.Parameters.Add("@LuasLvlNo2", SqlDbType.Float, 18, "LuasLvlNo2")
            Update_Command3.Parameters.Add("@LuasLvlNo3", SqlDbType.Float, 18, "LuasLvlNo3")
            Update_Command3.Parameters.Add("@LuasLvlNo4", SqlDbType.Float, 18, "LuasLvlNo4")
            Update_Command3.Parameters.Add("@LuasLvlNo5", SqlDbType.Float, 18, "LuasLvlNo5")
            Update_Command3.Parameters.Add("@NoWarisLevel", SqlDbType.VarChar, 5, "NoWarisLevel")
            Update_Command3.Parameters.Add("@Remark", SqlDbType.VarChar, 225, "Remark")

            ' Define intput (WHERE) parameters.
            param3 = Update_Command3.Parameters.Add("@OldNoWL", SqlDbType.VarChar, 30, "NoWL")
            param3.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command3

            ' Create the DeleteCommand.
            Dim Delete_Command3 = New SqlCommand( _
                "DELETE FROM GLLandPurchaseReqDt2 WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND NoWL = @NoWL", con)
            ' Add the parameters for the DeleteCommand.
            param3 = Delete_Command3.Parameters.Add("@NoWL", SqlDbType.VarChar, 40, "NoWL")
            param3.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command3

            Dim Dt3 As New DataTable("GLLandPurchaseReqDt2")

            Dt3 = ViewState("Dt3")
            da.Update(Dt3)
            Dt3.AcceptChanges()
            ViewState("Dt3") = Dt3

            'save dt4
            Dim SQLStringProcess As String
            If Not ViewState("NoWl") Is Nothing Then
                SQLStringProcess = "SELECT TransNmbr, NoWl, NoDok, Descript, Docdate, Name, NoAjb, Luas, Sisa, Name2, NoAjb2, Luas2, Sisa2, Name3, NoAjb3, Luas3, Sisa3 " + _
                ", Name4 , NoAjb4, Luas4, Sisa4,Name5, NoAjb5, Luas5, Sisa5, Remark FROM GLLandPurchaseReqSubDt2 WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr"))
                cmdSql = New SqlCommand(SQLStringProcess, con)
                da = New SqlDataAdapter(cmdSql)
                dbcommandBuilder = New SqlCommandBuilder(da)
                da.InsertCommand = dbcommandBuilder.GetInsertCommand

                da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
                da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

                'Dim param4 As SqlParameter
                '' Create the UpdateCommand.
                'Dim Update_Command4 = New SqlCommand( _
                '        "UPDATE GLLandPurchaseReqSubDt2 SET Name = @Name, Descript = @Descript, Docdate = @Docdate, NoAjb = @NoAjb, @Luas = @Luas, Sisa = @Sisa, " + _
                '        "Name2 = @Name2, NoAjb2 = @NoAjb2, @Luas2 = @Luas2, Sisa2 = @Sisa2,Name3 = @Name3, NoAjb3 = @NoAjb3, @Luas3 = @Luas3, Sisa3 = @Sisa3,  " + _
                '        "Name4 = @Name4,NoAjb4 = @NoAjb4, @Luas4 = @Luas4, Sisa4 = @Sisa4, Name5 = @Name5 ,NoAjb5 = @NoAjb5, @Luas5 = @Luas5, Sisa5 = @Sisa5, Remark = @Remark " + _
                '        " WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND NoWl = @OldNoWl AND NoDok = @OldNoDok ", con)
                '' Define output parameters.
                'Update_Command4.Parameters.Add("@Descript", SqlDbType.VarChar, 50, "Descript")
                'Update_Command4.Parameters.Add("@Docdate", SqlDbType.DateTime, 30, "Docdate")

                'Update_Command4.Parameters.Add("@Name", SqlDbType.VarChar, 30, "Name")
                'Update_Command4.Parameters.Add("@NoAjb", SqlDbType.VarChar, 30, "NoAjb")
                'Update_Command4.Parameters.Add("@Luas", SqlDbType.Float, 18, "Luas")
                'Update_Command4.Parameters.Add("@Sisa", SqlDbType.Float, 18, "Sisa")

                'Update_Command4.Parameters.Add("@Name2", SqlDbType.VarChar, 30, "Name2")
                'Update_Command4.Parameters.Add("@NoAjb2", SqlDbType.VarChar, 30, "NoAjb2")
                'Update_Command4.Parameters.Add("@Luas2", SqlDbType.Float, 18, "Luas2")
                'Update_Command4.Parameters.Add("@Sisa2", SqlDbType.Float, 18, "Sisa2")

                'Update_Command4.Parameters.Add("@Name3", SqlDbType.VarChar, 30, "Name3")
                'Update_Command4.Parameters.Add("@NoAjb3", SqlDbType.VarChar, 30, "NoAjb3")
                'Update_Command4.Parameters.Add("@Luas3", SqlDbType.Float, 18, "Luas3")
                'Update_Command4.Parameters.Add("@Sisa3", SqlDbType.Float, 18, "Sisa3")

                'Update_Command4.Parameters.Add("@Name4", SqlDbType.VarChar, 30, "Name4")
                'Update_Command4.Parameters.Add("@NoAjb4", SqlDbType.VarChar, 30, "NoAjb4")
                'Update_Command4.Parameters.Add("@Luas4", SqlDbType.Float, 18, "Luas4")
                'Update_Command4.Parameters.Add("@Sisa4", SqlDbType.Float, 18, "Sisa4")

                'Update_Command4.Parameters.Add("@Name5", SqlDbType.VarChar, 30, "Name5")
                'Update_Command4.Parameters.Add("@NoAjb5", SqlDbType.VarChar, 30, "NoAjb5")
                'Update_Command4.Parameters.Add("@Luas5", SqlDbType.Float, 18, "Luas5")
                'Update_Command4.Parameters.Add("@Sisa5", SqlDbType.Float, 18, "Sisa5")

                'Update_Command4.Parameters.Add("@Remark", SqlDbType.VarChar, 225, "Remark")

                '' Define intput (WHERE) parameters.
                'param4 = Update_Command4.Parameters.Add("@OldNoWl", SqlDbType.VarChar, 30, "NoWl")
                'param4 = Update_Command4.Parameters.Add("@OldNoDok", SqlDbType.VarChar, 30, "NoDok")
                'param4.SourceVersion = DataRowVersion.Original
                '' Attach the update command to the DataAdapter.
                'da.UpdateCommand = Update_Command4

                '' Create the DeleteCommand.
                'Dim Delete_Command4 = New SqlCommand( _
                '    "DELETE FROM GLLandPurchaseReqSubDt2 WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND NoWl = @NoWl AND NoDok = @NoDok ", con)
                '' Add the parameters for the DeleteCommand.
                'param4 = Delete_Command4.Parameters.Add("@NoWl", SqlDbType.VarChar, 30, "NoWl")
                'param4 = Delete_Command4.Parameters.Add("@NoDok", SqlDbType.VarChar, 20, "NoDok")
                'param4.SourceVersion = DataRowVersion.Original
                'da.DeleteCommand = Delete_Command4

                Dim Dt4 As New DataTable("GLLandPurchaseReqSubDt2")

                Dt4 = ViewState("Dt4")
                da.Update(Dt4)
                Dt4.AcceptChanges()
                ViewState("Dt4") = Dt4
            End If

        Catch ex As Exception
            Throw New Exception("Save All Data Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSaveTrans_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveTrans.Click
        Dim CurrFilter, Value As String
        Dim I As Integer

        Try

            If CekHd() = False Then
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail 'Item' must have at least 1 record")
                Exit Sub
            End If
            For Each dr In ViewState("Dt").Rows
                If CekDt(dr) = False Then
                    Exit Sub
                End If
            Next
            'If GetCountRecord(ViewState("Dt3")) = 0 Then
            '    lbStatus.Text = MessageDlg("Detail 'Material Waste' must have at least 1 record")
            '    Exit Sub
            'End If
            'If GetCountRecord(ViewState("Dt4")) = 0 Then
            '    lbStatus.Text = MessageDlg("Detail 'Machine Down Time' must have at least 1 record")
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

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            ModifyInput2(True, pnlInput, pnlDt, GridDt)
            'ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
            ModifyInput2(True, pnlInput, pnlDt3, GridDt3)
            ModifyInput2(True, pnlInput, PnlDt4, GridDt4)

            GridDt3.Columns(0).Visible = False

            MultiView2.Visible = True
            MultiView2.ActiveViewIndex = 0
            Menu2.Visible = True
            Menu2.Items.Item(0).Selected = True
            Menu1.Visible = True

            newTrans()
            btnHome.Visible = False
            MultiView1.ActiveViewIndex = 0
            Menu1.Items.Item(0).Selected = True
            GridDt.Columns(1).Visible = True
            MovePanel(PnlHd, pnlInput)
            EnableHd(True)
            EnableButtonView(False)
            tbBlok.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddDt.Click, btnAddDtKe2.Click
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
            btnSaveDt.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnAddDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddDt2.Click, btnAddDt2ke2.Click
        Try
            Cleardt2()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt2") = "Insert"
            MovePanel(pnlDt2, pnlEditDt2)
            EnableHd(False)
            StatusButtonSave(False)
            tbEquip.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt2 error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ViewState("TransNmbr") = ""
            ClearHd()
            Cleardt()
            Cleardt2()
            pnlDt.Visible = True
            BindDataDt("")
            'BindDataDt2("")
            BindDataDt3("")
            BindDataDt4("")
            EnableHd(True)
            btnGoEdit.Visible = False
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
    Protected Sub LbAdvSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LbAdvSearch.Click
        Dim FDateName, FDateValue, FilterName, FilterValue As String
        Try
            FDateName = "Report Date"
            FDateValue = "TransDate"
            FilterName = "Trasn No, Trasb Date, Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), Remark"
            Session("DateFieldName") = FDateName.Split(",")
            Session("DateFieldValue") = FDateValue.Split(",")
            Session("FieldName") = FilterName.Split(",")
            Session("FieldValue") = FilterValue.Split(",")
            AttachScript("OpenFilterCriteria()", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "Advanced Search Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        GridView1.PageIndex = e.NewPageIndex
        BindData(Session("AdvanceFilter"))
    End Sub


    Protected Sub GridDt3_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridDt3.RowCommand
        Try
            If e.CommandName = "Insert" Then
                btnAddDt3_Click(Nothing, Nothing)
            ElseIf e.CommandName = "DetailMaterial" Then
                Dim GVR As GridViewRow
                GVR = GridDt3.Rows(Convert.ToInt32(e.CommandArgument))
                MultiView1.ActiveViewIndex = 3
                lbNoWl.Text = GVR.Cells(2).Text

                lbwaris1.Text = GVR.Cells(7).Text
                lbwaris2.Text = GVR.Cells(9).Text
                lbwaris3.Text = GVR.Cells(11).Text
                lbwaris4.Text = GVR.Cells(13).Text
                lbwaris5.Text = GVR.Cells(15).Text

                tbLuas1.Text = GVR.Cells(8).Text
                tbLuas2.Text = GVR.Cells(10).Text
                tbLuas3.Text = GVR.Cells(12).Text
                tbLuas4.Text = GVR.Cells(14).Text
                tbLuas5.Text = GVR.Cells(16).Text

                ViewState("FormulaNo") = GVR.Cells(6).Text
                ViewState("NoWl") = GVR.Cells(2).Text
                lbNoWl.Text = ViewState("NoWl")
                Dim drow As DataRow()


               
                If ViewState("Dt4") Is Nothing Then
                    BindDataDt4(ViewState("TransNmbr"))
                Else
                    drow = ViewState("Dt4").Select("NoWl = " + QuotedStr(GVR.Cells(2).Text))
                    If drow.Length > 0 Then
                        BindGridDt(drow.CopyToDataTable, GridDt4)
                        GridDt4.Columns(0).Visible = Not ViewState("StateHd") = "View"
                        btnAdddt4.Visible = Not ViewState("StateHd") = "View"


                    Else
                        Dim DtTemp As DataTable
                        DtTemp = ViewState("Dt4").Clone
                        DtTemp.Rows.Add(DtTemp.NewRow())
                        GridDt4.DataSource = DtTemp
                        GridDt4.DataBind()
                        GridDt4.Columns(0).Visible = False
                        btnAdddt4.Visible = Not ViewState("StateHd") = "View"
                        btnAddDt4ke2.Visible = Not ViewState("StateHd") = "View"
                    End If

                    btnSaveAll.Visible = False
                    btnSaveTrans.Visible = False
                    btnBack.Visible = False

                    btnHome.Visible = False
                    btnBackDt2.Focus()
                End If

            End If
        Catch ex As Exception
            lbStatus.Text = "GridProcess_RowCommand Error : " + ex.ToString
        End Try

    End Sub


    Protected Sub btnBackDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBackDt.Click, btnBackDt2.Click
        MultiView1.ActiveViewIndex = 2
        btnSaveAll.Visible = Not (ViewState("StateHd") = "View")
        btnSaveTrans.Visible = Not (ViewState("StateHd") = "View")
        btnBack.Visible = Not (ViewState("StateHd") = "View")
        btnHome.Visible = (ViewState("StateHd") = "View")
    End Sub

    Protected Sub btnBackDt2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBackDt.Click, btnBackDt2.Click
        MultiView1.ActiveViewIndex = 2
        btnSaveAll.Visible = Not (ViewState("StateHd") = "View")
        btnSaveTrans.Visible = Not (ViewState("StateHd") = "View")
        btnBack.Visible = Not (ViewState("StateHd") = "View")
        btnHome.Visible = (ViewState("StateHd") = "View")
    End Sub

    Private Sub Cek()
        Dim Hd As DataTable

        'Hd = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

        'If Hd.Rows(0)("DocKtp").ToString <> "" Then
        '    cbKtp.Checked = False
        '    FupMain.Visible = True
        'End If

        'If Hd.Rows(0)("DocKK").ToString <> "" Then
        '    cbKk.Checked = False
        '    FubKK.Visible = True
        'End If
        'If Hd.Rows(0)("DocSPPT").ToString <> "" Then
        '    cbSPPT.Checked = False
        '    FubSPPT.Visible = True
        'End If

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
                    BindDataDt3(ViewState("TransNmbr"))
                    BindDataDt4(ViewState("TransNmbr"))

                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, pnlDt, GridDt)
                    ModifyInput2(False, pnlInput, pnlDt2, GridDt2)
                    ModifyInput2(False, pnlInput, pnlDt3, GridDt3)
                    ModifyInput2(False, pnlInput, PnlDt4, GridDt4)
    
                    MultiView1.ActiveViewIndex = 0
                    Menu1.Items.Item(0).Selected = True
                    btnHome.Visible = True
                    GridDt.Columns(1).Visible = True
                    EnableButtonView(True)
                    EnableHd(False)
                    Cek()
                    btnGoEdit.Visible = True

                ElseIf DDL.SelectedValue = "Edit" Then
                    If GVR.Cells(3).Text = "H" Or GVR.Cells(3).Text = "G" Or GVR.Cells(3).Text = "I" Then
                        CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        MovePanel(PnlHd, pnlInput)
                        'Dim Division As String
                        'Division = SQLExecuteScalar("EXEC S_GetPlantDivisionAll " + QuotedStr(ViewState("UserId")), ViewState("DBConnection"))

                        ViewState("TransNmbr") = GVR.Cells(2).Text
                        GridDt.PageIndex = 0
                        BindDataDt(ViewState("TransNmbr"))
                        BindDataDt2(ViewState("TransNmbr"))
                        BindDataDt3(ViewState("TransNmbr"))
                        BindDataDt4(ViewState("TransNmbr"))

                        FillTextBoxHd(ViewState("TransNmbr"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
                        ModifyInput2(True, pnlInput, pnlDt3, GridDt3)
                        ModifyInput2(True, pnlInput, PnlDt4, GridDt4)

                        btnHome.Visible = False
                        MultiView1.ActiveViewIndex = 0
                        Menu1.Items.Item(0).Selected = True
                        'GridDt.Columns(1).Visible = True
                        EnableHd(True)
                        btnGoEdit.Visible = False
                        'EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0)
                    Else
                        lbStatus.Text = MessageDlg("Data must be Hold or Get Approval to edit")
                        Exit Sub
                    End If

                ElseIf DDL.SelectedValue = "Reject" Then

                    If GVR.Cells(3).Text = "P" Then
                        lbStatus.Text = MessageDlg("Status Land Survey Is not H or G, cannot Reject Survey")
                        Exit Sub
                    End If

                    If GVR.Cells(3).Text = "R" Then
                        lbStatus.Text = MessageDlg("Land Survey Reject Already")
                        Exit Sub
                    End If

                    index = Convert.ToInt32(e.CommandArgument)
                    GVR = GridView1.Rows(index)

                    ViewState("TransNmbr") = GVR.Cells(2).Text
                    AttachScript("Reject();", Page, Me.GetType)

                    'End If
                    
                    ''lbStatus.Text = HiddenRemarkReject.Value
                    'Dim ResultCek, SqlStringCek, Result, SqlString, Value As String

                    ''klik
                    'If HiddenRemarkReject.Value <> "False Value" Or HiddenRemarkReject.Value <> "" Then
                    '    'AttachScript("Reject();", Page, Me.GetType)
                    '    If HiddenRemarkReject.Value <> "False Value" Or HiddenRemarkReject.Value <> "" Then
                    '        AttachScript("Reject();", Page, Me.GetType)
                    '        ResultCek = HiddenRemarkReject.Value
                    '        lbStatus.Text = HiddenRemarkReject.Value

                    '        SqlString = "Update GLLandPurchaseReqHd Set Status = 'H', remark = " + QuotedStr(HiddenRemarkReject.Value) + " WHERE Transnmbr = " + QuotedStr(GVR.Cells(2).Text) + " "
                    '        Result = SQLExecuteScalar(SqlString, ViewState("DBConnection"))

                    '        Value = ddlField.SelectedValue
                    '        tbFilter.Text = GVR.Cells(2).Text
                    '        ddlField.SelectedValue = "TransNmbr"
                    '        btnSearch_Click(Nothing, Nothing)
                    '        'tbFilter.Text = CurrFilter
                    '        ddlField.SelectedValue = Value
                    '        tbFilter.Text = ""

                    '        'Result = Result.Replace("0", "")

                    '        'If Trim(Result) <> "" Then
                    '        '    lbStatus.Text = MessageDlg(Result)
                    '        'End If
                    '        ' ResultCek = ""
                    '    Else
                    '        lbStatus.Text = MessageDlg("Remark reject must have value")
                    '        HiddenRemarkReject.Value = ""
                    '        Exit Sub
                    '    End If

                    'End If




                ElseIf DDL.SelectedValue = "Print Check Doc" Then
                    Try
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        Session("SelectCommand") = "EXEC S_PRLandOfferingSurvey ''" + QuotedStr(GVR.Cells(2).Text) + "'', '0'," + QuotedStr(ViewState("UserId"))
                        Session("ReportFile") = ".../../../Rpt/FormChecklistDokumen.frx"
                        Session("DBConnection") = ViewState("DBConnection")
                        AttachScript("openprintdlg()", Page, Me.GetType)
                    Catch ex As Exception
                        lbStatus.Text = "btn print Error = " + ex.ToString
                    End Try

                ElseIf DDL.SelectedValue = "Print Riwayat" Then
                    Try
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        Session("SelectCommand") = "EXEC S_PRLandOfferingSurvey ''" + QuotedStr(GVR.Cells(2).Text) + "'', '1'," + QuotedStr(ViewState("UserId"))
                        Session("ReportFile") = ".../../../Rpt/FormHistory.frx"
                        Session("DBConnection") = ViewState("DBConnection")
                        AttachScript("openprintdlg()", Page, Me.GetType)
                    Catch ex As Exception
                        lbStatus.Text = "btn print Error = " + ex.ToString
                    End Try

                ElseIf DDL.SelectedValue = "Print Letter C" Then
                    Try
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        Session("SelectCommand") = "EXEC S_PRLandOfferingSurvey ''" + QuotedStr(GVR.Cells(2).Text) + "'', '2'," + QuotedStr(ViewState("UserId"))
                        Session("ReportFile") = ".../../../Rpt/FormLetterC.frx"
                        Session("DBConnection") = ViewState("DBConnection")
                        AttachScript("openprintdlg()", Page, Me.GetType)
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
            Dim GVR As GridViewRow
            If e.CommandName = "View" Then
                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))
                '  lbWODt2.Text = GVR.Cells(2).Text
                MultiView1.ActiveViewIndex = 1
                Dim drow As DataRow()
                If ViewState("Dt2") Is Nothing Then
                    BindDataDt2(ViewState("TransNmbr"))
                Else
                    drow = ViewState("Dt2").Select("Block = " + QuotedStr(GVR.Cells(2).Text))
                    If drow.Length > 0 Then
                        BindGridDt(drow.CopyToDataTable, GridDt2)
                        GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
                    Else
                        Dim DtTemp As DataTable
                        DtTemp = ViewState("Dt2").Clone
                        DtTemp.Rows.Add(DtTemp.NewRow())
                        GridDt2.DataSource = DtTemp
                        GridDt2.DataBind()
                        GridDt2.Columns(0).Visible = False
                    End If
                End If
            ElseIf e.CommandName = "ViewA" Then
                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))
                MultiView1.ActiveViewIndex = 1
                Dim drow As DataRow()
                If ViewState("Dt2") Is Nothing Then
                    BindDataDt2(ViewState("TransNmbr"))
                Else
                    drow = ViewState("Dt2").Select("Equipment = " + QuotedStr(GVR.Cells(2).Text))
                    If drow.Length > 0 Then
                        BindGridDt(drow.CopyToDataTable, GridDt2)
                        GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
                    Else
                        Dim DtTemp As DataTable
                        DtTemp = ViewState("Dt2").Clone
                        DtTemp.Rows.Add(DtTemp.NewRow())
                        GridDt2.DataSource = DtTemp
                        GridDt2.DataBind()
                        GridDt2.Columns(0).Visible = False
                    End If
                End If
            ElseIf e.CommandName = "ViewE" Then
                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))
                MultiView1.ActiveViewIndex = 2

                Dim drow As DataRow()
                If ViewState("Dt3") Is Nothing Then
                    BindDataDt3(ViewState("TransNmbr"))
                Else
                    drow = ViewState("Dt3").Select("JobPlant = " + QuotedStr(GVR.Cells(2).Text))
                    If drow.Length > 0 Then
                        BindGridDt(drow.CopyToDataTable, GridDt3)
                        GridDt3.Columns(0).Visible = Not ViewState("StateHd") = "View"
                    Else
                        Dim DtTemp As DataTable
                        DtTemp = ViewState("Dt3").Clone
                        DtTemp.Rows.Add(DtTemp.NewRow())
                        GridDt3.DataSource = DtTemp
                        GridDt3.DataBind()
                        GridDt3.Columns(0).Visible = False
                    End If
                End If
            ElseIf e.CommandName = "ViewM" Then

                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))
                MultiView1.ActiveViewIndex = 3

                Dim drow As DataRow()
                If ViewState("Dt4") Is Nothing Then
                    BindDataDt4(ViewState("TransNmbr"))
                Else
                    drow = ViewState("Dt4").Select("Material = " + QuotedStr(GVR.Cells(2).Text))
                    If drow.Length > 0 Then
                        BindGridDt(drow.CopyToDataTable, GridDt4)
                        GridDt4.Columns(0).Visible = Not ViewState("StateHd") = "View"
                    Else
                        Dim DtTemp As DataTable
                        DtTemp = ViewState("Dt4").Clone
                        DtTemp.Rows.Add(DtTemp.NewRow())
                        GridDt4.DataSource = DtTemp
                        GridDt4.DataBind()
                        GridDt4.Columns(0).Visible = False
                    End If
                End If
            End If
        Catch ex As Exception
            lbStatus.Text = lbStatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub
    'Dim TQtyWO As Double = 0
    'Dim TQLuas As Double = 0
    'Dim MaxItem As Integer = 0
    'Protected Sub GridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt.RowDataBound
    '    Try
    '        If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "Block")) Then
    '            If e.Row.RowType = DataControlRowType.DataRow Then
    '                MaxItem = MaxItem + 1
    '                TQtyWO = GetTotalSum(ViewState("Dt"), "QtyTarget")
    '                TQLuas = GetTotalSum(ViewState("Dt"), "Area")
    '            ElseIf e.Row.RowType = DataControlRowType.Footer Then
    '            End If
    '        End If
    '        tbQtyTarget.Text = TQtyWO
    '        tbAreal.Text = TQLuas

    '    Catch ex As Exception
    '        lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
    '    End Try
    'End Sub

    'Protected Sub GridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt.RowDataBound
    '    Try

    '    Catch ex As Exception
    '        lbStatus.Text = "GridDt_RowDataBound Error : " & ex.ToString
    '    End Try
    'End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Try
            Dim dr As DataRow()
            Dim r, drt As DataRow
            Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
            dr = ViewState("Dt").Select("ItemNo = " + QuotedStr(GVR.Cells(1).Text))
            For Each r In dr
                r.Delete()
            Next

            'For i = 0 To GetCountRecord(ViewState("Dt2")) - 1
            '    drt = ViewState("Dt2").Rows(i)
            '    If Not drt.RowState = DataRowState.Deleted Then
            '        drt.Delete()
            '    End If
            'Next

            BindGridDt(ViewState("Dt"), GridDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Dim TQty As Double = 0
    Protected Sub GridDt2_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt2.RowDataBound
        Try
            'If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "Equipment")) Then
            '    If e.Row.RowType = DataControlRowType.DataRow Then
            '        TQty = GetTotalSum(ViewState("Dt2"), "Qty")
            '    ElseIf e.Row.RowType = DataControlRowType.Footer Then
            '    End If
            'End If
            'tbQty.Text = TQty

        Catch ex As Exception
            lbStatus.Text = "GridDt2_RowDataBound Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub GridDt2_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt2.RowDeleting
    '    Try
    '        Dim dr() As DataRow
    '        Dim GVR As GridViewRow
    '        GVR = GridDt2.Rows(e.RowIndex)
    '        dr = ViewState("Dt2").Select("Equipment = " + QuotedStr(GVR.Cells(1).Text))
    '        dr(0).Delete()
    '        BindGridDt(ViewState("Dt2"), GridDt2)
    '        'dr = ViewState("Dt2").Select("Equipment = " + QuotedStr(GVR.Cells(1).Text))
    '        'If dr.Length > 0 Then
    '        '    BindGridDt(dr.CopyToDataTable, GridDt2)
    '        '    GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
    '        'Else
    '        '    Dim DtTemp As DataTable
    '        '    DtTemp = ViewState("Dt2").Clone
    '        '    DtTemp.Rows.Add(DtTemp.NewRow())
    '        '    GridDt2.DataSource = DtTemp
    '        '    GridDt2.DataBind()
    '        '    GridDt2.Columns(0).Visible = False
    '        'End If

    '        EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
    '    Catch ex As Exception
    '        lbStatus.Text = "Grid Dt 2 Row Deleting Error : " + ex.ToString
    '    End Try
    'End Sub


    Protected Sub GridDt2_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt2.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt2.Rows(e.NewEditIndex)
            FillTextBoxDt2(GVR.Cells(1).Text)
            ViewState("Dt2Value") = GVR.Cells(1).Text
            MovePanel(pnlDt2, pnlEditDt2)
            EnableHd(False)
            ViewState("StateDt2") = "Edit"
            StatusButtonSave(False)
            btnSaveDt2.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid dt2 Editing Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnSaveAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveAll.Click
        Dim CurrFilter, Value As String
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail  must have at least 1 record")
                Exit Sub
            End If

            SaveAll()
            MovePanel(pnlInput, PnlHd)
            CurrFilter = tbFilter.Text
            Value = ddlField.SelectedValue
            tbFilter.Text = tbCode.Text
            ddlField.SelectedValue = "TransNmbr"
            btnSearch_Click(Nothing, Nothing)
            tbFilter.Text = CurrFilter
            ddlField.SelectedValue = Value
            BtnAdd_Click(Nothing, Nothing)
        Catch ex As Exception
            lbStatus.Text = "btnSaveTrans Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnAddDt3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt3.Click, btnAddDt3ke2.Click
        Try
            Cleardt3()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt3") = "Insert"
            MovePanel(pnlDt3, pnlEditDt3)
            EnableHd(False)
            StatusButtonSave(False)
            btnSaveDt3.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt3 error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancelDt3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelDt3.Click
        Try
            MovePanel(pnlEditDt3, pnlDt3)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt3 Error : " + ex.ToString
        End Try
    End Sub


    Function CekDt3(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not Dr Is Nothing Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("NoWl").ToString = "" Then
                    lbStatus.Text = MessageDlg("Wl No Must Have Value")
                    Return False
                End If
                If Dr("NoPercilDt2").ToString = "" Then
                    lbStatus.Text = MessageDlg("Percil Must Have Value")
                    Return False
                End If

            Else

                

                If tbWlNo.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("WlNo Must Have Value")
                    tbWlNo.Focus()
                    Return False
                End If

                If tbPercilNoDt.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Percil Dt Must Have Value")
                    tbPercilNoDt.Focus()
                    Return False
                End If

                If tbNameAwal.Text = "" Then
                    lbStatus.Text = MessageDlg("Nama Must Have Value")
                    tbNameAwal.Focus()
                    Return False
                End If
                If tbLuasAwal.Text = "" Then
                    lbStatus.Text = MessageDlg("Luas Awal Must Have Value")
                    tbLuasAwal.Focus()
                    Return False
                End If

                If tbLuaslvl1.Text = "" Then
                    lbStatus.Text = MessageDlg("Luas Level 1 Must Have Value")
                    tbLuaslvl1.Focus()
                    Return False
                End If
                If tbLuaslvl2.Text = "" Then
                    lbStatus.Text = MessageDlg("Luas level 2 Must Have Value")
                    tbLuaslvl2.Focus()
                    Return False
                End If
                If tbLuaslvl3.Text = "" Then
                    lbStatus.Text = MessageDlg("Luas level 3 Must Have Value")
                    tbLuaslvl3.Focus()
                    Return False
                End If
                If tbLuaslvl4.Text = "" Then
                    lbStatus.Text = MessageDlg("Luas Level 4 Must Have Value")
                    tbLuaslvl4.Focus()
                    Return False
                End If
                If tbLuaslvl5.Text = "" Then
                    lbStatus.Text = MessageDlg("Luas Level 5 Must Have Value")
                    tbLuaslvl5.Focus()
                    Return False
                End If


            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function
    Dim TQtyB As Double = 0
    Protected Sub GridDt3_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt3.RowDataBound
        Try
            'If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "JobPlant")) Then
            '    If e.Row.RowType = DataControlRowType.DataRow Then
            '        TQtyB = GetTotalSum(ViewState("Dt3"), "Qty")
            '    ElseIf e.Row.RowType = DataControlRowType.Footer Then
            '    End If
            'End If
            'tbQtyJobPlant.Text = TQtyB


        Catch ex As Exception
            lbStatus.Text = "GridDt3_RowDataBound Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt3_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt3.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow
            GVR = GridDt3.Rows(e.RowIndex)
            dr = ViewState("Dt3").Select("NoWl = " + QuotedStr(GVR.Cells(2).Text))
            dr(0).Delete()
            BindGridDt(ViewState("Dt3"), GridDt3)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt 3 Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt3_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt3.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt3.Rows(e.NewEditIndex)
            FillTextBoxDt3(GVR.Cells(2).Text)
            ViewState("Dt3Value") = GVR.Cells(2).Text
            MovePanel(pnlDt3, pnlEditDt3)
            EnableHd(False)
            ViewState("StateDt3") = "Edit"
            StatusButtonSave(False)
            btnSaveDt3.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid dt3 Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveDt4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt4.Click
        Try
            If CekDt4() = False Then
                btnSaveDt4.Focus()
                Exit Sub
            End If
            If ViewState("StateDt4") = "Edit" Then
                Dim Row As DataRow
                Row = ViewState("Dt4").Select("NoDok = " + QuotedStr(ViewState("Dt4Value")))(0)
                Row.BeginEdit()
                Row("NoDok") = tbNoDok.Text
                Row("Descript") = tbDescriptiondt2.Text
                Row("DocDate") = tbDatedt2.Text
                Row("Name") = tbNameSubDt.Text
                Row("Name2") = tbNameSubDt2.Text
                Row("Name3") = tbNameSubDt3.Text
                Row("Name4") = tbNameSubDt4.Text
                Row("Name5") = tbNameSubDt5.Text
                Row("NoAjb") = tbAJBSubDt.Text
                Row("NoAjb2") = tbAJBSubDt2.Text
                Row("NoAjb3") = tbAJBSubDt3.Text
                Row("NoAjb4") = tbAJBSubDt4.Text
                Row("NoAjb5") = tbAJBSubDt5.Text
                Row("Luas") = FormatFloat(tbLuasSubDt.Text, ViewState("DigitQty"))
                Row("Luas2") = FormatFloat(tbLuasSubDt2.Text, ViewState("DigitQty"))
                Row("Luas3") = FormatFloat(tbLuasSubDt3.Text, ViewState("DigitQty"))
                Row("Luas4") = FormatFloat(tbLuasSubDt4.Text, ViewState("DigitQty"))
                Row("Luas5") = FormatFloat(tbLuasSubDt5.Text, ViewState("DigitQty"))
                Row("Sisa") = FormatFloat(tbSisaSubDt.Text, ViewState("DigitQty"))
                Row("Sisa2") = FormatFloat(tbSisaSubDt2.Text, ViewState("DigitQty"))
                Row("Sisa3") = FormatFloat(tbSisaSubDt3.Text, ViewState("DigitQty"))
                Row("Sisa4") = FormatFloat(tbSisaSubDt4.Text, ViewState("DigitQty"))
                Row("Sisa5") = FormatFloat(tbSisaSubDt5.Text, ViewState("DigitQty"))
                Row("Remark") = tbremarkSubDt.Text
                Row.EndEdit()
            Else
                If CekExistData(ViewState("Dt4"), "NoDok", tbNoDok.Text) Then
                    lbStatus.Text = MessageDlg("item detail '" + tbNoDok.Text + "' has already exists")
                    Exit Sub
                End If

                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt4").NewRow
                dr("NoWl") = lbNoWl.Text
                dr("NoDok") = tbNoDok.Text
                dr("Descript") = tbDescriptiondt2.Text
                dr("DocDate") = tbDatedt2.Text
                dr("Name") = tbNameSubDt.Text
                dr("Name2") = tbNameSubDt2.Text
                dr("Name3") = tbNameSubDt3.Text
                dr("Name4") = tbNameSubDt4.Text
                dr("Name5") = tbNameSubDt5.Text
                dr("NoAjb") = tbAJBSubDt.Text
                dr("NoAjb2") = tbAJBSubDt2.Text
                dr("NoAjb3") = tbAJBSubDt3.Text
                dr("NoAjb4") = tbAJBSubDt4.Text
                dr("NoAjb5") = tbAJBSubDt5.Text
                dr("Luas") = FormatFloat(tbLuasSubDt.Text, ViewState("DigitQty"))
                dr("Luas2") = FormatFloat(tbLuasSubDt2.Text, ViewState("DigitQty"))
                dr("Luas3") = FormatFloat(tbLuasSubDt3.Text, ViewState("DigitQty"))
                dr("Luas4") = FormatFloat(tbLuasSubDt4.Text, ViewState("DigitQty"))
                dr("Luas5") = FormatFloat(tbLuasSubDt5.Text, ViewState("DigitQty"))
                dr("Sisa") = FormatFloat(tbSisaSubDt.Text, ViewState("DigitQty"))
                dr("Sisa2") = FormatFloat(tbSisaSubDt2.Text, ViewState("DigitQty"))
                dr("Sisa3") = FormatFloat(tbSisaSubDt3.Text, ViewState("DigitQty"))
                dr("Sisa4") = FormatFloat(tbSisaSubDt4.Text, ViewState("DigitQty"))
                dr("Sisa5") = FormatFloat(tbSisaSubDt5.Text, ViewState("DigitQty"))
                dr("Remark") = tbremarkSubDt.Text
                ViewState("Dt4").Rows.Add(dr)
                End If
                MovePanel(pnlEditDt4, PnlDt4)
                EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
                BindGridDt(ViewState("Dt4"), GridDt4)
                StatusButtonSave(True)
            btnSaveTrans.Focus()

            Dim drow As DataRow()
            If ViewState("Dt4") Is Nothing Then
                BindDataDt4(ViewState("TransNmbr"))
            Else
                drow = ViewState("Dt4").Select("NoWl = " + QuotedStr(lbNoWl.Text))
                If drow.Length > 0 Then
                    BindGridDt(drow.CopyToDataTable, GridDt4)
                    GridDt4.Columns(0).Visible = Not ViewState("StateHd") = "View"
                Else
                    Dim DtTemp As DataTable
                    DtTemp = ViewState("Dt4").Clone
                    DtTemp.Rows.Add(DtTemp.NewRow())
                    GridDt4.DataSource = DtTemp
                    GridDt4.DataBind()
                    GridDt4.Columns(0).Visible = False
                    btnAdddt4.Visible = True
                    btnAddDt4ke2.Visible = True
                End If
            End If
        Catch ex As Exception
            lbStatus.Text = "btn save Dt4 Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub
    Dim TQtyA As Double = 0
    Protected Sub GridDt4_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt4.RowDataBound
        Try
            'If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "Material")) Then
            '    If e.Row.RowType = DataControlRowType.DataRow Then
            '        TQtyA = GetTotalSum(ViewState("Dt4"), "QtyUse")
            '    ElseIf e.Row.RowType = DataControlRowType.Footer Then
            '    End If
            'End If
            'tbBooking.Text = TQtyA

        Catch ex As Exception
            lbStatus.Text = "GridDt3_RowDataBound Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt4_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt4.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow
            GVR = GridDt4.Rows(e.RowIndex)
            dr = ViewState("Dt4").Select("NoDok = " + QuotedStr(GVR.Cells(1).Text))
            dr(0).Delete()
            BindGridDt(ViewState("Dt4"), GridDt4)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt 4 Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt4_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt4.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt4.Rows(e.NewEditIndex)
            FillTextBoxDt4(GVR.Cells(1).Text)
            ViewState("Dt4Value") = GVR.Cells(1).Text
            MovePanel(PnlDt4, pnlEditDt4)
            EnableHd(False)
            ViewState("StateDt4") = "Edit"
            StatusButtonSave(False)
            EnableDt4(False)
            btnSaveDt4.Focus()

        Catch ex As Exception
            lbStatus.Text = "Grid dt4 Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddDt4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdddt4.Click, btnAddDt4ke2.Click
        Try
            Cleardt4()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt4") = "Insert"
            MovePanel(PnlDt4, pnlEditDt4)
            EnableHd(False)
            StatusButtonSave(False)
            EnableDt4(True)
            btnSaveDt4.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt4 error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnCancelDt4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelDt4.Click, btnBackDt2.Click, btnBackDt.Click, btnCancelDt.Click, btnCancelDt3.Click
        Try
            MovePanel(pnlEditDt4, PnlDt4)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
            StatusButtonSave(True)
            btnSaveTrans.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt4 Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            FillTextBoxDt(GVR.Cells(1).Text, GVR.Cells(1).Text)
            ViewState("DtValue") = GVR.Cells(1).Text
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            StatusButtonSave(False)
            btnSaveDt.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid dt Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlShowRecord_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlShowRecord.SelectedIndexChanged
        GridView1.PageIndex = 0
        GridView1.EditIndex = -1
        GridView1.PageSize = ddlShowRecord.SelectedValue
        BindData()
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
                    '   btnProcessDel.Visible = True
                End If
            Next
        Catch ex As Exception
            Throw New Exception("Check All Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub cbSelectHd_CheckedChanged1(ByVal sender As Object, ByVal e As System.EventArgs)
        '  CheckAllDt(GridRollOutput, sender)
    End Sub



    Protected Sub tbLainLain_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbLainLain.TextChanged
        Try

            Dim dt As DataTable
            Dim SQLStrings As String
            SQLStrings = "UPDATE GLLandPurchaseReqHd SET LainLain = " + QuotedStr(tbLainLain.Text) + _
            " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            SQLExecuteNonQuery(SQLStrings, ViewState("DBConnection").ToString)
        Catch ex As Exception
            Throw New Exception("ddlHitungTotal All Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbLainLain2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbLainLain2.TextChanged
        Try

            Dim dt As DataTable
            Dim SQLStrings As String
            SQLStrings = "UPDATE GLLandPurchaseReqHd SET LainLain2 = " + QuotedStr(tbLainLain2.Text) + _
            " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            SQLExecuteNonQuery(SQLStrings, ViewState("DBConnection").ToString)
        Catch ex As Exception
            Throw New Exception("ddlHitungTotal All Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbLainLain3_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbLainLain3.TextChanged
        Try

            Dim dt As DataTable
            Dim SQLStrings As String
            SQLStrings = "UPDATE GLLandPurchaseReqHd SET LainLain3 = " + QuotedStr(tbLainLain3.Text) + _
            " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            SQLExecuteNonQuery(SQLStrings, ViewState("DBConnection").ToString)
        Catch ex As Exception
            Throw New Exception("ddlHitungTotal All Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbNilai_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbNilai.TextChanged
        Try
            ddlHitungTotal_SelectedIndexChanged(Nothing, Nothing)
        Catch ex As Exception
            Throw New Exception("ddlHitungTotal All Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbSPPT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbSPPT.TextChanged
        Try
            ddlHitungTotal_SelectedIndexChanged(Nothing, Nothing)
            tbSPPT.Text = FormatNumber((tbSPPT.Text), ViewState("DigitCurr"))
        Catch ex As Exception
            Throw New Exception("ddlHitungTotal All Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbAjbSphShm_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAjbSphShm.TextChanged
        Try
            ddlHitungTotal_SelectedIndexChanged(Nothing, Nothing)

            tbAjbSphShm.Text = FormatNumber((tbAjbSphShm.Text), ViewState("DigitCurr"))
        Catch ex As Exception
            Throw New Exception("ddlHitungTotal All Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbLuasUkur_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbLuasUkur.TextChanged
        Try
            ddlHitungTotal_SelectedIndexChanged(Nothing, Nothing)
           
            tbLuasUkur.Text = FormatNumber((tbLuasUkur.Text), ViewState("DigitCurr"))
        Catch ex As Exception
            Throw New Exception("ddlHitungTotal All Error : " + ex.ToString)
        End Try
    End Sub


    Protected Sub ddlHitungTotal_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlHitungTotal.SelectedIndexChanged
        Try
            If tbNilai.Text = "" Then
                tbNilai.Text = 0
            End If

            If tbAjbSphShm.Text = "" Then
                tbAjbSphShm.Text = 0
            End If

            If tbSPPT.Text = "" Then
                tbSPPT.Text = 0
            End If

            If tbLuasUkur.Text = "" Then
                tbLuasUkur.Text = 0
            End If

            If ddlHitungTotal.SelectedValue = "SPPT" Then
                tbTotal.Text = FormatNumber((tbSPPT.Text * tbNilai.Text), ViewState("DigitCurr"))
                tbHrgFix.Text = FormatNumber((tbSPPT.Text), ViewState("DigitCurr"))
            ElseIf ddlHitungTotal.SelectedValue = "AJB/SPH/SHM" Then
                'tbTotal.Text = Val(tbAjbSphShm.Text * tbNilai.Text)
                tbTotal.Text = FormatNumber((tbAjbSphShm.Text * tbNilai.Text), ViewState("DigitCurr"))
                tbHrgFix.Text = FormatNumber((tbAjbSphShm.Text), ViewState("DigitCurr"))
            ElseIf ddlHitungTotal.SelectedValue = "Luas Ukur" Then
                'tbTotal.Text = Val(tbLuasUkur.Text * tbNilai.Text)
                tbTotal.Text = FormatNumber((tbLuasUkur.Text * tbNilai.Text), ViewState("DigitCurr"))
                tbHrgFix.Text = FormatNumber((tbLuasUkur.Text), ViewState("DigitCurr"))
            Else
                tbTotal.Text = 0
                tbHrgFix.Text = 0
            End If
            tbNilai.Text = FormatNumber((tbNilai.Text), ViewState("DigitCurr"))


        Catch ex As Exception
            Throw New Exception("ddlHitungTotal All Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlJenisDokumen_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlJenisDokumen.SelectedIndexChanged
        Try
            If ddlJenisDokumen.SelectedValue = "SHGB" Or ddlJenisDokumen.SelectedValue = "SHGU"  Then
                tbMasaBerlaku.Enabled = True
                tbMasaBerlaku.SelectedDate = ViewState("ServerDate")
            Else
                tbMasaBerlaku.Clear()
                tbMasaBerlaku.Enabled = False

            End If

        Catch ex As Exception
            Throw New Exception("ddlHitungTotal All Error : " + ex.ToString)
        End Try
    End Sub


    Protected Sub lbSeller_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSeller.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsSeller')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lbSeller_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbModerator_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbModerator.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsModerator')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lbModerator_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbArea_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbArea.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsArea')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lbModerator_Click Error : " + ex.ToString
        End Try
    End Sub




    Protected Sub lbKTP_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbKtp.Click
        Try
            Dim dr As DataTable
            Dim filePath, URL As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            If dr.Rows(0)("DocKTP").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            filePath = dr.Rows(0)("DocKTP").ToString
            URL = ResolveUrl("~/Dokumen/" + filePath)
            'URL = ResolveUrl("~/Dokumen/" + filePath)
            Dim s As String = "window.open('" & URL & "', '_blank');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)

        Catch ex As Exception
            lbStatus.Text = "lbModerator_Click Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub lbKK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbKK.Click
        Try
            Dim dr As DataTable
            Dim filePath, URL As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            If dr.Rows(0)("DocKK").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            filePath = dr.Rows(0)("DocKK").ToString
            URL = ResolveUrl("~/Dokumen/" + filePath)
            Dim s As String = "window.open('" & URL & "', '_blank');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)

        Catch ex As Exception
            lbStatus.Text = "lbKK_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbSPPT_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSPPT.Click
        Try
            Dim dr As DataTable
            Dim filePath, URL As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            If dr.Rows(0)("DocSPPT").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            filePath = dr.Rows(0)("DocSPPT").ToString
            URL = ResolveUrl("~/Dokumen/" + filePath)
            Dim s As String = "window.open('" & URL & "', '_blank');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)

        Catch ex As Exception
            lbStatus.Text = "lbSPPT_Click Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub lbSTTS_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSTTS.Click
        Try
            Dim dr As DataTable
            Dim filePath, URL As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            If dr.Rows(0)("DocSTTS").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            filePath = dr.Rows(0)("DocSTTS").ToString
            URL = ResolveUrl("~/Dokumen/" + filePath)
            Dim s As String = "window.open('" & URL & "', '_blank');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)

        Catch ex As Exception
            lbStatus.Text = "lbSTTS_Click Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub lbTTD_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbTTD.Click
        Try
            Dim dr As DataTable
            Dim filePath, URL As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            If dr.Rows(0)("DocTTD").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            filePath = dr.Rows(0)("DocTTD").ToString
            URL = ResolveUrl("~/Dokumen/" + filePath)
            Dim s As String = "window.open('" & URL & "', '_blank');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)

        Catch ex As Exception
            lbStatus.Text = "lbSTTS_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbAJB_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAJB.Click
        Try
            Dim dr As DataTable
            Dim filePath, URL As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            If dr.Rows(0)("DocAJB").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            filePath = dr.Rows(0)("DocAJB").ToString
            URL = ResolveUrl("~/Dokumen/" + filePath)
            Dim s As String = "window.open('" & URL & "', '_blank');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)

        Catch ex As Exception
            lbStatus.Text = "lbSTTS_Click Error : " + ex.ToString
        End Try
    End Sub



    Protected Sub lbAJB2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAJB2.Click
        Try
            Dim dr As DataTable
            Dim filePath, URL As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            If dr.Rows(0)("DocAJB2").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            filePath = dr.Rows(0)("DocAJB2").ToString
            URL = ResolveUrl("~/Dokumen/" + filePath)
            Dim s As String = "window.open('" & URL & "', '_blank');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)

        Catch ex As Exception
            lbStatus.Text = "lbSTTS_Click Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub lbSSP_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSSP.Click
        Try
            Dim dr As DataTable
            Dim filePath, URL As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            If dr.Rows(0)("DocSSP").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            filePath = dr.Rows(0)("DocSSP").ToString
            URL = ResolveUrl("~/Dokumen/" + filePath)
            Dim s As String = "window.open('" & URL & "', '_blank');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)

        Catch ex As Exception
            lbStatus.Text = "lbKK_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbSSD_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSSD.Click
        Try
            Dim dr As DataTable
            Dim filePath, URL As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            If dr.Rows(0)("DocSSD").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            filePath = dr.Rows(0)("DocSSD").ToString
            URL = ResolveUrl("~/Dokumen/" + filePath)
            Dim s As String = "window.open('" & URL & "', '_blank');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)

        Catch ex As Exception
            lbStatus.Text = "lbKK_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbSKTS_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSKTS.Click
        Try
            Dim dr As DataTable
            Dim filePath, URL As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            If dr.Rows(0)("DocKTS").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            filePath = dr.Rows(0)("DocKTS").ToString
            URL = ResolveUrl("~/Dokumen/" + filePath)
            Dim s As String = "window.open('" & URL & "', '_blank');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)

        Catch ex As Exception
            lbStatus.Text = "lbKK_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbSPBT_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSPBT.Click
        Try
            Dim dr As DataTable
            Dim filePath, URL As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            If dr.Rows(0)("DocSBPT").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            filePath = dr.Rows(0)("DocSBPT").ToString
            URL = ResolveUrl("~/Dokumen/" + filePath)
            Dim s As String = "window.open('" & URL & "', '_blank');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)

        Catch ex As Exception
            lbStatus.Text = "lbKK_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbSPKTT_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSPKTT.Click
        Try
            Dim dr As DataTable
            Dim filePath, URL As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            If dr.Rows(0)("DocSPKTT").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            filePath = dr.Rows(0)("DocSPKTT").ToString
            URL = ResolveUrl("~/Dokumen/" + filePath)
            Dim s As String = "window.open('" & URL & "', '_blank');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)

        Catch ex As Exception
            lbStatus.Text = "lbKK_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbBAM_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbBAM.Click
        Try
            Dim dr As DataTable
            Dim filePath, URL As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            If dr.Rows(0)("DocBAM").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            filePath = dr.Rows(0)("DocBAM").ToString
            URL = ResolveUrl("~/Dokumen/" + filePath)
            Dim s As String = "window.open('" & URL & "', '_blank');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)

        Catch ex As Exception
            lbStatus.Text = "lbKK_Click Error : " + ex.ToString
        End Try
    End Sub



    Protected Sub lbBAPL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbBAPL.Click
        Try
            Dim dr As DataTable
            Dim filePath, URL As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            If dr.Rows(0)("DocBAPL").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            filePath = dr.Rows(0)("DocBAPL").ToString
            URL = ResolveUrl("~/Dokumen/" + filePath)
            Dim s As String = "window.open('" & URL & "', '_blank');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)

        Catch ex As Exception
            lbStatus.Text = "lbBAPL_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbPTPP_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbPTPP.Click
        Try
            Dim dr As DataTable
            Dim filePath, URL As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            If dr.Rows(0)("DocPTPP").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            filePath = dr.Rows(0)("DocPTPP").ToString
            URL = ResolveUrl("~/Dokumen/" + filePath)
            Dim s As String = "window.open('" & URL & "', '_blank');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)

        Catch ex As Exception
            lbStatus.Text = "lbBAPL_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbPTPP2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbPtPP2.Click
        Try
            Dim dr As DataTable
            Dim filePath, URL As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            If dr.Rows(0)("DocPTPP2").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            filePath = dr.Rows(0)("DocPTPP2").ToString
            URL = ResolveUrl("~/Dokumen/" + filePath)
            Dim s As String = "window.open('" & URL & "', '_blank');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)

        Catch ex As Exception
            lbStatus.Text = "lbModerator_Click Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub lbSKRT_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSKRT.Click
        Try
            Dim dr As DataTable
            Dim filePath, URL As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            If dr.Rows(0)("DocSKRT").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            filePath = dr.Rows(0)("DocSKRT").ToString
            URL = ResolveUrl("~/Dokumen/" + filePath)
            Dim s As String = "window.open('" & URL & "', '_blank');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)

        Catch ex As Exception
            lbStatus.Text = "lbModerator_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbSKD_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSKD.Click
        Try
            Dim dr As DataTable
            Dim filePath, URL As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            If dr.Rows(0)("DocSKD").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            filePath = dr.Rows(0)("DocSKD").ToString
            URL = ResolveUrl("~/Dokumen/" + filePath)
            Dim s As String = "window.open('" & URL & "', '_blank');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)

        Catch ex As Exception
            lbStatus.Text = "lbModerator_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbFCGirik_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbFCGirik.Click
        Try
            Dim dr As DataTable
            Dim filePath, URL As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            If dr.Rows(0)("DocFcGirik").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            filePath = dr.Rows(0)("DocFcGirik").ToString
            URL = ResolveUrl("~/Dokumen/" + filePath)
            Dim s As String = "window.open('" & URL & "', '_blank');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)

        Catch ex As Exception
            lbStatus.Text = "lbModerator_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbFDP_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbFDP.Click
        Try
            Dim dr As DataTable
            Dim filePath, URL As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            If dr.Rows(0)("DocFDP").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            filePath = dr.Rows(0)("DocFDP").ToString
            URL = ResolveUrl("~/Dokumen/" + filePath)
            Dim s As String = "window.open('" & URL & "', '_blank');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)

        Catch ex As Exception
            lbStatus.Text = "lbModerator_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbPatok_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbPatok.Click
        Try
            Dim dr As DataTable
            Dim filePath, URL As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            If dr.Rows(0)("DocPatok").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            filePath = dr.Rows(0)("DocPatok").ToString
            URL = ResolveUrl("~/Dokumen/" + filePath)
            Dim s As String = "window.open('" & URL & "', '_blank');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)

        Catch ex As Exception
            lbStatus.Text = "lbModerator_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbSPoradik_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSporadik.Click
        Try
            Dim dr As DataTable
            Dim filePath, URL As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            If dr.Rows(0)("DocSporadik").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            filePath = dr.Rows(0)("DocSporadik").ToString
            URL = ResolveUrl("~/Dokumen/" + filePath)
            Dim s As String = "window.open('" & URL & "', '_blank');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)

        Catch ex As Exception
            lbStatus.Text = "lbModerator_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbAHU_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAHU.Click
        Try
            Dim dr As DataTable
            Dim filePath, URL As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            If dr.Rows(0)("DocAHU").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            filePath = dr.Rows(0)("DocAHU").ToString
            URL = ResolveUrl("~/Dokumen/" + filePath)
            Dim s As String = "window.open('" & URL & "', '_blank');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)

        Catch ex As Exception
            lbStatus.Text = "lbModerator_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbSejarah_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSejarah.Click
        Try
            Dim dr As DataTable
            Dim filePath, URL As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            If dr.Rows(0)("DocSejarah").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            filePath = dr.Rows(0)("DocSejarah").ToString
            URL = ResolveUrl("~/Dokumen/" + filePath)
            Dim s As String = "window.open('" & URL & "', '_blank');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)

        Catch ex As Exception
            lbStatus.Text = "lbModerator_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbSPJH_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSPJH.Click
        Try
            Dim dr As DataTable
            Dim filePath, URL As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            If dr.Rows(0)("DocSPJH").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            filePath = dr.Rows(0)("DocSPJH").ToString
            URL = ResolveUrl("~/Dokumen/" + filePath)
            Dim s As String = "window.open('" & URL & "', '_blank');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)

        Catch ex As Exception
            lbStatus.Text = "lbModerator_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbLainLain_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbLainLain.Click
        Try
            Dim dr As DataTable
            Dim filePath, URL As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            If dr.Rows(0)("DocLainLain").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            filePath = dr.Rows(0)("DocLainLain").ToString
            URL = ResolveUrl("~/Dokumen/" + filePath)
            Dim s As String = "window.open('" & URL & "', '_blank');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)

        Catch ex As Exception
            lbStatus.Text = "lbModerator_Click Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub lbKtpW_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbKtpW.Click
        Try
            Dim dr As DataTable
            Dim filePath, URL As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            If dr.Rows(0)("DocKtpW").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            filePath = dr.Rows(0)("DocKtpW").ToString
            URL = ResolveUrl("~/Dokumen/" + filePath)
            Dim s As String = "window.open('" & URL & "', '_blank');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)

        Catch ex As Exception
            lbStatus.Text = "lbModerator_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbSPW_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSPW.Click
        Try
            Dim dr As DataTable
            Dim filePath, URL As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            If dr.Rows(0)("DocSPW").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            filePath = dr.Rows(0)("DocSPW").ToString
            URL = ResolveUrl("~/Dokumen/" + filePath)
            Dim s As String = "window.open('" & URL & "', '_blank');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)

        Catch ex As Exception
            lbStatus.Text = "lbModerator_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbSPKW_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSPKW.Click
        Try
            Dim dr As DataTable
            Dim filePath, URL As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            If dr.Rows(0)("DocSPKW").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            filePath = dr.Rows(0)("DocSPKW").ToString
            URL = ResolveUrl("~/Dokumen/" + filePath)
            Dim s As String = "window.open('" & URL & "', '_blank');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)

        Catch ex As Exception
            lbStatus.Text = "lbModerator_Click Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub lbBPJS_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbBPJS.Click
        Try
            Dim dr As DataTable
            Dim filePath, URL As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            If dr.Rows(0)("DocBPJS").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            filePath = dr.Rows(0)("DocBPJS").ToString
            URL = ResolveUrl("~/Dokumen/" + filePath)
            Dim s As String = "window.open('" & URL & "', '_blank');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)

        Catch ex As Exception
            lbStatus.Text = "lbBPJS_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbLainLain2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbLainLain2.Click
        Try
            Dim dr As DataTable
            Dim filePath, URL As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            If dr.Rows(0)("DocLainLain2").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            filePath = dr.Rows(0)("DocLainLain2").ToString
            URL = ResolveUrl("~/Dokumen/" + filePath)
            Dim s As String = "window.open('" & URL & "', '_blank');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)

        Catch ex As Exception
            lbStatus.Text = "lbModerator_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbLainLain3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbLainLain3.Click
        Try
            Dim dr As DataTable
            Dim filePath, URL As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            If dr.Rows(0)("DocLainLain3").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            filePath = dr.Rows(0)("DocLainLain3").ToString
            URL = ResolveUrl("~/Dokumen/" + filePath)
            Dim s As String = "window.open('" & URL & "', '_blank');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)

        Catch ex As Exception
            lbStatus.Text = "lbModerator_Click Error : " + ex.ToString
        End Try
    End Sub



    Protected Sub btnDelKtp_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDelKTP.Click

        Try

            Dim dr As DataTable
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows(0)("DocKTP").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            Dim PathDel, NameFile2KTP, SQLString2 As String
            Dim dt As DataTable

            PathDel = dr.Rows(0)("DocKTP").ToString
            File.Delete(Server.MapPath("~/Dokumen/" + PathDel))

            SQLString2 = "UPDATE GLLandPurchaseReqHd SET DocKTP =  NULL " + _
            " WHERE TransNmbr = " + QuotedStr(tbCode.Text)

            SQLExecuteNonQuery(SQLString2, ViewState("DBConnection").ToString)
            lbKtp.Text = "Not Yet Uploaded"
            FupMain.Visible = True
            lblmassageKTP.Visible = False
        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub




 

End Class


