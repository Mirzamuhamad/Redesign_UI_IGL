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

Partial Class Transaction_TrSptIn_TrSptIn
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter

    Private Function GetStringHd() As String
        Return "Select * From V_PLSPTINHd " 'WHERE UserPrep = '" + ViewState("UserId") + "' "

    End Function


    'Protected GetStringHd As String = "Select * From V_PLSPTBSHD WHERE UserPrep = " + ViewState("UserId") + ""


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                FillCombo(ddlTPH, "SELECT TPH, TPH FROM MsTPH WHERE FgSPBL = 'N'", True, "TPH", "TPH", ViewState("DBConnection"))
                FillCombo(ddlDivision, "EXEC S_GetDivisionForSAUserDivision " + QuotedStr(ViewState("UserId")) + ", 'SPTBS'", True, "DivisionCode", "DivisionName", ViewState("DBConnection"))
                FillCombo(ddlTphDt, "SELECT * FROM MsTPH WHERE FgSPBL = 'Y'", True, "TPH", "TPH", ViewState("DBConnection"))
                FillCombo(ddlVechile, "SELECT * FROM V_MsIdentitasIKP ", True, "IdentitasIKPCode", "IdentitasIKPName", ViewState("DBConnection"))
                ' FillCombo(ddlBlock, "EXEC S_GetBlockTahunPanen " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ",' AND Division = '" + QuotedStr(ddlDivision.SelectedValue) + "''", True, "BlockCode", "BlockName", ViewState("DBConnection"))
                FillCombo(ddlBlock, "Select Block_Code,Block_Code+', '+Block_Name+', '+DivisionName+', '+KsuBlock AS BlockName FROM V_MsBlock WHERE FgPanen ='Y' ", True, "Block_Code", "BlockName", ViewState("DBConnection"))
                '=============================================================
                'Dim GVR As GridViewRow

                'For Each GVR In GridDt.Rows
                '    'Untuk protek jika KSU blocknya  beda
                '    If GetCountRecord(ViewState("Dt")) <> 0 Then
                '        Dim ksuGrid As String
                '        ksuGrid = SQLExecuteScalar("SELECT KSUBlock FROM MsBlock WHERE BlockCode = '" + GVR.Cells(5).Text + "'", ViewState("DBConnection").ToString)

                '        lbStatus.Text = GVR.Cells(5).Text
                '        'Exit Sub
                '        FillCombo(ddlBlock, "Select BlockCode, BlockName FROM MsBlock Where KSUBlock = " + QuotedStr(ksuGrid), True, "Block_Code", "Block_Name", ViewState("DBConnection"))
                '    Else
                '        FillCombo(ddlBlock, "EXEC S_GetBlockTahunPanen " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ",' AND Division = '" + QuotedStr(ddlDivision.SelectedValue) + "''", True, "BlockCode", "BlockName", ViewState("DBConnection"))
                '    End If
                '    '===================
                'Next

                aturDGV()

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
                    BindToText(tbCar, Session("Result")(0).ToString)
                    BindToText(tbCarName, Session("Result")(1).ToString)
                End If

                If ViewState("Sender") = "btnBM" Then
                    tbBM.Text = Session("Result")(0).ToString
                    BindToText(tbBMName, Session("Result")(1).ToString)
                End If

                If ViewState("Sender") = "btnBMT" Then
                    tbBMT.Text = Session("Result")(0).ToString
                    BindToText(tbBMTName, Session("Result")(1).ToString)
                End If


                If ViewState("Sender") = "btnOp" Then
                    tbOp.Text = Session("Result")(0).ToString
                    BindToText(tbOpName, Session("Result")(1).ToString)
                End If

                If ViewState("Sender") = "btnCarT" Then
                    BindToText(tbCarT, Session("Result")(0).ToString)
                    BindToText(tbCarNameT, Session("Result")(1).ToString)
                End If

                If ViewState("Sender") = "btnOPT" Then
                    tbOpT.Text = Session("Result")(0).ToString
                    BindToText(tbOPNameT, Session("Result")(1).ToString)
                End If

                If ViewState("Sender") = "btnTKPanen" Then
                    tbCode.Text = Session("Result")(0).ToString
                    BindToText(tbName, Session("Result")(1).ToString)
                    BindToText(tbPerson, Session("Result")(2).ToString)
                End If

                If ViewState("Sender") = "btnSupplier" Then
                    tbSuppcode.Text = Session("Result")(0).ToString
                    BindToText(tbSuppName, Session("Result")(1).ToString)
                    tbPersonDt2.Text = 0
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
            GridDt2.Columns(1).Visible = False
            GridDt2.Columns(4).Visible = False
            GridDt2.Columns(5).Visible = False
            GridDt2.Columns(8).Visible = False
            GridDt2.Columns(9).Visible = False
            'GridDt2.Columns(10).Visible = False
            'GridDt2.Columns(11).Visible = False

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

        tbPanenHK.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbNormal.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbAbnormal.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbBrondolan.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbPanenHK.Attributes.Add("OnKeyDown", "return PressNumeric();")

        tbPanenHKDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbNormalDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbAbnormalDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbBrondolanDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbPanenHKDt2.Attributes.Add("OnKeyDown", "return PressNumeric();")

        'tbScan.Attributes.Add("OnKeyDown", "return PressNumeric();")

        tbPanenHK.Attributes.Add("OnBlur", "setformatdt();")
        tbNormal.Attributes.Add("OnBlur", "setformatdt();")
        tbAbnormal.Attributes.Add("OnBlur", "setformatdt();")
        tbBrondolan.Attributes.Add("OnBlur", "setformatdt();")

    End Sub

    Sub aturDGV()

        Dim GVR As GridViewRow
        For Each GVR In GridDt.Rows
            'Untuk protek jika KSU blocknya  beda

            lbStatus.Text = "Test"
            If GVR.Cells(17).Text = "Y" Then
                GridDt.RowStyle.BackColor = Color.LightBlue
                GridDt.AlternatingRowStyle.BackColor = Color.WhiteSmoke
            End If

            For Each iniCell As DataGridItem In GVR.Cells
                If GVR.Cells(17).Text = "Y" Then
                    GridDt.RowStyle.BackColor = Color.LightBlue

                End If
            Next


            '===================
        Next


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
        Return "SELECT * From V_PLSPTBSDt WHERE TransNmbr = " + QuotedStr(Nmbr) + " ORDER BY COALESCE(Manual_input,'N')  DESC "
    End Function

    Private Function GetStringDt2(ByVal Nmbr As String) As String
        Return "SELECT * From V_PLSPTBSDt2 WHERE TransNmbr = " + QuotedStr(Nmbr) + " ORDER BY COALESCE(Manual_input,'N')  DESC "
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
                'Session("SelectCommand") = "EXEC S_PLFormSPTBS" + Result
                Session("SelectCommand") = "EXEC S_PLFormSPTIN" + Result
                Session("ReportFile") = ".../../../Rpt/FormSPTIN.frx"
                'lbStatus.Text = Session("SelectCommand")
                'Exit Sub
                'Session("ReportFile") = ".../../../Rpt/FormSPTBS.frx"
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
                        Result = ExecSPCommandGo(ActionValue, "S_PLSPTBS", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            'ddlDivision.Enabled = State
            'btnCar.Enabled = State
            'btnOP.Enabled = State
            'tbCar.Enabled = State
            'tbOp.Enabled =State 
            'tbJam.Enabled = State
        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try


    End Sub

    Private Sub EnableDt2(ByVal State As Boolean)
        Try
            ddlVechile.Enabled = State
            tbSPBLManual.Enabled = State
            tbTPHDt2.Enabled = State
            tbSuppcode.Enabled = State
            btnSupplier.Visible = State
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

    Private Sub BindDataDt2(ByVal Referens As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt2") = Nothing
            dt = SQLExecuteQuery(GetStringDt2(Referens), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt2") = dt
            BindGridDt(dt, GridDt2)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnHome.Click
        Try
            MovePanel(pnlInput, PnlHd)
            Menu1.Items.Item(0).Enabled = True
            Menu1.Items.Item(1).Enabled = True
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




            Menu1.Items.Item(0).Enabled = True
            Menu1.Items.Item(1).Enabled = True
            If Not (ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit") Then
                Exit Sub
            Else
                ViewState("StateSave") = ViewState("StateHd")
                ViewState("StateHd") = "View"
            End If

            'Save Hd
            If ViewState("StateSave") = "Insert" Then
                tbRef.Text = GetAutoNmbr("SPTIN", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
                SQLString = "INSERT INTO PLSPTBSHd (TransNmbr, Status, TransDate, InputDate, CarNo, Operator, TPH, Division, " + _
                "DateAngkut, JamAngkut, FgTransit, CarNoTransit, OperatorTransit,BMTransit,BM, Remark, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbRef.Text) + ", 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', GetDate(), " + _
                QuotedStr(tbCar.Text) + "," + QuotedStr(tbOp.Text) + ", " + QuotedStr(ddlTPH.SelectedValue) + ", " + QuotedStr(ddlDivision.SelectedValue) + ", '" + _
                Format(tbAngkutDate.SelectedValue, "yyyy-MM-dd") + "', " + QuotedStr(Now.ToShortTimeString) + ", " + QuotedStr(ddlTransit.SelectedValue) + "," + _
                QuotedStr(tbCarT.Text) + "," + QuotedStr(tbOpT.Text) + "," + QuotedStr(tbBMT.Text) + "," + QuotedStr(tbBM.Text) + "," + QuotedStr(tbRemark.Text) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", getDate()"

            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM PLSPTBSHd WHERE TransNmbr = " + QuotedStr(tbRef.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE PLSPTBSHd SET TPH = " + QuotedStr(ddlTPH.SelectedValue) + _
                ", CarNo = " + QuotedStr(tbCar.Text) + ", Operator = " + QuotedStr(tbOp.Text) + ", Division = " + QuotedStr(ddlDivision.SelectedValue) + _
                ", FgTransit = " + QuotedStr(ddlTransit.SelectedValue) + ", CarNoTransit = " + QuotedStr(tbCarT.Text) + _
                ", OperatorTransit = " + QuotedStr(tbOpT.Text) + ", Remark = " + QuotedStr(tbRemark.Text) + _
                ", BMTransit = " + QuotedStr(tbBMT.Text) + _
                ", BM = " + QuotedStr(tbBM.Text) + _
                ", TransDate = " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", DateAppr = getDate()" + _
                ", DateAngkut = " + QuotedStr(Format(tbAngkutDate.SelectedValue, "yyyy-MM-dd")) + ", JamAngkut = " + QuotedStr(tbJam.Text) + _
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
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, WorkBy, Person, Blok, PanenHK, Ancak, SPTBSManual, " + _
            " SPTBSDate, JamSPTBS, FgHariHitam, Normal, Abnormal, Brondolan,Weight, Manual_Input, Remark FROM PLSPTBSDt WHERE TransNmbr = '" & ViewState("Reference") & "'", con)

            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim param As SqlParameter

            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
                    "UPDATE PLSPTBSDt SET WorkBy = @WorkBy, Person = @Person, Blok = @Blok, Remark = @Remark " + _
                    ", PanenHK = @PanenHK, Ancak = @Ancak, SPTBSManual = @SPTBSManual " + _
                    ", SPTBSDate = @SPTBSDate, JamSPTBS = @JamSPTBS, FgHariHitam = @FgHariHitam " + _
                    ", Abnormal = @Abnormal, Brondolan = @Brondolan, Weight = @Weight " + _
                    ", Normal = @Normal WHERE TransNmbr = '" & ViewState("Reference") & "' AND Ancak = @OldAncak AND Blok = @OldBlok AND SPTBSManual = @OldSPTBSManual", con)
            ' Define output parameters.
            Update_Command.Parameters.Add("@WorkBy", SqlDbType.VarChar, 5, "WorkBy")
            Update_Command.Parameters.Add("@Person", SqlDbType.Int, 4, "Person")
            Update_Command.Parameters.Add("@Blok", SqlDbType.VarChar, 5, "Blok")
            Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 60, "Remark")
            Update_Command.Parameters.Add("@PanenHK", SqlDbType.Float, 18, "PanenHK")
            Update_Command.Parameters.Add("@Ancak", SqlDbType.VarChar, 60, "Ancak")
            Update_Command.Parameters.Add("@SPTBSManual", SqlDbType.VarChar, 30, "SPTBSManual")
            Update_Command.Parameters.Add("@SPTBSDate", SqlDbType.DateTime, 8, "SPTBSDate")
            Update_Command.Parameters.Add("@JamSPTBS", SqlDbType.VarChar, 5, "JamSPTBS")
            Update_Command.Parameters.Add("@FgHariHitam", SqlDbType.VarChar, 1, "FgHariHitam")
            Update_Command.Parameters.Add("@Abnormal", SqlDbType.Int, 4, "Abnormal")
            Update_Command.Parameters.Add("@Brondolan", SqlDbType.Int, 4, "Brondolan")
            Update_Command.Parameters.Add("@Weight", SqlDbType.Int, 4, "Weight")
            Update_Command.Parameters.Add("@Normal", SqlDbType.Int, 4, "Normal")

            ' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@OldBlok", SqlDbType.VarChar, 5, "Blok")
            param.SourceVersion = DataRowVersion.Original
            param = Update_Command.Parameters.Add("@OldAncak", SqlDbType.VarChar, 60, "Ancak")
            param.SourceVersion = DataRowVersion.Original
            param = Update_Command.Parameters.Add("@OldSPTBSManual", SqlDbType.VarChar, 30, "SPTBSManual")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM PLSPTBSDt WHERE TransNmbr = '" & ViewState("Reference") & "' AND Ancak = @Ancak AND Blok = @Blok AND SPTBSManual = @SPTBSManual", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@Blok", SqlDbType.VarChar, 5, "Blok")
            param.SourceVersion = DataRowVersion.Original
            param = Delete_Command.Parameters.Add("@Ancak", SqlDbType.VarChar, 60, "Ancak")
            param.SourceVersion = DataRowVersion.Original
            param = Delete_Command.Parameters.Add("@SPTBSManual", SqlDbType.VarChar, 30, "SPTBSManual")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("PLSPTBSDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt

            'If Not ViewState("Dt2") Is Nothing Then
            'save dt2

            'lbStatus.Text = ViewState("Reference")

            'save dt2
            cmdSql = New SqlCommand("SELECT TransNmbr, SuppCode, Ancak, SPBLManual,VehicleType, " + _
            "SPBLDate, JamSPBL, Normal, Abnormal, Brondolan, Weight,Manual_Input, Remark FROM PLSPTBSDt2 WHERE TransNmbr = '" & ViewState("Reference") & "' ", con)
            da = New SqlDataAdapter(cmdSql)
            dbcommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand

            'Dim param2 As SqlParameter
            ''Create the UpdateCommand.
            'Dim Update_Command2 = New SqlCommand( _
            '        " UPDATE PLSPTBSDt2 SET SuppCode = @SuppCode, VehicleType = @VehicleType, Remark = @Remark " + _
            '        ", PanenHK = @PanenHK, Ancak = @Ancak, SPBLManual = @SPBLManual " + _
            '        ", SPBLDate = @SPBLDate, JamSPBL = @JamSPBL " + _
            '        ", Abnormal = @Abnormal, Brondolan = @Brondolan " + _
            '        ", Normal = @Normal,Weight = @Weight WHERE TransNmbr = '" & ViewState("Reference") & "' AND Ancak = @OldAncak AND SuppCode = @OldSuppCode AND SPBLManual = @OldSPBLManual", con)
            ''Define output parameters.
            'Update_Command2.Parameters.Add("@SuppCode", SqlDbType.VarChar, 20, "SuppCode")
            ''Update_Command2.Parameters.Add("@Person", SqlDbType.Int, 4, "Person")
            ''Update_Command2.Parameters.Add("@VehicleType", SqlDbType.VarChar, 10, "VehicleType")
            'Update_Command2.Parameters.Add("@Remark", SqlDbType.VarChar, 100, "Remark")
            'Update_Command2.Parameters.Add("@PanenHK", SqlDbType.Float, 18, "PanenHK")
            'Update_Command2.Parameters.Add("@Ancak", SqlDbType.VarChar, 60, "Ancak")
            'Update_Command2.Parameters.Add("@SPBLManual", SqlDbType.VarChar, 30, "SPBLManual")
            'Update_Command2.Parameters.Add("@SPBLDate", SqlDbType.DateTime, 8, "SPBLDate")
            'Update_Command2.Parameters.Add("@JamSPBL", SqlDbType.VarChar, 5, "JamSPBL ")
            'Update_Command2.Parameters.Add("@FgHariHitam", SqlDbType.VarChar, 1, "FgHariHitam")
            'Update_Command2.Parameters.Add("@Abnormal", SqlDbType.Float, 8, "Abnormal")
            'Update_Command2.Parameters.Add("@Brondolan", SqlDbType.Float, 8, "Brondolan")
            'Update_Command2.Parameters.Add("@Normal", SqlDbType.Float, 8, "Normal")
            'Update_Command2.Parameters.Add("@Weight", SqlDbType.Float, 8, "Weight")

            ''Define intput (WHERE) parameters.
            'param2 = Update_Command2.Parameters.Add("@OldSuppCode", SqlDbType.VarChar, 5, "SuppCode")
            'param2.SourceVersion = DataRowVersion.Original
            'param2 = Update_Command2.Parameters.Add("@OldAncak", SqlDbType.VarChar, 60, "Ancak")
            'param2.SourceVersion = DataRowVersion.Original
            'param2 = Update_Command2.Parameters.Add("@OldSPBLManual", SqlDbType.VarChar, 30, "SPBLManual")
            'param2.SourceVersion = DataRowVersion.Original
            ''Attach the update command to the DataAdapter.
            'da.UpdateCommand = Update_Command2

            ''reate the DeleteCommand.
            'Dim Delete_Command2 = New SqlCommand( _
            '    "DELETE FROM PLSPTBSDt2 WHERE TransNmbr = '" & ViewState("Reference") & "' AND Ancak = @Ancak AND SuppCode = @SuppCode AND SPBLManual = @SPBLManual", con)
            ' 'Add the parameters for the DeleteCommand.
            'param2 = Delete_Command2.Parameters.Add("@SuppCode", SqlDbType.VarChar, 5, "SuppCode")
            'param2.SourceVersion = DataRowVersion.Original
            'param2 = Delete_Command2.Parameters.Add("@Ancak", SqlDbType.VarChar, 60, "Ancak")
            'param2.SourceVersion = DataRowVersion.Original
            'param2 = Delete_Command2.Parameters.Add("@SPBLManual", SqlDbType.VarChar, 30, "SPBLManual")
            'param2.SourceVersion = DataRowVersion.Original
            'da.DeleteCommand = Delete_Command2

            Dim Dt2 As New DataTable("PLSPTBSDt2")

            Dt2 = ViewState("Dt2")
            da.Update(Dt2)
            Dt2.AcceptChanges()
            ViewState("Dt2") = Dt2
            'End If

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


            If GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0 Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
                Exit Sub
            End If

            'For Each dr In ViewState("Dt").Rows
            '    If CekDt(dr, "blok,Ancak,SPTBSManual") = False Then
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

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try

            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, pnlDt, GridDt)
            ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
            newTrans()
            btnHome.Visible = False
            MultiView1.ActiveViewIndex = 0
            Menu1.Items.Item(0).Selected = True
            tbDate.Focus()
            EnableHd(True)
            DisableHD()
            tbScan.Focus()
            ddlDivision.Enabled = False

            'tbRef.Text = GetAutoNmbr("SPTIN", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
            'lbStatus.Text = tbRef.Text


        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnAddDtKe1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddDtKe1.Click, btnAddDtKe2.Click
        Try
            Cleardt2()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt2") = "Insert"
            MovePanel(pnlDt2, pnlEditDt2)
            EnableHd(False)
            EnableDt2(True)
            StatusButtonSave(False)
            btnSaveDt2.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Dt 2 Error : " + ex.ToString
        End Try
    End Sub



    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ViewState("DtRemark") = ""
            ddlTransit_SelectedIndexChanged(Nothing, Nothing)
            ClearHd()
            Cleardt()
            Cleardt2()
            pnlDt.Visible = True
            GridDt.Columns(1).Visible = False
            GridDt2.Columns(1).Visible = False
            BindDataDt("")
            BindDataDt2("")
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub EnableHD()

        Try
            tbRef.Enabled = False
            tbCar.Enabled = False
            tbCarName.Enabled = False
            'tbCarT.Enabled = False
            'tbCarNameT.Enabled = False
            tbOp.Enabled = False
            tbOpName.Enabled = False
            'tbOpT.Enabled = False
            'tbBMT.Enabled = False
            'tbBMTName.Enabled = False
            'tbOPNameT.Enabled = False
            tbBM.Enabled = False
            tbBMName.Enabled = False
            ddlTransit.Enabled = False
            ddlTPH.Enabled = False
            tbAngkutDate.Enabled = False
            tbDate.Enabled = False
            tbSuppCek.Enabled = False
            tbJam.Enabled = False
            ddlDivision.Enabled = False
            btnCar.Enabled = False
            btnOP.Enabled = False
            btnBM.Enabled = False

        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub DisableHD()

        Try
            tbRef.Enabled = True
            tbCar.Enabled = True
            tbCarName.Enabled = True
            'tbCarT.Enabled = True
            'tbCarNameT.Enabled = True
            tbOp.Enabled = True
            tbOpName.Enabled = True
            'tbOpT.Enabled = True
            'tbBMT.Enabled = True
            'tbBMTName.Enabled = True
            tbOPNameT.Enabled = True
            tbBM.Enabled = True
            tbBMName.Enabled = True
            ddlTransit.Enabled = True
            ddlTPH.Enabled = True
            tbAngkutDate.Enabled = True
            tbDate.Enabled = True
            tbSuppCek.Enabled = True
            tbJam.Enabled = True
            ddlDivision.Enabled = False
            btnCar.Enabled = True
            btnOP.Enabled = True
            btnBM.Enabled = True
            'btnCarT.Enabled = True
            'btnOPT.Enabled = True
            'btnBMT.Enabled = True

        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Dim SQLString As String
        Try
            tbRef.Text = ""
            tbCar.Text = ""
            tbCarName.Text = ""
            tbCarT.Text = ""
            tbCarNameT.Text = ""
            tbOp.Text = ""
            tbOpName.Text = ""
            tbOpT.Text = ""
            tbBMT.Text = ""
            tbBMTName.Text = ""
            tbOPNameT.Text = ""
            tbBM.Text = ""
            tbBMName.Text = ""
            ddlTransit.SelectedValue = "N"
            ddlTPH.SelectedValue = ""
            tbAngkutDate.SelectedValue = ViewState("ServerDate") 'Today
            tbDate.SelectedDate = ViewState("ServerDate") 'Today
            tbSuppCek.Text = ""
            tbJam.Text = Now.ToShortTimeString
            tbRemark.Text = ""
            'SqlString = SQLExecuteScalar(" EXEC S_GetDivisionForSAUserDivision " + QuotedStr(ViewState("UserId")) + ", '1'", ViewState("DBConnection").ToString)
            Dim Division As String
            Division = SQLExecuteScalar("EXEC S_GetDivisionForSAUserDivision " + QuotedStr(ViewState("UserId")) + ", '1'", ViewState("DBConnection"))

            ddlDivision.SelectedValue = Division
            ddlDivision_SelectedIndexChanged(Nothing, Nothing)
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub Cleardt()
        Try
            tbCode.Text = ""
            tbName.Text = ""
            tbPerson.Text = "0"
            tbTPH.Text = ""
            tbSPTBManual.Text = ""
            tbNormal.Text = "0"
            tbAbnormal.Text = "0"
            tbBrondolan.Text = "0"
            tbWeightDt.Text = "0"
            tbJamSPTBS.Text = ""
            tbSPTBSDate.SelectedDate = Today.Date
            tbRemarkDt.Text = ViewState("DtRemark")
            tbPanenHK.Text = "0"
            ddlBlock.SelectedValue = ""
            ddlTphDt.SelectedValue = ""
            ddlFgHariHitam.SelectedValue = "N"

        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub

    Private Sub Cleardt2()
        Try
            tbSuppcode.Text = ""
            tbSuppName.Text = ""
            tbPerson.Text = "0"
            tbTPHDt2.Text = ""
            tbSPBLManual.Text = ""
            tbNormalDt2.Text = "0"
            tbAbnormalDt2.Text = "0"
            tbBrondolanDt2.Text = "0"
            tbWeight.Text = "0"
            tbJamSPBL.Text = ""
            tbSPBLDate.SelectedDate = Today.Date
            tbRemarkDt2.Text = ViewState("DtRemark")
            tbPanenHKDt2.Text = "0"
            ddlVechile.SelectedValue = ""
            ddlFgHariHitamDt2.SelectedValue = "N"

        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        Dim index As Integer
        Try
            index = Int32.Parse(e.Item.Value)
            MultiView1.ActiveViewIndex = index
            btnSaveTrans.Focus()
            If MultiView1.ActiveViewIndex = 1 Then
                ddlDivision.SelectedValue = ""
            Else
                Dim Division As String
                Division = SQLExecuteScalar("EXEC S_GetDivisionForSAUserDivision " + QuotedStr(ViewState("UserId")) + ", '1'", ViewState("DBConnection"))

                ddlDivision.SelectedValue = Division
                ddlDivision_SelectedIndexChanged(Nothing, Nothing)
            End If
        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
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
            'For Each dr In ViewState("Dt").Rows
            '    If CekDt(dr, "blok,Ancak,SPTBSManual") = False Then
            '        Exit Sub
            '    End If
            'Next
            SaveAll()
            newTrans()
        Catch ex As Exception
            lbStatus.Text = "btn save all error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt.Click, btnAddDt2.Click
        Dim TglHitam As String

        Cleardt()
        If CekHd() = False Then
            Exit Sub
        End If

        TglHitam = SQLExecuteScalar("SELECT * FROM  VMsHoliday WHERE Holiday_date = " + QuotedStr(tbDate.SelectedDateFormatted) + "", ViewState("DBConnection"))
        If tbDate.SelectedDateFormatted = TglHitam Then
            ddlFgHariHitam.SelectedValue = "Y"
        Else
            ddlFgHariHitam.SelectedValue = "N"
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

            If ddlTPH.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Tujuan must have value")
                ddlTPH.Focus()
                Return False
            End If

            'If GetCountRecord(ViewState("Dt2")) = 0 Then
            '    If ddlDivision.SelectedValue = "" Then
            '        lbStatus.Text = MessageDlg("Division must have value")
            '        ddlDivision.Focus()
            '        Return False
            '    End If

            'End If



            'If Len(tbRemark.Text.Trim) > 60 Then
            '    lbStatus.Text = MessageDlg("Remark must have value or caracter must 60")
            '    tbRemark.Focus()
            '    Return False
            'End If

            If tbBM.Text = "" Then
                lbStatus.Text = MessageDlg("BM must have value")
                tbBM.Focus()
                Return False
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Hd Error : " + ex.ToString)
        End Try
    End Function


    Function CekDt(Optional ByVal Dr As DataRow = Nothing, Optional ByVal FieldKey As String = "") As Boolean
        Try
            If Not (Dr Is Nothing) Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("WorkBy").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("TK Panen Must Have Value")
                    Return False
                End If
                If CFloat(Dr("Person").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Person Must Have Value")
                    Return False
                End If
                If Dr("Blok").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Blok Must Have Value")
                    Return False
                End If
                If CFloat(Dr("PanenHK").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Hektar Panen Must Have Value")
                    Return False
                End If
                If Dr("Ancak").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("TPH Must Have Value")
                    Return False
                End If
                If Dr("SPTBSManual").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("SPTBS Manual Must Have Value")
                    Return False
                End If
                If CFloat(Dr("Normal").ToString) = 0 And CFloat(Dr("AbNormal").ToString) = 0 And CFloat(Dr("Brondolan").ToString) = 0 Then
                    lbStatus.Text = MessageDlg("Real Brondolan, Normal (TBS), AbNormal (TBS) must have value")
                    tbPerson.Focus()
                    Return False
                End If
                'If Dr("Remark").ToString.Trim = "" Then
                '    lbStatus.Text = MessageDlg("Remark Must Have Value")
                '    Return False
                'End If
            Else

                If tbCode.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("TK Panen Must Have Value")
                    tbCode.Focus()
                    Return False
                End If
                If CFloat(tbPerson.Text) < 0 Then
                    lbStatus.Text = MessageDlg("Person Must Have Value")
                    tbPerson.Focus()
                    Return False
                End If
                If ddlBlock.SelectedValue.Trim = "" Then
                    lbStatus.Text = MessageDlg("Blok  Must Have Value")
                    ddlBlock.Focus()
                    Return False
                End If
                If CFloat(tbPanenHK.Text) < 0 Then
                    lbStatus.Text = MessageDlg("Hektar Panen Must Have Value")
                    tbPanenHK.Focus()
                    Return False
                End If
                If ddlTphDt.SelectedValue.Trim = "" Then
                    lbStatus.Text = MessageDlg("TPH Must Have Value")
                    ddlTphDt.Focus()
                    Return False
                End If
                If tbSPTBManual.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("SPTBS Manual Must Have Value")
                    tbSPTBManual.Focus()
                    Return False
                End If
                If CFloat(tbNormal.Text) = 0 And CFloat(tbAbnormal.Text) = 0 And CFloat(tbBrondolan.Text) = 0 Then
                    lbStatus.Text = MessageDlg("Real Brondolan, Normal (TBS), AbNormal (TBS) must have value")
                    tbPerson.Focus()
                    Return False
                End If

                'If tbRemarkDt.Text.Trim = "" Then
                '    lbStatus.Text = MessageDlg("Remark Must Have Value")
                '    tbRemarkDt.Focus()
                '    Return False
                'End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function



    Function CekDt2(Optional ByVal Dr As DataRow = Nothing, Optional ByVal FieldKey As String = "") As Boolean
        Try
            If Not (Dr Is Nothing) Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("Supplier").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Supplier Must Have Value")
                    Return False
                End If

                If Dr("Ancak").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("TPH Must Have Value")
                    Return False
                End If

                If Dr("SPBLManual").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("SPBL Manual Must Have Value")
                    Return False
                End If
                If CFloat(Dr("Normal").ToString) < 0 And CFloat(Dr("AbNormal").ToString) < 0 And CFloat(Dr("Brondolan").ToString) < 0 Then
                    lbStatus.Text = MessageDlg("Real Brondolan, Normal (TBS), AbNormal (TBS) must have value")
                    tbPerson.Focus()
                    Return False
                End If
                'If Dr("Remark").ToString.Trim = "" Then
                '    lbStatus.Text = MessageDlg("Remark Must Have Value")
                '    Return False
                'End If
            Else

                If tbSuppcode.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("TK Panen Must Have Value")
                    tbSuppcode.Focus()
                    Return False
                End If

                If ddlVechile.SelectedValue.Trim = "" Then
                    lbStatus.Text = MessageDlg("Blok  Must Have Value")
                    ddlVechile.Focus()
                    Return False
                End If

                If CFloat(tbPanenHK.Text) < 0 Then
                    lbStatus.Text = MessageDlg("Hektar Panen Must Have Value")
                    tbPanenHK.Focus()
                    Return False
                End If
                If tbTPHDt2.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("TPH Must Have Value")
                    tbTPHDt2.Focus()
                    Return False
                End If

                If tbSPBLManual.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("SPBL Manual Must Have Value")
                    tbSPBLManual.Focus()
                    Return False
                End If
                If CFloat(tbNormalDt2.Text) < 0 And CFloat(tbAbnormalDt2.Text) < 0 And CFloat(tbBrondolanDt2.Text) < 0 Then
                    lbStatus.Text = MessageDlg("Real Brondolan, Normal (TBS), AbNormal (TBS) must have value")
                    tbPerson.Focus()
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
            FilterName = "Reference, Date, DateAngkut, Remark"
            FilterValue = "Reference, dbo.FormatDate(TransDate), dbo.FormatDate(DateAngkut), Remark"
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
                    If ViewState("Status") = "P" Then
                        GridDt.Columns(1).Visible = True
                    Else
                        GridDt.Columns(1).Visible = False
                    End If

                    Dim CekDt As String
                    CekDt = SQLExecuteScalar("SELECT TransNmbr FROM V_PLSPTBSDt WHERE TransNmbr = '" + ViewState("Reference") + "'", ViewState("DBConnection").ToString)
                    If CekDt = "" Then
                        MultiView1.ActiveViewIndex = 1
                        Menu1.Items.Item(1).Selected = True
                        Menu1.Items.Item(0).Enabled = False
                    Else
                        MultiView1.ActiveViewIndex = 0
                        Menu1.Items.Item(0).Selected = True
                        Menu1.Items.Item(1).Enabled = False
                    End If
                    GridDt.Columns(1).Visible = False
                    GridDt2.Columns(1).Visible = False
                    GridDt.PageIndex = 0
                    FillTextBoxHd(ViewState("Reference"))
                    BindDataDt(ViewState("Reference"))
                    BindDataDt2(ViewState("Reference"))
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, pnlDt2, GridDt2)
                    btnHome.Visible = True
                    EnableHd()
                    GridDt.Columns(0).Visible = False

                ElseIf DDL.SelectedValue = "Edit" Then
                    If GVR.Cells(3).Text = "H" Or GVR.Cells(3).Text = "G" Then
                        CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        MovePanel(PnlHd, pnlInput)
                        ViewState("Reference") = GVR.Cells(2).Text

                        Dim CekDt, Cekdt2 As String
                        CekDt = SQLExecuteScalar("SELECT TransNmbr FROM V_PLSPTBSDt WHERE TransNmbr = '" + ViewState("Reference") + "'", ViewState("DBConnection").ToString)
                        Cekdt2 = SQLExecuteScalar("SELECT TransNmbr FROM V_PLSPTBSDt2 WHERE TransNmbr = '" + ViewState("Reference") + "'", ViewState("DBConnection").ToString)
                        'lbStatus.Text = CekDt
                        'Exit Sub

                        If Cekdt2 = "" And CekDt = "" Then
                            MultiView1.ActiveViewIndex = 0
                            Menu1.Items.Item(0).Selected = True
                            Menu1.Items.Item(1).Enabled = True

                        ElseIf CekDt = "" Then
                            MultiView1.ActiveViewIndex = 1
                            Menu1.Items.Item(1).Selected = True
                            Menu1.Items.Item(0).Enabled = False

                        Else
                            MultiView1.ActiveViewIndex = 0
                            Menu1.Items.Item(0).Selected = True
                            Menu1.Items.Item(1).Enabled = False

                        End If

                        GridDt.Columns(1).Visible = False
                        GridDt2.Columns(1).Visible = False
                        GridDt.PageIndex = 0
                        BindDataDt(ViewState("Reference"))
                        FillTextBoxHd(ViewState("Reference"))
                        BindDataDt2(ViewState("Reference"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
                        btnHome.Visible = False
                        ddlDivision.Enabled = False
                        'EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                        EnableHd(Not DtExist())
                        DisableHD()
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
                        'Session("SelectCommand") = "EXEC S_PLFormSPTBS ''" + QuotedStr(GVR.Cells(2).Text) + "''"
                        Session("SelectCommand") = "EXEC S_PLFormSPTIN ''" + QuotedStr(GVR.Cells(2).Text) + "''"
                        Session("ReportFile") = ".../../../Rpt/FormSPTIN.frx"
                        'Session("ReportFile") = ".../../../Rpt/FormSPTBS.frx"
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
            ElseIf e.CommandName = "Closing" Then
                Dim GVR As GridViewRow
                GVR = GridDt.Rows(Convert.ToInt32(e.CommandArgument))
                If ViewState("Status") <> "P" Then
                    lbStatus.Text = MessageDlg("Status Transaction is not Post, cannot close TK Panen")
                    Exit Sub
                End If
                ViewState("ProductClose") = GVR.Cells(2).Text
                AttachScript("closing();", Page, Me.GetType)
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Dim dr() As DataRow

        Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
        dr = ViewState("Dt").Select("Blok+'|'+Ancak+'|'+SPTBSManual = " + QuotedStr(GVR.Cells(5).Text + "|" + GVR.Cells(8).Text + "|" + GVR.Cells(9).Text))
        dr(0).Delete()
        BindGridDt(ViewState("Dt"), GridDt)
        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
    End Sub

    Protected Sub GridDt2_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt2.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow = GridDt2.Rows(e.RowIndex)
            dr = ViewState("Dt2").Select("SuppCode+'|'+Ancak+'|'+SPBLManual = " + QuotedStr(GVR.Cells(2).Text + "|" + GVR.Cells(6).Text + "|" + GVR.Cells(7).Text))
            dr(0).Delete()
            BindGridDt(ViewState("Dt2"), GridDt2)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt 2 Row Deleting Error : " + ex.ToString
        End Try
    End Sub



    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            ViewState("DtValue") = GVR.Cells(5).Text + "|" + GVR.Cells(8).Text + "|" + GVR.Cells(9).Text
            'lbStatus.Text = ViewState("DtValue")
            'Exit Sub
            FillTextBoxDt(ViewState("DtValue"))
            'FillTextBoxDt(GVR.Cells(4).Text + "|" + GVR.Cells(7).Text + "|" + GVR.Cells(8).Text)
            'ViewState("DtValue") = GVR.Cells(4).Text + "|" + GVR.Cells(7).Text + "|" + GVR.Cells(8).Text
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            tbCode.Focus()
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub GridDt2_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt2.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt2.Rows(e.NewEditIndex)
            ViewState("Dt2Value") = GVR.Cells(2).Text + "|" + GVR.Cells(6).Text + "|" + GVR.Cells(7).Text
            'lbStatus.Text = ViewState("DtValue")
            'Exit Sub
            FillTextBoxDt2(ViewState("Dt2Value"))
            MovePanel(pnlDt2, pnlEditDt2)
            EnableHd(False)
            EnableDt2(False)
            ViewState("StateDt2") = "Edit"
            tbCode.Focus()

            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid dt2 Editing Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "Reference = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            'newTrans()
            tbRef.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToDate(tbAngkutDate, Dt.Rows(0)("DateAngkut").ToString)
            BindToDropList(ddlTPH, Dt.Rows(0)("TPH").ToString)
            BindToDropList(ddlDivision, Dt.Rows(0)("Division").ToString)
            BindToText(tbCar, Dt.Rows(0)("CarNo").ToString)
            BindToText(tbCarName, Dt.Rows(0)("CarName").ToString)
            BindToText(tbOp, Dt.Rows(0)("Operator").ToString)
            BindToText(tbOpName, Dt.Rows(0)("OperatorName").ToString)
            BindToText(tbBMT, Dt.Rows(0)("BMTransit").ToString)
            BindToText(tbBMTName, Dt.Rows(0)("BMTransitName").ToString)
            BindToText(tbCarT, Dt.Rows(0)("CarNoTransit").ToString)
            BindToText(tbCarNameT, Dt.Rows(0)("CarTransitName").ToString)
            BindToText(tbOpT, Dt.Rows(0)("OperatorTransit").ToString)
            BindToText(tbOPNameT, Dt.Rows(0)("OperatorTransitName").ToString)
            BindToText(tbJam, Dt.Rows(0)("JamAngkut").ToString)
            BindToText(tbBM, Dt.Rows(0)("BM").ToString)
            BindToText(tbBMName, Dt.Rows(0)("BMName").ToString)
            BindToDropList(ddlTransit, Dt.Rows(0)("FgTransit").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
            ddlDivision_SelectedIndexChanged(Nothing, Nothing)
            'FillCombo(ddlBlok , "EXEC S_GetCostCtrDept " + QuotedStr(ddlTPH.SelectedValue), True, "Cost_Ctr_Code", "Cost_Ctr_Name", ViewState("DBConnection"))
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal Product As String)
        Dim Dr As DataRow()
        Try

            Dr = ViewState("Dt").select("Blok+'|'+Ancak+'|'+SPTBSManual = " + QuotedStr(Product))
            If Dr.Length > 0 Then
                BindToText(tbCode, Dr(0)("WorkBy").ToString)
                BindToText(tbName, Dr(0)("WorkByName").ToString)
                BindToText(tbPerson, Dr(0)("Person").ToString)
                BindToDropList(ddlBlock, TrimStr(Dr(0)("Blok").ToString))
                BindToText(tbPanenHK, Dr(0)("PanenHK").ToString)
                BindToText(tbTPH, Dr(0)("Ancak").ToString)
                BindToText(tbSPTBManual, Dr(0)("SPTBSManual").ToString)
                BindToDate(tbSPTBSDate, Dr(0)("SPTBSDate").ToString)
                BindToText(tbJamSPTBS, Dr(0)("JamSPTBS").ToString)
                BindToDropList(ddlFgHariHitam, TrimStr(Dr(0)("FgHariHitam").ToString))
                BindToText(tbNormal, Dr(0)("Normal").ToString)
                BindToText(tbAbnormal, Dr(0)("Abnormal").ToString)
                BindToText(tbBrondolan, Dr(0)("Brondolan").ToString)
                BindToText(tbWeightDt, Dr(0)("Weight").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
            End If
            'Dt = BindDataTransaction(GetStringDt(tbRef.Text), "ItemNo = " + ItemNo, ViewState("DBConnection").ToString)
            'newTrans()
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub


    Protected Sub FillTextBoxDt2(ByVal Product As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt2").select("SuppCode+'|'+Ancak+'|'+SPBLManual = " + QuotedStr(Product))
            If Dr.Length > 0 Then
                BindToText(tbSuppcode, Dr(0)("SuppCode").ToString)
                BindToText(tbSuppName, Dr(0)("SupplierName").ToString)
                BindToText(tbPersonDt2, Dr(0)("Person").ToString)
                BindToDropList(ddlVechile, TrimStr(Dr(0)("VehicleType").ToString))
                BindToText(tbPanenHKDt2, Dr(0)("PanenHK").ToString)
                BindToText(tbTPHDt2, Dr(0)("Ancak").ToString)
                BindToText(tbSPBLManual, Dr(0)("SPBLManual").ToString)
                BindToDate(tbSPBLDate, Dr(0)("SPBLDate").ToString)
                BindToText(tbJamSPBL, Dr(0)("JamSPBL").ToString)
                'BindToDropList(ddlFgHariHitam, TrimStr(Dr(0)("FgHariHitam").ToString))
                BindToText(tbNormalDt2, Dr(0)("Normal").ToString)
                BindToText(tbAbnormalDt2, Dr(0)("ABNormal").ToString)
                BindToText(tbBrondolanDt2, Dr(0)("Brondolan").ToString)
                BindToText(tbWeight, Dr(0)("Weight").ToString)
                BindToText(tbRemarkDt2, Dr(0)("Remark").ToString)
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
                If ViewState("DtValue") <> ddlBlock.SelectedValue + "|" + tbTPH.Text + "|" + tbSPTBManual.Text Then
                    If CekExistData(ViewState("Dt"), "Blok,Ancak,SPTBSManual", ddlBlock.SelectedValue + "|" + tbTPH.Text + "|" + tbSPTBManual.Text) Then
                        lbStatus.Text = "item detail'" + tbName.Text + "' has already exists"
                        Exit Sub
                    End If
                End If
                Row = ViewState("Dt").Select("Blok+'|'+Ancak+'|'+SPTBSManual = " + QuotedStr(ViewState("DtValue")))(0)
                'If CekDt() = False Then
                '    Exit Sub
                'End If
                Row.BeginEdit()
                Row("WorkBy") = tbCode.Text
                Row("WorkByName") = tbName.Text
                Row("Person") = tbPerson.Text
                Row("PanenHK") = tbPanenHK.Text
                If ddlBlock.SelectedValue = "" Then
                    Row("Blok") = ddlBlock.SelectedValue
                    Row("BlokName") = ""
                Else
                    Row("Blok") = ddlBlock.SelectedValue
                    Row("BlokName") = ddlBlock.SelectedItem.Text
                End If
                Row("SPTBSManual") = tbSPTBManual.Text
                Row("JamSPTBS") = tbJamSPTBS.Text
                Row("SPTBSDate") = tbSPTBSDate.SelectedDate
                Row("FgHariHitam") = ddlFgHariHitam.SelectedValue
                Row("Normal") = tbNormal.Text
                Row("AbNormal") = tbAbnormal.Text
                Row("Brondolan") = tbBrondolan.Text
                Row("Weight") = tbWeightDt.Text
                Row("Ancak") = ddlTphDt.SelectedValue
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()
                'ViewState("Dt").AcceptChanges()
            Else
                'Insert
                'If CekDt() = False Then
                '    Exit Sub
                'End If

                '' Cek jika NoSPBL sudah pernah di scan dan sudah ada di dalam databse
                'Dim CekSPTBSManual As String
                'CekSPTBSManual = SQLExecuteScalar("SELECT SPTBSManual FROM PLSPTBSDt WHERE SPTBSManual = '" + tbSPTBManual.Text + "'", ViewState("DBConnection").ToString)
                'If CekSPTBSManual <> "" Then
                '    lbStatus.Text = MessageDlg("Your SPBL No is already on your Database !")
                '    'playSound()
                '    tbSPTBManual.Text = ""
                '    tbSPTBManual.Focus()
                '    Exit Sub
                'End If
                ''=============================================================
                Dim GVR As GridViewRow

                For Each GVR In GridDt.Rows
                    'Untuk protek jika KSU blocknya  beda
                    If GetCountRecord(ViewState("Dt")) <> 0 Then
                        Dim ksuGrid, KsuScan, Block As String
                        ksuGrid = SQLExecuteScalar("SELECT KSUBlock FROM MsBlock WHERE BlockCode = '" + GVR.Cells(5).Text + "'", ViewState("DBConnection").ToString)
                        KsuScan = SQLExecuteScalar("SELECT KSUBlock FROM MsBlock WHERE BlockCode = '" + ddlBlock.SelectedValue + "'", ViewState("DBConnection").ToString)
                        Block = SQLExecuteScalar("SELECT Block_Code FROM V_MsBlock WHERE Block_Code = '" + GVR.Cells(5).Text + "'", ViewState("DBConnection").ToString)
                        'lbStatus.Text = ksuGrid + KsuScan
                        'Exit Sub

                        If ksuGrid <> KsuScan Then
                            lbStatus.Text = MessageDlg("KSU Block Or Member Plasma must be the same or create new transaction !!")
                            tbScan.Text = ""
                            ddlBlock.Focus()
                            Exit Sub
                        End If


                        If ddlBlock.SelectedValue <> Block Then
                            'lbStatus.Text = MessageDlg("Block Divisi must be the same or create new transaction !!")
                            lbStatus.Text = MessageDlg("Terdapat Beda Block Di SPTIN ini !!")
                            tbScan.Text = ""
                            tbScan.Focus()
                            'Exit Sub
                        End If
                    End If
                    '===================

                    ' Cek jika NoSPBL sudah pernah di scan dan sudah ada di dalam databse
                    Dim CekSPBlManual As String
                    CekSPBlManual = SQLExecuteScalar("SELECT SPBL_SPTBS FROM V_cekSPBL_SPTBS WHERE SPBL_SPTBS = '" + tbSPTBManual.Text + "'", ViewState("DBConnection").ToString)
                    If CekSPBlManual <> "" Then
                        lbStatus.Text = MessageDlg("Your SPTBS No is already on your Database !")
                        'playSound()
                        tbScan.Text = ""
                        tbScan.Focus()
                        Exit Sub
                    End If
                    '=============================================================
                Next

                Dim divisi As String
                divisi = SQLExecuteScalar("SELECT Division FROM V_MsBlock WHERE Block_Code = '" + ddlBlock.SelectedValue + "'", ViewState("DBConnection").ToString)


                'lbStatus.Text = Holiday + "_" + (drUnit("Tgl_SPTBS")).ToString
                'Exit Sub

                'Untuk protek jika block divisinya beda
                If GetCountRecord(ViewState("Dt")) <> 0 Then
                    If ddlDivision.SelectedValue <> divisi Then
                        'lbStatus.Text = MessageDlg("Block Divisi must be the same or create new transaction !!")
                        lbStatus.Text = MessageDlg("Terdapat Beda Divisi Di SPTIN ini, Divisi '" + divisi + "' Inputed  !!")
                        tbScan.Text = ""
                        tbScan.Focus()
                        'Exit Sub
                    End If


                End If



                If CekExistData(ViewState("Dt"), "SPTBSManual", tbSPTBManual.Text) Then
                    lbStatus.Text = "item detail '" + tbSPTBManual.Text + "' has already exists"
                    Exit Sub
                End If


                If CekExistData(ViewState("Dt"), "Blok,Ancak,SPTBSManual", ddlBlock.SelectedValue + "|" + tbTPH.Text + "|" + tbSPTBManual.Text) Then
                    lbStatus.Text = "item detail '" + tbName.Text + "' has already exists"
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("WorkBy") = tbCode.Text
                dr("WorkByName") = tbName.Text
                dr("Person") = tbPerson.Text
                dr("PanenHK") = tbPanenHK.Text
                If ddlBlock.SelectedValue = "" Then
                    dr("Blok") = ddlBlock.SelectedValue
                    dr("BlokName") = ""
                Else
                    dr("Blok") = ddlBlock.SelectedValue
                    dr("BlokName") = ddlBlock.SelectedItem.Text
                End If
                dr("SPTBSManual") = tbSPTBManual.Text
                dr("JamSPTBS") = tbJamSPTBS.Text
                dr("SPTBSDate") = tbSPTBSDate.SelectedDate
                dr("FgHariHitam") = ddlFgHariHitam.SelectedValue
                dr("Normal") = tbNormal.Text
                dr("AbNormal") = tbAbnormal.Text
                dr("Brondolan") = tbBrondolan.Text
                dr("Weight") = tbWeightDt.Text
                dr("Ancak") = ddlTphDt.SelectedValue
                dr("Manual_Input") = "Y"
                dr("Remark") = tbRemarkDt.Text
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


    Protected Sub btnSaveDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveDt2.Click

        Try
            If CekDt2() = False Then
                Exit Sub
            End If

            If ViewState("StateDt2") = "Edit" Then
                Dim Row As DataRow
                If ViewState("Dt2Value") <> tbSuppcode.Text + "|" + tbTPH.Text + "|" + tbSPTBManual.Text Then
                    If CekExistData(ViewState("Dt2"), "SuppCode,Ancak,SPBLManual", tbSuppcode.Text + "|" + tbTPH.Text + "|" + tbSPTBManual.Text) Then
                        lbStatus.Text = "item detail'" + tbName.Text + "' has already exists"
                        Exit Sub
                    End If
                End If
                Row = ViewState("Dt2").Select("SuppCode+'|'+Ancak+'|'+SPBLManual = " + QuotedStr(ViewState("Dt2Value")))(0)
                Row.BeginEdit()
                Row("SuppCode") = tbSuppcode.Text
                Row("SupplierName") = tbSuppName.Text
                Row("Person") = 0
                Row("PanenHK") = tbPanenHKDt2.Text
                Row("VehicleType") = ddlVechile.SelectedValue
                Row("VehicleName") = ddlVechile.SelectedItem.Text
                Row("SPBlManual") = tbSPBLManual.Text
                Row("JamSPBl") = tbJamSPBL.Text
                Row("SPBlDate") = tbSPBLDate.SelectedDate
                Row("Normal") = tbNormalDt2.Text
                Row("AbNormal") = tbAbnormalDt2.Text
                Row("Brondolan") = tbBrondolanDt2.Text
                Row("Weight") = tbWeight.Text
                Row("Ancak") = tbTPHDt2.Text
                Row("Remark") = tbRemarkDt2.Text
                Row.EndEdit()
                'ViewState("Dt2").AcceptChanges()
            Else
                'Insert
                'If CekDt() = False Then
                '    Exit Sub
                'End If

                Dim GVR As GridViewRow
                For Each GVR In GridDt2.Rows
                    'Cek isi dari gridView, jika grid masih kosong maka tidak akan menjalankan proteksi ini
                    If GetCountRecord(ViewState("Dt2")) <> 0 Then
                        If GVR.Cells(2).Text <> tbSuppcode.Text Then
                            lbStatus.Text = MessageDlg("Supplier must be the same or create new transaction !!")
                            tbScan.Text = ""
                            tbScan.Focus()
                            Exit Sub
                        End If
                    End If

                    If GetCountRecord(ViewState("Dt2")) <> 0 Then
                        If GVR.Cells(14).Text <> ddlVechile.SelectedValue Then
                            lbStatus.Text = MessageDlg("IKP must be the same or create new transaction !!")
                            tbScan.Text = ""
                            tbScan.Focus()
                            Exit Sub
                        End If
                    End If


                    ' Cek jika NoSPBL sudah pernah di scan dan sudah ada di dalam databse
                    Dim CekSPBlManual As String
                    CekSPBlManual = SQLExecuteScalar("SELECT SPBL_SPTBS FROM V_cekSPBL_SPTBS WHERE SPBL_SPTBS = '" + tbSPBLManual.Text + "'", ViewState("DBConnection").ToString)
                    If CekSPBlManual <> "" Then
                        lbStatus.Text = MessageDlg("Your SPBL No is already on your Database !")
                        'playSound()
                        tbScan.Text = ""
                        tbScan.Focus()
                        Exit Sub
                    End If
                    '=============================================================





                Next


                '' Cek jika NoSPBL sudah pernah di scan dan sudah ada di dalam databse
                'Dim CekSPBlManual As String
                'CekSPBlManual = SQLExecuteScalar("SELECT SPBlManual FROM PLSPTBSDt2 WHERE SPBlManual = '" + tbSPBLManual.Text + "'", ViewState("DBConnection").ToString)
                'If CekSPBlManual <> "" Then
                '    lbStatus.Text = MessageDlg("Your SPBL No is already on your Database !")
                '    'playSound()
                '    tbSPBLManual.Text = ""
                '    tbSPBLManual.Focus()
                '    Exit Sub
                'End If
                ''=============================================================

                If CekExistData(ViewState("Dt2"), "SPBLManual", tbSPBLManual.Text) Then
                    lbStatus.Text = "item detail '" + tbSPBLManual.Text + "' has already exists"
                    Exit Sub
                End If


                If CekExistData(ViewState("Dt2"), "SuppCode,Ancak,SPBLManual", tbSuppcode.Text + "|" + tbTPH.Text + "|" + tbSPBLManual.Text) Then
                    lbStatus.Text = "item detail '" + tbName.Text + "' has already exists"
                    Exit Sub
                End If

                Dim dr As DataRow
                dr = ViewState("Dt2").NewRow
                dr("SuppCode") = tbSuppcode.Text
                dr("SupplierName") = tbSuppName.Text
                dr("Person") = tbPersonDt2.Text
                dr("PanenHK") = tbPanenHKDt2.Text
                dr("VehicleType") = ddlVechile.SelectedValue
                dr("VehicleName") = ddlVechile.SelectedItem.Text
                dr("SPBlManual") = tbSPBLManual.Text
                dr("JamSPBl") = tbJamSPBL.Text
                dr("SPBlDate") = tbSPBLDate.SelectedDate
                'dr("FgHariHitam") = ddlFgHariHitam.SelectedValue
                dr("Normal") = tbNormalDt2.Text
                dr("AbNormal") = tbAbnormalDt2.Text
                dr("Brondolan") = tbBrondolanDt2.Text
                dr("Weight") = tbWeight.Text
                dr("Ancak") = tbTPHDt2.Text
                dr("Manual_Input") = "Y"
                dr("Remark") = tbRemarkDt2.Text
                ViewState("Dt2").Rows.Add(dr)
            End If
            ViewState("DtRemark") = tbRemarkDt2.Text
            MovePanel(pnlEditDt2, pnlDt2)
            BindGridDt(ViewState("Dt2"), GridDt2)
            For Each GVR In GridDt2.Rows
                If GVR.Cells(11).Text = "Y" Then
                    GridDt2.ForeColor = System.Drawing.Color.Red
                End If
            Next

            EnableHd(GetCountRecord(ViewState("Dt2")) = 0)
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

    Protected Sub btnCancelDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelDt2.Click
        Try
            MovePanel(pnlEditDt2, pnlDt2)
            EnableHd(GetCountRecord(ViewState("Dt2")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt2  Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnTKPanen_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnTKPanen.Click
        Dim ResultField, CriteriaField As String
        Try

            Session("filter") = "select * from V_MsTeam Where Team_Type = 'Borongan' And FgPanen='Y' " '" + QuotedStr(ViewState("UserId").ToString) + ", " + QuotedStr(ddlDept.SelectedValue)
            CriteriaField = "Team_Code, Team_Name, Total_Member, Team_Type, Division"
            ResultField = "Team_Code, Team_Name, Total_Member, Team_Type, Division"

            ViewState("Sender") = "btnTKPanen"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn TK Panen Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSupplier_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSupplier.Click
        Dim ResultField, CriteriaField As String
        Try

            Session("filter") = "select Supplier_Code,Supplier_Name from V_MsSupplier Where  FgActive='Y' " 'GroupType = 'Panen' And " + QuotedStr(ViewState("UserId").ToString) + ", " + QuotedStr(ddlDept.SelectedValue)
            CriteriaField = "Supplier Code,Supplier Name"
            ResultField = "Supplier_Code,Supplier_Name"
            ViewState("Sender") = "btnSupplier"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())

        Catch ex As Exception
            lbStatus.Text = "btn TK Panen Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCode.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try
            Dt = SQLExecuteQuery("select * from V_MsTeam Where Team_Type = 'Borongan' And FgPanen='Y' AND Team_Code = '" + tbCode.Text + "'", ViewState("DBConnection").ToString).Tables(0)
            'Dr = FindMaster("Product", tbProductCode.Text, ViewState("DBConnection").ToString)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbCode.Text = Dr("Team_Code").ToString
                tbName.Text = Dr("Team_Name").ToString
                tbPerson.Text = TrimStr(Dr("Total_Member").ToString)
            Else
                tbCode.Text = ""
                tbName.Text = ""
                tbPerson.Text = "0"

            End If
            AttachScript("setformatdt();", Page, Me.GetType())
            tbCode.Focus()
        Catch ex As Exception
            Throw New Exception("tb TK PanenCode Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub lbTKPanen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbTKPanen.Click
        Try
            ViewState("InputProduct") = "Y"
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsTeam')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb Product Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub ddlDivision_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDivision.SelectedIndexChanged
        Try

            FillCombo(ddlBlock, "Select Block_Code,Block_Code+', '+Block_Name+', '+DivisionName+', '+KsuBlock AS BlockName FROM V_MsBlock WHERE FgPanen ='Y' ", True, "Block_Code", "BlockName", ViewState("DBConnection"))
            'FillCombo(ddlBlock, "Select Block_Code, Block_Name, Division FROm V_MsBlock Where Division = " + QuotedStr(ddlDivision.SelectedValue), True, "Block_Code", "Block_Name", ViewState("DBConnection"))
            'FillCombo(ddlBlock, "EXEC S_GetBlockTahunPanen " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", 'AND Division = '" + QuotedStr(ddlDivision.SelectedValue) + "''", True, "BlockCode", "BlockName", ViewState("DBConnection"))


        Catch ex As Exception
            Throw New Exception("tb WrhsCode Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlblock_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlBlock.SelectedIndexChanged
        Try

            Dim divisi As String

            divisi = SQLExecuteScalar("SELECT Division FROM V_MsBlock WHERE Block_Code = '" + ddlBlock.SelectedValue + "'", ViewState("DBConnection").ToString)
            'lbStatus.Text = divisi
            'Untuk protek jika block divisinya beda
            If GetCountRecord(ViewState("Dt")) <> 0 Then
                'If ddlDivision.SelectedValue <> divisi Then
                '    'lbStatus.Text = MessageDlg("Block Divisi must be the same or create new transaction !!")
                '    lbStatus.Text = MessageDlg("Terdapat Beda Divisi Di SPTIN ini !!")
                '    tbScan.Text = ""
                '    tbScan.Focus()
                '    'Exit Sub
                '    'ddlDivision.SelectedValue = ddlDivision.SelectedValue

                'End If
            Else
                ddlDivision.SelectedValue = divisi
            End If
            '=====================================
            
        Catch ex As Exception
            Throw New Exception("tb WrhsCode Error : " + ex.ToString)
        End Try
    End Sub

    'Untuk memanggil sound
    Private Sub playSound()
        Dim Mytone As New System.Media.SoundPlayer
        Mytone.SoundLocation = "D:\proyek\IALWeb\Execute\Sound\nice-work.wav"
        'Mytone.SoundLocation = ".../../../Sound/nice-work.wav"
        Mytone.Load()
        Mytone.Play()
    End Sub

    Protected Sub tbScan_textChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbScan.TextChanged
        Dim drResult As DataRow
        Dim GVR, GVR2, GVRT As GridViewRow
        Dim dtUnit, dtUnit2, DtCek As DataTable
        Dim drUnit, drUnit2, DrCek As DataRow
        Dim SQLString, SupplierName, VehicleType, IKPName, CekQrcode, CekSPTBSManual, CekSPBlManual As String
        Try

            'For Each GVR In GridDt2.Rows
            '    SQLString = "EXEC Sp_QrcodeScan " + QuotedStr(tbScan.Text)
            '    DtCek = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)
            '    DrCek = DtCek.Rows(0)
            '    lbStatus.Text = DrCek("Supplier").ToString() + "|" + DrCek("TPH").ToString() + "|" + DrCek("SPBL_NO").ToString() '+ "|" + (GVR.Cells(2).Text + "|" + GVR.Cells(8).Text + "|" + GVR.Cells(9).Text)

            '    If (GVR.Cells(2).Text + "|" + GVR.Cells(8).Text + "|" + GVR.Cells(9).Text) = DrCek("Supplier").ToString() + "|" + DrCek("TPH").ToString() + "|" + DrCek("SPBL_NO").ToString() Then
            '        lbStatus.Text = MessageDlg("Qrcode is already Scan!!")
            '        Exit Sub
            '    End If
            'Next

            ' Cek jika Qrcode sudah pernah di scan dan sudah ada di dalam databse
            CekQrcode = SQLExecuteScalar("SELECT QrCode FROM V_QrCode WHERE Qrcode = '" + tbScan.Text.Replace("'", ".") + "'", ViewState("DBConnection").ToString)
            If CekQrcode <> "" Then
                lbStatus.Text = MessageDlg("Your QrCode is already on your database !")
                'playSound()
                tbScan.Text = ""
                tbScan.Focus()
                Exit Sub
            End If
            '=============================================================

            If tbScan.Text.Count < 50 Then
                lbStatus.Text = "Your scan is not valid please try again"
                ' playSound()
                tbScan.Text = ""
                tbScan.Focus()
                Exit Sub
            End If

            'If tbSuppCek.Text <> "" Then
            '    If tbSuppCek.Text <> drUnit2("Supplier").ToString() Then
            '        lbStatus.Text = MessageDlg("Supplier must be the same or create new transaction !!")
            '        tbScan.Text = ""
            '        tbScan.Focus()
            '        Exit Sub
            '    End If
            'End If

            If Strings.Right(tbScan.Text, 2) = "|Y" Then
                ddlDivision.Enabled = False
                ddlDivision.SelectedValue = ""
                SQLString = "EXEC Sp_QrcodeScan " + QuotedStr(tbScan.Text)
                dtUnit2 = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)
                If dtUnit2.Rows.Count > 0 Then
                    drUnit2 = dtUnit2.Rows(0)

                    For Each GVR In GridDt2.Rows
                        'Cek isi dari gridView, jika grid masih kosong maka tidak akan menjalankan proteksi ini
                        If GetCountRecord(ViewState("Dt2")) <> 0 Then
                            If GVR.Cells(2).Text <> drUnit2("Supplier").ToString() Then
                                lbStatus.Text = MessageDlg("Supplier must be the same or create new transaction !!")
                                tbScan.Text = ""
                                tbScan.Focus()
                                Exit Sub
                            End If
                        End If


                        ' Cek jika NoSPBL sudah pernah di scan dan sudah ada di dalam databse
                        CekSPBlManual = SQLExecuteScalar("SELECT SPBL_SPTBS FROM V_cekSPBL_SPTBS WHERE SPBL_SPTBS = '" + drUnit2("SPBL_NO").ToString() + "'", ViewState("DBConnection").ToString)
                        If CekSPBlManual <> "" Then
                            lbStatus.Text = MessageDlg("Your SPBL No is already on your Database !")
                            'playSound()
                            tbScan.Text = ""
                            tbScan.Focus()
                            Exit Sub
                        End If
                        '=============================================================

                        'lbStatus.Text = GVR.Cells(14).Text
                        'Exit Sub

                        If GetCountRecord(ViewState("Dt2")) <> 0 Then
                            If GVR.Cells(14).Text <> drUnit2("Kendaraan").ToString() Then
                                lbStatus.Text = MessageDlg("IKP must be the same or create new transaction !!")
                                tbScan.Text = ""
                                tbScan.Focus()
                                Exit Sub
                            End If
                        End If


                        If (GVR.Cells(2).Text + "|" + GVR.Cells(6).Text + "|" + GVR.Cells(7).Text) = drUnit2("Supplier").ToString() + "|" + drUnit2("TPH").ToString() + "|" + drUnit2("SPBL_NO").ToString() Then
                            'lbStatus.Text = MessageDlg("Qrcode is already Scan!!")
                            lbStatus.Text = "Qrcode is already Scan!!"
                            tbScan.Text = ""
                            tbScan.Focus()
                            Exit Sub
                        End If
                    Next

                    'tbSuppCek.Text = drUnit2("Supplier").ToString
                    SupplierName = SQLExecuteScalar("SELECT Supplier_Name FROM V_MsSupplier WHERE Supplier_Code = '" + drUnit2("Supplier").ToString + "'", ViewState("DBConnection").ToString)
                    'VehicleType = SQLExecuteScalar("SELECT VehicleTypeCode FROM VMsVehicleType WHERE VehicleTypeName = '" + drUnit2("Kendaraan").ToString + "'", ViewState("DBConnection").ToString)
                    IKPName = SQLExecuteScalar("SELECT IdentitasIKPName FROM V_MsIdentitasIKP WHERE IdentitasIKPCode = '" + drUnit2("Kendaraan").ToString + "'", ViewState("DBConnection").ToString)

                    Dim dr2 As DataRow
                    dr2 = ViewState("Dt2").NewRow
                    dr2("SuppCode") = drUnit2("Supplier").ToString
                    dr2("SupplierName") = SupplierName
                    dr2("Person") = 0
                    dr2("VehicleType") = drUnit2("Kendaraan").ToString
                    dr2("VehicleName") = IKPName
                    dr2("SPBLManual") = drUnit2("SPBL_NO").ToString
                    dr2("JamSPBL") = drUnit2("Jam_SPBL").ToString
                    dr2("SPBLDate") = drUnit2("Tgl_SPBL").ToString
                    'dr("FgHariHitam") = ddlFgHariHitam.SelectedValue
                    dr2("Normal") = drUnit2("TBS_Normal").ToString
                    dr2("AbNormal") = drUnit2("TBS_Abnormal").ToString
                    dr2("Brondolan") = drUnit2("Brondolan").ToString
                    dr2("Weight") = drUnit2("Weight").ToString
                    dr2("Ancak") = drUnit2("TPH").ToString
                    dr2("Remark") = drUnit2("Remark").ToString.Replace("'", ".")
                    ViewState("Dt2").Rows.Add(dr2)
                    BindGridDt(ViewState("Dt2"), GridDt2)
                    MultiView1.ActiveViewIndex = 1
                    Menu1.Items.Item(1).Selected = True
                End If

            Else

                ddlDivision.Enabled = True
                SQLString = "EXEC Sp_QrcodeScan " + QuotedStr(tbScan.Text)
                dtUnit = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)
                If dtUnit.Rows.Count > 0 Then
                    drUnit = dtUnit.Rows(0)

                    For Each GVR2 In GridDt.Rows
                        If (GVR2.Cells(5).Text + "|" + GVR2.Cells(8).Text + "|" + GVR2.Cells(9).Text) = drUnit("Block").ToString() + "|" + drUnit("TPH").ToString() + "|" + drUnit("SPTBS_NO").ToString() Then
                            'lbStatus.Text = MessageDlg("Qrcode is already Scan!!")
                            lbStatus.Text = "Qrcode is already Scan!!"
                            tbScan.Text = ""
                            tbScan.Focus()
                            Exit Sub
                        End If


                        CekSPTBSManual = SQLExecuteScalar("SELECT SPBL_SPTBS FROM V_cekSPBL_SPTBS WHERE SPBL_SPTBS = '" + drUnit("SPTBS_NO").ToString() + "'", ViewState("DBConnection").ToString)
                        If CekSPTBSManual <> "" Then
                            lbStatus.Text = MessageDlg("Your SPTBS No is already on your Database !")
                            'playSound()
                            tbScan.Text = ""
                            tbScan.Focus()
                            Exit Sub
                        End If
                        '===========================================================

                        'Untuk protek jika KSU blocknya  beda
                        If GetCountRecord(ViewState("Dt")) <> 0 Then
                            Dim ksuGrid, KsuScan As String
                            ksuGrid = SQLExecuteScalar("SELECT KSUBlock FROM MsBlock WHERE BlockCode = '" + GVR2.Cells(5).Text + "'", ViewState("DBConnection").ToString)
                            KsuScan = SQLExecuteScalar("SELECT KSUBlock FROM MsBlock WHERE BlockCode = '" + drUnit("Block").ToString + "'", ViewState("DBConnection").ToString)
                            'lbStatus.Text = ksuGrid + KsuScan
                            'Exit Sub

                            If ksuGrid <> KsuScan Then
                                lbStatus.Text = MessageDlg("KSU Block Or Member Plasma must be the same or create new transaction !!")
                                tbScan.Text = ""
                                tbScan.Focus()
                                Exit Sub
                            End If
                        End If
                        '===================
                    Next

                    Dim WorkbyName, BlockName, divisi, Person, Holiday As String
                    WorkbyName = SQLExecuteScalar("SELECT Team_Name FROM V_MsTeam WHERE Team_Code = '" + drUnit("Worker").ToString + "'", ViewState("DBConnection").ToString)
                    BlockName = SQLExecuteScalar("SELECT Block_Name FROM V_MsBlock WHERE Block_Code = '" + drUnit("Block").ToString + "'", ViewState("DBConnection").ToString)
                    divisi = SQLExecuteScalar("SELECT Division FROM V_MsBlock WHERE Block_Code = '" + drUnit("Block").ToString + "'", ViewState("DBConnection").ToString)
                    Person = SQLExecuteScalar("SELECT Total_Member FROM V_MsTeam Where FgPanen='Y' AND Team_Code = '" + drUnit("Worker").ToString + "'", ViewState("DBConnection").ToString)
                    Holiday = SQLExecuteScalar("SELECT HolidayDate FROM VMsHoliday WHERE  HolidayDate = '" + Format(drUnit("Tgl_SPTBS"), "yyyyMMdd").ToString + "'", ViewState("DBConnection").ToString)
                    'KSu = SQLExecuteScalar("SELECT Block_Name FROM MsBlock WHERE BlockCode = '" + drUnit("Block").ToString + "'", ViewState("DBConnection").ToString)

                    'lbStatus.Text = Holiday + "_" + (drUnit("Tgl_SPTBS")).ToString
                    'Exit Sub

                    'Untuk protek jika block divisinya beda
                    If GetCountRecord(ViewState("Dt")) <> 0 Then

                        If ddlDivision.SelectedValue <> divisi Then
                            'lbStatus.Text = MessageDlg("Block Divisi must be the same or create new transaction !!")
                            lbStatus.Text = MessageDlg("Terdapat Beda Divisi Di SPTIN ini, Divisi '" + divisi + "' Inputed !!")
                            tbScan.Text = ""
                            tbScan.Focus()
                            'Exit Sub
                        End If

                    Else
                        ddlDivision.SelectedValue = divisi
                        ddlDivision_SelectedIndexChanged(Nothing, Nothing)
                        ddlDivision.Enabled = False
                    End If
                    '=====================================



                    Dim dr As DataRow
                    dr = ViewState("Dt").NewRow
                    dr("WorkBy") = drUnit("Worker").ToString
                    dr("Blok") = drUnit("Block").ToString
                    dr("BlokName") = BlockName
                    dr("WorkByName") = WorkbyName
                    dr("Person") = Person
                    dr("PanenHK") = drUnit("Ha")
                    'If ddlBlock.SelectedValue = "" Then
                    '    dr("Blok") = ddlBlock.SelectedValue
                    '    dr("BlokName") = ""
                    'Else
                    '    dr("Blok") = ddlBlock.SelectedValue
                    '    dr("BlokName") = ddlBlock.SelectedItem.Text
                    'End If
                    dr("SPTBSManual") = drUnit("SPTBS_NO").ToString
                    dr("JamSPTBS") = drUnit("Jam_SPTBS").ToString
                    dr("SPTBSDate") = drUnit("Tgl_SPTBS").ToString

                    If Holiday = drUnit("Tgl_SPTBS").ToString Then
                        dr("FgHariHitam") = "Y"
                    Else
                        dr("FgHariHitam") = "N"
                    End If
                    'dr("FgHariHitam") = ddlFgHariHitam.SelectedValue
                    dr("Normal") = drUnit("TBS_Normal").ToString
                    dr("AbNormal") = drUnit("TBS_Abnormal").ToString
                    dr("Brondolan") = drUnit("Brondolan").ToString
                    dr("Weight") = drUnit("Weight").ToString
                    dr("Ancak") = drUnit("TPH").ToString
                    dr("Remark") = drUnit("Remark").ToString.Replace("'", ".")
                    ViewState("Dt").Rows.Add(dr)
                    BindGridDt(ViewState("Dt"), GridDt)
                    MultiView1.ActiveViewIndex = 0
                    Menu1.Items.Item(0).Selected = True

                End If
            End If
            tbScan.Text = ""
            tbScan.Focus()


            ''lbStatus.Text = "Test"
            ''Exit Sub
            'SQLString = "EXEC Sp_QrcodeScan " + QuotedStr(tbRemark.Text)
            'dtUnit = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)
            'For Each drResult In (dr).Rows
            '    ExistRow = ViewState("Dt").Select("WorkBy = " + QuotedStr(tbRemark.Text))
            '    If ExistRow.Count = 0 Then
            '        SQLString = "EXEC Sp_QrcodeScan " + QuotedStr(tbRemark.Text)
            '        dtUnit = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)
            '        If dtUnit.Rows.Count > 0 Then
            '            drUnit = dtUnit.Rows(0)
            '            Dim dr As DataRow
            '            dr = ViewState("Dt").NewRow
            '            dr("WorkBy") = drUnit("PRODUCTID").ToString

            '            ViewState("Dt2").Rows.Add(dr)


            '        End If
            '    End If
            'Next


            'SQLString = "EXEC Sp_QrcodeScan " + QuotedStr(tbRemark.Text)
            'dtUnit = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)

            ' For Each drResult In dtUnit.Rows
            'If dtUnit.Rows.Count > 0 Then
            'drUnit = dtUnit.Rows(0)
            'Dim dr As DataRow
            'dr = ViewState("Dt").NewRow

            'dr("WorkBy") = "1"

            'ViewState("Dt").Rows.Add(dr)

            ''End If


            ''Next
            'BindGridDt(ViewState("Dt"), GridDt)
            'EnableHd(GetCountRecord(ViewState("Dt")) = 0) 
            'GridDt.Columns(1).Visible = GetCountRecord(ViewState("Dt")) > 0
            'Dim hasil As String
            'tbScan.Text

        Catch ex As Exception
            Throw New Exception("tb WrhsCode Error : " + ex.ToString)
        End Try
    End Sub


    Protected Sub btnCar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCar.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT CarNo, CarName FROM V_MsCarSPTIN"
            ResultField = "CarNo, CarName"
            ViewState("Sender") = "btnCar"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Cust Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnCarT_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCarT.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT CarNo, CarName FROM V_MsCarSPTIN" ' WHERE Car_No = " + QuotedStr(tbCarT.Text)
            ResultField = "CarNo, CarName"
            ViewState("Sender") = "btnCarT"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())

        Catch ex As Exception
            lbStatus.Text = "btn Search Cust Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnOP_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOP.Click
        Dim ResultField As String

        Try
            Session("filter") = "SELECT Emp_No,Emp_Name FROM V_MsEmployee WHERE Fg_Active='Y' "
            ResultField = "Emp_No,Emp_Name"
            ViewState("Sender") = "btnOp"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Cust Error : " + ex.ToString
        End Try

    End Sub

    Protected Sub btnOPT_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOPT.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT Emp_No,Emp_Name FROM V_MsEmployee WHERE Fg_Active='Y' "
            ResultField = "Emp_No,Emp_Name"
            ViewState("Sender") = "btnOPT"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Cust Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub ddlTransit_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTransit.SelectedIndexChanged
        Try
            tbCarT.Enabled = ddlTransit.SelectedValue = "Y"
            tbOpT.Enabled = ddlTransit.SelectedValue = "Y"
            btnCarT.Enabled = ddlTransit.SelectedValue = "Y"
            btnOPT.Enabled = ddlTransit.SelectedValue = "Y"
            btnBMT.Enabled = ddlTransit.SelectedValue = "Y"
            tbBMT.Enabled = ddlTransit.SelectedValue = "Y"
            tbCarT.Text = ""
            tbCarNameT.Text = ""
            tbOpT.Text = ""
            tbOPNameT.Text = ""
            tbBMT.Text = ""
            tbBMTName.Text = ""
        Catch ex As Exception
            lbStatus.Text = "ddlTransit_SelectedIndexChanged Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub tbSptbsDate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbSPTBSDate.SelectionChanged
        Dim TglHitam As String
        Try
            TglHitam = SQLExecuteScalar("SELECT Holiday_date FROM  VMsHoliday WHERE Holiday_date = " + QuotedStr(tbSPTBSDate.SelectedDateFormatted) + "", ViewState("DBConnection"))
            'lbStatus.Text = TglHitam + "_" + tbDate.SelectedDateFormatted
            ''Exit Sub
            If tbSPTBSDate.SelectedDateFormatted = TglHitam Then
                ddlFgHariHitam.SelectedValue = "Y"
            Else
                ddlFgHariHitam.SelectedValue = "N"
            End If
        Catch ex As Exception
            lbStatus.Text = "tbDate_SelectionChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnBM_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBM.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT Supplier_Code, Supplier_Name FROM V_MsSupplier WHERE FgActive='Y' AND Supplier_Type = 'EBM'"
            ResultField = "Supplier_Code, Supplier_Name"
            ViewState("Sender") = "btnBM"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())

        Catch ex As Exception
            lbStatus.Text = "btn Search Cust Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnBMT_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBMT.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT Supplier_Code, Supplier_Name FROM V_MsSupplier WHERE FgActive='Y' AND Supplier_Type = 'EBM'"
            ResultField = "Supplier_Code, Supplier_Name"
            ViewState("Sender") = "btnBMT"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Cust Error : " + ex.ToString
        End Try
    End Sub

End Class
