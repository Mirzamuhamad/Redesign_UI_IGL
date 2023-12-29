Imports System.Data
Imports System.Data.SqlClient
'Imports System.Data.Odbc
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc

Partial Class Transaction_TrSTIssueSlip_TrSTIssueSlip
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "SELECT * From V_STIssueSlipHd  "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Dim CurrFilter, Value As String
        Try
            If Not IsPostBack Then
                InitProperty()
                FillCombo(ddlwrhs, "EXEC S_GetWrhsUser " + QuotedStr(ViewState("UserId")), True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection"))
                'FillCombo(ddlDepartment, "EXEC S_GetDepartment", True, "Department_Code", "Department_Name", ViewState("DBConnection"))

                FillCombo(ddlDepartment, "SELECT Department_Code, Department_Name FROM V_STIssueSlipGetDataReff", True, "Department_Code", "Department_Name", ViewState("DBConnection"))

                SetInit()
                Session("AdvanceFilter") = ""
                lbCount.Text = SQLExecuteScalar("SELECT  Count(DISTINCT Request_No) from V_STIssueSlipGetDataReff ", ViewState("DBConnection").ToString)
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
                If ViewState("Sender") = "btnRequestNo" Then
                    ''''tbreq()
                    tbReffType.Text = Session("Result")(0).ToString
                    tbReqNo.Text = Session("Result")(1).ToString
                    BindToDropList(ddlDepartment, Session("Result")(2).ToString)
                    BindToText(tbRequestBy, Session("Result")(3).ToString)
                    BindToText(tbpurpose, Session("Result")(6).ToString)
                    Dim dr As DataRow
                    Dim dt As DataTable
                    Dim sqlstring As String
                    sqlstring = "select request_no, product_code, product_name, specification, qty, unit, remarkDt from V_STIssueSlipGetDataReff where request_no = " + QuotedStr(tbReqNo.Text)
                    dt = SQLExecuteQuery(sqlstring, ViewState("DBConnection").ToString).Tables(0)
                    Dim drresult As DataRow
                    For Each drresult In dt.Rows
                        'insert
                        dr = ViewState("Dt").newrow
                        dr("product") = drresult("product_code")
                        dr("product_name") = drresult("product_name")
                        dr("specification") = TrimStr(drresult("specification").ToString)
                        dr("remark") = drresult("remarkDt")
                        dr("Qty") = drresult("Qty")
                        dr("unit") = drresult("unit")
                        ViewState("Dt").rows.add(dr)
                    Next
                    BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    'tbQty.Enabled = False

                End If
                If ViewState("Sender") = "btnSubLed" Then
                    tbSubLed.Text = Session("Result")(0).ToString
                    tbSubLedName.Text = Session("Result")(1).ToString
                End If
                If ViewState("Sender") = "btnVehicle" Then
                    tbVehicle.Text = Session("Result")(0).ToString
                    tbVehicleName.Text = Session("Result")(1).ToString
                End If

                If ViewState("Sender") = "btnOut" Then
                    BtnAdd_Click(Nothing, Nothing)
                    Dim drResult As DataRow
                    Dim FirstTime As Boolean = True
                    For Each drResult In Session("Result").Rows
                        'insert
                        If FirstTime Then
                            BindToDropList(ddlDepartment, drResult("Department_Code"))
                            BindToDropList(ddlwrhs, drResult("Warehouse"))
                            BindToText(tbReqNo, drResult("Request_No"))
                            BindToText(tbpurpose, drResult("Purpose"))
                            BindToText(tbRequestBy, drResult("RequestBy"))
                            BindToText(tbIssuedBy, ViewState("UserId"))
                        End If
                        If CekExistData(ViewState("Dt"), "Product", drResult("Product_Code")) = False Then
                            Dim dr As DataRow
                            dr = ViewState("Dt").NewRow
                            dr("Product") = drResult("Product_Code")
                            dr("Product_Name") = drResult("Product_Name")
                            dr("Specification") = TrimStr(drResult("Specification"))
                            dr("Qty") = drResult("Qty")
                            dr("Unit") = drResult("Unit")
                            ViewState("Dt").Rows.Add(dr)
                        End If

                    Next
                    BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    'ModifyDt()
                End If
                'If ViewState("Sender") = "btnFormula" Then
                'tbFormula.Text = Session("Result")(0).ToString
                'tbProcess.Text = Session("Result")(1).ToString
                'tbFormulaName.Text = Session("Result")(2).ToString
                'End If

                If ViewState("Sender") = "btnProduct" Then
                    'ResultField = "Product_Code, Product_Name, Unit, Specification, Qty"
                    tbProductCode.Text = Session("Result")(0).ToString
                    tbProductName.Text = Session("Result")(1).ToString
                    BindToText(tbUnit, Session("Result")(2).ToString)
                    BindToText(tbSpecification, Session("Result")(3).ToString)
                    'If ddlRefftype.SelectedValue <> "Other" Then
                    BindToText(tbQty, Session("Result")(4).ToString)
                    'End If
                    'tbQty.Text = FormatFloat(tbQty.Text, ViewState("DigitQty"))
                    GetInfo(tbProductCode.Text)
                End If
                If ViewState("Sender") = "btnGetDt" Then
                    Dim drResult As DataRow
                    For Each drResult In Session("Result").Rows
                        'insert
                        Dim dr As DataRow
                        dr = ViewState("Dt").NewRow
                        dr("Product") = drResult("Product_Code")
                        dr("Product_Name") = drResult("Product_Name")
                        dr("Specification") = TrimStr(drResult("Specification").ToString)
                        dr("Qty") = drResult("Qty")
                        dr("Remark") = drResult("RemarkDt")
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
        FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
        If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
            ddlCommand.Items.Add("Print")
            ddlCommand2.Items.Add("Print")
        End If
        lblTitle.Text = "Issue Slip"
        Me.tbQty.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbQty.Attributes.Add("OnBlur", "setformat('wrhs');")
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
        Return "SELECT * From V_STIssueSlipDt WHERE TransNmbr = " + QuotedStr(Nmbr)
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

    Protected Sub BtnGo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnGo.Click, btnGo2.Click
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

                Session("SelectCommand") = "EXEC S_STFormIssueSlip " + Result
                'lbStatus.Text = Session("SelectCommand")
                'Exit Sub
                Session("ReportFile") = ".../../../Rpt/FormIssueSlip.frx"
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
                        Result = ExecSPCommandGo(ActionValue, "S_STIssueSlip", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            'btnGetDt.Visible = State
            ddlwrhs.Enabled = State 'And tbReqNo.Text = ""
            tbSubLed.Enabled = State And tbFgSubLed.Text.Trim <> "N"
            btnSubLed.Visible = tbSubLed.Enabled
            btnRequestNo.Visible = State
            'ddlRefftype.Enabled = State
            tbDate.Enabled = State 'And tbReqNo.Text = ""
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
            'EnableHd(GetCountRecord(ViewState("Dt")) = 0)
            EnableHd(GetCountRecord(ViewState("Dt")) <> 0)
            StatusButtonSave(True)
            PnlInfo.Visible = False
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
                        lbStatus.Text = "Product " + tbProductName.Text + " has been already exist"
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
                dr("Product_Name") = tbProductName.Text
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
            PnlInfo.Visible = False
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

                tbReference.Text = GetAutoNmbr("SIS", "N", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), ddlwrhs.SelectedValue, ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO STCIssueSlipHd (TransNmbr, Status, TransDate, Warehouse, FgSubLed, SubLed, " + _
                "ReffType, ReffNmbr, Department, RequestBy, IssuedBy, Vehicle, Driver, " + _
                "Remark, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbReference.Text) + ", 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', " + _
                QuotedStr(ddlwrhs.SelectedValue) + ", " + QuotedStr(tbFgSubLed.Text) + ", " + QuotedStr(tbSubLed.Text) + ", " + _
                QuotedStr(tbReffType.Text) + ", " + QuotedStr(tbReqNo.Text) + " ," + QuotedStr(ddlDepartment.SelectedValue) + " , " + _
                QuotedStr(tbRequestBy.Text) + ", " + QuotedStr(tbIssuedBy.Text) + ", " + QuotedStr(tbVehicle.Text) + ", " + QuotedStr(tbDriver.Text) + ", " + QuotedStr(tbRemark.Text) + "," + QuotedStr(ViewState("UserId").ToString) + ", GetDate()"
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM STCIssueSlipHd WHERE TransNmbr = " + QuotedStr(tbReference.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE STCIssueSlipHd SET TransDate = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "' " + _
                ", Warehouse = " + QuotedStr(ddlwrhs.SelectedValue) + ", FgSubLed = " + QuotedStr(tbFgSubLed.Text) + _
                ", SubLed = " + QuotedStr(tbSubLed.Text) + ", ReffType = " + QuotedStr(tbReffType.Text) + " , ReffNmbr = " + QuotedStr(tbReqNo.Text) + _
                ", Department = " + QuotedStr(ddlDepartment.SelectedValue) + ", RequestBy = " + QuotedStr(tbRequestBy.Text) + ", IssuedBy = " + QuotedStr(tbIssuedBy.Text) + _
                ", Vehicle = " + QuotedStr(tbVehicle.Text) + ", Driver = " + QuotedStr(tbDriver.Text) + _
                ", Remark = " + QuotedStr(tbRemark.Text) + ", UserPrep = " + QuotedStr(ViewState("UserId").ToString) + ", DatePrep = GetDate()" + _
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
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, Product, Qty, Unit, Remark FROM STCIssueSlipDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
                    "UPDATE STCIssueSlipDt SET Product = @Product, Qty = @Qty, Unit = @Unit, Remark = @Remark " + _
                    " WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND Product = @OldProduct ", con)
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
                "DELETE FROM STCIssueSlipDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND Product = @Product ", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@Product", SqlDbType.VarChar, 20, "Product")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("STCIssueSlipDt")

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
            btnRequestNo.Visible = True
            ddlDepartment.Enabled = False
            tbRequestBy.Enabled = False
            'btnRequestNo.Visible = ddlRefftype.SelectedValue <> "Other"
            'ddlDepartment.Enabled = ddlRefftype.SelectedValue = "Other"
            'tbRequestBy.Enabled = ddlRefftype.SelectedValue = "Other"
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
            'ddlRefftype.SelectedValue = "Issue Request"
            tbIssuedBy.Text = ViewState("UserId")
            pnlDt.Visible = True
            BindDataDt("")
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbReference.Text = ""
            tbReqNo.Text = ""
            tbpurpose.Text = ""
            tbReffType.Text = "Issue Request"
            'tbFormula.Text = ""
            'ddlRefftype.SelectedIndex = 0
            ddlwrhs.SelectedIndex = 0
            'ddlPurpose.SelectedIndex = 0
            ddlwrhs.Enabled = True
            tbFgSubLed.Text = "N"
            tbSubLed.Text = ""
            tbSubLed.Enabled = tbFgSubLed.Text <> "N"
            btnSubLed.Visible = tbSubLed.Enabled
            tbSubLedName.Text = ""
            ddlDepartment.SelectedIndex = 0
            tbRequestBy.Text = ""
            tbIssuedBy.Text = ""
            tbVehicle.Text = ""
            tbVehicleName.Text = ""
            tbDriver.Text = ""
            tbRemark.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbProductCode.Text = ""
            tbProductName.Text = ""
            tbSpecification.Text = ""
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
            'If ddlRefftype.SelectedValue = "Other" Then
            'Session("Filter") = "select Product_Code, Product_Name, Specification, Product_Alias, Unit from VMsProduct "
            'ResultField = "Product_Code, Product_Name, Unit, Specification"
            'Else
            Session("Filter") = "SELECT Product_Code, Product_Name, Specification, Qty, Unit FROM V_STIssueSlipGetDataReff WHERE Request_No = " + QuotedStr(tbReqNo.Text)
            ResultField = "Product_Code, Product_Name, Unit, Specification, Qty"
            'End If
            ViewState("Sender") = "btnProduct"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Product Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnVehicle_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVehicle.Click
        Dim ResultField As String
        Try
            Session("Filter") = "SELECT Vehicle_Code, Vehicle_Name FROM VMsVehicle Where Fg_Active ='Y'"
            ResultField = "Vehicle_Code, Vehicle_Name"
            'End If
            ViewState("Sender") = "btnVehicle"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Product Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbProductCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbProductCode.TextChanged
        Dim Dr As DataRow
        Dim dt As DataTable
        Dim sqlString As String
        Try
            'If ddlRefftype.SelectedValue = "Other" Then
            ' sqlString = "select Product_Code, Product_Name, Specification, Unit from VMsProduct Where Product_Code = " + QuotedStr(tbProductCode.Text)
            'Else
            sqlString = "SELECT Product_Code, Product_Name, Specification, Qty, Unit FROM V_STIssueSlipGetDataReff WHERE Request_No = " + QuotedStr(tbReqNo.Text) + " and Product_Code = " + QuotedStr(tbProductCode.Text)
            'End If
            dt = SQLExecuteQuery(sqlString, ViewState("DBConnection").ToString).Tables(0)
            If dt.Rows.Count > 0 Then
                Dr = dt.Rows(0)
                tbProductCode.Text = Dr("Product_Code")
                tbProductName.Text = Dr("Product_Name")
                tbSpecification.Text = Dr("Specification")
                tbUnit.Text = Dr("Unit")
                tbQty.Text = Dr("Qty").ToString
                GetInfo(tbProductCode.Text)
            Else
                tbProductCode.Text = ""
                tbProductName.Text = ""
                tbSpecification.Text = ""
                tbQty.Text = "0"
                tbUnit.Text = ""
                PnlInfo.Visible = False
            End If
            AttachScript("setformatdt();", Page, Me.GetType())
            tbProductCode.Focus()
        Catch ex As Exception
            Throw New Exception("tb ProductCode Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub tbVehicle_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbVehicle.TextChanged
        Dim Dr As DataRow
        Dim dt As DataTable
        Dim sqlString As String
        Try
            sqlString = "SELECT Vehicle_Code, Vehicle_Name FROM VMsVehicle Where Fg_Active ='Y' AND Vehicle_Code = " + QuotedStr(tbVehicle.Text)
            dt = SQLExecuteQuery(sqlString, ViewState("DBConnection").ToString).Tables(0)
            If dt.Rows.Count > 0 Then
                Dr = dt.Rows(0)
                tbVehicle.Text = Dr("Vehicle_Code")
                tbVehicleName.Text = Dr("Vehicle_Name")
            Else
                tbVehicle.Text = ""
                tbVehicleName.Text = ""
            End If
            tbDriver.Focus()
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
            If (tbReqNo.Text = "") Then 'And (ddlRefftype.SelectedValue <> "Other") 
                lbStatus.Text = MessageDlg("Request No must have value")
                tbReqNo.Focus()
                Return False
            End If
            If (ddlDepartment.SelectedValue = "") Then 'And (ddlRefftype.SelectedValue = "Other") 
                lbStatus.Text = MessageDlg("Depertment must have value")
                ddlDepartment.Focus()
                Return False
            End If
            'If ddlPurpose.SelectedValue.Trim = "" Then
            'lbStatus.Text = MessageDlg("Purpose Budget must have value")
            'ddlPurpose.Focus()
            'Return False
            'End If
            If (tbRequestBy.Text.Trim = "") Then 'And (ddlRefftype.SelectedValue = "Other") 
                lbStatus.Text = MessageDlg("Request By must have value")
                tbRequestBy.Focus()
                Return False
            End If
            If (Len(tbRemark.Text) > 255) Then
                lbStatus.Text = MessageDlg("Remark lenght has been greater than 255")
                tbRemark.Focus()
                Return False
            End If
            If (tbRemark.Text.Trim = "") Then
                lbStatus.Text = MessageDlg("Remark must have value")
                tbRemark.Focus()
                Return False
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Ceh Hd Error : " + ex.ToString)
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
                If Dr("Remark").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Remark Must Have Value")
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
                If tbRemarkDt.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Remark Must Have Value")
                    tbRemarkDt.Focus()
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
            FilterName = "Reference, Date, Request Type, Status, Wrhs Issue, Issue Subled, Request No, Department, Request By, Issued By, Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), ReffType, Status, Wrhs_Name, Subled_Name, ReffNmbr, Department_Name, RequestBy, IssuedBy, Remark"
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
        Dim cekMenu As String
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
                        cekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If cekMenu <> "" Then
                            lbStatus.Text = cekMenu
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
                        cekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        If cekMenu <> "" Then
                            lbStatus.Text = cekMenu
                            Exit Sub
                        End If
                        Session("SelectCommand") = "EXEC S_STFormIssueSlip ''" + QuotedStr(GVR.Cells(2).Text) + "''" ' + "," + QuotedStr(GVR.Cells(3).Text)
                        Session("ReportFile") = ".../../../Rpt/FormIssueSlip.frx"
                        Session("DBConnection") = ViewState("DBConnection")
                        AttachScript("openprintdlg();", Page, Me.GetType)
                    Catch ex As Exception
                        lbStatus.Text = "btn print Error = " + ex.ToString
                    End Try
                ElseIf DDL.SelectedValue = "Input Lot No" Then
                    'Dim i As Integer
                    Dim paramgo As String

                    Try
                        paramgo = "WIS|" + GVR.Cells(2).Text
                        'lbStatus.Text = paramgo
                        'Exit Sub
                        Dim SQLString As String
                        SQLString = "Select Product from V_STStockLotReff WHERE TransNmbr = " + QuotedStr(GVR.Cells(2).Text) + " and Type = 'WIS' "
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
        EnableHd(Not DtExist())
        'EnableHd(GetCountRecord(ViewState("Dt")) <> 0)
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            ViewState("DtValue") = GVR.Cells(1).Text
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
            BindToDropList(ddlDepartment, Dt.Rows(0)("Department").ToString)
            BindToText(tbReffType, Dt.Rows(0)("ReffType").ToString)
            BindToText(tbReqNo, Dt.Rows(0)("ReffNmbr").ToString)
            BindToText(tbFgSubLed, Dt.Rows(0)("FgSubLed").ToString)
            BindToText(tbSubLed, Dt.Rows(0)("SubLed").ToString)
            BindToText(tbSubLedName, Dt.Rows(0)("SubLed_Name").ToString)
            BindToText(tbIssuedBy, Dt.Rows(0)("IssuedBy").ToString)
            BindToText(tbRequestBy, Dt.Rows(0)("RequestBy").ToString)
            BindToText(tbVehicle, Dt.Rows(0)("VehicleNo").ToString)
            BindToText(tbVehicleName, Dt.Rows(0)("VehicleName").ToString)
            BindToText(tbDriver, Dt.Rows(0)("Driver").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
            BindToText(tbpurpose, Dt.Rows(0)("Purpose").ToString)
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
                GetInfo(tbProductCode.Text)
                BindToText(tbProductName, Dr(0)("Product_Name").ToString)
                BindToText(tbSpecification, Dr(0)("Specification").ToString)
                BindToText(tbQty, Dr(0)("Qty").ToString)
                BindToText(tbUnit, Dr(0)("Unit").ToString)
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
        Dim ResultField, ResultSame, Filter As String
        'Dim CriteriaField As String
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            Session("Result") = Nothing
            Filter = ""
            'If ddlRefftype.SelectedValue = "Other" Then
            'Session("Filter") = "select Product_Code, Product_Name, Specification,  Unit from VMsProduct "
            'ResultField = "Product_Code, Product_Name, Unit, Specification"
            'Else
            Session("Filter") = "SELECT Request_No, Product_Code, Product_Name, Specification, Qty, Unit, remarkDt FROM V_STIssueSlipGetDataReff WHERE Request_No = " + QuotedStr(tbReqNo.Text)
            ResultField = "Product_Code, Product_Name, Unit, Qty, Purpose, Purpose_Name, Specification, RemarkDt, Request_No"
            'End If
            Session("ClickSame") = "Request_No"
            Session("Column") = ResultField.Split(",")
            'Session("CriteriaField") = CriteriaField.Split(",")
            ResultSame = "Request_No"
            Session("ResultSame") = ResultSame.Split(",")
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
                tbFgSubLed.Text = "N"
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

    'Protected Sub ddlRefftype_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlRefftype.SelectedIndexChanged
    '    btnRequestNo.Visible = ddlRefftype.SelectedValue <> "Other"
    '    btnGetDt.Visible = ddlRefftype.SelectedValue = "Other"
    '    ddlDepartment.Enabled = ddlRefftype.SelectedValue = "Other"
    '    tbRequestBy.Enabled = ddlRefftype.SelectedValue = "Other"
    '    If ddlRefftype.SelectedValue <> "Other" Then
    '        tbReqNo.Text = ""
    '        ddlDepartment.SelectedIndex = 0
    '        tbRequestBy.Text = ""
    '    End If
    'End Sub

    Protected Sub btnRequestNo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRequestNo.Click

        Dim ResultField, CriteriaField As String 'ResultSame 
        Try
            If ddlwrhs.SelectedValue.Trim = "" Then
                lbStatus.Text = MessageDlg("Warehouse must have value")
                ddlwrhs.Focus()
                Exit Sub
            End If
            If tbSubLed.Text.Trim = "" And tbFgSubLed.Text.Trim <> "N" Then
                lbStatus.Text = MessageDlg("SubLed must have value")
                tbSubLed.Focus()
                Exit Sub
            End If
            Session("Result") = Nothing
            'If ddlRefftype.SelectedValue = "Issue Request" Then
            Session("Filter") = "SELECT distinct Request_Type, Request_No, Request_Date, Warehouse, Department_Code, Department_Name, RequestBy, Remark, PurposeCode, Purpose FROM V_STIssueSlipGetDataReff WHERE Request_Type IS NOT NULL " 'AND Warehouse = " + QuotedStr(ddlwrhs.SelectedValue)
            'Else
            'Session("Filter") = "SELECT distinct Request_No, Request_Date, Department_Code, Department_Name, RequestBy, RemarkDt FROM V_STIssueSlipGetDataReff WHERE Request_Type = " + QuotedStr(ddlRefftype.SelectedValue)
            'End If
            ResultField = "Request_Type, Request_No, Department_Code, RequestBy,  Remark, PurposeCode, Purpose"
            CriteriaField = "Request_Type, Request_No, Department_Code, RequestBy,  Remark, PurposeCode, Purpose"
            Session("Column") = ResultField.Split(",")
            ViewState("Sender") = "btnRequestNo"
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn get Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbCount_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCount.Click
        Dim ResultField, CriteriaField, ResultSame As String
        Try
            Session("filter") = "Select A.* from V_STIssueSlipGetDataReff A INNER JOIN VMsWrhsUser W ON A.Warehouse = W.Wrhs_Code WHERE W.UserId = " + QuotedStr(ViewState("UserId"))
            'Session("filter") = "SELECT * FROM V_STIssueSlipGetDataReff"
            'ViewState("CheckDlg") = False
            ResultField = " Request_No, Request_Date, Department_Code, Department_Name, RequestBy, Warehouse, Remark, Request_Type, Product_Code, Product_Name, Specification, Qty, Unit, RemarkDt, PurposeCode, Purpose"
            CriteriaField = "Request_No, Request_Date, Department_Code, Department_Name, RequestBy, Warehouse, Remark, Request_Type, Product_Code, Product_Name, Specification, Qty, Unit, RemarkDt, PurposeCode, Purpose"
            ViewState("CriteriaField") = CriteriaField.Split(",")
            Session("Column") = ResultField.Split(",")
            ResultSame = "Request_No"
            Session("ResultSame") = ResultSame.Split(",")
            ViewState("Sender") = "btnOut"
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lbCount_Click Error : " + ex.ToString
        End Try
        
    End Sub

    Protected Sub tbReqNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbReqNo.TextChanged
        Dim Dr As DataRow
        Dim dt As DataTable
        Dim sqlString As String
        Try
            'If ddlRefftype.SelectedValue = "Issue Request" Then
            sqlString = "SELECT Request_No, Product_Code, Product_Name, Specification, Qty, Unit, Purpose, Purpose_Name, RemarkDt FROM V_STIssueSlipGetDataReff WHERE Request_Type = 'Issue Request' And Request_No = " + QuotedStr(tbReqNo.Text)
            'Else
            'sqlString = "SELECT Request_No, Product_Code, Product_Name, Specification, Qty, Unit, Purpose, Purpose_Name, RemarkDt FROM V_STIssueSlipGetDataReff WHERE Request_Type = " + QuotedStr(ddlRefftype.SelectedValue) + " And Request_No = " + QuotedStr(tbReqNo.Text) + " and Product_Code = " + QuotedStr(tbProductCode.Text)
            'End If
            dt = SQLExecuteQuery(sqlString, ViewState("DBConnection").ToString).Tables(0)
            If dt.Rows.Count > 0 Then
                Dr = dt.Rows(0)
                tbProductCode.Text = Dr("Product_Code")
                tbProductName.Text = Dr("Product_Name")
                tbSpecification.Text = Dr("Specification")
                tbUnit.Text = Dr("Unit")
                'If ddlRefftype.SelectedValue = "Other" Then
                'tbQty.Text = "0"
                'tbRemarkDt.Text = ""
                'Else
                tbQty.Text = Dr("Qty").ToString
                tbRemarkDt.Text = Dr("RemarkDt")
                'End If
            Else
                tbProductCode.Text = ""
                tbProductName.Text = ""
                tbSpecification.Text = ""
                tbQty.Text = "0"
                tbUnit.Text = ""
            End If
            AttachScript("setformatdt();", Page, Me.GetType())
            tbProductCode.Focus()
        Catch ex As Exception
            Throw New Exception("tb ProductCode Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub GetInfo(ByVal Product As String)
        Dim SqlString As String
        Dim DS As DataSet
        Try
            SqlString = "EXEC S_GetInfoStock " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + _
            ", " + QuotedStr(ddlwrhs.SelectedValue) + ", " + QuotedStr(tbSubLed.Text) + ", " + QuotedStr(Product)

            DS = SQLExecuteQuery(SqlString, ViewState("DBConnection"))

            GridInfo.DataSource = DS.Tables(0)
            GridInfo.DataBind()
            PnlInfo.Visible = True
            lbInfo.Visible = DS.Tables(0).Rows.Count > 0
        Catch ex As Exception
            Throw New Exception("get info error : " + ex.ToString)
        End Try
    End Sub

    Dim QtyOnHand As Decimal = 0
    Protected Sub GridInfo_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridInfo.RowDataBound
        Try
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "Qty")) Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                    ' add the UnitPrice and QuantityTotal to the running total variables
                    QtyOnHand += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Qty"))
                ElseIf e.Row.RowType = DataControlRowType.Footer Then
                    e.Row.Cells(0).Text = "Total On Hand :"
                    ' for the Footer, display the running totals
                    e.Row.Cells(1).Text = FormatFloat(QtyOnHand, ViewState("DigitQty"))
                End If
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Info Row Data Bound Error : " + ex.ToString
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

            If ViewState("DtSub") Is Nothing Then
                piar = False
            Else
                piar = ViewState("DtSub").Rows.Count > 0
            End If

            Return (dete Or piar)

            'EnableHd(GetCountRecord(ViewState("Dt")) = 0 And ViewState("DtSub").Rows.Count = 0 And ViewState("DtPart").Rows.Count = 0)
        Catch ex As Exception
            Throw New Exception("Cek Data Hd Error : " + ex.ToString)
        End Try
    End Function
End Class
