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

Partial Class TrSptOut
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter

    Private Function GetStringHd() As String
        Return "Select * From V_PLSPTOutHd " 'WHERE UserPrep = '" + ViewState("UserId") + "' "

    End Function


    'Protected GetStringHd As String = "Select * From V_PLSPTBSHD WHERE UserPrep = " + ViewState("UserId") + ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                FillCombo(ddlTujuan, "Select TPH, TPH From V_Tujuan ", True, "TPH", "TPH", ViewState("DBConnection"))
                'FillCombo(ddlTPH, "EXEC S_GetTPH ", True, "TPH", "TPH", ViewState("DBConnection"))
                ' FillCombo(ddlDivision, "EXEC S_GetDivisionForSAUserDivision " + QuotedStr(ViewState("UserId")) + ", 'SPTBS'", True, "DivisionCode", "DivisionName", ViewState("DBConnection"))
                SetInit()
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
                If ViewState("Sender") = "btnCustomer" Then
                    BindToText(tbCustomer, Session("Result")(0).ToString)
                    BindToText(tbCustomerName, Session("Result")(1).ToString)
                End If

                If ViewState("Sender") = "btnCar" Then
                    BindToText(TbCar, Session("Result")(0).ToString)
                    BindToText(TbCarName, Session("Result")(1).ToString)
                End If

                If ViewState("Sender") = "btnOperator" Then
                    tbOperator.Text = Session("Result")(0).ToString
                    BindToText(tbOperatorrName, Session("Result")(1).ToString)
                End If

                If ViewState("Sender") = "btnBM" Then
                    tbBM.Text = Session("Result")(0).ToString
                    BindToText(tbBMName, Session("Result")(1).ToString)
                End If

                If ViewState("Sender") = "btnDo" Then
                    BindToText(tbNoDo, Session("Result")(0).ToString)
                    BindToText(tbNoSo, Session("Result")(1).ToString)
                    BindToText(tbCustomer, Session("Result")(2).ToString)
                    BindToText(tbCustomerName, Session("Result")(3).ToString)
                    BindToText(tbItemType, Session("Result")(6).ToString)
                    BindToText(tbItemTypeName, Session("Result")(7).ToString)
                    BindToText(tbContractNo, Session("Result")(8).ToString)
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
                            Result = ExecSPCommandGo("Delete", "S_PLSPTOut", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)

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

        'Proteksi agar hanya angka saja yang bisa di input
        ' tbTimbang1.Attributes.Add("OnKeyDown", "return PressNumeric();")
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

                        If GVR.Cells(3).Text <> "P" Then
                            lbStatus.Text = MessageDlg("All Data Must Be Posting First to Print or Select Data with status posting!")
                            Exit Sub
                        End If
                    End If
                Next
                Result = Result + "'"
                Session("SelectCommand") = "EXEC S_PLFormSPTOut" + Result
                'lbStatus.Text = Session("SelectCommand")
                'Exit Sub
                Session("ReportFile") = ".../../../Rpt/FormSPTOut.frx"
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
                        Result = ExecSPCommandGo(ActionValue, "S_PLSPTOut", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            'Save Hd
            If ViewState("StateSave") = "Insert" Then
                tbRef.Text = GetAutoNmbr("SPTOUT", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
                'lbStatus.Text = tbRef.Text
                'Exit Sub
                SQLString = "INSERT INTO PLSPTOutHd (TransNmbr, Status, TransDate, Do_No, So_No, Customer, Car, Operator, " + _
                "ItemType, BM, Remark, ContractNo, TphTujuan, FgHariHitam, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbRef.Text) + ", 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "',  " + _
                QuotedStr(tbNoDo.Text) + "," + QuotedStr(tbNoSo.Text) + ", " + QuotedStr(tbCustomer.Text) + ", " + _
                QuotedStr(TbCar.Text) + "," + QuotedStr(tbOperator.Text) + "," + QuotedStr(tbItemType.Text) + "," + QuotedStr(tbBM.Text) + "," + QuotedStr(tbRemark.Text) + "," + _
                QuotedStr(tbContractNo.Text) + "," + QuotedStr(ddlTujuan.SelectedValue) + "," + QuotedStr(ddlFgHitam.SelectedValue) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", getDate()"
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM PLSPTOutHd WHERE TransNmbr = " + QuotedStr(tbRef.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If

                SQLString = "UPDATE PLSPTOutHd SET Do_No = " + QuotedStr(tbNoDo.Text) + _
                ", So_No = " + QuotedStr(tbNoSo.Text) + ", Customer = " + QuotedStr(tbCustomer.Text) + ", Car = " + QuotedStr(TbCar.Text) + _
                ", Remark = " + QuotedStr(tbRemark.Text) + ",Operator = " + QuotedStr(tbOperator.Text) + _
                ",ItemType = " + QuotedStr(tbItemType.Text) + _
                ",BM = " + QuotedStr(tbBM.Text) + _
                ", TransDate = " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + _
                " WHERE TransNmbr = " + QuotedStr(tbRef.Text)
            End If

            SQLString = SQLString.Replace("'1900-01-01'", "NULL")
            SQLString = SQLString.Replace("'0001-01-01'", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)





            ''update Primary Key on Dt
            'Dim Row As DataRow()

            'Row = ViewState("Dt").Select("TransNmbr IS NULL")
            'For I = 0 To Row.Length - 1
            '    Row(I).BeginEdit()
            '    Row(I)("TransNmbr") = tbRef.Text
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
            If CekHd() = False Then
                Exit Sub
            End If
            'If IsNothing(ViewState("Dt")) Then
            '    lbStatus.Text = MessageDlg("Detail must have at least 1 record")
            '    Exit Sub
            'End If

            'For Each dr In ViewState("Dt").Rows
            '    If CekDt(dr, "Item") = False Then
            '        Exit Sub
            '    End If
            'Next
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

    Private Sub EnableHd(ByVal State As Boolean)
        Try
            'ddlDivision.Enabled = State


        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Dim TglHitam As String
        Try
            MovePanel(PnlHd, pnlInput)
            'ModifyInput2(True, pnlInput, pnlDt, GridDt)
            newTrans()
            'lbItemNo.Text = GetNewItemNo(ViewState("Dt"))
            btnHome.Visible = False
            tbDate.Focus()
            EnableHd(True)

            TglHitam = SQLExecuteScalar("SELECT * FROM  VMsHoliday WHERE Holiday_date = " + QuotedStr(tbDate.SelectedDateFormatted) + "", ViewState("DBConnection"))
            If tbDate.SelectedDateFormatted = TglHitam Then
                ddlFgHitam.SelectedValue = "Y"
            Else
                ddlFgHitam.SelectedValue = "N"
            End If

        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
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
            'BindDataDt("")
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub


    Private Sub ClearHd()
        Dim SQLString As String
        Try
            tbRef.Text = ""
            tbNoDo.Text = ""
            tbNoSo.Text = ""
            tbCustomer.Text = ""
            tbCustomerName.Text = ""
            TbCar.Text = ""
            TbCarName.Text = ""
            tbOperator.Text = ""
            tbOperatorrName.Text = ""
            tbDate.SelectedDate = ViewState("ServerDate") 'Today
            tbItemType.Text = ""
            tbItemTypeName.Text = ""
            tbBM.Text = ""
            tbBMName.Text = ""
            tbRemark.Text = ""
            tbContractNo.Text = ""
            ddlTujuan.SelectedValue = ""

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

            If tbNoDo.Text = "" Then
                lbStatus.Text = MessageDlg("No DO must have value")
                tbNoDo.Focus()
                Return False
            End If
            If tbNoSo.Text = "" Then
                lbStatus.Text = MessageDlg("No SO must have value")
                tbNoSo.Focus()
                Return False
            End If
            If tbCustomer.Text = "" Then
                lbStatus.Text = MessageDlg(" Customer must have value")
                tbCustomer.Focus()
                Return False
            End If
            If TbCar.Text = "" Then
                lbStatus.Text = MessageDlg("Car must have value")
                TbCar.Focus()
                Return False
            End If

            If tbItemType.Text = "" Then
                lbStatus.Text = MessageDlg("Item Type must have value")
                tbItemType.Focus()
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
                        'EnableHd(Not DtExist())
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

                        If GVR.Cells(3).Text <> "P" Then
                            lbStatus.Text = MessageDlg("Data Must Be Posting First to Print!")
                            Exit Sub
                        End If

                        Session("SelectCommand") = "EXEC S_PLFormSPTOut ''" + QuotedStr(GVR.Cells(2).Text) + "''"
                        'Session("SelectCommand") = "EXEC S_STFormIssueReq ''" + QuotedStr(GVR.Cells(2).Text) "''"
                        'Session("ReportFile") = ".../../../Rpt/FormSPTOut2.frx"
                        Session("ReportFile") = ".../../../Rpt/FormSPTOut.frx"
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
            tbRef.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToText(tbNoDo, Dt.Rows(0)("Do_No").ToString)
            BindToText(tbNoSo, Dt.Rows(0)("So_No").ToString)
            BindToText(tbCustomer, Dt.Rows(0)("Customer").ToString)
            BindToText(tbCustomerName, Dt.Rows(0)("CustomerName").ToString)
            BindToText(TbCar, Dt.Rows(0)("Car").ToString)
            BindToText(TbCarName, Dt.Rows(0)("CarName").ToString)
            BindToText(tbOperator, Dt.Rows(0)("Operator").ToString)
            BindToText(tbOperatorrName, Dt.Rows(0)("OperatorName").ToString)
            BindToText(tbItemType, Dt.Rows(0)("ItemType").ToString)
            BindToText(tbItemTypeName, Dt.Rows(0)("ItemTypeName").ToString)
            BindToText(tbBM, Dt.Rows(0)("BM").ToString)
            BindToText(tbBMName, Dt.Rows(0)("BMName").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
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
    '        'Dt = BindDataTransaction(GetStringDt(tbRef.Text), "ItemNo = " + ItemNo, ViewState("DBConnection").ToString)
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


    Protected Sub btnCustomer_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCustomer.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT Customer_Code, Customer_Name FROM V_MsCustomer "
            ResultField = "Customer_Code, Customer_Name"
            ViewState("Sender") = "btnCustomer"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Cust Error : " + ex.ToString
        End Try
    End Sub



    Protected Sub btnDO_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDO.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT TransNmbr, Customer, Customer_Name,SoNo,JenisProduct,JenisProductName,Product,ProductName,ContractNo  FROM V_MKDOHd Where status = 'P' "
            ResultField = "TransNmbr,SoNo, Customer, Customer_Name, JenisProduct, JenisProductName, Product, ProductName, ContractNo"
            ViewState("Sender") = "btnDo"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Cust Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnCar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCar.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT Car_No, Car_Name FROM V_MsCar "
            ResultField = "Car_No, Car_Name"
            ViewState("Sender") = "btnCar"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Cust Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnOperator_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOperator.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT Emp_No, Emp_Name FROM V_MsEmployee " 'WHERE Emp_No = " + QuotedStr(tbOp.Text)
            ResultField = "Emp_No, Emp_Name"
            ViewState("Sender") = "btnOperator"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Cust Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnBM_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBM.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT Emp_No, Emp_Name FROM V_MsEmployee " 'WHERE Emp_No = " + QuotedStr(tbOp.Text)
            ResultField = "Emp_No, Emp_Name"
            ViewState("Sender") = "btnBM"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Cust Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub tbDate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbDate.SelectionChanged
        Dim TglHitam As String
        Try
            TglHitam = SQLExecuteScalar("SELECT Holiday_date FROM  VMsHoliday WHERE Holiday_date = " + QuotedStr(tbDate.SelectedDateFormatted) + "", ViewState("DBConnection"))
            'lbStatus.Text = TglHitam + "_" + tbDate.SelectedDateFormatted
            ''Exit Sub
            If tbDate.SelectedDateFormatted = TglHitam Then
                ddlFgHitam.SelectedValue = "Y"
            Else
                ddlFgHitam.SelectedValue = "N"
            End If
        Catch ex As Exception
            lbStatus.Text = "tbDate_SelectionChanged Error : " + ex.ToString
        End Try
    End Sub

    'Label untuk direct ke file lain
    'Protected Sub lbTKPanen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbTKPanen.Click
    '    Try
    '        ViewState("InputProduct") = "Y"
    '        AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsTeam')();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lbStatus.Text = "lb Product Error : " + ex.ToString
    '    End Try
    'End Sub

End Class
