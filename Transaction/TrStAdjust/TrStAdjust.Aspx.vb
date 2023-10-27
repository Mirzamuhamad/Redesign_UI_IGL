Imports System.Data
Imports System.Data.SqlClient
'Imports System.Data.Odbc
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO
Imports System.Xml
Imports System.Data.OleDb


Partial Class TrStAdjust
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter

    Private Function GetStringHd() As String
        Return "SELECT DISTINCT TransNmbr, Nmbr, TransDate, Status, Warehouse, Wrhs_Name, FgSubLed, SubLed, SubLed_Name, Operator, Remark, AdjustType, FromOpname, ReffType, ReffOpname  From V_STAdjustHd WHERE AdjustType = 'SAD'"
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim CurrFilter, Value As String
        Try
            If Not IsPostBack Then
                InitProperty()
                FillCombo(ddlwrhs, "EXEC S_GetWrhsUser " + QuotedStr(ViewState("UserId")), True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection"))
                FillCombo(ddlCostCtr, "EXEC S_GetCostCtr ", True, "Cost_Ctr_Code", "Cost_Ctr_Name", ViewState("DBConnection"))
                
                SetInit()
                Session("AdvanceFilter") = ""
                ddlRow.SelectedValue = "20"

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
                If ViewState("Sender") = "btnOpnameMinus" Then
                    tbOpnameMinus.Text = Session("Result")(0).ToString
                    Dim dataMinus As DataTable
                    Dim dr As DataRow
                    Dim Product As String
                    dataMinus = SQLExecuteQuery("EXEC S_STAdjustGetStockMinusAfterOpname " + QuotedStr(tbOpnameMinus.Text.Trim) + ", " + QuotedStr(ddlReffType.SelectedValue), ViewState("DBConnection")).Tables(0)
                    For Each drResult In dataMinus.Rows

                        dr = ViewState("Dt").NewRow
                        If ddlFromOpname.SelectedValue = "Y" Then
                            dr("OpnameNo") = drResult("Opname_No")
                        Else
                            dr("OpnameNo") = " "
                        End If
                        dr("Product") = drResult("Product")
                        Product = drResult("Product")
                        dr("Product_Name") = drResult("Product_Name")
                        dr("Specification") = drResult("Specification")
                        dr("FgOperator") = drResult("FgOperator")
                        dr("Qty") = drResult("Qty")
                        dr("Unit") = drResult("Unit")
                        dr("PriceCost") = FormatNumber(GetPrice(Product, ddlwrhs.SelectedValue, Format(tbDate.SelectedValue, "yyyy-MM-dd")), ViewState("DigitHome"))
                        dr("TotalCost") = FormatNumber(CFloat(dr("Qty").ToString) * CFloat(dr("PriceCost").ToString), ViewState("DigitHome"))
                        dr("CostCtr") = TrimStr(drResult("CostCtr").ToString)
                        dr("Cost_Ctr_Name") = drResult("Cost_Ctr_Name")
                        ViewState("Dt").Rows.Add(dr)
                    Next
                    BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                End If
                If ViewState("Sender") = "btnSubLed" Then
                    tbSubLed.Text = Session("Result")(0).ToString
                    tbSubLedName.Text = Session("Result")(1).ToString
                End If
                If ViewState("Sender") = "btnProduct" Then

                    tbProductCode.Text = Session("Result")(0).ToString
                    tbProductName.Text = Session("Result")(1).ToString
                    BindToText(tbUnit, Session("Result")(2).ToString)
                    tbSpecification.Text = Session("Result")(3).ToString
                    BindToDropList(ddlCostCtr, Session("Result")(4).ToString)

                    If ddlFromOpname.SelectedValue = "Y" Then
                        If CDbl(Session("Result")(5).ToString.Replace(",", "")) > 0 Then
                            ddlFgOperator.SelectedValue = "+"
                            BindToText(tbQty, Session("Result")(5).ToString)
                        Else
                            ddlFgOperator.SelectedValue = "-"
                            BindToText(tbQty, FormatFloat(-CDbl(Session("Result")(5).ToString.Replace(",", "")), ViewState("DigitQty")))
                        End If

                        BindToText(tbRemarkDt, Session("Result")(6).ToString)
                        BindToText(tbOpnameNo, Session("Result")(7).ToString)
                    Else
                        tbQty.Text = FormatFloat(tbQty.Text, ViewState("DigitQty"))
                    End If
                    tbPrice.Text = FormatFloat(0, ViewState("DigitHome"))
                    tbTotal.Text = FormatFloat(0, ViewState("DigitHome"))
                    tbQty_TextChanged(Nothing, Nothing)

                End If
                If ViewState("Sender") = "btnGetDt" Then
                    Dim drResult As DataRow
                    Dim Product As String
                    For Each drResult In Session("Result").Rows
                        'insert
                        If CekExistData(ViewState("Dt"), "Product", drResult("Product_Code")) = False Then
                            Dim dr As DataRow
                            dr = ViewState("Dt").NewRow
                            If ddlFromOpname.SelectedValue = "Y" Then
                                dr("OpnameNo") = drResult("Opname_No")
                            Else
                                dr("OpnameNo") = " "
                            End If
                            If ddlFromOpname.SelectedValue = "N" Then
                                dr("Product") = drResult("Product_Code")
                                Product = drResult("Product_Code").ToString.Trim
                            Else
                                dr("Product") = drResult("Product")
                                Product = drResult("Product").ToString.Trim
                            End If
                            dr("Product_Name") = drResult("Product_Name")
                            dr("Specification") = drResult("Specification")
                            dr("CostCtr") = drResult("CostCtr")
                            dr("CostCtrName") = drResult("CostCtr_Name")
                            If ddlFromOpname.SelectedValue = "N" Then
                                dr("FgOperator") = "+"
                                dr("Qty") = 0

                            Else
                                If drResult("Qty") > 0 Then
                                    dr("FgOperator") = "+"
                                    dr("Qty") = drResult("Qty")
                                Else
                                    dr("FgOperator") = "-"
                                    dr("Qty") = -drResult("Qty")
                                End If

                            End If
                            dr("Unit") = drResult("Unit")

                            dr("PriceCost") = FormatNumber(0, ViewState("DigitHome"))
                            dr("TotalCost") = FormatNumber(0, ViewState("DigitHome"))
                            If ddlFromOpname.SelectedValue <> "N" Then
                                dr("Remark") = drResult("Remark")
                            End If
                            ViewState("Dt").Rows.Add(dr)
                        End If
                    Next
                BindGridDt(ViewState("Dt"), GridDt)
                EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                '    'Session("ResultSame") = Nothing

            End If
            If ViewState("Sender") = "btnGetDtNon" Then
                Dim drResult As DataRow
                Dim Product As String
                For Each drResult In Session("Result").Rows
                    'insert
                    If CekExistData(ViewState("Dt"), "Product", drResult("Product_Code")) = False Then
                        Dim dr As DataRow
                        dr = ViewState("Dt").NewRow
                        If ddlFromOpname.SelectedValue = "Y" Then
                            dr("OpnameNo") = drResult("Opname_No")
                        Else
                            dr("OpnameNo") = " "
                        End If
                        If ddlFromOpname.SelectedValue = "N" Then
                            dr("Product") = drResult("Product_Code")
                            Product = drResult("Product_Code").ToString.Trim
                        Else
                            dr("Product") = drResult("Product")
                            Product = drResult("Product").ToString.Trim
                        End If
                        dr("Product_Name") = drResult("Product_Name")
                        dr("Specification") = drResult("Specification")
                        dr("CostCtr") = drResult("CostCtr")
                        dr("CostCtrName") = drResult("CostCtrName")


                        If ddlFromOpname.SelectedValue = "N" Then
                            dr("FgOperator") = "+"
                            dr("Qty") = 0

                        Else
                            If drResult("Qty") > 0 Then
                                dr("FgOperator") = "+"
                                dr("Qty") = drResult("Qty")
                            Else
                                dr("FgOperator") = "-"
                                dr("Qty") = -drResult("Qty")
                            End If

                        End If
                        dr("Unit") = drResult("Unit")

                        If dr("FgOperator").ToString = "+" Then
                            dr("PriceCost") = FormatNumber(GetPrice(Product, ddlwrhs.SelectedValue, Format(tbDate.SelectedValue, "yyyy-MM-dd")), ViewState("DigitHome"))
                        Else
                            dr("PriceCost") = FormatNumber(0, ViewState("DigitHome"))
                        End If

                        dr("TotalCost") = FormatNumber(CFloat(dr("Qty").ToString) * CFloat(dr("PriceCost").ToString), ViewState("DigitHome"))
                        If ddlFromOpname.SelectedValue <> "N" Then
                            dr("Remark") = drResult("Remark")
                        End If
                        ViewState("Dt").Rows.Add(dr)
                    End If
                Next
                BindGridDt(ViewState("Dt"), GridDt)
                EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                '    'Session("ResultSame") = Nothing
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
        'ViewState("DigitCurr") = 0
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)

        FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
        If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
            ddlCommand.Items.Add("Print")
            ddlCommand2.Items.Add("Print")
        End If
        GridView1.PageSize = CInt(ViewState("PageSizeGrid"))
        Me.tbQty.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbPrice.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbQty.Attributes.Add("OnBlur", "setformat();")
    End Sub

    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim StrFilter As String
        Try
            StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue, ddlRange.SelectedValue)
            If AdvanceFilter.Length > 1 Then
                StrFilter = AdvanceFilter
            End If
            DT = BindDataTransaction(GetStringHd, StrFilter, ViewState("DBConnection").ToString)
            If DT.Rows.Count = 0 Then
                lbStatus.Text = "No Data"
                pnlNav.Visible = False
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
        Return "SELECT * From V_STAdjustDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try

            GridView1.PageIndex = 0
            Session("AdvanceFilter") = ""
            GridView1.PageSize = ddlRow.SelectedValue
            BindData(Session("AdvanceFilter"))
            pnlNav.Visible = True
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
                Session("SelectCommand") = "EXEC S_STFormBegin" + Result
                Session("ReportFile") = ".../../../Rpt/FormAdjust.frx"
                Session("DBConnection") = ViewState("DBConnection")
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
                        Result = ExecSPCommandGo(ActionValue, "S_STAdjust", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            btnGetDt.Visible = (ViewState("StateHd") = "Insert") Or (ViewState("StateHd") = "Edit")
            btnGetDtNon.Visible = (ViewState("StateHd") = "Insert") Or (ViewState("StateHd") = "Edit")
            ddlwrhs.Enabled = State 'And tbOpnameNo.Text = " "
            tbSubLed.Enabled = State And tbFgSubLed.Text.Trim <> "N"
            btnSubLed.Visible = tbSubLed.Enabled
            ddlReffType.Enabled = State
            btnOpnameMinus.Visible = State
            tbDate.Enabled = State
            ddlFromOpname.Enabled = State
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

    Protected Sub btnCancelDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelDt.Click
        Try
            MovePanel(pnlEditDt, pnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            StatusButtonSave(True)

        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt.Click
        Dim Row As DataRow
        Try
            If tbPrice.Text.Trim = "" Then
                tbPrice.Text = "0"
            End If
            If tbTotal.Text.Trim = "" Then
                tbTotal.Text = "0"
            End If
            If ViewState("StateDt") = "Edit" Then
                If ViewState("DtValue") <> tbProductCode.Text Then
                    If CekExistData(ViewState("Dt"), "Product", tbProductCode.Text) Then
                        lbStatus.Text = ViewState("Dt") + "Product" + tbProductCode.Text
                        Exit Sub
                        lbStatus.Text = "Product " + tbProductName.Text + "' has been already exist"
                        Exit Sub
                    End If
                End If
                Row = ViewState("Dt").Select("Product= " + QuotedStr(ViewState("DtValue")))(0)
                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                Row("OpnameNo") = tbOpnameNo.Text
                Row("Product") = tbProductCode.Text
                Row("Product_Name") = tbProductName.Text
                Row("Specification") = tbSpecification.Text
                Row("FgOperator") = ddlFgOperator.SelectedValue
                Row("Qty") = FormatFloat(tbQty.Text, ViewState("DigitQty"))
             
                Row("Unit") = tbUnit.Text
                Row("PriceCost") = tbPrice.Text
                Row("TotalCost") = tbTotal.Text
                If ddlCostCtr.SelectedValue = "" Then
                    Row("CostCtr") = ddlCostCtr.SelectedValue
                    Row("CostCtrName") = ""
                Else
                    Row("CostCtr") = ddlCostCtr.SelectedValue
                    Row("CostCtrName") = ddlCostCtr.SelectedItem.Text
                End If
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()
                'ViewState("Dt").AcceptChanges()
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
                dr("OpnameNo") = tbOpnameNo.Text
                dr("Product") = tbProductCode.Text
                dr("Product_Name") = tbProductName.Text
                dr("Specification") = tbSpecification.Text
                dr("FgOperator") = ddlFgOperator.SelectedValue
                dr("Qty") = FormatFloat(tbQty.Text, ViewState("DigitQty"))
              
                dr("Unit") = tbUnit.Text
                dr("PriceCost") = tbPrice.Text
                dr("TotalCost") = tbTotal.Text
                If ddlCostCtr.SelectedValue = "" Then
                    dr("CostCtr") = ddlCostCtr.SelectedValue
                    dr("CostCtrName") = ""
                Else
                    dr("CostCtr") = ddlCostCtr.SelectedValue
                    dr("CostCtrName") = ddlCostCtr.SelectedItem.Text
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
            If Not (ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit") Then
                Exit Sub
            Else
                ViewState("StateSave") = ViewState("StateHd")
                ViewState("StateHd") = "View"
            End If
            'Save Hd
            If ViewState("StateSave") = "Insert" Then
                tbReference.Text = GetAutoNmbr("SAD", "N", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), ddlwrhs.SelectedValue, ViewState("DBConnection").ToString)
           
                SQLString = "INSERT INTO STCAdjustHd (TransNmbr, Status, TransDate, Warehouse, FgSubLed, SubLed, " + _
                "FromOpname, ReffType, ReffOpname, Operator,  " + _
                "Remark, AdjustType, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbReference.Text) + ", 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', " + _
                QuotedStr(ddlwrhs.SelectedValue) + ", " + QuotedStr(tbFgSubLed.Text) + ", " + QuotedStr(tbSubLed.Text) + ", " + _
                QuotedStr(ddlFromOpname.SelectedValue) + ", " + QuotedStr(ddlReffType.SelectedValue) + ", " + QuotedStr(tbOpnameMinus.Text) + ", " + _
                QuotedStr(tbOperator.Text) + ", " + QuotedStr(tbRemark.Text) + ", 'SAD'," + QuotedStr(ViewState("UserId").ToString) + ", getDate()"
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM STCAdjustHd WHERE TransNmbr = " + QuotedStr(tbReference.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE STCAdjustHd SET TransDate = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "' " + _
                ", Warehouse = " + QuotedStr(ddlwrhs.SelectedValue) + ", FgSubLed = " + QuotedStr(tbFgSubLed.Text) + _
                ", SubLed = " + QuotedStr(tbSubLed.Text) + ", FromOpname = " + QuotedStr(ddlFromOpname.SelectedValue) + ", ReffType = " + QuotedStr(ddlReffType.SelectedValue) + ", ReffOpname = " + QuotedStr(tbOpnameMinus.Text) + _
                ", Operator = " + QuotedStr(tbOperator.Text) + ", Remark = " + QuotedStr(tbRemark.Text) + ", UserPrep = " + QuotedStr(ViewState("UserId").ToString) + ", DatePrep = getDate()" + _
                " WHERE TransNmbr = " + QuotedStr(tbReference.Text) + ""
            End If
            SQLString = ChangeQuoteNull(SQLString)
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
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, OpnameNo, Product, FgOperator, Qty, Unit, PriceCost, TotalCost, CostCtr, Remark FROM STCAdjustDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
                    "UPDATE STCAdjustDt SET OpnameNo = @OpnameNo, Product = @Product, FgOperator = @FgOperator, Qty = @Qty, Unit = @Unit, PriceCost = @PriceCost, TotalCost = @TotalCost, Remark = @Remark, " + _
                    " CostCtr = @CostCtr WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND OpnameNo = @OldOpnameNo AND Product = @OldProduct", con)
            ' Define output parameters.
            Update_Command.Parameters.Add("@OpnameNo", SqlDbType.VarChar, 20, "OpnameNo")
            Update_Command.Parameters.Add("@Product", SqlDbType.VarChar, 20, "Product")
            Update_Command.Parameters.Add("@FgOperator", SqlDbType.VarChar, 1, "FgOperator")
            Update_Command.Parameters.Add("@Qty", SqlDbType.Float, 18, "Qty")
            Update_Command.Parameters.Add("@Unit", SqlDbType.VarChar, 5, "Unit")
            Update_Command.Parameters.Add("@PriceCost", SqlDbType.Float, 18, "PriceCost")
            Update_Command.Parameters.Add("@TotalCost", SqlDbType.Float, 18, "TotalCost")
            Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 255, "Remark")
            Update_Command.Parameters.Add("@CostCtr", SqlDbType.VarChar, 20, "CostCtr")
            ' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@OldOpnameNo", SqlDbType.VarChar, 20, "OpnameNo")
            param.SourceVersion = DataRowVersion.Original
            param = Update_Command.Parameters.Add("@OldProduct", SqlDbType.VarChar, 20, "Product")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM STCAdjustDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND OpnameNo = @OpnameNo AND Product = @Product ", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@OpnameNo", SqlDbType.VarChar, 20, "OpnameNo")
            param.SourceVersion = DataRowVersion.Original
            param = Delete_Command.Parameters.Add("@Product", SqlDbType.VarChar, 20, "Product")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command
            Dim Dt As New DataTable("STCAdjustDt")

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
            pnlDt.Visible = True
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
            tbOperator.Text = ""
            tbRemark.Text = ""
            ddlFromOpname.SelectedValue = "N"
            tbOpnameMinus.Text = ""
            ddlReffType.SelectedValue = "+"
            ddlReffType.Enabled = True
            btnOpnameMinus.Visible = True
            'btnOpnameNo.Visible = True
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbProductCode.Text = ""
            tbProductName.Text = ""
            tbSpecification.Text = ""
            ddlFgOperator.SelectedValue = "+"
            tbQty.Text = FormatFloat(0, ViewState("DigitQty"))
            tbUnit.Text = ""
            tbPrice.Text = FormatNumber(0, ViewState("DigitHome"))
            tbTotal.Text = FormatNumber(0, ViewState("DigitHome"))
            tbOperator.Text = ViewState("UserId")
            tbRemarkDt.Text = ""
            tbOpnameNo.Text = " "
            ddlCostCtr.SelectedValue = ""
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
        Dim ResultField, CriteriaField As String
        Try
            If ddlFromOpname.SelectedValue = "N" Then
                CriteriaField = "Product_Code, Product_Name, Specification, Unit, CostCtr, CostCtr_Name, On_Hand, Product_Sub_Group, Product_Type"
                Session("Filter") = "EXEC S_STAdjustReff '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "' , " + QuotedStr(ddlwrhs.SelectedValue) + ", " + QuotedStr(tbSubLed.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + "," + QuotedStr(ddlFromOpname.SelectedValue)
                ResultField = "Product_Code, Product_Name, Unit, Specification, CostCtr, CostCtr_Name, Qn_Hand"
                Session("CriteriaField") = CriteriaField.Split(",")
            Else
                CriteriaField = "Product, Product_Name, Unit, Specification, CostCtr, Qty, Remark, Opname_No"
                Session("Filter") = "EXEC S_STAdjustReff '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "' , " + QuotedStr(ddlwrhs.SelectedValue) + ", " + QuotedStr(tbSubLed.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + "," + QuotedStr(ddlFromOpname.SelectedValue)
                ResultField = "Product, Product_Name, Unit, Specification, CostCtr, CostCtr_Name, Qty, Remark, Opname_No"
                Session("CriteriaField") = CriteriaField.Split(",")
            End If
            ViewState("Sender") = "btnProduct"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Product Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbProductCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbProductCode.TextChanged
        Dim dt As DataTable
        Dim Dr As DataRow
        Try
            dt = SQLExecuteQuery("SELECT Product_Code, Product_Name, Specification, Unit,CostCtr, CostCtrName FROM VMsProduct WHERE Product_Code = '" + tbProductCode.Text + "'", ViewState("DBConnection").ToString).Tables(0)
            If dt.Rows.Count > 0 Then
                Dr = dt.Rows(0)
                tbProductCode.Text = Dr("Product_Code")
                tbProductName.Text = Dr("Product_Name")
                tbSpecification.Text = TrimStr(Dr("Specification").ToString)
                tbUnit.Text = Dr("Unit")
                BindToDropList(ddlCostCtr, TrimStr(Dr("CostCtr").ToString))

                tbQty_TextChanged(Nothing, Nothing)
            Else
                tbProductCode.Text = ""
                tbProductName.Text = ""
                tbSpecification.Text = ""
                tbUnit.Text = ""
               ddlCostCtr.SelectedValue = ""

            End If
            AttachScript("setformatdt();", Page, Me.GetType())
            tbProductCode.Focus()
        Catch ex As Exception
            Throw New Exception("tb ProductCode Error : " + ex.ToString)
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
        tbProductCode.Enabled = ddlFromOpname.SelectedValue = "N"
        ddlFgOperator.Enabled = ddlFromOpname.SelectedValue = "N"
        tbQty.Enabled = ddlFromOpname.SelectedValue = "N"
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
            '    Return False
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
            If tbOperator.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Operator must have value")
                tbOperator.Focus()
                Return False
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("CeK Hd Error : " + ex.ToString)
        End Try
    End Function

    Function CekDt(Optional ByVal Dr As DataRow = Nothing) As Boolean
        Try
            If Not (Dr Is Nothing) Then
                If Dr.RowState = DataRowState.Deleted Then
                    Return True
                End If
                If ddlFromOpname.SelectedValue = "Y" Then
                    If Dr("OpnameNo").ToString.Trim = "" Then
                        lbStatus.Text = MessageDlg("Opname No Must Have Value")
                        Return False
                    End If
                End If
                If Dr("Product").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Product Must Have Value")
                    Return False
                End If
                If Dr("Unit").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Unit Must Have Value")
                    Return False
                End If
                 If Dr("CostCtr").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Cost Center Must Have Value")
                    Return False
                End If

            Else
                If ddlFromOpname.SelectedValue = "Y" Then
                    If tbOpnameNo.Text.Trim = "" Then
                        lbStatus.Text = MessageDlg("Opname No Must Have Value")
                        btnProduct.Focus()
                        Return False
                    End If
                End If
                If tbProductCode.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Product Must Have Value")
                    tbProductCode.Focus()
                    Return False
                End If
               If tbUnit.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Unit Must Have Value")
                    tbUnit.Focus()
                    Return False
                End If
                If ddlCostCtr.SelectedValue.Trim = "" Then
                    lbStatus.Text = MessageDlg("Cost Center Must Have Value")
                    ddlCostCtr.Focus()
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
            FilterName = "Adjust No, Adjust Date, Status, Warehouse, Operator, FromOpname, Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), Status, Wrhs_Name, Operator, From Opname, Remark"
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
                        btnHome.Visible = False
                       EnableHd(GetCountRecord(ViewState("Dt")) = 0)

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
                        Session("SelectCommand") = "EXEC S_STFormBegin ''" + QuotedStr(GVR.Cells(2).Text) + "''"
                        Session("ReportFile") = ".../../../Rpt/FormAdjust.frx"
                        Session("DBConnection") = ViewState("DBConnection")
                        AttachScript("openprintdlg();", Page, Me.GetType)
                    Catch ex As Exception
                        lbStatus.Text = "btn print Error = " + ex.ToString
                    End Try
                ElseIf DDL.SelectedValue = "Input Lot No" Then
                    'Dim i As Integer
                    Dim paramgo As String

                    Try
                        paramgo = "ADJUST|" + GVR.Cells(2).Text
                        Dim SQLString As String
                        SQLString = "Select Product from V_STStockLotReff WHERE TransNmbr = " + QuotedStr(GVR.Cells(2).Text) + " and Type = 'Adjust' "
                        Dim dt As DataTable
                        dt = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)
                        If dt.Rows.Count > 0 Then
                            Dim i As Integer
                            Dim pageurl As String
                            pageurl = HttpContext.Current.Request.Url.AbsoluteUri
                            i = pageurl.IndexOf("&transid")
                            If i > 0 Then
                                pageurl = pageurl.Substring(0, i)
                            End If
                            'lbStatus.Text = "***" + pageurl + "****"
                            'Exit Sub
                            Session("PrevPageStock") = pageurl  'HttpContext.Current.Request.Url.AbsoluteUri
                            AttachScript("OpenTransactionSelf('TrStockLot', '" + Request.QueryString("KeyId") + "', '" + paramgo + "' );", Page, Me.GetType())
                        Else
                            lbStatus.Text = "Transaction does not need input Lot No"
                        End If
                    Catch ex As Exception
                        lbStatus.Text = "Print Error : " + ex.ToString
                    End Try
                End If
            End If

        Catch ex As Exception
            lbStatus.Text = "Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridView1_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowCreated

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

    Dim TotalQty As Decimal = 0
    Protected Sub GridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt.RowDataBound
        Try
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "Product")) Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                    If DataBinder.Eval(e.Row.DataItem, "FgOperator") = "+" Then
                        TotalQty += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Qty"))
                    Else
                        TotalQty -= Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Qty"))
                    End If
                ElseIf e.Row.RowType = DataControlRowType.Footer Then
                    ' TotalQty = GetTotalSum(ViewState("Dt"), "Qty")
                    e.Row.Cells(5).Text = "Total : "
                    e.Row.Cells(6).Text = FormatFloat(TotalQty, ViewState("DigitQty"))

                End If
            End If
            'If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "Product")) Then
            '    If e.Row.RowType = DataControlRowType.DataRow Then
            '        If DataBinder.Eval(e.Row.DataItem, "FgOperator") = "+" Then
            '            TotalQty += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Qty"))
            '        Else
            '            TotalQty -= Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Qty"))
            '        End If
            '    ElseIf e.Row.RowType = DataControlRowType.Footer Then
            '        e.Row.Cells(5).Text = "Total:"
            '        e.Row.Cells(6).Text = FormatNumber(TotalQty, ViewState("DigitQty"))
            '    End If

            '    End If

        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Data Bound Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Dim dr() As DataRow
        Dim lb As Label
        Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
        dr = ViewState("Dt").Select("Product = " + QuotedStr(GVR.Cells(2).Text))
        dr(0).Delete()
        BindGridDt(ViewState("Dt"), GridDt)
        EnableHd(GetCountRecord(ViewState("Dt")) <> 0)
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Dim lb As Label
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            ViewState("DtValue") = GVR.Cells(2).Text
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
            BindToDropList(ddlwrhs, Dt.Rows(0)("Warehouse").ToString)
            BindToDropList(ddlFromOpname, Dt.Rows(0)("FromOpname").ToString)
            BindToText(tbOpnameMinus, Dt.Rows(0)("ReffOpname").ToString)
            BindToDropList(ddlReffType, Dt.Rows(0)("ReffType").ToString)
            BindToText(tbFgSubLed, Dt.Rows(0)("FgSubLed").ToString)
            BindToText(tbSubLed, Dt.Rows(0)("SubLed").ToString)
            BindToText(tbSubLedName, Dt.Rows(0)("SubLed_Name").ToString)
            BindToText(tbOperator, Dt.Rows(0)("Operator").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
            tbOpnameMinus.Text = ""

        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal Product As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("Product = " + QuotedStr(Product))
            If Dr.Length > 0 Then
                BindToText(tbOpnameNo, Dr(0)("OpnameNo").ToString)
                BindToText(tbProductCode, Dr(0)("Product").ToString)

                BindToText(tbProductName, Dr(0)("Product_Name").ToString)
                BindToText(tbSpecification, Dr(0)("Specification").ToString)
                BindToDropList(ddlFgOperator, Dr(0)("FgOperator").ToString)
                BindToDropList(ddlCostCtr, TrimStr(Dr(0)("CostCtr").ToString))
                BindToText(tbQty, Dr(0)("Qty").ToString)
                BindToText(tbUnit, Dr(0)("Unit").ToString)
                BindToText(tbPrice, Dr(0)("PriceCost").ToString)
                BindToText(tbTotal, Dr(0)("TotalCost").ToString)
                BindToText(tbRemarkDt, Dr(0)("Remark").ToString)
           
            End If
            tbProductCode.Enabled = ddlFromOpname.SelectedValue = "N"
            ddlFgOperator.Enabled = ddlFromOpname.SelectedValue = "N"
            tbQty.Enabled = ddlFromOpname.SelectedValue = "N"
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
            Dr = FindMaster("SubLed", tbSubLed.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbSubLed.Text = Dr("SubLed_No")
                tbSubLedName.Text = Dr("SubLed_Name")
            Else
                tbSubLed.Text = ""
                tbSubLedName.Text = ""
            End If
            AttachScript("setformat();", Page, Me.GetType())
            tbSubLed.Focus()
        Catch ex As Exception
            Throw New Exception("tb CustCode Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnGetDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGetDt.Click
        Dim ResultField, CriteriaField As String 'ResultSame 
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            Session("Result") = Nothing
            If ddlFromOpname.SelectedValue = "N" Then
                CriteriaField = "Product_Code, Product_Name, Specification, Unit, On_Hand, Product_Sub_Group, Product_Type "
                Session("Filter") = "EXEC S_STAdjustReff '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "' , " + QuotedStr(ddlwrhs.SelectedValue) + ", " + QuotedStr(tbSubLed.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", " + QuotedStr(ddlFromOpname.SelectedValue)

                ResultField = "Product_Code, Product_Name, specification, Unit, Qn_Hand, CostCtr, CostCtr_Name"
                Session("CriteriaField") = CriteriaField.Split(",")
            Else
                CriteriaField = "Opname_No, Product, Product_Name, Unit, Specification, Qty, Remark, CostCtr, CostCtr_Name"
                Session("Filter") = "EXEC S_STAdjustReff '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "' , " + QuotedStr(ddlwrhs.SelectedValue) + ", " + QuotedStr(tbSubLed.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", " + QuotedStr(ddlFromOpname.SelectedValue)
                
                ResultField = "Opname_No, Product, Product_Name, Unit, Specification, Qty, Remark, CostCtr, CostCtr_Name"
                Session("CriteriaField") = CriteriaField.Split(",")

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
                'tbFgSubLed.Text = Dr("FgSubLed")
                tbSubLed.Text = ""
                tbSubLedName.Text = ""
            Else
                'tbFgSubLed.Text = "N"
                tbSubLed.Text = ""
                tbSubLedName.Text = ""
            End If

            tbSubLed.Enabled = tbFgSubLed.Text <> "N"
            btnSubLed.Visible = tbSubLed.Enabled
            ddlwrhs.Focus()
        Catch ex As Exception
            Throw New Exception("tb WrhsCode Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnOpnameNo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOpnameNo.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT Opname_No, Opname_Date, Warehouse, Warehouse_Name, FgSubLed, SubLed, SubLed_Name, Operator, Remark " + _
                        "FROM V_STAdjustGetOpnameHd " + _
                        "WHERE PositionDate = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "' and UserId = " + QuotedStr(ViewState("UserId"))
            ResultField = "Opname_No"
            ViewState("Sender") = "btnOpnameNo"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Product Error : " + ex.ToString
        End Try
    End Sub

    Private Function GetPrice(ByVal product As String, ByVal wrhs As String, ByVal tgl As DateTime) As Double
        Dim dr As SqlDataReader
        dr = SQLExecuteReader("EXEC S_GetProductCOGSPrice " + QuotedStr(product) + ", " + QuotedStr(wrhs) + ", '" + Format(tgl, "yyyy-MM-dd") + "'", ViewState("DBConnection").ToString)
        dr.Read()
        Return dr("Price")
    End Function
    

    

    Protected Sub tbQty_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbQty.TextChanged
        Try
          tbQty.Text = FormatFloat(tbQty.Text, ViewState("DigitQty"))
            tbTotal.Text = FormatFloat(CFloat(tbQty.Text) * CFloat(tbPrice.Text), ViewState("DigitHome"))
        Catch ex As Exception
            lbStatus.Text = "tbQty_TextChanged Error : " + ex.ToString
        End Try
        
    End Sub
    
    

    Protected Sub ddlFromOpname_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFromOpname.SelectedIndexChanged
        'btnOpnameNo.Visible = ddlFromOpname.SelectedValue = "Y"
        tbProductCode.Enabled = ddlFromOpname.SelectedValue = "N"
        ddlFgOperator.Enabled = ddlFromOpname.SelectedValue = "N"
        tbQty.Enabled = ddlFromOpname.SelectedValue = "N"
    End Sub

    Dim QtyOnHand As Decimal = 0
    

    Protected Sub btnGetDtNon_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGetDtNon.Click
        Dim ResultField, CriteriaField As String 'ResultSame 
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            Session("Result") = Nothing
            If ddlFromOpname.SelectedValue = "N" Then
                CriteriaField = "Product_Code, Product_Name, Specification, Unit, CostCtr, CostCtrName"
                ResultField = "Product_Code, Product_Name, specification, Unit, CostCtr, CostCtrName"
                Session("Filter") = "EXEC S_STAdjustReffNon '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "' , " + QuotedStr(ddlwrhs.SelectedValue) + ", " + QuotedStr(tbSubLed.Text) + ", " + QuotedStr(ViewState("UserId").ToString)
                Session("CriteriaField") = CriteriaField.Split(",")
            Else
                'Opname_No = " + QuotedStr(tbOpnameNo.Text) + " and 
                Session("Filter") = "SELECT Opname_No, Product, Product_Name, Specification, Qty_Opname AS Qty, Unit, CostCtr, Cost_Ctr_Name, Remark FROM V_STAdjustGetOpnameDt WHERE Convert(Float,Replace(Qty_Opname,',','')) <> 0 AND PositionDate = '" + Format(tbDate.SelectedValue, "yyMMdd") + "' AND Warehouse = '" + ddlwrhs.SelectedValue + "' and SubLed = '" + tbSubLed.Text.Trim + "' "
                ResultField = "Opname_No, Product, Product_Name, Unit, Specification, Qty, CostCtr, Cost_Ctr_Name, Remark"
            End If
            Session("Column") = ResultField.Split(",")
            ViewState("Sender") = "btnGetDtNon"
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn get Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnOpnameMinus_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOpnameMinus.Click
        Dim ResultField As String
        Dim PositionDate As DateTime
        Try
            PositionDate = tbDate.SelectedValue
            PositionDate = PositionDate.AddDays(-1)
            If ddlwrhs.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Warehouse must have value")
                Exit Sub
            End If
            If tbFgSubLed.Text <> "N" And tbSubLed.Text = "" Then
                lbStatus.Text = "Warehouse Subled must have value"
                Exit Sub
            End If
            Session("filter") = "SELECT Opname_No, Opname_Date, Operator, Remark " + _
                        "FROM V_STAdjustGetOpnameHd " + _
                        "WHERE PositionDate = '" + Format(PositionDate, "yyyy-MM-dd") + "' and Warehouse = " + QuotedStr(ddlwrhs.SelectedValue) + " and Subled = " + QuotedStr(tbSubLed.Text) + " and UserId = " + QuotedStr(ViewState("UserId"))
            
            ResultField = "Opname_No"
            ViewState("Sender") = "btnOpnameMinus"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search OpnameMinus Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub ddlRow_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlRow.SelectedIndexChanged
        Try
            GridView1.PageIndex = 0
            GridView1.EditIndex = -1
            GridView1.PageSize = ddlRow.SelectedValue
            Session("AdvanceFilter") = ""
            BindData(Session("AdvanceFilter"))
        Catch ex As Exception
            lbStatus.Text = "ddlRow_SelectedIndexChanged =" + vbCrLf + ex.ToString
        End Try
    End Sub
    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        Dim index As Integer
        Try
            index = Int32.Parse(e.Item.Value)
            MultiView1.ActiveViewIndex = index
        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub
    Private Sub GetExcelSheets(ByVal FilePath As String, ByVal Extension As String, ByVal isHDR As String)
        Dim conStr As String = ""
        Select Case Extension
            Case ".xls"
                'Excel 97-03 
                conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'"
                Exit Select
            Case ".xlsx"
                'Excel 07 
                conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'"  'ConfigurationManager.ConnectionStrings("Excel07ConString").ConnectionString
                Exit Select
        End Select

        'Get the Sheets in Excel WorkBoo 
        conStr = String.Format(conStr, FilePath, isHDR)
        'lbStatus.Text = conStr
        'Exit Sub
        Dim connExcel As New OleDbConnection(conStr)
        Dim cmdExcel As New OleDbCommand()
        Dim oda As New OleDbDataAdapter()
        cmdExcel.Connection = connExcel
        If connExcel.State = ConnectionState.Closed Then
            connExcel.Open()
        End If
        'Bind the Sheets to DropDownList 
        ddlSheets.Items.Clear()
        ddlSheets.DataSource = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing)
        ddlSheets.DataTextField = "TABLE_NAME"
        ddlSheets.DataValueField = "TABLE_NAME"
        ddlSheets.DataBind()
        connExcel.Close()

        ddlSheets.SelectedIndex = 0
        DataExcel(ddlSheets.SelectedValue, FilePath, Extension, isHDR)
      End Sub
    Private Sub DataExcel(ByVal sheet As String, ByVal FilePath As String, ByVal Extension As String, ByVal isHDR As String)
        Try
            Dim conStr As String = ""
            Select Case Extension
                Case ".xls"
                    'Excel 97-03 
                    conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'"
                    Exit Select
                Case ".xlsx"
                    'Excel 07 
                    conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'"  'ConfigurationManager.ConnectionStrings("Excel07ConString").ConnectionString
                    Exit Select
            End Select

            'Get the Sheets in Excel WorkBoo 
            conStr = String.Format(conStr, FilePath, isHDR)
            Dim query As String = "SELECT * FROM [" + sheet + "]"
            Dim conn As New OleDbConnection(conStr)
            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If
            Dim cmd As New OleDbCommand(query, conn)
            Dim da As New OleDbDataAdapter(cmd)
            Dim ds As New DataSet
            da.Fill(ds)
            GridExcel.DataSource = ds.Tables(0)
            GridExcel.DataBind()

            Dim count As Integer
            count = ds.Tables(0).Columns.Count - 1
            ddlFindProductCode.Items.Clear()
            ddlFindProductCode.Items.Add(New ListItem("--Choose One--", ""))
            ddlFindQty.Items.Clear()
            ddlFindQty.Items.Add(New ListItem("--Choose One--", ""))
            ddlFindRemark.Items.Clear()
            ddlFindRemark.Items.Add(New ListItem("--Choose One--", ""))
            ddlFindCostCtr.Items.Clear()
            ddlFindCostCtr.Items.Add(New ListItem("--Choose One--", ""))
            ddlFindAdjust.Items.Clear()
            ddlFindAdjust.Items.Add(New ListItem("--Choose One--", ""))
            For j = 0 To count
                ddlFindProductCode.Items.Add(New ListItem(ds.Tables(0).Columns(j).ColumnName, ds.Tables(0).Columns(j).ColumnName))
                ddlFindQty.Items.Add(New ListItem(ds.Tables(0).Columns(j).ColumnName, ds.Tables(0).Columns(j).ColumnName))
                ddlFindRemark.Items.Add(New ListItem(ds.Tables(0).Columns(j).ColumnName, ds.Tables(0).Columns(j).ColumnName))
                ddlFindCostCtr.Items.Add(New ListItem(ds.Tables(0).Columns(j).ColumnName, ds.Tables(0).Columns(j).ColumnName))
                ddlFindAdjust.Items.Add(New ListItem(ds.Tables(0).Columns(j).ColumnName, ds.Tables(0).Columns(j).ColumnName))
          
            Next
            da.Dispose()
            conn.Close()
            conn.Dispose()
        Catch ex As Exception
            lbStatus.Text = "DataExcel Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnUploadExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUploadExcel.Click
        If fileuploadExcel.HasFile Then
            Dim FileName As String = Path.GetFileName(fileuploadExcel.PostedFile.FileName)
            Dim Extension As String = Path.GetExtension(fileuploadExcel.PostedFile.FileName)
            Dim FolderPath As String = ConfigurationManager.AppSettings("FolderPath")

            Dim FilePath As String = Server.MapPath("~/ExcelHSE/" + FileName)
            fileuploadExcel.SaveAs(FilePath)
            ViewState("FilePath") = FilePath
            ViewState("Extension") = Extension
            GetExcelSheets(FilePath, Extension, "Yes")
        End If
    End Sub
    Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
        Try
            ' call sp S_STOpnameTempDelete
            SQLExecuteNonQuery("EXEC S_STAdjustTempDelete " + QuotedStr(ViewState("UserId").ToString) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyyMMdd")) + ", " + QuotedStr(ddlwrhs.SelectedValue) + ", " + QuotedStr(tbSubLed.Text.Trim), ViewState("DBConnection").ToString)
            importtoTemp()
            importsave()

            BindGridDt(ViewState("Dt"), GridDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            GridDt.Columns(1).Visible = True


            MultiView1.ActiveViewIndex = 0
            Menu1.Items.Item(0).Selected = True
            'BindDataDt(tbCode.Text)
        Catch ex As Exception
            lbStatus.Text = "btnGenerate_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub importtoTemp()
        Try
            Dim GVR As GridViewRow
            Dim SQLstring As String
            If (ddlFindProductCode.SelectedValue = "") Then
                lbStatus.Text = MessageDlg("Product Code must have value")
                ddlFindProductCode.Focus()
                Exit Sub
            End If
            If (ddlFindQty.SelectedValue = "") Then
                lbStatus.Text = MessageDlg("Qty Wrhs must have value")
                ddlFindQty.Focus()
                Exit Sub
            End If
            If (ddlFindCostCtr.SelectedValue = "") Then
                lbStatus.Text = MessageDlg("Cost Center must have value")
                ddlFindCostCtr.Focus()
                Exit Sub
            End If
            If (ddlFindAdjust.SelectedValue = "") Then
                lbStatus.Text = MessageDlg("Adjust must have value")
                ddlFindAdjust.Focus()
                Exit Sub
            End If

            Dim IdFindProductCode, IdFindQty, IdFindCostCtr, IdFindRemark, IdFindAdjust As Integer
            Dim VarFindProductCode, VarFindQty, VarFindCostCtr, VarFindRemark, VarFindAdjust As String
            IdFindProductCode = ddlFindProductCode.SelectedIndex - 1
            IdFindQty = ddlFindQty.SelectedIndex - 1
            IdFindRemark = ddlFindRemark.SelectedIndex - 1
            IdFindCostCtr = ddlFindCostCtr.SelectedIndex - 1
            IdFindAdjust = ddlFindAdjust.SelectedIndex - 1
         

            For Each GVR In GridExcel.Rows
                If IdFindProductCode < 0 Then
                    VarFindProductCode = ""
                Else
                    VarFindProductCode = GVR.Cells(IdFindProductCode).Text
                End If
                'lbStatus.Text = VarFindItem
                'Exit Sub
                If IdFindQty < 0 Then
                    VarFindQty = ""
                Else
                    VarFindQty = GVR.Cells(IdFindQty).Text
                    ' tbQtyActual_TextChanged(Nothing, Nothing)
                End If
               If IdFindRemark < 0 Then
                    VarFindRemark = ""
                Else
                    VarFindRemark = GVR.Cells(IdFindRemark).Text
                End If
                If IdFindAdjust < 0 Then
                    VarFindAdjust = ""
                Else
                    VarFindAdjust = GVR.Cells(IdFindAdjust).Text
                End If

           
                If IdFindCostCtr <= 0 Then
                    VarFindCostCtr = "0"
                Else
                    VarFindCostCtr = GVR.Cells(IdFindCostCtr).Text
                    If GVR.Cells(IdFindCostCtr).Text = "" Then
                        VarFindCostCtr = "0"
                    End If
                End If
             
                VarFindProductCode = TrimStr(VarFindProductCode)
                VarFindQty = TrimStr(VarFindQty)
               VarFindCostCtr = TrimStr(VarFindCostCtr)
                VarFindRemark = TrimStr(VarFindRemark)
                VarFindAdjust = TrimStr(VarFindAdjust)
                If VarFindQty.Length = 0 Then
                    VarFindQty = "0"
                End If
              If VarFindCostCtr.Length = 0 Then
                    VarFindCostCtr = "0"
                End If
                If VarFindProductCode.Length > 0 Then
                    SQLstring = "EXEc S_STAdjustTempImportDt " + QuotedStr(ViewState("UserId").ToString) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyyMMdd")) + ", " + QuotedStr(ddlwrhs.SelectedValue) + ", " + QuotedStr(tbSubLed.Text.Trim) + ", " + _
                    QuotedStr(VarFindProductCode) + ", " + VarFindQty.Replace(",", "") + QuotedStr(VarFindCostCtr) + "," + QuotedStr(VarFindAdjust) + ", " + QuotedStr(VarFindRemark)
                    SQLExecuteNonQuery(SQLstring, ViewState("DBConnection"))
                End If
            Next
        Catch ex As Exception
            lbStatus.Text = "importtoTemp Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub importsave()
        Dim DrProduct, Row, Dr As DataRow
        Dim DtProduct As DataTable

        Try
            DtProduct = SQLExecuteQuery("EXEC S_STAdjustTempView " + QuotedStr(ViewState("UserId").ToString) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyyMMdd")) + ", " + QuotedStr(ddlwrhs.SelectedValue) + ", " + QuotedStr(tbSubLed.Text.Trim), ViewState("DBConnection").ToString).Tables(0)
            'lbStatus.Text = "EXEC S_STOpnameTempView " + QuotedStr(ViewState("UserId").ToString) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyyMMdd")) + ", " + QuotedStr(ddlWrhs.SelectedValue) + ", " + QuotedStr(tbSubled.Text.Trim)
            'Exit Sub

            For Each DrProduct In DtProduct.Rows
                If CekExistData(ViewState("Dt"), "Product", TrimStr(DrProduct("Product_Code"))) Then
                    Row = ViewState("Dt").Select("Product = " + QuotedStr(TrimStr(DrProduct("Product_Code"))))(0)
                    Row.BeginEdit()
                    Row("OpnameNo") = ""
                    Row("Product") = DrProduct("Product_Code")
                    Row("Product_Name") = DrProduct("Product_Name")
                    Row("Specification") = DrProduct("Specification")
                     Row("Remark") = DrProduct("Remark")
                    Row("Unit") = DrProduct("Unit")
                    Row("CostCtr") = DrProduct("CostCenter")
                    Row("CostCtrName") = DrProduct("CostCenterName")
                    Row("FgOperator") = DrProduct("FgAdjust")
                    Row("Qty") = FormatNumber(CFloat(DrProduct("Qty").ToString), ViewState("DigitHome"))
                    Row.EndEdit()
                Else
                    Dr = ViewState("Dt").NewRow
                    Dr("OpnameNo") = ""
                    Dr("Product") = DrProduct("Product_Code")
                    Dr("Product_Name") = DrProduct("Product_Name")
                    Dr("Specification") = DrProduct("Specification")
                    Dr("Remark") = DrProduct("Remark")
                    Dr("Unit") = DrProduct("Unit")
                    Dr("CostCtr") = DrProduct("CostCenter")
                    Dr("CostCtrName") = DrProduct("CostCenterName")
                    Dr("FgOperator") = DrProduct("FgAdjust")
                    Dr("Qty") = FormatNumber(CFloat(DrProduct("Qty").ToString), ViewState("DigitHome"))
                    ViewState("Dt").Rows.Add(Dr)
                End If
            Next
        
        Catch ex As Exception
            lbStatus.Text = "importsave Error : " + ex.ToString
        End Try
    End Sub
End Class

