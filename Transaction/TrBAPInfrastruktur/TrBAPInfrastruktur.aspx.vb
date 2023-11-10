Imports System.Data
Imports System.Data.SqlClient
'Imports System.Data.Odbc
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc

Partial Class BAPInfrastruktur
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select * From V_PRCBAPInfHD "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                'FillCombo(ddlLokasi, "SELECT AreaCode, AreaName FROM V_MsArea ", True, "AreaCode", "AreaName", ViewState("DBConnection"))
                SetInit()
                Session("AdvanceFilter") = ""
                lbCount.Text = SQLExecuteScalar("SELECT COUNT(No_Spk) FROM V_GetPemenang ", ViewState("DBConnection").ToString)
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

                If ViewState("Sender") = "btnSPK" Then
                    Dim drResult As DataRow
                    Dim MaxItem As String
                    Dim DtPekerjaan As DataTable
                    Dim SQLString As String

                    tbNoSPK.Text = Session("Result")(0).ToString
                    tbSuppCode.Text = Session("Result")(1).ToString
                    tbSuppName.Text = Session("Result")(2).ToString
                    tbPaketPekerjaan.Text = Session("Result")(3).ToString.Replace("amp;", "")
                    'Insert To Detail
                    SQLString = "EXEC S_GetPemenang " + QuotedStr(tbNoSPK.Text)
                    DtPekerjaan = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)

                    For Each drResult In DtPekerjaan.Rows
                        If CekExistData(ViewState("Dt"), "ItemNo", drResult("ItemNo")) = False Then
                            MaxItem = GetNewItemNo(ViewState("Dt"))
                            'insert
                            Dim dr As DataRow
                            dr = ViewState("Dt").NewRow
                            dr("ItemNo") = drResult("ItemNo")
                            dr("UraianPekerjaan") = drResult("UraianPekerjaan")
                            dr("Luas") = drResult("Luas")
                            dr("Biaya") = drResult("NilaiProject")
                            dr("BAPPersen") = drResult("BAPPersen")
                            dr("BAP") = drResult("BAP")
                            dr("BAPSebelumPersen") = drResult("BAPSebelumPersen")
                            dr("BAPSebelum") = drResult("BAPSebelum")
                            dr("TagihanBAPPersen") = drResult("TagihanBAPPersen")
                            dr("TagihanBAP") = drResult("TagihanBAP")
                            dr("SisaBAP") = drResult("SisaBAP")
                            dr("Remark") = drResult("Remark")
                            ViewState("Dt").Rows.Add(dr)
                        End If
                    Next
                    BindGridDt(ViewState("Dt"), GridDt)
                    'EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    CountTotalDt()
                End If



                If ViewState("Sender") = "btnOut" Then
                    BtnAdd_Click(Nothing, Nothing)
                    Dim drResult As DataRow
                    Dim MaxItem As String
                    Dim DtPekerjaan As DataTable
                    Dim SQLString As String

                    tbNoSPK.Text = Session("Result")(0).ToString
                    tbSuppCode.Text = Session("Result")(1).ToString
                    tbSuppName.Text = Session("Result")(2).ToString
                    tbPaketPekerjaan.Text = Session("Result")(3).ToString.Replace("amp;", "")
                    'Insert To Detail
                    SQLString = "EXEC S_GetPemenang " + QuotedStr(tbNoSPK.Text)
                    DtPekerjaan = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)

                    For Each drResult In DtPekerjaan.Rows
                        If CekExistData(ViewState("Dt"), "ItemNo", drResult("ItemNo")) = False Then
                            MaxItem = GetNewItemNo(ViewState("Dt"))
                            'insert
                            Dim dr As DataRow
                            dr = ViewState("Dt").NewRow
                            dr("ItemNo") = drResult("ItemNo")
                            dr("UraianPekerjaan") = drResult("UraianPekerjaan")
                            dr("Luas") = drResult("Luas")
                            dr("Biaya") = drResult("NilaiProject")
                            dr("BAPPersen") = drResult("BAPPersen")
                            dr("BAP") = drResult("BAP")
                            dr("BAPSebelumPersen") = drResult("BAPSebelumPersen")
                            dr("BAPSebelum") = drResult("BAPSebelum")
                            dr("TagihanBAPPersen") = drResult("TagihanBAPPersen")
                            dr("TagihanBAP") = drResult("TagihanBAP")
                            dr("SisaBAP") = drResult("SisaBAP")
                            dr("Remark") = drResult("Remark")
                            ViewState("Dt").Rows.Add(dr)
                        End If
                    Next
                    BindGridDt(ViewState("Dt"), GridDt)
                    ' EnableHd(GetCountRecord(ViewState("Dt")) = 0)
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


        tbBiaya.Attributes.Add("OnBlur", "setformatfordt();")
        tbBAPPersen.Attributes.Add("OnBlur", "setformatfordt();")
        tbBAP.Attributes.Add("OnBlur", "setformatfordt();")
        tbBAPSebelumPersen.Attributes.Add("OnBlur", "setformatfordt();")
        tbBAPSebelum.Attributes.Add("OnBlur", "setformatfordt();")
        tbBAPnowPersen.Attributes.Add("OnBlur", "setformatfordt();")
        tbBAPnowPersen.Attributes.Add("OnBlur", "setformatfordt();")
        tbSisaBAP.Attributes.Add("OnBlur", "setformatfordt();")

        Me.tbLuas.Attributes.Add("ReadOnly", "True")
        Me.tbBiaya.Attributes.Add("ReadOnly", "True")
        Me.tbBAP.Attributes.Add("ReadOnly", "True")
        Me.tbBAPSebelum.Attributes.Add("ReadOnly", "True")
        Me.tbBAPSebelumPersen.Attributes.Add("ReadOnly", "True")
        Me.tbBAPnowPersen.Attributes.Add("ReadOnly", "True")
        Me.tbBAPnow.Attributes.Add("ReadOnly", "True")
        Me.tbSisaBAP.Attributes.Add("ReadOnly", "True")
        Me.tbUraian.Attributes.Add("ReadOnly", "True")

        'Me.tbLuas.Attributes.Add("ReadOnly", "True")
        'Me.tbBiaya.Attributes.Add("ReadOnly", "True")

        Me.tbBAPPersen.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbBAP.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbBAPSebelum.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbBAPSebelumPersen.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbBAPnowPersen.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbBAPnow.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbSisaBAP.Attributes.Add("OnKeyDown", "return PressNumeric();")



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
        Return "SELECT * From V_PRCBAPInfDt WHERE TransNmbr = " + QuotedStr(Nmbr)
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
                Session("SelectCommand") = "EXEC S_FNFormBapINfrastruktur " + Result + "," + QuotedStr(ViewState("UserId"))
                Session("ReportFile") = ".../../../Rpt/FormBapINfrastruktur.frx"
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
                        Result = ExecSPCommandGo(ActionValue, "S_PRCBAPInf", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            tbSuppCode.Enabled = State
            btnSupp.Visible = False
            'BtnNoSPK.Enabled = State
            tbNoSPK.Enabled = False
            tbTotBayarBAP.Enabled = False
            tbTotNilai.Enabled = False
            tbTotSisaBAP.Enabled = False
            tbBAPsdSaatIni.Enabled = False
            tbBAPSebelumnya.Enabled = False
            tbPaketPekerjaan.Enabled = False

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
            tbBAPPersen.Enabled = State
            tbBAP.Enabled = State
            tbBAPSebelumPersen.Enabled = State
            tbBAPSebelum.Enabled = State
            tbBAPnowPersen.Enabled = State
            tbBAPnow.Enabled = State
            tbSisaBAP.Enabled = State
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
                Row("BAPPersen") = tbBAPPersen.Text
                Row("BAP") = tbBAP.Text
                Row("BAPSebelumPersen") = tbBAPSebelumPersen.Text
                Row("BAPSebelum") = tbBAPSebelum.Text
                Row("TagihanBAPPersen") = tbBAPnowPersen.Text
                Row("TagihanBAP") = tbBAPnow.Text
                Row("SisaBAP") = tbSisaBAP.Text
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
                dr("BAPPersen") = tbBAPPersen.Text
                dr("BAP") = tbBAP.Text
                dr("BAPSebelumPersen") = tbBAPSebelumPersen.Text
                dr("BAPSebelum") = tbBAPSebelum.Text
                dr("TagihanBAPPersen") = tbBAPnowPersen.Text
                dr("TagihanBAP") = tbBAPnow.Text
                dr("SisaBAP") = tbSisaBAP.Text
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
        Dim CekMenu As String
        Try

            CekMenu = CheckMenuLevel("Insert", ViewState("MenuLevel").Rows(0))
            If CekMenu <> "" Then
                lbStatus.Text = CekMenu
                Exit Sub
            End If

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

                tbRef.Text = GetAutoNmbr("BAP", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)


                SQLString = "INSERT INTO PRCBAPInfHd (TransNmbr,Status, TransDate,No_SPK,Paket_Pekerjaan,SuppCode, " + _
                "TotalNilai,TotalBAPsdSaatIni,TotalBAPSebelumnya,TotalBAP,TotalSisaBAP, " + _
                "Remark, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbRef.Text) + ", 'H', '" + Format(tbDate.SelectedDate, "yyyy-MM-dd") + "', " + _
                QuotedStr(tbNoSPK.Text) + ", " + QuotedStr(tbPaketPekerjaan.Text) + ", " + _
                QuotedStr(tbSuppCode.Text) + "," + _
                QuotedStr(tbTotNilai.Text.Replace(",", "")) + ", " + QuotedStr(tbBAPsdSaatIni.Text.Replace(",", "")) + ", " + _
                QuotedStr(tbBAPSebelumnya.Text.Replace(",", "")) + ", " + QuotedStr(tbTotBayarBAP.Text.Replace(",", "")) + "," + QuotedStr(tbTotSisaBAP.Text.Replace(",", "")) + ", " + _
                QuotedStr(tbRemark.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", getDate()"


            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM PRCBAPInfHd WHERE TransNmbr = " + QuotedStr(tbRef.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If

                SQLString = "UPDATE PRCBAPInfHd SET SuppCode = " + QuotedStr(tbSuppCode.Text) + ", No_SPK = " + QuotedStr(tbNoSPK.Text) + _
                ", Paket_Pekerjaan = " + QuotedStr(tbPaketPekerjaan.Text) + _
                ", TotalNilai = " + QuotedStr(tbTotNilai.Text.Replace(",", "")) + _
                ", TotalBAPsdSaatIni = " + QuotedStr(tbBAPsdSaatIni.Text.Replace(",", "")) + _
                ", TotalBAPSebelumnya = " + QuotedStr(tbBAPSebelumnya.Text.Replace(",", "")) + _
                ", TotalBAP = " + QuotedStr(tbTotBayarBAP.Text.Replace(",", "")) + _
                ", TotalSisaBAP = " + QuotedStr(tbTotSisaBAP.Text.Replace(",", "")) + _
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
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, ItemNo, UraianPekerjaan, Luas, Biaya,BAPPersen,BAP,BAPSebelumPersen,BAPSebelum,TagihanBAPPersen,TagihanBAP,SisaBAP,  Remark FROM PRCBAPInfDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            'Dim param As SqlParameter
            '' Create the UpdateCommand.
            'Dim Update_Command = New SqlCommand( _
            '"UPDATE PRCBAPInfDt SET ItemNo = @ItemNo, InvoiceNo = @InvoiceNo, " + _
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
            '    "DELETE FROM PRCBAPInfDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND ItemNo = @ItemNo ", con)
            '' Add the parameters for the DeleteCommand.
            'param = Delete_Command.Parameters.Add("@ItemNo", SqlDbType.Int, 4, "ItemNo")
            'param.SourceVersion = DataRowVersion.Original
            'da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("PRCBAPInfDt")

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


            If Val(tbTotNilai.Text) = Val(tbBAPsdSaatIni.Text) Or tbTotSisaBAP.Text = 0 Then
                UpdateSPK = SQLExecuteScalar("Update PRCPemenangHD Set FgDoneBAP = 'Y' WHERE TransNmbr = " + QuotedStr(tbNoSPK.Text), ViewState("DBConnection").ToString)
                UpdateSPK = SQLExecuteScalar("Update PRCSPKBeginHD Set FgDoneBAP = 'Y' WHERE TransNmbr = " + QuotedStr(tbNoSPK.Text), ViewState("DBConnection").ToString)
            End If


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
            tbSuppCode.Text = ""
            tbSuppName.Text = ""
            tbNoSPK.Text = ""
            tbPaketPekerjaan.Text = ""
            tbTotNilai.Text = 0
            tbBAPsdSaatIni.Text = 0
            tbBAPSebelumnya.Text = 0
            tbTotBayarBAP.Text = 0
            tbTotSisaBAP.Text = 0
            tbRemark.Text = ""
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
            tbBAPPersen.Text = 0
            tbBAP.Text = 0
            tbBAPSebelumPersen.Text = 0
            tbBAPSebelum.Text = 0
            tbBAPnowPersen.Text = 0
            tbBAPnow.Text = 0
            tbSisaBAP.Text = 0
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

    Protected Sub BtnNoSPK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnNoSPK.Click
        Dim ResultField As String
        Try
            Session("filter") = "select * from V_GetPemenang "
            ResultField = "No_SPK,SuppCode, Supplier_Name, Paket_Pekerjaan "
            ViewState("Sender") = "btnSPK"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchGrid();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Supp Error : " + ex.ToString
        End Try
    End Sub



    Protected Sub lbCount_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCount.Click
        Dim ResultField, ResultSame As String 'ResultSame 
        Dim CekMenu As String
        Try

            CekMenu = CheckMenuLevel("Insert", ViewState("MenuLevel").Rows(0))
            If CekMenu <> "" Then
                lbStatus.Text = CekMenu
                Exit Sub
            End If


            Session("filter") = "select * from V_GetPemenang "
            ResultField = "No_SPK,SuppCode, Supplier_Name, Paket_Pekerjaan "
            ViewState("Sender") = "btnOut"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchGrid();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn get Dt Error : " + ex.ToString
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
            tbSuppCode.Focus()
        Catch ex As Exception
            Throw New Exception("tb SuppCode Error : " + ex.ToString)
        End Try
    End Sub



    Private Sub CountTotalDt()
        Dim TotalBiaya, BAP, BAPSebelum, BAPnow, SisaBAP As Double
        Dim Dr As DataRow
        Try

            TotalBiaya = 0
            BAP = 0
            BAPSebelum = 0
            BAPnow = 0
            SisaBAP = 0
            For Each Dr In ViewState("Dt").Rows
                If Not Dr.RowState = DataRowState.Deleted Then
                    TotalBiaya = TotalBiaya + CFloat(Dr("Biaya").ToString)
                    BAP = BAP + CFloat(Dr("BAP").ToString)
                    BAPSebelum = BAPSebelum + CFloat(Dr("BAPSebelum").ToString)
                    BAPnow = BAPnow + CFloat(Dr("TagihanBAP").ToString)
                    SisaBAP = SisaBAP + CFloat(Dr("SisaBAP").ToString)
                End If
            Next
            tbTotNilai.Text = FormatNumber(TotalBiaya, ViewState("DigitHome"))
            tbBAPsdSaatIni.Text = FormatNumber(BAP, ViewState("DigitHome"))
            tbBAPSebelumnya.Text = FormatNumber(BAPSebelum, ViewState("DigitHome"))
            tbTotBayarBAP.Text = FormatNumber(BAPnow, ViewState("DigitHome"))
            tbTotSisaBAP.Text = FormatNumber(SisaBAP, ViewState("DigitHome"))

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

            'If CInt(Session(Request.QueryString("KeyId"))("Year")) <> Year(tbDate.SelectedValue) Or CInt(Session(Request.QueryString("KeyId"))("Period")) <> Month(tbDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(Session(Request.QueryString("KeyId"))("Period")) + " " + Session(Request.QueryString("KeyId"))("Year").ToString.Trim)
            '    Return False
            'End If

            If tbSuppCode.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Supplier must have value")
                btnSupp.Focus()
                Return False
            End If

            If tbNoSPK.Text = "" Then
                lbStatus.Text = MessageDlg("SPK No must have value")
                tbNoSPK.Focus()
                Return False
            End If


            'CekDate = SQLExecuteScalar("SELECT TOP 1 A.TransDate FROM PRCBAPInfHD A INNER JOIN PRCBAPInfDt B ON A.TransNmbr = B.TransNmbr WHERE A.No_SPK = " + QuotedStr(tbNoSPK.Text) + " AND A.Status <> 'D' ORder By A.TransDate DESC ", ViewState("DBConnection").ToString)

            'If CekDate > tbDate.SelectedDate Then
            '    lbStatus.Text = MessageDlg("BAP Date must be bigger than previous BAP Date")
            '    tbDate.Focus()
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

                If Dr("Biaya").ToString = "0" Or Dr("Biaya").ToString = "" Then
                    lbStatus.Text = MessageDlg("Biaya Must Have Value")
                    Return False
                End If

                'If Dr("BAPPersen").ToString = 0 Or Dr("BAPPersen").ToString = "" Then
                '    lbStatus.Text = "Qty Output Must Have Value"
                '    Return False
                'End If

                If Dr("BAPPersen").ToString > 100 Then
                    lbStatus.Text = MessageDlg("BAP Persen cannot greter more than 100%")
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

                If tbBAPPersen.Text > 100 Then
                    lbStatus.Text = MessageDlg("BAP Persen cannot greter more than 100%")
                    tbBiaya.Focus()
                    Return False
                End If

                If tbBAPPersen.Text > 0 Then
                    If Val(tbBAPPersen.Text) < Val(tbBAPSebelumPersen.Text) Then
                        lbStatus.Text = MessageDlg("BAP s/d Saat Ini, Terlalu Kecil Dari BAP Sebelumnya !! ")
                        tbBAPnow.Focus()
                        Return False
                    End If


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
                        Session("SelectCommand") = "EXEC S_FNFormBapINfrastruktur '''" + GVR.Cells(2).Text + "'''," + QuotedStr(ViewState("UserId"))
                        Session("ReportFile") = ".../../../Rpt/FormBapINfrastruktur.frx"
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

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            tbRef.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToText(tbSuppCode, Dt.Rows(0)("SuppCode").ToString)
            BindToText(tbSuppName, Dt.Rows(0)("Supplier_Name").ToString)
            BindToText(tbNoSPK, Dt.Rows(0)("No_SPK").ToString)
            BindToText(tbPaketPekerjaan, Dt.Rows(0)("Paket_Pekerjaan").ToString)
            BindToText(tbTotNilai, Dt.Rows(0)("TotalNilai").ToString, ViewState("DigitCurr"))
            BindToText(tbBAPsdSaatIni, Dt.Rows(0)("TotalBAPsdSaatIni").ToString, ViewState("DigitCurr"))
            BindToText(tbBAPSebelumnya, Dt.Rows(0)("TotalBAPSebelumnya").ToString, ViewState("DigitCurr"))
            BindToText(tbTotBayarBAP, Dt.Rows(0)("TotalBAP").ToString, ViewState("DigitCurr"))
            BindToText(tbTotSisaBAP, Dt.Rows(0)("TotalSisaBAP").ToString, ViewState("DigitCurr"))
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
                BindToText(tbLuas, Dr(0)("Luas").ToString, ViewState("DigitHome"))
                BindToText(tbBiaya, Dr(0)("Biaya").ToString, ViewState("DigitHome"))
                BindToText(tbBAPPersen, Dr(0)("BAPPersen").ToString, ViewState("DigitHome"))
                BindToText(tbBAP, Dr(0)("BAP").ToString, ViewState("DigitHome"))
                BindToText(tbBAPSebelumPersen, Dr(0)("BAPSebelumPersen").ToString, ViewState("DigitHome"))
                BindToText(tbBAPSebelum, Dr(0)("BAPSebelum").ToString, ViewState("DigitHome"))
                BindToText(tbBAPnowPersen, Dr(0)("TagihanBAPPersen").ToString, ViewState("DigitHome"))
                BindToText(tbBAPnow, Dr(0)("TagihanBAP").ToString, ViewState("DigitHome"))
                BindToText(tbSisaBAP, Dr(0)("SisaBAP").ToString, ViewState("DigitHome"))
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
