Imports System.Data

Partial Class Master_MsPayType_MsPayType
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            InitProperty()
            DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
            DataGridDt.PageSize = CInt(ViewState("PageSizeGrid"))
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGridDt.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            FillCombo(ddlCurrCode, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
            FillCombo(ddlBank, "SELECT Bank_code, Bank_Name FROM VMsBank", True, "Bank_Code", "Bank_Name", ViewState("DBConnection"))
            FillCombo(ddlCostCtr, "SELECT Cost_Ctr_Code, Cost_Ctr_Name, FgAll FROM VMsCostCtr", True, "Cost_Ctr_Code", "Cost_Ctr_Name", ViewState("DBConnection"))
            'Dim ddlBank As DropDownList
            'ViewState("Bank") = ddlBank.SelectedValue
            'FillCombo(ViewState("Bank"), "SELECT BankCode, BankName From MsBank", True, "BankCode", "BankName")
            'bindDataGrid()
        End If

        If Not Session("Result") Is Nothing Then
            Dim Acc As New TextBox
            Dim AccName As New Label
            Dim CurrCode As New DropDownList

            If ViewState("Sender") = "SearchBankChargeAdd" Or ViewState("Sender") = "SearchBankChargeEdit" Then
                If ViewState("Sender") = "SearchBankChargeAdd" Then
                    Acc = DataGridDt.FooterRow.FindControl("AccBankChargeAdd")
                    AccName = DataGridDt.FooterRow.FindControl("AccBankChargeNameAdd")
                Else
                    Acc = DataGridDt.Rows(DataGridDt.EditIndex).FindControl("AccBankChargeEdit")
                    AccName = DataGridDt.Rows(DataGridDt.EditIndex).FindControl("AccBankChargeNameEdit")
                End If
                Acc.Text = Session("Result")(0).ToString
                AccName.Text = Session("Result")(1).ToString
            ElseIf ViewState("Sender") = "SearchAccountAdd" Or ViewState("Sender") = "SearchAccountEdit" Then
                If ViewState("Sender") = "SearchAccountAdd" Then
                    Acc = DataGrid.FooterRow.FindControl("AccountAdd")
                    AccName = DataGrid.FooterRow.FindControl("AccountNameAdd")
                    CurrCode = DataGrid.FooterRow.FindControl("CurrCodeAdd")
                Else
                    Acc = DataGrid.Rows(DataGrid.EditIndex).FindControl("AccountEdit")
                    AccName = DataGrid.Rows(DataGrid.EditIndex).FindControl("AccountNameEdit")
                    CurrCode = DataGrid.Rows(DataGrid.EditIndex).FindControl("CurrCodeEdit")
                End If
                Acc.Text = Session("Result")(0).ToString
                AccName.Text = Session("Result")(1).ToString
                CurrCode.SelectedValue = Session("Result")(2).ToString
            End If
            If ViewState("Sender") = "btnAcc" Then
                tbAccount.Text = Session("Result")(0).ToString
                tbAccName.Text = Session("Result")(1).ToString
                ddlCurrCode.SelectedValue = Session("Result")(2).ToString
            End If

            tbAccount.Focus()
            Session("Result") = Nothing
            ViewState("Sender") = Nothing
            ViewState("Sender") = Nothing
            Session("filter") = Nothing
            Session("Column") = Nothing
        End If
        dsBank.ConnectionString = ViewState("DBConnection")
        dsCurrency.ConnectionString = ViewState("DBConnection")
        lstatus.Text = ""
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

    Function CheckMenuLevel(ByVal CommandName As String) As Boolean
        Try
            If CommandName = "Edit" Then
                If ViewState("MenuLevel").Rows(0)("FgEdit") = "N" Then
                    lstatus.Text = "<script language='javascript'> {alert('You are not authorized to edit record. Please contact administrator')}</script>"
                    Return False
                    Exit Function
                End If
            End If
            If CommandName = "Delete" Then
                If ViewState("MenuLevel").Rows(0)("FgDelete") = "N" Then
                    lstatus.Text = "<script language='javascript'> {alert('You are not authorized to delete record. Please contact administrator')}</script>"
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Check Menu Level Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        Try
            DataGrid.PageIndex = 0
            bindDataGrid()
            PnelNav.Visible = True
        Catch ex As Exception
            lstatus.Text = "Btn Search Error : " + vbCrLf + ex.ToString
        End Try
    End Sub
    Protected Sub btnExpand_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExpand.Click
        Try
            tbfilter2.Text = ""
            If pnlSearch.Visible Then
                pnlSearch.Visible = False
            Else
                pnlSearch.Visible = True
            End If
        Catch ex As Exception
            lstatus.Text = "btn Expand Error : " + ex.ToString
        End Try
    End Sub
    Private Sub bindDataGrid()
        Dim tempDS As New DataSet()
        Dim DV As DataView
        Dim StrFilter As String
        Try
            tempDS = SQLExecuteQuery(" SELECT A.*, B.AccountName, C.BankName AS BankName, D.CostCtrName, " + _
            "CASE A.FgMode WHEN 'B' THEN 'Bank' " + _
             "WHEN 'G' THEN 'Giro' WHEN 'O' THEN 'Other' WHEN 'C' THEN 'Cash' WHEN 'D' THEN 'DP' WHEN 'B' THEN 'BANK' WHEN 'C' THEN 'CN' WHEN 'N' THEN 'DN' WHEN 'I' THEN 'Income' WHEN 'E' THEN 'Expense' WHEN 'K' THEN 'Kas' END FgModeName, " + _
             "CASE A.FgType WHEN 'P' THEN 'Payment' " + _
             "WHEN 'R' THEN 'Receipt' WHEN 'A' THEN 'All' END FgTypeName " + _
             "FROM MsPayType A LEFT OUTER JOIN " + _
             "MsAccount B ON A.Account = B.Account LEFT OUTER JOIN " + _
             "MsCostCtr D ON A.CostCtr = D.CostCtrCode LEFT OUTER JOIN " + _
             "MsBank C ON A.Bank = C.BankCode ", ViewState("DBConnection").ToString)

            DV = tempDS.Tables(0).DefaultView
            StrFilter = ""
            If tbFilter.Text.Trim.Length > 0 Then
                If tbfilter2.Text.Trim.Length > 0 And pnlSearch.Visible Then
                    StrFilter = ddlField.SelectedValue + " like '%" + tbFilter.Text + "%' " + _
                    ddlNotasi.SelectedValue + " " + ddlField2.SelectedValue + " like '%" + tbfilter2.Text + "%'"
                Else
                    StrFilter = ddlField.SelectedValue + " like '%" + tbFilter.Text + "%'"
                End If
            Else
                StrFilter = ""
            End If
            DV.RowFilter = StrFilter

            If DV.Count = 0 Then
                'Dim DT As DataTable = tempDS.Tables(0)
                'ShowGridViewIfEmpty(DT, DataGrid)
                'DV = DT.DefaultView
                lstatus.Text = "No Data"
                DataGrid.Visible = False
                btnAdd2.Visible = False
            Else
                If ViewState("SortExpression") = Nothing Then
                    ViewState("SortExpression") = "PayCode ASC"
                End If

                DataGrid.Visible = True
                btnAdd2.Visible = True

                DV.Sort = ViewState("SortExpression")
                DataGrid.DataSource = DV
                DataGrid.DataBind()
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub


    Private Sub bindDataGridDt()
        Dim tempDS As New DataSet()
        Dim DV As DataView
        Dim SqlString As String
        Try
            SqlString = "SELECT A.PayCode, A.Currency, A.AccBankCharge, dbo.FormatFloat(A.ExpenseCharge,2) AS ExpenseCharge, A.FgDefault, A.UserId, A.UserDate, C.AccountName AS AccBankChargeName " + _
            "FROM MsPayTypeCharge A  LEFT OUTER JOIN MsAccount C ON A.AccBankCharge = C.Account " + _
            "WHERE A.PayCode = " + QuotedStr(ViewState("Nmbr"))

            tempDS = SQLExecuteQuery(SqlString, ViewState("DBConnection").ToString)

            DV = tempDS.Tables(0).DefaultView

            If DV.Count = 0 Then
                Dim DT As DataTable = tempDS.Tables(0)
                ShowGridViewIfEmpty(DT, DataGridDt)
                DV = DT.DefaultView
            Else
                DV.Sort = ViewState("SortExpressionDt")
                DataGridDt.DataSource = DV
                DataGridDt.DataBind()
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "bindDataGridDt Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Public Sub DataGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGrid.RowEditing
        Dim obj As GridViewRow
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If

            obj = DataGrid.Rows(e.NewEditIndex)
            pnlHd.Visible = False
            pnlInput.Visible = True
            tbPayCode.Enabled = False

            FillTextBox(obj.Cells(0).Text)

            'tbPayCode.Text = obj.Cells(0).Text
            'tbPayName.Text = obj.Cells(1).Text
            'tbAccount.Text = obj.Cells(2).Text
            'BindToText(tbAccName, obj.Cells(3).Text)
            'ddlCurrCode.SelectedValue = obj.Cells(4).Text
            'ddlFgmode.SelectedValue = obj.Cells(5).Text
            'ddlType.SelectedValue = obj.Cells(6).Text
            'ddlBank.SelectedValue = obj.Cells(7).Text
            'tbNorek.Text = obj.Cells(8).Text
            'tbNamaRekening.Text = obj.Cells(9).Text
            'tbSwift.Text = obj.Cells(10).Text
            'tbBankBranch.Text = obj.Cells(11).Text
            'tbBankAddr.Text = obj.Cells(12).Text
            'tbBankPhone.Text = obj.Cells(13).Text
            'tbBankFax.Text = obj.Cells(14).Text
            'tbCP.Text = obj.Cells(15).Text
            'tbCPAddress.Text = obj.Cells(16).Text
            'tbCPPhone.Text = obj.Cells(17).Text

            ViewState("State") = "Edit"

            If ddlFgmode.SelectedIndex <> 1 Then
                ddlBank.Enabled = False
                tbNorek.Enabled = False
                tbNamaRekening.Enabled = False
                tbSwift.Enabled = False
                tbBankBranch.Enabled = False
                tbBankAddr.Enabled = False
                tbBankPhone.Enabled = False
                tbBankFax.Enabled = False
                tbCP.Enabled = False
                tbCPAddress.Enabled = False
                tbCPPhone.Enabled = False
                tbNorek.Text = ""
                tbNamaRekening.Text = ""
                tbSwift.Text = ""
                tbBankBranch.Text = ""
                tbBankAddr.Text = ""
                tbBankPhone.Text = ""
                tbBankFax.Text = ""
                tbCP.Text = ""
                tbCPAddress.Text = ""
                tbCPPhone.Text = ""
            Else
                ddlBank.Enabled = True
                tbNorek.Enabled = True
                tbNamaRekening.Enabled = True
                tbSwift.Enabled = True
                tbBankBranch.Enabled = True
                tbBankAddr.Enabled = True
                tbBankPhone.Enabled = True
                tbBankFax.Enabled = True
                tbCP.Enabled = True
                tbCPAddress.Enabled = True
                tbCPPhone.Enabled = True
            End If

        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try
    End Sub
    Public Sub DataGridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DataGridDt.RowEditing
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            DataGridDt.EditIndex = e.NewEditIndex
            DataGridDt.ShowFooter = False
            bindDataGridDt()
        Catch ex As Exception
            lstatus.Text = "DataGrid_Edit exception : " + ex.ToString
        End Try

    End Sub
    Private Sub DataGrid_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles DataGrid.RowCancelingEdit
        Try
            DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGrid.EditIndex = -1
            bindDataGrid()
        Catch ex As Exception
            lstatus.Text = "DataGrid_Cancel Error : " + vbCrLf + ex.ToString
        End Try

    End Sub
    Private Sub DataGridDt_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles DataGridDt.RowCancelingEdit
        Try
            DataGridDt.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGridDt.EditIndex = -1
            bindDataGridDt()
        Catch ex As Exception
            lstatus.Text = "DataGridDt_Cancel Error : " + vbCrLf + ex.ToString
        End Try

    End Sub

    Protected Sub DataGrid_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGrid.RowCommand
        Dim GVR As GridViewRow = Nothing
        Dim DDL As DropDownList
        Dim index As Integer
        Try
            If Not (e.CommandName = "Sort" Or e.CommandName = "Page") Then
                index = Convert.ToInt32(e.CommandArgument)
                GVR = DataGrid.Rows(index)
            End If
            If e.CommandName = "Insert" Then
                '    GVR = DataGrid.FooterRow
                '    dbCode = GVR.FindControl("PayCodeAdd")
                '    dbName = GVR.FindControl("PayNameAdd")
                '    dbAccount = GVR.FindControl("AccountAdd")
                '    dbNoRekening = GVR.FindControl("NoRekeningAdd")
                '    dbNamarekening = GVR.FindControl("NamaRekeningAdd")
                '    dbSwiftCode = GVR.FindControl("SwiftCodeAdd")
                '    dbBankBranch = GVR.FindControl("BankBranchAdd")
                '    dbBankAddr = GVR.FindControl("BankAddrAdd")
                '    dbBankPhone = GVR.FindControl("BankPhoneAdd")
                '    dbBankFax = GVR.FindControl("BankFaxAdd")
                '    dbContactPerson = GVR.FindControl("ContactPersonAdd")
                '    dbContactAddr = GVR.FindControl("ContactAddrAdd")
                '    dbContactPhone = GVR.FindControl("ContactPhoneAdd")
                '    ddlCurr = GVR.FindControl("CurrCodeAdd")
                '    ddlBank = GVR.FindControl("BankAdd")
                '    ddlMode = GVR.FindControl("ModeAdd")
                '    ddlType = GVR.FindControl("TypeAdd")

                '    If dbCode.Text.Trim.Length = 0 Then
                '        lstatus.Text = "<script language='javascript'>alert('Payment Type Code must be filled');</script>"
                '        dbCode.Focus()
                '        Exit Sub
                '    End If
                '    If dbName.Text.Trim.Length = 0 Then
                '        lstatus.Text = "<script language='javascript'>alert('Payment Type Name must be filled');</script>"
                '        dbName.Focus()
                '        Exit Sub
                '    End If

                '    If dbAccount.Text.Trim.Length = 0 Then
                '        lstatus.Text = "<script language='javascript'>alert('Account must be filled');</script>"
                '        dbAccount.Focus()
                '        Exit Sub
                '    End If


                '    If SQLExecuteScalar("SELECT PayCode From MsPayType WHERE PayCode = " + QuotedStr(dbCode.Text), Session("DBConnection").ToString).Length > 0 Then
                '        lstatus.Text = "Payment " + QuotedStr(dbCode.Text) + " has already been exist"
                '        Exit Sub
                '    End If

                '    'kecuali mode bank maka akan tersimpan kosong
                '    If ddlMode.SelectedIndex <> 1 Then
                '        StrBank = ""
                '    Else
                '        StrBank = ddlBank.SelectedValue
                '    End If

                '    'insert the new entry
                '    SQLString = "Insert into MsPayType (PayCode, PayName, Account, CurrCode, FgMode, FgType, Bank, NoRekening, NamaRekening, SwiftCode, BankBranch, BankAddr, BankPhone, BankFax, ContactPerson, ContactAddr, ContactPhone, UserID, UserDate) " + _
                '    "SELECT " + QuotedStr(dbCode.Text) + "," + QuotedStr(dbName.Text) + "," + QuotedStr(dbAccount.Text) + ", " + QuotedStr(ddlCurr.SelectedValue) + "," + _
                '    QuotedStr(ddlMode.SelectedValue) + "," + QuotedStr(ddlType.SelectedValue) + ", '" + StrBank + "', " + _
                '    QuotedStr(dbNoRekening.Text) + ", " + QuotedStr(dbNamarekening.Text) + "," + QuotedStr(dbSwiftCode.Text) + ", " + QuotedStr(dbBankBranch.Text) + "," + _
                '    QuotedStr(dbBankAddr.Text) + "," + QuotedStr(dbBankPhone.Text) + "," + QuotedStr(dbBankFax.Text) + ", " + QuotedStr(dbContactPerson.Text) + "," + _
                '    QuotedStr(dbContactAddr.Text) + "," + QuotedStr(dbContactPhone.Text) + "," + _
                '    QuotedStr(Session("UserId").ToString) + ", GetDate()"
                '    SQLExecuteNonQuery(SQLString)
                '    bindDataGrid()
            ElseIf e.CommandName = "View" Then
                GVR = DataGrid.Rows(CInt(e.CommandArgument))
                ViewState("Nmbr") = GVR.Cells(0).Text
                lbPayType.Text = GVR.Cells(0).Text + " - " + GVR.Cells(1).Text
                pnlHd.Visible = False
                pnlDt.Visible = True
                bindDataGridDt()
            ElseIf e.CommandName = "Assign" Then
                Dim paramgo As String
                GVR = DataGrid.Rows(CInt(e.CommandArgument))
                paramgo = GVR.Cells(0).Text + "|" + GVR.Cells(1).Text
                If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenAssign('Pay Type', '" + Request.QueryString("KeyId") + "', '" + paramgo + "','AssMsPayTypeUser');", True)
                End If
            ElseIf e.CommandName = "Go" Then
                DDL = DataGrid.Rows(index).FindControl("ddl")
                ViewState("Nmbr") = GVR.Cells(1).Text
                If DDL.SelectedValue = "View" Then
                    ViewState("State") = "View"
                    'GVR = DataGrid.Rows(CInt(e.CommandArgument))
                    'ViewState("Nmbr") = GVR.Cells(1).Text
                    FillTextBox(ViewState("Nmbr"))
                    BtnSave.Visible = False
                    btnReset.Visible = False
                    ModifyInput(False)
                    pnlHd.Visible = False
                    pnlInput.Visible = True
                    bindDataGridDt()
                ElseIf DDL.SelectedValue = "Edit" Then
                    ViewState("State") = "Edit"
                    'GVR = DataGrid.Rows(CInt(e.CommandArgument))
                    'ViewState("Nmbr") = GVR.Cells(1).Text
                    FillTextBox(ViewState("Nmbr"))
                    ModifyInput(True)
                    pnlHd.Visible = False
                    pnlInput.Visible = True
                    tbPayCode.Enabled = False
                    BtnSave.Visible = True
                    btnReset.Visible = True
                ElseIf DDL.SelectedValue = "Detail" Then
                    'GVR = DataGrid.Rows(CInt(e.CommandArgument))
                    'ViewState("Nmbr") = GVR.Cells(1).Text
                    lbPayType.Text = GVR.Cells(1).Text + " - " + GVR.Cells(2).Text
                    pnlHd.Visible = False
                    pnlDt.Visible = True
                    bindDataGridDt()
                ElseIf DDL.SelectedValue = "User" Then
                    Dim paramgo As String
                    'GVR = DataGrid.Rows(CInt(e.CommandArgument))
                    paramgo = GVR.Cells(1).Text + "|" + GVR.Cells(2).Text
                    If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                        Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenAssign('Pay Type', '" + Request.QueryString("KeyId") + "','" + paramgo + "','AssMsPayTypeUser');", True)
                    End If
                ElseIf DDL.SelectedValue = "Delete" Then
                    Try
                        If CheckMenuLevel("Delete") = False Then
                            Exit Sub
                        End If
                        SQLExecuteNonQuery("Delete from MsPayTypeUser where PayCode = '" & GVR.Cells(1).Text & "'", ViewState("DBConnection").ToString)
                        SQLExecuteNonQuery("Delete from MsPayTypeCharge where PayCode = '" & GVR.Cells(1).Text & "'", ViewState("DBConnection").ToString)
                        SQLExecuteNonQuery("Delete from MsPayType where PayCode = '" & GVR.Cells(1).Text & "'", ViewState("DBConnection").ToString)
                        bindDataGrid()
                    Catch ex As Exception
                        lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
                    End Try
                End If
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub
    Public Sub DataGridDt_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DataGridDt.RowCommand
        Dim tbAccBankCharge, tbExpenseCharge As TextBox
        Dim ddlCurr, ddlDefault As DropDownList
        Dim lbCurr As Label
        Dim SQLString As String
        Dim GVR As GridViewRow
        Try
            If e.CommandName = "Insert" Then
                GVR = DataGridDt.FooterRow
                ddlCurr = GVR.FindControl("CurrCodeDtAdd")
                ddlDefault = GVR.FindControl("DefaultAdd")
                tbAccBankCharge = GVR.FindControl("AccBankChargeAdd")
                tbExpenseCharge = GVR.FindControl("ExpenseChargeAdd")

                If tbAccBankCharge.Text.Trim = "" Then
                    lstatus.Text = "<script language='javascript'>alert('Account Bank Charge must be filled');</script>"
                    tbAccBankCharge.Focus()
                    Exit Sub
                End If
                If IsNumeric(tbExpenseCharge.Text.Replace(",", "")) = 0 Then
                    lstatus.Text = MessageDlg("Expense Charge must be in numeric.")
                    tbExpenseCharge.Focus()
                    Exit Sub
                End If
                If CFloat(tbExpenseCharge.Text) <= 0 Then
                    lstatus.Text = MessageDlg("Expense Charge must be filled.")
                    tbExpenseCharge.Focus()
                    Exit Sub
                End If

                If SQLExecuteScalar("SELECT PayCode, Currency From MsPayTypeCharge WHERE PayCode = " + QuotedStr(ViewState("Nmbr")) + " AND Currency = " + QuotedStr(ddlCurr.SelectedValue), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = MessageDlg("Currency " + QuotedStr(ddlCurr.SelectedValue) + " has already been exist")
                    Exit Sub
                End If

                SQLString = "Insert Into MsPayTypeCharge (PayCode, Currency, AccBankCharge, ExpenseCharge, FgDefault, UserId, UserDate) " + _
                "SELECT " + QuotedStr(ViewState("Nmbr")) + "," + QuotedStr(ddlCurr.SelectedValue) + "," + _
                QuotedStr(tbAccBankCharge.Text) + "," + tbExpenseCharge.Text.Replace(",", "") + "," + QuotedStr(ddlDefault.SelectedValue) + "," + _
                QuotedStr(ViewState("UserId")) + ", getDate()"

                SQLString = Replace(SQLString, "''", "NULL")
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                bindDataGridDt()
            ElseIf e.CommandName = "SearchBankChargeEdit" Or e.CommandName = "SearchBankChargeAdd" Then
                Session("DBConnection") = ViewState("DBConnection")
                Dim FieldResult As String
                FieldResult = "Account, Description"
                Session("Column") = FieldResult.Split(",")
                If e.CommandName = "SearchBankChargeAdd" Then
                    ddlCurr = DataGridDt.FooterRow.FindControl("CurrCodeDtAdd")
                    ViewState("Sender") = "SearchBankChargeAdd"
                    Session("filter") = "Select Account, Description FROM VMsAccount Where ( Currency =" + QuotedStr(ViewState("Currency")) + " OR Currency = " + QuotedStr(ddlCurr.SelectedValue) + " ) AND FgActive = 'Y' AND FgType = 'PL' and FgNormal = 'D' and FgSubled IN ('N','F')"
                Else
                    lbCurr = DataGridDt.Rows(DataGridDt.EditIndex).FindControl("CurrCodeDtEdit")
                    ViewState("Sender") = "SearchBankChargeEdit"
                    Session("filter") = "Select Account, Description FROM VMsAccount Where ( Currency =" + QuotedStr(ViewState("Currency")) + " OR Currency = " + QuotedStr(lbCurr.Text) + " ) AND FgActive = 'Y' AND FgType = 'PL' and FgNormal = 'D' and FgSubled IN ('N','F')"
                End If
                If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenPopup();", True)
                End If
            End If
        Catch ex As Exception
            lstatus.Text = "Item Command Dt Error" + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGrid.RowUpdating
        'Dim SQLString, StrBank As String
        'Dim dbName, dbAccount, dbNoRekening, dbNamarekening, dbSwiftCode, dbBankBranch, dbBankAddr, dbBankPhone, dbBankFax, dbContactPerson, dbContactAddr, dbContactPhone As TextBox
        'Dim lbCode As Label
        'Dim ddlCurr, ddlBank, ddlMode, ddlType As DropDownList
        'Dim GVR As GridViewRow
        'Try
        '    GVR = DataGrid.Rows(e.RowIndex)
        '    lbCode = GVR.FindControl("PayCodeEdit")
        '    dbName = GVR.FindControl("PayNameEdit")
        '    dbAccount = GVR.FindControl("AccountEdit")
        '    dbNoRekening = GVR.FindControl("NoRekeningEdit")
        '    dbNamarekening = GVR.FindControl("NamaRekeningEdit")
        '    dbSwiftCode = GVR.FindControl("SwiftCodeEdit")
        '    dbBankBranch = GVR.FindControl("BankBranchEdit")
        '    dbBankAddr = GVR.FindControl("BankAddrEdit")
        '    dbBankPhone = GVR.FindControl("BankPhoneEdit")
        '    dbBankFax = GVR.FindControl("BankFaxEdit")
        '    dbContactPerson = GVR.FindControl("ContactPersonEdit")
        '    dbContactAddr = GVR.FindControl("ContactAddrEdit")
        '    dbContactPhone = GVR.FindControl("ContactPhoneEdit")
        '    ddlCurr = GVR.FindControl("CurrCodeEdit")
        '    ddlMode = GVR.FindControl("ModeEdit")
        '    ddlType = GVR.FindControl("TypeEdit")
        '    ddlBank = GVR.FindControl("BankEdit")

        '    If dbName.Text.Trim.Length = 0 Then
        '        lstatus.Text = "<script language='javascript'>alert('Payment Type Name must be filled.');</script>"
        '        dbName.Focus()
        '        Exit Sub
        '    End If

        '    'kecuali mode bank maka akan tersimpan kosong
        '    If ddlMode.SelectedIndex <> 1 Then
        '        StrBank = ""
        '    Else
        '        StrBank = ddlBank.SelectedValue
        '    End If

        '    'SQLString = "UPDATE MsPayType SET PayName = " + QuotedStr(dbName.Text) + _
        '    '", Account= " + QuotedStr(dbAccount.Text) + _
        '    '", CurrCode= " + QuotedStr(ddlCurr.SelectedValue) + _
        '    '" Where PayCode = " + QuotedStr(lbCode.Text)


        '    SQLString = "Update MsPayType set PayName =" + QuotedStr(dbName.Text) + ", " + _
        '    "Account =" + QuotedStr(dbAccount.Text) + ", CurrCode =" + QuotedStr(ddlCurr.SelectedValue) + ", " + _
        '    "FgMode =" + QuotedStr(ddlMode.SelectedValue) + ", FgType =" + QuotedStr(ddlType.SelectedValue) + ", " + _
        '    "Bank = '" + StrBank + "', " + _
        '    "NoRekening =" + QuotedStr(dbNoRekening.Text) + ", " + _
        '    "NamaRekening =" + QuotedStr(dbNamarekening.Text) + ", SwiftCode =" + QuotedStr(dbSwiftCode.Text) + ", BankBranch =" + QuotedStr(dbBankBranch.Text) + ", " + _
        '    "BankAddr =" + QuotedStr(dbBankAddr.Text) + ", BankPhone =" + QuotedStr(dbBankPhone.Text) + ", BankFax =" + QuotedStr(dbBankFax.Text) + ", " + _
        '    "ContactPerson =" + QuotedStr(dbContactPerson.Text) + ", ContactAddr =" + QuotedStr(dbContactAddr.Text) + ", ContactPhone =" + QuotedStr(dbContactPhone.Text) + _
        '    " where PayCode = " & QuotedStr(lbCode.Text)

        '    SQLExecuteNonQuery(SQLString)

        '    DataGrid.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
        '    DataGrid.EditIndex = -1
        '    bindDataGrid()

        'Catch ex As Exception
        '    lstatus.Text = lstatus.Text + "DataGrid_Update Error: " & ex.ToString
        'End Try
    End Sub
    Protected Sub DataGridDt_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DataGridDt.RowUpdating
        Dim tbAccBankCharge, tbExpenseCharge As TextBox
        Dim lbCurr As Label
        Dim ddlDefault As DropDownList
        Dim SQLString As String
        Dim GVR As GridViewRow
        Try
            GVR = DataGridDt.Rows(e.RowIndex)
            lbCurr = GVR.FindControl("CurrCodeDtEdit")
            ddlDefault = GVR.FindControl("DefaultEdit")
            tbAccBankCharge = GVR.FindControl("AccBankChargeEdit")
            tbExpenseCharge = GVR.FindControl("ExpenseChargeEdit")

            If IsNumeric(tbExpenseCharge.Text.Replace(",", "")) = 0 Then
                lstatus.Text = MessageDlg("Expense Charge must be in numeric.")
                tbExpenseCharge.Focus()
                Exit Sub
            End If
            If CFloat(tbExpenseCharge.Text) <= 0 Then
                lstatus.Text = MessageDlg("Expense Charge must be filled.")
                tbExpenseCharge.Focus()
                Exit Sub
            End If

            SQLString = "UPDATE MsPayTypeCharge SET AccBankCharge = " + QuotedStr(tbAccBankCharge.Text) + _
            ", ExpenseCharge= " + tbExpenseCharge.Text.Replace(",", "") + _
            ", FgDefault= " + QuotedStr(ddlDefault.SelectedValue) + " WHERE Currency = " + QuotedStr(lbCurr.Text) + _
            " AND PayCode =" + QuotedStr(ViewState("Nmbr"))

            SQLString = Replace(SQLString, "''", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            DataGridDt.ShowFooter = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
            DataGridDt.EditIndex = -1
            bindDataGridDt()
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "Datagrid dt update Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGrid.RowDeleting
        Dim GVR As GridViewRow = DataGrid.Rows(e.RowIndex)
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            'txtID = DataGrid.Rows(e.RowIndex).FindControl("PayCode")
            SQLExecuteNonQuery("Delete from MsPayTypeCharge where PayCode = '" & GVR.Cells(0).Text & "'", ViewState("DBConnection").ToString)
            SQLExecuteNonQuery("Delete from MsPayType where PayCode = '" & GVR.Cells(0).Text & "'", ViewState("DBConnection").ToString)
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub
    Protected Sub DataGridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DataGridDt.RowDeleting
        Dim txtID As Label
        Try
            If CheckMenuLevel("Delete") = False Then
                Exit Sub
            End If
            txtID = DataGridDt.Rows(e.RowIndex).FindControl("CurrCodeDt")

            SQLExecuteNonQuery("Delete from MsPayTypeCharge where PayCode = " + QuotedStr(ViewState("Nmbr")) + " AND Currency =" + QuotedStr(txtID.Text), ViewState("DBConnection").ToString)
            bindDataGridDt()

        Catch ex As Exception
            lstatus.Text = "DataGrid_Delete Error: " & ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGrid.PageIndexChanging
        DataGrid.PageIndex = e.NewPageIndex
        If DataGrid.EditIndex <> -1 Then
            DataGrid_RowCancelingEdit(Nothing, Nothing)
        End If
        bindDataGrid()
    End Sub
    Protected Sub DataGridDt_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGridDt.PageIndexChanging
        DataGridDt.PageIndex = e.NewPageIndex
        If DataGridDt.EditIndex <> -1 Then
            DataGridDt_RowCancelingEdit(Nothing, Nothing)
        End If
        bindDataGridDt()
    End Sub

    Protected Sub DataGrid_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles DataGrid.Sorting
        Try
            If ViewState("SortOrder") = Nothing Or ViewState("SortOrder") = "DESC" Then
                ViewState("SortOrder") = "ASC"
            Else
                ViewState("SortOrder") = "DESC"
            End If
            ViewState("SortExpression") = e.SortExpression + " " + ViewState("SortOrder")
            bindDataGrid()
        Catch ex As Exception
            lstatus.Text = "UserDG_SortCommandError =" + vbCrLf + ex.ToString
        End Try
    End Sub
    Protected Sub DataGridDt_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles DataGridDt.Sorting
        Try
            If ViewState("SortOrder") = Nothing Or ViewState("SortOrder") = "DESC" Then
                ViewState("SortOrder") = "ASC"
            Else
                ViewState("SortOrder") = "DESC"
            End If
            ViewState("SortExpressionDt") = e.SortExpression + " " + ViewState("SortOrder")
            bindDataGridDt()
        Catch ex As Exception
            lstatus.Text = "UserDG_SortCommandError =" + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccBankCharge_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dr As DataRow
        Dim ds As DataSet
        Dim Acc, tb As TextBox
        Dim AccName, lbcurr As Label
        Dim Count As Integer
        Dim dgi As GridViewRow
        Dim ddlCurr As DropDownList
        Try
            tb = sender
            If tb.ID = "AccBankChargeAdd" Then
                Count = DataGridDt.Controls(0).Controls.Count
                dgi = DataGridDt.Controls(0).Controls(Count - 2) '-1 for allowpaging = False
                Acc = dgi.FindControl("AccBankChargeAdd")
                AccName = dgi.FindControl("AccBankChargeNameAdd")
                ddlCurr = dgi.FindControl("CurrCodeDtAdd")
                ds = SQLExecuteQuery("Select Account, Description FROM VMsAccount Where ( Currency =" + QuotedStr(ViewState("Currency")) + " OR Currency = " + QuotedStr(ddlCurr.SelectedValue) + " ) AND FgActive = 'Y' AND FgType = 'PL' and FgNormal = 'D' and FgSubled IN ('N','F') AND Account = " + QuotedStr(Acc.Text), ViewState("DBConnection").ToString)
            Else
                Count = DataGridDt.EditIndex
                dgi = DataGridDt.Rows(Count)
                Acc = dgi.FindControl("AccBankChargeEdit")
                AccName = dgi.FindControl("AccBankChargeNameEdit")
                lbcurr = dgi.FindControl("CurrCodeDtEdit")
                ds = SQLExecuteQuery("Select Account, Description FROM VMsAccount Where ( Currency =" + QuotedStr(ViewState("Currency")) + " OR Currency = " + QuotedStr(lbcurr.Text) + " ) AND FgActive = 'Y' AND FgType = 'PL' and FgNormal = 'D' and FgSubled IN ('N','F') AND Account = " + QuotedStr(Acc.Text), ViewState("DBConnection").ToString)
            End If

            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                Acc.Text = ""
                AccName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                Acc.Text = dr("Account").ToString
                AccName.Text = dr("Description").ToString
            End If
        Catch ex As Exception
            lstatus.Text = "tb Acc Bank Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnBackDtTop_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBackDtTop.Click
        Try
            pnlHd.Visible = True
            pnlDt.Visible = False
        Catch ex As Exception
            lstatus.Text = "btn Back Dt Top Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        Try
            pnlHd.Visible = True
            pnlDt.Visible = False
        Catch ex As Exception
            lstatus.Text = "btn Back Dt Bottom Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub CurrCodeDtAdd_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Count As Integer
        Dim dgi As GridViewRow
        Dim tbAccBankCharge As TextBox
        Dim lbAccName As Label
        Try
            Count = DataGridDt.Controls(0).Controls.Count
            dgi = DataGridDt.FooterRow

            tbAccBankCharge = dgi.FindControl("AccBankChargeAdd")
            lbAccName = dgi.FindControl("AccBankChargeNameAdd")

            tbAccBankCharge.Text = ""
            lbAccName.Text = ""

        Catch ex As Exception
            lstatus.Text = "Curr Add Index Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPrint.Click
        Dim StrFilter, SQLString As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            Session("PrintType") = "Print"
            SQLString = "S_FormPrintMaster6 'VMsPayType', 'Payment_Code', 'Payment_Name', 'AccountName', 'Currency', 'VoucherNo+''/ ''+VoucherNoOut', 'FgModeName', 'Payment Type File', 'Payment Code', 'Payment Name', 'Account', 'Currency', 'Voucher No In / Out', 'Mode', " + QuotedStr(StrFilter) + ", " + QuotedStr(ViewState("UserId"))
            SQLString = Replace(SQLString, "PayCode", "Payment_Code")
            SQLString = Replace(SQLString, "PayName", "Payment_Name")
            SQLString = Replace(SQLString, "CurrCode", "Currency")
            Session("SelectCommand") = SQLString
            Session("ReportFile") = ".../../../Rpt/RptPrintMaster6.frx"
            AttachScript("openprintdlg();", Page, Me.GetType)

        Catch ex As Exception
            lstatus.Text = "btn Print Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAcc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAcc.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "Select * FROM VMsAccount Where FgActive = 'Y' "
            ResultField = "Account, Description, Currency"
            ViewState("Sender") = "btnAcc"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchGrid();", Page, Me.GetType())
        Catch ex As Exception
            lstatus.Text = "btn Account Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbAccount_TextChanged1(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAccount.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Try
            ds = SQLExecuteQuery("SELECT Account, Description, Currency FROM VMsAccount WHERE FgActive = 'Y' AND Account = " + QuotedStr(tbAccount.Text), ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbAccount.Text = ""
                tbAccName.Text = ""
                ddlCurrCode.SelectedIndex = 0
            Else
                dr = ds.Tables(0).Rows(0)
                tbAccount.Text = dr("Account").ToString
                tbAccName.Text = dr("Description").ToString
                ddlCurrCode.SelectedValue = dr("Currency").ToString
            End If

        Catch ex As Exception
            lstatus.Text = "tbAccount_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click, btnAdd2.Click
        Try
            pnlHd.Visible = False
            pnlInput.Visible = True
            ViewState("State") = "Insert"
            tbPayCode.Enabled = True
            ModifyInput(True)
            ClearInput()
        Catch ex As Exception
            lstatus.Text = "btn Add Error : " + ex.ToString
        End Try
    End Sub
    Private Sub ClearInput()
        Try
            If tbPayCode.Enabled Then
                tbPayCode.Text = ""
            End If

            tbPayName.Text = ""
            tbAccount.Text = ""
            tbAccName.Text = ""
            ddlCurrCode.SelectedIndex = 0
            ddlFgmode.SelectedIndex = 1
            ddlType.SelectedIndex = 0
            ddlBank.SelectedIndex = 0
            tbVoucherNo.Text = ""
            tbVoucherNoOut.Text = ""
            tbNorek.Text = ""
            tbNamaRekening.Text = ""
            tbSwift.Text = ""
            tbBankBranch.Text = ""
            tbBankAddr.Text = ""
            tbBankPhone.Text = ""
            tbBankFax.Text = ""
            tbCP.Text = ""
            tbCPAddress.Text = ""
            tbCPPhone.Text = ""

            If ddlFgmode.SelectedIndex <> 1 Then
                ddlBank.Enabled = False
                ddlBank.SelectedIndex = 0
                tbNorek.Enabled = False
                tbNamaRekening.Enabled = False
                tbSwift.Enabled = False
                tbBankBranch.Enabled = False
                tbBankAddr.Enabled = False
                tbBankPhone.Enabled = False
                tbBankFax.Enabled = False
                tbCP.Enabled = False
                tbCPAddress.Enabled = False
                tbCPPhone.Enabled = False
            Else
                ddlBank.SelectedIndex = 0
                ddlBank.Enabled = True
                tbNorek.Enabled = True
                tbNamaRekening.Enabled = True
                tbSwift.Enabled = True
                tbBankBranch.Enabled = True
                tbBankAddr.Enabled = True
                tbBankPhone.Enabled = True
                tbBankFax.Enabled = True
                tbCP.Enabled = True
                tbCPAddress.Enabled = True
                tbCPPhone.Enabled = True
            End If
        Catch ex As Exception
            Throw New Exception("Clear Input Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            pnlInput.Visible = False
            pnlHd.Visible = True
        Catch ex As Exception
            lstatus.Text = "Btn Cancel Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Try
            ClearInput()
            tbPayName.Focus()
        Catch ex As Exception
            lstatus.Text = "Btn Reset Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim SqlString, StrBank As String
        Try
            If cekInput() = False Then
                Exit Sub
            End If

            'kecuali mode bank maka akan tersimpan kosong
            If ddlFgmode.SelectedIndex <> 1 Then
                StrBank = ""
            Else
                StrBank = ddlBank.SelectedValue
            End If

            If ViewState("State") = "Insert" Then
                If SQLExecuteScalar("SELECT PayCode From MsPayType WHERE PayCode = " + QuotedStr(tbPayCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lstatus.Text = "Payment " + QuotedStr(tbPayCode.Text) + " has already been exist"
                    Exit Sub
                End If

                SqlString = "Insert into MsPayType (PayCode, PayName, Account, CurrCode, FgMode, FgType, Bank, NoRekening, NamaRekening, SwiftCode, BankBranch, BankAddr, " + _
                "BankPhone, BankFax, ContactPerson, ContactAddr, ContactPhone, CostCtr, UserID, UserDate, VoucherNo, VoucherNoOut) " + _
                "SELECT " + QuotedStr(tbPayCode.Text) + "," + QuotedStr(tbPayName.Text) + "," + QuotedStr(tbAccount.Text) + ", " + QuotedStr(ddlCurrCode.SelectedValue) + "," + _
                QuotedStr(ddlFgmode.SelectedValue) + "," + QuotedStr(ddlType.SelectedValue) + ", '" + StrBank + "', " + _
                QuotedStr(tbNorek.Text) + ", " + QuotedStr(tbNamaRekening.Text) + "," + QuotedStr(tbSwift.Text) + ", " + QuotedStr(tbBankBranch.Text) + "," + _
                QuotedStr(tbBankAddr.Text) + "," + QuotedStr(tbBankPhone.Text) + "," + QuotedStr(tbBankFax.Text) + ", " + QuotedStr(tbCP.Text) + "," + _
                QuotedStr(tbCPAddress.Text) + "," + QuotedStr(tbCPPhone.Text) + "," + QuotedStr(ddlCostCtr.SelectedValue) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", GetDate(), " + QuotedStr(tbVoucherNo.Text) + ", " + QuotedStr(tbVoucherNoOut.Text)
            Else
                SqlString = "Update MsPayType set PayName =" + QuotedStr(tbPayName.Text) + ", " + _
                            "Account =" + QuotedStr(tbAccount.Text) + ", CurrCode =" + QuotedStr(ddlCurrCode.SelectedValue) + ", " + _
                            "FgMode =" + QuotedStr(ddlFgmode.SelectedValue) + ", FgType =" + QuotedStr(ddlType.SelectedValue) + ", " + _
                            "Bank = '" + StrBank + "', CostCtr = " + QuotedStr(ddlCostCtr.SelectedValue) + _
                            ", VoucherNo =" + QuotedStr(tbVoucherNo.Text) + ", VoucherNoOut =" + QuotedStr(tbVoucherNoOut.Text) + _
                            ", NoRekening =" + QuotedStr(tbNorek.Text) + _
                            ", NamaRekening =" + QuotedStr(tbNamaRekening.Text) + ", SwiftCode =" + QuotedStr(tbSwift.Text) + ", BankBranch =" + QuotedStr(tbBankBranch.Text) + ", " + _
                            "BankAddr =" + QuotedStr(tbBankAddr.Text) + ", BankPhone =" + QuotedStr(tbBankPhone.Text) + ", BankFax =" + QuotedStr(tbBankFax.Text) + ", " + _
                            "ContactPerson =" + QuotedStr(tbCP.Text) + ", ContactAddr =" + QuotedStr(tbCPAddress.Text) + ", ContactPhone =" + QuotedStr(tbCPPhone.Text) + _
                            " where PayCode = " & QuotedStr(tbPayCode.Text)
            End If
            SQLExecuteNonQuery(SqlString, ViewState("DBConnection").ToString)
            bindDataGrid()
            pnlInput.Visible = False
            pnlHd.Visible = True
        Catch ex As Exception
            lstatus.Text = "Btn Save Error : " + ex.ToString
        End Try
    End Sub

    Private Function cekInput() As Boolean
        Try
            If tbPayCode.Text.Trim.Length = 0 Then
                lstatus.Text = "<script language='javascript'>alert('Payment Type Code must be filled');</script>"
                tbPayCode.Focus()
                Exit Function
            End If
            If tbPayName.Text.Trim.Length = 0 Then
                lstatus.Text = "<script language='javascript'>alert('Payment Type Name must be filled');</script>"
                tbPayName.Focus()
                Exit Function
            End If

            If tbAccount.Text.Trim.Length = 0 Then
                lstatus.Text = "<script language='javascript'>alert('Account must be filled');</script>"
                tbAccount.Focus()
                Exit Function
            End If
            If ddlFgmode.SelectedIndex = 1 Then
                If ddlBank.SelectedValue.Trim.Length = 0 Then
                    lstatus.Text = "<script language='javascript'>alert('Bank must be filled');</script>"
                    ddlBank.Focus()
                    Exit Function
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Input Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub ddlFgmode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFgmode.SelectedIndexChanged
        Try
            If ddlFgmode.SelectedIndex <> 1 Then
                ddlBank.Enabled = False
                ddlBank.SelectedIndex = 0
                tbNorek.Enabled = False
                tbNamaRekening.Enabled = False
                tbSwift.Enabled = False
                tbBankBranch.Enabled = False
                tbBankAddr.Enabled = False
                tbBankPhone.Enabled = False
                tbBankFax.Enabled = False
                tbCP.Enabled = False
                tbCPAddress.Enabled = False
                tbCPPhone.Enabled = False
                tbNorek.Text = ""
                tbNamaRekening.Text = ""
                tbSwift.Text = ""
                tbBankBranch.Text = ""
                tbBankAddr.Text = ""
                tbBankPhone.Text = ""
                tbBankFax.Text = ""
                tbCP.Text = ""
                tbCPAddress.Text = ""
                tbCPPhone.Text = ""
            Else
                ddlBank.SelectedIndex = 0
                ddlBank.Enabled = True
                tbNorek.Enabled = True
                tbNamaRekening.Enabled = True
                tbSwift.Enabled = True
                tbBankBranch.Enabled = True
                tbBankAddr.Enabled = True
                tbBankPhone.Enabled = True
                tbBankFax.Enabled = True
                tbCP.Enabled = True
                tbCPAddress.Enabled = True
                tbCPPhone.Enabled = True
            End If

        Catch ex As Exception
            lstatus.Text = "Mode Add Index Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub FillTextBox(ByVal PayCode As String)
        Dim SqlString As String
        Dim DT As DataTable
        Try
            SqlString = "SELECT A.*, B.AccountName, C.BankName AS BankName, " + _
             "CASE A.FgMode WHEN 'B' THEN 'Bank' " + _
             "WHEN 'G' THEN 'Giro' WHEN 'O' THEN 'Other' WHEN 'C' THEN 'Cash' WHEN 'D' THEN 'DP' WHEN 'B' THEN 'BANK' WHEN 'C' THEN 'CN' WHEN 'N' THEN 'DN' WHEN 'I' THEN 'Income' WHEN 'E' THEN 'Expense' WHEN 'K' THEN 'Kas' END FgModeName, " + _
             "CASE A.FgType WHEN 'P' THEN 'Payment' " + _
             "WHEN 'R' THEN 'Receipt' WHEN 'A' THEN 'All' END FgTypeName " + _
             "FROM MsPayType A LEFT OUTER JOIN " + _
             "MsAccount B ON A.Account = B.Account LEFT OUTER JOIN " + _
             "MsBank C ON A.Bank = C.BankCode "

            DT = BindDataTransaction(SqlString, " A.PayCode = " + QuotedStr(PayCode), ViewState("DBConnection").ToString)

            BindToText(tbPayCode, DT.Rows(0)("PayCode").ToString)
            BindToText(tbPayName, DT.Rows(0)("PayName").ToString)
            BindToText(tbAccount, DT.Rows(0)("Account").ToString)
            BindToText(tbAccName, DT.Rows(0)("AccountName").ToString)
            BindToDropList(ddlCurrCode, DT.Rows(0)("CurrCode").ToString)
            BindToDropList(ddlFgmode, DT.Rows(0)("FgMode").ToString)
            BindToDropList(ddlType, DT.Rows(0)("FgType").ToString)
            BindToDropList(ddlBank, DT.Rows(0)("Bank").ToString)
            BindToText(tbVoucherNo, DT.Rows(0)("VoucherNo").ToString)
            BindToText(tbVoucherNoOut, DT.Rows(0)("VoucherNoOut").ToString)
            BindToText(tbNorek, DT.Rows(0)("NoRekening").ToString)
            BindToText(tbNamaRekening, DT.Rows(0)("NamaRekening").ToString)
            BindToText(tbSwift, DT.Rows(0)("SwiftCode").ToString)
            BindToText(tbBankBranch, DT.Rows(0)("BankBranch").ToString)
            BindToText(tbBankAddr, DT.Rows(0)("BankAddr").ToString)
            BindToText(tbBankPhone, DT.Rows(0)("BankPhone").ToString)
            BindToText(tbBankFax, DT.Rows(0)("BankFax").ToString)
            BindToText(tbCP, DT.Rows(0)("ContactPerson").ToString)
            BindToText(tbCPAddress, DT.Rows(0)("ContactAddr").ToString)
            BindToText(tbCPPhone, DT.Rows(0)("ContactPhone").ToString)
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "FillTextBox error: " & ex.ToString
        End Try
    End Sub

    Private Sub ModifyInput(ByVal State As Boolean)
        tbPayCode.Enabled = State
        tbPayName.Enabled = State
        tbAccount.Enabled = State
        ddlCurrCode.Enabled = State
        ddlFgmode.Enabled = State
        ddlType.Enabled = State
        ddlBank.Enabled = State
        tbVoucherNo.Enabled = State
        tbVoucherNoOut.Enabled = State
        tbNorek.Enabled = State
        tbNamaRekening.Enabled = State
        tbSwift.Enabled = State
        tbBankBranch.Enabled = State
        tbBankAddr.Enabled = State
        tbBankPhone.Enabled = State
        tbBankFax.Enabled = State
        tbCP.Enabled = State
        tbCPAddress.Enabled = State
        tbCPPhone.Enabled = State
        btnAcc.Visible = State
        BtnSave.Visible = State
        btnReset.Visible = State
        ddlCostCtr.Enabled = State

        If State = True Then
            If ddlFgmode.SelectedIndex <> 1 Then
                ddlBank.Enabled = False
                tbNorek.Enabled = False
                tbNamaRekening.Enabled = False
                tbSwift.Enabled = False
                tbBankBranch.Enabled = False
                tbBankAddr.Enabled = False
                tbBankPhone.Enabled = False
                tbBankFax.Enabled = False
                tbCP.Enabled = False
                tbCPAddress.Enabled = False
                tbCPPhone.Enabled = False
                tbNorek.Text = ""
                tbNamaRekening.Text = ""
                tbSwift.Text = ""
                tbBankBranch.Text = ""
                tbBankAddr.Text = ""
                tbBankPhone.Text = ""
                tbBankFax.Text = ""
                tbCP.Text = ""
                tbCPAddress.Text = ""
                tbCPPhone.Text = ""
            Else
                ddlBank.Enabled = True
                tbNorek.Enabled = True
                tbNamaRekening.Enabled = True
                tbSwift.Enabled = True
                tbBankBranch.Enabled = True
                tbBankAddr.Enabled = True
                tbBankPhone.Enabled = True
                tbBankFax.Enabled = True
                tbCP.Enabled = True
                tbCPAddress.Enabled = True
                tbCPPhone.Enabled = True
            End If
        End If
    End Sub
End Class
