Imports System.Data
Imports System.Data.SqlClient
'Imports System.Data.Odbc
'Imports CrystalDecisions.CrystalReports.Engine
'Imports CrystalDecisions.Shared
'Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc

Partial Class BAPSerahTerimaBuktiKepemilikan
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "SELECT * FROM V_MKTBAPSTBKHd "

    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT * FROM V_MKTBAPSTBKDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

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
                If ViewState("Sender") = "btnNoPesanan" Then
                    tbNoPesanan.Text = Session("Result")(0).ToString
                    tbAreaCode.Text = Session("Result")(3).ToString
                    tbAreaName.Text = Session("Result")(4).ToString
                    If Session("Result")(5).ToString = "&nbsp;" Then
                        tbPenjualCode.Text = ""
                    Else
                        tbPenjualCode.Text = Session("Result")(5).ToString  'Replace(Session("Result")(7).ToString, ",", "")
                    End If
                    If Session("Result")(6).ToString = "&nbsp;" Then
                        tbPenjualName.Text = ""
                    Else
                        tbPenjualName.Text = Session("Result")(6).ToString  'Replace(Session("Result")(8).ToString, ",", "")
                    End If
                    tbPembeliCode.Text = Session("Result")(7).ToString
                    tbPembeliName.Text = Session("Result")(8).ToString
                End If

                If ViewState("Sender") = "btnMutCertNo" Then
                    tbMutCertNo.Text = Session("Result")(0).ToString
                End If

                If ViewState("Sender") = "btnUnit" Then
                    tbUnitCode.Text = Session("Result")(0).ToString
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
        'FillCombo(ddlUnit, "SELECT Unit_Code, Unit_Name FROM VMsUnit", True, "Unit_Code", "Unit_Name", ViewState("DBConnection"))

        'tbPrice.Attributes.Add("OnBlur", "setformatfordt();")
        'tbDisc.Attributes.Add("OnBlur", "setformatfordt();")
        'tbDiscValue.Attributes.Add("OnBlur", "setformatfordt();")
        'tbTandaJadi.Attributes.Add("OnBlur", "setformatfordt();")
        'tbDpp.Attributes.Add("OnBlur", "setformatfordt();")
        'tbDP.Attributes.Add("OnBlur", "setformatfordt();")
        'tbDPValue.Attributes.Add("OnBlur", "setformatfordt();")
        'tbTotalAmount.Attributes.Add("OnBlur", "setformatfordt();")

        'Me.tbTotalAmount.Attributes.Add("ReadOnly", "True")
        'Me.tbTotAmount.Attributes.Add("ReadOnly", "True")
        'Me.tbTotDisc.Attributes.Add("ReadOnly", "True")
        'Me.tbTotDPP.Attributes.Add("ReadOnly", "True")
        'Me.tbTotTJ.Attributes.Add("ReadOnly", "True")
        'Me.tbTotDP.Attributes.Add("ReadOnly", "True")
        'Me.tbDPValue.Attributes.Add("ReadOnly", "True")
        'Me.tbDpp.Attributes.Add("ReadOnly", "True")
        'Me.tbDiscValue.Attributes.Add("ReadOnly", "True")


        'Me.tbPrice.Attributes.Add("OnKeyDown", "return PressNumeric();")
        'Me.tbDisc.Attributes.Add("OnKeyDown", "return PressNumeric();")
        'Me.tbDiscValue.Attributes.Add("OnKeyDown", "return PressNumeric();")
        'Me.tbDpp.Attributes.Add("OnKeyDown", "return PressNumeric();")
        'Me.tbDP.Attributes.Add("OnKeyDown", "return PressNumeric();")

        'Me.tbLuasTanah.Attributes.Add("OnKeyDown", "return PressNumeric();")
        'Me.tbLuasBangunan.Attributes.Add("OnKeyDown", "return PressNumeric();")
        'Me.tbKtinggianDPL.Attributes.Add("OnKeyDown", "return PressNumeric();")
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
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Save this transaction first, to upload the dokumen")
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
            path2 = Server.MapPath("~/Dokumen/") + tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubInv.FileName
            namafile2 = tbCode.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubInv.FileName

            SQLString1 = "UPDATE MKTBAPSTBKHd SET FileBAPSTBK = " + QuotedStr(namafile2) + _
            " WHERE TransNmbr = " + QuotedStr(tbCode.Text)
            FubInv.SaveAs(path2)
            SQLExecuteNonQuery(SQLString1, ViewState("DBConnection").ToString)

            dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            lbDokInv.Text = dt.Rows(0)("FileBAPSTBK").ToString
            'lblmassageKTP.Visible = True
            FubInv.Visible = False
            btnClearInv.Visible = True

        Catch ex As Exception
            lbStatus.Text = "btnsaveINV_Click Error : " + ex.ToString
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

            If dr.Rows(0)("FileBAPSTBK").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            filePath = dr.Rows(0)("FileBAPSTBK").ToString
            URL = ResolveUrl("~/Dokumen/" + filePath)
            Dim s As String = "window.open('" & URL & "', '_blank');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", s, True)

        Catch ex As Exception
            lbStatus.Text = "lbDokInv_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnClearInv_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClearInv.Click
        Try
            Dim dr As DataTable
            Dim filePath As String
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
            filePath = dr.Rows(0)("FileBAPSTBK").ToString

            If File.Exists(Server.MapPath("~/Dokumen/" + filePath)) = True Then
                File.Delete(Server.MapPath("~/Dokumen/" + filePath))
                SQLExecuteNonQuery("UPDATE MKTBAPSTBKHd Set FileBAPSTBK = '' WHERE TransNmbr = '" + tbCode.Text + "' ", ViewState("DBConnection").ToString)

                lbDokInv.Text = "Not yet uploaded"
                FubInv.Visible = True
                btnClearInv.Visible = False
            End If

        Catch ex As Exception
            lbStatus.Text = "btnClearInv_Click Error : " + ex.ToString
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
                Session("SelectCommand") = "EXEC S_MKTFormBAPSTBK" + Result + "," + QuotedStr(ViewState("UserId"))
                Session("ReportFile") = ".../../../Rpt/FormBAPSTBK.frx"
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
                        Result = ExecSPCommandGo(ActionValue, "S_MKTBAPSTBK", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            'btnArea.Enabled = State
            'btnArea.Visible = State
            'tbArea.Enabled = False
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
                'If ViewState("DtValue") <> TbUnit.Text Then
                'If CekExistData(ViewState("Dt"), "ItemNo", lbItemNo.Text) Then
                '    lbStatus.Text = "Item No " + CInt(lbItemNo.Text) + " has been already exist"
                '    Exit Sub
                'End If
                ''End If
                'Row = ViewState("Dt").Select("ItemNo = " + QuotedStr(ViewState("DtValue")))(0)
                'If CekDt() = False Then
                '    Exit Sub
                'End If
                Row = ViewState("Dt").Select("ItemNo = " + lbItemNo.Text)(0)
                Row.BeginEdit()
                'Row("ItemNo") = CInt(lbItemNo.Text)
                Row("MutCertNo") = tbMutCertNo.Text
                Row("UnitCode") = tbUnitCode.Text
                Row("UnitName") = tbUnitName.Text
                Row("CertNo") = tbCertNo.Text
                Row("Luas") = tbLuas.Text
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If

                If CekExistData(ViewState("Dt"), "ItemNo", CInt(lbItemNo.Text)) = True Then
                    lbStatus.Text = "Item No " + CInt(lbItemNo.Text) + " has already been exist"
                    Exit Sub
                End If

                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("ItemNo") = CInt(lbItemNo.Text)
                dr("MutCertNo") = tbMutCertNo.Text
                dr("UnitCode") = tbUnitCode.Text
                dr("UnitName") = tbUnitName.Text
                dr("CertNo") = tbCertNo.Text
                dr("Luas") = tbLuas.Text
                dr("Remark") = tbRemarkDt.Text
                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            BindGridDt(ViewState("Dt"), GridDt)
            StatusButtonSave(True)
            'CountTotalDt()
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

                tbCode.Text = GetAutoNmbr("STBK", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO MKTBAPSTBKHd (TransNmbr,TransDate,Status,PSNo,UnitCode,AreaCode,SellCode,CustCode,Remark,FgReport,FgActive,UserPrep,DatePrep) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", 'H', " + _
                QuotedStr(tbNoPesanan.Text) + ", " + QuotedStr(tbKavlingCode.Text) + ", " + QuotedStr(tbAreaCode.Text) + ", " + _
                QuotedStr(tbPenjualCode.Text) + ", " + QuotedStr(tbPembeliCode.Text) + ", " + QuotedStr(tbRemark.Text) + ", 'Y', " + _
                " 'Y'," + QuotedStr(ViewState("UserId").ToString) + ", GetDate()"
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM MKTBAPSTBKHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If

                SQLString = "UPDATE MKTBAPSTBKHd SET TransDate = " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + _
                ", PSNo = " + QuotedStr(tbNoPesanan.Text) + _
                ", UnitCode = " + QuotedStr(tbKavlingCode.Text) + _
                ", AreaCode = " + QuotedStr(tbAreaCode.Text) + _
                ", SellCode = " + QuotedStr(tbPenjualCode.Text) + _
                ", CustCode = " + QuotedStr(tbPembeliCode.Text) + _
                ", Remark = " + QuotedStr(tbRemark.Text) + _
                ", DatePrep = GetDate()" + _
                " WHERE TransNmbr = " + QuotedStr(tbCode.Text) + " "
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

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString 'System.Configuration.ConfigurationManager.AppSettings.Get("ConString")
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr,ItemNo,MutCertNo,CertNo,Luas,UnitCode,Remark FROM MKTBAPSTBKDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
            "UPDATE MKTBAPSTBKDt SET ItemNo = @ItemNo, MutCertNo = @MutCertNo, " + _
            "CertNo = @CertNo, Luas = @Luas, UnitCode = @UnitCode,Remark = @Remark " + _
            "WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND ItemNo = @OldItemNo", con)

            '' Define output parameters.
            Update_Command.Parameters.Add("@ItemNo", SqlDbType.Int, 4, "ItemNo")
            Update_Command.Parameters.Add("@MutCertNo", SqlDbType.VarChar, 100, "MutCertNo")
            Update_Command.Parameters.Add("@CertNo", SqlDbType.VarChar, 100, "CertNo")
            Update_Command.Parameters.Add("@Luas", SqlDbType.Int, 22, "Luas")
            Update_Command.Parameters.Add("@UnitCode", SqlDbType.VarChar, 20, "UnitCode")
            Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 255, "Remark")
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
            param = Update_Command.Parameters.Add("@OldItemNo", SqlDbType.Int, 4, "ItemNo")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM MKTBAPSTBKDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND ItemNo = @ItemNo ", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@ItemNo", SqlDbType.Int, 4, "ItemNo")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("MKTBAPSTBKDt")

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
            tbFilter.Text = tbCode.Text
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
            Throw New Exception("New Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbCode.Text = ""
            tbDate.SelectedDate = ViewState("ServerDate") 'Today
            tbNoPesanan.Text = ""
            tbKavlingCode.Text = ""
            tbKavlingName.Text = ""
            tbAreaCode.Text = ""
            tbAreaName.Text = ""
            tbPembeliCode.Text = ""
            tbPembeliName.Text = ""
            tbPenjualCode.Text = ""
            tbPenjualName.Text = ""
            tbRemark.Text = ""
            EnableHd(True)
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub Cleardt()
        Try
            'TbUnit.Text = ""
            tbMutCertNo.Text = ""
            tbCertNo.Text = ""
            tbLuas.Text = ""
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

    Protected Sub btnNoPesanan_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNoPesanan.Click
        Dim ResultField As String
        Try
            'Session("filter") = "SELECT ID, StructureName FROM V_MsStructure WHERE LevelCode = '01' "
            Session("Filter") = "SELECT DISTINCT TransNmbr, TransDate, SPNo, Area_Code, Area_Name, Penjual, " + _
                                "Penjual_Name, Pembeli, Pembeli_Name " + _
                                "FROM V_MKTGetNoPesanan "
            ResultField = "TransNmbr, TransDate, SPNo, Area_Code, Area_Name, Penjual, " + _
                          "Penjual_Name, Pembeli, Pembeli_Name"
            ViewState("Sender") = "btnNoPesanan"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            'AttachScript("OpenSearchGrid();", Page, Me.GetType())
            AttachScript("OpenPopup();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Supp Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub btnUnit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUnit.Click
    '    Dim ResultField, CriteriaField, sqlstring As String
    '    Try

    '        sqlstring = "EXEC S_GetStructure " + QuotedStr(tbArea.Text)
    '        Session("DBConnection") = ViewState("DBConnection")
    '        Session("filter") = sqlstring
    '        ResultField = "StructureCode, StructureName, Account, AccountNAme, ID"
    '        CriteriaField = "StructureCode, StructureName, Account, AccountNAme,ID"
    '        Session("CriteriaField") = CriteriaField.Split(",")
    '        ViewState("Sender") = "btnUnit"
    '        Session("Column") = ResultField.Split(",")
    '        AttachScript("OpenSearchGrid();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lbStatus.Text = "btn Search Supp Error : " + ex.ToString
    '    End Try
    'End Sub

    'Protected Sub TbUnit_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TbUnit.TextChanged
    '    Dim Dr As DataRow
    '    Try
    '        Dr = FindMaster("Structure", TbUnit.Text, ViewState("DBConnection").ToString)
    '        If Not Dr Is Nothing Then
    '            TbUnit.Text = Dr("Supplier_Code")
    '            tbUnitName.Text = Dr("Supplier_Name")
    '        Else
    '            TbUnit.Text = ""
    '            tbUnitName.Text = ""
    '        End If
    '        TbUnit.Focus()
    '    Catch ex As Exception
    '        Throw New Exception("tb SuppCode Error : " + ex.ToString)
    '    End Try
    'End Sub

    'Private Sub CountTotalDt()
    '    Dim TotHarga, TotDisc, TotDPP, TotTJ, TotDP, TotAmount As Double
    '    Dim Dr As DataRow
    '    Try

    '        TotHarga = 0
    '        TotDisc = 0
    '        TotDPP = 0
    '        TotTJ = 0
    '        TotDP = 0
    '        TotAmount = 0
    '        For Each Dr In ViewState("Dt").Rows
    '            If Not Dr.RowState = DataRowState.Deleted Then
    '                TotHarga = TotHarga + CFloat(Dr("Price").ToString)
    '                TotDisc = TotDisc + CFloat(Dr("DiscValue").ToString)
    '                TotDPP = TotDPP + CFloat(Dr("DPP").ToString)
    '                TotTJ = TotTJ + CFloat(Dr("TJ").ToString)
    '                TotDP = TotDP + CFloat(Dr("DPValue").ToString)
    '                TotAmount = TotAmount + CFloat(Dr("AmountForex").ToString)
    '            End If
    '        Next
    '        tbTotHarga.Text = FormatNumber(TotHarga, ViewState("DigitHome"))
    '        tbTotDisc.Text = FormatNumber(TotDisc, ViewState("DigitHome"))
    '        tbTotDPP.Text = FormatNumber(TotDPP, ViewState("DigitHome"))
    '        tbTotTJ.Text = FormatNumber(TotTJ, ViewState("DigitHome"))
    '        tbTotDP.Text = FormatNumber(TotDP, ViewState("DigitHome"))
    '        tbTotAmount.Text = FormatNumber(TotAmount, ViewState("DigitHome"))

    '    Catch ex As Exception
    '        Throw New Exception("Count Total Dt Error : " + ex.ToString)
    '    End Try
    'End Sub

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
            If tbNoPesanan.Text = "" Then
                lbStatus.Text = MessageDlg("Pesanan No must have value")
                tbNoPesanan.Focus()
                Return False
            End If
            'If tbKavlingCode.Text = "" Then
            '    lbStatus.Text = MessageDlg("Kavling Code must have value")
            '    tbKavlingCode.Focus()
            '    Return False
            'End If

            'If CInt(Session(Request.QueryString("KeyId"))("Year")) <> Year(tbDate.SelectedValue) Or CInt(Session(Request.QueryString("KeyId"))("Period")) <> Month(tbDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(Session(Request.QueryString("KeyId"))("Period")) + " " + Session(Request.QueryString("KeyId"))("Year").ToString.Trim)
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

                'If Dr("Price").ToString = "0" Or Dr("Price").ToString = "" Then
                '    lbStatus.Text = MessageDlg("Price Must Have Value")
                '    Return False
                'End If
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
                If tbMutCertNo.Text = "" Then
                    lbStatus.Text = MessageDlg("No. Mutasi Sertifikat Must Have Value")
                    tbMutCertNo.Focus()
                    Return False
                End If
                If tbCertNo.Text = "" Then
                    lbStatus.Text = MessageDlg("No. Sertifikat Must Have Value")
                    tbCertNo.Focus()
                    Return False
                End If
                If tbLuas.Text = "" Or tbLuas.Text = "0" Then
                    lbStatus.Text = MessageDlg("Luas Must Have Value")
                    tbLuas.Focus()
                    Return False
                End If
                'If tbKtinggianDPL.Text = "" Or tbKtinggianDPL.Text = "0" Then
                '    lbStatus.Text = MessageDlg("Ketingian DPL Must Have Value")
                '    tbKtinggianDPL.Focus()
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

    'Protected Sub LbAdvSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LbAdvSearch.Click
    '    Dim FDateName, FDateValue, FilterName, FilterValue As String
    '    Try
    '        FDateName = "Date, Invoice Date"
    '        FDateValue = "TransDate, SuppInvDate"
    '        FilterName = "Reference, Date, Status,  Remark"
    '        FilterValue = "TransNmbr, dbo.FormatDate(TransDate), Status,  Remark"
    '        Session("DateFieldName") = FDateName.Split(",")
    '        Session("DateFieldValue") = FDateValue.Split(",")
    '        Session("FieldName") = FilterName.Split(",")
    '        Session("FieldValue") = FilterValue.Split(",")
    '        AttachScript("OpenFilterCriteria();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lbStatus.Text = "Advanced Search Click Error : " + ex.ToString
    '    End Try
    'End Sub

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
                        Session("SelectCommand") = "EXEC S_MKTFormBAPSTBK '''" + GVR.Cells(2).Text + "'''," + QuotedStr(ViewState("UserId"))
                        Session("ReportFile") = ".../../../Rpt/FormBAPSTBK.frx"
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
            ViewState("DtValue") = GVR.Cells(1).Text

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
            tbCode.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            'BindToDropList(ddlFgsewa, Dt.Rows(0)("FgSewa").ToString)
            'BindToDropList(ddlSistemBayar, Dt.Rows(0)("SystemPembayaran").ToString)
            BindToText(tbNoPesanan, Dt.Rows(0)("PSNo").ToString)
            BindToText(tbKavlingCode, Dt.Rows(0)("UnitCode").ToString)
            BindToText(tbKavlingName, Dt.Rows(0)("StructureName").ToString)
            BindToText(tbAreaCode, Dt.Rows(0)("AreaCode").ToString)
            BindToText(tbAreaName, Dt.Rows(0)("Area_Name").ToString)
            BindToText(tbPenjualCode, Dt.Rows(0)("SellCode").ToString)
            BindToText(tbPenjualName, Dt.Rows(0)("Penjual_Name").ToString)
            BindToText(tbPembeliCode, Dt.Rows(0)("CustCode").ToString)
            BindToText(tbPembeliName, Dt.Rows(0)("Pembeli_Name").ToString)
            'BindToText(tbTlp, Dt.Rows(0)("PhoneCPembeli").ToString)
            'BindToText(tbEmail, Dt.Rows(0)("EmailCPembeli").ToString)
            'BindToText(tbAlamat, Dt.Rows(0)("Alamat").ToString)
            'BindToText(tbTotHarga, Dt.Rows(0)("TotalHarga").ToString, ViewState("DigitCurr"))
            'BindToText(tbTotDisc, Dt.Rows(0)("TotalDisc").ToString, ViewState("DigitCurr"))
            'BindToText(tbTotDPP, Dt.Rows(0)("TotalDPP").ToString, ViewState("DigitCurr"))
            'BindToText(tbTotTJ, Dt.Rows(0)("TotalTJ").ToString, ViewState("DigitCurr"))
            'BindToText(tbTotDP, Dt.Rows(0)("TotalDP").ToString, ViewState("DigitCurr"))
            'BindToText(tbTotAmount, Dt.Rows(0)("TotalAmount").ToString, ViewState("DigitCurr"))
            'BindToText(tbAngsuran, Dt.Rows(0)("MasaAngsuran").ToString)
            'BindToDate(tbMasaBerlaku, Dt.Rows(0)("MasaBerlaku").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)

            If Dt.Rows(0)("FileBAPSTBK").ToString = "" Then
                'cbKtp.Checked = False
                lbDokInv.Text = "Not Yet Uploaded"
            Else
                lbDokInv.Text = Dt.Rows(0)("FileBAPSTBK").ToString
                'cbKtp.Checked = True
            End If

        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal UnitCode As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("ItemNo = " + UnitCode)
            If Dr.Length > 0 Then
                lbItemNo.Text = Dr(0)("ItemNo").ToString
                BindToText(tbMutCertNo, Dr(0)("MutCertNo").ToString)
                BindToText(tbCertNo, Dr(0)("CertNo").ToString)
                BindToText(tbLuas, Dr(0)("Luas").ToString)
                BindToText(tbUnitCode, Dr(0)("UnitCode").ToString)
                BindToText(tbUnitName, Dr(0)("UnitName").ToString)
                'BindToText(tbArahKav, Dr(0)("arahKavling").ToString)
                'BindToText(tbKtinggianDPL, Dr(0)("KtinggianDPL").ToString)
                'BindToText(tbPrice, Dr(0)("Price").ToString, ViewState("DigitHome"))
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

    Protected Sub btnMutCertNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMutCertNo.Click
        Dim ResultField As String
        Try
            Session("Filter") = "SELECT TransNmbr, TransDate, AreaCode, AreaName FROM V_PRCMTSertifikatHd WHERE Status<>'D' "
            ResultField = "TransNmbr, TransDate, AreaCode, AreaName"
            ViewState("Sender") = "btnMutCertNo"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenPopup();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Supp Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnUnit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUnit.Click
        Dim ResultField As String
        Try
            Session("Filter") = "SELECT Kavling, Kavling_Name FROM V_MKTGetNoPesanan WHERE TransNmbr = " + QuotedStr(tbNoPesanan.Text)
            ResultField = "Kavling, Kavling_Name"
            ViewState("Sender") = "btnUnit"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenPopup();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Supp Error : " + ex.ToString
        End Try
    End Sub

End Class
