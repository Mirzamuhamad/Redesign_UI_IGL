Imports System.Data
Imports System.Data.SqlClient
'Imports System.Data.Odbc
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc


Partial Class APPending
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select * From V_FNBeginRRHd "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                FillCombo(ddlCurr, "EXEC S_GetCurrency", False, "Currency", "Currency", ViewState("DBConnection"))
                FillCombo(ddlUnit, "EXEC S_GetUnit", False, "Unit_Code", "Unit_Name", ViewState("DBConnection"))
                ' FillCombo(ddlCostCenter, "SELECT CostCtrCode, CostCtrName FROM MsCostCtr ", True, "CostCtrCode", "CostCtrName", ViewState("DBConnection"))
                'FillCombo(ddlPType, "EXEC S_GetProductType", False, "Type_Code", "Type_Name", ViewState("DBConnection"))
                SetInit()
                Session("AdvanceFilter") = ""
                Dim dt As DataTable
                dt = SQLExecuteQuery("Select RptTitle, MenuName, COALESCE(MenuParam,'') AS MenuParam, COALESCE(FastRpt,'') AS FastRpt, COALESCE(ReportType,'') AS ReportType, COALESCE(ReportType2,'') AS ReportType2, COALESCE(FgPrintValue,'N') AS FgPrintValue, COALESCE(FgForceNewPage,'N') AS FgForceNewPage from MsMenu WHERE MenuId = " + QuotedStr(Request.QueryString("ContainerId").ToString), ViewState("DBConnection").ToString).Tables(0)
                lblTitle.Text = dt.Rows(0)("MenuName").ToString

            End If
            lbStatus.Text = ""
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If

            ' hasil dari search dialog
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnSupp" Then
                    tbSuppCode.Text = Session("Result")(0).ToString
                    tbSuppName.Text = Session("Result")(1).ToString
                    BindToDropList(ddlCurr, Session("Result")(2).ToString)
                    BindToText(tbAttn, Session("Result")(3).ToString)
                    ddlCurr_SelectedIndexChanged(Nothing, Nothing)
                    ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
                End If
                If ViewState("Sender") = "btnWrhs" Then
                    tbWrhsCode.Text = Session("Result")(0).ToString
                    tbWrhsName.Text = Session("Result")(1).ToString
                End If
                If ViewState("Sender") = "btnProduct" Then
                    tbProductCode.Text = Session("Result")(0).ToString
                    tbProductName.Text = Session("Result")(1).ToString
                    BindToText(tbUnitWrhs, Session("Result")(2).ToString)
                    BindToDropList(ddlUnit, Session("Result")(2).ToString)
                    'BindToDropList(ddlCostCenter, Session("Result")(3).ToString)
                    tbQty.Text = FormatFloat(tbQty.Text, ViewState("DigitQty"))
                    tbQtyWrhs.Text = FormatFloat(tbQtyWrhs.Text, ViewState("DigitQty"))
                End If
                'If ViewState("Sender") = "btnGetDt" Then
                '    Dim drResult As DataRow                    
                '    If IsNothing(Session("Result")) Then
                '        lbStatus.Text = "Session is empty"
                '        Exit Sub
                '    End If
                '    For Each drResult In Session("Result").Rows

                '        'insert
                '        Dim dr As DataRow
                '        dr = ViewState("Dt").NewRow
                '        dr("Product") = drResult("Product")
                '        dr("ProductName") = drResult("Product_Name")
                '        dr("UnitOrder") = ""
                '        dr("Unit") = ""
                '        dr("PriceForex") = 0
                '        dr("AmountForex") = 0
                '        dr("QtyOrder") = 0
                '        dr("Qty") = 0
                '        ViewState("Dt").Rows.Add(dr)
                '    Next
                '    BindGridDt(ViewState("Dt"), GridDt)
                '    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                '    'Session("ResultSame") = Nothing
                'End If
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
    End Sub

    Private Sub SetInit()
        ViewState("SortExpression") = Nothing
        ViewState("DigitCurr") = 0
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
        If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then

        End If
        GridView1.PageSize = CInt(ViewState("PageSizeGrid"))
        'GridDt.PageSize = CInt(ViewState("PageSizeGrid"))
        Me.tbAmountForex.Attributes.Add("ReadOnly", "True")
        Me.tbQtyWrhs.Attributes.Add("ReadOnly", "True")
        Me.tbBaseForex.Attributes.Add("ReadOnly", "True")
        Me.tbPPNForex.Attributes.Add("ReadOnly", "True")
        Me.tbTotalForex.Attributes.Add("ReadOnly", "True")

        Me.tbRate.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbQty.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbPrice.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbBaseForex.Attributes.Add("OnBlur", "BasePPnTotal(" + Me.tbBaseForex.ClientID + "," + Me.tbPPN.ClientID + "," + Me.tbPPNForex.ClientID + "," + Me.tbTotalForex.ClientID + "); setformat();")
        Me.tbPPN.Attributes.Add("OnBlur", "BasePPnTotal(" + Me.tbBaseForex.ClientID + "," + Me.tbPPN.ClientID + "," + Me.tbPPNForex.ClientID + "," + Me.tbTotalForex.ClientID + "); setformat();")
        Me.tbQty.Attributes.Add("OnBlur", "QtyxPrice(" + Me.tbQty.ClientID + "," + Me.tbPrice.ClientID + "," + Me.tbAmountForex.ClientID + "); setformatdt();")
        Me.tbPrice.Attributes.Add("OnBlur", "QtyxPrice(" + Me.tbQty.ClientID + "," + Me.tbPrice.ClientID + "," + Me.tbAmountForex.ClientID + "); setformatdt();")
    End Sub

    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim StrFilter As String
        Try
            'If AdvanceFilter.Length > 1 Then
            '    StrFilter = AdvanceFilter
            'Else
            '    StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            'End If
            StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
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
        Return "SELECT * From V_FNBeginRRDt WHERE TransNmbr = " + QuotedStr(Nmbr)
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
            Status = CekStatus(ActionValue)

            ListSelectNmbr = ""
            GetListCommand(Status, GridView1, "3,2", ListSelectNmbr, Nmbr, lbStatus.Text)
            If ListSelectNmbr = "" Then Exit Sub
            For j = 0 To (Nmbr.Length - 1)
                If Nmbr(j) = "" Then
                    Exit For
                Else
                    Result = ExecSPCommandGo(ActionValue, "S_FNBeginRR", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            'btnGetDt.Visible = State
            ddlReport.Enabled = State
            tbRef.Enabled = State
            tbSuppCode.Enabled = State
            btnSupp.Visible = State
            ddlCurr.Enabled = State
            'ddlPType.Enabled = State
            tbWrhsCode.Enabled = State
            btnWrhs.Visible = State
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
                If ViewState("DtValue") <> tbProductCode.Text Then
                    If CekExistData(ViewState("Dt"), "Product", tbProductCode.Text) Then
                        lbStatus.Text = MessageDlg("Product " + tbProductName.Text + " has been already exist")
                        Exit Sub
                    End If
                End If
                Row = ViewState("Dt").Select("Product = " + QuotedStr(ViewState("DtValue")))(0)
                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                Row("Product") = tbProductCode.Text
                Row("Product_Name") = tbProductName.Text
                Row("QtyOrder") = tbQty.Text
                Row("UnitOrder") = ddlUnit.SelectedValue
                If Row("UnitOrder") = "" Then
                    Row("UnitOrder") = DBNull.Value
                End If
                Row("Qty") = tbQtyWrhs.Text
                Row("Unit") = tbUnitWrhs.Text
                Row("Remark") = tbRemarkDt.Text
                Row("PriceForex") = tbPrice.Text
                Row("AmountForex") = tbAmountForex.Text
                'Row("CostCtr") = ddlCostCenter.SelectedValue
                'Row("CostCtrName") = ddlCostCenter.SelectedItem
                Row.EndEdit()
                'ViewState("Dt").AcceptChanges()
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If
                If CekExistData(ViewState("Dt"), "Product", tbProductCode.Text) = True Then
                    lbStatus.Text = MessageDlg("Product " + tbProductName.Text + " has already been exist")
                    Exit Sub
                End If
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("Product") = tbProductCode.Text
                dr("Product_Name") = tbProductName.Text
                dr("QtyOrder") = tbQty.Text
                dr("UnitOrder") = ddlUnit.SelectedValue
                If dr("UnitOrder") = "" Then
                    dr("UnitOrder") = DBNull.Value
                End If
                dr("Qty") = tbQtyWrhs.Text
                dr("Unit") = tbUnitWrhs.Text
                dr("Remark") = tbRemarkDt.Text
                dr("PriceForex") = tbPrice.Text
                dr("AmountForex") = tbAmountForex.Text
                'dr("CostCtr") = ddlCostCenter.SelectedValue
                'dr("CostCtrName") = ddlCostCenter.SelectedItem
                ViewState("Dt").Rows.Add(dr)
                'Dt = ViewState("Dt")
                'a = Dt.Compute("Sum(AmountForex)", "")                
                'tbBaseForex.Text = a.ToString
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
                lbStatus.Text = MessageDlg("Detail Data must be saved first")
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
                tbRef.Text = GetAutoNmbr("RRP", ddlReport.SelectedValue, Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
                SQLString = "INSERT INTO FINBeginRRHd (TransNmbr, FgReport, Status, TransDate, Supplier, Attn, Warehouse, " + _
                "PONo, SJSuppNo, SJSuppDate, CarNo, Currency, ForexRate, BaseForex, PPn, PPnForex, TotalForex, " + _
                "Remark, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbRef.Text) + ", " + QuotedStr(ddlReport.SelectedValue) + ", 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', " + _
                QuotedStr(tbSuppCode.Text) + ", " + QuotedStr(tbAttn.Text) + ", " + QuotedStr(tbWrhsCode.Text) + ", " + QuotedStr(tbPONo.Text) + ", " + _
                QuotedStr(tbSJSuppNo.Text) + ", '" + Format(tbSJSuppDate.SelectedValue, "yyyy-MM-dd") + "', " + _
                QuotedStr(tbCarNo.Text) + ", " + QuotedStr(ddlCurr.SelectedValue) + ", " + tbRate.Text.Replace(",", "") + ", " + _
                tbBaseForex.Text.Replace(",", "") + ", " + tbPPN.Text.Replace(",", "") + ", " + _
                tbPPNForex.Text.Replace(",", "") + ", " + tbTotalForex.Text.Replace(",", "") + ", " + _
                QuotedStr(tbRemark.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", getDate()"
            Else
                Dim CekStatus As String
                CekStatus = SQLExecuteScalar("Select Status FROM FINBeginRRHd WHERE TransNmbr = " + QuotedStr(tbRef.Text), ViewState("DBConnection").ToString)
                If CekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE FINBeginRRHd SET Supplier = " + QuotedStr(tbSuppCode.Text) + ", Attn = " + QuotedStr(tbAttn.Text) + _
                ", Warehouse = " + QuotedStr(tbWrhsCode.Text) + ", PONo = " + QuotedStr(tbPONo.Text) + ", SJSuppNo = " + QuotedStr(tbSJSuppNo.Text) + _
                ", SJSuppDate = '" + Format(tbSJSuppDate.SelectedValue, "yyyy-MM-dd") + "', CarNo = " + QuotedStr(tbCarNo.Text) + _
                ", Currency = " + QuotedStr(ddlCurr.SelectedValue) + ", ForexRate = " + tbRate.Text.Replace(",", "") + _
                ", BaseForex = " + tbBaseForex.Text.Replace(",", "") + ", PPn = " + tbPPN.Text.Replace(",", "") + _
                ", PPnForex = " + tbPPNForex.Text.Replace(",", "") + ", TotalForex = " + tbTotalForex.Text.Replace(",", "") + _
                ", Remark = " + QuotedStr(tbRemark.Text) + _
                ", TransDate = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', DateAppr = getDate()" + _
                " WHERE TransNmbr = " + QuotedStr(tbRef.Text) + ""
            End If
            'SQLString = ChangeQuoteNull(SQLString)
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
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, Product, QtyOrder, UnitOrder, Qty, Unit, PriceForex, AmountForex, Remark FROM FINBeginRRDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand

            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
                    "UPDATE FINBeginRRDt SET Product = @Product, QtyOrder = @QtyOrder, UnitOrder = @UnitOrder, Qty = @Qty, Unit = @Unit, PriceForex = @PriceForex, AmountForex = @AmountForex, Remark = @Remark WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND Product = @OldProduct", con)
            ' Define output parameters.
            Update_Command.Parameters.Add("@Product", SqlDbType.VarChar, 20, "Product")
            Update_Command.Parameters.Add("@QtyOrder", SqlDbType.Decimal, 18, "QtyOrder")
            Update_Command.Parameters.Add("@UnitOrder", SqlDbType.VarChar, 5, "UnitOrder")
            Update_Command.Parameters.Add("@Qty", SqlDbType.Decimal, 18, "Qty")
            Update_Command.Parameters.Add("@Unit", SqlDbType.VarChar, 5, "Unit")
            Update_Command.Parameters.Add("@PriceForex", SqlDbType.Decimal, 18, "PriceForex")
            Update_Command.Parameters.Add("@AmountForex", SqlDbType.Decimal, 18, "AmountForex")
            Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 255, "Remark")
            'Update_Command.Parameters.Add("@CostCtr", SqlDbType.VarChar, 10, "CostCtr")
            ' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@OldProduct", SqlDbType.VarChar, 20, "Product")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM FINBeginRRDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND Product = @Product", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@Product", SqlDbType.VarChar, 20, "Product")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("FINBeginRRDt")

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
            tbDate.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ViewState("StateHd") = "Insert"
            tbDate.SelectedDate = ViewState("ServerDate") 'Today
            tbSJSuppDate.SelectedDate = ViewState("ServerDate")
            ClearHd()
            Cleardt()
            'ddlCurr.SelectedValue = ViewState("Currency").ToString
            'ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, ViewState("DBConnection").ToString)
            pnlDt.Visible = True
            BindDataDt("")
            EnableHd(True)
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbRef.Text = ""
            tbSuppCode.Text = ""
            tbSuppName.Text = ""
            tbAttn.Text = ""
            tbWrhsCode.Text = ""
            tbWrhsName.Text = ""
            tbPONo.Text = ""
            tbSJSuppNo.Text = ""
            tbCarNo.Text = ""
            ddlCurr.SelectedValue = ViewState("Currency")
            tbRate.Text = "0"
            tbBaseForex.Text = "0"
            tbPPN.Text = "10"
            tbPPNForex.Text = "0"
            tbTotalForex.Text = "0"
            tbRemark.Text = ""
            ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbProductCode.Text = ""
            tbProductName.Text = ""
            tbUnitWrhs.Text = ""
            tbRemarkDt.Text = ""
            tbQty.Text = "0"
            tbQtyWrhs.Text = "0"
            tbPrice.Text = "0"
            tbAmountForex.Text = "0"
            ' ddlCostCenter.SelectedIndex = 0
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

    Protected Sub btnSupp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSupp.Click
        Dim ResultField As String
        Try
            Session("filter") = "select * from VMsSupplier"
            ResultField = "Supplier_Code, Supplier_Name, Currency, Contact_Person"
            ViewState("Sender") = "btnSupp"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Supp Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnWrhs_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnWrhs.Click
        Dim ResultField As String
        Try
            Session("filter") = "select * from VMsWarehouse WHERE Wrhs_Type = 'Owner' "
            ResultField = "Wrhs_Code, Wrhs_Name"
            ViewState("Sender") = "btnWrhs"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Wrhs Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnProduct_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProduct.Click
        Dim ResultField As String
        Try
            Session("filter") = "select * from VMsProduct "
            ResultField = "Product_Code, Product_Name, Unit, CostCtr, CostCtrName"
            ViewState("Sender") = "btnProduct"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Product Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbSuppCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbSuppCode.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Supplier", tbSuppCode.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbSuppCode.Text = Dr("Supplier_Code")
                tbSuppName.Text = Dr("Supplier_Name")
                BindToDropList(ddlCurr, Dr("Currency"))
                BindToText(tbAttn, Dr("Contact_Person"))
            Else
                tbSuppCode.Text = ""
                tbSuppName.Text = ""
                ddlCurr.SelectedValue = ViewState("Currency")
                tbAttn.Text = ""
            End If
            ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
            AttachScript("setformat();", Page, Me.GetType())
            tbSuppCode.Focus()
        Catch ex As Exception
            Throw New Exception("tb SuppCode Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub tbWrhsCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbWrhsCode.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Warehouse", tbWrhsCode.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                If Dr("Wrhs_Type") = "Owner" Then
                    tbWrhsCode.Text = Dr("Wrhs_Code")
                    tbWrhsName.Text = Dr("Wrhs_Name")
                Else
                    tbWrhsCode.Text = ""
                    tbWrhsName.Text = ""
                End If
            Else
                tbWrhsCode.Text = ""
                tbWrhsName.Text = ""
            End If
            tbWrhsCode.Focus()
        Catch ex As Exception
            Throw New Exception("tb WrhsCode Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub tbProductCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbProductCode.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("ProductForAPPending", tbProductCode.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbProductCode.Text = Dr("Product_Code")
                tbProductName.Text = Dr("Product_Name")
                tbUnitWrhs.Text = Dr("Unit")
                BindToDropList(ddlUnit, Dr("Unit"))
                ' BindToDropList(ddlCostCenter, TrimStr(Dr("CostCtr").ToString))
            Else
                tbProductCode.Text = ""
                tbProductName.Text = ""
                tbUnitWrhs.Text = ""
                ' ddlCostCenter.SelectedIndex = 0
            End If

            tbQtyWrhs.Text = FindConvertUnit(tbProductCode.Text, ddlUnit.SelectedValue, tbQty.Text, ViewState("DBConnection").ToString).ToString
            AttachScript("setformatdt();", Page, Me.GetType())
            tbProductCode.Focus()
        Catch ex As Exception
            Throw New Exception("tb ProductCode Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlCurr_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCurr.SelectedIndexChanged
        If ViewState("InputCurrency") = "Y" Then
            RefreshMaster("S_GetCurrency", "Currency", "Currency", ddlCurr, ViewState("DBConnection"))
            ViewState("InputCurrency") = Nothing
        End If
        ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"))
        AttachScript("setformat();", Page, Me.GetType())
        tbRate.Focus()
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
        tbProductCode.Focus()
    End Sub

    Function CekHd() As Boolean
        Try
            'If tbRef.Text.Trim = "" Then
            '    lbStatus.Text = "RR No must have value"
            '    tbRef.Focus()
            '    Return False
            'End If
            If tbDate.IsNull Then
                lbStatus.Text = MessageDlg("RR Date must have value")
                tbDate.Focus()
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
            If tbSuppCode.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Supplier must have value")
                btnSupp.Focus()
                Return False
            End If
            If tbWrhsCode.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Warehouse must have value")
                tbWrhsCode.Focus()
                Return False
            End If
            'If tbSJSuppNo.Text.Trim = "" Then
            '    lbStatus.Text = MessageDlg("SJ Supplier No must have value")
            '    tbSJSuppNo.Focus()
            '    Return False
            'End If
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
                If Dr("Product").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Product Must Have Value")
                    Return False
                End If
                If Dr("UnitOrder").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Unit Order Must Have Value")
                    Return False
                End If
                If CFloat(Dr("QtyOrder").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Order Must Have Value")
                    Return False
                End If
                If CFloat(Dr("Qty").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Warehouse Must Have Value")
                    Return False
                End If
                'If Dr("CostCtr").ToString.Trim = "" Then
                '    lbStatus.Text = MessageDlg("Cost Center Must Have Value")
                '    Return False
                'End If
            Else
                If tbProductCode.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Product Must Have Value")
                    tbProductCode.Focus()
                    Return False
                End If
                If ddlUnit.SelectedValue.Trim = "" Then
                    lbStatus.Text = MessageDlg("Unit Order Must Have Value")
                    ddlUnit.Focus()
                    Return False
                End If
                If CFloat(tbQty.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Order Must Have Value")
                    tbQty.Focus()
                    Return False
                End If
                If CFloat(tbQtyWrhs.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Warehouse Must Have Value")
                    tbQtyWrhs.Focus()
                    Return False
                End If
                'If ddlCostCenter.SelectedValue.Trim = "" Then
                '    lbStatus.Text = MessageDlg("Cost Center Must Have Value")
                '    ddlCostCenter.Focus()
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
        'Dim cb, cbselek As CheckBox
        'Dim GRW As GridViewRow
        'Try
        '    cb = sender
        '    For Each GRW In GridView1.Rows
        '        cbselek = GRW.FindControl("cbSelect")
        '        cbselek.Checked = cb.Checked
        '    Next
        'Catch ex As Exception
        '    lbStatus.Text = "cb Hd Checked Changed Error : " + ex.ToString
        'End Try
    End Sub

    Protected Sub LbAdvSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LbAdvSearch.Click
        Dim FDateName, FDateValue, FilterName, FilterValue As String
        Try
            FDateName = "Date"
            FDateValue = "TransDate"
            FilterName = "Reference, Date, Status, Supplier, Warehosue, Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), Status, Supplier, Wrhs_Name, Remark"
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
                        BindDataDt(ViewState("TransNmbr"))
                        FillTextBoxHd(ViewState("TransNmbr"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        tbRate.Enabled = ddlCurr.SelectedValue = ViewState("Currency")
                        btnHome.Visible = False
                        ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, ViewState("DBConnection").ToString)
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
                    Dim ReportGw As New ReportDocument
                    'Dim Reportds As DataSet

                    'Reportds = SQLExecuteQuery("EXEC S_FIN (" + QuotedStr(GVR.Cells(2).Text) + ")", ViewState("DBConnection").ToString)

                    'ReportGw.Load(Server.MapPath("~\Rpt\FormJEntry.Rpt"))
                    'ReportGw.SetDataSource(Reportds.Tables(0))

                    Session("Report") = ReportGw
                    Response.Write("<script language='javascript'> { window.open(""../../Rpt/PrintMaster.Aspx"", ""List"",""scrollbars=yes, resizable=yes,width=600,height=500"");}</script>")

                    'CrystalReportViewer1.ReportSource = ReportGw
                    'PnlHd.Visible = False
                    'pnlPrint.Visible = True
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
                'btnGetDt.Enabled = False
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Dim dr() As DataRow
        Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
        dr = ViewState("Dt").Select("Product = " + QuotedStr(GVR.Cells(1).Text))
        dr(0).Delete()
        BindGridDt(ViewState("Dt"), GridDt)
        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            FillTextBoxDt(GVR.Cells(1).Text)
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            ViewState("DtValue") = tbProductCode.Text
            tbSuppCode.Focus()
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbCurr_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCurr.Click
        Try
            ViewState("InputCurrency") = "Y"
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "', 'MsCurrency')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb Currency Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbSupp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSupp.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsSupplier')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb Supplier Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub lbWarehouse_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbWarehouse.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsWarehouse')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb Warehouse Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub lbProduct_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbProduct.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsProduct')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb Product Error : " + ex.ToString
        End Try
    End Sub

    Dim BaseForex As Decimal = 0

    ' untuk tampilkan data total di grid
    Protected Sub GridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt.RowDataBound
        Try
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "Product")) Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                    '' add the UnitPrice and QuantityTotal to the running total variables
                    'BaseForex += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "AmountForex"))                    
                ElseIf e.Row.RowType = DataControlRowType.Footer Then
                    BaseForex = GetTotalSum(ViewState("Dt"), "AmountForex")
                    tbBaseForex.Text = CStr(BaseForex) ' FormatNumber(BaseForex, ViewState("DigitCurr"))
                    AttachScript("BasePPnTotal(" + Me.tbBaseForex.ClientID + "," + Me.tbPPN.ClientID + "," + Me.tbPPNForex.ClientID + "," + Me.tbTotalForex.ClientID + "); setformat();", Page, Me.GetType())
                End If
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
        End Try
    End Sub

    'Protected Sub BtnGetDt_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnGetDt.Click
    '    Dim ResultField As String 'ResultSame 
    '    Try
    '        Session("Result") = Nothing
    '        Session("Filter") = "select * from VMsAccount"
    '        ResultField = "Account, Description, Currency, FgSubled"
    '        Session("Column") = ResultField.Split(",")
    '        'ResultSame = "Currency"
    '        'Session("ResultSame") = ResultSame.Split(",")
    '        ViewState("Sender") = "btnGetDt"
    '        AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
    '        'If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
    '        '    Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenSearchMultiDlg();", True)
    '        'End If
    '    Catch ex As Exception
    '        lbStatus.Text = "btn get Dt Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            tbRef.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToDropList(ddlReport, Dt.Rows(0)("FgReport").ToString)
            BindToText(tbSuppCode, Dt.Rows(0)("Supplier_Code").ToString)
            BindToText(tbSuppName, Dt.Rows(0)("Supplier_Name").ToString)
            BindToText(tbAttn, Dt.Rows(0)("Attn").ToString)
            BindToText(tbWrhsCode, Dt.Rows(0)("Warehouse").ToString)
            BindToText(tbWrhsName, Dt.Rows(0)("Wrhs_Name").ToString)
            BindToText(tbPONo, Dt.Rows(0)("PONo").ToString)
            BindToText(tbSJSuppNo, Dt.Rows(0)("SJSuppNo").ToString)
            BindToDate(tbSJSuppDate, Dt.Rows(0)("SJSuppDate").ToString)
            BindToText(tbCarNo, Dt.Rows(0)("CarNo").ToString)
            BindToDropList(ddlCurr, Dt.Rows(0)("Currency").ToString)
            BindToText(tbRate, Dt.Rows(0)("ForexRate").ToString)
            BindToText(tbBaseForex, Dt.Rows(0)("BaseForex").ToString)
            BindToText(tbPPN, Dt.Rows(0)("PPn").ToString)
            BindToText(tbPPNForex, Dt.Rows(0)("PPnForex").ToString)
            BindToText(tbTotalForex, Dt.Rows(0)("TotalForex").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal Product As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("Product = " + QuotedStr(Product))
            If Dr.Length > 0 Then
                BindToText(tbProductCode, Dr(0)("Product").ToString)
                BindToText(tbProductName, Dr(0)("Product_Name").ToString)
                BindToText(tbQty, Dr(0)("QtyOrder").ToString)
                BindToDropList(ddlUnit, Dr(0)("UnitOrder").ToString)
                BindToText(tbQtyWrhs, Dr(0)("Qty").ToString)
                BindToText(tbUnitWrhs, Dr(0)("Unit").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
                BindToText(tbPrice, Dr(0)("PriceForex").ToString)
                BindToText(tbAmountForex, Dr(0)("AmountForex").ToString)
                ' BindToDropList(ddlCostCenter, Dr(0)("CostCtr").ToString)
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
    Protected Sub tbQty_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbQty.TextChanged, ddlUnit.TextChanged
        Try
            tbQtyWrhs.Text = FindConvertUnit(tbProductCode.Text, ddlUnit.SelectedValue, tbQty.Text, ViewState("DBConnection").ToString).ToString
            tbQty.Focus()
        Catch ex As Exception
            lbStatus.Text = "tb Qty textchanged error : " + ex.ToString
        End Try
    End Sub

End Class
