Imports System.Data
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO

Partial Class Transaction_TrSTRRPO_TrSTRRPO
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select Distinct TransNmbr, Nmbr, TransDate, Report, FgHome, Status, Supplier, Supplier_Name, Driver, SuppInvNo,  PONo, Warehouse, Wrhs_Name, SJSuppNo, SJSuppDate, CarNo, Remark, RRType, ShipTo, PPNNo, PPNDate, PPNRate, CurrCode, ForexRate, ShipToName, ForexRate  From V_STRRPOHd WHERE RRType = 'RR'"

    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT * From V_STRRPODt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim CurrFilter, Value As String
        Try
            If Not IsPostBack Then
                InitProperty()

                FillCombo(ddlwrhs, "EXEC S_GetWrhsUserRR " + QuotedStr(ViewState("UserId")), True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection"))

                FillCombo(ddlUnitPacking, "EXEC S_GetUnit", True, "Unit_Code", "Unit_Code", ViewState("DBConnection"))
                FillCombo(ddlUnitSisa, "EXEC S_GetUnit", True, "Unit_Code", "Unit_Code", ViewState("DBConnection"))

                SetInit()
                Dim dt As DataTable
                dt = SQLExecuteQuery("Select RptTitle, MenuName, COALESCE(MenuParam,'') AS MenuParam, COALESCE(FastRpt,'') AS FastRpt, COALESCE(ReportType,'') AS ReportType, COALESCE(ReportType2,'') AS ReportType2, COALESCE(FgPrintValue,'N') AS FgPrintValue, COALESCE(FgForceNewPage,'N') AS FgForceNewPage from MsMenu WHERE MenuId = " + QuotedStr(Request.QueryString("ContainerId").ToString), ViewState("DBConnection").ToString).Tables(0)
                lblTitle.Text = dt.Rows(0)("MenuName").ToString

                Session("AdvanceFilter") = ""
                ddlRow.SelectedValue = "10"
                lbCount.Text = SQLExecuteScalar("SELECT COUNT(PO_No) FROM V_PRPOOutReff ", ViewState("DBConnection").ToString)
                If Not Request.QueryString("transid") Is Nothing Then
                    If Request.QueryString("transid").ToString.Length > 1 Then
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
                        tbFgReport.Text = Session("Result")(1).ToString
                        tbfgHome.Text = Session("Result")(2).ToString.Replace("&nbsp;", "N")
                        If Session("Result")(2).ToString = "Service" Then
                            ddlwrhs.SelectedValue = ""
                        End If
                        tbPONo.Text = Session("Result")(0).ToString
                    Else
                        tbSuppCode.Text = Session("Result")(1).ToString
                        tbSuppName.Text = Session("Result")(2).ToString
                        tbFgReport.Text = Session("Result")(3).ToString
                        tbfgHome.Text = Session("Result")(4).ToString.Replace("&nbsp;", "N")
                        
                        tbPONo.Text = Session("Result")(0).ToString
                        tbCarNo.Enabled = True
                        If Session("Result")(4).ToString = "Service" Then
                            ddlwrhs.SelectedValue = ""
                        End If
                    End If

                End If


                If ViewState("Sender") = "btnProductPart" Then
                    tbProductPart.Text = Session("Result")(0).ToString
                    tbProductPartName.Text = Session("Result")(1).ToString
                End If
                
                If ViewState("Sender") = "btnSupplier" Then
                    tbSuppCode.Text = Session("Result")(0).ToString
                    tbSuppName.Text = Session("Result")(1).ToString

                    BindToText(tbFgReport, Session("Result")(3).ToString)
                    tbPONo.Text = ""

                End If
                If ViewState("Sender") = "btnShipTo" Then
                    tbShip.Text = Session("Result")(0).ToString
                    tbShipName.Text = Session("Result")(1).ToString
                End If

                If ViewState("Sender") = "btnProduct" Then
                    tbProductCode.Text = Session("Result")(0).ToString
                    tbProductName.Text = Session("Result")(1).ToString
                    BindToText(tbSpecification, TrimStr(Session("Result")(2).ToString))
                    BindToText(tbQtyPO, Session("Result")(3).ToString)
                    BindToText(tbQtyPacking, Session("Result")(4).ToString)
                    ddlUnitPacking.SelectedValue = Session("Result")(5).ToString
                    BindToDropList(ddlUnitSisa, Session("Result")(7).ToString)
                    'BindToText(tbFgReport, Session("Report").ToString)
                    'BindToText(tbShip, Session("Ship_To").ToString)
                    'BindToText(tbShipName, Session("Ship_To_Name").ToString)
                    'BindToDropList(ddlwrhs, Session("WrhsCode").ToString)
                    'ChangeCurrency(ddlCurr, tbDate, tbRate, ViewState("Currency"), ViewState("DigitCurr"), ViewState("DBConnection"), ViewState("DigitHome"))
                    'If ddlUnitPacking.SelectedValue = ddluni Then
                    '    tbQtyPO.Enabled = False
                    'Else
                    '    tbQtyPO.Enabled = True
                    'End If
                End If

                If ViewState("Sender") = "btnGetDt" Then
                    Dim drResult As DataRow
                    For Each drResult In Session("Result").Rows
                        'insert
                        Dim dr As DataRow
                        Dim Row As DataRow()
                        Row = ViewState("Dt").Select("ProductCode= " + QuotedStr(drResult("Product_Code")))
                        'Product_Code, Product_Name, Specification, Product_Part, Product_Part_Name, Have_Part, FgPackages, QtyWrhsTotal, UnitWrhs, QtyPack, QtyOrder, UnitOrder, Remark
                        If Row.Count = 0 Then
                            dr = ViewState("Dt").NewRow
                            dr("ProductCode") = drResult("Product_Code")
                            dr("Product_Name") = drResult("Product_Name")
                            dr("ProductPart") = drResult("Product_Part")
                            dr("ProductPart_Name") = drResult("Product_Part_Name")
                            'dr("Specification") = TrimStr(drResult("Specification"))
                            dr("QtyPO") = drResult("QtyWrhsTotal")
                            dr("Qty") = drResult("QtyWrhsTotal")
                            dr("Unit") = drResult("UnitWrhs")
                            dr("FgPackages") = drResult("FgPackages")
                            dr("HavePart") = drResult("Have_Part")
                            dr("FgQC") = drResult("Fg_QC")
                            dr("HaveQC") = drResult("Fg_QC")
                            dr("JmlPacking") = drResult("QtyPack")

                            dr("QtyPacking") = CFloat(dr("Qty")) / CFloat(dr("JmlPacking"))
                            dr("QtyPacking") = CInt(dr("QtyPacking"))

                            'dr("QtyPacking") = drResult("QtyOrder")
                            dr("UnitPacking") = drResult("UnitOrder")
                            'dr("UnitSisa") = drResult("UnitWrhs")

                            dr("QtySisa") = CFloat(dr("Qty")) - CFloat(dr("QtyPacking")) * CFloat(dr("JmlPacking"))

                            'If tbPONo.Text <> "" Then
                            '    dr("Remark") = drResult("Remark")
                            'End If
                            ViewState("Dt").Rows.Add(dr)
                        End If
                    Next
                    BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                End If
                ' ResultField = "PO_No, FgReport, Supplier_Code, Supplier_Name, FgHome, Ship_to, Ship_to_Name, Product_Code, Product_Name, Specification, Have_Part, Qty, Unit, FgPackages, Product_Part, Product_Part_Name, WrhsCode, QtyPacking, UnitPacking, UnitPack"

                If ViewState("Sender") = "btnOut" Then
                    BtnAdd_Click(Nothing, Nothing)
                    Dim drResult As DataRow
                    Dim FirstTime As Boolean = True
                    For Each drResult In Session("Result").Rows
                        'insert
                        If FirstTime Then
                            BindToText(tbSuppCode, drResult("Supplier_Code").ToString)
                            BindToText(tbSuppName, drResult("Supplier_Name").ToString)
                            BindToText(tbFgReport, drResult("FgReport").ToString)
                            BindToText(tbShip, drResult("Ship_To").ToString)
                            BindToText(tbShipName, drResult("Ship_To_Name").ToString)
                            'BindToDropList(ddlwrhs, drResult("WrhsCode").ToString)
                            tbSuppCode_TextChanged(Nothing, Nothing)
                            BindToText(tbPONo, drResult("PO_No").ToString)
                            BindToText(tbfgHome, drResult("FgHome").ToString)
                        End If

                        If CekExistData(ViewState("Dt"), "ProductCode", drResult("Product_Code")) = False Then
                            Dim dr As DataRow
                            dr = ViewState("Dt").NewRow
                            dr("ProductCode") = drResult("Product_Code")
                            dr("Product_Name") = drResult("Product_Name")
                            'dr("Specification") = TrimStr(drResult("Specification"))
                            dr("ProductPart") = drResult("Product_Part")
                            dr("ProductPart_Name") = drResult("Product_Part_Name")
                            dr("QtyPO") = drResult("QtyWrhsTotal")
                            dr("Qty") = drResult("QtyWrhsTotal")
                            dr("Specification") = drResult("Specification")
                            dr("Unit") = drResult("UnitWrhs")
                            dr("FgPackages") = drResult("FgPackages")
                            dr("HavePart") = drResult("Have_Part")
                            dr("FgQC") = drResult("Fg_QC")
                            dr("HaveQC") = drResult("Fg_QC")
                            dr("JmlPacking") = drResult("QtyPack")

                            dr("QtyPacking") = CFloat(dr("Qty")) / CFloat(dr("JmlPacking"))
                            dr("QtyPacking") = CInt(dr("QtyPacking"))

                            'dr("QtyPacking") = drResult("QtyOrder")
                            dr("UnitPacking") = drResult("UnitOrder")
                            ' dr("UnitSisa") = drResult("UnitWrhs")

                            dr("QtySisa") = CFloat(dr("Qty")) - CFloat(dr("QtyPacking")) * CFloat(dr("JmlPacking"))

                            dr("Remark") = ""
                            ViewState("Dt").Rows.Add(dr)
                        End If
                        FirstTime = False
                    Next
                    BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    ddlwrhs.Enabled = True
                End If
                Session("Result") = Nothing
                ViewState("Sender") = Nothing
                Session("filter") = Nothing
                Session("Column") = Nothing
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

                            Result = ExecSPCommandGo("Delete", "S_STRRPO", Nmbr(j), CInt(ViewState("GLYear")), CInt(ViewState("GLPeriod")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)

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
        ViewState("SortExpression") = Nothing
        ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
        FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))
        If ViewState("MenuLevel").Rows(0)("FgPrint") = "Y" Then
            ddlCommand.Items.Add("Print")
            ddlCommand2.Items.Add("Print")
        End If
        'Me.tbQtyPO.Attributes.Add("ReadOnly", "False")
        Me.tbSpecification.Attributes.Add("ReadOnly", "True")
        tbQtyPacking.Attributes.Add("OnKeyDown", "return PressNumeric();")
        tbQtyPO.Attributes.Add("OnKeyDown", "return PressNumeric();")
        ' Me.tbQtyPacking.Attributes.Add("OnBlur", "setformat('');")
        ' Me.tbQtyPO.Attributes.Add("OnBlur", "setformat('wrhs');")
    End Sub

    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim StrFilter, GetString As String
        Try
            GetString = GetStringHd
            StrFilter = GenerateFilterDlg(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue, ddlRange.SelectedValue)
            If AdvanceFilter.Length > 1 Then
                StrFilter = AdvanceFilter
            End If
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

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            GridView1.PageIndex = 0
            Session("AdvanceFilter") = ""
            GridView1.PageSize = ddlRow.SelectedValue
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

                For Each GVR In GridView1.Rows
                    CB = GVR.FindControl("cbSelect")
                    If CB.Checked Then
                        If GVR.Cells(3).Text = "P" Then
                            ListSelectNmbr = GVR.Cells(2).Text
                            If Pertamax Then
                                Result = "'''" + ListSelectNmbr + "''"
                                QCNo = GVR.Cells(7).Text.Replace("&nbsp;", "")
                                Pertamax = False
                            Else
                                Result = Result + ",''" + ListSelectNmbr + "''"
                            End If
                        End If
                    End If
                Next
                Result = Result + "'"
                Session("SelectCommand") = "EXEC S_STFormRRPO " + Result
                Session("ReportFile") = ".../../../Rpt/Form_RRPO.frx"
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
            ddlwrhs.Enabled = ddlwrhs.SelectedValue = "" Or GetCountRecord(ViewState("Dt")) = 0 'State ' And tbPONo.Text = ""
            btnPONo.Visible = State
            btnSupp.Visible = btnPONo.Visible
            tbSuppCode.Enabled = btnSupp.Visible
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
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt.Click
        Dim Row As DataRow
        Dim Result, SQLString As String
        Try
            SQLString = "Declare @A VarChar(255) EXEC S_STRRPOCekPOQty '" + tbPONo.Text + "', '" + tbProductCode.Text + "', " + tbQtyPacking.Text.Replace(",", "") + ", @A Out SELECT @A"
            Result = SQLExecuteScalar(SQLString, ViewState("DBConnection"))
            If Result.Length > 5 Then
                lbStatus.Text = Result
                Exit Sub
            End If
            If ViewState("StateDt") = "Edit" Then
                If ViewState("DtValue") <> tbProductCode.Text Then
                    If CekExistData(ViewState("Dt"), "ProductCode", tbProductCode.Text) Then
                        lbStatus.Text = MessageDlg("items has been already exist")
                        Exit Sub
                    End If
                End If
                Row = ViewState("Dt").Select("ProductCode = " + QuotedStr(ViewState("DtValue")))(0)
                If CekDt() = False Then
                    Exit Sub
                End If
                Row.BeginEdit()
                Row("ProductCode") = tbProductCode.Text
                Row("ProductPart") = tbProductPart.Text
                Row("Product_Name") = tbProductName.Text
                Row("Specification") = tbSpecification.Text
                Row("FgQC") = ddlHaveQC.SelectedValue
                Row("QtyPacking") = tbQtyPacking.Text
                Row("Qty") = tbQty.Text
                Row("QtySisa") = tbQtySisa.Text
                Row("QtyPO") = tbQtyPO.Text
                Row("UnitPacking") = ddlUnitPacking.SelectedValue
                Row("Unit") = tbUnit.Text
                Row("Remark") = tbRemarkDt.Text

                Row.EndEdit()
                'ViewState("Dt").AcceptChanges()
            Else
                'Insert
                If CekDt() = False Then
                    Exit Sub
                End If
                If CekExistData(ViewState("Dt"), "ProductCode", tbProductCode.Text) = True Then
                    lbStatus.Text = "Items has been already exist"
                    Exit Sub
                End If

                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("ProductCode") = tbProductCode.Text
                dr("Product_Name") = tbProductName.Text
                dr("ProductPart") = tbProductPart.Text
                dr("Specification") = tbSpecification.Text
                dr("FgQC") = ddlHaveQC.SelectedValue
                dr("QtyPacking") = tbQtyPacking.Text
                dr("JmlPacking") = tbQtyPacking.Text
                dr("Qty") = tbQty.Text
                dr("QtySisa") = tbQtySisa.Text
                dr("QtyPO") = tbQtyPO.Text
                dr("UnitPacking") = ddlUnitPacking.SelectedValue
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
            If Not (ViewState("StateHd") = "Insert" Or ViewState("StateHd") = "Edit") Then
                Exit Sub
            Else
                ViewState("StateSave") = ViewState("StateHd")
                ViewState("StateHd") = "View"
            End If

            'Save Hd
            If ViewState("StateSave") = "Insert" Then
                tbReference.Text = GetAutoNmbr("RRPO", tbFgReport.Text, Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)
                SQLString = "INSERT INTO STCRRPOHd (TransNmbr, Status, TransDate, FgReport, SuppCode, PONo, WrhsCode, FgHome, ShipTo, " + _
                " SJSupplierNo, SJSupplierDate, RRType, CarNo, Driver, Remark, PPnNo, PPnDate, PPnRate, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbReference.Text) + ", 'H', '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "', " + _
                QuotedStr(tbFgReport.Text) + ", " + QuotedStr(tbSuppCode.Text) + "," + QuotedStr(tbPONo.Text) + ", " + _
                QuotedStr(ddlwrhs.SelectedValue) + ", " + QuotedStr(tbfgHome.Text) + ", " + QuotedStr(tbShip.Text) + ", " + _
                QuotedStr(tbSJSuppNo.Text) + ", '" + Format(tbSJSuppDate.SelectedValue, "yyyy-MM-dd") + "','RR'," + _
                QuotedStr(tbCarNo.Text) + ", " + QuotedStr(tbDriver.Text) + "," + QuotedStr(tbRemark.Text) + "," + QuotedStr(tbPPnNo.Text) + ", '" + Format(tbPPnDate.SelectedValue, "yyyy-MM-dd") + "'," + _
                QuotedStr(tbPpnRate.Text.Replace(", ", "")) + "," + QuotedStr(ViewState("UserId").ToString) + ", getDate()"
            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM STCRRPOHd WHERE TransNmbr = " + QuotedStr(tbReference.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed... Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE STCRRPOHd SET TransDate = '" + Format(tbDate.SelectedValue, "yyyy-MM-dd") + "' " + _
                ", FgReport = " + QuotedStr(tbFgReport.Text) + ", SuppCode = " + QuotedStr(tbSuppCode.Text) + ",PONo = " + QuotedStr(tbPONo.Text) + _
                ", WrhsCode = " + QuotedStr(ddlwrhs.SelectedValue) + ", Driver = " + QuotedStr(tbDriver.Text) + ", FgHome = " + QuotedStr(tbfgHome.Text) + _
                ", SJSupplierNo = " + QuotedStr(tbSJSuppNo.Text) + ", SJSupplierDate = '" + Format(tbSJSuppDate.SelectedValue, "yyyy-MM-dd") + "'" + _
                ", Shipto =  " + QuotedStr(tbShip.Text) + ", PPNNo = " + QuotedStr(tbPPnNo.Text) + ", PPNRate = " + QuotedStr(tbPpnRate.Text) + ", PPNDate = '" + Format(tbPPnDate.SelectedValue, "yyyy-MM-dd") + "'" + _
                ", CarNo = " + QuotedStr(tbCarNo.Text) + " , Remark = " + QuotedStr(tbRemark.Text) + ", UserPrep = " + QuotedStr(ViewState("UserId").ToString) + ", DatePrep = getDate()" + _
                " WHERE TransNmbr = " + QuotedStr(tbReference.Text)
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
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, ProductCode, ProductPart, QtyPO, Qty, Unit, Remark, QtyPacking, UnitPacking, QtySisa, JmlPacking, FgQC,HaveQc  FROM STCRRPODt WHERE TransNmbr = '" & ViewState("TransNmbr") & "'", con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
                    "UPDATE STCRRPODt SET ProductCode = @ProductCode, ProductPart = @ProductPart, Qty = @Qty, Unit = @Unit, QtyPO = @QtyPO, " + _
                    " FgQC = @FgQC, HaveQc = @HaveQc, Remark = @Remark, QtyPacking = @QtyPacking, UnitPacking = @UnitPacking, QtySisa = @QtySisa, JmlPacking = @JmlPacking " + _
                    " WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND ProductCode = @OldProductCode ", con)
            ' Define output parameters.
            Update_Command.Parameters.Add("@ProductCode", SqlDbType.VarChar, 20, "ProductCode")
            Update_Command.Parameters.Add("@ProductPart", SqlDbType.VarChar, 20, "ProductPart")
            Update_Command.Parameters.Add("@FgQC", SqlDbType.VarChar, 1, "FgQC")
            Update_Command.Parameters.Add("@Qty", SqlDbType.Decimal, 18, "Qty")
            Update_Command.Parameters.Add("@Unit", SqlDbType.VarChar, 5, "Unit")
            Update_Command.Parameters.Add("@QtyPO", SqlDbType.Float, 18, "QtyPO")
            Update_Command.Parameters.Add("@QtyPacking", SqlDbType.Decimal, 18, "QtyPacking")
            Update_Command.Parameters.Add("@UnitPacking", SqlDbType.VarChar, 5, "UnitPacking")
            Update_Command.Parameters.Add("@JmlPacking", SqlDbType.Decimal, 18, "JmlPacking")
            Update_Command.Parameters.Add("@QtySisa", SqlDbType.Decimal, 18, "QtySisa")
            Update_Command.Parameters.Add("@HaveQc", SqlDbType.VarChar, 1, "HaveQc")
            Update_Command.Parameters.Add("@Remark", SqlDbType.VarChar, 255, "Remark")

            ' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@OldProductCode", SqlDbType.VarChar, 20, "ProductCode")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM STCRRPODt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND ProductCode = @ProductCode ", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@ProductCode", SqlDbType.VarChar, 20, "ProductCode")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

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

            pnlDt.Visible = True
            BindDataDt("")
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            btnPONo.Visible = True
            btnSupp.Visible = True
            tbReference.Text = ""
            tbPONo.Text = ""
            tbFgReport.Text = "N"
            tbfgHome.Text = "N"
            ddlwrhs.SelectedIndex = 0
            ddlwrhs.Enabled = True
            tbDriver.Text = ""
            tbCarNo.Text = ""
            tbSuppCode.Text = ""
            tbSuppName.Text = ""
            tbSJSuppNo.Text = ""
            tbCarNo.Text = ""
            tbRemark.Text = ""
            tbPPnNo.Text = ""
            tbPPnDate.Clear()
            tbPpnRate.Text = "0"
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbProductCode.Text = ""
            tbProductName.Text = ""
            tbProductPart.Text = ""
            tbProductPartName.Text = ""
            tbSpecification.Text = ""
            tbQtyPO.Text = "0"
            tbQty.Text = "0"
            tbUnit.Text = ""
            tbQtyPacking.Text = "0"
            ddlUnitPacking.SelectedValue = ""
            ddlUnitSisa.SelectedValue = ""
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
            Session("Filter") = "SELECT DISTINCT Product, Product_Name, Specification, dbo.FormatFloat(Qty_PO,dbo.DigitQty()) as Qty_PO, dbo.FormatFloat(Qty_Order,dbo.DigitQty()) As Qty_Packing, Unit_Order, dbo.FormatFloat(Qty,dbo.DigitQty()) As Qty, Unit, Remark FROM V_PRPOPendingDt WHERE PO_No = " + QuotedStr(tbPONo.Text)
            ResultField = "Product, Product_Name, Specification, Qty_PO, Qty_Packing,Unit_Order, Qty, Unit, Remark"
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
            SQLString = "SELECT Product AS Product_Code, Product_Name, Specification, dbo.FormatFloat(Qty_PO,dbo.DigitQty()) As Qty_PO, dbo.FormatFloat(Qty_Packing,dbo.DigitQty()) As Qty_Packing, Unit_Packing, dbo.FormatFloat(Qty,dbo.DigitQty()) As Qty, Unit FROM V_PRPOPendingDt WHERE PO_No = " + QuotedStr(tbPONo.Text) + " And Product = " + QuotedStr(tbProductCode.Text)
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
                BindToText(tbQty, Dr("Qty").ToString)
                BindToDropList(ddlUnitPacking, Dr("Unit_Packing").ToString)
                BindToDropList(ddlUnitSisa, Dr("Unit_Packing").ToString)
                tbQtyPacking.Text = FormatFloat(tbQtyPacking.Text, ViewState("DigitQty"))
                tbQtyPO.Text = FormatFloat(tbQtyPO.Text, ViewState("DigitQty"))
            Else
                tbProductCode.Text = ""
                tbProductName.Text = ""
                tbSpecification.Text = ""
                tbQtyPacking.Text = "0"
                ddlUnitPacking.SelectedValue = ""
                ddlUnitSisa.SelectedValue = ""
                tbQty.Text = "0"
                tbQtyPO.Text = "0"
                tbQtyPacking.Text = "0"
            End If
            'If ddlUnitPacking.SelectedValue = ddlUnitPacking.SelectedValue Then
            '    tbQtyPO.Enabled = False
            'Else
            '    tbQtyPO.Enabled = True
            'End If
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

            If tbSJSuppNo.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("SJ Supp No must have value")
                tbSJSuppNo.Focus()
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
                If Dr("ProductCode").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Product Must Have Value")
                    Return False
                End If
                If Dr("ProductPart").ToString.Trim = "" Then
                    'lbStatus.Text = MessageDlg("Product Part Must Have Value")
                    ' Dr("ProductPart").ToString.Trim = " "
                    ' Return False
                End If
                If CFloat(Dr("Qty").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty PO Must Have Value")
                    Return False
                End If
                If Dr("Unit").ToString.Trim = "" Then
                    lbStatus.Text = MessageDlg("Unit PO Must Have Value")
                    Return False
                End If

            Else
                If tbProductCode.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Product Must Have Value")
                    tbProductCode.Focus()
                    Return False
                End If
                If tbProductPart.Text.Trim = "" Then
                    ' tbProductPart.Text.Trim = " "
                    ' Return False
                End If
                If CFloat(tbQtyPO.Text) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty PO Must Have Value")
                    tbQtyPO.Focus()
                    Return False
                End If
                If ddlUnitPacking.SelectedValue.Trim = "" Then
                    lbStatus.Text = MessageDlg("Unit PO Must Have Value")
                    ddlUnitPacking.Focus()
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
            FilterName = "Reference, Date, Status, Product Code, Product Name, Supplier, Supplier Name, Remark"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate), Status, ProductCode, ProductName, Supplier, Supplier_Name, Remark"
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
                    CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                    If CekMenu <> "" Then
                        lbStatus.Text = CekMenu
                        Exit Sub
                    End If
                    Dim QCNo, StatusPrint As String
                    Try
                        QCNo = GVR.Cells(7).Text.Replace("&nbsp;", "")
                        StatusPrint = GVR.Cells(3).Text.Replace("&nbsp;", "")
                        Session("SelectCommand") = "EXEC S_STFormRRPO '''" + GVR.Cells(2).Text + "'''"
                        Session("ReportFile") = ".../../../Rpt/Form_RRPO.frx"
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

    Protected Sub GridView1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        Try
            If ViewState("SortPacking") = Nothing Or ViewState("SortPacking") = "DESC" Then
                ViewState("SortPacking") = "ASC"
            Else
                ViewState("SortPacking") = "DESC"
            End If
            ViewState("SortExpression") = e.SortExpression + " " + ViewState("SortPacking")
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
        Try
            Dim dr() As DataRow
            Dim lb, lb1 As Label
            Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
            dr = ViewState("Dt").Select("ProductCode = " + QuotedStr(GVR.Cells(1).Text))
            dr(0).Delete()
            BindGridDt(ViewState("Dt"), GridDt)
            EnableHd(Not DtExist())
        Catch ex As Exception
            lbStatus.Text = "GridDt_RowDeleting Error : " + ex.ToString
        End Try

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
            If ddlUnitPacking.SelectedValue = ddlUnitPacking.SelectedValue Then
                tbQtyPO.Enabled = False
            Else
                tbQtyPO.Enabled = True
            End If
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
            BindToText(tbPONo, Dt.Rows(0)("PONo").ToString)
            BindToText(tbfgHome, Dt.Rows(0)("FgHome").ToString)
            BindToText(tbSuppCode, Dt.Rows(0)("Supplier").ToString)
            BindToText(tbSuppName, Dt.Rows(0)("Supplier_Name").ToString)
            BindToText(tbShip, Dt.Rows(0)("ShipTo").ToString)
            BindToText(tbShipName, Dt.Rows(0)("ShipToName").ToString)
            BindToText(tbSJSuppNo, Dt.Rows(0)("SJSuppNo").ToString)
            BindToDate(tbSJSuppDate, Dt.Rows(0)("SJSuppDate").ToString)
            BindToText(tbCarNo, Dt.Rows(0)("CarNo").ToString)
            BindToText(tbDriver, Dt.Rows(0)("Driver").ToString)
            BindToText(tbPPnNo, Dt.Rows(0)("PPNNo").ToString)
            BindToDate(tbPPnDate, Dt.Rows(0)("PPNDate").ToString)
            BindToText(tbPpnRate, CFloat(Dt.Rows(0)("PPNRate").ToString))
            BindToDropList(ddlwrhs, Dt.Rows(0)("Warehouse").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal Product As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("ProductCode = " + QuotedStr(Product))
            If Dr.Length > 0 Then
                BindToText(tbProductCode, Dr(0)("ProductCode").ToString)
                BindToText(tbProductName, Dr(0)("Product_Name").ToString)
                BindToText(tbProductPart, Dr(0)("ProductPart").ToString)
                BindToText(tbProductPartName, Dr(0)("ProductPart_Name").ToString)
                BindToText(tbSpecification, Dr(0)("Specification").ToString)
                BindToText(tbQty, Dr(0)("Qty").ToString)
                BindToText(tbQtyPO, Dr(0)("QtyPO").ToString)
                BindToText(tbQtySisa, CFloat(Dr(0)("QtySisa").ToString))
                BindToText(tbJmlPacking, CFloat(Dr(0)("JmlPacking").ToString))
                BindToText(tbQtyPacking, CFloat(Dr(0)("QtyPacking").ToString))
                BindToText(tbUnit, Dr(0)("Unit").ToString)
                BindToDropList(ddlUnitPacking, Dr(0)("UnitPacking").ToString)
                BindToDropList(ddlUnitSisa, Dr(0)("UnitPacking").ToString)
                BindToDropList(ddlHaveQC, Dr(0)("HaveQC").ToString)
                
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

    Protected Sub btnProductPart_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnProductPart.Click
        Dim ResultField As String
        Try
            Session("filter") = "Select PartNo, PartName from MsProductPart" ' WHERE PartNo = " + QuotedStr(tbProductPart.Text)
            ResultField = "PartNo, PartName"
            ViewState("Sender") = "btnProductPart"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Cust Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbProductPart_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbProductPart.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("ProductPart", tbProductPart.Text + "|" + tbProductCode.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbProductPart.Text = Dr("PartNo")
                tbProductPartName.Text = Dr("PartName")
            Else
                tbProductPart.Text = ""
                tbProductPartName.Text = ""
            End If
            'AttachScript("setformatDT();", Page, Me.GetType())
            tbProductPart.Focus()
        Catch ex As Exception
            Throw New Exception("tb CustCode Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnPONo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPONo.Click
        Dim ResultField As String
        Try
            If tbSuppCode.Text <> "" Then
                If ViewState("StateHd") = "Insert" Then
                    Session("filter") = "SELECT DISTINCT PO_No, PO_Date, FgHome, Report, Supplier, Supplier_Name, Attn, Delivery, Delivery_Name, RemarkHd, POType FROM V_PRPOPendingDt Where Supplier = " + QuotedStr(tbSuppCode.Text)
                Else
                    Session("filter") = "SELECT DISTINCT PO_No, PO_Date, Report,FgHome, Supplier, Supplier_Name, Attn, Delivery, Delivery_Name, RemarkHd, POType FROM V_PRPOPendingDt Where Supplier = " + QuotedStr(tbSuppCode.Text) + " and Report = " + QuotedStr(tbFgReport.Text)
                End If
                ResultField = "PO_No, Report,FgHome, POType"
            Else
                If ViewState("StateHd") = "Insert" Then
                    Session("filter") = "SELECT DISTINCT PO_No, PO_Date, Report,FgHome, Supplier, Supplier_Name, Attn, Delivery, Delivery_Name, RemarkHd, POType FROM V_PRPOPendingDt "
                Else
                    Session("filter") = "SELECT DISTINCT PO_No, PO_Date, Report,FgHome, Supplier, Supplier_Name, Attn, Delivery, Delivery_Name, RemarkHd, POType FROM V_PRPOPendingDt Where Report = " + QuotedStr(tbFgReport.Text)
                End If
                ResultField = "PO_No, Supplier, Supplier_Name, Report,FgHome, POType"
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
            Session("filter") = "SELECT SuppLier_Code, SuppLier_Name, Currency, Term, Contact_Person, Phone, PPN FROM VMsSupplier WHERE Fgactive = 'Y'"
            ResultField = "SuppLier_Code, SuppLier_Name, Currency, PPN"
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
        Dim Dt As DataTable
        Dim SqlString As String
        Try
            '    Dr = FindMaster("SupplierIAL ", tbSuppCode.Text + ",'',''", ViewState("DBConnection").ToString)
            SqlString = "EXEC S_FindSupplierIAL " + QuotedStr(tbSuppCode.Text) + ", '', " + QuotedStr(Format(tbDate.SelectedValue, "yyyy/MM/dd"))
            Dt = SQLExecuteQuery(SqlString, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
            Else
                Dr = Nothing
            End If


            If Not Dr Is Nothing Then
                tbSuppCode.Text = Dr("SuppCode")
                tbSuppName.Text = Dr("SuppName")
                BindToText(tbFgReport, Dr("Reported"))
                tbPONo.Text = ""

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
        dt = SQLExecuteQuery("EXEC S_STRRPOGetQtyConvert '" + tbPONo.Text + "','" + tbProductCode.Text + "' , " + tbQtyPacking.Text.Replace(",", ""), ViewState("DBConnection").ToString).Tables(0)
        If dt.Rows.Count > 0 Then
            dr = dt.Rows(0)
            Return dr
        Else
            Return Nothing
        End If
    End Function


    Protected Sub tbQtyPacking_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbQtyPacking.TextChanged
        Dim dr As DataRow
        Try

            '  dr = GetQty()
            'If ddlUnitPacking.SelectedValue = ddlUnitPacking.SelectedValue Then
            '    tbQtyPO.Enabled = False
            'Else
            '    tbQtyPO.Enabled = True
            'End If

            tbQty.Text = CFloat(tbQtyPacking.Text) * CFloat(tbJmlPacking.Text)
            tbQtySisa.Text = "0"


            tbQtyPacking.Text = FormatFloat(tbQtyPacking.Text, ViewState("DigitQty"))
            '  tbQtyPO.Text = FormatFloat(dr("QtyWrhs"), ViewState("DigitQty"))
            tbQtyPacking.Focus()
        Catch ex As Exception
            lbStatus.Text = "tbQtyPacking_TextChanged Error : " + ex.ToString
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

    Protected Sub lbCount_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCount.Click
        Dim ResultField, CriteriaField, ResultSame As String
        Try 'SELECT PO_No, PO_Date, Report, Supplier, Supplier_Name, Attn, Delivery, Delivery_Name, Product, Product_Name, Specification, dbo.FormatFloat(Qty_PO,dbo.DigitQty()) As Qty_PO, dbo.FormatFloat(Qty_Packing,dbo.DigitQty()) As Qty_Packing, Unit_Packing, dbo.FormatFloat(Qty,dbo.DigitQty()) As Qty, Unit FROM V_PRPOPendingDt 
            Session("filter") = "EXEC S_STRRPOReff '',''"
            ResultField = "PO_No, FgReport, Supplier_Code, Supplier_Name, FgHome, Ship_to, Ship_to_Name, WrhsCode, Product_Code, Product_Name, Specification, Product_Part, Product_Part_Name, Have_Part, FgPackages, QtyWrhsTotal, UnitWrhs, QtyPack, QtyOrder, UnitOrder, Remark, Fg_QC"
            CriteriaField = "PO_No, FgReport, Supplier_Code, Supplier_Name, FgHome, Ship_to, Ship_to_Name, WrhsCode, Product_Code, Product_Name, Specification, Product_Part, Product_Part_Name, Have_Part, FgPackages, QtyWrhsTotal, UnitWrhs, QtyPack, QtyOrder, UnitOrder, Remark, Fg_QC"

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

    Protected Sub tbDate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbDate.SelectionChanged
        Try
            tbSJSuppDate.SelectedDate = tbDate.SelectedDate
        Catch ex As Exception
            lbStatus.Text = " tbDate_SelectionChanged Error :" + ex.ToString
        End Try
    End Sub

    Protected Sub btnGetDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGetDt.Click
        Dim ResultField As String 'ResultSame 
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            Session("Result") = Nothing
            Session("Filter") = "EXEC S_STRRPOReff  ' AND A.PO_No = '" + QuotedStr(tbPONo.Text) + "' AND A.Supplier_Code = '" + QuotedStr(tbSuppCode.Text) + "'', ''"
            ResultField = "Product_Code, Product_Name, Specification, Product_Part, Product_Part_Name, Have_Part, FgPackages, QtyWrhsTotal, UnitWrhs, QtyPack, QtyOrder, UnitOrder, Remark, Fg_QC"
            Session("Column") = ResultField.Split(",")
            ViewState("Sender") = "btnGetDt"
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn get Dt Error : " + ex.ToString
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

    
    Protected Sub btnShip_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShip.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT Ship_To, Ship_To_Name, Address1, Address2, Remark FROM V_PRPODeliveryOutHd WHERE PO_No = " + QuotedStr(tbPONo.Text)
            ResultField = "Ship_To, Ship_To_Name"
            ViewState("Sender") = "btnShipTo"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search PO No Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub tbQty_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbQty.TextChanged
        Try

            tbQtyPacking.Text = CFloat(tbQty.Text) / CFloat(tbJmlPacking.Text)
            tbQtyPacking.Text = Int(tbQtyPacking.Text)
            tbQtySisa.Text = CFloat(tbQty.Text) - (CFloat(tbQtyPacking.Text) * CFloat(tbJmlPacking.Text))

        Catch ex As Exception
            lbStatus.Text = "tbQty_TextChanged Error : " + ex.ToString
        End Try
    End Sub



End Class
