Imports System.Data
Imports System.Data.SqlClient
'Imports System.Data.Odbc
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc

Partial Class Transaction_TrSTRROther_TrSTRROther
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select * From V_STRROtherHd"
    'Dim ViewState("Finish") As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim CurrFilter, Value As String
        Try
            If Not IsPostBack Then
                InitProperty()

                FillCombo(ddlwrhs, "EXEC S_GetWrhsUser " + QuotedStr(ViewState("UserId")), True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection"))
                SetInit()
                Session("AdvanceFilter") = ""
                If Not Request.QueryString("transid") Is Nothing Then
                    If Request.QueryString("transid").ToString.Length > 1 Then
                        'lbStatus.Text = Request.QueryString("transid").ToString
                        'Exit Sub
                        ddlRange.SelectedValue = "0"
                        CurrFilter = tbFilter.Text
                        Value = ddlField.SelectedValue
                        tbFilter.Text = Request.QueryString("transid").ToString
                        ddlField.SelectedValue = "TransNmbr"
                        btnSearch_Click(Nothing, Nothing)
                        tbFilter.Text = CurrFilter
                        ddlField.SelectedValue = Value
                    End If
                End If
            End If
            lbStatus.Text = ""
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If

            ' hasil dari search dialog
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnSubLed" Then
                    tbSubLed.Text = Session("Result")(0).ToString
                    tbSubLedName.Text = Session("Result")(1).ToString
                End If
                If ViewState("Sender") = "btnSupplier" Then
                    tbRRFrom.Text = Session("Result")(0).ToString
                    tbRRFromName.Text = Session("Result")(1).ToString
                End If
                If ViewState("Sender") = "btnCustomer" Then
                    tbRRFrom.Text = Session("Result")(0).ToString
                    tbRRFromName.Text = Session("Result")(1).ToString
                End If
                If ViewState("Sender") = "btnProduct" Then
                    tbProductCode.Text = Session("Result")(0).ToString
                    tbProductName.Text = Session("Result")(1).ToString
                    BindToText(tbSpecification, Session("Result")(2).ToString)
                    BindToText(tbUnit, Session("Result")(3).ToString)
                    'BindToText(tbQty, Session("Result")(5).ToString)
                    'BindToText(tbRemarkDt, Session("Result")(7).ToString)
                End If
                '  tbQty.Text = FormatNumber(tbQty.Text, ViewState("DigitQty"))

                If ViewState("Sender") = "btnGetDt" Then
                    Dim drResult As DataRow
                    For Each drResult In Session("Result").Rows
                        'insert
                        Dim dr As DataRow
                        dr = ViewState("Dt").NewRow
                        dr("Product") = drResult("Product_Code")
                        dr("ProductName") = drResult("Product_Name")
                        dr("Specification") = TrimStr(drResult("Specification"))
                        dr("Qty") = 0
                        dr("Unit") = drResult("Unit")
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
        If (Request.QueryString("ContainerId").ToString = "STRRCustomerID") Then
            Labelmenu.Text = "RR CUSTOMER"
            ViewState("RRType") = "RRC"

            lbRRFrom.Text = "Customer"
            Label1.Visible = False
            lbRRFrom.Visible = True
            tbRRFrom.Width = 103
            tbRRFromName.Visible = True
            btnRRFrom.Visible = tbRRFromName.Visible

            'ddlField.Items.Add(New ListItem("Customer ", "RRForm"))
            ddlField.Items.Add(New ListItem("Customer Name", "RRFromName"))
            ddlField.Items.Add(New ListItem("Received By", "ReceivedBy"))
            ddlField.Items.Add("Remark")

            'ddlField2.Items.Add(New ListItem("Customer ", "RRForm"))
            ddlField2.Items.Add(New ListItem("Customer Name", "RRFromName"))
            ddlField2.Items.Add(New ListItem("Received By", "ReceivedBy"))
            ddlField2.Items.Add("Remark")

            GridView1.Columns(8).HeaderText = "Customer"
        ElseIf (Request.QueryString("ContainerId").ToString = "STRRSupplierID") Then
            Labelmenu.Text = "RR SUPPLIER"
            'GridDt.Columns(7).Visible = False\\ Edited 22 April 2019(Mirza)
            'GridDt.Columns(8).Visible = False
            ViewState("RRType") = "RRS"

            'Labelmenu.Text = "RR SUPPLIER"
            Label1.Visible = False
            lbRRFrom.Visible = True
            lbRRFrom.Text = "Supplier"
            tbRRFrom.Width = 103
            tbRRFromName.Visible = True
            btnRRFrom.Visible = tbRRFromName.Visible

            'ddlField.Items.Add(New ListItem("Supplier", "RRForm"))
            ddlField.Items.Add(New ListItem("Supplier Name", "RRFromName"))
            ddlField.Items.Add(New ListItem("Received By", "ReceivedBy"))
            ddlField.Items.Add("Remark")

            'ddlField2.Items.Add(New ListItem("Supplier", "RRForm"))
            ddlField2.Items.Add(New ListItem("Supplier Name", "RRFromName"))
            ddlField2.Items.Add(New ListItem("Received By", "ReceivedBy"))
            ddlField2.Items.Add("Remark")

            GridView1.Columns(8).HeaderText = "Supplier"

        ElseIf (Request.QueryString("ContainerId").ToString = "STRROtherID") Then
            Labelmenu.Text = "RR OTHER"
            'GridDt.Columns(7).Visible = False \\ Edited 22 April 2019(Mirza)
            'GridDt.Columns(8).Visible = False
            ViewState("RRType") = "RRO"

            'Labelmenu.Text = "RR OTHER"
            Label1.Visible = True
            lbRRFrom.Visible = False
            tbRRFrom.Width = 335
            tbRRFromName.Visible = False
            lbRRFrom.Text = "Other"
            btnRRFrom.Visible = tbRRFromName.Visible

            'ddlField.Items.Add(New ListItem("RR From", "RRForm"))
            ddlField.Items.Add(New ListItem("RR From Name", "RRFromName"))
            ddlField.Items.Add(New ListItem("Received By", "ReceivedBy"))
            ddlField.Items.Add("Remark")

            'ddlField2.Items.Add(New ListItem("RR From", "RRForm"))
            ddlField2.Items.Add(New ListItem("RR From Name", "RRFromName"))
            ddlField2.Items.Add(New ListItem("Received By", "ReceivedBy"))
            ddlField2.Items.Add("Remark")


            GridView1.Columns(8).HeaderText = "RR From"

        End If

        FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
        If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
            ddlCommand.Items.Add("Print")
            ddlCommand2.Items.Add("Print")
        End If
        'Me.tbQty.Attributes.Add("OnKeyDown", "return PressNumeric();")
        'Me.tbQty.Attributes.Add("OnBlur", "setformat();")
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
            If Request.QueryString("ContainerId").ToString = "STRRCustomerID" Then
                GetStringHd = "Select * From V_STRROtherHd where RRType = 'RRC' "
            ElseIf Request.QueryString("ContainerId").ToString = "STRRSupplierID" Then
                GetStringHd = "Select * From V_STRROtherHd where RRType = 'RRS'"
            ElseIf Request.QueryString("ContainerId").ToString = "STRROtherID" Then
                GetStringHd = "Select * From V_STRROtherHd where RRType = 'RRO'"
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
        Return "SELECT * From V_STRROtherDt WHERE TransNmbr = " + QuotedStr(Nmbr)
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

                Session("DBConnection") = ViewState("DBConnection")
                Session("SelectCommand") = "EXEC S_STFormRROther " + Result + ", " + QuotedStr(ViewState("UserId"))
                Session("ReportFile") = ".../../../Rpt/FormSTRROther.frx"
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
                        Result = ExecSPCommandGo(ActionValue, "S_STRROther", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            btnGetDt.Visible = State
            If (Request.QueryString("ContainerId").ToString = "STRROtherID") Then
                '    'tbRRFrom.Enabled = State
                '    'tbRRFromName.Enabled = State
                '    btnRRFrom.Enabled = State
                btnRRFrom.Visible = False

            Else
                '    tbRRFrom.Enabled = State
                '    tbRRFromName.Enabled = State
                '    btnRRFrom.Enabled = State
                btnRRFrom.Visible = State

            End If
            ddlwrhs.Enabled = ddlwrhs.SelectedValue = "" Or GetCountRecord(ViewState("Dt")) = 0 'State ' And tbPONo.Text = ""
            tbSubLed.Enabled = State And tbFgSubLed.Text.Trim <> "N"
            btnSubLed.Visible = tbSubLed.Enabled
            tbRRFrom.Enabled = btnRRFrom.Enabled And (Request.QueryString("ContainerId").ToString = "STRROtherID")
            'ddlwrhs.Enabled = State
            'tbSubLed.Enabled = State And tbFgSubLed.Text.Trim <> "N"
            'btnSubLed.Visible = tbSubLed.Enabled
            ''btnSupp.Visible = State
            'tbDate.Enabled = State
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
                    If CekExistData(ViewState("Dt"), "Product ", tbProductCode.Text) Then
                        lbStatus.Text = "Product " + tbProductName.Text + "' has been already exist"
                        Exit Sub
                    End If
                End If
                Row = ViewState("Dt").Select("Product = " + QuotedStr(ViewState("DtValue")))(0)
                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                Row("Product") = tbProductCode.Text
                Row("ProductName") = tbProductName.Text
                Row("Specification") = tbSpecification.Text
                Row("Qty") = tbQty.Text
                Row("Unit") = tbUnit.Text
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()

            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If

                If CekExistData(ViewState("Dt"), "Product", tbProductCode.Text) = True Then
                    lbStatus.Text = "Product " + tbProductName.Text + " has been already exist"
                    Exit Sub
                End If

                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("Product") = tbProductCode.Text
                dr("ProductName") = tbProductName.Text
                dr("Specification") = tbSpecification.Text
                dr("Qty") = tbQty.Text
                dr("Unit") = tbUnit.Text
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
            If Request.QueryString("ContainerId").ToString = "STRRCustomerID" Then
                tbRRType.Text = "RRC"
            ElseIf Request.QueryString("ContainerId").ToString = "STRRSupplierID" Then
                tbRRType.Text = "RRS"
            ElseIf Request.QueryString("ContainerId").ToString = "STRROtherID" Then
                tbRRType.Text = "RRO"
            End If
            If Not (ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit") Then
                Exit Sub
            Else
                ViewState("StateSave") = ViewState("StateHd")
                ViewState("StateHd") = "View"
            End If
            'Save Hd
            If ViewState("StateSave") = "Insert" Then
                tbReference.Text = GetAutoNmbr(tbRRType.Text, "N", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
                SQLString = "INSERT INTO STCRROtherHd (TransNmbr, Status, TransDate, Warehouse, FgSubLed, SubLed, RRFrom, RRType, " + _
                "ReceivedBy, SJNo, SJDate, Remark, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbReference.Text) + ", 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', " + _
                QuotedStr(ddlwrhs.SelectedValue) + ", " + QuotedStr(tbFgSubLed.Text) + ", " + QuotedStr(tbSubLed.Text) + ", " + _
                QuotedStr(tbRRFrom.Text) + ", " + QuotedStr(tbRRType.Text) + ", " + _
                QuotedStr(tbReceivedBy.Text) + ", " + QuotedStr(tbSJSuppNo.Text) + ", '" + Format(tbSJSuppDate.SelectedValue, "yyyy-MM-dd") + "', " + _
                QuotedStr(tbRemark.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", getDate()"
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM STCRROtherHd WHERE TransNmbr = " + QuotedStr(tbReference.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE STCRROtherHd SET TransDate = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "' " + _
                ", RRFrom = " + QuotedStr(tbRRFrom.Text) + _
                ", Warehouse = " + QuotedStr(ddlwrhs.SelectedValue) + ", FgSubLed = " + QuotedStr(tbFgSubLed.Text) + _
                ", SubLed = " + QuotedStr(tbSubLed.Text) + ", ReceivedBy = " + QuotedStr(tbReceivedBy.Text) + ", SJNo = " + QuotedStr(tbSJSuppNo.Text) + ", SJDate = '" + Format(tbSJSuppDate.SelectedValue, "yyyy-MM-dd") + "' " + _
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
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, Product, Qty, Unit, Remark FROM STCRROtherDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
                    "UPDATE STCRROtherDt SET Product = @Product, Qty = @Qty, Unit = @Unit, Remark = @Remark " + _
                    "WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND Product = @OldProduct", con)
            ' Define output parameters.
            Update_Command.Parameters.Add("@Product", SqlDbType.VarChar, 20, "Product")
            Update_Command.Parameters.Add("@Qty", SqlDbType.Float, 18, "Qty")
            Update_Command.Parameters.Add("@Unit", SqlDbType.VarChar, 5, "Unit")
            Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 255, "Remark")
            ' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@OldProduct", SqlDbType.VarChar, 20, "Product")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM STCRROtherDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND Product = @Product ", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@Product", SqlDbType.VarChar, 20, "Product")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command
            Dim Dt As New DataTable("STCRROtherDt")

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
            If (Request.QueryString("ContainerId").ToString = "STRRCustomerID") Then
                tbRRFromName.Visible = True
                btnRRFrom.Visible = tbRRFromName.Visible
            ElseIf (Request.QueryString("ContainerId").ToString = "STRRSupplierID") Then
                tbRRFromName.Visible = True
                btnRRFrom.Visible = tbRRFromName.Visible
            Else
                tbRRFromName.Visible = False
                btnRRFrom.Visible = tbRRFromName.Visible
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
            tbSJSuppDate.SelectedDate = ViewState("ServerDate") 'Today
            tbReceivedBy.Text = ViewState("UserId")
            pnlDt.Visible = True
            btnGetDt.Visible = True
            BindDataDt("")
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbReference.Text = ""
            ddlwrhs.SelectedIndex = 0
            ddlwrhs.Enabled = True
            tbFgSubLed.Text = "N"
            tbSubLed.Text = ""
            tbSubLed.Enabled = tbFgSubLed.Text <> "N"
            btnSubLed.Visible = tbSubLed.Enabled
            tbSubLedName.Text = ""
            tbReceivedBy.Text = ""
            tbRRFrom.Text = ""
            tbRRFromName.Text = ""
            tbSJSuppNo.Text = ""
            tbRemark.Text = ""
            tbRRType.Text = ViewState("RRType")
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbProductCode.Text = ""
            tbProductName.Text = ""
            tbQty.Text = "0"
            tbUnit.Text = ""
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

    Protected Sub btnProduct_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProduct.Click
        Dim ResultField As String
        Try
            If (Request.QueryString("ContainerId").ToString = "STRRCustomerID") Then
                Session("Filter") = "SELECT Product_Code, Product_Name, Specification, Unit FROM VMsProduct WHERE Fg_Stock = 'Y' "
                ResultField = "Product_Code, Product_Name, Specification, Unit "
            ElseIf (Request.QueryString("ContainerId").ToString = "STRRSupplierID") Then
                Session("Filter") = "SELECT Product_Code, Product_Name, Specification, Unit FROM VMsSuppProduct where Supplier=  " + QuotedStr(tbRRFrom.Text)
                ResultField = "Product_Code, Product_Name, Specification, Unit "
            Else
                Session("Filter") = "SELECT Product_Code, Product_Name, Specification, Unit FROM VMsProduct WHERE Fg_Stock = 'Y'"
                ResultField = "Product_Code, Product_Name, Specification, Unit "
            End If
            ResultField = "Product_Code, Product_Name, Specification, Unit "
            ViewState("Sender") = "btnProduct"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Product Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbProductCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbProductCode.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try

            If (Request.QueryString("ContainerId").ToString = "STRRCustomerID") Then
                Dt = SQLExecuteQuery("SELECT Product_Code, Product_Name, Specification, Unit FROM VMsProduct WHERE Fg_Stock = 'Y'  AND Product_Code = '" + tbProductCode.Text + "'", ViewState("DBConnection").ToString).Tables(0)
            ElseIf (Request.QueryString("ContainerId").ToString = "STRRSupplierID") Then
                Dt = SQLExecuteQuery("SELECT Product_Code, Product_Name, Specification, Unit FROM VMsSuppProduct where Supplier=  " + QuotedStr(tbRRFrom.Text) + " AND  Product_Code = '" + tbProductCode.Text + "'", ViewState("DBConnection").ToString).Tables(0)
            Else
                Dt = SQLExecuteQuery("SELECT Product_Code, Product_Name, Specification, Unit FROM VMsProduct WHERE Fg_Stock = 'Y' AND Product_Code = '" + tbProductCode.Text + "'", ViewState("DBConnection").ToString).Tables(0)
            End If
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbProductCode.Text = Dr("Product_Code")
                tbProductName.Text = Dr("Product_Name")
                tbSpecification.Text = Dr("Specification")
                tbUnit.Text = Dr("Unit")
            Else
                tbProductCode.Text = ""
                tbProductName.Text = ""
                tbSpecification.Text = ""
                tbUnit.Text = ""
            End If
            tbProductCode.Focus()
        Catch ex As Exception
            Throw New Exception("tb Product Code TextChanged : " + ex.ToString)
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
        tbProductCode.Focus()
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
            '    Exit Function
            'End If
            If ddlwrhs.SelectedValue.Trim = "" Then
                lbStatus.Text = MessageDlg("Warehouse must have value")
                ddlwrhs.Focus()
                Return False
            End If
            If tbSubLed.Text.Trim = "" And tbFgSubLed.Text.Trim <> "N" Then
                lbStatus.Text = MessageDlg("SubLed must have value")
                tbSubLed.Focus()
                Return False
            End If
            If tbRRFrom.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("RR From must have value")
                tbRRFrom.Focus()
                Return False
            End If
            If tbSJSuppNo.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("SJ No must have value")
                tbSJSuppNo.Focus()
                Return False
            End If
            If tbReceivedBy.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Received By must have value")
                tbReceivedBy.Focus()
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
                If Dr("Product").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Product Must Have Value")
                    Return False
                End If
                If CFloat(Dr("Qty").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Warehouse Must Have Value")
                    Return False
                End If
                If Dr("Unit").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Unit Must Have Value")
                    Return False
                End If
            Else
                If tbProductCode.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Product Must Have Value")
                    tbProductCode.Focus()
                    Return False
                End If
                If CFloat(tbQty.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Must Have Value")
                    tbQty.Focus()
                    Return False
                End If
                If tbUnit.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Unit Must Have Value")
                    tbUnit.Focus()
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
            FilterName = "Reference, Date, Status, Warehouse, Warehouse Name, Subled, Subled Name, RR Form, RR From Name, Received By, Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), Status, Warehouse, WarehouseName, Subled, SubledName, RRForm, RRFromName, ReceivedBy, Remark"
            Session("DateFieldName") = FDateName.Split(",")
            Session("DateFieldValue") = FDateValue.Split(",")
            Session("FieldName") = FilterName.Split(",")
            Session("FieldValue") = FilterValue.Split(",")
            AttachScript("OpenFilterCriteria();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "Advanced Search Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridView1_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.PageIndexChanged
        
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
                        'ViewState("DigitCurr") = GetCurrDigit(ddlCurr.SelectedValue, Today, ViewState("DBConnection").ToString)
                        'EnableHd(GetCountRecord(ViewState("Dt")) = 0)
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
                        Session("SelectCommand") = "EXEC S_STFormRROther ''" + QuotedStr(GVR.Cells(2).Text) + "'', " + QuotedStr(ViewState("UserId"))
                        'lbStatus.Text = Session("SelectCommand")
                        'Exit Sub
                        Session("ReportFile") = ".../../../Rpt/FormSTRROther.frx"
                        Session("DBConnection") = ViewState("DBConnection")
                        AttachScript("openprintdlg();", Page, Me.GetType)
                    Catch ex As Exception
                        lbStatus.Text = "Print Error : " + ex.ToString
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
        Dim dr() As DataRow
        Dim GVR As DataRow = ViewState("Dt").Rows(e.RowIndex)
        dr = ViewState("Dt").Select("Product = " + QuotedStr(GVR("Product").ToString))
        dr(0).Delete()
        BindGridDt(ViewState("Dt"), GridDt)
        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As DataRow
        Try
            GVR = ViewState("Dt").Rows(e.NewEditIndex)
            ViewState("DtValue") = GVR("Product").ToString
            FillTextBoxDt(ViewState("DtValue"))
            MovePanel(pnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            tbProductCode.Focus()
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
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
    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            tbReference.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToText(tbRRFrom, Dt.Rows(0)("RRForm").ToString)
            BindToText(tbRRFromName, Dt.Rows(0)("RRFromName").ToString)
            BindToText(tbSJSuppNo, Dt.Rows(0)("SJNo").ToString)
            BindToDate(tbSJSuppDate, Dt.Rows(0)("SJDate").ToString)
            BindToDropList(ddlwrhs, Dt.Rows(0)("Warehouse").ToString)
            BindToText(tbFgSubLed, Dt.Rows(0)("FgSubLed").ToString)
            BindToText(tbSubLed, Dt.Rows(0)("SubLed").ToString)
            BindToText(tbSubLedName, Dt.Rows(0)("SubLedName").ToString)
            BindToText(tbReceivedBy, Dt.Rows(0)("ReceivedBy").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal Product As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("Product  = " + QuotedStr(Product))
            If Dr.Length > 0 Then
                BindToText(tbProductCode, Dr(0)("Product").ToString)
                BindToText(tbProductName, Dr(0)("ProductName").ToString)
                BindToText(tbSpecification, Dr(0)("Specification").ToString)
                BindToText(tbQty, Dr(0)("Qty").ToString)
                BindToText(tbUnit, Dr(0)("Unit").ToString)
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

    Protected Sub btnSubLed_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSubLed.Click
        Dim ResultField As String
        Try
            Session("filter") = "select SubLed_No, SubLed_Name from VMsSubLed WHERE FgSubLed = " + QuotedStr(tbFgSubLed.Text)
            ResultField = "SubLed_No, SubLed_Name"
            ViewState("Sender") = "btnSubLed"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Cust Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbSubLed_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbSubLed.TextChanged
        Dim Dr As DataRow
        Try
            'Dr = FindMaster("SubLed", tbSubLed.Text, ViewState("DBConnection").ToString)
            Dr = FindMaster("Subled", tbFgSubLed.Text + "|" + tbSubLed.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbSubLed.Text = Dr("SubLed_No")
                tbSubLedName.Text = Dr("SubLed_Name")
            Else
                tbSubLed.Text = ""
                tbSubLedName.Text = ""
            End If
            'AttachScript("setformat();", Page, Me.GetType())
            tbSubLed.Focus()
        Catch ex As Exception
            Throw New Exception("tb CustCode Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnGetDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGetDt.Click
        Dim ResultField As String 'ResultSame 
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            Session("Result") = Nothing
            If (Request.QueryString("ContainerId").ToString = "STRRCustomerID") Then
                Session("Filter") = "SELECT Product_Code, Product_Name, Specification, Unit FROM VMsProduct WHERE Fg_Stock = 'Y'"
                ResultField = "Product_Code, Product_Name, Specification, Unit "
            ElseIf (Request.QueryString("ContainerId").ToString = "STRRSupplierID") Then
                Session("Filter") = "SELECT Product_Code, Product_Name, Specification, Unit FROM VMsSuppProduct where Supplier=  " + QuotedStr(tbRRFrom.Text)
                ResultField = "Product_Code, Product_Name, Specification, Unit "
            Else
                Session("Filter") = "SELECT Product_Code, Product_Name, Specification, Unit FROM VMsProduct WHERE Fg_Stock = 'Y' "
                ResultField = "Product_Code, Product_Name, Specification, Unit "
            End If
            Session("Column") = ResultField.Split(",")
            ViewState("Sender") = "btnGetDt"
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn get Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlwrhs_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlwrhs.SelectedIndexChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("WrhsUser", ddlwrhs.SelectedValue + "|" + ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                'tbFgSubLed.Text = Dr("FgSubLed") '\\ Edited 22 April 2019(Mirza)
                tbSubLed.Text = ""
                tbSubLedName.Text = ""
            Else
                tbFgSubLed.Text = "N"
                tbSubLed.Text = ""
                tbSubLedName.Text = ""
            End If
            ViewState("SetLocation") = True
            tbSubLed.Enabled = tbFgSubLed.Text <> "N"
            btnSubLed.Visible = tbSubLed.Enabled
            ddlwrhs.Focus()
        Catch ex As Exception
            Throw New Exception("tb WrhsCode Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnRRFrom_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRRFrom.Click
        Dim ResultField As String
        Try
            If (Request.QueryString("ContainerId").ToString = "STRRCustomerID") Then
                Session("filter") = "SELECT Customer_Code, Customer_Name FROM VmsCustomer"
                ResultField = "Customer_Code, Customer_Name"
                ViewState("Sender") = "btnCustomer"
                Session("Column") = ResultField.Split(",")
            ElseIf (Request.QueryString("ContainerId").ToString = "STRRSupplierID") Then
                Session("filter") = "SELECT Supplier_Code, Supplier_Name FROM VmsSupplier"
                ResultField = "Supplier_Code, Supplier_Name"
                ViewState("Sender") = "btnSupplier"
                Session("Column") = ResultField.Split(",")
            End If
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Supplier No Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbRRFrom_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbRRFrom.Click
        Try
            If lbRRFrom.Text = "Customer" Then
                AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsCustomer')();", Page, Me.GetType())
            ElseIf lbRRFrom.Text = "Supplier" Then
                AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsSupplier')();", Page, Me.GetType())
            End If
        Catch ex As Exception
            lbStatus.Text = "lb RR From Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbRRFrom_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbRRFrom.TextChanged
        Dim Dr As DataRow
        Dim Dt As DataTable
        Try
            If lbRRFrom.Text = "Customer" Or lbRRFrom.Text = "Supplier" Then
                If lbRRFrom.Text = "Customer" Then
                    Dt = SQLExecuteQuery("SELECT Customer_Code AS RRFrom, Customer_Name AS RRFromName FROM VMsCustomer WHERE Customer_Code = '" + tbRRFrom.Text + "' ", ViewState("DBConnection").ToString).Tables(0)
                Else
                    Dt = SQLExecuteQuery("SELECT Supplier_Code AS RRFrom, Supplier_Name AS RRFromName FROM VMsSupplier WHERE Supplier_Code = '" + tbRRFrom.Text + "' ", ViewState("DBConnection").ToString).Tables(0)
                End If

                If Dt.Rows.Count > 0 Then
                    Dr = Dt.Rows(0)
                Else
                    Dr = Nothing
                End If
                If Not Dr Is Nothing Then
                    BindToText(tbRRFrom, Dr("RRFrom").ToString)
                    BindToText(tbRRFromName, Dr("RRFromName").ToString)
                Else
                    tbRRFrom.Text = ""
                    tbRRFromName.Text = ""
                End If
            End If
            'AttachScript("setformatdt();", Page, Me.GetType())
            'tbRRFrom.Focus()
        Catch ex As Exception
            Throw New Exception("tb RR From Error : " + ex.ToString)
        End Try
    End Sub

    Private Function FindProduct(ByVal Nmbr As String, ByVal Product As String) As String
        Dim dr As SqlDataReader
        dr = SQLExecuteReader("EXEC S_STRROtherFindProd " + QuotedStr(tbProductCode.Text), ViewState("DBConnection").ToString)
        dr.Read()
        Return dr("Product")
    End Function

    


End Class
