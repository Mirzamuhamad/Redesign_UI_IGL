Imports System.Data
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO
Imports System
Imports System.Drawing

Public Class lbStatus
    Public Sub New()

    End Sub
End Class
Public Class ddlDivision
    Public Sub New()

    End Sub
End Class

Partial Class TrLpInvoice
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter

    Private Function GetStringHd() As String
        Return "Select * From V_FINLpInvHd " 'WHERE UserPrep = '" + ViewState("UserId") + "' "

    End Function


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                SetInit()
                FillCombo(ddlpph, "Select PPHCode, PPhName From V_MsPPh ", True, "PPHCode", "PPhName", ViewState("DBConnection"))
                FillCombo(ddlJenisPayment, "SELECT Jenis, Jenis FROM V_Jenis ", True, "Jenis", "Jenis", ViewState("DBConnection"))
                FillCombo(ddlTerm, "EXEC S_GetTerm", False, "Term_Code", "Term_Name", ViewState("DBConnection"))
                FillCombo(ddlCurr, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
                ' FillCombo(ddlDivision, "EXEC S_GetDivisionForSAUserDivision " + QuotedStr(ViewState("UserId")) + ", 'SPTBS'", True, "DivisionCode", "DivisionName", ViewState("DBConnection"))

                Session("AdvanceFilter") = ""
                'aturDGV()
            End If
            lbStatus.Text = ""
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If
            ' hasil dari search dialog
            If Not Session("Result") Is Nothing Then

                If ViewState("Sender") = "btnLp" Then
                    BindToText(TbLp, Session("Result")(0).ToString)
                    BindToDropList(ddlJenisPayment, Session("Result")(1).ToString)
                    BindToText(tbNama, Session("Result")(2).ToString)
                    BindToText(tbdpp, Session("Result")(3).ToString)
                    BindToText(tbDPPCek, Session("Result")(3).ToString)
                    BindToText(tbNameCode, Session("Result")(1).ToString)
                    BindToText(tbRevisi, Session("Result")(5).ToString)
                    'BindToDropList(ddlTypeInv, Session("Result")(4).ToString)
                    tbdpp.Text = FormatFloat(CFloat(tbdpp.Text), ViewState("DigitCurr"))
                    tbppnValue.Text = FormatFloat(CFloat(tbdpp.Text) * CFloat(tbppn.Text) / 100, ViewState("DigitCurr"))
                    tbTotalAmount.Text = FormatFloat(CFloat(tbdpp.Text) + CFloat(tbppnValue.Text), ViewState("DigitCurr"))
                End If

                Session("Result") = Nothing
                ViewState("Sender") = Nothing
            End If


            If Not ViewState("deletetrans") Is Nothing Then
                Dim Result, ListSelectNmbr, msg, ActionValue, status As String
                Dim Nmbr(100) As String
                Dim j As Integer
                If HiddenRemarkDelete.Value = "true" Then
                    If sender.ID.ToString = "BtnGo" Then
                        ActionValue = ddlCommand.SelectedValue
                    Else
                        ActionValue = ddlCommand2.SelectedValue
                    End If

                    status = CekStatus(ActionValue)
                    ListSelectNmbr = ""
                    msg = ""

                    '3 = status, 2 & 3 = key, 
                    GetListCommand("G|H", GridView1, "3, 2", ListSelectNmbr, Nmbr, msg)
                    If ListSelectNmbr = "" Then Exit Sub

                    For j = 0 To (Nmbr.Length - 1)
                        If Nmbr(j) = "" Then
                            Exit For
                        Else
                            Result = ExecSPCommandGo("Delete", "S_PLFinLandPurchaseINV", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)

                            If Trim(Result) <> "" Then
                                lbStatus.Text = lbStatus.Text + Result + "<br />"
                            End If
                        End If
                    Next

                    BindData("TransNmbr in (" + ListSelectNmbr + ")")
                    If msg.Trim <> "" Then
                        lbStatus.Text = MessageDlg(msg)
                    End If

                End If
                ViewState("deletetrans") = Nothing
                HiddenRemarkDelete.Value = ""
                'GridDt.Columns(0).Visible = False
            End If

            FubInv.Attributes("onchange") = "UploadInvoice(this)"

            FubFaktur.Attributes("onchange") = "UploadFaktur(this)"

        Catch ex As Exception
            lbStatus.Text = "Page Load Error : " + ex.ToString
        End Try
    End Sub

    Sub aturDGV()
        GridView1.RowStyle.BackColor = Color.White
        GridView1.AlternatingRowStyle.BackColor = Color.Azure

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

    Protected Sub btnsaveINV_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsaveINV.Click
        Try

            Dim dr As DataTable
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

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
            path2 = Server.MapPath("~/DokumenLPINV/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubInv.FileName
            namafile2 = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubInv.FileName

            SQLString1 = "UPDATE FINLpInvHd SET FileDok1 = " + QuotedStr(namafile2) + _
            " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            FubInv.SaveAs(path2)
            SQLExecuteNonQuery(SQLString1, ViewState("DBConnection").ToString)

            dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            lbDokInv.Text = dt.Rows(0)("FileDok1").ToString
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

            If dr.Rows(0)("FileDok1").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            filePath = dr.Rows(0)("FileDok1").ToString
            URL = ResolveUrl("~/DokumenLPINV/" + filePath)
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
            filePath = dr.Rows(0)("FileDok1").ToString


            If File.Exists(Server.MapPath("~/DokumenLPINV/" + filePath)) = True Then
                File.Delete(Server.MapPath("~/DokumenLPINV/" + filePath))
                SQLExecuteNonQuery("UPDATE FINLpInvHd Set FileDok1 = '' WHERE TransNmbr = '" + tbCode.Text + "' ", ViewState("DBConnection").ToString)

                lbDokInv.Text = "Not yet uploaded"
                FubInv.Visible = True
                btnClearInv.Visible = False
            End If



        Catch ex As Exception
            lbStatus.Text = "lbBAP_Click Error : " + ex.ToString
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

            Path2KK = Server.MapPath("~/DokumenLPINV/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubFaktur.FileName
            NameFile2KK = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubFaktur.FileName
            SQLString2 = "UPDATE FINLpInvHd SET FileDok2 = " + QuotedStr(NameFile2KK) + _
            " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            FubFaktur.SaveAs(Path2KK)
            SQLExecuteNonQuery(SQLString2, ViewState("DBConnection").ToString)


            dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            lbFaktur.Text = dt.Rows(0)("FileDok2").ToString
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

            If dr.Rows(0)("FileDok2").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            filePath = dr.Rows(0)("FileDok2").ToString
            URL = ResolveUrl("~/DokumenLPINV/" + filePath)
            Dim s As String = "window.open('" & URL & "', '_blank');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)

        Catch ex As Exception
            lbStatus.Text = "lbModerator_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnClearFaktur_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClearFaktur.Click
        Try
            Dim dr As DataTable
            Dim filePath As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            filePath = dr.Rows(0)("FileDok2").ToString
            If File.Exists(Server.MapPath("~/DokumenLPINV/" + filePath)) = True Then
                File.Delete(Server.MapPath("~/DokumenLPINV/" + filePath))
                SQLExecuteNonQuery("UPDATE FINLpInvHd Set FileDok2 = '' WHERE TransNmbr = '" + tbCode.Text + "' ", ViewState("DBConnection").ToString)

                lbFaktur.Text = "Not yet uploaded"
                FubFaktur.Visible = True
                btnClearFaktur.Visible = False
            End If

        Catch ex As Exception
            lbStatus.Text = "lbBAP_Click Error : " + ex.ToString
        End Try
    End Sub

    Private Sub SetInit()
        FillRange(ddlRange)
        ViewState("DtRemark") = ""
        ViewState("DigitCurr") = 2
        ViewState("SortExpression") = Nothing
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))

        If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
            ddlCommand.Items.Add("Print")
            ddlCommand2.Items.Add("Print")
        End If


        tbppn.Attributes.Add("OnBlur", "setformathd();")
        tbppnValue.Attributes.Add("OnBlur", "setformathd();")
        tbdpp.Attributes.Add("OnBlur", "setformathd();")
        tbTotalAmount.Attributes.Add("OnBlur", "setformathd();")
        tbpph.Attributes.Add("OnBlur", "setformathd();")
        tbPphValue.Attributes.Add("OnBlur", "setformathd();")
        tbType.Attributes.Add("OnBlur", "setformathd();")

        'Proteksi agar hanya angka saja yang bisa di input
        tbdpp.Attributes.Add("OnKeyDown", "return PressNumeric();")

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
            'DT = BindDataTransaction(GetStringHd(), StrFilter, ViewState("DBConnection").ToString)
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
                ViewState("SortExpression") = "Reference DESC"
            End If
            DV.Sort = ViewState("SortExpression")
            GridView1.DataSource = DV
            GridView1.DataBind()
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT * From V_PLRRPksDt WHERE TransNmbr = " + QuotedStr(Nmbr)
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

                        'If GVR.Cells(3).Text <> "P" Then
                        '    lbStatus.Text = MessageDlg("All Data Must Be Posting First to Print or Select Data with status posting!")
                        '    Exit Sub
                        'End If
                    End If
                Next
                Result = Result + "'"
                Session("SelectCommand") = "EXEC S_PLFormLPInvoice" + Result
                'lbStatus.Text = Session("SelectCommand")
                'Exit Sub
                Session("ReportFile") = ".../../../Rpt/FormLPInvoice.frx"
                Session("DBConnection") = ViewState("DBConnection")
                AttachScript("openprintdlg();", Page, Me.GetType)

            ElseIf ActionValue = "Delete" Then
                If HiddenRemarkDelete.Value <> "False Value" Then
                    HiddenRemarkDelete.Value = ""
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
                    ViewState("deletetrans") = ListSelectNmbr
                    AttachScript("deletetrans();", Page, Me.GetType)

                End If

            Else
                Status = CekStatus(ActionValue)

                ListSelectNmbr = ""
                GetListCommand(Status, GridView1, "3,2", ListSelectNmbr, Nmbr, lbStatus.Text)
                If ListSelectNmbr = "" Then Exit Sub
                For j = 0 To (Nmbr.Length - 1)
                    If Nmbr(j) = "" Then
                        Exit For
                    Else
                        Result = ExecSPCommandGo(ActionValue, "S_PLFinLandPurchaseINV", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                        If Trim(Result) <> "" Then
                            lbStatus.Text = lbStatus.Text + Result + "<br/>"
                        End If
                    End If
                Next
                BindData("Reference in (" + ListSelectNmbr + ")")
            End If
        Catch ex As Exception
            lbStatus.Text = "Go Command Error : " + ex.ToString
        End Try
    End Sub


    'Private Sub BindDataDt(ByVal Referens As String)
    '    Try
    '        Dim dt As New DataTable
    '        ViewState("Dt") = Nothing
    '        dt = SQLExecuteQuery(GetStringDt(Referens), ViewState("DBConnection").ToString).Tables(0)
    '        ViewState("Dt") = dt
    '        BindGridDt(dt, GridDt)
    '    Catch ex As Exception
    '        Throw New Exception("Bind Data Dt Error : " + ex.ToString)
    '    End Try
    'End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnHome.Click
        Try
            MovePanel(pnlInput, PnlHd)
        Catch ex As Exception
            lbStatus.Text = "btn back Error : " + ex.ToString
        End Try
    End Sub

    Private Sub SaveAll()
        Dim SQLString As String
        Dim I As Integer
        Try
            'If pnlDt.Visible = False Then
            '    lbStatus.Text = "Detail Data must be saved first"
            '    Exit Sub
            'End If
            If Not (ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit") Then
                Exit Sub
            Else
                ViewState("StateSave") = ViewState("StateHd")
                ViewState("StateHd") = "View"
            End If

            ddlpph_SelectedIndexChanged(Nothing, Nothing)

            'Save Hd
            If ViewState("StateSave") = "Insert" Then
                tbCode.Text = GetAutoNmbr("LPI", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
                'lbStatus.Text = tbCode.Text
                'Exit Sub
                SQLString = "INSERT INTO FINLpInvHd (TransNmbr, Status,TransDate, Attn, NameCode, LPNo, Jenis, Invoice_No, InvoiceDate, NoFaktur, DPP, PPN, " + _
                "PPNAmount, PPH, PPhAmount, TotalAmount, PPHCode,INvType, PpnDate, Currency, ForexRate, Term, DueDate, NoDok1, NoDok2, Revisi, Remark,  UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', " + QuotedStr(tbNama.Text) + " , " + QuotedStr(tbNameCode.Text) + ", " + _
                QuotedStr(TbLp.Text) + "," + QuotedStr(ddlJenisPayment.SelectedValue) + ", " + QuotedStr(tbInvoice.Text) + ", '" + _
                Format(tbDateInvoice.SelectedValue, "yyyy-MM-dd") + "'," + QuotedStr(tbFakturNo.Text) + "," + QuotedStr(tbdpp.Text.Replace(",", "")) + "," + _
                QuotedStr(tbppn.Text.Replace(",", "")) + ", " + QuotedStr(tbppnValue.Text.Replace(",", "")) + "," + _
                QuotedStr(tbpph.Text.Replace(",", "")) + "," + QuotedStr(tbPphValue.Text.Replace(",", "")) + ", " + QuotedStr(tbTotalAmount.Text.Replace(",", "")) + ", " + _
                QuotedStr(ddlpph.SelectedValue) + ",  " + QuotedStr(ddlTypeInv.SelectedValue) + " , '" + _
                Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', " + QuotedStr(ddlCurr.SelectedValue) + " , " + _
                QuotedStr(tbRate.Text) + ", " + QuotedStr(ddlTerm.SelectedValue) + " , '" + _
                Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', " + QuotedStr(tbNoDok1.Text) + " , " + _
                QuotedStr(tbNoDok1.Text) + ", " + QuotedStr(tbRevisi.Text) + ", " + QuotedStr(tbRemark.Text) + " , " + _
                QuotedStr(ViewState("UserId").ToString) + ", getDate()"
            Else
                'lbStatus.Text = Format(tbDateInvoice.SelectedDate, "yyyy-MM-dd")
                'Exit Sub

                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM FINLpInvHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE FINLpInvHd SET Remark = " + QuotedStr(tbRemark.Text) + _
                ",TransDate = " + QuotedStr(Format(tbDate.SelectedDate, "yyyy-MM-dd")) + _
                ",PPNDate= " + QuotedStr(Format(tbPpndate.SelectedDate, "yyyy-MM-dd")) + _
                ",Currency= " + QuotedStr(ddlCurr.SelectedValue) + _
                ",DueDate= " + QuotedStr(Format(tbDueDate.SelectedDate, "yyyy-MM-dd")) + _
                ",Term= " + QuotedStr(ddlTerm.SelectedValue) + _
                ",ForexRate= " + QuotedStr(tbRate.Text) + _
                ",NoDok1= " + QuotedStr(tbNoDok1.Text) + _
                ",NoDok2= " + QuotedStr(tbNoDok2.Text) + _
                ",Revisi= " + QuotedStr(tbRevisi.Text) + _
                ", Attn = " + QuotedStr(tbNama.Text) + _
                ", NameCode = " + QuotedStr(tbNameCode.Text) + _
                ", INvType = " + QuotedStr(ddlTypeInv.SelectedValue) + _
                ", LPNo = " + QuotedStr(TbLp.Text) + _
                ", Jenis = " + QuotedStr(ddlJenisPayment.SelectedValue) + _
                ", Invoice_No = " + QuotedStr(tbInvoice.Text) + _
                ", PPHCode = " + QuotedStr(ddlpph.SelectedValue) + _
                ", InvoiceDate = " + QuotedStr(Format(tbDateInvoice.SelectedDate, "yyyy-MM-dd")) + _
                ", NoFaktur = " + QuotedStr(tbFakturNo.Text) + _
                ", DPP = " + QuotedStr(tbdpp.Text.Replace(",", "")) + _
                ", PPN = " + QuotedStr(tbppn.Text.Replace(",", "")) + _
                ", PPNAmount = " + QuotedStr(tbppnValue.Text.Replace(",", "")) + _
                ", Pph = " + QuotedStr(tbpph.Text.Replace(",", "")) + _
                ", PPhAmount = " + QuotedStr(tbPphValue.Text.Replace(",", "")) + _
                ", TotalAmount = " + QuotedStr(tbTotalAmount.Text.Replace(",", "")) + _
                " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            End If

            SQLString = SQLString.Replace("'1900-01-01'", "NULL")
            SQLString = SQLString.Replace("'0001-01-01'", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)



            ''update Primary Key on Dt
            'Dim Row As DataRow()

            'Row = ViewState("Dt").Select("TransNmbr IS NULL")
            'For I = 0 To Row.Length - 1
            '    Row(I).BeginEdit()
            '    Row(I)("TransNmbr") = tbCode.Text
            '    Row(I).EndEdit()
            'Next

            ''save dt
            'Dim ConnString As String = ViewState("DBConnection").ToString 'System.Configuration.ConfigurationManager.AppSettings.Get("ConString")
            'con = New SqlConnection(ConnString)
            'con.Open()
            'Dim cmdSql As New SqlCommand("SELECT TransNmbr, ItemNo, Timbang1, Timbang2, Potongan, Netto1, Netto2, Remark FROM PLRRPksDt WHERE TransNmbr = '" & ViewState("Reference") & "'", con)

            'da = New SqlDataAdapter(cmdSql)
            'Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            'da.InsertCommand = dbcommandBuilder.GetInsertCommand
            ''da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            ''da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            'Dim param As SqlParameter
            '' Create the UpdateCommand.
            'Dim Update_Command = New SqlCommand( _
            '        "UPDATE PLRRPksDt SET ItemNo = @Item, Timbang1 = @Timbang1, Timbang2 = @Timbang2, Potongan = @Potongan " + _
            '        ", Netto1 = @Netto1, Netto2 = @Netto2, Remark = @Remark WHERE TransNmbr = '" & ViewState("Reference") & "' AND ItemNo = @OldItem ", con)
            '' Define output parameters.
            'Update_Command.Parameters.Add("@Item", SqlDbType.Int, 1, "ItemNo")
            'Update_Command.Parameters.Add("@Timbang1", SqlDbType.Int, 4, "Timbang1")
            'Update_Command.Parameters.Add("@Timbang2", SqlDbType.Float, 22, "Timbang2")
            'Update_Command.Parameters.Add("@Potongan", SqlDbType.Float, 22, "Potongan")
            'Update_Command.Parameters.Add("@Netto1", SqlDbType.Float, 22, "Netto1")
            'Update_Command.Parameters.Add("@Netto2", SqlDbType.Float, 22, "Netto2")
            'Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 22, "Remark")

            '' Define intput (WHERE) parameters.
            'param = Update_Command.Parameters.Add("@OldItem", SqlDbType.Int, 1, "ItemNo")
            'param.SourceVersion = DataRowVersion.Original

            '' Attach the update command to the DataAdapter.
            'da.UpdateCommand = Update_Command

            '' Create the DeleteCommand.
            'Dim Delete_Command = New SqlCommand( _
            '    "DELETE FROM PLRRPksDt WHERE TransNmbr = '" & ViewState("Reference") & "' AND ItemNo = @oldItem ", con)
            '' Add the parameters for the DeleteCommand.
            'param = Delete_Command.Parameters.Add("@oldItem", SqlDbType.Int, 1, "ItemNo")
            'param.SourceVersion = DataRowVersion.Original
            'da.DeleteCommand = Delete_Command

            'Dim Dt As New DataTable("PLRRPksDt")

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

            Dim confirmValue As String = Request.Form("confirm_value")
            If confirmValue = "Yes" Then

                If CekHd() = False Then
                    Exit Sub
                End If


                SaveAll()
                ModifyInput(False, pnlInput)
                btnGoEdit.Visible = True
                Menu2.Items.Item(1).Enabled = True
                MultiView2.ActiveViewIndex = 1
                Menu2.Items.Item(1).Selected = True

            Else


                If CekHd() = False Then
                    Exit Sub
                End If

                SaveAll()
                MovePanel(pnlInput, PnlHd)
                CurrFilter = tbFilter.Text
                Value = ddlField.SelectedValue
                tbFilter.Text = tbCode.Text
                ddlField.SelectedValue = "Reference"
                btnSearch_Click(Nothing, Nothing)
                tbFilter.Text = CurrFilter
                ddlField.SelectedValue = Value
                tbFilter.Text = ""
                'ClientScript.RegisterStartupScript(Me.[GetType](), "alert", "alert('You clicked NO!')", True)
            End If


        Catch ex As Exception
            lbStatus.Text = "Save All Dt Error : " + ex.ToString
        End Try
    End Sub

    Private Sub EnableHd(ByVal State As Boolean)
        Try
            tbDate.Enabled = State
            tbNama.Enabled = False
            tbNameCode.Enabled = False
            TbLp.Enabled = State
            tbInvoice.Enabled = State
            tbDateInvoice.Enabled = State
            tbFakturNo.Enabled = State
            ddlJenisPayment.Enabled = State
            ddlpph.Enabled = State
            tbdpp.Enabled = State
            tbppn.Enabled = State
            tbppnValue.Enabled = State
            tbpph.Enabled = False
            tbRevisi.Enabled = False
            ddlTerm.Enabled = State
            btnLP.Visible = State
            ddlCurr.Enabled = State
            tbPphValue.Enabled = False
            ddlTypeInv.Enabled = State
            tbTotalAmount.Enabled = False
            tbRemark.Enabled = State
            If ddlJenisPayment.SelectedValue = "" Then
                btnLP.Enabled = False
                TbLp.Enabled = False
            Else
                btnLP.Enabled = True
                TbLp.Enabled = True
            End If
            tbRate.Enabled = False


        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ViewState("DtRemark") = ""
            'ddlTransit_SelectedIndexChanged(Nothing, Nothing)
            ClearHd()
            'Cleardt()
            'pnlDt.Visible = True
            'GridDt.Columns(1).Visible = False
            ViewState("DigitCurr") = 2
            'BindDataDt("")
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub


    Private Sub ClearHd()
        Dim SQLString As String
        Try
            tbDate.SelectedDate = ViewState("ServerDate")
            tbNama.Text = ""
            tbNameCode.Text = ""
            tbDPPCek.Text = 0
            TbLp.Text = ""
            tbInvoice.Text = ""
            tbDateInvoice.SelectedDate = ViewState("ServerDate")
            tbFakturNo.Text = ""
            ddlpph.SelectedValue = ""
            ddlJenisPayment.SelectedValue = ""
            tbdpp.Text = 0
            tbppn.Text = ViewState("PPN")
            tbppnValue.Text = 0
            tbpph.Text = 0
            tbPphValue.Text = 0
            tbTotalAmount.Text = 0
            tbRemark.Text = ""
            'ddlTypeInv.SelectedValue = ""
            tbNoDok1.Text = ""
            tbNoDok2.Text = ""
            tbRevisi.Text = ""
            tbDueDate.SelectedDate = ViewState("ServerDate")
            tbPpndate.SelectedDate = ViewState("ServerDate")
            ddlCurr.SelectedValue = ViewState("Currency")
            ddlCurr_SelectedIndexChanged(Nothing, Nothing)

        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub

    'Private Sub Cleardt()
    '    Try
    '        tbTimbang1.Text = "0"
    '        tbTimbang2.Text = "0"
    '        tbPotongan.Text = "0"
    '        tbNetto1.Text = "0"
    '        tbNetto2.Text = "0"
    '        tbRemarkDt.Text = ""
    '        lbItemNo.Text = ""

    '    Catch ex As Exception
    '        Throw New Exception("Clear Dt Error " + ex.ToString)
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


    Protected Sub btnSaveAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveAll.Click
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            'If IsNothing(ViewState("Dt")) Then
            '    lbStatus.Text = MessageDlg("Detail must have at least 1 record")
            '    Exit Sub
            'End If

            'For Each dr In ViewState("Dt").Rows
            '    If CekDt(dr, "ItemNo") = False Then

            '        Exit Sub
            '    End If
            'Next

            SaveAll()
            newTrans()
        Catch ex As Exception
            lbStatus.Text = "btn save all error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try

            newTrans()
            btnHome.Visible = False
            ModifyInput(True, pnlInput)
            MovePanel(PnlHd, pnlInput)
            MultiView2.ActiveViewIndex = 0
            Menu2.Items.Item(0).Selected = True
            Menu2.Items.Item(1).Enabled = False
            EnableHd(True)

        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnGoEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGoEdit.Click
        Dim CurrFilter, Value As String
        Try
            MovePanel(pnlInput, PnlHd)
            CurrFilter = tbFilter.Text
            Value = ddlField.SelectedValue
            tbFilter.Text = tbCode.Text
            ddlField.SelectedValue = "Reference"
            btnSearch_Click(Nothing, Nothing)
            tbFilter.Text = CurrFilter
            ddlField.SelectedValue = Value
            btnGoEdit.Visible = False
            tbFilter.Text = ""
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
                'GridDt.Columns(0).Visible = True
            End If
        End If
        If Menu2.Items.Item(1).Selected = True Then
            If ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit" Then
                'btnGoEdit.Visible = True
                'btnGetBAP.Visible = False
                'GridDt.Columns(0).Visible = False
            End If

        End If
    End Sub


    'Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt.Click, btnAddDt2.Click

    '    Dim TglHitam As String

    '    Cleardt()
    '    If CekHd() = False Then
    '        Exit Sub
    '    End If
    '    lbItemNo.Text = GetNewItemNo(ViewState("Dt"))
    '    ViewState("StateDt") = "Insert"
    '    MovePanel(pnlDt, pnlEditDt)
    '    EnableHd(False)
    '    StatusButtonSave(False)
    'End Sub

    Function CekHd() As Boolean
        Try
            If tbDate.IsNull Then
                lbStatus.Text = MessageDlg("Date must have value")
                tbDate.Focus()
                Return False
            End If

            If TbLp.Text = "" Then
                lbStatus.Text = MessageDlg("Land Purchase No must have value")
                TbLp.Focus()
                Return False
            End If

            If tbInvoice.Text = "" Then
                lbStatus.Text = MessageDlg("Invoice must have value")
                tbInvoice.Focus()
                Return False
            End If

            If tbFakturNo.Text = "" Then
                lbStatus.Text = MessageDlg("Faktur No must have value")
                tbFakturNo.Focus()
                Return False
            End If

            If tbFakturNo.Text = "" Then
                lbStatus.Text = MessageDlg("Faktur No must have value")
                tbFakturNo.Focus()
                Return False
            End If

            If ViewState("StateHd") = "Insert" Then


                If (Val(tbdpp.Text.Replace(",", "")) > Val(tbDPPCek.Text.Replace(",", ""))) Then
                    lbStatus.Text = MessageDlg("DPP can not grether more than Biaya Outstanding")
                    tbdpp.Focus()
                    Exit Function
                End If
            End If

            'If ViewState("StateHd") = "Edit" Then '

            '    If ddlJenisPayment.SelectedValue = "Tanah" Then
            '        Dim Tanah, DPP As String
            '        Tanah = "SELECT  BiayaModerator  FROM V_GLLandPurchaseHd WHERE TransNmbr = '" + TbLp.Text + "'"
            '        DPP = SQLExecuteScalar(Tanah, ViewState("DBConnection"))
            '        'lbStatus.Text = DPP
            '        'Exit Function

            '        tbDPPCek.Text = DPP

            '        If (Val(tbdpp.Text.Replace(",", "")) > Val(tbDPPCek.Text.Replace(",", ""))) Then
            '            lbStatus.Text = MessageDlg("DPP can not grether more than Biaya Outstanding")
            '            tbdpp.Focus()
            '            Exit Function
            '        End If
            '    End If

            '    If ddlJenisPayment.SelectedValue = "Notaris" Then
            '        Dim Notaris, DPP As String
            '        Notaris = "SELECT  JmlBiaya  FROM V_GLLandPurchaseHd WHERE TransNmbr = '" + TbLp.Text + "'"
            '        DPP = SQLExecuteScalar(Notaris, ViewState("DBConnection"))
            '        'lbStatus.Text = DPP
            '        'Exit Function

            '        tbDPPCek.Text = DPP

            '        If (Val(tbdpp.Text.Replace(",", "")) > Val(tbDPPCek.Text.Replace(",", ""))) Then
            '            lbStatus.Text = MessageDlg("DPP can not grether more than Biaya Outstanding")
            '            tbdpp.Focus()
            '            Exit Function
            '        End If
            '    End If

            '    If ddlJenisPayment.SelectedValue = "Moderator" Then
            '        Dim Moderator, DPP As String
            '        Moderator = "SELECT  BiayaModerator  FROM V_GLLandPurchaseHd WHERE TransNmbr = '" + TbLp.Text + "'"
            '        DPP = SQLExecuteScalar(Moderator, ViewState("DBConnection"))
            '        'lbStatus.Text = DPP
            '        'Exit Function

            '        tbDPPCek.Text = DPP

            '        If (Val(tbdpp.Text.Replace(",", "")) > Val(tbDPPCek.Text.Replace(",", ""))) Then
            '            lbStatus.Text = MessageDlg("DPP can not grether more than Biaya Outstanding")
            '            tbdpp.Focus()
            '            Exit Function
            '        End If

            '    End If

            '    If ddlJenisPayment.SelectedValue = "BPHTB" Then
            '        Dim BPHTB, DPP As String
            '        BPHTB = "SELECT  BiayaBPHTB  FROM V_GLLandPurchaseHd WHERE TransNmbr = '" + TbLp.Text + "'"
            '        DPP = SQLExecuteScalar(BPHTB, ViewState("DBConnection"))
            '        'lbStatus.Text = DPP
            '        'Exit Function

            '        tbDPPCek.Text = DPP

            '        If (Val(tbdpp.Text.Replace(",", "")) > Val(tbDPPCek.Text.Replace(",", ""))) Then
            '            lbStatus.Text = MessageDlg("DPP can not grether more than Biaya Outstanding")
            '            tbdpp.Focus()
            '            Exit Function
            '        End If

            '    End If

            '    If ddlJenisPayment.SelectedValue = "Lain-Lain" Then
            '        Dim Lain2, DPPLainLain As String
            '        Lain2 = "SELECT  BiayaLainLain  FROM V_GLLandPurchaseHd WHERE TransNmbr = '" + TbLp.Text + "'"
            '        DPPLainLain = SQLExecuteScalar(Lain2, ViewState("DBConnection"))
            '        'lbStatus.Text = DPP
            '        'Exit Function

            '        tbDPPCek.Text = DPPLainLain

            '        If (Val(tbdpp.Text.Replace(",", "")) > Val(tbDPPCek.Text.Replace(",", ""))) Then
            '            lbStatus.Text = MessageDlg("DPP can not grether more than Biaya Outstanding")
            '            tbdpp.Focus()
            '            Exit Function
            '        End If

            '    End If


            'End If


            'If Len(tbRemark.Text.Trim) > 60 Then
            '    lbStatus.Text = MessageDlg("Remark must have value or caracter must 60")
            '    tbRemark.Focus()
            '    Return False
            'End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Hd Error : " + ex.ToString)
        End Try
    End Function



    'Function CekDt(Optional ByVal Dr As DataRow = Nothing, Optional ByVal FieldKey As String = "") As Boolean
    '    Try
    '        If Not (Dr Is Nothing) Then
    '            If Dr.RowState = DataRowState.Deleted Then
    '                Return True
    '            End If
    '            If Dr("Timbang1").ToString.Trim = "" Then
    '                lbStatus.Text = MessageDlg("Timbang1 Must Have Value")
    '                Return False
    '            End If
    '            If CFloat(Dr("Timbang2").ToString) <= 0 Then
    '                lbStatus.Text = MessageDlg("Timbang2 Must Have Value")
    '                Return False
    '            End If
    '            If Dr("potongan").ToString.Trim = "" Then
    '                lbStatus.Text = MessageDlg("Potongan Must Have Value")
    '                Return False
    '            End If
    '            If CFloat(Dr("netto1").ToString) <= 0 Then
    '                lbStatus.Text = MessageDlg("netto1 Panen Must Have Value")
    '                Return False
    '            End If
    '            If Dr("netto2").ToString.Trim = "" Then
    '                lbStatus.Text = MessageDlg("netto2 Must Have Value")
    '                Return False
    '            End If

    '        Else


    '            'If tbRemarkDt.Text.Trim = "" Then
    '            '    lbStatus.Text = MessageDlg("Remark Must Have Value")
    '            '    tbRemarkDt.Focus()
    '            '    Return False
    '            'End If
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
            FDateName = "Date"
            FDateValue = "TransDate"
            FilterName = "Reference, Date, So_No, Do_No,Remark"
            FilterValue = "Reference, dbo.FormatDate(TransDate), Date, So_No, Do_No,Remark"
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
                    ViewState("Reference") = GVR.Cells(2).Text
                    ViewState("Status") = GVR.Cells(3).Text

                    'GridDt.PageIndex = 0
                    FillTextBoxHd(ViewState("Reference"))
                    'BindDataDt(ViewState("Reference"))
                    ViewState("StateHd") = "View"
                    'ModifyInput2(False, pnlInput, , )
                    btnSaveAll.Visible = False
                    btnBack.Visible = False
                    btnSaveTrans.Visible = False
                    btnHome.Visible = True
                    EnableHd(False)
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
                        ViewState("Reference") = GVR.Cells(2).Text
                        'GridDt.Columns(1).Visible = False
                        'GridDt.PageIndex = 0
                        'BindDataDt(ViewState("Reference"))
                        FillTextBoxHd(ViewState("Reference"))
                        ViewState("StateHd") = "Edit"
                        'ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        btnHome.Visible = False
                        btnSaveAll.Visible = True
                        btnBack.Visible = True
                        btnSaveTrans.Visible = True
                        'EnableHd(GetCountRecord(ViewState("Dt")) = 0)
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

                        'If GVR.Cells(3).Text <> "P" Then
                        '    lbStatus.Text = MessageDlg("Data Must Be Posting First to Print!")
                        '    Exit Sub
                        'End If

                        Session("SelectCommand") = "EXEC S_PLFormLPInvoice ''" + QuotedStr(GVR.Cells(2).Text) + "''"
                        Session("ReportFile") = ".../../../Rpt/FormLPInvoice.frx"
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


    Protected Sub Page_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        'Session("SelectCommand") = Nothing
        'Session("ReportFile") = Nothing
        'WebReport1.Dispose()
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


    'Protected Sub GridDt_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridDt.PageIndexChanging
    '    Try
    '        GridDt.PageIndex = e.NewPageIndex
    '        GridDt.DataSource = ViewState("Dt")
    '        GridDt.DataBind()
    '    Catch ex As Exception
    '        lbStatus.Text = "Grid dT Page Index Change Error : " + ex.ToString + " page index : " + e.NewPageIndex.ToString
    '    End Try
    'End Sub

    'Protected Sub GridDt_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridDt.RowCommand
    '    Try
    '        If e.CommandName = "Insert" Then
    '            btnAddDt_Click(Nothing, Nothing)
    '        ElseIf e.CommandName = "Closing" Then
    '            Dim GVR As GridViewRow
    '            GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))
    '            If ViewState("Status") <> "P" Then
    '                lbStatus.Text = MessageDlg("Status Transaction is not Post, cannot close TK Panen")
    '                Exit Sub
    '            End If
    '            ViewState("ProductClose") = GVR.Cells(2).Text
    '            AttachScript("closing();", Page, Me.GetType)
    '        End If
    '    Catch ex As Exception
    '        lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
    '    End Try
    'End Sub

    'Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
    '    Dim dr() As DataRow
    '    Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
    '    dr = ViewState("Dt").Select("ItemNo = " + QuotedStr(GVR.Cells(1).Text))
    '    dr(0).Delete()
    '    BindGridDt(ViewState("Dt"), GridDt)
    '    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
    'End Sub

    'Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
    '    Dim GVR As GridViewRow
    '    Try
    '        GVR = GridDt.Rows(e.NewEditIndex)
    '        ViewState("DtValue") = GVR.Cells(1).Text
    '        'lbStatus.Text = ViewState("DtValue")
    '        'Exit Sub
    '        FillTextBoxDt(ViewState("DtValue"))

    '        'FillTextBoxDt(GVR.Cells(4).Text + "|" + GVR.Cells(7).Text + "|" + GVR.Cells(8).Text)
    '        'ViewState("DtValue") = GVR.Cells(4).Text + "|" + GVR.Cells(7).Text + "|" + GVR.Cells(8).Text
    '        MovePanel(pnlDt, pnlEditDt)
    '        EnableHd(False)
    '        ViewState("StateDt") = "Edit"
    '        StatusButtonSave(False)
    '    Catch ex As Exception
    '        lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "Reference = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            'newTrans()
            tbCode.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToText(TbLp, Dt.Rows(0)("LandPurchaseNo").ToString)
            BindToText(tbNama, Dt.Rows(0)("Attn").ToString)
            BindToText(tbNameCode, Dt.Rows(0)("NameCode").ToString)
            BindToText(tbInvoice, Dt.Rows(0)("invoice_No").ToString)
            BindToDate(tbDateInvoice, Dt.Rows(0)("InvoiceDate").ToString)
            BindToText(tbFakturNo, Dt.Rows(0)("NoFaktur").ToString) '
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
            BindToDropList(ddlJenisPayment, Dt.Rows(0)("Jenis").ToString)

            BindToDropList(ddlpph, Dt.Rows(0)("PPhCode").ToString)
            BindToText(tbType, Dt.Rows(0)("Type").ToString)
            BindToDropList(ddlTypeInv, Dt.Rows(0)("INvtype").ToString)

            BindToText(tbdpp, Dt.Rows(0)("Dpp").ToString, ViewState("DigitCurr"))
            BindToText(tbppn, Dt.Rows(0)("Ppn").ToString, ViewState("DigitCurr"))
            BindToText(tbppnValue, Dt.Rows(0)("PpnAmount").ToString, ViewState("DigitCurr"))
            BindToText(tbpph, Dt.Rows(0)("Pph").ToString, ViewState("DigitCurr"))
            BindToText(tbPphValue, Dt.Rows(0)("PphAmount").ToString, ViewState("DigitCurr"))
            BindToText(tbTotalAmount, Dt.Rows(0)("TotalAmount").ToString, ViewState("DigitCurr"))

            BindToDate(tbPpndate, Dt.Rows(0)("PpnDate").ToString)
            BindToDate(tbDueDate, Dt.Rows(0)("DueDate").ToString)
            BindToText(tbRate, Dt.Rows(0)("ForexRate").ToString)
            BindToText(tbNoDok1, Dt.Rows(0)("NoDok1").ToString)
            BindToText(tbNoDok2, Dt.Rows(0)("NoDok2").ToString)
            BindToText(tbRevisi, Dt.Rows(0)("Revisi").ToString)
            BindToDropList(ddlCurr, Dt.Rows(0)("Currency").ToString)
            BindToDropList(ddlTerm, Dt.Rows(0)("Term").ToString)

            If Dt.Rows(0)("FileDok1").ToString = "" Then
                lbDokInv.Text = "Not Yet Uploaded"
                FubInv.Visible = True
                btnClearInv.Visible = False
            Else
                lbDokInv.Text = Dt.Rows(0)("FileDok1").ToString
                FubInv.Visible = False
                btnClearInv.Visible = True
            End If


            If Dt.Rows(0)("FileDok2").ToString = "" Then
                lbFaktur.Text = "Not Yet Uploaded"
                FubFaktur.Visible = True
                btnClearFaktur.Visible = False
            Else
                lbFaktur.Text = Dt.Rows(0)("FileDok2").ToString
                FubFaktur.Visible = False
                btnClearFaktur.Visible = True
            End If

        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    'Protected Sub FillTextBoxDt(ByVal ItemNo As String)
    '    Dim Dr As DataRow()
    '    Try
    '        Dr = ViewState("Dt").select("ItemNo = " + QuotedStr(ItemNo))
    '        If Dr.Length > 0 Then
    '            lbItemNo.Text = ItemNo.ToString
    '            BindToText(tbTimbang1, FormatNumber(Dr(0)("timbang1").ToString), 2)
    '            BindToText(tbTimbang2, FormatNumber(Dr(0)("timbang2").ToString), 2)
    '            BindToText(tbPotongan, FormatNumber(Dr(0)("potongan").ToString), 2)
    '            BindToText(tbNetto1, FormatNumber(TrimStr(Dr(0)("Netto1").ToString)), 2)
    '            BindToText(tbNetto2, FormatNumber(Dr(0)("Netto2").ToString), 2)
    '            BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
    '        End If
    '        'Dt = BindDataTransaction(GetStringDt(tbCode.Text), "ItemNo = " + ItemNo, ViewState("DBConnection").ToString)
    '        'newTrans()
    '    Catch ex As Exception
    '        Throw New Exception("fill text box detail error : " + ex.ToString)
    '    End Try
    'End Sub

    Private Sub StatusButtonSave(ByVal Bool As Boolean)
        btnSaveAll.Visible = Bool
        btnSaveTrans.Visible = Bool
        btnBack.Visible = Bool
    End Sub


    'Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveDt.Click
    '    Dim Row As DataRow
    '    Try
    '        If CekDt() = False Then
    '            Exit Sub
    '        End If

    '        If ViewState("StateDt") = "Edit" Then
    '            If ViewState("DtValue") <> lbItemNo.Text Then
    '                If CekExistData(ViewState("Dt"), "ItemNo", lbItemNo.Text) Then
    '                    lbStatus.Text = "item detail'" + lbItemNo.Text + "' has already exists"
    '                    Exit Sub
    '                End If
    '            End If
    '            Row = ViewState("Dt").Select("ItemNo = " + QuotedStr(ViewState("DtValue")))(0)
    '            'If CekDt() = False Then
    '            '    Exit Sub
    '            'End If
    '            Row = ViewState("Dt").Select("ItemNo = " + lbItemNo.Text)(0)
    '            Row.BeginEdit()
    '            Row("timbang1") = tbTimbang1.Text
    '            Row("timbang2") = tbTimbang2.Text
    '            Row("Potongan") = tbPotongan.Text
    '            Row("Netto1") = tbNetto1.Text
    '            Row("Netto2") = tbNetto2.Text
    '            Row("Remark") = tbRemarkDt.Text
    '            Row.EndEdit()
    '        Else
    '            'Insert
    '            If CekDt() = False Then
    '                Exit Sub
    '            End If
    '            If CekExistData(ViewState("Dt"), "ItemNo", lbItemNo.Text) Then
    '                lbStatus.Text = "item detail '" + lbItemNo.Text + "' has already exists"
    '                Exit Sub
    '            End If
    '            Dim dr As DataRow
    '            dr = ViewState("Dt").NewRow
    '            dr("ItemNo") = lbItemNo.Text
    '            dr("timbang1") = tbTimbang1.Text
    '            dr("timbang2") = tbTimbang2.Text
    '            dr("Potongan") = tbPotongan.Text
    '            dr("Netto1") = tbNetto1.Text
    '            dr("Netto2") = tbNetto2.Text
    '            dr("Remark") = tbRemarkDt.Text
    '            ViewState("Dt").Rows.Add(dr)
    '        End If
    '        ViewState("DtRemark") = tbRemarkDt.Text
    '        MovePanel(pnlEditDt, pnlDt)
    '        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
    '        BindGridDt(ViewState("Dt"), GridDt)
    '        StatusButtonSave(True)

    '    Catch ex As Exception
    '        lbStatus.Text = "btn save Dt Error : " + ex.ToString
    '    Finally
    '        If Not con Is Nothing Then con.Dispose()
    '        If Not da Is Nothing Then da.Dispose()
    '    End Try

    'End Sub

    'Private Function DtExist() As Boolean
    '    Dim dete, piar As Boolean
    '    Try
    '        If ViewState("Dt") Is Nothing Then
    '            dete = False
    '        Else
    '            dete = GetCountRecord(ViewState("Dt")) > 0
    '        End If

    '        Return (dete Or piar)

    '        'EnableHd(GetCountRecord(ViewState("Dt")) = 0 And ViewState("DtPR").Rows.Count = 0 And ViewState("DtPart").Rows.Count = 0)
    '    Catch ex As Exception
    '        Throw New Exception("Cek Data Hd Error : " + ex.ToString)
    '    End Try
    'End Function


    'Protected Sub btnCancelDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBack.Click, btnCancelDt.Click
    '    Try
    '        MovePanel(pnlEditDt, pnlDt)
    '        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
    '        'EnableHd(GridDt.Rows.Count = 0)
    '        Cleardt()
    '        StatusButtonSave(True)
    '        EnableHd(Not DtExist())
    '    Catch ex As Exception
    '        lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
    '    End Try
    'End Sub

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
                tbTotalAmount.Text = FormatFloat(CFloat(tbdpp.Text) + CFloat(tbppnValue.Text), ViewState("DigitCurr"))
            Else
                tbpph.Enabled = True
                tbPphValue.Enabled = True
                If Type = "-" Then
                    tbTotalAmount.Text = FormatFloat(CFloat(tbdpp.Text) + CFloat(tbppnValue.Text) - CFloat(tbPphValue.Text), ViewState("DigitCurr"))
                Else
                    tbTotalAmount.Text = FormatFloat(CFloat(tbdpp.Text) + CFloat(tbppnValue.Text) + CFloat(tbPphValue.Text), ViewState("DigitCurr"))
                End If

            End If

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub ddlJenisPayment_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlJenisPayment.SelectedIndexChanged
        Try

            If ddlJenisPayment.SelectedValue = "" Then
                btnLP.Enabled = False
                TbLp.Enabled = False
            Else
                btnLP.Enabled = True
                TbLp.Enabled = True
            End If

        Catch ex As Exception

        End Try
    End Sub

     


    Protected Sub btnLp_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLP.Click
        Dim ResultField, CriteriaField, sqlstring As String
        Try
            sqlstring = "EXEC S_GetLpINV '" + ddlJenisPayment.SelectedValue + "', '" + ddlTypeInv.SelectedValue + "'"
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = sqlstring
            ResultField = "TransNmbr, NameCode, Name, Biaya, Type, Revisi "
            CriteriaField = "TransNmbr, NameCode, Name, Biaya, Type, Revisi "
            Session("CriteriaField") = CriteriaField.Split(",")
            ViewState("Sender") = "btnLp"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchGrid();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Cust Error : " + ex.ToString
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

    Protected Sub lbCurr_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCurr.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsCurrency')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lbModerator_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbTerm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbTerm.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsTerm')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lbModerator_Click Error : " + ex.ToString
        End Try
    End Sub

 

End Class
