Imports System.Data
Imports System.Data.SqlClient
'Imports System.Data.Odbc
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc

Partial Class TrTandaTerima
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "SELECT * FROM V_FinTandaTerimaHd "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Try
            If Not IsPostBack Then
                InitProperty()
                FillCombo(ddlType, "SELECT TypeCode, TypeName FROM V_TypeBayar ", True, "TypeCode", "TypeName", ViewState("DBConnection"))
                FillCombo(ddlCurr, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))

                SetInit()
                Session("AdvanceFilter") = ""
                'lbCount.Text = SQLExecuteScalar("SELECT COUNT(Invoice_No) FROM V_GetInvPosting WHERE TotalAmount>=0 ", ViewState("DBConnection").ToString)
            End If
            lbStatus.Text = ""
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If

            ' hasil dari search dialog
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnCust" Then
                    tbCustCode.Text = Session("Result")(0).ToString
                    tbCustName.Text = Session("Result")(1).ToString

                End If

                If ViewState("Sender") = "btnReceiptNo" Then
                    tbReceiptNo.Text = Session("Result")(0).ToString
                    tbNoVoucher.Text = Session("Result")(1).ToString
                    tbReceipt.Text = FormatNumber(Session("Result")(2).ToString, ViewState("DigitHome"))
                    CountTotalDt()

                End If



                If ViewState("Sender") = "btnSpNo" Then
                    tbSpNo.Text = Session("Result")(0).ToString
                    tbItemSp.Text = Session("Result")(1).ToString
                    ddlType.SelectedValue = Session("Result")(2).ToString
                    TbUnit.Text = Session("Result")(3).ToString
                    'tbUnitName.Text = Session("Result")(4).ToString
                    tbAmountSP.Text = FormatNumber(Session("Result")(5).ToString, ViewState("DigitHome"))
                    tbPPn.Text = FormatNumber(Session("Result")(6).ToString, ViewState("DigitHome"))
                    tbPpnValue.Text = FormatNumber(Session("Result")(7).ToString, ViewState("DigitHome"))
                    tbTotalAmount.Text = FormatNumber(Session("Result")(8).ToString, ViewState("DigitHome"))
                    tbTypeName.Text = Session("Result")(9).ToString
                    tbPph.Text = Session("Result")(10).ToString
                    tbPphValue.Text = FormatNumber(Session("Result")(11).ToString, ViewState("DigitHome"))

                End If

                'If ViewState("Sender") = "btnGetInv" Then
                '    Dim drResult As DataRow
                '    Dim MaxItem As String
                '    If IsNothing(Session("Result")) Then
                '        lbStatus.Text = "Session is empty"
                '        Exit Sub
                '    End If
                '    For Each drResult In Session("Result").Rows
                '        If CekExistData(ViewState("Dt"), "InvoiceNo", drResult("Invoice_No")) = False Then
                '            MaxItem = GetNewItemNo(ViewState("Dt"))
                '            'insert
                '            Dim dr As DataRow
                '            dr = ViewState("Dt").NewRow
                '            dr("ItemNo") = MaxItem
                '            dr("InvoiceNo") = drResult("Invoice_No")
                '            dr("InvoiceDate") = drResult("Invoice_Date")
                '            dr("PoNo") = drResult("Rfference_No")
                '            dr("Invoice") = drResult("Invoice")
                '            dr("Potongan") = drResult("Potongan")
                '            dr("Dpp") = drResult("Dpp")
                '            dr("Ppn") = drResult("Ppn")
                '            dr("PpnInvoice") = drResult("PPn_Invoice")
                '            dr("Pph") = drResult("Pph")
                '            dr("PphInvoice") = drResult("Pph_Invoice")
                '            dr("Remark") = drResult("Remark")
                '            dr("InvoiceType") = drResult("InvoiceType")
                '            dr("TotalAmount") = drResult("TotalAmount")
                '            ViewState("Dt").Rows.Add(dr)
                '        End If
                '    Next
                '    BindGridDt(ViewState("Dt"), GridDt)
                '    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                '    CountTotalDt()
                'End If

                Session("Result") = Nothing
                ViewState("Sender") = Nothing

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

        GridDt.Columns(4).Visible = False
        GridDt.Columns(5).Visible = False
        'Me.tbdpp.Attributes.Add("ReadOnly", "False")
        'Me.tbTotalAmount.Attributes.Add("ReadOnly", "True")
        Me.tbAmount.Attributes.Add("ReadOnly", "True")
        Me.tbPpnTotal.Attributes.Add("ReadOnly", "True")
        Me.tbReceipt.Attributes.Add("ReadOnly", "True")
        Me.tbTotalAmountSP.Attributes.Add("ReadOnly", "True")
        'Me.tbPpnValue.Attributes.Add("ReadOnly", "True")
        Me.tbPPn.Attributes.Add("ReadOnly", "True")
        Me.tbAmountSP.Attributes.Add("ReadOnly", "True")
        Me.tbSelisih.Attributes.Add("ReadOnly", "True")
        'Me.tbPph.Attributes.Add("ReadOnly", "True")
        Me.tbPphValue.Attributes.Add("ReadOnly", "True")
        Me.tbPphTotal.Attributes.Add("ReadOnly", "True")
        Me.tbPotongan.Attributes.Add("OnKeyDown", "return PressNumeric();")


        tbSelisih.Attributes.Add("OnBlur", "setformatfordt();")
        tbPotongan.Attributes.Add("OnBlur", "setformatfordt();")
        tbTotalAmount.Attributes.Add("OnBlur", "setformatfordt();")
        tbTotalAmountSP.Attributes.Add("OnBlur", "setformatfordt();")
        tbAmount.Attributes.Add("OnBlur", "setformatfordt();")
        tbPpnTotal.Attributes.Add("OnBlur", "setformatfordt();")
        tbReceipt.Attributes.Add("OnBlur", "setformatfordt();")



        tbAmountSP.Attributes.Add("OnBlur", "setformatdt2();")
        tbPPn.Attributes.Add("OnBlur", "setformatdt2();")
        tbPpnValue.Attributes.Add("OnBlur", "setformatdt2();")
        tbPph.Attributes.Add("OnBlur", "setformatdt2();")
        tbPphValue.Attributes.Add("OnBlur", "setformatdt2();")
        tbTotalAmount.Attributes.Add("OnBlur", "setformatdt2();")

        tbPpnValue.Attributes.Add("OnBlur", "setformatdtPpn2();")


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
        Return "SELECT * From V_FinTandaTerimaDt WHERE TransNmbr = " + QuotedStr(Nmbr)
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
                Session("SelectCommand") = "EXEC S_FNFormTandaTerima " + Result + ", " + QuotedStr(ViewState("UserId").ToString) + " "
                Session("ReportFile") = ".../../../Rpt/FormTandaTerima.frx"
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
                        Result = ExecSPCommandGo(ActionValue, "S_FNTandaTerima", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            btnCust.Visible = State
            tbCustCode.Enabled = State
            tbNoVoucher.Enabled = False
            tbReceiptNo.Enabled = False
            'tbAttn.Enabled = State
            btnExpand.Enabled = State

        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub EnableDt(ByVal State As Boolean)
        Try
            tbPotongan.Enabled = State
            TbUnit.Enabled = False
            tbUnitName.Enabled = False
            btnUnit.Visible = False
            tbPPn.Enabled = State
            tbPpnValue.Enabled = State
            tbTotalAmount.Enabled = State
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
                Row("Type") = ddlType.SelectedValue
                Row("TypeName") = tbTypeName.Text
                Row("SpNo") = tbSpNo.Text
                Row("UnitCode") = TbUnit.Text
                Row("UnitName") = tbUnitName.Text
                Row("ItemSp") = tbItemSp.Text
                Row("PayKav") = tbAmountSP.Text
                Row("Ppn") = tbPPn.Text
                Row("PpnValue") = tbPpnValue.Text
                Row("Ppn") = tbPPn.Text
                Row("PpnValue") = tbPpnValue.Text
                Row("Pph") = tbPph.Text
                Row("PphValue") = tbPphValue.Text 'Math.Round(CFloat(tbdpp.Text) * CFloat(tbPPn.Text) / 100, 2)
                Row("AmountPayKav") = tbTotalAmount.Text  '(CFloat(tbdpp.Text) + Row("PPnInvoice").ToString) - Row("PPhInvoice").ToString 'tbTotalAmount.Text
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()

            Else


                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If

                If CekExistData(ViewState("Dt"), "ItemNo", lbItemNo.Text) = True Then
                    lbStatus.Text = MessageDlg("Item No " + lbItemNo.Text + " has already been exist")
                    Exit Sub
                End If

                If CekExistData(ViewState("Dt"), "ItemSP+''+UnitCode", tbItemSp.Text + "" + TbUnit.Text) = True Then
                    lbStatus.Text = MessageDlg("Item Pesanan " + tbItemSp.Text + " & " + TbUnit.Text + " has already been exist")
                    Exit Sub
                End If



                'If ddlInvoiceType.SelectedValue = "EXPENSE" Or ddlInvoiceType.SelectedValue = "PB" Then
                '    If GetCountRecord(ViewState("Dt")) >= 1 Then
                '        lbStatus.Text = MessageDlg("Cannot insert more than one record.!!")
                '        MovePanel(pnlEditDt, pnlDt)
                '        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                '        BindGridDt(ViewState("Dt"), GridDt)
                '        Exit Sub
                '    End If
                'End If

                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("ItemNo") = CInt(lbItemNo.Text)
                dr("Type") = ddlType.SelectedValue
                dr("TypeName") = tbTypeName.Text
                dr("SpNo") = tbSpNo.Text
                dr("UnitCode") = TbUnit.Text
                dr("UnitName") = tbUnitName.Text
                dr("ItemSp") = tbItemSp.Text
                dr("PayKav") = tbAmountSP.Text
                dr("Ppn") = tbPPn.Text
                dr("PpnValue") = tbPpnValue.Text
                dr("Ppn") = tbPPn.Text
                dr("Pph") = tbPph.Text 'Math.Round(CFloat(tbdpp.Text) * CFloat(tbPPn.Text) / 100, 2)
                dr("PphValue") = tbPphValue.Text
                dr("AmountPayKav") = tbTotalAmount.Text '(CFloat(tbdpp.Text) + Row("PPnInvoice").ToString) - Row("PPhInvoice").ToString 'tbTotalAmount.Text
                dr("Remark") = tbRemarkDt.Text
                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            BindGridDt(ViewState("Dt"), GridDt)
            StatusButtonSave(True)
            CountTotalDt()
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
            'System.Threading.Thread.Sleep(7000)

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

                tbRef.Text = GetAutoNmbr("KWI", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO FinTandaTerimaHd (TransNmbr,Status, TransDate, ReceiptNo, CustomerCode, " + _
                "Currency, Rate, NoVoucher, AmountSP, TotalPpn, Potongan, TotalAmountSP, TotalReceipt, Selisih, TotalPPh, " + _
                "Remark, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbRef.Text) + ", 'H', '" + Format(tbDate.SelectedDate, "yyyy-MM-dd") + "', " + QuotedStr(tbReceiptNo.Text) + ", " + _
                QuotedStr(tbCustCode.Text) + ", " + QuotedStr(ddlCurr.Text) + ", " + QuotedStr(tbRate.Text) + "," + QuotedStr(tbNoVoucher.Text) + ", " + _
                QuotedStr(tbAmount.Text.Replace(",", "")) + ", " + QuotedStr(tbPpnTotal.Text.Replace(",", "")) + ", " + _
                QuotedStr(tbPotongan.Text.Replace(",", "")) + ", " + QuotedStr(tbTotalAmountSP.Text.Replace(",", "")) + "," + QuotedStr(tbReceipt.Text.Replace(",", "")) + ", " + _
                QuotedStr(tbSelisih.Text.Replace(",", "")) + ", " + QuotedStr(tbPphTotal.Text.Replace(",", "")) + ", " + _
                QuotedStr(tbRemark.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", getDate()"
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM FinTandaTerimaHd WHERE TransNmbr = " + QuotedStr(tbRef.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                CountTotalDt()
                SQLString = "UPDATE FinTandaTerimaHd SET CustomerCode = " + QuotedStr(tbCustCode.Text) + _
                ", Currency = " + QuotedStr(ddlCurr.SelectedValue) + _
                ", Rate = " + QuotedStr(tbRate.Text) + _
                ", NoVoucher = " + QuotedStr(tbNoVoucher.Text) + _
                ", ReceiptNo = " + QuotedStr(tbReceiptNo.Text) + _
                ", AmountSP = " + QuotedStr(tbAmount.Text.Replace(",", "")) + _
                ", TotalPpn = " + QuotedStr(tbPpnTotal.Text.Replace(",", "")) + _
                ", TotalPPh = " + QuotedStr(tbPphTotal.Text.Replace(",", "")) + _
                ", Potongan = " + QuotedStr(tbPotongan.Text.Replace(",", "")) + _
                ", TotalAmountSP = " + QuotedStr(tbTotalAmountSP.Text.Replace(",", "")) + _
                ", TotalReceipt = " + QuotedStr(tbReceipt.Text.Replace(",", "")) + _
                ", Selisih = " + QuotedStr(tbSelisih.Text.Replace(",", "")) + _
                ", Remark = " + QuotedStr(tbRemark.Text) + ", TransDate = '" + Format(tbDate.SelectedDate, "yyyy-MM-dd") + "', DatePrep = getDate()" + _
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
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, ItemNo, UnitCode,SPNo,Type,ItemSP,PayKav,Ppn,PpnValue,Pph,PphValue,AmountPayKav, Remark FROM FinTandaTerimaDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            'Dim param As SqlParameter
            '' Create the UpdateCommand.
            'Dim Update_Command = New SqlCommand( _
            '"UPDATE FinTandaTerimaDt SET ItemNo = @ItemNo, InvoiceNo = @InvoiceNo, " + _
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
            '    "DELETE FROM FinTandaTerimaDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND ItemNo = @ItemNo ", con)
            '' Add the parameters for the DeleteCommand.
            'param = Delete_Command.Parameters.Add("@ItemNo", SqlDbType.Int, 4, "ItemNo")
            'param.SourceVersion = DataRowVersion.Original
            'da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("FinTandaTerimaDt")

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


            'ChangeReport("Add", "Y", True, tbDate, tbRate, tbPPnNo, tbPPndate, tbPpnRate) 'ddlReport.SelectedValue
            'If ddlCurr.SelectedValue <> ViewState("Currency") Then 'And (ddlReport.SelectedValue = "Y")
            '    tbPpnRate.Text = FormatNumber(FindTaxRate(ddlCurr.SelectedValue, tbPPndate.SelectedValue, ViewState("DBConnection").ToString), ViewState("DigitCurr"))
            'Else
            '    tbPpnRate.Text = FormatNumber(1, ViewState("DigitCurr"))
            'End If
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
            tbCustCode.Text = ""
            tbCustName.Text = ""
            tbReceiptNo.Text = ""
            tbNoVoucher.Text = ""
           
            tbAmount.Text = 0
            tbTotalAmountSP.Text = 0
            tbPotongan.Text = 0
            tbPpnTotal.Text = 0
            tbPphTotal.Text = 0
            tbReceipt.Text = 0
            tbSelisih.Text = 0
            tbRemark.Text = ""
            ddlCurr.SelectedValue = ViewState("Currency")
            EnableHd(True)
            ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))


            'ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, ViewState("DBConnection").ToString)
            'ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
            'AttachScript("setformat();", Page, Me.GetType())
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub Cleardt()
        Try
            tbSpNo.Text = ""
            TbUnit.Text = ""
            tbUnitName.Text = ""
            tbItemSp.Text = ""
            tbAmountSP.Text = 0
            tbPPn.Text = ViewState("PPN")
            tbPpnValue.Text = 0
            tbPph.Text = 0
            tbPphValue.Text = 0
            tbTotalAmount.Text = 0
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

    Protected Sub btnCust_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCust.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT Customer_Code, Customer_Name FROM V_MsCustomer"
            ResultField = "Customer_Code, Customer_Name"
            ViewState("Sender") = "btnCust"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchGrid();", Page, Me.GetType())
            'AttachScript("OpenPopup();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Supp Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnSpNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSpNo.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT * FROM V_GetSP WHERE Type = " + QuotedStr(ddlType.SelectedValue)
            ResultField = "TransNmbr,ItemNo,Type,UnitCode,UnitName,PayKav,Ppn,PpnValue,AmountDt2, TypeName,Pph, PphValue"
            ViewState("Sender") = "btnSpNo"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchGrid();", Page, Me.GetType())
            'AttachScript("OpenPopup();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Supp Error : " + ex.ToString
        End Try
    End Sub



    Protected Sub btnReceiptNo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnReceiptNo.Click
        Dim ResultField, ResultSame As String 'ResultSame 
        Try

            Session("Result") = Nothing

            'Session("Filter") = "SELECT TransNmbr, Voucher_No, TotalReceiptForex FROM V_FNReceiptNonTradeHd WHERE Status = 'P' AND TransNmbr Not in (SELECT ReceiptNo FROM FINTandaTerimaHd WHERE Status <> 'D')"
            Session("Filter") = "SELECT * FROM V_GetReceipt WHERE CusCode  = " + QuotedStr(tbCustCode.Text)
            ResultField = "TransNmbr, Voucher_No, TotalReceiptForex"
            Session("Column") = ResultField.Split(",")
            ResultSame = "Supplier, SupplierType, Invoicetype"
            Session("ResultSame") = ResultSame.Split(",")
            ViewState("Sender") = "btnReceiptNo"
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchGrid();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn get Dt Error : " + ex.ToString
        End Try
    End Sub



    'Protected Sub tbCustCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCustCode.TextChanged
    '    Dim Dr As DataRow
    '    Try
    '        Dr = FindMaster("Supplier", tbCustCode.Text, ViewState("DBConnection").ToString)
    '        If Not Dr Is Nothing Then
    '            tbCustCode.Text = Dr("Supplier_Code")
    '            tbCustName.Text = Dr("Supplier_Name")
    '            tbSuppType.Text = Dr("Supplier_Type")
    '        Else
    '            tbCustCode.Text = ""
    '            tbCustName.Text = ""
    '            tbSuppType.Text = ""
    '            tbAttn.Text = ""
    '        End If
    '        'ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, ViewState("DBConnection").ToString)
    '        'ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
    '        'AttachScript("setformat();", Page, Me.GetType())
    '        tbCustCode.Focus()
    '    Catch ex As Exception
    '        Throw New Exception("tb SuppCode Error : " + ex.ToString)
    '    End Try
    'End Sub


    Private Sub CountTotalDt()
        Dim Amount, Ppn, TotalAmount, Pph As Double
        Dim Dr As DataRow
        Try
            Amount = 0
            Ppn = 0
            TotalAmount = 0
            Pph = 0
            For Each Dr In ViewState("Dt").Rows
                If Not Dr.RowState = DataRowState.Deleted Then
                    Amount = Amount + CFloat(Dr("PayKav").ToString)
                    Ppn = Ppn + CFloat(Dr("PpnValue").ToString)
                    Pph = Pph + CFloat(Dr("PphValue").ToString)
                    TotalAmount = TotalAmount + CFloat(Dr("AmountPayKav").ToString)

                End If
            Next

            tbAmount.Text = FormatNumber(Amount, ViewState("DigitHome"))
            tbPpnTotal.Text = FormatNumber(Ppn, ViewState("DigitHome"))
            tbPphTotal.Text = FormatNumber(Pph, ViewState("DigitHome"))
            tbTotalAmountSP.Text = FormatNumber(TotalAmount, ViewState("DigitHome"))

            tbSelisih.Text = FormatNumber(CFloat(tbTotalAmountSP.Text) - CFloat(tbPotongan.Text) - CFloat(tbReceipt.Text), ViewState("DigitHome"))



        Catch ex As Exception
            Throw New Exception("Count Total Dt Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt.Click, btnAddDt2.Click
        Try
            Cleardt()

            If tbCustCode.Text = "" Then
                lbStatus.Text = MessageDlg("Customer Must have value")
                Exit Sub
            End If

            If tbReceiptNo.Text = "" Then
                lbStatus.Text = MessageDlg("Receipt No Must have value")
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

            If tbCustCode.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Supplier must have value")
                btnCust.Focus()
                Return False
            End If

            If tbSelisih.Text <> 0 Then
                lbStatus.Text = MessageDlg(" Total Selisih must be 0")
                tbSelisih.Focus()
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
                'If Dr.RowState = DataRowState.Deleted Then
                '    Return True
                'End If
                'If Dr("Account").ToString = "" Then
                '    lbStatus.Text = MessageDlg("Account Must Have Value")
                '    Return False
                'End If
                'If Dr("FgSubled").ToString <> "N" And TrimStr(Dr("SubLed").ToString) = "" Then
                '    lbStatus.Text = MessageDlg("SubLed Must Have Value")
                '    Return False
                'End If
                'If Dr("FgCostCtr").ToString = "Y" And Dr("CostCtr").ToString = "" Then
                '    lbStatus.Text = MessageDlg("Cost Center Must Have Value")
                '    Return False
                'End If
                'If Dr("Qty").ToString = "0" Or Dr("Qty").ToString = "" Then
                '    lbStatus.Text = MessageDlg("Qty Must Have Value")
                '    Return False
                'End If
                'If Dr("PriceForex").ToString = "0" Or Dr("PriceForex").ToString = "" Then
                '    lbStatus.Text = MessageDlg("Price Forex Must Have Value")
                '    Return False
                'End If
                'If Dr("AmountForex").ToString = "0" Or Dr("AmountForex").ToString = "" Then
                '    lbStatus.Text = MessageDlg("Amount Forex Must Have Value")
                '    Return False
                'End If
            Else

                If tbSpNo.Text = "" Then
                    lbStatus.Text = MessageDlg("No Pesanan Must Have Value")
                    tbSpNo.Focus()
                    Return False
                End If
                If tbItemSp.Text = "" Then
                    lbStatus.Text = MessageDlg("Item Pesanan Must Have Value")
                    tbItemSp.Focus()
                    Return False
                End If

                If tbTotalAmount.Text = "" Or tbTotalAmount.Text = 0 Then
                    lbStatus.Text = MessageDlg("Amount Must Have Value")
                    tbTotalAmount.Focus()
                    Return False
                End If

                'If ddlInvoiceType.SelectedValue = "Invoice" Then
                '    Dim cekInvoice As String
                '    cekInvoice = SQLExecuteScalar("Select Invoice FROM V_GetInvPosting WHERE Invoice_No = " + QuotedStr(tbInvoiceNo.Text), ViewState("DBConnection").ToString)
                '    'lbStatus.Text = cekInvoice
                '    'Exit Function
                '    If CFloat(tbInvoice.Text.Replace(",", "")) > CFloat(cekInvoice) Then
                '        lbStatus.Text = MessageDlg("Invoice Approval Cannot grether more than Total Invoice ")
                '        Exit Function
                '    End If

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
            FDateValue = "TransDate, SuppInvDate"
            FilterName = "Reference, Date, Status, Supplier Code, Customer Name,  Remark"
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

                        Session("SelectCommand") = "EXEC S_FNFormTandaTerima '''" + GVR.Cells(2).Text + "''', " + QuotedStr(ViewState("UserId").ToString) + " "
                        Session("ReportFile") = ".../../../Rpt/FormTandaTerima.frx"
                        Session("DBConnection") = ViewState("DBConnection")
                        AttachScript("openprintdlg();", Page, Me.GetType)

                        'Dim URL As String
                        'URL = ResolveUrl("../../Rpt/PrintForm.Aspx")
                        'Dim s As String = "window.open('" & URL & "', '_blank');"
                        'Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)
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
            'CountTotalDt()f
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
            CountTotalDt()
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
            'CountTotalDt()
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub lbCurr_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCurr.Click
    '    Try
    '        ViewState("InputCurrency") = "Y"
    '        AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsCurrency')();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lbStatus.Text = "lb Currency Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub lbCust_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCust.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsCustomer')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb Supplier Error : " + ex.ToString
        End Try
    End Sub


    'Dim BaseForex As Decimal = 0

    '' untuk tampilkan data total di grid
    'Protected Sub GridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt.RowDataBound
    '    Try
    '        If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "ItemNo")) Then
    '            If e.Row.RowType = DataControlRowType.DataRow Then
    '                '' add the UnitPrice and QuantityTotal to the running total variables
    '                BaseForex += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "AmountForex"))
    '            ElseIf e.Row.RowType = DataControlRowType.Footer Then
    '                tbBaseForex.Text = FormatNumber(BaseForex, ViewState("DigitCurr"))
    '                'AttachScript("BaseDiscPPnTotal(" + Me.tbBaseForex.ClientID + "," + tbDisc.ClientID + "," + tbDiscForex.ClientID + "," + Me.tbPPN.ClientID + "," + Me.tbPPNForex.ClientID + "," + Me.tbTotalForex.ClientID + "); setformat();", Page, Me.GetType())
    '            End If
    '        End If
    '        TotalHd()
    '    Catch ex As Exception
    '        lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            tbRef.Text = Nmbr

            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToText(tbCustCode, Dt.Rows(0)("CustomerCode").ToString)
            BindToText(tbCustName, Dt.Rows(0)("CustomerName").ToString)
            BindToText(tbReceiptNo, Dt.Rows(0)("ReceiptNo").ToString)
            BindToText(tbNoVoucher, Dt.Rows(0)("NoVoucher").ToString)
            BindToText(tbRate, Dt.Rows(0)("Rate").ToString)
            BindToDropList(ddlCurr, Dt.Rows(0)("Currency").ToString)
            BindToDropList(ddlCurr, Dt.Rows(0)("Currency").ToString)

            BindToText(tbAmount, Dt.Rows(0)("AmountSP").ToString, ViewState("DigitCurr"))
            BindToText(tbPpnTotal, Dt.Rows(0)("TotalPpn").ToString, ViewState("DigitCurr"))
            BindToText(tbPotongan, Dt.Rows(0)("Potongan").ToString, ViewState("DigitCurr"))
            BindToText(tbPphTotal, Dt.Rows(0)("TotalPph").ToString, ViewState("DigitCurr"))
            BindToText(tbTotalAmountSP, Dt.Rows(0)("totalAmountSP").ToString, ViewState("DigitCurr"))
            BindToText(tbReceipt, Dt.Rows(0)("TotalReceipt").ToString, ViewState("DigitCurr"))
            BindToText(tbSelisih, Dt.Rows(0)("Selisih").ToString, ViewState("DigitCurr"))
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
                BindToDropList(ddlType, Dr(0)("Type").ToString)
                BindToText(TbUnit, Dr(0)("UnitCode").ToString)
                BindToText(tbUnitName, Dr(0)("UnitName").ToString)
                BindToText(tbSpNo, Dr(0)("SpNo").ToString)
                BindToText(tbItemSp, Dr(0)("ItemSp").ToString)
                BindToText(tbTypeName, Dr(0)("TypeName").ToString)
                BindToText(tbAmountSP, Dr(0)("PayKav").ToString, ViewState("DigitHome"))
                BindToText(tbPPn, Dr(0)("Ppn").ToString, ViewState("DigitHome"))
                BindToText(tbPpnValue, Dr(0)("PpnValue").ToString, ViewState("DigitHome"))
                BindToText(tbPph, Dr(0)("Pph").ToString, ViewState("DigitHome"))
                BindToText(tbPphValue, Dr(0)("PphValue").ToString, ViewState("DigitHome"))
                BindToText(tbTotalAmount, Dr(0)("AmountPayKav").ToString, ViewState("DigitHome"))
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


End Class
