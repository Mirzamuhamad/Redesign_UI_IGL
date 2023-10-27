Imports System.Data
Imports BasicFrame.WebControls

Partial Class Transaction_TrBeginGiroOut_TrBeginGiroOut
    Inherits System.Web.UI.Page
    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                SetInit()
                FillCombo(ddlBankPayment, "EXEC S_GetBankPayment " + QuotedStr("Y"), True, "Bank_Code", "Bank_Name", ViewState("DBConnection")) 'ddlReport.SelectedValue
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
            ViewState("DigitCurr") = 0
            GridView1.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
            FillCombo(ddlCurr, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection").ToString)
            'FillCombo(ddlBankPayment, "EXEC S_GetBankPayment", False, "Bank_Code", "Bank_Name")
            If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then

            End If
            Me.tbRate.Attributes.Add("OnKeyDown", "return PressNumeric();")
            Me.tbTotalForex.Attributes.Add("OnKeyDown", "return PressNumeric();")
            Me.tbRate.Attributes.Add("OnBlur", "setformat();")
            Me.tbTotalForex.Attributes.Add("OnBlur", "setformat();")
            'tbTotalForex.Attributes.Add("ReadOnly", "True")
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
            'If AdvanceFilter.Length > 1 And StrFilter.Length > 1 Then
            '    StrFilter = StrFilter + " And " + AdvanceFilter
            'ElseIf AdvanceFilter.Length > 1 And StrFilter.Length <= 1 Then
            '    StrFilter = AdvanceFilter
            'End If
            If AdvanceFilter.Length > 1 Then
                StrFilter = AdvanceFilter
            End If
            DT = BindDataTransaction("Select * From V_FNBeginGiroOut", StrFilter, ViewState("DBConnection").ToString)
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
                ViewState("SortExpression") = "PaymentDate DESC"
            End If
            DV.Sort = Session("SortExpression")
            GridView1.DataSource = DV
            GridView1.DataBind()
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
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
    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            ModifyInput(True, pnlInput)
            newTrans()
            ViewState("DigitCurr") = 0
            MovePanel(PnlHd, pnlInput)
            ChangeCurrency(ddlCurr, tbPaymentDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
            btnHome.Visible = False
            tbGiroNo.Enabled = True
            tbGiroNo.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub
    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            tbGiroNo.Text = ""
            ddlUserType.SelectedValue = "Supplier"
            tbUserCode.Text = ""
            tbUserName.Text = ""
            tbPaymentNo.Text = ""
            tbPaymentDate.SelectedDate = ViewState("ServerDate") 'Today
            tbDueDate.SelectedDate = ViewState("ServerDate") 'Today
            ddlCurr.SelectedValue = ViewState("Currency")
            tbRate.Text = "1"
            tbRate.Enabled = False

            tbTotalForex.Text = "0"
            tbRemark.Text = ""
            FillCombo(ddlBankPayment, "EXEC S_GetBankPayment " + QuotedStr("Y"), True, "Bank_Code", "Bank_Name", ViewState("DBConnection")) 'ddlReport.SelectedValue
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
                    Result = ExecSPCommandGo(ActionValue, "S_FNBeginGiroOut", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        GridView1.PageIndex = e.NewPageIndex
        BindData(2)
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
                        'FillCombo(ddlBankPayment, "EXEC S_GetBankPayment " + QuotedStr(ddlReport.SelectedValue) + "", True, "Bank_Code", "Bank_Name", ViewState("DBConnection"))
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
                    'Code For Print
                ElseIf DDL.SelectedValue = "Delete" Then
                    CekMenu = CheckMenuLevel("Delete", ViewState("MenuLevel").Rows(0))
                    If CekMenu <> "" Then
                        lbStatus.Text = CekMenu
                        Exit Sub
                    End If
                    'Code For Deleting
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
            Dt = BindDataTransaction("Select * From V_FNBeginGiroOut", "GiroNo = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            newTrans()
            tbGiroNo.Text = Nmbr
            'BindToDate(tbTransDate, Dt.Rows(0)("TransDate").ToString)
            'BindToDropList(ddlReport, Dt.Rows(0)("FgReport").ToString)
            BindToDropList(ddlUserType, Dt.Rows(0)("UserType").ToString)
            BindToText(tbUserCode, Dt.Rows(0)("UserCode").ToString)
            BindToText(tbUserName, Dt.Rows(0)("UserName").ToString)
            BindToText(tbPaymentNo, Dt.Rows(0)("PaymentNo").ToString)
            BindToDate(tbPaymentDate, Dt.Rows(0)("PaymentDate").ToString)
            BindToDate(tbDueDate, Dt.Rows(0)("DueDate").ToString)
            BindToDropList(ddlCurr, Dt.Rows(0)("Currency").ToString)
            BindToText(tbRate, Dt.Rows(0)("ForexRate").ToString)
            BindToDropList(ddlBankPayment, Dt.Rows(0)("BankPayment").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
            BindToText(tbTotalForex, Dt.Rows(0)("TotalForex").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click, btnHome.Click
        Try
            'PnlHd.Visible = True
            'pnlInput.Visible = False
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
            If tbPaymentNo.Text.Length <= 0 Then
                lbStatus.Text = MessageDlg("Payment No Cannot Empty.")
                tbPaymentNo.Focus()
                Return False
            End If
            If tbPaymentDate.IsNull Then
                lbStatus.Text = MessageDlg("Payment Date Cannot Empty.")
                tbPaymentDate.Focus()
                Return False
            End If
            If tbDueDate.IsNull Then
                lbStatus.Text = MessageDlg("Due Date Cannot Empty.")
                tbPaymentDate.Focus()
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
                Infois = SQLExecuteScalar("SELECT COALESCE(GiroNo, '') FROM FINBeginGiroOut WHERE GiroNo = " + QuotedStr(tbGiroNo.Text), ViewState("DBConnection").ToString)
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
        Dim tgl As String
        Try
            If tbPaymentDate.IsNull Then
                tgl = "NULL"
            Else
                tgl = QuotedStr(Format(tbPaymentDate.SelectedDate, "yyyy-MM-dd"))
            End If
            If Not (ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit") Then
                Exit Sub
            Else
                ViewState("StateSave") = ViewState("StateHd")
                ViewState("StateHd") = "View"
            End If
            'Save Hd
            If ViewState("StateSave") = "Insert" Then
                'insert                
                SQLString = "Insert INTO FINBeginGiroOut (GiroNo, STATUS, UserType, " + _
                "UserCode, FgReport, PaymentNo, PaymentDate, BankPayment, Currency, " + _
                "ForexRate, DueDate, TotalForex, Remark, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbGiroNo.Text) + ",'H'," + QuotedStr(ddlUserType.SelectedValue) + _
                "," + QuotedStr(tbUserCode.Text) + "," + QuotedStr("Y") + "," + _
                QuotedStr(tbPaymentNo.Text) + "," + QuotedStr(Format(tbPaymentDate.SelectedDate, "yyyy-MM-dd")) + _
                "," + QuotedStr(ddlBankPayment.SelectedValue) + "," + QuotedStr(ddlCurr.SelectedValue) + "," + _
                tbRate.Text.Replace(",", "") + "," + QuotedStr(Format(tbDueDate.SelectedDate, "yyyy-MM-dd")) + "," + _
                tbTotalForex.Text.Replace(",", "") + "," + QuotedStr(tbRemark.Text) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", getDate() "
            Else
                Dim CekStatus As String
                CekStatus = SQLExecuteScalar("Select Status FROM FINBeginGiroOut WHERE GiroNo = " + QuotedStr(tbGiroNo.Text), ViewState("DBConnection").ToString)
                If CekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                'edit
                SQLString = "UPDATE FINBeginGiroOut SET UserType = " + QuotedStr(ddlUserType.SelectedValue) + _
                ", FgReport=" + QuotedStr("Y") + ", UserCode=" + QuotedStr(tbUserCode.Text) + _
                ", PaymentNo=" + QuotedStr(tbPaymentNo.Text) + ", PaymentDate=" + QuotedStr(Format(tbPaymentDate.SelectedDate, "yyyy-MM-dd")) + _
                ", DueDate=" + QuotedStr(Format(tbDueDate.SelectedDate, "yyyy-MM-dd")) + ", Currency=" + QuotedStr(ddlCurr.SelectedValue) + _
                ", ForexRate=" + tbRate.Text.Replace(",", "") + ", BankPayment=" + QuotedStr(ddlBankPayment.SelectedValue) + _
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
            ChangeCurrency(ddlCurr, tbPaymentDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
            tbRate.Focus()
            AttachScript("setformat();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "ddl Curr ERror : " + ex.ToString
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
            Session("filter") = "select User_Code, User_Name, Currency, Term, Contact_Person FROM VMsUserType where User_Type = " + QuotedStr(ddlUserType.SelectedValue)
            ResultField = "User_Code, User_Name, Currency"
            ViewState("Sender") = "btnUser"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType)
        Catch ex As Exception
            lbStatus.Text = "btn User Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbUserCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbUserCode.TextChanged
        Dim dr As DataRow
        Try
            dr = FindMaster("UserType", tbUserCode.Text + "|" + ddlUserType.SelectedValue, ViewState("DBConnection").ToString)
            If Not dr Is Nothing Then
                tbUserCode.Text = dr("user_code")
                tbUserName.Text = dr("user_name")
            Else
                tbUserCode.Text = ""
                tbUserName.Text = ""
            End If
            tbUserCode.Focus()
        Catch ex As Exception
            lbStatus.Text = "tb usercode code error : " + ex.ToString
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
            tbUserCode.Focus()
        Catch ex As Exception
            lbStatus.Text = "ddlUserType_SelectedIndexChanged Code ERror : " + ex.ToString
        End Try
    End Sub

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        Dim FDateName, FDateValue, FilterName, FilterValue As String
        Try
            FDateName = "Payment Date, Due Date"
            FDateValue = "PaymentDate, DueDate"
            FilterName = "Reference, Status, User Type, User Code, User Name, Payment No, Bank Payment, Currency, Remark"
            FilterValue = "GiroNo, Status, UserType, UserCode, UserName, PaymentNo, BankPayment, Currency, Remark"
            Session("DateFieldName") = FDateName.Split(",")
            Session("DateFieldValue") = FDateValue.Split(",")
            Session("FieldName") = FilterName.Split(",")
            Session("FieldValue") = FilterValue.Split(",")
            AttachScript("OpenFilterCriteria();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "Advanced Search Click Error : " + ex.ToString
        End Try
        'Dim FieldName(6), FieldValue(6), DateFieldName(2), DateFieldValue(2) As String
        'Try
        '    DateFieldName(0) = "Payment Date"
        '    DateFieldName(1) = "Due Date"
        '    DateFieldValue(0) = "PaymentDate"
        '    DateFieldValue(1) = "DueDate"


        '    FieldName(0) = "Giro No"
        '    FieldName(1) = "Status"
        '    FieldName(2) = "User Type"
        '    FieldName(3) = "User Code"
        '    FieldName(4) = "User Name"
        '    FieldName(5) = "Payment No"
        '    FieldName(6) = "Currency"

        '    FieldValue(0) = "GiroNo"
        '    FieldValue(1) = "Status"
        '    FieldValue(2) = "UserType"
        '    FieldValue(3) = "UserCode"
        '    FieldValue(4) = "UserName"
        '    FieldValue(5) = "PaymentNo"
        '    FieldValue(6) = "Currency"

        '    Session("DateFieldName") = DateFieldName
        '    Session("DateFieldValue") = DateFieldValue
        '    Session("FieldName") = FieldName
        '    Session("FieldValue") = FieldValue

        '    If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
        '        Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenPopupSearch();", True)
        '    End If
        'Catch ex As Exception
        '    lbStatus.Text = "Advanced Search Click Error : " + ex.ToString
        'End Try
    End Sub

    'Protected Sub ddlReport_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlReport.SelectedIndexChanged
    '    FillCombo(ddlBankPayment, "EXEC S_GetBankPayment " + QuotedStr(ddlReport.SelectedValue) + "", True, "Bank_Code", "Bank_Name", ViewState("DBConnection"))
    '    ddlBankPayment.SelectedValue = ""
    'End Sub
End Class
