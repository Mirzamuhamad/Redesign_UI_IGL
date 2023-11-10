Imports System.Data
Imports System.Data.SqlClient
'Imports System.Data.Odbc
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc

Partial Class AlokasiKawasan
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select * From V_FINAlokasiHd "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                'FillCombo(ddlExpenseType, "SELECT Payment_Code, Payment_Name FROM V_MsPayType WHERE FgMode = 'E'", True, "Payment_Code", "Payment_Name", ViewState("DBConnection"))
                'FillCombo(ddlCurr, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
                'FillCombo(ddlCostCenter, "EXEC S_GetCostCtr", True, "Cost_Ctr_Code", "Cost_Ctr_Name", ViewState("DBConnection"))
                'FillCombo(ddlUnit, "EXEC S_GetUnit", False, "Unit_Code", "Unit_Name", ViewState("DBConnection"))
                'FillCombo(ddlCostCtr, "EXEC S_GetCostCtr", True, "Cost_Ctr_Code", "Cost_Ctr_Name", ViewState("DBConnection"))
                ''FillCombo(ddlPType, "EXEC S_GetProductType", False, "Type_Code", "Type_Name", ViewState("DBConnection"))
                SetInit()
                Session("AdvanceFilter") = ""
                'lbCount.Text = SQLExecuteScalar("SELECT COUNT(Invoice_no) FROM V_GetInvPosting ", ViewState("DBConnection").ToString)
            End If
            lbStatus.Text = ""
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If

            ' hasil dari search dialog
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "BtnPayment" Then
                    tbPaymentNo.Text = Session("Result")(0).ToString
                    tbAmountPayment.Text = FormatNumber(Session("Result")(3).ToString, ViewState("DigitCurr"))
                    tbPercenAlokasi.Text = FormatNumber(Session("Result")(4).ToString, ViewState("DigitCurr"))
                End If

                If ViewState("Sender") = "btnLanPurchase" Then
                    tbLandPurchaseNo.Text = Session("Result")(0).ToString
                    tbAmountPayment.Text = Session("Result")(4).ToString
                End If


                If ViewState("Sender") = "BtnArea" Then
                    tbAreaCode.Text = Session("Result")(0).ToString
                    tbAreaName.Text = Session("Result")(1).ToString
                End If


                If ViewState("Sender") = "btnStructure" Then
                    tbStructureCode.Text = Session("Result")(0).ToString
                    tbStructureName.Text = Session("Result")(1).ToString

                    If Session("Result")(2).ToString = "&nbsp;" Then
                        tbAccount.Text = ""
                    Else
                        tbAccount.Text = Session("Result")(2).ToString

                    End If

                    If Session("Result")(3).ToString = "&nbsp;" Then
                        tbAccountName.Text = ""
                    Else
                        tbAccountName.Text = Session("Result")(3).ToString

                    End If
                    tbIdStruktur.Text = Session("Result")(4).ToString


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
        FillCombo(ddlTypeALokasi, "SELECT CostAlokasiCode,CostAlokasiName FROM MsCostAlokasi Where FgActive = 'Y' ", True, "CostAlokasiCode", "CostAlokasiName", ViewState("DBConnection")) 'ddlReport.SelectedValue
        ViewState("SortExpression") = Nothing
        ViewState("DigitCurr") = 2
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))

        If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
            ddlCommand.Items.Add("Print")
            ddlCommand2.Items.Add("Print")
        End If


        Me.tbTotalAmount.Attributes.Add("ReadOnly", "True")

        Me.tbAmountPayment.Attributes.Add("OnKeyDown", "return PressNumeric();")

        'tbInvoice.Attributes.Add("OnBlur", "setformatfordt();")


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
        Return "SELECT * From V_FINAlokasiDt  WHERE TransNmbr = " + QuotedStr(Nmbr) + " ORDER BY ItemNO "
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
                Session("SelectCommand") = "EXEC S_FNFormAlokasi " + Result + ", 'CN' "
                Session("ReportFile") = ".../../../Rpt/FormAlokasi.frx"
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
                        Result = ExecSPCommandGo(ActionValue, "S_FNAlokasi", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            tbAccount.Enabled = State
            tbRemark.Enabled = State
            tbTotalAmount.Enabled = False
        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub



    Private Sub EnableDt(ByVal State As Boolean)
        Try
            tbPaymentNo.Enabled = State
            tbAreaCode.Enabled = State
            tbStructureCode.Enabled = State
            tbIdStruktur.Enabled = State
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
                Row("PaymentNo") = tbPaymentNo.Text
                Row("AreaCode") = tbAreaCode.Text
                Row("AreaName") = tbAreaName.Text
                Row("StructureCode") = tbStructureCode.Text
                Row("StructureName") = tbStructureName.Text
                Row("AccNmbr") = tbAccount.Text
                Row("AccName") = tbAccountName.Text
                Row("TotalPayment") = tbAmountPayment.Text
                Row("Remark") = tbRemarkDt.Text
                Row("IDStructur") = tbIdStruktur.Text
                Row("TypeAlokasi") = ddlTypeALokasi.SelectedValue
                Row("LpNo") = tbLandPurchaseNo.Text
                Row("PercenAlokasi") = tbPercenAlokasi.Text
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
                dr("ItemNo") = lbItemNo.Text
                dr("PaymentNo") = tbPaymentNo.Text
                dr("AreaCode") = tbAreaCode.Text
                dr("AreaName") = tbAreaName.Text
                dr("StructureCode") = tbStructureCode.Text
                dr("StructureName") = tbStructureName.Text
                dr("AccNmbr") = tbAccount.Text
                dr("AccName") = tbAccountName.Text
                dr("TotalPayment") = tbAmountPayment.Text
                dr("Remark") = tbRemarkDt.Text
                dr("IDStructur") = tbIdStruktur.Text
                dr("TypeAlokasi") = ddlTypeALokasi.SelectedValue
                dr("LpNo") = tbLandPurchaseNo.Text
                dr("PercenAlokasi") = tbPercenAlokasi.Text
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
                'ddlReport.SelectedValue
                tbRef.Text = GetAutoNmbr("CAK", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO FINAlokasiHd (TransNmbr,Status, TransDate,  TotalAmount , " + _
                "Remark, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbRef.Text) + ", 'H', '" + Format(tbDate.SelectedDate, "yyyy-MM-dd") + "', " + _
                QuotedStr(tbTotalAmount.Text.Replace(",", "")) + ", " + _
                QuotedStr(tbRemark.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", getDate()"
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM FINAlokasiHd WHERE TransNmbr = " + QuotedStr(tbRef.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If

                SQLString = "UPDATE FINAlokasiHd SET TotalAmount = " + QuotedStr(tbTotalAmount.Text.Replace(",", "")) + _
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
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, ItemNo, PaymentNo, AreaCode, StructureCode, AccNmbr, TotalPayment, Remark, IDStructur, TypeAlokasi, LPNo, PercenAlokasi  FROM FINAlokasiDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
            "UPDATE FINAlokasiDt SET ItemNo = @ItemNo, PaymentNo = @PaymentNo, " + _
            "AreaCode = @AreaCode, StructureCode = @StructureCode, AccNmbr = @AccNmbr, " + _
            "TotalPayment = @TotalPayment,IDStructur = @IDStructur, TypeAlokasi = @TypeAlokasi,LPNo = @LPNo, " + _
            "Remark = @Remark, PercenAlokasi = @PercenAlokasi " + _
            "WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND ItemNo = @OldItemNo", con)

            ' Define output parameters.n
            Update_Command.Parameters.Add("@ItemNo", SqlDbType.VarChar, 5, "ItemNo")
            Update_Command.Parameters.Add("@PaymentNo", SqlDbType.VarChar, 30, "PaymentNo")
            Update_Command.Parameters.Add("@AreaCode", SqlDbType.VarChar, 30, "AreaCode")
            Update_Command.Parameters.Add("@StructureCode", SqlDbType.VarChar, 20, "StructureCode")
            Update_Command.Parameters.Add("@AccNmbr", SqlDbType.VarChar, 30, "AccNmbr")
            Update_Command.Parameters.Add("@TotalPayment", SqlDbType.Float, 22, "TotalPayment")
            Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 255, "Remark")
            Update_Command.Parameters.Add("@IDStructur", SqlDbType.VarChar, 20, "IDStructur")
            Update_Command.Parameters.Add("@TypeAlokasi", SqlDbType.VarChar, 20, "TypeAlokasi")
            Update_Command.Parameters.Add("@LPNo", SqlDbType.VarChar, 30, "LPNo")
            Update_Command.Parameters.Add("@PercenAlokasi", SqlDbType.Float, 22, "PercenAlokasi")

            '' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@OldItemNo", SqlDbType.Int, 4, "ItemNo")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM FINAlokasiDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND ItemNo = @ItemNo ", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@ItemNo", SqlDbType.Int, 4, "ItemNo")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("FINAlokasiDt")

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
            tbTotalAmount.Text = 0
            tbRemark.Text = ""
            EnableHd(True)

        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub Cleardt()
        Try
            tbPaymentNo.Text = ""
            tbAreaCode.Text = ""
            tbAreaName.Text = ""
            tbStructureCode.Text = ""
            tbStructureName.Text = ""
            tbAccount.Text = ""
            tbAccountName.Text = ""
            tbAmountPayment.Text = 0
            tbPercenAlokasi.Text = 0
            tbRemarkDt.Text = ""
            tbIdStruktur.Text = ""
            tbLandPurchaseNo.Text = ""
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
    Protected Sub btnLanPurchase_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLanPurchase.Click
        Dim ResultField As String
        Try
            'Session("filter") = "SELECT Reference, Status, LosNo, TJNo   FROM V_GLLandPurchaseHD WHERE Status = 'P'"
            Session("filter") = "SELECT * FROM V_GLLandPurchaseHD WHERE Status = 'P'"
            ResultField = "Reference, Status, LosNo, TJNo, TtlHrgTanah "
            ViewState("Sender") = "btnLanPurchase"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchGrid();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Supp Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnPaymentNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPaymentNo.Click
        Dim ResultField As String
        Try
            If ddlTypeALokasi.SelectedValue = "ADM" Then
                Session("filter") = "select * from V_GetVoucherPayment WHERE InvoiceType = 'CIA'"
            ElseIf ddlTypeALokasi.SelectedValue = "INF" Then
                Session("filter") = "select * from V_GetVoucherPayment WHERE InvoiceType = 'CIF'"
            Else
                Session("filter") = "select * from V_GetVoucherPayment WHERE InvoiceType = 'LPO'"
            End If
            ResultField = "PaymentNo, PaymentDate, Status, TotalPayment, PercenAlokasi"

            ViewState("Sender") = "BtnPayment"
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
            Session("filter") = "select ID, StructureName from V_MsStructure WHERE LevelCode = '01' "
            ResultField = "ID, StructureName"
            ViewState("Sender") = "BtnArea"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("myPopup();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Supp Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnStructure_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStructure.Click
        Dim ResultField, CriteriaField, sqlstring As String
        Try

            sqlstring = "EXEC S_GetStructure " + QuotedStr(tbAreaCode.Text)
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = sqlstring
            ResultField = "StructureCode, StructureName, Account, AccountNAme, ID"
            CriteriaField = "StructureCode, StructureName, Account, AccountNAme,ID"
            Session("CriteriaField") = CriteriaField.Split(",")
            ViewState("Sender") = "btnStructure"
            Session("Column") = ResultField.Split(",")
            AttachScript("myPopup();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Supp Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub ddlTypeALokasi_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTypeALokasi.SelectedIndexChanged

        Try
            tbAccount.Text = ""
            tbPaymentNo.Text = ""
            tbAccountName.Text = ""
            tbAmountPayment.Text = 0
            


            'If ddlTypeALokasi.SelectedValue = "ADM" Then
            '    btnPaymentNo.Visible = True
            '    btnLanPurchase.Visible = False
            '    btnArea.Visible = True
            '    btnStructure.Visible = True
            '    lbArea.Enabled = True
            '    lblStructure.Enabled = True
            '    tbAreaCode.Enabled = False
            '    tbAreaName.Enabled = False
            '    tbStructureCode.Enabled = False
            '    tbStructureName.Enabled = False
            '    tbLandPurchaseNo.Enabled = False
            '    tbPaymentNo.Enabled = False
            '    tbAccount.Text = ""
            '    tbLandPurchaseNo.Text = ""
            '    tbAccountName.Text = ""
            '    tbAmountPayment.Text = 0
            'Else
            '    btnPaymentNo.Visible = False
            '    btnLanPurchase.Visible = True
            '    'btnArea.Visible = False
            '    'btnStructure.Visible = False
            '    'lbArea.Enabled = False
            '    'lblStructure.Enabled = False
            '    tbAreaCode.Enabled = False
            '    tbAreaName.Enabled = False
            '    tbStructureCode.Enabled = False
            '    tbStructureName.Enabled = False
            '    tbLandPurchaseNo.Enabled = False
            '    tbPaymentNo.Enabled = False
            '    tbAccount.Text = ""
            '    tbPaymentNo.Text = ""
            '    tbAccountName.Text = ""
            '    tbAmountPayment.Text = 0
            'End If


        Catch ex As Exception
            lbStatus.Text = "ddl Type Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub tbPercenAlokasi_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPercenAlokasi.TextChanged
        Dim Amount, Percen As String
        Try
            Percen = SQLExecuteScalar("Select PercenAlokasi FROM V_GetVoucherPayment WHERE PaymentNo = " + QuotedStr(tbPaymentNo.Text), ViewState("DBConnection").ToString)
            Amount = SQLExecuteScalar("Select TotalNilai FROM V_GetVoucherPayment WHERE PaymentNo = " + QuotedStr(tbPaymentNo.Text), ViewState("DBConnection").ToString)

            'lbStatus.Text = Percen

            If tbPercenAlokasi.Text > Val(Percen) Or tbPercenAlokasi.Text = 0 Then
                tbPercenAlokasi.Text = FormatNumber(Percen, ViewState("DigitCurr"))
                tbAmountPayment.Text = FormatNumber((Amount * Val(tbPercenAlokasi.Text)) / 100, ViewState("DigitCurr"))
            End If

            tbAmountPayment.Text = FormatNumber((Amount * Val(tbPercenAlokasi.Text)) / 100, ViewState("DigitCurr"))

        Catch ex As Exception
            Throw New Exception("tb SuppCode Error : " + ex.ToString)
        End Try
    End Sub



    'Protected Sub tbSuppCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbSuppCode.TextChanged
    '    Dim Dr As DataRow
    '    Try
    '        Dr = FindMaster("Supplier", tbSuppCode.Text, ViewState("DBConnection").ToString)
    '        If Not Dr Is Nothing Then
    '            tbSuppCode.Text = Dr("Supplier_Code")
    '            tbSuppName.Text = Dr("Supplier_Name")
    '        Else
    '            tbSuppCode.Text = ""
    '            tbSuppName.Text = ""
    '            tbAttn.Text = ""
    '        End If
    '        'ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, ViewState("DBConnection").ToString)
    '        'ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
    '        'AttachScript("setformat();", Page, Me.GetType())
    '        tbSuppCode.Focus()
    '    Catch ex As Exception
    '        Throw New Exception("tb SuppCode Error : " + ex.ToString)
    '    End Try
    'End Sub


    Private Sub CountTotalDt()
        Dim TotalAmount As Double
        Dim Dr As DataRow
        Try

            TotalAmount = 0
            For Each Dr In ViewState("Dt").Rows
                If Not Dr.RowState = DataRowState.Deleted Then

                    TotalAmount = TotalAmount + CFloat(Dr("TotalPayment").ToString)

                End If
            Next
            tbTotalAmount.Text = FormatNumber(TotalAmount, ViewState("DigitHome"))


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
            btnLanPurchase.Visible = False
            ddlTypeALokasi.Text = "ADM"
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
                'If ddlTypeALokasi.SelectedValue <> "Tanah" Then
                If tbPaymentNo.Text = "" Then
                    lbStatus.Text = MessageDlg("Number Alokasi Must Have Value")
                    tbPaymentNo.Focus()
                    Return False
                End If
                'End If

                'If ddlTypeALokasi.SelectedValue = "Area" Then
                If tbAreaCode.Text = "" Then
                    lbStatus.Text = MessageDlg("Area Must Have Value")
                    tbAreaCode.Focus()
                    Return False
                End If

                If tbStructureCode.Text = "" Then
                    lbStatus.Text = MessageDlg("Structure Must Have Value")
                    tbStructureCode.Focus()
                    Return False
                End If
                'ElseIf ddlTypeALokasi.SelectedValue = "Tanah" Then
                '    If tbLandPurchaseNo.Text = "" Then
                '        lbStatus.Text = MessageDlg("Land Purchase No Must Have Value")
                '        tbLandPurchaseNo.Focus()
                '        Return False
                '    End If
                'End If

                If tbAmountPayment.Text = "0" Or tbTotalAmount.Text = "" Then
                    lbStatus.Text = MessageDlg("Payment Amount Must Have Value")
                    tbAmountPayment.Focus()
                    Return False
                End If


                'If ddlTypeALokasi.SelectedValue <> "Tanah" Then
                Dim cekTotalPayment As String
                cekTotalPayment = SQLExecuteScalar("Select TotalPayment FROM V_GetVoucherPayment WHERE PaymentNo = " + QuotedStr(tbPaymentNo.Text), ViewState("DBConnection").ToString)
                'lbStatus.Text = cekTotalPayment
                'Exit Function
                If CFloat(tbAmountPayment.Text.Replace(",", "")) > CFloat(cekTotalPayment) Then
                    lbStatus.Text = MessageDlg("Alokasi Cannot grether more than Total Invoice ")
                    Exit Function
                End If
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
            FilterName = "Reference, Date, Status, Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), Status, Remark"
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
                        Session("SelectCommand") = "EXEC S_FNFormAlokasi '''" + GVR.Cells(2).Text + "''', 'CN' "
                        Session("ReportFile") = ".../../../Rpt/FormAlokasi.frx"
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
            BindToText(tbTotalAmount, Dt.Rows(0)("TotalAmount").ToString, ViewState("DigitCurr"))
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
                BindToText(tbPaymentNo, Dr(0)("PaymentNo").ToString)
                BindToText(tbAreaCode, Dr(0)("AreaCode").ToString)
                BindToText(tbAreaName, Dr(0)("AreaName").ToString)
                BindToText(tbStructureCode, Dr(0)("StructureCode").ToString)
                BindToText(tbStructureName, Dr(0)("StructureName").ToString)
                BindToText(tbAccount, Dr(0)("AccNmbr").ToString)
                BindToText(tbAccountName, Dr(0)("AccName").ToString)
                BindToText(tbAmountPayment, Dr(0)("TotalPayment").ToString, ViewState("DigitHome"))
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
                BindToText(tbIdStruktur, Dr(0)("IDStructur").ToString)
                BindToDropList(ddlTypeALokasi, Dr(0)("TypeAlokasi").ToString)
                BindToText(tbLandPurchaseNo, Dr(0)("LPNo").ToString)
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


    Protected Sub lblStructure_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblStructure.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsStructure')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lbKegiatan_Click Error : " + ex.ToString
        End Try
    End Sub



    Protected Sub lbArea_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbArea.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsStructure')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lbArea_Click Error : " + ex.ToString
        End Try
    End Sub


End Class
