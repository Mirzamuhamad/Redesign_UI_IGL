Imports System.Data
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO

Partial Class Transaction_TrSPB_TrSPB
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select * From V_PLSPBHd"


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Try
            If Not IsPostBack Then
                InitProperty()
                FillCombo(ddlstatusTanam, "EXEC S_GetUmurTBS", True, "Umur_TBS", "Umur_TBS_Name", ViewState("DBConnection"))
                FillCombo(ddlEstate, "EXEC S_GetEstate ", True, "EstateCode", "EstateName", ViewState("DBConnection"))
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
                If ViewState("Sender") = "btnCar" Then
                    BindToText(tbKapal, Session("Result")(0).ToString)
                    BindToText(tbKapalName, Session("Result")(1).ToString)
                    tbKapal_TextChanged(Nothing, Nothing)
                End If
                If ViewState("Sender") = "btnCust" Then
                    tbCustCode.Text = Session("Result")(0).ToString
                    BindToText(tbCustName, Session("Result")(1).ToString)
                End If
                If ViewState("Sender") = "btnPengawas" Then
                    tbPengawasCode.Text = Session("Result")(0).ToString
                    BindToText(tbPengawasName, Session("Result")(1).ToString)
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

                            Result = ExecSPCommandGo("Delete", "S_PLSPTBS", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)

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
    End Sub

    Private Sub SetInit()
        FillRange(ddlRange)
        ViewState("DtRemark") = ""
        ViewState("SortExpression") = Nothing
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
        If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
            ddlCommand.Items.Add("Print")
            ddlCommand2.Items.Add("Print")
        End If

        'tbPanenHK.Attributes.Add("OnKeyDown", "return PressNumeric()")
        'tbNormal.Attributes.Add("OnKeyDown", "return PressNumeric()")
        'tbAbnormal.Attributes.Add("OnKeyDown", "return PressNumeric()")
        'tbBrondolan.Attributes.Add("OnKeyDown", "return PressNumeric()")

        'tbPanenHK.Attributes.Add("OnBlur", "setformatdt()")
        'tbNormal.Attributes.Add("OnBlur", "setformatdt()")
        'tbAbnormal.Attributes.Add("OnBlur", "setformatdt()")
        'tbBrondolan.Attributes.Add("OnBlur", "setformatdt()")
        btnAdd2.Visible = False
        btnGo2.Visible = False
        ddlCommand2.Visible = False
        BtnAdd.Visible = False
        BtnGo.Visible = False
        ddlCommand.Visible = False

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
            DV.Sort = ViewState("SortExpression")
            GridView1.DataSource = DV
            GridView1.DataBind()
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT * From V_PLSPBDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            GridView1.PageIndex = 0
            Session("AdvanceFilter") = ""
            BindData(Session("AdvanceFilter"))
            pnlNav.Visible = True
            'ddlCommand.Visible = True
            'BtnGo.Visible = True
            btnAdd2.Visible = False
            btnGo2.Visible = False
            ddlCommand2.Visible = False
            BtnAdd.Visible = False
            BtnGo.Visible = False
            ddlCommand.Visible = False
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
                Result = Result + "'"
                Session("SelectCommand") = "EXEC S_PLFormSPB" + Result
                'lbStatus.Text = Session("SelectCommand")
                'Exit Sub
                Session("ReportFile") = ".../../../Rpt/FormSPB.frx"
                Session("DBConnection") = ViewState("DBConnection")
                AttachScript("openprintdlg()", Page, Me.GetType)
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
                    AttachScript("deletetrans()", Page, Me.GetType)

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
                        Result = ExecSPCommandGo(ActionValue, "S_PLSPB", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                        If Trim(Result) <> "" Then
                            lbStatus.Text = lbStatus.Text + Result + " <br/>"

                        End If
                    End If
                Next
                BindData("Reference in (" + ListSelectNmbr + ")")
            End If
        Catch ex As Exception
            lbStatus.Text = "Go Command Error : " + ex.ToString
        End Try
    End Sub

    Private Sub EnableHd(ByVal State As Boolean)
        Try
            ddlEstate.Enabled = State

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
                tbRef.Text = GetAutoNmbr("PB", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
                SQLString = "INSERT INTO PLSPBHd (TransNmbr, Status, TransDate, InputDate, Estate, TPSReal, TPSGrading, TotalKirim, " + _
                " BrondolanKgs, BrdGrading, BrdKirim, Remark, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbRef.Text) + ", 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', '" + Format(tbDateMuat.SelectedValue, "yyyy-MM-dd") + "' , " + _
                QuotedStr(ddlEstate.SelectedValue) + ", " + tbTTPS.Text.Replace(",", "") + ", " + tbTGrading.Text.Replace(",", "") + ", " + tbTKirim.Text.Replace(",", "") + ", " + _
                tbbTPS.Text.Replace(",", "") + ", " + tbbTGrading.Text.Replace(",", "") + "," + tbbTKirim.Text.Replace(",", "") + "," + QuotedStr(tbRemark.Text) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", getDate()"
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM PLSPBHd WHERE TransNmbr = " + QuotedStr(tbRef.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE PLSPBHd SET Estate = " + QuotedStr(ddlEstate.SelectedValue) + _
                ", TPSReal = " + tbTTPS.Text.Replace(",", "") + ", BrondolanKgs = " + tbbTPS.Text.Replace(",", "") + _
                ", TPSGrading = " + tbTGrading.Text.Replace(",", "") + ", TotalKirim = " + tbTKirim.Text.Replace(",", "") + _
                ", BrdGrading = " + tbbTGrading.Text.Replace(",", "") + ", BrdKirim = " + tbbTKirim.Text.Replace(",", "") + _
                ", Remark = " + QuotedStr(tbRemark.Text) + _
                ", TransDate = " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", DateAppr = getDate()" + _
                " WHERE TransNmbr = " + QuotedStr(tbRef.Text)
            End If
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
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, SPBManualNo, TripNo, Customer, CarNo, DateBerangkat, JamBerangkat, StatusTanam, " + _
            " QtyJanjang, Owner, Pengawas, PriceTPS, " + _
            " QtyBrondolan FROM PLSPBDt WHERE TransNmbr = '" & ViewState("Reference") & "'", con)

            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
                    "UPDATE PLSPBDt SET SPBManualNo = @SPBManualNo, TripNo = @TripNo, Customer = @Customer, CarNo = @CarNo " + _
                    ", DateBerangkat = @DateBerangkat, JamBerangkat = @JamBerangkat, StatusTanam = @StatusTanam " + _
                    ", QtyJanjang = @QtyJanjang, Owner = @Owner, Pengawas = @Pengawas " + _
                    ", PriceTPS = @PriceTPS , QtyBrondolan = @QtyBrondolan " + _
                    ", QtyJanjang = @QtyJanjang WHERE TransNmbr = '" & ViewState("Reference") & "' AND SPBManualNo = @OldSPBManualNo", con)
            ' Define output parameters.
            Update_Command.Parameters.Add("@SPBManualNo", SqlDbType.VarChar, 20, "SPBManualNo")
            Update_Command.Parameters.Add("@TripNo", SqlDbType.VarChar, 20, "TripNo")
            Update_Command.Parameters.Add("@Customer", SqlDbType.VarChar, 12, "Customer")
            Update_Command.Parameters.Add("@CarNo", SqlDbType.VarChar, 30, "CarNo")
            Update_Command.Parameters.Add("@Owner", SqlDbType.VarChar, 60, "Owner")
            Update_Command.Parameters.Add("@DateBerangkat", SqlDbType.DateTime, 8, "DateBerangkat")
            Update_Command.Parameters.Add("@JamBerangkat", SqlDbType.VarChar, 5, "JamBerangkat")
            Update_Command.Parameters.Add("@StatusTanam", SqlDbType.VarChar, 5, "StatusTanam")
            Update_Command.Parameters.Add("@QtyBrondolan", SqlDbType.Int, 4, "QtyBrondolan")
            Update_Command.Parameters.Add("@QtyJanjang", SqlDbType.Int, 4, "QtyJanjang")
            Update_Command.Parameters.Add("@Pengawas", SqlDbType.VarChar, 60, "Pengawas")
            Update_Command.Parameters.Add("@FactorRate", SqlDbType.Float, 22, "FactorRate")
            Update_Command.Parameters.Add("@PriceTPS", SqlDbType.Float, 22, "PriceTPS")
            Update_Command.Parameters.Add("@BJR", SqlDbType.VarChar, 26, "BJR")

            ' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@OldSPBManualNo", SqlDbType.VarChar, 20, "SPBManualNo")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM PLSPBDt WHERE TransNmbr = '" & ViewState("Reference") & "' AND SPBManualNo = @SPBManualNo ", con)
            ' Add the parameters for the DeleteCommand.

            param = Delete_Command.Parameters.Add("@SPBManualNo", SqlDbType.VarChar, 20, "SPBManualNo")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("PLSPBDt")

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
            'If IsNothing(ViewState("Dt")) Then
            '    lbStatus.Text = MessageDlg("Detail must have at least 1 record")
            '    Exit Sub
            'End If
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
                Exit Sub
            End If
            For Each dr In ViewState("Dt").Rows
                If CekDt(dr, "SPBManualNo") = False Then
                    Exit Sub
                End If
            Next
            SaveAll()
            MovePanel(pnlInput, PnlHd)
            CurrFilter = tbFilter.Text
            Value = ddlField.SelectedValue
            tbFilter.Text = tbRef.Text
            ddlField.SelectedValue = "Reference"
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
            tbDate.Focus()
            EnableHd(True)
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ViewState("DtRemark") = ""
            ' ddlTransit_SelectedIndexChanged(Nothing, Nothing)
            ClearHd()
            Cleardt()
            pnlDt.Visible = True
            GridDt.Columns(1).Visible = False
            BindDataDt("")
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Dim SQLString As String
        Try
            tbRef.Text = ""
            ddlEstate.SelectedValue = ""
            tbTTPS.Text = "0"
            tbTKirim.Text = "0"
            tbTGrading.Text = "0"
            TbBalance.Text = "0"
            tbbTPS.Text = "0"
            tbbTKirim.Text = "0"
            tbbTGrading.Text = "0"
            TbbBalance.Text = "0"

            tbDateMuat.SelectedValue = ViewState("ServerDate") 'Today
            tbDate.SelectedDate = ViewState("ServerDate") 'Today

            tbRemark.Text = ""
            'SqlString = SQLExecuteScalar(" EXEC S_GetDivisionForSAUserDivision " + QuotedStr(ViewState("UserId")) + ", '1'", ViewState("DBConnection").ToString)
            ' Dim Division As String
            ' Division = SQLExecuteScalar("EXEC S_GetDivisionForSAUserDivision " + QuotedStr(ViewState("UserId")) + ", '1'", ViewState("DBConnection"))

            '  ddlDivision.SelectedValue = Division

        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbSPBManual.Text = ""
            tbTrip.Text = ""
            tbCustCode.Text = ""
            tbCustName.Text = ""
            tbKapal.Text = ""
            tbKapalName.Text = ""
            tbOwner.Text = ""
            tbPengawasCode.Text = ""
            tbPengawasName.Text = ""
            ddlstatusTanam.SelectedValue = ""
            tbSPBDate.SelectedDate = ViewState("ServerDate") 'Today
            tbJamSPB.Text = ""
            tbQtyJanjang.Text = "0"
            tbQtyBrondolan.Text = "0"
            tbPriceTPS.Text = "0"
            tbBJR.Text = ""
            tbFactorRate.Text = "0"

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
                If CekDt(dr, "SPBManualNo") = False Then
                    Exit Sub
                End If
            Next
            SaveAll()
            newTrans()
        Catch ex As Exception
            lbStatus.Text = "btn save all error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt.Click, btnAddDt2.Click
        Cleardt()
        If CekHd() = False Then
            Exit Sub
        End If
        ViewState("StateDt") = "Insert"
        MovePanel(pnlDt, pnlEditDt)
        EnableHd(False)
        StatusButtonSave(False)
    End Sub

    Function CekHd() As Boolean
        Try
            If tbDate.IsNull Then
                lbStatus.Text = MessageDlg("Date must have value")
                tbDate.Focus()
                Return False
            End If

            If ddlEstate.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Estate must have value")
                ddlEstate.Focus()
                Return False
            End If

            If Len(tbRemark.Text.Trim) > 60 Then
                lbStatus.Text = MessageDlg("Remark must have value or caracter must 60")
                tbRemark.Focus()
                Return False
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Hd Error : " + ex.ToString)
        End Try
    End Function

    Function CekDt(Optional ByVal Dr As DataRow = Nothing, Optional ByVal FieldKey As String = "") As Boolean
        Dim H, M As Integer
        Try
            If Not (Dr Is Nothing) Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("SPBManualNo").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("SPB Manual No Must Have Value")
                    Return False
                End If
                If Dr("Customer").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Customer Must Have Value")
                    Return False
                End If
                If Dr("CarNo").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Kapal No Must Have Value")
                    Return False
                End If
                If Dr("DateBerangkat").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Date Berangkat Must Have Value")
                    Return False
                End If

                If Dr("JamBerangkat").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Jam Berangkat Must Have Value")
                    Return False
                End If
                If CFloat(Dr("PriceTPS").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Price TPS Must Have Value")
                    Return False
                End If
                If CFloat(Dr("QtyJanjang").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Janjang Must Have Value")
                    Return False
                End If

                'If Dr("JamBerangkat").ToString.Trim = "" Then
                '    H = CInt(Str(tbJamSPB.Text, 1, 2))
                '    M = CInt(Str(tbJamSPB.Text, 4, 2))
                '    If (H > 23) Or (M > 59) Then Begin()
                '    MessageDlg("Jam Berangkat is invalid")
                '    tbJamSPB.Text = "00:00"
                '    Return False
                'End If

            Else

                If tbSPBManual.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("SPB Manual Must Have Value")
                    tbSPBManual.Focus()
                    Return False
                End If
                If tbCustCode.Text = "" Then
                    lbStatus.Text = MessageDlg("Customer Must Have Value")
                    tbCustCode.Focus()
                    Return False
                End If
                If tbKapal.Text = "" Then
                    lbStatus.Text = MessageDlg("Kapal No Must Have Value")
                    tbKapal.Focus()
                    Return False
                End If
                If tbJamSPB.Text = "" Then
                    lbStatus.Text = MessageDlg("Jam Berangkat Must Have Value")
                    tbJamSPB.Focus()
                    Return False
                End If
                If CFloat(tbPriceTPS.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Price TBS Must Have Value")
                    ddlstatusTanam.Focus()
                    Return False
                End If
                If CFloat(tbQtyJanjang.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Janjang Must Have Value")
                    ddlstatusTanam.Focus()
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
            FDateName = "Date"
            FDateValue = "TransDate"
            FilterName = "Reference, Date, InputDate, Remark"
            FilterValue = "Reference, dbo.FormatDate(TransDate), dbo.FormatDate(InputDate), Remark"
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
                    ViewState("Reference") = GVR.Cells(1).Text
                    ViewState("Status") = GVR.Cells(2).Text
                    If ViewState("Status") = "P" Then
                        GridDt.Columns(1).Visible = True
                    Else
                        GridDt.Columns(1).Visible = False
                    End If
                    GridDt.PageIndex = 0
                    FillTextBoxHd(ViewState("Reference"))
                    BindDataDt(ViewState("Reference"))
                    ViewState("StateHd") = "Complete"
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
                        ViewState("Reference") = GVR.Cells(2).Text
                        GridDt.Columns(1).Visible = False
                        GridDt.PageIndex = 0
                        BindDataDt(ViewState("Reference"))
                        FillTextBoxHd(ViewState("Reference"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        btnHome.Visible = False
                        'EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                        EnableHd(Not DtExist())
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
                        Session("SelectCommand") = "EXEC S_PLFormSPB ''" + QuotedStr(GVR.Cells(1).Text) + "''"

                        'Session("SelectCommand") = "EXEC S_STFormIssueReq ''" + QuotedStr(GVR.Cells(2).Text) "''"
                        Session("ReportFile") = ".../../../Rpt/FormSPB.frx"
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
        Dim dr() As DataRow

        Try
            Dim GVR As GridViewRow = GridDt.Rows(Convert.ToInt32(e.CommandArgument))

            If e.CommandName = "Complete" Then
                If GVR.Cells(20).Text = "Y" Then
                    lbStatus.Text = MessageDlg("Data is Complete")
                    Exit Sub
                End If
                FillCombo(ddlPabrik, "EXEC S_GetUmurTBS", True, "Umur_TBS", "Umur_TBS_Name", ViewState("DBConnection"))
                MovePanel(pnlClosing, pnlClosing)
                GridDt.Visible = False
                pnlEditDt.Visible = False
                pnlInput.Visible = False

                tbCSpbManual.Text = GVR.Cells(2).Text
                tbCTripNo.Text = GVR.Cells(3).Text
                tbCDate.SelectedDate = tbDate.SelectedDate
                tbCDateMuat.SelectedDate = tbDateMuat.SelectedDate
                tbCMinPles.Text = "0"
                tbCPersen.Text = "0"
                tbCComplateDate.SelectedDate = ViewState("ServerDate") 'Today
                ' tbCUserComplate.Text = QuotedStr(ViewState("UserId").ToString)
                tbCtglBongkar.SelectedDate = ViewState("ServerDate") 'Today
                tbCTiba.SelectedDate = ViewState("ServerDate") 'Today
                tbCUserComplate.Text = ViewState("UserId").ToString
                tbCJamTiba.Text = GVR.Cells(12).Text
                tbCJamBongkar.Text = GVR.Cells(12).Text
                tbCGradingKg.Text = "0"
                tbCGrading.Text = "0"
                tbCNettoKg.Text = "0"
                tbCBrutoKg.Text = "0"
                tbCttlGrading.Text = "0"
                tbCPersenGrading.Text = "0"
                StatusTanam(GVR.Cells(13).Text)
                FillTextBoxDtClosing(GVR.Cells(2).Text)
                LoadPriceTBS(GVR.Cells(13).Text)
                LoadPriceExpedisi(GVR.Cells(4).Text)
                LoadCarPrice(GVR.Cells(6).Text)
                LoadingPrice()
                StatusTanam(GVR.Cells(13).Text)

                'tbCGradingKg_TextChanged(Nothing, Nothing)
            End If



            'If e.CommandName = "Insert" Then
            '    btnAddDt_Click(Nothing, Nothing)
            'ElseIf e.CommandName = "Closing" Then
            '    Dim GVR As GridViewRow
            '    GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))
            '    If ViewState("Status") <> "P" Then
            '        lbStatus.Text = MessageDlg("Status Transaction is not Post, cannot close TK Panen")
            '        Exit Sub
            '    End If
            '    ViewState("ProductClose") = GVR.Cells(2).Text
            '    AttachScript("closing()", Page, Me.GetType)
            'End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub FillTextBoxDtClosing(ByVal Product As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("SPBManualNo = " + QuotedStr(Product))
            If Dr.Length > 0 Then
                BindToDate(tbCComplateDate, Dr(0)("DateComplete").ToString)
                BindToText(tbCSpbManual, Dr(0)("SPBManualNo").ToString)
                BindToText(tbCTripNo, Dr(0)("TripNo").ToString)
                BindToText(tbCCustomer, Dr(0)("Customer").ToString)
                BindToText(tbCCustomerName, TrimStr(Dr(0)("CustomerName").ToString))
                BindToText(tbCCarNo, Dr(0)("CarNo").ToString)
                'BindToText(tbKapalName, Dr(0)("CarName").ToString)
                'BindToText(tbOwner, Dr(0)("Owner").ToString)
                'BindToText(tbPengawasCode, Dr(0)("Pengawas").ToString)
                'BindToText(tbPengawasName, Dr(0)("PengawasName").ToString)
                BindToDate(tbCTiba, Dr(0)("ActDateTiba").ToString)
                BindToDate(tbCtglBongkar, Dr(0)("ActDateBongkar").ToString)
                ' BindToText(tbCJamTiba, Dr(0)("ActJamTiba").ToString)
                BindToText(tbCttlPlesMin, FormatNumber(Dr(0)("Price").ToString), ViewState("DigitQty"))
                BindToText(tbCttlPersen, FormatNumber(Dr(0)("PricePErcen").ToString), ViewState("DigitQty"))
                ' BindToText(tbCJamBongkar, Dr(0)("ActJamBongkar").ToString)
                BindToDropList(ddlPabrik, TrimStr(Dr(0)("StatusTanam").ToString))
                BindToText(tbCStatusTanam, TrimStr(Dr(0)("StatusTanamName").ToString))
                BindToDate(tbCTglBerangkat, Dr(0)("DateBerangkat").ToString)
                BindToText(tbCJamBerangkat, Dr(0)("JamBerangkat").ToString)
                BindToText(tbCPrice, Dr(0)("PriceTPS").ToString)
                BindToText(tbCPabrik, Dr(0)("PriceTPS").ToString)
                BindToText(tbFactorRate, FormatNumber(Dr(0)("FactorRate").ToString), ViewState("DigitQty"))
                BindToText(tbcNettoBjr, FormatNumber(Dr(0)("BJRNetto").ToString), ViewState("DigitQty"))
                ' BindToText(tbCGradingKg, Dr(0)("ActGradingKgs").ToString)
                ' BindToText(tbCGrading, Dr(0)("ActGradingPercent").ToString)
                '  BindToText(tbCNettoKg, FormatNumber(Dr(0)("BJRBruto").ToString), ViewState("DigitQty"))
                '  BindToText(tbCBrutoKg, Dr(0)("ActBrutoKgs").ToString)
                BindToText(tbCBrutoBJR, FormatNumber(Dr(0)("PricePErcen").ToString), ViewState("DigitQty"))
                BindToText(tbCRemark, Dr(0)("ActRemark").ToString)
                BindToText(tbCBongkar, Dr(0)("BongkarNo").ToString)
                BindToText(tbCKirim, Dr(0)("KirimNo").ToString)
                BindToText(tbCMuat, Dr(0)("MuatNo").ToString)
                BindToText(tbCTrid, Dr(0)("trip1muat").ToString)

                BindToText(tbBJR, Dr(0)("BJR").ToString)
                BindToText(tbCTBS, Dr(0)("QtyJanjang").ToString)
                BindToText(tbCBrondolan, Dr(0)("QtyBrondolan").ToString)
            End If
            'Dt = BindDataTransaction(GetStringDt(tbRef.Text), "ItemNo = " + ItemNo, ViewState("DBConnection").ToString)
            'newTrans()
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub


    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Dim dr() As DataRow
        Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
        dr = ViewState("Dt").Select("SPBManualNo = " + QuotedStr(GVR.Cells(2).Text))
        dr(0).Delete()
        ' ViewState("Dt").AcceptChanges()
        BindGridDt(ViewState("Dt"), GridDt)
        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            FillTextBoxDt(GVR.Cells(2).Text)
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            ViewState("DtValue") = tbSPBManual.Text
            tbSPBManual.Focus()
            StatusButtonSave(False)


        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "Reference = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            'newTrans()
            tbRef.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToDate(tbDateMuat, Dt.Rows(0)("InputDate").ToString)
            BindToDropList(ddlEstate, Dt.Rows(0)("Estate").ToString)
            BindToText(tbTTPS, Dt.Rows(0)("TPSReal").ToString)
            BindToText(tbTKirim, Dt.Rows(0)("TotalKirim").ToString)
            BindToText(tbTGrading, Dt.Rows(0)("TPSGrading").ToString)
            BindToText(TbBalance, Dt.Rows(0)("BalanceTBS").ToString)
            BindToText(tbbTPS, Dt.Rows(0)("BrondolanKgs").ToString)
            BindToText(tbbTKirim, Dt.Rows(0)("BrdKirim").ToString)
            BindToText(tbbTGrading, Dt.Rows(0)("BrdGrading").ToString)
            BindToText(TbbBalance, Dt.Rows(0)("BalanceBrd").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)

        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal Product As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("SPBManualNo = " + QuotedStr(Product))
            If Dr.Length > 0 Then
                BindToText(tbSPBManual, Dr(0)("SPBManualNo").ToString)
                BindToText(tbTrip, Dr(0)("TripNo").ToString)
                BindToText(tbCustCode, Dr(0)("Customer").ToString)
                BindToText(tbCustName, TrimStr(Dr(0)("CustomerName").ToString))
                BindToText(tbKapal, Dr(0)("CarNo").ToString)
                BindToText(tbKapalName, Dr(0)("CarName").ToString)
                BindToText(tbOwner, Dr(0)("Owner").ToString)
                BindToText(tbPengawasCode, Dr(0)("Pengawas").ToString)
                BindToText(tbPengawasName, Dr(0)("PengawasName").ToString)
                BindToDropList(ddlstatusTanam, TrimStr(Dr(0)("StatusTanam").ToString))
                BindToDate(tbSPBDate, Dr(0)("DateBerangkat").ToString)
                BindToText(tbJamSPB, Dr(0)("JamBerangkat").ToString)
                BindToText(tbPriceTPS, Dr(0)("PriceTPS").ToString)
                BindToText(tbFactorRate, Dr(0)("FactorRate").ToString)
                BindToText(tbBJR, Dr(0)("BJR").ToString)
                BindToText(tbQtyJanjang, Dr(0)("QtyJanjang").ToString)
                BindToText(tbQtyBrondolan, Dr(0)("QtyBrondolan").ToString)
            End If
            'Dt = BindDataTransaction(GetStringDt(tbRef.Text), "ItemNo = " + ItemNo, ViewState("DBConnection").ToString)
            'newTrans()
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub


    Private Sub StatusButtonSave(ByVal Bool As Boolean)
        btnSaveAll.Visible = Bool
        btnSaveTrans.Visible = Bool
        btnBack.Visible = Bool
    End Sub

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveDt.Click
        Dim Row As DataRow
        Try
            If CekDt() = False Then
                Exit Sub
            End If
            If ViewState("StateDt") = "Edit" Then
                If ViewState("DtValue") <> tbSPBManual.Text Then
                    If CekExistData(ViewState("Dt"), "SPBManualNo", tbSPBManual.Text) Then
                        lbStatus.Text = "SPB Manual '" + tbSPBManual.Text + "' has already exists"
                        Exit Sub
                    End If
                End If
                Row = ViewState("Dt").Select("SPBManualNo = " + QuotedStr(ViewState("DtValue")))(0)
                'If CekDt() = False Then
                '    Exit Sub
                'End If
                Row.BeginEdit()
                Row("SPBManualNo") = tbSPBManual.Text
                Row("TripNo") = tbTrip.Text
                Row("Customer") = tbCustCode.Text
                Row("CustomerName") = tbCustName.Text
                If ddlstatusTanam.SelectedValue = "" Then
                    Row("StatusTanam") = ddlstatusTanam.SelectedValue
                    Row("StatusTanamName") = ""
                Else
                    Row("StatusTanam") = ddlstatusTanam.SelectedValue
                    Row("StatusTanamName") = ddlstatusTanam.SelectedItem.Text
                End If
                Row("CarNo") = tbKapal.Text
                Row("CarName") = tbKapalName.Text
                Row("Owner") = tbOwner.Text
                Row("Pengawas") = tbPengawasCode.Text
                Row("PengawasName") = tbPengawasName.Text
                Row("DateBerangkat") = tbSPBDate.Text
                Row("JamBerangkat") = tbJamSPB.Text
                Row("PriceTPS") = tbPriceTPS.Text
                Row("BJR") = tbBJR.Text
                Row("FactorRate") = tbFactorRate.Text
                Row("QtyBrondolan") = tbQtyBrondolan.Text
                Row("QtyJanjang") = tbQtyJanjang.Text
                Row.EndEdit()
                'ViewState("Dt").AcceptChanges()
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If
                If CekExistData(ViewState("Dt"), "SPBManualNo", tbSPBManual.Text) Then
                    lbStatus.Text = "SPB Manual '" + tbSPBManual.Text + "' has already exists"
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("SPBManualNo") = tbSPBManual.Text
                dr("TripNo") = tbTrip.Text
                dr("Customer") = tbCustCode.Text
                dr("CustomerName") = tbCustName.Text
                If ddlstatusTanam.SelectedValue = "" Then
                    dr("StatusTanam") = ddlstatusTanam.SelectedValue
                    dr("StatusTanamName") = ""
                Else
                    dr("StatusTanam") = ddlstatusTanam.SelectedValue
                    dr("StatusTanamName") = ddlstatusTanam.SelectedItem.Text
                End If
                dr("CarNo") = tbKapal.Text
                dr("CarName") = tbKapalName.Text
                dr("Owner") = tbOwner.Text
                dr("Pengawas") = tbPengawasCode.Text
                dr("PengawasName") = tbPengawasName.Text
                dr("DateBerangkat") = tbSPBDate.Text
                dr("JamBerangkat") = tbJamSPB.Text
                dr("PriceTPS") = tbPriceTPS.Text
                dr("BJR") = tbBJR.Text
                dr("FactorRate") = tbFactorRate.Text
                dr("QtyBrondolan") = tbQtyBrondolan.Text
                dr("QtyJanjang") = tbQtyJanjang.Text
                ViewState("Dt").Rows.Add(dr)
            End If
            'ViewState("DtRemark") = tbRemarkDt.Text
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
    Private Function DtExist() As Boolean
        Dim dete, piar As Boolean
        Try
            If ViewState("Dt") Is Nothing Then
                dete = False
            Else
                dete = GetCountRecord(ViewState("Dt")) > 0
            End If

            Return (dete Or piar)

            'EnableHd(GetCountRecord(ViewState("Dt")) = 0 And ViewState("DtPR").Rows.Count = 0 And ViewState("DtPart").Rows.Count = 0)
        Catch ex As Exception
            Throw New Exception("Cek Data Hd Error : " + ex.ToString)
        End Try
    End Function
    Protected Sub btnCancelDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBack.Click, btnCancelDt.Click
        Try
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            'EnableHd(GridDt.Rows.Count = 0)
            StatusButtonSave(True)
            EnableHd(Not DtExist())
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub
    'Protected Sub lbTKPanen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbTKPanen.Click
    '    Try
    '        ViewState("InputProduct") = "Y"
    '        AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsTeam')()", Page, Me.GetType())
    '    Catch ex As Exception
    '        lbStatus.Text = "lb Product Error : " + ex.ToString
    '    End Try
    'End Sub
    Protected Sub btnCar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCar.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT Car_No, Car_Name  FROM V_MsCar Where FgExternalDelivery = 'Y'"
            ResultField = "Car_No, Car_Name"
            ViewState("Sender") = "btnCar"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg()", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Cust Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCust_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCust.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT Customer_Code, Customer_Name FROM VMsCustomer "
            ResultField = "Customer_Code, Customer_Name"
            ViewState("Sender") = "btnCust"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg()", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Cust Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnpengawas_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPengawas.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT Emp_No, Emp_Name FROM V_MsEmployee Where Fg_Active='Y' "
            ResultField = "Emp_No, Emp_Name"
            ViewState("Sender") = "btnPengawas"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg()", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Cust Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub tbCustCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCustCode.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try
            Dt = SQLExecuteQuery("EXEC S_FindCustomer " + QuotedStr(tbCustCode.Text), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbCustCode.Text = Dr("Customer_Code").ToString
                tbCustName.Text = Dr("Customer_Name").ToString
            Else
                tbCustCode.Text = ""
                tbCustName.Text = ""
            End If
            btnCust.Focus()
        Catch ex As Exception
            Throw New Exception("tbCustCode_TextChanged Error : " + ex.ToString)
        End Try
    End Sub


    Protected Sub tbPengawasCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPengawasCode.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try
            Dt = SQLExecuteQuery("EXEC S_FindEmployee " + QuotedStr(tbPengawasCode.Text), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbPengawasCode.Text = Dr("Emp_No").ToString
                tbPengawasName.Text = Dr("Emp_Name").ToString
            Else
                tbPengawasCode.Text = ""
                tbPengawasName.Text = ""
            End If
            btnPengawas.Focus()
        Catch ex As Exception
            Throw New Exception("tbPengawasCode_TextChanged Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub tbKapal_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbKapal.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try
            Dt = SQLExecuteQuery("EXEC S_FindCar " + QuotedStr(tbKapal.Text), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbKapal.Text = Dr("CarNo").ToString
                tbKapalName.Text = Dr("CarName").ToString
                tbOwner.Text = Dr("Supplier_Name").ToString
            Else
                tbKapal.Text = ""
                tbKapalName.Text = ""
                tbOwner.Text = ""
            End If
            btnPengawas.Focus()
        Catch ex As Exception
            Throw New Exception("tbKapal_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Dim TotalQty As Decimal = 0
    Dim TotalBrd As Decimal = 0
    Protected Sub GridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt.RowDataBound
        Try
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "SPBManualNo")) Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                    TotalQty += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "QtyJanjang"))
                    TotalBrd += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "QtyBrondolan"))
                End If
            ElseIf e.Row.RowType = DataControlRowType.Footer Then
                ' TotalQty = GetTotalSum(ViewState("Dt"), "Qty")
                e.Row.Cells(5).Text = "Total : "

            End If
            tbTKirim.Text = FormatFloat(TotalQty, ViewState("DigitQty"))
            tbbTKirim.Text = FormatFloat(TotalBrd, ViewState("DigitQty"))
            TbBalance.Text = CFloat(tbTTPS.Text) - CFloat(tbTKirim.Text) - CFloat(tbTGrading.Text)
            TbbBalance.Text = CFloat(tbbTPS.Text) - CFloat(tbbTKirim.Text) - CFloat(tbbTGrading.Text)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCacelCom_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCacelCom.Click
        Try
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            'EnableHd(GridDt.Rows.Count = 0)
            StatusButtonSave(True)
            EnableHd(Not DtExist())
            GridDt.Visible = True
            pnlEditDt.Visible = False
            pnlInput.Visible = True
            pnlClosing.Visible = False

            btnSaveAll.Visible = False
            btnSaveTrans.Visible = False
            btnBack.Visible = False



        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlPabrik_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlPabrik.SelectedIndexChanged
        Try
            LoadPriceTBS(ddlPabrik.SelectedValue)
        Catch ex As Exception
            lbStatus.Text = "ddlstatusTanam_TextChanged Error : " + ex.ToString
        End Try
        'Dim Dt As DataTable
        'Dim Dr As DataRow
        'Try
        '    Dt = SQLExecuteQuery("SELECT Umur_TBS, Umur_TBS_Name, FactorRate FROM V_MsUmurTBS WHERE Umur_TBS = " + QuotedStr(ddlPabrik.SelectedValue), ViewState("DBConnection").ToString).Tables(0)

        '    If Dt.Rows.Count > 0 Then
        '        Dr = Dt.Rows(0)
        '        ddlPabrik.SelectedValue = Dr("Umur_TBS").ToString
        '        tbCPabrik.Text = Dr("FactorRate").ToString
        '    Else
        '        ddlPabrik.SelectedValue = ""
        '        tbCPabrik.Text = ""
        '    End If
        '    AttachScript("setformatdt()", Page, Me.GetType())
        '    tbCPabrik.Focus()
        'Catch ex As Exception
        '    Throw New Exception("ddlWeek Error : " + ex.ToString)
        'End Try
    End Sub

    'Protected Sub tbCPabrik_textChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCPabrik.TextChanged
    '      if tbCPabrik.text = 0 then exit
    '      if tbCPrice.text = 0 then exit
    '      tbCttlPlesMin.text = tbCPabrik.Text -  tbCPrice.Text
    '      CdsDt2PricePercen.Text = (CdsDt2Price.Text / tbCPrice.Text) * 100

    '      if tbCBrutoKg.Text = 0 then Exit
    '      tbCttlGrading.Text = tbCBrutoKg.Text * tbCPrice.Text - tbCNettoKg.Text * tbCPabrik.Text
    '      tbCPercenGrading..Text = (tbCttlGrading.Text  / (tbCBrutoKg.Text * tbCPrice.Text)) * 100

    'End Sub

    Protected Sub tbCBrutoKg_textChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCBrutoKg.TextChanged
        Try
            tbCNettoKg.Text = FormatFloat(tbCBrutoKg.Text - (tbCGradingKg.Text), ViewState("DigitQty"))
            If CFloat(tbCTBS.Text) <> 0 Then tbCBrutoBJR.Text = FormatFloat(tbCBrutoKg.Text / tbCTBS.Text, ViewState("DigitQty"))
            If CFloat(tbCTBS.Text) <> 0 Then tbcNettoBjr.Text = FormatFloat(tbCNettoKg.Text / tbCTBS.Text, ViewState("DigitQty"))
            If tbCBrutoKg.Text = "0" Then Exit Sub
            If tbCPrice.Text = "0" Then Exit Sub
            tbCttlGrading.Text = FormatFloat((tbCBrutoKg.Text * tbCPrice.Text) - (tbCNettoKg.Text * tbCPabrik.Text), ViewState("DigitQty"))
            tbCPersenGrading.Text = FormatFloat((tbCGrading.Text / (tbCBrutoKg.Text * tbCPrice.Text)) * 100, ViewState("DigitQty"))
            tbCGradingKg.Text = FormatFloat((tbCBrutoKg.Text * tbCGrading.Text) / 100, ViewState("DigitQty"))
  
            tbCttlBongkar.Text = FormatFloat(tbCBongkar.Text * tbCNettoKg.Text, ViewState("DigitQty"))
            tbCttlMuat.Text = FormatFloat((tbCMuat.Text * tbCBrutoKg.Text), ViewState("DigitQty"))
            tbCttlKirim.Text = FormatFloat(tbCKirim.Text * tbCBrutoKg.Text, ViewState("DigitQty"))

        Catch ex As Exception
            Throw New Exception("LoadPriceExpedisi Error : " + ex.ToString)
        End Try
    End Sub


    Protected Sub tbCgrading_textChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCGrading.TextChanged
        Try
            If CFloat(tbCBrutoKg.Text) = 0 Then Exit Sub
            tbCGradingKg.Text = FormatFloat((tbCBrutoKg.Text * tbCGrading.Text) / 100, ViewState("DigitQty"))
            tbCNettoKg.Text = FormatFloat(tbCBrutoKg.Text - (tbCGradingKg.Text), ViewState("DigitQty"))

            If CFloat(tbCTBS.Text) <> 0 Then tbCBrutoBJR.Text = FormatFloat(tbCBrutoKg.Text / tbCTBS.Text, ViewState("DigitQty"))
            If CFloat(tbCTBS.Text) <> 0 Then tbcNettoBjr.Text = FormatFloat(tbCNettoKg.Text / tbCTBS.Text, ViewState("DigitQty"))

            tbCttlGrading.Text = FormatFloat((tbCBrutoKg.Text * tbCPrice.Text) - (tbCNettoKg.Text * tbCPabrik.Text), ViewState("DigitQty"))
            tbCPersenGrading.Text = FormatFloat((tbCGrading.Text / (tbCBrutoKg.Text * tbCPrice.Text)) * 100, ViewState("DigitQty"))


        Catch ex As Exception
            Throw New Exception("tbCgrading_textChange Error : " + ex.ToString)
        End Try
    End Sub


    Protected Sub btnComplete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnComplete.Click
        Dim SQLString As String
        Dim Result As String
        
        SQLString = "Declare @A VarChar(255) EXEC S_PLSPBComplete " + QuotedStr(tbRef.Text) + ", " + QuotedStr(tbCSpbManual.Text) + "," + QuotedStr(Format(tbCComplateDate.SelectedDate, "yyyy-MM-dd")) + ", '',  " + QuotedStr(Format(tbCTiba.SelectedDate, "yyyy-MM-dd")) + ", " + QuotedStr(tbCJamTiba.Text) + "," + QuotedStr(Format(tbCtglBongkar.SelectedDate, "yyyy-MM-dd")) + ", " + QuotedStr(tbCJamBongkar.Text) + "," + QuotedStr(ddlPabrik.SelectedValue) + ", " + QuotedStr(tbCBrutoKg.Text) + ", " + QuotedStr(tbCGrading.Text) + "," + QuotedStr(tbCGradingKg.Text) + ", " + QuotedStr(tbCNettoKg.Text) + "," + QuotedStr(tbCRemark.Text) + ", " + ViewState("GLYear").ToString + " , " + ViewState("GLPeriod").ToString + ", " + ViewState("UserId").ToString + ",   @A Out SELECT @A"
        Result = SQLExecuteScalar(SQLString, ViewState("DBConnection"))
        If Result.Length > 5 Then
            lbStatus.Text = Result
            Exit Sub
        End If
        MovePanel(pnlEditDt, pnlDt)

        btnCacelCom_Click(Nothing, Nothing)
    End Sub

    Sub LoadPriceTBS(ByVal StatusTanam As String)
        Dim Dt As DataTable
        Dim Dr As DataRow
        Dim SQLString As String
        Try
            SQLString = "EXEC S_FindUmurTBSPrice " + QuotedStr(StatusTanam) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd"))
            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)

            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbCPrice.Text = FormatFloat(Dr("PriceTBS").ToString, ViewState("DigitQty"))
                tbCPabrik.Text = FormatFloat(Dr("PriceTBS").ToString, ViewState("DigitQty"))
            Else
                tbCPrice.Text = "0"
                tbCPabrik.Text = "0"
            End If
            AttachScript("setformatdt()", Page, Me.GetType())
            tbCPabrik.Focus()
        Catch ex As Exception
            Throw New Exception("LoadPriceTBS Error : " + ex.ToString)
        End Try
    End Sub

    Sub LoadPriceExpedisi(ByVal Customer As String)
        Dim Dt As DataTable
        Dim Dr As DataRow
        Dim SQLString As String
        Try
            SQLString = "EXEC S_FindExpedisiPrice " + QuotedStr(Customer) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd"))
            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)

            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbCKirim.Text = FormatFloat(Dr("Price2").ToString, ViewState("DigitQty"))
                tbCttlKirim.Text = FormatFloat(Dr("Price2").ToString * CFloat(tbCBrutoKg.Text), ViewState("DigitQty"))
            Else
                tbCKirim.Text = "0"
                tbCttlKirim.Text = "0"
            End If
            AttachScript("setformatdt()", Page, Me.GetType())
            tbCKirim.Focus()
        Catch ex As Exception
            Throw New Exception("LoadPriceExpedisi Error : " + ex.ToString)
        End Try
    End Sub
    Sub LoadCarPrice(ByVal Car As String)
        Dim Dt As DataTable
        Dim Dr As DataRow
        Dim SQLString As String
        Try
            SQLString = "EXEC S_FindCarPrice " + QuotedStr(Car) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd"))
            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)

            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbCTrid.Text = FormatFloat(Dr("Price2").ToString, ViewState("DigitQty"))
            Else
                tbCTrid.Text = "0"
            End If
            AttachScript("setformatdt()", Page, Me.GetType())
            tbCTrid.Focus()
        Catch ex As Exception
            Throw New Exception("LoadCarPrice Error : " + ex.ToString)
        End Try
    End Sub
    Sub LoadingPrice()
        Dim Dt As DataTable
        Dim Dr As DataRow
        Dim SQLString As String
        Try
            SQLString = "EXEC S_FindLoadingPrice " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd"))
            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)

            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbCBongkar.Text = FormatFloat(Dr("PriceBongkar").ToString, ViewState("DigitQty"))
                tbCMuat.Text = FormatFloat(Dr("PriceMuat").ToString, ViewState("DigitQty"))
                tbCttlBongkar.Text = FormatFloat(Dr("PriceBongkar").ToString * CFloat(tbCNettoKg.Text), ViewState("DigitQty"))
                tbCttlMuat.Text = FormatFloat(Dr("PriceMuat").ToString * CFloat(tbCBrutoKg.Text), ViewState("DigitQty"))
                tbCttlKirim.Text = FormatFloat(CFloat(tbCKirim.Text) * CFloat(tbCBrutoKg.Text), ViewState("DigitQty"))
            Else
                tbCBongkar.Text = "0"
                tbCMuat.Text = "0"
                tbCttlBongkar.Text = "0"
                tbCttlMuat.Text = "0"
                tbCttlKirim.Text = "0"
            End If
            AttachScript("setformatdt()", Page, Me.GetType())
            tbCKirim.Focus()
        Catch ex As Exception
            Throw New Exception("LoadCarPrice Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbCGradingKg_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCGradingKg.TextChanged
        Try
            If tbCBrutoKg.Text = "0" Then Exit Sub
            tbCGrading.Text = FormatFloat((tbCGradingKg.Text / tbCBrutoKg.Text) * 100, ViewState("DigitQty"))
            tbCNettoKg.Text = FormatFloat(tbCBrutoKg.Text - (tbCGradingKg.Text), ViewState("DigitQty"))

            If tbCTBS.Text <> 0 Then tbCBrutoBJR.Text = FormatFloat(tbCBrutoKg.Text / tbCTBS.Text, ViewState("DigitQty"))
            If tbCTBS.Text <> 0 Then tbcNettoBjr.Text = FormatFloat(tbCNettoKg.Text / tbCTBS.Text, ViewState("DigitQty"))

            tbCttlGrading.Text = FormatFloat(tbCBrutoKg.Text * tbCPrice.Text - tbCNettoKg.Text * tbCPabrik.Text, ViewState("DigitQty"))
            tbCPersenGrading.Text = FormatFloat((tbCGrading.Text / (tbCBrutoKg.Text * tbCPrice.Text)) * 100, ViewState("DigitQty"))

            tbCGradingKg.Focus()
        Catch ex As Exception
            lbStatus.Text = "tbcGradingKg_textChange Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbCPabrik_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCPabrik.TextChanged
        Try
            If tbCPrice.Text = "0" Then Exit Sub
            If tbCPabrik.Text = "0" Then Exit Sub
            tbCttlPlesMin.Text = FormatFloat(tbCPabrik.Text - tbCPrice.Text, ViewState("DigitQty"))
            tbCttlPersen.Text = FormatFloat((tbCttlPlesMin.Text / tbCPrice.Text) * 100, ViewState("DigitQty"))

            If tbCBrutoKg.Text = 0 Then Exit Sub
            tbCttlGrading.Text = FormatFloat(tbCBrutoKg.Text * tbCPrice.Text - tbCNettoKg.Text * tbCPabrik.Text, ViewState("DigitQty"))
            tbCPersenGrading.Text = FormatFloat((tbCttlGrading.Text / (tbCBrutoKg.Text * tbCPrice.Text)) * 100, ViewState("DigitQty"))
            tbCPabrik.Focus()
        Catch ex As Exception
            lbStatus.Text = "tbCPabrik_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Sub StatusTanam(ByVal StatusTanam As String)

        Try
            Dim Dt As DataTable
            Dim Dr As DataRow
            Try
                ' FillCombo(ddlstatusTanam, "EXEC S_GetUmurTBS", True, "Umur_TBS", "Umur_TBS_Name", ViewState("DBConnection"))
                Dt = SQLExecuteQuery("EXEC S_FindUmurTBS " + QuotedStr(StatusTanam) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")), ViewState("DBConnection").ToString).Tables(0)
                If Dt.Rows.Count > 0 Then
                    Dr = Dt.Rows(0)
                    '  MessageDlg("A")
                    tbCStatusTanam.Text = Dr("Umur_TBS_Name").ToString
                    ddlPabrik.SelectedValue = Dr("Umur_TBS").ToString
                    tbCPrice.Text = FormatFloat(Dr("PriceTBS").ToString, ViewState("DigitQty"))
                    tbCPabrik.Text = FormatFloat(Dr("PriceTBS").ToString, ViewState("DigitQty"))

                Else
                    tbCStatusTanam.Text = ""
                    ddlPabrik.SelectedValue = ""
                    tbCPrice.Text = "0"
                    tbCPabrik.Text = "0"

                End If
                tbCPabrik_TextChanged(Nothing, Nothing)
                AttachScript("setformatdt()", Page, Me.GetType())
                ddlPabrik.Focus()
            Catch ex As Exception
                Throw New Exception("ddlstatusTanam_TextChanged Error : " + ex.ToString)
            End Try
        Catch ex As Exception
            lbStatus.Text = "StatusTanam Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub ddlstatusTanam_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlstatusTanam.SelectedIndexChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try
            ' FillCombo(ddlstatusTanam, "EXEC S_GetUmurTBS", True, "Umur_TBS", "Umur_TBS_Name", ViewState("DBConnection"))
            Dt = SQLExecuteQuery("EXEC S_FindUmurTBS " + QuotedStr(ddlstatusTanam.SelectedValue) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")), ViewState("DBConnection").ToString).Tables(0)
            'Dr = FindMaster("Product", tbProductCode.Text, ViewState("DBConnection").ToString)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                ' MessageDlg("A")
                '  ddlstatusTanam.SelectedValue.Text = Dr("Umur_TBS").ToString
                tbPriceTPS.Text = Dr("PriceTBS").ToString
                tbFactorRate.Text = Dr("FactorRate").ToString
                tbBJR.Text = Dr("BJR").ToString
            Else
                tbPriceTPS.Text = "0"
                tbFactorRate.Text = "0"
                tbBJR.Text = ""
            End If
            AttachScript("setformatdt()", Page, Me.GetType())
            ddlstatusTanam.Focus()
        Catch ex As Exception
            Throw New Exception("ddlstatusTanam_TextChanged Error : " + ex.ToString)
        End Try
    End Sub
End Class
