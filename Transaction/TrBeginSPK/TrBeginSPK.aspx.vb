Imports System.Data
Imports System.Data.SqlClient
'Imports System.Data.Odbc
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc

Partial Class BeginSPK
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select * From V_PRCSPKBeginHD "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                FillCombo(ddlLokasi, "SELECT AreaCode, AreaName FROM V_MsArea ", True, "AreaCode", "AreaName", ViewState("DBConnection"))
                FillCombo(ddlSatuan, "SELECT Unit_Code, Unit_Name FROM VMsUnit ", True, "Unit_Code", "Unit_Name", ViewState("DBConnection"))
                SetInit()
                Session("AdvanceFilter") = ""
                lbCount.Text = SQLExecuteScalar("EXEC S_GetPenerimaan 0 ", ViewState("DBConnection").ToString)
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


                If ViewState("Sender") = "btnTugas" Then
                    BindToText(tbTugasCode, Session("Result")(0).ToString)
                    BindToText(tbTugasName, Session("Result")(1).ToString)
                End If

                If ViewState("Sender") = "btnSPK" Then
                    BindToText(tbSPK, Session("Result")(0).ToString)
                    BindToText(tbSPKName, Session("Result")(1).ToString)
                End If


                If ViewState("Sender") = "btnPenawaran" Then

                    Dim drResult As DataRow
                    Dim MaxItem As String
                    Dim DtPekerjaan As DataTable
                    Dim drUnit, DrCek, DrData As DataRow
                    Dim SQLString, CekString, DataString As String

                    tbPenawaranNo.Text = Session("Result")(0).ToString
                    tbIntruksiNo.Text = Session("Result")(1).ToString
                    tbSuppCode.Text = Session("Result")(2).ToString
                    tbSuppName.Text = Session("Result")(3).ToString
                    tbPekerjaan.Text = Session("Result")(4).ToString.Replace("amp;", "")
                    ddlLokasi.Text = Session("Result")(5).ToString
                    tbStartDate.SelectedDate = Session("Result")(6).ToString
                    tbEndDate.SelectedDate = Session("Result")(7).ToString
                    tbDurasi.Text = Session("Result")(8).ToString

                    'Insert To Detail
                    SQLString = "SELECT * FROM V_GetDetailPekerjaan WHERE TransNmbr = " + QuotedStr(tbPenawaranNo.Text)
                    DtPekerjaan = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)


                    For Each drResult In DtPekerjaan.Rows
                        If CekExistData(ViewState("Dt"), "ItemNo", drResult("ItemNo")) = False Then
                            MaxItem = GetNewItemNo(ViewState("Dt"))
                            'insert
                            Dim dr As DataRow
                            dr = ViewState("Dt").NewRow
                            dr("ItemNo") = drResult("ItemNo")
                            dr("UraianPekerjaan") = drResult("JobName")
                            dr("UnitCode") = drResult("UnitCode")
                            dr("Luas") = drResult("Luas")
                            dr("BiayaSatuan") = drResult("BiayaSatuan")
                            dr("Biaya") = drResult("BiayaEstimasi")
                            dr("Remark") = drResult("Remark")
                            ViewState("Dt").Rows.Add(dr)
                        End If
                    Next

                    BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    CountTotalDt()

                End If


                If ViewState("Sender") = "btnOut" Then
                    BtnAdd_Click(Nothing, Nothing)
                    Dim drResult As DataRow
                    Dim MaxItem As String
                    Dim DtPekerjaan As DataTable
                    Dim drUnit, DrCek, DrData As DataRow
                    Dim SQLString, CekString, DataString As String

                    tbPenawaranNo.Text = Session("Result")(0).ToString
                    tbIntruksiNo.Text = Session("Result")(1).ToString
                    tbSuppCode.Text = Session("Result")(2).ToString
                    tbSuppName.Text = Session("Result")(3).ToString
                    tbPekerjaan.Text = Session("Result")(4).ToString.Replace("amp;", "")
                    ddlLokasi.Text = Session("Result")(5).ToString
                    tbStartDate.SelectedDate = Session("Result")(6).ToString
                    tbEndDate.SelectedDate = Session("Result")(7).ToString
                    tbDurasi.Text = Session("Result")(8).ToString

                    'Insert To Detail
                    SQLString = "SELECT * FROM V_GetDetailPekerjaan WHERE TransNmbr = " + QuotedStr(tbPenawaranNo.Text)
                    DtPekerjaan = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)

                    For Each drResult In DtPekerjaan.Rows
                        If CekExistData(ViewState("Dt"), "ItemNo", drResult("ItemNo")) = False Then
                            MaxItem = GetNewItemNo(ViewState("Dt"))
                            'insert
                            Dim dr As DataRow
                            dr = ViewState("Dt").NewRow
                            dr("ItemNo") = drResult("ItemNo")
                            dr("UraianPekerjaan") = drResult("JobName")
                            dr("UnitCode") = drResult("UnitCode")
                            dr("Luas") = drResult("Luas")
                            dr("BiayaSatuan") = drResult("BiayaSatuan")
                            dr("Biaya") = drResult("BiayaEstimasi")
                            dr("Remark") = drResult("Remark")
                            ViewState("Dt").Rows.Add(dr)
                        End If
                    Next
                    BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    CountTotalDt()
                End If


                Session("Result") = Nothing
                ViewState("Sender") = Nothing
                'Session("filter") = Nothing
                'Session("Column") = Nothing
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
        ViewState("PPN") = SQLExecuteScalar("Select Max(PPN) FROM MsPPN ", ViewState("DBConnection"))
    End Sub

    Protected Sub ddlShowRecord_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlShowRecord.SelectedIndexChanged
        GridView1.PageIndex = 0
        GridView1.EditIndex = -1
        GridView1.PageSize = ddlShowRecord.SelectedValue
        BindData()
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


        tbTotalBiaya.Attributes.Add("OnBlur", "setformatfordt();")
        tbPpn.Attributes.Add("OnBlur", "setformatfordt();")
        tbPpnValue.Attributes.Add("OnBlur", "setformatfordt();")
        tbPph.Attributes.Add("OnBlur", "setformatfordt();")
        tbpphValue.Attributes.Add("OnBlur", "setformatfordt();")
        tbTotalAmount.Attributes.Add("OnBlur", "setformatfordt();")
        tbOriginalAmount.Attributes.Add("OnBlur", "setformatfordt();")


        'Me.tbTotalBiaya.Attributes.Add("ReadOnly", "True")
        Me.tbpphValue.Attributes.Add("ReadOnly", "True")
        Me.tbPpnValue.Attributes.Add("ReadOnly", "True")
        Me.tbTotalAmount.Attributes.Add("ReadOnly", "True")
        Me.tbOriginalAmount.Attributes.Add("ReadOnly", "True")
        'Me.tbPenawaranNo.Attributes.Add("ReadOnly", "True")
        'Me.tbIntruksiNo.Attributes.Add("ReadOnly", "True")

        Me.tbTotalBiaya.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbPpn.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbpphValue.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbPph.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbpphValue.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbTotalAmount.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbOriginalAmount.Attributes.Add("OnKeyDown", "return PressNumeric();")



        tbBiayaSatuan.Attributes.Add("OnBlur", "setformathd();")
        tbLuas.Attributes.Add("OnBlur", "setformathd();")
        tbBiaya.Attributes.Add("OnBlur", "setformathd();")
        tbBAP.Attributes.Add("OnBlur", "setformathd();")
        tbBAPValue.Attributes.Add("OnBlur", "setformathd();")
        tbSisaBiaya.Attributes.Add("OnBlur", "setformathd();")

        Me.tbBiaya.Attributes.Add("ReadOnly", "True")


        Me.tbBiayaSatuan.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbBiaya.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbBiaya.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbBAP.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbBAPValue.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbSisaBiaya.Attributes.Add("OnKeyDown", "return PressNumeric();")



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
        Return "SELECT * From V_PRCSPKBeginDt WHERE TransNmbr = " + QuotedStr(Nmbr)
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
                Session("SelectCommand") = "EXEC S_PRCFormPemenang " + Result
                Session("ReportFile") = ".../../../Rpt/FormPemenang.frx"
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
                        Result = ExecSPCommandGo(ActionValue, "S_PRCSPKBegin", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            'tbDate.Enabled = State
            'tbSuppCode.Enabled = False
            'tbSuppName.Enabled = False
            'btnSupp.Visible = True
            'tbPenawaranNo.Enabled = True
            'ddlLokasi.Enabled = False
            'tbIntruksiNo.Enabled = False
            'tbPekerjaan.Enabled = False
            'tbStartDate.Enabled = False
            'tbEndDate.Enabled = False
            'tbDurasi.Enabled = False
            'tbSPK.Enabled = False
            'tbTugasName.Enabled = False
            'tbTotalAmount.Enabled = State
            ''tbPph.Enabled = State
            'tbpphValue.Enabled = State
            'tbPpnValue.Enabled = State
            'tbPpnValue.Enabled = State
            'tbTotalBiaya.Enabled = False
            'tbRemark.Enabled = State

        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub



    Private Sub EnableDt(ByVal State As Boolean)
        Try
            tbUraian.Enabled = State
            tbLuas.Enabled = State
            tbBiaya.Enabled = State
            tbRemarkDt.Enabled = State
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
                If ViewState("DtValue") <> lbItemNo.Text Then
                    If CekExistData(ViewState("Dt"), "ItemNo", lbItemNo.Text) Then
                        lbStatus.Text = "Item No " + lbItemNo.Text + " has been already exist"
                        Exit Sub
                    End If
                End If
                Row = ViewState("Dt").Select("ItemNo = " + QuotedStr(ViewState("DtValue")))(0)
                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                Row("UraianPekerjaan") = tbUraian.Text
                Row("Luas") = tbLuas.Text
                Row("Biaya") = tbBiaya.Text
                Row("BAPAwal") = tbBAP.Text
                Row("BAPawalValue") = tbBAPValue.Text
                Row("SisaBiaya") = tbSisaBiaya.Text
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()

            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If

                If CekExistData(ViewState("Dt"), "ItemNo", lbItemNo.Text) = True Then
                    lbStatus.Text = "Item No " + lbItemNo.Text + " has already been exist"
                    Exit Sub
                End If



                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("ItemNo") = CInt(lbItemNo.Text)
                dr("UraianPekerjaan") = tbUraian.Text
                dr("Luas") = tbLuas.Text
                dr("Biaya") = tbBiaya.Text
                dr("BAPAwal") = tbBAP.Text
                dr("BAPawalValue") = tbBAPValue.Text
                dr("SisaBiaya") = tbSisaBiaya.Text
                dr("Remark") = tbRemarkDt.Text
                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            BindGridDt(ViewState("Dt"), GridDt)
            StatusButtonSave(True)

            CountTotalDt()
            AttachScript("setformatfordt();", Page, Me.GetType())

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
                CountTotalDt()

                tbRef.Text = GetAutoNmbr("SPKB", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)


                SQLString = "INSERT INTO PRCSPKBeginHd (TransNmbr,Status, TransDate,No_Penawaran,No_Intruksi,Paket_Pekerjaan,SuppCode,Lokasi, " + _
                "StartDate,EndDate,Durasi,TotalBiaya,Ppn, PpnValue, Pph, PphValue,TotalAmount,TugasCode,TypeSPK, Alloc_Amount, OriginalAmount, " + _
                "Remark, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbRef.Text) + ", 'H', '" + Format(tbDate.SelectedDate, "yyyy-MM-dd") + "', " + _
                QuotedStr(tbPenawaranNo.Text) + ", " + QuotedStr(tbIntruksiNo.Text) + "," + QuotedStr(tbPekerjaan.Text) + ", " + _
                QuotedStr(tbSuppCode.Text) + ", " + QuotedStr(ddlLokasi.SelectedValue) + ",'" + _
                Format(tbStartDate.SelectedDate, "yyyy-MM-dd") + "', '" + Format(tbEndDate.SelectedDate, "yyyy-MM-dd") + "', " + _
                QuotedStr(tbDurasi.Text) + ", " + _
                QuotedStr(tbTotalBiaya.Text.Replace(",", "")) + ", " + QuotedStr(tbPpn.Text.Replace(",", "")) + ", " + _
                QuotedStr(tbPpnValue.Text.Replace(",", "")) + ", " + QuotedStr(tbPph.Text.Replace(",", "")) + "," + QuotedStr(tbpphValue.Text.Replace(",", "")) + ", " + _
                QuotedStr(tbTotalAmount.Text.Replace(",", "")) + ", " + QuotedStr(tbTugasCode.Text) + ",  " + QuotedStr(tbSPK.Text) + " ," + QuotedStr(tbUnallocatedAmount.Text.Replace(",", "")) + "," + _
                QuotedStr(tbOriginalAmount.Text.Replace(",", "")) + "," + _
                QuotedStr(tbRemark.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", getDate()"
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM PRCSPKBeginHd WHERE TransNmbr = " + QuotedStr(tbRef.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If

                SQLString = "UPDATE PRCSPKBeginHd SET SuppCode = " + QuotedStr(tbSuppCode.Text) + ", No_Penawaran = " + QuotedStr(tbPenawaranNo.Text) + _
                ", No_Intruksi = " + QuotedStr(tbIntruksiNo.Text) + _
                ", Lokasi = " + QuotedStr(ddlLokasi.SelectedValue) + _
                ", Paket_Pekerjaan = " + QuotedStr(tbPekerjaan.Text) + _
                ", Durasi = " + QuotedStr(tbDurasi.Text) + _
                ", TotalBiaya = " + QuotedStr(tbTotalBiaya.Text.Replace(",", "")) + _
                ", Ppn = " + QuotedStr(tbPpn.Text.Replace(",", "")) + _
                ", PpnValue = " + QuotedStr(tbPpnValue.Text.Replace(",", "")) + _
                ", Pph = " + QuotedStr(tbPph.Text.Replace(",", "")) + _
                ", PphValue = " + QuotedStr(tbpphValue.Text.Replace(",", "")) + _
                ", TotalAmount = " + QuotedStr(tbTotalAmount.Text.Replace(",", "")) + _
                ", Alloc_Amount = " + QuotedStr(tbUnallocatedAmount.Text.Replace(",", "")) + _
                ", OriginalAmount = " + QuotedStr(tbOriginalAmount.Text.Replace(",", "")) + _
                ", Remark = " + QuotedStr(tbRemark.Text) + _
                ", StartDate = '" + Format(tbStartDate.SelectedDate, "yyyy-MM-dd") + _
                "', EndDate = '" + Format(tbEndDate.SelectedDate, "yyyy-MM-dd") + _
                "', DatePrep = getDate()" + _
                ", TugasCode = " + QuotedStr(tbTugasCode.Text) + _
                ", TypeSPK = " + QuotedStr(tbSPK.Text) + _
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
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, ItemNo, UraianPekerjaan,UnitCode, Luas,BiayaSatuan, Biaya, BAPAwal, BAPAwalValue, SisaBiaya,  Remark FROM PRCSPKBeginDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            'Dim param As SqlParameter
            '' Create the UpdateCommand.
            'Dim Update_Command = New SqlCommand( _
            '"UPDATE PRCSPKBeginDt SET ItemNo = @ItemNo, InvoiceNo = @InvoiceNo, " + _
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
            '    "DELETE FROM PRCSPKBeginDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND ItemNo = @ItemNo ", con)
            '' Add the parameters for the DeleteCommand.
            'param = Delete_Command.Parameters.Add("@ItemNo", SqlDbType.Int, 4, "ItemNo")
            'param.SourceVersion = DataRowVersion.Original
            'da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("PRCSPKBeginDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt
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
            tbStartDate.SelectedDate = ViewState("ServerDate") 'Today
            tbEndDate.SelectedDate = ViewState("ServerDate") 'Today
            tbSuppCode.Text = ""
            tbSuppName.Text = ""
            tbPenawaranNo.Text = ""
            ddlLokasi.SelectedIndex = 0
            tbIntruksiNo.Text = ""
            tbPekerjaan.Text = ""
            tbDurasi.Text = ""
            tbTotalAmount.Text = 0
            tbPpn.Text = ViewState("PPN")
            tbPpnValue.Text = 0
            tbPph.Text = 0
            tbpphValue.Text = 0
            tbTotalBiaya.Text = 0
            tbBiayaSatuan.Text = 0
            ddlSatuan.SelectedValue = ""
            tbRemark.Text = ""
            tbTugasCode.Text = ""
            tbTugasName.Text = ""
            tbSPK.Text = ""
            tbSPKName.Text = ""
            tbUnallocatedAmount.Text = 0
            tbOriginalAmount.Text = 0
            EnableHd(True)
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub Cleardt()
        Try
            tbUraian.Text = ""
            tbLuas.Text = 0
            tbBiaya.Text = 0
            tbBiayaSatuan.Text = 0
            tbBAP.Text = 0
            tbBAPValue.Text = 0
            tbSisaBiaya.Text = 0
            ddlSatuan.SelectedValue = ""
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

    'Protected Sub btnGetInvt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGetInv.Click
    '    Dim ResultField, ResultSame As String 'ResultSame 
    '    Try
    '        If CekHd() = False Then
    '            Exit Sub
    '        End If
    '        Session("Result") = Nothing
    '        Session("Filter") = "select * from V_GetInvPosting WHERE Supplier+'|'+SupplierType= " + QuotedStr(tbSuppCode.Text) + "+'|'+" + QuotedStr(tbSuppType.Text)
    '        ResultField = "Invoice_No,Invoicetype, Invoice_Date,Supplier, SupplierName, Rfference_No,Invoice,Potongan, DPP, PPn, PPn_Invoice, PPh,PPh_Invoice, TotalAmount, SupplierType, Remark"
    '        Session("Column") = ResultField.Split(",")
    '        ResultSame = "Supplier, SupplierType, Invoicetype"
    '        Session("ResultSame") = ResultSame.Split(",")
    '        ViewState("Sender") = "btnGetInv"
    '        Session("DBConnection") = ViewState("DBConnection")
    '        AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lbStatus.Text = "btn get Dt Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub lbCount_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCount.Click
        Dim ResultField, CriteriaField, sqlstring As String
        Try
            sqlstring = "EXEC S_GetPenerimaan 1 "
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = sqlstring
            ResultField = "No_Penerimaan, InstruksiNo, SuppCode, SuppName, PaketPekerjaan, Lokasi, StartDate, EndDate, Durasi"
            CriteriaField = "No_Penerimaan, InstruksiNo, SuppCode, SuppName, PaketPekerjaan, Lokasi, StartDate, EndDate, Durasi"
            Session("CriteriaField") = CriteriaField.Split(",")
            ViewState("Sender") = "btnOut"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchGrid();", Page, Me.GetType())

        Catch ex As Exception
            lbStatus.Text = "btn Search Supp Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub tbSuppCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbSuppCode.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Supplier", tbSuppCode.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbSuppCode.Text = Dr("Supplier_Code")
                tbSuppName.Text = Dr("Supplier_Name")
            Else
                tbSuppCode.Text = ""
                tbSuppName.Text = ""
            End If
            'ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, ViewState("DBConnection").ToString)
            'ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
            'AttachScript("setformat();", Page, Me.GetType())
            tbSuppCode.Focus()
        Catch ex As Exception
            Throw New Exception("tb SuppCode Error : " + ex.ToString)
        End Try
    End Sub



    Private Sub CountTotalDt()
        Dim TotalBiaya, SisaBiaya As Double
        Dim Dr As DataRow
        Try

            TotalBiaya = 0
            For Each Dr In ViewState("Dt").Rows
                If Not Dr.RowState = DataRowState.Deleted Then
                    TotalBiaya = TotalBiaya + CFloat(Dr("Biaya").ToString)
                    SisaBiaya = SisaBiaya + CFloat(Dr("SisaBiaya").ToString)
                End If
            Next
            tbOriginalAmount.Text = FormatNumber(TotalBiaya, ViewState("DigitHome"))
            tbTotalBiaya.Text = FormatNumber(SisaBiaya, ViewState("DigitHome"))
            tbPpnValue.Text = CFloat(tbPpn.Text) * CFloat(tbTotalBiaya.Text) / 100
            tbpphValue.Text = CFloat(tbPph.Text) * CFloat(tbTotalBiaya.Text) / 100

            tbTotalAmount.Text = CFloat(tbTotalBiaya.Text) + CFloat(tbPpnValue.Text) - CFloat(tbpphValue.Text)
            tbTotalAmount.Text = FormatNumber(tbTotalAmount.Text, ViewState("DigitHome"))
            tbPpnValue.Text = FormatNumber(tbPpnValue.Text, ViewState("DigitHome"))
            tbpphValue.Text = FormatNumber(tbpphValue.Text, ViewState("DigitHome"))
            'AttachScript("setformatfordt();", Page, Me.GetType())
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
        Try
            If tbDate.IsNull Then
                lbStatus.Text = MessageDlg("Transaction Date must have value")
                tbDate.Focus()
                Return False
            End If

            'If CInt(Session(Request.QueryString("KeyId"))("Year")) <> Year(tbDate.SelectedValue) Or CInt(Session(Request.QueryString("KeyId"))("Period")) <> Month(tbDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(Session(Request.QueryString("KeyId"))("Period")) + " " + Session(Request.QueryString("KeyId"))("Year").ToString.Trim)
            '    Return False
            'End If

            If tbSuppCode.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Supplier must have value")
                btnSupp.Focus()
                Return False
            End If

            If tbPenawaranNo.Text = "" Then
                lbStatus.Text = MessageDlg("Penawaran No must have value")
                tbPenawaranNo.Focus()
                Return False
            End If

            If tbIntruksiNo.Text = "" Then
                lbStatus.Text = MessageDlg("Instruksi No must have value")
                tbIntruksiNo.Focus()
                Return False
            End If

            If tbTugasCode.Text = "" Then
                lbStatus.Text = MessageDlg("Pemberi Tugas must have value")
                tbTugasCode.Focus()
                Dim CekTugas As String
                CekTugas = SQLExecuteScalar("Select COUNT(TugasCode) From MsPemberiTugas", ViewState("DBConnection"))
                'lbStatus.Text = CekSpk
                'Exit Function
                If Val(CekTugas) <> 0 Then
                    btnTugas_Click(Nothing, Nothing)
                Else
                    lbPemberiTugas_Click(Nothing, Nothing)
                End If

                Return False
            End If

            If tbSPK.Text = "" Then
                lbStatus.Text = MessageDlg("Template SPK must have value")
                tbSPK.Focus()
                Dim CekSpk As String
                CekSpk = SQLExecuteScalar("Select COUNT(TypeSPKCode) From MsTypeSPK", ViewState("DBConnection"))
                'lbStatus.Text = CekSpk
                'Exit Function
                If Val(CekSpk) <> 0 Then
                    btnSPK_Click(Nothing, Nothing)
                Else
                    lbSPKtype_Click(Nothing, Nothing)
                End If


                Return False
            End If


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

                If Dr("Luas").ToString = "0" Or Dr("Luas").ToString = "" Then
                    lbStatus.Text = MessageDlg("Price Forex Must Have Value")
                    Return False
                End If

                If Dr("Biaya").ToString = "0" Or Dr("Biaya").ToString = "" Then
                    lbStatus.Text = MessageDlg("Biaya Must Have Value")
                    Return False
                End If

            Else

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
                        Dim Form1, Form2, Form3 As String
                        Form1 = "1"
                        Form2 = "2"
                        Form3 = "3"
                        Session("SelectCommand") = "EXEC S_FNFormBeginSPK '''" + GVR.Cells(2).Text + "''', " + QuotedStr(ViewState("UserId"))
                        Session("SelectCommand2") = "EXEC S_FNFormBeginSPK '''" + GVR.Cells(2).Text + "''', " + QuotedStr(ViewState("UserId"))
                        Session("SelectCommand3") = "EXEC S_FNFormBeginSPK '''" + GVR.Cells(2).Text + "''', " + QuotedStr(ViewState("UserId"))
                        Session("ReportFile") = ".../../../Rpt/FormBeginSPK.frx"
                        Session("DBConnection") = ViewState("DBConnection")
                        AttachScript("openprintdlg3ds();", Page, Me.GetType)
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
            dr = ViewState("Dt").Select("ItemNo = " + GVR.Cells(1).Text)
            dr(0).Delete()
            BindGridDt(ViewState("Dt"), GridDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        'Dim Dr As DataRow
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            FillTextBoxDt(GVR.Cells(1).Text)
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)

            ViewState("StateDt") = "Edit"
            ViewState("DtValue") = lbItemNo.Text

            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbSupp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSupp.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsSupplier')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb Supplier Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbStartDate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbStartDate.SelectionChanged
        Dim Sqlstring As String
        Sqlstring = SQLExecuteScalar("EXEC  S_GetSlisihHari '" + Format(tbStartDate.SelectedDate, "yyyy/MM/dd") + "', '" + Format(tbEndDate.SelectedDate, "yyyy/MM/dd") + "' ", ViewState("DBConnection"))
        tbDurasi.Text = Sqlstring
    End Sub


    Protected Sub tbEndDate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbEndDate.SelectionChanged
        Dim Sqlstring As String
        Sqlstring = SQLExecuteScalar("EXEC  S_GetSlisihHari '" + Format(tbStartDate.SelectedDate, "yyyy/MM/dd") + "', '" + Format(tbEndDate.SelectedDate, "yyyy/MM/dd") + "' ", ViewState("DBConnection"))
        tbDurasi.Text = Sqlstring
    End Sub

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            tbRef.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToText(tbSuppCode, Dt.Rows(0)("SuppCode").ToString)
            BindToText(tbSuppName, Dt.Rows(0)("Supplier_Name").ToString)
            BindToText(tbTugasCode, Dt.Rows(0)("TugasCode").ToString)
            BindToText(tbTugasName, Dt.Rows(0)("TugasName").ToString)
            BindToText(tbSPK, Dt.Rows(0)("TypeSPK").ToString)
            BindToText(tbSPKName, Dt.Rows(0)("TypeSPKName").ToString)
            BindToText(tbPenawaranNo, Dt.Rows(0)("No_Penawaran").ToString)
            BindToText(tbIntruksiNo, Dt.Rows(0)("No_Intruksi").ToString)
            BindToDropList(ddlLokasi, Dt.Rows(0)("Lokasi").ToString)
            BindToText(tbPekerjaan, Dt.Rows(0)("Paket_Pekerjaan").ToString)
            BindToDate(tbStartDate, Dt.Rows(0)("StartDate").ToString)
            BindToDate(tbEndDate, Dt.Rows(0)("EndDate").ToString)
            BindToText(tbDurasi, Dt.Rows(0)("Durasi").ToString)
            BindToText(tbTotalBiaya, Dt.Rows(0)("TotalBiaya").ToString, ViewState("DigitCurr"))
            BindToText(tbPpn, Dt.Rows(0)("Ppn").ToString, ViewState("DigitCurr"))
            BindToText(tbPpnValue, Dt.Rows(0)("PpnValue").ToString, ViewState("DigitCurr"))
            BindToText(tbPph, Dt.Rows(0)("Pph").ToString, ViewState("DigitCurr"))
            BindToText(tbpphValue, Dt.Rows(0)("PphValue").ToString, ViewState("DigitCurr"))
            BindToText(tbTotalAmount, Dt.Rows(0)("TotalAmount").ToString, ViewState("DigitCurr"))
            BindToText(tbUnallocatedAmount, Dt.Rows(0)("Alloc_Amount").ToString, ViewState("DigitCurr"))
            BindToText(tbOriginalAmount, Dt.Rows(0)("OriginalAmount").ToString, ViewState("DigitCurr"))
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
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
                BindToText(tbUraian, Dr(0)("UraianPekerjaan").ToString)
                BindToDropList(ddlSatuan, Dr(0)("UnitCode").ToString)
                BindToText(tbLuas, Dr(0)("Luas").ToString, ViewState("DigitHome"))
                BindToText(tbBiaya, Dr(0)("Biaya").ToString, ViewState("DigitHome"))
                BindToText(tbBiayaSatuan, Dr(0)("BiayaSatuan").ToString, ViewState("DigitHome"))
                BindToText(tbBAP, Dr(0)("BAPAwal").ToString, ViewState("DigitHome"))
                BindToText(tbBAPValue, Dr(0)("BAPAwalValue").ToString, ViewState("DigitHome"))
                BindToText(tbSisaBiaya, Dr(0)("SisaBiaya").ToString, ViewState("DigitHome"))
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
                'ViewState("DigitCurr") = SQLExecuteScalar("Select Digit From VMsCurrency WHERE Currency = " + QuotedStr(ddlCurr.SelectedValue), ViewState("DBConnection"))
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


    'Protected Sub btnPenawaran_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPenawaran.Click


    '    Dim ResultField, CriteriaField, sqlstring As String
    '    Try
    '        sqlstring = "EXEC S_GetPenerimaan 1 "
    '        Session("DBConnection") = ViewState("DBConnection")
    '        Session("filter") = sqlstring
    '        ResultField = "No_Penerimaan, InstruksiNo, SuppCode, SuppName, PaketPekerjaan, Lokasi, StartDate, EndDate, Durasi"
    '        CriteriaField = "No_Penerimaan, InstruksiNo, SuppCode, SuppName, PaketPekerjaan, Lokasi, StartDate, EndDate, Durasi"
    '        Session("CriteriaField") = CriteriaField.Split(",")
    '        ViewState("Sender") = "btnPenawaran"
    '        Session("Column") = ResultField.Split(",")
    '        AttachScript("OpenSearchGrid();", Page, Me.GetType())

    '    Catch ex As Exception
    '        lbStatus.Text = "btn Search Supp Error : " + ex.ToString
    '    End Try
    'End Sub


    Protected Sub btnTugas_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnTugas.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT TugasCode, TugasName, Title FROM MsPemberiTugas "
            ResultField = "TugasCode, TugasName, Title"
            ViewState("Sender") = "btnTugas"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("myPopup();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Cust Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnSPK_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSPK.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT TypeSPKCode, TypeSPKName FROM V_MstypeSPK "
            ResultField = "TypeSPKCode, TypeSPKName"
            ViewState("Sender") = "btnSPK"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("myPopup();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Cust Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbPemberiTugas_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbPemberiTugas.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsTugas')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lbArea_Click Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub lbSPKtype_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSPKtype.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsSPKType')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lbArea_Click Error : " + ex.ToString
        End Try
    End Sub


End Class
