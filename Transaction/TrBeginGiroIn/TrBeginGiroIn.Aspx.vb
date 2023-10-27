Imports System.Data
Partial Class Transaction_TrBeginGiroIn_TrBeginGiroIn
    Inherits System.Web.UI.Page

    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Load
        Try
            If ViewState("DigitCurr") Is Nothing Then
                ViewState("DigitCurr") = 0
            End If
            If Not IsPostBack Then
                InitProperty()
                SetInit()
                Session("AdvanceFilter") = ""
            End If
            lbStatus.Text = ""

            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If

            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnUser" Then
                    tbUserCode.Text = Session("Result")(0).ToString
                    BindToText(tbUserName, Session("Result")(1).ToString)
                    BindToDropList(ddlCurr, Session("Result")(2).ToString)
                    ddlCurr_SelectedIndexChanged(Nothing, Nothing)
                End If
                Session("Result") = Nothing
                ViewState("Sender") = Nothing
                'Session("filter") = Nothing
                'Session("Column") = Nothing
            End If
        Catch ex As Exception
            lbStatus.Text = "Form Load Error :" + ex.ToString
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
        Try
            Session("SortExpression") = Nothing
            GridView1.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
            FillCombo(ddlCurr, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
            FillCombo(ddlBankGiro, "EXEC S_GetBank", False, "Bank_Code", "Bank_Name", ViewState("DBConnection"))
            If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then

            End If
            tbRate.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbTotalForex.Attributes.Add("OnKeyDown", "return PressNumeric();")

            tbRate.Attributes.Add("OnBlur", "setformat();")
            tbTotalForex.Attributes.Add("OnBlur", "setformat();")
        Catch ex As Exception
            Throw New Exception("Set Init Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            If AdvanceFilter.Length > 1 And StrFilter.Length > 1 Then
                StrFilter = StrFilter + " And " + AdvanceFilter
            ElseIf AdvanceFilter.Length > 1 And StrFilter.Length <= 1 Then
                StrFilter = AdvanceFilter
            End If
            DT = BindDataTransaction("Select * From V_FNBeginGiroIn", StrFilter, ViewState("DBConnection").ToString)
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
                ViewState("SortExpression") = "ReceiptDate DESC"
            End If
            DV.Sort = ViewState("SortExpression")
            GridView1.DataSource = DV
            GridView1.DataBind()
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            pnlNav.Visible = True
            BindData(Session("AdvanceFilter"))
            'ddlCommand.Visible = True
            'BtnGo.Visible = True
        Catch ex As Exception
            lbStatus.Text = "Btn Search Error : " + ex.ToString
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

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        'Dim FieldName(6), FieldValue(6), DateFieldName(2), DateFieldValue(2) As String
        'DateFieldName(0) = "Receipt Date"
        'DateFieldName(1) = "Due Date"
        'DateFieldValue(0) = "ReceiptDate"
        'DateFieldValue(1) = "DueDate"


        'FieldName(0) = "Reference"
        'FieldName(1) = "Status"
        'FieldName(2) = "User Type"
        'FieldName(3) = "User Code"
        'FieldName(4) = "User Name"
        'FieldName(5) = "Receipt No"
        'FieldName(6) = "Currency"

        'FieldValue(0) = "GiroNo"
        'FieldValue(1) = "Status"
        'FieldValue(2) = "UserType"
        'FieldValue(3) = "UserCode"
        'FieldValue(4) = "UserName"
        'FieldValue(5) = "ReceiptNo"
        'FieldValue(6) = "Currency"

        'Session("DateFieldName") = DateFieldName
        'Session("DateFieldValue") = DateFieldValue
        'Session("FieldName") = FieldName
        'Session("FieldValue") = FieldValue

        'If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
        '    Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenPopupSearch();", True)
        'End If
        Dim FDateName, FDateValue, FilterName, FilterValue As String
        Try
            FDateName = "Date"
            FDateValue = "TransDate"
            FilterName = "Giro No, Status, User Type, User, Receipt No, Bank Giro, Currency, Remark"
            FilterValue = "GiroNo, Status, UserType, UserName, ReceiptNo, BankGiro, Currency, Remark"
            Session("DateFieldName") = FDateName.Split(",")
            Session("DateFieldValue") = FDateValue.Split(",")
            Session("FieldName") = FilterName.Split(",")
            Session("FieldValue") = FilterValue.Split(",")
            AttachScript("OpenFilterCriteria();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "Advanced Search Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            ModifyInput(True, pnlInput)
            newTrans()
            ViewState("DigitCurr") = 0
            tbGiroNo.Enabled = True
            MovePanel(PnlHd, pnlInput)
            ChangeCurrency(ddlCurr, tbReceiptDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
            btnHome.Visible = False
            tbGiroNo.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            tbGiroNo.Text = ""
            tbReceiptDate.SelectedDate = ViewState("ServerDate") 'Today
            ddlUserType.SelectedIndex = 0
            tbUserCode.Text = ""
            tbUserName.Text = ""
            tbReceiptNo.Text = ""
            tbDueDate.SelectedDate = ViewState("ServerDate") 'Today
            ddlCurr.SelectedValue = ViewState("Currency").ToString
            tbRate.Text = "1"
            tbRate.ReadOnly = True
            tbTotalForex.Text = "0"
            tbRemark.Text = ""
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
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
                    Result = ExecSPCommandGo(ActionValue, "S_FNBeginGiroIn", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                    If Trim(Result) <> "" Then
                        lbStatus.Text = lbStatus.Text + Result + " <br/>"

                    End If
                End If
            Next
            BindData("GiroNo in (" + ListSelectNmbr + ")")
        Catch ex As Exception
            lbStatus.Text = "Go Command Error : " + ex.ToString
        End Try
    End Sub
    Private Sub GoCmdClick(ByVal Command As String, ByVal Value As String)
        Dim DGI As GridViewRow
        Dim Cb As CheckBox
        Dim Result, SqlString, StrTrans As String
        Dim Trans(100) As String
        Dim FirstTime As Boolean
        Dim DS As DataSet
        Dim i, j As Integer
        Try
            i = 0
            StrTrans = ""
            FirstTime = True
            For Each DGI In GridView1.Rows
                Cb = DGI.FindControl("cbSelect")
                If Cb.Checked Then
                    If Command <> DGI.Cells(3).Text And Value <> "Delete" Then
                        lbStatus.Text = lbStatus.Text + "status " + DGI.Cells(2).Text + " must be " + Command + " <br/>"
                    ElseIf Value = "Delete" And (Not (DGI.Cells(4).Text = "H" Or DGI.Cells(4).Text = "G")) Then
                        lbStatus.Text = lbStatus.Text + "status " + DGI.Cells(2).Text + " must be H or G <br/>"
                    Else
                        Trans(i) = DGI.Cells(2).Text
                        i = i + 1
                    End If

                    If FirstTime Then
                        FirstTime = False
                        StrTrans = "'" + DGI.Cells(2).Text + "'"
                    Else
                        StrTrans = StrTrans + ",'" + DGI.Cells(2).Text + "'"
                    End If
                End If
            Next

            If StrTrans = "" Then Exit Sub

            For j = 0 To 100
                If Trans(j) = "" Then
                    Exit For
                Else
                    Select Case Value
                        Case "Get Approval"
                            Result = ExecSPPosting("S_FNBeginGiroInGetAppr", Trans(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                            If Trim(Result) <> "" Then
                                lbStatus.Text = lbStatus.Text + Result + " <br/>"
                                'Exit For
                            End If
                        Case "Post"
                            Result = ExecSPPosting("S_FNBeginGiroInPost", Trans(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                            If Trim(Result) <> "" Then
                                lbStatus.Text = lbStatus.Text + Result + " <br/>"
                                'Exit For
                            End If
                        Case "Un-Post"
                            Result = ExecSPPosting("S_FNBeginGiroInUnPost", Trans(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                            If Trim(Result) <> "" Then
                                lbStatus.Text = lbStatus.Text + Result + " <br/>"
                                'Exit For
                            End If
                        Case "Delete"
                            SqlString = "EXEC S_FNBeginGiroInDelete ('" + Trans(j) + "')"
                            Result = SQLExecuteScalar(SqlString, ViewState("DBConnection").ToString)
                            If Result <> "0" Then
                                lbStatus.Text = lbStatus.Text + Result + " <br/>"
                                'Exit For
                            End If
                        Case Else
                            Command = "Komplit?"
                    End Select
                End If
            Next
            DS = SQLExecuteQuery("Select * from V_FNBeginGiroIn WHERE GiroNo in (" + StrTrans + ")", ViewState("DBConnection").ToString)
            GridView1.DataSource = DS.Tables(0)
            GridView1.DataBind()
        Catch ex As Exception
            Throw New Exception("go cmd Click Error : " + ex.ToString)
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
                    btnHome.Visible = True
                    MovePanel(PnlHd, pnlInput)
                    FillTextBox(GVR)
                    ModifyInput(False, pnlInput)
                    btnHome.Visible = True
                    ViewState("StateHd") = "View"
                ElseIf DDL.SelectedValue = "Edit" Then
                    Dim lbStatusTemp As String
                    lbStatusTemp = GVR.Cells(3).Text
                    If lbStatusTemp = "H" Or lbStatusTemp = "G" Then
                        CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        MovePanel(PnlHd, pnlInput)
                        ViewState("GiroNo") = GVR.Cells(2).Text
                        ViewState("StateHd") = "Edit"
                        ModifyInput(True, pnlInput)
                        FillTextBox(GVR)
                        btnHome.Visible = False
                        tbGiroNo.Enabled = False
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
                ElseIf DDL.SelectedValue = "Delete" Then
                    CekMenu = CheckMenuLevel("Delete", ViewState("MenuLevel").Rows(0))
                    If CekMenu <> "" Then
                        lbStatus.Text = CekMenu
                        Exit Sub
                    End If
                End If
            End If
        Catch ex As Exception
            lbStatus.Text = "Row Command Error : " + ex.ToString
        End Try
    End Sub

    Private Sub FillTextBox(ByVal e As GridViewRow)
        Dim Dt As DataTable
        Dim Nmbr As String
        Try
            Nmbr = e.Cells(2).Text
            Dt = BindDataTransaction("Select * From V_FNBeginGiroIn", "GiroNo = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            newTrans()
            tbGiroNo.Text = Nmbr
            'BindToDropList(ddlReport, Dt.Rows(0)("FgReport").ToString)
            BindToDropList(ddlUserType, Dt.Rows(0)("UserType").ToString)
            BindToText(tbUserCode, Dt.Rows(0)("UserCode").ToString)
            BindToText(tbUserName, Dt.Rows(0)("UserName").ToString)
            BindToText(tbReceiptNo, Dt.Rows(0)("ReceiptNo").ToString)
            BindToDate(tbReceiptDate, Dt.Rows(0)("ReceiptDate").ToString)
            BindToDate(tbDueDate, Dt.Rows(0)("DueDate").ToString)
            BindToDropList(ddlBankGiro, Dt.Rows(0)("BankGiro").ToString)
            BindToDropList(ddlCurr, Dt.Rows(0)("Currency").ToString)
            BindToText(tbRate, Dt.Rows(0)("ForexRate").ToString)
            BindToText(tbTotalForex, Dt.Rows(0)("TotalForex").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)

        Catch ex As Exception
            Throw New Exception("fill text box error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click, btnHome.Click
        Try
            MovePanel(pnlInput, PnlHd)
        Catch ex As Exception
            lbStatus.Text = "Btn Cancel Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim CurrFilter, Value As String
        Try
            If cekhd() = False Then
                Exit Sub
            End If
            SaveData()
            MovePanel(pnlInput, PnlHd)
            CurrFilter = tbFilter.Text
            Value = ddlField.SelectedValue
            tbFilter.Text = tbGiroNo.Text
            ddlField.SelectedValue = "GiroNo"
            Button1_Click(Nothing, Nothing)
            tbFilter.Text = CurrFilter
            ddlField.SelectedValue = Value
        Catch ex As Exception
            lbStatus.Text = "btn Save Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnSaveNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveNew.Click
        Try
            If cekhd() = False Then
                Exit Sub
            End If
            SaveData()
            'ViewState("StateHd") = "Insert"
            'newTrans()
            BtnAdd_Click(Nothing, Nothing)
        Catch ex As Exception
            lbStatus.Text = "btn Save New Error : " + ex.ToString
        End Try
    End Sub
    Function cekhd() As Boolean
        Dim Infois As String
        Try
            If tbGiroNo.Text.Trim.Length <= 0 Then
                lbStatus.Text = MessageDlg("Giro No Cannot Empty.")
                tbGiroNo.Focus()
                Return False
            End If
            If tbUserName.Text.Length <= 0 Then
                lbStatus.Text = MessageDlg("User Cannot Empty.")
                tbUserCode.Focus()
                Return False
            End If
            If tbReceiptNo.Text.Length <= 0 Then
                lbStatus.Text = MessageDlg("Receipt No Cannot Empty.")
                tbReceiptNo.Focus()
                Return False
            End If
            If tbReceiptDate.IsNull Then
                lbStatus.Text = MessageDlg("Receipt Date Cannot Empty.")
                tbReceiptDate.Focus()
                Return False
            End If
            If tbDueDate.IsNull Then
                lbStatus.Text = MessageDlg("Due Date Cannot Empty.")
                tbReceiptDate.Focus()
                Return False
            End If
            If tbReceiptDate.SelectedDate > tbDueDate.SelectedDate Then
                lbStatus.Text = MessageDlg("Receipt date must earlier than due date.")
                tbReceiptDate.Focus()
                Return False
            End If
            If CFloat(tbRate.Text) <= 0 Then
                lbStatus.Text = MessageDlg("Rate must have value")
                tbRate.Focus()
                Return False
            End If
            If ddlCurr.SelectedValue <> ViewState("Currency") And CFloat(tbRate.Text) = 1 Then
                lbStatus.Text = MessageDlg("Rate must have value")
                tbRate.Focus()
                Return False
            End If
            If CFloat(tbTotalForex.Text) <= 0 Then
                lbStatus.Text = MessageDlg("Total Forex Cannot Empty.")
                tbTotalForex.Focus()
                Return False
            End If
            If ViewState("StateHd") = "Insert" Then
                Infois = SQLExecuteScalar("SELECT COALESCE(GiroNo, '') FROM FINBeginGiroIn WHERE GiroNo = " + QuotedStr(tbGiroNo.Text), ViewState("DBConnection").ToString)
                If Infois.Length > 0 Then
                    lbStatus.Text = MessageDlg("Giro No Exist, Cannot Save Data")
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Hd Error : " + ex.ToString)
        End Try
    End Function
    Protected Sub SaveData()
        Dim SQLString As String
        Try
            If Not (ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit") Then
                Exit Sub
            Else
                ViewState("StateSave") = ViewState("StateHd")
                ViewState("StateHd") = "View"
            End If
            'Save Hd
            If ViewState("StateSave") = "Insert" Then
                'insert                
                SQLString = "Insert INTO FINBeginGiroIn (GiroNo, STATUS, UserType, " + _
                "UserCode, FgReport, ReceiptNo, ReceiptDate, BankGiro, Currency, " + _
                "ForexRate, DueDate, TotalForex, Remark, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbGiroNo.Text) + ",'H'," + QuotedStr(ddlUserType.SelectedValue) + _
                "," + QuotedStr(tbUserCode.Text) + "," + QuotedStr("Y") + "," + _
                QuotedStr(tbReceiptNo.Text) + "," + QuotedStr(Format(tbReceiptDate.SelectedDate, "yyyy-MM-dd")) + _
                "," + QuotedStr(ddlBankGiro.SelectedValue) + "," + QuotedStr(ddlCurr.SelectedValue) + "," + _
                tbRate.Text.Replace(",", "") + "," + QuotedStr(Format(tbDueDate.SelectedDate, "yyyy-MM-dd")) + "," + _
                tbTotalForex.Text.Replace(",", "") + "," + QuotedStr(tbRemark.Text) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", getDate() "
            Else
                Dim CekStatus As String
                CekStatus = SQLExecuteScalar("Select Status FROM FINBeginGiroIn WHERE GiroNo = " + QuotedStr(tbGiroNo.Text), ViewState("DBConnection").ToString)
                If CekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                'edit
                SQLString = "UPDATE FINBeginGiroIn SET UserType = " + QuotedStr(ddlUserType.SelectedValue) + _
                ", FgReport=" + QuotedStr("Y") + ", UserCode=" + QuotedStr(tbUserCode.Text) + _
                ", ReceiptNo=" + QuotedStr(tbReceiptNo.Text) + ", ReceiptDate=" + QuotedStr(Format(tbReceiptDate.SelectedDate, "yyyy-MM-dd")) + _
                ", DueDate=" + QuotedStr(Format(tbDueDate.SelectedDate, "yyyy-MM-dd")) + ", Currency=" + QuotedStr(ddlCurr.SelectedValue) + _
                ", ForexRate=" + tbRate.Text.Replace(",", "") + ", BankGiro=" + QuotedStr(ddlBankGiro.SelectedValue) + _
                ", TotalForex=" + tbTotalForex.Text.Replace(",", "") + _
                ", Remark=" + QuotedStr(tbRemark.Text) + _
                " WHERE GiroNo = " + QuotedStr(tbGiroNo.Text)
            End If
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
        Catch ex As Exception
            Throw New Exception("Save Data Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlCurr_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCurr.SelectedIndexChanged
        Try
            If ViewState("InputCurrency") = "Y" Then
                RefreshMaster("S_GetCurrency", "Currency", "Currency", ddlCurr, ViewState("DBConnection"))
                ViewState("InputCurrency") = Nothing
            End If
            ChangeCurrency(ddlCurr, tbReceiptDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
            tbRate.Focus()
            AttachScript("setformat();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "ddl Curr Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbCurr_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCurr.Click
        Try
            ViewState("InputCurrency") = "Y"
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsCurrency');", Page, Me.GetType)
        Catch ex As Exception
            lbStatus.Text = "lb Currency Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnUser_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUser.Click
        Dim ResultField As String
        Try
            Session("filter") = "select User_Code, User_Name, Currency, Contact_Person, Term FROM VMsUserType WHERE User_Type =" + QuotedStr(ddlUserType.SelectedValue)
            ResultField = "User_Code, User_Name, Currency"
            ViewState("Sender") = "btnUser"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType)
            tbUserCode.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn Search User Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbUserCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbUserCode.TextChanged
        Dim DsUser As DataSet
        Dim DrUser As DataRow
        Try
            DsUser = SQLExecuteQuery("EXEC S_FindUserType " + QuotedStr(tbUserCode.Text) + ", " + QuotedStr(ddlUserType.SelectedValue), ViewState("DBConnection"))
            If DsUser.Tables(0).Rows.Count = 1 Then
                DrUser = DsUser.Tables(0).Rows(0)
                tbUserCode.Text = DrUser("User_Code")
                tbUserName.Text = DrUser("User_Name")
                If DrUser("Currency").ToString.Trim <> "" Then
                    ddlCurr.SelectedValue = DrUser("Currency")
                End If
            Else
                tbUserCode.Text = ""
                tbUserName.Text = ""
            End If
            tbUserCode.Focus()
        Catch ex As Exception
            Throw New Exception("tb User Code Changed ERror : " + ex.ToString)
        End Try
    End Sub
    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim cb, cbselek As CheckBox
        Dim GRW As GridViewRow
        Try
            cb = sender
            For Each GRW In GridView1.Rows
                cbselek = GRW.FindControl("cbSelect")
                cbselek.Checked = cb.Checked
            Next
        Catch ex As Exception
            lbStatus.Text = "cb Hd Checked Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlUserType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlUserType.SelectedIndexChanged
        Try
            tbUserCode.Text = ""
            tbUserName.Text = ""
        Catch ex As Exception
            lbStatus.Text = "ddl User Type Selected Index Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbReceiptDate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbReceiptDate.SelectionChanged
        Try
            tbDueDate.SelectedDate = tbReceiptDate.SelectedDate
        Catch ex As Exception
            lbStatus.Text = "tb Receipt Date Selection Changed Error : " + ex.ToString
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

End Class
