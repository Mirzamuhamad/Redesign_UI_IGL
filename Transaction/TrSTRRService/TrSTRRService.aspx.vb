Imports System.Data
Imports System.Data.SqlClient
'Imports System.Data.Odbc
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO
'Imports CrystalDecisions.ReportAppServer.ClientDoc

Partial Class Transaction_TrSTRRService_TrSTRRService
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select * From V_STRRPOHd WHERE RRType = 'RS' "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Dim CurrFilter, Value As String
        Try
            If Not IsPostBack Then
                InitProperty()
                ViewState("SetLocation") = False
                FillCombo(ddlwrhs, "EXEC S_GetWrhsUserRR " + QuotedStr(ViewState("UserId")), True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection"))
                SetInit()
                Session("AdvanceFilter") = ""
                lbCount.Text = SQLExecuteScalar("SELECT Count(DISTINCT PO_No) FROM V_PRPOPendingUser Where UPPER(POType) = 'SERVICE' ", ViewState("DBConnection").ToString)
                'lbStatus.Text = "SELECT Count(DISTINCT PO_No) FROM V_PRPOPendingUser Where POType = 'PO' AND FgQC = 'N' AND UserId = " + QuotedStr(ViewState("UserId").ToString)
                'Exit Sub
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
                If ViewState("Sender") = "btnPONo" Then
                    If tbSuppCode.Text <> "" Then
                        tbPONo.Text = Session("Result")(0).ToString
                        tbFgReport.Text = Session("Result")(1).ToString
                    Else
                        tbPONo.Text = Session("Result")(0).ToString
                        tbSuppCode.Text = Session("Result")(1).ToString
                        tbSuppName.Text = Session("Result")(2).ToString
                        tbFgReport.Text = Session("Result")(3).ToString
                        tbCarNo.Enabled = True

                    End If
                End If
                If ViewState("Sender") = "btnSubLed" Then
                    tbSubLed.Text = Session("Result")(0).ToString
                    tbSubLedName.Text = Session("Result")(1).ToString
                End If
                If ViewState("Sender") = "btnSupplier" Then
                    tbSuppCode.Text = Session("Result")(0).ToString
                    tbSuppName.Text = Session("Result")(1).ToString
                    tbPONo.Text = ""

                End If
                If ViewState("Sender") = "btnProduct" Then
                    tbProductCode.Text = Session("Result")(0).ToString
                    tbProductName.Text = Session("Result")(1).ToString
                    BindToText(tbSpecification, TrimStr(Session("Result")(2).ToString))
                    BindToText(tbQtyOrder, FormatFloat(Session("Result")(3).ToString, ViewState("DigitQty")))
                    BindToText(tbUnitOrder, Session("Result")(4).ToString)
                    BindToText(tbQty, FormatFloat(Session("Result")(5).ToString, ViewState("DigitQty")))
                    BindToText(tbUnit, Session("Result")(6).ToString)
                    tbFgStock.Text = Session("Result")(9).ToString
                    'ddlLocation.Enabled = Session("Result")(9).ToString = "Y"
                    'If ddlLocation.Enabled = False Then
                    'ddlLocation.SelectedIndex = 0
                    'End If


                End If
                If ViewState("Sender") = "btnGetDt" Then
                    Dim drResult As DataRow
                    For Each drResult In Session("Result").Rows
                        'insert
                        Dim dr As DataRow
                        dr = ViewState("Dt").NewRow
                        dr("Product") = drResult("Product")
                        dr("Product_Name") = drResult("Product_Name")
                        dr("Specification") = TrimStr(drResult("Specification"))
                        dr("Location") = ""
                        If drResult("Qty") > 0 Then
                            'dr("QtyRR") = drResult("Qty")
                            dr("Qty") = drResult("Qty")
                        End If
                        dr("QtyPO") = drResult("Qty_PO")
                        If drResult("Qty_Order") > 0 Then
                            dr("QtyOrder") = drResult("Qty_Order")
                        End If
                        dr("UnitOrder") = drResult("Unit_Order")
                        dr("Unit") = drResult("Unit")
                        If tbPONo.Text <> "" Then
                            dr("Remark") = drResult("Remark")
                        End If
                        ViewState("Dt").Rows.Add(dr)
                    Next
                    BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                End If
                If ViewState("Sender") = "btnOut" Then
                    BtnAdd_Click(Nothing, Nothing)
                    Dim drResult As DataRow
                    Dim FirstTime As Boolean = True
                    For Each drResult In Session("Result").Rows
                        'insert
                        If FirstTime Then
                            BindToText(tbSuppCode, drResult("Supplier").ToString)
                            BindToText(tbSuppName, drResult("Supplier_Name").ToString)
                            BindToText(tbPONo, drResult("PO_No").ToString)
                            BindToText(tbFgReport, drResult("Report").ToString)
                        End If

                        If CekExistData(ViewState("Dt"), "Product, Location", drResult("Product") + "|") = False Then
                            Dim dr As DataRow
                            dr = ViewState("Dt").NewRow
                            dr("Product") = drResult("Product")
                            dr("Product_Name") = drResult("Product_Name")
                            dr("Specification") = TrimStr(drResult("Specification"))
                            dr("Location") = ""
                            If drResult("Qty") > 0 Then
                                dr("Qty") = drResult("Qty")
                            End If
                            dr("QtyPO") = drResult("Qty_PO")
                            If drResult("Qty_Order") > 0 Then
                                dr("QtyOrder") = drResult("Qty_Order")
                            End If
                            dr("UnitOrder") = drResult("Unit_Order")
                            dr("Unit") = drResult("Unit")
                            dr("Remark") = ""
                            ViewState("Dt").Rows.Add(dr)
                        End If
                        FirstTime = False
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
        Me.tbSpecification.Attributes.Add("ReadOnly", "True")
        Me.tbQty.Attributes.Add("ReadOnly", "True")
        'Me.tbQtyRR.Attributes.Add("ReadOnly", "True")
        'Me.tbQtyPO.Attributes.Add("ReadOnly", "True")
        Me.tbQtyOrder.Attributes.Add("OnKeyDown", "return PressNumeric();")
        'Me.tbQty.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbQtyOrder.Attributes.Add("OnBlur", "setformat();")
        'Me.tbQtyRR.Attributes.Add("OnBlur", "setformat();")
        Me.tbQty.Attributes.Add("OnBlur", "setformat();")
    End Sub

    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim StrFilter, GetString As String
        Try
            GetString = GetStringHd
            StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue, ddlRange.SelectedValue)
            'If AdvanceFilter.Length > 1 And StrFilter.Length > 1 Then
            '    StrFilter = StrFilter + " And " + AdvanceFilter
            'ElseIf AdvanceFilter.Length > 1 And StrFilter.Length <= 1 Then
            '    StrFilter = AdvanceFilter
            'End If
            If AdvanceFilter.Length > 1 Then
                StrFilter = AdvanceFilter
            End If
            ' "UserId = " + QuotedStr(ViewState("UserId").ToString)
            DT = BindDataTransaction(GetString, StrFilter, ViewState("DBConnection").ToString)
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
        Return "SELECT * From V_STRRPODt WHERE TransNmbr = " + QuotedStr(Nmbr)
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
                Dim QCNo As String

                Pertamax = True
                Result = ""
                QCNo = ""

                'For Each GVR In GridView1.Rows
                '    CB = GVR.FindControl("cbSelect")
                '    If CB.Checked Then
                '        If GVR.Cells(3).Text = "P" Then
                '            ListSelectNmbr = GVR.Cells(2).Text
                '            If Pertamax Then
                '                Result = "'''" + ListSelectNmbr + "''"
                '                QCNo = GVR.Cells(7).Text.Replace("&nbsp;", "")
                '                Pertamax = False
                '            Else
                '                Result = Result + ",''" + ListSelectNmbr + "''"
                '            End If
                '        End If
                '    End If
                'Next
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
                Session("DBConnection") = ViewState("DBConnection")
                Result = Result + "'"
                Session("SelectCommand") = "EXEC S_STRRServiceForm " + Result
                'lbStatus.Text = Session("SelectCommand")
                'Exit Sub
                Session("ReportFile") = ".../../../Rpt/STRRServiceForm.frx"

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
                        Result = ExecSPCommandGo(ActionValue, "S_STRRPO", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            tbSuppCode.Enabled = State
            ddlwrhs.Enabled = ddlwrhs.SelectedValue = "" Or GetCountRecord(ViewState("Dt"), "Location") = 0 'State ' And tbPONo.Text = ""
            tbSubLed.Enabled = State And tbFgSubLed.Text.Trim <> "N"
            btnSubLed.Visible = tbSubLed.Enabled
            btnSupp.Visible = State
            'btnQCNo.Visible = State
            btnPONo.Visible = State
            tbDate.Enabled = State
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
        Dim Result, SQLString As String
        Try
            SQLString = "Declare @A VarChar(255) EXEC S_STRRPOCekPOQty '" + tbPONo.Text + "', '" + tbProductCode.Text + "', " + tbQty.Text.Replace(",", "") + ", @A Out SELECT @A"
            Result = SQLExecuteScalar(SQLString, ViewState("DBConnection"))
            If Result.Length > 5 Then
                lbStatus.Text = Result
                Exit Sub
            End If
            If ViewState("StateDt") = "Edit" Then
                If ViewState("DtValue") <> tbProductCode.Text + "|" + TrimStr(ddlLocation.SelectedValue) Then
                    If CekExistData(ViewState("Dt"), "Product, Location", tbProductCode.Text + "|" + TrimStr(ddlLocation.SelectedValue)) Then
                        lbStatus.Text = "Product " + tbProductName.Text + " Location '" + ddlLocation.SelectedItem.Text + "' has been already exist"
                        Exit Sub
                    End If
                End If
                Row = ViewState("Dt").Select("Product+'|'+Location = " + QuotedStr(ViewState("DtValue")))(0)
                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                Row("Product") = tbProductCode.Text
                Row("Product_Name") = tbProductName.Text
                Row("Specification") = tbSpecification.Text
                If ddlLocation.SelectedValue <> "" Then
                    Row("Location") = ddlLocation.SelectedValue
                    Row("Location_Name") = ddlLocation.SelectedItem.Text
                Else
                    Row("Location") = ""
                    Row("Location_Name") = ""
                End If

                Row("QtyOrder") = tbQtyOrder.Text
                Row("Qty") = tbQty.Text
                Row("UnitOrder") = tbUnitOrder.Text
                Row("Unit") = tbUnit.Text
                Row("Remark") = tbRemarkDt.Text
                Row("Fg_Stock") = tbFgStock.Text
                Row.EndEdit()
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If

                If CekExistData(ViewState("Dt"), "Product,Location", tbProductCode.Text + "|" + TrimStr(ddlLocation.SelectedValue)) = True Then
                    lbStatus.Text = "Product " + tbProductName.Text + " Location " + ddlLocation.SelectedItem.Text + " has been already exist"
                    Exit Sub
                End If

                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("Product") = tbProductCode.Text
                dr("Product_Name") = tbProductName.Text
                dr("Specification") = tbSpecification.Text
                If ddlLocation.SelectedValue <> "" Then
                    dr("Location") = ddlLocation.SelectedValue
                    dr("Location_Name") = ddlLocation.SelectedItem.Text
                Else
                    dr("Location") = ""
                    dr("Location_Name") = ""
                End If
                dr("QtyOrder") = tbQtyOrder.Text
                dr("Qty") = tbQty.Text
                dr("UnitOrder") = tbUnitOrder.Text
                dr("Unit") = tbUnit.Text
                dr("Remark") = tbRemarkDt.Text
                dr("Fg_Stock") = tbFgStock.Text
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
                tbReference.Text = GetAutoNmbr("RRSV", tbFgReport.Text, Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
                SQLString = "INSERT INTO STCRRPOHd (TransNmbr, Status, TransDate, FgReport, Supplier, PONO, Warehouse, FgSubLed, SubLed, " + _
                "ReceivedBy, SJSuppNo, SJSuppDate, CarNo, Expedition,ContainerNo," + _
                "Remark, RRType, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbReference.Text) + ", 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', " + _
                QuotedStr(tbFgReport.Text) + ", " + QuotedStr(tbSuppCode.Text) + "," + QuotedStr(tbPONo.Text) + ", " + _
                QuotedStr(ddlwrhs.SelectedValue) + ", " + QuotedStr(tbFgSubLed.Text) + ", " + QuotedStr(tbSubLed.Text) + ", " + _
                QuotedStr(tbReceivedBy.Text) + ", " + QuotedStr(tbSJSuppNo.Text) + ", '" + Format(tbSJSuppDate.SelectedValue, "yyyy-MM-dd") + "', " + _
                QuotedStr(tbCarNo.Text) + "," + QuotedStr(tbExpedition.Text) + "," + QuotedStr(tbContainer.Text) + "," + QuotedStr(tbRemark.Text) + ",'RS'," + QuotedStr(ViewState("UserId").ToString) + ", getDate()"
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM STCRRPOHd WHERE TransNmbr = " + QuotedStr(tbReference.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE STCRRPOHd SET TransDate = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "' " + _
                ", FgReport = " + QuotedStr(tbFgReport.Text) + ", Supplier = " + QuotedStr(tbSuppCode.Text) + ",PONo = " + QuotedStr(tbPONo.Text) + _
                ", Warehouse = " + QuotedStr(ddlwrhs.SelectedValue) + ", FgSubLed = " + QuotedStr(tbFgSubLed.Text) + ", Expedition = " + QuotedStr(tbExpedition.Text) + " , ContainerNo = " + QuotedStr(tbContainer.Text) + _
                ", SubLed = " + QuotedStr(tbSubLed.Text) + ", ReceivedBy = " + QuotedStr(tbReceivedBy.Text) + ", SJSuppNo = " + QuotedStr(tbSJSuppNo.Text) + ", SJSuppDate = '" + Format(tbSJSuppDate.SelectedValue, "yyyy-MM-dd") + _
                " ', CarNo = " + QuotedStr(tbCarNo.Text) + " , Remark = " + QuotedStr(tbRemark.Text) + ", UserPrep = " + QuotedStr(ViewState("UserId").ToString) + ", DatePrep = getDate()" + _
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
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, Product, Location, QtyPO, QtyOrder, UnitOrder, Qty, Unit, Remark FROM STCRRPODt WHERE TransNmbr = '" & ViewState("TransNmbr") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt As New DataTable("STCRRPODt")

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
            tbSJSuppDate.SelectedDate = ViewState("ServerDate") 'Today
            tbReceivedBy.Text = ViewState("UserId")
            pnlDt.Visible = True
            BindDataDt("")
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbReference.Text = ""
            tbPONo.Text = ""
            'tbQCNo.Text = ""
            tbFgReport.Text = ""
            ddlwrhs.SelectedIndex = 0
            'ddlProductType.SelectedIndex = 0
            ddlwrhs.Enabled = True
            tbFgSubLed.Text = "N"
            tbSubLed.Text = ""
            tbSubLed.Enabled = tbFgSubLed.Text <> "N"
            btnSubLed.Visible = tbSubLed.Enabled
            tbSubLedName.Text = ""
            tbReceivedBy.Text = ""
            tbExpedition.Text = ""
            tbContainer.Text = ""
            tbCarNo.Text = ""
            tbSuppCode.Text = ""
            tbSuppName.Text = ""
            tbSJSuppNo.Text = ""
            tbCarNo.Text = ""
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
            'tbQtyPO.Text = "0"
            tbQtyOrder.Text = "0"
            'tbQtyRR.Text = "0"
            tbQty.Text = "0"
            tbUnitOrder.Text = ""
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
            Session("Filter") = "SELECT Product, Product_Name, Specification, Qty_PO, Qty_Order, Unit_Order, Qty, Unit, Remark, Fg_Stock FROM V_PRPOPending WHERE UPPER(POType) = 'SERVICE' AND PO_No = " + QuotedStr(tbPONo.Text)
            ResultField = "Product, Product_Name, Specification, Qty_Order, Unit_Order, Qty, Unit, Remark, Qty_PO,Fg_Stock"
            ViewState("Sender") = "btnProduct"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Product Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbProductCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbProductCode.TextChanged
        Dim Dr As DataRow
        Dim DT As DataTable
        Dim SQLString As String
        Try
            SQLString = "SELECT Product AS Product_Code, Product_Name, Specification, Qty_PO, Qty_Order, Unit_Order, Qty, Unit, Fg_Stock FROM V_PRPOPending WHERE UPPER(POType) = 'SERVICE' AND PO_No = " + QuotedStr(tbPONo.Text) + " And Product = " + QuotedStr(tbProductCode.Text)
            DT = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If DT.Rows.Count > 0 Then
                Dr = DT.Rows(0)
            Else
                Dr = Nothing
            End If

            If Not Dr Is Nothing Then
                BindToText(tbProductCode, Dr("Product_Code").ToString)
                BindToText(tbProductName, Dr("Product_Name").ToString)
                BindToText(tbSpecification, Dr("Specification").ToString)
                'BindToText(tbQtyPO, Dr("Qty_PO").ToString)
                BindToText(tbQtyOrder, Dr("Qty_Order").ToString)
                BindToText(tbUnitOrder, Dr("Unit_Order").ToString)
                'If tbQCNo.Text = "" Then
                'BindToText(tbQtyRR, Dr("Qty").ToString)
                BindToText(tbQty, Dr("Qty").ToString)
                'Else
                'BindToText(tbQtyRR, Dr("Qty_RR").ToString)
                'BindToText(tbQty, Dr("Qty_Netto").ToString)
                'End If

                BindToText(tbUnit, Dr("Unit").ToString)
                'tbQtyPO.Text = FormatFloat(tbQtyPO.Text, ViewState("DigitQty"))
                tbQtyOrder.Text = FormatFloat(tbQtyOrder.Text, ViewState("DigitQty"))
                tbQty.Text = FormatFloat(tbQty.Text, ViewState("DigitQty"))
                'tbQtyRR.Text = FormatFloat(tbQtyRR.Text, ViewState("DigitQty"))
                tbFgStock.Text = Dr("Fg_Stock").ToString

                'ddlLocation.Enabled = Dr("Fg_Stock").ToString = "Y"
                'If ddlLocation.Enabled = False Then
                'ddlLocation.SelectedIndex = 0
                'End If



            Else
                tbProductCode.Text = ""
                tbProductName.Text = ""
                tbSpecification.Text = ""
                'tbQtyPO.Text = ""
                tbQtyOrder.Text = ""
                tbUnitOrder.Text = ""
                tbQty.Text = ""
                'tbQtyRR.Text = ""
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
        If ViewState("SetLocation") Then
            FillCombo(ddlLocation, "EXEC S_GetWrhsLocation " + QuotedStr(ddlwrhs.SelectedValue), True, "Location_Code", "Location_Name", ViewState("DBConnection"))
            ViewState("SetLocation") = False
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
            'If CInt(ViewState("GLYear")) <> Year(tbDate.SelectedValue) Or CInt(ViewState("GLPeriod")) <> Month(tbDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(ViewState("GLPeriod")) + " " + ViewState("GLYear").ToString.Trim)
            '    Exit Function
            'End If
            If tbPONo.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("PO No must have value")
                btnPONo.Focus()
                Return False
            End If
            If ddlwrhs.SelectedValue.Trim = "" Then
                lbStatus.Text = MessageDlg("Warehouse Receive must have value")
                ddlwrhs.Focus()
                Return False
            End If
            If tbSubLed.Text.Trim = "" And tbFgSubLed.Text.Trim <> "N" Then
                lbStatus.Text = MessageDlg("SubLed must have value")
                tbSubLed.Focus()
                Return False
            End If
            If tbSJSuppNo.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("SJ Supp No must have value")
                tbSJSuppNo.Focus()
                Return False
            End If
            If tbReceivedBy.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Received By must have value")
                tbReceivedBy.Focus()
                Return False
            End If
            'If ddlMethod.Text.Trim = "" Then
            '    lbStatus.Text = MessageDlg("Method Bongkar Ikat must have value")
            '    ddlMethod.Focus()
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
                'If ddlLocation.SelectedValue = "" And tbFgStock.Text = "Y" Then
                If Dr("Location").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Location Must Have Value")
                    Return False
                End If
                'End If
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

                'If ddlLocation.Enabled = True Then
                If ddlLocation.SelectedValue.Trim = "" Then
                    lbStatus.Text = MessageDlg("Location Must Have Value")
                    ddlLocation.Focus()
                    Return False
                End If
                'End If

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
            FilterName = "Reference, Date, Status, Supplier, Supplier Name, QCNo, PONo, Product Type, Warehouse Name, Received By, Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), Status, Supplier, Supplier_Name, QC No, PONo, Product_Type_Name, Wrhs_Name, ReceivedBy, Remark"
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
                        ViewState("SetLocation") = True
                        MovePanel(PnlHd, pnlInput)
                        ViewState("TransNmbr") = GVR.Cells(2).Text
                        GridDt.PageIndex = 0
                        BindDataDt(ViewState("TransNmbr"))
                        FillTextBoxHd(ViewState("TransNmbr"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, pnlDt, GridDt)
                        btnHome.Visible = False
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
                    Dim QCNo, StatusPrint As String
                    Try
                        If GVR.Cells(3).Text = "P" Then
                            QCNo = GVR.Cells(7).Text.Replace("&nbsp;", "")
                            StatusPrint = GVR.Cells(3).Text.Replace("&nbsp;", "")
                            Session("SelectCommand") = "EXEC S_STRRServiceForm ''" + QuotedStr(GVR.Cells(2).Text) + "''"
                            'Response.Write("<script type='text/javascript'>alert('" & Session("SelectCommand") & "');</script>")
                            'lbStatus.Text = Session("SelectCommand")
                            'Exit Sub
                            Session("ReportFile") = ".../../../Rpt/STRRServiceForm.frx"
                            Session("DBConnection") = ViewState("DBConnection")
                            AttachScript("openprintdlg();", Page, Me.GetType)
                        Else
                            lbStatus.Text = "Cannot print or preview, status RR No " + QuotedStr(GVR.Cells(2).Text) + " must be posted"
                            Exit Sub
                        End If
                    Catch ex As Exception
                        lbStatus.Text = "btn print Error = " + ex.ToString
                    End Try
                ElseIf DDL.SelectedValue = "Input Lot No" Then
                    'Dim i As Integer
                    Dim paramgo As String

                    Try
                        paramgo = "RR PO|" + GVR.Cells(2).Text
                        'lbStatus.Text = paramgo
                        'Exit Sub
                        Dim SQLString As String
                        SQLString = "Select Product from V_STStockLotReff WHERE TransNmbr = " + QuotedStr(GVR.Cells(2).Text) + " and Type = 'RR PO' "
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
        Dim lb As Label
        Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
        lb = GVR.FindControl("lbLocation")
        dr = ViewState("Dt").Select("Product+'|'+Location = " + QuotedStr(GVR.Cells(1).Text + "|" + TrimStr(lb.Text)))
        dr(0).Delete()
        EnableHd(GetCountRecord(ViewState("Dt")) = 0)
        BindGridDt(ViewState("Dt"), GridDt)
        'EnableHd(GetCountRecord(ViewState("Dt")) <> 0)
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Dim lb As Label
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            lb = GVR.FindControl("lbLocation")
            ViewState("DtValue") = GVR.Cells(1).Text + "|" + TrimStr(lb.Text)
            If ViewState("SetLocation") Then
                FillCombo(ddlLocation, "EXEC S_GetWrhsLocation " + QuotedStr(ddlwrhs.SelectedValue), True, "Location_Code", "Location_Name", ViewState("DBConnection"))
                ViewState("SetLocation") = False
            End If
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
            BindToText(tbFgReport, Dt.Rows(0)("Report").ToString)
            'BindToText(tbQCNo, Dt.Rows(0)("QCNo").ToString)
            BindToText(tbPONo, Dt.Rows(0)("PONo").ToString)
            BindToText(tbFgReport, Dt.Rows(0)("Report").ToString)
            BindToText(tbSuppCode, Dt.Rows(0)("Supplier").ToString)
            BindToText(tbSuppName, Dt.Rows(0)("Supplier_Name").ToString)
            BindToText(tbSJSuppNo, Dt.Rows(0)("SJSuppNo").ToString)
            BindToDate(tbSJSuppDate, Dt.Rows(0)("SJSuppDate").ToString)
            BindToText(tbCarNo, Dt.Rows(0)("CarNo").ToString)
            BindToDropList(ddlwrhs, Dt.Rows(0)("Warehouse").ToString)
            'BindToDropList(ddlProductType, Dt.Rows(0)("ProductType").ToString)
            'BindToDropList(ddlTeknik, Dt.Rows(0)("FgNeedKuli").ToString)
            'BindToDropList(ddlMethod, Dt.Rows(0)("BongkarType").ToString)
            BindToText(tbFgSubLed, Dt.Rows(0)("FgSubLed").ToString)
            BindToText(tbSubLed, Dt.Rows(0)("SubLed").ToString)
            BindToText(tbSubLedName, Dt.Rows(0)("SubLed_Name").ToString)
            BindToText(tbReceivedBy, Dt.Rows(0)("ReceivedBy").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal Product As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("Product+'|'+Location = " + QuotedStr(Product))
            If Dr.Length > 0 Then
                BindToText(tbProductCode, Dr(0)("Product").ToString)
                BindToText(tbProductName, Dr(0)("Product_Name").ToString)
                BindToText(tbSpecification, Dr(0)("Specification").ToString)
                BindToDropList(ddlLocation, Dr(0)("Location").ToString)
                'ddlLocation.Enabled = (Dr(0)("Fg_Stock").ToString = "Y")
                BindToText(tbFgStock, Dr(0)("Fg_Stock").ToString)
                'BindToText(tbQtyPO, Dr(0)("QtyPO").ToString)
                BindToText(tbQtyOrder, Dr(0)("QtyOrder").ToString)
                BindToText(tbUnitOrder, Dr(0)("UnitOrder").ToString)
                BindToText(tbQty, Dr(0)("Qty").ToString)
                'BindToText(tbQtyRR, Dr(0)("QtyRR").ToString)
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

            Dr = FindMaster("SubLed", tbFgSubLed.Text + "|" + tbSubLed.Text, ViewState("DBConnection").ToString)

            'lbStatus.Text = tbFgSubLed.Text + "|" + tbSubLed.Text
            'Exit Sub

            If Not Dr Is Nothing Then
                tbSubLed.Text = Dr("SubLed_No")
                tbSubLedName.Text = Dr("SubLed_Name")
            Else
                tbSubLed.Text = ""
                tbSubLedName.Text = ""
            End If
            AttachScript("setformatDT();", Page, Me.GetType())
            tbSubLed.Focus()
        Catch ex As Exception
            Throw New Exception("tb CustCode Error : " + ex.ToString)
        End Try
    End Sub

    'Protected Sub btnGetDt_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnGetDt.Click
    '    Dim ResultField As String 'ResultSame 
    '    Try
    '        If CekHd() = False Then
    '            Exit Sub
    '        End If
    '        'If tbQCNo.Text <> "" Then
    '        '    Dim dataQC As DataTable
    '        '    dataQC = SQLExecuteQuery("SELECT DISTINCT Product_Code, Product_Name, Specification, Qty_RR, Qty_Netto, Unit, Qty_PO, Qty_Order, Unit_Order FROM V_QCRRPendingDt WHERE QC_No = " + QuotedStr(tbQCNo.Text), ViewState("DBConnection")).Tables(0)
    '        '    For Each drResult In dataQC.Rows
    '        '        Dim dr As DataRow
    '        '        dr = ViewState("Dt").NewRow
    '        '        dr("Product") = drResult("Product_Code")
    '        '        dr("Product_Name") = drResult("Product_Name")
    '        '        dr("Specification") = RTrim(drResult("Specification").ToString)
    '        '        dr("Location") = ""
    '        '        dr("QtyRR") = FormatFloat(drResult("Qty_RR"), ViewState("DigitQty"))
    '        '        dr("Qty") = FormatFloat(drResult("Qty_Netto"), ViewState("DigitQty"))
    '        '        dr("Unit") = drResult("Unit")
    '        '        dr("QtyPO") = FormatFloat(drResult("Qty_PO"), ViewState("DigitQty"))
    '        '        dr("QtyOrder") = FormatFloat(drResult("Qty_Order"), ViewState("DigitQty"))
    '        '        dr("UnitOrder") = drResult("Unit_Order")
    '        '        ViewState("Dt").Rows.Add(dr)
    '        '    Next
    '        '    BindGridDt(ViewState("Dt"), GridDt)
    '        '    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
    '        'Else
    '        Session("Result") = Nothing
    '        '        Session("Filter") = "SELECT DISTINCT Product, Product_Name, Specification, Qty_PO, Qty_Order, Unit_Order, Qty, Unit, Remark FROM V_PRPOPending WHERE POType = 'PO' AND PO_No = " + QuotedStr(tbPONo.Text)
    '        ResultField = "Product, Product_Name, Specification, Unit, Qty_PO, Qty, Qty_Order, Unit_Order, Remark"
    '        Session("Column") = ResultField.Split(",")
    '        ViewState("Sender") = "btnGetDt"
    '        AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
    '        'End If
    '    Catch ex As Exception
    '        lbStatus.Text = "btn get Dt Error : " + ex.ToString
    '    End Try
    'End Sub

    Protected Sub ddlwrhs_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlwrhs.SelectedIndexChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("WrhsUser", ddlwrhs.SelectedValue + "|" + ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbFgSubLed.Text = Dr("FgSubLed")
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

    Protected Sub btnPONo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPONo.Click
        Dim ResultField As String
        Try
            If tbSuppCode.Text <> "" Then
                If ViewState("StateHd") = "Insert" Then
                    Session("filter") = "SELECT DISTINCT PO_No, PO_Date, Report, Supplier, Supplier_Name, Attn, Delivery, Delivery_Name FROM V_PRPOPendingUser Where UPPER(POType) = 'SERVICE' AND Supplier = " + QuotedStr(tbSuppCode.Text)
                Else
                    Session("filter") = "SELECT DISTINCT PO_No, PO_Date, Report, Supplier, Supplier_Name, Attn, Delivery, Delivery_Name FROM V_PRPOPendingUser Where UPPER(POType) = 'SERVICE' AND Supplier = " + QuotedStr(tbSuppCode.Text) + " and Report = " + QuotedStr(tbFgReport.Text)
                End If
                ResultField = "PO_No, Report"
            Else
                If ViewState("StateHd") = "Insert" Then
                    Session("filter") = "SELECT DISTINCT PO_No, PO_Date, Report, Supplier, Supplier_Name, Attn, Delivery, Delivery_Name FROM V_PRPOPendingUser Where UPPER(POType) = 'SERVICE' "
                Else
                    Session("filter") = "SELECT DISTINCT PO_No, PO_Date, Report, Supplier, Supplier_Name, Attn, Delivery, Delivery_Name FROM V_PRPOPendingUser Where UPPER(POType) = 'SERVICE' and Report = " + QuotedStr(tbFgReport.Text)
                End If
                ResultField = "PO_No, Supplier, Supplier_Name, Report"
            End If
            ViewState("Sender") = "btnPONo"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search PO No Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSupp_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSupp.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT Supplier_Code, Supplier_Name FROM VmsSupplier"
            ResultField = "Supplier_Code, Supplier_Name"
            ViewState("Sender") = "btnSupplier"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search PO No Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub lbSupplier_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSupplier.Click
        Try
            AttachScript("OpenMaster('" + Request.QueryString("KeyId") + "','MsSupplier')();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lb Supplier Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbSuppCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbSuppCode.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("supplier", tbSuppCode.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbSuppCode.Text = Dr("Supplier_Code")
                tbSuppName.Text = Dr("Supplier_Name")
                tbPONo.Text = ""
                tbFgReport.Text = ""
            Else
                tbSuppCode.Text = ""
                tbSuppName.Text = ""
                tbPONo.Text = ""
                tbFgReport.Text = ""
            End If
            AttachScript("setformatdt();", Page, Me.GetType())
            tbSuppCode.Focus()
        Catch ex As Exception
            Throw New Exception("tb Supplier Code Error : " + ex.ToString)
        End Try
    End Sub
    Private Function GetQty() As DataRow
        Dim dt As DataTable
        Dim dr As DataRow
        dt = SQLExecuteQuery("EXEC S_STRRPOGetQtyConvert '" + tbPONo.Text + "','" + tbProductCode.Text + "' , " + tbQtyOrder.Text.Replace(",", ""), ViewState("DBConnection").ToString).Tables(0)
        If dt.Rows.Count > 0 Then
            dr = dt.Rows(0)
            Return dr
        Else
            Return Nothing
        End If
    End Function

    'Private Function FindProduct(ByVal Nmbr As String, ByVal Product As String) As String
    '    Dim dr As SqlDataReader
    '    dr = SQLExecuteReader("EXEC S_STRRPOFindProd " + QuotedStr(tbPONo.Text) + ", " + QuotedStr(tbProductCode.Text), ViewState("DBConnection").ToString)
    '    dr.Read()
    '    Return dr("Product")
    'End Function

    Protected Sub ddlLocation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlLocation.SelectedIndexChanged
        ' ''If ViewState("InputLocation") = "Y" Then
        ' ''    RefreshMaster("S_GetWrhsLocation", "Location_Code", "Location_Name", ddlLocation, ViewState("DBConnection"))
        ' ''    ViewState("InputLocation") = Nothing
        ' ''End If
        ' ''Dim dr As DataRow
        ' ''dr = GetQty()
        ' ''tbQtyRR.Text = FormatNumber(dr("Qty"), ViewState("DigitQty"))
        ' ''tbQty.Text = FormatNumber(dr("QtyNet"), ViewState("DigitQty"))
        ' ''tbQty.Focus()
    End Sub

    Protected Sub tbQtyOrder_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbQtyOrder.TextChanged
        tbQtyOrder.Text = FormatFloat(tbQtyOrder.Text, ViewState("DigitQty"))
        Dim dr As DataRow
        dr = GetQty()
        'tbQtyRR.Text = FormatFloat(dr("Qty"), ViewState("DigitQty"))
        tbQty.Text = FormatFloat(dr("QtyNet"), ViewState("DigitQty"))
        tbRemarkDt.Focus()
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

    Protected Sub lbCount_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCount.Click
        Dim ResultField, CriteriaField, ResultSame As String
        Try
            Session("filter") = "SELECT PO_No, PO_Date, Report, Supplier, Supplier_Name, Attn, Delivery, Delivery_Name, Product, Product_Name, Specification, Qty_PO, Qty_Order, Unit_Order, Qty, Unit FROM V_PRPOPendingUser Where UPPER(POType) In ('SERVICE') "
            'ViewState("CheckDlg") = False

            ResultField = "PO_No, Report, Supplier, Supplier_Name, Product, Product_Name, Specification, Qty_PO, Qty_Order, Unit_Order, Qty, Unit"
            CriteriaField = "PO_No, Report, Supplier, Supplier_Name, Product, Product_Name, Specification, Qty_PO, Qty_Order, Unit_Order, Qty, Unit"

            ViewState("CriteriaField") = CriteriaField.Split(",")
            Session("Column") = ResultField.Split(",")
            ResultSame = "PO_No"
            Session("ResultSame") = ResultSame.Split(",")
            ViewState("Sender") = "btnOut"
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "lbCount_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbPONo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPONo.TextChanged
        'Dim Dr As DataRow
        'Dim DT As DataTable
        'Dim SQLString As String
        'Try
        '    If tbSuppCode.Text <> "" Then
        '        SQLString = "SELECT DISTINCT PO_No, PO_Date, Supplier, Supplier_Name, Product_Type, Product_Type_Name, Supplier_PO_No, Attn, Delivery_Type, Delivery, Delivery_Name, Product, Product_Name, Specification, Qty_Order, Unit_Order, Qty, Unit, FgQC FROM V_PRPOPending Where POType = 'PO' And PO_No = " + QuotedStr(tbPONo.Text) + " AND Supplier = " + QuotedStr(tbSuppCode.Text)
        '    Else
        '        SQLString = "SELECT DISTINCT PO_No, PO_Date, Supplier, Supplier_Name, Product_Type, Product_Type_Name, Supplier_PO_No, Attn, Delivery_Type, Delivery, Delivery_Name, Product, Product_Name, Specification, Qty_Order, Unit_Order, Qty, Unit, FgQC FROM V_PRPOPending Where POType = 'PO' And PO_No = " + QuotedStr(tbPONo.Text) + " "
        '    End If
        '    DT = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
        '    If DT.Rows.Count > 0 Then
        '        Dr = DT.Rows(0)
        '    Else
        '        Dr = Nothing
        '    End If
        '    If Not Dr Is Nothing Then
        '        BindToText(tbPONo, Dr("PO_No").ToString)
        '    End If
        '    ddlwrhs.Focus()
        'Catch ex As Exception
        '    Throw New Exception("tb Product Code TextChanged : " + ex.ToString)
        'End Try
    End Sub
End Class
