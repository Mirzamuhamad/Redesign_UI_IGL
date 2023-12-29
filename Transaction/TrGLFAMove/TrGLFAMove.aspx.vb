Imports System.Data
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO

Partial Class Transaction_TrGLFAMove_TrGLFAMove
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select * From V_GLFAMoveHd"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Try
            If Not IsPostBack Then
                InitProperty()
                ViewState("SetLocation") = False
                SetInit()
                Session("AdvanceFilter") = ""
            End If
            lbStatus.Text = ""
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If

            If Request.QueryString("ContainerId").ToString = "TrGLFAReturnID" Then
                ddlTypeSrc.Enabled = False
                tbSource.Enabled = False
                btnSource.Visible = False
            End If

            ' hasil dari search dialog
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnSource" Then
                    tbSource.Text = Session("Result")(0).ToString
                    tbSourceName.Text = Session("Result")(1).ToString
                End If
                If ViewState("Sender") = "btnDest" Then
                    tbDest.Text = Session("Result")(0).ToString
                    tbDestName.Text = Session("Result")(1).ToString
                End If
                If ViewState("Sender") = "btnReff" Then
                    tbReff.Text = Session("Result")(0).ToString
                    ddlTypeSrc.SelectedValue = Session("Result")(4).ToString
                    tbSource.Text = Session("Result")(5).ToString
                    tbSourceName.Text = Session("Result")(6).ToString
                    ddlTypeDest.SelectedValue = Session("Result")(1).ToString
                    tbDest.Text = Session("Result")(2).ToString
                    tbDestName.Text = Session("Result")(3).ToString
                    BindToText(tbOperator, Session("Result")(7).ToString)
                    BindToText(tbRemark, Session("Result")(8).ToString)
                End If
                If ViewState("Sender") = "btnFA" Then
                    tbFACode.Text = Session("Result")(0).ToString
                    tbFAName.Text = Session("Result")(1).ToString
                    tbQty.Text = Session("Result")(2).ToString
                End If
                'tbQty.Text = FormatNumber(tbQty.Text, ViewState("DigitQty"))

                If ViewState("Sender") = "btnGetDt" Then
                    Dim drResult As DataRow
                    For Each drResult In Session("Result").Rows
                        'insert
                        Dim dr As DataRow
                        dr = ViewState("Dt").NewRow
                        dr("FixedAsset") = drResult("FACode")
                        dr("FixedAssetName") = drResult("FAName")
                        'BindToDate(tbDate, dr("DueDate").ToString)
                        If dr("DueDate") Is Nothing Then
                            dr("DueDate") = DBNull.Value
                            ' Else
                            'dr("DueDate") = drResult("DueDate")
                        End If
                        dr("Qty") = drResult("Qty")
                        ViewState("Dt").Rows.Add(dr)
                    Next
                    BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
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
        ViewState("SortExpression") = Nothing
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        If Request.QueryString("ContainerId").ToString = "TrGLFAMoveID" Then
            Labelmenu.Text = "FIXED ASSET MOVING"
            Label1.Visible = False
            tbReff.Visible = False
            btnReff.Visible = Label1.Visible
            Label4.Visible = False
            GridDt.Columns(4).Visible = False
            Label2.Visible = False
            Label3.Visible = False
            tbDueDate.Visible = False
        ElseIf Request.QueryString("ContainerId").ToString = "TrGLFABorrowID" Then
            Labelmenu.Text = "FIXED ASSET BORROW"
            Label1.Visible = False
            tbReff.Visible = False
            btnReff.Visible = False
            Label4.Visible = False
            GridDt.Columns(4).Visible = True
            Label2.Visible = True
            Label3.Visible = True
            tbDueDate.Visible = True
        Else
            Labelmenu.Text = "FIXED ASSET RETURN"
            Label1.Visible = True
            Label4.Visible = True
            tbReff.Visible = True
            btnReff.Visible = True
            GridDt.Columns(4).Visible = False
            Label2.Visible = False
            Label3.Visible = False
            tbDueDate.Visible = False
        End If

        FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
        If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then

        End If
        Me.tbQty.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbQty.Attributes.Add("OnBlur", "setformat();")
    End Sub

    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue, ddlRange.SelectedValue)
            If AdvanceFilter.Length > 1 And StrFilter.Length > 1 Then
                StrFilter = StrFilter + " And " + AdvanceFilter
            ElseIf AdvanceFilter.Length > 1 And StrFilter.Length <= 1 Then
                StrFilter = AdvanceFilter
            End If
            'If Request.QueryString("ContainerId").ToString = "TrGLFAMoveID" Then
            GetStringHd = "Select * From V_GLFAMoveHd where MoveType = " + QuotedStr(Request.QueryString("MenuParam"))
            'ElseIf Request.QueryString("ContainerId").ToString = "TrGLFABorrowID" Then
            'GetStringHd = "Select * From V_GLFAMoveHd where MoveType = 'Borrow'"
            'Else
            'GetStringHd = "Select * From V_GLFAMoveHd where MoveType = 'Return'"
            'End If
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
        Return "SELECT * From V_GLFAMoveDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            GridView1.PageIndex = 0
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
            Status = CekStatus(ActionValue)

            ListSelectNmbr = ""
            GetListCommand(Status, GridView1, "3,2", ListSelectNmbr, Nmbr, lbStatus.Text)
            If ListSelectNmbr = "" Then Exit Sub
            For j = 0 To (Nmbr.Length - 1)
                If Nmbr(j) = "" Then
                    Exit For
                Else
                    Result = ExecSPCommandGo(ActionValue, "S_GLFAMove", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                    If Trim(Result) <> "" Then
                        lbStatus.Text = lbStatus.Text + Result + " <br/>"

                    End If
                End If
            Next
            BindData("TransNmbr in (" + ListSelectNmbr + ")")
        Catch ex As Exception
            lbStatus.Text = "Go Command Error : " + ex.ToString
        End Try
    End Sub

    Private Sub EnableHd(ByVal State As Boolean)
        Try
            btnGetDt.Visible = State
            tbDate.Enabled = State
            If Request.QueryString("ContainerId").ToString = "TrGLFAReturnID" Then
                ddlTypeSrc.Enabled = False
                tbSource.Enabled = False

                btnSource.Visible = False
                'btnDest.Visible = State
                'ddlTypeDest.Enabled = State
                'tbDest.Enabled = State
            Else
                ddlTypeSrc.Enabled = State
                tbSource.Enabled = State
                btnSource.Visible = State

                'ddlTypeDest.Enabled = State
                'tbDest.Enabled = State
                'btnDest.Visible = State
            End If
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
                If ViewState("DtValue") <> tbFACode.Text Then
                    If CekExistData(ViewState("Dt"), "FixedAsset", tbFACode.Text) Then
                        lbStatus.Text = "FixedAsset " + tbFAName.Text + "' has been already exist"
                        Exit Sub
                    End If
                End If
                If Request.QueryString("ContainerId").ToString = "TrGLFABorrowID" Then
                    If tbDueDate.IsNull Then
                        lbStatus.Text = "Due Date Must Fill"
                        Exit Sub
                    End If
                End If
                Row = ViewState("Dt").Select("FixedAsset = " + QuotedStr(ViewState("DtValue")))(0)
                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                Row("FixedAsset") = tbFACode.Text
                Row("FixedAssetName") = tbFAName.Text
                Row("Qty") = tbQty.Text
                'If Row("DueDate") Is Nothing Then
                If Not tbDueDate.Visible Then
                    Row("DueDate") = DBNull.Value
                Else
                    Row("DueDate") = tbDueDate.SelectedValue
                End If
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()

            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If

                If CekExistData(ViewState("Dt"), "FixedAsset", tbFACode.Text) = True Then
                    lbStatus.Text = "FixedAsset " + tbFAName.Text + " has been already exist"
                    Exit Sub
                End If
                If Request.QueryString("ContainerId").ToString = "TrGLFABorrowID" Then
                    If tbDueDate.IsNull Then
                        lbStatus.Text = "Due Date Must Fill"
                        Exit Sub
                    End If
                End If

                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("FixedAsset") = tbFACode.Text
                dr("FixedAssetName") = tbFAName.Text
                dr("Qty") = tbQty.Text
                If tbDueDate.Visible Then
                    dr("DueDate") = Format(tbDueDate.SelectedDate, "dd MMMM yyyy")
                Else
                    dr("DueDate") = DBNull.Value
                End If
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
        Dim SQLString As String
        Dim I As Integer
        Try
            If pnlDt.Visible = False Then
                lbStatus.Text = "Detail Data must be saved first"
                Exit Sub
            End If
            'If Request.QueryString("ContainerId").ToString = "TrGLFAMoveID" Then
            '    tbMoveType.Text = Request.QueryString("MenuParam")
            'ElseIf Request.QueryString("ContainerId").ToString = "TrGLFABorrowID" Then
            '    ViewState("TransType") = "FAB"
            'Else
            '    ViewState("TransType") = "FAN"
            'End If
            If Not (ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit") Then
                Exit Sub
            Else
                ViewState("StateSave") = ViewState("StateHd")
                ViewState("StateHd") = "View"
            End If
            'Save Hd
            If ViewState("StateSave") = "Insert" Then
                tbReference.Text = GetAutoNmbr(tbMoveType.Text, "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
                SQLString = "INSERT INTO GLFAMoveHd (TransNmbr, Status, TransDate, MoveType, Reference, FALocationTypeSrc, FALocationCodeSrc, FALocationTypeDest, FALocationCodeDest, " + _
                "Operator, Remark, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbReference.Text) + ", 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', " + _
                QuotedStr(tbMoveType.Text) + ", " + QuotedStr(tbReff.Text) + ", " + QuotedStr(ddlTypeSrc.SelectedValue) + ", " + _
                QuotedStr(tbSource.Text) + ", " + QuotedStr(ddlTypeDest.SelectedValue) + ", " + QuotedStr(tbDest.Text) + ", " + _
                QuotedStr(tbOperator.Text) + ", " + _
                QuotedStr(tbRemark.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", getDate()"
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM GLFAMoveHd WHERE TransNmbr = " + QuotedStr(tbReference.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE GLFAMoveHd SET TransDate = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "' " + _
                ", MoveType = " + QuotedStr(tbMoveType.Text) + _
                ", Reference = " + QuotedStr(tbReff.Text) + ", FALocationTypeSrc = " + QuotedStr(ddlTypeSrc.SelectedValue) + _
                ", FALocationCodeSrc = " + QuotedStr(tbSource.Text) + ", FALocationTypeDest = " + QuotedStr(ddlTypeDest.SelectedValue) + ", FALocationCodeDest = " + QuotedStr(tbDest.Text) + _
                ", Remark = " + QuotedStr(tbRemark.Text) + ", UserPrep = " + QuotedStr(ViewState("UserId").ToString) + ", DatePrep = getDate()" + _
                " WHERE TransNmbr = " + QuotedStr(tbReference.Text) + ""
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
                Row(I)("TransNmbr") = tbReference.Text
                Row(I).EndEdit()
            Next

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString 'System.Configuration.ConfigurationManager.AppSettings.Get("ConString")
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, FixedAsset, Qty, DueDate, Remark FROM GLFAMoveDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt As New DataTable("GLFAMoveDt")

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
            If Request.QueryString("ContainerId").ToString = "TrGLFABorrowID" Then
                If tbDueDate.IsNull Then
                    lbStatus.Text = "Due Date Must Fill"
                    Exit Sub
                End If
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
            tbFilter.Text = tbReference.Text
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

            If Request.QueryString("ContainerId").ToString = "TrGLFAMoveID" Then
                'tbRRFromName.Visible = True
                btnReff.Visible = False
            ElseIf Request.QueryString("ContainerId").ToString = "TrGLFABorrowID" Then
                'tbRRFromName.Visible = True
                'btnRRFrom.Visible = tbRRFromName.Visible
                btnReff.Visible = False
                tbDest.Enabled = True
            Else
                'tbRRFromName.Visible = False
                'btnRRFrom.Visible = tbRRFromName.Visible
                btnReff.Visible = True
                ddlTypeSrc.Enabled = False
                tbSource.Enabled = False
                btnSource.Visible = False
            End If
            tbDate.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            ClearHd()
            Cleardt()
            tbDate.SelectedDate = ViewState("ServerDate") 'Today
            tbOperator.Text = ViewState("UserId")
            tbMoveType.Text = Request.QueryString("MenuParam")
            ddlTypeSrc.SelectedIndex = 0
            ddlTypeDest.SelectedIndex = 0
            pnlDt.Visible = True
            BindDataDt("")
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbReference.Text = ""
            tbOperator.Text = ""
            tbSource.Text = ""
            tbSourceName.Text = ""
            tbDest.Text = ""
            tbDestName.Text = ""
            tbRemark.Text = ""
            tbReff.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbFACode.Text = ""
            tbFAName.Text = ""
            tbQty.Text = "0"
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
            If GetCountRecord(ViewState("Dt")) = 0 Then
                lbStatus.Text = MessageDlg("Detail must have at least 1 record")
                Exit Sub
            End If
            If Request.QueryString("ContainerId").ToString = "TrGLFABorrowID" Then
                If tbDueDate.IsNull Then
                    lbStatus.Text = "Due Date Must Fill"
                    Exit Sub
                End If
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

    Protected Sub btnFA_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFA.Click
        Dim ResultField As String
        Try
            If Request.QueryString("ContainerId").ToString = "TrGLFAMoveID" Then
                Session("Filter") = "Select FACode, FAName, FA_SubGrp_Code, FA_SubGrp_Name, Qty FROM VMsFixedAssetDt WHERE FgActive = 'Y' AND Qty > 0 and Fg_Moving = 'Y' AND FALocationType = " + QuotedStr(ddlTypeSrc.SelectedValue) + " AND FALocationCode = " + QuotedStr(tbSource.Text) + " "
                ResultField = "FACode, FAName, Qty"
            ElseIf Request.QueryString("ContainerId").ToString = "TrGLFABorrowID" Then
                Session("Filter") = "Select FACode, FAName, FA_SubGrp_Code, FA_SubGrp_Name, Qty FROM VMsFixedAssetDt WHERE FgActive = 'Y' AND Qty > 0 and Fg_Moving = 'Y' AND FALocationType = " + QuotedStr(ddlTypeSrc.SelectedValue) + " AND FALocationCode = " + QuotedStr(tbSource.Text) + " "
                ResultField = "FACode, FAName, Qty"
            Else
                Session("Filter") = "SELECT FixedAsset AS FACode, FixedAssetName As FAName, Qty_Borrow AS Qty FROM V_GLFAMoveDt WHERE Qty_Borrow > 0 AND TransNmbr = " + QuotedStr(tbReff.Text) + " "
                ResultField = "FACode, FAName, Qty"
            End If
            ViewState("Sender") = "btnFA"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search FixedAsset Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbFACode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbFACode.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Dim SQLString As String
        Try
            If Request.QueryString("ContainerId").ToString = "TrGLFAMoveID" Then
                SQLString = "Select FACode, FAName, FA_SubGrp_Code, FA_SubGrp_Name, Qty FROM VMsFixedAssetDt WHERE FgActive = 'Y' AND Qty > 0 and Fg_Moving = 'Y' AND FALocationType = " + QuotedStr(ddlTypeSrc.SelectedValue) + " AND FALocationCode = " + QuotedStr(tbSource.Text) + " AND FACode = " + QuotedStr(tbFACode.Text.Trim)
            ElseIf Request.QueryString("ContainerId").ToString = "TrGLFABorrowID" Then
                SQLString = "Select FACode, FAName, FA_SubGrp_Code, FA_SubGrp_Name, Qty FROM VMsFixedAssetDt WHERE FgActive = 'Y' AND Qty > 0 and Fg_Moving = 'Y' AND FALocationType = " + QuotedStr(ddlTypeSrc.SelectedValue) + " AND FALocationCode = " + QuotedStr(tbSource.Text) + " AND FACode = " + QuotedStr(tbFACode.Text.Trim)
            Else
                SQLString = "SELECT FixedAsset AS FACode, FixedAssetName As FAName, Qty_Borrow AS Qty FROM V_GLFAMoveDt WHERE Qty_Borrow > 0 AND TransNmbr = " + QuotedStr(tbReff.Text) + " AND FixedAsset = " + QuotedStr(tbFACode.Text.Trim)
            End If
            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbFACode.Text = Dr("FACode")
                tbFAName.Text = Dr("FAName")
                tbQty.Text = Dr("Qty")
                tbQty.Text = FormatFloat(tbQty.Text, ViewState("DigitQty"))
            Else
                tbFACode.Text = ""
                tbFAName.Text = ""
                tbQty.Text = "1"
            End If
            tbFACode.Focus()
        Catch ex As Exception
            Throw New Exception("tb FixedAsset Code TextChanged : " + ex.ToString)
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
        tbFACode.Focus()
    End Sub

    Function CekHd() As Boolean
        Try
            If tbDate.IsNull Then
                lbStatus.Text = MessageDlg("Date must have value")
                tbDate.Focus()
                Return False
            End If
            'If CInt(Session(Request.QueryString("KeyId"))("Year")) <> Year(tbDate.SelectedValue) Or CInt(Session(Request.QueryString("KeyId"))("Period")) <> Month(tbDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(Session(Request.QueryString("KeyId"))("Period")) + " " + Session(Request.QueryString("KeyId"))("Year").ToString.Trim)
            '    Return False
            'End If
            If tbSource.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Source must have value")
                tbSource.Focus()
                Return False
            End If
            If tbDest.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Destination must have value")
                tbDest.Focus()
                Return False
            End If
            If tbOperator.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Operator must have value")
                tbOperator.Focus()
                Return False
            End If

            Return True
        Catch ex As Exception
            Throw New Exception("Cek Hd Error : " + ex.ToString)
        End Try
    End Function

    Function CekDt(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not (Dr Is Nothing) Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If Dr("FixedAsset").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("FixedAsset Must Have Value")
                    Return False
                End If
                If Request.QueryString("ContainerId").ToString = "TrGLFABorrowID" Then
                    If Dr("DueDate").ToString.Trim = "" Then
                        lbStatus.Text = MessageDlg("Due Date Must Have Value")
                        Return False
                    End If
                End If
                If CFloat(Dr("Qty").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Must Have Value")
                    Return False
                End If
            Else
                If tbFACode.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("FixedAsset Must Have Value")
                    tbFACode.Focus()
                    Return False
                End If
                If Request.QueryString("ContainerId").ToString = "TrGLFABorrowID" Then

                End If
                If CFloat(tbQty.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Must Have Value")
                    tbQty.Focus()
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
            FilterName = "No, Date, Status, Reference, FA Type Location Src,  FA Location Src, FA Type Location Dest,  FA Location Dest, Operator, Remark"
            FilterValue = "TransNmbr, Trans_Date, Status, Reference, FA_Location_Type_Src,  FA_Location_Name_Src, FA_Location_Type_Dest,  FA_Location_Name_Dest, Operator, Remark"
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
            If Not (e.CommandName = "Sort") Or (e.CommandName = "Page") Then
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
                        ViewState("SetLocation") = True
                        MovePanel(PnlHd, pnlInput)
                        ViewState("TransNmbr") = GVR.Cells(2).Text
                        GridDt.PageIndex = 0
                        BindDataDt(ViewState("TransNmbr"))
                        FillTextBoxHd(ViewState("TransNmbr"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        btnHome.Visible = False
                        If Request.QueryString("MenuParam") = "FAN" Then
                            btnReff.Visible = True
                        Else
                            btnReff.Visible = False
                        End If
                        'ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, ViewState("DBConnection").ToString)
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    Else
                        lbStatus.Text = MessageDlg("Data must Hold or Get Approval to edit")
                        Exit Sub
                    End If
                ElseIf DDL.SelectedValue = "Print" Then
                    CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                    If CekMenu <> "" Then
                        lbStatus.Text = CekMenu
                        Exit Sub
                    End If
                    Session("SelectCommand") = "EXEC S_GLFormFAMove " + QuotedStr(GVR.Cells(2).Text) '+ " '+ GVR.Cells(3).Text"
                    If Request.QueryString("MenuParam") = "FAM" Then
                        Session("ReportFile") = Server.MapPath("~\Rpt\FormFAMove.frx")
                    ElseIf Request.QueryString("MenuParam") = "FAB" Then
                        Session("ReportFile") = Server.MapPath("~\Rpt\FormFAMoveBorrow.frx")
                    Else
                        Session("ReportFile") = Server.MapPath("~\Rpt\FormFAMoveReturn.frx")
                    End If
                    Session("DBConnection") = ViewState("DBConnection")
                    AttachScript("openprintdlg();", Page, Me.GetType)
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
        Dim dr() As DataRow
        Dim GVR As DataRow = ViewState("Dt").Rows(e.RowIndex)
        dr = ViewState("Dt").Select("FixedAsset = " + QuotedStr(GVR("FixedAsset").ToString))
        dr(0).Delete()
        BindGridDt(ViewState("Dt"), GridDt)
        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As DataRow
        Try
            GVR = ViewState("Dt").Rows(e.NewEditIndex)
            ViewState("DtValue") = GVR("FixedAsset").ToString
            FillTextBoxDt(ViewState("DtValue"))
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"

            tbFACode.Focus()
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbFA_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbFA.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsFixedAsset')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb FixedAsset Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            tbReference.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToText(tbReff, Dt.Rows(0)("Reference").ToString)
            BindToText(tbMoveType, Dt.Rows(0)("MoveType").ToString)
            BindToText(tbSource, Dt.Rows(0)("FA_Location_Code_Src").ToString)
            BindToText(tbSourceName, Dt.Rows(0)("FA_Location_Name_Src").ToString)
            BindToDropList(ddlTypeSrc, Dt.Rows(0)("FA_Location_Type_Src").ToString)
            BindToText(tbDest, Dt.Rows(0)("FA_Location_Code_Dest").ToString)
            BindToText(tbDestName, Dt.Rows(0)("FA_Location_Name_Dest").ToString)
            BindToDropList(ddlTypeDest, Dt.Rows(0)("FA_Location_Type_Dest").ToString)
            BindToText(tbOperator, Dt.Rows(0)("Operator").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal FixedAsset As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("FixedAsset = " + QuotedStr(FixedAsset))
            If Dr.Length > 0 Then
                BindToText(tbFACode, Dr(0)("FixedAsset").ToString)
                BindToText(tbFAName, Dr(0)("FixedAssetName").ToString)
                BindToText(tbQty, Dr(0)("Qty").ToString)
                BindToDate(tbDueDate, Dr(0)("DueDate").ToString)
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

    'Protected Sub tbSubLed_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbSubLed.TextChanged
    '    Dim Dr As DataRow
    '    Try
    '        Dr = FindMaster("SubLed", tbSubLed.Text, ViewState("DBConnection").ToString)
    '        If Not Dr Is Nothing Then
    '            tbSubLed.Text = Dr("SubLed_No")
    '            tbSubLedName.Text = Dr("SubLed_Name")
    '        Else
    '            tbSubLed.Text = ""
    '            tbSubLedName.Text = ""
    '        End If
    '        AttachScript("setformat();", Page, Me.GetType())
    '        tbSubLed.Focus()
    '    Catch ex As Exception
    '        Throw New Exception("tb CustCode Error : " + ex.ToString)
    '    End Try
    'End Sub

    Protected Sub btnGetDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetDt.Click
        Dim ResultField As String 'ResultSame 
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            Session("Result") = Nothing
            If Request.QueryString("ContainerId").ToString = "TrGLFAMoveID" Then
                Session("Filter") = "Select FACode, FAName, FA_SubGrp_Code, FA_SubGrp_Name, Qty FROM VMsFixedAssetDt WHERE FgActive = 'Y' AND Qty > 0 and Fg_Moving = 'Y' AND FALocationType = " + QuotedStr(ddlTypeSrc.SelectedValue) + " AND FALocationCode = " + QuotedStr(tbSource.Text) + " "
                ResultField = "FACode, FAName, Qty"
            ElseIf Request.QueryString("ContainerId").ToString = "TrGLFABorrowID" Then
                Session("Filter") = "Select FACode, FAName, FA_SubGrp_Code, FA_SubGrp_Name, Qty FROM VMsFixedAssetDt WHERE FgActive = 'Y' AND Qty > 0 and Fg_Moving = 'Y' AND FALocationType = " + QuotedStr(ddlTypeSrc.SelectedValue) + " AND FALocationCode = " + QuotedStr(tbSource.Text) + " "
                ResultField = "FACode, FAName, Qty"
            Else
                Session("Filter") = "SELECT FixedAsset AS FACode, FixedAssetName As FAName, Qty_Borrow AS Qty FROM V_GLFAMoveDt WHERE Qty_Borrow > 0 AND TransNmbr = " + QuotedStr(tbReff.Text) + " "
                ResultField = "FACode, FAName, Qty"
            End If

            Session("Column") = ResultField.Split(",")
            ViewState("Sender") = "btnGetDt"
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn get Dt Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub btnSupp_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnRRFrom.Click
    '    Dim ResultField As String
    '    Try
    '        If Request.QueryString("ContainerId").ToString = "FAMoveID" Then
    '            Session("filter") = "SELECT Customer_Code, Customer_Name FROM VmsCustomer"
    '            ResultField = "Customer_Code, Customer_Name"
    '            ViewState("Sender") = "btnCustomer"
    '            Session("Column") = ResultField.Split(",")
    '        ElseIf Request.QueryString("ContainerId").ToString = "FABorrowID" Then
    '            Session("filter") = "SELECT Supplier_Code, Supplier_Name FROM VmsSupplier"
    '            ResultField = "Supplier_Code, Supplier_Name"
    '            ViewState("Sender") = "btnSupplier"
    '            Session("Column") = ResultField.Split(",")
    '        End If

    '        AttachScript("OpenSearchDlg();", Page, Me.GetType())
    '    Catch ex As Exception
    '        lbStatus.Text = "btn Search Supplier No Error : " + ex.ToString
    '    End Try
    'End Sub

    'Private Function GetQty(ByVal Nmbr As String, ByVal FixedAsset As String, ByVal Qty As Double) As Double
    '    Dim dr As SqlDataReader
    '    dr = SQLExecuteReader("EXEC S_STRROtherGetQtyConvert " + QuotedStr(tbPONo.Text) + ", " + QuotedStr(tbFACode.Text) + " , " + QuotedStr(tbQtyOrder.Text), ViewState("DBConnection").ToString)
    '    dr.Read()
    '    Return dr("Qty")
    'End Function

    Private Function FindFixedAsset(ByVal Nmbr As String, ByVal FixedAsset As String) As String
        Dim dr As SqlDataReader
        dr = SQLExecuteReader("EXEC S_STRROtherFindProd " + QuotedStr(tbFACode.Text), ViewState("DBConnection").ToString)
        dr.Read()
        Return dr("FixedAsset")
    End Function


    Protected Sub btnSource_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSource.Click
        Dim ResultField As String
        Try
            Session("Filter") = "SELECT FA_Location_Code, FA_Location_Name FROM VMsFALocationAll where  FA_Location_Type = " + QuotedStr(ddlTypeSrc.SelectedValue)
            ResultField = "FA_Location_Code, FA_Location_Name"
            ViewState("Sender") = "btnSource"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Source Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnDest_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDest.Click
        Dim ResultField As String
        Try
            Session("Filter") = "SELECT FA_Location_Code, FA_Location_Name FROM VMsFALocationAll where  FA_Location_Type = " + QuotedStr(ddlTypeDest.SelectedValue)
            ResultField = "FA_Location_Code, FA_Location_Name"
            ViewState("Sender") = "btnDest"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Dest Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbSource_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbSource.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try
            Dt = SQLExecuteQuery("SELECT FA_Location_Code, FA_Location_Name FROM VMsFALocationAll where FA_Location_Type = '" + ddlTypeSrc.SelectedValue + "' AND FA_Location_Code = '" + tbSource.Text + "'", ViewState("DBConnection").ToString).Tables(0)
            'Dt = SQLExecuteQuery("EXEC S_FindFALocationAll '" + tbSource.Text + "', '" + ddlTypeSrc.SelectedValue + "'", ViewState("DBConnection").ToString).Tables(0)
            'Dr = FindMaster("FixedAsset", tbFACode.Text, ViewState("DBConnection").ToString)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbSource.Text = Dr("FA_Location_Code")
                tbSourceName.Text = Dr("FA_Location_Name")
            Else
                tbSource.Text = ""
                tbSourceName.Text = ""
            End If
            tbSource.Focus()
        Catch ex As Exception
            Throw New Exception("tb sourceError : " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlTypeSrc_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTypeSrc.SelectedIndexChanged
        tbSource.Text = ""
        tbSourceName.Text = ""
    End Sub

    Protected Sub ddlTypeDest_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTypeDest.SelectedIndexChanged
        tbDest.Text = ""
        tbDestName.Text = ""
    End Sub

    Protected Sub btnReff_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReff.Click
        Dim ResultField As String
        Try
            Session("Filter") = "SELECT DISTINCT TransNmbr, Trans_Date, FA_Location_Type_Src, FA_Location_Code_Src, FA_Location_Name_Src, FA_Location_Type_Dest, FA_Location_Code_Dest, FA_Location_Name_Dest, Operator, Remark  From V_GLFAMoveReff WHERE MoveType = 'FAB' "
            ResultField = "TransNmbr, FA_Location_Type_Src, FA_Location_Code_Src, FA_Location_Name_Src, FA_Location_Type_Dest, FA_Location_Code_Dest, FA_Location_Name_Dest, Operator, Remark  "
            ViewState("Sender") = "btnReff"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Reference Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub Page_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        Try
            Session("SelectCommand") = Nothing
            Session("ReportFile") = Nothing
            Session("PrintType") = Nothing
            WebReport1.Dispose()
        Catch ex As Exception
            lbStatus.Text = "page disposed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbDest_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbDest.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try
            Dt = SQLExecuteQuery("EXEC S_FindFALocationAll '" + tbDest.Text + "', '" + ddlTypeDest.SelectedValue + "'", ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbDest.Text = Dr("FA_Location_Code")
                tbDestName.Text = Dr("FA_Location_Name")
            Else
                tbDest.Text = ""
                tbDestName.Text = ""
            End If
            tbDest.Focus()
        Catch ex As Exception
            Throw New Exception("tb sourceError : " + ex.ToString)
        End Try
    End Sub

End Class
