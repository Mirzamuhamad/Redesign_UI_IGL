Imports System.Data
Imports System.Data.SqlClient
'Imports System.Data.Odbc
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc


Partial Class TrREvPemesanan
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select * From V_MKTTSPRevHD"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                SetInit()

                FillCombo(ddlType, "SELECT TypeCode, TypeName FROM V_TypeBayar", False, "TypeCode", "TypeName", ViewState("DBConnection"))
                FillCombo(ddlBank, "SELECT BankCode, BankName FROM MsBank", True, "BankCode", "BankName", ViewState("DBConnection"))
                Session("AdvanceFilter") = ""


            End If
            lbStatus.Text = ""
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If
            GridDt.Columns(0).Visible = False

            ' hasil dari search dialog
            If Not Session("Result") Is Nothing Then


                If ViewState("Sender") = "btnSP" Then
                    Dim drResult As DataRow
                    Dim ExistRow As DataRow()
                    Dim DtPesanan As DataTable
                    Dim SQLPesanan As String

                    tbNoPesan.Text = Session("Result")(0).ToString
                    tbAngsuran.Text = Session("Result")(5).ToString

                    SQLPesanan = "SELECT * FROM V_MKTTspDt WHERE TransNmbr =  " + QuotedStr(tbNoPesan.Text) ' + " And TransNmbr = " + QuotedStr(tbSpNo.Text)
                    DtPesanan = SQLExecuteQuery(SQLPesanan, ViewState("DBConnection")).Tables(0)

                    ''Insert Detail
                    For Each drResult In DtPesanan.Rows
                        ExistRow = ViewState("Dt").Select("UnitCode = " + QuotedStr(drResult("UnitCode").ToString))
                        If ExistRow.Count = 0 Then
                            'insert
                            If tbNoPesan.Text.Trim = "" Then
                                BindToText(tbNoPesan, drResult("TransNmbr").ToString)
                            End If

                            Dim dr As DataRow
                            dr = ViewState("Dt").NewRow
                            dr("Revisi") = lblRevisi.Text
                            dr("UnitCode") = drResult("UnitCode").ToString
                            dr("UnitName") = drResult("UnitName").ToString
                            dr("LuasTanah") = drResult("LuasTanah").ToString
                            dr("LuasBangunan") = drResult("LuasBangunan").ToString
                            dr("Price") = drResult("Price").ToString
                            dr("PpnTotal") = drResult("PpnTotal").ToString
                            dr("PphTotal") = drResult("PphTotal").ToString
                            dr("AmountTotal") = drResult("AmountTotal").ToString
                            dr("TJ") = drResult("TJ").ToString
                            dr("DP") = drResult("DP").ToString
                            dr("SisaTagihan") = drResult("SisaTagihan").ToString
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


                        SQLString = "SELECT * FROM V_MKTTspDt2 WHERE UnitCode =  " + QuotedStr(drResult("UnitCode").ToString) + " And TransNmbr = " + QuotedStr(tbNoPesan.Text)
                        DtPekerjaan = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)

                        For Each drDtResult In DtPekerjaan.Rows
                            ExistRowDT = ViewState("Dt2").Select("ItemNo+'|'+UnitCode= " + QuotedStr(drDtResult("ItemNo").ToString + "|" + drResult("UnitCode").ToString))
                            If ExistRowDT.Count = 0 Then
                                Dim Dtdr As DataRow
                                Dtdr = ViewState("Dt2").NewRow
                                Dtdr("UnitCode") = lblUnit.Text
                                Dtdr("Revisi") = lblRevisi.Text
                                Dtdr("ItemNo") = drDtResult("ItemNo".ToString)
                                Dtdr("PayDate") = drDtResult("PayDate".ToString)
                                Dtdr("Type") = drDtResult("Type".ToString)
                                Dtdr("TypeName") = drDtResult("TypeName".ToString)
                                Dtdr("PayKav") = drDtResult("PayKav".ToString)
                                Dtdr("PPn") = drDtResult("Ppn".ToString)
                                Dtdr("PpnValue") = drDtResult("PpnValue".ToString)
                                Dtdr("Pph") = drDtResult("Pph".ToString)
                                Dtdr("PphValue") = drDtResult("PphValue".ToString)
                                Dtdr("BankCode") = drDtResult("BankCode".ToString)
                                Dtdr("BankName") = drDtResult("BankName".ToString)
                                Dtdr("NoGiro") = drDtResult("NoGiro".ToString)
                                Dtdr("AmountDt2") = drDtResult("AmountDt2".ToString)
                                Dtdr("AngsuranKe") = drDtResult("AngsuranKe".ToString)
                                Dtdr("Remark") = drDtResult("Remark".ToString)
                                ViewState("Dt2").Rows.Add(Dtdr)
                            End If


                        Next
                        CountTotalDt()
                        'btnGetAngsuran_Click(Nothing, Nothing)


                    Next

                    BindGridDt(ViewState("Dt"), GridDt)
                    GridDt.Columns(0).Visible = False
                    'CountTotalDt2()
                    'EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
                    StatusButtonSave(True)

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

    'Private Sub CountTotalDt2()
    '    Dim Price, Disc, Dpp, Ppn, Pph, Total As Double
    '    Dim Dr As DataRow
    '    Try


    '        Price = 0
    '        Ppn = 0
    '        Pph = 0
    '        Total = 0
    '        Disc = 0
    '        Dpp = 0
    '        For Each Dr In ViewState("Dt").Rows
    '            If Not Dr.RowState = DataRowState.Deleted Then
    '                Price = Price + CFloat(Dr("Price").ToString)
    '                Ppn = Ppn + CFloat(Dr("PpnTotal").ToString)
    '                Pph = Pph + CFloat(Dr("PphTotal").ToString)
    '                Total = Total + CFloat(Dr("AmountTotal").ToString)
    '                Disc = Disc + CFloat(Dr("DiscValue").ToString)
    '                Dpp = Dpp + CFloat(Dr("Dpp").ToString)
    '            End If
    '        Next
    '        tbTotAmount.Text = FormatNumber(Total, ViewState("DigitHome"))
    '        tbTotPPN.Text = FormatNumber(Ppn, ViewState("DigitHome"))
    '        tbTotPPH.Text = FormatNumber(Pph, ViewState("DigitHome"))
    '        tbTotDisc.Text = FormatNumber(Disc, ViewState("DigitHome"))
    '        tbTotDPP.Text = FormatNumber(Dpp, ViewState("DigitHome"))
    '        tbTotHarga.Text = FormatNumber(Price, ViewState("DigitHome"))
    '        'AttachScript("setformathd();", Page, Me.GetType())
    '    Catch ex As Exception
    '        Throw New Exception("Count Total Dt Error : " + ex.ToString)
    '    End Try
    'End Sub

    Private Sub CountTotalDt()
        Dim TotalPpn, TotalPph, QtyTotal, TJ, DP, PL, AG As Double
        Dim Unit, UnitDP, UnitPL As String
        Dim Dr As DataRow
        Dim drow As DataRow()
        Dim havedetail As Boolean
        Try
            drow = ViewState("Dt2").Select("UnitCode = " + QuotedStr(TrimStr(lblUnit.Text)))
            QtyTotal = 0
            TotalPpn = 0
            TotalPph = 0
            If drow.Length > 0 Then
                havedetail = False
                For Each Dr In drow.CopyToDataTable.Rows
                    If Not Dr.RowState = DataRowState.Deleted Then
                        QtyTotal = QtyTotal + CFloat(Dr("AmountDt2").ToString)
                        TotalPpn = TotalPpn + CFloat(Dr("PpnValue").ToString)
                        TotalPph = TotalPph + CFloat(Dr("PphValue").ToString)
                    End If
                Next
            End If

            Unit = lblUnit.Text + "|TJ"
            drow = ViewState("Dt2").Select("UnitCode+'|'+Type = " + QuotedStr(Unit))
            TJ = 0
            If drow.Length > 0 Then
                havedetail = False
                For Each Dr In drow.CopyToDataTable.Rows
                    If Not Dr.RowState = DataRowState.Deleted Then
                        TJ = TJ + CFloat(Dr("AmountDt2").ToString)

                    End If
                Next
            End If


            UnitDP = lblUnit.Text + "|DP"
            drow = ViewState("Dt2").Select("UnitCode+'|'+Type = " + QuotedStr(UnitDP))
            DP = 0
            If drow.Length > 0 Then
                havedetail = False
                For Each Dr In drow.CopyToDataTable.Rows
                    If Not Dr.RowState = DataRowState.Deleted Then
                        DP = DP + CFloat(Dr("AmountDt2").ToString)

                    End If
                Next
            End If


            UnitDP = lblUnit.Text + "|AG"
            drow = ViewState("Dt2").Select("UnitCode+'|'+Type = " + QuotedStr(UnitDP))
            AG = 0
            If drow.Length > 0 Then
                havedetail = False
                For Each Dr In drow.CopyToDataTable.Rows
                    If Not Dr.RowState = DataRowState.Deleted Then
                        AG = AG + CFloat(Dr("AmountDt2").ToString)

                    End If
                Next
            End If



            UnitDP = lblUnit.Text + "|PL"
            drow = ViewState("Dt2").Select("UnitCode+'|'+Type = " + QuotedStr(UnitDP))
            PL = 0
            If drow.Length > 0 Then
                havedetail = False
                For Each Dr In drow.CopyToDataTable.Rows
                    If Not Dr.RowState = DataRowState.Deleted Then
                        PL = PL + CFloat(Dr("AmountDt2").ToString)

                    End If
                Next
            End If


            Dr = ViewState("Dt").Select("UnitCode = " + QuotedStr(TrimStr(lblUnit.Text)))(0)
            'price = CFloat(Dr("Price").ToString)
            'lbStatus.Text = Dr("PriceForex").ToString + "   " + ViewState("DigitCurr").ToString
            'Exit Sub
            Dr.BeginEdit()
            Dr("PpnTotal") = TotalPpn 'FormatNumber(QtyTotal, ViewState("DigitQty"))
            Dr("PphTotal") = TotalPph
            Dr("AmountTotal") = QtyTotal
            Dr("TJ") = TJ
            Dr("DP") = DP
            Dr("SisaTagihan") = FormatNumber(PL + AG, ViewState("DigitHome"))
            ' CountTotalDt2()
            Dr.EndEdit()
            BindGridDt(ViewState("Dt"), GridDt)
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
            'drow = ViewState("Dt2").Select("UnitCode+'|'+Type = " + QuotedStr(UnitAG))
            'lblAngsuran.Text = drow.Length + 1

            UnitAG = lblUnit.Text + "|DP"
            drow = ViewState("Dt2").Select("UnitCode+'|'+Type = " + QuotedStr(UnitAG))
            If drow.Length > 0 Then
                havedetail = False
                For Each Dr In drow.CopyToDataTable.Rows
                    If Not Dr.RowState = DataRowState.Deleted Then
                        DateAngsuran = (Dr("PayDate").ToString)
                    End If
                Next
            End If


            If lbItemNo.Text >= 3 Then
                If ddlType.SelectedValue = "AG" Then
                    lblAngsuran.Text = lbItemNo.Text - 2
                    Nilai = lbItemNo.Text - 2
                    tbTempoDate.SelectedDate = DateAdd(DateInterval.Month, Nilai, DateAngsuran)
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
            If GetCountRecord(ViewState("Dt")) <> 0 Then
                dt = ViewState("Dt")
                baris = dt.Rows.Count
                For i = 0 To GetCountRecord(ViewState("Dt")) - 1 'For i = 0 To baris - 1
                    GVR = GridDt.Rows(i)
                    Unit = GVR.Cells(2).Text
                    qty = GVR.Cells(6).Text
                    QtyTotal = 0

                    drow = ViewState("Dt2").Select("UnitCode = " + QuotedStr(Unit))

                    If drow.Length > 0 Then
                        For Each Dr In drow.CopyToDataTable.Rows
                            If Not Dr.RowState = DataRowState.Deleted Then
                                QtyTotal = QtyTotal + CFloat(Dr("PayKav").ToString)
                            End If
                        Next
                    End If

                    If qty <> QtyTotal Then
                        Dim Selisih As String
                        Selisih = FormatNumber(qty - QtyTotal, ViewState("DigitHome"))
                        lbStatus.Text = MessageDlg(GVR.Cells(3).Text + "Price must be equall with Nominal Total in Detail 2, Selisih : " + Selisih)
                        Return False
                    End If


                    UnitAG = GVR.Cells(2).Text + "|AG"
                    drow = ViewState("Dt2").Select("UnitCode+'|'+Type = " + QuotedStr(UnitAG))

                    If drow.Length <> tbAngsuran.Text Then
                        lbStatus.Text = MessageDlg(GVR.Cells(3).Text + " Masa Aangsuran must be equall with Angsuran in Detail 2")
                        Return False
                    End If



                Next
            End If

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

        tbNominal.Attributes.Add("OnBlur", "setformatdt2();")
        tbPpnDt2.Attributes.Add("OnBlur", "setformatdt2();")
        tbPpnValuedt2.Attributes.Add("OnBlur", "setformatdt2();")
        tbPphDt2.Attributes.Add("OnBlur", "setformatdt2();")
        tbPphValueDt2.Attributes.Add("OnBlur", "setformatdt2();")
        tbTotalNominal.Attributes.Add("OnBlur", "setformatdt2();")

        tbPrice.Attributes.Add("OnBlur", "setformathd();")
      
        tbTotalAmount.Attributes.Add("OnBlur", "setformathd();")
        tbPpnTotal.Attributes.Add("OnBlur", "setformathd();")
        tbPphtotal.Attributes.Add("OnBlur", "setformathd();")


        Me.tbTotalAmount.Attributes.Add("ReadOnly", "True")
        Me.tbPpnTotal.Attributes.Add("ReadOnly", "True")
        Me.tbPphtotal.Attributes.Add("ReadOnly", "True")
        Me.tbPpnValuedt2.Attributes.Add("ReadOnly", "True")
        Me.tbPphValueDt2.Attributes.Add("ReadOnly", "True")
        Me.tbTotalAmount.Attributes.Add("ReadOnly", "True")
        Me.tbTotalNominal.Attributes.Add("ReadOnly", "True")


        'Proteksi agar hanya angka saja yang bisa di input
        tbLuasBangunan.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbLuasTanah.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbPpnDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbPphDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbPrice.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbNominal.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbAngsuran.Attributes.Add("OnKeyDown", "return PressNumeric();")

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

    Private Function GetStringDt(ByVal Nmbr As String, ByVal Revisi As String) As String
        Return "SELECT * From V_MKTTSPRevDt WHERE TransNmbr = " + QuotedStr(Nmbr) + " AND Revisi =" + Revisi
    End Function

    Private Function GetStringDt2(ByVal Nmbr As String, ByVal Revisi As String) As String
        Return "SELECT * From V_MKTTSPRevDt2 WHERE TransNmbr = " + QuotedStr(Nmbr) + " AND Revisi =" + Revisi
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
        Dim Result, ListSelectNmbr, ActionValue, msg As String
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
                        If (GVR.Cells(4).Text = "P") Or (GVR.Cells(4).Text = "G") Or (GVR.Cells(4).Text = "H") Then
                            ListSelectNmbr = GVR.Cells(2).Text + "|" + GVR.Cells(3).Text
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
                Session("SelectCommand") = "EXEC S_FormMKTTSPRev " + Result + ", " + QuotedStr(ViewState("UserId").ToString) + ""
                Session("ReportFile") = ".../../../Rpt/FormMKTTSPRev.frx"
                AttachScript("openprintdlg();", Page, Me.GetType)
            Else
                Status = CekStatus(ActionValue)
                ListSelectNmbr = ""
                msg = ""
                'GetListCommand(Status, GridView1, "3,2", ListSelectNmbr, Nmbr, lbStatus.Text)
                GetListCommand(Status, GridView1, "4,2,3", ListSelectNmbr, Nmbr, msg)
                If ListSelectNmbr = "" Then Exit Sub
                For j = 0 To (Nmbr.Length - 1)
                    If Nmbr(j) = "" Then
                        Exit For
                    Else
                        Result = ExecSPCommandGo(ActionValue, "S_MKTTSPRev", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                        If Trim(Result) <> "" Then
                            lbStatus.Text = lbStatus.Text + Result + " <br/>"

                        End If
                    End If
                Next
                BindData("TransNmbr+'|'+LTRIM(STR(Revisi)) in (" + ListSelectNmbr + ")")
            End If
        Catch ex As Exception
            lbStatus.Text = "Go Command Error : " + ex.ToString
        End Try
    End Sub

    Private Sub EnableHd(ByVal State As Boolean)
        Try
            tbNoPesan.Enabled = State
            'tbRemark.Enabled = State
            'ddlpph.Enabled = State

        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDt(ByVal Referens As String, ByVal Revisi As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt") = Nothing
            dt = SQLExecuteQuery(GetStringDt(Referens, Revisi), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt") = dt
            BindGridDt(dt, GridDt)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub
    Private Sub BindDataDt2(ByVal Nmbr As String, ByVal Revisi As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt2") = Nothing
            dt = SQLExecuteQuery(GetStringDt2(Nmbr, Revisi), ViewState("DBConnection").ToString).Tables(0)
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
                        lbStatus.Text = "BAP No " + TbUnit.Text + " has been already exist"
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
                Row("Revisi") = lblRevisi.Text
                Row("LuasTanah") = tbLuasTanah.Text
                Row("LuasBangunan") = tbLuasBangunan.Text
                Row("Price") = tbPrice.Text
                Row("PpnTotal") = tbPpnTotal.Text
                Row("PphTotal") = tbPphtotal.Text
                Row("AmountTotal") = tbTotalAmount.Text
                Row("TJ") = tbQtyTJ.Text
                Row("DP") = tbQtyDP.Text
                Row("SisaTagihan") = tbQtySisaBayar.Text
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If
                If CekExistData(ViewState("Dt"), "UnitCode", TbUnit.Text) Then
                    lbStatus.Text = "BAP NO " + TbUnit.Text + " has been already exist"
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("UnitCode") = TbUnit.Text
                dr("UnitName") = tbUnitName.Text
                dr("Revisi") = lblRevisi.Text("Revisi") = lblRevisi.Text
                dr("LuasTanah") = tbLuasTanah.Text
                dr("LuasBangunan") = tbLuasBangunan.Text
                dr("Price") = tbPrice.Text
                dr("PpnTotal") = tbPpnTotal.Text
                dr("PphTotal") = tbPphtotal.Text
                dr("AmountTotal") = tbTotalAmount.Text
                dr("TJ") = tbQtyTJ.Text
                dr("DP") = tbQtyDP.Text
                dr("SisaTagihan") = tbQtySisaBayar.Text
                dr("Remark") = tbRemarkDt.Text
                ViewState("Dt").Rows.Add(dr)

            End If
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
            BindGridDt(ViewState("Dt"), GridDt)
            StatusButtonSave(True)
            'CountTotalDt2()
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
                Row = ViewState("Dt2").Select("ItemNo+'|'+UnitCode= " + QuotedStr(lbItemNo.Text + "|" + lblUnit.Text))(0)
                Row.BeginEdit()
                Row("PayDate") = tbTempoDate.SelectedDate
                Row("UnitCode") = lblUnit.Text
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
                Row("Revisi") = lblRevisi.Text
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
                dr("UnitCode") = lblUnit.Text
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
                dr("Revisi") = lblRevisi.Text
                ViewState("Dt2").Rows.Add(dr)

            End If

            MovePanel(pnlEditDt2, pnlDt2)
            Dim drow As DataRow()
            drow = ViewState("Dt2").Select("UnitCode = " + QuotedStr(TrimStr(lblUnit.Text)))
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

            CountTotalDt()
            btnCancelDt2.Visible = True
            btnSaveDt2.Visible = True

            'EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
            ' BindGridDt(ViewState("Dt2"), GridDt2)
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
                tbCode.Text = GetAutoNmbr("PRA", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)


                SQLString = "INSERT INTO MKTTSPRevHd (TransNmbr, Revisi, Status,Transdate, NoPesanan, MasaAngsuran, Remark, UserPrep, DatePrep) " + _
                "SELECT '" + tbCode.Text + "', 0 ,'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', " + _
                QuotedStr(tbNoPesan.Text) + ", " + QuotedStr(tbAngsuran.Text) + ", " + _
                QuotedStr(tbRemark.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", getDate()"


            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM MKTTSPRevHd WHERE TransNmbr = " + QuotedStr(tbCode.Text) + " And Revisi = " + lblRevisi.Text, ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE MKTTSPRevHd SET Transdate = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "' " + _
                ",NoPesanan = " + QuotedStr(tbNoPesan.Text) + _
                ",MasaAngsuran =  " + QuotedStr(tbAngsuran.Text) + _
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
            Dim cmdSql As New SqlCommand(" SELECT TransNmbr,Revisi,UnitCode,LuasTanah,LuasBangunan,Price,PpnTotal,PphTotal,AmountTotal,TJ,DP,SisaTagihan,Remark FROM MKTTSPRevDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand


            Dim Dt As New DataTable("MKTTSPRevDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt

            'save dt2
            cmdSql = New SqlCommand("SELECT TransNmbr, Revisi, ItemNo, UnitCode, PayDate,Type, PayKav, Ppn, PpnValue, Pph, PphValue, AmountDt2, Remark, BankCode, NoGiro, AngsuranKe FROM MKTTSPRevDt2 WHERE TransNmbr  = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            dbcommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand
            Dim Dt2 As New DataTable("MKTTSPRevDt2")

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


    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try

            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, pnlDt, GridDt)
            newTrans()
            btnHome.Visible = False
            MultiView1.ActiveViewIndex = 0
            Menu1.Items.Item(0).Selected = True
            GridDt.Columns(0).Visible = True
            btnAddDt.Visible = True
            btnAddDtKe2.Visible = True
            btnSP.Visible = True
            EnableHd(True)
            GridDt.Columns(0).Visible = False

            tbCode.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
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
            BindDataDt("", 0)
            BindDataDt2("", 0)
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbCode.Text = ""
            tbDate.SelectedDate = ViewState("ServerDate") 'Today
            tbAngsuran.Text = ""
            tbNoPesan.Text = ""
            lblRevisi.Text = 0

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
            tbPrice.Text = 0
            tbPpnTotal.Text = 0
            tbPphtotal.Text = 0
            tbQtyTJ.Text = 0
            tbQtyDP.Text = 0
            tbQtySisaBayar.Text = 0
            tbTotalAmount.Text = 0
            tbRemarkDt.Text = ""

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
    End Sub



    Public Function GetNewItemNo()
        Dim Row As DataRow()
        Dim R As DataRow
        Dim MaxItem As Integer = 0
        Row = ViewState("Dt2").Select("UnitCode = " + QuotedStr(TrimStr(lblUnit.Text)))

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
            btnSaveDt2.Focus()
            ddlType_SelectedIndexChanged(Nothing, Nothing)
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

            If tbNoPesan.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("No Pesan must have value")
                tbNoPesan.Focus()
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
                    lbStatus.Text = "Process Must Have Value"
                    Return False
                End If

            Else
                If TbUnit.Text.Trim = "" Then
                    lbStatus.Text = "Unit No Must Have Value"
                    TbUnit.Focus()
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

                'If Dr("UnitCode").ToString = "" Then
                '    lbStatus.Text = MessageDlg("Unit Must Have Value")
                '    Return False
                'End If

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

                'If TbUnit.Text = "" Or tbNominal.Text = "0" Then
                '    lbStatus.Text = MessageDlg("Unit Must Have Value")
                '    TbUnit.Focus()
                '    Return False
                'End If

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
            FilterName = "Reference, Date, Status, Customer Name,  Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), Status, CustomerName, Remark"
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
                    ViewState("Revisi") = GVR.Cells(3).Text
                    ViewState("Status") = GVR.Cells(4).Text
                    GridDt.PageIndex = 0

                    FillTextBoxHd(ViewState("TransNmbr"), ViewState("Revisi"))

                    BindDataDt(ViewState("TransNmbr"), ViewState("Revisi"))
                    BindDataDt2(ViewState("TransNmbr"), ViewState("Revisi"))
                    EnableHd(False)
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, pnlDt, GridDt)
                    ModifyInput2(False, pnlInput, pnlDt2, GridDt2)
                    btnHome.Visible = True
                    MultiView1.ActiveViewIndex = 0
                    Menu1.Items.Item(0).Selected = True
                    GridDt.Columns(0).Visible = False


                ElseIf DDL.SelectedValue = "Edit" Then
                    If GVR.Cells(4).Text = "H" Or GVR.Cells(4).Text = "G" Then
                        CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        MovePanel(PnlHd, pnlInput)
                        ViewState("TransNmbr") = GVR.Cells(2).Text
                        ViewState("Revisi") = GVR.Cells(3).Text
                        GridDt.PageIndex = 0
                        BindDataDt(ViewState("TransNmbr"), ViewState("Revisi"))
                        BindDataDt2(ViewState("TransNmbr"), ViewState("Revisi"))

                        ViewState("StateHd") = "Edit"

                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        FillTextBoxHd(ViewState("TransNmbr"), ViewState("Revisi"))
                        ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
                        btnHome.Visible = False
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
                        MultiView1.ActiveViewIndex = 0
                        Menu1.Items.Item(0).Selected = True
                        EnableHd(True)
                        'btnSP.Visible = False
                        'btnAddDt.Visible = False
                        'btnAddDtKe2.Visible = False
                        GridDt.Columns(0).Visible = False


                    Else
                        lbStatus.Text = MessageDlg("Data must Hold or Get Approval to edit")
                        Exit Sub
                    End If
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

                    SqlString = "Declare @A VarChar(255) EXEC S_MKTTSPRevCreateRevisi " + QuotedStr(GVR.Cells(2).Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", @A OUT SELECT @A"
                    Result = SQLExecuteScalar(SqlString, ViewState("DBConnection"))
                    Result = Result.Replace("0", "")

                    If Trim(Result) <> "" Then
                        lbStatus.Text = MessageDlg("Create Revisi Pesanan ke ( " + Result + " ) Sucess")

                    Else
                        lbStatus.Text = MessageDlg(Result)
                    End If
                    'Else
                    '    Exit Sub
                    'End If
                    btnSearch_Click(Nothing, Nothing)

                ElseIf DDL.SelectedValue = "Print" Then
                    Try
                        CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If

                        Session("DBConnection") = ViewState("DBConnection")
                        Session("SelectCommand") = "EXEC S_FormMKTTSPRev '" + GVR.Cells(2).Text + "', " + QuotedStr(ViewState("UserId").ToString) + " "
                        Session("ReportFile") = ".../../../Rpt/FormMKTTSPRev.frx"
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

                lblUnit.Text = GVR.Cells(2).Text
                lblUnitName.Text = GVR.Cells(3).Text

                MultiView1.ActiveViewIndex = 1

                If ViewState("StateHd") = "View" Then
                    ModifyInput2(False, pnlInput, pnlDt2, GridDt2)
                Else
                    ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
                End If
                btnGetAngsuran.Visible = False
                'GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
                If ViewState("Dt2") Is Nothing Then
                    BindDataDt2(ViewState("TransNmbr"), ViewState("Revisi"))
                End If
                Dim drow As DataRow()
                drow = ViewState("Dt2").Select("UnitCode = " + QuotedStr(lblUnit.Text))
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
            Dim Cek As String
            GVR = GridDt2.Rows(e.RowIndex)

            Cek = SQLExecuteScalar("SELECT Count(UnitCode) FROM V_FINTandaTerimaDt WHERE UnitCode =  " + QuotedStr(lblUnit.Text) + " AND SPNo = " + QuotedStr(tbNoPesan.Text) + " AND ItemSP = " + QuotedStr(GVR.Cells(1).Text) + " ", ViewState("DBConnection").ToString)
            'lbStatus.Text = Cek
            If Cek <> 0 Then
                lbStatus.Text = MessageDlg("Delete failed " + GVR.Cells(3).Text + " Already use on tanda terima/kwitansi")
                Exit Sub
            End If

            dr = ViewState("Dt2").Select("ItemNo = " + QuotedStr(GVR.Cells(1).Text))
            dr(0).Delete()

            Dim drow As DataRow()
            drow = ViewState("Dt2").Select("UnitCode = " + QuotedStr(TrimStr(lblUnit.Text)))
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


    Protected Sub FillTextBoxHd(ByVal Nmbr As String, ByVal Revisi As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr) + " AND Revisi = " + Revisi, ViewState("DBConnection").ToString)
            tbCode.Text = Nmbr
            lblRevisi.Text = Revisi
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToText(tbNoPesan, Dt.Rows(0)("NoPesanan").ToString)
            BindToText(tbAngsuran, Dt.Rows(0)("MasaAngsuran").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)

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
                BindToText(tbPrice, Dr(0)("Price").ToString, ViewState("DigitHome"))
                BindToText(tbPpnTotal, Dr(0)("PpnTotal").ToString, ViewState("DigitHome"))
                BindToText(tbPphtotal, Dr(0)("PphTotal").ToString, ViewState("DigitHome"))
                BindToText(tbTotalAmount, Dr(0)("AmountTotal").ToString, ViewState("DigitHome"))
                BindToText(tbQtyTJ, Dr(0)("TJ").ToString, ViewState("DigitHome"))
                BindToText(tbQtyDP, Dr(0)("DP").ToString, ViewState("DigitHome"))
                BindToText(tbQtySisaBayar, Dr(0)("SisaTagihan").ToString, ViewState("DigitHome"))
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)

            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt2(ByVal FALoc As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt2").select("ItemNo+'|'+UnitCode = " + QuotedStr(FALoc))
            If Dr.Length > 0 Then

                lbItemNo.Text = Dr(0)("ItemNo").ToString
                lblAngsuran.Text = Dr(0)("Angsuranke").ToString
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
        Dim Cek As String
        Try
            GVR = GridDt2.Rows(e.NewEditIndex)

            Cek = SQLExecuteScalar("SELECT Count(UnitCode) FROM V_FINTandaTerimaDt WHERE UnitCode =  " + QuotedStr(lblUnit.Text) + " AND SPNo = " + QuotedStr(tbNoPesan.Text) + " AND ItemSP = " + QuotedStr(GVR.Cells(1).Text) + " ", ViewState("DBConnection").ToString)
            'lbStatus.Text = Cek
            If Cek <> 0 Then
                lbStatus.Text = MessageDlg("Edit failed " + GVR.Cells(3).Text + " Already use on tanda terima/kwitansi")
                Exit Sub
            End If

            FillTextBoxDt2(GVR.Cells(1).Text + "|" + lblUnit.Text)
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

    Protected Sub btnSP_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSP.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT TransNmbr,Status,TransDate,SPNo,CustomerName, MasaAngsuran FROM V_MKTTsphd WHERE Status = 'P'"
            ResultField = "TransNmbr,Status,TransDate,SPNo,CustomerName, MasaAngsuran"
            ViewState("Sender") = "btnSP"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchGrid();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Supp Error : " + ex.ToString
        End Try
    End Sub



    'Protected Sub btnSP_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSP.Click
    '    Dim ResultField, CriteriaField, sqlstring, ResultSame As String
    '    Try

    '        sqlstring = "SELECT * FROM V_GetPenawaran "

    '        Session("DBConnection") = ViewState("DBConnection")
    '        Session("filter") = sqlstring
    '        ResultField = "TransNmbr,MasaAngsuran,SystemPembayaran,Area, AreaName,FgSewa,UnitCode,UnitName,LuasTanah,LuasBangunan,KtinggianDPL,ArahKavling,Price,Disc,DiscValue,Dpp"
    '        CriteriaField = "TransNmbr,MasaAngsuran,SystemPembayaran,Area, AreaName,FgSewa,UnitCode,UnitName,LuasTanah,LuasBangunan,KtinggianDPL,ArahKavling,Price,Disc,DiscValue,Dpp"
    '        'Session("ClickSame") = "Bill_To"
    '        Session("Column") = ResultField.Split(",")
    '        Session("CriteriaField") = CriteriaField.Split(",")
    '        ResultSame = "TransNmbr"
    '        Session("ResultSame") = ResultSame.Split(",")
    '        ViewState("Sender") = "btnSP"
    '        AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
    '        'AttachScript("FindMultiDlg();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lbStatus.Text = "btn Customer Click Error : " + ex.ToString
    '    End Try
    'End Sub



    Protected Sub ddlType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlType.SelectedIndexChanged

        Dim Nilai As String

        Try

            If ddlType.SelectedValue = "AG" Then
                CountTotalRecordAngsuran()
                tbRemarkDt2.Text = ""

            ElseIf ddlType.SelectedValue = "DP" Then

                tbTempoDate.SelectedDate = DateAdd(DateInterval.Month, 2, tbDate.SelectedDate)
                lblAngsuran.Text = 0
                tbRemarkDt2.Text = ""
                Nilai = SQLExecuteScalar("SELECT PayKav FROM MKTTSPDt2 WHERE UnitCode = " + QuotedStr(lblUnit.Text) + " AND Type = 'DP'  AND TransNmbr = " + QuotedStr(tbNoPesan.Text), ViewState("DBConnection"))
                tbNominal.Text = Nilai
                AttachScript("setformatdt2();", Page, Me.GetType())
                tbRemarkDt2.Text = "Sesuai tanggal  PPJB / AJB Notaris, masksimal 2 bulan setelah tanggal surat pesanan ini"

            ElseIf ddlType.SelectedValue = "TJ" Then
                tbTempoDate.SelectedDate = tbDate.Text
                tbRemarkDt2.Text = ""
                Nilai = SQLExecuteScalar("SELECT PayKav FROM MKTTSPDt2 WHERE UnitCode =  " + QuotedStr(lblUnit.Text) + " AND Type = 'TJ'  AND TransNmbr = " + QuotedStr(tbNoPesan.Text), ViewState("DBConnection"))
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

            drow = ViewState("Dt").Select("UnitCode = " + QuotedStr(TrimStr(lblUnit.Text)))
            QtyTotal = 0
            If drow.Length > 0 Then
                havedetail = False
                For Each Dr2 In drow.CopyToDataTable.Rows
                    If Not Dr2.RowState = DataRowState.Deleted Then

                        qty = CFloat(Dr2("Price").ToString)
                        'lbStatus.Text = CFloat(Dr2("UnitCode").ToString)

                        'Exit Sub


                        drow = ViewState("Dt2").Select("UnitCode = " + QuotedStr(Dr2("UnitCode").ToString))


                        Dim drDtResult As DataRow
                        Dim ExistRowDT As DataRow()
                        Dim MaxItem As String
                        Dim DtPekerjaan As DataTable
                        Dim SQLString As String
                        Dim DP, TJ, SisaTagihan As Double
                        Dim DateAngsuran As Date
                        Dim ItemNo As Integer

                        UnitAG = lblUnit.Text + "|DP"
                        drow = ViewState("Dt2").Select("UnitCode+'|'+Type = " + QuotedStr(UnitAG))

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


                        UnitTJ = lblUnit.Text + "|TJ"
                        drow = ViewState("Dt2").Select("UnitCode+'|'+Type = " + QuotedStr(UnitTJ))
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

                        SisaTagihan = qty - Val(TJ + DP)
                        'lbStatus.Text = SisaTagihan
                        'Exit Sub


                        If DP = 0 Then
                            DateAngsuran = tbDate.SelectedDate
                        End If


                        'lbStatus.Text = SQLString
                        'Exit Sub

                        ' Delete Record yang typenya Angsuran
                        Dim dtee() As DataRow
                        dtee = ViewState("Dt2").Select("UnitCode+'|'+Type = " + QuotedStr(lblUnit.Text + "|" + "AG"))
                        For i = 0 To dtee.Length - 1  'For i = 0 To baris - 1
                            Dim del2() As DataRow
                            Dim GVR2 As GridViewRow
                            GVR2 = GridDt2.Rows(1)
                            del2 = ViewState("Dt2").Select("UnitCode+'|'+Type = " + QuotedStr(lblUnit.Text + "|" + "AG"))
                            del2(0).Delete()
                            'lbStatus.Text = GVR2.Cells(3).Text
                            'Exit Sub
                        Next

                        SQLString = "EXEC S_GetCicilan " + QuotedStr(lblUnit.Text) + ", " + QuotedStr(SisaTagihan) + ", " + QuotedStr(ViewState("PPN")) + ", " + QuotedStr(tbAngsuran.Text) + ", " + QuotedStr(DateAngsuran)
                        DtPekerjaan = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)

                        'Insert Angsuran
                        For Each drDtResult In DtPekerjaan.Rows
                            ExistRowDT = ViewState("Dt2").Select("ItemNo+'|'+UnitCode = " + QuotedStr(drDtResult("ItemNo".ToString) + "|" + lblUnit.Text))
                            If ExistRowDT.Count = 0 Then
                                Dim Dtdr As DataRow
                                Dtdr = ViewState("Dt2").NewRow
                                Dtdr("UnitCode") = lblUnit.Text
                                Dtdr("ItemNo") = drDtResult("ItemNo".ToString)
                                Dtdr("Revisi") = lblRevisi.Text
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
                                Row = ViewState("Dt2").Select("ItemNo+'|'+UnitCode= " + QuotedStr(drDtResult("ItemNo".ToString) + "|" + lblUnit.Text))(0)
                                Row.BeginEdit()
                                Row("UnitCode") = lblUnit.Text
                                Row("ItemNo") = drDtResult("ItemNo".ToString)
                                Row("Revisi") = lblRevisi.Text
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

                        drow = ViewState("Dt2").Select("UnitCode = " + QuotedStr(TrimStr(lblUnit.Text)))
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

                        CountTotalDt()
                        btnCancelDt2.Visible = True
                        btnSaveDt2.Visible = True

                        'EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
                        'BindGridDt(ViewState("Dt2"), GridDt2)
                        StatusButtonSave(True)
                        btnBackDt2ke2.Focus()



                    End If

                Next
            End If


        Catch ex As Exception
            lbStatus.Text = "btnangsuran Error : " + ex.ToString
        End Try
    End Sub


End Class
