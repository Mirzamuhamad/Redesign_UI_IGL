Imports System.Data
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO
Imports System

Public Class lbStatus
    Public Sub New()

    End Sub
End Class
Public Class ddlDivision
    Public Sub New()
        
    End Sub
End Class




Partial Class Transaction_TrSPTBS_TrSPTBS
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter

    Private Function GetStringHd() As String
        Return "Select * From V_PLSPTBSHD WHERE UserPrep = '" + ViewState("UserId") + "' "

    End Function
    'Protected GetStringHd As String = "Select * From V_PLSPTBSHD WHERE UserPrep = " + ViewState("UserId") + ""


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Try
            If Not IsPostBack Then
                InitProperty()
                FillCombo(ddlTPH, "EXEC S_GetTPH ", True, "TPH", "TPH", ViewState("DBConnection"))
                FillCombo(ddlDivision, "EXEC S_GetDivisionForSAUserDivision " + QuotedStr(ViewState("UserId")) + ", 'SPTBS'", True, "DivisionCode", "DivisionName", ViewState("DBConnection"))
                ' FillCombo(ddlBlock, "EXEC S_GetBlockTahunPanen " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + "' AND Division = '" + QuotedStr(ddlDivision.SelectedValue) + "'", True, "BlockCode", "BlockName", ViewState("DBConnection"))
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

                If ViewState("Sender") = "btnBM" Then
                    tbBM.Text = Session("Result")(0).ToString
                    BindToText(tbBMName, Session("Result")(1).ToString)
                End If

                If ViewState("Sender") = "btnBMT" Then
                    tbBMT.Text = Session("Result")(0).ToString
                    BindToText(tbBMTName, Session("Result")(1).ToString)
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

        tbPanenHK.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbNormal.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbAbnormal.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbBrondolan.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbPanenHK.Attributes.Add("OnKeyDown", "return PressNumeric();")

        tbPanenHK.Attributes.Add("OnBlur", "setformatdt();")
        tbNormal.Attributes.Add("OnBlur", "setformatdt();")
        tbAbnormal.Attributes.Add("OnBlur", "setformatdt();")
        tbBrondolan.Attributes.Add("OnBlur", "setformatdt();")

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
            DV.Sort = ViewState("SortExpression")
            GridView1.DataSource = DV
            GridView1.DataBind()
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT * From V_PLSPTBSDt WHERE TransNmbr = " + QuotedStr(Nmbr)
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
                Result = Result + "'"
                Session("SelectCommand") = "EXEC S_PLFormSPTBS" + Result
                'lbStatus.Text = Session("SelectCommand")
                'Exit Sub
                Session("ReportFile") = ".../../../Rpt/FormSPTBS.frx"
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
            ddlDivision.Enabled = State
            'btnCar.Enabled = State
            'btnOP.Enabled = State
            'tbCar.Enabled = State
            'tbOp.Enabled =State 
            'tbJam.Enabled = State

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
                tbRef.Text = GetAutoNmbr("SPT", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
                SQLString = "INSERT INTO PLSPTBSHd (TransNmbr, Status, TransDate, InputDate, CarNo, Operator, TPH, Division, " + _
                "DateAngkut, JamAngkut, FgTransit, CarNoTransit, OperatorTransit,BMTransit,BM, Remark, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbRef.Text) + ", 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', GetDate(), " + _
                QuotedStr(tbCar.Text) + "," + QuotedStr(tbOp.Text) + ", " + QuotedStr(ddlTPH.SelectedValue) + ", " + QuotedStr(ddlDivision.SelectedValue) + ", '" + _
                Format(tbAngkutDate.SelectedValue, "yyyy-MM-dd") + "', " + QuotedStr(tbJam.Text) + ", " + QuotedStr(ddlTransit.SelectedValue) + "," + _
                QuotedStr(tbCarT.Text) + "," + QuotedStr(tbCarNameT.Text) + "," + QuotedStr(tbBMT.Text) + "," + QuotedStr(tbBM.Text) + "," + QuotedStr(tbRemark.Text) + "," + _
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

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString 'System.Configuration.ConfigurationManager.AppSettings.Get("ConString")
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, WorkBy, Person, Blok, PanenHK, Ancak, SPTBSManual, " + _
            " SPTBSDate, JamSPTBS, FgHariHitam, Normal, Abnormal, Brondolan, Remark FROM PLSPTBSDt WHERE TransNmbr = '" & ViewState("Reference") & "'", con)
           
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
                    ", Abnormal = @Abnormal, Brondolan = @Brondolan " + _
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
                If CekDt(dr, "blok,Ancak,SPTBSManual") = False Then
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
            ddlTransit_SelectedIndexChanged(Nothing, Nothing)
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
            tbCar.Text = ""
            tbBMT.Text = ""
            tbBM.Text = ""
            tbBMTName.Text = ""
            tbBMName.Text = ""
            tbCarName.Text = ""
            tbCarT.Text = ""
            tbCarNameT.Text = ""
            tbOp.Text = ""
            tbOpName.Text = ""
            tbOpT.Text = ""
            tbOPNameT.Text = ""
            ddlTransit.SelectedValue = "N"
            ddlTPH.SelectedValue = ""
            tbAngkutDate.SelectedValue = ViewState("ServerDate") 'Today
            tbDate.SelectedDate = ViewState("ServerDate") 'Today

            tbJam.Text = ""
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
            tbJamSPTBS.Text = ""
            tbSPTBSDate.SelectedDate = Today.Date
            tbRemarkDt.Text = ViewState("DtRemark")
            tbPanenHK.Text = "0"
            ddlBlock.SelectedValue = ""
            ddlFgHariHitam.SelectedValue = "N"
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
                If CekDt(dr, "blok,Ancak,SPTBSManual") = False Then
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
            If ddlDivision.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Division must have value")
                ddlDivision.Focus()
                Return False
            End If

            If Len(tbRemark.Text.Trim) > 60 Then
                lbStatus.Text = MessageDlg("Remark must have value or caracter must 60")
                tbRemark.Focus()
                Return False
            End If

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
                If CFloat(tbPerson.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Person Must Have Value")
                    tbPerson.Focus()
                    Return False
                End If
                If ddlBlock.SelectedValue.Trim = "" Then
                    lbStatus.Text = MessageDlg("Blok  Must Have Value")
                    ddlBlock.Focus()
                    Return False
                End If
                If CFloat(tbPanenHK.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Hektar Panen Must Have Value")
                    tbPanenHK.Focus()
                    Return False
                End If
                If tbTPH.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("TPH Must Have Value")
                    tbTPH.Focus()
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
                    GridDt.PageIndex = 0
                    FillTextBoxHd(ViewState("Reference"))
                    BindDataDt(ViewState("Reference"))
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
                        Session("SelectCommand") = "EXEC S_PLFormSPTBS ''" + QuotedStr(GVR.Cells(2).Text) + "''"

                        'Session("SelectCommand") = "EXEC S_STFormIssueReq ''" + QuotedStr(GVR.Cells(2).Text) "''"
                        Session("ReportFile") = ".../../../Rpt/FormSPTBS.frx"
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
            BindToText(tbCarT, Dt.Rows(0)("CarNoTransit").ToString)
            BindToText(tbCarNameT, Dt.Rows(0)("CarTransitName").ToString)
            BindToText(tbOpT, Dt.Rows(0)("OperatorTransit").ToString)
            BindToText(tbOPNameT, Dt.Rows(0)("OperatorTransitName").ToString)
            BindToText(tbBMT, Dt.Rows(0)("BMTransit").ToString)
            BindToText(tbBMTName, Dt.Rows(0)("BMTransitName").ToString)
            BindToText(tbBM, Dt.Rows(0)("BM").ToString)
            BindToText(tbBMName, Dt.Rows(0)("BMName").ToString)
            BindToText(tbJam, Dt.Rows(0)("JamAngkut").ToString)

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
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
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
                Row("Ancak") = tbTPH.Text
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()
                'ViewState("Dt").AcceptChanges()
            Else
                'Insert
                If CekDt() = False Then
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
                dr("Ancak") = tbTPH.Text
                dr("Remark") = tbRemarkDt.Text
                ViewState("Dt").Rows.Add(dr)
            End If
            ViewState("DtRemark") = tbRemarkDt.Text
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
            'FillCombo(ddlBlock, "Select Block_Code, Block_Name, Division FROm V_MsBlock Where Division = " + QuotedStr(ddlDivision.SelectedValue), True, "Block_Code", "Block_Name", ViewState("DBConnection"))
            FillCombo(ddlBlock, "EXEC S_GetBlockTahunPanen " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", 'AND Division = '" + QuotedStr(ddlDivision.SelectedValue) + "''", True, "BlockCode", "BlockName", ViewState("DBConnection"))
        Catch ex As Exception
            Throw New Exception("tb WrhsCode Error : " + ex.ToString)
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

    Protected Sub btnCarT_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCarT.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT Car_No, Car_Name FROM V_MsCar " ' WHERE Car_No = " + QuotedStr(tbCarT.Text)
            ResultField = "Car_No, Car_Name"
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
            Session("filter") = "SELECT Emp_No, Emp_Name FROM V_MsEmployee " 'WHERE Emp_No = " + QuotedStr(tbOp.Text)
            ResultField = "Emp_No, Emp_Name"
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
            Session("filter") = "SELECT Emp_No, Emp_Name FROM V_MsEmployee " 'WHERE Emp_No = " + QuotedStr(tbOpT.Text)
            ResultField = "Emp_No, Emp_Name"
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
            tbOPNameT.Text = ""
        Catch ex As Exception
            lbStatus.Text = "ddlTransit_SelectedIndexChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbDate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbDate.SelectionChanged
        Dim TglHitam As String
        Try
            TglHitam = SQLExecuteScalar("SELECT * FROM  VMsHoliday WHERE Holiday_date = " + QuotedStr(tbDate.SelectedDateFormatted) + "", ViewState("DBConnection"))
            'lbStatus.Text = TglHitam
            'Exit Sub
            If tbDate.SelectedDateFormatted = TglHitam Then
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

    Protected Sub btnBMT_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBMT.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT Emp_No, Emp_Name FROM V_MsEmployee " 'WHERE Emp_No = " + QuotedStr(tbOp.Text)
            ResultField = "Emp_No, Emp_Name"
            ViewState("Sender") = "btnBMT"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())

        Catch ex As Exception
            lbStatus.Text = "btn Search Cust Error : " + ex.ToString
        End Try
    End Sub

End Class
