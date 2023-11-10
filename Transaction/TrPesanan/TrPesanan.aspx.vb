Imports System.Data
Imports System.Data.SqlClient
'Imports System.Data.Odbc
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc


Partial Class TrPemesanan
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select * From V_MKTTSPHD"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                SetInit()
                'FillCombo(ddlpph, "SELECT PPHCode, PPHName FROM MsPPH", True, "PPHCode", "PPHName", ViewState("DBConnection"))
                FillCombo(ddlTerm, "EXEC S_GetTerm", False, "Term_Code", "Term_Name", ViewState("DBConnection"))
                FillCombo(ddlType, "SELECT TypeCode, TypeName FROM V_TypeBayar", False, "TypeCode", "TypeName", ViewState("DBConnection"))
                FillCombo(ddlDept, "SELECT DeptCode, DeptName FROM MsDepartment", False, "DeptCode", "DeptName", ViewState("DBConnection"))
                FillCombo(ddlCurr, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
                FillCombo(ddlBank, "SELECT BankCode, BankName FROM MsBank", True, "BankCode", "BankName", ViewState("DBConnection"))
                Session("AdvanceFilter") = ""
            End If
            lbStatus.Text = ""
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If

            ' hasil dari search dialog
            If Not Session("Result") Is Nothing Then

                If ViewState("Sender") = "btnCust" Then
                    tbCustomerCode.Text = Session("Result")(0).ToString
                    tbCustomerName.Text = Session("Result")(1).ToString
                End If

                If ViewState("Sender") = "btnUnit" Then
                    TbUnit.Text = Session("Result")(0).ToString
                    tbUnitName.Text = Session("Result")(1).ToString
                    btnSaveDt.Focus()
                End If

                If ViewState("Sender") = "btnArea" Then
                    tbArea.Text = Session("Result")(0).ToString
                    tbAreaName.Text = Session("Result")(1).ToString
                End If

                If ViewState("Sender") = "btnWakilPenjual" Then
                    tbWakilPenjual.Text = Session("Result")(0).ToString
                    tbNamaWakilPenjual.Text = Session("Result")(1).ToString
                End If


                If ViewState("Sender") = "btnSP" Then
                    ''Insert Detail

                    BindToText(tbSpNo, Session("Result")(0).ToString)
                    BindToText(tbAngsuran, Session("Result")(1).ToString)
                    BindToDropList(ddlRencanaBayar, Session("Result")(2).ToString)
                    BindToText(tbDiscHd, Session("Result")(3).ToString)
                    BindToText(tbTotDisc, Session("Result")(4).ToString)
                    BindToText(tbTotDPP, Session("Result")(5).ToString)
                    BindToText(tbArea, Session("Result")(6).ToString)
                    BindToText(tbAreaName, Session("Result")(7).ToString)
                    BindToDropList(ddlFgsewa, Session("Result")(8).ToString)
                    BindToText(tbPeName, Session("Result")(9).ToString)
                    BindToText(tbPeName2, Session("Result")(10).ToString)

                    Dim drResult As DataRow
                    Dim ExistRow As DataRow()
                    Dim DtKav As DataTable
                    Dim SQLKav As String

                    SQLKav = "SELECT * FROM V_GetPenawaran WHERE TransNmbr = " + QuotedStr(tbSpNo.Text)
                    DtKav = SQLExecuteQuery(SQLKav, ViewState("DBConnection")).Tables(0)

                    For Each drResult In DtKav.Rows
                        ExistRow = ViewState("Dt").Select("UnitCode = " + QuotedStr(drResult("UnitCode").ToString))
                        If ExistRow.Count = 0 Then                         'insert

                            Dim dr As DataRow
                            dr = ViewState("Dt").NewRow
                            dr("UnitCode") = drResult("UnitCode").ToString
                            dr("UnitName") = drResult("UnitName").ToString
                            dr("LuasTanah") = drResult("LuasTanah").ToString
                            dr("LuasBangunan") = drResult("LuasBangunan").ToString
                            dr("ArahKavling") = drResult("ArahKavling").ToString
                            dr("KtinggianDPL") = drResult("KtinggianDPL").ToString
                            dr("Price") = drResult("Price").ToString
                            ''dr("Disc") = drResult("Disc").ToString
                            ''dr("DiscValue") = drResult("DiscValue").ToString
                            'dr("Dpp") = drResult("Dpp").ToString
                            'dr("PpnTotal") = 0
                            'dr("PphTotal") = 0
                            'dr("AmountTotal") = 0
                            'dr("TJ") = 0
                            'dr("DP") = 0
                            'dr("SisaTagihan") = 0
                            ViewState("Dt").Rows.Add(dr)
                        End If



                        'Insert Sub Detail
                        lblUnit.Text = drResult("UnitCode").ToString
                        lblUnitName.Text = drResult("UnitName").ToString

                        'MultiView1.ActiveViewIndex = 1
                        Dim drDtResult As DataRow
                        Dim ExistRowDT As DataRow()
                        Dim Interval As Integer
                        Dim DtPekerjaan As DataTable
                        Dim SQLString As String


                        SQLString = "SELECT * FROM V_GetDpTJ WHERE TransNmbr = " + QuotedStr(tbSpNo.Text)
                        DtPekerjaan = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)

                        For Each drDtResult In DtPekerjaan.Rows
                            ExistRowDT = ViewState("Dt2").Select("ItemNo= " + QuotedStr(drDtResult("ItemNo").ToString))
                            If ExistRowDT.Count = 0 Then
                                Dim Dtdr As DataRow
                                Dtdr = ViewState("Dt2").NewRow
                                Interval = drDtResult("Interval".ToString)
                                Dtdr("ItemNo") = drDtResult("ItemNo".ToString)
                                Dtdr("PayDate") = DateAdd(DateInterval.Month, Interval, tbDate.SelectedDate)
                                Dtdr("Type") = drDtResult("TypeAngsuran".ToString)
                                Dtdr("TypeName") = drDtResult("TypeName".ToString)
                                Dtdr("PayKav") = drDtResult("Nilai".ToString)
                                Dtdr("PPn") = ViewState("PPN")
                                Dtdr("PpnValue") = Math.Floor(CFloat(Dtdr("PayKav".ToString)) * CFloat(Dtdr("PPn".ToString)) / 100)
                                Dtdr("Pph") = 0
                                Dtdr("PphValue") = 0
                                Dtdr("AmountDt2") = CFloat(Dtdr("PayKav".ToString)) + CFloat(Dtdr("PpnValue".ToString))
                                Dtdr("Remark") = drDtResult("Remark".ToString)
                                ViewState("Dt2").Rows.Add(Dtdr)
                            End If


                        Next
                        CountTotalDt()
                        btnGetAngsuran_Click(Nothing, Nothing)


                    Next

                    BindGridDt(ViewState("Dt"), GridDt)
                    CountTotalDt2()
                    'EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
                    StatusButtonSave(True)

                End If

                Session("Result") = Nothing
                ViewState("Sender") = Nothing
                Session("filter") = Nothing
                Session("Column") = Nothing

            End If

            FubInv.Attributes("onchange") = "UploadInvoice(this)"

            FubBAPExt.Attributes("onchange") = "UploadBAP(this)"

            FubFaktur.Attributes("onchange") = "UploadFaktur(this)"

            FubDokLain.Attributes("onchange") = "UploadLain(this)"


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
                    sqlstring = "Declare @A VarChar(255) EXEC [S_S_MKTTSPCancel] " + QuotedStr(ViewState("TransNmbr").ToString) + "," + CInt(Session(Request.QueryString("KeyId"))("Year")).ToString + ", " + CInt(Session(Request.QueryString("KeyId"))("Period")).ToString + "," + QuotedStr(HiddenRemarkReject.Value) + "," + QuotedStr(ViewState("UserId")) + ", @A OUT SELECT @A"
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

            SQLString1 = "UPDATE MKTTSPHD SET DokInvoice = " + QuotedStr(namafile2) + _
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
            SQLString2 = "UPDATE MKTTSPHD SET DokFaktur = " + QuotedStr(NameFile2KK) + _
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

            SQLString3 = "UPDATE MKTTSPHD SET DokLain = " + QuotedStr(NameFile2SPPT) + _
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

            SQLString4 = "UPDATE MKTTSPHD SET DokBAP = " + QuotedStr(NameFile2STTS) + _
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
                SQLExecuteNonQuery("UPDATE MKTTSPHD Set DokInvoice = '' WHERE TransNmbr = '" + tbCode.Text + "' ", ViewState("DBConnection").ToString)

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
                SQLExecuteNonQuery("UPDATE MKTTSPHD Set DokFaktur = '' WHERE TransNmbr = '" + tbCode.Text + "' ", ViewState("DBConnection").ToString)

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
                SQLExecuteNonQuery("UPDATE MKTTSPHD Set DokBAP = '' WHERE TransNmbr = '" + tbCode.Text + "' ", ViewState("DBConnection").ToString)
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
                SQLExecuteNonQuery("UPDATE MKTTSPHD Set DokLain = '' WHERE TransNmbr = '" + tbCode.Text + "' ", ViewState("DBConnection").ToString)
                lbDokLain.Text = "Not yet uploaded"
                FubDokLain.Visible = True
                btnClearLain.Visible = False
            End If

        Catch ex As Exception
            lbStatus.Text = "btnClearLain_Click Error : " + ex.ToString
        End Try
    End Sub


    Private Sub CountTotalDt2()
        Dim Price, Disc, Dpp, Ppn, Pph, Total As Double
        Dim Dr As DataRow
        Try


            Price = 0
            Ppn = 0
            Pph = 0
            Total = 0
            Disc = 0
            Dpp = 0
            For Each Dr In ViewState("Dt").Rows
                If Not Dr.RowState = DataRowState.Deleted Then
                    Price = Price + CFloat(Dr("Price").ToString)
                    Ppn = Ppn + CFloat(Dr("PpnTotal").ToString)
                    Pph = Pph + CFloat(Dr("PphTotal").ToString)
                    Total = Total + CFloat(Dr("AmountTotal").ToString)
                    Disc = Disc + CFloat(Dr("DiscValue").ToString)
                    Dpp = Dpp + CFloat(Dr("Dpp").ToString)
                End If
            Next
            'tbTotAmount.Text = FormatNumber(Total, ViewState("DigitHome"))
            'tbTotPPN.Text = FormatNumber(Ppn, ViewState("DigitHome"))
            'tbTotPPH.Text = FormatNumber(Pph, ViewState("DigitHome"))
            'tbTotDisc.Text = FormatNumber(Disc, ViewState("DigitHome"))
            tbTotDPP.Text = FormatNumber(Price, ViewState("DigitHome"))
            tbTotHarga.Text = FormatNumber(Price, ViewState("DigitHome"))
            AttachScript("setformathd();", Page, Me.GetType())
        Catch ex As Exception
            Throw New Exception("Count Total Dt Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub CountTotalDt()
        Dim TotalPpn, TotalPph, QtyTotal, TJ, DP, PL, AG As Double
        Dim Unit, UnitDP, UnitPL As String
        Dim Dr As DataRow
        Dim drow As DataRow()
        Dim havedetail As Boolean
        Try

           
            QtyTotal = 0
            TotalPpn = 0
            TotalPph = 0
            For Each Dr In ViewState("Dt2").Rows
                If Not Dr.RowState = DataRowState.Deleted Then
                    QtyTotal = QtyTotal + CFloat(Dr("AmountDt2").ToString)
                    TotalPpn = TotalPpn + CFloat(Dr("PpnValue").ToString)
                    TotalPph = TotalPph + CFloat(Dr("PphValue").ToString)
                End If
            Next

            ''lbStatus.Text = TotalPpn
            ''Exit Sub


            Unit = "TJ"
            drow = ViewState("Dt2").Select("Type = " + QuotedStr(Unit))
            TJ = 0
            If drow.Length > 0 Then
                havedetail = False
                For Each Dr In drow.CopyToDataTable.Rows
                    If Not Dr.RowState = DataRowState.Deleted Then
                        TJ = TJ + CFloat(Dr("AmountDt2").ToString)

                    End If
                Next
            End If


            UnitDP = "DP"
            drow = ViewState("Dt2").Select("Type = " + QuotedStr(UnitDP))
            DP = 0
            If drow.Length > 0 Then
                havedetail = False
                For Each Dr In drow.CopyToDataTable.Rows
                    If Not Dr.RowState = DataRowState.Deleted Then
                        DP = DP + CFloat(Dr("AmountDt2").ToString)

                    End If
                Next
            End If


            UnitDP = "AG"
            drow = ViewState("Dt2").Select("Type = " + QuotedStr(UnitDP))
            AG = 0
            If drow.Length > 0 Then
                havedetail = False
                For Each Dr In drow.CopyToDataTable.Rows
                    If Not Dr.RowState = DataRowState.Deleted Then
                        AG = AG + CFloat(Dr("AmountDt2").ToString)

                    End If
                Next
            End If



            UnitDP = "PL"
            drow = ViewState("Dt2").Select("Type = " + QuotedStr(UnitDP))
            PL = 0
            If drow.Length > 0 Then
                havedetail = False
                For Each Dr In drow.CopyToDataTable.Rows
                    If Not Dr.RowState = DataRowState.Deleted Then
                        PL = PL + CFloat(Dr("AmountDt2").ToString)

                    End If
                Next
            End If

            

            tbTotPPH.Text = FormatNumber(TotalPph, ViewState("DigitHome"))
            tbTotPPN.Text = FormatNumber(TotalPpn, ViewState("DigitHome"))
            tbTotAmount.Text = FormatNumber(QtyTotal, ViewState("DigitHome"))
            tbTJHd.Text = FormatNumber(TJ, ViewState("DigitHome"))
            tbDPValue.Text = FormatNumber(DP, ViewState("DigitHome"))
            tbSisaBayarHd.Text = FormatNumber(CFloat(PL + AG), ViewState("DigitHome"))

            'lbStatus.Text = tbTotPPN.Text
            'Exit Sub

            'Dr = ViewState("Dt").Select("UnitCode = " + QuotedStr(TrimStr(lblUnit.Text)))(0)
            ''price = CFloat(Dr("Price").ToString)
            ''lbStatus.Text = Dr("PriceForex").ToString + "   " + ViewState("DigitCurr").ToString
            ''Exit Sub
            'Dr.BeginEdit()
            'Dr("PpnTotal") = TotalPpn 'FormatNumber(QtyTotal, ViewState("DigitQty"))
            'Dr("PphTotal") = TotalPph
            'Dr("AmountTotal") = QtyTotal
            'Dr("TJ") = TJ
            'Dr("DP") = DP
            'Dr("SisaTagihan") = FormatNumber(PL + AG, ViewState("DigitHome"))
            ''Dr("AmountTotal") = FormatNumber(QtyTotal * Dr("PriceForex") * Dr("ForexRate"), ViewState("DigitHome"))
            ''Dr("Total") = FormatNumber(QtyTotal * price, ViewState("DigitHome"))
            'CountTotalDt2()
            'Dr.EndEdit()
            'BindGridDt(ViewState("Dt"), GridDt)

            'lbQtyTotal.Text = FormatNumber(QtyTotal, ViewState("DigitQty"))
        Catch ex As Exception
            Throw New Exception("Count Total Dt Error : " + ex.ToString)
        End Try
    End Sub


    Private Sub CountTotalRecordAngsuran()

        Dim drow() As DataRow
        Dim UnitAG As String
        Dim havedetail As Boolean
        Dim DateAngsuran As Date
        Dim Nilai As Integer

        Try
            'UnitAG = lblUnit.Text + "|AG"
            'drow = ViewState("Dt2").Select("Type = " + QuotedStr(UnitAG))
            'lblAngsuran.Text = drow.Length + 1

            UnitAG = "DP"
            drow = ViewState("Dt2").Select("Type = " + QuotedStr(UnitAG))
            If drow.Length > 0 Then
                havedetail = False
                For Each Dr In drow.CopyToDataTable.Rows
                    If Not Dr.RowState = DataRowState.Deleted Then
                        DateAngsuran = (Dr("PayDate").ToString)
                    End If
                Next
            End If

            'lbStatus.Text = lbItemNo.Text

            If lbItemNo.Text >= 3 Then
                If ddlType.SelectedValue = "AG" Then
                    lblAngsuran.Text = lbItemNo.Text - 2
                    Nilai = lbItemNo.Text - 2
                    tbTempoDate.SelectedDate = DateAdd(DateInterval.Month, Nilai, DateAngsuran)
                Else
                    lbItemNo.Text = 1
                End If
            End If

        Catch ex As Exception
            Throw New Exception("Count Total Dt Error : " + ex.ToString)
        End Try

    End Sub



    Private Function checkQty() As Boolean
        Dim drow() As DataRow
        Dim dt As New DataTable
        Dim GVR As GridViewRow
        Dim baris As Integer
        Dim i As Integer
        Dim Unit, UnitAG As String
        Dim qty, QtyTotal As Double
        Dim havedetail As Boolean

        Try
            'If GetCountRecord(ViewState("Dt")) <> 0 Then
            '    dt = ViewState("Dt")
            '    baris = dt.Rows.Count
            '    For i = 0 To GetCountRecord(ViewState("Dt")) - 1 'For i = 0 To baris - 1
            '        GVR = GridDt.Rows(i)
            '        Unit = GVR.Cells(2).Text
            qty = Val(tbTotDPP.Text.Replace(",", ""))
            QtyTotal = 0


            For Each Dr In ViewState("Dt2").Rows
                If Not Dr.RowState = DataRowState.Deleted Then
                   QtyTotal = QtyTotal + CFloat(Dr("PayKav").ToString)
                End If
            Next

            'lbStatus.Text = QtyTotal
            'Exit Function

            If qty <> QtyTotal Then
                Dim Selisih As String
                Selisih = FormatNumber(qty - QtyTotal, ViewState("DigitHome"))
                lbStatus.Text = MessageDlg("Total Price must be equall with Nominal Total in Detail Angsuran, Selisih : " + Selisih)
                Return False
            End If


            UnitAG = "AG"
            drow = ViewState("Dt2").Select("Type = " + QuotedStr(UnitAG))

            If drow.Length <> tbAngsuran.Text Then
                lbStatus.Text = MessageDlg(" Masa Aangsuran must be equall with Angsuran in Detail Angsuran")
                Return False
            End If



            '    Next
            'End If

            Return True
        Catch ex As Exception
            lbStatus.Text = "checkQty error : " + ex.ToString
        End Try
    End Function

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
        GridDt.Columns(1).Visible = False
        GridDt.Columns(12).Visible = False
        GridDt.Columns(13).Visible = False
        GridDt.Columns(14).Visible = False
        GridDt.Columns(15).Visible = False
        GridDt.Columns(16).Visible = False
        GridDt.Columns(17).Visible = False
        GridDt.Columns(18).Visible = False
        GridDt.Columns(19).Visible = False
        GridDt.Columns(20).Visible = False
 

        tbNominal.Attributes.Add("OnBlur", "setformatdt2();")
        tbPpnDt2.Attributes.Add("OnBlur", "setformatdt2();")
        tbPpnValuedt2.Attributes.Add("OnBlur", "setformatdt2();")
        tbPphDt2.Attributes.Add("OnBlur", "setformatdt2();")
        tbPphValueDt2.Attributes.Add("OnBlur", "setformatdt2();")
        tbTotalNominal.Attributes.Add("OnBlur", "setformatdt2();")

        tbTotDisc.Attributes.Add("OnBlur", "setformathd();")
        tbTotHarga.Attributes.Add("OnBlur", "setformathd();")
        tbDiscHd.Attributes.Add("OnBlur", "setformathd();")
        tbTotDPP.Attributes.Add("OnBlur", "setformathd();")
        tbTotAmount.Attributes.Add("OnBlur", "setformathd();")
        tbPphtotal.Attributes.Add("OnBlur", "setformathd();")
        tbDpp.Attributes.Add("OnBlur", "setformathd();")

        tbPrice.Attributes.Add("OnBlur", "setformatdtprice();")
        tbLuasTanah.Attributes.Add("OnBlur", "setformatdtprice();")
        tbPricePerm2.Attributes.Add("OnBlur", "setformatdtprice();")

        'tbPphDt2.Attributes.Add("OnBlur", "setformatdtppn2();")
        'tbPphValueDt2.Attributes.Add("OnBlur", "setformatdtppn2();")
        tbTotalNominal.Attributes.Add("OnBlur", "setformatdtppn2();")
        tbPpnValuedt2.Attributes.Add("OnBlur", "setformatdtppn2();")
       

        Me.tbTotalAmount.Attributes.Add("ReadOnly", "True")
        Me.tbPpnTotal.Attributes.Add("ReadOnly", "True")
        Me.tbPphtotal.Attributes.Add("ReadOnly", "True")
        'Me.tbPpnValuedt2.Attributes.Add("ReadOnly", "True")
        Me.tbPphValueDt2.Attributes.Add("ReadOnly", "True")
        Me.tbTotalAmount.Attributes.Add("ReadOnly", "True")
        Me.tbTotalNominal.Attributes.Add("ReadOnly", "True")
        Me.tbTotAmount.Attributes.Add("ReadOnly", "True")
        Me.tbTotDisc.Attributes.Add("ReadOnly", "True")
        Me.tbTotDPP.Attributes.Add("ReadOnly", "True")
        Me.tbTotPPH.Attributes.Add("ReadOnly", "True")
        Me.tbTotPPN.Attributes.Add("ReadOnly", "True")
        Me.tbDiscValue.Attributes.Add("ReadOnly", "True")
        Me.tbDpp.Attributes.Add("ReadOnly", "True")
        Me.tbTotHarga.Attributes.Add("ReadOnly", "True")


        'If ddlFgsewa.SelectedValue = "Y" Then
        '    Me.tbPphDt2.Attributes.Add("ReadOnly", "True")
        'Else
        '    Me.tbPphDt2.Attributes.Add("ReadOnly", "False")
        'End If


        'Proteksi agar hanya angka saja yang bisa di input
        tbLuasBangunan.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbLuasTanah.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbKtinggianDPL.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbPpnDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbPphDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbPrice.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbPricePerm2.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbNominal.Attributes.Add("OnKeyDown", "return PressNumeric();")

        tbAngsuran.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbDokumen.Attributes.Add("OnKeyDown", "return PressNumeric();")

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
        Return "SELECT * From V_MKTTSPDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Private Function GetStringDt2(ByVal Nmbr As String) As String
        Return "SELECT * From V_MKTTSPDt2 WHERE TransNmbr = " + QuotedStr(Nmbr)
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
                Session("SelectCommand") = "EXEC S_MKTFormTSP " + Result + ", " + QuotedStr(ViewState("UserId").ToString) + ""
                Session("ReportFile") = ".../../../Rpt/FormMKTTSP.frx"
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
                        Result = ExecSPCommandGo(ActionValue, "S_MKTTSP", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            tbCustomerCode.Enabled = State
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
            btnSaveAll.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnCancelDt2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelDt2.Click
        Try
            MovePanel(pnlEditDt2, pnlDt2)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
            StatusButtonSave(True)
            btnSaveTrans.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt2  Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt.Click
        Dim Row As DataRow
        Try
            If ViewState("StateDt") = "Edit" Then
                If ViewState("DtValue") <> TbUnit.Text Then
                    If CekExistData(ViewState("Dt"), "UnitCode", TbUnit.Text) Then
                        lbStatus.Text = "Unit Code " + TbUnit.Text + " has been already exist"
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
                Row("PricePer_m2") = tbPricePerm2.Text
                Row("Disc") = tbDisc.Text
                Row("DiscValue") = tbDiscValue.Text
                Row("Dpp") = tbDpp.Text
                Row("PpnTotal") = tbPpnTotal.Text
                Row("PphTotal") = tbPphtotal.Text
                Row("AmountTotal") = tbTotalAmount.Text
                Row("TJ") = tbQtyTJ.Text
                Row("DP") = tbQtyDP.Text
                Row("SisaTagihan") = tbQtySisaBayar.Text
                Row("DateSerahKunci") = tbSerahKunci.SelectedDate
                Row("StartSewa") = tbStartSewa.SelectedDate
                Row("EndSewa") = tbEndSewa.SelectedDate
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If
                If CekExistData(ViewState("Dt"), "UnitCode", TbUnit.Text) Then
                    lbStatus.Text = "Unit Code " + TbUnit.Text + " has been already exist"
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("UnitCode") = TbUnit.Text
                dr("UnitName") = tbUnitName.Text
                dr("LuasTanah") = tbLuasTanah.Text
                dr("LuasBangunan") = tbLuasBangunan.Text
                dr("ArahKavling") = tbArahKav.Text
                dr("KtinggianDPL") = tbKtinggianDPL.Text
                dr("Price") = tbPrice.Text
                dr("PricePer_m2") = tbPricePerm2.Text
                dr("Disc") = tbDisc.Text
                dr("DiscValue") = tbDiscValue.Text
                dr("Dpp") = tbDpp.Text
                dr("PpnTotal") = tbPpnTotal.Text
                dr("PphTotal") = tbPphtotal.Text
                dr("AmountTotal") = tbTotalAmount.Text
                dr("TJ") = tbQtyTJ.Text
                dr("DP") = tbQtyDP.Text
                dr("SisaTagihan") = tbQtySisaBayar.Text
                dr("DateSerahKunci") = tbSerahKunci.SelectedDate
                dr("StartSewa") = tbStartSewa.SelectedDate
                dr("EndSewa") = tbEndSewa.SelectedDate
                dr("Remark") = tbRemarkDt.Text
                ViewState("Dt").Rows.Add(dr)

            End If
            CountTotalDt2()
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
            BindGridDt(ViewState("Dt"), GridDt)
            StatusButtonSave(True)
            btnSaveAll.Focus()

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
                Row = ViewState("Dt2").Select("ItemNo= " + QuotedStr(lbItemNo.Text))(0)
                Row.BeginEdit()
                Row("PayDate") = tbTempoDate.SelectedDate
                Row("Type") = ddlType.SelectedValue
                If ddlType.SelectedValue = "AG" Then
                    Row("TypeName") = ddlType.SelectedItem.Text + " " + lblAngsuran.Text
                Else
                    Row("TypeName") = ddlType.SelectedItem.Text.Replace("Choose One", "")
                End If
                Row("PayKav") = tbNominal.Text
                Row("PPn") = tbPpnDt2.Text
                Row("PpnValue") = tbPpnValuedt2.Text
                Row("Pph") = tbPphDt2.Text
                Row("PphValue") = tbPphValueDt2.Text
                Row("AmountDt2") = tbTotalNominal.Text
                Row("BankCode") = ddlBank.SelectedValue
                Row("BankName") = ddlBank.SelectedItem.Text
                Row("NoGiro") = tbGiroNo.Text
                Row("AngsuranKe") = lblAngsuran.Text
                Row("Remark") = tbRemarkDt2.Text
                Row.EndEdit()

            Else

                If ddlType.SelectedValue = "AG" Then
                    If CekExistData(ViewState("Dt2"), "AngsuranKe+'|'+UnitCode", lblAngsuran.Text + "|" + lblUnit.Text) Then
                        lbStatus.Text = MessageDlg("Angsuran Ke : " + lblAngsuran.Text + " has been already exist")
                        Exit Sub
                    End If
                End If


                If ddlType.SelectedValue <> "AG" Then
                    If CekExistData(ViewState("Dt2"), "Type+'|'+UnitCode", ddlType.SelectedValue + "|" + lblUnit.Text) Then
                        lbStatus.Text = MessageDlg("Type  " + ddlType.SelectedItem.Text + " has been already exist")
                        btnSaveDt2.Focus()
                        Exit Sub
                    End If
                End If


                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt2").NewRow
                dr("ItemNo") = CInt(lbItemNo.Text)
                dr("PayDate") = tbTempoDate.SelectedDate
                dr("Type") = ddlType.SelectedValue


                If ddlType.SelectedValue = "AG" Then
                    dr("TypeName") = ddlType.SelectedItem.Text + " " + lblAngsuran.Text
                Else
                    dr("TypeName") = ddlType.SelectedItem.Text.Replace("Choose One", "")
                End If

                dr("PayKav") = tbNominal.Text
                dr("PPn") = tbPpnDt2.Text
                dr("PpnValue") = tbPpnValuedt2.Text
                dr("Pph") = tbPphDt2.Text
                dr("PphValue") = tbPphValueDt2.Text
                dr("AmountDt2") = tbTotalNominal.Text
                dr("BankCode") = ddlBank.SelectedValue
                dr("BankName") = ddlBank.SelectedItem.Text
                dr("NoGiro") = tbGiroNo.Text
                dr("AngsuranKe") = lblAngsuran.Text
                dr("Remark") = tbRemarkDt2.Text
                ViewState("Dt2").Rows.Add(dr)

            End If

            MovePanel(pnlEditDt2, pnlDt2)
            'Dim drow As DataRow()
            'drow = ViewState("Dt2").Select("UnitCode = " + QuotedStr(TrimStr(lblUnit.Text)))
            'If drow.Length > 0 Then
            '    BindGridDt(drow.CopyToDataTable, GridDt2)
            '    GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
            'Else
            '    Dim DtTemp As New DataTable
            '    DtTemp = ViewState("Dt2").Clone
            '    DtTemp.Rows.Add(DtTemp.NewRow()) ' create a new blank row to the DataTable
            '    GridDt2.DataSource = DtTemp
            '    GridDt2.DataBind()
            '    GridDt2.Columns(0).Visible = False
            'End If

            CountTotalDt()
            btnCancelDt2.Visible = True
            btnSaveDt2.Visible = True

            'EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
            BindGridDt(ViewState("Dt2"), GridDt2)
            StatusButtonSave(True)
            btnBackDt2ke2.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn save dt2 Error : " + ex.ToString
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
                tbCode.Text = GetAutoNmbr("TSP", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)


                SQLString = "INSERT INTO MKTTSPHd (TransNmbr,Status,TransDate,CustomerCode,FgSewa,SPNo,Lokasi,DepCode ," + _
                "POCustomer, PIC1, PICTlp1, PICEmail1, PIC2, PICTlp2, PICEmail2,  Perantara1, PerantaraTelp1, PerantaraEmail1, Perantara2, PerantaraTelp2, PerantaraEmail2, " + _
                "Currency, Rate, RencanaBayar, Bank, TermCode, TotalHarga,TotalDisc, TotalDPP, TotalPPN, TotalPPH, TotalAmount, MasaAngsuran, " + _
                "WakilCode, BatasDokumen, BatasDokumenDate,  Disc, DP, TandaJadi, SisaPembayaran, PphFinal, PphFinalValue, Remark, UserPrep, DatePrep) " + _
                "SELECT '" + tbCode.Text + "', 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', " + _
                QuotedStr(tbCustomerCode.Text) + ", " + QuotedStr(ddlFgsewa.SelectedValue) + ", " + _
                QuotedStr(tbSpNo.Text) + " ," + QuotedStr(tbArea.Text) + " ," + QuotedStr(ddlDept.SelectedValue) + "," + _
                QuotedStr(tbPoCust.Text) + " ," + QuotedStr(tbPICName.Text) + " ," + QuotedStr(tbPICTelp.Text) + "," + QuotedStr(tbPICEmail.Text) + "," + _
                QuotedStr(tbPICName2.Text) + " ," + QuotedStr(tbPICTelp2.Text) + "," + QuotedStr(tbPICEmail2.Text) + "," + _
                QuotedStr(tbPeName.Text) + " ," + QuotedStr(tbPeTelp.Text) + "," + QuotedStr(tbPeEmail.Text) + "," + _
                QuotedStr(tbPeName2.Text) + " ," + QuotedStr(tbPeTelp2.Text) + "," + QuotedStr(tbPeEmail2.Text) + "," + _
                QuotedStr(ddlCurr.Text) + ", " + QuotedStr(tbRate.Text) + ", " + _
                QuotedStr(ddlRencanaBayar.Text) + " ," + QuotedStr(tbBank.Text) + ", " + QuotedStr(ddlTerm.Text) + ", " + _
                QuotedStr(tbTotHarga.Text.Replace(",", "")) + ", " + QuotedStr(tbTotDisc.Text.Replace(",", "")) + ", " + QuotedStr(tbTotDPP.Text.Replace(",", "")) + "," + _
                QuotedStr(tbTotPPN.Text.Replace(",", "")) + ", " + QuotedStr(tbTotPPH.Text.Replace(",", "")) + "," + QuotedStr(tbTotAmount.Text.Replace(",", "")) + ", " + _
                QuotedStr(tbAngsuran.Text) + " ," + QuotedStr(tbWakilPenjual.Text) + "," + QuotedStr(tbDokumen.Text) + ", '" + Format(tbDateDokumen.SelectedValue, "yyyy-MM-dd") + "', " + _
                QuotedStr(tbDiscHd.Text.Replace(",", "")) + " ," + QuotedStr(tbDPValue.Text.Replace(",", "")) + ", " + QuotedStr(tbTJHd.Text.Replace(",", "")) + ", " + QuotedStr(tbSisaBayarHd.Text.Replace(",", "")) + "," + _
                QuotedStr(tbpphFinal.Text.Replace(",", "")) + ", " + QuotedStr(tbPphFinalValue.Text.Replace(",", "")) + "," + _
                QuotedStr(tbRemark.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", getDate()"


            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM MKTTSPHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE MKTTSPHd SET Transdate = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "' " + _
                ",MasaAngsuran = " + QuotedStr(tbAngsuran.Text) + _
                ",WakilCode =  " + QuotedStr(tbWakilPenjual.Text) + _
                ",BatasDokumen = " + QuotedStr(tbDokumen.Text) + _
                ",BatasDokumenDate= '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + _
                "', CustomerCode = " + QuotedStr(tbCustomerCode.Text) + _
                ", FgSewa = " + QuotedStr(ddlFgsewa.SelectedValue) + _
                ", SPNo = " + QuotedStr(tbSpNo.Text) + _
                ", Lokasi = " + QuotedStr(tbArea.Text) + _
                ", DepCode = " + QuotedStr(ddlDept.SelectedValue) + _
                ", POCustomer = " + QuotedStr(tbPoCust.Text) + _
                ", PIC1 = " + QuotedStr(tbPICName.Text) + _
                ", PICTlp1 = " + QuotedStr(tbPICTelp.Text) + _
                ", PICEmail1 = " + QuotedStr(tbPICEmail.Text) + _
                ", PIC2 = " + QuotedStr(tbPICName2.Text) + _
                ", PICTlp2 = " + QuotedStr(tbPICTelp2.Text) + _
                ", PICEmail2 = " + QuotedStr(tbPICEmail2.Text) + _
                ", Perantara1 = " + QuotedStr(tbPeName.Text) + _
                ", PerantaraTelp1 = " + QuotedStr(tbPeTelp.Text) + _
                ", PerantaraEmail1 = " + QuotedStr(tbPeEmail.Text) + _
                ", Perantara2 = " + QuotedStr(tbPeName2.Text) + _
                ", PerantaraTelp2 = " + QuotedStr(tbPeTelp2.Text) + _
                ", PerantaraEmail2 = " + QuotedStr(tbPeEmail2.Text) + _
                ", TermCode = " + QuotedStr(ddlTerm.Text) + _
                ", Currency = " + QuotedStr(ddlCurr.Text) + _
                ", Rate = " + QuotedStr(tbRate.Text) + _
                ", RencanaBayar = " + QuotedStr(ddlRencanaBayar.SelectedValue) + _
                ", Bank = " + QuotedStr(tbBank.Text) + _
                ", TotalHarga = " + QuotedStr(tbTotHarga.Text.Replace(",", "")) + _
                ", TotalDisc= " + QuotedStr(tbTotDisc.Text.Replace(",", "")) + _
                ", TotalDpp= " + QuotedStr(tbTotDPP.Text.Replace(",", "")) + _
                ", TotalPpn= " + QuotedStr(tbTotPPN.Text.Replace(",", "")) + _
                ", TotalPph= " + QuotedStr(tbTotPPH.Text.Replace(",", "")) + _
                ", TotalAmount= " + QuotedStr(tbTotAmount.Text.Replace(",", "")) + _
                ", Disc= " + QuotedStr(tbDiscHd.Text.Replace(",", "")) + _
                ", Dp = " + QuotedStr(tbDPValue.Text.Replace(",", "")) + _
                ", TandaJadi = " + QuotedStr(tbTJHd.Text.Replace(",", "")) + _
                ", SisaPembayaran = " + QuotedStr(tbSisaBayarHd.Text.Replace(",", "")) + _
                 ", PphFinal = " + QuotedStr(tbpphFinal.Text.Replace(",", "")) + _
                ", PphFinalValue = " + QuotedStr(tbPphFinalValue.Text.Replace(",", "")) + _
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
            Dim cmdSql As New SqlCommand(" SELECT TransNmbr, UnitCode, LuasTanah,LuasBangunan ,ArahKavling, KtinggianDPL, PricePer_m2, Price, Disc, DiscValue, Dpp, PpnTotal, PphTotal, AmountTotal, DateSerahKunci, StartSewa, EndSewa, Remark, TJ, DP, SisaTagihan FROM MKTTSPDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand


            Dim Dt As New DataTable("MKTTSPDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt

            'save dt2
            cmdSql = New SqlCommand("SELECT TransNmbr, ItemNo, UnitCode, PayDate,Type, PayKav, Ppn, PpnValue, Pph, PphValue, AmountDt2, Remark, BankCode, NoGiro, AngsuranKe FROM MKTTSPDt2 WHERE TransNmbr  = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            dbcommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt2 As New DataTable("MKTTSPDt2")

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
        Dim cekQty As Boolean
        Try

            'Dim confirmValue As String = Request.Form("confirm_value")
            'If confirmValue = "Yes" Then

            '    If CekHd() = False Then
            '        Exit Sub
            '    End If
            '    If GetCountRecord(ViewState("Dt")) = 0 Then
            '        lbStatus.Text = MessageDlg("Detail must have at least 1 record")
            '        Exit Sub
            '    End If

            '    If GetCountRecord(ViewState("Dt2")) = 0 Then
            '        lbStatus.Text = MessageDlg("Detail Payment must have at least 1 record")
            '        Exit Sub
            '    End If

            '    For Each dr In ViewState("Dt").Rows
            '        If CekDt(dr) = False Then
            '            Exit Sub
            '        End If
            '    Next

            '    SaveAll()
            '    ModifyInput2(False, pnlInput, pnlDt, GridDt)
            '    ModifyInput2(False, pnlInput, pnlDt2, GridDt2)
            '    btnGoEdit.Visible = True
            '    Menu2.Items.Item(1).Enabled = True
            '    MultiView2.ActiveViewIndex = 1
            '    Menu2.Items.Item(1).Selected = True
            '    'btnGoEdit.Visible = True
            '    btnSP.Visible = False
            '    GridDt.Columns(0).Visible = False
            '    'MovePanel(pnlInput, PnlHd)
            '    'CurrFilter = tbFilter.Text
            '    'Value = ddlField.SelectedValue
            '    'tbFilter.Text = tbCode.Text
            '    'ddlField.SelectedValue = "TransNmbr"
            '    'btnSearch_Click(Nothing, Nothing)
            '    'tbFilter.Text = CurrFilter
            '    'ddlField.SelectedValue = Value

            '    ' ClientScript.RegisterStartupScript(Me.[GetType](), "alert", "alert('You clicked YES!')", True)
            'Else

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
                lbStatus.Text = MessageDlg("Detail Payment schedule must have at least 1 record")
                Exit Sub
            End If
            For Each dr In ViewState("Dt").Rows
                If CekDt(dr) = False Then
                    Exit Sub
                End If
            Next

            cekQty = checkQty()
            If cekQty = False Then
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
            'ClientScript.RegisterStartupScript(Me.[GetType](), "alert", "alert('You clicked NO!')", True)
            ' End If


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
            btnSP.Visible = True
            EnableHd(True)
            ddlFgsewa_SelectedIndexChanged(Nothing, Nothing)
            btnGetAngsuran.Visible = True
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
                btnSP.Visible = True
                GridDt.Columns(0).Visible = True
            End If
        End If
        If Menu2.Items.Item(1).Selected = True Then
            If ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit" Then
                'btnGoEdit.Visible = True
                btnSP.Visible = False
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
            tbAngsuran.Text = 0
            tbDokumen.Text = 0
            tbDateDokumen.SelectedDate = ViewState("ServerDate") 'Today
            tbWakilPenjual.Text = ""
            tbNamaWakilPenjual.Text = ""
            tbCustomerCode.Text = ""
            tbCustomerName.Text = ""
            tbSpNo.Text = ""
            tbArea.Text = ""
            tbAreaName.Text = ""
            tbPICName.Text = ""
            tbPICTelp.Text = ""
            tbPICEmail.Text = ""
            tbPICName2.Text = ""
            tbPICTelp2.Text = ""
            tbPICEmail2.Text = ""
            tbPeName.Text = ""
            tbPeTelp.Text = ""
            tbPeEmail.Text = ""
            tbPeName2.Text = ""
            tbPeTelp2.Text = ""
            tbPeEmail2.Text = ""
            MultiView1.ActiveViewIndex = 0
            tbTotHarga.Text = "0"
            tbTotDisc.Text = "0"
            tbTotPPN.Text = "0"
            tbTotPPH.Text = "0"
            tbTotalAmount.Text = "0"
            ddlCurr.SelectedValue = ViewState("Currency")
            tbBank.Text = ""
            tbTJHd.Text = "0"
            tbDPValue.Text = "0"
            tbSisaBayarHd.Text = "0"
            tbDiscHd.Text = "0"
            tbTotDPP.Text = "0"
            tbpphFinal.Text = "0"
            tbPphFinalValue.Text = "0"

            ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try

            TbUnit.Text = ""
            tbUnitName.Text = ""
            tbLuasTanah.Text = 0
            tbLuasBangunan.Text = 0
            tbArahKav.Text = ""
            tbKtinggianDPL.Text = 0
            tbPricePerm2.Text = 0
            tbPrice.Text = 0
            tbDisc.Text = 0
            tbDiscValue.Text = 0
            tbPpnTotal.Text = 0
            tbPphtotal.Text = 0
            tbQtyTJ.Text = 0
            tbQtyDP.Text = 0
            tbQtySisaBayar.Text = 0
            tbDpp.Text = 0
            tbTotalAmount.Text = 0
            tbRemarkDt.Text = ""
            tbSerahKunci.SelectedDate = ViewState("ServerDate")
            tbStartSewa.SelectedDate = ViewState("ServerDate")
            tbEndSewa.SelectedDate = ViewState("ServerDate")
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt2()
        Try
            tbTempoDate.SelectedDate = ViewState("ServerDate") 'Today
            tbNominal.Text = 0
            tbPpnDt2.Text = ViewState("PPN")
            tbPpnValuedt2.Text = 0
            tbPphDt2.Text = 0
            tbPphValueDt2.Text = 0
            tbTotalNominal.Text = 0
            lblAngsuran.Text = 0
            tbGiroNo.Text = ""
            ddlBank.SelectedValue = ""
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
        Dim cekQty As Boolean
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

            cekQty = checkQty()
            If cekQty = False Then
                Exit Sub
            End If

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
        btnSaveDt.Focus()
        If ddlFgsewa.SelectedValue = "N" Then
            tbSerahKunci.Enabled = False
            tbStartSewa.Enabled = False
            tbEndSewa.Enabled = False
        Else
            tbSerahKunci.Enabled = True
            tbStartSewa.Enabled = True
            tbEndSewa.Enabled = True
        End If
       
    End Sub



    'Public Function GetNewItemNo()
    '    Dim Row As DataRow()
    '    Dim R As DataRow
    '    Dim MaxItem As Integer = 0
    '    Row = ViewState("Dt2").Select("UnitCode = " + QuotedStr(TrimStr(lblUnit.Text)))

    '    For Each R In Row
    '        If CInt(R("ItemNo").ToString) > MaxItem Then
    '            MaxItem = CInt(R("ItemNo").ToString)
    '        End If
    '    Next
    '    MaxItem = MaxItem + 1
    '    Return CStr(MaxItem)
    'End Function




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
            btnSaveDt2.Focus()
            ddlType_SelectedIndexChanged(Nothing, Nothing)
            lbItemNo.Text = GetNewItemNo(ViewState("Dt2"))
            If ddlFgsewa.SelectedValue = "N" Then
                tbPphDt2.Enabled = False
            Else
                tbPphDt2.Enabled = True
            End If
            'lbItemNo.Text = GetNewItemNo()
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
            If tbCustomerCode.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Customer must have value")
                tbCustomerCode.Focus()
                Return False
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
                If Dr("UnitCode").ToString.Trim = "" Then
                    lbStatus.Text = "Unit Code Must Have Value"
                    Return False
                End If

                If Dr("Price").ToString.Trim = "" Then
                    lbStatus.Text = "Unit Code Must Have Value"
                    Return False
                End If

            Else
                If TbUnit.Text.Trim = "" Then
                    lbStatus.Text = "Unit No Must Have Value"
                    TbUnit.Focus()
                    Return False
                End If

                If TbUnit.Text.Trim = "" Then
                    lbStatus.Text = "Unit No Must Have Value"
                    TbUnit.Focus()
                    Return False
                End If


                If tbPrice.Text = "" Or tbPrice.Text = 0 Then
                    lbStatus.Text = "Price Must Have Value"
                    tbPrice.Focus()
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

                If Dr("Biaya").ToString = "0" Or Dr("Biaya").ToString = "" Then
                    lbStatus.Text = MessageDlg("Biaya Must Have Value")
                    Return False
                End If

            Else

                'If tbTotalAmount.Text = "" Or tbTotalAmount.Text = "0" Then
                '    lbStatus.Text = MessageDlg("BAP % Must Have Value")
                '    tbTotalAmount.Focus()
                '    Return False
                'End If


                If tbNominal.Text = "" Or tbNominal.Text = "0" Then
                    lbStatus.Text = MessageDlg("Biaya Must Have Value")
                    tbNominal.Focus()
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
            FDateValue = "TransDate, CustomerInvDate"
            FilterName = "Reference, Date, Status, Customer Code, Customer Name,  Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), Status, CustomerCode, CustomerName, Remark"
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

                    Menu2.Items.Item(1).Enabled = False
                    ddlFgsewa_SelectedIndexChanged(Nothing, Nothing)
                    GridDt.Columns(1).Visible = False

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

                        'btnSP.Visible = False
                        'btnAddDt.Visible = False
                        'btnAddDtKe2.Visible = False

                        Menu2.Items.Item(1).Enabled = False
                        ddlFgsewa_SelectedIndexChanged(Nothing, Nothing)
                        If ddlFgsewa.SelectedValue = "N" Then
                            tbPphDt2.Enabled = False
                        Else
                            tbPphDt2.Enabled = True
                        End If
                        'lbItemN

                        If ddlFgsewa.SelectedValue = "N" Then
                            tbSerahKunci.Enabled = False
                            tbStartSewa.Enabled = False
                            tbEndSewa.Enabled = False
                        Else
                            tbSerahKunci.Enabled = True
                            tbStartSewa.Enabled = True
                            tbEndSewa.Enabled = True
                        End If


                    Else
                        lbStatus.Text = MessageDlg("Data must Hold or Get Approval to edit")
                        Exit Sub
                    End If

                ElseIf DDL.SelectedValue = "Cancel" Then
                    CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                    If CekMenu <> "" Then
                        lbStatus.Text = CekMenu
                        Exit Sub
                    End If

                    If GVR.Cells(3).Text = "H" Then
                        lbStatus.Text = MessageDlg("Status Surat Pesanan Is not H or G, cannot Cancel Pesanan")
                        Exit Sub
                    End If

                    If GVR.Cells(3).Text = "C" Then
                        lbStatus.Text = MessageDlg("Surat Pesanan Cancel Already")
                        Exit Sub
                    End If

                    index = Convert.ToInt32(e.CommandArgument)
                    GVR = GridView1.Rows(index)

                    ViewState("TransNmbr") = GVR.Cells(2).Text
                    AttachScript("Reject();", Page, Me.GetType)

                ElseIf DDL.SelectedValue = "Print" Then
                    Try
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If

                        Session("DBConnection") = ViewState("DBConnection")
                        Session("SelectCommand") = "EXEC S_MKTFormTSP '''" + GVR.Cells(2).Text + "''', " + QuotedStr(ViewState("UserId").ToString) + " "
                        Session("SelectCommand2") = "EXEC S_MKTFormTSP2 '''" + GVR.Cells(2).Text + "''', " + QuotedStr(ViewState("UserId").ToString) + " "
                        Session("ReportFile") = ".../../../Rpt/FormMKTTSP.frx"
                        AttachScript("openprintdlg2ds();", Page, Me.GetType)

                    Catch ex As Exception
                        lbStatus.Text = "btn print Error = " + ex.ToString
                    End Try

                ElseIf DDL.SelectedValue = "Tanda Terima" Then
                    CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                    If CekMenu <> "" Then
                        lbStatus.Text = CekMenu
                        Exit Sub
                    End If
                    Session("DBConnection") = ViewState("DBConnection")
                    Session("SelectCommand") = "EXEC S_MKTFormTSP2 '''" + GVR.Cells(2).Text + "''', " + QuotedStr(ViewState("UserId").ToString) + " "
                    Session("ReportFile") = ".../../../Rpt/FormMKTTSP2.frx"
                    AttachScript("openprintdlg();", Page, Me.GetType)


                ElseIf DDL.SelectedValue = "Kwitansi Global" Then
                    CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                    If CekMenu <> "" Then
                        lbStatus.Text = CekMenu
                        Exit Sub
                    End If
                    Session("DBConnection") = ViewState("DBConnection")
                    Session("SelectCommand") = "EXEC S_FNFormKwitansiGlobal '''" + GVR.Cells(2).Text + "''', " + QuotedStr(ViewState("UserId").ToString) + " "
                    Session("ReportFile") = ".../../../Rpt/S_FNFormKwitansiGlobal.frx"
                    AttachScript("openprintdlg();", Page, Me.GetType)
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

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        Dim index As Integer
        Try
            index = Int32.Parse(e.Item.Value)
            MultiView1.ActiveViewIndex = index
            btnSaveTrans.Focus()
            btnGetAngsuran.Visible = True
        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
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

                lblUnit.Text = GVR.Cells(2).Text
                lblUnitName.Text = GVR.Cells(3).Text

                MultiView1.ActiveViewIndex = 1

                If ViewState("StateHd") = "View" Then
                    ModifyInput2(False, pnlInput, pnlDt2, GridDt2)
                Else
                    ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
                End If
                btnGetAngsuran.Visible = True
                'GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
                If ViewState("Dt2") Is Nothing Then
                    BindDataDt2(ViewState("TransNmbr"))
                End If
                Dim drow As DataRow()
                drow = ViewState("Dt2").Select("UnitCode = " + QuotedStr(lblUnit.Text)) '+ " AND FAName = " + QuotedStr(TrimStr(lbFANameDt2.Text)) + " AND FAStatus = " + QuotedStr(TrimStr(lbStatusFA.Text)))
                '                drow = ViewState("Dt2").Select("FixedAsset = " + QuotedStr(TrimStr(lbFADt2.Text)))



                If drow.Length > 0 Then
                    BindGridDt(drow.CopyToDataTable, GridDt2)
                    GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
                    'GridDt2.Columns(0).Visible = False
                    'btnAddDt2.Visible = False
                    'btnAddDt2Ke2.Visible = False
                Else
                    Dim DtTemp As DataTable
                    DtTemp = ViewState("Dt2").Clone
                    DtTemp.Rows.Add(DtTemp.NewRow())
                    GridDt2.DataSource = DtTemp
                    GridDt2.DataBind()
                    'GridDt2.Columns(0).Visible = False
                End If

                btnBackDt2ke2.Focus()
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
            dr = ViewState("Dt").Select("UnitCode = " + QuotedStr(GVR.Cells(2).Text))
            dr(0).Delete()
            BindGridDt(ViewState("Dt"), GridDt)
            ' EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
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
            BindGridDt(ViewState("Dt2"), GridDt2)

            'Dim drow As DataRow()
            'drow = ViewState("Dt2").Select("UnitCode = " + QuotedStr(TrimStr(lblUnit.Text)))
            'If drow.Length > 0 Then
            '    BindGridDt(drow.CopyToDataTable, GridDt2)
            '    GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
            'Else
            '    Dim DtTemp As New DataTable
            '    DtTemp = ViewState("Dt2").Clone
            '    DtTemp.Rows.Add(DtTemp.NewRow()) ' create a new blank row to the DataTable
            '    GridDt2.DataSource = DtTemp
            '    GridDt2.DataBind()
            '    GridDt2.Columns(0).Visible = False
            'End If

            CountTotalDt()
            'BindGridDt(ViewState("Dt2"), GridDt2)
            'EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
            btnBackDt2ke2.Focus()
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
            'AttachScript("setformathd();", Page, Me.GetType())
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
            BindToText(tbCustomerCode, Dt.Rows(0)("CustomerCode").ToString)
            BindToText(tbCustomerName, Dt.Rows(0)("CustomerName").ToString)
            BindToText(tbSpNo, Dt.Rows(0)("SPNo").ToString)
            BindToText(tbArea, Dt.Rows(0)("Lokasi").ToString)
            BindToText(tbAreaName, Dt.Rows(0)("StructureName").ToString)

            'BindToDropList(ddlDept, Dt.Rows(0)("Department").ToString)
            BindToText(tbPoCust, Dt.Rows(0)("PoCustomer").ToString)
            BindToText(tbPICName, Dt.Rows(0)("PIC1").ToString)
            BindToText(tbPICTelp, Dt.Rows(0)("PICTlp1").ToString)
            BindToText(tbPICEmail, Dt.Rows(0)("PICEmail1").ToString)
            BindToText(tbPICName2, Dt.Rows(0)("PIC2").ToString)
            BindToText(tbPICTelp2, Dt.Rows(0)("PICTlp2").ToString)
            BindToText(tbPICEmail2, Dt.Rows(0)("PICEmail2").ToString)

            BindToText(tbPeName, Dt.Rows(0)("Perantara1").ToString)
            BindToText(tbPeTelp, Dt.Rows(0)("PerantaraTelp1").ToString)
            BindToText(tbPeEmail, Dt.Rows(0)("PerantaraEmail1").ToString)

            BindToText(tbPeName2, Dt.Rows(0)("Perantara2").ToString)
            BindToText(tbPeTelp2, Dt.Rows(0)("PerantaraTelp2").ToString)
            BindToText(tbPeEmail2, Dt.Rows(0)("PerantaraEmail2").ToString)

            BindToDropList(ddlRencanaBayar, Dt.Rows(0)("RencanaBayar").ToString)
            BindToText(tbBank, Dt.Rows(0)("Bank").ToString)

            BindToText(tbTotHarga, Dt.Rows(0)("TotalHarga").ToString, ViewState("DigitCurr"))
            BindToText(tbTotDisc, Dt.Rows(0)("TotalDisc").ToString, ViewState("DigitCurr"))
            BindToText(tbTotDPP, Dt.Rows(0)("TotalDPP").ToString, ViewState("DigitCurr"))
            BindToText(tbTotPPN, Dt.Rows(0)("TotalPPn").ToString, ViewState("DigitCurr"))
            BindToText(tbTotPPH, Dt.Rows(0)("TotalPph").ToString, ViewState("DigitCurr"))
            BindToText(tbTotAmount, Dt.Rows(0)("TotalAmount").ToString, ViewState("DigitCurr"))
            BindToText(tbDPValue, Dt.Rows(0)("DP").ToString, ViewState("DigitCurr"))
            BindToText(tbTJHd, Dt.Rows(0)("TandaJadi").ToString, ViewState("DigitCurr"))
            BindToText(tbSisaBayarHd, Dt.Rows(0)("SisaPembayaran").ToString, ViewState("DigitCurr"))
            BindToText(tbDiscHd, Dt.Rows(0)("Disc").ToString, ViewState("DigitCurr"))
            BindToDropList(ddlTerm, Dt.Rows(0)("TermCode").ToString)
            BindToText(tbRate, Dt.Rows(0)("Rate").ToString)
            BindToDropList(ddlCurr, Dt.Rows(0)("Currency").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)


            BindToText(tbAngsuran, Dt.Rows(0)("MasaAngsuran").ToString)
            BindToText(tbDokumen, Dt.Rows(0)("BatasDokumen").ToString)
            BindToDate(tbDateDokumen, Dt.Rows(0)("BatasDokumenDate").ToString)
            BindToText(tbWakilPenjual, Dt.Rows(0)("WakilCode").ToString)
            BindToText(tbNamaWakilPenjual, Dt.Rows(0)("WakilName").ToString)
            BindToDropList(ddlFgsewa, Dt.Rows(0)("Fgsewa").ToString)


            BindToText(tbpphFinal, Dt.Rows(0)("PphFinal").ToString, ViewState("DigitCurr"))
            BindToText(tbPphFinalValue, Dt.Rows(0)("PphFinalValue").ToString, ViewState("DigitCurr"))

            ''1
            'If Dt.Rows(0)("DokInvoice").ToString = "" Then
            '    'cbKtp.Checked = False
            '    lbDokInv.Text = "Not Yet Uploaded"
            'Else
            '    lbDokInv.Text = Dt.Rows(0)("DokInvoice").ToString
            '    'cbKtp.Checked = True
            'End If

            ''2
            'If Dt.Rows(0)("DokFaktur").ToString = "" Then
            '    lbFaktur.Text = "Not Yet Uploaded"
            'Else
            '    lbFaktur.Text = Dt.Rows(0)("DokFaktur").ToString
            'End If

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

    Protected Sub FillTextBoxDt(ByVal FixedAsset As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("UnitCode= " + QuotedStr(FixedAsset))
            If Dr.Length > 0 Then
                BindToText(TbUnit, Dr(0)("UnitCode").ToString)
                BindToText(tbUnitName, Dr(0)("UnitName").ToString)
                BindToText(tbLuasBangunan, Dr(0)("LuasBangunan").ToString)
                BindToText(tbLuasTanah, Dr(0)("LuasTanah").ToString)
                BindToText(tbKtinggianDPL, Dr(0)("KtinggianDPL").ToString)
                BindToText(tbArahKav, Dr(0)("ArahKavling").ToString)
                BindToText(tbPricePerm2, Dr(0)("PricePer_m2").ToString, ViewState("DigitHome"))
                BindToText(tbPrice, Dr(0)("Price").ToString, ViewState("DigitHome"))
                BindToText(tbDisc, Dr(0)("Disc").ToString, ViewState("DigitHome"))
                BindToText(tbDiscValue, Dr(0)("Disc").ToString, ViewState("DigitHome"))
                BindToText(tbDpp, Dr(0)("Disc").ToString, ViewState("DigitHome"))
                BindToText(tbPpnTotal, Dr(0)("PpnTotal").ToString, ViewState("DigitHome"))
                BindToText(tbPphtotal, Dr(0)("PphTotal").ToString, ViewState("DigitHome"))
                BindToText(tbTotalAmount, Dr(0)("AmountTotal").ToString, ViewState("DigitHome"))
                BindToText(tbQtyTJ, Dr(0)("TJ").ToString, ViewState("DigitHome"))
                BindToText(tbQtyDP, Dr(0)("DP").ToString, ViewState("DigitHome"))
                BindToText(tbQtySisaBayar, Dr(0)("SisaTagihan").ToString, ViewState("DigitHome"))
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
                BindToDate(tbSerahKunci, Dr(0)("DateSerahKunci").ToString)
                BindToDate(tbStartSewa, Dr(0)("StartSewa").ToString)
                BindToDate(tbEndSewa, Dr(0)("EndSewa").ToString)

            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt2(ByVal FALoc As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt2").select("ItemNo = " + QuotedStr(FALoc))
            If Dr.Length > 0 Then

                lbItemNo.Text = Dr(0)("ItemNo").ToString
                BindToDate(tbTempoDate, Dr(0)("PayDate").ToString)
                BindToDropList(ddlType, Dr(0)("Type").ToString)
                BindToText(tbNominal, Dr(0)("PayKav").ToString, ViewState("DigitHome"))
                BindToText(tbPpnDt2, Dr(0)("Ppn").ToString, ViewState("DigitHome"))
                BindToText(tbPpnValuedt2, Dr(0)("PpnValue").ToString, ViewState("DigitHome"))
                BindToText(tbPphDt2, Dr(0)("Pph").ToString, ViewState("DigitHome"))
                BindToText(tbPphValueDt2, Dr(0)("PphValue").ToString, ViewState("DigitHome"))
                BindToText(tbTotalNominal, Dr(0)("amountDt2").ToString, ViewState("DigitHome"))
                BindToDropList(ddlBank, Dr(0)("BankCode").ToString)
                BindToText(tbGiroNo, Dr(0)("NoGiro").ToString)
                lblAngsuran.Text = Dr(0)("AngsuranKe").ToString
                BindToText(tbRemarkDt2, Dr(0)("Remark").ToString)
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
            'FillTextBoxDt2(GVR.Cells(1).Text + "|" + lblUnit.Text)
            FillTextBoxDt2(GVR.Cells(1).Text)
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
            btnSaveTrans.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn back Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCust_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCust.Click
        Dim ResultField As String
        Try
            Session("filter") = "select * from V_MsCustomer"
            ResultField = "Customer_Code, Customer_Name"
            ViewState("Sender") = "btnCust"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchGrid();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Customer Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnGetInv_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSP.Click
        Dim ResultField, CriteriaField, sqlstring, ResultSame As String
        Try

            Session("filter") = "SELECT * FROM V_GetNoPenawaran "
            ResultField = "TransNmbr,MasaAngsuran,SystemPembayaran,Disc,TotalDisc,TotalDpp,Area,AreaName,FgSewa,Perantara1,Perantara2"
            ViewState("Sender") = "btnSP"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchGrid();", Page, Me.GetType())

            'sqlstring = "SELECT * FROM V_GetNoPenawaran "

            'Session("DBConnection") = ViewState("DBConnection")
            'Session("filter") = sqlstring
            'ResultField = "TransNmbr,MasaAngsuran,SystemPembayaran,Disc,TotalDisc,Area,AreaName,FgSewa,Perantara1,Perantara2"
            'CriteriaField = "TransNmbr,MasaAngsuran,SystemPembayaran,Disc,TotalDisc,Area,AreaName,FgSewa,Perantara1,Perantara2"
            ''Session("ClickSame") = "Bill_To"
            'Session("Column") = ResultField.Split(",")
            'Session("CriteriaField") = CriteriaField.Split(",")
            'ResultSame = "TransNmbr"
            'Session("ResultSame") = ResultSame.Split(",")
            'ViewState("Sender") = "btnSP"
            'AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
            'AttachScript("FindMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Customer Click Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub tbCustomerCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCustomerCode.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Customer", tbCustomerCode.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbCustomerCode.Text = Dr("Customer_Code")
                tbCustomerName.Text = Dr("Customer_Name")
                BindToDropList(ddlCurr, Dr("Currency"))
                BindToDropList(ddlTerm, Dr("Term"))
            Else
                tbCustomerCode.Text = ""
                tbCustomerName.Text = ""
                ddlCurr.SelectedValue = Session("Currency")
            End If
            ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
            'AttachScript("setformat();", Page, Me.GetType())
            tbCustomerCode.Focus()
        Catch ex As Exception
            Throw New Exception("tb CustomerCode Error : " + ex.ToString)
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


    Protected Sub ddlFgsewa_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFgsewa.SelectedIndexChanged
        If ddlFgsewa.SelectedValue = "Y" Then
            GridDt.Columns(8).Visible = True
            GridDt.Columns(9).Visible = True
            GridDt.Columns(10).Visible = True
        Else
            GridDt.Columns(8).Visible = False
            GridDt.Columns(9).Visible = False
            GridDt.Columns(10).Visible = False


        End If
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


    Protected Sub lbWakilPenjual_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbWakilPenjual.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsTugas')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lbWakilPembeli_Click Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnWakilPenjual_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnWakilPenjual.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT TugasCode, TugasName from V_MsPemberiTugas "
            ResultField = "TugasCode, TugasName"
            ViewState("Sender") = "btnWakilPenjual"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchGrid();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Supp Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbCust_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCust.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsCustomer')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb Supplier Error : " + ex.ToString
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



    Protected Sub ddlType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlType.SelectedIndexChanged

        Dim Nilai As String


        Try
            If ddlType.SelectedValue = "AG" Then
                CountTotalRecordAngsuran()
                tbRemarkDt2.Text = ""

            ElseIf ddlType.SelectedValue = "DP" Then
                tbTempoDate.SelectedDate = DateAdd(DateInterval.Month, 2, tbDate.SelectedDate)
                tbRemarkDt2.Text = "Sesuai tanggal  PPJB / AJB Notaris, masksimal 2 bulan setelah tanggal surat pesanan ini"
                lblAngsuran.Text = 0

                'tbRemarkDt2.Text = ""
                Nilai = SQLExecuteScalar("SELECT Nilai FROM V_GetDpTJ WHERE  TypeAngsuran = 'DP'  AND TransNmbr = " + QuotedStr(tbSpNo.Text), ViewState("DBConnection"))
                tbNominal.Text = Nilai
                AttachScript("setformatdt2();", Page, Me.GetType())

            ElseIf ddlType.SelectedValue = "TJ" Then
                tbTempoDate.SelectedDate = tbDate.Text
                tbRemarkDt2.Text = ""
                Nilai = SQLExecuteScalar("SELECT Nilai FROM V_GetDpTJ WHERE TypeAngsuran = 'TJ'  AND TransNmbr = " + QuotedStr(tbSpNo.Text), ViewState("DBConnection"))
                tbNominal.Text = Nilai
                AttachScript("setformatdt2();", Page, Me.GetType())
            Else
                lblAngsuran.Text = 0

            End If
            btnSaveDt2.Focus()

        Catch ex As Exception
            lbStatus.Text = "ddlCategory_SelectedIndexChanged Error : " + ex.ToString
        End Try
    End Sub



    Protected Sub btnGetAngsuran_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetAngsuran.Click

        Dim drow() As DataRow
        Dim dt As New DataTable
        Dim GVR As GridViewRow
        Dim baris As Integer
        Dim i As Integer
        Dim Unit, UnitAG, UnitTJ As String
        Dim qty, QtyTotal As Double
        Dim havedetail As Boolean

        Try

            If tbAngsuran.Text = 0 Then
                lbStatus.Text = MessageDlg("Generate failed, masa angsuran must have value")
            End If

            Dim Dr2 As DataRow


            Dim drDtResult As DataRow
            Dim ExistRowDT As DataRow()
            Dim MaxItem As String
            Dim DtPekerjaan As DataTable
            Dim SQLString As String
            Dim DP, TJ, SisaTagihan As Double
            Dim DateAngsuran As Date
            Dim ItemNo As Integer


            UnitAG = "DP"
            drow = ViewState("Dt2").Select("Type = " + QuotedStr(UnitAG))

            'drow(0).Delete()
            If drow.Length > 0 Then
                havedetail = False
                For Each Dr In drow.CopyToDataTable.Rows
                    If Not Dr.RowState = DataRowState.Deleted Then
                        DP = DP + CFloat(Dr("PayKav").ToString)
                        DateAngsuran = (Dr("PayDate").ToString)
                        ItemNo = (Dr("ItemNo").ToString)

                    End If
                Next
            End If

            'lbStatus.Text = DP
            'Exit Sub


            UnitTJ = "TJ"
            drow = ViewState("Dt2").Select("Type = " + QuotedStr(UnitTJ))
            If drow.Length > 0 Then
                havedetail = False
                For Each Dr In drow.CopyToDataTable.Rows
                    If Not Dr.RowState = DataRowState.Deleted Then
                        TJ = TJ + CFloat(Dr("PayKav").ToString)
                    End If
                Next
            End If

            'lbStatus.Text = TJ
            'Exit Sub

            SisaTagihan = tbTotDPP.Text - Val(TJ + DP)
            'lbStatus.Text = SisaTagihan
            'Exit Sub


            If DP = 0 Then
                DateAngsuran = tbDate.SelectedDate
            End If


            'lbStatus.Text = SQLString
            'Exit Sub

            ' Delete Record yang typenya Angsuran
            Dim dtee() As DataRow
            dtee = ViewState("Dt2").Select("Type = 'AG'")
            For i = 0 To dtee.Length - 1  'For i = 0 To baris - 1
                Dim del2() As DataRow
                Dim GVR2 As GridViewRow
                GVR2 = GridDt2.Rows(1)
                del2 = ViewState("Dt2").Select("Type = 'AG'")
                del2(0).Delete()
                'lbStatus.Text = GVR2.Cells(3).Text
                'Exit Sub
            Next

            SQLString = "EXEC S_GetCicilan '', " + QuotedStr(SisaTagihan) + ", " + QuotedStr(ViewState("PPN")) + ", " + QuotedStr(tbAngsuran.Text) + ", " + QuotedStr(DateAngsuran)
            DtPekerjaan = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)

            'Insert Angsuran
            For Each drDtResult In DtPekerjaan.Rows
                ExistRowDT = ViewState("Dt2").Select("ItemNo = " + QuotedStr(drDtResult("ItemNo".ToString)))
                If ExistRowDT.Count = 0 Then
                    Dim Dtdr As DataRow
                    Dtdr = ViewState("Dt2").NewRow
                    Dtdr("ItemNo") = drDtResult("ItemNo".ToString)
                    Dtdr("PayDate") = drDtResult("DateAgsuran".ToString)
                    Dtdr("Type") = drDtResult("TypeAngsuran".ToString)
                    Dtdr("TypeName") = drDtResult("TypeName".ToString)
                    Dtdr("PayKav") = drDtResult("Angsuran".ToString)
                    Dtdr("PPn") = drDtResult("PPn".ToString)
                    Dtdr("PpnValue") = drDtResult("PpnValue".ToString)
                    Dtdr("Pph") = 0
                    Dtdr("PphValue") = 0
                    Dtdr("AmountDt2") = drDtResult("TotalAngsuran".ToString)
                    Dtdr("AngsuranKe") = drDtResult("Period".ToString)
                    Dtdr("Remark") = drDtResult("Remark".ToString)
                    ViewState("Dt2").Rows.Add(Dtdr)
                Else
                    'Update jika sudah ada
                    Dim Row As DataRow
                    Row = ViewState("Dt2").Select("ItemNo= " + QuotedStr(drDtResult("ItemNo".ToString)))(0)
                    Row.BeginEdit()
                    Row("ItemNo") = drDtResult("ItemNo".ToString)
                    Row("PayDate") = drDtResult("DateAgsuran".ToString)
                    Row("Type") = drDtResult("TypeAngsuran".ToString)
                    Row("TypeName") = drDtResult("TypeName".ToString)
                    Row("PayKav") = drDtResult("Angsuran".ToString)
                    Row("PPn") = drDtResult("PPn".ToString)
                    Row("PpnValue") = drDtResult("PpnValue".ToString)
                    Row("Pph") = 0
                    Row("PphValue") = 0
                    Row("AmountDt2") = drDtResult("TotalAngsuran".ToString)
                    Row("AngsuranKe") = drDtResult("Period".ToString)
                    Row("Remark") = drDtResult("Remark".ToString)
                    Row.EndEdit()
                End If
            Next
            BindGridDt(ViewState("Dt2"), GridDt2)

            CountTotalDt()
            btnCancelDt2.Visible = True
            btnSaveDt2.Visible = True

            'EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
            'BindGridDt(ViewState("Dt2"), GridDt2)
            StatusButtonSave(True)
            btnBackDt2ke2.Focus()



        Catch ex As Exception
            lbStatus.Text = "btnangsuran Error : " + ex.ToString
        End Try
    End Sub



    Protected Sub tbPpnFinal_TextChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbpphFinal.TextChanged
        Try
            tbPphFinalValue.Text = CFloat(tbTotDPP.Text) * CFloat(tbpphFinal.Text) / 100
            
            tbPphFinalValue.Text = FormatNumber(tbPphFinalValue.Text, ViewState("DigitHome"))
        Catch ex As Exception

        End Try
    End Sub



    Protected Sub tbPphFinalValue_TextChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPphFinalValue.TextChanged
        Try
            tbpphFinal.Text = CFloat(tbPphFinalValue.Text) / CFloat(tbTotDPP.Text) * 100

            tbpphFinal.Text = FormatNumber(tbpphFinal.Text, ViewState("DigitHome"))
            tbPphFinalValue.Text = FormatNumber(tbPphFinalValue.Text, ViewState("DigitHome"))
        Catch ex As Exception

        End Try
    End Sub


End Class
