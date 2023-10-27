Imports System.Data
Imports System.Data.SqlClient
'Imports System.Data.Odbc
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc

Partial Class Transaction_TrDocReceive_TrDocReceive
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select * From V_PRCDocReceiveHD "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                FillCombo(ddlType, "SELECT IjinType, IjinName FROM V_TypeIjin ", True, "IjinType", "IjinName", ViewState("DBConnection"))
                SetInit()
                Session("AdvanceFilter") = ""
            End If
            lbStatus.Text = ""
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If

            ' hasil dari search dialog
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "BtnPayment" Then
                    tbPaymentNoDt.Text = Session("Result")(0).ToString
                End If

                'If ViewState("Sender") = "btnArea" Then
                '    tbArea.Text = Session("Result")(0).ToString
                '    tbAreaName.Text = Session("Result")(1).ToString
                'End If


                'If ViewState("Sender") = "btnUnit" Then
                '    TbUnit.Text = Session("Result")(0).ToString
                '    tbUnitName.Text = Session("Result")(1).ToString
                'End If

                If ViewState("Sender") = "btnGetDt" Then
                    Dim drResult As DataRow
                    Dim ExistRow As DataRow()
                    For Each drResult In Session("Result").Rows
                        ExistRow = ViewState("Dt2").Select("PaymentNo = " + QuotedStr(drResult("PaymentNo").ToString))
                        If ExistRow.Count = 0 Then
                            'insert
                            Dim dr As DataRow
                            dr = ViewState("Dt2").NewRow
                            dr("PaymentNo") = drResult("PaymentNo")
                            ViewState("Dt2").Rows.Add(dr)
                        End If
                    Next
                    BindGridDt(ViewState("Dt2"), GridDt2)

                    EnableHd(GetCountRecord(ViewState("Dt2")) = 0)
                End If


                Session("Result") = Nothing
                ViewState("Sender") = Nothing
                Session("filter") = Nothing
                Session("Column") = Nothing
            End If

            FubInv.Attributes("onchange") = "UploadInvoice(this)"

        Catch ex As Exception
            lbStatus.Text = "Page Load Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnGetDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGetDt.Click
        Dim ResultField, CriteriaField As String 'ResultSame 
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            Session("Result") = Nothing
            Session("Filter") = "SELECT * FROM V_GetPaymentPosting "
            ResultField = "PaymentNo, PaymentDate, Status, TotalPayment "
            CriteriaField = "PaymentNo, PaymentDate, Status, TotalPayment "
            Session("CriteriaField") = CriteriaField.Split(",")
            Session("Column") = ResultField.Split(",")
            ViewState("Sender") = "btnGetDt"
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn get Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnPaymentNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPaymentNo.Click
        Dim ResultField As String
        Try
            Session("filter") = " SELECT * FROM V_GetPaymentPosting "
            ResultField = "PaymentNo, PaymentDate, Status, TotalPayment"
            ViewState("Sender") = "BtnPayment"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchGrid();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Supp Error : " + ex.ToString
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
        'If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
        '    ddlCommand.Items.Add("Print")
        '    ddlCommand2.Items.Add("Print")
        'End If


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
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbRef.Text), ViewState("DBConnection").ToString)

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
            path2 = Server.MapPath("~/Image/DokumenReceivePDF/") + tbRef.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubInv.FileName
            namafile2 = tbRef.Text.Trim.Replace("/", "") + Format(Now, "-yyMMddHHmmss-") + FubInv.FileName

            SQLString1 = "UPDATE PRCDocReceiveHD SET FileUpDokumen = " + QuotedStr(namafile2) + _
            " WHERE TransNmbr = " + QuotedStr(tbRef.Text)
            FubInv.SaveAs(path2)
            SQLExecuteNonQuery(SQLString1, ViewState("DBConnection").ToString)

            dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbRef.Text), ViewState("DBConnection").ToString)
            lbDokInv.Text = dt.Rows(0)("FileUpDokumen").ToString
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
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbRef.Text), ViewState("DBConnection").ToString)

            If dr.Rows.Count = 0 Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            If dr.Rows(0)("FileUpDokumen").ToString = "" Then
                lbStatus.Text = MessageDlg("Belum ada dokumen yang di upload")
                Exit Sub
            End If

            filePath = dr.Rows(0)("FileUpDokumen").ToString
            URL = ResolveUrl("~/Image/DokumenReceivePDF/" + filePath)
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
            dr = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(tbRef.Text), ViewState("DBConnection").ToString)
            filePath = dr.Rows(0)("FileUpDokumen").ToString


            If File.Exists(Server.MapPath("~/Image/DokumenReceivePDF/" + filePath)) = True Then
                File.Delete(Server.MapPath("~/Image/DokumenReceivePDF/" + filePath))
                SQLExecuteNonQuery("UPDATE PRCDocReceiveHD Set FileUpDokumen = '' WHERE TransNmbr = '" + tbRef.Text + "' ", ViewState("DBConnection").ToString)

                lbDokInv.Text = "Not yet uploaded"
                FubInv.Visible = True
                btnClearInv.Visible = False
            End If



        Catch ex As Exception
            lbStatus.Text = "lbBAP_Click Error : " + ex.ToString
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

    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT * From V_PRCDocReceiveDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Private Function GetStringDt2(ByVal Nmbr As String) As String
        Return "SELECT * From V_PRCDocReceiveDt2 WHERE TransNmbr = " + QuotedStr(Nmbr)
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
                Session("SelectCommand") = "EXEC S_FNFormDocReceive " + Result + "," + QuotedStr(ViewState("UserId"))
                Session("ReportFile") = ".../../../Rpt/FormDocReceive.frx"
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
                        Result = ExecSPCommandGo(ActionValue, "S_PRCDocReceive", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            'EnableHd(GridDt.Rows.Count = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        Dim index As Integer
        Try


            index = Int32.Parse(e.Item.Value)
            MultiView1.ActiveViewIndex = index
            btnSaveTrans.Focus()


            If MultiView1.ActiveViewIndex = 1 Then

                'lbStatus.Text = ViewState("StateHD") = "View"
                pnlDt2.Visible = True
                pnlEditDt3.Visible = False
                'BindDataDt2(ViewState("TransNmbr"))
                'If lbStatus.Text = "True" Then
                '    GridDt3.Columns(0).Visible = True
                'End If

                'If lbStatus.Text = "False" Then
                '    GridDt3.Columns(0).Visible = False
                'End If


            End If

        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt.Click
        Dim Row As DataRow
        Try
            If ViewState("StateDt") = "Edit" Then
                If ViewState("DtValue") <> tbNoDok.Text Then
                    If CekExistData(ViewState("Dt"), "NoDoc", tbNoDok.Text) Then
                        lbStatus.Text = "Doc No " + tbNoDok.Text + " has been already exist"
                        Exit Sub
                    End If
                End If

                Row = ViewState("Dt").Select("NoDoc = " + QuotedStr(ViewState("DtValue")))(0)
                If CekDt() = False Then
                    Exit Sub
                End If

                Row.BeginEdit()
                Row("NoDoc") = tbNoDok.Text
                Row("TypeDoc") = ddlType.Text
                Row("IjinName") = ddlType.SelectedItem.Text
                Row("PaymentNo") = tbPaymentNo.Text
                Row("Perihal") = tbPerihal.Text
                Row("TerbitDate") = tbdateTerbit.Text
                Row("EndDate") = tbEndDate.Text
                Row("Instansi") = tbPenerbit.Text
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()

            Else

                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If

                If CekExistData(ViewState("Dt"), "NoDoc", tbNoDok.Text) = True Then
                    lbStatus.Text = "No Dokumen" + tbNoDok.Text + " has already been exist"
                    Exit Sub
                End If



                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("ItemNo") = CInt(lbItemNo.Text)
                dr("NoDoc") = tbNoDok.Text
                dr("TypeDoc") = ddlType.Text
                dr("IjinName") = ddlType.SelectedItem.Text
                dr("PaymentNo") = tbPaymentNo.Text
                dr("Perihal") = tbPerihal.Text
                dr("TerbitDate") = tbdateTerbit.Text
                dr("EndDate") = tbEndDate.Text
                dr("Instansi") = tbPenerbit.Text
                dr("Remark") = tbRemarkDt.Text
                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            BindGridDt(ViewState("Dt"), GridDt)
            StatusButtonSave(True)



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

            'Save Hd
            If ViewState("StateSave") = "Insert" Then

                tbRef.Text = GetAutoNmbr("DRC", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)


                SQLString = "INSERT INTO PRCDocReceiveHd (TransNmbr, Status, TransDate, Remark,  UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbRef.Text) + ", 'H', '" + Format(tbDate.SelectedDate, "yyyy-MM-dd") + "', " + _
                QuotedStr(tbRemark.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", getDate()"


            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM PRCDocReceiveHd WHERE TransNmbr = " + QuotedStr(tbRef.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If

                SQLString = "UPDATE PRCDocReceiveHd SET TransDate = '" + Format(tbDate.SelectedDate, "yyyy-MM-dd") + _
                "', Remark = " + QuotedStr(tbRemark.Text) + _
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

            Row = ViewState("Dt2").Select("TransNmbr IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("TransNmbr") = tbRef.Text
                Row(I).EndEdit()
            Next



            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString 'System.Configuration.ConfigurationManager.AppSettings.Get("ConString")
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr,PaymentNo,ItemNo,NoDoc,TypeDoc,Perihal,TerbitDate,EndDate,Instansi,Remark FROM PRCDocReceiveDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            'Dim param As SqlParameter
            '' Create the UpdateCommand.
            'Dim Update_Command = New SqlCommand( _
            '"UPDATE PRCDocReceiveDt SET ItemNo = @ItemNo, InvoiceNo = @InvoiceNo, " + _
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
            '    "DELETE FROM PRCDocReceiveDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND ItemNo = @ItemNo ", con)
            '' Add the parameters for the DeleteCommand.
            'param = Delete_Command.Parameters.Add("@ItemNo", SqlDbType.Int, 4, "ItemNo")
            'param.SourceVersion = DataRowVersion.Original
            'da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("PRCDocReceiveDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt

            'save dt3
            cmdSql = New SqlCommand("SELECT TransNmbr, PaymentNo, Remark FROM PRCDocReceiveDt2  WHERE TransNmbr = '" & ViewState("TransNmbr") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            dbcommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand


            Dim Dt2 As New DataTable("PRCDocReceiveDt2")

            Dt2 = ViewState("Dt2")
            da.Update(Dt2)
            Dt2.AcceptChanges()
            ViewState("Dt2") = Dt2


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
            ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
            newTrans()
            MultiView1.ActiveViewIndex = 0
            Menu1.Items.Item(0).Selected = True
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
            BindDataDt2("")
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbRef.Text = ""
            tbDate.SelectedDate = ViewState("ServerDate") 'Today

            EnableHd(True)
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub Cleardt()
        Try
            tbNoDok.Text = ""
            tbPaymentNo.Text = ""

            tbdateTerbit.SelectedDate = ViewState("ServerDate") 'Today
            tbEndDate.SelectedDate = ViewState("ServerDate") 'Today
            tbPerihal.Text = ""
            tbPenerbit.Text = ""

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


    Private Sub Cleardt3()
        Try
            tbPaymentNoDt.Text = ""
            tbRemarkdt3.Text = ""

        Catch ex As Exception
            Throw New Exception("Clear Dt 3 Error " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnAddDt3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt3.Click, btnAddDt3ke2.Click
        Try
            Cleardt3()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt2") = "Insert"
            MovePanel(pnlDt2, pnlEditDt3)
            EnableHd(False)
            StatusButtonSave(False)
            'tbEquip.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt3 error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancelDt3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelDt3.Click
        Try
            MovePanel(pnlEditDt3, pnlDt2)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt3 Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnSaveDt3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt3.Click
        Try
            If CekDt3() = False Then
                btnSaveDt3.Focus()
                Exit Sub
            End If

            If ViewState("StateDt2") = "Edit" Then
                Dim Row As DataRow
                Row = ViewState("Dt2").Select("PaymentNo = " + QuotedStr(ViewState("Dt2Value")))(0)
                Row.BeginEdit()
                Row("PaymentNo") = tbPaymentNoDt.Text
                Row("Remark") = tbRemarkdt3.Text

                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt2").NewRow
                dr("PaymentNo") = tbPaymentNoDt.Text
                dr("Remark") = tbRemarkdt3.Text


                ViewState("Dt2").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt3, pnlDt2)
            BindGridDt(ViewState("Dt2"), GridDt2)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn save Dt2 Error : " + ex.ToString
        Finally
            If Not con Is Nothing Then con.Dispose()
            If Not da Is Nothing Then da.Dispose()
        End Try
    End Sub

    Function CekDt3(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not Dr Is Nothing Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("PaymentNo").ToString = "" Then
                    lbStatus.Text = MessageDlg("Payment No Must Have Value")
                    Return False
                End If
            Else
                If tbPaymentNoDt.Text = "" Then
                    lbStatus.Text = MessageDlg("Payment No Must Have Value")
                    tbPaymentNoDt.Focus()
                    Return False
                End If


            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub GridDt2_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt2.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow
            GVR = GridDt2.Rows(e.RowIndex)
            dr = ViewState("Dt3").Select("PaymentNo = " + QuotedStr(GVR.Cells(1).Text))
            dr(0).Delete()
            BindGridDt(ViewState("Dt3"), GridDt2)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt 3 Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt2_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt2.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt2.Rows(e.NewEditIndex)
            FillTextBoxDt2(GVR.Cells(1).Text)
            ViewState("Dt2Value") = GVR.Cells(1).Text
            MovePanel(pnlDt2, pnlEditDt3)
            EnableHd(False)
            ViewState("StateDt2") = "Edit"
            StatusButtonSave(False)
            btnSaveDt3.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid dt3 Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub FillTextBoxDt2(ByVal PaymentNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt2").select("PaymentNo = " + QuotedStr(PaymentNo))
            If Dr.Length > 0 Then
                BindToText(tbPaymentNoDt, Dr(0)("PaymentNo").ToString)
                BindToText(tbRemarkdt3, Dr(0)("Remark").ToString)

            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail 3 error : " + ex.ToString)
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

    'Protected Sub btnArea_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnArea.Click
    '    Dim ResultField As String
    '    Try
    '        Session("filter") = "select ID, StructureName from V_MsStructure WHERE LevelCode = '01' "
    '        ResultField = "ID, StructureName"
    '        ViewState("Sender") = "btnArea"
    '        Session("Column") = ResultField.Split(",")
    '        Session("DBConnection") = ViewState("DBConnection")
    '        AttachScript("OpenSearchGrid();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lbStatus.Text = "btn Search Supp Error : " + ex.ToString
    '    End Try
    'End Sub

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



            'If tbNoSPK.Text = "" Then
            '    lbStatus.Text = MessageDlg("SPK No must have value")
            '    tbNoSPK.Focus()
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




            Else


                If tbNoDok.Text = "" Then
                    lbStatus.Text = MessageDlg("No Dokumen Must Have Value")
                    tbNoDok.Focus()
                    Return False
                End If



                'If tbDP.Text = "" Or tbDP.Text = "0" Then
                '    lbStatus.Text = MessageDlg("DP Must Have Value")
                '    tbDP.Focus()
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
            FDateValue = "TransDate, SuppInvDate"
            FilterName = "Reference, Date, Status,  Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), Status,  Remark"
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
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, pnlDt, GridDt)
                    ModifyInput2(False, pnlInput, pnlDt2, GridDt2)
                    MultiView1.ActiveViewIndex = 0
                    Menu1.Items.Item(0).Selected = True
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
                        BindDataDt2(ViewState("TransNmbr"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
                        btnHome.Visible = False
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                        FillTextBoxHd(ViewState("TransNmbr"))
                        MultiView1.ActiveViewIndex = 0
                        Menu1.Items.Item(0).Selected = True
                        EnableHd(True)
                        StatusButtonSave(True)


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
                        Session("SelectCommand") = "EXEC S_FNFormDocReceive '''" + GVR.Cells(2).Text + "'''," + QuotedStr(ViewState("UserId"))
                        Session("ReportFile") = ".../../../Rpt/FormDocReceive.frx"
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
            dr = ViewState("Dt").Select("NoDoc = " + QuotedStr(GVR.Cells(2).Text))
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
            FillTextBoxDt(GVR.Cells(2).Text)
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)

            ViewState("StateDt") = "Edit"
            ViewState("DtValue") = GVR.Cells(2).Text

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
            tbRef.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)

            'If Dt.Rows(0)("FileUpDokumen").ToString = "" Then
            '    'cbKtp.Checked = False
            '    lbDokInv.Text = "Not Yet Uploaded"
            'Else
            '    lbDokInv.Text = Dt.Rows(0)("FileUpDokumen").ToString
            '    'cbKtp.Checked = True
            'End If

        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub



    Protected Sub FillTextBoxDt(ByVal UnitCode As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("NoDoc = " + QuotedStr(UnitCode))
            If Dr.Length > 0 Then
                lbItemNo.Text = Dr(0)("ItemNo").ToString
                BindToText(tbNoDok, Dr(0)("NoDoc").ToString)
                BindToDropList(ddlType, Dr(0)("TypeDoc").ToString)
                BindToText(tbPaymentNo, Dr(0)("PaymentNo").ToString)
                BindToText(tbPerihal, Dr(0)("Perihal").ToString)
                BindToDate(tbdateTerbit, Dr(0)("TerbitDate").ToString)
                BindToDate(tbEndDate, Dr(0)("EndDate").ToString)
                BindToText(tbPenerbit, Dr(0)("Instansi").ToString)
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
